using Com.Gosol.KNTC.DAL.HeThong;
using Com.Gosol.KNTC.DAL.KNTC;
using Com.Gosol.KNTC.Model.HeThong;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.HeThong;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Ultilities;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Vml;
using Microsoft.Office.Interop.Word;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Workflow;
using Utils = Com.Gosol.KNTC.Ultilities.Utils;

namespace Com.Gosol.KNTC.BUS.KNTC
{
    public class PheDuyetKetQuaXuLyBUS
    {
        public IList<DTXuLyInfo> GetBySearch(ref int TotalRow, XuLyDonParamsForFilter p, IdentityHelper IdentityHelper)
        {
            QueryFilterInfo info = new QueryFilterInfo();
            //if(IdentityHelper.ChuTichUBND == 1 && IdentityHelper.CapID == EnumCapCoQuan.CapTinh.GetHashCode())
            //{
            //    IdentityHelper.CoQuanID = EnumCoQuan.BanTCDTinh.GetHashCode();
            //}
            info.CoQuanID = IdentityHelper.CoQuanID ?? 0;
            info.CanBoID = IdentityHelper.CanBoID ?? 0;
            info.KeyWord = p.Keyword;
            int _currentPage = p.PageNumber;
            info.Start = (_currentPage - 1) * p.PageSize;
            info.End = _currentPage * p.PageSize;
            info.LoaiKhieuToID = p.LoaiKhieuToID ?? 0;
            info.TrangThai = p.TrangThaiID;
            info.ChuTichUBND = IdentityHelper.ChuTichUBND;
            info.CanBoID = IdentityHelper.CanBoID ?? 0;
            info.HuongXuLyID = p.HuongXuLyID ?? 0;
            info.TuNgay = p.TuNgay ?? DateTime.MinValue;
            info.DenNgay = p.DenNgay ?? DateTime.MinValue;

            List<int> docIDList = new List<int>();
            IList<DTXuLyInfo> result = new List<DTXuLyInfo>();
            if (IdentityHelper.RoleID == RoleEnum.LanhDao.GetHashCode())
            {
                try
                {

                    //if (info.KeyWord == "" && info.LoaiKhieuToID == 0 && info.TuNgay == DateTime.MinValue && info.DenNgay == DateTime.MinValue)
                    //{
                    //    result = new DTXuLy().DTDuyetKQXL_LanhDao_GetAllByCoQuanID(ref TotalRow, info, docIDList);
                    //}
                    //else
                    //{
                    //    result = new DTXuLy().DTDuyetKQXL_LanhDao(ref TotalRow, info, docIDList);
                    //}

                    if (IdentityHelper.CapID == CapQuanLy.CapUBNDXa.GetHashCode())
                    {
                        result = new DTXuLy().DTDuyetKQXL_LanhDao_Xa(ref TotalRow, info, docIDList);
                    }
                    else result = new DTXuLy().DTDuyetKQXL_LanhDao(ref TotalRow, info, docIDList);
                }
                catch
                {
                }
            }

            if (IdentityHelper.RoleID == RoleEnum.LanhDaoPhong.GetHashCode())
            {
                info.PhongBanID = IdentityHelper.PhongBanID ?? 0;

                try
                {
                    if (info.KeyWord == "" && info.LoaiKhieuToID == 0 && info.TuNgay == DateTime.MinValue && info.DenNgay == DateTime.MinValue)
                    {
                        result = new DTXuLy().DTDuyetKQXL_TruongPhong_GetAllByCoQuanID(info, docIDList);
                    }
                    else
                    {
                        result = new DTXuLy().DTDuyetKQXL_TruongPhong(info, docIDList);
                    }
                }
                catch
                {
                }
            }

            //List<DoiTuongKNInfo> listDoiTuongKN = new DoiTuongKN().GetDiaChiDTKhieuNaiByNhomKN(result.ToList());
            //foreach (DTXuLyInfo dt in result)
            //{
            //    dt.NgayGuis = Format.FormatDate(dt.NgayQuaHan);

            //    string now = Format.FormatDate(DateTime.Now);
            //    DateTime currentDate = Utils.ConvertToDateTime(now, DateTime.MinValue);
            //    TimeSpan ngayConLai = dt.NgayQuaHan.Subtract(currentDate);
            //    var soNgayConLai = ngayConLai.Days;
            //    dt.NgayGQConLai = soNgayConLai;

            //    List<DoiTuongKNInfo> ltInfo = new List<DoiTuongKNInfo>();
            //    foreach (DoiTuongKNInfo doiTuong in listDoiTuongKN)
            //    {
            //        if (doiTuong.NhomKNID == dt.NhomKNID)
            //        {
            //            ltInfo.Add(doiTuong);
            //        }
            //    }

            //    int count = 0;
            //    String hoTen = "";
            //    foreach (var diaChiInfo in ltInfo)
            //    {
            //        hoTen += diaChiInfo.HoTen + "(" + diaChiInfo.DiaChiCT + ")";
            //        count++;
            //        if (count >= ltInfo.Count())
            //            break;
            //        else
            //            hoTen += ", <br/>";
            //    }
            //    if (ltInfo.Count > 0) dt.HoTenStr = hoTen;
            //}

            if (result.Count > 0)
            {
                foreach (var item in result)
                {
                    var renderTrangThai = new RenderTrangThai();
                    renderTrangThai.GetTrangThai(
                        item.LoaiQuyTrinh,
                        item.HuongGiaiQuyetID,
                        item.StateName,
                        item.StateID,
                        0,
                        item.TrangThaiDuyet ?? 0,
                        item.TrinhDuThao,
                        IdentityHelper,
                        item.NgayCapNhat,
                        item.ChuyenGiaiQuyetID,
                        item.KetQuaID,
                        item.LanhDaoDuyet2ID,
                        item.RutDonID
                        );
                    item.TrangThaiMoi = renderTrangThai.TrangThaiMoi;
                    item.TrangThaiIDMoi = renderTrangThai.TrangThaiIDMoi;
                    item.CheckTrangThai = renderTrangThai.CheckTrangThai;

                    var stateID = item.StateID;
                    if (IdentityHelper.RoleID == RoleEnum.LanhDao.GetHashCode())
                    {
                        if (stateID > 6)
                        {
                            item.TrangThai = "Đã duyệt";
                            item.TrangThaiID = 1;
                            if (item.TrinhDuThao >= 1 || item.LanhDaoDuyet2ID > 0)
                            {
                                item.TrangThai = "Đã trình";
                                item.TrangThaiID = 3;

                                if (item.LanhDaoDuyet2ID > 0 && item.LanhDaoDuyet2ID == IdentityHelper.CanBoID)
                                {
                                    if (item.TrangThaiDuyet == 0)
                                    {
                                        item.TrangThai = "Chưa duyệt";
                                        item.TrangThaiID = 0;
                                    }
                                    else if (item.TrangThaiDuyet == 1)
                                    {
                                        item.TrangThai = "Đã duyệt";
                                        item.TrangThaiID = 1;
                                    }
                                    else
                                    {
                                        item.TrangThai = "Xử lý lại";
                                        item.TrangThaiID = 2;
                                    }
                                }
                            }

                            if (IdentityHelper.ChuTichUBND == 1)
                            {
                                if (item.TrinhDuThao > 1)
                                {
                                    item.TrangThai = "Đang xác minh";
                                    item.TrangThaiID = 4;
                                }
                                else
                                {
                                    if (IdentityHelper.CapHanhChinh == EnumCapHanhChinh.CapUBNDHuyen.GetHashCode() && IdentityHelper.ChuTichUBND == 1)
                                    {
                                        if (item.TrinhDuThao == 0)
                                        {
                                            // bổ sung btn cấp huyện trình lên ct tỉnh 
                                            // trạng thái = 0 chưa duyệt cần duyệt update lên 1 để duyệt rồi xác minh
                                            item.TrangThai = "Chưa duyệt";
                                            item.TrangThaiID = 0;
                                        }
                                        else if (item.TrinhDuThao == 1)
                                        {
                                            // bổ sung btn cấp huyện trình lên ct tỉnh 
                                            // trạng thái = 0 chưa duyệt cần duyệt update lên 1 để duyệt rồi xác minh
                                            item.TrangThai = "Đã duyệt";
                                            item.TrangThaiID = 1;
                                        }
                                    }
                                    else
                                    {
                                        item.TrangThai = "Chưa ban hành quyết định giao xác minh";
                                        item.TrangThaiID = 3;
                                    }
                                }
                            }

                            if (stateID == 11 && item.HuongGiaiQuyetID > 0)
                            {
                                item.TrangThai = "Xử lý lại";
                                item.TrangThaiID = 2;
                            }
                        }
                        else if (stateID < 6 && stateID != 1)
                        {
                            item.TrangThai = "Xử lý lại";
                            item.TrangThaiID = 2;
                        }
                        else
                        {
                            item.TrangThai = "Chưa duyệt";
                            item.TrangThaiID = 0;
                            if (item.NgayGQConLai < 5)
                            {
                                item.TrangThaiQuaHan = 1;
                            }
                            else
                            {
                                item.TrangThaiQuaHan = 0;
                            }
                        }
                    }
                    if (IdentityHelper.RoleID == RoleEnum.LanhDaoPhong.GetHashCode())
                    {
                        if (stateID > 5)
                        {
                            item.TrangThai = "Đã duyệt";
                            item.TrangThaiID = 1;
                            if (item.TrinhDuThao > 1 || item.LanhDaoDuyet2ID > 0)
                            {
                                item.TrangThai = "Đã trình";
                                item.TrangThaiID = 3;
                            }
                        }
                        else if (stateID < 5)
                        {
                            item.TrangThai = "Xử lý lại";
                            item.TrangThaiID = 2;
                        }
                        else
                        {
                            item.TrangThai = "Chưa duyệt";
                            item.TrangThaiID = 0;
                            if (item.NgayGQConLai < 5)
                            {
                                item.TrangThaiQuaHan = 1;
                            }
                            else
                            {
                                item.TrangThaiQuaHan = 0;
                            }
                        }
                    }
                }
            }
            return result;
        }
        public DTXuLyInfo GetByID(int XuLyDonID)
        {
            DTXuLyInfo DTXuLyInfo = new DTXuLy().GetByID(XuLyDonID);

            var fileDinhKem = new XuLyDonDAL().GetFileYKienXuLy(XuLyDonID).Where(x => x.LoaiFile == EnumLoaiFile.FileKQXL.GetHashCode()).ToList();
            if (fileDinhKem.Count > 0)
            {
                DTXuLyInfo.DanhSachHoSoTaiLieu = new List<DanhSachHoSoTaiLieu>();
                DTXuLyInfo.DanhSachHoSoTaiLieu = fileDinhKem.GroupBy(p => p.GroupUID)
                   .Select(g => new DanhSachHoSoTaiLieu
                   {
                       GroupUID = g.Key,
                       TenFile = g.FirstOrDefault().TenFile,
                       NoiDung = g.FirstOrDefault().TomTat,
                       TenNguoiCapNhat = g.FirstOrDefault().TenCanBo,
                       NguoiCapNhatID = g.FirstOrDefault().NguoiUp,
                       NgayCapNhat = g.FirstOrDefault().NgayUp,
                       FileDinhKem = fileDinhKem.Where(x => x.GroupUID == g.Key && x.FileID > 0).GroupBy(x => x.FileID)
                                       .Select(y => new FileDinhKemModel
                                       {
                                           FileID = y.FirstOrDefault().FileID,
                                           TenFile = y.FirstOrDefault().TenFile,
                                           NgayCapNhat = y.FirstOrDefault().NgayUp,
                                           NguoiCapNhat = y.FirstOrDefault().NguoiUp,
                                           //FileType = y.FirstOrDefault().FileType,
                                           FileUrl = y.FirstOrDefault().FileUrl,
                                       }
                                       ).ToList(),

                   }
                   ).ToList();
            }
            var fileDinhKemDuThao = new XuLyDonDAL().GetFileYKienXuLy(XuLyDonID).Where(x => x.LoaiFile == EnumLoaiFile.FileTrinhDuThao.GetHashCode()).ToList();
            if (fileDinhKemDuThao.Count > 0)
            {
                DTXuLyInfo.DanhSachHoSoTaiLieuTrinhDuThao = new List<DanhSachHoSoTaiLieu>();
                DTXuLyInfo.DanhSachHoSoTaiLieuTrinhDuThao = fileDinhKemDuThao.GroupBy(p => p.GroupUID)
                   .Select(g => new DanhSachHoSoTaiLieu
                   {
                       GroupUID = g.Key,
                       TenFile = g.FirstOrDefault().TenFile,
                       NoiDung = g.FirstOrDefault().TomTat,
                       TenNguoiCapNhat = g.FirstOrDefault().TenCanBo,
                       NguoiCapNhatID = g.FirstOrDefault().NguoiUp,
                       NgayCapNhat = g.FirstOrDefault().NgayUp,
                       FileDinhKem = fileDinhKemDuThao.Where(x => x.GroupUID == g.Key && x.FileID > 0).GroupBy(x => x.FileID)
                                       .Select(y => new FileDinhKemModel
                                       {
                                           FileID = y.FirstOrDefault().FileID,
                                           TenFile = y.FirstOrDefault().TenFile,
                                           NgayCapNhat = y.FirstOrDefault().NgayUp,
                                           NguoiCapNhat = y.FirstOrDefault().NguoiUp,
                                           //FileType = y.FirstOrDefault().FileType,
                                           FileUrl = y.FirstOrDefault().FileUrl,
                                       }
                                       ).ToList(),

                   }
                   ).ToList();
            }
            return DTXuLyInfo;
        }

