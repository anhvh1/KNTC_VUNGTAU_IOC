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
    public class QL_CQSatNhap
    {
        private QL_CQSatNhapInfo GetData(SqlDataReader rdr)
        {
            QL_CQSatNhapInfo info = new QL_CQSatNhapInfo();

            info.CoQuanMoiID = Utils.GetInt32(rdr["CoQuanMoiID"], 0);
            info.TenCoQuanMoi = Utils.GetString(rdr["TenCoQuanMoi"], String.Empty);
            info.ChiaTachSapNhap = Utils.GetString(rdr["ChiaTachSapNhap"], String.Empty);
            //info.CoQuanCuID = Utils.GetInt32(rdr["CoQuanCuID"], 0);
            info.TenCoQuanCu = Utils.GetString(rdr["TenCoQuanCu"], String.Empty);

            return info;
        }

        public List<QL_CQSatNhapInfo> GetBySear(int start, int end, string key)
        {
            List<QL_CQSatNhapInfo> result = new List<QL_CQSatNhapInfo>();
            SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("pstart", SqlDbType.Int),
                    new SqlParameter("pend", SqlDbType.Int),
                    new SqlParameter("pkey", SqlDbType.NVarChar)
            };
            parameters[0].Value = start;
            parameters[1].Value = end;
            parameters[2].Value = key;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "QL_CQSatNhap_GetBySearch", parameters))
                {
                    while (dr.Read())
                    {
                        QL_CQSatNhapInfo info = GetData(dr);
                        info.CountTotal = Utils.GetInt32(dr["CountTotal"], 0);
                        result.Add(info);
                    }
                    dr.Close();

                }
            }
            catch { throw; }

            return result;
        }

        public int CountSear(string key)
        {
            QL_CQSatNhapInfo info = new QL_CQSatNhapInfo();
            SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("pkey", SqlDbType.NVarChar)
            };
            parameters[0].Value = key;

            var i = 0;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "QL_CQSatNhap_CountSearch", parameters))
                {
                    while (dr.Read())
                    {
                        info.CountTotal = Utils.ConvertToInt32(dr["CountTotal"], 0);
                        //result.Add(info);
                    }
                    dr.Close();

                }
            }
            catch { throw; }
            i = info.CountTotal;
            return i;
        }

        public QL_CQSatNhapInfo GetByID(int ID_Moi, int ID_Cu)
        {
            QL_CQSatNhapInfo info = new QL_CQSatNhapInfo();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("pCoQuanMoiID", SqlDbType.Int),
                new SqlParameter("pCoQuanCuID", SqlDbType.Int),
            };
            parameters[0].Value = ID_Moi;
            parameters[1].Value = ID_Cu;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "QL_CQSatNhap_GetByID", parameters))
                {
                    while (dr.Read())
                    {
                        info = GetData(dr);
                        info.CountTotal = Utils.GetInt32(dr["CountTotal"], 0);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return info;
        }

        public int Delete(int ID_Moi, int ID_Cu)
        {
            int val = 0;
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("pCoQuanMoiID", SqlDbType.Int),
                new SqlParameter("pCoQuanCuID", SqlDbType.Int),
            };
            parameters[0].Value = ID_Moi;
            parameters[1].Value = ID_Cu;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "QL_CQSatNhap_Delete", parameters);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
                conn.Close();
            }
            return val;
        }

        public int Insert_SatNhap(int IdCqCu, int IdCqMoi, int trangThai, DateTime? NgayThucHien, int? NguoiThucHienID)
        {
            int val = 0;
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("pCoQuanCuID", SqlDbType.Int),
                new SqlParameter("pCoQuanMoiID", SqlDbType.Int),
                new SqlParameter("pTrangThai", SqlDbType.Int),
                new SqlParameter("pNgayThucHien", SqlDbType.DateTime),
                new SqlParameter("pNguoiThucHienID", SqlDbType.Int),
            };
            parameters[0].Value = IdCqCu;
            parameters[1].Value = IdCqMoi;
            parameters[2].Value = trangThai;
            parameters[3].Value = NgayThucHien ?? Convert.DBNull;
            parameters[4].Value = NguoiThucHienID ?? Convert.DBNull;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "QL_CQSatNhap_Update_CQ_New", parameters);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
                conn.Close();
            }
            return val;
        }

        public int Insert_CB_US(int CanBoID, int IdCqMoi, int IdCqCu)
        {
            int val = 0;
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("pCanBoID", SqlDbType.Int),
                new SqlParameter("pCoQuanMoiID", SqlDbType.Int),
                new SqlParameter("pCoQuanCuID", SqlDbType.Int)
            };
            parameters[0].Value = CanBoID;
            parameters[1].Value = IdCqMoi;
            parameters[2].Value = IdCqCu;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "QL_CQSatNhap_InsertCB_US", parameters);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
                conn.Close();
            }
            return val;
        }

        public IList<QL_CQSatNhapInfo> GetByCoQuanMoiID(int CqID)
        {
            IList<QL_CQSatNhapInfo> result = new List<QL_CQSatNhapInfo>();
            SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("pCoQuanMoiID", SqlDbType.Int)
            };
            parameters[0].Value = CqID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "QL_CQSatNhap_GetByCqMoiID", parameters))
                {
                    while (dr.Read())
                    {
                        QL_CQSatNhapInfo info = new QL_CQSatNhapInfo();
                        info.CoQuanMoiID = Utils.GetInt32(dr["CoQuanMoiID"], 0);
                        info.ChiaTachSapNhap = Utils.GetString(dr["ChiaTachSapNhap"], String.Empty);
                        info.CoQuanCuID = Utils.GetInt32(dr["CoQuanCuID"], 0);
                        info.TrangThai = Utils.GetInt32(dr["TrangThai"], 0);
                        info.TenCoQuanCu = Utils.GetString(dr["TenCoQuanCu"], String.Empty);
                        result.Add(info);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex) { throw; }

            return result;
        }

        public int delete_SatNhap(int IdCqMoi)
        {
            int val = 0;
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("pCoQuanMoiID", SqlDbType.Int),
            };
            parameters[0].Value = IdCqMoi;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "QL_CQSatNhap_Delete_SatNhap", parameters);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
                conn.Close();
            }
            return val;
        }

        public int delete_ChiaTach(int IdCqMoi)
        {
            int val = 0;
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("pCoQuanID", SqlDbType.Int),
            };
            parameters[0].Value = IdCqMoi;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "QL_CQSatNhap_Delete_ChiaTach", parameters);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
                conn.Close();
            }
            return val;
        }

        public int Insert_ChiaTach(int IdCqCu, int IdCqMoi, int trangThai, DateTime? NgayThucHien, int? NguoiThucHienID)
        {
            int val = 0;
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("pCoQuanCuID", SqlDbType.Int),
                new SqlParameter("pCoQuanMoiID", SqlDbType.Int),
                new SqlParameter("pTrangThai", SqlDbType.Int),
                new SqlParameter("pNgayThucHien", SqlDbType.DateTime),
                new SqlParameter("pNguoiThucHienID", SqlDbType.Int),
            };
            parameters[0].Value = IdCqCu;
            parameters[1].Value = IdCqMoi;
            parameters[2].Value = trangThai;
            parameters[3].Value = NgayThucHien ?? Convert.DBNull;
            parameters[4].Value = NguoiThucHienID ?? Convert.DBNull;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "QL_CQSatNhap_Update_CQ_CT_New", parameters);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
                conn.Close();
            }
            return val;
        }
    }
}
