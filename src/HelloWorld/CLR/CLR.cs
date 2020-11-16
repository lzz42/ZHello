using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

/*
 * CLR via C#
 */

//引用友元程序集
//[assembly: InternalsVisibleTo(assemblyName: "ThreadSync,Publickey=c624defc-4780-4435-ae11-c630a26c83ac",
//    AllInternalsVisible = true)]

//using strstr = System.String;
namespace ZHello.CLR
{
    public class ILTest
    {
        public string A()
        {
            string str = "a";
            str += "b";
            str += "c";
            return str;
        }

        public string B()
        {
            string str = "a" + "b" + "c";
            return str;
        }

        public string C()
        {
            string a = "a";
            string b = "b";
            string c = "c";
            string str = a + b + c;
            return str;
        }
    }

    public class CLR
    {
        public static void Main(string[] args)
        {
            //CompressMemory.TestRuntimeHandle();
            Console.ReadLine();
        }
    }

    public partial class CommonLanguageRuntime
    {
        public static void Func()
        {
            int s = 889;
            object o = s;
            int y = (int)s;
            Console.WriteLine(s.ToString() + "" + o);
            Console.WriteLine("{0},{1},{2}", s, s, s);
            object obj = s;
            Console.WriteLine("{0},{1},{2}", obj, obj, obj);
            Monitor.Enter(obj);
            s = 55;
            Monitor.Exit(obj);
        }

        #region 动态类型

        /// <summary>
        /// 动态类型
        /// </summary>
        internal class Employee : IDynamicMetaObjectProvider
        {
            public virtual string GetName() => "";

            public Int32 GetYear() => 20;

            public static dynamic s;

            public DynamicMetaObject GetMetaObject(Expression parameter)
            {
                throw new NotImplementedException();
            }

            public static dynamic Plus(dynamic d)
            {
                if (d is bool)
                {
                    return true;
                }
                if (d is string)
                {
                    var t = typeof(string);
                    var m = t.GetMethod("Contains", BindingFlags.IgnoreCase, null, new Type[] { typeof(string) }, null);
                    var res1 = m?.Invoke(d, new object[] { '5' });
                    var res2 = d.Contains('5');
                    Console.WriteLine("res1:{0},res2:{1}", res1, res2);
                }
                return d + d;
            }

            public static Employee FindEmployee(string name)
            {
                dynamic ddd = 90;
                var so = Plus(ddd);
                return new Manager();
            }

            public override bool Equals(object obj)
            {
                if (obj == null)
                {
                    return false;
                }
                if (Object.ReferenceEquals(this, obj))
                {
                    return true;
                }
                if (this.GetType() != obj.GetType())
                {
                    return false;
                }
                //compare value type field
                return true;
            }

            public override int GetHashCode()
            {
                var id1 = RuntimeHelpers.GetHashCode(this);
                var id2 = base.GetHashCode();
                Console.WriteLine("ID1:{0},ID2:{1}", id1, id2);
                return id1 - id2;
            }

            public DynamicMetaObject GetMetaObject(System.Linq.Expressions.Expression parameter)
            {
                throw new NotImplementedException();
            }
        }

        internal sealed class Manager : Employee
        {
            public override string GetName()
            {
                return "Manager::";
            }
        }

        public static void Func1()
        {
            Employee e;
            int year;
            checked
            {
                e = new Manager();
                e = Employee.FindEmployee("ok");
                year = e.GetYear();
            }
            unchecked
            {
                e.GetName();
                var sum = int.MaxValue + long.MaxValue;
                Console.WriteLine("sum:{0}", sum);
            }
            unchecked
            {
                var sum = int.MaxValue + long.MaxValue;
                Console.WriteLine("sum:{0}", sum);
            }
            checked
            {
                var d = new Decimal();
                d = Decimal.MaxValue;
                d += 1;
            }
        }

        #endregion 动态类型
    }

