using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineKF.UI
{
    public partial class SaveHistory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            saveHtml();

            
        }
        protected void Page_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            if (ex is HttpRequestValidationException)
            {
                
                Server.ClearError(); // 如果不ClearError()这个异常会继续传到Application_Error()。

                saveHtml();
            }
        }

        private void saveHtml() {
            var fileName = Request["downChaterName"];
            if (fileName != null && fileName != "")
            {
                fileName = HttpUtility.UrlEncode(fileName);
                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ".html");
               

                var content =   "<!DOCTYPE html><html><head><title>" + fileName + "</title><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />" +
                                "<style type=\"text/css\">" +
                                    "p{margin-bottom: 6px;margin-top: 6px;font-size: 15px;}" +
                                    "p.operator{color: #04428f;text-align: left;word-break: break-all;word-wrap: break-word;font-size: 15px;}" +
                                    "p.operator span{color: #000;text-align: left;word-break: break-all;word-wrap: break-word;font-size: 15px;}" +
                                    "p.visitor{color: #323232;text-align:left;word-break: break-all;word-wrap: break-word;font-size: 15px;}" +
                                    "p.visitor span{color: #000;text-align: left;word-break: break-all;word-wrap: break-word;font-size: 15px;}" +
                                    "p.info{font-weight: normal!important;text-align: left;word-break: break-all;word-wrap: break-word;font-size: 15px;}" +
                               "</style>" +
                               "</head>" +
                               "<body>" + Request["downHistory"].ToString() + "  </body></html>";
                Response.Write(content);
                Response.End();
            }
        }

    }
}