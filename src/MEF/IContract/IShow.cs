using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IContract
{
    public interface IShow
    {        
        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        string Show(int a);

        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        string Show(int a, int b);
    }
}
