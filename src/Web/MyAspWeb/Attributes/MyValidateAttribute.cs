using System.ComponentModel.DataAnnotations;

namespace MyAspWeb.Attributes
{
    /// <summary>
    /// 自定义验证属性
    /// </summary>
    public class MyValidateAttribute : ValidationAttribute
    {

        public override string FormatErrorMessage(string name)
        {
            return base.FormatErrorMessage(name);
        }

        public override bool IsValid(object value)
        {
            return base.IsValid(value);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return base.IsValid(value, validationContext);
        }
    }
}