using Com.Gosol.KNTC.DAL.BaoCao;
using Com.Gosol.KNTC.DAL.KNTC;
using Com.Gosol.KNTC.Models.BaoCao;
using Com.Gosol.KNTC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Model.HeThong;
using System.Security.Cryptography.Xml;
using Com.Gosol.KNTC.Models.TiepDan;
using FileHoSoInfo = Com.Gosol.KNTC.Models.KNTC.FileHoSoInfo;
using Com.Gosol.KNTC.Ultilities;
using System.Data.SqlClient;
using System.Data;
using Com.Gosol.KNTC.Models.HeThong;
using DocumentFormat.OpenXml.EMMA;
using Com.Gosol.KNTC.DAL.HeThong;

namespace Com.Gosol.KNTC.BUS.KNTC
{
    public class ChiTietDonThuBUS
    {
        private DonThuDAL donThuDAL;
        public ChiTietDonThuBUS()
        {
            donThuDAL = new DonThuDAL();
        }

        public BaseResultModel GetChiTietDonThu(int DonThuID, int XuLyDonID, int CanBoID, string serverPath)
        {
            var Result = new BaseResultModel();
            try
            {
                DonThuChiTietModel DonThuChiTiet = new DonThuChiTietModel();
                DonThuChiTiet.DonThu = donThuDAL.GetByID(DonThuID, XuLyDonID);

                #region bổ sung thông tin để biết quy trình đơn thư
                DonThuChiTiet.XuLyDon = new XuLyDonDAL().GetByXuLyDonID_V2(XuLyDonID);
                DonThuChiTiet.LanhDaoDuyet1 = new CanBoDAL().GetByID(DonThuChiTiet.XuLyDon?.LanhDaoDuyet1ID ?? 0);
                DonThuChiTiet.LanhDaoDuyet2 = new CanBoDAL().GetByID(DonThuChiTiet.XuLyDon?.LanhDaoDuyet2ID ?? 0);
                DonThuChiTiet.DanhSachCoQuanGiaiQuyet = new ChuyenGiaiQuyet().GetListChuyenGiaiQuyet(XuLyDonID);
                DonThuChiTiet.PhanTPPhanGQModels = new PhanTPPhanGQ().GetByXuLyDonID(XuLyDonID).ToList();
                DonThuChiTiet.VaiTroGiaiQuyetInfos = new VaiTroGiaiQuyet().GetByXuLyDonID(XuLyDonID).ToList();
                DonThuChiTiet.CoQuanChuyenDonDi = new CoQuan().GetCoQuanByCoQuanID(DonThuChiTiet.XuLyDon.CQChuyenDonID).ToList();
                DonThuChiTiet.CoQuanChuyenDonDen = new CoQuan().GetCoQuanByCoQuanID(DonThuChiTiet.XuLyDon.CQChuyenDonDenID).ToList();
                DonThuChiTiet.ThongTinRutDon = new RutDon_V2BUS().GetByXuLyDonID(XuLyDonID);
                #endregion

                DonThuChiTiet.TiepDanInfo = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().GetTiepDanByXuLyDonID(XuLyDonID);


                //lấy danh sách file trong tiếp dân, tiếp nhận đơn cho đơn thư
                var fileTaiLieu = new FileHoSoDAL().FileTaiLieu_GetByXuLyDonID(XuLyDonID);
                if (fileTaiLieu != null && fileTaiLieu.Count > 0)
                {
                    #region file cơ quan đã giải quyết - lấy từ màn hình tiếp nhận đơn
                    List<FileHoSoInfo> fileCoQuanGiaiQuyet = new List<FileHoSoInfo>();
                    fileCoQuanGiaiQuyet = fileTaiLieu.Where(x => x.LoaiFile == EnumLoaiFile.FileCQGiaiQuyet.GetHashCode()).ToList();
                    if (fileCoQuanGiaiQuyet != null && fileCoQuanGiaiQuyet.Count > 0)
                    {
                        DonThuChiTiet.DonThu.FileCQGiaiQuyet = new List<DanhSachHoSoTaiLieu>();
                        DonThuChiTiet.DonThu.FileCQGiaiQuyet = fileCoQuanGiaiQuyet.GroupBy(p => p.GroupUID)
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
                        DonThuChiTiet.DonThu.FileKQTiep = new List<DanhSachHoSoTaiLieu>();
                        DonThuChiTiet.DonThu.FileKQTiep = fileKetQuaTiep.GroupBy(p => p.GroupUID)
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
                        DonThuChiTiet.DonThu.FileKQGiaiQuyet = new List<DanhSachHoSoTaiLieu>();
                        DonThuChiTiet.DonThu.FileKQGiaiQuyet = fileKetQuaGiaiQuyet.GroupBy(p => p.GroupUID)
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

                ////////////////


                try
                {
                    DonThuChiTiet.NhomKN = new NhomKN().GetByID(DonThuChiTiet.DonThu.NhomKNID);
                    if (DonThuChiTiet.NhomKN != null)
                    {
                        DonThuChiTiet.NhomKN.DanhSachDoiTuongKN = new DoiTuongKNDAL().GetCustomDataByNhomKNID(DonThuChiTiet.DonThu.NhomKNID).ToList();
                    }

                }
                catch
                {
                }

                try
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
                    DonThuChiTiet.KetQuaTranhChap = Info;
                }
                catch (Exception)
                {


                }

                try
                {
                    List<FileHoSoInfo> fileDinhKem = new List<FileHoSoInfo>();
                    CanBoInfo canBoInfo = new CanBo().GetCanBoByID(CanBoID);
                    if (canBoInfo.XemTaiLieuMat)
                    {
                        fileDinhKem = new FileHoSoDAL().GetByXuLyDonID_TrungDon(XuLyDonID).ToList();

                    }
                    else
                    {
                        fileDinhKem = new FileHoSoDAL().GetByXuLyDonID_TrungDon(XuLyDonID).Where(x => x.IsBaoMat != true || x.CanBoID == canBoInfo.CanBoID).ToList();
                    }
                    if (fileDinhKem.Count > 0)
                    {
                        DonThuChiTiet.DanhSachHoSoTaiLieu = new List<DanhSachHoSoTaiLieu>();
                        DonThuChiTiet.DanhSachHoSoTaiLieu = fileDinhKem.GroupBy(p => p.GroupUID)
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
                                                   FileUrl = serverPath + y.FirstOrDefault().FileURL,
                                               }
                                               ).ToList(),

                           }
                           ).ToList();
                    }
                }
                catch (Exception)
                {
                }

                try
                {
                    var danhSachDoiTuongBiKN = new DoiTuongBiKN().GetDanhSachByDonThuID(DonThuChiTiet.DonThu.DonThuID);
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
                        DonThuChiTiet.DanhSachDoiTuongBiKN = datas;
                    }