    /* CLR基础
    1.如何检测系统是否安装.NET Framework？
    　　检查%SystemRoot%\System32目录下是否存在文件MSCorEE.dll。
    2.如何确定已安装的.NET Framework版本？
    　　HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\NET Framework Setup\NDP
    3.关于x86和x64环境：
    　　若只有类型安全的托管代码，则在x86和x64都可以正常运行；
    　　若含有不安全代码，则需要制定平台和CPU架构；
    4.Windows x64平台为何能运行x86程序？
    　　win64提供了WoW64（windows on windows64）技术，该技术模拟x86指令集，但会影响性能；
        Wow64.dll：管理进程和线程的创建，勾住异常分发和Ntoskrnl.exe导出的基本系统调用。它也实现了文件重定向以及注册表重定向。
        Wow64Cpu.dll：位每个正在Wow64内部运行的线程，管理他们的32位的CPU环境，针对从32位到64位或者从64位到32位的CPU模式切换，提供了与处理器体系结构相关的支持。
        Wow64Win.dll：截取了Win32k.sys导出的GUI系统调用
        IA64系统上的IA32Exec.bin和Wow32ia32x.dll：包含了IA-32软件仿真器和它的接口库，因为Itanium处理器不能以原生方式高效地执行x86的32位指令，所以通过这两个额外的组件来实现软件仿真（通过二进制翻译）

    5.Windows运行Exe过程？
    　　a.检测Exe文件头，确定PE32还是PE32+，确定地址空间（32位还是64位或者WoW64），检查CPU架构信息；
    　　b.加载MSCorEE.dll对应版本(x86在C:\Windows\System32下，x64的x86版本在C:\Windows\SysWoW64下)；
    　　c.使用MSCorEE.dll中的方法初始化CLR，然后加载Exe程序，调用Main方法；
    Mark：使用非托管程序加载托管程序集，Windows会自动加载并初始化CLR，但由于进程已经运行，x86托管程序集无法完全加载到64位进程中。

    6.执行第一个方法过程：
    　　a.在Main函数执行前，CLR检测Main函数内代码引用的所有类型，CLR分配一个内部数据结构，管理所有引用类型的访问；
    　　b.在内部数据结构中的每个类型中定义的每个方法都有个一个对应记录项，该项的地址指向该方法的实现，即该项作为一个函数：JITCompiler；
    　　c.Main函数内首次调用方法时，该方法对应的JITCompiler函数被调用，该函数将IL代码编译成本地CPU指令，即该组件称为JITter或者JIT编译器；
    　　d.JITCompiler在程序集的元数据中查找被调用方法的IL代码，验证IL代码，将IL代码编译为本地CPU指令(CLR的JIT会对本地代码进行优化);
    　　e.CPU指令保存到动态分配的内存块中（程序终止该内存被释放），JITCompiler返回CLR类型创建的内部数据结构，找到被调用方法记录，修改JITCompiler记录项为执行CPU指令的内存块地址；
    　　f.JITCompiler跳转到内存块中的代码，执行（此处真正执行函数的CPU指令）并返回Main中；
    　　g.第二次调用函数时，会直接执行内存块中的代码，跳过JITCompiler函数，以后对该函数的调用都是本地代码方式进行；

     7.VisualStudio编辑并继续功能的由来：
    　　C#编译器使用/optimize-开关在生成的未优化代码中会包含许多NOP（no-operation，空操作）指令和分支指令，利用这些指令进行调试；优化代码则会删除这些多余指令；

    8.PDB（Program Database）文件：
    　　PDB文化帮助调试器查找局部变量并将IL代码映射到源代码；

    9.使用NGen.exe生成本地代码：
    　　将一个程序集的所有IL代码编译为本地代码，并保存到一个磁盘文件中，在运行时，加载该程序集前CLR会先判断是否存在这样的预先编译文件；NGen.exe对代码优化保守，不会对代码进行高度优化；

    10.IL和验证：

    　　IL基于栈，所有指令都要将操作数压栈push，结果从栈中弹出pop；IL指令是无类型的typeless；
    　　将IL代码编译为本地代码时，CLR执行验证过程verification，确定代码的安全；托管模块元数据包含验证所需函数和类型；

    11.健壮性和可靠性的区别：
    　　健壮性：robustness 描述系统对于参数变化的不敏感；可靠性：reliability 描述系统的可靠性，即提供固定的参数，产生稳定的、能预测的输出；

    12.PEVerify.exe工具：
    　　检查一个程序集的所有方法，并报告其中含有的不安全代码；使用CLR定位依赖的程序集，采用与CLR一样的绑定以及探测规则定位程序集；

    13.CLR支持的三种互操作情况：
    　　a.托管代码能调用DLL中的非托管代码,采用P/Invoke机制调用DLL中的函数；
    　　b.托管代码可以使用现有的COM组件或服务，可以创建一个托管程序集来描述COM组件，可以使用TlbImp.exe工具（将 COM 类型库中的类型定义转换为公共语言运行库程序集中的等效定义）
    　　c.非托管代码可以使用托管类型或服务，使用托管代码生成COM组件，TlbExp.exe、RegAsm.exe；

    14.MSCorLib.dll - 包含所有核心类型（C#编译器会自动引用该程序集）；

    15.响应文件 response file（.rsp）
    　　响应文件是一个包含一组编译器指令开关的文本文件。执行csc.exe时（编译源代码时），编译器打开响应文件使用其中的开关参数,使用@符号引用响应文件;
    可以指定多个rsp文件，csc.exe自动隐式查找两个csc.rsp文件：当前目录下；csc.exe目录下（全局rsp文件：%SystemRoot%\Microsoft.NET\Framework\v.XXXXX）；

    16.托管PE文件结构：

    　　托管PE文件构成：PE32（+）头、CLR头、元数据、IL代码；
            PE32（+）头：Windows要求的标准信息；
            CLR头：小的信息块（托管模块特有），包含CLR版本号、标志flag、一个MethodDef Token；可选的强名称数字签名；模块内部元数据表的大小和偏移量；
            元数据：二进制数据块，3个类别的多个表（源代码中所有定义都会在元数据中的某个表中创建一个记录项，可以使用ILDasm.exe查看元数据）：
            定义表（definition table）：ModuleDef、TypeDef、MethodDef、FieldDef、ParaDef、PropertyDef、EventDef等；
            引用表（reference table）：AssemblyRef、ModuleRef、TypeRef、MemberRef等；
            清单表（manifest table）：AssemblyDef、FileDef、ManifestDef、ExportedTypesDef；
        17.使用程序集连接器 al.exe

        18.强命名程序集
            a.为程序集分配强名称-程序集唯一标识：文件名；版本号；语言文化标识；公钥标记；
            Step-1:使用SN.exe获取一个密钥：
                创建包含二进制私钥和公钥的文件：sn -k <FilePath><Name>.snk；
                创建一个只包含公钥的文件（将snk公钥导出到指定文件）：sn -p <name>.snk <name>.PublicKey;
                显示公钥文件公钥信息：sn -tp <name>.PublicKey;
            Step-2:创建强名称程序集
                csc /keyfile:my.snk app.cs
            文件签名：
            1.生成程序集时，程序集中的FileDef清单元数据列出所有文件；
            2.每次将一个文件添加到清单中，文件内容进行哈希处理；
            3.哈希值和清单文件一起存储在FileDef表中，以此生成PE文件；
            4.对PE文件进行哈希处理，该哈希值使用发布者的私钥签名，得到RSA签名，并存储到PE文件；
            5.PE文件头进行更新，公钥嵌入AssemblyDef清单中；

        公钥标记为公钥哈希处理后的最后8个字节，可能会重复；

        19.GAC global assembly cache
            安装程序集（只能安装强名称）到GAC - GACUtil.exe
            */

