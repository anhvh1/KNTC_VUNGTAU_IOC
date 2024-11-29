using Com.Gosol.KNTC.DAL.DanhMuc;
using Com.Gosol.KNTC.DAL.HeThong;
using Com.Gosol.KNTC.DAL.KNTC;
using Com.Gosol.KNTC.Model.HeThong;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.BaoCao;
using Com.Gosol.KNTC.Models.DanhMuc;
using Com.Gosol.KNTC.Models.HeThong;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Models.TiepDan;
using Com.Gosol.KNTC.Ultilities;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.AspNetCore.Http;
using Microsoft.Office.Interop.Word;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using Spire.Doc.Fields;
using Spire.Pdf.Exporting.XPS.Schema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Workflow;
using Huyen = Com.Gosol.KNTC.DAL.KNTC.Huyen;
using Tinh = Com.Gosol.KNTC.DAL.KNTC.Tinh;
using Utils = Com.Gosol.KNTC.Ultilities.Utils;
using Xa = Com.Gosol.KNTC.DAL.KNTC.Xa;
using DocumentFormat.OpenXml.Office2016.Excel;
using Grpc.Core;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Numeric;
using RestSharp;
using Microsoft.Extensions.Configuration;
using DocumentFormat.OpenXml.VariantTypes;
using DocumentFormat.OpenXml.EMMA;

namespace Com.Gosol.KNTC.BUS.KNTC
{
    public class TiepDanBUS
    {
        private static int KETTHUC = 10; // Xử lý đơn đã ban hành kết quả
        public IList<TiepDanKhongDonInfo> GetBySearch(ref int TotalRow, int canBoID, int coQuanID, int PageSize, int currentPage, int loaiKTID, string keyword, DateTime tuNgay, DateTime denNgay, int thoiGianVuViec, int loaiCanBoTiep, int loaiRutDon, int HuongXuLyID, int LoaiTiepDanID)
        {
            int start = (currentPage - 1) * PageSize;
            int end = currentPage * PageSize;
            IList<TiepDanKhongDonInfo> ListInfo = new List<TiepDanKhongDonInfo>();
            //IList<TiepDanKhongDonInfo> ListInSoInfo = new List<TiepDanKhongDonInfo>();
            try
            {
                ListInfo = new TiepDanKhongDon().GetBySearch(ref TotalRow, keyword ?? "", loaiKTID, tuNgay, denNgay, coQuanID, currentPage, start, end, loaiCanBoTiep, loaiRutDon, canBoID, HuongXuLyID, LoaiTiepDanID);
                //ListInSoInfo = new TiepDanKhongDon().GetByInSoTiepDan(keyword, loaiKTID, tuNgay, denNgay, IdentityHelper.GetCoQuanID(), loaiCanBoTiep, loaiRutDon, canBoID);
            }
            catch
            {
                throw;
            }

            if (ListInfo.Count > 0)
            {
                if (thoiGianVuViec == 2)
                {
                    IList<TiepDanKhongDonInfo> ListNew = new List<TiepDanKhongDonInfo>();
                    int i = ListInfo.Count - 1;
                    while (i >= 0)
                    {
                        ListNew.Add(ListInfo[i]);
                        i--;
                    }
                    return ListNew;
                }
                else
                {
                    return ListInfo;
                }
            }
            else return ListInfo;

        }
        public IList<TiepDanDinhKyModel> GetBySearch_TiepDanDinhKy(ref int TotalRow, TiepDanParamsForFilter p)
        {
            IList<TiepDanDinhKyModel> ListInfo = new List<TiepDanDinhKyModel>();
            try
            {
                ListInfo = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().GetBySearch_New(ref TotalRow, p);
                if (ListInfo.Count > 0)
                {
                    foreach (var TiepDanInfo in ListInfo)
                    {
                        if (TiepDanInfo.Children != null && TiepDanInfo.Children.Count > 0)
                        {
                            foreach (var item in TiepDanInfo.Children)
                            {
                                if (item.NhomKNID > 0)
                                {
                                    item.NhomKN = new NhomKN().GetByID(item.NhomKNID.Value);
                                    if (item.NhomKN != null)
                                    {
                                        item.NhomKN.DanhSachDoiTuongKN = new DoiTuongKNDAL().GetCustomDataByNhomKNID(item.NhomKNID.Value).ToList();
                                    }
                                }
                            }
                        }
                    }
                }

            }
            catch
            {
            }

            return ListInfo;

        }
        public TiepDanDinhKyModel GetByID_TiepDanDinhKy(int TiepDanID)
        {
            var TiepDanInfo = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().GetByID(TiepDanID);
            if (TiepDanInfo == null) return TiepDanInfo;
            if (TiepDanInfo.NhomKNID > 0)
            {
                TiepDanInfo.NhomKN = new NhomKN().GetByID(TiepDanInfo.NhomKNID.Value);
                if (TiepDanInfo.NhomKN != null)
                {
                    TiepDanInfo.NhomKN.DanhSachDoiTuongKN = new DoiTuongKNDAL().GetCustomDataByNhomKNID(TiepDanInfo.NhomKNID.Value).ToList();
                }
            }

            //List<FileHoSoInfo> fileDinhKem = new List<FileHoSoInfo>();
            //CanBoInfo canBoInfo = new CanBo().GetCanBoByID(TiepDanInfo.CanBoTiepID ?? 0);
            //if (canBoInfo.XemTaiLieuMat)
            //{
            //    fileDinhKem = new FileHoSoDAL().GetByXuLyDonID_TrungDon(TiepDanInfo.XuLyDonID ?? 0).ToList();

            //}
            //else
            //{
            //    fileDinhKem = new FileHoSoDAL().GetByXuLyDonID_TrungDon(TiepDanInfo.XuLyDonID ?? 0).Where(x => x.IsBaoMat != true || x.CanBoID == canBoInfo.CanBoID).ToList();
            //}
            //if (fileDinhKem.Count > 0)
            //{
            //    TiepDanInfo.DanhSachHoSoTaiLieu = new List<DanhSachHoSoTaiLieu>();
            //    TiepDanInfo.DanhSachHoSoTaiLieu = fileDinhKem.GroupBy(p => p.GroupUID)
            //       .Select(g => new DanhSachHoSoTaiLieu
            //       {
            //           GroupUID = g.Key,
            //           TenFile = g.FirstOrDefault().TenFile,
            //           NoiDung = g.FirstOrDefault().TomTat,
            //           TenNguoiCapNhat = g.FirstOrDefault().TenCanBo,
            //           NguoiCapNhatID = g.FirstOrDefault().CanBoID,
            //           NgayCapNhat = g.FirstOrDefault().NgayUp,
            //           FileDinhKem = fileDinhKem.Where(x => x.GroupUID == g.Key && x.FileHoSoID > 0).GroupBy(x => x.FileHoSoID)
            //                           .Select(y => new FileDinhKemModel
            //                           {
            //                               FileID = y.FirstOrDefault().FileHoSoID,
            //                               TenFile = y.FirstOrDefault().TenFile,
            //                               NgayCapNhat = y.FirstOrDefault().NgayCapNhat,
            //                               NguoiCapNhat = y.FirstOrDefault().CanBoID,
            //                               //FileType = y.FirstOrDefault().FileType,
            //                               FileUrl = y.FirstOrDefault().FileURL,
            //                           }
            //                           ).ToList(),

            //       }
            //       ).ToList();
            //}
            return TiepDanInfo;
        }
        public TiepDanDinhKyModel GetByID_TiepDanDinhKy_New(int TiepDanID)
        {
            var TiepDanInfo = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().GetByID(TiepDanID);
            if (TiepDanInfo == null) return TiepDanInfo;
            if (TiepDanInfo.NhomKNID > 0)
            {
                TiepDanInfo.NhomKN = new NhomKN().GetByID(TiepDanInfo.NhomKNID.Value);
                if (TiepDanInfo.NhomKN != null)
                {
                    TiepDanInfo.NhomKN.DanhSachDoiTuongKN = new DoiTuongKNDAL().GetCustomDataByNhomKNID(TiepDanInfo.NhomKNID.Value).ToList();
                }
            }
            TiepDanInfo.Children = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().GetThongTinVuViec(TiepDanInfo.TiepDinhKyID).ToList();
            if (TiepDanInfo.Children != null && TiepDanInfo.Children.Count > 0)
            {
                foreach (var item in TiepDanInfo.Children)
                {
                    if (item.NhomKNID > 0)
                    {
                        item.NhomKN = new NhomKN().GetByID(item.NhomKNID.Value);
                        if (item.NhomKN != null)
                        {
                            item.NhomKN.DanhSachDoiTuongKN = new DoiTuongKNDAL().GetCustomDataByNhomKNID(item.NhomKNID.Value).ToList();
                        }
                    }
                }
            }
            return TiepDanInfo;
        }
        public BaseResultModel SaveTiepDanDinhKy(TiepDanDinhKyModel TiepDanInfo)
        {
            var Result = new BaseResultModel();
            if (TiepDanInfo.NgayTiep == null || TiepDanInfo.NgayTiep == DateTime.MinValue)
            {
                Result.Status = 0;
                Result.Message = "Vui lòng nhập Ngày tiếp";
                return Result;
            }

            NhomKNInfo nhomInfo = new NhomKNInfo();
            if (TiepDanInfo.NhomKN != null) nhomInfo = TiepDanInfo.NhomKN;
            //dai dien phap ly
            nhomInfo.DaiDienPhapLy = false;
            nhomInfo.DuocUyQuyen = false;
            if (nhomInfo.SoLuong == null || nhomInfo.SoLuong == 0) nhomInfo.SoLuong = 1;

            if (TiepDanInfo.NhomKN != null && TiepDanInfo.NhomKN.NhomKNID > 0)
            {
                nhomInfo.NhomKNID = new NhomKN().Update(nhomInfo);
            }
            else
            {
                nhomInfo.NhomKNID = new NhomKN().Insert(nhomInfo);
            }

            //Them doi tuong kn/tc
            if (TiepDanInfo.NhomKN != null && TiepDanInfo.NhomKN.DanhSachDoiTuongKN != null && TiepDanInfo.NhomKN.DanhSachDoiTuongKN.Count > 0)
            {
                TiepDanInfo.NhomKNID = nhomInfo.NhomKNID;
                foreach (var item in TiepDanInfo.NhomKN.DanhSachDoiTuongKN)
                {
                    item.NhomKNID = nhomInfo.NhomKNID;
                    if (item.HoTen != null)
                    {
                        item.HoTen = chuyenDoiChuHoa(item.HoTen.Trim());
                        item.CMND = item.CMND != null ? item.CMND.Trim() : "";
                        if (item.NgayCap != DateTime.MinValue)
                        {

                        }
                        else item.NgayCap = null;
                        if (item.DoiTuongKNID < 1)
                            new DoiTuongKN().Insert(item);
                        else
                        {
                            new DoiTuongKN().Update(item);
                        }
                    }
                }
            }

            if (TiepDanInfo.TiepDinhKyID > 0)
            {
                TiepDanInfo.TiepDinhKyID = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().UpdateTiepDanDinhKy(TiepDanInfo);
            }
            else
            {
                Result.Data = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().InsertTiepDanDinhKy(TiepDanInfo);
            }

            //File


            Result.Status = 1;
            Result.Data = TiepDanInfo;
            return Result;
        }
        public BaseResultModel SaveTiepDanDinhKy_New(TiepDanDinhKyModel TiepDanInfo)
        {
            var Result = new BaseResultModel();
            if (TiepDanInfo.NgayTiep == null || TiepDanInfo.NgayTiep == DateTime.MinValue)
            {
                Result.Status = 0;
                Result.Message = "Vui lòng nhập Ngày tiếp";
                return Result;
            }

            if (TiepDanInfo.Children != null && TiepDanInfo.Children.Count > 0)
            {
                foreach (var tiepDan in TiepDanInfo.Children)
                {
                    NhomKNInfo nhomInfo = new NhomKNInfo();
                    if (tiepDan.NhomKN != null) nhomInfo = tiepDan.NhomKN;
                    //dai dien phap ly
                    nhomInfo.DaiDienPhapLy = false;
                    nhomInfo.DuocUyQuyen = false;
                    if (nhomInfo.SoLuong == null || nhomInfo.SoLuong == 0) nhomInfo.SoLuong = 1;

                    if (tiepDan.NhomKN != null && tiepDan.NhomKN.NhomKNID > 0)
                    {
                        nhomInfo.NhomKNID = new NhomKN().Update(nhomInfo);
                    }
                    else
                    {
                        nhomInfo.NhomKNID = new NhomKN().Insert(nhomInfo);
                    }

                    //Them doi tuong kn/tc
                    if (tiepDan.NhomKN != null && tiepDan.NhomKN.DanhSachDoiTuongKN != null && tiepDan.NhomKN.DanhSachDoiTuongKN.Count > 0)
                    {
                        tiepDan.NhomKNID = nhomInfo.NhomKNID;
                        foreach (var item in tiepDan.NhomKN.DanhSachDoiTuongKN)
                        {
                            item.NhomKNID = nhomInfo.NhomKNID;
                            if (item.HoTen != null)
                            {
                                item.HoTen = chuyenDoiChuHoa(item.HoTen.Trim());
                                item.CMND = item.CMND != null ? item.CMND.Trim() : "";
                                if (item.NgayCap != DateTime.MinValue)
                                {

                                }
                                else item.NgayCap = null;
                                if (item.DoiTuongKNID < 1)
                                    new DoiTuongKN().Insert(item);
                                else
                                {
                                    new DoiTuongKN().Update(item);
                                }
                            }
                        }
                    }
                }
            }


            if (TiepDanInfo.TiepDinhKyID > 0)
            {
                TiepDanInfo.TiepDinhKyID = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().UpdateTiepDanDinhKy(TiepDanInfo);
            }
            else
            {
                Result.Data = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().InsertTiepDanDinhKy(TiepDanInfo);
            }

            //File


            Result.Status = 1;
            Result.Data = TiepDanInfo;
            return Result;
        }
        public BaseResultModel DeleteTiepDanDinhKy(int tiepdanID)
        {
            var Result = new BaseResultModel();
            var kq = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().DeleteTiepDanDinhKy(tiepdanID);
            if (kq > 0)
            {
                Result.Status = 1;
            }
            else Result.Status = 0;
            return Result;
        }

