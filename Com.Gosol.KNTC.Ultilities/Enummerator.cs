using System;
using System.ComponentModel;
using System.Web;

namespace Com.Gosol.KNTC.Ultilities
{

    #region AnhVH
    public enum EnumLogType
    {
        Error = 0, // lỗi
        //Action = 1, // thực hiện các chức năng
        DangNhap = 100,

        Insert = 101,
        Update = 102,
        Delete = 103,

        GetByID = 201,// lấy dữ liệu theo ID
        GetByName = 202, // lấy dữ liệu theo tên, key
        GetList = 203, // lấy danh sách dữ liệu      

        BackupDatabase = 901,
        RestoreDatabase = 902,

        Other = 500,

    }




    public enum EnumCapCoQuan : Int32
    {
        CapTrungUong = 0,
        CapTinh = 1,
        CapSo = 2,
        CapHuyen = 3,
        CapPhong = 4,
        CapXa = 5,
    }

    public enum EnumCapQuanLyCanBo : Int32
    {
        CapTinh = 1,
        CapHuyen = 2,
        ToanTinh = 3
    }

    public enum EnumTrangThaiCanBo
    {
        DangLamViec = 0,
        NghiHuu = 1,
        ChuyenCongTac = 2,
        NghiViec = 3,
    }

    /// <summary>
    /// biến động tài sản của cán bộ
    /// </summary>

    public enum StatusResult
    {
        // -98 = hết hạn, -99 = không đủ quyền, -1 = lỗi hệ thống, 0 = lỗi validate, 1 = thành công, 2 = đã tồn tại
        HetHan = -98,
        KhongDuQuyen = -99,
        LoiHeThong = -1,
        LoiValidate = 0,
        ThanhCong = 1,
        DaTonTai = 2,
    }
    #endregion

    #region ChamCong
    public enum EnumTrangThaiAnh : Int32
    {
        DuAnh = 1,
        ThieuAnh = 2,
        ChuaCoAnh = 3,
    }
    public enum EnumLoaiDoiTuong : Int32
    {
        NhanVien = 1,
        PhongBan = 2,
        Nhom = 3,
    }

    public enum EnumTrangThaiNhanVien : Int32
    {
        DaNghi = 0,
        DangLam = 1,
    }

    public enum EnumTrangThaiChamCong : Int32
    {
        DuCong = 1,
        ThieuCong = 2,
        QuenChamVao = 3,
        QuenChamRa = 4,
    }

    public enum EnumLoaiDonTu : Int32
    {
        LamThem = 1,
        ChamCongBu = 2,
        XinNghi = 3,
        QuenChamVao = 4,
        QuenChamRa = 5,
        DiRaNgoai = 6,
        XinDenMuon = 7,
        XinVeSom = 8,
    }

    public enum EnumTrangThaiDonTu : Int32
    {
        ChoDuyet = 100,
        DaDuyet = 200,
        TuChoi = 101,
    }
    public enum EnumQuyMoLuaChon : Int32
    {
        TatCa = 1,
        LuaChon = 2,
    }

    public enum EnumLoaiAnhNhanVien : Int32
    {
        AnhChinhDien = 1,
        AnhTrai = 2,
        AnhPhai = 3,
        AnhNgua = 4,
        AnhCui = 5,
        AnhCanMat = 6,
        AnhXa = 7
    }

    public enum EnumLoaiThongBaoDonTu : Int32
    {
        CanDuyet = 1,
        DaDuyet = 2,
        TuChoi = 3
    }

    public enum EnumLoaiNgayNghi : Int32
    {
        NghiLe = 1,
        NghiThamNien = 2
    }

    //public enum EnumLoaiFile
    //{
    //    MatTruocCMND = 1,
    //    MatSauCMND = 2,
    //    AnhChanDung = 3,
    //    AnhCheckIn = 4,
    //}
    #endregion

    #region KNTC
    public enum CapQuanLy
    {
        CapSoNganh = 1,//
        CapUBNDHuyen = 2,
        CapUBNDXa = 3,//
        CapUBNDTinh = 4,//
        CapTrungUong = 5,
        ToanHuyen = 6, // Toàn huyện= Tổng UBND huyện  +UBND Xã
        CapPhong = 11,
        CapToanTinh = 12,
        ToanHuyenChiTiet = 13,
        CapUBNDXaChiTiet = 14,
        CapUBNDHuyenChiTiet = 15,
        CapUBNDTinhChiTiet = 16,
        CapSoNganhChiTiet = 17

    }

