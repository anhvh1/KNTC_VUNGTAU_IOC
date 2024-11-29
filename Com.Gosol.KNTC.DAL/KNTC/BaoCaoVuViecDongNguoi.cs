using Com.Gosol.KNTC.Models.BaoCao;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Ultilities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Com.Gosol.KNTC.DAL.KNTC
{
    public static class BaoCaoVuViecDongNguoi
    {
        public static void CalculateBaoCaoCapTinh(ref List<BaoCaoVuViecDongNguoiInfo> resultList, DateTime startDate, DateTime endDate, List<CapInfo> capList, int tinhID, IdentityHelper IdentityHelper, string ContentRootPath)
        {
            DateTime firstDay = Season.GetFirstDayOfYear(startDate.Year);
            BaoCaoVuViecDongNguoiInfo bcInfo = new BaoCaoVuViecDongNguoiInfo();
            bcInfo.DonVi = "<b style='text-transform: uppercase'>Toàn tỉnh</b>";
            bcInfo.CssClass = "highlight";
            bcInfo.CapID = 0;
            resultList.Add(bcInfo);
            string filename = ContentRootPath + "\\Upload\\" + IdentityHelper.CanBoID + "_CoQuan.xml";
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            using (FileStream file = System.IO.File.Create(filename))
            {

            }
            XDocument doc = new XDocument();

            foreach (CapInfo capInfo in capList)
            {
                BaoCaoVuViecDongNguoiInfo bc2aInfo = new BaoCaoVuViecDongNguoiInfo();
                if (capInfo.CapID != (int)CapQuanLy.CapSoNganh)
                {
                    bc2aInfo.DonVi = "<b style='text-transform: uppercase'>" + capInfo.TenCap + "</b>";
                    bc2aInfo.CapID = capInfo.CapID;
                    bc2aInfo.CssClass = "highlight";
                    resultList.Add(bc2aInfo);
                }


                List<BaoCaoVuViecDongNguoiInfo> dtList = new BaoCaoVuViecDongNguoi_DAL().GET_DONTHUVUVIECDONGNGUOI(startDate, endDate).ToList();
                if (capInfo.CapID == (int)CapQuanLy.CapUBNDTinh || capInfo.CapID == (int)CapQuanLy.CapSoNganh)
                {
                    List<CoQuanInfo> cqList = new List<CoQuanInfo>();
                    if (IdentityHelper.RoleID == (int)EnumChucVu.LanhDao)
                    {
                        cqList = new CoQuan().GetCoQuanByCapAndTinhID(capInfo.CapID, tinhID).ToList();
                    }
                    else
                    {
                        cqList = new CoQuan().GetCoQuanByCapAndTinhID(capInfo.CapID, tinhID).Where(x => x.CoQuanID == IdentityHelper.CoQuanID).ToList();
                    }

                    if (cqList.Count > 0)
                    {
                        doc =
                           new XDocument(
                           new XElement("LogConfig", cqList.Select(x =>
          new XElement("SystemLog", new XElement("CoQuan", new XAttribute("CoQuanID", x.CoQuanID),
                         new XAttribute("CapID", x.CapID), new XAttribute("CoQuanChaID", x.CoQuanChaID),
                         new XAttribute("SuDungPM", x.SuDungPM))))));

                        doc.Save(filename);
                        foreach (CoQuanInfo cqInfo in cqList)
                        {
                            if (cqInfo.TinhID == IdentityHelper.TinhID)
                            {
                                BaoCaoVuViecDongNguoiInfo bc2aInfo2 = new BaoCaoVuViecDongNguoiInfo();
                                bc2aInfo2.DonVi = cqInfo.TenCoQuan;
                                bc2aInfo2.CapID = capInfo.CapID;
                                bc2aInfo2.CoQuanID = cqInfo.CoQuanID;
                                List<BaoCaoVuViecDongNguoiInfo> dtLists = dtList.Where(f => f.CoQuanID == cqInfo.CoQuanID).ToList();
                                foreach (BaoCaoVuViecDongNguoiInfo baoCaoVuViecDong in dtLists)
                                {
                                    bc2aInfo2.XLDKhieuKienDN += baoCaoVuViecDong.XLDKhieuKienDN;
                                    bc2aInfo2.DonNhieuNguoiDungTen += baoCaoVuViecDong.DonNhieuNguoiDungTen;
                                }
                                resultList.Add(bc2aInfo2);
                                bc2aInfo.XLDKhieuKienDN += bc2aInfo2.XLDKhieuKienDN;
                                bc2aInfo.DonNhieuNguoiDungTen += bc2aInfo2.DonNhieuNguoiDungTen;
                            }
                        }
                    }
                }
                else if (capInfo.CapID == (int)CapQuanLy.CapSoNganh)
                {
                    //phucth
                    //          List<CoQuanInfo> cqList = new CoQuan().GetCoQuanByCapAndTinhID(capInfo.CapID, tinhID).ToList();
                    //          if (cqList.Count > 0)
                    //          {
                    //              doc =
                    //                 new XDocument(
                    //                 new XElement("LogConfig", cqList.Select(x =>
                    //new XElement("SystemLog", new XElement("CoQuan", new XAttribute("CoQuanID", x.CoQuanID),
                    //               new XAttribute("CapID", x.CapID), new XAttribute("CoQuanChaID", x.CoQuanChaID),
                    //               new XAttribute("SuDungPM", x.SuDungPM))))));

                    //              doc.Save(filename);
                    //              foreach (CoQuanInfo cqInfo in cqList)
                    //              {
                    //                  if (cqInfo.TinhID == IdentityHelper.TinhID)
                    //                  {
                    //                      BaoCaoVuViecDongNguoiInfo bc2aInfo2 = new BaoCaoVuViecDongNguoiInfo();
                    //                      bc2aInfo2.DonVi = cqInfo.TenCoQuan;
                    //                      bc2aInfo2.CapID = capInfo.CapID;
                    //                      bc2aInfo2.CoQuanID = cqInfo.CoQuanID;
                    //                      List<BaoCaoVuViecDongNguoiInfo> dtLists = dtList.Where(f => f.CoQuanID == cqInfo.CoQuanID).ToList();
                    //                      foreach (BaoCaoVuViecDongNguoiInfo baoCaoVuViecDong in dtLists)
                    //                      {
                    //                          bc2aInfo2.XLDKhieuKienDN += baoCaoVuViecDong.XLDKhieuKienDN;
                    //                          bc2aInfo2.DonNhieuNguoiDungTen += baoCaoVuViecDong.DonNhieuNguoiDungTen;
                    //                      }
                    //                      resultList.Add(bc2aInfo2);
                    //                      bc2aInfo.XLDKhieuKienDN += bc2aInfo2.XLDKhieuKienDN;
                    //                      bc2aInfo.DonNhieuNguoiDungTen += bc2aInfo2.DonNhieuNguoiDungTen;
                    //                  }
                    //              }
                    //          }
                }
                else if (capInfo.CapID == (int)CapQuanLy.CapUBNDHuyen)
                {
                    List<HuyenInfo> huyenList = new Huyen().GetByTinh(IdentityHelper.TinhID ?? 0).ToList();
                    foreach (HuyenInfo huyenInfo in huyenList)
                    {
                        BaoCaoVuViecDongNguoiInfo bc2aInfo2 = new BaoCaoVuViecDongNguoiInfo();
                        bc2aInfo2.DonVi = huyenInfo.TenHuyen;
                        bc2aInfo2.CapID = capInfo.CapID;
                        bc2aInfo2.CoQuanID = huyenInfo.HuyenID;
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
                        List<CoQuanInfo> cqList = cqListCapHuyen.Where(x => x.HuyenID == huyenInfo.HuyenID).ToList();
                        doc =
                           new XDocument(
                           new XElement("LogConfig", cqList.Select(x =>
          new XElement("SystemLog", new XElement("CoQuan", new XAttribute("CoQuanID", x.CoQuanID),
                         new XAttribute("CapID", x.CapID), new XAttribute("CoQuanChaID", x.CoQuanChaID),
                         new XAttribute("SuDungPM", x.SuDungPM))))));

                        doc.Save(filename);
                        foreach (CoQuanInfo cqInfo in cqList)
                        {
                            //bc2aInfo2.CoQuanID = cqInfo.CoQuanID;
                            List<BaoCaoVuViecDongNguoiInfo> dtLists = dtList.Where(f => f.CoQuanID == cqInfo.CoQuanID).ToList();
                            foreach (BaoCaoVuViecDongNguoiInfo baoCaoVuViecDong in dtLists)
                            {
                                bc2aInfo2.XLDKhieuKienDN += baoCaoVuViecDong.XLDKhieuKienDN;
                                bc2aInfo2.DonNhieuNguoiDungTen += baoCaoVuViecDong.DonNhieuNguoiDungTen;
                            }

                        }
                        resultList.Add(bc2aInfo2);
                        bc2aInfo.XLDKhieuKienDN += bc2aInfo2.XLDKhieuKienDN;
                        bc2aInfo.DonNhieuNguoiDungTen += bc2aInfo2.DonNhieuNguoiDungTen;
                    }
                }
                else if (capInfo.CapID == (int)CapQuanLy.CapUBNDXa)
                {
                    List<HuyenInfo> huyenList = new Huyen().GetByTinh(IdentityHelper.TinhID ?? 0).ToList();
                    List<CoQuanInfo> cqListCapXa = new CoQuan().GetCoQuanByCapAndTinhID((int)CapQuanLy.CapUBNDXa, tinhID).ToList();
                    List<XaInfo> xaList = new List<XaInfo>();
                    foreach (HuyenInfo huyenInfo in huyenList)
                    {
                        List<CoQuanInfo> cqList = cqListCapXa.Where(x => x.HuyenID == huyenInfo.HuyenID).ToList();
                        doc =
                            new XDocument(
                                new XElement("LogConfig", cqList.Select(x =>
                                new XElement("SystemLog", new XElement("CoQuan", new XAttribute("CoQuanID", x.CoQuanID),
                                new XAttribute("CapID", x.CapID), new XAttribute("CoQuanChaID", x.CoQuanChaID),
                                new XAttribute("SuDungPM", x.SuDungPM))))));

                        doc.Save(filename);
                        BaoCaoVuViecDongNguoiInfo bc2aInfo2Huyen = new BaoCaoVuViecDongNguoiInfo()
                        {
                            DonVi = huyenInfo.TenHuyen,
                            CoQuanID = huyenInfo.HuyenID,
                            XLDKhieuKienDN = dtList.Where(x => cqList.Select(y => y.CoQuanID).Contains(x.CoQuanID)).Sum(x => x.XLDKhieuKienDN),
                            DonNhieuNguoiDungTen = dtList.Where(x => cqList.Select(y => y.CoQuanID).Contains(x.CoQuanID)).Sum(x => x.DonNhieuNguoiDungTen),
                        };
                        resultList.Add(bc2aInfo2Huyen);
                        foreach (CoQuanInfo cqInfo in cqList)
                        {
                            BaoCaoVuViecDongNguoiInfo bc2aInfo2 = new BaoCaoVuViecDongNguoiInfo();
                            bc2aInfo2.DonVi = cqInfo.TenCoQuan + " (" + huyenInfo.TenHuyen + ")";
                            bc2aInfo2.CapID = capInfo.CapID;
                            bc2aInfo2.CoQuanID = cqInfo.CoQuanID;
                            List<BaoCaoVuViecDongNguoiInfo> dtLists = dtList.Where(f => f.CoQuanID == cqInfo.CoQuanID).ToList();
                            foreach (BaoCaoVuViecDongNguoiInfo baoCaoVuViecDong in dtLists)
                            {
                                bc2aInfo2.XLDKhieuKienDN += baoCaoVuViecDong.XLDKhieuKienDN;
                                bc2aInfo2.DonNhieuNguoiDungTen += baoCaoVuViecDong.DonNhieuNguoiDungTen;
                            }
                            resultList.Add(bc2aInfo2);
                            bc2aInfo.XLDKhieuKienDN += bc2aInfo2.XLDKhieuKienDN;
                            bc2aInfo.DonNhieuNguoiDungTen += bc2aInfo2.DonNhieuNguoiDungTen;
                        }

                    }
                }

                bcInfo.XLDKhieuKienDN += bc2aInfo.XLDKhieuKienDN;
                bcInfo.DonNhieuNguoiDungTen += bc2aInfo.DonNhieuNguoiDungTen;
            }
        }

        public static void CalculateBaoCaoCapTrungUong(ref List<BaoCaoVuViecDongNguoiInfo> resultList, DateTime startDate, DateTime endDate, int tinhID)
        {
            List<CapInfo> capList = new CapDAL().GetAll().ToList();
            foreach (CapInfo capInfo in capList)
            {
                //Neu la cap trung uong, chi hien thi cac tinh
                if (capInfo.CapID == (int)CapQuanLy.CapUBNDTinh)
                {
                    BaoCaoVuViecDongNguoiInfo bc2aInfo = new BaoCaoVuViecDongNguoiInfo();
                    bc2aInfo.DonVi = "<b style='text-transform: uppercase'>" + capInfo.TenCap + "</b>";
                    bc2aInfo.CapID = capInfo.CapID;
                    resultList.Add(bc2aInfo);
                    List<CoQuanInfo> cqList = new CoQuan().GetCoQuanByCapAndTinhID(capInfo.CapID, tinhID).ToList();
                    List<BaoCaoVuViecDongNguoiInfo> dtList = new BaoCaoVuViecDongNguoi_DAL().GET_DONTHUVUVIECDONGNGUOI(startDate, endDate).ToList();
                    if (cqList.Count > 0)
                    {
                        foreach (CoQuanInfo cqInfo in cqList)
                        {
                            BaoCaoVuViecDongNguoiInfo bc2aInfo2 = new BaoCaoVuViecDongNguoiInfo();
                            bc2aInfo2.DonVi = cqInfo.TenCoQuan;
                            bc2aInfo2.CapID = capInfo.CapID;
                            bc2aInfo2.CoQuanID = cqInfo.CoQuanID;
                            List<BaoCaoVuViecDongNguoiInfo> dtLists = dtList.Where(f => f.CoQuanID == cqInfo.CoQuanID).ToList();
                            foreach (BaoCaoVuViecDongNguoiInfo baoCaoVuViecDong in dtLists)
                            {
                                bc2aInfo2.XLDKhieuKienDN += baoCaoVuViecDong.XLDKhieuKienDN;
                                bc2aInfo2.DonNhieuNguoiDungTen += baoCaoVuViecDong.DonNhieuNguoiDungTen;

                            }
                            resultList.Add(bc2aInfo2);

                        }
                    }
                }
            }
        }

        public static void CalculateBaoCaoCapSo(ref List<BaoCaoVuViecDongNguoiInfo> resultList, DateTime startDate, DateTime endDate, IdentityHelper IdentityHelper, string ContentRootPath)
        {
            DateTime firstDay = Season.GetFirstDayOfYear(startDate.Year);

            CoQuanInfo cqInfo = new CoQuan().GetCoQuanByID(IdentityHelper.CoQuanID ?? 0);
            List<CoQuanInfo> lst = new List<CoQuanInfo> { cqInfo };
            string filename = ContentRootPath + "\\Upload\\" + IdentityHelper.CanBoID + "_CoQuan.xml";
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
                               new XElement("LogConfig", lst.Select(x =>
              new XElement("SystemLog", new XElement("CoQuan", new XAttribute("CoQuanID", x.CoQuanID),
                             new XAttribute("CapID", x.CapID), new XAttribute("CoQuanChaID", x.CoQuanChaID),
                             new XAttribute("SuDungPM", x.SuDungPM))))));

            doc.Save(filename);
            BaoCaoVuViecDongNguoiInfo bc2aInfo2 = new BaoCaoVuViecDongNguoiInfo();
            bc2aInfo2.DonVi = cqInfo.TenCoQuan;
            bc2aInfo2.CapID = (int)CapQuanLy.CapSoNganh;
            bc2aInfo2.CoQuanID = cqInfo.CoQuanID;
            //26/08/2018 Sửa firstDay --> startDate
            List<BaoCaoVuViecDongNguoiInfo> dtList = new BaoCaoVuViecDongNguoi_DAL().GET_DONTHUVUVIECDONGNGUOI(startDate, endDate).ToList();
            List<BaoCaoVuViecDongNguoiInfo> dtLists = dtList.Where(f => f.CoQuanID == cqInfo.CoQuanID).ToList();
            foreach (BaoCaoVuViecDongNguoiInfo baoCaoVuViecDong in dtLists)
            {
                bc2aInfo2.XLDKhieuKienDN += baoCaoVuViecDong.XLDKhieuKienDN;
                bc2aInfo2.DonNhieuNguoiDungTen += baoCaoVuViecDong.DonNhieuNguoiDungTen;
            }
            resultList.Add(bc2aInfo2);

        }

        public static void CalculateBaoCaoPhong(ref List<BaoCaoVuViecDongNguoiInfo> resultList, DateTime startDate, DateTime endDate, IdentityHelper IdentityHelper)
        {
            DateTime firstDay = Season.GetFirstDayOfYear(startDate.Year);
            CoQuanInfo cqInfo = new CoQuan().GetCoQuanByID(IdentityHelper.CoQuanID ?? 0);
            List<CoQuanInfo> lst = new List<CoQuanInfo> { cqInfo };

            BaoCaoVuViecDongNguoiInfo bc2aInfo2 = new BaoCaoVuViecDongNguoiInfo();
            bc2aInfo2.DonVi = cqInfo.TenCoQuan;
            bc2aInfo2.CapID = (int)CapQuanLy.CapPhong;
            bc2aInfo2.CoQuanID = cqInfo.CoQuanID;

            List<BaoCaoVuViecDongNguoiInfo> dtList = new BaoCaoVuViecDongNguoi_DAL().GET_DONTHUVUVIECDONGNGUOI(startDate, endDate).ToList();
            List<BaoCaoVuViecDongNguoiInfo> dtLists = dtList.Where(f => f.CoQuanID == cqInfo.CoQuanID).ToList();
            foreach (BaoCaoVuViecDongNguoiInfo baoCaoVuViecDong in dtLists)
            {
                bc2aInfo2.XLDKhieuKienDN += baoCaoVuViecDong.XLDKhieuKienDN;
                bc2aInfo2.DonNhieuNguoiDungTen += baoCaoVuViecDong.DonNhieuNguoiDungTen;
            }
            resultList.Add(bc2aInfo2);
        }

        public static void CalculateBaoCaoCapHuyen(ref List<BaoCaoVuViecDongNguoiInfo> resultList, DateTime startDate, DateTime endDate, int tinhID, IdentityHelper IdentityHelper, string ContentRootPath)
        {
            DateTime firstDay = Season.GetFirstDayOfYear(startDate.Year);
            string filename = ContentRootPath + "\\Upload\\" + IdentityHelper.CanBoID + "_CoQuan.xml";
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            using (FileStream file = System.IO.File.Create(filename))
            {

            }
            XDocument doc = new XDocument();

            BaoCaoVuViecDongNguoiInfo totalInfo = new BaoCaoVuViecDongNguoiInfo();
            totalInfo.DonVi = "<b>TOÀN HUYỆN</b>";
            totalInfo.CapID = 0;
            resultList.Add(totalInfo);

            //cap huyen
            BaoCaoVuViecDongNguoiInfo caphuyenInfo = new BaoCaoVuViecDongNguoiInfo();
            caphuyenInfo.DonVi = "<b>Cấp UBND Huyện</b>";
            caphuyenInfo.CapID = (int)CapQuanLy.CapUBNDHuyen;
            resultList.Add(caphuyenInfo);

            List<BaoCaoVuViecDongNguoiInfo> dtList = new BaoCaoVuViecDongNguoi_DAL().GET_DONTHUVUVIECDONGNGUOI(startDate, endDate).ToList();
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
                cqHuyenList = cqListCapHuyen.Where(x => x.HuyenID == IdentityHelper.HuyenID).ToList();
                doc =
                             new XDocument(
                             new XElement("LogConfig", cqHuyenList.Select(x =>
            new XElement("SystemLog", new XElement("CoQuan", new XAttribute("CoQuanID", x.CoQuanID),
                           new XAttribute("CapID", x.CapID), new XAttribute("CoQuanChaID", x.CoQuanChaID),
                           new XAttribute("SuDungPM", x.SuDungPM))))));

                doc.Save(filename);
            }
            else
            {
                /*  c*//*qHuyenList = cqListCapHuyen.Where(x => x.CoQuanID == IdentityHelper.CoQuanID ?? 0).ToList();*/
                cqHuyenList = new CoQuan().GetAllCapCon(IdentityHelper.CoQuanID ?? 0).Where(x => x.CapID == CapQuanLy.CapPhong.GetHashCode() ||
                x.CapID == CapQuanLy.CapUBNDHuyen.GetHashCode()).ToList();
                doc =
                             new XDocument(
                             new XElement("LogConfig", cqHuyenList.Select(x =>
            new XElement("SystemLog", new XElement("CoQuan", new XAttribute("CoQuanID", x.CoQuanID),
                           new XAttribute("CapID", x.CapID), new XAttribute("CoQuanChaID", x.CoQuanChaID),
                           new XAttribute("SuDungPM", x.SuDungPM))))));

                doc.Save(filename);
            }
            foreach (CoQuanInfo cqInfo in cqHuyenList)
            {
                if (cqInfo.CapID == (int)CapQuanLy.CapUBNDHuyen || cqInfo.CapID == (int)CapQuanLy.CapPhong)
                {
                    BaoCaoVuViecDongNguoiInfo bc2aInfo = new BaoCaoVuViecDongNguoiInfo();
                    bc2aInfo.DonVi = cqInfo.TenCoQuan;
                    bc2aInfo.CapID = (int)CapQuanLy.CapUBNDHuyen;
                    bc2aInfo.CoQuanID = cqInfo.CoQuanID;
                    List<BaoCaoVuViecDongNguoiInfo> dtLists = dtList.Where(f => f.CoQuanID == cqInfo.CoQuanID).ToList();
                    foreach (BaoCaoVuViecDongNguoiInfo baoCaoVuViecDong in dtLists)
                    {
                        bc2aInfo.XLDKhieuKienDN += baoCaoVuViecDong.XLDKhieuKienDN;
                        bc2aInfo.DonNhieuNguoiDungTen += baoCaoVuViecDong.DonNhieuNguoiDungTen;
                    }
                    resultList.Add(bc2aInfo);
                    caphuyenInfo.XLDKhieuKienDN += bc2aInfo.XLDKhieuKienDN;
                    caphuyenInfo.DonNhieuNguoiDungTen += bc2aInfo.DonNhieuNguoiDungTen;
                    totalInfo.XLDKhieuKienDN += bc2aInfo.XLDKhieuKienDN;
                    totalInfo.DonNhieuNguoiDungTen += bc2aInfo.DonNhieuNguoiDungTen;
                    //TinhTong(ref caphuyenInfo, bc2aInfo);
                }
            }

            //cap UBND xa
            if (IdentityHelper.RoleID == (int)EnumChucVu.LanhDao)
            {
                BaoCaoVuViecDongNguoiInfo capxaInfo = new BaoCaoVuViecDongNguoiInfo();
                capxaInfo.DonVi = "<b>Cấp xã</b>";
                capxaInfo.CapID = (int)CapQuanLy.CapUBNDXa;
                resultList.Add(capxaInfo);
                HuyenInfo huyenInfo = new Huyen().GetByID(IdentityHelper.HuyenID ?? 0);
                List<CoQuanInfo> cqListCapXa = new CoQuan().GetCoQuanByCapAndTinhID((int)CapQuanLy.CapUBNDXa, tinhID).ToList();
                List<CoQuanInfo> cqList = cqListCapXa.Where(x => x.HuyenID == huyenInfo.HuyenID).ToList();

                foreach (CoQuanInfo cqInfo in cqList)
                {
                    BaoCaoVuViecDongNguoiInfo bc2aInfo2 = new BaoCaoVuViecDongNguoiInfo();
                    bc2aInfo2.DonVi = cqInfo.TenCoQuan + " (" + huyenInfo.TenHuyen + ")";
                    bc2aInfo2.CapID = (int)CapQuanLy.CapUBNDXa;
                    bc2aInfo2.CoQuanID = cqInfo.CoQuanID;
                    List<BaoCaoVuViecDongNguoiInfo> dtLists = dtList.Where(f => f.CoQuanID == cqInfo.CoQuanID).ToList();
                    foreach (BaoCaoVuViecDongNguoiInfo baoCaoVuViecDong in dtLists)
                    {
                        bc2aInfo2.XLDKhieuKienDN += baoCaoVuViecDong.XLDKhieuKienDN;
                        bc2aInfo2.DonNhieuNguoiDungTen += baoCaoVuViecDong.DonNhieuNguoiDungTen;
                    }
                    resultList.Add(bc2aInfo2);
                    capxaInfo.XLDKhieuKienDN += bc2aInfo2.XLDKhieuKienDN;
                    capxaInfo.DonNhieuNguoiDungTen += bc2aInfo2.DonNhieuNguoiDungTen;
                    totalInfo.XLDKhieuKienDN += bc2aInfo2.XLDKhieuKienDN;
                    totalInfo.DonNhieuNguoiDungTen += bc2aInfo2.DonNhieuNguoiDungTen;
                }
            }
        }

        public static void CalculateBaoCaoCapXa(ref List<BaoCaoVuViecDongNguoiInfo> resultList, DateTime startDate, DateTime endDate, IdentityHelper IdentityHelper, string ContentRootPath)
        {
            DateTime firstDay = Season.GetFirstDayOfYear(startDate.Year);

            CoQuanInfo cqInfo = new CoQuan().GetCoQuanByID(IdentityHelper.CoQuanID ?? 0);
            List<CoQuanInfo> cqList = new List<CoQuanInfo> { cqInfo };
            string filename = ContentRootPath + "\\Upload\\" + IdentityHelper.CanBoID + "_CoQuan.xml";
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
            BaoCaoVuViecDongNguoiInfo bc2aInfo2 = new BaoCaoVuViecDongNguoiInfo();
            bc2aInfo2.DonVi = cqInfo.TenCoQuan;
            bc2aInfo2.CapID = (int)CapQuanLy.CapUBNDXa;
            bc2aInfo2.CoQuanID = cqInfo.CoQuanID;
            //26/08/2018 Sửa firstDay --> startDate
            List<BaoCaoVuViecDongNguoiInfo> dtList = new BaoCaoVuViecDongNguoi_DAL().GET_DONTHUVUVIECDONGNGUOI(startDate, endDate).ToList();
            List<BaoCaoVuViecDongNguoiInfo> dtLists = dtList.Where(f => f.CoQuanID == cqInfo.CoQuanID).ToList();
            foreach (BaoCaoVuViecDongNguoiInfo baoCaoVuViecDong in dtLists)
            {
                bc2aInfo2.XLDKhieuKienDN += baoCaoVuViecDong.XLDKhieuKienDN;
                bc2aInfo2.DonNhieuNguoiDungTen += baoCaoVuViecDong.DonNhieuNguoiDungTen;
            }
            resultList.Add(bc2aInfo2);
        }

        #region Thống kê chi tiết
        //public static List<ChiTietVuViecDongNguoiInfo> ChiTietDonThuBaoCaoCapTinh( DateTime startDate, DateTime endDate, List<CapInfo> capList, int tinhID,int Start,int CanBoID)
        //{
        //    DateTime firstDay = Season.GetFirstDayOfYear(startDate.Year);
        //    List<ChiTietVuViecDongNguoiInfo> resultList = new List<ChiTietVuViecDongNguoiInfo>();
        //    foreach (CapInfo capInfo in capList)
        //    {
        //        List<ChiTietVuViecDongNguoiInfo> dtList = new BaoCaoVuViecDongNguoi_DAL().GETCHITIET_DONTHUVUVIECDONGNGUOI(startDate, endDate, Start, CanBoID).ToList();
        //        if (capInfo.CapID == (int)CapQuanLy.CapUBNDTinh)
        //        {
        //            List<CoQuanInfo> cqList = new CoQuan().GetCoQuanByCapAndTinhID(capInfo.CapID, tinhID).ToList();

        //            if (cqList.Count > 0)
        //            {
        //                foreach (CoQuanInfo cqInfo in cqList)
        //                {
        //                    if (cqInfo.TinhID == IdentityHelper.GetTinhID())
        //                    {

        //                        List<ChiTietVuViecDongNguoiInfo> dtLists = dtList.Where(f => f.CoQuanID == cqInfo.CoQuanID).ToList();
        //                        foreach (ChiTietVuViecDongNguoiInfo chiTietVuViecDong in dtLists)
        //                        {
        //                            chiTietVuViecDong.CapID = capInfo.CapID;
        //                            resultList.Add(chiTietVuViecDong);

        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        else if (capInfo.CapID == (int)CapQuanLy.CapSoNganh)
        //        {
        //            //phucth
        //            List<CoQuanInfo> cqList = new CoQuan().GetCoQuanByCapAndTinhID(capInfo.CapID, tinhID).ToList();
        //            if (cqList.Count > 0)
        //            {
        //                foreach (CoQuanInfo cqInfo in cqList)
        //                {
        //                    if (cqInfo.TinhID == IdentityHelper.GetTinhID())
        //                    {
        //                        List<ChiTietVuViecDongNguoiInfo> dtLists = dtList.Where(f => f.CoQuanID == cqInfo.CoQuanID).ToList();
        //                        foreach (ChiTietVuViecDongNguoiInfo chiTietVuViecDong in dtLists)
        //                        {
        //                            chiTietVuViecDong.CapID = capInfo.CapID;
        //                            resultList.Add(chiTietVuViecDong);

        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        else if (capInfo.CapID == (int)CapQuanLy.CapUBNDHuyen)
        //        {
        //            List<HuyenInfo> huyenList = new Huyen().GetByTinh(IdentityHelper.GetTinhID()).ToList();
        //            foreach (HuyenInfo huyenInfo in huyenList)
        //            {
        //                BaoCaoVuViecDongNguoiInfo bc2aInfo2 = new BaoCaoVuViecDongNguoiInfo();
        //                bc2aInfo2.DonVi = huyenInfo.TenHuyen;
        //                bc2aInfo2.CapID = capInfo.CapID;

        //                List<CoQuanInfo> cqListCapPhong = new CoQuan().GetCoQuanByCapAndTinhID((int)CapQuanLy.CapPhong, tinhID).ToList();
        //                List<CoQuanInfo> cqListCapUBHuyen = new CoQuan().GetCoQuanByCapAndTinhID((int)CapQuanLy.CapUBNDHuyen, tinhID).ToList();
        //                List<CoQuanInfo> cqListCapHuyen = new List<CoQuanInfo>();
        //                foreach (CoQuanInfo itemCoQuan in cqListCapPhong)
        //                {
        //                    cqListCapHuyen.Add(itemCoQuan);
        //                }
        //                foreach (CoQuanInfo itemCoQuan in cqListCapUBHuyen)
        //                {
        //                    cqListCapHuyen.Add(itemCoQuan);
        //                }
        //                List<CoQuanInfo> cqList = cqListCapHuyen.Where(x => x.HuyenID == huyenInfo.HuyenID).ToList();

        //                foreach (CoQuanInfo cqInfo in cqList)
        //                {
        //                    List<ChiTietVuViecDongNguoiInfo> dtLists = dtList.Where(f => f.CoQuanID == cqInfo.CoQuanID).ToList();
        //                    foreach (ChiTietVuViecDongNguoiInfo chiTietVuViecDong in dtLists)
        //                    {
        //                        chiTietVuViecDong.CapID = capInfo.CapID;
        //                        resultList.Add(chiTietVuViecDong);
        //                    }
        //                }
        //            }
        //        }
        //        else if (capInfo.CapID == (int)CapQuanLy.CapUBNDXa)
        //        {
        //            List<HuyenInfo> huyenList = new Huyen().GetByTinh(IdentityHelper.GetTinhID()).ToList();
        //            List<CoQuanInfo> cqListCapXa = new CoQuan().GetCoQuanByCapAndTinhID((int)CapQuanLy.CapUBNDXa, tinhID).ToList();
        //            List<XaInfo> xaList = new List<XaInfo>();
        //            foreach (HuyenInfo huyenInfo in huyenList)
        //            {
        //                List<CoQuanInfo> cqList = cqListCapXa.Where(x => x.HuyenID == huyenInfo.HuyenID).ToList();
        //                foreach (CoQuanInfo cqInfo in cqList)
        //                {
        //                    List<ChiTietVuViecDongNguoiInfo> dtLists = dtList.Where(f => f.CoQuanID == cqInfo.CoQuanID).ToList();
        //                    foreach (ChiTietVuViecDongNguoiInfo chiTietVuViecDong in dtLists)
        //                    {
        //                        chiTietVuViecDong.CapID = capInfo.CapID;
        //                        resultList.Add(chiTietVuViecDong);

        //                    }

        //                }

        //            }
        //        }
        //    }
        //    return resultList;
        //}

        //public static List<ChiTietVuViecDongNguoiInfo> ChiTietDonThuBaoCaoCapTrungUong(DateTime startDate, DateTime endDate, int tinhID,int Start,int CanBoID)
        //{
        //    List<CapInfo> capList = new Cap().GetAll().ToList();
        //    List<ChiTietVuViecDongNguoiInfo> resultList = new List<ChiTietVuViecDongNguoiInfo>();
        //    foreach (CapInfo capInfo in capList)
        //    {
        //        //Neu la cap trung uong, chi hien thi cac tinh
        //        if (capInfo.CapID == (int)CapQuanLy.CapUBNDTinh)
        //        {

        //            List<CoQuanInfo> cqList = new CoQuan().GetCoQuanByCapAndTinhID(capInfo.CapID, tinhID).ToList();
        //            List<ChiTietVuViecDongNguoiInfo> dtList = new BaoCaoVuViecDongNguoi_DAL().GETCHITIET_DONTHUVUVIECDONGNGUOI(startDate, endDate,Start, CanBoID).ToList();
        //            if (cqList.Count > 0)
        //            {
        //                foreach (CoQuanInfo cqInfo in cqList)
        //                {
        //                    List<ChiTietVuViecDongNguoiInfo> dtLists = dtList.Where(f => f.CoQuanID == cqInfo.CoQuanID).ToList();
        //                    foreach (ChiTietVuViecDongNguoiInfo chiTietVuViecDong in dtLists)
        //                    {
        //                        chiTietVuViecDong.CapID = capInfo.CapID;
        //                        resultList.Add(chiTietVuViecDong);

        //                    }

        //                }
        //            }
        //        }
        //    }
        //    return resultList;
        //}

        //public static List<ChiTietVuViecDongNguoiInfo> ChiTietDonThuBaoCaoCapHuyen(DateTime startDate, DateTime endDate,int tinhID,int Start,int CanBoID)
        //{
        //    DateTime firstDay = Season.GetFirstDayOfYear(startDate.Year);

        //    List<ChiTietVuViecDongNguoiInfo> resultList = new List<ChiTietVuViecDongNguoiInfo>();
        //    List<ChiTietVuViecDongNguoiInfo> dtList = new BaoCaoVuViecDongNguoi_DAL().GETCHITIET_DONTHUVUVIECDONGNGUOI(startDate, endDate,Start, CanBoID).ToList();
        //    List<CoQuanInfo> cqListCapPhong = new CoQuan().GetCoQuanByCapAndTinhID((int)CapQuanLy.CapPhong, tinhID).ToList();
        //    List<CoQuanInfo> cqListCapUBHuyen = new CoQuan().GetCoQuanByCapAndTinhID((int)CapQuanLy.CapUBNDHuyen, tinhID).ToList();
        //    List<CoQuanInfo> cqListCapHuyen = new List<CoQuanInfo>();
        //    foreach (CoQuanInfo itemCoQuan in cqListCapPhong)
        //    {
        //        cqListCapHuyen.Add(itemCoQuan);
        //    }
        //    foreach (CoQuanInfo itemCoQuan in cqListCapUBHuyen)
        //    {
        //        cqListCapHuyen.Add(itemCoQuan);
        //    }
        //    List<CoQuanInfo> cqHuyenList = cqListCapHuyen.Where(x => x.HuyenID == IdentityHelper.GetHuyenID()).ToList();

        //    foreach (CoQuanInfo cqInfo in cqHuyenList)
        //    {
        //        if (cqInfo.CapID == (int)CapQuanLy.CapUBNDHuyen || cqInfo.CapID == (int)CapQuanLy.CapPhong)
        //        {
        //            List<ChiTietVuViecDongNguoiInfo> dtLists = dtList.Where(f => f.CoQuanID == cqInfo.CoQuanID).ToList();
        //            foreach (ChiTietVuViecDongNguoiInfo chiTietVuViecDong in dtLists)
        //            {
        //                chiTietVuViecDong.CapID = (int)CapQuanLy.CapUBNDHuyen;
        //                resultList.Add(chiTietVuViecDong);

        //            }
        //        }
        //    }

        //    //cap UBND xa

        //    HuyenInfo huyenInfo = new Huyen().GetByID(IdentityHelper.GetHuyenID());
        //    List<CoQuanInfo> cqListCapXa = new CoQuan().GetCoQuanByCapAndTinhID((int)CapQuanLy.CapUBNDXa, tinhID).ToList();
        //    List<CoQuanInfo> cqList = cqListCapXa.Where(x => x.HuyenID == huyenInfo.HuyenID).ToList();
        //    foreach (CoQuanInfo cqInfo in cqList)
        //    {
        //        List<ChiTietVuViecDongNguoiInfo> dtLists = dtList.Where(f => f.CoQuanID == cqInfo.CoQuanID).ToList();
        //        foreach (ChiTietVuViecDongNguoiInfo chiTietVuViecDong in dtLists)
        //        {
        //            chiTietVuViecDong.CapID = (int)CapQuanLy.CapUBNDHuyen;
        //            resultList.Add(chiTietVuViecDong);

        //        }
        //    }
        //    return resultList;
        //}

        //public static List<ChiTietVuViecDongNguoiInfo> ChiTietDonThuBaoCaoCapSo( DateTime startDate, DateTime endDate,int Start,int CanBoID)
        //{
        //    DateTime firstDay = Season.GetFirstDayOfYear(startDate.Year);
        //    List<ChiTietVuViecDongNguoiInfo> resultList = new List<ChiTietVuViecDongNguoiInfo>();
        //    CoQuanInfo cqInfo = new CoQuan().GetCoQuanByID(IdentityHelper.CoQuanID ?? 0);

        //    BaoCaoVuViecDongNguoiInfo bc2aInfo2 = new BaoCaoVuViecDongNguoiInfo();
        //    bc2aInfo2.DonVi = cqInfo.TenCoQuan;
        //    bc2aInfo2.CapID = (int)CapQuanLy.CapSoNganh;
        //    bc2aInfo2.CoQuanID = cqInfo.CoQuanID;
        //    List<ChiTietVuViecDongNguoiInfo> dtList = new BaoCaoVuViecDongNguoi_DAL().GETCHITIET_DONTHUVUVIECDONGNGUOI(startDate, endDate,Start, CanBoID).ToList();
        //    List<ChiTietVuViecDongNguoiInfo> dtLists = dtList.Where(f => f.CoQuanID == cqInfo.CoQuanID).ToList();
        //    foreach (ChiTietVuViecDongNguoiInfo chiTietVuViecDong in dtLists)
        //    {
        //        chiTietVuViecDong.CapID = (int)CapQuanLy.CapSoNganh;
        //        resultList.Add(chiTietVuViecDong);

        //    }
        //    return resultList;
        //}

        //public static List<ChiTietVuViecDongNguoiInfo> ChiTietDonThuBaoCaoCapXa(DateTime startDate, DateTime endDate,int Start,int CanBoID)
        //{
        //    DateTime firstDay = Season.GetFirstDayOfYear(startDate.Year);
        //    List<ChiTietVuViecDongNguoiInfo> resultList = new List<ChiTietVuViecDongNguoiInfo>();
        //    CoQuanInfo cqInfo = new CoQuan().GetCoQuanByID(IdentityHelper.CoQuanID ?? 0);
        //    List<ChiTietVuViecDongNguoiInfo> dtList = new BaoCaoVuViecDongNguoi_DAL().GETCHITIET_DONTHUVUVIECDONGNGUOI(startDate, endDate,Start, CanBoID).ToList();
        //    List<ChiTietVuViecDongNguoiInfo> dtLists = dtList.Where(f => f.CoQuanID == cqInfo.CoQuanID).ToList();
        //    foreach (ChiTietVuViecDongNguoiInfo chiTietVuViecDong in dtLists)
        //    {
        //        chiTietVuViecDong.CapID = (int)CapQuanLy.CapSoNganh;
        //        resultList.Add(chiTietVuViecDong);

        //    }
        //    return resultList;
        //}
        #endregion

    }
}