    /*Chapter 4 类型基础
            20.CLR查找引用类型：
                a.同一文件 - 早期绑定；
                b.不同文件，同一程序集；
                c.不同文件，不同程序集；
            21.new操作符：
                a.计算类型以及所有基类型中定义的所有实例成员的字节数；
                b.从托管堆分配内存；
                c.初始化对象 类型对象指针和同步块索引；
                d.调用类型实例构造器；
            22.类型对象
                JIT将IL代码编译为本地代码时，CLR读取程序集中的元数据信息，然后在堆中为每个类型预定义的数据结构 - 类型对象；
                数据结构：
                额外的数据成员：
                类型对象指针 - 指向该实例的一个System.Type的一个实例引用，object.GetType()方法返回该引用；
                同步块索引；
                静态自动；
                方法表；
                类型对象都是System.Type的一个实例；
                System.Type实例的类型对象指针指向自身；
            23.方法执行
                a.CLR准备条件：方法内的所有类型在堆上已创建类型对象；
                b.序幕代码执行：在线程栈中为局部变量分配内存，自动将局部变量值初始化为null或者0；
                c.执行代码，创建引用对象,将该对象的内存地址返回到栈上的局部变量，该引用对象在堆内结构：类型对象指针、同步块索引、实例字段；
                d.方法回溯，若当前类型没有调用的方法，则JIT回溯类层次结构直到object，在沿途查找方法；
                f.虚实例方法调用：JIT在虚方法中添加额外代码，检查发出调用的变量，找到调用的对象，根据对象的类型对象指针获取实际类型，
                    在该类型的方法中查找要本地化的方法；
    */

