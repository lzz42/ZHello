using System;
using System.Collections.Generic;
using System.Linq;

namespace ZHello.DesignPattern
{
    #region Struct Pattern

    /// <summary>
    /// 适配器模式
    /// 转换已有资源为新资源使用
    /// 对象适配器：继承父类+组合代理器
    /// 类型适配器：继承父类+实现接口
    /// </summary>
    public class AdapterPattern
    {
        /// <summary>
        /// 适配者
        /// </summary>
        public class Adaptee
        {
            /// <summary>
            /// 要适配的方法
            /// </summary>
            public void SpecificFunc()
            {
                Console.WriteLine("Adaptee SpecificFunc()");
            }
        }

        /// <summary>
        /// 目标对象
        /// </summary>
        public class Target
        {
            public virtual void Func()
            {
            }
        }

        /// <summary>
        /// 目标接口
        /// </summary>
        public interface ITarget
        {
            void Func();
        }

        /// <summary>
        /// 适配对象的适配器
        /// </summary>
        public class Object_Adapter : Target
        {
            private Adaptee adaptee = new Adaptee();

            /// <summary>
            /// 在目标对象的实现方法中通过适配者的方法进行实现
            /// 完成转换从Adapter-》adaptee
            /// </summary>
            public override void Func()
            {
                adaptee.SpecificFunc();
            }
        }

        /// <summary>
        /// 适配接口的适配器
        /// </summary>
        public class Class_Adapter : Adaptee, ITarget
        {
            /// <summary>
            /// 通过父类的适配方法 实现目标适配接口
            /// </summary>
            public void Func()
            {
                base.SpecificFunc();
            }
        }

        public class User
        {
            public void UseNewObject(Target tObj)
            {
                tObj.Func();
            }

            public void UseNewInterface(ITarget tObj)
            {
                tObj.Func();
            }
        }

        public static void Used()
        {
            //即通过转换成指定的接口或者类，使用已有的类
            //目前只有Adapee类可以实现Spec操作
            //但需要Target 或者ITarget接口
            //Adapee未继承Target或者实现ITarget接口
            User user = new User();
            Target tObj = new Object_Adapter();
            tObj.Func();

            ITarget target = new Class_Adapter();
            target.Func();
            user.UseNewObject(tObj);
            user.UseNewInterface(target);
        }
    }

    /// <summary>
    /// 桥接模式
    /// 使用抽象代替变化的维度
    /// 抽象父类组合抽象实现
    /// 解决一个对象多个维度变化问题
    /// </summary>
    public class BridgePattern
    {
        public interface ISize
        {
        }

        public class Large : ISize { }

        public class Middle : ISize { }

        public class Small : ISize { }

        public interface IEngine { }

        public class LineEngine : IEngine { }

        public class VTypeEngine : IEngine { }

        public class WTypeEngine : IEngine { }

        public abstract class Producer { }

        public class JanpaneseProducer : Producer { }

        public class DomesticProducer : Producer { }

        public class AmericaProducer : Producer { }

        public class GermanyProducer : Producer { }

        public abstract class Car
        {
            public ISize Size { get; set; }
            public IEngine Engine { get; set; }
            public Producer Producer { get; set; }

            public abstract void Run();
        }

        public class XiaoKeCar : Car
        {
            public override void Run()
            {
            }
        }

        public class JeepCar : Car
        {
            public override void Run()
            {
            }
        }

        public class HavalCar : Car
        {
            public override void Run()
            {
                throw new NotImplementedException();
            }
        }

        public interface ICarConfig
        {
            ISize GetSize();

            IEngine GetEngine();

            Producer GetProducer();
        }

        public class XiaoKeCarConfig : ICarConfig
        {
            public IEngine GetEngine()
            {
                return new LineEngine();
            }

            public Producer GetProducer()
            {
                return new JanpaneseProducer();
            }

            public ISize GetSize()
            {
                return new Middle();
            }
        }

        public class JeepConfig : ICarConfig
        {
            public IEngine GetEngine()
            {
                return new VTypeEngine();
            }

