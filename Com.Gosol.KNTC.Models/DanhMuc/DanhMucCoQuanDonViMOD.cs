using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.DanhMuc
{
    public class DanhMucCoQuanDonViMOD
    {
        [Required]
        public int CoQuanID { get; set; }

        public string TenCoQuan { get; set; }
        public int? CoQuanChaID { get; set; }
        public int? CapID { get; set; }
        public int? ThamQuyenID { get; set; }
        public int? TinhID { get; set; }
        public int? HuyenID { get; set; }
        public int? XaID { get; set; }
        public bool? CapUBND { get; set; }
        public bool? CQCoHieuLuc { get; set; }
        public bool? CapThanhTra { get; set; }

        public bool? SuDungPM { get; set; }
        public string MaCQ { get; set; }
        public bool? SuDungQuyTrinh { get; set; }

        public bool? SuDungQuyTrinhGQ { get; set; }

        public bool? QTVanThuTiepNhanDon { get; set; }

        public bool? QTVanThuTiepDan { get; set; }

        //public int QuyTrinhGianTiep { get; set; }



        public List<DanhMucCoQuanDonViMOD> Children { get; set; }
        public DanhMucCoQuanDonViMOD() { }
        public DanhMucCoQuanDonViMOD(int CoQuanID, string TenCoQuan, int CoQuanChaID, int CapID, int ThamQuyenID, int TinhID, int HuyenID, int XaID, bool CapUBND, bool CapThanhTra, bool CQCoHieuLuc, bool SuDungPM, string MaCQ, bool SuDungQuyTrinh , bool SuDungQuyTrinhGQ , bool QTVanThuTiepNhanDon , bool QTVanThuTiepDan)
        {

            this.CoQuanID = CoQuanID;
            this.TenCoQuan = TenCoQuan;
            this.CoQuanChaID = CoQuanChaID;
            this.CapID = CapID;
            this.ThamQuyenID = ThamQuyenID;
            this.TinhID = TinhID;
            this.HuyenID = HuyenID;
            this.XaID = XaID;
            this.CapUBND = CapUBND;
            this.CapThanhTra = CapThanhTra;
            this.CQCoHieuLuc = CQCoHieuLuc;
            this.SuDungPM = SuDungPM;
            this.MaCQ = MaCQ;
            this.SuDungQuyTrinh = SuDungQuyTrinh;
            this.SuDungQuyTrinhGQ = SuDungQuyTrinhGQ;
            this.QTVanThuTiepNhanDon = QTVanThuTiepNhanDon;
            this.QTVanThuTiepDan = QTVanThuTiepDan;
        }
    }
    
    public class AddDanhMucCoQuanMOD
    {
        public string TenCoQuan { get; set; }
        public int? CoQuanChaID { get; set; }
        public int? CapID { get; set; }
        public int? ThamQuyenID { get; set; }
        public int? TinhID { get; set; }
        public int? HuyenID { get; set; }
        public int? XaID { get; set; }
        public bool? CapUBND { get; set; }
        public bool? CQCoHieuLuc { get; set; }
        public bool? CapThanhTra { get; set; }

        public bool? SuDungPM { get; set; }
        public string MaCQ { get; set; }
        public bool? SuDungQuyTrinh { get; set; }

        public bool? SuDungQuyTrinhGQ { get; set; }

        public bool? QTVanThuTiepNhanDon { get; set; }

        public bool? QTVanThuTiepDan { get; set; }

        //public int QuyTrinhGianTiep { get; set; }

    }
    public class UpdateDanhMucCoQuanMOD
    {

        public int CoQuanID { get; set; }


        public string TenCoQuan { get; set; }
        public int? CoQuanChaID { get; set; }
        public int? CapID { get; set; }
        public int? ThamQuyenID { get; set; }
        public int? TinhID { get; set; }
        public int? HuyenID { get; set; }
        public int? XaID { get; set; }
        public bool? CapUBND { get; set; }
        public bool? CQCoHieuLuc { get; set; }
        public bool? CapThanhTra { get; set; }

        public bool? SuDungPM { get; set; }
        public string MaCQ { get; set; }
        public bool? SuDungQuyTrinh { get; set; }

        public bool? SuDungQuyTrinhGQ { get; set; }

        public bool? QTVanThuTiepNhanDon { get; set; }

        public bool? QTVanThuTiepDan { get; set; }

    }
    //------
    public class DanhMucCoQuanDonViMODPartial
    {

        public int ID { get; set; }
        public string Ten { get; set; }
        public string MaCQ { get; set; }
        public int CoQuanChaID { get; set; }
        public int? Highlight { get; set; }
        public int? Cap { get; set; }
       
        public int? ParentID { get; set; }
        public List<DanhMucCoQuanDonViMODPartial> Children { get; set; }
        public List<DanhMucCoQuanDonViMODPartial> Parent { get; set; }
        public DanhMucCoQuanDonViMODPartial()
        {

        }
        public DanhMucCoQuanDonViMODPartial(int ID, string Ten, string MaCQ, int Highlight, int Cap, int ParentID, List<DanhMucCoQuanDonViMODPartial> Children, List<DanhMucCoQuanDonViMODPartial> Parent)
        {
            this.ID = ID;
            this.Ten = Ten;
            this.MaCQ = MaCQ;
            this.Highlight = Highlight;
            this.Cap = Cap;
            this.ParentID = ParentID;
            this.Children = Children;
            this.Parent = Parent;
        }
    }
    public class DanhMuKNTCoQuanMODPar : DanhMucCoQuanDonViMOD
    {
        public List<DanhMucCoQuanDonViMOD> Children1 { get; set; }
        public List<DanhMuKNTCoQuanPar> Parent1 { get; set; }
        public DanhMuKNTCoQuanMODPar()
        {

        }
        public DanhMuKNTCoQuanMODPar(List<DanhMucCoQuanDonViMOD> Children, List<DanhMuKNTCoQuanPar> Parent)
        {
            this.Children1 = Children1;
            this.Parent1 = Parent1;
        }


    }
    //DanhMucCoQuanDonViMOD
    public class DanhMuKNTCoQuanDonViMODModel : DanhMucCoQuanDonViMOD
    {
        public List<DanhMuKNTCoQuanDonViPartialModel> Children1 { get; set; }
        public DanhMuKNTCoQuanDonViMODModel(
            int CoQuanID, string TenCoQuan, int CoQuanChaID,
            string MaCQ, List<DanhMuKNTCoQuanDonViMODModel> Children)
        {
            this.CoQuanID = CoQuanID;
            this.TenCoQuan = TenCoQuan;
            this.CoQuanChaID = CoQuanChaID;
            this.MaCQ = MaCQ;
            this.Children1 = Children1;
        }
    }
    public class DanhMucCoQuanDonViMODPartialNew : DanhMucCoQuanDonViMOD
    {
        public string DiaChi { get; set; }
        public string TenCoQuanCha { get; set; }
        //public int CoQuanSuDungPhanMemID { get; set; }
        public DanhMucCoQuanDonViMODPartialNew()
        {

        }
        public DanhMucCoQuanDonViMODPartialNew(string DiaChi, string TenCoQuanCha)
        {
            this.DiaChi = DiaChi;
            this.TenCoQuanCha = TenCoQuanCha;
        }
    }

    public class CoQuanTESTMOD
    { 
        public int CoQuanID { get; set; }
        public string TenCoQuan { get; set; }
        public int CoQuanChaID { get; set; }
        //public int CoQuanSuDungPhanMemID { get; set; }
    }
    public class NameCapCoQuanID
    {

        public int Cap { get; set; }
        public string TenCap { get; set; }

        public int ThuTu { set; get; }
    }

    public class CapBac
    {
        public int? ThamQuyenID { get; set; }
        public string TenThamQuyen { get; set; }
        public int? MaThamQuyen { set; get; } 
        
    }
    public class CoQuanCha
    {
       
        public string CQCha { set; get; }
       
    }

   
    public class LocCap : DanhMucCoQuanDonViMODPartial
    {
        public int TenCap { get; set; } 
    }
}
