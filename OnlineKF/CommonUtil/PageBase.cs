using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Text;
using OnlineKF.CommonUtil;
using OnlineKF.Utils;

public class PageBase : System.Web.UI.Page
{

    /// <summary>
    /// 构造函数
    /// </summary>
    public PageBase()
    {


        // Page.Init += new EventHandler(PageTitle);
    }
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        checkLogin();

        if (Request.Headers["X-Requested-With"] != "XMLHttpRequest" && !IsPostBack)
        {
            PageTitle(null, null);
        }

    }


    /// <summary>
    /// 页面加载
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected virtual void PageTitle(object sender, EventArgs e)
    {

        if (Header != null)
        {
            Literal ltr = new Literal();
            StringBuilder sb = new StringBuilder();
           
            sb.Append("\r\n<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" /> \r\n")
            .Append("<meta name=\"keywords\" content=\"").Append(ConfigHelper.GetConfigString("webKeyWords")).Append("\" />\r\n")
            .Append("<meta name=\"description\" content=\"").Append(ConfigHelper.GetConfigString("webDescription")).Append("\" />\r\n")
            .Append("<link href=\"/css/base.css\" rel=\"stylesheet\" type=\"text/css\" media=\"all\" />\r\n")
            .Append("<script src=\"/js/jquery-1.9.1.min.js\" type=\"text/javascript\"></script>\r\n")
            .Append("<script src=\"/js/plugins/bind_data_util.js\" type=\"text/javascript\"></script>\r\n")
            .Append("<script src=\"/js/global.js\" type=\"text/javascript\"></script>\r\n")
            .Append("<script src=\"/js/base.js\" type=\"text/javascript\"></script>\r\n");
         
            ltr.Text = sb.ToString();
            Header.Controls.AddAt(1, ltr);
        }

    }

    private void checkLogin()
    {
        //登录验证

        if (!UserUtil.isLogin())
        {
            Response.Redirect("../../Login.html");
        }
        

    }

}
