using Com.Gosol.KNTC.DAL.HeThong;
using Com.Gosol.KNTC.DAL.KNTC;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.BaoCao;
using Com.Gosol.KNTC.Models.DanhMuc;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Ultilities;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Huyen = Com.Gosol.KNTC.DAL.KNTC.Huyen;
using RowItem = Com.Gosol.KNTC.Models.BaoCao.RowItem;
using Xa = Com.Gosol.KNTC.DAL.KNTC.Xa;

namespace Com.Gosol.KNTC.DAL.BaoCao
{
    public class BaoCaoDAL
    {
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
        public List<TableData> BaoCaoThongKeTheoLoaiKhieuTo(IdentityHelper IdentityHelper, List<int> ListCapID, int LoaiKhieuToID, int PhamViID, string ContentRootPath, DateTime startDate, DateTime endDate)
        {
            List<TableData> data = new List<TableData>();
            
            List<ThongKeInfo> bcList = new List<ThongKeInfo>();
            List<ThongKeInfo> resultList = new List<ThongKeInfo>();
            int canboID = IdentityHelper.CanBoID ?? 0;
            List<CoQuanInfo> ListCoQuan = new List<CoQuanInfo>();
            var LoaiKhieuTo = new LoaiKhieuTo().GetLoaiKhieuToByID(LoaiKhieuToID);
            int type = 0;
            int total = 0;        
            int tinhID = IdentityHelper.TinhID ?? 0; ;

            int phamviID = PhamViID;
            var listThanhTraTinh = new SystemConfigDAL().GetByKey("Thanh_Tra_Tinh_ID").ConfigValue.Split(',').ToList().Select(x => Utils.ConvertToInt32(x.ToString(), 0)).ToList();
            if ((IdentityHelper.CapID == (int)CapQuanLy.CapUBNDTinh) || (listThanhTraTinh.Contains(IdentityHelper.CoQuanID ?? 0)))
            {
                int CoQuanChaID = 0;

                if (ListCapID != null && ListCapID.Count > 0)
                {
                    var CoQuanChaPhuHop = new CoQuan().GetCoQuanByTinhID_New(IdentityHelper.TinhID);
                    if (ListCapID.Contains(CapQuanLy.CapUBNDHuyen.GetHashCode()))
                    {

                        var cqList1 = new CoQuan().GetAllCapCon(CoQuanChaPhuHop.CoQuanID).ToList().Where(x => (ListCapID.Contains(x.CapID) || x.CapID == (int)CapQuanLy.CapPhong)).ToList();
                        var cqList2 = cqList1.Where(x => x.CapID == CapQuanLy.CapUBNDHuyen.GetHashCode()).ToList();
                        ListCoQuan.AddRange(cqList2.Where(x => new CoQuan().GetAllCapCon(x.CoQuanID).ToList().Where(y => y.SuDungPM == true).ToList().Count > 0).Select(x => x));
                        ListCoQuan.AddRange(cqList1.Where(x => (x.CapID != (int)CapQuanLy.CapUBNDHuyen) && x.SuDungPM == true).Select(x => x));
                    }
                    else
                    {
                        ListCoQuan = new CoQuan().GetAllCapCon(CoQuanChaPhuHop.CoQuanID).ToList().Where(x => ListCapID.Contains(x.CapID) && x.SuDungPM == true)
                      .ToList();
                    }
                    CoQuanChaID = CoQuanChaPhuHop.CoQuanID;
                
                }
                else
                {
                    ListCoQuan = new List<CoQuanInfo> { new CoQuan().GetCoQuanByID(IdentityHelper.CoQuanID ?? 0) };
                }
            }
            else if (IdentityHelper.CapID == (int)CapQuanLy.CapSoNganh)
            {
                int cqID = IdentityHelper.CoQuanID ?? 0;
                var CoQuanTinhID = new CoQuan().GetCoQuanByTinhID_New(IdentityHelper.TinhID).CoQuanID;
                ListCoQuan = new CoQuan().GetAllCapCon(cqID).ToList();
            }
            else if (IdentityHelper.CapID == (int)CapQuanLy.CapUBNDHuyen || IdentityHelper.CapID == (int)CapQuanLy.CapPhong)
            {
                int cqID = IdentityHelper.CoQuanID ?? 0;
                var CoQuanTinhID = new CoQuan().GetCoQuanByTinhID_New(IdentityHelper.TinhID).CoQuanID;
                ListCoQuan = new CoQuan().GetAllCapCon(cqID).ToList();
            }
            else if (IdentityHelper.CapID == (int)CapQuanLy.CapUBNDXa)
            {
                int cqID = IdentityHelper.CoQuanID ?? 0;
                var CoQuanTinhID = new CoQuan().GetCoQuanByTinhID_New(IdentityHelper.TinhID).CoQuanID;
                ListCoQuan = new CoQuan().GetAllCapCon(cqID).ToList();
            }

            List<int> ListCoQuanID = ListCoQuan.Select(x => x.CoQuanID).ToList();
            List<LoaiKhieuToInfo> LstLkt = new LoaiKhieuTo().GetLoaiKhieuToByLoaiKhieuToChaID(LoaiKhieuToID).ToList();
            List<int> ListLoaiKhieuToID = LstLkt.Select(x => x.LoaiKhieuToID).ToList();
            List<LoaiKhieuToInfo> dtList = new LoaiKhieuTo().ThongKeLoaiKhieuTo_New(startDate, endDate, ListCoQuanID, ListLoaiKhieuToID).ToList();
            dtList.ForEach(x => x.Children = dtList.Where(y => y.LoaiKhieuToCha == x.LoaiKhieuToID).ToList());
            dtList.RemoveAll(x => x.LoaiKhieuToCha > 0);

            string filename = ContentRootPath + "Upload/" + IdentityHelper.CanBoID + "_CoQuan.xml"; ;
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
                               new XElement("LogConfig", ListCoQuan.Select(x =>
              new XElement("SystemLog", new XElement("CoQuan", new XAttribute("CoQuanID", x.CoQuanID),
                             new XAttribute("CapID", x.CapID), new XAttribute("CoQuanChaID", x.CoQuanChaID),
                             new XAttribute("SuDungPM", x.SuDungPM))))));

            doc.Save(filename);

            //rptReport.DataSource = bcList;