            public Producer GetProducer()
            {
                return new AmericaProducer();
            }

            public ISize GetSize()
            {
                return new Large();
            }
        }

        public abstract class CarConfig
        {
            public abstract ISize Size { get; }
            public abstract IEngine Engine { get; }
            public abstract Producer Producer { get; }
        }

        public class HavalCarConfig : CarConfig
        {
            private ISize size = new Small();
            public override ISize Size { get { return size; } }
            private IEngine engine = new WTypeEngine();
            public override IEngine Engine { get { return engine; } }
            private Producer producer = new DomesticProducer();
            public override Producer Producer { get { return producer; } }
        }

        public class CarFactory
        {
            public Car ProduceCar<T>(ICarConfig config)
                where T : Car, new()
            {
                Car car = new T();
                car.Size = config.GetSize();
                car.Engine = config.GetEngine();
                car.Producer = config.GetProducer();
                return car;
            }

            public Car ProduceCar<T>(CarConfig config)
                where T : Car, new()
            {
                Car car = new T();
                car.Size = config.Size;
                car.Engine = config.Engine;
                car.Producer = config.Producer;
                return car;
            }
        }

        public static void Used()
        {
            var factory = new CarFactory();
            var xiaoke = factory.ProduceCar<XiaoKeCar>(new XiaoKeCarConfig());
            var jeep = factory.ProduceCar<JeepCar>(new JeepConfig());
            var haval = factory.ProduceCar<HavalCar>(new HavalCarConfig());
        }
    }

    /// <summary>
    /// 装饰者模式
    /// 为某个类添加新职责,解除了多层继承添加职责的复杂度，可以有选择的添加职责
    /// </summary>
    public class DecoratorPattern
    {
        public interface IRun
        {
            void Run();
        }

        public abstract class Car : IRun
        {
            public abstract void Run();
        }

        public class XiaoKeCar : Car
        {
            public override void Run()
            {
                Console.WriteLine("this car can run");
            }
        }

        public interface IDecorator : IRun { }

        public abstract class DecoratorBase : IDecorator
        {
            protected IRun mRun = null;

            public DecoratorBase(IRun run)
            {
                mRun = run;
            }

            public abstract void Run();
        }

        public class CarMusicDecorator : DecoratorBase
        {
            public CarMusicDecorator(IRun run) : base(run)
            {
            }

            public override void Run()
            {
                Console.WriteLine("This Car Can Play Music");
                base.mRun.Run();
            }
        }

        public class CarJumpDecorator : DecoratorBase
        {
            public CarJumpDecorator(IRun run) : base(run)
            {
            }

            public override void Run()
            {
                Console.WriteLine("This Car Can Jump");
                base.mRun.Run();
            }
        }

        public class DecoratorFactory
        {
            public static T BuilderPlay<T>(T play)
            {
                Dictionary<Type, List<IDecorator>> steps = new Dictionary<Type, List<IDecorator>>();
                List<IDecorator> list = steps[typeof(T)];
                foreach (IDecorator item in list)
                {
                    play = (T)Activator.CreateInstance(item.GetType(), play);
                }
                return play;
            }
        }

        public static void Used()
        {
            //创建最初的类
            IRun car = new XiaoKeCar();
            car.Run();
            //添加职责
            car = new CarMusicDecorator(car);
            car.Run();
            //添加职责
            car = new CarJumpDecorator(car);
            car.Run();
        }
    }

    /// <summary>
    /// 组合模式
    /// 将一系列对象组合成树结构来表示整天和部分之间的关系，访问组合对象和访问单个对象具有一致性
    /// </summary>
    public class CompositePattern
    {
        public abstract class Tag
        {
            public string ID { get; set; }

            public abstract void Add(Tag tag);

            public abstract void Del(Tag tag);

            public abstract void DoSomething();

            public Tag(string id)
            {
                ID = id;
            }
        }

        public class LeafTag : Tag
        {
            public LeafTag(string id) : base(id)
            {
            }

