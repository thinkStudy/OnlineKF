using System;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// Summary description for SQLHelper
/// </summary>
namespace OnlineKF.Utils
{
    public class SQLHelper
    {
        public SQLHelper()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        /// 根据传入的数据库链接字符串获取相应的数据库链接
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public static SqlConnection GetConnection(string connStr)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = connStr;
            con.Open();
            return con;
        }


        //执行Command......................................................................................
        /// <summary>
        /// 封装SqlParameter
        /// </summary>
        /// <param name="ParamName">参数名称</param>
        /// <param name="DbType">参数数据类型</param>
        /// <param name="Size">类型大小</param>
        /// <param name="Direction">参数类型</param>
        /// <param name="Value">值</param>
        /// <returns>SqlParameter</returns>
        public static SqlParameter MakeParam(string ParamName, SqlDbType DbType, Int32 Size, ParameterDirection Direction, object Value)
        {
            SqlParameter param;

            if (Size > 0)
                param = new SqlParameter(ParamName, DbType, Size);
            else
                param = new SqlParameter(ParamName, DbType);

            if (Value == null)
                Value = DBNull.Value;
            else
            {
                if (DbType == SqlDbType.DateTime)
                {
                    if (DateTime.MinValue == (DateTime)Value)
                        Value = DBNull.Value;
                }
            }

            param.Direction = Direction;
            //非输出参数
            if (!(Direction == ParameterDirection.Output))
                param.Value = Value;
            else
            {//输出参数，但是有值，也设置
                if (!Value.Equals(DBNull.Value))
                    param.Value = Value;
            }
            return param;
        }

        /// <summary>
        /// 封装SqlParameter
        /// </summary>
        /// <param name="ParamName">参数名称</param>
        /// <param name="DbType">参数数据类型</param>
        /// <param name="Value">值</param>
        /// <returns>SqlParameter</returns>
        public static SqlParameter MakeParam(string ParamName, SqlDbType DbType, object Value)
        {
            SqlParameter param = new SqlParameter(ParamName, DbType);
            if (Value != null)
            {

                if (DbType == SqlDbType.DateTime)
                {
                    if (DateTime.MinValue == (DateTime)Value)
                        Value = DBNull.Value;
                }
                param.Value = Value;

            }

            else
                param.Value = DBNull.Value;


            return param;
        }

