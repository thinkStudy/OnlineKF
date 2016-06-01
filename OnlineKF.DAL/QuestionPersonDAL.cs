using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnlineKF.Model;
using System.Data.SqlClient;
using System.Data;


namespace OnlineKF.DAL
{

    public partial class QuestionPersonDAL
    {

        #region 数据访问层通用方法


        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from QuestionPerson ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.VarChar,500)			};
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
        /// 分页查询信息QuestionPersonModel
        /// </summary>
        /// <param name="pageSize">行数</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="strWhere">条件</param>
        /// <returns></returns>
        public List<QuestionPersonModel> GetQuestionPersonList(int pageSize, int pageIndex, string strWhere, string orderStr = " Order By id Desc ")
        {
            StringBuilder strSQl = new StringBuilder();
            strSQl.Append(" select * from(  select *,row_number() over( " + orderStr + ") as rownum from dbo.QuestionPerson where 1=1 " + strWhere + " ) as p  ");

            strSQl.Append(" where rownum between " + ((pageIndex - 1) * pageSize + 1) + " and " + pageIndex * pageSize + "");
            SqlParameter[] parameters = {
				    
                                        };
            List<QuestionPersonModel> list = new List<QuestionPersonModel>();
            DataTable dt = DbHelperSQL.Query(strSQl.ToString(), parameters).Tables[0];
            foreach (DataRow row in dt.Rows)
            {
                QuestionPersonModel model = new QuestionPersonModel();
                GetModel(row, model);
                list.Add(model);
            }
            return list;
        }


        /// <summary>
        /// 获取总数
        /// </summary>
        public int GetQuestionPersonCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(id) FROM [QuestionPerson] Where 1=1 ");
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
        public QuestionPersonModel GetQuestionPersonModel(string id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id, serviceid, servicelevel, createTime, status  ");
            strSql.Append("  from QuestionPerson ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.VarChar,500)			};
            parameters[0].Value = id;


            QuestionPersonModel model = new QuestionPersonModel();
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
        public List<QuestionPersonModel> getModelList(string strWhere)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id, serviceid, servicelevel, createTime, status  ");
            strSql.Append("  from QuestionPerson ");
            strSql.Append(" where 1=1 " + strWhere);
            SqlParameter[] parameters = { };


            List<QuestionPersonModel> list = new List<QuestionPersonModel>();
            DataTable dt = DbHelperSQL.Query(strSql.ToString(), parameters).Tables[0];
            foreach (DataRow row in dt.Rows)
            {
                QuestionPersonModel model = new QuestionPersonModel();
                GetModel(row, model);
                list.Add(model);
            }
            return list;

        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(QuestionPersonModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into QuestionPerson(");
            strSql.Append("id,serviceid,servicelevel,createTime,status");
            strSql.Append(") values (");
            strSql.Append("@id,@serviceid,@servicelevel,@createTime,@status");
            strSql.Append(") ");
            strSql.Append(";select top 1 id from QuestionPerson where id ='"+model.id+"'");

            SqlParameter[] parameters = setParam(model);

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return false;
            }
            else
            {

                return true;

            }

        }


        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(QuestionPersonModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update QuestionPerson set ");

            strSql.Append(" id = @id , ");
            strSql.Append(" serviceid = @serviceid , ");
            strSql.Append(" servicelevel = @servicelevel , ");
            strSql.Append(" createTime = @createTime , ");
            strSql.Append(" status = @status  ");
            strSql.Append(" where id=@id  ");

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
        private SqlParameter[] setParam(QuestionPersonModel model)
        {
            SqlParameter[] parameters = {
			            new SqlParameter("@id", SqlDbType.VarChar,500) ,            
                        new SqlParameter("@serviceid", SqlDbType.Int,4) ,            
                        new SqlParameter("@servicelevel", SqlDbType.Int,4) ,            
                        new SqlParameter("@createTime", SqlDbType.DateTime) ,            
                        new SqlParameter("@status", SqlDbType.Int,4)             
              
            };

            parameters[0].Value = model.id;
            parameters[1].Value = model.serviceid;
            parameters[2].Value = model.servicelevel;
            parameters[3].Value = model.createTime;
            parameters[4].Value = model.status; return parameters;
        }
        /// <summary>
        /// 数据转换为 QuestionPersonModel
        /// </summary>
        /// <param name="row"></param>
        /// <param name="model"></param>
        private QuestionPersonModel GetModel(DataRow row, QuestionPersonModel model)
        {

            model.id = row["id"].ToString();
            if (row["serviceid"].ToString() != "")
            {
                model.serviceid = int.Parse(row["serviceid"].ToString());
            }
            if (row["servicelevel"].ToString() != "")
            {
                model.servicelevel = int.Parse(row["servicelevel"].ToString());
            }
            if (row["createTime"].ToString() != "")
            {
                model.createTime = DateTime.Parse(row["createTime"].ToString());
            }
            if (row["status"].ToString() != "")
            {
                model.status = int.Parse(row["status"].ToString());
            }

            return model;
        }



        #endregion 结束通用方法


        /// <summary>
        /// 关闭咨询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool closeQuestion(string id) {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update QuestionPerson set ");
            strSql.Append(" status = 0  ");
            strSql.Append(" where id=" + id + "  ");

            SqlParameter[] parameters = {};

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
        /// 给客服人员评分
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool setServiceLevel(QuestionPersonModel model)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update QuestionPerson set ");
            strSql.Append(" servicelevel = " + model.servicelevel + " ");
            strSql.Append(" where id='" + model.id + "'  ");

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

    }
}