            public override void Add(Tag tag)
            {
                throw new NotImplementedException();
            }

            public override void Del(Tag tag)
            {
                throw new NotImplementedException();
            }

            public override void DoSomething()
            {
                Console.WriteLine(this.GetType().Name + " Do Something.");
            }
        }

        public class NormalTag : Tag
        {
            private List<Tag> tags { get; set; }

            public NormalTag(string id) : base(id)
            {
                tags = new List<Tag>();
            }

            public override void Add(Tag tag)
            {
                tags.Add(tag);
            }

            public override void Del(Tag tag)
            {
                tags.RemoveAll(t => t.ID == tag.ID);
            }

            public override void DoSomething()
            {
                Console.WriteLine(this.GetType().Name + " Do Something.");
                if (tags != null && tags.Count > 0)
                {
                    for (int i = 0; i < tags.Count; i++)
                    {
                        tags[i].DoSomething();
                    }
                }
            }
        }

        public class TagTree : Tag
        {
            private List<Tag> tags { get; set; }

            public TagTree(string id) : base(id)
            {
                tags = new List<Tag>();
            }

            public override void Add(Tag tag)
            {
                tags.Add(tag);
            }

            public override void Del(Tag tag)
            {
                tags.RemoveAll(t => t.ID == tag.ID);
            }

            public override void DoSomething()
            {
                Console.WriteLine(this.GetType().Name + " Do Something.");
                if (tags != null && tags.Count > 0)
                {
                    for (int i = 0; i < tags.Count; i++)
                    {
                        tags[i].DoSomething();
                    }
                }
            }
        }

        public static void Used()
        {
            //构建树节点
            var tagTree = new TagTree("tree01");
            //添加子节点
            tagTree.Add(new TagTree("subTree01"));
            tagTree.Add(new TagTree("subTree02"));
            tagTree.Add(new TagTree("subTree03"));
            tagTree.Add(new NormalTag("norTag01"));
            tagTree.Add(new NormalTag("norTag02"));
            tagTree.Add(new NormalTag("norTag03"));
            //调用树节点方法
            tagTree.DoSomething();
            //移除子节点
            tagTree.Del(new TagTree("subTree03"));
            //调用树节点方法
            tagTree.DoSomething();
        }
    }

    /// <summary>
    /// 外观模式
    /// 将复杂的细颗粒度对象服务包装为简易使用的粗颗粒度的功能服务
    /// </summary>
    public class FacadePattern
    {
        /// <summary>
        /// 发动机检测子系统
        /// </summary>
        public class EngineSystem
        {
            public bool CheckEngine()
            {
                Console.WriteLine("Check Engine.");
                return Environment.TickCount % 2 == 1;
            }
        }

        /// <summary>
        /// 刹车检测子系统
        /// </summary>
        public class BrokenSystem
        {
            public bool CheckBroken()
            {
                Console.WriteLine("Check Broken.");
                return Environment.TickCount % 2 == 1;
            }
        }

        /// <summary>
        /// 操控系统检测子系统
        /// </summary>
        public class ControlSystem
        {
            public bool CheckControl()
            {
                Console.WriteLine("Check Control.");
                return Environment.TickCount % 2 == 1;
            }
        }

        /// <summary>
        /// 汽车检测系统 - 总检测系统
        /// </summary>
        public class Facade_CarCheckSystem
        {
            private EngineSystem engineSystem = new EngineSystem();
            private BrokenSystem brokenSystem = new BrokenSystem();
            private ControlSystem controlSystem = new ControlSystem();

            public int CheckCarStatus()
            {
                var ee = engineSystem.CheckEngine() ? 1 : 0;
                var be = brokenSystem.CheckBroken() ? 1 : 0;
                var ce = controlSystem.CheckControl() ? 1 : 0;
                return ee & (be << 1) & (ce << 2);
            }

            public bool CheckEngine()
            {
                return engineSystem.CheckEngine();
            }

            public bool CheckBroken()
            {
                return brokenSystem.CheckBroken();
            }

