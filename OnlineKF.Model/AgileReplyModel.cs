using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineKF.Model
{

    public class AgileReplyModel
    {

        /// <summary>
        /// id
        /// <summary>		
        public int id { get; set; }
        /// <summary>
        /// 快捷回复内容
        /// <summary>		
        public string messagetxt { get; set; }
        /// <summary>
        /// 使用次数
        /// <summary>		
        public int usenumber { get; set; }
        /// <summary>
        /// 对应企业Id
        /// <summary>		
        public int company { get; set; }
        /// <summary>
        /// 对应客服ID
        /// <summary>		
        public int serviceId { get; set; }
        /// <summary>
        /// 是否共用，可指定添加人使用
        /// <summary>		
        public bool hasglobal { get; set; }
        /// <summary>
        /// 创建时间
        /// <summary>		
        public DateTime createdate { get; set; }

    }
}