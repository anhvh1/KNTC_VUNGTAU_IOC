using Com.Gosol.KNTC.Ultilities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Com.Gosol.KNTC.Models.BaoCao;
using DataTable = System.Data.DataTable;
using Com.Gosol.KNTC.DAL.HeThong;
using System.Xml;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.DAL.KNTC;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using Com.Gosol.KNTC.DAL.DanhMuc;
using Com.Gosol.KNTC.Models.DanhMuc;

namespace Com.Gosol.KNTC.DAL.BaoCao
{
    public class BaoCao2bDAL
    {
        private const string PARM_STARTDATE = "@StartDate";
        private const string PARM_ENDDATE = "@EndDate";
        private const string PARM_COQUANID = "@CoQuanID";
        private const string PARM_TINHID = @"TinhID";
        private BaoCao2bDonThuInfo GetData(SqlDataReader rdr)
        {
            BaoCao2bDonThuInfo info = new BaoCao2bDonThuInfo();
            info.CoQuanID = Utils.ConvertToInt32(rdr["CoQuanID"], 0);
            info.DonThuID = Utils.ConvertToInt32(rdr["DonThuID"], 0);
            info.HuongGiaiQuyetID = Utils.ConvertToInt32(rdr["HuongGiaiQuyetID"], 0);
            info.LoaiKetQuaID = Utils.ConvertToInt32(rdr["LoaiKetQuaID"], 0);
            info.LoaiKhieuTo1ID = Utils.ConvertToInt32(rdr["LoaiKhieuTo1ID"], 0);
            info.LoaiKhieuTo2ID = Utils.ConvertToInt32(rdr["LoaiKhieuTo2ID"], 0);
            info.LoaiKhieuTo3ID = Utils.ConvertToInt32(rdr["LoaiKhieuTo3ID"], 0);
            info.LoaiKhieuToID = Utils.ConvertToInt32(rdr["LoaiKhieuToID"], 0);
            info.NgayNhapDon = Utils.ConvertToDateTime(rdr["NgayNhapDon"], DateTime.MinValue);
            info.KetQuaID = Utils.ConvertToInt32(rdr["KetQuaID"], 0);
            info.TrangThaiDonID = Utils.ConvertToInt32(rdr["TrangThaiDonID"], 0);
            info.NhomKNID = Utils.ConvertToInt32(rdr["NhomKNID"], 0);
            info.SoLuong = Utils.ConvertToInt32(rdr["SoLuong"], 0);
            info.SoLan = Utils.ConvertToInt32(rdr["SoLan"], 0);
            info.ThamQuyenCQXuLy = Utils.ConvertToInt32(rdr["ThamQuyenCQXuLy"], 0);
            info.ThamQuyenCQChuyenDon = Utils.ConvertToInt32(rdr["ThamQuyenCQChuyenDon"], 0);

            info.NgayRaKQ = Utils.ConvertToDateTime(rdr["NgayRaKQ"], DateTime.MinValue);
            info.NgayThuLy = Utils.ConvertToDateTime(rdr["NgayRaKQ"], DateTime.MinValue);
            //info.CQTiepNhanID = Utils.ConvertToInt32(rdr["CQTiepNhanID"], 0);

            return info;
        }
        private KeKhaiDuLieuDauKy_2bInfo GetData2a(SqlDataReader rdr)
        {
            KeKhaiDuLieuDauKy_2bInfo info = new KeKhaiDuLieuDauKy_2bInfo();
            info.Col1 = Utils.ConvertToInt32(rdr["Col1"], 0);
            info.Col2 = Utils.ConvertToInt32(rdr["Col2"], 0);
            info.Col3 = Utils.ConvertToInt32(rdr["Col3"], 0);
            info.Col4 = Utils.ConvertToInt32(rdr["Col4"], 0);
            info.Col5 = Utils.ConvertToInt32(rdr["Col5"], 0);
            info.Col6 = Utils.ConvertToInt32(rdr["Col6"], 0);
            info.Col7 = Utils.ConvertToInt32(rdr["Col7"], 0);
            info.Col8 = Utils.ConvertToInt32(rdr["Col8"], 0);
            info.Col9 = Utils.ConvertToInt32(rdr["Col9"], 0);
            info.Col10 = Utils.ConvertToInt32(rdr["Col10"], 0);
            info.Col11 = Utils.ConvertToInt32(rdr["Col11"], 0);
            info.Col12 = Utils.ConvertToInt32(rdr["Col12"], 0);
            info.Col13 = Utils.ConvertToInt32(rdr["Col13"], 0);
            info.Col14 = Utils.ConvertToInt32(rdr["Col14"], 0);
            info.Col15 = Utils.ConvertToInt32(rdr["Col15"], 0);
            info.Col16 = Utils.ConvertToInt32(rdr["Col16"], 0);
            info.Col17 = Utils.ConvertToInt32(rdr["Col17"], 0);
            info.Col18 = Utils.ConvertToInt32(rdr["Col18"], 0);
            info.Col19 = Utils.ConvertToInt32(rdr["Col19"], 0);
            info.Col20 = Utils.ConvertToInt32(rdr["Col20"], 0);
            info.Col21 = Utils.ConvertToInt32(rdr["Col21"], 0);
            info.Col22 = Utils.ConvertToInt32(rdr["Col22"], 0);
            info.Col23 = Utils.ConvertToInt32(rdr["Col23"], 0);
            info.Col24 = Utils.ConvertToInt32(rdr["Col24"], 0);
            info.Col25 = Utils.ConvertToInt32(rdr["Col25"], 0);
            info.Col26 = Utils.ConvertToInt32(rdr["Col26"], 0);
            info.Col27 = Utils.ConvertToInt32(rdr["Col27"], 0);
            info.Col28 = Utils.ConvertToInt32(rdr["Col28"], 0);
            info.Col29 = Utils.ConvertToInt32(rdr["Col29"], 0);
            info.Col30 = Utils.ConvertToInt32(rdr["Col30"], 0);
            info.Col31 = Utils.ConvertToInt32(rdr["Col31"], 0);
            info.CoQuanID = Utils.ConvertToInt32(rdr["CoQuanID"], 0);
            info.NgaySuDung = Utils.ConvertToDateTime(rdr["NgaySuDung"], DateTime.MinValue);
            return info;
        }
        public List<ThongKeBC_2b_DongBo_IOC> BC2b(List<int> listCapChonBaoCao, string ContentRootPath, int RoleID, int CapID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, int HuyenDangNhapID, DateTime startDate, DateTime endDate)
        {
            List<ThongKeBC_2b_DongBo_IOC> data2 = new List<ThongKeBC_2b_DongBo_IOC>();
            List<TableData> data = new List<TableData>();
            DataTable dt = BaoCao2b_DataTable();
            List<BaoCao2bInfo> resultList = new List<BaoCao2bInfo>();

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

                CalculateBaoCaoCapTrungUong(ContentRootPath, RoleID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, ref resultList, startDate, endDate, tinhID);
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
                double col1 = Utils.ConvertToDouble(dro["Col1Data"], 0);
                double col7 = Utils.ConvertToDouble(dro["Col7Data"], 0);
                double col30 = Utils.ConvertToDouble(dro["Col30Data"], 0);
                double col31 = Utils.ConvertToDouble(dro["Col31Data"], 0);
                double col14 = Utils.ConvertToDouble(dro["Col14Data"], 0);
                double col26 = Utils.ConvertToDouble(dro["Col26Data"], 0);
                
                double txldKhongThuocThamQuyen = col1 - col30 -col31;
                //double col2 = col5 + col14 + col23;
                //double col3 = col6 + col7 + col15 + col16 + col24 + col25;
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
                RowItem RowItem2 = new RowItem(2 + tmp, Utils.AddCommasDouble(dro["Col1Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), isEdit, "text-align: right;" + Css, ref DataArr);
                //RowItem RowItem2 = new RowItem(2, dro["Col1Data"].ToString(), dro["CoQuanID"].ToString(), dro["CapID"].ToString(),null, "", ref DataArr);
                RowItem RowItem3 = new RowItem(3 + tmp, Utils.AddCommasDouble(dro["Col2Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), isEdit, "text-align: right;" + Css, ref DataArr);
                //RowItem RowItem3 = new RowItem(3, dro["Col2Data"].ToString(), dro["CoQuanID"].ToString(), dro["CapID"].ToString(),null, "", ref DataArr);
                RowItem RowItem4 = new RowItem(4 + tmp, Utils.AddCommasDouble(dro["Col3Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), isEdit, "text-align: right;" + Css, ref DataArr);
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
                RowItem RowItem31 = new RowItem(31 + tmp, Utils.AddCommasDouble(dro["Col30Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), isEdit, "text-align: right;" + Css, ref DataArr);
                RowItem RowItem32 = new RowItem(32 + tmp, Utils.AddCommasDouble(dro["Col31Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), isEdit, "text-align: right;" + Css, ref DataArr);
                RowItem RowItem33 = new RowItem(33 + tmp, Utils.AddCommasDouble(dro["Col32Data"].ToString()), dro["CoQuanID"].ToString(), dro["CapID"].ToString(), isEdit, "text-align: right;" + Css, ref DataArr);
                if (Convert.ToInt32(RowItem1.CoQuanID) > 0)
                {
                    var model = new ThongKeBC_2b_DongBo_IOC();
                    model.CoQuanId = Utils.ConvertToInt32(RowItem1.CoQuanID, 0);
                    model.TenCoQuan = Utils.ConvertToString(RowItem1.Content, string.Empty);
                    model.TongSoDonXLD = Utils.ConvertToInt32(col1, 0);
                    model.TSDXLDThuocThamQuyen = Utils.ConvertToInt32(col1, 0);
                    model.TSDXLDKhongThuocThamQuyen = Utils.ConvertToInt32(txldKhongThuocThamQuyen,0);
                    model.TSDXLDToCao = Utils.ConvertToInt32(col14, 0);
                    model.TSDXLDToCaoThuocThamQuyen = Utils.ConvertToInt32(col31, 0);
                    model.TSDXLDKhieuNai = Utils.ConvertToInt32(col7, 0);
                    model.TSDXLDKhieuNaiThuocThamQuyen = Utils.ConvertToInt32(col30, 0);
                    model.TSDXLDKienNghiPhanAnh = Utils.ConvertToInt32(col26, 0);
                    
                    data2.Add(model);
                }

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

            return data2;
        }
        public static void CalculateBaoCaoCapTinh(string ContentRootPath, int RoleID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, ref List<BaoCao2bInfo> resultList, DateTime startDate, DateTime endDate, List<CapInfo> capList, bool flag, int tinhID, ref DataTable dt)
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
                        using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "BaoCao2b_GetDonThu_1606", parm))
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


            #region Cong so lieu tong

            #endregion   /// Caa   /// Cần sửa
            //}

            if (flag == true)
            {

                DataRow dr = dt.NewRow();
                dr["TenCoQuan"] = "<b style='text-transform: uppercase'>Toàn tỉnh</b>";
                dr["CssClass"] = "highlight";
                dr["CapID"] = CapCoQuanViewChiTiet.ToanTinh.GetHashCode();
                dr["ThuTu"] = 5;
                dr["CoQuanID"] = 0;
                foreach (DataRow dro in dt.Rows)
                {
                    for (var i = 1; i <= 32; i++)
                    {
                        if (dro["Col" + i + "Data"] == null)
                        {
                            dro["Col" + i + "Data"] = 0;

                        }
                        dr["Col" + i + "Data"] = Utils.ConvertToInt32(dr["Col" + i + "Data"], 0) +
                            Utils.ConvertToInt32(dro["Col" + i + "Data"], 0);
                    }
                }

                dt.Rows.Add(dr);
            }
            foreach (CapInfo capInfo in capList)
            {
                if (capInfo.CapID == (int)CapQuanLy.CapUBNDTinh)
                {
                    DataRow dr = dt.NewRow();
                    dr["TenCoQuan"] = "<b style='text-transform: uppercase'>" + "UBND Cấp Tỉnh" + "</b>";
                    dr["CssClass"] = "highlight";
                    dr["CapID"] = CapCoQuanViewChiTiet.CapUBNDTinh.GetHashCode();
                    dr["ThuTu"] = 4.5;
                    dr["CoQuanID"] = 0;
                    //DataTable dtnew = BaoCao2b_DataTable_New();
                    foreach (DataRow dro in dt.Rows)
                    {
                        if (Utils.ConvertToInt32(dro["CapID"], 0) == (int)CapQuanLy.CapUBNDTinh)
                        {
                            for (var i = 1; i <= 32; i++)
                            {
                                if (dro["Col" + i + "Data"] == null)
                                {
                                    dro["Col" + i + "Data"] = 0;

                                }
                                dr["Col" + i + "Data"] = Utils.ConvertToInt32(dr["Col" + i + "Data"], 0) +
                                    Utils.ConvertToInt32(dro["Col" + i + "Data"], 0);
                            }

                        }
                    }

                    dt.Rows.Add(dr);
                }
                if (capInfo.CapID == (int)CapQuanLy.ToanHuyen)
                {
                    DataRow dr = dt.NewRow();
                    dr["TenCoQuan"] = "<b style='text-transform: uppercase'>" + "Toàn huyện" + "</b>";
                    dr["CssClass"] = "highlight";
                    dr["CapID"] = CapCoQuanViewChiTiet.ToanHuyen.GetHashCode();
                    dr["ThuTu"] = 2.75;
                    dr["CoQuanID"] = 0;
                    //DataTable dtnew = BaoCao2b_DataTable_New();
                    foreach (DataRow dro in dt.Rows)
                    {
                        if (/*/*Utils.ConvertToInt32(dro["CapID"], 0) == (int)CapQuanLy.CapUBNDTinh ||*/ /*Utils.ConvertToInt32(dro["CapID"], 0) == (int)CapQuanLy.CapSoNganh*/
                           /*|*/ Utils.ConvertToInt32(dro["CapID"], 0) == (int)CapQuanLy.CapUBNDHuyen || Utils.ConvertToInt32(dro["CapID"], 0) == (int)CapQuanLy.CapUBNDXa
                            || Utils.ConvertToInt32(dro["CapID"], 0) == (int)CapQuanLy.ToanHuyen)
                        {
                            for (var i = 1; i <= 32; i++)
                            {

                                if (dro["Col" + i + "Data"] == null)
                                {
                                    dro["Col" + i + "Data"] = 0;

                                }
                                dr["Col" + i + "Data"] = Utils.ConvertToInt32(dr["Col" + i + "Data"], 0) +
                                    Utils.ConvertToInt32(dro["Col" + i + "Data"], 0);
                            }
                        }
                    }

                    dt.Rows.Add(dr);
                }
                else if (capInfo.CapID == (int)CapQuanLy.CapSoNganh)
                {
                    DataRow dr = dt.NewRow();
                    dr["TenCoQuan"] = "<b style='text-transform: uppercase'>" + "Cấp Sở,Ngành" + "</b>";
                    dr["CssClass"] = "highlight";
                    dr["CapID"] = CapCoQuanViewChiTiet.CapSoNganh.GetHashCode();
                    dr["ThuTu"] = 3.5;
                    dr["CoQuanID"] = 0;
                    //DataTable dtnew = BaoCao2b_DataTable_New();
                    foreach (DataRow dro in dt.Rows)
                    {
                        if (Utils.ConvertToInt32(dro["CapID"], 0) == (int)CapQuanLy.CapSoNganh)
                        {
                            for (var i = 1; i <= 32; i++)
                            {
                                if (dro["Col" + i + "Data"] == null)
                                {
                                    dro["Col" + i + "Data"] = 0;

                                }
                                dr["Col" + i + "Data"] = Utils.ConvertToInt32(dr["Col" + i + "Data"], 0) +
                                    Utils.ConvertToInt32(dro["Col" + i + "Data"], 0);
                            }
                        }
                    }

                    dt.Rows.Add(dr);
                }
                else if (capInfo.CapID == (int)CapQuanLy.CapUBNDHuyen)
                {
                    DataRow dr = dt.NewRow();
                    dr["TenCoQuan"] = "<b style='text-transform: uppercase'>" + "UBND Cấp Huyện" + "</b>";
                    dr["CssClass"] = "highlight";
                    dr["CapID"] = CapCoQuanViewChiTiet.CapUBNDHuyen.GetHashCode();
                    dr["ThuTu"] = 2.5;
                    dr["CoQuanID"] = 0;
                    //DataTable dtnew = BaoCao2b_DataTable_New();
                    foreach (DataRow dro in dt.Rows)
                    {
                        if (Utils.ConvertToInt32(dro["CapID"], 0) == (int)CapQuanLy.CapUBNDHuyen)
                        {
                            for (var i = 1; i <= 32; i++)
                            {
                                if (dro["Col" + i + "Data"] == null)
                                {
                                    dro["Col" + i + "Data"] = 0;

                                }
                                dr["Col" + i + "Data"] = Utils.ConvertToInt32(dr["Col" + i + "Data"], 0) +
                                    Utils.ConvertToInt32(dro["Col" + i + "Data"], 0);
                            }
                        }
                    }

                    dt.Rows.Add(dr);
                }
                else if (capInfo.CapID == (int)CapQuanLy.CapUBNDXa)
                {
                    DataRow dr = dt.NewRow();
                    dr["TenCoQuan"] = "<b style='text-transform: uppercase'>" + "UBND Cấp Xã" + "</b>";
                    dr["CssClass"] = "highlight";
                    dr["CapID"] = CapCoQuanViewChiTiet.CapUBNDXa.GetHashCode();
                    dr["ThuTu"] = 1.5;
                    dr["CoQuanID"] = 0;
                    //DataTable dtnew = BaoCao2b_DataTable_New();
                    foreach (DataRow dro in dt.Rows)
                    {
                        if (Utils.ConvertToInt32(dro["CapID"], 0) == (int)CapQuanLy.CapUBNDXa)
                        {
                            for (var i = 1; i <= 32; i++)
                            {
                                if (dro["Col" + i + "Data"] == null)
                                {
                                    dro["Col" + i + "Data"] = 0;
                                }
                                dr["Col" + i + "Data"] = Utils.ConvertToInt32(dr["Col" + i + "Data"], 0) +
                                    Utils.ConvertToInt32(dro["Col" + i + "Data"], 0);
                            }
                        }
                    }

                    dt.Rows.Add(dr);
                }
            }
            dt.DefaultView.Sort = "ThuTu desc";
            dt = dt.DefaultView.ToTable();
            //foreach (DataRow item in dt.Rows)
            //{
            //    int LuotDanKhongDen = Utils.ConvertToInt32(item["Col31Data"], 0);
            //    if (LuotDanKhongDen == 0)
            //    {
            //        item["Col31Data"] = "";
            //    }
            //    if (LuotDanKhongDen > 0)
            //    {
            //        item["Col31Data"] = LuotDanKhongDen + " lượt lãnh đạo trực dân không đến";
            //    }
            //}
        }

        public static void CalculateBaoCaoCapSo(string ContentRootPath, int RoleID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, ref List<BaoCao2bInfo> resultList, DateTime startDate, DateTime endDate, ref DataTable dt)
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
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "BaoCao2b_GetDonThu_1606", parm))
                {
                    dt.Load(dr);
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static void CalculateBaoCaoCapHuyen(string ContentRootPath, int RoleID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, int HuyenDangNhapID, ref List<BaoCao2bInfo> resultList, DateTime startDate, DateTime endDate, int tinhID, ref DataTable dt)
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
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "BaoCao2b_GetDonThu_1606", parm))
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

        public static void CalculateBaoCaoCapXa(string ContentRootPath, int CoQuanDangNhapID, int CanBoDangNhapID, ref List<BaoCao2bInfo> resultList, DateTime startDate, DateTime endDate, ref DataTable dt)
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
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "BaoCao2b_GetDonThu_1606", parm))
                {
                    dt.Load(dr);
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public DataTable BaoCao2b_DataTable()
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

        // ContentRootPath, RoleID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, ref resultList, startDate, endDate, tinhID
        public static void CalculateBaoCaoCapTrungUong(string ContentRootPath, int RoleID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, ref List<BaoCao2bInfo> resultList, DateTime startDate, DateTime endDate, int tinhID)
        {
            List<CapInfo> capList = new BaoCao2aDAL().GetAll().ToList();
            foreach (CapInfo capInfo in capList)
            {
                //Neu la cap trung uong, chi hien thi cac tinh
                if (capInfo.CapID == (int)CapQuanLy.CapUBNDTinh)
                {
                    BaoCao2bInfo bc2bInfo = new BaoCao2bInfo();
                    bc2bInfo.DonVi = "<b style='text-transform: uppercase'>" + capInfo.TenCap + "</b>";
                    bc2bInfo.CapID = (int)CapCoQuanViewChiTiet.CapTrungUong;
                    resultList.Add(bc2bInfo);
                    List<CoQuanInfo> cqList = new CoQuan().GetCoQuanByCapAndTinhID(capInfo.CapID, tinhID).ToList();
                    if (cqList.Count > 0)
                    {
                        foreach (CoQuanInfo cqInfo in cqList)
                        {
                            BaoCao2bInfo bc2bInfo2 = new BaoCao2bInfo();
                            bc2bInfo2.DonVi = cqInfo.TenCoQuan;
                            List<BaoCao2bDonThuInfo> dtList = new BaoCao2bDAL().GetDonThuByTinh(startDate, endDate, cqInfo.TinhID).ToList();
                            Calculate(ref bc2bInfo2, dtList, startDate);
                            KeKhaiDuLieuDauKy_2bInfo keKhaiInfo = new BaoCao2bDAL().GetByCoQuan(cqInfo.CoQuanID);
                            if (keKhaiInfo.NgaySuDung >= startDate && keKhaiInfo.NgaySuDung <= endDate)
                            {
                                #region Cong so lieu tong
                                //bc2bInfo2.Col1Data += keKhaiInfo.Col1;
                                bc2bInfo2.Col2Data += keKhaiInfo.Col2;
                                bc2bInfo2.Col3Data += keKhaiInfo.Col3;
                                bc2bInfo.Col1Data = bc2bInfo2.Col2Data + bc2bInfo2.Col3Data;
                                bc2bInfo2.Col4Data += keKhaiInfo.Col4;
                                bc2bInfo2.Col5Data += keKhaiInfo.Col5;
                                bc2bInfo2.Col6Data += keKhaiInfo.Col6;
                                bc2bInfo2.Col7Data += keKhaiInfo.Col7;
                                bc2bInfo2.Col8Data += keKhaiInfo.Col8;
                                bc2bInfo2.Col9Data += keKhaiInfo.Col9;
                                bc2bInfo2.Col10Data += keKhaiInfo.Col10;
                                bc2bInfo2.Col11Data += keKhaiInfo.Col11;
                                bc2bInfo2.Col12Data += keKhaiInfo.Col12;
                                bc2bInfo2.Col13Data += keKhaiInfo.Col13;
                                bc2bInfo2.Col14Data += keKhaiInfo.Col14;
                                bc2bInfo2.Col15Data += keKhaiInfo.Col15;
                                bc2bInfo2.Col16Data += keKhaiInfo.Col16;
                                bc2bInfo2.Col17Data += keKhaiInfo.Col17;
                                bc2bInfo2.Col18Data += keKhaiInfo.Col18;
                                bc2bInfo2.Col19Data += keKhaiInfo.Col19;
                                bc2bInfo2.Col20Data += keKhaiInfo.Col20;
                                bc2bInfo2.Col21Data += keKhaiInfo.Col21;
                                bc2bInfo2.Col22Data += keKhaiInfo.Col22;
                                bc2bInfo2.Col23Data += keKhaiInfo.Col23;
                                bc2bInfo2.Col24Data += keKhaiInfo.Col24;
                                bc2bInfo2.Col25Data += keKhaiInfo.Col25;
                                bc2bInfo2.Col26Data += keKhaiInfo.Col26;
                                bc2bInfo2.Col27Data += keKhaiInfo.Col27;
                                bc2bInfo2.Col28Data += keKhaiInfo.Col28;
                                bc2bInfo2.Col29Data += keKhaiInfo.Col29;
                                bc2bInfo2.Col30Data += keKhaiInfo.Col30;
                                bc2bInfo2.Col31Data += keKhaiInfo.Col31;
                                #endregion
                            }
                            resultList.Add(bc2bInfo2);

                        }
                    }
                }
            }
        }

        private static void Calculate(ref BaoCao2bInfo bc2bInfo, List<BaoCao2bDonThuInfo> dtList, DateTime startDate)
        {
            foreach (BaoCao2bDonThuInfo dtInfo in dtList)
            {
                //Don tiep nhan trong ky
                if (dtInfo.NgayNhapDon >= startDate)
                {
                    bc2bInfo.Col1Data++;
                    if (dtInfo.SoLuong > 1)
                    {
                        bc2bInfo.Col2Data++;
                    }
                    else bc2bInfo.Col3Data++;
                }
                //Don ki truoc chuyen sang: ngay tiep nhan < startdate va ngay ra kq > startdate hoac chua co ket qua(ngay ra kq = min value)
                else if (dtInfo.NgayNhapDon < startDate && dtInfo.HuongGiaiQuyetID == 0)
                {
                    bc2bInfo.Col1Data++;
                    if (dtInfo.SoLuong > 1)
                    {
                        bc2bInfo.Col4Data++;
                    }
                    else bc2bInfo.Col5Data++;
                }
                //duyet cac don tiep nhan trong ky va ky truoc chuyen sang
                if (dtInfo.NgayNhapDon >= startDate || (dtInfo.NgayNhapDon < startDate && dtInfo.HuongGiaiQuyetID == 0))
                {
                    //Cac don du dk: duoc de xuat thu ly va ko bi tu choi thu ly
                    if (dtInfo.HuongGiaiQuyetID == (int)HuongGiaiQuyetEnum.DeXuatThuLy && dtInfo.TrangThaiDonID != (int)TrangThaiDonEnum.TuChoiThuLy)
                    {
                        bc2bInfo.Col6Data++;
                    }

                    //Phan loai don khieu nai, to cao
                    //Theo noi dung
                    //khieu nai
                    if (dtInfo.LoaiKhieuTo3ID == Constant.VeTranhChapDatDai)
                    {
                        bc2bInfo.Col8Data++;
                    }
                    else if (dtInfo.LoaiKhieuTo3ID == Constant.VeNhaTaiSan)
                    {
                        bc2bInfo.Col9Data++;
                    }
                    else if (dtInfo.LoaiKhieuTo3ID == Constant.VeChinhSach || dtInfo.LoaiKhieuTo3ID == Constant.VeCheDo)
                    {
                        bc2bInfo.Col10Data++;
                    }
                    else if (dtInfo.LoaiKhieuTo2ID == Constant.KN_LinhVucKhac)
                    {
                        bc2bInfo.Col11Data++;
                    }
                    else if (dtInfo.LoaiKhieuTo2ID == Constant.KN_LinhVucTuPhap)
                    {
                        bc2bInfo.Col12Data++;
                    }
                    else if (dtInfo.LoaiKhieuTo2ID == Constant.KN_VeDang)
                    {
                        bc2bInfo.Col13Data++;
                    }
                    bc2bInfo.Col7Data = bc2bInfo.Col8Data + bc2bInfo.Col9Data + bc2bInfo.Col10Data + bc2bInfo.Col11Data;

                    //to cao
                    if (dtInfo.LoaiKhieuTo2ID == Constant.TC_LinhVucHanhChinh)
                    {
                        bc2bInfo.Col15Data++;
                    }
                    else if (dtInfo.LoaiKhieuTo2ID == Constant.TC_LinhVucTuPhap)
                    {
                        bc2bInfo.Col16Data++;
                    }
                    else if (dtInfo.LoaiKhieuTo2ID == Constant.ThamNhung)
                    {
                        bc2bInfo.Col17Data++;
                    }
                    else if (dtInfo.LoaiKhieuTo2ID == Constant.TC_VeDang)
                    {
                        bc2bInfo.Col18Data++;
                    }
                    else if (dtInfo.LoaiKhieuTo2ID == Constant.TC_LinhVucKhac)
                    {
                        bc2bInfo.Col19Data++;
                    }
                    bc2bInfo.Col14Data = bc2bInfo.Col15Data + bc2bInfo.Col16Data + bc2bInfo.Col17Data + bc2bInfo.Col18Data + bc2bInfo.Col19Data;

                    //Theo tham quyen giai quyet
                    if (dtInfo.ThamQuyenCQXuLy == Constant.CQHanhChinhCacCap || dtInfo.ThamQuyenCQChuyenDon == Constant.CQHanhChinhCacCap)
                    {
                        bc2bInfo.Col20Data++;
                    }
                    else if (dtInfo.ThamQuyenCQXuLy == Constant.CQTuPhapCacCap || dtInfo.ThamQuyenCQChuyenDon == Constant.CQTuPhapCacCap)
                    {
                        bc2bInfo.Col21Data++;
                    }
                    else if (dtInfo.ThamQuyenCQXuLy == Constant.CQDang || dtInfo.ThamQuyenCQChuyenDon == Constant.CQDang)
                    {
                        bc2bInfo.Col22Data++;
                    }

                    //Theo trinh tu giai quyet
                    //chua duoc giai quyet: don chua co ket qua
                    if (dtInfo.KetQuaID == 0)
                    {
                        bc2bInfo.Col23Data++;
                    }
                    //da co ket qua
                    else
                    {
                        //giai quyet lan dau
                        if (dtInfo.SoLan == 1)
                        {
                            bc2bInfo.Col24Data++;
                        }
                        //giai quyet lan hai
                        else if (dtInfo.SoLan > 1)
                        {
                            bc2bInfo.Col25Data++;
                        }
                    }

                    //Don khac
                    if (dtInfo.LoaiKhieuTo1ID == Constant.PhanAnhKienNghi)
                    {
                        bc2bInfo.Col26Data++;
                    }

                    //Ket qua xu ly
                    if (dtInfo.HuongGiaiQuyetID == (int)HuongGiaiQuyetEnum.HuongDan)
                    {
                        bc2bInfo.Col27Data++;
                    }
                    else if (dtInfo.HuongGiaiQuyetID == (int)HuongGiaiQuyetEnum.ChuyenDon)
                    {
                        bc2bInfo.Col28Data++;
                    }
                    else if (dtInfo.HuongGiaiQuyetID == (int)HuongGiaiQuyetEnum.RaVanBanDonDoc)
                    {
                        bc2bInfo.Col29Data++;
                    }
                    else
                    if (dtInfo.HuongGiaiQuyetID == (int)HuongGiaiQuyetEnum.DeXuatThuLy)
                    {
                        if (dtInfo.LoaiKhieuTo1ID == Constant.KhieuNai)
                        {
                            bc2bInfo.Col30Data++;
                        }
                        else if (dtInfo.LoaiKhieuTo1ID == Constant.ToCao)
                        {
                            bc2bInfo.Col31Data++;
                        }
                    }
                }
            }
        }

        private CapInfo GetData_Cap(SqlDataReader dr)
        {
            CapInfo cInfo = new CapInfo();
            cInfo.CapID = Utils.GetInt32(dr["CapID"].ToString(), 0);
            cInfo.TenCap = Utils.GetString(dr["TenCap"].ToString(), string.Empty);
            cInfo.ThuTu = Utils.GetInt32(dr["ThuTu"], 0);
            if (cInfo.ThuTu != 0)
                cInfo.CapQuanLy = cInfo.ThuTu.ToString();

            return cInfo;
        }
        public IList<CapInfo> GetAll()
        {
            IList<CapInfo> caps = new List<CapInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "DM_Cap_GetAll", null))
                {

                    while (dr.Read())
                    {

                        CapInfo cInfo = GetData_Cap(dr);
                        caps.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return caps;
        }
        public KeKhaiDuLieuDauKy_2bInfo GetByCoQuan(int coquanID)
        {
            //IList<KeKhaiDuLieuDauKy_2aInfo> infoList = new List<KeKhaiDuLieuDauKy_2aInfo>();
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(@"CoQuanID", SqlDbType.Int),

            };
            parm[0].Value = coquanID;
            KeKhaiDuLieuDauKy_2bInfo info = new KeKhaiDuLieuDauKy_2bInfo();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "KeKhaiDuLieuDauKy_2b_GetByCoQuan", parm))
                {
                    if (dr.Read())
                    {
                        info = GetData2a(dr);
                        //infoList.Add(info);
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return info;
        }
        public IList<BaoCao2bDonThuInfo> GetDonThuByTinh(DateTime startDate, DateTime endDate, int tinhID)
        {
            IList<BaoCao2bDonThuInfo> infoList = new List<BaoCao2bDonThuInfo>();
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_STARTDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_ENDDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_TINHID, SqlDbType.Int)
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            parm[2].Value = tinhID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "BaoCao2b_GetDonThuByTinh", parm))
                {
                    while (dr.Read())
                    {
                        BaoCao2bDonThuInfo info = GetData(dr);
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

        public class KeKhaiDuLieuDauKy_2bInfo
        {
            public int Col1 { get; set; }
            public int Col2 { get; set; }
            public int Col3 { get; set; }
            public int Col4 { get; set; }
            public int Col5 { get; set; }
            public int Col6 { get; set; }
            public int Col7 { get; set; }
            public int Col8 { get; set; }
            public int Col9 { get; set; }
            public int Col10 { get; set; }
            public int Col11 { get; set; }
            public int Col12 { get; set; }
            public int Col13 { get; set; }
            public int Col14 { get; set; }
            public int Col15 { get; set; }
            public int Col16 { get; set; }
            public int Col17 { get; set; }
            public int Col18 { get; set; }
            public int Col19 { get; set; }
            public int Col20 { get; set; }
            public int Col21 { get; set; }
            public int Col22 { get; set; }
            public int Col23 { get; set; }
            public int Col24 { get; set; }
            public int Col25 { get; set; }
            public int Col26 { get; set; }
            public int Col27 { get; set; }
            public int Col28 { get; set; }
            public int Col29 { get; set; }
            public int Col30 { get; set; }
            public int Col31 { get; set; }
            public int CoQuanID { get; set; }
            public DateTime NgaySuDung { get; set; }
        }
    }
}
