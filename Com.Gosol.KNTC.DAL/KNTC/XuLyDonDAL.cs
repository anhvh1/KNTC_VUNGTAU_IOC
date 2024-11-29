using Com.Gosol.KNTC.Models;
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
    public class XuLyDonDAL
    {
        //Su dung de goi StoreProcedure
        #region store
        private const string GET_ALL = @"DonThu_GetAll";
        private const string GET_BY_ID = @"NV_XuLyDon_GetByID";
        private const string GETDONTHU_BY_ID_FORPORTAL = @"NV_DonThu_GetByID";
        private const string GET_XULYDON_BY_SODONTHU = @"NV_XuLyDon_GetBySoDonThu";
        private const string GET_JOINTIEPDAN_BY_ID = @"NV_XuLyDon_GetJoinTiepDanByID";
        private const string GET_BY_DONTHU_ID = @"NV_XuLyDon_GetByDonID";
        private const string INSERT = @"NV_XuLyDon_Insert";
        private const string INSERT_STEP1 = @"NV_XuLyDon_InsertStep1";
        private const string UPDATE = @"NV_XuLyDon_Update";
        private const string UPDATE_NEW = @"NV_XuLyDon_Update_New";
        private const string UPDATE_STEP1 = @"NV_XuLyDon_UpdateStep1";
        private const string UPDATE_STEP2 = @"NV_XuLyDon_UpdateStep2";
        private const string UPDATE_STEP3 = @"NV_XuLyDon_UpdateStep3";
        private const string UPDATE_STEP4 = @"NV_XuLyDon_UpdateStep4";
        private const string DELETE = @"DonThu_Delete";
        private const string DELETE_DTDATIEPNHAN = @"DonThu_Delete_DTDaTiepNhan";
        private const string GET_DONTHU_BY_DATECREATED = @"NV_DonThu_GetByDateCreated";

        private const string UPDATE_HXL = @"NV_XuLyDon_Update_HuongXL";

        private const string UPDATE_TRANGTHAI = "XuLyDon_UpdateTrangThai";
        private const string CHECK_ISHUONGGIAIQUYET = @"XuLyDon_CheckIsHuongGiaiQuyet";
        private const string UPDATE_NGAYTHULY = @"XuLyDon_UpdateNgayThuLy";
        private const string UPDATE_CHUYENDON = @"XuLyDon_UpdateChuyenDon";
        private const string UPDATE_COQUANGIAIQUYET = @"XuLyDon_UpdateCoQuanGQ";
        private const string UPDATE_CANBO_TIEPNHAN = @"XuLyDon_UpdateCanBoTiepNhan";
        private const string UPDATE_CQNHAN_VBDONDOC = @"XuLyDon_UpDateVBDonDoc";
        private const string UPDATE_CQNHAN_DONCHUYEN = @"XuLyDon_UpDateCQNhanDonChuyen";
        private const string UPDATE_NGAYQUAHAN = @"XuLyDon_UpdateNgayQuaHan";
        private const string UPDATE_CANBOXULY = @"XuLyDon_Update_CanBoXuLy";
        private const string UPDATE_CBDUOC_CHONXL = @"XuLyDon_Update_CBDuocChonXL";

        private const string INSERT_YKIENXL = @"YKienXuLy_InSert";
        private const string GET_ALL_YKIENXL = @"YKienXuLy_GetByID";
        private const string GET_ALL_FILEYKIENXL = @"FileYKienXuLyGetByID";
        private const string GET_ALL_FILEYKIENXL_NEW = @"FileYKienXuLyGetByID_New";
        private const string GET_LAST_FILEYKIENXL = @"FileYKienXuLyGetLastByID";

        private const string INSERT_YKIENGIAIQUYET = @"YKienGiaiQuyet_Insert";
        private const string GET_ALL_YKIENGIAIQUYET = @"YKienGiaiQuyet_GetByID";
        private const string GET_ALL_YKIENGIAIQUYET_NEW = @"YKienGiaiQuyet_GetByID_New";

        private const string UPDATE_CONG_VAN_BAO_CAO_GQ = "XuLyDon_UpdateCongVanBaoCaoGQ";

        private const string GET_HUONGXULYRAVANBANDONDOC = "XuLyDon_Get_HuongXuLyRaVBDonDoc";
        private const string COUNT_HUONGXULYRAVANBANDONDOC = "XuLyDon_Count_HuongXuLyRaVBDonDoc";

        //private const string GET_BY_PAGE = @"DM_DonThu_GetByPage";
        //private const string COUNT_ALL = @"DM_DonThu_CountAll";
        //private const string COUNT_SEARCH = @"DM_DonThu_CountSearch";
        //private const string SEARCH = @"DM_DonThu_GetBySearch";

        private const string GET_BY_PAGE = @"NV_DonThu_GetByPage";
        private const string COUNT_ALL = @"NV_DonThu_CountAll";
        private const string GET_XULYLAN1 = "NV_XuLyDon_GetXuLyLan1";

        //quanghv: stored
        private const string GET_SO_DON_BY_CO_QUAN = @"NV_XuLyDon_GetSoDonByCoQuan";

        private const string UPDATE_VIEWER = @"NV_XuLyDon_UpdateViewer";
        //quymd
        private const string DonThu_CountSoDonByLoaiKhieuTo = @"DonThu_CountSoDonByLoaiKhieuTo";
        #endregion

        //Ten cac bien dau vao
        private const string PARAM_XULYDON_ID = "@XuLyDonID";
        private const string PARAM_DON_THU_ID = "@DonThuID";
        private const string PARAM_SO_LAN = "@SoLan";
        private const string PARAM_SO_DON_THU = "@SoDonThu";
        private const string PARAM_NGAY_NHAP_DON = "@NgayNhapDon";
        private const string PARAM_NGAY_QUA_HAN = "@NgayQuaHan";
        private const string PARAM_NGUON_DON_DEN_ID = "@NguonDonDen";
        private const string PARAM_CQ_CHUYEN_DON_ID = "@CQChuyenDonID";
        private const string PARAM_CQ_CHUYEN_DON_DEN_ID = "@CQChuyenDonDenID";
        private const string PARAM_SO_CONG_VAN = "@SoCongVan";
        private const string PARAM_NGAY_CHUYEN_DON = "@NgayChuyenDon";
        private const string PARAM_NHOM_KN_ID = "@NhomKNID";
        private const string PARAM_DOI_TUONG_BI_KN_ID = "@DoiTuongBiKNID";
        //private const string PARAM_LOAI_KHIEU_TO_1ID = "@LoaiKhieuTo1ID";
        //private const string PARAM_LOAI_KHIEU_TO_2ID = "@LoaiKhieuTo2ID";
        //private const string PARAM_LOAI_KHIEU_TO_3ID = "@LoaiKhieuTo3ID";
        //private const string PARAM_LOAI_KHIEU_TO_ID = "@LoaiKhieuToID";
        private const string PARAM_GAP_LANH_DAO = "@GapLanhDao";
        private const string PARAM_NGAY_GAP_LANH_DAO = "@NgayGapLanhDao";
        private const string PARAM_NOI_DUNG_DON = "@NoiDungDon";
        private const string PARAM_CQ_DA_GIAI_QUYET_ID = "@CQDaGiaiQuyetID";
        private const string PARAM_HUONG_GIAI_QUYET_ID = "@HuongGiaiQuyetID";
        private const string PARAM_NOI_DUNG_HUONG_DAN = "@NoiDungHuongDan";
        private const string PARAM_CAN_BO_XU_LY_ID = "@CanBoXuLyID";
        private const string PARAM_CQ_TIEP_NHAN_ID = "@CQTiepNhanID";
        private const string PARAM_NGAY_CQKHAC_CHUYENDONDEN = "@NgayCQKhacChuyenDonDen";
        private const string PARAM_CQ_NGOAIHETHONG = @"CQNgoaiHeThong";

        private const string PARAM_CANBOGIAIQUYETID = @"CanBoGiaiQuyetID";
        private const string PARAM_NGAYGIAIQUYET = @"NgayGiaiQuyet";
        private const string PARAM_YKIENGIAIQUYET = @"YKienGiaiQuyet";

        private const string PARAM_CAN_BO_KY_ID = "@CanBoKyID";
        private const string PARAM_CQ_GIAI_QUYET_TIEP_ID = "@CQGiaiQuyetTiepID";
        private const string PARAM_TRANG_THAI_DON_ID = "@TrangThaiDonID";
        private const string PARAM_PT_KET_QUA_ID = "@PhanTichKQID";
        private const string PARAM_TRUNG_DON = "@TrungDon";
        private const string PARAM_CO_QUAN_ID = "@CoQuanID";

        private const string PARAM_CANBO_TIEPNHAN_ID = "@CanBoTiepNhanID";

        private const string PARAM_THUOCTHAMQUYEN = "@ThuocThamQuyen";
        private const string PARAM_DUDIEUKIEN = "@DuDieuKien";
        private const string PARAM_TENHUONGGIAIQUYET = @"TenHuongGiaiQuyet";
        private const string PARAM_NGAYTHULY = @"NgayThuLy";
        private const string PARAM_NGAYXULY = @"NgayXuLy";
        private const string PARAM_DADDUYET_XULY = @"DaDuyetXuLy";
        private const string PARAM_CANBO_DUOC_CHONXL = @"CBDuocChonXL";
        private const string PARAM_QTTIEPNHANDON = @"QTTiepNhanDon";

        //trinm
        private const string PARAM_DUAN_ID = "@DuAnID";
        private const string PARAM_START = "@Start";
        private const string PARAM_END = "@End";
        //
        private const string PARAM_TUNGAY = "@TuNgay";
        private const string PARAM_DENNGAY = "@DenNgay";
        private const string PARAM_KEYWORD = "@Keyword";
        private const string PARAM_CQNHANVBDONDOC = "@CoQuanNhanVanBanDonDoc";
        //y kien xl
        private const string PARM_FILEURL = @"FileUrl";
        private const string PARM_TENFILE = @"TenFile";
        private const string PARM_YKIENXULY = @"YKienXuLy";
        private const string PARM_NGAYXULY = @"NgayXuLy";
        private const string PARM_LOAIKHIEUTO = @"LoaiKhieuTo";
        private const string PARM_NGAYVIETDON = @"NgayVietDon";
        private const string PARM_DONTHUGOCID = @"DonThuGocID";
        private const string PARM_LANGIAIQUYET = @"LanGiaiQuyet";
        private const string PARAM_LOAIFILE = "@LoaiFile";
        private XuLyDonInfo GetCustomData(SqlDataReader rdr)
        {
            XuLyDonInfo info = new XuLyDonInfo();
            info.XuLyDonID = Utils.GetInt32(rdr["XuLyDonID"], 0);
            info.DonThuID = Utils.GetInt32(rdr["DonThuID"], 0);
            info.TrangThaiDonID = Utils.GetInt32(rdr["TrangThaiDonID"], 0);
            return info;
        }

        private XuLyDonInfo GetYKienXLData(SqlDataReader rdr)
        {
            XuLyDonInfo info = new XuLyDonInfo();
            info.CanBoXuLyID = Utils.GetInt32(rdr["CanBoID"], 0);
            info.TenCanBoXuLy = Utils.GetString(rdr["TenCanBoXuLy"], string.Empty);
            info.NgayXuLy = Utils.ConvertToDateTime(rdr["NgayXuLy"], DateTime.MinValue);
            info.YKienXuLy = Utils.GetString(rdr["YKienXuLy"], string.Empty);
            info.TenFile = Utils.GetString(rdr["TenFile"], string.Empty);
            info.FileUrl = Utils.GetString(rdr["FileUrl"], string.Empty);
            return info;
        }

        private XuLyDonInfo GetFileYKienXLData(SqlDataReader rdr)
        {
            XuLyDonInfo info = new XuLyDonInfo();
            info.FileYKienXuLyID = Utils.GetInt32(rdr["FileYKienXuLyID"], 0);
            info.TenFileYKienXL = Utils.GetString(rdr["TenFile"], string.Empty);
            info.TomTat = Utils.GetString(rdr["TomTat"], string.Empty);
            info.NguoiUp = Utils.GetInt32(rdr["NguoiUp"], 0);
            info.TenCoQuanUp = new CoQuan().GetCoQuanByCanBoID(info.NguoiUp).TenCoQuan;
            info.NgayUp = Utils.ConvertToDateTime(rdr["NgayUp"], DateTime.MinValue);
            info.NgayUps = Format.FormatDate(info.NgayUp);
            info.FileUrl = Utils.GetString(rdr["FileUrl"], string.Empty);
            return info;
        }

        private XuLyDonInfo GetYKienGQData(SqlDataReader rdr)
        {
            XuLyDonInfo info = new XuLyDonInfo();
            info.CanBoGiaiQuyetID = Utils.GetInt32(rdr["CanBoID"], 0);
            info.TenCanBoGiaiQuyet = Utils.GetString(rdr["TenCanBoGiaiQuyet"], string.Empty);
            info.NgayGiaiQuyet = Utils.ConvertToDateTime(rdr["NgayGiaiQuyet"], DateTime.MinValue);
            info.YKienGiaiQuyet = Utils.GetString(rdr["YKienGiaiQuyet"], string.Empty);
            info.TenFile = Utils.GetString(rdr["TenFile"], string.Empty);
            info.FileUrl = Utils.GetString(rdr["FileUrl"], string.Empty);
            return info;
        }

        private XuLyDonInfo GetHuongGiaiQuyetRaVBDonDoc(SqlDataReader rdr)
        {
            XuLyDonInfo info = new XuLyDonInfo();
            info.XuLyDonID = Utils.ConvertToInt32(rdr["XuLyDonID"], 0);
            info.DonThuID = Utils.ConvertToInt32(rdr["DonThuID"], 0);
            info.TenChuDon = Utils.ConvertToString(rdr["HoTenChuDon"], string.Empty);
            info.TenCoQuanRaVanBan = Utils.GetString(rdr["TenCoQuan"], string.Empty);
            info.NgayXuLy = Utils.ConvertToDateTime(rdr["NgayRaVanBanDonDoc"], DateTime.MinValue);
            info.NoiDung = Utils.GetString(rdr["NoiDungDon"], string.Empty);
            info.SoDonThu = Utils.GetString(rdr["SoDonThu"], string.Empty);
            return info;
        }


        private XuLyDonInfo GetData(SqlDataReader dr, string step)
        {
            XuLyDonInfo info = new XuLyDonInfo();

            switch (step)
            {
                case "step1":
                    info.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);
                    info.DonThuID = Utils.GetInt32(dr["DonThuID"], 0);
                    info.SoDonThu = Utils.GetString(dr["SoDonThu"], string.Empty);
                    info.NgayNhapDon = Utils.GetDateTime(dr["NgayNhapDon"], Constant.DEFAULT_DATE);
                    info.NgayQuaHan = Utils.GetDateTime(dr["NgayQuaHan"], Constant.DEFAULT_DATE);
                    info.NguonDonDenID = Utils.GetInt32(dr["NguonDonDen"], 0);
                    info.CQChuyenDonID = Utils.GetInt32(dr["CQChuyenDonID"], 0);
                    info.SoCongVan = Utils.GetString(dr["SoCongVan"], string.Empty);
                    info.NgayChuyenDon = Utils.GetDateTime(dr["NgayChuyenDon"], Constant.DEFAULT_DATE);
                    info.CanBoTiepNhapID = Utils.GetInt32(dr["CanBoTiepNhapID"], 0);
                    break;
                case "step2":
                    //info.NgayNhapDon = Utils.GetDateTime(dr["NgayNhapDon"], Constant.DEFAULT_DATE);
                    info.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);
                    //info.CoQuanID = Utils.GetInt32(dr["CoQuanID"], 0);
                    //info.CanBoTiepNhapID = Utils.GetInt32(dr["CanBoTiepNhapID"], 0);
                    info.DonThuID = Utils.GetInt32(dr["DonThuID"], 0);
                    info.CQDaGiaiQuyetID = Utils.GetString(dr["CQDaGiaiQuyetID"], string.Empty);
                    info.CQChuyenDonID = Utils.GetInt32(dr["CQChuyenDonID"], 0);
                    break;
                case "step4":
                    info.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);
                    info.HuongGiaiQuyetID = Utils.GetInt32(dr["HuongGiaiQuyetID"], 0);
                    info.NoiDungHuongDan = Utils.GetString(dr["NoiDungHuongDan"], string.Empty);
                    info.CanBoXuLyID = Utils.GetInt32(dr["CanBoXuLyID"], 0);
                    info.CanBoKyID = Utils.GetInt32(dr["CanBoKyID"], 0);
                    info.ThuocThamQuyen = Utils.GetBoolean(dr["ThuocThamQuyen"], false);
                    info.DuDieuKien = Utils.GetBoolean(dr["DuDieuKien"], false);
                    info.CanBoTiepNhapID = Utils.GetInt32(dr["CanBoTiepNhapID"], 0);
                    info.CoQuanID = Utils.GetInt32(dr["CoQuanID"], 0);

                    break;
                default:
                    info.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);
                    info.DonThuID = Utils.GetInt32(dr["DonThuID"], 0);
                    info.SoDonThu = Utils.GetString(dr["SoDonThu"], string.Empty);
                    info.NgayNhapDon = Utils.GetDateTime(dr["NgayNhapDon"], Constant.DEFAULT_DATE);
                    info.NgayQuaHan = Utils.GetDateTime(dr["NgayQuaHan"], Constant.DEFAULT_DATE);
                    info.NguonDonDenID = Utils.GetInt32(dr["NguonDonDen"], 0);
                    info.CQChuyenDonID = Utils.GetInt32(dr["CQChuyenDonID"], 0);
                    info.CQChuyenDonDenID = Utils.GetInt32(dr["CQChuyenDonDenID"], 0);
                    info.SoCongVan = Utils.GetString(dr["SoCongVan"], string.Empty);
                    info.NgayChuyenDon = Utils.GetDateTime(dr["NgayChuyenDon"], Constant.DEFAULT_DATE);
                    info.CQDaGiaiQuyetID = Utils.GetString(dr["CQDaGiaiQuyetID"], string.Empty);
                    //info.GapLanhDao = Utils.GetBoolean(dr["GapLanhDao"], false);
                    //info.NgayGapLanhDao = Utils.GetDateTime(dr["NgayGapLanhDao"], Constant.DEFAULT_DATE);
                    info.HuongGiaiQuyetID = Utils.GetInt32(dr["HuongGiaiQuyetID"], 0);
                    info.NoiDungHuongDan = Utils.GetString(dr["NoiDungHuongDan"], string.Empty);
                    info.CanBoXuLyID = Utils.GetInt32(dr["CanBoXuLyID"], 0);
                    info.CanBoKyID = Utils.GetInt32(dr["CanBoKyID"], 0);
                    info.TrangThaiDonID = Utils.GetInt32(dr["TrangThaiDonID"], 0);
                    info.CanBoTiepNhapID = Utils.GetInt32(dr["CanBoTiepNhapID"], 0);
                    break;
            }

            return info;
        }

        private DTXuLyInfo GetDataForShow(SqlDataReader dr)
        {
            DTXuLyInfo info = new DTXuLyInfo();
            info.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);
            info.SoBienNhanMotCua = Utils.GetString(dr["SoBienNhanMotCua"], string.Empty);
            info.MaHoSoMotCua = Utils.GetString(dr["MaHoSoMotCua"], string.Empty);
            info.TenChuDon = Utils.GetString(dr["TenChuDon"], string.Empty);
            info.TenLoaiKhieuTo = Utils.GetString(dr["TenLoaiKhieuTo"], string.Empty);
            info.NoiDungDon = Utils.GetString(dr["NoiDungDon"], string.Empty);
            info.NgayHenTraMotCuaStr = Format.FormatDate(Utils.ConvertToDateTime(dr["NgayHenTraMotCua"], DateTime.MinValue));
            info.HuongXuLy = Utils.GetString(dr["TenHuongXuLy"], string.Empty);
            info.TenCBXuLy = Utils.GetString(dr["TenCanBo"], string.Empty);
            info.NgayXuLy = Utils.ConvertToDateTime(dr["NgayXuLy"], DateTime.MinValue);

            return info;
        }

        //Get Insert Parmas
        private SqlParameter[] GetInsertParms()
        {
            SqlParameter[] parms;

            parms = new SqlParameter[]{
                new SqlParameter(PARAM_DON_THU_ID, SqlDbType.Int),
                new SqlParameter(PARAM_SO_DON_THU, SqlDbType.NVarChar, 20),
                new SqlParameter(PARAM_NGAY_NHAP_DON, SqlDbType.DateTime),
                new SqlParameter(PARAM_NGAY_QUA_HAN, SqlDbType.DateTime),
                new SqlParameter(PARAM_NGUON_DON_DEN_ID, SqlDbType.Int),
                new SqlParameter(PARAM_CQ_CHUYEN_DON_ID, SqlDbType.Int),
                new SqlParameter(PARAM_SO_CONG_VAN, SqlDbType.NVarChar,20),

                new SqlParameter(PARAM_NGAY_CHUYEN_DON, SqlDbType.DateTime),
                new SqlParameter(PARAM_CQ_DA_GIAI_QUYET_ID, SqlDbType.NVarChar,250),
                new SqlParameter(PARAM_CO_QUAN_ID, SqlDbType.Int),
                new SqlParameter(PARAM_CANBO_TIEPNHAN_ID, SqlDbType.Int),
                new SqlParameter(PARAM_HUONG_GIAI_QUYET_ID, SqlDbType.Int),
                new SqlParameter(PARAM_NOI_DUNG_HUONG_DAN, SqlDbType.NVarChar),
                new SqlParameter(PARAM_CAN_BO_XU_LY_ID, SqlDbType.Int),
                new SqlParameter(PARAM_CAN_BO_KY_ID, SqlDbType.Int),
                new SqlParameter(PARAM_CQ_GIAI_QUYET_TIEP_ID, SqlDbType.Int),
                new SqlParameter(PARAM_TRANG_THAI_DON_ID, SqlDbType.Int),
                new SqlParameter(PARAM_NGAYXULY, SqlDbType.DateTime),
                new SqlParameter(PARAM_CQ_CHUYEN_DON_DEN_ID, SqlDbType.Int),
                new SqlParameter(PARAM_NGAY_CQKHAC_CHUYENDONDEN, SqlDbType.DateTime),
                new SqlParameter(PARAM_CQ_NGOAIHETHONG,SqlDbType.NVarChar,200),
                //new SqlParameter("MaHoSoMotCua", SqlDbType.NVarChar, 200),
                //new SqlParameter("SoBienNhanMotCua", SqlDbType.NVarChar, 200),
                //new SqlParameter("NgayHenTraMotCua", SqlDbType.DateTime)
                new SqlParameter(PARAM_CANBO_DUOC_CHONXL, SqlDbType.Int),
                new SqlParameter(PARAM_QTTIEPNHANDON, SqlDbType.Int),
                new SqlParameter(PARM_DONTHUGOCID, SqlDbType.Int),
                new SqlParameter(PARM_LANGIAIQUYET, SqlDbType.Int),
                new SqlParameter(PARAM_SO_LAN, SqlDbType.Int),
                new SqlParameter("@XuLyDonChuyenID", SqlDbType.Int)
            };

            return parms;
        }

        //SET iNSERT PARMS
        private void SetInsertParms(SqlParameter[] parms, XuLyDonInfo DTInfo)
        {
            parms[0].Value = DTInfo.DonThuID;
            parms[1].Value = DTInfo.SoDonThu ?? Convert.DBNull;
            parms[2].Value = DTInfo.NgayNhapDon;
            parms[3].Value = DTInfo.NgayQuaHan;
            parms[4].Value = DTInfo.NguonDonDenID;
            parms[5].Value = DTInfo.CQChuyenDonID;
            parms[6].Value = DTInfo.SoCongVan ?? Convert.DBNull;
            parms[7].Value = DTInfo.NgayChuyenDon;
            parms[8].Value = DTInfo.CQDaGiaiQuyetID ?? Convert.DBNull;
            parms[9].Value = DTInfo.CoQuanID;
            parms[10].Value = DTInfo.CanBoTiepNhapID;

            parms[11].Value = DTInfo.HuongGiaiQuyetID;
            parms[12].Value = DTInfo.NoiDungHuongDan ?? Convert.DBNull;
            parms[13].Value = DTInfo.CanBoXuLyID;
            parms[14].Value = DTInfo.CanBoKyID;
            parms[15].Value = DTInfo.CQGiaiQuyetTiepID;
            parms[16].Value = DTInfo.TrangThaiDonID;
            parms[17].Value = DTInfo.NgayXuLy;

            parms[18].Value = DTInfo.CQChuyenDonDenID;
            parms[19].Value = DTInfo.NgayCQKhacChuyenDonDen;
            parms[20].Value = DTInfo.CQNgoaiHeThong ?? Convert.DBNull;
            parms[21].Value = DTInfo.CBDuocChonXL;
            parms[22].Value = DTInfo.QTTiepNhanDon;
            parms[23].Value = DTInfo.DonThuGocID;
            parms[24].Value = DTInfo.LanGiaiQuyet;
            parms[25].Value = DTInfo.SoLan;
            parms[26].Value = DTInfo.XuLyDonChuyenID;

            //parms[19].Value = DTInfo.MaHoSoMotCua;
            //parms[20].Value = DTInfo.SoBienNhanMotCua;
            //parms[21].Value = DTInfo.NgayHenTraMotCua;

            //if (DTInfo.MaHoSoMotCua == null)
            //    parms[19].Value = DBNull.Value;

            //if (DTInfo.SoBienNhanMotCua == null)
            //    parms[20].Value = DBNull.Value;

            //if (DTInfo.NgayHenTraMotCua == DateTime.MinValue)
            //    parms[21].Value = DBNull.Value;

            if (DTInfo.NgayQuaHan == DateTime.MinValue) parms[3].Value = DBNull.Value;
            if (DTInfo.CQChuyenDonID == 0) parms[5].Value = DBNull.Value;
            if (DTInfo.CQDaGiaiQuyetID == "") parms[8].Value = DBNull.Value;
            if (DTInfo.NgayChuyenDon == DateTime.MinValue) parms[7].Value = DBNull.Value;

            if (DTInfo.HuongGiaiQuyetID == 0) parms[11].Value = DBNull.Value;
            if (DTInfo.NoiDungHuongDan == null) parms[12].Value = DBNull.Value;
            if (DTInfo.CanBoXuLyID == 0) parms[13].Value = DBNull.Value;
            if (DTInfo.CanBoKyID == 0) parms[14].Value = DBNull.Value;
            if (DTInfo.CQGiaiQuyetTiepID == 0) parms[15].Value = DBNull.Value;
            if (DTInfo.TrangThaiDonID == 0) parms[16].Value = DBNull.Value;
            if (DTInfo.NgayXuLy == DateTime.MinValue) parms[17].Value = DBNull.Value;
            if (DTInfo.CQChuyenDonDenID == 0) parms[18].Value = DBNull.Value;
            if (DTInfo.NgayCQKhacChuyenDonDen == DateTime.MinValue) parms[19].Value = DBNull.Value;
            if (DTInfo.CBDuocChonXL == 0) parms[21].Value = DBNull.Value;
            if (DTInfo.QTTiepNhanDon == 0) parms[22].Value = DBNull.Value;
            if (DTInfo.DonThuGocID == 0 || DTInfo.DonThuGocID == null)
            {
                parms[23].Value = DBNull.Value;
            }
            if (DTInfo.LanGiaiQuyet == 0 || DTInfo.LanGiaiQuyet == null)
            {
                parms[24].Value = DBNull.Value;
            }
        }

        //get update parms
        private SqlParameter[] GetUpdateParms()
        {
            SqlParameter[] parms = new SqlParameter[] { };

            parms = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int),
                new SqlParameter(PARAM_DON_THU_ID, SqlDbType.Int),
                new SqlParameter(PARAM_SO_DON_THU, SqlDbType.NVarChar, 20),
                new SqlParameter(PARAM_NGAY_NHAP_DON, SqlDbType.DateTime),
                new SqlParameter(PARAM_NGAY_QUA_HAN, SqlDbType.DateTime),
                new SqlParameter(PARAM_NGUON_DON_DEN_ID, SqlDbType.Int),
                new SqlParameter(PARAM_CQ_CHUYEN_DON_ID, SqlDbType.Int),
                new SqlParameter(PARAM_SO_CONG_VAN, SqlDbType.NVarChar,20),
                new SqlParameter(PARAM_NGAY_CHUYEN_DON, SqlDbType.DateTime),
                new SqlParameter(PARAM_CQ_DA_GIAI_QUYET_ID, SqlDbType.NVarChar,250),
                new SqlParameter(PARAM_CO_QUAN_ID, SqlDbType.Int),

                new SqlParameter(PARAM_HUONG_GIAI_QUYET_ID, SqlDbType.Int),
                new SqlParameter(PARAM_NOI_DUNG_HUONG_DAN, SqlDbType.NVarChar),
                new SqlParameter(PARAM_CAN_BO_XU_LY_ID, SqlDbType.Int),
                new SqlParameter(PARAM_CAN_BO_KY_ID, SqlDbType.Int),
                new SqlParameter(PARAM_CQ_GIAI_QUYET_TIEP_ID, SqlDbType.Int),
                new SqlParameter(PARAM_TRANG_THAI_DON_ID, SqlDbType.Int),
                new SqlParameter(PARAM_NGAYXULY, SqlDbType.DateTime),
                new SqlParameter(PARAM_NGAY_CQKHAC_CHUYENDONDEN, SqlDbType.DateTime),
                new SqlParameter(PARAM_CQ_NGOAIHETHONG,SqlDbType.NVarChar,200),
                new SqlParameter(PARAM_CANBO_DUOC_CHONXL, SqlDbType.Int),
                new SqlParameter(PARAM_QTTIEPNHANDON, SqlDbType.Int),
                new SqlParameter(PARAM_CQ_CHUYEN_DON_DEN_ID, SqlDbType.Int)
            };



            return parms;
        }

        //set update parms
        private void SetUpdateParms(SqlParameter[] parms, XuLyDonInfo DTInfo)
        {
            parms[0].Value = DTInfo.XuLyDonID;
            parms[1].Value = DTInfo.DonThuID;
            parms[2].Value = DTInfo.SoDonThu ?? Convert.DBNull;
            parms[3].Value = DTInfo.NgayNhapDon;
            parms[4].Value = DTInfo.NgayQuaHan;
            parms[5].Value = DTInfo.NguonDonDenID;
            parms[6].Value = DTInfo.CQChuyenDonID;
            parms[7].Value = DTInfo.SoCongVan ?? Convert.DBNull;
            parms[8].Value = DTInfo.NgayChuyenDon;
            parms[9].Value = DTInfo.CQDaGiaiQuyetID ?? Convert.DBNull;
            parms[10].Value = DTInfo.CoQuanID;

            parms[11].Value = DTInfo.HuongGiaiQuyetID;
            parms[12].Value = DTInfo.NoiDungHuongDan ?? Convert.DBNull;
            parms[13].Value = DTInfo.CanBoXuLyID;
            parms[14].Value = DTInfo.CanBoKyID;
            parms[15].Value = DTInfo.CQGiaiQuyetTiepID;
            parms[16].Value = DTInfo.TrangThaiDonID;
            parms[17].Value = DTInfo.NgayXuLy;
            parms[18].Value = DTInfo.NgayCQKhacChuyenDonDen;
            parms[19].Value = DTInfo.CQNgoaiHeThong ?? Convert.DBNull;
            parms[20].Value = DTInfo.CBDuocChonXL;
            parms[21].Value = DTInfo.QTTiepNhanDon;
            parms[22].Value = DTInfo.CQChuyenDonDenID;
            if (DTInfo.NgayChuyenDon == DateTime.MinValue) parms[8].Value = DBNull.Value;
            if (DTInfo.NgayQuaHan == DateTime.MinValue) parms[4].Value = DBNull.Value;

            if (DTInfo.CQChuyenDonID == 0) parms[6].Value = DBNull.Value;
            if (DTInfo.CQDaGiaiQuyetID == "") parms[9].Value = DBNull.Value;

            if (DTInfo.HuongGiaiQuyetID == 0) parms[11].Value = DBNull.Value;
            if (DTInfo.NoiDungHuongDan == null) parms[12].Value = DBNull.Value;
            if (DTInfo.CanBoXuLyID == 0) parms[13].Value = DBNull.Value;
            if (DTInfo.CanBoKyID == 0) parms[14].Value = DBNull.Value;
            if (DTInfo.CQGiaiQuyetTiepID == 0) parms[15].Value = DBNull.Value;
            if (DTInfo.TrangThaiDonID == 0) parms[16].Value = DBNull.Value;
            if (DTInfo.NgayXuLy == DateTime.MinValue) parms[17].Value = DBNull.Value;
            if (DTInfo.NgayCQKhacChuyenDonDen == DateTime.MinValue) parms[18].Value = DBNull.Value;
            if (DTInfo.CBDuocChonXL == 0) parms[20].Value = DBNull.Value;
            if (DTInfo.QTTiepNhanDon == 0) parms[21].Value = DBNull.Value;
            if (DTInfo.CQChuyenDonDenID == 0) parms[22].Value = DBNull.Value;

        }

        //public IList<XuLyDonInfo> GetAll()
        //{
        //    IList<XuLyDonInfo> ListDT = new List<XuLyDonInfo>();
        //    try
        //    {
        //        using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_ALL, null))
        //        {

        //            while (dr.Read())
        //            {

        //                XuLyDonInfo DTInfo = GetData(dr);
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


        private SqlParameter[] GetInsertYKienXLParms()
        {
            SqlParameter[] parms;

            parms = new SqlParameter[]{
                new SqlParameter(PARM_FILEURL, SqlDbType.NVarChar, 2000),
                new SqlParameter(PARM_TENFILE, SqlDbType.NVarChar,2000),
                new SqlParameter(PARAM_CAN_BO_XU_LY_ID, SqlDbType.Int),
                new SqlParameter(PARM_NGAYXULY, SqlDbType.DateTime),
                new SqlParameter(PARM_YKIENXULY, SqlDbType.NVarChar,4000),
                new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int),

            };

            return parms;
        }

        //SET iNSERT PARMS
        private void SetInsertYKienXLParms(SqlParameter[] parms, XuLyDonInfo DTInfo)
        {
            parms[0].Value = DTInfo.FileUrl;
            parms[1].Value = DTInfo.TenFile;
            parms[2].Value = DTInfo.CanBoXuLyID;
            parms[3].Value = DTInfo.NgayXuLy;
            parms[4].Value = DTInfo.YKienXuLy;
            parms[5].Value = DTInfo.XuLyDonID;
            if (DTInfo.NgayXuLy == DateTime.MinValue)
            {
                parms[3].Value = DBNull.Value;
            }
        }

        private SqlParameter[] GetInsertYKienGQParms()
        {
            SqlParameter[] parms;

            parms = new SqlParameter[]{
                new SqlParameter(PARM_FILEURL, SqlDbType.NVarChar, 2000),
                new SqlParameter(PARM_TENFILE, SqlDbType.NVarChar,2000),
                new SqlParameter(PARAM_CANBOGIAIQUYETID, SqlDbType.Int),
                new SqlParameter(PARAM_NGAYGIAIQUYET, SqlDbType.DateTime),
                new SqlParameter(PARAM_YKIENGIAIQUYET, SqlDbType.NVarChar,4000),
                new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int),

            };

            return parms;
        }

        //SET iNSERT PARMS
        private void SetInsertYKienGQParms(SqlParameter[] parms, XuLyDonInfo DTInfo)
        {
            parms[0].Value = DTInfo.FileUrl;
            if (DTInfo.FileUrl == null)
            {
                parms[0].Value = DBNull.Value;
            }
            parms[1].Value = DTInfo.TenFile;
            if (DTInfo.TenFile == null)
            {
                parms[1].Value = DBNull.Value;
            }
            parms[2].Value = DTInfo.CanBoGiaiQuyetID;
            parms[3].Value = DTInfo.NgayGiaiQuyet;
            parms[4].Value = DTInfo.YKienGiaiQuyet;
            parms[5].Value = DTInfo.XuLyDonID;
        }


        public XuLyDonInfo GetXuLyLan1(int donthuID)
        {
            XuLyDonInfo DTInfo = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_DON_THU_ID,SqlDbType.Int)
            };
            parameters[0].Value = donthuID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_XULYLAN1, parameters))
                {

                    if (dr.Read())
                    {
                        DTInfo = GetCustomData(dr);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return DTInfo;
        }


        public XuLyDonInfo GetByID(int xulydonID, string step = "")
        {

            XuLyDonInfo DTInfo = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDON_ID,SqlDbType.Int)
            };
            parameters[0].Value = xulydonID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_ID, parameters))
                {

                    if (dr.Read())
                    {
                        DTInfo = GetData(dr, step);
                        DTInfo.NgayCQKhacChuyenDonDen = Utils.ConvertToDateTime(dr["NgayCQKhacChuyenDonDen"], DateTime.MinValue);
                        DTInfo.CQNgoaiHeThong = Utils.ConvertToString(dr["CQNgoaiHeThong"], string.Empty);
                        DTInfo.TenCanBoPhuTrach = Utils.GetString(dr["TenCanBoPhuTrach"], string.Empty);
                        DTInfo.TenCoQuanGiaiQuyet = Utils.GetString(dr["TenCoQuanGiaiQuyet"], string.Empty);
                        DTInfo.TenCoQuanGiaiQuyet = Utils.GetString(dr["TenCoQuanGiaiQuyet"], string.Empty);
                        DTInfo.CBDuocChonXL = Utils.ConvertToInt32(dr["CBDuocChonXL"], 0);
                        DTInfo.QTTiepNhanDon = Utils.ConvertToInt32(dr["QTTiepNhanDon"], 0);
                        DTInfo.SoLan = Utils.ConvertToInt32(dr["SoLan"], 0);
                        DTInfo.DonThuGocID = Utils.ConvertToInt32(dr["DonThuGocID"], 0);
                        DTInfo.NgayThuLy = Utils.ConvertToDateTime(dr["NgayThuLy"], DateTime.MinValue);
                        DTInfo.NgayXuLy = Utils.ConvertToDateTime(dr["NgayXuLy"], DateTime.MinValue);
                        DTInfo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        DTInfo.XuLyDonChuyenID = Utils.ConvertToInt32(dr["XuLyDonChuyenID"], 0);
                        DTInfo.LanGiaiQuyet = Utils.ConvertToInt32(dr["LanGiaiQuyet"], 0);
                        DTInfo.XuLyDonGocID = Utils.ConvertToInt32(dr["DonThuGocID"], 0);
                        DTInfo.YKienXuLy = Utils.ConvertToString(dr["YKienXuLy"], string.Empty);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {

            }
            return DTInfo;
        }


        public XuLyDonInfo GetByXuLyDonID(int xulydonID)
        {

            XuLyDonInfo DTInfo = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDON_ID,SqlDbType.Int)
            };
            parameters[0].Value = xulydonID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_ID, parameters))
                {

                    if (dr.Read())
                    {
                        DTInfo.NgayCQKhacChuyenDonDen = Utils.ConvertToDateTime(dr["NgayCQKhacChuyenDonDen"], DateTime.MinValue);
                        DTInfo.CQNgoaiHeThong = Utils.ConvertToString(dr["CQNgoaiHeThong"], string.Empty);
                        DTInfo.TenCanBoPhuTrach = Utils.GetString(dr["TenCanBoPhuTrach"], string.Empty);
                        DTInfo.TenCoQuanGiaiQuyet = Utils.GetString(dr["TenCoQuanGiaiQuyet"], string.Empty);
                        DTInfo.TenCoQuanGiaiQuyet = Utils.GetString(dr["TenCoQuanGiaiQuyet"], string.Empty);
                        DTInfo.CBDuocChonXL = Utils.ConvertToInt32(dr["CBDuocChonXL"], 0);
                        DTInfo.QTTiepNhanDon = Utils.ConvertToInt32(dr["QTTiepNhanDon"], 0);
                        DTInfo.SoLan = Utils.ConvertToInt32(dr["SoLan"], 0);
                        DTInfo.DonThuGocID = Utils.ConvertToInt32(dr["DonThuGocID"], 0);
                        DTInfo.NgayThuLy = Utils.ConvertToDateTime(dr["NgayThuLy"], DateTime.MinValue);
                        DTInfo.NgayXuLy = Utils.ConvertToDateTime(dr["NgayXuLy"], DateTime.MinValue);
                        DTInfo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        DTInfo.XuLyDonChuyenID = Utils.ConvertToInt32(dr["XuLyDonChuyenID"], 0);
                        DTInfo.LanGiaiQuyet = Utils.ConvertToInt32(dr["LanGiaiQuyet"], 0);
                        DTInfo.XuLyDonGocID = Utils.ConvertToInt32(dr["DonThuGocID"], 0);
                        DTInfo.YKienXuLy = Utils.ConvertToString(dr["YKienXuLy"], string.Empty);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {

            }
            return DTInfo;
        }

        //Lay theo ID, join TiepDanKhongDon
        public XuLyDonInfo GetJoinTiepDanByID(int xulydonID)
        {

            XuLyDonInfo DTInfo = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDON_ID,SqlDbType.Int)
            };
            parameters[0].Value = xulydonID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_JOINTIEPDAN_BY_ID, parameters))
                {

                    if (dr.Read())
                    {
                        DTInfo = GetJoinTiepDanData(dr);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return DTInfo;
        }
        private XuLyDonInfo GetJoinTiepDanData(SqlDataReader dr)
        {
            XuLyDonInfo info = new XuLyDonInfo();
            info.NgayNhapDon = Utils.GetDateTime(dr["NgayNhapDon"], Constant.DEFAULT_DATE);
            info.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);
            info.CoQuanID = Utils.GetInt32(dr["CoQuanID"], 0);
            info.CanBoTiepNhapID = Utils.GetInt32(dr["CanBoTiepNhapID"], 0);
            info.DonThuID = Utils.GetInt32(dr["DonThuID"], 0);
            info.CQDaGiaiQuyetID = Utils.GetString(dr["CQDaGiaiQuyetID"], string.Empty);
            info.GapLanhDao = Utils.GetBoolean(dr["GapLanhDao"], false);
            info.VuViecCu = Utils.GetBoolean(dr["VuViecCu"], false);
            info.TiepDanKhongDonID = Utils.ConvertToInt32(dr["TiepDanKhongDonID"], 0);
            info.DuAnID = Utils.ConvertToInt32(dr["DuAnID"], 0);
            info.CQTiepNhanID = Utils.ConvertToInt32(dr["CQTiepNhanID"], 0);

            //info.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);
            //info.DonThuID = Utils.GetInt32(dr["DonThuID"], 0);
            //info.SoDonThu = Utils.GetInt32(dr["SoDonThu"], 0);
            //info.NgayNhapDon = Utils.GetDateTime(dr["NgayNhapDon"], Constant.DEFAULT_DATE);
            //info.NgayQuaHan = Utils.GetDateTime(dr["NgayQuaHan"], Constant.DEFAULT_DATE);
            //info.NguonDonDenID = Utils.GetInt32(dr["NguonDonDenID"], 0);
            //info.CQChuyenDonID = Utils.GetInt32(dr["CQChuyenDonID"], 0);
            //info.SoCongVan = Utils.GetString(dr["SoCongVan"], string.Empty);
            //info.NgayChuyenDon = Utils.GetDateTime(dr["NgayChuyenDon"], Constant.DEFAULT_DATE);
            //info.CQDaGiaiQuyetID = Utils.GetInt32(dr["CQDaGiaiQuyetID"], 0);
            ////info.GapLanhDao = Utils.GetBoolean(dr["GapLanhDao"], false);
            ////info.NgayGapLanhDao = Utils.GetDateTime(dr["NgayGapLanhDao"], Constant.DEFAULT_DATE);
            //info.HuongGiaiQuyetID = Utils.GetInt32(dr["HuongGiaiQuyetID"], 0);
            //info.NoiDungHuongDan = Utils.GetString(dr["NoiDungHuongDan"], string.Empty);
            //info.CanBoXuLyID = Utils.GetInt32(dr["CanBoXuLyID"], 0);
            //info.CQTiepNhanID = Utils.GetInt32(dr["CQTiepNhanID"], 0);
            //info.CanBoKyID = Utils.GetInt32(dr["CanBoKyID"], 0);
            //info.CQGiaiQuyetTiepID = Utils.GetInt32(dr["CQGiaiQuyetTiepID"], 0);
            //info.TrangThaiDonID = Utils.GetInt32(dr["TrangThaiDonID"], 0);

            return info;
        }


        //lay xulydon info theo don id
        public XuLyDonInfo GetByDonID(int donthuID)
        {

            XuLyDonInfo DTInfo = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_DON_THU_ID,SqlDbType.Int)
            };
            parameters[0].Value = donthuID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_DONTHU_ID, parameters))
                {

                    if (dr.Read())
                    {
                        DTInfo = GetData(dr, "");
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return DTInfo;
        }

        //-----------delete----------------
        public int Delete(int DT_ID)
        {

            int val = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_DON_THU_ID,SqlDbType.Int)
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

        public int Delete_DonThuDaTiepNhan(int DT_ID, int xuLyDonID)
        {

            int val = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_DON_THU_ID,SqlDbType.Int),
                new SqlParameter(PARAM_XULYDON_ID,SqlDbType.Int)
            };
            parameters[0].Value = DT_ID;
            parameters[1].Value = xuLyDonID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, DELETE_DTDATIEPNHAN, parameters);
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
        public int UpdateTrangThai(int xldID, int trangthaidonID)
        {

            object val = null;
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int),
                new SqlParameter(PARAM_TRANG_THAI_DON_ID, SqlDbType.Int)
            };
            parms[0].Value = xldID;
            parms[1].Value = trangthaidonID;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, UPDATE_TRANGTHAI, parms);
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

        //------------UPDATE---------------------
        public int UpdateCongVanBaoCaoGQ(XuLyDonInfo xldInfo)
        {

            object val = null;
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int),
                new SqlParameter("SoCVBaoCaoGQ", SqlDbType.NVarChar, 200),
                new SqlParameter("NgayCVBaoCaoGQ", SqlDbType.DateTime)
            };
            parms[0].Value = xldInfo.XuLyDonID;
            parms[1].Value = xldInfo.SoCVBaoCaoGQ;
            parms[2].Value = xldInfo.NgayCVBaoCaoGQ;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, UPDATE_CONG_VAN_BAO_CAO_GQ, parms);
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

        //------------UPDATE---------------------
        public int Update(XuLyDonInfo DTInfo)
        {

            object val = null;
            SqlParameter[] parameters = GetUpdateParms();
            SetUpdateParms(parameters, DTInfo);

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, UPDATE_NEW, parameters);
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

        public int UpdateTrangThaiTrinhDuThao(int? XuLyDonID, int? TrinhDuThao)
        {

            object val = null;

            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("XuLyDonID", SqlDbType.Int),
                new SqlParameter("TrinhDuThao", SqlDbType.Int),
            };
            parameters[0].Value = XuLyDonID ?? Convert.DBNull;
            parameters[1].Value = TrinhDuThao ?? Convert.DBNull;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "v2_NV_XuLyDon_UpdateTrangThaiTrinhDuThao", parameters);
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

        public int BanHanhQuyetDinhXacMinh(int? XuLyDonID, int? TrinhDuThao, string NoiDungBanHanhXM, int? CanBoBanHanh, DateTime? NgayBanHanh)
        {

            object val = null;

            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("XuLyDonID", SqlDbType.Int),
                new SqlParameter("TrinhDuThao", SqlDbType.Int),
                new SqlParameter("NoiDungBanHanhXM", SqlDbType.NVarChar),
                new SqlParameter("CanBoBanHanh", SqlDbType.Int),
                new SqlParameter("NgayBanHanh", SqlDbType.DateTime),
            };
            parameters[0].Value = XuLyDonID ?? Convert.DBNull;
            parameters[1].Value = TrinhDuThao ?? Convert.DBNull;
            parameters[2].Value = NoiDungBanHanhXM ?? Convert.DBNull;
            parameters[3].Value = CanBoBanHanh ?? Convert.DBNull;
            parameters[4].Value = NgayBanHanh ?? Convert.DBNull;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "v2_NV_XuLyDon_BanHanhQuyetDinhXacMinh_New", parameters);
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
        public int BanHanhQuyetDinhGiaiQuyet(int? XuLyDonID, int? TrinhDuThao, string NoiDungBanHanhGQ)
        {

            object val = null;

            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("XuLyDonID", SqlDbType.Int),
                new SqlParameter("TrinhDuThao", SqlDbType.Int),
                new SqlParameter("NoiDungBanHanhGQ", SqlDbType.NVarChar),
            };
            parameters[0].Value = XuLyDonID ?? Convert.DBNull;
            parameters[1].Value = TrinhDuThao ?? Convert.DBNull;
            parameters[2].Value = NoiDungBanHanhGQ ?? Convert.DBNull;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "v2_NV_XuLyDon_BanHanhQuyetDinhGiaiQuyet", parameters);
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
        public int UpdateQuyTrinh(int? XuLyDonID, int? QuyTrinhXLD, int? QuyTrinhGQ)
        {
            object val = null;

            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("XuLyDonID", SqlDbType.Int),
                new SqlParameter("QuyTrinhXLD", SqlDbType.Int),
                new SqlParameter("QuyTrinhGQ", SqlDbType.NVarChar),
            };
            parameters[0].Value = XuLyDonID ?? Convert.DBNull;
            parameters[1].Value = QuyTrinhXLD ?? Convert.DBNull;
            parameters[2].Value = QuyTrinhGQ ?? Convert.DBNull;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "v2_NV_XuLyDon_UpdateQuyTrinh", parameters);
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
        public int Insert(XuLyDonInfo DTInfo)
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

        //lay so don tiep dan theo co quan ID
        public int getSoDonThu(int coQuanID)
        {
            object val = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_CO_QUAN_ID,SqlDbType.Int)
            };
            parameters[0].Value = coQuanID;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, GET_SO_DON_BY_CO_QUAN, parameters);
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


        public List<XuLyDonInfo> GetYKienGiaiQuyet(int XuLyDonID)
        {
            List<XuLyDonInfo> lsYKienGQ = new List<XuLyDonInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDON_ID,SqlDbType.Int),
            };
            parameters[0].Value = XuLyDonID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_ALL_YKIENGIAIQUYET_NEW, parameters))
                {
                    while (dr.Read())
                    {
                        XuLyDonInfo xuLyDonInfo = new XuLyDonInfo();
                        xuLyDonInfo.CanBoGiaiQuyetID = Utils.GetInt32(dr["CanBoID"], 0);
                        xuLyDonInfo.TenCanBoGiaiQuyet = Utils.GetString(dr["TenCanBoGiaiQuyet"], string.Empty);
                        xuLyDonInfo.NgayGiaiQuyet = Utils.ConvertToDateTime(dr["NgayGiaiQuyet"], DateTime.MinValue);
                        xuLyDonInfo.YKienGiaiQuyet = Utils.GetString(dr["YKienGiaiQuyet"], string.Empty);
                        //xuLyDonInfo.TenFile = Utils.GetString(dr["TenFile"], string.Empty);
                        xuLyDonInfo.FileUrl = Utils.GetString(dr["FileUrl"], string.Empty);
                        xuLyDonInfo.NguoiUp = Utils.ConvertToInt32(dr["NguoiUp"], 0);
                        var cq = new CoQuan().GetCoQuanByCanBoID(xuLyDonInfo.NguoiUp);
                        if (cq != null)
                        {
                            xuLyDonInfo.TenCoQuanUp = cq.TenCoQuan;
                        }
                        xuLyDonInfo.YKienGiaiQuyetID = Utils.ConvertToInt32(dr["YKienGiaiQuyetID"], 0);
                        xuLyDonInfo.TenCanBo = Utils.ConvertToString(dr["CanBoUpFile"], string.Empty);
                        xuLyDonInfo.IsBaoMat = Utils.ConvertToBoolean(dr["IsBaoMat"], false);
                        xuLyDonInfo.FileID = Utils.ConvertToInt32(dr["FileID"], 0);
                        xuLyDonInfo.TenFile = Utils.ConvertToString(dr["TenFileNew"], string.Empty);
                        xuLyDonInfo.NhomFileID = Utils.ConvertToInt32(dr["NhomFileID"], 0);
                        xuLyDonInfo.TenNhomFile = Utils.ConvertToString(dr["TenNhomFile"], string.Empty);
                        xuLyDonInfo.ThuTuHienThiNhom = Utils.ConvertToInt32(dr["ThuTuHienThi"], 0);
                        xuLyDonInfo.ThuTuHienThiFile = Utils.ConvertToInt32(dr["ThuTuHienThiFile"], 0);
                        xuLyDonInfo.FileBaoCaoXacMinhID = Utils.ConvertToInt32(dr["FileBaoCaoXacMinhID"], 0);
                        lsYKienGQ.Add(xuLyDonInfo);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return lsYKienGQ;
        }
        public List<XuLyDonInfo> GetAllXuLyByXuLyDonGoc(int XuLyDonGocID)
        {
            List<XuLyDonInfo> lsYKienGQ = new List<XuLyDonInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDON_ID,SqlDbType.Int),
            };
            parameters[0].Value = XuLyDonGocID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "GET_ALL_XuLyDonGocID", parameters))
                {
                    while (dr.Read())
                    {
                        XuLyDonInfo xuLyDonInfo = new XuLyDonInfo();
                        xuLyDonInfo.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);
                        //xuLyDonInfo.TenCanBoGiaiQuyet = Utils.GetString(dr["TenCanBoGiaiQuyet"], string.Empty);
                        //xuLyDonInfo.NgayGiaiQuyet = Utils.ConvertToDateTime(dr["NgayGiaiQuyet"], DateTime.MinValue);
                        //xuLyDonInfo.YKienGiaiQuyet = Utils.GetString(dr["YKienGiaiQuyet"], string.Empty);
                        //xuLyDonInfo.TenFile = Utils.GetString(dr["TenFile"], string.Empty);
                        //xuLyDonInfo.FileUrl = Utils.GetString(dr["FileUrl"], string.Empty);
                        //xuLyDonInfo.NguoiUp = Utils.ConvertToInt32(dr["NguoiUp"], 0);
                        //xuLyDonInfo.TenCoQuanUp = new CoQuan().GetCoQuanByCanBoID(xuLyDonInfo.NguoiUp).TenCoQuan;
                        //xuLyDonInfo.YKienGiaiQuyetID = Utils.ConvertToInt32(dr["YKienGiaiQuyetID"], 0);
                        //xuLyDonInfo.TenCanBo = Utils.ConvertToString(dr["CanBoUpFile"], string.Empty);
                        //xuLyDonInfo.IsBaoMat = Utils.ConvertToBoolean(dr["IsBaoMat"], false);
                        lsYKienGQ.Add(xuLyDonInfo);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
            }
            return lsYKienGQ;
        }

        // get list huong xu ly ra van ban don doc
        public List<XuLyDonInfo> GetHuongGiaiQuyetRaVBDonDoc(string Keyword, DateTime? tuNgay, DateTime? denNgay, int start, int end
               , int? LoaiKhieuToID, ref int total, int? CoQuanDangNhapID)
        {

            List<XuLyDonInfo> lsHuongGQ = new List<XuLyDonInfo>();
            SqlParameter[] parameters = new SqlParameter[]{

                 new SqlParameter(PARAM_KEYWORD,SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),

                new SqlParameter(PARAM_START, SqlDbType.Int),
                new SqlParameter(PARAM_END, SqlDbType.Int),
                new SqlParameter("@LoaiKhieuToID", SqlDbType.Int),
                   new SqlParameter("@Total", SqlDbType.Int),
                   new SqlParameter("@CoQuanNhanVanBanDonDoc", SqlDbType.Int)
            };
            //parameters[0].Value = HuongGiaiQuyetID;
            parameters[0].Value = Keyword == "" ? "" : Keyword;
            parameters[1].Value = tuNgay ?? Convert.DBNull;
            parameters[2].Value = denNgay ?? Convert.DBNull;
            //parameters[4].Value = cQNhanVBDonDoc;
            parameters[3].Value = start;
            parameters[4].Value = end;
            parameters[5].Value = LoaiKhieuToID ?? Convert.DBNull;
            parameters[6].Direction = ParameterDirection.Output;
            parameters[6].Size = 8;
            parameters[7].Value = CoQuanDangNhapID;

            if (tuNgay == DateTime.MinValue)
            {
                parameters[1].Value = DBNull.Value;
            }
            if (denNgay == DateTime.MinValue)
            {
                parameters[2].Value = DBNull.Value;
            }
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "XuLyDon_Get_HuongXuLyRaVBDonDoc_New", parameters))
                {
                    while (dr.Read())
                    {
                        XuLyDonInfo xuLyDonInfo = GetHuongGiaiQuyetRaVBDonDoc(dr);
                        xuLyDonInfo.HuongGiaiQuyetID = Utils.ConvertToInt32(dr["HuongGiaiQuyetID"], 0);
                        xuLyDonInfo.CanhBao = Utils.ConvertToInt32(dr["CanhBao"], 0);
                        xuLyDonInfo.TenChuDon = Utils.ConvertToString(dr["HoTenChuDon"], string.Empty);
                        xuLyDonInfo.NgayDonDoc = Utils.ConvertToDateTime(dr["NgayRaVanBanDonDoc"], DateTime.MinValue);
                        xuLyDonInfo.HanXuLy = Utils.ConvertToDateTime(dr["HanXuLy"], DateTime.MinValue);
                        xuLyDonInfo.HanXuLyStr = Format.FormatDate(xuLyDonInfo.HanXuLy);
                        xuLyDonInfo.NgayDonDocStr = Format.FormatDate(xuLyDonInfo.NgayDonDoc);
                        xuLyDonInfo.PhanLoai = Utils.ConvertToString(dr["TenLoaiKhieuTo"], string.Empty);
                        xuLyDonInfo.TenNguonDonDen = Utils.ConvertToString(dr["TenNguonDonDen"], string.Empty);
                        xuLyDonInfo.SoDonThu = Utils.ConvertToString(dr["SoDonThu"], string.Empty);
                        xuLyDonInfo.NoiDung = Utils.ConvertToString(dr["NoiDungDon"], string.Empty);
                        xuLyDonInfo.TrangThaiDonID = Utils.ConvertToInt32(dr["TrangThai"], 0);
                        xuLyDonInfo.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
                        xuLyDonInfo.DonThuID = Utils.ConvertToInt32(dr["DonThuID"], 0);
                        if (xuLyDonInfo.TrangThaiDonID == 1)
                        {
                            xuLyDonInfo.TenTrangThai = "Chưa nhận đôn đốc";
                            //btnView.Visible = true;
                        }
                        else if (xuLyDonInfo.TrangThaiDonID == 2)
                        {
                            xuLyDonInfo.TenTrangThai = "Chưa báo cáo";
                            //btnView.Visible = false;
                        }
                        else
                        {
                            xuLyDonInfo.TenTrangThai = "Đã báo cáo";
                            //btnView.Visible = false;
                        }
                        lsHuongGQ.Add(xuLyDonInfo);
                        //total = Utils.ConvertToInt32(parameters[6].Value, 0);
                    }
                    total = Utils.ConvertToInt32(parameters[6].Value, 0);
                    dr.Close();
                    //total = Utils.ConvertToInt32(parameters[6].Value, 0);
                }
            }
            catch
            {
                throw;
            }
            return lsHuongGQ;
        }
        public int CountListDonThu(string Keyword, DateTime? tuNgay, DateTime? denNgay, int start, int end
            , int? LoaiKhieuToID, ref int total, int? CoQuanDangNhapID)
        {
            XuLyDonInfo xuLyDonInfo = new XuLyDonInfo();
            //List<XuLyDonInfo> lsHuongGQ = new List<XuLyDonInfo>();
            SqlParameter[] parameters = new SqlParameter[]{

                 new SqlParameter(PARAM_KEYWORD,SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),

                //new SqlParameter(PARAM_START, SqlDbType.Int),
                //new SqlParameter(PARAM_END, SqlDbType.Int),
                new SqlParameter("@LoaiKhieuToID", SqlDbType.Int),
                new SqlParameter("@CoQuanDangNhapID", SqlDbType.Int)
                   //new SqlParameter("@Total", SqlDbType.Int)
            };
            //parameters[0].Value = HuongGiaiQuyetID;
            parameters[0].Value = Keyword;
            parameters[1].Value = tuNgay ?? Convert.DBNull;
            parameters[2].Value = denNgay ?? Convert.DBNull;
            //parameters[4].Value = cQNhanVBDonDoc;
            //parameters[3].Value = start;
            //parameters[4].Value = 100;
            parameters[3].Value = LoaiKhieuToID ?? Convert.DBNull;
            parameters[4].Value = CoQuanDangNhapID ?? Convert.DBNull;
            //parameters[6].Direction = ParameterDirection.Output;
            //parameters[6].Size = 8;

            if (tuNgay == DateTime.MinValue)
            {
                parameters[1].Value = DBNull.Value;
            }
            if (denNgay == DateTime.MinValue)
            {
                parameters[2].Value = DBNull.Value;
            }
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "Tong_DSDonDoc", parameters))
                {
                    while (dr.Read())
                    {
                        //xuLyDonInfo = GetHuongGiaiQuyetRaVBDonDoc(dr);
                        xuLyDonInfo.Tong = Utils.ConvertToInt32(dr["Tong"], 0);
                        //    xuLyDonInfo.CanhBao = Utils.ConvertToInt32(dr["CanhBao"], 0);
                        //    xuLyDonInfo.TenChuDon = Utils.ConvertToString(dr["HoTenChuDon"], string.Empty);
                        //    xuLyDonInfo.NgayDonDoc = Utils.ConvertToDateTime(dr["NgayRaVanBanDonDoc"], DateTime.MinValue);
                        //    xuLyDonInfo.HanXuLy = Utils.ConvertToDateTime(dr["HanXuLy"], DateTime.MinValue);
                        //    xuLyDonInfo.HanXuLyStr = Format.FormatDate(xuLyDonInfo.HanXuLy);
                        //    xuLyDonInfo.NgayDonDocStr = Format.FormatDate(xuLyDonInfo.NgayDonDoc);
                        //    xuLyDonInfo.PhanLoai = Utils.ConvertToString(dr["TenLoaiKhieuTo"], string.Empty);
                        //    xuLyDonInfo.TenNguonDonDen = Utils.ConvertToString(dr["TenNguonDonDen"], string.Empty);
                        //    xuLyDonInfo.SoDonThu = Utils.ConvertToString(dr["SoDonThu"], string.Empty);
                        //    xuLyDonInfo.NoiDung = Utils.ConvertToString(dr["NoiDungDon"], string.Empty);
                        //    xuLyDonInfo.TrangThaiDonID = Utils.ConvertToInt32(dr["TrangThai"], 0);
                        //    xuLyDonInfo.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
                        //    xuLyDonInfo.DonThuID = Utils.ConvertToInt32(dr["DonThuID"], 0);
                        //    lsHuongGQ.Add(xuLyDonInfo);
                        //    //total = Utils.ConvertToInt32(parameters[6].Value, 0);
                    }
                    //total = Utils.ConvertToInt32(parameters[6].Value, 0);
                    dr.Close();
                    //total = Utils.ConvertToInt32(parameters[6].Value, 0);
                }
            }
            catch
            {
                throw;
            }
            return xuLyDonInfo.Tong;
        }

        public int Count_ListVanBanDonDoc(int HuongGiaiQuyetID, string Keyword, DateTime tuNgay, DateTime denNgay, int cQNhanVBDonDoc)
        {

            int result = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_HUONG_GIAI_QUYET_ID,SqlDbType.Int),
                 new SqlParameter(PARAM_KEYWORD,SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_CQNHANVBDONDOC, SqlDbType.Int),
            };
            parameters[0].Value = HuongGiaiQuyetID;
            parameters[1].Value = Keyword;
            parameters[2].Value = tuNgay;
            parameters[3].Value = denNgay;
            parameters[4].Value = cQNhanVBDonDoc;
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
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_HUONGXULYRAVANBANDONDOC, parameters))
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

        public List<XuLyDonInfo> GetYKienXuLy(int XuLyDonID)
        {

            List<XuLyDonInfo> lsYKienXuLy = new List<XuLyDonInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDON_ID,SqlDbType.Int),
            };
            parameters[0].Value = XuLyDonID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_ALL_YKIENXL, parameters))
                {
                    while (dr.Read())
                    {
                        XuLyDonInfo xuLyDonInfo = GetYKienXLData(dr);
                        lsYKienXuLy.Add(xuLyDonInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return lsYKienXuLy;
        }

        public List<XuLyDonInfo> GetFileYKienXuLy(int XuLyDonID)
        {

            List<XuLyDonInfo> lsFileYKienXuLy = new List<XuLyDonInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDON_ID,SqlDbType.Int),
            };
            parameters[0].Value = XuLyDonID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_ALL_FILEYKIENXL_NEW, parameters))
                {
                    while (dr.Read())
                    {
                        XuLyDonInfo xuLyDonInfo = GetFileYKienXLData(dr);
                        xuLyDonInfo.FileID = Utils.ConvertToInt32(dr["FileID"], 0);
                        xuLyDonInfo.TenFile = Utils.ConvertToString(dr["TenFileNew"], string.Empty);
                        xuLyDonInfo.NhomFileID = Utils.ConvertToInt32(dr["NhomFileID"], 0);
                        xuLyDonInfo.TenNhomFile = Utils.ConvertToString(dr["TenNhomFile"], string.Empty);
                        xuLyDonInfo.ThuTuHienThiNhom = Utils.ConvertToInt32(dr["ThuTuHienThi"], 0);
                        xuLyDonInfo.ThuTuHienThiFile = Utils.ConvertToInt32(dr["ThuTuHienThiFile"], 0);
                        xuLyDonInfo.TenCanBoXuLy = Utils.GetString(dr["TenCanBo"], string.Empty);
                        //xuLyDonInfo.TenFile = Utils.GetString(dr["TenFile"], string.Empty);
                        xuLyDonInfo.IsBaoMat = Utils.ConvertToBoolean(dr["IsBaoMat"], false);
                        xuLyDonInfo.IsMaHoa = Utils.ConvertToBoolean(dr["IsMaHoa"], false);
                        xuLyDonInfo.LoaiFile = Utils.ConvertToInt32(dr["LoaiFile"], 0);
                        xuLyDonInfo.YKienXuLy = Utils.ConvertToString(dr["YKienXuLy"], string.Empty);
                        xuLyDonInfo.GroupUID = Utils.ConvertToString(dr["GroupUID"], string.Empty);

                        if (xuLyDonInfo.IsBaoMat == false)
                        {
                            xuLyDonInfo.IsBaoMatString = "1";
                        }
                        else
                        {
                            xuLyDonInfo.IsBaoMatString = "2";
                        }

                        xuLyDonInfo.NgayUps = string.Empty;
                        if (xuLyDonInfo.NgayUp != DateTime.MinValue)
                        {
                            xuLyDonInfo.NgayUps = xuLyDonInfo.NgayUp.ToString("dd/MM/yyyy");
                        }
                        xuLyDonInfo.FileUrl = xuLyDonInfo.FileUrl;
                        lsFileYKienXuLy.Add(xuLyDonInfo);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
            }
            return lsFileYKienXuLy;
        }

        public List<XuLyDonInfo> GetFileYKienXuLyLast(int XuLyDonID, int loaiFile)
        {
            List<XuLyDonInfo> lst_xulydon = new List<XuLyDonInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDON_ID,SqlDbType.Int),
                new SqlParameter(PARAM_LOAIFILE, SqlDbType.Int)
            };
            parameters[0].Value = XuLyDonID;
            parameters[1].Value = loaiFile;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_LAST_FILEYKIENXL, parameters))
                {
                    while (dr.Read())
                    {
                        XuLyDonInfo info = new XuLyDonInfo();
                        info = GetFileYKienXLData(dr);
                        info.TenCanBoXuLy = Utils.GetString(dr["TenCanBo"], string.Empty);
                        info.TenFile = Utils.GetString(dr["TenFile"], string.Empty);
                        info.NgayUps = Format.FormatDate(info.NgayUp);
                        info.IsBaoMat = Utils.ConvertToBoolean(dr["IsBaoMat"], false);
                        lst_xulydon.Add(info);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return lst_xulydon;
        }

        public IList<DTXuLyInfo> GetByPage(int page)
        {
            int start = (page - 1) * Constant.PageSize;
            int end = page * Constant.PageSize;

            IList<DTXuLyInfo> ls_donthu = new List<DTXuLyInfo>();
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
                        var DTInfo = GetDataForShow(dr);
                        ls_donthu.Add(DTInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return ls_donthu;
        }

        public IList<DTXuLyInfo> GetDonThuDaLayTuISO()
        {
            IList<DTXuLyInfo> ls_donthu = new List<DTXuLyInfo>();

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "XuLyDon_GetDonThuDaLayTuISO", null))
                {
                    while (dr.Read())
                    {
                        DTXuLyInfo DTInfo = GetDataForShow(dr);
                        ls_donthu.Add(DTInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return ls_donthu;
        }

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
        public bool CheckIsHuongGiaiQuyet(int xulydonid, string tenhuonggiaiquyet)
        {
            bool result = false;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_TENHUONGGIAIQUYET,SqlDbType.NVarChar,100),
                new SqlParameter(PARAM_XULYDON_ID,SqlDbType.Int)
            };
            parameters[0].Value = tenhuonggiaiquyet;
            parameters[1].Value = xulydonid;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, CHECK_ISHUONGGIAIQUYET, parameters))
                {

                    if (dr.Read())
                    {
                        result = Utils.ConvertToBoolean(dr["IsHuongGiaiQuyet"], false);
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
        public int UpdateNgayThuLy(int xldID, DateTime ngaythuly)
        {

            object val = null;
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int),
                new SqlParameter(PARAM_NGAYTHULY, SqlDbType.DateTime)
            };
            parms[0].Value = xldID;
            parms[1].Value = ngaythuly;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, UPDATE_NGAYTHULY, parms);
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
        public int UpdateChuyenDon(int xldID, int NguonDonDen, bool daDuyetXuLy)
        {

            object val = null;
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int),
                new SqlParameter(PARAM_NGUON_DON_DEN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_DADDUYET_XULY,SqlDbType.Bit)
            };
            parms[0].Value = xldID;
            parms[1].Value = NguonDonDen;
            parms[2].Value = daDuyetXuLy;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, UPDATE_CHUYENDON, parms);
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
        public int UpdateCoQuanGiaiQuyet(int xldID, int coquanid)
        {

            object val = null;
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int),
                new SqlParameter(PARAM_CO_QUAN_ID, SqlDbType.Int)
            };
            parms[0].Value = xldID;
            parms[1].Value = coquanid;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, UPDATE_COQUANGIAIQUYET, parms);
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

        public int UpdateHXL(XuLyDonInfo DTInfo)
        {

            object val = null;
            //SqlParameter[] parameters = GetUpdateParms();
            //SetUpdateParms(parameters, DTInfo);
            SqlParameter[] parms = new SqlParameter[] { };

            parms = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int),
                new SqlParameter(PARAM_CO_QUAN_ID, SqlDbType.Int),
                new SqlParameter(PARAM_CQ_CHUYEN_DON_ID, SqlDbType.Int),

                new SqlParameter(PARAM_HUONG_GIAI_QUYET_ID, SqlDbType.Int),
                new SqlParameter(PARAM_NOI_DUNG_HUONG_DAN, SqlDbType.NVarChar),
                new SqlParameter(PARAM_CAN_BO_XU_LY_ID, SqlDbType.Int),
                new SqlParameter(PARAM_CAN_BO_KY_ID, SqlDbType.Int),
                new SqlParameter(PARAM_TRANG_THAI_DON_ID, SqlDbType.Int),
                new SqlParameter(PARAM_NGAYXULY, SqlDbType.DateTime),
                new SqlParameter(PARAM_NGAY_CHUYEN_DON, SqlDbType.DateTime),
                new SqlParameter(PARAM_CQ_NGOAIHETHONG,SqlDbType.NVarChar,200)

            };

            parms[0].Value = DTInfo.XuLyDonID;
            parms[1].Value = DTInfo.CoQuanID;
            parms[2].Value = DTInfo.CQChuyenDonID;

            parms[3].Value = DTInfo.HuongGiaiQuyetID;
            parms[4].Value = DTInfo.NoiDungHuongDan;
            parms[5].Value = DTInfo.CanBoXuLyID;
            parms[6].Value = DTInfo.CanBoKyID;
            parms[7].Value = DTInfo.TrangThaiDonID;
            parms[8].Value = DTInfo.NgayXuLy;
            parms[9].Value = DTInfo.NgayChuyenDon;
            parms[10].Value = DTInfo.CQNgoaiHeThong;
            if (DTInfo.CQChuyenDonID == 0) parms[2].Value = DBNull.Value;
            if (DTInfo.HuongGiaiQuyetID == 0) parms[3].Value = DBNull.Value;
            if (DTInfo.NoiDungHuongDan == null) parms[4].Value = DBNull.Value;
            if (DTInfo.CanBoXuLyID == 0) parms[5].Value = DBNull.Value;
            if (DTInfo.CanBoKyID == 0) parms[6].Value = DBNull.Value;
            if (DTInfo.TrangThaiDonID == 0) parms[7].Value = DBNull.Value;
            if (DTInfo.NgayChuyenDon == DateTime.MinValue) parms[9].Value = DBNull.Value;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, UPDATE_HXL, parms);
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

        public int UpdateCanBoTiepNhan(int xldID, int canBoID)
        {

            object val = null;
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int),
                new SqlParameter(PARAM_CANBO_TIEPNHAN_ID, SqlDbType.Int)
            };
            parms[0].Value = xldID;
            parms[1].Value = canBoID;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, UPDATE_CANBO_TIEPNHAN, parms);
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

        public int UpdateCQNhanVBDonDoc(int xldID, int coQuanID)
        {

            object val = null;
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int),
                new SqlParameter(PARAM_CO_QUAN_ID, SqlDbType.Int)
            };
            parms[0].Value = xldID;
            parms[1].Value = coQuanID;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, UPDATE_CQNHAN_VBDONDOC, parms);
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
        public int UpdateNgayChuyen(int xldID, DateTime dueDate)
        {
            object val = null;
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@XuLyDonID", SqlDbType.Int),
                new SqlParameter("@NgayChuyen", SqlDbType.DateTime)
                //new SqlParameter(PARAM_CO_QUAN_ID, SqlDbType.Int)
            };
            parms[0].Value = xldID;
            parms[1].Value = dueDate;
            //parms[1].Value = coQuanID;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, "Update_NgayChuyenXLD", parms);
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

        public int UpdateCQNhanDonChuyen(int xldID, int coQuanID)
        {

            object val = null;
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int),
                new SqlParameter(PARAM_CO_QUAN_ID, SqlDbType.Int)
            };
            parms[0].Value = xldID;
            parms[1].Value = coQuanID;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, UPDATE_CQNHAN_DONCHUYEN, parms);
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


        public int UpdateCanBoXuLy(int xldID, int canBoID)
        {

            object val = null;
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int),
                new SqlParameter(PARAM_CAN_BO_XU_LY_ID, SqlDbType.Int)
            };
            parms[0].Value = xldID;
            parms[1].Value = canBoID;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, UPDATE_CANBOXULY, parms);
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

        public int UpdateNgayQuaHan(int xldID, DateTime ngayQuaHan)
        {
            object val = null;
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int),
                new SqlParameter(PARAM_NGAY_QUA_HAN,SqlDbType.DateTime)
            };
            parms[0].Value = xldID;
            parms[1].Value = ngayQuaHan;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, UPDATE_NGAYQUAHAN, parms);
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

        public int UpdateCBDuocChonXL(int xldID, int canBoID, int quyTrinh)
        {
            object val = null;
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int),
                new SqlParameter(PARAM_CANBO_DUOC_CHONXL,SqlDbType.Int),
                new SqlParameter(PARAM_QTTIEPNHANDON,SqlDbType.Int)
            };
            parms[0].Value = xldID;
            parms[1].Value = canBoID;
            parms[2].Value = quyTrinh;

            if (quyTrinh == (int)EnumQTTiepNhanDon.QTTiepNhanGianTiep) parms[1].Value = DBNull.Value;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, UPDATE_CBDUOC_CHONXL, parms);
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

        public int InsertYKienXL(XuLyDonInfo DTInfo)
        {

            object val = null;

            SqlParameter[] parameters = GetInsertYKienXLParms();
            SetInsertYKienXLParms(parameters, DTInfo);

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, INSERT_YKIENXL, parameters);
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


        public int InsertYKienGiaiQuyet(XuLyDonInfo DTInfo)
        {

            object val = null;

            SqlParameter[] parameters = GetInsertYKienGQParms();
            SetInsertYKienGQParms(parameters, DTInfo);

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, INSERT_YKIENGIAIQUYET, parameters);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
                conn.Close();
            }
            return Utils.ConvertToInt32(val, 0);
        }

        public int Update_Viewer(int xuLyDonID)
        {
            object val = 0;
            SqlParameter[] parms = new SqlParameter[]
            {
               new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int)

            };

            parms[0].Value = xuLyDonID;


            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, UPDATE_VIEWER, parms);
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

        #region -- for portal
        public XuLyDonInfo GetXuLyDonBySoDonThu(string soDonThu, int coQuanID)
        {

            XuLyDonInfo DTInfo = new XuLyDonInfo();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_SO_DON_THU,SqlDbType.NVarChar,50),
                 new SqlParameter(PARAM_CO_QUAN_ID,SqlDbType.Int),
            };
            parameters[0].Value = soDonThu;
            parameters[1].Value = coQuanID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_XULYDON_BY_SODONTHU, parameters))
                {

                    if (dr.Read())
                    {
                        DTInfo.DonThuID = Utils.ConvertToInt32(dr["DonThuID"], 0);
                        DTInfo.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return DTInfo;
        }

        public DonThuPortalInfo GetDonThuByIDForPortal(int donthuID, int xulydonID)
        {

            DonThuPortalInfo DTInfo = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_DON_THU_ID,SqlDbType.Int),
                new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int)
            };
            parameters[0].Value = donthuID;
            parameters[1].Value = xulydonID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GETDONTHU_BY_ID_FORPORTAL, parameters))
                {

                    if (dr.Read())
                    {
                        DTInfo = GetDataForPortal(dr);
                        DTInfo.NguonDonDen = Utils.ConvertToInt32(dr["NguonDonDen"], 0);
                        DTInfo.TenCQChuyenDonDen = Utils.GetString(dr["TenCQChuyenDonDen"], string.Empty);
                        DTInfo.TenCanBoTiepNhan = Utils.GetString(dr["TenCanBoTiepNhan"], string.Empty);
                        DTInfo.TenCoQuanTiepNhan = Utils.GetString(dr["TenCoQuanTiepNhan"], string.Empty);
                        DTInfo.TenPhongBanTiepNhan = Utils.GetString(dr["TenPhongBanTiepNhan"], string.Empty);
                        DTInfo.TenCoQuanBanHanh = Utils.GetString(dr["TenCoQuanBanHanh"], string.Empty);
                        DTInfo.CoQuanChuyenDonDi = Utils.GetString(dr["CoQuanChuyenDonDi"], string.Empty);
                        DTInfo.NgayChuyenDonSangCQKhac = Utils.ConvertToDateTime(dr["NgayChuyenDonDi"], DateTime.MinValue);
                        DTInfo.CQDaGiaiQuyetID = Utils.GetString(dr["CQDaGiaiQuyetID"], string.Empty);
                        var TenCanBoXl = Utils.GetString(dr["TenCanBoXuLy"], string.Empty);
                        if (TenCanBoXl != "")
                        {
                            DTInfo.TenCanBoXuLy = Utils.GetString(dr["TenCanBoXuLy"], string.Empty);
                        }
                        else
                        {
                            DTInfo.TenCanBoXuLy = Utils.GetString(dr["TenCanBoPhanXL"], string.Empty);
                        }


                        DTInfo.TenPhongBanXuLy = Utils.GetString(dr["TenPhongBanXuLy"], string.Empty);
                        //DTInfo.NgayQuaHanGQ = Utils.ConvertToDateTime(dr["HanGiaiQuyet"], DateTime.MinValue);
                        DTInfo.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        if (DTInfo.StateID == (int)EnumState.LDPhanGQ)
                        {
                            DTInfo.NgayQuaHanGQ = Utils.ConvertToDateTime(dr["HanGiaiQuyet"], DateTime.MinValue);
                        }
                        else
                        {
                            DTInfo.NgayQuaHanGQ = Utils.ConvertToDateTime(dr["HanGiaiQuyetDueDate"], DateTime.MinValue);

                        }

                        DTInfo.NgayNhapDon = Utils.ConvertToDateTime(dr["NgayNhapDon"], DateTime.MinValue);
                        if (DTInfo.NgayNhapDon != DateTime.MinValue)
                            DTInfo.NgayNhapDonStr = Format.FormatDate(DTInfo.NgayNhapDon);
                        DTInfo.NgayTiepNhan = DTInfo.NgayNhapDonStr;

                        DTInfo.TenLoaiKetQua = Utils.GetString(dr["TenLoaiKetQua"], string.Empty);
                        DTInfo.NgayXuLy = Utils.ConvertToDateTime(dr["NgayXuLy"], DateTime.MinValue);
                        if (DTInfo.NgayXuLy != DateTime.MinValue)
                            DTInfo.NgayXuLyStr = Format.FormatDate(DTInfo.NgayXuLy);
                        DTInfo.TenCoQuanGQ = Utils.GetString(dr["TenCoQuanGQ"], string.Empty);
                        DTInfo.TenCoQuanXL = Utils.GetString(dr["TenCoQuanXuLy"], string.Empty);
                        DTInfo.NgayChuyenDon = Utils.ConvertToDateTime(dr["NgayPhanCong"], DateTime.MinValue);
                        DTInfo.HanXuLy = Utils.ConvertToDateTime(dr["HanXuLy"], DateTime.MinValue);
                        DTInfo.SoDonThu = Utils.GetString(dr["SoDonThu"], string.Empty);
                        DTInfo.NgayPhan = Utils.ConvertToDateTime(dr["NgayPhan"], DateTime.MinValue);
                        DTInfo.SoCongVan = Utils.GetString(dr["SoCongVan"], string.Empty);
                        DTInfo.SoCVBaoCaoGQ = Utils.GetString(dr["SoCVBaoCaoGQ"], string.Empty);
                        DTInfo.NgayCVBaoCaoGQ = Utils.ConvertToDateTime(dr["NgayCVBaoCaoGQ"], DateTime.MinValue);
                        DTInfo.TenCQPhan = Utils.GetString(dr["TenCQPhan"], string.Empty);
                        DTInfo.LanGiaiQuyet = Utils.ConvertToInt32(dr["LanGiaiQuyet"], 0);

                        if (DTInfo.TenCoQuanGQ != "")
                        {
                            DTInfo.TenCoQuanXacMinh = DTInfo.TenCoQuanGQ;
                        }
                        else DTInfo.TenCoQuanXacMinh = DTInfo.TenCQPhan;
                        DTInfo.TenCoQuanTiepNhan = DTInfo.TenCoQuanXL;

                        if (DTInfo.StateID > (int)EnumState.LDDuyetXL)
                        {
                            DTInfo.TrangThaiDonID = 2;// da xu ly
                            DTInfo.TrangThaiDonThu = "Đã xử lý";
                        }
                        else
                        {
                            DTInfo.TrangThaiDonID = 1;// chua xu ly
                            DTInfo.TrangThaiDonThu = "Chưa xử lý";
                        }
                        if (DTInfo.StateID == (int)EnumState.KetThuc)
                        {
                            DTInfo.TrangThaiDonID = 5;// da giai quyet
                            DTInfo.TrangThaiDonThu = "Đã giải quyết";
                        }
                        else if (DTInfo.StateID == (int)EnumState.LDDuyetDQ || DTInfo.StateID == 21 || DTInfo.StateID == 22 || DTInfo.StateID == 25)
                        {
                            DTInfo.TrangThaiDonID = 4;// dang giai quyet
                            DTInfo.TrangThaiDonThu = "Đang giải quyết";
                        }
                        else if (DTInfo.StateID == 7 || DTInfo.StateID == 18 || DTInfo.StateID == 19)
                        {
                            DTInfo.TrangThaiDonID = 3;// chua giai quyet
                            DTInfo.TrangThaiDonThu = "Chưa giải quyết";
                        }
                        else if (DTInfo.StateID == 8)
                        {
                            DTInfo.NgayCapNhat = Utils.ConvertToDateTime(dr["NgayCapNhat"], DateTime.MinValue);
                            if (DTInfo.NgayCapNhat == DateTime.MinValue)
                            {
                                DTInfo.TrangThaiDonID = 3;// chua giai quyet
                                DTInfo.TrangThaiDonThu = "Chưa giải quyết";
                            }
                            else
                            {
                                DTInfo.TrangThaiDonID = 4;// dang giai quyet
                                DTInfo.TrangThaiDonThu = "Đang giải quyết";
                            }
                        }

                        List<FileHoSoInfo> fileYKienXL = new List<FileHoSoInfo>();
                        List<FileHoSoInfo> fileBanHanhQD = new List<FileHoSoInfo>();
                        int xemTaiLieuMat = 0;
                        DTInfo.lsFileYKienXuLy = new List<XuLyDonInfo>();
                        DTInfo.lsFileQuyetDinhGD = new List<FileHoSoInfo>();

                        if (DTInfo.XuLyDonID > 0)
                        {
                            if (xemTaiLieuMat == 1)
                            {

                                fileYKienXL = new FileHoSoDAL().GetFileYKienXuLyByXuLyDonID(DTInfo.XuLyDonID).ToList();
                            }
                            else
                            {
                                fileYKienXL = new FileHoSoDAL().GetFileYKienXuLyByXuLyDonID(DTInfo.XuLyDonID).Where(x => x.IsBaoMat != true).ToList();
                            }

                            //int step = 0;
                            for (int i = 0; i < fileYKienXL.Count; i++)
                            {
                                if (!string.IsNullOrEmpty(fileYKienXL[i].FileURL))
                                {
                                    if (string.IsNullOrEmpty(fileYKienXL[i].TenFile))
                                    {
                                        string[] arrtenFile = fileYKienXL[i].FileURL.Split('/');
                                        if (arrtenFile.Length > 0)
                                        {
                                            string[] duoiFile = arrtenFile[arrtenFile.Length - 1].Split('_');
                                            if (duoiFile.Length > 0)
                                            {
                                                fileYKienXL[i].TenFile = duoiFile[duoiFile.Length - 1];
                                            }
                                            else
                                            {
                                                fileYKienXL[i].TenFile = arrtenFile[arrtenFile.Length - 1];
                                            }
                                        }
                                    }
                                    fileYKienXL[i].FileURL = fileYKienXL[i].FileURL.Replace(" ", "%20");
                                }

                                XuLyDonInfo file = new XuLyDonInfo();
                                file.TenFile = fileYKienXL[i].TenFile;
                                file.FileUrl = fileYKienXL[i].FileURL;
                                DTInfo.lsFileYKienXuLy.Add(file);
                            }
                        }

                        if (true)
                        {
                            if (xemTaiLieuMat == 1)
                            {

                                fileBanHanhQD = new FileHoSoDAL().GetFileBanHanhQDByXuLyDonID(DTInfo.XuLyDonID).ToList();
                            }
                            else
                            {
                                fileBanHanhQD = new FileHoSoDAL().GetFileBanHanhQDByXuLyDonID(DTInfo.XuLyDonID).Where(x => x.IsBaoMat != true).ToList();
                            }
                            for (int j = 0; j < fileBanHanhQD.Count; j++)
                            {
                                if (!string.IsNullOrEmpty(fileBanHanhQD[j].FileURL))
                                {
                                    if (string.IsNullOrEmpty(fileBanHanhQD[j].TenFile))
                                    {
                                        string[] arrtenFile = fileBanHanhQD[j].FileURL.Split('/');
                                        if (arrtenFile.Length > 0)
                                        {
                                            string[] duoiFile = arrtenFile[arrtenFile.Length - 1].Split('_');
                                            if (duoiFile.Length > 0)
                                            {
                                                fileBanHanhQD[j].TenFile = duoiFile[duoiFile.Length - 1];
                                            }
                                            else
                                            {
                                                fileBanHanhQD[j].TenFile = arrtenFile[arrtenFile.Length - 1];
                                            }
                                        }
                                    }
                                    fileBanHanhQD[j].FileURL = fileBanHanhQD[j].FileURL.Replace(" ", "%20");
                                }
                            }
                            DTInfo.lsFileQuyetDinhGD = fileBanHanhQD;
                        }
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
            }

            return DTInfo;
        }

        private DonThuPortalInfo GetDataForPortal(SqlDataReader dr)
        {
            DonThuPortalInfo info = new DonThuPortalInfo();
            info.DonThuID = Utils.GetInt32(dr["DonThuID"], 0);

            info.NhomKNID = Utils.GetInt32(dr["NhomKNID"], 0);
            info.DoiTuongBiKNID = Utils.GetInt32(dr["DoiTuongBiKNID"], 0);
            info.LoaiKhieuTo1ID = Utils.GetInt32(dr["LoaiKhieuTo1ID"], 0);
            info.LoaiKhieuTo2ID = Utils.GetInt32(dr["LoaiKhieuTo2ID"], 0);
            info.LoaiKhieuTo3ID = Utils.GetInt32(dr["LoaiKhieuTo3ID"], 0);
            info.LoaiKhieuToID = Utils.GetInt32(dr["LoaiKhieuToID"], 0);
            info.NoiDungDon = Utils.GetString(dr["NoiDungDon"], string.Empty);

            info.TinhID = Utils.GetInt32(dr["TinhID"], 0);
            info.HuyenID = Utils.GetInt32(dr["HuyenID"], 0);
            info.XaID = Utils.GetInt32(dr["XaID"], 0);
            info.NgayVietDon = Utils.GetDateTime(dr["NgayVietDon"], Constant.DEFAULT_DATE);

            info.SoLan = Utils.ConvertToInt32(dr["SoLan"], 1);

            info.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
            info.TrangThaiDonID = Utils.ConvertToInt32(dr["TrangThaiDonID"], 0);
            info.LyDo = Utils.GetString(dr["LyDo"], String.Empty);

            info.TenLoaiKhieuTo1 = Utils.ConvertToString(dr["TenLoaiKhieuTo1"], String.Empty);
            info.TenLoaiKhieuTo2 = Utils.ConvertToString(dr["TenLoaiKhieuTo2"], String.Empty);
            info.TenLoaiKhieuTo3 = Utils.ConvertToString(dr["TenLoaiKhieuTo3"], String.Empty);

            info.NoiDungHuongDan = Utils.ConvertToString(dr["NoiDungHuongDan"], string.Empty);
            info.TenHuongGiaiQuyet = Utils.ConvertToString(dr["TenHuongGiaiQuyet"], string.Empty);

            return info;
        }
        #endregion


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
                result = SQLHelper.ExecuteScalar(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, DonThu_CountSoDonByLoaiKhieuTo, parameters);
            }
            catch
            {
                throw;
            }
            return Convert.ToInt32(result);
        }
        #region -- Đồng bộ dữ liệu portal
        public IList<DonThuPortalInfo> GetDonThuByDateCreated(DateTime ngayVietDon, int trangThaiDonID)
        {

            IList<DonThuPortalInfo> listDT = new List<DonThuPortalInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARM_NGAYVIETDON,SqlDbType.DateTime),
                new SqlParameter(@"TrangThaiDonID",SqlDbType.Int),

            };
            parameters[0].Value = ngayVietDon;
            parameters[1].Value = trangThaiDonID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DONTHU_BY_DATECREATED, parameters))
                {

                    while (dr.Read())
                    {
                        DonThuPortalInfo DTInfo = null;
                        DTInfo = GetDataForPortal(dr);
                        DTInfo.NguonDonDen = Utils.ConvertToInt32(dr["NguonDonDen"], 0);
                        DTInfo.TenCQChuyenDonDen = Utils.GetString(dr["TenCQChuyenDonDen"], string.Empty);
                        DTInfo.TenCanBoTiepNhan = Utils.GetString(dr["TenCanBoTiepNhan"], string.Empty);
                        DTInfo.TenCoQuanTiepNhan = Utils.GetString(dr["TenCoQuanTiepNhan"], string.Empty);
                        DTInfo.TenPhongBanTiepNhan = Utils.GetString(dr["TenPhongBanTiepNhan"], string.Empty);
                        DTInfo.CoQuanChuyenDonDi = Utils.GetString(dr["CoQuanChuyenDonDi"], string.Empty);
                        DTInfo.NgayChuyenDonSangCQKhac = Utils.ConvertToDateTime(dr["NgayChuyenDonDi"], DateTime.MinValue);
                        DTInfo.CQDaGiaiQuyetID = Utils.GetString(dr["CQDaGiaiQuyetID"], string.Empty);
                        var TenCanBoXl = Utils.GetString(dr["TenCanBoXuLy"], string.Empty);
                        if (TenCanBoXl != "")
                        {
                            DTInfo.TenCanBoXuLy = Utils.GetString(dr["TenCanBoXuLy"], string.Empty);
                        }
                        else
                        {
                            DTInfo.TenCanBoXuLy = Utils.GetString(dr["TenCanBoPhanXL"], string.Empty);
                        }


                        DTInfo.TenPhongBanXuLy = Utils.GetString(dr["TenPhongBanXuLy"], string.Empty);
                        //DTInfo.NgayQuaHanGQ = Utils.ConvertToDateTime(dr["HanGiaiQuyet"], DateTime.MinValue);
                        DTInfo.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        if (DTInfo.StateID == (int)EnumState.LDPhanGQ)
                        {
                            DTInfo.NgayQuaHanGQ = Utils.ConvertToDateTime(dr["HanGiaiQuyet"], DateTime.MinValue);
                        }
                        else
                        {
                            DTInfo.NgayQuaHanGQ = Utils.ConvertToDateTime(dr["HanGiaiQuyetDueDate"], DateTime.MinValue);

                        }

                        DTInfo.NgayNhapDon = Utils.ConvertToDateTime(dr["NgayNhapDon"], DateTime.MinValue);
                        if (DTInfo.NgayNhapDon != DateTime.MinValue)
                            DTInfo.NgayNhapDonStr = Format.FormatDate(DTInfo.NgayNhapDon);

                        DTInfo.TenLoaiKetQua = Utils.GetString(dr["TenLoaiKetQua"], string.Empty);
                        DTInfo.NgayXuLy = Utils.ConvertToDateTime(dr["NgayXuLy"], DateTime.MinValue);
                        if (DTInfo.NgayXuLy != DateTime.MinValue)
                            DTInfo.NgayXuLyStr = Format.FormatDate(DTInfo.NgayXuLy);
                        //DTInfo.TenCoQuanGQ = Utils.GetString(dr["TenCoQuanGQ"], string.Empty);
                        DTInfo.TenCoQuanXL = Utils.GetString(dr["TenCoQuanXuLy"], string.Empty);


                        DTInfo.NgayChuyenDon = Utils.ConvertToDateTime(dr["NgayPhanCong"], DateTime.MinValue);
                        DTInfo.HanXuLy = Utils.ConvertToDateTime(dr["HanXuLy"], DateTime.MinValue);
                        DTInfo.SoDonThu = Utils.GetString(dr["SoDonThu"], string.Empty);
                        DTInfo.NgayPhan = Utils.ConvertToDateTime(dr["NgayPhan"], DateTime.MinValue);
                        DTInfo.SoCongVan = Utils.GetString(dr["SoCongVan"], string.Empty);
                        DTInfo.SoCVBaoCaoGQ = Utils.GetString(dr["SoCVBaoCaoGQ"], string.Empty);
                        DTInfo.NgayCVBaoCaoGQ = Utils.ConvertToDateTime(dr["NgayCVBaoCaoGQ"], DateTime.MinValue);
                        DTInfo.TenCQPhan = Utils.GetString(dr["TenCQPhan"], string.Empty);
                        DTInfo.LanGiaiQuyet = Utils.ConvertToInt32(dr["LanGiaiQuyet"], 0);
                        DTInfo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);

                        DTInfo.CoQuanXuLyID = Utils.ConvertToInt32(dr["CoQuanXuLyID"], 0);
                        DTInfo.CoQuanBanHanhID = Utils.ConvertToInt32(dr["CoQuanBanHanhID"], 0);
                        DTInfo.CoQuanGiaiQuyetID = Utils.ConvertToInt32(dr["CoQuanGiaiQuyetID"], 0);

                        DTInfo.SoDatPhaiThu = Utils.ConvertToInt32(dr["SoDatPhaiThu"], 0);
                        DTInfo.SoTienPhaiThu = Utils.ConvertToInt32(dr["SoTienPhaiThu"], 0);
                        DTInfo.SoDoiTuongBiXuLy = Utils.ConvertToInt32(dr["SoDoiTuongBiXuLy"], 0);

                        DTInfo.NgayRaKQ = Utils.ConvertToDateTime(dr["NgayRaKQ"], DateTime.MinValue);
                        DTInfo.DueDate = Utils.ConvertToDateTime(dr["DueDate"], DateTime.MinValue);
                        listDT.Add(DTInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return listDT;
        }
        public DateTime GetCurrentDateForSync()
        {
            DateTime dt = Utils.ConvertToDateTime("01/01/1973", DateTime.MinValue);
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "ListDate_GetCurrentDate", null))
                {
                    while (dr.Read())
                    {
                        dt = Utils.ConvertToDateTime(dr["Datex"], dt);
                    }
                }
            }
            catch
            {

            }
            return dt;
        }
        public int InsertListDateSync(DateTime dt)
        {

            object val = null;

            SqlParameter parameters = new SqlParameter(@"Datex", SqlDbType.DateTime);

            parameters.Value = dt;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, "ListDate_Insert", parameters);
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
        public int UpdateDongBo(int xldID)
        {
            object val = null;
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int),

            };
            parms[0].Value = xldID;


            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, "XuLyDon_UpdateDongBo", parms);
                        trans.Commit();
                        val = 1;
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
        #endregion
        public List<XuLyDonInfo> GetXuLyDonByCMND(string cmnd, int coQuanID, int start, int end)
        {

            List<XuLyDonInfo> DTInfo = new List<XuLyDonInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(@"CMND",SqlDbType.NVarChar,50),
                 new SqlParameter(PARAM_CO_QUAN_ID,SqlDbType.Int),
                  new SqlParameter(@"Start",SqlDbType.Int),
                   new SqlParameter(@"End",SqlDbType.Int),
            };
            parameters[0].Value = cmnd;
            parameters[1].Value = coQuanID;
            parameters[2].Value = start;
            parameters[3].Value = end;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "NV_XuLyDon_GetByCMND_New", parameters))
                {
                    while (dr.Read())
                    {
                        XuLyDonInfo item = new XuLyDonInfo();
                        item.DonThuID = Utils.ConvertToInt32(dr["DonThuID"], 0);
                        item.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
                        DTInfo.Add(item);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return DTInfo;
        }

        public List<XuLyDonInfo> TraCuuQuyetDinhGiaiQuyet(TraCuuParams p, ref int TotalRow)
        {

            List<XuLyDonInfo> DTInfo = new List<XuLyDonInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(@"TuNgay", SqlDbType.DateTime),
                new SqlParameter(@"DenNgay", SqlDbType.DateTime),
                new SqlParameter(@"SoDonThu",SqlDbType.NVarChar),
                new SqlParameter(@"ChuDon",SqlDbType.NVarChar),
                new SqlParameter(@"CMND",SqlDbType.NVarChar),
                new SqlParameter(@"CoQuanID",SqlDbType.Int),
                new SqlParameter(@"Limit",SqlDbType.Int),
                new SqlParameter(@"Offset",SqlDbType.Int),
                new SqlParameter(@"TotalRow",SqlDbType.Int),
                new SqlParameter(@"NgayNopDon", SqlDbType.DateTime),
            };
            parameters[0].Value = p.TuNgay ?? Convert.DBNull;
            parameters[1].Value = p.DenNgay ?? Convert.DBNull;
            parameters[2].Value = p.SoDonThu ?? Convert.DBNull;
            parameters[3].Value = p.ChuDon != null ? p.ChuDon.Trim() : "";
            parameters[4].Value = p.CCCD ?? Convert.DBNull;
            parameters[5].Value = p.CoQuanID ?? Convert.DBNull;
            parameters[6].Value = p.Limit;
            parameters[7].Value = p.Offset;
            parameters[8].Direction = ParameterDirection.Output;
            parameters[8].Size = 8;
            parameters[9].Value = p.NgayNopDon ?? Convert.DBNull;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "NV_XuLyDon_TraCuu_New", parameters))
                {
                    while (dr.Read())
                    {
                        XuLyDonInfo item = new XuLyDonInfo();
                        item.DonThuID = Utils.ConvertToInt32(dr["DonThuID"], 0);
                        item.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
                        DTInfo.Add(item);
                    }
                    dr.Close();
                }
                TotalRow = Utils.ConvertToInt32(parameters[8].Value, 0);
            }
            catch
            {
            }
            return DTInfo;
        }

        public List<XuLyDonInfo> DanhSachHoSoDuocTraCuu(TraCuuParams p, ref int TotalRow)
        {

            List<XuLyDonInfo> DTInfo = new List<XuLyDonInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(@"TuNgay", SqlDbType.DateTime),
                new SqlParameter(@"DenNgay", SqlDbType.DateTime),
                new SqlParameter(@"SoDonThu",SqlDbType.NVarChar),
                new SqlParameter(@"ChuDon",SqlDbType.NVarChar),
                new SqlParameter(@"CMND",SqlDbType.NVarChar),
                new SqlParameter(@"CoQuanID",SqlDbType.Int),
                new SqlParameter(@"Limit",SqlDbType.Int),
                new SqlParameter(@"Offset",SqlDbType.Int),
                new SqlParameter(@"TotalRow",SqlDbType.Int),
                new SqlParameter(@"NgayNopDon", SqlDbType.DateTime),
            };
            parameters[0].Value = p.TuNgay ?? Convert.DBNull;
            parameters[1].Value = p.DenNgay ?? Convert.DBNull;
            parameters[2].Value = p.SoDonThu ?? Convert.DBNull;
            parameters[3].Value = p.ChuDon != null ? p.ChuDon.Trim() : "";
            parameters[4].Value = p.CCCD ?? Convert.DBNull;
            parameters[5].Value = p.CoQuanID ?? Convert.DBNull;
            parameters[6].Value = p.Limit;
            parameters[7].Value = p.Offset;
            parameters[8].Direction = ParameterDirection.Output;
            parameters[8].Size = 8;
            parameters[9].Value = p.NgayNopDon ?? Convert.DBNull;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "NV_XuLyDon_HoSoDuocTraCuu", parameters))
                {
                    while (dr.Read())
                    {
                        XuLyDonInfo item = new XuLyDonInfo();
                        item.DonThuID = Utils.ConvertToInt32(dr["DonThuID"], 0);
                        item.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
                        DTInfo.Add(item);
                    }
                    dr.Close();
                }
                TotalRow = Utils.ConvertToInt32(parameters[8].Value, 0);
            }
            catch
            {
            }
            return DTInfo;
        }

        public List<LichSuTraCuuModel> LichSuTraCuu(TraCuuParams p, ref int TotalRow)
        {
            List<LichSuTraCuuModel> DTInfo = new List<LichSuTraCuuModel>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(@"TuNgay", SqlDbType.DateTime),
                new SqlParameter(@"DenNgay", SqlDbType.DateTime),
                new SqlParameter(@"SoDonThu",SqlDbType.NVarChar),
                new SqlParameter(@"ChuDon",SqlDbType.NVarChar),
                new SqlParameter(@"CMND",SqlDbType.NVarChar),
                new SqlParameter(@"CoQuanID",SqlDbType.Int),
                new SqlParameter(@"Limit",SqlDbType.Int),
                new SqlParameter(@"Offset",SqlDbType.Int),
                new SqlParameter(@"TotalRow",SqlDbType.Int),
                new SqlParameter(@"NgayNopDon", SqlDbType.DateTime),
            };
            parameters[0].Value = p.TuNgay ?? Convert.DBNull;
            parameters[1].Value = p.DenNgay ?? Convert.DBNull;
            parameters[2].Value = p.SoDonThu ?? Convert.DBNull;
            parameters[3].Value = p.ChuDon != null ? p.ChuDon.Trim() : "";
            parameters[4].Value = p.CCCD ?? Convert.DBNull;
            parameters[5].Value = p.CoQuanID ?? Convert.DBNull;
            parameters[6].Value = p.Limit;
            parameters[7].Value = p.Offset;
            parameters[8].Direction = ParameterDirection.Output;
            parameters[8].Size = 8;
            parameters[9].Value = p.NgayNopDon ?? Convert.DBNull;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "NV_LichSuTraCuu_GetBySearch", parameters))
                {
                    while (dr.Read())
                    {
                        LichSuTraCuuModel item = new LichSuTraCuuModel();
                        item.TraCuuID = Utils.ConvertToInt32(dr["TraCuuID"], 0);
                        item.SoDonThu = Utils.ConvertToString(dr["SoDonThu"], string.Empty);
                        item.ChuDon = Utils.ConvertToString(dr["ChuDon"], string.Empty);
                        item.CCCD = Utils.ConvertToString(dr["CCCD"], string.Empty);
                        item.NgayNopDon = Utils.ConvertToNullableDateTime(dr["NgayNopDon"], null);
                        item.NgayTraCuu = Utils.ConvertToNullableDateTime(dr["NgayTraCuu"], null);
                        DTInfo.Add(item);
                    }
                    dr.Close();
                }
                TotalRow = Utils.ConvertToInt32(parameters[8].Value, 0);
            }
            catch
            {
            }
            return DTInfo;
        }

        public List<XuLyDonLog> GetXuLyDonLogs(int xuLyDonID)
        {
            List<XuLyDonLog> lstItem = new List<XuLyDonLog>();
            SqlParameter[] parameters = new SqlParameter[]{
                 new SqlParameter(PARAM_XULYDON_ID,SqlDbType.Int),
            };
            parameters[0].Value = xuLyDonID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "TransitionHistory_GetByXuLyDonID", parameters))
                {
                    while (dr.Read())
                    {
                        XuLyDonLog item = new XuLyDonLog();
                        item.CurrentStateID = Utils.ConvertToInt32(dr["CurrentStateID"], 0);
                        item.NextStateID = Utils.ConvertToInt32(dr["NextStateID"], 0);
                        item.CurrentState = Utils.ConvertToString(dr["CurrentState"], string.Empty);
                        item.NextState = Utils.ConvertToString(dr["NextState"], string.Empty);
                        lstItem.Add(item);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return lstItem;
        }

        public List<TheoDoiXuLyInfo> GetPheDuyetBaoCaoXacMinh(int xuLyDonID)
        {
            SqlParameter parm = new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int);
            parm.Value = xuLyDonID;

            List<TheoDoiXuLyInfo> dtList = new List<TheoDoiXuLyInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "YKienGiaiQuyet_GetFileDTCanDuyetGiaiQuyet_New", parm))
                {
                    while (dr.Read())
                    {
                        TheoDoiXuLyInfo dtInfo = new TheoDoiXuLyInfo();
                        dtInfo.FileDonThuCanDuyetGiaiQuyetID = Utils.ConvertToInt32(dr["FileDonThuCanDuyetGiaiQuyetID"], 0);
                        dtInfo.FileID = Utils.ConvertToInt32(dr["FileID"], 0);
                        dtInfo.TenFile = Utils.ConvertToString(dr["TenFileNew"], string.Empty);
                        dtInfo.NhomFileID = Utils.ConvertToInt32(dr["NhomFileID"], 0);
                        dtInfo.TenNhomFile = Utils.ConvertToString(dr["TenNhomFile"], string.Empty);
                        dtInfo.ThuTuHienThiNhom = Utils.ConvertToInt32(dr["ThuTuHienThi"], 0);
                        dtInfo.ThuTuHienThiFile = Utils.ConvertToInt32(dr["ThuTuHienThiFile"], 0);
                        dtInfo.DuongDanFile = Utils.GetString(dr["FileUrl"], string.Empty);
                        //dtInfo.TenCanBo = Utils.GetString(dr["TenCanBo"], string.Empty);
                        dtInfo.TenCapBoUp = Utils.ConvertToString(dr["CanBoUpFile"], string.Empty);
                        dtInfo.TomTat = Utils.ConvertToString(dr["TomTat"], string.Empty);
                        //dtInfo.TenFile = Utils.ConvertToString(dr["TenFile"], string.Empty);
                        dtInfo.CanBoID = Utils.ConvertToInt32(dr["NguoiUp"], 0);
                        dtInfo.TenCoQuanUp = new CoQuan().GetCoQuanByCanBoID(dtInfo.CanBoID).TenCoQuan;
                        dtInfo.TenBuoc = "Duyệt báo cáo xác minh";
                        dtInfo.IsBaoMat = Utils.ConvertToBoolean(dr["IsBaoMat"], false);
                        dtInfo.NgayCapNhat = Utils.ConvertToDateTime(dr["NgayUp"], DateTime.MinValue);
                        dtInfo.StringNgayCapNhat = "";
                        if (dtInfo.NgayCapNhat != DateTime.MinValue)
                            dtInfo.StringNgayCapNhat = dtInfo.NgayCapNhat.ToString("dd/MM/yyyy");
                        if (dtInfo.DuongDanFile != "")
                        {
                            dtList.Add(dtInfo);
                        }
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

        public int countDonThuByCMND(string cmnd, int coQuanID)
        {
            int result = 0;
            SqlParameter[] parameters = new SqlParameter[]
               {
                    new SqlParameter(@"CMND",SqlDbType.NVarChar,50),
                    new SqlParameter(PARAM_CO_QUAN_ID,SqlDbType.Int),
               };
            parameters[0].Value = cmnd;
            parameters[1].Value = coQuanID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "NV_XuLyDon_GetByCMND_Count", parameters))
                {
                    while (dr.Read())
                    {
                        result = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public int countDonThuByCoQuanTiepNhan(int coQuanID, string SoDonThu, DateTime tuNgay, DateTime denNgay)
        {
            int result = 0;
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter("@CoQuanID", SqlDbType.Int),
                new SqlParameter("@SoDonThu", SqlDbType.NVarChar),
                new SqlParameter("@TuNgay", SqlDbType.DateTime),
                new SqlParameter("@DenNgay", SqlDbType.DateTime)
            };
            parm[0].Value = coQuanID;
            parm[1].Value = SoDonThu;
            parm[2].Value = tuNgay;
            parm[3].Value = denNgay;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "SearchDonThuByCoQuanTiepNhan_Count", parm))
                {
                    while (dr.Read())
                    {
                        result = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public List<DonThuPortalInfo> SearchDonThuByCoQuanTiepNhan(int coQuanID, string SoDonThu, DateTime tuNgay, DateTime denNgay, int Start, int End, int type)
        {

            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter("@CoQuanID", SqlDbType.Int),
                new SqlParameter("@SoDonThu", SqlDbType.NVarChar),
                new SqlParameter("@TuNgay", SqlDbType.DateTime),
                new SqlParameter("@DenNgay", SqlDbType.DateTime),
                new SqlParameter("@Start", SqlDbType.Int),
                new SqlParameter("@End", SqlDbType.Int),
                new SqlParameter("@Type", SqlDbType.Int),
            };
            parm[0].Value = coQuanID;
            parm[1].Value = SoDonThu;
            parm[2].Value = tuNgay;
            parm[3].Value = denNgay;
            parm[4].Value = Start;
            parm[5].Value = End;
            parm[6].Value = type;
            if (tuNgay == DateTime.MinValue)
            {
                parm[2].Value = DBNull.Value;
            }
            if (denNgay == DateTime.MinValue)
            {
                parm[3].Value = DBNull.Value;
            }
            var query = new DataTable();
            List<DonThuPortalInfo> DonThus = new List<DonThuPortalInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "SearchDonThuByCoQuanTiepNhan", parm))
                {
                    query.Load(dr);
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            #region new
            var dataRows = query.AsEnumerable();
            int xemTaiLieuMat = 0;
            List<FileHoSoInfo> fileYKienXLAll = new List<FileHoSoInfo>();
            List<FileHoSoInfo> fileBanHanhQDAll = new List<FileHoSoInfo>();
            var listXuLyDonID = dataRows.Select(x => x.Field<int>("XuLyDonID")).ToList();
            if (xemTaiLieuMat == 1)
            {
                fileYKienXLAll = new FileHoSoDAL().GetFileYKienXuLyByListXuLyDon1(listXuLyDonID).ToList();
                fileBanHanhQDAll = new FileHoSoDAL().GetFileBanHanhQDByListXuLyDon1(listXuLyDonID).ToList();
            }
            else
            {
                fileYKienXLAll = new FileHoSoDAL().GetFileYKienXuLyByListXuLyDon1(listXuLyDonID).Where(x => x.IsBaoMat != true).ToList();
                fileBanHanhQDAll = new FileHoSoDAL().GetFileBanHanhQDByListXuLyDon1(listXuLyDonID).Where(x => x.IsBaoMat != true).ToList();
            }

            foreach (var item in dataRows)
            {
                // don tu info
                var DTInfo = new DonThuPortalInfo();
                DTInfo.TongDonThu = Utils.ConvertToInt32(item.Field<int?>("CountNum"), 0);
                DTInfo.DonThuID = Utils.ConvertToInt32(item.Field<int?>("DonThuID"), 0);

                DTInfo.NhomKNID = Utils.ConvertToInt32(item.Field<int?>("NhomKNID"), 0);
                DTInfo.DoiTuongBiKNID = Utils.ConvertToInt32(item.Field<int?>("DoiTuongBiKNID"), 0);
                DTInfo.LoaiKhieuTo1ID = Utils.ConvertToInt32(item.Field<int?>("LoaiKhieuTo1ID"), 0);
                DTInfo.LoaiKhieuTo2ID = Utils.ConvertToInt32(item.Field<int?>("LoaiKhieuTo2ID"), 0);
                DTInfo.LoaiKhieuTo3ID = Utils.ConvertToInt32(item.Field<int?>("LoaiKhieuTo3ID"), 0);
                DTInfo.LoaiKhieuToID = Utils.ConvertToInt32(item.Field<int?>("LoaiKhieuToID"), 0);
                DTInfo.NoiDungDon = Utils.GetString(item.Field<string>("NoiDungDon"), string.Empty);

                DTInfo.TinhID = Utils.ConvertToInt32(item.Field<int?>("TinhID"), 0);
                DTInfo.HuyenID = Utils.ConvertToInt32(item.Field<int?>("HuyenID"), 0);
                DTInfo.XaID = Utils.ConvertToInt32(item.Field<int?>("XaID"), 0);
                DTInfo.NgayVietDon = Utils.ConvertToDateTime(item.Field<DateTime?>("NgayVietDon"), DateTime.MinValue);

                DTInfo.SoLan = Utils.ConvertToInt32(item.Field<int?>("SoLan"), 1);

                DTInfo.XuLyDonID = Utils.ConvertToInt32(item.Field<int?>("XuLyDonID"), 0);
                //DTInfo.TrangThaiDonID = Utils.ConvertToInt32(item.Field<int?>("TrangThaiDonID"), 0);
                DTInfo.LyDo = Utils.GetString(item.Field<string>("LyDo"), String.Empty);

                DTInfo.TenLoaiKhieuTo1 = Utils.ConvertToString(item.Field<string>("TenLoaiKhieuTo1"), String.Empty);
                DTInfo.TenLoaiKhieuTo2 = Utils.ConvertToString(item.Field<string>("TenLoaiKhieuTo2"), String.Empty);
                DTInfo.TenLoaiKhieuTo3 = Utils.ConvertToString(item.Field<string>("TenLoaiKhieuTo3"), String.Empty);

                DTInfo.NoiDungHuongDan = Utils.ConvertToString(item.Field<string>("NoiDungHuongDan"), string.Empty);
                DTInfo.TenHuongGiaiQuyet = Utils.ConvertToString(item.Field<string>("TenHuongGiaiQuyet"), string.Empty);
                //DTInfo.NguonDonDen = Utils.ConvertToInt32(item.Field<int?>("NguonDonDen"), 0);    
                DTInfo.TenCQChuyenDonDen = Utils.ConvertToString(item.Field<string>("TenCQChuyenDonDen"), string.Empty);
                DTInfo.TenCanBoTiepNhan = Utils.GetString(item.Field<string>("TenCanBoTiepNhan"), string.Empty);
                DTInfo.TenCoQuanTiepNhan = Utils.GetString(item.Field<string>("TenCoQuanTiepNhan"), string.Empty);
                DTInfo.TenPhongBanTiepNhan = Utils.GetString(item.Field<string>("TenPhongBanTiepNhan"), string.Empty);
                DTInfo.TenCoQuanBanHanh = Utils.GetString(item.Field<string>("TenCoQuanBanHanh"), string.Empty);
                DTInfo.CoQuanChuyenDonDi = Utils.GetString(item.Field<string>("CoQuanChuyenDonDi"), string.Empty);
                DTInfo.NgayChuyenDonSangCQKhac = Utils.ConvertToDateTime(item.Field<DateTime?>("NgayChuyenDonDi"), DateTime.MinValue);
                DTInfo.CQDaGiaiQuyetID = Utils.GetString(item.Field<string>("CQDaGiaiQuyetID"), string.Empty);
                var TenCanBoXl = Utils.GetString(item.Field<string>("TenCanBoXuLy"), string.Empty);
                if (TenCanBoXl != "")
                {
                    DTInfo.TenCanBoXuLy = Utils.GetString(item.Field<string>("TenCanBoXuLy"), string.Empty);
                }
                else
                {
                    DTInfo.TenCanBoXuLy = Utils.GetString(item.Field<string>("TenCanBoPhanXL"), string.Empty);
                }


                DTInfo.TenPhongBanXuLy = Utils.GetString(item.Field<string>("TenPhongBanXuLy"), string.Empty);
                //DTInfo.NgayQuaHanGQ = Utils.ConvertToDateTime(dr["HanGiaiQuyet"], DateTime.MinValue);
                DTInfo.StateID = Utils.ConvertToInt32(item.Field<int?>("StateID"), 0);
                if (DTInfo.StateID == (int)EnumState.LDPhanGQ)
                {
                    DTInfo.NgayQuaHanGQ = Utils.ConvertToDateTime(item.Field<DateTime?>("HanGiaiQuyet"), DateTime.MinValue);
                }
                else
                {
                    DTInfo.NgayQuaHanGQ = Utils.ConvertToDateTime(item.Field<DateTime?>("HanGiaiQuyetDueDate"), DateTime.MinValue);

                }

                DTInfo.NgayNhapDon = Utils.ConvertToDateTime(item.Field<DateTime?>("NgayNhapDon"), DateTime.MinValue);
                if (DTInfo.NgayNhapDon != DateTime.MinValue)
                    DTInfo.NgayNhapDonStr = Format.FormatDate(DTInfo.NgayNhapDon);
                DTInfo.NgayTiepNhan = DTInfo.NgayNhapDonStr;

                DateTime NgayBanHanh = Utils.ConvertToDateTime(item.Field<DateTime?>("NgayBanHanh"), DateTime.MinValue);
                if (NgayBanHanh != DateTime.MinValue)
                    DTInfo.NgayBanHanh = Format.FormatDate(NgayBanHanh);

                DTInfo.TenLoaiKetQua = Utils.GetString(item.Field<string>("TenLoaiKetQua"), string.Empty);
                DTInfo.NgayXuLy = Utils.ConvertToDateTime(item.Field<DateTime?>("NgayXuLy"), DateTime.MinValue);
                if (DTInfo.NgayXuLy != DateTime.MinValue)
                    DTInfo.NgayXuLyStr = Format.FormatDate(DTInfo.NgayXuLy);
                DTInfo.TenCoQuanGQ = Utils.GetString(item.Field<string>("TenCoQuanGQ"), string.Empty);
                DTInfo.TenCoQuanXL = Utils.GetString(item.Field<string>("TenCoQuanXuLy"), string.Empty);
                DTInfo.NgayChuyenDon = Utils.ConvertToDateTime(item.Field<DateTime?>("NgayPhanCong"), DateTime.MinValue);
                DTInfo.HanXuLy = Utils.ConvertToDateTime(item.Field<DateTime?>("HanXuLy"), DateTime.MinValue);
                DTInfo.SoDonThu = Utils.GetString(item.Field<string>("SoDonThu"), string.Empty);
                DTInfo.NgayPhan = Utils.ConvertToDateTime(item.Field<DateTime?>("NgayPhan"), DateTime.MinValue);
                DTInfo.SoCongVan = Utils.GetString(item.Field<string>("SoCongVan"), string.Empty);
                DTInfo.SoCVBaoCaoGQ = Utils.GetString(item.Field<string>("SoCVBaoCaoGQ"), string.Empty);
                DTInfo.NgayCVBaoCaoGQ = Utils.ConvertToDateTime(item.Field<DateTime?>("NgayCVBaoCaoGQ"), DateTime.MinValue);
                DTInfo.TenCQPhan = Utils.GetString(item.Field<string>("TenCQPhan"), string.Empty);
                DTInfo.LanGiaiQuyet = Utils.ConvertToInt32(item.Field<int?>("LanGiaiQuyet"), 0);

                if (DTInfo.TenCoQuanGQ != "")
                {
                    DTInfo.TenCoQuanXacMinh = DTInfo.TenCoQuanGQ;
                }
                else DTInfo.TenCoQuanXacMinh = DTInfo.TenCQPhan;
                DTInfo.TenCoQuanTiepNhan = DTInfo.TenCoQuanXL;

                if (DTInfo.StateID > (int)EnumState.LDDuyetXL)
                {
                    DTInfo.TrangThaiDonID = 2;// da xu ly
                    DTInfo.TrangThaiDonThu = "Đã xử lý";
                }
                else
                {
                    DTInfo.TrangThaiDonID = 1;// chua xu ly
                    DTInfo.TrangThaiDonThu = "Chưa xử lý";
                }
                if (DTInfo.StateID == (int)EnumState.KetThuc)
                {
                    DTInfo.TrangThaiDonID = 5;// da giai quyet
                    DTInfo.TrangThaiDonThu = "Đã giải quyết";
                }
                else if (DTInfo.StateID == (int)EnumState.LDDuyetDQ || DTInfo.StateID == 21 || DTInfo.StateID == 22 || DTInfo.StateID == 25)
                {
                    DTInfo.TrangThaiDonID = 4;// dang giai quyet
                    DTInfo.TrangThaiDonThu = "Đang giải quyết";
                }
                else if (DTInfo.StateID == 7 || DTInfo.StateID == 18 || DTInfo.StateID == 19)
                {
                    DTInfo.TrangThaiDonID = 3;// chua giai quyet
                    DTInfo.TrangThaiDonThu = "Chưa giải quyết";
                }
                else if (DTInfo.StateID == 8)
                {
                    DTInfo.NgayCapNhat = Utils.ConvertToDateTime(item.Field<DateTime?>("NgayCapNhat"), DateTime.MinValue);
                    if (DTInfo.NgayCapNhat == DateTime.MinValue)
                    {
                        DTInfo.TrangThaiDonID = 3;// chua giai quyet
                        DTInfo.TrangThaiDonThu = "Chưa giải quyết";
                    }
                    else
                    {
                        DTInfo.TrangThaiDonID = 4;// dang giai quyet
                        DTInfo.TrangThaiDonThu = "Đang giải quyết";
                    }
                }

                DTInfo.lsFileYKienXuLy = new List<XuLyDonInfo>();
                List<FileHoSoInfo> fileYKienXL = new List<FileHoSoInfo>();
                fileYKienXL = fileYKienXLAll.Where(x => x.XuLyDonID == item.Field<int>("XuLyDonID")).ToList();
                for (int i = 0; i < fileYKienXL.Count; i++)
                {
                    if (!string.IsNullOrEmpty(fileYKienXL[i].FileURL))
                    {
                        if (string.IsNullOrEmpty(fileYKienXL[i].TenFile))
                        {
                            string[] arrtenFile = fileYKienXL[i].FileURL.Split('/');
                            if (arrtenFile.Length > 0)
                            {
                                string[] duoiFile = arrtenFile[arrtenFile.Length - 1].Split('_');
                                if (duoiFile.Length > 0)
                                {
                                    fileYKienXL[i].TenFile = duoiFile[duoiFile.Length - 1];
                                }
                                else
                                {
                                    fileYKienXL[i].TenFile = arrtenFile[arrtenFile.Length - 1];
                                }
                            }
                        }
                        fileYKienXL[i].FileURL = fileYKienXL[i].FileURL.Replace(" ", "%20");
                    }

                    XuLyDonInfo file = new XuLyDonInfo();
                    file.TenFile = fileYKienXL[i].TenFile;
                    file.FileUrl = fileYKienXL[i].FileURL;
                    DTInfo.lsFileYKienXuLy.Add(file);
                }
                // kết quả
                List<FileHoSoInfo> fileBanHanhQD = new List<FileHoSoInfo>();
                fileBanHanhQD = fileBanHanhQDAll.Where(x => x.XuLyDonID == DTInfo.XuLyDonID).ToList();
                for (int j = 0; j < fileBanHanhQD.Count; j++)
                {
                    if (!string.IsNullOrEmpty(fileBanHanhQD[j].FileURL))
                    {
                        if (string.IsNullOrEmpty(fileBanHanhQD[j].TenFile))
                        {
                            string[] arrtenFile = fileBanHanhQD[j].FileURL.Split('/');
                            if (arrtenFile.Length > 0)
                            {
                                string[] duoiFile = arrtenFile[arrtenFile.Length - 1].Split('_');
                                if (duoiFile.Length > 0)
                                {
                                    fileBanHanhQD[j].TenFile = duoiFile[duoiFile.Length - 1];
                                }
                                else
                                {
                                    fileBanHanhQD[j].TenFile = arrtenFile[arrtenFile.Length - 1];
                                }
                            }
                        }
                        fileBanHanhQD[j].FileURL = fileBanHanhQD[j].FileURL.Replace(" ", "%20");
                    }
                }
                DTInfo.lsFileQuyetDinhGD = fileBanHanhQD;

                DonThus.Add(DTInfo);
            }
            #endregion

            return DonThus;
        }

        public List<DonThuPortalInfo> GetDSDonThuDaCoKetQuaGiaiQuyet(int Start, int End)
        {
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter("@Start", SqlDbType.Int),
                new SqlParameter("@End", SqlDbType.Int),
            };

            parm[0].Value = Start;
            parm[1].Value = End;
            var query = new DataTable();
            List<DonThuPortalInfo> DonThus = new List<DonThuPortalInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "SearchDonThuDaCoKetQuaGiaiQuyet", parm))
                {
                    query.Load(dr);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            #region new
            var dataRows = query.AsEnumerable();
            int xemTaiLieuMat = 0;
            List<FileHoSoInfo> fileYKienXLAll = new List<FileHoSoInfo>();
            List<FileHoSoInfo> fileBanHanhQDAll = new List<FileHoSoInfo>();
            var listXuLyDonID = dataRows.Select(x => x.Field<int>("XuLyDonID")).ToList();
            if (xemTaiLieuMat == 1)
            {
                fileYKienXLAll = new FileHoSoDAL().GetFileYKienXuLyByListXuLyDon1(listXuLyDonID).ToList();
                fileBanHanhQDAll = new FileHoSoDAL().GetFileBanHanhQDByListXuLyDon1(listXuLyDonID).ToList();
            }
            else
            {
                fileYKienXLAll = new FileHoSoDAL().GetFileYKienXuLyByListXuLyDon1(listXuLyDonID).Where(x => x.IsBaoMat != true).ToList();
                fileBanHanhQDAll = new FileHoSoDAL().GetFileBanHanhQDByListXuLyDon1(listXuLyDonID).Where(x => x.IsBaoMat != true).ToList();
            }

            foreach (var item in dataRows)
            {
                // don tu info
                var DTInfo = new DonThuPortalInfo();
                DTInfo.TongDonThu = Utils.ConvertToInt32(item.Field<int?>("CountNum"), 0);
                DTInfo.DonThuID = Utils.ConvertToInt32(item.Field<int?>("DonThuID"), 0);
                DTInfo.NhomKNID = Utils.ConvertToInt32(item.Field<int?>("NhomKNID"), 0);
                DTInfo.DoiTuongBiKNID = Utils.ConvertToInt32(item.Field<int?>("DoiTuongBiKNID"), 0);
                DTInfo.LoaiKhieuTo1ID = Utils.ConvertToInt32(item.Field<int?>("LoaiKhieuTo1ID"), 0);
                DTInfo.LoaiKhieuTo2ID = Utils.ConvertToInt32(item.Field<int?>("LoaiKhieuTo2ID"), 0);
                DTInfo.LoaiKhieuTo3ID = Utils.ConvertToInt32(item.Field<int?>("LoaiKhieuTo3ID"), 0);
                DTInfo.LoaiKhieuToID = Utils.ConvertToInt32(item.Field<int?>("LoaiKhieuToID"), 0);
                DTInfo.NoiDungDon = Utils.GetString(item.Field<string>("NoiDungDon"), string.Empty);

                DTInfo.TinhID = Utils.ConvertToInt32(item.Field<int?>("TinhID"), 0);
                DTInfo.HuyenID = Utils.ConvertToInt32(item.Field<int?>("HuyenID"), 0);
                DTInfo.XaID = Utils.ConvertToInt32(item.Field<int?>("XaID"), 0);
                DTInfo.NgayVietDon = Utils.ConvertToDateTime(item.Field<DateTime?>("NgayVietDon"), DateTime.MinValue);

                DTInfo.SoLan = Utils.ConvertToInt32(item.Field<int?>("SoLan"), 1);

                DTInfo.XuLyDonID = Utils.ConvertToInt32(item.Field<int?>("XuLyDonID"), 0);
                //DTInfo.TrangThaiDonID = Utils.ConvertToInt32(item.Field<int?>("TrangThaiDonID"), 0);
                DTInfo.LyDo = Utils.GetString(item.Field<string>("LyDo"), String.Empty);

                DTInfo.TenLoaiKhieuTo1 = Utils.ConvertToString(item.Field<string>("TenLoaiKhieuTo1"), String.Empty);
                DTInfo.TenLoaiKhieuTo2 = Utils.ConvertToString(item.Field<string>("TenLoaiKhieuTo2"), String.Empty);
                DTInfo.TenLoaiKhieuTo3 = Utils.ConvertToString(item.Field<string>("TenLoaiKhieuTo3"), String.Empty);

                DTInfo.NoiDungHuongDan = Utils.ConvertToString(item.Field<string>("NoiDungHuongDan"), string.Empty);
                DTInfo.TenHuongGiaiQuyet = Utils.ConvertToString(item.Field<string>("TenHuongGiaiQuyet"), string.Empty);
                //DTInfo.NguonDonDen = Utils.ConvertToInt32(item.Field<int?>("NguonDonDen"), 0);    
                DTInfo.TenCQChuyenDonDen = Utils.ConvertToString(item.Field<string>("TenCQChuyenDonDen"), string.Empty);
                DTInfo.TenCanBoTiepNhan = Utils.GetString(item.Field<string>("TenCanBoTiepNhan"), string.Empty);
                DTInfo.TenCoQuanTiepNhan = Utils.GetString(item.Field<string>("TenCoQuanTiepNhan"), string.Empty);
                DTInfo.TenPhongBanTiepNhan = Utils.GetString(item.Field<string>("TenPhongBanTiepNhan"), string.Empty);
                DTInfo.TenCoQuanBanHanh = Utils.GetString(item.Field<string>("TenCoQuanBanHanh"), string.Empty);
                DTInfo.CoQuanChuyenDonDi = Utils.GetString(item.Field<string>("CoQuanChuyenDonDi"), string.Empty);
                DTInfo.NgayChuyenDonSangCQKhac = Utils.ConvertToDateTime(item.Field<DateTime?>("NgayChuyenDonDi"), DateTime.MinValue);
                DTInfo.CQDaGiaiQuyetID = Utils.GetString(item.Field<string>("CQDaGiaiQuyetID"), string.Empty);
                var TenCanBoXl = Utils.GetString(item.Field<string>("TenCanBoXuLy"), string.Empty);
                if (TenCanBoXl != "")
                {
                    DTInfo.TenCanBoXuLy = Utils.GetString(item.Field<string>("TenCanBoXuLy"), string.Empty);
                }
                else
                {
                    DTInfo.TenCanBoXuLy = Utils.GetString(item.Field<string>("TenCanBoPhanXL"), string.Empty);
                }


                DTInfo.TenPhongBanXuLy = Utils.GetString(item.Field<string>("TenPhongBanXuLy"), string.Empty);
                //DTInfo.NgayQuaHanGQ = Utils.ConvertToDateTime(dr["HanGiaiQuyet"], DateTime.MinValue);
                DTInfo.StateID = Utils.ConvertToInt32(item.Field<int>("StateID"), 0);
                if (DTInfo.StateID == (int)EnumState.LDPhanGQ)
                {
                    DTInfo.NgayQuaHanGQ = Utils.ConvertToDateTime(item.Field<DateTime?>("HanGiaiQuyet"), DateTime.MinValue);
                }
                else
                {
                    DTInfo.NgayQuaHanGQ = Utils.ConvertToDateTime(item.Field<DateTime?>("HanGiaiQuyetDueDate"), DateTime.MinValue);

                }

                DateTime NgayBanHanh = Utils.ConvertToDateTime(item.Field<DateTime?>("NgayBanHanh"), DateTime.MinValue);
                if (NgayBanHanh != DateTime.MinValue)
                    DTInfo.NgayBanHanh = Format.FormatDate(NgayBanHanh);

                DTInfo.NgayNhapDon = Utils.ConvertToDateTime(item.Field<DateTime?>("NgayNhapDon"), DateTime.MinValue);
                if (DTInfo.NgayNhapDon != DateTime.MinValue)
                    DTInfo.NgayNhapDonStr = Format.FormatDate(DTInfo.NgayNhapDon);

                DTInfo.TenLoaiKetQua = Utils.GetString(item.Field<string>("TenLoaiKetQua"), string.Empty);
                DTInfo.NgayXuLy = Utils.ConvertToDateTime(item.Field<DateTime?>("NgayXuLy"), DateTime.MinValue);
                if (DTInfo.NgayXuLy != DateTime.MinValue)
                    DTInfo.NgayXuLyStr = Format.FormatDate(DTInfo.NgayXuLy);
                DTInfo.TenCoQuanGQ = Utils.GetString(item.Field<string>("TenCoQuanGQ"), string.Empty);
                DTInfo.TenCoQuanXL = Utils.GetString(item.Field<string>("TenCoQuanXuLy"), string.Empty);
                DTInfo.NgayChuyenDon = Utils.ConvertToDateTime(item.Field<DateTime?>("NgayPhanCong"), DateTime.MinValue);
                DTInfo.HanXuLy = Utils.ConvertToDateTime(item.Field<DateTime?>("HanXuLy"), DateTime.MinValue);
                DTInfo.SoDonThu = Utils.GetString(item.Field<string>("SoDonThu"), string.Empty);
                DTInfo.NgayPhan = Utils.ConvertToDateTime(item.Field<DateTime?>("NgayPhan"), DateTime.MinValue);
                DTInfo.SoCongVan = Utils.GetString(item.Field<string>("SoCongVan"), string.Empty);
                DTInfo.SoCVBaoCaoGQ = Utils.GetString(item.Field<string>("SoCVBaoCaoGQ"), string.Empty);
                DTInfo.NgayCVBaoCaoGQ = Utils.ConvertToDateTime(item.Field<DateTime?>("NgayCVBaoCaoGQ"), DateTime.MinValue);
                DTInfo.TenCQPhan = Utils.GetString(item.Field<string>("TenCQPhan"), string.Empty);
                DTInfo.LanGiaiQuyet = Utils.ConvertToInt32(item.Field<int?>("LanGiaiQuyet"), 0);

                if (DTInfo.StateID > (int)EnumState.LDDuyetXL)
                {
                    DTInfo.TrangThaiDonID = 2;// da xu ly
                }
                else DTInfo.TrangThaiDonID = 1;// chua xu ly

                if (DTInfo.StateID == (int)EnumState.KetThuc)
                {
                    DTInfo.TrangThaiDonID = 5;// da giai quyet
                }
                else if (DTInfo.StateID == (int)EnumState.LDDuyetDQ || DTInfo.StateID == 21 || DTInfo.StateID == 22 || DTInfo.StateID == 25)
                {
                    DTInfo.TrangThaiDonID = 4;// dang giai quyet
                }
                else if (DTInfo.StateID == 7 || DTInfo.StateID == 18 || DTInfo.StateID == 19)
                {
                    DTInfo.TrangThaiDonID = 3;// chua giai quyet
                }
                else if (DTInfo.StateID == 8)
                {
                    DTInfo.NgayCapNhat = Utils.ConvertToDateTime(item.Field<DateTime?>("NgayCapNhat"), DateTime.MinValue);
                    if (DTInfo.NgayCapNhat == DateTime.MinValue)
                    {
                        DTInfo.TrangThaiDonID = 3;// chua giai quyet
                    }
                    else DTInfo.TrangThaiDonID = 4;// dang giai quyet
                }

                DTInfo.lsFileYKienXuLy = new List<XuLyDonInfo>();
                List<FileHoSoInfo> fileYKienXL = new List<FileHoSoInfo>();
                fileYKienXL = fileYKienXLAll.Where(x => x.XuLyDonID == item.Field<int>("XuLyDonID")).ToList();
                for (int i = 0; i < fileYKienXL.Count; i++)
                {
                    if (!string.IsNullOrEmpty(fileYKienXL[i].FileURL))
                    {
                        if (string.IsNullOrEmpty(fileYKienXL[i].TenFile))
                        {
                            string[] arrtenFile = fileYKienXL[i].FileURL.Split('/');
                            if (arrtenFile.Length > 0)
                            {
                                string[] duoiFile = arrtenFile[arrtenFile.Length - 1].Split('_');
                                if (duoiFile.Length > 0)
                                {
                                    fileYKienXL[i].TenFile = duoiFile[duoiFile.Length - 1];
                                }
                                else
                                {
                                    fileYKienXL[i].TenFile = arrtenFile[arrtenFile.Length - 1];
                                }
                            }
                        }
                        fileYKienXL[i].FileURL = fileYKienXL[i].FileURL.Replace(" ", "%20");
                    }

                    XuLyDonInfo file = new XuLyDonInfo();
                    file.TenFile = fileYKienXL[i].TenFile;
                    file.FileUrl = fileYKienXL[i].FileURL;
                    DTInfo.lsFileYKienXuLy.Add(file);
                }
                // kết quả
                List<FileHoSoInfo> fileBanHanhQD = new List<FileHoSoInfo>();
                fileBanHanhQD = fileBanHanhQDAll.Where(x => x.XuLyDonID == DTInfo.XuLyDonID).ToList();
                for (int j = 0; j < fileBanHanhQD.Count; j++)
                {
                    if (!string.IsNullOrEmpty(fileBanHanhQD[j].FileURL))
                    {
                        if (string.IsNullOrEmpty(fileBanHanhQD[j].TenFile))
                        {
                            string[] arrtenFile = fileBanHanhQD[j].FileURL.Split('/');
                            if (arrtenFile.Length > 0)
                            {
                                string[] duoiFile = arrtenFile[arrtenFile.Length - 1].Split('_');
                                if (duoiFile.Length > 0)
                                {
                                    fileBanHanhQD[j].TenFile = duoiFile[duoiFile.Length - 1];
                                }
                                else
                                {
                                    fileBanHanhQD[j].TenFile = arrtenFile[arrtenFile.Length - 1];
                                }
                            }
                        }
                        fileBanHanhQD[j].FileURL = fileBanHanhQD[j].FileURL.Replace(" ", "%20");
                    }
                }
                DTInfo.lsFileQuyetDinhGD = fileBanHanhQD;

                DonThus.Add(DTInfo);
            }
            #endregion

            return DonThus;
        }

        public int ChuTichGiaoXuLy_UpdateXuLyDon(int XuLyDonID, int CoQuanID, DateTime? HanXuLy, int CQChuyenTiep)
        {
            object val = null;
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@XuLyDonID", SqlDbType.Int),
                new SqlParameter("@HanXL", SqlDbType.DateTime),
                new SqlParameter("@CoQuanID", SqlDbType.Int),
                new SqlParameter("@CQChuyenTiep", SqlDbType.Int),
            };
            parms[0].Value = XuLyDonID;
            parms[1].Value = HanXuLy ?? Convert.DBNull;
            parms[2].Value = CoQuanID;
            parms[3].Value = CQChuyenTiep;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, "ChuTichGiaoXuLy_UpdateXuLyDon", parms);
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

        public int UpdateLanhDaoDuyet(int? XuLyDonID, int? LanhDaoDuyet1ID, int? LanhDaoDuyet2ID)
        {

            object val = null;

            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("XuLyDonID", SqlDbType.Int),
                new SqlParameter("LanhDaoDuyet1ID", SqlDbType.Int),
                new SqlParameter("LanhDaoDuyet2ID", SqlDbType.Int),
            };
            parameters[0].Value = XuLyDonID ?? Convert.DBNull;
            parameters[1].Value = LanhDaoDuyet1ID ?? Convert.DBNull;
            parameters[2].Value = LanhDaoDuyet2ID ?? Convert.DBNull;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "v2_NV_XuLyDon_UpdateLanhDaoDuyet", parameters);
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

        public int UpdateTrangThaiDuyet(int? XuLyDonID, int? TrangThaiDuyet)
        {

            object val = null;

            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("XuLyDonID", SqlDbType.Int),
                new SqlParameter("TrangThaiDuyet", SqlDbType.Int),
            };
            parameters[0].Value = XuLyDonID ?? Convert.DBNull;
            parameters[1].Value = TrangThaiDuyet ?? Convert.DBNull;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "v2_NV_XuLyDon_UpdateTrangThaiDuyet", parameters);
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


        public int UpdateCoQuanChuyenNgoaiHeThong(int XuLyDonID, string CoQuanNgoaiHeThong, string CoQuanTheoDoiDonDoc)
        {
            object val = null;
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@XuLyDonID", SqlDbType.Int),
                new SqlParameter("@CoQuanNgoaiHeThong", SqlDbType.NVarChar),
                new SqlParameter("@CoQuanTheoDoiDonDoc", SqlDbType.NVarChar)
            };
            parms[0].Value = XuLyDonID;
            parms[1].Value = CoQuanNgoaiHeThong ?? Convert.DBNull;
            parms[2].Value = CoQuanTheoDoiDonDoc ?? Convert.DBNull;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, "UpdateCoQuanChuyenNgoaiHeThong", parms);
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

        public int UpdateTrangThaiKhoa(int? XuLyDonID, bool? TrangThai)
        {

            object val = null;

            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("XuLyDonID", SqlDbType.Int),
                new SqlParameter("TrangThaiKhoa", SqlDbType.Int),
            };
            parameters[0].Value = XuLyDonID ?? Convert.DBNull;
            parameters[1].Value = TrangThai ?? Convert.DBNull;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "v2_NV_XuLyDon_UpdateTrangThaiKhoa", parameters);
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

        public int UpdateDocument(int? DocumentID, int? StateID, DateTime? DueDate)
        {

            object val = null;

            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("DocumentID", SqlDbType.Int),
                new SqlParameter("StateID", SqlDbType.Int),
                new SqlParameter("DueDate", SqlDbType.DateTime),
            };
            parameters[0].Value = DocumentID ?? Convert.DBNull;
            parameters[1].Value = StateID ?? Convert.DBNull;
            parameters[2].Value = DueDate ?? Convert.DBNull;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "v2_NV_XuLyDon_UpdateDocument", parameters);
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

        public XuLyDonInfo GetByXuLyDonID_V2(int xulydonID)
        {
            XuLyDonInfo DTInfo = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDON_ID,SqlDbType.Int)
            };
            parameters[0].Value = xulydonID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_NV_XuLyDon_GetByXuLyDonID", parameters))
                {

                    if (dr.Read())
                    {
                        return MapXuLyDonFromReader(dr);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {

            }
            return DTInfo;
        }

        // Phương thức map dữ liệu từ SqlDataReader vào đối tượng XulyDon
        private XuLyDonInfo MapXuLyDonFromReader(SqlDataReader reader)
        {
            XuLyDonInfo xulyDon = new XuLyDonInfo();
            // Map từng trường dữ liệu vào đối tượng XulyDon tương ứng
            xulyDon.XuLyDonID = Utils.ConvertToInt32(reader["XuLyDonID"], 0);
            xulyDon.DonThuID = Utils.ConvertToInt32(reader["DonThuID"], 0);
            xulyDon.SoLan = Utils.ConvertToInt32(reader["SoLan"], 0);
            xulyDon.LanGiaiQuyet = Utils.ConvertToInt32(reader["LanGiaiQuyet"], 0);
            xulyDon.SoDonThu = Utils.GetString(reader["SoDonThu"], string.Empty);
            xulyDon.NgayNhapDon = Utils.ConvertToDateTime(reader["NgayNhapDon"], DateTime.MinValue); 
            xulyDon.NgayQuaHan = Utils.ConvertToDateTime(reader["NgayQuaHan"], DateTime.MinValue);
            xulyDon.NguonDonDenID = Utils.ConvertToInt32(reader["NguonDonDen"], 0);
            xulyDon.CQChuyenDonDenID = Utils.ConvertToInt32(reader["CQChuyenDonDenID"], 0);
            xulyDon.SoCongVan = Utils.GetString(reader["SoCongVan"], string.Empty);
            xulyDon.NgayCQKhacChuyenDonDen = Utils.ConvertToDateTime(reader["NgayCQKhacChuyenDonDen"], DateTime.MinValue);
            xulyDon.NgayChuyenDon = Utils.ConvertToDateTime(reader["NgayChuyenDon"], DateTime.MinValue);
            xulyDon.ThuocThamQuyen = Utils.ConvertToBoolean(reader["ThuocThamQuyen"], false);
            xulyDon.DuDieuKien = Utils.ConvertToBoolean(reader["DuDieuKien"], false);
            xulyDon.HuongGiaiQuyetID = Utils.ConvertToInt32(reader["HuongGiaiQuyetID"], 0);
            xulyDon.NoiDungHuongDan = Utils.GetString(reader["NoiDungHuongDan"], string.Empty);
            xulyDon.CQChuyenDonID = Utils.ConvertToInt32(reader["CQChuyenDonID"], 0);
            xulyDon.CanBoXuLyID = Utils.ConvertToInt32(reader["CanBoXuLyID"], 0);
            xulyDon.CanBoKyID = Utils.ConvertToInt32(reader["CanBoKyID"], 0);
            xulyDon.CQDaGiaiQuyetID = Utils.GetString(reader["CQDaGiaiQuyetID"], string.Empty);
            xulyDon.CQGiaiQuyetTiepID = Utils.ConvertToInt32(reader["CQGiaiQuyetTiepID"], 0);
            xulyDon.TrangThaiDonID = Utils.ConvertToInt32(reader["TrangThaiDonID"], 0);
            xulyDon.PhanTichKQID = Utils.ConvertToInt32(reader["PhanTichKQID"], 0);
            xulyDon.CanBoTiepNhapID = Utils.ConvertToInt32(reader["CanBoTiepNhapID"], 0);
            xulyDon.CoQuanID = Utils.ConvertToInt32(reader["CoQuanID"], 0);
            xulyDon.NgayThuLy = Utils.ConvertToDateTime(reader["NgayThuLy"], DateTime.MinValue);
            xulyDon.DuAnID = Utils.ConvertToInt32(reader["DuAnID"], 0);
            xulyDon.NgayXuLy = Utils.ConvertToDateTime(reader["NgayXuLy"], DateTime.MinValue);
            xulyDon.MaHoSoMotCua = Utils.GetString(reader["MaHoSoMotCua"], string.Empty);
            xulyDon.SoBienNhanMotCua = Utils.GetString(reader["SoBienNhanMotCua"], string.Empty);
            xulyDon.NgayHenTraMotCua = Utils.ConvertToDateTime(reader["NgayHenTraMotCua"], DateTime.MinValue);
            xulyDon.SoCVBaoCaoGQ = Utils.GetString(reader["SoCVBaoCaoGQ"], string.Empty);
            xulyDon.NgayCVBaoCaoGQ = Utils.ConvertToDateTime(reader["NgayCVBaoCaoGQ"], DateTime.MinValue);
            xulyDon.CQNgoaiHeThong = Utils.GetString(reader["CQNgoaiHeThong"], string.Empty);
            xulyDon.CBDuocChonXL = Utils.ConvertToInt32(reader["CBDuocChonXL"], 0);
            xulyDon.QTTiepNhanDon = Utils.ConvertToInt32(reader["QTTiepNhanDon"], 0);
            xulyDon.DonThuGocID = Utils.ConvertToInt32(reader["DonThuGocID"], 0);
            xulyDon.ViewerVBDonDoc = Utils.ConvertToBoolean(reader["ViewerVBDonDoc"], false);
            xulyDon.XuLyDonChuyenID = Utils.ConvertToInt32(reader["DonThuGocID"], 0);
            xulyDon.TrinhDuThao = Utils.ConvertToInt32(reader["TrinhDuThao"], 0);
            xulyDon.NoiDungBanHanhXM = Utils.GetString(reader["NoiDungBanHanhXM"], string.Empty);
            xulyDon.NoiDungBanHanhGQ = Utils.GetString(reader["NoiDungBanHanhGQ"], string.Empty);
            xulyDon.HanXL = reader["HanXL"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["HanXL"]);
            xulyDon.CQChuyenTiep = Utils.ConvertToInt32(reader["CQChuyenTiep"], 0);
            xulyDon.CanBoBanHanh = Utils.ConvertToInt32(reader["CanBoBanHanh"], 0);
            xulyDon.NgayBanHanh = reader["NgayBanHanh"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["NgayBanHanh"]);
            xulyDon.LanhDaoDuyet1ID = Utils.ConvertToInt32(reader["LanhDaoDuyet1ID"], 0);
            xulyDon.LanhDaoDuyet2ID = Utils.ConvertToInt32(reader["LanhDaoDuyet2ID"], 0);
            xulyDon.QuyTrinhXLD = Utils.GetString(reader["QuyTrinhXLD"], string.Empty);
            xulyDon.QuyTrinhGQ = Utils.GetString(reader["QuyTrinhGQ"], string.Empty);
            xulyDon.CoQuanNgoaiHeThong = Utils.ConvertToBoolean(reader["CoQuanNgoaiHeThong"], false);
            xulyDon.CoQuanTheoDoiDonDoc = Utils.ConvertToBoolean(reader["CoQuanTheoDoiDonDoc"], false);
            xulyDon.TrangThaiKhoa = Utils.ConvertToBoolean(reader["TrangThaiKhoa"], false);
            xulyDon.DuocTraCuu = Utils.ConvertToBoolean(reader["DuocTraCuu"], false);
            xulyDon.TrangThaiDuyet = Utils.ConvertToBoolean(reader["TrangThaiDuyet"], false);

            return xulyDon;
        }
// bổ sung     
        public BaseResultModel ChuyenDon_Sang_TiepDanThuongXuyen(IdentityHelper IdentityHelper, string xuLyDonIDIds, string donThuIDIds)
        {
            var result = new BaseResultModel();
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@XuLyDonIDIds", SqlDbType.NVarChar),
                    new SqlParameter("@DonThuIDIds", SqlDbType.NVarChar),
                    new SqlParameter("@CanBoID", SqlDbType.Int),
                };
                parameters[0].Value = xuLyDonIDIds;
                parameters[1].Value = donThuIDIds;
                parameters[2].Value = IdentityHelper.CanBoID;

                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {

                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {

                        try
                        {
                            var val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, "v2_ChuyenDon_Sang_TiepDanThuongXuyen", parameters);
                            trans.Commit();
                            result.Status = 1;
                            result.Message = "Chuyển thành công";
                        }
                        catch
                        {
                            trans.Rollback();
                            throw;
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                throw;
            }
            return result;
        }
        public BaseResultModel ChuyenDon_Sang_TiepNhanDon(string xuLyDonIDIds, string donThuIDIds)
        {
            var result = new BaseResultModel();
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@XuLyDonIDIds", SqlDbType.NVarChar),
                    new SqlParameter("@DonThuIDIds", SqlDbType.NVarChar),
                };
                parameters[0].Value = xuLyDonIDIds;
                parameters[1].Value = donThuIDIds;

                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {

                        try
                        {
                            var val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, "v2_ChuyenDon_Sang_TiepNhanDon", parameters);
                            trans.Commit();
                            result.Status = 1;
                            result.Message = "Chuyển thành công";
                        }
                        catch
                        {
                            trans.Rollback();
                            throw;
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                throw;
            }
            return result;
        }
    }
}
