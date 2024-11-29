using Com.Gosol.KNTC.DAL.DanhMuc;
using Com.Gosol.KNTC.DAL.HeThong;
using Com.Gosol.KNTC.DAL.KNTC;
using Com.Gosol.KNTC.Models.BaoCao;
using Com.Gosol.KNTC.Models.DanhMuc;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Ultilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using DataTable = System.Data.DataTable;
using OfficeOpenXml;

namespace Com.Gosol.KNTC.DAL.BaoCaoTT2024
{
    public class TCD01TT2024DAL
    {
        public List<TableData> TCD01TT2024(List<int> listCapChonBaoCao, string ContentRootPath, int RoleID, int CapID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, int HuyenDangNhapID, DateTime startDate, DateTime endDate)
        {
            List<TableData> data = new List<TableData>();
            DataTable dt = BaoCao2a_DataTable();
            List<BaoCao2aInfo> resultList = new List<BaoCao2aInfo>();

            int capID = CapID;
            int tinhID = TinhDangNhapID;
            bool flag = true;
            bool flagCapXa = true;
            bool flagToanHuyen = true;
            if (capID == (int)CapQuanLy.CapUBNDTinh)
            {
                List<CapInfo> capList = new List<CapInfo>();

                foreach (int item in listCapChonBaoCao)
                {
                    CapInfo capInfo = new CapInfo();
                    capInfo.CapID = item;
                    capInfo.TenCap = GetTenCap(item);
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

                CalculateBaoCaoCapTinh(ContentRootPath, RoleID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, ref resultList, startDate, endDate, capList, flag, tinhID, ref dt);
            }
            else if (capID == (int)CapQuanLy.CapTrungUong)
            {

                //CalculateBaoCaoCapTrungUong(ContentRootPath, RoleID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, ref resultList, startDate, endDate, tinhID);
            }
            else if (capID == (int)CapQuanLy.CapUBNDHuyen || capID == (int)CapQuanLy.CapPhong)
            {
                CalculateBaoCaoCapHuyen(ContentRootPath, RoleID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, HuyenDangNhapID, ref resultList, startDate, endDate, tinhID, ref dt);
            }
            else if (capID == (int)CapQuanLy.CapSoNganh)
            {
                var listThanhTraTinh = new SystemConfigDAL().GetByKey("Thanh_Tra_Tinh_ID").ConfigValue.Split(',').ToList().Select(x => Utils.ConvertToInt32(x.ToString(), 0)).ToList();
                if (listThanhTraTinh.Contains(CoQuanDangNhapID) && RoleID == (int)EnumChucVu.LanhDao)
                {
                    List<CapInfo> capList = new List<CapInfo>();
                    foreach (int item in listCapChonBaoCao)
                    {
                        CapInfo capInfo = new CapInfo();
                        capInfo.CapID = item;
                        capInfo.TenCap = GetTenCap(item);
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

                    CalculateBaoCaoCapTinh(ContentRootPath, RoleID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, ref resultList, startDate, endDate, capList, flag, tinhID, ref dt);
                }
                else

                { CalculateBaoCaoCapSo(ContentRootPath, RoleID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, ref resultList, startDate, endDate, ref dt); ; }
            }
            else if (capID == (int)CapQuanLy.CapUBNDXa || capID == (int)CapQuanLy.CapPhong)
            {

                CalculateBaoCaoCapXa(ContentRootPath, CoQuanDangNhapID, CanBoDangNhapID, ref resultList, startDate, endDate, ref dt);
            }

            int stt = 2;
            foreach (DataRow dro in dt.Rows)
            {
                TableData tableData = new TableData();
                tableData.ID = stt++;
                var DataArr = new List<RowItem>();
                double col4 = Utils.ConvertToDouble(dro["Col4Data"], 0);
                double col5 = Utils.ConvertToDouble(dro["Col5Data"], 0);
                double col6 = Utils.ConvertToDouble(dro["Col6Data"], 0);
                double col7 = Utils.ConvertToDouble(dro["Col7Data"], 0);
                double col13 = Utils.ConvertToDouble(dro["Col13Data"], 0);
                double col14 = Utils.ConvertToDouble(dro["Col14Data"], 0);
                double col15 = Utils.ConvertToDouble(dro["Col15Data"], 0);
                double col16 = Utils.ConvertToDouble(dro["Col16Data"], 0);
                double col22 = Utils.ConvertToDouble(dro["Col22Data"], 0);
                double col23 = Utils.ConvertToDouble(dro["Col23Data"], 0);
                double col24 = Utils.ConvertToDouble(dro["Col24Data"], 0);
                double col25 = Utils.ConvertToDouble(dro["Col25Data"], 0);
                double col1 = col4 + col13 + col22;
                double col2 = col5 + col14 + col23;
                double col3 = col6 + col7 + col15 + col16 + col24 + col25;

                int RowItem_CapID = Utils.ConvertToInt32(dro["CapID"], 0);
                string Css = "";
                if (RowItem_CapID == CapCoQuanViewChiTiet.ToanTinh.GetHashCode() || RowItem_CapID == CapCoQuanViewChiTiet.CapUBNDTinh.GetHashCode()
                    || RowItem_CapID == CapCoQuanViewChiTiet.ToanHuyen.GetHashCode() || RowItem_CapID == CapCoQuanViewChiTiet.CapSoNganh.GetHashCode()
                    || RowItem_CapID == CapCoQuanViewChiTiet.CapUBNDHuyen.GetHashCode() || RowItem_CapID == CapCoQuanViewChiTiet.CapUBNDXa.GetHashCode())
                {
                    Css = "font-weight: bold;";
                }
                int tmp = stt * 100000;
                bool? isEdit = null;
                RowItem RowItem1 = new RowItem(1 + tmp, dro["TenCoQuan"].ToString(), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), isEdit, dro["CssClass"].ToString(), ref DataArr);
                RowItem RowItem2 = new RowItem(2 + tmp, Utils.AddCommas(col1), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), isEdit, "text-align: right;" + Css, ref DataArr);
                //RowItem RowItem2 = new RowItem(2, dro["Col1Data"].ToString(), dro["CoQuanID"].ToString(), dro["CapID"].ToString(),null, "", ref DataArr);
                RowItem RowItem3 = new RowItem(3 + tmp, Utils.AddCommas(col2), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), isEdit, "text-align: right;" + Css, ref DataArr);
                //RowItem RowItem3 = new RowItem(3, dro["Col2Data"].ToString(), dro["CoQuanID"].ToString(), dro["CapID"].ToString(),null, "", ref DataArr);
                RowItem RowItem4 = new RowItem(4 + tmp, Utils.AddCommas(col3), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), isEdit, "text-align: right;" + Css, ref DataArr);
                //RowItem RowItem4 = new RowItem(4, dro["Col3Data"].ToString(), dro["CoQuanID"].ToString(), dro["CapID"].ToString(),null, "", ref DataArr);
                RowItem RowItem5 = new RowItem(5 + tmp, Utils.AddCommasDouble(dro["Col4Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), isEdit, "text-align:right;" + Css, ref DataArr);
                RowItem RowItem6 = new RowItem(6 + tmp, Utils.AddCommasDouble(dro["Col5Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), isEdit, "text-align:right;" + Css, ref DataArr);
                RowItem RowItem7 = new RowItem(7 + tmp, Utils.AddCommasDouble(dro["Col6Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), isEdit, "text-align:right;" + Css, ref DataArr);
                RowItem RowItem8 = new RowItem(8 + tmp, Utils.AddCommasDouble(dro["Col7Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), isEdit, "text-align:right;" + Css, ref DataArr);
                RowItem RowItem9 = new RowItem(9 + tmp, Utils.AddCommasDouble(dro["Col8Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), isEdit, "text-align: right;" + Css, ref DataArr);
                RowItem RowItem10 = new RowItem(10 + tmp, Utils.AddCommasDouble(dro["Col9Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), isEdit, "text-align: right;" + Css, ref DataArr);
                RowItem RowItem11 = new RowItem(11 + tmp, Utils.AddCommasDouble(dro["Col10Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), isEdit, "text-align: right;" + Css, ref DataArr);
                RowItem RowItem12 = new RowItem(12 + tmp, Utils.AddCommasDouble(dro["Col11Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), isEdit, "text-align: right;" + Css, ref DataArr);
                RowItem RowItem13 = new RowItem(13 + tmp, Utils.AddCommasDouble(dro["Col12Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), isEdit, "text-align: right;" + Css, ref DataArr);
                RowItem RowItem14 = new RowItem(14 + tmp, Utils.AddCommasDouble(dro["Col13Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), isEdit, "text-align: right;" + Css, ref DataArr);
                RowItem RowItem15 = new RowItem(15 + tmp, Utils.AddCommasDouble(dro["Col14Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), isEdit, "text-align: right;" + Css, ref DataArr);
                RowItem RowItem16 = new RowItem(16 + tmp, Utils.AddCommasDouble(dro["Col15Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), isEdit, "text-align: right;" + Css, ref DataArr);
                RowItem RowItem17 = new RowItem(17 + tmp, Utils.AddCommasDouble(dro["Col16Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), isEdit, "text-align: right;" + Css, ref DataArr);
                RowItem RowItem18 = new RowItem(18 + tmp, Utils.AddCommasDouble(dro["Col17Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), isEdit, "text-align: right;" + Css, ref DataArr);
                RowItem RowItem19 = new RowItem(19 + tmp, Utils.AddCommasDouble(dro["Col18Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), isEdit, "text-align: right;" + Css, ref DataArr);
                RowItem RowItem20 = new RowItem(20 + tmp, Utils.AddCommasDouble(dro["Col19Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), isEdit, "text-align: right;" + Css, ref DataArr);
                RowItem RowItem21 = new RowItem(21 + tmp, Utils.AddCommasDouble(dro["Col20Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), isEdit, "text-align: right;" + Css, ref DataArr);
                RowItem RowItem22 = new RowItem(22 + tmp, Utils.AddCommasDouble(dro["Col21Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), isEdit, "text-align: right;" + Css, ref DataArr);
                RowItem RowItem23 = new RowItem(23 + tmp, Utils.AddCommasDouble(dro["Col22Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), isEdit, "text-align: right;" + Css, ref DataArr);
                RowItem RowItem24 = new RowItem(24 + tmp, Utils.AddCommasDouble(dro["Col23Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), isEdit, "text-align: right;" + Css, ref DataArr);
                RowItem RowItem25 = new RowItem(25 + tmp, Utils.AddCommasDouble(dro["Col24Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), isEdit, "text-align: right;" + Css, ref DataArr);
                RowItem RowItem26 = new RowItem(26 + tmp, Utils.AddCommasDouble(dro["Col25Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), isEdit, "text-align: right;" + Css, ref DataArr);
                RowItem RowItem27 = new RowItem(27 + tmp, Utils.AddCommasDouble(dro["Col26Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), isEdit, "text-align: right;" + Css, ref DataArr);
                RowItem RowItem28 = new RowItem(28 + tmp, Utils.AddCommasDouble(dro["Col27Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), isEdit, "text-align: right;" + Css, ref DataArr);
                RowItem RowItem29 = new RowItem(29 + tmp, Utils.AddCommasDouble(dro["Col28Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), isEdit, "text-align: right;" + Css, ref DataArr);
                RowItem RowItem30 = new RowItem(30 + tmp, Utils.AddCommasDouble(dro["Col29Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), isEdit, "text-align: right;" + Css, ref DataArr);
                tableData.DataArr = DataArr;
                //List<TableData> DataChild = new List<TableData>();
                //TableData chiu = new TableData();
                //chiu.ID = 1000000 + tableData.ID;
                //chiu.ParentID = tableData.ID;
                //chiu.DataArr = DataArr;
                //DataChild.Add(chiu);
                //tableData.DataChild = DataChild;
                data.Add(tableData);
            }

            return data;
        }
        public static void CalculateBaoCaoCapTinh(string ContentRootPath, int RoleID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, ref List<BaoCao2aInfo> resultList, DateTime startDate, DateTime endDate, List<CapInfo> capList, bool flag, int tinhID, ref DataTable dt)
        {
            List<int> ListCapID = new List<int>();
            capList.ForEach(x => ListCapID.Add(x.CapID));
            int CoQuanChaID = 0;

            if (capList.Count > 0)
            {
                List<CoQuanInfo> cqList = new List<CoQuanInfo>();

                if (RoleID == (int)EnumChucVu.LanhDao)
                {
                    var CoQuanChaPhuHop = new CoQuan().GetCoQuanByTinhID_New(TinhDangNhapID);
                    if (ListCapID.Contains(CapQuanLy.CapUBNDHuyen.GetHashCode()))
                    {

                        var cqList1 = new CoQuan().GetAllCapCon(CoQuanChaPhuHop.CoQuanID).ToList().Where(x => (ListCapID.Contains(x.CapID) || x.CapID == (int)CapQuanLy.CapPhong)).ToList();
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
                    cqList = cqList.Where(x => x.SuDungPM == true).ToList();
                }
                if (cqList.Count > 0)
                {
                    //string filename = HttpContext.Current.Server.MapPath("~/Upload/" + CanBoDangNhapID + "_CoQuan.xml");
                    string filename = ContentRootPath + "\\Upload\\" + CanBoDangNhapID + "_CoQuan.xml";
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
                    //
                    var pListCon = new SqlParameter("@ListCoQuanConID", SqlDbType.Structured);
                    pListCon.TypeName = "dbo.OneID";
                    var tbCoQuanConID = new DataTable();
                    tbCoQuanConID.Columns.Add("CoQuanID", typeof(string));

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
                        using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "BaoCaoTCD01_TT2024", parm))
                        {
                            dt.Load(dr);
                            //string filename = HttpContext.Current.Server.MapPath("~/Upload/" + "Test6.xml");
                            ////Directory.CreateDirectory(filename);
                            //using (FileStream file = System.IO.File.Create(filename))
                            //{

                            //}
                            //           DataSet ds = new DataSet();
                            //           ds.Tables.Add(dt);
                            //               //ds.WriteXml(filename, XmlWriteMode.WriteSchema);
                            //           XDocument doc = new XDocument();
                            //           //var ListSystemLog = GetAllSystemLogFromTo(TuNgay, DenNgay);
                            //           //dt.AsEnumerable();
                            //           doc =
                            //                 new XDocument(
                            //                 new XElement("LogConfig", dt.AsEnumerable().Select(x =>
                            //new XElement("SystemLog", new XElement("ha" +  x.Field<string>("RowCount_New").ToString(), new XElement("CoQuanID", x.Field<string>("CoQuanID")),
                            //               new XElement("TenCoQuan", x.Field<string>("CapID")))))));

                            //           doc.Save(filename);
                            //           XmlDocument xd = new XmlDocument();
                            //           xd.Load(filename);
                            //           XmlNode root = xd.DocumentElement;
                            //           XmlNodeList nodeList = root.SelectNodes("/LogConfig/SystemLog");

                            //foreach (XmlNode node in nodeList) // for each <testcase> node
                            //{
                            //    //CommonLib.TestCase tc = new CommonLib.TestCase();
                            //    BaoCao2aInfo bc = new BaoCao2aInfo();
                            //    bc.CoQuanID = int.Parse(node["CoQuanID"].InnerText);
                            //    bc.DonVi = node["TenCoQuan"].InnerText;
                            //    resultList.Add(bc);
                            //}
                        }
                    }
                    catch (Exception e)
                    {
                        throw;
                    }

                }
            }


            #region Cong so lieu tong

            #endregion   /// Caa   /// Cần sửa
            //}

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
                    for (var i = 1; i <= 31; i++)
                    {
                        if (dro["Col" + i + "Data"] == null)
                        {
                            dro["Col" + i + "Data"] = 0;

                        }
                        dr["Col" + i + "Data"] = Utils.ConvertToDouble(dr["Col" + i + "Data"], 0) +
                            Utils.ConvertToDouble(dro["Col" + i + "Data"], 0);
                    }
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
                            for (var i = 1; i <= 31; i++)
                            {
                                if (dro["Col" + i + "Data"] == null)
                                {
                                    dro["Col" + i + "Data"] = 0;

                                }
                                dr["Col" + i + "Data"] = Utils.ConvertToDouble(dr["Col" + i + "Data"], 0) +
                                    Utils.ConvertToDouble(dro["Col" + i + "Data"], 0);
                            }
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
                //            for (var i = 1; i <= 31; i++)
                //            {

                //                if (dro["Col" + i + "Data"] == null)
                //                {
                //                    dro["Col" + i + "Data"] = 0;

                //                }
                //                dr["Col" + i + "Data"] = Utils.ConvertToDouble(dr["Col" + i + "Data"], 0) +
                //                    Utils.ConvertToDouble(dro["Col" + i + "Data"], 0);
                //            }
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
                    //        for (var i = 1; i <= 31; i++)
                    //        {
                    //            if (dro["Col" + i + "Data"] == null)
                    //            {
                    //                dro["Col" + i + "Data"] = 0;

                    //            }
                    //            dr["Col" + i + "Data"] = Utils.ConvertToDouble(dr["Col" + i + "Data"], 0) +
                    //                Utils.ConvertToDouble(dro["Col" + i + "Data"], 0);
                    //        }
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
                            for (var i = 1; i <= 31; i++)
                            {
                                if (dro["Col" + i + "Data"] == null)
                                {
                                    dro["Col" + i + "Data"] = 0;

                                }
                                dr["Col" + i + "Data"] = Utils.ConvertToDouble(dr["Col" + i + "Data"], 0) +
                                    Utils.ConvertToDouble(dro["Col" + i + "Data"], 0);
                            }
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
                                    for (var i = 1; i <= 31; i++)
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
            foreach (DataRow item in dt.Rows)
            {
                int LuotDanKhongDen = Utils.ConvertToInt32(item["Col31Data"], 0);
                if (LuotDanKhongDen == 0)
                {
                    item["Col31Data"] = "";
                }
                if (LuotDanKhongDen > 0)
                {
                    item["Col31Data"] = LuotDanKhongDen + " lượt lãnh đạo trực dân không đến";
                }
            }
        }

        public static void CalculateBaoCaoCapSo(string ContentRootPath, int RoleID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, ref List<BaoCao2aInfo> resultList, DateTime startDate, DateTime endDate, ref DataTable dt)
        {
            int? CoQuanChaID = 0;
            //CoQuanInfo cqInfo = new DAL.CoQuan().GetCoQuanByID(CoQuanDangNhapID);
            //BaoCao2aInfo bc2bInfo2 = new BaoCao2aInfo();
            //bc2bInfo2.DonVi = cqInfo.TenCoQuan;
            //bc2bInfo2.CapID = (int)CapQuanLy.CapSoNganh;
            //Lay tat ca don thu trong nam, bao gom ca don ki truoc + ki nay
            DateTime firstDay = Season.GetFirstDayOfYear(startDate.Year);
            List<CoQuanInfo> cqList = new List<CoQuanInfo>();
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
                ListCoQuan.Where(x => x.SuDungPM == true).ToList();
                ListCoQuan.ForEach(x => tbCoQuanID.Rows.Add(x.CoQuanID));
                cqList = ListCoQuan;
            }
            else
            {
                tbCoQuanID.Rows.Add(CoQuanDangNhapID);
                cqList = new List<CoQuanInfo> { new CoQuan().GetCoQuanByID(CoQuanDangNhapID) };
            }
            string filename = ContentRootPath + "\\Upload\\" + CanBoDangNhapID + "_CoQuan.xml";
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
            //BaoCao2aInfo bc2bInfo2 = new BaoCao2aInfo();
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
            parm[4].Value = CoQuanChaID ?? Convert.DBNull;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "BaoCaoTCD01_TT2024", parm))
                {
                    dt.Load(dr);
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static void CalculateBaoCaoCapHuyen(string ContentRootPath, int RoleID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, int HuyenDangNhapID, ref List<BaoCao2aInfo> resultList, DateTime startDate, DateTime endDate, int tinhID, ref DataTable dt)
        {
            int CoQuanChaID = 0;
            List<CoQuanInfo> cqHuyenList = new List<CoQuanInfo>();
            if (RoleID == (int)EnumChucVu.LanhDao)
            {
                var CoQuanTinh = new CoQuan().GetCoQuanByTinhID_New(TinhDangNhapID);
                var CoQuanChaPhuHop = new CoQuan().GetCoQuanByHuyenID_New(HuyenDangNhapID, TinhDangNhapID, CoQuanTinh.CoQuanID);
                cqHuyenList = new CoQuan().GetAllCapCon(CoQuanChaPhuHop.CoQuanID).ToList();
                cqHuyenList.Where(x => x.SuDungPM == true).ToList();
                CoQuanChaID = CoQuanChaPhuHop.CoQuanID;
            }
            else
            {
                var CoQuanTinh = new CoQuan().GetCoQuanByTinhID_New(TinhDangNhapID);
                var CoQuanChaPhuHop = new CoQuan().GetCoQuanByHuyenID_New(HuyenDangNhapID, TinhDangNhapID, CoQuanTinh.CoQuanID);
                cqHuyenList = new CoQuan().GetAllCapCon(CoQuanChaPhuHop.CoQuanID).Where(x => (x.CapID == CapQuanLy.CapUBNDHuyen.GetHashCode()
                 || x.CapID == CapQuanLy.CapPhong.GetHashCode())).ToList();
                cqHuyenList.Where(x => x.SuDungPM == true).ToList();
            }
            string filename = ContentRootPath + "\\Upload\\" + CanBoDangNhapID + "_CoQuan.xml";
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
            //BaoCao2aInfo bc2bInfo2 = new BaoCao2aInfo();
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
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "BaoCaoTCD01_TT2024", parm))
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
                    for (var i = 1; i <= 31; i++)
                    {

                        if (dro["Col" + i + "Data"] == null)
                        {
                            dro["Col" + i + "Data"] = 0;

                        }
                        dr1["Col" + i + "Data"] = Utils.ConvertToDouble(dr1["Col" + i + "Data"], 0) +
                            Utils.ConvertToDouble(dro["Col" + i + "Data"], 0);
                    }

                }
            }
            dt.Rows.Add(dr1);
            // UBND Huyện
            dr1 = dt.NewRow();
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
                    for (var i = 1; i <= 31; i++)
                    {
                        if (dro["Col" + i + "Data"] == null)
                        {
                            dro["Col" + i + "Data"] = 0;

                        }
                        dr1["Col" + i + "Data"] = Utils.ConvertToDouble(dr1["Col" + i + "Data"], 0) +
                                Utils.ConvertToDouble(dro["Col" + i + "Data"], 0);
                    }

                }
            }
            dt.Rows.Add(dr1);
            dr1 = dt.NewRow();
            //dr1["TenCoQuan"] = "<b style='text-transform: uppercase'>" + "UBND Cấp Xã" + "</b>";
            //dr1["CssClass"] = "highlight";
            dr1["TenCoQuan"] = "UBND Cấp Xã";
            dr1["CssClass"] = "font-weight:bold;text-transform: uppercase";
            dr1["CapID"] = (int)CapCoQuanViewChiTiet.CapUBNDXa;
            dr1["ThuTu"] = 1.5;
            dr1["CoQuanID"] = 0;
            //DataTable dtnew = BaoCao2b_DataTable_New();
            foreach (DataRow dro in dt.Rows)
            {
                if (Utils.ConvertToInt32(dro["CapID"], 0) == (int)CapQuanLy.CapUBNDXa)
                {
                    for (var i = 1; i <= 31; i++)
                    {
                        if (dro["Col" + i + "Data"] == null)
                        {
                            dro["Col" + i + "Data"] = 0;

                        }
                        dr1["Col" + i + "Data"] = Utils.ConvertToDouble(dr1["Col" + i + "Data"], 0) +
                                Utils.ConvertToDouble(dro["Col" + i + "Data"], 0);
                    }

                }
            }
            dt.Rows.Add(dr1);

            dt.DefaultView.Sort = "ThuTu desc";
            dt = dt.DefaultView.ToTable();
            foreach (DataRow item in dt.Rows)
            {
                int LuotDanKhongDen = Utils.ConvertToInt32(item["Col31Data"], 0);
                if (LuotDanKhongDen == 0)
                {
                    item["Col31Data"] = "";
                }
                if (LuotDanKhongDen > 0)
                {
                    item["Col31Data"] = LuotDanKhongDen + " lượt lãnh đạo trực dân không đến";
                }
            }
        }

        public static void CalculateBaoCaoCapXa(string ContentRootPath, int CoQuanDangNhapID, int CanBoDangNhapID, ref List<BaoCao2aInfo> resultList, DateTime startDate, DateTime endDate, ref DataTable dt)
        {
            //CoQuanInfo cqInfo = new DAL.CoQuan().GetCoQuanByID(CoQuanDangNhapID);
            List<CoQuanInfo> cqList = new List<CoQuanInfo>();
            //BaoCao2aInfo bc2bInfo2 = new BaoCao2aInfo();
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
            cqList = new List<CoQuanInfo> { new CoQuan().GetCoQuanByID(CoQuanDangNhapID) };
            cqList = cqList.Where(x => x.SuDungPM == true).ToList();
            string filename = ContentRootPath + "\\Upload\\" + CanBoDangNhapID + "_CoQuan.xml";
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
            //BaoCao2aInfo bc2bInfo2 = new BaoCao2aInfo();
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
            parm[4].Value = 1;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "BaoCaoTCD01_TT2024", parm))
                {
                    dt.Load(dr);
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<TKDonThuInfo> TCD01_TT2024_GetDSChiTietDonThu(string ContentRootPath, DateTime startDate, DateTime endDate, int start, int pagesize, int? Index, int? xemTaiLieuMat, int? canBoID, int coQuanID, int? capID)
        {
            List<CoQuanInfo> ListCoQuan = new List<CoQuanInfo>();
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
                //ListCoQuan = ListCoQuan;
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


            List<TKDonThuInfo> infoList = new List<TKDonThuInfo>();
            var pList = new SqlParameter("@ListCoQuanID", SqlDbType.Structured);
            pList.TypeName = "dbo.IntList";
            var tbCoQuanID = new DataTable();
            var tmp = ListCoQuan.Select(x => x.CoQuanID).ToList();
            tbCoQuanID.Columns.Add("CoQuanID", typeof(string));
            ListCoQuan.ForEach(x => tbCoQuanID.Rows.Add(x.CoQuanID));
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter("@StartDate", SqlDbType.DateTime),
                new SqlParameter("@EndDate", SqlDbType.DateTime),
               pList,
                new SqlParameter("@Index", SqlDbType.Int),
                new SqlParameter("@End", SqlDbType.Int),
                new SqlParameter("@Start", SqlDbType.Int),
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
                //var query = new DataTable();
                //SqlDataReader dr1 = SQLHelper.ExecuteReader(SQLHelper.CONN_BACKEND, CommandType.StoredProcedure, "BaoCaoPhanAnhKienNghi_GetDSChiTietDonThu_New", parm);
                //query.Load(dr1);

                //var l = ListCoQuan.Select(x => x.CoQuanID).ToList();
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "BaoCaoTCD01_GetDSChiTietDonThu_New_TT2024", parm))
                {
                    while (dr.Read())
                    {
                        TKDonThuInfo dtInfo = new TKDonThuInfo();
                        dtInfo.DonThuID = Utils.ConvertToInt32(dr["DonThuID"], 0);
                        dtInfo.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
                        dtInfo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        dtInfo.SoLuong = Utils.ConvertToInt32(dr["SoLuong"], 0);
                        dtInfo.GapLanhDao = Utils.ConvertToBoolean(dr["GapLanhDao"], false);
                        dtInfo.LoaiKhieuTo1ID = Utils.ConvertToInt32(dr["LoaiKhieuTo1ID"], 0);
                        dtInfo.LoaiKhieuTo2ID = Utils.ConvertToInt32(dr["LoaiKhieuTo2ID"], 0);
                        dtInfo.LoaiKhieuTo3ID = Utils.ConvertToInt32(dr["LoaiKhieuTo3ID"], 0);
                        dtInfo.LoaiKhieuToID = Utils.ConvertToInt32(dr["LoaiKhieuToID"], 0);
                        dtInfo.VuViecCu = Utils.ConvertToBoolean(dr["VuViecCu"], false);
                        dtInfo.SoDon = Utils.ConvertToString(dr["SoDonThu"], string.Empty);
                        string noiDungDon = Utils.ConvertToString(dr["NoiDungTiep"], string.Empty);
                        if (noiDungDon == string.Empty)
                        {
                            noiDungDon = Utils.ConvertToString(dr["NoiDungDon"], string.Empty);
                        }
                        dtInfo.NoiDungDon = noiDungDon;
                        dtInfo.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"], 0);
                        dtInfo.TenChuDon = Utils.ConvertToString(dr["HoTen"], string.Empty);
                        dtInfo.DiaChi = Utils.ConvertToString(dr["DiaChiCT"], string.Empty);
                        DateTime ngayNhapDon = Utils.ConvertToDateTime(dr["NgayTiep"], DateTime.MinValue);
                        if (ngayNhapDon == DateTime.MinValue)
                        {
                            ngayNhapDon = Utils.ConvertToDateTime(dr["NgayNhapDon"], DateTime.MinValue);
                        }
                        dtInfo.NgayNhapDon = ngayNhapDon;
                        dtInfo.NgayNhapDonStr = Format.FormatDate(dtInfo.NgayNhapDon);
                        dtInfo.TenLoaiKhieuTo = Utils.ConvertToString(dr["TenLoaiKhieuTo"], string.Empty);
                        dtInfo.KetQuaID = Utils.ConvertToInt32(dr["KetQuaID"], 0);
                        dtInfo.HuongXuLyID = Utils.ConvertToInt32(dr["HuongGiaiQuyetID"], 0);
                        dtInfo.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        dtInfo.TenHuongGiaiQuyet = Utils.ConvertToString(dr["TenHuongGiaiQuyet"], string.Empty);
                        dtInfo.NgayCapNhat = Utils.ConvertToDateTime(dr["NgayCapNhat"], DateTime.MinValue);
                        if (dtInfo.HuongXuLyID == 31)
                        {
                            if (string.IsNullOrEmpty(dtInfo.SoDon))
                            {
                                dtInfo.KetQuaID_Str = "Đã giải quyết";
                            }
                            else if (dtInfo.HuongXuLyID == 0)
                            {
                                dtInfo.KetQuaID_Str = "";
                            }
                            else if (dtInfo.StateID == 7 || dtInfo.StateID == 6 || dtInfo.StateID == 18 || dtInfo.StateID == 19 || (dtInfo.StateID == 8 && dtInfo.NgayCapNhat == DateTime.MinValue))
                            {
                                dtInfo.KetQuaID_Str = "Chưa giải quyết";
                            }
                            else if (dtInfo.StateID == 9 || dtInfo.StateID == 22 || dtInfo.StateID == 21 || dtInfo.StateID == 25 || (dtInfo.StateID == 8 && dtInfo.NgayCapNhat != DateTime.MinValue))
                            {
                                dtInfo.KetQuaID_Str = "Đang giải quyết";
                            }
                            else if (dtInfo.StateID == 10)
                            {
                                dtInfo.KetQuaID_Str = "Đã giải quyết";
                            }
                            else
                            {
                                dtInfo.KetQuaID_Str = "Chưa giải quyết";
                            }
                        }
                        else if (string.IsNullOrEmpty(dtInfo.SoDon))
                        {
                            dtInfo.KetQuaID_Str = "Đã giải quyết";
                        }
                        else if (dtInfo.HuongXuLyID == 0)
                        {
                            dtInfo.KetQuaID_Str = "";
                        }
                        else if (dtInfo.StateID == 7 || dtInfo.StateID == 6 || dtInfo.StateID == 18 || dtInfo.StateID == 19 || (dtInfo.StateID == 8 && dtInfo.NgayCapNhat == DateTime.MinValue))
                        {
                            dtInfo.KetQuaID_Str = "Chưa giải quyết";
                        }
                        else if (dtInfo.StateID == 9 || dtInfo.StateID == 22 || dtInfo.StateID == 21 || dtInfo.StateID == 25 || (dtInfo.StateID == 8 && dtInfo.NgayCapNhat != DateTime.MinValue))
                        {
                            dtInfo.KetQuaID_Str = "Đang giải quyết";
                        }
                        else if (dtInfo.StateID == 10)
                        {
                            dtInfo.KetQuaID_Str = "Đã giải quyết";
                        }
                        else
                        {
                            dtInfo.KetQuaID_Str = "Chưa giải quyết";
                        }                       
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
        public DataTable BaoCao2a_DataTable()
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
            //dt.Columns.Add("Col31Data", typeof(string));
            dt.Columns.Add("CapID", typeof(string));
            dt.Columns.Add("IsCoQuan", typeof(string));
            dt.Columns.Add("CssClass", typeof(string));
            dt.Columns.Add("ThuTu", typeof(string));
            dt.Columns.Add("RowCount_New", typeof(string));
            return dt;
        }
        public string GetTenCap(int CapID)
        {
            if (CapID == CapQuanLy.CapUBNDTinh.GetHashCode())
            {
                return "UBND Cấp Tỉnh";
            }
            else if (CapID == CapQuanLy.CapSoNganh.GetHashCode())
            {
                return "Cấp Sở, Ngành";
            }
            else if (CapID == CapQuanLy.CapUBNDHuyen.GetHashCode())
            {
                return "UBND Cấp Huyện";
            }
            else if (CapID == CapQuanLy.CapUBNDXa.GetHashCode())
            {
                return "UBND Cấp Xã";
            }
            else return "";
        }     

        public string TCD01_TT2024_Excel(string rootPath, string pathFile, List<TableData> data, DateTime tuNgay, DateTime denNgay)
        {
            //var camera = new Camera;
            // path to your excel file
            string path = rootPath + @"\Templates\BaoCao_TT2024\TCD01_TT2024.xlsx";
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

        public string DSChiTietDonThu_Excel(string rootPath, string pathFile, List<TableData> data, DateTime tuNgay, DateTime denNgay)
        {
            //var camera = new Camera;
            // path to your excel file
            string path = rootPath + @"\Templates\BaoCao\DSChiTietDonThu.xlsx";
            FileInfo fileInfo = new FileInfo(path);
            FileInfo file = new FileInfo(rootPath + "\\" + pathFile);

            ExcelPackage package = new ExcelPackage(fileInfo);
            if (package.Workbook.Worksheets != null)
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                // get number of rows in the sheet
                int rows = worksheet.Dimension.Rows;
                int cols = worksheet.Dimension.Columns;

                //string TuNgayDenNgay = "SO_LIEU_TINH_TU_NGAY_DEN_NGAY";

                // loop through the worksheet rows
                //for (int i = 1; i <= rows; i++)
                //{
                //    for (int j = 1; j <= cols; j++)
                //    {
                //        if (worksheet.Cells[i, j].Value != null && TuNgayDenNgay.Contains(worksheet.Cells[i, j].Value.ToString()))
                //        {
                //            worksheet.Cells[i, j].Value = "Số liệu tính từ ngày " + tuNgay.ToString("dd/MM/yyyy") + " đến ngày " + denNgay.ToString("dd/MM/yyyy");
                //        }
                //    }
                //}
                if (data.Count > 0)
                {
                    worksheet.InsertRow(4, data.Count - 1, 3);
                    //worksheet.DeleteRow(data.Count);
                    for (int i = 0; i < data.Count; i++)
                    {
                        if (data[i].DataArr != null && data[i].DataArr.Count > 0)
                        {
                            for (int j = 0; j < data[i].DataArr.Count; j++)
                            {
                                if (data[i].DataArr[j].Content != "0")
                                {
                                    worksheet.Cells[i + 3, j + 1].Value = data[i].DataArr[j].Content;
                                    if (data[i].DataArr[j].Style.Contains("bold")) worksheet.Cells[i + 3, j + 1].Style.Font.Bold = true;
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
    }
}
