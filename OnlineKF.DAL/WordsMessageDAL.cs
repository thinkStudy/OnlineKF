using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnlineKF.Model;
using System.Data.SqlClient;
using System.Data;

namespace OnlineKF.DAL
{
	 	
		public partial class WordsMessageDAL
	{
   				
   	    #region 数据访问层通用方法

      
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from WordsMessage ");
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
        /// 分页查询信息WordsMessageModel
        /// </summary>
        /// <param name="pageSize">行数</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="strWhere">条件</param>
        /// <returns></returns>
        public List<WordsMessageModel> GetWordsMessageList(int pageSize, int pageIndex, string strWhere,string orderStr =" Order By id Desc "){
                	StringBuilder strSQl = new StringBuilder();
            strSQl.Append(" select * from(  select *,row_number() over( "+orderStr+") as rownum from dbo.WordsMessage where 1=1 " + strWhere + " ) as p  ");
            
            strSQl.Append(" where rownum between " + ((pageIndex - 1) * pageSize + 1) + " and " + pageIndex * pageSize + "");
            SqlParameter[] parameters = {
				    
                                        };
            List<WordsMessageModel> list = new List<WordsMessageModel>();
            DataTable dt = DbHelperSQL.Query(strSQl.ToString(), parameters).Tables[0];
            foreach (DataRow row in dt.Rows)
            {
                WordsMessageModel model = new WordsMessageModel();
                GetModel(row, model);
                list.Add(model);
            }
            return list;
                    }
		
		
		/// <summary>
        /// 获取总数
        /// </summary>
        public int GetWordsMessageCount(string strWhere)
        {
                    StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(id) FROM [WordsMessage] Where 1=1 ");
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
		public WordsMessageModel GetWordsMessageModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select id, messagecontent, email, name, phone, sex, serviceid, createData  ");			
			strSql.Append("  from WordsMessage ");
			strSql.Append(" where id=@id");
						SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			
			WordsMessageModel model=new WordsMessageModel();
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
		public List<WordsMessageModel> getModelList(string strWhere)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select id, messagecontent, email, name, phone, sex, serviceid, createData  ");			
			strSql.Append("  from WordsMessage ");
			strSql.Append(" where 1=1 "+strWhere);
			 SqlParameter[] parameters = { };
		
			
			List<WordsMessageModel> list = new List<WordsMessageModel>();
            DataTable dt = DbHelperSQL.Query(strSql.ToString(), parameters).Tables[0];
            foreach (DataRow row in dt.Rows)
            {
                WordsMessageModel model = new WordsMessageModel();
                GetModel(row, model);
                list.Add(model);
            }
            return list;
            
		}
		
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(WordsMessageModel model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into WordsMessage(");			
            strSql.Append("messagecontent,email,name,phone,sex,serviceid,createData");
			strSql.Append(") values (");
            strSql.Append("@messagecontent,@email,@name,@phone,@sex,@serviceid,@createData");            
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
		public bool Update(WordsMessageModel model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update WordsMessage set ");
			                                                
            strSql.Append(" messagecontent = @messagecontent , ");                                    
            strSql.Append(" email = @email , ");                                    
            strSql.Append(" name = @name , ");                                    
            strSql.Append(" phone = @phone , ");                                    
            strSql.Append(" sex = @sex , ");                                    
            strSql.Append(" serviceid = @serviceid , ");                                    
            strSql.Append(" createData = @createData  ");            			
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
        private SqlParameter[] setParam(WordsMessageModel model)
        {
            SqlParameter[] parameters = {
			            new SqlParameter("@id", SqlDbType.Int,4) ,            
                        new SqlParameter("@messagecontent", SqlDbType.NVarChar,4000) ,            
                        new SqlParameter("@email", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@name", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@phone", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@sex", SqlDbType.Int,4) ,            
                        new SqlParameter("@serviceid", SqlDbType.Int,4) ,            
                        new SqlParameter("@createData", SqlDbType.DateTime)             
              
            };
			       
            parameters[0].Value = model.id;                   
            parameters[1].Value = model.messagecontent;                   
            parameters[2].Value = model.email;                   
            parameters[3].Value = model.name;                   
            parameters[4].Value = model.phone;                   
            parameters[5].Value = model.sex;                   
            parameters[6].Value = model.serviceid;                   
            parameters[7].Value = model.createData;                        return parameters;
        }
		/// <summary>
        /// 数据转换为 WordsMessageModel
        /// </summary>
        /// <param name="row"></param>
        /// <param name="model"></param>
        private WordsMessageModel GetModel(DataRow row, WordsMessageModel model)
        {
            
           									    if(row["id"].ToString()!="")
					{
						model.id=int.Parse(row["id"].ToString());
					}
																																											model.messagecontent= row["messagecontent"].ToString();
																																						model.email= row["email"].ToString();
																																						model.name= row["name"].ToString();
																																						model.phone= row["phone"].ToString();
																																    if(row["sex"].ToString()!="")
					{
						model.sex=int.Parse(row["sex"].ToString());
					}
																																					    if(row["serviceid"].ToString()!="")
					{
						model.serviceid=int.Parse(row["serviceid"].ToString());
					}
																																					    if(row["createData"].ToString()!="")
					{
						model.createData=DateTime.Parse(row["createData"].ToString());
					}
																																		
				return model;
        }

   		
        
		 #endregion 结束通用方法
		 
		
        
		
	}
}

