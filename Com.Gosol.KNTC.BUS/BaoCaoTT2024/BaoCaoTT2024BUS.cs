using Com.Gosol.KNTC.DAL.BaoCao;
using Com.Gosol.KNTC.Models.BaoCao;
using Com.Gosol.KNTC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Gosol.KNTC.DAL.BaoCaoTT2024;
using Com.Gosol.KNTC.DAL.HeThong;
using Com.Gosol.KNTC.DAL.KNTC;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Ultilities;

namespace Com.Gosol.KNTC.BUS.BaoCaoTT2024
{
    public class BaoCaoTT2024BUS
    {
        private BaoCaoTT2024DAL baoCaoTT2024DAL;
        public BaoCaoTT2024BUS()
        {
            baoCaoTT2024DAL = new BaoCaoTT2024DAL();
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
        public BaseResultModel TCD01_TT2024(BaseReportParams p, string ContentRootPath, int RoleID, int CapID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, int HuyenDangNhapID)
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
                HeaderCol5_4_3.Name = "Số vụ việc tiếp lần đầu";
                HeaderCol5_4_3.ID = 17;
                HeaderCol5_4_3.ParentID = 10;
                HeaderCol5_4_3.Style = "";
                HeaderCol5_4.DataChild.Add(HeaderCol5_4_3);

                TableHeader HeaderCol5_4_4 = new TableHeader();
                HeaderCol5_4_4.Name = "Số vụ việc tiếp nhiều lần";
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
                HeaderCol6_1_5_3.Name = "Số vụ việc tiếp lần đầu";
                HeaderCol6_1_5_3.ID = 33;
                HeaderCol6_1_5_3.ParentID = 23;
                HeaderCol6_1_5_3.Style = "";
                HeaderCol6_1_5.DataChild.Add(HeaderCol6_1_5_3);

                TableHeader HeaderCol6_1_5_4 = new TableHeader();
                HeaderCol6_1_5_4.Name = "Số vụ việc tiếp nhiều lần";
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
                HeaderCol6_2_5_3.Name = "Số vụ việc tiếp lần đầu";
                HeaderCol6_2_5_3.ID = 39;
                HeaderCol6_2_5_3.ParentID = 28;
                HeaderCol6_2_5_3.Style = "";
                HeaderCol6_2_5.DataChild.Add(HeaderCol6_2_5_3);

                TableHeader HeaderCol6_2_5_4 = new TableHeader();
                HeaderCol6_2_5_4.Name = "Số vụ việc tiếp nhiều lần";
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
                RowItem RowItem2 = new RowItem(2 + tmp, "1=4+13+22", "", "", null, "text-align: center;width: 250px", ref DataArr);
                RowItem RowItem3 = new RowItem(3 + tmp, "2=5+14+23", "", "", null, "text-align: center;width: 250px", ref DataArr);
                RowItem RowItem4 = new RowItem(4 + tmp, "3=6+7+15+16+24+25", "", "", null, "text-align: center;width: 250px", ref DataArr);
                RowItem RowItem5 = new RowItem(5 + tmp, "4", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem6 = new RowItem(6 + tmp, "5", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem7 = new RowItem(7 + tmp, "6", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem8 = new RowItem(8 + tmp, "7", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem9 = new RowItem(9 + tmp, "8", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem10 = new RowItem(10 + tmp, "9", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem11 = new RowItem(11 + tmp, "10", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem12 = new RowItem(12 + tmp, "11", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem13 = new RowItem(13 + tmp, "12", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem14 = new RowItem(14 + tmp, "13", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem15 = new RowItem(15 + tmp, "14", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem16 = new RowItem(16 + tmp, "15", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem17 = new RowItem(17 + tmp, "16", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem18 = new RowItem(18 + tmp, "17", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem19 = new RowItem(19 + tmp, "18", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem20 = new RowItem(20 + tmp, "19", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem21 = new RowItem(21 + tmp, "20", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem22 = new RowItem(22 + tmp, "21", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem23 = new RowItem(23 + tmp, "22", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem24 = new RowItem(24 + tmp, "23", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem25 = new RowItem(25 + tmp, "24", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem26 = new RowItem(26 + tmp, "25", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem27 = new RowItem(27 + tmp, "26", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem28 = new RowItem(28 + tmp, "27", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem29 = new RowItem(29 + tmp, "28", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem30 = new RowItem(30 + tmp, "29", "", "", null, "text-align: center;width: 100px", ref DataArr);
                Row1.DataArr = DataArr;
                BaoCaoModel.DataTable.TableData.Add(Row1);

                List<TableData> data = new List<TableData>();
                data = new TCD01TT2024DAL().TCD01TT2024(p.ListCapID, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
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
        public BaseResultModel TCD01_TT2024_GetDSChiTietDonThu(string ContentRootPath, BaseReportParams p)
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
                tKDonThuInfos = new TCD01TT2024DAL().TCD01_TT2024_GetDSChiTietDonThu(ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now, p.Offset, p.PageSize, p.Index, p.XemTaiLieuMat, p.CanBoID, p.CoQuanID ?? 0, p.CapID);
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

        public BaseResultModel TCD01_TT2024_GetDSChiTietDonThu_XuatExcel(BaseReportParams p, string ContentRootPath, int CanBoDangNhapID)
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

        public BaseResultModel TCD01_TT2024_XuatExcel(BaseReportParams p, string ContentRootPath, int RoleID, int CapID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, int HuyenDangNhapID)
        {
            var Result = new BaseResultModel();
            try
            {
                List<TableData> data = new List<TableData>();
                data = new TCD01TT2024DAL().TCD01TT2024(p.ListCapID, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
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
                string path = @"Templates\FileTam\TCD01_" + CanBoDangNhapID + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";
                string urlExcel = new TCD01TT2024DAL().TCD01_TT2024_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
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
        public BaseResultModel XLD01_TT2024(BaseReportParams p, string ContentRootPath, int RoleID, int CapID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, int HuyenDangNhapID)
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
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Tổng số đơn phải xử lý", "", ref listTableHeader);
                //TableHeader HeaderCol3 = new TableHeader(3, 0, "Kỳ trước chuyển sang", "", ref listTableHeader);
                //TableHeader HeaderCol4 = new TableHeader(4, 0, "Tiếp nhận trong kỳ", "", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Số đơn đã xử lý", "", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Số đơn chưa xử lý (chuyển kỳ sau xử lý)", "", ref listTableHeader);
                TableHeader HeaderCol5 = new TableHeader(5, 0, "Đủ điều kiện xử lý", "", ref listTableHeader);
                TableHeader HeaderCol6 = new TableHeader(6, 0, "Phân loại đơn theo nội dung", "", ref listTableHeader);
                TableHeader HeaderCol7 = new TableHeader(7, 0, "Phân loại đơn theo tình trạng giải quyết", "", ref listTableHeader);
                TableHeader HeaderCol8 = new TableHeader(8, 0, "Kết quả xử lý đơn", "", ref listTableHeader);
                TableHeader HeaderCol9 = new TableHeader(9, 0, "Số văn bản phúc đáp nhận được do chuyển đơn", "", ref listTableHeader);
                //Cấp 2
                var DataChild2 = new List<TableHeader>();
                TableHeader HeaderCol21 = new TableHeader(21, 2, "Tổng số", "", ref DataChild2);
                TableHeader HeaderCol22 = new TableHeader(22, 2, "Kỳ trước chuyển sang", "", ref DataChild2);
                TableHeader HeaderCol23 = new TableHeader(23, 2, "Tiếp nhận trong kỳ", "", ref DataChild2);
                HeaderCol2.DataChild = DataChild2;

                var DataChild5 = new List<TableHeader>();
                TableHeader HeaderCol51 = new TableHeader(51, 5, "Số đơn", "", ref DataChild5);
                TableHeader HeaderCol52 = new TableHeader(52, 5, "Số vụ việc", "", ref DataChild5);
                HeaderCol5.DataChild = DataChild5;
               

                var DataChild6 = new List<TableHeader>();
                TableHeader HeaderCol61 = new TableHeader(61, 6, "Khiếu nại", "", ref DataChild6);
                TableHeader HeaderCol62 = new TableHeader(62, 6, "Tố cáo", "", ref DataChild6);
                TableHeader HeaderCol63 = new TableHeader(63, 6, "Kiến nghị, phản ánh", "", ref DataChild6);
                HeaderCol6.DataChild = DataChild6;

                var DataChild7 = new List<TableHeader>();
                TableHeader HeaderCol71 = new TableHeader(71, 7, "Đã giải quyết", "", ref DataChild7);
                TableHeader HeaderCol73 = new TableHeader(73, 7, "Chưa giải quyết xong", "", ref DataChild7);
                HeaderCol7.DataChild = DataChild7;

                var DataChild8 = new List<TableHeader>();
                TableHeader HeaderCol81 = new TableHeader(81, 8, "Đơn thuộc thẩm quyền", "", ref DataChild8);
                TableHeader HeaderCol82 = new TableHeader(82, 8, "Đơn không thuộc thẩm quyền", "", ref DataChild8);
                HeaderCol8.DataChild = DataChild8;
                //Cấp 3
                var DataChild71 = new List<TableHeader>();
                TableHeader HeaderCol711 = new TableHeader(711, 71, "Lần đầu", "", ref DataChild71);
                TableHeader HeaderCol712 = new TableHeader(712, 71, "Nhiều lần", "", ref DataChild71);
                HeaderCol71.DataChild = DataChild71;

                var DataChild81 = new List<TableHeader>();
                TableHeader HeaderCol811 = new TableHeader(811, 81, "Tổng số", "", ref DataChild81);
                TableHeader HeaderCol812 = new TableHeader(812, 81, "Khiếu nại", "", ref DataChild81);
                TableHeader HeaderCol813 = new TableHeader(813, 81, "Tố cáo", "", ref DataChild81);
                TableHeader HeaderCol814 = new TableHeader(814, 81, "Kiến nghị, phản ánh", "", ref DataChild81);
                HeaderCol81.DataChild = DataChild81;

                var DataChild82 = new List<TableHeader>();
                TableHeader HeaderCol821 = new TableHeader(821, 82, "Tổng số", "", ref DataChild82);
                TableHeader HeaderCol822 = new TableHeader(822, 82, "Hướng dẫn", "", ref DataChild82);
                TableHeader HeaderCol823 = new TableHeader(823, 82, "Chuyển đơn", "", ref DataChild82);
                TableHeader HeaderCol824 = new TableHeader(824, 82, "Đôn đốc giải quyết", "", ref DataChild82);
                HeaderCol82.DataChild = DataChild82;

                BaoCaoModel.DataTable.TableHeader = listTableHeader;
                #endregion
                #region TableData 
                TableData Row1 = new TableData();
                Row1.ID = 1;
                Row1.isClick = false;
                var DataArr = new List<RowItem>();
                RowItem RowItem1 = new RowItem(1, "Ms", "", "", null, "width: 215px", ref DataArr);
                RowItem RowItem2 = new RowItem(2, "1=2+3=4+5", "", "", null, "text-align: center;width: 250px", ref DataArr);
                RowItem RowItem3 = new RowItem(3, "2", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem4 = new RowItem(4, "3", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem5 = new RowItem(5, "4", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem6 = new RowItem(6, "5", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem7 = new RowItem(7, "6= 8+9+10= 11+12+13= 14+18", "", "", null, "text-align: center;width: 250px", ref DataArr);
                RowItem RowItem8 = new RowItem(8, "7", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem9 = new RowItem(9, "8", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem10 = new RowItem(10, "9", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem11 = new RowItem(11, "10", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem12 = new RowItem(12, "11", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem13 = new RowItem(13, "12", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem14 = new RowItem(14, "13", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem15 = new RowItem(15, "14=15+16+17", "", "", null, "text-align: center;width: 250px", ref DataArr);
                RowItem RowItem16 = new RowItem(16, "15", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem17 = new RowItem(17, "16", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem18 = new RowItem(18, "17", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem19 = new RowItem(19, "18 = 19+20+21", "", "", null, "text-align: center;width: 250px", ref DataArr);
                RowItem RowItem20 = new RowItem(20, "19", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem21 = new RowItem(21, "20", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem22 = new RowItem(22, "21", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem23 = new RowItem(23, "22", "", "", null, "text-align: center;width: 100px", ref DataArr);                

                Row1.DataArr = DataArr;
                BaoCaoModel.DataTable.TableData.Add(Row1);

                List<TableData> data = new List<TableData>();
                data = new XLD01_TT2024DAL().XLD01_TT2024(p.ListCapID, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
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
        public BaseResultModel XLD01_TT2024_XuatExcel(BaseReportParams p, string ContentRootPath, int RoleID, int CapID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, int HuyenDangNhapID)
        {
            var Result = new BaseResultModel();
            try
            {
                List<TableData> data = new List<TableData>();
                data = new XLD01_TT2024DAL().XLD01_TT2024(p.ListCapID, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
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
                string urlExcel = new XLD01_TT2024DAL().XLD01_TT2024_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
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

        public BaseResultModel XLD01_TT22024_GetDSChiTietDonThu(string ContentRootPath, BaseReportParams p)
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
                tKDonThuInfos = new XLD01_TT2024DAL().XLD01_TT2024_GetDSChiTietDonThu(ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now, p.Offset, p.PageSize, p.Index, p.XemTaiLieuMat, p.CanBoID, p.CoQuanID ?? 0, p.CapID);
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

        public BaseResultModel XLD01_TT22024_GetDSChiTietDonThu_XuatExcel(BaseReportParams p, string ContentRootPath, int CanBoDangNhapID)
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
        public BaseResultModel XLD02_TT2024(BaseReportParams p, string ContentRootPath, int RoleID, int CapID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, int HuyenDangNhapID)
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
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Tổng số đơn phải xử lý", "", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Đơn đã xử lý ", "", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Đủ điều kiện xử lý", "", ref listTableHeader);
                TableHeader HeaderCol5 = new TableHeader(5, 0, "Phân loại vụ việc khiếu nại theo nội dung", "", ref listTableHeader);
                TableHeader HeaderCol6 = new TableHeader(6, 0, "Phân loại vụ việc khiếu nại theo tình trạng giải quyết", "", ref listTableHeader);
                TableHeader HeaderCol7 = new TableHeader(7, 0, "Kết quả xử lý đơn", "", ref listTableHeader);
                TableHeader HeaderCol8 = new TableHeader(8, 0, "Số văn bản phúc đáp nhận được do chuyển đơn", "", ref listTableHeader);
                //Cấp 2
                var DataChild2 = new List<TableHeader>();
                TableHeader HeaderCol21 = new TableHeader(21, 2, "Tổng số", "", ref DataChild2);
                TableHeader HeaderCol22 = new TableHeader(22, 2, "Số đơn kỳ trước chuyển sang", "", ref DataChild2);
                TableHeader HeaderCol23 = new TableHeader(23, 2, "Số đơn tiếp nhận trong kỳ", "", ref DataChild2);
                HeaderCol2.DataChild = DataChild2;

                var DataChild3 = new List<TableHeader>();
                TableHeader HeaderCol31 = new TableHeader(31, 3, "Tổng", "", ref DataChild3);
                TableHeader HeaderCol32 = new TableHeader(32, 3, "Đơn kỳ trước chuyển sang", "", ref DataChild3);
                TableHeader HeaderCol33 = new TableHeader(33, 3, "Đơn tiếp nhận trong kỳ", "", ref DataChild3);
                HeaderCol3.DataChild = DataChild3;

                var DataChild4 = new List<TableHeader>();
                TableHeader HeaderCol41 = new TableHeader(41,4, "Số đơn", "", ref DataChild4);
                TableHeader HeaderCol42 = new TableHeader(42,4, "Số vụ việc", "", ref DataChild4);
                HeaderCol4.DataChild = DataChild4;

                var DataChild5 = new List<TableHeader>();
                TableHeader HeaderCol51 = new TableHeader(51, 5, "Lĩnh vực hành chính", "", ref DataChild5);
                TableHeader HeaderCol52 = new TableHeader(52, 5, "Lĩnh vực tư pháp", "", ref DataChild5);
                TableHeader HeaderCol53 = new TableHeader(53, 5, "Lĩnh vực Đảng, đoàn thể", "", ref DataChild5);
                TableHeader HeaderCol54 = new TableHeader(54, 5, "Lĩnh vực khác", "", ref DataChild5);
                HeaderCol5.DataChild = DataChild5;

                var DataChild6 = new List<TableHeader>();
                TableHeader HeaderCol61 = new TableHeader(61, 6, "Đã được giải quyết", "", ref DataChild6);
                TableHeader HeaderCol62 = new TableHeader(62, 6, "Chưa giải quyết xong", "", ref DataChild6);
                HeaderCol6.DataChild = DataChild6;

                var DataChild7 = new List<TableHeader>();
                TableHeader HeaderCol71 = new TableHeader(71, 7, "Vụ việc thuộc thẩm quyền", "", ref DataChild7);
                TableHeader HeaderCol72 = new TableHeader(72, 7, "Vụ việc không thuộc thẩm quyền", "", ref DataChild7);
                HeaderCol7.DataChild = DataChild7;
                //Cấp 3
                var DataChild51 = new List<TableHeader>();
                TableHeader HeaderCol511 = new TableHeader(511, 51, "Tổng", "", ref DataChild51);
                TableHeader HeaderCol512 = new TableHeader(512, 51, "Chế độ, chính sách", "", ref DataChild51);
                TableHeader HeaderCol513 = new TableHeader(513, 51, "Đất đai, nhà cửa", "", ref DataChild51);
                TableHeader HeaderCol514 = new TableHeader(514, 51, "Khác", "", ref DataChild51);
                HeaderCol51.DataChild = DataChild51;

                var DataChild61 = new List<TableHeader>();
                TableHeader HeaderCol611 = new TableHeader(611, 61, "Lần đầu", "", ref DataChild61);
                TableHeader HeaderCol612 = new TableHeader(612, 61, "Lần 2", "", ref DataChild61);
                TableHeader HeaderCol613 = new TableHeader(613, 61, "Đã có bản án của tòa", "", ref DataChild61);
                HeaderCol61.DataChild = DataChild61;

                var DataChild71 = new List<TableHeader>();
                TableHeader HeaderCol711 = new TableHeader(711, 71, "Tổng", "", ref DataChild71);
                TableHeader HeaderCol712 = new TableHeader(712, 71, "Lần đầu", "", ref DataChild71);
                TableHeader HeaderCol713 = new TableHeader(713, 71, "Lần 2", "", ref DataChild71);
                HeaderCol71.DataChild = DataChild71;

                var DataChild72 = new List<TableHeader>();
                TableHeader HeaderCol721 = new TableHeader(721, 72, "Tổng", "", ref DataChild72);
                TableHeader HeaderCol722 = new TableHeader(722, 72, "Hướng dẫn", "", ref DataChild72);
                TableHeader HeaderCol723 = new TableHeader(723, 72, "Đôn đổc giải quyết", "", ref DataChild72);
                HeaderCol72.DataChild = DataChild72;

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
                RowItem RowItem5 = new RowItem(5, "4=5+6", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem6 = new RowItem(6, "5", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem7 = new RowItem(7, "6", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem8 = new RowItem(8, "7", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem9 = new RowItem(9, "8", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem10 = new RowItem(10, "9=10+11+12", "", "", null, "text-align: center;width: 250px", ref DataArr);
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
                RowItem RowItem21 = new RowItem(21, "20=21+22", "", "", null, "text-align: center;width: 250px", ref DataArr);
                RowItem RowItem22 = new RowItem(22, "21", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem23 = new RowItem(23, "22", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem24 = new RowItem(24, "23=24+25", "", "", null, "text-align: center;width: 250px", ref DataArr);
                RowItem RowItem25 = new RowItem(25, "24", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem26 = new RowItem(26, "25", "", "", null, "text-align: center;width: 100px", ref DataArr);               
                RowItem RowItem27 = new RowItem(26, "26", "", "", null, "text-align: center;width: 100px", ref DataArr);               


                Row1.DataArr = DataArr;
                BaoCaoModel.DataTable.TableData.Add(Row1);

                List<TableData> data = new List<TableData>();
                data = new XLD02_TT2024DAL().XLD02_TT2024(p.ListCapID, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
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

        public BaseResultModel XLD02_TT2024_XuatExcel(BaseReportParams p, string ContentRootPath, int RoleID, int CapID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, int HuyenDangNhapID)
        {
            var Result = new BaseResultModel();
            try
            {
                List<TableData> data = new List<TableData>();
                data = new XLD02_TT2024DAL().XLD02_TT2024(p.ListCapID, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
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
                string urlExcel = new XLD02_TT2024DAL().XLD02_TT2024_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
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

        public BaseResultModel XLD02_TT2024_GetDSChiTietDonThu(string ContentRootPath, BaseReportParams p)
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
                tKDonThuInfos = new XLD02_TT2024DAL().XLD02_TT2024_GetDSChiTietDonThu(ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now, p.Offset, p.PageSize, p.Index, p.XemTaiLieuMat, p.CanBoID, p.CoQuanID ?? 0, p.CapID);
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

        public BaseResultModel XLD02_TT2024_GetDSChiTietDonThu_XuatExcel(BaseReportParams p, string ContentRootPath, int CanBoDangNhapID)
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
        public BaseResultModel XLD03_TT2024(BaseReportParams p, string ContentRootPath, int RoleID, int CapID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, int HuyenDangNhapID)
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
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Tổng số đơn phải xử lý", "", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Đơn đã xử lý", "", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Đủ điều kiện xử lý", "", ref listTableHeader);
                TableHeader HeaderCol5 = new TableHeader(5, 0, "Phân loại vụ việc tố cáo theo nội dung", "", ref listTableHeader);
                TableHeader HeaderCol6 = new TableHeader(6, 0, "Phân loại vụ việc theo tình trạng giải quyết", "", ref listTableHeader);
                TableHeader HeaderCol7 = new TableHeader(7, 0, "Kết quả xử lý", "", ref listTableHeader);
                TableHeader HeaderCol8 = new TableHeader(8, 0, "Số văn bản phúc đáp nhận được do chuyển đơn", "", ref listTableHeader);
                //Cấp 2
                var DataChild2 = new List<TableHeader>();
                TableHeader HeaderCol21 = new TableHeader(21, 2, "Tổng số", "", ref DataChild2);
                TableHeader HeaderCol22 = new TableHeader(22, 2, "Số đơn kỳ trước chuyển sang", "", ref DataChild2);
                TableHeader HeaderCol23 = new TableHeader(23, 2, "Số đơn tiếp nhận trong kỳ", "", ref DataChild2);
                HeaderCol2.DataChild = DataChild2;                

                var DataChild3 = new List<TableHeader>();
                TableHeader HeaderCol31 = new TableHeader(31, 3, "Tổng", "", ref DataChild3);
                TableHeader HeaderCol32 = new TableHeader(32, 3, "Đơn kỳ trước chuyển sang", "", ref DataChild3);
                TableHeader HeaderCol33 = new TableHeader(33, 3, "Đơn tiếp nhận trong kỳ", "", ref DataChild3);
                HeaderCol3.DataChild = DataChild3;

                var DataChild4 = new List<TableHeader>();
                TableHeader HeaderCol41 = new TableHeader(41, 4, "Số đơn", "", ref DataChild4);
                TableHeader HeaderCol42 = new TableHeader(42, 4, "Số vụ việc", "", ref DataChild4);
                HeaderCol4.DataChild = DataChild4;

                //var DataChild6 = new List<TableHeader>();
                //TableHeader HeaderCol61 = new TableHeader(61, 6, "Lĩnh vực hành chính", "", ref DataChild6);
                //TableHeader HeaderCol62 = new TableHeader(62, 6, "Lĩnh vực tư pháp", "", ref DataChild6);
                //TableHeader HeaderCol63 = new TableHeader(63, 6, "Lĩnh vực Đảng, đoàn thể", "", ref DataChild6);
                //TableHeader HeaderCol64 = new TableHeader(64, 6, "Lĩnh vực khác", "", ref DataChild6);
                //HeaderCol6.DataChild = DataChild6;

                var DataChild5 = new List<TableHeader>();
                TableHeader HeaderCol51 = new TableHeader(51, 5, "Lĩnh vực hành chính", "", ref DataChild5);
                TableHeader HeaderCol52 = new TableHeader(52, 5, "Tham nhũng", "", ref DataChild5);
                TableHeader HeaderCol53 = new TableHeader(53, 5, "Lĩnh vực tư pháp", "", ref DataChild5);
                TableHeader HeaderCol54 = new TableHeader(54, 5, "Lĩnh vực Đảng, đoàn thể", "", ref DataChild5);
                TableHeader HeaderCol55 = new TableHeader(55, 5, "Lĩnh vực khác", "", ref DataChild5);
                HeaderCol5.DataChild = DataChild5;

                var DataChild6 = new List<TableHeader>();
                TableHeader HeaderCol61 = new TableHeader(61, 6, "Tố cáo tiếp", "", ref DataChild6);
                TableHeader HeaderCol62 = new TableHeader(62, 6, "Chưa giải quyết", "", ref DataChild6);
                HeaderCol6.DataChild = DataChild6;

                var DataChild7 = new List<TableHeader>();
                TableHeader HeaderCol71 = new TableHeader(71, 7, "Vụ việc thuộc thẩm quyền", "", ref DataChild7);
                TableHeader HeaderCol72 = new TableHeader(72, 7, "Vụ việc không thuộc thẩm quyền", "", ref DataChild7);
                HeaderCol7.DataChild = DataChild7;
                //Cấp 3
                //var DataChild61 = new List<TableHeader>();
                //TableHeader HeaderCol611 = new TableHeader(611, 61, "Tổng", "", ref DataChild61);
                //TableHeader HeaderCol612 = new TableHeader(612, 61, "Chế độ, chính sách", "", ref DataChild61);
                //TableHeader HeaderCol613 = new TableHeader(613, 61, "Đất đai, nhà cửa", "", ref DataChild61);
                //TableHeader HeaderCol614 = new TableHeader(613, 61, "Khác", "", ref DataChild61);
                //HeaderCol61.DataChild = DataChild61;

                var DataChild51 = new List<TableHeader>();
                TableHeader HeaderCol511 = new TableHeader(511, 51, "Tổng cộng", "", ref DataChild51);
                TableHeader HeaderCol512 = new TableHeader(512, 51, "Chế độ, chính sách", "", ref DataChild51);
                TableHeader HeaderCol513 = new TableHeader(513, 51, "Đất đai, nhà cửa", "", ref DataChild51);
                TableHeader HeaderCol514 = new TableHeader(514, 51, "Công chức, công vụ", "", ref DataChild51);
                TableHeader HeaderCol515 = new TableHeader(515, 51, "Khác", "", ref DataChild51);
                HeaderCol51.DataChild = DataChild51;

                var DataChild61 = new List<TableHeader>();
                TableHeader HeaderCol611 = new TableHeader(611, 61, "Quá thời hạn chưa giải quyết", "", ref DataChild61);
                TableHeader HeaderCol612 = new TableHeader(612, 61, "Đã có kết luận giải quyết", "", ref DataChild61);
                HeaderCol61.DataChild = DataChild61;

                var DataChild71 = new List<TableHeader>();
                TableHeader HeaderCol711 = new TableHeader(711, 71, "Tổng số", "", ref DataChild71);
                TableHeader HeaderCol712 = new TableHeader(712, 71, "Tố cáo lần đầu", "", ref DataChild71);
                TableHeader HeaderCol713 = new TableHeader(713, 71, "Tố cáo tiếp", "", ref DataChild71);
                HeaderCol71.DataChild = DataChild71;

                var DataChild72 = new List<TableHeader>();
                TableHeader HeaderCol721 = new TableHeader(721, 71, "Tổng số", "", ref DataChild72);
                TableHeader HeaderCol722 = new TableHeader(722, 71, "Chuyển đơn", "", ref DataChild72);
                TableHeader HeaderCol723 = new TableHeader(723, 71, "Đôn đốc giải quyết", "", ref DataChild72);
                HeaderCol72.DataChild = DataChild72;

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
                RowItem RowItem5 = new RowItem(5, "4=5+6", "", "", null, "text-align: center;width: 250px", ref DataArr);
                RowItem RowItem6 = new RowItem(6, "5", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem7 = new RowItem(7, "6", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem8 = new RowItem(8, "7", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem9 = new RowItem(9, "8=9+14+15+16+17=18+19+20=21+24", "", "", null, "text-align: center;width: 250px", ref DataArr);
                RowItem RowItem10 = new RowItem(10, "9=10+11+12+13", "", "", null, "text-align: center;width: 250px", ref DataArr);
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
                RowItem RowItem22 = new RowItem(22, "21=22+23", "", "", null, "text-align: center;width: 250px", ref DataArr);
                RowItem RowItem23 = new RowItem(23, "22", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem24 = new RowItem(24, "23", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem25 = new RowItem(25, "24=25+26", "", "", null, "text-align: center;width: 250px", ref DataArr);
                RowItem RowItem26 = new RowItem(26, "25", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem27 = new RowItem(27, "26", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem28 = new RowItem(28, "27", "", "", null, "text-align: center;width: 100px", ref DataArr);                


                Row1.DataArr = DataArr;
                BaoCaoModel.DataTable.TableData.Add(Row1);

                List<TableData> data = new List<TableData>();
                data = new XLD03_TT2024DAL().XLD03_TT2024(p.ListCapID, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
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

        public BaseResultModel XLD03_TT2024_XuatExcel(BaseReportParams p, string ContentRootPath, int RoleID, int CapID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, int HuyenDangNhapID)
        {
            var Result = new BaseResultModel();
            try
            {
                List<TableData> data = new List<TableData>();
                data = new XLD03_TT2024DAL().XLD03_TT2024(p.ListCapID, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
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
                string urlExcel = new XLD03_TT2024DAL().XLD03_TT2024_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
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

        public BaseResultModel XLD03_TT2024_GetDSChiTietDonThu(string ContentRootPath, BaseReportParams p)
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
                tKDonThuInfos = new XLD03_TT2024DAL().XLD03_TT2024_GetDSChiTietDonThu(ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now, p.Offset, p.PageSize, p.Index, p.XemTaiLieuMat, p.CanBoID, p.CoQuanID ?? 0, p.CapID);
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

        public BaseResultModel XLD03_TT2024_GetDSChiTietDonThu_XuatExcel(BaseReportParams p, string ContentRootPath, int CanBoDangNhapID)
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
        public BaseResultModel XLD04_TT2024(BaseReportParams p, string ContentRootPath, int RoleID, int CapID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, int HuyenDangNhapID)
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
                TableHeader HeaderCol2 = new TableHeader(2, 0, "Tổng số đơn phải xử lý", "", ref listTableHeader);
                TableHeader HeaderCol3 = new TableHeader(3, 0, "Đơn đã xem xét về điều kiện xử lý", "", ref listTableHeader);
                TableHeader HeaderCol4 = new TableHeader(4, 0, "Đơn đủ điều kiện xử lý", "", ref listTableHeader);
                TableHeader HeaderCol5 = new TableHeader(5, 0, "Phân loại vụ việc theo nội dung", "", ref listTableHeader);
                TableHeader HeaderCol6 = new TableHeader(6, 0, "Phân loại vụ việc theo tình trạng giải quyết", "", ref listTableHeader);
                TableHeader HeaderCol7 = new TableHeader(7, 0, "Kết quả xử lý đơn", "", ref listTableHeader);
                TableHeader HeaderCol8 = new TableHeader(8, 0, "Kết quả giải quyết vụ việc thuộc thẩm quyền", "", ref listTableHeader);
                
                //Cấp 2
                var DataChild2 = new List<TableHeader>();
                TableHeader HeaderCol21 = new TableHeader(21, 2, "Tổng số đơn", "", ref DataChild2);
                TableHeader HeaderCol22 = new TableHeader(22, 2, "Số đơn kỳ trước chuyển sang", "", ref DataChild2);
                TableHeader HeaderCol23 = new TableHeader(23, 2, "Số đơn tiếp nhận trong kỳ", "", ref DataChild2);
                HeaderCol2.DataChild = DataChild2;

                var DataChild3 = new List<TableHeader>();
                TableHeader HeaderCol31 = new TableHeader(31, 3, "Tổng số đơn", "", ref DataChild3);
                TableHeader HeaderCol32 = new TableHeader(32, 3, "Số đơn kỳ trước chuyển sang", "", ref DataChild3);
                TableHeader HeaderCol33 = new TableHeader(33, 3, "Số đơn tiếp nhận trong kỳ", "", ref DataChild3);
                HeaderCol3.DataChild = DataChild3;

                var DataChild4 = new List<TableHeader>();
                TableHeader HeaderCol41 = new TableHeader(41, 4, "Số đơn", "", ref DataChild4);
                TableHeader HeaderCol42 = new TableHeader(42, 4, "Số vụ việc", "", ref DataChild4);
                HeaderCol4.DataChild = DataChild4;

                var DataChild5 = new List<TableHeader>();
                TableHeader HeaderCol51 = new TableHeader(51, 5, "Chế độ, chính sách", "", ref DataChild5);
                TableHeader HeaderCol52 = new TableHeader(52, 5, "Đất đai", "", ref DataChild5);
                TableHeader HeaderCol53 = new TableHeader(53, 5, "Tư pháp", "", ref DataChild5);
                TableHeader HeaderCol54 = new TableHeader(54, 5, "Khác", "", ref DataChild5);
                HeaderCol5.DataChild = DataChild5;

                var DataChild6 = new List<TableHeader>();
                TableHeader HeaderCol61 = new TableHeader(61, 6, "Đã được giải quyết", "", ref DataChild6);
                TableHeader HeaderCol62 = new TableHeader(62, 6, "Chưa được giải quyết", "", ref DataChild6);
                HeaderCol6.DataChild = DataChild6;

                var DataChild7 = new List<TableHeader>();
                TableHeader HeaderCol71 = new TableHeader(71, 7, "Vụ việc thuộc thẩm quyền", "", ref DataChild7);
                TableHeader HeaderCol72 = new TableHeader(72, 7, "Vụ việc không thuộc thẩm quyền", "", ref DataChild7);
                HeaderCol7.DataChild = DataChild7;

                var DataChild8 = new List<TableHeader>();
                TableHeader HeaderCol81 = new TableHeader(81, 8, "Vụ việc đã giải quyết", "", ref DataChild8);
                TableHeader HeaderCol82 = new TableHeader(82, 8, "Vụ việc chưa giải quyết", "", ref DataChild8);
                HeaderCol8.DataChild = DataChild8;
                //Cấp 3
                var DataChild72 = new List<TableHeader>();
                TableHeader HeaderCol721 = new TableHeader(721, 72, "Tổng số", "", ref DataChild72);
                TableHeader HeaderCol722 = new TableHeader(722, 72, "Chuyển đơn", "", ref DataChild72);
                TableHeader HeaderCol723 = new TableHeader(723, 72, "Đôn đổc giải quyết", "", ref DataChild72);
                HeaderCol72.DataChild = DataChild72;

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
                RowItem RowItem5 = new RowItem(5, "4 =5+6", "", "", null, "text-align: center;width: 250px", ref DataArr);
                RowItem RowItem6 = new RowItem(6, "5", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem7 = new RowItem(7, "6", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem8 = new RowItem(8, "7", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem9 = new RowItem(9, "8=9+10+11+12=13+14=15+18", "", "", null, "text-align: center;width: 250px", ref DataArr);
                RowItem RowItem10 = new RowItem(10, "9", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem11 = new RowItem(11, "10", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem12 = new RowItem(12, "11", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem13 = new RowItem(13, "12", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem14 = new RowItem(14, "13", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem15 = new RowItem(15, "14", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem16 = new RowItem(16, "15", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem17 = new RowItem(17, "16 =17+18", "", "", null, "text-align: center;width: 250px", ref DataArr);
                RowItem RowItem18 = new RowItem(18, "17", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem19 = new RowItem(19, "18", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem20 = new RowItem(20, "19", "", "", null, "text-align: center;width: 100px", ref DataArr);
                RowItem RowItem21 = new RowItem(21, "20", "", "", null, "text-align: center;width: 100px", ref DataArr);              

                Row1.DataArr = DataArr;
                BaoCaoModel.DataTable.TableData.Add(Row1);

                List<TableData> data = new List<TableData>();
                data = new XLD04_TT2024DAL().XLD04_TT2024(p.ListCapID, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
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

        public BaseResultModel XLD04_TT2024_XuatExcel(BaseReportParams p, string ContentRootPath, int RoleID, int CapID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, int HuyenDangNhapID)
        {
            var Result = new BaseResultModel();
            try
            {
                List<TableData> data = new List<TableData>();
                data = new XLD04_TT2024DAL().XLD04_TT2024(p.ListCapID, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
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
                string urlExcel = new XLD04_TT2024DAL().XLD04_TT2024_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
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

        public BaseResultModel XLD04_TT2024_GetDSChiTietDonThu(string ContentRootPath, BaseReportParams p)
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
                tKDonThuInfos = new XLD04_TT2024DAL().XLD04_TT2024_GetDSChiTietDonThu(ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now, p.Offset, p.PageSize, p.Index, p.XemTaiLieuMat, p.CanBoID, p.CoQuanID ?? 0, p.CapID);
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
        public BaseResultModel KQGQ01_TT2024(BaseReportParams p, string ContentRootPath, int RoleID, int CapID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, int HuyenDangNhapID)
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

        public BaseResultModel KQGQ01_TT2024_XuatExcel(BaseReportParams p, string ContentRootPath, int RoleID, int CapID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, int HuyenDangNhapID)
        {
            var Result = new BaseResultModel();
            try
            {
                List<TableData> data = new List<TableData>();
                data = new KQGQ01_TT2024DAL().KQGQ01_TT2024(p.ListCapID, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
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
                string urlExcel = new KQGQ01_TT2024DAL().KQGQ01_TT2024_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
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

        public BaseResultModel KQGQ01_TT2024_GetDSChiTietDonThu(string ContentRootPath, BaseReportParams p)
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

        public BaseResultModel KQGQ01_TT2024_GetDSChiTietDonThu_XuatExcel(BaseReportParams p, string ContentRootPath, int CanBoDangNhapID)
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
        public BaseResultModel KQGQ02_TT2024(BaseReportParams p, string ContentRootPath, int RoleID, int CapID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, int HuyenDangNhapID)
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
                data = new KQGQ02_TT2024DAL().KQGQ02_TT2024(p.ListCapID, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
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

        public BaseResultModel KQGQ02_TT2024_XuatExcel(BaseReportParams p, string ContentRootPath, int RoleID, int CapID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, int HuyenDangNhapID)
        {
            var Result = new BaseResultModel();
            try
            {
                List<TableData> data = new List<TableData>();
                data = new KQGQ02_TT2024DAL().KQGQ02_TT2024(p.ListCapID, ContentRootPath, RoleID, CapID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
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
                string urlExcel = new KQGQ02_TT2024DAL().KQGQ02_TT2024_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
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

        public BaseResultModel KQGQ02_TT2024_GetDSChiTietDonThu(string ContentRootPath, BaseReportParams p)
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
                tKDonThuInfos = new KQGQ02_TT2024DAL().KQGQ02_TT2024_GetDSChiTietDonThu(ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now, p.Offset, p.PageSize, p.Index, p.XemTaiLieuMat, p.CanBoID, p.CoQuanID ?? 0, p.CapID);
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

        public BaseResultModel KQGQ02_TT2024_GetDSChiTietDonThu_XuatExcel(BaseReportParams p, string ContentRootPath, int CanBoDangNhapID)
        {
            var Result = new BaseResultModel();
            try
            {
                List<TableData> data = new List<TableData>();
                List<TKDonThuInfo> tKDonThuInfos = new KQGQ02_TT2024DAL().KQGQ02_TT2024_GetDSChiTietDonThu(ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now, p.Offset, p.PageSize, p.Index, p.XemTaiLieuMat, p.CanBoID, p.CoQuanID ?? 0, p.CapID);
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

    }
}
