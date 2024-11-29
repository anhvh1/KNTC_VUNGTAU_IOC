using Com.Gosol.KNTC.Model.HeThong;
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
    public class VaiTroGiaiQuyet
    {
        private readonly string INSERT = @"VaiTroGiaiQuyet_Insert";
        private readonly string INSERT_UPDATE = @"VaiTroGiaiQuyet_Insert_Update";
        private readonly string DELETE_BY_XULYDONID = @"VaiTroGiaiQuyet_DeleteByXuLyDonID";
        private readonly string GET_BY_XULYDONID = @"VaiTroGiaiQuyet_GetVaiTroByXuLyDonID";
        private readonly string GET_CANBOPHUTRACH = @"VaiTroGiaiQuyet_GetVaiTro";
        private readonly string UPDATE_HOATDONG_BY_XULYDONID = @"VaiTroGiaiQuyet_Update_HoatDong";

        private const string PARM_HOATDONG = @"HoatDong";
        private const string PARM_XULYDONID = @"XuLyDonID";
        private const string PARM_CANBOID = @"CanBoID";
        private const string PARM_VAITRO = @"VaiTro";
        private const string PARM_CHUYENGIAIQUYETID = @"ChuyenGiaiQuyetID";
        private const string GET_CANBOXACMINH = @"DM_CanBo_GetBy_XuLyDonID";

        private SqlParameter[] GetInsertParms()
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_XULYDONID, SqlDbType.Int),
                new SqlParameter(PARM_CANBOID, SqlDbType.Int),
                new SqlParameter(PARM_VAITRO, SqlDbType.TinyInt),
                new SqlParameter(PARM_CHUYENGIAIQUYETID,SqlDbType.Int),

            };
            return parms;
        }

        private void SetInsertParms(SqlParameter[] parms, VaiTroGiaiQuyetInfo info)
        {
            parms[0].Value = info.XuLyDonID;
            parms[1].Value = info.CanBoID;
            parms[2].Value = Convert.ToByte(info.VaiTro);
            parms[3].Value = info.ChuyenGiaiQuyetID;
        }

        public int Insert(VaiTroGiaiQuyetInfo info)
        {
            object val = null;
            SqlParameter[] parameters = GetInsertParms();
            SetInsertParms(parameters, info);
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

        public int InsertUpdate(VaiTroGiaiQuyetInfo info)
        {
            object val = null;
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_XULYDONID, SqlDbType.Int),
                new SqlParameter(PARM_CANBOID, SqlDbType.Int),
                new SqlParameter(PARM_VAITRO, SqlDbType.TinyInt),
                new SqlParameter(PARM_CHUYENGIAIQUYETID,SqlDbType.Int),
                new SqlParameter(PARM_HOATDONG,SqlDbType.Int),

            };
            parms[0].Value = info.XuLyDonID;
            parms[1].Value = info.CanBoID;
            parms[2].Value = Convert.ToByte(info.VaiTro);
            parms[3].Value = info.ChuyenGiaiQuyetID;
            parms[4].Value = info.HoatDong;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, INSERT_UPDATE, parms);
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

        public CanBoInfo GetCanBoPhuTrach(int XuLyDonID, int VaiTro)
        {
            CanBoInfo info = new CanBoInfo();
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_XULYDONID, SqlDbType.Int),
                new SqlParameter(PARM_VAITRO, SqlDbType.TinyInt),
            };
            parm[0].Value = XuLyDonID;
            parm[1].Value = Convert.ToByte(VaiTro);

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_CANBOPHUTRACH, parm))
                {
                    if (dr.Read())
                    {
                        info.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        info.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                    }
                    dr.Close();

                }
            }
            catch (Exception e)
            {
                throw;
            }
            return info;
        }

        private CanBoInfo GetData(SqlDataReader dr)
        {
            CanBoInfo cBInfo = new CanBoInfo();

            cBInfo.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
            cBInfo.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
            cBInfo.VaiTroXacMinh = Utils.ConvertToInt32(dr["VaiTro"], 0);

            return cBInfo;
        }

        public List<VaiTroGiaiQuyetInfo> GetByXuLyDonID(int xldID)
        {
            List<VaiTroGiaiQuyetInfo> resultList = new List<VaiTroGiaiQuyetInfo>();
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_XULYDONID, SqlDbType.Int),
            };
            parm[0].Value = xldID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_XULYDONID, parm))
                {
                    while (dr.Read())
                    {
                        VaiTroGiaiQuyetInfo info = new VaiTroGiaiQuyetInfo();
                        info.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        info.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        info.HoatDong = Utils.ConvertToInt32(dr["HoatDong"], 0);
                        info.VaiTro = Utils.ConvertToInt32(dr["VaiTro"], 0);

                        resultList.Add(info);
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return resultList;
        }

        public List<CanBoInfo> GetCanBoXacMinh(int XuLyDonID)
        {
            List<CanBoInfo> info = new List<CanBoInfo>();
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_XULYDONID, SqlDbType.Int),
            };
            parm[0].Value = XuLyDonID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_CANBOXACMINH, parm))
                {
                    while (dr.Read())
                    {
                        CanBoInfo cBInfo = GetData(dr);
                        cBInfo.TenCoQuan = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        info.Add(cBInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return info;
        }

        public int DeleteByXuLyDonID(int XuLyDonID)
        {
            int val = 0;
            SqlParameter[] parameters = new SqlParameter[] {
new SqlParameter(PARM_XULYDONID, SqlDbType.Int) };
            parameters[0].Value = XuLyDonID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, DELETE_BY_XULYDONID, parameters);
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
            return val;
        }


        public int UpdateHoatDongByXuLyDonID(int XuLyDonID)
        {
            int val = 0;
            SqlParameter[] parameters = new SqlParameter[] {
            new SqlParameter(PARM_XULYDONID, SqlDbType.Int) };
            parameters[0].Value = XuLyDonID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, UPDATE_HOATDONG_BY_XULYDONID, parameters);
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
            return val;
        }
    }
}
