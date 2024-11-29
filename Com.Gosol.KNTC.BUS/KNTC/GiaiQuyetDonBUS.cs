using Com.Gosol.KNTC.DAL.KNTC;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Ultilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workflow;
using Utils = Com.Gosol.KNTC.Ultilities.Utils;
using Com.Gosol.KNTC.Model.HeThong;
using Com.Gosol.KNTC.Models.HeThong;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System.Configuration;
using System.Runtime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System.Security.Cryptography;
using Com.Gosol.KNTC.Security;
using Com.Gosol.KNTC.DAL.HeThong;
using DocumentFormat.OpenXml.Bibliography;
using System.Data.SqlClient;
using System.Data;

namespace Com.Gosol.KNTC.BUS.KNTC
{
    public class GiaiQuyetDonBUS
    {
        public IList<DonThuGiaiQuyetInfo> GetBySearch(ref int TotalRow, XuLyDonParamsForFilter p, IdentityHelper IdentityHelper)
        {
            QueryFilterInfo queryFilter = new QueryFilterInfo();
            queryFilter.CoQuanID = IdentityHelper.CoQuanID ?? 0;
            queryFilter.KeyWord = p.Keyword;
            int _currentPage = p.PageNumber;
            queryFilter.Start = (_currentPage - 1) * p.PageSize;
            queryFilter.End = _currentPage * p.PageSize;
            queryFilter.LoaiKhieuToID = p.LoaiKhieuToID ?? 0;
            queryFilter.StateName = Constant.LD_Phan_GiaiQuyet;
            queryFilter.CanBoID = IdentityHelper.CanBoID ?? 0;
            queryFilter.TrangThai = p.TrangThai;
            queryFilter.TrangThaiDonThu = p.TrangThaiDonThu;

            bool capUBND = IdentityHelper.CapUBND ?? false;
            bool qtGQPhucTap = IdentityHelper.SuDungQuyTrinhGQPhucTap ?? false;

            if (IdentityHelper.CapUBND == true)
            {
                queryFilter.TuNgayGoc = p.TuNgay ?? DateTime.MinValue;
                queryFilter.DenNgayGoc = p.DenNgay ?? DateTime.MinValue;
                queryFilter.TuNgayMoi = Constant.DEFAULT_DATE;
                queryFilter.DenNgayMoi = Constant.DEFAULT_DATE;
            }
            else
            {
                queryFilter.TuNgayGoc = p.TuNgay ?? DateTime.MinValue;
                queryFilter.DenNgayGoc = p.DenNgay ?? DateTime.MinValue;
                queryFilter.TuNgayMoi = p.TuNgay ?? DateTime.MinValue;
                queryFilter.DenNgayMoi = p.DenNgay ?? DateTime.MinValue;
            }

            if (IdentityHelper.SuDungQuyTrinhGQPhucTap == true)
            {
                queryFilter.CanBoID = IdentityHelper.CanBoID ?? 0;
            }

            IList<DonThuGiaiQuyetInfo> donThuList = new List<DonThuGiaiQuyetInfo>();
            #region role lanh dao
            if (IdentityHelper.RoleID == RoleEnum.LanhDao.GetHashCode())
            {
                try
                {
                    if (p.LocDonPhanHoi == true)
                    {
                        donThuList = new DonThuGiaiQuyet().GetDonThuCanPhanGiaiQuyet_PhanHoi_filter(queryFilter, capUBND, qtGQPhucTap);
                    }
                    else
                    {
                        donThuList = new DonThuGiaiQuyet().GetDonThuCanPhanGiaiQuyet_PhanHoi(queryFilter, ref TotalRow);

                    }
                }
                catch
                {
                }

                foreach (var donThuInfo in donThuList)
                {
                    if (donThuInfo.CoQuanID != IdentityHelper.CoQuanID)
                    {
                        donThuInfo.NgayQuaHan = donThuInfo.HanGQMoi;
                    }
                    else
                    {
                        if (capUBND)
                        {
                            donThuInfo.NgayQuaHan = donThuInfo.HanGQMoi;
                        }
                        else
                        {
                            if (qtGQPhucTap)
                            {
                                donThuInfo.NgayQuaHan = donThuInfo.HanGQQTPhucTap;
                            }
                            else
                            {
                                donThuInfo.NgayQuaHan = donThuInfo.HanGQGoc;
                            }
                        }
                    }
                    if (donThuInfo.NgayQuaHan != DateTime.MinValue)
                    {
                        donThuInfo.HanXuLy = donThuInfo.NgayQuaHan;
                    }

                    donThuInfo.NgayQuaHanStr = Format.FormatDate(donThuInfo.NgayQuaHan);

                    if (donThuInfo.NgayQuaHan != DateTime.MinValue)
                        donThuInfo.SoNgayConLai = donThuInfo.NgayQuaHan.Subtract(DateTime.Now).Days;
                    else
                        donThuInfo.SoNgayConLai = 5;

                }
            }
            #endregion

            #region role truong phong
            if (IdentityHelper.RoleID == RoleEnum.LanhDaoPhong.GetHashCode())
            {
                queryFilter.PhongBanID = IdentityHelper.PhongBanID ?? 0;
                queryFilter.TuNgay = p.TuNgay ?? DateTime.MinValue;
                queryFilter.DenNgay = p.DenNgay ?? DateTime.MinValue;

                try
                {
                    //donThuList = new DonThuGiaiQuyet().GetDonThuCanPhanGiaiQuyet_TP(queryFilter);
                    if (p.LocDonPhanHoi == true)
                    {
                        donThuList = new DonThuGiaiQuyet().GetDonThuCanPhanGiaiQuyet_TP_PhanHoi_filter(queryFilter);
                    }
                    else
                    {
                        donThuList = new DonThuGiaiQuyet().GetDonThuCanPhanGiaiQuyet_TP_PhanHoi(queryFilter, ref TotalRow);

                    }
                }
                catch
                {
                }
            }
            #endregion

            #region trang thai
            if (donThuList != null)
            {
                foreach (var donThuInfo in donThuList)
                {
                    var renderTrangThai = new RenderTrangThai();
                    renderTrangThai.GetTrangThai(
                        donThuInfo.LoaiQuyTrinh,
                        donThuInfo.HuongGiaiQuyetID,
                        donThuInfo.StateName,
                        donThuInfo.StateID,
                        0,
                        donThuInfo.TrangThaiDuyet ?? 0,
                        donThuInfo.TrinhDuThao,
                        IdentityHelper,
                        donThuInfo.NgayCapNhat,
                        donThuInfo.ChuyenGiaiQuyetID,
                        donThuInfo.KetQuaID,
                        donThuInfo.LanhDaoDuyet2ID,
                        donThuInfo.RutDonID
                        );
                    donThuInfo.TrangThaiMoi = renderTrangThai.TrangThaiMoi;
                    donThuInfo.TrangThaiIDMoi = renderTrangThai.TrangThaiIDMoi;
                    donThuInfo.CheckTrangThai = renderTrangThai.CheckTrangThai;


                    //if (IdentityHelper.CapUBND == true)
                    //{

                    if (donThuInfo.StateID == 7 || donThuInfo.StateID == 18)
                    {
                        donThuInfo.TenTrangThai = "Chưa giao xác minh";
                        donThuInfo.TrangThai = 0;

                        //if(IdentityHelper.RoleID == RoleEnum.LanhDao.GetHashCode())
                        //{

                        //}
                    }
                    else
                    {

                        //donThuInfo.TenTrangThai = "Đã giao";
                        donThuInfo.TenTrangThai = "Đang xác minh";
                        donThuInfo.TrangThai = 2;
                    }
                    //}
                    if (IdentityHelper.RoleID == RoleEnum.LanhDaoPhong.GetHashCode() && donThuInfo.StateID == 19
                        // tuandhh bổ sung cấp lãnh đạo phòng chỉ dành cho btd tỉnh, btd huyện lãnh đạo phân cho ai thì cấp đó xác minh
                        && IdentityHelper.CapHanhChinh != EnumCapHanhChinh.CapPhongThuocHuyen.GetHashCode())
                    {
                        donThuInfo.TenTrangThai = "Chưa giao xác minh";
                        donThuInfo.TrangThai = 0;
                    }

                    // tuandhh bổ sung quy trình cấp huyện: giao cho lãnh đạo phòng thuộc huyện (đợt thay đổi bổ sung nghiệp vụ btd cấp huyện)
                    if (IdentityHelper.CapHanhChinh == EnumCapHanhChinh.CapPhongThuocHuyen.GetHashCode() && IdentityHelper.RoleID == RoleEnum.LanhDao.GetHashCode() && donThuInfo.StateID == 19)
                    {
                        donThuInfo.TenTrangThai = "Chưa giao xác minh";
                        donThuInfo.TrangThai = 0;
                    }


                    if (IdentityHelper.CapThanhTra == 1)
                    {
                        donThuInfo.TenTrangThai = "Chưa giao xác minh";
                        donThuInfo.TrangThai = 0;

                        if (donThuInfo.StateID == 7 || donThuInfo.StateID == 18)
                        {

                            if (donThuInfo.SoNgayConLai < 5)
                            {
                                //quaHanTr = "quahan";
                            }
                            else
                            {
                                //quaHanTr = "cangiao";
                            }
                        }
                        else
                        {
                            if (donThuInfo.StateID == 19 && IdentityHelper.RoleID == 2)
                            {

                                donThuInfo.TenTrangThai = "Chưa giao xác minh";
                                donThuInfo.TrangThai = 0;

                                if (donThuInfo.SoNgayConLai < 5)
                                {
                                    //quaHanTr = "quahan";
                                }
                                else
                                {
                                    //quaHanTr = "cangiao";
                                }

                            }
                            else
                            {

                                //donThuInfo.TenTrangThai = "Đã giao";
                                donThuInfo.TenTrangThai = "Đang xác minh";
                                donThuInfo.TrangThai = 2;
                                if (donThuInfo.StateID == 8)
                                {
                                    if (donThuInfo.SoNgayConLai < 5)
                                    {
                                        //quaHanTr = "quahan";
                                    }
                                    else
                                    {
                                        //quaHanTr = "";
                                    }
                                }
                            }

                            //if (roleID == 1)
                            //{
                            //    if (json[i].StateID == 21 || json[i].StateID == 19)
                            //    {
                            //        if (json[i].SoNgayConLai < 5)
                            //        {
                            //            quaHanTr = "quahan";
                            //        }
                            //        else
                            //        {
                            //            quaHanTr = "";
                            //        }
                            //    }
                            //}

                        }
                    }


                    if (donThuInfo.StateID == 21)
                    {
                        if (IdentityHelper.RoleID == RoleEnum.LanhDaoPhong.GetHashCode()
                            || (IdentityHelper.CapID == CapQuanLy.CapUBNDXa.GetHashCode() && IdentityHelper.RoleID == RoleEnum.LanhDao.GetHashCode())
                            || (IdentityHelper.CapHanhChinh == EnumCapHanhChinh.CapPhongThuocHuyen.GetHashCode() && IdentityHelper.RoleID == RoleEnum.LanhDao.GetHashCode())
                            )
                        {
                            donThuInfo.TenTrangThai = "Chưa duyệt báo cáo xác minh";
                            donThuInfo.TrangThai = 4;

                            //if (json[i].NgayGQConLai < 5)
                            //{
                            //    quaHanTr = "quahan";
                            //}
                            //else
                            //{
                            //    quaHanTr = "";
                            //}
                        }
                        else
                        {
                            //tinhtrang = "Đã duyệt";
                            donThuInfo.TenTrangThai = "Đã trình báo cáo xác minh";
                            donThuInfo.TrangThai = 5;
                        }
                    }
                    else
                    {
                        if (donThuInfo.StateID == 22)
                        {
                            if (IdentityHelper.RoleID == 1)
                            {
                                //if (donThuInfo.ChuyenGiaiQuyetID != 0)
                                //{
                                donThuInfo.TenTrangThai = "Chưa duyệt báo cáo xác minh";
                                donThuInfo.TrangThai = 4;
                                //if (json[i].NgayGQConLai < 5)
                                //{
                                //    quaHanTr = "quahan";
                                //}
                                //else
                                //{
                                //    quaHanTr = "";
                                //}
                                //}
                                //else
                                //{
                                //    donThuInfo.TenTrangThai = "Đã trình báo cáo xác minh";
                                //    donThuInfo.TrangThai = 5;

                                //}
                            }
                            else
                            {
                                donThuInfo.TenTrangThai = "Đã trình báo cáo xác minh";
                                donThuInfo.TrangThai = 5;

                            }
                        }
                        else if (donThuInfo.StateID == 9)
                        {
                            //if (IdentityHelper.RoleID == 1 && IdentityHelper.CapID == CapQuanLy.CapUBNDXa.GetHashCode() 
                            //    || IdentityHelper.RoleID == 1 && IdentityHelper.CapID == CapQuanLy.CapUBNDHuyen.GetHashCode()
                            //    || IdentityHelper.RoleID == 1 && IdentityHelper.CapID == CapQuanLy.CapPhong.GetHashCode())
                            if (IdentityHelper.RoleID == 1)
                            {
                                //if (donThuInfo.ChuyenGiaiQuyetID == 0)
                                //{  
                                donThuInfo.TenTrangThai = "Chưa duyệt báo cáo xác minh";
                                donThuInfo.TrangThai = 4;
                                if (donThuInfo.CoQuanID != IdentityHelper.CoQuanID)
                                {
                                    donThuInfo.TenTrangThai = "Đã trình báo cáo xác minh";
                                    donThuInfo.TrangThai = 5;
                                }
                                //    //donThuInfo.TenTrangThai = "Đã trình báo cáo xác minh";
                                //    //donThuInfo.TrangThai = 5;

                                //    //if (json[i].NgayGQConLai < 5)
                                //    //{
                                //    //    quaHanTr = "quahan";
                                //    //}
                                //    //else
                                //    //{
                                //    //    quaHanTr = "";
                                //    //}
                                //}
                                //else
                                //{
                                //donThuInfo.TenTrangThai = "Đã trình báo cáo xác minh";
                                //    donThuInfo.TrangThai = 5;

                                //}

                            }
                            else
                            {
                                donThuInfo.TenTrangThai = "Đã trình báo cáo xác minh";
                                donThuInfo.TrangThai = 5;

                            }
                        }
                        else
                        {
                            //donThuInfo.TenTrangThai = "Đã trình báo cáo xác minh";
                            //donThuInfo.TrangThai = 5;

                        }

                    }

                    if (donThuInfo.StateID == 10)
                    {
                        donThuInfo.TenTrangThai = "Chưa ban hành quyết định giải quyết";
                        donThuInfo.TrangThai = 6;
                    }

                    if (donThuInfo.KetQuaID > 0)
                    {
                        donThuInfo.TenTrangThai = "Đã ban hành quyết định giải quyết";
                        donThuInfo.TrangThai = 7;
                    }
                    //else {
                    //    donThuInfo.TenTrangThai = "Chưa ban hành quyết định giải quyết";
                    //    donThuInfo.TrangThai = 6;
                    //}
                }
            }
            #endregion

            #region role chuyen vien
            if (IdentityHelper.RoleID == RoleEnum.ChuyenVien.GetHashCode())
            {
                //queryFilter.PhongBanID = IdentityHelper.GetPhongID();
                //queryFilter.TuNgay = Utils.ConvertToDateTime(txtTuNgay, DateTime.MinValue);
                //queryFilter.DenNgay = Utils.ConvertToDateTime(txtDenNgay, DateTime.MinValue);

                //try
                //{
                //    //donThuList = new DonThuGiaiQuyet().GetDonThuCanPhanGiaiQuyet_CV(queryFilter);
                //    if (p.LocDonPhanHoi == true)
                //    {
                //        donThuList = new DonThuGiaiQuyet().GetDonThuCanPhanGiaiQuyet_CV_PhanHoi_filter(queryFilter);
                //    }
                //    else
                //    {
                //        donThuList = new DonThuGiaiQuyet().GetDonThuCanPhanGiaiQuyet_CV_PhanHoi(queryFilter);

                //    }
                //}
                //catch
                //{
                //}

                //foreach (var donThuInfo in donThuList)
                //{
                //    if (donThuInfo.CoQuanID != IdentityHelper.CoQuanID)
                //    {
                //        donThuInfo.NgayQuaHan = donThuInfo.HanGQMoi;
                //    }
                //    else
                //        donThuInfo.NgayQuaHan = donThuInfo.HanGQGoc;

                //    donThuInfo.NgayQuaHanStr = Format.FormatDate(donThuInfo.NgayQuaHan);

                //    if (donThuInfo.NgayQuaHan != DateTime.MinValue)
                //        donThuInfo.SoNgayConLai = donThuInfo.NgayQuaHan.Subtract(DateTime.Now).Days;
                //    else
                //        donThuInfo.SoNgayConLai = 5;
                //}

                //QueryFilterInfo info = new QueryFilterInfo();
                //info.CoQuanID = IdentityHelper.GetCoQuanID();
                //info.KeyWord = txtSearch;
                //int _currentPage = Convert.ToInt32(currentPage);
                //info.Start = (_currentPage - 1) * IdentityHelper.GetPageSize();
                //info.End = _currentPage * IdentityHelper.GetPageSize();
                //info.LoaiKhieuToID = Convert.ToInt32(LoaiKhieuToId);
                //info.TuNgay = Utils.ConvertToDateTime(txtTuNgay, DateTime.MinValue);
                //info.DenNgay = Utils.ConvertToDateTime(txtDenNgay, DateTime.MinValue);

                donThuList = new DonThuGiaiQuyet().GetDonThuCanGiaiQuyet_PhanHoi(queryFilter, IdentityHelper.CanBoID ?? 0, ref TotalRow);
                foreach (var item in donThuList)
                {
                    DonThuInfo donInfo = new DonThuDAL().GetByID(item.DonThuID);
                    if (donInfo.NhomKNID > 0)
                    {
                        item.listDoiTuongKN = new DoiTuongKN().GetByNhomKNID(donInfo.NhomKNID).ToList();
                    }
                }
                if (donThuList != null)
                {
                    foreach (var donThuInfo in donThuList)
                    {
                        var renderTrangThai = new RenderTrangThai();
                        renderTrangThai.GetTrangThai(
                            donThuInfo.LoaiQuyTrinh,
                            donThuInfo.HuongGiaiQuyetID,
                            donThuInfo.StateName,
                            donThuInfo.StateID,
                            0,
                            donThuInfo.TrangThaiDuyet ?? 0,
                            donThuInfo.TrinhDuThao,
                            IdentityHelper,
                            donThuInfo.NgayCapNhat,
                            donThuInfo.ChuyenGiaiQuyetID,
                            donThuInfo.KetQuaID,
                            0,
                            donThuInfo.RutDonID
                            );
                        donThuInfo.TrangThaiMoi = renderTrangThai.TrangThaiMoi;
                        donThuInfo.TrangThaiIDMoi = renderTrangThai.TrangThaiIDMoi;
                        donThuInfo.CheckTrangThai = renderTrangThai.CheckTrangThai;

                        if (donThuInfo.StateID == 8 || (donThuInfo.StateID == 19 && IdentityHelper.CapHanhChinh == EnumCapHanhChinh.CapPhongThuocHuyen.GetHashCode() && !(IdentityHelper.BanTiepDan ?? false)))
                        {
                            if (donThuInfo.NgayCapNhatStr != "")
                            {
                                donThuInfo.TenTrangThai = "Đang xác minh";
                                donThuInfo.TrangThai = 2;

                                if (donThuInfo.NgayGQConLai < 5)
                                {
                                    //quaHanTr = "quahan";
                                }
                                else
                                {
                                    //quaHanTr = "";
                                }

                            }
                            if (donThuInfo.NgayCapNhatStr == "")
                            {
                                donThuInfo.TenTrangThai = "Chưa xác minh";
                                donThuInfo.TrangThai = 1;
                                if (donThuInfo.NgayGQConLai < 5)
                                {
                                    //quaHanTr = "quahan";
                                }
                                else
                                {
                                    //quaHanTr = "";
                                }
                            }

                        }
                        else if (donThuInfo.StateID > 8)
                        {
                            donThuInfo.TenTrangThai = "Đã trình báo cáo xác minh";
                            donThuInfo.TrangThai = 5;
                        }
                    }
                }

            }
            #endregion

            return donThuList;
        }

