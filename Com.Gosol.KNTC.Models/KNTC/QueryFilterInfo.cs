using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class QueryFilterInfo
    {
        public int CoQuanID { get; set; }

        public int CQChuyenDonDenID { get; set; }

        public string KeyWord { get; set; }

        public int LoaiKhieuToID { get; set; }

        public DateTime? TuNgay { get; set; }

        public DateTime? DenNgay { get; set; }

        public int Start { get; set; }

        public int End { get; set; }

        public string StateName { get; set; }

        public string StateName2 { get; set; }

        public int StateID { get; set; }

        public string TenCoQuan { get; set; }

        public int StateOrder { get; set; }

        public int CanBoID { get; set; }

        public bool IsChuyenVien { get; set; }

        public int PhongBanID { get; set; }

        public string PrevStateName { get; set; }

        public int VaiTro { get; set; }

        public bool IsTruongPhong { get; set; }

        public bool IsLanhDao { get; set; }

        public DateTime TuNgayGoc { get; set; }

        public DateTime DenNgayGoc { get; set; }

        public DateTime TuNgayMoi { get; set; }

        public DateTime DenNgayMoi { get; set; }

        public int LoaiRutDon { get; set; }

        public int LoaiXL { get; set; }

        public int PTKQDung { get; set; }

        public int PTKQDungMotPhan { get; set; }

        public int PTKQSai { get; set; }

        public int LoaiDon { get; set; }

        public int TrangThaiKQ { get; set; }

        public int HuongXuLyID { get; set; }

        public int CapID { get; set; }

        public int TinhID { get; set; }

        public int? TrangThai { get; set; }
        public int? TrangThaiDonThu { get; set; }
        public int? ChuTichUBND { get; set; }
    }
}
