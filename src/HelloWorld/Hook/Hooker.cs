using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ZHello.Hook
{
    /*
     */
    public abstract class Hooker
    {

    }

    public class obj
    {
        public static void MethodAddr()
        {
            unsafe
            {
                //获取函数汇编调用地址
                MethodInfo m = null;
                var p = m.MethodHandle.GetFunctionPointer().ToPointer();
            }
        }
    }

    /// <summary>
    /// 
    /// https://bbs.csdn.net/topics/391958344
    /// </summary>
    public unsafe class InLineHooker
    {
        byte[] jmp_inst =
        {
            0x50,                                              //push rax
            0x48,0xB8,0x90,0x90,0x90,0x90,0x90,0x90,0x90,0x90, //mov rax,target_addr
            0x50,                                              //push rax
            0x48,0x8B,0x44,0x24,0x08,                          //mov rax,qword ptr ss:[rsp+8]
            0xC2,0x08,0x00                                     //ret 8
        };

        protected byte[] newInstrs = { 0xE9, 0x90, 0x90, 0x90, 0x90 }; //jmp target
        protected byte* rawMethodPtr;

        public void HookMethod(MethodBase rawMethod, MethodBase hookMethod, MethodBase originalMethod)
        {
            //确保jit过了
            var typeHandles = rawMethod.DeclaringType.GetGenericArguments().Select(t => t.TypeHandle).ToArray();
            RuntimeHelpers.PrepareMethod(rawMethod.MethodHandle, typeHandles);
            rawMethodPtr = (byte*)rawMethod.MethodHandle.GetFunctionPointer().ToPointer();
            var hookMethodPtr = (byte*)hookMethod.MethodHandle.GetFunctionPointer().ToPointer();
            //生成跳转指令，使用相对地址，用于跳转到用户定义函数
            fixed (byte* newInstrPtr = newInstrs)
            {
                *(uint*)(newInstrPtr + 1) = (uint)hookMethodPtr - (uint)rawMethodPtr - 5;
            }
            //将对占位函数的调用指向原函数，实现调用占位函数即调用原始函数的功能
            if (originalMethod != null)
            {
                if (Environment.Is64BitProcess)
                {
                    MakePlacholderMethodCallPointsToRawMethod_x64(originalMethod);
                }
                else
                {
                    MakePlacholderMethodCallPointsToRawMethod_x86(originalMethod);
                }
            }
            //并且将对原函数的调用指向跳转指令，以此实现将对原始目标函数的调用跳转到用户定义函数执行的目的
            uint oldProtect;
            //系统方法没有写权限，需要修改页属性
            NativeAPI.VirtualProtect((IntPtr)rawMethodPtr, 5, Protection.PAGE_EXECUTE_READWRITE, out oldProtect);
            for (int i = 0; i < newInstrs.Length; i++)
            {
                *(rawMethodPtr + i) = newInstrs[i];
            }
        }

        /// <summary>
        /// 将对originalMethod的调用指向原函数
        /// </summary>
        /// <param name="originalMethod"></param>
        protected void MakePlacholderMethodCallPointsToRawMethod_x86(MethodBase originalMethod)
        {
            uint oldProtect;
            var needSize = LDasm.SizeofMin5Byte(rawMethodPtr);
            var total_length = (int)needSize + 5;
            byte[] code = new byte[total_length];
            IntPtr ptr = Marshal.AllocHGlobal(total_length);
            //code[0] = 0xcc;//调试用
            for (int i = 0; i < needSize; i++)
            {
                code[i] = rawMethodPtr[i];
            }
            code[needSize] = 0xE9;
            fixed (byte* p = &code[needSize + 1])
            {
                *((uint*)p) = (uint)rawMethodPtr - (uint)ptr - 5;
            }
            Marshal.Copy(code, 0, ptr, total_length);
            NativeAPI.VirtualProtect(ptr, (uint)total_length, Protection.PAGE_EXECUTE_READWRITE, out oldProtect);
            RuntimeHelpers.PrepareMethod(originalMethod.MethodHandle);
            *((uint*)originalMethod.MethodHandle.Value.ToPointer() + 2) = (uint)ptr;
        }

        protected void MakePlacholderMethodCallPointsToRawMethod_x64(MethodBase method)
        {
            uint oldProtect;
            var needSize = LDasm.SizeofMin5Byte(rawMethodPtr);
            byte[] src_instr = new byte[needSize];
            for (int i = 0; i < needSize; i++)
            {
                src_instr[i] = rawMethodPtr[i];
            }
            fixed (byte* p = &jmp_inst[3])
            {
                *((ulong*)p) = (ulong)(rawMethodPtr + needSize);
            }
            var totalLength = src_instr.Length + jmp_inst.Length;
            IntPtr ptr = Marshal.AllocHGlobal(totalLength);
            Marshal.Copy(src_instr, 0, ptr, src_instr.Length);
            Marshal.Copy(jmp_inst, 0, ptr + src_instr.Length, jmp_inst.Length);
            NativeAPI.VirtualProtect(ptr, (uint)totalLength, Protection.PAGE_EXECUTE_READWRITE, out oldProtect);
            RuntimeHelpers.PrepareMethod(method.MethodHandle);
            *((ulong*)((uint*)method.MethodHandle.Value.ToPointer() + 2)) = (ulong)ptr;
        }

    }

    public enum Protection
    {
        PAGE_NOACCESS = 0x01,
        PAGE_READONLY = 0x02,
        PAGE_READWRITE = 0x04,
        PAGE_WRITECOPY = 0x08,
        PAGE_EXECUTE = 0x10,
        PAGE_EXECUTE_READ = 0x20,
        PAGE_EXECUTE_READWRITE = 0x40,
        PAGE_EXECUTE_WRITECOPY = 0x80,
        PAGE_GUARD = 0x100,
        PAGE_NOCACHE = 0x200,
        PAGE_WRITECOMBINE = 0x400
    }

    public class NativeAPI
    {
        [DllImport("kernel32")]
        public static extern bool VirtualProtect(IntPtr lpAddress, uint dwSize, Protection flNewProtect, out uint lpflOldProtect);
    }

}