        public BaseResultModel DeleteVuViec(int ID)
        {
            var Result = new BaseResultModel();
            var kq = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().DeleteVuViec(ID);
            if (kq > 0)
            {
                Result.Status = 1;
            }
            else Result.Status = 0;
            return Result;
        }

        public IList<HuongGiaiQuyetInfo> GetAllHuongXuLy()
        {
            return new HuongGiaiQuyet().GetAll().ToList();
        }
        public IList<LoaiKhieuToInfo> DanhMucLoaiKhieuTo(int LoaiKhieuToID)
        {
            return new LoaiKhieuTo().GetLoaiKhieuToByParentID(LoaiKhieuToID).ToList();
        }
        public IList<QuocTichInfo> DanhMucQuocTich()
        {
            return new QuocTich().GetAll().ToList();
        }
        public IList<DanTocInfo> DanhMucDanToc()
        {
            return new DanToc().GetAll().ToList();
        }
        public IList<TinhInfo> DanhMucTinh()
        {
            return new Tinh().GetAll().ToList();
        }
        public IList<HuyenInfo> DanhMucHuyen(int TinhID)
        {
            return new Huyen().GetByTinh(TinhID).ToList();
        }
        public IList<XaInfo> DanhMucXa(int HuyenID)
        {
            return new Xa().GetByHuyen(HuyenID).ToList();
        }
        public IList<ChucVuInfo> DanhMucChucVu()
        {
            return new ChucVu().GetAll().ToList();
        }
        public IList<CanBoInfo> GetCanBoXuLy(int CoQuanID)
        {
            return new CanBo().GetByCoQuanID(CoQuanID).Where(x => x.State == 0).ToList();
        }
        public IList<CanBoInfo> GetDanhSachLanhDao()
        {
            return new CanBo().GetDanhSachLanhDao();
        }
        public IList<PhongBanInfo> GetPhongXuLy(int CoQuanID)
        {
            return new PhongBan().GetByCoQuanID(CoQuanID);
        }
        public IList<DanhMucFileInfo> DanhMucTenFile()
        {
            List<DanhMucFileInfo> listDMFile = new DanhMucFile().File_Get_DanhSachDangSuDung();
            //var url = HttpContext.Current.Request.Url.ToString();
            //var menuInfo = new DAL.Menu().GetByUrl(url);

            return listDMFile;
        }
        public IList<CanBoInfo> GetAllCanBo()
        {
            return new CanBo().GetAllCanBo();
        }
        public IList<CoQuanInfo> GetAllCoQuan()
        {
            return new CoQuan().GetAllCoQuan();
        }
        public IList<CapInfo> GetAllCap()
        {
            return new CapDAL().GetAll();
        }

        public int CheckSoDonTrung(string hoTen, string cmnd, string diaChi, string noiDung, int coQuanID)
        {
            int toCao = Constant.ToCao;
            int countNum = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().CheckSoDonTrung(hoTen, cmnd, diaChi, noiDung, toCao, coQuanID);
            return countNum;
        }
        public IList<TiepDanInfo> GetDonTrung(string hoTen, string cmnd, string diaChi, string noiDung, int coQuanID)
        {
            List<TiepDanInfo> lsTrungDon = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().GetDonTrung(hoTen, cmnd, diaChi, noiDung).Where(x => x.LoaiKhieuTo1ID != Constant.ToCao || x.CoQuanID == coQuanID).ToList();
            return lsTrungDon;
        }
        public IList<TiepDanInfo> KiemTraKhieuToLan2(string hoTen, string cmnd, string diaChi, string noiDung, int coQuanID)
        {
            IList<TiepDanInfo> ListInfo = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().GetKhieuToLan2(hoTen, cmnd, diaChi, noiDung, KETTHUC)
                        .Where(x => x.LoaiKhieuTo1ID != Constant.ToCao || x.CoQuanID == coQuanID)
                        .Where(y => y.HuongGiaiQuyetID == (int)HuongGiaiQuyetEnum.DeXuatThuLy)
                        .ToList();
            return ListInfo;
        }

        public IList<TiepDanInfo> CTDonTrung(int DonThuID)
        {
            IList<TiepDanInfo> lsTrungDon = null;
            if (DonThuID > 0)
            {
                try
                {
                    lsTrungDon = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().GetCTTrungDonByDonID(DonThuID);
                }
                catch { }
            }
            return lsTrungDon;
        }

        public IList<TiepDanInfo> CTDonKhieuToLan2(int donThuID)
        {
            string data = "";
            IList<TiepDanInfo> lsTrungDon = null;
            if (donThuID > 0)
            {

                try
                {
                    lsTrungDon = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().GetCTKhieuToLan2ByDonID(donThuID).Where(x => x.StateID == KETTHUC).ToList();

                }
                catch (Exception ex) { }
            }

            return lsTrungDon;
        }

        public int GetSTT(int coquanID)
        {
            return new Com.Gosol.KNTC.DAL.KNTC.TiepDan().GetSTT(coquanID);
        }
        public BaseResultModel Delete(int tiepdanID)
        {
            var Result = new BaseResultModel();
            try
            {
                var kq = new TiepDanKhongDon().Delete_Manage(tiepdanID);
                if (kq > 0)
                {
                    Result.Status = 1;
                }
                else Result.Status = 0;
            }
            catch (Exception)
            {
                Result.Status = 0;
                Result.Message = "Đơn đã được giải quyết không thể xóa";
            }

            return Result;
        }

