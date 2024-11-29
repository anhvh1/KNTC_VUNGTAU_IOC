using Com.Gosol.KNTC.DAL.KNTC;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.BaoCao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Gosol.KNTC.Ultilities;
using Com.Gosol.KNTC.DAL.BaoCao;
using DocumentFormat.OpenXml.Wordprocessing;
using TableHeader = Com.Gosol.KNTC.Models.BaoCao.TableHeader;
using System.Data;
using DataTable = Com.Gosol.KNTC.Models.BaoCao.DataTable;
using OfficeOpenXml;
using System.IO;

namespace Com.Gosol.KNTC.BUS.KNTC
{
    public class QuanLyNhapLieuBUS
    {
        public BaoCaoModel GetBySearch(ref int TotalRow, BasePagingParamsForFilter p)
        {
            var coQuanList = new ThongKeNhapLieu().GetByTime(p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
            if (p.CoQuanID > 0)
            {
                coQuanList = coQuanList.Where(x => x.CoQuanID == p.CoQuanID).ToList();
            }
            //chua nhap lieu
            if (p.TrangThai == 1)
            {
                coQuanList = coQuanList.Where(x => x.SLTiepDan == 0 && x.SLDonThu == 0 && x.SLXuLyDon == 0 && x.SLGiaiQuyetDon == 0).ToList();
            }
            else if (p.TrangThai == 2)
            {
                coQuanList = coQuanList.Where(x => x.SLTiepDan != 0 || x.SLDonThu != 0 || x.SLXuLyDon != 0 || x.SLGiaiQuyetDon != 0).ToList();
            }

            BaoCaoModel BaoCaoModel = new BaoCaoModel();
            BaoCaoModel.BieuSo = "";
            BaoCaoModel.ThongTinSoLieu = "Từ ngày " + (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy") + " đến ngày " + (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
            BaoCaoModel.TuNgay = (p.TuNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
            BaoCaoModel.DenNgay = (p.DenNgay ?? DateTime.Now).ToString("dd/MM/yyyy");
            BaoCaoModel.Title = "QUẢN LÝ NHẬP LIỆU";
            BaoCaoModel.DataTable = new DataTable();
            BaoCaoModel.DataTable.TableHeader = new List<TableHeader>();
            BaoCaoModel.DataTable.TableData = new List<TableData>();

            #region Header
            var listTableHeader = new List<TableHeader>();
            TableHeader HeaderCol1 = new TableHeader(1, 0, "STT", "width: 50px", ref listTableHeader);
            TableHeader HeaderCol2 = new TableHeader(2, 0, "Đơn vị", "", ref listTableHeader);
            TableHeader HeaderCol3 = new TableHeader(3, 0, "Tiếp công dân", "", ref listTableHeader);
            TableHeader HeaderCol4 = new TableHeader(3, 0, "Tiếp nhận đơn", "", ref listTableHeader);
            TableHeader HeaderCol5 = new TableHeader(4, 0, "Xử lý đơn", "", ref listTableHeader);
            TableHeader HeaderCol6 = new TableHeader(5, 0, "Giải quyết đơn", "", ref listTableHeader);
            BaoCaoModel.DataTable.TableHeader = listTableHeader;
            #endregion
            #region TableData 
            //Row1.ID = 1;
            //Row1.isClick = false;          
            List<TableData> data = new List<TableData>();
            int stt = 0;
            //foreach (ThongKeNhapLieuInfo cq in coQuanList)
            //{
            //    TableData tableData = new TableData();
            //    tableData.ID = stt++;
            //    var DataArr = new List<RowItem>();
            //    RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "text-align: center;width: 50px", ref DataArr);
            //    RowItem RowItem2 = new RowItem(2, cq.TenDonVi, "", "", null, "text-align: left;", ref DataArr);
            //    RowItem RowItem3 = new RowItem(3, cq.SLTiepDan.ToString(), "", "", null, "text-align: center;", ref DataArr);
            //    RowItem RowItem4 = new RowItem(4, cq.SLDonThu.ToString(), "", "", null, "text-align: center;", ref DataArr);
            //    RowItem RowItem5 = new RowItem(5, cq.SLXuLyDon.ToString(), "", "", null, "text-align: center;", ref DataArr);
            //    RowItem RowItem6 = new RowItem(6, cq.SLGiaiQuyetDon.ToString(), "", "", null, "text-align: center;", ref DataArr);

            //    tableData.DataArr = DataArr;
            //    data.Add(tableData);
            //}
            var totalSLTiepDan = coQuanList.Sum(x => x.SLTiepDan).ToString();
            var totalSLDonThu = coQuanList.Sum(x => x.SLDonThu).ToString();
            var totalSLXuLyDon = coQuanList.Sum(x => x.SLXuLyDon).ToString();
            var totalSLGiaiQuyetDon = coQuanList.Sum(x => x.SLGiaiQuyetDon).ToString();
            var DanhSachHuyen = coQuanList.Where(x => x.CapID == (int)CapQuanLy.CapUBNDHuyen).ToList();
            var DanhSachPhongThuocTinh = coQuanList.Where(x => x.CapID == (int)CapQuanLy.CapSoNganh || x.CoQuanChaID == x.CoQuanID).ToList();

            var DataArrTong = new List<RowItem>();
            TableData tableDataTong = new TableData();
            tableDataTong.ID = stt++;
            RowItem RowItem1H = new RowItem(1, stt.ToString(), "", "", null, "text-align: center;width: 50px", ref DataArrTong);
            if (p.CoQuanID > 0)
            {
                RowItem RowItem2H = new RowItem(2, coQuanList[0].TenDonVi, "", "", null, "text-align: left; font-weight : bold", ref DataArrTong);
            }
            else
            {
                RowItem RowItem2H = new RowItem(2, "Toàn tỉnh", "", "", null, "text-align: left; font-weight : bold", ref DataArrTong);
            }            
            RowItem RowItem3H = new RowItem(3, totalSLTiepDan, "", "", null, "text-align: center;", ref DataArrTong);
            RowItem RowItem4H = new RowItem(4, totalSLDonThu, "", "", null, "text-align: center;", ref DataArrTong);
            RowItem RowItem5H = new RowItem(5, totalSLXuLyDon, "", "", null, "text-align: center;", ref DataArrTong);
            RowItem RowItem6H = new RowItem(6, totalSLGiaiQuyetDon, "", "", null, "text-align: center;", ref DataArrTong);
            tableDataTong.DataArr = DataArrTong;
            data.Add(tableDataTong);

            foreach (var ptt in DanhSachPhongThuocTinh)
            {
                if (stt == 1 && p.CoQuanID is null)
                {
                    var DataArrCapTinh = new List<RowItem>();
                    TableData tableDataCapTinh = new TableData();
                    tableDataCapTinh.ID = stt++;
                    RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "text-align: center;width: 50px", ref DataArrCapTinh);
                    RowItem RowItem2 = new RowItem(2, "Cấp tỉnh", "", "", null, "text-align: left; font-weight : bold", ref DataArrCapTinh);
                    RowItem RowItem3 = new RowItem(3, coQuanList.Where(y => y.CapID == (int)CapQuanLy.CapSoNganh || y.CoQuanChaID == y.CoQuanID).Sum(x => x.SLTiepDan).ToString(), "", "", null, "text-align: center;", ref DataArrCapTinh);
                    RowItem RowItem4 = new RowItem(4, coQuanList.Where(y => y.CapID == (int)CapQuanLy.CapSoNganh || y.CoQuanChaID == y.CoQuanID).Sum(x => x.SLDonThu).ToString(), "", "", null, "text-align: center;", ref DataArrCapTinh);
                    RowItem RowItem5 = new RowItem(5, coQuanList.Where(y => y.CapID == (int)CapQuanLy.CapSoNganh || y.CoQuanChaID == y.CoQuanID).Sum(x => x.SLXuLyDon).ToString(), "", "", null, "text-align: center;", ref DataArrCapTinh);
                    RowItem RowItem6 = new RowItem(6, coQuanList.Where(y => y.CapID == (int)CapQuanLy.CapSoNganh || y.CoQuanChaID == y.CoQuanID).Sum(x => x.SLGiaiQuyetDon).ToString(), "", "", null, "text-align: center;", ref DataArrCapTinh);
                    tableDataCapTinh.DataArr = DataArrCapTinh;
                    data.Add(tableDataCapTinh);
                }
                else if(p.CoQuanID is null)
                {
                    var DataArrPhongThuocTinh = new List<RowItem>();
                    TableData tableDataPhongThuocTinh = new TableData();
                    tableDataPhongThuocTinh.ID = stt++;
                    RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "text-align: center;width: 50px", ref DataArrPhongThuocTinh);
                    RowItem RowItem2 = new RowItem(2, ptt.TenDonVi, "", "", null, "text-align: left", ref DataArrPhongThuocTinh);
                    RowItem RowItem3 = new RowItem(3, ptt.SLTiepDan.ToString(), "", "", null, "text-align: center;", ref DataArrPhongThuocTinh);
                    RowItem RowItem4 = new RowItem(4, ptt.SLDonThu.ToString(), "", "", null, "text-align: center;", ref DataArrPhongThuocTinh);
                    RowItem RowItem5 = new RowItem(5, ptt.SLXuLyDon.ToString(), "", "", null, "text-align: center;", ref DataArrPhongThuocTinh);
                    RowItem RowItem6 = new RowItem(6, ptt.SLGiaiQuyetDon.ToString(), "", "", null, "text-align: center;", ref DataArrPhongThuocTinh);
                    tableDataPhongThuocTinh.DataArr = DataArrPhongThuocTinh;
                    data.Add(tableDataPhongThuocTinh);
                }
            }
            foreach (var item in DanhSachHuyen)
            {
                var DataArrHuyen = new List<RowItem>();
                TableData tableDataHuyen = new TableData();
                tableDataHuyen.ID = stt++;
                RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "text-align: center;width: 50px", ref DataArrHuyen);
                RowItem RowItem2 = new RowItem(2, item.TenDonVi, "", "", null, "text-align: left; font-weight : bold", ref DataArrHuyen);
                RowItem RowItem3 = new RowItem(3, coQuanList.Where(y => y.HuyenID == item.HuyenID ).Sum(x => x.SLTiepDan).ToString(), "", "", null, "text-align: center;", ref DataArrHuyen);
                RowItem RowItem4 = new RowItem(4, coQuanList.Where(y => y.HuyenID == item.HuyenID ).Sum(x => x.SLDonThu).ToString(), "", "", null, "text-align: center;", ref DataArrHuyen);
                RowItem RowItem5 = new RowItem(5, coQuanList.Where(y => y.HuyenID == item.HuyenID ).Sum(x => x.SLXuLyDon).ToString(), "", "", null, "text-align: center;", ref DataArrHuyen);
                RowItem RowItem6 = new RowItem(6, coQuanList.Where(y => y.HuyenID == item.HuyenID ).Sum(x => x.SLGiaiQuyetDon).ToString(), "", "", null, "text-align: center;", ref DataArrHuyen);
                tableDataHuyen.DataArr = DataArrHuyen;
                data.Add(tableDataHuyen);
                foreach (var cq in coQuanList.Where(x => x.CoQuanChaID == item.CoQuanID && x.CapID == (int)CapQuanLy.CapPhong).ToList())
                {
                    TableData tableData = new TableData();
                    tableData.ID = stt++;
                    var DataArr = new List<RowItem>();
                    RowItem RowItem7 = new RowItem(1, stt.ToString(), "", "", null, "text-align: center;width: 50px", ref DataArr);
                    RowItem RowItem8 = new RowItem(2, cq.TenDonVi, "", "", null, "text-align: left;", ref DataArr);
                    RowItem RowItem9 = new RowItem(3, cq.SLTiepDan.ToString(), "", "", null, "text-align: center;", ref DataArr);
                    RowItem RowItem10 = new RowItem(4, cq.SLDonThu.ToString(), "", "", null, "text-align: center;", ref DataArr);
                    RowItem RowItem11 = new RowItem(5, cq.SLXuLyDon.ToString(), "", "", null, "text-align: center;", ref DataArr);
                    RowItem RowItem12 = new RowItem(6, cq.SLGiaiQuyetDon.ToString(), "", "", null, "text-align: center;", ref DataArr);
                    tableData.DataArr = DataArr;
                    data.Add(tableData);
                }
                foreach (var cq in coQuanList.Where(x => x.CoQuanChaID == item.CoQuanID && x.CapID == (int)CapQuanLy.CapUBNDXa).ToList())
                {
                    TableData tableData1 = new TableData();
                    tableData1.ID = stt++;
                    var DataArrCapXa = new List<RowItem>();
                    RowItem RowItem7 = new RowItem(1, stt.ToString(), "", "", null, "text-align: center;width: 50px", ref DataArrCapXa);
                    RowItem RowItem8 = new RowItem(2, cq.TenDonVi, "", "", null, "text-align: left;", ref DataArrCapXa);
                    RowItem RowItem9 = new RowItem(3, cq.SLTiepDan.ToString(), "", "", null, "text-align: center;", ref DataArrCapXa);
                    RowItem RowItem10 = new RowItem(4, cq.SLDonThu.ToString(), "", "", null, "text-align: center;", ref DataArrCapXa);
                    RowItem RowItem11 = new RowItem(5, cq.SLXuLyDon.ToString(), "", "", null, "text-align: center;", ref DataArrCapXa);
                    RowItem RowItem12 = new RowItem(6, cq.SLGiaiQuyetDon.ToString(), "", "", null, "text-align: center;", ref DataArrCapXa);
                    tableData1.DataArr = DataArrCapXa;
                    data.Add(tableData1);
                }
            }

            BaoCaoModel.DataTable.TableData.AddRange(data);

            #endregion
            return BaoCaoModel;
        }

