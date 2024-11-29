using Com.Gosol.KNTC.DAL.HeThong;
using Com.Gosol.KNTC.DAL.KNTC;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.BaoCao;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Ultilities;
using DocumentFormat.OpenXml.EMMA;
using ICSharpCode.SharpZipLib.Core;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workflow;

namespace Com.Gosol.KNTC.BUS.KNTC
{
    public class QuanLyHoSoDonThuBUS
    {
        public IList<DonThuInfo> GetBySearch(ref int TotalRow, BasePagingParamsForFilter p, IdentityHelper IdentityHelper)
        {
            int cr = p.PageNumber;
            if (cr == 0)
            {
                cr = 1;
            }
            int start = (cr - 1) * p.PageSize;
            int end = cr * p.PageSize;
            int coquanID = IdentityHelper.CoQuanID ?? 0;
            int cq = p.CoQuanID ?? 0;
            if (cq > 0)
            {
                coquanID = cq;
            }

            int huyenID = IdentityHelper.HuyenID ?? 0;
            var listThanhTraTinh = new SystemConfigDAL().GetByKey("Thanh_Tra_Tinh_ID").ConfigValue.Split(',').ToList().Select(x => Utils.ConvertToInt32(x.ToString(), 0)).ToList();
            if(listThanhTraTinh.Contains(IdentityHelper.CoQuanID ?? 0) || IdentityHelper.CanBoID == 20 || IdentityHelper.CoQuanID == 1)
            {
                coquanID = -1;
                huyenID = 0;
            }
            IList<DonThuInfo> ListInfo = new DonThuDAL().GetBySearchHSDT(ref TotalRow, p.Keyword, p.LoaiKhieuToID ?? 0, p.TuNgay ?? DateTime.MinValue, p.DenNgay ?? DateTime.MinValue, coquanID, start, end, p.TrangThai, huyenID);
            foreach (var item in ListInfo)
            {
                item.NgayNhapDonStr = Format.FormatDate(item.NgayNhapDon);
                //if (item.NgayQuaHanGQ != Constant.DEFAULT_DATE)
                //{
                //    item.HanGQStr = Format.FormatDate(item.NgayQuaHanGQ);
                //    if (item.NgayQuaHanGQ < DateTime.Now)
                //    {
                //        item.CssClass = "quahan_giaiquyet";
                //    }
                //}
                //else item.HanGQStr = "";

                int nhomKNID = item.NhomKNID;
                if (nhomKNID > 0)
                {
                    item.listDoiTuongKN = new DoiTuongKN().GetByNhomKNID(nhomKNID).ToList();

                    //item.HoTen = "";
                    //List<DoiTuongKNInfo> ltInfo = new DoiTuongKN().GetByNhomKNID(nhomKNID).ToList();
                    //int count = 0;

                    //foreach (var doituong in ltInfo)
                    //{
                    //    string tenTinh, tenHuyen, tenXa;

                    //    DoiTuongKNInfo diaChiInfo = new DoiTuongKN().GetDiaChiDTKhieuNai(doituong.DoiTuongKNID);


                    //    tenTinh = ", " + diaChiInfo.TenTinh;
                    //    tenHuyen = ", " + diaChiInfo.TenHuyen;


                    //    if (diaChiInfo.DiaChiCT != null)
                    //        tenXa = ", " + diaChiInfo.TenXa;
                    //    else
                    //        tenXa = diaChiInfo.TenXa;

                    //    item.HoTen += diaChiInfo.HoTen + "(" + diaChiInfo.DiaChiCT + ")";
                    //    count++;
                    //    if (count >= ltInfo.Count())
                    //        break;
                    //    else
                    //        item.HoTen += ", <br/>";
                    //}

                    //item.HoTenStr = item.HoTen;
                }
            }
            //TotalRow = new DonThuDAL().CountHSDT(p.Keyword, p.LoaiKhieuToID ?? 0, p.TuNgay ?? DateTime.MinValue, p.DenNgay ?? DateTime.MinValue, coquanID, huyenID);

            return ListInfo;
        }

