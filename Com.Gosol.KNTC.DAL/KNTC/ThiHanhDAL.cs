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
    public class ThiHanhDAL
    {
        //Su dung de goi StoreProcedure

        private const string GET_ALL = @"ThiHanh_GetAll";
        private const string GET_BY_ID = @"ThiHanh_GetByID";
        private const string INSERT = @"ThiHanh_Insert";
        private const string UPDATE = @"ThiHanh_Update";
        private const string DELETE = @"ThiHanh_Delete";
        private const string GET_BY_PAGE = @"ThiHanh_GetByPage";
        private const string COUNT_ALL = @"ThiHanh_CountAll";
        private const string SEARCH = @"ThiHanh_GetBySearch";
        private const string COUNT_SEARCH = @"ThiHanh_CountSearch";

        private const string GET_THI_HANH_BY_XLDID = @"ThiHanh_GetByXuLyDonID";
        private const string THI_HANH_INSERT = @"Thi_Hanh_INSERT";
        private const string THI_HANH_UPDATE = @"Thi_Hanh_UPDATE";
        private const string THIHANH_KETQUA_GETBYID = @"ThiHanh_KetQua_GetByID";
        private const string GET_BY_TIME = "ThiHanh_GetByTime";
        private const string INSERT_FILE_THIHANH = @"FileThiHanh_Insert";
        private const string INSERT_FILE_THIHANH_NEW = @"FileThiHanh_Insert_New";
        private const string FILETHIHANH_GETBYTHIHANHID = @"FileThiHanh_GetByThiHanhID";
        private const string FILETHIHANH_GETBYTHIHANHID_NEW = @"FileThiHanh_GetByThiHanhID_New";
        private const string FILETHIHANH_DELETE = @"FileThiHanh_Delete";
        //Ten cac bien dau vao
        private const string PARAM_THI_HANH_ID = "@ThiHanhID";
        private const string PARAM_NGAY_THI_HANH = "@NgayThiHanh";
        private const string PARAM_CQ_THI_HANH = "@CQThiHanh";
        private const string PARAM_TIEN_DA_THU = "@TienDaThu";
        private const string PARAM_DAT_DA_THU = "@DatDaThu";
        private const string PARAM_XU_LY_DON_ID = "@XuLyDonID";
        private const string PARAM_KET_QUA_ID = "@KetQuaID";
        private const string PARAM_FILE_DINHKEM = "@FileDinhKem";
        private const string PARAM_KEY = "@Keyword";
        private const string PARAM_START = "@Start";
        private const string PARAM_END = "@End";

        private const string PARAM_COQUAN_TH = @"CoQuanTHID";

        private const string PARAM_TEN_FILE = "@TenFile";
        private const string PARAM_TOMTAT = "@TomTat";
        private const string PARAM_FILE_URL = "@FileUrl";
        private const string PARAM_NGUOIUP = "@NguoiUp";
        private const string PARAM_NGAYUP = "@NgayUp";
        private const string PARAM_LOAIFILE = "@LoaiFile";

        private ThiHanhInfo GetData(SqlDataReader dr)
        {
            ThiHanhInfo cInfo = new ThiHanhInfo();
            cInfo.ThiHanhID = Utils.ConvertToInt32(dr["ThiHanhID"], 0);
            cInfo.NgayThiHanh = Utils.ConvertToDateTime(dr["NgayThiHanh"], Constant.DEFAULT_DATE);
            cInfo.CQThiHanh = Utils.ConvertToString(dr["CQThiHanh"], string.Empty);
            cInfo.TienDaThu = Utils.ConvertToInt32(dr["TienDaThu"], 0);
            cInfo.DatDaThu = Utils.ConvertToInt32(dr["DatDaThu"], 0);
            cInfo.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
            cInfo.KetQuaID = Utils.ConvertToInt32(dr["KetQuaID"], 0);

            return cInfo;
        }

        private SqlParameter[] GetInsertParms()
        {
            SqlParameter[] parms = new SqlParameter[]{

                new SqlParameter(PARAM_NGAY_THI_HANH,SqlDbType.DateTime),
                new SqlParameter(PARAM_CQ_THI_HANH,SqlDbType.NVarChar,200),
                new SqlParameter(PARAM_TIEN_DA_THU,SqlDbType.Int),
                new SqlParameter(PARAM_DAT_DA_THU,SqlDbType.Int),
                new SqlParameter(PARAM_XU_LY_DON_ID,SqlDbType.Int),
                new SqlParameter(PARAM_KET_QUA_ID,SqlDbType.Int)
            };
            return parms;
        }

        private void SetInsertParms(SqlParameter[] parms, ThiHanhInfo cInfo)
        {
            parms[0].Value = cInfo.NgayThiHanh;
            parms[1].Value = cInfo.CQThiHanh;
            parms[2].Value = cInfo.TienDaThu;
            parms[3].Value = cInfo.DatDaThu;
            parms[4].Value = cInfo.XuLyDonID;
            parms[5].Value = cInfo.KetQuaID;
        }

        private SqlParameter[] GetUpdateParms()
        {
            SqlParameter[] parms = new SqlParameter[]{

                new SqlParameter(PARAM_THI_HANH_ID,SqlDbType.Int),
                new SqlParameter(PARAM_NGAY_THI_HANH,SqlDbType.DateTime),
                new SqlParameter(PARAM_CQ_THI_HANH,SqlDbType.NVarChar,200),
                new SqlParameter(PARAM_TIEN_DA_THU,SqlDbType.Int),
                new SqlParameter(PARAM_DAT_DA_THU,SqlDbType.Int)
            };
            return parms;
        }

        private void SetUpdateParms(SqlParameter[] parms, ThiHanhInfo cInfo)
        {

            parms[0].Value = cInfo.ThiHanhID;
            parms[1].Value = cInfo.NgayThiHanh;
            parms[2].Value = cInfo.CQThiHanh;
            parms[3].Value = cInfo.TienDaThu;
            parms[4].Value = cInfo.DatDaThu;
        }

        public IList<ThiHanhInfo> GetAll()
        {
            IList<ThiHanhInfo> ketquas = new List<ThiHanhInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_ALL, null))
                {

                    while (dr.Read())
                    {

                        ThiHanhInfo cInfo = GetData(dr);
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

        public IList<ThiHanhInfo> GetByTime(DateTime startDate, DateTime endDate)
        {
            IList<ThiHanhInfo> thiHanhList = new List<ThiHanhInfo>();
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
                        ThiHanhInfo cInfo = GetData(dr);
                        thiHanhList.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return thiHanhList;
        }

        public ThiHanhInfo ThiHanh_KetQua_GetByID(int cID)
        {
            ThiHanhInfo cInfo = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XU_LY_DON_ID,SqlDbType.Int)

            };
            parameters[0].Value = cID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, THIHANH_KETQUA_GETBYID, parameters))
                {

                    if (dr.Read())
                    {
                        cInfo = Get_DataThiHanhJoin(dr);
                        cInfo.lstFileTH = new List<FileHoSoInfo>();
                        cInfo.lstFileTH = GetFileThiHanhByThiHanhID(cInfo.ThiHanhID, (int)EnumLoaiFile.FileThiHanh);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return cInfo;
        }

        public ThiHanhInfo GetByID(int cID)
        {

            ThiHanhInfo cInfo = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_THI_HANH_ID,SqlDbType.Int)
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

        public int Update(ThiHanhInfo cInfo)
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

        public int Insert(ThiHanhInfo cInfo)
        {

            int val = 0;

            SqlParameter[] parameters = GetInsertParms();
            SetInsertParms(parameters, cInfo);

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, INSERT, parameters);
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

        public IList<ThiHanhInfo> GetBySearch(string keyword, int page)
        {
            int start = (page - 1) * Constant.PageSize;
            int end = page * Constant.PageSize;

            IList<ThiHanhInfo> ketquas = new List<ThiHanhInfo>();
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
                        ThiHanhInfo cInfo = GetData(dr);
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

        public IList<ThiHanhInfo> GetByPage(int page)
        {
            int start = (page - 1) * Constant.PageSize;
            int end = page * Constant.PageSize;

            IList<ThiHanhInfo> ketquas = new List<ThiHanhInfo>();
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
                        ThiHanhInfo cInfo = GetData(dr);
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

        //
        private ThiHanhInfo Get_DataThiHanhJoin(SqlDataReader dr)
        {
            ThiHanhInfo cInfo = new ThiHanhInfo();
            cInfo.ThiHanhID = Utils.ConvertToInt32(dr["ThiHanhID"], 0);
            cInfo.NgayThiHanh = Utils.ConvertToDateTime(dr["NgayThiHanh"], Constant.DEFAULT_DATE);
            cInfo.CoQuanTHID = Utils.ConvertToInt32(dr["CQThiHanh"], 0);
            cInfo.TenCoQuanThiHanh = Utils.GetString(dr["TenCoQuanThiHanh"], string.Empty);
            cInfo.TienDaThu = Utils.ConvertToInt32(dr["TienDaThu"], 0);
            cInfo.DatDaThu = Utils.ConvertToInt32(dr["DatDaThu"], 0);
            cInfo.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
            cInfo.KetQuaID = Utils.ConvertToInt32(dr["KetQuaID"], 0);
            cInfo.TenLoaiKetQua = Utils.GetString(dr["TenLoaiKetQua"], string.Empty);
            cInfo.FileKetQua = Utils.GetString(dr["FileDinhKem"], string.Empty);

            return cInfo;
        }

        private ThiHanhInfo GetDataThiHanh(SqlDataReader dr)
        {
            ThiHanhInfo cInfo = new ThiHanhInfo();
            cInfo.ThiHanhID = Utils.ConvertToInt32(dr["ThiHanhID"], 0);
            cInfo.NgayThiHanh = Utils.ConvertToDateTime(dr["NgayThiHanh"], Constant.DEFAULT_DATE);
            cInfo.CoQuanTHID = Utils.ConvertToInt32(dr["CQThiHanh"], 0);
            cInfo.TienDaThu = Utils.ConvertToInt32(dr["TienDaThu"], 0);
            cInfo.DatDaThu = Utils.ConvertToInt32(dr["DatDaThu"], 0);
            cInfo.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
            cInfo.KetQuaID = Utils.ConvertToInt32(dr["KetQuaID"], 0);

            return cInfo;
        }

        public ThiHanhInfo GetThiHanhBy_XLDID(int cID)
        {
            ThiHanhInfo cInfo = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XU_LY_DON_ID,SqlDbType.Int)

            };
            parameters[0].Value = cID;


            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_THI_HANH_BY_XLDID, parameters))
                {

                    if (dr.Read())
                    {
                        cInfo = GetDataThiHanh(dr);
                        cInfo.FileDinhKem = Utils.ConvertToString(dr["FileDinhKem"], string.Empty);

                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return cInfo;
        }

        public int ThiHanh_Insert(ThiHanhInfo info)
        {
            object val = 0;

            SqlParameter[] parms = new SqlParameter[]{

                new SqlParameter(PARAM_NGAY_THI_HANH,SqlDbType.DateTime),
                new SqlParameter(PARAM_COQUAN_TH,SqlDbType.Int),
                new SqlParameter(PARAM_TIEN_DA_THU,SqlDbType.Int),
                new SqlParameter(PARAM_DAT_DA_THU,SqlDbType.Int),
                new SqlParameter(PARAM_XU_LY_DON_ID,SqlDbType.Int),
                new SqlParameter(PARAM_KET_QUA_ID,SqlDbType.Int),
                new SqlParameter(PARAM_FILE_DINHKEM,SqlDbType.NVarChar)
            };

            parms[0].Value = info.NgayThiHanh;
            parms[1].Value = info.CoQuanTHID;
            parms[2].Value = info.TienDaThu;
            parms[3].Value = info.DatDaThu;
            parms[4].Value = info.XuLyDonID;
            parms[5].Value = info.KetQuaID;
            parms[6].Value = info.FileDinhKem;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, THI_HANH_INSERT, parms);
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

        public int ThiHanh_Update(ThiHanhInfo info)
        {
            int val = 0;

            SqlParameter[] parms = new SqlParameter[]{

                new SqlParameter(PARAM_THI_HANH_ID,SqlDbType.Int),
                new SqlParameter(PARAM_NGAY_THI_HANH,SqlDbType.DateTime),
                new SqlParameter(PARAM_COQUAN_TH,SqlDbType.Int),
                new SqlParameter(PARAM_TIEN_DA_THU,SqlDbType.Int),
                new SqlParameter(PARAM_DAT_DA_THU,SqlDbType.Int),
                new SqlParameter(PARAM_FILE_DINHKEM,SqlDbType.NVarChar)
            };

            parms[0].Value = info.ThiHanhID;
            parms[1].Value = info.NgayThiHanh;
            parms[2].Value = info.CoQuanTHID;
            parms[3].Value = info.TienDaThu;
            parms[4].Value = info.DatDaThu;
            parms[5].Value = info.FileDinhKem;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, THI_HANH_UPDATE, parms);
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

        private SqlParameter[] GetInsertFileThiHanhParms()
        {
            SqlParameter[] parms = new SqlParameter[]{
                //new SqlParameter(PARAM_FileHoSo_ID, SqlDbType.Int),
                new SqlParameter(PARAM_TEN_FILE, SqlDbType.NVarChar),
                new SqlParameter(PARAM_TOMTAT, SqlDbType.NVarChar),
                new SqlParameter(PARAM_NGAYUP, SqlDbType.DateTime),
                new SqlParameter(PARAM_NGUOIUP, SqlDbType.Int),
                new SqlParameter(PARAM_FILE_URL, SqlDbType.NVarChar),
                new SqlParameter(PARAM_THI_HANH_ID, SqlDbType.Int),
                new SqlParameter("@FileID", SqlDbType.Int)
                };
            return parms;
        }

        private void SetInsertFileThiHanhParms(SqlParameter[] parms, FileHoSoInfo info)
        {

            //parms[0].Value = info.SoFileHoSo;
            parms[0].Value = info.TenFile;
            parms[1].Value = info.TomTat;
            parms[2].Value = info.NgayUp;
            parms[3].Value = info.NguoiUp;
            parms[4].Value = info.FileURL;
            parms[5].Value = info.ThiHanhID;
            parms[6].Value = info.FileID;

        }
        public int InsertFileThiHanh(FileHoSoInfo info)
        {

            object val;

            SqlParameter[] parameters = GetInsertFileThiHanhParms();
            SetInsertFileThiHanhParms(parameters, info);

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, INSERT_FILE_THIHANH_NEW, parameters);
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

        public int Delete(int thiHanhID)
        {

            int val = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_THI_HANH_ID,SqlDbType.Int),
            };
            parameters[0].Value = thiHanhID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, FILETHIHANH_DELETE, parameters);
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
        public List<FileHoSoInfo> GetFileThiHanhByThiHanhID(int thiHanhID, int loaiFile)
        {
            List<FileHoSoInfo> lst_HoSo = new List<FileHoSoInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_THI_HANH_ID,SqlDbType.Int),
                new SqlParameter(PARAM_LOAIFILE,SqlDbType.Int)
            };
            parameters[0].Value = thiHanhID;
            parameters[1].Value = loaiFile;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, FILETHIHANH_GETBYTHIHANHID_NEW, parameters))
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
                        cInfo.CanBoID = Utils.ConvertToInt32(dr["NguoiUp"], 0);
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

    }
}
