using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnlineKF.Model;
using System.Data.SqlClient;
using System.Data;

namespace OnlineKF.DAL
{

    public partial class AgileReplyDAL
    {

        #region 数据访问层通用方法


        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from AgileReply ");
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
        /// 分页查询信息AgileReplyModel
        /// </summary>
        /// <param name="pageSize">行数</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="strWhere">条件</param>
        /// <returns></returns>
        public List<AgileReplyModel> GetAgileReplyList(int pageSize, int pageIndex, string strWhere, string orderStr = " Order By id Desc ")
        {
            StringBuilder strSQl = new StringBuilder();
            strSQl.Append(" select * from(  select *,row_number() over( " + orderStr + ") as rownum from dbo.AgileReply where 1=1 " + strWhere + " ) as p  ");

            strSQl.Append(" where rownum between " + ((pageIndex - 1) * pageSize + 1) + " and " + pageIndex * pageSize + "");
            SqlParameter[] parameters = {
				    
                                        };
            List<AgileReplyModel> list = new List<AgileReplyModel>();
            DataTable dt = DbHelperSQL.Query(strSQl.ToString(), parameters).Tables[0];
            foreach (DataRow row in dt.Rows)
            {
                AgileReplyModel model = new AgileReplyModel();
                GetModel(row, model);
                list.Add(model);
            }
            return list;
        }


        /// <summary>
        /// 获取总数
        /// </summary>
        public int GetAgileReplyCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(id) FROM [AgileReply] Where 1=1 ");
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
        public AgileReplyModel GetAgileReplyModel(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id, messagetxt, usenumber, company, serviceId, hasglobal, createdate  ");
            strSql.Append("  from AgileReply ");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
            parameters[0].Value = id;


            AgileReplyModel model = new AgileReplyModel();
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
        public List<AgileReplyModel> getModelList(string strWhere)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id, messagetxt, usenumber, company, serviceId, hasglobal, createdate  ");
            strSql.Append("  from AgileReply ");
            strSql.Append(" where 1=1 " + strWhere);
            SqlParameter[] parameters = { };


            List<AgileReplyModel> list = new List<AgileReplyModel>();
            DataTable dt = DbHelperSQL.Query(strSql.ToString(), parameters).Tables[0];
            foreach (DataRow row in dt.Rows)
            {
                AgileReplyModel model = new AgileReplyModel();
                GetModel(row, model);
                list.Add(model);
            }
            return list;

        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(AgileReplyModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into AgileReply(");
            strSql.Append("messagetxt,usenumber,company,serviceId,hasglobal,createdate");
            strSql.Append(") values (");
            strSql.Append("@messagetxt,@usenumber,@company,@serviceId,@hasglobal,@createdate");
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
        public bool Update(AgileReplyModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update AgileReply set ");

            strSql.Append(" messagetxt = @messagetxt , ");
            strSql.Append(" usenumber = @usenumber , ");
            strSql.Append(" company = @company , ");
            strSql.Append(" serviceId = @serviceId , ");
            strSql.Append(" hasglobal = @hasglobal , ");
            strSql.Append(" createdate = @createdate  ");
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
        private SqlParameter[] setParam(AgileReplyModel model)
        {
            SqlParameter[] parameters = {
			            new SqlParameter("@id", SqlDbType.Int,4) ,            
                        new SqlParameter("@messagetxt", SqlDbType.NVarChar,500) ,            
                        new SqlParameter("@usenumber", SqlDbType.Int,4) ,            
                        new SqlParameter("@company", SqlDbType.Int,4) ,            
                        new SqlParameter("@serviceId", SqlDbType.Int,4) ,            
                        new SqlParameter("@hasglobal", SqlDbType.Bit,1) ,            
                        new SqlParameter("@createdate", SqlDbType.DateTime)             
              
            };

            parameters[0].Value = model.id;
            parameters[1].Value = model.messagetxt;
            parameters[2].Value = model.usenumber;
            parameters[3].Value = model.company;
            parameters[4].Value = model.serviceId;
            parameters[5].Value = model.hasglobal;
            parameters[6].Value = model.createdate; return parameters;
        }
        /// <summary>
        /// 数据转换为 AgileReplyModel
        /// </summary>
        /// <param name="row"></param>
        /// <param name="model"></param>
        private AgileReplyModel GetModel(DataRow row, AgileReplyModel model)
        {

            if (row["id"].ToString() != "")
            {
                model.id = int.Parse(row["id"].ToString());
            }
            model.messagetxt = row["messagetxt"].ToString();
            if (row["usenumber"].ToString() != "")
            {
                model.usenumber = int.Parse(row["usenumber"].ToString());
            }
            if (row["company"].ToString() != "")
            {
                model.company = int.Parse(row["company"].ToString());
            }
            if (row["serviceId"].ToString() != "")
            {
                model.serviceId = int.Parse(row["serviceId"].ToString());
            }
            if (row["hasglobal"].ToString() != "")
            {
                if ((row["hasglobal"].ToString() == "1") || (row["hasglobal"].ToString().ToLower() == "true"))
                {
                    model.hasglobal = true;
                }
                else
                {
                    model.hasglobal = false;
                }
            }
            if (row["createdate"].ToString() != "")
            {
                model.createdate = DateTime.Parse(row["createdate"].ToString());
            }

            return model;
        }



        #endregion 结束通用方法




        /// <summary>
        /// 记录使用次数
        /// </summary>
        public bool logUseNumber(AgileReplyModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update AgileReply set ");
            strSql.Append(" usenumber = @usenumber  ");
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
    }
}