        public List<BuocInfo> ThongTinBoSung(int xuLyDonID)
        {
            string data = "";
            List<XuLyDonLog> lstState = new XuLyDonDAL().GetXuLyDonLogs(xuLyDonID);
            var stateName = WorkflowInstance.Instance.GetCurrentStateOfDocument(xuLyDonID);
            List<BuocInfo> buocInfo = new List<BuocInfo>();

            BuocInfo item = new BuocInfo() { Value = "0", TenBuoc = "Chọn bước thực hiện cần bổ sung hồ sơ" };
            buocInfo.Add(item);

            item = new BuocInfo() { Value = (int)EnumBuoc.ThongTinChung + "-" + "0", TenBuoc = "Thông tin chung", LoaiFile = EnumLoaiFile.FileHoSo.GetHashCode() };
            buocInfo.Add(item);

            if (stateName == Constant.LD_DuyetXuLy)
            {
                item = new BuocInfo() { Value = (int)EnumBuoc.XuLyDon + "-" + "0", TenBuoc = "Xử lý đơn", LoaiFile = EnumLoaiFile.FileYKienXuLy.GetHashCode() };
                buocInfo.Add(item);
            }
            else if (stateName == Constant.LD_Phan_GiaiQuyet || stateName == Constant.TP_Phan_GiaiQuyet || stateName == Constant.LD_CapDuoi_Phan_GiaiQuyet)
            {
                List<XuLyDonLog> isHas = lstState
                .Where(i =>
                (Constant.LD_DuyetXuLy.Contains(i.CurrentState)
                || Constant.LD_DuyetXuLy.Contains(i.NextState))).ToList();
                if (isHas.Count > 0)
                {
                    item = new BuocInfo() { Value = (int)EnumBuoc.XuLyDon + "-" + "0", TenBuoc = "Xử lý đơn", LoaiFile = EnumLoaiFile.FileYKienXuLy.GetHashCode() };
                    buocInfo.Add(item);
                }

                //item = new BuocInfo() { Value = (int)EnumBuoc.LDPhanGiaiQuyet + "-" + "0", TenBuoc = "Giao xác minh" };
                //buocInfo.Add(item);

                item = new BuocInfo() { Value = (int)EnumBuoc.YeuCauDoiThoai + "-" + "0", TenBuoc = "Yều cầu đối thoại", LoaiFile = EnumLoaiFile.FileGiaiQuyet.GetHashCode() };
                buocInfo.Add(item);
            }
            else if (stateName == Constant.LD_Duyet_GiaiQuyet || stateName == Constant.TruongDoan_GiaiQuyet || stateName == Constant.TP_DuyetGQ || stateName == Constant.LD_CQCapDuoiDuyetGQ)
            {
                List<XuLyDonLog> isHas = lstState
                .Where(i => (Constant.LD_DuyetXuLy.Contains(i.CurrentState)
                || Constant.LD_DuyetXuLy.Contains(i.NextState))).ToList();

                if (isHas.Count > 0)
                {
                    item = new BuocInfo() { Value = (int)EnumBuoc.XuLyDon + "-" + "0", TenBuoc = "Xử lý đơn" , LoaiFile = EnumLoaiFile.FileYKienXuLy.GetHashCode() };
                    buocInfo.Add(item);
                }

                List<XuLyDonLog> isHas_lqpgq = lstState
                .Where(i => Constant.LD_Phan_GiaiQuyet.Contains(i.CurrentState)
                || Constant.LD_Phan_GiaiQuyet.Contains(i.NextState)
                || Constant.TP_Phan_GiaiQuyet.Contains(i.NextState)
                || Constant.LD_CapDuoi_Phan_GiaiQuyet.Contains(i.CurrentState)
                || Constant.LD_CapDuoi_Phan_GiaiQuyet.Contains(i.NextState)
                || Constant.TP_Phan_GiaiQuyet.Contains(i.NextState)
                ).ToList();

                if (isHas_lqpgq.Count > 0)
                {
                    //item = new BuocInfo() { Value = (int)EnumBuoc.LDPhanGiaiQuyet + "-" + "0", TenBuoc = "Giao xác minh" };
                    //buocInfo.Add(item);
                }

                IList<BuocXacMinhInfo> buocXacMinhs = new BuocXacMinh().GetByLoaiKhieuToID(xuLyDonID);

                for (int i = 0; i < buocXacMinhs.Count; i++)
                {
                    item = new BuocInfo() { Value = (int)EnumBuoc.GiaiQuyetDon + "-" + buocXacMinhs[i].BuocXacMinhID.ToString(), TenBuoc = "Giải quyết đơn - " + buocXacMinhs[i].TenBuoc, LoaiFile = EnumLoaiFile.FileGiaiQuyet.GetHashCode() };
                    buocInfo.Add(item);
                }

                item = new BuocInfo() { Value = (int)EnumBuoc.YeuCauDoiThoai + "-" + "0", TenBuoc = "Yêu cầu đối thoại", LoaiFile = EnumLoaiFile.FileGiaiQuyet.GetHashCode() };
                buocInfo.Add(item);

                int bcxm = new TheoDoiXuLyDAL().CheckBaoCaoXacMinh(xuLyDonID);
                if (bcxm > 0)
                {
                    item = new BuocInfo() { Value = (int)EnumBuoc.BaoCaoXacMinh + "-" + "0", TenBuoc = "Báo cáo xác minh", LoaiFile = EnumLoaiFile.FileBCXM.GetHashCode() };
                    buocInfo.Add(item);
                }

                List<XuLyDonLog> isHas_dgq = lstState
                .Where(i => Constant.LD_Duyet_GiaiQuyet.Contains(i.CurrentState)
                   || Constant.LD_Duyet_GiaiQuyet.Contains(i.NextState)
                   || Constant.TP_DuyetGQ.Contains(i.NextState)
                   || Constant.TP_DuyetGQ.Contains(i.CurrentState)
                   || Constant.TruongDoan_GiaiQuyet.Contains(i.NextState)
                   || Constant.TruongDoan_GiaiQuyet.Contains(i.CurrentState)
                   || Constant.LD_CQCapDuoiDuyetGQ.Contains(i.CurrentState)
                   || Constant.LD_CQCapDuoiDuyetGQ.Contains(i.CurrentState)).ToList();

                if (isHas_dgq.Count > 0)
                {
                    item = new BuocInfo() { Value = (int)EnumBuoc.DuyetGiaiQuyet + "-" + "0", TenBuoc = "Phê duyệt kết quả xác minh", LoaiFile = EnumLoaiFile.FileDTCDGQ.GetHashCode() };
                    buocInfo.Add(item);
                }
            }
            else if (stateName == Constant.Ket_Thuc)
            {
                List<XuLyDonLog> isHas_xl = lstState
                .Where(i => Constant.LD_DuyetXuLy.Contains(i.NextState)
                || Constant.LD_DuyetXuLy.Contains(i.CurrentState)).ToList();
                if (isHas_xl.Count > 0)
                {
                    item = new BuocInfo() { Value = (int)EnumBuoc.XuLyDon + "-" + "0", TenBuoc = "Xử lý đơn" , LoaiFile = EnumLoaiFile.FileYKienXuLy.GetHashCode() };
                    buocInfo.Add(item);
                }

                List<XuLyDonLog> isHas_lqpgq = lstState
                .Where(i => (Constant.LD_Phan_GiaiQuyet.Contains(i.CurrentState)
                || Constant.LD_Phan_GiaiQuyet.Contains(i.NextState)
                || Constant.TP_Phan_GiaiQuyet.Contains(i.NextState)
                || Constant.LD_CapDuoi_Phan_GiaiQuyet.Contains(i.CurrentState)
                || Constant.LD_CapDuoi_Phan_GiaiQuyet.Contains(i.NextState)
                || Constant.LD_Phan_GiaiQuyet.Contains(i.NextState))).ToList();

                if (isHas_lqpgq.Count > 0)
                {
                    //item = new BuocInfo() { Value = (int)EnumBuoc.LDPhanGiaiQuyet + "-" + "0", TenBuoc = "Giao xác minh" };
                    //buocInfo.Add(item);
                }

                List<XuLyDonLog> isHas_gq = lstState
               .Where(i => Constant.TruongDoan_GiaiQuyet.Contains(i.CurrentState)
                || Constant.TruongDoan_GiaiQuyet.Contains(i.NextState)).ToList();

                if (isHas_gq.Count > 0)
                {
                    IList<BuocXacMinhInfo> buocXacMinhs = new BuocXacMinh().GetByLoaiKhieuToID(xuLyDonID);

                    for (int i = 0; i < buocXacMinhs.Count; i++)
                    {
                        item = new BuocInfo() { Value = (int)EnumBuoc.GiaiQuyetDon + "-" + buocXacMinhs[i].BuocXacMinhID.ToString(), TenBuoc = "Giải quyết đơn - " + buocXacMinhs[i].TenBuoc, LoaiFile = EnumLoaiFile.FileGiaiQuyet.GetHashCode() };
                        buocInfo.Add(item);
                    }
                }

                if (isHas_lqpgq.Count > 0)
                {
                    item = new BuocInfo() { Value = (int)EnumBuoc.YeuCauDoiThoai + "-" + "0", TenBuoc = "Yêu cầu đối thoại", LoaiFile = EnumLoaiFile.FileGiaiQuyet.GetHashCode() };
                    buocInfo.Add(item);
                }

                int bcxm = new TheoDoiXuLyDAL().CheckBaoCaoXacMinh(xuLyDonID);
                if (bcxm > 0)
                {
                    item = new BuocInfo() { Value = (int)EnumBuoc.BaoCaoXacMinh + "-" + "0", TenBuoc = "Báo cáo xác minh", LoaiFile = EnumLoaiFile.FileBCXM.GetHashCode() };
                    buocInfo.Add(item);
                }

                List<XuLyDonLog> isHas_dgq = lstState
                .Where(i => Constant.LD_Duyet_GiaiQuyet.Contains(i.CurrentState)
                   || Constant.LD_Duyet_GiaiQuyet.Contains(i.NextState)
                   || Constant.TP_DuyetGQ.Contains(i.NextState)
                   || Constant.TP_DuyetGQ.Contains(i.CurrentState)
                   || Constant.TruongDoan_GiaiQuyet.Contains(i.NextState)
                   || Constant.TruongDoan_GiaiQuyet.Contains(i.CurrentState)
                   || Constant.LD_CQCapDuoiDuyetGQ.Contains(i.CurrentState)
                   || Constant.LD_CQCapDuoiDuyetGQ.Contains(i.CurrentState)).ToList();

                if (isHas_dgq.Count > 0)
                {
                    item = new BuocInfo() { Value = (int)EnumBuoc.DuyetGiaiQuyet + "-" + "0", TenBuoc = "Phê duyệt kết quả xác minh", LoaiFile = EnumLoaiFile.FileDTCDGQ.GetHashCode() };
                    buocInfo.Add(item);
                }

                ChuyenXuLyInfo kq = new ChuyenXuLy().GetKetQuaBanHanh(xuLyDonID);

                if (kq.KetQuaBanHanhID > 0)
                {
                    item = new BuocInfo() { Value = (int)EnumBuoc.BanHanh + "-" + "0", TenBuoc = "Ban hành", LoaiFile = EnumLoaiFile.FileBanHanhQD.GetHashCode() };
                    buocInfo.Add(item);
                }

                ThiHanhInfo th = new ThiHanhDAL().GetThiHanhBy_XLDID(xuLyDonID);
                if (th != null)
                {
                    item = new BuocInfo() { Value = (int)EnumBuoc.TheoDoi + "-" + "0", TenBuoc = "Theo dõi", LoaiFile = EnumLoaiFile.FileTheoDoi.GetHashCode() };
                    buocInfo.Add(item);
                }
            }

            return buocInfo;
        }

