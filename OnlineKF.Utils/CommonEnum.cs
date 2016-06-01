using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineKF.Utils
{
    public static class CommonEnum
    {
        /// <summary>
        /// 0：保存未发布 1：保存已发布（待审核） 2：审核通过
        /// </summary>
        public enum StatusEnum
        {
            /// <summary> 
            /// 0：保存未发布
            /// </summary>
            SaveNoRelease = 0,

            /// <summary>
            /// 1：保存已发布（待审核）
            /// </summary>
            SaveAndRelease,

            /// <summary>
            ///  2：审核通过
            /// </summary>
            Approved,

            /// <summary>
            /// 审核未通过
            /// </summary>
            Refuse
        }
    }
}
