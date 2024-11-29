using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class BCTinhHinhTD_XLD_GQInfo
    {
        public int CapID { get; set; }
        public int HuyenID { get; set; }
        public string TenCap { get; set; }
        public int CoQuanID { get; set; }
        public int CoQuanChaID { get; set; }
        public string TenCoQuan { get; set; }
        public int SoDonTiepDan { get; set; }
        public int SoXLTrongHan { get; set; }
        public int SoXLQuaHan { get; set; }
        public int TongSoXL { get; set; }
        public int TongSoDonGiaoGQ { get; set; }
        public int SoDangGQTrongHan { get; set; }
        public int SoDangGQQuaHan { get; set; }
        public int TongSoDangGQ { get; set; }
        public int DaCoBC { get; set; }
        public int DaBHGQ { get; set; }
        public int TongSoDonTiepNhan { get; set; }
        public int SoDonKhieuNai { get; set; }
        public int SoDonToCao { get; set; }
        public int SoDonThuCQKhacChuyenDen { get; set; }

        public int SoDonKNDaXuLy { get; set; }
        public int SoDonKNChuaXuLy { get; set; }

        public int SoDonTCDaXuLy { get; set; }
        public int SoDonTCChuaXuLy { get; set; }

        public int DaBHGQDung { get; set; }
        public int DaBHGQSai { get; set; }
        public int DaBHGQDungMotPhan { get; set; }

        public int SoXLDaXuLy { get; set; }
        public int SoXLChuaXuLy { get; set; }
        public int ChuaGiaiQuyet { get; set; }

        public int ChuaBHGQ { get; set; }
        public int DaGQ { get; set; }
        public int DaBHGQDonKN { get; set; }
        public int DaBHGQDonTC { get; set; }
        public int DaBHGQDonKNPA { get; set; }
        public int VuViecDongNguoi { get; set; }

        public List<BCTinhHinhTD_XLD_GQInfo> LsByCoQuan { get; set; }
        public List<BCTinhHinhTD_XLD_CapCoQuan_GQInfo> LsByCapCoQuan { get; set; }
        //Bổ sung khi them dashboard
        public int GQDKNDangGQ { get; set; }
        public int GQDTCDangGQ { get; set; }
        public int GQDKNPADangGQ { get; set; }
        public int GQDKNDaGQ { get; set; }
        public int GQDTCDaGQ { get; set; }
        public int GQDKNPADaGQ { get; set; }
        public int GQDKNChuaGQ { get; set; }
        public int GQDTCChuaGQ { get; set; }
        public int GQDKNPAChuaGQ { get; set; }
        public int TongGQDKN { get; set; }
        public int TongGQDTC { get; set; }
        public int TongGQDKNPA { get; set; }

        public int SLKhieuNai { get; set; }
        public int SLToCao { get; set; }
        public int SLPhanAnhKienNghi { get; set; }
        public int XLDKhieuNai { get; set; }
        public int XLDToCao { get; set; }
        public int XLDPhanAnhKienNghi { get; set; }
        public string Style { get; set; }
    }
    public class BCTinhHinhTD_XLD_CapCoQuan_GQInfo
    {
        public string TenCoQuan { get; set; }
        public int CoQuanID { get; set; }
        public List<BCTinhHinhTD_XLD_GQInfo> LsByCoQuan { get; set; }
    }
}