        public IList<DonThuGiaiQuyetInfo> GetBySearch_QuyTrinhDonGian(ref int TotalRow, XuLyDonParamsForFilter p, IdentityHelper IdentityHelper)
        {
            QueryFilterInfo queryFilter = new QueryFilterInfo();
            queryFilter.CoQuanID = IdentityHelper.CoQuanID ?? 0;
            queryFilter.KeyWord = p.Keyword;
            int _currentPage = p.PageNumber;
            queryFilter.Start = (_currentPage - 1) * p.PageSize;
            queryFilter.End = _currentPage * p.PageSize;
            queryFilter.LoaiKhieuToID = p.LoaiKhieuToID ?? 0;
            queryFilter.StateName = Constant.LD_Phan_GiaiQuyet;
            queryFilter.CanBoID = IdentityHelper.CanBoID ?? 0;
            queryFilter.TrangThai = p.TrangThai;

            bool capUBND = IdentityHelper.CapUBND ?? false;
            bool qtGQPhucTap = IdentityHelper.SuDungQuyTrinhGQPhucTap ?? false;

            if (IdentityHelper.CapUBND == true)
            {
                queryFilter.TuNgayGoc = p.TuNgay ?? DateTime.MinValue;
                queryFilter.DenNgayGoc = p.DenNgay ?? DateTime.MinValue;
                queryFilter.TuNgayMoi = Constant.DEFAULT_DATE;
                queryFilter.DenNgayMoi = Constant.DEFAULT_DATE;
            }
            else
            {
                queryFilter.TuNgayGoc = p.TuNgay ?? DateTime.MinValue;
                queryFilter.DenNgayGoc = p.DenNgay ?? DateTime.MinValue;
                queryFilter.TuNgayMoi = p.TuNgay ?? DateTime.MinValue;
                queryFilter.DenNgayMoi = p.DenNgay ?? DateTime.MinValue;
            }

            //if (IdentityHelper.SuDungQuyTrinhGQPhucTap == true)
            //{
            queryFilter.CanBoID = 0;
            //}

            IList<DonThuGiaiQuyetInfo> donThuList = new List<DonThuGiaiQuyetInfo>();
            try
            {
                if (p.LocDonPhanHoi == true)
                {
                    donThuList = new DonThuGiaiQuyet().GetDonThuCanPhanGiaiQuyet_PhanHoi_filter(queryFilter, capUBND, qtGQPhucTap);
                }
                else
                {
                    donThuList = new DonThuGiaiQuyet().GetDonThuCanPhanGiaiQuyet_PhanHoi(queryFilter, ref TotalRow);

                }
            }
            catch
            {
            }

            foreach (var donThuInfo in donThuList)
            {
                if (donThuInfo.CoQuanID != IdentityHelper.CoQuanID)
                {
                    donThuInfo.NgayQuaHan = donThuInfo.HanGQMoi;
                }
                else
                {
                    if (capUBND)
                    {
                        donThuInfo.NgayQuaHan = donThuInfo.HanGQMoi;
                    }
                    else
                    {
                        if (qtGQPhucTap)
                        {
                            donThuInfo.NgayQuaHan = donThuInfo.HanGQQTPhucTap;
                        }
                        else
                        {
                            donThuInfo.NgayQuaHan = donThuInfo.HanGQGoc;
                        }
                    }
                }
                if (donThuInfo.NgayQuaHan != DateTime.MinValue)
                {
                    donThuInfo.HanXuLy = donThuInfo.NgayQuaHan;
                }

                donThuInfo.NgayQuaHanStr = Format.FormatDate(donThuInfo.NgayQuaHan);

                if (donThuInfo.NgayQuaHan != DateTime.MinValue)
                    donThuInfo.SoNgayConLai = donThuInfo.NgayQuaHan.Subtract(DateTime.Now).Days;
                else
                    donThuInfo.SoNgayConLai = 5;

            }

            #region trang thai
            if (donThuList != null)
            {
                foreach (var donThuInfo in donThuList)
                {
                    //if (IdentityHelper.CapUBND == true)
                    //{

                    if (donThuInfo.StateID == 7 || donThuInfo.StateID == 18)
                    {
                        donThuInfo.TenTrangThai = "Chưa giao xác minh";
                        donThuInfo.TrangThai = 0;

                        //if(IdentityHelper.RoleID == RoleEnum.LanhDao.GetHashCode())
                        //{

                        //}
                    }
                    else
                    {
                        //donThuInfo.TenTrangThai = "Đã giao";
                        donThuInfo.TenTrangThai = "Đang xác minh";
                        donThuInfo.TrangThai = 2;
                    }
                    //}
                    if (IdentityHelper.RoleID == RoleEnum.LanhDaoPhong.GetHashCode() && donThuInfo.StateID == 19)
                    {
                        donThuInfo.TenTrangThai = "Chưa giao xác minh";
                        donThuInfo.TrangThai = 0;
                    }

                    if (IdentityHelper.CapThanhTra == 1)
                    {
                        donThuInfo.TenTrangThai = "Chưa giao xác minh";
                        donThuInfo.TrangThai = 0;

                        if (donThuInfo.StateID == 7 || donThuInfo.StateID == 18)
                        {

                            if (donThuInfo.SoNgayConLai < 5)
                            {
                                //quaHanTr = "quahan";
                            }
                            else
                            {
                                //quaHanTr = "cangiao";
                            }
                        }
                        else
                        {
                            if (donThuInfo.StateID == 19 && IdentityHelper.RoleID == 2)
                            {

                                donThuInfo.TenTrangThai = "Chưa giao xác minh";
                                donThuInfo.TrangThai = 0;

                                if (donThuInfo.SoNgayConLai < 5)
                                {
                                    //quaHanTr = "quahan";
                                }
                                else
                                {
                                    //quaHanTr = "cangiao";
                                }

                            }
                            else
                            {

                                //donThuInfo.TenTrangThai = "Đã giao";
                                donThuInfo.TenTrangThai = "Đang xác minh";
                                donThuInfo.TrangThai = 2;
                                if (donThuInfo.StateID == 8)
                                {
                                    if (donThuInfo.SoNgayConLai < 5)
                                    {
                                        //quaHanTr = "quahan";
                                    }
                                    else
                                    {
                                        //quaHanTr = "";
                                    }
                                }
                            }

                            //if (roleID == 1)
                            //{
                            //    if (json[i].StateID == 21 || json[i].StateID == 19)
                            //    {
                            //        if (json[i].SoNgayConLai < 5)
                            //        {
                            //            quaHanTr = "quahan";
                            //        }
                            //        else
                            //        {
                            //            quaHanTr = "";
                            //        }
                            //    }
                            //}

                        }
                    }


                    if (donThuInfo.StateID == 21)
                    {
                        if (IdentityHelper.RoleID == RoleEnum.LanhDaoPhong.GetHashCode())
                        {
                            donThuInfo.TenTrangThai = "Chưa duyệt báo cáo xác minh";
                            donThuInfo.TrangThai = 4;

                            //if (json[i].NgayGQConLai < 5)
                            //{
                            //    quaHanTr = "quahan";
                            //}
                            //else
                            //{
                            //    quaHanTr = "";
                            //}
                        }
                        else
                        {
                            //tinhtrang = "Đã duyệt";
                            donThuInfo.TenTrangThai = "Đã trình báo cáo xác minh";
                            donThuInfo.TrangThai = 5;

                        }
                    }
                    else
                    {
                        if (donThuInfo.StateID == 22)
                        {
                            if (IdentityHelper.RoleID == 1)
                            {
                                //if (donThuInfo.ChuyenGiaiQuyetID != 0)
                                //{
                                donThuInfo.TenTrangThai = "Chưa duyệt báo cáo xác minh";
                                donThuInfo.TrangThai = 4;
                                //if (json[i].NgayGQConLai < 5)
                                //{
                                //    quaHanTr = "quahan";
                                //}
                                //else
                                //{
                                //    quaHanTr = "";
                                //}
                                //}
                                //else
                                //{
                                //    donThuInfo.TenTrangThai = "Đã trình báo cáo xác minh";
                                //    donThuInfo.TrangThai = 5;

                                //}
                            }
                            else
                            {
                                donThuInfo.TenTrangThai = "Đã trình báo cáo xác minh";
                                donThuInfo.TrangThai = 5;

                            }
                        }
                        else if (donThuInfo.StateID == 9)
                        {
                            //if (IdentityHelper.RoleID == 1 && IdentityHelper.CapID == CapQuanLy.CapUBNDXa.GetHashCode() 
                            //    || IdentityHelper.RoleID == 1 && IdentityHelper.CapID == CapQuanLy.CapUBNDHuyen.GetHashCode()
                            //    || IdentityHelper.RoleID == 1 && IdentityHelper.CapID == CapQuanLy.CapPhong.GetHashCode())
                            if (IdentityHelper.RoleID == 1)
                            {
                                //if (donThuInfo.ChuyenGiaiQuyetID == 0)
                                //{  
                                donThuInfo.TenTrangThai = "Chưa duyệt báo cáo xác minh";
                                donThuInfo.TrangThai = 4;
                                if (donThuInfo.CoQuanID != IdentityHelper.CoQuanID)
                                {
                                    donThuInfo.TenTrangThai = "Đã trình báo cáo xác minh";
                                    donThuInfo.TrangThai = 5;
                                }
                                //    //donThuInfo.TenTrangThai = "Đã trình báo cáo xác minh";
                                //    //donThuInfo.TrangThai = 5;

                                //    //if (json[i].NgayGQConLai < 5)
                                //    //{
                                //    //    quaHanTr = "quahan";
                                //    //}
                                //    //else
                                //    //{
                                //    //    quaHanTr = "";
                                //    //}
                                //}
                                //else
                                //{
                                //donThuInfo.TenTrangThai = "Đã trình báo cáo xác minh";
                                //    donThuInfo.TrangThai = 5;

                                //}

                            }
                            else
                            {
                                donThuInfo.TenTrangThai = "Đã trình báo cáo xác minh";
                                donThuInfo.TrangThai = 5;

                            }
                        }
                        else
                        {
                            //donThuInfo.TenTrangThai = "Đã trình báo cáo xác minh";
                            //donThuInfo.TrangThai = 5;

                        }

                    }

                    if (donThuInfo.StateID == 10)
                    {
                        donThuInfo.TenTrangThai = "Chưa ban hành quyết định giải quyết";
                        donThuInfo.TrangThai = 6;
                    }

                    if (donThuInfo.KetQuaID > 0)
                    {
                        donThuInfo.TenTrangThai = "Đã ban hành quyết định giải quyết";
                        donThuInfo.TrangThai = 7;
                    }
                    //else {
                    //    donThuInfo.TenTrangThai = "Chưa ban hành quyết định giải quyết";
                    //    donThuInfo.TrangThai = 6;
                    //}
                }
            }
            #endregion

            return donThuList;
        }

