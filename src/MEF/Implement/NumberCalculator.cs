using IContract;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Implement
{
    [Export(typeof(ICal))]
    public class NumberCalculator : ICal
    {
        public int Opeartion(IOperator op, int[] paras)
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

        public IList<IOperator> GetOperations()
        {
            return new List<IOperator>
                {
                    new NumberOperator(){Name="+",NumberOperands=2},
                    new NumberOperator(){Name="-",NumberOperands=2},
                    new NumberOperator(){Name="*",NumberOperands=2},
                    new NumberOperator(){Name="/",NumberOperands=2}
                };
        }
    }
}