        public string GetPrevStateName(int XuLyDonID)
        {
            string StateName = WorkflowInstance.Instance.GetPrevStateOfDocument(XuLyDonID);
            return StateName;
        }
        public BaseResultModel DuyetXuLy(IdentityHelper IdentityHelper, DuyetXuLyModel DuyetXuLyModel)
        {
            var Result = new BaseResultModel();
            if (DuyetXuLyModel.XuLyDonID < 0)
            {
                Result.Status = 0;
                Result.Message = "Chưa chọn đơn thư cần phê duyệt";
                return Result;
            }
            string PrevState = GetPrevStateName(DuyetXuLyModel.XuLyDonID ?? 0);
            int CHUYENDON_GUIVBDONDOC = 6;

            int idxulydon = DuyetXuLyModel.XuLyDonID ?? 0;
            int CheckChucVu = IdentityHelper.RoleID ?? 0;

            bool kq = true;
            bool showThongBao = true;

            //duyet cap 2
            if (DuyetXuLyModel.LanhDaoDuyet2ID == IdentityHelper.CanBoID)
            {
                if (DuyetXuLyModel.TrangThaiPheDuyet == 1)//Dong y
                {
                    new XuLyDonDAL().UpdateTrangThaiDuyet(idxulydon, DuyetXuLyModel.TrangThaiPheDuyet);
                    if (IdentityHelper.CapHanhChinh == EnumCapHanhChinh.CapUBNDHuyen.GetHashCode() && IdentityHelper.ChuTichUBND == 1)
                    {
                        int val = new XuLyDonDAL().UpdateTrangThaiTrinhDuThao(DuyetXuLyModel.XuLyDonID, TrangThaiTrinhDuThao.DaTrinh.GetHashCode());
                    }

                    Result.Status = 1;
                    Result.Message = Constant.CONTENT_PHEDUYET_SUCCESS;
                }
                else //xu ly lai
                {
                    //new XuLyDonDAL().UpdateTrangThaiDuyet(idxulydon, DuyetXuLyModel.TrangThaiPheDuyet);

                    try
                    {
                        //PhanXuLyInfo info = new PhanXuLyInfo();
                        //info.GhiChu = DuyetXuLyModel.NoiDungPheDuyet;
                        //info.PhongBanID = IdentityHelper.PhongBanID ?? 0;
                        //info.CanBoID = IdentityHelper.CanBoID ?? 0;
                        //info.XuLyDonID = idxulydon;
                        //info.NgayPhan = DateTime.Now;

                        //new PhanXuLy().Insert(info);
                        //new XuLyDonDAL().UpdateCanBoXuLy(info.XuLyDonID, info.CanBoID);


                        PhanXuLyInfo info = new PhanXuLyInfo();
                        info.GhiChu = DuyetXuLyModel.NoiDungPheDuyet;
                        info.PhongBanID = IdentityHelper.PhongBanID ?? 0;
                        info.XuLyDonID = idxulydon;
                        info.NgayPhan = DateTime.Now;
                        new PhanXuLy().Insert(info);
                        //update state
                        DateTime hanxuly = Utils.ConvertToDateTime(DuyetXuLyModel.NgayHetHan, DateTime.Now);
                        new XuLyDonDAL().UpdateDocument(info.XuLyDonID, 11, hanxuly);
                    }
                    catch
                    {

                    }

                    Result.Status = 1;
                    Result.Message = Constant.CONTENT_PHEDUYET_SUCCESS;
                }
            }
            else
            {
                if (CheckChucVu == (int)EnumChucVu.LanhDao)
                {
                    #region role lanh dao
                    if (DuyetXuLyModel.TrangThaiPheDuyet == 1)
                    {
                        #region phe duyet
                        bool CheckIsDeXuatThuLy = new XuLyDonDAL().CheckIsHuongGiaiQuyet(idxulydon, Constant.DeXuatThuLy);
                        string commandCode = "";
                        if (CheckIsDeXuatThuLy == true)
                        {
                            #region de xuat thu ly
                            commandCode = WorkflowInstance.Instance.GetAvailabelCommands(idxulydon)[0];
                            DateTime NgayThuLy = DateTime.Now;
                            try
                            {
                                new XuLyDonDAL().UpdateNgayThuLy(idxulydon, NgayThuLy);

                            }
                            catch
                            {

                            }
                            int canboid = IdentityHelper.CanBoID ?? 0;
                            DocumentInfo dCInfo = new DonThuDAL().GetDocumentByID(idxulydon);

                            kq = WorkflowInstance.Instance.ExecuteCommand(idxulydon, canboid, commandCode, DateTime.Now.AddDays(45), DuyetXuLyModel.NoiDungPheDuyet ?? string.Empty);

                            #endregion
                        }
                        else
                        {
                            #region -- khong de xuat thu ly
                            TiepDanInfo info = new TiepDanInfo();
                            try
                            {
                                info = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().GetXuLyDonByXLDID(idxulydon);
                            }
                            catch
                            { }

                            int canboid = IdentityHelper.CanBoID ?? 0;
                            int chuyenDon = (int)HuongGiaiQuyetEnum.ChuyenDon;
                            int vBDonDoc = (int)HuongGiaiQuyetEnum.RaVanBanDonDoc;
                            int cVChiDao = (int)HuongGiaiQuyetEnum.CongVanChiDao;
                            bool suDungQTVanThuTiepDan = IdentityHelper.SuDungQTVanThuTiepDan ?? false;

                            Boolean laBanTCDTinh = false;
                            try
                            {
                                var listBanTCDTinh = new SystemConfigDAL().GetByKey("Ban_Tiep_Dan_ID").ConfigValue.Split(',').ToList().Select(x => Utils.ConvertToInt32(x.ToString(), 0)).ToList();
                                if (listBanTCDTinh.Contains(IdentityHelper.CoQuanID ?? 0))
                                {
                                    laBanTCDTinh = true;
                                }
                            }
                            catch (Exception)
                            {
                                if (IdentityHelper.CoQuanID == (int)EnumCoQuan.BanTCDTinh)
                                {
                                    laBanTCDTinh = true;
                                }
                            }


                            if (info.HuongGiaiQuyetID == chuyenDon)
                            {
                                #region chuyen don giống de xuat thu ly để trình lãnh đạo cấp trên chuyển
                                commandCode = WorkflowInstance.Instance.GetAvailabelCommands(idxulydon)[0];
                                DateTime NgayThuLy = DateTime.Now;
                                try
                                {
                                    new XuLyDonDAL().UpdateNgayThuLy(idxulydon, NgayThuLy);

                                }
                                catch
                                {

                                }
                                DocumentInfo dCInfo = new DonThuDAL().GetDocumentByID(idxulydon);

                                kq = WorkflowInstance.Instance.ExecuteCommand(idxulydon, canboid, commandCode, DateTime.Now.AddDays(45), DuyetXuLyModel.NoiDungPheDuyet ?? string.Empty);

                                #endregion

                                #region 
                                //TiepDanInfo xldInfo = null;
                                //xldInfo = info;

                                //if (IdentityHelper.SuDungQuyTrinhPhucTap == true)
                                //{
                                //    if (suDungQTVanThuTiepDan == false)
                                //        ChuyenDon(xldInfo, IdentityHelper);
                                //}

                                //DateTime NgayChuyen = DateTime.Now;
                                //if (suDungQTVanThuTiepDan)
                                //    commandCode = WorkflowInstance.Instance.GetAvailabelCommands(idxulydon)[6];
                                //else
                                //    commandCode = WorkflowInstance.Instance.GetAvailabelCommands(idxulydon)[3];


                                //kq = WorkflowInstance.Instance.ExecuteCommand(idxulydon, canboid, commandCode, NgayChuyen, DuyetXuLyModel.NoiDungPheDuyet);

                                //try
                                //{
                                //    if (kq == true)
                                //    {
                                //        if (suDungQTVanThuTiepDan == false)
                                //            new ChuyenXuLy().ChuyenDonInsert(idxulydon, NgayChuyen);

                                //    }
                                //}
                                //catch
                                //{
                                //}
                                //showThongBao = false;
                                //if (laBanTCDTinh)
                                //{
                                //    ScriptManager.RegisterStartupScript(this, typeof(Page), "PhanGiaiQuyet", "PhanGiaiQuyet('" + idxulydon + "');", true);
                                //}
                                #endregion
                            }
                            else if (info.HuongGiaiQuyetID == vBDonDoc || info.HuongGiaiQuyetID == cVChiDao)
                            {
                                if (suDungQTVanThuTiepDan)
                                    commandCode = WorkflowInstance.Instance.GetAvailabelCommands(idxulydon)[6];
                                else
                                    commandCode = WorkflowInstance.Instance.GetAvailabelCommands(idxulydon)[CHUYENDON_GUIVBDONDOC];

                                kq = WorkflowInstance.Instance.ExecuteCommand(idxulydon, canboid, commandCode, DateTime.Now, DuyetXuLyModel.NoiDungPheDuyet);
                                showThongBao = false;
                                //if (laBanTCDTinh)
                                //{
                                //    ScriptManager.RegisterStartupScript(this, typeof(Page), "PhanGiaiQuyet", "PhanGiaiQuyet('" + idxulydon + "');", true);
                                //}
                            }
                            else
                            {
                                commandCode = WorkflowInstance.Instance.GetAvailabelCommands(idxulydon)[3];
                                try
                                {
                                    kq = WorkflowInstance.Instance.ExecuteCommand(idxulydon, canboid, commandCode, DateTime.Now, !string.IsNullOrEmpty(DuyetXuLyModel.NoiDungPheDuyet) ? DuyetXuLyModel.NoiDungPheDuyet : "");
                                }
                                catch (Exception ex)
                                {
                                    Result.Status = -1;
                                    Result.Message = ex.Message;
                                    return Result;
                                }

                            }
                            #endregion
                        }
                        #endregion
                    }
                    else
                    {
                        #region lanh dao khong duyet
                        //DateTime hanxuly = Utils.ConvertToDateTime(DuyetXuLyModel.NgayHetHan, DateTime.Now);

                        //string commandCode = "";
                        ////int phongbanidnew = Utils.ConvertToInt32(ddlPhongBan.SelectedValue, 0);
                        ////int phongbanidold = Utils.ConvertToInt32(hdfPhongBanID.Value, 0);
                        //int phongbanidnew = IdentityHelper.PhongBanID ?? 0;
                        //int phongbanidold = IdentityHelper.PhongBanID ?? 0;
                        //int val = 0;
                        //if (PrevState == Constant.TP_DuyetXuLy)
                        //{
                        //    #region TP trinh len
                        //    if (phongbanidnew != phongbanidold)
                        //    {
                        //        PhanXuLyInfo info = new PhanXuLyInfo();
                        //        info.GhiChu = DuyetXuLyModel.NoiDungPheDuyet;
                        //        info.PhongBanID = phongbanidnew;
                        //        info.XuLyDonID = idxulydon;
                        //        info.NgayPhan = DateTime.Now;


                        //        try
                        //        {
                        //            val = new PhanXuLy().Insert(info);
                        //        }
                        //        catch
                        //        {

                        //        }
                        //    }
                        //    commandCode = WorkflowInstance.Instance.GetAvailabelCommands(idxulydon)[1];
                        //    #endregion
                        //}
                        //else if (PrevState == Constant.CV_XuLy)
                        //{
                        //    #region cv trinh len
                        //    //int canboidnew = Utils.ConvertToInt32(hdfCanBoIDNew.Value, 0);
                        //    //int canboidold = Utils.ConvertToInt32(hdfCanBoID.Value, 0);
                        //    int canboidnew = IdentityHelper.CanBoID ?? 0;
                        //    int canboidold = IdentityHelper.CanBoID ?? 0;
                        //    if (phongbanidnew != phongbanidold)
                        //    {
                        //        PhanXuLyInfo info = new PhanXuLyInfo();
                        //        info.GhiChu = DuyetXuLyModel.NoiDungPheDuyet;
                        //        info.PhongBanID = phongbanidnew;
                        //        info.CanBoID = canboidnew;
                        //        info.XuLyDonID = idxulydon;
                        //        info.NgayPhan = DateTime.Now;


                        //        try
                        //        {
                        //            val = new PhanXuLy().Insert(info);
                        //            new XuLyDonDAL().UpdateCanBoXuLy(info.XuLyDonID, info.CanBoID);
                        //        }
                        //        catch
                        //        {

                        //        }
                        //    }
                        //    else if (canboidnew != canboidold && canboidnew != 0)
                        //    {
                        //        PhanXuLyInfo info = new PhanXuLyInfo();
                        //        info.GhiChu = DuyetXuLyModel.NoiDungPheDuyet;
                        //        info.PhongBanID = phongbanidold;
                        //        info.CanBoID = canboidnew;
                        //        info.XuLyDonID = idxulydon;
                        //        info.NgayPhan = DateTime.Now;


                        //        try
                        //        {
                        //            val = new PhanXuLy().Insert(info);
                        //            new XuLyDonDAL().UpdateCanBoXuLy(info.XuLyDonID, info.CanBoID);
                        //        }
                        //        catch
                        //        {

                        //        }
                        //    }

                        //    commandCode = WorkflowInstance.Instance.GetAvailabelCommands(idxulydon)[2];
                        //    #endregion
                        //}
                        //else if (PrevState == Constant.CV_TiepNhan)
                        //{
                        //    #region ???
                        //    TiepDanInfo info = new TiepDanInfo();
                        //    try
                        //    {
                        //        info = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().GetXuLyDonByXLDID(idxulydon);
                        //    }
                        //    catch
                        //    {

                        //    }
                        //    if (info.HuongGiaiQuyetID == (int)HuongGiaiQuyetEnum.ChuyenDon)
                        //    {
                        //        new ChuyenXuLy().Delete(idxulydon);
                        //    }

                        //    commandCode = WorkflowInstance.Instance.GetAvailabelCommands(idxulydon)[5];
                        //    #endregion
                        //}
                        //int canboid = IdentityHelper.CanBoID ?? 0;
                        //kq = WorkflowInstance.Instance.ExecuteCommand(idxulydon, canboid, commandCode, hanxuly, DuyetXuLyModel.NoiDungPheDuyet);
                        try
                        {
                            //PhanXuLyInfo info = new PhanXuLyInfo();
                            //info.GhiChu = DuyetXuLyModel.NoiDungPheDuyet;
                            //info.PhongBanID = IdentityHelper.PhongBanID ?? 0;
                            //info.CanBoID = IdentityHelper.CanBoID ?? 0;
                            //info.XuLyDonID = idxulydon;
                            //info.NgayPhan = DateTime.Now;

                            //new PhanXuLy().Insert(info);
                            //new XuLyDonDAL().UpdateCanBoXuLy(info.XuLyDonID, info.CanBoID);


                            PhanXuLyInfo info = new PhanXuLyInfo();
                            info.GhiChu = DuyetXuLyModel.NoiDungPheDuyet;
                            info.PhongBanID = IdentityHelper.PhongBanID ?? 0;
                            info.XuLyDonID = idxulydon;
                            info.NgayPhan = DateTime.Now;
                            new PhanXuLy().Insert(info);
                            //update state
                            DateTime hanxuly = Utils.ConvertToDateTime(DuyetXuLyModel.NgayHetHan, DateTime.Now);
                            new XuLyDonDAL().UpdateDocument(info.XuLyDonID, 11, hanxuly);
                        }
                        catch
                        {

                        }

                        Result.Status = 1;
                        Result.Message = Constant.CONTENT_PHEDUYET_SUCCESS;
                        #endregion
                    }

                    //if (showThongBao == true)
                    //{

                    if (kq == true)
                    {
                        Result.Status = 1;
                        Result.Message = Constant.CONTENT_PHEDUYET_SUCCESS;
                        //lblContentErr.Text = "";
                        //lblContentSuccess.Text = Constant.CONTENT_PHEDUYET_SUCCESS;
                        //ScriptManager.RegisterStartupScript(this, typeof(Page), "showthongBaoSuccess", "showthongBaoSuccess();", true);

                    }
                    else
                    {
                        Result.Status = 0;
                        Result.Message = Constant.CONTENT_PHEDUYET_ERROR;
                        //lblContentErr.Text = Constant.CONTENT_PHEDUYET_ERROR;
                        //lblContentSuccess.Text = "";
                        //ScriptManager.RegisterStartupScript(this, typeof(Page), "showthongBaoError", "showthongBaoError();", true);

                    }
                    //}
                    #endregion
                }
                else if (CheckChucVu == (int)EnumChucVu.TruongPhong)
                {
                    #region truong phong 
                    if (DuyetXuLyModel.TrangThaiPheDuyet == 1)
                    {
                        #region TP duyet xu ly
                        bool duyetVaKetThucWF = false;
                        int DUYET_VA_KET_THUC_WF = 2;
                        int DUYET_XU_LY = 0;
                        string commandCode = string.Empty;

                        List<string> commandList = WorkflowInstance.Instance.GetAvailabelCommands(idxulydon);

                        XuLyDonInfo xldInfo = new XuLyDonDAL().GetByID(idxulydon, string.Empty);
                        //Neu don thu den tu Tiep dan va huong xu ly la huong dan, sau khi truong phong duyet se ket thuc WF
                        if (xldInfo.NguonDonDenID == (int)EnumNguonDonDen.TrucTiep && xldInfo.HuongGiaiQuyetID == (int)HuongGiaiQuyetEnum.HuongDan)
                        {
                            duyetVaKetThucWF = true;
                        }
                        if (xldInfo.HuongGiaiQuyetID == (int)HuongGiaiQuyetEnum.TraDon)
                        {
                            duyetVaKetThucWF = true;
                        }
                        DateTime hanXuLy;
                        if (duyetVaKetThucWF)
                        {
                            commandCode = commandList[DUYET_VA_KET_THUC_WF];
                            hanXuLy = Utils.ConvertToDateTime(DuyetXuLyModel.NgayHetHan, DateTime.MinValue);
                        }
                        else commandCode = commandList[DUYET_XU_LY];
                        {
                            TransitionHistoryInfo tranHSInfo = new TransitionHistoryDAL().GetDueDateByID(idxulydon, (int)EnumState.LDPhanXuLy);

                            if (tranHSInfo != null)
                            {
                                hanXuLy = tranHSInfo.DueDate;
                            }
                            else
                                hanXuLy = Utils.ConvertToDateTime(DuyetXuLyModel.NgayHetHan, DateTime.MinValue);

                        }
                        //DateTime hanXuLy = Utils.ConvertToDateTime(DuyetXuLyModel.NgayHetHan, DateTime.MinValue);

                        int canboid = IdentityHelper.CanBoID ?? 0;
                        kq = WorkflowInstance.Instance.ExecuteCommand(idxulydon, canboid, commandCode, hanXuLy, DuyetXuLyModel.NoiDungPheDuyet);
                        // sau khi duyệt xử lý trưởng phòng => gửi lên lãnh đạo duyệt xử lý

                        //commandList = WorkflowInstance.Instance.GetAvailabelCommands(idxulydon);
                        //commandCode = commandList[DUYET_XU_LY];
                        //kq = WorkflowInstance.Instance.ExecuteCommand(idxulydon, canboid, commandCode, DateTime.Now.AddDays(45), "");
                        #endregion
                    }
                    else
                    {

                        try
                        {
                            //PhanXuLyInfo info = new PhanXuLyInfo();
                            //info.GhiChu = DuyetXuLyModel.NoiDungPheDuyet;
                            //info.PhongBanID = IdentityHelper.PhongBanID ?? 0;
                            //info.CanBoID = IdentityHelper.CanBoID ?? 0;
                            //info.XuLyDonID = idxulydon;
                            //info.NgayPhan = DateTime.Now;

                            //new PhanXuLy().Insert(info);
                            //new XuLyDonDAL().UpdateCanBoXuLy(info.XuLyDonID, info.CanBoID);


                            PhanXuLyInfo info = new PhanXuLyInfo();
                            info.GhiChu = DuyetXuLyModel.NoiDungPheDuyet;
                            info.PhongBanID = IdentityHelper.PhongBanID ?? 0;
                            info.XuLyDonID = idxulydon;
                            info.NgayPhan = DateTime.Now;
                            new PhanXuLy().Insert(info);
                            //update state
                            DateTime hanxuly = Utils.ConvertToDateTime(DuyetXuLyModel.NgayHetHan, DateTime.Now);
                            new XuLyDonDAL().UpdateDocument(info.XuLyDonID, 11, hanxuly);
                        }
                        catch
                        {

                        }

                        ////int canboidnew = Utils.ConvertToInt32(ddlCanBo.SelectedValue, 0);
                        ////int canboidold = Utils.ConvertToInt32(hdfCanBoID.Value, 0);
                        ////int phongbanidold = Utils.ConvertToInt32(hdfPhongBanID.Value, 0);
                        //int canboidnew = IdentityHelper.CanBoID ?? 0;
                        //int canboidold = IdentityHelper.CanBoID ?? 0;
                        //int phongbanidold = IdentityHelper.PhongBanID ?? 0;
                        //if (canboidnew != canboidold)
                        //{
                        //    PhanXuLyInfo info = new PhanXuLyInfo();
                        //    info.GhiChu = DuyetXuLyModel.NoiDungPheDuyet;
                        //    info.PhongBanID = phongbanidold;
                        //    info.XuLyDonID = idxulydon;
                        //    info.NgayPhan = DateTime.Now;
                        //    info.CanBoID = canboidnew;

                        //    try
                        //    {
                        //        new PhanXuLy().Insert(info);
                        //        new XuLyDonDAL().UpdateCanBoXuLy(info.XuLyDonID, info.CanBoID);
                        //    }
                        //    catch
                        //    {

                        //    }
                        //}

                        //DateTime hanxuly = Utils.ConvertToDateTime(DuyetXuLyModel.NgayHetHan, DateTime.Now);
                        //string commandCode = WorkflowInstance.Instance.GetAvailabelCommands(idxulydon)[1];
                        //int canboid = IdentityHelper.CanBoID ?? 0;
                        //kq = WorkflowInstance.Instance.ExecuteCommand(idxulydon, canboid, commandCode, hanxuly, DuyetXuLyModel.NoiDungPheDuyet);

                    }
                    if (showThongBao == true)
                    {
                        if (kq == true)
                        {
                            Result.Status = 1;
                            Result.Message = Constant.CONTENT_PHEDUYET_SUCCESS;
                            //lblContentErr.Text = "";
                            //lblContentSuccess.Text = Constant.CONTENT_PHEDUYET_SUCCESS;
                            //ScriptManager.RegisterStartupScript(this, typeof(Page), "showthongBaoSuccess", "showthongBaoSuccess();", true);

                        }
                        else
                        {
                            Result.Status = 0;
                            Result.Message = Constant.CONTENT_PHEDUYET_ERROR;
                            //lblContentErr.Text = Constant.CONTENT_PHEDUYET_ERROR;
                            //lblContentSuccess.Text = "";
                            //ScriptManager.RegisterStartupScript(this, typeof(Page), "showthongBaoError", "showthongBaoError();", true);

                        }
                    }

                    #endregion
                }

                //new XuLyDonDAL().UpdateTrangThaiDuyet(idxulydon, null);
            }




            #region file
            //file dinh kem
            if (DuyetXuLyModel.DanhSachHoSoTaiLieu != null && DuyetXuLyModel.DanhSachHoSoTaiLieu.Count > 0)
            {
                foreach (var item in DuyetXuLyModel.DanhSachHoSoTaiLieu)
                {
                    if (item.DanhSachFileDinhKemID != null && item.DanhSachFileDinhKemID.Count > 0)
                    {
                        new FileDinhKemDAL().UpdateFileXuLyDon(item.DanhSachFileDinhKemID, DuyetXuLyModel.XuLyDonID ?? 0, 0);
                    }
                }

            }

            //try
            //{
            //    String fileDataStr = hdfFileData.Value;
            //    if (fileDataStr != string.Empty)
            //    {
            //        string[] fileParts = fileDataStr.Split(';');
            //        for (int i = 0; i < fileParts.Length; i++)
            //        {
            //            string fileStr = fileParts[i];
            //            string[] dataParts = fileStr.Split(',');

            //            FileHoSoInfo info = new FileHoSoInfo();
            //            info.XuLyDonID = idxulydon;
            //            info.FileURL = dataParts[0];
            //            info.NgayUp = Utils.ConvertToDateTime(dataParts[1], Constant.DEFAULT_DATE);
            //            info.TenFile = dataParts[2];
            //            info.TomTat = dataParts[3];
            //            info.NguoiUp = IdentityHelper.GetCanBoID();//IdentityHelper.GetUserID();
            //            info.FileID = Utils.ConvertToInt32(dataParts[6], 0);
            //            FileLogInfo infoFileLog = new FileLogInfo();
            //            infoFileLog.LoaiLog = (int)EnumLoaiLog.Them;
            //            infoFileLog.LoaiFile = (int)EnumLoaiFile.FileKQXL;
            //            int isBaoMat = Utils.ConvertToInt32(dataParts[4], 0);
            //            if (isBaoMat == (int)EnumDoBaoMat.BaoMat)
            //            {
            //                infoFileLog.IsBaoMat = true;
            //                infoFileLog.IsMaHoa = true;
            //            }
            //            else
            //            {
            //                infoFileLog.IsBaoMat = false;
            //                infoFileLog.IsMaHoa = false;
            //            }

            //            try
            //            {
            //                int result = new FileHoSo().InsertFileYKienXL(info);
            //                if (result > 0)
            //                {
            //                    infoFileLog.FileID = result;
            //                    new FileLog().Insert(infoFileLog);
            //                }

            //            }
            //            catch
            //            {
            //            }
            //        }
            //    }
            //}
            //catch
            //{
            //}
            #endregion

            return Result;
        }
        public BaseResultModel TrinhDuThao(DuyetXuLyModel DuyetXuLyModel, IdentityHelper IdentityHelper)
        {
            var Result = new BaseResultModel();
            if (DuyetXuLyModel.XuLyDonID < 0)
            {
                Result.Status = 0;
                Result.Message = "Chưa chọn đơn thư cần trình";
                return Result;
            }
            // bổ sung 1 bước ban tiếp dân trưởng ban trình lên chủ tịch tỉnh
            // đổi cờ trạng thái trình = 0 để lấy trạng thái là chưa chưa duyệt
            var checkBTDH = IdentityHelper.CapHanhChinh == EnumCapHanhChinh.CapPhongThuocHuyen.GetHashCode() && ((IdentityHelper.BanTiepDan) ?? false);
            var trangThaiTrinh = checkBTDH ? TrangThaiTrinhDuThao.ChuaTrinh.GetHashCode() : TrangThaiTrinhDuThao.DaTrinh.GetHashCode();
            int val = new XuLyDonDAL().UpdateTrangThaiTrinhDuThao(DuyetXuLyModel.XuLyDonID, trangThaiTrinh);
            if (DuyetXuLyModel.LanhDaoID > 0)
            {
                new XuLyDonDAL().UpdateLanhDaoDuyet(DuyetXuLyModel.XuLyDonID, null, DuyetXuLyModel.LanhDaoID);
            }
            //if(val > 0)
            //{
            //    Result.Status = 1;
            //    Result.Message = "Trình dự thảo thành công";
            //}
            //else
            //{
            //    Result.Status = 0;
            //    Result.Message = "Trình dự thảo thất bại";
            //}
            Result.Status = 1;
            Result.Message = checkBTDH ? "Trình duyệt thành công" : "Trình dự thảo thành công";
            #region file
            //file dinh kem
            if (DuyetXuLyModel.DanhSachHoSoTaiLieu != null && DuyetXuLyModel.DanhSachHoSoTaiLieu.Count > 0)
            {
                foreach (var item in DuyetXuLyModel.DanhSachHoSoTaiLieu)
                {
                    if (item.DanhSachFileDinhKemID != null && item.DanhSachFileDinhKemID.Count > 0)
                    {
                        new FileDinhKemDAL().UpdateFileXuLyDon(item.DanhSachFileDinhKemID, DuyetXuLyModel.XuLyDonID ?? 0, 0);
                    }
                }

            }
            #endregion

            return Result;
        }
        public BaseResultModel UpdateTrinhDuThao(DuyetXuLyModel DuyetXuLyModel, IdentityHelper IdentityHelper)
        {
            var Result = new BaseResultModel();
            if (DuyetXuLyModel.XuLyDonID < 0)
            {
                Result.Status = 0;
                Result.Message = "Chưa chọn đơn thư cần trình";
                return Result;
            }
            int val = new XuLyDonDAL().UpdateTrangThaiTrinhDuThao(DuyetXuLyModel.XuLyDonID, TrangThaiTrinhDuThao.DaTrinh.GetHashCode());
            if (DuyetXuLyModel.LanhDaoID > 0)
            {
                new XuLyDonDAL().UpdateLanhDaoDuyet(DuyetXuLyModel.XuLyDonID, null, DuyetXuLyModel.LanhDaoID);
            }

            Result.Status = 1;
            Result.Message = "Phê duyệt thành công";
            #region file
            //file dinh kem
            if (DuyetXuLyModel.DanhSachHoSoTaiLieu != null && DuyetXuLyModel.DanhSachHoSoTaiLieu.Count > 0)
            {
                foreach (var item in DuyetXuLyModel.DanhSachHoSoTaiLieu)
                {
                    if (item.DanhSachFileDinhKemID != null && item.DanhSachFileDinhKemID.Count > 0)
                    {
                        new FileDinhKemDAL().UpdateFileXuLyDon(item.DanhSachFileDinhKemID, DuyetXuLyModel.XuLyDonID ?? 0, 0);
                    }
                }

            }
            #endregion

            return Result;
        }
        public BaseResultModel LuuDuThao(DuyetXuLyModel DuyetXuLyModel)
        {
            var Result = new BaseResultModel();
            if (DuyetXuLyModel.XuLyDonID < 0)
            {
                Result.Status = 0;
                Result.Message = "Chưa chọn đơn thư cần trình dự thảo";
                return Result;
            }

            #region file
            //file dinh kem
            if (DuyetXuLyModel.DanhSachHoSoTaiLieu != null && DuyetXuLyModel.DanhSachHoSoTaiLieu.Count > 0)
            {
                foreach (var item in DuyetXuLyModel.DanhSachHoSoTaiLieu)
                {
                    if (item.DanhSachFileDinhKemID != null && item.DanhSachFileDinhKemID.Count > 0)
                    {
                        new FileDinhKemDAL().UpdateFileXuLyDon(item.DanhSachFileDinhKemID, DuyetXuLyModel.XuLyDonID ?? 0, 0);
                    }
                }

            }
            #endregion
            Result.Status = 1;
            Result.Message = "Lưu dự thảo thành công";

            return Result;
        }
        public BaseResultModel DuyetDuThao(DuyetXuLyModel DuyetXuLyModel)
        {
            var Result = new BaseResultModel();
            if (DuyetXuLyModel.XuLyDonID < 0)
            {
                Result.Status = 0;
                Result.Message = "Chưa chọn đơn thư cần duyệt";
                return Result;
            }

            int val = new XuLyDonDAL().BanHanhQuyetDinhXacMinh(DuyetXuLyModel.XuLyDonID, TrangThaiTrinhDuThao.DaDuyetQDXacMinh.GetHashCode(), DuyetXuLyModel.NoiDungBanHanh, DuyetXuLyModel.CanBoID, DateTime.Now);
            //int val = new XuLyDonDAL().UpdateTrangThaiTrinhDuThao(DuyetXuLyModel.XuLyDonID, TrangThaiTrinhDuThao.DaDuyet.GetHashCode());
            //if(val > 0)
            //{
            //    Result.Status = 1;
            //    Result.Message = "Trình dự thảo thành công";
            //}
            //else
            //{
            //    Result.Status = 0;
            //    Result.Message = "Trình dự thảo thất bại";
            //}
            Result.Status = 1;
            Result.Message = "Duyệt dự thảo thành công";
            #region file
            //file dinh kem
            if (DuyetXuLyModel.DanhSachHoSoTaiLieu != null && DuyetXuLyModel.DanhSachHoSoTaiLieu.Count > 0)
            {
                foreach (var item in DuyetXuLyModel.DanhSachHoSoTaiLieu)
                {
                    if (item.DanhSachFileDinhKemID != null && item.DanhSachFileDinhKemID.Count > 0)
                    {
                        new FileDinhKemDAL().UpdateFileXuLyDon(item.DanhSachFileDinhKemID, DuyetXuLyModel.XuLyDonID ?? 0, 0);
                    }
                }

            }
            #endregion

            return Result;
        }
        public BaseResultModel BanHanhQuyetDinhGiaiQuyet(DuyetXuLyModel DuyetXuLyModel)
        {
            var Result = new BaseResultModel();
            if (DuyetXuLyModel.XuLyDonID < 0)
            {
                Result.Status = 0;
                Result.Message = "Chưa chọn đơn thư cần duyệt";
                return Result;
            }

            int val = new XuLyDonDAL().BanHanhQuyetDinhGiaiQuyet(DuyetXuLyModel.XuLyDonID, TrangThaiTrinhDuThao.DaDuyetQDGiaiQuyet.GetHashCode(), DuyetXuLyModel.NoiDungGiaiQuyet);
            //int val = new XuLyDonDAL().UpdateTrangThaiTrinhDuThao(DuyetXuLyModel.XuLyDonID, TrangThaiTrinhDuThao.DaDuyet.GetHashCode());
            //if(val > 0)
            //{
            //    Result.Status = 1;
            //    Result.Message = "Trình dự thảo thành công";
            //}
            //else
            //{
            //    Result.Status = 0;
            //    Result.Message = "Trình dự thảo thất bại";
            //}
            Result.Status = 1;
            Result.Message = "Duyệt dự thảo thành công";
            #region file
            //file dinh kem
            if (DuyetXuLyModel.DanhSachHoSoTaiLieu != null && DuyetXuLyModel.DanhSachHoSoTaiLieu.Count > 0)
            {
                foreach (var item in DuyetXuLyModel.DanhSachHoSoTaiLieu)
                {
                    if (item.DanhSachFileDinhKemID != null && item.DanhSachFileDinhKemID.Count > 0)
                    {
                        new FileDinhKemDAL().UpdateFileXuLyDon(item.DanhSachFileDinhKemID, DuyetXuLyModel.XuLyDonID ?? 0, 0);
                    }
                }

            }
            #endregion

            return Result;
        }
        private void ChuyenDon(TiepDanInfo xldGocInfo, IdentityHelper IdentityHelper)
        {
            int cqNhapHoID = 0;
            //if (cbxNhapHo.Checked)
            //{
            //    cqNhapHoID = Utils.ConvertToInt32(ddlCoQuanNhapHo.SelectedValue, 0);
            //}

            ChuyenXuLyInfo cxlInfo = new ChuyenXuLyInfo();
            cxlInfo.XuLyDonID = xldGocInfo.XuLyDonID ?? 0;
            if (cqNhapHoID != 0)
            {
                cxlInfo.CQGuiID = cqNhapHoID;
            }
            else
            {
                cxlInfo.CQGuiID = IdentityHelper.CoQuanID ?? 0;
            }
            cxlInfo.CQNhanID = xldGocInfo.CQChuyenDonID ?? 0;
            cxlInfo.NgayChuyen = DateTime.Now;
            CloneDonThuTaiCQDuocChuyenDon(cxlInfo.CQNhanID, xldGocInfo, IdentityHelper);

            try
            {
                new ChuyenXuLy().Insert(cxlInfo);
            }
            catch
            {
            }
        }
        private void CloneDonThuTaiCQDuocChuyenDon(int coQuanNhanID, TiepDanInfo xldGocInfo, IdentityHelper IdentityHelper)
        {
            string commandCode = "";
            int xuLyDonCloneID = 0;

            TiepDanInfo xldCloneInfo = xldGocInfo;

            xldCloneInfo.CanBoTiepNhapID = 0;
            xldCloneInfo.SoDonThu = GetSoDonThu(coQuanNhanID, IdentityHelper);
            xldCloneInfo.CoQuanID = coQuanNhanID;
            xldCloneInfo.NguonDonDen = (int)EnumNguonDonDen.CoQuanKhac;
            xldCloneInfo.CQChuyenDonID = 0;
            xldCloneInfo.CQChuyenDonDenID = IdentityHelper.CoQuanID;

            xldCloneInfo.HuongGiaiQuyetID = 0;
            xldCloneInfo.CanBoXuLyID = 0;
            xldCloneInfo.NgayChuyenDon = DateTime.Now;
            xldCloneInfo.DaDuyetXuLy = false;
            xldCloneInfo.XuLyDonIDGoc = xldGocInfo.XuLyDonID;
            try
            {
                xuLyDonCloneID = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().InsertXuLyDon(xldCloneInfo);

            }
            catch
            {
            }

            ////Clone file dinh kem
            //List<FileHoSoInfo> fileHoSoList = new FileHoSo().GetByXuLyDonID(xldGocInfo.XuLyDonID).ToList();
            //foreach (var fileInfo in fileHoSoList)
            //{
            //    fileInfo.XuLyDonID = xuLyDonCloneID;
            //    try
            //    {
            //        int result = new FileHoSo().Insert(fileInfo);
            //        FileLogInfo infoFileLog = new FileLogInfo();
            //        infoFileLog.LoaiLog = (int)EnumLoaiLog.Them;
            //        infoFileLog.LoaiFile = fileInfo.LoaiFile;
            //        infoFileLog.IsBaoMat = fileInfo.IsBaoMat;
            //        infoFileLog.IsMaHoa = fileInfo.IsMaHoa;
            //        if (result > 0)
            //        {
            //            infoFileLog.FileID = result;
            //            new FileLog().Insert(infoFileLog);
            //        }
            //    }
            //    catch
            //    {
            //    }
            //}

            ////clone file y kien xu ly
            //List<XuLyDonInfo> lsFileYKienXL = new XuLyDon().GetFileYKienXuLy(xldGocInfo.XuLyDonID).ToList();
            //FileHoSoInfo fileYKienXLInfo = new FileHoSoInfo();
            //foreach (var files in lsFileYKienXL)
            //{

            //    fileYKienXLInfo.TenFile = files.TenFileYKienXL;
            //    fileYKienXLInfo.TomTat = files.TomTat;
            //    fileYKienXLInfo.NgayUp = files.NgayUp;
            //    fileYKienXLInfo.NguoiUp = files.NguoiUp;
            //    fileYKienXLInfo.FileURL = files.FileURL;
            //    fileYKienXLInfo.XuLyDonID = xuLyDonCloneID;

            //    FileLogInfo infoFileLog = new FileLogInfo();
            //    infoFileLog.LoaiLog = (int)EnumLoaiLog.Them;
            //    infoFileLog.LoaiFile = files.LoaiFile;
            //    infoFileLog.IsBaoMat = files.IsBaoMat;
            //    infoFileLog.IsMaHoa = files.IsMaHoa;
            //    try
            //    {
            //        int result = new FileHoSo().InsertFileYKienXL(fileYKienXLInfo);
            //        if (result > 0)
            //        {
            //            infoFileLog.FileID = result;
            //            new FileLog().Insert(infoFileLog);
            //        }
            //    }
            //    catch
            //    {
            //    }
            //}

            WorkflowInstance.Instance.AttachDocument(xuLyDonCloneID, "XuLyDon", IdentityHelper.UserID ?? 0, null);

            DateTime NgayChuyen = DateTime.Now;
            int canboid = IdentityHelper.CanBoID ?? 0;

        }
        public static string GetSoDonThu(int coquanID, IdentityHelper IdentityHelper)
        {
            string maCQ = string.Empty;
            string soDonThu = string.Empty;
            try
            {
                soDonThu = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().GetSoDonThu(coquanID);

            }
            catch (Exception)
            {

            }
            if (coquanID == IdentityHelper.CoQuanID)
            {
                maCQ = IdentityHelper.MaCoQuan;
            }
            else
            {
                CoQuanInfo cqInfo = new CoQuan().GetCoQuanByID(coquanID);
                maCQ = cqInfo?.MaCQ;
            }

            string numberPart = !string.IsNullOrEmpty(soDonThu) ? Regex.Replace(soDonThu.Replace(maCQ, ""), "[^0-9.]", "") : "";
            int soDonMoi = Utils.ConvertToInt32(numberPart, 0) + 1;

            return maCQ + soDonMoi;
        }