            public bool CheckControl()
            {
                return controlSystem.CheckControl();
            }
        }

        public static void Used()
        {
            var car = new Facade_CarCheckSystem();
            var result = car.CheckCarStatus();
            Console.WriteLine("Car Check Restult:" + result);
        }
    }

    /// <summary>
    /// 享元模式
    /// 复用已存在的对象，降低系统创建对象实例性能消耗，减少内存消耗
    /// </summary>
    public class FlyweightPattern
    {
        //使用共享支持大量的细颗粒度对象
        public abstract class Tire
        {
            protected object TirePas { get; set; }
            public string Id { get; private set; }

            protected Tire(string id)
            {
                this.Id = id;
            }
        }

        public class TireA : Tire
        {
            public TireA() : base(typeof(TireA).Name)
            {
            }
        }

        public class TireB : Tire
        {
            public TireB() : base(typeof(TireB).Name)
            {
            }
        }

        public class TireFactory
        {
            private List<Tire> _tires = new List<Tire>();

            public Tire this[string key]
            {
                get
                {
                    Tire res = null;
                    var temp = _tires.ToList().Find(r => r.Id == key);
                    if (temp != null)
                    {
                        res = temp;
                    }
                    else
                    {
                        res = new TireA();
                        _tires.Add(res);
                    }
                    return res;
                }
            }

            public Tire CreateTire<T>()
                where T : Tire, new()
            {
                string id = typeof(T).Name;
                Tire tire = null;
                tire = new T();
                if (_tires.Exists(t => t.Id == id))
                {
                    tire = _tires.Find(t => t.Id == id);
                }
                else
                {
                    tire = new T();
                    _tires.Add(tire);
                }
                return tire;
            }
        }

        public static void Used()
        {
            var factory = new TireFactory();
            var tireA1 = factory.CreateTire<TireA>();
            var tireA2 = factory.CreateTire<TireA>();
            var tireB1 = factory.CreateTire<TireA>();
            var tireB2 = factory.CreateTire<TireA>();
        }
    }

    /// <summary>
    /// 代理模式
    /// 通过代理类访问目标对象，实现一层对象访问的隔离
    /// </summary>
    public class ProxyPattern
    {
        public interface IRemotingService
        {
            void RemotingCall();
        }

        public class RemotingService : IRemotingService
        {
            public RemotingService()
            {
            }

            public void RemotingCall()
            {
                Console.WriteLine(this.GetType().Name + " IRemotingSerice Call.");
            }
        }

        public class RemotingServiceProxy : IRemotingService
        {
            private RemotingService remotingServiceObject;

            public RemotingServiceProxy()
            {
                remotingServiceObject = new RemotingService();
            }

            public void RemotingCall()
            {
                Console.WriteLine(this.GetType().Name + " Proxy  IRemotingSerice Call.");
                remotingServiceObject.RemotingCall();
            }
        }

        public abstract class RemotingServiceObjectBase
        {
            public abstract void RemotingCall();
        }

        public class RemotingServiceObject : RemotingServiceObjectBase
        {
            public override void RemotingCall()
            {
                Console.WriteLine(this.GetType().Name + " RemotingServiceObjectBase Call.");
            }
        }

        public class RemotingServiceObjectProxy : RemotingServiceObjectBase
        {
            private RemotingServiceObject remotingServiceObject;

            public RemotingServiceObjectProxy()
            {
                remotingServiceObject = new RemotingServiceObject();
            }

            public override void RemotingCall()
            {
                Console.WriteLine(this.GetType().Name + " RemotingServiceObjectBase Call.");
                remotingServiceObject.RemotingCall();
            }
        }

        public static void Used()
        {
            //接口实现的代理
            IRemotingService serviceProxy = new RemotingServiceProxy();
            //调用代理方法
            serviceProxy.RemotingCall();
            //抽象类实现的代理
            RemotingServiceObjectBase objectProxy = new RemotingServiceObjectProxy();
            //调用代理方法
            objectProxy.RemotingCall();
        }
    }

    #endregion Struct Pattern
}