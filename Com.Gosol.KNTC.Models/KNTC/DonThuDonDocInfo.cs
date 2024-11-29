using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class DonThuDonDocInfo
    {
        public int DonThuID { get; set; }
        public int LanGiaiQuyet { get; set; }
        public int XuLyDonIDLanHon1 { get; set; }
        public int Tong { get; set; }
        public int XuLyDonID { get; set; }
        public string TenChuDon { get; set; }
        public string SoDonThu { get; set; }
        public string NguonDonDen { get; set; }
        public DateTime? NgayDonDoc { get; set; }
        public string NgayDonDocStr { get; set; }
        public string NoiDungDon { get; set; }
        public string PhanLoai { get; set; }
        public string TenHuongXuLy { get; set; }
        public int HuongXuLyID { get; set; }
        public int CoQuanID { get; set; }
        public string TenCoQuan { get; set; }
        public DateTime? HanXuLy { get; set; }
        public string TenTrangThai { get; set; }
        public int TrangThaiID { get; set; }
        public int DonDocID { get; set; }
        public int? IsDonDoc { get; set; }
        public int? CanhBao { get; set; }
        public string TenTrangThaiDonDoc { get; set; }
        public int? NhomKNID { get; set; }
        public List<DoiTuongKNInfo> listDoiTuongKN { get; set; }
    }
}
