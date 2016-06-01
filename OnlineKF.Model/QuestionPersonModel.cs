using System; 
using System.Text;
using System.Collections.Generic; 
using System.Linq;
using System.Text;

namespace OnlineKF.Model
{
	
		public class QuestionPersonModel
	{

        /// <summary>
        /// 咨询人员信息 自动创建GUID
        /// <summary>		
        public string id { get; set; }
        /// <summary>
        /// 分配的客服id
        /// <summary>		
        public int serviceid { get; set; }
        /// <summary>
        /// 给客服人员评分 （1-5分）
        /// <summary>		
        public int servicelevel { get; set; }
        /// <summary>
        /// 创建时间
        /// <summary>		
        public DateTime createTime { get; set; }
        /// <summary>
        /// 状态：0关闭 1开启
        /// <summary>		
        public int status { get; set; }
		   
	}
}