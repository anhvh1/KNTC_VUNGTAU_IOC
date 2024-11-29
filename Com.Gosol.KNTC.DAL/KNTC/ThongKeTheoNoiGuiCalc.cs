using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Ultilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.DAL.KNTC
{
    public static class ThongKeTheoNoiGuiCalc
    {
        public static void TinhSoLieuToanTinh(DateTime startDate, DateTime endDate, int tinhID, int huyenID, ref List<TKTheoNoiPhatSinhInfo> resultList, ref List<TKTheoNoiPhatSinhInfo> tongToanTinh)
        {
            TinhInfo tinh = new TinhInfo();
            tinh.TinhID = tinhID;
            List<TinhInfo> tinhList = new List<TinhInfo>();
            tinhList.Add(tinh);
            List<HuyenInfo> huyenList = new Huyen().GetByTinh(tinhID).ToList();
            if (huyenID != 0)
            {
                huyenList = huyenList.Where(x => x.HuyenID == huyenID).ToList();
            }
            List<XaInfo> xaList = new List<XaInfo>();
            List<CoQuanInfo> listCoQuan = new List<CoQuanInfo>();
            foreach (HuyenInfo huyenInfo in huyenList)
            {
                List<XaInfo> data = new Xa().GetByHuyen(huyenInfo.HuyenID).ToList();
                xaList.AddRange(data);
                List<CoQuanInfo> cqHuyenList = new CoQuan().GetByHuyen(huyenInfo.HuyenID).ToList();
                listCoQuan.AddRange(cqHuyenList);
            }
            //var st1 = new Stopwatch();
            //st1.Start();
            tongToanTinh = new ThongKeDonThu().GetSoLieuBCTheoDCChuDon(startDate, endDate, 1, listCoQuan, tinhList, huyenList, xaList).ToList();
            List<TKTheoNoiPhatSinhInfo> soLieuHuyen = new ThongKeDonThu().GetSoLieuBCTheoDCChuDon(startDate, endDate, 2, listCoQuan, tinhList, huyenList, xaList).ToList();
            //st1.Stop();
            //var t1 = st1.Elapsed;
            //var st2 = new Stopwatch();
            //st2.Start();
            List<TKTheoNoiPhatSinhInfo> soLieuXa = new ThongKeDonThu().GetSoLieuBCTheoDCChuDon(startDate, endDate, 3, listCoQuan, tinhList, huyenList, xaList).ToList();
            //st2.Stop();
            //var t2 = st2.Elapsed;

            int stt = 1;
            foreach (HuyenInfo huyenInfo in huyenList)
            {
                int xaSTT = 1;
                TKTheoNoiPhatSinhInfo tkInfo = new TKTheoNoiPhatSinhInfo();
                tkInfo.Ten = huyenInfo.TenHuyen;
                tkInfo.STT = stt.ToString();
                foreach (TKTheoNoiPhatSinhInfo data in soLieuHuyen)
                {
                    if (data.HuyenID == huyenInfo.HuyenID)
                    {
                        tkInfo.TongSo = data.TongSo;
                        tkInfo.SLKhieuNai = data.SLKhieuNai;
                        tkInfo.SLToCao = data.SLToCao;
                        tkInfo.SLPhanAnh = data.SLPhanAnh;
                        tkInfo.SLKienNghi = data.SLKienNghi;
                        tkInfo.DiaPhuongID = data.HuyenID;
                    }
                }
                tkInfo.Level = 1;
                resultList.Add(tkInfo);

                foreach (XaInfo xaInfo in xaList)
                {
                    if (xaInfo.HuyenID == huyenInfo.HuyenID)
                    {
                        TKTheoNoiPhatSinhInfo tkInfo2 = new TKTheoNoiPhatSinhInfo();
                        tkInfo2.Ten = xaInfo.TenXa;
                        tkInfo2.STT = stt + "." + xaSTT.ToString();
                        foreach (TKTheoNoiPhatSinhInfo data in soLieuXa)
                        {
                            if (data.XaID == xaInfo.XaID)
                            {
                                tkInfo2.TongSo = data.TongSo;
                                tkInfo2.SLKhieuNai = data.SLKhieuNai;
                                tkInfo2.SLToCao = data.SLToCao;
                                tkInfo2.SLPhanAnh = data.SLPhanAnh;
                                tkInfo2.SLKienNghi = data.SLKienNghi;
                                tkInfo2.DiaPhuongID = data.XaID;
                            }
                        }
                        tkInfo2.Level = 2;
                        xaSTT++;
                        resultList.Add(tkInfo2);
                    }
                }

                stt++;
            }


            //List<TKDonThuInfo> dtList = new List<TKDonThuInfo>();
            //dtList = new ThongKeDonThu().GetDonThuByTinh(startDate, endDate, tinhID).ToList();

            //List<HuyenInfo> huyenList = new Huyen().GetByTinh(tinhID).ToList();

            //if (huyenID != 0)
            //{
            //    huyenList = huyenList.Where(x => x.HuyenID == huyenID).ToList();
            //}

            //int stt = 1;
            //foreach (HuyenInfo huyenInfo in huyenList)
            //{
            //    int xaSTT = 1;
            //    TKTheoNoiPhatSinhInfo tkInfo = new TKTheoNoiPhatSinhInfo();
            //    tkInfo.Ten = huyenInfo.TenHuyen;
            //    tkInfo.STT = stt.ToString();
            //    TinhSoLuongHuyen(ref tkInfo, dtList, huyenInfo.HuyenID);

            //    tkInfo.Level = 1;

            //    resultList.Add(tkInfo);

            //    List<XaInfo> xaList = new Xa().GetByHuyen(huyenInfo.HuyenID).ToList();
            //    foreach (XaInfo xaInfo in xaList)
            //    {
            //        TKTheoNoiPhatSinhInfo tkInfo2 = new TKTheoNoiPhatSinhInfo();
            //        tkInfo2.Ten = xaInfo.TenXa;
            //        tkInfo2.STT = stt + "." + xaSTT.ToString();                    
            //        TinhSoLuongXa(ref tkInfo2, dtList, xaInfo.XaID);

            //        tkInfo2.Level = 2;

            //        xaSTT++;
            //        resultList.Add(tkInfo2);
            //    }

            //    stt++;
            //}
        }

        public static void TinhSoLieuNoiPhatSinhToanTinh(DateTime startDate, DateTime endDate, int tinhID, int huyenID, ref List<TKTheoNoiPhatSinhInfo> resultList)
        {
            List<TKDonThuInfo> dtList = new List<TKDonThuInfo>();
            dtList = new ThongKeDonThu().GetDonThuNoiPhatSinhByTinh(startDate, endDate, tinhID, 0, 9999999).ToList();

            List<HuyenInfo> huyenList = new Huyen().GetByTinh(tinhID).ToList();

            if (huyenID != 0)
            {
                huyenList = huyenList.Where(x => x.HuyenID == huyenID).ToList();
            }

            int stt = 1;
            foreach (HuyenInfo huyenInfo in huyenList)
            {
                int xaSTT = 1;
                TKTheoNoiPhatSinhInfo tkInfo = new TKTheoNoiPhatSinhInfo();
                tkInfo.Ten = huyenInfo.TenHuyen;
                tkInfo.STT = stt.ToString();
                TinhSoLuongHuyen(ref tkInfo, dtList, huyenInfo.HuyenID);

                tkInfo.Level = 1;

                resultList.Add(tkInfo);

                List<XaInfo> xaList = new Xa().GetByHuyen(huyenInfo.HuyenID).ToList();
                foreach (XaInfo xaInfo in xaList)
                {
                    TKTheoNoiPhatSinhInfo tkInfo2 = new TKTheoNoiPhatSinhInfo();
                    tkInfo2.Ten = xaInfo.TenXa;
                    tkInfo2.STT = stt + "." + xaSTT.ToString();
                    TinhSoLuongXa(ref tkInfo2, dtList, xaInfo.XaID);

                    tkInfo2.Level = 2;

                    xaSTT++;
                    resultList.Add(tkInfo2);
                }

                stt++;
            }
        }

        public static void TinhSoLieuHuyen(DateTime startDate, DateTime endDate, int huyenID, ref List<TKTheoNoiPhatSinhInfo> resultList)
        {
            List<CoQuanInfo> cqHuyenList = new CoQuan().GetByHuyen(huyenID).ToList();
            List<HuyenInfo> huyenList = new List<HuyenInfo>();
            HuyenInfo huyen = new HuyenInfo();
            huyen.HuyenID = huyenID;
            huyenList.Add(huyen);
            List<XaInfo> xaList = new List<XaInfo>();
            foreach (HuyenInfo huyenInfo in huyenList)
            {
                List<XaInfo> data = new Xa().GetByHuyen(huyenInfo.HuyenID).ToList();
                xaList.AddRange(data);
            }

            //List<TKTheoNoiPhatSinhInfo> soLieuHuyen = new ThongKeDonThu().GetSoLieuBCTheoDCChuDon(startDate, endDate, 2, cqHuyenList, null, huyenList, xaList).ToList();
            List<TKTheoNoiPhatSinhInfo> soLieuXa = new ThongKeDonThu().GetSoLieuBCTheoDCChuDon(startDate, endDate, 3, cqHuyenList, null, huyenList, xaList).ToList();
            int xaSTT = 1;
            foreach (XaInfo xaInfo in xaList)
            {
                TKTheoNoiPhatSinhInfo tkInfo2 = new TKTheoNoiPhatSinhInfo();
                tkInfo2.Ten = xaInfo.TenXa;
                tkInfo2.STT = xaSTT.ToString();
                foreach (TKTheoNoiPhatSinhInfo data in soLieuXa)
                {
                    if (data.XaID == xaInfo.XaID)
                    {
                        tkInfo2.TongSo = data.TongSo;
                        tkInfo2.SLKhieuNai = data.SLKhieuNai;
                        tkInfo2.SLToCao = data.SLToCao;
                        tkInfo2.SLPhanAnh = data.SLPhanAnh;
                        tkInfo2.SLKienNghi = data.SLKienNghi;
                        tkInfo2.DiaPhuongID = data.XaID;
                    }
                }
                tkInfo2.Level = 2;
                xaSTT++;
                resultList.Add(tkInfo2);
            }

            //int stt = 1;
            //foreach (HuyenInfo huyenInfo in huyenList)
            //{
            //    int xaSTT = 1;
            //    TKTheoNoiPhatSinhInfo tkInfo = new TKTheoNoiPhatSinhInfo();
            //    tkInfo.Ten = huyenInfo.TenHuyen;
            //    tkInfo.STT = stt.ToString();
            //    foreach (TKTheoNoiPhatSinhInfo data in soLieuHuyen)
            //    {
            //        if (data.HuyenID == huyenInfo.HuyenID)
            //        {
            //            tkInfo.TongSo = data.TongSo;
            //            tkInfo.SLKhieuNai = data.SLKhieuNai;
            //            tkInfo.SLToCao = data.SLToCao;
            //            tkInfo.SLPhanAnh = data.SLPhanAnh;
            //            tkInfo.SLKienNghi = data.SLKienNghi;
            //            tkInfo.DiaPhuongID = data.HuyenID;
            //        }
            //    }
            //    tkInfo.Level = 1;
            //    resultList.Add(tkInfo);

            //    foreach (XaInfo xaInfo in xaList)
            //    {
            //        if (xaInfo.HuyenID == huyenInfo.HuyenID)
            //        {
            //            TKTheoNoiPhatSinhInfo tkInfo2 = new TKTheoNoiPhatSinhInfo();
            //            tkInfo2.Ten = xaInfo.TenXa;
            //            tkInfo2.STT = stt + "." + xaSTT.ToString();
            //            foreach (TKTheoNoiPhatSinhInfo data in soLieuXa)
            //            {
            //                if (data.XaID == xaInfo.XaID)
            //                {
            //                    tkInfo2.TongSo = data.TongSo;
            //                    tkInfo2.SLKhieuNai = data.SLKhieuNai;
            //                    tkInfo2.SLToCao = data.SLToCao;
            //                    tkInfo2.SLPhanAnh = data.SLPhanAnh;
            //                    tkInfo2.SLKienNghi = data.SLKienNghi;
            //                    tkInfo2.DiaPhuongID = data.XaID;
            //                }
            //            }
            //            tkInfo2.Level = 2;
            //            xaSTT++;
            //            resultList.Add(tkInfo2);
            //        }
            //    }

            //    stt++;
            //}


            //List<CoQuanInfo> cqHuyenList = new CoQuan().GetByHuyen(huyenID).ToList();
            //List<TKDonThuInfo> totalList = new List<TKDonThuInfo>();

            //foreach (CoQuanInfo cqInfo in cqHuyenList)
            //{
            //    List<TKDonThuInfo> dtList = new ThongKeDonThu().GetDonThuByCoQuan(startDate, endDate, cqInfo.CoQuanID).ToList();
            //    totalList.AddRange(dtList);
            //}            

            //int stt = 1;

            //List<XaInfo> xaList = new Xa().GetByHuyen(huyenID).ToList();
            //foreach (XaInfo xaInfo in xaList)
            //{
            //    TKTheoNoiPhatSinhInfo tkInfo2 = new TKTheoNoiPhatSinhInfo();
            //    tkInfo2.Ten = xaInfo.TenXa;
            //    tkInfo2.STT = stt.ToString();
            //    TinhSoLuongXa(ref tkInfo2, totalList, xaInfo.XaID);

            //    tkInfo2.Level = 2;

            //    stt++;
            //    resultList.Add(tkInfo2);
            //}

        }

        public static void TinhSoLieuXa(DateTime startDate, DateTime endDate, int cqID, int xaID, ref List<TKTheoNoiPhatSinhInfo> resultList)
        {
            XaInfo xa = new Xa().GetByID(xaID);
            List<XaInfo> xaList = new List<XaInfo>();
            xaList.Add(xa);
            List<CoQuanInfo> listCoQuan = new List<CoQuanInfo>();
            CoQuanInfo cq = new CoQuanInfo();
            cq.CoQuanID = cqID;
            listCoQuan.Add(cq);

            //List<CoQuanInfo> listCoQuan = new CoQuan().GetByHuyen(xa.HuyenID).ToList();

            //List<HuyenInfo> huyenList = new Huyen().GetByTinh(xa.TinhID).ToList();
            //List<CoQuanInfo> listCoQuan = new List<CoQuanInfo>();
            //foreach (HuyenInfo huyenInfo in huyenList)
            //{
            //    List<CoQuanInfo> cqHuyenList = new CoQuan().GetByHuyen(huyenInfo.HuyenID).ToList();
            //    listCoQuan.AddRange(cqHuyenList);
            //}


            List<TKTheoNoiPhatSinhInfo> soLieuXa = new ThongKeDonThu().GetSoLieuBCTheoDCChuDon(startDate, endDate, 3, listCoQuan, null, null, xaList).ToList();
            int stt = 1;
            int xaSTT = 1;
            foreach (XaInfo xaInfo in xaList)
            {
                TKTheoNoiPhatSinhInfo tkInfo2 = new TKTheoNoiPhatSinhInfo();
                tkInfo2.Ten = xaInfo.TenXa;
                tkInfo2.STT = stt + "." + xaSTT.ToString();
                foreach (TKTheoNoiPhatSinhInfo data in soLieuXa)
                {
                    if (data.XaID == xaInfo.XaID)
                    {
                        tkInfo2.TongSo = data.TongSo;
                        tkInfo2.SLKhieuNai = data.SLKhieuNai;
                        tkInfo2.SLToCao = data.SLToCao;
                        tkInfo2.SLPhanAnh = data.SLPhanAnh;
                        tkInfo2.SLKienNghi = data.SLKienNghi;
                        tkInfo2.DiaPhuongID = data.XaID;
                    }
                }
                tkInfo2.Level = 2;
                stt++;
                resultList.Add(tkInfo2);
            }
        }

        public static void TinhSoLieuNoiPhatSinhHuyen(DateTime startDate, DateTime endDate, int huyenID, ref List<TKTheoNoiPhatSinhInfo> resultList)
        {
            List<CoQuanInfo> cqHuyenList = new CoQuan().GetByHuyen(huyenID).ToList();
            //List<TKDonThuInfo> totalList = new List<TKDonThuInfo>();
            List<TKDonThuInfo> totalList = new ThongKeDonThu().GetDonThuNoiPhatSinhByListCoQuan(startDate, endDate, cqHuyenList, 0, 9999999).ToList();
            //foreach (CoQuanInfo cqInfo in cqHuyenList)
            //{
            //    List<TKDonThuInfo> dtList = new ThongKeDonThu().GetDonThuNoiPhatSinhByCoQuan(startDate, endDate, cqInfo.CoQuanID, 0 , 9999999).ToList();
            //    totalList.AddRange(dtList);
            //}

            HuyenInfo huyen = new Huyen().GetByID(huyenID);
            int stt = 1;
            int xaSTT = 1;
            TKTheoNoiPhatSinhInfo tkInfo = new TKTheoNoiPhatSinhInfo();
            tkInfo.Ten = huyen.TenHuyen;
            tkInfo.STT = stt.ToString();
            tkInfo.DiaPhuongID = huyenID;
            //TinhSoLuongHuyen(ref tkInfo, totalList, huyen.HuyenID);

            tkInfo.Level = 1;

            resultList.Add(tkInfo);

            List<XaInfo> xaList = new Xa().GetByHuyen(huyen.HuyenID).ToList();
            foreach (XaInfo xaInfo in xaList)
            {
                TKTheoNoiPhatSinhInfo tkInfo2 = new TKTheoNoiPhatSinhInfo();
                tkInfo2.Ten = xaInfo.TenXa;
                tkInfo2.STT = stt + "." + xaSTT.ToString();
                TinhSoLuongXa(ref tkInfo2, totalList, xaInfo.XaID);
                tkInfo.TongSo += tkInfo2.TongSo;
                tkInfo.SLKhieuNai += tkInfo2.SLKhieuNai;
                tkInfo.SLToCao += tkInfo2.SLToCao;
                tkInfo.SLKienNghi += tkInfo2.SLKienNghi;
                tkInfo.SLPhanAnh += tkInfo2.SLPhanAnh;

                tkInfo2.Level = 2;

                xaSTT++;
                resultList.Add(tkInfo2);
            }

            //int stt = 1;
            //List<XaInfo> xaList = new Xa().GetByHuyen(huyenID).ToList();
            //foreach (XaInfo xaInfo in xaList)
            //{
            //    TKTheoNoiPhatSinhInfo tkInfo2 = new TKTheoNoiPhatSinhInfo();
            //    tkInfo2.Ten = xaInfo.TenXa;
            //    tkInfo2.STT = stt.ToString();
            //    TinhSoLuongXa(ref tkInfo2, totalList, xaInfo.XaID);

            //    tkInfo2.Level = 2;

            //    stt++;
            //    resultList.Add(tkInfo2);
            //}

        }

        public static void TinhSoLieuNoiPhatSinhXa(DateTime startDate, DateTime endDate, int xaID, int huyenID, int cqID, ref List<TKTheoNoiPhatSinhInfo> resultList)
        {
            XaInfo xaInfo = new Xa().GetByID(xaID);
            List<TKDonThuInfo> totalList = new ThongKeDonThu().GetDonThuNoiPhatSinhByCoQuan(startDate, endDate, cqID, 0, 9999999).ToList();

            //List<CoQuanInfo> cqHuyenList = new CoQuan().GetByHuyen(huyenID).ToList();
            //List<TKDonThuInfo> totalList = new ThongKeDonThu().GetDonThuNoiPhatSinhByListCoQuan(startDate, endDate, cqHuyenList, 0, 9999999).ToList();

            TKTheoNoiPhatSinhInfo tkInfo2 = new TKTheoNoiPhatSinhInfo();
            tkInfo2.Ten = xaInfo.TenXa;
            tkInfo2.STT = "1";
            TinhSoLuongXa(ref tkInfo2, totalList, xaInfo.XaID);
            tkInfo2.Level = 2;
            resultList.Add(tkInfo2);
        }

        public static void TinhSoLieuSo(DateTime startDate, DateTime endDate, int cqID, ref List<TKTheoNoiPhatSinhInfo> resultList, IdentityHelper IdentityHelper)
        {
            List<HuyenInfo> huyenList = new Huyen().GetByTinh(IdentityHelper.TinhID ?? 0).ToList();
            List<XaInfo> xaList = new List<XaInfo>();
            foreach (HuyenInfo huyenInfo in huyenList)
            {
                List<XaInfo> data = new Xa().GetByHuyen(huyenInfo.HuyenID).ToList();
                xaList.AddRange(data);
            }
            List<CoQuanInfo> listCoQuan = new List<CoQuanInfo>();
            CoQuanInfo cq = new CoQuanInfo();
            cq.CoQuanID = cqID;
            listCoQuan.Add(cq);

            List<TKTheoNoiPhatSinhInfo> soLieuHuyen = new ThongKeDonThu().GetSoLieuBCTheoDCChuDon(startDate, endDate, 2, listCoQuan, null, huyenList, xaList).ToList();
            List<TKTheoNoiPhatSinhInfo> soLieuXa = new ThongKeDonThu().GetSoLieuBCTheoDCChuDon(startDate, endDate, 3, listCoQuan, null, huyenList, xaList).ToList();

            int stt = 1;
            foreach (HuyenInfo huyenInfo in huyenList)
            {
                int xaSTT = 1;
                TKTheoNoiPhatSinhInfo tkInfo = new TKTheoNoiPhatSinhInfo();
                tkInfo.Ten = huyenInfo.TenHuyen;
                tkInfo.STT = stt.ToString();
                foreach (TKTheoNoiPhatSinhInfo data in soLieuHuyen)
                {
                    if (data.HuyenID == huyenInfo.HuyenID)
                    {
                        tkInfo.TongSo = data.TongSo;
                        tkInfo.SLKhieuNai = data.SLKhieuNai;
                        tkInfo.SLToCao = data.SLToCao;
                        tkInfo.SLPhanAnh = data.SLPhanAnh;
                        tkInfo.SLKienNghi = data.SLKienNghi;
                        tkInfo.DiaPhuongID = data.HuyenID;
                    }
                }
                tkInfo.Level = 1;
                resultList.Add(tkInfo);

                foreach (XaInfo xaInfo in xaList)
                {
                    if (xaInfo.HuyenID == huyenInfo.HuyenID)
                    {
                        TKTheoNoiPhatSinhInfo tkInfo2 = new TKTheoNoiPhatSinhInfo();
                        tkInfo2.Ten = xaInfo.TenXa;
                        tkInfo2.STT = stt + "." + xaSTT.ToString();
                        foreach (TKTheoNoiPhatSinhInfo data in soLieuXa)
                        {
                            if (data.XaID == xaInfo.XaID)
                            {
                                tkInfo2.TongSo = data.TongSo;
                                tkInfo2.SLKhieuNai = data.SLKhieuNai;
                                tkInfo2.SLToCao = data.SLToCao;
                                tkInfo2.SLPhanAnh = data.SLPhanAnh;
                                tkInfo2.SLKienNghi = data.SLKienNghi;
                                tkInfo2.DiaPhuongID = data.XaID;
                            }
                        }
                        tkInfo2.Level = 2;
                        xaSTT++;
                        resultList.Add(tkInfo2);
                    }
                }

                stt++;
            }

            //List<TKDonThuInfo> dtList = new ThongKeDonThu().GetDonThuByCoQuan(startDate, endDate, cqID).ToList();            

            //List<HuyenInfo> huyenList = new Huyen().GetByTinh(IdentityHelper.GetTinhID()).ToList();
            //int stt = 1;
            //foreach (HuyenInfo huyenInfo in huyenList)
            //{
            //    int xaSTT = 1;
            //    TKTheoNoiPhatSinhInfo tkInfo = new TKTheoNoiPhatSinhInfo();
            //    tkInfo.Ten = huyenInfo.TenHuyen;
            //    tkInfo.STT = stt.ToString();
            //    TinhSoLuongHuyen(ref tkInfo, dtList, huyenInfo.HuyenID);

            //    tkInfo.Level = 1;

            //    resultList.Add(tkInfo);

            //    List<XaInfo> xaList = new Xa().GetByHuyen(huyenInfo.HuyenID).ToList();
            //    foreach (XaInfo xaInfo in xaList)
            //    {
            //        TKTheoNoiPhatSinhInfo tkInfo2 = new TKTheoNoiPhatSinhInfo();
            //        tkInfo2.Ten = xaInfo.TenXa;
            //        tkInfo2.STT = stt + "." + xaSTT.ToString();
            //        TinhSoLuongXa(ref tkInfo2, dtList, xaInfo.XaID);                   
            //        tkInfo2.Level = 2;

            //        xaSTT++;
            //        resultList.Add(tkInfo2);
            //    }

            //    stt++;
            //}
        }

        public static void TinhSoLieuNoiPhatSinhSo(DateTime startDate, DateTime endDate, int cqID, ref List<TKTheoNoiPhatSinhInfo> resultList, IdentityHelper IdentityHelper)
        {
            List<TKDonThuInfo> dtList = new ThongKeDonThu().GetDonThuNoiPhatSinhByCoQuan(startDate, endDate, cqID, 0, 9999999).ToList();

            List<HuyenInfo> huyenList = new Huyen().GetByTinh(IdentityHelper.TinhID ?? 0).ToList();
            int stt = 1;
            foreach (HuyenInfo huyenInfo in huyenList)
            {
                int xaSTT = 1;
                TKTheoNoiPhatSinhInfo tkInfo = new TKTheoNoiPhatSinhInfo();
                tkInfo.Ten = huyenInfo.TenHuyen;
                tkInfo.STT = stt.ToString();
                TinhSoLuongHuyen(ref tkInfo, dtList, huyenInfo.HuyenID);

                tkInfo.Level = 1;

                resultList.Add(tkInfo);

                List<XaInfo> xaList = new Xa().GetByHuyen(huyenInfo.HuyenID).ToList();
                foreach (XaInfo xaInfo in xaList)
                {
                    TKTheoNoiPhatSinhInfo tkInfo2 = new TKTheoNoiPhatSinhInfo();
                    tkInfo2.Ten = xaInfo.TenXa;
                    tkInfo2.STT = stt + "." + xaSTT.ToString();
                    TinhSoLuongXa(ref tkInfo2, dtList, xaInfo.XaID);
                    tkInfo2.Level = 2;

                    xaSTT++;
                    resultList.Add(tkInfo2);
                }

                stt++;
            }
        }

        private static void TinhSoLuongHuyen(ref TKTheoNoiPhatSinhInfo tkInfo, List<TKDonThuInfo> dtList, int huyenID)
        {
            tkInfo.TongSo = dtList.Where(x => x.HuyenID == huyenID).Count();
            tkInfo.SLKhieuNai = dtList.Where(x => x.HuyenID == huyenID && x.LoaiKhieuTo1ID == Constant.KhieuNai).Count();
            tkInfo.SLToCao = dtList.Where(x => x.HuyenID == huyenID && x.LoaiKhieuTo1ID == Constant.ToCao).Count();
            tkInfo.SLKienNghi = dtList.Where(x => x.HuyenID == huyenID && x.LoaiKhieuTo2ID == Constant.KienNghi).Count();
            tkInfo.SLPhanAnh = dtList.Where(x => x.HuyenID == huyenID && x.LoaiKhieuTo2ID == Constant.PhanAnh).Count();
            tkInfo.DiaPhuongID = huyenID;
        }

        private static void TinhSoLuongXa(ref TKTheoNoiPhatSinhInfo tkInfo, List<TKDonThuInfo> dtList, int xaID)
        {
            tkInfo.TongSo = dtList.Where(x => x.XaID == xaID).Count();
            tkInfo.SLKhieuNai = dtList.Where(x => x.XaID == xaID && x.LoaiKhieuTo1ID == Constant.KhieuNai).Count();
            tkInfo.SLToCao = dtList.Where(x => x.XaID == xaID && x.LoaiKhieuTo1ID == Constant.ToCao).Count();
            tkInfo.SLKienNghi = dtList.Where(x => x.XaID == xaID && x.LoaiKhieuTo2ID == Constant.KienNghi).Count();
            tkInfo.SLPhanAnh = dtList.Where(x => x.XaID == xaID && x.LoaiKhieuTo2ID == Constant.PhanAnh).Count();
            tkInfo.DiaPhuongID = xaID;
        }
    }
}