        public BaseResultModel PhanChoCoQuanKhac(IdentityHelper IdentityHelper, ChuyenDonModel ChuyenDonModel)
        {
            var Result = new BaseResultModel();
            int docunmentid = ChuyenDonModel.XuLyDonID ?? 0;
            int coQuanID = ChuyenDonModel.CoQuanID ?? 0;
            string commandCode = "";
            int canboid = IdentityHelper.CanBoID ?? 0;
            bool kq = true;
            DateTime dueDate = ChuyenDonModel.NgayChuyen ?? DateTime.Now;
            var a = new XuLyDonDAL().UpdateNgayChuyen(docunmentid, dueDate);

            if (ChuyenDonModel.CoQuanNgoaiHeThong != null && ChuyenDonModel.CoQuanNgoaiHeThong.Length > 0)
            {
                new XuLyDonDAL().UpdateCoQuanChuyenNgoaiHeThong(ChuyenDonModel.XuLyDonID ?? 0, ChuyenDonModel.CoQuanNgoaiHeThong, ChuyenDonModel.CoQuanTheoDoiDonDoc);
                Result.Status = 1;
                Result.Message = "Chuyển thành công";
                new RutDon_V2DAL().UpdateDocument_By_XuLyDonID(ChuyenDonModel.XuLyDonID ?? 0);
            }
            else
            {
                if (ChuyenDonModel.CoQuanTheoDoiDonDoc != null && ChuyenDonModel.CoQuanTheoDoiDonDoc.Length > 0)
                {
                    new XuLyDonDAL().UpdateCoQuanChuyenNgoaiHeThong(ChuyenDonModel.XuLyDonID ?? 0, null, ChuyenDonModel.CoQuanTheoDoiDonDoc);
                }

                string stateName = WorkflowInstance.Instance.GetCurrentStateOfDocument(docunmentid);
                if (stateName == Constant.CHUYENDON_RAVBDONDOC)
                {
                    commandCode = WorkflowInstance.Instance.GetAvailabelCommands(docunmentid).Where(x => x.ToString() == "KetThuc").FirstOrDefault();

                }
                else
                {
                    commandCode = WorkflowInstance.Instance.GetAvailabelCommands(docunmentid).Where(x => x.ToString() == "ChuyenDonHoacGuiVBDonDoc").FirstOrDefault();
                    kq = WorkflowInstance.Instance.ExecuteCommand(docunmentid, canboid, commandCode, dueDate, "");
                    stateName = WorkflowInstance.Instance.GetCurrentStateOfDocument(docunmentid);
                    if (stateName == Constant.CHUYENDON_RAVBDONDOC)
                    {
                        commandCode = WorkflowInstance.Instance.GetAvailabelCommands(docunmentid).Where(x => x.ToString() == "KetThuc").FirstOrDefault();
                    }
                }

                int val = 0;

                TiepDanInfo info = new TiepDanInfo();
                try
                {
                    info = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().GetXuLyDonByXLDID(docunmentid);
                }
                catch
                { }
                int chuyenDon = (int)HuongGiaiQuyetEnum.ChuyenDon;
                int vBDonDoc = (int)HuongGiaiQuyetEnum.RaVanBanDonDoc;
                int cVChiDao = (int)HuongGiaiQuyetEnum.CongVanChiDao;
                bool suDungQTVanThuTiepDan = IdentityHelper.SuDungQTVanThuTiepDan ?? false;
                if (info.HuongGiaiQuyetID == chuyenDon)
                {
                    #region chuyen don


                    TiepDanInfo xldInfo = null;
                    xldInfo = info;

                    //if (IdentityHelper.SuDungQuyTrinhPhucTap == true)
                    //{
                    #region chuyen don
                    int cqNhapHoID = 0;

                    ChuyenXuLyInfo cxlInfo = new ChuyenXuLyInfo();
                    cxlInfo.XuLyDonID = info.XuLyDonID ?? 0;
                    if (cqNhapHoID != 0)
                    {
                        cxlInfo.CQGuiID = cqNhapHoID;
                    }
                    else
                    {
                        cxlInfo.CQGuiID = IdentityHelper.CoQuanID ?? 0;
                    }
                    cxlInfo.CQNhanID = info.CQChuyenDonID ?? 0;
                    cxlInfo.NgayChuyen = DateTime.Now;

                    if (info.XuLyDonID != 0)
                        new XuLyDonDAL().UpdateCQNhanDonChuyen(docunmentid, coQuanID);


                    #region clone chuyen don
                    int donThuIDNew = 0;
                    try
                    {
                        #region clone don thu
                        DonThuInfo donThuInfo = new DonThuInfo();
                        donThuInfo = new DonThuDAL().GetByID(info.DonThuID ?? 0);
                        if (donThuInfo != null)
                        {
                            TiepDanInfo tiepDanInfo = new TiepDanInfo();
                            tiepDanInfo.NhomKNID = donThuInfo.NhomKNID;
                            tiepDanInfo.DoiTuongBiKNID = donThuInfo.DoiTuongBiKNID;
                            tiepDanInfo.LoaiKhieuTo1ID = donThuInfo.LoaiKhieuTo1ID;
                            tiepDanInfo.LoaiKhieuTo2ID = donThuInfo.LoaiKhieuTo2ID;
                            tiepDanInfo.LoaiKhieuTo3ID = donThuInfo.LoaiKhieuTo3ID;
                            tiepDanInfo.LoaiKhieuToID = donThuInfo.LoaiKhieuToID;
                            tiepDanInfo.NoiDungDon = donThuInfo.NoiDungDon;
                            tiepDanInfo.TrungDon = false;
                            tiepDanInfo.TinhID = donThuInfo.TinhID;
                            tiepDanInfo.HuyenID = donThuInfo.HuyenID;
                            tiepDanInfo.XaID = donThuInfo.XaID;
                            tiepDanInfo.DiaChiPhatSinh = donThuInfo.DiaChiPhatSinh;
                            tiepDanInfo.LeTanChuyen = false;
                            tiepDanInfo.NgayVietDon = donThuInfo.NgayVietDon;
                            try
                            {
                                donThuIDNew = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().InsertDonThu(tiepDanInfo);
                            }
                            catch { }
                            //new DonThu().Insert(donThuInfo);
                        }
                    }
                    catch { }
                    #endregion

                    int xuLyDonCloneID = 0;
                    TiepDanInfo xldCloneInfo = info;
                    xldCloneInfo.DonThuID = donThuIDNew;
                    // Sửa cán bộ tiếp nhận
                    xldCloneInfo.CanBoTiepNhapID = new XuLyDonDAL().GetByXuLyDonID_V2(info.XuLyDonID.Value).CanBoTiepNhapID;
                    xldCloneInfo.SoDonThu = GetSoDonThu(coQuanID, IdentityHelper);
                    xldCloneInfo.CoQuanID = coQuanID;
                    xldCloneInfo.NguonDonDen = (int)EnumNguonDonDen.CoQuanKhac;
                    xldCloneInfo.CQChuyenDonID = 0;
                    xldCloneInfo.CQChuyenDonDenID = IdentityHelper.CoQuanID ?? 0;

                    xldCloneInfo.HuongGiaiQuyetID = 0;
                    xldCloneInfo.CanBoXuLyID = 0;
                    xldCloneInfo.NgayChuyenDon = DateTime.Now;
                    xldCloneInfo.DaDuyetXuLy = false;
                    xldCloneInfo.XuLyDonIDGoc = ChuyenDonModel.XuLyDonID ?? 0;

                    try
                    {
                        xuLyDonCloneID = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().InsertXuLyDon(xldCloneInfo);

                    }
                    catch
                    {
                    }

                    WorkflowInstance.Instance.AttachDocument(xuLyDonCloneID, "XuLyDon", IdentityHelper.UserID ?? 0, null);
                    #endregion

                    try
                    {
                        new ChuyenXuLy().Insert(cxlInfo);
                    }
                    catch
                    {
                    }
                    #endregion
                    //}

                    DateTime NgayChuyen = DateTime.Now;
                    #endregion
                }
                else if (info.HuongGiaiQuyetID == vBDonDoc || info.HuongGiaiQuyetID == cVChiDao)
                {

                    new XuLyDonDAL().UpdateCQNhanVBDonDoc(docunmentid, coQuanID);
                }

                try
                {
                    kq = WorkflowInstance.Instance.ExecuteCommand(docunmentid, canboid, commandCode, dueDate, "");
                }
                catch (Exception)
                {
                }


                if (kq)
                    val = 1;

                Result.Data = val;
                Result.Status = 1;
                Result.Message = "Chuyển thành công";
            }


            return Result;

        }