    public class ValueClass
    {
        private struct Point
        {
            public float x, y;
        }

        public static void Func()
        {
            ArrayList a = new ArrayList();
            List<Point> ls = new List<Point>();
            Point p;
            for (int i = 0; i < 20; i++)
            {
                p.x = p.y = i;
                a.Add(p);//装箱
                ls.Add(p);//无装箱
            }
            for (int i = 0; i < a.Count; i++)
            {
                Point pp = (Point)a[i];//拆箱
                Point ppp = ls[i];//无拆箱
            }
            Point po;
            po.x = 11;
            po.y = 22;
            object o = po;
            po = (Point)o;
            po.x++;
            o = po;
        }

        public static void Func1()
        {
            int a = 32;
            object o = a;
            Console.WriteLine(a + "," + o);
            Console.WriteLine(a.ToString() + "," + o);

            var mvt = new MyValueType();
            mvt.GetHashCode();//不会装箱,方法内没有基类实现
            mvt.ToString();//会装箱，方法内有基类实现
            mvt.GetType();//会装箱，该方法由System.Object定义
            mvt.Clone();//不会装箱，值类型实现了接口
            var ic = mvt as ICloneable;//会装箱,转化为某个接口
            IComparable icc = mvt;//会装箱,转化为某个接口
        }
    }

    public static class ExternClass
    {
        public static bool ExternFunc(this MyValueType objs, object obj)
        {
            return false;
        }
    }

    public struct MyValueType : ICloneable, IComparable, IEquatable<MyValueType>
    {
        public object Clone()
        {
            return null;
        }

        public int CompareTo(MyValueType value)
        {
            return 0;
        }

        public int CompareTo(object obj)
        {
            return 0;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (this.GetType() != obj.GetType())
                return false;
            //比较所有字段
            return true;
        }

        public bool Equals(MyValueType obj)
        {
            //比较所有字段
            return true;
        }

        public static bool operator ==(MyValueType objl, MyValueType objr)
        {
            return false;
        }

        public static bool operator !=(MyValueType objl, MyValueType objr)
        {
            return false;
        }

        public override int GetHashCode()
        {
            var hc = RuntimeHelpers.GetHashCode(this);
            var hhc = System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(this);
            return 0;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }

