using Com.Gosol.KNTC.Models.HeThong;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Ultilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.DAL.KNTC
{
    public class KetQuaDAL
    {
        //Su dung de goi StoreProcedure

        private const string GET_ALL = @"KetQua_GetAll";
        private const string GET_BY_ID = @"KetQua_GetByID";
        private const string INSERT = @"KetQua_Insert";
        private const string UPDATE = @"KetQua_Update";
        private const string DELETE = @"KetQua_Delete";
        private const string GET_BY_PAGE = @"KetQua_GetByPage";
        private const string COUNT_ALL = @"KetQua_CountAll";
        private const string SEARCH = @"KetQua_GetBySearch";
        private const string COUNT_SEARCH = @"KetQua_CountSearch";
        private const string GET_CUSTOMBY_XULYDONID = @"KetQua_GetCustomByXuLyDonID";
        private const string GET_BY_TIME = "KetQua_GetByTime";
        private const string INSERT_FILE_KETQUA = @"FileKetQua_Insert";
        private const string INSERT_FILE_KETQUA_New = @"FileKetQua_Insert_New";
        private const string FILEKETQUA_GETBYKETQUAID = @"FileKetQua_GetByKetQuaID";
        private const string FILEKETQUA_GETBYKETQUAID_NEW = @"FileKetQua_GetByKetQuaID_New";
        private const string FILEKETQUA_DELETE = @"FileKetQua_Delete";


        //Ten cac bien dau vao
        private const string PARAM_KET_QUA_ID = "@KetQuaID";
        private const string PARAM_LOAI_KET_QUA_ID = "@LoaiKetQuaID";
        private const string PARAM_CAN_BO_ID = "@CanBoID";
        private const string PARAM_CO_QUAN_ID = "@CoQuanID";
        private const string PARAM_NGAY_RA_KQ = "@NgayRaKQ";
        private const string PARAM_SO_TIEN = "@SoTien";
        private const string PARAM_SO_DAT = "@SoDat";
        private const string PARAM_SO_NGUOI_DUOC_TRA_QUYEN_LOI = "@SoNguoiDuocTraQuyenLoi";
        private const string PARAM_SO_DOI_TUONG_BI_XU_LY = "@SoDoiTuongBiXuLy";
        private const string PARAM_SO_DOI_TUONG_DA_BI_XU_LY = "@SoDoiTuongDaBiXuLy";
        private const string PARAM_XU_LY_DON_ID = "@XuLyDonID";
        private const string PARAM_PHANTICHKQ = "@PhanTichKQ";
        private const string PARAM_KETQUAGQLAN2 = "@KetQuaGQLan2";
        private const string PARAM_KEY = "@Keyword";
        private const string PARAM_START = "@Start";
        private const string PARAM_END = "@End";

        private const string PARAM_TEN_FILE = "@TenFile";
        private const string PARAM_TOMTAT = "@TomTat";
        private const string PARAM_FILE_URL = "@FileUrl";
        private const string PARAM_NGUOIUP = "@NguoiUp";
        private const string PARAM_NGAYUP = "@NgayUp";
        private const string PARAM_LOAIFILE = "@LoaiFile";
        private const string PARAM_FILE_ID = "@FileID";

        private KetQuaInfo GetData(SqlDataReader dr)
        {
            KetQuaInfo cInfo = new KetQuaInfo();
            cInfo.KetQuaID = Utils.ConvertToInt32(dr["KetQuaID"], 0);
            cInfo.LoaiKetQuaID = Utils.ConvertToInt32(dr["LoaiKetQuaID"], 0);
            cInfo.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
            cInfo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
            cInfo.NgayRaKQ = Utils.ConvertToDateTime(dr["NgayRaKQ"], Constant.DEFAULT_DATE);
            cInfo.SoTien = Utils.ConvertToInt32(dr["SoTien"], 0);
            cInfo.SoDat = Utils.ConvertToInt32(dr["SoDat"], 0);
            cInfo.SoNguoiDuocTraQuyenLoi = Utils.ConvertToInt32(dr["SoNguoiDuocTraQuyenLoi"], 0);
            cInfo.SoDoiTuongBiXuLy = Utils.ConvertToInt32(dr["SoDoiTuongBiXuLy"], 0);
            cInfo.SoDoiTuongDaBiXuLy = Utils.ConvertToInt32(dr["SoDoiTuongDaBiXuLy"], 0);
            cInfo.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);

            return cInfo;
        }

        private KetQuaInfo GetCustomData(SqlDataReader dr)
        {
            KetQuaInfo cInfo = new KetQuaInfo();
            cInfo.KetQuaID = Utils.ConvertToInt32(dr["KetQuaID"], 0);
            cInfo.LoaiKetQuaID = Utils.ConvertToInt32(dr["LoaiKetQuaID"], 0);
            cInfo.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
            cInfo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
            cInfo.NgayRaKQ = Utils.ConvertToDateTime(dr["NgayRaKQ"], DateTime.MinValue);
            cInfo.NgayRaKQStr = Format.FormatDate(cInfo.NgayRaKQ ?? DateTime.Now);
            cInfo.SoTien = Utils.ConvertToInt32(dr["SoTien"], 0);
            cInfo.SoDat = Utils.ConvertToInt32(dr["SoDat"], 0);
            cInfo.SoNguoiDuocTraQuyenLoi = Utils.ConvertToInt32(dr["SoNguoiDuocTraQuyenLoi"], 0);
            cInfo.SoDoiTuongBiXuLy = Utils.ConvertToInt32(dr["SoDoiTuongBiXuLy"], 0);
            cInfo.SoDoiTuongDaBiXuLy = Utils.ConvertToInt32(dr["SoDoiTuongDaBiXuLy"], 0);
            cInfo.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
            cInfo.TenLoaiKetQua = Utils.ConvertToString(dr["TenLoaiKetQua"], string.Empty);
            cInfo.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
            cInfo.TenCoQuan = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
            cInfo.FileUrl = Utils.ConvertToString(dr["FileUrl"], string.Empty);

            return cInfo;
        }

        private SqlParameter[] GetInsertParms()
        {
            SqlParameter[] parms = new SqlParameter[]{

                new SqlParameter(PARAM_LOAI_KET_QUA_ID,SqlDbType.Int),
                new SqlParameter(PARAM_CAN_BO_ID,SqlDbType.Int),
                 new SqlParameter(PARAM_CO_QUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_NGAY_RA_KQ,SqlDbType.DateTime),
                new SqlParameter(PARAM_SO_TIEN,SqlDbType.Int),
                new SqlParameter(PARAM_SO_DAT,SqlDbType.Int),
                new SqlParameter(PARAM_SO_NGUOI_DUOC_TRA_QUYEN_LOI,SqlDbType.Int),
                new SqlParameter(PARAM_SO_DOI_TUONG_BI_XU_LY,SqlDbType.Int),
                new SqlParameter(PARAM_SO_DOI_TUONG_DA_BI_XU_LY,SqlDbType.Int),
                new SqlParameter(PARAM_XU_LY_DON_ID,SqlDbType.Int),
                new SqlParameter(PARAM_FILE_URL,SqlDbType.NVarChar,2000),
                new SqlParameter(PARAM_PHANTICHKQ, SqlDbType.Int),
                new SqlParameter(PARAM_KETQUAGQLAN2, SqlDbType.Int)
            };
            return parms;
        }
        private SqlParameter[] GetInsertParmsDonDoc()
        {
            SqlParameter[] parms = new SqlParameter[]{

                new SqlParameter("@NgayDonDoc",SqlDbType.DateTime),
                new SqlParameter("@NoiDungDonDoc",SqlDbType.NVarChar),

                new SqlParameter(PARAM_XU_LY_DON_ID,SqlDbType.Int),
                new SqlParameter("@DonDocID",SqlDbType.Int)

            };
            return parms;
        }
        private SqlParameter[] GetUpdateParmsDonDoc()
        {
            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter("@KetQuaID",SqlDbType.Int),
                new SqlParameter("@NgayRaDonDoc",SqlDbType.Int),
                new SqlParameter("@NoiDungDonDoc",SqlDbType.Int),

                new SqlParameter(PARAM_XU_LY_DON_ID,SqlDbType.Int),
                new SqlParameter(PARAM_FILE_URL,SqlDbType.NVarChar,2000)

            };
            return parms;
        }


        private void SetInsertParms(SqlParameter[] parms, KetQuaInfo cInfo)
        {
            parms[0].Value = cInfo.LoaiKetQuaID;
            parms[1].Value = cInfo.CanBoID;
            parms[2].Value = cInfo.CoQuanID;
            parms[3].Value = cInfo.NgayRaKQ;
            parms[4].Value = cInfo.SoTien;
            parms[5].Value = cInfo.SoDat;
            parms[6].Value = cInfo.SoNguoiDuocTraQuyenLoi;
            parms[7].Value = cInfo.SoDoiTuongBiXuLy;
            parms[8].Value = cInfo.SoDoiTuongDaBiXuLy;
            parms[9].Value = cInfo.XuLyDonID;
            parms[10].Value = cInfo.FileUrl;
            parms[11].Value = cInfo.PhanTichKQ;
            parms[12].Value = cInfo.KetQuaGQLan2;
            if (cInfo.PhanTichKQ == 0) parms[11].Value = DBNull.Value;
            if (cInfo.KetQuaGQLan2 == 0) parms[12].Value = DBNull.Value;
        }
        private void SetInsertParmsDonDoc(SqlParameter[] parms, KetQuaInfo cInfo)
        {
            parms[0].Value = cInfo.NgayDonDoc;
            parms[1].Value = cInfo.NoiDungDonDoc;
            //parms[2].Value = cInfo.CoQuanID;
            //parms[3].Value = cInfo.NgayRaKQ;
            //parms[4].Value = cInfo.SoTien;
            //parms[5].Value = cInfo.SoDat;
            //parms[6].Value = cInfo.SoNguoiDuocTraQuyenLoi;
            //parms[7].Value = cInfo.SoDoiTuongBiXuLy;
            //parms[8].Value = cInfo.SoDoiTuongDaBiXuLy;
            parms[2].Value = cInfo.XuLyDonID;
            parms[3].Direction = ParameterDirection.Output;
            parms[3].Size = 8;

            //parms[11].Value = cInfo.PhanTichKQ;
            //parms[12].Value = cInfo.KetQuaGQLan2;
            //if (cInfo.PhanTichKQ == 0) parms[11].Value = DBNull.Value;
            //if (cInfo.KetQuaGQLan2 == 0) parms[12].Value = DBNull.Value;
        }
        private void SetUpdateParmsDonDoc(SqlParameter[] parms, KetQuaInfo cInfo)
        {
            parms[0].Value = cInfo.KetQuaID;
            parms[1].Value = cInfo.NgayDonDoc;
            parms[2].Value = cInfo.NoiDungDonDoc;
            //parms[2].Value = cInfo.CoQuanID;
            //parms[3].Value = cInfo.NgayRaKQ;
            //parms[4].Value = cInfo.SoTien;
            //parms[5].Value = cInfo.SoDat;
            //parms[6].Value = cInfo.SoNguoiDuocTraQuyenLoi;
            //parms[7].Value = cInfo.SoDoiTuongBiXuLy;
            //parms[8].Value = cInfo.SoDoiTuongDaBiXuLy;
            parms[3].Value = cInfo.XuLyDonID;
            parms[4].Value = cInfo.FileUrl;
            //parms[11].Value = cInfo.PhanTichKQ;
            //parms[12].Value = cInfo.KetQuaGQLan2;
            //if (cInfo.PhanTichKQ == 0) parms[11].Value = DBNull.Value;
            //if (cInfo.KetQuaGQLan2 == 0) parms[12].Value = DBNull.Value;
        }
        private SqlParameter[] GetUpdateParms()
        {
            SqlParameter[] parms = new SqlParameter[]{

                new SqlParameter(PARAM_KET_QUA_ID,SqlDbType.Int),
                new SqlParameter(PARAM_LOAI_KET_QUA_ID,SqlDbType.Int),
                new SqlParameter(PARAM_CAN_BO_ID,SqlDbType.Int),
                 new SqlParameter(PARAM_CO_QUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_NGAY_RA_KQ,SqlDbType.DateTime),
                new SqlParameter(PARAM_SO_TIEN,SqlDbType.Int),
                new SqlParameter(PARAM_SO_DAT,SqlDbType.Int),
                new SqlParameter(PARAM_SO_NGUOI_DUOC_TRA_QUYEN_LOI,SqlDbType.Int),
                new SqlParameter(PARAM_SO_DOI_TUONG_BI_XU_LY,SqlDbType.Int),
                new SqlParameter(PARAM_SO_DOI_TUONG_DA_BI_XU_LY,SqlDbType.Int),
                new SqlParameter(PARAM_FILE_URL,SqlDbType.NVarChar,2000),
                new SqlParameter(PARAM_PHANTICHKQ, SqlDbType.Int),
                new SqlParameter(PARAM_KETQUAGQLAN2, SqlDbType.Int)

            };
            return parms;
        }

        private void SetUpdateParms(SqlParameter[] parms, KetQuaInfo cInfo)
        {

            parms[0].Value = cInfo.KetQuaID;
            parms[1].Value = cInfo.LoaiKetQuaID;
            parms[2].Value = cInfo.CanBoID;
            parms[3].Value = cInfo.CoQuanID;
            parms[4].Value = cInfo.NgayRaKQ;
            parms[5].Value = cInfo.SoTien;
            parms[6].Value = cInfo.SoDat;
            parms[7].Value = cInfo.SoNguoiDuocTraQuyenLoi;
            parms[8].Value = cInfo.SoDoiTuongBiXuLy;
            parms[9].Value = cInfo.SoDoiTuongDaBiXuLy;
            parms[10].Value = cInfo.FileUrl;
            parms[11].Value = cInfo.PhanTichKQ;
            parms[12].Value = cInfo.KetQuaGQLan2;

            if (cInfo.PhanTichKQ == 0) parms[11].Value = DBNull.Value;
            if (cInfo.KetQuaGQLan2 == 0) parms[12].Value = DBNull.Value;
        }

        public IList<KetQuaInfo> GetAll()
        {
            IList<KetQuaInfo> ketquas = new List<KetQuaInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_ALL, null))
                {

                    while (dr.Read())
                    {

                        KetQuaInfo cInfo = GetData(dr);
                        ketquas.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return ketquas;
        }

        public IList<KetQuaInfo> GetByTime(DateTime startDate, DateTime endDate)
        {
            IList<KetQuaInfo> ketquas = new List<KetQuaInfo>();
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("StartDate", SqlDbType.DateTime),
                new SqlParameter("EndDate", SqlDbType.DateTime)
            };

            parms[0].Value = startDate;
            parms[1].Value = endDate;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_TIME, parms))
                {

                    while (dr.Read())
                    {
                        KetQuaInfo cInfo = GetData(dr);
                        cInfo.FileUrl = Utils.ConvertToString(dr["FileUrl"], string.Empty);
                        ketquas.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return ketquas;
        }

        public KetQuaInfo GetByID(int cID)
        {

            KetQuaInfo cInfo = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_KET_QUA_ID,SqlDbType.Int)
            };
            parameters[0].Value = cID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_ID, parameters))
                {

                    if (dr.Read())
                    {
                        cInfo = GetData(dr);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return cInfo;
        }
        public PXLModel GetByID(int? CanBoID, int? XuLyDonID)
        {

            PXLModel cInfo = new PXLModel();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@CanBoID",SqlDbType.Int),
                new SqlParameter("@XuLyDonID",SqlDbType.Int)
            };
            parameters[0].Value = CanBoID;
            parameters[1].Value = XuLyDonID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "PXLHGQST_GetByID", parameters))
                {

                    if (dr.Read())
                    {
                        {
                            cInfo = new PXLModel();
                            cInfo.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
                            cInfo.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                            cInfo.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                            cInfo.HanGiaiQuyetCu = Utils.ConvertToDateTime(dr["NgayHetHanCu"], DateTime.MinValue);
                            cInfo.PreStateID = Utils.ConvertToInt32(dr["PreStateID"], 0);
                        }
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return cInfo;
        }
        public KetQuaInfo GetByXuLyDonID(int XuLyDonID)
        {

            KetQuaInfo cInfo = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("XuLyDonID",SqlDbType.Int)
            };
            parameters[0].Value = XuLyDonID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_KetQua_GetByXuLyDonID", parameters))
                {

                    if (dr.Read())
                    {
                        cInfo = new KetQuaInfo();
                        cInfo.KetQuaID = Utils.ConvertToInt32(dr["KetQuaID"], 0);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return cInfo;
        }

        public KetQuaInfo GetCustomByXuLyDonID(int xLDID)
        {

            KetQuaInfo kQuaInfo = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XU_LY_DON_ID,SqlDbType.Int)
            };
            parameters[0].Value = xLDID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_CUSTOMBY_XULYDONID, parameters))
                {

                    if (dr.Read())
                    {
                        kQuaInfo = GetCustomData(dr);
                        kQuaInfo.lstFileKQ = new List<FileHoSoInfo>();
                        kQuaInfo.lstFileKQ = GetFileHoSoByKetQuaID(kQuaInfo.KetQuaID, (int)EnumLoaiFile.FileBanHanhQD);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return kQuaInfo;
        }
        public List<KetQuaInfo> GetCustomByXuLyDonID_DonDoc(int xLDID)
        {

            List<KetQuaInfo> kQuaInfo = new List<KetQuaInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XU_LY_DON_ID,SqlDbType.Int)
            };
            parameters[0].Value = xLDID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "DonDoc_GetCustomByXuLyDonID", parameters))
                {

                    while (dr.Read())
                    {
                        var Info = new KetQuaInfo();
                        Info.DonDocID = Utils.ConvertToInt32(dr["DonDocID"], 0);
                        Info.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
                        Info.NgayDonDoc = Utils.ConvertToDateTime(dr["NgayDonDoc"], DateTime.Now);
                        Info.NoiDungDonDoc = Utils.ConvertToString(dr["NoiDungDonDoc"], string.Empty);

                        kQuaInfo.Add(Info);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return kQuaInfo;
        }
        public int Update(KetQuaInfo cInfo)
        {

            int val = 0;
            SqlParameter[] parameters = GetUpdateParms();
            SetUpdateParms(parameters, cInfo);

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, UPDATE, parameters);
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
        public int UpdateDonDoc(KetQuaInfo cInfo)
        {

            int val = 0;
            SqlParameter[] parameters = GetUpdateParmsDonDoc();
            SetUpdateParmsDonDoc(parameters, cInfo);

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "Update_RaVanBanDonDoc", parameters);
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

        public int Insert(KetQuaInfo cInfo)
        {

            object val = 0;

            SqlParameter[] parameters = GetInsertParms();
            SetInsertParms(parameters, cInfo);

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
        public int InsertPreStateDueDate(int? StateID, int? XuLyDonID, DateTime? NgayHetHanCu, int? CanBoID)
        {

            object val = 0;


            SqlParameter[] parms = new SqlParameter[]{

                new SqlParameter("@StateID",SqlDbType.Int),
                new SqlParameter("@XuLyDonID",SqlDbType.Int),
                new SqlParameter("@NgayHetHanCu",SqlDbType.DateTime),
                new SqlParameter("@CanBoID",SqlDbType.Int)
            };
            parms[0].Value = StateID ?? Convert.DBNull;
            parms[1].Value = XuLyDonID ?? Convert.DBNull;
            parms[2].Value = NgayHetHanCu ?? Convert.DBNull;
            parms[3].Value = CanBoID ?? Convert.DBNull;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "Insert_Aft", parms);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw ex;
                    }
                }
                conn.Close();
            }
            return Convert.ToInt32(val);
        }

        public int Insert_RaVanBanDonDoc(KetQuaInfo cInfo, ref int DonDocID)
        {

            object val = 0;
            SqlParameter[] parameters = GetInsertParmsDonDoc();
            SetInsertParmsDonDoc(parameters, cInfo);

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "DonDoc_Insert", parameters);
                        //var a = parameters[3].Value.ToString();
                        //DonDocID = Utils.ConvertToInt32(parameters[3].Value, 0);
                        trans.Commit();
                        DonDocID = Utils.ConvertToInt32(parameters[3].Value, 0);
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
        public int Insert_GiaHanGiaiQuyet(KetQuaInfo cInfo, ref int GiaHanGiaiQuyetID, int? StateID)
        {

            object val = 0;
            SqlParameter[] parms = new SqlParameter[]{

                new SqlParameter("@HanGiaiQuyetMoi",SqlDbType.DateTime),
                new SqlParameter("@LyDoDieuChinh",SqlDbType.NVarChar),

                new SqlParameter(PARAM_XU_LY_DON_ID,SqlDbType.Int),
                new SqlParameter("@HanGiaiQuyetCu",SqlDbType.DateTime),
                 new SqlParameter("@CanBoThayDoi",SqlDbType.Int),
                new SqlParameter("@NgayThayDoi",SqlDbType.DateTime),
                 new SqlParameter("@GiaHanGiaiQuyetID",SqlDbType.Int),
                         new SqlParameter("@StateID",SqlDbType.Int)

            };
            parms[0].Value = cInfo.HanGiaiQuyetMoi ?? Convert.DBNull;
            parms[1].Value = cInfo.LyDoDieuChinh;
            //parms[2].Value = cInfo.CoQuanID;
            //parms[3].Value = cInfo.NgayRaKQ;
            //parms[4].Value = cInfo.SoTien;
            //parms[5].Value = cInfo.SoDat;
            //parms[6].Value = cInfo.SoNguoiDuocTraQuyenLoi;
            //parms[7].Value = cInfo.SoDoiTuongBiXuLy;
            //parms[8].Value = cInfo.SoDoiTuongDaBiXuLy;
            parms[2].Value = cInfo.XuLyDonID;
            parms[3].Value = cInfo.HanGiaiQuyetCu ?? Convert.DBNull;
            parms[4].Value = cInfo.NguoiUp;
            parms[5].Value = cInfo.NgayThayDoi ?? Convert.DBNull;
            parms[6].Direction = ParameterDirection.Output;
            parms[7].Value = StateID ?? Convert.DBNull;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "GiaHanGiaiQuyet_Insert", parms);
                        //var a = parameters[3].Value.ToString();
                        //DonDocID = Utils.ConvertToInt32(parameters[3].Value, 0);
                        trans.Commit();
                        GiaHanGiaiQuyetID = Utils.ConvertToInt32(parms[6].Value, 0);
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
        public int Update_DueDateTrans(DateTime? HanXuLyMoi, int? XuLyDonID, int? RoleID, int? TransitionID, int? CanBoDangNhap)
        {

            object val = 0;
            SqlParameter[] parms = new SqlParameter[]{

                new SqlParameter(PARAM_XU_LY_DON_ID,SqlDbType.Int),
                 new SqlParameter("@HanXuLyMoi",SqlDbType.DateTime),
                   new SqlParameter("@RoleID",SqlDbType.Int),
                      new SqlParameter("@TransitionID",SqlDbType.Int),
                      new SqlParameter("@CanBoID",SqlDbType.Int)

            };
            parms[0].Value = XuLyDonID ?? Convert.DBNull;
            parms[1].Value = HanXuLyMoi ?? Convert.DBNull;
            parms[2].Value = RoleID ?? Convert.DBNull;
            parms[3].Value = TransitionID == 0 ? Convert.DBNull : TransitionID;
            parms[4].Value = CanBoDangNhap ?? Convert.DBNull;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "Trans_UpdateDue", parms);
                        //var a = parameters[3].Value.ToString();
                        //DonDocID = Utils.ConvertToInt32(parameters[3].Value, 0);
                        trans.Commit();
                        //GiaHanGiaiQuyetID = Utils.ConvertToInt32(parms[6].Value, 0);
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
        public int Update_PreState(int? StateID, int? xulydonID, int? canboID)
        {

            object val = 0;
            SqlParameter[] parms = new SqlParameter[]{

                   new SqlParameter("@StateID",SqlDbType.Int),
   new SqlParameter("@XuLyDonID",SqlDbType.Int),
      new SqlParameter("@CanBoID",SqlDbType.Int)

            };
            parms[0].Value = StateID ?? Convert.DBNull;
            parms[1].Value = xulydonID ?? Convert.DBNull;
            parms[2].Value = canboID ?? Convert.DBNull;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "Trans_UpdatePreState", parms);
                        //var a = parameters[3].Value.ToString();
                        //DonDocID = Utils.ConvertToInt32(parameters[3].Value, 0);
                        trans.Commit();
                        //GiaHanGiaiQuyetID = Utils.ConvertToInt32(parms[6].Value, 0);
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
        public IList<KetQuaInfo> GetBySearch(string keyword, int page)
        {
            int start = (page - 1) * Constant.PageSize;
            int end = page * Constant.PageSize;

            IList<KetQuaInfo> ketquas = new List<KetQuaInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_KEY,SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_START,SqlDbType.Int),
                new SqlParameter(PARAM_END,SqlDbType.Int)
            };
            parameters[0].Value = keyword;
            parameters[1].Value = start;
            parameters[2].Value = end;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, SEARCH, parameters))
                {
                    while (dr.Read())
                    {
                        KetQuaInfo cInfo = GetData(dr);
                        ketquas.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return ketquas;
        }

        public IList<KetQuaInfo> GetByPage(int page)
        {
            int start = (page - 1) * Constant.PageSize;
            int end = page * Constant.PageSize;

            IList<KetQuaInfo> ketquas = new List<KetQuaInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_START,SqlDbType.Int),
                new SqlParameter(PARAM_END,SqlDbType.Int)
            };
            parameters[0].Value = start;
            parameters[1].Value = end;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_PAGE, parameters))
                {
                    while (dr.Read())
                    {
                        KetQuaInfo cInfo = GetData(dr);
                        ketquas.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return ketquas;
        }

        public int CountAll()
        {
            int result = 0;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_ALL, null))
                {

                    if (dr.Read())
                    {
                        result = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return result;
        }

        public int CountSearch(string keyword)
        {

            int result = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_KEY,SqlDbType.NVarChar,50)
            };
            parameters[0].Value = keyword;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_SEARCH, parameters))
                {

                    if (dr.Read())
                    {
                        result = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return result;
        }
        //Xu ly file kết quả
        private SqlParameter[] GetInsertFileKQParms()
        {
            SqlParameter[] parms = new SqlParameter[]{
                //new SqlParameter(PARAM_FileHoSo_ID, SqlDbType.Int),
                new SqlParameter(PARAM_TEN_FILE, SqlDbType.NVarChar),
                new SqlParameter(PARAM_TOMTAT, SqlDbType.NVarChar),
                new SqlParameter(PARAM_NGAYUP, SqlDbType.DateTime),
                new SqlParameter(PARAM_NGUOIUP, SqlDbType.Int),
                new SqlParameter(PARAM_FILE_URL, SqlDbType.NVarChar),
                new SqlParameter(PARAM_KET_QUA_ID, SqlDbType.Int),
                new SqlParameter(PARAM_FILE_ID, SqlDbType.Int)


                };
            return parms;
        }
        private SqlParameter[] GetInsertFileDonDoc()
        {
            SqlParameter[] parms = new SqlParameter[]{
                //new SqlParameter(PARAM_FileHoSo_ID, SqlDbType.Int),

                new SqlParameter("@TenFile", SqlDbType.NVarChar),

                new SqlParameter("@NgayUp", SqlDbType.DateTime),
                new SqlParameter("@NguoiUp", SqlDbType.Int),
                new SqlParameter("@FileURL", SqlDbType.NVarChar),
                new SqlParameter("@DonDocID", SqlDbType.Int),
                new SqlParameter("@FileHoSoID", SqlDbType.Int),
                new SqlParameter("@FileID", SqlDbType.Int)

                };
            return parms;
        }
        private void SetInsertFileKQParms(SqlParameter[] parms, FileHoSoInfo info)
        {

            //parms[0].Value = info.SoFileHoSo;
            parms[0].Value = info.TenFile;
            parms[1].Value = info.TomTat;
            parms[2].Value = info.NgayUp;
            parms[3].Value = info.NguoiUp;
            parms[4].Value = info.FileURL;
            parms[5].Value = info.KetQuaID;
            parms[6].Value = info.FileID;

        }
        private void SetInsertDonDoc(SqlParameter[] parms, FileHoSoInfo info)
        {

            //parms[0].Value = info.SoFileHoSo;
            parms[0].Value = info.TenFile;
            //parms[1].Value = info.TomTat;
            parms[1].Value = info.NgayUp;
            parms[2].Value = info.NguoiUp;
            parms[3].Value = info.FileURL;
            parms[4].Value = info.DonDocID;
            parms[5].Direction = ParameterDirection.Output;
            parms[6].Value = info.FileID;

        }
        public int InsertFileKetQua(FileHoSoInfo info)
        {

            object val;

            SqlParameter[] parameters = GetInsertFileKQParms();
            SetInsertFileKQParms(parameters, info);

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, INSERT_FILE_KETQUA_New, parameters);
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
        public int InsertFileDonDoc(FileHoSoInfo info, ref int FileHoSoID)
        {

            object val;

            SqlParameter[] parameters = GetInsertFileDonDoc();
            SetInsertDonDoc(parameters, info);

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "Insert_FileDonDoc_New", parameters);
                        trans.Commit();
                        FileHoSoID = Utils.ConvertToInt32(parameters[5].Value, 0);
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
        public int InsertFileGiaHanGiaiQuyet(FileHoSoInfo info)
        {

            object val;


            SqlParameter[] parms = new SqlParameter[]{
                //new SqlParameter(PARAM_FileHoSo_ID, SqlDbType.Int),
                new SqlParameter("@TenFile", SqlDbType.NVarChar),

                new SqlParameter("@NgayUp", SqlDbType.DateTime),
                new SqlParameter("@NguoiUp", SqlDbType.Int),
                new SqlParameter("@FileURL", SqlDbType.NVarChar),
                new SqlParameter("@GiaHanGiaiQuyetID", SqlDbType.Int),
                 new SqlParameter("@FileHoSoID", SqlDbType.Int)

                };

            parms[0].Value = info.TenFile;
            //parms[1].Value = info.TomTat;
            parms[1].Value = info.NgayUp;
            parms[2].Value = info.NguoiUp;
            parms[3].Value = info.FileURL;
            parms[4].Value = info.GiaHanGiaiQuyetID;
            parms[5].Direction = ParameterDirection.Output;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "Insert_FileGiaHanGiaiQuyet", parms);
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
        public int Delete(int ketQuaID)
        {

            int val = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_KET_QUA_ID,SqlDbType.Int),
            };
            parameters[0].Value = ketQuaID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, FILEKETQUA_DELETE, parameters);
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
        public List<FileHoSoInfo> GetFileHoSoByKetQuaID(int ketQuaID, int loaiFile)
        {
            List<FileHoSoInfo> lst_HoSo = new List<FileHoSoInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_KET_QUA_ID,SqlDbType.Int),
                new SqlParameter(PARAM_LOAIFILE,SqlDbType.Int)
            };
            parameters[0].Value = ketQuaID;
            parameters[1].Value = loaiFile;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, FILEKETQUA_GETBYKETQUAID_NEW, parameters))
                {
                    while (dr.Read())
                    {
                        FileHoSoInfo cInfo = new FileHoSoInfo();
                        cInfo.FileHoSoID = Utils.ConvertToInt32(dr["FileHoSoID"], 0);
                        cInfo.NguoiUp = Utils.ConvertToInt32(dr["NguoiUp"], 0);
                        cInfo.TenCoQuanUp = new CoQuan().GetCoQuanByCanBoID(cInfo.NguoiUp).TenCoQuan;
                        //cInfo.TenFile = Utils.ConvertToString(dr["TenFile"], string.Empty);
                        cInfo.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        cInfo.NgayUp = Utils.ConvertToDateTime(dr["NgayUp"], DateTime.MinValue);
                        cInfo.NgayUp_str = Format.FormatDate(cInfo.NgayUp);
                        cInfo.FileURL = Utils.ConvertToString(dr["FileURL"], string.Empty);
                        cInfo.IsBaoMat = Utils.ConvertToBoolean(dr["IsBaoMat"], false);
                        //cInfo.CanBoID = Utils.ConvertToInt32(dr["NguoiUp"], 0);
                        cInfo.FileID = Utils.ConvertToInt32(dr["FileID"], 0);
                        cInfo.TenFile = Utils.ConvertToString(dr["TenFileNew"], string.Empty);
                        cInfo.NhomFileID = Utils.ConvertToInt32(dr["NhomFileID"], 0);
                        cInfo.TenNhomFile = Utils.ConvertToString(dr["TenNhomFile"], string.Empty);
                        cInfo.ThuTuHienThiNhom = Utils.ConvertToInt32(dr["ThuTuHienThi"], 0);
                        cInfo.ThuTuHienThiFile = Utils.ConvertToInt32(dr["ThuTuHienThiFile"], 0);
                        lst_HoSo.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return lst_HoSo;
        }
        public List<FileHoSoInfo> GetFileHoSoByDonDocID(int dondocID, int loaiFile)
        {
            List<FileHoSoInfo> lst_HoSo = new List<FileHoSoInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@DonDocID",SqlDbType.Int),
                new SqlParameter(PARAM_LOAIFILE,SqlDbType.Int)
            };
            parameters[0].Value = dondocID;
            parameters[1].Value = loaiFile;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "FileHoSoDonDoc_GetByKetQuaID", parameters))
                {
                    while (dr.Read())
                    {
                        FileHoSoInfo cInfo = new FileHoSoInfo();
                        cInfo.FileHoSoID = Utils.ConvertToInt32(dr["FileHoSoID"], 0);
                        cInfo.NguoiUp = Utils.ConvertToInt32(dr["NguoiUp"], 0);
                        cInfo.TenCoQuanUp = new CoQuan().GetCoQuanByCanBoID(cInfo.NguoiUp).TenCoQuan;
                        cInfo.TenFile = Utils.ConvertToString(dr["TenFile"], string.Empty);
                        cInfo.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        cInfo.NgayUp = Utils.ConvertToDateTime(dr["NgayUp"], DateTime.MinValue);
                        cInfo.NgayUp_str = Format.FormatDate(cInfo.NgayUp);
                        cInfo.FileURL = Utils.ConvertToString(dr["FileURL"], string.Empty);
                        cInfo.IsBaoMat = Utils.ConvertToBoolean(dr["IsBaoMat"], false);
                        //cInfo.CanBoID = Utils.ConvertToInt32(dr["NguoiUp"], 0);
                        lst_HoSo.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return lst_HoSo;
        }


        #region create by AnhVH 10/07/2020

        //////////////////////////// Ra quyết định

        /// <summary>
        /// thêm mới thông tin quyết định giải quyết -- ban hành quyết định
        /// </summary>
        /// <param name="cInfo"></param>
        /// <returns></returns>
        public int QuyetDinh_ThemMoi_ThongTin(QuyetDinhInfo cInfo)
        {
            int val = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@SoQuyetDinh",SqlDbType.NVarChar),
                new SqlParameter("@NgayRaKQ",SqlDbType.DateTime),
                 new SqlParameter("@CoQuanID",SqlDbType.Int),
                new SqlParameter("@QuyetDinh",SqlDbType.Int),
                new SqlParameter("@PhanTichKQ",SqlDbType.Int),
                new SqlParameter("@KetQuaGQLan2",SqlDbType.Int),
                new SqlParameter("@XuLyDonID",SqlDbType.Int),
                new SqlParameter("@CanBoID",SqlDbType.Int),
                   new SqlParameter("ThoiHanThiHanh",SqlDbType.DateTime),
                new SqlParameter("TomTatNoiDungGQ",SqlDbType.NVarChar),
                new SqlParameter("LoaiKetQuaID",SqlDbType.Int),
                new SqlParameter("SoTienThuHoi",SqlDbType.Decimal),
                new SqlParameter("SoDatThuHoi",SqlDbType.Decimal),
                new SqlParameter("SoCaNhan",SqlDbType.Int),
                new SqlParameter("SoTienCaNhanTraLai",SqlDbType.Decimal),
                new SqlParameter("SoDatCaNhanTraLai",SqlDbType.Decimal),
                new SqlParameter("SoToChuc",SqlDbType.Int),
                new SqlParameter("SoTienToChucTraLai",SqlDbType.Decimal),
                new SqlParameter("SoDatToChucTraLai",SqlDbType.Decimal),
                new SqlParameter("SoNguoiBiKienNghiXuLy",SqlDbType.Int),
                new SqlParameter("SoCanBoBiXuLy",SqlDbType.Int),
                new SqlParameter("SoNguoiChuyenCoQuanDieuTra",SqlDbType.Int),
                new SqlParameter("SoCanBoChuyenCoQuanDieuTra",SqlDbType.Int),
            };

            parameters[0].Value = cInfo.SoQuyetDinh ?? Convert.DBNull;
            parameters[1].Value = cInfo.NgayQuyetDinh ?? Convert.DBNull;
            parameters[2].Value = cInfo.CoQuanBanHanh ?? Convert.DBNull;
            parameters[3].Value = cInfo.QuyetDinh ?? Convert.DBNull;
            parameters[4].Value = cInfo.PhanTichKetQua ?? Convert.DBNull;
            parameters[5].Value = cInfo.KetQuaGiaiQuyetLan2 ?? Convert.DBNull;
            parameters[6].Value = cInfo.XuLyDonID ?? Convert.DBNull;
            parameters[7].Value = cInfo.CanBoID ?? Convert.DBNull;
            parameters[8].Value = cInfo.ThoiHanThiHanh ?? Convert.DBNull;
            parameters[9].Value = cInfo.TomTatNoiDungGQ ?? Convert.DBNull;
            parameters[10].Value = cInfo.LoaiKetQuaID ?? Convert.DBNull;
            parameters[11].Value = cInfo.SoTienThuHoi ?? Convert.DBNull;
            parameters[12].Value = cInfo.SoDatThuHoi ?? Convert.DBNull;
            parameters[13].Value = cInfo.SoCaNhan ?? Convert.DBNull;
            parameters[14].Value = cInfo.SoTienCaNhanTraLai ?? Convert.DBNull;
            parameters[15].Value = cInfo.SoDatCaNhanTraLai ?? Convert.DBNull;
            parameters[16].Value = cInfo.SoToChuc ?? Convert.DBNull;
            parameters[17].Value = cInfo.SoTienToChucTraLai ?? Convert.DBNull;
            parameters[18].Value = cInfo.SoDatToChucTraLai ?? Convert.DBNull;
            parameters[19].Value = cInfo.SoNguoiBiKienNghiXuLy ?? Convert.DBNull;
            parameters[20].Value = cInfo.SoCanBoBiXuLy ?? Convert.DBNull;
            parameters[21].Value = cInfo.SoNguoiChuyenCoQuanDieuTra ?? Convert.DBNull;
            parameters[22].Value = cInfo.SoCanBoChuyenCoQuanDieuTra ?? Convert.DBNull;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = Utils.ConvertToInt32(SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, "v2_QuyetDinh_Insert_ThongTin", parameters), 0);
                        //val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "QuyetDinh_Insert_ThongTin", parameters);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw ex;
                    }
                }
                conn.Close();
            }
            return Convert.ToInt32(val);
        }

        /// <summary>
        /// Thêm 1 nội dung quyết định,
        /// bao gồm cả danh sách cơ quan thi hành, 
        /// </summary>
        /// <param name="Info"></param>
        /// <returns></returns>
        public int QuyetDinh_ThemMoi_NoiDung(NoiDungQuyetDinhInfo Info)
        {
            int val = 0;
            var pList = new SqlParameter("DanhSachCoQuanThiHanh", SqlDbType.Structured);
            pList.TypeName = "dbo.TwoID";
            var tbCoQuanThiHanh = new DataTable();
            tbCoQuanThiHanh.Columns.Add("ID1", typeof(int));
            tbCoQuanThiHanh.Columns.Add("ID2", typeof(int));
            if (Info.ListCoQuanThiHanh.Count > 0)
            {
                Info.ListCoQuanThiHanh.ForEach(x => tbCoQuanThiHanh.Rows.Add(x.CoQuanID, x.VaiTro));
            }
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(@"KetQuaID",SqlDbType.Int),
                new SqlParameter("LoaiKetQua",SqlDbType.Int),
                 new SqlParameter("SoTien",SqlDbType.Decimal),
                new SqlParameter("SoDat",SqlDbType.Decimal),
                new SqlParameter("SoNguoiDuocTraQuyenLoi",SqlDbType.Int),
                new SqlParameter("SoDoiTuongBiXuLy",SqlDbType.Int),
                new SqlParameter("NoiDungQuyetDinh",SqlDbType.NVarChar),
                new SqlParameter("ThoiHanThiHanh",SqlDbType.DateTime),
                new SqlParameter("CanBoID",SqlDbType.Int),
                pList
            };

            parameters[0].Value = Info.KetQuaID ?? Convert.DBNull;
            parameters[1].Value = Info.LoaiKetQuaID ?? Convert.DBNull;
            parameters[2].Value = Info.SoTien ?? Convert.DBNull;
            parameters[3].Value = Info.SoDat ?? Convert.DBNull;
            parameters[4].Value = Info.SoNguoiDuocTraQuyenLoi ?? Convert.DBNull;
            parameters[5].Value = Info.SoDoiTuongBiXuLy ?? Convert.DBNull;
            parameters[6].Value = Info.NoiDungQuyetDinh ?? Convert.DBNull;
            parameters[7].Value = Info.ThoiHanThiHanh ?? Convert.DBNull;
            parameters[8].Value = Info.CanBoID ?? Convert.DBNull;
            parameters[9].Value = tbCoQuanThiHanh;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = Utils.ConvertToInt32(SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, "QuyetDinh_Insert_NoiDung", parameters), 0);
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


        /// <summary>
        /// thêm quyết định, 
        /// phần thông tin quyết định, 
        /// danh sách nội dung quyết định
        /// thêm file đính kèm
        /// thêm log file
        /// </summary>
        /// <param name="cInfo"></param>
        /// <returns></returns>
        public int QuyetDinh_ThemMoi(QuyetDinhInfo cInfo)
        {
            var val = 0;
            try
            {
                val = QuyetDinh_ThemMoi_ThongTin(cInfo);
                if (val > 0 && cInfo.ListFileQuyetDinh != null && cInfo.ListFileQuyetDinh.Count > 0)
                {
                    foreach (var item in cInfo.ListFileQuyetDinh)
                    {
                        //var query = InsertFileKetQua(item);
                        //if (query <= 0)
                        //{
                        //    return query;
                        //}
                        //FileLogInfo infoFileLog = new FileLogInfo();
                        //infoFileLog.LoaiLog = (int)EnumLoaiLog.Them;
                        //infoFileLog.LoaiFile = (int)EnumLoaiFile.FileBanHanhQD;
                        //infoFileLog.IsBaoMat = false;
                        //infoFileLog.IsMaHoa = false;
                        //infoFileLog.FileID = query;
                        //new FileLog().Insert(infoFileLog);
                        item.KetQuaID = val;
                        var query = QuyetDinh_Them_File(item);
                        if (query < 0)
                        {
                            return query;
                        }
                    }
                }
                if (val > 0)
                {
                    if (cInfo.ListNoiDungQuyetDinh != null && cInfo.ListNoiDungQuyetDinh.Count > 0)
                    {
                        foreach (var item in cInfo.ListNoiDungQuyetDinh)
                        {
                            item.KetQuaID = val;
                            var query = QuyetDinh_ThemMoi_NoiDung(item);
                            if (query < 1) return query;
                        }
                    }
                }
                return val;
            }
            catch (Exception)
            {
                return -1;
                throw;
            }
        }


        /// <summary>
        /// Lấy đầy đủ thông tin của quyết định (kết quả cũ) theo XuLyDonID,
        /// gồm: Thông tin quyết định, danh sách file đính kèm của qđ, danh sách nội dung quyết định, ds cơ quan thi hành của 1 nội dung
        /// </summary>
        /// <param name="XuLyDonID"></param>
        /// <returns></returns>
        public QuyetDinhInfo QuyetDinh_GetBy_XuLyDonID(int XuLyDonID)
        {
            var result = new QuyetDinhInfo();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("XuLyDonID",SqlDbType.Int)
            };
            parameters[0].Value = XuLyDonID;
            try
            {
                var tmp = new List<KetQuaInfo>();
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_QuyetDinh_GetBy_XuLyDonID", parameters))
                {

                    while (dr.Read())
                    {
                        KetQuaInfo kQuaInfo = new KetQuaInfo();

                        kQuaInfo.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
                        kQuaInfo.KetQuaID = Utils.ConvertToInt32(dr["KetQuaID"], 0);
                        kQuaInfo.LoaiKhieuTo1ID = Utils.ConvertToInt32(dr["LoaiKhieuTo1ID"], 0);
                        kQuaInfo.SoQuyetDinh = Utils.ConvertToString(dr["SoQuyetDinh"], string.Empty);
                        kQuaInfo.NgayRaKQ = Utils.ConvertToDateTime(dr["NgayRaKQ"], DateTime.MinValue);
                        kQuaInfo.NgayRaKQStr = kQuaInfo.NgayRaKQ.Value.ToString("dd/MM/yyyy");
                        kQuaInfo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        kQuaInfo.TenCoQuanBanHanh = Utils.ConvertToString(dr["TenCoQuanBanHanh"], string.Empty);
                        kQuaInfo.TenCoQuan = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        kQuaInfo.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        kQuaInfo.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        kQuaInfo.QuyetDinh = Utils.ConvertToInt32(dr["QuyetDinh"], 0);
                        kQuaInfo.PhanTichKQ = Utils.ConvertToInt32(dr["PhanTichKQ"], 0);
                        kQuaInfo.KetQuaGQLan2 = Utils.ConvertToInt32(dr["KetQuaGQLan2"], 0);

                        kQuaInfo.ThoiHanThiHanh = Utils.ConvertToNullableDateTime(dr["ThoiHanThiHanh"], null);
                        kQuaInfo.TomTatNoiDungGQ = Utils.ConvertToString(dr["TomTatNoiDungGQ"], string.Empty);
                        kQuaInfo.LoaiKetQuaID = Utils.ConvertToNullableInt32(dr["LoaiKetQuaID"], null);
                        kQuaInfo.SoTienThuHoi = Utils.ConvertToDecimal(dr["SoTienThuHoi"], 0);
                        kQuaInfo.SoDatThuHoi = Utils.ConvertToDecimal(dr["SoDatThuHoi"], 0);
                        kQuaInfo.SoCaNhan = Utils.ConvertToNullableInt32(dr["SoCaNhan"], 0);
                        kQuaInfo.SoTienCaNhanTraLai = Utils.ConvertToDecimal(dr["SoTienCaNhanTraLai"], 0);
                        kQuaInfo.SoDatCaNhanTraLai = Utils.ConvertToDecimal(dr["SoDatCaNhanTraLai"], 0);
                        kQuaInfo.SoToChuc = Utils.ConvertToNullableInt32(dr["SoCaNhan"], 0);
                        kQuaInfo.SoTienToChucTraLai = Utils.ConvertToDecimal(dr["SoTienToChucTraLai"], 0);
                        kQuaInfo.SoDatToChucTraLai = Utils.ConvertToDecimal(dr["SoDatToChucTraLai"], 0);
                        kQuaInfo.SoNguoiBiKienNghiXuLy = Utils.ConvertToNullableInt32(dr["SoNguoiBiKienNghiXuLy"], 0);
                        kQuaInfo.SoCanBoBiXuLy = Utils.ConvertToNullableInt32(dr["SoCanBoBiXuLy"], 0);
                        kQuaInfo.SoNguoiChuyenCoQuanDieuTra = Utils.ConvertToNullableInt32(dr["SoNguoiChuyenCoQuanDieuTra"], 0);
                        kQuaInfo.SoCanBoChuyenCoQuanDieuTra = Utils.ConvertToNullableInt32(dr["SoCanBoChuyenCoQuanDieuTra"], 0);

                        kQuaInfo.NoiDungID = Utils.ConvertToInt32(dr["NoiDungID"], 0);
                        //kQuaInfo.LoaiKetQuaID = Utils.ConvertToInt32(dr["LoaiKetQua"], 0);
                        kQuaInfo.SoTienD = Utils.GetDecimal(dr["SoTien"], 0);
                        kQuaInfo.SoDatD = Utils.GetDecimal(dr["SoDat"], 0);
                        kQuaInfo.SoNguoiDuocTraQuyenLoi = Utils.ConvertToInt32(dr["SoNguoiDuocTraQuyenLoi"], 0);
                        kQuaInfo.SoDoiTuongBiXuLy = Utils.ConvertToInt32(dr["SoDoiTuongBiXuLy"], 0);
                        kQuaInfo.NoiDungQuyetDinh = Utils.ConvertToString(dr["NoiDungQuyetDinh"], string.Empty);
                        // fix bug thời hạn thi hành 4/7/2024
                        kQuaInfo.ThoiHanThiHanh = dr["ThoiHanThiHanh"] != DBNull.Value ? Convert.ToDateTime(dr["ThoiHanThiHanh"]) : (DateTime?)null;
                        if (kQuaInfo.ThoiHanThiHanh != null)
                        {
                            kQuaInfo.ThoiHanThiHanhStr = kQuaInfo.ThoiHanThiHanh.Value.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            kQuaInfo.ThoiHanThiHanhStr = null;
                        }

                        kQuaInfo.CoQuanThiHanh = Utils.ConvertToInt32(dr["CoQuanThiHanh"], 0);
                        kQuaInfo.VaiTro = Utils.ConvertToInt32(dr["VaiTro"], 0);
                        kQuaInfo.CoQuanThiHanhQuyetDinhID = Utils.ConvertToInt32(dr["CoQuanThiHanhQuyetDinhID"], 0);

                        tmp.Add(kQuaInfo);
                    }
                    dr.Close();
                }
                if (tmp.Count > 0)
                {
                    result = (from m in tmp
                              group m by new { m.XuLyDonID, m.KetQuaID } into kq
                              select new QuyetDinhInfo()
                              {
                                  KetQuaID = kq.Key.KetQuaID,
                                  XuLyDonID = kq.Key.XuLyDonID,
                                  SoQuyetDinh = tmp.FirstOrDefault(x => x.KetQuaID == kq.Key.KetQuaID).SoQuyetDinh,
                                  NgayQuyetDinh = tmp.FirstOrDefault(x => x.KetQuaID == kq.Key.KetQuaID).NgayRaKQ,
                                  NgayQuyetDinhStr = tmp.FirstOrDefault(x => x.KetQuaID == kq.Key.KetQuaID).NgayRaKQ.Value.ToString("dd/MM/yyyy"),
                                  CoQuanBanHanh = tmp.FirstOrDefault(x => x.KetQuaID == kq.Key.KetQuaID).CoQuanID,
                                  TenCoQuanBanHanh = tmp.FirstOrDefault(x => x.KetQuaID == kq.Key.KetQuaID).TenCoQuanBanHanh,
                                  TenCoQuan = tmp.FirstOrDefault(x => x.KetQuaID == kq.Key.KetQuaID).TenCoQuan,
                                  TenCanBo = tmp.FirstOrDefault(x => x.KetQuaID == kq.Key.KetQuaID).TenCanBo,
                                  CanBoID = tmp.FirstOrDefault(x => x.KetQuaID == kq.Key.KetQuaID).CanBoID,
                                  QuyetDinh = tmp.FirstOrDefault(x => x.KetQuaID == kq.Key.KetQuaID).QuyetDinh,
                                  PhanTichKetQua = tmp.FirstOrDefault(x => x.KetQuaID == kq.Key.KetQuaID).PhanTichKQ,
                                  KetQuaGiaiQuyetLan2 = tmp.FirstOrDefault(x => x.KetQuaID == kq.Key.KetQuaID).KetQuaGQLan2,
                                  LoaiKhieuToID = tmp.FirstOrDefault(x => x.KetQuaID == kq.Key.KetQuaID).LoaiKhieuTo1ID,

                                  ThoiHanThiHanh = tmp.FirstOrDefault(x => x.KetQuaID == kq.Key.KetQuaID).ThoiHanThiHanh,
                                  TomTatNoiDungGQ = tmp.FirstOrDefault(x => x.KetQuaID == kq.Key.KetQuaID).TomTatNoiDungGQ,
                                  LoaiKetQuaID = tmp.FirstOrDefault(x => x.KetQuaID == kq.Key.KetQuaID).LoaiKetQuaID,
                                  SoTienThuHoi = tmp.FirstOrDefault(x => x.KetQuaID == kq.Key.KetQuaID).SoTienThuHoi,
                                  SoDatThuHoi = tmp.FirstOrDefault(x => x.KetQuaID == kq.Key.KetQuaID).SoDatThuHoi,
                                  SoCaNhan = tmp.FirstOrDefault(x => x.KetQuaID == kq.Key.KetQuaID).SoCaNhan,
                                  SoTienCaNhanTraLai = tmp.FirstOrDefault(x => x.KetQuaID == kq.Key.KetQuaID).SoTienCaNhanTraLai,
                                  SoDatCaNhanTraLai = tmp.FirstOrDefault(x => x.KetQuaID == kq.Key.KetQuaID).SoDatCaNhanTraLai,
                                  SoToChuc = tmp.FirstOrDefault(x => x.KetQuaID == kq.Key.KetQuaID).SoToChuc,
                                  SoTienToChucTraLai = tmp.FirstOrDefault(x => x.KetQuaID == kq.Key.KetQuaID).SoTienToChucTraLai,
                                  SoDatToChucTraLai = tmp.FirstOrDefault(x => x.KetQuaID == kq.Key.KetQuaID).SoDatToChucTraLai,
                                  SoNguoiBiKienNghiXuLy = tmp.FirstOrDefault(x => x.KetQuaID == kq.Key.KetQuaID).SoNguoiBiKienNghiXuLy,
                                  SoCanBoBiXuLy = tmp.FirstOrDefault(x => x.KetQuaID == kq.Key.KetQuaID).SoCanBoBiXuLy,
                                  SoNguoiChuyenCoQuanDieuTra = tmp.FirstOrDefault(x => x.KetQuaID == kq.Key.KetQuaID).SoNguoiChuyenCoQuanDieuTra,
                                  SoCanBoChuyenCoQuanDieuTra = tmp.FirstOrDefault(x => x.KetQuaID == kq.Key.KetQuaID).SoCanBoChuyenCoQuanDieuTra,
                                  ListNoiDungQuyetDinh = (from x in tmp
                                                          where x.NoiDungID > 0
                                                          group x by x.NoiDungID into nd
                                                          select new NoiDungQuyetDinhInfo()
                                                          {
                                                              NoiDungID = nd.Key.Value,
                                                              LoaiKetQuaID = tmp.FirstOrDefault(y => y.NoiDungID == nd.Key).LoaiKetQuaID,
                                                              SoTien = tmp.FirstOrDefault(y => y.NoiDungID == nd.Key).SoTienD,
                                                              SoDat = tmp.FirstOrDefault(y => y.NoiDungID == nd.Key).SoDatD,
                                                              SoNguoiDuocTraQuyenLoi = tmp.FirstOrDefault(y => y.NoiDungID == nd.Key).SoNguoiDuocTraQuyenLoi,
                                                              SoDoiTuongBiXuLy = tmp.FirstOrDefault(y => y.NoiDungID == nd.Key).SoDoiTuongBiXuLy,
                                                              NoiDungQuyetDinh = tmp.FirstOrDefault(y => y.NoiDungID == nd.Key).NoiDungQuyetDinh ?? null,
                                                              ThoiHanThiHanh = tmp.FirstOrDefault(y => y.NoiDungID == nd.Key).ThoiHanThiHanh ?? null,
                                                              ThoiHanThiHanhStr = tmp.FirstOrDefault(y => y.NoiDungID == nd.Key).ThoiHanThiHanh != null ? tmp.FirstOrDefault(y => y.NoiDungID == nd.Key).ThoiHanThiHanh.Value.ToString("dd/MM/yyyy") : null,
                                                              ListCoQuanThiHanh = (from n in tmp
                                                                                   where n.CoQuanThiHanhQuyetDinhID > 0 && n.NoiDungID == nd.Key
                                                                                   group n by n.CoQuanThiHanhQuyetDinhID into cqth
                                                                                   select new CoQuanThiHanhInfo()
                                                                                   {
                                                                                       CoQuanID = tmp.FirstOrDefault(y => y.CoQuanThiHanhQuyetDinhID == cqth.Key).CoQuanThiHanh ?? null,
                                                                                       VaiTro = tmp.FirstOrDefault(y => y.CoQuanThiHanhQuyetDinhID == cqth.Key).VaiTro ?? null,
                                                                                   }).ToList()

                                                          }).ToList(),
                              }
                            ).FirstOrDefault();
                    result.ListFileQuyetDinh = GetFileHoSoByKetQuaID(result.KetQuaID, (int)EnumLoaiFile.FileBanHanhQD);

                    var fileDinhKem = result.ListFileQuyetDinh;
                    if (fileDinhKem.Count > 0)
                    {
                        result.DanhSachHoSoTaiLieu = new List<DanhSachHoSoTaiLieu>();
                        result.DanhSachHoSoTaiLieu = fileDinhKem.GroupBy(p => p.GroupUID)
                           .Select(g => new DanhSachHoSoTaiLieu
                           {
                               GroupUID = g.Key,
                               TenFile = g.FirstOrDefault().TenFile,
                               NoiDung = g.FirstOrDefault().TomTat,
                               TenNguoiCapNhat = g.FirstOrDefault().TenCanBo,
                               NguoiCapNhatID = g.FirstOrDefault().NguoiUp,
                               NgayCapNhat = g.FirstOrDefault().NgayUp,
                               FileDinhKem = fileDinhKem.Where(x => x.GroupUID == g.Key && x.FileHoSoID > 0).GroupBy(x => x.FileHoSoID)
                                               .Select(y => new FileDinhKemModel
                                               {
                                                   FileID = y.FirstOrDefault().FileHoSoID,
                                                   TenFile = y.FirstOrDefault().TenFile,
                                                   NgayCapNhat = y.FirstOrDefault().NgayUp,
                                                   NguoiCapNhat = y.FirstOrDefault().NguoiUp,
                                                   //FileType = y.FirstOrDefault().FileType,
                                                   FileUrl = y.FirstOrDefault().FileURL,
                                               }
                                               ).ToList(),

                           }
                           ).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                return result;
                throw ex;
            }
            return result;
        }


        /// <summary>
        /// thêm file đính kèm của quyết định,
        /// thêm cả logfile
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int QuyetDinh_Them_File(FileHoSoInfo info)
        {
            int val;
            SqlParameter[] parameters = GetInsertFileKQParms();
            SetInsertFileKQParms(parameters, info);
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = Utils.ConvertToInt32(SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, "FileKetQua_Insert_New", parameters), 0);
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
                conn.Close();
                if (val > 0)
                {
                    FileLogInfo infoFileLog = new FileLogInfo();
                    infoFileLog.LoaiLog = (int)EnumLoaiLog.Them;
                    infoFileLog.LoaiFile = (int)EnumLoaiFile.FileBanHanhQD;
                    infoFileLog.IsBaoMat = false;
                    infoFileLog.IsMaHoa = false;
                    infoFileLog.FileID = val;
                    new FileLogDAL().Insert(infoFileLog);
                }
            }
            return val;
        }

        /// <summary>
        /// xoá toàn bộ file của quyết định
        /// </summary>
        /// <param name="KetQuaID"></param>
        /// <returns></returns>
        public int QuyetDinh_Xoa_File(int KetQuaID)
        {

            int val = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("KetQuaID",SqlDbType.Int),
            };
            parameters[0].Value = KetQuaID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "FileKetQua_Delete", parameters);
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

        /// <summary>
        /// xoá toàn bộ nội dung của quyết định, xoá cả cơ quan thi hành
        /// </summary>
        /// <param name="KetQuaID"></param>
        /// <returns></returns>
        public int QuyetDinh_Xoa_NoiDung(int KetQuaID)
        {

            int val = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("KetQuaID",SqlDbType.Int),
            };
            parameters[0].Value = KetQuaID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "QuyetDinh_Delete_NoiDung", parameters);
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


        /// <summary>
        /// cập nhật thông tin của quyết định
        /// </summary>
        /// <param name="cInfo"></param>
        /// <returns></returns>
        public int QuyetDinh_CapNhat_ThongTin(QuyetDinhInfo cInfo)
        {
            int val = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(@"KetQuaID",SqlDbType.Int),
                new SqlParameter(@"SoQuyetDinh",SqlDbType.NVarChar),
                new SqlParameter("NgayRaKQ",SqlDbType.DateTime),
                 new SqlParameter("CoQuanID",SqlDbType.Int),
                new SqlParameter("QuyetDinh",SqlDbType.Int),
                new SqlParameter("PhanTichKQ",SqlDbType.Int),
                new SqlParameter("KetQuaGQLan2",SqlDbType.Int),
                new SqlParameter("XuLyDonID",SqlDbType.Int),
                new SqlParameter("CanBoID",SqlDbType.Int),
                new SqlParameter("ThoiHanThiHanh",SqlDbType.DateTime),
                new SqlParameter("TomTatNoiDungGQ",SqlDbType.NVarChar),
                new SqlParameter("LoaiKetQuaID",SqlDbType.Int),
                new SqlParameter("SoTienThuHoi",SqlDbType.Decimal),
                new SqlParameter("SoDatThuHoi",SqlDbType.Decimal),
                new SqlParameter("SoCaNhan",SqlDbType.Int),
                new SqlParameter("SoTienCaNhanTraLai",SqlDbType.Decimal),
                new SqlParameter("SoDatCaNhanTraLai",SqlDbType.Decimal),
                new SqlParameter("SoToChuc",SqlDbType.Int),
                new SqlParameter("SoTienToChucTraLai",SqlDbType.Decimal),
                new SqlParameter("SoDatToChucTraLai",SqlDbType.Decimal),
                new SqlParameter("SoNguoiBiKienNghiXuLy",SqlDbType.Int),
                new SqlParameter("SoCanBoBiXuLy",SqlDbType.Int),
                new SqlParameter("SoNguoiChuyenCoQuanDieuTra",SqlDbType.Int),
                new SqlParameter("SoCanBoChuyenCoQuanDieuTra",SqlDbType.Int),
            };

            parameters[0].Value = cInfo.KetQuaID;
            parameters[1].Value = cInfo.SoQuyetDinh ?? Convert.DBNull;
            parameters[2].Value = cInfo.NgayQuyetDinh ?? Convert.DBNull;
            parameters[3].Value = cInfo.CoQuanBanHanh ?? Convert.DBNull;
            parameters[4].Value = cInfo.QuyetDinh ?? Convert.DBNull;
            parameters[5].Value = cInfo.PhanTichKetQua ?? Convert.DBNull;
            parameters[6].Value = cInfo.KetQuaGiaiQuyetLan2 ?? Convert.DBNull;
            parameters[7].Value = cInfo.XuLyDonID ?? Convert.DBNull;
            parameters[8].Value = cInfo.CanBoID ?? Convert.DBNull;
            parameters[9].Value = cInfo.ThoiHanThiHanh ?? Convert.DBNull;
            parameters[10].Value = cInfo.TomTatNoiDungGQ ?? Convert.DBNull;
            parameters[11].Value = cInfo.LoaiKetQuaID ?? Convert.DBNull;
            parameters[12].Value = cInfo.SoTienThuHoi ?? Convert.DBNull;
            parameters[13].Value = cInfo.SoDatThuHoi ?? Convert.DBNull;
            parameters[14].Value = cInfo.SoCaNhan ?? Convert.DBNull;
            parameters[15].Value = cInfo.SoTienCaNhanTraLai ?? Convert.DBNull;
            parameters[16].Value = cInfo.SoDatCaNhanTraLai ?? Convert.DBNull;
            parameters[17].Value = cInfo.SoToChuc ?? Convert.DBNull;
            parameters[18].Value = cInfo.SoTienToChucTraLai ?? Convert.DBNull;
            parameters[19].Value = cInfo.SoDatToChucTraLai ?? Convert.DBNull;
            parameters[20].Value = cInfo.SoNguoiBiKienNghiXuLy ?? Convert.DBNull;
            parameters[21].Value = cInfo.SoCanBoBiXuLy ?? Convert.DBNull;
            parameters[22].Value = cInfo.SoNguoiChuyenCoQuanDieuTra ?? Convert.DBNull;
            parameters[23].Value = cInfo.SoCanBoChuyenCoQuanDieuTra ?? Convert.DBNull;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "v2_QuyetDinh_Update_ThongTin", parameters);
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

        /// <summary>
        /// cập nhật quyết định full: Thông tin, file, nội dung, cơ quan thi hành
        /// </summary>
        /// <param name="cInfo"></param>
        /// <returns></returns>
        public int QuyetDinh_CapNhat(QuyetDinhInfo cInfo)
        {
            var val = 0;
            try
            {
                // Cập nhật thông tin quyết định
                val = QuyetDinh_CapNhat_ThongTin(cInfo);
                if (val > 0)
                {
                    // xoá file đính kèm
                    var query = QuyetDinh_Xoa_File(cInfo.KetQuaID);
                    // thêm file
                    if (cInfo.ListFileQuyetDinh != null && cInfo.ListFileQuyetDinh.Count > 0)
                    {
                        foreach (var item in cInfo.ListFileQuyetDinh)
                        {
                            item.KetQuaID = cInfo.KetQuaID;
                            query = QuyetDinh_Them_File(item);
                            if (query < 1)
                            {
                                return query;
                            }
                        }
                    }
                    // xoá nội dung quyết định
                    query = QuyetDinh_Xoa_NoiDung(cInfo.KetQuaID);
                    // thêm mới nội dung quyết định
                    if (cInfo.ListNoiDungQuyetDinh != null && cInfo.ListNoiDungQuyetDinh.Count > 0)
                    {
                        foreach (var item in cInfo.ListNoiDungQuyetDinh)
                        {
                            item.KetQuaID = cInfo.KetQuaID;
                            query = QuyetDinh_ThemMoi_NoiDung(item);
                            if (query < 1)
                            {
                                return query;
                            }
                        }
                    }
                }
                return val;
            }
            catch (Exception)
            {
                return -1;
                throw;
            }
        }


        ///////////////////////// Thi hành quyết định

        /// <summary>
        /// lấy danh sách đơn thư cần thực hiện thi hành theo đơn vị,
        /// (đơn vị trong bảng CoQuanThiHanh_NoiDungQuyetDinh), 
        /// cố bộ lọc, phân trang
        /// </summary>
        /// <param name="info"></param>
        /// <param name="TotalRow"></param>
        /// <returns></returns>
        public IList<DonThuThiHanhInfo> QuyetDinh_Get_DanhSachDonThuCanThiHanh(QueryFilterInfo info, ref int TotalRow)
        {
            IList<DonThuThiHanhInfo> dsDonThu = new List<DonThuThiHanhInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("CoQuanID",SqlDbType.Int),
                new SqlParameter("Keyword",SqlDbType.NVarChar,50),
                new SqlParameter("LoaiKhieuToID",SqlDbType.Int),
                new SqlParameter("TuNgay",SqlDbType.DateTime),
                new SqlParameter("DenNgay",SqlDbType.DateTime),
                new SqlParameter("Start",SqlDbType.Int),
                new SqlParameter("End",SqlDbType.Int),
                new SqlParameter("TrangThai",SqlDbType.Int),
                new SqlParameter("TotalRow",SqlDbType.Int),

            };
            parameters[0].Value = info.CoQuanID;
            parameters[1].Value = info.KeyWord ?? Convert.DBNull;
            parameters[2].Value = info.LoaiKhieuToID;
            parameters[3].Value = info.TuNgay;
            parameters[4].Value = info.DenNgay;
            parameters[5].Value = info.Start;
            parameters[6].Value = info.End;
            parameters[7].Value = info.TrangThai ?? Convert.DBNull;
            parameters[8].Direction = ParameterDirection.Output;
            parameters[8].Size = 8;


            if ((info.TuNgay) == DateTime.MinValue)
            {
                parameters[3].Value = DBNull.Value;
            }

            if ((info.DenNgay) == DateTime.MinValue)
            {
                parameters[4].Value = DBNull.Value;
            }

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_QuyetDinh_DanhSachDonThuCanThiHanh", parameters))
                {
                    while (dr.Read())
                    {
                        var dt = new DonThuThiHanhInfo();
                        dt.KetQuaID = Utils.ConvertToInt32(dr["KetQuaID"], 0);
                        dt.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"].ToString(), 0);
                        dt.LoaiKhieuTo1ID = Utils.ConvertToInt32(dr["LoaiKhieuTo1ID"].ToString(), 0);
                        dt.CoQuanThiHanh = Utils.ConvertToInt32(dr["CoQuanThiHanh"].ToString(), 0);
                        dt.SoDonThu = Utils.ConvertToString(dr["SoDonThu"], string.Empty);
                        dt.TenChuDon = Utils.ConvertToString(dr["TenChuDon"].ToString(), String.Empty);
                        dt.NoiDungDon = Utils.ConvertToString(dr["NoiDungDon"].ToString(), String.Empty);
                        dt.ThoiHanThiHanh = Utils.ConvertToDateTime(dr["ThoiHanThiHanh"].ToString(), DateTime.Now);
                        dt.TrangThaiID = Utils.ConvertToInt32(dr["TrangThai"].ToString(), 0);
                        dt.TenTrangThai = Utils.ConvertToString(dr["TenTrangThai"].ToString(), String.Empty);
                        dt.DonThuID = Utils.ConvertToInt32(dr["DonThuID"].ToString(), 0);
                        dt.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"].ToString(), 0);
                        dt.ThiHanhID = Utils.ConvertToInt32(dr["ThiHanhID"].ToString(), 0);
                        if (dt.ThiHanhID == 0) dt.CoQuanThiHanh = 0;
                        dt.NgayThiHanh = Utils.ConvertToNullableDateTime(dr["NgayThiHanh"], null);
                        //dt.NgayTiepNhan = Utils.ConvertToNullableDateTime(dr["NgayNhapDon"], null);
                        //dt.NgayTiepNhan = Utils.ConvertToNullableDateTime(dr["NgayTiepNhan"], null);
                        //if(dt.ThiHanhID > 0)
                        //{
                        //    dt.TrangThaiID = 1;
                        //    dt.TenTrangThai = "Đã thi hành";
                        //}
                        //else
                        //{
                        //    dt.TrangThaiID = 0;
                        //    dt.TenTrangThai = "Chưa thi hành";
                        //}
                        dsDonThu.Add(dt);
                    }
                    dr.Close();
                }
                TotalRow = Utils.ConvertToInt32(parameters[8].Value, 0);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return dsDonThu;
        }

        /// <summary>
        /// lấy chi tiết kết quả thi hành của 1 đơn thư
        /// , bao gồm: thông tin quyết định, nội dung quyết định, nội dung đã thi hành, file thi hành...
        /// </summary>
        /// <param name="XuLyDonID"></param>
        /// <returns></returns>
        public KetQuaThiHanhInfo QuyetDinh_GetChiTietThiHanh_By_XuLyDonID(int XuLyDonID, int? CoQuanID)
        {
            var result = new KetQuaThiHanhInfo();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("XuLyDonID",SqlDbType.Int),
                 new SqlParameter("CoQuanID",SqlDbType.Int)
            };
            parameters[0].Value = XuLyDonID;
            parameters[1].Value = CoQuanID ?? Convert.DBNull;
            try
            {
                var tmp = new List<KetQuaPatialInfo>();
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "QuyetDinh_GetChiTietThiHanh_By_XuLyDonID", parameters))
                {

                    while (dr.Read())
                    {
                        KetQuaPatialInfo kQuaInfo = new KetQuaPatialInfo();

                        kQuaInfo.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
                        kQuaInfo.KetQuaID = Utils.ConvertToInt32(dr["KetQuaID"], 0);

                        kQuaInfo.NoiDungID = Utils.ConvertToInt32(dr["NoiDungID"], 0);
                        kQuaInfo.LoaiKetQuaID = Utils.ConvertToNullableInt32(dr["LoaiKetQua"], null);
                        kQuaInfo.TenLoaiKetQua = Utils.ConvertToString(dr["TenLoaiKetQua"], string.Empty);
                        kQuaInfo.SoTienD = Utils.GetDecimal(dr["SoTien"], 0);
                        kQuaInfo.SoDatD = Utils.GetDecimal(dr["SoDat"], 0);
                        kQuaInfo.SoNguoiDuocTraQuyenLoi = Utils.ConvertToInt32(dr["SoNguoiDuocTraQuyenLoi"], 0);
                        kQuaInfo.SoDoiTuongBiXuLy = Utils.ConvertToInt32(dr["SoDoiTuongBiXuLy"], 0);
                        kQuaInfo.NoiDungQuyetDinh = Utils.ConvertToString(dr["NoiDungQuyetDinh"], string.Empty);
                        kQuaInfo.ThoiHanThiHanh = Utils.ConvertToNullableDateTime(dr["ThoiHanThiHanh"], null);
                        kQuaInfo.TenCoQuan = Utils.ConvertToString(dr["TenCoQuan_NoiDung"], string.Empty);
                        kQuaInfo.CoQuanID = Utils.ConvertToNullableInt32(dr["CoQuanThiHanh"], null);
                        kQuaInfo.CoQuanThiHanhQuyetDinhID = Utils.ConvertToInt32(dr["CoQuanThiHanhQuyetDinhID"], 0);
                        kQuaInfo.VaiTro = Utils.ConvertToInt32(dr["VaiTro"], 0);

                        kQuaInfo.ThiHanhID = Utils.ConvertToNullableInt32(dr["ThiHanhID"], null);
                        kQuaInfo.NgayThiHanh = Utils.ConvertToNullableDateTime(dr["NgayThiHanh"], null);
                        kQuaInfo.CoQuanThiHanh = Utils.ConvertToNullableInt32(dr["CQThiHanh"], null); // cơ quan thực hiện thi hành - trong bảng thi hành
                        kQuaInfo.LoaiThiHanhID = Utils.ConvertToNullableInt32(dr["LoaiThiHanhID"], null);
                        kQuaInfo.SoTienDaThu = Utils.GetDecimal(dr["TienDaThu"], 0);
                        kQuaInfo.SoDatDaThu = Utils.GetDecimal(dr["DatDaThu"], 0);
                        kQuaInfo.SoNguoiDuocTraQuyenLoi_TH = Utils.ConvertToInt32(dr["SoNguoiDuocTraQuyenLoi_TH"], 0);
                        kQuaInfo.SoDoiTuongBiXuLy_TH = Utils.ConvertToInt32(dr["SoDoiTuongBiXuLy_TH"], 0);
                        kQuaInfo.NoiDungThiHanh = Utils.ConvertToString(dr["NoiDungThiHanh"], string.Empty);
                        kQuaInfo.TenCoQuan_ThiHanh = Utils.ConvertToString(dr["TenCoQuan_ThiHanh"], string.Empty);

                        kQuaInfo.FileHoSoID = Utils.ConvertToInt32(dr["FileHoSoID"], 0);
                        kQuaInfo.TenFile = Utils.ConvertToString(dr["TenFile"], string.Empty);
                        kQuaInfo.FileURL = Utils.ConvertToString(dr["FileURL"], string.Empty);
                        kQuaInfo.NgayUp = Utils.ConvertToDateTime(dr["NgayUp"], DateTime.MinValue);


                        tmp.Add(kQuaInfo);
                    }
                    dr.Close();
                }
                if (tmp.Count > 0)
                {
                    result = (from m in tmp
                              group m by new { m.XuLyDonID, m.KetQuaID } into kq
                              select new KetQuaThiHanhInfo()
                              {
                                  KetQuaID = kq.Key.KetQuaID,
                                  XuLyDonID = kq.Key.XuLyDonID,
                                  ListItemKetQuaThiHanh = (from x in tmp
                                                           where x.NoiDungID > 0
                                                           group x by x.NoiDungID into noidung
                                                           select new KetQuaThiHanhItemInfo()
                                                           {
                                                               NoiDungQuyetDinh = new NoiDungQuyetDinhInfo()
                                                               {
                                                                   NoiDungID = noidung.Key.Value,
                                                                   TenLoaiKetQua = tmp.FirstOrDefault(i => i.NoiDungID == noidung.Key.Value).TenLoaiKetQua ?? null,
                                                                   SoTien = tmp.FirstOrDefault(i => i.NoiDungID == noidung.Key.Value).SoTienD ?? null,
                                                                   SoDat = tmp.FirstOrDefault(i => i.NoiDungID == noidung.Key.Value).SoDatD ?? null,
                                                                   SoNguoiDuocTraQuyenLoi = tmp.FirstOrDefault(i => i.NoiDungID == noidung.Key.Value).SoNguoiDuocTraQuyenLoi ?? null,
                                                                   SoDoiTuongBiXuLy = tmp.FirstOrDefault(i => i.NoiDungID == noidung.Key.Value).SoDoiTuongBiXuLy ?? null,
                                                                   NoiDungQuyetDinh = tmp.FirstOrDefault(i => i.NoiDungID == noidung.Key.Value).NoiDungQuyetDinh ?? null,
                                                                   ThoiHanThiHanh = tmp.FirstOrDefault(i => i.NoiDungID == noidung.Key.Value).ThoiHanThiHanh ?? null,
                                                                   ThoiHanThiHanhStr = tmp.FirstOrDefault(i => i.NoiDungID == noidung.Key.Value).ThoiHanThiHanh > DateTime.MinValue ? tmp.FirstOrDefault(i => i.NoiDungID == noidung.Key.Value).ThoiHanThiHanh.Value.ToString("dd/MM/yyyy") : null,
                                                                   ListCoQuanThiHanh = (from y in tmp
                                                                                        where y.NoiDungID == noidung.Key.Value && y.CoQuanID > 0
                                                                                        group y by y.CoQuanThiHanhQuyetDinhID into cq
                                                                                        select new CoQuanThiHanhInfo()
                                                                                        {
                                                                                            CoQuanID = tmp.FirstOrDefault(i => i.NoiDungID == noidung.Key.Value && i.CoQuanThiHanhQuyetDinhID == cq.Key.Value).CoQuanID ?? null,
                                                                                            TenCoQuan = tmp.FirstOrDefault(i => i.NoiDungID == noidung.Key.Value && i.CoQuanThiHanhQuyetDinhID == cq.Key.Value).TenCoQuan ?? null,
                                                                                            VaiTro = tmp.FirstOrDefault(i => i.NoiDungID == noidung.Key.Value && i.CoQuanThiHanhQuyetDinhID == cq.Key.Value).VaiTro ?? null,
                                                                                            TenVaiTro = tmp.FirstOrDefault(i => i.NoiDungID == noidung.Key.Value && i.CoQuanThiHanhQuyetDinhID == cq.Key.Value).VaiTro == 1 ? "Phụ trách"
                                                                                            : tmp.FirstOrDefault(i => i.NoiDungID == noidung.Key.Value && i.CoQuanThiHanhQuyetDinhID == cq.Key.Value).VaiTro == 2 ? "Phối hợp"
                                                                                            : tmp.FirstOrDefault(i => i.NoiDungID == noidung.Key.Value && i.CoQuanThiHanhQuyetDinhID == cq.Key.Value).VaiTro == 3 ? "Theo dõi"
                                                                                            : ""
                                                                                            //TenCoQuan = y.TenCoQuan ?? null,
                                                                                            //VaiTro = y.VaiTro ?? null,
                                                                                            //TenVaiTro = y.VaiTro == 1 ? "Phụ trách" : y.VaiTro == 2 ? "Phối hợp" : y.VaiTro == 3 ? "Theo dõi" : ""
                                                                                        }).ToList(),
                                                                   TenTrangThai = tmp.FirstOrDefault(i => i.NoiDungID == noidung.Key.Value).ThiHanhID > 0 ? "Đã thi hành" : "Chưa thi hành"

                                                               },
                                                               NoiDungThiHanh = new NoiDungThiHanhInfo()
                                                               {
                                                                   ThiHanhID = tmp.FirstOrDefault(i => i.NoiDungID == noidung.Key.Value).ThiHanhID > 0 ? tmp.FirstOrDefault(i => i.NoiDungID == noidung.Key.Value).ThiHanhID : null,
                                                                   NgayThiHanh = tmp.FirstOrDefault(i => i.NoiDungID == noidung.Key.Value).NgayThiHanh > DateTime.MinValue ? tmp.FirstOrDefault(i => i.NoiDungID == noidung.Key.Value).NgayThiHanh : null,
                                                                   NgayThiHanhStr = tmp.FirstOrDefault(i => i.NoiDungID == noidung.Key.Value).NgayThiHanh > DateTime.MinValue ? tmp.FirstOrDefault(i => i.NoiDungID == noidung.Key.Value).NgayThiHanh.Value.ToString("dd/MM/yyyy") : null,
                                                                   CoQuanThiHanh = tmp.FirstOrDefault(i => i.NoiDungID == noidung.Key.Value).CoQuanThiHanh > 0 ? tmp.FirstOrDefault(i => i.NoiDungID == noidung.Key.Value).CoQuanThiHanh : null,
                                                                   LoaiThiHanhID = tmp.FirstOrDefault(i => i.NoiDungID == noidung.Key.Value).LoaiThiHanhID ?? null,
                                                                   SoTienDaThu = tmp.FirstOrDefault(i => i.NoiDungID == noidung.Key.Value).SoTienDaThu ?? null,
                                                                   SoDatDaThu = tmp.FirstOrDefault(i => i.NoiDungID == noidung.Key.Value).SoDatDaThu ?? null,
                                                                   SoNguoiDuocTraQuyenLoi = tmp.FirstOrDefault(i => i.NoiDungID == noidung.Key.Value).SoNguoiDuocTraQuyenLoi_TH ?? null,
                                                                   SoDoiTuongBiXuLy = tmp.FirstOrDefault(i => i.NoiDungID == noidung.Key.Value).SoDoiTuongBiXuLy_TH ?? null,
                                                                   NoiDungThiHanh = tmp.FirstOrDefault(i => i.NoiDungID == noidung.Key.Value).NoiDungThiHanh ?? null,
                                                                   ListFileThiHanh = (from z in tmp
                                                                                      where z.NoiDungID == noidung.Key.Value && z.FileHoSoID != null && z.FileHoSoID > 0
                                                                                      group z by new { z.FileHoSoID, z.TenFile, z.NgayUp, z.FileURL } into file
                                                                                      select new FileHoSoInfo()
                                                                                      {
                                                                                          FileHoSoID = file.Key.FileHoSoID.Value,
                                                                                          TenFile = file.Key.TenFile ?? null,
                                                                                          NgayCapNhat = file.Key.NgayUp ?? null,
                                                                                          FileURL = file.Key.FileURL ?? null,
                                                                                      }).ToList(),

                                                               }
                                                           }).ToList(),
                              }
                            ).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return result;
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// Thực hiện thi hành 1 nội dung của quyết định
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int QuyetDinh_ThemMoi_ThiHanh(NoiDungThiHanhInfo info)
        {
            int val = 0;

            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter("XuLyDonID",SqlDbType.Int),
                new SqlParameter("NoiDungID",SqlDbType.Int),
                new SqlParameter("NgayThiHanh",SqlDbType.DateTime),
                new SqlParameter("CQThiHanh",SqlDbType.Int),
                new SqlParameter("LoaiThiHanhID",SqlDbType.Int),
                new SqlParameter("TienDaThu",SqlDbType.Decimal),
                new SqlParameter("DatDaThu",SqlDbType.Decimal),
                new SqlParameter("SoNguoiDuocTraQuyenLoi",SqlDbType.Int),
                new SqlParameter("SoDoiTuongBiXuLy",SqlDbType.Int),
                new SqlParameter("NoiDungThiHanh",SqlDbType.NVarChar),
                 new SqlParameter("SoTienThuHoi",SqlDbType.Decimal),
                new SqlParameter("SoDatThuHoi",SqlDbType.Decimal),
                new SqlParameter("SoCaNhan",SqlDbType.Int),
                new SqlParameter("SoTienCaNhanTraLai",SqlDbType.Decimal),
                new SqlParameter("SoDatCaNhanTraLai",SqlDbType.Decimal),
                new SqlParameter("SoToChuc",SqlDbType.Int),
                new SqlParameter("SoTienToChucTraLai",SqlDbType.Decimal),
                new SqlParameter("SoDatToChucTraLai",SqlDbType.Decimal),
                new SqlParameter("SoNguoiBiKienNghiXuLy",SqlDbType.Int),
                new SqlParameter("SoCanBoBiXuLy",SqlDbType.Int),
                new SqlParameter("SoNguoiChuyenCoQuanDieuTra",SqlDbType.Int),
                new SqlParameter("SoCanBoChuyenCoQuanDieuTra",SqlDbType.Int),
                new SqlParameter("CanBoID",SqlDbType.Int),
            };

            parms[0].Value = info.XuLyDonID ?? Convert.DBNull;
            parms[1].Value = info.NoiDungID ?? Convert.DBNull;
            parms[2].Value = info.NgayThiHanh ?? Convert.DBNull;
            parms[3].Value = info.CoQuanThiHanh ?? Convert.DBNull;
            parms[4].Value = info.LoaiThiHanhID ?? Convert.DBNull;
            parms[5].Value = info.SoTienDaThu ?? Convert.DBNull;
            parms[6].Value = info.SoDatDaThu ?? Convert.DBNull;
            parms[7].Value = info.SoNguoiDuocTraQuyenLoi ?? Convert.DBNull;
            parms[8].Value = info.SoDoiTuongBiXuLy ?? Convert.DBNull;
            parms[9].Value = info.GhiChu ?? Convert.DBNull;
            parms[10].Value = info.SoTienThuHoi ?? Convert.DBNull;
            parms[11].Value = info.SoDatThuHoi ?? Convert.DBNull;
            parms[12].Value = info.SoCaNhan ?? Convert.DBNull;
            parms[13].Value = info.SoTienCaNhanTraLai ?? Convert.DBNull;
            parms[14].Value = info.SoDatCaNhanTraLai ?? Convert.DBNull;
            parms[15].Value = info.SoToChuc ?? Convert.DBNull;
            parms[16].Value = info.SoTienToChucTraLai ?? Convert.DBNull;
            parms[17].Value = info.SoDatToChucTraLai ?? Convert.DBNull;
            parms[18].Value = info.SoNguoiBiKienNghiXuLy ?? Convert.DBNull;
            parms[19].Value = info.SoCanBoBiXuLy ?? Convert.DBNull;
            parms[20].Value = info.SoNguoiChuyenCoQuanDieuTra ?? Convert.DBNull;
            parms[21].Value = info.SoCanBoChuyenCoQuanDieuTra ?? Convert.DBNull;
            parms[22].Value = info.CanBoID ?? Convert.DBNull;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = Utils.ConvertToInt32(SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, "v2_QuyetDinh_Insert_ThiHanh_New", parms), 0);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw ex;
                        throw;
                    }
                }
                conn.Close();
                if (val > 0 && info.ListFileThiHanh != null && info.ListFileThiHanh.Count > 0)
                {
                    foreach (var item in info.ListFileThiHanh)
                    {
                        FileHoSoInfo infoFileHoSo = new FileHoSoInfo();
                        infoFileHoSo.ThiHanhID = val;
                        infoFileHoSo.FileURL = item.FileURL;
                        infoFileHoSo.NgayUp = item.NgayUp;
                        infoFileHoSo.TenFile = item.TenFile;
                        infoFileHoSo.TomTat = item.TomTat;
                        infoFileHoSo.NguoiUp = item.NguoiUp;

                        FileLogInfo infoFileLog = new FileLogInfo();
                        infoFileLog.LoaiLog = (int)EnumLoaiLog.Them;
                        infoFileLog.LoaiFile = (int)EnumLoaiFile.FileThiHanh;
                        infoFileLog.IsBaoMat = item.IsBaoMat;
                        try
                        {
                            int resultFile = new ThiHanhDAL().InsertFileThiHanh(infoFileHoSo);
                            if (resultFile > 0)
                            {
                                infoFileLog.FileID = resultFile;
                                var query = new FileLogDAL().Insert(infoFileLog);
                                if (query < 0)
                                {
                                    return query;
                                }
                            }
                            else return resultFile;
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
            return Convert.ToInt32(val);
        }

        /// <summary>
        /// xoá tất file thi hành
        /// </summary>
        /// <param name="thiHanhID"></param>
        /// <returns></returns>
        public int QuyetDinh_XoaFileThiHanh(int thiHanhID)
        {

            int val = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("ThiHanhID",SqlDbType.Int),
            };
            parameters[0].Value = thiHanhID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "FileThiHanh_Delete", parameters);
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
        /// <summary>
        /// Cập nhật 1 thi hành của 1 nội dung,
        /// bao gồm: nội dung thi hành, file thi hành
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int QuyetDinh_CapNhat_ThiHanh(NoiDungThiHanhInfo info)
        {
            int val = 0;

            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter("ThiHanhID",SqlDbType.Int),
                new SqlParameter("NgayThiHanh",SqlDbType.DateTime),
                new SqlParameter("CQThiHanh",SqlDbType.Int),
                new SqlParameter("LoaiThiHanhID",SqlDbType.Int),
                new SqlParameter("TienDaThu",SqlDbType.Decimal),
                new SqlParameter("DatDaThu",SqlDbType.Decimal),
                new SqlParameter("SoNguoiDuocTraQuyenLoi",SqlDbType.Int),
                new SqlParameter("SoDoiTuongBiXuLy",SqlDbType.Int),
                new SqlParameter("NoiDungThiHanh",SqlDbType.NVarChar),
                  new SqlParameter("SoTienThuHoi",SqlDbType.Decimal),
                new SqlParameter("SoDatThuHoi",SqlDbType.Decimal),
                new SqlParameter("SoCaNhan",SqlDbType.Int),
                new SqlParameter("SoTienCaNhanTraLai",SqlDbType.Decimal),
                new SqlParameter("SoDatCaNhanTraLai",SqlDbType.Decimal),
                new SqlParameter("SoToChuc",SqlDbType.Int),
                new SqlParameter("SoTienToChucTraLai",SqlDbType.Decimal),
                new SqlParameter("SoDatToChucTraLai",SqlDbType.Decimal),
                new SqlParameter("SoNguoiBiKienNghiXuLy",SqlDbType.Int),
                new SqlParameter("SoCanBoBiXuLy",SqlDbType.Int),
                new SqlParameter("SoNguoiChuyenCoQuanDieuTra",SqlDbType.Int),
                new SqlParameter("SoCanBoChuyenCoQuanDieuTra",SqlDbType.Int),
                new SqlParameter("CanBoID",SqlDbType.Int),
            };

            parms[0].Value = info.ThiHanhID ?? Convert.DBNull;
            parms[1].Value = info.NgayThiHanh ?? Convert.DBNull;
            parms[2].Value = info.CoQuanThiHanh ?? Convert.DBNull;
            parms[3].Value = info.LoaiThiHanhID ?? Convert.DBNull;
            parms[4].Value = info.SoTienDaThu ?? Convert.DBNull;
            parms[5].Value = info.SoDatDaThu ?? Convert.DBNull;
            parms[6].Value = info.SoNguoiDuocTraQuyenLoi ?? Convert.DBNull;
            parms[7].Value = info.SoDoiTuongBiXuLy ?? Convert.DBNull;
            parms[8].Value = info.GhiChu ?? Convert.DBNull;
            parms[9].Value = info.SoTienThuHoi ?? Convert.DBNull;
            parms[10].Value = info.SoDatThuHoi ?? Convert.DBNull;
            parms[11].Value = info.SoCaNhan ?? Convert.DBNull;
            parms[12].Value = info.SoTienCaNhanTraLai ?? Convert.DBNull;
            parms[13].Value = info.SoDatCaNhanTraLai ?? Convert.DBNull;
            parms[14].Value = info.SoToChuc ?? Convert.DBNull;
            parms[15].Value = info.SoTienToChucTraLai ?? Convert.DBNull;
            parms[16].Value = info.SoDatToChucTraLai ?? Convert.DBNull;
            parms[17].Value = info.SoNguoiBiKienNghiXuLy ?? Convert.DBNull;
            parms[18].Value = info.SoCanBoBiXuLy ?? Convert.DBNull;
            parms[19].Value = info.SoNguoiChuyenCoQuanDieuTra ?? Convert.DBNull;
            parms[20].Value = info.SoCanBoChuyenCoQuanDieuTra ?? Convert.DBNull;
            parms[21].Value = info.CanBoID ?? Convert.DBNull;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "v2_QuyetDinh_Update_ThiHanh_New", parms);
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
                conn.Close();
                if (val > 0)
                {
                    var query = QuyetDinh_XoaFileThiHanh(info.ThiHanhID.Value);
                    if (query >= 0 && info.ListFileThiHanh != null && info.ListFileThiHanh.Count > 0)
                    {
                        foreach (var item in info.ListFileThiHanh)
                        {
                            FileHoSoInfo infoFileHoSo = new FileHoSoInfo();
                            infoFileHoSo.ThiHanhID = val;
                            infoFileHoSo.FileURL = item.FileURL;
                            infoFileHoSo.NgayUp = item.NgayUp;
                            infoFileHoSo.TenFile = item.TenFile;
                            infoFileHoSo.TomTat = item.TomTat;
                            infoFileHoSo.NguoiUp = item.NguoiUp;

                            FileLogInfo infoFileLog = new FileLogInfo();
                            infoFileLog.LoaiLog = (int)EnumLoaiLog.Them;
                            infoFileLog.LoaiFile = (int)EnumLoaiFile.FileThiHanh;
                            infoFileLog.IsBaoMat = item.IsBaoMat;
                            try
                            {
                                int resultFile = new ThiHanhDAL().InsertFileThiHanh(infoFileHoSo);
                                if (resultFile > 0)
                                {
                                    infoFileLog.FileID = resultFile;
                                    query = new FileLogDAL().Insert(infoFileLog);
                                    if (query < 0)
                                    {
                                        return query;
                                    }
                                }
                                else return resultFile;
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                    }
                }
            }
            return Convert.ToInt32(val);
        }

        /// <summary>
        /// Lấy thông tin trạng thái thi hành của đơn thư
        /// </summary>
        /// <param name="XuLyDonID"></param>
        /// <returns></returns>
        public DonThuThiHanhInfo QuyetDinh_GetTrangThaiThiHanh_By_XuLyDonID(int XuLyDonID)
        {
            var result = new DonThuThiHanhInfo();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("XuLyDonID",SqlDbType.Int)
            };
            parameters[0].Value = XuLyDonID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "QuyetDinh_GetTrangThaiThiHanh_By_XuLyDonID", parameters))
                {
                    while (dr.Read())
                    {
                        result.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
                        result.KetQuaID = Utils.ConvertToInt32(dr["KetQuaID"], 0);
                        result.TrangThaiID = Utils.ConvertToInt32(dr["TrangThai"], 0);
                        result.TenTrangThai = Utils.ConvertToString(dr["TenTrangThai"], string.Empty);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                return result;
                throw ex;
            }
            return result;
        }

        public IList<FileHoSoInfo> GetAllFileQuyetDinh(int xulydonID)
        {
            IList<FileHoSoInfo> ListDT = new List<FileHoSoInfo>();

            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@XuLyDonID", SqlDbType.Int)
            };
            parameters[0].Value = xulydonID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "GetAllFileQuyetDinh", parameters))
                {

                    while (dr.Read())
                    {

                        FileHoSoInfo info = new FileHoSoInfo();
                        info.FileHoSoID = Utils.GetInt32(dr["FileHoSoID"], 0);
                        info.FileURL = Utils.GetString(dr["FileURL"], String.Empty);
                        info.NguoiUp = Utils.GetInt32(dr["NguoiUp"], 0);
                        if (info.NguoiUp > 0)
                        {
                            info.TenCoQuanUp = new CoQuan().GetCoQuanByCanBoID(info.NguoiUp).TenCoQuan;
                        }
                        info.NgayUp = Utils.GetDateTime(dr["NgayUp"], DateTime.MinValue);
                        info.NgayUps = string.Empty;
                        if (info.NgayUp != DateTime.MinValue)
                        {
                            info.NgayUps = info.NgayUp.ToString("dd/MM/yyyy");
                            info.NgayUp_str = Format.FormatDateTime(info.NgayUp);
                        }
                        info.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);
                        info.FileID = Utils.ConvertToInt32(dr["FileID"], 0);
                        info.TenFile = Utils.ConvertToString(dr["TenFileNew"], string.Empty);
                        info.NhomFileID = Utils.ConvertToInt32(dr["NhomFileID"], 0);
                        info.TenNhomFile = Utils.ConvertToString(dr["TenNhomFile"], string.Empty);
                        info.ThuTuHienThiNhom = Utils.ConvertToInt32(dr["ThuTuHienThi"], 0);
                        info.ThuTuHienThiFile = Utils.ConvertToInt32(dr["ThuTuHienThiFile"], 0);
                        info.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        info.IsBaoMat = Utils.ConvertToBoolean(dr["IsBaoMat"], false);
                        info.IsMaHoa = Utils.ConvertToBoolean(dr["IsMaHoa"], false);
                        info.LoaiFile = Utils.ConvertToInt32(dr["LoaiFile"], 0);
                        info.TomTat = "";
                        info.CanBoID = Utils.ConvertToInt32(dr["NguoiUp"], 0);
                        ListDT.Add(info);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ListDT;
        }
        /////////////////////////////////

        public DonThuThiHanhInfo GetThiHanh_By_XuLyDonID(int XuLyDonID)
        {
            var result = new DonThuThiHanhInfo();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("XuLyDonID",SqlDbType.Int)
            };
            parameters[0].Value = XuLyDonID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_GetThiHanh_By_XuLyDonID", parameters))
                {
                    while (dr.Read())
                    {
                        result.ThiHanhID = Utils.ConvertToInt32(dr["ThiHanhID"], 0);
                        result.LoaiKhieuTo1ID = Utils.ConvertToInt32(dr["LoaiKhieuTo1ID"], 0);
                        result.CoQuanThiHanh = Utils.ConvertToInt32(dr["CQThiHanh"], 0);
                        result.NgayThiHanh = Utils.ConvertToNullableDateTime(dr["NgayThiHanh"], null);
                        result.SoTienThuHoi = Utils.ConvertToDecimal(dr["SoTienThuHoi"], 0);
                        result.SoDatThuHoi = Utils.ConvertToDecimal(dr["SoDatThuHoi"], 0);
                        result.SoCaNhan = Utils.ConvertToNullableInt32(dr["SoCaNhan"], 0);
                        result.SoTienCaNhanTraLai = Utils.ConvertToDecimal(dr["SoTienCaNhanTraLai"], 0);
                        result.SoDatCaNhanTraLai = Utils.ConvertToDecimal(dr["SoDatCaNhanTraLai"], 0);
                        result.SoToChuc = Utils.ConvertToNullableInt32(dr["SoCaNhan"], 0);
                        result.SoTienToChucTraLai = Utils.ConvertToDecimal(dr["SoTienToChucTraLai"], 0);
                        result.SoDatToChucTraLai = Utils.ConvertToDecimal(dr["SoDatToChucTraLai"], 0);
                        result.SoNguoiBiKienNghiXuLy = Utils.ConvertToNullableInt32(dr["SoNguoiBiKienNghiXuLy"], 0);
                        result.SoCanBoBiXuLy = Utils.ConvertToNullableInt32(dr["SoCanBoBiXuLy"], 0);
                        result.SoNguoiChuyenCoQuanDieuTra = Utils.ConvertToNullableInt32(dr["SoNguoiChuyenCoQuanDieuTra"], 0);
                        result.SoCanBoChuyenCoQuanDieuTra = Utils.ConvertToNullableInt32(dr["SoCanBoChuyenCoQuanDieuTra"], 0);
                        result.TenCoQuan = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        result.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        result.GhiChu = Utils.ConvertToString(dr["NoiDungThiHanh"], string.Empty);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                return result;
                throw ex;
            }
            return result;
        }
        #endregion
    }
}
