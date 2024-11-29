using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Ultilities;
using DocumentFormat.OpenXml.EMMA;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.DAL.KNTC
{
    public class DonThuGiaiQuyet
    {
        #region Database query string

        private const string COUNT = @"PhanGiaiQuyet_Count";
        private const string GET_ALL = @"PhanGiaiQuyet_GetAll";
        private const string GET_BY_SEARCH = @"PhanGiaiQuyet_GetBySearch";
        private const string GET_BY_ID = @"PhanGiaiQuyet_GetByID";
        private const string DELETE = @"PhanGiaiQuyet_Delete";
        private const string UPDATE = @"PhanGiaiQuyet_Update";
        private const string INSERT = @"PhanGiaiQuyet_Insert";

        private const string GET_DONTHU_CANGIAIQUYET_TRONGCOQUAN = "PhanGiaiQuyet_GetDonThuCanPhanGiaiQuyetTrongCoQuan";
        private const string COUNT_DONTHU_CANPHANGIAIQUYET_TRONGCOQUAN = @"PhanGiaiQuyet_CountDonThuCanPhanGiaiQuyetTrongCoQuan";
        private const string COUNT_DONTHU_CANPHANGIAIQUYET_TP = @"PhanGiaiQuyet_CountDonThuCanPhanGiaiQuyetTP";
        private const string GET_DONTHU_CANPHANGIAIQUYET_TP = "PhanGiaiQuyet_GetDonThuCanPhanGiaiQuyetTP";

        private const string GET_DONTHU_CANPHANGIAIQUYET_CV = "PhanGiaiQuyet_GetDonThuCanPhanGiaiQuyetCV";
        private const string COUNT_DONTHU_CANPHANGIAIQUYET_CV = "PhanGiaiQuyet_CountDonThuCanPhanGiaiQuyetCV";

        private const string GET_DONTHU_CANGIAIQUYET_TRONGPHONG = "PhanGiaiQuyet_GetDonThuCanPhanGiaiQuyetTrongPhong";
        private const string GET_DONTHU_CANDUYET_TRONGCOQUAN = "PhanGiaiQuyet_GetDonThuCanDuyetTrongCoQuan";
        private const string GET_DONTHU_CANDUYET_TRONGPHONG = "PhanGiaiQuyet_GetDonThuCanDuyetTrongPhong";

        private const string COUNT_DONTHU_CHUAPHANGIAIQUYET_TRONGCOQUAN = "PhanGiaiQuyet_CountDonThuChuaPhanGiaiQuyetTrongCoQuan";
        private const string COUNT_DONTHU_CHUAPHANGIAIQUYET_TRONGPHONG = "PhanGiaiQuyet_CountDonThuChuaPhanGiaiQuyetTrongPhong";

        private const string GET_DS_DONTHU_CANGIAIQUYET = @"XuLyDon_GetDonThuCanGiaiQuyet";
        private const string COUNT_DONTHU_CANGIAIQUYET = @"XuLyDon_CountDonThuCanGiaiQuyet";

        //Don thu dang giai quyet
        private const string GET_DONTHU_DANGGIAIQUYET_CHUYENVIEN = @"XuLyDon_GetDangGiaiQuyet_ChuyenVien";
        private const string COUNT_DONTHU_DANGGIAIQUYET_CHUYENVIEN = @"XuLyDon_Count_DangGiaiQuyet_ChuyenVien";
        private const string GET_DONTHU_DANGGIAIQUYET_LANHDAO = @"XuLyDon_GetDangGiaiQuyet_LanhDao";
        private const string COUNT_DONTHU_DANGGIAIQUYET_LANHDAO = @"XuLyDon_Count_DangGiaiQuyet_LanhDao";

        //Don Thu can duyet giai quyet
        private const string GET_DONTHU_CANDUYET_GIAIQUYET = "XuLyDon_GetDTDuyetGiaiQuyet";
        private const string COUNT_DONTHU_CANDUYET_GIAIQUYET = "XuLyDon_CountDTDuyetGiaiQuyet";
        private const string GET_DONTHU_CANDUYET_GIAIQUYET_TP = @"XuLyDon_GetDTDuyetGiaiQuyet_TP";
        private const string COUNT_DONTHU_CANDUYET_GIAIQUYET_TP = "XuLyDon_CountDTDuyetGiaiQuyet_TP";
        private const string GET_DONTHU_CANDUYET_GIAIQUYET_NEW = "XuLyDon_GetDTDuyetGiaiQuyet_New";
        private const string GET_DONTHU_CANDUYET_GIAIQUYET_TP_NEW = @"XuLyDon_GetDTDuyetGiaiQuyet_TP_New";

        private const string GET_DONTHU_GIAIQUYET_INFO = "DonThu_GetQuaTrinhGiaiQuyetInfo";
        private const string GET_DONTHU_GIAIQUYET_DATATABLE = "DonThu_GetQuaTrinhGiaiQuyetDataTable";
        private const string GET_DUONGDANFILE = "DonThu_GetDuongDanFile";
        private const string GET_CHITIET_BUOCQUATRINHGIAIQUYET = "DonThu_GetChiTietBuocQuaTrinhGiaiQuyet";

        //Rut don
        private const string GET_DONTHU_DARUT = "DonThu_GetDaRut";
        private const string COUNT_DONTHU_DARUT = "DonThu_CountDaRut";

        //chuyen don, ra van ban don doc
        private const string GET_DONTHU_CANCHUYENDON_RAVBDONDOC = "ChuyenDon_GetDTChuyenDon_RaVBDonDoc";
        private const string COUNT_DONTHU_CANCHUYENDON_VBDONDOC = "ChuyenDon_CountDTChuyenDon_RaVBDonDoc";

        #endregion

        #region paramaters constant

        private const string PARM_PHANGIAIQUYETID = @"PhanGiaiQuyetID";
        private const string PARM_XULYDONID = @"XuLyDonID";
        private const string PARM_TRANGTHAIXULYID = @"TrangThaiXuLyID";
        private const string PARM_PHONGBANID = @"PhongBanID";
        private const string PARM_CANBOID = @"CanBoID";
        private const string PARM_TRANGTHAI = @"TrangThai";
        private const string PARM_CHAID = @"ChaID";
        private const string PARM_COQUANID = "@CoQuanID";
        private const string PARM_CANBOGIAOID = "CanBoGiaoID";
        private const string PARM_LOAIKHIEUTOID = @"LoaiKhieuToID";
        private const string PARM_TUNGAY = @"TuNgay";
        private const string PARM_DENNGAY = @"DenNgay";
        private const string PARM_STATENAME = @"StateName";
        private const string PARM_DOCUMENT_ID_LIST = @"DocumentIDList";
        private const string PARM_DONTHUID = "@DonThuID";
        private const string PARM_VAITRO = "@VaiTro";

        private const string PARM_START = "@Start";
        private const string PARM_END = "@End";
        private const string PARM_KEYWORD = "@KeyWord";

        #endregion


        private DonThuGiaiQuyetInfo GetData(SqlDataReader rdr)
        {
            DonThuGiaiQuyetInfo info = new DonThuGiaiQuyetInfo();

            info.XuLyDonID = Utils.GetInt32(rdr["XuLyDonID"], 0);
            info.DonThuID = Utils.GetInt32(rdr["DonThuID"], 0);
            info.HoTen = Utils.GetString(rdr["HoTen"], string.Empty);
            info.SoDonThu = Utils.GetString(rdr["SoDonThu"], string.Empty);
            info.NoiDungDon = Utils.GetString(rdr["NoiDungDon"], String.Empty);
            if (info.NoiDungDon.Length > Constant.LengthNoiDungDon)
            {
                info.NoiDungDon = info.NoiDungDon.Substring(0, Constant.LengthNoiDungDon) + Constant.ChuoiCuoiNDDon;
            }

            info.TenLoaiKhieuTo = Utils.GetString(rdr["TenLoaiKhieuTo"], String.Empty);

            return info;
        }

        private DonThuGiaiQuyetInfo GetDataCTCanGiaiQuyet(SqlDataReader rdr)
        {
            DonThuGiaiQuyetInfo info = new DonThuGiaiQuyetInfo();

            info.XuLyDonID = Utils.GetInt32(rdr["XuLyDonID"], 0);
            info.DonThuID = Utils.GetInt32(rdr["DonThuID"], 0);
            info.HoTen = Utils.GetString(rdr["HoTen"], string.Empty);
            info.SoDonThu = Utils.GetString(rdr["SoDonThu"], string.Empty);
            info.NoiDungDon = Utils.GetString(rdr["NoiDungDon"], String.Empty);
            if (info.NoiDungDon.Length > Constant.LengthNoiDungDon)
            {
                info.NoiDungDon = info.NoiDungDon.Substring(0, Constant.LengthNoiDungDon) + Constant.ChuoiCuoiNDDon;
            }
            info.NgayPhanCong = Utils.GetDateTime(rdr["ModifiedDate"], DateTime.MinValue);
            info.NgayPhanCongs = info.NgayPhanCong.ToString("dd/MM/yyyy");
            info.HanGiaiQuyet = Utils.GetDateTime(rdr["DueDate"], DateTime.MinValue);
            info.HanGiaiQuyets = info.HanGiaiQuyet.Value.ToString("dd/MM/yyyy");
            info.TenLoaiKhieuTo = Utils.GetString(rdr["TenLoaiKhieuTo"], String.Empty);
            info.TenCanBoGiao = Utils.GetString(rdr["TenCanBo"], String.Empty);
            info.LoaiKhieuTo1ID = Utils.GetInt32(rdr["LoaiKhieuTo1ID"], 0);

            return info;
        }
        private DonThuGiaiQuyetInfo GetDataDangGiaiQuyet(SqlDataReader rdr)
        {
            DonThuGiaiQuyetInfo info = new DonThuGiaiQuyetInfo();

            info.XuLyDonID = Utils.GetInt32(rdr["XuLyDonID"], 0);
            info.DonThuID = Utils.GetInt32(rdr["DonThuID"], 0);
            info.HoTen = Utils.GetString(rdr["HoTen"], string.Empty);
            info.SoDonThu = Utils.GetString(rdr["SoDonThu"], string.Empty);
            info.NoiDungDon = Utils.GetString(rdr["NoiDungDon"], String.Empty);
            info.NguonDonDen = Utils.GetInt32(rdr["NguonDonDen"], 0);
            info.TenNguonDonDen = "";
            if (info.NguonDonDen == (int)EnumNguonDonDen.TrucTiep)
            {
                info.TenNguonDonDen = Constant.NguonDon_TrucTieps;
            }
            if (info.NguonDonDen == (int)EnumNguonDonDen.BuuChinh)
            {
                info.TenNguonDonDen = Constant.NguonDon_BuuChinhs;
            }
            if (info.NguonDonDen == (int)EnumNguonDonDen.CoQuanKhac)
            {
                info.TenNguonDonDen = Constant.NguonDon_CoQuanKhacs;
            }
            if (info.NoiDungDon.Length > Constant.LengthNoiDungDon)
            {
                info.NoiDungDon = info.NoiDungDon.Substring(0, Constant.LengthNoiDungDon) + Constant.ChuoiCuoiNDDon;
            }
            info.HanGiaiQuyet = Utils.GetDateTime(rdr["DueDate"], DateTime.MinValue);
            if (info.HanGiaiQuyet != DateTime.MinValue)
            {
                info.HanGiaiQuyets = info.HanGiaiQuyet.Value.ToString("dd/MM/yyyy");
            }
            info.TenLoaiKhieuTo = Utils.GetString(rdr["TenLoaiKhieuTo"], String.Empty);
            return info;
        }

        private DonThuGiaiQuyetInfo GetDataRutDon(SqlDataReader rdr)
        {
            DonThuGiaiQuyetInfo info = new DonThuGiaiQuyetInfo();

            info.XuLyDonID = Utils.GetInt32(rdr["XuLyDonID"], 0);
            info.DonThuID = Utils.GetInt32(rdr["DonThuID"], 0);
            info.HoTen = Utils.GetString(rdr["HoTen"], string.Empty);
            info.SoDonThu = Utils.GetString(rdr["SoDonThu"], string.Empty);
            info.NoiDungDon = Utils.GetString(rdr["NoiDungDon"], String.Empty);
            info.TenLoaiKhieuTo = Utils.GetString(rdr["TenLoaiKhieuTo"], String.Empty);
            info.NgayRutDon = Utils.GetDateTime(rdr["NgayRutDon"], DateTime.MinValue);
            info.TenLoaiKhieuTo = Utils.GetString(rdr["TenLoaiKhieuTo"], String.Empty);
            info.LyDoRutDon = Utils.GetString(rdr["LyDo"], String.Empty);
            return info;
        }



        private DonThuGiaiQuyetInfo GetDataPhongBan(SqlDataReader rdr)
        {
            DonThuGiaiQuyetInfo phanGiaiQuyetInfo = new DonThuGiaiQuyetInfo();
            phanGiaiQuyetInfo.PhanGiaiQuyetID = Utils.GetInt32(rdr["PhanGiaiQuyetID"], 0);
            phanGiaiQuyetInfo.XuLyDonID = Utils.GetInt32(rdr["XuLyDonID"], 0);
            phanGiaiQuyetInfo.PhongBanID = Utils.GetInt32(rdr["PhongBanID"], 0);
            phanGiaiQuyetInfo.CanBoID = Utils.GetInt32(rdr["CanBoID"], 0);
            phanGiaiQuyetInfo.TrangThai = Utils.GetInt32(rdr["TrangThai"], 0);
            phanGiaiQuyetInfo.ChaID = Utils.GetInt32(rdr["ChaID"], 0);

            phanGiaiQuyetInfo.SoDonThu = Utils.GetString(rdr["SoDonThu"], string.Empty);
            phanGiaiQuyetInfo.NhomKNID = Utils.GetInt32(rdr["NhomKNID"], 0);
            phanGiaiQuyetInfo.NoiDungDon = Utils.GetString(rdr["NoiDungDon"], String.Empty);
            phanGiaiQuyetInfo.NgayQuaHan = Utils.GetDateTime(rdr["NgayQuaHan"], DateTime.MinValue);
            phanGiaiQuyetInfo.DonThuID = Utils.GetInt32(rdr["DonThuID"], 0);
            phanGiaiQuyetInfo.TrangThaiGiao = Utils.ConvertToInt32(rdr["TrangThaiGiao"], 0);

            phanGiaiQuyetInfo.TenCanBo = Utils.GetString(rdr["TenCanBo"], String.Empty);
            phanGiaiQuyetInfo.TenPhongBan = Utils.GetString(rdr["TenPhongBan"], String.Empty);

            phanGiaiQuyetInfo.TrangThaiGQCapDuoi = Utils.GetInt32(rdr["TrangThaiGQCapDuoi"], 0);
            phanGiaiQuyetInfo.PhanGiaiQuyetCapDuoiID = Utils.GetInt32(rdr["PhanGiaiQuyetCapDuoiID"], 0);

            return phanGiaiQuyetInfo;
        }

        private DonThuGiaiQuyetInfo GetDataCanDuyet(SqlDataReader rdr)
        {
            DonThuGiaiQuyetInfo phanGiaiQuyetInfo = new DonThuGiaiQuyetInfo();
            phanGiaiQuyetInfo.PhanGiaiQuyetID = Utils.GetInt32(rdr["PhanGiaiQuyetID"], 0);
            phanGiaiQuyetInfo.PhanGiaiQuyetCapDuoiID = Utils.GetInt32(rdr["PhanGiaiQuyetCapDuoiID"], 0);

            phanGiaiQuyetInfo.XuLyDonID = Utils.GetInt32(rdr["XuLyDonID"], 0);
            phanGiaiQuyetInfo.PhongBanID = Utils.GetInt32(rdr["PhongBanID"], 0);
            phanGiaiQuyetInfo.CanBoID = Utils.GetInt32(rdr["CanBoID"], 0);
            phanGiaiQuyetInfo.TrangThai = Utils.GetInt32(rdr["TrangThai"], 0);
            phanGiaiQuyetInfo.ChaID = Utils.GetInt32(rdr["ChaID"], 0);

            phanGiaiQuyetInfo.SoDonThu = Utils.GetString(rdr["SoDonThu"], string.Empty);
            phanGiaiQuyetInfo.NhomKNID = Utils.GetInt32(rdr["NhomKNID"], 0);
            phanGiaiQuyetInfo.NoiDungDon = Utils.GetString(rdr["NoiDungDon"], String.Empty);
            phanGiaiQuyetInfo.NgayQuaHan = Utils.GetDateTime(rdr["NgayQuaHan"], DateTime.MinValue);
            phanGiaiQuyetInfo.DonThuID = Utils.GetInt32(rdr["DonThuID"], 0);

            phanGiaiQuyetInfo.TenCanBo = Utils.GetString(rdr["TenCanBo"], String.Empty);
            phanGiaiQuyetInfo.TenPhongBan = Utils.GetString(rdr["TenPhongBan"], String.Empty);

            return phanGiaiQuyetInfo;
        }

        private DonThuGiaiQuyetInfo GetDataDuyetGiaiQuyet(SqlDataReader rdr)
        {
            DonThuGiaiQuyetInfo info = new DonThuGiaiQuyetInfo();
            info.XuLyDonID = Utils.GetInt32(rdr["XuLyDonID"], 0);
            info.DonThuID = Utils.GetInt32(rdr["DonThuID"], 0);
            info.HoTen = Utils.GetString(rdr["HoTen"], string.Empty);
            info.SoDonThu = Utils.GetString(rdr["SoDonThu"], string.Empty);
            info.NoiDungDon = Utils.GetString(rdr["NoiDungDon"], String.Empty);
            info.TenLoaiKhieuTo = Utils.GetString(rdr["TenLoaiKhieuTo"], string.Empty);
            info.StateName = Utils.GetString(rdr["StateName"], string.Empty);

            if (info.NoiDungDon.Length > Constant.LengthNoiDungDon)
            {
                info.NoiDungDon = info.NoiDungDon.Substring(0, Constant.LengthNoiDungDon) + Constant.ChuoiCuoiNDDon;
            }
            info.NgayGui = Utils.GetDateTime(rdr["ModifiedDate"], DateTime.MinValue);
            if (info.NgayGui != DateTime.MinValue)
            {
                info.NgayGuis = info.NgayGui.ToString("dd/MM/yyyy");
            }
            info.NguonDonDen = Utils.GetInt32(rdr["NguonDonDen"], 0);
            info.TenNguonDonDen = "";
            if (info.NguonDonDen == (int)EnumNguonDonDen.TrucTiep)
            {
                info.TenNguonDonDen = Constant.NguonDon_TrucTieps;
            }
            if (info.NguonDonDen == (int)EnumNguonDonDen.CoQuanKhac)
            {
                info.TenNguonDonDen = Constant.NguonDon_CoQuanKhacs;
            }
            if (info.NguonDonDen == (int)EnumNguonDonDen.BuuChinh)
            {
                info.TenNguonDonDen = Constant.NguonDon_BuuChinhs;
            }
            if (info.NguonDonDen == (int)EnumNguonDonDen.TraoTay)
            {
                info.TenNguonDonDen = Constant.NguonDon_TraoTays;
            }

            info.TenCanBo = Utils.GetString(rdr["TenCanBo"], string.Empty);
            info.HanGiaiQuyet = Utils.ConvertToDateTime(rdr["HanGiaiQuyetNew"], DateTime.MinValue);
            if (info.HanGiaiQuyet != DateTime.MinValue)
            {
                info.HanGiaiQuyets = Format.FormatDate(info.HanGiaiQuyet.Value);
            }

            return info;
        }

        public DonThuGiaiQuyetInfo GetQuaTrinhGiaiQuyetInfo(SqlDataReader rdr)
        {
            DonThuGiaiQuyetInfo duyetGiaiQuyetInfo = new DonThuGiaiQuyetInfo();
            duyetGiaiQuyetInfo.SoDonThu = Utils.GetString(rdr["SoDonThu"], string.Empty);
            duyetGiaiQuyetInfo.XuLyDonID = Utils.GetInt32(rdr["XuLyDonID"], 0);
            duyetGiaiQuyetInfo.HoTen = Utils.GetString(rdr["HoTen"], string.Empty);

            duyetGiaiQuyetInfo.TenCanBo = Utils.GetString(rdr["TenCanBo"], string.Empty);

            return duyetGiaiQuyetInfo;
        }

        public DonThuGiaiQuyetInfo GetChiTietBuocQuaTrinhGiaiQuyet(SqlDataReader rdr)
        {
            DonThuGiaiQuyetInfo duyetGiaiQuyetInfo = new DonThuGiaiQuyetInfo();
            duyetGiaiQuyetInfo.TrangThaiXuLyID = Utils.GetInt32(rdr["TrangThaiXuLyID"], 0);
            duyetGiaiQuyetInfo.NoiDungDon = Utils.GetString(rdr["NoiDung"], string.Empty);
            duyetGiaiQuyetInfo.NgayCapNhat = Utils.GetDateTime(rdr["NgayCapNhat"], DateTime.MinValue);

            duyetGiaiQuyetInfo.DuongDanFile = Utils.GetString(rdr["DuongDanFile"], string.Empty);

            return duyetGiaiQuyetInfo;
        }

        public DonThuGiaiQuyetInfo GetQuaTrinhGiaiQuyet_DataTable(SqlDataReader rdr)
        {
            DonThuGiaiQuyetInfo duyetGiaiQuyetInfo = new DonThuGiaiQuyetInfo();
            duyetGiaiQuyetInfo.GhiChu = Utils.GetString(rdr["GhiChu"], string.Empty);
            duyetGiaiQuyetInfo.XuLyDonID = Utils.GetInt32(rdr["XuLyDonID"], 0);
            duyetGiaiQuyetInfo.TheoDoiXuLyID = Utils.GetInt32(rdr["TheoDoiXuLyID"], 0);
            duyetGiaiQuyetInfo.TenTrangThaiXuLy = Utils.GetString(rdr["TenTrangThaiXuLy"], String.Empty);
            duyetGiaiQuyetInfo.TrangThaiXuLyID = Utils.GetInt32(rdr["TrangThaiXuLyID"], 0);
            duyetGiaiQuyetInfo.NgayCapNhat = Utils.GetDateTime(rdr["NgayCapNhat"], DateTime.MinValue);
            duyetGiaiQuyetInfo.DuongDanFile = Utils.GetString(rdr["DuongDanFile"], String.Empty);
            return duyetGiaiQuyetInfo;
        }

        private DonThuGiaiQuyetInfo GetBasicData(SqlDataReader rdr)
        {
            DonThuGiaiQuyetInfo phanGiaiQuyetInfo = new DonThuGiaiQuyetInfo();
            phanGiaiQuyetInfo.PhanGiaiQuyetID = Utils.GetInt32(rdr["PhanGiaiQuyetID"], 0);
            phanGiaiQuyetInfo.XuLyDonID = Utils.GetInt32(rdr["XuLyDonID"], 0);
            phanGiaiQuyetInfo.PhongBanID = Utils.GetInt32(rdr["PhongBanID"], 0);
            phanGiaiQuyetInfo.CanBoID = Utils.GetInt32(rdr["CanBoID"], 0);
            phanGiaiQuyetInfo.CanBoGiaoID = Utils.GetInt32(rdr["CanBoGiaoID"], 0);
            phanGiaiQuyetInfo.TrangThai = Utils.GetInt32(rdr["TrangThai"], 0);
            phanGiaiQuyetInfo.ChaID = Utils.GetInt32(rdr["ChaID"], 0);

            return phanGiaiQuyetInfo;
        }

        private SqlParameter[] GetInsertParms()
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_XULYDONID, SqlDbType.Int),
                new SqlParameter(PARM_PHONGBANID, SqlDbType.Int),
                new SqlParameter(PARM_CANBOID, SqlDbType.Int),
                new SqlParameter(PARM_TRANGTHAI, SqlDbType.Int),
                new SqlParameter(PARM_CHAID, SqlDbType.Int),
                new SqlParameter(PARM_CANBOGIAOID, SqlDbType.Int),
            }; return parms;
        }

        private void SetInsertParms(SqlParameter[] parms, DonThuGiaiQuyetInfo info)
        {
            parms[0].Value = info.XuLyDonID;
            parms[1].Value = info.PhongBanID;
            parms[2].Value = info.CanBoID;
            parms[3].Value = info.TrangThai;
            parms[4].Value = info.ChaID;
            parms[5].Value = info.CanBoGiaoID;

            if (info.PhongBanID == 0) parms[1].Value = DBNull.Value;
            if (info.CanBoID == 0) parms[2].Value = DBNull.Value;
            if (info.ChaID == 0) parms[4].Value = DBNull.Value;
        }

        private SqlParameter[] GetUpdateParms()
        {
            List<SqlParameter> parms = GetInsertParms().ToList();
            parms.Insert(0, new SqlParameter(PARM_PHANGIAIQUYETID, SqlDbType.Int));
            return parms.ToArray();
        }

        private void SetUpdateParms(SqlParameter[] parms, DonThuGiaiQuyetInfo info)
        {
            parms[0].Value = info.PhanGiaiQuyetID;
            parms[1].Value = info.XuLyDonID;
            parms[2].Value = info.PhongBanID;
            parms[3].Value = info.CanBoID;
            parms[4].Value = info.TrangThai;
            parms[5].Value = info.ChaID;
            parms[6].Value = info.CanBoGiaoID;

            if (info.PhongBanID == 0) parms[2].Value = DBNull.Value;
            if (info.CanBoID == 0) parms[3].Value = DBNull.Value;
            if (info.ChaID == 0) parms[5].Value = DBNull.Value;
        }

        public int Count(String keyword = "")
        {
            int result = 0;
            SqlParameter parm = new SqlParameter(PARM_KEYWORD, SqlDbType.NVarChar, 50);
            parm.Value = keyword;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT, parm))
                {
                    if (dr.Read())
                    {
                        result = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return result;
        }

        public int CountDonThuChuaPhanGiaiQuyet(int coquanID)
        {
            int result = 0;
            SqlParameter parm = new SqlParameter(PARM_COQUANID, SqlDbType.Int);
            parm.Value = coquanID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_DONTHU_CHUAPHANGIAIQUYET_TRONGCOQUAN, parm))
                {
                    if (dr.Read())
                    {
                        result = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return result;
        }

        public int CountDonThuChuaPhanGiaiQuyetTrongPhong(int phongbanID)
        {
            int result = 0;
            SqlParameter parm = new SqlParameter(PARM_PHONGBANID, SqlDbType.Int);
            parm.Value = phongbanID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_DONTHU_CHUAPHANGIAIQUYET_TRONGPHONG, parm))
                {
                    if (dr.Read())
                    {
                        result = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return result;
        }
        public SqlParameter[] GetParms_LocDL_LD()
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARM_KEYWORD, SqlDbType.NVarChar,50),
                new SqlParameter(PARM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter("@TuNgayGoc", SqlDbType.DateTime),
                new SqlParameter("@DenNgayGoc", SqlDbType.DateTime),
                new SqlParameter(PARM_START, SqlDbType.Int),
                new SqlParameter(PARM_END, SqlDbType.Int),
                new SqlParameter(PARM_STATENAME, SqlDbType.NVarChar,100),
                new SqlParameter(PARM_CANBOID, SqlDbType.Int),
                new SqlParameter(PARM_PHONGBANID, SqlDbType.Int),
                new SqlParameter("@TuNgayMoi", SqlDbType.DateTime),
                new SqlParameter("@DenNgayMoi", SqlDbType.DateTime),
            }; return parms;
        }


        public void SetParms_LocDL_LD(SqlParameter[] parms, QueryFilterInfo info)
        {
            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.KeyWord;
            parms[2].Value = info.LoaiKhieuToID;
            parms[3].Value = info.TuNgayGoc;
            parms[4].Value = info.DenNgayGoc;
            parms[5].Value = info.Start;
            parms[6].Value = info.End;
            parms[7].Value = info.StateName;
            parms[8].Value = info.CanBoID;
            parms[9].Value = info.PhongBanID;
            parms[10].Value = info.TuNgayMoi;
            parms[11].Value = info.DenNgayMoi;

            if (info.TuNgayGoc == DateTime.MinValue) parms[3].Value = DBNull.Value;
            if (info.DenNgayGoc == DateTime.MinValue) parms[4].Value = DBNull.Value;
            if (info.TuNgayMoi == DateTime.MinValue) parms[10].Value = DBNull.Value;
            if (info.DenNgayMoi == DateTime.MinValue) parms[11].Value = DBNull.Value;
        }

        public SqlParameter[] GetParms_Count_LocDL_LD()
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARM_KEYWORD, SqlDbType.NVarChar,50),
                new SqlParameter(PARM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter("@TuNgayGoc", SqlDbType.DateTime),
                new SqlParameter("@DenNgayGoc", SqlDbType.DateTime),
                new SqlParameter("@TuNgayMoi", SqlDbType.DateTime),
                new SqlParameter("@DenNgayMoi", SqlDbType.DateTime),
                new SqlParameter(PARM_STATENAME, SqlDbType.NVarChar,100),
                new SqlParameter(PARM_CANBOID, SqlDbType.Int),
            }; return parms;
        }


        public void SetParms_Count_LocDL_LD(SqlParameter[] parms, QueryFilterInfo info)
        {
            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.KeyWord;
            parms[2].Value = info.LoaiKhieuToID;
            parms[3].Value = info.TuNgayGoc;
            parms[4].Value = info.DenNgayGoc;
            parms[5].Value = info.TuNgayMoi;
            parms[6].Value = info.DenNgayMoi;
            parms[7].Value = info.StateName;
            parms[8].Value = info.CanBoID;

            if (info.TuNgayGoc == DateTime.MinValue) parms[3].Value = DBNull.Value;
            if (info.DenNgayGoc == DateTime.MinValue) parms[4].Value = DBNull.Value;
            if (info.TuNgayMoi == DateTime.MinValue) parms[5].Value = DBNull.Value;
            if (info.DenNgayMoi == DateTime.MinValue) parms[6].Value = DBNull.Value;
        }

        public SqlParameter[] GetParms_LocDL_TP()
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARM_KEYWORD, SqlDbType.NVarChar,50),
                new SqlParameter(PARM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter("@TuNgay", SqlDbType.DateTime),
                new SqlParameter("@DenNgay", SqlDbType.DateTime),
                new SqlParameter(PARM_START, SqlDbType.Int),
                new SqlParameter(PARM_END, SqlDbType.Int),
                new SqlParameter(PARM_STATENAME, SqlDbType.NVarChar,100),
                new SqlParameter(PARM_CANBOID, SqlDbType.Int),
                new SqlParameter(PARM_PHONGBANID, SqlDbType.Int)
            }; return parms;
        }


        public void SetParms_LocDL_TP(SqlParameter[] parms, QueryFilterInfo info)
        {
            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.KeyWord ?? Convert.DBNull;
            parms[2].Value = info.LoaiKhieuToID;
            parms[3].Value = info.TuNgay;
            parms[4].Value = info.DenNgay;
            parms[5].Value = info.Start;
            parms[6].Value = info.End;
            parms[7].Value = info.StateName;
            parms[8].Value = info.CanBoID;
            parms[9].Value = info.PhongBanID;

            if (info.TuNgay == DateTime.MinValue) parms[3].Value = DBNull.Value;
            if (info.DenNgay == DateTime.MinValue) parms[4].Value = DBNull.Value;

        }

        public SqlParameter[] GetParam_RutDon()
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARM_KEYWORD, SqlDbType.NVarChar,50),
                new SqlParameter(PARM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter(PARM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARM_START, SqlDbType.Int),
                new SqlParameter(PARM_END, SqlDbType.Int),

            }; return parms;
        }


        public void SetParam_RutDon(SqlParameter[] parms, QueryFilterInfo info)
        {
            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.KeyWord;
            parms[2].Value = info.LoaiKhieuToID;
            parms[3].Value = info.TuNgay;
            parms[4].Value = info.DenNgay;
            parms[5].Value = info.Start;
            parms[6].Value = info.End;


            if (info.TuNgay == DateTime.MinValue) parms[3].Value = DBNull.Value;
            if (info.DenNgay == DateTime.MinValue) parms[4].Value = DBNull.Value;
        }

        public SqlParameter[] GetParam_CountRutDon()
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARM_KEYWORD, SqlDbType.NVarChar,50),
                new SqlParameter(PARM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter(PARM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARM_DENNGAY, SqlDbType.DateTime),


            }; return parms;
        }


        public void SetParam_CountRutDon(SqlParameter[] parms, QueryFilterInfo info)
        {
            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.KeyWord;
            parms[2].Value = info.LoaiKhieuToID;
            parms[3].Value = info.TuNgay;
            parms[4].Value = info.DenNgay;



            if (info.TuNgay == DateTime.MinValue) parms[3].Value = DBNull.Value;
            if (info.DenNgay == DateTime.MinValue) parms[4].Value = DBNull.Value;
        }


        public SqlParameter[] GetParms_DTCanGiaiQuyet()
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARM_KEYWORD, SqlDbType.NVarChar,50),
                new SqlParameter(PARM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter(PARM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARM_START, SqlDbType.Int),
                new SqlParameter(PARM_END, SqlDbType.Int),
                //new SqlParameter(PARM_STATENAME, SqlDbType.NVarChar,100),
                new SqlParameter(PARM_CANBOID,SqlDbType.Int),
            }; return parms;
        }


        public void SetParms_DTCanGiaiQuyet(SqlParameter[] parms, QueryFilterInfo info, int canboid)
        {
            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.KeyWord ?? Convert.DBNull;
            parms[2].Value = info.LoaiKhieuToID;
            parms[3].Value = info.TuNgay;
            parms[4].Value = info.DenNgay;
            parms[5].Value = info.Start;
            parms[6].Value = info.End;
            // parms[7].Value = info.StateName;
            parms[7].Value = canboid;

            if (info.TuNgay == DateTime.MinValue) parms[3].Value = DBNull.Value;
            if (info.DenNgay == DateTime.MinValue) parms[4].Value = DBNull.Value;
        }

        public IList<DonThuGiaiQuyetInfo> GetDTCanChuyenDon_RaVBDonDoc(QueryFilterInfo info)
        {
            IList<DonThuGiaiQuyetInfo> phangiaiquyets = new List<DonThuGiaiQuyetInfo>();

            SqlParameter[] parms = GetParms_LocDL_LD();

            SetParms_LocDL_LD(parms, info);

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DONTHU_CANCHUYENDON_RAVBDONDOC, parms))
                {
                    while (dr.Read())
                    {
                        DonThuGiaiQuyetInfo phanGiaiQuyetInfo = GetData(dr);
                        //phanGiaiQuyetInfo.TenCoQuanPhanGQ = Utils.ConvertToString(dr["TenCoQuanPhanGQ"], string.Empty);
                        phanGiaiQuyetInfo.TenNguonDonDen = Utils.GetString(dr["TenNguonDonDen"], string.Empty);
                        phanGiaiQuyetInfo.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        phanGiaiQuyetInfo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        //phanGiaiQuyetInfo.TheoDoiXuLyID = Utils.ConvertToInt32(dr["TheoDoiXuLyID"], 0);
                        string now = Format.FormatDate(DateTime.Now);
                        DateTime ngayHienTai = Utils.ConvertToDateTime(now, DateTime.MinValue);


                        //phanGiaiQuyetInfo.HanGQGoc = Utils.ConvertToDateTime(dr["HanGQGoc"], DateTime.MinValue);
                        //phanGiaiQuyetInfo.HanGQMoi = Utils.ConvertToDateTime(dr["HanGQMoi"], DateTime.MinValue);
                        phanGiaiQuyetInfo.CQNhanDonChuyen = Utils.ConvertToString(dr["CQNhanDonChuyen"], string.Empty);
                        phanGiaiQuyetInfo.CQNhanVanBanDonDoc = Utils.ConvertToString(dr["CQNhanVanBanDonDoc"], string.Empty);
                        phanGiaiQuyetInfo.HuongXuLyID = Utils.ConvertToInt32(dr["HuongGiaiQuyetID"], 0);
                        phanGiaiQuyetInfo.TenHuongGiaiQuyet = Utils.ConvertToString(dr["TenHuongGiaiQuyet"], string.Empty);

                        phangiaiquyets.Add(phanGiaiQuyetInfo);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return phangiaiquyets;
        }
        public int CountDTCanChuyenDon_RaVBDonDoc(QueryFilterInfo info)
        {
            int Count = 0;

            SqlParameter[] parms = GetParms_Count_LocDL_LD();

            SetParms_Count_LocDL_LD(parms, info);

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_DONTHU_CANPHANGIAIQUYET_TRONGCOQUAN, parms))
                {
                    if (dr.Read())
                    {
                        Count = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return Count;
        }

        //Lay danh sach cac don thu can phan giai quyet trong co quan. Cac don thu phan cho co quan khac giai quyet se ko hien thi trong danh sach nay
        public IList<DonThuGiaiQuyetInfo> GetDonThuCanPhanGiaiQuyet(QueryFilterInfo info)
        {
            IList<DonThuGiaiQuyetInfo> phangiaiquyets = new List<DonThuGiaiQuyetInfo>();

            SqlParameter[] parms = GetParms_LocDL_LD();

            SetParms_LocDL_LD(parms, info);

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DONTHU_CANGIAIQUYET_TRONGCOQUAN, parms))
                {
                    while (dr.Read())
                    {
                        DonThuGiaiQuyetInfo phanGiaiQuyetInfo = GetData(dr);
                        phanGiaiQuyetInfo.DuongDanFile = Utils.ConvertToString(dr["FileUrl"], string.Empty);
                        //phanGiaiQuyetInfo.ExecuteTime = Utils.ConvertToInt32(dr["ExecuteTime"], 1);
                        //phanGiaiQuyetInfo.DaPhanCQKhac = Utils.ConvertToInt32(dr["DaPhanCQKhac"], 1);
                        //phanGiaiQuyetInfo.DaPhanTrongCQ = Utils.ConvertToInt32(dr["DaPhanTrongCQ"], 1);
                        phanGiaiQuyetInfo.TenCoQuanPhanGQ = Utils.ConvertToString(dr["TenCoQuanPhanGQ"], string.Empty);
                        phanGiaiQuyetInfo.TenCoQuanGQ = Utils.ConvertToString(dr["TenCoQuanGQ"], string.Empty);
                        phanGiaiQuyetInfo.TenNguonDonDen = Utils.GetString(dr["TenNguonDonDen"], string.Empty);
                        phanGiaiQuyetInfo.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        phanGiaiQuyetInfo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        phanGiaiQuyetInfo.ChuyenGiaiQuyetID = Utils.ConvertToInt32(dr["CQGiaoID"], 0);
                        phanGiaiQuyetInfo.TheoDoiXuLyID = Utils.ConvertToInt32(dr["TheoDoiXuLyID"], 0);

                        string now = Format.FormatDate(DateTime.Now);
                        DateTime ngayHienTai = Utils.ConvertToDateTime(now, DateTime.MinValue);
                        //TimeSpan ngayConLai = 0;

                        phanGiaiQuyetInfo.HanGQGoc = Utils.ConvertToDateTime(dr["HanGQGoc"], DateTime.MinValue);
                        phanGiaiQuyetInfo.HanGQMoi = Utils.ConvertToDateTime(dr["HanGQMoi"], DateTime.MinValue);
                        phanGiaiQuyetInfo.HanGQQTPhucTap = Utils.ConvertToDateTime(dr["HanGQQTPhucTap"], DateTime.MinValue);
                        phanGiaiQuyetInfo.LanhDaoDuyet2ID = Utils.ConvertToInt32(dr["LanhDaoDuyet2ID"], 0);


                        phangiaiquyets.Add(phanGiaiQuyetInfo);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return phangiaiquyets;
        }
        public IList<DonThuGiaiQuyetInfo> GetDonThuCanPhanGiaiQuyet_PhanHoi(QueryFilterInfo info, ref int TotalRow)
        {
            IList<DonThuGiaiQuyetInfo> phangiaiquyets = new List<DonThuGiaiQuyetInfo>();

            //SqlParameter[] parms = GetParms_LocDL_LD();
            //SetParms_LocDL_LD(parms, info);

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARM_KEYWORD, SqlDbType.NVarChar,50),
                new SqlParameter(PARM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter("@TuNgayGoc", SqlDbType.DateTime),
                new SqlParameter("@DenNgayGoc", SqlDbType.DateTime),
                new SqlParameter(PARM_START, SqlDbType.Int),
                new SqlParameter(PARM_END, SqlDbType.Int),
                new SqlParameter(PARM_STATENAME, SqlDbType.NVarChar,100),
                new SqlParameter(PARM_CANBOID, SqlDbType.Int),
                new SqlParameter(PARM_PHONGBANID, SqlDbType.Int),
                new SqlParameter("@TuNgayMoi", SqlDbType.DateTime),
                new SqlParameter("@DenNgayMoi", SqlDbType.DateTime),
                new SqlParameter("@TotalRow",SqlDbType.Int),
                new SqlParameter("@TrangThai",SqlDbType.Int),
                new SqlParameter("@HuongXuLyID",SqlDbType.Int),
                new SqlParameter("@TrangThaiDonThu",SqlDbType.Int),
                new SqlParameter("@ChuTichUBND",SqlDbType.Int),
            };
            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.KeyWord ?? Convert.DBNull;
            parms[2].Value = info.LoaiKhieuToID;
            parms[3].Value = info.TuNgayGoc;
            parms[4].Value = info.DenNgayGoc;
            parms[5].Value = info.Start;
            parms[6].Value = info.End;
            parms[7].Value = info.StateName;
            parms[8].Value = info.CanBoID;
            parms[9].Value = info.PhongBanID;
            parms[10].Value = info.TuNgayMoi;
            parms[11].Value = info.DenNgayMoi;
            parms[12].Direction = ParameterDirection.Output;
            parms[12].Size = 8;
            parms[13].Value = info.TrangThai ?? Convert.DBNull;
            parms[14].Value = info.HuongXuLyID;
            parms[15].Value = info.TrangThaiDonThu ?? Convert.DBNull;
            parms[16].Value = info.ChuTichUBND ?? Convert.DBNull;

            if (info.TuNgayGoc == DateTime.MinValue) parms[3].Value = DBNull.Value;
            if (info.DenNgayGoc == DateTime.MinValue) parms[4].Value = DBNull.Value;
            if (info.TuNgayMoi == DateTime.MinValue) parms[10].Value = DBNull.Value;
            if (info.DenNgayMoi == DateTime.MinValue) parms[11].Value = DBNull.Value;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_PhanGiaiQuyet_GetDonThuCanPhanGiaiQuyetTrongCoQuan_New2", parms))
                {
                    while (dr.Read())
                    {
                        DonThuGiaiQuyetInfo phanGiaiQuyetInfo = GetData(dr);
                        phanGiaiQuyetInfo.TrangThaiDuyet = Utils.ConvertToInt32(dr["TrangThaiDuyet"], 0);

                        phanGiaiQuyetInfo.DuongDanFile = Utils.ConvertToString(dr["FileUrl"], string.Empty);

                        phanGiaiQuyetInfo.TenCoQuanPhanGQ = Utils.ConvertToString(dr["TenCoQuanPhanGQ"], string.Empty);
                        phanGiaiQuyetInfo.TenCoQuanGQ = Utils.ConvertToString(dr["TenCoQuanGQ"], string.Empty);
                        // Lấy cơ quan chuyển đến 1/7/2024 bằng tên nguồn đơn đến
                        var tenNguonDonDen = new DonThuDAL().GetByID(phanGiaiQuyetInfo.DonThuID, phanGiaiQuyetInfo.XuLyDonID).TenNguonDonDen;
                        if (tenNguonDonDen == null || tenNguonDonDen == "")
                        {
                            phanGiaiQuyetInfo.TenNguonDonDen = Utils.GetString(dr["TenNguonDonDen"], string.Empty);
                        }
                        else
                        {
                            phanGiaiQuyetInfo.TenNguonDonDen = tenNguonDonDen + " chuyển đơn";
                        }
                        phanGiaiQuyetInfo.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        phanGiaiQuyetInfo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        phanGiaiQuyetInfo.ChuyenGiaiQuyetID = Utils.ConvertToInt32(dr["CQGiaoID"], 0);
                        phanGiaiQuyetInfo.TheoDoiXuLyID = Utils.ConvertToInt32(dr["TheoDoiXuLyID"], 0);
                        phanGiaiQuyetInfo.TrangThaiPhanHoi = Utils.ConvertToInt32(dr["TrangThaiPhanHoi"], 0);
                        phanGiaiQuyetInfo.TenHuongGiaiQuyet = Utils.ConvertToString(dr["TenHuongGiaiQuyet"], string.Empty);

                        string now = Format.FormatDate(DateTime.Now);
                        DateTime ngayHienTai = Utils.ConvertToDateTime(now, DateTime.MinValue);
                        //TimeSpan ngayConLai = 0;                    
                        phanGiaiQuyetInfo.KetQuaID = Utils.ConvertToInt32(dr["KetQuaID"], 0);
                        phanGiaiQuyetInfo.HanGQGoc = Utils.ConvertToDateTime(dr["HanGQGoc"], DateTime.MinValue);
                        phanGiaiQuyetInfo.HanGQMoi = Utils.ConvertToDateTime(dr["HanGQMoi"], DateTime.MinValue);
                        phanGiaiQuyetInfo.NgayTiep = Utils.ConvertToDateTime(dr["NgayNhapDon"], DateTime.MinValue);
                        phanGiaiQuyetInfo.HanGQQTPhucTap = Utils.ConvertToDateTime(dr["HanGQQTPhucTap"], DateTime.MinValue);
                        phanGiaiQuyetInfo.HanGiaiQuyet = Utils.ConvertToNullableDateTime(dr["HanGQQTPhucTap"], null);
                        phanGiaiQuyetInfo.TiepDanID = Utils.ConvertToInt32(dr["TiepDanKhongDonID"], 0);
                        phanGiaiQuyetInfo.LoaiKhieuTo1ID = Utils.ConvertToInt32(dr["LoaiKhieuTo1ID"], 0);
                        phanGiaiQuyetInfo.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"], 0);
                        phanGiaiQuyetInfo.CQPhoiHopID = Utils.ConvertToInt32(dr["CQPhoiHopID"], 0);
                        if (phanGiaiQuyetInfo.NhomKNID > 0)
                        {
                            phanGiaiQuyetInfo.listDoiTuongKN = new DoiTuongKN().GetByNhomKNID(phanGiaiQuyetInfo.NhomKNID).ToList();
                        }

                        phanGiaiQuyetInfo.LoaiQuyTrinh = Utils.ConvertToInt32(dr["LoaiQuyTrinh"], 0);
                        phanGiaiQuyetInfo.HuongGiaiQuyetID = Utils.ConvertToInt32(dr["HuongGiaiQuyetID"], 0);
                        phanGiaiQuyetInfo.TrinhDuThao = Utils.ConvertToInt32(dr["TrinhDuThao"], 0);
                        phanGiaiQuyetInfo.NgayTiepNhan = Utils.GetDateTime(dr["NgayTiepNhan"], DateTime.MinValue);
                        phanGiaiQuyetInfo.LanhDaoDuyet2ID = Utils.ConvertToInt32(dr["LanhDaoDuyet2ID"], 0);
                        phanGiaiQuyetInfo.KetQuaID = Utils.ConvertToInt32(dr["KetQuaID"], 0);
                        phanGiaiQuyetInfo.CoCapNhapDoanToXacMinh = Utils.ConvertToInt32(dr["CoCapNhapDoanToXacMinh"], 0);
                        // bổ sung thêm rút đơn id
                        phanGiaiQuyetInfo.RutDonID = Utils.ConvertToInt32(dr["RutDonID"], 0);
                        phangiaiquyets.Add(phanGiaiQuyetInfo);
                    }
                    dr.Close();
                }
                TotalRow = Utils.ConvertToInt32(parms[12].Value, 0);
            }
            catch { throw; }
            return phangiaiquyets;
        }
        public IList<DonThuGiaiQuyetInfo> GetDonThuCanPhanGiaiQuyet_PhanHoi_New(QueryFilterInfo info)
        {
            IList<DonThuGiaiQuyetInfo> phangiaiquyets = new List<DonThuGiaiQuyetInfo>();

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARM_STATENAME, SqlDbType.NVarChar,100),
                new SqlParameter(PARM_CANBOID, SqlDbType.Int),
                new SqlParameter(PARM_PHONGBANID, SqlDbType.Int)

            };
            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.StateName;
            parms[2].Value = info.CanBoID;
            parms[3].Value = info.PhongBanID;


            //if (info.TuNgayGoc == DateTime.MinValue) parms[3].Value = DBNull.Value;
            //if (info.DenNgayGoc == DateTime.MinValue) parms[4].Value = DBNull.Value;
            //if (info.TuNgayMoi == DateTime.MinValue) parms[10].Value = DBNull.Value;
            //if (info.DenNgayMoi == DateTime.MinValue) parms[11].Value = DBNull.Value;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "PhanGiaiQuyet_GetDonThuCanPhanGiaiQuyetTrongCoQuan_PhanHoi_New", parms))
                {
                    while (dr.Read())
                    {
                        DonThuGiaiQuyetInfo phanGiaiQuyetInfo = GetData(dr);
                        phanGiaiQuyetInfo.DuongDanFile = Utils.ConvertToString(dr["FileUrl"], string.Empty);

                        phanGiaiQuyetInfo.TenCoQuanPhanGQ = Utils.ConvertToString(dr["TenCoQuanPhanGQ"], string.Empty);
                        phanGiaiQuyetInfo.TenCoQuanGQ = Utils.ConvertToString(dr["TenCoQuanGQ"], string.Empty);
                        phanGiaiQuyetInfo.TenNguonDonDen = Utils.GetString(dr["TenNguonDonDen"], string.Empty);
                        phanGiaiQuyetInfo.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        phanGiaiQuyetInfo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        phanGiaiQuyetInfo.ChuyenGiaiQuyetID = Utils.ConvertToInt32(dr["CQGiaoID"], 0);
                        phanGiaiQuyetInfo.TheoDoiXuLyID = Utils.ConvertToInt32(dr["TheoDoiXuLyID"], 0);
                        phanGiaiQuyetInfo.TrangThaiPhanHoi = Utils.ConvertToInt32(dr["TrangThaiPhanHoi"], 0);


                        string now = Format.FormatDate(DateTime.Now);
                        DateTime ngayHienTai = Utils.ConvertToDateTime(now, DateTime.MinValue);
                        //TimeSpan ngayConLai = 0;

                        phanGiaiQuyetInfo.HanGQGoc = Utils.ConvertToDateTime(dr["HanGQGoc"], DateTime.MinValue);
                        phanGiaiQuyetInfo.HanGQMoi = Utils.ConvertToDateTime(dr["HanGQMoi"], DateTime.MinValue);
                        phanGiaiQuyetInfo.HanGQQTPhucTap = Utils.ConvertToDateTime(dr["HanGQQTPhucTap"], DateTime.MinValue);

                        phangiaiquyets.Add(phanGiaiQuyetInfo);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return phangiaiquyets;
        }
        public IList<DonThuGiaiQuyetInfo> GetDonThuCanPhanGiaiQuyet_PhanHoi_filter(QueryFilterInfo info, bool capUBND, bool QTPhucTap = true)
        {
            IList<DonThuGiaiQuyetInfo> phangiaiquyets = new List<DonThuGiaiQuyetInfo>();

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARM_KEYWORD, SqlDbType.NVarChar,50),
                new SqlParameter(PARM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter("@TuNgayGoc", SqlDbType.DateTime),
                new SqlParameter("@DenNgayGoc", SqlDbType.DateTime),
                new SqlParameter(PARM_START, SqlDbType.Int),
                new SqlParameter(PARM_END, SqlDbType.Int),
                new SqlParameter(PARM_STATENAME, SqlDbType.NVarChar,100),
                new SqlParameter(PARM_CANBOID, SqlDbType.Int),
                new SqlParameter(PARM_PHONGBANID, SqlDbType.Int),
                new SqlParameter("@TuNgayMoi", SqlDbType.DateTime),
                new SqlParameter("@DenNgayMoi", SqlDbType.DateTime),
                new SqlParameter("@IsCapUBND", SqlDbType.Bit),
                new SqlParameter("@QTPhucTap", SqlDbType.Bit),
            };

            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.KeyWord;
            parms[2].Value = info.LoaiKhieuToID;
            parms[3].Value = info.TuNgayGoc;
            parms[4].Value = info.DenNgayGoc;
            parms[5].Value = info.Start;
            parms[6].Value = info.End;
            parms[7].Value = info.StateName;
            parms[8].Value = info.CanBoID;
            parms[9].Value = info.PhongBanID;
            parms[10].Value = info.TuNgayMoi;
            parms[11].Value = info.DenNgayMoi;

            if (info.TuNgayGoc == DateTime.MinValue) parms[3].Value = DBNull.Value;
            if (info.DenNgayGoc == DateTime.MinValue) parms[4].Value = DBNull.Value;
            if (info.TuNgayMoi == DateTime.MinValue) parms[10].Value = DBNull.Value;
            if (info.DenNgayMoi == DateTime.MinValue) parms[11].Value = DBNull.Value;
            parms[12].Value = capUBND;
            parms[13].Value = QTPhucTap;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "PhanGiaiQuyet_GetDonThuCanPhanGiaiQuyetTrongCoQuan_PhanHoi_filter", parms))
                {
                    while (dr.Read())
                    {
                        DonThuGiaiQuyetInfo phanGiaiQuyetInfo = GetData(dr);
                        phanGiaiQuyetInfo.DuongDanFile = Utils.ConvertToString(dr["FileUrl"], string.Empty);

                        phanGiaiQuyetInfo.TenCoQuanPhanGQ = Utils.ConvertToString(dr["TenCoQuanPhanGQ"], string.Empty);
                        phanGiaiQuyetInfo.TenCoQuanGQ = Utils.ConvertToString(dr["TenCoQuanGQ"], string.Empty);
                        // Lấy cơ quan chuyển đến 1/7/2024 bằng tên nguồn đơn đến
                        var tenNguonDonDen = new DonThuDAL().GetByID(phanGiaiQuyetInfo.DonThuID, phanGiaiQuyetInfo.XuLyDonID).TenNguonDonDen;
                        if (tenNguonDonDen == null || tenNguonDonDen == "")
                        {
                            phanGiaiQuyetInfo.TenNguonDonDen = Utils.GetString(dr["TenNguonDonDen"], string.Empty);
                        }
                        else
                        {
                            phanGiaiQuyetInfo.TenNguonDonDen = tenNguonDonDen + " chuyển đơn";
                        }
                        phanGiaiQuyetInfo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        phanGiaiQuyetInfo.ChuyenGiaiQuyetID = Utils.ConvertToInt32(dr["CQGiaoID"], 0);
                        phanGiaiQuyetInfo.TheoDoiXuLyID = Utils.ConvertToInt32(dr["TheoDoiXuLyID"], 0);
                        phanGiaiQuyetInfo.TrangThaiPhanHoi = Utils.ConvertToInt32(dr["TrangThaiPhanHoi"], 0);


                        string now = Format.FormatDate(DateTime.Now);
                        DateTime ngayHienTai = Utils.ConvertToDateTime(now, DateTime.MinValue);
                        //TimeSpan ngayConLai = 0;

                        phanGiaiQuyetInfo.HanGQGoc = Utils.ConvertToDateTime(dr["HanGQGoc"], DateTime.MinValue);
                        phanGiaiQuyetInfo.HanGQMoi = Utils.ConvertToDateTime(dr["HanGQMoi"], DateTime.MinValue);
                        phanGiaiQuyetInfo.HanGQQTPhucTap = Utils.ConvertToDateTime(dr["HanGQQTPhucTap"], DateTime.MinValue);
                        phanGiaiQuyetInfo.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"], 0);
                        if (phanGiaiQuyetInfo.NhomKNID > 0)
                        {
                            phanGiaiQuyetInfo.listDoiTuongKN = new DoiTuongKN().GetByNhomKNID(phanGiaiQuyetInfo.NhomKNID).ToList();
                        }

                        phanGiaiQuyetInfo.LoaiQuyTrinh = Utils.ConvertToInt32(dr["LoaiQuyTrinh"], 0);
                        phanGiaiQuyetInfo.HuongGiaiQuyetID = Utils.ConvertToInt32(dr["HuongGiaiQuyetID"], 0);
                        phanGiaiQuyetInfo.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        phanGiaiQuyetInfo.TrangThaiDuyet = Utils.ConvertToInt32(dr["TrangThaiDuyet"], 0);
                        phanGiaiQuyetInfo.TrinhDuThao = Utils.ConvertToInt32(dr["TrinhDuThao"], 0);
                        phanGiaiQuyetInfo.NgayTiepNhan = Utils.GetDateTime(dr["NgayTiepNhan"], DateTime.MinValue);
                        phanGiaiQuyetInfo.LanhDaoDuyet2ID = Utils.ConvertToInt32(dr["LanhDaoDuyet2ID"], 0);
                        phanGiaiQuyetInfo.KetQuaID = Utils.ConvertToInt32(dr["KetQuaID"], 0);
                        phanGiaiQuyetInfo.CoCapNhapDoanToXacMinh = Utils.ConvertToInt32(dr["CoCapNhapDoanToXacMinh"], 0);
                        // bổ sung trạng thái rút đơn ID
                        phanGiaiQuyetInfo.RutDonID = Utils.ConvertToInt32(dr["RutDonID"], 0);

                        phangiaiquyets.Add(phanGiaiQuyetInfo);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex) { throw; }
            return phangiaiquyets;
        }
        public int GetDonThuCanPhanGiaiQuyet_PhanHoi_filter_Count_Paging(QueryFilterInfo info, bool capUBND, bool QTPhucTap = true)
        {
            IList<DonThuGiaiQuyetInfo> phangiaiquyets = new List<DonThuGiaiQuyetInfo>();

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARM_KEYWORD, SqlDbType.NVarChar,50),
                new SqlParameter(PARM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter("@TuNgayGoc", SqlDbType.DateTime),
                new SqlParameter("@DenNgayGoc", SqlDbType.DateTime),
                new SqlParameter(PARM_STATENAME, SqlDbType.NVarChar,100),
                new SqlParameter(PARM_CANBOID, SqlDbType.Int),
                new SqlParameter(PARM_PHONGBANID, SqlDbType.Int),
                new SqlParameter("@TuNgayMoi", SqlDbType.DateTime),
                new SqlParameter("@DenNgayMoi", SqlDbType.DateTime),
                new SqlParameter("@IsCapUBND", SqlDbType.Bit),
                new SqlParameter("@QTPhucTap", SqlDbType.Bit),
            };

            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.KeyWord;
            parms[2].Value = info.LoaiKhieuToID;
            parms[3].Value = info.TuNgayGoc;
            parms[4].Value = info.DenNgayGoc;
            parms[5].Value = info.StateName;
            parms[6].Value = info.CanBoID;
            parms[7].Value = info.PhongBanID;
            parms[8].Value = info.TuNgayMoi;
            parms[9].Value = info.DenNgayMoi;

            if (info.TuNgayGoc == DateTime.MinValue) parms[3].Value = DBNull.Value;
            if (info.DenNgayGoc == DateTime.MinValue) parms[4].Value = DBNull.Value;
            if (info.TuNgayMoi == DateTime.MinValue) parms[8].Value = DBNull.Value;
            if (info.DenNgayMoi == DateTime.MinValue) parms[9].Value = DBNull.Value;

            parms[10].Value = capUBND;
            parms[11].Value = QTPhucTap;

            int CountNum = 0;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "PhanGiaiQuyet_GetDonThuCanPhanGiaiQuyetTrongCoQuan_PhanHoi_filter_Count_Paging", parms))
                {
                    if (dr.Read())
                    {
                        CountNum = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex) { throw; }
            return CountNum;
        }
        public int GetDonThuCanPhanGiaiQuyet_PhanHoi_filter_Count_Noti(int CoQuanID, int CanBoID, bool capUBND, bool QTPhucTap = true)
        {
            IList<DonThuGiaiQuyetInfo> phangiaiquyets = new List<DonThuGiaiQuyetInfo>();

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@CoQuanID", SqlDbType.Int),
                new SqlParameter("@CanBoID", SqlDbType.Int),
                new SqlParameter("@IsCapUBND", SqlDbType.Bit),
                new SqlParameter("@QTPhucTap", SqlDbType.Bit),
            };
            int Count = 0;
            parms[0].Value = CoQuanID;
            parms[1].Value = CanBoID;
            parms[2].Value = capUBND;
            parms[3].Value = QTPhucTap;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "PhanGiaiQuyet_GetDonThuCanPhanGiaiQuyetTrongCoQuan_PhanHoi_filter_count", parms))
                {
                    if (dr.Read())
                    {
                        Count = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex) { throw; }
            return Count;
        }
        public int CountDonThuCanPhanGiaiQuyet_LD(QueryFilterInfo info)
        {
            int Count = 0;

            SqlParameter[] parms = GetParms_Count_LocDL_LD();

            SetParms_Count_LocDL_LD(parms, info);

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_DONTHU_CANPHANGIAIQUYET_TRONGCOQUAN, parms))
                {
                    if (dr.Read())
                    {
                        Count = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return Count;
        }

        public int CountDonThuChuyenDon_VBDonDoc(QueryFilterInfo info)
        {
            int Count = 0;

            SqlParameter[] parms = GetParms_Count_LocDL_LD();

            SetParms_Count_LocDL_LD(parms, info);

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_DONTHU_CANCHUYENDON_VBDONDOC, parms))
                {
                    if (dr.Read())
                    {
                        Count = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return Count;
        }

        public int CountDonThuCanPhanGiaiQuyet_TP(QueryFilterInfo info)
        {
            int Count = 0;

            SqlParameter[] parms = GetParms_LocDL_TP();

            SetParms_LocDL_TP(parms, info);

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_DONTHU_CANPHANGIAIQUYET_TP, parms))
                {
                    if (dr.Read())
                    {
                        Count = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return Count;
        }

        public IList<DonThuGiaiQuyetInfo> GetDonThuCanPhanGiaiQuyet_TP(QueryFilterInfo info)
        {
            IList<DonThuGiaiQuyetInfo> phangiaiquyets = new List<DonThuGiaiQuyetInfo>();

            SqlParameter[] parms = GetParms_LocDL_TP();

            SetParms_LocDL_TP(parms, info);

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DONTHU_CANPHANGIAIQUYET_TP, parms))
                {
                    while (dr.Read())
                    {
                        DonThuGiaiQuyetInfo phanGiaiQuyetInfo = GetData(dr);
                        phanGiaiQuyetInfo.DuongDanFile = Utils.ConvertToString(dr["FileUrl"], string.Empty);
                        phanGiaiQuyetInfo.TenCoQuanPhanGQ = Utils.ConvertToString(dr["TenCoQuanPhanGQ"], string.Empty);
                        phanGiaiQuyetInfo.TenCoQuanGQ = Utils.ConvertToString(dr["TenCoQuanGQ"], string.Empty);
                        phanGiaiQuyetInfo.TenNguonDonDen = Utils.GetString(dr["TenNguonDonDen"], string.Empty);
                        phanGiaiQuyetInfo.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        phanGiaiQuyetInfo.TheoDoiXuLyID = Utils.ConvertToInt32(dr["TheoDoiXuLyID"], 0);

                        string now = Format.FormatDate(DateTime.Now);
                        DateTime ngayHienTai = Utils.ConvertToDateTime(now, DateTime.MinValue);

                        phanGiaiQuyetInfo.NgayQuaHan = Utils.ConvertToDateTime(dr["HanGiaiQuyetDueDate"], DateTime.MinValue);
                        phanGiaiQuyetInfo.SoNgayConLai = phanGiaiQuyetInfo.NgayQuaHan.Subtract(ngayHienTai).Days;


                        phangiaiquyets.Add(phanGiaiQuyetInfo);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return phangiaiquyets;
        }

        public IList<DonThuGiaiQuyetInfo> GetDonThuCanPhanGiaiQuyet_TP_PhanHoi(QueryFilterInfo info, ref int TotalRow)
        {
            IList<DonThuGiaiQuyetInfo> phangiaiquyets = new List<DonThuGiaiQuyetInfo>();

            //SqlParameter[] parms = GetParms_LocDL_TP();

            //SetParms_LocDL_TP(parms, info);

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARM_KEYWORD, SqlDbType.NVarChar,50),
                new SqlParameter(PARM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter("@TuNgay", SqlDbType.DateTime),
                new SqlParameter("@DenNgay", SqlDbType.DateTime),
                new SqlParameter(PARM_START, SqlDbType.Int),
                new SqlParameter(PARM_END, SqlDbType.Int),
                new SqlParameter(PARM_STATENAME, SqlDbType.NVarChar,100),
                new SqlParameter(PARM_CANBOID, SqlDbType.Int),
                new SqlParameter(PARM_PHONGBANID, SqlDbType.Int),
                new SqlParameter("@TotalRow",SqlDbType.Int),
                new SqlParameter("@TrangThai",SqlDbType.Int),
            };
            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.KeyWord ?? Convert.DBNull;
            parms[2].Value = info.LoaiKhieuToID;
            parms[3].Value = info.TuNgay;
            parms[4].Value = info.DenNgay;
            parms[5].Value = info.Start;
            parms[6].Value = info.End;
            parms[7].Value = info.StateName;
            parms[8].Value = info.CanBoID;
            parms[9].Value = info.PhongBanID;
            parms[10].Direction = ParameterDirection.Output;
            parms[10].Size = 8;
            parms[11].Value = info.TrangThai ?? Convert.DBNull;

            if (info.TuNgay == DateTime.MinValue) parms[3].Value = DBNull.Value;
            if (info.DenNgay == DateTime.MinValue) parms[4].Value = DBNull.Value;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_PhanGiaiQuyet_GetDonThuCanPhanGiaiQuyetTP_PhanHoi", parms))
                {
                    while (dr.Read())
                    {
                        DonThuGiaiQuyetInfo phanGiaiQuyetInfo = GetData(dr);
                        phanGiaiQuyetInfo.DuongDanFile = Utils.ConvertToString(dr["FileUrl"], string.Empty);
                        phanGiaiQuyetInfo.TenCoQuanPhanGQ = Utils.ConvertToString(dr["TenCoQuanPhanGQ"], string.Empty);
                        phanGiaiQuyetInfo.TenCoQuanGQ = Utils.ConvertToString(dr["TenCoQuanGQ"], string.Empty);
                        // Lấy cơ quan chuyển đến 1/7/2024 bằng tên nguồn đơn đến
                        var tenNguonDonDen = new DonThuDAL().GetByID(phanGiaiQuyetInfo.DonThuID, phanGiaiQuyetInfo.XuLyDonID).TenNguonDonDen;
                        if (tenNguonDonDen == null || tenNguonDonDen == "")
                        {
                            phanGiaiQuyetInfo.TenNguonDonDen = Utils.GetString(dr["TenNguonDonDen"], string.Empty);
                        }
                        else
                        {
                            phanGiaiQuyetInfo.TenNguonDonDen = tenNguonDonDen + " chuyển đơn";
                        }
                        phanGiaiQuyetInfo.TenHuongGiaiQuyet = Utils.GetString(dr["TenHuongGiaiQuyet"], string.Empty);
                        phanGiaiQuyetInfo.TheoDoiXuLyID = Utils.ConvertToInt32(dr["TheoDoiXuLyID"], 0);
                        phanGiaiQuyetInfo.TrangThaiPhanHoi = Utils.ConvertToInt32(dr["TrangThaiPhanHoi"], 0);

                        string now = Format.FormatDate(DateTime.Now);
                        DateTime ngayHienTai = Utils.ConvertToDateTime(now, DateTime.MinValue);

                        phanGiaiQuyetInfo.NgayQuaHan = Utils.ConvertToDateTime(dr["HanGiaiQuyetDueDate"], DateTime.MinValue);
                        phanGiaiQuyetInfo.NgayTiep = Utils.ConvertToDateTime(dr["NgayNhapDon"], DateTime.Now);

                        phanGiaiQuyetInfo.NgayQuaHanStr = Format.FormatDate(phanGiaiQuyetInfo.NgayQuaHan);
                        if (phanGiaiQuyetInfo.NgayQuaHan != DateTime.MinValue)
                            phanGiaiQuyetInfo.SoNgayConLai = phanGiaiQuyetInfo.NgayQuaHan.Subtract(ngayHienTai).Days;
                        else
                            phanGiaiQuyetInfo.SoNgayConLai = 5;
                        phanGiaiQuyetInfo.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"], 0);
                        if (phanGiaiQuyetInfo.NhomKNID > 0)
                        {
                            phanGiaiQuyetInfo.listDoiTuongKN = new DoiTuongKN().GetByNhomKNID(phanGiaiQuyetInfo.NhomKNID).ToList();
                        }
                        if (phanGiaiQuyetInfo.NgayQuaHan != DateTime.MinValue)
                        {
                            phanGiaiQuyetInfo.HanXuLy = phanGiaiQuyetInfo.NgayQuaHan;
                        }

                        phanGiaiQuyetInfo.LoaiQuyTrinh = Utils.ConvertToInt32(dr["LoaiQuyTrinh"], 0);
                        phanGiaiQuyetInfo.HuongGiaiQuyetID = Utils.ConvertToInt32(dr["HuongGiaiQuyetID"], 0);
                        phanGiaiQuyetInfo.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        phanGiaiQuyetInfo.TrangThaiDuyet = Utils.ConvertToInt32(dr["TrangThaiDuyet"], 0);
                        phanGiaiQuyetInfo.TrinhDuThao = Utils.ConvertToInt32(dr["TrinhDuThao"], 0);
                        phanGiaiQuyetInfo.ChuyenGiaiQuyetID = Utils.ConvertToInt32(dr["ChuyenGiaiQuyetID"], 0);
                        phanGiaiQuyetInfo.NgayTiepNhan = Utils.GetDateTime(dr["NgayTiepNhan"], DateTime.MinValue);
                        phanGiaiQuyetInfo.KetQuaID = Utils.ConvertToInt32(dr["KetQuaID"], 0);
                        phanGiaiQuyetInfo.CoCapNhapDoanToXacMinh = Utils.ConvertToInt32(dr["CoCapNhapDoanToXacMinh"], 0);

                        // bổ sung rút đơn id
                        phanGiaiQuyetInfo.RutDonID = Utils.ConvertToInt32(dr["RutDonID"], 0);
                        phangiaiquyets.Add(phanGiaiQuyetInfo);
                    }
                    dr.Close();
                }
                TotalRow = Utils.ConvertToInt32(parms[10].Value, 0);
            }
            catch (Exception ex) { throw; }
            return phangiaiquyets;
        }
        public IList<DonThuGiaiQuyetInfo> GetDonThuCanPhanGiaiQuyet_TP_PhanHoi_New(QueryFilterInfo info)
        {
            IList<DonThuGiaiQuyetInfo> phangiaiquyets = new List<DonThuGiaiQuyetInfo>();

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARM_STATENAME, SqlDbType.NVarChar,100),
                new SqlParameter(PARM_CANBOID, SqlDbType.Int),
                new SqlParameter(PARM_PHONGBANID, SqlDbType.Int)
            };


            parms[0].Value = info.CoQuanID;
            parms[7].Value = info.StateName;
            parms[8].Value = info.CanBoID;
            parms[9].Value = info.PhongBanID;


            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "PhanGiaiQuyet_GetDonThuCanPhanGiaiQuyetTP_PhanHoi_New", parms))
                {
                    while (dr.Read())
                    {
                        DonThuGiaiQuyetInfo phanGiaiQuyetInfo = GetData(dr);
                        phanGiaiQuyetInfo.DuongDanFile = Utils.ConvertToString(dr["FileUrl"], string.Empty);
                        phanGiaiQuyetInfo.TenCoQuanPhanGQ = Utils.ConvertToString(dr["TenCoQuanPhanGQ"], string.Empty);
                        phanGiaiQuyetInfo.TenCoQuanGQ = Utils.ConvertToString(dr["TenCoQuanGQ"], string.Empty);
                        phanGiaiQuyetInfo.TenNguonDonDen = Utils.GetString(dr["TenNguonDonDen"], string.Empty);
                        phanGiaiQuyetInfo.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        phanGiaiQuyetInfo.TheoDoiXuLyID = Utils.ConvertToInt32(dr["TheoDoiXuLyID"], 0);
                        phanGiaiQuyetInfo.TrangThaiPhanHoi = Utils.ConvertToInt32(dr["TrangThaiPhanHoi"], 0);

                        string now = Format.FormatDate(DateTime.Now);
                        DateTime ngayHienTai = Utils.ConvertToDateTime(now, DateTime.MinValue);

                        phanGiaiQuyetInfo.NgayQuaHan = Utils.ConvertToDateTime(dr["HanGiaiQuyetDueDate"], DateTime.MinValue);

                        phanGiaiQuyetInfo.NgayQuaHanStr = Format.FormatDate(phanGiaiQuyetInfo.NgayQuaHan);
                        if (phanGiaiQuyetInfo.NgayQuaHan != DateTime.MinValue)
                            phanGiaiQuyetInfo.SoNgayConLai = phanGiaiQuyetInfo.NgayQuaHan.Subtract(ngayHienTai).Days;
                        else
                            phanGiaiQuyetInfo.SoNgayConLai = 5;

                        phangiaiquyets.Add(phanGiaiQuyetInfo);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex) { throw; }
            return phangiaiquyets;
        }

        public int GetDonThuCanPhanGiaiQuyet_TP_PhanHoi_filter_Count_Noti(int CoQuanID, int CanBoID)
        {
            IList<DonThuGiaiQuyetInfo> phangiaiquyets = new List<DonThuGiaiQuyetInfo>();

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@CoQuanID", SqlDbType.Int),
                new SqlParameter("@CanBoID", SqlDbType.Int),
            };
            int Count = 0;
            parms[0].Value = CoQuanID;
            parms[1].Value = CanBoID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "PhanGiaiQuyet_GetDonThuCanPhanGiaiQuyetTP_PhanHoi_filter_Count", parms))
                {
                    if (dr.Read())
                    {
                        Count = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex) { throw; }
            return Count;
        }
        public IList<DonThuGiaiQuyetInfo> GetDonThuCanPhanGiaiQuyet_TP_PhanHoi_filter(QueryFilterInfo info)
        {
            IList<DonThuGiaiQuyetInfo> phangiaiquyets = new List<DonThuGiaiQuyetInfo>();

            SqlParameter[] parms = GetParms_LocDL_TP();

            SetParms_LocDL_TP(parms, info);

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "PhanGiaiQuyet_GetDonThuCanPhanGiaiQuyetTP_PhanHoi_filter", parms))
                {
                    while (dr.Read())
                    {
                        DonThuGiaiQuyetInfo phanGiaiQuyetInfo = GetData(dr);
                        phanGiaiQuyetInfo.DuongDanFile = Utils.ConvertToString(dr["FileUrl"], string.Empty);
                        phanGiaiQuyetInfo.TenCoQuanPhanGQ = Utils.ConvertToString(dr["TenCoQuanPhanGQ"], string.Empty);
                        phanGiaiQuyetInfo.TenCoQuanGQ = Utils.ConvertToString(dr["TenCoQuanGQ"], string.Empty);
                        // Lấy cơ quan chuyển đến 1/7/2024 bằng tên nguồn đơn đến
                        var tenNguonDonDen = new DonThuDAL().GetByID(phanGiaiQuyetInfo.DonThuID, phanGiaiQuyetInfo.XuLyDonID).TenNguonDonDen;
                        if (tenNguonDonDen == null || tenNguonDonDen == "")
                        {
                            phanGiaiQuyetInfo.TenNguonDonDen = Utils.GetString(dr["TenNguonDonDen"], string.Empty);
                        }
                        else
                        {
                            phanGiaiQuyetInfo.TenNguonDonDen = tenNguonDonDen + " chuyển đơn";
                        }
                        phanGiaiQuyetInfo.TheoDoiXuLyID = Utils.ConvertToInt32(dr["TheoDoiXuLyID"], 0);
                        phanGiaiQuyetInfo.TrangThaiPhanHoi = Utils.ConvertToInt32(dr["TrangThaiPhanHoi"], 0);

                        string now = Format.FormatDate(DateTime.Now);
                        DateTime ngayHienTai = Utils.ConvertToDateTime(now, DateTime.MinValue);

                        phanGiaiQuyetInfo.NgayQuaHan = Utils.ConvertToDateTime(dr["HanGiaiQuyetDueDate"], DateTime.MinValue);
                        phanGiaiQuyetInfo.NgayQuaHanStr = Format.FormatDate(phanGiaiQuyetInfo.NgayQuaHan);
                        //phanGiaiQuyetInfo.SoNgayConLai = phanGiaiQuyetInfo.NgayQuaHan.Subtract(ngayHienTai).Days;
                        if (phanGiaiQuyetInfo.NgayQuaHan != DateTime.MinValue)
                            phanGiaiQuyetInfo.SoNgayConLai = phanGiaiQuyetInfo.NgayQuaHan.Subtract(ngayHienTai).Days;
                        else
                            phanGiaiQuyetInfo.SoNgayConLai = 5;
                        phanGiaiQuyetInfo.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"], 0);
                        if (phanGiaiQuyetInfo.NhomKNID > 0)
                        {
                            phanGiaiQuyetInfo.listDoiTuongKN = new DoiTuongKN().GetByNhomKNID(phanGiaiQuyetInfo.NhomKNID).ToList();
                        }

                        phanGiaiQuyetInfo.LoaiQuyTrinh = Utils.ConvertToInt32(dr["LoaiQuyTrinh"], 0);
                        phanGiaiQuyetInfo.HuongGiaiQuyetID = Utils.ConvertToInt32(dr["HuongGiaiQuyetID"], 0);
                        phanGiaiQuyetInfo.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        phanGiaiQuyetInfo.TrangThaiDuyet = Utils.ConvertToInt32(dr["TrangThaiDuyet"], 0);
                        phanGiaiQuyetInfo.TrinhDuThao = Utils.ConvertToInt32(dr["TrinhDuThao"], 0);
                        phanGiaiQuyetInfo.ChuyenGiaiQuyetID = Utils.ConvertToInt32(dr["ChuyenGiaiQuyetID"], 0);
                        phanGiaiQuyetInfo.NgayTiepNhan = Utils.GetDateTime(dr["NgayTiepNhan"], DateTime.MinValue);
                        phanGiaiQuyetInfo.KetQuaID = Utils.ConvertToInt32(dr["KetQuaID"], 0);
                        phanGiaiQuyetInfo.CoCapNhapDoanToXacMinh = Utils.ConvertToInt32(dr["CoCapNhapDoanToXacMinh"], 0);

                        // Bổ sung rút đơn id
                        phanGiaiQuyetInfo.RutDonID = Utils.ConvertToInt32(dr["RutDonID"], 0);
                        phangiaiquyets.Add(phanGiaiQuyetInfo);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex) { throw; }
            return phangiaiquyets;
        }
        public int GetDonThuCanPhanGiaiQuyet_TP_PhanHoi_filter_Count_Paging(QueryFilterInfo info)
        {


            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARM_KEYWORD, SqlDbType.NVarChar,50),
                new SqlParameter(PARM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter("@TuNgay", SqlDbType.DateTime),
                new SqlParameter("@DenNgay", SqlDbType.DateTime),
                new SqlParameter(PARM_STATENAME, SqlDbType.NVarChar,100),
                new SqlParameter(PARM_CANBOID, SqlDbType.Int),
                new SqlParameter(PARM_PHONGBANID, SqlDbType.Int),

            };
            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.KeyWord;
            parms[2].Value = info.LoaiKhieuToID;
            parms[3].Value = info.TuNgay;
            parms[4].Value = info.DenNgay;
            parms[5].Value = info.StateName;
            parms[6].Value = info.CanBoID;
            parms[7].Value = info.PhongBanID;


            if (info.TuNgay == DateTime.MinValue) parms[3].Value = DBNull.Value;
            if (info.DenNgay == DateTime.MinValue) parms[4].Value = DBNull.Value;


            int CountNum = 0;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "PhanGiaiQuyet_GetDonThuCanPhanGiaiQuyetTP_PhanHoi_filter_Count_Paging", parms))
                {
                    if (dr.Read())
                    {
                        CountNum = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex) { throw; }
            return CountNum;
        }

        public int CountDonThuCanPhanGiaiQuyet_CV(QueryFilterInfo info)
        {
            int Count = 0;

            SqlParameter[] parms = GetParms_Count_LocDL_LD();

            SetParms_Count_LocDL_LD(parms, info);

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_DONTHU_CANPHANGIAIQUYET_CV, parms))
                {
                    if (dr.Read())
                    {
                        Count = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return Count;
        }

        public IList<DonThuGiaiQuyetInfo> GetDonThuCanPhanGiaiQuyet_CV(QueryFilterInfo info)
        {
            IList<DonThuGiaiQuyetInfo> phangiaiquyets = new List<DonThuGiaiQuyetInfo>();

            SqlParameter[] parms = GetParms_LocDL_LD();

            SetParms_LocDL_LD(parms, info);

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DONTHU_CANPHANGIAIQUYET_CV, parms))
                {
                    while (dr.Read())
                    {
                        DonThuGiaiQuyetInfo phanGiaiQuyetInfo = GetData(dr);
                        phanGiaiQuyetInfo.DuongDanFile = Utils.ConvertToString(dr["FileUrl"], string.Empty);
                        //phanGiaiQuyetInfo.ExecuteTime = Utils.ConvertToInt32(dr["ExecuteTime"], 1);
                        //phanGiaiQuyetInfo.DaPhanCQKhac = Utils.ConvertToInt32(dr["DaPhanCQKhac"], 1);
                        //phanGiaiQuyetInfo.DaPhanTrongCQ = Utils.ConvertToInt32(dr["DaPhanTrongCQ"], 1);
                        phanGiaiQuyetInfo.TenCoQuanPhanGQ = Utils.ConvertToString(dr["TenCoQuanPhanGQ"], string.Empty);
                        phanGiaiQuyetInfo.TenCoQuanGQ = Utils.ConvertToString(dr["TenCoQuanGQ"], string.Empty);
                        phanGiaiQuyetInfo.TenNguonDonDen = Utils.GetString(dr["TenNguonDonDen"], string.Empty);
                        phanGiaiQuyetInfo.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        phanGiaiQuyetInfo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        phanGiaiQuyetInfo.ChuyenGiaiQuyetID = Utils.ConvertToInt32(dr["CQGiaoID"], 0);
                        phanGiaiQuyetInfo.TheoDoiXuLyID = Utils.ConvertToInt32(dr["TheoDoiXuLyID"], 0);

                        string now = Format.FormatDate(DateTime.Now);
                        DateTime ngayHienTai = Utils.ConvertToDateTime(now, DateTime.MinValue);
                        //TimeSpan ngayConLai = 0;

                        phanGiaiQuyetInfo.HanGQGoc = Utils.ConvertToDateTime(dr["HanGQGoc"], DateTime.MinValue);
                        phanGiaiQuyetInfo.HanGQMoi = Utils.ConvertToDateTime(dr["HanGQMoi"], DateTime.MinValue);

                        phangiaiquyets.Add(phanGiaiQuyetInfo);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return phangiaiquyets;
        }

        public IList<DonThuGiaiQuyetInfo> GetDonThuCanPhanGiaiQuyet_CV_PhanHoi(QueryFilterInfo info)
        {
            IList<DonThuGiaiQuyetInfo> phangiaiquyets = new List<DonThuGiaiQuyetInfo>();

            SqlParameter[] parms = GetParms_LocDL_LD();

            SetParms_LocDL_LD(parms, info);

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "PhanGiaiQuyet_GetDonThuCanPhanGiaiQuyetCV_PhanHoi", parms))
                {
                    while (dr.Read())
                    {
                        DonThuGiaiQuyetInfo phanGiaiQuyetInfo = GetData(dr);
                        phanGiaiQuyetInfo.DuongDanFile = Utils.ConvertToString(dr["FileUrl"], string.Empty);
                        //phanGiaiQuyetInfo.ExecuteTime = Utils.ConvertToInt32(dr["ExecuteTime"], 1);
                        //phanGiaiQuyetInfo.DaPhanCQKhac = Utils.ConvertToInt32(dr["DaPhanCQKhac"], 1);
                        //phanGiaiQuyetInfo.DaPhanTrongCQ = Utils.ConvertToInt32(dr["DaPhanTrongCQ"], 1);
                        phanGiaiQuyetInfo.TenCoQuanPhanGQ = Utils.ConvertToString(dr["TenCoQuanPhanGQ"], string.Empty);
                        phanGiaiQuyetInfo.TenCoQuanGQ = Utils.ConvertToString(dr["TenCoQuanGQ"], string.Empty);
                        phanGiaiQuyetInfo.TenNguonDonDen = Utils.GetString(dr["TenNguonDonDen"], string.Empty);
                        phanGiaiQuyetInfo.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        phanGiaiQuyetInfo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        phanGiaiQuyetInfo.ChuyenGiaiQuyetID = Utils.ConvertToInt32(dr["CQGiaoID"], 0);
                        phanGiaiQuyetInfo.TheoDoiXuLyID = Utils.ConvertToInt32(dr["TheoDoiXuLyID"], 0);
                        phanGiaiQuyetInfo.TrangThaiPhanHoi = Utils.ConvertToInt32(dr["TrangThaiPhanHoi"], 0);


                        string now = Format.FormatDate(DateTime.Now);
                        DateTime ngayHienTai = Utils.ConvertToDateTime(now, DateTime.MinValue);
                        //TimeSpan ngayConLai = 0;

                        phanGiaiQuyetInfo.HanGQGoc = Utils.ConvertToDateTime(dr["HanGQGoc"], DateTime.MinValue);
                        phanGiaiQuyetInfo.HanGQMoi = Utils.ConvertToDateTime(dr["HanGQMoi"], DateTime.MinValue);
                        phanGiaiQuyetInfo.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"], 0);
                        if (phanGiaiQuyetInfo.NhomKNID > 0)
                        {
                            phanGiaiQuyetInfo.listDoiTuongKN = new DoiTuongKN().GetByNhomKNID(phanGiaiQuyetInfo.NhomKNID).ToList();
                        }
                        phangiaiquyets.Add(phanGiaiQuyetInfo);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return phangiaiquyets;
        }
        public IList<DonThuGiaiQuyetInfo> GetDonThuCanPhanGiaiQuyet_CV_PhanHoi_New(QueryFilterInfo info)
        {
            IList<DonThuGiaiQuyetInfo> phangiaiquyets = new List<DonThuGiaiQuyetInfo>();

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARM_STATENAME, SqlDbType.NVarChar,100),
                new SqlParameter(PARM_CANBOID, SqlDbType.Int),
                new SqlParameter(PARM_PHONGBANID, SqlDbType.Int)

            };

            parms[0].Value = info.CoQuanID;
            parms[7].Value = info.StateName;
            parms[8].Value = info.CanBoID;
            parms[9].Value = info.PhongBanID;


            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "PhanGiaiQuyet_GetDonThuCanPhanGiaiQuyetCV_PhanHoi_New", parms))
                {
                    while (dr.Read())
                    {
                        DonThuGiaiQuyetInfo phanGiaiQuyetInfo = GetData(dr);
                        phanGiaiQuyetInfo.DuongDanFile = Utils.ConvertToString(dr["FileUrl"], string.Empty);
                        //phanGiaiQuyetInfo.ExecuteTime = Utils.ConvertToInt32(dr["ExecuteTime"], 1);
                        //phanGiaiQuyetInfo.DaPhanCQKhac = Utils.ConvertToInt32(dr["DaPhanCQKhac"], 1);
                        //phanGiaiQuyetInfo.DaPhanTrongCQ = Utils.ConvertToInt32(dr["DaPhanTrongCQ"], 1);
                        phanGiaiQuyetInfo.TenCoQuanPhanGQ = Utils.ConvertToString(dr["TenCoQuanPhanGQ"], string.Empty);
                        phanGiaiQuyetInfo.TenCoQuanGQ = Utils.ConvertToString(dr["TenCoQuanGQ"], string.Empty);
                        phanGiaiQuyetInfo.TenNguonDonDen = Utils.GetString(dr["TenNguonDonDen"], string.Empty);
                        phanGiaiQuyetInfo.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        phanGiaiQuyetInfo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        phanGiaiQuyetInfo.ChuyenGiaiQuyetID = Utils.ConvertToInt32(dr["CQGiaoID"], 0);
                        phanGiaiQuyetInfo.TheoDoiXuLyID = Utils.ConvertToInt32(dr["TheoDoiXuLyID"], 0);
                        phanGiaiQuyetInfo.TrangThaiPhanHoi = Utils.ConvertToInt32(dr["TrangThaiPhanHoi"], 0);


                        string now = Format.FormatDate(DateTime.Now);
                        DateTime ngayHienTai = Utils.ConvertToDateTime(now, DateTime.MinValue);
                        //TimeSpan ngayConLai = 0;

                        phanGiaiQuyetInfo.HanGQGoc = Utils.ConvertToDateTime(dr["HanGQGoc"], DateTime.MinValue);
                        phanGiaiQuyetInfo.HanGQMoi = Utils.ConvertToDateTime(dr["HanGQMoi"], DateTime.MinValue);

                        phangiaiquyets.Add(phanGiaiQuyetInfo);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return phangiaiquyets;
        }
        public int GetDonThuCanPhanGiaiQuyet_CV_PhanHoi_Filter_Count_Noti(int CoQuanID, int CanBoID)
        {

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@CoQuanID", SqlDbType.Int),
                new SqlParameter("@CanBoID", SqlDbType.Int),
            };
            int Count = 0;
            parms[0].Value = CoQuanID;
            parms[1].Value = CanBoID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "PhanGiaiQuyet_GetDonThuCanPhanGiaiQuyetCV_PhanHoi_Filter_count", parms))
                {
                    if (dr.Read())
                    {
                        Count = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex) { throw; }
            return Count;
        }
        public IList<DonThuGiaiQuyetInfo> GetDonThuCanPhanGiaiQuyet_CV_PhanHoi_filter(QueryFilterInfo info)
        {
            IList<DonThuGiaiQuyetInfo> phangiaiquyets = new List<DonThuGiaiQuyetInfo>();

            SqlParameter[] parms = GetParms_LocDL_LD();

            SetParms_LocDL_LD(parms, info);

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "PhanGiaiQuyet_GetDonThuCanPhanGiaiQuyetCV_PhanHoi_filter", parms))
                {
                    while (dr.Read())
                    {
                        DonThuGiaiQuyetInfo phanGiaiQuyetInfo = GetData(dr);
                        phanGiaiQuyetInfo.DuongDanFile = Utils.ConvertToString(dr["FileUrl"], string.Empty);
                        //phanGiaiQuyetInfo.ExecuteTime = Utils.ConvertToInt32(dr["ExecuteTime"], 1);
                        //phanGiaiQuyetInfo.DaPhanCQKhac = Utils.ConvertToInt32(dr["DaPhanCQKhac"], 1);
                        //phanGiaiQuyetInfo.DaPhanTrongCQ = Utils.ConvertToInt32(dr["DaPhanTrongCQ"], 1);
                        phanGiaiQuyetInfo.TenCoQuanPhanGQ = Utils.ConvertToString(dr["TenCoQuanPhanGQ"], string.Empty);
                        phanGiaiQuyetInfo.TenCoQuanGQ = Utils.ConvertToString(dr["TenCoQuanGQ"], string.Empty);
                        phanGiaiQuyetInfo.TenNguonDonDen = Utils.GetString(dr["TenNguonDonDen"], string.Empty);
                        phanGiaiQuyetInfo.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        phanGiaiQuyetInfo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        phanGiaiQuyetInfo.ChuyenGiaiQuyetID = Utils.ConvertToInt32(dr["CQGiaoID"], 0);
                        phanGiaiQuyetInfo.TheoDoiXuLyID = Utils.ConvertToInt32(dr["TheoDoiXuLyID"], 0);
                        phanGiaiQuyetInfo.TrangThaiPhanHoi = Utils.ConvertToInt32(dr["TrangThaiPhanHoi"], 0);


                        string now = Format.FormatDate(DateTime.Now);
                        DateTime ngayHienTai = Utils.ConvertToDateTime(now, DateTime.MinValue);
                        //TimeSpan ngayConLai = 0;

                        phanGiaiQuyetInfo.HanGQGoc = Utils.ConvertToDateTime(dr["HanGQGoc"], DateTime.MinValue);
                        phanGiaiQuyetInfo.HanGQMoi = Utils.ConvertToDateTime(dr["HanGQMoi"], DateTime.MinValue);
                        phanGiaiQuyetInfo.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"], 0);
                        if (phanGiaiQuyetInfo.NhomKNID > 0)
                        {
                            phanGiaiQuyetInfo.listDoiTuongKN = new DoiTuongKN().GetByNhomKNID(phanGiaiQuyetInfo.NhomKNID).ToList();
                        }
                        phangiaiquyets.Add(phanGiaiQuyetInfo);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return phangiaiquyets;
        }
        #region == Don thu can giai quyet ==
        public IList<DonThuGiaiQuyetInfo> GetDonThuCanGiaiQuyet(QueryFilterInfo info, int canboid)
        {
            IList<DonThuGiaiQuyetInfo> ListInfo = new List<DonThuGiaiQuyetInfo>();

            SqlParameter[] parms = GetParms_DTCanGiaiQuyet();

            SetParms_DTCanGiaiQuyet(parms, info, canboid);

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DS_DONTHU_CANGIAIQUYET, parms))
                {
                    while (dr.Read())
                    {
                        DonThuGiaiQuyetInfo dtinfo = GetDataCTCanGiaiQuyet(dr);
                        dtinfo.CoQuanGiaoID = Utils.ConvertToInt32(dr["CoQuanGiaoID"], 0);
                        dtinfo.HanGQ = Utils.ConvertToDateTime(dr["HanGiaiQuyetNew"], DateTime.MinValue);
                        dtinfo.HanGQStr = dtinfo.HanGQ.ToString("dd/MM/yyyy");
                        dtinfo.NguonDonDen = Utils.ConvertToInt32(dr["NguonDonDen"], 0);
                        dtinfo.StateName = Utils.GetString(dr["StateName"], string.Empty);
                        dtinfo.StateID = Utils.GetInt32(dr["StateID"], 0);
                        dtinfo.NgayCapNhat = Utils.ConvertToDateTime(dr["NgayCapNhat"], DateTime.MinValue);
                        dtinfo.NgayCapNhatStr = Format.FormatDate(dtinfo.NgayCapNhat);

                        dtinfo.SoCVBaoCaoGQ = Utils.ConvertToString(dr["SoCVBaoCaoGQ"], string.Empty);
                        dtinfo.NgayCVBaoCaoGQ = Utils.ConvertToDateTime(dr["NgayCVBaoCaoGQ"], DateTime.MinValue);

                        string now = Format.FormatDate(DateTime.Now);
                        DateTime ngayHienTai = Utils.ConvertToDateTime(now, DateTime.MinValue);

                        DateTime ngayQuaHan = Utils.ConvertToDateTime(dr["HanGiaiQuyetNew"], DateTime.MinValue);
                        TimeSpan ngayConLai = ngayQuaHan.Subtract(ngayHienTai);
                        dtinfo.NgayGQConLai = ngayConLai.Days;

                        //dtinfo.RutDonID = Utils.GetInt32(dr["RutDonID"], 0);
                        ListInfo.Add(dtinfo);
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
        public IList<DonThuGiaiQuyetInfo> GetDonThuCanGiaiQuyet_PhanHoi(QueryFilterInfo info, int canboid, ref int TotalRow)
        {
            IList<DonThuGiaiQuyetInfo> ListInfo = new List<DonThuGiaiQuyetInfo>();

            //SqlParameter[] parms = GetParms_DTCanGiaiQuyet();

            //SetParms_DTCanGiaiQuyet(parms, info, canboid);

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARM_KEYWORD, SqlDbType.NVarChar,50),
                new SqlParameter(PARM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter(PARM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARM_START, SqlDbType.Int),
                new SqlParameter(PARM_END, SqlDbType.Int),
                new SqlParameter(PARM_CANBOID,SqlDbType.Int),
                new SqlParameter("TrangThai",SqlDbType.Int),
                new SqlParameter("@TotalRow",SqlDbType.Int),
                new SqlParameter("TrangThaiDonThu",SqlDbType.Int),
            };
            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.KeyWord ?? Convert.DBNull;
            parms[2].Value = info.LoaiKhieuToID;
            parms[3].Value = info.TuNgay ?? Convert.DBNull;
            parms[4].Value = info.DenNgay ?? Convert.DBNull;
            parms[5].Value = info.Start;
            parms[6].Value = info.End;
            parms[7].Value = canboid;
            parms[8].Value = info.TrangThai ?? Convert.DBNull;
            parms[9].Direction = ParameterDirection.Output;
            parms[9].Size = 8;
            parms[10].Value = info.TrangThaiDonThu ?? Convert.DBNull;

            if (info.TuNgay == DateTime.MinValue) parms[3].Value = DBNull.Value;
            if (info.DenNgay == DateTime.MinValue) parms[4].Value = DBNull.Value;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_XuLyDon_GetDonThuCanGiaiQuyet_PhanHoi_New", parms))
                {
                    while (dr.Read())
                    {
                        DonThuGiaiQuyetInfo dtinfo = GetDataCTCanGiaiQuyet(dr);
                        dtinfo.CoQuanGiaoID = Utils.ConvertToInt32(dr["CoQuanGiaoID"], 0);
                        dtinfo.HanGQ = Utils.ConvertToDateTime(dr["HanGiaiQuyetNew"], DateTime.MinValue);
                        dtinfo.HanGQStr = dtinfo.HanGQ.ToString("dd/MM/yyyy");
                        dtinfo.NguonDonDen = Utils.ConvertToInt32(dr["NguonDonDen"], 0);
                        // Lấy cơ quan chuyển đến 1/7/2024 bằng tên nguồn đơn đến
                        var tenNguonDonDen = new DonThuDAL().GetByID(dtinfo.DonThuID, dtinfo.XuLyDonID).TenNguonDonDen;
                        if (tenNguonDonDen == null || tenNguonDonDen == "")
                        {
                            dtinfo.TenNguonDonDen = Utils.GetString(dr["TenNguonDonDen"], string.Empty);
                        }
                        else
                        {
                            dtinfo.TenNguonDonDen = tenNguonDonDen + " chuyển đơn";
                        }
                        dtinfo.TenHuongGiaiQuyet = Utils.GetString(dr["TenHuongGiaiQuyet"], string.Empty);
                        dtinfo.StateName = Utils.GetString(dr["StateName"], string.Empty);
                        dtinfo.NgayCapNhat = Utils.ConvertToDateTime(dr["NgayCapNhat"], DateTime.MinValue);
                        dtinfo.NgayCapNhatStr = Format.FormatDate(dtinfo.NgayCapNhat);

                        dtinfo.SoCVBaoCaoGQ = Utils.ConvertToString(dr["SoCVBaoCaoGQ"], string.Empty);
                        dtinfo.TrangThaiPhanHoi = Utils.ConvertToInt32(dr["TrangThaiPhanHoi"], 0);
                        dtinfo.PhanLoaiPhanHoi = Utils.ConvertToInt32(dr["PhanLoaiPhanHoi"], 0);

                        dtinfo.NgayCVBaoCaoGQ = Utils.ConvertToDateTime(dr["NgayCVBaoCaoGQ"], DateTime.MinValue);
                        dtinfo.NgayTiep = Utils.ConvertToDateTime(dr["NgayNhapDon"], DateTime.MinValue);

                        if (dtinfo.HanGiaiQuyet != DateTime.MinValue)
                        {
                            dtinfo.HanXuLy = dtinfo.HanGiaiQuyet;
                        }

                        string now = Format.FormatDate(DateTime.Now);
                        DateTime ngayHienTai = Utils.ConvertToDateTime(now, DateTime.MinValue);

                        DateTime ngayQuaHan = Utils.ConvertToDateTime(dr["HanGiaiQuyetNew"], DateTime.MinValue);
                        TimeSpan ngayConLai = ngayQuaHan.Subtract(ngayHienTai);
                        dtinfo.NgayGQConLai = ngayConLai.Days;
                        //dtinfo.RutDonID = Utils.GetInt32(dr["RutDonID"], 0);

                        dtinfo.LoaiQuyTrinh = Utils.ConvertToInt32(dr["LoaiQuyTrinh"], 0);
                        dtinfo.HuongGiaiQuyetID = Utils.ConvertToInt32(dr["HuongGiaiQuyetID"], 0);
                        dtinfo.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        dtinfo.TrangThaiDuyet = Utils.ConvertToInt32(dr["TrangThaiDuyet"], 0);
                        dtinfo.TrinhDuThao = Utils.ConvertToInt32(dr["TrinhDuThao"], 0);
                        dtinfo.ChuyenGiaiQuyetID = Utils.ConvertToInt32(dr["ChuyenGiaiQuyetID"], 0);
                        dtinfo.NgayTiepNhan = Utils.GetDateTime(dr["NgayTiepNhan"], DateTime.MinValue);
                        dtinfo.KetQuaID = Utils.ConvertToInt32(dr["KetQuaID"], 0);
                        // bổ sung rút đơn id
                        dtinfo.RutDonID = Utils.ConvertToInt32(dr["RutDonID"], 0);
                        ListInfo.Add(dtinfo);
                    }
                    dr.Close();
                }
                TotalRow = Utils.ConvertToInt32(parms[9].Value, 0);
            }
            catch
            {
                throw;
            }
            return ListInfo;
        }
        public IList<DonThuGiaiQuyetInfo> GetDonThuCanGiaiQuyet_PhanHoi_New(QueryFilterInfo info, int canboid)
        {
            IList<DonThuGiaiQuyetInfo> ListInfo = new List<DonThuGiaiQuyetInfo>();

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_COQUANID, SqlDbType.Int),


                new SqlParameter(PARM_CANBOID,SqlDbType.Int)
            };

            parms[0].Value = info.CoQuanID;
            parms[1].Value = canboid;



            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "XuLyDon_GetDonThuCanGiaiQuyet_PhanHoi_New", parms))
                {
                    while (dr.Read())
                    {
                        DonThuGiaiQuyetInfo dtinfo = GetDataCTCanGiaiQuyet(dr);
                        dtinfo.CoQuanGiaoID = Utils.ConvertToInt32(dr["CoQuanGiaoID"], 0);
                        dtinfo.HanGQ = Utils.ConvertToDateTime(dr["HanGiaiQuyetNew"], DateTime.MinValue);
                        dtinfo.HanGQStr = dtinfo.HanGQ.ToString("dd/MM/yyyy");
                        dtinfo.NguonDonDen = Utils.ConvertToInt32(dr["NguonDonDen"], 0);
                        dtinfo.StateName = Utils.GetString(dr["StateName"], string.Empty);
                        dtinfo.StateID = Utils.GetInt32(dr["StateID"], 0);
                        dtinfo.NgayCapNhat = Utils.ConvertToDateTime(dr["NgayCapNhat"], DateTime.MinValue);
                        dtinfo.NgayCapNhatStr = Format.FormatDate(dtinfo.NgayCapNhat);

                        dtinfo.SoCVBaoCaoGQ = Utils.ConvertToString(dr["SoCVBaoCaoGQ"], string.Empty);
                        dtinfo.TrangThaiPhanHoi = Utils.ConvertToInt32(dr["TrangThaiPhanHoi"], 0);
                        dtinfo.PhanLoaiPhanHoi = Utils.ConvertToInt32(dr["PhanLoaiPhanHoi"], 0);

                        dtinfo.NgayCVBaoCaoGQ = Utils.ConvertToDateTime(dr["NgayCVBaoCaoGQ"], DateTime.MinValue);


                        string now = Format.FormatDate(DateTime.Now);
                        DateTime ngayHienTai = Utils.ConvertToDateTime(now, DateTime.MinValue);

                        DateTime ngayQuaHan = Utils.ConvertToDateTime(dr["HanGiaiQuyetNew"], DateTime.MinValue);
                        TimeSpan ngayConLai = ngayQuaHan.Subtract(ngayHienTai);
                        dtinfo.NgayGQConLai = ngayConLai.Days;

                        //dtinfo.RutDonID = Utils.GetInt32(dr["RutDonID"], 0);
                        ListInfo.Add(dtinfo);
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
        public int CountDonThuCanGiaiQuyet(QueryFilterInfo info, int canboid)
        {
            int Count = 0;

            SqlParameter[] parms = GetParms_DTCanGiaiQuyet();

            SetParms_DTCanGiaiQuyet(parms, info, canboid);

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_DONTHU_CANGIAIQUYET, parms))
                {
                    if (dr.Read())
                    {
                        Count = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return Count;
        }
        #endregion
        //Lay danh sach cac don thu can phan giai quyet trong phong/ban
        //public IList<DonThuGiaiQuyetInfo> GetDonThuCanGiaiQuyetTheoPhong(int phongID)
        //{
        //    IList<DonThuGiaiQuyetInfo> phangiaiquyets = new List<DonThuGiaiQuyetInfo>();

        //    SqlParameter[] parms = new SqlParameter[] {
        //        new SqlParameter(PARM_PHONGBANID, SqlDbType.Int)               
        //    };

        //    parms[0].Value = phongID;

        //    try
        //    {
        //        using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DONTHU_CANGIAIQUYET_TRONGPHONG, parms))
        //        {
        //            while (dr.Read())
        //            {
        //                DonThuGiaiQuyetInfo phanGiaiQuyetInfo = GetDataPhongBan(dr);
        //                phangiaiquyets.Add(phanGiaiQuyetInfo);
        //            }
        //            dr.Close();
        //        }
        //    }
        //    catch { throw; }
        //    return phangiaiquyets;
        //}

        public IList<DonThuGiaiQuyetInfo> GetDonThuCanDuyet(int coquanID)
        {
            IList<DonThuGiaiQuyetInfo> phangiaiquyets = new List<DonThuGiaiQuyetInfo>();

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_COQUANID, SqlDbType.Int)
            };

            parms[0].Value = coquanID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DONTHU_CANDUYET_TRONGCOQUAN, parms))
                {
                    while (dr.Read())
                    {
                        DonThuGiaiQuyetInfo phanGiaiQuyetInfo = GetDataCanDuyet(dr);
                        phangiaiquyets.Add(phanGiaiQuyetInfo);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return phangiaiquyets;
        }

        public IList<DonThuGiaiQuyetInfo> GetDonThuCanDuyetTheoPhong(int phongID)
        {
            IList<DonThuGiaiQuyetInfo> phangiaiquyets = new List<DonThuGiaiQuyetInfo>();

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_PHONGBANID, SqlDbType.Int)
            };

            parms[0].Value = phongID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DONTHU_CANDUYET_TRONGPHONG, parms))
                {
                    while (dr.Read())
                    {
                        DonThuGiaiQuyetInfo phanGiaiQuyetInfo = GetDataCanDuyet(dr);
                        phangiaiquyets.Add(phanGiaiQuyetInfo);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return phangiaiquyets;
        }

        public DonThuGiaiQuyetInfo GetByID(int phangiaiquyetID)
        {
            DonThuGiaiQuyetInfo phanGiaiQuyetInfo = null;
            SqlParameter[] parameters = new SqlParameter[] {
new SqlParameter(PARM_PHANGIAIQUYETID, SqlDbType.Int) };
            parameters[0].Value = phangiaiquyetID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_ID, parameters))
                {
                    if (dr.Read())
                    {
                        phanGiaiQuyetInfo = GetBasicData(dr);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return phanGiaiQuyetInfo;
        }

        public int Delete(int phangiaiquyetID)
        {
            int val = 0;
            SqlParameter[] parameters = new SqlParameter[] {
new SqlParameter(PARM_PHANGIAIQUYETID, SqlDbType.Int) };
            parameters[0].Value = phangiaiquyetID;
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

        public int Update(DonThuGiaiQuyetInfo phanGiaiQuyetInfo)
        {
            int val = 0;
            SqlParameter[] parameters = GetUpdateParms();
            SetUpdateParms(parameters, phanGiaiQuyetInfo);
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


        public int Insert(DonThuGiaiQuyetInfo phanGiaiQuyetInfo)
        {
            object val = null;
            SqlParameter[] parameters = GetInsertParms();
            SetInsertParms(parameters, phanGiaiQuyetInfo);
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


        // Don Thu Dang Giai Quyet
        public SqlParameter[] GetParmsDonThuDangGiaiQuyet()
        {
            SqlParameter parm = new SqlParameter(PARM_DOCUMENT_ID_LIST, SqlDbType.Structured);
            parm.TypeName = "IntList";

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARM_KEYWORD, SqlDbType.NVarChar,50),
                new SqlParameter(PARM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter(PARM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARM_START, SqlDbType.Int),
                new SqlParameter(PARM_END, SqlDbType.Int),
                parm

            };
            return parms;
        }

        public void SetParmsDonThuDangGiaiQuyet(SqlParameter[] parms, QueryFilterInfo info, List<int> documentIDlist)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("n", typeof(int));

            foreach (var docID in documentIDlist)
            {
                dataTable.Rows.Add(docID);
            }

            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.KeyWord;
            parms[2].Value = info.LoaiKhieuToID;
            parms[2].Value = info.LoaiKhieuToID;

            parms[3].Value = info.TuNgay;
            parms[4].Value = info.DenNgay;

            if (info.TuNgay == DateTime.MinValue)
            {
                parms[3].Value = DBNull.Value;
            }
            if (info.DenNgay == DateTime.MinValue)
            {
                parms[4].Value = DBNull.Value;
            }

            parms[5].Value = info.Start;
            parms[6].Value = info.End;
            parms[7].Value = dataTable;

        }
        #region == Danh sach don thu dang giai quyet ==
        #region == Dieu kien loc ==
        #region== Lanh dao ==
        public SqlParameter[] GetPara_Count_LanhDao()
        {
            SqlParameter para = new SqlParameter(PARM_DOCUMENT_ID_LIST, SqlDbType.Structured);
            para.TypeName = "IntList";

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARM_KEYWORD, SqlDbType.NVarChar,50),
                new SqlParameter(PARM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter(PARM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARM_CANBOID,SqlDbType.Int),
                para
            }; return parms;
        }

        public void SetPara_Count_LanhDao(SqlParameter[] parms, QueryFilterInfo info, List<int> documentIDlist)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("n", typeof(int));

            foreach (var docID in documentIDlist)
            {
                dataTable.Rows.Add(docID);
            }

            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.KeyWord;
            parms[2].Value = info.LoaiKhieuToID;
            parms[3].Value = info.TuNgay;
            parms[4].Value = info.DenNgay;
            parms[5].Value = info.CanBoID;
            parms[6].Value = dataTable;

            if (info.TuNgay == DateTime.MinValue) parms[3].Value = DBNull.Value;
            if (info.DenNgay == DateTime.MinValue) parms[4].Value = DBNull.Value;
        }

        public SqlParameter[] GetPara_Search_LanhDao()
        {
            SqlParameter para = new SqlParameter(PARM_DOCUMENT_ID_LIST, SqlDbType.Structured);
            para.TypeName = "IntList";

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARM_KEYWORD, SqlDbType.NVarChar,50),
                new SqlParameter(PARM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter(PARM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARM_START, SqlDbType.Int),
                new SqlParameter(PARM_END, SqlDbType.Int),
                new SqlParameter(PARM_CANBOID, SqlDbType.Int),
                para,

            }; return parms;
        }

        public void SetPara_Search_LanhDao(SqlParameter[] parms, QueryFilterInfo info, List<int> documentIDlist)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("n", typeof(int));

            foreach (var docID in documentIDlist)
            {
                dataTable.Rows.Add(docID);
            }

            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.KeyWord;
            parms[2].Value = info.LoaiKhieuToID;
            parms[3].Value = info.TuNgay;
            parms[4].Value = info.DenNgay;
            parms[5].Value = info.Start;
            parms[6].Value = info.End;
            parms[7].Value = info.CanBoID;
            parms[8].Value = dataTable;

            if (info.TuNgay == DateTime.MinValue) parms[3].Value = DBNull.Value;
            if (info.DenNgay == DateTime.MinValue) parms[4].Value = DBNull.Value;
        }



        #endregion
        #region == truong phong ==
        public SqlParameter[] GetPara_Count_ChuyenVien()
        {
            SqlParameter para = new SqlParameter(PARM_DOCUMENT_ID_LIST, SqlDbType.Structured);
            para.TypeName = "IntList";

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARM_KEYWORD, SqlDbType.NVarChar,50),
                new SqlParameter(PARM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter(PARM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARM_CANBOID,SqlDbType.Int),
                para
            }; return parms;
        }

        public SqlParameter[] GetPara_Search_ChuyenVien()
        {
            SqlParameter para = new SqlParameter(PARM_DOCUMENT_ID_LIST, SqlDbType.Structured);
            para.TypeName = "IntList";

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARM_KEYWORD, SqlDbType.NVarChar,50),
                new SqlParameter(PARM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter(PARM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARM_START, SqlDbType.Int),
                new SqlParameter(PARM_END, SqlDbType.Int),
                new SqlParameter(PARM_CANBOID,SqlDbType.Int),
                para
            }; return parms;
        }

        public void SetPara_Count_ChuyenVien(SqlParameter[] parms, QueryFilterInfo info, List<int> documentIDlist)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("n", typeof(int));

            foreach (var docID in documentIDlist)
            {
                dataTable.Rows.Add(docID);
            }

            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.KeyWord;
            parms[2].Value = info.LoaiKhieuToID;
            parms[3].Value = info.TuNgay;
            parms[4].Value = info.DenNgay;
            parms[5].Value = info.CanBoID;
            parms[6].Value = dataTable;

            if (info.TuNgay == DateTime.MinValue) parms[3].Value = DBNull.Value;
            if (info.DenNgay == DateTime.MinValue) parms[4].Value = DBNull.Value;
        }

        public void SetPara_Search_ChuyenVien(SqlParameter[] parms, QueryFilterInfo info, List<int> documentIDlist)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("n", typeof(int));

            foreach (var docID in documentIDlist)
            {
                dataTable.Rows.Add(docID);
            }

            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.KeyWord;
            parms[2].Value = info.LoaiKhieuToID;
            parms[3].Value = info.TuNgay;
            parms[4].Value = info.DenNgay;
            parms[5].Value = info.Start;
            parms[6].Value = info.End;
            parms[7].Value = info.CanBoID;

            parms[8].Value = dataTable;

            if (info.TuNgay == DateTime.MinValue) parms[3].Value = DBNull.Value;
            if (info.DenNgay == DateTime.MinValue) parms[4].Value = DBNull.Value;
        }

        #endregion
        #endregion
        public List<DonThuGiaiQuyetInfo> GetDTDangGiaiQuyet_LanhDao(QueryFilterInfo info, List<int> documentIDList)
        {
            List<DonThuGiaiQuyetInfo> xldList = new List<DonThuGiaiQuyetInfo>();
            SqlParameter[] parms = GetPara_Search_LanhDao();
            SetPara_Search_LanhDao(parms, info, documentIDList);
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DONTHU_DANGGIAIQUYET_LANHDAO, parms))
                {
                    while (dr.Read())
                    {
                        DonThuGiaiQuyetInfo phanGiaiQuyetInfo = GetDataDangGiaiQuyet(dr);
                        xldList.Add(phanGiaiQuyetInfo);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return xldList;
        }

        public int CountDTDangGiaiQuyet_LanhDao(QueryFilterInfo info, List<int> docIDList)
        {
            int Count = 0;

            SqlParameter[] parms = GetPara_Count_LanhDao();

            SetPara_Count_LanhDao(parms, info, docIDList);

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_DONTHU_DANGGIAIQUYET_LANHDAO, parms))
                {
                    if (dr.Read())
                    {
                        Count = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return Count;
        }
        public List<DonThuGiaiQuyetInfo> GetDTDangGiaiQuyet_ChuyenVien(QueryFilterInfo info, List<int> documentIDList)
        {
            List<DonThuGiaiQuyetInfo> xldList = new List<DonThuGiaiQuyetInfo>();
            SqlParameter[] parms = GetPara_Search_ChuyenVien();
            SetPara_Search_ChuyenVien(parms, info, documentIDList);
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DONTHU_DANGGIAIQUYET_CHUYENVIEN, parms))
                {
                    while (dr.Read())
                    {
                        DonThuGiaiQuyetInfo phanGiaiQuyetInfo = GetDataDangGiaiQuyet(dr);
                        xldList.Add(phanGiaiQuyetInfo);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return xldList;
        }

        public int CountDTDangGiaiQuyet_ChuyenVien(QueryFilterInfo info, List<int> docIDList)
        {
            int Count = 0;

            SqlParameter[] parms = GetPara_Count_ChuyenVien();

            SetPara_Count_ChuyenVien(parms, info, docIDList);

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_DONTHU_DANGGIAIQUYET_CHUYENVIEN, parms))
                {
                    if (dr.Read())
                    {
                        Count = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return Count;
        }

        #endregion
        // Can duyet giai quyet
        public List<DonThuGiaiQuyetInfo> GetDTDuyetGiaiQuyet(QueryFilterInfo info, List<int> documentIDList)
        {
            List<DonThuGiaiQuyetInfo> xldList = new List<DonThuGiaiQuyetInfo>();
            SqlParameter[] parms = GetPara_Search_LanhDao();
            SetPara_Search_LanhDao(parms, info, documentIDList);
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DONTHU_CANDUYET_GIAIQUYET_NEW, parms))
                {
                    while (dr.Read())
                    {
                        DonThuGiaiQuyetInfo phanGiaiQuyetInfo = GetDataDuyetGiaiQuyet(dr);
                        string now = Format.FormatDate(DateTime.Now);
                        DateTime ngayHienTai = Utils.ConvertToDateTime(now, DateTime.MinValue);
                        phanGiaiQuyetInfo.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        DateTime ngayQuaHan = Utils.ConvertToDateTime(dr["HanGiaiQuyetNew"], DateTime.MinValue);
                        TimeSpan ngayConLai = ngayQuaHan.Subtract(ngayHienTai);
                        phanGiaiQuyetInfo.NgayGQConLai = ngayConLai.Days;
                        phanGiaiQuyetInfo.ChuyenGiaiQuyetID = Utils.ConvertToInt32(dr["ChuyenGiaiQuyetID"], 0);
                        phanGiaiQuyetInfo.Count = Utils.ConvertToInt32(dr["CountNum"], 0);
                        phanGiaiQuyetInfo.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"], 0);
                        xldList.Add(phanGiaiQuyetInfo);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return xldList;
        }

        public int CountDTDuyetGiaiQuyet(QueryFilterInfo info, List<int> docIDList)
        {
            int Count = 0;

            SqlParameter[] parms = GetPara_Count_LanhDao();

            SetPara_Count_LanhDao(parms, info, docIDList);

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_DONTHU_CANDUYET_GIAIQUYET, parms))
                {
                    if (dr.Read())
                    {
                        Count = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return Count;
        }

        public int CountDTDuyetGiaiQuyetTP(QueryFilterInfo info, List<int> docIDList)
        {
            int Count = 0;

            SqlParameter[] parms = GetPara_Count_LanhDao();

            SetPara_Count_LanhDao(parms, info, docIDList);

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_DONTHU_CANDUYET_GIAIQUYET_TP, parms))
                {
                    if (dr.Read())
                    {
                        Count = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return Count;
        }

        public List<DonThuGiaiQuyetInfo> GetDTDuyetGiaiQuyet_TP(QueryFilterInfo info, List<int> documentIDList)
        {
            List<DonThuGiaiQuyetInfo> xldList = new List<DonThuGiaiQuyetInfo>();
            SqlParameter[] parms = GetPara_Search_LanhDao();
            SetPara_Search_LanhDao(parms, info, documentIDList);
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DONTHU_CANDUYET_GIAIQUYET_TP_NEW, parms))
                {
                    while (dr.Read())
                    {
                        DonThuGiaiQuyetInfo phanGiaiQuyetInfo = GetDataDuyetGiaiQuyet(dr);
                        string now = Format.FormatDate(DateTime.Now);
                        DateTime ngayHienTai = Utils.ConvertToDateTime(now, DateTime.MinValue);
                        phanGiaiQuyetInfo.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        DateTime ngayQuaHan = Utils.ConvertToDateTime(dr["HanGiaiQuyetNew"], DateTime.MinValue);
                        TimeSpan ngayConLai = ngayQuaHan.Subtract(ngayHienTai);
                        phanGiaiQuyetInfo.NgayGQConLai = ngayConLai.Days;
                        phanGiaiQuyetInfo.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"], 0);
                        xldList.Add(phanGiaiQuyetInfo);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return xldList;
        }


        public List<DonThuGiaiQuyetInfo> GetDTDaRut(QueryFilterInfo info)
        {
            List<DonThuGiaiQuyetInfo> xldList = new List<DonThuGiaiQuyetInfo>();
            SqlParameter[] parms = GetParam_RutDon();
            SetParam_RutDon(parms, info);
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DONTHU_DARUT, parms))
                {
                    while (dr.Read())
                    {
                        DonThuGiaiQuyetInfo rutDonInfo = GetDataRutDon(dr);
                        xldList.Add(rutDonInfo);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return xldList;
        }

        public int CountDTDaRut(QueryFilterInfo info)
        {
            int Count = 0;

            SqlParameter[] parms = GetParam_CountRutDon();

            SetParam_CountRutDon(parms, info);

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_DONTHU_DARUT, parms))
                {
                    if (dr.Read())
                    {
                        Count = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return Count;
        }


        public SqlParameter[] GetParmsQuaTrinhGiaiQuyetInfo()
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_XULYDONID, SqlDbType.Int),
            };
            return parms;
        }

        public SqlParameter[] GetParmsChiTietBuocQuaTrinhGiaiQuyet()
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_XULYDONID, SqlDbType.Int),
                new SqlParameter(PARM_TRANGTHAIXULYID, SqlDbType.Int),
            };
            return parms;
        }

        public void SetParmsQuaTrinhGiaiQuyetInfo(SqlParameter[] parms, int xulydonid)
        {
            parms[0].Value = xulydonid;

        }

        public void SetParmsChiTietBuocQuaTrinhGiaiQuyet(SqlParameter[] parms, int xulydonid, int trangthaixulyid)
        {
            parms[0].Value = xulydonid;
            parms[1].Value = trangthaixulyid;

        }
        public DonThuGiaiQuyetInfo GetQuaTrinhGiaiQuyetInfo(int xulydonid, int VaiTro)
        {
            DonThuGiaiQuyetInfo info = new DonThuGiaiQuyetInfo();
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_XULYDONID, SqlDbType.Int),
                new SqlParameter(PARM_VAITRO, SqlDbType.TinyInt),

            };
            parms[0].Value = xulydonid;
            parms[1].Value = Convert.ToByte(VaiTro);
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DONTHU_GIAIQUYET_INFO, parms))
                {
                    while (dr.Read())
                    {
                        info = GetQuaTrinhGiaiQuyetInfo(dr);

                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return info;
        }


        public List<DonThuGiaiQuyetInfo> GetQuaTrinhGiaiQuyet_DataTable(int xulydonid)
        {
            List<DonThuGiaiQuyetInfo> xldList = new List<DonThuGiaiQuyetInfo>();
            SqlParameter[] parms = GetParmsQuaTrinhGiaiQuyetInfo();
            SetParmsQuaTrinhGiaiQuyetInfo(parms, xulydonid);
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DONTHU_GIAIQUYET_DATATABLE, parms))
                {
                    while (dr.Read())
                    {
                        DonThuGiaiQuyetInfo phanGiaiQuyetInfo = GetQuaTrinhGiaiQuyet_DataTable(dr);
                        xldList.Add(phanGiaiQuyetInfo);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return xldList;
        }


        public List<DonThuGiaiQuyetInfo> GetChiTietBuocQuaTrinhGiaiQuyet(int xulydonid, int trangthaixulyID)
        {
            List<DonThuGiaiQuyetInfo> xldList = new List<DonThuGiaiQuyetInfo>();
            SqlParameter[] parms = GetParmsChiTietBuocQuaTrinhGiaiQuyet();
            SetParmsChiTietBuocQuaTrinhGiaiQuyet(parms, xulydonid, trangthaixulyID);
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_CHITIET_BUOCQUATRINHGIAIQUYET, parms))
                {
                    while (dr.Read())
                    {
                        DonThuGiaiQuyetInfo phanGiaiQuyetInfo = GetChiTietBuocQuaTrinhGiaiQuyet(dr);
                        xldList.Add(phanGiaiQuyetInfo);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return xldList;
        }
        public List<String> GetDuongDanFile(int theoDoiXuLyID)
        {
            List<String> list = new List<string>();
            SqlParameter[] parms = GetParmsQuaTrinhGiaiQuyetInfo();
            SetParmsQuaTrinhGiaiQuyetInfo(parms, theoDoiXuLyID);
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DUONGDANFILE, parms))
                {
                    while (dr.Read())
                    {
                        string phanGiaiQuyetInfo = Utils.GetString(dr["DuongDanFile"], String.Empty);
                        list.Add(phanGiaiQuyetInfo);
                    }
                    dr.Close();
                }
            }

            catch { throw; }
            return list;
        }
    }
}
