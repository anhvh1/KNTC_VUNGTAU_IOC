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
    public class DoiTuongBiKN
    {
        //Su dung de goi StoreProcedure

        private const string GET_ALL = @"DoiTuongBiKN_GetAll";
        private const string GET_BY_ID = @"DoiTuongBiKN_GetByID";
        private const string GET_BY_DONTHUID = @"DoiTuongBiKN_GetBy_DonThuID";
        //private const string GET_BY_NHOMKN_ID = @"DoiTuongBiKN_GetByNhomKNID";
        private const string INSERT = @"DoiTuongBiKN_Insert";
        private const string UPDATE = @"DoiTuongBiKN_Update";
        private const string DELETE = @"DoiTuongBiKN_Delete";

        //private const string GET_BY_PAGE = @"DM_DoiTuongBiKN_GetByPage";
        //private const string COUNT_ALL = @"DM_DoiTuongBiKN_CountAll";
        //private const string COUNT_SEARCH = @"DM_DoiTuongBiKN_CountSearch";
        //private const string SEARCH = @"DM_DoiTuongBiKN_GetBySearch";

        //quanghv: stored
        //private const string GET_SO_DON_BY_CO_QUAN = @"NV_DoiTuongBiKN_GetSoDonByCoQuan";

        //Ten cac bien dau vao
        private const string PARAM_DOITUONGBIKN_ID = "@DoiTuongBiKNID";
        private const string PARAM_TEN_DOI_TUONG_BI_KN = "@TenDoiTuongBiKN";
        private const string PARAM_TINH_ID = "@TinhID";
        private const string PARAM_HUYEN_ID = "@HuyenID";
        private const string PARAM_XA_ID = "@XaID";
        private const string PARAM_DIACHICHITIET = "@DiaChiCT";
        private const string PARAM_LOAI_DOI_TUONG_BI_KN = "@LoaiDoiTuongBiKNID";
        private const string PARAM_DONTHUID = "@DonThuID";

        private DoiTuongBiKNInfo GetData(SqlDataReader dr)
        {
            DoiTuongBiKNInfo info = new DoiTuongBiKNInfo();
            info.DoiTuongBiKNID = Utils.ConvertToInt32(dr["DoiTuongBiKNID"].ToString(), 0);
            info.TenDoiTuongBiKN = Utils.GetString(dr["TenDoiTuongBiKN"].ToString(), string.Empty);
            info.TinhID = Utils.ConvertToNullableInt32(dr["TinhID"].ToString(), null);
            info.HuyenID = Utils.ConvertToNullableInt32(dr["HuyenID"].ToString(), null);
            info.XaID = Utils.ConvertToNullableInt32(dr["XaID"].ToString(), null);
            info.DiaChiCT = Utils.GetString(dr["DiaChiCT"], string.Empty);
            info.SoNhaDoiTuongBiKN = info.DiaChiCT;
            info.LoaiDoiTuongBiKNID = Utils.ConvertToInt32(dr["LoaiDoiTuongBiKNID"].ToString(), 0);
            if (info.LoaiDoiTuongBiKNID == (int)DMLoaiDoiTuongBiKN.CaNhan)
            {
                info.StringLoaiDoiTuong = Constant.CaNhan;
            }
            if (info.LoaiDoiTuongBiKNID == (int)DMLoaiDoiTuongBiKN.CoQuanToChuc)
            {
                info.StringLoaiDoiTuong = Constant.CoQuan;
            }
            info.TenTinh = Utils.ConvertToString(dr["TenTinh"].ToString(), string.Empty);
            info.TenHuyen = Utils.ConvertToString(dr["TenHuyen"].ToString(), string.Empty);
            info.TenXa = Utils.ConvertToString(dr["TenXa"].ToString(), string.Empty);
            info.TenDanToc = Utils.ConvertToString(dr["TenDanToc"].ToString(), string.Empty);
            info.TenQuocTich = Utils.ConvertToString(dr["TenQuocTich"].ToString(), string.Empty);
            info.TenChucVu = Utils.ConvertToString(dr["TenChucVu"].ToString(), string.Empty);
            info.GioiTinhDoiTuongBiKN = Utils.ConvertToInt32(dr["GioiTinh"].ToString(), 0);
            info.TenChucVu = Utils.ConvertToString(dr["ChucVu"].ToString(), string.Empty);
            return info;
        }

        //Get Insert Parmas
        private SqlParameter[] GetInsertParms()
        {
            SqlParameter[] parms = new SqlParameter[]{   
                //new SqlParameter(PARAM_DOITUONGBIKN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TEN_DOI_TUONG_BI_KN,SqlDbType.NVarChar),
                new SqlParameter(PARAM_TINH_ID, SqlDbType.Int),
                new SqlParameter(PARAM_HUYEN_ID, SqlDbType.Int),
                new SqlParameter(PARAM_XA_ID, SqlDbType.Int),
                new SqlParameter(PARAM_DIACHICHITIET, SqlDbType.NVarChar),
                new SqlParameter(PARAM_LOAI_DOI_TUONG_BI_KN, SqlDbType.Int),
                new SqlParameter("@DonThuID", SqlDbType.Int),
                new SqlParameter("@GioiTinh", SqlDbType.Int),
                new SqlParameter("@ChucVu", SqlDbType.NVarChar),
                };
            return parms;
        }

        //SET iNSERT PARMS
        private void SetInsertParms(SqlParameter[] parms, DoiTuongBiKNInfo info)
        {

            //parms[0].Value = info.DoiTuongBiKNID;
            parms[0].Value = info.TenDoiTuongBiKN ?? Convert.DBNull;
            parms[1].Value = info.TinhID ?? Convert.DBNull;
            parms[2].Value = info.HuyenID ?? Convert.DBNull;
            parms[3].Value = info.XaID ?? Convert.DBNull;
            parms[4].Value = info.DiaChiCT ?? (info.SoNhaDoiTuongBiKN ?? Convert.DBNull);
            parms[5].Value = info.LoaiDoiTuongBiKNID ?? Convert.DBNull;
            parms[6].Value = info.DonThuID ?? Convert.DBNull;
            parms[7].Value = info.GioiTinhDoiTuongBiKN ?? Convert.DBNull;
            parms[8].Value = info.TenChucVu ?? Convert.DBNull;

        }

        //get update parms
        private SqlParameter[] GetUpdateParms()
        {
            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter(PARAM_DOITUONGBIKN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TEN_DOI_TUONG_BI_KN,SqlDbType.NVarChar),
                new SqlParameter(PARAM_TINH_ID, SqlDbType.Int),
                new SqlParameter(PARAM_HUYEN_ID, SqlDbType.Int),
                new SqlParameter(PARAM_XA_ID, SqlDbType.Int),
                new SqlParameter(PARAM_DIACHICHITIET, SqlDbType.NVarChar),
                new SqlParameter(PARAM_LOAI_DOI_TUONG_BI_KN, SqlDbType.Int),
                new SqlParameter("@DonThuID", SqlDbType.Int),
                new SqlParameter("@GioiTinh", SqlDbType.Int),
                new SqlParameter("@ChucVu", SqlDbType.NVarChar),
            };
            return parms;
        }

        //set update parms
        private void SetUpdateParms(SqlParameter[] parms, DoiTuongBiKNInfo info)
        {
            parms[0].Value = info.DoiTuongBiKNID;
            parms[1].Value = info.TenDoiTuongBiKN ?? Convert.DBNull;
            parms[2].Value = info.TinhID ?? Convert.DBNull;
            parms[3].Value = info.HuyenID ?? Convert.DBNull;
            parms[4].Value = info.XaID ?? Convert.DBNull;
            parms[5].Value = info.DiaChiCT ?? (info.SoNhaDoiTuongBiKN ?? Convert.DBNull);
            parms[6].Value = info.LoaiDoiTuongBiKNID ?? Convert.DBNull;
            parms[7].Value = info.DonThuID ?? Convert.DBNull;
            parms[8].Value = info.GioiTinhDoiTuongBiKN ?? Convert.DBNull;
            parms[9].Value = info.TenChucVu ?? Convert.DBNull;
        }

        public IList<DoiTuongBiKNInfo> GetAll()
        {
            IList<DoiTuongBiKNInfo> ListDT = new List<DoiTuongBiKNInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_ALL, null))
                {

                    while (dr.Read())
                    {

                        DoiTuongBiKNInfo info = GetData(dr);
                        ListDT.Add(info);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return ListDT;
        }

        public DoiTuongBiKNInfo GetByID(int DTID)
        {

            DoiTuongBiKNInfo info = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_DOITUONGBIKN_ID,SqlDbType.Int)
            };
            parameters[0].Value = DTID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_ID, parameters))
                {

                    if (dr.Read())
                    {
                        info = GetData(dr);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return info;
        }

        public DoiTuongBiKNInfo GetByDonThuID(int DonThuID)
        {

            DoiTuongBiKNInfo info = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_DONTHUID,SqlDbType.Int)
            };
            parameters[0].Value = DonThuID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_DONTHUID, parameters))
                {

                    if (dr.Read())
                    {
                        info = GetData(dr);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return info;
        }

        public List<DoiTuongBiKNInfo> GetDanhSachByDonThuID(int DonThuID)
        {

            List<DoiTuongBiKNInfo> ds = new List<DoiTuongBiKNInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_DONTHUID,SqlDbType.Int)
            };
            parameters[0].Value = DonThuID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_DONTHUID, parameters))
                {
                    while (dr.Read())
                    {

                        var info = new DoiTuongBiKNInfo();
                        info = GetData(dr);
                        ds.Add(info); ;
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return ds;
        }

        //public DoiTuongBiKNInfo GetByNhomKNID(int nhomKNID)
        //{

        //    DoiTuongBiKNInfo info = null;
        //    SqlParameter[] parameters = new SqlParameter[]{
        //        new SqlParameter(PARAM_NHOMKN_ID,SqlDbType.Int)
        //    };
        //    parameters[0].Value = nhomKNID;
        //    try
        //    {
        //        using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_NHOMKN_ID, parameters))
        //        {

        //            if (dr.Read())
        //            {
        //                info = GetData(dr);
        //            }
        //            dr.Close();
        //        }
        //    }
        //    catch
        //    {
        //    }
        //    return info;
        //}

        //-----------delete----------------
        public int Delete(int DT_ID)
        {

            int val = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_DOITUONGBIKN_ID,SqlDbType.Int)
            };
            parameters[0].Value = DT_ID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, DELETE, parameters);
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

        //------------UPDATE---------------------
        public int Update(DoiTuongBiKNInfo info)
        {

            object val = null;
            SqlParameter[] parameters = GetUpdateParms();
            SetUpdateParms(parameters, info);

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, UPDATE, parameters);
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


        //------------------INSERT-----------------
        public int Insert(DoiTuongBiKNInfo info)
        {

            object val;

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
            return Utils.ConvertToInt32(val, 0);
        }


        public int DeleteByDonThuID(string ids, int donThuID)
        {

            int val = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@Ids",SqlDbType.NVarChar),
                new SqlParameter("@DonThuId",SqlDbType.Int)
            };
            parameters[0].Value = ids;
            parameters[1].Value = donThuID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "DoiTuongBiKN_Delete_By_DonThuID", parameters);
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
