using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnlineKF.Model;
using System.Data.SqlClient;
using System.Data;

namespace OnlineKF.DAL
{

    public partial class MessageDataDAL
    {

        #region 数据访问层通用方法


        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from MessageData ");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
            parameters[0].Value = id;


            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 分页查询信息MessageDataModel
        /// </summary>
        /// <param name="pageSize">行数</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="strWhere">条件</param>
        /// <returns></returns>
        public List<MessageDataModel> GetMessageDataList(int pageSize, int pageIndex, string strWhere, string orderStr = " Order By id Desc ")
        {
            StringBuilder strSQl = new StringBuilder();
            strSQl.Append(" select * from(  select *,row_number() over( " + orderStr + ") as rownum from dbo.MessageData where 1=1 " + strWhere + " ) as p  ");

            strSQl.Append(" where rownum between " + ((pageIndex - 1) * pageSize + 1) + " and " + pageIndex * pageSize + "");
            SqlParameter[] parameters = {
				    
                                        };
            List<MessageDataModel> list = new List<MessageDataModel>();
            DataTable dt = DbHelperSQL.Query(strSQl.ToString(), parameters).Tables[0];
            foreach (DataRow row in dt.Rows)
            {
                MessageDataModel model = new MessageDataModel();
                GetModel(row, model);
                list.Add(model);
            }
            return list;
        }


        /// <summary>
        /// 获取总数
        /// </summary>
        public int GetMessageDataCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(id) FROM [MessageData] Where 1=1 ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(strWhere);
            }
            object obj = DbHelperSQL.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public MessageDataModel GetMessageDataModel(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id, questionid, message, onlycount, createtime  ");
            strSql.Append("  from MessageData ");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
            parameters[0].Value = id;


            MessageDataModel model = new MessageDataModel();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);

            if (ds.Tables[0].Rows.Count > 0)
            {
                GetModel(ds.Tables[0].Rows[0], model);
                return model;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public List<MessageDataModel> getModelList(string strWhere)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id, questionid, message, onlycount, createtime  ");
            strSql.Append("  from MessageData ");
            strSql.Append(" where 1=1 " + strWhere);
            SqlParameter[] parameters = { };


            List<MessageDataModel> list = new List<MessageDataModel>();
            DataTable dt = DbHelperSQL.Query(strSql.ToString(), parameters).Tables[0];
            foreach (DataRow row in dt.Rows)
            {
                MessageDataModel model = new MessageDataModel();
                GetModel(row, model);
                list.Add(model);
            }
            return list;

        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(MessageDataModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into MessageData(");
            strSql.Append("questionid,message,backmessage,onlycount,createtime");
            strSql.Append(") values (");
            strSql.Append("@questionid,@message,@backmessage,@onlycount,@createtime");
            strSql.Append(") ");
            strSql.Append(";select @@IDENTITY");

            SqlParameter[] parameters = setParam(model);


            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {

                return Convert.ToInt32(obj);

            }

        }


        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(MessageDataModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update MessageData set ");

            strSql.Append(" questionid = @questionid , ");
            strSql.Append(" message = @message , ");
            strSql.Append(" backmessage = @backmessage , ");
            strSql.Append(" onlycount = @onlycount , ");
            strSql.Append(" createtime = @createtime  ");
            strSql.Append(" where id=@id ");

            SqlParameter[] parameters = setParam(model);

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 设置sqlparam
        /// </summary>
        /// <param name="project"></param>
        private SqlParameter[] setParam(MessageDataModel model)
        {
            SqlParameter[] parameters = {
			            new SqlParameter("@id", SqlDbType.Int,4) ,            
                        new SqlParameter("@questionid", SqlDbType.VarChar,500) ,            
                        new SqlParameter("@message", SqlDbType.Text) ,            
                        new SqlParameter("@backmessage", SqlDbType.Text) ,            
                        new SqlParameter("@onlycount", SqlDbType.Int,4) ,            
                        new SqlParameter("@createtime", SqlDbType.DateTime)             
              
            };

            parameters[0].Value = model.id;
            parameters[1].Value = model.questionid;
            parameters[2].Value = model.message;
            parameters[3].Value = model.backmessage;
            parameters[4].Value = model.onlycount;
            parameters[5].Value = model.createtime; return parameters;
        }
        /// <summary>
        /// 数据转换为 MessageDataModel
        /// </summary>
        /// <param name="row"></param>
        /// <param name="model"></param>
        private MessageDataModel GetModel(DataRow row, MessageDataModel model)
        {

            if (row["id"].ToString() != "")
            {
                model.id = int.Parse(row["id"].ToString());
            }
            model.questionid = row["questionid"].ToString();
            model.message = row["message"].ToString();

            //model.backmessage = row["backmessage"].ToString();
            if (row["onlycount"].ToString() != "")
            {
                model.onlycount = int.Parse(row["onlycount"].ToString());
            }
            if (row["createtime"].ToString() != "")
            {
                model.createtime = DateTime.Parse(row["createtime"].ToString());
            }

            return model;
        }



        #endregion 结束通用方法


        /// <summary>
        /// 累加聊天信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public bool totalInsert(MessageDataModel model, int maxLength)
        {

            SqlParameter[] parameters = {
					new SqlParameter("@questionId", SqlDbType.VarChar,500),
                    new SqlParameter("@content", SqlDbType.NVarChar,4000),
                    new SqlParameter("@maxLength", SqlDbType.Int)
			};
            parameters[0].Value = model.questionid;
            parameters[1].Value = model.message;
            parameters[2].Value = maxLength;

            int rowsAffected = 0;
            DbHelperSQL.RunProcedure("P_MessageData_insert", parameters, out rowsAffected);

            return rowsAffected > 0;
        }

        /// <summary>
        /// 修改聊天信息为已读状态
        /// </summary>
        /// <param name="model"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public bool updateIsOnly(string idstr)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update MessageData set ");
            strSql.Append(" onlycount = 0 ");
            strSql.Append(" where id in (" + idstr + ") ");

            SqlParameter[] parameters = { };

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 修改聊天信息为已读状态
        /// </summary>
        /// <param name="model"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public bool updateIsOnly(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update MessageData set ");
            strSql.Append(" onlycount = 0 ");
            strSql.Append(" where id = " + id + " ");

            SqlParameter[] parameters = { };

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 根据客服人员ID，查询未读信息数量
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public IList<MessageDataModel> queryServicePersonMsg(int serviceId) {
            var strSQl = "select msg.id, msg.questionid,msg.onlycount from dbo.MessageData as msg,dbo.QuestionPerson as qp,dbo.ServicePerson as sp"
                        + " where qp.serviceid = sp.id and msg.questionid = qp.id and sp.id = " + serviceId + " and msg.onlycount > 0  and qp.status = 1 order by msg.onlycount";

            SqlParameter[] parameters = { };

            List<MessageDataModel> list = new List<MessageDataModel>();
            DataTable dt = DbHelperSQL.Query(strSQl.ToString(), parameters).Tables[0];
            foreach (DataRow row in dt.Rows)
            {
                MessageDataModel model = new MessageDataModel();
                model.id = int.Parse(row["id"].ToString());
                model.questionid = row["questionid"].ToString();
                model.onlycount = int.Parse(row["onlycount"].ToString());
                list.Add(model);
            }
            return list;
        }
    }
}

