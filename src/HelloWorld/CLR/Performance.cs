/*
 C#性能优化
    装箱与拆箱 boxing and unbnoxing
    函数调用：
        C#中函数分为3类：
        1.非虚实例方法-普通方法
        2.虚方法
        3.静态方法
        方法构成：方法名+签名+返回值；
        方法的记录：每一个方法在程序集的方法定义表中都有一个记录项，每个记录项用一个标识flag指明方法的类型：实例方法，虚方法，静态方法；
        编译器根据每个方法的标识生成IL代码指令，即call和callvirt；
        call与callvirt区别：
        call：
        可调用静态方法（必须指定方法定义的类型），实例方法和虚方法（必须指定引用对象的变量，假定变量不为空）
        callvirt：
        可调用实例方法和虚方法，JIT会调查发出调用的对象的类型是否为null（即使调用非虚实例方法也要执行null检查）,
        即验证变量是否为null，若为空则抛出NullReferenceException，然后以多态方式调用；
        共同点：
        接收一个隐藏的this实参作为第一个参数，this实参引用要操作的对象；

        调用call性能比callvirt高，call不会进行调用变量的非空判断,JIT编译器不能内嵌（inline）虚方法；
        如何尽量让方法编译为IL后，JIT调用时，使用call而不是callvirt：
        1.尽量使用静态类中的静态方法 - 静态方法IL代码为call；
        2.尽量使用非虚方法，某些编译器会使用call调用非虚方法（C#编译器会使用callvirt调用所有的实例方法）；
        3.调用值类型中的方法，一般使用call；
        4.尽量使用sealed密封类，JIT使用非虚方式（call）调用该类中的虚方法（C#编译器生成callvirt指令，JIT会优化这个调用）；
    类型构造器
    Try...Catch..
    Demand和LinkDemand
     */

namespace ZHello.CLR
{
    public class TestClass
    {
        public static void Test()
        {
            ClassA.StaticFunc(); //call
            ClassB.StaticFunc(); //call
            var objA = new ClassA();
            objA.BaseNormalFunc();   //callvirt
            objA.BaseVirtualFunc();  //callvirt
            objA.SubNormalFunc();    //callvirt
            var objB = new ClassB();
            objB.BaseNormalFunc();   //callvirt
            objB.BaseVirtualFunc();  //callvirt
            objB.SubNormalFunc();    //callvirt
            objB.ToString();

            ValueType.StaticFunc();
            ValueType vt;
            vt.NormalFunc();
            ValueType vt2 = new ValueType();
            vt2.NormalFunc();
        }
    }

    public abstract class BaseClass
    {
        /// <summary>
        /// 基类虚方法
        /// </summary>
        public virtual void BaseVirtualFunc()
        {
        }

        /// <summary>
        /// 基类普通方法
        /// </summary>
        public void BaseNormalFunc()
        {
        }
    }

    public class ClassA : BaseClass
    {
        public static void StaticFunc()
        {
        }

        public void SubNormalFunc()
        {
        }

        public override void BaseVirtualFunc()
        {
            base.BaseVirtualFunc();
        }

        public new void BaseNormalFunc()
        {
            base.BaseNormalFunc();
        }
    }

    public sealed class ClassB : BaseClass
    {
        public static void StaticFunc()
        {
        }

        public void SubNormalFunc()
        {
        }

        public override void BaseVirtualFunc()
        {
        }

        public new void BaseNormalFunc()
        {
        }
    }

    public struct ValueType
    {
        public void NormalFunc()
        {
        }

        public static void StaticFunc()
        {
        }
    }
}