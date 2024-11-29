using Com.Gosol.KNTC.DAL.HeThong;
using Com.Gosol.KNTC.DAL.KNTC;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.HeThong;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Ultilities;
using DocumentFormat.OpenXml.Bibliography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.BUS.KNTC
{
    public class KetQuaTranhChapBUS
    {
        public IList<ChuyenXuLyInfo> GetBySearch(ref int TotalRow, BasePagingParamsForFilter p, IdentityHelper IdentityHelper)
        {
            int cr = p.PageNumber;
            if (cr == 0)
            {
                cr = 1;
            }
            int start = (cr - 1) * p.PageSize;
            int end = cr * p.PageSize;

            QueryFilterInfo filter = new QueryFilterInfo();
            filter.CoQuanID = IdentityHelper.CoQuanID ?? 0;
            filter.KeyWord = "%" + p.Keyword + "%";
            filter.StateName = Constant.Ket_Thuc;
            filter.PrevStateName = Constant.CV_TiepNhan;
            filter.TrangThaiKQ = p.TrangThai ?? 0;
            filter.LoaiKhieuToID = Constant.TranhChap;
            filter.HuongXuLyID = (int)HuongGiaiQuyetEnum.DeXuatThuLy;

            IList<ChuyenXuLyInfo> ListInfo = new List<ChuyenXuLyInfo>();
            try
            {
                if (filter.TrangThaiKQ == 0)
                {
                    ListInfo = new ChuyenXuLy().GetDSDonThuTranhChap(filter, start, end).ToList();
                }
                else if (filter.TrangThaiKQ == 1)
                {
                    ListInfo = new ChuyenXuLy().GetDSDonThuTranhChap(filter, start, end).Where(x => x.KetQuaTranhChapID == 0).OrderByDescending(x => x.XuLyDonID).ToList();
                }
                else
                {
                    ListInfo = new ChuyenXuLy().GetDSDonThuTranhChap(filter, start, end).Where(x => x.KetQuaTranhChapID != 0).OrderByDescending(x => x.XuLyDonID).ToList();
                }

                if (ListInfo.Count > 0)
                {
                    foreach (var item in ListInfo)
                    {
                        //Don thuoc tham quyen giai quyet
                        if (item.CoQuanID == IdentityHelper.CoQuanID)
                        {
                            item.ThuocThamQuyen = "1";
                        }
                        item.NgayPhanCongStr = Format.FormatDate(item.NgayPhan);
                        if (item.KetQuaTranhChapID_Str != string.Empty)
                        {
                            item.TrangThaiStr = "Đã có kết quả";
                            item.TrangThaiCssClass = "dabanhanh";
                        }
                        else
                        {
                            item.TrangThaiStr = "Chưa có kết quả";
                        }

                        int donthuID = item.DonThuID;
                        DonThuInfo dt = new DonThuInfo();
                        try
                        {
                            dt = new DonThuDAL().GetByID(donthuID);

                        }
                        catch { }

                        int nhomKNID = dt.NhomKNID;
                        if (nhomKNID > 0)
                        {
                            item.listDoiTuongKN = new DoiTuongKN().GetByNhomKNID(nhomKNID).ToList();
                            //dt.HoTen = "";
                            //List<DoiTuongKNInfo> ltInfo = new DAL.DoiTuongKN().GetByNhomKNID(nhomKNID).ToList();
                            //int count = 0;

                            //foreach (var doituong in ltInfo)
                            //{
                            //    string tenTinh, tenHuyen, tenXa;

                            //    DoiTuongKNInfo diaChiInfo = new DAL.DoiTuongKN().GetDiaChiDTKhieuNai(doituong.DoiTuongKNID);


                            //    tenTinh = ", " + diaChiInfo.TenTinh;
                            //    tenHuyen = ", " + diaChiInfo.TenHuyen;


                            //    if (diaChiInfo.DiaChiCT != null)
                            //        tenXa = ", " + diaChiInfo.TenXa;
                            //    else
                            //        tenXa = diaChiInfo.TenXa;

                            //    dt.HoTen += diaChiInfo.HoTen + "(" + diaChiInfo.DiaChiCT + ")";
                            //    count++;
                            //    if (count >= ltInfo.Count())
                            //        break;
                            //    else
                            //        dt.HoTen += ", <br/>";
                            //}

                            //if (ltInfo.Count > 0)
                            //    item.TenChuDonStr = dt.HoTen;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

            TotalRow = new ChuyenXuLy().CountSearchDSTranhChap(filter);
            return ListInfo;
        }

        public BaseResultModel NhapKetQuaTranhChap(KetQuaTranhChapInfo kq)
        {
            var Result = new BaseResultModel();

            int val = 0;
            int xldId = kq.XuLyDonID;
            //KetQuaTranhChapInfo kq = new KetQuaTranhChapInfo();
            //List<HoiDongHoaGiaiInfo> hds = new List<HoiDongHoaGiaiInfo>();
            //string file_upload = fileUrl;
            if (xldId > 0)
            {
                KetQuaTranhChapInfo kq_exists = new KetQuaTranhChapInfo();
                kq_exists = new KetQuaTranhChapDAL().GetKQByXuLyDonID(xldId);
                int ketQuaTranhChapID = 0;
                #region == Data
                //kq.CoQuanID = IdentityHelper.GetCoQuanID();
                //kq.CanBoID = IdentityHelper.GetCanBoID();
                //kq.XuLyDonID = xuLydonID;
                //kq.NDHoaGiai = ndHoaGiai;
                //kq.KetQuaHoaGiai = ketQuaHoaGiai;
                //kq.FileUrl = fileUrl;
                kq.NgayRaKQ = DateTime.Now;

                //string[] tenCanBo = tenCanBos.Split('*');
                //string[] nhiemVu = nhiemVus.Split('*');
                //for (int i = 0; i < tenCanBo.Length; i++)
                //{
                //    HoiDongHoaGiaiInfo hd = new HoiDongHoaGiaiInfo();
                //    hd.TenCanBo = tenCanBo[i];
                //    hd.NhiemVu = nhiemVu[i];
                //    hds.Add(hd);
                //}
                #endregion

                if (kq_exists == null)
                {
                    val = new KetQuaTranhChapDAL().Insert(kq, kq.lstHoiDong);
                    ketQuaTranhChapID = val;
                    Result.Data = ketQuaTranhChapID;               
                    //file dinh kem
                    if (kq.DanhSachHoSoTaiLieu != null && kq.DanhSachHoSoTaiLieu.Count > 0)
                    {
                        foreach (var item in kq.DanhSachHoSoTaiLieu)
                        {
                            if (item.DanhSachFileDinhKemID != null && item.DanhSachFileDinhKemID.Count > 0)
                            {
                                new FileDinhKemDAL().UpdateFileTranhChap(item.DanhSachFileDinhKemID, val);
                            }
                        }

                    }
                }
                else
                {
                    kq.KetQuaTranhChapID = kq_exists.KetQuaTranhChapID;
                    ketQuaTranhChapID = kq_exists.KetQuaTranhChapID;
                    val = new KetQuaTranhChapDAL().Update(kq, kq.lstHoiDong);
                    Result.Data = kq.KetQuaTranhChapID;
                    //file dinh kem
                    if (kq.DanhSachHoSoTaiLieu != null && kq.DanhSachHoSoTaiLieu.Count > 0)
                    {
                        foreach (var item in kq.DanhSachHoSoTaiLieu)
                        {
                            if (item.DanhSachFileDinhKemID != null && item.DanhSachFileDinhKemID.Count > 0)
                            {
                                new FileDinhKemDAL().UpdateFileTranhChap(item.DanhSachFileDinhKemID, kq.KetQuaTranhChapID);
                            }
                        }

                    }

                }
            }
            Result.Status = 1;
            Result.Message = "Nhập kết quả tranh chấp thành công";
            return Result;
        }

        public KetQuaTranhChapInfo GetByID(int XuLyDonID)
        {
            KetQuaTranhChapInfo Info = new KetQuaTranhChapInfo();
            Info = new KetQuaTranhChapDAL().GetKQByXuLyDonID(XuLyDonID);
            var fileDinhKem = new KetQuaTranhChapDAL().GetFileByXuLyDonID(XuLyDonID).ToList();
            if (fileDinhKem.Count > 0)
            {
                Info.DanhSachHoSoTaiLieu = new List<DanhSachHoSoTaiLieu>();
                Info.DanhSachHoSoTaiLieu = fileDinhKem.GroupBy(p => p.GroupUID)
                   .Select(g => new DanhSachHoSoTaiLieu
                   {
                       GroupUID = g.Key,
                       TenFile = g.FirstOrDefault().TenFile,
                       NoiDung = g.FirstOrDefault().TomTat,
                       TenNguoiCapNhat = g.FirstOrDefault().TenCanBo,
                       NguoiCapNhatID = g.FirstOrDefault().CanBoUpID,
                       NgayCapNhat = g.FirstOrDefault().NgayCapNhat,
                       FileDinhKem = fileDinhKem.Where(x => x.GroupUID == g.Key && x.KetQuaTranhChapID > 0).GroupBy(x => x.KetQuaTranhChapID)
                                       .Select(y => new FileDinhKemModel
                                       {
                                           FileID = y.FirstOrDefault().KetQuaTranhChapID,
                                           TenFile = y.FirstOrDefault().TenFile,
                                           NgayCapNhat = y.FirstOrDefault().NgayCapNhat,
                                           NguoiCapNhat = y.FirstOrDefault().CanBoUpID,
                                           //FileType = y.FirstOrDefault().FileType,
                                           FileUrl = y.FirstOrDefault().FileUrl,
                                       }
                                       ).ToList(),

                   }
                   ).ToList();
            }

            return Info;
        }
    }


}
