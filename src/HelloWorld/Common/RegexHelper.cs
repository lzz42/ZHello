using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;

namespace ZHello.Common
{
    public class RegexHelper
    {
        public static bool HasChinese(string value)
        {
            Contract.Requires(!string.IsNullOrEmpty(value));
            var parttern = @"[\u4e00-\u9fa5]+";
            parttern = @"[\S\s]*[\u4E00-\u9FFF]+[\S\s]*+";
            var regex = new Regex(parttern, RegexOptions.IgnoreCase);
            var res = regex.IsMatch(value);
            return res;
        }

        public static void ReplaceStr()
        {
        }

        public static void PickStr()
        {
        }
    }
}