
#define DEBUGX

#undef DEBUGX

using System;
using System.Diagnostics;
using System.Reflection;

namespace ZHello.Base
{

    public class BaseKnowledge
    {
        public interface ITest
        {
#if DEBUGX
            int Index {get;set;}
#elif DEBUGX2
            int Index2 {get;set;}
#else
            int Index {get;set;}
#endif
            string S { get; set; }
            Delegate MDelegate { get; set; }

            void Func1();
        }

        public interface ITest2<T>
        {
            T Obj { get; set; }

        }

        /// <summary>
        /// 隐式类型转换
        /// </summary>
        /// <param name="k"></param>
        public static implicit operator Int32(BaseKnowledge k)
        {
            return k.ToString().GetHashCode();
        }

        /// <summary>
        /// 显式类型转换
        /// </summary>
        /// <param name="k"></param>
        public static explicit operator string(BaseKnowledge k)
        {
            return k.ToString().GetHashCode().ToString();
        }

        public static DateTime GetDllBuildDate(string version)
        {
            if (string.IsNullOrEmpty(version))
                return DateTime.MinValue;
            var vers = version.Split('.');
            if (vers.Length != 4)
                return DateTime.MinValue;
            int build = 0, reversion = 0;
            int.TryParse(vers[2], out build);
            int.TryParse(vers[3], out reversion);
            var b = new DateTime(2000, 1, 1);
            b = b.AddDays(build);
            b = b.AddSeconds(reversion * 2);
            return b;
        }

        public static void GetDllVersionInfo(out string version, out DateTime time)
        {
            version = null;
            time = DateTime.MinValue;
            version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            time = GetDllBuildDate(version);
        }

        /// <summary>
        /// 编译器条件编译
        /// </summary>
        [Conditional(conditionString: "Release")]
        public static void Fun1()
        {
        }

        public static void Main()
        {
            var k = new BaseKnowledge();
            int a = k;
            string s = (string)k;
            AppDomain.CurrentDomain.DomainUnload -= CurrentDomain_DomainUnload;
            AppDomain.CurrentDomain.DomainUnload += CurrentDomain_DomainUnload;
            AppDomain.CurrentDomain.AssemblyLoad -= CurrentDomain_AssemblyLoad;
            AppDomain.CurrentDomain.AssemblyLoad += CurrentDomain_AssemblyLoad;
            AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            var v = Assembly.GetExecutingAssembly().GetName().Version;
            var v1 = Assembly.GetExecutingAssembly().GetName().VersionCompatibility;
            var v2 = Assembly.GetExecutingAssembly().GetCustomAttributesData();
            Console.ReadLine();
        }

        /// <summary>
        /// 当前应用程序域卸载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 应用程序集加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            var ass = args.LoadedAssembly;
        }

        /// <summary>
        /// 程序集解析失败
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var ass = args.Name;
            var sa = args.RequestingAssembly;
            return null;
        }
    }
}

