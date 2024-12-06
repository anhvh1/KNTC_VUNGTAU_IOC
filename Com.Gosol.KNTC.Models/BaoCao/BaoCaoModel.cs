using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.BaoCao
{
    public class BaoCaoModel
    {
        public string BieuSo { get; set; }
        public string ThongTinSoLieu { get; set; }
        public string TuNgay { get; set; }  
        public string DenNgay { get; set; }
        public string Title { get; set; }
        public DataTable DataTable { get; set; }
        public string DonViTinh { get; set; }
    }
    public class DataTable
    {
        public List<TableHeader> TableHeader { get; set; }
        public List<TableData> TableData { get; set; }
    }
    public class TableHeader
    {
        public int ID { get; set; }
        public int? ParentID { get; set; }
        public string Name { get; set; }
        public string Style { get; set; }
        public List<TableHeader> DataChild { get; set; }
        public TableHeader()
        {
          
        }
        public TableHeader(int ID, int? ParentID, string Name, string Style, ref List<TableHeader> TableHeader)
        {
            this.ID = ID;
            this.ParentID = ParentID;
            this.Name = Name;  
            this.Style = Style;
            TableHeader.Add(this);
        }
    }

    public class TableData
    {
        public int? ID  { get; set; }
        public int? ParentID { get; set; }
        public List<RowItem> DataArr { get; set; }
        public List<TableData> DataChild { get; set; }
        public int? DonThuID { get; set; }
        public int? XuLyDonID { get; set; }
        public bool? isClick { get; set; }
        public int? CoQuanID { get; set; }
        public int? LoaiBaoCao { get; set; }
        public DateTime? NgaySuDung { get; set; }
    }

    public class RowItem
    {
        public int ID { get; set; }
        public string Content  { get; set; }
        public string CoQuanID { get; set; }
        public string CapID { get; set; }
        public string LoaiKhieuToID { get; set; }
        public bool? isEdit { get; set; }
        public string Style { get; set; }
        public int? TypeEdit { get; set; }
        public bool? Continue { get; set; }
        public bool? Ending { get; set; }
        public List<int> ListTotal { get; set; }
        public RowItem()
        {   

        }
        public RowItem(int ID, string Content, string CoQuanID, string CapID, bool? isEdit, string Style, ref List<RowItem> DataArr)
        {
            this.ID = ID;
            this.Content = Content; 
            this.CoQuanID = CoQuanID;
            this.CapID = CapID;
            this.isEdit = isEdit;
            this.Style = Style;
            DataArr.Add(this);
        }

        public RowItem(int ID1, string Content, string CoQuanID, string CapID, bool? isEdit, string Style, int? TypeEdit, ref List<RowItem> DataArr)
        {
            this.ID = ID1;
            this.Content = Content;
            this.CoQuanID = CoQuanID;
            this.CapID = CapID;
            this.isEdit = isEdit;
            this.Style = Style;
            this.TypeEdit = TypeEdit;
            DataArr.Add(this);
        }

        public RowItem(int ID2, string Content, string CoQuanID, string CapID, bool? isEdit, string Style, List<int> ListTotal, int? TypeEdit, ref List<RowItem> DataArr)
        {
            this.ID = ID2;
            this.Content = Content;
            this.CoQuanID = CoQuanID;
            this.CapID = CapID;
            this.isEdit = isEdit;
            this.Style = Style;
            this.ListTotal = ListTotal;
            this.TypeEdit = TypeEdit;
            DataArr.Add(this);
        }

        public RowItem(int ID3, string Content, string CoQuanID, string CapID, string LoaiKhieuToID, bool? isEdit, string Style, ref List<RowItem> DataArr)
        {
            this.ID = ID3;
            this.Content = Content;
            this.CoQuanID = CoQuanID;
            this.CapID = CapID;
            this.LoaiKhieuToID = LoaiKhieuToID;
            this.isEdit = isEdit;
            this.Style = Style;
            this.ListTotal = ListTotal;
            DataArr.Add(this);
        }
        
    }
    public class ThongKeBC_DongBo_IOC
    {
        public long? Id { get; set; }
        public int? CoQuanId { get; set; }
        public string? TenCoQuan { get; set; }
        public int? TongSoLuotTCD { get; set; }
        public int? TongSoLuotTTX { get; set; }
        public int? TongSoLuotTTT { get; set; }
        public int? TongSoLuotUQT { get; set; }
        public int? TongSoDonXLD { get; set; }
        public int? TSDXLDThuocThamQuyen { get; set; }
        public int? TSDXLDKhongThuocThamQuyen { get; set; }
        public int? TSDXLDToCao { get; set; }
        public int? TSDXLDToCaoThuocThamQuyen { get; set; }
        public int? TSDXLDKhieuNai { get; set; }
        public int? TSDXLDKhieuNaiThuocThamQuyen { get; set; }
        public int? TSDXLDKhienNghiPhanAnh { get; set; }
        public int? TSDXLDKhienNghiPhanAnhTTQ { get; set; }
        public int? LoaiBaoCao { get; set; }
        public int? Thang { get; set; }
        public int? Nam { get; set; }
        public int? NgayTao { get; set; }
        public int? NguoiTao { get; set; }
        public int? NgaySua { get; set; }
        public int? NguoiSua { get; set; }
    }

    public class ThongKeBC_2a_DongBo_IOC
    {
        public long Id { get; set; }
        public int? CoQuanId { get; set; }
        public string? TenCoQuan { get; set; }
        public int? TongSoLuotTCD { get; set; }
        public int? TongSoLuotTTX { get; set; }
        public int? TongSoLuotTTT { get; set; }
        public int? TongSoLuotUQT { get; set; }
        public int? Thang { get; set; }
        public int? Nam { get; set; }

    }
    public class ThongKeBC_2a_DongBo_IOC_Request
    {
        public int? CoQuanId { get; set; }
        public int? TongSoLuotTCD { get; set; }
        public int? TongSoLuotTTX { get; set; }
        public int? TongSoLuotTTT { get; set; }
        public int? TongSoLuotUQT { get; set; }
        public int? Thang { get; set; }
        public int? Nam { get; set; }

    }
    public class ThongKeBC_2a_DongBo_IOC_UpdateRequest
    {
        public long Id { get; set; }
        public int? CoQuanId { get; set; }
        public int? TongSoLuotTCD { get; set; }
        public int? TongSoLuotTTX { get; set; }
        public int? TongSoLuotTTT { get; set; }
        public int? TongSoLuotUQT { get; set; }
        public int? Thang { get; set; }
        public int? Nam { get; set; }

    }
    public class ThongKeBC_2b_DongBo_IOC
    {
        public long Id { get; set; }
        public int? CoQuanId { get; set; }
        public string? TenCoQuan { get; set; }
        public int? TongSoDonXLD { get; set; }
        public int? TSDXLDThuocThamQuyen { get; set; }
        public int? TSDXLDKhongThuocThamQuyen { get; set; }
        public int? TSDXLDToCao { get; set; }
        public int? TSDXLDToCaoThuocThamQuyen { get; set; }
        public int? TSDXLDKhieuNai { get; set; }
        public int? TSDXLDKhieuNaiThuocThamQuyen { get; set; }
        public int? TSDXLDKienNghiPhanAnh { get; set; }
        public int? TSDXLDKienNghiPhanAnhTTQ { get; set; }
        public int? Thang { get; set; }
        public int? Nam { get; set; }

    }
    public class FilterDongBo_IOC
    {
        public int? Thang { get; set; }
        public int? Nam { get; set; }
    }
    public class ThongKeBC_2b_DongBo_IOC_Request
    {
        public int? CoQuanId { get; set; }
        public int? TongSoDonXLD { get; set; }
        public int? TSDXLDThuocThamQuyen { get; set; }
        public int? TSDXLDKhongThuocThamQuyen { get; set; }
        public int? TSDXLDToCao { get; set; }
        public int? TSDXLDToCaoThuocThamQuyen { get; set; }
        public int? TSDXLDKhieuNai { get; set; }
        public int? TSDXLDKhieuNaiThuocThamQuyen { get; set; }
        public int? TSDXLDKienNghiPhanAnh { get; set; }
        public int? TSDXLDKienNghiPhanAnhTTQ { get; set; }
        public int? Thang { get; set; }
        public int? Nam { get; set; }

    }
    public class ThongKeBC_2b_DongBo_IOC_UpdateRequest
    {
        public long Id { get; set; }
        public int? CoQuanId { get; set; }
        public int? TongSoDonXLD { get; set; }
        public int? TSDXLDThuocThamQuyen { get; set; }
        public int? TSDXLDKhongThuocThamQuyen { get; set; }
        public int? TSDXLDToCao { get; set; }
        public int? TSDXLDToCaoThuocThamQuyen { get; set; }
        public int? TSDXLDKhieuNai { get; set; }
        public int? TSDXLDKhieuNaiThuocThamQuyen { get; set; }
        public int? TSDXLDKienNghiPhanAnh { get; set; }
        public int? TSDXLDKienNghiPhanAnhTTQ { get; set; }
        public int? Thang { get; set; }
        public int? Nam { get; set; }

    }
}