        public BaseResultModel GiaoXacMinh(IdentityHelper IdentityHelper, GiaoXacMinhModel GiaoXacMinhModel)
        {
            var Result = new BaseResultModel();
            int ChuyenGiaiQuyetID = InsertHistory(IdentityHelper, GiaoXacMinhModel);
            InsertUpdatePhanGiaiQuyetArr(GiaoXacMinhModel, ChuyenGiaiQuyetID);
            try
            {
                if (GiaoXacMinhModel.DanhSachHoSoTaiLieu != null && GiaoXacMinhModel.DanhSachHoSoTaiLieu.Count > 0)
                {
                    foreach (var item in GiaoXacMinhModel.DanhSachHoSoTaiLieu)
                    {
                        if (item.DanhSachFileDinhKemID != null && item.DanhSachFileDinhKemID.Count > 0)
                        {
                            new FileDinhKemDAL().UpdateFileGiaoXacMinh(item.DanhSachFileDinhKemID, ChuyenGiaiQuyetID);
                        }
                    }
                }
            }
            catch (Exception)
            {

            }

            try
            {
                //update quy trinh gqd
                if (IdentityHelper.SuDungQuyTrinhGQPhucTap == true)
                {
                    new XuLyDonDAL().UpdateQuyTrinh(GiaoXacMinhModel.XuLyDonID, null, 1);
                }
            }
            catch (Exception)
            {
            }

            Result.Status = 1;
            Result.Data = ChuyenGiaiQuyetID;
            Result.Message = "Giao xác minh thành công";
            return Result;
        }

