using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineKF.Model
{
    public class ResultModel
    {
        //{\"status\":" + status + ",\"msg\":\"" + msg + "\",\"data\":" + dataStr + "}";

        public int status { get; set; }
        public string msg { get; set; }
        public object data { get; set; }
     
    }

    public class ResultPageModel:ResultModel
    {
        //{\"page\":" + page + ",\"pageSize\":" + pageSize + ",\"totalCount\":" + totalCount + ",\"msg\":\"" + page + "\",\"data\":" + dataStr + "}";

        public int page { get; set; }
        public int pageSize { get; set; }
        public int totalCount { get; set; }
    }
}