    /*Chapter 5 基元类型 引用类型 值类型

        24.什么是基元类型
            编译器直接支持的数据类型，基元类型直接映射FCL中存在的类型
            string还是String？
            string是C#中的关键字直接映射到FCL类型 System.String
            无论x86或x64平台int都是映射到System.Int32
            不同编程语言对于同一个关键字可能映射不同的类型：eg：long

        对于结果进行截断处理，即向下取整;
        默认检查溢出关闭；
        IL指令：
        add 不执行溢出检查
        add.ovf 执行溢出检查，抛出异常

        26.System.Demical 特殊类型,CLR不视为基元类型
            CLR没有相应的IL指令对应，checked和unchecked对其无效；
            内部重载方法，调用其成员，因此运行速度慢；

        27.引用类型和值类型
            引用类型降低性能 总是在托管堆上分配对象 claas；

            1.从托管堆上分配内存；
            2.堆上分配的每个对象都有一些额外成员(类型对象指针，同步块索引)，这些成员必须初始化；
            3.对象中的其他字节总是设置为0；
            4.从托管堆上分配对象时，可能会强制执行一次垃圾收集操作；

            值类型 在线程栈上分配对象：enum struct ；
            枚举类型具有特殊性；

            继承关系：
            值类型--》System.ValueType--》System.Object
            枚举类型--》System.Enum--》System.ValueType--》System.Object

            所有值类型都是隐式密封的
            值类型的两种形式：未装箱unboxed、已装箱boxed
            值类型使用new操作符，编译器知道该类型的生成位置，new保证了值类型中的所有字段初始化为0
            声明为值类型：
            1.类型具有基元类型行为；（不可变类型immutable：一个类型没有提供会更改其字段的成员）
            2.不需要从其他类型继承；
            3.不需要派生其他类型；
            4.类型实例较小（小于等于16字节）或者 类型的实例较大（大于16字节），但不需要作为方法的实参传递或者方法返回；

            System.ValueType:该类型重写Equals方法和GetHashCode方法
            引用类型变量默认初始化为null
            值类型变量赋值：逐字段复制；
            引用类型变量赋值：复制内存地址；
            值类型实例内存回收是不会调用Finalize方法；

        28.控制类型中的字段布局 - 提高性能
            System.Runtime.InteropServices.StructLayoutAttribute
            C#编译器默认为引用类型使用LayoutKind.Auto,值类型：LayoutKind.Sequential
            指定每个字段的偏移量：LayoutKind.Explicit,字段使用属性：FiledOffset,指出该字段距离实例起始处偏移（单位字节）
            在同一类型中，一个引用类型和一个值类型互相重叠不合法；
            多个引用类型可以重叠，多个值类型可以重叠，所有重叠字节都可以通过公共字段访问；

        29.值类型：装箱和拆箱
            装箱IL指令:box
            装箱代价比拆箱代价高:

            接口变量必须是包含对堆上的一个对象的引用；
            调用虚方法时不会装箱；调用非虚方法时装箱；
            装箱boxing：
            a.在托管堆分配内存，内存大小：
                值类型所有字段所需内存 + 类型对象指针 + 同步块索引；
            b.值类型的字段复制到托管堆；
            c.返回托管堆中该对象的地址；
            拆箱unboxing：
            a.获取已装箱对象所有的字段的地址（不要求内存中复制任何字节）；
            将该值由托管堆复制到线程栈上，该过程属于字段复制操作，紧跟拆箱；
            拆箱异常：已装箱实例为Null---NullReferenceException，类型不一致---InvalidCastException；
            对一个对象进行拆箱时，只能将其转型为原先装箱时的类型，然后可进行类型转换；
        调用值类型的虚方法：
            值类型是隐式密封类，CLR可以非虚的调用值类型的重写方法，同时该值类型实例不会被装箱
            但值类型中重写的虚方法中在基类实现，该值类型实例会装箱；
            调用非虚的、继承的方法时，值类型一定会被装箱，这些方法由System.Object定义；
            将值类型的未装箱的实例转型为类型的某个接口时，会对实例进行装箱---接口变量必须包含对堆上的一个对象的引用；
            调用值类型实现的接口不会被装箱（在接口方法中没有基类实现），编译器可以直接调用；
            将值类型的所有字段设置为readonly；

        29.1.对象相等性和同一性
            a. bool System.Equals(object）方法：
                该方法实现对象的同一性identity比较；自己定义值类型应该重写该方法，ValueType类的Equals方法使用反射实现性能较慢；
                自定义Equals的四个特征：自反x.Equals(x)==true,对称x.Equals(y)==y.Equals(x),可传递x=y,y=z,则x=z,一致x=y,x=y;
                实现System.IEquatable<T>
                还应重载==和!=;
            b.静态方法：bool System.Object.ReferenceEquals(object,object):
                检查两个引用是否是引用同一个对象；
        29.2.对象的哈希码
            哈希码用途：System.Collections.Hashtable类型和System.Collections.Generic.Dictionary类型以及其他集合中，对比两个对象相等时，
            必须具有相等的哈希码；
            集合中添加key/value时，首先获取键的哈希码，该哈希码指出该键值对的存储位置（哈希桶中bucket），
            集合中查找键时，根据键的哈希码，搜索哈希桶，然后再查找与指定键相等的对象；
            计算类型的哈希码：
            1.FCL提供的方法：
                保证返回对象唯一性ID
                System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(object)
        29.3.dynamic基元类型
            编译器将dynamic类型转换为object类型，生成payload代码，payload代码使用运行时绑定器（runtime binder）的类；
            不用语言使用不同的运行时绑定器，c#再microsoft.csharp.dll（同样加载System.dll、System.Core.dll）中
 */

