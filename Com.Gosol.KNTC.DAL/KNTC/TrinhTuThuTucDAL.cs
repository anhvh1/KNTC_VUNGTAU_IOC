using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Ultilities;
//using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
//using DocumentFormat.OpenXml.EMMA;
//using DocumentFormat.OpenXml.Wordprocessing;
using Com.Gosol.KNTC.DAL.DanhMuc;
using Com.Gosol.KNTC.Models.DanhMuc;
using Com.Gosol.KNTC.Models.HeThong;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Security;

namespace Com.Gosol.KNTC.DAL.KNTC
{
    public class TrinhTuThuTucDAL
    {
        public int Insert(TrinhTuThuTucModel Info)
        {
            object val = null;
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("TieuDe", SqlDbType.NVarChar),
                new SqlParameter("NoiDung", SqlDbType.NVarChar),
                new SqlParameter("NgayTao", SqlDbType.DateTime),
                new SqlParameter("NguoiTaoID", SqlDbType.Int),
                new SqlParameter("Public", SqlDbType.Bit),    
            };
            parms[0].Value = Info.TieuDe ?? Convert.DBNull;
            parms[1].Value = Info.NoiDung ?? Convert.DBNull;
            parms[2].Value = Info.NgayTao ?? DateTime.Now;
            parms[3].Value = Info.NguoiTaoID ?? Convert.DBNull;
            parms[4].Value = Info.Public ?? Convert.DBNull;
         
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, "TrinhTuThuTuc_Insert", parms);
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
                conn.Close();
            }
            return Convert.ToInt32(val);
        }

        public int Delete(int TrinhTuThuTucID)
        {
            object val;
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("TrinhTuThuTucID", SqlDbType.Int),
            };
            parameters[0].Value = TrinhTuThuTucID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, "TrinhTuThuTuc_Delete", parameters);
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
                conn.Close();
            }
            return Utils.ConvertToInt32(val, 0);
        }

        public int Update(TrinhTuThuTucModel Info)
        {
            object val = 0;
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("TrinhTuThuTucID", SqlDbType.Int),
                new SqlParameter("TieuDe", SqlDbType.NVarChar),
                new SqlParameter("NoiDung", SqlDbType.NVarChar),
                new SqlParameter("NgayTao", SqlDbType.DateTime),
                new SqlParameter("NguoiTaoID", SqlDbType.Int),
                new SqlParameter("Public", SqlDbType.Bit),
            };

            parms[0].Value = Info.TrinhTuThuTucID;
            parms[1].Value = Info.TieuDe ?? Convert.DBNull;
            parms[2].Value = Info.NoiDung ?? Convert.DBNull;
            parms[3].Value = Info.NgayTao ?? DateTime.Now;
            parms[4].Value = Info.NguoiTaoID ?? Convert.DBNull;
            parms[5].Value = Info.Public ?? Convert.DBNull;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, "TrinhTuThuTuc_Update", parms);
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
                conn.Close();
            }
            return Convert.ToInt32(val);
        }

        public int UpdateTrangThaiPublic(TrinhTuThuTucModel Info)
        {
            object val = 0;
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("TrinhTuThuTucID", SqlDbType.Int),      
                new SqlParameter("Public", SqlDbType.Bit),
            };

            parms[0].Value = Info.TrinhTuThuTucID;
            parms[1].Value = Info.Public ?? Convert.DBNull;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, "TrinhTuThuTuc_UpdateTrangThaiPublish", parms);
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
                conn.Close();
            }
            return Convert.ToInt32(val);
        }

        public TrinhTuThuTucModel GetByID(int TrinhTuThuTucID)
        {
            TrinhTuThuTucModel Info = null;
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("TrinhTuThuTucID", SqlDbType.Int),
            };
            parameters[0].Value = TrinhTuThuTucID;

            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "TrinhTuThuTuc_GetByID", parameters))
            {
                if (dr.Read())
                {
                    Info = new TrinhTuThuTucModel();
                    Info.TrinhTuThuTucID = Utils.ConvertToInt32(dr["TrinhTuThuTucID"], 0);
                    Info.TieuDe = Utils.ConvertToString(dr["TieuDe"], string.Empty);
                    Info.NoiDung = Utils.ConvertToString(dr["NoiDung"], string.Empty);
                    Info.NgayTao = Utils.ConvertToNullableDateTime(dr["NgayTao"], null);
                    Info.NguoiTaoID = Utils.ConvertToInt32(dr["NguoiTaoID"], 0);
                    Info.Public = Utils.ConvertToBoolean(dr["Public"], false);
                }
                dr.Close();
            }
            return Info;
        }

        public List<TrinhTuThuTucModel> GetPagingBySearch(BasePagingParams p, ref int TotalRow)
        {
           
            List<TrinhTuThuTucModel> list = new List<TrinhTuThuTucModel>();
            SqlParameter[] parameters = new SqlParameter[]
              {
                new SqlParameter("@Keyword",SqlDbType.NVarChar),
                new SqlParameter("@pLimit",SqlDbType.Int),
                new SqlParameter("@pOffset",SqlDbType.Int),
                new SqlParameter("@TotalRow",SqlDbType.Int),
                new SqlParameter("@TrangThaiID",SqlDbType.Int),
              };
            parameters[0].Value = p.Keyword == null ? "" : p.Keyword.Trim();
            parameters[1].Value = p.Limit;
            parameters[2].Value = p.Offset;
            parameters[3].Direction = ParameterDirection.Output;
            parameters[3].Size = 8; 
            parameters[4].Value = p.TrangThai ?? Convert.DBNull;
          
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "TrinhTuThuTuc_GetPagingBySearch", parameters))
                {
                    while (dr.Read())
                    {
                        TrinhTuThuTucModel Info = new TrinhTuThuTucModel();
                        Info.TrinhTuThuTucID = Utils.ConvertToInt32(dr["TrinhTuThuTucID"], 0);
                        Info.TieuDe = Utils.ConvertToString(dr["TieuDe"], string.Empty);
                        Info.NoiDung = Utils.ConvertToString(dr["NoiDung"], string.Empty);
                        Info.NgayTao = Utils.ConvertToNullableDateTime(dr["NgayTao"], null);
                        Info.NguoiTaoID = Utils.ConvertToInt32(dr["NguoiTaoID"], 0);
                        Info.TenNguoiTao = Utils.ConvertToString(dr["TenNguoiTao"], string.Empty);
                        Info.Public = Utils.ConvertToBoolean(dr["Public"], false);
                        list.Add(Info);
                    }

                    dr.Close();
                }
                TotalRow = Utils.ConvertToInt32(parameters[3].Value, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            return list;
        }


    }
}
