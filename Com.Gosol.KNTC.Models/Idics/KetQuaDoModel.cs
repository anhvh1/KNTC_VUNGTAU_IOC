using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.Idics
{
    public class KetQuaDoModel
    {
        public int? LanDoID { get; set; }
        public DateTime? NgayDo { get; set; }
        public string DiaDiem { get; set; }
        public ExaminationDataModel ExaminationDataEntity { get; set; }
        public PhysicalBodyCompositionModel PhysicalBodyCompositionEntity { get; set; }
        public UserModel UserEntity { get; set; }
    }

    public class PhepDoModel
    {
        public int PhepDoID { get; set; }
        public int? PhepDoChaID { get; set; }
        public string TenPhepDo { get; set; }
        public string MaPhepDo { get; set; }
        public string DonViDo { get; set; }
        public string KetQua { get; set; }
        public string ThamChieu { get; set; }
        public string TrangThai { get; set; }
        public decimal GioiHanTren { get; set; }
        public decimal GioiHanDuoi { get; set; }
        public string GhiChu { get; set; }
        public int HienThi { get; set; }
    }

    public class ThongTinLanDoModel
    {
        public int? LanDoID { get; set; }
        public DateTime? NgayDo { get; set; }
        public string DiaDiem { get; set; }
        public string Barcode { get; set; }
        public string Ten { get; set; }
        public string NamSinh { get; set; }
        public string GioiTinh { get; set; }
        public string CMND { get; set; }
        public string DienThoai { get; set; }
        public string DiaChi { get; set; }
        public List<PhepDoModel> DanhSachPhepDo { get; set; }
    }
}
