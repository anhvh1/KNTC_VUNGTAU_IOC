using Com.Gosol.KNTC.DAL.BaoCao;
using Com.Gosol.KNTC.DAL.DanhMuc;
using Com.Gosol.KNTC.DAL.KNTC;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.BaoCao;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Security;
using Com.Gosol.KNTC.Ultilities;
using DocumentFormat.OpenXml.Drawing.Charts;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTable = Com.Gosol.KNTC.Models.BaoCao.DataTable;

namespace Com.Gosol.KNTC.BUS.BaoCao
{
    public class KeKhaiDuLieuDauKyBUS
    {
        private KeKhaiDuLieuDauKyDAL KeKhaiDuLieuDauKyDAL;
        public KeKhaiDuLieuDauKyBUS()
        {
            KeKhaiDuLieuDauKyDAL = new KeKhaiDuLieuDauKyDAL();
        }
        public BaseResultModel TCD01(KeKhaiDuLieuDauKyParams p)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.BieuSo = "Biểu số: 01/TCD";
                //BaoCaoModel.ThongTinSoLieu = "Số liệu tính từ ngày " + (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy") + " đến ngày " + (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                //BaoCaoModel.TuNgay = (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                //BaoCaoModel.DenNgay = (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.Title = "TỔNG HỢP KẾT QUẢ TIẾP CÔNG DÂN THƯỜNG XUYÊN, ĐỊNH KỲ VÀ ĐỘT XUẤT";
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();
                #region Header
                // Cấp 1

                TableHeader HeaderCol1 = new TableHeader();
                HeaderCol1.Name = "Đơn vị";
                HeaderCol1.ID = 1;
                HeaderCol1.Style = "";
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

