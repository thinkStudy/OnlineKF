using System; 
using System.Text;
using System.Collections.Generic; 
using System.Linq;
using System.Text;

namespace OnlineKF.Model
{
	
		public class ServicePersonModel
	{
   		     
      	/// <summary>
		/// 客服人员ID
        /// <summary>		
        public int id{ get; set;}
		/// <summary>
		/// 真实姓名
        /// <summary>		
        public string name{ get; set;}
		/// <summary>
		/// 登录名称
        /// <summary>		
        public string loginname{ get; set;}
		/// <summary>
		/// 登录密码
        /// <summary>		
        public string loginpwd{ get; set;}
		/// <summary>
		/// 年龄
        /// <summary>		
        public int age{ get; set;}
		/// <summary>
		/// 所属公司 对应公司ID
        /// <summary>		
        public int compayid{ get; set;}
		/// <summary>
		/// 服务人员等级
        /// <summary>		
        public int personlevel{ get; set;}
		/// <summary>
		/// 当前服务的人数，优先分配人数少的客服
        /// <summary>		
        public int serviceNumber{ get; set;}
		/// <summary>
		/// 最大服务人数 0 无限
        /// <summary>		
        public int maxcount{ get; set;}
		/// <summary>
		/// 备用字段
        /// <summary>		
        public string remark{ get; set;}
		/// <summary>
		/// createTime
        /// <summary>		
        public DateTime createTime{ get; set;}
		   
	}
}