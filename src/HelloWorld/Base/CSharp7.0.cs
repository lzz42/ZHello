namespace ZHello.Base
{
    internal class CSharp7
    {
        public static void UseTuple()
        {
            /*
             元组 - 泛型值类型
             */
            //声明元组
            (string name, int age, string addr, float height) p = ("Alice", 20, "USA", 170.5f);
            //使用元组
            System.Diagnostics.Trace.WriteLine(string.Format("Info:{0},{1}.{2},{3}", p.name, p.age, p.addr, p.height));
            System.Diagnostics.Trace.WriteLine($@"Info:{{}}{p.name},{p.age},{p.addr},{p.height}");
            //将元组分配到已声明的变量
            (string name, int age, float height) = ("Alice", 21, 170.6f);
            System.Diagnostics.Trace.WriteLine($@"Info:{{}}{name},{age},{height}");
            //将元组分配到到预声明的变量
            string country;
            int peopleNumber;
            float area;
            (country, peopleNumber, area) = ("USA", 9000, 1000.1f);
            //将元组分配到隐式类型变量
            (var Address, var Number, var Count) = ("USA", 1, 1.1f);
            //将元组分配到隐式类型变量且使用分布式语法
            var (Address1, Number1, Count1) = ("USA", 1, 1.1f);
            //将元组分配到隐式类型变量中
            var tuple1 = (Address2: "U.S.A", Number2: 2, Count2: 89.2f);
            //将元组分配到隐式类型变量中,按 Item-number 属性访问元组元素
            var tuple2 = ("U.S.A", 2, 89.2f);
        }
    }
}