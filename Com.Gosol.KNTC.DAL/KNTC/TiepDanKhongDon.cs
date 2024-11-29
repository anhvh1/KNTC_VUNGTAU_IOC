using Com.Gosol.KNTC.Model.HeThong;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Models.TiepDan;
using Com.Gosol.KNTC.Ultilities;
using DocumentFormat.OpenXml.EMMA;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.DAL.KNTC
{
    public class TiepDanKhongDon
    {

        private const string GET_ALL_MANAGE = @"NV_TiepDanKhongDon_GetAll";
        private const string COUNT_ALL_MANAGE = @"NV_TiepDanKhongDon_CountAll";
        private const string SEARCH_MANAGE = @"NV_TiepDanKhongDon_GetBySearch";
        private const string COUNT_SEARCH_MANAGE = @"NV_TiepDanKhongDon_CountSearch";
        private const string DELETE_MANAGE = @"NV_TiepDanKhongDon_Delete";
        private const string GET_INSO = @"NV_TiepDanKhongDon_GetInSo";
        private const string GET_IN_TIME = "NV_TiepDanKhongDon_GetInTime";
        private const string GET_DS_GAP_LANH_DAO = "TiepDanKhongDon_Get_GapLanhDao";
        private const string GET_KHIEUKIENDONGNGUOI = @"NV_TiepDanKhongDon_KhieuKienDongNguoi";
        private const string GETTONGSODONBYLOAIKHIEUTO = @"TiepDanKhongDon_CountSoDonByLoaiKhieuTo";
        private const string GET_FILESOTIEPDAN_LIST = @"SoTiepDan_GetFileSoTiepDan_List";

        //cac bien dau vao
        private const string PARAM_DONTHU_ID = "@DonThuID";
        private const string PARAM_TIEP_DAN_KHONG_DON_ID = "@TiepDanKhongDonID";
        private const string PARAM_NGAY_TIEP = "@NgayTiep";
        private const string PARAM_NOI_DUNG_TIEP = "@NoiDungTiep";
        private const string PARAM_VU_VIEC_CU = "@VuViecCu";
        private const string PARAM_TEN_CAN_BO = "@TenCanBo";
        private const string PARAM_TEN_CO_QUAN = "@TenCoQuan";
        private const string PARAM_CO_QUAN_ID = "@CoQuanID";
        private const string PARAM_NHOMKNID = @"NhomKNID";

        private const string PARAM_KEY = "@Keyword";
        private const string PARAM_START = "@Start";
        private const string PARAM_END = "@End";
        private const string PARAM_LOAIKHIEUTO_ID = "@LoaiKhieuToID";
        private const string PARAM_ORDER = "@Order";
        private const string PARAM_DDL_SEARCH = "@ddl_search";
        private const string PARAM_STARTs = "@start";
        private const string PARAM_ENDs = "@end";
        private const string PARAM_THOIGIAN = "@ThoiGian";
        private const string PARM_LOAIKHIEUTO = "@LoaiKhieuTo";


        //quanghv
        private const string GET_ALL = @"DonThu_GetAll";
        private const string GET_BY_ID = @"NV_TiepDanKhongDon_GetByID";
        private const string GET_BY_DONTHU_ID = @"NV_XuLyDon_GetByDonID";
        private const string INSERT = @"NV_TiepDanKhongDonStep2_Insert";
        private const string UPDATE = @"NV_TiepDanKhongDonStep2_Update";
        private const string GET_BY_XLD_ID = @"NV_TiepDanKhongDon_GetByXuLyDonID";
        private const string DELETE = @"NV_TiepDanKhongDon_Delete";
        private const string GET_BY_COQUANID = @"NV_TiepDanKhongDon_GetByCoQuanID";
        private const string GET_LIST_VUVIEC = @"NV_TiepDanKhongDon_GetListVuViec";
        private const string UPDATE_VUVIECCU = @"NV_TiepDanKhongDonStep1_UpdateVuViecCu";
        private const string CHECK_VUVIECMOI = @"NV_TiepDanKhongDon_CheckVuViecMoi";
        private const string GET_TOTAL_VUVIEC = @"NV_TiepDanKhongDon_GetTotalVuViec";

        private const string GET_BY_PAGE = @"NV_DonThu_GetByPage";
        private const string COUNT_ALL = @"NV_DonThu_CountAll";
        private const string GET_TIEPDANKHONGDON_BYNHOMKNID = @"TiepDanKhongDon_GetByID";
        //ten cac bien dau vao
        private const string PARAM_TIEPDANKHONGDON_ID = "@TiepDanKhongDonID";
        private const string PARAM_NGAYTIEP = "@NgayTiep";
        private const string PARAM_NOIDUNGTIEP = "@NoiDungTiep";
        private const string PARAM_VUVIECCU = "@VuViecCu";
        private const string PARAM_CANBOTIEP_ID = "@CanBoTiepID";
        //private const string PARAM_DONTHU_ID = "@DonThuID";

        private const string PARAM_GAPLANHDAO = "@GapLanhDao";
        private const string PARAM_XULYDON_ID = "@XuLyDonID";
        private const string PARAM_LOAICANBOTIEP = "@LoaiCanBoTiep";

        private const string PARAM_PAGE = "@Page";
        private const string PARAM_LIMIT = "@Limit";
        private const string PARAM_SEARCHKEY = "@SearchKey";

        private const string PARAM_TUNGAY = @"TuNgay";
        private const string PARAM_DENNGAY = @"DenNgay";
        private const string PARAM_DONTHUID = @"DonThuID";
        private const string PARAM_XULYDONID = @"XuLyDonID";
        //end quanghv
        private const string THSDPHANMEM_COUNTDONTHUDATIEP = "TiepDanKhongDon_CountSoDonDaTiepNhan";
        private TiepDanKhongDonInfo GetDataForShowManage(SqlDataReader dr)
        {
            TiepDanKhongDonInfo TDKDInfo = new TiepDanKhongDonInfo();
            TDKDInfo.TiepDanKhongDonID = Utils.GetInt32(dr["TiepDanKhongDonID"], 0);
            TDKDInfo.DonThuID = Utils.GetInt32(dr["DonThuID"], 0);
            TDKDInfo.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);
            TDKDInfo.NgayTiep = Utils.GetDateTime(dr["NgayTiep"], Constant.DEFAULT_DATE);
            TDKDInfo.NgayGapLanhDao = Utils.GetDateTime(dr["NgayGapLanhDao"], Constant.DEFAULT_DATE);
            TDKDInfo.NoiDungTiep = Utils.ConvertToString(dr["NoiDungTiep"].ToString(), string.Empty);
            TDKDInfo.NoiDungDon = Utils.ConvertToString(dr["NoiDungDon"].ToString(), string.Empty);
            if (TDKDInfo.NoiDungDon == "" && TDKDInfo.NoiDungTiep != null && TDKDInfo.NoiDungTiep.Length > 0)
            {
                TDKDInfo.NoiDungDon = TDKDInfo.NoiDungTiep;
            }
            TDKDInfo.VuViecCu = Utils.GetBoolean(dr["VuViecCu"], false);
            if (TDKDInfo.VuViecCu)
            {
                TDKDInfo.TrangThaiVuViec = "Cũ";
            }
            else
            {
                TDKDInfo.TrangThaiVuViec = "Mới";
            }
            TDKDInfo.TenCanBo = Utils.ConvertToString(dr["TenCanBo"].ToString(), string.Empty);
            if (Utils.ConvertToBoolean(dr["GapLanhDao"], false) == true)
            {
                TDKDInfo.TenCanBo = Utils.ConvertToString(dr["TenLanhDaoTiep"].ToString(), string.Empty);
            }

            TDKDInfo.TenCoQuan = Utils.GetString(dr["TenCoQuan"].ToString(), string.Empty);
            TDKDInfo.NhomKNID = Utils.GetInt32(dr["NhomKNID"], 0);

            return TDKDInfo;
        }

        private TiepDanKhongDonInfo GetDataGapLanhDao(SqlDataReader dr)
        {
            TiepDanKhongDonInfo TDKDInfo = new TiepDanKhongDonInfo();
            TDKDInfo.TiepDanKhongDonID = Utils.GetInt32(dr["TiepDanKhongDonID"], 0);
            TDKDInfo.HoTen = Utils.GetString(dr["HoTen"], string.Empty);
            TDKDInfo.DiaChi = Utils.GetString(dr["DiaChiCT"], string.Empty);
            TDKDInfo.TenLoaiKhieuTo = Utils.ConvertToString(dr["TenLoaiKhieuTo"], string.Empty);
            TDKDInfo.NgayTiep = Utils.GetDateTime(dr["NgayTiep"], Constant.DEFAULT_DATE);
            TDKDInfo.NoiDungTiep = Utils.ConvertToString(dr["NoiDungTiep"].ToString(), string.Empty);
            TDKDInfo.TenLanhDaoTiep = Utils.ConvertToString(dr["LanhDaoDangKy"], string.Empty);
            return TDKDInfo;
        }

        //lay xulydon info theo don id
        public List<TiepDanKhongDonInfo> GetByXuLyDonID(int xulydonID)
        {

            List<TiepDanKhongDonInfo> tiepdanList = new List<TiepDanKhongDonInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDON_ID,SqlDbType.Int)
            };
            parameters[0].Value = xulydonID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_XLD_ID, parameters))
                {

                    while (dr.Read())
                    {
                        TiepDanKhongDonInfo info = GetData(dr);
                        tiepdanList.Add(info);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return tiepdanList;
        }

        public IList<TiepDanKhongDonInfo> GetInTime(DateTime startDate, DateTime endDate, int coquanID)
        {
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_CO_QUAN_ID, SqlDbType.Int)
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            parm[2].Value = coquanID;
            IList<TiepDanKhongDonInfo> LsTDKD = new List<TiepDanKhongDonInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_IN_TIME, parm))
                {

                    while (dr.Read())
                    {

                        TiepDanKhongDonInfo TDKDInfo = GetDataForShowManage(dr);
                        TDKDInfo.CanBoTiepID = Utils.ConvertToInt32(dr["CanBoTiepID"], 0);
                        TDKDInfo.DonThuID = Utils.ConvertToInt32(dr["DonThuID"], 0);
                        TDKDInfo.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
                        TDKDInfo.LanTiep = Utils.ConvertToInt32(dr["LanTiep"], 0);
                        TDKDInfo.KetQuaTiep = Utils.ConvertToString(dr["KetQuaTiep"], string.Empty);
                        TDKDInfo.SoDon = Utils.ConvertToString(dr["SoDonThu"], string.Empty);
                        TDKDInfo.SoDonThu = Utils.ConvertToString(dr["SoDonThu"], string.Empty);
                        TDKDInfo.HoTen = Utils.ConvertToString(dr["HoTen"], string.Empty);
                        TDKDInfo.TenChuDon = Utils.ConvertToString(dr["HoTens"], string.Empty);
                        TDKDInfo.DiaChi = Utils.ConvertToString(dr["DiaChiCT"], string.Empty);
                        TDKDInfo.TenLoaiKhieuTo = Utils.ConvertToString(dr["TenLoaiKhieuTo"], string.Empty);
                        TDKDInfo.LoaiKhieuTo1ID = Utils.ConvertToInt32(dr["LoaiKhieuTo1ID"], 0);
                        if (TDKDInfo.NgayTiep != DateTime.MinValue)
                        {
                            TDKDInfo.NgayTiepStr = Format.FormatDate(TDKDInfo.NgayTiep);
                            TDKDInfo.NgayNhapDonStr = Format.FormatDate(TDKDInfo.NgayTiep);
                        }
                        LsTDKD.Add(TDKDInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return LsTDKD;
        }


        public IList<TiepDanKhongDonInfo> GetAll()
        {
            IList<TiepDanKhongDonInfo> LsTDKD = new List<TiepDanKhongDonInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_ALL_MANAGE, null))
                {

                    while (dr.Read())
                    {

                        TiepDanKhongDonInfo TDKDInfo = GetDataForShowManage(dr);
                        LsTDKD.Add(TDKDInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return LsTDKD;
        }


        public int CountAll_Manage()
        {
            object result;

            //return result;

            //quanghv
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {

                        result = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, COUNT_ALL_MANAGE, null);
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

            return Utils.ConvertToInt32(result, 0);
        }

        public IList<TiepDanKhongDonInfo> GetByInSoTiepDan(string keyword, int lktID, DateTime tuNgay, DateTime denNgay, int coquanID, int loaiCanBoTiep, int loaiRutDon, int canBoID)
        {
            IList<TiepDanKhongDonInfo> tiepdaninfo = new List<TiepDanKhongDonInfo>();

            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_KEY, SqlDbType.NVarChar, 50),
                new SqlParameter(PARAM_LOAIKHIEUTO_ID, SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_CO_QUAN_ID, SqlDbType.Int),
                new SqlParameter(PARAM_LOAICANBOTIEP, SqlDbType.Int),
                new SqlParameter(@"LoaiRutDon", SqlDbType.Int),
            };

            parameters[0].Value = keyword;
            parameters[1].Value = lktID;
            parameters[2].Value = tuNgay;
            parameters[3].Value = denNgay;
            parameters[4].Value = coquanID;
            parameters[5].Value = loaiCanBoTiep;
            parameters[6].Value = loaiRutDon;
            if (tuNgay == DateTime.MinValue) parameters[2].Value = DBNull.Value;
            if (denNgay == DateTime.MinValue) parameters[3].Value = DBNull.Value;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_INSO, parameters))
                {
                    while (dr.Read())
                    {
                        TiepDanKhongDonInfo Info = GetDataForShowManage(dr);
                        Info.KetQuaTiep = Utils.GetString(dr["KetQuaTiep"], string.Empty);
                        Info.TenLoaiKhieuTo = Utils.GetString(dr["TenLoaiKhieuTo"], string.Empty);
                        Info.TenCQDaGiaiQuyet = Utils.GetString(dr["TenCQDaGiaiQuyet"], string.Empty);
                        Info.HuongGiaiQuyetID = Utils.GetInt32(dr["HuongGiaiQuyetID"], 0);
                        Info.SoNguoi = Utils.ConvertToInt32(dr["SoLuong"], 0);
                        Info.SoDon = Utils.GetString(dr["SoDon"], string.Empty);
                        Info.HoTen = Utils.GetString(dr["HoTen"], string.Empty);

                        if (Info.HuongGiaiQuyetID == 0 && Info.XuLyDonID == 0)
                            Info.HuongGiaiQuyetID = (int)HuongGiaiQuyetEnum.HuongDan;
                        Info.TenCanBoTiep = Utils.ConvertToString(dr["TenLanhDaoTiep"].ToString(), string.Empty);
                        Info.HuongXuLy = Utils.ConvertToString(dr["TenHuongGiaiQuyet"], String.Empty);

                        tiepdaninfo.Add(Info);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return tiepdaninfo;
        }
        private TiepDanKhongDonInfo GetFileData(SqlDataReader rdr)
        {
            TiepDanKhongDonInfo info = new TiepDanKhongDonInfo();
            info.TenFile = Utils.ConvertToString(rdr["TenFile"], String.Empty);
            info.FileUrl = Utils.ConvertToString(rdr["FileURL"], String.Empty);
            info.IsBaoMat = Utils.ConvertToBoolean(rdr["IsBaoMat"], false);
            info.NguoiUp = Utils.ConvertToInt32(rdr["NguoiUp"], 0);
            return info;
        }
        public List<TiepDanKhongDonInfo> GetFileSoTiepDan(int XuLyDonID)
        {
            List<TiepDanKhongDonInfo> ListInfo = new List<TiepDanKhongDonInfo>();

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_XULYDONID, SqlDbType.Int),
            };
            parms[0].Value = XuLyDonID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_FILESOTIEPDAN_LIST, parms))
                {
                    while (dr.Read())
                    {
                        TiepDanKhongDonInfo Info = new TiepDanKhongDonInfo();
                        Info = GetFileData(dr);
                        ListInfo.Add(Info);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }

            return ListInfo;
        }

        public IList<TiepDanKhongDonInfo> GetBySearch(ref int TotalRow, string keyword, int lktID, DateTime tuNgay, DateTime denNgay, int coquanID, int page, int start, int end, int loaiCanBoTiep, int loaiRutDon, int canBoID, int HuongXuLyID, int LoaiTiepDanID)
        {
            IList<TiepDanKhongDonInfo> tiepdaninfo = new List<TiepDanKhongDonInfo>();

            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_KEY, SqlDbType.NVarChar, 50),
                new SqlParameter(PARAM_LOAIKHIEUTO_ID, SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_CO_QUAN_ID, SqlDbType.Int),
                new SqlParameter(PARAM_START, SqlDbType.Int),
                new SqlParameter(PARAM_END, SqlDbType.Int),
                new SqlParameter(PARAM_LOAICANBOTIEP, SqlDbType.Int),
                new SqlParameter(@"LoaiRutDon", SqlDbType.Int),
                new SqlParameter(@"HuongXuLyID", SqlDbType.Int),
                new SqlParameter(@"TotalRow",SqlDbType.Int),
                new SqlParameter(@"LoaiTiepDanID",SqlDbType.Int),
            };

            parameters[0].Value = keyword;
            parameters[1].Value = lktID;
            parameters[2].Value = tuNgay;
            parameters[3].Value = denNgay;
            parameters[4].Value = coquanID;
            parameters[5].Value = start;
            parameters[6].Value = end;
            parameters[7].Value = loaiCanBoTiep;
            parameters[8].Value = loaiRutDon;
            parameters[9].Value = HuongXuLyID;
            parameters[10].Direction = ParameterDirection.Output;
            parameters[10].Size = 8;
            parameters[11].Value = LoaiTiepDanID;
            if (tuNgay == DateTime.MinValue) parameters[2].Value = DBNull.Value;
            if (denNgay == DateTime.MinValue) parameters[3].Value = DBNull.Value;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_NV_TiepDanKhongDon_GetBySearch_New2", parameters))
                {
                    while (dr.Read())
                    {
                        TiepDanKhongDonInfo Info = GetDataForShowManage(dr);
                        Info.CanBoXuLyID = Utils.ConvertToInt32(dr["CanBoXuLyID"], 0);
                        Info.CanBoTiepNhanID = Utils.ConvertToInt32(dr["CanBoTiepNhapID"], 0);
                        Info.LoaiTiepDanID = Utils.ConvertToInt32(dr["LoaiTiepDanID"], 0);
                        Info.TenLoaiTiepDan = (Info.LoaiTiepDanID ?? 0) != 0 ? (EnumExtensions.GetEnumValue<EnumLoaiTiepDan>(Info.LoaiTiepDanID ?? 0)).GetDescription() : "";
                        Info.KetQuaTiep = Utils.GetString(dr["KetQuaTiep"], string.Empty);
                        Info.CQDaGiaiQuyetID = Utils.GetString(dr["CQDaGiaiQuyetID"], string.Empty);
                        Info.HuongGiaiQuyetID = Utils.GetInt32(dr["HuongGiaiQuyetID"], 0);
                        //if (Info.HuongGiaiQuyetID == 0 && Info.XuLyDonID == 0)
                        //    Info.HuongGiaiQuyetID = (int)HuongGiaiQuyetEnum.HuongDan;
                        Info.SoLuong = Utils.GetInt32(dr["SoLuong"], 0);
                        Info.TenLoaiKhieuTo = Utils.GetString(dr["TenLoaiKhieuTo"], string.Empty);
                        Info.LanTiep = Utils.GetInt32(dr["LanTiep"], 0);
                        Info.SoDonThu = Utils.GetString(dr["SoDonThu"], string.Empty);
                        Info.SoDon = Utils.GetString(dr["SoDon"], string.Empty);
                        Info.HoTen = Utils.GetString(dr["HoTen"], string.Empty);
                        Info.TenCanBoTiep = Utils.ConvertToString(dr["TenLanhDaoTiep"].ToString(), string.Empty);
                        Info.HuongXuLy = Utils.ConvertToString(dr["TenHuongGiaiQuyet"], String.Empty);
                        Info.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"], 0);
                        Info.NguonDonDen = Utils.ConvertToInt32(dr["NguonDonDen"], 0);

                        if (Info.NguonDonDen == (int)EnumNguonDonDen.TrucTiep)
                        {
                            Info.TenNguonDonDen = Constant.NguonDon_TrucTieps;
                        }
                        if (Info.NguonDonDen == (int)EnumNguonDonDen.CoQuanKhac)
                        {
                            Info.TenNguonDonDen = Constant.NguonDon_CoQuanKhacs;
                        }
                        if (Info.NguonDonDen == (int)EnumNguonDonDen.BuuChinh)
                        {
                            Info.TenNguonDonDen = Constant.NguonDon_BuuChinhs;
                        }
                        if (Info.NguonDonDen == (int)EnumNguonDonDen.TraoTay)
                        {
                            Info.TenNguonDonDen = Constant.NguonDon_TraoTays;
                        }

                        Info.DanKhongDenID = Utils.ConvertToInt32(dr["DanKhongDenID"], 0);
                        if (Info.DanKhongDenID > 0 && Info.TiepDanKhongDonID == 0)
                        {
                            Info.TiepDanKhongDonID = 1000000000 + Info.DanKhongDenID.Value;
                        }
                        //List<TiepDanKhongDonInfo> fileDinhKem = new List<TiepDanKhongDonInfo>();
                        //CanBoInfo canBoInfo = new CanBo().GetCanBoByID(canBoID);
                        //if (canBoInfo.XemTaiLieuMat)
                        //{
                        //    fileDinhKem = GetFileSoTiepDan(Info.XuLyDonID).ToList();
                        //}
                        //else
                        //{
                        //    fileDinhKem = GetFileSoTiepDan(Info.XuLyDonID).Where(x => x.IsBaoMat != true || x.NguoiUp == canBoID).ToList();
                        //}

                        //int step = 0;
                        //for (int i = 0; i < fileDinhKem.Count; i++)
                        //{
                        //    if (!string.IsNullOrEmpty(fileDinhKem[i].FileUrl))
                        //    {
                        //        if (string.IsNullOrEmpty(fileDinhKem[i].TenFile))
                        //        {
                        //            string[] arrtenFile = fileDinhKem[i].FileUrl.Split('/');
                        //            if (arrtenFile.Length > 0)
                        //            {
                        //                string[] duoiFile = arrtenFile[arrtenFile.Length - 1].Split('_');
                        //                if (duoiFile.Length > 0)
                        //                {
                        //                    fileDinhKem[i].TenFile = duoiFile[duoiFile.Length - 1];
                        //                }
                        //                else
                        //                {
                        //                    fileDinhKem[i].TenFile = arrtenFile[arrtenFile.Length - 1];
                        //                }
                        //            }
                        //        }
                        //        fileDinhKem[i].FileUrl = fileDinhKem[i].FileUrl.Replace(" ", "%20");
                        //    }
                        //    step++;
                        //    if (fileDinhKem[i].IsBaoMat == false)
                        //    {
                        //        string sec_false = "<li style='margin-bottom: 5px;'>" + step + ". " + "<a href='" + fileDinhKem[i].FileUrl + "' download>" + fileDinhKem[i].TenFile + "</a></li>";
                        //        Info.HuongXuLy = "<div style='margin-bottom: 5px;'><span>" + Info.HuongXuLy + "</span></div>" + "<ul>" + sec_false + "</ul>";
                        //    }
                        //    else
                        //    {
                        //        string sec_true = "<li style='margin-bottom: 5px;'>" + step + ". " + "<a style='color: #EE7600;' href=" + "/DownloadFileDonThuCT.aspx?url=" + fileDinhKem[i].FileUrl + ">" + fileDinhKem[i].TenFile + "</a></li>";
                        //        Info.HuongXuLy = "<div style='margin-bottom: 5px;'><span>" + Info.HuongXuLy + "</span></div>" + "<ul>" + sec_true + "</ul>";
                        //    }
                        //}
                        Info.ThanhPhanThamGia = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().GetThanhPhanThamGia(Info.TiepDanKhongDonID).ToList();
                        if (Info.NhomKNID > 0)
                        {
                            Info.NhomKN = new NhomKN().GetByID(Info.NhomKNID);
                            Info.listDoiTuongKN = new DoiTuongKN().GetByNhomKNID(Info.NhomKNID).ToList();
                        }
                        // bổ sung rút đơn ID
                        Info.RutDonID = Utils.ConvertToInt32(dr["RutDonID"], 0);
                        tiepdaninfo.Add(Info);
                    }
                    dr.Close();
                }
                TotalRow = Utils.ConvertToInt32(parameters[10].Value, 0);
            }
            catch
            {

                throw;
            }
            return tiepdaninfo;
        }

        public int CountSearch(string keyword, int lktID, DateTime tuNgay, DateTime denNgay, int coquanID, int loaiCanBoTiep, int loaiRutDon)
        {

            object result = 0;

            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_KEY, SqlDbType.NVarChar, 50),
                new SqlParameter(PARAM_LOAIKHIEUTO_ID, SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_CO_QUAN_ID, SqlDbType.Int),
                new SqlParameter(PARAM_LOAICANBOTIEP, SqlDbType.Int),
                new SqlParameter(@"LoaiRutDon", SqlDbType.Int)
            };

            parameters[0].Value = keyword;
            parameters[1].Value = lktID;
            parameters[2].Value = tuNgay;
            parameters[3].Value = denNgay;
            parameters[4].Value = coquanID;
            parameters[5].Value = loaiCanBoTiep;
            parameters[6].Value = loaiRutDon;
            if (tuNgay == DateTime.MinValue)
            {
                parameters[2].Value = DBNull.Value;

            }
            if (denNgay == DateTime.MinValue)
            {
                parameters[3].Value = DBNull.Value;

            }

            try
            {
                result = SQLHelper.ExecuteScalar(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_SEARCH_MANAGE, parameters);

            }
            catch
            {

                throw;
            }
            return Convert.ToInt32(result);
        }

        public int Delete_Manage(int TDKD_ID)
        {

            int val = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_TIEP_DAN_KHONG_DON_ID,SqlDbType.Int)
            };
            parameters[0].Value = TDKD_ID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, DELETE_MANAGE, parameters);
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

        //quanghv
        private TiepDanKhongDonInfo GetData(SqlDataReader dr)
        {
            TiepDanKhongDonInfo info = new TiepDanKhongDonInfo();

            info.TiepDanKhongDonID = Utils.GetInt32(dr["TiepDanKhongDonID"], 0);
            info.NgayTiep = Utils.GetDateTime(dr["NgayTiep"], Constant.DEFAULT_DATE);
            info.NoiDungTiep = Utils.GetString(dr["NoiDungTiep"], string.Empty);
            info.VuViecCu = Utils.GetBoolean(dr["VuViecCu"], false);
            info.TrangThaiVuViec = info.VuViecCu ? "Cũ" : "Mới";
            info.CanBoTiepID = Utils.GetInt32(dr["CanBoTiepID"], 0);
            info.DonThuID = Utils.GetInt32(dr["DonThuID"], 0);
            info.CoQuanID = Utils.GetInt32(dr["CoQuanID"], 0);
            info.GapLanhDao = Utils.GetBoolean(dr["GapLanhDao"], false);

            return info;
        }

        private TiepDanKhongDonInfo GetDataVuViec(SqlDataReader dr)
        {
            TiepDanKhongDonInfo info = new TiepDanKhongDonInfo();

            info.TiepDanKhongDonID = Utils.GetInt32(dr["TiepDanKhongDonID"], 0);
            info.NgayTiep = Utils.GetDateTime(dr["NgayTiep"], Constant.DEFAULT_DATE);
            info.NoiDungTiep = Utils.GetString(dr["NoiDungTiep"], string.Empty);
            //info.VuViecCu = Utils.GetBoolean(dr["VuViecCu"], false);
            info.TenCanBoTiep = Utils.GetString(dr["TenCanBoTiep"], string.Empty);
            info.DonThuID = Utils.GetInt32(dr["DonThuID"], 0);
            info.NhomKNID = Utils.GetInt32(dr["NhomKNID"], 0);
            info.TenLoaiDoiTuong = Utils.GetString(dr["TenLoaiDoiTuong"], string.Empty);
            //info.HoTen = Utils.ConvertToString(dr["HoTen"], string.Empty);
            //info.CMND = Utils.GetString(dr["CMND"], string.Empty);
            //info.DiaChi = Utils.GetString(dr["DiaChi"], string.Empty);

            return info;
        }

        private TiepDanKhongDonInfo GetDataForShow(SqlDataReader dr)
        {
            TiepDanKhongDonInfo DTInfo = new TiepDanKhongDonInfo();
            DTInfo.DonThuID = Utils.GetInt32(dr["DonThuID"], 0);

            //DTInfo.NgayNhapDon = Utils.GetDateTime(dr["NgayNhapDon"], Constant.DEFAULT_DATE);

            //DTInfo.NoiDungDon = Utils.GetString(dr["NoiDungDon"], string.Empty);


            //DTInfo.HoTen = Utils.GetString(dr["HoTen"].ToString(), String.Empty);
            //DTInfo.DiaChiCT = Utils.GetString(dr["DiaChiCT"].ToString(), String.Empty);

            //DTInfo.TenLoaiKhieuTo = Utils.GetString(dr["TenLoaiKhieuTo"].ToString(), String.Empty);
            //DTInfo.TenCanBo = Utils.GetString(dr["TenCanBo"], String.Empty);
            //DTInfo.TenCoQuan = Utils.GetString(dr["TenCoQuan"], String.Empty);
            //DTInfo.TenXa = Utils.GetString(dr["TenXa"], String.Empty);
            //DTInfo.TenHuyen = Utils.GetString(dr["TenHuyen"], String.Empty);
            //DTInfo.TenTinh = Utils.GetString(dr["TenTinh"], String.Empty);
            //DTInfo.SoLuong = Utils.GetInt32(dr["SoLuong"], 0);


            return DTInfo;
        }

        //Get Insert Parmas
        private SqlParameter[] GetInsertParms()
        {
            SqlParameter[] parms = new SqlParameter[]{
                    //new SqlParameter(PARAM_TIEPDANKHONGDON_ID, SqlDbType.Int),
                    new SqlParameter(PARAM_NGAYTIEP, SqlDbType.DateTime),
                    new SqlParameter(PARAM_NOIDUNGTIEP, SqlDbType.NVarChar),
                    new SqlParameter(PARAM_VUVIECCU, SqlDbType.Bit),
                    new SqlParameter(PARAM_CANBOTIEP_ID, SqlDbType.Int),
                    new SqlParameter(PARAM_DONTHU_ID, SqlDbType.Int),
                    new SqlParameter(PARAM_CO_QUAN_ID, SqlDbType.Int),
                    new SqlParameter(PARAM_GAPLANHDAO, SqlDbType.Bit),
                    new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int)

                };
            return parms;
        }

        //SET iNSERT PARMS
        private void SetInsertParms(SqlParameter[] parms, TiepDanKhongDonInfo DTInfo)
        {
            parms[0].Value = DTInfo.NgayTiep;
            parms[1].Value = DTInfo.NoiDungTiep;
            parms[2].Value = DTInfo.VuViecCu;
            parms[3].Value = DTInfo.CanBoTiepID;
            parms[4].Value = DTInfo.DonThuID;
            parms[5].Value = DTInfo.CoQuanID;
            parms[6].Value = DTInfo.GapLanhDao;
            parms[7].Value = DTInfo.XuLyDonID;
        }

        //get update parms
        private SqlParameter[] GetUpdateParms()
        {
            SqlParameter[] parms = new SqlParameter[]{
                        new SqlParameter(PARAM_TIEPDANKHONGDON_ID, SqlDbType.Int),
                        new SqlParameter(PARAM_NGAYTIEP, SqlDbType.DateTime),
                        new SqlParameter(PARAM_NOIDUNGTIEP, SqlDbType.NVarChar),
                        new SqlParameter(PARAM_VUVIECCU, SqlDbType.Bit),
                        new SqlParameter(PARAM_CANBOTIEP_ID, SqlDbType.Int),
                        new SqlParameter(PARAM_DONTHU_ID, SqlDbType.Int),
                        new SqlParameter(PARAM_GAPLANHDAO, SqlDbType.Bit),
                        new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int)
            };
            return parms;
        }

        //set update parms
        private void SetUpdateParms(SqlParameter[] parms, TiepDanKhongDonInfo DTInfo)
        {

            parms[0].Value = DTInfo.TiepDanKhongDonID;
            parms[1].Value = DTInfo.NgayTiep;
            parms[2].Value = DTInfo.NoiDungTiep;
            parms[3].Value = DTInfo.VuViecCu;
            parms[4].Value = DTInfo.CanBoTiepID;
            parms[5].Value = DTInfo.DonThuID;
            parms[6].Value = DTInfo.GapLanhDao;
            parms[7].Value = DTInfo.XuLyDonID;
        }

        //public IList<TiepDanKhongDonInfo> GetAll()
        //{
        //    IList<TiepDanKhongDonInfo> ListDT = new List<TiepDanKhongDonInfo>();
        //    try
        //    {
        //        using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_ALL, null))
        //        {

        //            while (dr.Read())
        //            {

        //                TiepDanKhongDonInfo DTInfo = GetData(dr);
        //                ListDT.Add(DTInfo);
        //            }
        //            dr.Close();
        //        }
        //    }
        //    catch
        //    {
        //    }
        //    return ListDT;
        //}

        public IList<TiepDanKhongDonInfo> GetListVuViecMoi(int coquanID, int page, string searchKey)
        {
            IList<TiepDanKhongDonInfo> ListDT = new List<TiepDanKhongDonInfo>();

            TiepDanKhongDonInfo info = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_CO_QUAN_ID,SqlDbType.Int),
                //new SqlParameter(PARAM_PAGE,SqlDbType.Int),
                //new SqlParameter(PARAM_LIMIT,SqlDbType.Int),
                new SqlParameter(PARAM_START,SqlDbType.Int),
                new SqlParameter(PARAM_END,SqlDbType.Int),
                new SqlParameter(PARAM_SEARCHKEY,SqlDbType.NVarChar, 20)
            };
            parameters[0].Value = coquanID;
            //parameters[1].Value = page;
            //parameters[2].Value = Constant.PageSize;
            parameters[1].Value = (page - 1) * Constant.PageSize;
            parameters[2].Value = page * Constant.PageSize;
            parameters[3].Value = searchKey;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_LIST_VUVIEC, parameters))
                {

                    while (dr.Read())
                    {

                        info = GetDataVuViec(dr);
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

        public int GetTotalVuViecMoi(int coquanID, string searchKey)
        {
            object val = 0;

            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_CO_QUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_SEARCHKEY,SqlDbType.NVarChar, 20)
            };
            parameters[0].Value = coquanID;
            parameters[1].Value = searchKey;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {

                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, GET_TOTAL_VUVIEC, parameters);
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

        public TiepDanKhongDonInfo GetByID(int xulydonID)
        {

            TiepDanKhongDonInfo DTInfo = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_TIEPDANKHONGDON_ID,SqlDbType.Int)
            };
            parameters[0].Value = xulydonID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_ID, parameters))
                {

                    if (dr.Read())
                    {
                        DTInfo = GetData(dr);
                        DTInfo.LoaiKhieuTo1ID = Utils.GetInt32(dr["LoaiKhieuTo1ID"], 0);
                        DTInfo.LoaiKhieuTo2ID = Utils.GetInt32(dr["LoaiKhieuTo2ID"], 0);
                        DTInfo.LoaiKhieuTo3ID = Utils.GetInt32(dr["LoaiKhieuTo3ID"], 0);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return DTInfo;
        }

        public TiepDanKhongDonInfo GetByNhomKNID(int NhomKNID)
        {

            TiepDanKhongDonInfo DTInfo = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_NHOMKNID,SqlDbType.Int)
            };
            parameters[0].Value = NhomKNID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_TIEPDANKHONGDON_BYNHOMKNID, parameters))
                {

                    if (dr.Read())
                    {
                        DTInfo = GetData(dr);
                        DTInfo.TenCanBoTiep = Utils.GetString(dr["CanBoTiepNhan"], string.Empty);
                        DTInfo.LoaiKhieuTo1ID = Utils.GetInt32(dr["LoaiKhieuTo1ID"], 0);
                        DTInfo.LoaiKhieuTo2ID = Utils.GetInt32(dr["LoaiKhieuTo2ID"], 0);
                        DTInfo.LoaiKhieuTo3ID = Utils.GetInt32(dr["LoaiKhieuTo3ID"], 0);
                        DTInfo.TenLoaiKhieuTo1 = Utils.ConvertToString(dr["TenLoaiKhieuTo1"], String.Empty);
                        DTInfo.TenLoaiKhieuTo2 = Utils.ConvertToString(dr["TenLoaiKhieuTo2"], String.Empty);
                        DTInfo.TenLoaiKhieuTo3 = Utils.ConvertToString(dr["TenLoaiKhieuTo3"], String.Empty);
                        DTInfo.SoDon = Utils.GetString(dr["SoDon"], string.Empty);
                        DTInfo.NoiDungDon = Utils.GetString(dr["NoiDungDon"], string.Empty);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return DTInfo;
        }


        //lay xulydon info theo don id
        public TiepDanKhongDonInfo GetByDonID(int donthuID)
        {

            TiepDanKhongDonInfo DTInfo = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_DONTHU_ID,SqlDbType.Int)
            };
            parameters[0].Value = donthuID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_DONTHU_ID, parameters))
                {

                    if (dr.Read())
                    {
                        DTInfo = GetData(dr);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return DTInfo;
        }

        public Boolean CheckVuViecMoi(int donthuID)
        {
            Boolean result = false;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_DONTHU_ID,SqlDbType.Int)
            };
            parameters[0].Value = donthuID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {

                        result = Utils.ConvertToInt32(SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, CHECK_VUVIECMOI, parameters), 0) > 0 ? true : false;
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
                conn.Close();
                //using (SqlDataReader dr = SQLHelper.ExecuteScalar(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, CHECK_VUVIECMOI, parameters))
                //{

                //    if (dr.Read())
                //    {
                //        DTInfo = GetData(dr);
                //    }
                //    dr.Close();
                //}
            }
            return result;
        }

        //-----------delete----------------
        public int Delete(int DT_ID)
        {

            int val = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_TIEPDANKHONGDON_ID,SqlDbType.Int)
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
        public int Update(TiepDanKhongDonInfo DTInfo)
        {

            object val = null;
            SqlParameter[] parameters = GetUpdateParms();
            SetUpdateParms(parameters, DTInfo);

            //string stored = UPDATE;
            //if (opt != "vuvieccu")
            //   stored = UPDATE_VUVIECCU;


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
            return Utils.ConvertToInt32(val, 0); // tra ve so don thu
        }


        //------------------INSERT-----------------
        public int Insert(TiepDanKhongDonInfo DTInfo)
        {

            object val = null;

            SqlParameter[] parameters = GetInsertParms();
            SetInsertParms(parameters, DTInfo);

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

        /*
         * quanghv
         * 
         **/

        //lay so don thu theo co quan ID
        //public int getSoDonThu(int coQuanID)
        //{
        //    object val = null;
        //    SqlParameter[] parameters = new SqlParameter[]{
        //        new SqlParameter(PARAM_CO_QUAN_ID,SqlDbType.Int)
        //    };
        //    parameters[0].Value = coQuanID;

        //    using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
        //    {

        //        conn.Open();
        //        using (SqlTransaction trans = conn.BeginTransaction())
        //        {

        //            try
        //            {
        //                val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, GET_SO_DON_BY_CO_QUAN, parameters);
        //                trans.Commit();
        //            }
        //            catch
        //            {
        //                trans.Rollback();
        //                throw;
        //            }
        //        }
        //        conn.Close();
        //    }
        //    return Utils.ConvertToInt32(val, 0);
        //}

        /**
         * end quanghv
         * */

        /*
         * trinm
         * */
        //public IList<TiepDanKhongDonInfo> GetByPage(int page)
        //{
        //    int start = (page - 1) * Constant.PageSize;
        //    int end = page * Constant.PageSize;

        //    IList<TiepDanKhongDonInfo> ls_donthu = new List<TiepDanKhongDonInfo>();
        //    SqlParameter[] parameters = new SqlParameter[]{
        //        new SqlParameter(PARAM_START,SqlDbType.Int),
        //        new SqlParameter(PARAM_END,SqlDbType.Int)
        //    };
        //    parameters[0].Value = start;
        //    parameters[1].Value = end;
        //    try
        //    {
        //        using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_PAGE, parameters))
        //        {
        //            while (dr.Read())
        //            {
        //                TiepDanKhongDonInfo DTInfo = GetDataForShow(dr);
        //                ls_donthu.Add(DTInfo);
        //            }
        //            dr.Close();
        //        }
        //    }
        //    catch
        //    {

        //        throw;
        //    }
        //    return ls_donthu;
        //}

        //----------------------COUNT ALL----------------------
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

        public IList<TiepDanKhongDonInfo> GetDSGapLanhDao(DateTime tuNgay, DateTime denNgay, int coQuanID)
        {
            IList<TiepDanKhongDonInfo> tiepdaninfo = new List<TiepDanKhongDonInfo>();

            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_CO_QUAN_ID, SqlDbType.Int),
            };
            parameters[0].Value = tuNgay;
            parameters[1].Value = denNgay;
            parameters[2].Value = coQuanID;
            if (tuNgay == DateTime.MinValue) parameters[0].Value = DBNull.Value;
            if (denNgay == DateTime.MinValue) parameters[1].Value = DBNull.Value;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DS_GAP_LANH_DAO, parameters))
                {
                    while (dr.Read())
                    {
                        TiepDanKhongDonInfo Info = GetDataGapLanhDao(dr);
                        tiepdaninfo.Add(Info);
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return tiepdaninfo;
        }

        public int THSDPhanMem_CountDonThuDaTiepNhan(DateTime tuNgay, DateTime denNgay, int coquanID)
        {
            object result = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_CO_QUAN_ID, SqlDbType.Int)
            };
            parameters[0].Value = tuNgay;
            parameters[1].Value = denNgay;
            parameters[2].Value = coquanID;
            if (tuNgay == DateTime.MinValue)
            {
                parameters[0].Value = DBNull.Value;
            }
            if (denNgay == DateTime.MinValue)
            {
                parameters[1].Value = DBNull.Value;
            }
            try
            {
                result = SQLHelper.ExecuteScalar(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, THSDPHANMEM_COUNTDONTHUDATIEP, parameters);
            }
            catch
            {
                throw;
            }
            return Convert.ToInt32(result);
        }
        //CuongLB  thêm get data khiếu kiện đông người
        public IList<TiepDanKhongDonInfo> GetKhieuKienDongNguoi(DateTime tuNgay, DateTime denNgay)
        {
            IList<TiepDanKhongDonInfo> tiepdaninfo = new List<TiepDanKhongDonInfo>();

            SqlParameter[] parameters = new SqlParameter[]{

                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),

            };

            parameters[0].Value = tuNgay;
            parameters[1].Value = denNgay;


            if (tuNgay == DateTime.MinValue) parameters[0].Value = DBNull.Value;
            if (denNgay == DateTime.MinValue) parameters[1].Value = DBNull.Value;
            //TiepDanKhongDonInfo infoTong = new TiepDanKhongDonInfo();
            //infoTong.NgayNhapDonStr = "0";
            //infoTong.NgayXuLyStr = "";
            //infoTong.SoLuong = 0;
            //infoTong.SoNguoi = 0;
            //infoTong.HoTen = "<center><b>Tổng</b></center>";
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_KHIEUKIENDONGNGUOI, parameters))
                {
                    //int dem = 1;
                    while (dr.Read())
                    {
                        TiepDanKhongDonInfo Info = new TiepDanKhongDonInfo();
                        Info.NgayXuLyStr = Format.FormatDate(Utils.GetDateTime(dr["NgayXuLy"], DateTime.MinValue));
                        Info.NgayNhapDonStr = Format.FormatDate(Utils.GetDateTime(dr["NgayNhapDon"], DateTime.MinValue));
                        Info.SoLuong = Utils.GetInt32(dr["SoLuong"], 0);
                        Info.SoNguoi = Utils.GetInt32(dr["SoNguoiDaiDien"], 0);
                        Info.HoTen = Utils.GetString(dr["HoTen"], string.Empty);
                        tiepdaninfo.Add(Info);
                        //infoTong.SoLuong += Info.SoLuong;
                        //infoTong.SoNguoi += Info.SoNguoi;
                        //infoTong.NgayNhapDonStr = dem.ToString();
                        //dem++;
                    }

                    dr.Close();
                    //tiepdaninfo.Add(infoTong);
                }
            }
            catch
            {

                throw;
            }
            return tiepdaninfo;
        }
        // Tổng số đơn tiếp trực tiếp
        public int BC_TongHopTHTiepDan_GetTongSoDon_ByLoaiKhieuTo(DateTime tuNgay, DateTime denNgay, int coquanID, int loaiKhieuTo)
        {
            object result = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_CO_QUAN_ID, SqlDbType.Int),
                new SqlParameter(PARM_LOAIKHIEUTO, SqlDbType.Int)
            };
            parameters[0].Value = tuNgay;
            parameters[1].Value = denNgay;
            if (tuNgay == DateTime.MinValue)
            {
                parameters[0].Value = DBNull.Value;
            }
            if (denNgay == DateTime.MinValue)
            {
                parameters[1].Value = DBNull.Value;
            }
            parameters[2].Value = coquanID;
            parameters[3].Value = loaiKhieuTo;
            try
            {
                result = SQLHelper.ExecuteScalar(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GETTONGSODONBYLOAIKHIEUTO, parameters);
            }
            catch
            {
                throw;
            }
            return Convert.ToInt32(result);
        }
    }
}