                TableHeader HeaderCol7 = new TableHeader();
                HeaderCol7.Name = "Ghi chú";
                HeaderCol7.ID = 7;
                //HeaderCol7.DataChild = new List<TableHeader>();
                BaoCaoModel.DataTable.TableHeader.Add(HeaderCol7);
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
                HeaderCol6_2.Name = "Ủy quyền tiếp";
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
                string style = "width: 50px;";
                var DataArr = new List<RowItem>();
                RowItem RowItem1 = new RowItem(1 + tmp, "MS", "", "", null, "width: 215px", ref DataArr);
                RowItem RowItem2 = new RowItem(2 + tmp, "1=4+13+22", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem3 = new RowItem(3 + tmp, "2=5+14+23", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem4 = new RowItem(4 + tmp, "3=6+7 +15+16 +24+25", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem5 = new RowItem(5 + tmp, "4", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem6 = new RowItem(6 + tmp, "5", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem7 = new RowItem(7 + tmp, "6", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem8 = new RowItem(8 + tmp, "7", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem9 = new RowItem(9 + tmp, "8", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem10 = new RowItem(10 + tmp, "9", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem30 = new RowItem(30 + tmp, "29", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem11 = new RowItem(11 + tmp, "10", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem12 = new RowItem(12 + tmp, "11", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem13 = new RowItem(13 + tmp, "12", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem14 = new RowItem(14 + tmp, "13", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem15 = new RowItem(15 + tmp, "14", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem16 = new RowItem(16 + tmp, "15", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem17 = new RowItem(17 + tmp, "16", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem18 = new RowItem(18 + tmp, "17", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem19 = new RowItem(19 + tmp, "18", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem20 = new RowItem(20 + tmp, "19", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem21 = new RowItem(21 + tmp, "20", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem22 = new RowItem(22 + tmp, "21", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem23 = new RowItem(23 + tmp, "22", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem24 = new RowItem(24 + tmp, "23", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem25 = new RowItem(25 + tmp, "24", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem26 = new RowItem(26 + tmp, "25", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem27 = new RowItem(27 + tmp, "26", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem28 = new RowItem(28 + tmp, "27", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem29 = new RowItem(29 + tmp, "28", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem31 = new RowItem(31 + tmp, "30", "", "", null, "text-align: center;width: 215px", ref DataArr);
                Row1.DataArr = DataArr.OrderBy(x => x.ID).ToList();
                BaoCaoModel.DataTable.TableData.Add(Row1);

                List<TableData> data = new List<TableData>();

                data = new KeKhaiDuLieuDauKyDAL().TCD01(p);
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
        public BaseResultModel TCD02(KeKhaiDuLieuDauKyParams p)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.BieuSo = "Biểu số: 02/TCD";
                //BaoCaoModel.ThongTinSoLieu = "Số liệu tính từ ngày " + (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy") + " đến ngày " + (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                //BaoCaoModel.TuNgay = (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                //BaoCaoModel.DenNgay = (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.Title = "TỔNG HỢP KẾT QUẢ PHÂN LOẠI, XỬ LÝ ĐƠN QUA TIẾP CÔNG DÂN";
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();
                #region Header
                var listTableHeader = new List<TableHeader>();
                //Cấp 2
                TableHeader HeaderCol1 = new TableHeader(1, 0, "Đơn vị", "", ref listTableHeader);
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
                string style = "width: 50px;";
                var DataArr = new List<RowItem>();
                RowItem RowItem1 = new RowItem(1, "MS", "", "", null, "width: 215px", ref DataArr);
                RowItem RowItem2 = new RowItem(2, "1=3+5+7=9+11", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem3 = new RowItem(3, "2=4+6+8=10+12", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem4 = new RowItem(4, "3", "", "", null,style +  "text-align: center;", ref DataArr);
                RowItem RowItem5 = new RowItem(5, "4", "", "", null,style +  "text-align: center;", ref DataArr);
                RowItem RowItem6 = new RowItem(6, "5", "", "", null,style +  "text-align: center;", ref DataArr);
                RowItem RowItem7 = new RowItem(7, "6", "", "", null,style +  "text-align: center;", ref DataArr);
                RowItem RowItem8 = new RowItem(8, "7", "", "", null,style +  "text-align: center;", ref DataArr);
                RowItem RowItem9 = new RowItem(9, "8", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem10 = new RowItem(10, "9", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem11 = new RowItem(11, "10", "", "", null,style +  "text-align: center;", ref DataArr);
                RowItem RowItem12 = new RowItem(12, "11", "", "", null,style +  "text-align: center;", ref DataArr);
                RowItem RowItem13 = new RowItem(13, "12", "", "", null,style +  "text-align: center;", ref DataArr);
                RowItem RowItem14 = new RowItem(14, "13", "", "", null,style +  "text-align: center;", ref DataArr);
                RowItem RowItem15 = new RowItem(15, "14", "", "", null,style +  "text-align: center;", ref DataArr);
                RowItem RowItem16 = new RowItem(16, "15", "", "", null,style +  "text-align: center;", ref DataArr);
                RowItem RowItem17 = new RowItem(17, "16", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem18 = new RowItem(18, "17", "", "", null, "text-align: center;width: 215px", ref DataArr);

                Row1.DataArr = DataArr.OrderBy(x => x.ID).ToList();
                BaoCaoModel.DataTable.TableData.Add(Row1);

                List<TableData> data = new List<TableData>();
                data = new KeKhaiDuLieuDauKyDAL().TCD02(p);
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
        public BaseResultModel XLD01(KeKhaiDuLieuDauKyParams p)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.BieuSo = "Biểu số: 01/XLD";
                //BaoCaoModel.ThongTinSoLieu = "Số liệu tính từ ngày " + (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy") + " đến ngày " + (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                //BaoCaoModel.TuNgay = (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                //BaoCaoModel.DenNgay = (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.Title = "TỔNG HỢP KẾT QUẢ XỬ LÝ ĐƠN";
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();
                #region Header
                var listTableHeader = new List<TableHeader>();
                //Cấp 2
                TableHeader HeaderCol1 = new TableHeader(1, 0, "Đơn vị", "", ref listTableHeader);
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Tổng số đơn", "", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Kỳ trước chuyển sang", "", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Tiếp nhận trong kỳ", "", ref listTableHeader);
                TableHeader HeaderCol5 = new TableHeader(5, 0, "Số đơn đã xử lý", "", ref listTableHeader);
                TableHeader HeaderCol6 = new TableHeader(6, 0, "Đủ điều kiện xử lý", "", ref listTableHeader);
                TableHeader HeaderCol7 = new TableHeader(7, 0, "Phân loại đơn theo nội dung", "", ref listTableHeader);
                TableHeader HeaderCol8 = new TableHeader(8, 0, "Phân loại đơn theo tình trạng giải quyết", "", ref listTableHeader);
                TableHeader HeaderCol9 = new TableHeader(9, 0, "Kết quả xử lý đơn", "", ref listTableHeader);
                TableHeader HeaderCol10 = new TableHeader(10, 0, "Số văn bản phúc đáp nhận được do chuyển đơn", "", ref listTableHeader);
                TableHeader HeaderCol11 = new TableHeader(11, 0, "Ghi chú", "", ref listTableHeader);
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
                string style = "width: 50px;";
                var DataArr = new List<RowItem>();
                RowItem RowItem1 = new RowItem(1, "MS", "", "", null, "width: 215px", ref DataArr);
                RowItem RowItem2 = new RowItem(2, "1=2+3+ ...+7", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem3 = new RowItem(3, "2", "", "", null,style +  "text-align: center;", ref DataArr);
                RowItem RowItem4 = new RowItem(4, "3", "", "", null,style +  "text-align: center;", ref DataArr);
                RowItem RowItem5 = new RowItem(5, "4", "", "", null,style +  "text-align: center;", ref DataArr);
                RowItem RowItem6 = new RowItem(6, "5", "", "", null,style +  "text-align: center;", ref DataArr);
                RowItem RowItem7 = new RowItem(7, "6", "", "", null,style +  "text-align: center;", ref DataArr);
                RowItem RowItem8 = new RowItem(8, "7", "", "", null,style +  "text-align: center;", ref DataArr);
                RowItem RowItem9 = new RowItem(9, "8", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem10 = new RowItem(10, "9=11+12+13 =14+15+16+17 =18+22", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem11 = new RowItem(11, "10", "", "", null,style +  "text-align: center;", ref DataArr);
                RowItem RowItem12 = new RowItem(12, "11", "", "", null,style +  "text-align: center;", ref DataArr);
                RowItem RowItem13 = new RowItem(13, "12", "", "", null,style +  "text-align: center;", ref DataArr);
                RowItem RowItem14 = new RowItem(14, "13", "", "", null,style +  "text-align: center;", ref DataArr);
                RowItem RowItem15 = new RowItem(15, "14", "", "", null,style +  "text-align: center;", ref DataArr);
                RowItem RowItem16 = new RowItem(16, "15", "", "", null,style +  "text-align: center;", ref DataArr);
                RowItem RowItem17 = new RowItem(17, "16", "", "", null,style +  "text-align: center;", ref DataArr);
                RowItem RowItem18 = new RowItem(18, "17", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem19 = new RowItem(19, "18 = 19+20+21", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem20 = new RowItem(20, "19", "", "", null,style +  "text-align: center;", ref DataArr);
                RowItem RowItem21 = new RowItem(21, "20", "", "", null,style +  "text-align: center;", ref DataArr);
                RowItem RowItem22 = new RowItem(22, "21", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem23 = new RowItem(23, "22 = 23+24+25", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem24 = new RowItem(24, "23", "", "", null,style +  "text-align: center;", ref DataArr);
                RowItem RowItem25 = new RowItem(25, "24", "", "", null,style +  "text-align: center;", ref DataArr);
                RowItem RowItem26 = new RowItem(26, "25", "", "", null,style +  "text-align: center;", ref DataArr);
                RowItem RowItem27 = new RowItem(27, "26", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem28 = new RowItem(28, "27", "", "", null, "text-align: center;width: 215px", ref DataArr);

                Row1.DataArr = DataArr.OrderBy(x => x.ID).ToList();
                BaoCaoModel.DataTable.TableData.Add(Row1);

                List<TableData> data = new List<TableData>();
                data = new KeKhaiDuLieuDauKyDAL().XLD01(p);
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
        public BaseResultModel XLD02(KeKhaiDuLieuDauKyParams p)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.BieuSo = "Biểu số: 02/XLD";
                //BaoCaoModel.ThongTinSoLieu = "Số liệu tính từ ngày " + (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy") + " đến ngày " + (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                //BaoCaoModel.TuNgay = (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                //BaoCaoModel.DenNgay = (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
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
                TableHeader HeaderCol11 = new TableHeader(11, 0, "Ghi chú", "", ref listTableHeader);
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
                string style = "width: 50px;";
                var DataArr = new List<RowItem>();
                RowItem RowItem1 = new RowItem(1, "MS", "", "", null, "width: 215px", ref DataArr);
                RowItem RowItem2 = new RowItem(2, "1=2+..+5", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem3 = new RowItem(3, "2", "", "", null,style +  "text-align: center;", ref DataArr);
                RowItem RowItem4 = new RowItem(4, "3", "", "", null,style +  "text-align: center;", ref DataArr);
                RowItem RowItem5 = new RowItem(5, "4", "", "", null,style +  "text-align: center;", ref DataArr);
                RowItem RowItem6 = new RowItem(6, "5", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem7 = new RowItem(7, "6 = 7+8", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem8 = new RowItem(8, "7", "", "", null,style +  "text-align: center;", ref DataArr);
                RowItem RowItem9 = new RowItem(9, "8", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem10 = new RowItem(10, "9", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem11 = new RowItem(11, "10=11+15+16 +17= 18+…+22= 23+26", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem12 = new RowItem(12, "11", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem13 = new RowItem(13, "12", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem14 = new RowItem(14, "13", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem15 = new RowItem(15, "14", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem16 = new RowItem(16, "15", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem17 = new RowItem(17, "16", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem18 = new RowItem(18, "17", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem19 = new RowItem(19, "18", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem20 = new RowItem(20, "19", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem21 = new RowItem(21, "20", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem22 = new RowItem(22, "21", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem23 = new RowItem(23, "22", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem24 = new RowItem(24, "23=24+25", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem25 = new RowItem(25, "24", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem26 = new RowItem(26, "25", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem27 = new RowItem(27, "26 = 27+ 28+29", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem28 = new RowItem(28, "27", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem29 = new RowItem(29, "28", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem30 = new RowItem(30, "29", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem31 = new RowItem(31, "30", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem32 = new RowItem(32, "31", "", "", null, "text-align: center;width: 215px", ref DataArr);


                Row1.DataArr = DataArr.OrderBy(x => x.ID).ToList();
                BaoCaoModel.DataTable.TableData.Add(Row1);

                List<TableData> data = new List<TableData>();
                data = new KeKhaiDuLieuDauKyDAL().XLD02(p);
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
        public BaseResultModel XLD03(KeKhaiDuLieuDauKyParams p)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.BieuSo = "Biểu số: 03/XLD";
                //BaoCaoModel.ThongTinSoLieu = "Số liệu tính từ ngày " + (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy") + " đến ngày " + (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                //BaoCaoModel.TuNgay = (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                //BaoCaoModel.DenNgay = (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.Title = "TỔNG HỢP KẾT QUẢ XỬ LÝ ĐƠN TỐ CÁO";
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();
                #region Header
                var listTableHeader = new List<TableHeader>();
                //Cấp 1
                TableHeader HeaderCol1 = new TableHeader(1, 0, "Đơn vị", "", ref listTableHeader);
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Tổng số đơn", "", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Đơn kỳ trước chuyển sang", "", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Đơn tiếp nhận đơn trong kỳ", "", ref listTableHeader);
                TableHeader HeaderCol5 = new TableHeader(5, 0, "Đơn đã xử lý", "", ref listTableHeader);
                TableHeader HeaderCol6 = new TableHeader(6, 0, "Đủ điều kiện xử lý", "", ref listTableHeader);
                TableHeader HeaderCol7 = new TableHeader(7, 0, "Phân loại vụ việc theo nội dung", "", ref listTableHeader);
                TableHeader HeaderCol8 = new TableHeader(8, 0, "Phân loại vụ việc theo tình trạng giải quyết", "", ref listTableHeader);
                TableHeader HeaderCol9 = new TableHeader(9, 0, "Kết quả xử lý", "", ref listTableHeader);
                TableHeader HeaderCol10 = new TableHeader(10, 0, "Số văn bản phúc đáp nhận được do chuyển đơn", "", ref listTableHeader);
                TableHeader HeaderCol11 = new TableHeader(11, 0, "Ghi chú", "", ref listTableHeader);
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
                string style = "width: 50px;";
                var DataArr = new List<RowItem>();
                RowItem RowItem1 = new RowItem(1, "MS", "", "", null, "width: 215px", ref DataArr);
                RowItem RowItem2 = new RowItem(2, "1=2+..+7", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem3 = new RowItem(3, "2", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem4 = new RowItem(4, "3", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem5 = new RowItem(5, "4", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem6 = new RowItem(6, "5", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem7 = new RowItem(7, "6", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem8 = new RowItem(8, "7", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem9 = new RowItem(9, "8", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem10 = new RowItem(10, "9", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem11 = new RowItem(11, "10", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem12 = new RowItem(12, "11", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem13 = new RowItem(13, "12=13+18 +...+21= 22+...+25 =26+29", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem14 = new RowItem(14, "13", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem15 = new RowItem(15, "14", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem16 = new RowItem(16, "15", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem17 = new RowItem(17, "16", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem18 = new RowItem(18, "17", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem19 = new RowItem(19, "18", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem20 = new RowItem(20, "19", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem21 = new RowItem(21, "20", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem22 = new RowItem(22, "21", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem23 = new RowItem(23, "22", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem24 = new RowItem(24, "23", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem25 = new RowItem(25, "24", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem26 = new RowItem(26, "25", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem27 = new RowItem(27, "26 = 27+28", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem28 = new RowItem(28, "27", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem29 = new RowItem(29, "28", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem30 = new RowItem(30, "29=30+ 31+32", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem31 = new RowItem(31, "30", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem32 = new RowItem(32, "31", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem33 = new RowItem(33, "32", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem34 = new RowItem(34, "33", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem35 = new RowItem(35, "34", "", "", null, "text-align: center;width: 215px", ref DataArr);


                Row1.DataArr = DataArr.OrderBy(x => x.ID).ToList();
                BaoCaoModel.DataTable.TableData.Add(Row1);

                List<TableData> data = new List<TableData>();
                data = new KeKhaiDuLieuDauKyDAL().XLD03(p);
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
        public BaseResultModel XLD04(KeKhaiDuLieuDauKyParams p)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.BieuSo = "Biểu số: 04/XLD";
                //BaoCaoModel.ThongTinSoLieu = "Số liệu tính từ ngày " + (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy") + " đến ngày " + (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                //BaoCaoModel.TuNgay = (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                //BaoCaoModel.DenNgay = (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.Title = "TỔNG HỢP KẾT QUẢ XỬ LÝ ĐƠN KIẾN NGHỊ, PHẢN ÁNH";
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();
                #region Header
                var listTableHeader = new List<TableHeader>();
                //Cấp 1
                TableHeader HeaderCol1 = new TableHeader(1, 0, "Đơn vị", "", ref listTableHeader);
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Tổng số đơn", "", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Đơn kỳ trước chuyển sang", "", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Đơn tiếp nhận đơn trong kỳ", "", ref listTableHeader);
                TableHeader HeaderCol5 = new TableHeader(5, 0, "Đã xử lý trong kỳ", "", ref listTableHeader);
                TableHeader HeaderCol6 = new TableHeader(6, 0, "Đủ điều kiện xử lý", "", ref listTableHeader);
                TableHeader HeaderCol7 = new TableHeader(7, 0, "Phân loại vụ việc theo nội dung", "", ref listTableHeader);
                TableHeader HeaderCol8 = new TableHeader(8, 0, "Phân loại vụ việc theo tình trạng giải quyết", "", ref listTableHeader);
                TableHeader HeaderCol9 = new TableHeader(9, 0, "Kết quả xử lý", "", ref listTableHeader);
                TableHeader HeaderCol10 = new TableHeader(10, 0, "Số văn bản phúc đáp nhận được do chuyển đơn", "", ref listTableHeader);
                TableHeader HeaderCol11 = new TableHeader(11, 0, "Ghi chú", "", ref listTableHeader);
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
                string style = "width: 50px;";
                var DataArr = new List<RowItem>();
                RowItem RowItem1 = new RowItem(1, "MS", "", "", null, "width: 215px", ref DataArr);
                RowItem RowItem2 = new RowItem(2, "1=2+..+7", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem3 = new RowItem(3, "2", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem4 = new RowItem(4, "3", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem5 = new RowItem(5, "4", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem6 = new RowItem(6, "5", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem7 = new RowItem(7, "6", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem8 = new RowItem(8, "7", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem9 = new RowItem(9, "8=9+10", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem10 = new RowItem(10, "9", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem11 = new RowItem(11, "10", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem12 = new RowItem(12, "11", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem13 = new RowItem(13, "12=13+..+ 16=17+..+ 19=20+21", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem14 = new RowItem(14, "13", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem15 = new RowItem(15, "14", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem16 = new RowItem(16, "15", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem17 = new RowItem(17, "16", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem18 = new RowItem(18, "17", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem19 = new RowItem(19, "18", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem20 = new RowItem(20, "19", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem21 = new RowItem(21, "20", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem22 = new RowItem(22, "21= 22+23", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem23 = new RowItem(23, "22", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem24 = new RowItem(24, "23", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem25 = new RowItem(25, "24", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem26 = new RowItem(26, "25", "", "", null, "text-align: center;width: 215px", ref DataArr);

                Row1.DataArr = DataArr.OrderBy(x => x.ID).ToList();
                BaoCaoModel.DataTable.TableData.Add(Row1);

                List<TableData> data = new List<TableData>();
                data = new KeKhaiDuLieuDauKyDAL().XLD04(p);
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
        public BaseResultModel KQGQ01(KeKhaiDuLieuDauKyParams p)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.BieuSo = "Biểu số: 01/KQGQ";
                //BaoCaoModel.ThongTinSoLieu = "Số liệu tính từ ngày " + (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy") + " đến ngày " + (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                //BaoCaoModel.TuNgay = (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                //BaoCaoModel.DenNgay = (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.Title = "TỔNG HỢP KẾT QUẢ GIẢI QUYẾT THUỘC THẨM QUYỀN";
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();
                #region Header
                var listTableHeader = new List<TableHeader>();
                //Cấp 1
                TableHeader HeaderCol1 = new TableHeader(1, 0, "Đơn vị", "", ref listTableHeader);
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Đơn khiếu nại thuộc thẩm quyền", "", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Tổng số vụ việc khiếu nại thuộc thẩm quyền", "", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Kết quả giải quyết", "", ref listTableHeader);
                TableHeader HeaderCol5 = new TableHeader(5, 0, "Phân tích kết quả giải quyết (vụ việc)", "", ref listTableHeader);
                TableHeader HeaderCol6 = new TableHeader(6, 0, "Ghi chú", "", ref listTableHeader);
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
                string style = "width: 50px;";
                var DataArr = new List<RowItem>();
                RowItem RowItem1 = new RowItem(1, "MS", "", "", null, "width: 215px", ref DataArr);
                RowItem RowItem2 = new RowItem(2, "1=2+3", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem3 = new RowItem(3, "2", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem4 = new RowItem(4, "3", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem5 = new RowItem(5, "4", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem6 = new RowItem(6, "5=20+..+24", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem7 = new RowItem(7, "6", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem8 = new RowItem(8, "7", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem9 = new RowItem(9, "8", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem10 = new RowItem(10, "9", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem11 = new RowItem(11, "10", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem12 = new RowItem(12, "11", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem13 = new RowItem(13, "12", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem14 = new RowItem(14, "13", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem15 = new RowItem(15, "14", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem16 = new RowItem(16, "15", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem17 = new RowItem(17, "16", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem18 = new RowItem(18, "17", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem19 = new RowItem(19, "18", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem20 = new RowItem(20, "19", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem21 = new RowItem(21, "20", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem22 = new RowItem(22, "21", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem23 = new RowItem(23, "22", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem24 = new RowItem(24, "23", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem25 = new RowItem(25, "24", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem26 = new RowItem(26, "25", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem27 = new RowItem(27, "26", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem28 = new RowItem(28, "27", "", "", null, "text-align: center;width: 215px", ref DataArr);

                Row1.DataArr = DataArr.OrderBy(x => x.ID).ToList();
                BaoCaoModel.DataTable.TableData.Add(Row1);

                List<TableData> data = new List<TableData>();
                data = new KeKhaiDuLieuDauKyDAL().KQGQ01(p);
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
        public BaseResultModel KQGQ02(KeKhaiDuLieuDauKyParams p)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.BieuSo = "Biểu số: 02/KQGQ";
                //BaoCaoModel.ThongTinSoLieu = "Số liệu tính từ ngày " + (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy") + " đến ngày " + (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                //BaoCaoModel.TuNgay = (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                //BaoCaoModel.DenNgay = (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.Title = "TỔNG HỢP KẾT QUẢ THI HÀNH QUYẾT ĐỊNH GIẢI QUYẾT KHIẾU NẠI";
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();
                #region Header
                var listTableHeader = new List<TableHeader>();
                //Cấp 1
                TableHeader HeaderCol1 = new TableHeader(1, 0, "Đơn vị", "", ref listTableHeader);
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Tổng số quyết định phải thực hiện trong kỳ", "", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Số quyết định đã thực hiện xong", "", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Thu hồi cho nhà nước", "", ref listTableHeader);
                TableHeader HeaderCol5 = new TableHeader(5, 0, "Trả lại cho tổ chức, cá nhân", "", ref listTableHeader);
                TableHeader HeaderCol6 = new TableHeader(6, 0, "Đã xử lý hành chính", "", ref listTableHeader);
                TableHeader HeaderCol7 = new TableHeader(7, 0, "Đã khởi tố", "", ref listTableHeader);
                TableHeader HeaderCol8 = new TableHeader(8, 0, "Ghi chú", "", ref listTableHeader);
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
                string style = "width: 50px;";
                var DataArr = new List<RowItem>();
                RowItem RowItem1 = new RowItem(1, "MS", "", "", null, "width: 215px", ref DataArr);
                RowItem RowItem2 = new RowItem(2, "1", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem3 = new RowItem(3, "2", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem4 = new RowItem(4, "3", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem5 = new RowItem(5, "4", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem6 = new RowItem(6, "5", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem7 = new RowItem(7, "6", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem8 = new RowItem(8, "7", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem9 = new RowItem(9, "8", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem10 = new RowItem(10, "9", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem11 = new RowItem(11, "10", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem12 = new RowItem(12, "11", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem13 = new RowItem(13, "12", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem14 = new RowItem(14, "13", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem15 = new RowItem(15, "14", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem16 = new RowItem(16, "15", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem17 = new RowItem(17, "16", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem18 = new RowItem(18, "17", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem19 = new RowItem(19, "18", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem20 = new RowItem(20, "19", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem21 = new RowItem(21, "20", "", "", null, "text-align: center;width: 215px", ref DataArr);

                Row1.DataArr = DataArr.OrderBy(x => x.ID).ToList();
                BaoCaoModel.DataTable.TableData.Add(Row1);

                List<TableData> data = new List<TableData>();
                data = new KeKhaiDuLieuDauKyDAL().KQGQ02(p);
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
        public BaseResultModel KQGQ03(KeKhaiDuLieuDauKyParams p)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.BieuSo = "Biểu số: 03/KQGQ";
                //BaoCaoModel.ThongTinSoLieu = "Số liệu tính từ ngày " + (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy") + " đến ngày " + (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                //BaoCaoModel.TuNgay = (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                //BaoCaoModel.DenNgay = (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.Title = "TỔNG HỢP KẾT QUẢ GIẢI QUYẾT TỐ CÁO THUỘC THẨM QUYỀN";
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();
                #region Header
                var listTableHeader = new List<TableHeader>();
                //Cấp 1
                TableHeader HeaderCol1 = new TableHeader(1, 0, "Đơn vị", "", ref listTableHeader);
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Đơn tố cáo thuộc thẩm quyền", "", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Tổng số vụ việc tố cáo thuộc thẩm quyền", "", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Trong đó số vụ việc tố cáo tiếp", "", ref listTableHeader);
                TableHeader HeaderCol5 = new TableHeader(5, 0, "Kết quả giải quyết", "", ref listTableHeader);
                TableHeader HeaderCol6 = new TableHeader(6, 0, "Phân tích kết quả giải quyết (vụ việc)", "", ref listTableHeader);
                TableHeader HeaderCol7 = new TableHeader(7, 0, "Chấp hành thời hạn giải quyết", "", ref listTableHeader);
                TableHeader HeaderCol8 = new TableHeader(8, 0, "Ghi chú", "", ref listTableHeader);
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
                TableHeader HeaderCol66 = new TableHeader(66, 6, "Trong đó tố cáo tiếp có, có sai", "", ref DataChild6);
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
                string style = "width: 50px;";
                var DataArr = new List<RowItem>();
                RowItem RowItem1 = new RowItem(1, "MS", "", "", null, "width: 215px", ref DataArr);
                RowItem RowItem2 = new RowItem(2, "1=2+3", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem3 = new RowItem(3, "2", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem4 = new RowItem(4, "3", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem5 = new RowItem(5, "4", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem6 = new RowItem(6, "5", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem7 = new RowItem(7, "6=7+..+10= 24+26+28= 30+31", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem8 = new RowItem(8, "7", "", "", null, "text-align: center;", ref DataArr);
                RowItem RowItem9 = new RowItem(9, "8=25+27 +29", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem10 = new RowItem(10, "9", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem11 = new RowItem(11, "10", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem12 = new RowItem(12, "11", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem13 = new RowItem(13, "12", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem14 = new RowItem(14, "13", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem15 = new RowItem(15, "14", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem16 = new RowItem(16, "15", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem17 = new RowItem(17, "16", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem18 = new RowItem(18, "17", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem19 = new RowItem(19, "18", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem20 = new RowItem(20, "19", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem21 = new RowItem(21, "20", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem22 = new RowItem(22, "21", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem23 = new RowItem(23, "22", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem24 = new RowItem(24, "23", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem25 = new RowItem(25, "24", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem26 = new RowItem(26, "25", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem27 = new RowItem(27, "26", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem28 = new RowItem(28, "27", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem29 = new RowItem(29, "28", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem30 = new RowItem(30, "29", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem31 = new RowItem(31, "30", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem32 = new RowItem(32, "31", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem33 = new RowItem(33, "32", "", "", null, "text-align: center;width: 215px", ref DataArr);

                Row1.DataArr = DataArr.OrderBy(x => x.ID).ToList();
                BaoCaoModel.DataTable.TableData.Add(Row1);

                List<TableData> data = new List<TableData>();
                data = new KeKhaiDuLieuDauKyDAL().KQGQ03(p);
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
        public BaseResultModel KQGQ04(KeKhaiDuLieuDauKyParams p)
        {
            var Result = new BaseResultModel();
            try
            {
                BaoCaoModel BaoCaoModel = new BaoCaoModel();
                BaoCaoModel.BieuSo = "Biểu số: 04/KQGQ";
                //BaoCaoModel.ThongTinSoLieu = "Số liệu tính từ ngày " + (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy") + " đến ngày " + (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                //BaoCaoModel.TuNgay = (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                //BaoCaoModel.DenNgay = (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
                BaoCaoModel.Title = "TỔNG HỢP KẾT QUẢ THỰC HIỆN KẾT LUẬN NỘI DUNG TỐ CÁO ";
                BaoCaoModel.DataTable = new DataTable();
                BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
                BaoCaoModel.DataTable.TableData = new List<TableData>();
                #region Header
                var listTableHeader = new List<TableHeader>();
                //Cấp 1
                TableHeader HeaderCol1 = new TableHeader(1, 0, "Đơn vị", "", ref listTableHeader);
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Tổng số kết luận phải thực hiện", "", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Số kết luận đã thực hiện xong", "", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Thu hồi cho Nhà nước", "", ref listTableHeader);
                TableHeader HeaderCol5 = new TableHeader(5, 0, "Trả lại cho tổ chức, cá nhân", "", ref listTableHeader);
                TableHeader HeaderCol6 = new TableHeader(6, 0, "Đã xử lý hành chính", "", ref listTableHeader);
                TableHeader HeaderCol7 = new TableHeader(7, 0, "Đã khởi tố", "", ref listTableHeader);
                TableHeader HeaderCol8 = new TableHeader(8, 0, "Ghi chú", "", ref listTableHeader);
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
                string style = "width: 50px;";
                var DataArr = new List<RowItem>();
                RowItem RowItem1 = new RowItem(1, "MS", "", "", null, "width: 215px", ref DataArr);
                RowItem RowItem2 = new RowItem(2, "1", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem3 = new RowItem(3, "2", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem4 = new RowItem(4, "3", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem5 = new RowItem(5, "4", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem6 = new RowItem(6, "5", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem7 = new RowItem(7, "6", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem8 = new RowItem(8, "7", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem9 = new RowItem(9, "8", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem10 = new RowItem(10, "9", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem11 = new RowItem(11, "10", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem12 = new RowItem(12, "11", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem13 = new RowItem(13, "12", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem14 = new RowItem(14, "13", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem15 = new RowItem(15, "14", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem16 = new RowItem(16, "15", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem17 = new RowItem(17, "16", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem18 = new RowItem(18, "17", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem19 = new RowItem(19, "18", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem20 = new RowItem(20, "19", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem21 = new RowItem(21, "20", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem22 = new RowItem(22, "21", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem23 = new RowItem(23, "22", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem24 = new RowItem(24, "23", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem25 = new RowItem(25, "24", "", "", null, style + "text-align: center;", ref DataArr);
                RowItem RowItem26 = new RowItem(26, "25", "", "", null, "text-align: center;width: 215px", ref DataArr);

                Row1.DataArr = DataArr.OrderBy(x => x.ID).ToList();
                BaoCaoModel.DataTable.TableData.Add(Row1);

                List<TableData> data = new List<TableData>();
                data = new KeKhaiDuLieuDauKyDAL().KQGQ04(p);
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
        public BaseResultModel Save(List<TableData> DuLieuDauKy)
        {
            var Result = new BaseResultModel();
            if(DuLieuDauKy != null && DuLieuDauKy.Count > 0)
            {
                foreach (var item in DuLieuDauKy)
                {
                    KeKhaiDuLieuDauKyModel KeKhaiDuLieuDauKyModel = new KeKhaiDuLieuDauKyModel();
                    KeKhaiDuLieuDauKyModel.LoaiBaoCao = item.LoaiBaoCao;
                    KeKhaiDuLieuDauKyModel.CoQuanID = item.CoQuanID;
                    KeKhaiDuLieuDauKyModel.NgaySuDung = item.NgaySuDung;
                    if (item.DataArr != null && item.DataArr.Count > 0)
                    {
                        if(item.DataArr.Count > 1 && item.DataArr[1] != null) KeKhaiDuLieuDauKyModel.Col1 = Utils.ConvertToDecimal(item.DataArr[1].Content, 0);
                        if(item.DataArr.Count > 2 && item.DataArr[2] != null) KeKhaiDuLieuDauKyModel.Col2 = Utils.ConvertToDecimal(item.DataArr[2].Content, 0);
                        if(item.DataArr.Count > 3 && item.DataArr[3] != null) KeKhaiDuLieuDauKyModel.Col3 = Utils.ConvertToDecimal(item.DataArr[3].Content, 0);
                        if(item.DataArr.Count > 4 && item.DataArr[4] != null) KeKhaiDuLieuDauKyModel.Col4 = Utils.ConvertToDecimal(item.DataArr[4].Content, 0);
                        if(item.DataArr.Count > 5 && item.DataArr[5] != null) KeKhaiDuLieuDauKyModel.Col5 = Utils.ConvertToDecimal(item.DataArr[5].Content, 0);
                        if(item.DataArr.Count > 6 && item.DataArr[6] != null) KeKhaiDuLieuDauKyModel.Col6 = Utils.ConvertToDecimal(item.DataArr[6].Content, 0);
                        if(item.DataArr.Count > 7 && item.DataArr[7] != null) KeKhaiDuLieuDauKyModel.Col7 = Utils.ConvertToDecimal(item.DataArr[7].Content, 0);
                        if(item.DataArr.Count > 8 && item.DataArr[8] != null) KeKhaiDuLieuDauKyModel.Col8 = Utils.ConvertToDecimal(item.DataArr[8].Content, 0);
                        if(item.DataArr.Count > 9 && item.DataArr[9] != null) KeKhaiDuLieuDauKyModel.Col9 = Utils.ConvertToDecimal(item.DataArr[9].Content, 0);
                        if(item.DataArr.Count > 10 && item.DataArr[10] != null) KeKhaiDuLieuDauKyModel.Col10 = Utils.ConvertToDecimal(item.DataArr[10].Content, 0);
                        if(item.DataArr.Count > 11 && item.DataArr[11] != null) KeKhaiDuLieuDauKyModel.Col11 = Utils.ConvertToDecimal(item.DataArr[11].Content, 0);
                        if(item.DataArr.Count > 12 && item.DataArr[12] != null) KeKhaiDuLieuDauKyModel.Col12 = Utils.ConvertToDecimal(item.DataArr[12].Content, 0);
                        if(item.DataArr.Count > 13 && item.DataArr[13] != null) KeKhaiDuLieuDauKyModel.Col13 = Utils.ConvertToDecimal(item.DataArr[13].Content, 0);
                        if(item.DataArr.Count > 14 && item.DataArr[14] != null) KeKhaiDuLieuDauKyModel.Col14 = Utils.ConvertToDecimal(item.DataArr[14].Content, 0);
                        if(item.DataArr.Count > 15 && item.DataArr[15] != null) KeKhaiDuLieuDauKyModel.Col15 = Utils.ConvertToDecimal(item.DataArr[15].Content, 0);
                        if(item.DataArr.Count > 16 && item.DataArr[16] != null) KeKhaiDuLieuDauKyModel.Col16 = Utils.ConvertToDecimal(item.DataArr[16].Content, 0);
                        if(item.DataArr.Count > 17 && item.DataArr[17] != null) KeKhaiDuLieuDauKyModel.Col17 = Utils.ConvertToDecimal(item.DataArr[17].Content, 0);
                        if(item.DataArr.Count > 18 && item.DataArr[18] != null) KeKhaiDuLieuDauKyModel.Col18 = Utils.ConvertToDecimal(item.DataArr[18].Content, 0);
                        if(item.DataArr.Count > 19 && item.DataArr[19] != null) KeKhaiDuLieuDauKyModel.Col19 = Utils.ConvertToDecimal(item.DataArr[19].Content, 0);
                        if(item.DataArr.Count > 20 && item.DataArr[20] != null) KeKhaiDuLieuDauKyModel.Col20 = Utils.ConvertToDecimal(item.DataArr[20].Content, 0);
                        if(item.DataArr.Count > 21 && item.DataArr[21] != null) KeKhaiDuLieuDauKyModel.Col21 = Utils.ConvertToDecimal(item.DataArr[21].Content, 0);
                        if(item.DataArr.Count > 22 && item.DataArr[22] != null) KeKhaiDuLieuDauKyModel.Col22 = Utils.ConvertToDecimal(item.DataArr[22].Content, 0);
                        if(item.DataArr.Count > 23 && item.DataArr[23] != null) KeKhaiDuLieuDauKyModel.Col23 = Utils.ConvertToDecimal(item.DataArr[23].Content, 0);
                        if(item.DataArr.Count > 24 && item.DataArr[24] != null) KeKhaiDuLieuDauKyModel.Col24 = Utils.ConvertToDecimal(item.DataArr[24].Content, 0);
                        if(item.DataArr.Count > 25 && item.DataArr[25] != null) KeKhaiDuLieuDauKyModel.Col25 = Utils.ConvertToDecimal(item.DataArr[25].Content, 0);
                        if(item.DataArr.Count > 26 && item.DataArr[26] != null) KeKhaiDuLieuDauKyModel.Col26 = Utils.ConvertToDecimal(item.DataArr[26].Content, 0);
                        if(item.DataArr.Count > 27 && item.DataArr[27] != null) KeKhaiDuLieuDauKyModel.Col27 = Utils.ConvertToDecimal(item.DataArr[27].Content, 0);
                        if(item.DataArr.Count > 28 && item.DataArr[28] != null) KeKhaiDuLieuDauKyModel.Col28 = Utils.ConvertToDecimal(item.DataArr[28].Content, 0);
                        if(item.DataArr.Count > 29 && item.DataArr[29] != null) KeKhaiDuLieuDauKyModel.Col29 = Utils.ConvertToDecimal(item.DataArr[29].Content, 0);
                        if(item.DataArr.Count > 30 && item.DataArr[30] != null) KeKhaiDuLieuDauKyModel.Col30 = Utils.ConvertToDecimal(item.DataArr[30].Content, 0);
                        if(item.DataArr.Count > 31 && item.DataArr[31] != null) KeKhaiDuLieuDauKyModel.Col31 = Utils.ConvertToDecimal(item.DataArr[31].Content, 0);
                        if(item.DataArr.Count > 32 && item.DataArr[32] != null) KeKhaiDuLieuDauKyModel.Col32 = Utils.ConvertToDecimal(item.DataArr[32].Content, 0);
                        if(item.DataArr.Count > 33 && item.DataArr[33] != null) KeKhaiDuLieuDauKyModel.Col33 = Utils.ConvertToDecimal(item.DataArr[33].Content, 0);
                        if(item.DataArr.Count > 34 && item.DataArr[34] != null) KeKhaiDuLieuDauKyModel.Col34 = Utils.ConvertToDecimal(item.DataArr[34].Content, 0);
                        if(item.DataArr.Count > 35 && item.DataArr[35] != null) KeKhaiDuLieuDauKyModel.Col35 = Utils.ConvertToDecimal(item.DataArr[35].Content, 0);
                        if (item.DataArr.Count > 30 && item.DataArr[30] != null && item.LoaiBaoCao == 1)
                        {
                            KeKhaiDuLieuDauKyModel.Col30_GhiChu = item.DataArr[30].Content;
                        }
                        if (item.DataArr.Count > 17 && item.DataArr[17] != null && item.LoaiBaoCao == 2)
                        {
                            KeKhaiDuLieuDauKyModel.Col17_GhiChu = item.DataArr[17].Content;
                        }
                        if (item.DataArr.Count > 27 && item.DataArr[27] != null && item.LoaiBaoCao == 3)
                        {
                            KeKhaiDuLieuDauKyModel.Col27_GhiChu = item.DataArr[27].Content;
                        }
                        if (item.DataArr.Count > 31 && item.DataArr[31] != null && item.LoaiBaoCao == 4)
                        {
                            KeKhaiDuLieuDauKyModel.Col31_GhiChu = item.DataArr[31].Content;
                        }
                        if (item.DataArr.Count > 34 && item.DataArr[34] != null && item.LoaiBaoCao == 5)
                        {
                            KeKhaiDuLieuDauKyModel.Col34_GhiChu = item.DataArr[34].Content;
                        }
                        if (item.DataArr.Count > 25 && item.DataArr[25] != null && item.LoaiBaoCao == 6)
                        {
                            KeKhaiDuLieuDauKyModel.Col25_GhiChu = item.DataArr[25].Content;
                        }
                        if (item.DataArr.Count > 27 && item.DataArr[27] != null && item.LoaiBaoCao == 7)
                        {
                            KeKhaiDuLieuDauKyModel.Col27_GhiChu = item.DataArr[27].Content;
                        }
                        if (item.DataArr.Count > 20 && item.DataArr[20] != null && item.LoaiBaoCao == 8)
                        {
                            KeKhaiDuLieuDauKyModel.Col20_GhiChu = item.DataArr[20].Content;
                        }
                        if (item.DataArr.Count > 32 && item.DataArr[32] != null && item.LoaiBaoCao == 9)
                        {
                            KeKhaiDuLieuDauKyModel.Col32_GhiChu = item.DataArr[32].Content;
                        }
                        if (item.DataArr.Count > 25 && item.DataArr[25] != null && item.LoaiBaoCao == 10)
                        {
                            KeKhaiDuLieuDauKyModel.Col25_GhiChu = item.DataArr[25].Content;
                        }
                    }
                    
                    Result.Data = KeKhaiDuLieuDauKyDAL.Insert(KeKhaiDuLieuDauKyModel);
                }
            }
          
            Result.Status = 1;
            Result.Message = "Lưu thành công";
            return Result;
        }
    }
}