        /// <summary>
        /// PrepareCommand 用于执行
        /// </summary>
        /// <param name="cmd">SqlCommand</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务</param>
        /// <param name="cmdType">类型  存储过程 CommandType.StoredProcedure，SQL语句 CommandType.Text</param>
        /// <param name="cmdText">SQL语句或是存储过程</param>
        /// <param name="cmdParms">参数集合</param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {

            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null && cmdParms.Length > 0)
            {
                foreach (SqlParameter parm in cmdParms)
                {

                    //string a = parm.Value.ToString();
                   // string b = parm.ParameterName.ToString();


                    cmd.Parameters.Add(parm);
                }


            }
        }


        /// <summary>
        /// PrepareCommand 用于执行
        /// </summary>
        /// <param name="cmd">SqlCommand</param>
        /// <param name="cmdType">类型  存储过程 CommandType.StoredProcedure，SQL语句 CommandType.Text</param>
        /// <param name="cmdParms">参数集合</param>
        private static void PrepareCommand(SqlCommand cmd, CommandType cmdType, SqlParameter[] cmdParms)
        {
            cmd.CommandType = cmdType;
            if (cmdParms != null && cmdParms.Length > 0)
            {
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }


        /// <summary>
        /// Execute SqlCommand (兼容事务模式)
        /// 需在方法外将cmd初始化 如 new SqlCommand(cmdText,conn,trans)
        /// </summary>
        /// <param name="cmd">SqlCommand</param>
        /// <param name="cmdType">类型</param>
        /// <param name="cmdParms">参数集合</param>
        /// <returns></returns>
        public static void ExecuteNonQuery(SqlCommand cmd, CommandType cmdType, SqlParameter[] cmdParms)
        {
            PrepareCommand(cmd, cmdType, cmdParms);
            cmd.ExecuteNonQuery();
        }


        /// <summary>
        /// Execute SqlCommand 非事务
        /// 直接传入数据库字符串、SQL语句、参数集合 即可
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="cmdType">类型 存储过程 CommandType.StoredProcedure，SQL语句 CommandType.Text </param>
        /// <param name="cmdText">Command语句</param>
        /// <param name="cmdParms">参数集合 无参数则传入null</param>
        /// <returns></returns>
        public static void ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(cmdText, conn);
                PrepareCommand(cmd, cmdType, cmdParms);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 执行存储过程，直接传入数据库字符串、SQL语句、参数集合 即可
        /// </summary>
        /// <param name="connectionString">数据库字符串</param>
        /// <param name="cmdText">存储过程名</param>
        /// <param name="cmdParms">参数集合</param>
        /// <returns></returns>
        public static void ExecuteProc(string connectionString, string cmdText, SqlParameter[] cmdParms)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(cmdText, conn);
                PrepareCommand(cmd, CommandType.StoredProcedure, cmdParms);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 获取结果集 To Reader
        /// </summary>
        /// <param name="conn">数据库连接</param>
        /// <param name="cmdType">类型 存储过程 CommandType.StoredProcedure，SQL语句 CommandType.Text </param>
        /// <param name="cmdText">Command语句</param>
        /// <param name="cmdParms">参数集合 无参数则传入null</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader ExecuteToReader(SqlConnection conn, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {

            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
            SqlDataReader rdr = cmd.ExecuteReader();

            return rdr;
        }


        /// <summary>
        /// 获取结果集 To DataTable(执行存储过程)
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="cmdText">Command语句</param>
        /// <param name="cmdParms">参数集合 无参数则传入null</param>
        /// <returns></returns>
        public static DataTable ExecuteProcToDataTable(string connectionString, string cmdText, SqlParameter[] cmdParms)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, conn, null, CommandType.StoredProcedure, cmdText, cmdParms);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];
            }
        }

        /// <summary>
        /// 获取结果集 To DataTable
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="cmdType">类型 存储过程 CommandType.StoredProcedure，SQL语句 CommandType.Text </param>
        /// <param name="cmdText">Command语句</param>
        /// <param name="cmdParms">参数集合 无参数则传入null</param>
        /// <returns>DataTable</returns>
        public static DataTable ExecuteToDataTable(string connectionString, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];
            }
        }

        //取字段......................................................................................
        /// <summary>
        /// 是否为空值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsDBOrNull(object obj)
        {
            if (obj == DBNull.Value)
            {
                return true;
            }
            if (obj == null)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取返回值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>空值返回0</returns>
        public static int GetRetInt(object obj)
        {
            if (!SQLHelper.IsDBOrNull(obj))
            {
                return Convert.ToInt32(obj);
            }
            return 0;
        }

        /// <summary>
        /// 获取Short返回值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>空值返回0</returns>
        public static short GetRetShort(object obj)
        {
            if (!SQLHelper.IsDBOrNull(obj))
            {
                return Convert.ToInt16(obj);
            }
            return 0;
        }

        /// <summary>
        /// 获取返回值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>空值返回0</returns>
        public static long GetRetLong(object obj)
        {
            if (!SQLHelper.IsDBOrNull(obj))
            {
                return Convert.ToInt64(obj);
            }
            return 0;
        }

        /// <summary>
        /// 获取返回值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>空值返回0</returns>
        public static double GetRetDouble(object obj)
        {
            if (!SQLHelper.IsDBOrNull(obj))
            {
                return Convert.ToDouble(obj);
            }
            return 0;
        }

        /// <summary>
        /// 获取返回值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>空值返回0</returns>
        public static decimal GetRetDecimal(object obj)
        {
            if (!SQLHelper.IsDBOrNull(obj))
            {
                return Convert.ToDecimal(obj);
            }
            return 0;
        }

        /// <summary>
        /// 获取返回值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>空值返回""</returns>
        public static string GetRetString(object obj)
        {
            if (!SQLHelper.IsDBOrNull(obj))
            {
                return obj.ToString();
            }
            return "";
        }

        /// <summary>
        /// 获取返回值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>空值返回DateTime.MinValue</returns>
        public static DateTime GetRetDateTime(object obj)
        {
            if (!SQLHelper.IsDBOrNull(obj))
            {
                return Convert.ToDateTime(obj);
            }
            //DateTime def = DateTime.Parse("1900-01-01");
            return DateTime.MinValue;
        }

        /// <summary>
        /// 去除sql注入危险字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string checkSqlString(string str)
        {
            str = str.Replace(";", "");
            str = str.Replace("&", "&amp;");
            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");
            str = str.Replace("'", "");
            str = str.Replace("--", "");
            str = str.Replace("/", "");
            str = str.Replace("%", "");
            return str;
        }
    }
}