                    //int DTBiKNID = DonThuChiTiet.DonThu.DoiTuongBiKNID;
                    //DonThuChiTiet.DoiTuongBiKN = new DoiTuongBiKN().GetByID(DTBiKNID);
                    //CaNhanBiKNInfo canhanbiknInfo = new CaNhanBiKN().getCaNhanBiKN(DTBiKNID);
                    //if (canhanbiknInfo != null)
                    //{
                    //    DonThuChiTiet.DoiTuongBiKN.CaNhanBiKNID = canhanbiknInfo.CaNhanBiKNID;
                    //    DonThuChiTiet.DoiTuongBiKN.TenNgheNghiep = canhanbiknInfo.NgheNghiep;
                    //    DonThuChiTiet.DoiTuongBiKN.ChucVuID = canhanbiknInfo.ChucVuID;
                    //    DonThuChiTiet.DoiTuongBiKN.TenChucVu = canhanbiknInfo.TenChucVu;
                    //    DonThuChiTiet.DoiTuongBiKN.QuocTichDoiTuongBiKNID = canhanbiknInfo.QuocTichID;
                    //    DonThuChiTiet.DoiTuongBiKN.NoiCongTacDoiTuongBiKN = canhanbiknInfo.NoiCongTac;
                    //    DonThuChiTiet.DoiTuongBiKN.DanTocDoiTuongBiKNID = canhanbiknInfo.DanTocID;
                    //    DonThuChiTiet.DoiTuongBiKN.NoiCongTacDoiTuongBiKN = canhanbiknInfo.NoiCongTac;
                    //    DonThuChiTiet.DoiTuongBiKN.GioiTinhDoiTuongBiKN = canhanbiknInfo.GioiTinh;
                    //}
                }
                catch (Exception)
                {
                }

                #region get all file
                try
                {
                    List<FileHoSoInfo> listFile = new List<FileHoSoInfo>();
                    List<FileHoSoInfo> listFileHoSo = new List<FileHoSoInfo>();
                    List<FileHoSoInfo> listFileHoSo_DonDoc = new List<FileHoSoInfo>();
                    List<RutDonInfo> rutdonInfo = new List<RutDonInfo>();
                    List<XuLyDonInfo> lsFileYKienXuLy = new List<XuLyDonInfo>();
                    List<FileHoSoInfo> lsFilePhanXL = new List<FileHoSoInfo>();
                    List<FileKetQuaTranhChapInfo> lsFileKQ = new List<FileKetQuaTranhChapInfo>();
                    List<ChuyenGiaiQuyetInfo> cgqList = new List<ChuyenGiaiQuyetInfo>();
                    List<TheoDoiXuLyInfo> lsFileXacMinh = new List<TheoDoiXuLyInfo>();
                    List<XuLyDonInfo> lsYKienGiaiQuyet = new List<XuLyDonInfo>();
                    List<TheoDoiXuLyInfo> lsFilePheDuyetXacMinh = new List<TheoDoiXuLyInfo>();
                    KetQuaInfo kqInfo = new KetQuaInfo();
                    ThiHanhInfo thiHanhInfo = new ThiHanhInfo();

                    CanBoInfo canBoInfo = new CanBo().GetCanBoByID(CanBoID);
                    if (canBoInfo.XemTaiLieuMat)
                    {
                        //lay thong tin file ho so
                        //listFileHoSo = new FileHoSo().GetByXuLyDonID(XuLyDonID).ToList();
                        listFileHoSo = new FileHoSoDAL().GetByXuLyDonID_TrungDon(XuLyDonID).ToList();
                        listFileHoSo_DonDoc = new FileHoSoDAL().GetByXuLyDonID_DonDoc(XuLyDonID).ToList();
                        //lay thong tin rut don
                        rutdonInfo = new RutDonDAL().GetByXuLyDonID(XuLyDonID);
                        //file y kien xl
                        lsFileYKienXuLy = new XuLyDonDAL().GetFileYKienXuLy(XuLyDonID).ToList();
                        if (lsFileYKienXuLy != null)
                        {
                            foreach (var fileInfo in lsFileYKienXuLy)
                            {
                                fileInfo.NgayUps = Format.FormatDate(fileInfo.NgayUp);
                            }
                        }
                        lsFilePhanXL = new FileHoSoDAL().GetFilePhanXuLyByXuLyDonID(XuLyDonID).ToList();
                        if (lsFilePhanXL != null)
                        {
                            foreach (var fileInfo in lsFilePhanXL)
                            {
                                fileInfo.NgayUps = Format.FormatDate(fileInfo.NgayUp);
                            }
                        }
                        //File tranh chấp
                        lsFileKQ = new KetQuaTranhChapDAL().GetFileByXuLyDonID(XuLyDonID).ToList();
                        // lay thong tin quyet dinh giao xac minh
                        cgqList = new TheoDoiXuLyDAL().GetQuyetDinhGiaoXacMinh(XuLyDonID).ToList();
                        // lay thong tin qua trinh xac minh
                        lsFileXacMinh = new TheoDoiXuLyDAL().GetQuaTrinhXacMinh(XuLyDonID).ToList();
                        lsYKienGiaiQuyet = new XuLyDonDAL().GetYKienGiaiQuyet(XuLyDonID).ToList();
                        if (lsYKienGiaiQuyet != null)
                        {
                            foreach (var donThuInfo in lsYKienGiaiQuyet)
                            {
                                donThuInfo.NgayGiaiQuyetStr = Format.FormatDate(donThuInfo.NgayGiaiQuyet);
                            }
                        }
                        lsFilePheDuyetXacMinh = new XuLyDonDAL().GetPheDuyetBaoCaoXacMinh(XuLyDonID);
                        //lay thong tin quyet dinh giai quyet don
                        kqInfo = new KetQuaDAL().GetCustomByXuLyDonID(XuLyDonID);
                        if (kqInfo != null)
                        {
                            kqInfo.NgayRaKQStr = Format.FormatDate(kqInfo.NgayRaKQ ?? DateTime.Now);
                        }
                        //lay thong tin ket qua thi hanh
                        thiHanhInfo = new ThiHanhDAL().ThiHanh_KetQua_GetByID(XuLyDonID);
                        if (thiHanhInfo != null)
                        {
                            thiHanhInfo.NgayThiHanhStr = Format.FormatDate(thiHanhInfo.NgayThiHanh);
                        }
                    }
                    else
                    {
                        //lay thong tin file ho so
                        //listFileHoSo = new FileHoSo().GetByXuLyDonID(XuLyDonID).Where(x => x.IsBaoMat != true || x.CanBoID == canBoID).ToList();
                        listFileHoSo = new FileHoSoDAL().GetByXuLyDonID_TrungDon(XuLyDonID).Where(x => x.IsBaoMat != true || x.CanBoID == CanBoID).ToList();
                        listFileHoSo_DonDoc = new FileHoSoDAL().GetByXuLyDonID_DonDoc(XuLyDonID).Where(x => x.IsBaoMat != true || x.CanBoID == CanBoID).ToList();
                        //lay thong tin rut don
                        rutdonInfo = new RutDonDAL().GetByXuLyDonID(XuLyDonID).Where(x => x.IsBaoMat != true || x.NguoiUp == CanBoID).ToList();
                        //file y kien xl
                        lsFileYKienXuLy = new XuLyDonDAL().GetFileYKienXuLy(XuLyDonID).Where(x => x.IsBaoMat != true || x.NguoiUp == CanBoID).ToList();
                        if (lsFileYKienXuLy != null)
                        {
                            foreach (var fileInfo in lsFileYKienXuLy)
                            {
                                fileInfo.NgayUps = Format.FormatDate(fileInfo.NgayUp);
                            }
                        }
                        lsFilePhanXL = new FileHoSoDAL().GetFilePhanXuLyByXuLyDonID(XuLyDonID).ToList().Where(x => x.IsBaoMat != true).ToList();
                        if (lsFilePhanXL != null)
                        {
                            foreach (var fileInfo in lsFilePhanXL)
                            {
                                fileInfo.NgayUps = Format.FormatDate(fileInfo.NgayUp);
                            }
                        }
                        //File tranh chấp
                        lsFileKQ = new KetQuaTranhChapDAL().GetFileByXuLyDonID(XuLyDonID).ToList().Where(x => x.IsBaoMat != true || x.CanBoUpID == CanBoID).ToList();
                        // lay thong tin quyet dinh giao xac minh
                        cgqList = new TheoDoiXuLyDAL().GetQuyetDinhGiaoXacMinh(XuLyDonID).ToList().Where(x => x.IsBaoMat != true || x.CanBoID == CanBoID).ToList();
                        // lay thong tin qua trinh xac minh
                        lsFileXacMinh = new TheoDoiXuLyDAL().GetQuaTrinhXacMinh(XuLyDonID).Where(x => x.IsBaoMat != true || x.CanBoID == CanBoID).ToList();
                        lsYKienGiaiQuyet = new XuLyDonDAL().GetYKienGiaiQuyet(XuLyDonID).Where(x => x.IsBaoMat != true || x.NguoiUp == CanBoID).ToList();
                        if (lsYKienGiaiQuyet != null)
                        {
                            foreach (var donThuInfo in lsYKienGiaiQuyet)
                            {
                                donThuInfo.NgayGiaiQuyetStr = Format.FormatDate(donThuInfo.NgayGiaiQuyet);
                            }
                        }
                        lsFilePheDuyetXacMinh = new XuLyDonDAL().GetPheDuyetBaoCaoXacMinh(XuLyDonID).Where(x => x.IsBaoMat != true || x.CanBoID == CanBoID).ToList();
                        //lay thong tin quyet dinh giai quyet don
                        kqInfo = new KetQuaDAL().GetCustomByXuLyDonID(XuLyDonID);
                        if (kqInfo != null)
                        {
                            kqInfo.lstFileKQ = kqInfo.lstFileKQ.Where(x => x.IsBaoMat != true || x.NguoiUp == CanBoID).ToList();
                            kqInfo.NgayRaKQStr = Format.FormatDate(kqInfo.NgayRaKQ ?? DateTime.Now);
                        }
                        //lay thong tin ket qua thi hanh
                        thiHanhInfo = new ThiHanhDAL().ThiHanh_KetQua_GetByID(XuLyDonID);
                        if (thiHanhInfo != null)
                        {
                            thiHanhInfo.lstFileTH = thiHanhInfo.lstFileTH.Where(x => x.IsBaoMat != true || x.CanBoID == CanBoID).ToList();
                            thiHanhInfo.NgayThiHanhStr = Format.FormatDate(thiHanhInfo.NgayThiHanh);
                        }
                    }

                    foreach (var item in listFileHoSo)
                    {
                        item.Type = 1;
                        item.CANBOTHEM = "Đã thêm bởi " + item.TenCanBo + " (" + item.TenCoQuanUp + ")";
                        item.NDFILE = "File hồ sơ";
                    }
                    listFile.AddRange(listFileHoSo);
                    foreach (var item in listFileHoSo_DonDoc)
                    {
                        item.Type = 2;
                        item.CANBOTHEM = "Đã thêm bởi " + item.TenCanBo + " (" + item.TenCoQuanUp + ")";
                        item.NDFILE = "File hồ sơ đôn đốc";
                    }
                    listFile.AddRange(listFileHoSo_DonDoc);
                    foreach (var item in rutdonInfo)
                    {
                        FileHoSoInfo file = new FileHoSoInfo();
                        file.Type = 3;
                        file.IsBaoMat = item.IsBaoMat;
                        file.FileHoSoID = item.FileRutDonID;
                        file.FileID = item.FileID;
                        file.NhomFileID = item.NhomFileID;
                        file.TenNhomFile = item.TenNhomFile;
                        file.ThuTuHienThiNhom = item.ThuTuHienThiNhom;
                        file.ThuTuHienThiFile = item.ThuTuHienThiFile;
                        file.FileURL = item.FileUrl;
                        file.TenFile = item.TenFile;
                        file.TenCanBo = item.TenCanBo;
                        file.TenCoQuanUp = item.TenCoQuanUp;
                        file.NguoiUp = item.NguoiUp;
                        file.NgayUps = item.NgayUps;
                        file.CANBOTHEM = "Đã thêm bởi " + item.TenCanBo + " (" + item.TenCoQuanUp + ")";
                        file.NDFILE = item.LyDo;
                        listFile.Add(file);
                    }
                    foreach (var item in lsFileYKienXuLy)
                    {
                        FileHoSoInfo file = new FileHoSoInfo();
                        file.Type = 4;
                        file.IsBaoMat = item.IsBaoMat;
                        file.FileHoSoID = item.FileYKienXuLyID;
                        file.FileID = item.FileID;
                        file.NhomFileID = item.NhomFileID;
                        file.TenNhomFile = item.TenNhomFile;
                        file.ThuTuHienThiNhom = item.ThuTuHienThiNhom;
                        file.ThuTuHienThiFile = item.ThuTuHienThiFile;
                        file.FileURL = item.FileUrl;
                        file.TenFile = item.TenFile;
                        file.TenCanBo = item.TenCanBoXuLy;
                        file.NguoiUp = item.NguoiUp;
                        file.TenCoQuanUp = item.TenCoQuanUp;
                        file.NgayUps = item.NgayUps;
                        file.CANBOTHEM = "Đã thêm bởi " + item.TenCanBoXuLy + " (" + item.TenCoQuanUp + ")";
                        file.NDFILE = "Nội dung: " + item.YKienXuLy;
                        if ((item.YKienXuLy == null || item.YKienXuLy == "") && item.LoaiFile == 6)
                        {
                            file.NDFILE = "File xử lý";
                        }
                        else if (item.LoaiFile == 2 && (item.YKienXuLy == null || item.YKienXuLy == ""))
                        {
                            file.NDFILE = "LĐ phê duyệt kết quả xử lý";
                        }
                        else if (item.LoaiFile == 0)
                        {
                            file.NDFILE = "File chuyển đơn";
                        }
                        else
                        {
                            file.NDFILE = "Nội dung: " + item.YKienXuLy;
                        }
                        listFile.Add(file);
                    }
                    foreach (var item in lsFilePhanXL)
                    {
                        item.Type = 5;
                        item.FileHoSoID = item.FilePhanXuLyID;
                        item.CANBOTHEM = "Đã thêm bởi " + item.TenCanBo + " (" + item.TenCoQuanUp + ")";
                        item.NDFILE = "File phân xử lý";
                    }
                    listFile.AddRange(lsFilePhanXL);
                    foreach (var item in lsFileKQ)
                    {
                        FileHoSoInfo file = new FileHoSoInfo();
                        file.Type = 6;
                        file.IsBaoMat = item.IsBaoMat;
                        file.FileHoSoID = item.KetQuaTranhChapID;
                        file.FileID = item.FileID;
                        file.NhomFileID = item.NhomFileID;
                        file.TenNhomFile = item.TenNhomFile;
                        file.ThuTuHienThiNhom = item.ThuTuHienThiNhom;
                        file.ThuTuHienThiFile = item.ThuTuHienThiFile;
                        file.FileURL = item.FileUrl;
                        file.TenFile = item.TenFile;
                        file.TenCanBo = item.TenCanBo;
                        file.TenCoQuanUp = "";
                        file.NguoiUp = item.CanBoUpID;
                        file.NgayUps = item.NgayCapNhat_Str;
                        file.CANBOTHEM = "Đã thêm bởi " + item.TenCanBo + " - " + item.NgayCapNhat_Str;
                        file.NDFILE = "File tranh chấp";
                        listFile.Add(file);
                    }
                    foreach (var item in cgqList)
                    {
                        FileHoSoInfo file = new FileHoSoInfo();
                        file.Type = 7;
                        file.IsBaoMat = item.IsBaoMat;
                        file.FileHoSoID = item.FileHoSoID;
                        file.FileID = item.FileID;
                        file.NhomFileID = item.NhomFileID;
                        file.TenNhomFile = item.TenNhomFile;
                        file.ThuTuHienThiNhom = item.ThuTuHienThiNhom;
                        file.ThuTuHienThiFile = item.ThuTuHienThiFile;
                        file.FileURL = item.FileUrl;
                        file.TenFile = item.TenFile;
                        file.TenCanBo = item.TenCanBo;
                        file.TenCoQuanUp = item.TenCoQuanPhan;
                        file.NguoiUp = item.CanBoID;
                        file.NgayUps = item.NgayChuyen_Str;
                        file.CANBOTHEM = "Đã thêm bởi " + item.TenCanBo + " (" + item.TenCoQuanPhan + ")";
                        if (item.GhiChu != "")
                        {
                            file.NDFILE = "Nội dung: " + item.GhiChu;
                        }
                        else
                        {
                            file.NDFILE = "File quyết định giao xác minh";
                        }
                        listFile.Add(file);
                    }
                    foreach (var item in lsFileXacMinh)
                    {
                        FileHoSoInfo file = new FileHoSoInfo();
                        file.Type = 8;
                        file.IsBaoMat = item.IsBaoMat;
                        file.FileHoSoID = item.FileGiaiQuyetID;
                        file.FileID = item.FileID;
                        file.NhomFileID = item.NhomFileID;
                        file.TenNhomFile = item.TenNhomFile;
                        file.ThuTuHienThiNhom = item.ThuTuHienThiNhom;
                        file.ThuTuHienThiFile = item.ThuTuHienThiFile;
                        file.FileURL = item.DuongDanFile;
                        file.TenFile = item.TenFile;
                        file.TenCanBo = item.TenCanBo;
                        file.TenCoQuanUp = item.TenCoQuanUp;
                        file.NguoiUp = item.CanBoID;
                        file.NgayUps = item.StringNgayCapNhat;
                        file.CANBOTHEM = "Đã thêm bởi " + item.TenCanBo + " (" + item.TenCoQuanUp + ")";
                        if (item.NoiDung != "" || item.NoiDung != null)
                        {
                            file.NDFILE = "Nội dung: " + item.NoiDung;
                        }
                        else
                        {
                            file.NDFILE = item.TenBuoc;
                        }
                        listFile.Add(file);
                    }
                    foreach (var item in lsYKienGiaiQuyet)
                    {
                        FileHoSoInfo file = new FileHoSoInfo();
                        file.Type = 9;
                        file.IsBaoMat = item.IsBaoMat;
                        file.FileHoSoID = item.FileBaoCaoXacMinhID;
                        file.FileID = item.FileID;
                        file.NhomFileID = item.NhomFileID;
                        file.TenNhomFile = item.TenNhomFile;
                        file.ThuTuHienThiNhom = item.ThuTuHienThiNhom;
                        file.ThuTuHienThiFile = item.ThuTuHienThiFile;
                        file.FileURL = item.FileUrl;
                        file.TenFile = item.TenFile;
                        file.TenCanBo = item.TenCanBo;
                        file.TenCoQuanUp = item.TenCoQuanUp;
                        file.NguoiUp = item.NguoiUp;
                        file.NgayUps = item.NgayGiaiQuyetStr;
                        file.CANBOTHEM = "Đã thêm bởi " + item.TenCanBo + " (" + item.TenCoQuanUp + ")";
                        if (item.YKienGiaiQuyet != "")
                        {
                            file.NDFILE = "Nội dung: " + item.YKienGiaiQuyet;
                        }
                        else
                        {
                            file.NDFILE = "Báo cáo kết quả xác minh";
                        }
                        listFile.Add(file);
                    }
                    foreach (var item in lsFilePheDuyetXacMinh)
                    {
                        FileHoSoInfo file = new FileHoSoInfo();
                        file.Type = 10;
                        file.IsBaoMat = item.IsBaoMat;
                        file.FileHoSoID = item.FileDonThuCanDuyetGiaiQuyetID;
                        file.FileID = item.FileID;
                        file.NhomFileID = item.NhomFileID;
                        file.TenNhomFile = item.TenNhomFile;
                        file.ThuTuHienThiNhom = item.ThuTuHienThiNhom;
                        file.ThuTuHienThiFile = item.ThuTuHienThiFile;
                        file.FileURL = item.DuongDanFile;
                        file.TenFile = item.TenFile;
                        file.TenCanBo = item.TenCapBoUp;
                        file.TenCoQuanUp = item.TenCoQuanUp;
                        file.NguoiUp = item.CanBoID;
                        file.NgayUps = item.StringNgayCapNhat;
                        file.CANBOTHEM = "Đã thêm bởi " + item.TenCapBoUp + " (" + item.TenCoQuanUp + ")";
                        file.NDFILE = "Duyệt báo cáo xác minh";
                        listFile.Add(file);
                    }
                    if (kqInfo != null && kqInfo.lstFileKQ != null)
                    {
                        foreach (var item in kqInfo.lstFileKQ)
                        {
                            item.Type = 11;
                            item.CANBOTHEM = "Đã thêm bởi " + item.TenCanBo + " (" + item.TenCoQuanUp + ")";
                            item.NDFILE = "File ban hành kết quả";
                        }
                        listFile.AddRange(kqInfo.lstFileKQ);
                    }

                    if (thiHanhInfo != null && thiHanhInfo.lstFileTH != null)
                    {
                        foreach (var item in thiHanhInfo.lstFileTH)
                        {
                            item.Type = 12;
                            item.CANBOTHEM = "Đã thêm bởi " + item.TenCanBo + " (" + item.TenCoQuanUp + ")";
                            item.NDFILE = "File thi hành kết luận";
                        }
                        listFile.AddRange(thiHanhInfo.lstFileTH);
                    }

                    foreach (var item in listFile)
                    {
                        if (item.TenNhomFile == null || item.TenNhomFile == "")
                        {
                            item.TenNhomFile = "File chưa phân loại";
                        }

                    }

                    List<FileHoSoInfo> nhomFile = new List<FileHoSoInfo>();
                    foreach (var item in listFile)
                    {
                        if (item.NhomFileID > 0)
                        {
                            Boolean check = true;
                            foreach (var nhom in nhomFile)
                            {
                                if (item.NhomFileID == nhom.NhomFileID)
                                {
                                    check = false;
                                    break;
                                }
                            }
                            if (check)
                            {
                                nhomFile.Add(item);
                            }
                        }

                    }
                    nhomFile = nhomFile.OrderBy(x => x.ThuTuHienThiNhom).ToList();

                    DonThuChiTiet.FileHoSo = listFile;
                }
                catch
                {
                    //throw;
                }
                #endregion

                try
                {
                    DTXuLyInfo DTXuLyInfo = new DTXuLy().GetByID(XuLyDonID);
                    KetQuaXacMinhModel KetQuaXacMinh = new KetQuaXacMinhModel();
                    if (DTXuLyInfo != null && DTXuLyInfo.SoDonThu != null)
                    {
                        KetQuaXacMinh.SoDonThu = DTXuLyInfo.SoDonThu;
                    }
                    if (DTXuLyInfo.NhomKNID > 0)
                    {
                        KetQuaXacMinh.NhomKN = new NhomKN().GetByID(DTXuLyInfo.NhomKNID);
                        if (KetQuaXacMinh.NhomKN != null)
                        {
                            KetQuaXacMinh.NhomKN.DanhSachDoiTuongKN = new DoiTuongKNDAL().GetCustomDataByNhomKNID(DTXuLyInfo.NhomKNID).ToList();
                        }
                    }
                    KetQuaXacMinh.BuocXacMinh = new BuocXacMinh().GetByLoaiKhieuToID(XuLyDonID).ToList();
                    int stt = KetQuaXacMinh.BuocXacMinh.Count;
                    var ListInfo = new TheoDoiXuLyDAL().GetQuaTrinhGiaiQuyet(XuLyDonID).ToList();
                    if (ListInfo != null)
                    {
                        //buoc xac minh trong danh muc buoc xm
                        foreach (var buocXacMinh in KetQuaXacMinh.BuocXacMinh)
                        {
                            buocXacMinh.TrangThai = 0;
                            foreach (var kqXM in ListInfo)
                            {
                                if (buocXacMinh.BuocXacMinhID == kqXM.BuocXacMinhID)
                                {
                                    buocXacMinh.TheoDoiXuLyID = kqXM.TheoDoiXuLyID;
                                    buocXacMinh.NgayCapNhat = kqXM.NgayCapNhat;
                                    buocXacMinh.GhiChu = kqXM.GhiChu;
                                    buocXacMinh.TenCanBo = kqXM.TenCanBo;
                                    buocXacMinh.TenCoQuan = kqXM.TenCoQuan;
                                    buocXacMinh.CanBoID = kqXM.CanBoID;
                                    if (kqXM.TheoDoiXuLyID > 0)
                                    {
                                        buocXacMinh.TrangThai = 1;
                                        buocXacMinh.TenTrangThai = "Đã cập nhật";
                                    }

                                }
                            }
                        }
                        //buoc xac minh ngoai danh muc
                        var BuocXacMinhEx = ListInfo.Where(x => x.BuocXacMinhID == 0).OrderBy(x => x.TheoDoiXuLyID).ToList();
                        if (BuocXacMinhEx != null && BuocXacMinhEx.Count > 0)
                        {
                            foreach (var kqXM in BuocXacMinhEx)
                            {
                                var buocXacMinh = new BuocXacMinhInfo();
                                buocXacMinh.TheoDoiXuLyID = kqXM.TheoDoiXuLyID;
                                buocXacMinh.TenBuoc = kqXM.TenBuoc;
                                buocXacMinh.NgayCapNhat = kqXM.NgayCapNhat;
                                buocXacMinh.GhiChu = kqXM.GhiChu;
                                buocXacMinh.TenCanBo = kqXM.TenCanBo;
                                buocXacMinh.TenCoQuan = kqXM.TenCoQuan;
                                buocXacMinh.CanBoID = kqXM.CanBoID;
                                if (kqXM.TheoDoiXuLyID > 0)
                                {
                                    buocXacMinh.TrangThai = 1;
                                    buocXacMinh.TenTrangThai = "Đã cập nhật";
                                }
                                buocXacMinh.OrderBy = stt++;

                                KetQuaXacMinh.BuocXacMinh.Add(buocXacMinh);
                            }
                        }

                        //file xm
                        try
                        {
                            foreach (var buocXacMinh in KetQuaXacMinh.BuocXacMinh)
                            {
                                buocXacMinh.DanhSachHoSoTaiLieu = new List<DanhSachHoSoTaiLieu>();
                                var fileDinhKem = new FileGiaiQuyet().GetFileGiaiQuyetByBuocXacMinhID(XuLyDonID, EnumLoaiFile.FileGiaiQuyet.GetHashCode(), buocXacMinh.BuocXacMinhID);
                                if (fileDinhKem.Count > 0)
                                {
                                    buocXacMinh.DanhSachHoSoTaiLieu = new List<DanhSachHoSoTaiLieu>();
                                    buocXacMinh.DanhSachHoSoTaiLieu = fileDinhKem.GroupBy(p => p.GroupUID)
                                       .Select(g => new DanhSachHoSoTaiLieu
                                       {
                                           GroupUID = g.Key,
                                           TenFile = g.FirstOrDefault().TenFile,
                                           NoiDung = g.FirstOrDefault().TomTat,
                                           TenNguoiCapNhat = g.FirstOrDefault().TenCanBo,
                                           NguoiCapNhatID = g.FirstOrDefault().NguoiUp,
                                           NgayCapNhat = g.FirstOrDefault().NgayUp,
                                           FileDinhKem = fileDinhKem.Where(x => x.GroupUID == g.Key && x.FileHoSoID > 0).GroupBy(x => x.FileHoSoID)
                                                           .Select(y => new FileDinhKemModel
                                                           {
                                                               FileID = y.FirstOrDefault().FileHoSoID,
                                                               TenFile = y.FirstOrDefault().TenFile,
                                                               NgayCapNhat = y.FirstOrDefault().NgayUp,
                                                               NguoiCapNhat = y.FirstOrDefault().NguoiUp,
                                                               //FileType = y.FirstOrDefault().FileType,
                                                               FileUrl = y.FirstOrDefault().FileURL,
                                                           }
                                                           ).ToList(),

                                       }
                                       ).ToList();
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }


                    GiaoXacMinhModel giaoXacMinhModel = new GiaoXacMinhModel();
                    ChuyenGiaiQuyetInfo ChuyenGiaiQuyet = new ChuyenGiaiQuyet().GetChuyenGiaiQuyetCoQuanKhac(XuLyDonID);
                    if (ChuyenGiaiQuyet != null)
                    {
                        giaoXacMinhModel.XuLyDonID = XuLyDonID;
                        giaoXacMinhModel.CoQuanID = ChuyenGiaiQuyet.CoQuanGiaiQuyetID;
                        giaoXacMinhModel.GhiChu = ChuyenGiaiQuyet.GhiChu;
                        giaoXacMinhModel.SoQuyetDinh = ChuyenGiaiQuyet.SoQuyetDinh;
                        giaoXacMinhModel.QuyetDinh = ChuyenGiaiQuyet.QuyetDinh;
                        giaoXacMinhModel.NgayQuyetDinh = ChuyenGiaiQuyet.NgayQuyetDinh;
                        giaoXacMinhModel.CQPhoiHopGQ = new ChuyenGiaiQuyet().GetCoQuanPhoiHop(XuLyDonID);
                        if (DonThuChiTiet.DonThu != null && DonThuChiTiet.DonThu.HanGiaiQuyetCu != DateTime.MinValue)
                        {
                            giaoXacMinhModel.HanGiaiQuyet = DonThuChiTiet.DonThu.HanGiaiQuyetCu;
                        }
                    }

                    KetQuaXacMinh.GiaoXacMinh = new GiaoXacMinhModel();
                    KetQuaXacMinh.GiaoXacMinh = giaoXacMinhModel;
                    KetQuaXacMinh.GiaoXacMinh.ToXacMinh = new VaiTroGiaiQuyet().GetByXuLyDonID(XuLyDonID);
                    try
                    {
                        var fileXacMinh = new TheoDoiXuLyDAL().GetQuyetDinhGiaoXacMinh(XuLyDonID).ToList();
                        if (fileXacMinh.Count > 0)
                        {
                            KetQuaXacMinh.GiaoXacMinh.DanhSachHoSoTaiLieu = new List<DanhSachHoSoTaiLieu>();
                            KetQuaXacMinh.GiaoXacMinh.DanhSachHoSoTaiLieu = fileXacMinh.GroupBy(p => p.GroupUID)
                               .Select(g => new DanhSachHoSoTaiLieu
                               {
                                   GroupUID = g.Key,
                                   TenFile = g.FirstOrDefault().TenFile,
                                   NoiDung = g.FirstOrDefault().GhiChu,
                                   TenNguoiCapNhat = g.FirstOrDefault().TenCanBo,
                                   NguoiCapNhatID = g.FirstOrDefault().CanBoID,
                                   NgayCapNhat = g.FirstOrDefault().NgayChuyen,
                                   FileDinhKem = fileXacMinh.Where(x => x.GroupUID == g.Key && x.FileHoSoID > 0).GroupBy(x => x.FileHoSoID)
                                                   .Select(y => new FileDinhKemModel
                                                   {
                                                       FileID = y.FirstOrDefault().FileHoSoID,
                                                       TenFile = y.FirstOrDefault().TenFile,
                                                       NgayCapNhat = y.FirstOrDefault().NgayChuyen,
                                                       NguoiCapNhat = y.FirstOrDefault().CanBoID,
                                                       //FileType = y.FirstOrDefault().FileType,
                                                       FileUrl = serverPath + y.FirstOrDefault().FileUrl,
                                                   }
                                                   ).ToList(),

                               }
                               ).ToList();
                        }
                    }
                    catch (Exception)
                    {
                    }


                    DonThuChiTiet.KetQuaXacMinh = KetQuaXacMinh;

                    DonThuChiTiet.KetQuaXuLy = new KetQuaXuLyModel();
                    DonThuChiTiet.KetQuaXuLy.HuongXuLy = DTXuLyInfo.HuongXuLy;
                    DonThuChiTiet.KetQuaXuLy.NoiDungXuLy = DTXuLyInfo.NoiDungHuongDan;
                    DonThuChiTiet.KetQuaXuLy.ChuyenChoCoQuan = DTXuLyInfo.ChuyenChoCoQuan;
                    if (DTXuLyInfo.HuongGiaiQuyetID > 0)
                    {
                        DonThuChiTiet.KetQuaXuLy.TrangThaiXuLy = "Đã xử lý";
                    }
                    else DonThuChiTiet.KetQuaXuLy.TrangThaiXuLy = "Chưa xử lý";

                    DonThuChiTiet.KetQuaXuLy.CoQuanXuLy = DonThuChiTiet.DonThu.TenCoQuanXL;
                    DonThuChiTiet.KetQuaXuLy.CanBoXuLy = DonThuChiTiet.DonThu.TenCanBoXuLy;
                    if (DonThuChiTiet.KetQuaXuLy.CanBoXuLy == null || DonThuChiTiet.KetQuaXuLy.CanBoXuLy == "")
                    {
                        DonThuChiTiet.KetQuaXuLy.CanBoXuLy = DonThuChiTiet.DonThu.TenCanBoTiepNhan;
                    }
                    if (DonThuChiTiet.DonThu.NgayXuLyDon != DateTime.MinValue)
                    {
                        DonThuChiTiet.KetQuaXuLy.NgayXuLy = DonThuChiTiet.DonThu.NgayXuLyDon;
                    }

                    try
                    {
                        var fileDinhKem = new XuLyDonDAL().GetFileYKienXuLy(XuLyDonID).Where(x => x.LoaiFile == EnumLoaiFile.FileKQXL.GetHashCode() || x.LoaiFile == EnumLoaiFile.FileYKienXuLy.GetHashCode()).ToList();
                        if (fileDinhKem.Count > 0)
                        {
                            DonThuChiTiet.KetQuaXuLy.DanhSachHoSoTaiLieu = new List<DanhSachHoSoTaiLieu>();
                            DonThuChiTiet.KetQuaXuLy.DanhSachHoSoTaiLieu = fileDinhKem.GroupBy(p => p.GroupUID)
                               .Select(g => new DanhSachHoSoTaiLieu
                               {
                                   GroupUID = g.Key,
                                   TenFile = g.FirstOrDefault().TenFile,
                                   NoiDung = g.FirstOrDefault().TomTat,
                                   TenNguoiCapNhat = g.FirstOrDefault().TenCanBoXuLy,
                                   NguoiCapNhatID = g.FirstOrDefault().NguoiUp,
                                   NgayCapNhat = g.FirstOrDefault().NgayUp,
                                   FileDinhKem = fileDinhKem.Where(x => x.GroupUID == g.Key && x.FileYKienXuLyID > 0).GroupBy(x => x.FileYKienXuLyID)
                                                   .Select(y => new FileDinhKemModel
                                                   {
                                                       FileID = y.FirstOrDefault().FileYKienXuLyID,
                                                       TenFile = y.FirstOrDefault().TenFile,
                                                       NgayCapNhat = y.FirstOrDefault().NgayUp,
                                                       NguoiCapNhat = y.FirstOrDefault().NguoiUp,
                                                       //FileType = y.FirstOrDefault().FileType,
                                                       FileUrl = serverPath + y.FirstOrDefault().FileUrl,
                                                   }
                                                   ).ToList(),

                               }
                               ).ToList();
                        }
                    }
                    catch (Exception)
                    {
                    }

                    try
                    {
                        DonThuChiTiet.ThongTinQuyetDinhGQ = new KetQuaDAL().QuyetDinh_GetBy_XuLyDonID(XuLyDonID);
                        DonThuChiTiet.ThongTinThiHanh = new KetQuaDAL().GetThiHanh_By_XuLyDonID(XuLyDonID);
                        if (DonThuChiTiet.ThongTinThiHanh != null && DonThuChiTiet.ThongTinThiHanh.ThiHanhID > 0)
                        {
                            var fileDinhKem = new ThiHanhDAL().GetFileThiHanhByThiHanhID(DonThuChiTiet.ThongTinThiHanh.ThiHanhID ?? 0, EnumLoaiFile.FileThiHanh.GetHashCode());
                            if (fileDinhKem.Count > 0)
                            {
                                DonThuChiTiet.ThongTinThiHanh.DanhSachHoSoTaiLieu = new List<DanhSachHoSoTaiLieu>();
                                DonThuChiTiet.ThongTinThiHanh.DanhSachHoSoTaiLieu = fileDinhKem.GroupBy(p => p.GroupUID)
                                   .Select(g => new DanhSachHoSoTaiLieu
                                   {
                                       GroupUID = g.Key,
                                       TenFile = g.FirstOrDefault().TenFile,
                                       NoiDung = g.FirstOrDefault().TomTat,
                                       TenNguoiCapNhat = g.FirstOrDefault().TenCanBo,
                                       NguoiCapNhatID = g.FirstOrDefault().NguoiUp,
                                       NgayCapNhat = g.FirstOrDefault().NgayUp,
                                       FileDinhKem = fileDinhKem.Where(x => x.GroupUID == g.Key && x.FileHoSoID > 0).GroupBy(x => x.FileHoSoID)
                                                       .Select(y => new FileDinhKemModel
                                                       {
                                                           FileID = y.FirstOrDefault().FileHoSoID,
                                                           TenFile = y.FirstOrDefault().TenFile,
                                                           NgayCapNhat = y.FirstOrDefault().NgayUp,
                                                           NguoiCapNhat = y.FirstOrDefault().NguoiUp,
                                                           FileUrl = serverPath + y.FirstOrDefault().FileURL,
                                                       }
                                                       ).ToList(),

                                   }
                                   ).ToList();
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                    //thong tin don doc
                    try
                    {
                        DonThuChiTiet.ThongTinDonDoc = new KetQuaDAL().GetCustomByXuLyDonID_DonDoc(XuLyDonID);
                        if (DonThuChiTiet.ThongTinDonDoc.Count > 0)
                        {
                            foreach (var item in DonThuChiTiet.ThongTinDonDoc)
                            {
                                var fileDinhKem = new KetQuaDAL().GetFileHoSoByDonDocID(item.DonDocID, (int)EnumLoaiFile.FileVBDonDoc);
                                if (fileDinhKem.Count > 0)
                                {
                                    item.DanhSachHoSoTaiLieu = new List<DanhSachHoSoTaiLieu>();
                                    item.DanhSachHoSoTaiLieu = fileDinhKem.GroupBy(p => p.GroupUID)
                                       .Select(g => new DanhSachHoSoTaiLieu
                                       {
                                           GroupUID = g.Key,
                                           TenFile = g.FirstOrDefault().TenFile,
                                           NoiDung = g.FirstOrDefault().TomTat,
                                           TenNguoiCapNhat = g.FirstOrDefault().TenCanBo,
                                           NguoiCapNhatID = g.FirstOrDefault().NguoiUp,
                                           NgayCapNhat = g.FirstOrDefault().NgayUp,
                                           FileDinhKem = fileDinhKem.Where(x => x.GroupUID == g.Key && x.FileHoSoID > 0).GroupBy(x => x.FileHoSoID)
                                                           .Select(y => new FileDinhKemModel
                                                           {
                                                               FileID = y.FirstOrDefault().FileHoSoID,
                                                               TenFile = y.FirstOrDefault().TenFile,
                                                               NgayCapNhat = y.FirstOrDefault().NgayUp,
                                                               NguoiCapNhat = y.FirstOrDefault().NguoiUp,
                                                               FileUrl = serverPath + y.FirstOrDefault().FileURL,
                                                           }
                                                           ).ToList(),

                                       }
                                       ).ToList();
                                }
                            }

                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }

                    DonThuChiTiet.TienTrinhXuLy = new List<TransitionHistoryInfo>();
                    try
                    {
                        var TienTrinhInfo = new TransitionHistoryDAL().GetLuongDonByID(XuLyDonID).ToList();
                        DonThuChiTiet.TienTrinhXuLy = GetTienTrinhXuLy(DTXuLyInfo.CoQuanID, DTXuLyInfo.HuongGiaiQuyetID, DTXuLyInfo.QuyTrinhXLD, DTXuLyInfo.QuyTrinhGQ);
                        if (DonThuChiTiet.TienTrinhXuLy != null && DonThuChiTiet.TienTrinhXuLy.Count > 0 && TienTrinhInfo != null && TienTrinhInfo.Count > 0)
                        {
                            foreach (var item in DonThuChiTiet.TienTrinhXuLy)
                            {
                                foreach (var data in TienTrinhInfo)
                                {
                                    if (item.StateID == data.StateID)
                                    {
                                        item.ThoiGianThucHien = data.ThoiGianThucHien;
                                        item.CanBoThucHien = data.CanBoThucHien;
                                        item.YKienCanBo = data.YKienCanBo;
                                        item.TenCoQuan = data.TenCoQuan;
                                        item.TrangThai = 1;
                                    }

                                    if (item.StateID == 11 && DonThuChiTiet.DonThu != null && DonThuChiTiet.DonThu.LanhDaoDuyet1ID == 0 && DonThuChiTiet.DonThu.LanhDaoDuyet2ID > 0)
                                    {
                                        //item.ThoiGianThucHien = DonThuChiTiet.DonThu.NgayBanHanh;
                                        item.CanBoThucHien = DonThuChiTiet.DonThu.TenCanBoTiepNhan;
                                        item.TenCoQuan = DonThuChiTiet.DonThu.TenCoQuanGQ;
                                        item.TrangThai = 1;
                                    }

                                    if (item.StateID == 999) //Ban hanh quyet dinh giao xac minh
                                    {
                                        if (DonThuChiTiet.DonThu != null && DonThuChiTiet.DonThu.TrinhDuThao >= TrangThaiTrinhDuThao.DaDuyetQDXacMinh.GetHashCode())
                                        {
                                            item.ThoiGianThucHien = DonThuChiTiet.DonThu.NgayBanHanh;
                                            item.CanBoThucHien = DonThuChiTiet.DonThu.TenCanBoBanHanh;
                                            item.TenCoQuan = DonThuChiTiet.DonThu.TenCoQuanBanHanh;
                                            item.TrangThai = 1;
                                        }
                                    }
                                    if (item.StateID == 1000) // Cap nhat quyet dinh giai quyet
                                    {
                                        if (DonThuChiTiet.ThongTinQuyetDinhGQ != null && DonThuChiTiet.ThongTinQuyetDinhGQ.CanBoID > 0)
                                        {
                                            item.ThoiGianThucHien = DonThuChiTiet.ThongTinQuyetDinhGQ.NgayQuyetDinh;
                                            item.CanBoThucHien = DonThuChiTiet.ThongTinQuyetDinhGQ.TenCanBo;
                                            item.TenCoQuan = DonThuChiTiet.ThongTinQuyetDinhGQ.TenCoQuan;
                                            item.TrangThai = 1;
                                        }
                                    }
                                    if (item.StateID == 1001) //Thi hanh
                                    {
                                        if (DonThuChiTiet.ThongTinThiHanh != null && DonThuChiTiet.ThongTinThiHanh.ThiHanhID > 0)
                                        {
                                            item.ThoiGianThucHien = DonThuChiTiet.ThongTinThiHanh.NgayThiHanh;
                                            item.CanBoThucHien = DonThuChiTiet.ThongTinThiHanh.TenCanBo;
                                            item.TenCoQuan = DonThuChiTiet.ThongTinThiHanh.TenCoQuan;
                                            item.TrangThai = 1;
                                        }
                                    }

                                }
                            }
                        }

                        //cap nhat Ket qua xu ly
                        foreach (var item in TienTrinhInfo)
                        {
                            if (item.StateID == 6)
                            {
                                DonThuChiTiet.KetQuaXuLy.LanhDaoPheDuyet = item.CanBoThucHien;
                            }
                            if (item.StateID == 11)
                            {
                                if (DonThuChiTiet.KetQuaXuLy.CanBoXuLy == null || DonThuChiTiet.KetQuaXuLy.CanBoXuLy == "")
                                    DonThuChiTiet.KetQuaXuLy.CanBoXuLy = item.CanBoThucHien;
                            }
                        }
                    }
                    catch
                    {
                    }

                }
                catch (Exception)
                {

                }

                Result.Status = 1;
                Result.Data = DonThuChiTiet;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public List<DanhMucFileInfo> DanhMucTenFile()
        {
            List<DanhMucFileInfo> listDMFile = new DanhMucFileDAL().File_Get_DanhSachDangSuDung();
            return listDMFile;
        }

        public List<LoaiKhieuToInfo> GetLoaiKhieuTos()
        {
            List<LoaiKhieuToInfo> loaiKhieuTos = new LoaiKhieuToDAL().GetLoaiKhieuTos().ToList();
            return loaiKhieuTos;
        }

        public List<TransitionHistoryInfo> GetTienTrinhXuLy(int CoQuanID, int HuongGiaiQuyetID, int QuyTrinhXLD, int QuyTrinhGQ)
        {
            List<TransitionHistoryInfo> TienTrinhXuLy = new List<TransitionHistoryInfo>();

            var cq = new CoQuan().GetCoQuanByID(CoQuanID);
            if (cq.CapID == CapQuanLy.CapUBNDTinh.GetHashCode())
            {
                if (QuyTrinhXLD == 1)
                {
                    TienTrinhXuLy.Add(new TransitionHistoryInfo("Xử lý đơn", null, null, null, "Chuyên viên tiếp nhận trình kết quả xử lý lên LĐ", null, 11, 0));
                    TienTrinhXuLy.Add(new TransitionHistoryInfo("Duyệt kết quả xử lý", null, null, null, "Duyệt kết quả xử lý LD", null, 6, 0));
                    TienTrinhXuLy.Add(new TransitionHistoryInfo("Ban hành quyết định giao xác minh", null, null, null, "Chủ tịch UBND ban hành quyết định giao xác minh", null, 999, 0));
                    TienTrinhXuLy.Add(new TransitionHistoryInfo("Cập nhật quyết định giao xác minh", null, null, null, "Lãnh đạo cơ quan cấp trên phân giải quyết cho cơ quan cấp dưới", null, 7, 0));
                    TienTrinhXuLy.Add(new TransitionHistoryInfo("Giao xác minh", null, null, null, "Lãnh đạo cấp dưới phân phó chánh thanh tra hoặc trưởng phòng giải quyết", null, 18, 0));
                    if (QuyTrinhGQ == 1)
                    {
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Lập đoàn (tổ) xác minh", null, null, null, "Phó chánh thanh tra hoặc trưởng phòng phân trưởng đoàn giải quyết", null, 19, 0));
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Trình báo cáo xác minh", null, null, null, "Trưởng đoàn trình kết quả giải quyết lên phó chánh thanh tra hoặc trưởng phòng", null, 8, 0));
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Duyệt báo cáo xác minh", null, null, null, "Phó chánh thanh tra hoặc trưởng phòng trình kết quả giải quyết lên lãnh đạo cấp dưới", null, 21, 0));
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Duyệt báo cáo xác minh", null, null, null, "Lãnh đạo cấp dưới trình kết quả giải quyết lên lãnh đạo cấp trên", null, 22, 0));
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Phê duyệt quyết định giải quyết", null, null, null, "Duyệt kết quả giải quyết", null, 9, 0));
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Cập nhật nội dung quyết định giải quyết", null, null, null, "Cập nhật nội dung quyết định giải quyết", null, 1000, 0));
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Theo dõi thi hành quyết định giải quyết", null, null, null, "Theo dõi thi hành quyết định giải quyết", null, 1001, 0));
                    }
                    else
                    {
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Cập nhật quyết định thành lập tổ/đoàn xác minh", null, null, null, "Lãnh đạo cấp dưới phân phó chánh thanh tra hoặc trưởng phòng giải quyết", null, 18, 0));
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Cập nhật báo cáo xác minh đã được phê duyệt", null, null, null, "Trưởng đoàn trình kết quả giải quyết lên phó chánh thanh tra hoặc trưởng phòng", null, 8, 0));
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Cập nhật nội dung quyết định giải quyết", null, null, null, "Cập nhật nội dung quyết định giải quyết", null, 1000, 0));
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Theo dõi thi hành quyết định giải quyết", null, null, null, "Theo dõi thi hành quyết định giải quyết", null, 1001, 0));
                    }
                }
                else
                {
                    TienTrinhXuLy.Add(new TransitionHistoryInfo("Xử lý đơn", null, null, null, "Chuyên viên tiếp nhận trình kết quả xử lý lên LĐ", null, 11, 0));
                    TienTrinhXuLy.Add(new TransitionHistoryInfo("Cập nhật quyết định giao xác minh", null, null, null, "Lãnh đạo cơ quan cấp trên phân giải quyết cho cơ quan cấp dưới", null, 7, 0));
                    TienTrinhXuLy.Add(new TransitionHistoryInfo("Cập nhật quyết định thành lập tổ/đoàn xác minh", null, null, null, "Lãnh đạo cấp dưới phân phó chánh thanh tra hoặc trưởng phòng giải quyết", null, 18, 0));
                    TienTrinhXuLy.Add(new TransitionHistoryInfo("Cập nhật báo cáo xác minh đã được phê duyệt", null, null, null, "Trưởng đoàn trình kết quả giải quyết lên phó chánh thanh tra hoặc trưởng phòng", null, 8, 0));
                    TienTrinhXuLy.Add(new TransitionHistoryInfo("Cập nhật nội dung quyết định giải quyết", null, null, null, "Cập nhật nội dung quyết định giải quyết", null, 1000, 0));
                    TienTrinhXuLy.Add(new TransitionHistoryInfo("Theo dõi thi hành quyết định giải quyết", null, null, null, "Theo dõi thi hành quyết định giải quyết", null, 1001, 0));
                }

                if (cq.BanTiepDan == false)
                {
                    if (QuyTrinhXLD == 1)
                    {
                        TienTrinhXuLy = new List<TransitionHistoryInfo>();
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Xử lý đơn", null, null, null, "Chuyên viên tiếp nhận trình kết quả xử lý lên LĐ", null, 11, 0));
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Duyệt kết quả xử lý", null, null, null, "Duyệt kết quả xử lý LD", null, 6, 0));
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Giao xác minh", null, null, null, "Lãnh đạo cấp dưới phân phó chánh thanh tra hoặc trưởng phòng giải quyết", null, 7, 0));
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Lập đoàn (tổ) xác minh", null, null, null, "Phó chánh thanh tra hoặc trưởng phòng phân trưởng đoàn giải quyết", null, 19, 0));
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Trình báo cáo xác minh", null, null, null, "Trưởng đoàn trình kết quả giải quyết lên phó chánh thanh tra hoặc trưởng phòng", null, 8, 0));
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Duyệt báo cáo xác minh", null, null, null, "Phó chánh thanh tra hoặc trưởng phòng trình kết quả giải quyết lên lãnh đạo cấp dưới", null, 21, 0));
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Phê duyệt quyết định giải quyết", null, null, null, "Duyệt kết quả giải quyết", null, 9, 0));
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Cập nhật nội dung quyết định giải quyết", null, null, null, "Cập nhật nội dung quyết định giải quyết", null, 1000, 0));
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Theo dõi thi hành quyết định giải quyết", null, null, null, "Theo dõi thi hành quyết định giải quyết", null, 1001, 0));
                    }
                    else
                    {
                        TienTrinhXuLy = new List<TransitionHistoryInfo>();
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Xử lý đơn", null, null, null, "Chuyên viên tiếp nhận trình kết quả xử lý lên LĐ", null, 11, 0));
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Cập nhật quyết định thành lập tổ/đoàn xác minh", null, null, null, "Phó chánh thanh tra hoặc trưởng phòng phân trưởng đoàn giải quyết", null, 7, 0));
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Cập nhật báo cáo xác minh đã được phê duyệt", null, null, null, "Trưởng đoàn trình kết quả giải quyết lên phó chánh thanh tra hoặc trưởng phòng", null, 8, 0));
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Cập nhật nội dung quyết định giải quyết", null, null, null, "Cập nhật nội dung quyết định giải quyết", null, 1000, 0));
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Theo dõi thi hành quyết định giải quyết", null, null, null, "Theo dõi thi hành quyết định giải quyết", null, 1001, 0));
                    }
                }

            }
            else if (cq.CapID == CapQuanLy.CapSoNganh.GetHashCode())
            {
                if (QuyTrinhXLD == 1)
                {
                    TienTrinhXuLy.Add(new TransitionHistoryInfo("Xử lý đơn", null, null, null, "Chuyên viên tiếp nhận trình kết quả xử lý lên LĐ", null, 11, 0));
                    TienTrinhXuLy.Add(new TransitionHistoryInfo("Duyệt kết quả xử lý", null, null, null, "Duyệt kết quả xử lý LD", null, 6, 0));
                    //TienTrinhXuLy.Add(new TransitionHistoryInfo("Giao xác minh", null, null, null, "Lãnh đạo cấp dưới phân phó chánh thanh tra hoặc trưởng phòng giải quyết", null, 18, 0));
                    TienTrinhXuLy.Add(new TransitionHistoryInfo("Lập đoàn (tổ) xác minh", null, null, null, "Phó chánh thanh tra hoặc trưởng phòng phân trưởng đoàn giải quyết", null, 19, 0));
                    TienTrinhXuLy.Add(new TransitionHistoryInfo("Trình báo cáo xác minh", null, null, null, "Trưởng đoàn trình kết quả giải quyết lên phó chánh thanh tra hoặc trưởng phòng", null, 8, 0));
                    TienTrinhXuLy.Add(new TransitionHistoryInfo("Duyệt báo cáo xác minh", null, null, null, "Phó chánh thanh tra hoặc trưởng phòng trình kết quả giải quyết lên lãnh đạo cấp dưới", null, 21, 0));
                    //TienTrinhXuLy.Add(new TransitionHistoryInfo("Duyệt báo cáo xác minh", null, null, null, "Lãnh đạo cấp dưới trình kết quả giải quyết lên lãnh đạo cấp trên", null, 22, 0));
                    TienTrinhXuLy.Add(new TransitionHistoryInfo("Phê duyệt quyết định giải quyết", null, null, null, "Duyệt kết quả giải quyết", null, 9, 0));
                    TienTrinhXuLy.Add(new TransitionHistoryInfo("Cập nhật nội dung quyết định giải quyết", null, null, null, "Cập nhật nội dung quyết định giải quyết", null, 1000, 0));
                    TienTrinhXuLy.Add(new TransitionHistoryInfo("Theo dõi thi hành quyết định giải quyết", null, null, null, "Theo dõi thi hành quyết định giải quyết", null, 1001, 0));
                }
                else
                {
                    TienTrinhXuLy.Add(new TransitionHistoryInfo("Xử lý đơn", null, null, null, "Chuyên viên tiếp nhận trình kết quả xử lý lên LĐ", null, 11, 0));
                    TienTrinhXuLy.Add(new TransitionHistoryInfo("Cập nhật quyết định thành lập tổ/đoàn xác minh", null, null, null, "Lãnh đạo cơ quan cấp trên phân giải quyết cho cơ quan cấp dưới", null, 7, 0));
                    //TienTrinhXuLy.Add(new TransitionHistoryInfo("Cập nhật quyết định thành lập tổ/đoàn xác minh", null, null, null, "Lãnh đạo cấp dưới phân phó chánh thanh tra hoặc trưởng phòng giải quyết", null, 18, 0));
                    TienTrinhXuLy.Add(new TransitionHistoryInfo("Cập nhật báo cáo xác minh đã được phê duyệt", null, null, null, "Trưởng đoàn trình kết quả giải quyết lên phó chánh thanh tra hoặc trưởng phòng", null, 8, 0));
                    TienTrinhXuLy.Add(new TransitionHistoryInfo("Cập nhật nội dung quyết định giải quyết", null, null, null, "Cập nhật nội dung quyết định giải quyết", null, 1000, 0));
                    TienTrinhXuLy.Add(new TransitionHistoryInfo("Theo dõi thi hành quyết định giải quyết", null, null, null, "Theo dõi thi hành quyết định giải quyết", null, 1001, 0));
                }
            }
            else if (cq.CapID == CapQuanLy.CapUBNDHuyen.GetHashCode() || cq.CapID == CapQuanLy.CapPhong.GetHashCode())
            {
                if (QuyTrinhXLD == 1)
                {
                    TienTrinhXuLy.Add(new TransitionHistoryInfo("Xử lý đơn", null, null, null, "Chuyên viên tiếp nhận trình kết quả xử lý lên LĐ", null, 11, 0));
                    TienTrinhXuLy.Add(new TransitionHistoryInfo("Duyệt kết quả xử lý", null, null, null, "Duyệt kết quả xử lý LD", null, 6, 0));
                    TienTrinhXuLy.Add(new TransitionHistoryInfo("Ban hành quyết định giao xác minh", null, null, null, "Chủ tịch UBND ban hành quyết định giao xác minh", null, 999, 0));
                    TienTrinhXuLy.Add(new TransitionHistoryInfo("Cập nhật quyết định giao xác minh", null, null, null, "Lãnh đạo cơ quan cấp trên phân giải quyết cho cơ quan cấp dưới", null, 7, 0));

                    if (QuyTrinhGQ == 1)
                    {
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Lập đoàn (tổ) xác minh", null, null, null, "Lãnh đạo cấp dưới phân trưởng đoàn giải quyết", null, 18, 0));
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Trình báo cáo xác minh", null, null, null, "Trưởng đoàn trình kết quả giải quyết lên lãnh đạo đơn vị", null, 8, 0));
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Duyệt báo cáo xác minh", null, null, null, "Lãnh đạo cấp dưới trình kết quả giải quyết lên lãnh đạo cấp trên", null, 22, 0));
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Phê duyệt quyết định giải quyết", null, null, null, "Duyệt kết quả giải quyết", null, 9, 0));
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Cập nhật nội dung quyết định giải quyết", null, null, null, "Cập nhật nội dung quyết định giải quyết", null, 1000, 0));
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Theo dõi thi hành quyết định giải quyết", null, null, null, "Theo dõi thi hành quyết định giải quyết", null, 1001, 0));
                    }
                    else
                    {
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Cập nhật quyết định thành lập tổ/đoàn xác minh", null, null, null, "Lãnh đạo cấp dưới phân phó chánh thanh tra hoặc trưởng phòng giải quyết", null, 18, 0));
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Cập nhật báo cáo xác minh đã được phê duyệt", null, null, null, "Trưởng đoàn trình kết quả giải quyết lên phó chánh thanh tra hoặc trưởng phòng", null, 8, 0));
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Cập nhật nội dung quyết định giải quyết", null, null, null, "Cập nhật nội dung quyết định giải quyết", null, 1000, 0));
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Theo dõi thi hành quyết định giải quyết", null, null, null, "Theo dõi thi hành quyết định giải quyết", null, 1001, 0));
                    }

                }
                else
                {
                    TienTrinhXuLy.Add(new TransitionHistoryInfo("Xử lý đơn", null, null, null, "Chuyên viên tiếp nhận trình kết quả xử lý lên LĐ", null, 11, 0));
                    //TienTrinhXuLy.Add(new TransitionHistoryInfo("Duyệt kết quả xử lý", null, null, null, "Duyệt kết quả xử lý LD", null, 6, 0));
                    //TienTrinhXuLy.Add(new TransitionHistoryInfo("Ban hành quyết định giao xác minh", null, null, null, "Chủ tịch UBND ban hành quyết định giao xác minh", null, 999, 0));
                    TienTrinhXuLy.Add(new TransitionHistoryInfo("Cập nhật văn bản giao xác minh", null, null, null, "Lãnh đạo cơ quan cấp trên phân giải quyết cho cơ quan cấp dưới", null, 7, 0));
                    TienTrinhXuLy.Add(new TransitionHistoryInfo("Cập nhật quyết định thành lập tổ/đoàn xác minh", null, null, null, "Lãnh đạo cấp dưới phân phó chánh thanh tra hoặc trưởng phòng giải quyết", null, 18, 0));
                    TienTrinhXuLy.Add(new TransitionHistoryInfo("Cập nhật báo cáo xác minh đã được phê duyệt", null, null, null, "Trưởng đoàn trình kết quả giải quyết lên phó chánh thanh tra hoặc trưởng phòng", null, 8, 0));
                    //TienTrinhXuLy.Add(new TransitionHistoryInfo("Duyệt báo cáo xác minh", null, null, null, "Lãnh đạo cấp dưới trình kết quả giải quyết lên lãnh đạo cấp trên", null, 22, 0));
                    //TienTrinhXuLy.Add(new TransitionHistoryInfo("Phê duyệt quyết định giải quyết", null, null, null, "Duyệt kết quả giải quyết", null, 9, 0));
                    TienTrinhXuLy.Add(new TransitionHistoryInfo("Cập nhật nội dung quyết định giải quyết", null, null, null, "Cập nhật nội dung quyết định giải quyết", null, 1000, 0));
                    TienTrinhXuLy.Add(new TransitionHistoryInfo("Theo dõi thi hành quyết định giải quyết", null, null, null, "Theo dõi thi hành quyết định giải quyết", null, 1001, 0));
                }

                if (cq.BanTiepDan == false)
                {
                    if (QuyTrinhXLD == 1)
                    {
                        TienTrinhXuLy = new List<TransitionHistoryInfo>();
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Xử lý đơn", null, null, null, "Chuyên viên tiếp nhận trình kết quả xử lý lên LĐ", null, 11, 0));
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Duyệt kết quả xử lý", null, null, null, "Duyệt kết quả xử lý LD", null, 6, 0));
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Lập đoàn (tổ) xác minh", null, null, null, "Lãnh đạo cấp dưới phân phó chánh thanh tra hoặc trưởng phòng giải quyết", null, 7, 0));
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Trình báo cáo xác minh", null, null, null, "Trưởng đoàn trình kết quả giải quyết lên phó chánh thanh tra hoặc trưởng phòng", null, 8, 0));
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Duyệt báo cáo xác minh", null, null, null, "Lãnh đạo cấp dưới trình kết quả giải quyết lên lãnh đạo cấp trên", null, 9, 0));
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Cập nhật nội dung quyết định giải quyết", null, null, null, "Cập nhật nội dung quyết định giải quyết", null, 1000, 0));
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Theo dõi thi hành quyết định giải quyết", null, null, null, "Theo dõi thi hành quyết định giải quyết", null, 1001, 0));
                    }
                    else
                    {
                        TienTrinhXuLy = new List<TransitionHistoryInfo>();
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Xử lý đơn", null, null, null, "Chuyên viên tiếp nhận trình kết quả xử lý lên LĐ", null, 11, 0));
                        //TienTrinhXuLy.Add(new TransitionHistoryInfo("Duyệt kết quả xử lý", null, null, null, "Duyệt kết quả xử lý LD", null, 6, 0));
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Cập nhật quyết định thành lập tổ/đoàn xác minh", null, null, null, "Lãnh đạo cấp dưới phân phó chánh thanh tra hoặc trưởng phòng giải quyết", null, 7, 0));
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Cập nhật báo cáo xác minh đã được phê duyệt", null, null, null, "Trưởng đoàn trình kết quả giải quyết lên phó chánh thanh tra hoặc trưởng phòng", null, 8, 0));
                        //TienTrinhXuLy.Add(new TransitionHistoryInfo("Duyệt báo cáo xác minh", null, null, null, "Lãnh đạo cấp dưới trình kết quả giải quyết lên lãnh đạo cấp trên", null, 9, 0));
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Cập nhật nội dung quyết định giải quyết", null, null, null, "Cập nhật nội dung quyết định giải quyết", null, 1000, 0));
                        TienTrinhXuLy.Add(new TransitionHistoryInfo("Theo dõi thi hành quyết định giải quyết", null, null, null, "Theo dõi thi hành quyết định giải quyết", null, 1001, 0));
                    }
                }

            }
            else if (cq.CapID == CapQuanLy.CapUBNDXa.GetHashCode())
            {
                if (QuyTrinhXLD == 1)
                {
                    TienTrinhXuLy.Add(new TransitionHistoryInfo("Xử lý đơn", null, null, null, "Chuyên viên tiếp nhận trình kết quả xử lý lên LĐ", null, 11, 0));
                    //TienTrinhXuLy.Add(new TransitionHistoryInfo("Duyệt kết quả xử lý", null, null, null, "Duyệt kết quả xử lý LD", null, 6, 0));
                    TienTrinhXuLy.Add(new TransitionHistoryInfo("Giao xác minh", null, null, null, "Lãnh đạo cơ quan cấp trên phân giải quyết cho cơ quan cấp dưới", null, 7, 0));
                    TienTrinhXuLy.Add(new TransitionHistoryInfo("Trình báo cáo xác minh", null, null, null, "Trưởng đoàn trình kết quả giải quyết lên phó chánh thanh tra hoặc trưởng phòng", null, 8, 0));
                    TienTrinhXuLy.Add(new TransitionHistoryInfo("Phê duyệt quyết định giải quyết", null, null, null, "Duyệt kết quả giải quyết", null, 9, 0));
                    TienTrinhXuLy.Add(new TransitionHistoryInfo("Cập nhật nội dung quyết định giải quyết", null, null, null, "Cập nhật nội dung quyết định giải quyết", null, 1000, 0));
                    TienTrinhXuLy.Add(new TransitionHistoryInfo("Theo dõi thi hành quyết định giải quyết", null, null, null, "Theo dõi thi hành quyết định giải quyết", null, 1001, 0));
                }
                else
                {
                    TienTrinhXuLy = new List<TransitionHistoryInfo>();
                    TienTrinhXuLy.Add(new TransitionHistoryInfo("Xử lý đơn", null, null, null, "Chuyên viên tiếp nhận trình kết quả xử lý lên LĐ", null, 11, 0));
                    TienTrinhXuLy.Add(new TransitionHistoryInfo("Cập nhật quyết định giao xác minh", null, null, null, "Lãnh đạo cơ quan cấp trên phân giải quyết cho cơ quan cấp dưới", null, 7, 0));
                    TienTrinhXuLy.Add(new TransitionHistoryInfo("Cập nhật báo cáo xác minh", null, null, null, "Trưởng đoàn trình kết quả giải quyết lên phó chánh thanh tra hoặc trưởng phòng", null, 8, 0));
                    TienTrinhXuLy.Add(new TransitionHistoryInfo("Cập nhật nội dung quyết định giải quyết", null, null, null, "Cập nhật nội dung quyết định giải quyết", null, 1000, 0));
                    TienTrinhXuLy.Add(new TransitionHistoryInfo("Theo dõi thi hành quyết định giải quyết", null, null, null, "Theo dõi thi hành quyết định giải quyết", null, 1001, 0));
                }
            }


            return TienTrinhXuLy;
        }
    }
}
