using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IContract
{
    public interface IMetaData
    {
        string VData { get; }//只能定义只读属性 才能用作MetaData代替
    }
}
