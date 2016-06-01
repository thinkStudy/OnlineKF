using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace OnlineKF.Utils
{
    /// <summary>
    /// 类型帮助类
    /// </summary>
    public class TypeHelper
    {
        public static string getFinancingType(int type) {
            return type == 1 ? "定向增资" : "非公开转让";
        }
        public static DataTable getFinancingData()
        {
            DataTable result = new DataTable();
            DataRow dr;
            result.Columns.Add(new DataColumn("value"));
            result.Columns.Add(new DataColumn("name"));
            dr = result.NewRow();
            dr[0] = 1;
            dr[1] = "定向增资";
            result.Rows.Add(dr);
            dr = result.NewRow();
            dr[0] = 2;
            dr[1] = "非公开转让";
            result.Rows.Add(dr);
            return result;
        }
    }
}
