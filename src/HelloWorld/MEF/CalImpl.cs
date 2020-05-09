using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace ZHello.MEF
{
    /*约定实现*/

    /// <summary>
    /// 导出计算器A 基本算术运算器
    /// </summary>
    [Export(contractType: typeof(ICal))]
    public class CalculatorA : ICal
    {
        public int Opeartion(IOperation op, int[] paras)
        {
            if (op == null || paras == null)
            {
                return -2;
            }
            if (op.NumberOperands != paras.Length)
            {
                return -3;
            }
            var result = 0;
            switch (op.Name)
            {
                case "+":
                    result = paras[0] + paras[1];
                    break;

                case "-":
                    result = paras[0] - paras[1];
                    break;

                case "*":
                    result = paras[0] * paras[1];
                    break;

                case "/":
                    {
                        if (paras[1] == 0)
                        {
                            result = int.MinValue;
                        }
                        else
                        {
                            result = paras[0] + paras[1];
                        }
                    }
                    break;

                default:
                    break;
            }
            return result;
        }

        IList<IOperation> ICal.GetOperations()
        {
            return new List<IOperation>
                {
                    new OperationA(){Name="+",NumberOperands=2},
                    new OperationA(){Name="-",NumberOperands=2},
                    new OperationA(){Name="*",NumberOperands=2},
                    new OperationA(){Name="/",NumberOperands=2}
                };
        }
    }

    /// <summary>
    /// 导出计算器B 逻辑位操作运算器
    /// </summary>
    //[Export(contractType: typeof(ICal))]
    public class CalculatorB : ICal
    {
        public int Opeartion(IOperation op, int[] paras)
        {
            if (op == null || paras == null)
            {
                return -2;
            }
            if (op.NumberOperands != paras.Length)
            {
                return -3;
            }
            var result = 0;
            switch (op.Name)
            {
                case "|":
                    result = paras[0] | paras[1];
                    break;

                case "&":
                    result = paras[0] & paras[1];
                    break;

                case "<<":
                    result = paras[0] << paras[1];
                    break;

                case ">>":
                    result = paras[0] >> paras[1];
                    break;

                default:
                    break;
            }
            return result;
        }

        IList<IOperation> ICal.GetOperations()
        {
            return new List<IOperation>
                {
                    new OperationA(){Name="|",NumberOperands=2},
                    new OperationA(){Name="&",NumberOperands=2},
                    new OperationA(){Name="<<",NumberOperands=2},
                    new OperationA(){Name=">>",NumberOperands=2}
                };
        }
    }

    /// <summary>
    /// 运算符
    /// </summary>
    public class OperationA : IOperation
    {
        public string Name { get; internal set; }

        public int NumberOperands { get; internal set; }
    }

    public class CalculatorShow : ICalShow
    {
        public string Show(int a)
        {
            return "ok int :: " + a;
        }

        public string Show(int a, int b)
        {
            return "ok int :: " + a + ",int :: " + b;
        }
    }

    [Export("MEFAchieve.DefinC")]
    public class DefineC
    {
        public string GetStr()
        {
            return "Definc ";
        }
    }

    public class DefineD
    {
        [Export("Add", typeof(Func<int, int, int>))]
        [ExportMetadata("n1", "v1")]
        public int Add(int a, int b)
        {
            return a + b;
        }

        [Export("RefeObj", typeof(Action<string, string>))]
        [ExportMetadata("n2", "v2")]
        public void RefeObj(string s1, string s2)
        {
            s1 += s2;
        }

        [MetaDataEx("Subtract", typeof(Func<double, double, double>), V = "VX")]
        public double Subtract(double d1, double d2)
        {
            return d1 - d2;
        }
    }
}