/* 基础
*
* 1..NET Framework
* 包括：运行环境（运行时）和类库
* 运行环境提供.net应用程序所需的核心服务，主要：内存分配、清理服务
*
* 运行环境-公共语言基础结构CLI-Common Language Infrastructure
* 规范性：主要定义了可执行代码和运行环境的规范；
* CLR
* 公共语言运行时（Common Language Runtime）-确保应用程序符合安全规则，并为应用程序提供资源；
* 微软专用的运行环境CLR遵循标准就是CLI；
* VES
* 运行环境-Virtual Execution System
* CTS
* CLI的核心组成-通用类型系统（Common Type System），定义编程语言的类型和操作规范；
* CLS
* 公共语言规范-（Common Language Specification）-提供高级语言互操作的规范；
* Managed Code
* 托管代码-通过.net编写的程序；
* CIL
* 托管代码通过公共中间语言（Common Intermediate Language ）和文件格式进行传输和存储
* 所有的源代码语言都要编译成CIL指令集。MSIL-Microsoft实现的CIL；
* 托管数据由CLR自动分配和释放，存储在托管堆上（managed heap），通过垃圾回收机制自动释放数据。
* 托管堆（managed heap）
* 应用程序第一次启动，CLR为该应用程序预留一块内存即托管堆
* 堆：带有内存基址的一块内存区。内存区连续，存储区按线性方式分配，垃圾回收
* 泛型约束
* 1.where T:struct 结构约束    T必须是值类型,必须在其他所有约束之前，与class不能共用
* 2.where T:class  类型约束    T必须是引用类型，必须在其他所有约束之前，与struct不能共用
* 3.where T:IFoo   接口预算    T必须实现IFoo接口
* 4.where T:Foo    基类约束    T必须继承自基类Foo
* 5.where T:new()  构造约束    T必须具有默认的new()构造函数（这里仅限于默认构造函数），必须为最后一个约束,且不能与struct约束共用
* 6.where T1:T2    裸类型约束   T1必须派生自泛型T2
*
* 约束合并
* eg：where T:Foo,IFoo1,new()
*
* 协变和抗变（逆变）
* 指对参数和返回值的类型转换
* net中参数类型是协变的
    参数不能 父类->子类 但是可以 子类->父类
* 即Func(ClassA ca,ClassB cb) 传值时 Func(ClassAA caa,ClassBB cbb) ClassAA:ClassA ClassBB:ClassB
* 即可以传递派生自基类的任何对象作为参数
* 方法的返回值总是抗变的 返回值不能 父类->子类 但是可以 子类->父类
* 若 ClassAA() 可以返回 ClassA
* 即可以返回 返回类型的子类的任何对象
* 即父类不一定转换为子类，但子类一定可以转换为父类
* C#4 支持 泛型接口 和 泛型委托 的协变和抗变
* 对于泛型接口
* 参数使用out标注 表示该参数是协变的
* 返回值使用in标注 表示该返回值是抗变的
*
* nullable<T>--解决结构为空问题
* 可空泛型（结构泛型）
* 1.泛型约束：泛型类型必须为结构类型，不能为类类型（类本身为引用类型可空，而结构为值类型不能为空）
* 2.强制类型转换：Nullable转换为T显示；T转换为Nullable为隐式；
* 引申 定义可空类型变量
* 使用？定义
* eg：以下都是可空的int类型
* Nullable<int> x1;
* int? x2;
*
* 类型转换：
* 非可空类型总是可以转换为可空类型，且为隐式转换并且总是能够成功
* 可空类型转换为非可空类型可能会失败，若可空类型为null时，则转换失败，所以需要显式强制转换；
* 另一种方式就是（即不使用显式强制转换）使用合并运算符进行转换，为转换定义默认值
* 合并运算符(coalescing operator) ??
* 泛型方法
* 1.泛型方法可以定义在非泛型类中
* 2.泛型方法可以重载，但不能根据泛型约束不同重载，可以根据泛型类型数量不同重载
* 但不可以指定in out
* in out 只适用于泛型接口或泛型委托
* 编译器调用时有限匹配最佳方法，即有限使用非泛型匹配的方法，其他类型则使用泛型方法，这个是在编译期间调用的，不是运行期间
* 即编译时使用哪个方法匹配，运行时就使用哪个方法匹配；-> 泛型方法调用泛型方法问题，运行时不会出现泛型内调用非泛型匹配方法
* 注意：编译期间调用 与 运行期间调用
*
* 创建某个类型的第一个实例时,所进行的操作顺序为:
    1.静态变量设置为0
    2.执行静态变量初始化器
    3.执行基类的静态构造函数
    4.执行静态构造函数
    5.实例变量设置为0
    6.执行衯变量初始化器
    7.执行基类中合适的实例构造函数
    8.执行实例构造函数

    同样类型的第二个以及以后的实例将从第五步开始执行.
*
* 数组
* 1.数组是一种数据结构
* 2.数组是引用类型
* 3.初始化后数组大小不能改变，不同于集合
* 4.预定义类型数组，自定义类型数组
* 5.若数组中的元素是引用类型，则必须为每个元素分配内存
* 6.多维数组 与 锯齿数组
* 7.Array类抽象数组类
* 数组复制
* 1.数组为引用类型，若直接复制将会得到对同一个数组对象（地址）的两个引用
* 2.复制数组实现ICloneable接口,实现数组的 浅表副本 复制
* 3.若数组元素为值类型，则Clone()方法复制所有的值
* 4.若数组元素为引用类型，则Clone()方法只复制元素的引用而不复制元素本身
* 对比Clone()和Copy()
* 同：都是创建数组的浅表副本
* 异：Clone()会创建新的数组，而Copy()必须传递阶数相同且有足够元素的数组
*
* 若创建数组的深层副本，就必须迭代数组并创建新对象
* 数组排序
*1.数组内元素使用QuickSort算法对元素进行排序;
*2.Sort()方法需要数组中的元素实现ICompareable接口;
*3.自定义的数组元素必须实现ICompareable接口；
*
* ICompareable、ICompareable<T>接口
* 接口方法 int CompareTo(Object)、int CompareTo(T);
* 说明：若实例与参数对象相同，返回0；若实例在参数对象前面则返回值小于0；若实例在参数对象后面则返回值大于0；
*
* IComparer或者IComparer<T>接口
* 若数组元素类不能修改或排序方式不同，则需要实现IComparer或者IComparer<T>接口
* 接口方法 int Compare(Object,Object)、int Compare(T,T)
* 说明：返回值与ICompareable类似
*
* 数组参数 与 数组协变
* 1.函数参数、返回值均可定义为数组类型
* 2.数组支持协变，即可以将数组声明为基类,将其派生类元素赋予数组
* 3.协变只能用于引用类型，不能用于值类型
*
* 数组段 ArraySegment<T>
* 1.数组的一段，通过（原数组，偏移量，元素个数）初始化
* 2.数组段不复制原数组元素，但可以通过ArraySegment<T>访问原数组，若数组段元素改变则会相应的反映到原数组中
*
* yield
* 使用迭代快，C#编译器会生成yield类型，其中包含状态值，yield类型实现IEnumerator和IDisposeable接口，或者其泛型版本
* yield语句会生成一个枚举器
* 类支持的默认迭代是返回IEnumerator的GetEnumerator()方法，命令返回IEnumerable
* 用yield return 返回枚举器
* 元组
* 区别与数组
* 1.数组合并相同类型对象，元组合并不同类型对象
* 2..Net4定义了8个泛型Tuple类和一个静态Tuple类，作为元组的工厂，不同的泛型Tuple类支持不同数量元素
* 3.元组使用静态类Tuple的静态Create方法创建
* 4.可以使用Item1、Item2、。。。访问元组元素
* 5.如果元素的元素项超过8个，可以使用8个参数的Tuple类定义，最后一个模板参数为TRest,TRest必须传递一个元组
*
* 结构比较-数组和元组
* 1.数组和元组都实现接口IStructuralEquatable, IStructuralComparable，且都为显式实现, IComparable
* 2.IStructuralEquatable  比较两个数组或元组是否拥有相同的内容
* 3.IStructuralComparable 用于给元组或者数组排序
*
* 运算符-operator
* 1.sizeof -> * & 值能用于不安全代码 unsafe{}
* 2.运算符的简化
* 3.条件运算符 condition？true：false;
* 4.checked 和unchecked
*  checked { } CLR 执行溢出检查
*  unchecked { } CLR 不执行溢出检查
*  unchecked 是默认行为
* 5.is运算符 用于检查该对象是否与特定类型兼容
*  兼容：是该类型、该类型的派生类
* 6.as执行类型的显示转换，若类型兼容则成功，否则返回null
*  可以不需要is测试类型兼容性
* 7.sizeof用于确定栈中值类型需要的长度，单位字节
* 对于复杂类型以及非基元类型，sizeof需要放在unsafe{}下执行
* 8.typeof 返回一个表示特定类型的system.Type对象
*  利用反射查找对象相关信息
* 9.可空类型和运算符
*      int？
* 10.空合并运算符 ？？
* 处理可空类型和引用类型时，表示null可能的值
* 运算符使用 int？ res = a??b;
* 要求：b必须与a类型相同或者可以隐式转换为a类型
* 计算：若a不是null则结果res为a值；若a为null则res为b值；
* 11.运算符优先级
* 初级运算符 > 一元运算符 > 乘除运算符 > 加减运算符 > 移位运算符 > 关系运算符 > 比较运算符
* > &(AND) > ^(XOR) > |(OR) > && > || > 条件运算符(?:) > 赋值运算符
* 注：复杂表达式避免使用运算符优先级生成结果，最好使用（）
*  初级运算符：(),[],sizeof,typeof,X++,X--,new,checked,unchecked
*  一元运算符：+,-,!,~,++x,--X,数据类型强制转换
*  乘除运算符:*,/,%
*  移位运算符：<<,>>
*  关系运算符: <,>,>=,<=,as,is
*  赋值运算符：=,+=,-=,*=,/=,&=,|=,^=,>>=,<<=,>>>=
*
* 类型的安全性
* 1.类型转换
* 类型隐式转换
* 保证值不发生任何变化
* 原则：
* 1.只能从较小的整数形式转换为较大的整数形式，浮点类型，有符号类型特殊处理
* 2.可空类型隐式转换为其他可空类型
* 3.非可空类型隐式转换为可空类型
* 4.可空类型不能隐式转换为非可空类型
* demical不能隐式转换为其他整型或者浮点类型，因为demical内部结构不同
*
* 显式转换
* 可以使用cast显式转换
* Type A = (Type)B;
* 可以使用checked检测算术溢出
* double b;
* int a = checked((int)b);
*
* 比较对象相等性
* 区分引用类型和值类型（基元类型，结构类型，枚举实例）
*
* 1.比较引用类型
* system.Object 类定义了ReferenceEquals()和两个版本的Equals() 以及相等运算符（==）
* 1.1 bool ReferenceEquals(object,object)
*  静态方法，测试两个引用是否引用类的同一个实例，即两个引用是否包含相同的内存地址,相同返回true，否则返回false
* 1.2 virtual bool Equals(object)
*  可以重写，从而按值比较对象。特别若希望类的实例作为字典中的键，就需要重写该方法,重写的Equals内代码不会抛出异常
* 1.3 static bool Equals(object,object)
*  静态方法，处理两个对象有null的情况下，
*  若都为null，则返回true，若只有一个为null，则返回false，若都不为null，则返回虚函数Equals
*  比较运算符（==）
*  严格的值比较与严格引用比较中间项，一般情况表示比较引用
*
* 2.比较值类型相等
* 与比较引用类型类似:
* ReferenceEquals()比较引用；
* Equals()比较值
* 比较运算符中间项
* 不能对结构重载==运算符
* Equals()默认比较地址，值类型中存在引用需要重写Equals()
* 运算符重载
* 编译器知道所有常见运算符对于数据类型的含义。
* 1.运算符的工作方式
*  即编译器遇到运算符是如何处理
*  1.根据参数类型查找该运算符最匹配的重载，注预定义的返回类型不会影响编译器对重载方法的选择；
*  2.若找不到匹配项在报错
*
* 2.运算符重载
*
* public static returns operator opeartors (pars[]);
* 若为二元运算符，则第一个参数表示运算符左边的参数命名为lhs，第二个参数表示运算符右边的参数rhs
* 要求：运算符重载必须声明为public 和static 即应该与特定的实例无关，而只与类或者结构相关，其代码不能访问非静态成员或者被this引用
* 编译器不会颠倒参数顺序
* C#不允许=重载，-=，+=，*=，/=，&=，|=等没有重载
* 比较运算符重载：
* C#要求
* 1.若重载了==则必须重载!=，>和<，<=和>=都必须成对重载；
* 2.必须重载system.Object 继承的Equals()和GetHasCode(),原因:Equals()应实现与“==”相同类型的相等逻辑；
* 3.比较运算符必须返回bool类型；
* 4.不要通过System.Object的中个Equals()实例版本，重载比较运算符，会出现null.Equals(objectB)异常
*
* 浅度比较与深度比较
* 浅度比较：比较对象是否指向内存中的同一位置；
* 深度比较：比较对象的值和属性是否相等；
*
* 并非所有的运算符都可以重载
* 赋值运算符不能显式重载；
* []索引运算符不能直接重载--索引器成员支持索引运算符[]
* ()数据类型转换运算符--不能直接重载，用户定义的类型强制转换允许定制强制转换运算符
* 比较运算符必须成对重载
* 按位一元运算符（！，~，true，false）--true和false必须成对重载
* 隐式类型转换
* public static implicit operator [type]([params])
* 显式类型转换
* publc staic explicit operator [tye]([params])
* 用户定义的类型强制转换
* 1.首先应遵循预定义类型都类型强制转换原则：
* 若无论源如何，类型强制转换总是安全的，则声明为隐式转换--implicit
* 若可能会出错、丢失数据、抛出异常，则应声明为显式转换--explicit
* 同样需要声明为 public static implicit/explicit ...
*
* 1.类之间的类型强制转换
*  限制：
*  1.派生类之间不能定义类型强制转换（本身已经存在）
*  2.类型强制转换必须在源类型（结构）或者目标类型（结构）内部定义
*  3.对于每一种转换只能有一种类强制转换
*
* 2.基类与派生类之间类强制转换
* 派生类可以隐式的转换为基类，因为对于基类的任何引用都可以引用基类的对象或任何派生自基类的对象
* 基类转换为派生类，通常在派生类中添加基类实例参数的构造函数
*
* 3.装箱与拆箱
* 基本结构与派生结构之间的强制转都是基元型或结构与system.Object之间的转换
* 结构到object 隐式是装箱
* 具体过程：
* 1.在定义结构时，.NET Framework会隐式的提供另一个类即装箱的该类，该类包含与当前结构完全相同的字段，但是是引用类型，存储在堆上；
* 2.隐式的将该结构实例强制转换为Object时，就会实例化一个该结构的装箱类，并使用该结构数据初始化
* 3.从而对于Object的引用其实就是该实例化的装箱类的引用
*
* 拆箱
* 就是将装箱类的数据复制到一个新的该结构中。
*
* 装箱 拆箱 都是把数据复制到新装箱类或者新拆箱的对象中。
*
* 4.多重类型转换
* 若不存在直接类型转换，存在隐式或者显示类型转换
* 多重类型转换
* eg：目标：A->D，但不存在直接A到D的转换，
* 而是存在A->B,B->C（隐式），C->D(隐式)，
* 则可以A->D进行强制转换
* 委托
* 寻址方法的.NET版本，类型安全类，定义了返回类型和参数类型
* 1.声明委托
* 即定义委托，定义时必须指明所表示方法的返回值、签名等
* 给方法的返回值和签名类型指定名称
* delegate void FuncCallBack();
* 定义位置：
* 委托是一个新类，可以在定义类的任何地方定义委托
* 委托实现派生自基类System.MulticastDelegate类，System.MulticastDelegate类派生自基类System.Delegate
* 在C#中委托总是可以接受一个参数的构造函数，该参数为委托引用的方法，此方法必须匹配委托定义时的签名
* 在任何代码中都应提供委托实例的名称
* 委托推断
* 减少输入量，只传递地址名称
* 只能把方法的地址赋予委托变量（应不含括号）
* 类型安全，确保被调用的方法签名正确，但不关心在什么类型对象上使用该方法，以及该方法是否为静态方法
* 给定委托的实例可以引用任何类型的对象上的实例方法或者静态方法，只要方法的签名匹配与委托的签名(参数修饰符必须一致)
*
* Action<T>和Func<T>委托
* 泛型Action<T>委托表示引用返回void的方法，存在不同变体，最多存在16种不同类型的参数，但不存在没有参数的方法
* 泛型Func<T>委托表示引用带返回类型的方法，存在不同变体，最多存在16种不同类型的参数，Func<out result>可允许调用带返回值且无参数的方法
* eg:Func<T,K,TResult> 最后一个参数总为返回值
*
*多播委托
* 一个委托只包含一个方法调用，多个方法调用需要多个委托
* 多播委托：一个委托包含多个方法，按照一定顺序连续调用多个方法，委托签名必须为void否则将只会得到最后一个方法的返回值
* 实际上：多播委托派生自类System.MultiCastDelegate类（该类派生自System.Delegate）该类其他成员允许把多个方法的调用链接为一个列表
*
* 支持“+”、“+=”、“-”、“-=”运算，即列表添加或者删除函数调用
* mark：
* 1.方法执行顺序，对于同一个委托的方法调用顺序并未定义，所以避免依赖于方法的顺序产生结果
* 2.方法异常，若执行委托的方法的过程中抛出异常，则之后的所有方法都将不会执行调用（即委托迭代会停止），解决应自己迭代委托执行
* Delegate类型定义了，GetInvocationList()方法，该方法返回一个Delegate对象数组，可以迭代执行此数组，处理异常
* eg：Delegate[] delegates = Objet.GetInvocationList();
*      foreach(Action a in delegates){ ... }
*
* 匿名方法
* 匿名方法委托参数的一段代码
* 代码执行不是很快，编译器自动指定一个匿名方法的名称
*
* 遵循原则：
*  1.匿名方法不能跳转到匿名方法外部
*  2.匿名方法的外部不能跳转到匿名方法内部
*  3.匿名方法内部不能访问不安全代码，不能使用ref、out
* 可以使用Lambda表达是代替匿名方法
*
* Lambda表达式
* 三部分组成
* 1.放在括号中的参数列表
* 2.=>运算符
* 3.C#语句
* C#编译器会提取=>后的代码，创建一个匿名方法
* 参数：
*  1.可以通过类型推断确定参数类型，也可以定义参数类型
*
* 字符串string
* string映射System.String类型，
* System.Text和System.Text.RegularExpressions
* 1.创建字符串---可以考虑使用System.Text.StringBulider
* 2.格式化表达式---使用接口IFormatProvider和IFormattable处理
* 3.正则表达式---识别字符串、提取子字符串System
*
* System.String类
* 1.string类常用方法
*      Compare、CompareOriginal、Contact、CopyTo、Format、IndexOf、IndexOfAny、Insert、Join、LastIndexOf、
*      LastIndexOfAny、PadLeft、PadRight、Replace、Split、SubString、Trim、ToLower、ToUpper
* 2.创建字符串
*      重复修改字符串，效率很低。
*      string为不可变的数据类型，一旦初始化该对象就不能再改变
*      StringBuilder类仅限对字符串进行替换、追加、删除、修改处理
*      （需引用namespace：System.Text）
*      StringBuilder类属性：Length-字符串的实际长度；Capacity-字符串分配在内存的最大长度；
*      初始化时指定内存初始容量，在内存中操作，若内存不够，则自动进行容量翻倍
*
*      StringBuilder成员：
*      1.构造函数可以指定字符串或容量，也可以设置MaxCapacity最大容量
*
* 3.格式化字符串
*      .NET定义标准方式：IFormattable接口
*      格式说明符{obj,len:FormatStr}    eg:{0,10:E}
*
*      格式化过程：传递给格式化函数，对象，格式说明符
*      1.首先检查该对象是否实现了IFormattable接口（使用is关键字测试或者强制转换测试）；
*      2.若测试失败则执行该对象的ToString()方法；
*      3.
*
*  IFormattable接口
*  只有一个方法string ToString(string,IFormatProvider);
* 正则表达式
* 在大字符串中定位一个小字符串
* System.Text.RegularExpressions
*
* 1.概述
*  正则表达式语言是一种专门用于字符串处理的语言。
*  主要功能：
*  1.一组用于标识字符类型转义代码
*  2.一个系统，用于字符串检索、组合等操作
*  3.识别/标记/删除字符串中所有单词
*  4.单词转化为标题格式
*  5.确保句子正确大小写
*  6.区分URI各个元素
*
* 正则表达式字符串：包含转义序列以及特定含义字符
*
* 模式字符串：要匹配项或者要搜索项字符串，可包含元字符和转义序列
* 转义序列：以反斜杠开始\的字符且具有特殊含义
*     eg：
*     \b-表示字的边界;\B-不是边界的任意位置;
*     \S-表示不是空白字符的任何字符;\s-任何空白字符；
*     *-限定符，前面的字符可以重复任意次，包括0次;.-除了换行符以外的所有单个字符
*     ^-输入文本的开头;
*     $-输入文本的结尾;
*     +-可以重复1次或多次的前导字符;eg:ra+t -> rat/raat/raaat/...;
*     ?-可以重复1次或者0次的前导字符;eg:ra?t -> rat/rt;
*
*  若搜索一个元字符，则使用反斜杠进行转义
* 元字符：指定的特殊命令字符
*
*      可以将要替换的字符放在[]中，eg：[a|b];
*      指定范围[],eg:[a-z]/[1-9];
*
*  可以使用圆括号将任意数量字符组合成为一个组
*  匹配结果不能重复，若存在重复，则默认下选择最长匹配
*
* 集合
* 元素个数可动态改变;
* NameSpace：System.Collections 和 System.Collections.Generic;
*
* 泛型类型集合：System.Collections.Generic;
* 特定类型集合：System.Collections.Specialized;
* 线程安全集合：System.Collections.Concurrent;
*
* 集合可根据集合类实现的接口组合为列表、集合、字典;
*
* 集合接口和集合类型
* IEnumerable<T> - foreach 枚举;
* ICollection<T> - 泛型集合类实现，Count、Add、Remove、Clear、CopyTo;
* IList<T> - 可通过位置访问元素列表;
*
* 列表 IList<T>
* 实现接口：IList、ICollection、IEnumerable、IList<T>、ICollection<T>、IEnumerable<T>;
*
* 动态列表添加元素：
* 4-》8-》16-》32。。。
* 每次容量调整重新设置为原来的2倍，且重新分配内存块;
*
* 1.集合的初始值设定项;
* 初始化集合时可以使用{}指定该集合的初始值IL使用Add方法添加每一项元素;
* 2.添加元素
* Add（）、或者AddRange（）添加多个元素,通用可以添加数组;
* 3.插入元素
* Insert()、InsertRange（）;
* 4.访问元素
* IList和IList<T>接口，实现访问器访问元素;
* 或者foreach遍历，IList实现IEnumerable和IEnumerator接口;
* IList<T>、ForEach（Action<T>）;调用每一项元素作为方法的参数;
* 5.删除元素
* RemoveAt（） RemoveRange（）
* 6.搜索元素
* 搜索某个特性的元素
* int FindIndex（bool Predicate<T> match）
* T Find(bool Predicate<T> match)
* List<T> FindAll(bool Predicate<T> match)
* 7.排序元素
* Sort()-快速排序
* 需要Comparison<T>委托和ICompare<T>接口
* 若元素实现ICompareable接口，则可以使用不带参数的Sort（）
* 8.类型转换
* ConvetAll<TOutput>
* TOutput---Converter委托
* public sealed delegate TOutput Converter<TInput,TOutput>(TInput from)
*
* 只读集合
* List<T>的AsReadOnly（）返回只读集合ReadOnlyCollection<T>
*
* 队列Queue<T>
* 以FIFO方式处理元素的集合。
*
* System.Collection.Generic
* Queue<T>
* ICollection的IEnumerable<T>接口,但没有实现ICollection<T>接口
* 没有实现IList<T>接口
*
* Enqueue()在队列一端添加元素
* Dequeue()在队列尾读取同时删除该元素
* Peek()读取但不删除
*
* 栈 Statck<T>
* LIFO、FILO
* 添加元素-入栈-Push()
* 读取并删除元素-出栈-Pop()
* 读取元素但不删除-Peek()
*
* 链表 LinkedList<T>
* 有序列表 SortedList<TKey,TValue>-按照键给元素排序
*
* 字典 Dictionary<TKey,TValue>
* 映射表或散列表
*
* 键类型
* 用作字典中键类型必须重新Object的GetHashCode()
* 必须实现IEquatable<T>.Equals()方法或者Object的Equals（）
* 确定元素位置调用GetHashCode()，用于计算元素位置索引，且为一个素数，所以字典的容量是一个素数
* 字典的性能取决于GetHashCode()的实现代码
*
* GetHashCode()要求
* 1.相同的对象总是返回相同的值；
* 2.不同的对象可以返回相同值；
* 3.执行速度快，计算开销不大；
* 4.不能抛出异常；
* 5.至少使用一个实例字段；
* 6.散列代码值应平均分布在int可以存储的整个数字范围上；
* 7.散列代码最好在对象的生存周期中不发生变化；
* IEquatable<T>.Equals() or Object的Equals()
* 且应保证
* 若A.Equals(B)==true
* 则A.GetHasCode()==B.GetHashCode()
* string 可以用作键 实现IEquatble接口且重载GetHashCode方法
* Int32 可以用作键，但存在性能损失
* 键类型若没有实现IEquatable接口以及重载GetHashCode方法，则可以实现IEqualityComparer<T>比较器
*
* Lookup类 Lookup<TKey,TElement>
* 创建
* 必须调用ToLookup()方法，
* ToLookup()扩展方法，实现了IEnumerable<T>的所有类都可以使用
* 有序字典 SortedDictionary<TKey,TValue>
* 二叉搜索树，其中的元素根据键排序。
* 键必须实现ICompareable<TKey>或者实现IComparer<TKey>比较器
* 与SortedList比较
* 1.SortedList使用内存少；
* 2.SortedDictionary插入删除未排序数据快；
* 3.对于已排序数据，填充集合时，若不扩容，则SortedList比较快；
*
* 集合 set
* 包含不重复的元素的集合
* 两个集 都实现ISet<T>接口
* HashSet<T> 包含不重复元素的无序表
* SortedSet<T> 包含不重复元素的有序表
*
* 可观察的集合 ObservableCollection<T>
* 需要知道集合中的元素何时删除或者添加信息
* 位数组 BitArray BitVector32
* BitArray System.Collections 可以设置大小 引用类型
* BitVector32 System.Collections.Specialized 基于栈，速度快仅32位，值类型
*
* 并发集合
* System.Collections.Concurrent
* 防止多线程冲突访问集合。
* Linq概述
*
* Linq查询：
* 1.必须以from开始，以select或group 结束，可以使用where、orderby、join。let、以及其他from
* 2.Linq语句执行，Linq语句只是给查询赋值，只要访问该查询就会执行该查询
* 3.为IEnumerable<T>提供扩展，可以在实现该接口的类的集合上使用Linq
*
* 扩展方法
* 在静态类中声明，定义为静态方法，第一个参数定义为扩展类型，并使用this前置标识区分一般静态方法与扩展方法
* 参数 public static T Func(this T t)
* 1.扩展方法不能访问扩展类型的私有成员；
* 2.扩展方法是调用静态方法的另一种方式（不必提供类名，一般类型+方法名调用），只需导入该类的命名空间即可。
* 定义Linq扩展方法的类是System.Linq中的Enumerable类
*
* 推迟查询的执行
* 运行时定义查询，查询不会执行，迭代数据时查询执行。
* 但特殊情况下查询会立即执行。
* ToArray()/ToEnumerable()/ToList()等。
*
* 标准查询操作符
* 1.筛选
*  source.where(()=>);
*  from T in source
*  where(()=>)
*  select T
* 2.使用索引筛选
*  where传递第二个索引参数
*  where((t,index)=>{...});
* 3.类型筛选
* 基于类型筛选
* 使用OfType()方法
* 4.复合from子句
* from t in T
* from k in t...
* 转换为SelectMany()方法
* 5.排序
* orderby解析为OrderBy()方法
* 6.分组
* group解析为GroupBy()方法
* 7.对嵌套对象分组
* 使用匿名类型
* 8.连接
* 合并数据源
* join ... on ... equals ...
* 左外连接 join ... on ... equals ... source.DefaultIfEmpty()
* 9.集合操作
* Distinct()/Union()/Intersect()/Except()
* 10.合并
* Zip()将两个相关的序列合并为一个
* 11.分区
* Take()/Skip()分区操作
* 12.聚合操作符
* 13.转换运算符
* 使用转换运算符会导致马上执行查询
* 14.生成操作符
* 并行Linq
* System.Linq空间中的新类ParallelEnumerable可以将查询工作分解到多个线程上进行。
* ParallelEnumerable的大多数扩展方法是ParallelQuery<TSource>类的扩展。
* AsParalle()方法扩展了IEnumerable<TSource>接口，返回ParallelQuery<TSource>类型
*
* Parallel - 并行、平行
* 1.并行查询
*   source.AsParalle()
* 2.分区器
* Partitioner类 - 影响要创建的分区，
* System.Collection.Concurrent     * 表达式树 Expression<T>
 * 类型为Expression<T>的Lambda表达式，C#编译器从Lambda中创建一个表达式树，存储在程序集中
 * 在运行期间分析表达式树，并进行优化
 * Expression<T>
 * 表达式树从派生自抽象基类Expression类中构建。
 *
 * Linq提供程序
 * 为特定数据源提供标准查询操作符。
 * Linq提供程序的实现方案根据名称空间和第一个参数类型来选择。
 * 实现扩展方法的类的名称空间必须是开放的。
 * 编译器根据第一个参数 即source参数确定要使用的方法。
 * 影响并行机制：
 * WithExceptionMode() - 传递ParallelExceptionMode的Default值或者ForceParallism值，
 *      - 系统开销
 * WithDegreeOfParallism(int degree) - 指定并行最大任务数
 *
 * 3.取消
 * 取消长时间查询-WithCancellation()
 * public static ParallelQuery<TSource> WithCancellation<TSource>(this ParallelQuery<TSource> source, CancellationToken cancellationToken);
 *
 * 传递一个CancellationToken令牌作为参数
 * CancellationToken令牌从CancellationTokenSource类中创建
 * 在查询的线程中，若取消查询，则try...Catch异常OperationCanceledException
 * 主线程中调用CancellationTokenSource类的Cancel(),可以取消任务
 *
 * DLR - Dynamic Language Runtime
 * 动态语言运行时
 * System.Dynamic
 * System.Runtime.ComplierServices
 *
 * dynamic类型
 * 允许编写忽略编译期间类型检查的代码
 * 定义为dynaminc的对象在运行期间改变对象。
 * var为类型推断，一旦类型确定就不能更改
 *
 * dynamic限制：
 * 1.动态对象不支持扩展方法；
 * 2.匿名函数（Lambd）不支持动态方法做参数，即Linq不能用于动态对象；
 * IL中执行：
 * 对于动态类型，添加了对
 * System.Runtime.CompilerServices.CallSite和System.Runtime.CompilerServices.CallSiteBinder的引用
 * CallSite在运行期间处理查找操作的类型。
 * CallSite完成查找操作后，调用CallSiteBinder()方法，从CallSite中提取信息生成表达式树。
 *
 * 包含DLR ScriptRuntime
 * 脚本编辑执行功能。
 *
 * 执行脚本；
 * 1.确定脚本文件或脚本代码
 * 2.其他ScriptRuntime环境
 *      a.创建ScriptRuntime对象
 *      b.设置合适的ScriptEngine
 *      c.创建ScriptSource以及ScriptCode
 *
 * 3.ScriptRuntime对象由静态方法CreateFromConfiguration()创建，即在App.Config文件中添加节点
 *
 * 4.ScriptEngine提供脚本执行工作。
 * 5.ScriptSource允许访问脚本，表示脚本源代码。
 * 6.ScriptCode - 名称空间，若对脚本传入传出值，需要将变量绑定到ScriptCode上，
 *      执行SetVariable()传值，GetVariable()取值
 * 7.ScriptSource.Excute(ScriptCode)
 * 8.value = ScriptCode.GetVariable()..
 *
 * DynamicObject ExpandoObject - 创建自己的动态类型
 *
 * 1.从DynamicObject派生，但必须重写几个方法
 * 2.使用密封类ExpandoObject
 *
 * 1.DynamicObject
 * A:DynamicObject{...}
 *
 * dynaminc dyn = new A();
 * dyn.Name=...
 *
 * 2.ExpandoObject
 *
 * dynaminc dyn = new ExpandoObject();
 * dyn.Name=...
 *
 * 自定义动态类型与dynamic类型区别
 * 1.创建dynaminc类型的空对象，必须把dynamic类型赋予某个对象
 * 2.执行GetType，dynaminc返回赋予对象的类型
 *
 * 异步编程 - asynchronous 同步 - synchronous
 * 1.异步编程重要性
 * C#5.0 新增关键字
 * async 、await
 * 异步模式 - asynchronous model
 *
 * 定义了BeginXXXX和EndXXXX方法
 * BeginXXXX接受同步方法所有的输入参数
 * EndXXXX接受同步方法所有的输出参数，并按照同步方法的返回类型返回结果
 * BeginXXXX就收一个AsyncCallBack参数，即用于在异步方法完成后要调用的委托
 * BeginXXXX返回IAsyncResult，用于验证调用是否已经为完成，并等到方法执行结束
 *
 * 基于事件的异步模式
 *
 * 基于事件的异步模式定义带有“Async”后缀的方法
 * 方法完成时，定义要调用的事件，并非委托
 * 事件处理程序是从拥有同步上下文的线程中调用，即在Winform和WPF中的UI线程。
 *
 * 在自定义类中实现，需要使用BackgroundWorker类来实现，该类实现了基于事件的异步模式
 *
 * 基于任务的异步模式 TAP
 * 定义带有"Async"后缀的方法，并返回一个Task类型,"TaskAsync"
 * Task<T>类型不需要声明，只需要声明T类型变量，然后使用await关键字即可，即T t= await Task<T>...
 * await会解除线程阻塞，完成任务。
 *
 * async创建了一个动态机，类似于yield return语句。
 *  await只能在async中使用
 *
 * WPF
 * 在后台进程上填充已绑定的UI的集合。
 * 需使用BindingOperations.EnableCollectionSynchronization属性，启用集合的同步访问功能。
 * eg：
 * private object lockList = new object();
 *
 * BindingOperations.EnableCollectionSynchronization(datasource,lockList);
 *
 * Asynchronous Base
 *
 * 1.创建任务-Create Task
 * 使用Task.Run()创建一个任务
 *
 * 2.调用异步方法
 * 使用await关键字来调用返回任务的异步方法
 * 使用await需要使用有async修饰的方法
 * async修饰符只能用于返回Task或者void的方法，不能用于程序入口点
 * await只能用于返回Task类型的方法
 *
 * 3.延续任务
 * Task<T>该对象包含任务创建信息，并保存到任务结束
 * Task<T>类的ContinueWith方法定义了任务完成后调用的代码
 * 接受已完成的任务作为参数，可以使用result属性访问任务返回的结果
 * 编译器把await后的所有代码放入ContinueWith中来转换await关键字
 *
 * 4.同步上下文-SynchronizationContext
 * 某些应用程序会绑定到指定的线程上（WPF中只有UI线程才可以访问UI元素）
 * 默认情况下，生成的代码会将线程转换到拥有同步上下文的线程中
 *
 * 在一个异步执行的方法中，需要将部分操作递交给其他某个线程执行
 * Eg：如果异步方法调用涉及到对某个窗体中的某个控件的操作，需要将该操作递交给UI线程中执行，因为控件只能在自己被创建的线程中被操作
 * 解决：
 * 1.调用System.Windows.Forms.Control的Invoke或者BeginInvoke方法，将相应的操作通过委托的方式传入该方法中执行；
 * 2.利用同步上下文（SynchronizationContext）
 *
 * 同步上下文：将某个操作封送（Marshal）到某个指定的线程，使其在目标线程上下文中被执行
 *
 * 使用多个异步方法
 * 在一个异步方法中可以调用一个或多个异步方法
 *
 * 1.按顺序调用异步方法
 * 使用await调用异步方法
 *
 * 2.使用组合器
 * 异步方法不依赖于其他异步方法，且不使用await调用，即每个异步方法的返回结果赋给Task
 * Task WhenAll - 所有的方法完成后返回一个Task；WhenAny - 任何一个方法完成后返回Task
 * 一个组合器接受多个同一类型的参数并返回同类型值
 *
 * Task组合器实现
 * eg:
 * Task<int> t1 = FuncAsync1();
 * Task<string> t2 = FuncAsync2();
 * await Task.WhenAll(t1,t2);
 *  //... t1.Result t2.Result
 *
 * 转换异步模式
 * 将基于事件的异步方法转换为基于任务的异步方法
 * BeginXXXX和EndXXXX 转换为 async 和 await
 * Task<T>.TaskFactory.FromAsync<T>(BeginXXX,EndXXX,输入参数,对象状态参数)
 *
 * 错误处理
 * 返回void的异步方法不会等待
 *
 * 1.异步方法异常处理
 * 使用await关键字 放在try/catch中
 *
 * 2.多个异步方法异常处理
 *  a.try 外声明t1/t2/。。。 使catch可以访问，使用属性IsFaulted检查异常状态
 *  使用Exception.InnerException 访问异常信息
 *  b.使用AggregateException
 *      使用组合器 WhenAll定义变量，捕获所有的异常信息列表，遍历结果的 t.Exception.InnerExceptions 异常集合即可
 *
 * 取消
 * 基于CancellationTokenSource类，用于发送取消请求，请求发送引用CancelationToken类
 *
 * 1.开始取消任务
 * CancellationTokenSource类
 *  a。创建令牌 CancellationTokenSource cts
 *  b。将令牌传递给要取消的方法
 *  c。调用令牌的OnCancel()方法进行取消，即cts.OnCancel();或者使用CancelAfter(int time) 传入时间值定时取消（ms）
 *
 * 2.使用框架特性取消任务
 * 将CancellationToken 传入方法。框架中的某些异步方法提供可传入CancellationToken的重载版本以支持取消。
 * 即传入 cts.Token
 * 重载方法定期检查取消令牌，以确定是否满足取消条件
 *
 * 3.取消自定义任务
 * Task类的Run提供重载版本，支持CancellationTokenSource参数
 * 使用IsCancellationRequested属性检查令牌
 *
 * 内存管理-RAM Management
 *
 * 1.值数据类型 - stack
 * windows使用虚拟寻址系统，把系统可用内存地址映射到硬件内存中的实际地址
 * 32为处理器每个进程可以使用的最大4G内存，其包含了程序的所有部门：可执行代码、代码加载的DLL、程序运行时所使用的所有变量的内容等
 * 4G内存称为虚拟地址内存/虚拟内存/内存。
 * 4G中每个存储单元都是从0开始往上排序，若访问某个值则必须体提供表示该存储单元的数字
 * 编程语言中，编译器将变量名转换为处理器识别的内存地址。
 *
 * 内存地址中的一个区域-栈，栈仅存储值数据类型成员，即非对象成员；
 * 调用方法时，栈会存储传递给该方法的所有参数的副本；
 * 栈的工作方式：
 * 变量的作用域 - 变量的生存周期 - 变量的生存周期必须嵌套- 引用变量超出作用域，该变量就从栈上删除
 * 栈指针：表示栈中下一个空闲存储单元的地址，即指针当前值为最后一个使用的单元地址
 * 栈向下填充-从高地址向低地址填充
 * 栈中增加变量-栈指针递减（变量存储长度（字节））
 * 栈中删除变量-栈指针递增
 * 对于 int a，b 存入栈的顺序不确定，但编译器保证先存入内存的变量后删除
 *
 * 2.引用数据类型 - heap
 * 托管堆（堆）是4G内存中的另一个区域，
 * 4个字节将0~4G的内存表示为一个整数
 * 堆的内存向上分配
 * eg:object obj = new Object();
 *
 * 1.首先在栈分配对象引用地址（4字节）,即存储obj值；
 * 2.实例化对象-在堆上新建对象（根据该对象的大小在堆上搜索包含该大小的连续块），即存储新建的对象，并将该对象在堆的地址作为值传给栈上对应的该对象引用；
 * 3.若一个变量引用赋予同类型的另一个变量，则该对象则存在两个引用（即两个栈上的引用），只有对象不被任何变量引用该对象才会删除；
 * 4.只要保存对数据的引用，该数据就存储在堆上不被删除；
 *
 * 垃圾回收 - 针对托管堆
 * 1.垃圾回收器运行时，将堆上没有引用的对象删除；
 * 2.整理压缩堆，使对象移动到堆顶端，从而形成连续的区域
 * 3.移动对象时，同时需要更新栈上的对象引用值，即对象的堆地址值
 * 4.需要交换的页面较少，提高性能；
 * 5.一般垃圾回收器在.NET运行库中自动执行，但可以通过System.GC.Collect()方法，强制垃圾回收器执行；
 * 6.但垃圾回收器的逻辑不一定保证，在一次垃圾收集中，删除所有的未引用的对象；
 *
 * 新对象存储：
 * 1.创建新对象，首先存储在托管堆的第一部门，即第0代，其他新建对象仍旧存放在第0代；
 * 2.垃圾回收进行，删除未引用对象，仍有引用的对象移动到，堆的另一区域，即第1代；
 * 3.依次若第1代原有对象，则移动到第2代上；
 * 4.若新增对象的大小超出第0代容量，则进行立即垃圾回收（或者调用GC.Collect()）
 *
 * 相关对象相邻放置会使程序执行更快。
 *
 * 大对象（超过85 000字节）有特殊的存放堆，但不会执行压缩；
 * 应用程序仅会为第0代和第1代的会后而阻塞，减少总暂停时间，第2代以后以及大对象都在后台线程进行；
 * 此功能默认打开，关闭需要，设置配置文件中元素<gcConcurrent> 设置为false
 *
 * 垃圾回收平衡
 *
 * 控制垃圾回收的方式
 * 设置GCSetting.LatencyMode属性的枚举值；
 * Batch - 禁用并发
 * Interactive - 默认值
 * LowLatency - 只有系统存在内存压力时，才进行完整的回收
 * SustainedLowLatency - 只有系统存在内存压力时，才进行完整的内存块回收
 *
 * 对于64为高内存量，
 * 设置元素<gcAllorVeryLargeObjects>配置，允许创建大于2G的对象，32为存在2G限制；
 *
 * 释放非托管资源
 *
 * 释放非托管资源
 * 两种方式：
 * 1.定义析构函数（终结器）
 * 2.实现IDisposable接口
 *
 * 1.析构函数 - finalizer
 * C#定义析构函数时，编译器传递给程序集Finalized()方法
 * ~ClassName() --- 没有返回值、没有参数、没有访问修饰符；
 * 析构函数会延迟对象销毁时间，没有析构函数的对象会在垃圾回收器的一次处理中从内存销毁；
 * 但有析构函数的对象需要两次才能销毁：第一次，调用析构函数，但不销毁对象；第二次销毁对象；
 * 运行库使用一个线程执行所有对象的析构函数 Finalize()方法
 *
 * 2.IDisposable接口
 * 只有一个方法 Dispose()
 *  引用时必须保证Dispose方法在对象销毁时调用
 *  1.使用try。。。finally。。。
 *      T t；
 *      try{t = new T();...}
 *      catch(..){...}
 *      finally
 *      {
 *          if(t!=null)
 *          t.Dispose();
 *      }
 * 2.使用using语句  - 支持Dispose() 以及Close()
 *      using(T t = new T())
 *      {。。。}
 *      在t作用域结束时（超出作用域时）自动调用Dispose()方法释放资源
 *      IL中与try。。。finally。。。一样
 *
 * 3.实现IDispose和析构函数
 *
 * 不安全代码
 * 1.使用指针直接访问内存
 * 引用：一个类型安全的指针；
 * 优点：
 * 1.向后兼容
 * 2.性能
 *
 * 使用unsafe关键字编写不安全代码
 *      unsafe   {   }
 *      不能将局部变量标记为unsafe
 *
 * 指针的语法
 * C#中*与类型相关,而与变量无关
 * 指针当前值为一个地址值
 * eg：
 * int x =100；
 * int* pInt = &x；
 * 若x在栈中存储位置为 0x0080-0x0083(整型占4个字节 32位)
 * 则指针 pInt值为 0x0080 即 x地址为 0x0080 ，且栈向下存储，所以 0x0080以下的为未使用空间
 * 同样存入x后，栈指针指向0x0080
 * 至于 0x0080~0x0084 具体存储位就不知道了 低位-》高位 ； 高位-》低位；
 *
 * eg:int* p     *
 * & - 表示获取地址；
 * * - 表示获取内容；
 *
 * dword - 四个字节 跨dword边界存储数据会降低硬件性能
 *
 * 指针只能声明为非托管类型，垃圾回收器能访问的类型称为托管类型；
 *
 * 指针强制转换为整型
 * 由于指针实际上存储一个表示地址的整数，因此任何指针地址都可以和整型相互转换
 * 指针到整型-必须为显示转换
 * 除了unit long ulong都可能溢出，整型范围比地址（地址是无符号）范围小
 * checked关键字不能用于涉及指针的转换；
 *
 * 指针之间的转换
 *
 * void*指针
 *
 * 指针的算术运算
 * void指针不能进行算术运算
 *
 * 为类型T的指针加值X，其中指针值为P 则结果
 * P+X*（sizeof(T)）
 * 给定类型的连续存储单元，指针加法允许存储单元之间的移动
 * 但byte char 其总字节数不是4的倍数，但存储按照4的倍数存储
 *
 * sizeof - 获取类型大小，不能对类使用，可以对自定义结构使用
 *
 * 结构指针 - 指针成员访问运算符 - 不能定义指向匿名结构的指针
 * 结构指针的工作方式与预定义值类型的工作方式完全相同
 * 限制：结构不能包含任何引用类型 - 指针不能指向任何引用类型
 * 指针成员访问运算符 ->
 *
 * 类成员指针 - 为类的值成员定义指针
 * 特殊语法
 * 不能直接创建该值成员的指针，垃圾回收会移动该类的对象，从而使指针指向错误的位置
 * 使用fixed关键字 指定指针 并指定fixed作用域，在该作用域内，垃圾回收不会移动该对象在堆上的位置
 * 即 fixed(int* p = &(T.X))
 * {
 * }
 * eg
 *  声明多个指针 也可以嵌套
 *  fixed(int* p1 =&(T.x))
 *  fixed(double* p2 =&(T.y))
 *  {
 * }
 *
 * 或者对于同一类型指针
 * fixed(int* p1 = &() , p2 = &() , 。。。){}
 *
 * 使用指针优化性能
 * 1.创建基于栈的数组
 *  在栈中创建高性能、低系统开销的数组
 *  首先需要在栈上申请内存
 *  使用关键字 stackalloc - 指示.net运行库在栈上分配一定的内存
 *      调用时需提供 1、要存储的数据类型；2.需要存储的数据项数；
 *      mark：stackalloc 值会分配内存，不会对该内存地址进行初始化；
 *      eg： int*  p = stackalloc int[20]；
 *      指针 = stackalloc 数据类型[项数] ；
 *      所以分配的内存 = 项数 * sizeof(数据类型)
 *
 * stackalloc 总是返回分配数据类型的指针，并且指向新分配内存的顶部。
 *      使用数组：
 *      *p = 100; //给第0项赋值
 *      *(p+1) =200；//给第一项赋值
 *      或者
 *      p[0] = 100;//给第0项赋值
 * 若p为任意指针类型 x为整型 则p[x] 被编译器编译为*(p+x)
 * 不会抛出越界异常信息
 *
 * 特性 attribute
 * 自定义特性
   也是一个类不过是一个特殊的类
 * 1.必须继承自System.Attribute父类
 * 2.必须有AttributeUsage特性

 * 编译器对特性的处理（使用）
 * 1.编译器编译对象时遇到特性
 * 2.若特性名后缀没有Attribute则自动添加Attribute后缀
 * 3.根据特性名在所在空间搜索该特性的定义特性
 *
 * AttributeUsage特性
 * windows预定义的特性，可以称为元特性，即所有的特性定义都必须有这个特性
 * 用途：标识自定义特性可以用到哪些类型的元素
 * 使用方法：特性定义前添加AttributeUsage特性
 * [AttributeUsage(AttributeTargets.Class,AllowMultiple=false,Inherited=true)]
 * 枚举参数 AttributeTargets 指定应用目标类型：类？方法？字段？程序集？等可以使用|
 * AllowMultiple = true/false 指定是否可以对统一对象多次使用该特性
 * Inherited =true/false    指定是否可以将此特性自动应用到派生类或接口中
 *
 * 特性可以有构造函数以及构造参数（定义与类相似）可以重载构造函数
 * 可选参数 在引用特性时可以在参数列表中指定特性中定义的公共属性或者字段的值
 * System.Type类---反射的核心类
 * 抽象基类 获取指向任何给定类型的Type引用（3）
 * 1.使用C#的Typeof运算符、is、as
 * 2.使用继承自System.Object的GetType()方法
 * 3.使用Type类的静态方法Type.GetType（）
 * note:这里返回的type引用仅仅是实例的一个数据类型，不含实例的任何数据信息
 *
 * Type的属性（Property）
 * Name 类型名 FullName 类型的完全限定名   NameSpace 类型的名称空间
 * BaseType 类型的直属基类 UnderlyingSystemType 该类型在.NET运行库中的映射类型
 *
 * System.Type类---反射的核心类
 * 抽象基类 获取指向任何给定类型的Type引用（3）
 * 1.使用C#的Typeof运算符、is、as
 * 2.使用继承自System.Object的GetType()方法
 * 3.使用Type类的静态方法Type.GetType（）
 * note:这里返回的type引用仅仅是实例的一个数据类型，不含实例的任何数据信息
 *
 * Type的属性（Property）
 * Name 类型名 FullName 类型的完全限定名   NameSpace 类型的名称空间
 * BaseType 类型的直属基类 UnderlyingSystemType 该类型在.NET运行库中的映射类型
 * 自定义特性
 * 在运行期间处理和检查代码
 * 自定义特性将自定义数据元素与程序元素关联
 * 自定义特性类
 * 1.直接或者间接派生自System.Attribute
 * 2.包含控制特性用法信息（AttributeUsage()）:应用目标元素AttributeTargets、是否可以复用AllowMultiple、是否可以继承Inherited、以及其他参数等
 *
 * AttributeUsage特性 - 元特性 - 只应用到其他特性上
 * 必须参数 AttributeTargets 枚举值 - 指定特性可以应用的元素类型
 * 包括：All - 所有元素、Assembly - 程序集、Class - 类、Constructor - 构造器、Delegate - 委托 、Enum - 枚举、
 *          GenericParameter - 通用参数、Interface - 接口、Method - 方法、Module - 模块、Parameter - 参数、
 *          Property - 属性、Return Value - 返回值、Struct - 结构
 * 将特性应用到程序元素上时，应将特性放在元素前的方括号中[]
 * 对于Assembly 和Module 可以放在任何位置，但需要前缀
 * [Assembly:someAttribute(...)]
 * [Module:someAttribute(...)]
 *  AllowMultiple - 是否对同一对象复用
 *  Inherited - 是否可以被继承
 *
 * 反射
 * System.Type 该类可以访问关于任何数据类型的信息；
 * System.Reflection.Assembly 访问给定程序集的相关信息；
 *
 * System.Type类
 * 抽象基类
 * 获取任何给定类型的Type引用
 *      1.使用typeof运算符 Type  t = typeof(obj);
 *      2.使用GetType()、所有类都会从基类继承该方法，Type t = obj.GetType();
 *      3.使用Type类的静态方法GetType() Type t = Type.GetType（obj）；
 *
 * 可用的属性都是只读的：可以使用Type确定其数据类型，但不能修改该类型；
 * Type的属性
 * 1.Name - 数据类型名
 * 2.FullName - 数据类型的完全限定名（包括namespace）
 * 3.Namespace - 定义数据类型的命名空间
 *
 * 引用
 * 1.BaseType - 该Type的直接基本类型
 * 2.UnderlyingSystemType - 该类型在.net运行库中映射的类型
 *
 * 方法
 * GetConstructor()、GetConstructors() - ConstructorInfo
 * GetEvent（）、GetEvents（） - EventInfo
 * GetField（）、GetFields（） - FieldInfo
 * GetMethod（）、GetMethods（） - MethodInfo
 * GetProperty（）、GetProperties（） - PropertyInfo
 * GetMeber（）、GetMembers（）、GetDefaultMembers（） - MemberInfo - 返回所有成员信息
 *
 * System.Reflection.Assembly类
 * 访问给定程序集的元数据
 *   1.加载程序集
 *       使用静态成员 Assembly.Load()以及Assembly.LoadFrom()
 *       Assembly.Load() - 参数为程序集名 - 运行库会在本地目录、全局程序集缓存中查找并加载该程序集
 *       Assembly.LoadFrom() - 参数为程序集完整的路径名，不会搜索
 *
 *   2.获取程序集所有定义的类型信息
 *   Assembly.GetTypes() - 返回该程序集内所有的类型的System.Type引用数组；
 *
 *   3.获取自定义特性详细信息
 *   静态方法 Assembly.GetCustomAttributes() - 所有的特性都做一般特性Attribute获取，自定义特性需要显示转换
 *   对于方法属性等的特性，需要MethodInfo、PropertyInfo等的GetCustomAttributes()
 *   重载 GetCustomAttributes(Type-要搜索的属性类型，bool-是否搜索其继承链)
 *   GetCustomAttribute（） - 返回一个特性，若没有则返回null ，若存在两个以上则抛出异常 System.Reflection.AmbiguonsMatchException
 *   对于属性PropertyInfo 存在 方法用于设置、获取该属性值
 *   void SetValue（object-属性对象，object-要设置的值，object[]- 索引化属性的可选索引值,异一般为null）
 *   object-返回属性值 GetValue（object-属性对象，object[]- 索引化属性的可选索引值,异一般为null）
 *
 *
*/

