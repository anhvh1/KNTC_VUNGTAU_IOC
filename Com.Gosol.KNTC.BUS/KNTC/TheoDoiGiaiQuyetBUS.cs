using Com.Gosol.KNTC.DAL.KNTC;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Ultilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Gosol.KNTC.DAL.HeThong;
using Workflow;

namespace Com.Gosol.KNTC.BUS.KNTC
{
    public class TheoDoiGiaiQuyetBUS
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
                    if (IdentityHelper.CapID == CapQuanLy.CapUBNDHuyen.GetHashCode())
                    {
                        result = new DTXuLy().DTDuyetKQXL_LanhDao_Huyen(ref TotalRow, info, docIDList);
                    }
                    else
                    {
                        result = new DTXuLy().DTDuyetKQXL_ChuTich(ref TotalRow, info, docIDList);
                    }
                    //}

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
                        0,
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
                            if (item.TrinhDuThao >= 1)
                            {
                                item.TrangThai = "Đã duyệt";
                                //item.TrangThai = "Đã trình";
                                item.TrangThaiID = 3;
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
                                            item.TrangThai = "Chưa ban hành quyết định giao xác minh";
                                            item.TrangThaiID = 3;
                                        }
                                    }
                                    else
                                    {
                                        item.TrangThai = "Chưa ban hành quyết định giao xác minh";
                                        item.TrangThaiID = 3;
                                    }
                                }

                                if (item.TrinhDuThao == TrangThaiTrinhDuThao.DaDuyetQDXacMinh.GetHashCode())
                                {
                                    item.TrangThai = "Đang xác minh";
                                    item.TrangThaiID = 4;
                                    //item.TrangThai = "Chưa ban hành quyết định giải quyết";
                                    //item.TrangThaiID = 5;
                                    //if(item.ChuyenGiaiQuyetID > 0)
                                    //{
                                    //    item.TrangThai = "Đang xác minh";
                                    //    item.TrangThaiID = 4;
                                    //}
                                    if (item.StateID == 9)
                                    {
                                        item.TrangThai = "Chưa ban hành quyết định giải quyết";
                                        item.TrangThaiID = 5;
                                    }
                                }
                                if (item.TrinhDuThao == TrangThaiTrinhDuThao.DaDuyetQDGiaiQuyet.GetHashCode())
                                {
                                    item.TrangThai = "Đang thi hành quyết định giải quyết";
                                    item.TrangThaiID = 6;
                                }
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
                            if (item.TrinhDuThao > 1)
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

                    try
                    {
                        GiaoXacMinhModel giaoXacMinhModel = new GiaoXacMinhModel();
                        ChuyenGiaiQuyetInfo data = new ChuyenGiaiQuyet().GetChuyenGiaiQuyetCoQuanKhac(item.XuLyDonID);
                        if (data != null)
                        {
                            giaoXacMinhModel.XuLyDonID = item.XuLyDonID;
                            giaoXacMinhModel.CoQuanID = data.CoQuanGiaiQuyetID;
                            giaoXacMinhModel.GhiChu = data.GhiChu;
                            giaoXacMinhModel.CQPhoiHopGQ = new List<CQPhoiHopGQInfo>();
                        }
                        item.GiaoXacMinh = giaoXacMinhModel;
                    }
                    catch (Exception)
                    {
                    }

                }
            }
            return result;
        }

        public BaseResultModel BanHanhQuyetDinhGiaoXacMinh(DuyetXuLyModel DuyetXuLyModel)
        {
            var Result = new BaseResultModel();
            if (DuyetXuLyModel.XuLyDonID < 0)
            {
                Result.Status = 0;
                Result.Message = "Chưa chọn đơn thư cần duyệt";
                return Result;
            }
            int val = new XuLyDonDAL().BanHanhQuyetDinhXacMinh(DuyetXuLyModel.XuLyDonID, TrangThaiTrinhDuThao.DaDuyetQDXacMinh.GetHashCode(), DuyetXuLyModel.NoiDungBanHanh, DuyetXuLyModel.CanBoID, DateTime.Now);
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
            Result.Message = "Ban hành thành công";
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

        public BaseResultModel BanHanhQuyetDinhGiaiQuyet(IdentityHelper IdentityHelper, DuyetXuLyModel DuyetXuLyModel)
        {
            var Result = new BaseResultModel();
            if (DuyetXuLyModel.XuLyDonID < 0)
            {
                Result.Status = 0;
                Result.Message = "Chưa chọn đơn thư cần duyệt";
                return Result;
            }

            int val = new XuLyDonDAL().BanHanhQuyetDinhGiaiQuyet(DuyetXuLyModel.XuLyDonID, TrangThaiTrinhDuThao.DaDuyetQDGiaiQuyet.GetHashCode(), DuyetXuLyModel.NoiDungGiaiQuyet);

            BaoCaoXacMinhModel BaoCaoXacMinh = new BaoCaoXacMinhModel();
            BaoCaoXacMinh.XuLyDonID = DuyetXuLyModel.XuLyDonID;
            BaoCaoXacMinh.NoiDung = DuyetXuLyModel.NoiDungGiaiQuyet;

            DongYKetQuaGiaiQuyet(IdentityHelper, BaoCaoXacMinh);
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
            Result.Message = "Ban hành thành công";
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

        public int DongYKetQuaGiaiQuyet(IdentityHelper IdentityHelper, BaoCaoXacMinhModel BaoCaoXacMinh)
        {
            int TPTrinhKQGiaiQuyetLDCapDuoi = 0;
            int TPTrinhKQGiaiQuyetLDCapTren = 2;
            int LDCapDuoiDuyetGQ = 0;
            int LDCapDuoiKhongDuyetGQ = 1;
            int LDDuyetGQ = 0;
            int LDKhongDuyetGQ = 1;
            int LDKhongDuyetGQ_TraTP = 4;
            int LDPhanGQCapDuoi = 2;

            int coQuanPhanID = 0;
            int xuLyDonID = BaoCaoXacMinh.XuLyDonID ?? 0;
            DateTime hanGiaiQuyet = BaoCaoXacMinh.HanGiaiQuyet ?? DateTime.Now;
            string stateName = null;
            string commandCode = null;
            try
            {
                coQuanPhanID = new ChuyenGiaiQuyet().GetCoQuanChuyenGiaiQuyet(xuLyDonID, IdentityHelper.CoQuanID ?? 0);
            }
            catch
            {

            }

            stateName = WorkflowInstance.Instance.GetCurrentStateOfDocument(xuLyDonID);
            if (stateName == Constant.TP_DuyetGQ)
            {
                if (coQuanPhanID != 0)
                {
                    commandCode = WorkflowInstance.Instance.GetAvailabelCommands(xuLyDonID)[TPTrinhKQGiaiQuyetLDCapDuoi];
                }
                else
                {
                    commandCode = WorkflowInstance.Instance.GetAvailabelCommands(xuLyDonID)[TPTrinhKQGiaiQuyetLDCapTren];
                }
            }

            if (stateName == Constant.LD_CQCapDuoiDuyetGQ)
            {
                commandCode = WorkflowInstance.Instance.GetAvailabelCommands(xuLyDonID)[LDCapDuoiDuyetGQ];
            }
            if (stateName == Constant.LD_Duyet_GiaiQuyet)
            {
                commandCode = WorkflowInstance.Instance.GetAvailabelCommands(xuLyDonID)[LDDuyetGQ];
            }

            int canboid = IdentityHelper.CanBoID ?? 0;

            try
            {
                WorkflowInstance.Instance.ExecuteCommand(xuLyDonID, canboid, commandCode, hanGiaiQuyet, BaoCaoXacMinh.NoiDung ?? "");

                //String fileDataStr = fileUrl;
                //if (fileDataStr != string.Empty)
                //{
                //    string[] fileParts = fileDataStr.Split(';');
                //    for (int i = 0; i < fileParts.Length; i++)
                //    {
                //        string fileStr = fileParts[i];
                //        string[] dataParts = fileStr.Split(',');

                //        FileHoSoInfo info = new FileHoSoInfo();
                //        info.XuLyDonID = xuLyDonID;
                //        info.FileURL = dataParts[0];
                //        info.NgayUp = Utils.ConvertToDateTime(dataParts[1], Constant.DEFAULT_DATE);
                //        info.TenFile = dataParts[2];
                //        info.TomTat = dataParts[3];
                //        info.NguoiUp = canboid;
                //        info.FileID = Utils.ConvertToInt32(dataParts[6], 0);
                //        FileLogInfo infoFileLog = new FileLogInfo();
                //        infoFileLog.LoaiLog = (int)EnumLoaiLog.Them;
                //        infoFileLog.LoaiFile = (int)EnumLoaiFile.FileDTCDGQ;
                //        int isBaoMat = Utils.ConvertToInt32(dataParts[4], 0);
                //        if (isBaoMat == (int)EnumDoBaoMat.BaoMat)
                //        {
                //            infoFileLog.IsBaoMat = true;
                //            infoFileLog.IsMaHoa = true;
                //        }
                //        else
                //        {
                //            infoFileLog.IsBaoMat = false;
                //            infoFileLog.IsMaHoa = false;
                //        }

                //        try
                //        {
                //            int result = new FileHoSo().InsertDonThuCDGQ(info);
                //            if (result > 0)
                //            {
                //                infoFileLog.FileID = result;
                //                new FileLog().Insert(infoFileLog);
                //            }

                //        }
                //        catch
                //        {
                //        }
                //    }
                //}
            }
            catch
            {
            }

            return xuLyDonID;

        }

    }
}
