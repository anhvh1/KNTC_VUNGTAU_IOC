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
}