/* 程序集
 * 什么是程序集
 * .net应用程序部署的基本单元
 * 程序集的功能：
 *      1.自描述
 *      2.版本的依赖性在程序集的清单中进行记录
 *      3.可以并行加载，运行同一程序集的不同版本在同一进程中加载
 *      4.应用程序使用应用程序域确保其独立性
 *      5.安装简单
 *      6.
 *
 * 程序集结构
 *      组成：描述本身的程序集元数据（程序集元数据）、描述导出类型和方法的元数据（类型元数据）、MSIL代码、资源
 *      文件：可以是一个文件也可以是多个文件
 *
 * 程序集清单
 *      元数据的一部分，用于描述程序集以及引用程序集所需信息，并列出所有依赖关系
 *
 * 组成：
 *      1.标识 - 名称、版本、文化、公钥
 *      2.属于该程序集的文件列表
 *      3.被引用程序集列表
 *      4.一组许可请求
 *      5.导出类型
 *
 * 名称空间与程序集
 * namespace 与 assembly 完全独立。
 * 一个namespace可以存在于多个程序集
 * 一个程序集也可以包含多个namaspace
 *
 * 私有程序集与共享程序集
 *      可能存在命名冲突
 *      共享程序集 - 程序集必须是唯一的，必须有一个唯一的名称，即强名
 *
 * 附属程序集 - 只包含资源的程序集
 * 查看程序集 - 使用工具ildasm
 *
 * 构建程序集
 *      创建模块和程序集
 *      模块 - 一个没有程序集特性的dll
 *
 * 程序集特性
 *      AssemblyInfo.cs - 配置程序集清单
 *      assembly:  - 将特性标记为程序集级别的特性
 *
 * 创建和动态加载程序集
 *      Assembly类静态方法Load（）静态方法，动态加载程序集
 *      Assembly.Load(AssemblyName)
 *      动态编译程序集：
 *      使用Microsoft.CSharp下的CSharpCodeProvider类，可以从DOM树、文件、源代码中生成程序集
 *
应用程序域 - ApplicationDomain
 *  多个应用程序可以在同一个进程的多个应用程序域中运行。
 *  程序集加载到应用程序域中。
 *  一个进程可以有多个应用程序域，一个应用程序域可以加载多个程序集。
 *  应用程序域中的程序集应只加载一次 - 最小化占用内存
 *  实例与静态成员不能在应用程序域之间共享，也不能访问另一个应用程序域的对象；访问需要代理Proxy
 *  AppDomain类用于创建和终止应用程序域，加载、卸载程序集和类型、枚举应用程序域中的程序集和线程；
 *  卸载程序集只能通过终止应用程序域进行；
 *  跨应用程序域访问对象：
 *          1.首先该对象必须派生自基类：MarshalByRefObject
 *          2.实例化一个代理
 *          3.通过应用程序域之间的通道访问该类
 *
 * AppDomain 的GetAssembies（）查看该应用程序域所有已加载的程序集
 *
 共享程序集
 *
 *  强名
 *  1.共享程序集名必须是全局唯一的，且必须可以保护该名称
 *  2.任何其他人不能使用同一名称创建程序集
 *  强名组成：
 *  1.程序集本身名称
 *  2.版本号
 *  3.公钥 - 保证强名独一无二
 *  4.文化
 *  程序集必须有一个强名，唯一标识
 *  公钥不能用作信任密钥，可以使用Authenticode建立信任关系；
 *  Authenticcode签名的公钥可以与强名的公钥不同；
 *          使用命名空间层次给类命名
 *          强名中使用公钥/私钥对，不能访问私钥就不能破坏该程序集
 *
 * 使用强名获得完整性
 *
 * 创建共享组件必须使用公钥/私钥，编译器将公钥写入程序集清单中
 * 创建属于该程序集文件散列表，并用私钥签名
 * 私钥不在文件中，使用公钥对签名进行验证
 *      客户端引用共享程序集：
 *      1.客户端将共享程序集的公钥写入程序集清单中 - 为了压缩空间，仅将公钥标记写入清单，即公钥标记在公钥散射表中的最后8个自己，且唯一；
 *      2.运行期间加载共享程序集 - 共享程序集的散射表可以使用存储在客户端的=程序集的公钥验证
 *
 * 保证完整性 - 只要共享程序集来自期望的发布商，即完成了完整性
全局程序缓存 - global assembly cache - GAC
 * 全局可以使用的程序缓存；
 *  <windows> - 系统windows目录，一般指C:\windows目录
 * 位置：<windows>\Mircsoft.Net\assembly 下
 * GACxxx - 共享程序集
 * GAC_32 - 专用于32位平台程序集
 * GAC_64 - 专用于64位平台程序集
 *
 * <windows>\assembly\NativeImages_<runtime version>  - 编译为本机代码的程序集
 *
 * 实用工具 gacutil.exe
 *
 创建共享程序集
 *      1.创建程序集 - 类库
 *      2.创建强名 - 使用强名工具 sn  使用sn - vs2010命令提示符 - sn -k path/file.snk
 *      3.添加强名文件 - 右键 - 属性 - 签名 - 为程序集签名
 *      4.安装共享程序集 - 使用gacutil /i 名安装 若已安装 也可以使用 /f 强制安装 gacutil /i my.dll /f
 *      5.使用共享程序集 - 添加引用，可以选择该引用 属性 复制到本地 为false
 *
程序集的延迟签名
 *      延迟签名，签名文件不会存储在程序集中，但会预留空间
 *
 * 延迟签名：
 *      1.使用sn创建密钥文 snk - sn -k path\file.snk
 *      2.提取公钥 - sn -p source.snk output.snk 提取source.snk中的公钥到output.snk文件中，output.snk中仅含公钥
 *              ，这个文件就可以使用延迟签名编译测试
 *      3.关闭签名的验证功能 - sn -Vr Target.dll
 *      4.发布前，重新签名 - sn -R Target.dll source.snk
 *
 引用
 * GAC中的程序集可以包含关联的引用；
 *
 *      设置程序集引用：
 *      1.使用gacutil 和 /r 参数  /r需要 一个引用类型、一个引用ID、一个描述；
 *      2.引用类型枚举值：uninstall_key、filepath、opaque；
 *              uninstall_key ： MSI使用，定义注册表键后卸载使用该项；
 *              filepath：指定一个目录；
 *              opaque：允许设置任意类型的引用类型；
 *      从GAC删除程序集：
 *      1.使用gacutil /u 参数，需要删除该程序集的所有引用
 *          eg： gacutil /u Target   /r fliepath "" description
本机映像生成器 - Ngen.exe
 * 可以在安装期间将IL代码编译为本地代码（native code），可以运行较快，即不需要再Runtime中进行编译，减少程序启动时间
 * 本机映像缓存 在本机映像缓存中安装本机映像，本机映像的缓存的物理目录<windows>\assembly\NativeImages<RuntimeVersion>
 * vs2010 dos上 ngen
 *
配置.net应用程序
 * 使用XML文件配置应用程序；
 *
 * 配置类别
 *      1.启动设置 - 使用<startup> 元素配置，指定运行库版本
 *      2.运行库设置 - 指定运行库如何进行垃圾回收，以及进行程序集绑定 ，指定代码策略 代码库（code base）
 *      3.wcf设置
 *      4.安全设置
 *
 *  配置文件：
 *      1.应用程序配置文件 - 应用程序的特定设置，在可执行文件同目录下，且同名，添加config后缀，asp.net 为web.cofig
 *      2.计算机配置文件 - 系统范围的配置，可以指定程序集绑定和远程配置，位置%runtime_install_path%\config\Machine.config
 *      3.发行者策略文件 - 由组件的创建者指定共享程序集可与旧版本兼容，存储在全局程序集缓存中
 *
 * 私有程序集必须位于应用程序所在目录或者子目录下，进程Probing用于查找私有程序集，若私有程序集没有强名，则Probing不使用版本号
 * 共享程序集可以  安装在GAC中、放在一个目录、网络共享、Web站点上；
 *
 绑定程序集 - binding
 *
 * 共享程序集 可以安装在GAC中使用，若应用程序间共享程序集，但不使用GAC，则可以指定一个共享目录，将程序集放在其中；
 * 查找程序集：
 *      1.使用XML文件的codebase元素 - 只用于共享程序集
 *      2.使用XML文件的Probing元素 - 可用于私有或共享程序集
 *
 *      <codeBase> - 应用程序配置文件 - config文件
 *      <codeBase>特性：
 *          version - 指定程序集的原始引用版本
 *          href - 定义加载程序集的目录 eg：网络加载“http://...”，文件目录“file://...”等
 *
 *      <probing>
 *      若没有配置 codeBase，程序集不在GAC中，运行库利用probing查找
 *      特性： privatePath - 运行库应在应用程序根目录下的子目录中查找；不能为应用程序根目录外的目录，否则必须有一个共享名
 *
 版本问题
 *
 * 版本号：
 *      <Major>.<Minor>.<Build>.<Revision> - <主版本号>.<次版本号>.<内部版本号>.<修订版本号>
 *      <Major>.<Minor>.*
 *      * : 自动生成；
 *          内部版本号：自2000年1月1日来的天数
 *          修订版本号：自当地时间的午夜开始的秒数除以2
 *
 * 通过编程方式获取版本
 *      程序集反射 - 程序集的FullName 包含类名、版本、位置、公钥标记；
 *
 * 绑定到程序集版本 - 版本重定向 - 应用程序
 *      使用配置文件重新定位绑定程序集的另一个版本，重定向程序集
 *      <assemblyIdentity> 元素指定 程序集名称、文化、公钥标识
 *      重定向 使用<bindingRedirect> 特性 oldVersion 指定旧版本范围  1.1.0.0~1.2.0.0 ，newVersion 指定新版本号
 *
 * 发行者策略文件 - 版本重定向 - 共享程序集
 *      发行者策略文件只能用于安装在GAC中共享程序集，若共享程序集出错，更新新版本时
 *      1.对于每一个使用该程序集的应用程序进行重定向
 *      2.使用发行者策略使应用程序重定向
 *
 *      建立发行者策略：
 *      1.创建发行者策略文件 - 把已有版本重定向到新版本的XML文件，语法与配置文件相同，可以使用配置文件，即config文件
 *      2.创建发行者策略程序集 - 使用程序集连接器al，将发行者策略文件添加到生成的程序集中
 *              生成的程序集名称必须以policy开头，其后是应重定向的程序集的主次版本号以及共享程序集文件名，且必须与原程序集使用相同的密钥文件
 *              eg： al /linkresource:mypolicy.config /out:policy.1.0.ch19_SharedAssembly.dll /kefile:<filePath>/ch19_AssemblyKey.snk
 *      3.把发行者策略程序集添加到GAC
 *
 * 重写发行者策略 - 禁用发行者策略
 *      禁用发行者策略，应用程序配置文件的 <publisherPolicy>元素的 apply = “no”即可禁止使用发行者策略，可以自己重定向程序集
 *
 * 运行库版本
 *      配置文件中重定向运行库版本 <supportedRuntime>
 *      可以定义多个，以指定运行库版本的使用优先级
 *      <supportedRuntime version= "" sku=""> - version - 运行库版本；sku - .net Framework版本
 *       sku 值：可以在注册表查询：hklm\software\microsoft\.netframework\v4......\skus
 *
不同技术间共享程序集
 * 1.共享源代码
 *      可以使用C#预处理器指令 定义 条件编译符
 *      处理不同代码
 *
 * 2.可移植类库
 *      可以共享二进制程序集
 *      MVVM模式（Model - View-ViewModel） - 使用一个中间层将用户界面与数据分隔
 *
 *
 */

