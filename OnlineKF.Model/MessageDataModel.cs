using System; 
using System.Text;
using System.Collections.Generic; 
using System.Linq;


namespace OnlineKF.Model
{
	
		public class MessageDataModel
	{

        /// <summary>
        /// id
        /// <summary>		
        public int id { get; set; }
        /// <summary>
        /// 对应咨询用户ID
        /// <summary>		
        public string questionid { get; set; }
        /// <summary>
        /// 咨询聊天信息,超过8万字自动备份到backMessage，以json格式存储{sm:{m:"客服发言",t:"[0 文字 1图片 2文件]"},qm:{m:"咨询人员发言",t:"[0 文字 1图片 2文件]"}} 
        /// <summary>		
        public string message { get; set; }
        /// <summary>
        /// 聊天信息备份
        /// <summary>		
        public string backmessage { get; set; }
        /// <summary>
        /// 未读信息数量
        /// <summary>		
        public int onlycount { get; set; }
        /// <summary>
        /// 创建时间
        /// <summary>		
        public DateTime createtime { get; set; }
		   
	}
}