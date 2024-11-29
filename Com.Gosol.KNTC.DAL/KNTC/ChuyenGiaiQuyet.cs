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
    public class ChuyenGiaiQuyet
    {
        private const string INSERT = @"ChuyenGiaiQuyet_Insert";

        private const string PARAM_XULYDONID = "@XuLyDonID";
        private const string PARAM_NGUOIUP = "@NguoiUp";
        private const string PARAM_LOAIFILE = "@LoaiFile";
        private const string PARAM_COQUANPHANID = "@CoQuanPhanID";
        private const string PARAM_COQUANPHOIHOPID = "@CQPhoiHopID";
        private const string PARAM_COQUANGIAIQUYETID = "@CoQuanGiaiQuyetID";
        private const string PARAM_GHICHU = "@GhiChu";
        private const string PARAM_NGAYCHUYEN = "@NgayChuyen";
        private const string PARAM_FILEURL = "@FileUrl";

        private const string CHECKIS_PHAN_TRONGCOQUAN = @"ChuyenGiaiQuyet_CheckIsPhanTrongCoQuan";
        private const string GETCOQUAN_CHUYENGQ = @"ChuyenGiaiQuyet_getCoQuanChuyenGQDen";
        private const string DELETE_BY_XULYDONID = @"ChuyenGiaiQuyet_DeleteByXuLyDonID";
        private const string DELETE_CHUYENCQKHAC = @"ChuyenGiaiQuyet_DeleteChuyenCQKhac";
        private const string DELETE_CQPHOIHOP = @"ChuyenGiaiQuyet_DeleteCQPhoiHop";
        private const string INSERT_CQPHOIHOP = @"ChuyenGiaiQuyet_InsertCQPhoiHop";
        private const string GET_CHUYENGIAIQUYET_CQ_KHAC = @"ChuyenGiaiQuyet_GetChuyenGiaiQuyetCoQuanKhac";
        private const string GET_CHUYEN_GQ_BY_COQUAN = "ChuyenGiaiQuyet_GetByCoQuan";
        private const string GET_CHUYEN_GQ_JOIN_YKIENGQ = @"ChuyenGiaiQuyet_Join_YKienGQ";
        private const string FILECHUYENGIAIQUYET_GETBYXULYDONID = @"FileChuyenGiaiQuyet_GetByXuLyDonID";
        private const string FILECHUYENGIAIQUYET_DELETEBYXULYDONID = @"FileChuyenGiaiQuyet_DeleteByXuLyDonID";

        private SqlParameter[] GetInsertParms()
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_XULYDONID, SqlDbType.Int),
                new SqlParameter(PARAM_COQUANPHANID, SqlDbType.Int),
                new SqlParameter(PARAM_COQUANGIAIQUYETID, SqlDbType.Int),
                new SqlParameter(PARAM_GHICHU, SqlDbType.NText),
                new SqlParameter(PARAM_NGAYCHUYEN,SqlDbType.DateTime),
                new SqlParameter(PARAM_FILEURL,SqlDbType.NVarChar, 200)
            }; return parms;
        }

        private void SetInsertParms(SqlParameter[] parms, ChuyenGiaiQuyetInfo info)
        {
            parms[0].Value = info.XuLyDonID;
            parms[1].Value = info.CoQuanPhanID;
            parms[2].Value = info.CoQuanGiaiQuyetID;
            parms[3].Value = info.GhiChu ?? Convert.DBNull; ;
            parms[4].Value = info.NgayChuyen;
            parms[5].Value = info.FileUrl ?? Convert.DBNull;
        }
        public int Insert(ChuyenGiaiQuyetInfo info)
        {
            object val = null;

            //SqlParameter[] parameters = GetInsertParms();
            //SetInsertParms(parameters, info);

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_XULYDONID, SqlDbType.Int),
                new SqlParameter(PARAM_COQUANPHANID, SqlDbType.Int),
                new SqlParameter(PARAM_COQUANGIAIQUYETID, SqlDbType.Int),
                new SqlParameter(PARAM_GHICHU, SqlDbType.NText),
                new SqlParameter(PARAM_NGAYCHUYEN,SqlDbType.DateTime),
                new SqlParameter(PARAM_FILEURL,SqlDbType.NVarChar),
                new SqlParameter("SoQuyetDinh",SqlDbType.NVarChar),
                new SqlParameter("NgayQuyetDinh",SqlDbType.DateTime),
                new SqlParameter("QuyetDinh",SqlDbType.NVarChar),
            };

            parms[0].Value = info.XuLyDonID;
            parms[1].Value = info.CoQuanPhanID;
            parms[2].Value = info.CoQuanGiaiQuyetID;
            parms[3].Value = info.GhiChu ?? Convert.DBNull; ;
            parms[4].Value = info.NgayChuyen;
            parms[5].Value = info.FileUrl ?? Convert.DBNull;
            parms[6].Value = info.SoQuyetDinh ?? Convert.DBNull;
            parms[7].Value = info.NgayQuyetDinh ?? Convert.DBNull;
            parms[8].Value = info.QuyetDinh ?? Convert.DBNull;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, "v2_ChuyenGiaiQuyet_Insert", parms);
                        //val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, INSERT, parms);
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

        #region -- insert cq phoi hop
        private SqlParameter[] GetInsertCQPhoiHopParms()
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_XULYDONID, SqlDbType.Int),
                new SqlParameter(PARAM_COQUANPHOIHOPID, SqlDbType.Int)
            }; return parms;
        }

        private void SetInsertCQPhoiHopParms(SqlParameter[] parms, CQPhoiHopGQInfo info)
        {
            parms[0].Value = info.XuLyDonID;
            parms[1].Value = info.CQPhoiHopID;
        }
        public int InsertCQPhoiHopGQ(CQPhoiHopGQInfo info)
        {
            object val = null;

            SqlParameter[] parameters = GetInsertCQPhoiHopParms();
            SetInsertCQPhoiHopParms(parameters, info);
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, INSERT_CQPHOIHOP, parameters);
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
        #endregion

        public bool CheckIsPhanTrongCoQuan(int xulydonid)
        {

            bool result = false;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDONID,SqlDbType.Int),

            };
            parameters[0].Value = xulydonid;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, CHECKIS_PHAN_TRONGCOQUAN, parameters))
                {

                    if (dr.Read())
                    {
                        result = Utils.ConvertToBoolean(dr["CheckIsPhanTrongCoQuan"], false);
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

        public List<ChuyenGiaiQuyetInfo> GetChuyenGQByCoQuan(int coQuanID, DateTime startDate, DateTime endDate)
        {

            List<ChuyenGiaiQuyetInfo> cgqList = new List<ChuyenGiaiQuyetInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_COQUANPHANID,SqlDbType.Int),
                new SqlParameter("StartDate", SqlDbType.DateTime),
                new SqlParameter("EndDate", SqlDbType.DateTime)

            };
            parameters[0].Value = coQuanID;
            parameters[1].Value = startDate;
            parameters[2].Value = endDate;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_CHUYEN_GQ_BY_COQUAN, parameters))
                {

                    while (dr.Read())
                    {
                        ChuyenGiaiQuyetInfo cgqInfo = new ChuyenGiaiQuyetInfo();
                        cgqInfo.CoQuanGiaiQuyetID = Utils.ConvertToInt32(dr["CoQuanGiaiQuyetID"], 0);
                        cgqInfo.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        cgqInfo.TenCoQuanGQ = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        cgqInfo.KetQuaID = Utils.ConvertToInt32(dr["KetQuaID"], 0);

                        cgqList.Add(cgqInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return cgqList;
        }

        public List<ChuyenGiaiQuyetInfo> GetChuyenGQJoinYKienGQ(int coQuanID, DateTime startDate, DateTime endDate)
        {

            List<ChuyenGiaiQuyetInfo> cgqList = new List<ChuyenGiaiQuyetInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_COQUANPHANID,SqlDbType.Int),
                new SqlParameter("StartDate", SqlDbType.DateTime),
                new SqlParameter("EndDate", SqlDbType.DateTime)

            };
            parameters[0].Value = coQuanID;
            parameters[1].Value = startDate;
            parameters[2].Value = endDate;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_CHUYEN_GQ_JOIN_YKIENGQ, parameters))
                {

                    while (dr.Read())
                    {
                        ChuyenGiaiQuyetInfo cgqInfo = new ChuyenGiaiQuyetInfo();
                        cgqInfo.CoQuanGiaiQuyetID = Utils.ConvertToInt32(dr["CoQuanGiaiQuyetID"], 0);
                        cgqInfo.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        cgqInfo.TenCoQuanGQ = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        cgqInfo.KetQuaID = Utils.ConvertToInt32(dr["KetQuaID"], 0);
                        cgqInfo.YKienGiaiQuyetID = Utils.ConvertToInt32(dr["YKienGiaiQuyetID"], 0);

                        cgqList.Add(cgqInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return cgqList;
        }

        public ChuyenGiaiQuyetInfo GetChuyenGiaiQuyetCoQuanKhac(int xulydonID)
        {

            ChuyenGiaiQuyetInfo cgqInfo = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDONID,SqlDbType.Int)

            };
            parameters[0].Value = xulydonID;


            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_CHUYENGIAIQUYET_CQ_KHAC, parameters))
                {

                    if (dr.Read())
                    {
                        cgqInfo = new ChuyenGiaiQuyetInfo();
                        cgqInfo.CoQuanGiaiQuyetID = Utils.ConvertToInt32(dr["CoQuanGiaiQuyetID"], 0);
                        cgqInfo.CoQuanPhanID = Utils.ConvertToInt32(dr["CoQuanPhanID"], 0);
                        cgqInfo.GhiChu = Utils.ConvertToString(dr["GhiChu"], string.Empty);
                        cgqInfo.FileUrl = Utils.ConvertToString(dr["FileUrl"], string.Empty);
                        cgqInfo.SoQuyetDinh = Utils.ConvertToString(dr["SoQuyetDinh"], string.Empty);
                        cgqInfo.QuyetDinh = Utils.ConvertToString(dr["QuyetDinh"], string.Empty);
                        cgqInfo.NgayQuyetDinh = Utils.ConvertToNullableDateTime(dr["NgayQuyetDinh"], null);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return cgqInfo;
        }

        public List<CQPhoiHopGQInfo> GetCoQuanPhoiHop(int xulydonID)
        {

            List<CQPhoiHopGQInfo> cgqInfo = new List<CQPhoiHopGQInfo>();

            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDONID,SqlDbType.Int)
            };
            parameters[0].Value = xulydonID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "ChuyenGiaiQuyet_GetCQPhoiHop", parameters))
                {

                    while (dr.Read())
                    {
                        CQPhoiHopGQInfo info = new CQPhoiHopGQInfo();
                        info.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
                        info.CQPhoiHopID = Utils.ConvertToInt32(dr["CQPhoiHopID"], 0);
                        info.TenCoQuan = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        info.PhoiHopGQID = Utils.ConvertToInt32(dr["PhoiHopGQID"], 0);
                        cgqInfo.Add(info);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return cgqInfo;
        }

        public int GetCoQuanChuyenGiaiQuyet(int xulydonid, int coquanid)
        {

            int result = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDONID,SqlDbType.Int),
                new SqlParameter(PARAM_COQUANGIAIQUYETID,SqlDbType.Int),

            };
            parameters[0].Value = xulydonid;
            parameters[1].Value = coquanid;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GETCOQUAN_CHUYENGQ, parameters))
                {

                    if (dr.Read())
                    {
                        result = Utils.ConvertToInt32(dr["CoQuanPhanID"], 0);
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
        public IList<CoQuanInfo> GetCoQuanDuocPhanGiaiQuyet(int xulydonid, int coquanid)
        {

            List<CoQuanInfo> result = new List<CoQuanInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@XuLyDonID",SqlDbType.Int),
                new SqlParameter("@CoQuanPhanID",SqlDbType.Int),

            };
            parameters[0].Value = xulydonid;
            parameters[1].Value = coquanid;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "ChuyenGiaiQuyet_GetCoQuanDuocPhan", parameters))
                {

                    while (dr.Read())
                    {
                        CoQuanInfo temp = new CoQuanInfo();
                        temp.CoQuanID = Utils.ConvertToInt32(dr["CoQuanGiaiQuyetID"], 0);
                        temp.IsPhuTrach = Utils.ConvertToInt32(dr["PhuTrach"], 0);
                        result.Add(temp);
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
        private const string GET_COQUAN_GQ = "ChuyenGiaiQuyet_GetCoQuanGiaiQuyet";
        public int GetCoQuanGiaiQuyet(int xulydonid)
        {

            int result = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDONID,SqlDbType.Int)

            };
            parameters[0].Value = xulydonid;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_COQUAN_GQ, parameters))
                {

                    if (dr.Read())
                    {
                        result = Utils.ConvertToInt32(dr["CoQuanGiaiQuyetID"], 0);
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

        public int DeleteChuyenCQKhac(int xldID)
        {

            int val = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDONID,SqlDbType.Int)
            };
            parameters[0].Value = xldID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, DELETE_CHUYENCQKHAC, parameters);
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

        public int DeleteCQPhoiHop(int xldID)
        {

            int val = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDONID,SqlDbType.Int)
            };
            parameters[0].Value = xldID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, DELETE_CQPHOIHOP, parameters);
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

        public int DeleteByXuLyDonID(int xldID)
        {

            int val = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDONID,SqlDbType.Int)
            };
            parameters[0].Value = xldID;
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

        public int PhanHoiInsert(int xldID, string noiDung, int phanLoai, int coQuanID, int canBoID)
        {
            int val = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@XuLyDonID",SqlDbType.Int),
                new SqlParameter("@NoiDung",SqlDbType.NVarChar),
                new SqlParameter("@PhanLoai",SqlDbType.Int),
                new SqlParameter("@CoQuanID",SqlDbType.Int),
                new SqlParameter("@CanBoID",SqlDbType.Int)
            };
            parameters[0].Value = xldID;
            parameters[1].Value = noiDung;
            parameters[2].Value = phanLoai;
            parameters[3].Value = coQuanID;
            parameters[4].Value = canBoID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "PhanHoi_Insert", parameters);
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
        public int PhanHoiUpdateTrangThai(int xldID, int trangThai)
        {
            int val = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@XuLyDonID",SqlDbType.Int),
                new SqlParameter("@TrangThai",SqlDbType.NVarChar)
            };
            parameters[0].Value = xldID;
            parameters[1].Value = trangThai;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "PhanHoi_UpdateTrangThai", parameters);
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
        public int PhanHoiDelete(int xldID)
        {
            int val = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@XuLyDonID",SqlDbType.Int)
            };
            parameters[0].Value = xldID;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "PhanHoi_Delete", parameters);
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
        public IList<YKienPhanHoiInfo> PhanHoiGetYKienByXuLyDonID(int xldID)
        {
            List<YKienPhanHoiInfo> cgqList = new List<YKienPhanHoiInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@XuLyDonID",SqlDbType.Int)

            };
            parameters[0].Value = xldID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "PhanHoi_GetByXuLyDonID", parameters))
                {

                    while (dr.Read())
                    {
                        YKienPhanHoiInfo cgqInfo = new YKienPhanHoiInfo();
                        cgqInfo.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
                        cgqInfo.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        cgqInfo.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        cgqInfo.NoiDungPhanHoi = Utils.ConvertToString(dr["NoiDungPhanHoi"], string.Empty);
                        cgqList.Add(cgqInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return cgqList;
        }
        public IList<YKienPhanHoiInfo> PhanHoiGetYKienByCanBoID(int xldID, int CanBoID)
        {
            List<YKienPhanHoiInfo> cgqList = new List<YKienPhanHoiInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@XuLyDonID",SqlDbType.Int),
                new SqlParameter("@CanBoID",SqlDbType.Int),

            };
            parameters[0].Value = xldID;
            parameters[1].Value = CanBoID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "PhanHoi_GetByCanBoID", parameters))
                {

                    while (dr.Read())
                    {
                        YKienPhanHoiInfo cgqInfo = new YKienPhanHoiInfo();
                        cgqInfo.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
                        cgqInfo.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        cgqInfo.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        cgqInfo.NoiDungPhanHoi = Utils.ConvertToString(dr["NoiDungPhanHoi"], string.Empty);
                        cgqList.Add(cgqInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return cgqList;
        }


        public List<FileHoSoInfo> GetFileGiaiQuyetByXyLyDonID(int xuLyDonID, int coQuanPhanID, int nguoiUpID, int loaiFile)
        {
            List<FileHoSoInfo> lst_HoSo = new List<FileHoSoInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDONID,SqlDbType.Int),
                new SqlParameter(PARAM_COQUANPHANID,SqlDbType.Int),
                new SqlParameter(PARAM_NGUOIUP,SqlDbType.Int),
                new SqlParameter(PARAM_LOAIFILE,SqlDbType.Int)
            };
            parameters[0].Value = xuLyDonID;
            parameters[1].Value = coQuanPhanID;
            parameters[2].Value = nguoiUpID;
            parameters[3].Value = loaiFile;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, FILECHUYENGIAIQUYET_GETBYXULYDONID, parameters))
                {
                    while (dr.Read())
                    {
                        FileHoSoInfo cInfo = new FileHoSoInfo();
                        cInfo.NguoiUp = Utils.ConvertToInt32(dr["NguoiUp"], 0);
                        cInfo.TenFile = Utils.ConvertToString(dr["TenFile"], string.Empty);
                        cInfo.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        cInfo.NgayUp = Utils.ConvertToDateTime(dr["NgayUp"], DateTime.MinValue);
                        cInfo.NgayUp_str = Format.FormatDate(cInfo.NgayUp);
                        cInfo.FileURL = Utils.ConvertToString(dr["FileURL"], string.Empty);
                        cInfo.IsBaoMat = Utils.ConvertToBoolean(dr["IsBaoMat"], false);
                        cInfo.CanBoID = Utils.ConvertToInt32(dr["NguoiUp"], 0);
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

        public int DeleteFileChuyenGiaiQuyet(int xuLyDonID, int coQuanPhanID, int nguoiUpID, int loaiFile)
        {

            int val = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDONID,SqlDbType.Int),
                new SqlParameter(PARAM_COQUANPHANID,SqlDbType.Int),
                new SqlParameter(PARAM_NGUOIUP,SqlDbType.Int),
                new SqlParameter(PARAM_LOAIFILE,SqlDbType.Int)
            };
            parameters[0].Value = xuLyDonID;
            parameters[1].Value = coQuanPhanID;
            parameters[2].Value = nguoiUpID;
            parameters[3].Value = loaiFile;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, FILECHUYENGIAIQUYET_DELETEBYXULYDONID, parameters);
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

        String GET_CHUYEN_GQ_TONG_HOP = "BC_ChuyenGiaiQuyet";
        public List<BaoCaoChuyenGQInfo> GetChuyenGQTongHop(int coQuanID, DateTime startDate, DateTime endDate)
        {

            List<BaoCaoChuyenGQInfo> cgqList = new List<BaoCaoChuyenGQInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_COQUANPHANID,SqlDbType.Int),
                new SqlParameter("StartDate", SqlDbType.DateTime),
                new SqlParameter("EndDate", SqlDbType.DateTime)

            };
            parameters[0].Value = coQuanID;
            parameters[1].Value = startDate;
            parameters[2].Value = endDate;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_CHUYEN_GQ_TONG_HOP, parameters))
                {

                    while (dr.Read())
                    {
                        BaoCaoChuyenGQInfo info = new BaoCaoChuyenGQInfo();
                        info.TenCoQuan = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        info.CoQuanGiaiQuyetID = Utils.ConvertToInt32(dr["CoQuanGiaiQuyetID"], 0);
                        info.SoLuong = Utils.ConvertToInt32(dr["TongSo"], 0);
                        info.SLDonChuaBaoCao = Utils.ConvertToInt32(dr["ChuaCoBC"], 0);
                        info.SLDonDaBaoCao = Utils.ConvertToInt32(dr["DaCoBC"], 0);
                        info.SLDaBanHanhQD = Utils.ConvertToInt32(dr["DaBanHanhQD"], 0);
                        cgqList.Add(info);
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return cgqList;
        }

        public List<ChuyenGiaiQuyetInfo> GetListChuyenGiaiQuyet(int xulydonID)
        {

            var list = new List<ChuyenGiaiQuyetInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDONID,SqlDbType.Int)

            };
            parameters[0].Value = xulydonID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_NV_ChuyenGiaiQuyet_GetDanhSachChuyenGiaiQuyet", parameters))
                {
                    while (dr.Read())
                    {
                        var model = new ChuyenGiaiQuyetInfo();
                        model.CoQuanGiaiQuyetID = Utils.ConvertToInt32(dr["CoQuanGiaiQuyetID"], 0);
                        model.CoQuanPhanID = Utils.ConvertToInt32(dr["CoQuanPhanID"], 0);
                        model.GhiChu = Utils.ConvertToString(dr["GhiChu"], string.Empty);
                        model.FileUrl = Utils.ConvertToString(dr["FileUrl"], string.Empty);
                        model.SoQuyetDinh = Utils.ConvertToString(dr["SoQuyetDinh"], string.Empty);
                        model.QuyetDinh = Utils.ConvertToString(dr["QuyetDinh"], string.Empty);
                        model.NgayQuyetDinh = Utils.ConvertToNullableDateTime(dr["NgayQuyetDinh"], null);
                        model.TenCoQuanPhan = Utils.ConvertToString(dr["TenCoQuanPhan"], string.Empty);
                        model.TenCoQuanGQ = Utils.ConvertToString(dr["TenCoQuanGQ"], string.Empty);
                        list.Add(model);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return list;
        }


        public int DeleteByChuyenGiaiQuyetID(int id)
        {

            int val = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("ChuyenGiaiQuyetID",SqlDbType.Int)
            };
            parameters[0].Value = id;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "v2_ChuyenGiaiQuyet_DeleteByChuyenGiaiQuyetID", parameters);
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
