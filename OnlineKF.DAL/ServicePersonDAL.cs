using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnlineKF.Model;
using System.Data.SqlClient;
using System.Data;

namespace OnlineKF.DAL
{
	 	
		public partial class ServicePersonDAL
	{
   				
   	    #region 数据访问层通用方法

      
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from ServicePerson ");
			strSql.Append(" where id=@id");
						SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;


			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
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
        /// 分页查询信息ServicePersonModel
        /// </summary>
        /// <param name="pageSize">行数</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="strWhere">条件</param>
        /// <returns></returns>
        public List<ServicePersonModel> GetServicePersonList(int pageSize, int pageIndex, string strWhere,string orderStr =" Order By id Desc "){
                	StringBuilder strSQl = new StringBuilder();
            strSQl.Append(" select * from(  select *,row_number() over( "+orderStr+") as rownum from dbo.ServicePerson where 1=1 " + strWhere + " ) as p  ");
            
            strSQl.Append(" where rownum between " + ((pageIndex - 1) * pageSize + 1) + " and " + pageIndex * pageSize + "");
            SqlParameter[] parameters = {
				    
                                        };
            List<ServicePersonModel> list = new List<ServicePersonModel>();
            DataTable dt = DbHelperSQL.Query(strSQl.ToString(), parameters).Tables[0];
            foreach (DataRow row in dt.Rows)
            {
                ServicePersonModel model = new ServicePersonModel();
                GetModel(row, model);
                list.Add(model);
            }
            return list;
                    }
		
		
		/// <summary>
        /// 获取总数
        /// </summary>
        public int GetServicePersonCount(string strWhere)
        {
                    StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(id) FROM [ServicePerson] Where 1=1 ");
            if (strWhere.Trim() != "")
            {
                strSql.Append( strWhere);
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
		public ServicePersonModel GetServicePersonModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select id, name, loginname, loginpwd, age, compayid, personlevel, serviceNumber, maxcount, remark, createTime  ");			
			strSql.Append("  from ServicePerson ");
			strSql.Append(" where id=@id");
						SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			
			ServicePersonModel model=new ServicePersonModel();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			
			if(ds.Tables[0].Rows.Count>0)
			{
				GetModel(ds.Tables[0].Rows[0],model);	
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
		public List<ServicePersonModel> getModelList(string strWhere)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select id, name, loginname, loginpwd, age, compayid, personlevel, serviceNumber, maxcount, remark, createTime  ");			
			strSql.Append("  from ServicePerson ");
			strSql.Append(" where 1=1 "+strWhere);
			 SqlParameter[] parameters = { };
		
			
			List<ServicePersonModel> list = new List<ServicePersonModel>();
            DataTable dt = DbHelperSQL.Query(strSql.ToString(), parameters).Tables[0];
            foreach (DataRow row in dt.Rows)
            {
                ServicePersonModel model = new ServicePersonModel();
                GetModel(row, model);
                list.Add(model);
            }
            return list;
            
		}
		
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(ServicePersonModel model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into ServicePerson(");			
            strSql.Append("name,loginname,loginpwd,age,compayid,personlevel,serviceNumber,maxcount,remark,createTime");
			strSql.Append(") values (");
            strSql.Append("@name,@loginname,@loginpwd,@age,@compayid,@personlevel,@serviceNumber,@maxcount,@remark,@createTime");            
            strSql.Append(") ");            
            strSql.Append(";select @@IDENTITY");		
			
			SqlParameter[] parameters = setParam(model);
            
			   
			object obj = DbHelperSQL.GetSingle(strSql.ToString(),parameters);			
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
		public bool Update(ServicePersonModel model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update ServicePerson set ");
			                                                
            strSql.Append(" name = @name , ");                                    
            strSql.Append(" loginname = @loginname , ");                                    
            strSql.Append(" loginpwd = @loginpwd , ");                                    
            strSql.Append(" age = @age , ");                                    
            strSql.Append(" compayid = @compayid , ");                                    
            strSql.Append(" personlevel = @personlevel , ");                                    
            strSql.Append(" serviceNumber = @serviceNumber , ");                                    
            strSql.Append(" maxcount = @maxcount , ");                                    
            strSql.Append(" remark = @remark , ");                                    
            strSql.Append(" createTime = @createTime  ");            			
			strSql.Append(" where id=@id ");
						
			SqlParameter[] parameters = setParam(model);
            
            int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
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
        private SqlParameter[] setParam(ServicePersonModel model)
        {
            SqlParameter[] parameters = {
			            new SqlParameter("@id", SqlDbType.Int,4) ,            
                        new SqlParameter("@name", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@loginname", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@loginpwd", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@age", SqlDbType.Int,4) ,            
                        new SqlParameter("@compayid", SqlDbType.Int,4) ,            
                        new SqlParameter("@personlevel", SqlDbType.Int,4) ,            
                        new SqlParameter("@serviceNumber", SqlDbType.Int,4) ,            
                        new SqlParameter("@maxcount", SqlDbType.Int,4) ,            
                        new SqlParameter("@remark", SqlDbType.NVarChar,1000) ,            
                        new SqlParameter("@createTime", SqlDbType.DateTime)             
              
            };
			       
            parameters[0].Value = model.id;                   
            parameters[1].Value = model.name;                   
            parameters[2].Value = model.loginname;                   
            parameters[3].Value = model.loginpwd;                   
            parameters[4].Value = model.age;                   
            parameters[5].Value = model.compayid;                   
            parameters[6].Value = model.personlevel;                   
            parameters[7].Value = model.serviceNumber;                   
            parameters[8].Value = model.maxcount;                   
            parameters[9].Value = model.remark;                   
            parameters[10].Value = model.createTime;                        return parameters;
        }
		/// <summary>
        /// 数据转换为 ServicePersonModel
        /// </summary>
        /// <param name="row"></param>
        /// <param name="model"></param>
        private ServicePersonModel GetModel(DataRow row, ServicePersonModel model)
        {
            
           									    if(row["id"].ToString()!="")
					{
						model.id=int.Parse(row["id"].ToString());
					}
																																											model.name= row["name"].ToString();
																																						model.loginname= row["loginname"].ToString();
																																						model.loginpwd= row["loginpwd"].ToString();
																																    if(row["age"].ToString()!="")
					{
						model.age=int.Parse(row["age"].ToString());
					}
																																					    if(row["compayid"].ToString()!="")
					{
						model.compayid=int.Parse(row["compayid"].ToString());
					}
																																					    if(row["personlevel"].ToString()!="")
					{
						model.personlevel=int.Parse(row["personlevel"].ToString());
					}
																																					    if(row["serviceNumber"].ToString()!="")
					{
						model.serviceNumber=int.Parse(row["serviceNumber"].ToString());
					}
																																					    if(row["maxcount"].ToString()!="")
					{
						model.maxcount=int.Parse(row["maxcount"].ToString());
					}
																																											model.remark= row["remark"].ToString();
																																    if(row["createTime"].ToString()!="")
					{
						model.createTime=DateTime.Parse(row["createTime"].ToString());
					}
																																		
				return model;
        }

   		
        
		 #endregion 结束通用方法


        /// <summary>
        /// 根据登录名称查询用户信息
        /// </summary>
        public ServicePersonModel GetServicePersonModel(string loginname)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id, name, loginname, loginpwd, age, compayid, personlevel, serviceNumber, maxcount, remark, createTime  ");
            strSql.Append("  from ServicePerson ");
            strSql.Append(" where loginname=@loginname");
            SqlParameter[] parameters = {
					new SqlParameter("@loginname", SqlDbType.NVarChar,50)
			};
            parameters[0].Value = loginname;


            ServicePersonModel model = new ServicePersonModel();
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
		
	}
}

