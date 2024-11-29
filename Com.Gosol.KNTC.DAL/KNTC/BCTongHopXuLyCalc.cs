using Com.Gosol.KNTC.Model.HeThong;
using Com.Gosol.KNTC.Models.KNTC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.DAL.KNTC
{
    public static class BCTongHopXuLyCalc
    {
        // chuyen bus bao cao ngay 24/01/2019
        public static List<BCTongHopXuLyInfo> Calculate(DateTime startDate, DateTime endDate, int coQuanID, int phongBanID)
        {
            List<BCTongHopXuLyInfo> resultList = new List<BCTongHopXuLyInfo>();
            List<CanBoInfo> canBoList = new CanBo().GetByCoQuanID(coQuanID).ToList();
            List<PhongBanInfo> phongBanList = new PhongBan().GetByCoQuanID(coQuanID).ToList();

            var dtList = new DTXuLy().GetDonThuDaXuLy(startDate, endDate, coQuanID);
            var tiepDanList = new TiepDanKhongDon().GetInTime(startDate, endDate, coQuanID);

            int stt = 1;


            foreach (var phongBanInfo in phongBanList)
            {
                BCTongHopXuLyInfo phongBCInfo = new BCTongHopXuLyInfo();
                phongBCInfo.TenCanBo = phongBanInfo.TenPhongBan;
                phongBCInfo.Level = 1;
                phongBCInfo.STT = stt.ToString();
                phongBCInfo.PhongBanID = phongBanInfo.PhongBanID;
                phongBCInfo.CanBoID = 0;
                resultList.Add(phongBCInfo);

                var canBoInPhongList = canBoList.Where(x => x.PhongBanID == phongBanInfo.PhongBanID).ToList();
                int childSTT = 1;
                foreach (var canBoInfo in canBoInPhongList)
                {
                    BCTongHopXuLyInfo bcInfo = new BCTongHopXuLyInfo();
                    bcInfo.Level = 2;
                    bcInfo.TenCanBo = canBoInfo.TenCanBo;
                    bcInfo.PhongBanID = canBoInfo.PhongBanID;
                    bcInfo.CanBoID = canBoInfo.CanBoID;

                    bcInfo.SLTiepCongDan = tiepDanList.Where(x => x.CanBoTiepID == canBoInfo.CanBoID).Count();
                    bcInfo.SLDonXuLy = dtList.Where(x => x.CanBoXuLyID == canBoInfo.CanBoID).Count();
                    bcInfo.SLDonXuLyTrongHan = dtList.Where(x => x.CanBoXuLyID == canBoInfo.CanBoID && x.NgayXuLy <= x.NgayQuaHan).Count();
                    bcInfo.SLDonXuLyQuaHan = dtList.Where(x => x.CanBoXuLyID == canBoInfo.CanBoID && x.NgayXuLy > x.NgayQuaHan).Count();
                    bcInfo.STT = stt.ToString() + "." + childSTT++;

                    phongBCInfo.SLTiepCongDan += bcInfo.SLTiepCongDan;
                    phongBCInfo.SLDonXuLy += bcInfo.SLDonXuLy;
                    phongBCInfo.SLDonXuLyTrongHan += bcInfo.SLDonXuLyTrongHan;
                    phongBCInfo.SLDonXuLyQuaHan += bcInfo.SLDonXuLyQuaHan;

                    resultList.Add(bcInfo);
                }

                stt++;
            }

            return resultList;
        }

        public static List<BCTongHopXuLyInfo> CalculateBCTongHopKQTDXLD(DateTime startDate, DateTime endDate, int coQuanID, int phongBanID, ref BCTongHopXuLyInfo tongInfo, int loaiDon, ref string testTime)
        {
            testTime += "Begin: " + DateTime.Now.ToString("HH:mm:ss") + "; ";
            List<BCTongHopXuLyInfo> resultList = new List<BCTongHopXuLyInfo>();
            QueryFilterInfo infoQF = new QueryFilterInfo()
            {
                TuNgayGoc = startDate,
                TuNgayMoi = startDate,
                DenNgayGoc = endDate.AddDays(1),
                DenNgayMoi = endDate.AddDays(1),
                CoQuanID = coQuanID,
                LoaiDon = loaiDon
            };

            IList<BCTongHopXuLyInfo> xldCanBoList = new BCTongHopKQTDXLD().DSCanBo_XuLyDon_GetByDate(infoQF);
            testTime += "xldCanBoList: " + DateTime.Now.ToString("HH:mm:ss") + "; ";
            IList<BCTongHopXuLyInfo> gqdCanBoList = new BCTongHopKQTDXLD().DSCanBo_GiaiQuyetDon_GetByDate(infoQF);
            testTime += "gqdCanBoList: " + DateTime.Now.ToString("HH:mm:ss") + "; ";
            IList<BCTongHopXuLyInfo> gqdPhongBanList = new BCTongHopKQTDXLD().DSPhongBan_GiaiQuyetDon_GetByDate(infoQF);
            testTime += "gqdPhongBanList: " + DateTime.Now.ToString("HH:mm:ss") + "; ";
            IList<BCTongHopXuLyInfo> gqdCoQuanList = new BCTongHopKQTDXLD().DSCoQuan_GiaiQuyetDon_GetByDate(infoQF);
            testTime += "gqdCoQuanList: " + DateTime.Now.ToString("HH:mm:ss") + "; ";
            List<CanBoInfo> canBoList = new CanBo().GetByCoQuanID(coQuanID).ToList();
            testTime += "canBoList: " + DateTime.Now.ToString("HH:mm:ss") + "; ";
            List<PhongBanInfo> phongBanList = new PhongBan().GetByCoQuanID(coQuanID).ToList();
            testTime += "phongBanList: " + DateTime.Now.ToString("HH:mm:ss") + "; ";
            foreach (CanBoInfo cbInfo in canBoList)
            {
                if (cbInfo.PhongBanID == 0)
                {
                    phongBanList.Insert(0, new PhongBanInfo() { TenPhongBan = "Chưa phân phòng ban", PhongBanID = 0 });
                    break;
                }
            }

            int stt = 1;
            foreach (PhongBanInfo phongBanInfo in phongBanList)
            {
                BCTongHopXuLyInfo phongBCInfo = new BCTongHopXuLyInfo();
                phongBCInfo.TenCanBo = phongBanInfo.TenPhongBan;
                phongBCInfo.Level = 1;
                phongBCInfo.STT = stt.ToString();
                phongBCInfo.PhongBanID = phongBanInfo.PhongBanID;
                phongBCInfo.CanBoID = 0;
                resultList.Add(phongBCInfo);

                var canBoInPhongList = canBoList.Where(x => x.PhongBanID == phongBanInfo.PhongBanID).ToList();
                int childSTT = 1;
                foreach (var canBoInfo in canBoInPhongList)
                {
                    BCTongHopXuLyInfo bcInfo = new BCTongHopXuLyInfo();
                    bcInfo.Level = 2;
                    bcInfo.TenCanBo = canBoInfo.TenCanBo;
                    bcInfo.PhongBanID = canBoInfo.PhongBanID;
                    bcInfo.CanBoID = canBoInfo.CanBoID;
                    bcInfo.STT = stt.ToString() + "." + childSTT++;
                    foreach (BCTongHopXuLyInfo rawInfo in xldCanBoList)
                    {
                        if (canBoInfo.CanBoID == rawInfo.CanBoID)
                        {
                            bcInfo.SLTiepCongDan = rawInfo.SLTiepCongDan;
                            bcInfo.XLDTongSo = rawInfo.XLDTongSo;
                            bcInfo.XLDChuaXuLy = rawInfo.XLDChuaXuLy;
                            bcInfo.XLDDaXuLyTrongHan = rawInfo.XLDDaXuLyTrongHan;
                            bcInfo.XLDDaXuLyQuaHan = rawInfo.XLDDaXuLyQuaHan;
                            bcInfo.XLDDaXuLy = rawInfo.XLDDaXuLy;

                            if (phongBCInfo.PhongBanID != 0)
                            {
                                phongBCInfo.SLTiepCongDan += rawInfo.SLTiepCongDan;
                                phongBCInfo.XLDTongSo += rawInfo.XLDTongSo;
                                phongBCInfo.XLDChuaXuLy += rawInfo.XLDChuaXuLy;
                                phongBCInfo.XLDDaXuLyTrongHan += rawInfo.XLDDaXuLyTrongHan;
                                phongBCInfo.XLDDaXuLyQuaHan += rawInfo.XLDDaXuLyQuaHan;
                                phongBCInfo.XLDDaXuLy += rawInfo.XLDDaXuLy;
                            }
                            tongInfo.SLTiepCongDan += rawInfo.SLTiepCongDan;
                            tongInfo.XLDTongSo += rawInfo.XLDTongSo;
                            tongInfo.XLDChuaXuLy += rawInfo.XLDChuaXuLy;
                            tongInfo.XLDDaXuLyTrongHan += rawInfo.XLDDaXuLyTrongHan;
                            tongInfo.XLDDaXuLyQuaHan += rawInfo.XLDDaXuLyQuaHan;
                            tongInfo.XLDDaXuLy += rawInfo.XLDDaXuLy;

                            foreach (BCTongHopXuLyInfo gqdInfo in gqdCanBoList)
                            {
                                if (canBoInfo.CanBoID == gqdInfo.CanBoID)
                                {
                                    bcInfo.GQDTongSo = gqdInfo.GQDTongSo;
                                    bcInfo.GQDGiaoPhongNV = gqdInfo.GQDGiaoPhongNV;
                                    bcInfo.GQDChuaGQ = gqdInfo.GQDChuaGQ;
                                    bcInfo.GQDDangGQTrongHan = gqdInfo.GQDDangGQTrongHan;
                                    bcInfo.GQDDangGQQuaHan = gqdInfo.GQDDangGQQuaHan;
                                    bcInfo.GQDDaGQ = gqdInfo.GQDDaGQ;
                                    bcInfo.GQDDangGQ = gqdInfo.GQDDangGQ;
                                    bcInfo.DaCoBaoCao = gqdInfo.DaCoBaoCao;
                                    break;
                                }
                            }


                            bcInfo.SLTiepCongDanStr = bcInfo.SLTiepCongDan.ToString();
                            bcInfo.XLDTongSoStr = bcInfo.XLDTongSo.ToString();
                            bcInfo.XLDChuaXuLyStr = bcInfo.XLDChuaXuLy.ToString();
                            bcInfo.XLDDaXuLyTrongHanStr = bcInfo.XLDDaXuLyTrongHan.ToString();
                            bcInfo.XLDDaXuLyQuaHanStr = bcInfo.XLDDaXuLyQuaHan.ToString();
                            bcInfo.XLDDaXuLyStr = bcInfo.XLDDaXuLy.ToString();
                            bcInfo.GQDTongSoStr = bcInfo.GQDTongSo.ToString();
                            bcInfo.GQDGiaoPhongNVStr = bcInfo.GQDGiaoPhongNV.ToString();
                            bcInfo.GQDChuaGQStr = bcInfo.GQDChuaGQ.ToString();
                            bcInfo.GQDDangGQTrongHanStr = bcInfo.GQDDangGQTrongHan.ToString();
                            bcInfo.GQDDangGQQuaHanStr = bcInfo.GQDDangGQQuaHan.ToString();
                            bcInfo.GQDDaGQStr = bcInfo.GQDDaGQ.ToString();
                            bcInfo.GQDDangGQStr = bcInfo.GQDDangGQ.ToString();
                            bcInfo.DaCoBaoCaoStr = bcInfo.DaCoBaoCao.ToString();
                            resultList.Add(bcInfo);
                            break;
                        }
                    }

                }
                foreach (BCTongHopXuLyInfo gqdInfo in gqdPhongBanList)
                {
                    if (phongBanInfo.PhongBanID == gqdInfo.PhongBanID)
                    {
                        if (phongBCInfo.PhongBanID != 0)
                        {
                            phongBCInfo.GQDTongSo = gqdInfo.GQDTongSo;
                            phongBCInfo.GQDGiaoPhongNV = gqdInfo.GQDGiaoPhongNV;
                            phongBCInfo.GQDChuaGQ = gqdInfo.GQDChuaGQ;
                            phongBCInfo.GQDDangGQTrongHan = gqdInfo.GQDDangGQTrongHan;
                            phongBCInfo.GQDDangGQQuaHan = gqdInfo.GQDDangGQQuaHan;
                            phongBCInfo.GQDDaGQ = gqdInfo.GQDDaGQ;
                            phongBCInfo.GQDDangGQ = gqdInfo.GQDDangGQ;
                            phongBCInfo.DaCoBaoCao = gqdInfo.DaCoBaoCao;
                        }
                    }

                }

                stt++;
            }
            foreach (BCTongHopXuLyInfo gqdInfo in gqdCoQuanList)
            {
                if (gqdInfo.CoQuanID == coQuanID || gqdInfo.CoQuanGiaiQuyetID == coQuanID || gqdInfo.CQPhoiHopID == coQuanID)
                {
                    tongInfo.GQDTongSo += gqdInfo.GQDTongSo;
                    tongInfo.GQDGiaoPhongNV += gqdInfo.GQDGiaoPhongNV;
                    tongInfo.GQDChuaGQ += gqdInfo.GQDChuaGQ;
                    tongInfo.GQDDangGQTrongHan += gqdInfo.GQDDangGQTrongHan;
                    tongInfo.GQDDangGQQuaHan += gqdInfo.GQDDangGQQuaHan;
                    tongInfo.GQDDaGQ += gqdInfo.GQDDaGQ;
                    tongInfo.GQDDangGQ += gqdInfo.GQDDangGQ;
                    tongInfo.DaCoBaoCao += gqdInfo.DaCoBaoCao;
                }
            }
            foreach (BCTongHopXuLyInfo phongBCInfo in resultList)
            {
                if (phongBCInfo.PhongBanID != 0)
                {
                    phongBCInfo.GQDTongSoStr = phongBCInfo.GQDTongSo.ToString();
                    phongBCInfo.GQDGiaoPhongNVStr = phongBCInfo.GQDGiaoPhongNV.ToString();
                    phongBCInfo.GQDChuaGQStr = phongBCInfo.GQDChuaGQ.ToString();
                    phongBCInfo.GQDDangGQTrongHanStr = phongBCInfo.GQDDangGQTrongHan.ToString();
                    phongBCInfo.GQDDangGQQuaHanStr = phongBCInfo.GQDDangGQQuaHan.ToString();
                    phongBCInfo.GQDDaGQStr = phongBCInfo.GQDDaGQ.ToString();
                    phongBCInfo.GQDDangGQStr = phongBCInfo.GQDDangGQ.ToString();
                    phongBCInfo.DaCoBaoCaoStr = phongBCInfo.DaCoBaoCao.ToString();

                    phongBCInfo.SLTiepCongDanStr = phongBCInfo.SLTiepCongDan.ToString();
                    phongBCInfo.XLDTongSoStr = phongBCInfo.XLDTongSo.ToString();
                    phongBCInfo.XLDChuaXuLyStr = phongBCInfo.XLDChuaXuLy.ToString();
                    phongBCInfo.XLDDaXuLyTrongHanStr = phongBCInfo.XLDDaXuLyTrongHan.ToString();
                    phongBCInfo.XLDDaXuLyQuaHanStr = phongBCInfo.XLDDaXuLyQuaHan.ToString();
                    phongBCInfo.XLDDaXuLyStr = phongBCInfo.XLDDaXuLy.ToString();
                }
            }
            testTime += "end: " + DateTime.Now.ToString("HH:mm:ss") + "; ";
            return resultList;
        }

    }
}
