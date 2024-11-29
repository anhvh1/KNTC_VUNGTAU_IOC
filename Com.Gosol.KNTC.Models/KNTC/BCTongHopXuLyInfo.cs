using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class BCTongHopXuLyInfo
    {
        public string STT { get; set; }
        public int CanBoID { get; set; }
        public string TenCanBo { get; set; }

        public int SLDonXuLy { get; set; }
        public int SLDonXuLyTrongHan { get; set; }
        public int SLDonXuLyQuaHan { get; set; }
        public int Level { get; set; }

        public int PhongBanID { get; set; }
        public string TenPhongBan { get; set; }
        public int CoQuanID { get; set; }

        public int CapID { get; set; }

        public string TenCoQuan { get; set; }

        public int CoQuanGiaiQuyetID { get; set; }
        public int CQPhoiHopID { get; set; }
        public string CQPhoiHopStr { get; set; }


        public int SLTiepCongDan { get; set; }
        public int SLKhieuNai { get; set; }
        public int SLToCao { get; set; }
        public int SLPhanAnhKienNghi { get; set; }
        public int XLDTongSo { get; set; }
        public int XLDKhieuNai { get; set; }
        public int XLDToCao { get; set; }
        public int XLDPhanAnhKienNghi { get; set; }
        public int XLDChuaXuLy { get; set; }
        public int XLDDaXuLyTrongHan { get; set; }
        public int XLDDaXuLyQuaHan { get; set; }
        public int XLDDaXuLy { get; set; }

        public int GQDTongSo { get; set; }
        public int GQDGiaoPhongNV { get; set; }
        public int GQDChuaGQ { get; set; }
        public int GQDDangGQTrongHan { get; set; }
        public int GQDDangGQQuaHan { get; set; }
        public int GQDDaGQ { get; set; }
        public int GQDDangGQ { get; set; }
        public int DaCoBaoCao { get; set; }
        public int KQKNDung { get; set; }
        public int KQKNDungMotPhan { get; set; }
        public int KQKNSai { get; set; }

        public int XLDKhieuKienDN { get; set; }

        public string SoDon { get; set; }
        public int XuLyDonID { get; set; }
        public int DonThuID { get; set; }
        public int NhomKNID { get; set; }
        public DateTime NgayTiep { get; set; }
        public string NgayTiepStr { get; set; }
        public string NoiDungTiep { get; set; }
        public string TenLoaiKhieuTo { get; set; }
        public string HoTen { get; set; }
        public string DiaChiCT { get; set; }


        public string SLTiepCongDanStr { get; set; }
        public string XLDTongSoStr { get; set; }
        public string XLDChuaXuLyStr { get; set; }
        public string XLDDaXuLyTrongHanStr { get; set; }
        public string XLDDaXuLyQuaHanStr { get; set; }
        public string XLDDaXuLyStr { get; set; }

        public string GQDTongSoStr { get; set; }
        public string GQDGiaoPhongNVStr { get; set; }
        public string GQDChuaGQStr { get; set; }
        public string GQDDangGQTrongHanStr { get; set; }
        public string GQDDangGQQuaHanStr { get; set; }
        public string GQDDaGQStr { get; set; }
        public string GQDDangGQStr { get; set; }
        public string DaCoBaoCaoStr { get; set; }

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

        public int StateID { get; set; }
        public int KetQuaID { get; set; }
        public int HuongXuLyID { get; set; }
        public string KetQuaID_Str { get; set; }
        public string TenHuongGiaiQuyet { get; set; }

    }
}
