using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnlineKF.DAL;
using OnlineKF.Model;
namespace OnlineKF.BLL
{
	 	
		public partial class MessageDataBLL
	{
   		     
		private readonly MessageDataDAL dal=new MessageDataDAL();
		public MessageDataBLL()
		{}
		
		private static MessageDataBLL _messageDataBLL = null;
		 /// <summary>
        /// 单例模式
        /// </summary>
        /// <returns></returns>
        public static MessageDataBLL GetInstance()
        {
            if (_messageDataBLL == null)
            {
                _messageDataBLL = new MessageDataBLL();
            }
            return _messageDataBLL;
        }
        
		 #region 通用方法

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(MessageDataModel model)
		{
						return dal.Add(model);
						
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(MessageDataModel model)
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
        /// 分页查询信息 MessageDataModel
        /// </summary>
        /// <param name="pageSize">行数</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="strWhere">条件</param>
        /// <returns></returns>
        public List<MessageDataModel> GetMessageDataList(int pageSize, int pageIndex, string strWhere, out int totalCount,string orderStr = "")
        {
            List<MessageDataModel> result;
            
            if(orderStr == "")
                result = dal.GetMessageDataList(pageSize, pageIndex, strWhere);
            else
                result = dal.GetMessageDataList(pageSize, pageIndex, strWhere, orderStr);

            totalCount = dal.GetMessageDataCount(strWhere);

            return result;
        }

        
		/// <summary>
		/// 咨询人员查询聊天信息
		/// </summary>
		public MessageDataModel GetMessageDataModel(int id)
		{
            var result = dal.GetMessageDataModel(id);
            
            return result;
		}

        /// <summary>
        /// 客服人员查询聊天信息
        /// </summary>
        public MessageDataModel GetServicePersonMsg(int id)
        {
            var result = dal.GetMessageDataModel(id);

            dal.updateIsOnly(id.ToString());

            return result;
        }
        /// <summary>
        /// 获取总数
        /// </summary>
        public int GetMessageDataCount(string strWhere)
        {
           return dal.GetMessageDataCount(strWhere);
        }

   		 #endregion 通用方法

        /// <summary>
        /// 累加聊天信息
        /// </summary>
        public bool totalInsert(MessageDataModel model, int maxLength) {
            return dal.totalInsert(model, maxLength);
        }

        /// <summary>
        /// 得到未读聊天信息
        /// </summary>
        public List<MessageDataModel> getModelList(string strWhere)
        {
            List<MessageDataModel> returnModel =  dal.getModelList(strWhere);
           
            return returnModel;
        }
        /// <summary>
        /// 根据客服人员ID，查询未读信息数量
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public IList<MessageDataModel> queryServicePersonMsg(int serviceId)
        {
            return dal.queryServicePersonMsg(serviceId);
        }

	}
}
