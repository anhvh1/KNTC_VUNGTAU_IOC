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
    public class BaoCao2aDAL
    {
        private const string PARM_STARTDATE = "@StartDate";
        private const string PARM_ENDDATE = "@EndDate";
        private const string PARM_COQUANID = "@CoQuanID";
        private const string PARM_TINHID = @"TinhID";
        private BaoCao2aDonThuInfo GetData(SqlDataReader rdr)
        {
            BaoCao2aDonThuInfo info = new BaoCao2aDonThuInfo();
            info.DonThuID = Utils.ConvertToInt32(rdr["DonThuID"], 0);
            info.GapLanhDao = Utils.ConvertToBoolean(rdr["GapLanhDao"], false);
            info.LoaiKhieuTo1ID = Utils.ConvertToInt32(rdr["LoaiKhieuTo1ID"], 0);
            info.LoaiKhieuTo2ID = Utils.ConvertToInt32(rdr["LoaiKhieuTo2ID"], 0);
            info.LoaiKhieuTo3ID = Utils.ConvertToInt32(rdr["LoaiKhieuTo3ID"], 0);
            info.LoaiKhieuToID = Utils.ConvertToInt32(rdr["LoaiKhieuToID"], 0);
            info.NgayTiep = Utils.ConvertToDateTime(rdr["NgayTiep"], DateTime.MinValue);
            info.NgayNhapDon = Utils.ConvertToDateTime(rdr["NgayNhapDon"], DateTime.MinValue);
            info.SoLuong = Utils.ConvertToInt32(rdr["SoLuong"], 0);
            info.HuongXuLy = Utils.ConvertToInt32(rdr["HuongGiaiQuyetID"], 0);
            info.KetQuaID = Utils.ConvertToInt32(rdr["KetQuaID"], 0);
            info.VuViecCu = Utils.ConvertToBoolean(rdr["VuViecCu"], false);
            //info.CQTiepNhanID = Utils.ConvertToInt32(rdr["CQTiepNhanID"], 0);

            return info;
        }
        private KeKhaiDuLieuDauKy_2aInfo GetData2a(SqlDataReader rdr)
        {
            KeKhaiDuLieuDauKy_2aInfo info = new KeKhaiDuLieuDauKy_2aInfo();
            info.TiepThuongXuyen_Luot = Utils.ConvertToInt32(rdr["TiepThuongXuyen_Luot"], 0);
            info.TiepThuongXuyen_Nguoi = Utils.ConvertToInt32(rdr["TiepThuongXuyen_Nguoi"], 0);
            info.TiepThuongXuyen_VuViecCu = Utils.ConvertToInt32(rdr["TiepThuongXuyen_VuViecCu"], 0);
            info.TiepThuongXuyen_VuViecMoi = Utils.ConvertToInt32(rdr["TiepThuongXuyen_VuViecMoi"], 0);
            info.TiepThuongXuyen_DDN_SoDoan = Utils.ConvertToInt32(rdr["TiepThuongXuyen_DDN_SoDoan"], 0);
            info.TiepThuongXuyen_DDN_Nguoi = Utils.ConvertToInt32(rdr["TiepThuongXuyen_DDN_Nguoi"], 0);
            info.TiepThuongXuyen_DDN_VuViecCu = Utils.ConvertToInt32(rdr["TiepThuongXuyen_DDN_VuViecCu"], 0);
            info.TiepThuongXuyen_DDN_VuViecMoi = Utils.ConvertToInt32(rdr["TiepThuongXuyen_DDN_VuViecMoi"], 0);
            info.LanhDaoTiep_Luot = Utils.ConvertToInt32(rdr["LanhDaoTiep_Luot"], 0);
            info.LanhDaoTiep_Nguoi = Utils.ConvertToInt32(rdr["LanhDaoTiep_Nguoi"], 0);
            info.LanhDaoTiep_VuViecCu = Utils.ConvertToInt32(rdr["LanhDaoTiep_VuViecCu"], 0);
            info.LanhDaoTiep_VuViecMoi = Utils.ConvertToInt32(rdr["LanhDaoTiep_VuViecMoi"], 0);
            info.LanhDaoTiep_DDN_SoDoan = Utils.ConvertToInt32(rdr["LanhDaoTiep_DDN_SoDoan"], 0);
            info.LanhDaoTiep_DDN_Nguoi = Utils.ConvertToInt32(rdr["LanhDaoTiep_DDN_Nguoi"], 0);
            info.LanhDaoTiep_DDN_VuViecCu = Utils.ConvertToInt32(rdr["LanhDaoTiep_DDN_VuViecCu"], 0);
            info.LanhDaoTiep_DDN_VuViecMoi = Utils.ConvertToInt32(rdr["LanhDaoTiep_DDN_VuViecMoi"], 0);
            info.NoiDung_KhieuNai_HanhChinh_Dat = Utils.ConvertToInt32(rdr["NoiDung_KhieuNai_HanhChinh_Dat"], 0);
            info.NoiDung_KhieuNai_HanhChinh_ChinhSach = Utils.ConvertToInt32(rdr["NoiDung_KhieuNai_HanhChinh_ChinhSach"], 0);
            info.NoiDung_KhieuNai_HanhChinh_TaiSan = Utils.ConvertToInt32(rdr["NoiDung_KhieuNai_HanhChinh_TaiSan"], 0);
            info.NoiDung_KhieuNai_HanhChinh_CheDoCCVC = Utils.ConvertToInt32(rdr["NoiDung_KhieuNai_HanhChinh_CheDoCCVC"], 0);
            info.NoiDung_KhieuNai_TuPhap = Utils.ConvertToInt32(rdr["NoiDung_KhieuNai_TuPhap"], 0);
            info.NoiDung_KhieuNai_CTVHXH = Utils.ConvertToInt32(rdr["NoiDung_KhieuNai_CTVHXH"], 0);
            info.NoiDung_ToCao_HanhChinh = Utils.ConvertToInt32(rdr["NoiDung_ToCao_HanhChinh"], 0);
            info.NoiDung_ToCao_TuPhap = Utils.ConvertToInt32(rdr["NoiDung_ToCao_TuPhap"], 0);
            info.NoiDung_ToCao_ThamNhung = Utils.ConvertToInt32(rdr["NoiDung_ToCao_ThamNhung"], 0);
            info.NoiDung_Khac = Utils.ConvertToInt32(rdr["NoiDung_Khac"], 0);
            info.KetQua_ChuaGiaiQuyet = Utils.ConvertToInt32(rdr["KetQua_ChuaGiaiQuyet"], 0);
            info.KetQua_DaGQ_ChuaCoQD = Utils.ConvertToInt32(rdr["KetQua_DaGQ_ChuaCoQD"], 0);
            info.KetQua_DaGQ_DaCoQD = Utils.ConvertToInt32(rdr["KetQua_DaGQ_DaCoQD"], 0);
            info.KetQua_DaGQ_DaCoBanAn = Utils.ConvertToInt32(rdr["KetQua_DaGQ_DaCoBanAn"], 0);
            info.CoQuanID = Utils.ConvertToInt32(rdr["CoQuanID"], 0);
            info.NgaySuDung = Utils.ConvertToDateTime(rdr["NgaySuDung"], DateTime.MinValue);
            return info;
        }
        public List<ThongKeBC_2a_DongBo_IOC> BC2a(List<int> listCapChonBaoCao, string ContentRootPath, int RoleID, int CapID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, int HuyenDangNhapID, DateTime startDate, DateTime endDate)
        {
            List<ThongKeBC_2a_DongBo_IOC> data2 = new List<ThongKeBC_2a_DongBo_IOC>();
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
                double col9 = Utils.ConvertToDouble(dro["Col9Data"], 0);
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
                double tongSoLuotTCD = col1 + col9;
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
                if (Convert.ToInt32(RowItem1.CoQuanID) > 0)
                {
                    var model = new ThongKeBC_2a_DongBo_IOC();
                    model.CoQuanId = Utils.ConvertToInt32(RowItem1.CoQuanID, 0);
                    model.TenCoQuan = Utils.ConvertToString(RowItem1.Content, string.Empty);
                    model.TongSoLuotTCD = Utils.ConvertToInt32(tongSoLuotTCD, 0);
                    model.TongSoLuotTTX = Utils.ConvertToInt32(col1, 0);
                    model.TongSoLuotTTT = Utils.ConvertToInt32(col9, 0);
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
                        using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "BaoCao2a_GetDonThu_1606_New", parm))
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
                    for (var i = 1; i <= 31; i++)
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
                            for (var i = 1; i <= 31; i++)
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
                            for (var i = 1; i <= 31; i++)
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
                            for (var i = 1; i <= 31; i++)
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
                            for (var i = 1; i <= 31; i++)
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
                            for (var i = 1; i <= 31; i++)
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
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "BaoCao2a_GetDonThu_1606_New", parm))
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
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "BaoCao2a_GetDonThu_1606_New", parm))
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
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "BaoCao2a_GetDonThu_1606_New", parm))
                {
                    dt.Load(dr);
                }

            }
            catch (Exception e)
            {
                throw e;
            }
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

        // ContentRootPath, RoleID, CoQuanDangNhapID, CanBoDangNhapID, TinhDangNhapID, ref resultList, startDate, endDate, tinhID
        public static void CalculateBaoCaoCapTrungUong(string ContentRootPath, int RoleID, int CoQuanDangNhapID, int CanBoDangNhapID, int TinhDangNhapID, ref List<BaoCao2aInfo> resultList, DateTime startDate, DateTime endDate, int tinhID)
        {
            List<CapInfo> capList = new BaoCao2aDAL().GetAll().ToList();
            foreach (CapInfo capInfo in capList)
            {
                //Neu la cap trung uong, chi hien thi cac tinh
                if (capInfo.CapID == (int)CapQuanLy.CapUBNDTinh)
                {
                    BaoCao2aInfo bc2aInfo = new BaoCao2aInfo();
                    bc2aInfo.DonVi = "<b style='text-transform: uppercase'>" + capInfo.TenCap + "</b>";
                    bc2aInfo.CapID = (int)CapCoQuanViewChiTiet.CapTrungUong;
                    resultList.Add(bc2aInfo);
                    List<CoQuanInfo> cqList = new CoQuan().GetCoQuanByCapAndTinhID(capInfo.CapID, tinhID).ToList();
                    if (cqList.Count > 0)
                    {
                        foreach (CoQuanInfo cqInfo in cqList)
                        {
                            BaoCao2aInfo bc2aInfo2 = new BaoCao2aInfo();
                            bc2aInfo2.DonVi = cqInfo.TenCoQuan;
                            List<BaoCao2aDonThuInfo> dtList = new BaoCao2aDAL().GetDonThuByTinh(startDate, endDate, cqInfo.TinhID).ToList();
                            Calculate(ref bc2aInfo2, dtList, startDate, endDate);
                            KeKhaiDuLieuDauKy_2aInfo keKhaiInfo = new BaoCao2aDAL().GetByCoQuan(cqInfo.CoQuanID);
                            if (keKhaiInfo.NgaySuDung >= startDate && keKhaiInfo.NgaySuDung <= endDate)
                            {
                                #region Cong so lieu tong
                                bc2aInfo2.Col1Data += keKhaiInfo.TiepThuongXuyen_Luot;
                                bc2aInfo2.Col2Data += keKhaiInfo.TiepThuongXuyen_Nguoi;
                                bc2aInfo2.Col3Data += keKhaiInfo.TiepThuongXuyen_VuViecCu;
                                bc2aInfo2.Col4Data += keKhaiInfo.TiepThuongXuyen_VuViecMoi;
                                bc2aInfo2.Col5Data += keKhaiInfo.TiepThuongXuyen_DDN_SoDoan;
                                bc2aInfo2.Col6Data += keKhaiInfo.TiepThuongXuyen_DDN_Nguoi;
                                bc2aInfo2.Col7Data += keKhaiInfo.TiepThuongXuyen_DDN_VuViecCu;
                                bc2aInfo2.Col8Data += keKhaiInfo.TiepThuongXuyen_DDN_VuViecMoi;
                                bc2aInfo2.Col9Data += keKhaiInfo.LanhDaoTiep_Luot;
                                bc2aInfo2.Col10Data += keKhaiInfo.LanhDaoTiep_Nguoi;
                                bc2aInfo2.Col11Data += keKhaiInfo.LanhDaoTiep_VuViecCu;
                                bc2aInfo2.Col12Data += keKhaiInfo.LanhDaoTiep_VuViecMoi;
                                bc2aInfo2.Col13Data += keKhaiInfo.LanhDaoTiep_DDN_SoDoan;
                                bc2aInfo2.Col14Data += keKhaiInfo.LanhDaoTiep_DDN_Nguoi;
                                bc2aInfo2.Col15Data += keKhaiInfo.LanhDaoTiep_DDN_VuViecCu;
                                bc2aInfo2.Col16Data += keKhaiInfo.LanhDaoTiep_DDN_VuViecMoi;
                                bc2aInfo2.Col17Data += keKhaiInfo.NoiDung_KhieuNai_HanhChinh_Dat;
                                bc2aInfo2.Col18Data += keKhaiInfo.NoiDung_KhieuNai_HanhChinh_ChinhSach;
                                bc2aInfo2.Col19Data += keKhaiInfo.NoiDung_KhieuNai_HanhChinh_TaiSan;
                                bc2aInfo2.Col20Data += keKhaiInfo.NoiDung_KhieuNai_HanhChinh_CheDoCCVC;
                                bc2aInfo2.Col21Data += keKhaiInfo.NoiDung_KhieuNai_TuPhap;
                                bc2aInfo2.Col22Data += keKhaiInfo.NoiDung_KhieuNai_CTVHXH;
                                bc2aInfo2.Col23Data += keKhaiInfo.NoiDung_ToCao_HanhChinh;
                                bc2aInfo2.Col24Data += keKhaiInfo.NoiDung_ToCao_TuPhap;
                                bc2aInfo2.Col25Data += keKhaiInfo.NoiDung_ToCao_ThamNhung;
                                bc2aInfo2.Col26Data += keKhaiInfo.NoiDung_Khac;
                                bc2aInfo2.Col27Data += keKhaiInfo.KetQua_ChuaGiaiQuyet;
                                bc2aInfo2.Col28Data += keKhaiInfo.KetQua_DaGQ_ChuaCoQD;
                                bc2aInfo2.Col29Data += keKhaiInfo.KetQua_DaGQ_DaCoQD;
                                bc2aInfo2.Col30Data += keKhaiInfo.KetQua_DaGQ_DaCoBanAn;
                                #endregion
                            }
                            resultList.Add(bc2aInfo2);

                        }
                    }
                }
            }
        }

        private static void Calculate(ref BaoCao2aInfo bc2aInfo, List<BaoCao2aDonThuInfo> dtList, DateTime startDate, DateTime endDate)
        {
            //dem so luot tiep: duyet danh sach tiep cong dan
            foreach (BaoCao2aDonThuInfo dtInfo in dtList)
            {
                //So luot tiep thuong xuyen
                if (!dtInfo.GapLanhDao)
                {
                    bc2aInfo.Col1Data++;
                    //So nguoi tiep thuong xuyen                
                    bc2aInfo.Col2Data += dtInfo.SoLuong;

                    //Neu vu viec phat sinh truoc ki bao cao, voi la vu viec cu
                    if (dtInfo.VuViecCu)
                    {
                        bc2aInfo.Col3Data++;
                    }
                    else
                    {
                        bc2aInfo.Col4Data++;
                    }
                    //doan dong nguoi
                    if (dtInfo.SoLuong >= 5)
                    {
                        bc2aInfo.Col5Data++;
                        bc2aInfo.Col6Data += dtInfo.SoLuong;
                        if (dtInfo.VuViecCu)
                        {
                            bc2aInfo.Col7Data++;
                        }
                        else
                        {
                            bc2aInfo.Col8Data++;
                        }
                    }
                }
                //tiep lanh dao
                else
                {
                    //luot, nguoi
                    bc2aInfo.Col9Data++;
                    bc2aInfo.Col10Data += dtInfo.SoLuong;
                    if (dtInfo.VuViecCu)
                    {
                        bc2aInfo.Col11Data++;
                    }
                    else
                    {
                        bc2aInfo.Col12Data++;
                    }
                    //doan dong nguoi
                    if (dtInfo.SoLuong >= 5)
                    {
                        bc2aInfo.Col13Data++;
                        bc2aInfo.Col14Data += dtInfo.SoLuong;

                        //Neu vu viec phat sinh truoc ki bao cao, voi la vu viec cu
                        if (dtInfo.VuViecCu)
                        {
                            bc2aInfo.Col15Data++;
                        }
                        else
                        {
                            bc2aInfo.Col16Data++;
                        }
                    }
                }

                //noi dung tiep cong dan: thong ke tren so vu viec moi phat sinh
                if (dtInfo.LoaiKhieuTo3ID == Constant.VeTranhChapDatDai)
                {
                    bc2aInfo.Col17Data++;
                }
                else if (dtInfo.LoaiKhieuTo3ID == Constant.VeChinhSach)
                {
                    bc2aInfo.Col18Data++;
                }
                else if (dtInfo.LoaiKhieuTo3ID == Constant.VeNhaTaiSan)
                {
                    bc2aInfo.Col19Data++;
                }
                else if (dtInfo.LoaiKhieuTo3ID == Constant.VeCheDo)
                {
                    bc2aInfo.Col20Data++;
                }
                else if (dtInfo.LoaiKhieuTo2ID == Constant.KN_LinhVucTuPhap)
                {
                    bc2aInfo.Col21Data++;
                }
                else if (dtInfo.LoaiKhieuTo2ID == Constant.KN_LinhVucKhac)
                {
                    bc2aInfo.Col22Data++;
                }
                else if (dtInfo.LoaiKhieuTo2ID == Constant.TC_LinhVucHanhChinh)
                {
                    bc2aInfo.Col23Data++;
                }
                else if (dtInfo.LoaiKhieuTo2ID == Constant.TC_LinhVucTuPhap)
                {
                    bc2aInfo.Col24Data++;
                }
                else if (dtInfo.LoaiKhieuTo2ID == Constant.ThamNhung)
                {
                    bc2aInfo.Col25Data++;
                }
                else if (dtInfo.LoaiKhieuTo1ID == Constant.PhanAnhKienNghi)
                {
                    bc2aInfo.Col26Data++;
                }

                if (dtInfo.HuongXuLy == 0) // đề xuất thụ lý
                {
                    bc2aInfo.Col27Data++;
                }
                else if (dtInfo.HuongXuLy == (int)HuongGiaiQuyetEnum.DeXuatThuLy)
                {
                    if (dtInfo.KetQuaID == 0)
                    {
                        bc2aInfo.Col28Data++;
                    }
                    else
                    {
                        bc2aInfo.Col29Data++;
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
        public KeKhaiDuLieuDauKy_2aInfo GetByCoQuan(int coquanID)
        {
            //IList<KeKhaiDuLieuDauKy_2aInfo> infoList = new List<KeKhaiDuLieuDauKy_2aInfo>();
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(@"CoQuanID", SqlDbType.Int),

            };
            parm[0].Value = coquanID;
            KeKhaiDuLieuDauKy_2aInfo info = new KeKhaiDuLieuDauKy_2aInfo();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "KeKhaiDuLieuDauKy_2a_GetByCoQuan", parm))
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
        public IList<BaoCao2aDonThuInfo> GetDonThuByTinh(DateTime startDate, DateTime endDate, int tinhID)
        {
            IList<BaoCao2aDonThuInfo> infoList = new List<BaoCao2aDonThuInfo>();
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
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "BaoCao2a_GetDonThuByTinh", parm))
                {
                    while (dr.Read())
                    {
                        BaoCao2aDonThuInfo info = GetData(dr);
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

        public class KeKhaiDuLieuDauKy_2aInfo
        {
            public int TiepThuongXuyen_Luot { get; set; }
            public int TiepThuongXuyen_Nguoi { get; set; }
            public int TiepThuongXuyen_VuViecCu { get; set; }
            public int TiepThuongXuyen_VuViecMoi { get; set; }
            public int TiepThuongXuyen_DDN_SoDoan { get; set; }
            public int TiepThuongXuyen_DDN_Nguoi { get; set; }
            public int TiepThuongXuyen_DDN_VuViecCu { get; set; }
            public int TiepThuongXuyen_DDN_VuViecMoi { get; set; }
            public int LanhDaoTiep_Luot { get; set; }
            public int LanhDaoTiep_Nguoi { get; set; }
            public int LanhDaoTiep_VuViecCu { get; set; }
            public int LanhDaoTiep_VuViecMoi { get; set; }
            public int LanhDaoTiep_DDN_SoDoan { get; set; }
            public int LanhDaoTiep_DDN_Nguoi { get; set; }
            public int LanhDaoTiep_DDN_VuViecCu { get; set; }
            public int LanhDaoTiep_DDN_VuViecMoi { get; set; }
            public int NoiDung_KhieuNai_HanhChinh_Dat { get; set; }
            public int NoiDung_KhieuNai_HanhChinh_ChinhSach { get; set; }
            public int NoiDung_KhieuNai_HanhChinh_TaiSan { get; set; }
            public int NoiDung_KhieuNai_HanhChinh_CheDoCCVC { get; set; }
            public int NoiDung_KhieuNai_TuPhap { get; set; }
            public int NoiDung_KhieuNai_CTVHXH { get; set; }
            public int NoiDung_ToCao_HanhChinh { get; set; }
            public int NoiDung_ToCao_TuPhap { get; set; }
            public int NoiDung_ToCao_ThamNhung { get; set; }
            public int NoiDung_Khac { get; set; }
            public int KetQua_ChuaGiaiQuyet { get; set; }
            public int KetQua_DaGQ_ChuaCoQD { get; set; }
            public int KetQua_DaGQ_DaCoQD { get; set; }
            public int KetQua_DaGQ_DaCoBanAn { get; set; }
            public int CoQuanID { get; set; }
            public DateTime NgaySuDung { get; set; }
        }
    }
}
