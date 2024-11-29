using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Ultilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.DAL.KNTC
{
    public class DongBo_Log_Dal
    {
        #region Database query string

        private const string INSERT = @"DongBo_Log_Insert";
        private const string DONGBO_INSERT = @"DongBo_Insert";
        private const string DONGBO_UPDATE = "DongBo_Update";
        private const string DONGBO_GETALL = "DongBo_GetALL";
        private const string DONGBO_LOG_UPDATE_TRANGTHAIDON = "DongBo_Log_UpdateTrangThaiDon";
        private const string ThaoLuanGiaiQuyet_Count_XuLyDonID = "ThaoLuanGiaiQuyet_Count_XuLyDonID";
        private const string KetQua_Count_XuLyDonID = @"KetQua_Count_XuLyDonID";
        private const string ThiHanh_Count_XuLyDonID = "ThiHanh_Count_XuLyDonID";
        private const string XuLyDon_Count_XuLyDonID = "XuLyDon_Count_XuLyDonID";
        private const string GETTRANGTHAIDONHIENTAI = "DongBo_Log_GetTrangThaiDon";
        #endregion

        #region paramaters constant

        private const string PARM_XULYDONID = "@XuLyDonID";
        private const string PARM_CANBOID = "@CanBoID";
        private const string PARM_TENBUOCDONGBO = "@TenBuocDongBo";
        private const string PARM_TRANGTHAI = "@TrangThai";
        private const string PARM_CREATEDATE = "@CreateDate";
        private const string PARM_TRANGTHAIDONTHU = "@TrangThaiDonThu";
        private const string PARM_DONGBOID = "@DongBoID";

        //dong bo
        private const string PARM_LOAIDONGBO = "@LoaiDongBo";
        private const string PARM_NGAYTRONGTUAN = "@NgayTrongTuan";
        private const string PARM_GIODONGBO = "@GioDongBo";
        private const string PARM_URL = "@Url";
        private const string PARM_PASSWORD = "@Password";
        #endregion

        private SqlParameter[] GetInsertParms()
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_XULYDONID, SqlDbType.Int),
                new SqlParameter(PARM_CANBOID, SqlDbType.Int),
                new SqlParameter(PARM_TRANGTHAI, SqlDbType.Int),
                new SqlParameter(PARM_CREATEDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_TRANGTHAIDONTHU, SqlDbType.Int),
                  new SqlParameter(PARM_DONGBOID, SqlDbType.Int),
            };
            return parms;
        }

        private void SetInsertParms(SqlParameter[] parms, DongBo_LogInfo info)
        {
            parms[0].Value = info.XyLyDonID;
            parms[1].Value = info.CanBoID;
            parms[2].Value = info.TrangThai;
            parms[3].Value = info.CreateDate;
            parms[4].Value = info.TrangThaiDonThu;
            parms[5].Value = info.DongBoID;
        }

        public int DongBo_Update(DongBo_LogInfo info)
        {
            object val = null;
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_NGAYTRONGTUAN, SqlDbType.NVarChar),
                new SqlParameter(PARM_GIODONGBO, SqlDbType.NVarChar),
                new SqlParameter(PARM_URL, SqlDbType.NVarChar),
                new SqlParameter(PARM_PASSWORD, SqlDbType.NVarChar),
            };
            parms[0].Value = info.NgayTrongTuan ?? Convert.DBNull;
            parms[1].Value = info.GioDongBo ?? Convert.DBNull;
            parms[2].Value = info.URl;
            parms[3].Value = info.Password;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, DONGBO_UPDATE, parms);
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

        public int DongBo_Log_UpdateTrangThaiDon(DongBo_LogInfo info)
        {
            object val = null;
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_XULYDONID, SqlDbType.Int),
                new SqlParameter(PARM_TRANGTHAIDONTHU, SqlDbType.Int),
                 new SqlParameter(PARM_TRANGTHAI, SqlDbType.Int),
            };
            parms[0].Value = info.XyLyDonID;
            parms[1].Value = info.TrangThaiDonThu;
            parms[2].Value = info.TrangThai;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, DONGBO_LOG_UPDATE_TRANGTHAIDON, parms);
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

        public int DongBo_Log_UpdateForMapping(string TypeApi, int P_ID, string P_MappingCode)
        {
            try
            {
                string strStore = string.Empty;
                switch (TypeApi)
                {
                    case "CQ":
                    case "BN":
                        strStore = "SyncApi_CoQuan_UPdateForMapping";
                        break;
                    case "KT":
                        strStore = "SyncApi_LoaiKhieuTo_UPdateForMapping";
                        break;
                    //case "QD":
                    //	strStore = "8";
                    //	break;
                    //case "TQ":
                    //	strStore = "9";
                    //	break;
                    case "KQ":
                        strStore = "SyncApi_LoaiKetQua_UPdateForMapping";
                        break;
                    case "ND":
                        strStore = "SyncApi_NguonDonDen_UPdateForMapping";
                        break;
                    case "DMT":
                        strStore = "SyncApi_Tinh_UPdateForMapping";
                        break;
                    case "DMH":
                        strStore = "SyncApi_Huyen_UPdateForMapping";
                        break;
                    case "DMX":
                        strStore = "SyncApi_Xa_UPdateForMapping";
                        break;
                    case "DT":
                        strStore = "SyncApi_DanToc_UPdateForMapping";
                        break;
                    case "QG":
                        strStore = "SyncApi_QuocTich_UPdateForMapping";
                        break;
                    case "GQ":
                        strStore = "SyncApi_HuongGiaiQuyet_UPdateForMapping";
                        break;
                }

                object val = null;
                SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@P_ID", SqlDbType.Int), new SqlParameter("@P_MappingCode", SqlDbType.NVarChar) };
                parms[0].Value = P_ID;
                parms[1].Value = P_MappingCode;
                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, strStore, parms);
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
            catch (Exception)
            {
                return -1;
            }
        }

        public DongBo_LogInfo GetLichDongBo()
        {

            DongBo_LogInfo lichDongBo = new DongBo_LogInfo();

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, DONGBO_GETALL, null))
                {
                    if (dr.Read())
                    {
                        lichDongBo.GioDongBo = Utils.ConvertToString(dr["GioDongBo"], string.Empty);
                        lichDongBo.NgayTrongTuan = Utils.ConvertToString(dr["NgayTrongTuan"], string.Empty);
                        lichDongBo.URl = Utils.ConvertToString(dr["URl"], string.Empty);
                        lichDongBo.Password = Utils.ConvertToString(dr["Password"], string.Empty);
                    }
                }
            }
            catch
            {

                throw;
            }
            return lichDongBo;
        }

        public int Insert(DongBo_LogInfo systemLogInfo)
        {
            object val = null;
            SqlParameter[] parameters = GetInsertParms();
            SetInsertParms(parameters, systemLogInfo);
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, INSERT, parameters);
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

        public int Count_ThaoLuanGiaiQuyet_By_XuLyDonID(int xuLyDonID)
        {

            int count = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARM_XULYDONID,SqlDbType.Int),
            };

            parameters[0].Value = xuLyDonID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, ThaoLuanGiaiQuyet_Count_XuLyDonID, parameters))
                {
                    if (dr.Read())
                    {
                        count = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                }
            }
            catch
            {
                throw;
            }
            return count;
        }

        public int Count_KetQua_By_XuLyDonID(int xuLyDonID)
        {

            int count = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARM_XULYDONID,SqlDbType.Int),
            };

            parameters[0].Value = xuLyDonID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, KetQua_Count_XuLyDonID, parameters))
                {
                    if (dr.Read())
                    {
                        count = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                }
            }
            catch
            {
                throw;
            }
            return count;
        }

        public int Count_ThiHanh_By_XuLyDonID(int xuLyDonID)
        {

            int count = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARM_XULYDONID,SqlDbType.Int),
            };

            parameters[0].Value = xuLyDonID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, ThiHanh_Count_XuLyDonID, parameters))
                {
                    if (dr.Read())
                    {
                        count = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                }
            }
            catch
            {
                throw;
            }
            return count;
        }
        public int Count_XuLyDon_By_XuLyDonID(int xuLyDonID)
        {

            int count = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARM_XULYDONID,SqlDbType.Int),
            };

            parameters[0].Value = xuLyDonID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, XuLyDon_Count_XuLyDonID, parameters))
                {
                    if (dr.Read())
                    {
                        count = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                }
            }
            catch
            {
                throw;
            }
            return count;
        }

        public DongBo_LogInfo getTrangThaiDonThu_HienTai(int xuLyDonID)
        {

            DongBo_LogInfo info = new DongBo_LogInfo();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARM_XULYDONID,SqlDbType.Int),
            };

            parameters[0].Value = xuLyDonID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GETTRANGTHAIDONHIENTAI, parameters))
                {
                    if (dr.Read())
                    {
                        info.TrangThaiDonThu = Utils.ConvertToInt32(dr["TrangThaiDonThu"], 0);
                        info.XyLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
                    }
                }
            }
            catch
            {
                throw;
            }
            return info;
        }
    }
}
