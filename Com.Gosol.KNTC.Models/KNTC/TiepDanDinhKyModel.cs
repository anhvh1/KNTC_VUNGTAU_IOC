using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class TiepDanDinhKyModel
    {
        public int TiepDinhKyID { get; set; } 
        public int? DanKhongDenID { get; set; } 
        public DateTime? NgayTiep { get; set; }
        public int? LanhDaoTiep { get; set; }
        public string TenLanhDaoTiep { get; set; }
        public int? UyQuyenTiep { get; set; }
        public string ChucVu { get; set; }
        public string NoiDungTiep { get; set; }
        public string KetQuaTiep { get; set; }
        public string KetQuaGQCacNganh { get; set; }
        public int? LoaiTiepDanID { get; set; }
        public int? NhomKNID { get; set; }
        public int? CoQuanID { get; set; }
        public int? CTTiepDinhKyID { get; set; }
        public NhomKNInfo NhomKN { get; set; }
        public List<ThanhPhanThamGiaTĐinhKyInfo> ThanhPhanThamGia { get; set; }
        public List<DanhSachHoSoTaiLieu> DanhSachHoSoTaiLieu { get; set; }
        public List<TiepDanDinhKyModel> Children { get; set; }
    }

    public class ThanhPhanThamGiaTĐinhKyInfo
    {
        public int? ID { get; set; }
        public int? TiepDinhKyID { get; set; }
        public string TenCanBo { get; set; }
        public string ChucVu { get; set; }
    }

    public class ChiTietTiepDanDinhKyModel : TiepDanDinhKyModel
    {
        public int? ID { get; set; }
        public string TenCanBo { get; set; }
        public string ChucVuThanhPhanThamGia { get; set; }
    }
}
