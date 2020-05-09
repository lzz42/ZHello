using System;
using ZHello.DesignPattern;
using static ZHello.DesignPattern.IteratorPattern;
using static ZHello.DesignPattern.MediatorPattern;
using static ZHello.DesignPattern.MementoPattern;
using static ZHello.DesignPattern.ObserverPattern;
using static ZHello.DesignPattern.StrategyPattern;
using static ZHello.DesignPattern.TemplateMethodPattern;
using static ZHello.DesignPattern.VisitorPattern;
using SContext = ZHello.DesignPattern.StrategyPattern.Context;
using TAbstraction = ZHello.DesignPattern.TemplateMethodPattern.Abstraction;

namespace ZHello
{
    public class PatternTest
    {
        public void Main()
        {
            //创建型 5
            SingletonPattern.Used();
            SimpleFactory.Used();
            AbstructFactory.Used();
            BuilderParttern.Used();
            PrototypePattern.Used();

            //结构型 7
            AdapterPattern.Used();
            BridgePattern.Used();
            DecoratorPattern.Used();
            CompositePattern.Used();
            FlyweightPattern.Used();
            FacadePattern.Used();
            ProxyPattern.Used();

            //行为型 11
            ChainOfResponsibilityPattern.Used();
            CommandPattern.Used();
            InterpreterPattern.Used();
            //特殊类型 熔断器模式
        }

        public static void Main_Iterator()
        {
            var agg1 = new ConcreteAggregate1();
            var agg2 = new ConcreteAggregate2();
            var it1 = agg1.CreateIterator();
            var it2 = agg2.CreateIterator();
            //TOOD:统一方法遍历it1、it2

            Console.ReadLine();
        }

        public static void Main_Mediator()
        {
            var mediator = new ConcreteMediator1();
            var coll1 = new ConcreteColleage1
            {
                Med = mediator
            };
            var coll2 = new ConcreteColleage2
            {
                Med = mediator
            };

            coll1.SendMsg(new Message() { Msg = "coll01" });
            coll2.SendMsg(new Message() { Msg = "coll02" });

            Console.ReadLine();
        }

        public static void Main_Memento()
        {
            var originator = new Originator();
            var taker = new CareTaker();

            taker.SetMemento(originator.SaveMemento());

            originator.RestoreMemento(taker.GetMemento());

            Console.ReadLine();
        }

        public static void Main_Observer()
        {
            var sub1 = new ConcreteSubject();

            var ob1 = new ConcreteObserver1();
            var ob2 = new ConcreteObserver1();

            //添加观察者
            sub1.Add(ob1);
            sub1.Add(ob2);

            sub1.Notify("New Msg");
            sub1.Renmove(ob2);

            Console.ReadLine();
        }

        public static void Main_State()
        {
        }

        public static void Main_Strategy()
        {
            var context = new SContext();
            //定义算法
            var cs1 = new ConcreteStrategy1();
            var cs2 = new ConcreteStrategy2();
            //
            context.SetStrategy(cs1);
            context.RunAlgorithm();
            context.SetStrategy(cs2);
            context.RunAlgorithm();

            Console.ReadLine();
        }

        public static void Main_TemplateMethodPattern()
        {
            TAbstraction cc1 = new ConcreteClass1();
            TAbstraction cc2 = new ConcreteClass2();
            cc1.TemplateMethod();
            cc2.TemplateMethod();

            Console.ReadLine();
        }

        public static void Main_Visitor()
        {
            var os = new ObjectStruct();
            os.AddElement(new ConcreteElement1());
            os.AddElement(new ConcreteElement1());

            var vis1 = new ConcreteVisotor1();
            os.Accept(vis1);

            Console.ReadLine();
        }
    }
}