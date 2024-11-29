using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.DanhMuc
{
    public class DanhMucDiaGioiHanhChinhMOD_v2 
    {
        public int? TinhID { get; set; }
        public int? HuyenID { get; set; }
        public int? XaID { get; set; }
        public string TenTinh { get; set; }
        public string TenHuyen { get; set; }
        public string TenXa { get; set; }
        public string TenDayDu { get; set; }
        public string MappingCode { get; set; }
        public int? LoaiDiaDanh { get; set; }

        public DanhMucDiaGioiHanhChinhMOD_v2()
        {

        }
        public DanhMucDiaGioiHanhChinhMOD_v2(int TinhID, int HuyenID, int XaID, string tenTinh, string tenHuyen, string tenXa, string MappingCode , int LoaiDiaDanh)
        {
            this.TinhID = TinhID;
            this.HuyenID = HuyenID;
            this.XaID = XaID;
            this.TenTinh = tenTinh;
            this.TenHuyen = tenHuyen;
            this.TenXa = tenXa;
            this.MappingCode = MappingCode;
            this.LoaiDiaDanh = LoaiDiaDanh;

        }

       
    }
    public class DanhMucDiaGioiHanhChinhMODPartial_v2
    {
        public int ID { get; set; }
        public string Ten { get; set; }
        public string TenDayDu { get; set; }
        public int LoaiDiaDanh { get; set; }
        public int? TotalChildren { get; set; }
        public int? ParentID { get; set; }
        public int? Cap { get; set; }
        public int? Highlight { get; set; }

        

        public DanhMucDiaGioiHanhChinhMODPartial_v2()
        {

        }
        //- tu them--
       
        public DanhMucDiaGioiHanhChinhMODPartial_v2(int ID, string Ten, string TenDayDu,int LoaiDiaDanh, int TotalChildren, int Cap, int Highlight)
        {
            this.ID = ID;
            this.Ten = Ten;
            this.TenDayDu = TenDayDu;
            this.LoaiDiaDanh = LoaiDiaDanh;
            this.TotalChildren = TotalChildren;
            this.Cap = Cap;
            this.Highlight = Highlight;
        }


       
    }
    public class DanhMucDiaGioiHanhChinhMODlUpdatePartial_v2
    {
        public int ID { get; set; }
        public string Ten { get; set; }
        public string TenDayDu { get; set; }
        public int? TotalChildren { get; set; }
        public int? TinhID { get; set; }
        public int? HuyenID { get; set; }
        public int? Cap { get; set; }
        public int? Highlight { get; set; }

        //public int? LoaiDiaDanh { get; set; }
        public DanhMucDiaGioiHanhChinhMODlUpdatePartial_v2()
        {

        }
        public DanhMucDiaGioiHanhChinhMODlUpdatePartial_v2(int ID, string Ten, string TenDayDu, int TinhID, int HuyenID, int Cap, int TotalChildren, int Highlight )
        {
            this.ID = ID;
            this.Ten = Ten;
            this.TenDayDu = TenDayDu;
            this.TotalChildren = TotalChildren;
            this.HuyenID = HuyenID;
            this.TinhID = TinhID;
            this.Cap = Cap;
            this.Highlight = Highlight;
           
        }
        
    }

    public class ThamSoLocDanhMuc_v2 : ThamSoLocDanhMuc
    {
       
        public int? Cap { get; set; }
    }

    

}
