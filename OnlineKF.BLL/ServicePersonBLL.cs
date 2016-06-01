using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnlineKF.DAL;
using OnlineKF.Model;
namespace OnlineKF.BLL
{
	 	
		public partial class ServicePersonBLL
	{
   		     
		private readonly ServicePersonDAL dal=new ServicePersonDAL();
		public ServicePersonBLL()
		{}
		
		private static ServicePersonBLL _servicePersonBLL = null;
		 /// <summary>
        /// 单例模式
        /// </summary>
        /// <returns></returns>
        public static ServicePersonBLL GetInstance()
        {
            if (_servicePersonBLL == null)
            {
                _servicePersonBLL = new ServicePersonBLL();
            }
            return _servicePersonBLL;
        }
        
		 #region 通用方法

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(ServicePersonModel model)
		{
						return dal.Add(model);
						
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(ServicePersonModel model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int id)
		{
			
			return dal.Delete(id);
		}
	

		/// <summary>
        /// 分页查询信息 ServicePersonModel
        /// </summary>
        /// <param name="pageSize">行数</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="strWhere">条件</param>
        /// <returns></returns>
        public List<ServicePersonModel> GetServicePersonList(int pageSize, int pageIndex, string strWhere, out int totalCount,string orderStr = "")
        {
            List<ServicePersonModel> result;
            
            if(orderStr == "")
                result = dal.GetServicePersonList(pageSize, pageIndex, strWhere);
            else
                result = dal.GetServicePersonList(pageSize, pageIndex, strWhere, orderStr);

            totalCount = dal.GetServicePersonCount(strWhere);

            return result;
        }

        
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public ServicePersonModel GetServicePersonModel(int id)
		{
			return dal.GetServicePersonModel(id);
		}
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public List<ServicePersonModel> getModelList(string strWhere)
		{
			return dal.getModelList(strWhere);
		}
        /// <summary>
        /// 获取总数
        /// </summary>
        public int GetServicePersonCount(string strWhere)
        {
           return dal.GetServicePersonCount(strWhere);
        }

   		 #endregion 通用方法

             /// <summary>
        /// 根据登录名称查询用户信息
        /// </summary>
        public ServicePersonModel GetServicePersonModel(string loginname) {
            return dal.GetServicePersonModel(loginname);
        }
	}
}
