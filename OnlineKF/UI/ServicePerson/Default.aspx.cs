using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OnlineKF.CommonUtil;

namespace OnlineKF.UI.ServicePerson
{
    public partial class Default : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.serviceName.Text = UserUtil.getUserModel.name;
        }
    }
}