        public BaseResultModel Save(TiepDanInfo TiepDanInfo, IdentityHelper IdentityHelper)
        {
            var Result = new BaseResultModel();
            string Message = "";
            if (TiepDanInfo.NgayNhapDon == null || TiepDanInfo.NgayNhapDon == DateTime.MinValue)
            {
                Result.Status = 0;
                Result.Message = "Vui lòng nhập Ngày nhập đơn";
                return Result;
            }
            if (TiepDanInfo.LoaiTiepDanID == EnumLoaiTiepDan.TiepDinhKy.GetHashCode() || TiepDanInfo.LoaiTiepDanID == EnumLoaiTiepDan.TiepDotXuat.GetHashCode()) //Tiếp dân định kỳ và tiếp dân đột xuất
            {
                TiepDanInfo.GapLanhDao = true;
            }
            int isNhapThongTinDonThu = TiepDanInfo.TiepDanCoDon ?? 0;
            bool isTrungDon = false;
            int phongbanID = TiepDanInfo.PhongID ?? 0;
            //int hdfTrungDon = Utils.ConvertToInt32(hdf_CheckDisblaTrung.Value, 0);
            int hdfTrungDon = TiepDanInfo.DonThuTrung ?? 0;
            //int donThu_trung_edit = Utils.ConvertToInt32(hdf_DonThuTrung.Value, 0);
            int donThu_trung_edit = TiepDanInfo.DonThuTrung ?? 0;
            //int kNLan2 = Utils.ConvertToInt32(hdfKNLan2.Value, 0);
            //int kNLan2_edit = Utils.ConvertToInt32(hdfDonThuKNLan2.Value, 0);  
            int kNLan2 = TiepDanInfo.DonThuKNLan2 ?? 0;
            int kNLan2_edit = TiepDanInfo.DonThuKNLan2 ?? 0;
            bool isKNLan2 = false;

            if (kNLan2 == 1)
            {
                isKNLan2 = true;
            }

            if (hdfTrungDon == 1 || donThu_trung_edit == 1)
            {
                isTrungDon = true;
            }

            int dtbiknID = 0;
            // tiếp thường xuyên
            if (TiepDanInfo.LanhDaoTiepDanID == null || TiepDanInfo.LanhDaoTiepDanID == 0)
            {
                #region insert dt kn, dtbikn

                DonThuInfo dtInfo = new DonThuInfo();

                //Them nhom kn/tc
                NhomKNInfo nhomInfo = new NhomKNInfo();
                if (TiepDanInfo.NhomKN != null) nhomInfo = TiepDanInfo.NhomKN;
                //nhomInfo.SoLuong = Utils.ConvertToInt32(txt_songuoi.Text, 1);
                //nhomInfo.TenCQ = txt_tencqtc.Text;
                //nhomInfo.LoaiDoiTuongKNID = Utils.ConvertToInt32(hoso_type.SelectedValue, 0);
                //nhomInfo.DiaChiCQ = txt_diachicq.Text.Trim();
                int KQQuaTiepDan = TiepDanInfo.KQQuaTiepDan ?? 0;
                //dai dien phap ly
                nhomInfo.DaiDienPhapLy = false;
                nhomInfo.DuocUyQuyen = false;
                if (nhomInfo.SoLuong == null || nhomInfo.SoLuong == 0) nhomInfo.SoLuong = 1;

                //int daidienSelected = TiepDanInfo.ThongTinNguoiDaiDien ?? 0;
                int daidienSelected = TiepDanInfo.NhomKN.ThongTinNguoiDaiDien ?? 0;
                if (daidienSelected == 1)
                    nhomInfo.DuocUyQuyen = true;
                else
                    nhomInfo.DaiDienPhapLy = true;
                //end - dai dien phap ly

                int nhomId = TiepDanInfo.NhomKNID ?? 0;
                if (TiepDanInfo.NhomKN != null && TiepDanInfo.NhomKN.NhomKNID > 0) nhomId = TiepDanInfo.NhomKN.NhomKNID;
                if (isKNLan2)
                {
                    nhomId = 0;
                }
                if (nhomId < 1)
                {
                    //insert
                    nhomId = new NhomKN().Insert(nhomInfo);
                }
                else
                {
                    nhomInfo.NhomKNID = nhomId;
                    nhomInfo.NhomKNID = new NhomKN().Update(nhomInfo);
                }

                int songuoidaidien = TiepDanInfo.SoNguoiDaiDien ?? 1;

                //Them doi tuong kn/tc
                updateDoiTuongKNTC(TiepDanInfo, songuoidaidien, nhomId);

                //Them Don thu (step1) 
                dtInfo.NhomKNID = nhomId;

                //int isNhapDoiTuongBiKN = Utils.ConvertToInt32(hdfNhapThongTinDoiTuongBiKN.Value, 0);
                //if (TiepDanInfo.DoiTuongBiKN != null)
                //{
                //    if (hdfTrungDon != 1 || donThu_trung_edit != 1)
                //    {
                //        //them doi tuong bi khieu nai
                //        dtbiknID = InsertDoiTuongBiKN(TiepDanInfo);
                //    }
                //    dtInfo.DoiTuongBiKNID = dtbiknID;
                //}



                #endregion

                //int tiepdanID = Utils.ConvertToInt32(hdf_TiepDanID.Value, 0);
                //int donthuID = Utils.ConvertToInt32(hdf_DonThuID.Value, 0);
                //int xulydonID = Utils.ConvertToInt32(hdf_xulydonId.Value, 0);
                //int letanchuyenID = Utils.ConvertToInt32(hdfLeTanChuyenID.Value, 0);
                int tiepdanID = TiepDanInfo.TiepDanKhongDonID ?? 0;
                int donthuID = TiepDanInfo.DonThuID ?? 0;
                int xulydonID = TiepDanInfo.XuLyDonID ?? 0;
                int letanchuyenID = TiepDanInfo.LeTanChuyenID ?? 0;
                int xuLyDonNew = 0;

                // thêm mới
                if (tiepdanID == 0)
                {
                    //Trung don hoac Le tan chuyen or kn lan 2
                    if (donthuID != 0 || xulydonID != 0)
                    {
                        //Le tan chuyen
                        if (letanchuyenID > 0)
                        {
                            //LeTanChuyenInfo letanInfo = new LeTanChuyen().GetByID(letanchuyenID);
                            //new LeTanChuyen().UpdateDaTiep(true, letanchuyenID);
                        }
                        //Trung don or kn lan 2
                        else
                        {
                            #region trung don
                            if (hdfTrungDon == 1 || donThu_trung_edit == 1)
                            {
                                isTrungDon = true;
                            }

                            TiepDanInfo tiepDanInfo = null;

                            if (donthuID > 0)
                            {
                                //hdf_xulydonId.Value = "";
                                //hdfDonThuGocID.Value = donthuID.ToString();

                                if (isKNLan2)
                                {
                                    //hdf_DonThuID.Value = "";
                                    donthuID = InsertDonThu(dtInfo.NhomKNID, dtInfo.DoiTuongBiKNID, TiepDanInfo);
                                }

                                tiepDanInfo = InsertXuLyDon(IdentityHelper, TiepDanInfo, donthuID, isTrungDon, isKNLan2);

                                xuLyDonNew = tiepDanInfo.XuLyDonID ?? 0;

                                CapNhatFileDinhKem(TiepDanInfo, xuLyDonNew, donthuID);
                                //CapNhatFileDinhKem(xuLyDonNew, donthuID);
                                //CapNhatYKienXL(xuLyDonNew);

                            }

                            //if (!IdentityHelper.GetSuDungQuyTrinhPhucTap())
                            //{
                            //    bool isChuyenDon = false;
                            //    if (ddl_huonggiaiquyet.SelectedValue == ((int)HuongGiaiQuyetEnum.ChuyenDon).ToString())
                            //        isChuyenDon = true;
                            //    if (isChuyenDon)
                            //    {
                            //        ChuyenDon(tiepDanInfo, xuLyDonNew);
                            //    }
                            //}

                            tiepdanID = InsertTiepDanKhongDon(TiepDanInfo, xuLyDonNew, donthuID, dtInfo.NhomKNID, KQQuaTiepDan, letanchuyenID, isTrungDon);
                            //hdf_CheckDisblaTrung.Value = " 1";
                            try
                            {
                                GanDonThuVaoWFVaThucThiCommand(IdentityHelper, TiepDanInfo, xuLyDonNew);
                            }
                            catch (Exception) { }

                            if (IdentityHelper.SuDungQuyTrinhPhucTap == true)
                            {
                                //ChuyenXuLyChoPhong(phongbanID, xuLyDonNew);
                            }
                            #endregion
                        }

                    }
                    else
                    {
                        #region vu viec moi
                        try
                        {
                            TiepDanInfo xldInfo = null;


                            if (isNhapThongTinDonThu == 1)
                            {
                                donthuID = InsertDonThu(dtInfo.NhomKNID, dtInfo.DoiTuongBiKNID, TiepDanInfo);
                                if (donthuID > 0)
                                {
                                    if (kNLan2_edit == 1)
                                    {
                                        isKNLan2 = true;
                                    }
                                    xldInfo = InsertXuLyDon(IdentityHelper, TiepDanInfo, donthuID, isTrungDon, isKNLan2);
                                    xulydonID = xldInfo.XuLyDonID ?? 0;


                                    CapNhatFileDinhKem(TiepDanInfo, xulydonID, donthuID);
                                    //CapNhatFileDinhKem(xulydonID, donthuID);
                                    //CapNhatYKienXL(xulydonID);
                                }

                                //if (!IdentityHelper.GetSuDungQuyTrinhPhucTap())
                                //{
                                //    bool isChuyenDon = false;
                                //    if (ddl_huonggiaiquyet.SelectedValue == ((int)HuongGiaiQuyetEnum.ChuyenDon).ToString())
                                //        isChuyenDon = true;
                                //    if (isChuyenDon)
                                //    {
                                //        ChuyenDon(xldInfo, xulydonID);
                                //    }
                                //}
                            }

                            tiepdanID = InsertTiepDanKhongDon(TiepDanInfo, xulydonID, donthuID, dtInfo.NhomKNID, KQQuaTiepDan, letanchuyenID, isTrungDon);
                            if (TiepDanInfo.TiepDanKhongDonID == null || TiepDanInfo.TiepDanKhongDonID == 0) TiepDanInfo.TiepDanKhongDonID = tiepdanID;
                            //hdf_TiepDanID.Value = tiepdanID.ToString();

                            if (isNhapThongTinDonThu == 1)
                            {
                                try
                                {
                                    GanDonThuVaoWFVaThucThiCommand(IdentityHelper, TiepDanInfo, xulydonID);
                                }
                                catch (Exception) { }

                                if (IdentityHelper.SuDungQuyTrinhPhucTap == true)
                                {
                                    //ChuyenXuLyChoPhong(phongbanID, xulydonID);
                                }
                            }

                            //btnLuuVaIn.Visible = true;
                            //ScriptManager.RegisterStartupScript(this, this.GetType(), "showSucess", "<script type='text/javascript'>showSucess();</script>", false);
                        }
                        catch { }
                        #endregion

                    }

                    //hdf_xulydonId.Value = xulydonID.ToString();
                    //hdf_DonThuID.Value = donthuID.ToString();
                    //hdf_TiepDanID.Value = tiepdanID.ToString();

                    //udpStep1.Update();

                }

                // sửa
                else
                {
                    #region Update tiep dan
                    try
                    {
                        if (isNhapThongTinDonThu == 1)
                        {
                            donthuID = InsertDonThu(dtInfo.NhomKNID, dtInfo.DoiTuongBiKNID, TiepDanInfo);
                            if (kNLan2_edit == 1)
                            {
                                isKNLan2 = true;
                            }
                            var xldInfo = InsertXuLyDon(IdentityHelper, TiepDanInfo, donthuID, isTrungDon, isKNLan2);
                            xulydonID = xldInfo.XuLyDonID ?? 0;

                            #region file dinh kem
                            CapNhatFileDinhKem(TiepDanInfo, xulydonID, donthuID);
                            //CapNhatFileDinhKem(xulydonID, donthuID);
                            #endregion

                            //CapNhatYKienXL(xulydonID);

                            //if (!IdentityHelper.SuDungQuyTrinhPhucTap())
                            //{
                            //    bool isChuyenDon = false;


                            //    if (ddl_huonggiaiquyet.SelectedValue == ((int)HuongGiaiQuyetEnum.ChuyenDon).ToString())
                            //        isChuyenDon = true;
                            //    if (isChuyenDon)
                            //    {
                            //        ChuyenDon(xldInfo, xulydonID);
                            //    }
                            //}
                        }
                        UpdateDonThuVaoWFVaThucThiCommand(IdentityHelper, TiepDanInfo, xulydonID);
                        InsertTiepDanKhongDon(TiepDanInfo, xulydonID, donthuID, dtInfo.NhomKNID, KQQuaTiepDan, 0, isTrungDon);
                    }
                    catch
                    {
                        throw;
                    }
                    #endregion
                }

                // thêm đơn thư lấy id để thêm đối tượng bị KNTC
                if (TiepDanInfo.DanhSachDoiTuongBiKN != null && TiepDanInfo.DanhSachDoiTuongBiKN.Count > 0)
                {
                    if (hdfTrungDon != 1 || donThu_trung_edit != 1)
                    {
                        //them doi tuong bi khieu nai lấy mặc định thằng đầu tiên để tránh sai nghiệp vụ
                        InsertDoiTuongBiKN_V2(TiepDanInfo, donthuID);
                    }
                    //dtInfo.DoiTuongBiKNID = dtbiknID;
                }
            }

            // tiếp định kỳ,đột xuất
            else if (TiepDanInfo.LanhDaoTiepDanID > 0)
            {
                #region lanh dao tiep dan
                //int tdkdID = Utils.ConvertToInt32(hdf_TiepDanID.Value, 0);
                //int lantiep = Utils.ConvertToInt32(hdf_LanTiep.Value, 0);
                //int donthuid = Utils.ConvertToInt32(hdf_DonThuID.Value, 0);
                //int xulydonid = Utils.ConvertToInt32(hdf_xulydonId.Value, 0);
                int tiepdankhongdonidold = TiepDanInfo.LanhDaoTiepDanID ?? 0;

                TiepDanInfo tdInfo = new TiepDanInfo();
                tdInfo = TiepDanInfo;
                //tdInfo.XuLyDonID = xulydonid;
                //tdInfo.NgayTiep = Utils.ConvertToDateTime(txt_ngaynhapdon.Text, DateTime.MinValue);
                tdInfo.GapLanhDao = false;
                tdInfo.NoiDungTiep = string.Empty;
                tdInfo.VuViecCu = true;

                tdInfo.CanBoTiepID = IdentityHelper.CanBoID ?? 0;
                //tdInfo.DonThuID = donthuid;
                tdInfo.CoQuanID = IdentityHelper.CoQuanID ?? 0;
                //tdInfo.LanTiep = lantiep;
                //tdInfo.KetQuaTiep = txtKetQuaTiep.Text;
                //tdInfo.SoDon = txt_sodon.Text;
                //int KQQuaTiepDan = int.Parse(rdlKQTiep.SelectedValue);
                //tdInfo.KQQuaTiepDan = KQQuaTiepDan;
                int val = 0;
                try
                {
                    val = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().InsertTiepDan(tdInfo);
                    TiepDanInfo.TiepDanKhongDonID = val;
                }
                catch
                {

                }
                if (val > 0)
                {
                    try
                    {
                        new LanhDaoTiepDan().Update(tiepdankhongdonidold, true);
                        //Response.Redirect("/LanhDaoTiepDan.aspx");
                    }
                    catch
                    {

                    }
                }
                #endregion
            }

            var tiepDan = GetByID(TiepDanInfo.TiepDanKhongDonID ?? 0);
            Result.Status = 1;
            Result.Message = tiepDan != null ? "Lưu thành công số đơn thư:" + tiepDan?.SoDonThu : "Lưu thành công";
            Result.Data = TiepDanInfo;
            return Result;
        }

        public BaseResultModel SaveTiepCongDan_DanKhongDen(TiepCongDan_DanKhongDenInfo TiepDanInfo)
        {
            var Result = new BaseResultModel();
            int temp = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().InsertDanKhongDen(TiepDanInfo);
            if (temp > 0)
            {
                Result.Message = "Lưu thành công";
                Result.Status = 1;
            }
            else
            {
                Result.Message = "Thất bại! Vui lòng thử lại";
                Result.Status = 0;
            }

            Result.Data = TiepDanInfo;
            return Result;
        }

        public BaseResultModel DeleteDanKhongDen(int DanKhongDenID)
        {
            var Result = new BaseResultModel();
            var temp = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().DeleteDanKhongDen(DanKhongDenID);
            if (temp > 0)
            {
                Result.Status = 1;
            }
            else Result.Status = 0;
            return Result;
        }


        public TiepCongDan_DanKhongDenInfo TiepDan_DanKhongDen_GetByID(int DanKhongDenID)
        {
            TiepCongDan_DanKhongDenInfo DanKhongDen = new TiepCongDan_DanKhongDenInfo();
            if (DanKhongDenID > 0)
            {
                DanKhongDen = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().GetTiepDanDanKhongDen(DanKhongDenID);
            }

            return DanKhongDen;
        }


