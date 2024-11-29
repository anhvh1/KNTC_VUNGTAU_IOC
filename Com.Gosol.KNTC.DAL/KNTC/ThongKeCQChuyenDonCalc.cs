using Com.Gosol.KNTC.Models.KNTC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.DAL.KNTC
{
    public static class ThongKeCQChuyenDonCalc
    {
        public static void TinhSoLieuTrungUong(ref List<ThongKeInfo> resultList, DateTime startDate, DateTime endDate, ref int total)
        {
            // sua doi ngay 2019-06-20
            resultList = new ThongKeDonThu().ThongKeDonThuChuyenCoQuan_GetDonThu(startDate, endDate);
            /* old
        List<TKDonThuInfo> dtList = new ThongKeDonThu().GetDonThu(startDate, endDate).ToList();

        List<CoQuanInfo> cqList = new DAL.CoQuan().GetCoQuans().ToList();
        foreach (CoQuanInfo cqInfo in cqList)
        {

            ThongKeInfo tkInfo = new ThongKeInfo();
            tkInfo.Ten = cqInfo.TenCoQuan;
            tkInfo.SoLuong = CalculateSoLuong(dtList, cqInfo.CoQuanID);
            if (dtList.Count > 0)
            {
                tkInfo.TyLe = (double)tkInfo.SoLuong / dtList.Count;
            }
            else
            {
                tkInfo.TyLe = dtList.Count;
            }
            tkInfo.CoQuanID = cqInfo.CoQuanID;
            resultList.Add(tkInfo);

        }
        */
        }

        public static void TinhSoLieuToanTinh(ref List<ThongKeInfo> resultList, DateTime startDate, DateTime endDate, int tinhID, ref int total)
        {
            // sua doi ngay 2019-06-20
            resultList = new ThongKeDonThu().ThongKeDonThuChuyenCoQuan_GetDonThuByTinhID(startDate, endDate, tinhID);
            /* old
            List<TKDonThuInfo> dtList = new ThongKeDonThu().GetDonThuByTinh(startDate, endDate, tinhID).Where(x => x.CoQuanChuyenDonID != 0).ToList();
            List<CoQuanInfo> cqList = new DAL.CoQuan().GetCoQuans().ToList();
            foreach (CoQuanInfo cqInfo in cqList)
            {
                ThongKeInfo tkInfo = new ThongKeInfo();
                tkInfo.Ten = cqInfo.TenCoQuan;
                tkInfo.SoLuong = CalculateSoLuong(dtList, cqInfo.CoQuanID);
                if (dtList.Count >0)
                {
                    tkInfo.TyLe = (double)tkInfo.SoLuong / dtList.Count;
                }
                else
                {
                    tkInfo.TyLe = dtList.Count;
                }
                tkInfo.CoQuanID = cqInfo.CoQuanID;
                resultList.Add(tkInfo);
            }
            */
        }

        public static void TinhSoLieuTheoCoQuan(ref List<ThongKeInfo> resultList, DateTime startDate, DateTime endDate, int cqID, ref int total)
        {
            // sua doi ngay 2019-06-20
            resultList = new ThongKeDonThu().ThongKeDonThuChuyenCoQuan_GetDonThuByCoQuan(startDate, endDate, cqID);
            /* old
            List<TKDonThuInfo> dtList = new ThongKeDonThu().GetDonThuByCoQuan(startDate, endDate, cqID).Where(x => x.CoQuanChuyenDonID != 0).ToList();

            List<CoQuanInfo> cqList = new DAL.CoQuan().GetCoQuans().ToList();
            foreach (CoQuanInfo cqInfo in cqList)
            {

                ThongKeInfo tkInfo = new ThongKeInfo();
                tkInfo.Ten = cqInfo.TenCoQuan;
                tkInfo.SoLuong = CalculateSoLuong(dtList, cqInfo.CoQuanID);
                if (dtList.Count > 0)
                {
                    tkInfo.TyLe = (double)tkInfo.SoLuong / dtList.Count;
                }
                else
                {
                    tkInfo.TyLe = dtList.Count;
                }
                tkInfo.CoQuanID = cqInfo.CoQuanID;
                resultList.Add(tkInfo);

            }
            */
        }

        public static void TinhSoLieuTheoHuyen(ref List<ThongKeInfo> resultList, DateTime startDate, DateTime endDate, int huyenID, ref int total)
        {
            // sua doi ngay 2019-06-20
            resultList = new ThongKeDonThu().ThongKeDonThuChuyenCoQuan_GetDonThuByHuyenID(startDate, endDate, huyenID);
            /* old
            List<CoQuanInfo> cqHuyenList = new DAL.CoQuan().GetByHuyen(huyenID).ToList();
            List<TKDonThuInfo> totalList = new List<TKDonThuInfo>();

            foreach (CoQuanInfo cqInfo in cqHuyenList)
            {
                List<TKDonThuInfo> dtList = new ThongKeDonThu().GetDonThuByCoQuan(startDate, endDate, cqInfo.CoQuanID).ToList();
                totalList.AddRange(dtList);
            }

            totalList = totalList.Where(x => x.CoQuanChuyenDonID != 0).ToList();

            List<CoQuanInfo> cqList = new DAL.CoQuan().GetCoQuans().ToList();
            foreach (CoQuanInfo cqInfo in cqList)
            {
                ThongKeInfo tkInfo = new ThongKeInfo();
                tkInfo.Ten = cqInfo.TenCoQuan;
                tkInfo.SoLuong = CalculateSoLuong(totalList, cqInfo.CoQuanID);
                //tkInfo.TyLe = (double)tkInfo.SoLuong / totalList.Count;
                if (totalList.Count > 0)
                {
                    tkInfo.TyLe = (double)tkInfo.SoLuong / totalList.Count;
                }
                else
                {
                    tkInfo.TyLe = totalList.Count;
                }
                tkInfo.CoQuanID = cqInfo.CoQuanID;
                resultList.Add(tkInfo);
            }
            */
        }

        private static int CalculateSoLuong(List<TKDonThuInfo> dtList, int cqID)
        {
            int result = 0;
            foreach (TKDonThuInfo dtInfo in dtList)
            {
                if (dtInfo.CoQuanChuyenDonID == cqID)
                {
                    result++;
                }
            }
            return result;
        }

        // Them ngay 29/10/2019
        public static void TinhSoLieuDonDiToanTinh(ref List<ThongKeInfo> resultList, DateTime startDate, DateTime endDate, int tinhID, ref int total)
        {
            resultList = new ThongKeDonThu().ThongKeDonThuChuyenCoQuanDi_GetDonThuByTinhID(startDate, endDate, tinhID);
        }

        public static void TinhSoLieuDonDiTheoHuyen(ref List<ThongKeInfo> resultList, DateTime startDate, DateTime endDate, int huyenID, ref int total)
        {
            resultList = new ThongKeDonThu().ThongKeDonThuChuyenCoQuanDi_GetDonThuByHuyenID(startDate, endDate, huyenID);
        }

        public static void TinhSoLieuDonDiTheoCoQuan(ref List<ThongKeInfo> resultList, DateTime startDate, DateTime endDate, int cqID, ref int total)
        {
            resultList = new ThongKeDonThu().ThongKeDonThuChuyenCoQuanDi_GetDonThuByCoQuan(startDate, endDate, cqID);
        }

        public static void TinhSoLieuDonDiTrungUong(ref List<ThongKeInfo> resultList, DateTime startDate, DateTime endDate, ref int total)
        {
            resultList = new ThongKeDonThu().ThongKeDonThuChuyenCoQuanDi_GetDonThu(startDate, endDate);
        }

        public static void GetCoQuanNhanDonTrungUong(ref List<ThongKeInfo> resultList, DateTime startDate, DateTime endDate, int coQuanChuyenDenID)
        {
            resultList = new ThongKeDonThu().GetCoQuanCoDonThuChuyenDen_GetDonThu(startDate, endDate, coQuanChuyenDenID);
        }

        public static void GetCoQuanNhanDonToanTinh(ref List<ThongKeInfo> resultList, DateTime startDate, DateTime endDate, int tinhID, int coQuanChuyenDenID)
        {
            resultList = new ThongKeDonThu().GetcoQuanDonThuChuyenDen_GetDonThuByTinhID(startDate, endDate, tinhID, coQuanChuyenDenID);
        }

        public static void GetCoQuanNhanDonTheoCoQuan(ref List<ThongKeInfo> resultList, DateTime startDate, DateTime endDate, int cqID, int coQuanChuyenDenID)
        {
            resultList = new ThongKeDonThu().GetcoQuanDonThuChuyenDen_GetDonThuByCoQuan(startDate, endDate, cqID, coQuanChuyenDenID);
        }

        public static void GetCoQuanNhanDonTheoHuyen(ref List<ThongKeInfo> resultList, DateTime startDate, DateTime endDate, int huyenID, int coQuanChuyenDenID)
        {
            resultList = new ThongKeDonThu().GetcoQuanDonThuChuyenDen_GetDonThuByHuyenID(startDate, endDate, huyenID, coQuanChuyenDenID);
        }
    }
}
