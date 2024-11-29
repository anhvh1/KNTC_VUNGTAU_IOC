using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class BaoCaoPhanAnhKienNghiDonThuInfo
    {
        public int DonThuID { get; set; }
        public int CoQuanID { get; set; }
        public DateTime NgayNhapDon { get; set; }
        public int HuongGiaiQuyetID { get; set; }
        public Boolean TrungDon { get; set; }
        public int LoaiKetQuaID { get; set; }
        public int PhanTichKQID { get; set; }
        public int TienPhaiThu { get; set; }
        public int DatPhaiThu { get; set; }
        public int SoNguoiDuocTraQuyenLoi { get; set; }
        public int SoDoiTuongBiXuLy { get; set; }
        public int SoDoiTuongDaBiXuLy { get; set; }
        public DateTime NgayRaKQ { get; set; }
        public int ThiHanhID { get; set; }
        public int LoaiThiHanhID { get; set; }
        public int TienDaThu { get; set; }
        public int DatDaThu { get; set; }
        public int SoLan { get; set; }
        public DateTime NgayThiHanh { get; set; }
        public int TrangThaiDonID { get; set; }
        public int CQTiepNhanID { get; set; }
        public int RutDonID { get; set; }
        public string CQDaGiaiQuyet { get; set; }
        public int KetQuaGQLan2 { get; set; }
    }
}
