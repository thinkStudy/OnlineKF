using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

namespace OnlineKF.Utils
{
    /// <summary>
    /// XssFilter 跨战脚本过滤器
    /// </summary>
    public class XssFilter
    {
        public XssFilter()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 过滤xss攻击
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FilterXss(string input)
        {
            if (StringTools.IsBlank(input))
                return input;
            //过滤所有事件
            input = FilterEven(input);
            //过滤标签
            input = FilterTags(input);
            //过滤样式表中的Expression
            input = FilterCssExp(input);
            //过滤协议头
            input = FilterProto(input);
            //过滤转义字符
            input = FilterHex(input);
            //过滤&#+ascii格式
            input = FilterAscII(input);
            return input;
        }



        /// <summary>
        /// 过滤所有事件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FilterEven(string input)
        {
            if (StringTools.IsBlank(input))
                return input;
            string pattern = @"(<[^>]*)(\bon[\w ]*=)([^>]*>)";
            string replacement = @"${1}_disEven=${3}";
            Regex reg = new Regex(pattern, RegexOptions.IgnoreCase);
            //循环替换
            return LoopReplace(reg, input, replacement);
        }

        /// <summary>
        /// 过滤标签
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FilterTags(string input)
        {
            if (StringTools.IsBlank(input))
                return input;
            string[] tags = { "form", "iframe", "frameset", "frame", "applet", "script" };
            string replacement = @"$1_disTag";
            foreach (string tag in tags)
            {
                string pattern = @"(<|</)" + tag + "";
                Regex reg = new Regex(pattern, RegexOptions.IgnoreCase);
                input = reg.Replace(input, replacement);
            }
            return input;
        }

        /// <summary>
        /// 过滤样式表中的expression
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FilterCssExp(string input)
        {
            if (StringTools.IsBlank(input))
                return input;
            //去掉注释     
            //(<[^>]*style=[\s\S]*?)/\*[\s\S]*?\*/([^>]*>)
            string pattern = @"(<[^>]*style=[^>]*)/\*[\s\S]*?\*/([^>]*>)";
            string replacement = @"${1}${2}";
            Regex reg = new Regex(pattern, RegexOptions.IgnoreCase);
            //循环过滤
            input = LoopReplace(reg, input, replacement);

            //过滤Expression
            pattern = @"(<[^>]*style=[^>]*)(expression)([^>]*>)";
            replacement = @"$1_ExpDisabled$3";
            reg = new Regex(pattern, RegexOptions.IgnoreCase);
            input = LoopReplace(reg, input, replacement);
            return input;
        }

        /// <summary>
        /// 过滤协议头
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FilterProto(string input)
        {
            if (StringTools.IsBlank(input))
                return input;
            string[] protocols = new string[]{ "javascript", "vbscript", "livescript", 
            "ms-its", "mhtml", "data", "firefoxurl", "mocha","-moz-binding" };

            string replacement = @"$1/$2";
            foreach (string protocol in protocols)
            {
                string pattern = @"(<[^>]*)" + protocol + @":([^>]*>)";
                Regex reg = new Regex(pattern, RegexOptions.IgnoreCase);
                input = reg.Replace(input, replacement);
            }
            return input;
        }

        /// <summary>
        /// 过滤转义字符\ 加 16进制格式 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FilterHex(string input)
        {
            if (StringTools.IsBlank(input))
                return input;
            // 转义字符\ 加 16进制格式 (style和javascript中均支持)
            // 如<img STYLE="background-image: \75\72\6c\28\6a\61\76\61\73\63\72\69\70\74\3a\61\6c\65\72\74\28\27\58\53\53\27\29\29"> 
            string pattern = @"(<[^>]*)(\\)([^>]*>)";
            string replacement = @"${1}${3}";
            Regex reg = new Regex(pattern, RegexOptions.IgnoreCase);
            return LoopReplace(reg, input, replacement);
        }

        /// <summary>
        /// 过滤&# 加 ASCII格式 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FilterAscII(string input)
        {
            if (StringTools.IsBlank(input))
                return input;
            //&# 加 ASCII格式
            //如<IMG SRC=&#106;&#97;&#118;&#97;&#115;&#99;&#114;&#105;&#112;&#116;&#58;&#97;&#108;&#101;&#114;&#116;&#40;&#39;&#88;&#83;&#83;&#39;&#41;> 
            string pattern = @"(<[^>]*)(&#)([^>]*>)";
            string replacement = @"${1}${3}";
            Regex reg = new Regex(pattern, RegexOptions.IgnoreCase);
            return LoopReplace(reg, input, replacement);
        }

        /// <summary>
        /// 循环替换，直到无匹配项为止
        /// </summary>
        /// <param name="reg"></param>
        /// <param name="input"></param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public static string LoopReplace(Regex reg, string input, string replacement)
        {
            if (StringTools.IsBlank(input))
                return input;
            while (reg.Matches(input).Count > 0)
            {
                input = reg.Replace(input, replacement);
            }
            return input;
        }

    }
}