        public TiepDanInfo GetByID(int TiepDanID)
        {
            var TiepDanInfo = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().GetTiepDanByTiepDanID(TiepDanID);
            if (TiepDanInfo == null) return TiepDanInfo;
            if (TiepDanInfo.NhomKNID > 0)
            {
                TiepDanInfo.NhomKN = new NhomKN().GetByID(TiepDanInfo.NhomKNID.Value);
                if (TiepDanInfo.NhomKN != null)
                {
                    TiepDanInfo.NhomKN.DanhSachDoiTuongKN = new DoiTuongKNDAL().GetCustomDataByNhomKNID(TiepDanInfo.NhomKNID.Value).ToList();
                }
            }

            if (TiepDanInfo.DonThuID > 0)
            {
                DonThuInfo donthuInfo = new DonThuDAL().GetByID(TiepDanInfo.DonThuID.Value);
                TiepDanInfo.DonThu = donthuInfo;
                TiepDanInfo.NoiDungDon = donthuInfo.NoiDungDon;
                TiepDanInfo.DiaChiPhatSinh = donthuInfo.DiaChiPhatSinh;
                TiepDanInfo.TinhID = donthuInfo.TinhID == 0 ? null : donthuInfo.TinhID;
                TiepDanInfo.HuyenID = donthuInfo.HuyenID == 0 ? null : donthuInfo.HuyenID;
                TiepDanInfo.XaID = donthuInfo.XaID == 0 ? null : donthuInfo.XaID;
                TiepDanInfo.NgayVietDon = donthuInfo.NgayVietDon;

                var danhSachDoiTuongBiKN = new DoiTuongBiKN().GetDanhSachByDonThuID(donthuInfo.DonThuID);
                if (danhSachDoiTuongBiKN != null && danhSachDoiTuongBiKN.Count > 0)
                {
                    var datas = new List<DoiTuongBiKNInfo>();
                    foreach (var item in danhSachDoiTuongBiKN)
                    {
                        var model = new DoiTuongBiKN().GetByID(item.DoiTuongBiKNID);
                        var caNhanBiKNInfo = new CaNhanBiKN().getCaNhanBiKN(item.DoiTuongBiKNID);
                        if (caNhanBiKNInfo != null)
                        {
                            model.CaNhanBiKNID = caNhanBiKNInfo.CaNhanBiKNID;
                            model.TenNgheNghiep = caNhanBiKNInfo.NgheNghiep;
                            model.ChucVuID = caNhanBiKNInfo.ChucVuID;
                            model.TenChucVu = model.TenChucVu;
                            model.QuocTichDoiTuongBiKNID = caNhanBiKNInfo.QuocTichID;
                            model.NoiCongTacDoiTuongBiKN = caNhanBiKNInfo.NoiCongTac;
                            model.DanTocDoiTuongBiKNID = caNhanBiKNInfo.DanTocID;
                            model.NoiCongTacDoiTuongBiKN = caNhanBiKNInfo.NoiCongTac;
                            datas.Add(model);
                        }
                    }
                    TiepDanInfo.DanhSachDoiTuongBiKN = datas;
                }

                //TiepDanInfo.DoiTuongBiKN = new DoiTuongBiKN().GetByID(donthuInfo.DoiTuongBiKNID);
                //CaNhanBiKNInfo canhanbiknInfo = new CaNhanBiKN().getCaNhanBiKN(donthuInfo.DoiTuongBiKNID);
                //if (canhanbiknInfo != null)
                //{
                //    TiepDanInfo.DoiTuongBiKN.CaNhanBiKNID = canhanbiknInfo.CaNhanBiKNID;
                //    TiepDanInfo.DoiTuongBiKN.TenNgheNghiep = canhanbiknInfo.NgheNghiep;
                //    TiepDanInfo.DoiTuongBiKN.ChucVuID = canhanbiknInfo.ChucVuID;
                //    TiepDanInfo.DoiTuongBiKN.TenChucVu = canhanbiknInfo.TenChucVu;
                //    TiepDanInfo.DoiTuongBiKN.QuocTichDoiTuongBiKNID = canhanbiknInfo.QuocTichID;
                //    TiepDanInfo.DoiTuongBiKN.NoiCongTacDoiTuongBiKN = canhanbiknInfo.NoiCongTac;
                //    TiepDanInfo.DoiTuongBiKN.DanTocDoiTuongBiKNID = canhanbiknInfo.DanTocID;
                //    TiepDanInfo.DoiTuongBiKN.NoiCongTacDoiTuongBiKN = canhanbiknInfo.NoiCongTac;
                //}
            }
            TiepDanInfo.ThanhPhanThamGia = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().GetThanhPhanThamGia(TiepDanID).ToList();
            List<FileHoSoInfo> fileDinhKem = new List<FileHoSoInfo>();
            CanBoInfo canBoInfo = new CanBo().GetCanBoByID(TiepDanInfo.CanBoTiepID ?? 0);
            if (canBoInfo.XemTaiLieuMat)
            {
                fileDinhKem = new FileHoSoDAL().GetByXuLyDonID_TrungDon(TiepDanInfo.XuLyDonID ?? 0).ToList();

            }
            else
            {
                fileDinhKem = new FileHoSoDAL().GetByXuLyDonID_TrungDon(TiepDanInfo.XuLyDonID ?? 0).Where(x => x.IsBaoMat != true || x.CanBoID == canBoInfo.CanBoID).ToList();
            }
            if (fileDinhKem.Count > 0)
            {
                TiepDanInfo.DanhSachHoSoTaiLieu = new List<DanhSachHoSoTaiLieu>();
                TiepDanInfo.DanhSachHoSoTaiLieu = fileDinhKem.GroupBy(p => p.GroupUID)
                   .Select(g => new DanhSachHoSoTaiLieu
                   {
                       GroupUID = g.Key,
                       TenFile = g.FirstOrDefault().TenFile,
                       NoiDung = g.FirstOrDefault().TomTat,
                       TenNguoiCapNhat = g.FirstOrDefault().TenCanBo,
                       NguoiCapNhatID = g.FirstOrDefault().CanBoID,
                       NgayCapNhat = g.FirstOrDefault().NgayUp,
                       FileDinhKem = fileDinhKem.Where(x => x.GroupUID == g.Key && x.FileHoSoID > 0).GroupBy(x => x.FileHoSoID)
                                       .Select(y => new FileDinhKemModel
                                       {
                                           FileID = y.FirstOrDefault().FileHoSoID,
                                           TenFile = y.FirstOrDefault().TenFile,
                                           NgayCapNhat = y.FirstOrDefault().NgayCapNhat,
                                           NguoiCapNhat = y.FirstOrDefault().CanBoID,
                                           //FileType = y.FirstOrDefault().FileType,
                                           FileUrl = y.FirstOrDefault().FileURL,
                                       }
                                       ).ToList(),

                   }
                   ).ToList();
            }
            #region code cũ - nhưng những file này ko lưu trong dbo.FilDinhKem mà lưu trong file hồ sơ
            //TiepDanInfo.FileCQGiaiQuyet = new List<DanhSachHoSoTaiLieu>();
            //var FileCQGiaiQuyet = new FileDinhKemDAL().GetByNgiepVuID(TiepDanInfo.XuLyDonID ?? 0, EnumLoaiFile.FileCQGiaiQuyet.GetHashCode());
            //if (FileCQGiaiQuyet.Count > 0)
            //{
            //    TiepDanInfo.FileCQGiaiQuyet = new List<DanhSachHoSoTaiLieu>();
            //    TiepDanInfo.FileCQGiaiQuyet = FileCQGiaiQuyet.GroupBy(p => p.GroupUID)
            //       .Select(g => new DanhSachHoSoTaiLieu
            //       {
            //           GroupUID = g.Key,
            //           TenFile = g.FirstOrDefault().TenFile,
            //           NoiDung = g.FirstOrDefault().NoiDung,
            //           //TenNguoiCapNhat = g.FirstOrDefault().TenCanBo,
            //           NguoiCapNhatID = g.FirstOrDefault().NguoiCapNhat,
            //           NgayCapNhat = g.FirstOrDefault().NgayCapNhat,
            //           FileDinhKem = FileCQGiaiQuyet.Where(x => x.GroupUID == g.Key && x.FileID > 0).GroupBy(x => x.FileID)
            //                           .Select(y => new FileDinhKemModel
            //                           {
            //                               FileID = y.FirstOrDefault().FileID,
            //                               TenFile = y.FirstOrDefault().TenFile,
            //                               NgayCapNhat = y.FirstOrDefault().NgayCapNhat,
            //                               NguoiCapNhat = y.FirstOrDefault().NguoiCapNhat,
            //                               //FileType = y.FirstOrDefault().FileType,
            //                               FileUrl = y.FirstOrDefault().FileUrl,
            //                           }
            //                           ).ToList(),

            //       }
            //       ).ToList();
            //}
            //TiepDanInfo.FileKQGiaiQuyet = new List<DanhSachHoSoTaiLieu>();
            //var FileKQGiaiQuyet = new FileDinhKemDAL().GetByNgiepVuID(TiepDanInfo.XuLyDonID ?? 0, EnumLoaiFile.FileKQGiaiQuyet.GetHashCode());
            //if (FileKQGiaiQuyet.Count > 0)
            //{
            //    TiepDanInfo.FileKQGiaiQuyet = new List<DanhSachHoSoTaiLieu>();
            //    TiepDanInfo.FileKQGiaiQuyet = FileCQGiaiQuyet.GroupBy(p => p.GroupUID)
            //       .Select(g => new DanhSachHoSoTaiLieu
            //       {
            //           GroupUID = g.Key,
            //           TenFile = g.FirstOrDefault().TenFile,
            //           NoiDung = g.FirstOrDefault().NoiDung,
            //           //TenNguoiCapNhat = g.FirstOrDefault().TenCanBo,
            //           NguoiCapNhatID = g.FirstOrDefault().NguoiCapNhat,
            //           NgayCapNhat = g.FirstOrDefault().NgayCapNhat,
            //           FileDinhKem = FileCQGiaiQuyet.Where(x => x.GroupUID == g.Key && x.FileID > 0).GroupBy(x => x.FileID)
            //                           .Select(y => new FileDinhKemModel
            //                           {
            //                               FileID = y.FirstOrDefault().FileID,
            //                               TenFile = y.FirstOrDefault().TenFile,
            //                               NgayCapNhat = y.FirstOrDefault().NgayCapNhat,
            //                               NguoiCapNhat = y.FirstOrDefault().NguoiCapNhat,
            //                               //FileType = y.FirstOrDefault().FileType,
            //                               FileUrl = y.FirstOrDefault().FileUrl,
            //                           }
            //                           ).ToList(),

            //       }
            //       ).ToList();
            //}
            //TiepDanInfo.FileKQTiep = new List<DanhSachHoSoTaiLieu>();
            //var FileKQTiep = new FileDinhKemDAL().GetByNgiepVuID(TiepDanInfo.XuLyDonID ?? 0, EnumLoaiFile.FileKQTiep.GetHashCode());
            //if (FileKQTiep.Count > 0)
            //{
            //    TiepDanInfo.FileKQTiep = new List<DanhSachHoSoTaiLieu>();
            //    TiepDanInfo.FileKQTiep = FileCQGiaiQuyet.GroupBy(p => p.GroupUID)
            //       .Select(g => new DanhSachHoSoTaiLieu
            //       {
            //           GroupUID = g.Key,
            //           TenFile = g.FirstOrDefault().TenFile,
            //           NoiDung = g.FirstOrDefault().NoiDung,
            //           //TenNguoiCapNhat = g.FirstOrDefault().TenCanBo,
            //           NguoiCapNhatID = g.FirstOrDefault().NguoiCapNhat,
            //           NgayCapNhat = g.FirstOrDefault().NgayCapNhat,
            //           FileDinhKem = FileCQGiaiQuyet.Where(x => x.GroupUID == g.Key && x.FileID > 0).GroupBy(x => x.FileID)
            //                           .Select(y => new FileDinhKemModel
            //                           {
            //                               FileID = y.FirstOrDefault().FileID,
            //                               TenFile = y.FirstOrDefault().TenFile,
            //                               NgayCapNhat = y.FirstOrDefault().NgayCapNhat,
            //                               NguoiCapNhat = y.FirstOrDefault().NguoiCapNhat,
            //                               //FileType = y.FirstOrDefault().FileType,
            //                               FileUrl = y.FirstOrDefault().FileUrl,
            //                           }
            //                           ).ToList(),

            //       }
            //       ).ToList();
            //}
            #endregion

            #region xử lý lấy các file liên quan trong màn hình tiếp dân
            var fileTaiLieu = new FileHoSoDAL().FileTaiLieu_GetByXuLyDonID(TiepDanInfo.XuLyDonID ?? 0);
            if (fileTaiLieu != null && fileTaiLieu.Count > 0)
            {
                #region file cơ quan đã giải quyết - lấy từ màn hình tiếp nhận đơn
                List<FileHoSoInfo> fileCoQuanGiaiQuyet = new List<FileHoSoInfo>();
                fileCoQuanGiaiQuyet = fileTaiLieu.Where(x => x.LoaiFile == EnumLoaiFile.FileCQGiaiQuyet.GetHashCode()).ToList();
                if (fileCoQuanGiaiQuyet != null && fileCoQuanGiaiQuyet.Count > 0)
                {
                    TiepDanInfo.FileCQGiaiQuyet = new List<DanhSachHoSoTaiLieu>();
                    TiepDanInfo.FileCQGiaiQuyet = fileCoQuanGiaiQuyet.GroupBy(p => p.GroupUID)
                       .Select(g => new DanhSachHoSoTaiLieu
                       {
                           GroupUID = g.Key,
                           TenFile = g.FirstOrDefault().TenFile,
                           NoiDung = g.FirstOrDefault().TomTat,
                           TenNguoiCapNhat = g.FirstOrDefault().TenCanBo,
                           NguoiCapNhatID = g.FirstOrDefault().CanBoID,
                           NgayCapNhat = g.FirstOrDefault().NgayUp,
                           FileDinhKem = fileCoQuanGiaiQuyet.Where(x => x.GroupUID == g.Key && x.FileHoSoID > 0).GroupBy(x => x.FileHoSoID)
                                           .Select(y => new FileDinhKemModel
                                           {
                                               FileID = y.FirstOrDefault().FileHoSoID,
                                               //TenFile = y.FirstOrDefault().TenFile,
                                               TenFile = y.FirstOrDefault().FileURL.Split('_').Length > 0 ? string.Join("_", y.FirstOrDefault().FileURL.Split('_').Skip(2)) : y.FirstOrDefault().TenFile,
                                               NgayCapNhat = y.FirstOrDefault().NgayCapNhat,
                                               NguoiCapNhat = y.FirstOrDefault().CanBoID,
                                               //FileType = y.FirstOrDefault().FileType,
                                               FileUrl = y.FirstOrDefault().FileURL,
                                           }
                                           ).ToList(),
                       }
                       ).ToList();
                }

                #endregion


                #region file kết quả tiếp - lấy từ màn hình tiếp nhận đơn
                List<FileHoSoInfo> fileKetQuaTiep = new List<FileHoSoInfo>();
                fileKetQuaTiep = fileTaiLieu.Where(x => x.LoaiFile == EnumLoaiFile.FileKQTiep.GetHashCode()).ToList();
                if (fileKetQuaTiep != null && fileKetQuaTiep.Count > 0)
                {
                    TiepDanInfo.FileKQTiep = new List<DanhSachHoSoTaiLieu>();
                    TiepDanInfo.FileKQTiep = fileKetQuaTiep.GroupBy(p => p.GroupUID)
                       .Select(g => new DanhSachHoSoTaiLieu
                       {
                           GroupUID = g.Key,
                           TenFile = g.FirstOrDefault().TenFile,
                           NoiDung = g.FirstOrDefault().TomTat,
                           TenNguoiCapNhat = g.FirstOrDefault().TenCanBo,
                           NguoiCapNhatID = g.FirstOrDefault().CanBoID,
                           NgayCapNhat = g.FirstOrDefault().NgayUp,
                           FileDinhKem = fileKetQuaTiep.Where(x => x.GroupUID == g.Key && x.FileHoSoID > 0).GroupBy(x => x.FileHoSoID)
                                           .Select(y => new FileDinhKemModel
                                           {
                                               FileID = y.FirstOrDefault().FileHoSoID,
                                               //TenFile = y.FirstOrDefault().TenFile,
                                               TenFile = y.FirstOrDefault().FileURL.Split('_').Length > 0 ? string.Join("_", y.FirstOrDefault().FileURL.Split('_').Skip(2)) : y.FirstOrDefault().TenFile,
                                               NgayCapNhat = y.FirstOrDefault().NgayCapNhat,
                                               NguoiCapNhat = y.FirstOrDefault().CanBoID,
                                               //FileType = y.FirstOrDefault().FileType,
                                               FileUrl = y.FirstOrDefault().FileURL,
                                           }
                                           ).ToList(),
                       }
                       ).ToList();
                }
                #endregion

                #region file kết quả giải quyết  - lấy từ màn hình tiếp nhận đơn
                List<FileHoSoInfo> fileKetQuaGiaiQuyet = new List<FileHoSoInfo>();
                fileKetQuaGiaiQuyet = fileTaiLieu.Where(x => x.LoaiFile == EnumLoaiFile.FileKQGiaiQuyet.GetHashCode()).ToList();
                if (fileKetQuaGiaiQuyet != null && fileKetQuaGiaiQuyet.Count > 0)
                {
                    TiepDanInfo.FileKQGiaiQuyet = new List<DanhSachHoSoTaiLieu>();
                    TiepDanInfo.FileKQGiaiQuyet = fileKetQuaGiaiQuyet.GroupBy(p => p.GroupUID)
                       .Select(g => new DanhSachHoSoTaiLieu
                       {
                           GroupUID = g.Key,
                           TenFile = g.FirstOrDefault().TenFile,
                           NoiDung = g.FirstOrDefault().TomTat,
                           TenNguoiCapNhat = g.FirstOrDefault().TenCanBo,
                           NguoiCapNhatID = g.FirstOrDefault().CanBoID,
                           NgayCapNhat = g.FirstOrDefault().NgayUp,
                           FileDinhKem = fileKetQuaGiaiQuyet.Where(x => x.GroupUID == g.Key && x.FileHoSoID > 0).GroupBy(x => x.FileHoSoID)
                                           .Select(y => new FileDinhKemModel
                                           {
                                               FileID = y.FirstOrDefault().FileHoSoID,
                                               //TenFile = y.FirstOrDefault().TenFile,
                                               TenFile = y.FirstOrDefault().FileURL.Split('_').Length > 0 ? string.Join("_", y.FirstOrDefault().FileURL.Split('_').Skip(2)) : y.FirstOrDefault().TenFile,
                                               NgayCapNhat = y.FirstOrDefault().NgayCapNhat,
                                               NguoiCapNhat = y.FirstOrDefault().CanBoID,
                                               //FileType = y.FirstOrDefault().FileType,
                                               FileUrl = y.FirstOrDefault().FileURL,
                                           }
                                           ).ToList(),
                       }
                       ).ToList();
                }
                #endregion

            }


            #endregion


            return TiepDanInfo;
        }

