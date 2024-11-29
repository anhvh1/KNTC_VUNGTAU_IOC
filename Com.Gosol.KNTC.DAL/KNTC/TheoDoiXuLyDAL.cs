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
    public class TheoDoiXuLyDAL
    {
        #region Database query string

        private const string SELECT_ALL = @"TheoDoiXuLy_GetAll";
        private const string SELECT_BY_ID = @"TheoDoiXuLy_GetByID";
        private const string DELETE = @"TheoDoiXuLy_Delete";
        private const string UPDATE = @"TheoDoiXuLy_Update";
        private const string INSERT = @"TheoDoiXuLy_Insert";

        private const string GET_BY_XULYDON = @"TheoDoiXuLy_GetByXuLyDon";

        private const string GET_DONTHU = "TheoDoiXuLy_GetDonThu";
        private const string GET_DONTHU_BY_PAGE = @"TheoDoiXuLy_GetDonThuByPage";
        private const string GET_DONTHU_PHAN_XU_LY_BY_PAGE = "TheoDoiXuLy_GetDonThuPhanXuLyByPage";
        private const string GET_DONTHU_BY_SEARCH = @"TheoDoiXuLy_GetDonThuBySearch";
        private const string GET_DONTHU_PHAN_XU_LY_BY_SEARCH = "TheoDoiXuLy_GetDonThuPhanXuLyBySearch";
        private const string COUNT_ALL_DONTHU = @"TheoDoiXuLy_CountAllDonThu";
        private const string COUNT_SEARCH_DONTHU = "TheoDoiXuLy_CountSearchDonThu";
        private const string COUNT_SEARCH_DONTHU_PHANXULY = "TheoDoiXuLy_CountSearchDonThuPhanXuLy";

        private const string GET_DONTHU_PHANXULY = "TheoDoiXuLy_GetDonThuPhanXuLy";
        private const string COUNT_DONTHU_PHANXULY_CHUAGIAIQUYET = "TheoDoiXuLy_CountDonThuPhanXuLyChuaGiaiQuyet";
        private const string COUNT_DONTHU_CHUAGIAIQUYET = "TheoDoiXuLy_CountDonThuChuaGiaiQuyet";

        private const string GET_DONTHU_BY_CANBO = "TheoDoiXuLy_GetDonThuByCanBoID";

        private const string GET_BY_XULYDON_AND_TRANGTHAIDON = "TheoDoiXuLy_GetByXuLyDonAndTrangThaiDon";
        private const string GET_QUATRINHGIAIQUYET = @"TheoDoiXuLy_GetQuyTrinhGiaiQuyet";
        private const string GET_QUATRINHXACMINH = @"TheoDoiXuLy_GetQuaTrinhXacMinh";
        private const string GET_QUATRINHXACMINH_NEW = @"TheoDoiXuLy_GetQuaTrinhXacMinh_New";
        private const string GET_QUYETDINHGIAOXACMINH = @"ChuyenGiaiQuyet_GetByID";
        private const string GET_QUYETDINHGIAOXACMINH_NEW = @"ChuyenGiaiQuyet_GetByID_New";
        private const string CHECKBAOCAOXACMINH = @"TheoDoiXuLy_CheckBaoCaoXacMinh";
        #endregion

        #region paramaters constant

        private const string PARM_THEODOIXULYID = @"TheoDoiXuLyID";
        private const string PARM_TRANGTHAIXULYID = @"TrangThaiXuLyID";
        private const string PARM_NGAYCAPNHAT = @"NgayCapNhat";
        private const string PARM_XULYDONID = @"XuLyDonID";
        private const string PARM_FILEURL = "@FileURL";
        private const string PARM_GHICHU = "@GhiChu";
        private const string PARM_TENBUOC = "@TenBuoc";

        private const string PARM_COQUANID = @"CoQuanID";
        private const string PARM_CANBOID = "CanBoID";
        private const string PARM_BUOCXACMINHID = @"BuocXacMinhID";

        private const string PARM_START = @"Start";
        private const string PARM_END = @"End";
        private const string PARM_KEYWORD = @"Keyword";

        #endregion

        private TheoDoiXuLyInfo GetData(SqlDataReader rdr)
        {
            TheoDoiXuLyInfo theoDoiXuLyInfo = new TheoDoiXuLyInfo();
            theoDoiXuLyInfo.TheoDoiXuLyID = Utils.GetInt32(rdr["TheoDoiXuLyID"], 0);
            theoDoiXuLyInfo.TrangThaiXuLyID = Utils.ConvertToInt32(rdr["TrangThaiXuLyID"], 0);
            theoDoiXuLyInfo.NgayCapNhat = Utils.GetDateTime(rdr["NgayCapNhat"], DateTime.Now);
            theoDoiXuLyInfo.StringNgayCapNhat = theoDoiXuLyInfo.NgayCapNhat.ToString("dd/MM/yyyy");
            theoDoiXuLyInfo.XuLyDonID = Utils.GetInt32(rdr["XuLyDonID"], 0);
            theoDoiXuLyInfo.TenTrangThaiXuLy = Utils.ConvertToString(rdr["TenTrangThaiXuLy"], String.Empty);
            theoDoiXuLyInfo.GhiChu = Utils.ConvertToString(rdr["GhiChu"], string.Empty);

            return theoDoiXuLyInfo;
        }

        private TDXLDonThuInfo GetDonThuCustomData(SqlDataReader rdr)
        {
            TDXLDonThuInfo dtInfo = new TDXLDonThuInfo();
            dtInfo.DonThuID = Utils.ConvertToInt32(rdr["SoDonThu"], 0);
            dtInfo.XuLyDonID = Utils.ConvertToInt32(rdr["XuLyDonID"], 0);

            return dtInfo;
        }

        private TDXLDonThuInfo GetDonThuData(SqlDataReader rdr)
        {
            TDXLDonThuInfo dtInfo = new TDXLDonThuInfo();
            dtInfo.DonThuID = Utils.ConvertToInt32(rdr["DonThuID"], 0);
            dtInfo.XuLyDonID = Utils.ConvertToInt32(rdr["XuLyDonID"], 0);
            dtInfo.SoDonThu = Utils.ConvertToInt32(rdr["SoDonThu"], 0);
            dtInfo.NhomKNID = Utils.GetInt32(rdr["NhomKNID"], 0);
            dtInfo.NoiDungDon = Utils.ConvertToString(rdr["NoiDungDon"], String.Empty);
            dtInfo.LoaiKhieuTo1ID = Utils.ConvertToInt32(rdr["LoaiKhieuTo1ID"], 0);
            dtInfo.ThoiHan = Utils.ConvertToDateTime(rdr["ThoiHan"], DateTime.Now);
            dtInfo.GhiChu = Utils.GetString(rdr["GhiChu"], String.Empty);
            dtInfo.LanGiaiQuyet = Utils.GetInt32(rdr["LanGiaiQuyet"], 0);
            dtInfo.PhanGiaiQuyetID = Utils.GetInt32(rdr["PhanGiaiQuyetID"], 0);
            dtInfo.TenCanBo = Utils.GetString(rdr["TenCanBo"], String.Empty);

            return dtInfo;
        }

        private SqlParameter[] GetInsertParms()
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_TRANGTHAIXULYID, SqlDbType.Int),
                new SqlParameter(PARM_XULYDONID, SqlDbType.Int),
                new SqlParameter(PARM_NGAYCAPNHAT, SqlDbType.DateTime),
                new SqlParameter(PARM_GHICHU, SqlDbType.NVarChar, 200),
                new SqlParameter(PARM_CANBOID, SqlDbType.Int)

            }; return parms;
        }

        private void SetInsertParms(SqlParameter[] parms, TheoDoiXuLyInfo theoDoiXuLyInfo)
        {
            parms[0].Value = theoDoiXuLyInfo.TrangThaiXuLyID;
            parms[1].Value = theoDoiXuLyInfo.XuLyDonID;
            parms[2].Value = theoDoiXuLyInfo.NgayCapNhat;
            parms[3].Value = theoDoiXuLyInfo.GhiChu;
            parms[4].Value = theoDoiXuLyInfo.CanBoID;
            //parms[3].Value = theoDoiXuLyInfo.FileURL;

            if (theoDoiXuLyInfo.TrangThaiXuLyID == 0) parms[0].Value = DBNull.Value;
        }

        private SqlParameter[] GetUpdateParms()
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_THEODOIXULYID, SqlDbType.Int),
                new SqlParameter(PARM_NGAYCAPNHAT, SqlDbType.DateTime),
                new SqlParameter(PARM_GHICHU, SqlDbType.NVarChar, 200),
                new SqlParameter(PARM_CANBOID, SqlDbType.Int)
            };
            return parms;
        }

        private void SetUpdateParms(SqlParameter[] parms, TheoDoiXuLyInfo theoDoiXuLyInfo)
        {
            parms[0].Value = theoDoiXuLyInfo.TheoDoiXuLyID;
            parms[1].Value = theoDoiXuLyInfo.NgayCapNhat;
            parms[2].Value = theoDoiXuLyInfo.GhiChu;
            parms[3].Value = theoDoiXuLyInfo.CanBoID;
        }

        public int CountAllDonThu(int cqID)
        {
            int result = 0;
            SqlParameter parm = new SqlParameter(PARM_COQUANID, SqlDbType.Int);
            parm.Value = cqID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_ALL_DONTHU, parm))
                {
                    if (dr.Read())
                    {
                        result = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();

                }
            }
            catch (Exception e)


            {
                throw;
            }
            return result;
        }

        public IList<TheoDoiXuLyInfo> GetQuaTrinhXacMinh(int XuLyDonId)
        {
            SqlParameter parm = new SqlParameter(PARM_XULYDONID, SqlDbType.Int);
            parm.Value = XuLyDonId;

            IList<TheoDoiXuLyInfo> dtList = new List<TheoDoiXuLyInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_QUATRINHXACMINH_NEW, parm))
                {
                    while (dr.Read())
                    {
                        TheoDoiXuLyInfo dtInfo = GetDataQTGQ(dr);
                        dtInfo.DuongDanFile = Utils.GetString(dr["DuongDanFile"], string.Empty);
                        dtInfo.NoiDung = Utils.ConvertToString(dr["NoiDung"], string.Empty);
                        dtInfo.TenCanBo = Utils.GetString(dr["TenCanBo"], string.Empty);
                        dtInfo.TenBuoc = Utils.ConvertToString(dr["TenBuoc"], string.Empty);
                        //dtInfo.TenFile = Utils.ConvertToString(dr["TenFile"], string.Empty);
                        dtInfo.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        dtInfo.TenCoQuanUp = new CoQuan().GetCoQuanByCanBoID(dtInfo.CanBoID).TenCoQuan;
                        dtInfo.BuocXacMinhID = Utils.ConvertToInt32(dr["BuocXacMinhID"], 0);
                        if (dtInfo.BuocXacMinhID == 0)
                        {
                            dtInfo.TenBuoc = "Yêu cầu đối thoại";
                        }
                        dtInfo.IsBaoMat = Utils.ConvertToBoolean(dr["IsBaoMat"], false);
                        dtInfo.FileID = Utils.ConvertToInt32(dr["FileID"], 0);
                        dtInfo.TenFile = Utils.ConvertToString(dr["TenFileNew"], string.Empty);
                        dtInfo.NhomFileID = Utils.ConvertToInt32(dr["NhomFileID"], 0);
                        dtInfo.TenNhomFile = Utils.ConvertToString(dr["TenNhomFile"], string.Empty);
                        dtInfo.ThuTuHienThiNhom = Utils.ConvertToInt32(dr["ThuTuHienThi"], 0);
                        dtInfo.ThuTuHienThiFile = Utils.ConvertToInt32(dr["ThuTuHienThiFile"], 0);
                        dtInfo.FileGiaiQuyetID = Utils.ConvertToInt32(dr["FileGiaiQuyetID"], 0);
                        dtList.Add(dtInfo);
                    }
                    dr.Close();
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return dtList;
        }

        public IList<ChuyenGiaiQuyetInfo> GetQuyetDinhGiaoXacMinh(int XuLyDonId)
        {
            SqlParameter parm = new SqlParameter(PARM_XULYDONID, SqlDbType.Int);
            parm.Value = XuLyDonId;

            IList<ChuyenGiaiQuyetInfo> dtList = new List<ChuyenGiaiQuyetInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_QUYETDINHGIAOXACMINH_NEW, parm))
                {
                    while (dr.Read())
                    {
                        ChuyenGiaiQuyetInfo dtInfo = new ChuyenGiaiQuyetInfo();
                        dtInfo.IsBaoMat = Utils.ConvertToBoolean(dr["IsBaoMat"], false);
                        dtInfo.FileUrl = Utils.ConvertToString(dr["FileUrl"], string.Empty);
                        dtInfo.GhiChu = Utils.ConvertToString(dr["GhiChu"], string.Empty);
                        //dtInfo.TenFile = Utils.ConvertToString(dr["TenFile"], string.Empty);
                        dtInfo.TenCoQuanPhan = Utils.ConvertToString(dr["TenCoQuanPhan"], string.Empty);
                        dtInfo.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        dtInfo.NgayChuyen = Utils.ConvertToDateTime(dr["NgayUp"], DateTime.MinValue);
                        dtInfo.CanBoID = Utils.ConvertToInt32(dr["NguoiUp"], 0);
                        dtInfo.NgayChuyen_Str = Format.FormatDate(dtInfo.NgayChuyen);
                        dtInfo.FileID = Utils.ConvertToInt32(dr["FileID"], 0);
                        dtInfo.TenFile = Utils.ConvertToString(dr["TenFileNew"], string.Empty);
                        dtInfo.NhomFileID = Utils.ConvertToInt32(dr["NhomFileID"], 0);
                        dtInfo.TenNhomFile = Utils.ConvertToString(dr["TenNhomFile"], string.Empty);
                        dtInfo.ThuTuHienThiNhom = Utils.ConvertToInt32(dr["ThuTuHienThi"], 0);
                        dtInfo.ThuTuHienThiFile = Utils.ConvertToInt32(dr["ThuTuHienThiFile"], 0);
                        dtInfo.FileHoSoID = Utils.ConvertToInt32(dr["FileHoSoID"], 0);
                        dtList.Add(dtInfo);
                    }
                    dr.Close();
                }
            }
            catch (Exception e)
            {
            }
            return dtList;
        }

        public int CountSearchDonThu(String keyword, int cqID, int nguondon)
        {
            int result = 0;
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_KEYWORD, SqlDbType.NVarChar, 200),
                new SqlParameter(PARM_COQUANID, SqlDbType.Int),
            };
            parm[0].Value = keyword;
            parm[1].Value = cqID;

            String spName = COUNT_SEARCH_DONTHU;

            if (nguondon == Constant.DonDuocPhanXuLy)
            {
                spName = COUNT_SEARCH_DONTHU_PHANXULY;
            }

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, spName, parm))
                {
                    if (dr.Read())
                    {
                        result = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();

                }
            }
            catch (Exception e)
            {
                throw;
            }
            return result;
        }

        public int CountDonThuChuaGiaiQuyet(int cqID, int nguondon)
        {
            int result = 0;
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_COQUANID, SqlDbType.Int)
            };
            parm[0].Value = cqID;

            String spName = String.Empty;
            if (nguondon == Constant.DonTrongCoQuan)
            {
                spName = COUNT_DONTHU_CHUAGIAIQUYET;
            }
            else if (nguondon == Constant.DonDuocPhanXuLy)
            {
                spName = COUNT_DONTHU_PHANXULY_CHUAGIAIQUYET;
            }

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, spName, parm))
                {
                    if (dr.Read())
                    {
                        result = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();

                }
            }
            catch (Exception e)
            {
                throw;
            }
            return result;
        }

        public IList<TheoDoiXuLyInfo> GetQuaTrinhGiaiQuyet(int XuLyDonId)
        {
            SqlParameter parm = new SqlParameter(PARM_XULYDONID, SqlDbType.Int);
            parm.Value = XuLyDonId;

            IList<TheoDoiXuLyInfo> dtList = new List<TheoDoiXuLyInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_QUATRINHGIAIQUYET, parm))
                {
                    while (dr.Read())
                    {
                        TheoDoiXuLyInfo dtInfo = GetDataQTGQ(dr);
                        dtInfo.TenTrangThaiXuLy = Utils.ConvertToString(dr["TenTrangThaiXuLy"], string.Empty);
                        dtInfo.TenBuoc = Utils.ConvertToString(dr["TenBuoc"], string.Empty);
                        dtInfo.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        dtInfo.TenCoQuan = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        dtInfo.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        dtInfo.BuocXacMinhID = Utils.ConvertToInt32(dr["BuocXacMinhID"], 0);
                        dtInfo.SoFile = Utils.ConvertToInt32(dr["SoFile"], 0);
                        dtList.Add(dtInfo);
                    }
                    dr.Close();
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return dtList;
        }

        private TheoDoiXuLyInfo GetDataQTGQ(SqlDataReader rdr)
        {
            TheoDoiXuLyInfo info = new TheoDoiXuLyInfo();
            info.TrangThaiXuLyID = Utils.ConvertToInt32(rdr["TrangThaiXuLyID"], 0);
            info.TheoDoiXuLyID = Utils.GetInt32(rdr["TheoDoiXuLyID"], 0);
            info.NgayCapNhat = Utils.ConvertToDateTime(rdr["NgayCapNhat"], DateTime.MinValue);
            info.GhiChu = Utils.ConvertToString(rdr["GhiChu"], string.Empty);
            // info.DuongDanFile = Utils.GetString(rdr["DuongDanFile"], string.Empty);
            info.StringNgayCapNhat = "";
            if (info.NgayCapNhat != DateTime.MinValue)
                info.StringNgayCapNhat = info.NgayCapNhat.ToString("dd/MM/yyyy");
            return info;
        }

        private ChuyenGiaiQuyetInfo GetQuyetDinhGiaoXacMinh(SqlDataReader rdr)
        {
            ChuyenGiaiQuyetInfo info = new ChuyenGiaiQuyetInfo();
            info.TenCoQuanPhan = Utils.GetString(rdr["TenCoQuanPhan"], string.Empty);
            info.FileUrl = Utils.GetString(rdr["FileUrl"], string.Empty);
            info.NgayChuyen = Utils.ConvertToDateTime(rdr["NgayChuyen"], DateTime.MinValue);
            return info;
        }

        public IList<TDXLDonThuInfo> GetDonThu(int cqID, int nguondon)
        {
            SqlParameter parm = new SqlParameter(PARM_COQUANID, SqlDbType.Int);
            parm.Value = cqID;

            String spName = String.Empty;

            if (nguondon == Constant.DonTrongCoQuan)
            {
                spName = GET_DONTHU;
            }
            else
            {
                spName = GET_DONTHU_PHANXULY;
            }

            IList<TDXLDonThuInfo> dtList = new List<TDXLDonThuInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, spName, parm))
                {
                    while (dr.Read())
                    {
                        TDXLDonThuInfo dtInfo = GetDonThuData(dr);
                        dtList.Add(dtInfo);
                    }
                    dr.Close();
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return dtList;
        }

        public IList<TDXLDonThuInfo> GetDonThu(int canboID)
        {
            SqlParameter parm = new SqlParameter(PARM_CANBOID, SqlDbType.Int);
            parm.Value = canboID;

            String spName = String.Empty;

            IList<TDXLDonThuInfo> dtList = new List<TDXLDonThuInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DONTHU_BY_CANBO, parm))
                {
                    while (dr.Read())
                    {
                        TDXLDonThuInfo dtInfo = GetDonThuData(dr);
                        dtList.Add(dtInfo);
                    }
                    dr.Close();
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return dtList;
        }

        public IList<TDXLDonThuInfo> GetDonThuBySearch(String keyword, int page, int cqID, int nguondon)
        {
            int start = (page - 1) * Constant.PageSize;
            int end = page * Constant.PageSize;

            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_KEYWORD, SqlDbType.NVarChar, 50),
                new SqlParameter(PARM_START, SqlDbType.Int),
                new SqlParameter(PARM_END, SqlDbType.Int),
                new SqlParameter(PARM_COQUANID, SqlDbType.Int)
            };

            parm[0].Value = keyword;
            parm[1].Value = start;
            parm[2].Value = end;
            parm[3].Value = cqID;

            String spName = GET_DONTHU_BY_SEARCH;

            if (nguondon == Constant.DonDuocPhanXuLy)
            {
                spName = GET_DONTHU_PHAN_XU_LY_BY_SEARCH;
            }

            IList<TDXLDonThuInfo> dtList = new List<TDXLDonThuInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, spName, parm))
                {
                    while (dr.Read())
                    {
                        TDXLDonThuInfo dtInfo = GetDonThuData(dr);
                        dtList.Add(dtInfo);
                    }
                    dr.Close();
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return dtList;
        }

        public IList<TDXLDonThuInfo> GetDonThuByPage(int page, int cqID)
        {
            IList<TDXLDonThuInfo> dtList = new List<TDXLDonThuInfo>();
            int start = (page - 1) * Constant.PageSize;
            int end = (page) * Constant.PageSize;
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_START, SqlDbType.Int),
                new SqlParameter(PARM_END, SqlDbType.Int),
                new SqlParameter(PARM_COQUANID, SqlDbType.Int)
            };
            parm[0].Value = start;
            parm[1].Value = end;
            parm[2].Value = cqID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DONTHU_BY_PAGE, parm))
                {
                    while (dr.Read())
                    {
                        TDXLDonThuInfo dtInfo = GetDonThuData(dr);
                        dtList.Add(dtInfo);
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return dtList;
        }

        public TheoDoiXuLyInfo GetByXuLyDon(int xldID, int trangthaiID)
        {
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_XULYDONID, SqlDbType.Int),
                new SqlParameter(PARM_TRANGTHAIXULYID, SqlDbType.Int)
            };
            parm[0].Value = xldID;
            parm[1].Value = trangthaiID;

            TheoDoiXuLyInfo theoDoiXuLyInfo = null;

            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_XULYDON_AND_TRANGTHAIDON, parm))
            {
                if (dr.Read())
                {
                    theoDoiXuLyInfo = GetData(dr);
                }
                dr.Close();
            }
            return theoDoiXuLyInfo;
        }

        public IList<TheoDoiXuLyInfo> GetByXuLyDon(int xldID)
        {
            SqlParameter parm = new SqlParameter(PARM_XULYDONID, SqlDbType.Int);
            parm.Value = xldID;

            IList<TheoDoiXuLyInfo> theodoixulys = new List<TheoDoiXuLyInfo>();
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_XULYDON, parm))
            {
                while (dr.Read())
                {
                    TheoDoiXuLyInfo theoDoiXuLyInfo = GetData(dr);
                    theodoixulys.Add(theoDoiXuLyInfo);
                }
                dr.Close();
            }
            return theodoixulys;
        }

        public IList<TheoDoiXuLyInfo> GetAll()
        {
            IList<TheoDoiXuLyInfo> theodoixulys = new List<TheoDoiXuLyInfo>();
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, SELECT_ALL, null))
            {
                while (dr.Read())
                {
                    TheoDoiXuLyInfo theoDoiXuLyInfo = GetData(dr);
                    theodoixulys.Add(theoDoiXuLyInfo);
                }
                dr.Close();
            }
            return theodoixulys;
        }

        public TheoDoiXuLyInfo GetByID(int theodoixulyID)
        {
            TheoDoiXuLyInfo theoDoiXuLyInfo = null;
            SqlParameter[] parameters = new SqlParameter[] {
            new SqlParameter(PARM_THEODOIXULYID, SqlDbType.Int) };
            parameters[0].Value = theodoixulyID;
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, SELECT_BY_ID, parameters))
            {
                if (dr.Read())
                {
                    theoDoiXuLyInfo = GetData(dr);
                }
                dr.Close();
            }
            return theoDoiXuLyInfo;
        }

        public int Delete(int theodoixulyID)
        {
            int val = 0;
            SqlParameter[] parameters = new SqlParameter[] {
new SqlParameter(PARM_THEODOIXULYID, SqlDbType.Int) };
            parameters[0].Value = theodoixulyID;
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

        public int Update(TheoDoiXuLyInfo theoDoiXuLyInfo)
        {
            int val = 0;
            SqlParameter[] parameters = GetUpdateParms();
            SetUpdateParms(parameters, theoDoiXuLyInfo);
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

        public int Insert(TheoDoiXuLyInfo theoDoiXuLyInfo)
        {
            object val = null;
            SqlParameter[] parameters = GetInsertParms();
            SetInsertParms(parameters, theoDoiXuLyInfo);
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

        private SqlParameter[] GetInsertParmsNew()
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_TRANGTHAIXULYID, SqlDbType.Int),
                new SqlParameter(PARM_XULYDONID, SqlDbType.Int),
                new SqlParameter(PARM_NGAYCAPNHAT, SqlDbType.DateTime),
                new SqlParameter(PARM_GHICHU, SqlDbType.NVarChar),
                new SqlParameter(PARM_CANBOID, SqlDbType.Int),
                new SqlParameter(PARM_BUOCXACMINHID, SqlDbType.Int),
                new SqlParameter(PARM_TENBUOC, SqlDbType.NVarChar)

            }; return parms;
        }

        private void SetInsertParmsNew(SqlParameter[] parms, TheoDoiXuLyInfo theoDoiXuLyInfo)
        {
            parms[0].Value = theoDoiXuLyInfo.TrangThaiXuLyID;
            parms[1].Value = theoDoiXuLyInfo.XuLyDonID;
            parms[2].Value = theoDoiXuLyInfo.NgayCapNhat;
            parms[3].Value = theoDoiXuLyInfo.GhiChu;
            parms[4].Value = theoDoiXuLyInfo.CanBoID;
            parms[5].Value = theoDoiXuLyInfo.BuocXacMinhID;
            parms[6].Value = theoDoiXuLyInfo.TenBuoc;

            if (theoDoiXuLyInfo.TrangThaiXuLyID == 0) parms[0].Value = DBNull.Value;
        }

        public int Insert_New(TheoDoiXuLyInfo theoDoiXuLyInfo)
        {
            object val = null;
            SqlParameter[] parameters = GetInsertParmsNew();
            SetInsertParmsNew(parameters, theoDoiXuLyInfo);
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, "TheoDoiXuLy_Insert_1508", parameters);
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

        private SqlParameter[] GetUpdateParmsNew()
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_THEODOIXULYID, SqlDbType.Int),
                new SqlParameter(PARM_NGAYCAPNHAT, SqlDbType.DateTime),
                new SqlParameter(PARM_GHICHU, SqlDbType.NVarChar, 200),
                new SqlParameter(PARM_CANBOID, SqlDbType.Int),
                new SqlParameter(PARM_BUOCXACMINHID, SqlDbType.Int),
                new SqlParameter(PARM_TENBUOC, SqlDbType.NVarChar),
            };
            return parms;
        }

        private void SetUpdateParmsNew(SqlParameter[] parms, TheoDoiXuLyInfo theoDoiXuLyInfo)
        {
            parms[0].Value = theoDoiXuLyInfo.TheoDoiXuLyID;
            parms[1].Value = theoDoiXuLyInfo.NgayCapNhat;
            parms[2].Value = theoDoiXuLyInfo.GhiChu;
            parms[3].Value = theoDoiXuLyInfo.CanBoID;
            parms[4].Value = theoDoiXuLyInfo.BuocXacMinhID;
            parms[5].Value = theoDoiXuLyInfo.TenBuoc;
        }

        public int Update_New(TheoDoiXuLyInfo theoDoiXuLyInfo)
        {
            int val = 0;
            SqlParameter[] parameters = GetUpdateParmsNew();
            SetUpdateParmsNew(parameters, theoDoiXuLyInfo);
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "TheoDoiXuLy_Update_1508", parameters);
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

        public int CheckBaoCaoXacMinh(int xuLyDonID)
        {
            int val = 0;
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter(PARM_XULYDONID, SqlDbType.Int),
            };
            parameters[0].Value = xuLyDonID;
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "TheoDoiXuLy_CheckBaoCaoXacMinh", parameters))
            {
                if (dr.Read())
                {
                    val = Utils.ConvertToInt32(dr["TheoDoiXuLyID"], 0);
                }
                dr.Close();
            }
            return val;
        }
    }
}
