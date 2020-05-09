using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;

namespace ZHello.Base
{
    public class TypeConverter_Test
    {
        public void Init()
        {
            //1.注册转换器和转换类型
            TypeDescriptor.AddAttributes(typeof(MyView), new TypeConverterAttribute(typeof(MyViewConverter)));
        }

        public void Main_Test()
        {
            var typeConvert = TypeDescriptor.GetConverter(typeof(MyView));
            var myView = new MyView();
            var yourView = new YourView();
        }
    }

    [ComVisible(true)]
    [TypeConverter(typeof(MyViewConverter))]
    public class MyView
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public bool Gender { get; set; }
    }

    [ComVisible(true)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class YourView
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public bool Gender { get; set; }
    }

    public class MyViewConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(int))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value.GetType() == typeof(int))
            {
                return new MyView()
                {
                    Age = (int)(value)
                };
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(MyView))
            {
                var view = (MyView)value;
                return view;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return base.GetPropertiesSupported(context);
        }
    }
}