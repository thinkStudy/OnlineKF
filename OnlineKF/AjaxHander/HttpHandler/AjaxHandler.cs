using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Xml;
using System.Web.SessionState;
using System.Collections;
using System.Text.RegularExpressions;

using System.Globalization;
using OnlineKF.Utils;




namespace OnlineKF.AjaxHander.HttpHandler
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class AjaxAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_ajaxMember"></param>
        public AjaxAttribute(bool _ajaxMember)
        {
            this._ajaxMember = _ajaxMember;
        }

        private bool _ajaxMember = false;
        /// <summary>
        /// 
        /// </summary>
        public bool AjaxMember
        {
            get { return _ajaxMember; }
            set { _ajaxMember = value; }
        }
    }

    class AjaxHandler : IHttpHandler, IReadOnlySessionState
    {
        //域名
        private const string responser = "OnlineKF.AjaxHander.";

        public bool IsReusable
        {
            get { return true; }
        }
        public void ProcessRequest(HttpContext context)
        {
            string act = context.Request["act"].Trim();
            string[] tl = act.Split('.');
            var msg = "";
            if (tl.Length == 2)
            {
                object o = null; //返回结果

                string responserType = responser + tl[0].Trim();
                string methodName = tl[1].Trim();

               
                object[] poq = new object[] { };
                Type type = Type.GetType(responserType);
                if (type == null) return;
                MethodInfo method = type.GetMethod(methodName);

                try
                {
                    if (method == null) return;

                    AjaxAttribute aj;
                    bool goon = false;
                    foreach (Attribute attr in method.GetCustomAttributes(true))
                    {
                        aj = attr as AjaxAttribute;
                        if (null != aj)
                        {
                            if (aj.AjaxMember)
                                goon = true;
                        }
                    }
                    if (!goon) return;

                    ParameterInfo[] para = method.GetParameters();
                    poq = new object[para.Length];
                    StreamReader sr = null;
                    byte[] b = new byte[context.Request.ContentLength];
                    if (context.Request.HttpMethod == "POST" && context.Request.InputStream.Read(b, 0, b.Length) >= 0)
                    {
                        sr = new StreamReader(new MemoryStream(b), UTF8Encoding.UTF8);
                        this.RetreiveParameters(ref sr, para, ref poq);
                    }
                }
                catch (Exception ex)
                {
                    LogUtils.LogWriterToIISOut(ex);
                    msg = ex.Message;
                }

                
                if (method.IsStatic)
                {
                    o = type.InvokeMember(methodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod | BindingFlags.IgnoreCase, null, null, poq);
                }
                else
                {
                    object c = (object)Activator.CreateInstance(Type.GetType(responserType), new object[] { });

                    if (c != null)
                    {
                        o = method.Invoke(c, poq);
                    }
                }
              

                #region 结果打印
              
                if (o != null)
                {
                    if (o.GetType() == typeof(XmlDocument))
                    {
                        context.Response.Write(JavaScriptConvert.SerializeXmlNode((XmlDocument)o));
                    }
                    else if (o.GetType() == typeof(string))
                    {
                        context.Response.Write(JavaScriptConvert.ToString(o));
                    }
                    else if (o.GetType() == typeof(bool))
                    {
                        context.Response.Write(o.ToString().ToLower());
                    }
                    else
                    {
                        context.Response.ContentType = "application/json"; //确保输出为json
                        context.Response.Write(JavaScriptConvert.SerializeObject(o));
                    }
                }
                #endregion
            }
        }
        private void RetreiveParameters(ref StreamReader sr, ParameterInfo[] para, ref object[] po)
        {
            for (int i = 0; i < para.Length; i++)
            {
                po[i] = para[i].DefaultValue;
            }

            Hashtable ht = new Hashtable(); 
            try
            {
                string line = sr.ReadLine();
                string[] arrParam = line.Split('&');
                string key, value;
                string param = null;
                foreach (string p in arrParam)
                {

                   // param = p.Replace("%26", "%").Replace("%3D", "=");
                    param = p.Replace("%3D", "=");

                    key = null;
                    for (int i = 0; i < para.Length; i++)
                    {
                        if (param.StartsWith(para[i].Name + "="))
                        {
                            key = para[i].Name;
                            break;
                        }
                    }

                    // Ok, we found a new argument.
                    if (key != null)
                    {
                        value = param.Substring(key.Length + 1);
                        value = HttpUtility.UrlDecode(value);
                        value = ValidateUtil.FilterHTML(value);
                        ht.Add(key, value);
                    }
                }
            }
            catch (Exception) { }
            finally
            {
                sr.Close();
            }

            for (int i = 0; i < para.Length; i++)
            {
                if (ht[para[i].Name] != null)
                {
                    po[i] = FromString(ht[para[i].Name].ToString(), para[i].ParameterType);
                }
            }
        }

        private static object FromString(string s, Type t)
        {
            if (s == null || (s == "null" && t != typeof(String)))
            {
                return null;
            }

            if (t == typeof(String))
            {
                return s;
            }
            else if (t == typeof(Int16))
            {
                return Convert.ToInt16(s);
            }
            else if (t == typeof(Int32))
            {
                return Convert.ToInt32(s);
            }
            else if (t == typeof(Int64))
            {
                return Convert.ToInt64(s);
            }
            else if (t == typeof(Double))
            {
                return Convert.ToDouble(s.Replace(".", CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator));
            }
            else if (t == typeof(Single))
            {
                return Convert.ToSingle(s.Replace(".", CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator));
            }
            else if (t == typeof(Decimal))
            {
                return Convert.ToDecimal(s.Replace(".", CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator));
            }
            else if (t == typeof(Boolean))
            {
                return bool.Parse(s.ToLower());
            }
            else if (t == typeof(bool[]))
            {
                string[] ss = s.Split(',');
                bool[] sb = new bool[ss.Length];

                for (int i = 0; i < ss.Length; i++)
                {
                    sb[i] = bool.Parse(ss[i].ToLower());
                }

                return sb;
            }
            else if (t == typeof(Int16[]))
            {
                string[] ss = s.Split(',');
                Int16[] si = new Int16[ss.Length];
                for (int i = 0; i < ss.Length; i++)
                {
                    si[i] = Int16.Parse(ss[i]);
                }

                return si;
            }
            else if (t == typeof(Int32[]))
            {
                string[] ss = s.Split(',');
                Int32[] si = new Int32[ss.Length];
                for (int i = 0; i < ss.Length; i++)
                {
                    si[i] = Int32.Parse(ss[i]);
                }

                return si;
            }
            else if (t == typeof(Int64[]))
            {
                string[] ss = s.Split(',');
                Int64[] si = new Int64[ss.Length];
                for (int i = 0; i < ss.Length; i++)
                {
                    si[i] = Int64.Parse(ss[i]);
                }

                return si;
            }
            else if (t == typeof(Double[]))
            {
                string[] ss = s.Split(',');
                Double[] si = new Double[ss.Length];
                for (int i = 0; i < ss.Length; i++)
                {
                    si[i] = Double.Parse(ss[i].Replace(".", CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator));
                }

                return si;
            }
            else if (t == typeof(Single[]))
            {
                string[] ss = s.Split(',');
                Single[] si = new Single[ss.Length];
                for (int i = 0; i < ss.Length; i++)
                {
                    si[i] = Single.Parse(ss[i].Replace(".", CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator));
                }

                return si;
            }
            else if (t == typeof(Decimal[]))
            {
                string[] ss = s.Split(',');
                Decimal[] si = new Decimal[ss.Length];
                for (int i = 0; i < ss.Length; i++)
                {
                    si[i] = Decimal.Parse(ss[i].Replace(".", CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator));
                }

                return si;
            }
            else if (t == typeof(string[]) && s.StartsWith("[") && s.EndsWith("]"))
            {
                s = s.Substring(1, s.Length - 2);

                Regex rex = new Regex(
                    // match quoted text if possible
                    // otherwise match until a comma is found
                    @"  ""[^\\""]*""  |  [^,]+  ",
                    RegexOptions.IgnorePatternWhitespace);

                string[] r = new string[rex.Matches(s).Count];

                int i = 0;
                foreach (Match m in rex.Matches(s))
                {
                    r[i++] = m.Value.Substring(1, m.Value.Length - 2);
                }

                return r;
            }

            // TODO: 
            throw new NotImplementedException();
        }
    }
}