        public BaseResultModel ExportExcel(BasePagingParamsForFilter p, int CanBoDangNhapID, string ContentRootPath)
        {
            var Result = new BaseResultModel();
            try
            {
                var coQuanList = new ThongKeNhapLieu().GetByTime(p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                if (p.CoQuanID > 0)
                {
                    coQuanList = coQuanList.Where(x => x.CoQuanID == p.CoQuanID).ToList();
                }
                //chua nhap lieu
                if (p.TrangThai == 0)
                {
                    coQuanList = coQuanList.Where(x => x.SLTiepDan == 0 && x.SLDonThu == 0 && x.SLXuLyDon == 0 && x.SLGiaiQuyetDon == 0).ToList();
                }
                else if (p.TrangThai == 1)
                {
                    coQuanList = coQuanList.Where(x => x.SLTiepDan != 0 || x.SLDonThu != 0 || x.SLXuLyDon != 0 || x.SLGiaiQuyetDon != 0).ToList();
                }
                List<TableData> data = new List<TableData>();
                int stt = 0;
                //foreach (ThongKeNhapLieuInfo cq in coQuanList)
                //{
                //    TableData tableData = new TableData();
                //    tableData.ID = stt++;
                //    var DataArr = new List<RowItem>();
                //    RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "width: 15px", ref DataArr);
                //    RowItem RowItem2 = new RowItem(2, cq.TenDonVi, "", "", null, "text-align: left;", ref DataArr);
                //    RowItem RowItem3 = new RowItem(3, cq.SLTiepDan.ToString(), "", "", null, "text-align: center;", ref DataArr);
                //    RowItem RowItem4 = new RowItem(4, cq.SLXuLyDon.ToString(), "", "", null, "text-align: center;", ref DataArr);
                //    RowItem RowItem5 = new RowItem(4, cq.SLDonThu.ToString(), "", "", null, "text-align: center;", ref DataArr);
                //    RowItem RowItem6 = new RowItem(5, cq.SLGiaiQuyetDon.ToString(), "", "", null, "text-align: center;", ref DataArr);

                //    tableData.DataArr = DataArr;
                //    data.Add(tableData);
                //}
                var totalSLTiepDan = coQuanList.Sum(x => x.SLTiepDan).ToString();
                var totalSLDonThu = coQuanList.Sum(x => x.SLDonThu).ToString();
                var totalSLXuLyDon = coQuanList.Sum(x => x.SLXuLyDon).ToString();
                var totalSLGiaiQuyetDon = coQuanList.Sum(x => x.SLGiaiQuyetDon).ToString();
                var DanhSachHuyen = coQuanList.Where(x => x.CapID == (int)CapQuanLy.CapUBNDHuyen).ToList();
                var DanhSachPhongThuocTinh = coQuanList.Where(x => x.CapID == (int)CapQuanLy.CapSoNganh || x.CoQuanChaID == x.CoQuanID).ToList();

                var DataArrTong = new List<RowItem>();
                TableData tableDataTong = new TableData();
                tableDataTong.ID = stt++;
                RowItem RowItem1H = new RowItem(1, stt.ToString(), "", "", null, "text-align: center;width: 50px", ref DataArrTong);
                RowItem RowItem2H = new RowItem(2, "Toàn tỉnh", "", "", null, "text-align: left; font-weight : bold", ref DataArrTong);
                RowItem RowItem3H = new RowItem(3, totalSLTiepDan, "", "", null, "text-align: center;", ref DataArrTong);
                RowItem RowItem4H = new RowItem(4, totalSLDonThu, "", "", null, "text-align: center;", ref DataArrTong);
                RowItem RowItem5H = new RowItem(5, totalSLXuLyDon, "", "", null, "text-align: center;", ref DataArrTong);
                RowItem RowItem6H = new RowItem(6, totalSLGiaiQuyetDon, "", "", null, "text-align: center;", ref DataArrTong);
                tableDataTong.DataArr = DataArrTong;
                data.Add(tableDataTong);

                foreach (var ptt in DanhSachPhongThuocTinh)
                {
                    if (stt == 1)
                    {
                        var DataArrCapTinh = new List<RowItem>();
                        TableData tableDataCapTinh = new TableData();
                        tableDataCapTinh.ID = stt++;
                        RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "text-align: center;width: 50px", ref DataArrCapTinh);
                        RowItem RowItem2 = new RowItem(2, "Cấp tỉnh", "", "", null, "text-align: left; font-weight : bold", ref DataArrCapTinh);
                        RowItem RowItem3 = new RowItem(3, coQuanList.Where(y => y.CapID == (int)CapQuanLy.CapSoNganh || y.CoQuanChaID == y.CoQuanID).Sum(x => x.SLTiepDan).ToString(), "", "", null, "text-align: center;", ref DataArrCapTinh);
                        RowItem RowItem4 = new RowItem(4, coQuanList.Where(y => y.CapID == (int)CapQuanLy.CapSoNganh || y.CoQuanChaID == y.CoQuanID).Sum(x => x.SLDonThu).ToString(), "", "", null, "text-align: center;", ref DataArrCapTinh);
                        RowItem RowItem5 = new RowItem(5, coQuanList.Where(y => y.CapID == (int)CapQuanLy.CapSoNganh || y.CoQuanChaID == y.CoQuanID).Sum(x => x.SLXuLyDon).ToString(), "", "", null, "text-align: center;", ref DataArrCapTinh);
                        RowItem RowItem6 = new RowItem(6, coQuanList.Where(y => y.CapID == (int)CapQuanLy.CapSoNganh || y.CoQuanChaID == y.CoQuanID).Sum(x => x.SLGiaiQuyetDon).ToString(), "", "", null, "text-align: center;", ref DataArrCapTinh);
                        tableDataCapTinh.DataArr = DataArrCapTinh;
                        data.Add(tableDataCapTinh);
                    }
                    else
                    {
                        var DataArrPhongThuocTinh = new List<RowItem>();
                        TableData tableDataPhongThuocTinh = new TableData();
                        tableDataPhongThuocTinh.ID = stt++;
                        RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "text-align: center;width: 50px", ref DataArrPhongThuocTinh);
                        RowItem RowItem2 = new RowItem(2, ptt.TenDonVi, "", "", null, "text-align: left", ref DataArrPhongThuocTinh);
                        RowItem RowItem3 = new RowItem(3, ptt.SLTiepDan.ToString(), "", "", null, "text-align: center;", ref DataArrPhongThuocTinh);
                        RowItem RowItem4 = new RowItem(4, ptt.SLDonThu.ToString(), "", "", null, "text-align: center;", ref DataArrPhongThuocTinh);
                        RowItem RowItem5 = new RowItem(5, ptt.SLXuLyDon.ToString(), "", "", null, "text-align: center;", ref DataArrPhongThuocTinh);
                        RowItem RowItem6 = new RowItem(6, ptt.SLGiaiQuyetDon.ToString(), "", "", null, "text-align: center;", ref DataArrPhongThuocTinh);
                        tableDataPhongThuocTinh.DataArr = DataArrPhongThuocTinh;
                        data.Add(tableDataPhongThuocTinh);
                    }
                }
                foreach (var item in DanhSachHuyen)
                {
                    var DataArrHuyen = new List<RowItem>();
                    TableData tableDataHuyen = new TableData();
                    tableDataHuyen.ID = stt++;
                    RowItem RowItem1 = new RowItem(1, stt.ToString(), "", "", null, "text-align: center;width: 50px", ref DataArrHuyen);
                    RowItem RowItem2 = new RowItem(2, item.TenDonVi, "", "", null, "text-align: left; font-weight : bold", ref DataArrHuyen);
                    RowItem RowItem3 = new RowItem(3, coQuanList.Where(y => y.HuyenID == item.HuyenID).Sum(x => x.SLTiepDan).ToString(), "", "", null, "text-align: center;", ref DataArrHuyen);
                    RowItem RowItem4 = new RowItem(4, coQuanList.Where(y => y.HuyenID == item.HuyenID).Sum(x => x.SLDonThu).ToString(), "", "", null, "text-align: center;", ref DataArrHuyen);
                    RowItem RowItem5 = new RowItem(5, coQuanList.Where(y => y.HuyenID == item.HuyenID).Sum(x => x.SLXuLyDon).ToString(), "", "", null, "text-align: center;", ref DataArrHuyen);
                    RowItem RowItem6 = new RowItem(6, coQuanList.Where(y => y.HuyenID == item.HuyenID).Sum(x => x.SLGiaiQuyetDon).ToString(), "", "", null, "text-align: center;", ref DataArrHuyen);
                    tableDataHuyen.DataArr = DataArrHuyen;
                    data.Add(tableDataHuyen);
                    foreach (var cq in coQuanList.Where(x => x.CoQuanChaID == item.CoQuanID && x.CapID == (int)CapQuanLy.CapPhong).ToList())
                    {
                        TableData tableData = new TableData();
                        tableData.ID = stt++;
                        var DataArr = new List<RowItem>();
                        RowItem RowItem7 = new RowItem(1, stt.ToString(), "", "", null, "text-align: center;width: 50px", ref DataArr);
                        RowItem RowItem8 = new RowItem(2, cq.TenDonVi, "", "", null, "text-align: left;", ref DataArr);
                        RowItem RowItem9 = new RowItem(3, cq.SLTiepDan.ToString(), "", "", null, "text-align: center;", ref DataArr);
                        RowItem RowItem10 = new RowItem(4, cq.SLDonThu.ToString(), "", "", null, "text-align: center;", ref DataArr);
                        RowItem RowItem11 = new RowItem(5, cq.SLXuLyDon.ToString(), "", "", null, "text-align: center;", ref DataArr);
                        RowItem RowItem12 = new RowItem(6, cq.SLGiaiQuyetDon.ToString(), "", "", null, "text-align: center;", ref DataArr);
                        tableData.DataArr = DataArr;
                        data.Add(tableData);
                    }
                    foreach (var cq in coQuanList.Where(x => x.CoQuanChaID == item.CoQuanID && x.CapID == (int)CapQuanLy.CapUBNDXa).ToList())
                    {
                        TableData tableData1 = new TableData();
                        tableData1.ID = stt++;
                        var DataArrCapXa = new List<RowItem>();
                        RowItem RowItem7 = new RowItem(1, stt.ToString(), "", "", null, "text-align: center;width: 50px", ref DataArrCapXa);
                        RowItem RowItem8 = new RowItem(2, cq.TenDonVi, "", "", null, "text-align: left;", ref DataArrCapXa);
                        RowItem RowItem9 = new RowItem(3, cq.SLTiepDan.ToString(), "", "", null, "text-align: center;", ref DataArrCapXa);
                        RowItem RowItem10 = new RowItem(4, cq.SLDonThu.ToString(), "", "", null, "text-align: center;", ref DataArrCapXa);
                        RowItem RowItem11 = new RowItem(5, cq.SLXuLyDon.ToString(), "", "", null, "text-align: center;", ref DataArrCapXa);
                        RowItem RowItem12 = new RowItem(6, cq.SLGiaiQuyetDon.ToString(), "", "", null, "text-align: center;", ref DataArrCapXa);
                        tableData1.DataArr = DataArrCapXa;
                        data.Add(tableData1);
                    }
                }

                string path = @"Templates\FileTam\ThongKeNhapLieu_" + CanBoDangNhapID + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";
                string urlExcel = ThongKeNhapLieu_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
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

        public string ThongKeNhapLieu_Excel(string rootPath, string pathFile, List<TableData> data, DateTime tuNgay, DateTime denNgay)
        {
            // path to your excel file
            string path = rootPath + @"\Templates\BaoCao\ThongKeNhapLieu.xlsx";
            FileInfo fileInfo = new FileInfo(path);
            FileInfo file = new FileInfo(rootPath + "\\" + pathFile);

            ExcelPackage package = new ExcelPackage(fileInfo);
            if (package.Workbook.Worksheets != null)
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                // get number of rows in the sheet
                int rows = worksheet.Dimension.Rows;
                int cols = worksheet.Dimension.Columns;

                string TuNgayDenNgay = "SO_LIEU_TINH_TU_NGAY_DEN_NGAY";

                // loop through the worksheet rows
                for (int i = 1; i <= rows; i++)
                {
                    for (int j = 1; j <= cols; j++)
                    {
                        if (worksheet.Cells[i, j].Value != null && TuNgayDenNgay.Contains(worksheet.Cells[i, j].Value.ToString()))
                        {
                            worksheet.Cells[i, j].Value = "Từ ngày " + tuNgay.ToString("dd/MM/yyyy") + " đến ngày " + denNgay.ToString("dd/MM/yyyy");
                        }
                    }
                }
                if (data.Count > 0)
                {
                    worksheet.InsertRow(6, data.Count - 1, 5);
                    //worksheet.DeleteRow(data.Count);
                    for (int i = 0; i < data.Count; i++)
                    {
                        if (data[i].DataArr != null && data[i].DataArr.Count > 0)
                        {
                            for (int j = 0; j < data[i].DataArr.Count; j++)
                            {
                                //if (data[i].DataArr[j].Content != "0")
                                //{
                                worksheet.Cells[i + 5, j + 1].Value = data[i].DataArr[j].Content;
                                if (data[i].DataArr[j].Style.Contains("bold")) worksheet.Cells[i + 5, j + 1].Style.Font.Bold = true;
                                //}
                            }
                        }

                    }

                }

                // save changes
                package.SaveAs(file);
            }

            return pathFile;
        }
    }
}
