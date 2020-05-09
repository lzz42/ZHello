using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZHello.DesignPattern
{
    #region Action Pattern

    /// <summary>
    /// 责任链模式
    /// 某个请求需要多个对象对其进行处理，避免请求发送者和接收之间的耦合关系
    /// 处理方式，将对象连城一个链，并沿着该链进行传递该请求，直到有对象处理为止
    /// 需要：
    /// 1.抽象请求处理者-定义处理请求的接口，内含自身的单向链表，包含下一个具体处理者引用
    /// 2.具体请求处理者-接收到请求后，进行判断处理，然后交给下一个处理者或者直接输出结果
    /// </summary>
    public class ChainOfResponsibilityPattern
    {
        /// <summary>
        /// 待处理材料 每种材料都有一个熔点
        /// 只有机器处理温度大于熔点时 才能处理该材料
        /// 尽可能使用处理温度较低的机器
        /// </summary>
        public abstract class Material
        {
            public float MeltingPoint { get; set; }

            public Material(float mltingPoint)
            {
                MeltingPoint = mltingPoint;
            }
        }

        public class Iron : Material
        {
            public Iron() : base(1538f)
            {
            }
        }

        public class Glass : Material
        {
            public Glass() : base(800f)
            {
            }
        }

        public class UnknownMaterial : Material
        {
            public UnknownMaterial(float meltingPoint) : base(meltingPoint)
            { }
        }

        public abstract class Machine
        {
            public float ProcessTemperature { get; set; }
            public Machine ParentMachine { get; set; }

            public Material Content { get; set; }

            public Machine(float pt)
            {
                ProcessTemperature = pt;
            }

            public abstract bool Process(Material something);
        }

        public class GlassMachine : Machine
        {
            public GlassMachine() : base(1000f)
            {
            }

            public override bool Process(Material something)
            {
                if (something.MeltingPoint < base.ProcessTemperature)
                {
                    Content = something;
                    return true;
                }
                else
                {
                    if (ParentMachine != null)
                    {
                        return ParentMachine.Process(something);
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public class IronMachine : Machine
        {
            public IronMachine() : base(1200f)
            {
            }

            public override bool Process(Material something)
            {
                if (something.MeltingPoint < base.ProcessTemperature)
                {
                    Content = something;
                    return true;
                }
                else
                {
                    if (ParentMachine != null)
                    {
                        return ParentMachine.Process(something);
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public class CopperMachine : Machine
        {
            public CopperMachine() : base(1083f)
            {
            }

            public override bool Process(Material something)
            {
                if (something.MeltingPoint < base.ProcessTemperature)
                {
                    Content = something;
                    return true;
                }
                else
                {
                    if (ParentMachine != null)
                    {
                        return ParentMachine.Process(something);
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public static void Used()
        {
            //构建 处理机器
            Machine glassMachine = new GlassMachine();
            Machine ironMachine = new IronMachine();
            Machine copperMachine = new CopperMachine();
            //构建 处理机器链
            glassMachine.ParentMachine = copperMachine;
            ironMachine.ParentMachine = ironMachine;
            copperMachine.ParentMachine = null;
            //构建 待处理材料
            Material iron = new Iron();
            Material glass = new Glass();
            Material uk = new UnknownMaterial(1234f);
            //开始 处理材料
            glassMachine.Process(iron);
            glassMachine.Process(glass);
            glassMachine.Process(uk);
        }
    }

    /// <summary>
    /// 命令模式
    /// 高内聚的模式
    /// 将一个请求封装成一个对象
    /// 解决命令的请求者和命令的实现者之间的耦合
    /// 需要：
    /// 1.抽象命令-cmd
    /// 2.抽象接收者-receiver 接收调用者请求执行操作的目标对象（命令参数主体）
    /// 3.抽象执行者-invoker 具体执行命令主体
    /// </summary>
    public class CommandPattern
    {
        public interface ICommand
        {
            Receiver Receiver { get; set; }

            bool Execute();

            bool Undo();
        }

        /// <summary>
        /// 抽象命令
        /// </summary>
        public abstract class CommandBase : ICommand
        {
            public string Cmd { get; set; }
            public Receiver Receiver { get; set; }

            public abstract bool Execute();

            public abstract bool Undo();
        }

        public class Add : CommandBase
        {
            public Add()
            {
                base.Cmd = "add";
            }

            public override bool Execute()
            {
                if (Receiver != null)
                {
                    Receiver.ShowInfo();
                }
                Console.WriteLine("Command Add Execute");
                return true;
            }

            public override bool Undo()
            {
                if (Receiver != null)
                {
                    Receiver.ShowInfo();
                }
                Console.WriteLine("Command Add Undo");
                return true;
            }
        }

        public class Del : CommandBase
        {
            public Del()
            {
                base.Cmd = "del";
            }

            public override bool Execute()
            {
                if (Receiver != null)
                {
                    Receiver.ShowInfo();
                }
                Console.WriteLine("Command Del Execute");
                return true;
            }

            public override bool Undo()
            {
                if (Receiver != null)
                {
                    Receiver.ShowInfo();
                }
                Console.WriteLine("Command Del Execute");
                return true;
            }
        }

        public abstract class Invoker
        {
            public abstract void AddCmd(CommandBase cmd);

            public abstract bool Invoke();

            public abstract bool RollBack();
        }

        public class RealInvoker : Invoker
        {
            private List<CommandBase> commands { get; set; }

            public RealInvoker()
            {
                commands = new List<CommandBase>();
            }

            public override bool Invoke()
            {
                if (commands != null && commands.Count > 0)
                {
                    commands.ForEach(c =>
                    {
                        c.Execute();
                    });
                }
                return true;
            }

            public override bool RollBack()
            {
                if (commands != null && commands.Count > 0)
                {
                    commands.ForEach(c =>
                    {
                        c.Undo();
                    });
                }
                return true;
            }

            public override void AddCmd(CommandBase cmd)
            {
                commands.Add(cmd);
            }
        }

        public abstract class Receiver
        {
            public abstract void ShowInfo();
        }

        public class SqlServerDB : Receiver
        {
            public override void ShowInfo()
            {
                Console.WriteLine("Receiver: " + this.GetType().FullName);
            }
        }

        public class OracleDB : Receiver
        {
            public override void ShowInfo()
            {
                Console.WriteLine("Receiver: " + this.GetType().FullName);
            }
        }

        public static void Used()
        {
            //构建 命令主体
            var add = new Add();
            var del = new Del();
            //设置命令执行参数主体
            add.Receiver = new SqlServerDB();
            del.Receiver = new OracleDB();
            //构建命令执行者
            var real = new RealInvoker();
            //将命令添加到执行者
            real.AddCmd(add);
            real.AddCmd(del);
            //执行命令
            real.Invoke();
            real.RollBack();
        }
    }

    /// <summary>
    /// 解释器模式
    /// 特定问题发生频率很高，可以将问题表述为一个实例
    /// 1.上下文：context
    /// 2.客户端：client
    /// 3.抽象表达式：AbstractExpression
    /// 4.终结符表达式：TerminalExpression
    /// 5.非终结符表达式：NonterminalExpression
    /// </summary>
    public class InterpreterPattern
    {
        /// <summary>
        /// 抽象表达式
        /// </summary>
        public abstract class Expression
        {
            public abstract bool HasValue();

            public abstract char GetNext();

            public abstract char GetCurrent();

            public abstract string Interpret(Context context);

            public abstract string DeInterpret(Context context);
        }

        public class MorseExpression : Expression
        {
            private string _input { get; set; }
            private Queue<char> _inputQueue { get; set; }

            public MorseExpression(string input)
            {
                _input = input;
                if (!string.IsNullOrEmpty(input))
                {
                    _inputQueue = new Queue<char>(_input.Length);
                    for (int i = 0; i < input.Length; i++)
                    {
                        _inputQueue.Enqueue(input[i]);
                    }
                }
                else
                {
                    _inputQueue = new Queue<char>();
                }
            }

            public override char GetCurrent()
            {
                if (HasValue())
                {
                    return _inputQueue.Peek();
                }
                else
                {
                    return '\n';
                }
            }

            public override char GetNext()
            {
                if (HasValue())
                {
                    return _inputQueue.Dequeue();
                }
                else
                {
                    return '\n';
                }
            }

            public override bool HasValue()
            {
                return _inputQueue.Count > 0;
            }

            public override string Interpret(Context context)
            {
                if (context == null)
                    return null;
                if (!HasValue())
                    return null;
                var builder = new StringBuilder();
                while (HasValue())
                {
                    var key = GetNext();
                    if (context.IsOperatorChar(key))
                    {
                        builder.Append(' ');
                    }
                    else
                    {
                        builder.Append(context.Transfer(key) + ' ');
                    }
                }
                return builder.ToString();
            }

            public override string DeInterpret(Context context)
            {
                throw new NotImplementedException();
            }
        }

        public abstract class Context
        {
            public abstract char DeTransfer(string key);

            public abstract string Transfer(char key);

            public abstract bool IsOperatorChar(char key);
        }

        public class MorseContext : Context
        {
            //Morse to ASCII
            private Dictionary<byte, byte> morseToASCII { get; set; }

            private Dictionary<string, char> morseToChar { get; set; }
            private List<char> operatorChar { get; set; }

            public MorseContext()
            {
                morseToChar = new Dictionary<string, char>()
                {
                    { ".----",'1'},
                    { "..---",'2'},
                    { "...--",'3'},
                    { "....-",'4'},
                    { ".....",'5'},
                    { "-....",'6'},
                    { "--...",'7'},
                    { "---..",'8'},
                    { "----.",'9'},
                    { "-----",'0'},

                    { ".-",'A'},
                    { "-...",'B'},
                    { "-.-.",'C'},
                    { "-..",'D'},
                    { ".",'E'},
                    { "..-.",'F'},
                    { "--.",'G'},
                    { "....",'H'},
                    { "..",'I'},
                    { ".---",'J'},
                    { "-.-",'K'},
                    { ".-..",'L'},
                    { "--",'M'},
                    { "-.",'N'},
                    { "---",'O'},
                    { ".--.",'P'},
                    { "--.-",'Q'},
                    { ".-.",'R'},
                    { "...",'S'},
                    { "-",'T'},
                    { "..-",'U'},
                    { "...-",'V'},
                    { ".--",'W'},
                    { "-..-",'X'},
                    { "-.--",'Y'},
                    { "--..",'Z'},
                };
                operatorChar = new List<char>() { ' ', ',', '.', '?', '!' };
            }

            public override char DeTransfer(string key)
            {
                if (!string.IsNullOrEmpty(key))
                {
                    if (morseToChar.ContainsKey(key))
                    {
                        return morseToChar[key];
                    }
                }
                return char.MinValue;
            }

            public override string Transfer(char key)
            {
                key = char.ToUpper(key);
                if (morseToChar.Values.Contains(key))
                {
                    foreach (var kvp in morseToChar)
                    {
                        if (kvp.Value == key)
                        {
                            return kvp.Key;
                        }
                    }
                }
                return null;
            }

            public override bool IsOperatorChar(char key)
            {
                key = char.ToUpper(key);
                return operatorChar.Contains(key);
            }
        }

        public abstract class Client
        {
            protected Context Context { get; set; }

            public Client(Context context)
            {
                Context = context;
            }

            public abstract string Transfer(string input);
        }

        public class MorseTransferClient : Client
        {
            public MorseTransferClient()
                : base(new MorseContext())
            {
            }

            public override string Transfer(string input)
            {
                var expression = new MorseExpression(input);
                return expression.Interpret(base.Context);
            }
        }

        public static void Used()
        {
            string input = "QSCGUK13579";
            Client client = new MorseTransferClient();
            string output = client.Transfer(input);
            System.Diagnostics.Trace.TraceInformation(string.Format("Input:{0};\nOutput:{1}", input, output));
        }
    }

    /// <summary>
    /// 迭代器模式
    /// </summary>
    public class IteratorPattern
    {
        public interface Iterator
        {
        }

        public class ConcreteIterator1 : Iterator
        {
            private IList<object> _list;
            private int _index = 0;

            public object Current
            {
                get
                {
                    return _list[_index];
                }
            }

            public bool MoveNext()
            {
                if (_list.Count >= _index + 1)
                {
                    ++_index;
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                _index = 0;
            }
        }

        public class ConcreteIterator2 : Iterator
        {
            private object[] _list;
            private int _index = 0;

            public object Current
            {
                get
                {
                    return _list[_index];
                }
            }

            public bool MoveNext()
            {
                if (_list.Length >= _index + 1)
                {
                    ++_index;
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                _index = 0;
            }
        }

        public abstract class Aggregate
        {
            public abstract Iterator CreateIterator();
        }

        public class ConcreteAggregate1 : Aggregate
        {
            public override Iterator CreateIterator()
            {
                return new ConcreteIterator1();
            }
        }

        public class ConcreteAggregate2 : Aggregate
        {
            public override Iterator CreateIterator()
            {
                return new ConcreteIterator2();
            }
        }
    }

    /// <summary>
    /// 中介者模式
    /// </summary>
    public class MediatorPattern
    {
        public abstract class Mediator
        {
            public IList<Colleage> colls = new List<Colleage>();

            public abstract void Contact(Message msg, Colleage coll);
        }

        public class Message
        {
            public string Msg { get; set; }

            public override string ToString()
            {
                return Msg;
            }
        }

        public class ConcreteMediator1 : Mediator
        {
            public override void Contact(Message msg, Colleage coll)
            {
                //TODO:get target colleage and call receive function
                colls.ToList().ForEach(c => { if (c != coll) { c.ReceiveMsg(msg); } });
            }
        }

        public abstract class Colleage
        {
            private Mediator _med;
            public Mediator Med { get { return _med; } set { value.colls.Add(this); _med = value; } }
            public string ID { get; set; }

            public abstract void ReceiveMsg(Message msg);

            public virtual void SendMsg(Message msg)
            {
                Med.Contact(msg, this);
            }
        }

        public class ConcreteColleage1 : Colleage
        {
            public override void ReceiveMsg(Message msg)
            {
                Console.WriteLine(this.GetType().ToString() + "::" + msg);
            }

            public override void SendMsg(Message msg)
            {
                base.SendMsg(msg);
            }
        }

        public class ConcreteColleage2 : Colleage
        {
            public override void ReceiveMsg(Message msg)
            {
                Console.WriteLine(this.GetType().ToString() + "::" + msg);
            }

            public override void SendMsg(Message msg)
            {
                base.SendMsg(msg);
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class MementoPattern
    {
        public class Memento
        {
            private string _state { get; set; }

            public void SetState(string state)
            {
                _state = state;
            }

            public string GetState()
            {
                return _state;
            }
        }

        public class CareTaker
        {
            private Memento _memen { get; set; }

            public Memento GetMemento()
            {
                return _memen;
            }

            public void SetMemento(Memento memento)
            {
                _memen = memento;
            }
        }

        public class Originator
        {
            public Memento SaveMemento()
            {
                return new Memento();
            }

            public void RestoreMemento(Memento memento)
            {
            }
        }
    }

    /// <summary>
    /// 观察者模式
    /// </summary>
    public class ObserverPattern
    {
        public interface IObserver
        {
            void Update(string msg);
        }

        public class ConcreteObserver1 : IObserver
        {
            public void Update(string msg)
            {
                throw new NotImplementedException();
            }
        }

        public class ConcreteObserver2 : IObserver
        {
            public void Update(string msg)
            {
                throw new NotImplementedException();
            }
        }

        public interface ISubject
        {
            IList<IObserver> Observers { get; set; }

            void Add(IObserver ser);

            void Renmove(IObserver ser);

            void Notify(string msg);
        }

        public class ConcreteSubject : ISubject
        {
            public IList<IObserver> Observers { get; set; } = new List<IObserver>();

            public void Add(IObserver ser)
            {
                Observers.Add(ser);
            }

            public void Notify(string msg)
            {
                Observers.ToList().ForEach(s => s.Update(msg));
            }

            public void Renmove(IObserver ser)
            {
                Observers.Remove(ser);
            }
        }
    }

    /// <summary>
    /// 状态模式
    /// </summary>
    public class StatePattern
    {
        public class Context
        {
            private State _state;

            public void SetState(State state)
            {
                //set state change and set action
                state.Handle();
            }
        }

        public abstract class State
        {
            public abstract void Handle();
        }

        public class ConcreteState1 : State
        {
            public override void Handle()
            {
                throw new NotImplementedException();
            }
        }

        public class ConcreteState2 : State
        {
            public override void Handle()
            {
                throw new NotImplementedException();
            }
        }
    }

    /// <summary>
    /// 策略模式
    /// </summary>
    public class StrategyPattern
    {
        public class Context
        {
            private Strategy _strategy;

            public void SetStrategy(Strategy st)
            {
                _strategy = st;
            }

            public void RunAlgorithm()
            {
                _strategy?.Algorithm();
            }
        }

        public abstract class Strategy
        {
#pragma warning disable CS3008 // 标识符不符合 CLS
            protected IList<IComparable> _array;
#pragma warning restore CS3008 // 标识符不符合 CLS

            public int CalculateDegree()
            {
                return 0;
            }

            public abstract void Algorithm();
        }

        public class ConcreteStrategy1 : Strategy
        {
            /// <summary>
            /// bubble sort
            /// </summary>
            public override void Algorithm()
            {
                for (int i = 0; i < _array.Count; i++)
                {
                    for (int j = i + 1; j < _array.Count; j++)
                    {
                        if (_array[i].CompareTo(_array[j]) > 0)
                        {
                            var temp = _array[j];
                            _array[j] = _array[i];
                            _array[i] = temp;
                        }
                    }
                }
            }
        }

        public class ConcreteStrategy2 : Strategy
        {
            /// <summary>
            /// quick sort
            /// </summary>
            public override void Algorithm()
            {
                Sort_q(_array, 0, _array.Count - 1);
            }

            private void Sort_q(IList<IComparable> rest, int left, int right)
            {
                if (left < right)
                {
                    //找到中间元素为中间值
                    var middle = rest[(left + right) / 2];
                    int i = left - 1,
                        j = right + 1;
                    while (true)
                    {
                        //找到比middle小的元素
                        while (rest[++i].CompareTo(middle) < 0 && i < right) ;
                        //找到比middle大的元素
                        while (rest[--j].CompareTo(middle) > 0 && j > 0) ;
                        //若越界则退出循环
                        if (i >= j)
                            break;
                        //交互元素
                        var num = rest[i];
                        rest[j] = rest[j];
                        rest[j] = num;
                    }
                    //迭代排序
                    Sort_q(rest, left, i - 1);
                    Sort_q(rest, j + 1, right);
                }
            }
        }

        public class ConcreteStrategy3 : Strategy
        {
            /// <summary>
            /// insert sort
            /// </summary>
            public override void Algorithm()
            {
                for (int i = 1; i < _array.Count; i++)
                {
                    int j;
                    var temp = _array[i];//index i
                    for (j = i; j > 0; j--)//遍历i之前元素
                    {
                        if (_array[j - 1].CompareTo(temp) > 0)//比较当前元素与之前元素
                        {
                            _array[j] = _array[j - 1];
                        }
                        else
                        {
                            break;
                        }
                    }
                    _array[j] = temp;
                }
            }
        }

        public class ConcreteStrategy4 : Strategy
        {
            /// <summary>
            /// select sort
            /// </summary>
            public override void Algorithm()
            {
                IComparable temp;
                for (int i = 0; i < _array.Count; i++)
                {
                    temp = _array[i];//index i
                    int j;
                    int select = i;
                    for (j = i + 1; j < _array.Count; j++)
                    {
                        if (_array[j].CompareTo(temp) < 0)
                        {
                            temp = _array[j];
                            select = j;
                        }
                    }
                    _array[select] = _array[i];
                    _array[i] = temp;
                }
            }
        }
    }

    /// <summary>
    /// 模板模式
    /// 定义一个模板类，预留使用的每个抽象实现
    /// 子类实现每个抽象实现
    /// </summary>
    public class TemplateMethodPattern
    {
        public abstract class Abstraction
        {
            public void TemplateMethod()
            {
                //Console.WriteLine(this.GetType().ToString() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name);
                Func1();
                Func2();
                Func3();
            }

            protected abstract void Func1();

            protected abstract void Func2();

            protected abstract void Func3();
        }

        public class ConcreteClass1 : Abstraction
        {
            protected override void Func1()
            {
                //Console.WriteLine(this.GetType().ToString() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

            protected override void Func2()
            {
                //Console.WriteLine(this.GetType().ToString() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

            protected override void Func3()
            {
                //Console.WriteLine(this.GetType().ToString() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        public class ConcreteClass2 : Abstraction
        {
            protected override void Func1()
            {
                //Console.WriteLine(this.GetType().ToString() + "::" + System.Reflection.MethodInfo.GetCurrentMethod().Name);
            }

            protected override void Func2()
            {
                //Console.WriteLine(this.GetType().ToString() + "::" + System.Reflection.MethodInfo.GetCurrentMethod().Name);
            }

            protected override void Func3()
            {
                //Console.WriteLine(this.GetType().ToString() + "::" + System.Reflection.MethodInfo.GetCurrentMethod().Name);
            }
        }
    }

    /// <summary>
    /// 访问者模式
    /// 封装某些作用于某种数据结构的操作
    /// 用于在不改变数据结构的情况下定义作用于元素的操作
    /// </summary>
    public class VisitorPattern
    {
        /// <summary>
        /// 抽象访问者
        /// </summary>
        public abstract class Visitor
        {
            /// <summary>
            /// 指定访问的元素
            /// 具体访问元素的方法
            /// </summary>
            /// <param name="element"></param>
            public abstract void Visit(Element element);
        }

        /// <summary>
        /// 抽象元素
        /// </summary>
        public abstract class Element
        {
            /// <summary>
            /// 指定接收的访问者，对访问者进行过滤
            /// </summary>
            /// <param name="vistor"></param>
            public abstract void Accept(Visitor vistor);

            /// <summary>
            /// 元素内方法
            /// </summary>
            public abstract void Func();
        }

        /// <summary>
        /// 具体访问者
        /// </summary>
        public class ConcreteVisotor1 : Visitor
        {
            /// <summary>
            ///
            /// </summary>
            /// <param name="e"></param>
            public override void Visit(Element e)
            {
                e.Func();
            }
        }

        public class ConcreteElement1 : Element
        {
            public override void Accept(Visitor vistor)
            {
                vistor.Visit(this);
            }

            public override void Func()
            {
                //Console.WriteLine(this.GetType().ToString() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        /// <summary>
        /// 具体元素集合
        /// </summary>
        public class ObjectStruct
        {
            public IList<Element> Elements = new List<Element>();

            public void AddElement(Element ele)
            {
                Elements.Add(ele);
            }

            public void RemoveElement(Element ele)
            {
                Elements.Remove(ele);
            }

            public void Accept(Visitor vistor)
            {
                Elements.ToList().ForEach(e => e.Accept(vistor));
            }
        }
    }

    #endregion Action Pattern
}