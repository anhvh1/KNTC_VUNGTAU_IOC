using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Com.Gosol.KNTC.Ultilities
{
    public class Constant
    {
        public static readonly DateTime DEFAULT_DATE = DateTime.ParseExact("01/01/1753 12:00:00 AM", "dd/MM/yyyy hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);

        public static readonly String CAPNHAT = "Cập nhật thông tin ";
        public static readonly String THEMMOI = "Thêm mới thông tin ";
        public static readonly String XOA = "Xóa thông tin ";

        public static readonly String MSG_SUKNTCESS = "Cập nhật dữ liệu thành công.";
        public static readonly String NO_FILE = "Không tìm thấy file cần download.";
        public static readonly String NO_DATA = "Không có dữ liệu.";
        public static readonly String ERR_INSERT = "Xảy ra lỗi trong quá trình thêm mới.";
        public static readonly String ERR_UPDATE = "Xảy ra lỗi trong quá trình cập nhật.";
        public static readonly String ERR_DELETE = "Xảy ra lỗi trong quá trình xóa dữ liệu.";
        public static readonly String ERR_UPLOAD = "Xảy ra lỗi trong quá trình upload file.";
        public static readonly String ERR_FILENOTFOUND = "Không tìm thấy file bạn cần download.";
        public static readonly String ERR_EXP_TOKEN = "Phiên làm việc đã hết hạn! Xin vui lòng đăng nhập lại.";
        public static readonly String NOT_AKNTCESS = "Người dùng không có quyền sử dụng chức năng này.";
        
        public static readonly String NOT_USINGAPP = "Gói đăng ký không bao gồm sử dụng dịch vụ di động.";
        public static readonly String NOT_ACCOUNT = "Tài khoản hoặc mật khẩu không đúng.";
        public static readonly String NOT_ACCOUNT_CAS = "Tài khoản chưa được phân quyền trên hệ thống. Vui lòng kiểm tra lại";
        public static readonly String NOT_USERTrangThai = "Tài khoản của bị đang bị khóa, vui lòng liên hệ quản trị viên để được hỗ trợ.";
        public static readonly String NOT_USERACTIVE = "Tài khoản của bạn chưa được kích hoạt, vui lòng kiểm tra Email để kích hoạt.";
        public static readonly string NOT_ACTIVE = "Nhà thuốc hiện tại đã ngưng hoạt động. Vui lòng liên hệ hotline của Medigate để biết thêm thông tin chi tiết. Xin cảm ơn.";
        public static readonly String NOT_USER_ACTIVE = "Tài khoản của nhà thuốc hiện chưa được kích hoạt. Vui lòng liên hệ hotline của Medigate để biết thêm thông tin chi tiết. Xin cảm ơn.";
        public static readonly string NOT_PAY = "Tài khoản của nhà thuốc hiện chưa thanh toán. Vui lòng liên hệ hotline của Medigate để biết thêm thông tin chi tiết. Xin cảm ơn.";
        public static readonly string NOT_EXPIRED = "Nhà thuốc hiện tại đã hết hạn sử dụng gói dịch vụ. Vui lòng liên hệ hotline của Medigate để biết thêm thông tin chi tiết. Xin cảm ơn.";
        public static readonly String API_Error_System = "Lỗi hệ thống, vui lòng liên hệ admin để được hỗ trợ!";

        //Các hình thức chấm công
        public static readonly string TheoThoiGianCaLam = "TheoThoiGianCaLam"; //theo thời gian thì đủ giờ là đủ công
        public static readonly string TheoKhungCaLam = "TheoKhungCaLam"; //theo khung thì fix giờ đến giờ về

        public static String GetUserMessage(int status)
        {
            switch (status)
            {
                default: return String.Empty;
                case 0: return NO_DATA;
                case -99: return ERR_EXP_TOKEN;
                case -98: return NOT_AKNTCESS;
            }
        }

        public static String GetSysMessage(int status)
        {
            switch (status)
            {
                default: return String.Empty;
                case 0: return NO_DATA;
                case -99: return ERR_EXP_TOKEN;
            }
        }

        public const string EncryptKey = "GoSolutions";

        public const string AgencyName = "Viện hàn lâm KHCNVN";

        public const string DepartmentName = "Văn phòng";

        private int pagesize = 0;

        public const int PageSize = 12;

        //public static readonly DateTime DEFAULT_DATE = DateTime.ParseExact("01/01/1753 12:00:00 AM", "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);

        public static readonly int Male = 1;

        public static readonly int Female = 0;

        //public static readonly string CAPNHAT = "Cập nhật thông tin ";

        //public static readonly string THEMMOI = "Thêm mới thông tin ";

        //public static readonly string XOA = "Xóa thông tin ";

        public static readonly string DEFAULT_TITLE_ERROR = "defaultError";

        public static readonly string CONTENT_DELETE_ERROR = "Dữ liệu đang được sử dụng. Vui lòng kiểm tra lại trước khi xóa!";

        public static readonly string HEADER_MESSAGE_ERROR = "Thông báo lỗi";

        public static readonly string CONTENT_TRINHKY_ERROR = "Đơn thư chưa có hướng xử lý không thể trình ký";

        public static readonly string CONTENT_MESSAGE_ERROR = "Cập nhật dữ liệu thất bại!";

        public static readonly string CONTENT_PHEDUYET_ERROR = "Thao tác phê duyệt chưa được thực hiện.";

        public static readonly string CONTENT_TRINHKQGIAIQUYET_ERROR = "Chưa thể trình giải quyết.";

        public static readonly string CONTENT_CHONDONTHU_ERROR = "Vui lòng chọn một đơn thư trước!";

        public static readonly string MESSAGE_INSERT_ERROR = "Dữ liệu đã tồn tại. Thêm mới thất bại!";

        public static readonly string MESSAGE_UPDATE_ERROR = "Dữ liệu đã tồn tại. Cập nhật thất bại!";

        public static readonly string DEFAULT_TITLE_SUCCESS = "defaultSuccess";

        public static readonly string MESSAGE_INSERT_SUCCESS = "Thêm mới dữ liệu thành công !";

        public static readonly string CONTENT_MESSAGE_SUCCESS = "Cập nhật dữ liệu thành công !";

        public static readonly string CONTENT_DELETE_SUCCESS = "Xóa dữ liệu thành công !";

        public static readonly string CONTENT_DELETE_ERR = "Đã xảy ra lỗi trong quá trình xóa dữ liệu !";

        public static readonly string CONTENT_TRINHKY_SUCCESS = "Trình lãnh đạo ký thành công!";

        public static readonly string CONTENT_PHEDUYET_SUCCESS = "Phê duyệt thành công!";

        public static readonly string CONTENT_TRINHKQGIAIQUYET_SUCCESS = "Trình giải quyết thành công!";

        public static readonly string CONTENT_DUYETGAP_SUCCESS = "Đồng ý tiếp thành công";

        public static readonly string CONTENT_TUCHOI_SUCCESS = "Từ chối tiếp thành công";

        public static readonly string CONTENT_MESSAGE_EROR = "Đã xảy ra lỗi trong quá trình cập nhật dữ liệu !";

        public static readonly int LengthNoiDungDon = 4000;

        public static readonly string ChuoiCuoiNDDon = "...";

        public static readonly int CV = 1;

        public static readonly int Dispatch = 2;

        public static readonly int passGrade = 4;

        public const int Softcover = 1;

        public const int Hardcover = 2;

        public const int TinhTrienKhaiID = 21;

        public const int KhieuNai = 1;

        public const int ToCao = 8;

        public const int PhanAnhKienNghi = 9;

        public const int KienNghi = 62;

        public const int PhanAnh = 23;

        public const int KN_LinhVucHanhChinh = 13;

        public const int KN_LinhVucTuPhap = 2;

        public const int KN_VeDang = 20;

        public const int LinhVucCTVHXH = 15;

        public const int VeTranhChapDatDai = 16;

        public const int VeChinhSach = 17;

        public const int VeNhaTaiSan = 18;

        public const int VeCheDo = 19;

        public const int TC_LinhVucHanhChinh = 10;

        public const int TC_LinhVucTuPhap = 11;

        public const int ThamNhung = 12;

        public const int TC_VeDang = 21;

        public const int TC_LinhVucKhac = 22;

        public const int TranhChap = 67;

        public const int KN_LinhVucKhac = 71;

        public const int ChuaGiaiQuyet = 0;

        public const int KienNghiXuLyHanhChinh = 8;

        public const int KienNghiThuHoiChoNhaNuoc = 14;

        public const int TraLaiChoCongDan = 15;

        public const int DieuTraKhoiTo = 17;

        public const int CongNhanQDLan1 = 12;

        public const int SuaQDLan1 = 13;

        public const int HuyQDLan1 = 23;

        public const int HuongDanTraLoi = 29;

        public const int ChuyenDon = 30;

        public const int ThuLyGiaiQuyet = 31;

        public const int CQHanhChinhCacCap = 11;

        public const int CQTuPhapCacCap = 14;

        public const int CQDang = 20;

        public const int Dung = 1;

        public const int Sai = 2;

        public const int Dung1Phan = 3;

        public const string ToolTip = "Bạn không có quyền sử dụng chức năng này";

        public const string NoCreate = "Bạn không có quyền sử dụng chức năng này";

        public const string NoEdit = "Bạn không có quyền sử dụng chức năng này";

        public const string NoDelete = "Bạn không có quyền sử dụng chức năng này";

        public const int TrangThai_DeXuatThuLy = 2;

        public const int TrangThai_TuChoiThuLy = 3;

        public const int TrangThai_DangXuLy = 4;

        public const int TrangThai_CoKetQua = 5;

        public const int TrangThai_RutDon = 11;

        public const int PhieuHuongDan = 1;

        public const int PhieuTraDonKN_HuongDan = 2;

        public const int PhieuChuyenDon_PhanAnhKienNghi = 3;

        public const int PhieuChuyenDonToCao = 4;

        public const int PhieuDeXuatThuLyDon = 5;

        public const int PhieuDeXuatThuLyToCao = 6;

        public const int ThongBaoKhongThuLyGiaiQuyet_GuiCQChuyenDonDen = 7;

        public const int ThongBaoKhongThuLyGiaiQuyet_GuiNguoiKN = 8;

        public const int ThongBaoKhongThuLyGiaiQuyetToCao = 9;

        public const int ThongBaoKhongThuLyGiaiQuyetToCaoTiep = 10;

        public const int ThongBaoThuLyGiaiQuyetKN = 11;

        public const int PhieuNhanHoSo = 12;

        public const int DonTrongCoQuan = 0;

        public const int DonDuocPhanXuLy = 1;

        public const string CoQuan = "CoQuan";

        public const string CaNhan = "CaNhan";

        public const string TapThe = "TapThe";

        public const int NguonDon_BuuChinh = 2;

        public const int NguonDon_CoQuanKhacChuyenToi = 3;

        public const string NguonDon_BuuChinhs = "Bưu chính";

        public const string NguonDon_CoQuanKhacs = "Cơ quan khác chuyển tới";

        public const string NguonDon_TrucTieps = "Trực tiếp";

        public const string NguonDon_TraoTays = "Trao tay";

        public const string LD_Phan_GiaiQuyet = "LD phân giải quyết";

        public const string LD_CapDuoi_Phan_GiaiQuyet = "LD cơ quan cấp dưới phân giải quyết";

        public const string TP_Phan_GiaiQuyet = "Phó chánh thanh tra hoặc lãnh đạo phòng phân giải quyết";

        public const string TP_XuLy = "TP xử lý";

        public const string TP_PhanXuLy = "TP phân xử lý";

        public const string LD_PhanXuLy = "LD phân xử lý";

        public const string CV_XuLy = "Chuyên viên xử lý";

        public const string LD_DuyetXuLy = "LD duyệt xử lý";

        public const string LD_Duyet_GiaiQuyet = "LD duyệt giải quyết";

        public const string TP_DuyetGQ = "Phó chánh thanh tra hoặc lãnh đạo phòng duyệt giải quyết";

        public const string LD_CQCapDuoiDuyetGQ = "LD cấp dưới duyệt giải quyết";

        public const string TruongDoan_GiaiQuyet = "Trưởng đoàn giải quyết";

        public const string CV_TiepNhan = "Chuyên viên tiếp nhận";

        public const string Ket_Thuc = "Kết thúc";

        public const string TP_DuyetXuLy = "TP duyệt xử lý";

        public const string RutDon = "Rút đơn";

        public const string CHUYENDON_RAVBDONDOC = "Chuyển đơn hoặc gửi văn bản đôn đốc";

        public const int LD_Phan_GiaiQuyet_Order = 7;

        public static readonly string MSG_INSERT_SUCCESS = "Cập nhật dữ liệu thành công.";

        //public static readonly string ERR_UPLOAD = "Xảy ra lỗi trong quá trình upload file.";

        //public static readonly string ERR_INSERT = "Xảy ra lỗi trong quá trình cập nhật.";

        //public static readonly string ERR_FILENOTFOUND = "Không tìm thấy file bạn cần download";

        public const string IsLanhDao = "IsLanhDao";

        public const string IsTruongPhuong = "IsTruongPhong";

        public const string DeXuatThuLy = "Đề xuất thụ lý";

        public const string ChuyenDons = "Chuyển đơn";

        public static readonly string DM_HUONGGIAIQUYET = " danh mục hướng giải quyết";

        public static readonly string DM_COQUAN = " danh mục cơ quan,đơn vị";

        public static readonly string DM_LOAIKETQUA = " danh mục loại kết quả";

        public static readonly string DM_LOAIDOITUONGKN = " danh mục loại đối tượng khiếu nại";

        public static readonly string DM_LOAIDOITUONGBIKN = " danh mục loại đối tượng bị khiếu nại";

        public static readonly string DM_DANTOC = " danh mục dân tộc";

        public static readonly string DM_QUOCTICH = " danh mục quốc tịch";

        public static readonly string DM_TINH = " danh mục tỉnh";

        public static readonly string DM_HUYEN = " danh mục huyện";

        public static readonly string DM_XA = " danh mục xã";

        public static readonly string DM_THAMQUYEN = " danh mục thẩm quyền";

        public static readonly string DM_PHONGBAN = " danh mục phòng ban";

        public static readonly string DM_CHUCVU = " danh mục chức vụ";

        public static readonly string DM_NGUONDONDEN = " danh mục nguồn đơn đến";

        public static readonly string DM_LOAIKHIEUTO = " danh mục loại khiếu tố";

        public static readonly string DM_TRANGTHAIDON = " danh mục trạng thái đơn";

        public static readonly string EMAIL_TITLE_DUYETXL = "[KNTC] Thông báo bạn có đơn thư cần duyệt xử lý.";

        public static readonly string EMAIL_TITLE_XLLAI = "[KNTC] Thông báo bạn có đơn thư cần xử lý lại.";

        public static readonly string EMAIL_TITLE_DUYETGQ = "[KNTC] Thông báo bạn có đơn thư cần duyệt kết quả giải quyết.";

        public static readonly string EMAIL_TITLE_GQLAI = "[KNTC] Thông báo bạn có đơn thư cần xác minh lại.";

        public static readonly string EMAIL_TITLE_PHANGQ = "[KNTC] Thông báo bạn có đơn thư cần giao xác minh.";

        public static readonly string EMAIL_TITLE_XACMINHDON = "[KNTC] Thông báo bạn có đơn thư cần xác minh.";

        public static readonly string EMAIL_TITLE_KQPHUCDAP = "[KNTC] Thông báo bạn có kết quả phúc đáp đơn thư.";

        public static readonly int DM_EMAIL_DUYETXL = 1;

        public static readonly int DM_EMAIL_XLLAI = 2;

        public static readonly int DM_EMAIL_COHS_GIAOXM = 3;

        public static readonly int DM_EMAIL_GIAOXM = 4;

        public static readonly int DM_EMAIL_DUYETXM = 5;

        public static readonly int DM_EMAIL_XMLAI = 6;

        public static readonly int DM_EMAIL_BTD_DUYETXM = 7;

        public static readonly string TITLE_DM_EMAIL_DUYETXL = "[KNTC] Thông báo phê duyệt kết quả xử lý";

        public static readonly string TITLE_DM_EMAIL_XLLAI = "[KNTC] Thông báo cán bộ xử lý lại";

        public static readonly string TITLE_DM_EMAIL_COHS_GIAOXM = "[KNTC] Thông báo LĐ có hồ sơ được giao nhiệm vụ xác minh";

        public static readonly string TITLE_DM_EMAIL_GIAOXM = "[KNTC] Thông báo cán bộ được giao nhiệm vụ xác minh";

        public static readonly string TITLE_DM_EMAIL_DUYETXM = "[KNTC] Thông báo LĐ phê duyệt kết quả xác minh";

        public static readonly string TITLE_DM_EMAIL_XMLAI = "[KNTC] Thông báo cho cán bộ thực hiện xác minh lại";

        public static readonly string TITLE_DM_EMAIL_BTD_DUYETXM = "[KNTC] Thông báo cho LĐ ban tiếp dân phê duyệt kết quả xác minh";

        public static readonly string PATH_FILE_HOSO_ENCRYPT = "/UploadFiles/filehoso/encrypt/";

        public static readonly string PATH_FILE_HOSO_DECRYPT = "/UploadFiles/filehoso/decrypt/";

        public static readonly string PATH_FILE_HOSO = "/UploadFiles/filehoso/";

        public static readonly string PATH_FILE_KQXL_ENCRYPT = "/UploadFiles/FileKetQuaXuLy/encrypt/";

        public static readonly string PATH_FILE_KQXL_DECRYPT = "/UploadFiles/FileKetQuaXuLy/decrypt/";

        public static readonly string PATH_FILE_KQXL = "/UploadFiles/FileKetQuaXuLy/";

        public static readonly string PATH_FILE_DTCPGQ_ENCRYPT = "/UploadFiles/FileDonThuCanPhanGiaiQuyet/encrypt/";

        public static readonly string PATH_FILE_DTCPGQ_DECRYPT = "/UploadFiles/FileDonThuCanPhanGiaiQuyet/decrypt/";

        public static readonly string PATH_FILE_DTCPGQ = "/UploadFiles/FileDonThuCanPhanGiaiQuyet/";

        public static readonly string PATH_FILE_BHGQ_ENCRYPT = "/UploadFiles/FileBanHanhQD/encrypt/";

        public static readonly string PATH_FILE_BHGQ_DECRYPT = "/UploadFiles/FileBanHanhQD/decrypt/";

        public static readonly string PATH_FILE_BHGQ = "/UploadFiles/FileBanHanhQD/";

        public static readonly string PATH_FILE_DonDoc_ENCRYPT = "/UploadFiles/FileDonDoc/encrypt/";

        public static readonly string PATH_FILE_DonDoc_DECRYPT = "/UploadFiles/FileDonDoc/decrypt/";

        public static readonly string PATH_FILE_DonDoc = "/UploadFiles/FileDonDoc/";

        public static readonly string PATH_FILE_HSDS_ENCRYPT = "/UploadFiles/FileHoSoDonStep/encrypt/";

        public static readonly string PATH_FILE_HSDS_DECRYPT = "/UploadFiles/FileHoSoDonStep/decrypt/";

        public static readonly string PATH_FILE_HSDS = "/UploadFiles/FileHoSoDonStep/";

        public static readonly string PATH_FILE_RUTDON_ENCRYPT = "/UploadFiles/FileRutDon/encrypt/";

        public static readonly string PATH_FILE_RUTDON_DECRYPT = "/UploadFiles/FileRutDon/decrypt/";

        public static readonly string PATH_FILE_RUTDON = "/UploadFiles/FileRutDon/";

        public static readonly string PATH_FILE_PHANXULY_ENCRYPT = "/UploadFiles/FilePhanXuLy/encrypt/";

        public static readonly string PATH_FILE_PHANXULY_DECRYPT = "/UploadFiles/FilePhanXuLy/decrypt/";

        public static readonly string PATH_FILE_PHANXULY = "/UploadFiles/FilePhanXuLy/";

        public static readonly string PATH_FILE_GIAIQUYET_ENCRYPT = "/UploadFiles/GiaiQuyet/encrypt/";

        public static readonly string PATH_FILE_GIAIQUYET_DECRYPT = "/UploadFiles/GiaiQuyet/decrypt/";

        public static readonly string PATH_FILE_GIAIQUYET = "/UploadFiles/GiaiQuyet/";

        public static readonly string PATH_FILE_TRANHCHAP_ENCRYPT = "/UploadFiles/TranhChap/encrypt/";

        public static readonly string PATH_FILE_TRANHCHAP_DECRYPT = "/UploadFiles/TranhChap/decrypt/";

        public static readonly string PATH_FILE_TRANHCHAP = "/UploadFiles/TranhChap/";

        public static readonly string PATH_FILE_DONTHUCANDUYETGIAIQUYET_ENCRYPT = "/UploadFiles/FileDonThuCanDuyetGiaiQuyet/encrypt/";

        public static readonly string PATH_FILE_DONTHUCANDUYETGIAIQUYET_DECRYPT = "/UploadFiles/FileDonThuCanDuyetGiaiQuyet/decrypt/";

        public static readonly string PATH_FILE_DONTHUCANDUYETGIAIQUYET = "/UploadFiles/FileDonThuCanDuyetGiaiQuyet/";

        public static readonly string PATH_FILE_VBDONDOC_ENCRYPT = "/UploadFiles/VBDonDoc/encrypt/";

        public static readonly string PATH_FILE_VBDONDOC_DECRYPT = "/UploadFiles/VBDonDoc/decrypt/";

        public static readonly string PATH_FILE_VBDONDOC = "/UploadFiles/VBDonDoc/";

        public static readonly string PATH_FILE_QLHS_ENCRYPT = "/UploadFiles/QLHS/encrypt/";

        public static readonly string PATH_FILE_QLHS_DECRYPT = "/UploadFiles/QLHS/decrypt/";

        public static readonly string PATH_FILE_QLHS = "/UploadFiles/QLHS/";

        public static readonly string PATH_FILE_DMBXM_ENCRYPT = "/UploadFiles/DMBuocXacMinh/encrypt/";

        public static readonly string PATH_FILE_DMBXM_DECRYPT = "/UploadFiles/DMBuocXacMinh/decrypt/";

        public static readonly string PATH_FILE_DMBXM = "/UploadFiles/DMBuocXacMinh/";

        public static readonly string PATH_FILE_CHITIETDONTHU_DECRYPT = "/UploadFiles/ChiTietDonThu/decrypt/";
    }
}