    public enum EnumCapHanhChinhHDSD
    {
        [Description("UBND tỉnh")]
        CapUBNDTinh = 20,//
        [Description("Ban tiếp dân tỉnh")]
        BTDTinh = 21,//
        [Description("Sở ban ngành")]
        CapSoNganh = 30,//
        [Description("Phòng thuộc sở")]
        CapPhongThuocSo = 31, // phòng thuộc sở, ví dụ phòng thanh tra, phòng nghiệp vụ 1, phòng tài vụ...
        [Description("UBND huyện")]
        CapUBNDHuyen = 40,
        [Description("Ban tiếp dân huyện")]
        BTDHuyen = 42,
        [Description("Phòng thuộc huyện")]

        CapPhongThuocHuyen = 41, // phòng thuộc huyện. ví dụ phòng tài nguyên môi trường
        [Description("UBND xã")]
        CapUBNDXa = 50,//
    }

    // dùng để phân biết cơ quan thuộc cấp nào từ ubnd tỉnh, phòng thuộc ubnd tỉnh, sở, phòng thuộc sở...
    public enum EnumCapHanhChinh
    {
        CapTrungUong = 10,
        CapUBNDTinh = 20,//
        CapPhongThuocTinh = 21, // phòng thuộc ubnd tỉnh, không phải cấp sở. ví dụ văn phòng ubnd tỉnh
        CapSoNganh = 30,//
        CapPhongThuocSo = 31, // phòng thuộc sở, ví dụ phòng thanh tra, phòng nghiệp vụ 1, phòng tài vụ...
        CapUBNDHuyen = 40,
        CapPhongThuocHuyen = 41, // phòng thuộc huyện. ví dụ phòng tài nguyên môi trường
        CapUBNDXa = 50,//
        CapPhongThuocXa =51 // phòng thuộc xã. phòng tư pháp, phòng văn hóa... trong hệ thống kntc đang không tạo phòng này cho các xã
    }

    public enum EnumChucVu
    {
        LanhDao = 1,
        TruongPhong = 2,
        ChuyenVien = 3,
    }
    public enum CapCoQuanViewChiTiet
    {
        ToanTinh = 13,//
        CapSoNganh = 9,//
        CapUBNDHuyen = 14,//
        CapUBNDXa = 10,//
        CapUBNDTinh = 8,//
        CapTrungUong = 7,
        ToanHuyen = 15, // Toàn huyện= Tổng UBND huyện  +UBND Xã //
        CapPhong = 12
    }
    public enum EnumLoaiFile
    {
        FileHoSo = 1,
        FileKQXL = 2,
        FileDTCPGQ = 3,
        FileBanHanhQD = 4,
        FileTheoDoi = 5,
        FileYKienXuLy = 6,
        //FileHSDS = 7,
        FileGiaiQuyet = 8,
        FileKetQuaTranhChap = 9,
        FileRutDon = 10,
        FileThiHanh = 11,
        FilePhanXuLy = 12,
        FileVBDonDoc = 13,
        FileDTCDGQ = 14,
        FileBCXM = 15, //File báo cáo xác minh
        FileDMBXM = 16,
        FileGiaHanGiaiQuyet = 17,
        FileBieuMau = 18,
        FileHuongDanSuDung = 19,
        FileTrinhDuThao = 20,
        FileDuyetDuThao = 21,
        FileBanHanhGiaiQuyet = 22,
        FileThumbnail = 23,
        FileTrinhTuThuTuc = 24,
        FileQuyTrinhNghiepVu = 25,
        FileCQGiaiQuyet = 26,
        FileKQTiep = 27,
        FileKQGiaiQuyet = 28,
        FileDBCXM= 29,
    }
    public enum EnumDoBaoMat
    {
        BinhThuong = 1,
        BaoMat = 2,
    }
    public enum EnumLoaiLog
    {
        Them = 1,
        Sua = 2,
        Xoa = 2,
    }
    public enum EnumState
    {
        LDPhanXuLy = 1,
        ChuyenVienXL = 4,
        TPDuyetXL = 5,
        LDDuyetXL = 6,
        LDPhanGQ = 7,
        TruongDoanGQ = 8,
        LDDuyetDQ = 9,
        KetThuc = 10,
        ChuyenVienTiepNhan = 11,
        TPPhanXL = 12,
        LDCapDuoiDuyetGQ = 22,
    }
    public enum EnumQTTiepNhanDon
    {
        QTTiepNhanGianTiep = 1,
        QTVanThuTiepNhan,
        QTVanThuTiepDan,
        QTGianTiepBTD
    }
    public enum TrangThaiDonEnum
    {
        DeXuatThuLy = 2,
        TuChoiThuLy = 3,
        DangXuLy = 4,
        CoKetQua = 5,
        RutDon = 11
    }
    public enum HuongGiaiQuyetEnum
    {
        HuongDan = 30,
        DeXuatThuLy = 31,
        ChuyenDon = 32,
        TraDon = 33,
        RaVanBanDonDoc = 34,
        GuiVanBanThongBao = 35,
        TuChoiTiep = 68,
        CongVanChiDao = 69,
        BaoCao = 70,
        LuuDon = 71,
        TuChoiThuLy = 72
    }
    public enum PhanTichKQEnum
    {
        Dung = 1,
        DungMotPhan = 2,
        Sai = 3
    }

