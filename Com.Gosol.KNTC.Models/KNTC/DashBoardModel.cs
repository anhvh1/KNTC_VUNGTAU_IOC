using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class DashBoardModel
    {
        public List<Data> SoLieuTongHop { get; set; }
        public List<BieuDoCot> SoLieuBieuDoCot { get; set; }
        public List<Data> SoLieuBieuTron { get; set; }
        public List<Data> SoLieuBieuDoTronCungKy { get; set; }   
        public List<SoLieuCanhBao> SoLieuCanhBao { get; set; }   
        public List<int> ListCapID { get; set; }
        public List<BCTinhHinhTD_XLD_GQInfo> lsData { get; set; }
    }

    public class Data
    {
        public String Key { get; set; }
        public decimal Value { get; set; }
        public Data (String Key, decimal Value)
        { 
            this.Key = Key;
            this.Value = Value;
        }
    }

    public class BieuDoCot
    {
        public string TenCot { get; set; }
        public int LoaiCot { get; set; }
        public int CapID { get; set; }
        public int TrongKy { get; set; }
        public int CungKy { get; set; }
        public int DaXuLy { get; set; }
        public int DaGiaiQuyet { get; set; }
        public int ChuaGiaiQuyet { get; set; }
        public List<Data> Data { get; set; }
        public BieuDoCot(string TenCot, int LoaiCot, int CapID, int TrongKy, int CungKy, int DaXuLy, int DaGiaiQuyet, int ChuaGiaiQuyet)
        {
            this.TenCot = TenCot;
            this.LoaiCot = LoaiCot;
            this.CapID = CapID;
            this.TrongKy = TrongKy;
            this.CungKy = CungKy;
            this.DaXuLy = DaXuLy;
            this.DaGiaiQuyet = DaGiaiQuyet;
            this.ChuaGiaiQuyet = ChuaGiaiQuyet;
            Data = new List<Data>();
            Data.Add(new Data("Trong kỳ", TrongKy));
            Data.Add(new Data("Cùng kỳ", CungKy));
            Data.Add(new Data("Đã xử lý", DaXuLy));
            Data.Add(new Data("Đã giải quyết", DaGiaiQuyet));
            Data.Add(new Data("Chưa giải quyết", ChuaGiaiQuyet));
        }
    }

    public class SoLieuCanhBao
    {
        public string TenCanhBao { get; set; }
        public string MaChucNang { get; set; }
        public List<Data> Data { get; set; }
       
    }

    public class SoLieuModel
    {
        public int CanXuLy { get; set; }
        public int DaXuLy { get; set; }
        public int CanTrinhKetQua { get; set; }
        public int DaTrinhKetQua { get; set; }
        public int CanCapNhat { get; set; }
        public int DaCapNhat { get; set; }

        public int CanGiaoXacMinh { get; set; }
        public int DaGiaoXacMinh { get; set; }
        public int CanDuyetBCXacMinh { get; set; }
        public int DaDuyetBCXacMinh { get; set; }
        public int CanBanHanhQDGQ { get; set; }
        public int DaBanHanhQDGQ { get; set; }


        public int CanPheDuyet { get; set; }
        public int DaPheDuyet { get; set; }
        public int CanTrinhDuThao { get; set; }
        public int DaTrinhDuThao { get; set; }
        public int QuaHan { get; set; }
        public int DenHan { get; set; }
        public int ChuaDenHan { get; set; }

        public int CanBanHanhGXM { get; set; }
        public int DaBanHanhGXM { get; set; }
        public int CanBanHanhGQ { get; set; }
        public int DaBanHanhGQ { get; set; }
        public int QuaHanBanHanh { get; set; }
        public int DenHanBanHanh { get; set; }
        public int ChuaDenHanBanHanh { get; set; }


        public int CanCapNhatNDQDGXM { get; set; }
        public int DaCapNhatNDQDGXM { get; set; }
        public int CanCapNhatBCQDKL { get; set; }
        public int DaCapNhatBCQDKL { get; set; }

        public int CanThiHanh { get; set; }
        public int DaThiHanh { get; set; }

        public int CanXMDonThu { get; set; }
        public int DaXMDonThu { get; set; }

    }
}
