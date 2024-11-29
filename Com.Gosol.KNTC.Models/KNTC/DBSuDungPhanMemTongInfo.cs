using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class DBSuDungPhanMemTongInfo
    {
        public int TongTiepDan { get; set; }
        public int TongTiepDanNhomKN { get; set; }
        public int TongXuLyDon { get; set; }
        public int TongDonThuBHGQ { get; set; }
        public int TongDonThuBHGQ_KN { get; set; }
        public int TongDonThuBHGQ_TC { get; set; }
        public int TongDonThuBHGQ_KNPA { get; set; }
        public DBSuDungPhanMemChartInfo DataChart { get; set; }
    }

    public class DBSuDungPhanMemChartInfo
    {
        public List<DBSuDungPhanMemChartDataInfo> Data { get; set; }
        public string[] Categories { get; set; }
        public int[] CoQuanIDs { get; set; }
        public int[] CoQuanChaIDs { get; set; }
        public int[] CapIDs { get; set; }
    }

    public class DBSuDungPhanMemChartDataInfo
    {
        public string name { get; set; }
        public float[] data { get; set; }
        public string color { get; set; }

        public int[] coquanid { get; set; }
        public int[] coquanchaid { get; set; }

        public int[] capid { get; set; }
    }

    public class DBSuDungPhanMemInfo
    {
        public int CanBoID { get; set; }
        public int CoQuanID { get; set; }
        public string TenCoQuan { get; set; }
        public int CoQuanChaID { get; set; }
        public int TiepDan { get; set; }
        public int TiepDanNhomKN { get; set; }
        public int XuLyDon { get; set; }
        public int DonThuBHGQ { get; set; }
        public int DonThuBHGQ_KN { get; set; }
        public int DonThuBHGQ_TC { get; set; }

        public int CanBoXuLyID { get; set; }
        public string SoDonThu { get; set; }
        public string NguonDonDen { get; set; }
        public string TenNguonDonDen { get; set; }
        public string NguoiDaiDien { get; set; }
        public string NoiDungDon { get; set; }
        public int LoaiKhieuTo1ID { get; set; }
        public string TenLoaiKhieuTo { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime NgayNhapDon { get; set; }
        public int HuongGiaiQuyetID { get; set; }
        public string TenHuongGiaiQuyet { get; set; }
        public int ChoGiaiQuyet { get; set; }
        public int KetQuaID { get; set; }
        public int ChoXuLy { get; set; }
        public int SapHetHanGiaiQuyet { get; set; }
        public int SapHetHanXuLy { get; set; }
        public int DaDuyetXuLy { get; set; }
        public string NgayHienThi { get; set; }


    }
}