    /*Chapter 6 类型和成员

30.同步索引块-SyncBlockIndex
    a.线程同步 --- 用lock以及Monitor不能锁定值类型对象，无同步块索引
    SyncBlock[]
    SyncTable<*SyncBlock,*object>

    SyncBlock结构
    AwareLock
    PTR_IntropSyncBlockInfo
    SLink m_link:
    ADIndex
    DWord m_dwHashCode --存储对象哈希值

    CLR初始化时，构建一个SyncBlock数组；
    当一个线程进行Monitor.Enter(obj)时，或者lock开始，线程检查obj的同步索引块；
    若索引为空，即无同步块，从SyncBlock数组中选择一个空闲块赋值到该索引；
    若索引不为空，则等待；
    Monitor退出时，obj的同步块索引；
    b.存储特定数据
    总共32位，高6位为控制为，低26位根据高6位为确定存储值；
    c.即存储哈希值又作为lock对象；

31.重写Equals(object)方法
    自反：x.Equals(x)==true;
    对称：x.Equals(y)==y.Equals(x);
    可传递：
    一致：

32.友元程序集
    定义为internal的代码可以被指定的程序集访问
    使用System.Runtime.ComplierServices.InternalsVisibleTo特性定义在程序集上
    参数指定程序集名称和公钥

33.静态类
    静态类直接从基类System.Object继承；
    不实现任何接口；
    只能定义静态成员；
    不能作为字段、方法参数、局部变量；
34.分部类、结构、接口
    partial
    完全由C#编译器提供；
35.CLR调用虚方法（属性/事件）
    CLR编译方法时，在方法定义表中写入三个记录项，并用记录项的一组标识指明根据方法的类型：静态方法、实例方法、虚方法；
    call：可调用静态、实例、虚方法，调用方法时，必须指定所定义的类型或者实例对象；
    callvirt：只能调用实例、虚方法，会检查变量是否为null，执行比call慢；
    C#使用callvirt调用所有的实例方法（密封类、基类方法以及值类型特殊）；
    调用虚方法速度比非虚方法速度慢；
    JIT没有内嵌虚方法；
    sealed类
        36.

         */

    public class ConstantAndField
    {
        public const string CStr = "";
        public const decimal CD = 0.22M;
        public const ConstantAndField CCAF = null;
        public static int sa;
        public int ia;
        public readonly int ra;
        public static readonly int sra;
    }