        public IList<CoQuanInfo> GetCoQuanChuyenDon(IdentityHelper IdentityHelper)
        {
            // BTD tỉnh => Trưởng ban tiếp dân chuyển đơn
            // BTD huyện => Chủ tịch huyện chuyển đơn
            // Phòng thuộc huyện => lãnh đạo phòng chuyển đơn
            // SBN => lãnh đạo sở chuyển đơn
            // Phòng thuộc sở => lãnh đạo sở chuyển đơn
            // Xã => chủ tịch xã chuyển đơn

            var ListAllCQ = new CoQuan().GetAllCoQuan();
            List<CoQuanInfo> cqList = new List<CoQuanInfo>();
            if (IdentityHelper.SuDungQuyTrinhPhucTap == false)
            {
                return ListAllCQ;
            }

            if (IdentityHelper.CapHanhChinh == EnumCapHanhChinh.CapUBNDXa.GetHashCode())
            {
                var cr = ListAllCQ.Where(x => x.CoQuanID == IdentityHelper.CoQuanID.Value).FirstOrDefault();
                cqList = new CoQuan().GetCoQuanByParentID(cr.CoQuanChaID).Where(x => x.CapID == CapQuanLy.CapUBNDXa.GetHashCode()).ToList();
            }
            else if (IdentityHelper.CapHanhChinh == EnumCapHanhChinh.CapUBNDHuyen.GetHashCode())
            {
                if (IdentityHelper.ChuTichUBND == 1)
                {
                    var cr = ListAllCQ.Where(x => x.CoQuanID == IdentityHelper.CoQuanID.Value).FirstOrDefault();
                    var pr = ListAllCQ.Where(x => x.CoQuanID == cr.CoQuanChaID).FirstOrDefault();
                    cqList = new CoQuan().GetCoQuanByParentID(IdentityHelper.CoQuanID ?? 0).Where(x => x.CapID == CapQuanLy.CapUBNDXa.GetHashCode() || x.CapID == CapQuanLy.CapPhong.GetHashCode()).ToList();
                }
            }
            else if (IdentityHelper.CapHanhChinh == EnumCapHanhChinh.CapPhongThuocHuyen.GetHashCode())
            {
                var cr = ListAllCQ.Where(x => x.CoQuanID == IdentityHelper.CoQuanID.Value).FirstOrDefault();
                var pr = ListAllCQ.Where(x => x.CoQuanID == cr.CoQuanChaID).FirstOrDefault();
                cqList = new CoQuan().GetCoQuanByParentID(cr.CoQuanChaID).Where(x => x.CapID == CapQuanLy.CapUBNDXa.GetHashCode() || x.CapID == CapQuanLy.CapPhong.GetHashCode()).ToList();
            }
            else if (IdentityHelper.CapHanhChinh == EnumCapHanhChinh.CapSoNganh.GetHashCode())
            {
                cqList = ListAllCQ.Where(x => x.CapID == CapQuanLy.CapSoNganh.GetHashCode() || (x.CapID == CapQuanLy.CapPhong.GetHashCode() && (x.BanTiepDan ?? false))).ToList();
            }
            else if (IdentityHelper.CapHanhChinh == EnumCapHanhChinh.CapUBNDTinh.GetHashCode())
            {
                if ((IdentityHelper.BanTiepDan ?? false))
                {
                    cqList = ListAllCQ.Where(x => (x.CapID == CapQuanLy.CapPhong.GetHashCode() && (x.BanTiepDan ?? false))).ToList();

                }
            }

            //if (IdentityHelper.CapID == CapQuanLy.CapUBNDXa.GetHashCode())
            //{
            //    cqList = ListAllCQ.Where(x => x.CoQuanID == IdentityHelper.CoQuanID.Value).ToList();
            //}
            //else if (IdentityHelper.CapID == CapQuanLy.CapUBNDHuyen.GetHashCode())
            //{
            //    if (IdentityHelper.ChuTichUBND == 1)
            //    {
            //        var cr = ListAllCQ.Where(x => x.CoQuanID == IdentityHelper.CoQuanID.Value).FirstOrDefault();
            //        var pr = ListAllCQ.Where(x => x.CoQuanID == cr.CoQuanChaID).FirstOrDefault();
            //        cqList = new CoQuan().GetCoQuanByParentID(IdentityHelper.CoQuanID ?? 0).Where(x => x.CapID == CapQuanLy.CapUBNDXa.GetHashCode() || x.CapID == CapQuanLy.CapPhong.GetHashCode()).ToList();
            //    }
            //    else
            //    {
            //        var cr = ListAllCQ.Where(x => x.CoQuanID == IdentityHelper.CoQuanID.Value).FirstOrDefault();
            //        var pr = ListAllCQ.Where(x => x.CoQuanID == cr.CoQuanChaID).FirstOrDefault();
            //        cqList = new CoQuan().GetCoQuanByParentID(cr.CoQuanChaID).Where(x => x.CapID == CapQuanLy.CapUBNDXa.GetHashCode()).ToList();
            //    }
            //    //cqList.Add(pr);
            //}
            //else
            //{
            //    var cr = ListAllCQ.Where(x => x.CoQuanID == IdentityHelper.CoQuanID.Value).FirstOrDefault();
            //    var pr = ListAllCQ.Where(x => x.CoQuanID == cr.CoQuanChaID).FirstOrDefault();
            //    cqList = ListAllCQ.Where(x => x.CapID == CapQuanLy.CapUBNDHuyen.GetHashCode()).ToList();
            //    //cqList.Add(pr);
            //    try
            //    {
            //        var listThanhTraTinh = new SystemConfigDAL().GetByKey("Thanh_Tra_Tinh_ID").ConfigValue.Split(',').ToList().Select(x => Utils.ConvertToInt32(x.ToString(), 0)).ToList();
            //        if (listThanhTraTinh.Contains(IdentityHelper.CoQuanID.Value))
            //        {
            //            var capSo = ListAllCQ.Where(x => x.CapID == CapQuanLy.CapSoNganh.GetHashCode()).ToList();
            //            cqList.AddRange(capSo);
            //        }
            //    }
            //    catch (Exception)
            //    {
            //    }


            //}

            return cqList;
        }

