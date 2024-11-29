using Com.Gosol.KNTC.DAL.BaoCao;
using Com.Gosol.KNTC.DAL.DanhMuc;
using Com.Gosol.KNTC.DAL.HeThong;
using Com.Gosol.KNTC.DAL.KNTC;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.BaoCao;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Security;
using Com.Gosol.KNTC.Ultilities;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using DocumentFormat.OpenXml.InkML;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTable = Com.Gosol.KNTC.Models.BaoCao.DataTable;

namespace Com.Gosol.KNTC.BUS.BaoCao
{
    public class BaoCaoBUS
    {
        private BaoCaoDAL baoCaoDAL;
        public BaoCaoBUS()
        {
            baoCaoDAL = new BaoCaoDAL();
        }

        public List<CoQuanInfo> DanhSachCoQuan(IdentityHelper IdentityHelper)
        {
            List<CoQuanInfo> cqList = new List<CoQuanInfo>();
            var listThanhTraTinh = new SystemConfigDAL().GetByKey("Thanh_Tra_Tinh_ID").ConfigValue.Split(',').ToList().Select(x => Utils.ConvertToInt32(x.ToString(), 0)).ToList();
            if ((listThanhTraTinh.Contains(IdentityHelper.CoQuanID ?? 0) || IdentityHelper.CapID == CapQuanLy.CapUBNDTinh.GetHashCode()))
            {
                if (IdentityHelper.RoleID == (int)EnumChucVu.LanhDao)
                {
                    var CoQuanChaPhuHop = new CoQuan().GetCoQuanByTinhID_New(IdentityHelper.TinhID);
                    cqList = new CoQuan().GetAllCapCon(CoQuanChaPhuHop.CoQuanID).ToList().Where(x => x.SuDungPM == true)
                      .ToList();
                }
                else
                {
                    cqList = new CoQuan().GetAllCapCon(IdentityHelper.CoQuanID ?? 0).ToList().Where(x => x.SuDungPM == true)
                     .ToList();
                }
            }
            else if (IdentityHelper.CapID == CapQuanLy.CapUBNDHuyen.GetHashCode())
            {
                if (IdentityHelper.RoleID == (int)EnumChucVu.LanhDao)
                {
                    var CoQuanTinh = new CoQuan().GetCoQuanByTinhID_New(IdentityHelper.TinhID);
                    var CoQuanChaPhuHop = new CoQuan().GetCoQuanByHuyenID_New(IdentityHelper.HuyenID, IdentityHelper.TinhID, CoQuanTinh.CoQuanID);
                    cqList = new CoQuan().GetAllCapCon(CoQuanChaPhuHop.CoQuanID).ToList();
                    cqList = cqList.Where(x => x.SuDungPM == true).ToList();
                }
                else
                {
                    var CoQuanTinh = new CoQuan().GetCoQuanByTinhID_New(IdentityHelper.TinhID);
                    var CoQuanChaPhuHop = new CoQuan().GetCoQuanByHuyenID_New(IdentityHelper.HuyenID, IdentityHelper.TinhID, CoQuanTinh.CoQuanID);
                    cqList = new CoQuan().GetAllCapCon(CoQuanChaPhuHop.CoQuanID).Where(x => (x.CapID == CapQuanLy.CapUBNDHuyen.GetHashCode()
                     || x.CapID == CapQuanLy.CapPhong.GetHashCode())).ToList();
                    cqList = cqList.Where(x => x.SuDungPM == true).ToList();

                }

            }
            return cqList;
        }
        public IList<LoaiKhieuToInfo> DanhMucLoaiKhieuTo(int LoaiKhieuToID)
        {
            var List = new LoaiKhieuTo().GetLoaiKhieuTos().ToList();
            List.ForEach(x => x.Children = List.Where(y => y.LoaiKhieuToCha == x.LoaiKhieuToID).ToList());
            List.RemoveAll(x => x.LoaiKhieuToCha > 0);
            return List;
        }
        public BaseResultModel TCD01(BaseReportParams p, string ContentRootPath, int RoleID, int CapID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, int HuyenDangNhapID)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.BieuSo = "Biểu số: 01/TCD";
                BaoCaoModel.ThongTinSoLieu = "Số liệu tính từ ngày " + (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy") + " đến ngày " + (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.TuNgay = (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.DenNgay = (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.Title = "TỔNG HỢP KẾT QUẢ TIẾP CÔNG DÂN THƯỜNG XUYÊN, ĐỊNH KỲ VÀ ĐỘT XUẤT";
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();
                #region Header
                // Cấp 1

                TableHeader HeaderCol1 = new TableHeader();
                HeaderCol1.Name = "Đơn vị";
                HeaderCol1.ID = 1;
                HeaderCol1.Style = "width: 215px";
                BaoCaoModel.DataTable.TableHeader.Add(HeaderCol1);

                TableHeader HeaderCol2 = new TableHeader();
                HeaderCol2.Name = "Tổng số lượt tiếp";
                HeaderCol2.ID = 2;
                HeaderCol2.Style = "";
                BaoCaoModel.DataTable.TableHeader.Add(HeaderCol2);

                TableHeader HeaderCol3 = new TableHeader();
                HeaderCol3.Name = "Tổng số người được tiếp";
                HeaderCol3.ID = 3;
                HeaderCol3.Style = "";
                BaoCaoModel.DataTable.TableHeader.Add(HeaderCol3);

                TableHeader HeaderCol4 = new TableHeader();
                HeaderCol4.Name = "Tổng số vụ việc tiếp";
                HeaderCol4.ID = 4;
                HeaderCol4.Style = "";
                BaoCaoModel.DataTable.TableHeader.Add(HeaderCol4);

                TableHeader HeaderCol5 = new TableHeader();
                HeaderCol5.Name = "Tiếp thường xuyên";
                HeaderCol5.ID = 5;
                HeaderCol5.DataChild = new List<TableHeader>();
                BaoCaoModel.DataTable.TableHeader.Add(HeaderCol5);
                
                TableHeader HeaderCol6 = new TableHeader();
                HeaderCol6.Name = "Tiếp định kỳ và đột xuất của Thủ trưởng";
                HeaderCol6.ID = 6;
                HeaderCol6.DataChild = new List<TableHeader>();
                BaoCaoModel.DataTable.TableHeader.Add(HeaderCol6);        
                
                //Cấp 2

                TableHeader HeaderCol5_1 = new TableHeader();
                HeaderCol5_1.Name = "Số lượt tiếp";
                HeaderCol5_1.ID = 7;
                HeaderCol5_1.ParentID = 5;
                HeaderCol5_1.Style = "";
                HeaderCol5.DataChild.Add(HeaderCol5_1);

                TableHeader HeaderCol5_2 = new TableHeader();
                HeaderCol5_2.Name = "Số người được tiếp";
                HeaderCol5_2.ID = 8;
                HeaderCol5_2.ParentID = 5;
                HeaderCol5_2.Style = "";
                HeaderCol5.DataChild.Add(HeaderCol5_2);

                TableHeader HeaderCol5_3 = new TableHeader();
                HeaderCol5_3.Name = "Số vụ việc";
                HeaderCol5_3.ID = 9;
                HeaderCol5_3.ParentID = 5;
                HeaderCol5_3.DataChild = new List<TableHeader>();
                HeaderCol5.DataChild.Add(HeaderCol5_3);

                TableHeader HeaderCol5_4 = new TableHeader();
                HeaderCol5_4.Name = "Trong đó đoàn đông người";
                HeaderCol5_4.ID = 10;
                HeaderCol5_4.ParentID = 5;
                HeaderCol5_4.DataChild = new List<TableHeader>();
                HeaderCol5.DataChild.Add(HeaderCol5_4);

                TableHeader HeaderCol6_1 = new TableHeader();
                HeaderCol6_1.Name = "Thủ trưởng tiếp";
                HeaderCol6_1.ID = 11;
                HeaderCol6_1.ParentID = 6;
                HeaderCol6_1.DataChild = new List<TableHeader>();
                HeaderCol6.DataChild.Add(HeaderCol6_1);

                TableHeader HeaderCol6_2 = new TableHeader();
                HeaderCol6_2.Name = "ủy quyền tiếp";
                HeaderCol6_2.ID = 12;
                HeaderCol6_2.ParentID = 6;
                HeaderCol6_2.DataChild = new List<TableHeader>();
                HeaderCol6.DataChild.Add(HeaderCol6_2);

                //Cấp 3

                TableHeader HeaderCol5_3_1 = new TableHeader();
                HeaderCol5_3_1.Name = "Tiếp lần đầu";
                HeaderCol5_3_1.ID = 13;
                HeaderCol5_3_1.ParentID = 9;
                HeaderCol5_3_1.Style = "";
                HeaderCol5_3.DataChild.Add(HeaderCol5_3_1);

                TableHeader HeaderCol5_3_2 = new TableHeader();
                HeaderCol5_3_2.Name = "Tiếp nhiều lần";
                HeaderCol5_3_2.ID = 14;
                HeaderCol5_3_2.ParentID = 9;
                HeaderCol5_3_2.Style = "";
                HeaderCol5_3.DataChild.Add(HeaderCol5_3_2);
         
                TableHeader HeaderCol5_4_1 = new TableHeader();
                HeaderCol5_4_1.Name = "Số đoàn được tiếp";
                HeaderCol5_4_1.ID = 15;
                HeaderCol5_4_1.ParentID = 10;
                HeaderCol5_4_1.Style = "";
                HeaderCol5_4.DataChild.Add(HeaderCol5_4_1);

                TableHeader HeaderCol5_4_2 = new TableHeader();
                HeaderCol5_4_2.Name = "Số người được tiếp";
                HeaderCol5_4_2.ID = 16;
                HeaderCol5_4_2.ParentID = 10;
                HeaderCol5_4_2.Style = "";
                HeaderCol5_4.DataChild.Add(HeaderCol5_4_2);

                TableHeader HeaderCol5_4_3 = new TableHeader();
                HeaderCol5_4_3.Name = "Tiếp lần đầu";
                HeaderCol5_4_3.ID = 17;
                HeaderCol5_4_3.ParentID = 10;
                HeaderCol5_4_3.Style = "";
                HeaderCol5_4.DataChild.Add(HeaderCol5_4_3);

                TableHeader HeaderCol5_4_4 = new TableHeader();
                HeaderCol5_4_4.Name = "Tiếp nhiều lần";
                HeaderCol5_4_4.ID = 18;
                HeaderCol5_4_4.ParentID = 10;
                HeaderCol5_4_4.Style = "";
                HeaderCol5_4.DataChild.Add(HeaderCol5_4_4);

                TableHeader HeaderCol6_1_1 = new TableHeader();
                HeaderCol6_1_1.Name = "Số kỳ tiếp";
                HeaderCol6_1_1.ID = 19;
                HeaderCol6_1_1.ParentID = 11;
                HeaderCol6_1_1.Style = "";
                HeaderCol6_1.DataChild.Add(HeaderCol6_1_1);

                TableHeader HeaderCol6_1_2 = new TableHeader();
                HeaderCol6_1_2.Name = "Số lượt tiếp";
                HeaderCol6_1_2.ID = 20;
                HeaderCol6_1_2.ParentID = 11;
                HeaderCol6_1_2.Style = "";
                HeaderCol6_1.DataChild.Add(HeaderCol6_1_2);

                TableHeader HeaderCol6_1_3 = new TableHeader();
                HeaderCol6_1_3.Name = "Số người được tiếp";
                HeaderCol6_1_3.ID = 21;
                HeaderCol6_1_3.ParentID = 11;
                HeaderCol6_1_3.Style = "";
                HeaderCol6_1.DataChild.Add(HeaderCol6_1_3);

                TableHeader HeaderCol6_1_4 = new TableHeader();
                HeaderCol6_1_4.Name = "Số vụ việc";
                HeaderCol6_1_4.ID = 22;
                HeaderCol6_1_4.ParentID = 11;
                HeaderCol6_1_4.DataChild = new List<TableHeader>();
                HeaderCol6_1.DataChild.Add(HeaderCol6_1_4);

                TableHeader HeaderCol6_1_5 = new TableHeader();
                HeaderCol6_1_5.Name = "Trong đó đoàn đông người";
                HeaderCol6_1_5.ID = 23;
                HeaderCol6_1_5.ParentID = 11;
                HeaderCol6_1_5.DataChild = new List<TableHeader>();
                HeaderCol6_1.DataChild.Add(HeaderCol6_1_5);


                TableHeader HeaderCol6_2_1 = new TableHeader();
                HeaderCol6_2_1.Name = "Số kỳ tiếp";
                HeaderCol6_2_1.ID = 24;
                HeaderCol6_2_1.ParentID = 12;
                HeaderCol6_2_1.Style = "";
                HeaderCol6_2.DataChild.Add(HeaderCol6_2_1);

                TableHeader HeaderCol6_2_2 = new TableHeader();
                HeaderCol6_2_2.Name = "Số lượt tiếp";
                HeaderCol6_2_2.ID = 25;
                HeaderCol6_2_2.ParentID = 12;
                HeaderCol6_2_2.Style = "";
                HeaderCol6_2.DataChild.Add(HeaderCol6_2_2);

                TableHeader HeaderCol6_2_3 = new TableHeader();
                HeaderCol6_2_3.Name = "Số người được tiếp";
                HeaderCol6_2_3.ID = 26;
                HeaderCol6_2_3.ParentID = 12;
                HeaderCol6_2_3.Style = "";
                HeaderCol6_2.DataChild.Add(HeaderCol6_2_3);

                TableHeader HeaderCol6_2_4 = new TableHeader();
                HeaderCol6_2_4.Name = "Số vụ việc";
                HeaderCol6_2_4.ID = 27;
                HeaderCol6_2_4.ParentID = 12;
                HeaderCol6_2_4.DataChild = new List<TableHeader>();
                HeaderCol6_2.DataChild.Add(HeaderCol6_2_4);

                TableHeader HeaderCol6_2_5 = new TableHeader();
                HeaderCol6_2_5.Name = "Trong đó đoàn đông người";
                HeaderCol6_2_5.ID = 28;
                HeaderCol6_2_5.ParentID = 12;
                HeaderCol6_2_5.DataChild = new List<TableHeader>();
                HeaderCol6_2.DataChild.Add(HeaderCol6_2_5);

                //Cấp 4

                TableHeader HeaderCol6_1_4_1 = new TableHeader();
                HeaderCol6_1_4_1.Name = "Tiếp lần đầu";
                HeaderCol6_1_4_1.ID = 29;
                HeaderCol6_1_4_1.ParentID = 22;
                HeaderCol6_1_4_1.Style = "";
                HeaderCol6_1_4.DataChild.Add(HeaderCol6_1_4_1);

                TableHeader HeaderCol6_1_4_2 = new TableHeader();
                HeaderCol6_1_4_2.Name = "Tiếp nhiều lần";
                HeaderCol6_1_4_2.ID = 30;
                HeaderCol6_1_4_2.ParentID = 22;
                HeaderCol6_1_4_2.Style = "";
                HeaderCol6_1_4.DataChild.Add(HeaderCol6_1_4_2);

                TableHeader HeaderCol6_1_5_1 = new TableHeader();
                HeaderCol6_1_5_1.Name = "Số đoàn được tiếp";
                HeaderCol6_1_5_1.ID = 31;
                HeaderCol6_1_5_1.ParentID = 23;
                HeaderCol6_1_5_1.Style = "";
                HeaderCol6_1_5.DataChild.Add(HeaderCol6_1_5_1);

                TableHeader HeaderCol6_1_5_2 = new TableHeader();
                HeaderCol6_1_5_2.Name = "Số người được tiếp";
                HeaderCol6_1_5_2.ID = 32;
                HeaderCol6_1_5_2.ParentID = 23;
                HeaderCol6_1_5_2.Style = "";
                HeaderCol6_1_5.DataChild.Add(HeaderCol6_1_5_2);

                TableHeader HeaderCol6_1_5_3 = new TableHeader();
                HeaderCol6_1_5_3.Name = "Tiếp lần đầu";
                HeaderCol6_1_5_3.ID = 33;
                HeaderCol6_1_5_3.ParentID = 23;
                HeaderCol6_1_5_3.Style = "";
                HeaderCol6_1_5.DataChild.Add(HeaderCol6_1_5_3);

                TableHeader HeaderCol6_1_5_4 = new TableHeader();
                HeaderCol6_1_5_4.Name = "Tiếp nhiều lần";
                HeaderCol6_1_5_4.ID = 34;
                HeaderCol6_1_5_4.ParentID = 23;
                HeaderCol6_1_5_4.Style = "";
                HeaderCol6_1_5.DataChild.Add(HeaderCol6_1_5_4);


                TableHeader HeaderCol6_2_4_1 = new TableHeader();
                HeaderCol6_2_4_1.Name = "Tiếp lần đầu";
                HeaderCol6_2_4_1.ID = 35;
                HeaderCol6_2_4_1.ParentID = 27;
                HeaderCol6_2_4_1.Style = "";
                HeaderCol6_2_4.DataChild.Add(HeaderCol6_2_4_1);

                TableHeader HeaderCol6_2_4_2 = new TableHeader();
                HeaderCol6_2_4_2.Name = "Tiếp nhiều lần";
                HeaderCol6_2_4_2.ID = 36;
                HeaderCol6_2_4_2.ParentID = 27;
                HeaderCol6_2_4_2.Style = "";
                HeaderCol6_2_4.DataChild.Add(HeaderCol6_2_4_2);

                TableHeader HeaderCol6_2_5_1 = new TableHeader();
                HeaderCol6_2_5_1.Name = "Số đoàn được tiếp";
                HeaderCol6_2_5_1.ID = 37;
                HeaderCol6_2_5_1.ParentID = 28;
                HeaderCol6_2_5_1.Style = "";
                HeaderCol6_2_5.DataChild.Add(HeaderCol6_2_5_1);

                TableHeader HeaderCol6_2_5_2 = new TableHeader();
                HeaderCol6_2_5_2.Name = "Số người được tiếp";
                HeaderCol6_2_5_2.ID = 38;
                HeaderCol6_2_5_2.ParentID = 28;
                HeaderCol6_2_5_2.Style = "";
                HeaderCol6_2_5.DataChild.Add(HeaderCol6_2_5_2);

                TableHeader HeaderCol6_2_5_3 = new TableHeader();
                HeaderCol6_2_5_3.Name = "Tiếp lần đầu";
                HeaderCol6_2_5_3.ID = 39;
                HeaderCol6_2_5_3.ParentID = 28;
                HeaderCol6_2_5_3.Style = "";
                HeaderCol6_2_5.DataChild.Add(HeaderCol6_2_5_3);

                TableHeader HeaderCol6_2_5_4 = new TableHeader();
                HeaderCol6_2_5_4.Name = "Tiếp nhiều lần";
                HeaderCol6_2_5_4.ID = 40;
                HeaderCol6_2_5_4.ParentID = 28;
                HeaderCol6_2_5_4.Style = "";
                HeaderCol6_2_5.DataChild.Add(HeaderCol6_2_5_4);
                #endregion
                #region TableData 
                TableData Row1 = new TableData();
                Row1.ID = 1;
                Row1.isClick = false;
                int tmp = 100000;
                var DataArr = new List<RowItem>();
                RowItem RowItem1 = new RowItem(1 + tmp, "Ms", "", "", null, "width: 215px", ref DataArr);
                RowItem RowItem2 = new RowItem(2 + tmp, "1=4+13+22","", "", null, "text-align: center;width: 250px", ref DataArr);
                RowItem RowItem3 = new RowItem(3 + tmp, "2=5+14+23", "", "", null, "text-align: center;width: 250px", ref DataArr);
                RowItem RowItem4 = new RowItem(4 + tmp, "3=6+7 +15+16 +24+25", "", "", null, "text-align: center;width: 250px", ref DataArr);
                RowItem RowItem5 = new RowItem(5 + tmp, "4","", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem6 = new RowItem(6 + tmp, "5","", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem7 = new RowItem(7 + tmp, "6","", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem8 = new RowItem(8 + tmp, "7","", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem9 = new RowItem(9 + tmp, "8", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem10 = new RowItem(10 + tmp, "9", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem11 = new RowItem(11 + tmp, "10","", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem12 = new RowItem(12 + tmp, "11","", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem13 = new RowItem(13 + tmp, "12","", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem14 = new RowItem(14 + tmp, "13","", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem15 = new RowItem(15 + tmp, "14","", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem16 = new RowItem(16 + tmp, "15","", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem17 = new RowItem(17 + tmp, "16","", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem18 = new RowItem(18 + tmp, "17","", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem19 = new RowItem(19 + tmp, "18","", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem20 = new RowItem(20 + tmp, "19","", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem21 = new RowItem(21 + tmp, "20","", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem22 = new RowItem(22 + tmp, "21","", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem23 = new RowItem(23 + tmp, "22","", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem24 = new RowItem(24 + tmp, "23","", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem25 = new RowItem(25 + tmp, "24","", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem26 = new RowItem(26 + tmp, "25","", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem27 = new RowItem(27 + tmp, "26","", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem28 = new RowItem(28 + tmp, "27","", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem29 = new RowItem(29 + tmp, "28","", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem30 = new RowItem(30 + tmp, "29", "", "", null, "text-align: center;width: 100px", ref DataArr);
                Row1.DataArr = DataArr;
                BaoCaoModel.DataTable.TableData.Add(Row1);
                
                List<TableData> data = new List<TableData>();
                data = new TCD01DAL().TCD01(p.ListCapID, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                BaoCaoModel.DataTable.TableData.AddRange(data);

                #endregion

                Result.Status = 1;
                Result.Data = BaoCaoModel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel TCD01_GetDSChiTietDonThu(string ContentRootPath, BaseReportParams p)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();
                     
                var listTableHeader = new List<TableHeader>();
                TableHeader HeaderCol1 = new TableHeader(1, 0, "STT", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Số đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Ngày tiếp nhận", "font-weight:bold", ref listTableHeader);   
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Chủ đơn", "font-weight:bold", ref listTableHeader); 
                TableHeader HeaderCol5 = new TableHeader(5, 0, "Địa chỉ", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol6 = new TableHeader(6, 0, "Nội dung đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol7 = new TableHeader(7, 0, "Loại đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol8 = new TableHeader(8, 0, "Kết quả xử lý", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol9 = new TableHeader(9, 0, "Kết quả giải quyết", "font-weight:bold", ref listTableHeader);
                BaoCaoModel.DataTable.TableHeader = listTableHeader;

                List<TKDonThuInfo> tKDonThuInfos = new List<TKDonThuInfo>();
                tKDonThuInfos = new TCD01DAL().TCD01_GetDSChiTietDonThu(ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now, p.Offset, p.PageSize, p.Index, p.XemTaiLieuMat, p.CanBoID, p.CoQuanID ?? 0, p.CapID);
                if(tKDonThuInfos.Count > 0)
                {
                    List<TableData> data = new List<TableData>();
                    int stt = p.Offset;
                    foreach (var item in tKDonThuInfos)
                    {
                        TableData tableData = new TableData();
                        tableData.ID = stt++;
                        tableData.DonThuID = item.DonThuID;
                        tableData.XuLyDonID = item.XuLyDonID;

                        var DataArr = new List<RowItem>();
                        RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "", ref DataArr);
                        RowItem RowItem2 = new RowItem(2, item.SoDon, "", "", null, "", ref DataArr);
                        RowItem RowItem3 = new RowItem(3, item.NgayNhapDonStr, "", "", null, "", ref DataArr);
                        RowItem RowItem4 = new RowItem(4, item.TenChuDon, "", "", null, "", ref DataArr);
                        RowItem RowItem5 = new RowItem(5, item.DiaChi, "", "", null, "", ref DataArr);
                        RowItem RowItem6 = new RowItem(6, item.NoiDungDon, "", "", null, "", ref DataArr);
                        RowItem RowItem7 = new RowItem(7, item.TenLoaiKhieuTo, "", "", null, "", ref DataArr);
                        RowItem RowItem8 = new RowItem(8, item.TenHuongGiaiQuyet, "", "", null, "", ref DataArr);
                        RowItem RowItem9 = new RowItem(9, item.KetQuaID_Str, "", "", null, "", ref DataArr);
                      
                        tableData.DataArr = DataArr;
                        data.Add(tableData);
                    }
                    BaoCaoModel.DataTable.TableData = data;
                }
                
                Result.Status = 1;
                Result.Data = BaoCaoModel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            
            return Result;
        }

        public BaseResultModel TCD01_GetDSChiTietDonThu_XuatExcel(BaseReportParams p, string ContentRootPath, int CanBoDangNhapID)
        {
            var Result = new BaseResultModel();
            try
            {
                List<TableData> data = new List<TableData>();
                List<TKDonThuInfo> tKDonThuInfos = new TCD01DAL().TCD01_GetDSChiTietDonThu(ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now, p.Offset, p.PageSize, p.Index, p.XemTaiLieuMat, p.CanBoID, p.CoQuanID ?? 0, p.CapID);
                if (tKDonThuInfos.Count > 0)
                {
                    int stt = p.Offset;
                    foreach (var item in tKDonThuInfos)
                    {
                        TableData tableData = new TableData();
                        tableData.ID = stt++;
                        tableData.DonThuID = item.DonThuID;
                        tableData.XuLyDonID = item.XuLyDonID;

                        var DataArr = new List<RowItem>();
                        RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "", ref DataArr);
                        RowItem RowItem2 = new RowItem(2, item.SoDon, "", "", null, "", ref DataArr);
                        RowItem RowItem3 = new RowItem(3, item.NgayNhapDonStr, "", "", null, "", ref DataArr);
                        RowItem RowItem4 = new RowItem(4, item.TenChuDon, "", "", null, "", ref DataArr);
                        RowItem RowItem5 = new RowItem(5, item.DiaChi, "", "", null, "", ref DataArr);
                        RowItem RowItem6 = new RowItem(6, item.NoiDungDon, "", "", null, "", ref DataArr);
                        RowItem RowItem7 = new RowItem(7, item.TenLoaiKhieuTo, "", "", null, "", ref DataArr);
                        RowItem RowItem8 = new RowItem(8, item.TenHuongGiaiQuyet, "", "", null, "", ref DataArr);
                        RowItem RowItem9 = new RowItem(9, item.KetQuaID_Str, "", "", null, "", ref DataArr);

                        tableData.DataArr = DataArr;
                        data.Add(tableData);
                    }    
                }
                string path = @"Templates\FileTam\TCD01_DSChiTietDonThu" + CanBoDangNhapID + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";
                string urlExcel = new TCD01DAL().DSChiTietDonThu_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                Result.Status = 1;
                Result.Data = urlExcel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel TCD01_XuatExcel(BaseReportParams p, string ContentRootPath, int RoleID, int CapID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, int HuyenDangNhapID)
        {
            var Result = new BaseResultModel();
            try
            {
                List<TableData> data = new List<TableData>();
                data = new TCD01DAL().TCD01(p.ListCapID, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                #region Thêm hàng Tổng
                TableData rowToanTinh = new TableData();
                TableData rowToanHuyen = new TableData();
                TableData rowTong = new TableData();
                if (data.Count > 1)
                {
                    foreach (var item in data)
                    {
                        if(item.DataArr != null && item.DataArr.Count > 0)
                        {
                            foreach (var col in item.DataArr)
                            {
                                if(col.CapID == CapCoQuanViewChiTiet.ToanTinh.GetHashCode().ToString())
                                {
                                    rowToanTinh = Utils.DeepCopy(item);
                                    break;
                                }
                                else if (col.CapID == CapCoQuanViewChiTiet.ToanHuyen.GetHashCode().ToString())
                                {
                                    rowToanHuyen = Utils.DeepCopy(item);
                                    break;
                                }
                            }
                        }
                    }
                }
                else if(data.Count == 1)
                {
                    rowTong = Utils.DeepCopy(data[0]);
                }

                if(rowToanTinh.DataArr != null && rowToanTinh.DataArr.Count > 0)
                {
                    rowTong.DataArr = rowToanTinh.DataArr;
                }
                else if (rowToanHuyen.DataArr != null && rowToanHuyen.DataArr.Count > 0)
                {
                    rowTong.DataArr = rowToanHuyen.DataArr;
                }
                if(rowTong.DataArr != null)
                {
                    rowTong.DataArr[0].Content = "Tổng";
                    data.Add(rowTong);
                }
                #endregion
                string path = @"Templates\FileTam\TCD01_" + CanBoDangNhapID + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";
                string urlExcel = new TCD01DAL().TCD01_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                Result.Status = 1;
                Result.Data = urlExcel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel TTR01(BaseReportParams p)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.BieuSo = "Biểu số: 01/TTr";
                BaoCaoModel.ThongTinSoLieu = "Số liệu tính từ ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " đến ngày " + DateTime.Now.ToString("dd/MM/yyyy");
                BaoCaoModel.TuNgay = DateTime.Now.ToString("dd/MM/yyyy");
                BaoCaoModel.DenNgay = DateTime.Now.ToString("dd/MM/yyyy");
                BaoCaoModel.Title = "TỔNG HỢP KẾT QUẢ THANH TRA HÀNH CHÍNH";
                BaoCaoModel.DonViTinh = "Đơn vị tính: Tiền (triệu đồng); đất (<span>m<sup>2</sup></span>)";
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();
                #region Header           
                var listTableHeader = new List<TableHeader>();
                //col1
                TableHeader HeaderCol1 = new TableHeader(1, 0, "Đơn vị", "", ref listTableHeader);
                //col2
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Số cuộc thanh tra thực hiện trong kỳ", "", ref listTableHeader);
                var DataChild2 = new List<TableHeader>();
                TableHeader HeaderCol2_1 = new TableHeader(3, 2, "Số cuộc thanh tra thực hiện trong kỳ", "", ref DataChild2);
                TableHeader HeaderCol2_2 = new TableHeader(4, 2, "Phân loại", "", ref DataChild2);
                HeaderCol2.DataChild = DataChild2;
                var DataChild2_2 = new List<TableHeader>();
                TableHeader HeaderCol2_2_1 = new TableHeader(5, 4, "Triển khai từ kỳ trước chuyển sang", "", ref DataChild2_2);
                TableHeader HeaderCol2_2_2 = new TableHeader(6, 4, "Triển khai trong kỳ", "", ref DataChild2_2);
                TableHeader HeaderCol2_2_3 = new TableHeader(7, 4, "Theo Kế hoạch", "", ref DataChild2_2);
                TableHeader HeaderCol2_2_4 = new TableHeader(8, 4, "Đột xuất", "", ref DataChild2_2);
                HeaderCol2_2.DataChild = DataChild2_2;
                //col3
                TableHeader HeaderCol3 = new TableHeader(9, 0, "Đã ban hành kết luận", "", ref listTableHeader);
                //col4
                TableHeader HeaderCol4 = new TableHeader(10, 0, "Số đơn vị được thanh tra", "", ref listTableHeader);
                //col5
                TableHeader HeaderCol5 = new TableHeader(11, 0, "Tổng vi phạm về kinh tế", "", ref listTableHeader);
                var DataChild5 = new List<TableHeader>();
                TableHeader HeaderCol5_1 = new TableHeader(12, 11, "Tiền và tài sản quy thành tiền", "", ref DataChild5);
                TableHeader HeaderCol5_2 = new TableHeader(13, 11, "Đất (<span>m<sup>2</sup></span>)", "", ref DataChild5);
                HeaderCol5.DataChild = DataChild5;
                //col6
                TableHeader HeaderCol6 = new TableHeader(14, 0, "Kiến nghị xử lý", "", ref listTableHeader);
                var DataChild6 = new List<TableHeader>();
                TableHeader HeaderCol6_1 = new TableHeader(15, 14, "Thu hồi về NSNN", "", ref DataChild6);
                TableHeader HeaderCol6_2 = new TableHeader(16, 14, "Xử lý khác về kinh tế", "", ref DataChild6);
                TableHeader HeaderCol6_3 = new TableHeader(17, 14, "Hành chính", "", ref DataChild6);
                TableHeader HeaderCol6_4 = new TableHeader(18, 14, "Chuyển cơ quan điều tra", "", ref DataChild6);
                TableHeader HeaderCol6_5 = new TableHeader(19, 14, "Hoàn thiện cơ chế, chính sách (số văn bản)", "", ref DataChild6);               
                HeaderCol6.DataChild = DataChild6;
                var DataChild6_1 = new List<TableHeader>();
                TableHeader HeaderCol6_1_1 = new TableHeader(20, 15, "Tiền (Tr.đ)", "", ref DataChild6_1);
                TableHeader HeaderCol6_1_2 = new TableHeader(21, 15, "Đất (<span>m<sup>2</sup></span>)", "", ref DataChild6_1);
                HeaderCol6_1.DataChild = DataChild6_1;
                var DataChild6_2 = new List<TableHeader>();
                TableHeader HeaderCol6_2_1 = new TableHeader(22, 16, "Tiền (Tr.đ)", "", ref DataChild6_2);
                TableHeader HeaderCol6_2_2 = new TableHeader(23, 16, "Đất (<span>m<sup>2</sup></span>)", "", ref DataChild6_2);
                HeaderCol6_2.DataChild = DataChild6_2;
                var DataChild6_3 = new List<TableHeader>();
                TableHeader HeaderCol6_3_1 = new TableHeader(22, 16, "Tổ chức", "", ref DataChild6_3);
                TableHeader HeaderCol6_3_2 = new TableHeader(23, 16, "Cá nhân", "", ref DataChild6_3);
                HeaderCol6_3.DataChild = DataChild6_3;
                var DataChild6_4 = new List<TableHeader>();
                TableHeader HeaderCol6_4_1 = new TableHeader(22, 16, "Vụ", "", ref DataChild6_4);
                TableHeader HeaderCol6_4_2 = new TableHeader(23, 16, "Đối tượng", "", ref DataChild6_4);
                HeaderCol6_4.DataChild = DataChild6_4;
                BaoCaoModel.DataTable.TableHeader = listTableHeader;
                #endregion
                #region TableData              
                TableData Row1 = new TableData();
                Row1.ID = 1;
                var DataArr = new List<RowItem>();
                //RowItem RowItem1 = new RowItem(1, "Ms", null, "", ref DataArr);
                //RowItem RowItem2 = new RowItem(2, "1=2+3=4+5", null, "", ref DataArr);
                //RowItem RowItem3 = new RowItem(3, "2", null, "", ref DataArr);
                //RowItem RowItem4 = new RowItem(4, "3", null, "", ref DataArr);
                //RowItem RowItem5 = new RowItem(5, "4", null, "", ref DataArr);
                //RowItem RowItem6 = new RowItem(6, "5", null, "", ref DataArr);
                //RowItem RowItem7 = new RowItem(7, "6", null, "", ref DataArr);
                //RowItem RowItem8 = new RowItem(8, "7", null, "", ref DataArr);
                //RowItem RowItem9 = new RowItem(9, "8=10+12", null, "", ref DataArr);
                //RowItem RowItem10 = new RowItem(10, "9=11+13", null, "", ref DataArr);
                //RowItem RowItem11 = new RowItem(11, "10", null, "", ref DataArr);
                //RowItem RowItem12 = new RowItem(12, "11", null, "", ref DataArr);
                //RowItem RowItem13 = new RowItem(13, "12", null, "", ref DataArr);
                //RowItem RowItem14 = new RowItem(14, "13", null, "", ref DataArr);
                //RowItem RowItem15 = new RowItem(15, "14", null, "", ref DataArr);
                //RowItem RowItem16 = new RowItem(16, "15", null, "", ref DataArr);
                //RowItem RowItem17 = new RowItem(17, "16", null, "", ref DataArr);
                //RowItem RowItem18 = new RowItem(18, "17", null, "", ref DataArr);
                //RowItem RowItem19 = new RowItem(19, "18", null, "", ref DataArr);
                Row1.DataArr = DataArr;
                BaoCaoModel.DataTable.TableData.Add(Row1);

                for (int i = 2; i < 60; i++)
                {
                    TableData TableData = new TableData();
                    TableData.ID = i;
                    TableData.DataArr = new List<RowItem>();

                    for (int j = 0; j < 19; j++)
                    {
                        RowItem RowItem = new RowItem();
                        RowItem.ID = j;
                        if (j == 0)
                        {
                            RowItem.Content = "Cơ quan " + i.ToString();
                        }
                        else
                        {
                            RowItem.Content = j.ToString();
                        }
                        TableData.DataArr.Add(RowItem);
                    }
                    TableData.DataChild = new List<TableData>();
                    TableData DataChild = new TableData();
                    DataChild.ID = i + 100;
                    DataChild.ParentID = i;
                    DataChild.DataArr = new List<RowItem>();

                    for (int j = 0; j < 19; j++)
                    {
                        RowItem RowItem = new RowItem();
                        RowItem.ID = j;
                        if (j == 0)
                        {
                            RowItem.Content = "Cơ quan " + i + "." + j;
                        }
                        else
                        {
                            RowItem.Content = j.ToString();
                        }
                        DataChild.DataArr.Add(RowItem);
                    }
                    TableData.DataChild.Add(DataChild);


                    BaoCaoModel.DataTable.TableData.Add(TableData);
                }
                #endregion

                Result.Status = 1;
                Result.Data = BaoCaoModel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel TCD02(BaseReportParams p, string ContentRootPath, int RoleID, int CapID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, int HuyenDangNhapID)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.BieuSo = "Biểu số: 02/TCD";
                BaoCaoModel.ThongTinSoLieu = "Số liệu tính từ ngày " + (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy") + " đến ngày " + (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.TuNgay = (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.DenNgay = (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.Title = "TỔNG HỢP KẾT QUẢ PHÂN LOẠI, XỬ LÝ ĐƠN QUA TIẾP CÔNG DÂN";
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();
                #region Header
                var listTableHeader = new List<TableHeader>();
                //Cấp 2
                TableHeader HeaderCol1 = new TableHeader(1, 0, "Đơn vị", "width: 215px", ref listTableHeader);
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Tổng số đơn nhận được qua tiếp công dân", "", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Tổng số vụ việc được tiếp", "", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Phân loại theo nội dung", "", ref listTableHeader);
                TableHeader HeaderCol5 = new TableHeader(5, 0, "Phân loại theo thẩm quyền", "", ref listTableHeader);
                TableHeader HeaderCol6 = new TableHeader(6, 0, "Số văn bản phúc đáp nhận được do chuyển đơn", "", ref listTableHeader);
                TableHeader HeaderCol7 = new TableHeader(7, 0, "Ghi chú", "", ref listTableHeader);
                //Cấp 2
                var DataChild4 = new List<TableHeader>();
                TableHeader HeaderCol41 = new TableHeader(41, 4, "Khiếu nại", "", ref DataChild4);
                TableHeader HeaderCol42 = new TableHeader(42, 4, "Tố cáo", "", ref DataChild4);
                TableHeader HeaderCol43 = new TableHeader(43, 4, "Phản ánh, kiến nghị", "", ref DataChild4);
                HeaderCol4.DataChild = DataChild4;

                var DataChild5 = new List<TableHeader>();
                TableHeader HeaderCol51 = new TableHeader(51, 5, "Thuộc thẩm quyền", "", ref DataChild5);
                TableHeader HeaderCol52 = new TableHeader(52, 5, "Không thuộc thẩm quyền", "", ref DataChild5);
                HeaderCol5.DataChild = DataChild5;
                //Cấp 3
                var DataChild41 = new List<TableHeader>();
                TableHeader HeaderCol411 = new TableHeader(411, 41, "Số đơn", "", ref DataChild41);
                TableHeader HeaderCol412 = new TableHeader(412, 41, "Số vụ việc", "", ref DataChild41);
                HeaderCol41.DataChild = DataChild41;

                var DataChild42 = new List<TableHeader>();
                TableHeader HeaderCol421 = new TableHeader(421, 42, "Số đơn", "", ref DataChild42);
                TableHeader HeaderCol422 = new TableHeader(422, 42, "Số vụ việc", "", ref DataChild42);
                HeaderCol42.DataChild = DataChild42;

                var DataChild43 = new List<TableHeader>();
                TableHeader HeaderCol431 = new TableHeader(431, 43, "Số đơn", "", ref DataChild43);
                TableHeader HeaderCol432 = new TableHeader(432, 43, "Số vụ việc", "", ref DataChild43);
                HeaderCol43.DataChild = DataChild43;

                var DataChild51 = new List<TableHeader>();
                TableHeader HeaderCol511 = new TableHeader(511, 51, "Số đơn", "", ref DataChild51);
                TableHeader HeaderCol512 = new TableHeader(512, 51, "Số vụ việc", "", ref DataChild51);
                HeaderCol51.DataChild = DataChild51;

                var DataChild52 = new List<TableHeader>();
                TableHeader HeaderCol521 = new TableHeader(521, 52, "Số đơn", "", ref DataChild52);
                TableHeader HeaderCol522 = new TableHeader(522, 52, "Số vụ việc", "", ref DataChild52);
                HeaderCol52.DataChild = DataChild52;
                //Cấp 4
                var DataChild522 = new List<TableHeader>();
                TableHeader HeaderCol5221 = new TableHeader(5221, 522, "Tổng", "", ref DataChild522);
                TableHeader HeaderCol5222 = new TableHeader(5222, 522, "Hướng dẫn", "", ref DataChild522);
                TableHeader HeaderCol5223 = new TableHeader(5223, 522, "Chuyển đơn", "", ref DataChild522);
                TableHeader HeaderCol5224 = new TableHeader(5224, 522, "Đôn đốc giải quyết", "", ref DataChild522);
                HeaderCol522.DataChild = DataChild522;

                BaoCaoModel.DataTable.TableHeader = listTableHeader;
                #endregion
                #region TableData 
                TableData Row1 = new TableData();
                Row1.ID = 1;
                Row1.isClick = false;
                var DataArr = new List<RowItem>();
                RowItem RowItem1 = new RowItem(1, "Ms", "", "", null, "width: 215px", ref DataArr);
                RowItem RowItem2 = new RowItem(2, "1=3+5+7=9+11", "", "", null, "text-align: center;width: 250px", ref DataArr);
                RowItem RowItem3 = new RowItem(3, "2=4+6+8=10+12", "", "", null, "text-align: center;width: 250px", ref DataArr);
                RowItem RowItem4 = new RowItem(4, "3", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem5 = new RowItem(5, "4", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem6 = new RowItem(6, "5", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem7 = new RowItem(7, "6", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem8 = new RowItem(8, "7", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem9 = new RowItem(9, "8", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem10 = new RowItem(10, "9", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem11 = new RowItem(11, "10", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem12 = new RowItem(12, "11", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem13 = new RowItem(13, "12", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem14 = new RowItem(14, "13", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem15 = new RowItem(15, "14", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem16 = new RowItem(16, "15", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem17 = new RowItem(17, "16", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem18 = new RowItem(18, "17", "", "", null, "text-align: center;width: 100px", ref DataArr);
                
                Row1.DataArr = DataArr;
                BaoCaoModel.DataTable.TableData.Add(Row1);

                List<TableData> data = new List<TableData>();
                data = new TCD02DAL().TCD02(p.ListCapID, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                BaoCaoModel.DataTable.TableData.AddRange(data);

                #endregion

                Result.Status = 1;
                Result.Data = BaoCaoModel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel TCD02_XuatExcel(BaseReportParams p, string ContentRootPath, int RoleID, int CapID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, int HuyenDangNhapID)
        {
            var Result = new BaseResultModel();
            try
            {
                List<TableData> data = new List<TableData>();
                data = new TCD02DAL().TCD02(p.ListCapID, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                #region Thêm hàng Tổng
                TableData rowToanTinh = new TableData();
                TableData rowToanHuyen = new TableData();
                TableData rowTong = new TableData();
                if (data.Count > 1)
                {
                    foreach (var item in data)
                    {
                        if (item.DataArr != null && item.DataArr.Count > 0)
                        {
                            foreach (var col in item.DataArr)
                            {
                                if (col.CapID == CapCoQuanViewChiTiet.ToanTinh.GetHashCode().ToString())
                                {
                                    rowToanTinh = Utils.DeepCopy(item);
                                    break;
                                }
                                else if (col.CapID == CapCoQuanViewChiTiet.ToanHuyen.GetHashCode().ToString())
                                {
                                    rowToanHuyen = Utils.DeepCopy(item);
                                    break;
                                }
                            }
                        }
                    }
                }
                else if (data.Count == 1)
                {
                    rowTong = Utils.DeepCopy(data[0]);
                }

                if (rowToanTinh.DataArr != null && rowToanTinh.DataArr.Count > 0)
                {
                    rowTong.DataArr = rowToanTinh.DataArr;
                }
                else if (rowToanHuyen.DataArr != null && rowToanHuyen.DataArr.Count > 0)
                {
                    rowTong.DataArr = rowToanHuyen.DataArr;
                }
                if (rowTong.DataArr != null)
                {
                    rowTong.DataArr[0].Content = "Tổng";
                    data.Add(rowTong);
                }
                #endregion
                string path = @"Templates\FileTam\TCD02_" + CanBoDangNhapID + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";
                string urlExcel = new TCD02DAL().TCD02_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                Result.Status = 1;
                Result.Data = urlExcel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel TCD02_GetDSChiTietDonThu(string ContentRootPath, BaseReportParams p)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();

                var listTableHeader = new List<TableHeader>();
                TableHeader HeaderCol1 = new TableHeader(1, 0, "STT", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Số đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Ngày tiếp nhận", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Chủ đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol5 = new TableHeader(5, 0, "Địa chỉ", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol6 = new TableHeader(6, 0, "Nội dung đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol7 = new TableHeader(7, 0, "Loại đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol8 = new TableHeader(8, 0, "Kết quả xử lý", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol9 = new TableHeader(9, 0, "Kết quả giải quyết", "font-weight:bold", ref listTableHeader);
                BaoCaoModel.DataTable.TableHeader = listTableHeader;

                List<TKDonThuInfo> tKDonThuInfos = new List<TKDonThuInfo>();
                tKDonThuInfos = new TCD02DAL().TCD02_GetDSChiTietDonThu(ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now, p.Offset, p.PageSize, p.Index, p.XemTaiLieuMat, p.CanBoID, p.CoQuanID ?? 0, p.CapID);
                if (tKDonThuInfos.Count > 0)
                {
                    List<TableData> data = new List<TableData>();
                    int stt = p.Offset;
                    foreach (var item in tKDonThuInfos)
                    {
                        TableData tableData = new TableData();
                        tableData.ID = stt++;
                        tableData.DonThuID = item.DonThuID;
                        tableData.XuLyDonID = item.XuLyDonID;

                        var DataArr = new List<RowItem>();
                        RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "", ref DataArr);
                        RowItem RowItem2 = new RowItem(2, item.SoDon, "", "", null, "", ref DataArr);
                        RowItem RowItem3 = new RowItem(3, item.NgayNhapDonStr, "", "", null, "", ref DataArr);
                        RowItem RowItem4 = new RowItem(4, item.TenChuDon, "", "", null, "", ref DataArr);
                        RowItem RowItem5 = new RowItem(5, item.DiaChi, "", "", null, "", ref DataArr);
                        RowItem RowItem6 = new RowItem(6, item.NoiDungDon, "", "", null, "", ref DataArr);
                        RowItem RowItem7 = new RowItem(7, item.TenLoaiKhieuTo, "", "", null, "", ref DataArr);
                        RowItem RowItem8 = new RowItem(8, item.TenHuongGiaiQuyet, "", "", null, "", ref DataArr);
                        RowItem RowItem9 = new RowItem(9, item.KetQuaID_Str, "", "", null, "", ref DataArr);

                        tableData.DataArr = DataArr;
                        data.Add(tableData);
                    }
                    BaoCaoModel.DataTable.TableData = data;
                }

                Result.Status = 1;
                Result.Data = BaoCaoModel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel TCD02_GetDSChiTietDonThu_XuatExcel(BaseReportParams p, string ContentRootPath, int CanBoDangNhapID)
        {
            var Result = new BaseResultModel();
            try
            {
                List<TableData> data = new List<TableData>();
                List<TKDonThuInfo> tKDonThuInfos = new TCD02DAL().TCD02_GetDSChiTietDonThu(ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now, p.Offset, p.PageSize, p.Index, p.XemTaiLieuMat, p.CanBoID, p.CoQuanID ?? 0, p.CapID);
                if (tKDonThuInfos.Count > 0)
                {
                    int stt = p.Offset;
                    foreach (var item in tKDonThuInfos)
                    {
                        TableData tableData = new TableData();
                        tableData.ID = stt++;
                        tableData.DonThuID = item.DonThuID;
                        tableData.XuLyDonID = item.XuLyDonID;

                        var DataArr = new List<RowItem>();
                        RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "", ref DataArr);
                        RowItem RowItem2 = new RowItem(2, item.SoDon, "", "", null, "", ref DataArr);
                        RowItem RowItem3 = new RowItem(3, item.NgayNhapDonStr, "", "", null, "", ref DataArr);
                        RowItem RowItem4 = new RowItem(4, item.TenChuDon, "", "", null, "", ref DataArr);
                        RowItem RowItem5 = new RowItem(5, item.DiaChi, "", "", null, "", ref DataArr);
                        RowItem RowItem6 = new RowItem(6, item.NoiDungDon, "", "", null, "", ref DataArr);
                        RowItem RowItem7 = new RowItem(7, item.TenLoaiKhieuTo, "", "", null, "", ref DataArr);
                        RowItem RowItem8 = new RowItem(8, item.TenHuongGiaiQuyet, "", "", null, "", ref DataArr);
                        RowItem RowItem9 = new RowItem(9, item.KetQuaID_Str, "", "", null, "", ref DataArr);

                        tableData.DataArr = DataArr;
                        data.Add(tableData);
                    }
                }
                string path = @"Templates\FileTam\TCD02_DSChiTietDonThu" + CanBoDangNhapID + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";
                string urlExcel = new TCD01DAL().DSChiTietDonThu_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                Result.Status = 1;
                Result.Data = urlExcel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel XLD01(BaseReportParams p, string ContentRootPath, int RoleID, int CapID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, int HuyenDangNhapID)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.BieuSo = "Biểu số: 01/XLD";
                BaoCaoModel.ThongTinSoLieu = "Số liệu tính từ ngày " + (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy") + " đến ngày " + (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.TuNgay = (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.DenNgay = (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.Title = "TỔNG HỢP KẾT QUẢ XỬ LÝ ĐƠN";
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();
                #region Header
                var listTableHeader = new List<TableHeader>();
                //Cấp 2
                TableHeader HeaderCol1 = new TableHeader(1, 0, "Đơn vị", "width: 215px", ref listTableHeader);
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Tổng số đơn", "", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Kỳ trước chuyển sang", "", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Tiếp nhận trong kỳ", "", ref listTableHeader);
                TableHeader HeaderCol5 = new TableHeader(5, 0, "Số đơn đã xử lý", "", ref listTableHeader);
                TableHeader HeaderCol6 = new TableHeader(6, 0, "Đủ điều kiện xử lý", "", ref listTableHeader);
                TableHeader HeaderCol7 = new TableHeader(7, 0, "Phân loại đơn theo nội dung", "", ref listTableHeader);
                TableHeader HeaderCol8 = new TableHeader(8, 0, "Phân loại đơn theo tình trạng giải quyết", "", ref listTableHeader);
                TableHeader HeaderCol9 = new TableHeader(9, 0, "Kết quả xử lý đơn", "", ref listTableHeader);
                TableHeader HeaderCol10 = new TableHeader(10, 0, "Số văn bản phúc đáp nhận được do chuyển đơn", "", ref listTableHeader);
                //Cấp 2
                var DataChild3 = new List<TableHeader>();
                TableHeader HeaderCol31 = new TableHeader(31, 3, "Đơn có nhiều người đứng tên", "", ref DataChild3);
                TableHeader HeaderCol32 = new TableHeader(32, 3, "Đơn một người đứng tên", "", ref DataChild3);
                TableHeader HeaderCol33 = new TableHeader(33, 3, "Đơn khác", "", ref DataChild3);
                HeaderCol3.DataChild = DataChild3;

                var DataChild4 = new List<TableHeader>();
                TableHeader HeaderCol41 = new TableHeader(41, 4, "Đơn có nhiều người đứng tên", "", ref DataChild4);
                TableHeader HeaderCol42 = new TableHeader(42, 4, "Đơn một người đứng tên", "", ref DataChild4);
                TableHeader HeaderCol43 = new TableHeader(43, 4, "Đơn khác", "", ref DataChild4);
                HeaderCol4.DataChild = DataChild4;

                var DataChild6 = new List<TableHeader>();
                TableHeader HeaderCol61 = new TableHeader(61, 6, "Số đơn", "", ref DataChild6);
                TableHeader HeaderCol62 = new TableHeader(62, 6, "Số vụ việc", "", ref DataChild6);
                HeaderCol6.DataChild = DataChild6;

                var DataChild7 = new List<TableHeader>();
                TableHeader HeaderCol71 = new TableHeader(71, 7, "Khiếu nại", "", ref DataChild7);
                TableHeader HeaderCol72 = new TableHeader(72, 7, "Tố cáo", "", ref DataChild7);
                TableHeader HeaderCol73 = new TableHeader(73, 7, "Kiến nghị, phản ánh", "", ref DataChild7);
                HeaderCol7.DataChild = DataChild7;

                var DataChild8 = new List<TableHeader>();
                TableHeader HeaderCol81 = new TableHeader(81, 8, "Đã giải quyết", "", ref DataChild8);
                TableHeader HeaderCol82 = new TableHeader(82, 8, "Đang giải quyết", "", ref DataChild8);
                TableHeader HeaderCol83 = new TableHeader(83, 8, "Chưa giải quyết", "", ref DataChild8);
                HeaderCol8.DataChild = DataChild8;

                var DataChild9 = new List<TableHeader>();
                TableHeader HeaderCol91 = new TableHeader(91, 9, "Đơn thuộc thẩm quyền", "", ref DataChild9);
                TableHeader HeaderCol92 = new TableHeader(92, 9, "Đơn không thuộc thẩm quyền", "", ref DataChild9);
                HeaderCol9.DataChild = DataChild9;
                //Cấp 3
                var DataChild81 = new List<TableHeader>();
                TableHeader HeaderCol811 = new TableHeader(811, 81, "Lần đầu", "", ref DataChild81);
                TableHeader HeaderCol812 = new TableHeader(812, 81, "Nhiều lần", "", ref DataChild81);
                HeaderCol81.DataChild = DataChild81;

                var DataChild91 = new List<TableHeader>();
                TableHeader HeaderCol911 = new TableHeader(911, 91, "Tổng số", "", ref DataChild91);
                TableHeader HeaderCol912 = new TableHeader(912, 91, "Khiếu nại", "", ref DataChild91);
                TableHeader HeaderCol913 = new TableHeader(913, 91, "Tố cáo", "", ref DataChild91);
                TableHeader HeaderCol914 = new TableHeader(914, 91, "Kiến nghị, phản ánh", "", ref DataChild91);
                HeaderCol91.DataChild = DataChild91;

                var DataChild92 = new List<TableHeader>();
                TableHeader HeaderCol921 = new TableHeader(921, 92, "Tổng số", "", ref DataChild92);
                TableHeader HeaderCol922 = new TableHeader(922, 92, "Hướng dẫn", "", ref DataChild92);
                TableHeader HeaderCol923 = new TableHeader(923, 92, "Chuyển đơn", "", ref DataChild92);
                TableHeader HeaderCol924 = new TableHeader(924, 92, "Đôn đốc giải quyết", "", ref DataChild92);
                HeaderCol92.DataChild = DataChild92;

                BaoCaoModel.DataTable.TableHeader = listTableHeader;
                #endregion
                #region TableData 
                TableData Row1 = new TableData();
                Row1.ID = 1;
                Row1.isClick = false;
                var DataArr = new List<RowItem>();
                RowItem RowItem1 = new RowItem(1, "Ms", "", "", null, "width: 215px", ref DataArr);
                RowItem RowItem2 = new RowItem(2, "1=2+3+ ...+7", "", "", null, "text-align: center;width: 250px", ref DataArr);
                RowItem RowItem3 = new RowItem(3, "2", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem4 = new RowItem(4, "3", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem5 = new RowItem(5, "4", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem6 = new RowItem(6, "5", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem7 = new RowItem(7, "6", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem8 = new RowItem(8, "7", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem9 = new RowItem(9, "8", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem10 = new RowItem(10, "9=11+12+13 =14+15+16+17 =18+22", "", "", null, "text-align: center;width: 250px", ref DataArr);
                RowItem RowItem11 = new RowItem(11, "10", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem12 = new RowItem(12, "11", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem13 = new RowItem(13, "12", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem14 = new RowItem(14, "13", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem15 = new RowItem(15, "14", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem16 = new RowItem(16, "15", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem17 = new RowItem(17, "16", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem18 = new RowItem(18, "17", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem19 = new RowItem(19, "18 = 19+20+21", "", "", null, "text-align: center;width: 250px", ref DataArr);
                RowItem RowItem20 = new RowItem(20, "19", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem21 = new RowItem(21, "20", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem22 = new RowItem(22, "21", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem23 = new RowItem(23, "22 = 23+24+25", "", "", null, "text-align: center;width: 250px", ref DataArr);
                RowItem RowItem24 = new RowItem(24, "23", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem25 = new RowItem(25, "24", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem26 = new RowItem(26, "25", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem27 = new RowItem(27, "26", "", "", null, "text-align: center;width: 100px", ref DataArr);

                Row1.DataArr = DataArr;
                BaoCaoModel.DataTable.TableData.Add(Row1);

                List<TableData> data = new List<TableData>();
                data = new XLD01DAL().XLD01(p.ListCapID, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                BaoCaoModel.DataTable.TableData.AddRange(data);

                #endregion

                Result.Status = 1;
                Result.Data = BaoCaoModel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel XLD01_XuatExcel(BaseReportParams p, string ContentRootPath, int RoleID, int CapID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, int HuyenDangNhapID)
        {
            var Result = new BaseResultModel();
            try
            {
                List<TableData> data = new List<TableData>();
                data = new XLD01DAL().XLD01(p.ListCapID, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                #region Thêm hàng Tổng
                TableData rowToanTinh = new TableData();
                TableData rowToanHuyen = new TableData();
                TableData rowTong = new TableData();
                if (data.Count > 1)
                {
                    foreach (var item in data)
                    {
                        if (item.DataArr != null && item.DataArr.Count > 0)
                        {
                            foreach (var col in item.DataArr)
                            {
                                if (col.CapID == CapCoQuanViewChiTiet.ToanTinh.GetHashCode().ToString())
                                {
                                    rowToanTinh = Utils.DeepCopy(item);
                                    break;
                                }
                                else if (col.CapID == CapCoQuanViewChiTiet.ToanHuyen.GetHashCode().ToString())
                                {
                                    rowToanHuyen = Utils.DeepCopy(item);
                                    break;
                                }
                            }
                        }
                    }
                }
                else if (data.Count == 1)
                {
                    rowTong = Utils.DeepCopy(data[0]);
                }

                if (rowToanTinh.DataArr != null && rowToanTinh.DataArr.Count > 0)
                {
                    rowTong.DataArr = rowToanTinh.DataArr;
                }
                else if (rowToanHuyen.DataArr != null && rowToanHuyen.DataArr.Count > 0)
                {
                    rowTong.DataArr = rowToanHuyen.DataArr;
                }
                if (rowTong.DataArr != null)
                {
                    rowTong.DataArr[0].Content = "Tổng";
                    data.Add(rowTong);
                }
                #endregion
                string path = @"Templates\FileTam\XLD01_" + CanBoDangNhapID + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";
                string urlExcel = new XLD01DAL().XLD01_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                Result.Status = 1;
                Result.Data = urlExcel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel XLD01_GetDSChiTietDonThu(string ContentRootPath, BaseReportParams p)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();

                var listTableHeader = new List<TableHeader>();
                TableHeader HeaderCol1 = new TableHeader(1, 0, "STT", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Số đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Ngày tiếp nhận", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Chủ đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol5 = new TableHeader(5, 0, "Địa chỉ", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol6 = new TableHeader(6, 0, "Nội dung đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol7 = new TableHeader(7, 0, "Loại đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol8 = new TableHeader(8, 0, "Kết quả xử lý", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol9 = new TableHeader(9, 0, "Kết quả giải quyết", "font-weight:bold", ref listTableHeader);
                BaoCaoModel.DataTable.TableHeader = listTableHeader;

                List<TKDonThuInfo> tKDonThuInfos = new List<TKDonThuInfo>();
                tKDonThuInfos = new XLD01DAL().XLD01_GetDSChiTietDonThu(ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now, p.Offset, p.PageSize, p.Index, p.XemTaiLieuMat, p.CanBoID, p.CoQuanID ?? 0, p.CapID);
                if (tKDonThuInfos.Count > 0)
                {
                    List<TableData> data = new List<TableData>();
                    int stt = p.Offset;
                    foreach (var item in tKDonThuInfos)
                    {
                        TableData tableData = new TableData();
                        tableData.ID = stt++;
                        tableData.DonThuID = item.DonThuID;
                        tableData.XuLyDonID = item.XuLyDonID;

                        var DataArr = new List<RowItem>();
                        RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "", ref DataArr);
                        RowItem RowItem2 = new RowItem(2, item.SoDon, "", "", null, "", ref DataArr);
                        RowItem RowItem3 = new RowItem(3, item.NgayNhapDonStr, "", "", null, "", ref DataArr);
                        RowItem RowItem4 = new RowItem(4, item.TenChuDon, "", "", null, "", ref DataArr);
                        RowItem RowItem5 = new RowItem(5, item.DiaChi, "", "", null, "", ref DataArr);
                        RowItem RowItem6 = new RowItem(6, item.NoiDungDon, "", "", null, "", ref DataArr);
                        RowItem RowItem7 = new RowItem(7, item.TenLoaiKhieuTo, "", "", null, "", ref DataArr);
                        RowItem RowItem8 = new RowItem(8, item.TenHuongGiaiQuyet, "", "", null, "", ref DataArr);
                        RowItem RowItem9 = new RowItem(9, item.KetQuaID_Str, "", "", null, "", ref DataArr);

                        tableData.DataArr = DataArr;
                        data.Add(tableData);
                    }
                    BaoCaoModel.DataTable.TableData = data;
                }

                Result.Status = 1;
                Result.Data = BaoCaoModel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel XLD01_GetDSChiTietDonThu_XuatExcel(BaseReportParams p, string ContentRootPath, int CanBoDangNhapID)
        {
            var Result = new BaseResultModel();
            try
            {
                List<TableData> data = new List<TableData>();
                List<TKDonThuInfo> tKDonThuInfos = new XLD01DAL().XLD01_GetDSChiTietDonThu(ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now, p.Offset, p.PageSize, p.Index, p.XemTaiLieuMat, p.CanBoID, p.CoQuanID ?? 0, p.CapID);
                if (tKDonThuInfos.Count > 0)
                {
                    int stt = p.Offset;
                    foreach (var item in tKDonThuInfos)
                    {
                        TableData tableData = new TableData();
                        tableData.ID = stt++;
                        tableData.DonThuID = item.DonThuID;
                        tableData.XuLyDonID = item.XuLyDonID;

                        var DataArr = new List<RowItem>();
                        RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "", ref DataArr);
                        RowItem RowItem2 = new RowItem(2, item.SoDon, "", "", null, "", ref DataArr);
                        RowItem RowItem3 = new RowItem(3, item.NgayNhapDonStr, "", "", null, "", ref DataArr);
                        RowItem RowItem4 = new RowItem(4, item.TenChuDon, "", "", null, "", ref DataArr);
                        RowItem RowItem5 = new RowItem(5, item.DiaChi, "", "", null, "", ref DataArr);
                        RowItem RowItem6 = new RowItem(6, item.NoiDungDon, "", "", null, "", ref DataArr);
                        RowItem RowItem7 = new RowItem(7, item.TenLoaiKhieuTo, "", "", null, "", ref DataArr);
                        RowItem RowItem8 = new RowItem(8, item.TenHuongGiaiQuyet, "", "", null, "", ref DataArr);
                        RowItem RowItem9 = new RowItem(9, item.KetQuaID_Str, "", "", null, "", ref DataArr);

                        tableData.DataArr = DataArr;
                        data.Add(tableData);
                    }
                }
                string path = @"Templates\FileTam\XLD01_DSChiTietDonThu" + CanBoDangNhapID + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";
                string urlExcel = new TCD01DAL().DSChiTietDonThu_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                Result.Status = 1;
                Result.Data = urlExcel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel XLD02(BaseReportParams p, string ContentRootPath, int RoleID, int CapID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, int HuyenDangNhapID)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.BieuSo = "Biểu số: 02/XLD";
                BaoCaoModel.ThongTinSoLieu = "Số liệu tính từ ngày " + (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy") + " đến ngày " + (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.TuNgay = (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.DenNgay = (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.Title = "TỔNG HỢP KẾT QUẢ XỬ LÝ ĐƠN KHIẾU NẠI";
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();
                #region Header
                var listTableHeader = new List<TableHeader>();
                //Cấp 1
                TableHeader HeaderCol1 = new TableHeader(1, 0, "Đơn vị", "width: 215px", ref listTableHeader);
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Tổng số đơn", "", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Đơn kỳ trước chuyển sang", "", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Đơn tiếp nhận đơn trong kỳ", "", ref listTableHeader);
                TableHeader HeaderCol5 = new TableHeader(5, 0, "Đơn đã xử lý", "", ref listTableHeader);
                TableHeader HeaderCol6 = new TableHeader(6, 0, "Đủ điều kiện xử lý", "", ref listTableHeader);
                TableHeader HeaderCol7 = new TableHeader(7, 0, "Phân loại vụ việc theo nội dung", "", ref listTableHeader);
                TableHeader HeaderCol8 = new TableHeader(8, 0, "Phân loại vụ việc theo tình trạng giải quyết", "", ref listTableHeader);
                TableHeader HeaderCol9 = new TableHeader(9, 0, "Kết quả xử lý", "", ref listTableHeader);
                TableHeader HeaderCol10 = new TableHeader(10, 0, "Số văn bản phúc đáp nhận được do chuyển đơn", "", ref listTableHeader);
                //Cấp 2
                var DataChild3 = new List<TableHeader>();
                TableHeader HeaderCol31 = new TableHeader(31, 3, "Đơn có nhiều người đứng tên", "", ref DataChild3);
                TableHeader HeaderCol32 = new TableHeader(32, 3, "Đơn một người đứng tên", "", ref DataChild3);
                HeaderCol3.DataChild = DataChild3;

                var DataChild4 = new List<TableHeader>();
                TableHeader HeaderCol41 = new TableHeader(41, 4, "Đơn có nhiều người đứng tên", "", ref DataChild4);
                TableHeader HeaderCol42 = new TableHeader(42, 4, "Đơn một người đứng tên", "", ref DataChild4);
                HeaderCol4.DataChild = DataChild4;

                var DataChild5 = new List<TableHeader>();
                TableHeader HeaderCol51 = new TableHeader(51, 5, "Tổng", "", ref DataChild5);
                TableHeader HeaderCol52 = new TableHeader(52, 5, "Đơn kỳ trước chuyển sang", "", ref DataChild5);
                TableHeader HeaderCol53 = new TableHeader(53, 5, "Đơn tiếp nhận trong kỳ", "", ref DataChild5);
                HeaderCol5.DataChild = DataChild5;

                var DataChild6 = new List<TableHeader>();
                TableHeader HeaderCol61 = new TableHeader(61, 6, "Số đơn", "", ref DataChild6);
                TableHeader HeaderCol62 = new TableHeader(62, 6, "Số vụ việc", "", ref DataChild6);
                HeaderCol6.DataChild = DataChild6;

                var DataChild7 = new List<TableHeader>();
                TableHeader HeaderCol71 = new TableHeader(71, 7, "Lĩnh vực hành chính", "", ref DataChild7);
                TableHeader HeaderCol72 = new TableHeader(72, 7, "Lĩnh vực tư pháp", "", ref DataChild7);
                TableHeader HeaderCol73 = new TableHeader(73, 7, "Lĩnh vực Đảng, đoàn thể", "", ref DataChild7);
                TableHeader HeaderCol74 = new TableHeader(74, 7, "Lĩnh vực khác", "", ref DataChild7);
                HeaderCol7.DataChild = DataChild7;

                var DataChild8 = new List<TableHeader>();
                TableHeader HeaderCol81 = new TableHeader(81, 8, "Đã giải quyết", "", ref DataChild8);
                TableHeader HeaderCol82 = new TableHeader(82, 8, "Đang giải quyết", "", ref DataChild8);
                TableHeader HeaderCol83 = new TableHeader(83, 8, "Chưa giải quyết", "", ref DataChild8);
                HeaderCol8.DataChild = DataChild8;

                var DataChild9 = new List<TableHeader>();
                TableHeader HeaderCol91 = new TableHeader(91, 9, "Vụ việc thuộc thẩm quyền", "", ref DataChild9);
                TableHeader HeaderCol92 = new TableHeader(92, 9, "Vụ việc không thuộc thẩm quyền", "", ref DataChild9);
                HeaderCol9.DataChild = DataChild9;
                //Cấp 3
                var DataChild71 = new List<TableHeader>();
                TableHeader HeaderCol711 = new TableHeader(711, 71, "Tổng", "", ref DataChild71);
                TableHeader HeaderCol712 = new TableHeader(712, 71, "Chế độ, chính sách", "", ref DataChild71);
                TableHeader HeaderCol713 = new TableHeader(713, 71, "Đất đai, nhà cửa", "", ref DataChild71);
                TableHeader HeaderCol714 = new TableHeader(714, 71, "Khác", "", ref DataChild71);
                HeaderCol71.DataChild = DataChild71;

                var DataChild81 = new List<TableHeader>();
                TableHeader HeaderCol811 = new TableHeader(811, 81, "Lần đầu", "", ref DataChild81);
                TableHeader HeaderCol812 = new TableHeader(812, 81, "Lần 2", "", ref DataChild81);
                TableHeader HeaderCol813 = new TableHeader(813, 81, "Đã có bản án của tòa", "", ref DataChild81);
                HeaderCol81.DataChild = DataChild81;

                var DataChild91 = new List<TableHeader>();
                TableHeader HeaderCol911 = new TableHeader(911, 91, "Tổng", "", ref DataChild91);
                TableHeader HeaderCol912 = new TableHeader(912, 91, "Lần đầu", "", ref DataChild91);
                TableHeader HeaderCol913 = new TableHeader(913, 91, "Lần 2", "", ref DataChild91);
                HeaderCol91.DataChild = DataChild91;

                var DataChild92 = new List<TableHeader>();
                TableHeader HeaderCol921 = new TableHeader(921, 92, "Tổng", "", ref DataChild92);
                TableHeader HeaderCol922 = new TableHeader(922, 92, "Hướng dẫn", "", ref DataChild92);
                TableHeader HeaderCol923 = new TableHeader(923, 92, "Chuyển đơn", "", ref DataChild92);
                TableHeader HeaderCol924 = new TableHeader(924, 92, "Đôn đổc giải quyết", "", ref DataChild92);
                HeaderCol92.DataChild = DataChild92;

                BaoCaoModel.DataTable.TableHeader = listTableHeader;
                #endregion
                #region TableData 
                TableData Row1 = new TableData();
                Row1.ID = 1;
                Row1.isClick = false;
                var DataArr = new List<RowItem>();
                RowItem RowItem1 = new RowItem(1, "Ms", "", "", null, "width: 215px", ref DataArr);
                RowItem RowItem2 = new RowItem(2, "1=2+..+5", "", "", null, "text-align: center;width: 250px", ref DataArr);
                RowItem RowItem3 = new RowItem(3, "2", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem4 = new RowItem(4, "3", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem5 = new RowItem(5, "4", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem6 = new RowItem(6, "5", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem7 = new RowItem(7, "6 = 7+8", "", "", null, "text-align: center;width: 250px", ref DataArr);
                RowItem RowItem8 = new RowItem(8, "7", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem9 = new RowItem(9, "8", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem10 = new RowItem(10, "9", "", "", null, "", ref DataArr);
                RowItem RowItem11 = new RowItem(11, "10=11+15+16 +17= 18+…+22= 23+26", "", "", null, "text-align: center;width: 250px", ref DataArr);
                RowItem RowItem12 = new RowItem(12, "11", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem13 = new RowItem(13, "12", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem14 = new RowItem(14, "13", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem15 = new RowItem(15, "14", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem16 = new RowItem(16, "15", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem17 = new RowItem(17, "16", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem18 = new RowItem(18, "17", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem19 = new RowItem(19, "18", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem20 = new RowItem(20, "19", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem21 = new RowItem(21, "20", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem22 = new RowItem(22, "21", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem23 = new RowItem(23, "22", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem24 = new RowItem(24, "23=24+25", "", "", null, "text-align: center;width: 250px", ref DataArr);
                RowItem RowItem25 = new RowItem(25, "24", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem26 = new RowItem(26, "25", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem27 = new RowItem(27, "26 = 27+ 28+29", "", "", null, "text-align: center;width: 250px", ref DataArr);
                RowItem RowItem28 = new RowItem(28, "27", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem29 = new RowItem(29, "28", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem30 = new RowItem(30, "29", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem31 = new RowItem(31, "30", "", "", null, "text-align: center;width: 100px", ref DataArr);
               

                Row1.DataArr = DataArr;
                BaoCaoModel.DataTable.TableData.Add(Row1);

                List<TableData> data = new List<TableData>();
                data = new XLD02DAL().XLD02(p.ListCapID, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                BaoCaoModel.DataTable.TableData.AddRange(data);

                #endregion

                Result.Status = 1;
                Result.Data = BaoCaoModel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel XLD02_XuatExcel(BaseReportParams p, string ContentRootPath, int RoleID, int CapID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, int HuyenDangNhapID)
        {
            var Result = new BaseResultModel();
            try
            {
                List<TableData> data = new List<TableData>();
                data = new XLD02DAL().XLD02(p.ListCapID, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                #region Thêm hàng Tổng
                TableData rowToanTinh = new TableData();
                TableData rowToanHuyen = new TableData();
                TableData rowTong = new TableData();
                if (data.Count > 1)
                {
                    foreach (var item in data)
                    {
                        if (item.DataArr != null && item.DataArr.Count > 0)
                        {
                            foreach (var col in item.DataArr)
                            {
                                if (col.CapID == CapCoQuanViewChiTiet.ToanTinh.GetHashCode().ToString())
                                {
                                    rowToanTinh = Utils.DeepCopy(item);
                                    break;
                                }
                                else if (col.CapID == CapCoQuanViewChiTiet.ToanHuyen.GetHashCode().ToString())
                                {
                                    rowToanHuyen = Utils.DeepCopy(item);
                                    break;
                                }
                            }
                        }
                    }
                }
                else if (data.Count == 1)
                {
                    rowTong = Utils.DeepCopy(data[0]);
                }

                if (rowToanTinh.DataArr != null && rowToanTinh.DataArr.Count > 0)
                {
                    rowTong.DataArr = rowToanTinh.DataArr;
                }
                else if (rowToanHuyen.DataArr != null && rowToanHuyen.DataArr.Count > 0)
                {
                    rowTong.DataArr = rowToanHuyen.DataArr;
                }
                if (rowTong.DataArr != null)
                {
                    rowTong.DataArr[0].Content = "Tổng";
                    data.Add(rowTong);
                }
                #endregion
                string path = @"Templates\FileTam\XLD02_" + CanBoDangNhapID + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";
                string urlExcel = new XLD02DAL().XLD02_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                Result.Status = 1;
                Result.Data = urlExcel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel XLD02_GetDSChiTietDonThu(string ContentRootPath, BaseReportParams p)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();

                var listTableHeader = new List<TableHeader>();
                TableHeader HeaderCol1 = new TableHeader(1, 0, "STT", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Số đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Ngày tiếp nhận", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Chủ đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol5 = new TableHeader(5, 0, "Địa chỉ", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol6 = new TableHeader(6, 0, "Nội dung đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol7 = new TableHeader(7, 0, "Loại đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol8 = new TableHeader(8, 0, "Kết quả xử lý", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol9 = new TableHeader(9, 0, "Kết quả giải quyết", "font-weight:bold", ref listTableHeader);
                BaoCaoModel.DataTable.TableHeader = listTableHeader;

                List<TKDonThuInfo> tKDonThuInfos = new List<TKDonThuInfo>();
                tKDonThuInfos = new XLD02DAL().XLD02_GetDSChiTietDonThu(ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now, p.Offset, p.PageSize, p.Index, p.XemTaiLieuMat, p.CanBoID, p.CoQuanID ?? 0, p.CapID);
                if (tKDonThuInfos.Count > 0)
                {
                    List<TableData> data = new List<TableData>();
                    int stt = p.Offset;
                    foreach (var item in tKDonThuInfos)
                    {
                        TableData tableData = new TableData();
                        tableData.ID = stt++;
                        tableData.DonThuID = item.DonThuID;
                        tableData.XuLyDonID = item.XuLyDonID;

                        var DataArr = new List<RowItem>();
                        RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "", ref DataArr);
                        RowItem RowItem2 = new RowItem(2, item.SoDon, "", "", null, "", ref DataArr);
                        RowItem RowItem3 = new RowItem(3, item.NgayNhapDonStr, "", "", null, "", ref DataArr);
                        RowItem RowItem4 = new RowItem(4, item.TenChuDon, "", "", null, "", ref DataArr);
                        RowItem RowItem5 = new RowItem(5, item.DiaChi, "", "", null, "", ref DataArr);
                        RowItem RowItem6 = new RowItem(6, item.NoiDungDon, "", "", null, "", ref DataArr);
                        RowItem RowItem7 = new RowItem(7, item.TenLoaiKhieuTo, "", "", null, "", ref DataArr);
                        RowItem RowItem8 = new RowItem(8, item.TenHuongGiaiQuyet, "", "", null, "", ref DataArr);
                        RowItem RowItem9 = new RowItem(9, item.KetQuaID_Str, "", "", null, "", ref DataArr);

                        tableData.DataArr = DataArr;
                        data.Add(tableData);
                    }
                    BaoCaoModel.DataTable.TableData = data;
                }

                Result.Status = 1;
                Result.Data = BaoCaoModel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel XLD02_GetDSChiTietDonThu_XuatExcel(BaseReportParams p, string ContentRootPath, int CanBoDangNhapID)
        {
            var Result = new BaseResultModel();
            try
            {
                List<TableData> data = new List<TableData>();
                List<TKDonThuInfo> tKDonThuInfos = new XLD02DAL().XLD02_GetDSChiTietDonThu(ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now, p.Offset, p.PageSize, p.Index, p.XemTaiLieuMat, p.CanBoID, p.CoQuanID ?? 0, p.CapID);
                if (tKDonThuInfos.Count > 0)
                {
                    int stt = p.Offset;
                    foreach (var item in tKDonThuInfos)
                    {
                        TableData tableData = new TableData();
                        tableData.ID = stt++;
                        tableData.DonThuID = item.DonThuID;
                        tableData.XuLyDonID = item.XuLyDonID;

                        var DataArr = new List<RowItem>();
                        RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "", ref DataArr);
                        RowItem RowItem2 = new RowItem(2, item.SoDon, "", "", null, "", ref DataArr);
                        RowItem RowItem3 = new RowItem(3, item.NgayNhapDonStr, "", "", null, "", ref DataArr);
                        RowItem RowItem4 = new RowItem(4, item.TenChuDon, "", "", null, "", ref DataArr);
                        RowItem RowItem5 = new RowItem(5, item.DiaChi, "", "", null, "", ref DataArr);
                        RowItem RowItem6 = new RowItem(6, item.NoiDungDon, "", "", null, "", ref DataArr);
                        RowItem RowItem7 = new RowItem(7, item.TenLoaiKhieuTo, "", "", null, "", ref DataArr);
                        RowItem RowItem8 = new RowItem(8, item.TenHuongGiaiQuyet, "", "", null, "", ref DataArr);
                        RowItem RowItem9 = new RowItem(9, item.KetQuaID_Str, "", "", null, "", ref DataArr);

                        tableData.DataArr = DataArr;
                        data.Add(tableData);
                    }
                }
                string path = @"Templates\FileTam\XLD02_DSChiTietDonThu" + CanBoDangNhapID + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";
                string urlExcel = new TCD01DAL().DSChiTietDonThu_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                Result.Status = 1;
                Result.Data = urlExcel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel XLD03(BaseReportParams p, string ContentRootPath, int RoleID, int CapID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, int HuyenDangNhapID)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.BieuSo = "Biểu số: 03/XLD";
                BaoCaoModel.ThongTinSoLieu = "Số liệu tính từ ngày " + (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy") + " đến ngày " + (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.TuNgay = (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.DenNgay = (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.Title = "TỔNG HỢP KẾT QUẢ XỬ LÝ ĐƠN TỐ CÁO";
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();
                #region Header
                var listTableHeader = new List<TableHeader>();
                //Cấp 1
                TableHeader HeaderCol1 = new TableHeader(1, 0, "Đơn vị", "width: 215px", ref listTableHeader);
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Tổng số đơn", "", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Đơn kỳ trước chuyển sang", "", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Đơn tiếp nhận đơn trong kỳ", "", ref listTableHeader);
                TableHeader HeaderCol5 = new TableHeader(5, 0, "Đơn đã xử lý", "", ref listTableHeader);
                TableHeader HeaderCol6 = new TableHeader(6, 0, "Đủ điều kiện xử lý", "", ref listTableHeader);
                TableHeader HeaderCol7 = new TableHeader(7, 0, "Phân loại vụ việc theo nội dung", "", ref listTableHeader);
                TableHeader HeaderCol8 = new TableHeader(8, 0, "Phân loại vụ việc theo tình trạng giải quyết", "", ref listTableHeader);
                TableHeader HeaderCol9 = new TableHeader(9, 0, "Kết quả xử lý", "", ref listTableHeader);
                TableHeader HeaderCol10 = new TableHeader(10, 0, "Số văn bản phúc đáp nhận được do chuyển đơn", "", ref listTableHeader);
                //Cấp 2
                var DataChild3 = new List<TableHeader>();
                TableHeader HeaderCol31 = new TableHeader(31, 3, "Đơn có nhiều người đứng tên", "", ref DataChild3);
                TableHeader HeaderCol32 = new TableHeader(32, 3, "Đơn một người đứng tên", "", ref DataChild3);
                TableHeader HeaderCol33 = new TableHeader(33, 3, "Đơn khác", "", ref DataChild3);
                HeaderCol3.DataChild = DataChild3;

                var DataChild4 = new List<TableHeader>();
                TableHeader HeaderCol41 = new TableHeader(41, 4, "Đơn có nhiều người đứng tên", "", ref DataChild4);
                TableHeader HeaderCol42 = new TableHeader(42, 4, "Đơn một người đứng tên", "", ref DataChild4);
                TableHeader HeaderCol43 = new TableHeader(43, 4, "Đơn khác", "", ref DataChild4);
                HeaderCol4.DataChild = DataChild4;

                var DataChild5 = new List<TableHeader>();
                TableHeader HeaderCol51 = new TableHeader(51, 5, "Tổng", "", ref DataChild5);
                TableHeader HeaderCol52 = new TableHeader(52, 5, "Đơn kỳ trước chuyển sang", "", ref DataChild5);
                TableHeader HeaderCol53 = new TableHeader(53, 5, "Đơn tiếp nhận trong kỳ", "", ref DataChild5);
                HeaderCol5.DataChild = DataChild5;

                var DataChild6 = new List<TableHeader>();
                TableHeader HeaderCol61 = new TableHeader(61, 6, "Số đơn", "", ref DataChild6);
                TableHeader HeaderCol62 = new TableHeader(62, 6, "Số vụ việc", "", ref DataChild6);
                HeaderCol6.DataChild = DataChild6;
                
                //var DataChild6 = new List<TableHeader>();
                //TableHeader HeaderCol61 = new TableHeader(61, 6, "Lĩnh vực hành chính", "", ref DataChild6);
                //TableHeader HeaderCol62 = new TableHeader(62, 6, "Lĩnh vực tư pháp", "", ref DataChild6);
                //TableHeader HeaderCol63 = new TableHeader(63, 6, "Lĩnh vực Đảng, đoàn thể", "", ref DataChild6);
                //TableHeader HeaderCol64 = new TableHeader(64, 6, "Lĩnh vực khác", "", ref DataChild6);
                //HeaderCol6.DataChild = DataChild6;

                var DataChild7 = new List<TableHeader>();
                TableHeader HeaderCol71 = new TableHeader(71, 7, "Lĩnh vực hành chính", "", ref DataChild7);
                TableHeader HeaderCol72 = new TableHeader(72, 7, "Tham nhũng", "", ref DataChild7);
                TableHeader HeaderCol73 = new TableHeader(73, 7, "Lĩnh vực tư pháp", "", ref DataChild7);
                TableHeader HeaderCol74 = new TableHeader(74, 7, "Lĩnh vực Đảng, đoàn thể", "", ref DataChild7);
                TableHeader HeaderCol75 = new TableHeader(75, 7, "Lĩnh vực khác", "", ref DataChild7);
                HeaderCol7.DataChild = DataChild7;

                var DataChild8 = new List<TableHeader>();
                TableHeader HeaderCol81 = new TableHeader(81, 8, "Chưa giải quyết, trong hạn", "", ref DataChild8);
                TableHeader HeaderCol82 = new TableHeader(82, 8, "Đang giải quyết", "", ref DataChild8);
                TableHeader HeaderCol83 = new TableHeader(83, 8, "Tố cáo tiếp", "", ref DataChild8);
                HeaderCol8.DataChild = DataChild8;

                var DataChild9 = new List<TableHeader>();
                TableHeader HeaderCol91 = new TableHeader(91, 9, "Vụ việc thuộc thẩm quyền", "", ref DataChild9);
                TableHeader HeaderCol92 = new TableHeader(92, 9, "Vụ việc không thuộc thẩm quyền", "", ref DataChild9);
                HeaderCol9.DataChild = DataChild9;
                //Cấp 3
                //var DataChild61 = new List<TableHeader>();
                //TableHeader HeaderCol611 = new TableHeader(611, 61, "Tổng", "", ref DataChild61);
                //TableHeader HeaderCol612 = new TableHeader(612, 61, "Chế độ, chính sách", "", ref DataChild61);
                //TableHeader HeaderCol613 = new TableHeader(613, 61, "Đất đai, nhà cửa", "", ref DataChild61);
                //TableHeader HeaderCol614 = new TableHeader(613, 61, "Khác", "", ref DataChild61);
                //HeaderCol61.DataChild = DataChild61;

                var DataChild71 = new List<TableHeader>();
                TableHeader HeaderCol711 = new TableHeader(711, 71, "Tổng cộng", "", ref DataChild71);
                TableHeader HeaderCol712 = new TableHeader(712, 71, "Chế độ, chính sách", "", ref DataChild71);
                TableHeader HeaderCol713 = new TableHeader(713, 71, "Đất đai, nhà cửa", "", ref DataChild71);
                TableHeader HeaderCol714 = new TableHeader(714, 71, "Công chức, công vụ", "", ref DataChild71);
                TableHeader HeaderCol715 = new TableHeader(715, 71, "Khác", "", ref DataChild71);
                HeaderCol71.DataChild = DataChild71;

                var DataChild83 = new List<TableHeader>();
                TableHeader HeaderCol831 = new TableHeader(831, 83, "Quá thời hạn chưa giải quyết", "", ref DataChild83);
                TableHeader HeaderCol832 = new TableHeader(832, 83, "Đã có kết luận giải quyết", "", ref DataChild83);
                HeaderCol83.DataChild = DataChild83;

                var DataChild91 = new List<TableHeader>();
                TableHeader HeaderCol911 = new TableHeader(911, 91, "Tổng số", "", ref DataChild91);
                TableHeader HeaderCol912 = new TableHeader(912, 91, "Tố cáo lần đầu", "", ref DataChild91);
                TableHeader HeaderCol913 = new TableHeader(913, 91, "Tố cáo tiếp", "", ref DataChild91);
                HeaderCol91.DataChild = DataChild91;

                var DataChild92 = new List<TableHeader>();
                TableHeader HeaderCol921 = new TableHeader(921, 91, "Tổng số", "", ref DataChild92);
                TableHeader HeaderCol922 = new TableHeader(922, 91, "Hướng dẫn", "", ref DataChild92);
                TableHeader HeaderCol923 = new TableHeader(923, 91, "Chuyển đơn", "", ref DataChild92);
                TableHeader HeaderCol924 = new TableHeader(924, 91, "Đôn đốc giải quyết", "", ref DataChild92);
                HeaderCol92.DataChild = DataChild92;

                BaoCaoModel.DataTable.TableHeader = listTableHeader;
                #endregion
                #region TableData 
                TableData Row1 = new TableData();
                Row1.ID = 1;
                Row1.isClick = false;
                var DataArr = new List<RowItem>();
                RowItem RowItem1 = new RowItem(1, "Ms", "", "", null, "width: 215px", ref DataArr);
                RowItem RowItem2 = new RowItem(2, "1=2+..+7", "", "", null, "text-align: center;width: 250px", ref DataArr);
                RowItem RowItem3 = new RowItem(3, "2", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem4 = new RowItem(4, "3", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem5 = new RowItem(5, "4", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem6 = new RowItem(6, "5", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem7 = new RowItem(7, "6", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem8 = new RowItem(8, "7", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem9 = new RowItem(9, "8", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem10 = new RowItem(10, "9", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem11 = new RowItem(11, "10", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem12 = new RowItem(12, "11", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem13 = new RowItem(13, "12=13+18 +...+21= 22+...+25 =26+29", "", "", null, "text-align: center;width: 250px", ref DataArr);
                RowItem RowItem14 = new RowItem(14, "13", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem15 = new RowItem(15, "14", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem16 = new RowItem(16, "15", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem17 = new RowItem(17, "16", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem18 = new RowItem(18, "17", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem19 = new RowItem(19, "18", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem20 = new RowItem(20, "19", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem21 = new RowItem(21, "20", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem22 = new RowItem(22, "21", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem23 = new RowItem(23, "22", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem24 = new RowItem(24, "23", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem25 = new RowItem(25, "24", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem26 = new RowItem(26, "25", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem27 = new RowItem(27, "26 = 27+28", "", "", null, "text-align: center;width: 250px", ref DataArr);
                RowItem RowItem28 = new RowItem(28, "27", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem29 = new RowItem(29, "28", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem30 = new RowItem(30, "29=30+ 31+32", "", "", null, "text-align: center;width: 250px", ref DataArr);
                RowItem RowItem31 = new RowItem(31, "30", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem32 = new RowItem(32, "31", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem33 = new RowItem(33, "32", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem34 = new RowItem(34, "33", "", "", null, "text-align: center;width: 100px", ref DataArr);


                Row1.DataArr = DataArr;
                BaoCaoModel.DataTable.TableData.Add(Row1);

                List<TableData> data = new List<TableData>();
                data = new XLD03DAL().XLD03(p.ListCapID, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                BaoCaoModel.DataTable.TableData.AddRange(data);

                #endregion

                Result.Status = 1;
                Result.Data = BaoCaoModel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel XLD03_XuatExcel(BaseReportParams p, string ContentRootPath, int RoleID, int CapID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, int HuyenDangNhapID)
        {
            var Result = new BaseResultModel();
            try
            {
                List<TableData> data = new List<TableData>();
                data = new XLD03DAL().XLD03(p.ListCapID, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                #region Thêm hàng Tổng
                TableData rowToanTinh = new TableData();
                TableData rowToanHuyen = new TableData();
                TableData rowTong = new TableData();
                if (data.Count > 1)
                {
                    foreach (var item in data)
                    {
                        if (item.DataArr != null && item.DataArr.Count > 0)
                        {
                            foreach (var col in item.DataArr)
                            {
                                if (col.CapID == CapCoQuanViewChiTiet.ToanTinh.GetHashCode().ToString())
                                {
                                    rowToanTinh = Utils.DeepCopy(item);
                                    break;
                                }
                                else if (col.CapID == CapCoQuanViewChiTiet.ToanHuyen.GetHashCode().ToString())
                                {
                                    rowToanHuyen = Utils.DeepCopy(item);
                                    break;
                                }
                            }
                        }
                    }
                }
                else if (data.Count == 1)
                {
                    rowTong = Utils.DeepCopy(data[0]);
                }

                if (rowToanTinh.DataArr != null && rowToanTinh.DataArr.Count > 0)
                {
                    rowTong.DataArr = rowToanTinh.DataArr;
                }
                else if (rowToanHuyen.DataArr != null && rowToanHuyen.DataArr.Count > 0)
                {
                    rowTong.DataArr = rowToanHuyen.DataArr;
                }
                if (rowTong.DataArr != null)
                {
                    rowTong.DataArr[0].Content = "Tổng";
                    data.Add(rowTong);
                }
                #endregion
                string path = @"Templates\FileTam\XLD03_" + CanBoDangNhapID + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";
                string urlExcel = new XLD03DAL().XLD03_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                Result.Status = 1;
                Result.Data = urlExcel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel XLD03_GetDSChiTietDonThu(string ContentRootPath, BaseReportParams p)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();

                var listTableHeader = new List<TableHeader>();
                TableHeader HeaderCol1 = new TableHeader(1, 0, "STT", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Số đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Ngày tiếp nhận", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Chủ đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol5 = new TableHeader(5, 0, "Địa chỉ", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol6 = new TableHeader(6, 0, "Nội dung đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol7 = new TableHeader(7, 0, "Loại đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol8 = new TableHeader(8, 0, "Kết quả xử lý", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol9 = new TableHeader(9, 0, "Kết quả giải quyết", "font-weight:bold", ref listTableHeader);
                BaoCaoModel.DataTable.TableHeader = listTableHeader;

                List<TKDonThuInfo> tKDonThuInfos = new List<TKDonThuInfo>();
                tKDonThuInfos = new XLD03DAL().XLD03_GetDSChiTietDonThu(ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now, p.Offset, p.PageSize, p.Index, p.XemTaiLieuMat, p.CanBoID, p.CoQuanID ?? 0, p.CapID);
                if (tKDonThuInfos.Count > 0)
                {
                    List<TableData> data = new List<TableData>();
                    int stt = p.Offset;
                    foreach (var item in tKDonThuInfos)
                    {
                        TableData tableData = new TableData();
                        tableData.ID = stt++;
                        tableData.DonThuID = item.DonThuID;
                        tableData.XuLyDonID = item.XuLyDonID;

                        var DataArr = new List<RowItem>();
                        RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "", ref DataArr);
                        RowItem RowItem2 = new RowItem(2, item.SoDon, "", "", null, "", ref DataArr);
                        RowItem RowItem3 = new RowItem(3, item.NgayNhapDonStr, "", "", null, "", ref DataArr);
                        RowItem RowItem4 = new RowItem(4, item.TenChuDon, "", "", null, "", ref DataArr);
                        RowItem RowItem5 = new RowItem(5, item.DiaChi, "", "", null, "", ref DataArr);
                        RowItem RowItem6 = new RowItem(6, item.NoiDungDon, "", "", null, "", ref DataArr);
                        RowItem RowItem7 = new RowItem(7, item.TenLoaiKhieuTo, "", "", null, "", ref DataArr);
                        RowItem RowItem8 = new RowItem(8, item.TenHuongGiaiQuyet, "", "", null, "", ref DataArr);
                        RowItem RowItem9 = new RowItem(9, item.KetQuaID_Str, "", "", null, "", ref DataArr);

                        tableData.DataArr = DataArr;
                        data.Add(tableData);
                    }
                    BaoCaoModel.DataTable.TableData = data;
                }

                Result.Status = 1;
                Result.Data = BaoCaoModel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel XLD03_GetDSChiTietDonThu_XuatExcel(BaseReportParams p, string ContentRootPath, int CanBoDangNhapID)
        {
            var Result = new BaseResultModel();
            try
            {
                List<TableData> data = new List<TableData>();
                List<TKDonThuInfo> tKDonThuInfos = new XLD03DAL().XLD03_GetDSChiTietDonThu(ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now, p.Offset, p.PageSize, p.Index, p.XemTaiLieuMat, p.CanBoID, p.CoQuanID ?? 0, p.CapID);
                if (tKDonThuInfos.Count > 0)
                {
                    int stt = p.Offset;
                    foreach (var item in tKDonThuInfos)
                    {
                        TableData tableData = new TableData();
                        tableData.ID = stt++;
                        tableData.DonThuID = item.DonThuID;
                        tableData.XuLyDonID = item.XuLyDonID;

                        var DataArr = new List<RowItem>();
                        RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "", ref DataArr);
                        RowItem RowItem2 = new RowItem(2, item.SoDon, "", "", null, "", ref DataArr);
                        RowItem RowItem3 = new RowItem(3, item.NgayNhapDonStr, "", "", null, "", ref DataArr);
                        RowItem RowItem4 = new RowItem(4, item.TenChuDon, "", "", null, "", ref DataArr);
                        RowItem RowItem5 = new RowItem(5, item.DiaChi, "", "", null, "", ref DataArr);
                        RowItem RowItem6 = new RowItem(6, item.NoiDungDon, "", "", null, "", ref DataArr);
                        RowItem RowItem7 = new RowItem(7, item.TenLoaiKhieuTo, "", "", null, "", ref DataArr);
                        RowItem RowItem8 = new RowItem(8, item.TenHuongGiaiQuyet, "", "", null, "", ref DataArr);
                        RowItem RowItem9 = new RowItem(9, item.KetQuaID_Str, "", "", null, "", ref DataArr);

                        tableData.DataArr = DataArr;
                        data.Add(tableData);
                    }
                }
                string path = @"Templates\FileTam\XLD03_DSChiTietDonThu" + CanBoDangNhapID + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";
                string urlExcel = new TCD01DAL().DSChiTietDonThu_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                Result.Status = 1;
                Result.Data = urlExcel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel XLD04(BaseReportParams p, string ContentRootPath, int RoleID, int CapID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, int HuyenDangNhapID)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.BieuSo = "Biểu số: 04/XLD";
                BaoCaoModel.ThongTinSoLieu = "Số liệu tính từ ngày " + (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy") + " đến ngày " + (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.TuNgay = (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.DenNgay = (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.Title = "TỔNG HỢP KẾT QUẢ XỬ LÝ ĐƠN KIẾN NGHỊ, PHẢN ÁNH";
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();
                #region Header
                var listTableHeader = new List<TableHeader>();
                //Cấp 1
                TableHeader HeaderCol1 = new TableHeader(1, 0, "Đơn vị", "width: 215px", ref listTableHeader);
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Tổng số đơn", "", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Đơn kỳ trước chuyển sang", "", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Đơn tiếp nhận đơn trong kỳ", "", ref listTableHeader);
                TableHeader HeaderCol5 = new TableHeader(5, 0, "Đã xử lý trong kỳ", "", ref listTableHeader);
                TableHeader HeaderCol6 = new TableHeader(6, 0, "Đủ điều kiện xử lý", "", ref listTableHeader);
                TableHeader HeaderCol7 = new TableHeader(7, 0, "Phân loại vụ việc theo nội dung", "", ref listTableHeader);
                TableHeader HeaderCol8 = new TableHeader(8, 0, "Phân loại vụ việc theo tình trạng giải quyết", "", ref listTableHeader);
                TableHeader HeaderCol9 = new TableHeader(9, 0, "Kết quả xử lý", "", ref listTableHeader);
                TableHeader HeaderCol10 = new TableHeader(10, 0, "Số văn bản phúc đáp nhận được do chuyển đơn", "", ref listTableHeader);
                //Cấp 2
                var DataChild3 = new List<TableHeader>();
                TableHeader HeaderCol31 = new TableHeader(31, 3, "Đơn có nhiều người đứng tên", "", ref DataChild3);
                TableHeader HeaderCol32 = new TableHeader(32, 3, "Đơn một người đứng tên", "", ref DataChild3);
                TableHeader HeaderCol33 = new TableHeader(33, 3, "Đơn khác", "", ref DataChild3);
                HeaderCol3.DataChild = DataChild3;

                var DataChild4 = new List<TableHeader>();
                TableHeader HeaderCol41 = new TableHeader(41, 4, "Đơn có nhiều người đứng tên", "", ref DataChild4);
                TableHeader HeaderCol42 = new TableHeader(42, 4, "Đơn một người đứng tên", "", ref DataChild4);
                TableHeader HeaderCol43 = new TableHeader(43, 4, "Đơn khác", "", ref DataChild4);
                HeaderCol4.DataChild = DataChild4;

                var DataChild5 = new List<TableHeader>();
                TableHeader HeaderCol51 = new TableHeader(51, 5, "Tổng số", "", ref DataChild5);
                TableHeader HeaderCol52 = new TableHeader(52, 5, "Đơn kỳ trước chuyển sang", "", ref DataChild5);
                TableHeader HeaderCol53 = new TableHeader(53, 5, "Đơn tiếp nhận trong kỳ", "", ref DataChild5);
                HeaderCol5.DataChild = DataChild5;

                var DataChild6 = new List<TableHeader>();
                TableHeader HeaderCol61 = new TableHeader(61, 6, "Số đơn", "", ref DataChild6);
                TableHeader HeaderCol62 = new TableHeader(62, 6, "Số vụ việc", "", ref DataChild6);
                HeaderCol6.DataChild = DataChild6;

                var DataChild7 = new List<TableHeader>();
                TableHeader HeaderCol71 = new TableHeader(71, 7, "Chế độ, chính sách", "", ref DataChild7);
                TableHeader HeaderCol72 = new TableHeader(72, 7, "Đất đai", "", ref DataChild7);
                TableHeader HeaderCol73 = new TableHeader(73, 7, "Tư pháp", "", ref DataChild7);
                TableHeader HeaderCol74 = new TableHeader(74, 7, "Khác", "", ref DataChild7);
                HeaderCol7.DataChild = DataChild7;

                var DataChild8 = new List<TableHeader>();
                TableHeader HeaderCol81 = new TableHeader(81, 8, "Đã được giải quyết", "", ref DataChild8);
                TableHeader HeaderCol82 = new TableHeader(82, 8, "Đang giải quyết", "", ref DataChild8);
                TableHeader HeaderCol83 = new TableHeader(83, 8, "Chưa được giải quyết", "", ref DataChild8);
                HeaderCol8.DataChild = DataChild8;

                var DataChild9 = new List<TableHeader>();
                TableHeader HeaderCol91 = new TableHeader(91, 9, "Vụ việc thuộc thẩm quyền", "", ref DataChild9);
                TableHeader HeaderCol92 = new TableHeader(92, 9, "Vụ việc không thuộc thẩm quyền", "", ref DataChild9);
                HeaderCol9.DataChild = DataChild9;
                //Cấp 3
                var DataChild92 = new List<TableHeader>();
                TableHeader HeaderCol921 = new TableHeader(921, 92, "Tổng số", "", ref DataChild92);
                TableHeader HeaderCol922 = new TableHeader(922, 92, "Chuyển đơn", "", ref DataChild92);
                TableHeader HeaderCol923 = new TableHeader(923, 92, "Đôn đổc giải quyết", "", ref DataChild92);
                HeaderCol92.DataChild = DataChild92;

                BaoCaoModel.DataTable.TableHeader = listTableHeader;
                #endregion
                #region TableData 
                TableData Row1 = new TableData();
                Row1.ID = 1;
                Row1.isClick = false;
                var DataArr = new List<RowItem>();
                RowItem RowItem1 = new RowItem(1, "Ms", "", "", null, "width: 215px", ref DataArr);
                RowItem RowItem2 = new RowItem(2, "1=2+..+7", "", "", null, "text-align: center;width: 250px", ref DataArr);
                RowItem RowItem3 = new RowItem(3, "2", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem4 = new RowItem(4, "3", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem5 = new RowItem(5, "4", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem6 = new RowItem(6, "5", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem7 = new RowItem(7, "6", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem8 = new RowItem(8, "7", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem9 = new RowItem(9, "8=9+10", "", "", null, "text-align: center;width: 250px", ref DataArr);
                RowItem RowItem10 = new RowItem(10, "9", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem11 = new RowItem(11, "10", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem12 = new RowItem(12, "11", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem13 = new RowItem(13, "12=13+..+ 16=17+..+ 19=20+21", "", "", null, "text-align: center;width: 250px", ref DataArr);
                RowItem RowItem14 = new RowItem(14, "13", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem15 = new RowItem(15, "14", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem16 = new RowItem(16, "15", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem17 = new RowItem(17, "16", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem18 = new RowItem(18, "17", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem19 = new RowItem(19, "18", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem20 = new RowItem(20, "19", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem21 = new RowItem(21, "20", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem22 = new RowItem(22, "21= 22+23", "", "", null, "text-align: center;width: 250px", ref DataArr);
                RowItem RowItem23 = new RowItem(23, "22", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem24 = new RowItem(24, "23", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem25 = new RowItem(25, "24", "", "", null, "text-align: center;width: 100px", ref DataArr);

                Row1.DataArr = DataArr;
                BaoCaoModel.DataTable.TableData.Add(Row1);

                List<TableData> data = new List<TableData>();
                data = new XLD04DAL().XLD04(p.ListCapID, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                BaoCaoModel.DataTable.TableData.AddRange(data);

                #endregion

                Result.Status = 1;
                Result.Data = BaoCaoModel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel XLD04_XuatExcel(BaseReportParams p, string ContentRootPath, int RoleID, int CapID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, int HuyenDangNhapID)
        {
            var Result = new BaseResultModel();
            try
            {
                List<TableData> data = new List<TableData>();
                data = new XLD04DAL().XLD04(p.ListCapID, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                #region Thêm hàng Tổng
                TableData rowToanTinh = new TableData();
                TableData rowToanHuyen = new TableData();
                TableData rowTong = new TableData();
                if (data.Count > 1)
                {
                    foreach (var item in data)
                    {
                        if (item.DataArr != null && item.DataArr.Count > 0)
                        {
                            foreach (var col in item.DataArr)
                            {
                                if (col.CapID == CapCoQuanViewChiTiet.ToanTinh.GetHashCode().ToString())
                                {
                                    rowToanTinh = Utils.DeepCopy(item);
                                    break;
                                }
                                else if (col.CapID == CapCoQuanViewChiTiet.ToanHuyen.GetHashCode().ToString())
                                {
                                    rowToanHuyen = Utils.DeepCopy(item);
                                    break;
                                }
                            }
                        }
                    }
                }
                else if (data.Count == 1)
                {
                    rowTong = Utils.DeepCopy(data[0]);
                }

                if (rowToanTinh.DataArr != null && rowToanTinh.DataArr.Count > 0)
                {
                    rowTong.DataArr = rowToanTinh.DataArr;
                }
                else if (rowToanHuyen.DataArr != null && rowToanHuyen.DataArr.Count > 0)
                {
                    rowTong.DataArr = rowToanHuyen.DataArr;
                }
                if (rowTong.DataArr != null)
                {
                    rowTong.DataArr[0].Content = "Tổng";
                    data.Add(rowTong);
                }
                #endregion
                string path = @"Templates\FileTam\XLD04_" + CanBoDangNhapID + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";
                string urlExcel = new XLD04DAL().XLD04_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                Result.Status = 1;
                Result.Data = urlExcel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel XLD04_GetDSChiTietDonThu(string ContentRootPath, BaseReportParams p)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();

                var listTableHeader = new List<TableHeader>();
                TableHeader HeaderCol1 = new TableHeader(1, 0, "STT", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Số đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Ngày tiếp nhận", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Chủ đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol5 = new TableHeader(5, 0, "Địa chỉ", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol6 = new TableHeader(6, 0, "Nội dung đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol7 = new TableHeader(7, 0, "Loại đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol8 = new TableHeader(8, 0, "Kết quả xử lý", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol9 = new TableHeader(9, 0, "Kết quả giải quyết", "font-weight:bold", ref listTableHeader);
                BaoCaoModel.DataTable.TableHeader = listTableHeader;

                List<TKDonThuInfo> tKDonThuInfos = new List<TKDonThuInfo>();
                tKDonThuInfos = new XLD04DAL().XLD04_GetDSChiTietDonThu(ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now, p.Offset, p.PageSize, p.Index, p.XemTaiLieuMat, p.CanBoID, p.CoQuanID ?? 0, p.CapID);
                if (tKDonThuInfos.Count > 0)
                {
                    List<TableData> data = new List<TableData>();
                    int stt = p.Offset;
                    foreach (var item in tKDonThuInfos)
                    {
                        TableData tableData = new TableData();
                        tableData.ID = stt++;
                        tableData.DonThuID = item.DonThuID;
                        tableData.XuLyDonID = item.XuLyDonID;

                        var DataArr = new List<RowItem>();
                        RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "", ref DataArr);
                        RowItem RowItem2 = new RowItem(2, item.SoDon, "", "", null, "", ref DataArr);
                        RowItem RowItem3 = new RowItem(3, item.NgayNhapDonStr, "", "", null, "", ref DataArr);
                        RowItem RowItem4 = new RowItem(4, item.TenChuDon, "", "", null, "", ref DataArr);
                        RowItem RowItem5 = new RowItem(5, item.DiaChi, "", "", null, "", ref DataArr);
                        RowItem RowItem6 = new RowItem(6, item.NoiDungDon, "", "", null, "", ref DataArr);
                        RowItem RowItem7 = new RowItem(7, item.TenLoaiKhieuTo, "", "", null, "", ref DataArr);
                        RowItem RowItem8 = new RowItem(8, item.TenHuongGiaiQuyet, "", "", null, "", ref DataArr);
                        RowItem RowItem9 = new RowItem(9, item.KetQuaID_Str, "", "", null, "", ref DataArr);

                        tableData.DataArr = DataArr;
                        data.Add(tableData);
                    }
                    BaoCaoModel.DataTable.TableData = data;
                }

                Result.Status = 1;
                Result.Data = BaoCaoModel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel XLD04_GetDSChiTietDonThu_XuatExcel(BaseReportParams p, string ContentRootPath, int CanBoDangNhapID)
        {
            var Result = new BaseResultModel();
            try
            {
                List<TableData> data = new List<TableData>();
                List<TKDonThuInfo> tKDonThuInfos = new XLD04DAL().XLD04_GetDSChiTietDonThu(ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now, p.Offset, p.PageSize, p.Index, p.XemTaiLieuMat, p.CanBoID, p.CoQuanID ?? 0, p.CapID);
                if (tKDonThuInfos.Count > 0)
                {
                    int stt = p.Offset;
                    foreach (var item in tKDonThuInfos)
                    {
                        TableData tableData = new TableData();
                        tableData.ID = stt++;
                        tableData.DonThuID = item.DonThuID;
                        tableData.XuLyDonID = item.XuLyDonID;

                        var DataArr = new List<RowItem>();
                        RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "", ref DataArr);
                        RowItem RowItem2 = new RowItem(2, item.SoDon, "", "", null, "", ref DataArr);
                        RowItem RowItem3 = new RowItem(3, item.NgayNhapDonStr, "", "", null, "", ref DataArr);
                        RowItem RowItem4 = new RowItem(4, item.TenChuDon, "", "", null, "", ref DataArr);
                        RowItem RowItem5 = new RowItem(5, item.DiaChi, "", "", null, "", ref DataArr);
                        RowItem RowItem6 = new RowItem(6, item.NoiDungDon, "", "", null, "", ref DataArr);
                        RowItem RowItem7 = new RowItem(7, item.TenLoaiKhieuTo, "", "", null, "", ref DataArr);
                        RowItem RowItem8 = new RowItem(8, item.TenHuongGiaiQuyet, "", "", null, "", ref DataArr);
                        RowItem RowItem9 = new RowItem(9, item.KetQuaID_Str, "", "", null, "", ref DataArr);

                        tableData.DataArr = DataArr;
                        data.Add(tableData);
                    }
                }
                string path = @"Templates\FileTam\XLD04_DSChiTietDonThu" + CanBoDangNhapID + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";
                string urlExcel = new TCD01DAL().DSChiTietDonThu_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                Result.Status = 1;
                Result.Data = urlExcel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel KQGQ01(BaseReportParams p, string ContentRootPath, int RoleID, int CapID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, int HuyenDangNhapID)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.BieuSo = "Biểu số: 01/KQGQ";
                BaoCaoModel.ThongTinSoLieu = "Số liệu tính từ ngày " + (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy") + " đến ngày " + (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.TuNgay = (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.DenNgay = (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.Title = "TỔNG HỢP KẾT QUẢ GIẢI QUYẾT THUỘC THẨM QUYỀN";
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();
                #region Header
                var listTableHeader = new List<TableHeader>();
                //Cấp 1
                TableHeader HeaderCol1 = new TableHeader(1, 0, "Đơn vị", "width: 215px", ref listTableHeader);
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Đơn khiếu nại thuộc thẩm quyền", "", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Tổng số vụ việc khiếu nại thuộc thẩm quyền", "", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Kết quả giải quyết", "", ref listTableHeader);
                TableHeader HeaderCol5 = new TableHeader(5, 0, "Phân tích kết quả giải quyết (vụ việc)", "", ref listTableHeader);
                //Cấp 2
                var DataChild2 = new List<TableHeader>();
                TableHeader HeaderCol21 = new TableHeader(21, 2, "Tổng số", "", ref DataChild2);
                TableHeader HeaderCol22 = new TableHeader(22, 2, "Đơn kỳ trước chuyển sang", "", ref DataChild2);
                TableHeader HeaderCol23 = new TableHeader(23, 2, "Đơn tiếp nhận trong kỳ", "", ref DataChild2);
                HeaderCol2.DataChild = DataChild2;

                var DataChild4 = new List<TableHeader>();
                TableHeader HeaderCol41 = new TableHeader(41, 4, "Đã giải quyết", "", ref DataChild4);
                TableHeader HeaderCol42 = new TableHeader(42, 4, "Kiến nghị thu hồi cho NN", "", ref DataChild4);
                TableHeader HeaderCol43 = new TableHeader(43, 4, "Trả lại cho tổ chức, cá nhân", "", ref DataChild4);
                TableHeader HeaderCol44 = new TableHeader(44, 4, "Kiến nghị xử lý hành chính", "", ref DataChild4);
                TableHeader HeaderCol45 = new TableHeader(45, 4, "Chuyển cơ quan điều tra", "", ref DataChild4);
                HeaderCol4.DataChild = DataChild4;

                var DataChild5 = new List<TableHeader>();
                TableHeader HeaderCol51 = new TableHeader(51, 5, "Giải quyết lần đầu", "", ref DataChild5);
                TableHeader HeaderCol52 = new TableHeader(52, 5, "Giải quyết lần 2", "", ref DataChild5);
                TableHeader HeaderCol53 = new TableHeader(53, 5, "Chấp hành thời hạn giải quyết", "", ref DataChild5);
                HeaderCol5.DataChild = DataChild5;
                //Cấp 3
                var DataChild41 = new List<TableHeader>();
                TableHeader HeaderCol411 = new TableHeader(411, 41, "Số vụ việc giải quyết bằng QĐ hành chính", "", ref DataChild41);
                TableHeader HeaderCol412 = new TableHeader(412, 41, "Số vụ việc rút đơn thông qua giải thích, thuyết phục", "", ref DataChild41);
                HeaderCol41.DataChild = DataChild41;

                var DataChild42 = new List<TableHeader>();
                TableHeader HeaderCol421 = new TableHeader(421, 42, "Tiền (Trđ)", "", ref DataChild42);
                TableHeader HeaderCol422 = new TableHeader(422, 42, "Đất (m2)", "", ref DataChild42);
                HeaderCol42.DataChild = DataChild42;

                var DataChild43 = new List<TableHeader>();
                TableHeader HeaderCol431 = new TableHeader(431, 43, "Tổ chức", "", ref DataChild43);
                TableHeader HeaderCol432 = new TableHeader(432, 43, "Cá nhân", "", ref DataChild43);
                TableHeader HeaderCol433 = new TableHeader(433, 43, "Số tổ chức được trả lại quyền lợi", "", ref DataChild43);
                TableHeader HeaderCol434 = new TableHeader(434, 43, "Số cá nhân được trả lại quyền lợi", "", ref DataChild43);
                HeaderCol43.DataChild = DataChild43;

                var DataChild44 = new List<TableHeader>();
                TableHeader HeaderCol441 = new TableHeader(441, 44, "Tổng số người bị kiến nghị xử lý", "", ref DataChild44);
                TableHeader HeaderCol442 = new TableHeader(442, 44, "Trong đó số cán bộ, công chức, viên chức", "", ref DataChild44);
                HeaderCol44.DataChild = DataChild44;

                var DataChild45 = new List<TableHeader>();
                TableHeader HeaderCol451 = new TableHeader(451, 45, "Số vụ", "", ref DataChild45);
                TableHeader HeaderCol452 = new TableHeader(452, 45, "Tổng số người", "", ref DataChild45);
                TableHeader HeaderCol453 = new TableHeader(453, 45, "Trong đó số cán bộ, công chức, viên chức", "", ref DataChild45);
                HeaderCol45.DataChild = DataChild45;

                var DataChild51 = new List<TableHeader>();
                TableHeader HeaderCol511 = new TableHeader(511, 51, "Khiếu nại đúng", "", ref DataChild51);
                TableHeader HeaderCol512 = new TableHeader(512, 51, "Khiếu nại sai", "", ref DataChild51);
                TableHeader HeaderCol513 = new TableHeader(513, 51, "Khiếu nại đúng một phần", "", ref DataChild51);
                HeaderCol51.DataChild = DataChild51;

                var DataChild52 = new List<TableHeader>();
                TableHeader HeaderCol521 = new TableHeader(521, 52, "Công nhận QĐ g/q lần đẩu", "", ref DataChild52);
                TableHeader HeaderCol522 = new TableHeader(522, 52, "Hủy, sửa QĐ g/q lần đầu", "", ref DataChild52);
                HeaderCol52.DataChild = DataChild52;

                var DataChild53 = new List<TableHeader>();
                TableHeader HeaderCol531 = new TableHeader(531, 53, "Đúng quy định", "", ref DataChild53);
                TableHeader HeaderCol532 = new TableHeader(532, 53, "Không đúng quy định", "", ref DataChild53);
                HeaderCol53.DataChild = DataChild53;

                var DataChild431 = new List<TableHeader>();
                TableHeader HeaderCol4311 = new TableHeader(4311, 431, "Tiền (Trđ)", "", ref DataChild431);
                TableHeader HeaderCol4312 = new TableHeader(4312, 431, "Đất (m2)", "", ref DataChild431);
                HeaderCol431.DataChild = DataChild431;

                var DataChild432 = new List<TableHeader>();
                TableHeader HeaderCol4321 = new TableHeader(4321, 432, "Tiền (Trđ)", "", ref DataChild432);
                TableHeader HeaderCol4322 = new TableHeader(4322, 432, "Đất (m2)", "", ref DataChild432);
                HeaderCol432.DataChild = DataChild432;

                BaoCaoModel.DataTable.TableHeader = listTableHeader;
                #endregion
                #region TableData 
                TableData Row1 = new TableData();
                Row1.ID = 1;
                Row1.isClick = false;
                var DataArr = new List<RowItem>();
                RowItem RowItem1 = new RowItem(1, "Ms", "", "", null, "width: 215px", ref DataArr);
                RowItem RowItem2 = new RowItem(2, "1=2+3", "", "", null, "text-align: center;width: 250px", ref DataArr);
                RowItem RowItem3 = new RowItem(3, "2", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem4 = new RowItem(4, "3", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem5 = new RowItem(5, "4", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem6 = new RowItem(6, "5=20+..+24", "", "", null, "text-align: center;width: 250px", ref DataArr);
                RowItem RowItem7 = new RowItem(7, "6", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem8 = new RowItem(8, "7", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem9 = new RowItem(9, "8", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem10 = new RowItem(10, "9", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem11 = new RowItem(11, "10", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem12 = new RowItem(12, "11", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem13 = new RowItem(13, "12", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem14 = new RowItem(14, "13", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem15 = new RowItem(15, "14", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem16 = new RowItem(16, "15", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem17 = new RowItem(17, "16", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem18 = new RowItem(18, "17", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem19 = new RowItem(19, "18", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem20 = new RowItem(20, "19", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem21 = new RowItem(21, "20", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem22 = new RowItem(22, "21", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem23 = new RowItem(23, "22", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem24 = new RowItem(24, "23", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem25 = new RowItem(25, "24", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem26 = new RowItem(26, "25", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem27 = new RowItem(27, "26", "", "", null, "text-align: center;width: 100px", ref DataArr);

                Row1.DataArr = DataArr;
                BaoCaoModel.DataTable.TableData.Add(Row1);

                List<TableData> data = new List<TableData>();
                data = new KQGQ01DAL().KQGQ01(p.ListCapID, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                BaoCaoModel.DataTable.TableData.AddRange(data);

                #endregion

                Result.Status = 1;
                Result.Data = BaoCaoModel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel KQGQ01_XuatExcel(BaseReportParams p, string ContentRootPath, int RoleID, int CapID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, int HuyenDangNhapID)
        {
            var Result = new BaseResultModel();
            try
            {
                List<TableData> data = new List<TableData>();
                data = new KQGQ01DAL().KQGQ01(p.ListCapID, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                #region Thêm hàng Tổng
                TableData rowToanTinh = new TableData();
                TableData rowToanHuyen = new TableData();
                TableData rowTong = new TableData();
                if (data.Count > 1)
                {
                    foreach (var item in data)
                    {
                        if (item.DataArr != null && item.DataArr.Count > 0)
                        {
                            foreach (var col in item.DataArr)
                            {
                                if (col.CapID == CapCoQuanViewChiTiet.ToanTinh.GetHashCode().ToString())
                                {
                                    rowToanTinh = Utils.DeepCopy(item);
                                    break;
                                }
                                else if (col.CapID == CapCoQuanViewChiTiet.ToanHuyen.GetHashCode().ToString())
                                {
                                    rowToanHuyen = Utils.DeepCopy(item);
                                    break;
                                }
                            }
                        }
                    }
                }
                else if (data.Count == 1)
                {
                    rowTong = Utils.DeepCopy(data[0]);
                }

                if (rowToanTinh.DataArr != null && rowToanTinh.DataArr.Count > 0)
                {
                    rowTong.DataArr = rowToanTinh.DataArr;
                }
                else if (rowToanHuyen.DataArr != null && rowToanHuyen.DataArr.Count > 0)
                {
                    rowTong.DataArr = rowToanHuyen.DataArr;
                }
                if (rowTong.DataArr != null)
                {
                    rowTong.DataArr[0].Content = "Tổng";
                    data.Add(rowTong);
                }
                #endregion
                string path = @"Templates\FileTam\KQGQ01_" + CanBoDangNhapID + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";
                string urlExcel = new KQGQ01DAL().KQGQ01_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                Result.Status = 1;
                Result.Data = urlExcel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel KQGQ01_GetDSChiTietDonThu(string ContentRootPath, BaseReportParams p)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();

                var listTableHeader = new List<TableHeader>();
                TableHeader HeaderCol1 = new TableHeader(1, 0, "STT", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Số đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Ngày tiếp nhận", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Chủ đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol5 = new TableHeader(5, 0, "Địa chỉ", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol6 = new TableHeader(6, 0, "Nội dung đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol7 = new TableHeader(7, 0, "Loại đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol8 = new TableHeader(8, 0, "Kết quả xử lý", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol9 = new TableHeader(9, 0, "Kết quả giải quyết", "font-weight:bold", ref listTableHeader);
                BaoCaoModel.DataTable.TableHeader = listTableHeader;

                List<TKDonThuInfo> tKDonThuInfos = new List<TKDonThuInfo>();
                tKDonThuInfos = new KQGQ01DAL().KQGQ01_GetDSChiTietDonThu(ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now, p.Offset, p.PageSize, p.Index, p.XemTaiLieuMat, p.CanBoID, p.CoQuanID ?? 0, p.CapID);
                if (tKDonThuInfos.Count > 0)
                {
                    List<TableData> data = new List<TableData>();
                    int stt = p.Offset;
                    foreach (var item in tKDonThuInfos)
                    {
                        TableData tableData = new TableData();
                        tableData.ID = stt++;
                        tableData.DonThuID = item.DonThuID;
                        tableData.XuLyDonID = item.XuLyDonID;

                        var DataArr = new List<RowItem>();
                        RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "", ref DataArr);
                        RowItem RowItem2 = new RowItem(2, item.SoDon, "", "", null, "", ref DataArr);
                        RowItem RowItem3 = new RowItem(3, item.NgayNhapDonStr, "", "", null, "", ref DataArr);
                        RowItem RowItem4 = new RowItem(4, item.TenChuDon, "", "", null, "", ref DataArr);
                        RowItem RowItem5 = new RowItem(5, item.DiaChi, "", "", null, "", ref DataArr);
                        RowItem RowItem6 = new RowItem(6, item.NoiDungDon, "", "", null, "", ref DataArr);
                        RowItem RowItem7 = new RowItem(7, item.TenLoaiKhieuTo, "", "", null, "", ref DataArr);
                        RowItem RowItem8 = new RowItem(8, item.TenHuongGiaiQuyet, "", "", null, "", ref DataArr);
                        RowItem RowItem9 = new RowItem(9, item.KetQuaID_Str, "", "", null, "", ref DataArr);

                        tableData.DataArr = DataArr;
                        data.Add(tableData);
                    }
                    BaoCaoModel.DataTable.TableData = data;
                }

                Result.Status = 1;
                Result.Data = BaoCaoModel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel KQGQ01_GetDSChiTietDonThu_XuatExcel(BaseReportParams p, string ContentRootPath, int CanBoDangNhapID)
        {
            var Result = new BaseResultModel();
            try
            {
                List<TableData> data = new List<TableData>();
                List<TKDonThuInfo> tKDonThuInfos = new KQGQ01DAL().KQGQ01_GetDSChiTietDonThu(ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now, p.Offset, p.PageSize, p.Index, p.XemTaiLieuMat, p.CanBoID, p.CoQuanID ?? 0, p.CapID);
                if (tKDonThuInfos.Count > 0)
                {
                    int stt = p.Offset;
                    foreach (var item in tKDonThuInfos)
                    {
                        TableData tableData = new TableData();
                        tableData.ID = stt++;
                        tableData.DonThuID = item.DonThuID;
                        tableData.XuLyDonID = item.XuLyDonID;

                        var DataArr = new List<RowItem>();
                        RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "", ref DataArr);
                        RowItem RowItem2 = new RowItem(2, item.SoDon, "", "", null, "", ref DataArr);
                        RowItem RowItem3 = new RowItem(3, item.NgayNhapDonStr, "", "", null, "", ref DataArr);
                        RowItem RowItem4 = new RowItem(4, item.TenChuDon, "", "", null, "", ref DataArr);
                        RowItem RowItem5 = new RowItem(5, item.DiaChi, "", "", null, "", ref DataArr);
                        RowItem RowItem6 = new RowItem(6, item.NoiDungDon, "", "", null, "", ref DataArr);
                        RowItem RowItem7 = new RowItem(7, item.TenLoaiKhieuTo, "", "", null, "", ref DataArr);
                        RowItem RowItem8 = new RowItem(8, item.TenHuongGiaiQuyet, "", "", null, "", ref DataArr);
                        RowItem RowItem9 = new RowItem(9, item.KetQuaID_Str, "", "", null, "", ref DataArr);

                        tableData.DataArr = DataArr;
                        data.Add(tableData);
                    }
                }
                string path = @"Templates\FileTam\KQGQ01_DSChiTietDonThu" + CanBoDangNhapID + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";
                string urlExcel = new TCD01DAL().DSChiTietDonThu_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                Result.Status = 1;
                Result.Data = urlExcel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel KQGQ02(BaseReportParams p, string ContentRootPath, int RoleID, int CapID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, int HuyenDangNhapID)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.BieuSo = "Biểu số: 02/KQGQ";
                BaoCaoModel.ThongTinSoLieu = "Số liệu tính từ ngày " + (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy") + " đến ngày " + (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.TuNgay = (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.DenNgay = (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.Title = "TỔNG HỢP KẾT QUẢ THI HÀNH QUYẾT ĐỊNH GIẢI QUYẾT KHIẾU NẠI";
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();
                #region Header
                var listTableHeader = new List<TableHeader>();
                //Cấp 1
                TableHeader HeaderCol1 = new TableHeader(1, 0, "Đơn vị", "width: 215px", ref listTableHeader);
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Tổng số quyết định phải thực hiện trong kỳ", "", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Số quyết định đã thực hiện xong", "", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Thu hồi cho nhà nước", "", ref listTableHeader);
                TableHeader HeaderCol5 = new TableHeader(5, 0, "Trả lại cho tổ chức, cá nhân", "", ref listTableHeader);
                TableHeader HeaderCol6 = new TableHeader(6, 0, "Đã xử lý hành chính", "", ref listTableHeader);
                TableHeader HeaderCol7 = new TableHeader(7, 0, "Đã khởi tố", "", ref listTableHeader);
                //Cấp 2
                var DataChild4 = new List<TableHeader>();
                TableHeader HeaderCol41 = new TableHeader(41, 4, "Phải thu", "", ref DataChild4);
                TableHeader HeaderCol42 = new TableHeader(42, 4, "Đã thu", "", ref DataChild4);      
                HeaderCol4.DataChild = DataChild4;

                var DataChild5 = new List<TableHeader>();
                TableHeader HeaderCol51 = new TableHeader(51, 5, "Phải trả", "", ref DataChild5);
                TableHeader HeaderCol52 = new TableHeader(52, 5, "Đã trả", "", ref DataChild5);
                HeaderCol5.DataChild = DataChild5;

                var DataChild6 = new List<TableHeader>();
                TableHeader HeaderCol61 = new TableHeader(61, 6, "Tổng số người bị xử lý", "", ref DataChild6);
                TableHeader HeaderCol62 = new TableHeader(62, 6, "Trong đó số cán bộ, công chức, viên chức", "", ref DataChild6);
                HeaderCol6.DataChild = DataChild6;

                var DataChild7 = new List<TableHeader>();
                TableHeader HeaderCol71 = new TableHeader(71, 7, "Số vụ", "", ref DataChild7);
                TableHeader HeaderCol72 = new TableHeader(72, 7, "Số người", "", ref DataChild7);
                TableHeader HeaderCol73 = new TableHeader(73, 7, "Trong đó số cán bộ, công chức, viên chức", "", ref DataChild7);
                HeaderCol7.DataChild = DataChild7;
                //Cấp 3
                var DataChild41 = new List<TableHeader>();
                TableHeader HeaderCol411 = new TableHeader(411, 41, "Tiền (Trđ)", "", ref DataChild41);
                TableHeader HeaderCol412 = new TableHeader(412, 41, "Đất (m2)", "", ref DataChild41);
                HeaderCol41.DataChild = DataChild41;

                var DataChild42 = new List<TableHeader>();
                TableHeader HeaderCol421 = new TableHeader(421, 42, "Tiền (Trđ)", "", ref DataChild42);
                TableHeader HeaderCol422 = new TableHeader(422, 42, "Đất (m2)", "", ref DataChild42);
                HeaderCol42.DataChild = DataChild42;

                var DataChild51 = new List<TableHeader>();
                TableHeader HeaderCol511 = new TableHeader(511, 51, "Tổ chức", "", ref DataChild51);
                TableHeader HeaderCol512 = new TableHeader(512, 51, "Cá nhân", "", ref DataChild51);
                HeaderCol51.DataChild = DataChild51;

                var DataChild52 = new List<TableHeader>();
                TableHeader HeaderCol521 = new TableHeader(521, 52, "Tổ chức", "", ref DataChild52);
                TableHeader HeaderCol522 = new TableHeader(522, 52, "Cá nhân", "", ref DataChild52);
                HeaderCol52.DataChild = DataChild52;
                //Cấp 4
                var DataChild511 = new List<TableHeader>();
                TableHeader HeaderCol5111 = new TableHeader(5111, 511, "Tiền (Trđ)", "", ref DataChild511);
                TableHeader HeaderCol5112 = new TableHeader(5112, 511, "Đất (m2)", "", ref DataChild511);
                HeaderCol511.DataChild = DataChild511;

                var DataChild512 = new List<TableHeader>();
                TableHeader HeaderCol5121 = new TableHeader(5121, 512, "Tiền (Trđ)", "", ref DataChild512);
                TableHeader HeaderCol5122 = new TableHeader(5122, 512, "Đất (m2)", "", ref DataChild512);
                HeaderCol512.DataChild = DataChild512;

                var DataChild521 = new List<TableHeader>();
                TableHeader HeaderCol5211 = new TableHeader(5211, 521, "Tiền (Trđ)", "", ref DataChild521);
                TableHeader HeaderCol5212 = new TableHeader(5212, 521, "Đất (m2)", "", ref DataChild521);
                HeaderCol521.DataChild = DataChild521;

                var DataChild522 = new List<TableHeader>();
                TableHeader HeaderCol5221 = new TableHeader(5221, 522, "Tiền (Trđ)", "", ref DataChild522);
                TableHeader HeaderCol5222 = new TableHeader(5222, 522, "Đất (m2)", "", ref DataChild522);
                HeaderCol522.DataChild = DataChild522;

                BaoCaoModel.DataTable.TableHeader = listTableHeader;
                #endregion
                #region TableData 
                TableData Row1 = new TableData();
                Row1.ID = 1;
                Row1.isClick = false;
                var DataArr = new List<RowItem>();
                RowItem RowItem1 = new RowItem(1, "Ms", "", "", null, "width: 215px", ref DataArr);
                RowItem RowItem2 = new RowItem(2, "1", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem3 = new RowItem(3, "2", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem4 = new RowItem(4, "3", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem5 = new RowItem(5, "4", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem6 = new RowItem(6, "5", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem7 = new RowItem(7, "6", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem8 = new RowItem(8, "7", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem9 = new RowItem(9, "8", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem10 = new RowItem(10, "9", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem11 = new RowItem(11, "10", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem12 = new RowItem(12, "11", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem13 = new RowItem(13, "12", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem14 = new RowItem(14, "13", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem15 = new RowItem(15, "14", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem16 = new RowItem(16, "15", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem17 = new RowItem(17, "16", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem18 = new RowItem(18, "17", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem19 = new RowItem(19, "18", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem20 = new RowItem(20, "19", "", "", null, "text-align: center;width: 100px", ref DataArr);

                Row1.DataArr = DataArr;
                BaoCaoModel.DataTable.TableData.Add(Row1);

                List<TableData> data = new List<TableData>();
                data = new KQGQ02DAL().KQGQ02(p.ListCapID, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                BaoCaoModel.DataTable.TableData.AddRange(data);

                #endregion

                Result.Status = 1;
                Result.Data = BaoCaoModel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel KQGQ02_XuatExcel(BaseReportParams p, string ContentRootPath, int RoleID, int CapID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, int HuyenDangNhapID)
        {
            var Result = new BaseResultModel();
            try
            {
                List<TableData> data = new List<TableData>();
                data = new KQGQ02DAL().KQGQ02(p.ListCapID, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                #region Thêm hàng Tổng
                TableData rowToanTinh = new TableData();
                TableData rowToanHuyen = new TableData();
                TableData rowTong = new TableData();
                if (data.Count > 1)
                {
                    foreach (var item in data)
                    {
                        if (item.DataArr != null && item.DataArr.Count > 0)
                        {
                            foreach (var col in item.DataArr)
                            {
                                if (col.CapID == CapCoQuanViewChiTiet.ToanTinh.GetHashCode().ToString())
                                {
                                    rowToanTinh = Utils.DeepCopy(item);
                                    break;
                                }
                                else if (col.CapID == CapCoQuanViewChiTiet.ToanHuyen.GetHashCode().ToString())
                                {
                                    rowToanHuyen = Utils.DeepCopy(item);
                                    break;
                                }
                            }
                        }
                    }
                }
                else if (data.Count == 1)
                {
                    rowTong = Utils.DeepCopy(data[0]);
                }

                if (rowToanTinh.DataArr != null && rowToanTinh.DataArr.Count > 0)
                {
                    rowTong.DataArr = rowToanTinh.DataArr;
                }
                else if (rowToanHuyen.DataArr != null && rowToanHuyen.DataArr.Count > 0)
                {
                    rowTong.DataArr = rowToanHuyen.DataArr;
                }
                if (rowTong.DataArr != null)
                {
                    rowTong.DataArr[0].Content = "Tổng";
                    data.Add(rowTong);
                }
                #endregion
                string path = @"Templates\FileTam\KQGQ02_" + CanBoDangNhapID + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";
                string urlExcel = new KQGQ02DAL().KQGQ02_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                Result.Status = 1;
                Result.Data = urlExcel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel KQGQ02_GetDSChiTietDonThu(string ContentRootPath, BaseReportParams p)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();

                var listTableHeader = new List<TableHeader>();
                TableHeader HeaderCol1 = new TableHeader(1, 0, "STT", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Số đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Ngày tiếp nhận", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Chủ đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol5 = new TableHeader(5, 0, "Địa chỉ", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol6 = new TableHeader(6, 0, "Nội dung đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol7 = new TableHeader(7, 0, "Loại đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol8 = new TableHeader(8, 0, "Kết quả xử lý", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol9 = new TableHeader(9, 0, "Kết quả giải quyết", "font-weight:bold", ref listTableHeader);
                BaoCaoModel.DataTable.TableHeader = listTableHeader;

                List<TKDonThuInfo> tKDonThuInfos = new List<TKDonThuInfo>();
                tKDonThuInfos = new KQGQ02DAL().KQGQ02_GetDSChiTietDonThu(ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now, p.Offset, p.PageSize, p.Index, p.XemTaiLieuMat, p.CanBoID, p.CoQuanID ?? 0, p.CapID);
                if (tKDonThuInfos.Count > 0)
                {
                    List<TableData> data = new List<TableData>();
                    int stt = p.Offset;
                    foreach (var item in tKDonThuInfos)
                    {
                        TableData tableData = new TableData();
                        tableData.ID = stt++;
                        tableData.DonThuID = item.DonThuID;
                        tableData.XuLyDonID = item.XuLyDonID;

                        var DataArr = new List<RowItem>();
                        RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "", ref DataArr);
                        RowItem RowItem2 = new RowItem(2, item.SoDon, "", "", null, "", ref DataArr);
                        RowItem RowItem3 = new RowItem(3, item.NgayNhapDonStr, "", "", null, "", ref DataArr);
                        RowItem RowItem4 = new RowItem(4, item.TenChuDon, "", "", null, "", ref DataArr);
                        RowItem RowItem5 = new RowItem(5, item.DiaChi, "", "", null, "", ref DataArr);
                        RowItem RowItem6 = new RowItem(6, item.NoiDungDon, "", "", null, "", ref DataArr);
                        RowItem RowItem7 = new RowItem(7, item.TenLoaiKhieuTo, "", "", null, "", ref DataArr);
                        RowItem RowItem8 = new RowItem(8, item.TenHuongGiaiQuyet, "", "", null, "", ref DataArr);
                        RowItem RowItem9 = new RowItem(9, item.KetQuaID_Str, "", "", null, "", ref DataArr);

                        tableData.DataArr = DataArr;
                        data.Add(tableData);
                    }
                    BaoCaoModel.DataTable.TableData = data;
                }

                Result.Status = 1;
                Result.Data = BaoCaoModel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel KQGQ02_GetDSChiTietDonThu_XuatExcel(BaseReportParams p, string ContentRootPath, int CanBoDangNhapID)
        {
            var Result = new BaseResultModel();
            try
            {
                List<TableData> data = new List<TableData>();
                List<TKDonThuInfo> tKDonThuInfos = new KQGQ02DAL().KQGQ02_GetDSChiTietDonThu(ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now, p.Offset, p.PageSize, p.Index, p.XemTaiLieuMat, p.CanBoID, p.CoQuanID ?? 0, p.CapID);
                if (tKDonThuInfos.Count > 0)
                {
                    int stt = p.Offset;
                    foreach (var item in tKDonThuInfos)
                    {
                        TableData tableData = new TableData();
                        tableData.ID = stt++;
                        tableData.DonThuID = item.DonThuID;
                        tableData.XuLyDonID = item.XuLyDonID;

                        var DataArr = new List<RowItem>();
                        RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "", ref DataArr);
                        RowItem RowItem2 = new RowItem(2, item.SoDon, "", "", null, "", ref DataArr);
                        RowItem RowItem3 = new RowItem(3, item.NgayNhapDonStr, "", "", null, "", ref DataArr);
                        RowItem RowItem4 = new RowItem(4, item.TenChuDon, "", "", null, "", ref DataArr);
                        RowItem RowItem5 = new RowItem(5, item.DiaChi, "", "", null, "", ref DataArr);
                        RowItem RowItem6 = new RowItem(6, item.NoiDungDon, "", "", null, "", ref DataArr);
                        RowItem RowItem7 = new RowItem(7, item.TenLoaiKhieuTo, "", "", null, "", ref DataArr);
                        RowItem RowItem8 = new RowItem(8, item.TenHuongGiaiQuyet, "", "", null, "", ref DataArr);
                        RowItem RowItem9 = new RowItem(9, item.KetQuaID_Str, "", "", null, "", ref DataArr);

                        tableData.DataArr = DataArr;
                        data.Add(tableData);
                    }
                }
                string path = @"Templates\FileTam\KQGQ02_DSChiTietDonThu" + CanBoDangNhapID + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";
                string urlExcel = new TCD01DAL().DSChiTietDonThu_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                Result.Status = 1;
                Result.Data = urlExcel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel KQGQ03(BaseReportParams p, string ContentRootPath, int RoleID, int CapID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, int HuyenDangNhapID)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.BieuSo = "Biểu số: 03/KQGQ";
                BaoCaoModel.ThongTinSoLieu = "Số liệu tính từ ngày " + (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy") + " đến ngày " + (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.TuNgay = (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.DenNgay = (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.Title = "TỔNG HỢP KẾT QUẢ GIẢI QUYẾT TỐ CÁO THUỘC THẨM QUYỀN";
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();
                #region Header
                var listTableHeader = new List<TableHeader>();
                //Cấp 1
                TableHeader HeaderCol1 = new TableHeader(1, 0, "Đơn vị", "width: 215px", ref listTableHeader);
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Đơn tố cáo thuộc thẩm quyền", "", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Tổng số vụ việc tố cáo thuộc thẩm quyền", "", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Trong đó số vụ việc tố cáo tiếp", "", ref listTableHeader);
                TableHeader HeaderCol5 = new TableHeader(5, 0, "Kết quả giải quyết", "", ref listTableHeader);
                TableHeader HeaderCol6 = new TableHeader(6, 0, "Phân tích kết quả giải quyết (vụ việc)", "", ref listTableHeader);
                TableHeader HeaderCol7 = new TableHeader(7, 0, "Chấp hành thời hạn giải quyết", "", ref listTableHeader);
                //Cấp 2
                var DataChild2 = new List<TableHeader>();
                TableHeader HeaderCol21 = new TableHeader(21, 2, "Tổng số", "", ref DataChild2);
                TableHeader HeaderCol22 = new TableHeader(22, 2, "Kỳ trước chuyển sang", "", ref DataChild2);
                TableHeader HeaderCol23 = new TableHeader(23, 2, "Tiếp nhận trong kỳ", "", ref DataChild2);
                HeaderCol2.DataChild = DataChild2;

                var DataChild5 = new List<TableHeader>();
                TableHeader HeaderCol51 = new TableHeader(51, 5, "Đã giải quyết", "", ref DataChild5);
                TableHeader HeaderCol52 = new TableHeader(52, 5, "Kiến nghị thu hồi cho NN", "", ref DataChild5);
                TableHeader HeaderCol53 = new TableHeader(53, 5, "Trả lại cho tổ chức, cá nhân", "", ref DataChild5);
                TableHeader HeaderCol54 = new TableHeader(54, 5, "Số tổ chức được trả lại quyền lợi", "", ref DataChild5);
                TableHeader HeaderCol55 = new TableHeader(55, 5, "Số cá nhân được trả lại quyền lợi", "", ref DataChild5);
                TableHeader HeaderCol56 = new TableHeader(56, 5, "Kiến nghị xử lý hành chính", "", ref DataChild5);
                TableHeader HeaderCol57 = new TableHeader(57, 5, "Chuyển cơ quan điều tra", "", ref DataChild5);          
                HeaderCol5.DataChild = DataChild5;

                var DataChild6 = new List<TableHeader>();
                TableHeader HeaderCol61 = new TableHeader(61, 6, "Tố cáo đúng", "", ref DataChild6);
                TableHeader HeaderCol62 = new TableHeader(62, 6, "Trong đó tố cáo tiếp đúng", "", ref DataChild6);
                TableHeader HeaderCol63 = new TableHeader(63, 6, "Tố cáo sai", "", ref DataChild6);
                TableHeader HeaderCol64 = new TableHeader(64, 6, "Trong đó tố cáo tiếp sai", "", ref DataChild6);
                TableHeader HeaderCol65 = new TableHeader(65, 6, "Tố cáo có đúng, có sai", "", ref DataChild6);
                TableHeader HeaderCol66 = new TableHeader(66, 6, "Trong đó tố cáo tiếp có đúng, có sai", "", ref DataChild6);
                HeaderCol6.DataChild = DataChild6;

                var DataChild7 = new List<TableHeader>();
                TableHeader HeaderCol71 = new TableHeader(71, 7, "Đúng quy định", "", ref DataChild7);
                TableHeader HeaderCol72 = new TableHeader(72, 7, "Không đúng quy định", "", ref DataChild7);
                HeaderCol7.DataChild = DataChild7;
                //Cấp 3
                var DataChild51 = new List<TableHeader>();
                TableHeader HeaderCol511 = new TableHeader(511, 51, "Tổng số", "", ref DataChild51);
                TableHeader HeaderCol512 = new TableHeader(512, 51, "Số vụ việc lần đầu", "", ref DataChild51);
                TableHeader HeaderCol513 = new TableHeader(513, 51, "Số vụ việc tố cáo tiếp", "", ref DataChild51);
                TableHeader HeaderCol514 = new TableHeader(514, 51, "Số vụ việc rút toàn bộ nội dung tố cáo", "", ref DataChild51);
                TableHeader HeaderCol515 = new TableHeader(515, 51, "Số vụ việc đình chỉ không do rút tố cáo", "", ref DataChild51);
                HeaderCol51.DataChild = DataChild51;

                var DataChild52 = new List<TableHeader>();
                TableHeader HeaderCol521 = new TableHeader(521, 52, "Tiền (Trđ)", "", ref DataChild52);
                TableHeader HeaderCol522 = new TableHeader(522, 52, "Đất (m2)", "", ref DataChild52);
                HeaderCol52.DataChild = DataChild52;

                var DataChild53 = new List<TableHeader>();
                TableHeader HeaderCol531 = new TableHeader(531, 53, "Tổ chức", "", ref DataChild53);
                TableHeader HeaderCol532 = new TableHeader(532, 53, "Cá nhân", "", ref DataChild53);
                HeaderCol53.DataChild = DataChild53;

                var DataChild56 = new List<TableHeader>();
                TableHeader HeaderCol561 = new TableHeader(561, 56, "Số người bị kiến nghị xử lý", "", ref DataChild56);
                TableHeader HeaderCol562 = new TableHeader(562, 56, "Trong đó số cán bộ, công chức, viên chức", "", ref DataChild56);
                HeaderCol56.DataChild = DataChild56;

                var DataChild57 = new List<TableHeader>();
                TableHeader HeaderCol571 = new TableHeader(571, 57, "Số vụ", "", ref DataChild57);
                TableHeader HeaderCol572 = new TableHeader(572, 57, "Số đối tượng", "", ref DataChild57);
                TableHeader HeaderCol573 = new TableHeader(573, 57, "Trong đó số cán bộ, công chức, viên chức", "", ref DataChild57);
                HeaderCol57.DataChild = DataChild57;
                //Cấp 4
                var DataChild531 = new List<TableHeader>();
                TableHeader HeaderCol5311 = new TableHeader(5311, 531, "Tiền (Trđ)", "", ref DataChild531);
                TableHeader HeaderCol5312 = new TableHeader(5312, 531, "Đất (m2)", "", ref DataChild531);
                HeaderCol531.DataChild = DataChild531;

                var DataChild532 = new List<TableHeader>();
                TableHeader HeaderCol5321 = new TableHeader(5321, 532, "Tiền (Trđ)", "", ref DataChild532);
                TableHeader HeaderCol5322 = new TableHeader(5322, 532, "Đất (m2)", "", ref DataChild532);
                HeaderCol532.DataChild = DataChild532;

                BaoCaoModel.DataTable.TableHeader = listTableHeader;
                #endregion
                #region TableData 
                TableData Row1 = new TableData();
                Row1.ID = 1;
                Row1.isClick = false;
                var DataArr = new List<RowItem>();
                RowItem RowItem1 = new RowItem(1, "Ms", "", "", null, "width: 215px", ref DataArr);
                RowItem RowItem2 = new RowItem(2, "1", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem3 = new RowItem(3, "2", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem4 = new RowItem(4, "3", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem5 = new RowItem(5, "4", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem6 = new RowItem(6, "5", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem7 = new RowItem(7, "6", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem8 = new RowItem(8, "7", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem9 = new RowItem(9, "8", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem10 = new RowItem(10, "9", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem11 = new RowItem(11, "10", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem12 = new RowItem(12, "11", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem13 = new RowItem(13, "12", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem14 = new RowItem(14, "13", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem15 = new RowItem(15, "14", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem16 = new RowItem(16, "15", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem17 = new RowItem(17, "16", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem18 = new RowItem(18, "17", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem19 = new RowItem(19, "18", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem20 = new RowItem(20, "19", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem21 = new RowItem(21, "20", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem22 = new RowItem(22, "21", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem23 = new RowItem(23, "22", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem24 = new RowItem(24, "23", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem25 = new RowItem(25, "24", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem26 = new RowItem(26, "25", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem27 = new RowItem(27, "26", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem28 = new RowItem(28, "27", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem29 = new RowItem(29, "28", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem30 = new RowItem(30, "29", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem31 = new RowItem(31, "30", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem32 = new RowItem(32, "31", "", "", null, "text-align: center;width: 100px", ref DataArr);

                Row1.DataArr = DataArr;
                BaoCaoModel.DataTable.TableData.Add(Row1);

                List<TableData> data = new List<TableData>();
                data = new KQGQ03DAL().KQGQ03(p.ListCapID, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                BaoCaoModel.DataTable.TableData.AddRange(data);

                #endregion

                Result.Status = 1;
                Result.Data = BaoCaoModel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel KQGQ03_XuatExcel(BaseReportParams p, string ContentRootPath, int RoleID, int CapID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, int HuyenDangNhapID)
        {
            var Result = new BaseResultModel();
            try
            {
                List<TableData> data = new List<TableData>();
                data = new KQGQ03DAL().KQGQ03(p.ListCapID, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                #region Thêm hàng Tổng
                TableData rowToanTinh = new TableData();
                TableData rowToanHuyen = new TableData();
                TableData rowTong = new TableData();
                if (data.Count > 1)
                {
                    foreach (var item in data)
                    {
                        if (item.DataArr != null && item.DataArr.Count > 0)
                        {
                            foreach (var col in item.DataArr)
                            {
                                if (col.CapID == CapCoQuanViewChiTiet.ToanTinh.GetHashCode().ToString())
                                {
                                    rowToanTinh = Utils.DeepCopy(item);
                                    break;
                                }
                                else if (col.CapID == CapCoQuanViewChiTiet.ToanHuyen.GetHashCode().ToString())
                                {
                                    rowToanHuyen = Utils.DeepCopy(item);
                                    break;
                                }
                            }
                        }
                    }
                }
                else if (data.Count == 1)
                {
                    rowTong = Utils.DeepCopy(data[0]);
                }

                if (rowToanTinh.DataArr != null && rowToanTinh.DataArr.Count > 0)
                {
                    rowTong.DataArr = rowToanTinh.DataArr;
                }
                else if (rowToanHuyen.DataArr != null && rowToanHuyen.DataArr.Count > 0)
                {
                    rowTong.DataArr = rowToanHuyen.DataArr;
                }
                if (rowTong.DataArr != null)
                {
                    rowTong.DataArr[0].Content = "Tổng";
                    data.Add(rowTong);
                }
                #endregion
                string path = @"Templates\FileTam\KQGQ03_" + CanBoDangNhapID + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";
                string urlExcel = new KQGQ03DAL().KQGQ03_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                Result.Status = 1;
                Result.Data = urlExcel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel KQGQ03_GetDSChiTietDonThu(string ContentRootPath, BaseReportParams p)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();

                var listTableHeader = new List<TableHeader>();
                TableHeader HeaderCol1 = new TableHeader(1, 0, "STT", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Số đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Ngày tiếp nhận", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Chủ đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol5 = new TableHeader(5, 0, "Địa chỉ", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol6 = new TableHeader(6, 0, "Nội dung đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol7 = new TableHeader(7, 0, "Loại đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol8 = new TableHeader(8, 0, "Kết quả xử lý", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol9 = new TableHeader(9, 0, "Kết quả giải quyết", "font-weight:bold", ref listTableHeader);
                BaoCaoModel.DataTable.TableHeader = listTableHeader;

                List<TKDonThuInfo> tKDonThuInfos = new List<TKDonThuInfo>();
                tKDonThuInfos = new KQGQ03DAL().KQGQ03_GetDSChiTietDonThu(ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now, p.Offset, p.PageSize, p.Index, p.XemTaiLieuMat, p.CanBoID, p.CoQuanID ?? 0, p.CapID);
                if (tKDonThuInfos.Count > 0)
                {
                    List<TableData> data = new List<TableData>();
                    int stt = p.Offset;
                    foreach (var item in tKDonThuInfos)
                    {
                        TableData tableData = new TableData();
                        tableData.ID = stt++;
                        tableData.DonThuID = item.DonThuID;
                        tableData.XuLyDonID = item.XuLyDonID;

                        var DataArr = new List<RowItem>();
                        RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "", ref DataArr);
                        RowItem RowItem2 = new RowItem(2, item.SoDon, "", "", null, "", ref DataArr);
                        RowItem RowItem3 = new RowItem(3, item.NgayNhapDonStr, "", "", null, "", ref DataArr);
                        RowItem RowItem4 = new RowItem(4, item.TenChuDon, "", "", null, "", ref DataArr);
                        RowItem RowItem5 = new RowItem(5, item.DiaChi, "", "", null, "", ref DataArr);
                        RowItem RowItem6 = new RowItem(6, item.NoiDungDon, "", "", null, "", ref DataArr);
                        RowItem RowItem7 = new RowItem(7, item.TenLoaiKhieuTo, "", "", null, "", ref DataArr);
                        RowItem RowItem8 = new RowItem(8, item.TenHuongGiaiQuyet, "", "", null, "", ref DataArr);
                        RowItem RowItem9 = new RowItem(9, item.KetQuaID_Str, "", "", null, "", ref DataArr);

                        tableData.DataArr = DataArr;
                        data.Add(tableData);
                    }
                    BaoCaoModel.DataTable.TableData = data;
                }

                Result.Status = 1;
                Result.Data = BaoCaoModel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel KQGQ03_GetDSChiTietDonThu_XuatExcel(BaseReportParams p, string ContentRootPath, int CanBoDangNhapID)
        {
            var Result = new BaseResultModel();
            try
            {
                List<TableData> data = new List<TableData>();
                List<TKDonThuInfo> tKDonThuInfos = new KQGQ03DAL().KQGQ03_GetDSChiTietDonThu(ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now, p.Offset, p.PageSize, p.Index, p.XemTaiLieuMat, p.CanBoID, p.CoQuanID ?? 0, p.CapID);
                if (tKDonThuInfos.Count > 0)
                {
                    int stt = p.Offset;
                    foreach (var item in tKDonThuInfos)
                    {
                        TableData tableData = new TableData();
                        tableData.ID = stt++;
                        tableData.DonThuID = item.DonThuID;
                        tableData.XuLyDonID = item.XuLyDonID;

                        var DataArr = new List<RowItem>();
                        RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "", ref DataArr);
                        RowItem RowItem2 = new RowItem(2, item.SoDon, "", "", null, "", ref DataArr);
                        RowItem RowItem3 = new RowItem(3, item.NgayNhapDonStr, "", "", null, "", ref DataArr);
                        RowItem RowItem4 = new RowItem(4, item.TenChuDon, "", "", null, "", ref DataArr);
                        RowItem RowItem5 = new RowItem(5, item.DiaChi, "", "", null, "", ref DataArr);
                        RowItem RowItem6 = new RowItem(6, item.NoiDungDon, "", "", null, "", ref DataArr);
                        RowItem RowItem7 = new RowItem(7, item.TenLoaiKhieuTo, "", "", null, "", ref DataArr);
                        RowItem RowItem8 = new RowItem(8, item.TenHuongGiaiQuyet, "", "", null, "", ref DataArr);
                        RowItem RowItem9 = new RowItem(9, item.KetQuaID_Str, "", "", null, "", ref DataArr);

                        tableData.DataArr = DataArr;
                        data.Add(tableData);
                    }
                }
                string path = @"Templates\FileTam\KQGQ03_DSChiTietDonThu" + CanBoDangNhapID + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";
                string urlExcel = new TCD01DAL().DSChiTietDonThu_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                Result.Status = 1;
                Result.Data = urlExcel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel KQGQ04(BaseReportParams p, string ContentRootPath, int RoleID, int CapID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, int HuyenDangNhapID)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.BieuSo = "Biểu số: 04/KQGQ";
                BaoCaoModel.ThongTinSoLieu = "Số liệu tính từ ngày " + (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy") + " đến ngày " + (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.TuNgay = (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.DenNgay = (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.Title = "TỔNG HỢP KẾT QUẢ THỰC HIỆN KẾT LUẬN NỘI DUNG TỐ CÁO ";
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();
                #region Header
                var listTableHeader = new List<TableHeader>();
                //Cấp 1
                TableHeader HeaderCol1 = new TableHeader(1, 0, "Đơn vị", "width: 215px", ref listTableHeader);
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Tổng số kết luận phải thực hiện", "", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Số kết luận đi thực hiện xong", "", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Thu hồi cho Nhà nước", "", ref listTableHeader);
                TableHeader HeaderCol5 = new TableHeader(5, 0, "Trả lại cho tổ chức, cá nhân", "", ref listTableHeader);
                TableHeader HeaderCol6 = new TableHeader(6, 0, "Đã xử lý hành chính", "", ref listTableHeader);
                TableHeader HeaderCol7 = new TableHeader(7, 0, "Đã khởi tố", "", ref listTableHeader);
                //Cấp 2
                var DataChild4 = new List<TableHeader>();
                TableHeader HeaderCol41 = new TableHeader(41, 4, "Phải thu", "", ref DataChild4);
                TableHeader HeaderCol42 = new TableHeader(42, 4, "Đã thu", "", ref DataChild4);
                HeaderCol4.DataChild = DataChild4;

                var DataChild5 = new List<TableHeader>();
                TableHeader HeaderCol51 = new TableHeader(51, 5, "Phải trả", "", ref DataChild5);
                TableHeader HeaderCol52 = new TableHeader(52, 5, "Đã trả", "", ref DataChild5); 
                HeaderCol5.DataChild = DataChild5;

                var DataChild6 = new List<TableHeader>();
                TableHeader HeaderCol61 = new TableHeader(61, 6, "Tổng số tổ chức bị xử lý", "", ref DataChild6);
                TableHeader HeaderCol62 = new TableHeader(62, 6, "Tổng số cá nhân bị xử lý", "", ref DataChild6);
                TableHeader HeaderCol63 = new TableHeader(63, 6, "Trong đó số cán bộ, công chức, viên chức", "", ref DataChild6); 
                HeaderCol6.DataChild = DataChild6;

                var DataChild7 = new List<TableHeader>();
                TableHeader HeaderCol71 = new TableHeader(71, 7, "Số vụ", "", ref DataChild7);
                TableHeader HeaderCol72 = new TableHeader(72, 7, "Số đối tượng", "", ref DataChild7);
                TableHeader HeaderCol73 = new TableHeader(73, 7, "Trong đó số cán bộ, công chức, viên chức", "", ref DataChild7);
                HeaderCol7.DataChild = DataChild7;
                //Cấp 3
                var DataChild41 = new List<TableHeader>();
                TableHeader HeaderCol411 = new TableHeader(411, 41, "Tiền (Trđ)", "", ref DataChild41);
                TableHeader HeaderCol412 = new TableHeader(412, 41, "Đất (m2)", "", ref DataChild41);           
                HeaderCol41.DataChild = DataChild41;

                var DataChild42 = new List<TableHeader>();
                TableHeader HeaderCol421 = new TableHeader(421, 42, "Tiền (Trđ)", "", ref DataChild42);
                TableHeader HeaderCol422 = new TableHeader(422, 42, "Đất (m2)", "", ref DataChild42);
                HeaderCol42.DataChild = DataChild42;

                var DataChild51 = new List<TableHeader>();
                TableHeader HeaderCol511 = new TableHeader(511, 51, "Số tổ chức phải được trả lại quyền lợi", "", ref DataChild51);
                TableHeader HeaderCol512 = new TableHeader(512, 51, "Số cá nhân phải được trả lại quyền lợi", "", ref DataChild51);
                TableHeader HeaderCol513 = new TableHeader(513, 51, "Tổ chức", "", ref DataChild51);
                TableHeader HeaderCol514 = new TableHeader(514, 51, "Cá nhân", "", ref DataChild51);
                HeaderCol51.DataChild = DataChild51;

                var DataChild52 = new List<TableHeader>();
                TableHeader HeaderCol521 = new TableHeader(521, 52, "Số tổ chức đã được trả lại quyền lợi", "", ref DataChild52);
                TableHeader HeaderCol522 = new TableHeader(522, 52, "Số cá nhân đã được trả lại quyền lợi", "", ref DataChild52);
                TableHeader HeaderCol523 = new TableHeader(523, 52, "Tổ chức", "", ref DataChild52);
                TableHeader HeaderCol524 = new TableHeader(524, 52, "Cá nhân", "", ref DataChild52);
                HeaderCol52.DataChild = DataChild52;
                //Cấp 4
                var DataChild513 = new List<TableHeader>();
                TableHeader HeaderCol5131 = new TableHeader(5131, 513, "Tiền (Trđ)", "", ref DataChild513);
                TableHeader HeaderCol5132 = new TableHeader(5132, 513, "Đất (m2)", "", ref DataChild513);
                HeaderCol513.DataChild = DataChild513;

                var DataChild514 = new List<TableHeader>();
                TableHeader HeaderCol5141 = new TableHeader(5141, 514, "Tiền (Trđ)", "", ref DataChild514);
                TableHeader HeaderCol5142 = new TableHeader(5142, 514, "Đất (m2)", "", ref DataChild514);
                HeaderCol514.DataChild = DataChild514;

                var DataChild523 = new List<TableHeader>();
                TableHeader HeaderCol5231 = new TableHeader(5231, 523, "Tiền (Trđ)", "", ref DataChild523);
                TableHeader HeaderCol5232 = new TableHeader(5232, 523, "Đất (m2)", "", ref DataChild523);
                HeaderCol523.DataChild = DataChild523;

                var DataChild524 = new List<TableHeader>();
                TableHeader HeaderCol5241 = new TableHeader(5241, 524, "Tiền (Trđ)", "", ref DataChild524);
                TableHeader HeaderCol5242 = new TableHeader(5242, 524, "Đất (m2)", "", ref DataChild524);
                HeaderCol524.DataChild = DataChild524;

                BaoCaoModel.DataTable.TableHeader = listTableHeader;
                #endregion
                #region TableData 
                TableData Row1 = new TableData();
                Row1.ID = 1;
                Row1.isClick = false;
                var DataArr = new List<RowItem>();
                RowItem RowItem1 = new RowItem(1, "Ms", "", "", null, "width: 215px", ref DataArr);
                RowItem RowItem2 = new RowItem(2, "1", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem3 = new RowItem(3, "2", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem4 = new RowItem(4, "3", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem5 = new RowItem(5, "4", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem6 = new RowItem(6, "5", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem7 = new RowItem(7, "6", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem8 = new RowItem(8, "7", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem9 = new RowItem(9, "8", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem10 = new RowItem(10, "9", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem11 = new RowItem(11, "10", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem12 = new RowItem(12, "11", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem13 = new RowItem(13, "12", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem14 = new RowItem(14, "13", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem15 = new RowItem(15, "14", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem16 = new RowItem(16, "15", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem17 = new RowItem(17, "16", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem18 = new RowItem(18, "17", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem19 = new RowItem(19, "18", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem20 = new RowItem(20, "19", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem21 = new RowItem(21, "20", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem22 = new RowItem(22, "21", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem23 = new RowItem(23, "22", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem24 = new RowItem(24, "23", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem25 = new RowItem(25, "24", "", "", null, "text-align: center;width: 100px", ref DataArr);
               
                Row1.DataArr = DataArr;
                BaoCaoModel.DataTable.TableData.Add(Row1);

                List<TableData> data = new List<TableData>();
                data = new KQGQ04DAL().KQGQ04(p.ListCapID, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                BaoCaoModel.DataTable.TableData.AddRange(data);

                #endregion

                Result.Status = 1;
                Result.Data = BaoCaoModel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel KQGQ04_XuatExcel(BaseReportParams p, string ContentRootPath, int RoleID, int CapID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, int HuyenDangNhapID)
        {
            var Result = new BaseResultModel();
            try
            {
                List<TableData> data = new List<TableData>();
                data = new KQGQ04DAL().KQGQ04(p.ListCapID, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                #region Thêm hàng Tổng
                TableData rowToanTinh = new TableData();
                TableData rowToanHuyen = new TableData();
                TableData rowTong = new TableData();
                if (data.Count > 1)
                {
                    foreach (var item in data)
                    {
                        if (item.DataArr != null && item.DataArr.Count > 0)
                        {
                            foreach (var col in item.DataArr)
                            {
                                if (col.CapID == CapCoQuanViewChiTiet.ToanTinh.GetHashCode().ToString())
                                {
                                    rowToanTinh = Utils.DeepCopy(item);
                                    break;
                                }
                                else if (col.CapID == CapCoQuanViewChiTiet.ToanHuyen.GetHashCode().ToString())
                                {
                                    rowToanHuyen = Utils.DeepCopy(item);
                                    break;
                                }
                            }
                        }
                    }
                }
                else if (data.Count == 1)
                {
                    rowTong = Utils.DeepCopy(data[0]);
                }

                if (rowToanTinh.DataArr != null && rowToanTinh.DataArr.Count > 0)
                {
                    rowTong.DataArr = rowToanTinh.DataArr;
                }
                else if (rowToanHuyen.DataArr != null && rowToanHuyen.DataArr.Count > 0)
                {
                    rowTong.DataArr = rowToanHuyen.DataArr;
                }
                if (rowTong.DataArr != null)
                {
                    rowTong.DataArr[0].Content = "Tổng";
                    data.Add(rowTong);
                }
                #endregion
                string path = @"Templates\FileTam\KQGQ04_" + CanBoDangNhapID + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";
                string urlExcel = new KQGQ04DAL().KQGQ04_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                Result.Status = 1;
                Result.Data = urlExcel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel KQGQ04_GetDSChiTietDonThu(string ContentRootPath, BaseReportParams p)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();

                var listTableHeader = new List<TableHeader>();
                TableHeader HeaderCol1 = new TableHeader(1, 0, "STT", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Số đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Ngày tiếp nhận", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Chủ đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol5 = new TableHeader(5, 0, "Địa chỉ", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol6 = new TableHeader(6, 0, "Nội dung đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol7 = new TableHeader(7, 0, "Loại đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol8 = new TableHeader(8, 0, "Kết quả xử lý", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol9 = new TableHeader(9, 0, "Kết quả giải quyết", "font-weight:bold", ref listTableHeader);
                BaoCaoModel.DataTable.TableHeader = listTableHeader;

                List<TKDonThuInfo> tKDonThuInfos = new List<TKDonThuInfo>();
                tKDonThuInfos = new KQGQ04DAL().KQGQ04_GetDSChiTietDonThu(ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now, p.Offset, p.PageSize, p.Index, p.XemTaiLieuMat, p.CanBoID, p.CoQuanID ?? 0, p.CapID);
                if (tKDonThuInfos.Count > 0)
                {
                    List<TableData> data = new List<TableData>();
                    int stt = p.Offset;
                    foreach (var item in tKDonThuInfos)
                    {
                        TableData tableData = new TableData();
                        tableData.ID = stt++;
                        tableData.DonThuID = item.DonThuID;
                        tableData.XuLyDonID = item.XuLyDonID;

                        var DataArr = new List<RowItem>();
                        RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "", ref DataArr);
                        RowItem RowItem2 = new RowItem(2, item.SoDon, "", "", null, "", ref DataArr);
                        RowItem RowItem3 = new RowItem(3, item.NgayNhapDonStr, "", "", null, "", ref DataArr);
                        RowItem RowItem4 = new RowItem(4, item.TenChuDon, "", "", null, "", ref DataArr);
                        RowItem RowItem5 = new RowItem(5, item.DiaChi, "", "", null, "", ref DataArr);
                        RowItem RowItem6 = new RowItem(6, item.NoiDungDon, "", "", null, "", ref DataArr);
                        RowItem RowItem7 = new RowItem(7, item.TenLoaiKhieuTo, "", "", null, "", ref DataArr);
                        RowItem RowItem8 = new RowItem(8, item.TenHuongGiaiQuyet, "", "", null, "", ref DataArr);
                        RowItem RowItem9 = new RowItem(9, item.KetQuaID_Str, "", "", null, "", ref DataArr);

                        tableData.DataArr = DataArr;
                        data.Add(tableData);
                    }
                    BaoCaoModel.DataTable.TableData = data;
                }

                Result.Status = 1;
                Result.Data = BaoCaoModel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel KQGQ04_GetDSChiTietDonThu_XuatExcel(BaseReportParams p, string ContentRootPath, int CanBoDangNhapID)
        {
            var Result = new BaseResultModel();
            try
            {
                List<TableData> data = new List<TableData>();
                List<TKDonThuInfo> tKDonThuInfos = new KQGQ04DAL().KQGQ04_GetDSChiTietDonThu(ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now, p.Offset, p.PageSize, p.Index, p.XemTaiLieuMat, p.CanBoID, p.CoQuanID ?? 0, p.CapID);
                if (tKDonThuInfos.Count > 0)
                {
                    int stt = p.Offset;
                    foreach (var item in tKDonThuInfos)
                    {
                        TableData tableData = new TableData();
                        tableData.ID = stt++;
                        tableData.DonThuID = item.DonThuID;
                        tableData.XuLyDonID = item.XuLyDonID;

                        var DataArr = new List<RowItem>();
                        RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "", ref DataArr);
                        RowItem RowItem2 = new RowItem(2, item.SoDon, "", "", null, "", ref DataArr);
                        RowItem RowItem3 = new RowItem(3, item.NgayNhapDonStr, "", "", null, "", ref DataArr);
                        RowItem RowItem4 = new RowItem(4, item.TenChuDon, "", "", null, "", ref DataArr);
                        RowItem RowItem5 = new RowItem(5, item.DiaChi, "", "", null, "", ref DataArr);
                        RowItem RowItem6 = new RowItem(6, item.NoiDungDon, "", "", null, "", ref DataArr);
                        RowItem RowItem7 = new RowItem(7, item.TenLoaiKhieuTo, "", "", null, "", ref DataArr);
                        RowItem RowItem8 = new RowItem(8, item.TenHuongGiaiQuyet, "", "", null, "", ref DataArr);
                        RowItem RowItem9 = new RowItem(9, item.KetQuaID_Str, "", "", null, "", ref DataArr);

                        tableData.DataArr = DataArr;
                        data.Add(tableData);
                    }
                }
                string path = @"Templates\FileTam\KQGQ04_DSChiTietDonThu" + CanBoDangNhapID + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";
                string urlExcel = new TCD01DAL().DSChiTietDonThu_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                Result.Status = 1;
                Result.Data = urlExcel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel BaoCaoThongKeTheoLoaiKhieuTo(BaseReportParams p, string ContentRootPath, IdentityHelper IdentityHelper)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.BieuSo = "";
                BaoCaoModel.ThongTinSoLieu = "Số liệu tính từ ngày " + (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy") + " đến ngày " + (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.TuNgay = (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.DenNgay = (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.Title = "THỐNG KÊ THEO LOẠI KHIẾU TỐ";
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();
                #region Header
                var listTableHeader = new List<TableHeader>();
                //Cấp 1
                TableHeader HeaderCol1 = new TableHeader(1, 0, "STT", "width: 5%", ref listTableHeader);
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Loại đơn", "", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Tổng", "", ref listTableHeader);
                //TableHeader HeaderCol4 = new TableHeader(4, 0, "Tiếp dân", "", ref listTableHeader);
                TableHeader HeaderCol5 = new TableHeader(5, 0, "Tiếp dân", "", ref listTableHeader);
                TableHeader HeaderCol6 = new TableHeader(6, 0, "Tiếp nhận đơn", "", ref listTableHeader);
               
                BaoCaoModel.DataTable.TableHeader = listTableHeader;
                #endregion
                #region TableData 

                List<TableData> data = new List<TableData>();
                data = new BaoCaoDAL().BaoCaoThongKeTheoLoaiKhieuTo(IdentityHelper, p.ListCapID, p.LoaiKhieuToID ?? 0, p.PhamViID ?? 0, ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                BaoCaoModel.DataTable.TableData.AddRange(data);

                #endregion

                Result.Status = 1;
                Result.Data = BaoCaoModel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel BaoCaoThongKeTheoLoaiKhieuTo_GetDSChiTietDonThu(string ContentRootPath, BaseReportParams p, IdentityHelper IdentityHelper)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();

                var listTableHeader = new List<TableHeader>();
                TableHeader HeaderCol1 = new TableHeader(1, 0, "STT", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Số đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Ngày tiếp nhận", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Chủ đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol5 = new TableHeader(5, 0, "Địa chỉ", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol6 = new TableHeader(6, 0, "Nội dung đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol7 = new TableHeader(7, 0, "Loại đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol8 = new TableHeader(8, 0, "Kết quả xử lý", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol9 = new TableHeader(9, 0, "Kết quả giải quyết", "font-weight:bold", ref listTableHeader);
                BaoCaoModel.DataTable.TableHeader = listTableHeader;

                List<TKDonThuInfo> tKDonThuInfos = new List<TKDonThuInfo>();
                //int start = p.Offset;
                //int end = (p.PageNumber == 0 ? 1: p.PageNumber) * p.PageSize;
                tKDonThuInfos = new BaoCaoDAL().BaoCaoThongKeTheoLoaiKhieuTo_GetDSChiTietDonThu(IdentityHelper, p.ListCapID, p.LoaiKhieuToID ?? 0, p.CoQuanID ?? 0, p.CapID ?? 0, p.Offset, p.Limit, p.Index ?? 0, ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                if (tKDonThuInfos.Count > 0)
                {
                    List<TableData> data = new List<TableData>();
                    int stt = p.Offset;
                    foreach (var item in tKDonThuInfos)
                    {
                        TableData tableData = new TableData();
                        tableData.ID = stt++;
                        tableData.DonThuID = item.DonThuID;
                        tableData.XuLyDonID = item.XuLyDonID;

                        var DataArr = new List<RowItem>();
                        RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "", ref DataArr);
                        RowItem RowItem2 = new RowItem(2, item.SoDon, "", "", null, "", ref DataArr);
                        RowItem RowItem3 = new RowItem(3, item.NgayNhapDonStr, "", "", null, "", ref DataArr);
                        RowItem RowItem4 = new RowItem(4, item.TenChuDon, "", "", null, "", ref DataArr);
                        RowItem RowItem5 = new RowItem(5, item.DiaChi, "", "", null, "", ref DataArr);
                        RowItem RowItem6 = new RowItem(6, item.NoiDungDon, "", "", null, "", ref DataArr);
                        RowItem RowItem7 = new RowItem(7, item.TenLoaiKhieuTo, "", "", null, "", ref DataArr);
                        RowItem RowItem8 = new RowItem(8, item.TenHuongGiaiQuyet, "", "", null, "", ref DataArr);
                        RowItem RowItem9 = new RowItem(9, item.KetQuaID_Str, "", "", null, "", ref DataArr);

                        tableData.DataArr = DataArr;
                        data.Add(tableData);
                    }
                    BaoCaoModel.DataTable.TableData = data;
                }

                Result.Status = 1;
                Result.Data = BaoCaoModel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel BaoCaoThongKeTheoLoaiKhieuTo_XuatExcel(BaseReportParams p, string ContentRootPath, IdentityHelper IdentityHelper)
        {
            var Result = new BaseResultModel();
            try
            {
                List<TableData> data = new List<TableData>();
                data = new BaoCaoDAL().BaoCaoThongKeTheoLoaiKhieuTo(IdentityHelper, p.ListCapID, p.LoaiKhieuToID ?? 0, p.PhamViID ?? 0, ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                string path = @"Templates\FileTam\ThongKeTheoLoaiKhieuTo_" + IdentityHelper.CanBoID + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";
                string urlExcel = new BaoCaoDAL().BaoCaoThongKeTheoLoaiKhieuTo_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                Result.Status = 1;
                Result.Data = urlExcel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel BaoCaoThongKeTheoLoaiKhieuTo_GetDSChiTietDonThu_XuatExcel(IdentityHelper IdentityHelper, BaseReportParams p, string ContentRootPath, int CanBoDangNhapID)
        {
            var Result = new BaseResultModel();
            try
            {
                List<TableData> data = new List<TableData>();
                List<TKDonThuInfo> tKDonThuInfos = new BaoCaoDAL().BaoCaoThongKeTheoLoaiKhieuTo_GetDSChiTietDonThu(IdentityHelper, p.ListCapID, p.LoaiKhieuToID ?? 0, p.CoQuanID ?? 0, p.Type ?? 0, p.Offset, p.Limit, p.Index ?? 0, ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                if (tKDonThuInfos.Count > 0)
                {
                    int stt = p.Offset;
                    foreach (var item in tKDonThuInfos)
                    {
                        TableData tableData = new TableData();
                        tableData.ID = stt++;
                        tableData.DonThuID = item.DonThuID;
                        tableData.XuLyDonID = item.XuLyDonID;

                        var DataArr = new List<RowItem>();
                        RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "", ref DataArr);
                        RowItem RowItem2 = new RowItem(2, item.SoDon, "", "", null, "", ref DataArr);
                        RowItem RowItem3 = new RowItem(3, item.NgayNhapDonStr, "", "", null, "", ref DataArr);
                        RowItem RowItem4 = new RowItem(4, item.TenChuDon, "", "", null, "", ref DataArr);
                        RowItem RowItem5 = new RowItem(5, item.DiaChi, "", "", null, "", ref DataArr);
                        RowItem RowItem6 = new RowItem(6, item.NoiDungDon, "", "", null, "", ref DataArr);
                        RowItem RowItem7 = new RowItem(7, item.TenLoaiKhieuTo, "", "", null, "", ref DataArr);
                        RowItem RowItem8 = new RowItem(8, item.TenHuongGiaiQuyet, "", "", null, "", ref DataArr);
                        RowItem RowItem9 = new RowItem(9, item.KetQuaID_Str, "", "", null, "", ref DataArr);

                        tableData.DataArr = DataArr;
                        data.Add(tableData);
                    }
                }
                string path = @"Templates\FileTam\TCD01_DSChiTietDonThu" + CanBoDangNhapID + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";
                string urlExcel = new TCD01DAL().DSChiTietDonThu_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                Result.Status = 1;
                Result.Data = urlExcel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel THKQGiaiQuyetDonKienNghiPhanAnh(BaseReportParams p, string ContentRootPath, IdentityHelper IdentityHelper)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.BieuSo = "";
                BaoCaoModel.ThongTinSoLieu = "Số liệu tính từ ngày " + (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy") + " đến ngày " + (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.TuNgay = (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.DenNgay = (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.Title = "TỔNG HỢP KẾT QUẢ GIẢI QUYẾT ĐƠN KIẾN NGHỊ, PHẢN ÁNH";
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();
                #region Header
                var listTableHeader = new List<TableHeader>();
                //Cấp 1
                TableHeader HeaderCol1 = new TableHeader(1, 0, "Đơn vị", "width: 215px", ref listTableHeader);
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Đơn thuộc thẩm quyền", "", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Kết quả giải quyết", "", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Chấp hành thời gian giải quyết theo quy định", "", ref listTableHeader);
                TableHeader HeaderCol5 = new TableHeader(5, 0, "Việc thi hành quyết định giải quyết kiến nghị, phản ánh", "", ref listTableHeader);
                TableHeader HeaderCol6 = new TableHeader(6, 0, "Ghi chú", "", ref listTableHeader);      
                //Cấp 2
                var DataChild2 = new List<TableHeader>();
                TableHeader HeaderCol21 = new TableHeader(21, 2, "Tổng số đơn", "", ref DataChild2);
                TableHeader HeaderCol22 = new TableHeader(22, 2, "Trong đó", "", ref DataChild2);
                HeaderCol2.DataChild = DataChild2;

                var DataChild3 = new List<TableHeader>();
                TableHeader HeaderCol31 = new TableHeader(31, 3, "Đã giải quyết", "", ref DataChild3);
                TableHeader HeaderCol32 = new TableHeader(32, 3, "Phân tích kết quả (vụ việc)", "", ref DataChild3);
                TableHeader HeaderCol33 = new TableHeader(33, 3, "Kiến nghị thu hồi cho Nhà nước", "", ref DataChild3);
                TableHeader HeaderCol34 = new TableHeader(34, 3, "Trả lại cho công dân", "", ref DataChild3);
                TableHeader HeaderCol35 = new TableHeader(35, 3, "Số người được trả lại quyền lợi", "", ref DataChild3);
                TableHeader HeaderCol36 = new TableHeader(36, 3, "Kiến nghị xử lý hành chính", "", ref DataChild3);
                HeaderCol3.DataChild = DataChild3;

                var DataChild4 = new List<TableHeader>();
                TableHeader HeaderCol41 = new TableHeader(41, 4, "Tổng số người", "", ref DataChild4);
                TableHeader HeaderCol42 = new TableHeader(42, 4, "Số người đã bị xử lý", "", ref DataChild4);    
                HeaderCol4.DataChild = DataChild4;

                var DataChild5 = new List<TableHeader>();
                TableHeader HeaderCol51 = new TableHeader(51, 5, "Tổng số quyết định phải tổ chức thực hiện trong kỳ báo cáo", "", ref DataChild5);
                TableHeader HeaderCol52 = new TableHeader(52, 5, "Đã thực hiện", "", ref DataChild5);
                TableHeader HeaderCol53 = new TableHeader(53, 5, "Thu hồi cho nhà nước", "", ref DataChild5);
                TableHeader HeaderCol54 = new TableHeader(54, 5, "Trả lại cho công dân", "", ref DataChild5);
                HeaderCol5.DataChild = DataChild5;
                //Cấp 3
                var DataChild22 = new List<TableHeader>();
                TableHeader HeaderCol221 = new TableHeader(221, 22, "Đơn nhận trong kỳ báo cáo", "", ref DataChild22);
                TableHeader HeaderCol222 = new TableHeader(222, 22, "Đơn tồn kỳ trước chuyển sang", "", ref DataChild22);
                HeaderCol22.DataChild = DataChild22;

                var DataChild31 = new List<TableHeader>();
                TableHeader HeaderCol311 = new TableHeader(311, 31, "Số đơn thuộc thẩm quyền", "", ref DataChild31);
                TableHeader HeaderCol312 = new TableHeader(312, 31, "Số vụ việc thuộc thẩm quyền", "", ref DataChild31);
                TableHeader HeaderCol313 = new TableHeader(313, 31, "Số vụ việc rút đơn", "", ref DataChild31);
                HeaderCol31.DataChild = DataChild31;

                var DataChild32 = new List<TableHeader>();
                TableHeader HeaderCol321 = new TableHeader(321, 32, "Đúng", "", ref DataChild32);
                TableHeader HeaderCol322 = new TableHeader(322, 32, "Sai", "", ref DataChild32);
                TableHeader HeaderCol323 = new TableHeader(323, 32, "Đúng 1 phần", "", ref DataChild32);
                HeaderCol32.DataChild = DataChild32;

                var DataChild33 = new List<TableHeader>();
                TableHeader HeaderCol331 = new TableHeader(331, 33, "Tiền (Trđ)", "", ref DataChild33);
                TableHeader HeaderCol332 = new TableHeader(332, 33, "Đất (m2)", "", ref DataChild33);
                HeaderCol33.DataChild = DataChild33;

                var DataChild34 = new List<TableHeader>();
                TableHeader HeaderCol341 = new TableHeader(341, 34, "Tiền (Trđ)", "", ref DataChild34);
                TableHeader HeaderCol342 = new TableHeader(342, 34, "Đất (m2)", "", ref DataChild34);
                HeaderCol34.DataChild = DataChild34;

                var DataChild36 = new List<TableHeader>();
                TableHeader HeaderCol361 = new TableHeader(361, 36, "Tiền (Trđ)", "", ref DataChild36);
                TableHeader HeaderCol362 = new TableHeader(362, 36, "Đất (m2)", "", ref DataChild36);
                HeaderCol36.DataChild = DataChild36;

                var DataChild53 = new List<TableHeader>();
                TableHeader HeaderCol531 = new TableHeader(531, 53, "Phải thu", "", ref DataChild53);
                TableHeader HeaderCol532 = new TableHeader(532, 53, "Đã thu", "", ref DataChild53);
                HeaderCol53.DataChild = DataChild53;

                var DataChild54 = new List<TableHeader>();
                TableHeader HeaderCol541 = new TableHeader(541, 54, "Phải trả", "", ref DataChild54);
                TableHeader HeaderCol542 = new TableHeader(542, 54, "Đã trả", "", ref DataChild54);
                HeaderCol54.DataChild = DataChild54;
                //Cấp 4
                var DataChild531 = new List<TableHeader>();
                TableHeader HeaderCol5311 = new TableHeader(5311, 531, "Tiền (Trđ)", "", ref DataChild531);
                TableHeader HeaderCol5312 = new TableHeader(5312, 531, "Đất (m2)", "", ref DataChild531);
                HeaderCol531.DataChild = DataChild531;

                var DataChild532 = new List<TableHeader>();
                TableHeader HeaderCol5321 = new TableHeader(5321, 532, "Tiền (Trđ)", "", ref DataChild532);
                TableHeader HeaderCol5322 = new TableHeader(5322, 532, "Đất (m2)", "", ref DataChild532);
                HeaderCol532.DataChild = DataChild532;

                var DataChild541 = new List<TableHeader>();
                TableHeader HeaderCol5411 = new TableHeader(5411, 541, "Tiền (Trđ)", "", ref DataChild541);
                TableHeader HeaderCol5412 = new TableHeader(5412, 541, "Đất (m2)", "", ref DataChild541);
                HeaderCol541.DataChild = DataChild541;

                var DataChild542 = new List<TableHeader>();
                TableHeader HeaderCol5421 = new TableHeader(5421, 542, "Tiền (Trđ)", "", ref DataChild542);
                TableHeader HeaderCol5422 = new TableHeader(5422, 542, "Đất (m2)", "", ref DataChild542);
                HeaderCol542.DataChild = DataChild542;

                BaoCaoModel.DataTable.TableHeader = listTableHeader;
                #endregion
                #region TableData 
                TableData Row1 = new TableData();
                Row1.ID = 1;
                Row1.isClick = false;
                var DataArr = new List<RowItem>();
                RowItem RowItem1 = new RowItem(1, "", "", "", null, "width: 215px", ref DataArr);
                RowItem RowItem2 = new RowItem(2, "1=2+3", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem3 = new RowItem(3, "2", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem4 = new RowItem(4, "3", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem5 = new RowItem(5, "4", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem6 = new RowItem(6, "5", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem7 = new RowItem(7, "6", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem8 = new RowItem(8, "7", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem9 = new RowItem(9, "8", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem10 = new RowItem(10, "9", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem11 = new RowItem(11, "10", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem12 = new RowItem(12, "11", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem13 = new RowItem(13, "12", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem14 = new RowItem(14, "13", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem15 = new RowItem(15, "14", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem16 = new RowItem(16, "15", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem17 = new RowItem(17, "16", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem18 = new RowItem(18, "17", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem19 = new RowItem(19, "18", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem20 = new RowItem(20, "19", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem21 = new RowItem(21, "20", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem22 = new RowItem(22, "21", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem23 = new RowItem(23, "22", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem24 = new RowItem(24, "23", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem25 = new RowItem(25, "24", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem26 = new RowItem(26, "25", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem27 = new RowItem(27, "26", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem28 = new RowItem(28, "27", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem29 = new RowItem(29, "28", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem30 = new RowItem(30, "29", "", "", null, "text-align: center;width: 100px", ref DataArr);
               
                Row1.DataArr = DataArr;
                BaoCaoModel.DataTable.TableData.Add(Row1);

                BaoCaoModel.DataTable.TableHeader = listTableHeader;
                #endregion
                #region TableData 

                List<TableData> data = new List<TableData>();
                data = new BaoCaoDAL().THKQGiaiQuyetDonKienNghiPhanAnh(p.ListCapID, IdentityHelper, ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                BaoCaoModel.DataTable.TableData.AddRange(data);

                #endregion

                Result.Status = 1;
                Result.Data = BaoCaoModel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel THKQGiaiQuyetDonKienNghiPhanAnh_GetDSChiTietDonThu(string ContentRootPath, BaseReportParams p, IdentityHelper IdentityHelper)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();

                var listTableHeader = new List<TableHeader>();
                TableHeader HeaderCol1 = new TableHeader(1, 0, "STT", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Số đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Ngày tiếp nhận", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Chủ đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol5 = new TableHeader(5, 0, "Địa chỉ", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol6 = new TableHeader(6, 0, "Nội dung đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol7 = new TableHeader(7, 0, "Loại đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol8 = new TableHeader(8, 0, "Kết quả xử lý", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol9 = new TableHeader(9, 0, "Kết quả giải quyết", "font-weight:bold", ref listTableHeader);
                BaoCaoModel.DataTable.TableHeader = listTableHeader;

                List<TKDonThuInfo> tKDonThuInfos = new List<TKDonThuInfo>();
                tKDonThuInfos = new BaoCaoDAL().THKQGiaiQuyetDonKienNghiPhanAnh_GetDSChiTietDonThu(p, IdentityHelper, p.Offset, p.PageSize, ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                if (tKDonThuInfos.Count > 0)
                {
                    List<TableData> data = new List<TableData>();
                    int stt = p.Offset;
                    foreach (var item in tKDonThuInfos)
                    {
                        TableData tableData = new TableData();
                        tableData.ID = stt++;
                        tableData.DonThuID = item.DonThuID;
                        tableData.XuLyDonID = item.XuLyDonID;

                        var DataArr = new List<RowItem>();
                        RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "", ref DataArr);
                        RowItem RowItem2 = new RowItem(2, item.SoDon, "", "", null, "", ref DataArr);
                        RowItem RowItem3 = new RowItem(3, item.NgayNhapDonStr, "", "", null, "", ref DataArr);
                        RowItem RowItem4 = new RowItem(4, item.TenChuDon, "", "", null, "", ref DataArr);
                        RowItem RowItem5 = new RowItem(5, item.DiaChi, "", "", null, "", ref DataArr);
                        RowItem RowItem6 = new RowItem(6, item.NoiDungDon, "", "", null, "", ref DataArr);
                        RowItem RowItem7 = new RowItem(7, item.TenLoaiKhieuTo, "", "", null, "", ref DataArr);
                        RowItem RowItem8 = new RowItem(8, item.TenHuongGiaiQuyet, "", "", null, "", ref DataArr);
                        RowItem RowItem9 = new RowItem(9, item.KetQuaID_Str, "", "", null, "", ref DataArr);

                        tableData.DataArr = DataArr;
                        data.Add(tableData);
                    }
                    BaoCaoModel.DataTable.TableData = data;
                }

                Result.Status = 1;
                Result.Data = BaoCaoModel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel THKQGiaiQuyetDonKienNghiPhanAnh_XuatExcel(BaseReportParams p, string ContentRootPath, IdentityHelper IdentityHelper)
        {
            var Result = new BaseResultModel();
            try
            {
                List<TableData> data = new List<TableData>();
                data = new BaoCaoDAL().THKQGiaiQuyetDonKienNghiPhanAnh(p.ListCapID, IdentityHelper, ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                string path = @"Templates\FileTam\THKQGiaiQuyetDonKienNghiPhanAnh_" + IdentityHelper.CanBoID + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";
                string urlExcel = new BaoCaoDAL().THKQGiaiQuyetDonKienNghiPhanAnh_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                Result.Status = 1;
                Result.Data = urlExcel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel THKQGiaiQuyetDonKienNghiPhanAnh_GetDSChiTietDonThu_XuatExcel(IdentityHelper IdentityHelper, BaseReportParams p, string ContentRootPath, int CanBoDangNhapID)
        {
            var Result = new BaseResultModel();
            try
            {
                List<TableData> data = new List<TableData>();
                List<TKDonThuInfo> tKDonThuInfos = new BaoCaoDAL().THKQGiaiQuyetDonKienNghiPhanAnh_GetDSChiTietDonThu(p, IdentityHelper, p.Offset, p.PageSize, ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                if (tKDonThuInfos.Count > 0)
                {
                    int stt = p.Offset;
                    foreach (var item in tKDonThuInfos)
                    {
                        TableData tableData = new TableData();
                        tableData.ID = stt++;
                        tableData.DonThuID = item.DonThuID;
                        tableData.XuLyDonID = item.XuLyDonID;

                        var DataArr = new List<RowItem>();
                        RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "", ref DataArr);
                        RowItem RowItem2 = new RowItem(2, item.SoDon, "", "", null, "", ref DataArr);
                        RowItem RowItem3 = new RowItem(3, item.NgayNhapDonStr, "", "", null, "", ref DataArr);
                        RowItem RowItem4 = new RowItem(4, item.TenChuDon, "", "", null, "", ref DataArr);
                        RowItem RowItem5 = new RowItem(5, item.DiaChi, "", "", null, "", ref DataArr);
                        RowItem RowItem6 = new RowItem(6, item.NoiDungDon, "", "", null, "", ref DataArr);
                        RowItem RowItem7 = new RowItem(7, item.TenLoaiKhieuTo, "", "", null, "", ref DataArr);
                        RowItem RowItem8 = new RowItem(8, item.TenHuongGiaiQuyet, "", "", null, "", ref DataArr);
                        RowItem RowItem9 = new RowItem(9, item.KetQuaID_Str, "", "", null, "", ref DataArr);

                        tableData.DataArr = DataArr;
                        data.Add(tableData);
                    }
                }
                string path = @"Templates\FileTam\TCD01_DSChiTietDonThu" + CanBoDangNhapID + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";
                string urlExcel = new TCD01DAL().DSChiTietDonThu_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                Result.Status = 1;
                Result.Data = urlExcel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel ThongKeTheoCOQuanChuyenDon(BaseReportParams p, string ContentRootPath, IdentityHelper IdentityHelper)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.BieuSo = "";
                BaoCaoModel.ThongTinSoLieu = "Số liệu tính từ ngày " + (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy") + " đến ngày " + (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.TuNgay = (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.DenNgay = (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.Title = "KẾT QUẢ THỐNG KÊ THEO CƠ QUAN CHUYỂN ĐƠN";
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();
                #region Header
                var listTableHeader = new List<TableHeader>();
                //Cấp 1
                TableHeader HeaderCol1 = new TableHeader(1, 0, "STT", "width: 5%", ref listTableHeader);
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Cơ quan chuyển đơn", "", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Số lượng", "", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Tỷ lệ (%)", "", ref listTableHeader);
               
                BaoCaoModel.DataTable.TableHeader = listTableHeader;
                #endregion
                #region TableData 

                List<TableData> data = new List<TableData>();
                data = new BaoCaoDAL().ThongKeTheoCOQuanChuyenDon(IdentityHelper, ContentRootPath, p.PhamViID ?? 0, p.CoQuanID ?? 0, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                BaoCaoModel.DataTable.TableData.AddRange(data);

                #endregion

                Result.Status = 1;
                Result.Data = BaoCaoModel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel ThongKeTheoCOQuanChuyenDon_GetDSCoQuanNhanDon(BaseReportParams p, string ContentRootPath, IdentityHelper IdentityHelper)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();

                var listTableHeader = new List<TableHeader>();
                TableHeader HeaderCol1 = new TableHeader(1, 0, "STT", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Cơ quan nhận đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Số lượng", "font-weight:bold", ref listTableHeader);
              
                BaoCaoModel.DataTable.TableHeader = listTableHeader;

                List<ThongKeInfo> tKDonThuInfos = new List<ThongKeInfo>();
                tKDonThuInfos = new BaoCaoDAL().ThongKeTheoCOQuanChuyenDon_GetDSCoQuanNhanDon(IdentityHelper, p.PhamViID ?? 0, p.CoQuanID ?? 0, p.Type ?? 0, p.Offset, p.PageSize, ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                if (tKDonThuInfos.Count > 0)
                {
                    List<TableData> data = new List<TableData>();
                    int stt = p.Offset;
                    foreach (var item in tKDonThuInfos)
                    {
                        TableData tableData = new TableData();
                        tableData.ID = stt++;
                        //tableData.DonThuID = item.DonThuID;
                        //tableData.XuLyDonID = item.XuLyDonID;

                        var DataArr = new List<RowItem>();
                        RowItem RowItem1 = new RowItem(1, stt.ToString(), item.CoQuanID.ToString(), item.CoQuanChuyenID.ToString(), null, "", ref DataArr);
                        RowItem RowItem2 = new RowItem(2, item.Ten, item.CoQuanID.ToString(), item.CoQuanChuyenID.ToString(), null, "", ref DataArr);
                        RowItem RowItem3 = new RowItem(3, item.SoLuong.ToString(), item.CoQuanID.ToString(), item.CoQuanChuyenID.ToString(), null, "", ref DataArr);
                        
                        tableData.DataArr = DataArr;
                        data.Add(tableData);
                    }
                    BaoCaoModel.DataTable.TableData = data;
                }

                Result.Status = 1;
                Result.Data = BaoCaoModel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel ThongKeTheoCOQuanChuyenDon_GetDSChiTietDonThu(BaseReportParams p, string ContentRootPath, IdentityHelper IdentityHelper)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();

                var listTableHeader = new List<TableHeader>();
                TableHeader HeaderCol1 = new TableHeader(1, 0, "STT", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Số đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Ngày tiếp nhận", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Chủ đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol5 = new TableHeader(5, 0, "Địa chỉ", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol6 = new TableHeader(6, 0, "Nội dung đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol7 = new TableHeader(7, 0, "Loại đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol8 = new TableHeader(8, 0, "Kết quả xử lý", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol9 = new TableHeader(9, 0, "Kết quả giải quyết", "font-weight:bold", ref listTableHeader);

                BaoCaoModel.DataTable.TableHeader = listTableHeader;
                int laThanhTraTinh = 0;
                var listThanhTraTinh = new SystemConfigDAL().GetByKey("Thanh_Tra_Tinh_ID").ConfigValue.Split(',').ToList().Select(x => Utils.ConvertToInt32(x.ToString(), 0)).ToList();
                if (listThanhTraTinh.Contains(IdentityHelper.CoQuanID ?? 0))
                {
                    laThanhTraTinh = 1;
                }
                p.CoQuanChuyenID = p.CapID;
                List<TKDonThuInfo> tKDonThuInfos = new List<TKDonThuInfo>();
                tKDonThuInfos = new BaoCaoDAL().ThongKeTheoCOQuanChuyenDon_GetDSChiTietDonThu(p.TuNgay ?? DateTime.MinValue, p.DenNgay ?? DateTime.MinValue,
                    IdentityHelper.CoQuanID ?? 0, IdentityHelper.CapID ?? 0, 0, IdentityHelper.TinhID ?? 0, IdentityHelper.HuyenID ?? 0, p.CoQuanID ?? 0,
                    p.PhamViID ?? 0, p.CoQuanChuyenID ?? 0, p.Offset, p.Limit, 1, IdentityHelper.CanBoID ?? 0, laThanhTraTinh, p.HuyenID ?? 0);
                if (tKDonThuInfos.Count > 0)
                {
                    List<TableData> data = new List<TableData>();
                    int stt = p.Offset;
                    foreach (var item in tKDonThuInfos)
                    {
                        TableData tableData = new TableData();
                        tableData.ID = stt++;
                        tableData.DonThuID = item.DonThuID;
                        tableData.XuLyDonID = item.XuLyDonID;

                        var DataArr = new List<RowItem>();
                        RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "", ref DataArr);
                        RowItem RowItem2 = new RowItem(2, item.SoDon, "", "", null, "", ref DataArr);
                        RowItem RowItem3 = new RowItem(3, item.NgayNhapDonStr, "", "", null, "", ref DataArr);
                        RowItem RowItem4 = new RowItem(4, item.TenChuDon, "", "", null, "", ref DataArr);
                        RowItem RowItem5 = new RowItem(5, item.DiaChi, "", "", null, "", ref DataArr);
                        RowItem RowItem6 = new RowItem(6, item.NoiDungDon, "", "", null, "", ref DataArr);
                        RowItem RowItem7 = new RowItem(7, item.TenLoaiKhieuTo, "", "", null, "", ref DataArr);
                        RowItem RowItem8 = new RowItem(8, item.TenHuongGiaiQuyet, "", "", null, "", ref DataArr);
                        RowItem RowItem9 = new RowItem(9, item.KetQuaID_Str, "", "", null, "", ref DataArr);

                        tableData.DataArr = DataArr;
                        data.Add(tableData);
                    }
                    BaoCaoModel.DataTable.TableData = data;
                }

                Result.Status = 1;
                Result.Data = BaoCaoModel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel ThongKeTheoCOQuanChuyenDon_XuatExcel(BaseReportParams p, string ContentRootPath, IdentityHelper IdentityHelper)
        {
            var Result = new BaseResultModel();
            try
            {
                List<TableData> data = new List<TableData>();
                data = new BaoCaoDAL().ThongKeTheoCOQuanChuyenDon(IdentityHelper, ContentRootPath, p.PhamViID ?? 0, p.CoQuanID ?? 0, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                string path = @"Templates\FileTam\ThongKeTheoCOQuanChuyenDon_" + IdentityHelper.CanBoID + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";
                string urlExcel = new BaoCaoDAL().ThongKeTheoCOQuanChuyenDon_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                Result.Status = 1;
                Result.Data = urlExcel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel ThongKeTheoCOQuanChuyenDon_GetDSChiTietDonThu_XuatExcel(IdentityHelper IdentityHelper, BaseReportParams p, string ContentRootPath, int CanBoDangNhapID)
        {
            var Result = new BaseResultModel();
            try
            {
                List<TableData> data = new List<TableData>();
                int laThanhTraTinh = 0;
                var listThanhTraTinh = new SystemConfigDAL().GetByKey("Thanh_Tra_Tinh_ID").ConfigValue.Split(',').ToList().Select(x => Utils.ConvertToInt32(x.ToString(), 0)).ToList();
                if (listThanhTraTinh.Contains(IdentityHelper.CoQuanID ?? 0))
                {
                    laThanhTraTinh = 1;
                }
                p.CoQuanChuyenID = p.CapID;
                List<TKDonThuInfo> tKDonThuInfos = new BaoCaoDAL().ThongKeTheoCOQuanChuyenDon_GetDSChiTietDonThu(p.TuNgay ?? DateTime.MinValue, p.DenNgay ?? DateTime.MinValue,
                    IdentityHelper.CoQuanID ?? 0, IdentityHelper.CapID ?? 0, 0, IdentityHelper.TinhID ?? 0, IdentityHelper.HuyenID ?? 0, p.CoQuanID ?? 0,
                    p.PhamViID ?? 0, p.CoQuanChuyenID ?? 0, p.Offset, p.Limit, 1, IdentityHelper.CanBoID ?? 0, laThanhTraTinh, p.HuyenID ?? 0);
                if (tKDonThuInfos.Count > 0)
                {
                    int stt = p.Offset;
                    foreach (var item in tKDonThuInfos)
                    {
                        TableData tableData = new TableData();
                        tableData.ID = stt++;
                        //tableData.DonThuID = item.DonThuID;
                        //tableData.XuLyDonID = item.XuLyDonID;

                        var DataArr = new List<RowItem>();
                        RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "", ref DataArr);
                        RowItem RowItem2 = new RowItem(2, item.SoDon, "", "", null, "", ref DataArr);
                        RowItem RowItem3 = new RowItem(3, item.NgayNhapDonStr, "", "", null, "", ref DataArr);
                        RowItem RowItem4 = new RowItem(4, item.TenChuDon, "", "", null, "", ref DataArr);
                        RowItem RowItem5 = new RowItem(5, item.DiaChi, "", "", null, "", ref DataArr);
                        RowItem RowItem6 = new RowItem(6, item.NoiDungDon, "", "", null, "", ref DataArr);
                        RowItem RowItem7 = new RowItem(7, item.TenLoaiKhieuTo, "", "", null, "", ref DataArr);
                        RowItem RowItem8 = new RowItem(8, item.TenHuongGiaiQuyet, "", "", null, "", ref DataArr);
                        RowItem RowItem9 = new RowItem(9, item.KetQuaID_Str, "", "", null, "", ref DataArr);

                        tableData.DataArr = DataArr;
                        data.Add(tableData);
                    }
                }
                string path = @"Templates\FileTam\TCD01_DSChiTietDonThu" + CanBoDangNhapID + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";
                string urlExcel = new TCD01DAL().DSChiTietDonThu_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                Result.Status = 1;
                Result.Data = urlExcel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel ThongKeTheoDiaChiChuDon(BaseReportParams p, string ContentRootPath, IdentityHelper IdentityHelper)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.BieuSo = "";
                BaoCaoModel.ThongTinSoLieu = "Số liệu tính từ ngày " + (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy") + " đến ngày " + (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.TuNgay = (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.DenNgay = (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.Title = "KẾT QUẢ THỐNG KÊ THEO ĐỊA CHỈ CHỦ ĐƠN";
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();
                #region Header
                var listTableHeader = new List<TableHeader>();
                //Cấp 1
                TableHeader HeaderCol1 = new TableHeader(1, 0, "STT", "width: 5%", ref listTableHeader);
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Địa chỉ", "", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Khiếu nại", "", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Tố cáo", "", ref listTableHeader);
                TableHeader HeaderCol5 = new TableHeader(5, 0, "Kiến nghị", "", ref listTableHeader);
                TableHeader HeaderCol6 = new TableHeader(6, 0, "Phản ánh", "", ref listTableHeader);
                TableHeader HeaderCol7 = new TableHeader(7, 0, "Tổng", "", ref listTableHeader);

                BaoCaoModel.DataTable.TableHeader = listTableHeader;
                #endregion
                #region TableData 

                List<TableData> data = new List<TableData>();
                data = new BaoCaoDAL().ThongKeTheoDiaChiChuDon(IdentityHelper, p.CoQuanID ?? 0, p.PhamViID ?? 0, ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                BaoCaoModel.DataTable.TableData.AddRange(data);

                #endregion

                Result.Status = 1;
                Result.Data = BaoCaoModel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel ThongKeTheoDiaChiChuDon_GetDSChiTietDonThu(BaseReportParams p, string ContentRootPath, IdentityHelper IdentityHelper)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();

                var listTableHeader = new List<TableHeader>();
                TableHeader HeaderCol1 = new TableHeader(1, 0, "STT", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Số đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Ngày tiếp nhận", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Chủ đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol5 = new TableHeader(5, 0, "Địa chỉ", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol6 = new TableHeader(6, 0, "Nội dung đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol7 = new TableHeader(7, 0, "Loại đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol8 = new TableHeader(8, 0, "Kết quả xử lý", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol9 = new TableHeader(9, 0, "Kết quả giải quyết", "font-weight:bold", ref listTableHeader);
                BaoCaoModel.DataTable.TableHeader = listTableHeader;

                List<TKDonThuInfo> tKDonThuInfos = new List<TKDonThuInfo>();
                tKDonThuInfos = new BaoCaoDAL().ThongKeTheoDiaChiChuDon_GetDSChiTietDonThu(p, IdentityHelper, p.PhamViID ?? 0, p.CoQuanID ?? 0, p.CapID ?? 0, p.Offset, p.PageSize, ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                if (tKDonThuInfos.Count > 0)
                {
                    List<TableData> data = new List<TableData>();
                    int stt = p.Offset;
                    foreach (var item in tKDonThuInfos)
                    {
                        TableData tableData = new TableData();
                        tableData.ID = stt++;
                        tableData.DonThuID = item.DonThuID;
                        tableData.XuLyDonID = item.XuLyDonID;

                        var DataArr = new List<RowItem>();
                        RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "", ref DataArr);
                        RowItem RowItem2 = new RowItem(2, item.SoDon, "", "", null, "", ref DataArr);
                        RowItem RowItem3 = new RowItem(3, item.NgayNhapDonStr, "", "", null, "", ref DataArr);
                        RowItem RowItem4 = new RowItem(4, item.TenChuDon, "", "", null, "", ref DataArr);
                        RowItem RowItem5 = new RowItem(5, item.DiaChi, "", "", null, "", ref DataArr);
                        RowItem RowItem6 = new RowItem(6, item.NoiDungDon, "", "", null, "", ref DataArr);
                        RowItem RowItem7 = new RowItem(7, item.TenLoaiKhieuTo, "", "", null, "", ref DataArr);
                        RowItem RowItem8 = new RowItem(8, item.TenHuongGiaiQuyet, "", "", null, "", ref DataArr);
                        RowItem RowItem9 = new RowItem(9, item.KetQuaID_Str, "", "", null, "", ref DataArr);

                        tableData.DataArr = DataArr;
                        data.Add(tableData);
                    }
                    BaoCaoModel.DataTable.TableData = data;
                }

                Result.Status = 1;
                Result.Data = BaoCaoModel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel ThongKeTheoDiaChiChuDon_GetDSChiTietDonThu_XuatExcel(IdentityHelper IdentityHelper, BaseReportParams p, string ContentRootPath, int CanBoDangNhapID)
        {
            var Result = new BaseResultModel();
            try
            {
                List<TableData> data = new List<TableData>();
                p.CoQuanChuyenID = p.CapID;
                List<TKDonThuInfo> tKDonThuInfos = new BaoCaoDAL().ThongKeTheoDiaChiChuDon_GetDSChiTietDonThu(p, IdentityHelper, p.PhamViID ?? 0, p.CoQuanID ?? 0, p.CapID ?? 0, p.Offset, p.PageSize, ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                if (tKDonThuInfos.Count > 0)
                {
                    int stt = p.Offset;
                    foreach (var item in tKDonThuInfos)
                    {
                        TableData tableData = new TableData();
                        tableData.ID = stt++;
                        //tableData.DonThuID = item.DonThuID;
                        //tableData.XuLyDonID = item.XuLyDonID;

                        var DataArr = new List<RowItem>();
                        RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "", ref DataArr);
                        RowItem RowItem2 = new RowItem(2, item.SoDon, "", "", null, "", ref DataArr);
                        RowItem RowItem3 = new RowItem(3, item.NgayNhapDonStr, "", "", null, "", ref DataArr);
                        RowItem RowItem4 = new RowItem(4, item.TenChuDon, "", "", null, "", ref DataArr);
                        RowItem RowItem5 = new RowItem(5, item.DiaChi, "", "", null, "", ref DataArr);
                        RowItem RowItem6 = new RowItem(6, item.NoiDungDon, "", "", null, "", ref DataArr);
                        RowItem RowItem7 = new RowItem(7, item.TenLoaiKhieuTo, "", "", null, "", ref DataArr);
                        RowItem RowItem8 = new RowItem(8, item.TenHuongGiaiQuyet, "", "", null, "", ref DataArr);
                        RowItem RowItem9 = new RowItem(9, item.KetQuaID_Str, "", "", null, "", ref DataArr);

                        tableData.DataArr = DataArr;
                        data.Add(tableData);
                    }
                }
                string path = @"Templates\FileTam\TCD01_DSChiTietDonThu" + CanBoDangNhapID + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";
                string urlExcel = new TCD01DAL().DSChiTietDonThu_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                Result.Status = 1;
                Result.Data = urlExcel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel ThongKeTheoDiaChiChuDon_XuatExcel(BaseReportParams p, string ContentRootPath, IdentityHelper IdentityHelper)
        {
            var Result = new BaseResultModel();
            try
            {
                List<TableData> data = new List<TableData>();
                data = new BaoCaoDAL().ThongKeTheoDiaChiChuDon(IdentityHelper, p.CoQuanID ?? 0, p.PhamViID ?? 0, ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                string path = @"Templates\FileTam\ThongKeTheoDiaChiChuDon_" + IdentityHelper.CanBoID + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";
                string urlExcel = new BaoCaoDAL().ThongKeTheoDiaChiChuDon_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                Result.Status = 1;
                Result.Data = urlExcel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel ThongKeTheoNoiPhatSinh(BaseReportParams p, string ContentRootPath, IdentityHelper IdentityHelper)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.BieuSo = "";
                BaoCaoModel.ThongTinSoLieu = "Số liệu tính từ ngày " + (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy") + " đến ngày " + (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.TuNgay = (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.DenNgay = (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.Title = "KẾT QUẢ THỐNG KÊ THEO NƠI PHÁT SINH";
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();
                #region Header
                var listTableHeader = new List<TableHeader>();
                //Cấp 1
                TableHeader HeaderCol1 = new TableHeader(1, 0, "STT", "width: 5%", ref listTableHeader);
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Địa chỉ", "", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Khiếu nại", "", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Tố cáo", "", ref listTableHeader);
                TableHeader HeaderCol5 = new TableHeader(5, 0, "Kiến nghị", "", ref listTableHeader);
                TableHeader HeaderCol6 = new TableHeader(6, 0, "Phản ánh", "", ref listTableHeader);
                TableHeader HeaderCol7 = new TableHeader(7, 0, "Tổng", "", ref listTableHeader);

                BaoCaoModel.DataTable.TableHeader = listTableHeader;
                #endregion
                #region TableData 

                List<TableData> data = new List<TableData>();
                data = new BaoCaoDAL().ThongKeTheoNoiPhatSinh(IdentityHelper, p.CoQuanID ?? 0, p.PhamViID ?? 0, ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                BaoCaoModel.DataTable.TableData.AddRange(data);

                #endregion

                Result.Status = 1;
                Result.Data = BaoCaoModel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel ThongKeTheoNoiPhatSinh_GetDSChiTietDonThu(BaseReportParams p, string ContentRootPath, IdentityHelper IdentityHelper)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();

                var listTableHeader = new List<TableHeader>();
                TableHeader HeaderCol1 = new TableHeader(1, 0, "STT", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Số đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Ngày tiếp nhận", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Chủ đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol5 = new TableHeader(5, 0, "Địa chỉ", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol6 = new TableHeader(6, 0, "Nội dung đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol7 = new TableHeader(7, 0, "Loại đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol8 = new TableHeader(8, 0, "Kết quả xử lý", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol9 = new TableHeader(9, 0, "Kết quả giải quyết", "font-weight:bold", ref listTableHeader);
                BaoCaoModel.DataTable.TableHeader = listTableHeader;

                List<TKDonThuInfo> tKDonThuInfos = new List<TKDonThuInfo>();
                tKDonThuInfos = new BaoCaoDAL().ThongKeTheoNoiPhatSinh_GetDSChiTietDonThu(p, IdentityHelper, p.PhamViID ?? 0, p.CoQuanID ?? 0, p.CapID ?? 0, p.Offset, p.PageSize, ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                if (tKDonThuInfos.Count > 0)
                {
                    List<TableData> data = new List<TableData>();
                    int stt = p.Offset;
                    foreach (var item in tKDonThuInfos)
                    {
                        TableData tableData = new TableData();
                        tableData.ID = stt++;
                        tableData.DonThuID = item.DonThuID;
                        tableData.XuLyDonID = item.XuLyDonID;

                        var DataArr = new List<RowItem>();
                        RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "", ref DataArr);
                        RowItem RowItem2 = new RowItem(2, item.SoDon, "", "", null, "", ref DataArr);
                        RowItem RowItem3 = new RowItem(3, item.NgayNhapDonStr, "", "", null, "", ref DataArr);
                        RowItem RowItem4 = new RowItem(4, item.TenChuDon, "", "", null, "", ref DataArr);
                        RowItem RowItem5 = new RowItem(5, item.DiaChi, "", "", null, "", ref DataArr);
                        RowItem RowItem6 = new RowItem(6, item.NoiDungDon, "", "", null, "", ref DataArr);
                        RowItem RowItem7 = new RowItem(7, item.TenLoaiKhieuTo, "", "", null, "", ref DataArr);
                        RowItem RowItem8 = new RowItem(8, item.TenHuongGiaiQuyet, "", "", null, "", ref DataArr);
                        RowItem RowItem9 = new RowItem(9, item.KetQuaID_Str, "", "", null, "", ref DataArr);

                        tableData.DataArr = DataArr;
                        data.Add(tableData);
                    }
                    BaoCaoModel.DataTable.TableData = data;
                }

                Result.Status = 1;
                Result.Data = BaoCaoModel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel ThongKeTheoNoiPhatSinh_GetDSChiTietDonThu_XuatExcel(IdentityHelper IdentityHelper, BaseReportParams p, string ContentRootPath, int CanBoDangNhapID)
        {
            var Result = new BaseResultModel();
            try
            {
                List<TableData> data = new List<TableData>();
                p.CoQuanChuyenID = p.CapID;
                List<TKDonThuInfo> tKDonThuInfos = new BaoCaoDAL().ThongKeTheoNoiPhatSinh_GetDSChiTietDonThu(p, IdentityHelper, p.PhamViID ?? 0, p.CoQuanID ?? 0, p.CapID ?? 0, p.Offset, p.PageSize, ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                if (tKDonThuInfos.Count > 0)
                {
                    int stt = p.Offset;
                    foreach (var item in tKDonThuInfos)
                    {
                        TableData tableData = new TableData();
                        tableData.ID = stt++;
                        //tableData.DonThuID = item.DonThuID;
                        //tableData.XuLyDonID = item.XuLyDonID;

                        var DataArr = new List<RowItem>();
                        RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "", ref DataArr);
                        RowItem RowItem2 = new RowItem(2, item.SoDon, "", "", null, "", ref DataArr);
                        RowItem RowItem3 = new RowItem(3, item.NgayNhapDonStr, "", "", null, "", ref DataArr);
                        RowItem RowItem4 = new RowItem(4, item.TenChuDon, "", "", null, "", ref DataArr);
                        RowItem RowItem5 = new RowItem(5, item.DiaChi, "", "", null, "", ref DataArr);
                        RowItem RowItem6 = new RowItem(6, item.NoiDungDon, "", "", null, "", ref DataArr);
                        RowItem RowItem7 = new RowItem(7, item.TenLoaiKhieuTo, "", "", null, "", ref DataArr);
                        RowItem RowItem8 = new RowItem(8, item.TenHuongGiaiQuyet, "", "", null, "", ref DataArr);
                        RowItem RowItem9 = new RowItem(9, item.KetQuaID_Str, "", "", null, "", ref DataArr);

                        tableData.DataArr = DataArr;
                        data.Add(tableData);
                    }
                }
                string path = @"Templates\FileTam\TCD01_DSChiTietDonThu" + CanBoDangNhapID + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";
                string urlExcel = new TCD01DAL().DSChiTietDonThu_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                Result.Status = 1;
                Result.Data = urlExcel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel ThongKeTheoNoiPhatSinh_XuatExcel(BaseReportParams p, string ContentRootPath, IdentityHelper IdentityHelper)
        {
            var Result = new BaseResultModel();
            try
            {
                List<TableData> data = new List<TableData>();
                data = new BaoCaoDAL().ThongKeTheoNoiPhatSinh(IdentityHelper, p.CoQuanID ?? 0, p.PhamViID ?? 0, ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                string path = @"Templates\FileTam\ThongKeTheoDiaChiChuDon_" + IdentityHelper.CanBoID + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";
                string urlExcel = new BaoCaoDAL().ThongKeTheoNoiPhatSinh_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                Result.Status = 1;
                Result.Data = urlExcel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel ThongKeTheoVuViecDongNguoi(BaseReportParams p, string ContentRootPath, IdentityHelper IdentityHelper)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.BieuSo = "";
                BaoCaoModel.ThongTinSoLieu = "Số liệu tính từ ngày " + (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy") + " đến ngày " + (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.TuNgay = (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.DenNgay = (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.Title = "KẾT QUẢ THỐNG KÊ THEO VỤ VIỆC ĐÔNG NGƯỜI";
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();
                #region Header
                var listTableHeader = new List<TableHeader>();
                //Cấp 1
                TableHeader HeaderCol1 = new TableHeader(1, 0, "STT", "width: 5%", ref listTableHeader);
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Nơi phát sinh", "", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Đoàn đông người", "", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Đơn nhiều người đứng tên", "", ref listTableHeader);
               
                BaoCaoModel.DataTable.TableHeader = listTableHeader;
                #endregion
                #region TableData 

                List<TableData> data = new List<TableData>();
                data = new BaoCaoDAL().ThongKeTheoVuViecDongNguoi(p.ListCapID, IdentityHelper, ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                BaoCaoModel.DataTable.TableData.AddRange(data);

                #endregion

                Result.Status = 1;
                Result.Data = BaoCaoModel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        //public BaseResultModel ThongKeTheoVuViecDongNguoi_GetDSChiTietDonThu(BaseReportParams p, string ContentRootPath, IdentityHelper IdentityHelper)
        //{
        //    var Result = new BaseResultModel();
        //    try
        //    {
        //        BaoCaoModel BaoCaoModel = new BaoCaoModel();
        //        BaoCaoModel.DataTable = new DataTable();
        //        BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
        //        BaoCaoModel.DataTable.TableData = new List<TableData>();

        //        var listTableHeader = new List<TableHeader>();
        //        TableHeader HeaderCol1 = new TableHeader(1, 0, "STT", "font-weight:bold", ref listTableHeader);
        //        TableHeader HeaderCol2 = new TableHeader(2, 0, "Số đơn", "font-weight:bold", ref listTableHeader);
        //        TableHeader HeaderCol3 = new TableHeader(3, 0, "Ngày tiếp nhận", "font-weight:bold", ref listTableHeader);
        //        TableHeader HeaderCol4 = new TableHeader(4, 0, "Chủ đơn", "font-weight:bold", ref listTableHeader);
        //        TableHeader HeaderCol5 = new TableHeader(5, 0, "Địa chỉ", "font-weight:bold", ref listTableHeader);
        //        TableHeader HeaderCol6 = new TableHeader(6, 0, "Nội dung đơn", "font-weight:bold", ref listTableHeader);
        //        TableHeader HeaderCol7 = new TableHeader(7, 0, "Loại đơn", "font-weight:bold", ref listTableHeader);
        //        TableHeader HeaderCol8 = new TableHeader(8, 0, "Kết quả xử lý", "font-weight:bold", ref listTableHeader);
        //        TableHeader HeaderCol9 = new TableHeader(9, 0, "Kết quả giải quyết", "font-weight:bold", ref listTableHeader);
        //        BaoCaoModel.DataTable.TableHeader = listTableHeader;

        //        string loaiSoLieu = "0";
        //        if (p.Index == 2)
        //        {
        //            loaiSoLieu = "1";
        //        }
        //        else if (p.Index == 3)
        //        {
        //            loaiSoLieu = "2";
        //        }
        //        else if (p.Index == 4)
        //        {
        //            loaiSoLieu = "3";
        //        }
        //        else if (p.Index == 5)
        //        {
        //            loaiSoLieu = "4";
        //        }
        //        else if (p.Index == 6)
        //        {
        //            loaiSoLieu = "5";
        //        }
        //        var start = (p.PageNumber - 1) * p.PageSize;
        //        var end = (p.PageNumber) * p.PageSize;
        //        DateTime startDate = p.TuNgay ?? DateTime.MinValue;
        //        DateTime endDate = p.DenNgay ?? DateTime.MinValue;
        //        int phongBanID = p.CoQuanID ?? 0;
        //        int canBoID = p.CapID ?? 0;
        //        int loaiDon = p.LoaiKhieuToID ?? 0;
        //        QueryFilterInfo infoQF = new QueryFilterInfo()
        //        {
        //            TuNgayGoc = startDate,
        //            TuNgayMoi = startDate,
        //            DenNgayGoc = endDate.AddDays(1),
        //            DenNgayMoi = endDate.AddDays(1),
        //            CoQuanID = IdentityHelper.CoQuanID ?? 0,
        //            PhongBanID = phongBanID,
        //            CanBoID = canBoID,
        //            LoaiXL = Utils.ConvertToInt32(loaiSoLieu, 0),
        //            LoaiDon = loaiDon,
        //            Start = start,
        //            End = end
        //        };

        //        string TIEPDAN = "1";
        //        string XLD_TONG = "2";
        //        string XLD_CHUAXULY = "3";
        //        string XLD_DAXULY_TRONGHAN = "4";
        //        string XLD_DAXULY_QUAHAN = "5";

        //        string GQD_TONG = "6";
        //        string GQD_CHUAGQ = "7";
        //        string GQD_DANGGQ_TRONGHAN = "8";
        //        string GQD_DANGGQ_QUAHAN = "9";
        //        string GQD_DAGQ = "10";
        //        string GQD_DACOBAOCAO = "11";
        //        string GQD_GIAOPHONGNV = "12";
        //        IList<BCTongHopXuLyInfo> resultList = new List<BCTongHopXuLyInfo>();

        //        try
        //        {

        //            #region tiep cong dan
        //            if (loaiSoLieu == TIEPDAN)
        //            {
        //                resultList = new BCTongHopKQTDXLD().DSDonThu_TiepDan_GetByDate(infoQF);
        //            }
        //            #endregion


        //            #region don xu ly
        //            if (loaiSoLieu == XLD_TONG || loaiSoLieu == XLD_CHUAXULY || loaiSoLieu == XLD_DAXULY_QUAHAN || loaiSoLieu == XLD_DAXULY_TRONGHAN)
        //            {
        //                resultList = new BCTongHopKQTDXLD().DSDonThu_XuLyDon_GetByDate(infoQF);
        //            }
        //            #endregion

        //            #region don giai quyet
        //            if (loaiSoLieu == GQD_TONG || loaiSoLieu == GQD_CHUAGQ || loaiSoLieu == GQD_DANGGQ_QUAHAN || loaiSoLieu == GQD_DANGGQ_TRONGHAN || loaiSoLieu == GQD_DAGQ || loaiSoLieu == GQD_DACOBAOCAO || loaiSoLieu == GQD_GIAOPHONGNV)
        //            {
        //                if (canBoID > 0)
        //                    resultList = new BCTongHopKQTDXLD().DSDonThu_CanBo_GiaiQuyetDon_GetByDate(infoQF);
        //                else if (phongBanID > 0)
        //                    resultList = new BCTongHopKQTDXLD().DSDonThu_PhongBan_GiaiQuyetDon_GetByDate(infoQF);
        //                else
        //                    resultList = new BCTongHopKQTDXLD().DSDonThu_GiaiQuyetDon_GetByDate(infoQF);
        //            }
        //            #endregion
        //        }
        //        catch (Exception ex)
        //        {

        //        }

        //        if (resultList.Count > 0)
        //        {
        //            List<TableData> data = new List<TableData>();
        //            int stt = p.Offset;
        //            foreach (var item in resultList)
        //            {
        //                TableData tableData = new TableData();
        //                tableData.ID = stt++;
        //                tableData.DonThuID = item.DonThuID;
        //                tableData.XuLyDonID = item.XuLyDonID;

        //                var DataArr = new List<RowItem>();
        //                RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "", ref DataArr);
        //                RowItem RowItem2 = new RowItem(2, item.SoDon, "", "", null, "", ref DataArr);
        //                RowItem RowItem3 = new RowItem(3, item.NgayTiepStr, "", "", null, "", ref DataArr);
        //                RowItem RowItem4 = new RowItem(4, item.HoTen, "", "", null, "", ref DataArr);
        //                RowItem RowItem5 = new RowItem(5, item.DiaChiCT, "", "", null, "", ref DataArr);
        //                RowItem RowItem6 = new RowItem(6, item.NoiDungTiep, "", "", null, "", ref DataArr);
        //                RowItem RowItem7 = new RowItem(7, item.TenLoaiKhieuTo, "", "", null, "", ref DataArr);
        //                RowItem RowItem8 = new RowItem(8, item.TenHuongGiaiQuyet, "", "", null, "", ref DataArr);
        //                RowItem RowItem9 = new RowItem(9, item.KetQuaID_Str, "", "", null, "", ref DataArr);

        //                tableData.DataArr = DataArr;
        //                data.Add(tableData);
        //            }
        //            BaoCaoModel.DataTable.TableData = data;
        //        }

        //        Result.Status = 1;
        //        Result.Data = BaoCaoModel;
        //    }
        //    catch (Exception ex)
        //    {
        //        Result.Status = -1;
        //        Result.Message = ex.ToString();
        //        Result.Data = null;
        //    }

        //    return Result;
        //}

        public BaseResultModel ThongKeTheoRutDon(BaseReportParams p, string ContentRootPath, IdentityHelper IdentityHelper)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.BieuSo = "";
                BaoCaoModel.ThongTinSoLieu = "Số liệu tính từ ngày " + (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy") + " đến ngày " + (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.TuNgay = (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.DenNgay = (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.Title = "DANH SÁCH RÚT ĐƠN";
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();
                #region Header
                var listTableHeader = new List<TableHeader>();
                //Cấp 1
                TableHeader HeaderCol1 = new TableHeader(1, 0, "STT", "width: 5%", ref listTableHeader);
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Số đơn", "", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Tên chủ đơn", "", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Địa chỉ chủ đơn", "", ref listTableHeader);
                TableHeader HeaderCol5 = new TableHeader(5, 0, "Nội dung đơn", "", ref listTableHeader);
                TableHeader HeaderCol6 = new TableHeader(6, 0, "Loại đơn", "", ref listTableHeader);
                TableHeader HeaderCol7 = new TableHeader(7, 0, "Ngày rút", "", ref listTableHeader);
                TableHeader HeaderCol8 = new TableHeader(8, 0, "Lý do rút", "", ref listTableHeader);
                TableHeader HeaderCol9 = new TableHeader(9, 0, "Cơ quan thực hiện", "", ref listTableHeader);

                BaoCaoModel.DataTable.TableHeader = listTableHeader;
                #endregion
                #region TableData 

                List<TableData> data = new List<TableData>();
                data = new BaoCaoDAL().ThongKeTheoRutDon(p.ListCapID, IdentityHelper, ContentRootPath, p.CoQuanID ?? 0, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                BaoCaoModel.DataTable.TableData.AddRange(data);

                #endregion

                Result.Status = 1;
                Result.Data = BaoCaoModel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel ThongKeTheoRutDon_XuatExcel(BaseReportParams p, string ContentRootPath, IdentityHelper IdentityHelper)
        {
            var Result = new BaseResultModel();
            try
            {
                List<TableData> data = new List<TableData>();
                data = new BaoCaoDAL().ThongKeTheoRutDon(p.ListCapID, IdentityHelper, ContentRootPath, p.CoQuanID ?? 0, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                string path = @"Templates\FileTam\ThongKeTheoRutDon_" + IdentityHelper.CanBoID + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";
                string urlExcel = new BaoCaoDAL().ThongKeTheoRutDon_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                Result.Status = 1;
                Result.Data = urlExcel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel TongHopTinhHinhTCD_XL_GQD(BaseReportParams p, string ContentRootPath, IdentityHelper IdentityHelper)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.BieuSo = "";
                BaoCaoModel.ThongTinSoLieu = "Số liệu tính từ ngày " + (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy") + " đến ngày " + (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.TuNgay = (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.DenNgay = (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.Title = "TỔNG HỢP TÌNH HÌNH TIẾP CÔNG DÂN, XỬ LÝ VÀ GIẢI QUYẾT ĐƠN THƯ";
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();
                #region Header
                var listTableHeader = new List<TableHeader>();
                //Cấp 1
                TableHeader HeaderCol1 = new TableHeader(1, 0, "STT", "", ref listTableHeader);
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Tên cơ quan", "width: 250px", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Tiếp công dân", "width: 100px", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Xử lý đơn", "width: 100px", ref listTableHeader);
                TableHeader HeaderCol5 = new TableHeader(5, 0, "Giải quyết đơn", "width: 100px", ref listTableHeader);     
                //Cấp 2
                var DataChild4 = new List<TableHeader>();
                TableHeader HeaderCol41 = new TableHeader(41, 4, "Trong hạn", "width: 100px", ref DataChild4);
                TableHeader HeaderCol42 = new TableHeader(42, 4, "Quá hạn", "width: 100px", ref DataChild4);
                TableHeader HeaderCol43 = new TableHeader(43, 4, "Tổng số", "width: 100px", ref DataChild4);
                HeaderCol4.DataChild = DataChild4;

                var DataChild5 = new List<TableHeader>();
                TableHeader HeaderCol51 = new TableHeader(51, 5, "Tổng số", "width: 100px", ref DataChild5);
                TableHeader HeaderCol52 = new TableHeader(52, 5, "Chưa giải quyết", "width: 100px", ref DataChild5);
                TableHeader HeaderCol53 = new TableHeader(53, 5, "Đang giải quyết", "width: 100px", ref DataChild5);
                TableHeader HeaderCol54 = new TableHeader(54, 5, "Đã giải quyết", "width: 100px", ref DataChild5);
                TableHeader HeaderCol55 = new TableHeader(55, 5, "Đã ban hành QĐ", "width: 100px", ref DataChild5);
                HeaderCol5.DataChild = DataChild5;
                //Cấp 3
                var DataChild53 = new List<TableHeader>();
                TableHeader HeaderCol531 = new TableHeader(531, 53, "Trong hạn", "width: 100px", ref DataChild53);
                TableHeader HeaderCol532 = new TableHeader(532, 53, "Quá hạn", "width: 100px", ref DataChild53);
                TableHeader HeaderCol533 = new TableHeader(533, 53, "Tổng số", "width: 100px", ref DataChild53);
                HeaderCol53.DataChild = DataChild53;

                var DataChild55 = new List<TableHeader>();
                TableHeader HeaderCol521 = new TableHeader(541, 54, "K/n đúng", "width: 100px", ref DataChild55);
                TableHeader HeaderCol522 = new TableHeader(542, 54, "K/n đúng 1 phần", "width: 100px", ref DataChild55);
                TableHeader HeaderCol523 = new TableHeader(543, 54, "K/n sai", "width: 100px", ref DataChild55);
                HeaderCol55.DataChild = DataChild55;      

                BaoCaoModel.DataTable.TableHeader = listTableHeader;
                #endregion
                #region TableData 

                List<TableData> data = new List<TableData>();
                data = new BaoCaoDAL().TongHopTinhHinhTCD_XL_GQD(p.ListCapID, IdentityHelper, ContentRootPath, p.CoQuanID ?? 0, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                BaoCaoModel.DataTable.TableData.AddRange(data);

                #endregion

                Result.Status = 1;
                Result.Data = BaoCaoModel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel TongHopTinhHinhTCD_XL_GQD_XuatExcel(BaseReportParams p, string ContentRootPath, IdentityHelper IdentityHelper)
        {
            var Result = new BaseResultModel();
            try
            {
                List<TableData> data = new List<TableData>();
                data = new BaoCaoDAL().TongHopTinhHinhTCD_XL_GQD(p.ListCapID, IdentityHelper, ContentRootPath, p.CoQuanID ?? 0, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                string path = @"Templates\FileTam\TongHopTinhHinhTCD_XL_GQD_" + IdentityHelper.CanBoID + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";
                string urlExcel = new BaoCaoDAL().TongHopTinhHinhTCD_XL_GQD_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                Result.Status = 1;
                Result.Data = urlExcel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel TongHopTinhHinhTCD_XL_GQD_GetDSChiTietDonThu(BaseReportParams p, string ContentRootPath, IdentityHelper IdentityHelper)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();

                var listTableHeader = new List<TableHeader>();
                TableHeader HeaderCol1 = new TableHeader(1, 0, "STT", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Số đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Ngày tiếp nhận", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Chủ đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol5 = new TableHeader(5, 0, "Địa chỉ", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol6 = new TableHeader(6, 0, "Nội dung đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol7 = new TableHeader(7, 0, "Loại đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol8 = new TableHeader(8, 0, "Kết quả xử lý", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol9 = new TableHeader(9, 0, "Kết quả giải quyết", "font-weight:bold", ref listTableHeader);
                BaoCaoModel.DataTable.TableHeader = listTableHeader;

                int LoaiXL = 0;
                if (p.Index == 2)
                {
                    LoaiXL = 1;
                }
                else if (p.Index == 3)
                {
                    LoaiXL = 2;
                }
                else if (p.Index == 4)
                {
                    LoaiXL = 3;
                }
                else if (p.Index == 5)
                {
                    LoaiXL = 4;
                }
                else if (p.Index == 6)
                {
                    LoaiXL = 5;
                }
                else if (p.Index == 7)
                {
                    LoaiXL = 13;
                }
                else if (p.Index == 8)
                {
                    LoaiXL = 6;
                }
                else if (p.Index == 9)
                {
                    LoaiXL = 7;
                }
                else if (p.Index == 10)
                {
                    LoaiXL = 8;
                }
                else if (p.Index == 11)
                {
                    LoaiXL = 9;
                }
                else if (p.Index == 12)
                {
                    LoaiXL = 10;
                }
                else if (p.Index == 13)
                {
                    LoaiXL = 11;
                }
                else if (p.Index == 14)
                {
                    LoaiXL = 12;
                }
                var start = (p.PageNumber - 1) * p.PageSize;
                var end = (p.PageNumber) * p.PageSize;
                List<DTXuLyInfo> tKDonThuInfos = new List<DTXuLyInfo>();
                tKDonThuInfos = new BaoCaoDAL().TongHopTinhHinhTCD_XL_GQD_GetDSChiTietDonThu(p.TuNgay ?? DateTime.MinValue, p.DenNgay ?? DateTime.MinValue, p.CoQuanID ?? 0, p.CapID ?? 0, IdentityHelper.TinhID ?? 0, LoaiXL, start, end).ToList();
                if (tKDonThuInfos.Count > 0)
                {
                    List<TableData> data = new List<TableData>();
                    int stt = p.Offset;
                    foreach (var item in tKDonThuInfos)
                    {
                        TableData tableData = new TableData();
                        tableData.ID = stt++;
                        tableData.DonThuID = item.DonThuID;
                        tableData.XuLyDonID = item.XuLyDonID;

                        var DataArr = new List<RowItem>();
                        RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "", ref DataArr);
                        RowItem RowItem2 = new RowItem(2, item.SoDon, "", "", null, "", ref DataArr);
                        RowItem RowItem3 = new RowItem(3, item.NgayNhapDonStr, "", "", null, "", ref DataArr);
                        RowItem RowItem4 = new RowItem(4, item.TenChuDon, "", "", null, "", ref DataArr);
                        RowItem RowItem5 = new RowItem(5, item.DiaChi, "", "", null, "", ref DataArr);
                        RowItem RowItem6 = new RowItem(6, item.NoiDungDon, "", "", null, "", ref DataArr);
                        RowItem RowItem7 = new RowItem(7, item.TenLoaiKhieuTo, "", "", null, "", ref DataArr);
                        RowItem RowItem8 = new RowItem(8, item.TenHuongGiaiQuyet, "", "", null, "", ref DataArr);
                        RowItem RowItem9 = new RowItem(9, item.KetQuaID_Str, "", "", null, "", ref DataArr);

                        tableData.DataArr = DataArr;
                        data.Add(tableData);
                    }
                    BaoCaoModel.DataTable.TableData = data;
                }

                Result.Status = 1;
                Result.Data = BaoCaoModel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel TongHopTinhHinhTCD_XL_GQD_GetDSChiTietDonThu_XuatExcel(IdentityHelper IdentityHelper, BaseReportParams p, string ContentRootPath, int CanBoDangNhapID)
        {
            var Result = new BaseResultModel();
            try
            {
                List<TableData> data = new List<TableData>();
                int LoaiXL = 0;
                if (p.Index == 2)
                {
                    LoaiXL = 1;
                }
                else if (p.Index == 3)
                {
                    LoaiXL = 2;
                }
                else if (p.Index == 4)
                {
                    LoaiXL = 3;
                }
                else if (p.Index == 5)
                {
                    LoaiXL = 4;
                }
                else if (p.Index == 6)
                {
                    LoaiXL = 5;
                }
                else if (p.Index == 7)
                {
                    LoaiXL = 13;
                }
                else if (p.Index == 8)
                {
                    LoaiXL = 6;
                }
                else if (p.Index == 9)
                {
                    LoaiXL = 7;
                }
                else if (p.Index == 10)
                {
                    LoaiXL = 8;
                }
                else if (p.Index == 11)
                {
                    LoaiXL = 9;
                }
                else if (p.Index == 12)
                {
                    LoaiXL = 10;
                }
                else if (p.Index == 13)
                {
                    LoaiXL = 11;
                }
                else if (p.Index == 14)
                {
                    LoaiXL = 12;
                }
                var start = (p.PageNumber - 1) * p.PageSize;
                var end = (p.PageNumber) * p.PageSize;
                List<DTXuLyInfo> tKDonThuInfos = new BaoCaoDAL().TongHopTinhHinhTCD_XL_GQD_GetDSChiTietDonThu(p.TuNgay ?? DateTime.MinValue, p.DenNgay ?? DateTime.MinValue, p.CoQuanID ?? 0, p.CapID ?? 0, IdentityHelper.TinhID ?? 0, LoaiXL, start, end).ToList();
                if (tKDonThuInfos.Count > 0)
                {
                    int stt = p.Offset;
                    foreach (var item in tKDonThuInfos)
                    {
                        TableData tableData = new TableData();
                        tableData.ID = stt++;
                        //tableData.DonThuID = item.DonThuID;
                        //tableData.XuLyDonID = item.XuLyDonID;

                        var DataArr = new List<RowItem>();
                        RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "", ref DataArr);
                        RowItem RowItem2 = new RowItem(2, item.SoDon, "", "", null, "", ref DataArr);
                        RowItem RowItem3 = new RowItem(3, item.NgayNhapDonStr, "", "", null, "", ref DataArr);
                        RowItem RowItem4 = new RowItem(4, item.TenChuDon, "", "", null, "", ref DataArr);
                        RowItem RowItem5 = new RowItem(5, item.DiaChi, "", "", null, "", ref DataArr);
                        RowItem RowItem6 = new RowItem(6, item.NoiDungDon, "", "", null, "", ref DataArr);
                        RowItem RowItem7 = new RowItem(7, item.TenLoaiKhieuTo, "", "", null, "", ref DataArr);
                        RowItem RowItem8 = new RowItem(8, item.TenHuongGiaiQuyet, "", "", null, "", ref DataArr);
                        RowItem RowItem9 = new RowItem(9, item.KetQuaID_Str, "", "", null, "", ref DataArr);

                        tableData.DataArr = DataArr;
                        data.Add(tableData);
                    }
                }
                string path = @"Templates\FileTam\TCD01_DSChiTietDonThu" + CanBoDangNhapID + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";
                string urlExcel = new TCD01DAL().DSChiTietDonThu_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                Result.Status = 1;
                Result.Data = urlExcel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel ThongKeDonChuyenGiaiQuyet(BaseReportParams p, string ContentRootPath, IdentityHelper IdentityHelper)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.BieuSo = "";
                BaoCaoModel.ThongTinSoLieu = "Số liệu tính từ ngày " + (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy") + " đến ngày " + (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.TuNgay = (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.DenNgay = (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.Title = "THỐNG KÊ ĐƠN CHUYỂN GIẢI QUYẾT";
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();
                #region Header
                var listTableHeader = new List<TableHeader>();
                //Cấp 1
                TableHeader HeaderCol1 = new TableHeader(1, 0, "STT", "width: 5%", ref listTableHeader);
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Cơ quan nhận đơn", "", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Tổng số đơn đã giao", "", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Báo cáo", "", ref listTableHeader);
                TableHeader HeaderCol5 = new TableHeader(5, 0, "Đã ban hành GQ", "", ref listTableHeader);
                //Cấp 2
                var DataChild4 = new List<TableHeader>();
                TableHeader HeaderCol41 = new TableHeader(41, 4, "Chưa có báo cáo", "", ref DataChild4);
                TableHeader HeaderCol42 = new TableHeader(42, 4, "Đã có báo cáo", "", ref DataChild4);
                HeaderCol4.DataChild = DataChild4;

                BaoCaoModel.DataTable.TableHeader = listTableHeader;
                #endregion
                #region TableData 

                List<TableData> data = new List<TableData>();
                data = new BaoCaoDAL().ThongKeDonChuyenGiaiQuyet(IdentityHelper, ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                BaoCaoModel.DataTable.TableData.AddRange(data);

                #endregion

                Result.Status = 1;
                Result.Data = BaoCaoModel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel ThongKeDonChuyenGiaiQuyet_XuatExcel(BaseReportParams p, string ContentRootPath, IdentityHelper IdentityHelper)
        {
            var Result = new BaseResultModel();
            try
            {
                List<TableData> data = new List<TableData>();
                data = new BaoCaoDAL().ThongKeDonChuyenGiaiQuyet(IdentityHelper, ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                string path = @"Templates\FileTam\ThongKeDonChuyenGiaiQuyet_" + IdentityHelper.CanBoID + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";
                string urlExcel = new BaoCaoDAL().ThongKeDonChuyenGiaiQuyet_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                Result.Status = 1;
                Result.Data = urlExcel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel ThongKeDonChuyenGiaiQuyet_GetDSChiTietDonThu(BaseReportParams p, string ContentRootPath, IdentityHelper IdentityHelper)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();

                var listTableHeader = new List<TableHeader>();
                TableHeader HeaderCol1 = new TableHeader(1, 0, "STT", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Số đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Ngày tiếp nhận", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Chủ đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol5 = new TableHeader(5, 0, "Địa chỉ", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol6 = new TableHeader(6, 0, "Nội dung đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol7 = new TableHeader(7, 0, "Loại đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol8 = new TableHeader(8, 0, "Kết quả xử lý", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol9 = new TableHeader(9, 0, "Kết quả giải quyết", "font-weight:bold", ref listTableHeader);
                BaoCaoModel.DataTable.TableHeader = listTableHeader;

                int trangThaiGQ = 0;
                if (p.Index == 2)
                {
                    trangThaiGQ = 1;
                }
                else if (p.Index == 3)
                {
                    trangThaiGQ = 2;
                }
                else if (p.Index == 4)
                {
                    trangThaiGQ = 3;
                }
                else if (p.Index == 5)
                {
                    trangThaiGQ = 4;
                }
                var start = (p.PageNumber - 1) * p.PageSize;
                var end = (p.PageNumber) * p.PageSize;
                List<TKDonThuInfo> tKDonThuInfos = new List<TKDonThuInfo>();
                tKDonThuInfos = new ThongKeDonThu().GetDanhSachDonThu_BCChuyenGQ(1, IdentityHelper.CanBoID ?? 0, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now, 0, p.CoQuanID ?? 0, trangThaiGQ, start, end).ToList();
                if (tKDonThuInfos.Count > 0)
                {
                    List<TableData> data = new List<TableData>();
                    int stt = p.Offset;
                    foreach (var item in tKDonThuInfos)
                    {
                        TableData tableData = new TableData();
                        tableData.ID = stt++;
                        tableData.DonThuID = item.DonThuID;
                        tableData.XuLyDonID = item.XuLyDonID;

                        var DataArr = new List<RowItem>();
                        RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "", ref DataArr);
                        RowItem RowItem2 = new RowItem(2, item.SoDon, "", "", null, "", ref DataArr);
                        RowItem RowItem3 = new RowItem(3, item.NgayNhapDonStr, "", "", null, "", ref DataArr);
                        RowItem RowItem4 = new RowItem(4, item.TenChuDon, "", "", null, "", ref DataArr);
                        RowItem RowItem5 = new RowItem(5, item.DiaChi, "", "", null, "", ref DataArr);
                        RowItem RowItem6 = new RowItem(6, item.NoiDungDon, "", "", null, "", ref DataArr);
                        RowItem RowItem7 = new RowItem(7, item.TenLoaiKhieuTo, "", "", null, "", ref DataArr);
                        RowItem RowItem8 = new RowItem(8, item.TenHuongGiaiQuyet, "", "", null, "", ref DataArr);
                        RowItem RowItem9 = new RowItem(9, item.KetQuaID_Str, "", "", null, "", ref DataArr);

                        tableData.DataArr = DataArr;
                        data.Add(tableData);
                    }
                    BaoCaoModel.DataTable.TableData = data;
                }

                Result.Status = 1;
                Result.Data = BaoCaoModel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel ThongKeDonChuyenGiaiQuyet_GetDSChiTietDonThu_XuatExcel(BaseReportParams p, string ContentRootPath, int CanBoDangNhapID)
        {
            var Result = new BaseResultModel();
            try
            {
                int trangThaiGQ = 0;
                if (p.Index == 2)
                {
                    trangThaiGQ = 1;
                }
                else if (p.Index == 3)
                {
                    trangThaiGQ = 2;
                }
                else if (p.Index == 4)
                {
                    trangThaiGQ = 3;
                }
                else if (p.Index == 5)
                {
                    trangThaiGQ = 4;
                }
               
                List<TableData> data = new List<TableData>();
                List<TKDonThuInfo> tKDonThuInfos = new ThongKeDonThu().GetDanhSachDonThu_BCChuyenGQ(1, 0, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now, 0, p.CoQuanID ?? 0, trangThaiGQ, 0, 9999999).ToList();
                if (tKDonThuInfos.Count > 0)
                {
                    int stt = p.Offset;
                    foreach (var item in tKDonThuInfos)
                    {
                        TableData tableData = new TableData();
                        tableData.ID = stt++;
                        tableData.DonThuID = item.DonThuID;
                        tableData.XuLyDonID = item.XuLyDonID;

                        var DataArr = new List<RowItem>();
                        RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "", ref DataArr);
                        RowItem RowItem2 = new RowItem(2, item.SoDon, "", "", null, "", ref DataArr);
                        RowItem RowItem3 = new RowItem(3, item.NgayNhapDonStr, "", "", null, "", ref DataArr);
                        RowItem RowItem4 = new RowItem(4, item.TenChuDon, "", "", null, "", ref DataArr);
                        RowItem RowItem5 = new RowItem(5, item.DiaChi, "", "", null, "", ref DataArr);
                        RowItem RowItem6 = new RowItem(6, item.NoiDungDon, "", "", null, "", ref DataArr);
                        RowItem RowItem7 = new RowItem(7, item.TenLoaiKhieuTo, "", "", null, "", ref DataArr);
                        RowItem RowItem8 = new RowItem(8, item.HuongGiaiQuyetExcel, "", "", null, "", ref DataArr);
                        RowItem RowItem9 = new RowItem(9, item.KetQuaExcel, "", "", null, "", ref DataArr);

                        tableData.DataArr = DataArr;
                        data.Add(tableData);
                    }
                }
                string path = @"Templates\FileTam\ThongKeDonChuyenGiaiQuyet_DSChiTietDonThu" + CanBoDangNhapID + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";
                string urlExcel = new TCD01DAL().DSChiTietDonThu_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                Result.Status = 1;
                Result.Data = urlExcel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel BaoCaoXuLyCongViec(BaseReportParams p, string ContentRootPath, IdentityHelper IdentityHelper)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.BieuSo = "";
                BaoCaoModel.ThongTinSoLieu = "Số liệu tính từ ngày " + (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy") + " đến ngày " + (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.TuNgay = (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.DenNgay = (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.Title = "BÁO CÁO XỬ LÝ CÔNG VIỆC";
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();
                #region Header
                var listTableHeader = new List<TableHeader>();
                //Cấp 1
                TableHeader HeaderCol1 = new TableHeader(1, 0, "STT", "width: 5%", ref listTableHeader);
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Cán bộ", "", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Tiếp công dân", "", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Xử lý đơn", "", ref listTableHeader);        
                //Cấp 2
                var DataChild4 = new List<TableHeader>();
                TableHeader HeaderCol41 = new TableHeader(41, 4, "Tổng số", "", ref DataChild4);
                TableHeader HeaderCol42 = new TableHeader(42, 4, "Chưa xử lý", "", ref DataChild4);
                TableHeader HeaderCol43 = new TableHeader(43, 4, "Đã xử lý", "", ref DataChild4);
                HeaderCol4.DataChild = DataChild4;
                //Cấp 3
                var DataChild43 = new List<TableHeader>();
                TableHeader HeaderCol431 = new TableHeader(431, 43, "Trong hạn", "", ref DataChild43);
                TableHeader HeaderCol432 = new TableHeader(432, 43, "Quá hạn", "", ref DataChild43);  
                HeaderCol43.DataChild = DataChild43;

                BaoCaoModel.DataTable.TableHeader = listTableHeader;
                #endregion
                #region TableData 

                List<TableData> data = new List<TableData>();
                data = new BaoCaoDAL().BaoCaoXuLyCongViec(IdentityHelper, p.LoaiKhieuToID ?? 0, ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                BaoCaoModel.DataTable.TableData.AddRange(data);

                #endregion

                Result.Status = 1;
                Result.Data = BaoCaoModel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel BaoCaoXuLyCongViec_XuatExcel(BaseReportParams p, string ContentRootPath, IdentityHelper IdentityHelper)
        {
            var Result = new BaseResultModel();
            try
            {
                List<TableData> data = new List<TableData>();
                data = new BaoCaoDAL().BaoCaoXuLyCongViec(IdentityHelper, p.LoaiKhieuToID ?? 0, ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                string path = @"Templates\FileTam\BaoCaoXuLyCongViec_" + IdentityHelper.CanBoID + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";
                string urlExcel = new BaoCaoDAL().BaoCaoXuLyCongViec_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                Result.Status = 1;
                Result.Data = urlExcel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel BaoCaoXuLyCongViec_GetDSChiTietDonThu(BaseReportParams p, string ContentRootPath, IdentityHelper IdentityHelper)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();

                var listTableHeader = new List<TableHeader>();
                TableHeader HeaderCol1 = new TableHeader(1, 0, "STT", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Số đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Ngày tiếp nhận", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Chủ đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol5 = new TableHeader(5, 0, "Địa chỉ", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol6 = new TableHeader(6, 0, "Nội dung đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol7 = new TableHeader(7, 0, "Loại đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol8 = new TableHeader(8, 0, "Kết quả xử lý", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol9 = new TableHeader(9, 0, "Kết quả giải quyết", "font-weight:bold", ref listTableHeader);
                BaoCaoModel.DataTable.TableHeader = listTableHeader;

                string loaiSoLieu = "0";
                if (p.Index == 2)
                {
                    loaiSoLieu = "1";
                }
                else if (p.Index == 3)
                {
                    loaiSoLieu = "2";
                }
                else if (p.Index == 4)
                {
                    loaiSoLieu = "3";
                }
                else if (p.Index == 5)
                {
                    loaiSoLieu = "4";
                }
                else if (p.Index == 6)
                {
                    loaiSoLieu = "5";
                }
                var start = (p.PageNumber - 1) * p.PageSize;
                var end = (p.PageNumber) * p.PageSize;
                DateTime startDate = p.TuNgay ?? DateTime.MinValue;
                DateTime endDate = p.DenNgay ?? DateTime.MinValue;
                int phongBanID = p.CoQuanID ?? 0;
                int canBoID = p.CapID ?? 0;
                int loaiDon = p.LoaiKhieuToID ?? 0;
                QueryFilterInfo infoQF = new QueryFilterInfo()
                {
                    TuNgayGoc = startDate,
                    TuNgayMoi = startDate,
                    DenNgayGoc = endDate.AddDays(1),
                    DenNgayMoi = endDate.AddDays(1),
                    CoQuanID = IdentityHelper.CoQuanID ?? 0,
                    PhongBanID = phongBanID,
                    CanBoID = canBoID,
                    LoaiXL = Utils.ConvertToInt32(loaiSoLieu, 0),
                    LoaiDon = loaiDon,
                    Start = start,
                    End = end
                };

                string TIEPDAN = "1";
                string XLD_TONG = "2";
                string XLD_CHUAXULY = "3";
                string XLD_DAXULY_TRONGHAN = "4";
                string XLD_DAXULY_QUAHAN = "5";

                string GQD_TONG = "6";
                string GQD_CHUAGQ = "7";
                string GQD_DANGGQ_TRONGHAN = "8";
                string GQD_DANGGQ_QUAHAN = "9";
                string GQD_DAGQ = "10";
                string GQD_DACOBAOCAO = "11";
                string GQD_GIAOPHONGNV = "12";
                IList<BCTongHopXuLyInfo> resultList = new List<BCTongHopXuLyInfo>();
                
                try
                {

                    #region tiep cong dan
                    if (loaiSoLieu == TIEPDAN)
                    {
                        resultList = new BCTongHopKQTDXLD().DSDonThu_TiepDan_GetByDate(infoQF);
                    }
                    #endregion


                    #region don xu ly
                    if (loaiSoLieu == XLD_TONG || loaiSoLieu == XLD_CHUAXULY || loaiSoLieu == XLD_DAXULY_QUAHAN || loaiSoLieu == XLD_DAXULY_TRONGHAN)
                    {
                        resultList = new BCTongHopKQTDXLD().DSDonThu_XuLyDon_GetByDate(infoQF);
                    }
                    #endregion

                    #region don giai quyet
                    if (loaiSoLieu == GQD_TONG || loaiSoLieu == GQD_CHUAGQ || loaiSoLieu == GQD_DANGGQ_QUAHAN || loaiSoLieu == GQD_DANGGQ_TRONGHAN || loaiSoLieu == GQD_DAGQ || loaiSoLieu == GQD_DACOBAOCAO || loaiSoLieu == GQD_GIAOPHONGNV)
                    {
                        if (canBoID > 0)
                            resultList = new BCTongHopKQTDXLD().DSDonThu_CanBo_GiaiQuyetDon_GetByDate(infoQF);
                        else if (phongBanID > 0)
                            resultList = new BCTongHopKQTDXLD().DSDonThu_PhongBan_GiaiQuyetDon_GetByDate(infoQF);
                        else
                            resultList = new BCTongHopKQTDXLD().DSDonThu_GiaiQuyetDon_GetByDate(infoQF);
                    }
                    #endregion
                }
                catch (Exception ex)
                {

                }
               
                if (resultList.Count > 0)
                {
                    List<TableData> data = new List<TableData>();
                    int stt = p.Offset;
                    foreach (var item in resultList)
                    {
                        TableData tableData = new TableData();
                        tableData.ID = stt++;
                        tableData.DonThuID = item.DonThuID;
                        tableData.XuLyDonID = item.XuLyDonID;

                        var DataArr = new List<RowItem>();
                        RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "", ref DataArr);
                        RowItem RowItem2 = new RowItem(2, item.SoDon, "", "", null, "", ref DataArr);
                        RowItem RowItem3 = new RowItem(3, item.NgayTiepStr, "", "", null, "", ref DataArr);
                        RowItem RowItem4 = new RowItem(4, item.HoTen, "", "", null, "", ref DataArr);
                        RowItem RowItem5 = new RowItem(5, item.DiaChiCT, "", "", null, "", ref DataArr);
                        RowItem RowItem6 = new RowItem(6, item.NoiDungTiep, "", "", null, "", ref DataArr);
                        RowItem RowItem7 = new RowItem(7, item.TenLoaiKhieuTo, "", "", null, "", ref DataArr);
                        RowItem RowItem8 = new RowItem(8, item.TenHuongGiaiQuyet, "", "", null, "", ref DataArr);
                        RowItem RowItem9 = new RowItem(9, item.KetQuaID_Str, "", "", null, "", ref DataArr);

                        tableData.DataArr = DataArr;
                        data.Add(tableData);
                    }
                    BaoCaoModel.DataTable.TableData = data;
                }

                Result.Status = 1;
                Result.Data = BaoCaoModel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }
        
        public BaseResultModel BaoCaoXuLyCongViec_GetDSChiTietDonThu_XuatExcel(BaseReportParams p, string ContentRootPath, IdentityHelper IdentityHelper)
        {
            var Result = new BaseResultModel();
            try
            {
                string loaiSoLieu = "0";
                if (p.Index == 2)
                {
                    loaiSoLieu = "1";
                }
                else if (p.Index == 3)
                {
                    loaiSoLieu = "2";
                }
                else if (p.Index == 4)
                {
                    loaiSoLieu = "3";
                }
                else if (p.Index == 5)
                {
                    loaiSoLieu = "4";
                }
                else if (p.Index == 6)
                {
                    loaiSoLieu = "5";
                }
                var start = (p.PageNumber - 1) * p.PageSize;
                var end = (p.PageNumber) * p.PageSize;
                DateTime startDate = p.TuNgay ?? DateTime.MinValue;
                DateTime endDate = p.DenNgay ?? DateTime.MinValue;
                int phongBanID = p.CoQuanID ?? 0;
                int canBoID = p.CapID ?? 0;
                int loaiDon = p.LoaiKhieuToID ?? 0;
                QueryFilterInfo infoQF = new QueryFilterInfo()
                {
                    TuNgayGoc = startDate,
                    TuNgayMoi = startDate,
                    DenNgayGoc = endDate.AddDays(1),
                    DenNgayMoi = endDate.AddDays(1),
                    CoQuanID = IdentityHelper.CoQuanID ?? 0,
                    PhongBanID = phongBanID,
                    CanBoID = canBoID,
                    LoaiXL = Utils.ConvertToInt32(loaiSoLieu, 0),
                    LoaiDon = loaiDon,
                    Start = start,
                    End = end
                };

                string TIEPDAN = "1";
                string XLD_TONG = "2";
                string XLD_CHUAXULY = "3";
                string XLD_DAXULY_TRONGHAN = "4";
                string XLD_DAXULY_QUAHAN = "5";

                string GQD_TONG = "6";
                string GQD_CHUAGQ = "7";
                string GQD_DANGGQ_TRONGHAN = "8";
                string GQD_DANGGQ_QUAHAN = "9";
                string GQD_DAGQ = "10";
                string GQD_DACOBAOCAO = "11";
                string GQD_GIAOPHONGNV = "12";
                IList<BCTongHopXuLyInfo> resultList = new List<BCTongHopXuLyInfo>();

                try
                {

                    #region tiep cong dan
                    if (loaiSoLieu == TIEPDAN)
                    {
                        resultList = new BCTongHopKQTDXLD().DSDonThu_TiepDan_GetByDate(infoQF);
                    }
                    #endregion


                    #region don xu ly
                    if (loaiSoLieu == XLD_TONG || loaiSoLieu == XLD_CHUAXULY || loaiSoLieu == XLD_DAXULY_QUAHAN || loaiSoLieu == XLD_DAXULY_TRONGHAN)
                    {
                        resultList = new BCTongHopKQTDXLD().DSDonThu_XuLyDon_GetByDate(infoQF);
                    }
                    #endregion

                    #region don giai quyet
                    if (loaiSoLieu == GQD_TONG || loaiSoLieu == GQD_CHUAGQ || loaiSoLieu == GQD_DANGGQ_QUAHAN || loaiSoLieu == GQD_DANGGQ_TRONGHAN || loaiSoLieu == GQD_DAGQ || loaiSoLieu == GQD_DACOBAOCAO || loaiSoLieu == GQD_GIAOPHONGNV)
                    {
                        if (canBoID > 0)
                            resultList = new BCTongHopKQTDXLD().DSDonThu_CanBo_GiaiQuyetDon_GetByDate(infoQF);
                        else if (phongBanID > 0)
                            resultList = new BCTongHopKQTDXLD().DSDonThu_PhongBan_GiaiQuyetDon_GetByDate(infoQF);
                        else
                            resultList = new BCTongHopKQTDXLD().DSDonThu_GiaiQuyetDon_GetByDate(infoQF);
                    }
                    #endregion
                }
                catch (Exception ex)
                {

                }
                List<TableData> data = new List<TableData>();
                if (resultList.Count > 0)
                {  
                    int stt = 0;
                    foreach (var item in resultList)
                    {
                        TableData tableData = new TableData();
                        tableData.ID = stt++;
                        tableData.DonThuID = item.DonThuID;
                        tableData.XuLyDonID = item.XuLyDonID;

                        var DataArr = new List<RowItem>();
                        RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "", ref DataArr);
                        RowItem RowItem2 = new RowItem(2, item.SoDon, "", "", null, "", ref DataArr);
                        RowItem RowItem3 = new RowItem(3, item.NgayTiepStr, "", "", null, "", ref DataArr);
                        RowItem RowItem4 = new RowItem(4, item.HoTen, "", "", null, "", ref DataArr);
                        RowItem RowItem5 = new RowItem(5, item.DiaChiCT, "", "", null, "", ref DataArr);
                        RowItem RowItem6 = new RowItem(6, item.NoiDungTiep, "", "", null, "", ref DataArr);
                        RowItem RowItem7 = new RowItem(7, item.TenLoaiKhieuTo, "", "", null, "", ref DataArr);
                        RowItem RowItem8 = new RowItem(8, item.TenHuongGiaiQuyet, "", "", null, "", ref DataArr);
                        RowItem RowItem9 = new RowItem(9, item.KetQuaID_Str, "", "", null, "", ref DataArr);

                        tableData.DataArr = DataArr;
                        data.Add(tableData);
                    }
                }
                string path = @"Templates\FileTam\BaoCaoXuLyCongViec_DSChiTietDonThu" + IdentityHelper.CanBoID + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";
                string urlExcel = new TCD01DAL().DSChiTietDonThu_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                Result.Status = 1;
                Result.Data = urlExcel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel ThongKeTheoVuViecDongNguoi_GetDSChiTietDonThu(BaseReportParams p, string ContentRootPath, IdentityHelper IdentityHelper)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();

                var listTableHeader = new List<TableHeader>();
                TableHeader HeaderCol1 = new TableHeader(1, 0, "STT", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Số đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Ngày tiếp nhận", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Chủ đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol5 = new TableHeader(5, 0, "Địa chỉ", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol6 = new TableHeader(6, 0, "Nội dung đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol7 = new TableHeader(7, 0, "Loại đơn", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol8 = new TableHeader(8, 0, "Kết quả xử lý", "font-weight:bold", ref listTableHeader);
                TableHeader HeaderCol9 = new TableHeader(9, 0, "Kết quả giải quyết", "font-weight:bold", ref listTableHeader);
                BaoCaoModel.DataTable.TableHeader = listTableHeader;

                List<ChiTietVuViecDongNguoiInfo> tKDonThuInfos = new List<ChiTietVuViecDongNguoiInfo>();
                tKDonThuInfos = new BaoCaoDAL().ThongKeTheoVuViecDongNguoi_GetDSChiTietDonThu(p, IdentityHelper, p.Offset, p.PageSize, ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                if (tKDonThuInfos.Count > 0)
                {
                    List<TableData> data = new List<TableData>();
                    int stt = p.Offset;
                    foreach (var item in tKDonThuInfos)
                    {
                        TableData tableData = new TableData();
                        tableData.ID = stt++;
                        tableData.DonThuID = item.DonThuID;
                        tableData.XuLyDonID = item.XuLyDonID;

                        var DataArr = new List<RowItem>();
                        RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "", ref DataArr);
                        RowItem RowItem2 = new RowItem(2, item.SoDon, "", "", null, "", ref DataArr);
                        RowItem RowItem3 = new RowItem(3, item.NgayNhapDonStr, "", "", null, "", ref DataArr);
                        RowItem RowItem4 = new RowItem(4, item.TenChuDon, "", "", null, "", ref DataArr);
                        RowItem RowItem5 = new RowItem(5, item.DiaChi, "", "", null, "", ref DataArr);
                        RowItem RowItem6 = new RowItem(6, item.NoiDungDon, "", "", null, "", ref DataArr);
                        RowItem RowItem7 = new RowItem(7, item.TenLoaiKhieuTo, "", "", null, "", ref DataArr);
                        RowItem RowItem8 = new RowItem(8, item.TenHuongGiaiQuyet, "", "", null, "", ref DataArr);
                        RowItem RowItem9 = new RowItem(9, item.KetQuaID_Str, "", "", null, "", ref DataArr);

                        tableData.DataArr = DataArr;
                        data.Add(tableData);
                    }
                    BaoCaoModel.DataTable.TableData = data;
                }

                Result.Status = 1;
                Result.Data = BaoCaoModel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel ThongKeTheoVuViecDongNguoi_GetDSChiTietDonThu_XuatExcel(IdentityHelper IdentityHelper, BaseReportParams p, string ContentRootPath, int CanBoDangNhapID)
        {
            var Result = new BaseResultModel();
            try
            {
                List<TableData> data = new List<TableData>();
                p.CoQuanChuyenID = p.CapID;
                List<ChiTietVuViecDongNguoiInfo> tKDonThuInfos = new BaoCaoDAL().ThongKeTheoVuViecDongNguoi_GetDSChiTietDonThu(p, IdentityHelper, p.Offset, p.PageSize, ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                if (tKDonThuInfos.Count > 0)
                {
                    int stt = p.Offset;
                    foreach (var item in tKDonThuInfos)
                    {
                        TableData tableData = new TableData();
                        tableData.ID = stt++;
                        //tableData.DonThuID = item.DonThuID;
                        //tableData.XuLyDonID = item.XuLyDonID;

                        var DataArr = new List<RowItem>();
                        RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "", ref DataArr);
                        RowItem RowItem2 = new RowItem(2, item.SoDon, "", "", null, "", ref DataArr);
                        RowItem RowItem3 = new RowItem(3, item.NgayNhapDonStr, "", "", null, "", ref DataArr);
                        RowItem RowItem4 = new RowItem(4, item.TenChuDon, "", "", null, "", ref DataArr);
                        RowItem RowItem5 = new RowItem(5, item.DiaChi, "", "", null, "", ref DataArr);
                        RowItem RowItem6 = new RowItem(6, item.NoiDungDon, "", "", null, "", ref DataArr);
                        RowItem RowItem7 = new RowItem(7, item.TenLoaiKhieuTo, "", "", null, "", ref DataArr);
                        RowItem RowItem8 = new RowItem(8, item.TenHuongGiaiQuyet, "", "", null, "", ref DataArr);
                        RowItem RowItem9 = new RowItem(9, item.KetQuaID_Str, "", "", null, "", ref DataArr);

                        tableData.DataArr = DataArr;
                        data.Add(tableData);
                    }
                }
                string path = @"Templates\FileTam\TCD01_DSChiTietDonThu" + CanBoDangNhapID + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";
                string urlExcel = new TCD01DAL().DSChiTietDonThu_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                Result.Status = 1;
                Result.Data = urlExcel;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

    }
}
