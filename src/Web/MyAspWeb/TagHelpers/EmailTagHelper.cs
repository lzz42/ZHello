using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MyAspWeb.TagHelpers
{
    public class EmailTagHelper : TagHelper
    {
        public const string EmailDomain = "emaildomain.com";
        public string MailTo { get; set; }

        public override void Init(TagHelperContext context)
        {
            base.Init(context);
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            var addr = $"{MailTo}--->{output.Content}@{EmailDomain}";
            output.Attributes.SetAttribute("href", $"mailto--->{addr}");
            output.Content.SetContent($"{addr}");
        }

        //public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        //{
        //    output.TagName = "a";
        //    var content = await output.GetChildContentAsync();
        //    var addr = $"{MailTo}@{EmailDomain}";
        //    output.Attributes.SetAttribute("href", $"async-mailto:{addr}");
        //    output.Content.SetContent($"async-{addr}::{content.GetContent()}");
        //}
    }
}