        public BaseResultModel TrinhLanhDao(DuyetXuLyModel DuyetXuLyModel, IdentityHelper identityHelper)
        {
            var Result = new BaseResultModel();
            if (DuyetXuLyModel.XuLyDonID < 0)
            {
                Result.Status = 0;
                Result.Message = "Chưa chọn đơn thư cần trình";
                return Result;
            }

            if (DuyetXuLyModel.LanhDaoID > 0)
            {
                new XuLyDonDAL().UpdateLanhDaoDuyet(DuyetXuLyModel.XuLyDonID, null, DuyetXuLyModel.LanhDaoID);
            }

            if ((identityHelper.RoleID ?? 0) == (int)EnumChucVu.TruongPhong && (identityHelper.CapHanhChinh == EnumCapHanhChinh.CapSoNganh.GetHashCode() || identityHelper.CapHanhChinh == EnumCapHanhChinh.CapPhongThuocHuyen.GetHashCode()))
            {
                List<string> commandList = WorkflowInstance.Instance.GetAvailabelCommands(DuyetXuLyModel.XuLyDonID ?? 0);
                int DUYET_XU_LY = 0;
                string commandCode = string.Empty;
                bool kq = true;
                commandList = WorkflowInstance.Instance.GetAvailabelCommands(DuyetXuLyModel.XuLyDonID ?? 0);
                commandCode = commandList[DUYET_XU_LY];
                kq = WorkflowInstance.Instance.ExecuteCommand(DuyetXuLyModel.XuLyDonID ?? 0, identityHelper.CanBoID ?? 0, commandCode, DateTime.Now.AddDays(45), "");
            }

            Result.Status = 1;
            Result.Message = "Trình lãnh đạo thành công";
            #region file
            //file dinh kem
            //if (DuyetXuLyModel.DanhSachHoSoTaiLieu != null && DuyetXuLyModel.DanhSachHoSoTaiLieu.Count > 0)
            //{
            //    foreach (var item in DuyetXuLyModel.DanhSachHoSoTaiLieu)
            //    {
            //        if (item.DanhSachFileDinhKemID != null && item.DanhSachFileDinhKemID.Count > 0)
            //        {
            //            new FileDinhKemDAL().UpdateFileXuLyDon(item.DanhSachFileDinhKemID, DuyetXuLyModel.XuLyDonID ?? 0, 0);
            //        }
            //    }

            //}
            #endregion

            return Result;
        }

