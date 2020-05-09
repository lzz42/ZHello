using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IContract
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class MetaDataAttribute : ExportAttribute, IMetaData
    {
        public string VData { get; set; }
        public MetaDataAttribute(string name, Type type)
            : base(name, type)
        {

        }
    }

}