        protected void updateDoiTuongKNTC(TiepDanInfo data, int nums, int nhomId)
        {
            int kNLan2 = data.KNLan2 ?? 0;
            bool isKNLan2 = false;

            if (kNLan2 == 1)
            {
                isKNLan2 = true;
            }
            if (data.NhomKN != null && data.NhomKN.DanhSachDoiTuongKN != null && data.NhomKN.DanhSachDoiTuongKN.Count > 0)
            {
                foreach (var item in data.NhomKN.DanhSachDoiTuongKN)
                {
                    item.NhomKNID = nhomId;
                    if (item.HoTen != null)
                    {
                        item.HoTen = chuyenDoiChuHoa(item.HoTen.Trim());
                        item.CMND = item.CMND != null ? item.CMND.Trim() : "";
                        if (item.NgayCap != DateTime.MinValue)
                        {

                        }
                        else item.NgayCap = null;
                        if (item.DoiTuongKNID < 1)
                            new DoiTuongKN().Insert(item);
                        else
                        {
                            new DoiTuongKN().Update(item);
                        }
                    }
                }
            }

            //int _lastID = 0;
            //int max_doituong = nums > _currentSoDaiDien ? nums : _currentSoDaiDien;
            //for (int i = 0; i < max_doituong; i++)
            //{
            //    //xoa ban ghi dai dien khong su dung
            //    if (_currentSoDaiDien > nums && i >= nums)
            //    {
            //        new DAL.DoiTuongKN().DeleteByNhomID(nhomId, _lastID);
            //        break;
            //    }

            //    DoiTuongKNInfo dtInfo = new DoiTuongKNInfo();

            //    //id for update
            //    HiddenField hdf_doituongId = rpt_hoso.Items[i].FindControl("hdf_doituongId") as HiddenField;
            //    if (isKNLan2)
            //    {
            //        hdf_doituongId.Value = "";
            //    }

            //    dtInfo.DoiTuongKNID = Utils.ConvertToInt32(hdf_doituongId.Value, 0);

            //    _lastID = dtInfo.DoiTuongKNID;

            //    TextBox txt_hoten = rpt_hoso.Items[i].FindControl("txt_hoten") as TextBox;
            //    DropDownList ddl_sex = rpt_hoso.Items[i].FindControl("ddl_sex") as DropDownList;
            //    TextBox txt_cmnd = rpt_hoso.Items[i].FindControl("txt_cmnd") as TextBox;
            //    TextBox txt_cmnd_ngaycap = rpt_hoso.Items[i].FindControl("txt_ngaycapcmnd") as TextBox;
            //    TextBox txt_cmnd_noicap = rpt_hoso.Items[i].FindControl("txt_noicapcmnd") as TextBox;
            //    TextBox txt_DienThoai = rpt_hoso.Items[i].FindControl("txt_DienThoai") as TextBox;
            //    DropDownList ddl_dantoc1 = rpt_hoso.Items[i].FindControl("ddl_dantoc1") as DropDownList;
            //    DropDownList ddl_huyen1 = rpt_hoso.Items[i].FindControl("ddl_huyen1") as DropDownList;
            //    DropDownList ddl_tinh1 = rpt_hoso.Items[i].FindControl("ddl_tinh1") as DropDownList;
            //    DropDownList ddl_xa1 = rpt_hoso.Items[i].FindControl("ddl_xa1") as DropDownList;
            //    TextBox txtNgheNghiep = rpt_hoso.Items[i].FindControl("txtNgheNghiep") as TextBox;
            //    DropDownList ddl_quoctich1 = rpt_hoso.Items[i].FindControl("ddl_quoctich1") as DropDownList;
            //    TextBox txt_nhapdiachi = rpt_hoso.Items[i].FindControl("txt_nhapdiachi") as TextBox;
            //    TextBox txt_diachi = rpt_hoso.Items[i].FindControl("txt_diachi") as TextBox;

            //    dtInfo.HoTen = chuyenDoiChuHoa(txt_hoten.Text.Trim());
            //    dtInfo.CMND = txt_cmnd.Text.Trim();
            //    var NgayCap = Utils.ConvertToDateTime(txt_cmnd_ngaycap.Text.Trim(), DateTime.MinValue);
            //    if (NgayCap != DateTime.MinValue)
            //    {
            //        dtInfo.NgayCap = NgayCap;
            //    }
            //    else dtInfo.NgayCap = null;
            //    //dtInfo.NgayCap = Utils.ConvertToNullableDateTime(txt_cmnd_ngaycap.Text.Trim(), null);
            //    dtInfo.NoiCap = txt_cmnd_noicap.Text.Trim();
            //    dtInfo.SoDienThoai = txt_DienThoai.Text.Trim();
            //    dtInfo.GioiTinh = Utils.ConvertToInt32(ddl_sex.SelectedValue, 0);
            //    dtInfo.NgheNghiep = txtNgheNghiep.Text.ToString();
            //    dtInfo.QuocTichID = Utils.ConvertToInt32(ddl_quoctich1.SelectedValue, 0);
            //    dtInfo.DanTocID = Utils.ConvertToInt32(ddl_dantoc1.SelectedValue, 0);
            //    dtInfo.TinhID = Utils.ConvertToInt32(ddl_tinh1.SelectedValue, 0);
            //    dtInfo.HuyenID = Utils.ConvertToInt32(ddl_huyen1.SelectedValue, 0);
            //    dtInfo.XaID = Utils.ConvertToInt32(ddl_xa1.SelectedValue, 0);
            //    dtInfo.DiaChiCT = txt_nhapdiachi.Text + ", " + txt_diachi.Text.Trim();

            //    dtInfo.NhomKNID = nhomId;

            //    //insert or update
            //    //int doituongId = Utils.ConvertToInt32(txt_doituonId.Text, 0);
            //    if (dtInfo.DoiTuongKNID < 1)
            //        new DoiTuongKN().Insert(dtInfo);
            //    else
            //    {
            //        //dtInfo.DoiTuongKNID = doituongId;
            //        new DoiTuongKN().Update(dtInfo);
            //    }
            //}
        }

        protected int InsertDonThu(int nhomKNID, int doituongbiknID, TiepDanInfo data)
        {
            int donthuID = data.DonThuID ?? 0;
            TiepDanInfo info = new TiepDanInfo();
            info.NhomKNID = nhomKNID;
            info.DoiTuongBiKNID = doituongbiknID;
            info.LoaiKhieuTo1ID = data.LoaiKhieuTo1ID;
            info.LoaiKhieuTo2ID = data.LoaiKhieuTo2ID;
            info.LoaiKhieuTo3ID = data.LoaiKhieuTo3ID;
            info.LoaiKhieuToID = 0;
            info.NoiDungDon = data.NoiDungDon;
            info.TrungDon = false;
            info.DiaChiPhatSinh = data.DiaChiPhatSinh;
            info.TinhID = data.TinhID;
            info.HuyenID = data.HuyenID;
            info.XaID = data.XaID;
            info.LeTanChuyen = false;
            info.NgayVietDon = data.NgayVietDon ?? DateTime.Now;
            try
            {
                if (donthuID > 0)
                {
                    info.DonThuID = donthuID;
                    new Com.Gosol.KNTC.DAL.KNTC.TiepDan().UpdateDonThu(info);
                }
                else
                    donthuID = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().InsertDonThu(info);
            }
            catch { }
            return donthuID;
        }
        //int bikn_canhanID = 20; //loai doi tuong kn = ca nha
        //int bikn_coquanID = 19; //loai doi tuong kn = co quan, to chuc
        int bikn_canhanID = DMLoaiDoiTuongBiKN.CaNhan.GetHashCode(); //loai doi tuong kn = ca nha
        int bikn_coquanID = DMLoaiDoiTuongBiKN.CoQuanToChuc.GetHashCode(); //loai doi tuong kn = co quan, to chuc
        protected int InsertDoiTuongBiKN(TiepDanInfo data)
        {
            DoiTuongBiKNInfo dtInfo = new DoiTuongBiKNInfo();
            dtInfo = data.DoiTuongBiKN;
            //dtInfo.TinhID = Utils.ConvertToInt32(ddl_tinhdtbikn.SelectedValue, 0);
            //dtInfo.HuyenID = Utils.ConvertToInt32(ddl_huyendtbikn.SelectedValue, 0);
            //dtInfo.XaID = Utils.ConvertToInt32(ddl_xadtbikn.SelectedValue, 0);
            //dtInfo.DiaChiCT = txt_nhapdiachidtbikn.Text + ", " + txt_diachidtbikn.Text;

            //------------------------------------TH CO QUAN , TO CHUC-------------------------------------
            //Them doi tuong bi kn/tc
            //int doituongbiknId = Utils.ConvertToInt32(hfd_doituongbiKNID.Value, 0);
            //int canhanbiknId = Utils.ConvertToInt32(hdf_canhanbiKNID.Value, 0);

            //lay LoaiDoiTuongBiKNID
            //int loaiBiKNID = Utils.ConvertToInt32(hosodtbikn.SelectedValue, bikn_canhanID);
            if (dtInfo.LoaiDoiTuongBiKNID == DMLoaiDoiTuongBiKN.CoQuanToChuc.GetHashCode())
            {
                //insert doi tuong bi kn

                //dtInfo.LoaiDoiTuongBiKNID = loaiBiKNID;
                //dtInfo.TenDoiTuongBiKN = txtCQBiKN.Text;
                if (dtInfo.DoiTuongBiKNID < 1)
                    dtInfo.DoiTuongBiKNID = new DoiTuongBiKN().Insert(dtInfo);
                else
                {
                    //dtInfo.DoiTuongBiKNID = doituongbiknId;
                    new DoiTuongBiKN().Update(dtInfo);
                    //new DAL.CaNhanBiKN().Delete(canhanbiknId);
                }
            }
            //------------------------------------TH CA NHAN-------------------------------------------
            //insert ca nhan bi kn/tc neu (truong hop chon loai doi tuong la ca nhan)           
            if (dtInfo.LoaiDoiTuongBiKNID == DMLoaiDoiTuongBiKN.CaNhan.GetHashCode())
            {
                dtInfo.TenDoiTuongBiKN = chuyenDoiChuHoa(dtInfo.TenDoiTuongBiKN);
                //dtInfo.LoaiDoiTuongBiKNID = loaiBiKNID;

                //if (validationDoiTuongBiKN2(doituongbiknInfo))
                //{
                CaNhanBiKNInfo cnInfo = new CaNhanBiKNInfo();
                cnInfo.CaNhanBiKNID = dtInfo.CaNhanBiKNID;
                cnInfo.NgheNghiep = dtInfo.TenNgheNghiep;
                cnInfo.ChucVuID = dtInfo.ChucVuID;
                cnInfo.ChucVu = dtInfo.TenChucVu;
                cnInfo.GioiTinh = dtInfo.GioiTinhDoiTuongBiKN;
                cnInfo.QuocTichID = dtInfo.QuocTichDoiTuongBiKNID;
                cnInfo.DanTocID = dtInfo.DanTocDoiTuongBiKNID;
                cnInfo.NoiCongTac = dtInfo.NoiCongTacDoiTuongBiKN;
                cnInfo.DoiTuongBiKNID = dtInfo.DoiTuongBiKNID;
                if (dtInfo.DoiTuongBiKNID < 1)
                {
                    dtInfo.DoiTuongBiKNID = new DoiTuongBiKN().Insert(dtInfo);
                    cnInfo.DoiTuongBiKNID = dtInfo.DoiTuongBiKNID;
                    new CaNhanBiKN().Insert(cnInfo);
                }
                else
                {
                    //dtInfo.DoiTuongBiKNID = doituongbiknId;
                    new DoiTuongBiKN().Update(dtInfo);

                    if (dtInfo.CaNhanBiKNID < 1)
                    {
                        new CaNhanBiKN().Insert(cnInfo);
                    }
                    else
                    {
                        cnInfo.CaNhanBiKNID = dtInfo.CaNhanBiKNID;
                        //canhanbiknId = new DAL.CaNhanBiKN().GetByID
                        new CaNhanBiKN().Update(cnInfo);
                    }
                }
                //}
                //else
                //    return;

            }
            return dtInfo.DoiTuongBiKNID;
        }

