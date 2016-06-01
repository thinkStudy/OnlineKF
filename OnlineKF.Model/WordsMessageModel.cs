using System; 
using System.Text;
using System.Collections.Generic; 
using System.Linq;
using System.Text;

namespace OnlineKF.Model
{
	
		public class WordsMessageModel
	{
   		     
      	/// <summary>
		/// id
        /// <summary>		
        public int id{ get; set;}
		/// <summary>
		/// 留言内容
        /// <summary>		
        public string messagecontent{ get; set;}
		/// <summary>
		/// 联系邮箱
        /// <summary>		
        public string email{ get; set;}
		/// <summary>
		/// 姓名
        /// <summary>		
        public string name{ get; set;}
		/// <summary>
		/// 联系电话
        /// <summary>		
        public string phone{ get; set;}
		/// <summary>
		/// 性别 0：男 1：女
        /// <summary>		
        public int sex{ get; set;}
		/// <summary>
		/// 分配的客服id
        /// <summary>		
        public int serviceid{ get; set;}
		/// <summary>
		/// 创建时间
        /// <summary>		
        public DateTime createData{ get; set;}
		   
	}
}