        public BaseResultModel CapNhapDoanToXacMinh(IdentityHelper IdentityHelper, GiaoXacMinhModel GiaoXacMinhModel)
        {
            var Result = new BaseResultModel();

            var listChuyenGiaiQuyet = new ChuyenGiaiQuyet().GetListChuyenGiaiQuyet(GiaoXacMinhModel.XuLyDonID ?? 0);
            if (listChuyenGiaiQuyet.Any())
            {
                new ChuyenGiaiQuyet().DeleteByChuyenGiaiQuyetID(listChuyenGiaiQuyet.OrderByDescending(x => x.ChuyenGiaiQuyetID).FirstOrDefault().ChuyenGiaiQuyetID);
            }

            int coquanid = IdentityHelper.CoQuanID ?? 0;
            ChuyenGiaiQuyetInfo cgqinfo = new ChuyenGiaiQuyetInfo();
            cgqinfo.CoQuanGiaiQuyetID = coquanid;
            cgqinfo.CoQuanPhanID = coquanid;
            cgqinfo.NgayChuyen = DateTime.Now;
            cgqinfo.XuLyDonID = GiaoXacMinhModel.XuLyDonID ?? 0;
            cgqinfo.GhiChu = GiaoXacMinhModel.GhiChu;
            cgqinfo.FileUrl = string.Empty;

            int chuyenGiaiQuyetID = 0;
            try
            {
                chuyenGiaiQuyetID = new ChuyenGiaiQuyet().Insert(cgqinfo);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Data = 0;
                Result.Message = "Cập nhập đoàn tổ xác minh thất bại";
                Result.MessageDetail = ex.Message;
                return Result;
            }

            if (chuyenGiaiQuyetID > 0)
            {
                InsertUpdatePhanGiaiQuyetArr(GiaoXacMinhModel, (int)chuyenGiaiQuyetID);

                try
                {
                    if (GiaoXacMinhModel.DanhSachHoSoTaiLieu != null && GiaoXacMinhModel.DanhSachHoSoTaiLieu.Count > 0)
                    {
                        foreach (var item in GiaoXacMinhModel.DanhSachHoSoTaiLieu)
                        {
                            if (item.DanhSachFileDinhKemID != null && item.DanhSachFileDinhKemID.Count > 0)
                            {
                                new FileDinhKemDAL().UpdateFileGiaoXacMinh(item.DanhSachFileDinhKemID, (int)chuyenGiaiQuyetID);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Result.Status = -1;
                    Result.Data = 0;
                    Result.Message = "Cập nhập đoàn tổ xác minh thất bại";
                    Result.MessageDetail = ex.Message;
                    return Result;
                }

                Result.Status = 1;
                Result.Data = 0;
                Result.Message = "Cập nhập đoàn tổ xác minh thành công";
                return Result;
            }
            Result.Status = -1;
            Result.Data = 0;
            Result.Message = "Cập nhập đoàn tổ xác minh thất bại";
            Result.MessageDetail = "Không insert được cơ quan chuyển giải quyết";
            return Result;
        }



        public int InsertHistory(IdentityHelper IdentityHelper, GiaoXacMinhModel GiaoXacMinhModel)
        {
            var kq = new ChuyenGiaiQuyet().PhanHoiDelete(GiaoXacMinhModel.XuLyDonID ?? 0);

            int docunmentid = GiaoXacMinhModel.XuLyDonID ?? 0;
            bool suDungQuyTrinhGQ = IdentityHelper.SuDungQuyTrinhGQPhucTap ?? false;

            string stateName = WorkflowInstance.Instance.GetCurrentStateOfDocument(docunmentid);

            var phanTPGQList = new PhanTPPhanGQ().GetByXuLyDonID(docunmentid);
            var vaiTroGQList = new VaiTroGiaiQuyet().GetByXuLyDonID(docunmentid);

            if (stateName == Constant.LD_Phan_GiaiQuyet || stateName == Constant.LD_CapDuoi_Phan_GiaiQuyet || stateName == Constant.TP_Phan_GiaiQuyet)
            {
                string commandCode = "";
                if (stateName == Constant.LD_Phan_GiaiQuyet)
                {
                    if (suDungQuyTrinhGQ)
                    {
                        //commandCode = WorkflowInstance.Instance.GetAvailabelCommands(docunmentid)[3];
                        commandCode = WorkflowInstance.Instance.GetAvailabelCommands(docunmentid).Where(x => x.ToString() == "LDPhanTPPhongGQ").FirstOrDefault();

                        if (IdentityHelper.CapID == CapQuanLy.CapUBNDXa.GetHashCode())
                        {
                            commandCode = WorkflowInstance.Instance.GetAvailabelCommands(docunmentid).Where(x => x.ToString() == "LDPhanTruongDoanGQ").FirstOrDefault();
                        }
                        if (IdentityHelper.CapHanhChinh == EnumCapHanhChinh.CapPhongThuocHuyen.GetHashCode() && IdentityHelper.RoleID == (int)RoleEnum.LanhDao)
                        {
                            commandCode = WorkflowInstance.Instance.GetAvailabelCommands(docunmentid).Where(x => x.ToString() == "LDPhanTruongDoanGQ").FirstOrDefault();
                        }
                    }

                    else
                        commandCode = WorkflowInstance.Instance.GetAvailabelCommands(docunmentid).Where(x => x.ToString() == "LDPhanTruongDoanGQ").FirstOrDefault();
                }
                else if (stateName == Constant.LD_CapDuoi_Phan_GiaiQuyet)
                {
                    if (suDungQuyTrinhGQ)
                        // bổ sung cấp huyện: ct huyện phân cấp dưới giải quyết => cấp dưới phân trưởng đoàn
                        if (IdentityHelper.RoleID == (int)RoleEnum.LanhDao && IdentityHelper.CapHanhChinh == EnumCapHanhChinh.CapPhongThuocHuyen.GetHashCode() || IdentityHelper.CapHanhChinh == EnumCapHanhChinh.CapUBNDXa.GetHashCode())
                        {
                            commandCode = WorkflowInstance.Instance.GetAvailabelCommands(docunmentid).Where(x => x.ToString() == "LDCapDuoiPhanTDoanGQ").FirstOrDefault();
                        }
                        else
                            commandCode = WorkflowInstance.Instance.GetAvailabelCommands(docunmentid).Where(x => x.ToString() == "LDCapDuoiPhanTPGQ").FirstOrDefault();
                    else
                        commandCode = WorkflowInstance.Instance.GetAvailabelCommands(docunmentid).Where(x => x.ToString() == "LDCapDuoiPhanTDoanGQ").FirstOrDefault();
                }
                else if (stateName == Constant.TP_Phan_GiaiQuyet)
                {
                    //TH LD don vị phan GQ lai thi khong goi Workflow
                    if (IdentityHelper.RoleID != (int)RoleEnum.LanhDao)
                    {
                        commandCode = WorkflowInstance.Instance.GetAvailabelCommands(docunmentid).Where(x => x.ToString() == "TPPhanTDoanGQ").FirstOrDefault();
                    }
                }

                DateTime HanGQ = DateTime.Now.AddDays(45);
                DateTime duedate = GiaoXacMinhModel.HanGiaiQuyet ?? HanGQ;
                int canboid = IdentityHelper.CanBoID ?? 0;
                if (commandCode != "")
                    WorkflowInstance.Instance.ExecuteCommand(docunmentid, canboid, commandCode, duedate, GiaoXacMinhModel.GhiChu ?? "");
                int xulydonID = GiaoXacMinhModel.XuLyDonID ?? 0;
                int stateID = new DonThuDAL().GetByXuLyDonID(xulydonID).StateID;
                //DateTime ngayHetHanCu = Utils.ConvertToDateTime(ngayhethancu, DateTime.MinValue);
                int CanBoID = Utils.ConvertToInt32(canboid, 0);
                //KetQuaInfo ketqua = new KetQuaInfo();

                var ketqua = new KetQuaDAL().Update_PreState(stateID, xulydonID, CanBoID);
            }

            #region chuyen giai quyet
            int coquanid = IdentityHelper.CoQuanID ?? 0;
            ChuyenGiaiQuyetInfo cgqinfo = new ChuyenGiaiQuyetInfo();
            cgqinfo.CoQuanGiaiQuyetID = coquanid;
            cgqinfo.CoQuanPhanID = coquanid;
            cgqinfo.NgayChuyen = DateTime.Now;
            cgqinfo.XuLyDonID = docunmentid;
            cgqinfo.GhiChu = GiaoXacMinhModel.GhiChu;
            cgqinfo.FileUrl = string.Empty;

            int val = 0;
            try
            {
                val = new ChuyenGiaiQuyet().Insert(cgqinfo);
            }
            catch
            {
            }

            try
            {
                //String fileDataStr = fileUrl;
                //if (fileDataStr != string.Empty)
                //{
                //    new ChuyenGiaiQuyet().DeleteFileChuyenGiaiQuyet(docunmentid, IdentityHelper.CoQuanID ?? 0, IdentityHelper.CanBoID, (int)EnumLoaiFile.FileDTCPGQ);
                //    string[] fileParts = fileDataStr.Split(';');
                //    for (int i = 0; i < fileParts.Length; i++)
                //    {

                //        string fileStr = fileParts[i];
                //        string[] dataParts = fileStr.Split(',');

                //        FileHoSoInfo info = new FileHoSoInfo();
                //        info.ChuyenGiaiQuyetID = val;
                //        info.FileURL = dataParts[0];
                //        info.NgayUp = Utils.ConvertToDateTime(dataParts[1], Constant.DEFAULT_DATE);
                //        info.TenFile = dataParts[2];
                //        info.TomTat = dataParts[3];
                //        info.NguoiUp = IdentityHelper.GetCanBoID();//IdentityHelper.GetUserID();
                //        info.FileID = Utils.ConvertToInt32(dataParts[6], 0);
                //        FileLogInfo infoFileLog = new FileLogInfo();
                //        infoFileLog.LoaiLog = (int)EnumLoaiLog.Them;
                //        infoFileLog.LoaiFile = (int)EnumLoaiFile.FileDTCPGQ;
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
                //            int result = new FileHoSo().InsertFileDonThuCanPhanGiaiQuyet(info);
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

            #endregion

            #region phan truong phong phan giai quyet
            //int capThanhTra = Utils.ConvertToInt32(isCapThanhTra, 0);
            int capThanhTra = IdentityHelper.CapThanhTra ?? 0;
            int tPID = GiaoXacMinhModel.TruongPhongID ?? 0;
            new PhanTPPhanGQ().DelByXLDIDAndCanBoID(docunmentid, tPID);

            //if (stateName == Constant.LD_CapDuoi_Phan_GiaiQuyet || (capThanhTra == 1 && stateName == Constant.LD_Phan_GiaiQuyet) || stateName == Constant.TP_Phan_GiaiQuyet)
            if (stateName == Constant.LD_CapDuoi_Phan_GiaiQuyet || stateName == Constant.LD_Phan_GiaiQuyet || stateName == Constant.TP_Phan_GiaiQuyet)
            {
                if (IdentityHelper.RoleID == (int)RoleEnum.LanhDao)
                {
                    PhanTPPhanGQModel pTPPhanGQInfo = new PhanTPPhanGQModel();
                    pTPPhanGQInfo.XuLyDonID = docunmentid;
                    pTPPhanGQInfo.PhongBanID = 0;
                    pTPPhanGQInfo.CanBoID = tPID;
                    pTPPhanGQInfo.NgayPhanGQ = DateTime.Now;

                    try
                    {
                        val = new PhanTPPhanGQ().Insert(pTPPhanGQInfo);
                    }
                    catch (Exception ex) { }
                }
            }
            #endregion

            return val;
        }

        public int InsertUpdatePhanGiaiQuyetArr(GiaoXacMinhModel GiaoXacMinhModel, int ChuyenGiaiQuyetID)
        {
            int val = 0;

            try
            {
                if (GiaoXacMinhModel.ToXacMinh != null && GiaoXacMinhModel.ToXacMinh.Count > 0)
                {
                    foreach (var item in GiaoXacMinhModel.ToXacMinh)
                    {
                        item.XuLyDonID = GiaoXacMinhModel.XuLyDonID ?? 0;
                        item.ChuyenGiaiQuyetID = ChuyenGiaiQuyetID;
                        val = new VaiTroGiaiQuyet().InsertUpdate(item);
                    }

                }
            }
            catch
            {
            }

            return val;

        }

        public IList<CanBoInfo> GetCanBoGQ(IdentityHelper IdentityHelper, int XuLyDonID)
        {
            IList<CanBoInfo> canBoGQList = new List<CanBoInfo>();
            if (IdentityHelper.SuDungQuyTrinhGQPhucTap == true)
            {
                canBoGQList = new CanBo().GetCanBoGQByCQID(IdentityHelper.CoQuanID ?? 0, XuLyDonID);
            }
            else
            {
                canBoGQList = new CanBo().GetCBGQByCoQuanID_QTDONGIAN(IdentityHelper.CoQuanID ?? 0, XuLyDonID);
            }
            return canBoGQList;
        }

        public IList<CanBoInfo> GetTruongPhong(IdentityHelper IdentityHelper)
        {
            IList<CanBoInfo> truongPhongList = new CanBo().GetTruongPhongByCQID(IdentityHelper.CoQuanID ?? 0);
            return truongPhongList;
        }

        public IList<BuocXacMinhInfo> DanhSachBuocXacMinhByXuLyDonID(int XuLyDonID)
        {
            IList<BuocXacMinhInfo> lstItem = new List<BuocXacMinhInfo>();
            lstItem = new BuocXacMinh().GetByLoaiKhieuToID(XuLyDonID);

            return lstItem;
        }

        public BaseResultModel CapNhatBuocXacMinh(IdentityHelper IdentityHelper, BuocXacMinhInfo BuocXacMinhInfo)
        {
            var Result = new BaseResultModel();
            TheoDoiXuLyInfo info = new TheoDoiXuLyInfo();
            info.XuLyDonID = BuocXacMinhInfo.XuLyDonID ?? 0;
            info.TheoDoiXuLyID = BuocXacMinhInfo.TheoDoiXuLyID ?? 0;
            info.NgayCapNhat = BuocXacMinhInfo.NgayCapNhat ?? DateTime.Now;
            info.GhiChu = BuocXacMinhInfo.GhiChu ?? "";
            info.TenBuoc = BuocXacMinhInfo.TenBuoc ?? "";
            info.BuocXacMinhID = BuocXacMinhInfo.BuocXacMinhID;
            info.DanhSachHoSoTaiLieu = BuocXacMinhInfo.DanhSachHoSoTaiLieu;
            SaveTheoDoiXuLy(IdentityHelper, info);
            Result.Status = 1;
            Result.Message = "Cập nhật bước xác minh thành công";
            return Result;
        }

        public int SaveTheoDoiXuLy(IdentityHelper IdentityHelper, TheoDoiXuLyInfo info)
        {
            //TheoDoiXuLyInfo info = new TheoDoiXuLyInfo();
            //info.XuLyDonID = Convert.ToInt32(xuLyDonID);
            //info.TheoDoiXuLyID = Convert.ToInt32(theoDoiXuLyID);
            //info.NgayCapNhat = Utils.ConvertToDateTime(ngayCapNhat, DateTime.MinValue);
            info.NgayCapNhat = info.NgayCapNhat.Date + DateTime.Now.TimeOfDay;
            //info.GhiChu = ghiChu;
            info.CanBoID = IdentityHelper.CanBoID ?? 0;
            //info.BuocXacMinhID = Utils.ConvertToInt32(buocXacMinhID, 0);

            if (info.TheoDoiXuLyID == 0)
            {
                try
                {
                    info.TheoDoiXuLyID = new TheoDoiXuLyDAL().Insert_New(info);
                }
                catch
                {
                }

                //if (info.TheoDoiXuLyID != 0)
                //{
                //    try
                //    {
                //        String fileDataStr = fileUrl;
                //        if (fileDataStr != string.Empty)
                //        {
                //            string[] fileParts = fileDataStr.Split(';');
                //            for (int i = 0; i < fileParts.Length; i++)
                //            {
                //                string fileStr = fileParts[i];
                //                string[] dataParts = fileStr.Split(',');
                //                int FileID = Utils.ConvertToInt32(dataParts[6], 0);
                //                result = InsertFileGQ(Utils.ConvertToDateTime(dataParts[1], Constant.DEFAULT_DATE), info.GhiChu, info.TheoDoiXuLyID, dataParts[0], dataParts[2], FileID);

                //                FileLogInfo infoFileLog = new FileLogInfo();
                //                infoFileLog.LoaiLog = (int)EnumLoaiLog.Them;
                //                infoFileLog.LoaiFile = (int)EnumLoaiFile.FileGiaiQuyet;
                //                int isBaoMat = Utils.ConvertToInt32(dataParts[4], 0);
                //                if (isBaoMat == (int)EnumDoBaoMat.BaoMat)
                //                {
                //                    infoFileLog.IsBaoMat = true;
                //                    infoFileLog.IsMaHoa = true;
                //                }
                //                else
                //                {
                //                    infoFileLog.IsBaoMat = false;
                //                    infoFileLog.IsMaHoa = false;
                //                }

                //                try
                //                {
                //                    infoFileLog.FileID = result;
                //                    new FileLog().Insert(infoFileLog);
                //                }
                //                catch
                //                {
                //                }
                //            }
                //        }
                //    }
                //    catch
                //    {
                //    }
                //}
            }
            else
            {

                //try
                //{
                //    new FileGiaiQuyet().Delete(info.TheoDoiXuLyID);
                //}
                //catch
                //{
                //}

                //try
                //{
                //    String fileDataStr = fileUrl;
                //    if (fileDataStr != string.Empty)
                //    {
                //        string[] fileParts = fileDataStr.Split(';');
                //        for (int i = 0; i < fileParts.Length; i++)
                //        {
                //            string fileStr = fileParts[i];
                //            string[] dataParts = fileStr.Split(',');
                //            int FileID = Utils.ConvertToInt32(dataParts[6], 0);
                //            result = InsertFileGQ(Utils.ConvertToDateTime(dataParts[1], Constant.DEFAULT_DATE), info.GhiChu, info.TheoDoiXuLyID, dataParts[0], dataParts[2], FileID);

                //            FileLogInfo infoFileLog = new FileLogInfo();
                //            infoFileLog.LoaiLog = (int)EnumLoaiLog.Them;
                //            infoFileLog.LoaiFile = (int)EnumLoaiFile.FileGiaiQuyet;
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
                //                infoFileLog.FileID = result;
                //                new FileLog().Insert(infoFileLog);
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


                try
                {
                    new TheoDoiXuLyDAL().Update_New(info);
                }
                catch
                {
                }
            }

            try
            {
                if (info.DanhSachHoSoTaiLieu != null && info.DanhSachHoSoTaiLieu.Count > 0)
                {
                    foreach (var item in info.DanhSachHoSoTaiLieu)
                    {
                        if (item.DanhSachFileDinhKemID != null && item.DanhSachFileDinhKemID.Count > 0)
                        {
                            new FileDinhKemDAL().UpdateFileGiaiQuyet(item.DanhSachFileDinhKemID, info.TheoDoiXuLyID);
                        }
                    }

                }
            }
            catch (Exception)
            {
            }


            return info.TheoDoiXuLyID;
        }

        public List<TheoDoiXuLyInfo> GetQuyTrinhGiaiQuyet(int XuLyDonID)
        {
            var ListInfo = new TheoDoiXuLyDAL().GetQuaTrinhGiaiQuyet(XuLyDonID).ToList();
            return ListInfo;
        }

        public KetQuaXacMinhModel GetByID(int XuLyDonID)
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

            KetQuaXacMinh.GiaoXacMinh = new GiaoXacMinhModel();
            KetQuaXacMinh.GiaoXacMinh.ToXacMinh = new VaiTroGiaiQuyet().GetByXuLyDonID(XuLyDonID);

            #region old
            //if (KetQuaXacMinh.BuocXacMinh != null && ListInfo != null && DTXuLyInfo.LoaiKhieuTo1ID != 9)
            //{
            //    foreach (var buocXacMinh in KetQuaXacMinh.BuocXacMinh)
            //    {
            //        buocXacMinh.TrangThai = 0;
            //        foreach (var kqXM in ListInfo)
            //        {
            //            if(buocXacMinh.BuocXacMinhID == kqXM.BuocXacMinhID)
            //            {
            //                buocXacMinh.TheoDoiXuLyID = kqXM.TheoDoiXuLyID;
            //                buocXacMinh.NgayCapNhat = kqXM.NgayCapNhat;
            //                buocXacMinh.GhiChu = kqXM.GhiChu;
            //                buocXacMinh.TenCanBo = kqXM.TenCanBo;
            //                buocXacMinh.TenCoQuan = kqXM.TenCoQuan;
            //                buocXacMinh.CanBoID = kqXM.CanBoID;
            //                if(kqXM.TheoDoiXuLyID > 0)
            //                {
            //                    buocXacMinh.TrangThai = 1;
            //                    buocXacMinh.TenTrangThai = "Đã cập nhật";
            //                }

            //                try
            //                {
            //                    buocXacMinh.DanhSachHoSoTaiLieu = new List<DanhSachHoSoTaiLieu>();
            //                    var fileDinhKem = new FileGiaiQuyet().GetFileGiaiQuyetByBuocXacMinhID(XuLyDonID, EnumLoaiFile.FileGiaiQuyet.GetHashCode(), buocXacMinh.BuocXacMinhID);
            //                    if (fileDinhKem.Count > 0)
            //                    {
            //                        buocXacMinh.DanhSachHoSoTaiLieu = new List<DanhSachHoSoTaiLieu>();
            //                        buocXacMinh.DanhSachHoSoTaiLieu = fileDinhKem.GroupBy(p => p.GroupUID)
            //                           .Select(g => new DanhSachHoSoTaiLieu
            //                           {
            //                               GroupUID = g.Key,
            //                               TenFile = g.FirstOrDefault().TenFile,
            //                               NoiDung = g.FirstOrDefault().TomTat,
            //                               TenNguoiCapNhat = g.FirstOrDefault().TenCanBo,
            //                               NguoiCapNhatID = g.FirstOrDefault().NguoiUp,
            //                               NgayCapNhat = g.FirstOrDefault().NgayUp,
            //                               FileDinhKem = fileDinhKem.Where(x => x.GroupUID == g.Key && x.FileHoSoID > 0).GroupBy(x => x.FileHoSoID)
            //                                               .Select(y => new FileDinhKemModel
            //                                               {
            //                                                   FileID = y.FirstOrDefault().FileHoSoID,
            //                                                   TenFile = y.FirstOrDefault().TenFile,
            //                                                   NgayCapNhat = y.FirstOrDefault().NgayUp,
            //                                                   NguoiCapNhat = y.FirstOrDefault().NguoiUp,
            //                                                   //FileType = y.FirstOrDefault().FileType,
            //                                                   FileUrl = y.FirstOrDefault().FileURL,
            //                                               }
            //                                               ).ToList(),

            //                           }
            //                           ).ToList();
            //                    }
            //                }
            //                catch (Exception)
            //                {

            //                }


            //            }
            //        }
            //    }
            //}
            //else if(ListInfo != null && DTXuLyInfo.LoaiKhieuTo1ID == 9)// phan anh kien nghi
            //{
            //    KetQuaXacMinh.BuocXacMinh = new List<BuocXacMinhInfo>();
            //    foreach (var kqXM in ListInfo)
            //    {
            //        var buocXacMinh = new BuocXacMinhInfo();
            //        buocXacMinh.TheoDoiXuLyID = kqXM.TheoDoiXuLyID;
            //        buocXacMinh.TenBuoc = kqXM.TenBuoc;
            //        buocXacMinh.NgayCapNhat = kqXM.NgayCapNhat;
            //        buocXacMinh.GhiChu = kqXM.GhiChu;
            //        buocXacMinh.TenCanBo = kqXM.TenCanBo;
            //        buocXacMinh.TenCoQuan = kqXM.TenCoQuan;
            //        buocXacMinh.CanBoID = kqXM.CanBoID;
            //        if (kqXM.TheoDoiXuLyID > 0)
            //        {
            //            buocXacMinh.TrangThai = 1;
            //            buocXacMinh.TenTrangThai = "Đã cập nhật";
            //        }

            //        try
            //        {
            //            buocXacMinh.DanhSachHoSoTaiLieu = new List<DanhSachHoSoTaiLieu>();
            //            var fileDinhKem = new FileGiaiQuyet().GetFileGiaiQuyetByBuocXacMinhID(XuLyDonID, EnumLoaiFile.FileGiaiQuyet.GetHashCode(), buocXacMinh.BuocXacMinhID);
            //            if (fileDinhKem.Count > 0)
            //            {
            //                buocXacMinh.DanhSachHoSoTaiLieu = new List<DanhSachHoSoTaiLieu>();
            //                buocXacMinh.DanhSachHoSoTaiLieu = fileDinhKem.GroupBy(p => p.GroupUID)
            //                   .Select(g => new DanhSachHoSoTaiLieu
            //                   {
            //                       GroupUID = g.Key,
            //                       TenFile = g.FirstOrDefault().TenFile,
            //                       NoiDung = g.FirstOrDefault().TomTat,
            //                       TenNguoiCapNhat = g.FirstOrDefault().TenCanBo,
            //                       NguoiCapNhatID = g.FirstOrDefault().NguoiUp,
            //                       NgayCapNhat = g.FirstOrDefault().NgayUp,
            //                       FileDinhKem = fileDinhKem.Where(x => x.GroupUID == g.Key && x.FileHoSoID > 0).GroupBy(x => x.FileHoSoID)
            //                                       .Select(y => new FileDinhKemModel
            //                                       {
            //                                           FileID = y.FirstOrDefault().FileHoSoID,
            //                                           TenFile = y.FirstOrDefault().TenFile,
            //                                           NgayCapNhat = y.FirstOrDefault().NgayUp,
            //                                           NguoiCapNhat = y.FirstOrDefault().NguoiUp,
            //                                           //FileType = y.FirstOrDefault().FileType,
            //                                           FileUrl = y.FirstOrDefault().FileURL,
            //                                       }
            //                                       ).ToList(),

            //                   }
            //                   ).ToList();
            //            }
            //        }
            //        catch (Exception)
            //        {

            //        }

            //        KetQuaXacMinh.BuocXacMinh.Add(buocXacMinh);
            //    }

            //    if(KetQuaXacMinh.BuocXacMinh.Count > 0)
            //    {
            //        KetQuaXacMinh.BuocXacMinh = KetQuaXacMinh.BuocXacMinh.OrderBy(x => x.TheoDoiXuLyID).ToList();
            //        int stt = 1;
            //        for (int i = 0; i < KetQuaXacMinh.BuocXacMinh.Count; i++)
            //        {
            //            KetQuaXacMinh.BuocXacMinh[i].OrderBy = stt++;
            //        }
            //    }
            //}

            //var fileDinhKem = new XuLyDonDAL().GetFileYKienXuLy(XuLyDonID).Where(x => x.LoaiFile == EnumLoaiFile.FileYKienXuLy.GetHashCode()).ToList();
            //if (fileDinhKem.Count > 0)
            //{
            //    DTXuLyInfo.DanhSachHoSoTaiLieu = new List<DanhSachHoSoTaiLieu>();
            //    DTXuLyInfo.DanhSachHoSoTaiLieu = fileDinhKem.GroupBy(p => p.GroupUID)
            //       .Select(g => new DanhSachHoSoTaiLieu
            //       {
            //           GroupUID = g.Key,
            //           TenFile = g.FirstOrDefault().TenFile,
            //           NoiDung = g.FirstOrDefault().TomTat,
            //           TenNguoiCapNhat = g.FirstOrDefault().TenCanBo,
            //           NguoiCapNhatID = g.FirstOrDefault().NguoiUp,
            //           NgayCapNhat = g.FirstOrDefault().NgayUp,
            //           FileDinhKem = fileDinhKem.Where(x => x.GroupUID == g.Key && x.FileID > 0).GroupBy(x => x.FileID)
            //                           .Select(y => new FileDinhKemModel
            //                           {
            //                               FileID = y.FirstOrDefault().FileID,
            //                               TenFile = y.FirstOrDefault().TenFile,
            //                               NgayCapNhat = y.FirstOrDefault().NgayUp,
            //                               NguoiCapNhat = y.FirstOrDefault().NguoiUp,
            //                               //FileType = y.FirstOrDefault().FileType,
            //                               FileUrl = y.FirstOrDefault().FileURL,
            //                           }
            //                           ).ToList(),

            //       }
            //       ).ToList();
            //}
            #endregion
            return KetQuaXacMinh;
        }

        public BaseResultModel TrinhKy(IdentityHelper IdentityHelper, BaoCaoXacMinhModel BaoCaoXacMinh)
        {
            var Result = new BaseResultModel();
            int idxulydon = BaoCaoXacMinh.XuLyDonID ?? 0;
            if (IdentityHelper.RoleID == RoleEnum.ChuyenVien.GetHashCode() || (IdentityHelper.RoleID == RoleEnum.LanhDaoPhong.GetHashCode() && IdentityHelper.CapHanhChinh == EnumCapHanhChinh.CapPhongThuocHuyen.GetHashCode()))
            {
                int coQuanPhanGQID = 0;
                coQuanPhanGQID = new ChuyenGiaiQuyet().GetCoQuanChuyenGiaiQuyet(idxulydon, IdentityHelper.CoQuanID ?? 0);

                XuLyDonInfo yKienGQInfo = new XuLyDonInfo();

                yKienGQInfo.CanBoGiaiQuyetID = IdentityHelper.CanBoID ?? 0;
                yKienGQInfo.NgayGiaiQuyet = Utils.ConvertToDateTime(DateTime.Now, DateTime.Now);
                yKienGQInfo.YKienGiaiQuyet = BaoCaoXacMinh.NoiDung;
                yKienGQInfo.XuLyDonID = idxulydon;

                DateTime duedate = BaoCaoXacMinh.HanGiaiQuyet ?? DateTime.Now;

                string data = "";
                bool suDungQuyTrinhGQ = IdentityHelper.SuDungQuyTrinhGQPhucTap ?? false;
                string commandCode = "";

                if (suDungQuyTrinhGQ)
                {
                    commandCode = WorkflowInstance.Instance.GetAvailabelCommands(idxulydon)[0];
                    // bổ sung nghiệp vụ cấp phòng thuộc huyện thì trình lên lãnh đạo cấp dưới duyệt giải quyết
                    if (IdentityHelper.CapHanhChinh == EnumCapHanhChinh.CapPhongThuocHuyen.GetHashCode())
                    {
                        commandCode = WorkflowInstance.Instance.GetAvailabelCommands(idxulydon).FirstOrDefault(x => x.Equals("TDoanTrinhLDCapDuoiDuyetGQ", StringComparison.OrdinalIgnoreCase));
                    }
                }
                else
                {
                    if (coQuanPhanGQID != 0)
                    {
                        commandCode = WorkflowInstance.Instance.GetAvailabelCommands(idxulydon).Where(x => x.ToString() == "TDoanTrinhLDCapDuoiDuyetGQ").FirstOrDefault();

                    }
                    else
                    {
                        commandCode = WorkflowInstance.Instance.GetAvailabelCommands(idxulydon).Where(x => x.ToString() == "TDoanTrinhLDCapTrenDuyetGQ").FirstOrDefault();

                    }
                }

                int canboid = IdentityHelper.CanBoID ?? 0;
                bool kq = WorkflowInstance.Instance.ExecuteCommand(idxulydon, canboid, commandCode, duedate, String.Empty);
                int result = 0;
                //if (yKienGQInfo.YKienGiaiQuyet != "" || fileUrl != "")
                if (yKienGQInfo.YKienGiaiQuyet != "")
                {
                    try
                    {
                        result = new XuLyDonDAL().InsertYKienGiaiQuyet(yKienGQInfo);
                    }
                    catch (Exception ex)
                    {
                    }
                    //try
                    //{
                    //    String fileDataStr = fileUrl;
                    //    if (fileDataStr != string.Empty)
                    //    {
                    //        string[] fileParts = fileDataStr.Split(';');
                    //        for (int i = 0; i < fileParts.Length; i++)
                    //        {
                    //            string fileStr = fileParts[i];
                    //            string[] dataParts = fileStr.Split(',');

                    //            FileHoSoInfo info = new FileHoSoInfo();
                    //            info.YKienGiaiQuyetID = result;
                    //            info.FileURL = dataParts[0];
                    //            info.NgayUp = Utils.ConvertToDateTime(dataParts[1], Constant.DEFAULT_DATE);
                    //            info.TenFile = dataParts[2];
                    //            info.TomTat = dataParts[3];
                    //            info.NguoiUp = IdentityHelper.CanBoID ?? 0;//IdentityHelper.GetUserID();
                    //            info.FileID = Utils.ConvertToInt32(dataParts[6], 0);
                    //            FileLogInfo infoFileLog = new FileLogInfo();
                    //            infoFileLog.LoaiLog = (int)EnumLoaiLog.Them;
                    //            infoFileLog.LoaiFile = (int)EnumLoaiFile.FileBCXM;
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
                    //                int result_file = new FileHoSo().InsertBCXM(info);
                    //                if (result_file > 0)
                    //                {
                    //                    infoFileLog.FileID = result_file;
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
                }

                if (IdentityHelper.SuDungQuyTrinhPhucTap == false && IdentityHelper.SuDungQuyTrinhGQPhucTap == false)
                {

                    if (coQuanPhanGQID > 0)
                    {
                        ChuyenDonThuVeCoQuanPhanGiaiQuyet(idxulydon, coQuanPhanGQID);
                    }
                }

                if (IdentityHelper.SuDungQuyTrinhGQPhucTap == false)
                {
                    //quy trinh don gian
                    //duyet bao cao xac minh
                    DongYKetQuaGiaiQuyet(IdentityHelper, BaoCaoXacMinh);
                    try
                    {
                        if (IdentityHelper.CapID != CapQuanLy.CapUBNDXa.GetHashCode())
                        {
                            //bo qua buoc trinh len chu tich ubnd
                            DongYKetQuaGiaiQuyet(IdentityHelper, BaoCaoXacMinh);
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            else if (IdentityHelper.RoleID == RoleEnum.LanhDao.GetHashCode())
            {
                //trinh len chu tich

            }

            Result.Message = "Trình báo cáo xác minh thành công";
            Result.Status = 1;

            return Result;

        }

        private static void ChuyenDonThuVeCoQuanPhanGiaiQuyet(int xldID, int coQuanPhanID)
        {
            try
            {
                new XuLyDonDAL().UpdateCoQuanGiaiQuyet(xldID, coQuanPhanID);
            }
            catch
            {
            }
        }

        public BaseResultModel DuyetBaoCaoXacMinh(IdentityHelper IdentityHelper, BaoCaoXacMinhModel BaoCaoXacMinh)
        {
            var Result = new BaseResultModel();
            if (BaoCaoXacMinh.TrangThaiPheDuyet == 1)
            {
                DongYKetQuaGiaiQuyet(IdentityHelper, BaoCaoXacMinh);
                DuyetBCXM_Insert(IdentityHelper, BaoCaoXacMinh);
            }
            else
            {
                YeuCauGiaiQuyetLai(IdentityHelper, BaoCaoXacMinh);
                DuyetBCXM_Insert(IdentityHelper, BaoCaoXacMinh);
            }

            Result.Status = 1;
            Result.Message = "Duyệt báo cáo xác minh thành công";
            return Result;
        }

        public BaseResultModel DuyetBCXM_Insert(IdentityHelper IdentityHelper, BaoCaoXacMinhModel BaoCaoXacMinh)
        {
            var result = new BaseResultModel();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@NoiDung",SqlDbType.NVarChar),
                new SqlParameter("@CanBoID",SqlDbType.Int),
                new SqlParameter("@XuLyDonID",SqlDbType.Int),
                new SqlParameter("@NgayCapNhat",SqlDbType.Date),
                new SqlParameter("@TrangThai",SqlDbType.Int),
                new SqlParameter("@HanXuLy",SqlDbType.Date),
            };
            parameters[0].Value = BaoCaoXacMinh.NoiDung ?? Convert.DBNull;
            parameters[1].Value = IdentityHelper.CanBoID;
            parameters[2].Value = BaoCaoXacMinh.XuLyDonID;
            parameters[3].Value = DateTime.Now;
            parameters[4].Value = BaoCaoXacMinh.TrangThaiPheDuyet ?? Convert.DBNull;
            parameters[5].Value = BaoCaoXacMinh.HanGiaiQuyet ?? Convert.DBNull;
            using (var connection = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                connection.Open();
                using (var trans = connection.BeginTransaction())
                {
                    try
                    {
                        var query = Utils.ConvertToInt32(SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, "V2_NV_DuyetBaoCaoXacMinh_Insert", parameters).ToString(), 0);
                        result.Status = 1;
                        result.Data = query;
                        result.Message = "duyệt báo cáo xác minh thành công";
                        trans.Commit();
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
            if (Utils.ConvertToInt32(result.Data, 0) > 0)
            {
                if (BaoCaoXacMinh.DanhSachHoSoTaiLieu.Count > 0)
                {
                    Insert_FileBaoCaoXM(IdentityHelper, BaoCaoXacMinh);
                }

            }
            return result;
        }
        public BaseResultModel Insert_FileBaoCaoXM(IdentityHelper IdentityHelper, BaoCaoXacMinhModel BaoCaoXacMinh)
        {
            var result = new BaseResultModel();
            foreach (var item in BaoCaoXacMinh.DanhSachHoSoTaiLieu)
            {
                foreach (var item1 in item.DanhSachFileDinhKemID)
                {
                    SqlParameter[] parameters = new SqlParameter[]
                    {
                        new SqlParameter("@XuLyDonID",SqlDbType.Int),
                        new SqlParameter("@FileID",SqlDbType.Int),
                        new SqlParameter("@TrangThai",SqlDbType.Int),
                    };
                    parameters[0].Value = BaoCaoXacMinh.XuLyDonID;
                    parameters[1].Value = item1;
                    parameters[2].Value = BaoCaoXacMinh.TrangThaiPheDuyet ?? Convert.DBNull;
                    using (var connection = new SqlConnection(SQLHelper.appConnectionStrings))
                    {
                        connection.Open();
                        using (var trans = connection.BeginTransaction())
                        {
                            try
                            {
                                var query = Utils.ConvertToInt32(SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, "V2_NV_FilePheDuyetBaoCaoXacMinh_Insert", parameters).ToString(), 0);
                                result.Status = 1;
                                result.Data = query;
                                result.Message = " thành công";
                                trans.Commit();
                            }
                            catch (Exception)
                            {
                                trans.Rollback();
                                throw;
                            }
                        }
                    }
                }

            }
            return result;
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
            WorkflowInstance.Instance.ExecuteCommand(xuLyDonID, canboid, commandCode, hanGiaiQuyet, BaoCaoXacMinh.NoiDung ?? "");

            try
            {
                // tuandhh bổ sung cấp huyện thuộc phòng đoàn tổ xác minh trình lãnh đạo phòng thì gộp state duyệt và ban hành quyết định => cập nhập BC,KL,QĐ như cấp SBN
                if (IdentityHelper.CapHanhChinh == EnumCapHanhChinh.CapPhongThuocHuyen.GetHashCode() && IdentityHelper.RoleID == RoleEnum.LanhDao.GetHashCode() && BaoCaoXacMinh.LoaiQuyTrinh == 6)
                {
                    commandCode = WorkflowInstance.Instance.GetAvailabelCommands(xuLyDonID)[0];
                    WorkflowInstance.Instance.ExecuteCommand(xuLyDonID, canboid, commandCode, hanGiaiQuyet, "");
                }

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
                //            int result = new FileHoSoDAL().InsertDonThuCDGQ(info);
                //            if (result > 0)
                //            {
                //                infoFileLog.FileID = result;
                //                new FileLogDAL().Insert(infoFileLog);
                //            }

                //        }
                //        catch
                //        {
                //        }
                //    }
                //}
                // bổ sung insert file 

            }
            catch
            {
            }

            return xuLyDonID;

        }

        public int YeuCauGiaiQuyetLai(IdentityHelper IdentityHelper, BaoCaoXacMinhModel BaoCaoXacMinh)
        {
            int TPKhongDuyetKQGiaiQuyet = 1;
            int LDCapDuoiKhongDuyetGQ = 1;
            int LDKhongDuyetGQTraLDCapDuoi = 1;
            int LDKhongDuyetGQTraTP = 4;

            bool suDungQuyTrinhGQ = IdentityHelper.SuDungQuyTrinhGQPhucTap ?? false;
            int coquanphanid = 0;
            int xuLyDonID = BaoCaoXacMinh.XuLyDonID ?? 0;
            DateTime hanGiaiQuyet = BaoCaoXacMinh.HanGiaiQuyet ?? DateTime.Now;
            string stateName = null;
            string commandCode = null;
            try
            {
                coquanphanid = new ChuyenGiaiQuyet().GetCoQuanChuyenGiaiQuyet(xuLyDonID, IdentityHelper.CoQuanID ?? 0);
            }
            catch
            {

            }

            stateName = WorkflowInstance.Instance.GetCurrentStateOfDocument(xuLyDonID);
            if (stateName == Constant.TP_DuyetGQ)
            {
                #region -- TP duyet
                commandCode = WorkflowInstance.Instance.GetAvailabelCommands(xuLyDonID)[TPKhongDuyetKQGiaiQuyet];

                #endregion
            }

            if (stateName == Constant.LD_CQCapDuoiDuyetGQ)
            {
                if (suDungQuyTrinhGQ)
                    commandCode = WorkflowInstance.Instance.GetAvailabelCommands(xuLyDonID)[LDCapDuoiKhongDuyetGQ];
                else
                {
                    commandCode = WorkflowInstance.Instance.GetAvailabelCommands(xuLyDonID).Where(x => x.ToString() == "LDCapDuoiKhongDuyetTraLaiTDoan").FirstOrDefault();

                }

            }
            if (stateName == Constant.LD_Duyet_GiaiQuyet)
            {
                if (suDungQuyTrinhGQ)
                {
                    if (coquanphanid != 0)
                    {
                        commandCode = WorkflowInstance.Instance.GetAvailabelCommands(xuLyDonID)[LDKhongDuyetGQTraLDCapDuoi];
                    }
                    else
                        commandCode = WorkflowInstance.Instance.GetAvailabelCommands(xuLyDonID)[LDKhongDuyetGQTraTP];
                }
                else
                {
                    if (coquanphanid != 0)
                    {
                        commandCode = WorkflowInstance.Instance.GetAvailabelCommands(xuLyDonID).Where(x => x.ToString() == "LDCapTrenKhongDuyetTraCapDuoi").FirstOrDefault();
                    }
                    else
                    {
                        commandCode = WorkflowInstance.Instance.GetAvailabelCommands(xuLyDonID).Where(x => x.ToString() == "LDKhongDuyetTraTruongDoan").FirstOrDefault();
                    }

                }
            }

            int canboid = IdentityHelper.CanBoID ?? 0;
            WorkflowInstance.Instance.ExecuteCommand(xuLyDonID, canboid, !string.IsNullOrEmpty(commandCode) ? commandCode : "", hanGiaiQuyet, !string.IsNullOrEmpty(BaoCaoXacMinh.NoiDung) ? BaoCaoXacMinh.NoiDung : "");

            //try
            //{
            //    String fileDataStr = fileUrl;
            //    if (fileDataStr != string.Empty)
            //    {
            //        string[] fileParts = fileDataStr.Split(';');
            //        for (int i = 0; i < fileParts.Length; i++)
            //        {
            //            string fileStr = fileParts[i];
            //            string[] dataParts = fileStr.Split(',');

            //            FileHoSoInfo info = new FileHoSoInfo();
            //            info.XuLyDonID = xuLyDonID;
            //            info.FileURL = dataParts[0];
            //            info.NgayUp = Utils.ConvertToDateTime(dataParts[1], Constant.DEFAULT_DATE);
            //            info.TenFile = dataParts[2];
            //            info.TomTat = dataParts[3];
            //            info.NguoiUp = IdentityHelper.GetCanBoID();//IdentityHelper.GetUserID();
            //            info.FileID = Utils.ConvertToInt32(dataParts[6], 0);

            //            FileLogInfo infoFileLog = new FileLogInfo();
            //            infoFileLog.LoaiLog = (int)EnumLoaiLog.Them;
            //            infoFileLog.LoaiFile = (int)EnumLoaiFile.FileDTCDGQ;
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
            //                int result = new FileHoSo().InsertDonThuCDGQ(info);
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

            return xuLyDonID;
        }

        public BaseResultModel GiaHanThoiGianXacMinh(KetQuaInfo info, IdentityHelper IdentityHelper)
        {
            var Result = new BaseResultModel();

            //int xldId = Convert.ToInt32(hdfXuLyDonID.Value);
            //KetQuaInfo info = new KetQuaInfo();
            //kq.HanGiaiQuyetMoi = Utils.ConvertToDateTime(txtNgayDonDoc.Text, DateTime.MinValue);
            //kq.LyDoDieuChinh = Utils.ConvertToString(txtNDDonDoc.InnerText, string.Empty);
            //GetKetQuaInfo(ref info);
            //info.XuLyDonID = xldId;
            info.NgayThayDoi = DateTime.Now;
            info.NguoiUp = IdentityHelper.CanBoID;

            var GetPXLByXuLyDonCanBo = new KetQuaDAL().GetByID(IdentityHelper.CanBoID, info.XuLyDonID);
            info.HanGiaiQuyetCu = GetPXLByXuLyDonCanBo.HanGiaiQuyetCu;
            info.FileUrl = string.Empty;
            int GiaHanGiaiQuyetID = 0;
            int result = new KetQuaDAL().Insert_GiaHanGiaiQuyet(info, ref GiaHanGiaiQuyetID, 0);
            int TransitionID = 0;
            var RoleID = IdentityHelper.RoleID;

            var CoQuanDangNhap = new CoQuan().GetCoQuanByCoQuanID_New(IdentityHelper.CoQuanID ?? 0).FirstOrDefault();
            if (RoleID == 1 && CoQuanDangNhap.CapUBND == true)
            {
                TransitionID = 38;
            }
            else if (RoleID == 1 && CoQuanDangNhap.CapUBND == false && GetPXLByXuLyDonCanBo.PreStateID == 19 && GetPXLByXuLyDonCanBo.StateID == 18 && CoQuanDangNhap.SuDungQuyTrinhGQ == true)
            {
                TransitionID = 37;
            }
            else if (RoleID == 1 && CoQuanDangNhap.CapUBND == false && GetPXLByXuLyDonCanBo.PreStateID == 19 && GetPXLByXuLyDonCanBo.StateID == 7 && CoQuanDangNhap.SuDungQuyTrinhGQ == true)
            {
                TransitionID = 39;
            }
            else if (RoleID == 1 && CoQuanDangNhap.CapUBND == false && GetPXLByXuLyDonCanBo.PreStateID == 8 && GetPXLByXuLyDonCanBo.StateID == 18 && CoQuanDangNhap.SuDungQuyTrinhGQ == false)
            {
                TransitionID = 49;
            }
            else if (RoleID == 2 && CoQuanDangNhap.CapUBND == false)
            {
                TransitionID = 36;
            }
            else if (RoleID == 1 && CoQuanDangNhap.CapUBND == false && GetPXLByXuLyDonCanBo.PreStateID == 8 && GetPXLByXuLyDonCanBo.StateID == 7 && CoQuanDangNhap.SuDungQuyTrinhGQ == false)
            {
                TransitionID = 46;
            }
            else
            {
                TransitionID = 0;
            }
            var CanBoDangNhap = IdentityHelper.CanBoID;
            new KetQuaDAL().Update_DueDateTrans(info.HanGiaiQuyetMoi, info.XuLyDonID, IdentityHelper.RoleID, TransitionID, CanBoDangNhap);
            //message
            if (result > 0)
            {
                try
                {
                    if (info.DanhSachHoSoTaiLieu != null && info.DanhSachHoSoTaiLieu.Count > 0)
                    {
                        foreach (var item in info.DanhSachHoSoTaiLieu)
                        {
                            if (item.DanhSachFileDinhKemID != null && item.DanhSachFileDinhKemID.Count > 0)
                            {
                                new FileDinhKemDAL().UpdateFileGiaHanGiaiQuyet(item.DanhSachFileDinhKemID, GiaHanGiaiQuyetID);
                            }
                        }
                    }
                }
                catch (Exception)
                {

                }
                //String fileDataStr = hdfFileData.Value;
                //if (fileDataStr != string.Empty)
                //{
                //    string[] fileParts = fileDataStr.Split(';');
                //    for (int i = 0; i < fileParts.Length; i++)
                //    {
                //        string fileStr = fileParts[i];
                //        string[] dataParts = fileStr.Split(',');

                //        FileHoSoInfo infoFileHoSo = new FileHoSoInfo();
                //        infoFileHoSo.KetQuaID = result;
                //        infoFileHoSo.GiaHanGiaiQuyetID = GiaHanGiaiQuyetID;
                //        infoFileHoSo.FileURL = dataParts[0];
                //        infoFileHoSo.NgayUp = Utils.ConvertToDateTime(dataParts[1], Constant.DEFAULT_DATE);
                //        infoFileHoSo.TenFile = dataParts[2];
                //        infoFileHoSo.TomTat = dataParts[3];
                //        infoFileHoSo.NguoiUp = IdentityHelper.GetCanBoID();//IdentityHelper.GetUserID();

                //        FileLogInfo infoFileLog = new FileLogInfo();
                //        infoFileLog.LoaiLog = (int)EnumLoaiLog.Them;
                //        infoFileLog.LoaiFile = (int)EnumLoaiFile.FileGiaHanGiaiQuyet;
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
                //            int resultFile = new KetQua().InsertFileGiaHanGiaiQuyet(infoFileHoSo);
                //            if (resultFile > 0)
                //            {
                //                infoFileLog.FileID = resultFile;
                //                new FileLog().Insert(infoFileLog);
                //            }

                //        }
                //        catch (Exception ex)
                //        {
                //        }
                //    }
                //}

                Result.Message = Constant.CONTENT_MESSAGE_SUCCESS;
                Result.Status = 1;
            }
            else
            {
                Result.Message = Constant.CONTENT_MESSAGE_ERROR;
                Result.Status = 0;
            }

            return Result;
        }


    }
}