/* 诊断 - diagnosis
* System.Diagnosis - 提供跟踪代码、事件日志、性能测试、代码协定
* 获得正在运行的应用程序的实时信息，找出问题，监视资源
*
* 事件查看器 - 监视应用程序运行时错误情况
* 性能监视器 - 监视应用程序所需资源
* 协定分析器 - 按协定设计使用方法签名定义参数类型
*
*
*
*
代码协定
 * 契约式编程 - 需要Code Contract Editor Extensions扩展
 * 功能：有效性判断，抛出异常
 * System.Diagonsis.Contracts 用于静态检查代码和运行时检查代码
 * 可以定义方法中的前置条件、后置条件、变量
 *      前置条件：定义方法参数必须满足的要求
 *      后置条件：定义方法返回值必须满足的要求
 *      变量：定义方法中的变量必须满足的要求
 *
 * 使用Contract类定义，在方法中定义所有协定要求，所有的条件都必须放在方法的开头
 * 可以给ContractFailed事件定义全局处理程序，运行期间失败都会调用该处理程序
 * 调用ContractFailedEventArgs 的SetHandled()方法会禁止失败时的标准行为：抛出异常
 *
 * 前置条件
 *      用于检查方法的传入参数，使用Contract.Requires()方法
 *      Requires()      Requires<TException>
 *      必须传入一个bool值，第二个参数为可选的消息字符串，不满足条件时显示该消息
 *      ForAll - 检查集合的每一项要求
 *      Exists - 检查任意一项是否满足要求
 *
 * 后置条件
 *      Ensure() 和 EnsureOnThrow<TException>()
 *      保证返回某个值 协定使用特殊值 Result<T>  T为返回值类型
 *      比较新旧值，可以使用OldValue<T> 在方法入口传递值
 *      若返回值使用out标识  使用ValueAsResult<T> ()
 *
 * 常量
 *      为对象生命周期中的变量定义协定，总是在方法结尾进行检查
 *      Contract.Invariant() - 定义在对象整个周期都必须满足的条件
 *                  且只能在使用了ContractInvariantMethod特性的方法内使用
 *                  且只能包含 对协定常量检查 的代码
 *
 * 纯粹性
 *      指自定义方法不会修改对象的任何可见状态
 *      是Pure特性标记为纯粹 默认get访问器是纯粹的
 *      应用：协定方法中可以使用自定义方法，但这些方法必须是纯粹的
 *
 * 接口的协定
 *      给接口指定ContractClass特性，并指定参数Type T ，T作为该接口的代码协定
 *      T 为接口代码协定检查类，并且实现该接口
 *      实现该接口的类型，必须满足T类指定的所有代码协定
 *
 *      eg：
 *      [ContractClass(Typeof(T))]
 *      public interface IDiagnosis
 *      {}
 *      [ContractClassFor(Typeof(IDiagnosis))]
 *      public class T:IDiagnosis
 *      {...}
 *
 * 简写
 *      如果某个协定被重复使用，则可以使用一个重用机制，向包含多个协定的方法应用特性ContractAbbreviator
 *      然后就可以在需要此协定的地方使用该方法，不必再重新定义协定
 *
 * 协定和遗留代码
 *      Contract.EndContractBlock() - 指定前面的代码作为协定处理
跟踪 - trace
 * 从正在运行的程序中查看消息
 * 跟踪体系结构主要组成：
 *      1.源 - 跟踪信息的源头，使用源可以发送跟踪消息
 *      2.开关 - 定义要记录信息的级别
 *      3.跟踪侦听器 - 定义写入跟踪信息的位置
 *      4.侦听器 - 定义了要写入的跟踪消息
 *
 * 源 - 跟踪源
 *      TraceSource类写入跟踪消息；
 *      跟踪需要设置Trace标识；
 *      实例化跟踪源 TraceSource
 *      TraceInformation(string message) - 在跟踪输出中写入一条消息型消息
 *      TraceEvent(enum TraceEventType,int Flag,string message) - 在跟踪输出中写入一条消息型消息
 *      Flush() - 将消息写入侦听器，不保存在内存上
 *      Close() - 关闭跟踪源 ， 同时会调用Flush() 方法
 *      TraceEventType 枚举值 ：
 *      Critical - 致命       Error - 错误     Warning - 警告     Information  - 消息       Verbose - 调试
 *
 * 跟踪开关
 *      启用或禁用跟踪消息 - 派生自抽象类Switch，派生类BoolSwitch、TraceSwitch、SourceSwitch；
 *      SourceLevels 枚举值：
 *      Off - 关闭        Error - 错误      Warning - 警告        Info - 消息       Verbose  - 调试
 *
 * 跟踪源和跟踪开关可以在应用程序配置文件中配置：
 * <System.Diagnosis> 下
 *
 *
 * 侦听器
 *      确定跟踪消息的写入位置 - 派生自抽象基类 TraceListener
 *      TextWriterTraceListener -  基于文件的侦听器
 *      XMLWriterTraceListener - 写入XMl文件
 *      DelimitedListTraceListener - 写入有分隔符的文件
 *      EventLogTraceListener/EventProviderTraceListener - 写入事件日志
 *      WebPageTraceListener - 写入web跟踪文件Trace.axd
 *      自定义侦听器
 *
 * 可以通过应用程序配置文件进行配置
 * 跟踪源  -  <sources> 下新建<source name switchName switchType> name - 源名称 switchName - 开关名称 switchType - 开关类型
 * 侦听器 - <source ...> 下 <listeners> <Add name , type , traceOutputOptions , initiallizeData> name - 名称  type - 类型  traceOutputOptions - 枚举
 *              项  initiallizeData - 文件，若侦听器由多个源使用则可以配置为<sharedListeners>
 * 跟踪开关 - <switches> 下<Add name value>  name - 开关名称 value - 开关级别
 *
 * 筛选器
 *      Filter属性 - 筛选去 - 定义侦听器是否写入消息
 *      在侦听器写入消息前，先调用关联的筛选器的的ShouldTrace()方法以确定是否应写入消息
 *      派生自抽象基类 TraceFilter - 派生类
 *      SourceFilter - 指定从特定源中写入消息
 *      EventTypeFilter - 扩展开关功能，根据跟踪的严重程度确定是否写入消息
 *      同样可以在配置文件中配置
 *      <linsteners><Add><filter  type initiailizeData > ... ...
 *
 * 相关性
 *      查看不同方法的关系
 *      查看跟踪事件的调用栈、逻辑调用栈 - 配置 - 使用XML侦听器跟踪调用栈 - 配置traceOutputOptions
 *      服务跟踪查看器工具 - svctraceviewer.exe
 *
 * 使用ETW进行跟踪
 *      Event Tracing for Windows 进行快速跟踪
 *      使用ETW时，将EventProviderListener配置为侦听器
 *      1.实例化EventProviderTraceListener 需要使用一个唯一的GUID - 使用GUID工具生成
 *      2.启动跟踪会话 - 使用logman命令  logman start mysession -p {<guid>} -o mytrace.etl -ets
 *              start - 启动一个新会话 -p - 定义提供程序的名称 -o - 定义输出文件  -ets - 把命令发给事件跟踪系统
 *              结束跟踪会话 logman stop mysession -ets
 *      3.日志文件  mytrace.etl  是二进制格式，需要转换 使用工具 tracerpt  -of - 指定目标文件格式 xml csv evtx
 *              tracerpt mytrace.etl -o mytrace.xml -of xml
 *
 * 使用EventSource
 *      新的跟踪类，完全基于ETW - System.Diagnostics.Tracing
 *      使用：
 *      1.创建派生自EventSource的派生类
 *      2.并调用基类中的WriteEvent方法，写入消息
 *      3.跟踪消息  - 进程外 - 使用logman、prefView读取跟踪消息、
 *          进程内部访问跟踪消息 - 使用EventListener基类，重写OnEventWirtten()方法，并把跟踪消息传递给参数EventWrittenEventArgs
 *
 * 使用EventSource进行高级跟踪
 *      默认情况下，事件源与类名相同，使用EventSource特性可以改变名称和唯一标识符；
 *      每个事件方法都可以标记Event特性 - 定义事件的ID、opcode、跟踪级别、自定义关键字、任务等
 *      侦听器通过Event特性的属性值过滤事件
 *
 *
 *
 *
 事件日志 - EventLog
 *      系统事件查看器 - %windir%\system32\eventvwr.msc /s
 *      写入事件日志 - EventLogTraceListener 类
 *
 * 事件日志体系结构
 *      事件日志存放在几个日志文件中：应用程序、安全性、系统日志
 *      事件日志服务的注册表配置 - HKLM/system/CurrentControlSet/Services/EventLog
 *      系统日志 - 记录系统驱动程序和设备驱动程序消息
 *      应用程序日志 - 记录消息 - 应用程序和服务 - 注册表配置位置 - HKLM/system/CurrentControlSet/Services/EventLog/Application/<ApplicationName>
 *      安全性日志 -  应用程序的只读日志
 *
 * 事件日志类
 *      两个API
 *      1.System.Diagnostics.Eventing - 用得少
 *      2.System.Diagnostics  - 主要介绍此类
 *      System.Diagnostics 下的类
 *      1.EventLog - 读写日志中的项，将应用程序建立为事件源
 *      2.EventLogEntry - 表示事件日志中的一项，可使用EventLogEntryCollection遍历所有的EventLogEntry
 *      3.EventLogInstaller - EventLog组件的安装程序
 *      4.EventProviderTraceListener - 将跟踪信息写入事件日志
 *
 * 1.创建事件源 - Create Event Source
 *      可以使用EventLog类或者EventLogInstaller类的CreateEventSource方法（EventLogInstaller调用Eventlog类的CreateEventSource（）方法）
 *      创建事件源 需要管理员权限
 *      EventSourceCreationData -  表示用于在本地或远程计算机上创建事件日志源的配置设置
 *      事件源名称 - 应用程序标识符
 *      日志文件名称 - 将日志信息写入应用程序日志中，也可以自己创建日志 - 目录 -<windows>\System32\WinEt\Logs
 *
 * 2.写入事件日志
 *      使用EventLog类的WriteEntry()或者WriteEvent（）
 *      EventLog类有连个WriteEntry（）方法；
 *      1.静态WriteEntry（）
 *      2.实例WriteEntry（）
 *
 *
 * 资源文件 - 消息资源文件
 *      在资源文件中定义消息，并将消息标识传递给WriteEntry()
 *      支持本地化
 *      消息资源文件 - 本地文件 - 严格定义消息 - 文本文件 - 扩展名为.mc
 *      1.使用消息编辑器mc.exe创建一个二进制位文件
 *      2.编译消息文件 - mc -s msgNameMessage.mc ,得到一个.bin文件和一个Messages.rc文件（包含对资源文件的引用）
 *      3.使用资源编译器rc.exe创建资源文件 - rc msgNameMessage.rc，得到资源文件msgNameMessage.res
 *      4.使用连接器将二进制消息文件msgNameMessage.res与本地dll关联 link /dll /subsystem:windows /noentry /machine:x86 msgNameMessage.res
 *      5.定义资源文件 检查事件源是否存在--验证资源文件是否可用--将消息dll复制到应用程序目录--该目录使用SpecialFolder的枚举值ProgramFiles指定
 *              --若共享消息资源文件则放在Environment.SpecialFolder.CommonFiles中
 *      6.实例化EventSourceCreationData类
 *      7.定义资源文件引用 ： ESCD类的属性CatagoryResourceFile、MessageResourceFile、ParameterResourceFile
 *      8.CreateEventSource（）注册新的事件源和日志文件
 *      9.EventLog类RegisterDisplayName（） - 指定日志在日志查看器中显示的名称
 性能监视
 * 性能监视器 - PerfMon
 * 性能监视类
 *      System.Diagnostics
 *      1.PerformanceCounter - 监视计数和写入计数
 *      2.PerformanceCounterCategory - 查看所有已有的类别，创建新类别
 *      3.PerformanceConunterInstaller - 安装性能计数器
 *
 * 性能计数器生成器
 *
 */