            int stt = 0;
            foreach (LoaiKhieuToInfo dro in dtList)
            {
                TableData tableData = new TableData();
                tableData.ID = stt++;
                var DataArr = new List<RowItem>();
  
                string Css = "";
                if (dro.LoaiKhieuToID == 1 || dro.LoaiKhieuToID == 8 || dro.LoaiKhieuToID == 9 || dro.LoaiKhieuToID == 71)
                {
                    Css = "font-weight: bold;";
                }
              
                RowItem RowItem1 = new RowItem(1, stt.ToString(), null, type.ToString(), dro.LoaiKhieuToID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem2 = new RowItem(2, dro.TenLoaiKhieuTo, null, type.ToString(), dro.LoaiKhieuToID.ToString(), null, "text-align: left;" + Css, ref DataArr);
                RowItem RowItem3 = new RowItem(3, dro.Tong.ToString(), null, type.ToString(), dro.LoaiKhieuToID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem4 = new RowItem(4, dro.DonTrucTiep.ToString(), null, type.ToString(), dro.LoaiKhieuToID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem5 = new RowItem(5, dro.DonGianTiep.ToString(), null, type.ToString(), dro.LoaiKhieuToID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                
                tableData.DataArr = DataArr;
                data.Add(tableData);
                //loai khieu to 2
                if(dro.Children != null && dro.Children.Count > 0)
                {
                    int j = 0;
                    foreach (var loai2 in dro.Children)
                    {
                        TableData tableDataChildren = new TableData();
                        tableDataChildren.ID = 100 + j++;
                        var DataArrChildren = new List<RowItem>();
                        RowItem IRowItem1 = new RowItem(1, stt + "." + j, null, type.ToString(), loai2.LoaiKhieuToID.ToString(), null, "text-align: center;", ref DataArrChildren);
                        RowItem IRowItem2 = new RowItem(2, loai2.TenLoaiKhieuTo, null, type.ToString(), loai2.LoaiKhieuToID.ToString(), null, "text-align: left;", ref DataArrChildren);
                        RowItem IRowItem3 = new RowItem(3, loai2.Tong.ToString(), null, type.ToString(), loai2.LoaiKhieuToID.ToString(), null, "text-align: center;", ref DataArrChildren);
                        RowItem IRowItem4 = new RowItem(4, loai2.DonTrucTiep.ToString(), null, type.ToString(), loai2.LoaiKhieuToID.ToString(), null, "text-align: center;", ref DataArrChildren);
                        RowItem IRowItem5 = new RowItem(5, loai2.DonGianTiep.ToString(), null, type.ToString(), loai2.LoaiKhieuToID.ToString(), null, "text-align: center;", ref DataArrChildren);

                        tableDataChildren.DataArr = DataArrChildren;
                        data.Add(tableDataChildren);
                        //loai khieu to 3
                        if (loai2.Children != null && loai2.Children.Count > 0)
                        {
                            int k = 0;
                            foreach (var loai3 in loai2.Children)
                            {
                                TableData tableDataChildren3 = new TableData();
                                tableDataChildren3.ID = 100 + k++;
                                var DataArrChildren3 = new List<RowItem>();
                                RowItem Row1 = new RowItem(1, stt + "." + j + "." + k, null, type.ToString(), loai3.LoaiKhieuToID.ToString(), null, "text-align: center;", ref DataArrChildren3);
                                RowItem Row2 = new RowItem(2, loai3.TenLoaiKhieuTo, null, type.ToString(), loai3.LoaiKhieuToID.ToString(), null, "text-align: left;", ref DataArrChildren3);
                                RowItem Row3 = new RowItem(3, loai3.Tong.ToString(), null, type.ToString(), loai3.LoaiKhieuToID.ToString(), null, "text-align: center;", ref DataArrChildren3);
                                RowItem Row4 = new RowItem(4, loai3.DonTrucTiep.ToString(), null, type.ToString(), loai3.LoaiKhieuToID.ToString(), null, "text-align: center;", ref DataArrChildren3);
                                RowItem Row5 = new RowItem(5, loai3.DonGianTiep.ToString(), null, type.ToString(), loai3.LoaiKhieuToID.ToString(), null, "text-align: center;", ref DataArrChildren3);

                                tableDataChildren3.DataArr = DataArrChildren3;
                                data.Add(tableDataChildren3);
                            }
                        }
                    }
                }
            }

            var soluong = 0;
            var DonTrucTiep = 0;
            var DonGianTiep = 0;
            soluong = dtList.Sum(x => x.Tong);
            DonTrucTiep = dtList.Sum(x => x.DonTrucTiep);
            DonGianTiep = dtList.Sum(x => x.DonGianTiep);

            TableData rowTong = new TableData();
            rowTong.ID = stt++;
            var DataArrTong = new List<RowItem>();
            RowItem RowItemTong1 = new RowItem(1, "", null, null, null, "text-align: center;", ref DataArrTong);
            RowItem RowItemTong2 = new RowItem(2, "Tổng cộng", null, null, null, "text-align: left;", ref DataArrTong);
            RowItem RowItemTong3 = new RowItem(3, soluong.ToString(), null, null, null, "text-align: center;", ref DataArrTong);
            RowItem RowItemTong4 = new RowItem(4, DonTrucTiep.ToString(), null, null, null, "text-align: center;", ref DataArrTong);
            RowItem RowItemTong5 = new RowItem(6, DonGianTiep.ToString(), null, null, null, "text-align: center;", ref DataArrTong);
            rowTong.DataArr = DataArrTong;
            data.Add(rowTong);

            return data;
        }

        public List<TKDonThuInfo> BaoCaoThongKeTheoLoaiKhieuTo_GetDSChiTietDonThu(IdentityHelper IdentityHelper, List<int> ListCapID, int LoaiKhieuToID, int CoQuanID, int Type, int start,int end, int Index, string ContentRootPath, DateTime startDate, DateTime endDate)
        {
            List<TKDonThuInfo> resultList = new List<TKDonThuInfo>();
            int TypeV = Type;
            int IndexV = 0;
            if(Index == 2)
            {
                IndexV = 1;// tong
            }
            else if (Index == 3)
            {
                IndexV = 2;// tiep dan
            }
            else
            {
                IndexV = 3;// tiep nhan don - gian tiep
            }
            int LoaiKhieuToIDV = LoaiKhieuToID;
            //int XemTaiLieuMatV = int.Parse(XemTaiLieuMat);
            int coQuanIDSelect = CoQuanID;

            int capID = IdentityHelper.CapID ?? 0;
            int tinhID = IdentityHelper.TinhID ?? 0;
            string data = "";
            var CanBoID = IdentityHelper.CanBoID ?? 0;
            try
            {
                List<CoQuanInfo> ListCoQuan = new List<CoQuanInfo>();
                string filename = ContentRootPath + "Upload/" + CanBoID + "_CoQuan.xml";
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(filename);
                XmlNode root = xdoc.DocumentElement;
                XmlNodeList nodeList = root.SelectNodes("/LogConfig/SystemLog/CoQuan");
                foreach (XmlNode node in nodeList)
                {
                    //CommonLib.TestCase tc = new CommonLib.TestCase();
                    CoQuanInfo cq = new CoQuanInfo();
                    cq.CoQuanID = int.Parse(node.Attributes.GetNamedItem("CoQuanID").Value);
                    cq.CapID = int.Parse(node.Attributes.GetNamedItem("CapID").Value);
                    cq.CoQuanChaID = int.Parse(node.Attributes.GetNamedItem("CoQuanChaID").Value);
                    cq.SuDungPM = bool.Parse(node.Attributes.GetNamedItem("SuDungPM").Value);
                    //bc.DonVi = node["TenCoQuan"].InnerText;
                    ListCoQuan.Add(cq);
                }

                var listThanhTraTinh = new SystemConfigDAL().GetByKey("Thanh_Tra_Tinh_ID").ConfigValue.Split(',').ToList().Select(x => Utils.ConvertToInt32(x.ToString(), 0)).ToList();
                if ((IdentityHelper.CapID == (int)CapQuanLy.CapUBNDTinh) || (listThanhTraTinh.Contains(IdentityHelper.CoQuanID ?? 0)))
                {
                    if (ListCapID != null && ListCapID.Count > 0)
                    {
                        var CoQuanChaPhuHop = new CoQuan().GetCoQuanByTinhID_New(IdentityHelper.TinhID);
                        if (ListCapID.Contains(CapQuanLy.CapUBNDHuyen.GetHashCode()))
                        {

                            var cqList1 = new CoQuan().GetAllCapCon(CoQuanChaPhuHop.CoQuanID).ToList().Where(x => (ListCapID.Contains(x.CapID) || x.CapID == (int)CapQuanLy.CapPhong)).ToList();
                            var cqList2 = cqList1.Where(x => x.CapID == CapQuanLy.CapUBNDHuyen.GetHashCode()).ToList();
                            ListCoQuan.AddRange(cqList2.Where(x => new CoQuan().GetAllCapCon(x.CoQuanID).ToList().Where(y => y.SuDungPM == true).ToList().Count > 0).Select(x => x));
                            ListCoQuan.AddRange(cqList1.Where(x => (x.CapID != (int)CapQuanLy.CapUBNDHuyen) && x.SuDungPM == true).Select(x => x));
                        }
                        else
                        {
                            ListCoQuan = new CoQuan().GetAllCapCon(CoQuanChaPhuHop.CoQuanID).ToList().Where(x => ListCapID.Contains(x.CapID) && x.SuDungPM == true)
                          .ToList();
                        }
                        //CoQuanChaID = CoQuanChaPhuHop.CoQuanID;
                    }
                    else
                    {
                        ListCoQuan = new List<CoQuanInfo> { new CoQuan().GetCoQuanByID(IdentityHelper.CoQuanID ?? 0) };
                    }


                    //var CoQuanTinhID = new CoQuan().GetCoQuanByTinhID_New(IdentityHelper.TinhID).CoQuanID;
                    //ListCoQuan = new CoQuan().GetAllCapCon(CoQuanTinhID).ToList();
                }


                List<LoaiKhieuToInfo> LstLkt = new LoaiKhieuTo().GetLoaiKhieuToByLoaiKhieuToChaID(LoaiKhieuToIDV).ToList();
                List<int> lsk = LstLkt.Select(x => x.LoaiKhieuToID).ToList();
                resultList = GETCHITIET_DanhSachDonThu_New(startDate, endDate, start, end, ListCoQuan, lsk, IndexV).ToList();

            }
            catch (Exception ex)
            {
            }

            return resultList;
        }

        public List<TKDonThuInfo> GETCHITIET_DanhSachDonThu(DateTime startDate, DateTime endDate, int Start, int CanBoID, List<CoQuanInfo> List, int End,
           List<int> ListLoaiKhieuToID, int? Type, int? Index, int? XemTaiLieuMat)
        {
            List<TKDonThuInfo> infoList = new List<TKDonThuInfo>();
            var pList = new SqlParameter("@ListCoQuanID", SqlDbType.Structured);
            pList.TypeName = "dbo.IntList";
            var tbCoQuanID = new System.Data.DataTable();
            var tmp = List.Select(x => x.CoQuanID).ToList();
            tbCoQuanID.Columns.Add("CoQuanID", typeof(string));
            List.ForEach(x => tbCoQuanID.Rows.Add(x.CoQuanID));

            var pList1 = new SqlParameter("@LoaiKhieuToID", SqlDbType.Structured);
            pList1.TypeName = "dbo.IntList";
            var tbLoaiKhieuToID = new System.Data.DataTable();
            //var tmp = List.Select(x => x.CoQuanID).ToList();
            tbLoaiKhieuToID.Columns.Add("LoaiKhieuToID", typeof(string));
            ListLoaiKhieuToID.ForEach(x => tbLoaiKhieuToID.Rows.Add(x));
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter("@StartDate", SqlDbType.DateTime),
                new SqlParameter("@EndDate", SqlDbType.DateTime),
                  new SqlParameter("@Start", SqlDbType.Int),
                    new SqlParameter("@End", SqlDbType.Int),
                    pList,
                    pList1,
                     new SqlParameter("@Type", SqlDbType.Int),
                      new SqlParameter("@Index", SqlDbType.Int)
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            parm[2].Value = Start;
            parm[3].Value = 39;
            parm[4].Value = tbCoQuanID;
            parm[5].Value = tbLoaiKhieuToID;
            parm[6].Value = Type;
            parm[7].Value = Index;
            //parm[8].Value = tbCoQuanID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "BC_ThongKeTheoLoaiKhieuTo_DanhSach", parm))
                {
                    while (dr.Read())
                    {
                        TKDonThuInfo info = new TKDonThuInfo();
                        info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        info.SoDon = Utils.ConvertToString(dr["SoDonThu"], string.Empty);
                        info.TenChuDon = Utils.ConvertToString(dr["HoTen"], string.Empty);
                        info.DiaChi = Utils.ConvertToString(dr["DiaChiCT"], string.Empty);
                        info.NoiDungDon = Utils.ConvertToString(dr["NoiDungDon"], string.Empty);
                        //info.TenLoaiKhieuTo = Utils.ConvertToString(dr["TenLoaiKhieuTo"], string.Empty);
                        info.NgayNhapDon = Utils.ConvertToDateTime(dr["NgayTiepNhan"], DateTime.MinValue);
                        info.NgayNhapDonStr = Ultilities.Format.FormatDate(info.NgayNhapDon);
                        //info.SoLuong = Utils.ConvertToInt32(dr["SoLuong"], 0);
                        info.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"], 0);
                        info.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
                        info.DonThuID = Utils.ConvertToInt32(dr["DonThuID"], 0);
                        info.HuongXuLyID = Utils.ConvertToInt32(dr["HuongGiaiQuyetID"], 0);
                        info.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        info.TenHuongGiaiQuyet = Utils.ConvertToString(dr["TenHuongGiaiQuyet"], string.Empty);
                        info.KetQuaID = Utils.ConvertToInt32(dr["KetQuaID"], 0);
                        info.TenLoaiKhieuTo = Utils.ConvertToString(dr["TenLoaiKhieuTo"], string.Empty);
                        var NgayCapNhat = DateTime.MinValue;
                        if (info.HuongXuLyID == 31)
                        {
                            if (string.IsNullOrEmpty(info.SoDon))
                            {
                                info.KetQuaID_Str = "Đã giải quyết";
                            }
                            else if (info.HuongXuLyID == 0)
                            {
                                info.KetQuaID_Str = "";
                            }
                            else if (info.StateID == 7 || info.StateID == 6 || info.StateID == 18 || info.StateID == 19 || (info.StateID == 8 && NgayCapNhat == DateTime.MinValue))
                            {
                                info.KetQuaID_Str = "Chưa giải quyết";
                            }
                            else if (info.StateID == 9 || info.StateID == 22 || info.StateID == 21 || info.StateID == 25 || (info.StateID == 8 && NgayCapNhat != DateTime.MinValue))
                            {
                                info.KetQuaID_Str = "Đang giải quyết";
                            }
                            else if (info.StateID == 10)
                            {
                                info.KetQuaID_Str = "Đã giải quyết";
                            }
                            else
                            {
                                info.KetQuaID_Str = "Chưa giải quyết";
                            }
                        }

                        //else
                        //{
                        //   info.KetQuaID_Str = "";
                        //}
                        else if (string.IsNullOrEmpty(info.SoDon))
                        {
                            info.KetQuaID_Str = "Đã giải quyết";
                        }
                        else if (info.HuongXuLyID == 0)
                        {
                            info.KetQuaID_Str = "";
                        }
                        else if (info.StateID == 7 || info.StateID == 6 || info.StateID == 18 || info.StateID == 19 || (info.StateID == 8 && NgayCapNhat == DateTime.MinValue))
                        {
                            info.KetQuaID_Str = "Chưa giải quyết";
                        }
                        else if (info.StateID == 9 || info.StateID == 22 || info.StateID == 21 || info.StateID == 25 || (info.StateID == 8 && NgayCapNhat != DateTime.MinValue))
                        {
                            info.KetQuaID_Str = "Đang giải quyết";
                        }
                        else if (info.StateID == 10)
                        {
                            info.KetQuaID_Str = "Đã giải quyết";
                        }
                        else
                        {
                            info.KetQuaID_Str = "Chưa giải quyết";
                        }
                        infoList.Add(info);
                        //if (string.IsNullOrEmpty(info.TenLoaiKhieuTo))
                        //{
                        //    info.TenLoaiKhieuTo = "";
                        //}
                        //if (info.HuongXuLyID == 0)
                        //{
                        //    info.KetQuaID_Str = "Chưa giải quyết";
                        //}
                        //else if (info.HuongXuLyID == (int)HuongGiaiQuyetEnum.DeXuatThuLy && info.StateID != 10)
                        //{
                        //    info.KetQuaID_Str = "Đang giải quyết";
                        //}
                        //else
                        //{
                        //    info.KetQuaID_Str = "Đã giải quyết";
                        //}
                        //var xemTaiLieuMat = 1;
                        //List<FileHoSoInfo> fileYKienXL = new List<FileHoSoInfo>();
                        //List<FileHoSoInfo> fileBanHanhQD = new List<FileHoSoInfo>();
                        //if (info.XuLyDonID > 0)
                        //{
                        //    if (XemTaiLieuMat == 1)
                        //    {

                        //        fileYKienXL = new DAL.FileHoSo().GetFileYKienXuLyByXuLyDonID(info.XuLyDonID).ToList();
                        //    }
                        //    else
                        //    {
                        //        fileYKienXL = new DAL.FileHoSo().GetFileYKienXuLyByXuLyDonID(info.XuLyDonID).Where(x => x.IsBaoMat != true || x.NguoiUp == CanBoID).ToList();
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
                        //            info.TenHuongGiaiQuyet = "<div style='margin-bottom: 5px;'><span>" + info.TenHuongGiaiQuyet + "</span></div>" + "<ul>" + sec_false + "</ul>";
                        //        }
                        //        else
                        //        {
                        //            string sec_true = "<li style='margin-bottom: 5px;'>" + step + ". " + "<a style='color: #EE7600;' href=" + "/DownloadFileDonThuCT.aspx?url=" + fileYKienXL[i].FileURL + ">" + fileYKienXL[i].TenFile + "</a></li>";
                        //            info.TenHuongGiaiQuyet = "<div style='margin-bottom: 5px;'><span>" + info.TenHuongGiaiQuyet + "</span></div>" + "<ul>" + sec_true + "</ul>";
                        //        }
                        //    }
                        //}
                        //if (info.KetQuaID > 0)
                        //{
                        //    if (XemTaiLieuMat == 1)
                        //    {

                        //        fileBanHanhQD = new DAL.FileHoSo().GetFileBanHanhQDByXuLyDonID(info.XuLyDonID).ToList();
                        //    }
                        //    else
                        //    {
                        //        fileBanHanhQD = new DAL.FileHoSo().GetFileBanHanhQDByXuLyDonID(info.XuLyDonID).Where(x => x.IsBaoMat != true || x.NguoiUp == CanBoID).ToList();
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
                        //            info.KetQuaID_Str = "<div style='margin-bottom: 5px;'><span>" + info.KetQuaID_Str + "</span></div>" + "<ul>" + sec_false + "</ul>";
                        //        }
                        //        else
                        //        {
                        //            string sec_true = "<li style='margin-bottom: 5px;'>" + steps + ". " + "<a style='color: #EE7600;' href=" + "/DownloadFileDonThuCT.aspx?url=" + fileBanHanhQD[j].FileURL + ">" + fileBanHanhQD[j].TenFile + "</a></li>";
                        //            info.KetQuaID_Str = "<div style='margin-bottom: 5px;'><span>" + info.KetQuaID_Str + "</span></div>" + "<ul>" + sec_true + "</ul>";
                        //        }
                        //    }
                        //}


                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            List<FileHoSoInfo> fileYKienXLAll = new List<FileHoSoInfo>();
            List<FileHoSoInfo> fileBanHanhQDAll = new List<FileHoSoInfo>();

            if (XemTaiLieuMat == 1)
            {
                fileYKienXLAll = new FileHoSoDAL().GetFileYKienXuLyByListXuLyDon(infoList).ToList();
                fileBanHanhQDAll = new FileHoSoDAL().GetFileBanHanhQDByListXuLyDon(infoList).ToList();
            }
            else
            {
                fileYKienXLAll = new FileHoSoDAL().GetFileYKienXuLyByListXuLyDon(infoList).Where(x => x.IsBaoMat != true || x.NguoiUp == CanBoID).ToList();
                fileBanHanhQDAll = new FileHoSoDAL().GetFileBanHanhQDByListXuLyDon(infoList).Where(x => x.IsBaoMat != true || x.NguoiUp == CanBoID).ToList();
            }

            if (fileYKienXLAll.Count > 0)
            {
                foreach (TKDonThuInfo info in infoList)
                {
                    List<FileHoSoInfo> fileYKienXL = new List<FileHoSoInfo>();
                    for (int i = 0; i < fileYKienXLAll.Count; i++)
                    {
                        if (fileYKienXLAll[i].XuLyDonID == info.XuLyDonID)
                        {
                            fileYKienXL.Add(fileYKienXLAll[i]);
                        }
                    }

                    int step = 0;
                    for (int i = 0; i < fileYKienXL.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(fileYKienXL[i].FileURL))
                        {
                            if (string.IsNullOrEmpty(fileYKienXL[i].TenFile))
                            {
                                string[] arrtenFile = fileYKienXL[i].FileURL.Split('/');
                                if (arrtenFile.Length > 0)
                                {
                                    string[] duoiFile = arrtenFile[arrtenFile.Length - 1].Split('_');
                                    if (duoiFile.Length > 0)
                                    {
                                        fileYKienXL[i].TenFile = duoiFile[duoiFile.Length - 1];
                                    }
                                    else
                                    {
                                        fileYKienXL[i].TenFile = arrtenFile[arrtenFile.Length - 1];
                                    }
                                }
                            }
                            fileYKienXL[i].FileURL = fileYKienXL[i].FileURL.Replace(" ", "%20");
                        }
                        step++;
                        if (fileYKienXL[i].IsBaoMat == false)
                        {
                            string sec_false = "<li style='margin-bottom: 5px;'>" + step + ". " + "<a href='" + fileYKienXL[i].FileURL + "' download>" + fileYKienXL[i].TenFile + "</a></li>";
                            info.TenHuongGiaiQuyet = "<div style='margin-bottom: 5px;'><span>" + info.TenHuongGiaiQuyet + "</span></div>" + "<ul>" + sec_false + "</ul>";
                        }
                        else
                        {
                            string sec_true = "<li style='margin-bottom: 5px;'>" + step + ". " + "<a style='color: #EE7600;' href=" + "/DownloadFileDonThuCT.aspx?url=" + fileYKienXL[i].FileURL + ">" + fileYKienXL[i].TenFile + "</a></li>";
                            info.TenHuongGiaiQuyet = "<div style='margin-bottom: 5px;'><span>" + info.TenHuongGiaiQuyet + "</span></div>" + "<ul>" + sec_true + "</ul>";
                        }
                    }
                }

            }
            if (fileBanHanhQDAll.Count > 0)
            {
                foreach (TKDonThuInfo info in infoList)
                {
                    List<FileHoSoInfo> fileBanHanhQD = new List<FileHoSoInfo>();
                    for (int i = 0; i < fileBanHanhQDAll.Count; i++)
                    {
                        if (fileBanHanhQDAll[i].XuLyDonID == info.XuLyDonID)
                        {
                            fileBanHanhQD.Add(fileBanHanhQDAll[i]);
                        }
                    }

                    int steps = 0;
                    for (int j = 0; j < fileBanHanhQD.Count; j++)
                    {
                        if (!string.IsNullOrEmpty(fileBanHanhQD[j].FileURL))
                        {
                            if (string.IsNullOrEmpty(fileBanHanhQD[j].TenFile))
                            {
                                string[] arrtenFile = fileBanHanhQD[j].FileURL.Split('/');
                                if (arrtenFile.Length > 0)
                                {
                                    string[] duoiFile = arrtenFile[arrtenFile.Length - 1].Split('_');
                                    if (duoiFile.Length > 0)
                                    {
                                        fileBanHanhQD[j].TenFile = duoiFile[duoiFile.Length - 1];
                                    }
                                    else
                                    {
                                        fileBanHanhQD[j].TenFile = arrtenFile[arrtenFile.Length - 1];
                                    }
                                }
                            }
                            fileBanHanhQD[j].FileURL = fileBanHanhQD[j].FileURL.Replace(" ", "%20");
                        }
                        steps++;
                        if (fileBanHanhQD[j].IsBaoMat == false)
                        {
                            string sec_false = "<li style='margin-bottom: 5px;'>" + steps + ". " + "<a href='" + fileBanHanhQD[j].FileURL + "' download>" + fileBanHanhQD[j].TenFile + "</a></li>";
                            info.KetQuaID_Str = "<div style='margin-bottom: 5px;'><span>" + info.KetQuaID_Str + "</span></div>" + "<ul>" + sec_false + "</ul>";
                        }
                        else
                        {
                            string sec_true = "<li style='margin-bottom: 5px;'>" + steps + ". " + "<a style='color: #EE7600;' href=" + "/DownloadFileDonThuCT.aspx?url=" + fileBanHanhQD[j].FileURL + ">" + fileBanHanhQD[j].TenFile + "</a></li>";
                            info.KetQuaID_Str = "<div style='margin-bottom: 5px;'><span>" + info.KetQuaID_Str + "</span></div>" + "<ul>" + sec_true + "</ul>";
                        }
                    }
                }
            }
            //infoList = infoList.Where(x => x.STT >= Start && x.STT <= (Start + 10)).ToList();
            return infoList;
        }

        public List<TKDonThuInfo> GETCHITIET_DanhSachDonThu_New(DateTime startDate, DateTime endDate, int Start, int End, List<CoQuanInfo> ListCoQuan, List<int> ListLoaiKhieuToID, int? Index)
        {
            List<TKDonThuInfo> infoList = new List<TKDonThuInfo>();
            var pList = new SqlParameter("@ListCoQuanID", SqlDbType.Structured);
            pList.TypeName = "dbo.IntList";
            var tbCoQuanID = new System.Data.DataTable();
            var tmp = ListCoQuan.Select(x => x.CoQuanID).ToList();
            tbCoQuanID.Columns.Add("CoQuanID", typeof(string));
            ListCoQuan.ForEach(x => tbCoQuanID.Rows.Add(x.CoQuanID));

            var pList1 = new SqlParameter("@LoaiKhieuToID", SqlDbType.Structured);
            pList1.TypeName = "dbo.IntList";
            var tbLoaiKhieuToID = new System.Data.DataTable();  
            tbLoaiKhieuToID.Columns.Add("LoaiKhieuToID", typeof(string));
            ListLoaiKhieuToID.ForEach(x => tbLoaiKhieuToID.Rows.Add(x));
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter("@StartDate", SqlDbType.DateTime),
                new SqlParameter("@EndDate", SqlDbType.DateTime),
                new SqlParameter("@Start", SqlDbType.Int),
                new SqlParameter("@End", SqlDbType.Int),
                pList,
                pList1,
                new SqlParameter("@Index", SqlDbType.Int)
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            parm[2].Value = Start;
            parm[3].Value = End;
            parm[4].Value = tbCoQuanID;
            parm[5].Value = tbLoaiKhieuToID;
            parm[6].Value = Index;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "BC_ThongKeTheoLoaiKhieuTo_DanhSach_New", parm))
                {
                    while (dr.Read())
                    {
                        TKDonThuInfo info = new TKDonThuInfo();
                        info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        info.SoDon = Utils.ConvertToString(dr["SoDonThu"], string.Empty);
                        info.TenChuDon = Utils.ConvertToString(dr["HoTen"], string.Empty);
                        info.DiaChi = Utils.ConvertToString(dr["DiaChiCT"], string.Empty);
                        info.NoiDungDon = Utils.ConvertToString(dr["NoiDungDon"], string.Empty);
                        //info.TenLoaiKhieuTo = Utils.ConvertToString(dr["TenLoaiKhieuTo"], string.Empty);
                        info.NgayNhapDon = Utils.ConvertToDateTime(dr["NgayTiepNhan"], DateTime.MinValue);
                        info.NgayNhapDonStr = Ultilities.Format.FormatDate(info.NgayNhapDon);
                        //info.SoLuong = Utils.ConvertToInt32(dr["SoLuong"], 0);
                        info.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"], 0);
                        info.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
                        info.DonThuID = Utils.ConvertToInt32(dr["DonThuID"], 0);
                        info.HuongXuLyID = Utils.ConvertToInt32(dr["HuongGiaiQuyetID"], 0);
                        info.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        info.TenHuongGiaiQuyet = Utils.ConvertToString(dr["TenHuongGiaiQuyet"], string.Empty);
                        info.KetQuaID = Utils.ConvertToInt32(dr["KetQuaID"], 0);
                        info.TenLoaiKhieuTo = Utils.ConvertToString(dr["TenLoaiKhieuTo"], string.Empty);
                        var NgayCapNhat = DateTime.MinValue;
                        if (info.HuongXuLyID == 31)
                        {
                            if (string.IsNullOrEmpty(info.SoDon))
                            {
                                info.KetQuaID_Str = "Đã giải quyết";
                            }
                            else if (info.HuongXuLyID == 0)
                            {
                                info.KetQuaID_Str = "";
                            }
                            else if (info.StateID == 7 || info.StateID == 6 || info.StateID == 18 || info.StateID == 19 || (info.StateID == 8 && NgayCapNhat == DateTime.MinValue))
                            {
                                info.KetQuaID_Str = "Chưa giải quyết";
                            }
                            else if (info.StateID == 9 || info.StateID == 22 || info.StateID == 21 || info.StateID == 25 || (info.StateID == 8 && NgayCapNhat != DateTime.MinValue))
                            {
                                info.KetQuaID_Str = "Đang giải quyết";
                            }
                            else if (info.StateID == 10)
                            {
                                info.KetQuaID_Str = "Đã giải quyết";
                            }
                            else
                            {
                                info.KetQuaID_Str = "Chưa giải quyết";
                            }
                        }
                        else if (string.IsNullOrEmpty(info.SoDon))
                        {
                            info.KetQuaID_Str = "Đã giải quyết";
                        }
                        else if (info.HuongXuLyID == 0)
                        {
                            info.KetQuaID_Str = "";
                        }
                        else if (info.StateID == 7 || info.StateID == 6 || info.StateID == 18 || info.StateID == 19 || (info.StateID == 8 && NgayCapNhat == DateTime.MinValue))
                        {
                            info.KetQuaID_Str = "Chưa giải quyết";
                        }
                        else if (info.StateID == 9 || info.StateID == 22 || info.StateID == 21 || info.StateID == 25 || (info.StateID == 8 && NgayCapNhat != DateTime.MinValue))
                        {
                            info.KetQuaID_Str = "Đang giải quyết";
                        }
                        else if (info.StateID == 10)
                        {
                            info.KetQuaID_Str = "Đã giải quyết";
                        }
                        else
                        {
                            info.KetQuaID_Str = "Chưa giải quyết";
                        }
                        infoList.Add(info);
                      
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return infoList;
        }

        public string BaoCaoThongKeTheoLoaiKhieuTo_Excel(string rootPath, string pathFile, List<TableData> data, DateTime tuNgay, DateTime denNgay)
        {
            //var camera = new Camera;
            // path to your excel file
            string path = rootPath + @"\Templates\BaoCao\ThongKeTheoLoaiKhieuTo.xlsx";
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
                    worksheet.InsertRow(6, data.Count - 1, 5);
                    //worksheet.DeleteRow(data.Count);
                    for (int i = 0; i < data.Count; i++)
                    {
                        if (data[i].DataArr != null && data[i].DataArr.Count > 0)
                        {
                            for (int j = 0; j < data[i].DataArr.Count; j++)
                            {
                                if (data[i].DataArr[j].Content != "0")
                                {
                                    worksheet.Cells[i + 5, j + 1].Value = data[i].DataArr[j].Content;
                                    if (data[i].DataArr[j].Style.Contains("bold")) worksheet.Cells[i + 5, j + 1].Style.Font.Bold = true;
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
      
        public List<TableData> THKQGiaiQuyetDonKienNghiPhanAnh(List<int> listCapChonBaoCao, IdentityHelper IdentityHelper, string ContentRootPath, DateTime startDate, DateTime endDate)
        {
            List<TableData> data = new List<TableData>();

           
            //for (int i = 0; i < cblCap.Items.Count; i++)
            //{
            //    if (cblCap.Items[i].Selected == true)
            //    {
            //        listCapChonBaoCao.Value += Utils.ConvertToInt32(cblCap.Items[i].Value, 0).ToString() + ",";
            //    }
            //}
            DateTime firstDays = startDate;
            var listThanhTraTinh = new SystemConfigDAL().GetByKey("Thanh_Tra_Tinh_ID").ConfigValue.Split(',').ToList().Select(x => Utils.ConvertToInt32(x.ToString(), 0)).ToList();
            //if (listThanhTraTinh.Contains(IdentityHelper.CoQuanID ?? 0) && IdentityHelper.RoleID == (int)EnumChucVu.LanhDao)
            //{
            //    hdfcapID.Value = "20"; // Cấp check thanh tra tỉnh
            //    ThanhTraTinh.Value = "0"; // Cấp check thanh tra tỉnh
            //}
            //else
            //{
            //    hdfcapID.Value = IdentityHelper.CapID.ToString();
            //    ThanhTraTinh.Value = IdentityHelper.CapID.ToString();
            //}
            //lblCurrentYear.Text = startDate.Year.ToString();
            if (startDate == DateTime.Now.Date && endDate == DateTime.Now.Date)
            {
                endDate = endDate.AddDays(1);
            }
            List<BaoCaoPhanAnhKienNghiInfo> resultList = new List<BaoCaoPhanAnhKienNghiInfo>();

            int capID = IdentityHelper.CapID ?? 0;
            int tinhID = IdentityHelper.TinhID ?? 0;
            bool flag = true;
            bool flagCapXa = true;
            bool flagToanHuyen = true;
            if (capID == (int)CapQuanLy.CapUBNDTinh /*|| (IdentityHelper.CapID == (int)CapQuanLy.CapSoNganh && IdentityHelper.RoleID == (int)EnumChucVu.LanhDao)*/)
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
                        //flagCapXa = false;
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
              
                BaoCaoPhanAnhKienNghiCalc.CalculateBaoCaoCapTinh(ref resultList, startDate, endDate, capList, flag, tinhID, IdentityHelper);
            }
            else if (capID == (int)CapQuanLy.CapTrungUong)
            {
                //Neu la cap trung uong, xem bao cao theo tinh
                BaoCaoPhanAnhKienNghiCalc.CalculateBaoCaoCapTrungUong(ref resultList, startDate, endDate, tinhID);
            }
            else if (capID == (int)CapQuanLy.CapUBNDHuyen)
            {
                BaoCaoPhanAnhKienNghiCalc.CalculateBaoCaoCapHuyen(ref resultList, startDate, endDate, tinhID, IdentityHelper);
            }
            else if (capID == (int)CapQuanLy.CapSoNganh)
            {
                List<CoQuanInfo> CoQuanInfoList = new List<CoQuanInfo>();
                /*var listThanhTraTinh1 = new SystemConfig().GetByKey("Thanh_Tra_Tinh_ID").ConfigValue.Split(',').ToList*//*().Select(x => Utils.ConvertToInt32(x.ToString(), 0)).ToList();*/
                if (listThanhTraTinh.Contains(IdentityHelper.CoQuanID ?? 0) && IdentityHelper.RoleID == (int)EnumChucVu.LanhDao)
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
                
                    BaoCaoPhanAnhKienNghiCalc.CalculateBaoCaoCapTinh(ref resultList, startDate, endDate, capList, flag, tinhID, IdentityHelper);
                }
                else
                {
                    BaoCaoPhanAnhKienNghiCalc.CalculateBaoCaoCapSo(ref resultList, startDate, endDate, IdentityHelper);
                }
            }
            else if (capID == (int)CapQuanLy.CapUBNDXa || capID == (int)CapQuanLy.CapPhong)
            {
                BaoCaoPhanAnhKienNghiCalc.CalculateBaoCaoCapXa(ref resultList, startDate, endDate, IdentityHelper);
            }

            int stt = 1;
            foreach (BaoCaoPhanAnhKienNghiInfo dro in resultList)
            {
                TableData tableData = new TableData();
                tableData.ID = stt++;
                var DataArr = new List<RowItem>();
                double col1 = dro.Col1Data + dro.Col2Data;
                int RowItem_CapID = dro.CapID;
                string Css = "";
                if (RowItem_CapID == CapCoQuanViewChiTiet.ToanTinh.GetHashCode() || RowItem_CapID == CapCoQuanViewChiTiet.CapUBNDTinh.GetHashCode()
                    || RowItem_CapID == CapCoQuanViewChiTiet.ToanHuyen.GetHashCode() || RowItem_CapID == CapCoQuanViewChiTiet.CapSoNganh.GetHashCode()
                    || RowItem_CapID == CapCoQuanViewChiTiet.CapUBNDHuyen.GetHashCode() || RowItem_CapID == CapCoQuanViewChiTiet.CapUBNDXa.GetHashCode())
                {
                    Css = "font-weight: bold;";
                }

                RowItem RowItem1 = new RowItem(1, dro.DonVi, dro.CoQuanID.ToString(), dro.CapID.ToString(), null, dro.Style, ref DataArr);
                RowItem RowItem2 = new RowItem(2, (dro.Col2Data + dro.Col3Data).ToString(), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem3 = new RowItem(3, dro.Col2Data.ToString(), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem4 = new RowItem(4, dro.Col3Data.ToString(), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem5 = new RowItem(5, dro.Col4Data.ToString(), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem6 = new RowItem(6, dro.Col4Data.ToString(), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem7 = new RowItem(7, dro.Col6Data.ToString(), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem8 = new RowItem(8, dro.Col7Data.ToString(), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem9 = new RowItem(9, dro.Col8Data.ToString(), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem10 = new RowItem(10, dro.Col9Data.ToString(), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem11 = new RowItem(11, dro.Col10Data.ToString(), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem12 = new RowItem(12, dro.Col11Data.ToString(), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem13 = new RowItem(13, dro.Col12Data.ToString(), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem14 = new RowItem(14, dro.Col13Data.ToString(), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem15 = new RowItem(15, dro.Col14Data.ToString(), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem16 = new RowItem(16, dro.Col15Data.ToString(), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem17 = new RowItem(17, dro.Col16Data.ToString(), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem18 = new RowItem(18, dro.Col17Data.ToString(), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem19 = new RowItem(19, dro.Col18Data.ToString(), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem20 = new RowItem(20, dro.Col19Data.ToString(), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem21 = new RowItem(21, dro.Col20Data.ToString(), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem22 = new RowItem(22, dro.Col21Data.ToString(), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem23 = new RowItem(23, dro.Col22Data.ToString(), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem24 = new RowItem(24, dro.Col23Data.ToString(), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem25 = new RowItem(25, dro.Col24Data.ToString(), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem26 = new RowItem(26, dro.Col25Data.ToString(), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem27 = new RowItem(27, dro.Col26Data.ToString(), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem28 = new RowItem(28, dro.Col27Data.ToString(), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem29 = new RowItem(29, dro.Col28Data.ToString(), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem30 = new RowItem(30, "", dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: left;" + Css, ref DataArr);

                //RowItem RowItem1 = new RowItem(1, dro.DonVi, dro.CoQuanID.ToString(), dro.CapID.ToString(), null, dro.Style, ref DataArr);
                //RowItem RowItem2 = new RowItem(2, Utils.AddCommas(col1), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                //RowItem RowItem3 = new RowItem(3, Utils.AddCommasDouble(dro.Col2Data.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                //RowItem RowItem4 = new RowItem(4, Utils.AddCommasDouble(dro.Col3Data.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                //RowItem RowItem5 = new RowItem(5, Utils.AddCommasDouble(dro.Col4Data.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                //RowItem RowItem6 = new RowItem(6, Utils.AddCommasDouble(dro.Col5Data.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                //RowItem RowItem7 = new RowItem(7, Utils.AddCommasDouble(dro.Col6Data.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                //RowItem RowItem8 = new RowItem(8, Utils.AddCommasDouble(dro.Col7Data.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                //RowItem RowItem9 = new RowItem(9, Utils.AddCommasDouble(dro.Col8Data.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                //RowItem RowItem10 = new RowItem(10, Utils.AddCommasDouble(dro.Col9Data.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                //RowItem RowItem11 = new RowItem(11, Utils.AddCommasDouble(dro.Col10Data.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                //RowItem RowItem12 = new RowItem(12, Utils.AddCommasDouble(dro.Col11Data.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                //RowItem RowItem13 = new RowItem(13, Utils.AddCommasDouble(dro.Col12Data.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                //RowItem RowItem14 = new RowItem(14, Utils.AddCommasDouble(dro.Col13Data.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                //RowItem RowItem15 = new RowItem(15, Utils.AddCommasDouble(dro.Col14Data.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                //RowItem RowItem16 = new RowItem(16, Utils.AddCommasDouble(dro.Col15Data.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                //RowItem RowItem17 = new RowItem(17, Utils.AddCommasDouble(dro.Col16Data.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                //RowItem RowItem18 = new RowItem(18, Utils.AddCommasDouble(dro.Col17Data.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                //RowItem RowItem19 = new RowItem(19, Utils.AddCommasDouble(dro.Col18Data.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                //RowItem RowItem20 = new RowItem(20, Utils.AddCommasDouble(dro.Col19Data.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                //RowItem RowItem21 = new RowItem(21, Utils.AddCommasDouble(dro.Col20Data.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                //RowItem RowItem22 = new RowItem(22, Utils.AddCommasDouble(dro.Col21Data.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                //RowItem RowItem23 = new RowItem(23, Utils.AddCommasDouble(dro.Col22Data.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                //RowItem RowItem24 = new RowItem(24, Utils.AddCommasDouble(dro.Col23Data.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                //RowItem RowItem25 = new RowItem(25, Utils.AddCommasDouble(dro.Col24Data.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                //RowItem RowItem26 = new RowItem(26, Utils.AddCommasDouble(dro.Col25Data.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                //RowItem RowItem27 = new RowItem(27, Utils.AddCommasDouble(dro.Col26Data.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                //RowItem RowItem28 = new RowItem(28, Utils.AddCommasDouble(dro.Col27Data.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                //RowItem RowItem29 = new RowItem(29, Utils.AddCommasDouble(dro.Col28Data.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                //RowItem RowItem30 = new RowItem(30,"", dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: left;" + Css, ref DataArr);

                tableData.DataArr = DataArr;
                data.Add(tableData);
            }


            return data;
        }

        public List<TKDonThuInfo> THKQGiaiQuyetDonKienNghiPhanAnh_GetDSChiTietDonThu(BaseReportParams p, IdentityHelper IdentityHelper, int start, int Index, string ContentRootPath, DateTime startDate, DateTime endDate)
        {
            //string capToanTinhID = Utils.ConvertToString(context.Request["capToanTinhID"], string.Empty);
            int capID = p.CapID ?? 0;
            int index = p.Index ?? 0;
            int tinhID = 0;
            //bool isCoQuan = Utils.ConvertToBoolean(context.Request["isCoQuan"], false);
            int coQuanID = p.CoQuanID ?? 0;
            int canBoID = p.CanBoID ?? 0;
            int xemTaiLieuMat = 1;
            //int page = Utils.ConvertToInt32(context.Request["page"], 0);
            //int getAll = Utils.ConvertToInt32(context.Request["getAll"], 0);
            int CapCoQuanDangNhap = IdentityHelper.CapID ?? 0;
            int coQuanDangNhapID = IdentityHelper.CoQuanID ?? 0; ;
            //string listCapChonBaoCao = context.Request["listCapChonBaoCao"];
            //int soLuong = Utils.ConvertToInt32(context.Request["soLuong"], 0);
            //string listCapChonBaoCao = "0,1,2,3,4";
            var tpm = p.ListCapIDStr.Split(',').ToList();
            var listCap = new List<int>();

            for (int i = 0; i < tpm.Count; i++)
            {
                switch (Utils.ConvertToInt32(tpm[i], 0))
                {
                    case 4:
                        listCap.Add(CapQuanLy.CapUBNDTinh.GetHashCode());
                        break;
                    case 1:
                        listCap.Add(CapQuanLy.CapSoNganh.GetHashCode());
                        break;
                    case 2:
                        {
                            listCap.Add(CapQuanLy.CapUBNDHuyen.GetHashCode());
                            listCap.Add(CapQuanLy.CapPhong.GetHashCode());
                            break;
                        }


                    case 3:
                        listCap.Add(CapQuanLy.CapUBNDXa.GetHashCode());
                        break;
                     
                }
            }

        
            List<CapInfo> tempList = new CapDAL().GetAll().ToList();
            List<CapInfo> capList = new List<CapInfo>();
            List<CoQuanInfo> ListCoQuan = new List<CoQuanInfo>();
            if (capID == (int)CapCoQuanViewChiTiet.ToanTinh) // toàn tỉnhl
            {
                var CoQuanChaPhuHop = new CoQuan().GetCoQuanByTinhID_New(tinhID);
                ListCoQuan = new CoQuan().GetAllCapCon(CoQuanChaPhuHop.CoQuanID).Where(x => listCap.Contains(x.CapID) && x.SuDungPM == true).ToList();
            }
            else if (capID == (int)CapCoQuanViewChiTiet.CapUBNDTinh) // ubnd tỉnh
            {
                var CoQuanChaPhuHop = new CoQuan().GetCoQuanByTinhID_New(tinhID);
                ListCoQuan = new CoQuan().GetAllCapCon(CoQuanChaPhuHop.CoQuanID).Where(x => x.CapID == (int)CapQuanLy.CapUBNDTinh && x.SuDungPM == true).ToList();
            }
            else if (capID == (int)CapCoQuanViewChiTiet.CapSoNganh) // sở
            {
                if (CapCoQuanDangNhap == CapQuanLy.CapSoNganh.GetHashCode())
                {
                    ListCoQuan = new CoQuan().GetCoQuanByCoQuanID(coQuanDangNhapID).ToList();
                    //.GetAllCapCon(coQuanDangNhapID).Where(x => x.CapID == (int)CapQuanLy.CapSoNganh).ToList();
                }
                else
                {
                    var CoQuanChaPhuHop = new CoQuan().GetCoQuanByTinhID_New(tinhID);
                    ListCoQuan = new CoQuan().GetAllCapCon(CoQuanChaPhuHop.CoQuanID).Where(x => x.CapID == (int)CapQuanLy.CapSoNganh && x.SuDungPM == true
                       /*|| x.CapID != (int)CapQuanLy.CapSoNganh || x.CapID != (int)CapQuanLy.CapTrungUong*/).ToList();
                }

            }
            else if (capID == (int)CapCoQuanViewChiTiet.ToanHuyen) // toàn huyện
            {

                if (CapCoQuanDangNhap == CapQuanLy.CapUBNDHuyen.GetHashCode())
                {
                    ListCoQuan = new CoQuan().GetAllCapCon(coQuanDangNhapID).Where(x =>
                   ((x.CapID == (int)CapQuanLy.CapPhong ||
                    x.CapID == (int)CapQuanLy.CapUBNDXa)) && x.SuDungPM == true)
                    .ToList();
                }
                else
                {
                    var CoQuanChaPhuHop = new CoQuan().GetCoQuanByTinhID_New(tinhID);
                    if (listCap.Contains((int)CapQuanLy.CapUBNDHuyen) && listCap.Contains((int)CapQuanLy.CapUBNDXa))
                    {
                        ListCoQuan = new CoQuan().GetAllCapCon(CoQuanChaPhuHop.CoQuanID).Where(x => (x.CapID == (int)CapQuanLy.CapUBNDHuyen || x.CapID == CapQuanLy.CapPhong.GetHashCode() || x.CapID == CapQuanLy.CapUBNDXa.GetHashCode()) && x.SuDungPM == true).ToList();
                    }
                    else if (listCap.Contains((int)CapQuanLy.CapUBNDHuyen))
                    {
                        ListCoQuan = new CoQuan().GetAllCapCon(CoQuanChaPhuHop.CoQuanID).Where(x => (x.CapID == (int)CapQuanLy.CapUBNDHuyen || x.CapID == CapQuanLy.CapPhong.GetHashCode()) && x.SuDungPM == true).ToList();
                    }
                    else
                    {
                        ListCoQuan = new CoQuan().GetAllCapCon(CoQuanChaPhuHop.CoQuanID).Where(x => (x.CapID == CapQuanLy.CapUBNDXa.GetHashCode()) && x.SuDungPM == true).ToList();
                    }

                }
            }
            else if (capID == (int)CapCoQuanViewChiTiet.CapUBNDHuyen) // ubnd huyện
            {
                if (CapCoQuanDangNhap == CapQuanLy.CapUBNDHuyen.GetHashCode())
                {
                    ListCoQuan = new CoQuan().GetAllCapCon(coQuanDangNhapID).Where(x =>
                    (x.CapID == (int)CapQuanLy.CapPhong))
                    .ToList();
                }
                else
                {
                    var CoQuanChaPhuHop = new CoQuan().GetCoQuanByTinhID_New(tinhID);
                    ListCoQuan = new CoQuan().GetAllCapCon(CoQuanChaPhuHop.CoQuanID).Where(x => (x.CapID == (int)CapQuanLy.CapUBNDHuyen || x.CapID == CapQuanLy.CapPhong.GetHashCode()) && x.SuDungPM == true).ToList();
                }
            }
            else if (capID == (int)CapCoQuanViewChiTiet.CapUBNDXa) // ubnd xã
            {
                if (CapCoQuanDangNhap == CapQuanLy.CapUBNDHuyen.GetHashCode())
                {
                    ListCoQuan = new CoQuan().GetAllCapCon(coQuanDangNhapID).Where(x => x.CapID == (int)CapQuanLy.CapUBNDXa && x.SuDungPM == true).ToList();
                }
                else
                {
                    var CoQuanChaPhuHop = new CoQuan().GetCoQuanByTinhID_New(tinhID);
                    ListCoQuan = new CoQuan().GetAllCapCon(CoQuanChaPhuHop.CoQuanID).Where(x => x.CapID == (int)CapQuanLy.CapUBNDXa && x.SuDungPM == true).ToList();
                }

            }
            else
            {
                if (capID == (int)CapQuanLy.CapUBNDHuyen)
                {
                    ListCoQuan = new CoQuan().GetAllCapCon(coQuanID).Where(x => (x.CapID == CapQuanLy.CapPhong.GetHashCode() || x.CapID == CapQuanLy.CapUBNDHuyen.GetHashCode()) && x.SuDungPM == true).ToList();
                }
                else
                {
                    ListCoQuan = new List<CoQuanInfo> { new CoQuan().GetCoQuanByID(coQuanID) };
                }

            }


            List<TKDonThuInfo> donThuLists = new List<TKDonThuInfo>();      
            int pagesize = p.PageSize;
            donThuLists = new BaoCaoPhanAnhKienNghi().GetDSChiTietDonThu(startDate, endDate, ListCoQuan, start, pagesize, index, xemTaiLieuMat, canBoID, capID).ToList();

            return donThuLists;
        }

        public string THKQGiaiQuyetDonKienNghiPhanAnh_Excel(string rootPath, string pathFile, List<TableData> data, DateTime tuNgay, DateTime denNgay)
        {
            //var camera = new Camera;
            // path to your excel file
            string path = rootPath + @"\Templates\BaoCao\THKQGiaiQuyetDonKienNghiPhanAnh.xlsx";
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
                    for (int i = 0; i < data.Count; i++)
                    {
                        if (data[i].DataArr != null && data[i].DataArr.Count > 0)
                        {
                            for (int j = 0; j < data[i].DataArr.Count; j++)
                            {
                                if (data[i].DataArr[j].Content != "0")
                                {
                                    worksheet.Cells[i + 8, j + 1].Value = data[i].DataArr[j].Content;
                                    //if (data[i].DataArr[j].Style.Contains("bold")) worksheet.Cells[i + 5, j + 1].Style.Font.Bold = true;
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

        public List<TableData> ThongKeTheoCOQuanChuyenDon(IdentityHelper IdentityHelper, string ContentRootPath, int PhamViID,int CoQuanID, DateTime startDate, DateTime endDate)
        {
            List<TableData> data = new List<TableData>();
            int total = 0;
            List<ThongKeInfo> resultList = new List<ThongKeInfo>();
            Boolean laThanhTraTinh = false;
            var listThanhTraTinh = new SystemConfigDAL().GetByKey("Thanh_Tra_Tinh_ID").ConfigValue.Split(',').ToList().Select(x => Utils.ConvertToInt32(x.ToString(), 0)).ToList();
            if (listThanhTraTinh.Contains(IdentityHelper.CoQuanID ?? 0) && IdentityHelper.RoleID == EnumChucVu.LanhDao.GetHashCode())
            {
                laThanhTraTinh = true;
            }
            int capID = IdentityHelper.CapID ?? 0;

            if (capID == (int)CapQuanLy.CapTrungUong)
            {
                int phamviID = PhamViID;

                //phaviID = 1: du lieu cap trung uong 
                //phamviID = 2: du lieu cap tinh
                //phamviID khac: du lieu cap huyen, so
                if (phamviID == 1)
                {
                    //ThongKeCQChuyenDonCalc.TinhSoLieuTrungUong(ref resultList, startDate, endDate, ref total);
                    ThongKeCQChuyenDonCalc.TinhSoLieuDonDiTrungUong(ref resultList, startDate, endDate, ref total);
                }
                else if (phamviID == 2)
                {
                    int tinhID = IdentityHelper.TinhID ?? 0;
                    ThongKeCQChuyenDonCalc.TinhSoLieuDonDiToanTinh(ref resultList, startDate, endDate, tinhID, ref total);
                }
            }
            else if (capID == (int)CapQuanLy.CapUBNDTinh || laThanhTraTinh)
            {
                if (IdentityHelper.RoleID == EnumChucVu.LanhDao.GetHashCode())
                {
                    int phamviID = PhamViID;

                    //phamviID = 2: du lieu cap tinh
                    //phamviID khac: du lieu cap huyen, so
                    if (phamviID == 2)
                    {
                        int tinhID = IdentityHelper.TinhID ?? 0;
                        ThongKeCQChuyenDonCalc.TinhSoLieuDonDiToanTinh(ref resultList, startDate, endDate, IdentityHelper.TinhID ?? 0, ref total);
                    }
                    else if (phamviID == 4)
                    {
                        int huyenID = CoQuanID;
                        ThongKeCQChuyenDonCalc.TinhSoLieuDonDiTheoHuyen(ref resultList, startDate, endDate, huyenID, ref total);
                    }
                    else
                    {
                        int cqID = CoQuanID;
                        if (cqID != 0)
                        {
                            ThongKeCQChuyenDonCalc.TinhSoLieuDonDiTheoCoQuan(ref resultList, startDate, endDate, cqID, ref total);
                        }
                    }
                }
                else
                {
                    int cqID = IdentityHelper.CoQuanID ?? 0;
                    ThongKeCQChuyenDonCalc.TinhSoLieuDonDiTheoCoQuan(ref resultList, startDate, endDate, cqID, ref total);
                }
            }
            else if (capID == (int)CapQuanLy.CapSoNganh)
            {
                int cqID = IdentityHelper.CoQuanID ?? 0;
                ThongKeCQChuyenDonCalc.TinhSoLieuDonDiTheoCoQuan(ref resultList, startDate, endDate, cqID, ref total);
            }
            else if (capID == (int)CapQuanLy.CapUBNDHuyen)
            {
                ThongKeCQChuyenDonCalc.TinhSoLieuDonDiTheoHuyen(ref resultList, startDate, endDate, IdentityHelper.HuyenID ?? 0, ref total);
            }
            else if (capID == (int)CapQuanLy.CapUBNDXa)
            {
                int cqID = IdentityHelper.CoQuanID ?? 0;
                ThongKeCQChuyenDonCalc.TinhSoLieuDonDiTheoCoQuan(ref resultList, startDate, endDate, cqID, ref total);
            }
            else if (capID == (int)CapQuanLy.CapPhong)
            {
                int cqID = IdentityHelper.CoQuanID ?? 0;
                ThongKeCQChuyenDonCalc.TinhSoLieuDonDiTheoCoQuan(ref resultList, startDate, endDate, cqID, ref total);
            }

            //rptReport.DataSource = resultList;
            int stt = 0;
            foreach (ThongKeInfo dro in resultList)
            {
                TableData tableData = new TableData();
                tableData.ID = stt++;
                var DataArr = new List<RowItem>();

                string Css = "";
                if (dro.LoaiKhieuToID == 1 || dro.LoaiKhieuToID == 8 || dro.LoaiKhieuToID == 9)
                {
                    Css = "font-weight: bold;";
                }

                RowItem RowItem1 = new RowItem(1, stt.ToString(), dro.CoQuanID.ToString(), null, null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem2 = new RowItem(2, dro.Ten, dro.CoQuanID.ToString(), null, null, "text-align: left;" + Css, ref DataArr);
                RowItem RowItem3 = new RowItem(3, dro.SoLuong.ToString(), dro.CoQuanID.ToString(), dro.Cap.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem4 = new RowItem(4, (dro.TyLe * 100).ToString("0.##"), dro.CoQuanID.ToString(), dro.Cap.ToString(), null, "text-align: center;" + Css, ref DataArr);
                
                tableData.DataArr = DataArr;
                data.Add(tableData);
            }

            return data;
        }

        public List<ThongKeInfo> ThongKeTheoCOQuanChuyenDon_GetDSCoQuanNhanDon(IdentityHelper IdentityHelper, int PhamViID, int CoQuanID, int Type, int start, int Index, string ContentRootPath, DateTime startDate, DateTime endDate)
        {
            List<ThongKeInfo> resultList = new List<ThongKeInfo>();

            try
            {
                DateTime start_Date = startDate;
                DateTime end_Date = endDate;
                int coQuanID = CoQuanID;
                int capID = IdentityHelper.CapID ?? 0;

                Boolean laThanhTraTinh = false;
                var listThanhTraTinh = new SystemConfigDAL().GetByKey("Thanh_Tra_Tinh_ID").ConfigValue.Split(',').ToList().Select(x => Utils.ConvertToInt32(x.ToString(), 0)).ToList();
                if (listThanhTraTinh.Contains(IdentityHelper.CoQuanID ?? 0) && IdentityHelper.RoleID == EnumChucVu.LanhDao.GetHashCode())
                {
                    laThanhTraTinh = true;
                }

                if (capID == (int)CapQuanLy.CapTrungUong)
                {
                    int phamviID = PhamViID;

                    //phaviID = 1: du lieu cap trung uong
                    //phamviID = 2: du lieu cap tinh
                    //phamviID khac: du lieu cap huyen, so
                    if (phamviID == 1)
                    {
                        //ThongKeCQChuyenDonCalc.TinhSoLieuTrungUong(ref resultList, startDate, endDate, ref total_den);
                        ThongKeCQChuyenDonCalc.GetCoQuanNhanDonTrungUong(ref resultList, start_Date, end_Date, coQuanID);
                    }
                    else if (phamviID == 2)
                    {
                        int tinhID = IdentityHelper.TinhID ?? 0;
                        ThongKeCQChuyenDonCalc.GetCoQuanNhanDonToanTinh(ref resultList, start_Date, end_Date, tinhID, coQuanID);
                    }
                }
                else if (capID == (int)CapQuanLy.CapUBNDTinh || laThanhTraTinh == true)
                {
                    int phamviID = PhamViID;

                    //phamviID = 2: du lieu cap tinh
                    //phamviID khac: du lieu cap huyen, so
                    if (phamviID == 2)
                    {
                        int tinhID = IdentityHelper.TinhID ?? 0;
                        ThongKeCQChuyenDonCalc.GetCoQuanNhanDonToanTinh(ref resultList, start_Date, end_Date, IdentityHelper.TinhID ?? 0, coQuanID);
                    }
                    else if (phamviID == 4)
                    {
                        int huyenID = CoQuanID;
                        ThongKeCQChuyenDonCalc.GetCoQuanNhanDonTheoHuyen(ref resultList, start_Date, end_Date, huyenID, coQuanID);
                    }
                    else
                    {
                        int cqID = CoQuanID;
                        if (cqID != 0)
                        {
                            ThongKeCQChuyenDonCalc.GetCoQuanNhanDonTheoCoQuan(ref resultList, start_Date, end_Date, cqID, 0);
                        }
                    }
                }
                else if (capID == (int)CapQuanLy.CapSoNganh)
                {
                    int cqID = IdentityHelper.CoQuanID ?? 0;
                    ThongKeCQChuyenDonCalc.GetCoQuanNhanDonTheoCoQuan(ref resultList, start_Date, end_Date, cqID, 0);
                }
                else if (capID == (int)CapQuanLy.CapUBNDHuyen)
                {
                    ThongKeCQChuyenDonCalc.GetCoQuanNhanDonTheoHuyen(ref resultList, start_Date, end_Date, IdentityHelper.HuyenID ?? 0, coQuanID);
                }
                else if (capID == (int)CapQuanLy.CapUBNDXa)
                {
                    int cqID = IdentityHelper.CoQuanID ?? 0;
                    ThongKeCQChuyenDonCalc.GetCoQuanNhanDonTheoCoQuan(ref resultList, start_Date, end_Date, cqID, coQuanID);
                }
                else if (capID == (int)CapQuanLy.CapPhong)
                {
                    int cqID = IdentityHelper.CoQuanID ?? 0;
                    ThongKeCQChuyenDonCalc.GetCoQuanNhanDonTheoCoQuan(ref resultList, start_Date, end_Date, cqID, 0);
                }
            }
            catch (Exception ex)
            {
            }

            return resultList;
        }

        public List<TKDonThuInfo> ThongKeTheoCOQuanChuyenDon_GetDSChiTietDonThu(DateTime startDate, DateTime endDate, int coquanID, int cap, int ddlCoQuan, int tinhLogin, int huyenLogin, int chiTietCoQuanID, int phamViID, int coQuanChuyenID, int start, int end, int xemTaiLieuMat, int canBoID, int laThanhTraTinh, int huyenID)
        {
            List<ThongKeInfo> resultList = new List<ThongKeInfo>();
            List<TKDonThuInfo> donThuList = new List<TKDonThuInfo>();

            //int tinhLogin = IdentityHelper.TinhID ?? 0;
            //int huyenLogin = IdentityHelper.HuyenID ?? 0;
            //int coquanID = IdentityHelper.CoQuanID ?? 0;
            //int ddlCoQuan = CoQuanID;
            //int cap = IdentityHelper.CapID ?? 0;
            //int chiTietCoQuanID = IdentityHelper.CoQuanID ?? 0;
            //int phamViID = PhamViID;
            //int coQuanChuyenID = CoQuanID;
            //int xemTaiLieuMat = 1;
            //int canBoID = IdentityHelper.CanBoID ?? 0;
            //int laThanhTraTinh = 0;
          

            if (ddlCoQuan > 0)
                coquanID = ddlCoQuan;

            if (cap == (int)CapQuanLy.CapTrungUong)
            {
                donThuList = new ThongKeDonThu().ThongKeDonThuChuyenCoQuan_GetDanhSachChiTiet_ByTinhID(startDate, endDate, tinhLogin, 0, coQuanChuyenID, start, end, xemTaiLieuMat, canBoID);
            }
            else if (cap == (int)CapQuanLy.CapUBNDTinh || laThanhTraTinh == 1)
            {
                if (phamViID == 2)
                    donThuList = new ThongKeDonThu().ThongKeDonThuChuyenCoQuan_GetDanhSachChiTiet_ByTinhID(startDate, endDate, tinhLogin, chiTietCoQuanID, coQuanChuyenID, start, end, xemTaiLieuMat, canBoID);
                else if (phamViID == 4)
                    donThuList = new ThongKeDonThu().ThongKeDonThuChuyenCoQuan_GetDanhSachChiTiet_ByHuyenID(startDate, endDate, huyenID, chiTietCoQuanID, coQuanChuyenID, start, end, xemTaiLieuMat, canBoID);
                else
                    donThuList = new ThongKeDonThu().ThongKeDonThuChuyenCoQuan_GetDanhSachChiTiet_ByCoQuan(startDate, endDate, coquanID, chiTietCoQuanID, coQuanChuyenID, start, end, xemTaiLieuMat, canBoID);
            }
            else if (cap == (int)CapQuanLy.CapSoNganh)
            {
                donThuList = new ThongKeDonThu().ThongKeDonThuChuyenCoQuan_GetDanhSachChiTiet_ByCoQuan(startDate, endDate, coquanID, chiTietCoQuanID, coQuanChuyenID, start, end, xemTaiLieuMat, canBoID);
            }
            else if (cap == (int)CapQuanLy.CapUBNDHuyen)
            {
                donThuList = new ThongKeDonThu().ThongKeDonThuChuyenCoQuan_GetDanhSachChiTiet_ByHuyenID(startDate, endDate, huyenLogin, chiTietCoQuanID, coQuanChuyenID, start, end, xemTaiLieuMat, canBoID);
            }
            else if (cap == (int)CapQuanLy.CapUBNDXa)
            {
                donThuList = new ThongKeDonThu().ThongKeDonThuChuyenCoQuan_GetDanhSachChiTiet_ByCoQuan(startDate, endDate, coquanID, chiTietCoQuanID, coQuanChuyenID, start, end, xemTaiLieuMat, canBoID);
            }
            else if (cap == (int)CapQuanLy.CapPhong)
            {
                donThuList = new ThongKeDonThu().ThongKeDonThuChuyenCoQuan_GetDanhSachChiTiet_ByCoQuan(startDate, endDate, coquanID, chiTietCoQuanID, coQuanChuyenID, start, end, xemTaiLieuMat, canBoID);
            }

            return donThuList;
        }

        public string ThongKeTheoCOQuanChuyenDon_Excel(string rootPath, string pathFile, List<TableData> data, DateTime tuNgay, DateTime denNgay)
        {
            //var camera = new Camera;
            // path to your excel file
            string path = rootPath + @"\Templates\BaoCao\ThongKeTheoCOQuanChuyenDon.xlsx";
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
                    worksheet.InsertRow(6, data.Count - 1, 5);
                    for (int i = 0; i < data.Count; i++)
                    {
                        if (data[i].DataArr != null && data[i].DataArr.Count > 0)
                        {
                            for (int j = 0; j < data[i].DataArr.Count; j++)
                            {
                                if (data[i].DataArr[j].Content != "0")
                                {
                                    worksheet.Cells[i + 5, j + 1].Value = data[i].DataArr[j].Content;
                                    //if (data[i].DataArr[j].Style.Contains("bold")) worksheet.Cells[i + 5, j + 1].Style.Font.Bold = true;
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

        public List<TableData> ThongKeTheoDiaChiChuDon(IdentityHelper IdentityHelper, int CoQuanID, int PhamViID, string ContentRootPath, DateTime startDate, DateTime endDate)
        {
            List<TableData> data = new List<TableData>();

            List<TKTheoNoiPhatSinhInfo> resultList = new List<TKTheoNoiPhatSinhInfo>();
            List<TKTheoNoiPhatSinhInfo> tongToanTinh = new List<TKTheoNoiPhatSinhInfo>();

            int capID = IdentityHelper.CapID ?? 0;
            Boolean laThanhTraTinh = false;
            var listThanhTraTinh = new SystemConfigDAL().GetByKey("Thanh_Tra_Tinh_ID").ConfigValue.Split(',').ToList().Select(x => Utils.ConvertToInt32(x.ToString(), 0)).ToList();
            if (listThanhTraTinh.Contains(IdentityHelper.CoQuanID ?? 0))
            {
                laThanhTraTinh = true;
            }

            if (capID == (int)CapQuanLy.CapUBNDTinh || laThanhTraTinh)
            {
                //if (IdentityHelper.RoleID == EnumChucVu.LanhDao.GetHashCode())
                //{
                    int phamviID = PhamViID;
                    //du lieu toan tinh
                    if (phamviID == 2)
                    {
                        ThongKeTheoNoiGuiCalc.TinhSoLieuToanTinh(startDate, endDate, IdentityHelper.TinhID ?? 0, 0, ref resultList, ref tongToanTinh);
                    }
                    //du lieu cap huyen
                    else if (phamviID == 4)
                    {
                        int huyenID = CoQuanID;
                        ThongKeTheoNoiGuiCalc.TinhSoLieuToanTinh(startDate, endDate, IdentityHelper.TinhID ?? 0, huyenID, ref resultList, ref tongToanTinh);
                    }
                    else if (phamviID == 5)
                    {
                        ThongKeTheoNoiGuiCalc.TinhSoLieuSo(startDate, endDate, IdentityHelper.CoQuanID ?? 0, ref resultList, IdentityHelper);
                    }
                //}
                //else
                //{
                //    ThongKeTheoNoiGuiCalc.TinhSoLieuSo(startDate, endDate, IdentityHelper.CoQuanID ?? 0, ref resultList, IdentityHelper);
                //}
            }
            else if (capID == (int)CapQuanLy.CapSoNganh)
            {
                int cqID = IdentityHelper.CoQuanID ?? 0;
                ThongKeTheoNoiGuiCalc.TinhSoLieuSo(startDate, endDate, cqID, ref resultList, IdentityHelper);
            }
            else if (capID == (int)CapQuanLy.CapUBNDHuyen)
            {
                int huyenID = CoQuanID;
                ThongKeTheoNoiGuiCalc.TinhSoLieuHuyen(startDate, endDate, huyenID, ref resultList);
            }
            else if (capID == (int)CapQuanLy.CapUBNDXa)
            {
                int cqID = IdentityHelper.CoQuanID ?? 0;
                int xaID = IdentityHelper.XaID ?? 0;
                //int tinhID = IdentityHelper.TinhID;
                ThongKeTheoNoiGuiCalc.TinhSoLieuXa(startDate, endDate, cqID, xaID, ref resultList);
            }
            else if (capID == (int)CapQuanLy.CapPhong)
            {
                int cqID = IdentityHelper.CoQuanID ?? 0;
                ThongKeTheoNoiGuiCalc.TinhSoLieuSo(startDate, endDate, cqID, ref resultList, IdentityHelper);
            }


            int stt = 1;
            foreach (TKTheoNoiPhatSinhInfo dro in resultList)
            {
                TableData tableData = new TableData();
                tableData.ID = stt++;
                var DataArr = new List<RowItem>();

                string Css = "";
                //if (dro.LoaiKhieuToID == 1 || dro.LoaiKhieuToID == 8 || dro.LoaiKhieuToID == 9)
                //{
                //    Css = "font-weight: bold;";
                //}

                RowItem RowItem1 = new RowItem(1, dro.STT, dro.DiaPhuongID.ToString(), dro.Level.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem2 = new RowItem(2, dro.Ten, dro.DiaPhuongID.ToString(), dro.Level.ToString(), null, "text-align: left;" + Css, ref DataArr);
                RowItem RowItem3 = new RowItem(3, dro.SLKhieuNai.ToString(), dro.DiaPhuongID.ToString(), dro.Level.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem4 = new RowItem(4, dro.SLToCao.ToString(), dro.DiaPhuongID.ToString(), dro.Level.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem5 = new RowItem(5, dro.SLKienNghi.ToString(), dro.DiaPhuongID.ToString(), dro.Level.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem6 = new RowItem(6, dro.SLPhanAnh.ToString(), dro.DiaPhuongID.ToString(), dro.Level.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem7 = new RowItem(7, (dro.SLKhieuNai + dro.SLToCao + dro.SLKienNghi + dro.SLPhanAnh).ToString(), dro.DiaPhuongID.ToString(), dro.Level.ToString(), null, "text-align: center;" + Css, ref DataArr);

                tableData.DataArr = DataArr;
                data.Add(tableData);
            }

            int SLKhieuNai = resultList.Sum(x => x.SLKhieuNai);
            int SLToCao = resultList.Sum(x => x.SLToCao);
            int SLKienNghi = resultList.Sum(x => x.SLKienNghi);
            int SLPhanAnh = resultList.Sum(x => x.SLPhanAnh);
            int TongSo = SLKhieuNai + SLToCao + SLKienNghi + SLPhanAnh;

            TableData rowTong = new TableData();
            rowTong.ID = stt++;
            var DataArrTong = new List<RowItem>();
            RowItem RowItemTong1 = new RowItem(1, "", null, null, null, "text-align: center;", ref DataArrTong);
            RowItem RowItemTong2 = new RowItem(2, "Tổng cộng", null, null, null, "text-align: left; font-weight:bold;", ref DataArrTong);
            RowItem RowItemTong3 = new RowItem(3, SLKhieuNai.ToString(), null, null, null, "text-align: center;font-weight:bold;", ref DataArrTong);
            RowItem RowItemTong4 = new RowItem(4, SLToCao.ToString(), null, null, null, "text-align: center;font-weight:bold;", ref DataArrTong);
            RowItem RowItemTong5 = new RowItem(5, SLKienNghi.ToString(), null, null, null, "text-align: center;font-weight:bold;", ref DataArrTong);
            RowItem RowItemTong6 = new RowItem(6, SLPhanAnh.ToString(), null, null, null, "text-align: center;font-weight:bold;", ref DataArrTong);
            RowItem RowItemTong7 = new RowItem(7, TongSo.ToString(), null, null, null, "text-align: center;font-weight:bold;", ref DataArrTong);
            rowTong.DataArr = DataArrTong;
            data.Add(rowTong);

            return data;
        }

        public List<TKDonThuInfo> ThongKeTheoDiaChiChuDon_GetDSChiTietDonThu(BaseReportParams p, IdentityHelper IdentityHelper, int PhamViID, int CoQuanID, int Type, int start, int end, string ContentRootPath, DateTime startDate, DateTime endDate)
        {
            List<TKDonThuInfo> resultList = new List<TKDonThuInfo>();
            int laThanhTraTinh = 0;
            var listThanhTraTinh = new SystemConfigDAL().GetByKey("Thanh_Tra_Tinh_ID").ConfigValue.Split(',').ToList().Select(x => Utils.ConvertToInt32(x.ToString(), 0)).ToList();
            if (listThanhTraTinh.Contains(IdentityHelper.CoQuanID ?? 0))
            {
                laThanhTraTinh = 1;
            }
            int lktID = 0;
            if (p.Index == 2)
            {
                lktID = 1;
            }
            else if (p.Index == 3)
            {
                lktID = 8;
            }
            else if(p.Index == 4)
            {
                lktID = 68;
            }
            else if(p.Index == 5)
            {
                lktID = 23;
            }
            else if (p.Index == 6)
            {
                lktID = 0;
            }

            if (Type == 1)
            {
                resultList = GetDSDonThuByHuyen(p.CoQuanID ?? 0, lktID, IdentityHelper.CoQuanID ?? 0, IdentityHelper.CapID ?? 0, PhamViID, startDate, endDate, start, end, p.CanBoID ?? 0, laThanhTraTinh);
            }
            else if (Type == 2)
            {
                resultList = GetDSDonThuByXa(p.CoQuanID ?? 0, lktID, IdentityHelper.CoQuanID ?? 0, IdentityHelper.CapID ?? 0, PhamViID, startDate, endDate, start, end, p.CanBoID ?? 0, laThanhTraTinh);
            }
            else
            {
                resultList = GetDSDonThuTong(IdentityHelper.TinhID ?? 0, IdentityHelper.HuyenID ?? 0, IdentityHelper.XaID ?? 0, p.CoQuanID ?? 0, lktID, IdentityHelper.CoQuanID ?? 0, IdentityHelper.CapID ?? 0, PhamViID, startDate, endDate, start, end, p.CanBoID ?? 0, laThanhTraTinh);
            }
            return resultList;
        }

        public string ThongKeTheoDiaChiChuDon_Excel(string rootPath, string pathFile, List<TableData> data, DateTime tuNgay, DateTime denNgay)
        {
            //var camera = new Camera;
            // path to your excel file
            string path = rootPath + @"\Templates\BaoCao\ThongKeTheoDiaChiChuDon.xlsx";
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
                    worksheet.InsertRow(6, data.Count - 1, 5);
                    //worksheet.DeleteRow(data.Count);
                    for (int i = 0; i < data.Count; i++)
                    {
                        if (data[i].DataArr != null && data[i].DataArr.Count > 0)
                        {
                            for (int j = 0; j < data[i].DataArr.Count; j++)
                            {
                                if (data[i].DataArr[j].Content != "0")
                                {
                                    worksheet.Cells[i + 5, j + 1].Value = data[i].DataArr[j].Content;
                                    if (data[i].DataArr[j].Style.Contains("bold")) worksheet.Cells[i + 5, j + 1].Style.Font.Bold = true;
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


        public List<TKDonThuInfo> GetDSDonThuByXa(int xaID, int lktID, int coquanID, int cap, int ddlPhamViDuLieu, DateTime startDate, DateTime endDate, int start, int end, int canBoID, int laThanhTraTinh)
        {          
            //int lktID = Utils.ConvertToInt32(context.Request["lktID"], 0);
            //int coquanID = Utils.ConvertToInt32(context.Request["coquanID"], 0);
            //int cap = Utils.ConvertToInt32(context.Request["cap"], 0);
            //int ddlPhamViDuLieu = Utils.ConvertToInt32(context.Request["ddlPhamViDuLieu"], 0);
            //DateTime startDate = Utils.ConvertToDateTime(context.Request["startDate"], DateTime.MinValue);
            //DateTime endDate = Utils.ConvertToDateTime(context.Request["endDate"], DateTime.MinValue);
            //int start = Utils.ConvertToInt32(context.Request["start"], 0);
            //int end = Utils.ConvertToInt32(context.Request["end"], 0);
            int xemTaiLieuMat = 1;
            //int canBoID = Utils.ConvertToInt32(context.Request["canBoID"], 0);
            //int laThanhTraTinh = Utils.ConvertToInt32(context.Request["laThanhTraTinh"], 0);

            var xaInfo = new Xa().GetByID(xaID);

            List<TKDonThuInfo> donThuList = new List<TKDonThuInfo>();
            if (cap == (int)CapQuanLy.CapUBNDTinh || laThanhTraTinh == 1)
            {
                //donThuList = new ThongKeDonThu().GetDonThuByTinh(startDate, endDate, xaInfo.TinhID).ToList();
                List<HuyenInfo> huyenList = new Huyen().GetByTinh(xaInfo.TinhID).ToList();
                List<CoQuanInfo> listCoQuan = new List<CoQuanInfo>();
                foreach (HuyenInfo huyenInfo in huyenList)
                {
                    List<CoQuanInfo> cqHuyenList = new CoQuan().GetByHuyen(huyenInfo.HuyenID).ToList();
                    listCoQuan.AddRange(cqHuyenList);
                }
                if (ddlPhamViDuLieu == 0 || ddlPhamViDuLieu == 5)
                {
                    List<CoQuanInfo> coQuans = new List<CoQuanInfo>();
                    CoQuanInfo cq = new CoQuanInfo();
                    cq.CoQuanID = coquanID;
                    coQuans.Add(cq);
                    listCoQuan = coQuans;
                }
                donThuList = new ThongKeDonThu().GetDonThu_BCTheoDCChuDon(startDate, endDate, xaID, 3, lktID, start, end, listCoQuan, xemTaiLieuMat, canBoID).ToList();
            }
            else if (cap == (int)CapQuanLy.CapUBNDHuyen)
            {
                List<CoQuanInfo> listCoQuan = new CoQuan().GetByHuyen(xaInfo.HuyenID).ToList();
                donThuList = new ThongKeDonThu().GetDonThu_BCTheoDCChuDon(startDate, endDate, xaID, 3, lktID, start, end, listCoQuan, xemTaiLieuMat, canBoID).ToList();
            }
            else if (cap == (int)CapQuanLy.CapUBNDXa)
            {
                List<CoQuanInfo> coQuans = new List<CoQuanInfo>();
                CoQuanInfo cq = new CoQuanInfo();
                cq.CoQuanID = coquanID;
                coQuans.Add(cq);

                //List<CoQuanInfo> listCoQuan = new CoQuan().GetByHuyen(xaInfo.HuyenID).ToList();
                donThuList = new ThongKeDonThu().GetDonThu_BCTheoDCChuDon(startDate, endDate, xaID, 3, lktID, start, end, coQuans, xemTaiLieuMat, canBoID).ToList();
            }
            else
            {
                List<CoQuanInfo> listCoQuan = new List<CoQuanInfo>();
                CoQuanInfo cq = new CoQuanInfo();
                cq.CoQuanID = coquanID;
                listCoQuan.Add(cq);
                donThuList = new ThongKeDonThu().GetDonThu_BCTheoDCChuDon(startDate, endDate, xaID, 3, lktID, start, end, listCoQuan, xemTaiLieuMat, canBoID).ToList();

            }

            foreach (var donThuInfo in donThuList)
            {
                donThuInfo.NgayNhapDonStr = Ultilities.Format.FormatDate(donThuInfo.NgayNhapDon);
            }



            return donThuList;
        }

        public List<TKDonThuInfo> GetDSDonThuByHuyen(int huyenID, int lktID, int coquanID, int cap, int ddlPhamViDuLieu, DateTime startDate, DateTime endDate, int start, int end, int canBoID, int laThanhTraTinh)
        {
            //int huyenID = Utils.ConvertToInt32(context.Request["huyenID"], 0);
            //int lktID = Utils.ConvertToInt32(context.Request["lktID"], 0);
            //int coquanID = Utils.ConvertToInt32(context.Request["coquanID"], 0);
            //int cap = Utils.ConvertToInt32(context.Request["cap"], 0);
            //int ddlPhamViDuLieu = Utils.ConvertToInt32(context.Request["ddlPhamViDuLieu"], 0);
            //DateTime startDate = Utils.ConvertToDateTime(context.Request["startDate"], DateTime.MinValue);
            //DateTime endDate = Utils.ConvertToDateTime(context.Request["endDate"], DateTime.MinValue);
            //int start = Utils.ConvertToInt32(context.Request["start"], 0);
            //int end = Utils.ConvertToInt32(context.Request["end"], 0);
            int xemTaiLieuMat = 1;
            //int canBoID = Utils.ConvertToInt32(context.Request["canBoID"], 0);
            //int laThanhTraTinh = Utils.ConvertToInt32(context.Request["laThanhTraTinh"], 0);

            var huyenInfo = new Huyen().GetByID(huyenID);

            List<TKDonThuInfo> donThuList = new List<TKDonThuInfo>();
            if (cap == (int)CapQuanLy.CapUBNDTinh || laThanhTraTinh == 1)
            {
                //donThuList = new ThongKeDonThu().GetDonThuByTinh(startDate, endDate, huyenInfo.TinhID).ToList();
                List<HuyenInfo> huyenList = new Huyen().GetByTinh(huyenInfo.TinhID).ToList();
                List<CoQuanInfo> listCoQuan = new List<CoQuanInfo>();
                foreach (HuyenInfo info in huyenList)
                {
                    List<CoQuanInfo> cqHuyenList = new CoQuan().GetByHuyen(info.HuyenID).ToList();
                    listCoQuan.AddRange(cqHuyenList);
                }
                if (ddlPhamViDuLieu == 0 || ddlPhamViDuLieu == 5)
                {
                    List<CoQuanInfo> coQuans = new List<CoQuanInfo>();
                    CoQuanInfo cq = new CoQuanInfo();
                    cq.CoQuanID = coquanID;
                    coQuans.Add(cq);
                    listCoQuan = coQuans;
                }
                donThuList = new ThongKeDonThu().GetDonThu_BCTheoDCChuDon(startDate, endDate, huyenID, 2, lktID, start, end, listCoQuan, xemTaiLieuMat, canBoID).ToList();
            }
            else if (cap == (int)CapQuanLy.CapUBNDHuyen)
            {
                List<CoQuanInfo> listCoQuan = new CoQuan().GetByHuyen(huyenInfo.HuyenID).ToList();
                donThuList = new ThongKeDonThu().GetDonThu_BCTheoDCChuDon(startDate, endDate, huyenID, 2, lktID, start, end, listCoQuan, xemTaiLieuMat, canBoID).ToList();

                //List<CoQuanInfo> cqHuyenList = new CoQuan().GetByHuyen(huyenInfo.HuyenID).ToList();

                //foreach (CoQuanInfo cqInfo in cqHuyenList)
                //{
                //    List<TKDonThuInfo> dtList = new ThongKeDonThu().GetDonThuByCoQuan(startDate, endDate, cqInfo.CoQuanID).ToList();
                //    donThuList.AddRange(dtList);
                //}
            }
            else
            {
                List<CoQuanInfo> listCoQuan = new List<CoQuanInfo>();
                CoQuanInfo cq = new CoQuanInfo();
                cq.CoQuanID = coquanID;
                listCoQuan.Add(cq);
                donThuList = new ThongKeDonThu().GetDonThu_BCTheoDCChuDon(startDate, endDate, huyenID, 2, lktID, start, end, listCoQuan, xemTaiLieuMat, canBoID).ToList();

                //donThuList = new ThongKeDonThu().GetDonThuByCoQuan(startDate, endDate, coquanID).ToList();
            }

            //donThuList = donThuList.Where(x => x.HuyenID == huyenID).ToList();
            //if (lktID != 0)
            //{
            //    donThuList = donThuList.Where(x => x.LoaiKhieuTo1ID == lktID || x.LoaiKhieuTo2ID == lktID).ToList();
            //}

            foreach (var donThuInfo in donThuList)
            {
                donThuInfo.NgayNhapDonStr = Ultilities.Format.FormatDate(donThuInfo.NgayNhapDon);
            }

            return donThuList;
        }
        
        public List<TKDonThuInfo> GetDSDonThuTong(int tinhLogin, int huyenLogin, int xaLogin,int huyenID, int lktID, int coquanID, int cap, int ddlPhamViDuLieu, DateTime startDate, DateTime endDate, int start, int end, int canBoID, int laThanhTraTinh)
        {
            //int tinhLogin = Utils.ConvertToInt32(context.Request["tinhLogin"], 0);
            //int huyenLogin = Utils.ConvertToInt32(context.Request["huyenLogin"], 0);
            //int xaLogin = Utils.ConvertToInt32(context.Request["xaLogin"], 0);
            //int huyenID = Utils.ConvertToInt32(context.Request["huyenID"], 0);
            //int lktID = Utils.ConvertToInt32(context.Request["lktID"], 0);
            //int coquanID = Utils.ConvertToInt32(context.Request["coquanID"], 0);
            //int cap = Utils.ConvertToInt32(context.Request["cap"], 0);
            //int ddlPhamViDuLieu = Utils.ConvertToInt32(context.Request["ddlPhamViDuLieu"], 0);
            //DateTime startDate = Utils.ConvertToDateTime(context.Request["startDate"], DateTime.MinValue);
            //DateTime endDate = Utils.ConvertToDateTime(context.Request["endDate"], DateTime.MinValue);
            //int start = Utils.ConvertToInt32(context.Request["start"], 0);
            //int end = Utils.ConvertToInt32(context.Request["end"], 0);
            int xemTaiLieuMat = 1;
            //int canBoID = Utils.ConvertToInt32(context.Request["canBoID"], 0);
            //int laThanhTraTinh = Utils.ConvertToInt32(context.Request["laThanhTraTinh"], 0);
            //var huyenInfo = new HuyenInfo();

            List<TKDonThuInfo> donThuList = new List<TKDonThuInfo>();

            if (cap == (int)CapQuanLy.CapUBNDTinh || laThanhTraTinh == 1)
            {
                #region cap UBND tinh
                //2 du lieu toan tinh
                //4 du lieu cap huyen
                if (ddlPhamViDuLieu == 2)
                {
                    List<HuyenInfo> huyenList = new Huyen().GetByTinh(tinhLogin).ToList();
                    if (huyenID != 0)
                    {
                        huyenList = huyenList.Where(x => x.HuyenID == huyenID).ToList();
                    }
                    List<CoQuanInfo> listCoQuan = new List<CoQuanInfo>();
                    foreach (HuyenInfo huyenInfo in huyenList)
                    {
                        List<CoQuanInfo> cqHuyenList = new CoQuan().GetByHuyen(huyenInfo.HuyenID).ToList();
                        listCoQuan.AddRange(cqHuyenList);
                    }
                    donThuList = new ThongKeDonThu().GetDonThu_BCTheoDCChuDon_Tong(startDate, endDate, tinhLogin, 2, lktID, start, end, listCoQuan, huyenList, null, xemTaiLieuMat, canBoID).ToList();
                    //donThuList = new ThongKeDonThu().GetDonThu_BCTheoDCChuDon(startDate, endDate, tinhLogin, 1, lktID, start, end, listCoQuan, xemTaiLieuMat, canBoID).ToList();

                    //List<TKDonThuInfo> donThuTinhList = new ThongKeDonThu().GetDonThuByTinh(startDate, endDate, tinhLogin).ToList();

                    //List<HuyenInfo> huyenList = new Huyen().GetByTinh(tinhLogin).ToList();
                    //if (huyenID != 0)
                    //{
                    //    huyenList = huyenList.Where(x => x.HuyenID == huyenID).ToList();
                    //}

                    //foreach (HuyenInfo huyenInfo in huyenList)
                    //{
                    //    List<TKDonThuInfo> dtList = donThuTinhList.Where(x => x.HuyenID == huyenInfo.HuyenID).ToList();

                    //    donThuList.AddRange(dtList);
                    //}
                }
                if (ddlPhamViDuLieu == 4)
                {
                    List<HuyenInfo> huyenList = new Huyen().GetByTinh(tinhLogin).ToList();
                    if (huyenID != 0)
                    {
                        huyenList = huyenList.Where(x => x.HuyenID == huyenID).ToList();
                    }
                    List<CoQuanInfo> listCoQuan = new List<CoQuanInfo>();
                    foreach (HuyenInfo huyenInfo in huyenList)
                    {
                        List<CoQuanInfo> cqHuyenList = new CoQuan().GetByHuyen(huyenInfo.HuyenID).ToList();
                        listCoQuan.AddRange(cqHuyenList);
                    }
                    donThuList = new ThongKeDonThu().GetDonThu_BCTheoDCChuDon(startDate, endDate, huyenID, 2, lktID, start, end, listCoQuan, xemTaiLieuMat, canBoID).ToList();
                }
                // du lieu trong don vi
                else if (ddlPhamViDuLieu == 0 || ddlPhamViDuLieu == 5)
                {
                    List<HuyenInfo> huyenList = new Huyen().GetByTinh(tinhLogin).ToList();
                    List<CoQuanInfo> listCoQuan = new List<CoQuanInfo>();
                    CoQuanInfo cq = new CoQuanInfo();
                    cq.CoQuanID = coquanID;
                    listCoQuan.Add(cq);
                    donThuList = new ThongKeDonThu().GetDonThu_BCTheoDCChuDon_Tong(startDate, endDate, tinhLogin, 2, lktID, start, end, listCoQuan, huyenList, null, xemTaiLieuMat, canBoID).ToList();

                  
                }
                #endregion
            }
            else if (cap == (int)CapQuanLy.CapUBNDHuyen)
            {
                
                List<XaInfo> xaList = new Xa().GetByHuyen(huyenLogin).ToList();
                List<CoQuanInfo> cqHuyenList = new CoQuan().GetByHuyen(huyenLogin).ToList();
                donThuList = new ThongKeDonThu().GetDonThu_BCTheoDCChuDon_Tong(startDate, endDate, huyenLogin, 3, lktID, start, end, cqHuyenList, null, xaList, xemTaiLieuMat, canBoID).ToList();
                #region cap UBND huyen
                
                #endregion
            }
            else if (cap == (int)CapQuanLy.CapSoNganh)
            {
                List<HuyenInfo> huyenList = new Huyen().GetByTinh(tinhLogin).ToList();
                List<CoQuanInfo> listCoQuan = new List<CoQuanInfo>();
                CoQuanInfo cq = new CoQuanInfo();
                cq.CoQuanID = coquanID;
                listCoQuan.Add(cq);
                donThuList = new ThongKeDonThu().GetDonThu_BCTheoDCChuDon_Tong(startDate, endDate, tinhLogin, 2, lktID, start, end, listCoQuan, huyenList, null, xemTaiLieuMat, canBoID).ToList();

                #region cap so nganh
               
                #endregion
            }
            else if (cap == (int)CapQuanLy.CapUBNDXa)
            {
                
                List<CoQuanInfo> listCoQuan = new List<CoQuanInfo>();
                CoQuanInfo cq = new CoQuanInfo();
                cq.CoQuanID = coquanID;
                listCoQuan.Add(cq);
                donThuList = new ThongKeDonThu().GetDonThu_BCTheoDCChuDon(startDate, endDate, xaLogin, 3, lktID, start, end, listCoQuan, xemTaiLieuMat, canBoID).ToList();
            }
            else if (cap == (int)CapQuanLy.CapPhong)
            {
                List<CoQuanInfo> listCoQuan = new List<CoQuanInfo>();
                CoQuanInfo cq = new CoQuanInfo();
                cq.CoQuanID = coquanID;
                listCoQuan.Add(cq);
                List<HuyenInfo> huyenList = new Huyen().GetByTinh(tinhLogin).ToList();
              
                donThuList = new ThongKeDonThu().GetDonThu_BCTheoDCChuDon_Tong(startDate, endDate, tinhLogin, 2, lktID, start, end, listCoQuan, huyenList, null, xemTaiLieuMat, canBoID).ToList();
            }
           
            foreach (var donThuInfo in donThuList)
            {
                donThuInfo.NgayNhapDonStr = Ultilities.Format.FormatDate(donThuInfo.NgayNhapDon);
            }

            return donThuList;
        }

        public List<TableData> ThongKeTheoNoiPhatSinh(IdentityHelper IdentityHelper, int CoQuanID, int PhamViID, string ContentRootPath, DateTime startDate, DateTime endDate)
        {
            List<TableData> data = new List<TableData>();

            List<TKTheoNoiPhatSinhInfo> resultList = new List<TKTheoNoiPhatSinhInfo>();
            List<TKTheoNoiPhatSinhInfo> tongToanTinh = new List<TKTheoNoiPhatSinhInfo>();

            int capID = IdentityHelper.CapID ?? 0;
            Boolean laThanhTraTinh = false;
            var listThanhTraTinh = new SystemConfigDAL().GetByKey("Thanh_Tra_Tinh_ID").ConfigValue.Split(',').ToList().Select(x => Utils.ConvertToInt32(x.ToString(), 0)).ToList();
            if (listThanhTraTinh.Contains(IdentityHelper.CoQuanID ?? 0) && IdentityHelper.RoleID == EnumChucVu.LanhDao.GetHashCode())
            {
                laThanhTraTinh = true;
            }

            if (capID == (int)CapQuanLy.CapUBNDTinh || laThanhTraTinh)
            {
                //if (IdentityHelper.RoleID == EnumChucVu.LanhDao.GetHashCode())
                //{
                    int phamviID = PhamViID;
                    //du lieu toan tinh
                    if (phamviID == 2)
                    {
                        ThongKeTheoNoiGuiCalc.TinhSoLieuToanTinh(startDate, endDate, IdentityHelper.TinhID ?? 0, 0, ref resultList, ref tongToanTinh);
                    }
                    //du lieu cap huyen
                    else if (phamviID == 4)
                    {
                        int huyenID = CoQuanID;
                        ThongKeTheoNoiGuiCalc.TinhSoLieuToanTinh(startDate, endDate, IdentityHelper.TinhID ?? 0, huyenID, ref resultList, ref tongToanTinh);
                    }
                    else if (phamviID == 5)
                    {
                        ThongKeTheoNoiGuiCalc.TinhSoLieuSo(startDate, endDate, IdentityHelper.CoQuanID ?? 0, ref resultList, IdentityHelper);
                    }
                //}
                //else
                //{
                //    ThongKeTheoNoiGuiCalc.TinhSoLieuSo(startDate, endDate, IdentityHelper.CoQuanID ?? 0, ref resultList, IdentityHelper);
                //}
            }
            else if (capID == (int)CapQuanLy.CapSoNganh)
            {
                int cqID = IdentityHelper.CoQuanID ?? 0;
                ThongKeTheoNoiGuiCalc.TinhSoLieuSo(startDate, endDate, cqID, ref resultList, IdentityHelper);
            }
            else if (capID == (int)CapQuanLy.CapUBNDHuyen)
            {
                int huyenID = CoQuanID;
                ThongKeTheoNoiGuiCalc.TinhSoLieuHuyen(startDate, endDate, huyenID, ref resultList);
            }
            else if (capID == (int)CapQuanLy.CapUBNDXa)
            {
                int cqID = IdentityHelper.CoQuanID ?? 0;
                int xaID = IdentityHelper.XaID ?? 0;
                //int tinhID = IdentityHelper.TinhID;
                ThongKeTheoNoiGuiCalc.TinhSoLieuXa(startDate, endDate, cqID, xaID, ref resultList);
            }
            else if (capID == (int)CapQuanLy.CapPhong)
            {
                int cqID = IdentityHelper.CoQuanID ?? 0;
                ThongKeTheoNoiGuiCalc.TinhSoLieuSo(startDate, endDate, cqID, ref resultList, IdentityHelper);
            }


            int stt = 1;
            foreach (TKTheoNoiPhatSinhInfo dro in resultList)
            {
                TableData tableData = new TableData();
                tableData.ID = stt++;
                var DataArr = new List<RowItem>();

                string Css = "";
                //if (dro.LoaiKhieuToID == 1 || dro.LoaiKhieuToID == 8 || dro.LoaiKhieuToID == 9)
                //{
                //    Css = "font-weight: bold;";
                //}

                RowItem RowItem1 = new RowItem(1, dro.STT, dro.DiaPhuongID.ToString(), dro.Level.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem2 = new RowItem(2, dro.Ten, dro.DiaPhuongID.ToString(), dro.Level.ToString(), null, "text-align: left;" + Css, ref DataArr);
                RowItem RowItem3 = new RowItem(3, dro.SLKhieuNai.ToString(), dro.DiaPhuongID.ToString(), dro.Level.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem4 = new RowItem(4, dro.SLToCao.ToString(), dro.DiaPhuongID.ToString(), dro.Level.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem5 = new RowItem(5, dro.SLKienNghi.ToString(), dro.DiaPhuongID.ToString(), dro.Level.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem6 = new RowItem(6, dro.SLPhanAnh.ToString(), dro.DiaPhuongID.ToString(), dro.Level.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem7 = new RowItem(7, (dro.SLKhieuNai + dro.SLToCao + dro.SLKienNghi + dro.SLPhanAnh).ToString(), dro.DiaPhuongID.ToString(), dro.Level.ToString(), null, "text-align: center;" + Css, ref DataArr);

                tableData.DataArr = DataArr;
                data.Add(tableData);
            }

            int SLKhieuNai = resultList.Sum(x => x.SLKhieuNai);
            int SLToCao = resultList.Sum(x => x.SLToCao);
            int SLKienNghi = resultList.Sum(x => x.SLKienNghi);
            int SLPhanAnh = resultList.Sum(x => x.SLPhanAnh);
            int TongSo = SLKhieuNai + SLToCao + SLKienNghi + SLPhanAnh;

            TableData rowTong = new TableData();
            rowTong.ID = stt++;
            var DataArrTong = new List<RowItem>();
            RowItem RowItemTong1 = new RowItem(1, "", null, null, null, "text-align: center;", ref DataArrTong);
            RowItem RowItemTong2 = new RowItem(2, "Tổng cộng", null, null, null, "text-align: left; font-weight:bold;", ref DataArrTong);
            RowItem RowItemTong3 = new RowItem(3, SLKhieuNai.ToString(), null, null, null, "text-align: center;font-weight:bold;", ref DataArrTong);
            RowItem RowItemTong4 = new RowItem(4, SLToCao.ToString(), null, null, null, "text-align: center;font-weight:bold;", ref DataArrTong);
            RowItem RowItemTong5 = new RowItem(5, SLKienNghi.ToString(), null, null, null, "text-align: center;font-weight:bold;", ref DataArrTong);
            RowItem RowItemTong6 = new RowItem(6, SLPhanAnh.ToString(), null, null, null, "text-align: center;font-weight:bold;", ref DataArrTong);
            RowItem RowItemTong7 = new RowItem(7, TongSo.ToString(), null, null, null, "text-align: center;font-weight:bold;", ref DataArrTong);
            rowTong.DataArr = DataArrTong;
            data.Add(rowTong);

            return data;
        }

        public List<TKDonThuInfo> ThongKeTheoNoiPhatSinh_GetDSChiTietDonThu(BaseReportParams p, IdentityHelper IdentityHelper, int PhamViID, int CoQuanID, int Type, int start, int end, string ContentRootPath, DateTime startDate, DateTime endDate)
        {
            List<TKDonThuInfo> resultList = new List<TKDonThuInfo>();
            int laThanhTraTinh = 0;
            var listThanhTraTinh = new SystemConfigDAL().GetByKey("Thanh_Tra_Tinh_ID").ConfigValue.Split(',').ToList().Select(x => Utils.ConvertToInt32(x.ToString(), 0)).ToList();
            if (listThanhTraTinh.Contains(IdentityHelper.CoQuanID ?? 0) && IdentityHelper.RoleID == EnumChucVu.LanhDao.GetHashCode())
            {
                laThanhTraTinh = 1;
            }
            int lktID = 0;
            if(p.Index == 2)
            {
                lktID = 1;//khieu nai
            }
            else if (p.Index == 3)
            {
                lktID = 8;//to cao
            }
            else if (p.Index == 4)
            {
                lktID = 68;//kien nghi
            }
            else if (p.Index == 5)
            {
                lktID = 23;//phan anh
            }

            if (Type == 1)
            {
                resultList = GetDSDonThuByHuyen(p.CoQuanID ?? 0, lktID, IdentityHelper.CoQuanID ?? 0, p.CapID ?? 0, PhamViID, startDate, endDate, start, end, p.CanBoID ?? 0, laThanhTraTinh);
            }
            else if (Type == 2)
            {
                resultList = GetDSDonThuByXa(p.CoQuanID ?? 0, lktID, IdentityHelper.CoQuanID ?? 0, p.CapID ?? 0, PhamViID, startDate, endDate, start, end, p.CanBoID ?? 0, laThanhTraTinh);
            }
            else
            {
                resultList = GetDSDonThuTong(IdentityHelper.TinhID ?? 0, IdentityHelper.HuyenID ?? 0, IdentityHelper.XaID ?? 0, p.HuyenID ?? 0, lktID, IdentityHelper.CoQuanID ?? 0, p.CapID ?? 0, PhamViID, startDate, endDate, start, end, p.CanBoID ?? 0, laThanhTraTinh);
            }
            return resultList;
        }

        public string ThongKeTheoNoiPhatSinh_Excel(string rootPath, string pathFile, List<TableData> data, DateTime tuNgay, DateTime denNgay)
        {
            //var camera = new Camera;
            // path to your excel file
            string path = rootPath + @"\Templates\BaoCao\ThongKeTheoDiaChiChuDon.xlsx";
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
                    worksheet.InsertRow(6, data.Count - 1, 5);
                    //worksheet.DeleteRow(data.Count);
                    for (int i = 0; i < data.Count; i++)
                    {
                        if (data[i].DataArr != null && data[i].DataArr.Count > 0)
                        {
                            for (int j = 0; j < data[i].DataArr.Count; j++)
                            {
                                if (data[i].DataArr[j].Content != "0")
                                {
                                    worksheet.Cells[i + 5, j + 1].Value = data[i].DataArr[j].Content;
                                    if (data[i].DataArr[j].Style.Contains("bold")) worksheet.Cells[i + 5, j + 1].Style.Font.Bold = true;
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

        public List<TableData> ThongKeTheoVuViecDongNguoi(List<int> listCapChonBaoCao, IdentityHelper IdentityHelper, string ContentRootPath, DateTime startDate, DateTime endDate)
        {
            List<TableData> data = new List<TableData>();

            List<ThongKeInfo> bcList = new List<ThongKeInfo>();
            List<BaoCaoVuViecDongNguoiInfo> resultList = new List<BaoCaoVuViecDongNguoiInfo>();

            int canboID = IdentityHelper.CanBoID ?? 0;
            int capID = IdentityHelper.CapID ?? 0;
            int cqID = IdentityHelper.CoQuanID ?? 0;
            int tinhID = IdentityHelper.TinhID ?? 0;
            var listThanhTraTinh = new SystemConfigDAL().GetByKey("Thanh_Tra_Tinh_ID").ConfigValue.Split(',').ToList().Select(x => Utils.ConvertToInt32(x.ToString(), 0)).ToList();
            if (capID == (int)CapQuanLy.CapUBNDTinh ||
                (IdentityHelper.CapID == (int)CapQuanLy.CapSoNganh && listThanhTraTinh.Contains(IdentityHelper.CoQuanID ?? 0) && IdentityHelper.RoleID == (int)EnumChucVu.LanhDao)
               )
            {
                List<CapInfo> capList = new List<CapInfo>();
                foreach (int item in listCapChonBaoCao)
                {
                    CapInfo capInfo = new CapInfo();
                    capInfo.CapID = item;
                    capInfo.TenCap = GetTenCap(item);
                    capList.Add(capInfo);
                }
                //phucth
                BaoCaoVuViecDongNguoi.CalculateBaoCaoCapTinh(ref resultList, startDate, endDate, capList, tinhID, IdentityHelper, ContentRootPath);
            }
            else if (capID == (int)CapQuanLy.CapTrungUong)
            {

                BaoCaoVuViecDongNguoi.CalculateBaoCaoCapTrungUong(ref resultList, startDate, endDate, tinhID);
            }
            else if (capID == (int)CapQuanLy.CapUBNDHuyen)
            {
                BaoCaoVuViecDongNguoi.CalculateBaoCaoCapHuyen(ref resultList, startDate, endDate, tinhID, IdentityHelper, ContentRootPath);
            }
            else if (capID == (int)CapQuanLy.CapSoNganh)
            {
                //phuocqh
                BaoCaoVuViecDongNguoi.CalculateBaoCaoCapSo(ref resultList, startDate, endDate, IdentityHelper, ContentRootPath);
            }
            else if (capID == (int)CapQuanLy.CapPhong)
            {
                BaoCaoVuViecDongNguoi.CalculateBaoCaoPhong(ref resultList, startDate, endDate, IdentityHelper);
            }
            else if (capID == (int)CapQuanLy.CapUBNDXa)
            {

                BaoCaoVuViecDongNguoi.CalculateBaoCaoCapXa(ref resultList, startDate, endDate, IdentityHelper, ContentRootPath);
            }

            int stt = 1;
            foreach (BaoCaoVuViecDongNguoiInfo dro in resultList)
            {
                TableData tableData = new TableData();
                tableData.ID = stt++;
                var DataArr = new List<RowItem>();

                string Css = "";
                //if (dro.LoaiKhieuToID == 1 || dro.LoaiKhieuToID == 8 || dro.LoaiKhieuToID == 9)
                //{
                //    Css = "font-weight: bold;";
                //}

                RowItem RowItem1 = new RowItem(1, stt.ToString(), null, null, null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem2 = new RowItem(2, dro.DonVi, null, null, null, "text-align: left;" + Css, ref DataArr);
                RowItem RowItem3 = new RowItem(3, dro.XLDKhieuKienDN.ToString(), null, null, null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem4 = new RowItem(4, dro.DonNhieuNguoiDungTen.ToString(), null, null, null, "text-align: center;" + Css, ref DataArr);
              
                tableData.DataArr = DataArr;
                data.Add(tableData);
            }

            return data;
        }

        public List<TableData> ThongKeTheoRutDon(List<int> listCapChonBaoCao, IdentityHelper IdentityHelper, string ContentRootPath, int CoQuanID, DateTime startDate, DateTime endDate)
        {
            //List<int> ListCapID = new List<int>();
            //capList.ForEach(x => ListCapID.Add(x.CapID));
            //int CoQuanChaID = 0;



            List<TableData> data = new List<TableData>();

            int coquanID = IdentityHelper.CoQuanID ?? 0;
            List<int> ListCoQuanID = new List<int>();
            var coquanidselect = CoQuanID;
            var listThanhTraTinh = new SystemConfigDAL().GetByKey("Thanh_Tra_Tinh_ID").ConfigValue.Split(',').ToList().Select(x => Utils.ConvertToInt32(x.ToString(), 0)).ToList();

            if (IdentityHelper.CapID == (int)CapQuanLy.CapUBNDTinh || listThanhTraTinh.Contains(IdentityHelper.CoQuanID ?? 0) || listCapChonBaoCao.Contains(CapQuanLy.CapUBNDTinh.GetHashCode()))
            {
                if (coquanidselect == 0)
                {
                    ListCoQuanID = new CoQuan().GetAllCapCon(0).ToList().Select(x => x.CoQuanID).ToList();
                }
                else
                {
                    ListCoQuanID = new List<int> { coquanidselect };
                }

            }
            else if (IdentityHelper.CapID == (int)CapQuanLy.CapUBNDHuyen || listCapChonBaoCao.Contains(CapQuanLy.CapUBNDHuyen.GetHashCode()))
            {
                if (coquanidselect == 0)
                {
                    //ListCoQuanID = new CoQuan().GetAllCapCon(0).Where(x => x.HuyenID == IdentityHelper.GetHuyenID()).ToList().Select(x => x.CoQuanID).ToList();
                    ListCoQuanID = new CoQuan().GetCoQuanByHuyenID(IdentityHelper.HuyenID ?? 0).ToList().Select(x => x.CoQuanID).ToList();
                }
                else
                {
                    ListCoQuanID = new List<int> { coquanidselect };
                }
            }
            //}
            else
            {
                ListCoQuanID = new List<int> { coquanID };
            }

            List<RutDonInfo> rdList = new RutDon().GetByCoQuan_New(ListCoQuanID, startDate, endDate).ToList();
           
            int stt = 0;
            foreach (RutDonInfo dro in rdList)
            {
                TableData tableData = new TableData();
                tableData.ID = stt++;
                var DataArr = new List<RowItem>();

                string Css = "";
                //if (dro.LoaiKhieuToID == 1 || dro.LoaiKhieuToID == 8 || dro.LoaiKhieuToID == 9)
                //{
                //    Css = "font-weight: bold;";
                //}
                List<DoiTuongKNInfo> dtknList = new DoiTuongKN().GetByNhomKNID(dro.NhomKNID).ToList();
                DoiTuongKNInfo lastInfo = dtknList.Last();
                string TenChuDon = "";
                string DiaChiChuDon = "";
                foreach (DoiTuongKNInfo dtknInfo in dtknList)
                {
                    if (dtknInfo != lastInfo)
                    {
                        TenChuDon += dtknInfo.HoTen + ", ";
                        DiaChiChuDon += dtknInfo.DiaChiCT + "; ";
                    }
                    else
                    {
                        TenChuDon += dtknInfo.HoTen;
                        DiaChiChuDon += dtknInfo.DiaChiCT;
                    }
                }
                string NgayRut = Ultilities.Format.FormatDate(dro.NgayRutDon ?? DateTime.Now);
                if (dro.NgayRutDon == null) NgayRut = "";

                RowItem RowItem1 = new RowItem(1, stt.ToString(), null, null, null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem2 = new RowItem(2, dro.SoDon, null, null, null, "text-align: left;" + Css, ref DataArr);
                RowItem RowItem3 = new RowItem(3, TenChuDon, null, null, null, "text-align: left;" + Css, ref DataArr);
                RowItem RowItem4 = new RowItem(4, DiaChiChuDon, null, null, null, "text-align: left;" + Css, ref DataArr);
                RowItem RowItem5 = new RowItem(5, dro.NoiDungDon, null, null, null, "text-align: left;" + Css, ref DataArr);
                RowItem RowItem6 = new RowItem(6, dro.TenLoaiKhieuTo1, null, null, null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem7 = new RowItem(7, NgayRut, null, null, null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem8 = new RowItem(8, dro.LyDo, null, null, null, "text-align: left;" + Css, ref DataArr);
                RowItem RowItem9 = new RowItem(9, dro.TenCoQuan, null, null, null, "text-align: left;" + Css, ref DataArr);

                tableData.DataArr = DataArr;
                data.Add(tableData);
            }

            return data;
        }

        public string ThongKeTheoRutDon_Excel(string rootPath, string pathFile, List<TableData> data, DateTime tuNgay, DateTime denNgay)
        {
            //var camera = new Camera;
            // path to your excel file
            string path = rootPath + @"\Templates\BaoCao\ThongKeTheoRutDon.xlsx";
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

        public List<TableData> TongHopTinhHinhTCD_XL_GQD(List<int> listCapChonBaoCao, IdentityHelper IdentityHelper, string ContentRootPath, int CoQuanID, DateTime startDate, DateTime endDate)
        {
            List<TableData> data = new List<TableData>();
            List<BCTinhHinhTD_XLD_GQInfo> lsData = new List<BCTinhHinhTD_XLD_GQInfo>();
            DateTime tuNgays = startDate;
            DateTime denNgays = endDate;
            int coQuanIDs = CoQuanID;
            int capUser = IdentityHelper.CapID ?? 0;
            int coQuanUserID = IdentityHelper.CoQuanID ?? 0;
            int tinhID = IdentityHelper.TinhID ?? 0;
            int pRoleUser = IdentityHelper.RoleID ?? 0;
            if (tuNgays == DateTime.Now.Date && denNgays == DateTime.Now.Date)
            {
                denNgays = denNgays.AddDays(1);
            }
            Boolean laThanhTraTinh = false;
            var listThanhTraTinh = new SystemConfigDAL().GetByKey("Thanh_Tra_Tinh_ID").ConfigValue.Split(',').ToList().Select(x => Utils.ConvertToInt32(x.ToString(), 0)).ToList();
            if (listThanhTraTinh.Contains(IdentityHelper.CoQuanID ?? 0) && IdentityHelper.RoleID == (int)EnumChucVu.LanhDao)
            {
                laThanhTraTinh = true;
            }


            QueryFilterInfo infoQF = new QueryFilterInfo()
            {
                TuNgayGoc = tuNgays,
                TuNgayMoi = tuNgays,

                DenNgayGoc = denNgays,
                DenNgayMoi = denNgays,
                CoQuanID = coQuanUserID,
                PTKQDung = (int)PhanTichKQEnum.Dung,
                PTKQDungMotPhan = (int)PhanTichKQEnum.DungMotPhan,
                PTKQSai = (int)PhanTichKQEnum.Sai,
            };



            try
            {
                IList<BCTongHopXuLyInfo> xldList = new BCTinhHinhTD_XLD_GQ().DSCoQuan_XuLyDon_GetByDate(infoQF);
                IList<BCTongHopXuLyInfo> gqdList = new BCTinhHinhTD_XLD_GQ().DSCoQuan_GiaiQuyetDon_GetByDate(infoQF);
                //IList<BCTongHopXuLyInfo> xldList = new BCTinhHinhTD_XLD_GQ().DashBoard_ThongKeXuLyDon_GetByDate(infoQF);
                //IList<BCTongHopXuLyInfo> gqdList = new BCTinhHinhTD_XLD_GQ().DashBoard_ThongKeGiaiQuyetDon_GetByDate(infoQF);
                //if (capUser == (int)CapQuanLy.CapUBNDTinh || coQuanUserID == (int)EnumCoQuan.BanTCDTinh || (coQuanUserID == (int)EnumCoQuan.ThanhTraTinh && pRoleUser == (int)EnumChucVu.LanhDao))
                if ((capUser == (int)CapQuanLy.CapUBNDTinh) || laThanhTraTinh)
                {
                    List<BCTinhHinhTD_XLD_GQInfo> tempList = new CapDAL().GetAllCap().ToList();
                    lsData = tempList.Where(x => x.CapID != (int)CapQuanLy.CapTrungUong && x.CapID != (int)CapQuanLy.CapPhong).ToList();
                    if (pRoleUser != (int)EnumChucVu.LanhDao)
                    {
                        lsData = lsData.Where(x => x.CapID == capUser).ToList();
                    }
                    foreach (BCTinhHinhTD_XLD_GQInfo item in lsData)
                    {
                        if (item.CapID == (int)CapQuanLy.CapUBNDHuyen)
                        {
                            List<HuyenInfo> huyenList = new Huyen().GetByTinh(IdentityHelper.TinhID ?? 0).ToList();
                            List<BCTinhHinhTD_XLD_GQInfo> lstCQCapPhong = new CoQuan().GetCoQuanByCapForBC((int)CapQuanLy.CapPhong, tinhID).ToList();
                            List<BCTinhHinhTD_XLD_GQInfo> lstCQCapUBHuyen = new CoQuan().GetCoQuanByCapForBC((int)CapQuanLy.CapUBNDHuyen, tinhID).ToList();
                            List<BCTinhHinhTD_XLD_GQInfo> lstCQCapHuyen = new List<BCTinhHinhTD_XLD_GQInfo>();
                            foreach (BCTinhHinhTD_XLD_GQInfo itemCoQuan in lstCQCapUBHuyen)
                            {
                                lstCQCapHuyen.Add(itemCoQuan);
                            }
                            foreach (BCTinhHinhTD_XLD_GQInfo itemCoQuan in lstCQCapPhong)
                            {
                                lstCQCapHuyen.Add(itemCoQuan);
                            }

                            item.LsByCapCoQuan = new List<BCTinhHinhTD_XLD_CapCoQuan_GQInfo>();
                            foreach (HuyenInfo huyenInfo in huyenList)
                            {
                                BCTinhHinhTD_XLD_CapCoQuan_GQInfo lstXLDCapCoQuan = new BCTinhHinhTD_XLD_CapCoQuan_GQInfo();
                                lstXLDCapCoQuan.TenCoQuan = huyenInfo.TenHuyen;
                                lstXLDCapCoQuan.CoQuanID = huyenInfo.HuyenID;
                                lstXLDCapCoQuan.LsByCoQuan = lstCQCapHuyen.Where(x => x.HuyenID == huyenInfo.HuyenID).ToList();

                                foreach (BCTinhHinhTD_XLD_GQInfo itemCoQuan in lstXLDCapCoQuan.LsByCoQuan)
                                {
                                    foreach (BCTongHopXuLyInfo raw in xldList)
                                    {
                                        if (itemCoQuan.CoQuanID == raw.CoQuanID)
                                        {
                                            itemCoQuan.SoDonTiepDan = raw.SLTiepCongDan;
                                            itemCoQuan.SoXLTrongHan = raw.XLDDaXuLyTrongHan;
                                            itemCoQuan.SoXLQuaHan = raw.XLDDaXuLyQuaHan;
                                            itemCoQuan.SoXLDaXuLy = raw.XLDDaXuLy;
                                            itemCoQuan.SoXLChuaXuLy = raw.XLDChuaXuLy;
                                            itemCoQuan.TongSoXL = raw.XLDTongSo;
                                            itemCoQuan.VuViecDongNguoi = raw.XLDKhieuKienDN;
                                        }
                                    }
                                    foreach (BCTongHopXuLyInfo raw in gqdList)
                                    {
                                        var cqphItem = "," + itemCoQuan.CoQuanID + ",";
                                        if (raw.CoQuanGiaiQuyetID == 0)
                                            raw.CoQuanGiaiQuyetID = raw.CoQuanID;
                                        if (itemCoQuan.CoQuanID == raw.CoQuanGiaiQuyetID)
                                        {

                                            itemCoQuan.TongSoDonGiaoGQ += raw.GQDTongSo;
                                            itemCoQuan.SoDangGQTrongHan += raw.GQDDangGQTrongHan;
                                            itemCoQuan.SoDangGQQuaHan += raw.GQDDangGQQuaHan;
                                            itemCoQuan.TongSoDangGQ += raw.GQDDangGQ;
                                            itemCoQuan.DaCoBC += raw.GQDDaGQ;
                                            itemCoQuan.ChuaGiaiQuyet += raw.GQDChuaGQ;
                                            itemCoQuan.DaBHGQDung += raw.KQKNDung;
                                            itemCoQuan.DaBHGQDungMotPhan += raw.KQKNDungMotPhan;
                                            itemCoQuan.DaBHGQSai += raw.KQKNSai;
                                            itemCoQuan.DaBHGQ = (itemCoQuan.DaBHGQDung + itemCoQuan.DaBHGQDungMotPhan + itemCoQuan.DaBHGQSai);

                                        }
                                    }

                                }
                                item.LsByCapCoQuan.Add(lstXLDCapCoQuan);
                            }
                        }
                        else if (item.CapID == (int)CapQuanLy.CapUBNDXa)
                        {
                            List<HuyenInfo> huyenList = new Huyen().GetByTinh(IdentityHelper.TinhID ?? 0).ToList();
                            List<BCTinhHinhTD_XLD_GQInfo> lstCQCapPhong = new CoQuan().GetCoQuanByCapForBC((int)CapQuanLy.CapUBNDXa, tinhID).ToList();
                            item.LsByCapCoQuan = new List<BCTinhHinhTD_XLD_CapCoQuan_GQInfo>();
                            foreach (HuyenInfo huyenInfo in huyenList)
                            {
                                BCTinhHinhTD_XLD_CapCoQuan_GQInfo lstXLDCapCoQuan = new BCTinhHinhTD_XLD_CapCoQuan_GQInfo();
                                lstXLDCapCoQuan.TenCoQuan = huyenInfo.TenHuyen;
                                lstXLDCapCoQuan.CoQuanID = huyenInfo.HuyenID;
                                lstXLDCapCoQuan.LsByCoQuan = lstCQCapPhong.Where(x => x.HuyenID == huyenInfo.HuyenID).ToList();

                                foreach (BCTinhHinhTD_XLD_GQInfo itemCoQuan in lstXLDCapCoQuan.LsByCoQuan)
                                {
                                    foreach (BCTongHopXuLyInfo raw in xldList)
                                    {
                                        if (itemCoQuan.CoQuanID == raw.CoQuanID)
                                        {
                                            itemCoQuan.SoDonTiepDan = raw.SLTiepCongDan;
                                            itemCoQuan.SoXLTrongHan = raw.XLDDaXuLyTrongHan;
                                            itemCoQuan.SoXLQuaHan = raw.XLDDaXuLyQuaHan;
                                            itemCoQuan.SoXLDaXuLy = raw.XLDDaXuLy;
                                            itemCoQuan.SoXLChuaXuLy = raw.XLDChuaXuLy;
                                            itemCoQuan.TongSoXL = raw.XLDTongSo;
                                            itemCoQuan.VuViecDongNguoi = raw.XLDKhieuKienDN;
                                        }
                                    }
                                    foreach (BCTongHopXuLyInfo raw in gqdList)
                                    {
                                        var cqphItem = "," + itemCoQuan.CoQuanID + ",";
                                        if (raw.CoQuanGiaiQuyetID == 0)
                                            raw.CoQuanGiaiQuyetID = raw.CoQuanID;
                                        if (itemCoQuan.CoQuanID == raw.CoQuanGiaiQuyetID)
                                        {

                                            itemCoQuan.TongSoDonGiaoGQ += raw.GQDTongSo;
                                            itemCoQuan.SoDangGQTrongHan += raw.GQDDangGQTrongHan;
                                            itemCoQuan.SoDangGQQuaHan += raw.GQDDangGQQuaHan;
                                            itemCoQuan.TongSoDangGQ += raw.GQDDangGQ;
                                            itemCoQuan.DaCoBC += raw.GQDDaGQ;
                                            itemCoQuan.ChuaGiaiQuyet += raw.GQDChuaGQ;
                                            itemCoQuan.DaBHGQDung += raw.KQKNDung;
                                            itemCoQuan.DaBHGQDungMotPhan += raw.KQKNDungMotPhan;
                                            itemCoQuan.DaBHGQSai += raw.KQKNSai;
                                            itemCoQuan.DaBHGQ = (itemCoQuan.DaBHGQDung + itemCoQuan.DaBHGQDungMotPhan + itemCoQuan.DaBHGQSai);
                                        }
                                    }
                                }
                                item.LsByCapCoQuan.Add(lstXLDCapCoQuan);
                            }
                        }
                        else
                        {
                            if (pRoleUser == (int)EnumChucVu.LanhDao)
                            {
                                item.LsByCoQuan = new CoQuan().GetCoQuanByCapForBC(item.CapID, tinhID).ToList();
                            }
                            else
                            {
                                item.LsByCoQuan = new CoQuan().GetCoQuanByCapForBC(item.CapID, tinhID).Where(x => x.CoQuanID == coQuanUserID).ToList();

                            }
                            foreach (BCTinhHinhTD_XLD_GQInfo itemCoQuan in item.LsByCoQuan)
                            {
                                foreach (BCTongHopXuLyInfo raw in xldList)
                                {
                                    if (itemCoQuan.CoQuanID == raw.CoQuanID)
                                    {
                                        itemCoQuan.SoDonTiepDan = raw.SLTiepCongDan;
                                        itemCoQuan.SoXLTrongHan = raw.XLDDaXuLyTrongHan;
                                        itemCoQuan.SoXLQuaHan = raw.XLDDaXuLyQuaHan;
                                        itemCoQuan.SoXLDaXuLy = raw.XLDDaXuLy;
                                        itemCoQuan.SoXLChuaXuLy = raw.XLDChuaXuLy;
                                        itemCoQuan.TongSoXL = raw.XLDTongSo;
                                        itemCoQuan.VuViecDongNguoi = raw.XLDKhieuKienDN;
                                    }
                                }
                                foreach (BCTongHopXuLyInfo raw in gqdList)
                                {
                                    var cqphItem = "," + itemCoQuan.CoQuanID + ",";
                                    if (raw.CoQuanGiaiQuyetID == 0)
                                        raw.CoQuanGiaiQuyetID = raw.CoQuanID;
                                    if (itemCoQuan.CoQuanID == raw.CoQuanGiaiQuyetID)
                                    {
                                        itemCoQuan.TongSoDonGiaoGQ += raw.GQDTongSo;
                                        itemCoQuan.SoDangGQTrongHan += raw.GQDDangGQTrongHan;
                                        itemCoQuan.SoDangGQQuaHan += raw.GQDDangGQQuaHan;
                                        itemCoQuan.TongSoDangGQ += raw.GQDDangGQ;
                                        itemCoQuan.DaCoBC += raw.GQDDaGQ;
                                        itemCoQuan.ChuaGiaiQuyet += raw.GQDChuaGQ;
                                        itemCoQuan.DaBHGQDung += raw.KQKNDung;
                                        itemCoQuan.DaBHGQDungMotPhan += raw.KQKNDungMotPhan;
                                        itemCoQuan.DaBHGQSai += raw.KQKNSai;
                                        itemCoQuan.DaBHGQ = (itemCoQuan.DaBHGQDung + itemCoQuan.DaBHGQDungMotPhan + itemCoQuan.DaBHGQSai);
                                    }
                                }

                            }

                        }

                    }

                }
                else if ((capUser == (int)CapQuanLy.CapSoNganh && coQuanUserID != (int)EnumCoQuan.BanTCDTinh) || capUser == (int)CapQuanLy.CapPhong || capUser == (int)CapQuanLy.CapUBNDXa)
                {
                    List<BCTinhHinhTD_XLD_GQInfo> tempList = new CapDAL().GetAllCap().ToList();
                    lsData = tempList.Where(x => x.CapID == capUser).ToList();
                    foreach (BCTinhHinhTD_XLD_GQInfo item in lsData)
                    {
                        item.LsByCoQuan = new CoQuan().GetCoQuanByCapForBC(item.CapID, tinhID).Where(x => x.CoQuanID == coQuanUserID).ToList();
                        foreach (BCTinhHinhTD_XLD_GQInfo itemCoQuan in item.LsByCoQuan)
                        {
                            foreach (BCTongHopXuLyInfo raw in xldList)
                            {
                                if (itemCoQuan.CoQuanID == raw.CoQuanID)
                                {
                                    itemCoQuan.SoDonTiepDan = raw.SLTiepCongDan;
                                    itemCoQuan.SoXLTrongHan = raw.XLDDaXuLyTrongHan;
                                    itemCoQuan.SoXLQuaHan = raw.XLDDaXuLyQuaHan;
                                    itemCoQuan.SoXLDaXuLy = raw.XLDDaXuLy;
                                    itemCoQuan.SoXLChuaXuLy = raw.XLDChuaXuLy;
                                    itemCoQuan.TongSoXL = raw.XLDTongSo;
                                }
                            }
                            foreach (BCTongHopXuLyInfo raw in gqdList)
                            {
                                var cqph = "," + raw.CQPhoiHopStr + ",";
                                var cqphItem = "," + itemCoQuan.CoQuanID + ",";
                                if (itemCoQuan.CoQuanID == raw.CoQuanGiaiQuyetID)
                                {
                                    itemCoQuan.TongSoDonGiaoGQ += raw.GQDTongSo;
                                    itemCoQuan.SoDangGQTrongHan += raw.GQDDangGQTrongHan;
                                    itemCoQuan.SoDangGQQuaHan += raw.GQDDangGQQuaHan;
                                    itemCoQuan.TongSoDangGQ += raw.GQDDangGQ;
                                    itemCoQuan.DaCoBC += raw.GQDDaGQ;
                                    itemCoQuan.ChuaGiaiQuyet += raw.GQDChuaGQ;
                                    itemCoQuan.DaBHGQDung += raw.KQKNDung;
                                    itemCoQuan.DaBHGQDungMotPhan += raw.KQKNDungMotPhan;
                                    itemCoQuan.DaBHGQSai += raw.KQKNSai;
                                    itemCoQuan.DaBHGQ = (itemCoQuan.DaBHGQDung + itemCoQuan.DaBHGQDungMotPhan + itemCoQuan.DaBHGQSai);
                                }
                            }
                        }
                    }
                }
                else if (capUser == (int)CapQuanLy.CapUBNDHuyen)
                {
                    List<BCTinhHinhTD_XLD_GQInfo> tempList = new CapDAL().GetAllCap().ToList();
                    if (pRoleUser == (int)EnumChucVu.LanhDao && IdentityHelper.CapID == (int)CapQuanLy.CapUBNDTinh) /*lãnh đạo tỉnh chọn xem thong tin trong 1 huyện*/
                    {
                        lsData = tempList.Where(x => x.CapID == (int)CapQuanLy.CapUBNDHuyen || x.CapID == (int)CapQuanLy.CapUBNDXa).ToList();
                    }
                    else if (pRoleUser == (int)EnumChucVu.LanhDao) /*lãnh đạo chọn xem thong tin trong toàn huyện*/
                    {
                        lsData = tempList.Where(x => x.CapID == (int)CapQuanLy.CapUBNDHuyen || x.CapID == (int)CapQuanLy.CapUBNDXa).ToList();
                    }
                    else
                    {
                        lsData = tempList.Where(x => x.CapID == (int)CapQuanLy.CapUBNDHuyen).ToList();
                    }


                    foreach (BCTinhHinhTD_XLD_GQInfo item in lsData)
                    {
                        if (item.CapID == (int)CapQuanLy.CapUBNDHuyen)
                        {
                            List<BCTinhHinhTD_XLD_GQInfo> lstCQCapPhong = new CoQuan().GetCoQuanByCapForBC((int)CapQuanLy.CapPhong, tinhID).ToList();
                            List<BCTinhHinhTD_XLD_GQInfo> lstCQCapUBHuyen = new CoQuan().GetCoQuanByCapForBC((int)CapQuanLy.CapUBNDHuyen, tinhID).ToList();
                            List<BCTinhHinhTD_XLD_GQInfo> lstCQCapHuyen = new List<BCTinhHinhTD_XLD_GQInfo>();
                            foreach (BCTinhHinhTD_XLD_GQInfo itemCoQuan in lstCQCapUBHuyen)
                            {
                                lstCQCapHuyen.Add(itemCoQuan);
                            }
                            foreach (BCTinhHinhTD_XLD_GQInfo itemCoQuan in lstCQCapPhong)
                            {
                                lstCQCapHuyen.Add(itemCoQuan);
                            }

                            item.LsByCapCoQuan = new List<BCTinhHinhTD_XLD_CapCoQuan_GQInfo>();

                            BCTinhHinhTD_XLD_CapCoQuan_GQInfo lstXLDCapCoQuan = new BCTinhHinhTD_XLD_CapCoQuan_GQInfo();
                            HuyenInfo huyenInfo = new Huyen().GetByID(IdentityHelper.HuyenID ?? 0);
                            lstXLDCapCoQuan.TenCoQuan = huyenInfo.TenHuyen;
                            lstXLDCapCoQuan.CoQuanID = huyenInfo.HuyenID;
                            if (pRoleUser == (int)EnumChucVu.LanhDao)
                            {

                                lstXLDCapCoQuan.LsByCoQuan = lstCQCapHuyen.Where(x => x.HuyenID == huyenInfo.HuyenID).ToList();
                            }
                            else
                            {
                                lstXLDCapCoQuan.LsByCoQuan = lstCQCapHuyen.Where(x => x.CoQuanID == coQuanUserID).ToList();
                            }
                            foreach (BCTinhHinhTD_XLD_GQInfo itemCoQuan in lstXLDCapCoQuan.LsByCoQuan)
                            {
                                foreach (BCTongHopXuLyInfo raw in xldList)
                                {
                                    if (itemCoQuan.CoQuanID == raw.CoQuanID)
                                    {
                                        itemCoQuan.SoDonTiepDan = raw.SLTiepCongDan;
                                        itemCoQuan.SoXLTrongHan = raw.XLDDaXuLyTrongHan;
                                        itemCoQuan.SoXLQuaHan = raw.XLDDaXuLyQuaHan;
                                        itemCoQuan.SoXLDaXuLy = raw.XLDDaXuLy;
                                        itemCoQuan.SoXLChuaXuLy = raw.XLDChuaXuLy;
                                        itemCoQuan.TongSoXL = raw.XLDTongSo;
                                        itemCoQuan.VuViecDongNguoi = raw.XLDKhieuKienDN;
                                    }
                                }
                                foreach (BCTongHopXuLyInfo raw in gqdList)
                                {
                                    var cqphItem = "," + itemCoQuan.CoQuanID + ",";
                                    if (raw.CoQuanGiaiQuyetID == 0)
                                        raw.CoQuanGiaiQuyetID = raw.CoQuanID;
                                    if (itemCoQuan.CoQuanID == raw.CoQuanGiaiQuyetID)
                                    {
                                        itemCoQuan.TongSoDonGiaoGQ += raw.GQDTongSo;
                                        itemCoQuan.SoDangGQTrongHan += raw.GQDDangGQTrongHan;
                                        itemCoQuan.SoDangGQQuaHan += raw.GQDDangGQQuaHan;
                                        itemCoQuan.TongSoDangGQ += raw.GQDDangGQ;
                                        itemCoQuan.DaCoBC += raw.GQDDaGQ;
                                        itemCoQuan.ChuaGiaiQuyet += raw.GQDChuaGQ;
                                        itemCoQuan.DaBHGQDung += raw.KQKNDung;
                                        itemCoQuan.DaBHGQDungMotPhan += raw.KQKNDungMotPhan;
                                        itemCoQuan.DaBHGQSai += raw.KQKNSai;
                                        itemCoQuan.DaBHGQ = (itemCoQuan.DaBHGQDung + itemCoQuan.DaBHGQDungMotPhan + itemCoQuan.DaBHGQSai);

                                    }
                                }
                            }
                            item.LsByCapCoQuan.Add(lstXLDCapCoQuan);
                        }
                        else if (item.CapID == (int)CapQuanLy.CapUBNDXa)
                        {
                            List<BCTinhHinhTD_XLD_GQInfo> lstCQCapPhong = new CoQuan().GetCoQuanByCapForBC((int)CapQuanLy.CapUBNDXa, tinhID).ToList();

                            BCTinhHinhTD_XLD_CapCoQuan_GQInfo lstXLDCapCoQuan = new BCTinhHinhTD_XLD_CapCoQuan_GQInfo();
                            HuyenInfo huyenInfo = new HuyenInfo();
                            huyenInfo = new Huyen().GetByID(IdentityHelper.HuyenID ?? 0);
                            lstXLDCapCoQuan.TenCoQuan = huyenInfo.TenHuyen;
                            lstXLDCapCoQuan.CoQuanID = huyenInfo.HuyenID;
                            lstXLDCapCoQuan.LsByCoQuan = lstCQCapPhong.Where(x => x.HuyenID == huyenInfo.HuyenID).ToList();
                            item.LsByCapCoQuan = new List<BCTinhHinhTD_XLD_CapCoQuan_GQInfo>();
                            foreach (BCTinhHinhTD_XLD_GQInfo itemCoQuan in lstXLDCapCoQuan.LsByCoQuan)
                            {
                                foreach (BCTongHopXuLyInfo raw in xldList)
                                {
                                    if (itemCoQuan.CoQuanID == raw.CoQuanID)
                                    {
                                        itemCoQuan.SoDonTiepDan = raw.SLTiepCongDan;
                                        itemCoQuan.SoXLTrongHan = raw.XLDDaXuLyTrongHan;
                                        itemCoQuan.SoXLQuaHan = raw.XLDDaXuLyQuaHan;
                                        itemCoQuan.SoXLDaXuLy = raw.XLDDaXuLy;
                                        itemCoQuan.SoXLChuaXuLy = raw.XLDChuaXuLy;
                                        itemCoQuan.TongSoXL = raw.XLDTongSo;
                                        itemCoQuan.VuViecDongNguoi = raw.XLDKhieuKienDN;
                                    }
                                }
                                foreach (BCTongHopXuLyInfo raw in gqdList)
                                {
                                    var cqphItem = "," + itemCoQuan.CoQuanID + ",";
                                    if (raw.CoQuanGiaiQuyetID == 0)
                                        raw.CoQuanGiaiQuyetID = raw.CoQuanID;
                                    if (itemCoQuan.CoQuanID == raw.CoQuanGiaiQuyetID)
                                    {
                                        itemCoQuan.TongSoDonGiaoGQ += raw.GQDTongSo;
                                        itemCoQuan.SoDangGQTrongHan += raw.GQDDangGQTrongHan;
                                        itemCoQuan.SoDangGQQuaHan += raw.GQDDangGQQuaHan;
                                        itemCoQuan.TongSoDangGQ += raw.GQDDangGQ;
                                        itemCoQuan.DaCoBC += raw.GQDDaGQ;
                                        itemCoQuan.ChuaGiaiQuyet += raw.GQDChuaGQ;
                                        itemCoQuan.DaBHGQDung += raw.KQKNDung;
                                        itemCoQuan.DaBHGQDungMotPhan += raw.KQKNDungMotPhan;
                                        itemCoQuan.DaBHGQSai += raw.KQKNSai;
                                        itemCoQuan.DaBHGQ = (itemCoQuan.DaBHGQDung + itemCoQuan.DaBHGQDungMotPhan + itemCoQuan.DaBHGQSai);

                                    }
                                }

                            }
                            item.LsByCapCoQuan.Add(lstXLDCapCoQuan);
                        }
                    }
                }
            }
            catch (Exception ex) { }
            
            

            int stt = 0;
            foreach (BCTinhHinhTD_XLD_GQInfo dro in lsData)
            {
                if((dro.CapID == 2 || dro.CapID == 3) && dro.LsByCapCoQuan != null)
                {
                    TableData tableDataCap = new TableData();
                    tableDataCap.ID = stt++;
                    var DataArrCap = new List<RowItem>();
                    string Css = "";
                    RowItem CapRowItem1 = new RowItem(1, stt.ToString(), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, dro.Style, ref DataArrCap);
                    RowItem CapRowItem2 = new RowItem(2, dro.TenCap, dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: left;" + Css, ref DataArrCap);
                    RowItem CapRowItem3 = new RowItem(3, Utils.AddCommasDouble(dro.SoDonTiepDan.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArrCap);
                    RowItem CapRowItem4 = new RowItem(4, Utils.AddCommasDouble(dro.SoXLTrongHan.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArrCap);
                    RowItem CapRowItem5 = new RowItem(5, Utils.AddCommasDouble(dro.SoXLQuaHan.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArrCap);
                    RowItem CapRowItem6 = new RowItem(6, Utils.AddCommasDouble(dro.SoXLDaXuLy.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArrCap);
                    RowItem CapRowItem7 = new RowItem(7, Utils.AddCommasDouble(dro.TongSoDonGiaoGQ.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArrCap);
                    RowItem CapRowItem8 = new RowItem(8, Utils.AddCommasDouble(dro.ChuaGiaiQuyet.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArrCap);
                    RowItem CapRowItem9 = new RowItem(9, Utils.AddCommasDouble(dro.SoDangGQTrongHan.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArrCap);
                    RowItem CapRowItem10 = new RowItem(10, Utils.AddCommasDouble(dro.SoDangGQQuaHan.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArrCap);
                    RowItem CapRowItem11 = new RowItem(11, Utils.AddCommasDouble(dro.TongSoDangGQ.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArrCap);
                    RowItem CapRowItem12 = new RowItem(12, Utils.AddCommasDouble(dro.DaCoBC.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArrCap);
                    RowItem CapRowItem13 = new RowItem(13, Utils.AddCommasDouble(dro.DaBHGQDung.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArrCap);
                    RowItem CapRowItem14 = new RowItem(14, Utils.AddCommasDouble(dro.DaBHGQDungMotPhan.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArrCap);
                    RowItem CapRowItem15 = new RowItem(15, Utils.AddCommasDouble(dro.DaBHGQSai.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArrCap);

                    tableDataCap.DataArr = DataArrCap;
                    data.Add(tableDataCap);

                    var dataByCoQuan = dro.LsByCapCoQuan;
                    var tsLuotTiepDanSN = 0;
                    var tsLuotXLDonSN = 0;
                    var tsKhieuNaiDangGQSN = 0;
                    var tsKhieuNaiDaGQSN = 0;
                    var tsToCaoDangGQSN = 0;
                    var tsToCaoDaGQSN = 0;
                    var tsKNPADangGQSN = 0;
                    var tsKNPADaGQSN = 0;
                    var tsVuViecDongNguoiSN = 0;
                    for (var k = 0; k < dataByCoQuan.Count; k++)
                    {
                        TableData tableData = new TableData();
                        tableData.ID = stt++;
                        var DataArr = new List<RowItem>();

                        int RowItem_CapID = dro.CapID;
                       
                        if (RowItem_CapID == CapCoQuanViewChiTiet.ToanTinh.GetHashCode() || RowItem_CapID == CapCoQuanViewChiTiet.CapUBNDTinh.GetHashCode()
                            || RowItem_CapID == CapCoQuanViewChiTiet.ToanHuyen.GetHashCode() || RowItem_CapID == CapCoQuanViewChiTiet.CapSoNganh.GetHashCode()
                            || RowItem_CapID == CapCoQuanViewChiTiet.CapUBNDHuyen.GetHashCode() || RowItem_CapID == CapCoQuanViewChiTiet.CapUBNDXa.GetHashCode())
                        {
                            Css = "font-weight: bold;";
                        }
                        string TenCoQuan = "";
                        if (dataByCoQuan[k].CoQuanID == 218 || dataByCoQuan[k].CoQuanID == 377)
                        {
                            TenCoQuan = "Thành phố " + dataByCoQuan[k].TenCoQuan;
                        }
                        else
                        {
                            TenCoQuan = "Huyện " + dataByCoQuan[k].TenCoQuan;
                        }

                        RowItem RowItem1 = new RowItem(1, stt.ToString(), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, dro.Style, ref DataArr);
                        RowItem RowItem2 = new RowItem(2, TenCoQuan ?? dro.TenCap, dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: left;" + Css, ref DataArr);
                        RowItem RowItem3 = new RowItem(3, Utils.AddCommasDouble(dro.SoDonTiepDan.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                        RowItem RowItem4 = new RowItem(4, Utils.AddCommasDouble(dro.SoXLTrongHan.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                        RowItem RowItem5 = new RowItem(5, Utils.AddCommasDouble(dro.SoXLQuaHan.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                        RowItem RowItem6 = new RowItem(6, Utils.AddCommasDouble(dro.SoXLDaXuLy.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                        RowItem RowItem7 = new RowItem(7, Utils.AddCommasDouble(dro.TongSoDonGiaoGQ.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                        RowItem RowItem8 = new RowItem(8, Utils.AddCommasDouble(dro.ChuaGiaiQuyet.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                        RowItem RowItem9 = new RowItem(9, Utils.AddCommasDouble(dro.SoDangGQTrongHan.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                        RowItem RowItem10 = new RowItem(10, Utils.AddCommasDouble(dro.SoDangGQQuaHan.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                        RowItem RowItem11 = new RowItem(11, Utils.AddCommasDouble(dro.TongSoDangGQ.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                        RowItem RowItem12 = new RowItem(12, Utils.AddCommasDouble(dro.DaCoBC.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                        RowItem RowItem13 = new RowItem(13, Utils.AddCommasDouble(dro.DaBHGQDung.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                        RowItem RowItem14 = new RowItem(14, Utils.AddCommasDouble(dro.DaBHGQDungMotPhan.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                        RowItem RowItem15 = new RowItem(15, Utils.AddCommasDouble(dro.DaBHGQSai.ToString()), dro.CoQuanID.ToString(), dro.CapID.ToString(), null, "text-align: center;" + Css, ref DataArr);

                        tableData.DataArr = DataArr;
                        data.Add(tableData);
                        //var tmpChiTietCap = "<tr class='showCoQuan_" + dataCap[i].CapID + "'>" + $("#tmpChiTietCap > table tr:first-child").html() + "</tr>";
                        //if (dataByCoQuan[k].CoQuanID == 60 || dataByCoQuan[k].CoQuanID == 66)
                        //{
                        //    tmpChiTietCap = tmpChiTietCap.replace(/ _tenubnd_ / g, "Thành phố " + dataByCoQuan[k].TenCoQuan);
                        //}
                        //else
                        //{
                        //    tmpChiTietCap = tmpChiTietCap.replace(/ _tenubnd_ / g, "Huyện " + dataByCoQuan[k].TenCoQuan);
                        //}
                        //tmpChiTietCap = tmpChiTietCap.replace(/ _ChiTietCapID_ / g, dataByCoQuan[k].CoQuanID);
                        //tmpChiTietCap = tmpChiTietCap.replace(/ _CapID_ / g, dataCap[i].CapID);

                        //$("#baocao > tbody.bdbaocao").append(tmpChiTietCap);

                        var tsLuotTiepDan = 0;
                        var soDonTrongHan = 0;
                        var soDonQuaHan = 0;
                        var tsLuotXLDon = 0;
                        var tsDonXuLy = 0;
                        var tsGiaiQuyetDon = 0;
                        var tsChuaGiaiQuyetDon = 0;
                        var soDangGiaiQuyetTH = 0;
                        var soDangGiaiQuyetQH = 0;
                        var tsDangGiaiQuyet = 0;
                        var daCoBaoCao = 0;
                        var khieuNaiDung = 0;
                        var khieuNaiDungMotPhan = 0;
                        var khieuNaiSai = 0;
                        var dataChiTietByCoQuan = dataByCoQuan[k].LsByCoQuan;
                        for (var l = 0; l < dataChiTietByCoQuan.Count; l++)
                        {
                            //var tmplCoQuan = "<tr style='display:none;' class='cl_hiden showCoQuanCT_" + dataCap[i].CapID + "13" + dataByCoQuan[k].CoQuanID + "'>" + $("#tmpChiTiet > table tr:first-child").html() + "</tr>";

                            //tmplCoQuan = tmplCoQuan.replace(/ _stt_ / g, stt);
                            //tmplCoQuan = tmplCoQuan.replace(/ _tencoquan_ / g, dataChiTietByCoQuan[l].TenCoQuan);
                            //tmplCoQuan = tmplCoQuan.replace(/ _CapID_ / g, dataChiTietByCoQuan[l].CapID);
                            //tmplCoQuan = tmplCoQuan.replace(/ _CoQuanID_ / g, dataChiTietByCoQuan[l].CoQuanID);
                            //tmplCoQuan = tmplCoQuan.replace(/ _pDonViID_ / g, dataChiTietByCoQuan[l].CoQuanID);
                            //tmplCoQuan = tmplCoQuan.replace(/ _sotiepcongdan_ / g, dataChiTietByCoQuan[l].SoDonTiepDan);
                            //tsLuotTiepDan = tsLuotTiepDan + dataChiTietByCoQuan[l].SoDonTiepDan;

                            //tmplCoQuan = tmplCoQuan.replace(/ _soxltronghan_ / g, dataChiTietByCoQuan[l].SoXLTrongHan);
                            //soDonTrongHan = soDonTrongHan + dataChiTietByCoQuan[l].SoXLTrongHan;

                            //tmplCoQuan = tmplCoQuan.replace(/ _soxlquahan_ / g, dataChiTietByCoQuan[l].SoXLQuaHan);
                            //soDonQuaHan = soDonQuaHan + dataChiTietByCoQuan[l].SoXLQuaHan;

                            //tmplCoQuan = tmplCoQuan.replace(/ _tongsoxl_ / g, dataChiTietByCoQuan[l].SoXLDaXuLy);
                            //tsLuotXLDon = tsLuotXLDon + dataChiTietByCoQuan[l].SoXLDaXuLy;

                            //tmplCoQuan = tmplCoQuan.replace(/ _tongsogiaogq_ / g, dataChiTietByCoQuan[l].TongSoDonGiaoGQ);
                            //tsGiaiQuyetDon = tsGiaiQuyetDon + dataChiTietByCoQuan[l].TongSoDonGiaoGQ;

                            //tmplCoQuan = tmplCoQuan.replace(/ _tongsochuagq_ / g, dataChiTietByCoQuan[l].ChuaGiaiQuyet);
                            //tsChuaGiaiQuyetDon = tsChuaGiaiQuyetDon + dataChiTietByCoQuan[l].ChuaGiaiQuyet;

                            //tmplCoQuan = tmplCoQuan.replace(/ _sodanggqtronghan_ / g, dataChiTietByCoQuan[l].SoDangGQTrongHan);
                            //soDangGiaiQuyetTH = soDangGiaiQuyetTH + dataChiTietByCoQuan[l].SoDangGQTrongHan;

                            //tmplCoQuan = tmplCoQuan.replace(/ _sodanggqquahan_ / g, dataChiTietByCoQuan[l].SoDangGQQuaHan);
                            //soDangGiaiQuyetQH = soDangGiaiQuyetQH + dataChiTietByCoQuan[l].SoDangGQQuaHan;

                            //tmplCoQuan = tmplCoQuan.replace(/ _tongsodanggq_ / g, dataChiTietByCoQuan[l].TongSoDangGQ);
                            //tsDangGiaiQuyet = tsDangGiaiQuyet + dataChiTietByCoQuan[l].TongSoDangGQ;

                            //tmplCoQuan = tmplCoQuan.replace(/ _dacobc_ / g, dataChiTietByCoQuan[l].DaCoBC);
                            //daCoBaoCao = daCoBaoCao + dataChiTietByCoQuan[l].DaCoBC;

                            //tmplCoQuan = tmplCoQuan.replace(/ _dabanhanhKnDung_ / g, dataChiTietByCoQuan[l].DaBHGQDung);
                            //khieuNaiDung = khieuNaiDung + dataChiTietByCoQuan[l].DaBHGQDung;

                            //tmplCoQuan = tmplCoQuan.replace(/ _dabanhanhKnPhan_ / g, dataChiTietByCoQuan[l].DaBHGQDungMotPhan);
                            //khieuNaiDungMotPhan = khieuNaiDungMotPhan + dataChiTietByCoQuan[l].DaBHGQDungMotPhan;

                            //tmplCoQuan = tmplCoQuan.replace(/ _dabanhanhKnSai_ / g, dataChiTietByCoQuan[l].DaBHGQSai);
                            //khieuNaiSai = khieuNaiSai + dataChiTietByCoQuan[l].DaBHGQSai;
                            //    $("#baocao > tbody.bdbaocao").append(tmplCoQuan);

                            //stt++;
                        }
                        //$("#TSLuotTiepDan" + dataCap[i].CapID + dataByCoQuan[k].CoQuanID).text(tsLuotTiepDan);
                        //$("#TSLuotXLDTrongHan" + dataCap[i].CapID + dataByCoQuan[k].CoQuanID).text(soDonTrongHan);
                        //$("#TSDonXLQuaHan" + dataCap[i].CapID + dataByCoQuan[k].CoQuanID).text(soDonQuaHan);
                        //$("#TSDonXuLy" + dataCap[i].CapID + dataByCoQuan[k].CoQuanID).text(tsLuotXLDon);
                        //$("#TSGiaoGiaiQuyet" + dataCap[i].CapID + dataByCoQuan[k].CoQuanID).text(tsGiaiQuyetDon);
                        //$("#TSChuaGiaiQuyet" + dataCap[i].CapID + dataByCoQuan[k].CoQuanID).text(tsChuaGiaiQuyetDon);
                        //$("#TSDangGiaiQuyetTH" + dataCap[i].CapID + dataByCoQuan[k].CoQuanID).text(soDangGiaiQuyetTH);
                        //$("#TSDangGiaiQuyetQH" + dataCap[i].CapID + dataByCoQuan[k].CoQuanID).text(soDangGiaiQuyetQH);
                        //$("#TSDangGiaiQuyet" + dataCap[i].CapID + dataByCoQuan[k].CoQuanID).text(tsDangGiaiQuyet);
                        //$("#DaCoBaoCao" + dataCap[i].CapID + dataByCoQuan[k].CoQuanID).text(daCoBaoCao);
                        //$("#KNDung" + dataCap[i].CapID + dataByCoQuan[k].CoQuanID).text(khieuNaiDung);
                        //$("#KNDungMotPhan" + dataCap[i].CapID + dataByCoQuan[k].CoQuanID).text(khieuNaiDungMotPhan);
                        //$("#KNSai" + dataCap[i].CapID + dataByCoQuan[k].CoQuanID).text(khieuNaiSai);

                        if (dataByCoQuan[k].LsByCoQuan != null && dataByCoQuan[k].LsByCoQuan.Count > 0)
                        {
                            foreach (var item in dataByCoQuan[k].LsByCoQuan)
                            {
                                TableData tableDataCoQuan = new TableData();
                                tableDataCoQuan.ID = stt++;
                                var DataArrCoQuan = new List<RowItem>();
                              
                                RowItem IRowItem1 = new RowItem(1, stt.ToString(), item.CoQuanID.ToString(), item.CapID.ToString(), null, item.Style, ref DataArrCoQuan);
                                RowItem IRowItem2 = new RowItem(2, item.TenCoQuan ?? item.TenCap, item.CoQuanID.ToString(), item.CapID.ToString(), null, "text-align: left;" + Css, ref DataArrCoQuan);
                                RowItem IRowItem3 = new RowItem(3, Utils.AddCommasDouble(item.SoDonTiepDan.ToString()), item.CoQuanID.ToString(), item.CapID.ToString(), null, "text-align: center;" + Css, ref DataArrCoQuan);
                                RowItem IRowItem4 = new RowItem(4, Utils.AddCommasDouble(item.SoXLTrongHan.ToString()), item.CoQuanID.ToString(), item.CapID.ToString(), null, "text-align: center;" + Css, ref DataArrCoQuan);
                                RowItem IRowItem5 = new RowItem(5, Utils.AddCommasDouble(item.SoXLQuaHan.ToString()), item.CoQuanID.ToString(), item.CapID.ToString(), null, "text-align: center;" + Css, ref DataArrCoQuan);
                                RowItem IRowItem6 = new RowItem(6, Utils.AddCommasDouble(item.SoXLDaXuLy.ToString()), item.CoQuanID.ToString(), item.CapID.ToString(), null, "text-align: center;" + Css, ref DataArrCoQuan);
                                RowItem IRowItem7 = new RowItem(7, Utils.AddCommasDouble(item.TongSoDonGiaoGQ.ToString()), item.CoQuanID.ToString(), item.CapID.ToString(), null, "text-align: center;" + Css, ref DataArrCoQuan);
                                RowItem IRowItem8 = new RowItem(8, Utils.AddCommasDouble(item.ChuaGiaiQuyet.ToString()), item.CoQuanID.ToString(), item.CapID.ToString(), null, "text-align: center;" + Css, ref DataArrCoQuan);
                                RowItem IRowItem9 = new RowItem(9, Utils.AddCommasDouble(item.SoDangGQTrongHan.ToString()), item.CoQuanID.ToString(), item.CapID.ToString(), null, "text-align: center;" + Css, ref DataArrCoQuan);
                                RowItem IRowItem10 = new RowItem(10, Utils.AddCommasDouble(item.SoDangGQQuaHan.ToString()), item.CoQuanID.ToString(), item.CapID.ToString(), null, "text-align: center;" + Css, ref DataArrCoQuan);
                                RowItem IRowItem11 = new RowItem(11, Utils.AddCommasDouble(item.TongSoDangGQ.ToString()), item.CoQuanID.ToString(), item.CapID.ToString(), null, "text-align: center;" + Css, ref DataArrCoQuan);
                                RowItem IRowItem12 = new RowItem(12, Utils.AddCommasDouble(item.DaCoBC.ToString()), item.CoQuanID.ToString(), item.CapID.ToString(), null, "text-align: center;" + Css, ref DataArrCoQuan);
                                RowItem IRowItem13 = new RowItem(13, Utils.AddCommasDouble(item.DaBHGQDung.ToString()), item.CoQuanID.ToString(), item.CapID.ToString(), null, "text-align: center;" + Css, ref DataArrCoQuan);
                                RowItem IRowItem14 = new RowItem(14, Utils.AddCommasDouble(item.DaBHGQDungMotPhan.ToString()), item.CoQuanID.ToString(), item.CapID.ToString(), null, "text-align: center;" + Css, ref DataArrCoQuan);
                                RowItem IRowItem15 = new RowItem(15, Utils.AddCommasDouble(item.DaBHGQSai.ToString()), item.CoQuanID.ToString(), item.CapID.ToString(), null, "text-align: center;" + Css, ref DataArrCoQuan);

                                tableDataCoQuan.DataArr = DataArrCoQuan;
                                data.Add(tableDataCoQuan);
                            }
                        }
                    }
                }
                else if(dro.LsByCoQuan != null)
                {
                    foreach (var item in dro.LsByCoQuan)
                    {
                        TableData tableDataCoQuan = new TableData();
                        tableDataCoQuan.ID = stt++;
                        var DataArrCoQuan = new List<RowItem>();
                        string Css = "";
                        RowItem IRowItem1 = new RowItem(1, stt.ToString(), item.CoQuanID.ToString(), item.CapID.ToString(), null, item.Style, ref DataArrCoQuan);
                        RowItem IRowItem2 = new RowItem(2, item.TenCoQuan ?? item.TenCap, item.CoQuanID.ToString(), item.CapID.ToString(), null, "text-align: left;" + Css, ref DataArrCoQuan);
                        RowItem IRowItem3 = new RowItem(3, Utils.AddCommasDouble(item.SoDonTiepDan.ToString()), item.CoQuanID.ToString(), item.CapID.ToString(), null, "text-align: center;" + Css, ref DataArrCoQuan);
                        RowItem IRowItem4 = new RowItem(4, Utils.AddCommasDouble(item.SoXLTrongHan.ToString()), item.CoQuanID.ToString(), item.CapID.ToString(), null, "text-align: center;" + Css, ref DataArrCoQuan);
                        RowItem IRowItem5 = new RowItem(5, Utils.AddCommasDouble(item.SoXLQuaHan.ToString()), item.CoQuanID.ToString(), item.CapID.ToString(), null, "text-align: center;" + Css, ref DataArrCoQuan);
                        RowItem IRowItem6 = new RowItem(6, Utils.AddCommasDouble(item.SoXLDaXuLy.ToString()), item.CoQuanID.ToString(), item.CapID.ToString(), null, "text-align: center;" + Css, ref DataArrCoQuan);
                        RowItem IRowItem7 = new RowItem(7, Utils.AddCommasDouble(item.TongSoDonGiaoGQ.ToString()), item.CoQuanID.ToString(), item.CapID.ToString(), null, "text-align: center;" + Css, ref DataArrCoQuan);
                        RowItem IRowItem8 = new RowItem(8, Utils.AddCommasDouble(item.ChuaGiaiQuyet.ToString()), item.CoQuanID.ToString(), item.CapID.ToString(), null, "text-align: center;" + Css, ref DataArrCoQuan);
                        RowItem IRowItem9 = new RowItem(9, Utils.AddCommasDouble(item.SoDangGQTrongHan.ToString()), item.CoQuanID.ToString(), item.CapID.ToString(), null, "text-align: center;" + Css, ref DataArrCoQuan);
                        RowItem IRowItem10 = new RowItem(10, Utils.AddCommasDouble(item.SoDangGQQuaHan.ToString()), item.CoQuanID.ToString(), item.CapID.ToString(), null, "text-align: center;" + Css, ref DataArrCoQuan);
                        RowItem IRowItem11 = new RowItem(11, Utils.AddCommasDouble(item.TongSoDangGQ.ToString()), item.CoQuanID.ToString(), item.CapID.ToString(), null, "text-align: center;" + Css, ref DataArrCoQuan);
                        RowItem IRowItem12 = new RowItem(12, Utils.AddCommasDouble(item.DaCoBC.ToString()), item.CoQuanID.ToString(), item.CapID.ToString(), null, "text-align: center;" + Css, ref DataArrCoQuan);
                        RowItem IRowItem13 = new RowItem(13, Utils.AddCommasDouble(item.DaBHGQDung.ToString()), item.CoQuanID.ToString(), item.CapID.ToString(), null, "text-align: center;" + Css, ref DataArrCoQuan);
                        RowItem IRowItem14 = new RowItem(14, Utils.AddCommasDouble(item.DaBHGQDungMotPhan.ToString()), item.CoQuanID.ToString(), item.CapID.ToString(), null, "text-align: center;" + Css, ref DataArrCoQuan);
                        RowItem IRowItem15 = new RowItem(15, Utils.AddCommasDouble(item.DaBHGQSai.ToString()), item.CoQuanID.ToString(), item.CapID.ToString(), null, "text-align: center;" + Css, ref DataArrCoQuan);

                        tableDataCoQuan.DataArr = DataArrCoQuan;
                        data.Add(tableDataCoQuan);
                    }
                }
            }

            return data;
        }

        public string TongHopTinhHinhTCD_XL_GQD_Excel(string rootPath, string pathFile, List<TableData> data, DateTime tuNgay, DateTime denNgay)
        {
            //var camera = new Camera;
            // path to your excel file
            string path = rootPath + @"\Templates\BaoCao\TongHopTinhHinhTCD_XL_GQD.xlsx";
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
                    worksheet.InsertRow(7, data.Count - 1, 6);
                    for (int i = 0; i < data.Count; i++)
                    {
                        if (data[i].DataArr != null && data[i].DataArr.Count > 0)
                        {
                            for (int j = 0; j < data[i].DataArr.Count; j++)
                            {
                                if (data[i].DataArr[j] != null && data[i].DataArr[j].Content != "0" && worksheet.Cells[i + 6, j + 1] != null)
                                {
                                    worksheet.Cells[i + 6, j + 1].Value = data[i].DataArr[j].Content;
                                    //if (data[i].DataArr[j].Style.Contains("bold")) worksheet.Cells[i + 6, j + 1].Style.Font.Bold = true;
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

        public List<DTXuLyInfo> TongHopTinhHinhTCD_XL_GQD_GetDSChiTietDonThu(DateTime tuNgays, DateTime denNgays, int coQuanID, int capID, int tinhID, int loaiXL, int start, int end)
        {
            IList<DTXuLyInfo> list = new List<DTXuLyInfo>();
            //DateTime tuNgays = Utils.ConvertToDateTime(tuNgay, DateTime.MinValue);
            //DateTime denNgays = Utils.ConvertToDateTime(denNgay, DateTime.MinValue);
            QueryFilterInfo infoQF = new QueryFilterInfo()
            {
                TuNgayGoc = tuNgays,
                TuNgayMoi = tuNgays,
                DenNgayGoc = denNgays.AddDays(1),
                DenNgayMoi = denNgays.AddDays(1),
                CoQuanID = coQuanID,
                CapID = capID,
                TinhID = tinhID,
                LoaiXL = loaiXL,
                PTKQDung = (int)PhanTichKQEnum.Dung,
                PTKQDungMotPhan = (int)PhanTichKQEnum.DungMotPhan,
                PTKQSai = (int)PhanTichKQEnum.Sai,
                Start = start,
                End = end
            };

            //TIEPDAN = "1";
            //XLD_DAXULY_TRONGHAN = "2";
            //XLD_DAXULY_QUAHAN = "3";
            //XLD_DAXULY_TONG = "4";
            //GQD_TONG = "5";
            //GQD_CHUAGQ = "13";
            //GQD_DANGGQ_TRONGHAN = "6";
            //GQD_DANGGQ_QUAHAN = "7";
            //GQD_DANGGQ_TONG = "8";
            //GQD_DAGQ = "9";
            //GQD_KQKN_DUNG = "10";
            //GQD_KQKN_DUNGMOTPHAN = "11";
            //GQD_KQKN_SAI = "12";
            try
            {
                if (loaiXL == 1)
                {
                    list = new BCTinhHinhTD_XLD_GQ().DSDonThu_TiepDan_GetByDate(infoQF);
                }
                else if (loaiXL == 2 || loaiXL == 3 || loaiXL == 4)
                {
                    list = new BCTinhHinhTD_XLD_GQ().DSDonThu_XuLyDon_GetByDate(infoQF);
                }
                else if (loaiXL == 5 || loaiXL == 6 || loaiXL == 7 || loaiXL == 8 || loaiXL == 9 || loaiXL == 10 || loaiXL == 11 || loaiXL == 12 || loaiXL == 13)
                {
                    list = new BCTinhHinhTD_XLD_GQ().DSDonThu_GiaiQuyetDon_GetByDate(infoQF);
                }
            }
            catch
            {

            }

            return list.ToList();
        }

        public List<TableData> ThongKeDonChuyenGiaiQuyet(IdentityHelper IdentityHelper, string ContentRootPath, DateTime startDate, DateTime endDate)
        {
            List<TableData> data = new List<TableData>();

            var baocaoList = new ChuyenGiaiQuyet().GetChuyenGQTongHop(IdentityHelper.CoQuanID ?? 0, startDate, endDate);
 
            int stt = 0;
            foreach (BaoCaoChuyenGQInfo dro in baocaoList)
            {
                TableData tableData = new TableData();
                tableData.ID = stt++;
                var DataArr = new List<RowItem>();

                string Css = "";

                RowItem RowItem1 = new RowItem(1, stt.ToString(), dro.CoQuanGiaiQuyetID.ToString(), null, null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem2 = new RowItem(2, dro.TenCoQuan, dro.CoQuanGiaiQuyetID.ToString(), null, null, "text-align: left;" + Css, ref DataArr);
                RowItem RowItem3 = new RowItem(3, dro.SoLuong.ToString(), dro.CoQuanGiaiQuyetID.ToString(), null, null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem4 = new RowItem(4, dro.SLDonChuaBaoCao.ToString(), dro.CoQuanGiaiQuyetID.ToString(), null, null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem5 = new RowItem(5, dro.SLDonDaBaoCao.ToString(), dro.CoQuanGiaiQuyetID.ToString(), null, null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem6 = new RowItem(6, dro.SLDaBanHanhQD.ToString(), dro.CoQuanGiaiQuyetID.ToString(), null, null, "text-align: center;" + Css, ref DataArr);
               
                tableData.DataArr = DataArr;
                data.Add(tableData);
            }

            int SoLuong = baocaoList.Sum(x => x.SoLuong);
            int SLDonChuaBaoCao = baocaoList.Sum(x => x.SLDonChuaBaoCao);
            int SLDonDaBaoCao = baocaoList.Sum(x => x.SLDonDaBaoCao);
            int SLDaBanHanhQD = baocaoList.Sum(x => x.SLDaBanHanhQD);

            TableData rowTong = new TableData();
            rowTong.ID = stt++;
            var DataArrTong = new List<RowItem>();
            RowItem RowItemTong1 = new RowItem(1, "", "0", null, null, "text-align: center;", ref DataArrTong);
            RowItem RowItemTong2 = new RowItem(2, "Tổng cộng", "0", null, null, "text-align: left; font-weight:bold;", ref DataArrTong);
            RowItem RowItemTong3 = new RowItem(3, SoLuong.ToString(), "0", null, null, "text-align: center;font-weight:bold;", ref DataArrTong);
            RowItem RowItemTong4 = new RowItem(4, SLDonChuaBaoCao.ToString(), "0", null, null, "text-align: center;font-weight:bold;", ref DataArrTong);
            RowItem RowItemTong5 = new RowItem(5, SLDonDaBaoCao.ToString(), "0", null, null, "text-align: center;font-weight:bold;", ref DataArrTong);
            RowItem RowItemTong6 = new RowItem(6, SLDaBanHanhQD.ToString(), "0", null, null, "text-align: center;font-weight:bold;", ref DataArrTong);
            
            rowTong.DataArr = DataArrTong;
            data.Add(rowTong);

            return data;
        }

        public string ThongKeDonChuyenGiaiQuyet_Excel(string rootPath, string pathFile, List<TableData> data, DateTime tuNgay, DateTime denNgay)
        {
            //var camera = new Camera;
            // path to your excel file
            string path = rootPath + @"\Templates\BaoCao\ThongKeDonChuyenGiaiQuyet.xlsx";
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
                    worksheet.InsertRow(6, data.Count - 1, 5);
                    //worksheet.DeleteRow(data.Count);
                    for (int i = 0; i < data.Count; i++)
                    {
                        if (data[i].DataArr != null && data[i].DataArr.Count > 0)
                        {
                            for (int j = 0; j < data[i].DataArr.Count; j++)
                            {
                                if (data[i].DataArr[j].Content != "0")
                                {
                                    worksheet.Cells[i + 5, j + 1].Value = data[i].DataArr[j].Content;
                                    if (data[i].DataArr[j].Style.Contains("bold")) worksheet.Cells[i + 5, j + 1].Style.Font.Bold = true;
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

        public List<TableData> BaoCaoXuLyCongViec(IdentityHelper IdentityHelper, int LoaiKhieuToID, string ContentRootPath, DateTime startDate, DateTime endDate)
        {
            List<TableData> data = new List<TableData>();

            if (startDate == DateTime.Now.Date && endDate == DateTime.Now.Date)
            {
                endDate = endDate.AddDays(1);
            }
            BCTongHopXuLyInfo tongInfo = new BCTongHopXuLyInfo();
            int loaiDon = LoaiKhieuToID;
            string testTime = "";
          
            var baocaoList = BCTongHopXuLyCalc.CalculateBCTongHopKQTDXLD(startDate, endDate, IdentityHelper.CoQuanID ?? 0, IdentityHelper.PhongBanID ?? 0, ref tongInfo, loaiDon, ref testTime);
 
            int stt = 1;
            foreach (BCTongHopXuLyInfo dro in baocaoList)
            {
                TableData tableData = new TableData();
                tableData.ID = stt++;
                var DataArr = new List<RowItem>();

                string Css = "";
             
                RowItem RowItem1 = new RowItem(1, dro.STT, dro.PhongBanID.ToString(), dro.CanBoID.ToString(), LoaiKhieuToID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem2 = new RowItem(2, dro.TenCanBo, dro.PhongBanID.ToString(), dro.CanBoID.ToString(), LoaiKhieuToID.ToString(), null, "text-align: left;" + Css, ref DataArr);
                RowItem RowItem3 = new RowItem(3, dro.SLTiepCongDanStr, dro.PhongBanID.ToString(), dro.CanBoID.ToString(), LoaiKhieuToID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem4 = new RowItem(4, dro.XLDTongSoStr, dro.PhongBanID.ToString(), dro.CanBoID.ToString(), LoaiKhieuToID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem5 = new RowItem(5, dro.XLDChuaXuLyStr, dro.PhongBanID.ToString(), dro.CanBoID.ToString(), LoaiKhieuToID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem6 = new RowItem(6, dro.XLDDaXuLyTrongHanStr, dro.PhongBanID.ToString(), dro.CanBoID.ToString(), LoaiKhieuToID.ToString(), null, "text-align: center;" + Css, ref DataArr);
                RowItem RowItem7 = new RowItem(7, dro.XLDDaXuLyQuaHanStr, dro.PhongBanID.ToString(), dro.CanBoID.ToString(), LoaiKhieuToID.ToString(), null, "text-align: center;" + Css, ref DataArr);

                tableData.DataArr = DataArr;
                data.Add(tableData);
            }

            int SLTiepCongDan = baocaoList.Sum(x => x.SLTiepCongDan);
            int XLDTongSo = baocaoList.Sum(x => x.XLDTongSo);
            int XLDChuaXuLy = baocaoList.Sum(x => x.XLDChuaXuLy);
            int XLDDaXuLyTrongHan = baocaoList.Sum(x => x.XLDDaXuLyTrongHan);
            int XLDDaXuLyQuaHan = baocaoList.Sum(x => x.XLDDaXuLyQuaHan);
        
            TableData rowTong = new TableData();
            rowTong.ID = stt++;
            var DataArrTong = new List<RowItem>();
            RowItem RowItemTong1 = new RowItem(1, "", null, null, LoaiKhieuToID.ToString(), null, "text-align: center;", ref DataArrTong);
            RowItem RowItemTong2 = new RowItem(2, "Tổng cộng", null, null, LoaiKhieuToID.ToString(), null, "text-align: left;", ref DataArrTong);
            RowItem RowItemTong3 = new RowItem(3, SLTiepCongDan.ToString(), null, null, LoaiKhieuToID.ToString(), null, "text-align: center;", ref DataArrTong);
            RowItem RowItemTong4 = new RowItem(4, XLDTongSo.ToString(), null, null, LoaiKhieuToID.ToString(), null, "text-align: center;", ref DataArrTong);
            RowItem RowItemTong5 = new RowItem(5, XLDChuaXuLy.ToString(), null, null, LoaiKhieuToID.ToString(), null, "text-align: center;", ref DataArrTong);
            RowItem RowItemTong6 = new RowItem(6, XLDDaXuLyTrongHan.ToString(), null, null, LoaiKhieuToID.ToString(), null, "text-align: center;", ref DataArrTong);
            RowItem RowItemTong7 = new RowItem(7, XLDDaXuLyQuaHan.ToString(), null, null, LoaiKhieuToID.ToString(), null, "text-align: center;", ref DataArrTong);
            rowTong.DataArr = DataArrTong;
            data.Add(rowTong);

            return data;
        }

        public string BaoCaoXuLyCongViec_Excel(string rootPath, string pathFile, List<TableData> data, DateTime tuNgay, DateTime denNgay)
        {
            //var camera = new Camera;
            // path to your excel file
            string path = rootPath + @"\Templates\BaoCao\BaoCaoXuLyCongViec.xlsx";
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
                    worksheet.InsertRow(7, data.Count - 1, 6);
                    //worksheet.DeleteRow(data.Count);
                    for (int i = 0; i < data.Count; i++)
                    {
                        if (data[i].DataArr != null && data[i].DataArr.Count > 0)
                        {
                            for (int j = 0; j < data[i].DataArr.Count; j++)
                            {
                                if (data[i].DataArr[j].Content != "0")
                                {
                                    worksheet.Cells[i + 6, j + 1].Value = data[i].DataArr[j].Content;
                                    if (data[i].DataArr[j].Style.Contains("bold")) worksheet.Cells[i + 6, j + 1].Style.Font.Bold = true;
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

        public List<ChiTietVuViecDongNguoiInfo> ThongKeTheoVuViecDongNguoi_GetDSChiTietDonThu(BaseReportParams p, IdentityHelper IdentityHelper, int start, int end, string ContentRootPath, DateTime startDate, DateTime endDate)
        {
            //int start = ;
            int LanCT = p.PageNumber;
            int SoLuongCT = p.PageSize;
            int Type = 0;
            if(p.Index == 2)
            {
                Type = 1;
            }
            else Type = 2;
            //if (LanCT == 1)
            //{
            //    //SoLuongCT = SoLuongCT;
            //}
            //else
            //{
            //    SoLuongCT = 40;
            //}
            int coQuanIDSelect = p.CoQuanID ?? 0;
            int capIDSends = p.CapID ?? 0;
            DateTime startDates = Utils.ConvertToDateTime(startDate, DateTime.Now);
            DateTime endDates = Utils.ConvertToDateTime(endDate, DateTime.Now);
            List<ChiTietVuViecDongNguoiInfo> resultList = new List<ChiTietVuViecDongNguoiInfo>();
            int capID = IdentityHelper.CapID ?? 0;
            int tinhID = IdentityHelper.TinhID ?? 0;
            string data = "";
            var CanBoID = IdentityHelper.CanBoID ?? 0;
            Boolean laThanhTraTinh = false;
            var listThanhTraTinh = new SystemConfigDAL().GetByKey("Thanh_Tra_Tinh_ID").ConfigValue.Split(',').ToList().Select(x => Utils.ConvertToInt32(x.ToString(), 0)).ToList();
            if (listThanhTraTinh.Contains(IdentityHelper.CoQuanID ?? 0) && IdentityHelper.RoleID == EnumChucVu.LanhDao.GetHashCode())
            {
                laThanhTraTinh = true;
            }
            try
            {
                if (capID == (int)CapQuanLy.CapUBNDTinh || laThanhTraTinh)
                {
                    List<int> capList = new List<int>();
                    string[] capIDs = p.ListCapIDStr.Split(',');
                    for (int i = 0; i < capIDs.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(capIDs[i]))
                        {
                          
                            capList.Add(int.Parse(capIDs[i].ToString()));

                        }
                    }
                    if (capList.Contains(CapQuanLy.CapUBNDHuyen.GetHashCode()))
                    {
                        capList.Add(CapQuanLy.CapPhong.GetHashCode());
                    }
                    if (p.ListCapIDStr == "")
                    {
                        capList.Add(CapQuanLy.CapUBNDTinh.GetHashCode());
                    }
                    //phucth
                    if (capIDSends == 0 && coQuanIDSelect == 0)
                    {
                        Boolean laCapUBNDTinh = false;
                        Boolean laCapUBNDHuyen = false;
                        foreach (var item in capList)
                        {
                            if (item == (int)CapQuanLy.CapUBNDTinh)
                            {
                                laCapUBNDTinh = true;
                            }
                            if (item == (int)CapQuanLy.CapUBNDHuyen)
                            {
                                laCapUBNDHuyen = true;
                            }
                        }
                        List<CoQuanInfo> ListCoQuan = new List<CoQuanInfo>();
                        if (!laCapUBNDTinh && laCapUBNDHuyen)
                        {//truong hop chi chon CapUBNDHuyen
                            List<CoQuanInfo> cqListCapPhong = new CoQuan().GetCoQuanByCapAndTinhID((int)CapQuanLy.CapPhong, tinhID).ToList();
                            List<CoQuanInfo> cqListCapUBHuyen = new CoQuan().GetCoQuanByCapAndTinhID((int)CapQuanLy.CapUBNDHuyen, tinhID).ToList();
                            List<CoQuanInfo> cqListCapHuyen = new List<CoQuanInfo>();
                            foreach (CoQuanInfo itemCoQuan in cqListCapPhong)
                            {
                                cqListCapHuyen.Add(itemCoQuan);
                            }
                            foreach (CoQuanInfo itemCoQuan in cqListCapUBHuyen)
                            {
                                cqListCapHuyen.Add(itemCoQuan);
                            }
                            ListCoQuan = cqListCapHuyen;
                        }
                        else
                        {
                            ListCoQuan = new CoQuan().GetCoQuanByTinhID(tinhID).ToList().Where(x => capList.Contains(x.CapID)).ToList();
                        }

                        resultList = new BaoCaoVuViecDongNguoi_DAL().GETCHITIET_DONTHUVUVIECDONGNGUOI(startDates, endDates, start, CanBoID, ListCoQuan, SoLuongCT, Type).ToList();
                    }
                    else if (capIDSends != 2 && capIDSends != 0 && coQuanIDSelect == 0)
                    {
                        var ListCoQuan = new CoQuan().GetCoQuanByTinhID(tinhID).ToList().Where(x => x.CapID == capIDSends).ToList();
                        resultList = new BaoCaoVuViecDongNguoi_DAL().GETCHITIET_DONTHUVUVIECDONGNGUOI(startDates, endDates, start, CanBoID, ListCoQuan, SoLuongCT, Type).ToList(); ;
                    }
                    else if (capIDSends == 2 && coQuanIDSelect == 0)
                    {
                        List<CoQuanInfo> cqListCapPhong = new CoQuan().GetCoQuanByCapAndTinhID((int)CapQuanLy.CapPhong, tinhID).ToList();
                        List<CoQuanInfo> cqListCapUBHuyen = new CoQuan().GetCoQuanByCapAndTinhID((int)CapQuanLy.CapUBNDHuyen, tinhID).ToList();
                        List<CoQuanInfo> cqListCapHuyen = new List<CoQuanInfo>();
                        foreach (CoQuanInfo itemCoQuan in cqListCapPhong)
                        {
                            cqListCapHuyen.Add(itemCoQuan);
                        }
                        foreach (CoQuanInfo itemCoQuan in cqListCapUBHuyen)
                        {
                            cqListCapHuyen.Add(itemCoQuan);
                        }
                        var ListCoQuan = cqListCapHuyen;
                        //var ListCoQuan = new CoQuan().GetCoQuanByTinhID(tinhID).ToList().Where(x => x.CapID == capIDSends).ToList();
                        resultList = new BaoCaoVuViecDongNguoi_DAL().GETCHITIET_DONTHUVUVIECDONGNGUOI(startDates, endDates, start, CanBoID, ListCoQuan, SoLuongCT, Type).ToList(); ;
                    }
                    else if (capIDSends == 2 && coQuanIDSelect != 0)
                    {
                        List<CoQuanInfo> cqListCapPhong = new CoQuan().GetCoQuanByCapAndTinhID((int)CapQuanLy.CapPhong, tinhID).ToList();
                        List<CoQuanInfo> cqListCapUBHuyen = new CoQuan().GetCoQuanByCapAndTinhID((int)CapQuanLy.CapUBNDHuyen, tinhID).ToList();
                        List<CoQuanInfo> cqListCapHuyen = new List<CoQuanInfo>();
                        foreach (CoQuanInfo itemCoQuan in cqListCapPhong)
                        {
                            cqListCapHuyen.Add(itemCoQuan);
                        }
                        foreach (CoQuanInfo itemCoQuan in cqListCapUBHuyen)
                        {
                            cqListCapHuyen.Add(itemCoQuan);
                        }
                        var ListCoQuan = cqListCapHuyen.Where(x => x.HuyenID == coQuanIDSelect).ToList();
                        //var ListCoQuan = new List<CoQuanInfo> { new CoQuan().GetCoQuanByID(coQuanIDSelect) };
                        resultList = new BaoCaoVuViecDongNguoi_DAL().GETCHITIET_DONTHUVUVIECDONGNGUOI(startDates, endDates, start, CanBoID, ListCoQuan, SoLuongCT, Type).ToList(); ;
                    }
                    else if (capIDSends != 0 && capIDSends != 2 && coQuanIDSelect != 0)
                    {
                        var ListCoQuan = new List<CoQuanInfo> { new CoQuan().GetCoQuanByID(coQuanIDSelect) };
                        resultList = new BaoCaoVuViecDongNguoi_DAL().GETCHITIET_DONTHUVUVIECDONGNGUOI(startDates, endDates, start, CanBoID, ListCoQuan, SoLuongCT, Type).ToList(); ;
                    }
                }
             
                else if (capID == (int)CapQuanLy.CapUBNDHuyen)
                {
                    if (capIDSends == 0 && coQuanIDSelect == 0)
                    {
                        var ListCoQuan = new CoQuan().GetCoQuanByTinhID(tinhID).ToList().Where(x => x.HuyenID == IdentityHelper.HuyenID).ToList();
                        resultList = new BaoCaoVuViecDongNguoi_DAL().GETCHITIET_DONTHUVUVIECDONGNGUOI(startDates, endDates, start, CanBoID, ListCoQuan, SoLuongCT, Type).ToList();
                    }
                    else if (capIDSends != 0 && coQuanIDSelect == 0)
                    {
                        List<CoQuanInfo> cqListCapPhong = new CoQuan().GetCoQuanByCapAndTinhID((int)CapQuanLy.CapPhong, tinhID).ToList();
                        List<CoQuanInfo> cqListCapUBHuyen = new CoQuan().GetCoQuanByCapAndTinhID((int)CapQuanLy.CapUBNDHuyen, tinhID).ToList();
                        List<CoQuanInfo> cqListCapHuyen = new List<CoQuanInfo>();
                        foreach (CoQuanInfo itemCoQuan in cqListCapPhong)
                        {
                            cqListCapHuyen.Add(itemCoQuan);
                        }
                        foreach (CoQuanInfo itemCoQuan in cqListCapUBHuyen)
                        {
                            cqListCapHuyen.Add(itemCoQuan);
                        }
                        var ListCoQuan = cqListCapHuyen.Where(x => x.HuyenID == IdentityHelper.HuyenID).ToList();
                        //var ListCoQuan = new CoQuan().GetCoQuanByTinhID(tinhID).ToList().Where(x => x.HuyenID == IdentityHelper.GetHuyenID() && x.CapID == capIDSends).ToList();
                        resultList = new BaoCaoVuViecDongNguoi_DAL().GETCHITIET_DONTHUVUVIECDONGNGUOI(startDates, endDates, start, CanBoID, ListCoQuan, SoLuongCT, Type).ToList(); ;
                    }
                    else if (capIDSends != 0 && coQuanIDSelect != 0)
                    {
                        var ListCoQuan = new List<CoQuanInfo> { new CoQuan().GetCoQuanByID(coQuanIDSelect) };
                        resultList = new BaoCaoVuViecDongNguoi_DAL().GETCHITIET_DONTHUVUVIECDONGNGUOI(startDates, endDates, start, CanBoID, ListCoQuan, SoLuongCT, Type).ToList(); ;
                    }
                }
                else if (capID == (int)CapQuanLy.CapSoNganh || capID == (int)CapQuanLy.CapPhong)
                {
                    var ListCoQuan = new List<CoQuanInfo> { new CoQuan().GetCoQuanByID(coQuanIDSelect) };
                    resultList = new BaoCaoVuViecDongNguoi_DAL().GETCHITIET_DONTHUVUVIECDONGNGUOI(startDates, endDates, start, CanBoID, ListCoQuan, SoLuongCT, Type).ToList();
                }
                else if (capID == (int)CapQuanLy.CapUBNDXa)
                {
                    var ListCoQuan = new List<CoQuanInfo> { new CoQuan().GetCoQuanByID(coQuanIDSelect) };
                    resultList = new BaoCaoVuViecDongNguoi_DAL().GETCHITIET_DONTHUVUVIECDONGNGUOI(startDates, endDates, start, CanBoID, ListCoQuan, SoLuongCT, Type).ToList();
                }

            }
            catch
            {
            }
            var resultListCT = new List<ChiTietVuViecDongNguoiInfo>();
            if (LanCT == 1)
            {
                int stt = 0;
                resultList = (from t in resultList
                              orderby t.NgayNhapDon
                              select
                              new ChiTietVuViecDongNguoiInfo
                              {
                                  NgayNhapDon = t.NgayNhapDon,
                                  CoQuanID = t.CoQuanID,
                                  DiaChi = t.DiaChi,
                                  SoDon = t.SoDon,
                                  NoiDungDon = t.NoiDungDon,
                                  NgayNhapDonStr = t.NgayNhapDonStr,
                                  TenLoaiKhieuTo = t.TenLoaiKhieuTo,
                                  TenChuDon = t.TenChuDon,
                                  SoLuong = t.SoLuong,
                                  XuLyDonID = t.XuLyDonID,
                                  DonThuID = t.DonThuID,
                                  NhomKNID = t.NhomKNID,
                                  HuongXuLyID = t.HuongXuLyID,
                                  KetQuaID_Str = t.KetQuaID_Str,
                                  TenHuongGiaiQuyet = t.TenHuongGiaiQuyet,
                                  STT = stt++

                              }).ToList();
                resultListCT = resultList.Where(x => x.STT >= start && x.STT <= (start + 40)).ToList();
            }
            else
            {

            }


            return resultListCT;
        }

    }
}
