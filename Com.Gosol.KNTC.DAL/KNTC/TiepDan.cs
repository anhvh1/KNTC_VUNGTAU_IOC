using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Ultilities;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Vml;
using Microsoft.Office.Interop.Word;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.DAL.KNTC
{
    public class TiepDan
    {
        //Su dung de goi StoreProcedure
        private const string UPDATE_XULYDON = @"NVTiepDan_UpdateXuLyDon";
        private const string INSERT_XULYDON = @"NVTiepDan_InsertXuLyDon";
        private const string UPDATE_DONTHU = @"NVTiepDan_UpdateDonThu";
        private const string INSERT_DONTHU = @"NVTiepDan_InsertDonThu";
        private const string UPDATE_TIEPDAN = @"NVTiepDan_UpdateTiepDan";
        private const string UPDATE_TIEPDAN_NEW = @"NVTiepDan_UpdateTiepDan_New_v2";
        private const string INSERT_TIEPDAN = @"NVTiepDan_InsertTiepDan";
        private const string INSERT_TIEPDAN_NEW = @"NVTiepDan_InsertTiepDan_New_v2";

        private const string INSERT_THANHPHANTHAMGIA = @"ThanhPhanThamGia_Insert";
        private const string DELETE_THANHPHANTHAMGIA = @"ThanhPhanThamGia_Delete";
        private const string GET_THANHPHANTHAMGIA_BY_TIEPDANKHONGDONID = @"ThanhPhanThamGia_GetByTiepDanKhongDonID";
        private const string SODONTHU_GETBYTIEPDANID = @"SoDonThu_GetByTiepDanID";

        private const string GET_XULYDON_BY_ID = @"NVTiepDan_GetXuLyDonID";
        private const string GET_ALLXULYDON_BY_ID = @"NVTiepDan_GetAllXuLyDonByID";
        private const string GET_TIEPDAN_BY_TIEPDANID = @"NVTiepDan_GetTiepDanByTiepDanID";
        private const string GET_MATIEPDAN = @"TiepDanKhongDon_GetMaTiepDan";


        private const string GET_ALL = @"DonThu_GetAll";
        private const string GET_BY_ID = @"NV_XuLyDon_GetByID";
        private const string GET_JOINTIEPDAN_BY_ID = @"NV_XuLyDon_GetJoinTiepDanByID";
        private const string GET_BY_DONTHU_ID = @"NV_XuLyDon_GetByDonID";
        private const string INSERT = @"NV_DonThu_Insert";
        private const string INSERT_STEP1 = @"NV_XuLyDon_InsertStep1";
        private const string UPDATE = @"DonThu_Update";
        private const string UPDATE_STEP1 = @"NV_XuLyDon_UpdateStep1";
        private const string UPDATE_STEP2 = @"NV_XuLyDon_UpdateStep2";
        private const string UPDATE_STEP3 = @"NV_XuLyDon_UpdateStep3";
        private const string UPDATE_STEP4 = @"NV_XuLyDon_UpdateStep4";
        private const string DELETE = @"DonThu_Delete";

        private const string UPDATE_TRANGTHAI = "XuLyDon_UpdateTrangThai";
        //private const string UPDATE_XULYDONID_CLONE = "NV_XuLyDon_UpdateXuLyDonID_Clone";

        //private const string GET_BY_PAGE = @"DM_DonThu_GetByPage";
        //private const string COUNT_ALL = @"DM_DonThu_CountAll";
        //private const string COUNT_SEARCH = @"DM_DonThu_CountSearch";
        //private const string SEARCH = @"DM_DonThu_GetBySearch";

        private const string GET_BY_PAGE = @"NV_DonThu_GetByPage";
        private const string COUNT_ALL = @"NV_DonThu_CountAll";
        private const string GET_XULYLAN1 = "NV_XuLyDon_GetXuLyLan1";


        private const string GET_SO_DON_BY_CO_QUAN = @"NV_XuLyDon_GetSoDonByCoQuan";
        private const string NV_XULYDON_GETSODONBYCOQUAN_NEW = @"NV_XuLyDon_GetSoDonByCoQuan_New";

        private const string GET_STT_TIEPDAN_BY_CO_QUAN = @"NV_TiepDan_GetSoDonByCoQuan";
        private const string GET_SO_TT_LETAN_BY_CO_QUAN = @"NV_TiepDan_GetSoTTLeTanByCoQuan";

        private const string COUND_TRUNGDON_BY_DONTHUID = @"NV_TiepDan_CountTrungDon";

        private const string GET_CTTRUNGDON_BY_DONTHU_ID = @"NV_TiepDan_GetCTTrungDonByDonID";
        private const string GET_CTKHIEUTOLAN2_BY_DONTHU_ID = @"NV_TiepDan_GetCTTrungDonByDonID_DonThuKTLan2";
        private const string CHECK_KHIEUTOLAN2_BY_HOTEN = @"NV_TiepDan_CheckKhieuToLan2_ByHoTen";
        private const string CHECK_KHIEUTOLAN2_SOLANGQ = @"NV_TiepDan_CheckKhieuToLan2_SoLanGQ";

        //Ten cac bien dau vao
        // param don thu
        private const string PARAM_DON_THU_ID = "@DonThuID";
        private const string PARAM_NHOM_KN_ID = "@NhomKNID";
        private const string PARAM_DOI_TUONG_BI_KN_ID = "@DoiTuongBiKNID";
        private const string PARAM_LOAI_KHIEU_TO_1ID = "@LoaiKhieuTo1ID";
        private const string PARAM_LOAI_KHIEU_TO_2ID = "@LoaiKhieuTo2ID";
        private const string PARAM_LOAI_KHIEU_TO_3ID = "@LoaiKhieuTo3ID";
        private const string PARAM_TENLANHDAOTIEP = @"TenLanhDaoTiep";
        private const string PARAM_LOAI_KHIEU_TO_ID = "@LoaiKhieuToID";
        private const string PARAM_NOI_DUNG_DON = "@NoiDungDon";
        private const string PARAM_TRUNG_DON = "@TrungDon";
        private const string PARAM_TINH_ID = "@TinhID";
        private const string PARAM_HUYEN_ID = "@HuyenID";
        private const string PARAM_XA_ID = "@XaID";
        private const string PARAM_LETANCHUYEN = "@LeTanChuyen";
        private const string PARAM_NGAYVIETDON = "@NgayVietDon";
        private const string PARAM_STATEID = "@StateID";


        //param xu ly don
        private const string PARAM_XULYDON_ID = "@XuLyDonID";
        private const string PARAM_SO_LAN = "@SoLan";
        private const string PARAM_SO_DON_THU = "@SoDonThu";
        private const string PARAM_NGAY_NHAP_DON = "@NgayNhapDon";
        private const string PARAM_NGAY_QUA_HAN = "@NgayQuaHan";
        private const string PARAM_NGUON_DON_DEN_ID = "@NguonDonDen";
        private const string PARAM_CQ_CHUYEN_DON_ID = "@CQChuyenDonID";
        private const string PARAM_SO_CONG_VAN = "@SoCongVan";
        private const string PARAM_NGAY_CHUYEN_DON = "@NgayChuyenDon";
        private const string PARAM_THUOCTHAMQUYEN = "@ThuocThamQuyen";
        private const string PARAM_DUDIEUKIEN = "@DuDieuKien";
        private const string PARAM_HUONG_GIAI_QUYET_ID = "@HuongGiaiQuyetID";
        private const string PARAM_NOI_DUNG_HUONG_DAN = "@NoiDungHuongDan";
        private const string PARAM_CAN_BO_XU_LY_ID = "@CanBoXuLyID";
        private const string PARAM_CAN_BO_KY_ID = "@CanBoKyID";
        private const string PARAM_CQ_DA_GIAI_QUYET_ID = "@CQDaGiaiQuyetID";
        private const string PARAM_TRANG_THAI_DON_ID = "@TrangThaiDonID";
        private const string PARAM_PT_KET_QUA_ID = "@PhanTichKQID";
        private const string PARAM_CANBO_TIEPNHAP_ID = "@CanBoTiepNhapID";
        private const string PARAM_CO_QUAN_ID = "@CoQuanID";
        private const string PARAM_NGAYTHULY = "@NgayThuLy";
        private const string PARAM_LYDO = "@LyDo";
        private const string PARAM_DUAN_ID = "@DuAnID";
        private const string PARAM_NGAYXULY = "@NgayXuLy";
        private const string PARAM_DADUYET_XULY = "@DaDuyetXuLy";
        private const string PARAM_XULYDONID_CLONE = "@XuLyDonIDClone";
        private const string PARAM_CQ_CHUYEN_DONDEN_ID = "@CQChuyenDonDenID";
        private const string PARAM_CBDUOC_CHONXL = "@CBDuocChonXL";
        private const string PARAM_QTTIEPNHANDON = "@QTTiepNhanDon";
        private const string PARAM_DONTHUGOC_ID = "@DonThuGocID";
        private const string PARAM_LANGIAIQUYET = "@LanGiaiQuyet";

        //param tiep dan khong don
        private const string PARAM_TIEP_DAN_KHONG_DON_ID = "@TiepDanKhongDonID";
        private const string PARAM_NGAY_TIEP = "@NgayTiep";
        private const string PARAM_GAP_LANH_DAO = "@GapLanhDao";
        private const string PARAM_NGAY_GAP_LANH_DAO = "@NgayGapLanhDao";
        private const string PARAM_LANHDAO_DANGKY = "@LanhDaoDangKy";
        private const string PARAM_NOI_DUNG_TIEP = "@NoiDungTiep";
        private const string PARAM_VU_VIEC_CU = "@VuViecCu";
        private const string PARAM_CAN_BO_TIEP_ID = "@CanBoTiepID";
        private const string PARAM_KQ_TIEPDAN = "@KQQuaTiepDan";
        //private const string PARAM_DON_THU_ID = "@DonThuID";
        //private const string PARAM_CO_QUAN_ID = "@CoQuanID";
        //private const string PARAM_XU_LY_DON_ID = "@XuLyDonID";
        private const string PARAM_LAN_TIEP = "@LanTiep";
        private const string PARAM_KET_QUA_TIEP = "@KetQuaTiep";
        private const string PARAM_SO_DON = "@SoDon";
        private const string PARAM_HUONGGIAIQUYETID = "@HuongGiaiQuyetID";
        private const string PARAM_YEU_CAU_NGUOI_DUOC_TIEP = "@YeuCauNguoiDuocTiep";
        private const string PARAM_THONG_TIN_TAI_LIEU = "@ThongTinTaiLieu";
        private const string PARAM_KET_LUAN_NGUOI_CHU_TRI = "@KetLuanNguoiChuTri";
        private const string PARAM_NGUOI_DUOC_TIEP_PHAT_BIEU = "@NguoiDuocTiepPhatBieu";
        private const string PARAM_TIEP_DAN_CO_DON = "@TiepDanCoDon";
        private const string PARAM_NHOM_THAM_QUYEN_GIAI_QUYET_ID = "@NhomThamQuyenGiaiQuyetID";
        private const string PARAM_NGAY_BAN_HANH_QUYET_DINH_GIAI_QUYET = "@NgayBanHanhQuyetDinhGiaiQuyet";
        private const string PARAM_LOAI_TIEP_DAN_ID = "@LoaiTiepDanID";
        private const string PARAM_KET_QUA_GIAI_QUYET = "@KetQuaGiaiQuyet";
        private const string PARAM_THANH_PHAN_THAM_GIA_TIEP = "@ThanhPhanThamGiaTiep";
        private const string PARAM_UY_QUYEN_TIEP = "@UyQuyenTiep";

        //thanh phan tham gia
        private const string PARAM_TEN_CAN_BO = "@TenCanBo";
        private const string PARAM_CHUC_VU = "@ChucVu";
        //trinm
        private const string PARAM_CMND = "@CMND";
        private const string PARAM_HOTEN = "@HoTen";
        private const string PARAM_DIACHI = "@DiaChi";
        private const string PARAM_START = "@Start";
        private const string PARAM_END = "@End";

        private XuLyDonInfo GetCustomData(SqlDataReader rdr)
        {
            XuLyDonInfo info = new XuLyDonInfo();
            info.XuLyDonID = Utils.GetInt32(rdr["XuLyDonID"], 0);
            info.DonThuID = Utils.GetInt32(rdr["DonThuID"], 0);
            info.TrangThaiDonID = Utils.GetInt32(rdr["TrangThaiDonID"], 0);
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
                    info.NguonDonDenID = Utils.GetInt32(dr["NguonDonDenID"], 0);
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
                    break;
                case "step4":
                    info.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);
                    info.HuongGiaiQuyetID = Utils.GetInt32(dr["HuongGiaiQuyetID"], 0);
                    info.NoiDungHuongDan = Utils.GetString(dr["NoiDungHuongDan"], string.Empty);
                    info.CanBoXuLyID = Utils.GetInt32(dr["CanBoXuLyID"], 0);
                    info.CQTiepNhanID = Utils.GetInt32(dr["CQTiepNhanID"], 0);
                    info.CanBoKyID = Utils.GetInt32(dr["CanBoKyID"], 0);
                    info.CQGiaiQuyetTiepID = Utils.GetInt32(dr["CQGiaiQuyetTiepID"], 0);
                    info.ThuocThamQuyen = Utils.GetBoolean(dr["ThuocThamQuyen"], false);
                    info.DuDieuKien = Utils.GetBoolean(dr["DuDieuKien"], false);
                    info.CanBoTiepNhapID = Utils.GetInt32(dr["CanBoTiepNhapID"], 0);

                    //
                    info.CoQuanID = Utils.GetInt32(dr["CoQuanID"], 0);

                    break;
                default:
                    info.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);
                    info.DonThuID = Utils.GetInt32(dr["DonThuID"], 0);
                    info.SoDonThu = Utils.GetString(dr["SoDonThu"], string.Empty);
                    info.NgayNhapDon = Utils.GetDateTime(dr["NgayNhapDon"], Constant.DEFAULT_DATE);
                    info.NgayQuaHan = Utils.GetDateTime(dr["NgayQuaHan"], Constant.DEFAULT_DATE);
                    info.NguonDonDenID = Utils.GetInt32(dr["NguonDonDenID"], 0);
                    info.CQChuyenDonID = Utils.GetInt32(dr["CQChuyenDonID"], 0);
                    info.SoCongVan = Utils.GetString(dr["SoCongVan"], string.Empty);
                    info.NgayChuyenDon = Utils.GetDateTime(dr["NgayChuyenDon"], Constant.DEFAULT_DATE);
                    info.CQDaGiaiQuyetID = Utils.GetString(dr["CQDaGiaiQuyetID"], string.Empty);
                    //info.GapLanhDao = Utils.GetBoolean(dr["GapLanhDao"], false);
                    //info.NgayGapLanhDao = Utils.GetDateTime(dr["NgayGapLanhDao"], Constant.DEFAULT_DATE);
                    info.HuongGiaiQuyetID = Utils.GetInt32(dr["HuongGiaiQuyetID"], 0);
                    info.NoiDungHuongDan = Utils.GetString(dr["NoiDungHuongDan"], string.Empty);
                    info.CanBoXuLyID = Utils.GetInt32(dr["CanBoXuLyID"], 0);
                    info.CQTiepNhanID = Utils.GetInt32(dr["CQTiepNhanID"], 0);
                    info.CanBoKyID = Utils.GetInt32(dr["CanBoKyID"], 0);
                    info.CQGiaiQuyetTiepID = Utils.GetInt32(dr["CQGiaiQuyetTiepID"], 0);
                    info.TrangThaiDonID = Utils.GetInt32(dr["TrangThaiDonID"], 0);
                    info.CanBoTiepNhapID = Utils.GetInt32(dr["CanBoTiepNhapID"], 0);
                    break;
            }

            return info;
        }

        private XuLyDonInfo GetDataForShow(SqlDataReader dr)
        {
            XuLyDonInfo DTInfo = new XuLyDonInfo();
            DTInfo.DonThuID = Utils.GetInt32(dr["DonThuID"], 0);

            DTInfo.NgayNhapDon = Utils.GetDateTime(dr["NgayNhapDon"], Constant.DEFAULT_DATE);

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
        private SqlParameter[] GetInsertParms(string step)
        {
            SqlParameter[] parms;

            if (step == "step1")
            {
                parms = new SqlParameter[]{
                    new SqlParameter(PARAM_DON_THU_ID, SqlDbType.Int),
                    new SqlParameter(PARAM_SO_DON_THU, SqlDbType.Int),
                    new SqlParameter(PARAM_NGAY_NHAP_DON, SqlDbType.DateTime),
                    new SqlParameter(PARAM_NGAY_QUA_HAN, SqlDbType.DateTime),
                    new SqlParameter(PARAM_NGUON_DON_DEN_ID, SqlDbType.Int),
                    new SqlParameter(PARAM_CQ_CHUYEN_DON_ID, SqlDbType.Int),
                    new SqlParameter(PARAM_SO_CONG_VAN, SqlDbType.NVarChar,20),
                    new SqlParameter(PARAM_NGAY_CHUYEN_DON, SqlDbType.DateTime),
                    new SqlParameter(PARAM_CO_QUAN_ID, SqlDbType.Int),
                    new SqlParameter(PARAM_CANBO_TIEPNHAP_ID, SqlDbType.Int)


                };
            }

            else
            {
                parms = new SqlParameter[] { };
            }
            return parms;
        }

        //SET iNSERT PARMS
        private void SetInsertParms(SqlParameter[] parms, XuLyDonInfo DTInfo, string step)
        {
            if (step == "step1")
            {
                parms[0].Value = DTInfo.DonThuID;
                parms[1].Value = DTInfo.SoDonThu;
                parms[2].Value = DTInfo.NgayNhapDon;
                parms[3].Value = DTInfo.NgayQuaHan;
                parms[4].Value = DTInfo.NguonDonDenID;
                parms[5].Value = DTInfo.CQChuyenDonID;
                parms[6].Value = DTInfo.SoCongVan;
                parms[7].Value = DTInfo.NgayChuyenDon;
                parms[8].Value = DTInfo.CoQuanID;
                parms[9].Value = DTInfo.CanBoTiepNhapID;
            }
            else
            {
                parms[0].Value = DTInfo.SoDonThu;
                parms[1].Value = DTInfo.NgayNhapDon;
                parms[2].Value = DTInfo.NgayQuaHan;
                parms[3].Value = DTInfo.NguonDonDenID;
                parms[4].Value = DTInfo.CQChuyenDonID;
                parms[5].Value = DTInfo.SoCongVan;
                parms[6].Value = DTInfo.NgayChuyenDon;
                parms[14].Value = DTInfo.CQDaGiaiQuyetID;
                parms[15].Value = DTInfo.HuongGiaiQuyetID;
                parms[16].Value = DTInfo.NoiDungHuongDan;
                parms[17].Value = DTInfo.CanBoXuLyID;
                parms[18].Value = DTInfo.CanBoKyID;
                parms[19].Value = DTInfo.CQGiaiQuyetTiepID;
                parms[20].Value = DTInfo.TrangThaiDonID;
                //parms[21].Value = DTInfo.KetQuaID;
                parms[22].Value = DTInfo.CoQuanID;
            }
        }

        //get update parms
        private SqlParameter[] GetUpdateParms(string step)
        {
            SqlParameter[] parms = new SqlParameter[] { };

            switch (step)
            {
                case "step1":
                    parms = new SqlParameter[]{
                        new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int),
                        new SqlParameter(PARAM_DON_THU_ID, SqlDbType.Int),
                        new SqlParameter(PARAM_SO_DON_THU, SqlDbType.Int),
                        new SqlParameter(PARAM_NGAY_NHAP_DON, SqlDbType.DateTime),
                        new SqlParameter(PARAM_NGAY_QUA_HAN, SqlDbType.DateTime),
                        new SqlParameter(PARAM_NGUON_DON_DEN_ID, SqlDbType.Int),
                        new SqlParameter(PARAM_CQ_CHUYEN_DON_ID, SqlDbType.Int),
                        new SqlParameter(PARAM_SO_CONG_VAN, SqlDbType.NVarChar,20),
                        new SqlParameter(PARAM_NGAY_CHUYEN_DON, SqlDbType.DateTime),
                        new SqlParameter(PARAM_CO_QUAN_ID, SqlDbType.Int)
                        //new SqlParameter(PARAM_CANBO_TIEPNHAN_ID, SqlDbType.Int)

                };
                    break;
                case "step2":
                    parms = new SqlParameter[]{
                        new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int),
                        new SqlParameter(PARAM_CQ_DA_GIAI_QUYET_ID, SqlDbType.Int),
                        new SqlParameter(PARAM_CQ_CHUYEN_DON_ID, SqlDbType.Int),
                        new SqlParameter(PARAM_DUAN_ID, SqlDbType.Int)
                        //new SqlParameter(PARAM_NGAY_GAP_LANH_DAO, SqlDbType.DateTime)
                    };
                    break;
                case "step4":
                    parms = new SqlParameter[]{
                        new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int),
                        new SqlParameter(PARAM_HUONG_GIAI_QUYET_ID, SqlDbType.Int),
                        new SqlParameter(PARAM_NOI_DUNG_HUONG_DAN, SqlDbType.NVarChar),
                        new SqlParameter(PARAM_CAN_BO_XU_LY_ID, SqlDbType.Int),
                        new SqlParameter(PARAM_CQ_CHUYEN_DON_ID, SqlDbType.Int),
                        new SqlParameter(PARAM_CAN_BO_KY_ID, SqlDbType.Int),
                        new SqlParameter(PARAM_CQ_CHUYEN_DON_ID, SqlDbType.Int),
                        new SqlParameter(PARAM_THUOCTHAMQUYEN, SqlDbType.Bit),
                        new SqlParameter(PARAM_DUDIEUKIEN, SqlDbType.Bit),
                        new SqlParameter(PARAM_TRANG_THAI_DON_ID, SqlDbType.Int)
                    };
                    break;
                default:
                    break;
            }

            return parms;
        }

        //set update parms
        private void SetUpdateParms(SqlParameter[] parms, XuLyDonInfo DTInfo, string step)
        {
            switch (step)
            {
                case "step1":
                    parms[0].Value = DTInfo.XuLyDonID;
                    parms[1].Value = DTInfo.DonThuID;
                    parms[2].Value = DTInfo.SoDonThu;
                    parms[3].Value = DTInfo.NgayNhapDon;
                    parms[4].Value = DTInfo.NgayQuaHan;
                    parms[5].Value = DTInfo.NguonDonDenID;
                    parms[6].Value = DTInfo.CQChuyenDonID;
                    parms[7].Value = DTInfo.SoCongVan;
                    parms[8].Value = DTInfo.NgayChuyenDon;
                    parms[9].Value = DTInfo.CoQuanID;
                    //parms[10].Value = DTInfo.CanBoTiepNhapID;
                    break;
                case "step2":
                    parms[0].Value = DTInfo.XuLyDonID;
                    parms[1].Value = DTInfo.CQDaGiaiQuyetID;
                    parms[2].Value = DTInfo.CQTiepNhanID;
                    parms[3].Value = DTInfo.DuAnID;
                    //parms[2].Value = DTInfo.GapLanhDao;
                    //parms[8].Value = DTInfo.NgayGapLanhDao;
                    break;
                case "step4":
                    parms[0].Value = DTInfo.XuLyDonID;
                    parms[1].Value = DTInfo.HuongGiaiQuyetID;
                    parms[2].Value = DTInfo.NoiDungHuongDan;
                    parms[3].Value = DTInfo.CanBoXuLyID;
                    parms[4].Value = DTInfo.CQTiepNhanID;
                    parms[5].Value = DTInfo.CanBoKyID;
                    parms[6].Value = DTInfo.CQGiaiQuyetTiepID;
                    parms[7].Value = DTInfo.ThuocThamQuyen;
                    parms[8].Value = DTInfo.DuDieuKien;
                    parms[9].Value = DTInfo.TrangThaiDonID;
                    break;
                default:
                    break;
            }

        }

        #region ==insert bang don thu==
        private SqlParameter[] GetInsertParmsDonThu()
        {
            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter(PARAM_NHOM_KN_ID, SqlDbType.Int),
                new SqlParameter(PARAM_DOI_TUONG_BI_KN_ID, SqlDbType.Int),
                new SqlParameter(PARAM_LOAI_KHIEU_TO_1ID, SqlDbType.Int),
                new SqlParameter(PARAM_LOAI_KHIEU_TO_2ID, SqlDbType.Int),
                new SqlParameter(PARAM_LOAI_KHIEU_TO_3ID, SqlDbType.Int),
                new SqlParameter(PARAM_LOAI_KHIEU_TO_ID, SqlDbType.Int),
                new SqlParameter(PARAM_NOI_DUNG_DON, SqlDbType.NVarChar),
                new SqlParameter(PARAM_TRUNG_DON, SqlDbType.Bit),
                new SqlParameter(PARAM_TINH_ID, SqlDbType.Int),
                new SqlParameter(PARAM_HUYEN_ID, SqlDbType.Int),
                new SqlParameter(PARAM_XA_ID, SqlDbType.Int),
                new SqlParameter(PARAM_LETANCHUYEN, SqlDbType.Bit),
                new SqlParameter(PARAM_NGAYVIETDON, SqlDbType.DateTime),
                new SqlParameter(PARAM_DIACHI, SqlDbType.NVarChar),
                };
            return parms;
        }

        private void SetInsertParmsDonThu(SqlParameter[] parms, TiepDanInfo tdInfo)
        {

            parms[0].Value = tdInfo.NhomKNID ?? Convert.DBNull;
            parms[1].Value = tdInfo.DoiTuongBiKNID ?? Convert.DBNull;
            parms[2].Value = tdInfo.LoaiKhieuTo1ID ?? Convert.DBNull;
            parms[3].Value = tdInfo.LoaiKhieuTo2ID ?? Convert.DBNull;
            parms[4].Value = tdInfo.LoaiKhieuTo3ID ?? Convert.DBNull;
            parms[5].Value = tdInfo.LoaiKhieuToID ?? Convert.DBNull;
            parms[6].Value = tdInfo.NoiDungDon ?? Convert.DBNull;
            parms[7].Value = tdInfo.TrungDon ?? Convert.DBNull;
            parms[8].Value = tdInfo.TinhID ?? Convert.DBNull;
            parms[9].Value = tdInfo.HuyenID ?? Convert.DBNull;
            parms[10].Value = tdInfo.XaID ?? Convert.DBNull;
            parms[11].Value = tdInfo.LeTanChuyen ?? Convert.DBNull;

            if (tdInfo.NgayVietDon == DateTime.MinValue)
            {
                parms[12].Value = DBNull.Value;
            }
            else
            {
                parms[12].Value = tdInfo.NgayVietDon ?? Convert.DBNull;
            }

            if (tdInfo.DoiTuongBiKNID == 0) parms[1].Value = DBNull.Value;
            parms[13].Value = tdInfo.DiaChiPhatSinh ?? Convert.DBNull;
        }


        private SqlParameter[] GetUpdateParmsDonThu()
        {
            SqlParameter[] parms = new SqlParameter[]{

                new SqlParameter(PARAM_DON_THU_ID, SqlDbType.Int),
                new SqlParameter(PARAM_NHOM_KN_ID, SqlDbType.Int),
                new SqlParameter(PARAM_DOI_TUONG_BI_KN_ID, SqlDbType.Int),
                new SqlParameter(PARAM_LOAI_KHIEU_TO_1ID, SqlDbType.Int),
                new SqlParameter(PARAM_LOAI_KHIEU_TO_2ID, SqlDbType.Int),
                new SqlParameter(PARAM_LOAI_KHIEU_TO_3ID, SqlDbType.Int),
                new SqlParameter(PARAM_LOAI_KHIEU_TO_ID, SqlDbType.Int),
                new SqlParameter(PARAM_NOI_DUNG_DON, SqlDbType.NVarChar),
                new SqlParameter(PARAM_TRUNG_DON, SqlDbType.Bit),
                new SqlParameter(PARAM_TINH_ID, SqlDbType.Int),
                new SqlParameter(PARAM_HUYEN_ID, SqlDbType.Int),
                new SqlParameter(PARAM_XA_ID, SqlDbType.Int),
                new SqlParameter(PARAM_LETANCHUYEN, SqlDbType.Bit),
                new SqlParameter(PARAM_NGAYVIETDON, SqlDbType.DateTime),
                new SqlParameter(PARAM_DIACHI, SqlDbType.NVarChar),
            };
            return parms;
        }

        private void SetUpdateParmsDonThu(SqlParameter[] parms, TiepDanInfo tdInfo)
        {
            parms[0].Value = tdInfo.DonThuID ?? Convert.DBNull;
            parms[1].Value = tdInfo.NhomKNID ?? Convert.DBNull;
            parms[2].Value = tdInfo.DoiTuongBiKNID ?? Convert.DBNull;
            parms[3].Value = tdInfo.LoaiKhieuTo1ID ?? Convert.DBNull;
            parms[4].Value = tdInfo.LoaiKhieuTo2ID ?? Convert.DBNull;
            parms[5].Value = tdInfo.LoaiKhieuTo3ID ?? Convert.DBNull;
            parms[6].Value = tdInfo.LoaiKhieuToID ?? Convert.DBNull;
            parms[7].Value = tdInfo.NoiDungDon ?? Convert.DBNull;
            parms[8].Value = tdInfo.TrungDon ?? Convert.DBNull;
            parms[9].Value = tdInfo.TinhID ?? Convert.DBNull;
            parms[10].Value = tdInfo.HuyenID ?? Convert.DBNull;
            parms[11].Value = tdInfo.XaID ?? Convert.DBNull;
            parms[12].Value = tdInfo.LeTanChuyen ?? Convert.DBNull;
            if (tdInfo.NgayVietDon == DateTime.MinValue)
            {
                parms[13].Value = DBNull.Value;
            }
            else
            {
                parms[13].Value = tdInfo.NgayVietDon ?? Convert.DBNull;
            }
            if (tdInfo.DoiTuongBiKNID == 0) parms[2].Value = DBNull.Value;
            parms[14].Value = tdInfo.DiaChiPhatSinh ?? Convert.DBNull;

        }
        #endregion

        #region ==insert bang xu ly don==
        private SqlParameter[] GetInsertParmsXuLyDon()
        {
            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter(PARAM_DON_THU_ID, SqlDbType.Int),
                new SqlParameter(PARAM_SO_LAN, SqlDbType.Int),
                new SqlParameter(PARAM_SO_DON_THU, SqlDbType.NVarChar, 255),
                new SqlParameter(PARAM_NGAY_NHAP_DON, SqlDbType.DateTime),
                new SqlParameter(PARAM_NGAY_QUA_HAN, SqlDbType.DateTime),
                new SqlParameter(PARAM_NGUON_DON_DEN_ID, SqlDbType.Int),
                new SqlParameter(PARAM_CQ_CHUYEN_DON_ID, SqlDbType.Int),
                new SqlParameter(PARAM_SO_CONG_VAN, SqlDbType.NVarChar),
                new SqlParameter(PARAM_NGAY_CHUYEN_DON, SqlDbType.DateTime),
                new SqlParameter(PARAM_THUOCTHAMQUYEN, SqlDbType.Bit),
                new SqlParameter(PARAM_DUDIEUKIEN, SqlDbType.Bit),
                new SqlParameter(PARAM_HUONG_GIAI_QUYET_ID, SqlDbType.Int),
                new SqlParameter(PARAM_NOI_DUNG_HUONG_DAN, SqlDbType.NVarChar),
                new SqlParameter(PARAM_CAN_BO_XU_LY_ID, SqlDbType.Int),
                new SqlParameter(PARAM_CAN_BO_KY_ID, SqlDbType.Int),
                new SqlParameter(PARAM_CQ_DA_GIAI_QUYET_ID, SqlDbType.NVarChar,250),
                new SqlParameter(PARAM_TRANG_THAI_DON_ID, SqlDbType.Int),
                new SqlParameter(PARAM_PT_KET_QUA_ID, SqlDbType.Int),
                new SqlParameter(PARAM_CANBO_TIEPNHAP_ID, SqlDbType.Int),
                new SqlParameter(PARAM_CO_QUAN_ID, SqlDbType.Int),
                new SqlParameter(PARAM_NGAYTHULY, SqlDbType.DateTime),
                new SqlParameter(PARAM_LYDO, SqlDbType.NVarChar),
                new SqlParameter(PARAM_DUAN_ID, SqlDbType.Int),
                new SqlParameter(PARAM_NGAYXULY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DADUYET_XULY, SqlDbType.Bit),
                new SqlParameter(PARAM_CQ_CHUYEN_DONDEN_ID, SqlDbType.Int),
                new SqlParameter(PARAM_CBDUOC_CHONXL, SqlDbType.Int),
                new SqlParameter(PARAM_QTTIEPNHANDON, SqlDbType.Int),
                new SqlParameter(PARAM_DONTHUGOC_ID, SqlDbType.Int),
                new SqlParameter(PARAM_LANGIAIQUYET, SqlDbType.Int),
                new SqlParameter("@XuLyDonChuyenID", SqlDbType.Int),
                new SqlParameter("@NgayTiep", SqlDbType.DateTime),
                };
            return parms;
        }

        private void SetInsertParmsXuLyDon(SqlParameter[] parms, TiepDanInfo tdInfo)
        {

            parms[0].Value = tdInfo.DonThuID ?? Convert.DBNull;
            parms[1].Value = tdInfo.SoLan ?? Convert.DBNull;
            parms[2].Value = tdInfo.SoDonThu ?? Convert.DBNull;

            if (tdInfo.NgayNhapDon == DateTime.MinValue)
                parms[3].Value = DBNull.Value;
            else
                parms[3].Value = tdInfo.NgayNhapDon ?? Convert.DBNull;

            if (tdInfo.NgayQuaHan == DateTime.MinValue)
                parms[4].Value = DBNull.Value;
            else
            {
                parms[4].Value = tdInfo.NgayQuaHan ?? Convert.DBNull;
            }
            parms[5].Value = tdInfo.NguonDonDen ?? Convert.DBNull;
            parms[6].Value = tdInfo.CQChuyenDonID ?? Convert.DBNull;
            parms[7].Value = tdInfo.SoCongVan ?? Convert.DBNull;
            if (tdInfo.NgayChuyenDon == DateTime.MinValue)
            {
                parms[8].Value = DBNull.Value;
            }
            else
            {
                parms[8].Value = tdInfo.NgayChuyenDon ?? Convert.DBNull;
            }
            parms[9].Value = tdInfo.ThuocThamQuyen ?? Convert.DBNull;
            parms[10].Value = tdInfo.DuDieuKien ?? Convert.DBNull;
            parms[11].Value = tdInfo.HuongGiaiQuyetID ?? Convert.DBNull;
            parms[12].Value = tdInfo.NoiDungHuongDan ?? Convert.DBNull;
            parms[13].Value = tdInfo.CanBoXuLyID ?? Convert.DBNull;
            parms[14].Value = tdInfo.CanBoKyID ?? Convert.DBNull;
            parms[15].Value = tdInfo.CQDaGiaiQuyetID ?? Convert.DBNull;
            parms[16].Value = tdInfo.TrangThaiDonID ?? Convert.DBNull;
            parms[17].Value = tdInfo.PhanTichKQID ?? Convert.DBNull;
            parms[18].Value = tdInfo.CanBoTiepNhapID ?? Convert.DBNull;
            parms[19].Value = tdInfo.CoQuanID ?? Convert.DBNull;
            if (tdInfo.NgayThuLy == DateTime.MinValue)
                parms[20].Value = DBNull.Value;
            else
                parms[20].Value = tdInfo.NgayThuLy ?? Convert.DBNull;

            parms[21].Value = tdInfo.LyDo ?? Convert.DBNull;
            parms[22].Value = tdInfo.DuAnID ?? Convert.DBNull;
            if (tdInfo.NgayXuLy != DateTime.MinValue)
                parms[23].Value = tdInfo.NgayXuLy ?? Convert.DBNull;
            else
                parms[23].Value = DBNull.Value;
            parms[24].Value = tdInfo.DaDuyetXuLy ?? Convert.DBNull;
            parms[25].Value = tdInfo.CQChuyenDonDenID ?? Convert.DBNull;
            parms[26].Value = tdInfo.CBDuocChonXL ?? Convert.DBNull;
            parms[27].Value = tdInfo.QTTiepNhanDon ?? Convert.DBNull;
            parms[28].Value = tdInfo.DonThuGocID ?? Convert.DBNull;
            parms[29].Value = tdInfo.LanGiaiQuyet ?? Convert.DBNull;

            if (tdInfo.HuongGiaiQuyetID == 0) parms[11].Value = DBNull.Value;
            if (tdInfo.CanBoXuLyID == 0) parms[13].Value = DBNull.Value;
            if (tdInfo.CQChuyenDonDenID == 0) parms[25].Value = DBNull.Value;
            if (tdInfo.CBDuocChonXL == 0) parms[26].Value = DBNull.Value;
            if (tdInfo.QTTiepNhanDon == 0) parms[27].Value = DBNull.Value;
            if (tdInfo.DonThuGocID == 0) parms[28].Value = DBNull.Value;
            parms[30].Value = tdInfo.XuLyDonIDGoc ?? Convert.DBNull;
            parms[31].Value = tdInfo.NgayTiep ?? Convert.DBNull;
        }

        private SqlParameter[] GetUpdateParmsXuLyDon()
        {
            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int),
                new SqlParameter(PARAM_DON_THU_ID, SqlDbType.Int),
                new SqlParameter(PARAM_SO_LAN, SqlDbType.Int),
                new SqlParameter(PARAM_NGAY_NHAP_DON, SqlDbType.DateTime),
                new SqlParameter(PARAM_NGAY_QUA_HAN, SqlDbType.DateTime),
                new SqlParameter(PARAM_NGUON_DON_DEN_ID, SqlDbType.Int),
                new SqlParameter(PARAM_CQ_CHUYEN_DON_ID, SqlDbType.Int),
                new SqlParameter(PARAM_SO_CONG_VAN, SqlDbType.NVarChar),
                new SqlParameter(PARAM_NGAY_CHUYEN_DON, SqlDbType.DateTime),
                new SqlParameter(PARAM_THUOCTHAMQUYEN, SqlDbType.Bit),
                new SqlParameter(PARAM_DUDIEUKIEN, SqlDbType.Bit),
                new SqlParameter(PARAM_HUONG_GIAI_QUYET_ID, SqlDbType.Int),
                new SqlParameter(PARAM_NOI_DUNG_HUONG_DAN, SqlDbType.NVarChar),
                new SqlParameter(PARAM_CAN_BO_XU_LY_ID, SqlDbType.Int),
                new SqlParameter(PARAM_CAN_BO_KY_ID, SqlDbType.Int),
                new SqlParameter(PARAM_CQ_DA_GIAI_QUYET_ID, SqlDbType.NVarChar,250),
                new SqlParameter(PARAM_TRANG_THAI_DON_ID, SqlDbType.Int),
                new SqlParameter(PARAM_PT_KET_QUA_ID, SqlDbType.Int),
                new SqlParameter(PARAM_CANBO_TIEPNHAP_ID, SqlDbType.Int),
                new SqlParameter(PARAM_CO_QUAN_ID, SqlDbType.Int),
                new SqlParameter(PARAM_NGAYTHULY, SqlDbType.DateTime),
                new SqlParameter(PARAM_LYDO, SqlDbType.NVarChar),
                new SqlParameter(PARAM_DUAN_ID, SqlDbType.Int),
                new SqlParameter(PARAM_NGAYXULY, SqlDbType.DateTime),
                new SqlParameter(PARAM_CBDUOC_CHONXL, SqlDbType.Int),
                new SqlParameter(PARAM_QTTIEPNHANDON, SqlDbType.Int),
                new SqlParameter(PARAM_DONTHUGOC_ID, SqlDbType.Int),
                new SqlParameter(PARAM_LANGIAIQUYET, SqlDbType.Int)

            };
            return parms;
        }

        private void SetUpdateParmsXuLyDon(SqlParameter[] parms, TiepDanInfo tdInfo)
        {
            parms[0].Value = tdInfo.XuLyDonID ?? Convert.DBNull;
            parms[1].Value = tdInfo.DonThuID ?? Convert.DBNull;
            parms[2].Value = tdInfo.SoLan ?? Convert.DBNull;
            if (tdInfo.NgayNhapDon == DateTime.MinValue)
            {
                parms[3].Value = DBNull.Value;
            }
            else
            {
                parms[3].Value = tdInfo.NgayNhapDon ?? Convert.DBNull;
            }
            if (tdInfo.NgayQuaHan == DateTime.MinValue)
            {
                parms[4].Value = DBNull.Value;
            }
            else
            {
                parms[4].Value = tdInfo.NgayQuaHan ?? Convert.DBNull;
            }
            parms[5].Value = tdInfo.NguonDonDen ?? Convert.DBNull;
            parms[6].Value = tdInfo.CQChuyenDonID ?? Convert.DBNull;
            parms[7].Value = tdInfo.SoCongVan ?? Convert.DBNull;
            if (tdInfo.NgayChuyenDon == DateTime.MinValue)
            {
                parms[8].Value = DBNull.Value;
            }
            else
            {
                parms[8].Value = tdInfo.NgayChuyenDon ?? Convert.DBNull;
            }

            parms[9].Value = tdInfo.ThuocThamQuyen ?? Convert.DBNull;
            parms[10].Value = tdInfo.DuDieuKien ?? Convert.DBNull;
            parms[11].Value = tdInfo.HuongGiaiQuyetID ?? Convert.DBNull;
            parms[12].Value = tdInfo.NoiDungHuongDan ?? Convert.DBNull;
            parms[13].Value = tdInfo.CanBoXuLyID ?? Convert.DBNull;
            parms[14].Value = tdInfo.CanBoKyID ?? Convert.DBNull;
            parms[15].Value = tdInfo.CQDaGiaiQuyetID ?? Convert.DBNull;
            parms[16].Value = tdInfo.TrangThaiDonID ?? Convert.DBNull;
            parms[17].Value = tdInfo.PhanTichKQID ?? Convert.DBNull;
            parms[18].Value = tdInfo.CanBoTiepNhapID ?? Convert.DBNull;
            parms[19].Value = tdInfo.CoQuanID ?? Convert.DBNull;
            if (tdInfo.NgayThuLy == DateTime.MinValue)
            {
                parms[20].Value = DBNull.Value;
            }
            else
            {
                parms[20].Value = tdInfo.NgayThuLy ?? Convert.DBNull;
            }
            parms[21].Value = tdInfo.LyDo ?? Convert.DBNull;
            parms[22].Value = tdInfo.DuAnID ?? Convert.DBNull;

            if (tdInfo.NgayXuLy == DateTime.MinValue)
            {
                parms[23].Value = DBNull.Value;
            }
            else
            {
                parms[23].Value = tdInfo.NgayXuLy ?? Convert.DBNull;
            }
            parms[24].Value = tdInfo.CBDuocChonXL ?? Convert.DBNull;
            parms[25].Value = tdInfo.QTTiepNhanDon ?? Convert.DBNull;
            parms[26].Value = tdInfo.DonThuGocID ?? Convert.DBNull;
            parms[27].Value = tdInfo.LanGiaiQuyet ?? Convert.DBNull;

            if (tdInfo.HuongGiaiQuyetID == 0) parms[11].Value = DBNull.Value;
            if (tdInfo.CBDuocChonXL == 0) parms[24].Value = DBNull.Value;
            if (tdInfo.QTTiepNhanDon == 0) parms[25].Value = DBNull.Value;
            if (tdInfo.DonThuGocID == 0) parms[26].Value = DBNull.Value;

        }
        #endregion

        #region ==insert bang tiep dan khong don==
        private SqlParameter[] GetInsertParmsTiepDan()
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_NGAY_TIEP, SqlDbType.DateTime),
                new SqlParameter(PARAM_GAP_LANH_DAO, SqlDbType.Bit),
                new SqlParameter(PARAM_NGAY_GAP_LANH_DAO, SqlDbType.DateTime),
                new SqlParameter(PARAM_NOI_DUNG_TIEP, SqlDbType.NVarChar),
                new SqlParameter(PARAM_VU_VIEC_CU, SqlDbType.Bit),
                new SqlParameter(PARAM_CAN_BO_TIEP_ID, SqlDbType.Int),
                new SqlParameter(PARAM_DON_THU_ID, SqlDbType.Int),
                new SqlParameter(PARAM_CO_QUAN_ID, SqlDbType.Int),
                new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int),
                new SqlParameter(PARAM_LAN_TIEP, SqlDbType.Int),
                new SqlParameter(PARAM_KET_QUA_TIEP, SqlDbType.NVarChar),
                new SqlParameter(PARAM_SO_DON, SqlDbType.NVarChar),
                new SqlParameter(PARAM_NHOM_KN_ID, SqlDbType.Int),
                new SqlParameter(PARAM_LOAI_KHIEU_TO_1ID, SqlDbType.Int),
                new SqlParameter(PARAM_LOAI_KHIEU_TO_2ID, SqlDbType.Int),
                new SqlParameter(PARAM_LOAI_KHIEU_TO_3ID, SqlDbType.Int),
                new SqlParameter(PARAM_TENLANHDAOTIEP,SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LANHDAO_DANGKY,SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_KQ_TIEPDAN,SqlDbType.Int),
                //new SqlParameter(PARAM_HUONGGIAIQUYETID, SqlDbType.Int),
                new SqlParameter(PARAM_YEU_CAU_NGUOI_DUOC_TIEP, SqlDbType.NVarChar),
                new SqlParameter(PARAM_THONG_TIN_TAI_LIEU, SqlDbType.NVarChar),
                new SqlParameter(PARAM_KET_LUAN_NGUOI_CHU_TRI, SqlDbType.NVarChar),
                new SqlParameter(PARAM_NGUOI_DUOC_TIEP_PHAT_BIEU, SqlDbType.NVarChar),
                new SqlParameter(PARAM_TIEP_DAN_CO_DON, SqlDbType.Int),
                new SqlParameter(PARAM_NHOM_THAM_QUYEN_GIAI_QUYET_ID, SqlDbType.Int),
                new SqlParameter(PARAM_NGAY_BAN_HANH_QUYET_DINH_GIAI_QUYET, SqlDbType.DateTime),
                new SqlParameter(PARAM_CHUC_VU, SqlDbType.NVarChar),
                new SqlParameter(PARAM_LOAI_TIEP_DAN_ID, SqlDbType.Int),
                new SqlParameter(PARAM_KET_QUA_GIAI_QUYET, SqlDbType.NVarChar),
                new SqlParameter(PARAM_THANH_PHAN_THAM_GIA_TIEP, SqlDbType.NVarChar),
                 new SqlParameter(PARAM_UY_QUYEN_TIEP, SqlDbType.Bit),
            };
            return parms;
        }

        private void SetInsertParmsTiepDan(SqlParameter[] parms, TiepDanInfo tdInfo)
        {

            parms[0].Value = tdInfo.NgayTiep ?? Convert.DBNull;
            parms[1].Value = tdInfo.GapLanhDao ?? Convert.DBNull;
            if (tdInfo.NgayGapLanhDao == DateTime.MinValue)
                parms[2].Value = DBNull.Value;
            else
                parms[2].Value = tdInfo.NgayGapLanhDao ?? Convert.DBNull;
            parms[3].Value = tdInfo.NoiDungTiep ?? Convert.DBNull;
            parms[4].Value = tdInfo.VuViecCu ?? Convert.DBNull;
            parms[5].Value = tdInfo.CanBoTiepID ?? Convert.DBNull;
            parms[6].Value = tdInfo.DonThuID ?? Convert.DBNull;
            parms[7].Value = tdInfo.CoQuanID ?? Convert.DBNull;
            parms[8].Value = tdInfo.XuLyDonID ?? Convert.DBNull;
            parms[9].Value = tdInfo.LanTiep ?? Convert.DBNull;
            parms[10].Value = tdInfo.KetQuaTiep ?? Convert.DBNull;
            parms[11].Value = tdInfo.SoDon ?? Convert.DBNull;
            parms[12].Value = tdInfo.NhomKNID ?? Convert.DBNull;
            parms[13].Value = tdInfo.LoaiKhieuTo1ID ?? Convert.DBNull;
            parms[14].Value = tdInfo.LoaiKhieuTo2ID ?? Convert.DBNull;
            parms[15].Value = tdInfo.LoaiKhieuTo3ID ?? Convert.DBNull;
            parms[16].Value = tdInfo.TenLanhDaoTiep ?? Convert.DBNull;
            parms[17].Value = tdInfo.LanhDaoDangKy ?? Convert.DBNull;
            parms[18].Value = tdInfo.KQQuaTiepDan ?? Convert.DBNull;
            parms[19].Value = tdInfo.YeuCauNguoiDuocTiep ?? Convert.DBNull;
            parms[20].Value = tdInfo.ThongTinTaiLieu ?? Convert.DBNull;
            parms[21].Value = tdInfo.KetLuanNguoiChuTri ?? Convert.DBNull;
            parms[22].Value = tdInfo.NguoiDuocTiepPhatBieu ?? Convert.DBNull;
            parms[23].Value = tdInfo.TiepDanCoDon ?? Convert.DBNull;
            parms[24].Value = tdInfo.NhomThamQuyenGiaiQuyetID ?? Convert.DBNull;
            parms[25].Value = tdInfo.NgayBanHanhQuyetDinhGiaiQuyet ?? Convert.DBNull;
            parms[26].Value = tdInfo.ChucVu ?? Convert.DBNull;
            parms[27].Value = tdInfo.LoaiTiepDanID ?? Convert.DBNull;
            parms[28].Value = tdInfo.KetQuaGiaiQuyet ?? Convert.DBNull;
            parms[29].Value = tdInfo.ThanhPhanThamGiaTiep ?? Convert.DBNull;
            parms[30].Value = tdInfo.UyQuyenTiep ?? Convert.DBNull;
            //parms[18].Value = tdInfo.HuongGiaiQuyetID;
            if (tdInfo.DonThuID == 0) parms[6].Value = DBNull.Value;
            if (tdInfo.XuLyDonID == 0) parms[8].Value = DBNull.Value;

            if (tdInfo.LoaiKhieuTo1ID == 0) parms[13].Value = DBNull.Value;
            if (tdInfo.LoaiKhieuTo2ID == 0) parms[14].Value = DBNull.Value;
            if (tdInfo.LoaiKhieuTo3ID == 0) parms[15].Value = DBNull.Value;
            if (tdInfo.LanhDaoDangKy == string.Empty) parms[17].Value = DBNull.Value;

        }

        private SqlParameter[] GetUpdateParmsTiepDan()
        {
            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter(PARAM_TIEP_DAN_KHONG_DON_ID, SqlDbType.Int),
                new SqlParameter(PARAM_NGAY_TIEP, SqlDbType.DateTime),
                new SqlParameter(PARAM_GAP_LANH_DAO, SqlDbType.Bit),
                new SqlParameter(PARAM_NGAY_GAP_LANH_DAO, SqlDbType.DateTime),
                new SqlParameter(PARAM_NOI_DUNG_TIEP, SqlDbType.NVarChar),
                new SqlParameter(PARAM_VU_VIEC_CU, SqlDbType.Bit),
                new SqlParameter(PARAM_CAN_BO_TIEP_ID, SqlDbType.Int),
                new SqlParameter(PARAM_DON_THU_ID, SqlDbType.Int),
                new SqlParameter(PARAM_CO_QUAN_ID, SqlDbType.Int),
                new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int),
                new SqlParameter(PARAM_LAN_TIEP, SqlDbType.Int),
                new SqlParameter(PARAM_KET_QUA_TIEP, SqlDbType.NVarChar),
                new SqlParameter(PARAM_NHOM_KN_ID, SqlDbType.Int),
                new SqlParameter(PARAM_LOAI_KHIEU_TO_1ID, SqlDbType.Int),
                new SqlParameter(PARAM_LOAI_KHIEU_TO_2ID, SqlDbType.Int),
                new SqlParameter(PARAM_LOAI_KHIEU_TO_3ID, SqlDbType.Int),
                new SqlParameter(PARAM_TENLANHDAOTIEP,SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_KQ_TIEPDAN,SqlDbType.Int),
                new SqlParameter(PARAM_YEU_CAU_NGUOI_DUOC_TIEP, SqlDbType.NVarChar),
                new SqlParameter(PARAM_THONG_TIN_TAI_LIEU, SqlDbType.NVarChar),
                new SqlParameter(PARAM_KET_LUAN_NGUOI_CHU_TRI, SqlDbType.NVarChar),
                new SqlParameter(PARAM_NGUOI_DUOC_TIEP_PHAT_BIEU, SqlDbType.NVarChar),
                new SqlParameter(PARAM_LOAI_TIEP_DAN_ID, SqlDbType.Int),
                 new SqlParameter(PARAM_KET_QUA_GIAI_QUYET, SqlDbType.NVarChar),
                 new SqlParameter(PARAM_THANH_PHAN_THAM_GIA_TIEP, SqlDbType.NVarChar),
            };
            return parms;
        }

        private void SetUpdateParmsTiepDan(SqlParameter[] parms, TiepDanInfo tdInfo)
        {
            parms[0].Value = tdInfo.TiepDanKhongDonID;

            if (tdInfo.NgayTiep == DateTime.MinValue)
            {
                parms[1].Value = DBNull.Value;
            }
            else { parms[1].Value = tdInfo.NgayTiep ?? Convert.DBNull; }
            parms[2].Value = tdInfo.GapLanhDao ?? Convert.DBNull;
            if (tdInfo.NgayGapLanhDao == DateTime.MinValue)
            {
                parms[3].Value = DBNull.Value;
            }
            else
            {
                parms[3].Value = tdInfo.NgayGapLanhDao ?? Convert.DBNull;
            }

            parms[4].Value = tdInfo.NoiDungTiep ?? Convert.DBNull;
            parms[5].Value = tdInfo.VuViecCu ?? Convert.DBNull;
            parms[6].Value = tdInfo.CanBoTiepID ?? Convert.DBNull;
            parms[7].Value = tdInfo.DonThuID ?? Convert.DBNull;
            parms[8].Value = tdInfo.CoQuanID ?? Convert.DBNull;
            parms[9].Value = tdInfo.XuLyDonID ?? Convert.DBNull;
            parms[10].Value = tdInfo.LanTiep ?? Convert.DBNull;
            parms[11].Value = tdInfo.KetQuaTiep ?? Convert.DBNull;
            parms[12].Value = tdInfo.NhomKNID ?? Convert.DBNull;
            parms[13].Value = tdInfo.LoaiKhieuTo1ID ?? Convert.DBNull;
            parms[14].Value = tdInfo.LoaiKhieuTo2ID ?? Convert.DBNull;
            parms[15].Value = tdInfo.LoaiKhieuTo3ID ?? Convert.DBNull;
            parms[16].Value = tdInfo.TenLanhDaoTiep ?? Convert.DBNull;

            if (tdInfo.DonThuID == 0) parms[7].Value = DBNull.Value;
            if (tdInfo.XuLyDonID == 0) parms[9].Value = DBNull.Value;
            if (tdInfo.LoaiKhieuTo1ID == 0) parms[13].Value = DBNull.Value;
            if (tdInfo.LoaiKhieuTo2ID == 0) parms[14].Value = DBNull.Value;
            if (tdInfo.LoaiKhieuTo3ID == 0) parms[15].Value = DBNull.Value;
            if (tdInfo.TenLanhDaoTiep == string.Empty) parms[16].Value = DBNull.Value;
            parms[17].Value = tdInfo.KQQuaTiepDan ?? Convert.DBNull;
            parms[18].Value = tdInfo.YeuCauNguoiDuocTiep ?? Convert.DBNull;
            parms[19].Value = tdInfo.ThongTinTaiLieu ?? Convert.DBNull;
            parms[20].Value = tdInfo.KetLuanNguoiChuTri ?? Convert.DBNull;
            parms[21].Value = tdInfo.NguoiDuocTiepPhatBieu ?? Convert.DBNull;
            parms[22].Value = tdInfo.LoaiTiepDanID ?? Convert.DBNull;
            parms[23].Value = tdInfo.KetQuaGiaiQuyet ?? Convert.DBNull;
            parms[24].Value = tdInfo.ThanhPhanThamGiaTiep ?? Convert.DBNull;
        }
        #endregion

        public int UpdateXuLyDon(TiepDanInfo tdInfo)
        {

            int val = 0;
            SqlParameter[] parameters = GetUpdateParmsXuLyDon();
            SetUpdateParmsXuLyDon(parameters, tdInfo);

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, UPDATE_XULYDON, parameters);
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

        public int InsertXuLyDon(TiepDanInfo tdInfo)
        {

            object val = null;

            SqlParameter[] parameters = GetInsertParmsXuLyDon();
            SetInsertParmsXuLyDon(parameters, tdInfo);

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, "NVTiepDan_InsertXuLyDon_New", parameters);
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
        public int InsertXuLyDon1(TiepDanInfo tdInfo)
        {

            object val = null;

            SqlParameter[] parameters = GetInsertParmsXuLyDon();
            SetInsertParmsXuLyDon(parameters, tdInfo);

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, INSERT_XULYDON, parameters);
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

        public int UpdateDonThu(TiepDanInfo tdInfo)
        {

            int val = 0;
            SqlParameter[] parameters = GetUpdateParmsDonThu();
            SetUpdateParmsDonThu(parameters, tdInfo);

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, UPDATE_DONTHU, parameters);
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

        public int InsertDonThu(TiepDanInfo tdInfo)
        {

            object val = null;

            SqlParameter[] parameters = GetInsertParmsDonThu();
            SetInsertParmsDonThu(parameters, tdInfo);

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, INSERT_DONTHU, parameters);
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

        public int UpdateTiepDan(TiepDanInfo tdInfo)
        {

            int val = 0;
            SqlParameter[] parameters = GetUpdateParmsTiepDan();
            SetUpdateParmsTiepDan(parameters, tdInfo);

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "v2_NVTiepDan_UpdateTiepDan_New", parameters);
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

        public int InsertTiepDan(TiepDanInfo tdInfo)
        {

            object val = null;

            SqlParameter[] parameters = GetInsertParmsTiepDan();
            SetInsertParmsTiepDan(parameters, tdInfo);

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        //val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, INSERT_TIEPDAN_NEW, parameters);
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, "v2_NVTiepDan_InsertTiepDan_New", parameters);
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

        private TiepDanInfo GetTiepDanData(SqlDataReader dr)
        {
            TiepDanInfo info = new TiepDanInfo();

            info.TiepDanKhongDonID = Utils.GetInt32(dr["TiepDanKhongDonID"], 0);
            info.NgayTiep = Utils.GetDateTime(dr["NgayTiep"], DateTime.MinValue);
            info.GapLanhDao = Utils.GetBoolean(dr["GapLanhDao"], false);
            info.NgayGapLanhDao = Utils.GetDateTime(dr["NgayGapLanhDao"], DateTime.MinValue);
            info.NoiDungTiep = Utils.GetString(dr["NoiDungTiep"], string.Empty);
            info.VuViecCu = Utils.GetBoolean(dr["VuViecCu"], false);
            info.CanBoTiepID = Utils.GetInt32(dr["CanBoTiepID"], 0);
            info.CoQuanID = Utils.GetInt32(dr["CoQuanID"], 0);
            info.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);
            info.SoDon = Utils.GetString(dr["SoDon"], string.Empty);
            info.DonThuID = Utils.GetInt32(dr["DonThuID"], 0);
            info.LanTiep = Utils.GetInt32(dr["LanTiep"], 0);
            info.KetQuaTiep = Utils.GetString(dr["KetQuaTiep"], string.Empty);
            info.NhomKNID = Utils.GetInt32(dr["NhomKNID"], 0);
            info.TenLanhDaoTiep = Utils.GetString(dr["TenLanhDaoTiep"], string.Empty);

            info.YeuCauNguoiDuocTiep = Utils.GetString(dr["YeuCauNguoiDuocTiep"], string.Empty);
            info.ThongTinTaiLieu = Utils.GetString(dr["ThongTinTaiLieu"], string.Empty);
            info.KetLuanNguoiChuTri = Utils.GetString(dr["KetLuanNguoiChuTri"], string.Empty);
            info.NguoiDuocTiepPhatBieu = Utils.GetString(dr["NguoiDuocTiepPhatBieu"], string.Empty);

            return info;
        }

        public TiepDanInfo GetTiepDanByTiepDanID(int tiepdanID)
        {

            TiepDanInfo DTInfo = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_TIEP_DAN_KHONG_DON_ID,SqlDbType.Int)
            };
            parameters[0].Value = tiepdanID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_NVTiepDan_GetTiepDanByTiepDanID", parameters))
                {

                    if (dr.Read())
                    {
                        DTInfo = GetTiepDanData(dr);
                        DTInfo.LoaiTiepDanID = Utils.ConvertToInt32(dr["LoaiTiepDanID"], 0);
                        DTInfo.LoaiKhieuTo1ID = Utils.ConvertToInt32(dr["LoaiKhieuTo1ID"], 0);
                        DTInfo.LoaiKhieuTo2ID = Utils.ConvertToInt32(dr["LoaiKhieuTo2ID"], 0);
                        DTInfo.LoaiKhieuTo3ID = Utils.ConvertToInt32(dr["LoaiKhieuTo3ID"], 0);
                        DTInfo.LanhDaoDangKy = Utils.ConvertToString(dr["LanhDaoDangKy"], string.Empty);
                        DTInfo.CQDaGiaiQuyetID = Utils.ConvertToString(dr["CQDaGiaiQuyetID"], string.Empty);
                        DTInfo.KQQuaTiepDan = Utils.ConvertToInt32(dr["KQQuaTiepDan"], 0);
                        DTInfo.SoLan = Utils.ConvertToInt32(dr["SoLan"], 0);
                        DTInfo.CBDuocChonXL = Utils.ConvertToInt32(dr["CBDuocChonXL"], 0);
                        DTInfo.NgayNhapDon = Utils.ConvertToNullableDateTime(dr["NgayNhapDon"], null);
                        DTInfo.SoDonThu = Utils.GetString(dr["SoDonThu"], string.Empty);
                        DTInfo.TenCanBoTiep = Utils.GetString(dr["TenCanBoTiep"], string.Empty);
                        DTInfo.TiepDanCoDon = Utils.ConvertToInt32(dr["TiepDanCoDon"], 0);
                        DTInfo.NhomThamQuyenGiaiQuyetID = Utils.ConvertToInt32(dr["NhomThamQuyenGiaiQuyetID"], 0);
                        DTInfo.NgayBanHanhQuyetDinhGiaiQuyet = Utils.ConvertToNullableDateTime(dr["NgayBanHanhQuyetDinhGiaiQuyet"], null);
                        DTInfo.KetQuaGiaiQuyet = Utils.GetString(dr["KetQuaGiaiQuyet"], string.Empty);
                        DTInfo.ThanhPhanThamGiaTiep = Utils.GetString(dr["ThanhPhanThamGiaTiep"], string.Empty);
                        DTInfo.UyQuyenTiep = Utils.ConvertToBoolean(dr["UyQuyenTiep"], false);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return DTInfo;
        }
        public TiepDanInfo GetTiepDanByXuLyDonID(int xulydonid)
        {

            TiepDanInfo DTInfo = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDON_ID,SqlDbType.Int)
            };
            parameters[0].Value = xulydonid;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_MATIEPDAN, parameters))
                {

                    if (dr.Read())
                    {
                        DTInfo = GetTiepDanData(dr);
                        DTInfo.KetQuaGiaiQuyet = Utils.GetString(dr["KetQuaGiaiQuyet"], string.Empty);

                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return DTInfo;
        }

        private TiepDanInfo GetXuLyDonData(SqlDataReader dr)
        {
            TiepDanInfo info = new TiepDanInfo();

            info.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);
            info.DonThuID = Utils.GetInt32(dr["DonThuID"], 0);
            info.LanTiep = Utils.GetInt32(dr["LanTiep"], 0);
            info.CanBoTiepID = Utils.GetInt32(dr["CanBoTiepID"], 0);
            info.VuViecCu = Utils.GetBoolean(dr["VuViecCu"], false);
            info.KetQuaTiep = Utils.GetString(dr["KetQuaTiep"], string.Empty);
            info.KQQuaTiepDan = Utils.GetInt32(dr["KQQuaTiepDan"], 0);

            return info;
        }

        public TiepDanInfo GetXuLyDonByID(int tiepdanID)
        {

            TiepDanInfo DTInfo = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_TIEP_DAN_KHONG_DON_ID,SqlDbType.Int)
            };
            parameters[0].Value = tiepdanID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_XULYDON_BY_ID, parameters))
                {

                    if (dr.Read())
                    {
                        DTInfo = GetXuLyDonData(dr);
                        DTInfo.SoLan = Utils.ConvertToInt32(dr["SoLan"], 0);
                        DTInfo.LanGiaiQuyet = Utils.ConvertToInt32(dr["LanGiaiQuyet"], 0);
                        DTInfo.DonThuGocID = Utils.ConvertToInt32(dr["DonThuGocID"], 0);
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return DTInfo;
        }

        private TiepDanInfo GetAllXuLyDonData(SqlDataReader dr)
        {
            TiepDanInfo info = new TiepDanInfo();

            info.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);
            info.DonThuID = Utils.GetInt32(dr["DonThuID"], 0);
            info.SoLan = Utils.GetInt32(dr["SoLan"], 0);
            info.SoDonThu = Utils.GetString(dr["SoDonThu"], string.Empty);
            info.NgayNhapDon = Utils.GetDateTime(dr["NgayNhapDon"], DateTime.MinValue);
            info.NgayQuaHan = Utils.GetDateTime(dr["NgayQuaHan"], DateTime.MinValue);
            info.NguonDonDen = Utils.GetInt32(dr["NguonDonDen"], 0);
            info.CQChuyenDonID = Utils.GetInt32(dr["CQChuyenDonID"], 0);
            info.SoCongVan = Utils.GetString(dr["SoCongVan"], string.Empty);
            info.NgayChuyenDon = Utils.GetDateTime(dr["NgayChuyenDon"], DateTime.MinValue);
            info.ThuocThamQuyen = Utils.GetBoolean(dr["ThuocThamQuyen"], false);
            info.DuDieuKien = Utils.GetBoolean(dr["DuDieuKien"], false);
            info.HuongGiaiQuyetID = Utils.GetInt32(dr["HuongGiaiQuyetID"], 0);
            info.NoiDungHuongDan = Utils.GetString(dr["NoiDungHuongDan"], string.Empty);
            info.CanBoXuLyID = Utils.GetInt32(dr["CanBoXuLyID"], 0);
            info.CanBoKyID = Utils.GetInt32(dr["CanBoKyID"], 0);
            info.CQDaGiaiQuyetID = Utils.GetString(dr["CQDaGiaiQuyetID"], string.Empty);
            info.TrangThaiDonID = Utils.GetInt32(dr["TrangThaiDonID"], 0);
            info.PhanTichKQID = Utils.GetInt32(dr["PhanTichKQID"], 0);
            info.CanBoTiepNhapID = Utils.GetInt32(dr["CanBoTiepNhapID"], 0);
            info.CoQuanID = Utils.GetInt32(dr["CoQuanID"], 0);
            info.NgayThuLy = Utils.GetDateTime(dr["NgayThuLy"], DateTime.MinValue);
            info.LyDo = Utils.GetString(dr["LyDo"], string.Empty);
            info.DuAnID = Utils.GetInt32(dr["DuAnID"], 0);

            return info;
        }

        public TiepDanInfo GetXuLyDonByXLDID(int xulydonID)
        {

            TiepDanInfo DTInfo = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDON_ID,SqlDbType.Int)
            };
            parameters[0].Value = xulydonID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_ALLXULYDON_BY_ID, parameters))
                {

                    if (dr.Read())
                    {
                        DTInfo = GetAllXuLyDonData(dr);
                        DTInfo.CBDuocChonXL = Utils.GetInt32(dr["CBDuocChonXL"], 0);
                        DTInfo.QTTiepNhanDon = Utils.GetInt32(dr["QTTiepNhanDon"], 0);
                    }
                    dr.Close();
                }
            }
            catch
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
        public int Update(string step, XuLyDonInfo DTInfo)
        {

            object val = null;
            SqlParameter[] parameters = GetUpdateParms(step);
            SetUpdateParms(parameters, DTInfo, step);

            string stored = UPDATE_STEP1;

            switch (step)
            {
                case "step1":
                    stored = UPDATE_STEP1;
                    break;
                case "step2":
                    stored = UPDATE_STEP2;
                    break;
                case "step3":
                    stored = UPDATE_STEP3;
                    break;
                case "step4":
                    stored = UPDATE_STEP4;
                    break;
            }

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, stored, parameters);
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
        public int Insert(string step, XuLyDonInfo DTInfo)
        {

            object val = null;

            SqlParameter[] parameters = GetInsertParms(step);
            SetInsertParms(parameters, DTInfo, step);

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        if (step == "step1")
                            val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, INSERT_STEP1, parameters);
                        else
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

        ////lay so don thu theo co quan ID
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

        public string GetSoDonThu(int coQuanID)
        {
            string result = string.Empty;
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARAM_CO_QUAN_ID, SqlDbType.Int)
            };
            parameters[0].Value = coQuanID;
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_SO_DON_BY_CO_QUAN, parameters))
            {
                if (dr.Read())
                {
                    result = Utils.GetString(dr["SoDonThu"], string.Empty);
                }
                dr.Close();
            }
            return result;
        }

        public string GetSoDonThuByNamTiepNhan(int coQuanID, int namTiepNhan)
        {
            string result = string.Empty;
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARAM_CO_QUAN_ID, SqlDbType.Int),
                new SqlParameter("NamTiepNhan", SqlDbType.Int)
            };
            parameters[0].Value = coQuanID;
            parameters[1].Value = namTiepNhan;
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "NV_XuLyDon_GetSoDonByCoQuanAndNamTiepNhan", parameters))
            {
                if (dr.Read())
                {
                    result = Utils.GetString(dr["SoDonThu"], string.Empty);
                }
                dr.Close();
            }
            return result;
        }

        public string GetSoDonThu_New(int coQuanID)
        {
            string result = string.Empty;
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARAM_CO_QUAN_ID, SqlDbType.Int)
            };
            parameters[0].Value = coQuanID;
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, NV_XULYDON_GETSODONBYCOQUAN_NEW, parameters))
            {
                if (dr.Read())
                {
                    result = Utils.GetString(dr["SoDonThu"], string.Empty);
                }
                dr.Close();
            }
            return result;
        }



        public int GetSTT(int coQuanID)
        {
            int result = 0;
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARAM_CO_QUAN_ID, SqlDbType.Int)
            };
            parameters[0].Value = coQuanID;
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_STT_TIEPDAN_BY_CO_QUAN, parameters))
            {
                if (dr.Read())
                {
                    result = Utils.ConvertToInt32(dr["SoDon"], 0);
                }
                dr.Close();
            }
            return result;
        }

        public int getSoTTLeTan(int coQuanID)
        {
            int result = 0;
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARAM_CO_QUAN_ID, SqlDbType.Int)
            };
            parameters[0].Value = coQuanID;
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_SO_TT_LETAN_BY_CO_QUAN, parameters))
            {
                if (dr.Read())
                {
                    result = Utils.ConvertToInt32(dr["STT"], 0);
                }
                dr.Close();
            }
            return result;
        }

        //check trung don 8/7/2014
        private const string CHECK_TRUNG_DON_BY_HOTEN = @"NV_TiepDan_CheckTrungDonByHoTen";
        public IList<TiepDanInfo> GetDonTrung(string hoTen, string cmnd, string diachi, string noidungdon)
        {
            IList<TiepDanInfo> ls_donthu = new List<TiepDanInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_HOTEN, SqlDbType.NVarChar, 150),
                new SqlParameter(PARAM_DIACHI, SqlDbType.NVarChar, 250),
                new SqlParameter(PARAM_CMND, SqlDbType.NVarChar, 50),
                new SqlParameter(PARAM_NOI_DUNG_DON, SqlDbType.NVarChar, 200)
            };

            parameters[0].Value = hoTen ?? Convert.DBNull;
            parameters[1].Value = diachi ?? Convert.DBNull;
            parameters[2].Value = cmnd ?? Convert.DBNull;
            parameters[3].Value = noidungdon ?? Convert.DBNull;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, CHECK_TRUNG_DON_BY_HOTEN, parameters))
                {
                    while (dr.Read())
                    {
                        TiepDanInfo DTInfo = GetDataByHoTen(dr);
                        //DTInfo.TenCoQuan = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        DTInfo.CoQuanID = Utils.GetInt32(dr["CoQuanID"], 0);
                        DTInfo.TenCoQuan = Utils.GetString(dr["TenCoQuan"], string.Empty);
                        DTInfo.SoLan = new TiepDan().CoundTrungDon(DTInfo.DonThuID ?? 0);
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

        public int CheckSoDonTrung(string hoTen, string cmnd, string diachi, string noidungdon, int? toCao, int? coQuanID)
        {
            //IList<TiepDanInfo> ls_donthu = new List<TiepDanInfo>();
            int dem = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_HOTEN, SqlDbType.NVarChar, 150),
                new SqlParameter(PARAM_DIACHI, SqlDbType.NVarChar, 250),
                new SqlParameter(PARAM_CMND, SqlDbType.NVarChar, 50),
                new SqlParameter(PARAM_NOI_DUNG_DON, SqlDbType.NVarChar, 200),
                new SqlParameter("@ToCao", SqlDbType.Int),
                new SqlParameter("@CoQuanID", SqlDbType.Int)
            };

            parameters[0].Value = hoTen ?? Convert.DBNull;
            parameters[1].Value = diachi ?? Convert.DBNull;
            parameters[2].Value = cmnd ?? Convert.DBNull;
            parameters[3].Value = noidungdon ?? Convert.DBNull;
            parameters[4].Value = toCao ?? Convert.DBNull;
            parameters[5].Value = coQuanID ?? Convert.DBNull;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "NV_TiepDan_CheckSoDonTrung", parameters))
                {
                    if (dr.Read())
                    {
                        dem = Utils.GetInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return dem;
        }

        private TiepDanInfo GetDataByHoTen(SqlDataReader dr)
        {
            TiepDanInfo info = new TiepDanInfo();
            info.DonThuID = Utils.GetInt32(dr["DonThuID"], 0);
            info.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);
            //info.CoQuanID = Utils.GetInt32(dr["CoQuanID"], 0);
            info.LoaiKhieuTo1ID = Utils.ConvertToInt32(dr["LoaiKhieuTo1ID"], 0);
            //info.LanTiep = Utils.GetInt32(dr["LanTiep"], 0);
            info.HoTen = Utils.GetString(dr["HoTen"], string.Empty);
            info.DiaChiCT = Utils.GetString(dr["DiaChi"], string.Empty);
            info.TenLoaiKhieuTo = Utils.GetString(dr["TenLoaiKhieuTo"], string.Empty);
            info.NoiDungDon = Utils.GetString(dr["NoiDungDon"], string.Empty);
            //info.NoiDungHuongDan = Utils.GetString(dr["NoiDungHuongDan"], string.Empty);
            //info.TenHuongGiaiQuyet = Utils.GetString(dr["TenHuongGiaiQuyet"], string.Empty);

            return info;
        }

        public int CoundTrungDon(int donthuID)
        {
            int result = 0;
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARAM_DON_THU_ID, SqlDbType.Int)
            };
            parameters[0].Value = donthuID;
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUND_TRUNGDON_BY_DONTHUID, parameters))
            {
                if (dr.Read())
                {
                    result = Utils.ConvertToInt32(dr["SoLanTrung"], 0);
                }
                dr.Close();
            }
            return result;
        }

        //public int UpdateXuLyDonIDClone(int xuLyDonID, int xuLyDonID_Clone)
        //{

        //    int val = 0;

        //    SqlParameter[] parameters = new SqlParameter[]{
        //        new SqlParameter(PARAM_XULYDON_ID,SqlDbType.Int),
        //        new SqlParameter(PARAM_XULYDONID_CLONE,SqlDbType.Int)
        //    };
        //    parameters[0].Value = xuLyDonID;
        //    parameters[1].Value = xuLyDonID_Clone;

        //    using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
        //    {

        //        conn.Open();
        //        using (SqlTransaction trans = conn.BeginTransaction())
        //        {

        //            try
        //            {
        //                val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, UPDATE_XULYDONID_CLONE, parameters);
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
        //    return val;
        //}

        public IList<TiepDanInfo> GetCTTrungDonByDonID(int donthuID)
        {
            IList<TiepDanInfo> ls_donthu = new List<TiepDanInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_DON_THU_ID,SqlDbType.Int)
            };

            parameters[0].Value = donthuID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_CTTRUNGDON_BY_DONTHU_ID, parameters))
                {
                    while (dr.Read())
                    {
                        TiepDanInfo DTInfo = GetDataCTTrungDon(dr);
                        DTInfo.SoDonThu = Utils.ConvertToString(dr["SoDonThu"], string.Empty);
                        DTInfo.TenCoQuanDaGQ = Utils.ConvertToString(dr["TenCoQuanDaGQ"], string.Empty);
                        DTInfo.StateID = Utils.ConvertToInt32(dr["StateID"], 0);

                        if (DTInfo.TenHuongGiaiQuyet == "" && DTInfo.KetQuaTiep == "")
                        {
                            DTInfo.TenHuongGiaiQuyet = "";
                        }
                        else if (DTInfo.TenHuongGiaiQuyet != "" && DTInfo.KetQuaTiep == "")
                        {
                            DTInfo.TenHuongGiaiQuyet = DTInfo.TenHuongGiaiQuyet;
                        }
                        else
                        {
                            DTInfo.TenHuongGiaiQuyet = DTInfo.TenHuongGiaiQuyet + " : " + DTInfo.KetQuaTiep;
                        }
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

        public IList<TiepDanInfo> GetCTKhieuToLan2ByDonID(int donthuID)
        {
            IList<TiepDanInfo> ls_donthu = new List<TiepDanInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_DON_THU_ID,SqlDbType.Int)
            };

            parameters[0].Value = donthuID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_CTKHIEUTOLAN2_BY_DONTHU_ID, parameters))
                {
                    while (dr.Read())
                    {
                        TiepDanInfo DTInfo = GetDataCTTrungDon(dr);
                        DTInfo.SoDonThu = Utils.ConvertToString(dr["SoDonThu"], string.Empty);
                        DTInfo.TenCoQuanDaGQ = Utils.ConvertToString(dr["TenCoQuanDaGQ"], string.Empty);
                        DTInfo.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
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


        private TiepDanInfo GetDataCTTrungDon(SqlDataReader dr)
        {
            TiepDanInfo info = new TiepDanInfo();
            info.DonThuID = Utils.GetInt32(dr["DonThuID"], 0);
            info.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);
            info.CoQuanID = Utils.GetInt32(dr["CoQuanID"], 0);
            info.LoaiKhieuTo1ID = Utils.ConvertToInt32(dr["LoaiKhieuTo1ID"], 0);
            //info.LanTiep = Utils.GetInt32(dr["LanTiep"], 0);
            info.HoTen = Utils.GetString(dr["HoTen"], string.Empty);
            info.DiaChiCT = Utils.GetString(dr["DiaChi"], string.Empty);
            info.TenLoaiKhieuTo = Utils.GetString(dr["TenLoaiKhieuTo"], string.Empty);
            info.NoiDungDon = Utils.GetString(dr["NoiDungDon"], string.Empty);
            info.NoiDungHuongDan = Utils.GetString(dr["NoiDungHuongDan"], string.Empty);
            info.TenHuongGiaiQuyet = Utils.GetString(dr["TenHuongGiaiQuyet"], string.Empty);
            info.TenCoQuan = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
            info.NgayNhapDon = Utils.ConvertToDateTime(dr["NgayNhapDon"], DateTime.MinValue);
            info.NgayNhapDon_Str = Utils.FormatDate(info.NgayNhapDon.Value);
            info.KetQuaTiep = Utils.ConvertToString(dr["KetQuaTiep"], string.Empty);

            return info;
        }

        public IList<TiepDanInfo> GetKhieuToLan2(string hoTen, string cmnd, string diachi, string noidungdon, int? stateID)
        {
            IList<TiepDanInfo> ls_donthu = new List<TiepDanInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_HOTEN, SqlDbType.NVarChar, 50),
                new SqlParameter(PARAM_DIACHI, SqlDbType.NVarChar, 50),
                new SqlParameter(PARAM_CMND, SqlDbType.NVarChar, 50),
                new SqlParameter(PARAM_NOI_DUNG_DON, SqlDbType.NVarChar, 200),
                new SqlParameter(PARAM_STATEID, SqlDbType.Int),
            };

            parameters[0].Value = hoTen ?? Convert.DBNull;
            parameters[1].Value = diachi ?? Convert.DBNull;
            parameters[2].Value = cmnd ?? Convert.DBNull;
            parameters[3].Value = noidungdon ?? Convert.DBNull;
            parameters[4].Value = stateID ?? Convert.DBNull;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, CHECK_KHIEUTOLAN2_BY_HOTEN, parameters))
                {
                    while (dr.Read())
                    {
                        TiepDanInfo info = new TiepDanInfo();
                        info.DonThuID = Utils.GetInt32(dr["DonThuID"], 0);
                        info.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);
                        info.HoTen = Utils.GetString(dr["HoTen"], string.Empty);
                        info.DiaChiCT = Utils.GetString(dr["DiaChi"], string.Empty);
                        info.TenLoaiKhieuTo = Utils.GetString(dr["TenLoaiKhieuTo"], string.Empty);
                        info.NoiDungDon = Utils.GetString(dr["NoiDungDon"], string.Empty);
                        info.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
                        info.LoaiKhieuTo1ID = Utils.ConvertToInt32(dr["LoaiKhieuTo1ID"], 0);
                        info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        info.HuongGiaiQuyetID = Utils.ConvertToInt32(dr["HuongGiaiQuyetID"], 0);

                        info.LanGQ = CountSoLanGQ(info.DonThuID ?? 0, 10);
                        ls_donthu.Add(info);
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

        public int CountSoLanGQ(int donthuID, int stateID)
        {
            int result = 0;
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARAM_DON_THU_ID, SqlDbType.Int),
                new SqlParameter(PARAM_STATEID, SqlDbType.Int)
            };
            parameters[0].Value = donthuID;
            parameters[1].Value = stateID;
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, CHECK_KHIEUTOLAN2_SOLANGQ, parameters))
            {
                if (dr.Read())
                {
                    result = Utils.ConvertToInt32(dr["LanGQ"], 0);
                }
                dr.Close();
            }
            return result;
        }
        /// <summary>
        /// Insert thành phần tham gia
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int InsertThanhPhanThamGia(List<ThanhPhanThamGiaInfo> info)
        {

            int val = 0;
            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter(PARAM_TIEP_DAN_KHONG_DON_ID, SqlDbType.Int),
            };
            parms[0].Value = info[0].TiepDanKhongDonID;

            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_TIEP_DAN_KHONG_DON_ID, SqlDbType.Int),
                new SqlParameter(PARAM_TEN_CAN_BO, SqlDbType.NVarChar),
                new SqlParameter(PARAM_CHUC_VU, SqlDbType.NVarChar)
            };


            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        //xóa thành phần tham gia cũ trước khi insert mới
                        SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, DELETE_THANHPHANTHAMGIA, parms);

                        foreach (var item in info)
                        {
                            parameters[0].Value = item.TiepDanKhongDonID;
                            parameters[1].Value = item.TenCanBo;
                            parameters[2].Value = item.ChucVu;
                            //insert
                            val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, INSERT_THANHPHANTHAMGIA, parameters);
                        }

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
        /// Get thành phần tham gia theo TiepDanKhongDonID
        /// </summary>
        /// <param name="tiepDanKhongDonID"></param>
        /// <returns></returns>
        public IList<ThanhPhanThamGiaInfo> GetThanhPhanThamGia(int tiepDanKhongDonID)
        {
            IList<ThanhPhanThamGiaInfo> ListTP = new List<ThanhPhanThamGiaInfo>();

            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_TIEP_DAN_KHONG_DON_ID, SqlDbType.Int),
                };
            parameters[0].Value = tiepDanKhongDonID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_THANHPHANTHAMGIA_BY_TIEPDANKHONGDONID, parameters))
                {

                    while (dr.Read())
                    {
                        ThanhPhanThamGiaInfo info = new ThanhPhanThamGiaInfo();
                        info.TiepDanKhongDonID = Utils.ConvertToInt32(dr["TiepDanKhongDonID"], 0);
                        info.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        info.ChucVu = Utils.ConvertToString(dr["ChucVu"], string.Empty);
                        ListTP.Add(info);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return ListTP;
        }

        public string SoDonThu_getByTiepDanID(int tiepDanKhongDonID)
        {
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_TIEP_DAN_KHONG_DON_ID, SqlDbType.Int),
                };
            parameters[0].Value = tiepDanKhongDonID;
            string SoDonThu = "";
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, SODONTHU_GETBYTIEPDANID, parameters))
                {

                    while (dr.Read())
                    {
                        SoDonThu = Utils.ConvertToString(dr["SoDonThu"], string.Empty);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return SoDonThu;
        }

        public void FillTiepDanData(string path, TiepDanInfo TiepDanInfo)
        {
            Application app = null;
            Document doc = null;
            object Missing = System.Reflection.Missing.Value;
            object outfilename = path + "123";
            try
            {
                app = new Microsoft.Office.Interop.Word.Application();
                doc = app.Documents.Open(path, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing);
                FindReplaceAnywhere(app, "DAY", DateTime.Now.Day.ToString());
                FindReplaceAnywhere(app, "MONTH", DateTime.Now.Month.ToString());
                FindReplaceAnywhere(app, "YEAR", DateTime.Now.Year.ToString());
                //doc.SaveAs(outfilename, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing);
            }
            finally
            {
                try
                {
                    if (doc != null) ((Microsoft.Office.Interop.Word._Document)doc).Close(true, Missing, Missing);
                }
                finally { }
                if (app != null) ((Microsoft.Office.Interop.Word._Application)app).Quit(true, Missing, Missing);
            }
            //doc.ExportAsFixedFormat(path, Microsoft.Office.Interop.Word.WdExportFormat.wdExportFormatPDF);
            //System.IO.FileStream fs = new System.IO.FileStream(doc, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            //System.IO.MemoryStream ms = new System.IO.MemoryStream();
            //fs.CopyTo(ms);

            MemoryStream stream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, doc);

        }
        private void FindAndReplace(Microsoft.Office.Interop.Word.Application doc, object findText, object replaceWithText)
        {
            //options
            object matchCase = false;
            object matchWholeWord = true;
            object matchWildCards = false;
            object matchSoundsLike = false;
            object matchAllWordForms = false;
            object forward = true;
            object format = false;
            object matchKashida = false;
            object matchDiacritics = false;
            object matchAlefHamza = false;
            object matchControl = false;
            object read_only = false;
            object visible = true;
            object replace = 2;
            object wrap = 1;
            //execute find and replace
            doc.Selection.Find.Execute(ref findText, ref matchCase, ref matchWholeWord,
                ref matchWildCards, ref matchSoundsLike, ref matchAllWordForms, ref forward, ref wrap, ref format, ref replaceWithText, ref replace,
                ref matchKashida, ref matchDiacritics, ref matchAlefHamza, ref matchControl);
        }

        public static void FindReplaceAnywhere(Microsoft.Office.Interop.Word.Application app, string findText, string replaceText)
        {
            var doc = app.ActiveDocument;
            Microsoft.Office.Interop.Word.WdStoryType lngJunk = doc.Sections[1].Headers[WdHeaderFooterIndex.wdHeaderFooterPrimary].Range.StoryType;
            // Iterate through all story types in the current document
            foreach (Microsoft.Office.Interop.Word.Range rngStory in doc.StoryRanges)
            {
                // Iterate through all linked stories
                var internalRangeStory = rngStory;

                do
                {
                    searchAndReplaceInStory(internalRangeStory, findText, replaceText);
                    try
                    {
                        switch (internalRangeStory.StoryType)
                        {
                            case Microsoft.Office.Interop.Word.WdStoryType.wdEvenPagesHeaderStory: // 6
                            case Microsoft.Office.Interop.Word.WdStoryType.wdPrimaryHeaderStory:   // 7
                            case Microsoft.Office.Interop.Word.WdStoryType.wdEvenPagesFooterStory: // 8
                            case Microsoft.Office.Interop.Word.WdStoryType.wdPrimaryFooterStory:   // 9
                            case Microsoft.Office.Interop.Word.WdStoryType.wdFirstPageHeaderStory: // 10
                            case Microsoft.Office.Interop.Word.WdStoryType.wdFirstPageFooterStory: // 11

                                if (internalRangeStory.ShapeRange.Count > 0)
                                {
                                    foreach (Microsoft.Office.Interop.Word.Shape oShp in internalRangeStory.ShapeRange)
                                    {
                                        if (oShp.TextFrame.HasText != 0)
                                        {
                                            searchAndReplaceInStory(oShp.TextFrame.TextRange, findText, replaceText);
                                        }
                                    }
                                }
                                break;

                            default:
                                break;
                        }
                    }
                    catch
                    {
                        // On Error Resume Next
                    }
                    internalRangeStory = internalRangeStory.NextStoryRange;
                } while (internalRangeStory != null);
            }

        }

        private static void searchAndReplaceInStory(Microsoft.Office.Interop.Word.Range rngStory, string strSearch, string strReplace)
        {
            object Missing = System.Reflection.Missing.Value;

            rngStory.Find.ClearFormatting();
            rngStory.Find.Replacement.ClearFormatting();
            rngStory.Find.Text = strSearch;
            rngStory.Find.Replacement.Text = strReplace;
            rngStory.Find.Wrap = WdFindWrap.wdFindContinue;

            object arg1 = Missing; // Find Pattern
            object arg2 = Missing; //MatchCase
            object arg3 = Missing; //MatchWholeWord
            object arg4 = Missing; //MatchWildcards
            object arg5 = Missing; //MatchSoundsLike
            object arg6 = Missing; //MatchAllWordForms
            object arg7 = Missing; //Forward
            object arg8 = Missing; //Wrap
            object arg9 = Missing; //Format
            object arg10 = Missing; //ReplaceWith
            object arg11 = WdReplace.wdReplaceAll; //Replace
            object arg12 = Missing; //MatchKashida
            object arg13 = Missing; //MatchDiacritics
            object arg14 = Missing; //MatchAlefHamza
            object arg15 = Missing; //MatchControl

            rngStory.Find.Execute(ref arg1, ref arg2, ref arg3, ref arg4, ref arg5, ref arg6, ref arg7, ref arg8, ref arg9, ref arg10, ref arg11, ref arg12, ref arg13, ref arg14, ref arg15);
        }

        public int InsertDanKhongDen(TiepCongDan_DanKhongDenInfo dkdInfo)
        {

            object val = null;

            //var dkdInfoTemp = GetTiepDan_DanKhongDenByNgayTrucVaNguoiTao(dkdInfo.NguoiTaoID, dkdInfo.NgayTruc);
            if (dkdInfo != null && dkdInfo.DanKhongDenID > 0)
            {
                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SqlParameter[] param = new SqlParameter[]{
                    new SqlParameter("CanBoID", SqlDbType.Int),
                    new SqlParameter("TenCanBo", SqlDbType.NVarChar),
                    new SqlParameter("NguoiTaoID", SqlDbType.Int),
                    new SqlParameter("NgayTruc", SqlDbType.DateTime),
                    new SqlParameter("DanKhongDenID", SqlDbType.Int),
                    new SqlParameter("ChucVu", SqlDbType.NVarChar),
                    };
                    param[0].Value = dkdInfo.CanBoID ?? Convert.DBNull;
                    param[1].Value = dkdInfo.TenCanBo ?? Convert.DBNull;
                    param[2].Value = dkdInfo.NguoiTaoID ?? Convert.DBNull;
                    param[3].Value = dkdInfo.NgayTruc ?? DateTime.Now;
                    param[4].Value = dkdInfo.DanKhongDenID;
                    param[5].Value = dkdInfo.ChucVu ?? Convert.DBNull;
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {

                        try
                        {
                            val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "TiepDan_DanKhongDen_Update", param);
                            trans.Commit();
                            if (Utils.ConvertToInt32(val, 0) > 0) val = dkdInfo.DanKhongDenID;
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
            else
            {
                SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("CanBoID", SqlDbType.Int),
                new SqlParameter("TenCanBo", SqlDbType.NVarChar),
                new SqlParameter("NguoiTaoID", SqlDbType.Int),
                new SqlParameter("NgayTruc", SqlDbType.DateTime),
                 new SqlParameter("ChucVu", SqlDbType.NVarChar),
                };
                parameters[0].Value = dkdInfo.CanBoID ?? Convert.DBNull;
                parameters[1].Value = dkdInfo.TenCanBo ?? Convert.DBNull;
                parameters[2].Value = dkdInfo.NguoiTaoID ?? Convert.DBNull;
                parameters[3].Value = dkdInfo.NgayTruc ?? DateTime.Now;
                parameters[4].Value = dkdInfo.ChucVu ?? Convert.DBNull;
                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {

                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {

                        try
                        {
                            val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, "TiepDan_DanKhongDen_Insert", parameters);
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
            }

            return Utils.ConvertToInt32(val, 0);
        }

        public TiepCongDan_DanKhongDenInfo GetTiepDan_DanKhongDenByNgayTrucVaNguoiTao(int? NguoiTaoID, DateTime? NgayTruc)
        {

            TiepCongDan_DanKhongDenInfo dkdInfo = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("NgayTruc",SqlDbType.DateTime),
                new SqlParameter("NguoiTaoID",SqlDbType.Int),
            };
            parameters[0].Value = NgayTruc;
            parameters[1].Value = NguoiTaoID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "TiepDan_DanKhongDen_GetByNgayTrucVanguoiTao", parameters))
                {

                    if (dr.Read())
                    {
                        dkdInfo = new TiepCongDan_DanKhongDenInfo();
                        dkdInfo.DanKhongDenID = Utils.ConvertToInt32(dr["DanKhongDenID"], 0);
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return dkdInfo;
        }

        public List<TiepCongDan_DanKhongDenInfo> GetPagingTiepCongDan_DanKhongDen(int start, int end, string TenCanBo, DateTime? TuNgay, DateTime? DenNgay, int? CoQuanID)
        {
            List<TiepCongDan_DanKhongDenInfo> list = new List<TiepCongDan_DanKhongDenInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("start",SqlDbType.Int),
                new SqlParameter("end",SqlDbType.Int),
                new SqlParameter("TenCanBo",SqlDbType.NVarChar),
                new SqlParameter("TuNgay",SqlDbType.DateTime),
                new SqlParameter("DenNgay",SqlDbType.DateTime),
                new SqlParameter("CoQuanID",SqlDbType.Int),
            };
            parameters[0].Value = start;
            parameters[1].Value = end;
            parameters[2].Value = TenCanBo;
            parameters[3].Value = TuNgay ?? Convert.DBNull;
            parameters[4].Value = DenNgay ?? Convert.DBNull;
            parameters[5].Value = CoQuanID ?? Convert.DBNull;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "TiepDan_DanKhongDen_GetListPaging", parameters))
                {

                    while (dr.Read())
                    {
                        TiepCongDan_DanKhongDenInfo dkdInfo = new TiepCongDan_DanKhongDenInfo();
                        dkdInfo.DanKhongDenID = Utils.ConvertToInt32(dr["DanKhongDenID"], 0);
                        dkdInfo.NguoiTaoID = Utils.ConvertToInt32(dr["NguoiTaoID"], 0);
                        dkdInfo.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        dkdInfo.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], String.Empty);
                        dkdInfo.ChucVu = Utils.ConvertToString(dr["ChucVu"], String.Empty);
                        dkdInfo.NgayTruc = Utils.ConvertToDateTime(dr["NgayTruc"], DateTime.MinValue);
                        if (dkdInfo.NgayTruc != DateTime.MinValue)
                        {
                            dkdInfo.NgayTrucStr = dkdInfo.NgayTruc.Value.ToString("dd/MM/yyyy");
                        }
                        list.Add(dkdInfo);
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

        public int CountListPaging(string TenCanBo, DateTime? TuNgay, DateTime? DenNgay)
        {
            int Count = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("TenCanBo",SqlDbType.NVarChar),
                new SqlParameter("TuNgay",SqlDbType.DateTime),
                new SqlParameter("DenNgay",SqlDbType.DateTime),
            };
            parameters[0].Value = TenCanBo;
            parameters[1].Value = TuNgay ?? Convert.DBNull;
            parameters[2].Value = DenNgay ?? Convert.DBNull;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "TiepDan_DanKhongDen_CountListPaging", parameters))
                {

                    if (dr.Read())
                    {
                        Count = Utils.ConvertToInt32(dr["Count"], 0);
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return Count;
        }

        public TiepCongDan_DanKhongDenInfo GetTiepDanDanKhongDen(int danKhongDenID)
        {

            TiepCongDan_DanKhongDenInfo DTInfo = new TiepCongDan_DanKhongDenInfo();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("DanKhongDenID",SqlDbType.Int)
            };
            parameters[0].Value = danKhongDenID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "NVTiepDan_GetByDanKhongDenID", parameters))
                {

                    if (dr.Read())
                    {
                        DTInfo.DanKhongDenID = Utils.ConvertToInt32(dr["DanKhongDenID"], 0);
                        DTInfo.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        DTInfo.ChucVu = Utils.ConvertToString(dr["ChucVu"], string.Empty);
                        DTInfo.NguoiTaoID = Utils.ConvertToInt32(dr["NguoiTaoID"], 0);
                        DTInfo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        DTInfo.NgayTruc = Utils.ConvertToDateTime(dr["NgayTruc"], DateTime.Now);
                        DTInfo.NgayTrucStr = DTInfo.NgayTruc.Value.ToString("dd/MM/yyyy");
                        DTInfo.ChucVu = Utils.ConvertToString(dr["ChucVu"], string.Empty);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return DTInfo;
        }

        public int UpdateDanKhongDen(TiepCongDan_DanKhongDenInfo dkdInfo)
        {

            object val = null;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                SqlParameter[] param = new SqlParameter[]{
                    new SqlParameter("CanBoID", SqlDbType.Int),
                    new SqlParameter("TenCanBo", SqlDbType.NVarChar),
                    new SqlParameter("NguoiTaoID", SqlDbType.Int),
                    new SqlParameter("DanKhongDenID", SqlDbType.Int),
                    new SqlParameter("ChucVu", SqlDbType.NVarChar),
                    new SqlParameter("NgayTruc", SqlDbType.DateTime),
                    };
                param[0].Value = dkdInfo.CanBoID ?? Convert.DBNull;
                param[1].Value = dkdInfo.TenCanBo ?? Convert.DBNull;
                param[2].Value = dkdInfo.NguoiTaoID ?? Convert.DBNull;
                param[3].Value = dkdInfo.DanKhongDenID;
                param[4].Value = dkdInfo.ChucVu ?? Convert.DBNull;
                param[5].Value = dkdInfo.NgayTruc ?? DateTime.Now;
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "TiepDan_DanKhongDen_SoTiepDan_Update", param);
                        trans.Commit();
                        if (Utils.ConvertToInt32(val, 0) > 0) val = dkdInfo.DanKhongDenID;
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

        public int DeleteDanKhongDen(int DanKhongDenID)
        {

            object val = null;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                SqlParameter[] param = new SqlParameter[]{
                    new SqlParameter("DanKhongDenID", SqlDbType.Int),
                    };
                param[0].Value = DanKhongDenID;
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "TiepDan_DanKhongDen_Delete", param);
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

        public int InsertTiepDanDinhKy(TiepDanDinhKyModel tdInfo)
        {

            int TiepDinhKyID = 0;
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("NgayTiep", SqlDbType.DateTime),
                new SqlParameter("LanhDaoTiep", SqlDbType.Int),
                new SqlParameter("ChucVu", SqlDbType.NVarChar),
                new SqlParameter("NoiDungTiep", SqlDbType.NVarChar),
                new SqlParameter("KetQuaTiep", SqlDbType.NVarChar),
                new SqlParameter("KetQuaGQCacNganh", SqlDbType.NVarChar),
                new SqlParameter("UyQuyenTiep", SqlDbType.Int),
                new SqlParameter("LoaiTiepDanID", SqlDbType.Int),
                new SqlParameter("CoQuanID", SqlDbType.Int),
                new SqlParameter("NhomKNID", SqlDbType.Int),
                new SqlParameter("TenLanhDaoTiep", SqlDbType.NVarChar),
            };

            parms[0].Value = tdInfo.NgayTiep == null ? Convert.DBNull : tdInfo.NgayTiep.Value.Date;
            parms[1].Value = tdInfo.LanhDaoTiep ?? Convert.DBNull;
            parms[2].Value = tdInfo.ChucVu ?? Convert.DBNull;
            parms[3].Value = tdInfo.NoiDungTiep ?? Convert.DBNull;
            parms[4].Value = tdInfo.KetQuaTiep ?? Convert.DBNull;
            parms[5].Value = tdInfo.KetQuaGQCacNganh ?? Convert.DBNull;
            parms[6].Value = tdInfo.UyQuyenTiep ?? Convert.DBNull;
            parms[7].Value = tdInfo.LoaiTiepDanID ?? Convert.DBNull;
            parms[8].Value = tdInfo.CoQuanID ?? Convert.DBNull;
            parms[9].Value = tdInfo.NhomKNID ?? Convert.DBNull;
            parms[10].Value = tdInfo.TenLanhDaoTiep ?? Convert.DBNull;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        TiepDinhKyID = Utils.ConvertToInt32(SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, "v2_NV_TiepDanDinhKy_Insert", parms), 0);
                        if (TiepDinhKyID > 0 && tdInfo.ThanhPhanThamGia != null && tdInfo.ThanhPhanThamGia.Count > 0)
                        {
                            foreach (var item in tdInfo.ThanhPhanThamGia)
                            {
                                SqlParameter[] parameters = new SqlParameter[]{
                                    new SqlParameter("TiepDinhKyID", SqlDbType.Int),
                                    new SqlParameter("TenCanBo", SqlDbType.NVarChar),
                                    new SqlParameter("ChucVu", SqlDbType.NVarChar)
                                };

                                parameters[0].Value = TiepDinhKyID;
                                parameters[1].Value = item.TenCanBo ?? Convert.DBNull;
                                parameters[2].Value = item.ChucVu ?? Convert.DBNull;
                                //insert
                                SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "v2_NV_ThanhPhanThamGiaTDDinhKy_Insert", parameters);
                            }
                        }

                        if (TiepDinhKyID > 0 && tdInfo.Children != null && tdInfo.Children.Count > 0)
                        {
                            foreach (var item in tdInfo.Children)
                            {
                                SqlParameter[] parms_td = new SqlParameter[] {
                                    new SqlParameter("TiepDinhKyID", SqlDbType.Int),
                                    new SqlParameter("NoiDungTiep", SqlDbType.NVarChar),
                                    new SqlParameter("KetQuaTiep", SqlDbType.NVarChar),
                                    new SqlParameter("KetQuaGQCacNganh", SqlDbType.NVarChar),
                                    new SqlParameter("UyQuyenTiep", SqlDbType.Int),
                                    new SqlParameter("NhomKNID", SqlDbType.Int),
                                };

                                parms_td[0].Value = TiepDinhKyID;
                                parms_td[1].Value = item.NoiDungTiep ?? Convert.DBNull;
                                parms_td[2].Value = item.KetQuaTiep ?? Convert.DBNull;
                                parms_td[3].Value = item.KetQuaGQCacNganh ?? Convert.DBNull;
                                parms_td[4].Value = item.UyQuyenTiep ?? Convert.DBNull;
                                parms_td[5].Value = item.NhomKNID ?? Convert.DBNull;
                                //insert
                                SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "v2_NV_CTTiepDanDinhKy_Insert", parms_td);
                            }
                        }

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
            return TiepDinhKyID;
        }

        public int UpdateTiepDanDinhKy(TiepDanDinhKyModel tdInfo)
        {

            int val = 0;
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("NgayTiep", SqlDbType.DateTime),
                new SqlParameter("LanhDaoTiep", SqlDbType.Int),
                new SqlParameter("ChucVu", SqlDbType.NVarChar),
                new SqlParameter("NoiDungTiep", SqlDbType.NVarChar),
                new SqlParameter("KetQuaTiep", SqlDbType.NVarChar),
                new SqlParameter("KetQuaGQCacNganh", SqlDbType.NVarChar),
                new SqlParameter("UyQuyenTiep", SqlDbType.Int),
                new SqlParameter("LoaiTiepDanID", SqlDbType.Int),
                new SqlParameter("TiepDinhKyID", SqlDbType.Int),
                new SqlParameter("TenLanhDaoTiep", SqlDbType.NVarChar),
            };

            parms[0].Value = tdInfo.NgayTiep == null ? Convert.DBNull : tdInfo.NgayTiep.Value.Date;
            parms[1].Value = tdInfo.LanhDaoTiep ?? Convert.DBNull;
            parms[2].Value = tdInfo.ChucVu ?? Convert.DBNull;
            parms[3].Value = tdInfo.NoiDungTiep ?? Convert.DBNull;
            parms[4].Value = tdInfo.KetQuaTiep ?? Convert.DBNull;
            parms[5].Value = tdInfo.KetQuaGQCacNganh ?? Convert.DBNull;
            parms[6].Value = tdInfo.UyQuyenTiep ?? Convert.DBNull;
            parms[7].Value = tdInfo.LoaiTiepDanID ?? Convert.DBNull;
            parms[8].Value = tdInfo.TiepDinhKyID;
            parms[9].Value = tdInfo.TenLanhDaoTiep ?? Convert.DBNull;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "v2_NV_TiepDanDinhKy_Update", parms);
                        if (val > 0 && tdInfo.TiepDinhKyID > 0)
                        {
                            SqlParameter[] parms_del = new SqlParameter[]{
                                new SqlParameter("TiepDinhKyID", SqlDbType.Int),
                            };
                            parms_del[0].Value = tdInfo.TiepDinhKyID;
                            SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "v2_NV_ThanhPhanThamGiaTDDinhKy_Delete", parms_del);

                            if (tdInfo.ThanhPhanThamGia != null && tdInfo.ThanhPhanThamGia.Count > 0)
                            {
                                foreach (var item in tdInfo.ThanhPhanThamGia)
                                {
                                    SqlParameter[] parameters = new SqlParameter[]{
                                    new SqlParameter("TiepDinhKyID", SqlDbType.Int),
                                    new SqlParameter("TenCanBo", SqlDbType.NVarChar),
                                    new SqlParameter("ChucVu", SqlDbType.NVarChar)
                                };

                                    parameters[0].Value = item.TiepDinhKyID ?? Convert.DBNull;
                                    parameters[1].Value = item.TenCanBo ?? Convert.DBNull;
                                    parameters[2].Value = item.ChucVu ?? Convert.DBNull;
                                    //insert
                                    val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "v2_NV_ThanhPhanThamGiaTDDinhKy_Insert", parameters);
                                }
                            }

                            if (tdInfo.Children != null && tdInfo.Children.Count > 0)
                            {
                                foreach (var item in tdInfo.Children)
                                {
                                    if (item.CTTiepDinhKyID > 0)
                                    {
                                        SqlParameter[] parms_td = new SqlParameter[] {
                                            new SqlParameter("CTTiepDinhKyID", SqlDbType.Int),
                                            new SqlParameter("NoiDungTiep", SqlDbType.NVarChar),
                                            new SqlParameter("KetQuaTiep", SqlDbType.NVarChar),
                                            new SqlParameter("KetQuaGQCacNganh", SqlDbType.NVarChar),
                                            new SqlParameter("UyQuyenTiep", SqlDbType.Int),
                                            new SqlParameter("NhomKNID", SqlDbType.Int),
                                        };

                                        parms_td[0].Value = item.CTTiepDinhKyID;
                                        parms_td[1].Value = item.NoiDungTiep ?? Convert.DBNull;
                                        parms_td[2].Value = item.KetQuaTiep ?? Convert.DBNull;
                                        parms_td[3].Value = item.KetQuaGQCacNganh ?? Convert.DBNull;
                                        parms_td[4].Value = item.UyQuyenTiep ?? Convert.DBNull;
                                        parms_td[5].Value = item.NhomKNID ?? Convert.DBNull;
                                        //insert
                                        SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "v2_NV_CTTiepDanDinhKy_Update", parms_td);
                                    }
                                    else
                                    {
                                        SqlParameter[] parms_td = new SqlParameter[] {
                                            new SqlParameter("TiepDinhKyID", SqlDbType.Int),
                                            new SqlParameter("NoiDungTiep", SqlDbType.NVarChar),
                                            new SqlParameter("KetQuaTiep", SqlDbType.NVarChar),
                                            new SqlParameter("KetQuaGQCacNganh", SqlDbType.NVarChar),
                                            new SqlParameter("UyQuyenTiep", SqlDbType.Int),
                                            new SqlParameter("NhomKNID", SqlDbType.Int),
                                        };

                                        parms_td[0].Value = tdInfo.TiepDinhKyID;
                                        parms_td[1].Value = item.NoiDungTiep ?? Convert.DBNull;
                                        parms_td[2].Value = item.KetQuaTiep ?? Convert.DBNull;
                                        parms_td[3].Value = item.KetQuaGQCacNganh ?? Convert.DBNull;
                                        parms_td[4].Value = item.UyQuyenTiep ?? Convert.DBNull;
                                        parms_td[5].Value = item.NhomKNID ?? Convert.DBNull;
                                        //insert
                                        SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "v2_NV_CTTiepDanDinhKy_Insert", parms_td);
                                    }
                                }
                            }

                        }
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

        public IList<TiepDanDinhKyModel> GetBySearch(ref int TotalRow, TiepDanParamsForFilter p)
        {
            IList<TiepDanDinhKyModel> Result = new List<TiepDanDinhKyModel>();
            IList<ChiTietTiepDanDinhKyModel> tiepdaninfo = new List<ChiTietTiepDanDinhKyModel>();

            SqlParameter[] parameters = new SqlParameter[]
            {
              new SqlParameter("@Keyword",SqlDbType.NVarChar),
              new SqlParameter("@OrderByName",SqlDbType.NVarChar),
              new SqlParameter("@OrderByOption",SqlDbType.NVarChar),
              new SqlParameter("@pLimit",SqlDbType.Int),
              new SqlParameter("@pOffset",SqlDbType.Int),
              new SqlParameter("@TotalRow",SqlDbType.Int),
              new SqlParameter("@CoQuanID",SqlDbType.Int),
              new SqlParameter("@LoaiTiepDanID",SqlDbType.Int),
            };
            parameters[0].Value = p.Keyword == null ? "" : p.Keyword.Trim();
            parameters[1].Value = p.OrderByName;
            parameters[2].Value = p.OrderByOption;
            parameters[3].Value = p.Limit;
            parameters[4].Value = p.Offset;
            parameters[5].Direction = ParameterDirection.Output;
            parameters[5].Size = 8;
            parameters[6].Value = p.CoQuanID ?? Convert.DBNull;
            parameters[7].Value = p.LoaiTiepDanID ?? Convert.DBNull;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_NV_TiepDanDinhKy_GetBySearch", parameters))
                {
                    while (dr.Read())
                    {
                        ChiTietTiepDanDinhKyModel Info = new ChiTietTiepDanDinhKyModel();
                        Info.TiepDinhKyID = Utils.ConvertToInt32(dr["TiepDinhKyID"], 0);
                        Info.NgayTiep = Utils.ConvertToNullableDateTime(dr["NgayTiep"], null);
                        Info.LanhDaoTiep = Utils.ConvertToInt32(dr["LanhDaoTiep"], 0);
                        Info.UyQuyenTiep = Utils.ConvertToInt32(dr["UyQuyenTiep"], 0);
                        Info.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"], 0);
                        Info.LoaiTiepDanID = Utils.ConvertToInt32(dr["LoaiTiepDanID"], 0);
                        Info.ChucVu = Utils.ConvertToString(dr["ChucVu"], string.Empty);
                        Info.NoiDungTiep = Utils.ConvertToString(dr["NoiDungTiep"], string.Empty);
                        Info.KetQuaTiep = Utils.ConvertToString(dr["KetQuaTiep"], string.Empty);
                        Info.KetQuaGQCacNganh = Utils.ConvertToString(dr["KetQuaGQCacNganh"], string.Empty);
                        Info.TenLanhDaoTiep = Utils.ConvertToString(dr["TenLanhDaoTiep"], string.Empty);
                        Info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        //thanh phan tham gia
                        Info.ID = Utils.ConvertToInt32(dr["ID"], 0);
                        Info.ChucVuThanhPhanThamGia = Utils.ConvertToString(dr["ChucVuThanhPhanThamGia"], string.Empty);
                        Info.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        if (Info.NhomKNID > 0)
                        {
                            Info.NhomKN = new NhomKN().GetByID(Info.NhomKNID.Value);
                            if (Info.NhomKN != null)
                            {
                                Info.NhomKN.DanhSachDoiTuongKN = new DoiTuongKN().GetByNhomKNID(Info.NhomKNID.Value).ToList();
                            }
                        }
                        tiepdaninfo.Add(Info);
                    }
                    dr.Close();
                }
                TotalRow = Utils.ConvertToInt32(parameters[5].Value, 0);
            }
            catch
            {

                throw;
            }

            var list = tiepdaninfo.GroupBy(p => p.TiepDinhKyID)
                   .Select(g => new TiepDanDinhKyModel
                   {
                       TiepDinhKyID = g.Key,
                       NgayTiep = g.FirstOrDefault().NgayTiep,
                       LanhDaoTiep = g.FirstOrDefault().LanhDaoTiep,
                       TenLanhDaoTiep = g.FirstOrDefault().TenLanhDaoTiep,
                       UyQuyenTiep = g.FirstOrDefault().UyQuyenTiep,
                       NhomKNID = g.FirstOrDefault().NhomKNID,
                       LoaiTiepDanID = g.FirstOrDefault().LoaiTiepDanID,
                       ChucVu = g.FirstOrDefault().ChucVu,
                       NoiDungTiep = g.FirstOrDefault().NoiDungTiep,
                       KetQuaTiep = g.FirstOrDefault().KetQuaTiep,
                       KetQuaGQCacNganh = g.FirstOrDefault().KetQuaGQCacNganh,
                       CoQuanID = g.FirstOrDefault().CoQuanID,
                       NhomKN = g.FirstOrDefault().NhomKN,
                       ThanhPhanThamGia = tiepdaninfo.Where(x => x.TiepDinhKyID == g.Key && x.ID > 0).GroupBy(x => x.ID)
                                       .Select(y => new ThanhPhanThamGiaTĐinhKyInfo
                                       {
                                           ID = y.FirstOrDefault().ID,
                                           TiepDinhKyID = g.Key,
                                           TenCanBo = y.FirstOrDefault().TenCanBo,
                                           ChucVu = y.FirstOrDefault().ChucVuThanhPhanThamGia,
                                       }
                                       ).ToList(),
                   }
                   ).ToList();

            Result = list.GroupBy(p => p.NgayTiep)
                   .Select(g => new TiepDanDinhKyModel
                   {
                       NgayTiep = g.FirstOrDefault().NgayTiep,
                       LanhDaoTiep = g.FirstOrDefault().LanhDaoTiep,
                       TenLanhDaoTiep = g.FirstOrDefault().TenLanhDaoTiep,
                       ThanhPhanThamGia = g.FirstOrDefault().ThanhPhanThamGia,
                       Children = list.Where(x => x.NgayTiep == g.Key && x.LanhDaoTiep == g.FirstOrDefault().LanhDaoTiep).ToList()
                   }
                   ).ToList();
            return Result;
        }

        public IList<TiepDanDinhKyModel> GetBySearch_New(ref int TotalRow, TiepDanParamsForFilter p)
        {
            IList<TiepDanDinhKyModel> Result = new List<TiepDanDinhKyModel>();
            IList<ChiTietTiepDanDinhKyModel> tiepdaninfo = new List<ChiTietTiepDanDinhKyModel>();

            SqlParameter[] parameters = new SqlParameter[]
            {
              new SqlParameter("@Keyword",SqlDbType.NVarChar),
              new SqlParameter("@OrderByName",SqlDbType.NVarChar),
              new SqlParameter("@OrderByOption",SqlDbType.NVarChar),
              new SqlParameter("@pLimit",SqlDbType.Int),
              new SqlParameter("@pOffset",SqlDbType.Int),
              new SqlParameter("@TotalRow",SqlDbType.Int),
              new SqlParameter("@CoQuanID",SqlDbType.Int),
              new SqlParameter("@LoaiTiepDanID",SqlDbType.Int),
            };
            parameters[0].Value = p.Keyword == null ? "" : p.Keyword.Trim();
            parameters[1].Value = p.OrderByName;
            parameters[2].Value = p.OrderByOption;
            parameters[3].Value = p.Limit;
            parameters[4].Value = p.Offset;
            parameters[5].Direction = ParameterDirection.Output;
            parameters[5].Size = 8;
            parameters[6].Value = p.CoQuanID ?? Convert.DBNull;
            parameters[7].Value = p.LoaiTiepDanID ?? Convert.DBNull;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_NV_TiepDanDinhKy_GetBySearch_New", parameters))
                {
                    while (dr.Read())
                    {
                        ChiTietTiepDanDinhKyModel Info = new ChiTietTiepDanDinhKyModel();
                        Info.TiepDinhKyID = Utils.ConvertToInt32(dr["TiepDinhKyID"], 0);
                        Info.NgayTiep = Utils.ConvertToNullableDateTime(dr["NgayTiep"], null);
                        Info.LanhDaoTiep = Utils.ConvertToInt32(dr["LanhDaoTiep"], 0);
                        Info.UyQuyenTiep = Utils.ConvertToInt32(dr["UyQuyenTiep"], 0);
                        Info.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"], 0);
                        Info.LoaiTiepDanID = Utils.ConvertToInt32(dr["LoaiTiepDanID"], 0);
                        Info.ChucVu = Utils.ConvertToString(dr["ChucVu"], string.Empty);
                        Info.NoiDungTiep = Utils.ConvertToString(dr["NoiDungTiep"], string.Empty);
                        Info.KetQuaTiep = Utils.ConvertToString(dr["KetQuaTiep"], string.Empty);
                        Info.KetQuaGQCacNganh = Utils.ConvertToString(dr["KetQuaGQCacNganh"], string.Empty);
                        Info.TenLanhDaoTiep = Utils.ConvertToString(dr["TenLanhDaoTiep"], string.Empty);
                        Info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        //thanh phan tham gia
                        Info.ID = Utils.ConvertToInt32(dr["ID"], 0);
                        Info.ChucVuThanhPhanThamGia = Utils.ConvertToString(dr["ChucVuThanhPhanThamGia"], string.Empty);
                        Info.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        if (Info.NhomKNID > 0)
                        {
                            Info.NhomKN = new NhomKN().GetByID(Info.NhomKNID.Value);
                            if (Info.NhomKN != null)
                            {
                                Info.NhomKN.DanhSachDoiTuongKN = new DoiTuongKN().GetByNhomKNID(Info.NhomKNID.Value).ToList();
                            }
                        }
                        tiepdaninfo.Add(Info);
                    }
                    dr.Close();
                }
                TotalRow = Utils.ConvertToInt32(parameters[5].Value, 0);
            }
            catch
            {

                throw;
            }

            Result = tiepdaninfo.GroupBy(p => p.TiepDinhKyID)
                   .Select(g => new TiepDanDinhKyModel
                   {
                       TiepDinhKyID = g.Key,
                       NgayTiep = g.FirstOrDefault().NgayTiep,
                       LanhDaoTiep = g.FirstOrDefault().LanhDaoTiep,
                       TenLanhDaoTiep = g.FirstOrDefault().TenLanhDaoTiep,
                       UyQuyenTiep = g.FirstOrDefault().UyQuyenTiep,
                       NhomKNID = g.FirstOrDefault().NhomKNID,
                       LoaiTiepDanID = g.FirstOrDefault().LoaiTiepDanID,
                       ChucVu = g.FirstOrDefault().ChucVu,
                       NoiDungTiep = g.FirstOrDefault().NoiDungTiep,
                       KetQuaTiep = g.FirstOrDefault().KetQuaTiep,
                       KetQuaGQCacNganh = g.FirstOrDefault().KetQuaGQCacNganh,
                       CoQuanID = g.FirstOrDefault().CoQuanID,
                       NhomKN = g.FirstOrDefault().NhomKN,
                       ThanhPhanThamGia = tiepdaninfo.Where(x => x.TiepDinhKyID == g.Key && x.ID > 0).GroupBy(x => x.ID)
                                       .Select(y => new ThanhPhanThamGiaTĐinhKyInfo
                                       {
                                           ID = y.FirstOrDefault().ID,
                                           TiepDinhKyID = g.Key,
                                           TenCanBo = y.FirstOrDefault().TenCanBo,
                                           ChucVu = y.FirstOrDefault().ChucVuThanhPhanThamGia,
                                       }
                                       ).ToList(),
                   }
                   ).ToList();

            foreach (var item in Result)
            {
                item.Children = GetThongTinVuViec(item.TiepDinhKyID).ToList();
            }
            return Result;
        }


        public TiepDanDinhKyModel GetByID(int TiepDinhKyID)
        {
            IList<TiepDanDinhKyModel> Result = new List<TiepDanDinhKyModel>();
            IList<ChiTietTiepDanDinhKyModel> tiepdaninfo = new List<ChiTietTiepDanDinhKyModel>();

            SqlParameter[] parameters = new SqlParameter[]
            {
              new SqlParameter("@TiepDinhKyID",SqlDbType.Int),
            };
            parameters[0].Value = TiepDinhKyID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_NV_TiepDanDinhKy_GetByID", parameters))
                {
                    while (dr.Read())
                    {
                        ChiTietTiepDanDinhKyModel Info = new ChiTietTiepDanDinhKyModel();
                        Info.TiepDinhKyID = Utils.ConvertToInt32(dr["TiepDinhKyID"], 0);
                        Info.NgayTiep = Utils.ConvertToNullableDateTime(dr["NgayTiep"], null);
                        Info.LanhDaoTiep = Utils.ConvertToInt32(dr["LanhDaoTiep"], 0);
                        Info.UyQuyenTiep = Utils.ConvertToInt32(dr["UyQuyenTiep"], 0);
                        Info.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"], 0);
                        Info.LoaiTiepDanID = Utils.ConvertToInt32(dr["LoaiTiepDanID"], 0);
                        Info.ChucVu = Utils.ConvertToString(dr["ChucVu"], string.Empty);
                        Info.NoiDungTiep = Utils.ConvertToString(dr["NoiDungTiep"], string.Empty);
                        Info.KetQuaTiep = Utils.ConvertToString(dr["KetQuaTiep"], string.Empty);
                        Info.KetQuaGQCacNganh = Utils.ConvertToString(dr["KetQuaGQCacNganh"], string.Empty);
                        Info.TenLanhDaoTiep = Utils.ConvertToString(dr["TenLanhDaoTiep"], string.Empty);
                        Info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        //thanh phan tham gia
                        Info.ID = Utils.ConvertToInt32(dr["ID"], 0);
                        Info.ChucVuThanhPhanThamGia = Utils.ConvertToString(dr["ChucVuThanhPhanThamGia"], string.Empty);
                        Info.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);

                        tiepdaninfo.Add(Info);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }

            Result = tiepdaninfo.GroupBy(p => p.TiepDinhKyID)
                   .Select(g => new TiepDanDinhKyModel
                   {
                       TiepDinhKyID = g.Key,
                       NgayTiep = g.FirstOrDefault().NgayTiep,
                       LanhDaoTiep = g.FirstOrDefault().LanhDaoTiep,
                       TenLanhDaoTiep = g.FirstOrDefault().TenLanhDaoTiep,
                       UyQuyenTiep = g.FirstOrDefault().UyQuyenTiep,
                       NhomKNID = g.FirstOrDefault().NhomKNID,
                       LoaiTiepDanID = g.FirstOrDefault().LoaiTiepDanID,
                       ChucVu = g.FirstOrDefault().ChucVu,
                       NoiDungTiep = g.FirstOrDefault().NoiDungTiep,
                       KetQuaTiep = g.FirstOrDefault().KetQuaTiep,
                       KetQuaGQCacNganh = g.FirstOrDefault().KetQuaGQCacNganh,
                       CoQuanID = g.FirstOrDefault().CoQuanID,
                       ThanhPhanThamGia = tiepdaninfo.Where(x => x.TiepDinhKyID == g.Key && x.ID > 0).GroupBy(x => x.ID)
                                       .Select(y => new ThanhPhanThamGiaTĐinhKyInfo
                                       {
                                           ID = y.FirstOrDefault().ID,
                                           TiepDinhKyID = g.Key,
                                           TenCanBo = y.FirstOrDefault().TenCanBo,
                                           ChucVu = y.FirstOrDefault().ChucVuThanhPhanThamGia,
                                       }
                                       ).ToList(),
                   }
                   ).ToList();
            return Result.FirstOrDefault();
        }

        public IList<TiepDanDinhKyModel> GetThongTinVuViec(int TiepDinhKyID)
        {
            IList<TiepDanDinhKyModel> Result = new List<TiepDanDinhKyModel>();

            SqlParameter[] parameters = new SqlParameter[]
            {
              new SqlParameter("@TiepDinhKyID",SqlDbType.Int),
            };
            parameters[0].Value = TiepDinhKyID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_NV_CTTiepDanDinhKy_GetByID", parameters))
                {
                    while (dr.Read())
                    {
                        TiepDanDinhKyModel Info = new TiepDanDinhKyModel();
                        Info.TiepDinhKyID = Utils.ConvertToInt32(dr["TiepDinhKyID"], 0);
                        Info.CTTiepDinhKyID = Utils.ConvertToInt32(dr["CTTiepDinhKyID"], 0);
                        Info.UyQuyenTiep = Utils.ConvertToInt32(dr["UyQuyenTiep"], 0);
                        Info.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"], 0);
                        Info.NoiDungTiep = Utils.ConvertToString(dr["NoiDungTiep"], string.Empty);
                        Info.KetQuaTiep = Utils.ConvertToString(dr["KetQuaTiep"], string.Empty);
                        Info.KetQuaGQCacNganh = Utils.ConvertToString(dr["KetQuaGQCacNganh"], string.Empty);
                        Result.Add(Info);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }


            return Result;
        }

        public int DeleteTiepDanDinhKy(int TiepDinhKyID)
        {
            int val = 0;
            SqlParameter[] parameters = new SqlParameter[]
            {
              new SqlParameter("@TiepDinhKyID",SqlDbType.Int),
            };
            parameters[0].Value = TiepDinhKyID;

            try
            {
                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "v2_NV_TiepDanDinhKy_Delete", parameters);
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
            }
            catch
            {

                throw;
            }


            return val;
        }

        public int DeleteVuViec(int CTTiepDinhKyID)
        {
            int val = 0;
            SqlParameter[] parameters = new SqlParameter[]
            {
              new SqlParameter("@CTTiepDinhKyID",SqlDbType.Int),
            };
            parameters[0].Value = CTTiepDinhKyID;

            try
            {
                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "v2_NV_CTTiepDanDinhKy_Delete", parameters);
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
            }
            catch
            {

                throw;
            }


            return val;
        }

        public int CapNhapSoDonThuTheoNamVaCoQuan(int coQuanID, int namTiepNhan)
        {

            int val = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("CoQuanID",SqlDbType.Int),
                new SqlParameter("NamTiepNhan",SqlDbType.Int)
            };
            parameters[0].Value = coQuanID;
            parameters[1].Value = namTiepNhan;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "v2_XuLyDon_CapNhapSoDonThuTheoNamVaCoQuan", parameters);
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