/*  任务 线程 同步
 *  线程是程序中的独立的指令流；
 *  线程包含资源：window句柄、文件系统句柄、其他内核对象（每个线程都分配虚拟内存）
 *  一个进程至少包含一个线程
 *  线程具有：
 *  一个优先级、处理程序的位置计数器、一个存储局部变量的栈
 *  每个线程都有自己的栈，程序的内存和堆由一个进程的所有线程共享
 *  同一进程内的线程间通信 -- 线程寻址相同的虚拟内存地址
 *  进程管理的资源：
 *      1.虚拟内存
 *      2.window句柄
 *  任务并行与数据并行
 *      任务并行：使用CPU的代码被并行化
 *      数据并行：使用数据集合
 Parallel类 Parallel - 平行、并行
 *
 *  对线程的抽象 - System.Threading.Tasks - 提供数据和任务的并行
 *  并行静态方法：for-foreach
 *      Parallel.Invoke() - 用于任务并行
 *      Parallel.ForEach() - 用于数据并行
 Parallel.For()循环
 *      并行迭代，但迭代顺序没有定义
 *      第一个参数 int 循环开始
 *      第二个参数 int 循环结束
 *      第三个参数 Action<int>
 *      返回值 ParallelLoopResult 结构，IsCompleted属性标识循环是否正常结束
 *      Task.Delay() - 异步方法 用于释放线程供其他使用
 *      For没有延迟等待
 *      Parallel只等待创建他的任务不等待其他后台活动
 *
 * 主线程结束所有的后台线程都会终止
 * 提前停止Parallel.For
 *      重载Parallel.For(int begin，int end，Action<int,ParallelLoopState>)
 *      ParallelLoopState
 *      Break()或者Stop()- 停止并结束For()
 *
 * Parallel.For<TLoacl>(int begin,int end ,)
 * For<TLocal>(int fromInclusive, int toExclusive
 * , Func<TLocal> localInit     -   用于返回每个线程的本地数据的初始状态的函数委托,每个线程进行初始化
 * , Func<int, ParallelLoopState, TLocal, TLocal> body - 将为每个迭代调用一次的委托，第三个参数 接受Init的返回值，第四个参数本次循环的返回值
 * , Action<TLocal> localFinally - 用于对每个线程的本地状态执行一个最终操作的委托，仅对每个线程调用一次，为线程的退出方法
 * );
Parallel.ForEach()
  *     以不定序,异步,遍历IEnumerable集合
Parallel.Invoke() - 调用多个方法
 *      提供了多个任务并行运行
 *      允许传递一个Action数组委托，并在其中指定方法
 *      Parallel.Invoke(Action[])
 *
任务 - Task
 *  任务表示应完成的某个工作单元
 *  工作单元
 *      1.可以在单独的线程中运行
 *      2.可以以同步方式启动一个任务，等待主调线程
 *    使用任务可以获得一个抽象层，对底层线程进行控制
 *
 * 启动任务
 *      使用TaskFactory类或Task类的构造函数或Start()方法
 *
 * 使用线程池的任务
 *      线程池 - 提供了一个后台线程的池
 *      线程池独立管理线程 - 线程池中的线程完成后，返回线程池
 *
 *      创建任务：
 *          1.使用实例化的TaskFactory类，把TaskMethod方法传递给StartNew,即可立即启动任务
 *          2.使用Task类静态属性Factory访问TaskFactory，然后调用StartNew
 *          3.使用Task构造函数，参数为Action，完成后不知立即执行，而是处于Created状态，需要调用Start
 *          4.使用Task类静态方法Run，传递Lambda表达式 - .Net 4.5
 *
 * 同步任务
 *      同步运行，以相同的线程作为主调线程
 *      实例Task类的 RunSyncronsly()
 *
 * 使用单独线程的任务
 *      若任务运行时间过长，可以指定参数 TaskCreationOptions.LongRunning 会创建一个新线程，而不是使用线程池中的线程，该线程不由线程池管理
 *      eg： var th = new Task(Action,name,TaskCreationOptions.LongRunning )
 *
 * Future --- 任务的结果
 *      任务结束时：
 *      1.将状态信息写入共享对象中 - 共享对象必须是线程安全的
 *      2.使用返回某个结果的任务 - future - 可以定义任务返回结果的类型 - Task类的泛型版本
 *      Task<TResult>()
 *
 * 连续的任务
 *      通过任务指定在任务完成后，应运行的另一个任务
 *      连续处理程序有一个Task参数
 *      连续任务通过在任务上调用ContinueWith(Action)后缀TaskFactory类
 *
 * 任务层次结构
 *      父/子层次 - 一个任务内启动了另一个任务
 *      取消父任务，就取消了子任务
 *      父任务状态：若父任务先于子任务结束，则其状态为WaitingForChildrenToComplete，所有任务结束变为RanToCompletion
 *      可以指定TaskCreationOption中的DetachedFromParent创建子任务；

取消框架
 *  允许以标准方式取消长时间运行的任务 - .Net4.5
 *  取消框架基于协作行为；长时间运行的任务会检查自身是否被取消，并返回控制权
 *  支持取消的方法接受CancellationToken参数，并定义了IsCancellationRequested属性 - 检查此属性
 *  长时间运行任务检查取消方式：
 *          1.取消标记时，使用WaitHandle属性
 *          2.使用Register()方法 - Register方法接受Action参数和ICancelableOperation参数，
 *              Action - 取消标记时调用，ICancelableOperation - 接口，实现该接口的对象有一个Cancel()方法，在取消时调用该方法
 *
 * Parallel.For() - 取消
 *      For() 重载中有一个ParallelOptions参数，该类型参数接受一个CancellationToken属性对象，CancellationToken由CancellationTokenSource创建
 *      且CancellationTokenSource有一个Cancel方法（实现ICancelableOption）
 *      cts.Token.Register(Action) - 将要取消时调用的方法
 *
 * 任务的取消
 *      1.Create a CancellationTokenSource also use Task.Factory.CancellationToken
 *      2.set Action cts.Token.Register(Action)
 *      3.....
线程池 - ThreadPool
 *      创建线程需要时间
 *      TheradPool类在需要时增减池中的线程数，直到最大线程数，ThreadPool.SetMaxThreads(int-最大工作线程数，int-最大IO线程数)
 *      若线程超过极限，则新任务需要等待
 *      Thread.QueueUserWorkItem(Action-WaitCallBack)
 *      线程池收到创建新线程的请求后，从线程池中选择一个线程来执行该方法
 *      若线程池没有运行，则新建一个线程池，并启动第一个线程
 *      若线程池已经运行且存在一个空闲线程，则将该任务传递给此线程
 *
 *  线程池使用限制：
 *          1.池内所有线程都为后台线程，若前台线程都结束则后台线程将停止，不能将入池的后台线程修改为前台线程
 *          2.不能给线程池中个线程设置优先级和名称
 *          3.对于COM对象，入池的所有线程都是多线程单元（multi-threaded apartment -MTA），许多COM对象需要都需要单线程单元（single-thread apartment STA）
 *          4.入池的线程只能用于短时间任务，若需要一直运行，则可以使用Thread类或者Task类的LongRunning
 *
 *      对象池：
 *      1.初始化对象池
 *      2.使用池 - 向对象池中请求一个对象
 *      3.判断对象池是否有空闲对象
 *          4.若有空闲对象，则判断该空闲对象是否可用
 *              4.1.若该对象可用，则使用该对象，该对象使用完后返回对象池
 *              4.2.若该对象不用，则销毁该对象，并试图创建新对象，判断对象池是否已满
 *                  4.2.1.若对象池已满，则延迟申请对象
 *                  4.2.2.若对象池未满，则新建对象，然后使用该对象，该对象使用完后返回对象池
 *          5.若无空闲对象，则判断对象池是否已满
 *                  5.1.若对象池已满，则延迟申请对象
 *                  5.2.若对象池未满，则新建对象，然后使用该对象，该对象使用完后返回对象池
Thread类
 *      创建和控制线程，可以创建前台线程，设置线程的优先级
 *      Thread构造函数允许接受
 *          ThreadStart参数 - 委托定义了 返回类型为void的无参数方法
 *          ParameterizedThreadStart -
 *       使用Start方法启动线程
 * 给线程传递数据
 *      两种方式：
 *      1.使用带参数的构造方法ParameterizedThreadStart，构造线程实例 - 线程的入口点必须有Object参数且返回值为void - Action（Object）
 *      2.创建一个自定义类，将线程的方法定义为类的一个实例方法
 *
 * 后台线程
 *      只要有一个前台进程在运行，应用程序的进程就在运行；若多个前台进程在运行，而Main方法结束了，应用程序进程就是激活是，直到所有的前台进程完成
 *      默认下，Thread创建的进程都是前台进程，线程池创建的进程都是后台进程
 *      Thread类设置属性IsBackground确定进程的前后台
 *      后台线程适于完成后任务
 *
 * 线程的优先级
 *      线程由操作系统调度，线程的优先级影响线程的调度顺序，调度优先级最高的早CPU上运行
 *      线程等待资源：
 *      如果线程不主动释放CPU资源，线程调度器抢占该线程；线程有一个时间量，即可以持续使用CPU直到时间量内
 *      若优先级相同的多个线程等待使用CPU，则线程调度器使用循环调度机制，将CPU逐个交个各个线程，若线程被其他线程抢占，则会排在队列最后
 *      只有多个优先级相同的线程使用CPU时，才使用时间量和循环调度机制
 *      优先级是动态的：
 *      1.若线程是CPU密集型的，则其优先级降低为定义时的基本优先级
 *      2.若线程在等待资源，则其优先级会升高
 *
 *      Thread类设置Priority属性影响线程的基本优先级 - ThreadPriority枚举值
 *
 * 控制线程
 *      Thread实例化后，调用Start方法，该线程不会立即执行，等待线程调度器调度执行（状态为Unstarted）
 *      运行时状态为Running，Thread.ThreadState查看当前线程状态
 *      Thread.Sleep - 线程处于WaitSleepJoin状态
 *      Thread.Abort - 停止线程 AbortRequest-》Aborted
 *      Thread.Join - 停止当前线程，直到加入的线程完 - WaitSleepJoin
线程问题
 *      争用条件
 *      多个线程访问相同的对象，且对共享状态没有同步
 *      避免该问题：
 *      1.锁定共享对象 - 使用lock关键字 lock（resouce）{ 。。。} - 只有引用类型才可用于锁定 - 保证每个线程都有锁定对象
 *      2.将共享对象设置为线程安全对象 - 对象本身使用同一个对象来锁定
 *
 * 死锁
 *      至少有两个线程被挂起等待对方解锁，只要锁定块没有结束，就不会解除锁定
 *      解决：
 *          1.定义锁定顺序避免死锁出现
 *          2.定义锁定超时，处理死锁情况
同步 - synchronous
 *      避免同步问题 - 不共享数据
 *      同步技术 - 确保一次只有一个线程访问和改变共享状态
 *      可用于同步的技术：
 *      lock语句 - 进程内同步
 *      InterLocked类、Monitor类 - 进程内同步
 *      Mutex类、Event类、SemaphoreSlim类、ReaderWriterLockSlim类 - 多个进程间线程同步
lock语句和线程安全
 *      锁定值类型，只是锁定了一个副本
 *      在lock块最后对象被解锁
 *      锁定静态成员：
 *          把锁放在object类型上    lock（typeof（StaticClass））{  }
 *      使用lock可以将类的实例成员设置为线程安全的 使用 方法属性内使用 lock(this){  }
 *      实例的对象也可以用于外部访问，所以且不能在类中控制访问，需要SyncRoot模式 - 创建一个私有的object，该对象仅用于锁定
 *      使用锁定需要时间，可以创建两个版本
 *      在一个线程使用lock，并不意味其他所有线程都在等待，必须对每个访问共享对象的线程显式使用同步
 *      lock(this) 与lock(obj) - 线程锁定的对象是不是为同一个对象
 *      msdn
 *      lock 确保当一个线程位于代码的临界区时，另一个线程不进入临界区。
 *          如果其他线程试图进入锁定的代码，则它将一直等待（即被阻止），直到该对象被释放。
 *      不能lock null 否则会出现ArgumentNullException
 *      不能锁定值类型，不能锁定null
 *
 * InterLocked类
 *      MSDN：为多个线程共享的变量提供原子操作。
 *      用于使变量的简单语句原子化 - 只能用于简单的同步问题
 *      提供了以线程安全的方式递增、递减、交换和读取值的方法
 *      方法：
 *      Add(int,int) - 相加
 *      CompareExchange<T>(ref T,T,T) - 比较交换
 *      Increment(int) - 递增     Decrement(int) - 递减

 * Monitor类
 *      MSDN：提供同步访问对象的机制
 *      静态类
 *      lock语句由编译器解析为Monitor类
 *      lock（object） - 被解析为调用Monitor.Enter方法，该方法会一直等待直到线程锁定对象为止
 *      一次只有一个线程能锁定对象，解除了锁定，线程进入同步阶段
 *      Monitor.Exit方法解除锁定 - 在finally中
 *      lock(object) {  //synchronized code  }
 *      Monitor.Enter(Object)
 *      try {   //synchronized code  } finally   {   Monitor.Exit(object);   }
 *      优点：添加等待被锁定的超时
 *      Monitor.TryEntry(object,int,ref bool) - object锁定对象、int-超时时长、bool-若obj被锁定则为true，若超时则为false
 *      Monitor.TryEntry(obj，int，bool)
 *      if（bool）
 *      {
 *          try{ //code   } finally { Monitor.Exit(obj)；}
 *      }
 *      else
 *      {       }

 * SpinLock结构
 *      提供一个相互排斥锁基元，在该基元中，尝试获取锁的线程将在重复检查的循环中等待，直至该锁变为可用为止
 *      MSDN：SpinLock 结构是一个低级别的互斥同步基元，它在等待获取锁时进行旋转。
 *              在多核计算机上，当等待时间预计较短且极少出现争用情况时，SpinLock 的性能将高于其他类型的锁。
 *              即使 SpinLock 未获取锁，它也会产生线程的时间片。它这样做是为了避免线程优先级别反转，并使垃圾回收器能够继续执行。
 *              在使用 SpinLock 时，请确保任何线程持有锁的时间不会超过一个非常短的时间段，并确保任何线程在持有锁时不会阻塞。
 *              由于 SpinLock 是一个值类型，因此，如果您希望两个副本都引用同一个锁，则必须通过引用显式传递该锁。
 *              有关如何使用 SpinLock 的示例，请参见 如何：使用 SpinLock 进行低级别同步。
                自旋锁可用于叶级锁定，此时在大小方面或由于垃圾回收压力，使用 Monitor 所隐含的对象分配消耗过多。自旋锁非常有助于避免阻塞，但是如果预期有大量阻塞，由于旋转过多，您可能不应该使用自旋锁。当锁是细粒度的并且数量巨大（例如链接的列表中每个节点一个锁）时以及锁保持时间总是非常短时，旋转可能非常有帮助。通常，在保持一个旋锁时，应避免任何这些操作：
 *              1.阻塞    2.调用本身可能阻塞的任何内容  3.一次保持多个自旋锁，
                4.进行动态调度的调用（接口和虚方法）
                5.在某一方不拥有的任何代码中进行动态调度的调用，或
                6.分配内存。
                SpinLock 仅当您确定这样做可以改进应用程序的性能之后才能使用。另外，务必请注意 SpinLock 是一个值类型（出于性能原因）。因此，您必须非常小心，不要意外复制了 SpinLock 实例，因为两个实例（原件和副本）之间完全独立，这可能会导致应用程序出现错误行为。如果必须传递 SpinLock 实例，则应该通过引用而不是通过值传递。
 *              不要将 SpinLock 实例存储在只读字段中。
 *              传送SpinLock实例时需要注意 - SpinLock为结构是值类型，所以需要通过引用传递SpinLock实例
 *              同样适用Entry、TryEntry、Exit
 *
 * WaitHandle基类
 *      MSDN：封装等待对共享资源的独占访问的操作系统特定的对象
 *      一个抽象基类，用于等待一个信号的设置
 *      使用WaitHandle类，可以等待一个信号的出现 - WaitOne 、等待必须发出信号的多个对象的信号的出现 - WaitAll
 *      等待多个对象中一个信号的出现 - WatiAny - WaitAll和WaitAny是WaitHandle的静态方法 - 接受一个WaitHandle参数数组
 *      派生：基类WaitHandle派生出 Mutex、EventWaitHandle、Semaphore
 *
 * Mutex类
 *  MSDN：用于进程间同步的同步基元
 *  提供跨多个进程同步访问的一个类
 *  类似于Monitor ：只有一个线程能拥有锁定，只有一个线程能获得互斥锁定，访问受互斥保护的同步代码区域
 *  Mutex构造函数：
 *      1.指定互斥是否最初应由主调线程拥有
 *      2.定义互斥名称
 *      3.获得互斥是否已存在信息
 *      eg Mutex mu = new Mutex(bool,string,out bool)
 *
 *  互斥可以在另一个进程中定义，因为操作系统可以识别有名称的互斥，它由不同的进程共享，若未给互斥命名则他是未命名的。不能在进程间共享
 *      打开已有的互斥：Mutex.OpenExisting()
 *      互斥锁定 ： WaitOne()
 *      释放互斥：ReleaseMutex()
 *      禁止应用程序启动两次
 *
 * Semaphore
 *      MSDN：限制可同时访问某一资源或资源池的线程数
 *      备注
        使用 Semaphore 类来控制对资源池的访问。
 *      线程通过调用进入信号量 WaitOne 方法，它继承自 WaitHandle 类，并通过调用释放信号量 Release 方法。
        上一个信号量的计数将减少每次一个线程进入信号量，并当一个线程释放该信号量时递增。
 *      当计数为零时，后续请求阻止，直到其他线程释放信号量。如果所有线程都已都释放信号量，计数是最大值时指定创建信号量。
        没有任何有保证的顺序，如先进先出或后进先出，被阻止的线程在其中输入信号量。
        一个线程可以进入信号量多次，请通过调用 WaitOne 方法重复。
 *      若要释放某些或所有这些项，可以调用该线程的无参数 Release() 多个时间，也可以调用的方法重载 Release(Int32) 方法重载来指定要发布的实体数。
        Semaphore 类并不强制在调用线程标识 WaitOne 或 Release。它是程序员的责任，以确保线程不释放信号量次数太多。
 *      Eg:假定信号量的最大计数为 2 并且线程 A 和线程 B 都进入了该信号量。如果线程 B 中的编程错误将导致它调用 Release 两次，这两个调用都成功。
 *      信号量计数已满，并且当线程 A 最终调用 Release, 、 SemaphoreFullException 引发。
        个信号量分为两种类型：
 *      本地个信号量和已命名的系统信号量。
 *      如果您创建 Semaphore 对象使用的构造函数接受一个名称，它是与该名称的操作系统信号量相关联。
 *      名为的系统个信号量在整个操作系统，都可见，并可用于同步进程的活动。
 *      您可以创建多个 Semaphore 对象来表示同一个已命名系统信号量，并且您可以使用 OpenExisting 方法以打开一个现有的已命名系统信号量。
        仅在您的进程内存在本地信号量。可供您具有对本地的引用的过程中的任何线程 Semaphore 对象。每个 Semaphore 对象是单独的本地信号量。
 *
 *      信号量：可以同时由多个线程使用
 *      是一种计数的互斥锁定
 *      使用信号量限制可以访问 受旗语锁定保护-资源 的线程数
 *      信号量的两个类：
 *      Semaphore和SemaphoreSlim
 *      Semapbore:使用系统范围资源，允许在不同进程间同步
 *      SemaphoreSlim：Semaphore类的轻型优化版
 *      Semaphore构造函数：
 *      Semaphore（int - 最初释放的锁定数，int - 锁定个数计数）
 *      锁定资源：semaphore.WaitOne()
 *      释放资源：semaphore.Release();
 *
 * Events类
 * 与互斥、信号量一样，Events类一个系统范围内的资源同步方法
 * 系统事件：
 * System.Threading 中提供了类：ManualResetEvent、AutoResetEvent、ManualResetEventSlim、CountdownEvent
 * 与关键字event区别
 * event关键字：基于委托
 * Events类：基于.net封装器，用于系统范围本机资源的同步
 * 事件通知任务：事件可以发送信号也可以不发送信号
 * 发送信号：Set（）
 * 不发送信号：Reset（）
 *
 * WaitHandle 类
 *
 * Barrier类
 *      工作有多任务分支且以后需要合并工作
 *      用于需要同步的参与者，激活一个任务，可以动态的添加参与者，参与者继续前可以等待所有其他参与者的完成
 *
 * ReaderWriterLockSlim类
 *  ReaderWriterLockSlim：
 *      MSDN:表示用于管理资源访问的锁定状态，可实现多线程读取或进行独占式写入访问
 *  使锁定机制允许锁定多个读取器访问某个资源，
 *  若没有写入器锁定资源，则可以有多个读取器访问资源，但只能有一个写入器锁定资源
 *  读取锁定：阻塞锁定 - EnterReadLock（）  非阻塞锁定 - TryEnterReadLock（）
 *  写入锁定：EnterWriterLock（） 、TryEnterWriterLock()
 *  升级的读取锁定：EnterUpgradableReadLock（）、TryEnterUpgradableReadLock（） - 获取写入锁定，无需释放读取锁定
 *
 * Timer类

    用于某个时间间隔后调用某个方法
    Timer：- 五个名称空间
    System.Threading    核心功能，构造函数可以传递委托，并按照指定的时间间隔调用该委托，基于委托机制
    System.Timers       一个组件，派生自Component类，使用System.Threading.Timer，但基于事件机制
    System.Windows.Forms    使用System.Timers.Timer和System.Threading.Timer,可以从不是主调线程的另一个线程中调用回调方法或事件
    System.Web.UI           一个可用于Web页面的Axjx扩展
    System.Windows.Threading    WPF应用程序使用的DispatcherTimer类，运行在UI线程上
    System.Threading.Timer
         构造函数 传递委托
         Timer(Action(object) - 回调方法,object - 回调参数,TimeSpan - 第一次回调时间段,TimeSpan - 回调时间间隔，若为-1则表示只调用一次)
    System.Timers.Timer
        构造函数只需要一个时间间隔
数据流
    处理数据流以及并行进行数据转换 - Task Parallel Library Data Flow（TPL Data Flow）
    作为一个NuGet包安装 - Microsoft.Tpl.Dataflow
    使用动作块
    TPL Data Flow的核心是数据块，这些数据块作为提供数据的源或者接受数据的目标，或者同时作为源和目标

  源与目标数据块
    每个块都实现了IDataflowBloc接口，该接口包含一个返回Task的属性Completion以及Complete和Fault方法
    Complete方法后，块不接受任何输入以及输出；调用Fault方法将块状态置为失败状态
    作为目标块 - 实现ITargetBlock接口，派生自IDataflowBlock,定义OfferMessage方法 - 发送一条由块处理的信息
    作为源块 - 实现ISourceBlock接口，提供链接到目标块以及处理消息的方法
    ActionBlock - 目标块 - 提供数据流块，其调用为每个接收的数据提供的 Action<T> 委托
    BufferBlock - 源、目标块 - 提供用于存储数据的缓冲区

连接块
    TransformBlock - 源块和目标块，通过使用委托来转换源 - 泛型
    eg：var trans = new TransformBlock<TInput - 输入类型, TOutput - 输出类型>(Func<TInput,TOutput> - 转换方法-委托、lambda表达式 )
    trans.LinkTo(TOutput) - 连接块,将结果传入连接的目标块 还可以对消息进行筛选

    BroadcastBlock - 向多个目标传递输入源
    JoinBlock - 将多个源连接到一个目标
    BatchBlock - 将输入作为数组进行批处理
    DataflowOptions - 可以配置块 - 配置最大项数，配置取消标记

    主题
    about_NuGet

    简短说明
    提供有关 NuGet 程序包管理器命令的信息。

    详细说明
    本主题介绍 NuGet 程序包管理器命令。NuGet 是一种集成的程序包
    管理工具，用于将库和工具添加到 .NET 项目。

    包括以下 NuGet cmdlets。

    Cmdlet					说明
    ------------------		----------------------------------------------
    Get-Package				获取已安装的程序包。使用 -ListAvailable，
    获取可从程序包源获得的程序包。

            Install-Package			将程序包及其依赖项安装到项目中。

            Uninstall-Package		卸载程序包。如果其他程序包依赖此程序包，
    除非指定 –Force 选项，否则此命令将失效。

            Update-Package			将程序包及其依赖项更新到更新的版本。

            Add-BindingRedirect		检查项目输出路径中的所有程序集
    并视需要将绑定重定向添加到应用程序（或 web）
    配置文件。

            Get-Project				为指定的项目返回对 DTE（开发工具环境）的引用
    如果未指定，则返回
    在程序包管理器控制台中选定的默认项目。

    Open-PackagePage        打开指向指定程序包的 ProjectUrl、LicenseUrl 或
    ReportAbuseUrl 的浏览器。

    Register-TabExpansion	为命令参数注册选项卡扩展。

几条规则
    1.尽力是同步要求低 - 避免共享状态 避免同步
    2.类的静态成员应是线程安全
    3.实例状态通常不需要线程安全 -   最好在类的外部进行数据同步，且不对类的每个成员进行数据同步

*/

/*  安全性
        应用程序的安全性
        1.应用程序用户
            用户进行身份验证；用户授权；

        2.应用程序本身
            web程序对于服务器危害
     * 身份验证与授权
           身份验证 - 标识用户
           授权 - 用户访问特点资源

        标识和Principal
            使用标识验证运行应用程序的用户 WindowsIdentity - windows用户
            实现IIdentity接口，该接口可以访问用户名、用户是否通过身份验证、验证类型等
            principal - 一个包含用户标识和用户所属角色的对象
            IPrincipal接口，定义Identity属性 - 返回IIdentity对象；定义IsInRole()方法 - 验证用户是否为指定角色
            角色 - role - 用相同安全权限的用户集合，用户管理单元
            .NET中的Principal类有WindowsPrincipal、GenericPrincipal、RolePrincipal 派生于ClaimsPrincipal，可以自定义派生于ClaimsPrincipal类
            导入命名空间 System.Security
            System.Security.Claims System.Secutiry.Principal
            .NET默认只能使用通用的Principal填充主体

        角色
           基于角色的安全访问用于解决资源访问问题
        声明基于角色的安全性
            调用接口IIdentity接口的IsInRole()方法或使用属性，实现基于角色的安全性请求
            在类或方法级别可以使用特性PrincipalPermissional声明权限要求

        声称
            Principal类的Claims属性

        客户端应用程序服务
            ASP.NET 使用基于System.Web.Security的MemberShip和Roles类
            MemberShip类 - 验证、创建、删除、查找用户，修改用户密码等
            Roles类 - 添加删除角色，给用户授予角色，修改用户角色等
            角色和用户的存储位置取决于提供程序
            对于客户端应用程序，.NET4.5.1提供程序：
            ClientFormAuthenticationMemberShipProvider
            ClientWindowsAuthenticationMemberShipProvider

        应用程序服务
            用户 - MembershipProvider
            角色 - RoleProvider
            在配置文件中配置应用程序身份验证服务

        客户端应用程序
    加密
        对称密钥 - 使用同一个密钥进行加密与解密
        非对称密钥 - 使用公钥和私钥，公钥加密-私钥解密，或者，私钥加密-公钥解密
        公钥私钥总是成对创建的
        公钥 - 可以由任何人使用
        私钥 - 必须进行安全加锁
        对称加密与解密算法比非对称算法速度快
        网络通信一般先使用非对称加密进行密钥互换，然后使用对称密钥进行加密通信

        .NET加密 - System.Security.Cryptography
        类以Cng前缀或后缀 - Cryptography next generation
        不是以Cng、Managed、CryptoServiceProvider后缀的都是抽象基类
        Managed - 表示用托管代码实现
        CryptoServiceProvider - 实现了抽象基类的类
        Cng - 用于利用新Cryptography CNG API的类

        散列算法 - 从任意长度的二进制字符串中创建一个长度固定的散列值 - 用于和数字签名保证数据的完整性 - MD5 SHA SHA512
        对称密钥算法 - 使用相同的密钥进行加密和解密 - DES AES
        非对称算法 - 使用不同的密钥进行加密和解密 - RSA DSA ECDSA
签名
        首先创建密钥对 - Create()接受一个算法作为参数
           cng= CngKey.Create(CngAlgorithm.ECDsaP384);
           导出公钥使用Export方法导出- cng.Export（）

        获取数据签名
            通过密钥使用SingData（data）获取数据签名
        验证数据签名
            使用Import导入公钥，得到密钥对象
            并使用该密钥生成ECDsaCng类，使用该类的VertifyData（data，dataSignture）验证数据签名
 交换密钥和安全传输

 资源的访问控制
        资源通过资源访问控制列表（ACL）来保护
        资源--关联--安全描述符
        描述符--包含拥有者信息，以及两个访问控制列表--DACL-自由访问控制列表，SACL-系统访问控制列表
        DACL - 确定访问权限
        SACL - 确定安全事件日志审核规则
        ACL - 包含访问控制项ACE
        ACE- 包含类型、安全标识、权限
        System.Security.AccessControl - 读取和修改访问控制权限

        一般，.NET中的类方法GetAccessControl()返回相应的Security类-安全描述符
        GetAccessControl()方法以AuthorizationRuleCollection类的形式返回DACL
        GetAuditRules() - 访问SACL

        GetAccessRules(bool,bool,Type) - type 为返回安全标识符类型，必须派生自基类IdentityReference
            一般为NTAccount和SecurityIdentifier 都表示用户或者组
            NTAccount - 按照名称查找安全对象
            SecurityIdentifier按照唯一安全标识符查找安全对象

        AuthorizationRuleCollection 类包含AuthorizationRule对象 - 改对象的ACE在.net中的表示

        更改ACE权限
            1.实例化NTAccount类 - 参数为用户组-选择用户
            2.实例化ACE实例 -参数为NTAccount类型以及权限参数-生成ACE权限
            3.构建Security类实例，添加ACE-添加ACE权限
            4.指定对象SetAccessControl(object,Security)-赋予ACE权限
代码访问安全性
        指定代码能做什么
        .net4中添加第2级安全透明性 - 区分允许授权调用的代码和不允许授权调用的代码
        分为3类
            1.security-critical - 安全第一-代码可以运行任意代码，这种代码不能由透明代码调用
            2.safe-critical - 安全重要 - 代码可以由透明代码调用，安全验证由此执行
            3.transparent - 代码可执行操作有限，只能运行在指定的权限集中，不能包含不安全和无法验证的代码，不能调用Security-critical代码

        第2级安全透明性
            使用securityRules注释程序集指定SecurityRuleSet值
            eg:[assembly:SecurityRules(SecurityRuleSet.Level2)]
            设置SecurityTransparent特性，标注改程序集为不能执行不安全代码以及授权操作，只能调用其他透明代码或者safe-critical代码
            eg：[assembly:SecurityTransparent()]
            设置AllowPartiallyTrustedCallers特性，在透明代码与其他类之间，允许个别类或成员有其他特性
            eg：[assembly:AllowPartiallyTrustedCallers()]
            默认Security-critical特性代码若无其他特性，可以给个别类或成员设置SecuritySafeCritiacl特性以允许透明代码调用
            eg：[assembly:SecurityCritical()]
 权限
        允许或禁止代码组执行的动作
        预定义权限  自定义权限
        .net权限 - 独立于系统权限，由CLR验证

        权限集
            权限的集合
            把权限赋予代码组就不需要单独处理每个程序集
            已命名的权限列表：7个
            FullTrust - 无权限限制
            SkipVerification - 跳过认证
            Excetion - 运行受保护的资源，但不能访问
            Nothing - 无任何权限，代码不能执行
            LocalIntranet - 指定权限全集的一个子集
            Internet - 未知来源代码默认策略
            Everything - 授予权限集合中的所有权限，除了忽略验证权限

        使用沙盒API包含未授权的代码
            使用沙盒API可以创建一个包含不完全受信任的权限的应用程序
            1.实例化权限集类型 PermissionSet
            2.添加权限类型 PermissionSet.AddPermission（）
            3.获取当前应用程序域配置信息AppDomainSetup
            4.创建一个新的应用程序域
                CreateDomain（string-名称，Evidence-证据，AppDomainSetup-应用程序域设置信息,PermissionSet-权限集合）
            5.创建新应用程序域实例ObjectHandle - CreateInstance（string-程序集名，string-类型名）
            6.返回被包装的对象ObjectHandle.Unwrap（）；
            7.该对象转换并执行代码

        隐式权限
            当赋予某些权限时，也会隐式的赋予其他权限
            检查隐式权限 - 检查权限集A是否为权限集B的子集
               bool PermissionA.IsSubSetOf(PermissionB)
使用证书发布代码
        使用数字证书对程序集进行签名，软件验证身份
        工具：
            1.软件发布者证书测试工具 - Cert2spec.exe
            2.pvk2pfx.exe
            3.signtool.exe
            4.证书管理器-certmgr 或MMC插件Certificates
        1.创建证书和密钥文件
            控制台 makecert -sv name密钥文件名.pvk -r -n "CN=ABC Corporation" name证书文件名.cer
        2.
    */

