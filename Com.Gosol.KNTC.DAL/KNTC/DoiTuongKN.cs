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
    public class DoiTuongKN
    {
        //Su dung de goi StoreProcedure

        private const string GET_ALL = @"DoiTuongKN_GetAll";
        private const string GET_BY_ID = @"DoiTuongKN_GetByID";
        private const string GET_DIACHI_DTKHIEUNAI = @"DoiTuongKN_GetDiaChi_DTKhieuNai";
        private const string GET_DIACHI_DTKHIEUNAI_BYNHOMKN = @"DoiTuongKN_GetDiaChi_DTKhieuNai_ByNhomKN";
        private const string GET_BY_NHOMKN_ID = @"DoiTuongKN_GetByNhomKNID";
        private const string INSERT = @"DoiTuongKN_Insert_New";
        private const string UPDATE = @"DoiTuongKN_Update_New";
        private const string DELETE = @"DoiTuongKN_Delete";
        private const string DELETE_BY_NHOM_ID = @"DoiTuongKN_DeleteByNhom";

        private const string GET_FIRST_BY_NHOMKN_ID = @"DoiTuongKN_GetFirstByNhomKNID";
        private const string GET_BY_NHOM_KN_ID_JOIN = @"DoiTuongKN_GetByNhomKNIDJoin";
        private const string GET_BY_PAGE = @"DM_DoiTuongKN_GetByPage";
        private const string COUNT_ALL = @"DM_DoiTuongKN_CountAll";
        private const string COUNT_SEARCH = @"DM_DoiTuongKN_CountSearch";
        private const string SEARCH = @"DM_DoiTuongKN_GetBySearch";

        //quanghv: stored
        private const string GET_SO_DON_BY_CO_QUAN = @"NV_DoiTuongKN_GetSoDonByCoQuan";

        //Ten cac bien dau vao
        private const string PARAM_DOITUONGKN_ID = "@DoiTuongKNID";
        private const string PARAM_HO_TEN = "@HoTen";
        private const string PARAM_CMND = "@CMND";
        private const string PARAM_NGAYCAP = "@NgayCap";
        private const string PARAM_NOICAP = "@NoiCap";
        private const string PARAM_GIOITINH = "@GioiTinh";
        private const string PARAM_NGHENGHIEP = "@NgheNghiep";
        private const string PARAM_QUOCTICH_ID = "@QuocTichID";
        private const string PARAM_DANTOC_ID = "@DanTocID";
        private const string PARAM_TINH_ID = "@TinhID";
        private const string PARAM_HUYEN_ID = "@HuyenID";
        private const string PARAM_XA_ID = "@XaID";
        private const string PARAM_DIACHICHITIET = "@DiaChiCT";
        private const string PARAM_NHOMKN_ID = "@NhomKNID";
        private const string PARAM_SODIENTHOAI = "@SoDienThoai";
        private const string PARAM_SONHA = "@SoNha";
        private const string PARAM_LISTID = "@ListID";
        private DoiTuongKNInfo GetData(SqlDataReader dr)
        {
            DoiTuongKNInfo info = new DoiTuongKNInfo();
            info.DoiTuongKNID = Utils.GetInt32(dr["DoiTuongKNID"], 0);
            info.CMND = Utils.GetString(dr["CMND"], string.Empty);
            info.HoTen = Utils.GetString(dr["HoTen"], string.Empty);
            info.GioiTinh = Utils.GetInt32(dr["GioiTinh"], 0);
            info.NgheNghiep = Utils.ConvertToString(dr["NgheNghiep"], String.Empty);
            info.QuocTichID = Utils.GetInt32(dr["QuocTichID"], 0);
            info.DanTocID = Utils.GetInt32(dr["DanTocID"], 0);
            info.TinhID = Utils.GetInt32(dr["TinhID"], 0);
            info.HuyenID = Utils.GetInt32(dr["HuyenID"], 0);
            info.XaID = Utils.GetInt32(dr["XaID"], 0);
            info.DiaChiCT = Utils.GetString(dr["DiaChiCT"], string.Empty);
            info.NhomKNID = Utils.GetInt32(dr["NhomKNID"], 0);
            info.SoDienThoai = Utils.GetString(dr["SoDienThoai"], string.Empty);
            info.NoiCap = Utils.GetString(dr["NoiCap"], string.Empty);
            info.NgayCap = Utils.ConvertToDateTime(dr["NgayCap"], DateTime.MinValue);

            return info;
        }

        private DoiTuongKNInfo GetCustomDataByNhomKNID(SqlDataReader dr)
        {
            DoiTuongKNInfo info = new DoiTuongKNInfo();
            info.DoiTuongKNID = Utils.GetInt32(dr["DoiTuongKNID"], 0);
            info.CMND = Utils.GetString(dr["CMND"], string.Empty);
            info.HoTen = Utils.GetString(dr["HoTen"], string.Empty);
            info.GioiTinh = Utils.GetInt32(dr["GioiTinh"], 0);
            info.DiaChiCT = Utils.GetString(dr["DiaChiCT"], string.Empty);
            info.NhomKNID = Utils.GetInt32(dr["NhomKNID"], 0);
            info.TenTinh = Utils.ConvertToString(dr["TenTinh"], string.Empty);
            info.TenHuyen = Utils.ConvertToString(dr["TenHuyen"], string.Empty);
            info.TenXa = Utils.ConvertToString(dr["TenXa"], string.Empty);
            info.NgheNghiep = Utils.ConvertToString(dr["NgheNghiep"], string.Empty);
            info.TenDanToc = Utils.ConvertToString(dr["TenDanToc"], string.Empty);
            info.TenQuocTich = Utils.ConvertToString(dr["TenQuocTich"], string.Empty);
            info.NoiCap = Utils.GetString(dr["NoiCap"], string.Empty);
            info.NgayCap = Utils.ConvertToDateTime(dr["NgayCap"], DateTime.MinValue);
            return info;
        }
        //Get Insert Parmas
        private SqlParameter[] GetInsertParms()
        {
            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter(PARAM_HO_TEN, SqlDbType.NVarChar),
                new SqlParameter(PARAM_CMND, SqlDbType.VarChar),
                new SqlParameter(PARAM_GIOITINH, SqlDbType.Int),
                new SqlParameter(PARAM_NGHENGHIEP, SqlDbType.NVarChar),
                new SqlParameter(PARAM_QUOCTICH_ID, SqlDbType.Int),
                new SqlParameter(PARAM_DANTOC_ID, SqlDbType.Int),
                new SqlParameter(PARAM_TINH_ID, SqlDbType.Int),
                new SqlParameter(PARAM_HUYEN_ID, SqlDbType.Int),
                new SqlParameter(PARAM_XA_ID, SqlDbType.Int),
                new SqlParameter(PARAM_DIACHICHITIET, SqlDbType.NVarChar),
                new SqlParameter(PARAM_NHOMKN_ID, SqlDbType.Int),
                new SqlParameter(PARAM_SODIENTHOAI, SqlDbType.NVarChar),
                new SqlParameter(PARAM_NGAYCAP, SqlDbType.DateTime),
                new SqlParameter(PARAM_NOICAP, SqlDbType.NVarChar),
                new SqlParameter(PARAM_SONHA, SqlDbType.NVarChar),
                };
            return parms;
        }

        //SET iNSERT PARMS
        private void SetInsertParms(SqlParameter[] parms, DoiTuongKNInfo info)
        {

            parms[0].Value = info.HoTen ?? Convert.DBNull;
            parms[1].Value = info.CMND ?? Convert.DBNull;
            parms[2].Value = info.GioiTinh ?? Convert.DBNull;
            parms[3].Value = info.NgheNghiep ?? Convert.DBNull;
            parms[4].Value = info.QuocTichID ?? Convert.DBNull;
            parms[5].Value = info.DanTocID ?? Convert.DBNull;
            parms[6].Value = info.TinhID ?? Convert.DBNull;
            parms[7].Value = info.HuyenID ?? Convert.DBNull;
            parms[8].Value = info.XaID ?? Convert.DBNull;
            parms[9].Value = info.DiaChiCT ?? Convert.DBNull;
            parms[10].Value = info.NhomKNID ?? Convert.DBNull;
            parms[11].Value = info.SoDienThoai ?? Convert.DBNull;
            parms[12].Value = info.NgayCap ?? Convert.DBNull;
            parms[13].Value = info.NoiCap ?? Convert.DBNull;
            parms[14].Value = info.SoNha ?? Convert.DBNull;
            if (info.NgayCap == null)
            {
                parms[12].Value = DBNull.Value;
            }
            if (info.NoiCap == null)
            {
                parms[13].Value = DBNull.Value;
            }
        }

        //get update parms
        private SqlParameter[] GetUpdateParms()
        {
            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter(PARAM_DOITUONGKN_ID, SqlDbType.Int),
                new SqlParameter(PARAM_HO_TEN, SqlDbType.NVarChar),
                new SqlParameter(PARAM_CMND, SqlDbType.VarChar),
                new SqlParameter(PARAM_GIOITINH, SqlDbType.Int),
                new SqlParameter(PARAM_NGHENGHIEP, SqlDbType.NVarChar),
                new SqlParameter(PARAM_QUOCTICH_ID, SqlDbType.Int),
                new SqlParameter(PARAM_DANTOC_ID, SqlDbType.Int),
                new SqlParameter(PARAM_TINH_ID, SqlDbType.Int),
                new SqlParameter(PARAM_HUYEN_ID, SqlDbType.Int),
                new SqlParameter(PARAM_XA_ID, SqlDbType.Int),
                new SqlParameter(PARAM_DIACHICHITIET, SqlDbType.NVarChar),
                new SqlParameter(PARAM_NHOMKN_ID, SqlDbType.Int),
                new SqlParameter(PARAM_SODIENTHOAI, SqlDbType.NVarChar),
                new SqlParameter(PARAM_NGAYCAP, SqlDbType.DateTime),
                new SqlParameter(PARAM_NOICAP, SqlDbType.NVarChar),
                new SqlParameter(PARAM_SONHA, SqlDbType.NVarChar),
            };
            return parms;
        }

        //set update parms
        private void SetUpdateParms(SqlParameter[] parms, DoiTuongKNInfo info)
        {
            parms[0].Value = info.DoiTuongKNID;
            parms[1].Value = info.HoTen ?? Convert.DBNull;
            parms[2].Value = info.CMND ?? Convert.DBNull;
            parms[3].Value = info.GioiTinh ?? Convert.DBNull;
            parms[4].Value = info.NgheNghiep ?? Convert.DBNull; 
            parms[5].Value = info.QuocTichID ?? Convert.DBNull;
            parms[6].Value = info.DanTocID ?? Convert.DBNull;
            parms[7].Value = info.TinhID ?? Convert.DBNull;
            parms[8].Value = info.HuyenID ?? Convert.DBNull;
            parms[9].Value = info.XaID ?? Convert.DBNull;
            parms[10].Value = info.DiaChiCT ?? Convert.DBNull;
            parms[11].Value = info.NhomKNID ?? Convert.DBNull;
            parms[12].Value = info.SoDienThoai ?? Convert.DBNull;
            parms[13].Value = info.NgayCap ?? Convert.DBNull;
            parms[14].Value = info.NoiCap ?? Convert.DBNull;
            parms[15].Value = info.SoNha ?? Convert.DBNull;
            if (info.NgayCap == null)
            {
                parms[13].Value = DBNull.Value;
            }
            if (info.NoiCap == null)
            {
                parms[14].Value = DBNull.Value;
            }
        }

        public IList<DoiTuongKNInfo> GetAll()
        {
            IList<DoiTuongKNInfo> ListDT = new List<DoiTuongKNInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_ALL, null))
                {

                    while (dr.Read())
                    {

                        DoiTuongKNInfo info = GetData(dr);
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

        public DoiTuongKNInfo GetByID(int DTID)
        {

            DoiTuongKNInfo info = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_DOITUONGKN_ID,SqlDbType.Int)
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

        public DoiTuongKNInfo GetDiaChiDTKhieuNai(int DTID)
        {

            DoiTuongKNInfo info = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_DOITUONGKN_ID,SqlDbType.Int)
            };
            parameters[0].Value = DTID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DIACHI_DTKHIEUNAI, parameters))
                {

                    if (dr.Read())
                    {
                        info = GetData(dr);
                        info.TenTinh = Utils.GetString(dr["TenTinh"], string.Empty);
                        info.TenHuyen = Utils.GetString(dr["TenHuyen"], string.Empty);
                        info.TenXa = Utils.GetString(dr["TenXa"], string.Empty);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return info;
        }

        public List<DoiTuongKNInfo> GetDiaChiDTKhieuNaiByNhomKN(List<DTXuLyInfo> listNhomKN)
        {
            SqlParameter para = new SqlParameter(PARAM_LISTID, SqlDbType.Structured);
            para.TypeName = "IntList";
            var dataTable = new DataTable();
            dataTable.Columns.Add("n", typeof(int));
            foreach (var NhomKN in listNhomKN)
            {
                dataTable.Rows.Add(NhomKN.NhomKNID);
            }

            List<DoiTuongKNInfo> listDoiTuong = new List<DoiTuongKNInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                para
            };
            parameters[0].Value = dataTable;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DIACHI_DTKHIEUNAI_BYNHOMKN, parameters))
                {

                    while (dr.Read())
                    {
                        DoiTuongKNInfo info = GetData(dr);
                        info.TenTinh = Utils.GetString(dr["TenTinh"], string.Empty);
                        info.TenHuyen = Utils.GetString(dr["TenHuyen"], string.Empty);
                        info.TenXa = Utils.GetString(dr["TenXa"], string.Empty);
                        listDoiTuong.Add(info);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return listDoiTuong;
        }

        public IList<DoiTuongKNInfo> GetByNhomKNID(int nhomKNID)
        {
            List<DoiTuongKNInfo> ls = new List<DoiTuongKNInfo>();
            //DoiTuongKNInfo info = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_NHOMKN_ID,SqlDbType.Int)
            };
            parameters[0].Value = nhomKNID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_NHOMKN_ID, parameters))
                {

                    while (dr.Read())
                    {
                        //info = GetData(dr);
                        DoiTuongKNInfo info = GetData(dr);
                        //info.TenTinh = Utils.GetString(dr["TenTinh"], string.Empty);
                        //info.TenHuyen = Utils.GetString(dr["TenHuyen"], string.Empty);
                        //info.TenXa = Utils.GetString(dr["TenXa"], string.Empty);
                        ls.Add(info);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return ls;
        }

        public IList<DoiTuongKNInfo> GetCustomDataByNhomKNID(int nhomKNID)
        {
            List<DoiTuongKNInfo> ls = new List<DoiTuongKNInfo>();
            //DoiTuongKNInfo info = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_NHOMKN_ID,SqlDbType.Int)
            };
            parameters[0].Value = nhomKNID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_NHOM_KN_ID_JOIN, parameters))
                {

                    while (dr.Read())
                    {
                        //info = GetData(dr);
                        DoiTuongKNInfo info = GetCustomDataByNhomKNID(dr);
                        ls.Add(info);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return ls;
        }

        public DoiTuongKNInfo GetFirstByNhomKNID(int nhomKNID)
        {

            DoiTuongKNInfo info = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_NHOMKN_ID,SqlDbType.Int)
            };
            parameters[0].Value = nhomKNID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_FIRST_BY_NHOMKN_ID, parameters))
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
                new SqlParameter(PARAM_DOITUONGKN_ID,SqlDbType.Int)
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

        //xoa id khong nam trong nhom
        public int DeleteByNhomID(int nhomId, int doituongID)
        {

            int val = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_NHOMKN_ID, SqlDbType.Int),
                new SqlParameter(PARAM_DOITUONGKN_ID, SqlDbType.Int)
            };
            parameters[0].Value = nhomId;
            parameters[1].Value = doituongID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, DELETE_BY_NHOM_ID, parameters);
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
        public int Update(DoiTuongKNInfo info)
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
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, "v2_DoiTuongKN_Update", parameters);
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
        public int Insert(DoiTuongKNInfo info)
        {

            int val = 0;

            SqlParameter[] parameters = GetInsertParms();
            SetInsertParms(parameters, info);

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "v2_DoiTuongKN_Insert", parameters);
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
