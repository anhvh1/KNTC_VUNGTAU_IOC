using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class DoiTuongBiKNInfo
    {
        public int DoiTuongBiKNID { get; set; }
        public string TenDoiTuongBiKN { get; set; }
        public int? TinhID { get; set; }
        public int? HuyenID { get; set; }
        public int? XaID { get; set; }
        public string DiaChiCT { get; set; }
        public int? LoaiDoiTuongBiKNID { get; set; }

        public string TenTinh { get; set; }
        public string TenHuyen { get; set; }
        public string TenXa { get; set; }
        public string TenDanToc { get; set; }
        public string TenNgheNghiep { get; set; }
        public string TenChucVu { get; set; }
        public string TenQuocTich { get; set; }
        public string StringLoaiDoiTuong { get; set; }
        //Ca nhan
        public int? CaNhanBiKNID { get; set; }
        public int? ChucVuID { get; set; }
        public int? QuocTichDoiTuongBiKNID { get; set; }
        public int? GioiTinhDoiTuongBiKN { get; set; }
        public int? DanTocDoiTuongBiKNID { get; set; }
        public string SoNhaDoiTuongBiKN { get; set; }
        public string NoiCongTacDoiTuongBiKN { get; set; }
        public int? DonThuID { get; set; }

    }
}