/* 互操作
    .net和com技术
    .net和com不需要重新以前的代码模块
    com技术 - 定义了一个允许使用不同编程语言编写组件的组件模型
元数据
    com中，组件的所有信息都存储在类型库内
    可以使用没有类型库的组件，但必须使用c#代码重新定义COM接口

    释放内存
        COM依赖于引用计数，所有的COM对象都必须实现IUnknown接口（提供3个方法），其中两个涉及计数的方法:AddRef(增加计数)/Release（减少计数）
        计数为0时，对象销毁自身，释放内存

    接口
        定义了组件提供的方法和客户端可以使用的方法
        1.自定义接口 - 脚本客户端不能使用
            派生自IUnknown，在一个虚拟表（vtable）中定义了各个方法的顺序，客户端从而可以直接访问接口的方法
            将vtable绑定到方法-通过内存地址实现 - 因此脚本客户端不能使用自定义接口

        2.调度接口 - 脚本客户端可以使用，但是效率很低
            IDispatch，客户端总能使用，派生自IUnknown，提供4个方法，其中GetIDsOfNames和Invoke
            需要两个表 GetIDsOfNames - 将方法或属性名称映射到一个调度ID；Invoke - 将调度ID映射到方法或属性名称
            客户端调用组件的方法A -》 GetIDsOfNames 获取改方法的ID-》客户端使用该ID-》Invoke-》使用对于方法
            一般这两个表存储在类型库中

        3.双重接口 - 脚本客户端可以使用
            派生自IDispatch，提供了额外的方法

        4.强制转换和QueryInterface

方法绑定 function binding
    客户端映射到方法的方式
    后期绑定 - 运行期间查找要调用的方法，通过反射System.Reflection，只对调度接口和双重接口有效
    前期绑定 - 直接使用vtable（vtable绑定）- 只应用与自定义接口和双重接口；ID绑定-调度ID存储在客户端代码内，运行期间只需要调用一次Invoke即可
                GetIDsOfNames在设计期间调用的，对此不能改变调度ID
数据类型
    对于调度接口和双重接口，在COM中只能使用的数据类型仅限于一组自动兼容是数据类型
    IDispatch接口的Invoke接受一个variant类型的一个数组作为参数，variant是许多类型的联合，eg：byte、short、long、.net中用object
注册 - registration
    com中通过注册表所有的组件都是全局可用的
    所有的COM对象都有一个128位的唯一标识 - 类ID（CLSID）
    创建COM对象的COM API调用CoCreateInstance，在注册表中查找CLSID和DLL或exe的路径加载DLL或启动exe、初始化组件
    COM对象还有一个ProgID - 一个容易记的名称映射到CLSID
    每个接口和类型库都有一个唯一标识符-IID以及typelib ID
线程
    COM使用单元模型处理线程问题
    1.单线程单元 single-threaded apartment STA
        只有一个线程（创建实例是线程）能访问组件，一个进程中可以由多个STA
        在STA中不需要考虑资源同步问题，COM已经保证了只有一个线程可以方法该组件实例
        线程安全的COM对象会在注册表中将注册表项ThreadingModel设置为Apartment即需要使用STA

    2.多线程单元 multi-threaded apartment MTA
        多个线程可以同时访问组件

    3.线程中性单元 - thread neutral apartment
        只能用于配置为com+应用程序的COM组件
    ThreadingModel 值：
    Apartment - 需要STA-单线程单元-不需要考虑线程安全
    Free - 需要MTA-多线程单元-需要考虑线程安全
    Both - 不明确是否需要考虑线程安全 - STA MTA TNA
错误处理
    在COM中对于错误处理，通过方法中返回hresult值来定义错误
    hresult为S_OK表示成功
    或者事项ISupportErrorInfo
事件
    COM事件，组件必须实现IConnectionPointContainer接口和一个或者多个实现了IConnectionPoint接口的连接点对象
    （connection point object CPO），组件还定义了由CPO调用传出接口ICompletedEvents
    客户端必须在sink对象上实现这个传出对象，在运行期间客户端向COM端口查询接口IConnectionPointContainer
封送
    封送机制-从.net到COM组件的数据以及从COM到.net组件的数据必须转换为对应的表示形式 - 具体操作取决于数据的类型能否按位块传输
在.net客户端使用COM组件

    1.创建COM组件-C++ ATL项目 添加ATL类

    2.创建运行库可调用包装（runtime callabel wrapper，RCW）-使用RCW，.net看到的不是COM组件而是.net对象
        tlbimp
    3.使用RCW-添加名称空间以及System.Runtime.InteropServices，需要使用Marshal类来释放对象 这里RCW只会调用一次AddRef无论对RCW引用了多少次
        COM对象使用的是本地内存堆，而.net对象使用的是托管内存堆，垃圾回收只针对托管堆

    4.通过动态语言扩展使用COM服务-借助dynamic以及COM绑定器，可以不用创建RCW对象调用COM组件
        通过Type.GetTypeFromProgID（string） 获取Type对象，使用Activator.CreateInstance（Type） 创建dynamic对象

    5.线程问题-线程必须连接一个单元，特性[STAThread]-STA，[MTAThread]-MTA 定义了线程应连接的单元
        若没有应用特性默认连接一个MTA，也可以使用Thread类的ApartmentState属性设置单元状态，但只能设置一次，会忽略第二次设置
        若线程选择了不支持的单元，COM运行库自动为COM组件创建正确的单元，若调用组件方法越过单元边界则性能会下降

    6.添加连接点-
在COM客户端中使用.NET组件
    1.COM可调用包-CCW-将.NET组件封装为COM可用的对象
    2.创建.net组件
    3.创建类型库COM互操作特性 tlbexp工具
    4.COM注册
    5.
平台调用
    使用平台调用 p/Invoke - 可以重用只包含导出函数而不包含COM对象的非托管库，CLR会加载应该调用的函数的并封送参数的DLL
    使用非托管函数：
        1.确定该函数在导出时的名称 - 使用dumpbin工具,列出所有的导出函数
            dumpbin /export <dll path>/name.dll | more
        2.调用本机函数-必须定义具有相同数量的参数的C#外部方法，并且在非托管代码中的定义的参数类型能够映射到托管代码中的参数类型
            windows API中，LP-long指针、C-常量、STR-以NULL结尾的字符串，T-泛型
            声明方法需要使用extern 且使用特性[DllImport("")] /[MarshalAs(UnmanagedType)]-定义类型映射
            [return:MarshalAs(UnmanagedType)] - 定义返回值的类型映射
            DllImport 属性以及字段
                EntryPoint - 允许C#在声明非托管库中的函数时使用不同的名称，非托管库中的方法的名称定义在改字段中
                CallingConvention - 定义调用约定（根据非托管函数），如何处理参数以及保存在栈的位置，也可以自己定义调用约定
                CharSet - 设置定义管理字符串的方式 ANSI、UNICODE、AUTO
                SetLastError - 读取设置的错误号 - Marshal.GetLastWin32Error
        3.遵循原则
            1.创建内部类，包含平台调用
            2.创建公有类，为应用程序提供本机方法
            3.使用安全特性标记必要的安全性

        Windows句柄 - 一个32位值，.NET1.0使用IntPtr类型表示，.NET2.0使用SafeHandle表示 - windows句柄抽象基类

*/

/* 文件和注册表
    文件-System.IO
    注册表-System.Win32
    串行化-将数据转换为字节流并存储在某个地方的过程 - System.Runtime.Serialization
管理文件系统
    System.MarshalByRefObject - 远程操作基对象类，允许在应用程序域之间编组数据
    FileSystemInfo - 表示任何文件系统对象的基类
    FileInfo、File - 文件系统中的文件
    DirectoryInfo、Directory - 文件系统中的文件夹
    Path - static 处理路径名
    DriveInfo - 驱动器信息
表示文件和文件夹的类
    Directory和File
        都是静态类，可以对文件（夹）进行操作省去实例化的系统开销
    DirectoryInfo和FileInfo
        属性表：
        CreationTime - 创建时间
        DirectoryName - 包含文件夹的完整路径 - FileInfo
        Parent - 指定子目录的父目录 - DirectoryInfo
        Exists - 指定文件或文件夹是否存在
        Extension - 扩展名 - 文件夹返回空白
        FullName - 完整路径名
        LastAccessTime - 最后一次访问时间
        LastWriteTime - 最后一次修改时间
        Name - 名称
        Root - 根路径 - DirectoryInfo
        Length - 文件大小 - FileInfo
        Function
        Create() - 创建文件流或文件夹
        Delete() - 删除
        CopyTo（） - 复制
        MoveTo（）- 移动
        GetDirectories（） - 获取所有包含的文件夹
        GetFiles（） - 获取包含的所有的文件
        EnumerateFiles（）
        GetFileSystemInfos（）
Path类
    静态方法，执行对路径的操作
读写文件
    读写文件
        File.ReadAllText()
        File.ReadAllLines()
        File.ReadAllBytes()
        File.WriteAllText()
        File.WriteAllLines()
        File,WriteAllBytes()
流

    一个用于传输数据的对象，两个传输方向：
    读取流：数据从外部源传输到程序
    写入流：数据从程序传输到外部源
    外部源：网络协议、命名管道、文件、内存
    内存读写 - System.IO.MemoryStream
    网络 - System.Net.Sockets.NetworkStream
    读写管道 - System.IO.Stream 抽象类

    流可以在数据类型之间转换
    FileStream - 文件流，用于读写二进制文件中的二进制数据
    StreamReader、StreamWriter - 读写文本文件
    BinaryWriter、BinaryReader - 提供其他流对象的包装器、对二进制数据进行额外的格式化
        code --> BinaryWriter --> 底层流对象(与其他数据源交互) --> BinaryReader --> code
    基本流与一般流的区别

    基本流按照字节工作

    缓存流
        读写文件时
        请求读/写文件流，流将读/写请求发给windows，操作系统将会连接文件系统，定位文件，并从磁盘中读取文件的一个大块
        ，保存在一个内存区域-缓冲区，请求就会从缓冲区读取数据，直到该缓冲区读完，系统读取下一个大块

    使用FileStream读取二进制文件
        FileStream（string - FilePath，FileMode-打开模式，FileAccess-访问方式，FileShare-共享方式）
        也可以从FileInfo实例中创建文件流
        流使用完毕应该关闭
        流读写方法
        FileStream.ReadByte()/FileStream.Read(byte[],int,int)
        WriteByte()/Write()

    读写文本
        使用StreamReader和StreamWriter
        优势：
            1.读写方法可以按照行进行并且流自动确定下一个回车符位置，写文本自动追加回车符和换行符
            2.不要考虑文本的编码方式ASCII或Unicode - 字节码标记-确定编码方式

        StreamReader类
映射内存文件
    System.IO.MemoryMappedFiles
    把文件的一部分或者全部加载到一段虚拟内存中，文件内容显示给应用程序,可以实现资源共享
    使用映射内存文件
        1.映射内存的文件实例  MemoryMappedFiles.CreateFromFile()
        2.访问器对象     memoryMappedFiles.CreateViewAccessor()
读取驱动器信息
    DriveInfo类
文件的安全性
    ACL - 访问控制表 - 文件、目录、注册表键
    System.Sercurity.AccessControl

    从文件读取ACL
        FlieStream -> FileSecurity =  fileStream.GetAccessControl ->fileSecurity.GetAccessRules()
        获取到文件的ACL列表 - FileSystemAccessRule

    从目录读取ACL
        DirectoryInfo->DirectorySecurity = directoryInfo.GetAccessControl()->directorySecurity.GetAccessRules()
         获取到目录的ACL列表 - FileSystemAccessRule

    添加删除ACL项
        FileSecurity.AddAccessRule（newRule）
        FileSecurity.RemoveAccessRule（newRule）
        FileSecurity.SetAccessControl(...)
读写注册表
    注册表 - 包含windows安装、用户首选项、已安装软件、设备所有配置信息的核心存储库
    访问注册表-Registry RegistryKey
    namespace - Microsoft.Win32

    注册表
        注册表配置单元 - registry hive 最顶层的节点
            7个配置单元 register只能看到5个
            HKCR- hkey_classes_root - 系统上文件细节（文件类型、文件关联）以及COM注册
            HKCU- hkey_current_user - 当前登录用户计算机配置信息、桌面设置、系统变量、系统环境、网络等
            HKLM- hkey_local_machine - 所有安装到计算机的软件和硬件信息，包括HKCR - HKLM/software/classes
            HKUSER- hkey_users - 所有用户的用户首选项，以及HKCU
            HKCF- hkey_current_config - 包含计算机上所有的硬件信息
            hkey_dyn_data - 一个容器，存储注册表中易丢失数据
            hkey_performance_data - 与运行程序性能相关的信息

    注册表只有键 - 键包含数据和其他键以及默认值，每个值都有值名称、数据类型、数据
        三种数据类型
            REG_SZ - 类似于字符串
            REG_DWORD - 类似于int
            REG_BINARY - 类似与byte[]

    注册表类
        Registry - 简单的访问，可以访问配置单元，用于定位，提供表示配置单元的RegistryKey实例
        RegistryKey - 注册表键的实例，可以进行增删查改操作，实例化过程 - 打开对应键
        从Registry的静态属性开始
        定位键
            GetValue() / SetValue() - 读写值
            CreateSubKey（） - 新建子健
            OpenSubKey（） - 打开子健
            Delete（） - 删除当前键
读写独立存储器
    独立存储器 - 存储应用程序状态以及用户设置
    独立存储器 - 一个虚拟磁盘，只能保存创建其的应用程序或其他应用程序实例共享数据项
    两种：
        1.由用户以及程序集访问 - 计算机的存储位置，可以由多个应用程序实例访问，通过用户身份以及应用程序身份保证
        2.由用户、程序集以及域访问 - 每个应用程序实例都有一个自己的存储器

    使用：
        存储信息
        1.构建独立存储器类 IsolatedStorageFile实例，使用IsolatedStorageFile静态方法CreateXXX创建
        2.创建独立存储文件流 IsolatedStorageFileStream 指定存储文件名、文件模式、共享模式
        3.向流中写入信息 ； 关闭流
        读取信息
        1.构建独立存储器类 IsolatedStorageFile实例，使用IsolatedStorageFile静态方法CreateXXX创建
        2.获取文件流信息
        3.读取流信息 - 新建IsolatedStorageFileStream实例
        4.StreamReader读取信息
*/

