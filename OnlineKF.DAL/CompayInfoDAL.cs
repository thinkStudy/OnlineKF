using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnlineKF.Model;
using System.Data.SqlClient;
using System.Data;

namespace OnlineKF.DAL
{
	 	
		public partial class CompayInfoDAL
	{
   				
   	    #region 数据访问层通用方法

      
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int compayid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from CompayInfo ");
			strSql.Append(" where compayid=@compayid");
						SqlParameter[] parameters = {
					new SqlParameter("@compayid", SqlDbType.Int,4)
			};
			parameters[0].Value = compayid;


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
        /// 分页查询信息CompayInfoModel
        /// </summary>
        /// <param name="pageSize">行数</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="strWhere">条件</param>
        /// <returns></returns>
        public List<CompayInfoModel> GetCompayInfoList(int pageSize, int pageIndex, string strWhere,string orderStr =" Order By compayid Desc "){
                	StringBuilder strSQl = new StringBuilder();
            strSQl.Append(" select * from(  select *,row_number() over( "+orderStr+") as rownum from dbo.CompayInfo where 1=1 " + strWhere + " ) as p  ");
            
            strSQl.Append(" where rownum between " + ((pageIndex - 1) * pageSize + 1) + " and " + pageIndex * pageSize + "");
            SqlParameter[] parameters = {
				    
                                        };
            List<CompayInfoModel> list = new List<CompayInfoModel>();
            DataTable dt = DbHelperSQL.Query(strSQl.ToString(), parameters).Tables[0];
            foreach (DataRow row in dt.Rows)
            {
                CompayInfoModel model = new CompayInfoModel();
                GetModel(row, model);
                list.Add(model);
            }
            return list;
                    }
		
		
		/// <summary>
        /// 获取总数
        /// </summary>
        public int GetCompayInfoCount(string strWhere)
        {
                    StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(compayid) FROM [CompayInfo] Where 1=1 ");
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
		public CompayInfoModel GetCompayInfoModel(int compayid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select compayid, name, compayinfo, type, remark  ");			
			strSql.Append("  from CompayInfo ");
			strSql.Append(" where compayid=@compayid");
						SqlParameter[] parameters = {
					new SqlParameter("@compayid", SqlDbType.Int,4)
			};
			parameters[0].Value = compayid;

			
			CompayInfoModel model=new CompayInfoModel();
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
		public List<CompayInfoModel> getModelList(string strWhere)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select compayid, name, compayinfo, type, remark  ");			
			strSql.Append("  from CompayInfo ");
			strSql.Append(" where 1=1 "+strWhere);
			 SqlParameter[] parameters = { };
		
			
			List<CompayInfoModel> list = new List<CompayInfoModel>();
            DataTable dt = DbHelperSQL.Query(strSql.ToString(), parameters).Tables[0];
            foreach (DataRow row in dt.Rows)
            {
                CompayInfoModel model = new CompayInfoModel();
                GetModel(row, model);
                list.Add(model);
            }
            return list;
            
		}
		
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(CompayInfoModel model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into CompayInfo(");			
            strSql.Append("name,compayinfo,type,remark");
			strSql.Append(") values (");
            strSql.Append("@name,@compayinfo,@type,@remark");            
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
		public bool Update(CompayInfoModel model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update CompayInfo set ");
			                                                
            strSql.Append(" name = @name , ");                                    
            strSql.Append(" compayinfo = @compayinfo , ");                                    
            strSql.Append(" type = @type , ");                                    
            strSql.Append(" remark = @remark  ");            			
			strSql.Append(" where compayid=@compayid ");
						
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
        private SqlParameter[] setParam(CompayInfoModel model)
        {
            SqlParameter[] parameters = {
			            new SqlParameter("@compayid", SqlDbType.Int,4) ,            
                        new SqlParameter("@name", SqlDbType.NVarChar,100) ,            
                        new SqlParameter("@compayinfo", SqlDbType.NVarChar,4000) ,            
                        new SqlParameter("@type", SqlDbType.Int,4) ,            
                        new SqlParameter("@remark", SqlDbType.NVarChar,1000)             
              
            };
			       
            parameters[0].Value = model.compayid;                   
            parameters[1].Value = model.name;                   
            parameters[2].Value = model.compayinfo;                   
            parameters[3].Value = model.type;                   
            parameters[4].Value = model.remark;                        return parameters;
        }
		/// <summary>
        /// 数据转换为 CompayInfoModel
        /// </summary>
        /// <param name="row"></param>
        /// <param name="model"></param>
        private CompayInfoModel GetModel(DataRow row, CompayInfoModel model)
        {
            
           									    if(row["compayid"].ToString()!="")
					{
						model.compayid=int.Parse(row["compayid"].ToString());
					}
																																											model.name= row["name"].ToString();
																																						model.compayinfo= row["compayinfo"].ToString();
																																    if(row["type"].ToString()!="")
					{
						model.type=int.Parse(row["type"].ToString());
					}
																																											model.remark= row["remark"].ToString();
																													
				return model;
        }

   		
        
		 #endregion 结束通用方法
		 
		
        
		
	}
}

