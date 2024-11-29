using Com.Gosol.KNTC.Models.BaoCao;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Ultilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.DAL.KNTC
{
    public class BaoCaoPhanAnhKienNghiCalc
    {
        public static void CalculateBaoCaoCapTinh(ref List<BaoCaoPhanAnhKienNghiInfo> resultList, DateTime startDate, DateTime endDate, List<CapInfo> capList, bool flag, int tinhID, IdentityHelper IdentityHelper)
        {

            BaoCaoPhanAnhKienNghiInfo bcInfo = new BaoCaoPhanAnhKienNghiInfo();
            if (flag == true)
            {
                bcInfo.DonVi = "Toàn tỉnh";
                bcInfo.CssClass = "font-weight:bold;text-transform: uppercase";
                //bcInfo.CapID = 0;
                bcInfo.CapID = CapCoQuanViewChiTiet.ToanTinh.GetHashCode();
                resultList.Add(bcInfo);
            }
            //System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            //stopwatch.Start();
            //DateTime firstDays = Season.GetFirstDayOfYear(startDate.Year);
            DateTime firstDays = startDate;
            //List<BaoCaoPhanAnhKienNghiDonThuInfo> dtLists = new BaoCaoPhanAnhKienNghi().GetDonThu(firstDays, endDate, 0).ToList();
            //var timer = stopwatch.Elapsed.TotalSeconds;
            foreach (CapInfo capInfo in capList)
            {
                BaoCaoPhanAnhKienNghiInfo bc2cInfo = new BaoCaoPhanAnhKienNghiInfo();
                BaoCaoPhanAnhKienNghiInfo bc2cInfoToanHuyen = new BaoCaoPhanAnhKienNghiInfo();
                //if (capInfo.CapID != (int)CapQuanLy.ToanHuyen)
                //{
                //    bc2cInfo.DonVi = "<b style='text-transform: uppercase'>" + capInfo.TenCap + "</b>";
                //    bc2cInfo.CssClass = "font-weight:bold;text-transform: uppercase";
                //    bc2cInfo.CapID = capInfo.CapID;
                //    resultList.Add(bc2cInfo);
                //}
                if (capInfo.CapID != (int)CapQuanLy.ToanHuyen)
                {
                    bc2cInfo.DonVi = "" + capInfo.TenCap + "";
                    bc2cInfo.CssClass = "font-weight:bold;text-transform: uppercase";
                    if (capInfo.CapID == CapQuanLy.CapUBNDTinh.GetHashCode())
                    {
                        bc2cInfo.CapID = CapCoQuanViewChiTiet.CapUBNDTinh.GetHashCode();
                    }
                    else if (capInfo.CapID == CapQuanLy.CapUBNDHuyen.GetHashCode())
                    {
                        bc2cInfo.CapID = CapCoQuanViewChiTiet.CapUBNDHuyen.GetHashCode();
                    }
                    else if (capInfo.CapID == CapQuanLy.CapUBNDXa.GetHashCode())
                    {
                        bc2cInfo.CapID = CapCoQuanViewChiTiet.CapUBNDXa.GetHashCode();
                    }
                    else if (capInfo.CapID == CapQuanLy.CapSoNganh.GetHashCode())
                    {
                        bc2cInfo.CapID = CapCoQuanViewChiTiet.CapSoNganh.GetHashCode();
                    }

                    resultList.Add(bc2cInfo);
                }
                else
                {
                    bc2cInfoToanHuyen.DonVi = "" + capInfo.TenCap + "";
                    bc2cInfoToanHuyen.CssClass = "font-weight:bold;text-transform: uppercase";
                    //bc2cInfoToanHuyen.CapID = capInfo.CapID;
                    bc2cInfoToanHuyen.CapID = CapCoQuanViewChiTiet.ToanHuyen.GetHashCode();
                    resultList.Add(bc2cInfoToanHuyen);
                }

                //Tinh cac co quan thuoc cap SoNganh va cap Tinh
                if (capInfo.CapID == (int)CapQuanLy.CapUBNDTinh)
                {
                    List<CoQuanInfo> cqList = new List<CoQuanInfo>();
                    if (IdentityHelper.RoleID == (int)EnumChucVu.LanhDao)
                    {
                        var CoQuanChaPhuHop = new CoQuan().GetCoQuanByTinhID_New(IdentityHelper.TinhID);
                        cqList = new CoQuan().GetAllCapCon(CoQuanChaPhuHop.CoQuanID).ToList().Where(x => x.CapID == CapQuanLy.CapUBNDTinh.GetHashCode() && x.SuDungPM == true).ToList();
                        cqList.ForEach(x => x.TinhID = IdentityHelper.TinhID ?? 0);
                    }
                    else
                    {
                        cqList = new CoQuan().GetCoQuanByCapAndTinhID(capInfo.CapID, tinhID).Where(x => x.CoQuanID == IdentityHelper.CoQuanID && x.SuDungPM == true).ToList();
                    }
                    //List<CoQuanInfo> cqList = new List<CoQuanInfo>();
                    //if (IdentityHelper.RoleID == (int)EnumChucVu.LanhDao)
                    //{
                    //    cqList = new CoQuan().GetCoQuanByCapAndTinhID(capInfo.CapID, tinhID).ToList();
                    //}
                    //else
                    //{
                    //    cqList = new CoQuan().GetCoQuanByCapAndTinhID(capInfo.CapID, tinhID).Where(x => x.CoQuanID == IdentityHelper.CoQuanID).ToList();
                    //}
                    if (cqList.Count > 0)
                    {
                        List<BaoCaoPhanAnhKienNghiInfo> dtListFull = new BaoCaoPhanAnhKienNghi().GetBaoCao(startDate, endDate, cqList.ToList()).ToList();

                        foreach (CoQuanInfo cqInfo in cqList)
                        {
                            BaoCaoPhanAnhKienNghiInfo bc2cInfo2 = new BaoCaoPhanAnhKienNghiInfo();
                            bc2cInfo2.DonVi = cqInfo.TenCoQuan;
                            bc2cInfo2.CapID = capInfo.CapID;
                            bc2cInfo2.CoQuanID = cqInfo.CoQuanID;
                            //Lay tat ca don thu trong nam, bao gom ca don ki truoc + ki nay
                            DateTime firstDay = Season.GetFirstDayOfYear(startDate.Year);
                            //List<BaoCaoPhanAnhKienNghiDonThuInfo> dtList = dtLists.Where(x => x.CoQuanID == cqInfo.CoQuanID).ToList();
                            List<BaoCaoPhanAnhKienNghiInfo> dtList = dtListFull.Where(x => x.CoQuanID == cqInfo.CoQuanID).ToList();
                            //new BaoCaoPhanAnhKienNghi().GetBaoCao(startDate, endDate, cqInfo.CoQuanID).ToList();
                            Calculate(ref bc2cInfo2, dtList, startDate, endDate, 0);

                            resultList.Add(bc2cInfo2);

                            #region Cong so lieu tong
                            TinhTong(ref bc2cInfo, bc2cInfo2);
                            #endregion

                        }
                    }
                }
                else if (capInfo.CapID == (int)CapQuanLy.CapSoNganh)
                {
                    List<CoQuanInfo> cqList = new List<CoQuanInfo>();
                    if (IdentityHelper.RoleID == (int)EnumChucVu.LanhDao)
                    {
                        var CoQuanChaPhuHop = new CoQuan().GetCoQuanByTinhID_New(IdentityHelper.TinhID);
                        cqList = new CoQuan().GetAllCapCon(CoQuanChaPhuHop.CoQuanID).ToList().Where(x => x.CapID == CapQuanLy.CapSoNganh.GetHashCode() && x.SuDungPM == true).ToList();
                        cqList.ForEach(x => x.TinhID = IdentityHelper.TinhID ?? 0);
                    }
                    else
                    {
                        cqList = new CoQuan().GetCoQuanByCapAndTinhID(capInfo.CapID, tinhID).Where(x => x.CoQuanID == IdentityHelper.CoQuanID && x.SuDungPM == true).ToList();
                    }
                    //List<CoQuanInfo> cqList = new List<CoQuanInfo>();
                    //if (IdentityHelper.RoleID == (int)EnumChucVu.LanhDao)
                    //{
                    //    cqList = new CoQuan().GetCoQuanByCapAndTinhID(capInfo.CapID, tinhID).ToList();
                    //}
                    //else
                    //{
                    //    cqList = new CoQuan().GetCoQuanByCapAndTinhID(capInfo.CapID, tinhID).Where(x => x.CoQuanID == IdentityHelper.CoQuanID).ToList();
                    //}
                    if (cqList.Count > 0)
                    {
                        List<BaoCaoPhanAnhKienNghiInfo> dtListFull = new BaoCaoPhanAnhKienNghi().GetBaoCao(startDate, endDate, cqList.ToList()).ToList();

                        foreach (CoQuanInfo cqInfo in cqList)
                        {
                            BaoCaoPhanAnhKienNghiInfo bc2cInfo2 = new BaoCaoPhanAnhKienNghiInfo();
                            bc2cInfo2.DonVi = cqInfo.TenCoQuan;
                            bc2cInfo2.CapID = capInfo.CapID;
                            bc2cInfo2.CoQuanID = cqInfo.CoQuanID;
                            //Lay tat ca don thu trong nam, bao gom ca don ki truoc + ki nay
                            DateTime firstDay = Season.GetFirstDayOfYear(startDate.Year);
                            //List<BaoCaoPhanAnhKienNghiDonThuInfo> dtList = dtLists.Where(x => x.CoQuanID == cqInfo.CoQuanID).ToList();
                            List<BaoCaoPhanAnhKienNghiInfo> dtList = dtListFull.Where(x => x.CoQuanID == cqInfo.CoQuanID).ToList();
                            //new BaoCaoPhanAnhKienNghi().GetBaoCao(startDate, endDate, cqInfo.CoQuanID).ToList();
                            Calculate(ref bc2cInfo2, dtList, startDate, endDate, 0);

                            resultList.Add(bc2cInfo2);

                            #region Cong so lieu tong
                            TinhTong(ref bc2cInfo, bc2cInfo2);
                            #endregion

                        }
                    }
                }
                else if (capInfo.CapID == (int)CapQuanLy.ToanHuyen)
                {
                    List<HuyenInfo> huyenList = new Huyen().GetByTinh(IdentityHelper.TinhID ?? 0).ToList();
                    List<CoQuanInfo> cqListCapPhong = new CoQuan().GetCoQuanByCapAndTinhID((int)CapQuanLy.CapPhong, tinhID).ToList();
                    List<CoQuanInfo> cqListCapUBHuyen = new CoQuan().GetCoQuanByCapAndTinhID((int)CapQuanLy.CapUBNDHuyen, tinhID).ToList();
                    List<CoQuanInfo> cqListCapXa = new CoQuan().GetCoQuanByCapAndTinhID((int)CapQuanLy.CapUBNDXa, tinhID).ToList();
                    List<CoQuanInfo> cqListCapHuyen = new List<CoQuanInfo>();
                    foreach (CoQuanInfo itemCoQuan in cqListCapPhong)
                    {
                        cqListCapHuyen.Add(itemCoQuan);
                    }
                    foreach (CoQuanInfo itemCoQuan in cqListCapUBHuyen)
                    {
                        cqListCapHuyen.Add(itemCoQuan);
                    }
                    foreach (CoQuanInfo itemCoQuan in cqListCapXa)
                    {
                        cqListCapHuyen.Add(itemCoQuan);
                    }

                    List<int> ListID = new List<int>();
                    capList.ForEach(x => ListID.Add(x.CapID));
                    if (ListID.Contains((int)CapQuanLy.CapUBNDHuyen))
                    {
                        cqListCapHuyen = cqListCapHuyen.Where(x => (ListID.Contains(x.CapID) || x.CapID == (int)CapQuanLy.CapPhong) && x.SuDungPM == true).ToList();
                    }
                    else
                    {
                        cqListCapHuyen = cqListCapHuyen.Where(x => ListID.Contains(x.CapID) && x.SuDungPM == true).ToList();
                    }

                    List<BaoCaoPhanAnhKienNghiInfo> dtListFull = new BaoCaoPhanAnhKienNghi().GetBaoCao(startDate, endDate, cqListCapHuyen.ToList()).ToList();
                    foreach (HuyenInfo huyenInfo in huyenList)
                    {
                        BaoCaoPhanAnhKienNghiInfo bc2cInfo2 = new BaoCaoPhanAnhKienNghiInfo();
                        List<BaoCaoPhanAnhKienNghiDonThuInfo> totalDTList = new List<BaoCaoPhanAnhKienNghiDonThuInfo>();
                        bc2cInfo2.DonVi = huyenInfo.TenHuyen;
                        bc2cInfo2.CapID = capInfo.CapID;
                        bc2cInfo2.CoQuanID = huyenInfo.HuyenID;
                        List<CoQuanInfo> cqList = cqListCapHuyen.Where(x => x.HuyenID == huyenInfo.HuyenID && x.SuDungPM == true).ToList();
                        //List<BaoCaoPhanAnhKienNghiInfo> dtListFull = new BaoCaoPhanAnhKienNghi().GetBaoCao(startDate, endDate, cqList.ToList()).ToList();
                        foreach (CoQuanInfo cqInfo in cqList)
                        {
                            //DateTime firstDay = Season.GetFirstDayOfYear(startDate.Year);
                            //List<BaoCaoPhanAnhKienNghiDonThuInfo> dtList = dtLists.Where(x => x.CoQuanID == cqInfo.CoQuanID).ToList();
                            ////loc cac don thu dc tiep nhan o huyen
                            //List<BaoCaoPhanAnhKienNghiDonThuInfo> huyenDTList = new List<BaoCaoPhanAnhKienNghiDonThuInfo>();
                            //foreach (BaoCaoPhanAnhKienNghiDonThuInfo dtInfo in dtList)
                            //{
                            //    if (dtInfo.CQTiepNhanID == cqInfo.CoQuanID || dtInfo.CQTiepNhanID == 0)
                            //    {
                            //        huyenDTList.Add(dtInfo);
                            //    }
                            //}

                            //totalDTList.AddRange(huyenDTList);
                            //List<BaoCaoPhanAnhKienNghiInfo> dtList = new BaoCaoPhanAnhKienNghi().GetBaoCao(startDate, endDate, cqInfo.CoQuanID).ToList();
                            List<BaoCaoPhanAnhKienNghiInfo> dtList = dtListFull.Where(x => x.CoQuanID == cqInfo.CoQuanID).ToList();
                            Calculate(ref bc2cInfo2, dtList, startDate, endDate, 0);
                        }

                        //Calculate(ref bc2cInfo2, totalDTList, startDate, endDate, 0);
                        TinhTong(ref bc2cInfoToanHuyen, bc2cInfo2);
                    }
                }
                else if (capInfo.CapID == (int)CapQuanLy.CapUBNDHuyen)
                {
                    List<HuyenInfo> huyenList = new Huyen().GetByTinh(IdentityHelper.TinhID ?? 0).ToList();
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
                    List<BaoCaoPhanAnhKienNghiInfo> dtListFull = new BaoCaoPhanAnhKienNghi().GetBaoCao(startDate, endDate, cqListCapHuyen.ToList()).ToList();
                    foreach (HuyenInfo huyenInfo in huyenList)
                    {
                        BaoCaoPhanAnhKienNghiInfo bc2cInfo2 = new BaoCaoPhanAnhKienNghiInfo();
                        bc2cInfo2.DonVi = huyenInfo.TenHuyen;
                        bc2cInfo2.CapID = capInfo.CapID;

                        List<CoQuanInfo> cqList = cqListCapHuyen.Where(x => x.HuyenID == huyenInfo.HuyenID && x.SuDungPM == true).ToList();
                        bc2cInfo2.CoQuanID = cqList.FirstOrDefault().CoQuanChaID;
                        foreach (CoQuanInfo cqInfo in cqList)
                        {

                            //Lay tat ca don thu trong nam, bao gom ca don ki truoc + ki nay
                            DateTime firstDay = Season.GetFirstDayOfYear(startDate.Year);
                            List<BaoCaoPhanAnhKienNghiInfo> dtList = dtListFull.Where(x => x.CoQuanID == cqInfo.CoQuanID).ToList();
                            Calculate(ref bc2cInfo2, dtList, startDate, endDate, 0);

                        }

                        resultList.Add(bc2cInfo2);

                        #region Cong so lieu tong
                        TinhTong(ref bc2cInfo, bc2cInfo2);
                        #endregion
                    }

                }
                else if (capInfo.CapID == (int)CapQuanLy.CapUBNDXa)
                {
                    List<HuyenInfo> huyenList = new Huyen().GetByTinh(IdentityHelper.TinhID ?? 0).ToList();
                    List<CoQuanInfo> cqListCapXa = new CoQuan().GetCoQuanByCapAndTinhID((int)CapQuanLy.CapUBNDXa, tinhID).
                        Where(x => x.SuDungPM == true).ToList();
                    List<XaInfo> xaList = new List<XaInfo>();
                    List<BaoCaoPhanAnhKienNghiInfo> dtListFull = new BaoCaoPhanAnhKienNghi().GetBaoCao(startDate, endDate, cqListCapXa.ToList()).ToList();


                    foreach (HuyenInfo huyenInfo in huyenList)
                    {
                        BaoCaoPhanAnhKienNghiInfo bc2cInfo2huyen = new BaoCaoPhanAnhKienNghiInfo
                        {
                            DonVi=huyenInfo.TenDayDu,                            
                            CoQuanID=huyenInfo.HuyenID,                            
                        };                        
                        
                        resultList.Add(bc2cInfo2huyen);                        
                        List<CoQuanInfo> cqList = cqListCapXa.Where(x => x.HuyenID == huyenInfo.HuyenID && x.SuDungPM == true).ToList();

                        //List<BaoCaoPhanAnhKienNghiInfo> dtListFull = new BaoCaoPhanAnhKienNghi().GetBaoCao(startDate, endDate, cqList.ToList()).ToList();
                        foreach (CoQuanInfo cqInfo in cqList)
                        {
                            List<BaoCaoPhanAnhKienNghiDonThuInfo> totalDTList = new List<BaoCaoPhanAnhKienNghiDonThuInfo>();
                            BaoCaoPhanAnhKienNghiInfo bc2cInfo2 = new BaoCaoPhanAnhKienNghiInfo();
                            bc2cInfo2.DonVi = cqInfo.TenCoQuan + " (" + huyenInfo.TenDayDu + ")";
                            bc2cInfo2.CapID = capInfo.CapID;
                            bc2cInfo2.CoQuanID = cqInfo.CoQuanID;
                            DateTime firstDay = Season.GetFirstDayOfYear(startDate.Year);
                            //List<BaoCaoPhanAnhKienNghiDonThuInfo> dtList = dtLists.Where(x => x.CoQuanID == cqInfo.CoQuanID).ToList();
                            //List<BaoCaoPhanAnhKienNghiDonThuInfo> xaDTList = new List<BaoCaoPhanAnhKienNghiDonThuInfo>();
                            //foreach (BaoCaoPhanAnhKienNghiDonThuInfo dtInfo in dtList)
                            //{
                            //    {
                            //        xaDTList.Add(dtInfo);
                            //    }
                            //}
                            //totalDTList.AddRange(xaDTList);
                            //List<BaoCaoPhanAnhKienNghiInfo> dtList = new BaoCaoPhanAnhKienNghi().GetBaoCao(startDate, endDate, cqInfo.CoQuanID).ToList();
                            List<BaoCaoPhanAnhKienNghiInfo> dtList = dtListFull.Where(x => x.CoQuanID == cqInfo.CoQuanID).ToList();
                            Calculate(ref bc2cInfo2, dtList, startDate, endDate, 0);
                            resultList.Add(bc2cInfo2);

                            #region Cong so lieu tong
                            bc2cInfo.Col1Data += bc2cInfo2.Col1Data;
                            bc2cInfo.Col2Data += bc2cInfo2.Col2Data;
                            bc2cInfo.Col3Data += bc2cInfo2.Col3Data;
                            bc2cInfo.Col4Data += bc2cInfo2.Col4Data;
                            bc2cInfo.Col5Data += bc2cInfo2.Col5Data;
                            bc2cInfo.Col6Data += bc2cInfo2.Col6Data;
                            bc2cInfo.Col7Data += bc2cInfo2.Col7Data;
                            bc2cInfo.Col8Data += bc2cInfo2.Col8Data;
                            bc2cInfo.Col9Data += bc2cInfo2.Col9Data;
                            bc2cInfo.Col10Data += bc2cInfo2.Col10Data;
                            bc2cInfo.Col11Data += bc2cInfo2.Col11Data;
                            bc2cInfo.Col12Data += bc2cInfo2.Col12Data;
                            bc2cInfo.Col13Data += bc2cInfo2.Col13Data;
                            bc2cInfo.Col14Data += bc2cInfo2.Col14Data;
                            bc2cInfo.Col15Data += bc2cInfo2.Col15Data;
                            bc2cInfo.Col16Data += bc2cInfo2.Col16Data;
                            bc2cInfo.Col17Data += bc2cInfo2.Col17Data;
                            bc2cInfo.Col18Data += bc2cInfo2.Col18Data;
                            bc2cInfo.Col19Data += bc2cInfo2.Col19Data;
                            bc2cInfo.Col20Data += bc2cInfo2.Col20Data;
                            bc2cInfo.Col21Data += bc2cInfo2.Col21Data;
                            bc2cInfo.Col22Data += bc2cInfo2.Col22Data;
                            bc2cInfo.Col23Data += bc2cInfo2.Col23Data;
                            bc2cInfo.Col24Data += bc2cInfo2.Col24Data;
                            bc2cInfo.Col25Data += bc2cInfo2.Col25Data;
                            bc2cInfo.Col26Data += bc2cInfo2.Col26Data;
                            bc2cInfo.Col27Data += bc2cInfo2.Col27Data;
                            bc2cInfo.Col28Data += bc2cInfo2.Col28Data;
                            bc2cInfo.SlCol10 += bc2cInfo2.SlCol10;
                            bc2cInfo.SlCol11 += bc2cInfo2.SlCol11;
                            bc2cInfo.SlCol12 += bc2cInfo2.SlCol12;
                            bc2cInfo.SlCol13 += bc2cInfo2.SlCol13;
                            bc2cInfo.SlCol14 += bc2cInfo2.SlCol14;
                            bc2cInfo.SlCol15 += bc2cInfo2.SlCol15;
                            bc2cInfo.SlCol16 += bc2cInfo2.SlCol16;
                            bc2cInfo.SlCol21 += bc2cInfo2.SlCol21;
                            bc2cInfo.SlCol22 += bc2cInfo2.SlCol22;
                            bc2cInfo.SlCol23 += bc2cInfo2.SlCol23;
                            bc2cInfo.SlCol24 += bc2cInfo2.SlCol24;
                            bc2cInfo.SlCol25 += bc2cInfo2.SlCol25;
                            bc2cInfo.SlCol26 += bc2cInfo2.SlCol26;
                            bc2cInfo.SlCol27 += bc2cInfo2.SlCol27;
                            bc2cInfo.SlCol28 += bc2cInfo2.SlCol28;
                            #endregion

                            TinhTong(ref bc2cInfo2huyen, bc2cInfo2);

                        }

                        
                    }


                }

                #region Cong so lieu tong
                if (flag == true)
                {
                    TinhTong(ref bcInfo, bc2cInfo);
                }
                #endregion
            }
        }

        public static void CalculateBaoCaoCapSo(ref List<BaoCaoPhanAnhKienNghiInfo> resultList, DateTime startDate, DateTime endDate, IdentityHelper IdentityHelper)
        {

            List<CoQuanInfo> CoQuanInfoList = new List<CoQuanInfo>();
            CoQuanInfo cqInfo = new CoQuan().GetCoQuanByID(IdentityHelper.CoQuanID ?? 0);

            CoQuanInfoList.Add(cqInfo);

            BaoCaoPhanAnhKienNghiInfo bc2cInfo2 = new BaoCaoPhanAnhKienNghiInfo();
            bc2cInfo2.DonVi = cqInfo.TenCoQuan;
            //bc2cInfo2.CapID = (int)CapQuanLy.CapSoNganh;
            bc2cInfo2.CapID = CapCoQuanViewChiTiet.CapSoNganh.GetHashCode();
            //Lay tat ca don thu trong nam, bao gom ca don ki truoc + ki nay
            //DateTime firstDay = Season.GetFirstDayOfYear(startDate.Year);
            DateTime firstDays = startDate;
            //List<BaoCaoPhanAnhKienNghiDonThuInfo> dtList = new BaoCaoPhanAnhKienNghi().GetDonThu(firstDays, endDate, cqInfo.CoQuanID).ToList();
            List<BaoCaoPhanAnhKienNghiInfo> dtListFull1 = new BaoCaoPhanAnhKienNghi().GetBaoCao(startDate, endDate, CoQuanInfoList.ToList()).ToList();
            List<BaoCaoPhanAnhKienNghiInfo> dtList = dtListFull1.Where(x => x.CoQuanID == cqInfo.CoQuanID).ToList();
            Calculate(ref bc2cInfo2, dtList, startDate, endDate, 0);
            //List<BaoCaoPhanAnhKienNghiInfo> dtList = new BaoCaoPhanAnhKienNghi().GetBaoCao(startDate, endDate, cqInfo.CoQuanID).ToList();
            //Calculate(ref bc2cInfo2, dtList, startDate, endDate, 0);
            resultList.Add(bc2cInfo2);


        }

        public static void CalculateBaoCaoCapHuyen(ref List<BaoCaoPhanAnhKienNghiInfo> resultList, DateTime startDate, DateTime endDate, int tinhID, IdentityHelper IdentityHelper)
        {
            BaoCaoPhanAnhKienNghiInfo totalInfo = new BaoCaoPhanAnhKienNghiInfo();
            totalInfo.DonVi = "<b>TOÀN HUYỆN</b>";
            //totalInfo.CapID = (int)CapQuanLy.ToanHuyen;
            totalInfo.CapID = CapCoQuanViewChiTiet.ToanHuyen.GetHashCode();
            totalInfo.CoQuanID = IdentityHelper.HuyenID ?? 0;
            totalInfo.IsCoQuan = false;
            resultList.Add(totalInfo);

            #region tinh cap huyen
            BaoCaoPhanAnhKienNghiInfo caphuyenInfo = new BaoCaoPhanAnhKienNghiInfo();
            caphuyenInfo.DonVi = "<b>Cấp UBND Huyện</b>";
            //caphuyenInfo.CapID = (int)CapQuanLy.CapUBNDHuyen;
            caphuyenInfo.CapID = CapCoQuanViewChiTiet.CapUBNDHuyen.GetHashCode();
            caphuyenInfo.CoQuanID = IdentityHelper.HuyenID ?? 0;
            caphuyenInfo.IsCoQuan = false;
            resultList.Add(caphuyenInfo);
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
            List<CoQuanInfo> cqHuyenList = new List<CoQuanInfo>();
            if (IdentityHelper.RoleID == (int)EnumChucVu.LanhDao)
            {
                cqHuyenList = cqListCapHuyen.Where(x => x.HuyenID == IdentityHelper.HuyenID && x.SuDungPM == true).ToList();
            }
            else
            {
                cqHuyenList = cqListCapHuyen.Where(x => x.CoQuanID == IdentityHelper.CoQuanID && x.SuDungPM == true).ToList();
            }
            //Lay tat ca don thu trong nam, bao gom ca don ki truoc + ki nay
            //DateTime firstDays = Season.GetFirstDayOfYear(startDate.Year);
            DateTime firstDays = startDate;
            //List<BaoCaoPhanAnhKienNghiDonThuInfo> dtLists = new BaoCaoPhanAnhKienNghi().GetDonThu(firstDays, endDate, 0).ToList();
            List<BaoCaoPhanAnhKienNghiInfo> dtListFull = new BaoCaoPhanAnhKienNghi().GetBaoCao(startDate, endDate, cqHuyenList.ToList()).ToList();
            foreach (CoQuanInfo cqInfo in cqHuyenList)
            {
                BaoCaoPhanAnhKienNghiInfo bc2cInfo = new BaoCaoPhanAnhKienNghiInfo();
                bc2cInfo.DonVi = cqInfo.TenCoQuan;
                bc2cInfo.CapID = cqInfo.CapID;
                //bc2cInfo.CapID = (int)CapQuanLy.CapUBNDHuyen;
                //bc2cInfo.CapID = CapCoQuanViewChiTiet.CapUBNDHuyen.GetHashCode();
                bc2cInfo.CoQuanID = cqInfo.CoQuanID;
                bc2cInfo.IsCoQuan = true;
                List<BaoCaoPhanAnhKienNghiInfo> dtList = dtListFull.Where(x => x.CoQuanID == cqInfo.CoQuanID).ToList();
                //List<BaoCaoPhanAnhKienNghiInfo> dtList = new BaoCaoPhanAnhKienNghi().GetBaoCao(startDate, endDate, cqInfo.CoQuanID).ToList();
                Calculate(ref bc2cInfo, dtList, startDate, endDate, 0);
                resultList.Add(bc2cInfo);

                TinhTong(ref caphuyenInfo, bc2cInfo);
            }
            #endregion

            #region tinh cap xa
            if (IdentityHelper.RoleID == (int)EnumChucVu.LanhDao)
            {
                BaoCaoPhanAnhKienNghiInfo capxaInfo = new BaoCaoPhanAnhKienNghiInfo();
                capxaInfo.DonVi = "<b>Cấp xã</b>";
                //capxaInfo.CapID = (int)CapQuanLy.CapUBNDXa;
                capxaInfo.CapID = CapCoQuanViewChiTiet.CapUBNDXa.GetHashCode();
                //capxaInfo.CoQuanID = IdentityHelper.HuyenID;
                capxaInfo.IsCoQuan = false;
                resultList.Add(capxaInfo);

                HuyenInfo huyenInfo = new Huyen().GetByID(IdentityHelper.HuyenID ?? 0);
                List<CoQuanInfo> cqListCapXa = new CoQuan().GetCoQuanByCapAndTinhID((int)CapQuanLy.CapUBNDXa, tinhID).ToList();
                List<CoQuanInfo> cqList = cqListCapXa.Where(x => x.HuyenID == huyenInfo.HuyenID && x.SuDungPM == true).ToList();
                List<BaoCaoPhanAnhKienNghiInfo> dtListFull1 = new BaoCaoPhanAnhKienNghi().GetBaoCao(startDate, endDate, cqList.ToList()).ToList();
                foreach (CoQuanInfo cqInfo in cqList)
                {
                    //List<BaoCaoPhanAnhKienNghiDonThuInfo> totalDTList = new List<BaoCaoPhanAnhKienNghiDonThuInfo>();
                    //List<BaoCaoPhanAnhKienNghiInfo> dt_baocao = new BaoCaoPhanAnhKienNghi().GetBaoCao(firstDays, endDate, cqInfo.CoQuanID).ToList();
                    List<BaoCaoPhanAnhKienNghiInfo> dt_baocao = dtListFull1.Where(x => x.CoQuanID == cqInfo.CoQuanID).ToList();
                    BaoCaoPhanAnhKienNghiInfo bc2cInfo2 = new BaoCaoPhanAnhKienNghiInfo();
                    bc2cInfo2.DonVi = cqInfo.TenCoQuan + " (" + huyenInfo.TenHuyen + ")";
                    bc2cInfo2.CapID = cqInfo.CapID;
                    bc2cInfo2.CoQuanID = cqInfo.CoQuanID;
                    //List<BaoCaoPhanAnhKienNghiDonThuInfo> dtList = new BaoCaoPhanAnhKienNghi().GetDonThu(firstDays, endDate, cqInfo.CoQuanID).ToList();
                    //List<BaoCaoPhanAnhKienNghiDonThuInfo> xaDTList = new List<BaoCaoPhanAnhKienNghiDonThuInfo>();
                    //foreach (BaoCaoPhanAnhKienNghiDonThuInfo dtInfo in dtList)
                    //{
                    //    //if (dtInfo.CQTiepNhanID != cqInfo.CoQuanID && dtInfo.CQTiepNhanID != 0)
                    //    {
                    //        xaDTList.Add(dtInfo);
                    //    }
                    //}

                    //totalDTList.AddRange(xaDTList);
                    Calculate(ref bc2cInfo2, dt_baocao, startDate, endDate, 0);
                    resultList.Add(bc2cInfo2);

                    #region Cong so lieu tong
                    capxaInfo.Col1Data += bc2cInfo2.Col1Data;
                    capxaInfo.Col2Data += bc2cInfo2.Col2Data;
                    capxaInfo.Col3Data += bc2cInfo2.Col3Data;
                    capxaInfo.Col4Data += bc2cInfo2.Col4Data;
                    capxaInfo.Col5Data += bc2cInfo2.Col5Data;
                    capxaInfo.Col6Data += bc2cInfo2.Col6Data;
                    capxaInfo.Col7Data += bc2cInfo2.Col7Data;
                    capxaInfo.Col8Data += bc2cInfo2.Col8Data;
                    capxaInfo.Col9Data += bc2cInfo2.Col9Data;
                    capxaInfo.Col10Data += bc2cInfo2.Col10Data;
                    capxaInfo.Col11Data += bc2cInfo2.Col11Data;
                    capxaInfo.Col12Data += bc2cInfo2.Col12Data;
                    capxaInfo.Col13Data += bc2cInfo2.Col13Data;
                    capxaInfo.Col14Data += bc2cInfo2.Col14Data;
                    capxaInfo.Col15Data += bc2cInfo2.Col15Data;
                    capxaInfo.Col16Data += bc2cInfo2.Col16Data;
                    capxaInfo.Col17Data += bc2cInfo2.Col17Data;
                    capxaInfo.Col18Data += bc2cInfo2.Col18Data;
                    capxaInfo.Col19Data += bc2cInfo2.Col19Data;
                    capxaInfo.Col20Data += bc2cInfo2.Col20Data;
                    capxaInfo.Col21Data += bc2cInfo2.Col21Data;
                    capxaInfo.Col22Data += bc2cInfo2.Col22Data;
                    capxaInfo.Col23Data += bc2cInfo2.Col23Data;
                    capxaInfo.Col24Data += bc2cInfo2.Col24Data;
                    capxaInfo.Col25Data += bc2cInfo2.Col25Data;
                    capxaInfo.Col26Data += bc2cInfo2.Col26Data;
                    capxaInfo.Col27Data += bc2cInfo2.Col27Data;
                    capxaInfo.Col28Data += bc2cInfo2.Col28Data;
                    capxaInfo.SlCol10 += capxaInfo.SlCol10;
                    capxaInfo.SlCol11 += capxaInfo.SlCol11;
                    capxaInfo.SlCol12 += capxaInfo.SlCol12;
                    capxaInfo.SlCol13 += capxaInfo.SlCol13;
                    capxaInfo.SlCol14 += capxaInfo.SlCol14;
                    capxaInfo.SlCol15 += capxaInfo.SlCol15;
                    capxaInfo.SlCol16 += capxaInfo.SlCol16;
                    capxaInfo.SlCol21 += capxaInfo.SlCol21;
                    capxaInfo.SlCol22 += capxaInfo.SlCol22;
                    capxaInfo.SlCol23 += capxaInfo.SlCol23;
                    capxaInfo.SlCol24 += capxaInfo.SlCol24;
                    capxaInfo.SlCol25 += capxaInfo.SlCol25;
                    capxaInfo.SlCol26 += capxaInfo.SlCol26;
                    capxaInfo.SlCol27 += capxaInfo.SlCol27;
                    capxaInfo.SlCol28 += capxaInfo.SlCol28;
                    #endregion
                }

                TinhTong(ref totalInfo, capxaInfo);
            }

            #endregion

            TinhTong(ref totalInfo, caphuyenInfo);

        }

        public static void CalculateBaoCaoCapXa(ref List<BaoCaoPhanAnhKienNghiInfo> resultList, DateTime startDate, DateTime endDate, IdentityHelper IdentityHelper)
        {
            CoQuanInfo cqInfo = new CoQuan().GetCoQuanByID(IdentityHelper.CoQuanID ?? 0);
            List<CoQuanInfo> CoQuanInfoList = new List<CoQuanInfo>();
            CoQuanInfoList.Add(cqInfo);
            BaoCaoPhanAnhKienNghiInfo bc2cInfo2 = new BaoCaoPhanAnhKienNghiInfo();
            bc2cInfo2.DonVi = cqInfo.TenCoQuan;
            //bc2cInfo2.CapID = (int)CapQuanLy.CapUBNDXa;

            bc2cInfo2.CoQuanID = cqInfo.CoQuanID;
            //Lay tat ca don thu trong nam, bao gom ca don ki truoc + ki nay
            //DateTime firstDays = Season.GetFirstDayOfYear(startDate.Year);
            DateTime firstDays = startDate;
            //List<BaoCaoPhanAnhKienNghiDonThuInfo> dtList = new BaoCaoPhanAnhKienNghi().GetDonThu(firstDays, endDate, cqInfo.CoQuanID).ToList();
            //List<BaoCaoPhanAnhKienNghiInfo> dt_baocao = new BaoCaoPhanAnhKienNghi().GetBaoCao(startDate, endDate, cqInfo.CoQuanID).ToList();
            List<BaoCaoPhanAnhKienNghiInfo> dtListFull1 = new BaoCaoPhanAnhKienNghi().GetBaoCao(startDate, endDate, CoQuanInfoList.ToList()).ToList();
            List<BaoCaoPhanAnhKienNghiInfo> dt_baocao = dtListFull1.Where(x => x.CoQuanID == cqInfo.CoQuanID).ToList();
            Calculate(ref bc2cInfo2, dt_baocao, startDate, endDate, 0);
            resultList.Add(bc2cInfo2);
        }

        public static void CalculateBaoCaoCapTrungUong(ref List<BaoCaoPhanAnhKienNghiInfo> resultList, DateTime startDate, DateTime endDate, int tinhID)
        {
            BaoCaoPhanAnhKienNghiInfo bcInfo = new BaoCaoPhanAnhKienNghiInfo();
            List<CapInfo> capList = new CapDAL().GetAll().ToList();
            foreach (CapInfo capInfo in capList)
            {
                if (capInfo.CapID == (int)CapQuanLy.CapUBNDTinh)
                {
                    BaoCaoPhanAnhKienNghiInfo bc2cInfo = new BaoCaoPhanAnhKienNghiInfo();
                    bc2cInfo.DonVi = "<b style='text-transform: uppercase'>" + capInfo.TenCap + "</b>";
                    //bc2cInfo.CapID = (int)CapQuanLy.CapTrungUong;
                    bc2cInfo.CapID = CapCoQuanViewChiTiet.CapTrungUong.GetHashCode();
                    resultList.Add(bc2cInfo);
                    List<CoQuanInfo> cqList = new CoQuan().GetCoQuanByCapAndTinhID(capInfo.CapID, tinhID).ToList();
                    List<BaoCaoPhanAnhKienNghiInfo> dtListFull = new BaoCaoPhanAnhKienNghi().GetBaoCao(startDate, endDate, cqList.ToList()).ToList();
                    if (cqList.Count > 0)
                    {
                        foreach (CoQuanInfo cqInfo in cqList)
                        {
                            BaoCaoPhanAnhKienNghiInfo bc2cInfo2 = new BaoCaoPhanAnhKienNghiInfo();
                            bc2cInfo2.DonVi = cqInfo.TenCoQuan;

                            //Lay tat ca don thu trong nam, bao gom ca don ki truoc + ki nay
                            //DateTime firstDays = Season.GetFirstDayOfYear(startDate.Year);
                            //DateTime firstDays = startDate;
                            List<BaoCaoPhanAnhKienNghiDonThuInfo> dtList = new BaoCaoPhanAnhKienNghi().GetDonThuByTinh(startDate, endDate, cqInfo.TinhID).ToList();
                            //List<BaoCaoPhanAnhKienNghiInfo> dtList = new BaoCaoPhanAnhKienNghi().GetBaoCao(startDate, endDate, cqInfo.CoQuanID).ToList();
                            // List<BaoCaoPhanAnhKienNghiInfo> dtList = dtListFull.Where(x => x.CoQuanID == cqInfo.CoQuanID).ToList();
                            // Calculate(ref bc2cInfo2, dtList, startDate, endDate, 0);
                            resultList.Add(bc2cInfo2);

                        }
                    }
                }
            }
        }

        private static void Calculate(ref BaoCaoPhanAnhKienNghiInfo bc2cInfo, List<BaoCaoPhanAnhKienNghiInfo> dtList, DateTime startDate, DateTime endDate, int CoQuanID)
        {
            BaoCaoPhanAnhKienNghiInfo dt_test = new BaoCaoPhanAnhKienNghiInfo();
            if (dtList != null && dtList.Count > 0)
            {
                dt_test = dtList.FirstOrDefault();
                bc2cInfo.Col2Data += dt_test.Col2Data;
                bc2cInfo.Col3Data += dt_test.Col3Data;
                bc2cInfo.Col4Data += dt_test.Col4Data;
                bc2cInfo.Col6Data += dt_test.Col6Data;
                bc2cInfo.Col7Data += dt_test.Col7Data;
                bc2cInfo.Col8Data += dt_test.Col8Data;
                bc2cInfo.Col9Data += dt_test.Col9Data;
                bc2cInfo.Col10Data += dt_test.Col10Data;
                bc2cInfo.Col11Data += dt_test.Col11Data;
                bc2cInfo.Col12Data += dt_test.Col12Data;
                bc2cInfo.Col13Data += dt_test.Col13Data;
                bc2cInfo.Col14Data += dt_test.Col14Data;
                bc2cInfo.Col15Data += dt_test.Col15Data;
                bc2cInfo.Col16Data += dt_test.Col16Data;
                bc2cInfo.Col17Data += dt_test.Col17Data;
                bc2cInfo.Col18Data += dt_test.Col18Data;
                bc2cInfo.Col19Data += dt_test.Col19Data;
                bc2cInfo.Col20Data += dt_test.Col20Data;
                bc2cInfo.Col21Data += dt_test.Col21Data;
                bc2cInfo.Col22Data += dt_test.Col22Data;
                bc2cInfo.Col23Data += dt_test.Col23Data;
                bc2cInfo.Col24Data += dt_test.Col24Data;
                bc2cInfo.Col25Data += dt_test.Col25Data;
                bc2cInfo.Col26Data += dt_test.Col26Data;
                bc2cInfo.Col27Data += dt_test.Col27Data;
                bc2cInfo.Col28Data += dt_test.Col28Data;
                bc2cInfo.SlCol10 += dt_test.SlCol10;
                bc2cInfo.SlCol11 += dt_test.SlCol11;
                bc2cInfo.SlCol12 += dt_test.SlCol12;
                bc2cInfo.SlCol13 += dt_test.SlCol13;
                bc2cInfo.SlCol14 += dt_test.SlCol14;
                bc2cInfo.SlCol15 += dt_test.SlCol15;
                bc2cInfo.SlCol16 += dt_test.SlCol16;
                bc2cInfo.SlCol21 += dt_test.SlCol21;
                bc2cInfo.SlCol22 += dt_test.SlCol22;
                bc2cInfo.SlCol23 += dt_test.SlCol23;
                bc2cInfo.SlCol24 += dt_test.SlCol24;
                bc2cInfo.SlCol25 += dt_test.SlCol25;
                bc2cInfo.SlCol26 += dt_test.SlCol26;
                bc2cInfo.SlCol27 += dt_test.SlCol27;
                bc2cInfo.SlCol28 += dt_test.SlCol28;
            }

            //foreach (BaoCaoPhanAnhKienNghiDonThuInfo dtInfo in dtList)
            //{
            //    if (dtInfo.HuongGiaiQuyetID == (int)HuongGiaiQuyetEnum.DeXuatThuLy)
            //    {
            //        //Don thuoc tham quyen
            //        //Don nhan trong ky
            //        if (dtInfo.NgayNhapDon >= startDate)
            //        {
            //            bc2cInfo.Col2Data++;
            //        }
            //        //Don ton ky truoc chuyen sang: ngay nhap don < startDate va chua co ket qua hoac ket qua xay ra trng ky bao cao
            //        else if (dtInfo.NgayRaKQ == DateTime.MinValue || dtInfo.NgayRaKQ >= startDate)
            //        {
            //            bc2cInfo.Col3Data++;
            //        }


            //        //Xet cac don trong ky + don ton ky truoc chuyen sang
            //        if (dtInfo.NgayNhapDon >= startDate || dtInfo.NgayRaKQ == DateTime.MinValue || dtInfo.NgayRaKQ >= startDate)
            //        {
            //            //So don thuoc tham quyen da giai quyet: co ket qua hoac da rut don
            //            if (dtInfo.LoaiKetQuaID > 0 || dtInfo.RutDonID > 0)
            //            {
            //                //so don & so vu viec
            //                bc2cInfo.Col4Data++;
            //                //so vu viec rut don
            //                if (dtInfo.LoaiKetQuaID <= 0 && dtInfo.RutDonID > 0)
            //                {
            //                    bc2cInfo.Col6Data++;
            //                }
            //            }

            //            //phan tich ket qua su viec
            //            if (dtInfo.PhanTichKQID == (int)PhanTichKQEnum.Dung)
            //            {
            //                bc2cInfo.Col7Data++;
            //            }
            //            else if (dtInfo.PhanTichKQID == (int)PhanTichKQEnum.Sai)
            //            {
            //                bc2cInfo.Col8Data++;
            //            }
            //            else if (dtInfo.PhanTichKQID == (int)PhanTichKQEnum.DungMotPhan)
            //            {
            //                bc2cInfo.Col9Data++;
            //            }

            //            //Kien nghi thu hoi
            //            if (dtInfo.LoaiKetQuaID == Constant.KienNghiThuHoiChoNhaNuoc)
            //            {
            //                bc2cInfo.Col10Data += dtInfo.TienPhaiThu;
            //                bc2cInfo.Col11Data += dtInfo.DatPhaiThu;
            //            }
            //            //tra lai cho cong dan
            //            else if (dtInfo.LoaiKetQuaID == Constant.TraLaiChoCongDan)
            //            {
            //                bc2cInfo.Col12Data += dtInfo.TienPhaiThu;
            //                bc2cInfo.Col13Data += dtInfo.DatPhaiThu;
            //                bc2cInfo.Col14Data += dtInfo.SoNguoiDuocTraQuyenLoi;
            //            }
            //            //xu ly hanh chinh
            //            else if (dtInfo.LoaiKetQuaID == Constant.KienNghiXuLyHanhChinh)
            //            {
            //                bc2cInfo.Col15Data += dtInfo.SoDoiTuongBiXuLy;
            //                bc2cInfo.Col16Data += dtInfo.SoDoiTuongDaBiXuLy;
            //            }
            //            //Chap hanh thoi gian giai quyet theo quy dinh                
            //            if (dtInfo.ThiHanhID > 0)
            //            {
            //                if ((dtInfo.NgayThiHanh - dtInfo.NgayRaKQ).TotalDays <= 15)
            //                {
            //                    bc2cInfo.Col17Data++;
            //                }
            //                else
            //                {
            //                    bc2cInfo.Col18Data++;
            //                }
            //            }

            //            //Thi hanh quyet dinh giai quyet khieu nai
            //            if (dtInfo.LoaiKetQuaID != 0)
            //            {
            //                //SO vu phai giai quyet
            //                bc2cInfo.Col19Data++;
            //                //Da thuc hien
            //                if (dtInfo.ThiHanhID > 0)
            //                {
            //                    bc2cInfo.Col20Data++;
            //                    if (dtInfo.LoaiKetQuaID == Constant.KienNghiThuHoiChoNhaNuoc)
            //                    {
            //                        bc2cInfo.Col21Data += dtInfo.TienPhaiThu;
            //                        bc2cInfo.Col22Data += dtInfo.DatPhaiThu;
            //                        bc2cInfo.Col23Data += dtInfo.TienDaThu;
            //                        bc2cInfo.Col24Data += dtInfo.DatDaThu;
            //                    }
            //                    else if (dtInfo.LoaiKetQuaID == Constant.TraLaiChoCongDan)
            //                    {
            //                        bc2cInfo.Col25Data += dtInfo.TienPhaiThu;
            //                        bc2cInfo.Col26Data += dtInfo.DatPhaiThu;
            //                        bc2cInfo.Col27Data += dtInfo.TienDaThu;
            //                        bc2cInfo.Col28Data += dtInfo.DatDaThu;
            //                    }
            //                }
            //            }

            //        }
            //    }
            //}
            bc2cInfo.Col1Data = bc2cInfo.Col2Data + bc2cInfo.Col3Data;
            bc2cInfo.Col5Data = bc2cInfo.Col4Data;
        }

        private static void TinhTong(ref BaoCaoPhanAnhKienNghiInfo bc2cInfo, BaoCaoPhanAnhKienNghiInfo bc2cInfo2)
        {
            Type type = bc2cInfo.GetType();
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (property.PropertyType == typeof(int) && property.Name != "CapID" && property.Name != "CoQuanID")
                {
                    int newValue = Utils.ConvertToInt32(property.GetValue(bc2cInfo, null), 0) + Utils.ConvertToInt32(property.GetValue(bc2cInfo2, null), 0);
                    property.SetValue(bc2cInfo, newValue, null);
                }
                if (property.PropertyType == typeof(decimal))
                {
                    decimal newValue = Utils.GetDecimal(property.GetValue(bc2cInfo, null), 0) + Utils.GetDecimal(property.GetValue(bc2cInfo2, null), 0);
                    property.SetValue(bc2cInfo, newValue, null);
                }
            }
        }
    }
}