        protected void InsertDoiTuongBiKN_V2(TiepDanInfo tiepDanInfo, int donThuID)
        {
            if (tiepDanInfo.DanhSachDoiTuongBiKN != null && tiepDanInfo.DanhSachDoiTuongBiKN.Count > 0)
            {
                var listIds = new List<int>();
                foreach (var data in tiepDanInfo.DanhSachDoiTuongBiKN)
                {
                    DoiTuongBiKNInfo dtInfo = new DoiTuongBiKNInfo();
                    dtInfo = data;
                    dtInfo.DonThuID = donThuID;
                    if (dtInfo.LoaiDoiTuongBiKNID == DMLoaiDoiTuongBiKN.CoQuanToChuc.GetHashCode())
                    {
                        if (dtInfo.DoiTuongBiKNID < 1)
                            dtInfo.DoiTuongBiKNID = new DoiTuongBiKN().Insert(dtInfo);
                        else
                        {
                            new DoiTuongBiKN().Update(dtInfo);
                        }
                    }

                    //------------------------------------TH CA NHAN-------------------------------------------
                    //insert ca nhan bi kn/tc neu (truong hop chon loai doi tuong la ca nhan)           
                    if (dtInfo.LoaiDoiTuongBiKNID == DMLoaiDoiTuongBiKN.CaNhan.GetHashCode())
                    {
                        dtInfo.TenDoiTuongBiKN = chuyenDoiChuHoa(dtInfo.TenDoiTuongBiKN);
                        CaNhanBiKNInfo cnInfo = new CaNhanBiKNInfo();
                        cnInfo.CaNhanBiKNID = dtInfo.CaNhanBiKNID;
                        cnInfo.NgheNghiep = dtInfo.TenNgheNghiep;
                        cnInfo.ChucVuID = dtInfo.ChucVuID;
                        cnInfo.ChucVu = dtInfo.TenChucVu;
                        cnInfo.GioiTinh = dtInfo.GioiTinhDoiTuongBiKN;
                        cnInfo.QuocTichID = dtInfo.QuocTichDoiTuongBiKNID;
                        cnInfo.DanTocID = dtInfo.DanTocDoiTuongBiKNID;
                        cnInfo.NoiCongTac = dtInfo.NoiCongTacDoiTuongBiKN;
                        cnInfo.DoiTuongBiKNID = dtInfo.DoiTuongBiKNID;
                        if (dtInfo.DoiTuongBiKNID < 1)
                        {
                            dtInfo.DoiTuongBiKNID = new DoiTuongBiKN().Insert(dtInfo);
                            cnInfo.DoiTuongBiKNID = dtInfo.DoiTuongBiKNID;
                            new CaNhanBiKN().Insert(cnInfo);
                        }
                        else
                        {
                            new DoiTuongBiKN().Update(dtInfo);

                            if (dtInfo.CaNhanBiKNID < 1)
                            {
                                new CaNhanBiKN().Insert(cnInfo);
                            }
                            else
                            {
                                cnInfo.CaNhanBiKNID = dtInfo.CaNhanBiKNID;
                                new CaNhanBiKN().Update(cnInfo);
                            }
                        }
                    }
                    listIds.Add(dtInfo.DoiTuongBiKNID);
                }
                new DoiTuongBiKN().DeleteByDonThuID(string.Join(",", listIds.Select(x => x)), tiepDanInfo.DonThuID ?? 0);
            }
        }

        protected TiepDanInfo InsertXuLyDon(IdentityHelper IdentityHelper, TiepDanInfo xldInfo, int donthuID, bool isTrungDon, bool isKNLan2)
        {
            bool suDungQTPhucTap = IdentityHelper.SuDungQuyTrinhPhucTap ?? false;
            bool suDungQTVanThuTiepDan = IdentityHelper.SuDungQTVanThuTiepDan ?? false;

            //int xldID = Utils.ConvertToInt32(hdf_xulydonId.Value, 0);
            int xldID = xldInfo.XuLyDonID ?? 0;
            //int donThuGocID = Utils.ConvertToInt32(hdfDonThuGocID.Value, 0);

            //TiepDanInfo xldInfo = new TiepDanInfo();
            //xldInfo.HuongGiaiQuyetID = Utils.ConvertToInt32(ddl_huonggiaiquyet.SelectedValue, 0);
            //int cqNhapHoID = 0;
            //if (cbxNhapHo.Checked)
            //{
            //    cqNhapHoID = Utils.ConvertToInt32(ddlCoQuanNhapHo.SelectedValue, 0);
            //}

            if (ValidationSubmit(xldInfo.HuongGiaiQuyetID ?? 0))
            {
                xldInfo.DonThuID = donthuID;
                if (isTrungDon)
                    xldInfo.SoLan = 2;
                else
                    xldInfo.SoLan = 1;

                //if (isKNLan2)
                //{
                //    xldInfo.LanGiaiQuyet = 2;
                //    xldInfo.DonThuGocID = donThuGocID;
                //}
                //else
                //{
                //    xldInfo.LanGiaiQuyet = 1;
                //    xldInfo.DonThuGocID = 0;
                //}

                //xldInfo.NgayNhapDon = Utils.ConvertToDateTime(txt_ngaynhapdon.Text, DateTime.MinValue);
                //xldInfo.NgayQuaHan = DateTime.MinValue;//Utils.ConvertToDateTime(txt_ngayquahan.Text, DateTime.MinValue);
                //xldInfo.NguonDonDen = (int)EnumNguonDonDen.TrucTiep;
                //xldInfo.CQChuyenDonID = 0;
                //xldInfo.SoCongVan = string.Empty;
                //xldInfo.NgayChuyenDon = DateTime.MinValue;
                //xldInfo.ThuocThamQuyen = false;
                //xldInfo.DuDieuKien = false;
                //if (xldInfo.HuongGiaiQuyetID == (int)HuongGiaiQuyetEnum.DeXuatThuLy)
                //    xldInfo.NoiDungHuongDan = txtKetquaxl.Text;
                //else
                //    xldInfo.NoiDungHuongDan = txtNoiDungHuongDan.Text;
                //xldInfo.CanBoXuLyID = IdentityHelper.GetCanBoID();
                //xldInfo.CanBoKyID = 0;
                //xldInfo.CQDaGiaiQuyetID = txtCoQuanDaGQ.Text;
                //xldInfo.TrangThaiDonID = 0;
                //xldInfo.PhanTichKQID = 0;
                //xldInfo.CanBoTiepNhapID = IdentityHelper.GetCanBoID();
                //if (cqNhapHoID != 0)
                //{
                //    xldInfo.CoQuanID = cqNhapHoID;
                //}
                //else
                //{
                //    xldInfo.CoQuanID = IdentityHelper.GetCoQuanID();
                //}
                //xldInfo.NgayThuLy = DateTime.MinValue;
                //xldInfo.LyDo = string.Empty;
                //xldInfo.DuAnID = 0;
                //if (!IdentityHelper.GetSuDungQuyTrinhPhucTap())
                //{
                //    xldInfo.NgayXuLy = DateTime.Now;
                //}
                //else
                //{
                //    if (xldInfo.HuongGiaiQuyetID != 0)
                //        xldInfo.NgayXuLy = DateTime.Now;
                //    else
                //        xldInfo.NgayXuLy = DateTime.MinValue;
                //}

                //xldInfo.DaDuyetXuLy = true;

                //if (xldInfo.HuongGiaiQuyetID == (int)HuongGiaiQuyetEnum.DeXuatThuLy)
                //{
                //    xldInfo.TrangThaiDonID = (int)TrangThaiDonEnum.DeXuatThuLy;
                //}

                //if (xldInfo.HuongGiaiQuyetID == (int)HuongGiaiQuyetEnum.ChuyenDon)
                //{
                //    xldInfo.CQChuyenDonID = Utils.ConvertToInt32(ddl_chuyendon_cqgiaiquyet.SelectedValue, 0);
                //    xldInfo.NgayChuyenDon = Utils.ConvertToDateTime(txt_ngaychuyendon.Text, DateTime.Now);
                //    xldInfo.LyDo = txt_ghichuchuyendon.Text;

                //}

                xldInfo.CBDuocChonXL = 0;
                xldInfo.QTTiepNhanDon = 0;
                xldInfo.NgayNhapDon = xldInfo.NgayNhapDon ?? xldInfo.NgayVietDon;

                if (suDungQTPhucTap && suDungQTVanThuTiepDan)
                {
                    //xldInfo.CBDuocChonXL = Utils.ConvertToInt32(ddlCanBoXL.SelectedValue, 0);
                    xldInfo.QTTiepNhanDon = (int)EnumQTTiepNhanDon.QTVanThuTiepDan;
                }

                if (xldInfo.XuLyDonID > 0)
                {
                    //xldInfo.XuLyDonID = xldID;
                    new Com.Gosol.KNTC.DAL.KNTC.TiepDan().UpdateXuLyDon(xldInfo);
                }
                else
                {
                    xldInfo.SoDonThu = GetSoDonThuByNamTiepNhan(IdentityHelper.CoQuanID ?? 0, IdentityHelper, xldInfo.NgayTiep.Value.Year) + $"/{xldInfo.NgayTiep.Value.Year}";
                    xldInfo.XuLyDonIDGoc = xldID;
                    xldInfo.CanBoTiepNhapID = IdentityHelper.CanBoID;
                    xldID = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().InsertXuLyDon(xldInfo);
                    xldInfo.XuLyDonID = xldID;
                }

            }

            return xldInfo;
        }

