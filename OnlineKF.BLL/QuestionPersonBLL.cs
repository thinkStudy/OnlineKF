using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnlineKF.DAL;
using OnlineKF.Model;

namespace OnlineKF.BLL
{

    public partial class QuestionPersonBLL
    {

        private readonly QuestionPersonDAL dal = new QuestionPersonDAL();
        public QuestionPersonBLL()
        { }

        private static QuestionPersonBLL _questionPersonBLL = null;
        /// <summary>
        /// 单例模式
        /// </summary>
        /// <returns></returns>
        public static QuestionPersonBLL GetInstance()
        {
            if (_questionPersonBLL == null)
            {
                _questionPersonBLL = new QuestionPersonBLL();
            }
            return _questionPersonBLL;
        }

        #region 通用方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(QuestionPersonModel model)
        {
            return dal.Add(model);

        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(QuestionPersonModel model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string id)
        {

            return dal.Delete(id);
        }


        /// <summary>
        /// 分页查询信息 QuestionPersonModel
        /// </summary>
        /// <param name="pageSize">行数</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="strWhere">条件</param>
        /// <returns></returns>
        public List<QuestionPersonModel> GetQuestionPersonList(int pageSize, int pageIndex, string strWhere, out int totalCount, string orderStr = "")
        {
            List<QuestionPersonModel> result;

            if (orderStr == "")
                result = dal.GetQuestionPersonList(pageSize, pageIndex, strWhere);
            else
                result = dal.GetQuestionPersonList(pageSize, pageIndex, strWhere, orderStr);

            totalCount = dal.GetQuestionPersonCount(strWhere);

            return result;
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public QuestionPersonModel GetQuestionPersonModel(string id)
        {
            return dal.GetQuestionPersonModel(id);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public List<QuestionPersonModel> getModelList(string strWhere)
        {
            return dal.getModelList(strWhere);
        }
        /// <summary>
        /// 获取总数
        /// </summary>
        public int GetQuestionPersonCount(string strWhere)
        {
            return dal.GetQuestionPersonCount(strWhere);
        }

        #endregion 通用方法

        public bool closeQuestion(string id)
        {
            return dal.closeQuestion(id);
        }

        /// <summary>
        /// 给客服人员评分
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool setServiceLevel(QuestionPersonModel model)
        {
            return dal.setServiceLevel(model);
        }
    }
}
