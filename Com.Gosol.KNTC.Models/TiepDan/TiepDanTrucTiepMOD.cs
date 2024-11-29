using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.TiepDan
{
    public class TiepDanTrucTiepMOD
    {
        /*public string TenTinh { get; set; }
        public string TenHuyen { get; set; }
        public string TenXa { get; set; }
        public string TenQuocTich { get; set; }
        public string TenDanToc { get; set; }*/ 
        public List<DoiTuongKNMOD> DoiTuongKN { get; set; } 
        public List<NhomKNMOD> NhomKN { get; set; }

    }

    public class DoiTuongKNMOD
    {
        // Đối tượng khiếu nại tố cáo
        public int DoiTuongKNID { get; set; }
        public string HoTen { get; set; }
        public string CMND { get; set; }
        public DateTime? NgayCap { get; set; }
        public string NoiCap { get; set; }
        public int HuyenID { get; set; }
        public int TinhID { get; set; }
        public int DanTocID { get; set; }
        public int QuocTichID { get; set; }
        public string SoDienThoai { get; set; }

        public int GioiTinh { get; set; }
        public string NgheNghiep { get; set; }
        public int XaID { get; set; }
        public string DiaChiCT { get; set; }

        public int NhomKNID { get; set; }
    }

    public class NhomKNMOD
    {
        public int NhomKNID { get; set; }
        public int SoLuong { get; set; }
        public string TenCQ { get; set; }
        public int LoaiDoiTuongKNID { get; set; }
        public string DiaChiCQ { get; set; }
        public bool DaiDienPhapLy { get; set; }
        public bool DuocUyQuyen { get; set; }
    }

    public class LoaiDoiTuongKNMOD
    {
        public int LoaiDoiTuongKNID { get; set; }
        public string TenLoaiDoiTuongKN { get; set; }
    }
}