        protected int InsertTiepDanKhongDon(TiepDanInfo tdInfo, int xulydonID, int donthuID, int nhomKNID, int KQTiepDan, int letanchuyenID = 0, bool isTrungDon = false)
        {
            //int hgq = Utils.ConvertToInt32(ddl_huonggiaiquyet.SelectedValue, 0);
            //int tdkdID = Utils.ConvertToInt32(hdf_TiepDanID.Value, 0);
            //int lantiep = Utils.ConvertToInt32(hdf_LanTiep.Value, 0);
            int tiepdanID = 0;
            int cqNhapHoID = 0;

            //if (cbxNhapHo.Checked)
            //{
            //    cqNhapHoID = Utils.ConvertToInt32(ddlCoQuanNhapHo.SelectedValue, 0);
            //}

            //TiepDanInfo tdInfo = new TiepDanInfo();
            tdInfo.NhomKNID = nhomKNID;
            tdInfo.KQQuaTiepDan = KQTiepDan;
            tdInfo.XuLyDonID = xulydonID;
            //tdInfo.NgayTiep = Utils.ConvertToDateTime(txt_ngaynhapdon.Text, DateTime.MinValue);
            //if (rdl_gaplanhdao.SelectedValue == "0")
            //{
            //    tdInfo.GapLanhDao = false;
            //    tdInfo.TenLanhDaoTiep = string.Empty;
            //}
            //else
            //{
            //    tdInfo.GapLanhDao = true;
            //    tdInfo.TenLanhDaoTiep = txtTenLanhDao.Text;
            //}
            //tdInfo.NgayGapLanhDao = DateTime.MinValue;
            //tdInfo.NoiDungTiep = txt_noidung.Text;

            //tdInfo.VuViecCu = cbxVuViecCu.Checked;
            //tdInfo.CanBoTiepID = IdentityHelper.GetCanBoID();
            tdInfo.DonThuID = donthuID;
            //tdInfo.KQQuaTiepDan = KQQuaTiepDan;
            //if (cqNhapHoID != 0)
            //    tdInfo.CoQuanID = cqNhapHoID;
            //else tdInfo.CoQuanID = IdentityHelper.GetCoQuanID();

            if (isTrungDon)
                tdInfo.LanTiep = 2;
            else
                tdInfo.LanTiep = 1;

            //tdInfo.LoaiKhieuTo1ID = Utils.ConvertToInt32(ddl_loaidon.SelectedValue, 0);
            //tdInfo.LoaiKhieuTo2ID = Utils.ConvertToInt32(ddl_loaikn.SelectedValue, 0);
            //tdInfo.LoaiKhieuTo3ID = Utils.ConvertToInt32(ddl_chitiet.SelectedValue, 0);

            //tdInfo.NoiDungTiep = txtNoiDungTiep.Text;
            //tdInfo.YeuCauNguoiDuocTiep = txtYeuCauNguoiDuocTiep.Text;
            //tdInfo.ThongTinTaiLieu = txtThongTinTaiLieu.Text;
            //tdInfo.KetQuaTiep = txtKetQuaTiep.Text;
            //tdInfo.KetLuanNguoiChuTri = txtKetLuanNguoiChuTri.Text;
            //tdInfo.NguoiDuocTiepPhatBieu = txtNguoiDuocTiepPhatBieu.Text;
            //tdInfo.LanhDaoDangKy = txtDangKyGapLD.Text;

            //Cán bộ tham gia
            //int SoLuongCanBoThamGia = Utils.ConvertToInt32(slThanhPhanThamGia, 0);
            //List<ThanhPhanThamGiaInfo> listCanBoThamGia = new List<ThanhPhanThamGiaInfo>();
            //ThanhPhanThamGiaInfo cbtgInfo = new ThanhPhanThamGiaInfo();
            //cbtgInfo.TenCanBo = txtChucVuCanBoThamGia0.Text;
            //cbtgInfo.ChucVu = txtChucVuCanBoThamGia0.Text;
            //listCanBoThamGia.Add(cbtgInfo);
            if (tdInfo.TiepDanKhongDonID == null || tdInfo.TiepDanKhongDonID == 0)
            {
                tdInfo.SoDon = GetSTTTiepDan(tdInfo.CoQuanDangNhapID ?? 0).ToString();
                tiepdanID = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().InsertTiepDan(tdInfo);
            }
            else
            {
                tiepdanID = tdInfo.TiepDanKhongDonID ?? 0;
                //lantiep = Utils.ConvertToInt32(hdf_LanTiep.Value, 0);

                //tdInfo.TiepDanKhongDonID = tdkdID;
                //tdInfo.LanTiep = lantiep;
                new Com.Gosol.KNTC.DAL.KNTC.TiepDan().UpdateTiepDan(tdInfo);
                //Insert/update thành phần tham gia
                //List<ThanhPhanThamGiaInfo> listThanhPhanUpdate = GetDataThanhPhanThamGia(tdInfo.TiepDanKhongDonID);
                if (tdInfo.ThanhPhanThamGia != null && tdInfo.ThanhPhanThamGia.Count > 0)
                {
                    new Com.Gosol.KNTC.DAL.KNTC.TiepDan().InsertThanhPhanThamGia(tdInfo.ThanhPhanThamGia);
                }

            }
            //Insert/update thành phần tham gia
            //List<ThanhPhanThamGiaInfo> listThanhPhan = GetDataThanhPhanThamGia(tiepdanID);
            if (tdInfo.ThanhPhanThamGia != null && tdInfo.ThanhPhanThamGia.Count > 0)
            {
                foreach (var item in tdInfo.ThanhPhanThamGia)
                {
                    item.TiepDanKhongDonID = tiepdanID;
                }
                new Com.Gosol.KNTC.DAL.KNTC.TiepDan().InsertThanhPhanThamGia(tdInfo.ThanhPhanThamGia);
            }

            return tiepdanID;
        }

        private void ChuyenDon(TiepDanInfo xldGocInfo, int xuLyDonIDGoc)
        {
            //int cqNhapHoID = 0;
            //if (cbxNhapHo.Checked)
            //{
            //    cqNhapHoID = Utils.ConvertToInt32(ddlCoQuanNhapHo.SelectedValue, 0);
            //}

            //ChuyenXuLyInfo cxlInfo = new ChuyenXuLyInfo();
            //cxlInfo.XuLyDonID = xldGocInfo.XuLyDonID;
            //if (cqNhapHoID != 0)
            //{
            //    cxlInfo.CQGuiID = cqNhapHoID;
            //}
            //else
            //{
            //    cxlInfo.CQGuiID = IdentityHelper.GetCoQuanID();
            //}
            //cxlInfo.CQNhanID = Utils.ConvertToInt32(ddl_chuyendon_cqgiaiquyet.SelectedValue, 0);
            //cxlInfo.NgayChuyen = DateTime.Now;

            //CloneDonThuTaiCQDuocChuyenDon(cxlInfo.CQNhanID, xldGocInfo, xuLyDonIDGoc, xldGocInfo.DonThuID);

            //try
            //{
            //    new ChuyenXuLy().Insert(cxlInfo);
            //}
            //catch
            //{
            //}
        }

        private int GetSTTTiepDan(int coquanID)
        {
            int stt = 0;
            try
            {
                stt = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().GetSTT(coquanID);
            }
            catch { }

            return stt + 1;
        }
        //Com.Gosol.KNTC.DAL.KNTC
        protected List<ThanhPhanThamGiaInfo> GetDataThanhPhanThamGia(int TiepDanKhongDonID)
        {
            //string valStr = strThanhPhanThamGia.Value;
            //var listVal = valStr.Split('-');
            List<ThanhPhanThamGiaInfo> listThanhPhanThamGia = new List<ThanhPhanThamGiaInfo>();
            //if (valStr != "")
            //{
            //    foreach (var item in listVal)
            //    {
            //        var data = item.Split('*');
            //        ThanhPhanThamGiaInfo tpInfo = new ThanhPhanThamGiaInfo
            //        {
            //            TiepDanKhongDonID = TiepDanKhongDonID,
            //            TenCanBo = data[0],
            //            ChucVu = data[1]
            //        };
            //        listThanhPhanThamGia.Add(tpInfo);
            //    }
            //}
            return listThanhPhanThamGia;
        }

        #region -- attack document
        private void GanDonThuVaoWFVaThucThiCommand(IdentityHelper IdentityHelper, TiepDanInfo TiepDanInfo, int xulydonID)
        {
            string command = string.Empty;
            int TrinhTPDuyet = 2;
            int TrinhTPPhanXuLy = 3;
            int TiepDanThuLy = 4;
            int TiepDanKetThuc = 5;

            WorkflowInstance.Instance.AttachDocument(xulydonID, "XuLyDon", IdentityHelper.UserID ?? 0, null);
            List<string> commandList = WorkflowInstance.Instance.GetAvailabelCommands(xulydonID);

            if (IdentityHelper.SuDungQuyTrinhPhucTap == true)
            {

            }
            else
            {
                #region quy trinh don gian
                bool isDeXuatThuLy = false;
                int huongxuly = TiepDanInfo.HuongGiaiQuyetID ?? 0;

                int raVBDonDoc = (int)HuongGiaiQuyetEnum.RaVanBanDonDoc;
                int cVanChiDao = (int)HuongGiaiQuyetEnum.CongVanChiDao;

                if (huongxuly != 0)
                {
                    if (huongxuly == ((int)HuongGiaiQuyetEnum.DeXuatThuLy)) isDeXuatThuLy = true;
                    if (isDeXuatThuLy)
                    {
                        if (TiepDanInfo.LoaiKhieuTo1ID == Constant.TranhChap && IdentityHelper.CapID == (int)CapQuanLy.CapUBNDXa)
                        {
                            command = commandList.Where(x => x.ToString() == "TiepDanKetThuc").FirstOrDefault();
                        }
                        else
                        {
                            command = commandList.Where(x => x.ToString() == "TiepDanThuLy").FirstOrDefault();
                        }
                    }
                    else
                    {
                        if (huongxuly == cVanChiDao || huongxuly == raVBDonDoc)
                        {
                            command = commandList.Where(x => x.ToString() == "ChuyenDonHoacGuiVBDonDoc").FirstOrDefault();
                        }
                        else
                        {
                            command = commandList.Where(x => x.ToString() == "TiepDanKetThuc").FirstOrDefault();
                        }

                    }
                    WorkflowInstance.Instance.ExecuteCommand(xulydonID, IdentityHelper.CanBoID ?? 0, command, DateTime.Now.AddDays(10), string.Empty);
                }
                #endregion
            }
        }

        private void UpdateDonThuVaoWFVaThucThiCommand(IdentityHelper IdentityHelper, TiepDanInfo TiepDanInfo, int xulydonID)
        {
            string command = string.Empty;
            int TrinhTPDuyet = 2;
            int TrinhTPPhanXuLy = 3;
            int TiepDanThuLy = 4;
            int TiepDanKetThuc = 5;

            bool result = WorkflowInstance.Instance.DeleteDocument(xulydonID);
            if (result)
            {
                WorkflowInstance.Instance.AttachDocument(xulydonID, "XuLyDon", IdentityHelper.UserID ?? 0, null);
                List<string> commandList = WorkflowInstance.Instance.GetAvailabelCommands(xulydonID);

                if (IdentityHelper.SuDungQuyTrinhPhucTap == true)
                {

                }
                else
                {
                    #region quy trinh don gian
                    bool isDeXuatThuLy = false;
                    int huongxuly = TiepDanInfo.HuongGiaiQuyetID ?? 0;

                    int raVBDonDoc = (int)HuongGiaiQuyetEnum.RaVanBanDonDoc;
                    int cVanChiDao = (int)HuongGiaiQuyetEnum.CongVanChiDao;

                    if (huongxuly != 0)
                    {
                        if (huongxuly == ((int)HuongGiaiQuyetEnum.DeXuatThuLy)) isDeXuatThuLy = true;
                        if (isDeXuatThuLy)
                        {
                            if (TiepDanInfo.LoaiKhieuTo1ID == Constant.TranhChap && IdentityHelper.CapID == (int)CapQuanLy.CapUBNDXa)
                            {
                                command = commandList.Where(x => x.ToString() == "TiepDanKetThuc").FirstOrDefault();
                            }
                            else
                            {
                                command = commandList.Where(x => x.ToString() == "TiepDanThuLy").FirstOrDefault();
                            }
                        }
                        else
                        {
                            if (huongxuly == cVanChiDao || huongxuly == raVBDonDoc)
                            {
                                command = commandList.Where(x => x.ToString() == "ChuyenDonHoacGuiVBDonDoc").FirstOrDefault();
                            }
                            else
                            {
                                command = commandList.Where(x => x.ToString() == "TiepDanKetThuc").FirstOrDefault();
                            }

                        }
                        WorkflowInstance.Instance.ExecuteCommand(xulydonID, IdentityHelper.CanBoID ?? 0, command, DateTime.Now.AddDays(10), string.Empty);
                    }
                    #endregion
                }
            }

        }
        #endregion

        public string GetSoDonThu(int coquanID, IdentityHelper IdentityHelper)
        {
            string soDonThuFull = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().GetSoDonThu(coquanID);
            string maCQ = string.Empty;

            if (coquanID == IdentityHelper.CoQuanID)
            {
                maCQ = IdentityHelper.MaCoQuan;
            }
            else
            {
                CoQuanInfo cqInfo = new CoQuan().GetCoQuanByID(coquanID);
                maCQ = cqInfo.MaCQ;
            }
            var soDonThu = !string.IsNullOrEmpty(soDonThuFull) ? soDonThuFull.Split('/')[0] : "";
            string numberPart = Regex.Replace(soDonThu.Replace(maCQ, ""), "[^0-9.]", "");
            int soDonMoi = Utils.ConvertToInt32(numberPart, 0) + 1;
            return maCQ + soDonMoi;
        }

        public string GetSoDonThuByNamTiepNhan(int coquanID, IdentityHelper IdentityHelper, int? namTiepNhan = null)
        {
            var namTiepNhanInt = (namTiepNhan ?? 0) == 0 ? DateTime.Now.Year : (int)namTiepNhan;
            string soDonThuFull = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().GetSoDonThuByNamTiepNhan(coquanID, namTiepNhanInt);
            string maCQ = string.Empty;

            if (coquanID == IdentityHelper.CoQuanID)
            {
                maCQ = IdentityHelper.MaCoQuan;
            }
            else
            {
                CoQuanInfo cqInfo = new CoQuan().GetCoQuanByID(coquanID);
                maCQ = cqInfo.MaCQ;
            }
            var soDonThu = !string.IsNullOrEmpty(soDonThuFull) ? soDonThuFull.Split('/')[0] : "";
            string numberPart = Regex.Replace(soDonThu.Replace(maCQ, ""), "[^0-9.]", "");
            int soDonMoi = Utils.ConvertToInt32(numberPart, 0) + 1;
            return maCQ + soDonMoi;
        }