        public BaseResultModel ExportExcel(BasePagingParamsForFilter p, string ContentRootPath, IdentityHelper IdentityHelper)
        {
            var Result = new BaseResultModel();
            try
            {
                string pathFile = @"Templates\FileTam\ChiTietDonThu_" + IdentityHelper.CanBoID + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";
                int TotalRow = 0;
                p.PageSize = 9999999;
                IList<DonThuInfo> data = GetBySearch(ref TotalRow, p, IdentityHelper);

                string path = ContentRootPath + @"\Templates\BaoCao\QuanLyHoSoDonThu.xlsx";
                FileInfo fileInfo = new FileInfo(path);
                FileInfo file = new FileInfo(ContentRootPath + "\\" + pathFile);

                ExcelPackage package = new ExcelPackage(fileInfo);
                if (package.Workbook.Worksheets != null)
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                    // get number of rows in the sheet
                    int rows = worksheet.Dimension.Rows;
                    int cols = worksheet.Dimension.Columns;

                    //string TuNgayDenNgay = "SO_LIEU_TINH_TU_NGAY_DEN_NGAY";

                    //// loop through the worksheet rows
                    //for (int i = 1; i <= rows; i++)
                    //{
                    //    for (int j = 1; j <= cols; j++)
                    //    {
                    //        if (worksheet.Cells[i, j].Value != null && TuNgayDenNgay.Contains(worksheet.Cells[i, j].Value.ToString()))
                    //        {
                    //            worksheet.Cells[i, j].Value = "Số liệu tính từ ngày " + tuNgay.ToString("dd/MM/yyyy") + " đến ngày " + denNgay.ToString("dd/MM/yyyy");
                    //        }
                    //    }
                    //}
                    if (data.Count > 0)
                    {
                        worksheet.InsertRow(4, data.Count - 1, 3);
                        for (int i = 0; i < data.Count; i++)
                        {
                            worksheet.Cells[i + 3, 1].Value = (i + 1).ToString();
                            worksheet.Cells[i + 3, 2].Value = data[i].SoDonThu;
                            worksheet.Cells[i + 3, 3].Value = data[i].TenNguonDonDen;
                            worksheet.Cells[i + 3, 4].Value = data[i].HoTen;
                            worksheet.Cells[i + 3, 5].Value = data[i].NoiDungDon;
                            worksheet.Cells[i + 3, 6].Value = data[i].TenLoaiKhieuTo;
                            worksheet.Cells[i + 3, 7].Value = data[i].NgayNhapDonStr;
                            worksheet.Cells[i + 3, 8].Value = data[i].TenCoQuan;
                            worksheet.Cells[i + 3, 9].Value = data[i].TenHuongGiaiQuyet;
                            //if (data[i].NgayQuaHanGQ != null && data[i].NgayQuaHanGQ != DateTime.MinValue)
                            //{
                            //    worksheet.Cells[i + 3, 10].Value = data[i].NgayQuaHanGQ.ToString("dd/MM/yyyy");
                            //}
                            
                        }

                    }

                    // save changes
                    package.SaveAs(file);
                }
                //List<TableData> data = new List<TableData>();
                //data = new BaoCaoDAL().ExportExcel(p.ListCapID, IdentityHelper, ContentRootPath, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                //string path = @"Templates\FileTam\THKQGiaiQuyetDonKienNghiPhanAnh_" + IdentityHelper.CanBoID + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";
                //string urlExcel = new BaoCaoDAL().THKQGiaiQuyetDonKienNghiPhanAnh_Excel(ContentRootPath, path, data, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now);
                Result.Status = 1;
                Result.Data = pathFile;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public class BuocInfo
        {
            public string Value { get; set; }
            public string TenBuoc { get; set; }
            public int LoaiFile { get; set; }
        }
    }
}
