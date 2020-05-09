using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;

namespace ZHello.DesignPattern
{
    #region Create Pattern

    /// <summary>
    /// 单例模式
    /// </summary>
    public class SingletonPattern
    {
        public class SingleClass
        {
            //对于 深复制对象和反序列化对象 无此单一实例限制

            //key-1 类内部 has-a 该类静态未初始化实例 创建使用引用
            private static SingleClass _instance = null;

            private int timeStamp { get; set; }

            //key-2 私有构造函数 限制使用new创建路径
            private SingleClass()
            {
                timeStamp = Environment.TickCount;
            }

            public int GetCreateTimeStamp()
            {
                return timeStamp;
            }

            public static SingleClass GetInstance()
            {
                if (_instance == null)
                {
                    _instance = new SingleClass();
                }
                return _instance;
            }

            private static object _lock = new object();

            /// <summary>
            /// 线程安全的获取单例实例
            /// </summary>
            /// <returns></returns>
            public static SingleClass GetInstanceAsync()
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new SingleClass();
                        }
                    }
                }
                return _instance;
            }

            private static string _name = "[local|global]/" + Process.GetCurrentProcess().MainModule.FileName;
            private static Mutex _mutex;

            /// <summary>
            /// 进程安全获取唯一实例
            /// </summary>
            /// <returns></returns>
            public static SingleClass GetInstanceProcessAsync()
            {
                if (_name.Length > 260)
                {
                    _name = _name.Substring(_name.Length - 261);
                }
                try
                {
                    //MutexSecurity ms = new MutexSecurity(_name, System.Security.AccessControl.AccessControlSections.Access);
                    _mutex = new Mutex(true, _name, out bool res);
                    if (res)
                    {
                        _mutex.ReleaseMutex();
                        if (_instance == null)
                        {
                            lock (_lock)
                            {
                                if (_instance == null)
                                {
                                    _instance = new SingleClass();
                                }
                            }
                        }
                    }
                    else
                    {
                        if (Mutex.TryOpenExisting(_name, out _mutex))
                        {
                            //
                        }
                        else
                        {
                            //
                        }
                    }
                }
                catch (Exception e)
                {
                }
                return _instance;
            }
        }

        public static void Used()
        {
            //获取单一实例
            SingleClass singleClass = SingleClass.GetInstance();
            Console.WriteLine("Object Create Time:" + singleClass.GetCreateTimeStamp());

            SingleClass singleClass2 = SingleClass.GetInstance();
            Console.WriteLine("Object2 Create Time:" + singleClass2.GetCreateTimeStamp());

            SingleClass singleClass3 = SingleClass.GetInstance();
            Console.WriteLine("Object3 Create Time:" + singleClass3.GetCreateTimeStamp());
        }
    }

    /// <summary>
    /// 简单工厂模式
    /// 使用统一接口创建对象
    /// </summary>
    public class SimpleFactory
    {
        /// <summary>
        /// 抽象产品
        /// </summary>
        public abstract class Product
        {
            public abstract void Used();
        }

        /// <summary>
        /// 具体产品A
        /// </summary>
        public class Product1 : Product
        {
            public override void Used()
            {
                Console.WriteLine("Product1");
            }
        }

        /// <summary>
        /// 具体产品B
        /// </summary>
        public class Product2 : Product
        {
            public override void Used()
            {
                Console.WriteLine("Product2");
            }
        }

        /// <summary>
        /// 生成产品A和产品B的工厂
        /// </summary>
        public class Factory
        {
            public Product ProduceProduct(string str)
            {
                switch (str)
                {
                    case "A":
                        {
                            return new Product1();
                        }
                    case "B":
                        {
                            return new Product2();
                        }
                    default:
                        throw new Exception();
                }
            }
        }

        public static void Used()
        {
            //构建工厂
            Factory factory = new Factory();
            //生产产品
            Product prodA = factory.ProduceProduct("A");
            Product prodB = factory.ProduceProduct("B");
            //使用产品
            prodA.Used();
            prodB.Used();
        }
    }

    /// <summary>
    /// 抽象工厂模式
    /// 使用一个类统一规定对象创建类，创建对象
    /// </summary>
    public class AbstructFactory
    {
        /// <summary>
        /// 抽象产品
        /// </summary>
        public abstract class Product
        {
            public abstract void Used();
        }

        /// <summary>
        /// 具体产品A
        /// </summary>
        public class ProductA : Product
        {
            public override void Used()
            {
                Console.WriteLine("ProductA");
            }
        }

        /// <summary>
        /// 具体产品B
        /// </summary>
        public class ProductB : Product
        {
            public override void Used()
            {
                Console.WriteLine("ProductB");
            }
        }

        /// <summary>
        /// 抽象工厂
        /// </summary>
        public abstract class Factory
        {
            public abstract Product PorduceProduct();
        }

        /// <summary>
        /// 具体工厂A
        /// </summary>
        public class ProductAFactory : Factory
        {
            public override Product PorduceProduct()
            {
                return new ProductA();
            }
        }

        /// <summary>
        /// 具体工厂B
        /// </summary>
        public class ProductBFactory : Factory
        {
            public override Product PorduceProduct()
            {
                return new ProductB();
            }
        }

        /// <summary>
        /// 构建具体工厂
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static Factory BuildFactory(string factory)
        {
            switch (factory)
            {
                case "af":
                    return new ProductAFactory();

                case "bf":
                    return new ProductBFactory();

                default:
                    return null;
            }
        }

        public static void Used()
        {
            //构建工厂
            Factory aFactory = BuildFactory("af");
            Factory bFactory = BuildFactory("bf");
            //生产产品
            Product prodA = aFactory.PorduceProduct();
            Product prodB = bFactory.PorduceProduct();
            //使用产品
            prodA.Used();
            prodB.Used();
        }
    }

    /// <summary>
    /// 构造者模式
    /// 用于将一个类的 构建和表示 进行 分离
    /// </summary>
    public class BuilderParttern
    {
        /// <summary>
        /// 部件
        /// </summary>
        public abstract class Part
        {
            private string _name;

            public Part(string name)
            {
                _name = name;
            }

            public override string ToString()
            {
                return _name;
            }
        }

        public abstract class Product
        {
            protected IList<Part> Parts { get; set; } = new List<Part>();

            public abstract void AddPart(Part part);

            /// <summary>
            /// 构建产品
            /// </summary>
            public virtual void Used()
            {
                var sb = new StringBuilder();
                foreach (var item in Parts)
                {
                    sb.Append(item.ToString());
                }
                Console.WriteLine("Using Product:{0},Value:{1}", this.GetType().Name, sb.ToString());
            }
        }

        public class PartA : Part
        {
            public PartA(string name) : base(name)
            {
            }
        }

        public class PartB : Part
        {
            public PartB(string name) : base(name)
            {
            }
        }

        /// <summary>
        /// 产品
        /// </summary>
        public class ProductA : Product
        {
            /// <summary>
            /// 添加部件
            /// </summary>
            /// <param name="p"></param>
            public override void AddPart(Part p)
            {
                Parts.Add(p);
            }
        }

        /// <summary>
        /// 产品
        /// </summary>
        public class ProductB : Product
        {
            /// <summary>
            /// 添加部件
            /// </summary>
            /// <param name="p"></param>
            public override void AddPart(Part p)
            {
                Parts.Add(p);
            }
        }

        /// <summary>
        /// 抽象构建者
        /// </summary>
        public abstract class Builder
        {
            public abstract void ProductPart();

            /// <summary>
            /// 获取构建的产品
            /// </summary>
            /// <returns></returns>
            public abstract Product BuildProduct();
        }

        /// <summary>
        /// 具体构建者A
        /// </summary>
        public class Product1Bulder : Builder
        {
            private Product p = new ProductA();

            public override void ProductPart()
            {
                p.AddPart(new PartA("A1"));
                p.AddPart(new PartA("A2"));
                p.AddPart(new PartA("A3"));
            }

            public override Product BuildProduct()
            {
                return p;
            }
        }

        /// <summary>
        /// 具体构建者B
        /// </summary>
        public class Product2Bulder : Builder
        {
            private Product p = new ProductB();

            public override void ProductPart()
            {
                p.AddPart(new PartB("B1"));
                p.AddPart(new PartB("B2"));
                p.AddPart(new PartB("B3"));
            }

            public override Product BuildProduct()
            {
                return p;
            }
        }

        /// <summary>
        /// 指导者
        /// </summary>
        public class Director
        {
            /// <summary>
            /// 指导者根据构建者 构建产品 指定构建步骤
            /// </summary>
            /// <param name="builder"></param>
            /// <returns></returns>
            public Product Construct(Builder builder)
            {
                builder.ProductPart();
                return builder.BuildProduct();
            }
        }

        public static void Used()
        {
            //创建 指导者
            Director director = new Director();
            //创建 构建具体者
            Builder builder01 = new Product1Bulder();
            Builder builder02 = new Product2Bulder();
            //指导者根据构建者 构建 具体产品
            Product prodA = director.Construct(builder01);
            Product prodB = director.Construct(builder02);
            //使用产品
            prodA.Used();
            prodB.Used();
        }
    }

    /// <summary>
    /// 原型模式
    /// 创建重复对象
    /// </summary>
    public class PrototypePattern
    {
        public interface IClone
        {
            object Clone();
        }

        public abstract class Prototype : IClone
        {
            public abstract object Clone();
        }

        public class ConcretePrototype1 : Prototype
        {
            private bool isUseBinaryFormat = true;

            /// <summary>
            /// 对象深复制
            /// </summary>
            /// <returns></returns>
            public override object Clone()
            {
                if (this.GetType().IsSerializable)
                {
                    if (isUseBinaryFormat)
                    {
                        //序列化与反序列化方法 进行对象的深复制
                        //二进制序列化
                        IFormatter formatter = new BinaryFormatter();
                        Stream stream = new MemoryStream();
                        using (stream)
                        {
                            formatter.Serialize(stream, this);
                            stream.Seek(0, SeekOrigin.Begin);
                            return (Prototype)formatter.Deserialize(stream);
                        }
                    }
                    else
                    {
                        //XML序列化
                        System.Xml.Serialization.XmlSerializer formatter1 = new System.Xml.Serialization.XmlSerializer(this.GetType());
                        using (var mems = new System.IO.MemoryStream())
                        {
                            formatter1.Serialize(mems, this);
                            mems.Seek(0, System.IO.SeekOrigin.Begin);
                            return (Prototype)formatter1.Deserialize(mems);
                        }
                    }
                }
                else
                {
                    //使用反射实现
                    //效率不高
                    var t = this.GetType();
                    var obj = Activator.CreateInstance(t);
                    var properties = t.GetProperties();
                    for (int i = 0; i < properties.Length; i++)
                    {
                        var p = properties[i];
                        var value = p.GetValue(obj);
                        if (!value.GetType().IsClass)
                        {
                            p.SetValue(obj, p.GetValue(obj));//对于引用类型还需要进行再次的递归深复制
                        }
                        else
                        {
                        }
                    }
                    return (Prototype)obj;
                }
            }
        }

        public class ConcretePrototype2 : Prototype
        {
            public override object Clone()
            {
                return null;
            }
        }

        public static void Used()
        {
            var obj1 = new ConcretePrototype1();
            //需要大量相同对象时 可以使用深复制
            ConcretePrototype1[] objs = new ConcretePrototype1[100];
            for (int i = 0; i < objs.Length; i++)
            {
                objs[0] = (ConcretePrototype1)obj1.Clone();
            }
        }
    }

    #endregion Create Pattern
}