using System;
using System.Reflection;
using CSScriptLibrary;

namespace ZHello.Cecil
{
    public class CS_Script
    {
        public void Main()
        {
            //编译方法，并返回动态生成的程序集，方法默认加载类DynamicClass,类型完全限定名为Submission#0+DynamicClass
            Assembly compilemethod = CSScript.RoslynEvaluator.CompileMethod(@"
                using System;
                public static void CompileMethod(int t){
                    System.Diagnostics.Trace.WriteLine(""ComlileMethod::""+t);
                }
                ");
            var p = compilemethod.GetType("Submission#0" + "+DynamicClass");
            var me = p.GetMethod("CompileMethod");
            me.Invoke(null, new object[] { 1024 });
            //加载方法并返回默认类的一个实例
            dynamic lm = CSScript.Evaluator.LoadMethod(@"
                using System;
                public void LoadMethod(string str){
                    System.Diagnostics.Trace.WriteLine(""LoadMethod::""+str);
                }
                ");
            lm.LoadMethod("2020-12-29");
            //加载类，并返回代码段中的第一个类的实例
            dynamic lc = CSScript.Evaluator.LoadCode(@"
                using System;
                public class CSScriptCC{
                    public void LoadCode(string str){
                        System.Diagnostics.Trace.WriteLine(""CSScriptCC LoadCode::""+str);
                    }
                }
                ");
            lc.LoadCode("2020-2021");
            //编译类并返回动态生成的程序集
            var eval = CSScript.Evaluator.ReferenceDomainAssemblies(DomainAssemblies.AllStaticNonGAC);
            var ass = eval.CompileCode(@"
                using System;
                public static class CSSScriptStatic{
                    public static void LoadStaticCode(string str){
                        System.Diagnostics.Trace.WriteLine(""CSSScriptStatic_LoadStaticCode::""+str);
                    }
                }
            ");
            var type = ass.GetType("Submission#0" + "+CSSScriptStatic");
            //var obj = ass.CreateInstance("Submission#0" + "+CSSScriptStatic");
            var m = type.GetMethod("LoadStaticCode");
            m.Invoke(null, new object[] { "2020" });
            //生成委托
            var dlg = eval.CreateDelegate(@"int Sqr(int x)
            {
                return x*x;
            }");
            Console.WriteLine(dlg(11));
        }
    }
}