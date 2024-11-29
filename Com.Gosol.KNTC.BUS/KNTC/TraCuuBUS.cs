using Com.Gosol.KNTC.DAL.KNTC;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using Workflow;
using Com.Gosol.KNTC.Ultilities;
using DocumentFormat.OpenXml.EMMA;
using Com.Gosol.KNTC.DAL.HeThong;

namespace Com.Gosol.KNTC.BUS.KNTC
{
    public class TraCuuBUS
    {
        public List<DonThuPortalInfo> TraCuu(TraCuuParams p, ref int TotalRow)
        {
            List<DonThuPortalInfo> donThuList = new List<DonThuPortalInfo>();
            try
            {
                List<XuLyDonInfo> xldList = new List<XuLyDonInfo>();
                xldList = new XuLyDonDAL().TraCuuQuyetDinhGiaiQuyet(p, ref TotalRow);
                foreach (XuLyDonInfo xldInfo in xldList)
                {
                    if (xldInfo.DonThuID > 0)
                    {
                        DonThuPortalInfo donThuInfo = new XuLyDonDAL().GetDonThuByIDForPortal(xldInfo.DonThuID, xldInfo.XuLyDonID);
                        int xulydonid = donThuInfo.XuLyDonID;
                        string trangThaiDon = "";
                        string StateName = string.Empty;
                        StateName = WorkflowInstance.Instance.GetCurrentStateOfDocument(xulydonid);
                        var prevState = WorkflowInstance.Instance.GetPrevStateOfDocument(xulydonid);
                        if (StateName == Constant.CV_XuLy || StateName == Constant.TP_PhanXuLy || StateName == Constant.TP_DuyetXuLy || StateName == Constant.LD_DuyetXuLy || StateName == Constant.CV_TiepNhan || StateName == Constant.LD_CapDuoi_Phan_GiaiQuyet)
                        {
                            trangThaiDon = "Đơn thư đang xử lý";
                        }
                        else if (StateName == Constant.LD_Phan_GiaiQuyet || StateName == Constant.Ket_Thuc || StateName == Constant.TruongDoan_GiaiQuyet || StateName == Constant.LD_Duyet_GiaiQuyet || StateName == Constant.CHUYENDON_RAVBDONDOC)
                        {
                            trangThaiDon = "Đã có kết quả xử lý";
                            var xldonInfo = new XuLyDonDAL().GetByID(xulydonid, string.Empty);
                            if (xldonInfo != null)
                            {
                                var hgqInfo = new HuongGiaiQuyet().GetByID(xldonInfo.HuongGiaiQuyetID);
                                if (hgqInfo != null)
                                {
                                    trangThaiDon += ": " + hgqInfo.TenHuongGiaiQuyet;
                                }
                            }
                        }
                        donThuInfo.TrangThaiDonThu = trangThaiDon;
                        donThuInfo.lsDoiTuongKN = new DoiTuongKN().GetCustomDataByNhomKNID(donThuInfo.NhomKNID).ToList();
                        donThuInfo.NhomKNInfo = new NhomKN().GetByID(donThuInfo.NhomKNID);

                        var ThongTinQuyetDinhGQ = new KetQuaDAL().QuyetDinh_GetBy_XuLyDonID(donThuInfo.XuLyDonID);
                        donThuInfo.SoQuyetDinh = ThongTinQuyetDinhGQ.SoQuyetDinh;
                        donThuInfo.NgayQuyetDinh = ThongTinQuyetDinhGQ.NgayQuyetDinh;
                        donThuInfo.TenCoQuanBanHanh = ThongTinQuyetDinhGQ.TenCoQuanBanHanh;
                        donThuInfo.lsFileQuyetDinhGD = ThongTinQuyetDinhGQ.ListFileQuyetDinh;
                        donThuInfo.DanhSachHoSoTaiLieu = ThongTinQuyetDinhGQ.DanhSachHoSoTaiLieu;

                        donThuList.Add(donThuInfo);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return donThuList;
        }
        public List<DonThuPortalInfo> DanhSachHoSoDuocTraCuu(TraCuuParams p, ref int TotalRow)
        {
            List<DonThuPortalInfo> donThuList = new List<DonThuPortalInfo>();
            try
            {
                List<XuLyDonInfo> xldList = new List<XuLyDonInfo>();
                xldList = new XuLyDonDAL().DanhSachHoSoDuocTraCuu(p, ref TotalRow);
                foreach (XuLyDonInfo xldInfo in xldList)
                {
                    if (xldInfo.DonThuID > 0)
                    {
                        DonThuPortalInfo donThuInfo = new XuLyDonDAL().GetDonThuByIDForPortal(xldInfo.DonThuID, xldInfo.XuLyDonID);
                        int xulydonid = donThuInfo.XuLyDonID;
                        string trangThaiDon = "";
                        string StateName = string.Empty;
                        StateName = WorkflowInstance.Instance.GetCurrentStateOfDocument(xulydonid);
                        var prevState = WorkflowInstance.Instance.GetPrevStateOfDocument(xulydonid);
                        if (StateName == Constant.CV_XuLy || StateName == Constant.TP_PhanXuLy || StateName == Constant.TP_DuyetXuLy || StateName == Constant.LD_DuyetXuLy || StateName == Constant.CV_TiepNhan || StateName == Constant.LD_CapDuoi_Phan_GiaiQuyet)
                        {
                            trangThaiDon = "Đơn thư đang xử lý";
                        }
                        else if (StateName == Constant.LD_Phan_GiaiQuyet || StateName == Constant.Ket_Thuc || StateName == Constant.TruongDoan_GiaiQuyet || StateName == Constant.LD_Duyet_GiaiQuyet || StateName == Constant.CHUYENDON_RAVBDONDOC)
                        {
                            trangThaiDon = "Đã có kết quả xử lý";
                            var xldonInfo = new XuLyDonDAL().GetByID(xulydonid, string.Empty);
                            if (xldonInfo != null)
                            {
                                var hgqInfo = new HuongGiaiQuyet().GetByID(xldonInfo.HuongGiaiQuyetID);
                                if (hgqInfo != null)
                                {
                                    trangThaiDon += ": " + hgqInfo.TenHuongGiaiQuyet;
                                }
                            }
                        }
                        donThuInfo.TrangThaiDonThu = trangThaiDon;
                        donThuInfo.lsDoiTuongKN = new DoiTuongKN().GetCustomDataByNhomKNID(donThuInfo.NhomKNID).ToList();
                        donThuInfo.NhomKNInfo = new NhomKN().GetByID(donThuInfo.NhomKNID);

                        var ThongTinQuyetDinhGQ = new KetQuaDAL().QuyetDinh_GetBy_XuLyDonID(donThuInfo.XuLyDonID);
                        donThuInfo.SoQuyetDinh = ThongTinQuyetDinhGQ.SoQuyetDinh;
                        donThuInfo.NgayQuyetDinh = ThongTinQuyetDinhGQ.NgayQuyetDinh;
                        donThuInfo.TenCoQuanBanHanh = ThongTinQuyetDinhGQ.TenCoQuanBanHanh;
                        donThuInfo.lsFileQuyetDinhGD = ThongTinQuyetDinhGQ.ListFileQuyetDinh;
                        donThuInfo.DanhSachHoSoTaiLieu = ThongTinQuyetDinhGQ.DanhSachHoSoTaiLieu;

                        donThuList.Add(donThuInfo);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return donThuList;
        }
        public List<LichSuTraCuuModel> LichSuTraCuu(TraCuuParams p, ref int TotalRow)
        {   
            List<LichSuTraCuuModel> ListInfo = new XuLyDonDAL().LichSuTraCuu(p, ref TotalRow);
            return ListInfo;
        }
        public List<DonThuPortalInfo> Get_TrangThaiByCMND(TraCuuParams p)
        {
            List<DonThuPortalInfo> donThuList = new List<DonThuPortalInfo>();
            try
            {

                int _currentPage = p.PageNumber;
                int Start = (_currentPage - 1) * p.PageSize;
                int End = _currentPage * p.PageSize;
                if(p.CCCD != null && p.CCCD.Length > 0)
                {
                    List<XuLyDonInfo> xldList = new List<XuLyDonInfo>();
                    xldList = new XuLyDonDAL().GetXuLyDonByCMND(p.CCCD, p.CoQuanID ?? 0, Start, End);
                    int count = new XuLyDonDAL().countDonThuByCMND(p.CCCD, p.CoQuanID ?? 0);
                    foreach (XuLyDonInfo xldInfo in xldList)
                    {
                        if (xldInfo.DonThuID > 0)
                        {
                            DonThuPortalInfo donThuInfo = new XuLyDonDAL().GetDonThuByIDForPortal(xldInfo.DonThuID, xldInfo.XuLyDonID);
                            int xulydonid = donThuInfo.XuLyDonID;
                            //string trangThaiDon = "";
                            //string StateName = string.Empty;
                            //StateName = WorkflowInstance.Instance.GetCurrentStateOfDocument(xulydonid);
                            //var prevState = WorkflowInstance.Instance.GetPrevStateOfDocument(xulydonid);
                            //if (StateName == Constant.CV_XuLy || StateName == Constant.TP_PhanXuLy || StateName == Constant.TP_DuyetXuLy || StateName == Constant.LD_DuyetXuLy || StateName == Constant.CV_TiepNhan || StateName == Constant.LD_CapDuoi_Phan_GiaiQuyet)
                            //{
                            //    trangThaiDon = "Đơn thư đang xử lý";
                            //}
                            //else if (StateName == Constant.LD_Phan_GiaiQuyet || StateName == Constant.Ket_Thuc || StateName == Constant.TruongDoan_GiaiQuyet || StateName == Constant.LD_Duyet_GiaiQuyet || StateName == Constant.CHUYENDON_RAVBDONDOC)
                            //{
                            //    trangThaiDon = "Đã có kết quả xử lý";
                            //    var xldonInfo = new XuLyDonDAL().GetByID(xulydonid, string.Empty);
                            //    if (xldonInfo != null)
                            //    {
                            //        var hgqInfo = new HuongGiaiQuyet().GetByID(xldonInfo.HuongGiaiQuyetID);
                            //        if (hgqInfo != null)
                            //        {
                            //            trangThaiDon += ": " + hgqInfo.TenHuongGiaiQuyet;
                            //        }
                            //    }
                            //}
                            //donThuInfo.TrangThaiDonThu = trangThaiDon;

                            donThuInfo.lsDoiTuongKN = new DoiTuongKN().GetCustomDataByNhomKNID(donThuInfo.NhomKNID).ToList();
                            donThuInfo.NhomKNInfo = new NhomKN().GetByID(donThuInfo.NhomKNID);
                            donThuInfo.TongDonThu = count;

                            donThuList.Add(donThuInfo);
                        }
                    }
                }
                else
                {
                    XuLyDonInfo xldInfo = new XuLyDonInfo();
                    xldInfo = new XuLyDonDAL().GetXuLyDonBySoDonThu(p.SoDonThu, p.CoQuanID ?? 0);
                    if (xldInfo != null)
                    {
                        var donThuInfo = new XuLyDonDAL().GetDonThuByIDForPortal(xldInfo.DonThuID, xldInfo.XuLyDonID);

                        int xulydonid = donThuInfo.XuLyDonID;
                        //string trangThaiDon = "";
                        //string StateName = string.Empty;
                        //StateName = WorkflowInstance.Instance.GetCurrentStateOfDocument(xulydonid);
                        //var prevState = WorkflowInstance.Instance.GetPrevStateOfDocument(xulydonid);
                        //if (StateName == Constant.CV_XuLy || StateName == Constant.TP_PhanXuLy || StateName == Constant.TP_DuyetXuLy || StateName == Constant.LD_DuyetXuLy || StateName == Constant.CV_TiepNhan || StateName == Constant.LD_CapDuoi_Phan_GiaiQuyet)
                        //{
                        //    trangThaiDon = "Đơn thư đang xử lý";
                        //}
                        //else if (StateName == Constant.LD_Phan_GiaiQuyet || StateName == Constant.Ket_Thuc || StateName == Constant.TruongDoan_GiaiQuyet || StateName == Constant.LD_Duyet_GiaiQuyet || StateName == Constant.CHUYENDON_RAVBDONDOC)
                        //{
                        //    trangThaiDon = "Đã có kết quả xử lý";
                        //    var xldonInfo = new XuLyDonDAL().GetByID(xulydonid, string.Empty);
                        //    if (xldonInfo != null)
                        //    {
                        //        var hgqInfo = new HuongGiaiQuyet().GetByID(xldonInfo.HuongGiaiQuyetID);
                        //        if (hgqInfo != null)
                        //        {
                        //            trangThaiDon += ": " + hgqInfo.TenHuongGiaiQuyet;
                        //        }
                        //    }
                        //}
                        //donThuInfo.TrangThaiDonThu = trangThaiDon;

                        donThuInfo.lsDoiTuongKN = new DoiTuongKN().GetCustomDataByNhomKNID(donThuInfo.NhomKNID).ToList();
                        donThuInfo.NhomKNInfo = new NhomKN().GetByID(donThuInfo.NhomKNID);

                        donThuList.Add(donThuInfo);
                    }
                }
               
            }
            catch (Exception ex)
            {

            }

            foreach (var item in donThuList)
            {
                var ThongTinQuyetDinhGQ = new KetQuaDAL().QuyetDinh_GetBy_XuLyDonID(item.XuLyDonID);
                item.SoQuyetDinh = ThongTinQuyetDinhGQ.SoQuyetDinh;
                item.NgayQuyetDinh = ThongTinQuyetDinhGQ.NgayQuyetDinh;
                item.TenCoQuanBanHanh = ThongTinQuyetDinhGQ.TenCoQuanBanHanh;
            }
           
            return donThuList;
        }
        public List<DonThuPortalInfo> GetDonThuByCoQuanTiepNhan(TraCuuParams p)
        {
            List<DonThuPortalInfo> donThuList = new List<DonThuPortalInfo>();
            string sodt = "%%";
            int type = 0;
            if (p.SoDonThu != null && p.SoDonThu != "0")
            {
                sodt = p.SoDonThu;
                type = 1;
            }
            int _currentPage = p.PageNumber;
            int Start = (_currentPage - 1) * p.PageSize;
            int End = _currentPage * p.PageSize;
            donThuList = new XuLyDonDAL().SearchDonThuByCoQuanTiepNhan(p.CoQuanID ?? 0, sodt, p.TuNgay ?? DateTime.MinValue, p.DenNgay ?? DateTime.MinValue, Start, End, type);
            foreach (DonThuPortalInfo info in donThuList)
            {
                info.lsDoiTuongKN = new DoiTuongKN().GetCustomDataByNhomKNID(info.NhomKNID).ToList();
            }

            return donThuList;
        }
        public IList<TiepDanKhongDonInfo> GetBySearch(ref int TotalRow, int canBoID, int coQuanID, int PageSize, int currentPage, int loaiKTID, string keyword, DateTime tuNgay, DateTime denNgay, int thoiGianVuViec, int loaiCanBoTiep, int loaiRutDon, int HuongXuLyID, int LoaiTiepDanID)
        {
            int start = (currentPage - 1) * PageSize;
            int end = currentPage * PageSize;
            IList<TiepDanKhongDonInfo> ListInfo = new List<TiepDanKhongDonInfo>();
            ListInfo = new TiepDanKhongDon().GetBySearch(ref TotalRow, keyword ?? "", loaiKTID, tuNgay, denNgay, coQuanID, currentPage, start, end, loaiCanBoTiep, loaiRutDon, canBoID, HuongXuLyID, LoaiTiepDanID);

            return ListInfo;

        }
        public BaseResultModel UpdateTrangThaiKhoa(int? XuLyDonID, bool? TrangThai)
        {
            var Result = new BaseResultModel();
            try
            {
                int val = new XuLyDonDAL().UpdateTrangThaiKhoa(XuLyDonID, TrangThai);
                Result.Status = 1;
                Result.Message = "Cập nhật trạng thái thành công";
            }
            catch (Exception)
            {
                Result.Status = 0;
                Result.Message = "Cập nhật trạng thái thất bại";
            }
           
            return Result;
        }
    }
}
