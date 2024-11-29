using Com.Gosol.KNTC.Models.KNTC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.DAL.KNTC
{
    public class ThongKeLoaiKhieuToCalc
    {
        public static void CalculateBaoCaoCapTrungUong(ref List<ThongKeInfo> bcList, DateTime startDate, DateTime endDate, ref int total)
        {

            List<TKDonThuInfo> dtList = new ThongKeDonThu().GetDonThu(startDate, endDate).ToList();
            total = dtList.Count;

            List<LoaiKhieuToInfo> parentList = new LoaiKhieuTo().GetLoaiKhieuToWithLevel1().ToList();
            List<LoaiKhieuToInfo> treeList = new List<LoaiKhieuToInfo>();
            string sttPrefix = "";
            int stt = 1;
            foreach (LoaiKhieuToInfo parentInfo in parentList)
            {
                GetTreeList(ref bcList, parentInfo, sttPrefix, stt, dtList);
                stt++;
            }
        }

        public static void CalculateBaoCaoCapTrungUong(ref List<ThongKeInfo> bcList, DateTime startDate, DateTime endDate, int lktID, ref int total)
        {

            List<TKDonThuInfo> dtList = new ThongKeDonThu().GetDonThu(startDate, endDate).ToList();
            foreach (TKDonThuInfo dtInfo in dtList)
            {
                if (dtInfo.LoaiKhieuTo1ID == lktID || dtInfo.LoaiKhieuTo2ID == lktID || dtInfo.LoaiKhieuTo3ID == lktID)
                {
                    total++;
                }
            }

            LoaiKhieuToInfo parentInfo = new LoaiKhieuTo().GetLoaiKhieuToByID(lktID);
            string sttPrefix = "";
            int stt = 1;

            GetTreeList(ref bcList, parentInfo, sttPrefix, stt, dtList);

        }

        public static void CalculateBaoCaoCapTinh(ref List<ThongKeInfo> bcList, DateTime startDate, DateTime endDate, int tinhID, ref int total)
        {

            List<TKDonThuInfo> dtList = new ThongKeDonThu().GetDonThuByTinh(startDate, endDate, tinhID).ToList();
            total = dtList.Count;

            List<LoaiKhieuToInfo> parentList = new LoaiKhieuTo().GetLoaiKhieuToWithLevel1().ToList();
            List<LoaiKhieuToInfo> treeList = new List<LoaiKhieuToInfo>();
            string sttPrefix = "";
            int stt = 1;
            foreach (LoaiKhieuToInfo parentInfo in parentList)
            {
                GetTreeList(ref bcList, parentInfo, sttPrefix, stt, dtList);
                stt++;
            }
        }

        public static void CalculateBaoCaoCapTinh(ref List<ThongKeInfo> bcList, DateTime startDate, DateTime endDate, int tinhID, int lktID, ref int total)
        {

            List<TKDonThuInfo> dtList = new ThongKeDonThu().GetDonThuByTinh(startDate, endDate, tinhID).ToList();
            foreach (TKDonThuInfo dtInfo in dtList)
            {
                if (dtInfo.LoaiKhieuTo1ID == lktID || dtInfo.LoaiKhieuTo2ID == lktID || dtInfo.LoaiKhieuTo3ID == lktID)
                {
                    total++;
                }
            }

            LoaiKhieuToInfo parentInfo = new LoaiKhieuTo().GetLoaiKhieuToByID(lktID);
            string sttPrefix = "";
            int stt = 1;

            GetTreeList(ref bcList, parentInfo, sttPrefix, stt, dtList);

        }

        public static void CalculateBaoCaoTheoCoQuan(ref List<ThongKeInfo> bcList, DateTime startDate, DateTime endDate, int cqID, ref int total)
        {

            List<TKDonThuInfo> dtList = new ThongKeDonThu().GetDonThuByCoQuan(startDate, endDate, cqID).ToList();
            total = dtList.Count;

            List<LoaiKhieuToInfo> parentList = new LoaiKhieuTo().GetLoaiKhieuToWithLevel1().ToList();
            List<LoaiKhieuToInfo> treeList = new List<LoaiKhieuToInfo>();
            string sttPrefix = "";
            int stt = 1;
            foreach (LoaiKhieuToInfo parentInfo in parentList)
            {
                GetTreeList(ref bcList, parentInfo, sttPrefix, stt, dtList);
                stt++;
            }
        }

        public static void CalculateBaoCaoTheoCoQuan(ref List<ThongKeInfo> bcList, DateTime startDate, DateTime endDate, int cqID, int lktID, ref int total)
        {

            List<TKDonThuInfo> dtList = new ThongKeDonThu().GetDonThuByCoQuan(startDate, endDate, cqID).ToList();
            foreach (TKDonThuInfo dtInfo in dtList)
            {
                if (dtInfo.LoaiKhieuTo1ID == lktID || dtInfo.LoaiKhieuTo2ID == lktID || dtInfo.LoaiKhieuTo3ID == lktID)
                {
                    total++;
                }
            }

            LoaiKhieuToInfo parentInfo = new LoaiKhieuTo().GetLoaiKhieuToByID(lktID);
            string sttPrefix = "";
            int stt = 1;

            GetTreeList(ref bcList, parentInfo, sttPrefix, stt, dtList);

        }

        public static void TinhBaoCaoTheoHuyen(ref List<ThongKeInfo> bcList, DateTime startDate, DateTime endDate, int huyenID, int lktID, ref int total)
        {
            List<CoQuanInfo> cqList = new CoQuan().GetByHuyen(huyenID).ToList();
            List<TKDonThuInfo> resultList = new List<TKDonThuInfo>();

            foreach (CoQuanInfo cqInfo in cqList)
            {
                List<TKDonThuInfo> dtList = new ThongKeDonThu().GetDonThuByCoQuan(startDate, endDate, cqInfo.CoQuanID).ToList();
                resultList.AddRange(dtList);
            }

            foreach (TKDonThuInfo dtInfo in resultList)
            {
                if (dtInfo.LoaiKhieuTo1ID == lktID || dtInfo.LoaiKhieuTo2ID == lktID || dtInfo.LoaiKhieuTo3ID == lktID || lktID == 0)
                {
                    total++;
                }
            }


            string sttPrefix = "";
            int stt = 1;

            if (lktID == 0)
            {
                List<LoaiKhieuToInfo> parentList = new LoaiKhieuTo().GetLoaiKhieuToWithLevel1().ToList();
                foreach (LoaiKhieuToInfo parentInfo in parentList)
                {
                    GetTreeList(ref bcList, parentInfo, sttPrefix, stt, resultList);
                }
            }
            else
            {
                LoaiKhieuToInfo parentInfo = new LoaiKhieuTo().GetLoaiKhieuToByID(lktID);
                GetTreeList(ref bcList, parentInfo, sttPrefix, stt, resultList);
            }
        }

        public static void TinhBaoCaoTheoXa(ref List<ThongKeInfo> bcList, DateTime startDate, DateTime endDate, int xaID, int lktID, ref int total)
        {
            List<CoQuanInfo> cqList = new CoQuan().GetByXa(xaID).ToList();
            List<TKDonThuInfo> resultList = new List<TKDonThuInfo>();

            foreach (CoQuanInfo cqInfo in cqList)
            {
                List<TKDonThuInfo> dtList = new ThongKeDonThu().GetDonThuByCoQuan(startDate, endDate, cqInfo.CoQuanID).ToList();
                resultList.AddRange(dtList);
            }

            foreach (TKDonThuInfo dtInfo in resultList)
            {
                if (dtInfo.LoaiKhieuTo1ID == lktID || dtInfo.LoaiKhieuTo2ID == lktID || dtInfo.LoaiKhieuTo3ID == lktID || lktID == 0)
                {
                    total++;
                }
            }


            string sttPrefix = "";
            int stt = 1;

            if (lktID == 0)
            {
                List<LoaiKhieuToInfo> parentList = new LoaiKhieuTo().GetLoaiKhieuToWithLevel1().ToList();
                foreach (LoaiKhieuToInfo parentInfo in parentList)
                {
                    GetTreeList(ref bcList, parentInfo, sttPrefix, stt, resultList);
                }
            }
            else
            {
                LoaiKhieuToInfo parentInfo = new LoaiKhieuTo().GetLoaiKhieuToByID(lktID);
                GetTreeList(ref bcList, parentInfo, sttPrefix, stt, resultList);
            }
        }

        private static void GetTreeList(ref List<ThongKeInfo> bcList, LoaiKhieuToInfo parentInfo, string sttPrefix, int stt, List<TKDonThuInfo> dtList)
        {
            ThongKeInfo bcInfo = new ThongKeInfo();
            bcInfo.STT = sttPrefix + stt;
            bcInfo.LoaiKhieuToID = parentInfo.LoaiKhieuToID;
            bcInfo.TiepDanKhongDon = parentInfo.TiepDanKhongDon;
            bcInfo.DonTrucTiep = parentInfo.DonTrucTiep;
            bcInfo.DonGianTiep = parentInfo.DonGianTiep;
            if (parentInfo.Cap == 1)
            {
                bcInfo.Ten = "<b>" + parentInfo.TenLoaiKhieuTo + "</b>";
            }
            else
            {
                bcInfo.Ten = parentInfo.TenLoaiKhieuTo;
            }
            bcInfo.SoLuong = bcInfo.TiepDanKhongDon + bcInfo.DonTrucTiep + bcInfo.DonGianTiep;
            //if (dtList.Count > 0)
            //{
            //    bcInfo.TyLe = (double)bcInfo.SoLuong / dtList.Count;
            //}
            //else { bcInfo.TyLe = dtList.Count; }
            bcList.Add(bcInfo);
            sttPrefix = bcInfo.STT + ".";
            List<LoaiKhieuToInfo> childList = new LoaiKhieuTo().GetLoaiKhieuToByParentID(parentInfo.LoaiKhieuToID).ToList();
            foreach (LoaiKhieuToInfo childInfo in childList)
            {
                GetTreeList(ref bcList, childInfo, sttPrefix, stt, dtList);
                stt++;
            }
        }
        private static void GetTreeList1(ref List<ThongKeInfo> bcList, LoaiKhieuToInfo parentInfo, string sttPrefix, int stt, List<LoaiKhieuToInfo> dtList, DateTime startDate, DateTime endDate, int Type
               , List<int> ListCoQuanID)
        {
            ThongKeInfo bcInfo = new ThongKeInfo();
            bcInfo.STT = sttPrefix + stt;
            bcInfo.LoaiKhieuToID = parentInfo.LoaiKhieuToID;
            var lkt = dtList.Where(x => x.LoaiKhieuToID == bcInfo.LoaiKhieuToID).ToList();
            bcInfo.TiepDanKhongDon = lkt.Select(x => x.TiepDanKhongDon).Sum();/* parentInfo.TiepDanKhongDon;*/
            bcInfo.DonTrucTiep = lkt.Select(x => x.DonTrucTiep).Sum();
            bcInfo.DonGianTiep = lkt.Select(x => x.DonGianTiep).Sum();
            bcInfo.Cap = parentInfo.Cap;
            if (parentInfo.Cap == 1)
            {
                bcInfo.Ten = "" + parentInfo.TenLoaiKhieuTo + "";
            }
            else
            {
                bcInfo.Ten = parentInfo.TenLoaiKhieuTo;
            }
            bcInfo.SoLuong = bcInfo.TiepDanKhongDon + bcInfo.DonTrucTiep + bcInfo.DonGianTiep;
            //if (dtList.Count > 0)
            //{
            //    bcInfo.TyLe = (double)bcInfo.SoLuong / dtList.Count;
            //}
            //else { bcInfo.TyLe = dtList.Count; }
            bcList.Add(bcInfo);
            sttPrefix = bcInfo.STT + ".";
            List<LoaiKhieuToInfo> childList = new LoaiKhieuTo().GetLoaiKhieuToByParentID(parentInfo.LoaiKhieuToID).ToList();
            foreach (LoaiKhieuToInfo childInfo in childList)
            {
                var ListLoaiKhieuToID = new List<int> { childInfo.LoaiKhieuToID };
                List<LoaiKhieuToInfo> dtList1 = new LoaiKhieuTo().ThongKeLoaiKhieuTo(startDate, endDate, ListCoQuanID, ListLoaiKhieuToID, childInfo.Cap).ToList();
                GetTreeList1(ref bcList, childInfo, sttPrefix, stt, dtList1, startDate, endDate, childInfo.Cap, ListCoQuanID);
                stt++;
            }
        }
        private static void GetTreeList_New(ref List<ThongKeInfo> bcList, LoaiKhieuToInfo parentInfo, string sttPrefix, int stt, List<LoaiKhieuToInfo> dtList, List<LoaiKhieuToInfo> LChaCon)
        {
            ThongKeInfo bcInfo = new ThongKeInfo();
            bcInfo.STT = sttPrefix + stt;
            bcInfo.LoaiKhieuToID = parentInfo.LoaiKhieuToID;
            var lkt = dtList.Where(x => x.LoaiKhieuToID == bcInfo.LoaiKhieuToID).ToList();
            bcInfo.TiepDanKhongDon = lkt.Select(x => x.TiepDanKhongDon).Sum();/* parentInfo.TiepDanKhongDon;*/
            bcInfo.DonTrucTiep = lkt.Select(x => x.DonTrucTiep).Sum();
            bcInfo.DonGianTiep = lkt.Select(x => x.DonGianTiep).Sum();
            bcInfo.Cap = parentInfo.Cap;
            if (parentInfo.Cap == 1)
            {
                bcInfo.Ten = "" + parentInfo.TenLoaiKhieuTo + "";
            }
            else
            {
                bcInfo.Ten = parentInfo.TenLoaiKhieuTo;
            }
            bcInfo.SoLuong = bcInfo.TiepDanKhongDon + bcInfo.DonTrucTiep + bcInfo.DonGianTiep;
            //if (dtList.Count > 0)
            //{
            //    bcInfo.TyLe = (double)bcInfo.SoLuong / dtList.Count;
            //}
            //else { bcInfo.TyLe = dtList.Count; }
            if (LChaCon.Any(x => x.Cap == 1))
            {
                if (bcInfo.SoLuong > 0 || parentInfo.Cap == 1)
                {
                    bcList.Add(bcInfo);
                }
            }
            else if (LChaCon.Any(x => x.Cap == 2))
            {
                if (bcInfo.SoLuong > 0 || parentInfo.Cap == 2)
                {
                    bcList.Add(bcInfo);
                }
            }
            else
            {
                bcList.Add(bcInfo);
            }


            sttPrefix = bcInfo.STT + ".";
            List<LoaiKhieuToInfo> childList = LChaCon.Where(x => x.LoaiKhieuToCha == parentInfo.LoaiKhieuToID).ToList();
            foreach (LoaiKhieuToInfo childInfo in childList)
            {
                GetTreeList_New(ref bcList, childInfo, sttPrefix, stt, dtList, LChaCon);
                stt++;
            }
        }

        private static int CalculateSoLuong(List<TKDonThuInfo> dtList, LoaiKhieuToInfo lktInfo)
        {
            int result = 0;
            foreach (TKDonThuInfo dtInfo in dtList)
            {
                if (lktInfo.Cap == 1)
                {
                    if (dtInfo.LoaiKhieuTo1ID == lktInfo.LoaiKhieuToID)
                    {
                        result++;
                    }
                }
                else if (lktInfo.Cap == 2)
                {
                    if (dtInfo.LoaiKhieuTo2ID == lktInfo.LoaiKhieuToID)
                    {
                        result++;
                    }
                }
                else if (lktInfo.Cap >= 3)
                {
                    if (dtInfo.LoaiKhieuTo3ID == lktInfo.LoaiKhieuToID)
                    {
                        result++;
                    }
                }
            }
            return result;
        }
        public static void ThongKeTheoLoaiKhieuTo(ref List<ThongKeInfo> bcList, List<LoaiKhieuToInfo> LKT, List<int> ListLoaiKhieuToID, List<int> ListCoQuanID, DateTime startDate, DateTime endDate, int tinhID, ref int total, int Type)
        {

            //
            List<LoaiKhieuToInfo> dtList = new LoaiKhieuTo().ThongKeLoaiKhieuTo(startDate, endDate, ListCoQuanID, ListLoaiKhieuToID, Type).ToList();
            total = dtList.Select(x => x.TiepDanKhongDon).Sum() + dtList.Select(x => x.DonTrucTiep).Sum() + dtList.Select(x => x.DonGianTiep).Sum();
            //var dtList = new List<TKDonThuInfo>();
            List<LoaiKhieuToInfo> parentListC1 = LKT.Where(x => x.Cap == 1).ToList();
            List<LoaiKhieuToInfo> parentListC2 = LKT.Where(x => x.Cap == 2).ToList();
            //List<LoaiKhieuToInfo> treeList = new List<LoaiKhieuToInfo>();
            string sttPrefix = "";
            int stt = 1;
            if (Type == 0)
            {
                List<LoaiKhieuToInfo> dtList1 = new LoaiKhieuTo().ThongKeLoaiKhieuTo(startDate, endDate, ListCoQuanID, ListLoaiKhieuToID, 1).ToList();
                total = dtList.Count;

                List<LoaiKhieuToInfo> parentList = new LoaiKhieuTo().GetLoaiKhieuToWithLevel1().ToList();
                List<LoaiKhieuToInfo> treeList = new List<LoaiKhieuToInfo>();
                //string sttPrefix = "";
                //int stt = 1;
                foreach (LoaiKhieuToInfo parentInfo in parentList)
                {
                    GetTreeList1(ref bcList, parentInfo, sttPrefix, stt, dtList1, startDate, endDate, Type, ListCoQuanID);
                    stt++;
                }
            }
            else if (parentListC1.Count > 0)
            {
                foreach (LoaiKhieuToInfo parentInfo in parentListC1)
                {
                    GetTreeList_New(ref bcList, parentInfo, sttPrefix, stt, dtList, LKT);
                    stt++;
                }
            }
            else if (parentListC2.Count > 0)
            {
                foreach (LoaiKhieuToInfo parentInfo in parentListC2)
                {
                    GetTreeList_New(ref bcList, parentInfo, sttPrefix, stt, dtList, LKT);
                    stt++;
                }
            }
            else
            {
                foreach (LoaiKhieuToInfo parentInfo in LKT)
                {
                    GetTreeList_New(ref bcList, parentInfo, sttPrefix, stt, dtList, LKT);
                    stt++;
                }
            }

        }
    }
}
