using Com.Gosol.KNTC.Ultilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Gosol.KNTC.Models.KNTC;

namespace Com.Gosol.KNTC.DAL.KNTC
{
    public class ThongKeNhapLieu
    {
        private const string GET_BY_TIME = "ThongKeNhapLieu_GetByTime";

        private const string PARM_STARTDATE = "@StartDate";
        private const string PARM_ENDDATE = "@EndDate";
        private const string PARM_COQUANID = "@CoQuanID";

        private ThongKeNhapLieuInfo GetData(SqlDataReader rdr)
        {
            ThongKeNhapLieuInfo info = new ThongKeNhapLieuInfo();
            info.TenDonVi = Utils.ConvertToString(rdr["TenCoQuan"], string.Empty);
            info.CoQuanID = Utils.ConvertToInt32(rdr["CoQuanID"], 0);
            info.CoQuanChaID = Utils.ConvertToInt32(rdr["CoQuanChaID"], 0);
            info.SLTiepDan = Utils.ConvertToInt32(rdr["SLTiepDan"], 0);
            info.SLDonThu = Utils.ConvertToInt32(rdr["SLDonThu"], 0);
            info.SLXuLyDon = Utils.ConvertToInt32(rdr["SLXuLyDon"], 0);
            info.SLGiaiQuyetDon = Utils.ConvertToInt32(rdr["SLGiaiQuyetDon"], 0);
            info.HuyenID = Utils.ConvertToInt32(rdr["HuyenID"],0);
            info.CapID = Utils.ConvertToInt32(rdr["CapID"],0);
            return info;
        }

        public IList<ThongKeNhapLieuInfo> GetByTime(DateTime? startDate, DateTime? endDate)
        {
            IList<ThongKeNhapLieuInfo> infoList = new List<ThongKeNhapLieuInfo>();
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_STARTDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_ENDDATE, SqlDbType.DateTime)
            };
            parm[0].Value = startDate ?? Convert.DBNull;
            parm[1].Value = endDate ?? Convert.DBNull;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_TIME, parm))
                {
                    while (dr.Read())
                    {
                        ThongKeNhapLieuInfo info = GetData(dr);
                        infoList.Add(info);
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return infoList;
        }
    }
}