/* 事务 Transcations
        特征：任务要么全部完成，要么都不完成，出现一次失败可以进行数据回滚
        System.Transcations
        文件系统和注册表也支持事务处理
        使用：
        1.写入、更新数据库
        2.不稳定的、基于内存对象的执行事务处理
        3.消息队列写入消息
        概述
            事务由事务管理器来管理和协调；
            每个影响事务结果的资源都由一个资源管理器来管理；
            事务管理器与资源管理器通信，以定义事务的结果；

        事务处理阶段
            事务分为3个阶段：
            1.激活阶段：创建事务 - 资源管理器在事务管理器中登记
            2.准备阶段：每个资源管理器定义事务结果，并向事务管理器发出已准备完成指令
            3.提交阶段：所有的资源管理器都准备好了，则开始等待事务管理器发出提交指令，
                事务管理器发出提交指令后，所有资源管理器完成事务中的工作，并返回已提交指令

        ACID属性
            A - atomicity 原子性，表示一个工作单元，在事务中要么整个任务都完成，要么都不完成
            C - consistency 一致性，事务开始前的状态和事务完成后的状态必须有效，事务执行过程中可以有临时值
            I - isolation  隔离性，并发的事务独立于状态，状态在事务处理过程中可以发生变化
            D - durability 持久性，事务完成后必须可以持久的存储，中断必须可以重启恢复
            并非每个事务都具有这4个属性
            事务确保结果永远不处于无效状态

        数据库和实体类
传统事务
        ADO.NET事务
            传统的ADO.NET，若没有创建事务，则每一天SQL语句都是一个事务
            SqlConnection定义了BegionTranscation方法返回一个SqlTranscation事务类
            将事务要处理的命令关联到该类上
            事务完成 Commit ，事务出错回滚 RollBack

            命令->事务->连接（一个连接的本地事务）

            ADO.NET不支持跨多个连接 的事务，不是分布式事务，很难是多个对象参与一个事务

        System.EnterpriseServices
            自动事务处理
            不需要显示声明事务，运行库自动创建事务，只需要给需要事务处理的类添加特效【Transcation】
            【AutoComplete】特性标记方法为自动设置事务状态
            多个对象可以运行在一个事务中，事务自动登记
            需要COM+主机模型
            且使用此特性的类必须派生自基类ServicedComponent
System.Transcations
        提供事务相关的TranscationXXXX类
        Transcation是所有的事务处理类的基类 - 定义所有事务类的属性、方法、事件
        CommittableTranscation - 唯一一个支持提交的事务类 Commit(),其他所有的事务类都只能回滚
        DependentTranscation - 用于依赖其他事务的事务，依赖的事务可以依赖从可以提交的事务中创建的事务
        SubordinateTranscation - 和分布式事务协调器（DTC）使用-表示非根事务，由DTC管理

        Transaction属性
            currrent - 静态属性，返回一个环境事务处理
            IsolationLevel - 事务的隔离级别
            TransactionInformaion  - 返回提供当前事务状态信息实例类
            EnlistVolatile()、EnlistDurable（）、EnlistPromotableSinglePhase - 登记参与事务处理的资源管理器
            Rollback - 回滚
            DependentClone - 依赖克隆，创建一个依赖当前事务的事务
            TranscationCompleted - 当前事务完成事件

        可提交的事务
            唯一支持提交的类CommittableTranscation
        事务处理的升级
            根据本地参与事务的资源管理器，创建本地事务或分布式事务
            若将多个资源添加到事务中，事务可以先设置为本地事务，然后升级为分布式事务
            事务升级需要启动分布式事务协调器DTC 升级失败需要验证DTC是否启动
            Component Services MMC 插件查看系统所有DTC状态

        依赖事务
            使用依赖事务可以影响来自多个任务或线程的某个事务，依赖事务会依靠另一个事务，并影响事务的结果
            依赖事务可以调用Complete或RollBack方法来定义事务结果
            如果根事务结束，且所有的依赖事务都将成功位置为true则提交事务，如果某个依赖事务调用RollBack回滚
            设置终止位，则整个事务都将终止
            DependentClone(DependentCloneOption)
            DependentCloneOption -
                RollbackIfNotComplete - 依赖事务没有在根事务提交前调用Complete，事务终止
                BlockCommitUntilComplete - 根事务等待所有依赖事务定义的结果

        环境事务
            优点：不需要手动用连接登记事务，在支持环境事务的资源中，自动实现
            环境事务与当前线程关联
            使用静态属性，可以使用静态属性Transcation.Current获取和设置环境事务
            创建环境事务：
                1.实例化CommitTranscation对象，将其赋予Transcation.Current属性，以初始化环境事务
                2.使用TranscationScope类 - TranscationScope类的构造函数会创建一个环境事务
                    TranscationScope类的重要方法 - Complete()和Dispose()
                        Complete() - 设置事务的作用域的成功位
                        Dispose（）- 完成该作用域，若该作用域与根作用域相关，则提交或回滚事务
                    TranscationScope类实现了IDispose接口，所以可以使用Using定义作用域
                    创建了TranscationScope类后就可以使用Transcation.Current的get访问器访问事务
                获取事务完成信息，可以设置环境事务TranscationCompleted事件设置OnTranscationCompleted()方法

        嵌套的作用域和环境事务
            使用TranscationScope类可以嵌套作用域，
            嵌套作用域可以：
                1.直接位于原作用域中
                2.位于原作用域调出的方法中
                嵌套的作用域可以使用与外层作用域相同的事务、抑制事务、创建独立于外层作用域的事务
                作用域的要求通过TranscationScopeOption枚举定义，将该值传递给TranscationScope的构造函数
                    TranscationScopeOption
                        1.Required - 作用域需要一个事务，若外层作用域有一个事务，则内层作用域使用同一个已有的事务，
                            若外层作用域没有事务则新建一个事务；若内外层共享一个事务，则这两个作用域都回影响这个事务结果
                            ，即只有所有作用域成功置位，事务才会提交，若根作用域被删除前存在一个没有调用Compelte方法的作用域，则事务终止

                        2.RequiresNew - 总是需要一个新作用域，若外层存在一个作用域，则两个作用域互不影响
                        3.Suppress - 无论外层作用域是否包含事务，新作用域都不会包含环境事务

        GUID一个包含128位唯一值的全局唯一标识符

        多线程和环境事务
            一个环境事务绑定一个线程，若新建一个线程就不会有原有线程的环境事务
            要在另一个线程中使用同一个环境事务，需要使用依赖事务
            给新线程传递一个依赖事务，该依赖事务由环境事务生成-参数设置为BlockCommitUntilComplete
            在线程方法中调用Transcation.Current的set访问器将环境事务设置为传递的依赖事务
            新线程通过作用域通过依赖事务来使用同一个事务，使用完依赖事务需要调用Complete方法
隔离级别
        ACID
        脏读 - 锁定记录、不可重复读 - 锁定读取、幻影读 - 锁定范围
        隔离级别
        IsolationLevel
            ReadUncommitted - 事务不会相互隔离，不等待其他事务释放锁定记录 - 出现脏读
            ReadCommitted - 等待其他事务释放对记录的写入锁定 ,
                    为要读取的当前记录设置读取锁定，为要写入的当前记录设置写入锁定，直到事务完成为止 - 出现不可重复读

            RepeatableRead - 为读取的记录设置锁定直到事务完成 - 幻影读仍可能出现
            Serializable - 范围锁定，运行事务时不能添加与所读取的数据位于同一范围的数据
            Snapshot - 对实际数据建立快照
            Unspecified - 提供程序另一个隔离级别值，不同于IsolationLevel枚举值
            Chaos - 类似于ReadUncommitted，不能锁定更新的记录

        设置隔离级别
            TranscationScope类的构造函数 TranscationOption设置

自定义资源管理器
        资源管理器，管理稳定的资源，管理不稳定的内存中的资源
        资源管理器实现了IEnlistmentNotification接口
        该接口定义了Prepare()/InDoubt()/Commit()/Rollback()
        管理资源的事务，作为事务的一部分，必须使用Transcation登记
            不稳定的资源管理器调用EnlistVolatile()
            稳定的资源管理器调用EnlistDurable()

        Prepare - 事务管理器调用Prepare（）准备事务
        InDoubt - 事务管理器调用Commit()出现问题，就调用InDoubt（）

        事务资源
            事务资源必须保存实际值和临时值;
            实际值从事务的外部获取，并定义了事务回滚时的有效状态
            临时值定义了事务提交时事务的有效状态
            使非事务类型转变为事务类型，定义了泛型类Transcational<T>封装了一个非泛型类型
            在Transcational类中
                托管资源实际值包含在变量liveValue中，
                临时值存储在ResourceManager<T>中，
                enlistedTranscation与环境事务相关联
                在环境事务中调用该构造函数，就调用帮助方法GetEnListment() - GetEnListment()检查是否有一个环境
                事务，并断言是否没有一个环境事务，若没有登记事务，就实例化ResourceManager<T>辅助类，并调用EnlistVolatile()
                用该事务登记资源管理器，并将enlistedTranscation设置为环境事务

            Transcational<T>使用的资源管理器仅限于Transcational<T>本身，即作为一个内部类实现
            深层复制 - 将对象序列化到流中，并从流中反序列化到对象
            只要序列是可以序列化的，Transcational<T>就可以将非事务类型转换为事务类型
文件系统事务
        基于文件的资源管理器，可以复制原文件到一个临时目录，将对文件的修改写入一个临时目录，使这些改变永久保存
        提交事务时，原始文件用临时文件代替
        对于文件系统和注册表系统，新增了Windows API调用，共同之处需要一个句柄将事务作为变量传递，且都不支持环境事务
        调用本地方法时，本地方法参数必须映射带.net数据类型，SafeHandle基类用以映射本地Handle类型
        SafeHandle类型一个抽象基类，包装了操作系统句柄，并支持句柄资源的关键终止操作
        根据句柄的允许值可以使用派生类SafeHandleMinusOneInvalid和SafeHandleZeroOrMinusIsInvalid包装本地句柄
        将句柄映射到事务上
*/

/* 网络编程
System.Net
System.Net.Sockets

HpptClient类
    System.Net.Http - 简化在客户端以及服务器上使用Web服务
    发送Http请求，接受请求的响应
    派生自System.Net.Http.HttpMessageInvoker类
        SendAsync()-异步发送

异步调用Web服务
    HtttpClient对象是线程安全的，即一个HttpClient对象可以用于处理多个请求，每个实例都维护自己的线程池，HttpClient
    实例之间的请求会被隔离
    GetAsync(string)-传递要调用的方法的地址，返回一个HttpResponseMessage对象
    HttpResponseMessage对象，包含标题、状态、内容的响应

标题
    HttpClient的DefaultRequestHeaders属性可以设置或改变标题
    可以使用Add方法给集合添加标题，添加标题后，标题和值都会随着每一个请求一起发送
    DefaultHeaders属性返回HttpRequestHeaders对象
    HttpHeader类是HttpRequestHeaders和HttpResponseHeaders的基类，HttpHeader类对象定义为
    KeyValuesPair<string,IEnumerable<string>>,每个标题集合中可以有多个值
    修改值：删除原值，添加新值
HttpContent
响应中的Content属性放回一个HttpContent对象
读取数据
    1.ReadAsStringAsync - 返回数据字符串
    2.ReadAsByteArrayAsync - 返回数据字节数组
    3.ReadAsStreamAsync - 返回数据流
    4.LoadIntoBufferAsync - 将数据加载到内存缓存中

Headers属性返回HttpContentHeaders对象
HttpMessageHandler
HttpClient类可以将HttpMessageHandler作为构造函数参数，用于定制请求
默认使用WebRequestHandler对象
HttpClientHandler - Http处理消息的抽象基类
1.线程调用IE 导航到指定页面
2.使用WebBrowser控件
WebRequest和WebResponse类结构
WebRequest和WebReponse都是抽象基类，该类提供了处理Web请求和响应的通用功能，且独立于给定操作所需的协议
请求总是通过某一项特定的协议（http、ftp、smtp）实现，并为该协议编写派生类所需的处理请求
实用工具类
URL
    Uri和UriBuilder是System的类
    Uri - 分析、组合、比较Uri
    UriBuilder - 从字符串中构建Uri

IP地址和DNS名称
    IP地址，客户端或服务器上的唯一标识
        IP4-32位值
        IP6-64位值
    发送网络请求，必须将主机名翻译为IP地址，翻译工作由一个或多个DNS服务完成
    DNS服务器上保存一个映射表将主机名映射为IP地址或其他DNS的IP地址，请求通过IP地址进行定位
    IP地址类
        IPAddress - IP地址，与字符串形式相互转换
    IPHostEntity - 用于封装与某台特定的主机相关的信息
    Dns类 - 与默认的DNS服务器通信，检索IP地址
较低层协议
System.Net.Sockets
协议栈
STMPClient
    通过STMP协议传递邮件信息

使用TCP类
    传输控制协议-连接和发送两个端点之间的信息
    端点-包含IP地址与端口号
    已有的协议已经定义好了端口号：
        http-80，STMP-25，
        Internet地址编码分配机构将端口号赋予已知的服务，除非使用某个已知的服务，否则使用大于1024的端口号
    TcpClient类封装了TCP连接，并提供属性控制连接，以及读写GetStream（）请求NetworkStream对象
    TcpListener类用于侦听引入的TCP连接，连接请求达到时，使用AcceptSocket（）-返回一个套接字，与远程计算机通信

TCP和UDP
    UPD - 用户数据报协议，无连接的协议
    UPD类
        可以将通信端点作为Send()以及Receive()方法的参数
        回显服务 echo service - 端口7接受TCP或UPD协议，回显服务将发送给服务器的数据再发送回客户端
            用于诊断和测试

    Socket类
        提供网络编程的最高控制
        构造函数需要为TCP协议的流套接字指定IP寻址方案
        包含许多方法用于异步接受、连接、发送、接收数据，通过委托回调使用这些方法

    WebSocket协议
        用于完全双工的双向通信，一般在浏览器和Web服务器之间，仅交流支持WebSocket的客户端
        WebSocket维持一个打开的连接，TCP发送的是字节流，WebSocket在服务器和客户端之间来回发送消息
        并非所有的浏览器都支持WebSocket协议
        服务器导游ASP.Net4.5的IIS8支持低级的WebSocket协议
        WebSocket端点 - 可以使用任意类型的处理程序或模块构建
网络编程
1.尽可能一直使用最通用的类
2.代码应利用新的应用程序级别的协议
*/

/* windows服务
需求：
1.不需要用户交互
2.需要权限比交互的用户权限更大的用户权限

服务示例
    Simple TCP/IP Services - 驻留小型TCP/IP服务器的服务程序
    World Wide Publishing Service - IIS服务
    EventLog - 消息记录到事件日志系统服务
    Windows Search - 在磁盘上建立数据索引服务
    SuperFetch - 将常用应用程序和库预先加载到内存中服务，缩短应用程序启动时间

window服务的体系结构
    操作Windows服务
        1.服务程序 - 提供需要是实际功能
        2.服务控制程序 - 将请求发送个服务，eg：开始、暂停、停止、重启
        3.服务配置程序 - 安装服务，将服务复制到文件系统，并将服务信息写到注册表，该注册信息由
            服务控制管理器（SCM）用于启动和停止服务
服务程序-ServiceProgram
1.主函数---服务程序的入口点
2.service-main函数---服务的实际功能，用SCM注册一个处理程序
3.处理程序

服务控制管理器-Service Control Manager -SCM
    作用：与服务进行通信
        SCM ---启动服务进程--->服务
        服务 ---注册service-main--->SCM
        SCM ---service-main--->服务
        服务 ---服务处理程序--->SCM

每一项服务都必须注册一个service-main函数，主函数是服务的入口点
service-main函数的入口点必须用SCM注册

主函数---服务程序的一般入口点，即Main()方法，可以注册多个service-main函数
service-main函数---提供实际的功能
    ，服务必须为每一项服务注册一个service-main函数
    ，服务程序可以在一个程序中提供多个服务
    SCM为每一个应该启动的服务调用service-main函数
    service-main函数的一个重要任务是用SCM注册一个处理程序

处理程序函数---必须响应来自SCM的事件（停止、暂停、重新开始）
服务控制程序---独立于SCM和服务本身
可以控制服务，将控制代码发送给服务，处理程序响应这些事件；
可以询问服务状态，并实现一个自定义控制代码的自定义处理程序
服务配置程序
不能使用xcopy安装服务，服务必须在注册表中配置
必须配置服务程序的用户、服务的依赖关系
Windows服务类
.net Framework中
System.ServiceProcess
1.必须从ServiceBase类继承，才能实现服务，ServiceBase类用于注册服务、响应服务开始和停止请求
2.ServiceController类，用于实现服务控制程序，将请求发送给服务
3.ServiceProcessInstaller和ServiceInstaller类用于安装和配置服务程序
创建windows服务
    新建-选择Windows服务
    Windows服务属性：
    AutoLog --- 将启动和停止服务的日志自动写入事件日志中
    CanPauseAndContinue、CanShutdown、CanStop---指定服务的暂停、继续、关闭、停止请求
    ServiceName --- 写到注册表中的服务的名称，使用该名称控制服务
    CanHandleSessionChangeEvent --- 确定服务是否可以处理终端服务器会话中的改变事件
    CanHandlePowerEvent --- 是否响应低电源事件，并相应的改变服务的行为

ServiceBase类
    ServiceBase类是所有的Windows服务的基类
    Quote派生自ServiceBase类，且使用一个未归档的辅助类System.ServiceProcess.NativeModels与SCM通信，NativeModels类是Win32API调用的包装类
    ServiceBase类是内部类
    SCM启动服务进程
    --->进程中调用Main（）方法
    --->Main()中调用ServiceBase的Run（）方法
    --->Run()使用NativeModels.StartServiceCtrlDispatcher()方法注册，ServiceMainCallbcak（）方法，并将记录写入到事件日志中
    --->SCM调用已注册的ServiceMainCallback()方法
    --->ServieMainCallback（)方法使用NativeModels.ResisterServiceCtrlHandler[Ex]()，在SCM中注册处理程序，并在SCM中设置服务状态
    --->调用OnStart()方法，在OnStart()方法中实现启动代码
    --->如果OnStart()启动成功，则将启动信息写入事件日志中

    处理程序在ServiceCommandCallback()中实现的
    当改变对服务的请求时，SCM调用ServiceCommandCallback()
    --->OnPause()/OnContinue()/OnStop()/OnCustomCommand()/OnPowerEvent()

    调用ServiceBase的静态函数Run()---将SCM引用提供给服务的入口点，服务的主线程处于阻塞状态，等待服务的结束
    Run()函数可以传递多个ServiceBase派生类实例数组作为参数，即多个服务运行在同一个线程内
    且需要初始化多个服务的某些共享状态，共享状态必须在Run()运行前完成
    运行Run()时主线程处于阻塞状态，直到服务进程停止为止，以后的指令在服务结束之前不能执行
    初始化的花费时间不能超过30S，若初始化时间过长SCM认为启动失败
    --->可以在另一个线程中进行初始化，然后事件对象可以用信号通知线程完成工作

    服务启动
        OnStart()方法
        调用OnStart()方法不能阻塞，必须返回给调用者（ServiceBase的ServiceMainCallback()）
        ServiceBase类注册处理程序，并在调用OnStart()后将服务成功启动的消息通知SCM
        OnStop()--停止服务
        OnPause()--暂停服务
        OnContinue()--继续暂停的服务（CanPauseAndContinue属性必须为true）
        OnShutdown()--windows系统关闭时调用（CanShutdown属性必须设置为true）
        OnPowerEvent()--系统电源发生变化时，电源变化信息在PowerBroadcastStatus类型参数中
        OnCustomCommand()--为服务控制程序发送过来的自定义命令提供服务，方法签名int参数用于获取自定义命令编号
            （编号取值128~276，小于128为系统预留）
线程化与服务
1.服务初始化时间限制---必须创建线程
2.OnStart()必须及时返回，处理多个服务端的请求---线程池
服务的安装
服务必须在注册表中注册，注册表路径：HKLM\System\CurrentControlSet\Services

Installer：ProjectInstaller、ComponentInstaller
    ComponentInstaller:ServiceInstaller、ServiceProcessInstaller
    ProjectInstaller--->ServiceInstaller、ServiceProcessInstaller

安装程序---服务设计界面选择Add Installer
    新增一个ProjectInstaller类、一个ServiceInstaller，一个ServiceProcessInstaller实例
    安装程序类
        ProjectInstaller类派生自System.Configuration.Install.Installer(所有自定义安装程序的基类)
        Installer类构建基于事务的安装程序：Install()\Commit()\Rollback()\Uninstall()
        对于RunInstaller特性为true，安装程序会调用ProjectInstaller类
            自定义安装程序以及Installutil.exe都会检查该特性
    ServiceInstaller类和ServiceProcessInstaller类
        都派生自ComponentInstaller类，该类派生自Installer类
        该类的派生类作为安装进程的一部分
        一个服务进程可以包含多个服务
        ServiceProcessInstaller用于配置进程，为该进程的所有服务定义值
        ServiceInstaller用于服务的配置，每个服务都需要一个ServiceInstaller实例
            所有服务共享属性：Username、Password、Account、HelpText
        ServiceInstaller属性：StartType、DisplayName、ServiceName、ServicesDependentOn

    SeviceInstallerDialog
        System.ServiceProcess.Design
        在安装过程中希望输入服务应使用的账户；
        将ServiceProcessInstaller类的Acount设置为ServiceAcount.User
            并且Username和Password设置为null，则自动显示Dialog

    Installutil
        使用installutil.exe实用程序安装和卸载服务
        安装服务：installutil quoteservice.exe
        卸载服务：installutil /u quoteservice.exe
Windows服务的监控和控制
Services MMC、net.exe、sc.exe
MMC
net.exe
    dos net start---显示所有正在运行的服务
    net start servicename --- 启动服务
    net stop servicenam --- 停止服务
    net pause   net continue
sc.exe
    卸载、删除、配置、查看服务
virtual studio server explorer

自定义ServiceController
    System.ServiceProcess.ServiceController
    使用ServiceController类获取每一个服务的信息
故障排除与日志信息

*/

