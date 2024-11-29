using Com.Gosol.KNTC.DAL.DanhMuc;
using Com.Gosol.KNTC.DAL.HeThong;
using Com.Gosol.KNTC.DAL.KNTC;
using Com.Gosol.KNTC.Models.BaoCao;
using Com.Gosol.KNTC.Models.DanhMuc;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Ultilities;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using Spire.Pdf.Exporting.XPS.Schema;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using DataTable = System.Data.DataTable;

namespace Com.Gosol.KNTC.DAL.BaoCao
{
    public class KQGQ03DAL
    {
        private const string PARM_STARTDATE = "@StartDate";
        private const string PARM_ENDDATE = "@EndDate";
        private const string PARM_COQUANID = "@CoQuanID";
        private const string PARM_TINHID = "@TinhID";
        private const string PARAM_START = "@Start";
        private const string PARAM_END = "@End";
        public List<TableData> KQGQ03(List<int> listCapChonBaoCao, string ContentRootPath, int RoleID, int CapID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, int HuyenDangNhapID, DateTime startDate, DateTime endDate)
        {
            List<TableData> data = new List<TableData>();
            endDate = endDate.AddDays(1);
            List<BaoCao2cInfo> resultList = new List<BaoCao2cInfo>();
            DataTable dt = BaoCao2c_DataTable();
            bool flag = true;
            bool flagCapXa = true;
            bool flagToanHuyen = true;
            if (CapID == (int)CapQuanLy.CapUBNDTinh)
            {
                List<CapInfo> capList = new List<CapInfo>();
                foreach (int item in listCapChonBaoCao)
                {
                    CapInfo capInfo = new CapInfo();
                    capInfo.CapID = item;
                    capInfo.TenCap = Utils.GetTenCap(item);
                    if (capInfo.CapID == (int)CapQuanLy.CapUBNDHuyen || capInfo.CapID == (int)CapQuanLy.CapUBNDXa)
                    {
                        if (capList.Count == 0)
                        {
                            flag = false;
                        }
                        if (flagToanHuyen == true)
                        {
                            CapInfo capInfoToanHuyen = new CapInfo();
                            capInfoToanHuyen.CapID = (int)CapQuanLy.ToanHuyen;
                            capInfoToanHuyen.TenCap = "Toàn huyện";
                            capList.Add(capInfoToanHuyen);
                            flagToanHuyen = false;
                        }

                    }
                    if (flagCapXa == true && capInfo.CapID == (int)CapQuanLy.CapUBNDXa)
                    {
                        CapInfo capInfoXa = new CapInfo();
                        capInfoXa.CapID = (int)CapQuanLy.CapUBNDXa;
                        capInfoXa.TenCap = "UBND Cấp Xã";
                        capList.Add(capInfoXa);
                    }
                    else
                    {
                        capList.Add(capInfo);
                    }
                }
                string capToanTinh = string.Empty;
                foreach (CapInfo capInfo in capList)
                {
                    capToanTinh = capToanTinh + capInfo.CapID + "*";
                }

                CalculateBaoCaoCapTinh(ContentRootPath, RoleID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, ref resultList, startDate, endDate, capList, flag, TinhDangNhapID, ref dt);
            }
            else if (CapID == (int)CapQuanLy.CapTrungUong)
            {
                //Neu la cap trung uong, xem bao cao theo tinh
                CalculateBaoCaoCapTrungUong(ref resultList, startDate, endDate, TinhDangNhapID);
            }
            else if (CapID == (int)CapQuanLy.CapUBNDHuyen)
            {
                CalculateBaoCaoCapHuyen(ContentRootPath, RoleID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID, ref resultList, startDate, endDate, TinhDangNhapID, ref dt);
            }
            else if (CapID == (int)CapQuanLy.CapSoNganh)
            {
                var listThanhTraTinh = new SystemConfigDAL().GetByKey("Thanh_Tra_Tinh_ID").ConfigValue.Split(',').ToList().Select(x => Utils.ConvertToInt32(x.ToString(), 0)).ToList();
                if (listThanhTraTinh.Contains(CoQuanDangNhapID) && RoleID == (int)EnumChucVu.LanhDao)
                {
                    List<CapInfo> capList = new List<CapInfo>();
                    foreach (int item in listCapChonBaoCao)
                    {
                        CapInfo capInfo = new CapInfo();
                        capInfo.CapID = item;
                        capInfo.TenCap = Utils.GetTenCap(item);
                        if (capInfo.CapID == (int)CapQuanLy.CapUBNDHuyen)
                        {
                            if (capList.Count == 0)
                            {
                                flag = false;
                            }
                            CapInfo capInfoToanHuyen = new CapInfo();
                            capInfoToanHuyen.CapID = (int)CapQuanLy.ToanHuyen;
                            capInfoToanHuyen.TenCap = "Toàn huyện";
                            capList.Add(capInfoToanHuyen);
                            flagCapXa = false;
                        }
                        if (flagCapXa == false && capInfo.CapID == (int)CapQuanLy.CapUBNDXa)
                        {

                        }
                        else
                        {
                            capList.Add(capInfo);
                        }
                        if (flagCapXa == false)
                        {
                            if (capInfo.CapID != (int)CapQuanLy.CapUBNDXa)
                            {
                                CapInfo capInfoXa = new CapInfo();
                                capInfoXa.CapID = (int)CapQuanLy.CapUBNDXa;
                                capInfoXa.TenCap = "UBND Cấp Xã";
                                capList.Add(capInfoXa);
                            }
                        }
                    }
                    string capToanTinh = string.Empty;
                    foreach (CapInfo capInfo in capList)
                    {
                        capToanTinh = capToanTinh + capInfo.CapID + "*";
                    }

                    CalculateBaoCaoCapTinh(ContentRootPath, RoleID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, ref resultList, startDate, endDate, capList, flag, TinhDangNhapID, ref dt);
                }
                else
                {
                    CalculateBaoCaoCapSo(ContentRootPath, RoleID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, ref resultList, startDate, endDate, ref dt);
                }

            }
            else if (CapID == (int)CapQuanLy.CapUBNDXa || CapID == (int)CapQuanLy.CapPhong)
            {
                CalculateBaoCaoCapXa(ContentRootPath, CoQuanDangNhapID, CanBoDangNhapID, ref resultList, startDate, endDate, ref dt);
            }

            int stt = 1;
            foreach (DataRow dro in dt.Rows)
            {
                TableData tableData = new TableData();
                tableData.ID = stt++;
                var DataArr = new List<RowItem>();

                int RowItem_CapID = Utils.ConvertToInt32(dro["CapID"], 0);
                string Css = "";
                if (RowItem_CapID == CapCoQuanViewChiTiet.ToanTinh.GetHashCode() || RowItem_CapID == CapCoQuanViewChiTiet.CapUBNDTinh.GetHashCode()
                    || RowItem_CapID == CapCoQuanViewChiTiet.ToanHuyen.GetHashCode() || RowItem_CapID == CapCoQuanViewChiTiet.CapSoNganh.GetHashCode()
                    || RowItem_CapID == CapCoQuanViewChiTiet.CapUBNDHuyen.GetHashCode() || RowItem_CapID == CapCoQuanViewChiTiet.CapUBNDXa.GetHashCode())
                {
                    Css = "font-weight: bold;";
                }

                RowItem RowItem1 = new RowItem(1, dro["TenCoQuan"].ToString(), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), null, dro["CssClass"].ToString(), ref DataArr);
                RowItem RowItem2 = new RowItem(2, Utils.AddCommas(dro["Col1Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem3 = new RowItem(3, Utils.AddCommas(dro["Col2Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem4 = new RowItem(4, Utils.AddCommas(dro["Col3Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem5 = new RowItem(5, Utils.AddCommas(dro["Col4Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem6 = new RowItem(6, Utils.AddCommas(dro["Col5Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem7 = new RowItem(7, Utils.AddCommas(dro["Col6Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem8 = new RowItem(8, Utils.AddCommas(dro["Col7Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem9 = new RowItem(9, Utils.AddCommas(dro["Col8Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem10 = new RowItem(10, Utils.AddCommas(dro["Col9Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem11 = new RowItem(11, Utils.AddCommas(dro["Col10Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem12 = new RowItem(12, Utils.AddCommasDouble_Tien(dro["Col11Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem13 = new RowItem(13, Utils.AddCommasDouble(dro["Col12Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem14 = new RowItem(14, Utils.AddCommasDouble_Tien(dro["Col13Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem15 = new RowItem(15, Utils.AddCommasDouble(dro["Col14Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem16 = new RowItem(16, Utils.AddCommasDouble_Tien(dro["Col15Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem17 = new RowItem(17, Utils.AddCommasDouble(dro["Col16Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem18 = new RowItem(18, Utils.AddCommas(dro["Col17Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem19 = new RowItem(19, Utils.AddCommas(dro["Col18Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem20 = new RowItem(20, Utils.AddCommas(dro["Col19Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem21 = new RowItem(21, Utils.AddCommas(dro["Col20Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem22 = new RowItem(22, Utils.AddCommas(dro["Col21Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem23 = new RowItem(23, Utils.AddCommas(dro["Col22Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem24 = new RowItem(24, Utils.AddCommas(dro["Col23Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem25 = new RowItem(25, Utils.AddCommas(dro["Col24Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem26 = new RowItem(26, Utils.AddCommas(dro["Col25Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem27 = new RowItem(27, Utils.AddCommas(dro["Col26Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem28 = new RowItem(28, Utils.AddCommas(dro["Col27Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem29 = new RowItem(29, Utils.AddCommas(dro["Col28Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem30 = new RowItem(30, Utils.AddCommas(dro["Col29Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem31 = new RowItem(31, Utils.AddCommas(dro["Col30Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem32 = new RowItem(32, Utils.AddCommas(dro["Col31Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), null, "text-align: center;" + Css, ref DataArr);

                tableData.DataArr = DataArr;
                data.Add(tableData);
            }

            return data;
        }
        public string KQGQ03_Excel(string rootPath, string pathFile, List<TableData> data, DateTime tuNgay, DateTime denNgay)
        {
            //var camera = new Camera;
            // path to your excel file
            string path = rootPath + @"\Templates\BaoCao\KQGQ03.xlsx";
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
                            worksheet.Cells[i, j].Value = "Số liệu tính từ ngày " + tuNgay.ToString("dd/MM/yyyy") + " đến ngày " + denNgay.ToString("dd/MM/yyyy");
                        }
                    }
                }
                if (data.Count > 0)
                {
                    worksheet.InsertRow(9, data.Count - 1, 8);
                    //worksheet.DeleteRow(data.Count);
                    for (int i = 0; i < data.Count; i++)
                    {
                        if (data[i].DataArr != null && data[i].DataArr.Count > 0)
                        {
                            for (int j = 0; j < data[i].DataArr.Count; j++)
                            {
                                if (data[i].DataArr[j].Content != "0")
                                {
                                    worksheet.Cells[i + 8, j + 1].Value = data[i].DataArr[j].Content;
                                    if (data[i].DataArr[j].Style.Contains("bold")) worksheet.Cells[i + 8, j + 1].Style.Font.Bold = true;
                                }
                            }
                        }

                    }

                }

                // save changes
                package.SaveAs(file);
            }

            return pathFile;
        }
        public static void CalculateBaoCaoCapTinh(string ContentRootPath, int RoleID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, ref List<BaoCao2cInfo> resultList, DateTime startDate, DateTime endDate, List<CapInfo> capList, bool flag, int tinhID, ref DataTable dt)
        {
            int CoQuanChaID = 0;
            BaoCao2cInfo bcInfo = new BaoCao2cInfo();
            List<int> ListCapID = new List<int>();
            capList.ForEach(x => ListCapID.Add(x.CapID));
            //foreach (CapInfo capInfo in capList)
            //{
            //BaoCao2cInfo bc2bInfo = new BaoCao2cInfo();
            //BaoCao2cInfo bc2bInfoToanHuyen = new BaoCao2cInfo();
            if (capList.Count > 0)
            {
                List<CoQuanInfo> cqList = new List<CoQuanInfo>();

                if (RoleID == (int)EnumChucVu.LanhDao)
                {
                    var CoQuanChaPhuHop = new CoQuan().GetCoQuanByTinhID_New(TinhDangNhapID);
                    if (ListCapID.Contains(CapQuanLy.CapUBNDHuyen.GetHashCode()))
                    {

                        var cqList1 = new CoQuan().GetAllCapCon(CoQuanChaPhuHop.CoQuanID).ToList().Where(x => (ListCapID.Contains(x.CapID) || x.CapID == (int)CapQuanLy.CapPhong))
                     .ToList();
                        var cqList2 = cqList1.Where(x => x.CapID == CapQuanLy.CapUBNDHuyen.GetHashCode()).ToList();
                        cqList.AddRange(cqList2.Where(x => new CoQuan().GetAllCapCon(x.CoQuanID).ToList().Where(y => y.SuDungPM == true).ToList().Count > 0).Select(x => x));
                        cqList.AddRange(cqList1.Where(x => (x.CapID != (int)CapQuanLy.CapUBNDHuyen) && x.SuDungPM == true).Select(x => x));
                    }
                    else
                    {
                        cqList = new CoQuan().GetAllCapCon(CoQuanChaPhuHop.CoQuanID).ToList().Where(x => ListCapID.Contains(x.CapID) && x.SuDungPM == true)
                      .ToList();
                    }
                    CoQuanChaID = CoQuanChaPhuHop.CoQuanID;
                }

                else
                {
                    cqList = new List<CoQuanInfo> { new CoQuan().GetCoQuanByID(CoQuanDangNhapID) };
                }
                if (cqList.Count > 0)
                {
                    string filename = ContentRootPath + "/Upload/" + CanBoDangNhapID + "_CoQuan.xml";
                    if (File.Exists(filename))
                    {
                        File.Delete(filename);
                    }
                    using (FileStream file = System.IO.File.Create(filename))
                    {

                    }
                    XDocument doc = new XDocument();
                    doc =
                                       new XDocument(
                                       new XElement("LogConfig", cqList.Select(x =>
                      new XElement("SystemLog", new XElement("CoQuan", new XAttribute("CoQuanID", x.CoQuanID),
                                     new XAttribute("CapID", x.CapID), new XAttribute("CoQuanChaID", x.CoQuanChaID),
                                     new XAttribute("SuDungPM", x.SuDungPM))))));

                    doc.Save(filename);
                    var pList = new SqlParameter("@ListCoQuanID", SqlDbType.Structured);
                    pList.TypeName = "dbo.IntList";
                    var tbCoQuanID = new DataTable();
                    tbCoQuanID.Columns.Add("CoQuanID", typeof(string));
                    cqList.ForEach(x => tbCoQuanID.Rows.Add(x.CoQuanID));
                    //BaoCao2cInfo bc2bInfo2 = new BaoCao2cInfo();
                    SqlParameter[] parm = new SqlParameter[] {
                        new SqlParameter("@StartDate", SqlDbType.DateTime),
                        new SqlParameter("@EndDate", SqlDbType.DateTime),
                        pList,
                        new SqlParameter("@Flag", SqlDbType.Int),
                        new SqlParameter("@CoQuanChaID", SqlDbType.Int)
            };
                    parm[0].Value = startDate;
                    parm[1].Value = endDate;
                    parm[2].Value = tbCoQuanID;
                    parm[3].Value = 1;
                    parm[4].Value = CoQuanChaID;
                    try
                    {
                        using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_BaoCaoKQGQ03", parm))
                        {
                            dt.Load(dr);
                        }
                    }
                    catch (Exception e)
                    {
                        throw;
                    }

                }
            }

            if (flag == true)
            {

                DataRow dr = dt.NewRow();
                //dr["TenCoQuan"] = "<b style='text-transform: uppercase'>Toàn tỉnh</b>";
                //dr["CssClass"] = "highlight";
                dr["TenCoQuan"] = "Toàn tỉnh";
                dr["CssClass"] = "font-weight:bold;text-transform: uppercase";
                dr["CapID"] = CapCoQuanViewChiTiet.ToanTinh.GetHashCode();
                dr["ThuTu"] = 1000;// 5;
                dr["CoQuanID"] = 0;
                foreach (DataRow dro in dt.Rows)
                {
                    for (var i = 1; i <= 37; i++)
                    {
                        if (dro["Col" + i + "Data"] == null)
                        {
                            dro["Col" + i + "Data"] = 0;

                        }
                        dr["Col" + i + "Data"] = Utils.ConvertToDouble(dr["Col" + i + "Data"], 0) +
                            Utils.ConvertToDouble(dro["Col" + i + "Data"], 0);
                    }

                    dr["slCol15"] = Utils.ConvertToDouble(dr["slCol15"], 0) +
                           Utils.ConvertToDouble(dro["slCol15"], 0);

                    dr["slCol17"] = Utils.ConvertToDouble(dr["slCol17"], 0) +
                           Utils.ConvertToDouble(dro["slCol17"], 0);

                    dr["slCol20"] = Utils.ConvertToDouble(dr["slCol20"], 0) +
                           Utils.ConvertToDouble(dro["slCol20"], 0);

                    dr["slCol25"] = Utils.ConvertToDouble(dr["slCol25"], 0) +
                           Utils.ConvertToDouble(dro["slCol25"], 0);

                    dr["slCol30"] = Utils.ConvertToDouble(dr["slCol30"], 0) +
                           Utils.ConvertToDouble(dro["slCol30"], 0);

                    dr["slCol34"] = Utils.ConvertToDouble(dr["slCol34"], 0) +
                           Utils.ConvertToDouble(dro["slCol34"], 0);
                }
                dt.Rows.Add(dr);
            }
            foreach (CapInfo capInfo in capList)
            {
                if (capInfo.CapID == (int)CapQuanLy.CapUBNDTinh)
                {
                    DataRow dr = dt.NewRow();
                    //dr["TenCoQuan"] = "<b style='text-transform: uppercase'>" + "UBND Cấp Tỉnh" + "</b>";
                    //dr["CssClass"] = "highlight";
                    dr["TenCoQuan"] = "UBND Cấp Tỉnh";
                    dr["CssClass"] = "font-weight:bold;text-transform: uppercase";
                    dr["CapID"] = CapCoQuanViewChiTiet.CapUBNDTinh.GetHashCode();
                    dr["ThuTu"] = 2000;// 4.5;
                    dr["CoQuanID"] = 0;
                    //DataTable dtnew = BaoCao2b_DataTable_New();
                    foreach (DataRow dro in dt.Rows)
                    {
                        if (Utils.ConvertToInt32(dro["CapID"], 0) == (int)CapQuanLy.CapUBNDTinh || Utils.ConvertToInt32(dro["CapID"], 0) == (int)CapQuanLy.CapSoNganh)
                        {
                            for (var i = 1; i <= 37; i++)
                            {

                                dr["Col" + i + "Data"] = Utils.ConvertToDouble(dr["Col" + i + "Data"], 0) +
                                    Utils.ConvertToDouble(dro["Col" + i + "Data"], 0);
                            }

                            dr["slCol15"] = Utils.ConvertToDouble(dr["slCol15"], 0) +
                                   Utils.ConvertToDouble(dro["slCol15"], 0);

                            dr["slCol17"] = Utils.ConvertToDouble(dr["slCol17"], 0) +
                                   Utils.ConvertToDouble(dro["slCol17"], 0);

                            dr["slCol20"] = Utils.ConvertToDouble(dr["slCol20"], 0) +
                                   Utils.ConvertToDouble(dro["slCol20"], 0);

                            dr["slCol25"] = Utils.ConvertToDouble(dr["slCol25"], 0) +
                                   Utils.ConvertToDouble(dro["slCol25"], 0);

                            dr["slCol30"] = Utils.ConvertToDouble(dr["slCol30"], 0) +
                                   Utils.ConvertToDouble(dro["slCol30"], 0);

                            dr["slCol34"] = Utils.ConvertToDouble(dr["slCol34"], 0) +
                                   Utils.ConvertToDouble(dro["slCol34"], 0);
                            dro["ThuTu"] = 2000 + 1;
                        }
                    }
                    dt.Rows.Add(dr);
                }
                //if (capInfo.CapID == (int)CapQuanLy.ToanHuyen)
                //{
                //    DataRow dr = dt.NewRow();
                //    //dr["TenCoQuan"] = "<b style='text-transform: uppercase'>" + "Toàn huyện" + "</b>";
                //    //dr["CssClass"] = "highlight";
                //    dr["TenCoQuan"] = "Toàn huyện";
                //    dr["CssClass"] = "font-weight:bold;text-transform: uppercase";
                //    dr["CapID"] = CapCoQuanViewChiTiet.ToanHuyen.GetHashCode();
                //    dr["ThuTu"] = 2.75;
                //    dr["CoQuanID"] = 0;
                //    //DataTable dtnew = BaoCao2b_DataTable_New();
                //    foreach (DataRow dro in dt.Rows)
                //    {
                //        if (/*/*Utils.ConvertToInt32(dro["CapID"], 0) == (int)CapQuanLy.CapUBNDTinh ||*/ /*Utils.ConvertToInt32(dro["CapID"], 0) == (int)CapQuanLy.CapSoNganh*/
                //           /*|*/ Utils.ConvertToInt32(dro["CapID"], 0) == (int)CapQuanLy.CapUBNDHuyen || Utils.ConvertToInt32(dro["CapID"], 0) == (int)CapQuanLy.CapUBNDXa
                //            || Utils.ConvertToInt32(dro["CapID"], 0) == (int)CapQuanLy.ToanHuyen)
                //        {
                //            for (var i = 1; i <= 37; i++)
                //            {

                //                if (dro["Col" + i + "Data"] == null)
                //                {
                //                    dro["Col" + i + "Data"] = 0;

                //                }
                //                dr["Col" + i + "Data"] = Utils.ConvertToDouble(dr["Col" + i + "Data"], 0) +
                //                    Utils.ConvertToDouble(dro["Col" + i + "Data"], 0);
                //            }

                //            dr["slCol15"] = Utils.ConvertToDouble(dr["slCol15"], 0) +
                //                   Utils.ConvertToDouble(dro["slCol15"], 0);

                //            dr["slCol17"] = Utils.ConvertToDouble(dr["slCol17"], 0) +
                //                   Utils.ConvertToDouble(dro["slCol17"], 0);

                //            dr["slCol20"] = Utils.ConvertToDouble(dr["slCol20"], 0) +
                //                   Utils.ConvertToDouble(dro["slCol20"], 0);

                //            dr["slCol30"] = Utils.ConvertToDouble(dr["slCol30"], 0) +
                //                   Utils.ConvertToDouble(dro["slCol30"], 0);

                //            dr["slCol34"] = Utils.ConvertToDouble(dr["slCol34"], 0) +
                //                   Utils.ConvertToDouble(dro["slCol34"], 0);
                //        }
                //    }
                //    dt.Rows.Add(dr);
                //}
                else if (capInfo.CapID == (int)CapQuanLy.CapSoNganh)
                {
                    //DataRow dr = dt.NewRow();
                    ////dr["TenCoQuan"] = "<b style='text-transform: uppercase'>" + "Cấp Sở,Ngành" + "</b>";
                    ////dr["CssClass"] = "highlight";
                    //dr["TenCoQuan"] = "Cấp Sở,Ngành";
                    //dr["CssClass"] = "font-weight:bold;text-transform: uppercase";
                    //dr["CapID"] = CapCoQuanViewChiTiet.CapSoNganh.GetHashCode();
                    //dr["ThuTu"] = 3.5;
                    //dr["CoQuanID"] = 0;
                    ////DataTable dtnew = BaoCao2b_DataTable_New();
                    //foreach (DataRow dro in dt.Rows)
                    //{
                    //    if (Utils.ConvertToInt32(dro["CapID"], 0) == (int)CapQuanLy.CapSoNganh)
                    //    {
                    //        for (var i = 1; i <= 37; i++)
                    //        {
                    //            dr["Col" + i + "Data"] = Utils.ConvertToDouble(dr["Col" + i + "Data"], 0) +
                    //                Utils.ConvertToDouble(dro["Col" + i + "Data"], 0);
                    //        }
                    //        dr["slCol15"] = Utils.ConvertToDouble(dr["slCol15"], 0) +
                    //               Utils.ConvertToDouble(dro["slCol15"], 0);

                    //        dr["slCol17"] = Utils.ConvertToDouble(dr["slCol17"], 0) +
                    //               Utils.ConvertToDouble(dro["slCol17"], 0);

                    //        dr["slCol20"] = Utils.ConvertToDouble(dr["slCol20"], 0) +
                    //               Utils.ConvertToDouble(dro["slCol20"], 0);

                    //        dr["slCol25"] = Utils.ConvertToDouble(dr["slCol25"], 0) +
                    //               Utils.ConvertToDouble(dro["slCol25"], 0);

                    //        dr["slCol30"] = Utils.ConvertToDouble(dr["slCol30"], 0) +
                    //               Utils.ConvertToDouble(dro["slCol30"], 0);

                    //        dr["slCol34"] = Utils.ConvertToDouble(dr["slCol34"], 0) +
                    //               Utils.ConvertToDouble(dro["slCol34"], 0);
                    //    }
                    //}
                    //dt.Rows.Add(dr);
                }
                else if (capInfo.CapID == (int)CapQuanLy.CapUBNDHuyen)
                {
                    DataRow dr = dt.NewRow();
                    //dr["TenCoQuan"] = "<b style='text-transform: uppercase'>" + "UBND Cấp Huyện" + "</b>";
                    //dr["CssClass"] = "highlight";
                    dr["TenCoQuan"] = "UBND Cấp Huyện";
                    dr["CssClass"] = "font-weight:bold;text-transform: uppercase";
                    dr["CapID"] = CapCoQuanViewChiTiet.CapUBNDHuyen.GetHashCode();
                    dr["ThuTu"] = 5000;// 2.5;
                    dr["CoQuanID"] = 0;
                    //DataTable dtnew = BaoCao2b_DataTable_New();
                    foreach (DataRow dro in dt.Rows)
                    {
                        if (Utils.ConvertToInt32(dro["CapID"], 0) == (int)CapQuanLy.CapUBNDHuyen)
                        {
                            for (var i = 1; i <= 37; i++)
                            {
                                if (dro["Col" + i + "Data"] == null)
                                {
                                    dro["Col" + i + "Data"] = 0;

                                }
                                dr["Col" + i + "Data"] = Utils.ConvertToDouble(dr["Col" + i + "Data"], 0) +
                                    Utils.ConvertToDouble(dro["Col" + i + "Data"], 0);
                            }

                            dr["slCol15"] = Utils.ConvertToDouble(dr["slCol15"], 0) +
                                   Utils.ConvertToDouble(dro["slCol15"], 0);

                            dr["slCol17"] = Utils.ConvertToDouble(dr["slCol17"], 0) +
                                   Utils.ConvertToDouble(dro["slCol17"], 0);

                            dr["slCol20"] = Utils.ConvertToDouble(dr["slCol20"], 0) +
                                   Utils.ConvertToDouble(dro["slCol20"], 0);

                            dr["slCol25"] = Utils.ConvertToDouble(dr["slCol25"], 0) +
                                   Utils.ConvertToDouble(dro["slCol25"], 0);

                            dr["slCol30"] = Utils.ConvertToDouble(dr["slCol30"], 0) +
                                   Utils.ConvertToDouble(dro["slCol30"], 0);

                            dr["slCol34"] = Utils.ConvertToDouble(dr["slCol34"], 0) +
                                   Utils.ConvertToDouble(dro["slCol34"], 0);
                            dro["ThuTu"] = 5000 + 1;

                        }
                    }
                    dt.Rows.Add(dr);
                }
                else if (capInfo.CapID == (int)CapQuanLy.CapUBNDXa)
                {
                    DataRow dr = dt.NewRow();
                    //dr["TenCoQuan"] = "<b style='text-transform: uppercase'>" + "UBND Cấp Xã" + "</b>";
                    //dr["CssClass"] = "highlight";
                    dr["TenCoQuan"] = "UBND Cấp Xã";
                    dr["CssClass"] = "font-weight:bold;text-transform: uppercase";
                    dr["CapID"] = CapCoQuanViewChiTiet.CapUBNDXa.GetHashCode();
                    dr["ThuTu"] = 6000;// 1.5;
                    dr["CoQuanID"] = 0;
                    //DataTable dtnew = BaoCao2b_DataTable_New();

                    // lấy danh sách huyện
                    var danhSachXa = dt.AsEnumerable().Where(row => Utils.ConvertToInt32(row.Field<string>("CapID"), 0) == (int)CapQuanLy.CapUBNDXa).ToList();
                    if (danhSachXa != null && danhSachXa.Count > 0)
                    {
                        var danhSachHuyenID = (from r in danhSachXa.AsEnumerable() select r["CoQuanChaID"]).Distinct().ToList();
                        var danhSachHuyen = new List<DanhMucCoQuanDonViModelPartial>();
                        if (danhSachHuyenID != null && danhSachHuyenID.Count > 0)
                        {
                            danhSachHuyen = new DanhMucCoQuanDonViDAL().DanhSachUBNDHuyen();
                        }
                        var stt = 100;
                        foreach (var item in danhSachHuyen)
                        {
                            DataRow dr1 = dt.NewRow();
                            dr1["TenCoQuan"] = item.Ten.Replace("UBND", "");
                            dr1["CssClass"] = "font-weight: bold; text-transform: uppercase";
                            dr1["CapID"] = CapCoQuanViewChiTiet.CapUBNDXa.GetHashCode();
                            dr1["ThuTu"] = 6000 + stt;
                            dr1["CoQuanID"] = 0;
                            dt.Rows.Add(dr1);
                            foreach (DataRow dro in dt.Rows)
                            {
                                if (Utils.ConvertToInt32(dro["CoQuanChaID"], 0) == item.ID)
                                {
                                    for (var i = 1; i <= 37; i++)
                                    {
                                        if (dro["Col" + i + "Data"] == null)
                                        {
                                            dro["Col" + i + "Data"] = 0;

                                        }
                                        dr["Col" + i + "Data"] = Utils.ConvertToDouble(dr["Col" + i + "Data"], 0) +
                                            Utils.ConvertToDouble(dro["Col" + i + "Data"], 0);
                                        dr1["Col" + i + "Data"] = Utils.ConvertToDouble(dr1["Col" + i + "Data"], 0) +
                                            Utils.ConvertToDouble(dro["Col" + i + "Data"], 0);
                                    }

                                    dr["slCol15"] = Utils.ConvertToDouble(dr["slCol15"], 0) +
                                           Utils.ConvertToDouble(dro["slCol15"], 0);

                                    dr["slCol17"] = Utils.ConvertToDouble(dr["slCol17"], 0) +
                                           Utils.ConvertToDouble(dro["slCol17"], 0);

                                    dr["slCol20"] = Utils.ConvertToDouble(dr["slCol20"], 0) +
                                           Utils.ConvertToDouble(dro["slCol20"], 0);

                                    dr["slCol25"] = Utils.ConvertToDouble(dr["slCol25"], 0) +
                                           Utils.ConvertToDouble(dro["slCol25"], 0);

                                    dr["slCol30"] = Utils.ConvertToDouble(dr["slCol30"], 0) +
                                           Utils.ConvertToDouble(dro["slCol30"], 0);

                                    dr["slCol34"] = Utils.ConvertToDouble(dr["slCol34"], 0) +
                                           Utils.ConvertToDouble(dro["slCol34"], 0);
                                    dro["ThuTu"] = Utils.ConvertToInt32(dr1["ThuTu"], 0) + 1;
                                }
                            }
                            stt = stt + 100;
                        }
                    }
                    dt.Rows.Add(dr);
                }
            }
            dt.DefaultView.Sort = "ThuTu";
            dt = dt.DefaultView.ToTable();
        }
        public static void CalculateBaoCaoCapSo(string ContentRootPath, int RoleID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, ref List<BaoCao2cInfo> resultList, DateTime startDate, DateTime endDate, ref DataTable dt)
        {
            int? CoQuanChaID = 0;
            //CoQuanInfo cqInfo = new CoQuan().GetCoQuanByID(CoQuanDangNhapID);
            //BaoCao2cInfo bc2bInfo2 = new BaoCao2cInfo();
            //bc2bInfo2.DonVi = cqInfo.TenCoQuan;
            //bc2bInfo2.CapID = (int)CapQuanLy.CapSoNganh;
            //Lay tat ca don thu trong nam, bao gom ca don ki truoc + ki nay
            List<CoQuanInfo> cqlist = new List<CoQuanInfo>();
            DateTime firstDay = Season.GetFirstDayOfYear(startDate.Year);
            //26/08/2018 Sửa firstDay --> startDate
            var pList = new SqlParameter("@ListCoQuanID", SqlDbType.Structured);
            pList.TypeName = "dbo.IntList";
            var tbCoQuanID = new DataTable();
            tbCoQuanID.Columns.Add("CoQuanID", typeof(string));
            //List<BaoCao2bDonThuInfo> dtList = new DAL.BaoCao2b().GetDonThu(startDate, endDate, cqInfo.CoQuanID).ToList();
            var listThanhTraTinh = new SystemConfigDAL().GetByKey("Thanh_Tra_Tinh_ID").ConfigValue.Split(',').ToList().Select(x => Utils.ConvertToInt32(x.ToString(), 0)).ToList();
            if (listThanhTraTinh.Contains(CoQuanDangNhapID) && RoleID == (int)EnumChucVu.LanhDao)
            {
                var CoQuanChaPhuHop = new CoQuan().GetCoQuanByTinhID_New(TinhDangNhapID);
                var ListCoQuan = new CoQuan().GetAllCapCon(CoQuanChaPhuHop.CoQuanID).Where(x => x.SuDungPM == true).ToList();
                ListCoQuan.ForEach(x => tbCoQuanID.Rows.Add(x.CoQuanID));
                cqlist = ListCoQuan;
            }
            else
            {
                tbCoQuanID.Rows.Add(CoQuanDangNhapID);
                cqlist = new List<CoQuanInfo> { new CoQuan().GetCoQuanByID(CoQuanDangNhapID) };
            }
            cqlist = cqlist.Where(x => x.SuDungPM == true).ToList();
            string filename = ContentRootPath + "/Upload/" + CanBoDangNhapID + "_CoQuan.xml";
            using (FileStream file = System.IO.File.Create(filename))
            {

            }
            XDocument doc = new XDocument();
            doc =
                               new XDocument(
                               new XElement("LogConfig", cqlist.Select(x =>
              new XElement("SystemLog", new XElement("CoQuan", new XAttribute("CoQuanID", x.CoQuanID),
                             new XAttribute("CapID", x.CapID), new XAttribute("CoQuanChaID", x.CoQuanChaID),
                             new XAttribute("SuDungPM", x.SuDungPM))))));

            doc.Save(filename);
            //BaoCao2cInfo bc2bInfo2 = new BaoCao2cInfo();
            SqlParameter[] parm = new SqlParameter[] {
                        new SqlParameter("@StartDate", SqlDbType.DateTime),
                        new SqlParameter("@EndDate", SqlDbType.DateTime),
                        pList,
                        new SqlParameter("@Flag", SqlDbType.Int)
                        ,
                         new SqlParameter("@CoQuanChaID", SqlDbType.Int)
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            parm[2].Value = tbCoQuanID;
            parm[3].Value = 1;
            parm[4].Value = CoQuanChaID ?? Convert.DBNull;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_BaoCaoKQGQ03", parm))
                {
                    dt.Load(dr);
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static void CalculateBaoCaoCapHuyen(string ContentRootPath, int RoleID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, int HuyenDangNhapID, ref List<BaoCao2cInfo> resultList, DateTime startDate, DateTime endDate, int tinhID, ref DataTable dt)
        {
            int CoQuanChaID = 0;
            List<CoQuanInfo> cqHuyenList = new List<CoQuanInfo>();
            if (RoleID == (int)EnumChucVu.LanhDao)
            {
                var CoQuanTinh = new CoQuan().GetCoQuanByTinhID_New(TinhDangNhapID);
                var CoQuanChaPhuHop = new CoQuan().GetCoQuanByHuyenID_New(HuyenDangNhapID, TinhDangNhapID, CoQuanTinh.CoQuanID);
                cqHuyenList = new CoQuan().GetAllCapCon(CoQuanChaPhuHop.CoQuanID).ToList();
                cqHuyenList = cqHuyenList.Where(x => x.SuDungPM == true).ToList();
                CoQuanChaID = CoQuanChaPhuHop.CoQuanID;
            }
            else
            {
                var CoQuanTinh = new CoQuan().GetCoQuanByTinhID_New(TinhDangNhapID);
                var CoQuanChaPhuHop = new CoQuan().GetCoQuanByHuyenID_New(HuyenDangNhapID, TinhDangNhapID, CoQuanTinh.CoQuanID);
                cqHuyenList = new CoQuan().GetAllCapCon(CoQuanChaPhuHop.CoQuanID).Where(x => (x.CapID == CapQuanLy.CapUBNDHuyen.GetHashCode()
                 || x.CapID == CapQuanLy.CapPhong.GetHashCode())).ToList();
                cqHuyenList = cqHuyenList.Where(x => x.SuDungPM == true).ToList();
            }

            string filename = ContentRootPath + "/Upload/" + CanBoDangNhapID + "_CoQuan.xml";
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            using (FileStream file = System.IO.File.Create(filename))
            {

            }
            XDocument doc = new XDocument();
            doc =
                               new XDocument(
                               new XElement("LogConfig", cqHuyenList.Select(x =>
              new XElement("SystemLog", new XElement("CoQuan", new XAttribute("CoQuanID", x.CoQuanID),
                             new XAttribute("CapID", x.CapID), new XAttribute("CoQuanChaID", x.CoQuanChaID),
                             new XAttribute("SuDungPM", x.SuDungPM))))));

            doc.Save(filename);
            var pList = new SqlParameter("@ListCoQuanID", SqlDbType.Structured);
            pList.TypeName = "dbo.IntList";
            var tbCoQuanID = new DataTable();
            tbCoQuanID.Columns.Add("CoQuanID", typeof(string));
            cqHuyenList.ForEach(x => tbCoQuanID.Rows.Add(x.CoQuanID));
            //BaoCao2cInfo bc2bInfo2 = new BaoCao2cInfo();
            SqlParameter[] parm = new SqlParameter[] {
                        new SqlParameter("@StartDate", SqlDbType.DateTime),
                        new SqlParameter("@EndDate", SqlDbType.DateTime),
                        pList,
                        new SqlParameter("@Flag", SqlDbType.Int),
                           new SqlParameter("@CoQuanChaID", SqlDbType.Int)
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            parm[2].Value = tbCoQuanID;
            parm[3].Value = 0;
            parm[4].Value = CoQuanChaID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_BaoCaoKQGQ03", parm))
                {
                    dt.Load(dr);
                }

            }
            catch (Exception e)
            {
                throw e;
            }

            // Toàn huyện
            DataRow dr1 = dt.NewRow();
            //dr1["TenCoQuan"] = "<b style='text-transform: uppercase'>" + "Toàn huyện" + "</b>";
            //dr1["CssClass"] = "highlight";
            dr1["TenCoQuan"] = "Toàn huyện";
            dr1["CssClass"] = "font-weight:bold;text-transform: uppercase";
            dr1["CapID"] = (int)CapCoQuanViewChiTiet.ToanHuyen;
            dr1["ThuTu"] = 2.75;
            dr1["CoQuanID"] = HuyenDangNhapID;
            //DataTable dtnew = BaoCao2b_DataTable_New();
            foreach (DataRow dro in dt.Rows)
            {
                if (/*/*Utils.ConvertToInt32(dro["CapID"], 0) == (int)CapQuanLy.CapUBNDTinh ||*/ /*Utils.ConvertToInt32(dro["CapID"], 0) == (int)CapQuanLy.CapSoNganh*/
                   /*|*/ Utils.ConvertToInt32(dro["CapID"], 0) == (int)CapQuanLy.CapUBNDHuyen || Utils.ConvertToInt32(dro["CapID"], 0) == (int)CapQuanLy.CapUBNDXa
                    || Utils.ConvertToInt32(dro["CapID"], 0) == (int)CapQuanLy.ToanHuyen || Utils.ConvertToInt32(dro["CapID"], 0) == (int)CapQuanLy.CapPhong)
                {
                    for (var i = 1; i <= 37; i++)
                    {


                        dr1["Col" + i + "Data"] = Utils.ConvertToDouble(dr1["Col" + i + "Data"], 0) +
                            Utils.ConvertToDouble(dro["Col" + i + "Data"], 0);
                    }

                    dr1["slCol15"] = Utils.ConvertToDouble(dr1["slCol15"], 0) +
                           Utils.ConvertToDouble(dro["slCol15"], 0);

                    dr1["slCol17"] = Utils.ConvertToDouble(dr1["slCol17"], 0) +
                           Utils.ConvertToDouble(dro["slCol17"], 0);

                    dr1["slCol20"] = Utils.ConvertToDouble(dr1["slCol20"], 0) +
                           Utils.ConvertToDouble(dro["slCol20"], 0);

                    dr1["slCol25"] = Utils.ConvertToDouble(dr1["slCol25"], 0) +
                           Utils.ConvertToDouble(dro["slCol25"], 0);

                    dr1["slCol30"] = Utils.ConvertToDouble(dr1["slCol30"], 0) +
                           Utils.ConvertToDouble(dro["slCol30"], 0);

                    dr1["slCol34"] = Utils.ConvertToDouble(dr1["slCol34"], 0) +
                           Utils.ConvertToDouble(dro["slCol34"], 0);


                }
            }
            dt.Rows.Add(dr1);
            // UBND Huyện
            dr1 = dt.NewRow();
            //dr1["TenCoQuan"] = "<b style='text-transform: uppercase'>" + "UBND Cấp Huyện" + "</b>";
            //dr1["CssClass"] = "highlight";
            dr1["TenCoQuan"] = "UBND Cấp Huyện";
            dr1["CssClass"] = "font-weight:bold;text-transform: uppercase";
            dr1["CapID"] = (int)CapCoQuanViewChiTiet.CapUBNDHuyen;
            dr1["ThuTu"] = 2.5;
            dr1["CoQuanID"] = 0;
            //DataTable dtnew = BaoCao2b_DataTable_New();
            foreach (DataRow dro in dt.Rows)
            {
                if (Utils.ConvertToInt32(dro["CapID"], 0) == (int)CapQuanLy.CapUBNDHuyen || Utils.ConvertToInt32(dro["CapID"], 0) == (int)CapQuanLy.CapPhong)
                {
                    for (var i = 1; i <= 37; i++)
                    {

                        dr1["Col" + i + "Data"] = Utils.ConvertToDouble(dr1["Col" + i + "Data"], 0) +
                                Utils.ConvertToDouble(dro["Col" + i + "Data"], 0);
                    }

                    dr1["slCol15"] = Utils.ConvertToDouble(dr1["slCol15"], 0) +
                           Utils.ConvertToDouble(dro["slCol15"], 0);

                    dr1["slCol17"] = Utils.ConvertToDouble(dr1["slCol17"], 0) +
                           Utils.ConvertToDouble(dro["slCol17"], 0);

                    dr1["slCol20"] = Utils.ConvertToDouble(dr1["slCol20"], 0) +
                           Utils.ConvertToDouble(dro["slCol20"], 0);

                    dr1["slCol25"] = Utils.ConvertToDouble(dr1["slCol25"], 0) +
                           Utils.ConvertToDouble(dro["slCol25"], 0);

                    dr1["slCol30"] = Utils.ConvertToDouble(dr1["slCol30"], 0) +
                           Utils.ConvertToDouble(dro["slCol30"], 0);

                    dr1["slCol34"] = Utils.ConvertToDouble(dr1["slCol34"], 0) +
                           Utils.ConvertToDouble(dro["slCol34"], 0);


                }
            }
            dt.Rows.Add(dr1);
            dr1 = dt.NewRow();
            //dr1["TenCoQuan"] = "<b style='text-transform: uppercase'>" + "UBND Cấp Xã" + "</b>";
            //dr1["CssClass"] = "highlight";
            dr1["TenCoQuan"] = "Toàn tỉnh";
            dr1["CssClass"] = "font-weight:bold;text-transform: uppercase";
            dr1["CapID"] = (int)CapCoQuanViewChiTiet.CapUBNDXa;
            dr1["ThuTu"] = 1.5;
            dr1["CoQuanID"] = 0;
            //DataTable dtnew = BaoCao2b_DataTable_New();
            foreach (DataRow dro in dt.Rows)
            {
                if (Utils.ConvertToInt32(dro["CapID"], 0) == (int)CapQuanLy.CapUBNDXa)
                {
                    for (var i = 1; i <= 37; i++)
                    {

                        dr1["Col" + i + "Data"] = Utils.ConvertToDouble(dr1["Col" + i + "Data"], 0) +
                                Utils.ConvertToDouble(dro["Col" + i + "Data"], 0);
                    }

                    dr1["slCol15"] = Utils.ConvertToDouble(dr1["slCol15"], 0) +
                           Utils.ConvertToDouble(dro["slCol15"], 0);

                    dr1["slCol17"] = Utils.ConvertToDouble(dr1["slCol17"], 0) +
                           Utils.ConvertToDouble(dro["slCol17"], 0);

                    dr1["slCol20"] = Utils.ConvertToDouble(dr1["slCol20"], 0) +
                           Utils.ConvertToDouble(dro["slCol20"], 0);

                    dr1["slCol25"] = Utils.ConvertToDouble(dr1["slCol25"], 0) +
                           Utils.ConvertToDouble(dro["slCol25"], 0);

                    dr1["slCol30"] = Utils.ConvertToDouble(dr1["slCol30"], 0) +
                           Utils.ConvertToDouble(dro["slCol30"], 0);

                    dr1["slCol34"] = Utils.ConvertToDouble(dr1["slCol34"], 0) +
                           Utils.ConvertToDouble(dro["slCol34"], 0);


                }
            }
            dt.Rows.Add(dr1);
            dt.DefaultView.Sort = "ThuTu desc";
            dt = dt.DefaultView.ToTable();

        }
        public static void CalculateBaoCaoCapXa(string ContentRootPath, int CoQuanDangNhapID, int CanBoDangNhapID, ref List<BaoCao2cInfo> resultList, DateTime startDate, DateTime endDate, ref DataTable dt)
        {
            List<CoQuanInfo> cqlist = new List<CoQuanInfo>();
            //CoQuanInfo cqInfo = new CoQuan().GetCoQuanByID(CoQuanDangNhapID);

            //BaoCao2cInfo bc2bInfo2 = new BaoCao2cInfo();
            //bc2bInfo2.DonVi = cqInfo.TenCoQuan;
            //bc2bInfo2.CoQuanID = CoQuanDangNhapID;
            //bc2bInfo2.IsCoQuan = true;
            //Lay tat ca don thu trong nam, bao gom ca don ki truoc + ki nay
            DateTime firstDay = Season.GetFirstDayOfYear(startDate.Year);
            //26/08/2018 Sửa firstDay --> startDate
            //List<BaoCao2bDonThuInfo> dtList = new DAL.BaoCao2b().GetDonThu(startDate, endDate, cqInfo.CoQuanID).ToList();
            var pList = new SqlParameter("@ListCoQuanID", SqlDbType.Structured);
            pList.TypeName = "dbo.IntList";
            var tbCoQuanID = new DataTable();
            tbCoQuanID.Columns.Add("CoQuanID", typeof(string));
            tbCoQuanID.Rows.Add(CoQuanDangNhapID);
            //BaoCao2cInfo bc2bInfo2 = new BaoCao2cInfo();
            cqlist = new List<CoQuanInfo> { new CoQuan().GetCoQuanByID(CoQuanDangNhapID) };
            string filename = ContentRootPath + "/Upload/" + CanBoDangNhapID + "_CoQuan.xml";
            using (FileStream file = System.IO.File.Create(filename))
            {

            }
            XDocument doc = new XDocument();
            doc =
                               new XDocument(
                               new XElement("LogConfig", cqlist.Select(x =>
              new XElement("SystemLog", new XElement("CoQuan", new XAttribute("CoQuanID", x.CoQuanID),
                             new XAttribute("CapID", x.CapID), new XAttribute("CoQuanChaID", x.CoQuanChaID),
                             new XAttribute("SuDungPM", x.SuDungPM))))));

            doc.Save(filename);
            SqlParameter[] parm = new SqlParameter[] {
                        new SqlParameter("@StartDate", SqlDbType.DateTime),
                        new SqlParameter("@EndDate", SqlDbType.DateTime),
                        pList,
                        new SqlParameter("@Flag", SqlDbType.Int),
                          new SqlParameter("@CoQuanChaID", SqlDbType.Int)
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            parm[2].Value = tbCoQuanID;
            parm[3].Value = 0;
            parm[4].Value = 0;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_BaoCaoKQGQ03", parm))
                {
                    dt.Load(dr);
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static void CalculateBaoCaoCapTrungUong(ref List<BaoCao2cInfo> resultList, DateTime startDate, DateTime endDate, int tinhID)
        {
            BaoCao2cInfo bcInfo = new BaoCao2cInfo();
            List<CapInfo> capList = new CapDAL().GetAll().ToList();
            foreach (CapInfo capInfo in capList)
            {
                if (capInfo.CapID == (int)CapQuanLy.CapUBNDTinh)
                {
                    BaoCao2cInfo bc2cInfo = new BaoCao2cInfo();
                    bc2cInfo.DonVi = "<b style='text-transform: uppercase'>" + capInfo.TenCap + "</b>";
                    bc2cInfo.CapID = (int)CapQuanLy.CapTrungUong;
                    resultList.Add(bc2cInfo);
                    List<CoQuanInfo> cqList = new CoQuan().GetCoQuanByCapAndTinhID(capInfo.CapID, tinhID).ToList();
                    if (cqList.Count > 0)
                    {
                        foreach (CoQuanInfo cqInfo in cqList)
                        {
                            BaoCao2cInfo bc2cInfo2 = new BaoCao2cInfo();
                            bc2cInfo2.DonVi = cqInfo.TenCoQuan;

                            //Lay tat ca don thu trong nam, bao gom ca don ki truoc + ki nay
                            DateTime firstDays = startDate;
                            //List<BaoCao2cDonThuInfo> dtList = new DAL.BaoCao2c().GetDonThuByTinh(firstDays, endDate, cqInfo.TinhID).ToList();
                            //Calculate(ref bc2cInfo2, dtList, startDate);   
                            resultList.Add(bc2cInfo2);

                        }
                    }
                }
            }
        }
        public List<TKDonThuInfo> KQGQ03_GetDSChiTietDonThu(string ContentRootPath, DateTime startDate, DateTime endDate, int start, int pagesize, int? Index, int? xemTaiLieuMat, int? canBoID, int coQuanID, int? capID)
        {
            List<CoQuanInfo> ListCoQuan = new List<CoQuanInfo>();
            endDate = endDate.AddDays(1);
            string filename = ContentRootPath + "Upload/" + canBoID + "_CoQuan.xml";
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(filename);
            XmlNode root = xdoc.DocumentElement;
            XmlNodeList nodeList = root.SelectNodes("/LogConfig/SystemLog/CoQuan");

            foreach (XmlNode node in nodeList)
            {
                CoQuanInfo cq = new CoQuanInfo();
                cq.CoQuanID = int.Parse(node.Attributes.GetNamedItem("CoQuanID").Value);
                cq.CapID = int.Parse(node.Attributes.GetNamedItem("CapID").Value);
                cq.CoQuanChaID = int.Parse(node.Attributes.GetNamedItem("CoQuanChaID").Value);
                cq.SuDungPM = bool.Parse(node.Attributes.GetNamedItem("SuDungPM").Value);
                ListCoQuan.Add(cq);
            }
            ListCoQuan = ListCoQuan.Where(x => x.SuDungPM == true).ToList();

            if (capID == (int)CapCoQuanViewChiTiet.ToanTinh) // toàn tỉnh
            {
                ListCoQuan = ListCoQuan;
            }
            else if (capID == (int)CapCoQuanViewChiTiet.ToanHuyen) // toàn huyện
            {
                ListCoQuan = ListCoQuan.Where(x => x.CapID == (int)CapQuanLy.CapUBNDHuyen || x.CapID == (int)CapQuanLy.CapPhong ||
                x.CapID == (int)CapQuanLy.CapUBNDXa).ToList();
            }
            else if (capID == (int)CapCoQuanViewChiTiet.CapSoNganh) // sở
            {
                ListCoQuan = ListCoQuan.Where(x => x.CapID == (int)CapQuanLy.CapSoNganh).ToList();
            }
            else if (capID == (int)CapCoQuanViewChiTiet.CapUBNDHuyen) // ubnd huyện
            {
                ListCoQuan = ListCoQuan.Where(x => x.CapID == (int)CapQuanLy.CapUBNDHuyen || x.CapID == (int)CapQuanLy.CapPhong).ToList();
            }
            else if (capID == (int)CapCoQuanViewChiTiet.CapUBNDTinh) // ubnd tỉnh
            {
                ListCoQuan = ListCoQuan.Where(x => x.CapID == (int)CapQuanLy.CapUBNDTinh).ToList();
            }
            else if (capID == (int)CapCoQuanViewChiTiet.CapUBNDXa) // ubnd xã
            {
                ListCoQuan = ListCoQuan.Where(x => x.CapID == (int)CapQuanLy.CapUBNDXa).ToList();
            }
            else
            {
                if (capID == (int)CapQuanLy.CapUBNDHuyen)
                {
                    ListCoQuan = ListCoQuan.Where(x => (x.CapID == (int)CapQuanLy.CapUBNDHuyen && x.CoQuanID == coQuanID) || (x.CapID == (int)CapQuanLy.CapPhong && x.CoQuanChaID == coQuanID)).ToList();
                }
                else
                {
                    ListCoQuan = new List<CoQuanInfo> { new CoQuan().GetCoQuanByID(coQuanID) };
                }

            }

            List<TKDonThuInfo> donThuLists = new List<TKDonThuInfo>();
            donThuLists = GetDSChiTietDonThu(startDate, endDate, ListCoQuan, start, pagesize, Index, xemTaiLieuMat, canBoID, capID).ToList();

            return donThuLists;
        }
        public List<TKDonThuInfo> GetDSChiTietDonThu(DateTime startDate, DateTime endDate, List<CoQuanInfo> ListCoQuan, int start, int pagesize, int? Index, int? xemTaiLieuMat, int? canBoID, int? capID)
        {
            List<TKDonThuInfo> infoList = new List<TKDonThuInfo>();
            var pList = new SqlParameter("@ListCoQuanID", SqlDbType.Structured);
            pList.TypeName = "dbo.IntList";
            var tbCoQuanID = new DataTable();
            tbCoQuanID.Columns.Add("CoQuanID", typeof(string));
            ListCoQuan.ForEach(x => tbCoQuanID.Rows.Add(x.CoQuanID));
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_STARTDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_ENDDATE, SqlDbType.DateTime),
               pList,
                new SqlParameter("@Index", SqlDbType.Int),
                new SqlParameter(PARAM_END, SqlDbType.Int),
                new SqlParameter(PARAM_START, SqlDbType.Int),
                 new SqlParameter("@CapID", SqlDbType.Int)
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            parm[2].Value = tbCoQuanID;
            parm[3].Value = Index ?? Convert.DBNull;
            parm[4].Value = pagesize;
            parm[5].Value = start;
            parm[6].Value = capID ?? Convert.DBNull;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_BaoCaoKQGQ03_GetDSChiTietDonThu", parm))
                {
                    while (dr.Read())
                    {
                        TKDonThuInfo dtInfo = new TKDonThuInfo();

                        dtInfo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        dtInfo.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
                        dtInfo.DonThuID = Utils.ConvertToInt32(dr["DonThuID"], 0);
                        dtInfo.HuongXuLyID = Utils.ConvertToInt32(dr["HuongGiaiQuyetID"], 0);

                        dtInfo.NgayNhapDon = Utils.ConvertToDateTime(dr["NgayNhapDon"], DateTime.MinValue);
                        dtInfo.KetQuaID = Utils.ConvertToInt32(dr["KetQuaID"], 0);

                        dtInfo.SoDon = Utils.ConvertToString(dr["SoDonThu"], string.Empty);
                        dtInfo.DiaChi = Utils.ConvertToString(dr["DiaChiCT"], string.Empty);
                        string noiDungDon = Utils.ConvertToString(dr["NoiDungTiep"], string.Empty);
                        if (noiDungDon == string.Empty)
                        {
                            noiDungDon = Utils.ConvertToString(dr["NoiDungDon"], string.Empty);
                        }
                        dtInfo.NoiDungDon = noiDungDon;
                        //   dtInfo.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"], 0);
                        dtInfo.TenChuDon = Utils.ConvertToString(dr["HoTen"], string.Empty);
                        //   dtInfo.DiaChi = Utils.ConvertToString(dr["DiaChiCT"], string.Empty);
                        //DateTime ngayNhapDon = Utils.ConvertToDateTime(dr["NgayTiep"], DateTime.MinValue);
                        //if (ngayNhapDon == DateTime.MinValue)
                        //{
                        //    ngayNhapDon = Utils.ConvertToDateTime(dr["NgayNhapDon"], DateTime.MinValue);
                        //}
                        //       dtInfo.NgayNhapDon = ngayNhapDon;
                        dtInfo.NgayNhapDonStr = Format.FormatDate(dtInfo.NgayNhapDon);
                        dtInfo.TenLoaiKhieuTo = Utils.ConvertToString(dr["TenLoaiKhieuTo"], string.Empty);
                        //  --   dtInfo.KetQuaID = Utils.ConvertToInt32(dr["KetQuaID"], 0);
                        dtInfo.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        dtInfo.TenHuongGiaiQuyet = Utils.ConvertToString(dr["TenHuongGiaiQuyet"], string.Empty);
                        if (dtInfo.HuongXuLyID == 0)
                        {
                            dtInfo.KetQuaID_Str = "Chưa giải quyết";
                        }
                        else if (dtInfo.HuongXuLyID == (int)HuongGiaiQuyetEnum.DeXuatThuLy && dtInfo.StateID != 10)
                        {
                            dtInfo.KetQuaID_Str = "Đang giải quyết";
                        }
                        else
                        {
                            dtInfo.KetQuaID_Str = "Đã giải quyết";
                        }
                        //List<FileHoSoInfo> fileYKienXL = new List<FileHoSoInfo>();
                        //List<FileHoSoInfo> fileBanHanhQD = new List<FileHoSoInfo>();
                        //if (dtInfo.XuLyDonID > 0)
                        //{
                        //    if (xemTaiLieuMat == 1)
                        //    {

                        //        fileYKienXL = new FileHoSoDAL().GetFileYKienXuLyByXuLyDonID(dtInfo.XuLyDonID).ToList();
                        //    }
                        //    else
                        //    {
                        //        fileYKienXL = new FileHoSoDAL().GetFileYKienXuLyByXuLyDonID(dtInfo.XuLyDonID).Where(x => x.IsBaoMat != true || x.NguoiUp == canBoID).ToList();
                        //    }

                        //    int step = 0;
                        //    for (int i = 0; i < fileYKienXL.Count; i++)
                        //    {
                        //        if (!string.IsNullOrEmpty(fileYKienXL[i].FileURL))
                        //        {
                        //            if (string.IsNullOrEmpty(fileYKienXL[i].TenFile))
                        //            {
                        //                string[] arrtenFile = fileYKienXL[i].FileURL.Split('/');
                        //                if (arrtenFile.Length > 0)
                        //                {
                        //                    string[] duoiFile = arrtenFile[arrtenFile.Length - 1].Split('_');
                        //                    if (duoiFile.Length > 0)
                        //                    {
                        //                        fileYKienXL[i].TenFile = duoiFile[duoiFile.Length - 1];
                        //                    }
                        //                    else
                        //                    {
                        //                        fileYKienXL[i].TenFile = arrtenFile[arrtenFile.Length - 1];
                        //                    }
                        //                }
                        //            }
                        //            fileYKienXL[i].FileURL = fileYKienXL[i].FileURL.Replace(" ", "%20");
                        //        }
                        //        step++;
                        //        if (fileYKienXL[i].IsBaoMat == false)
                        //        {
                        //            string sec_false = "<li style='margin-bottom: 5px;'>" + step + ". " + "<a href='" + fileYKienXL[i].FileURL + "' download>" + fileYKienXL[i].TenFile + "</a></li>";
                        //            dtInfo.TenHuongGiaiQuyet = "<div style='margin-bottom: 5px;'><span>" + dtInfo.TenHuongGiaiQuyet + "</span></div>" + "<ul>" + sec_false + "</ul>";
                        //        }
                        //        else
                        //        {
                        //            string sec_true = "<li style='margin-bottom: 5px;'>" + step + ". " + "<a style='color: #EE7600;' href=" + "/DownloadFileDonThuCT.aspx?url=" + fileYKienXL[i].FileURL + ">" + fileYKienXL[i].TenFile + "</a></li>";
                        //            dtInfo.TenHuongGiaiQuyet = "<div style='margin-bottom: 5px;'><span>" + dtInfo.TenHuongGiaiQuyet + "</span></div>" + "<ul>" + sec_true + "</ul>";
                        //        }
                        //    }
                        //}

                        //if (dtInfo.KetQuaID > 0)
                        //{
                        //    if (xemTaiLieuMat == 1)
                        //    {

                        //        fileBanHanhQD = new FileHoSoDAL().GetFileBanHanhQDByXuLyDonID(dtInfo.XuLyDonID).ToList();
                        //    }
                        //    else
                        //    {
                        //        fileBanHanhQD = new FileHoSoDAL().GetFileBanHanhQDByXuLyDonID(dtInfo.XuLyDonID).Where(x => x.IsBaoMat != true || x.NguoiUp == canBoID).ToList();
                        //    }

                        //    int steps = 0;
                        //    for (int j = 0; j < fileBanHanhQD.Count; j++)
                        //    {
                        //        if (!string.IsNullOrEmpty(fileBanHanhQD[j].FileURL))
                        //        {
                        //            if (string.IsNullOrEmpty(fileBanHanhQD[j].TenFile))
                        //            {
                        //                string[] arrtenFile = fileBanHanhQD[j].FileURL.Split('/');
                        //                if (arrtenFile.Length > 0)
                        //                {
                        //                    string[] duoiFile = arrtenFile[arrtenFile.Length - 1].Split('_');
                        //                    if (duoiFile.Length > 0)
                        //                    {
                        //                        fileBanHanhQD[j].TenFile = duoiFile[duoiFile.Length - 1];
                        //                    }
                        //                    else
                        //                    {
                        //                        fileBanHanhQD[j].TenFile = arrtenFile[arrtenFile.Length - 1];
                        //                    }
                        //                }
                        //            }
                        //            fileBanHanhQD[j].FileURL = fileBanHanhQD[j].FileURL.Replace(" ", "%20");
                        //        }
                        //        steps++;
                        //        if (fileBanHanhQD[j].IsBaoMat == false)
                        //        {
                        //            string sec_false = "<li style='margin-bottom: 5px;'>" + steps + ". " + "<a href='" + fileBanHanhQD[j].FileURL + "' download>" + fileBanHanhQD[j].TenFile + "</a></li>";
                        //            dtInfo.KetQuaID_Str = "<div style='margin-bottom: 5px;'><span>" + dtInfo.KetQuaID_Str + "</span></div>" + "<ul>" + sec_false + "</ul>";
                        //        }
                        //        else
                        //        {
                        //            string sec_true = "<li style='margin-bottom: 5px;'>" + steps + ". " + "<a style='color: #EE7600;' href=" + "/DownloadFileDonThuCT.aspx?url=" + fileBanHanhQD[j].FileURL + ">" + fileBanHanhQD[j].TenFile + "</a></li>";
                        //            dtInfo.KetQuaID_Str = "<div style='margin-bottom: 5px;'><span>" + dtInfo.KetQuaID_Str + "</span></div>" + "<ul>" + sec_true + "</ul>";
                        //        }
                        //    }
                        //}

                        infoList.Add(dtInfo);
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return infoList;
        }
        public DataTable BaoCao2c_DataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("DonVi", typeof(string));
            dt.Columns.Add("TenCoQuan", typeof(string));
            dt.Columns.Add("CoQuanID", typeof(string));
            dt.Columns.Add("Col1Data", typeof(string));
            dt.Columns.Add("Col2Data", typeof(string));
            dt.Columns.Add("Col3Data", typeof(string));
            dt.Columns.Add("Col4Data", typeof(string));
            dt.Columns.Add("Col5Data", typeof(string));
            dt.Columns.Add("Col6Data", typeof(string));
            dt.Columns.Add("Col7Data", typeof(string));
            dt.Columns.Add("Col8Data", typeof(string));
            dt.Columns.Add("Col9Data", typeof(string));
            dt.Columns.Add("Col10Data", typeof(string));
            dt.Columns.Add("Col11Data", typeof(string));
            dt.Columns.Add("Col12Data", typeof(string));
            dt.Columns.Add("Col13Data", typeof(string));
            dt.Columns.Add("Col14Data", typeof(string));
            dt.Columns.Add("Col15Data", typeof(string));
            dt.Columns.Add("Col16Data", typeof(string));
            dt.Columns.Add("Col17Data", typeof(string));
            dt.Columns.Add("Col18Data", typeof(string));
            dt.Columns.Add("Col19Data", typeof(string));
            dt.Columns.Add("Col20Data", typeof(string));
            dt.Columns.Add("Col21Data", typeof(string));
            dt.Columns.Add("Col22Data", typeof(string));
            dt.Columns.Add("Col23Data", typeof(string));
            dt.Columns.Add("Col24Data", typeof(string));
            dt.Columns.Add("Col25Data", typeof(string));
            dt.Columns.Add("Col26Data", typeof(string));
            dt.Columns.Add("Col27Data", typeof(string));
            dt.Columns.Add("Col28Data", typeof(string));
            dt.Columns.Add("Col29Data", typeof(string));
            dt.Columns.Add("Col30Data", typeof(string));
            dt.Columns.Add("Col31Data", typeof(string));
            dt.Columns.Add("Col32Data", typeof(string));
            dt.Columns.Add("Col33Data", typeof(string));
            dt.Columns.Add("Col34Data", typeof(string));
            dt.Columns.Add("Col35Data", typeof(string));
            dt.Columns.Add("Col36Data", typeof(string));
            dt.Columns.Add("Col37Data", typeof(string));
            dt.Columns.Add("slCol15", typeof(string));
            dt.Columns.Add("slCol17", typeof(string));
            dt.Columns.Add("slCol20", typeof(string));
            dt.Columns.Add("slCol25", typeof(string));
            dt.Columns.Add("slCol30", typeof(string));
            dt.Columns.Add("slCol34", typeof(string));
            dt.Columns.Add("CapID", typeof(string));
            dt.Columns.Add("IsCoQuan", typeof(string));
            dt.Columns.Add("CssClass", typeof(string));
            dt.Columns.Add("ThuTu", typeof(string));
            return dt;
        }
        public class BaoCao2cDonThuInfo
        {
            public int DonThuID { get; set; }
            public int CoQuanID { get; set; }
            public DateTime NgayNhapDon { get; set; }
            public int HuongGiaiQuyetID { get; set; }
            public Boolean TrungDon { get; set; }
            public int LoaiKetQuaID { get; set; }
            public int PhanTichKQID { get; set; }
            public int TienPhaiThu { get; set; }
            public int DatPhaiThu { get; set; }
            public int SoNguoiDuocTraQuyenLoi { get; set; }
            public int SoDoiTuongBiXuLy { get; set; }
            public int SoDoiTuongDaBiXuLy { get; set; }
            public DateTime NgayRaKQ { get; set; }
            public int ThiHanhID { get; set; }
            public int LoaiThiHanhID { get; set; }
            public int TienDaThu { get; set; }
            public int DatDaThu { get; set; }
            public int SoLan { get; set; }
            public DateTime NgayThiHanh { get; set; }
            public int TrangThaiDonID { get; set; }
            public int CQTiepNhanID { get; set; }
            public int RutDonID { get; set; }
            public string CQDaGiaiQuyet { get; set; }
            public int KetQuaGQLan2 { get; set; }
        }
    }
}