        private bool ValidationSubmit(int hgqId)
        {
            if (hgqId == (int)HuongGiaiQuyetEnum.ChuyenDon)
            {
                //if (Utils.ConvertToInt32(ddl_chuyendon_cqgiaiquyet.SelectedValue, 0) < 1)
                //{
                //    plh_err.Visible = true;
                //    err_msg.Text = "Vui lòng chọn Cơ quan giải quyết!";
                //    ddl_chuyendon_cqgiaiquyet.Focus();
                //    return false;
                //}
            }

            return true;
        }

        private void ChuyenXuLyChoPhong(int phongbanID, int xulydonID)
        {
            try
            {
                //PhanXuLuInfo phanXLInfo = new PhanXuLuInfo();
                //phanXLInfo.XuLyDonID = xulydonID;
                //phanXLInfo.PhongBanID = phongbanID;
                //phanXLInfo.NgayPhan = DateTime.Now;
                //phanXLInfo.GhiChu = string.Empty;
                //new DAL.PhanXuLy().Insert(phanXLInfo);
            }
            catch (Exception e)
            {
            }
        }

        private string chuyenDoiChuHoa(string str)
        {
            string strchuyendoi = "";
            string[] laytu = !string.IsNullOrEmpty(str) ? str.Split(' ') : new string[0];
            string kytudau = "";
            for (int i = 0; i < laytu.Length; i++)
            {
                if (laytu[i].ToString() != "")
                {
                    kytudau = laytu[i].Substring(0, 1);
                    strchuyendoi += kytudau.ToUpper() + laytu[i].Remove(0, 1) + " ";
                }
            }
            return strchuyendoi;
        }

        private void CapNhatFileDinhKem(TiepDanInfo tiepDanInfo, int xuLyDonID, int donThuID)
        {
            if (tiepDanInfo.DanhSachHoSoTaiLieu != null && tiepDanInfo.DanhSachHoSoTaiLieu.Count > 0)
            {
                foreach (var item in tiepDanInfo.DanhSachHoSoTaiLieu)
                {
                    if (item.DanhSachFileDinhKemID != null && item.DanhSachFileDinhKemID.Count > 0)
                    {
                        new FileDinhKemDAL().UpdateFileHoSo(item.DanhSachFileDinhKemID, xuLyDonID, donThuID, string.Empty);
                        foreach (var file in item.DanhSachFileDinhKemID)
                        {
                            new FileLogDAL().Delete(file, item.FileType);
                        }

                    }
                    // xóa file
                    if (item.FileDinhKemDelete != null && item.FileDinhKemDelete.Count > 0)
                    {
                        item.FileDinhKemDelete.ForEach(x => x.FileType = item.FileType);
                        new FileDinhKemDAL().Delete(item.FileDinhKemDelete);
                    }
                }

            }
            //if (tiepDanInfo.FileCQGiaiQuyet != null && tiepDanInfo.FileCQGiaiQuyet.Count > 0)
            //{
            //    foreach (var item in tiepDanInfo.FileCQGiaiQuyet)
            //    {
            //        if (item.DanhSachFileDinhKemID != null && item.DanhSachFileDinhKemID.Count > 0)
            //        {
            //            new FileDinhKemDAL().UpdateNghiepVuID_New(item.DanhSachFileDinhKemID, xuLyDonID);
            //        }
            //    }

            //}
            //if (tiepDanInfo.FileKQTiep != null && tiepDanInfo.FileKQTiep.Count > 0)
            //{
            //    foreach (var item in tiepDanInfo.FileKQTiep)
            //    {
            //        if (item.DanhSachFileDinhKemID != null && item.DanhSachFileDinhKemID.Count > 0)
            //        {
            //            new FileDinhKemDAL().UpdateNghiepVuID_New(item.DanhSachFileDinhKemID, xuLyDonID);
            //        }
            //    }

            //}
            //if (tiepDanInfo.FileKQGiaiQuyet != null && tiepDanInfo.FileKQGiaiQuyet.Count > 0)
            //{
            //    foreach (var item in tiepDanInfo.FileKQGiaiQuyet)
            //    {
            //        if (item.DanhSachFileDinhKemID != null && item.DanhSachFileDinhKemID.Count > 0)
            //        {
            //            new FileDinhKemDAL().UpdateNghiepVuID_New(item.DanhSachFileDinhKemID, xuLyDonID);
            //        }
            //    }

            //}
        }

        public IList<BieuMauInfo> DanhSachBieuMau(int CapID)
        {
            if (CapID == CapQuanLy.CapSoNganh.GetHashCode() || CapID == CapQuanLy.CapUBNDHuyen.GetHashCode() || CapID == CapQuanLy.CapPhong.GetHashCode())
            {
                CapID = CapQuanLy.CapUBNDTinh.GetHashCode();
            }
            IList<BieuMauInfo> list = new BieuMau().GetBySearch(0, 5000, CapID, "%%");
            return list;
        }

        public BaseResultModel InPhieu(string MaPhieuIn, int XuLyDonID, int DonThuID, int TiepDanKhongDonID, int CapID, string server, int CoQuanID, int CanBoID)
        {
            var Result = new BaseResultModel();
            string path = server + "/Templates/PhieuIn/";

            if (CapID == (int)CapQuanLy.CapUBNDXa)
            {
                path += "CapXa/";
            }
            string document = path + MaPhieuIn + ".docx";
            string tmp = "Templates/FileTam/" + MaPhieuIn + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".docx";
            string tmpPDF = "Templates/FileTam/" + MaPhieuIn + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".pdf";
            string filecopy = server + tmp;
            File.Copy(document, filecopy);
            //Replace(filecopy, "DAY", DateTime.Now.Day.ToString());
            var coQuanInfo = new CoQuan().GetCoQuanByID(CoQuanID);
            var canBoInfo = new CanBo().GetCanBoByID(CanBoID);
            var xldInfo = new XuLyDonDAL().GetByID(XuLyDonID);
            var dTBiKNInfo = new DoiTuongBiKN().GetByDonThuID(DonThuID);
            var tiepDanInfo = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().GetTiepDanByTiepDanID(TiepDanKhongDonID);

            if (tiepDanInfo != null)
            {
                new InPhieuDAL().FillTiepDanData(filecopy, tiepDanInfo);
            }

            if (xldInfo != null)
            {
                var donThuInfo = new DonThuDAL().GetByID(xldInfo.DonThuID, XuLyDonID);

                new InPhieuDAL().FillXuLyDonData(filecopy, xldInfo, coQuanInfo, canBoInfo);
                new InPhieuDAL().FillDonThuData(filecopy, donThuInfo);
            }
            else
            {

            }

            if (dTBiKNInfo != null)
            {
                new InPhieuDAL().FillDTBiKNData(filecopy, dTBiKNInfo);
            }
            #region pdf
            //string pathPDF = server + tmpPDF;
            //Application app = null;
            //Document doc = null;
            //var stream = new MemoryStream();
            //IFormatter formatter = new BinaryFormatter();
            //object Missing = System.Reflection.Missing.Value;
            //try
            //{
            //    app = new Microsoft.Office.Interop.Word.Application();
            //    if (filecopy != null && filecopy.Length > 0)
            //    {
            //        doc = app.Documents.Open(filecopy, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing);
            //        if (tiepDanInfo != null)
            //        {
            //            //pathPDF = "~/Templates/PhieuIn/" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + tiepDanKhongDonID + "_NewPDFFile.pdf";
            //            //pathPDF = context.Server.MapPath(pathPDF);

            //            doc.ExportAsFixedFormat(pathPDF, Microsoft.Office.Interop.Word.WdExportFormat.wdExportFormatPDF);
            //            //using (FileStream file = new FileStream(pathPDF, FileMode.Open, FileAccess.Read))
            //            //    file.CopyTo(stream);
            //        }
            //    }
            //}
            //finally
            //{
            //    try
            //    {
            //        if (doc != null) ((Microsoft.Office.Interop.Word._Document)doc).Close(WdSaveOptions.wdDoNotSaveChanges);
            //        //Xóa file
            //        //File.Delete(pathDocx);
            //        File.Delete(pathPDF);
            //    }
            //    finally { }
            //    if (app != null) ((Microsoft.Office.Interop.Word._Application)app).Quit(true, Missing, Missing);
            //}
            #endregion

            new InPhieuDAL().Clear(filecopy);

            Result.Status = 1;
            Result.Data = tmp;
            return Result;
        }

        public BaseResultModel InPhieuPDF(string MaPhieuIn, int XuLyDonID, int DonThuID, int TiepDanKhongDonID, int CapID, string server, int CoQuanID, int CanBoID, IConfiguration _config)
        {
            var Result = new BaseResultModel();
            string path = server + "/Templates/PhieuIn/";

            if (CapID == (int)CapQuanLy.CapUBNDXa)
            {
                path += "CapXa/";
            }
            string document = path + MaPhieuIn + ".docx";
            string FileName = MaPhieuIn + DateTime.Now.ToString("ddMMyyyyHHmmss");
            string tmp = "Templates/FileTam/" + FileName + ".docx";
            string tmpPDF = "Templates/FileTam/" + FileName + ".pdf";
            string filecopy = server + tmp;
            File.Copy(document, filecopy);
            //Replace(filecopy, "DAY", DateTime.Now.Day.ToString());
            var coQuanInfo = new CoQuan().GetCoQuanByID(CoQuanID);
            var canBoInfo = new CanBo().GetCanBoByID(CanBoID);
            var xldInfo = new XuLyDonDAL().GetByID(XuLyDonID);
            var dTBiKNInfo = new DoiTuongBiKN().GetByDonThuID(DonThuID);
            var tiepDanInfo = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().GetTiepDanByTiepDanID(TiepDanKhongDonID);

            if (tiepDanInfo != null)
            {
                new InPhieuDAL().FillTiepDanData(filecopy, tiepDanInfo);
            }

            if (xldInfo != null)
            {
                var donThuInfo = new DonThuDAL().GetByID(xldInfo.DonThuID, XuLyDonID);

                new InPhieuDAL().FillXuLyDonData(filecopy, xldInfo, coQuanInfo, canBoInfo);
                new InPhieuDAL().FillDonThuData(filecopy, donThuInfo);
            }
            else
            {

            }

            if (dTBiKNInfo != null)
            {
                new InPhieuDAL().FillDTBiKNData(filecopy, dTBiKNInfo);
            }


            new InPhieuDAL().Clear(filecopy);

            string url = _config.GetValue<string>("AppSettings:ApiConvert") + FileName;
            var client = new RestClient(url);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

            Result.Status = 1;
            Result.Data = tmpPDF;
            //Result.Data = url;
            return Result;
        }

        public IList<DTXuLyInfo> GetBySearchDonThuDaTiepNhan(ref int TotalRow, IdentityHelper IdentityHelper, int? LoaiKhieuToID, string Keyword, DateTime? TuNgay, DateTime? DenNgay, int? CQChuyenDonDenID, int? LoaiRutDon, int CurrentPage)
        {

            IList<DTXuLyInfo> ListInfo = new List<DTXuLyInfo>();

            QueryFilterInfo info = new QueryFilterInfo();
            info.CoQuanID = IdentityHelper.CoQuanID ?? 0;
            info.KeyWord = Keyword ?? "";
            info.Start = (CurrentPage - 1) * IdentityHelper.PageSize;
            info.End = CurrentPage * IdentityHelper.PageSize;
            info.LoaiKhieuToID = LoaiKhieuToID ?? 0;
            info.TuNgay = TuNgay;
            info.DenNgay = DenNgay;
            info.CanBoID = IdentityHelper.CanBoID ?? 0;
            info.CQChuyenDonDenID = CQChuyenDonDenID ?? 0;
            info.LoaiRutDon = LoaiRutDon ?? 0;

            try
            {
                ListInfo = new DTXuLy().GetSoTiepNhanGianTiep_BTDTinh(ref TotalRow, info);

            }
            catch
            {
                throw;
            }

            return ListInfo;
        }

        public BaseResultModel CapNhapSoDonThuTheoNam(IdentityHelper IdentityHelper, int namTiepNhan)
        {
            var resultModel = new BaseResultModel();

            try
            {
                var val = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().CapNhapSoDonThuTheoNamVaCoQuan(IdentityHelper.CoQuanID ?? 0, namTiepNhan);
            }
            catch (Exception ex)
            {
                resultModel.Status = -1;
                resultModel.Message = $"Cập nhập thất bại!";
                resultModel.MessageDetail = ex.Message;
                throw;
            }

            resultModel.Status = 1;
            resultModel.Message = $"Cập nhập thành!";
            //Result.Data = url;
            return resultModel;
        }
        /// <summary>
        /// Chuyển đơn thu từ tiếp dân thường xuyên sang tiếp nhận đơn
        /// </summary>
        public BaseResultModel ChuyenDonTDTTSangTND(string xuLyDonIDIDS, string donThuIDIds)
        {
            var result = new BaseResultModel();
            try
            {
                result = new XuLyDonDAL().ChuyenDon_Sang_TiepNhanDon(xuLyDonIDIDS, donThuIDIds);
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                throw;
            }
            return result;
        }
    }
}