/* 全球化与本地化 Globalization And Localization
全球化--System.Globalization
本地化--System.Resource

全球化-Globalization
    1.Unicode问题
        区分基本字符与组合字符
        System.Globalization的StringInfo类处理组合字符问题
        string可以包含由基本字符和组合字符组成的文本元素
    2.区域性和区域 -region
        CultureInfo表示区域性--定义日历、数字和日期的显示格式以及排序字符串
        RegionInfo类表示区域设置
        特定、中立、不变的区域性
        特定的区域性---与真正存在的区域性相关，可以映射到中立的区域性
        中立的区域性---
        不变的区域性---独立于真正的区域性
        CurrentCulture与CurrentUICulture
        设置区域性时区分用户界面的区域性与数字及日期格式的区域性
        区域性与线程相关
        CurrentCulture用于设置与格式化和排序项一起使用的区域性
        CurrentUICulture用于设置用户界面的语言
    3.数字格式化
        System中的数字结构的格式化ToString()
        可以给ToString()传递一个字符串参数以及实现了IFormatProvider接口的类
        字符串---指定表示法的格式（区分：标准数字格式化字符串和图形数字格式化字符串）
            对于标准数字格式化---字符串预定义
            C--货币，D--输出为小数，E--输出用科学计数法，F--定点输出
            G--一般输出，N--输出为数字，X--输出为十六进制数
            对于图形数字格式化---指定位数、节、组分隔符、百分号
        IFormatProvider接口由NumberFormatInfo、DateTimeFormatInfo、CultureInfo类实现
        该接口定义了GetFormat()方法---返回一个格式对象
        NumberFormatInfo---为数字定义自定义格式，可以创建独立于区域性的对象或不变的对象
            从静态属性InvariantInfo返回一个与区域性无关的只读NumberFormatInfo对象，该对象的
            格式化值取决于当前线程的CultureInfo类
    4.日期格式化
        DateTimeFormatInfo类
    5.使用区域性
    6.排序
        排序字符串取决于区域性，默认下，为排序而比较字符串的算法依赖于区域性
资源
资源文件与附属程序集
1.创建资源文件
    可以使用文本文件或者XML文件的resX文件
    对于内嵌的字符串表的资源一般使用文本文件创建
2.资源文件生成器 -resgen.exe
    可以使用资源文件生成器程序在资源文件的外部创建一个资源文件
    resgen sourceFileName
    得到的资源文件可以作为一个外部文件添加到程序集或者内嵌到dll或exe中
    Resgen还可以创建基于XML的resX的资源文件
    resgen sourceFileName   targetFileName.resX
    Resgen支持强类型化的资源，强类型化的资源用一个访问资源的类表示，这个类用Resgen使用程序
    /str创建
    resgen /str:语言,名称空间,类名，源代码文件名   targetFileName.resX
    Resgen不支持添加图片
    ResXGen可以在.resX文件中引用图片
3.ResourceWriter
    System.Resource中的一个类
    用于编写二进制资源文件
    ResXResourceWriter编写基于XML的资源文件
    都支持图片和其他可以序列化的对象（使用ResXResourceWriter必须引用System.Windows.Froms）
4.使用资源文件
    使用C#命令行编辑器csc.exe和/resource选项
    访问嵌入资源，可以使用System.Resource的ResourceManager类，
    将嵌入的资源为参数的程序集传递给ResourceMnanger的构造函数
    资源的根名：名称空间和资源文件名（不带文件扩展名）组成
    通过强类型化的资源，可以简化代码
    使用托管资源编辑器创建强类型化的资源，将Access Modifier从No Code Generation重置为public或者internal
    使用public---生成的类使用公共访问修饰符，可以在其他程序集中使用
    使用internal---生成的类使用内部访问修饰符，只能在程序集内部使用
5.System.Resources名称空间
    ResourceManager---用于从程序集或资源文件中获取当前区域性的资源，或者获取指定区域的ResourceSet类
    ResourceSet---表示特定区域的资源，创建该实例时，会枚举一个实现IResourceReader接口的类，并在散列表中存储所有的资源
    IResourceReader---接口，用于从ResourceSet中枚举资源，ResourceReader实现该接口
    ResourceWriter---用于创建资源文件，改类实现IResourceWriter接口
    ResXResourceSet、ResXResourceReader、ResXResourceWriter类似于ResourceSet
    ResXResourceReader和ResXResourceWriter---用于创建基于XML的资源文件.resX，而不是二进制文件
    ResXFileRef---用于链接资源
    Sytem.Resources.Tools---包含StronglyTypeResourceBuilder，可以从资源这种建类

使用Virtual Studio的Window Forms本地化
1.将窗体的Localizable属性设置为true
2.自动生成资源文件resx，并动态向其添加元素项

1.通过编程方式修改区域性
    在翻译资源并构建附属程序集后，可以根据用户配置的区域性获得正确的译文
    CurrentCulture属性用于格式化
    CurrentUICulture属性用于加载资源
    可以使用命令行选项启动应用程序，传递语言代码

2.使用自定义资源消息
    对于翻译硬编码的字符串
    可以在Resources.resx文件中直接编写自定义资源消息与特定语言相关的派生文本
    或者新建资源文件
    在强类型化的资源中，不需要实例化ResourceMessager类，使用强类型化资源的属性

3.资源的自动回退
    附属程序集中的资源值若没有，则在父程序集中查找
    ，若父程序集中也没有，则在父程序集的父程序集（中立程序集）中查找
    中立程序集中不包含区域性代码
    在主程序集的区域性代码中，不应定义任何区域性

4.外包翻译-outsource translation
    使用资源文件很容易进行外包翻译，翻译资源文件时，只需要XML编辑器即可
    但是控件大小不能调整
    Windows的窗体设计器--Windows资源本地化编辑器（winres.exe）,使用该工具无需翻译C#资源文件
    ，只需翻译二进制文件或基于XML的资源文件，翻译完成即可导入项目中，构建附属程序集
    不希望改变控件大小和位置，且不处理XML文件，可以发送一个基于文本的文件
    是resgen.exe可以从XML文件中创建一个文本文件
    resgen myresource.resX myresource.txt
    从返回的文本文件中创建XML文件，并添加区域名
    resgen myresource.es.txt myresource.es.resX
ASP.NET Web Forms的本地化
与Windows应用程序的本地化类似
但必须区分用户界面的区域性和用于格式化的区域性，都可以在Web和页面级定义也可以通过编程定义
独立于Web服务器的操作系统，区域性和用户界面区域性可以用Web.config配置文件中的元素<globalization>定义
EG: <globalization culture="en-US" uiCulture="en-US"/>
如何特定的web页面有所不同则可以使用Page指令指定区域性
EG:<%Page Language="C#" Culture="en-US" UICulture="en-US" %>
若页面的语言应根据客户端的语言不同设置而不同，可以通过编程将线程中的区域性设置为从客户端接收语言设置
使用Page指令将区域性设置为“Auto”
EG：<%Page Language="C#" Culture="Auto" UICulture="Auto" %>
使用资源ASP.NET区分用于全网站的资源与只用于一个页面的资源
    1.一个页面内资源 Tools->General Loacl Resource命令->为页面创建资源，web控件与本地资源文件使用
        meta:resourceKey特性指定
    2.对于多个页面间共享的资源
        创建一个ASP.NET文件夹->添加资源文件->控件关联：使用Expression按钮->Resource类型：
        设置ClassKey-资源文件名（这里会生成强类型化的资源文件），ResourceKey-资源的名称
        ASPX文件中，用绑定语法关联<%$>
        EG:<asp:Label ID="Label2" Runat="server" Text="<%$ Resource:<ClassKey>,<ResourceKey> %>" />
WPF本地化
可以使用.net资源，也可以使用XAML（XML for application markup language）资源字典
1.用于WPF的.NET资源
    添加资源文件，将资源文件的默认Internal修饰符修改为public，并在XAML文件中添加命名空间别名
    添加别名：xmlns:name=clr-namespace:rresource文件的名称空间
    使用x:Static 标记扩展
    eg：content="{x:Static name:class.property}"
    优点：
        1..NET资源容易管理
        2.x:Static绑定由编译器检查

2.XAML资源字典
    使用XAML创建本地化内容
    a.从主要内容中创建附属程序集
        编译WPF应用程序时，XAML代码编译为二进制式BAML，BAML存储在一个程序集中，将BAML代码从主程序集移动到一个独立的
        附属程序集中，修改.csproj生成文件，添加<UICulture>元素，作为<propertyGroup>的一个子元素
        1.先卸载   2.编辑.csproj文件   3.再次加载项目
        将BAML分离到一个程序集后，并应用NeutralResourceLanguage特性，给附属程序集提供资源回退位置
        在AssemblyInfo中设置程序集特性
        EG：[assembly:NeutralResourcesLanguage("en-US",UltimateResourceFallbackLocation.Satellite)]
        若BAML保存在主程序集中，在UltimateResourceFallbackLocation设置为MainAssembly
    b.对本地化内容使用资源字典
        1.添加资源字典
        2.资源字典中添加元素
        3.将资源字典添加到资源中
            若资源字典只需要在一个窗口或一个特定的WPF元素中使用---将字典添加到该窗口或WPF元素的的资源集合中
            若多个窗口都使用一个资源字典---将字典添加到<Application>元素的App.xml文件中
            EG:        <ResourceDictionary>
                            <ResourceDictionary.MergedDictionaries>
                                <ResourceDictionary Source="Dictionary1.xaml"/>
                            </ResourceDictionary.MergedDictionaries>
                        </ResourceDictionary>
            要从代码隐藏中使用XAML资源字典,可以使用Resource属性索引器.FindResource(),TryFindResource()
            资源查找 当前元素->父元素->Windows->Application
    c.给进行本地化的元素添加x:Uid特性
        用于本地化的Uid特性
        对于自定义资源字典文件,可以引用应从代码中本地化的文本,为本地化XAML代码和WPF元素
        将X:Uid特性作用于需要本地化的元素的唯一标识
        对于XAML可以使用 msbuild命令(打开msbuild命令,转到该目录下)
            msbuild /t:updateuid
        验证uid的正确性 /t:checkuid
    d.从程序集中提取本地化内容
        用于本地化的LocBaml工具
        使用LocBaml从附属程序集中提取需要本地化的内容到csv文件
        翻译csv文件
        使用LocBaml根据csv文件、附属程序集，创建附属程序集
    e.翻译相应内容
    f.为每种语言创建附属程序集
自定义资源读取器
资源读取器 4.5的一部分，可以从资源文件和附属程序集中读取资源
使用自定义资源读取器，必须创建自定义资源集和自定义资源管理器
资源读取器必须实现IResourceReader接口
自定义资源集派生自ResourceSet
自定义资源管理器派生自ResourceManager
创建自定义区域性
使用System.Globalization中的CultureAndRegionInfoBuilder类
位于sysglobl程序集中
实例化CultureAndRegionInfoBuilder类并指定父区域性
最后调用register方法注册新区域
新区域文件位于<windows>\Globalization中，扩展名为.nlp
在系统上注册自定义语言需要管理员权限，需要在应用程序清单文件(添加清单文件)指定请求的执行权限
在项目属性中在Application中设置
*/

/* XML&XAML
1.XML定义
    XML - Extensible Markup Language 可扩展标记语言
    是SGML(标准通用语言)的一个子集
    用途：传输和存储数据，设计宗旨是传输数据
    特点：标签没有预定义，需要自行定义标签；XML具有自我描述性
    与HTML差异：
    1.XML不是HTML的替代
    2.XML设计为传输和存储数据，焦点是数据内容--传输数据；HTML设计为显示数据，焦点是数据外观--显示数据
    XML是不作为的---设计为结构化、存储以及传输信息
    XML仅仅是纯文本
    XML可以定义标签
    XML是对HTML的补充
    XML是独立于软件和硬件的信息传输工具
    XML是W3C的推荐标准
    XML将数据从HTML中分离
    XML简化数据共享--以纯文本方式进行存储
    XML简化数据传输
    XML简化平台的变更
    XML用于创建新的Internet语言
    XML文档形成一种树结构，从根开始扩展到叶子

2.XML文档结构
    XML声明：定义XML版本以及编码结构
    <?xml version="1.0" encoding="ISO-8859-1"?>
    XML文档形成一种树结构
        1.XML文档必须包含根元素，该元素是其他元素的父元素
        2.所有元素均可拥有子元素
        3.所有元素均可拥有文本内容和属性
        4.所有元素都必须有关闭标签（XML声明不属于XML本身的组成部分，不是XML元素）
        5.XML元素使用XML标签定义，标签对大小写敏感
        6.XML标签必须正确的嵌套
        7.XML的属性值必须加引号
    实体引用：
        对于特殊字符必须使用实体引用：
        Eg：<,> => &it; ,&gt;
        XML中5个预定义的实体引用：
        &lt; - 小于(<)
        &gt; - 大于(>)
        &amp; - 和号(&)
        &apos; - 单引号(')
        &quot; - 双引号(")
        在XML中只有<和&确实是非法的
    XML注释：
        <!-- Mark -->
    XML中空格会被保留---HTML会将连续的空格合并为一个
    XML以LF存储换行
        Windows中以回车符CR和换行符LF存储换行
        Unix中以LF存储换行    Macintosh 使用CR存储换行
    XML元素
        XML元素指从开始标签到结束标签的部分（包括标签）
        可以包含元素、文本，拥有属性
        元素内容---包含其他元素
        文本内容---仅包含文本
    XML命名规则
        XML元素命名
        1.可以包含字母、数字以及其他字符
        2.不能以数字或标点符号开始
        3.不能以xml（XML，Xml）开始
        4.不能包含空格
        最佳习惯：
            1.名称具有描述性
            2.简短
            3.避免字符"-",".",":"

    XML元素可以扩展
    XML元素可以在开始标签中包含属性，类型HTML
    Attribute提供关于元素的额外信息
        HTML属性---通常提供不属于数据组成部分的信息
        XML---属性值必须被引号包围，单引号双引号均可使用，若属性值本身包含双引号，则必须用单引号或者实体引用
    XML元素与属性
        在XML避免使用属性（尽量使用元素提供数据，使用属性提供与数据无关的信息）
        理由：
            1.属性无法包含多重值-元素可以
            2.属性无法描述树结构-元素可以
            3.属性不易扩展
            4.属性难以阅读
    针对元数据的XML属性
        向元素分配ID引用，ID索引标识XML元素，仅用于标识不同的标签，不是标签数据的组成部分
        元数据(与数据相关的数据)应当存储为属性，数据本身存储为元素

    形式良好的XML-拥有正确语法的XML
        通过DTD验证的XML是合格的XML
        形式良好的XML文档
            1.XML文档必须具有根元素
            2.XML必须有关闭标签
            3.XML标签对大小写敏感
            4.XML元素必须被正确嵌套
            5.XML属性值必须加引号
    验证XML
        合法的XML遵守文档类型定义(DTD document type define)的语法规则
        <!DOCTYPE nodeName []>
        XML DTD
        定义XML文档结构---使用一系列合法的元素来定义文档结构
        XML Schame
        W3C基于XML的代替者
        <xs:element name="">  ... </xs:element>
        一个通用的验证器
        XML错误会终止程序
            1.对XML进行语法检查---XML验证器
            2.根据DTO进行验证

    XML浏览器支持

    使用CSS显示XML
        使用CSS可以为XML文档添加显示信息，将XML链接到CSS文件
        <?xml-stylesheet type="text/css" href="cd_catalog.css"?>
        使用 CSS 格式化 XML 不是常用的方法，更不能代表 XML 文档样式化的未来
        W3C 推荐使用XSLT
    使用XSLT显示XML
        XSLT是首选的XML样式表语言
        XSLT (eXtensible Stylesheet Language Transformations) 远比 CSS 更加完善
        在服务器上通过XSLT转换XML
XAML
基于XML的，格式组织良好的标记语言
XAML是强类型的
广泛的扩展性
是WPF类的映射

基本语法规则：
1.区分大小写
2.所有的属性值都包含在双引号中，无论是什么类型
3.所有的元素都必须自我封闭，一个起始标记和一个结束标记<b></b>，或起始标记和结束标记在一行内<b  />
4.必须符合XML文档要求
XAML概述
xaml - eXtensible Applicaion Markup Language    可扩展应用程序标记语言
声明性的XML语法
对于WPF、WF、XPS文档、Silverlight、Windows 8
XAMl代码使用XML文本声明
XML名称空间定义WPF对XAML的扩展 xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation
WF4使用XML名称空间。。。定义工作流

在WPF、WF中XAML元素映射到.NET的类
将.Net名称空间映射到XML别名上，可以在XML中使用自定义类
BAML - binary application markup language 二进制应用程序标记语言
读写XAML和BAML通过读取器与写入器完成
System.Xaml --- 核心XAML类
System.Windows.Markup --- 使用Xaml空间的所有技术
元素映射到.NET对象上

使用自定义.NET类
在XAML中声明.NET名称空间，并定义一个XML别名
关键字clr-namespace映射到.NET名称空间
若名称空间与XAMLdaim不在同一个程序集中，就必须在XAML名称空间别名中包含该程序集名
即：Assembly=NamSpace
私有程序集和共享程序集都可以如此引用
对于共享程序集需要指定其全名，包括版本号、区域性、程序集的密钥令牌
若将.NET名称空间映射到XML名称空间中，使用程序集特性XmlnsDefinition
[assembly:XmlnsDefinition(XML名称空间, .NET名称空间)]
将属性用作特性
若属性类型可表示为字符串，或将字符串转换为属性类型，就可以将属性设置为特性
类型转换器派生自System.ComponentModel名称空间中的TypeConverter
需要转换的类型用TryConverter特性定义了类型转换器
将属性用作元素
总是可以使用元素语法给属性赋值
将属性标记为ContentProperty:[ContentProperty("属性名")]
通过这个标记就可以将属性值写入为子元素值
基本的.net类型
对于XAML2006，核心.net类型需要从XML命名空间中引用（使用别名）
XAML2009，用别名x定义了核心.net类型
使用集合和XAML
定义数组使用x:Array扩展，其中的type属性用于指定数组元素类型
XAML2006不支持泛型---定义派生zi泛型的非泛型类，使用该非泛型类
XAML2009支持泛型，通过x:Arguments特性定义泛型类型
使用XAML代码调用构造函数
XAML2006中不能使用没有默认构造函数的类
XAML2009使用x:Arguments通过参数调用构造函数
依赖属性
WPF使用依赖属性完成数据绑定、变更通知、样式变化，对于数据绑定，绑定到的.net属性源UI元素必须具有依赖属性
依赖属性在get、set访问器中调用GetValue()和SetValue()，方法属于基类DependencyObject
使用依赖属性，数据成员在基类管理的内部集合中，仅在值变化时分配数据，对于没有变化的值，数据可以在不同的实例或基类间共享
GetValue（）和SetValue（）需要一个DependencyProperty参数，该参数由类的静态成员定义,该静态成员与属性同名，并在该属性名后追加Property
DependencyProperty.Register()---用于向依赖属性系统中注册属性
Register(string-属性名，Type-属性类型，object-拥有者（类）)

1.创建依赖属性
        DependencyProperty类静态方法Register()注册属性，并返回一个DependencyProperty实例
        属性声明中get、set属性访问器中分别调用GetValue()和SetValue()方法，并引用之前注册是实例
        则属性成为依赖属性

2.强制值回调
    依赖属性支持强制检查--检查属性值是否有效
    将检查函数传递给ProperMetaData对象，并在依赖属性注册中引用该参数

3.值变更回调和事件
    PropertyMetadata的第二个参数为值变更事件委托
事件的冒泡和隧道
隧道事件---从外部向内部移动，外部控件首先接收到内部控件引发的事件
冒泡事件---从内部向外部移动
WPF支持冒泡和隧道事件
改变源的同时也改变事件类型
自定义类若要定义事件的冒泡和隧道，则该类必须派生自UIElement
并定义AddHandler()和RemoveHandler()方法

1.首先定义依赖属性
    由于派生自UIElement，UIElement派生自Visual，而Visual派生自DependencyObject
    所以间接派生自DependencyObject
2.注册依赖属性时，添加值改变委托，以及强制性检查委托
    在值改变委托函数中，添加对自定义事件RoutedPropertyChangedHandler<T>的实例化，并调用该类的事件触发方法
3.定义静态路由事件---定义静态路由事件RoutedEvent使用EventManager进行注册
4.定义事件RoutedPropertyChangedHandler<T>---并引用add和remove进行声明，并引用基类的AddHandler和RemoveHandler()方法
5.定义该类的事件触发方法---引用基类的RaiseEvent(RoutedPropertyChangedHandler<T>)
附加属性
为其他类型定义属性
Eg：一些容器控件为子控件添加了附加属性
附加属性类必须派生自基类DependencyObject,并定义一个普通属性，其中get和set访问器访问基类的GetValue()和SetValue()
然后调用DependencyObject的RegisterAttached()方法注册一个附加属性
在XAML中附加属性可以附加到任何元素上
标记扩展
通过标记扩展扩展XAML的元素或特性语法
花括号通常表示XML的标记扩展的符号
特性的标记扩展通常使用简写记号，不再使用元素
特性语法通过花括号和没有Extension后缀的扩展类名来定义
Eg：Property=“{ExtensionClassName keyValue}”
或者使用元素语法(作为上级元素的子元素)：
<ExtensionClassName ResourceKey="keyValue" \>
创建自定义标记扩展
基于派生自MarkupExtension的子类
且后缀扩展名为Extension
重写ProvideValue()方法，该方法返回扩展的值，返回的类型用MarkupExtensionReturnType特性注解该类
对于ProvideValue（）方法参数为IServiceProvider接口类型对象，通过该接口可以查询不同的服务
例如：IProvideValueTarget或者IXamlTypeResolver
IProvideValueTypeTarget---用于访问通过TargetObject和TargetProperty属性应用标记扩展的控件和属性
IXamlTypeResolver---用于将XAML元素名解析为CLR对象
XAML定义的标记扩展
XAML中定义的标记扩展---x:Array==ArrayExtension;x:Type==TypeExtension（根据字符串输入返回类型）；x：Null==NullExtension（值设置为空）；
    x：static==StaticExtension（调用类的静态成员）
WPF使用标记扩展访问资源---数据绑定和颜色转换
WF使用标记扩展和活动
WCF使用标记扩展指定给端点定义
读写XAML
XAML可以从文本XML形式、BAML或对象中读取，写入XML或对象树
API位于System.Xaml中
XAMLService类---加载、分析、保存、转换XAML
加载：XamlService.Load()--从文件、流、读取器中加载XAML代码；XamlReader抽象基类--XamlObjectReader(读取对象树)/XamlXmlReader（读取XML文件）/Baml2006Reader（读取二进制形式）
一般的XamlService不支持特定的WPF特性，
读写WPF XAML使用System.Windows.Markup中的XAMLReader和XamlWriter
XAML编译器如何解析XAML文件
1.编译器在处理XAML文件时，得到的都是字符串
2.对于一个XML元素的开始表示为类型实例
3.对于属性或子元素表示XML组成---对该实例的属性进行设置
4.分析XML属性（Attribute）进行赋值的字符串时，XAML编译器根据字符串内容决定自身的分析逻辑
5.对于普通的属性赋值字符串，XAML编译器根据属性类型决定是否需要对字符串进行转化，若属性类型不是字符串，XAML处理器调用相应的转化逻辑
    ，对于枚举类型，XAML编译器通过Enum的Parse方法得到相应的数值
6.对于自定义类型，XAML根据该自定义类型声明或属性声明生标识的TypeConverter将字符串转换为该自定义类型
    ，该类型必须是值类型或者具有默认构造函数的类型或者表明了专业类型转换器的类型（TypeConverterAttribute）
7.对于不满足以上要求的类型，需要使用标记扩展
8.对于标记扩展，若XAML编译器遇到大括号或者从MarkupExtension派生的对象元素，XAML编译器将按照标记扩展解析该字符串，直到大括号结束或者元素结束
9.首先XAML根据字符串决定标记扩展所对应的MarkupExtension派生类型，然后按照以下规则进行解析：
9.1.逗号表示各个标记的分隔符
9.2.若分隔符的标记没有等号赋值，则其被视为构造函数的参数，改参数需要与构造函数的参数个数匹配，若两个构造函数的参数个数相同，则编译器无法分析，该行为没有定义
9.3.如果每个标记都包含等号，则XAML编译器调用默认构造函数，并对该属性进行赋值
9.4.若标记扩展使用了构造函数以及属性赋值，XAML编译器内部调用构造函数并进行属性赋值，最后在应用程序加载时调用该类型的ProvideValue（）函数，用来定义标记应该返回哪个对象
    该函数调用会传入当前上下文的信息，以允许ProvideValue()函数根据该上下文创建相应的对象
9.5.若标记之间存在嵌套则，XAML编译器会首先计算标记扩展的最内层
*/

/* MEF - Managed Extensibility Framework
    应用程序插件模型---动态加载和使用程序集中的功能---解决查找和使用插件问题
    System.ComponentModel.Composition
    MEF定义：
    是一个用于创建可扩展的轻型应用程序库。可以发现并使用扩展（无需配置）；封装代码（避免生成硬依赖项）；可以在应用程序间重用扩展
    应用程序或插件均按照约定好的协议（接口）进行开发，系统将自动扫描指定的文件夹，并按照协议自动导入
MEF体系结构
    net4.5提供两种技术编写动态加载插件的应用程序：
    1.MEF---通过部件和容器构建，容器在类别中查找部件，类别在程序集或目录中查找插件。容器将入口连接到出口
    2.MAF（Managed Add-in Framework）---使用一个管道在插件和宿主程序之间通信，可以通过应用程序域或者进程分开
    MEF：
        部件加载过程：
            1.容器在类别中查找部件
            2.类别使用出口查找部件
            3.出口提供程序访问类别，提供类别中的出口
            4.多个出口提供程序可以连接，以定制出口
            5.容器使用出口提供程序把入口连接到出口
        MEF三大类别：
            1.用于宿主的类---类别和容器
            2.基元类---基类，扩展MEF体系结构
            3.基于特性机制的类---通过反射构成
        MEF实现方式基于特性，该特性指定哪些部件应导出，并将其映射到入口上，允许使用抽象基类ComposablePart、扩展方法以及ReflectionModelServices类
        中基于反射的机制来实现其他技术
使用属性的MEF

*/

/* 声明式安全检查 Demand,LinkDemand
 * 声明式安全：
 *  提供两种检查：
 *  Demand：指定代码访问安全堆栈审核，
 *  LinkDemand：在JIT时，只检查直接调用方，不检查调用方的调用方，通过检查后，后续调用不会有安全性系统开销
 *
 *
 */

/*
 * 序列化
 *
 *  JSON格式化
 *
 *  依赖注入和控制反转
 *
 *
 */