    /* Chapter 7 常量和字段
     *
     * 7.1  常量
     *  常量值必须在编译时能确定，编译器将常量值保存在程序集元数据中（只能使用基元类型定义常量）
     *  基元类型：Boolean,Char,Byte,SByte,Int16,Int32,Int64,UInt16,UInt32,UInt64,Single,Double,Decimal,String
     *  或者非基元类型，值为null
     *  常量被视为静态成员，定义常量将创建元数据
     *  编译器会直接将常量值嵌入程序集的IL代码，不为常量分配内存，不能获取常量地址，不能传递引用方式传递
     *  所以运行时从一个程序集中提取另一个程序集中的常量，应该使用readonly字段
     *
     *  7.2 字段
     *  修饰词：
     *  static（static） Instance（默认）
     *  InitOnly（readonly） 只能由构造器方法中写入（可以用反射修改readonly值）
     *  Volatile（volatile） 不执行线程不安全的优化，只能使用以下类型：引用类型，值类型基元类型，基础类型为基元值类型的枚举类型
     *  字段何时分配内存？
     *  字段存储在动态内存中，仅能在运行时获取
     *  类型字段（静态字段） - 在类型对象中分配，类型对象在类型加载到第一个AppDomain时创建，
     *      加载时间：引用了该类型的任何方法时首次进行JIT编译时；
     *  实例字段    -   构造类型的一个实例时分配
     *
     *  内联初始化：代码中直接赋值来初始化
     *  当引用类型标记为readonly时，不可修改的是引用，并非字段引用的对象

     */

    /*  Chapter 8 方法
     *
     * 8.1 实例构造器和类 - 引用类型
     * 构造器 constructor
     * 方法定义元数据表：.ctor
     * 创建引用类型实例过程：
     *  1.为实例的数据字段分配内存（内存归0）；
     *  2.初始化对象的附加字段，即类型对象指针/同步块索引；
     *  3.调用类型的实例构造器；
     *
     * 若修饰符为abstract，则可访问性为protected
     * 若修饰符为static，编译器不会在类定义中生成默认构造器
     *
     * 特殊   -   可以在不调用类型构造器时创建一个类型实例
     * A.Object.MemberwiseClone()
     *  1.分配内存；
     *  2.初始化对象附加字段；
     *  3.将源对象的字节数复制到新对象中；
     * B.使用运行时反序列化器反序列话对象；
     *  反序列化使用：System.Runtime.Serialization.FormatterServices的
     *  GetUninitializedObject或者GetSafeUninitializedObject分配内存；
     *
     *

     */

    /*
    线程基础
    线程对CPU进行虚拟化
    线程开销
        组成：
        1、线程内核对象Thread kernel object 一个数据结构：线程属性描述；线程上下文（包含CPU寄存器集合，在x86上使用约700byte，x64约1240byte，IA64约2500byte）；
        2.线程环境块 thread environment block TEB，用户模式下的内存块，消耗一个内存页x86和x64为4K，IA64为8K
            包含：1.异常处理链首；2.线程本地数据；3.GDI和OpenGL的数据结构；
        3.用户模式栈 user-mode stack ：存储传递给方法的局部变量和实参，以及方法返回的地址，默认情况分配1MB内存，
        4.内核模式栈 kernel-mode stack：x86为12K，x64为24K
        5.DLL线程链接和线程分离通知 attach detach：在线程创建时，会调用进程中加载的所有DLL的DllMain方法，并传递标识DLL_THREAD_ATTACH和DLL_THREAD_DETACH
        6.上下文切换：将当期CPU寄存器值保存到当期线程内核对象的上下文结构；选择可调度的线程运行，若不同进程需要切换虚拟地址空间；将新线程上下文加载到CPU；windows每30ms进行一次上下文切换；
        7.执行垃圾回收，会挂起所有线程；

 */
}