    public enum KetQuaGQLan2Enum
    {
        CongNhanQDLan1 = 1,
        SuaHuyQDLan1 = 2
    }
    public enum EnumCoQuan
    {
        BanTCDTinh = 245,
        ThanhTraTinh = 230,
    }

    public enum EnumKetQuaTiepDan
    {
        ChuaGiaiQuyet = 1,
        ChuaCoQDGiaiQuyet = 2,
        DaCoQDGiaiQuyet = 3,
        DaCoBanAnCuaToa = 4,
    }

    public enum DMLoaiDoiTuongKN
    {
        CaNhan = 1,
        TapThe = 2,
        CoQuanToChuc = 3
    }

    public enum DMLoaiDoiTuongBiKN
    {
        CaNhan = 1,
        CoQuanToChuc = 2,
    }
    public enum EnumNguonDonDen
    {
        TrucTiep = 21,
        BuuChinh = 26,
        CoQuanKhac = 22,
        TraoTay = 28
    }
    public enum EnumLoaiTiepDan : Int32
    {
        [Description("Thường xuyên")]
        TiepThuongXuyen = 1,
        [Description("Định kỳ")]
        TiepDinhKy = 2,
        [Description("Đột xuất")]
        TiepDotXuat = 3
    }
    public enum RoleEnum
    {
        LanhDao = 1,
        LanhDaoPhong = 2,
        ChuyenVien = 3
    }
    public enum TrangThaiXuLy
    {
        ChuaXuLy = 1,
        DaXuLy = 2,
        ChuyenVien = 3
    }
    public enum TrangThaiPheDuyetKetQuaXuLy
    {
        ChuaDuyet = 0,
        DaDuyet = 1,
        XuLyLai = 2
    }

    public enum TrangThaiTrinhDuThao
    {
        ChuaTrinh = 0,
        DaTrinh = 1,
        DaDuyetQDXacMinh = 2,
        DaDuyetQDGiaiQuyet = 3,
    }
    public enum TrangThaiBanHanh
    {
        ChuaCapNhat = 1,
        DaCapNhat = 2
    }
    public enum TrangThaiThiHanh
    {
        ChuaThiHanh = 1,
        DangThiHanh = 2,
        DaThiHanh = 3
    }

    public enum TrangThaiGiaiQuyetDonLD
    {
        ChuaThiHanh = 1,
        DangThiHanh = 2,
        DaThiHanh = 3
    }

    public enum VaiTroEnum
    {
        PhuTrach = 1,
        PhoiHop = 2,
        TheoDoi = 3
    }

    public enum TrangThaiDongBo
    {
        New = 0,
        DangDongBo = 1,
        DaDongBo = 2,
    }

    public enum TrangThaiDonThuDongBo
    {
        CapNhatVuViec = 1,
        CapNhatKetQuaXuLy = 2,
        CapNhatThongTinGiaiQuyet = 3,
        CapNhatKetQuaGiaiQuyet = 4,
        CapNhatThiHanhGiaiQuyet = 5,
    }

    public enum LoaiDongBo
    {
        TheoNgayGio = 1,
        TheoThu = 2,
    }

    public enum EnumBuoc
    {
        ThongTinChung = 1,
        XuLyDon = 2,
        GiaiQuyetDon = 3,
        BanHanh = 4,
        TheoDoi = 5,
        LDPhanGiaiQuyet = 6,
        YeuCauDoiThoai = 7,
        BaoCaoXacMinh = 8,
        DuyetGiaiQuyet = 9,
    }
    public enum TypeEdit
    {
        String = 1,
        Number = 2,
    }
    #endregion

    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var type = value.GetType();
            var fieldInfo = type.GetField(value.ToString());
            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        public static T GetEnumValue<T>(int value) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            foreach (T enumValue in Enum.GetValues(typeof(T)))
            {
                if (Convert.ToInt32(enumValue) == value)
                {
                    return enumValue;
                }
            }

            throw new ArgumentException("No enum value corresponds to the specified value");
        }
    }
};