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
    public class NhomKN
    {
        //Su dung de goi StoreProcedure

        private const string GET_ALL = @"NhomKN_GetAll";
        private const string GET_BY_ID = @"NhomKN_GetByID";
        private const string INSERT = @"NhomKN_Insert";
        private const string UPDATE = @"NhomKN_Update";
        private const string DELETE = @"NhomKN_Delete";

        private const string GET_BY_PAGE = @"DM_NhomKN_GetByPage";
        private const string COUNT_ALL = @"DM_NhomKN_CountAll";
        private const string COUNT_SEARCH = @"DM_NhomKN_CountSearch";
        private const string SEARCH = @"DM_NhomKN_GetBySearch";

        //quanghv: stored
        private const string GET_SO_DON_BY_CO_QUAN = @"NV_NhomKN_GetSoDonByCoQuan";

        //Ten cac bien dau vao
        private const string PARAM_NHOMKN_ID = "@NhomKNID";
        private const string PARAM_SO_LUONG = "@SoLuong";
        private const string PARAM_TENCQ = "@TenCoQuan";
        private const string PARAM_LOAI_DOI_TUONG = "@LoaiDoiTuongKNID";
        private const string PARAM_DIACHI_CQ = "@DiaChiCQ";
        private const string PARAM_DAIDIEN_PHAPLY = "@DaiDienPhapLy";
        private const string PARAM_DUOC_UYQUYEN = "@DuocUyQuyen";

        private NhomKNInfo GetData(SqlDataReader dr)
        {
            NhomKNInfo info = new NhomKNInfo();
            info.NhomKNID = Utils.GetInt32(dr["NhomKNID"], 0);
            info.SoLuong = Utils.GetInt32(dr["SoLuong"], 0);
            info.TenCQ = Utils.GetString(dr["TenCQ"], String.Empty);
            info.LoaiDoiTuongKNID = Utils.GetInt32(dr["LoaiDoiTuongKNID"], 0);
            if (info.LoaiDoiTuongKNID == (int)DMLoaiDoiTuongKN.CaNhan)
            {
                info.StringLoaiDoiTuongKN = Constant.CaNhan;
            }
            if (info.LoaiDoiTuongKNID == (int)DMLoaiDoiTuongKN.CoQuanToChuc)
            {
                info.StringLoaiDoiTuongKN = Constant.CoQuan;
            }
            if (info.LoaiDoiTuongKNID == (int)DMLoaiDoiTuongKN.TapThe)
            {
                info.StringLoaiDoiTuongKN = Constant.TapThe;
            }
            info.DiaChiCQ = Utils.ConvertToString(dr["DiaChiCQ"], String.Empty);
            info.DaiDienPhapLy = Utils.GetBoolean(dr["DaiDienPhapLy"], false);
            info.DuocUyQuyen = Utils.GetBoolean(dr["DuocUyQuyen"], false);
            if(info.DuocUyQuyen == true)
            {
                info.ThongTinNguoiDaiDien = 1;
            }
            if (info.DaiDienPhapLy == true)
            {
                info.ThongTinNguoiDaiDien = 2;
            }

            return info;
        }

        //Get Insert Parmas
        private SqlParameter[] GetInsertParms()
        {
            SqlParameter[] parms = new SqlParameter[]{
                //new SqlParameter(PARAM_NHOMKN_ID, SqlDbType.Int),
                new SqlParameter(PARAM_SO_LUONG, SqlDbType.Int),
                new SqlParameter(PARAM_TENCQ, SqlDbType.NVarChar),
                new SqlParameter(PARAM_LOAI_DOI_TUONG, SqlDbType.Int),
                new SqlParameter(PARAM_DIACHI_CQ, SqlDbType.NVarChar),
                new SqlParameter(PARAM_DAIDIEN_PHAPLY, SqlDbType.Bit),
                new SqlParameter(PARAM_DUOC_UYQUYEN, SqlDbType.Bit)

                };
            return parms;
        }

        //SET iNSERT PARMS
        private void SetInsertParms(SqlParameter[] parms, NhomKNInfo info)
        {

            //parms[0].Value = info.SoNhomKN;
            parms[0].Value = info.SoLuong ?? Convert.DBNull;
            parms[1].Value = info.TenCQ ?? Convert.DBNull;
            parms[2].Value = info.LoaiDoiTuongKNID ?? Convert.DBNull;
            parms[3].Value = info.DiaChiCQ ?? Convert.DBNull; ;
            parms[4].Value = info.DaiDienPhapLy ?? Convert.DBNull;
            parms[5].Value = info.DuocUyQuyen ?? Convert.DBNull;

        }

        //get update parms
        private SqlParameter[] GetUpdateParms()
        {
            SqlParameter[] parms = new SqlParameter[]{

                new SqlParameter(PARAM_NHOMKN_ID, SqlDbType.Int),
                new SqlParameter(PARAM_SO_LUONG, SqlDbType.Int),
                new SqlParameter(PARAM_TENCQ, SqlDbType.NVarChar),
                new SqlParameter(PARAM_LOAI_DOI_TUONG, SqlDbType.Int),
                new SqlParameter(PARAM_DIACHI_CQ, SqlDbType.NVarChar),
                new SqlParameter(PARAM_DAIDIEN_PHAPLY, SqlDbType.Bit),
                new SqlParameter(PARAM_DUOC_UYQUYEN, SqlDbType.Bit)

            };
            return parms;
        }

        //set update parms
        private void SetUpdateParms(SqlParameter[] parms, NhomKNInfo info)
        {

            parms[0].Value = info.NhomKNID;
            parms[1].Value = info.SoLuong ?? Convert.DBNull; ;
            parms[2].Value = info.TenCQ ?? Convert.DBNull;
            parms[3].Value = info.LoaiDoiTuongKNID ?? Convert.DBNull;
            parms[4].Value = info.DiaChiCQ ?? Convert.DBNull;
            parms[5].Value = info.DaiDienPhapLy ?? Convert.DBNull;
            parms[6].Value = info.DuocUyQuyen ?? Convert.DBNull;

        }

        public IList<NhomKNInfo> GetAll()
        {
            IList<NhomKNInfo> ListDT = new List<NhomKNInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_ALL, null))
                {

                    while (dr.Read())
                    {

                        NhomKNInfo info = GetData(dr);
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

        public NhomKNInfo GetByID(int DTID)
        {

            NhomKNInfo info = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_NHOMKN_ID,SqlDbType.Int)
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

        //-----------delete----------------
        public int Delete(int DT_ID)
        {

            int val = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_NHOMKN_ID,SqlDbType.Int)
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
        public int Update(NhomKNInfo info)
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
        public int Insert(NhomKNInfo info)
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
    }
}