        public IList<CanBoInfo> GetDanhSachLanhDao(IdentityHelper IdentityHelper)
        {
            var Data = new CanBo().GetDanhSachLanhDao();
            if (IdentityHelper.CoQuanID > 0 && Data.Count > 0)
            {
                var cq = new CoQuan().GetCoQuanByID(IdentityHelper.CoQuanID ?? 0);
                //if (IdentityHelper.RoleID == RoleEnum.LanhDao.GetHashCode())
                //{
                //    var cq = new CoQuan().GetCoQuanByID(IdentityHelper.CoQuanID ?? 0);
                //    Data = Data.Where(x => x.CoQuanID == cq.CoQuanChaID && (x.CanBoID != 20 && x.TenCanBo != "Administrator")).ToList();
                //}
                //else 

                if (IdentityHelper.CapHanhChinh == EnumCapHanhChinh.CapPhongThuocSo.GetHashCode())
                {
                    Data = new CanBo().GetDanhSachLanhDao_V2();

                    if (IdentityHelper.RoleID == RoleEnum.ChuyenVien.GetHashCode())
                    {
                        Data = Data.Where(x => x.CoQuanID == IdentityHelper.CoQuanID && (x.CanBoID != 20 && x.TenCanBo != "Administrator")).ToList();
                    }
                    else if (IdentityHelper.RoleID == RoleEnum.LanhDaoPhong.GetHashCode())// lãnh đạo phòng
                    {
                        Data = Data.Where(x => x.CoQuanID == cq.CoQuanChaID && x.RoleID == 1 && (x.CanBoID != 20 && x.TenCanBo != "Administrator")).ToList();
                    }
                    else if (IdentityHelper.RoleID == RoleEnum.LanhDao.GetHashCode())// lãnh đạo đơn vị
                    {
                        Data = Data.Where(x => x.CoQuanID == cq.CoQuanChaID && x.RoleID == 1 && (x.CanBoID != 20 && x.TenCanBo != "Administrator")).ToList();
                    }
                }
                else
                {
                    if (IdentityHelper.RoleID == RoleEnum.LanhDao.GetHashCode())
                    {
                        Data = Data.Where(x => (x.CoQuanID == IdentityHelper.CoQuanID || x.CoQuanID == IdentityHelper.CoQuanChaID) && (x.CanBoID != 20 && x.TenCanBo != "Administrator") && x.CanBoID != IdentityHelper.CanBoID).ToList();
                    }
                    else Data = Data.Where(x => x.CoQuanID == IdentityHelper.CoQuanID && (x.CanBoID != 20 && x.TenCanBo != "Administrator") && x.CanBoID != IdentityHelper.CanBoID).ToList();
                }
            }

            return Data;
        }
    }
}
