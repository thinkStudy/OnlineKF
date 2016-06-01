using System; 
using System.Text;
using System.Collections.Generic; 
using System.Linq;
using System.Text;

namespace OnlineKF.Model
{
	
		public class CompayInfoModel
	{
   		     
      	/// <summary>
		/// 企业ID
        /// <summary>		
        public int compayid{ get; set;}
		/// <summary>
		/// 企业名称
        /// <summary>		
        public string name{ get; set;}
		/// <summary>
		/// 企业信息
        /// <summary>		
        public string compayinfo{ get; set;}
		/// <summary>
		/// 企业类型 0游戏 
        /// <summary>		
        public int type{ get; set;}
		/// <summary>
		/// 备用字段
        /// <summary>		
        public string remark{ get; set;}
		   
	}
}