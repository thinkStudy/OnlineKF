using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnlineKF.DAL;
using OnlineKF.Model;
namespace OnlineKF.BLL
{
	 	
		public partial class CompayInfoBLL
	{
   		     
		private readonly CompayInfoDAL dal=new CompayInfoDAL();
		public CompayInfoBLL()
		{}
		
		private static CompayInfoBLL _compayInfoBLL = null;
		 /// <summary>
        /// 单例模式
        /// </summary>
        /// <returns></returns>
        public static CompayInfoBLL GetInstance()
        {
            if (_compayInfoBLL == null)
            {
                _compayInfoBLL = new CompayInfoBLL();
            }
            return _compayInfoBLL;
        }
        
		 #region 通用方法

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(CompayInfoModel model)
		{
						return dal.Add(model);
						
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(CompayInfoModel model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int compayid)
		{
			
			return dal.Delete(compayid);
		}
	

		/// <summary>
        /// 分页查询信息 CompayInfoModel
        /// </summary>
        /// <param name="pageSize">行数</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="strWhere">条件</param>
        /// <returns></returns>
        public List<CompayInfoModel> GetCompayInfoList(int pageSize, int pageIndex, string strWhere, out int totalCount,string orderStr = "")
        {
            List<CompayInfoModel> result;
            
            if(orderStr == "")
                result = dal.GetCompayInfoList(pageSize, pageIndex, strWhere);
            else
                result = dal.GetCompayInfoList(pageSize, pageIndex, strWhere, orderStr);

            totalCount = dal.GetCompayInfoCount(strWhere);

            return result;
        }

        
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public CompayInfoModel GetCompayInfoModel(int compayid)
		{
			return dal.GetCompayInfoModel(compayid);
		}
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public List<CompayInfoModel> getModelList(string strWhere)
		{
			return dal.getModelList(strWhere);
		}
        /// <summary>
        /// 获取总数
        /// </summary>
        public int GetCompayInfoCount(string strWhere)
        {
           return dal.GetCompayInfoCount(strWhere);
        }

   		 #endregion 通用方法
	}
}
