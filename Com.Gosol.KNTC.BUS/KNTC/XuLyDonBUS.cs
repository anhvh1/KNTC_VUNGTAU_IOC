using Com.Gosol.KNTC.DAL.HeThong;
using Com.Gosol.KNTC.DAL.KNTC;
using Com.Gosol.KNTC.Model.HeThong;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.HeThong;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Security;
using Com.Gosol.KNTC.Ultilities;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Workflow;
using Utils = Com.Gosol.KNTC.Ultilities.Utils;

namespace Com.Gosol.KNTC.BUS.KNTC
{
    public class XuLyDonBUS
    {
        public IList<DTXuLyInfo> GetBySearch(ref int TotalRow, XuLyDonParamsForFilter p, IdentityHelper IdentityHelper)
        {
            int cr = p.PageNumber;
            if (cr == 0)
            {
                cr = 1;
            }
            int start = (cr - 1) * p.PageSize;
            int end = cr * p.PageSize;
            // cho cac bien vao query info
            QueryFilterInfo info = new QueryFilterInfo();
            info.CoQuanID = IdentityHelper.CoQuanID ?? 0;
            info.KeyWord = "%" + (p.Keyword ?? "") + "%";
            info.Start = start;
            info.End = end;
            info.LoaiKhieuToID = p.LoaiKhieuToID ?? 0;
            info.TrangThai = p.TrangThaiXuLyID;
            info.HuongXuLyID = p.HuongXuLyID ?? 0;
            info.TuNgay = p.TuNgay ?? DateTime.MinValue;
            info.DenNgay = p.DenNgay ?? DateTime.MinValue;
            info.CanBoID = IdentityHelper.CanBoID ?? 0;
            int roleID = IdentityHelper.RoleID ?? 0;
            if (roleID == (int)RoleEnum.LanhDao)
            {
                info.CanBoID = 0;
            }

            IList<DTXuLyInfo> DataSource = new DTXuLy().GetDTDaTiepNhan(info, IdentityHelper.CanBoID ?? 0, ref TotalRow);
            if (DataSource.Count > 0)
            {
                foreach (var item in DataSource)
                {
                    var renderTrangThai = new RenderTrangThai();
                    renderTrangThai.GetTrangThai(
                        item.LoaiQuyTrinh,
                        item.HuongGiaiQuyetID,
                        item.StateName,
                        item.StateID,
                        item.NextStateID,
                        item.TrangThaiDuyet ?? 0,
                        item.TrinhDuThao,
                        IdentityHelper,
                        item.NgayCapNhat,
                        item.ChuyenGiaiQuyetID,
                        item.KetQuaID,
                        0,
                        // bổ sung trạng thái rút đơn bằng RutDonID
                        item.RutDonID
                        );
                    item.TrangThaiMoi = renderTrangThai.TrangThaiMoi;
                    item.TrangThaiIDMoi = renderTrangThai.TrangThaiIDMoi;
                    item.CheckTrangThai = renderTrangThai.CheckTrangThai;

                    
                    if (item.HuongGiaiQuyetID == 0)
                    {
                        item.TrangThaiStr = "Chưa xử lý";
                        item.TrangThaiID = 0;
                    }                   
                    else if (item.StateName == Constant.CV_TiepNhan)
                    {
                        if (IdentityHelper.SuDungQuyTrinhPhucTap == true)
                        {
                            item.TrangThaiStr = "Đã xử lý";
                            item.TrangThaiID = 1;
                            if (item.NextStateID == 11 || item.TrangThaiDuyet == 2)
                            {
                                item.TrangThaiStr = "Xử lý lại";
                                item.TrangThaiID = 2;
                            }
                        }
                        else
                        {
                            item.TrangThaiStr = "Chưa xử lý";
                            item.TrangThaiID = 0;
                        }
                    }
                    else
                    {
                        if (IdentityHelper.SuDungQuyTrinhPhucTap == true)
                        {
                            item.TrangThaiStr = "Đã trình";
                            item.TrangThaiID = 3;
                        }
                        else
                        {
                            if (item.StateName == Constant.LD_Phan_GiaiQuyet)
                            {
                                item.TrangThaiStr = "Đã xử lý";
                                item.TrangThaiID = 4;
                                //item.TrangThaiID = 1;
                            }
                            else
                            {
                                if (item.StateName == Constant.Ket_Thuc)
                                {
                                    item.TrangThaiStr = "Đã xử lý";
                                    item.TrangThaiID = 1;
                                }
                                else
                                {
                                    //item.TrangThaiStr = "Đã giao";
                                    item.TrangThaiStr = "Đã xử lý";
                                    item.TrangThaiID = 4;
                                }
                            }
                        }
                    }
                    // bổ sung chưa xử lý đơn đã rút đơn
                    if (item.HuongGiaiQuyetID == 0 && item.RutDonID != 0)
                    {
                        item.TrangThaiStr = "Đã rút đơn";
                        item.TrangThaiID = 4;
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
                        dt.HoTen = "";
                        List<DoiTuongKNInfo> ltInfo = new DoiTuongKN().GetByNhomKNID(nhomKNID).ToList();
                        int count = 0;
                        foreach (var doituong in ltInfo)
                        {
                            string tenTinh, tenHuyen, tenXa;

                            DoiTuongKNInfo diaChiInfo = new DoiTuongKN().GetDiaChiDTKhieuNai(doituong.DoiTuongKNID);


                            tenTinh = ", " + diaChiInfo.TenTinh;
                            tenHuyen = ", " + diaChiInfo.TenHuyen;


                            if (diaChiInfo.DiaChiCT != null)
                                tenXa = ", " + diaChiInfo.TenXa;
                            else
                                tenXa = diaChiInfo.TenXa;

                            dt.HoTen += diaChiInfo.HoTen + "(" + diaChiInfo.DiaChiCT + ")";
                            count++;
                            if (count >= ltInfo.Count())
                                break;
                            else
                                dt.HoTen += ", <br/>";
                        }

                        if (ltInfo.Count > 0)
                            item.HoTen = dt.HoTen;
                    }

                }
            }

            return DataSource;
        }



        public DTXuLyInfo GetByID(int XuLyDonID)
        {
            DTXuLyInfo DTXuLyInfo = new DTXuLy().GetByID(XuLyDonID);
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
            var fileDinhKem = new XuLyDonDAL().GetFileYKienXuLy(XuLyDonID).Where(x => x.LoaiFile == EnumLoaiFile.FileYKienXuLy.GetHashCode()).ToList();
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
                       FileDinhKem = fileDinhKem.Where(x => x.GroupUID == g.Key && x.FileYKienXuLyID > 0).GroupBy(x => x.FileYKienXuLyID)
                                       .Select(y => new FileDinhKemModel
                                       {
                                           FileID = y.FirstOrDefault().FileYKienXuLyID,
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

        public BaseResultModel SaveXuLyDon(TiepDanInfo Info, IdentityHelper IdentityHelper)
        {
            var TiepDanInfo = new TiepDanBUS().GetByID(Info.TiepDanKhongDonID ?? 0);
            TiepDanInfo.NoiDungHuongDan = Info.NoiDungHuongDan;
            TiepDanInfo.HuongGiaiQuyetID = Info.HuongGiaiQuyetID;

            var Result = new BaseResultModel();
            string Message = "";
            int isNhapThongTinDonThu = TiepDanInfo.TiepDanCoDon ?? 0;
            bool isTrungDon = false;
            int phongbanID = TiepDanInfo.PhongID ?? 0;
            //int hdfTrungDon = Utils.ConvertToInt32(hdf_CheckDisblaTrung.Value, 0);
            int hdfTrungDon = TiepDanInfo.CheckTrung ?? 0;
            //int donThu_trung_edit = Utils.ConvertToInt32(hdf_DonThuTrung.Value, 0);
            int donThu_trung_edit = TiepDanInfo.DonThuTrung ?? 0;
            //int kNLan2 = Utils.ConvertToInt32(hdfKNLan2.Value, 0);
            //int kNLan2_edit = Utils.ConvertToInt32(hdfDonThuKNLan2.Value, 0);  
            int kNLan2 = TiepDanInfo.KNLan2 ?? 0;
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

            if (TiepDanInfo.LanhDaoTiepDanID == null || TiepDanInfo.LanhDaoTiepDanID == 0)
            {
                #region insert dt kn, dtbikn

                DonThuInfo dtInfo = new DonThuInfo();

                //Them nhom kn/tc
                NhomKNInfo nhomInfo = new NhomKNInfo();
                if (TiepDanInfo.NhomKN != null) nhomInfo = TiepDanInfo.NhomKN;
                int KQQuaTiepDan = TiepDanInfo.KQQuaTiepDan ?? 0;
                //dai dien phap ly
                nhomInfo.DaiDienPhapLy = false;
                nhomInfo.DuocUyQuyen = false;

                int daidienSelected = TiepDanInfo.ThongTinNguoiDaiDien ?? 0;
                if (daidienSelected == 1)
                    nhomInfo.DuocUyQuyen = true;
                else
                    nhomInfo.DaiDienPhapLy = true;
                //end - dai dien phap ly

                int nhomId = TiepDanInfo.NhomKNID ?? 0;
                //if (isKNLan2)
                //{
                //    nhomId = 0;
                //}
                //if (nhomId < 1)
                //{
                //    //insert
                //    nhomId = new NhomKN().Insert(nhomInfo);
                //}
                //else
                //{
                //    nhomInfo.NhomKNID = nhomId;
                //    nhomInfo.NhomKNID = new NhomKN().Update(nhomInfo);
                //}

                int songuoidaidien = TiepDanInfo.SoNguoiDaiDien ?? 1;

                //Them doi tuong kn/tc
                //updateDoiTuongKNTC(TiepDanInfo, songuoidaidien, nhomId);

                //Them Don thu (step1) 
                dtInfo.NhomKNID = nhomId;
                //if (TiepDanInfo.DoiTuongBiKN != null)
                //{
                //    if (hdfTrungDon != 1 || donThu_trung_edit != 1)
                //    {
                //        //them doi tuong bi khieu nai
                //        dtbiknID = InsertDoiTuongBiKN(TiepDanInfo);
                //    }
                //    dtInfo.DoiTuongBiKNID = dtbiknID;
                //}
                if (TiepDanInfo.DoiTuongBiKN != null)
                {
                    dtInfo.DoiTuongBiKNID = TiepDanInfo.DoiTuongBiKN.DoiTuongBiKNID;
                }

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
                                    //donthuID = InsertDonThu(dtInfo.NhomKNID, dtInfo.DoiTuongBiKNID, TiepDanInfo);
                                }

                                tiepDanInfo = InsertXuLyDon(IdentityHelper, TiepDanInfo, donthuID, isTrungDon, isKNLan2);

                                xuLyDonNew = tiepDanInfo.XuLyDonID ?? 0;

                                CapNhatFileDinhKem(TiepDanInfo, xuLyDonNew, donthuID);
                                //CapNhatFileDinhKem(xuLyDonNew, donthuID);
                                //CapNhatYKienXL(xuLyDonNew);

                            }

                            if (IdentityHelper.SuDungQuyTrinhPhucTap == false)
                            {
                                bool isChuyenDon = false;
                                if (TiepDanInfo.HuongGiaiQuyetID == (int)HuongGiaiQuyetEnum.ChuyenDon)
                                    isChuyenDon = true;
                                if (isChuyenDon)
                                {
                                    ChuyenDon(IdentityHelper, tiepDanInfo, xuLyDonNew);
                                }
                            }

                            //tiepdanID = InsertTiepDanKhongDon(TiepDanInfo, xuLyDonNew, donthuID, dtInfo.NhomKNID, KQQuaTiepDan, letanchuyenID, isTrungDon);
                            //hdf_CheckDisblaTrung.Value = " 1";

                            GanDonThuVaoWFVaThucThiCommand(IdentityHelper, TiepDanInfo, xuLyDonNew);

                            if (IdentityHelper.SuDungQuyTrinhPhucTap == true)
                            {
                                ChuyenXuLyChoPhong(phongbanID, xuLyDonNew);
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
                                //donthuID = InsertDonThu(dtInfo.NhomKNID, dtInfo.DoiTuongBiKNID, TiepDanInfo);
                                donthuID = TiepDanInfo.DonThuID ?? 0;
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

                                if (IdentityHelper.SuDungQuyTrinhPhucTap == false)
                                {
                                    bool isChuyenDon = false;
                                    if (TiepDanInfo.HuongGiaiQuyetID == (int)HuongGiaiQuyetEnum.ChuyenDon)
                                        isChuyenDon = true;
                                    if (isChuyenDon)
                                    {
                                        ChuyenDon(IdentityHelper, xldInfo, xulydonID);
                                    }
                                }
                            }

                            //tiepdanID = InsertTiepDanKhongDon(TiepDanInfo, xulydonID, donthuID, dtInfo.NhomKNID, KQQuaTiepDan, letanchuyenID, isTrungDon);

                            if (isNhapThongTinDonThu == 1)
                            {
                                GanDonThuVaoWFVaThucThiCommand(IdentityHelper, TiepDanInfo, xulydonID);

                                if (IdentityHelper.SuDungQuyTrinhPhucTap == true)
                                {
                                    ChuyenXuLyChoPhong(phongbanID, xulydonID);
                                }
                            }
                        }
                        catch { }
                        #endregion

                    }

                    //hdf_xulydonId.Value = xulydonID.ToString();
                    //hdf_DonThuID.Value = donthuID.ToString();
                    //hdf_TiepDanID.Value = tiepdanID.ToString();

                    //udpStep1.Update();

                }
                else
                {
                    #region Update tiep dan
                    try
                    {
                        if (isNhapThongTinDonThu == 1)
                        {
                            //donthuID = InsertDonThu(dtInfo.NhomKNID, dtInfo.DoiTuongBiKNID, TiepDanInfo);
                            donthuID = TiepDanInfo.DonThuID ?? 0;
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

                    }
                    catch
                    {
                        throw;
                    }
                    #endregion
                }
            }
            Result.Status = 1;
            Result.Message = "Lưu thành công";
            Result.Data = TiepDanInfo;
            return Result;
        }

        public TiepDanInfo InsertXuLyDon(IdentityHelper IdentityHelper, TiepDanInfo Info, int donthuID, bool isTrungDon, bool isKNLan2)
        {
            bool suDungQTPhucTap = IdentityHelper.SuDungQuyTrinhPhucTap ?? false;
            bool suDungQTVanThuTiepDan = IdentityHelper.SuDungQTVanThuTiepDan ?? false;

            //int xldID = Utils.ConvertToInt32(hdf_xulydonId.Value, 0);
            TiepDanInfo xldInfo = new TiepDanInfo();
            int xldID = Info.XuLyDonID ?? 0;
            if (xldID > 0)
            {
                xldInfo = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().GetXuLyDonByXLDID(xldID);
                xldInfo.HuongGiaiQuyetID = Info.HuongGiaiQuyetID;
                xldInfo.NoiDungHuongDan = Info.NoiDungHuongDan;
                xldInfo.DanhSachHoSoTaiLieu = Info.DanhSachHoSoTaiLieu;
            }
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
                //xldInfo.DonThuID = donthuID;
                if (isTrungDon)
                    xldInfo.SoLan = 2;
                else
                    xldInfo.SoLan = 1;
                #region old
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

                //xldInfo.CBDuocChonXL = 0;
                //xldInfo.QTTiepNhanDon = 0;

                //if (suDungQTPhucTap && suDungQTVanThuTiepDan)
                //{
                //    //xldInfo.CBDuocChonXL = Utils.ConvertToInt32(ddlCanBoXL.SelectedValue, 0);
                //    xldInfo.QTTiepNhanDon = (int)EnumQTTiepNhanDon.QTVanThuTiepDan;
                //}
                #endregion
                if (xldInfo.XuLyDonID > 0)
                {
                    //xldInfo.XuLyDonID = xldID;
                    xldInfo.CanBoXuLyID = IdentityHelper.CanBoID;
                    new Com.Gosol.KNTC.DAL.KNTC.TiepDan().UpdateXuLyDon(xldInfo);
                }
                else
                {
                    xldInfo.SoDonThu = GetSoDonThu(IdentityHelper.CoQuanID ?? 0, IdentityHelper);
                    xldInfo.XuLyDonIDGoc = xldID;
                    xldInfo.CanBoTiepNhapID = IdentityHelper.CanBoID;
                    xldID = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().InsertXuLyDon(xldInfo);
                    xldInfo.XuLyDonID = xldID;
                }

                var state = WorkflowInstance.Instance.GetCurrentStateOfDocument(xldID);
                if (state == Constant.CV_XuLy)
                {
                    int huongXL = xldInfo.HuongGiaiQuyetID ?? 0;
                    int raVBDonDoc = (int)HuongGiaiQuyetEnum.RaVanBanDonDoc;
                    int cVanChiDao = (int)HuongGiaiQuyetEnum.CongVanChiDao;

                    List<string> commandList = WorkflowInstance.Instance.GetAvailabelCommands(xldID);
                    string command = string.Empty;

                    bool isDeXuatThuLy = false;
                    if (xldInfo.HuongGiaiQuyetID == ((int)HuongGiaiQuyetEnum.DeXuatThuLy)) isDeXuatThuLy = true;

                    if (isDeXuatThuLy)
                    {
                        if (xldInfo.LoaiKhieuTo1ID == Constant.TranhChap && IdentityHelper.CapID == (int)CapQuanLy.CapUBNDXa)
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
                        if (huongXL == cVanChiDao || huongXL == raVBDonDoc)
                        {
                            command = commandList.Where(x => x.ToString() == "ChuyenDonHoacGuiVBDonDoc").FirstOrDefault();
                        }
                        else
                        {
                            command = commandList.Where(x => x.ToString() == "TiepDanKetThuc").FirstOrDefault();
                        }

                    }

                    WorkflowInstance.Instance.ExecuteCommand(xldID, IdentityHelper.CanBoID ?? 0, command, DateTime.Now.AddDays(10), string.Empty);
                }
                UpdateDonThuVaoWFVaThucThiCommand(IdentityHelper, xldInfo, xldInfo.XuLyDonID ?? 0);
            }
            //file dinh kem
            if (xldInfo.DanhSachHoSoTaiLieu != null && xldInfo.DanhSachHoSoTaiLieu.Count > 0)
            {
                foreach (var item in xldInfo.DanhSachHoSoTaiLieu)
                {
                    if (item.DanhSachFileDinhKemID != null && item.DanhSachFileDinhKemID.Count > 0)
                    {
                        new FileDinhKemDAL().UpdateFileXuLyDon(item.DanhSachFileDinhKemID, xldInfo.XuLyDonID ?? 0, 0);
                    }
                }

            }

            try
            {
                //update quy trinh xld
                if (IdentityHelper.SuDungQuyTrinhPhucTap == true)
                {
                    new XuLyDonDAL().UpdateQuyTrinh(xldInfo.XuLyDonID, 1, null);
                }
            }
            catch (Exception)
            {
            }

            return xldInfo;
        }

        public static string GetSoDonThu(int coquanID, IdentityHelper IdentityHelper)
        {
            string soDonThu = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().GetSoDonThu(coquanID);
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
        private void CapNhatFileDinhKem(TiepDanInfo tiepDanInfo, int xuLyDonID, int donThuID)
        {
            if (tiepDanInfo.DanhSachHoSoTaiLieu != null && tiepDanInfo.DanhSachHoSoTaiLieu.Count > 0)
            {
                foreach (var item in tiepDanInfo.DanhSachHoSoTaiLieu)
                {
                    if (item.DanhSachFileDinhKemID != null && item.DanhSachFileDinhKemID.Count > 0)
                    {
                        new FileDinhKemDAL().UpdateFileHoSo(item.DanhSachFileDinhKemID, xuLyDonID, donThuID, string.Empty);
                    }
                }

            }
        }

        private void ChuyenDon(IdentityHelper IdentityHelper, TiepDanInfo xldGocInfo, int xuLyDonIDGoc)
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
            cxlInfo.CQNhanID = xldGocInfo.CQNhanID ?? 0;
            cxlInfo.NgayChuyen = DateTime.Now;

            CloneDonThuTaiCQDuocChuyenDon(IdentityHelper, cxlInfo.CQNhanID, xldGocInfo, xuLyDonIDGoc, xldGocInfo.DonThuID ?? 0);

            try
            {
                new ChuyenXuLy().Insert(cxlInfo);
            }
            catch
            {
            }
        }

        private void CloneDonThuTaiCQDuocChuyenDon(IdentityHelper IdentityHelper, int coQuanNhanID, TiepDanInfo xldGocInfo, int xuLyDonIDGoc, int donThuID)
        {
            #region clone don thu
            int donThuIDNew = 0;
            try
            {

                DonThuInfo donThuInfo = new DonThuInfo();
                donThuInfo = new DonThuDAL().GetByID(donThuID);
                if (donThuInfo != null)
                {
                    TiepDanInfo info = new TiepDanInfo();
                    info.NhomKNID = donThuInfo.NhomKNID;
                    info.DoiTuongBiKNID = donThuInfo.DoiTuongBiKNID;
                    info.LoaiKhieuTo1ID = donThuInfo.LoaiKhieuTo1ID;
                    info.LoaiKhieuTo2ID = donThuInfo.LoaiKhieuTo2ID;
                    info.LoaiKhieuTo3ID = donThuInfo.LoaiKhieuTo3ID;
                    info.LoaiKhieuToID = donThuInfo.LoaiKhieuToID;
                    info.NoiDungDon = donThuInfo.NoiDungDon;
                    info.TrungDon = false;
                    info.TinhID = donThuInfo.TinhID;
                    info.HuyenID = donThuInfo.HuyenID;
                    info.XaID = donThuInfo.XaID;
                    info.DiaChiPhatSinh = donThuInfo.DiaChiPhatSinh;
                    info.LeTanChuyen = false;
                    info.NgayVietDon = donThuInfo.NgayVietDon;
                    try
                    {
                        donThuIDNew = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().InsertDonThu(info);
                    }
                    catch { }
                    //new DonThu().Insert(donThuInfo);
                }
            }
            catch { }
            #endregion

            #region clone xu ly don
            TiepDanInfo xldCloneInfo = xldGocInfo;
            xldCloneInfo.DonThuID = donThuIDNew;
            xldCloneInfo.CanBoTiepNhapID = 0;
            xldCloneInfo.SoDonThu = GetSoDonThu(IdentityHelper, coQuanNhanID);
            xldCloneInfo.CoQuanID = coQuanNhanID;
            xldCloneInfo.NguonDonDen = (int)EnumNguonDonDen.CoQuanKhac;
            xldCloneInfo.CQChuyenDonID = 0;//IdentityHelper.GetCoQuanID();
            xldCloneInfo.CQChuyenDonDenID = IdentityHelper.CoQuanID;
            xldCloneInfo.HuongGiaiQuyetID = 0;
            xldCloneInfo.CanBoXuLyID = 0;
            xldCloneInfo.DaDuyetXuLy = false;
            xldCloneInfo.NgayChuyenDon = DateTime.Now;
            xldCloneInfo.XuLyDonIDGoc = xldGocInfo.XuLyDonID;
            try
            {
                xldCloneInfo.XuLyDonID = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().InsertXuLyDon(xldCloneInfo);
            }
            catch
            {
            }
            #endregion

            //new TiepDan().UpdateXuLyDonIDClone(xuLyDonIDGoc, xldCloneInfo.XuLyDonID);


            List<FileHoSoInfo> fileHoSoList = new FileHoSoDAL().GetByXuLyDonID(xuLyDonIDGoc).ToList();
            foreach (var fileInfo in fileHoSoList)
            {
                fileInfo.XuLyDonID = xldCloneInfo.XuLyDonID ?? 0;
                FileLogInfo infoFileLog = new FileLogInfo();

                infoFileLog.LoaiLog = (int)EnumLoaiLog.Them;
                infoFileLog.LoaiFile = fileInfo.LoaiFile;
                infoFileLog.IsBaoMat = fileInfo.IsBaoMat;
                infoFileLog.IsMaHoa = fileInfo.IsMaHoa;
                try
                {
                    int resultInsert = new FileHoSoDAL().Insert(fileInfo);
                    if (resultInsert > 0)
                    {
                        infoFileLog.FileID = resultInsert;
                        new FileLogDAL().Insert(infoFileLog);
                    }
                }
                catch
                {
                }
            }

            List<XuLyDonInfo> lsFileYKienXL = new XuLyDonDAL().GetFileYKienXuLy(xuLyDonIDGoc).ToList();
            FileHoSoInfo fileYKienXLInfo = new FileHoSoInfo();
            foreach (var files in lsFileYKienXL)
            {
                fileYKienXLInfo.TenFile = files.TenFileYKienXL;
                fileYKienXLInfo.TomTat = files.TomTat;
                fileYKienXLInfo.NgayUp = files.NgayUp;
                fileYKienXLInfo.NguoiUp = files.NguoiUp;
                fileYKienXLInfo.FileURL = files.FileUrl;
                fileYKienXLInfo.XuLyDonID = xldCloneInfo.XuLyDonID ?? 0;

                FileLogInfo infoFileLog = new FileLogInfo();
                infoFileLog.LoaiLog = (int)EnumLoaiLog.Them;
                infoFileLog.LoaiFile = files.LoaiFile;
                infoFileLog.IsBaoMat = files.IsBaoMat;
                infoFileLog.IsMaHoa = files.IsMaHoa;

                try
                {
                    int resultInsert = new FileHoSoDAL().InsertFileYKienXL(fileYKienXLInfo);
                    if (resultInsert > 0)
                    {
                        infoFileLog.FileID = resultInsert;
                        new FileLogDAL().Insert(infoFileLog);
                    }
                }
                catch
                {
                }
            }

            WorkflowInstance.Instance.AttachDocument(xldCloneInfo.XuLyDonID ?? 0, "XuLyDon", IdentityHelper.UserID ?? 0, null);
        }

        public static string GetSoDonThu(IdentityHelper IdentityHelper, int coquanID)
        {
            string soDonThu = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().GetSoDonThu(coquanID);
            string maCQ = string.Empty;

            if (coquanID == IdentityHelper.CoQuanID) { maCQ = IdentityHelper.MaCoQuan; }
            else
            {
                CoQuanInfo cqInfo = new CoQuan().GetCoQuanByID(coquanID);
                maCQ = cqInfo.MaCQ;
            }

            string numberPart = Regex.Replace(soDonThu.Replace(maCQ, ""), "[^0-9.]", "");
            int soDonMoi = Utils.ConvertToInt32(numberPart, 0) + 1;
            return maCQ + soDonMoi;
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
        private void ChuyenXuLyChoPhong(int phongbanID, int xulydonID)
        {
            try
            {
                PhanXuLyInfo phanXLInfo = new PhanXuLyInfo();
                phanXLInfo.XuLyDonID = xulydonID;
                phanXLInfo.PhongBanID = phongbanID;
                phanXLInfo.NgayPhan = DateTime.Now;
                phanXLInfo.GhiChu = string.Empty;
                new PhanXuLy().Insert(phanXLInfo);
            }
            catch (Exception e)
            {
            }
        }

        public BaseResultModel TrinhDuyet(IdentityHelper IdentityHelper, DTXuLyInfo info)
        {
            var Result = new BaseResultModel();

            bool sDQTVanThuTiepNhan = IdentityHelper.SuDungQTVanThuTiepNhanDon ?? false;
            bool sDQuyTrinhPhucTap = IdentityHelper.SuDungQuyTrinhPhucTap ?? false;
            bool sDQuyTrinhVanThuTiepDan = IdentityHelper.SuDungQTVanThuTiepDan ?? false;
            int sDungQTGianTiep = IdentityHelper.QuyTrinhGianTiep ?? 0;

            //int idxulydon = Utils.ConvertToInt32(PhanXuLyInfo.XuLyDonID.Value, 0);
            //int nguondonden = Utils.ConvertToInt32(hdfNguonDonDen.Value, 0);
            //int huonggiaiquyet = Utils.ConvertToInt32(hdfHuongGiaiQuyetID.Value, 0);
            //int canBoTiepNhanID = IdentityHelper.GetCanBoID();

            int idxulydon = info.XuLyDonID;
            int nguondonden = info.NguonDonDen ?? 0;
            int huonggiaiquyet = info.HuongGiaiQuyetID;
            int canBoTiepNhanID = IdentityHelper.CanBoID ?? 0;

            if (idxulydon != 0)
            {
                XuLyDonInfo xLDInfo = new XuLyDonDAL().GetByID(idxulydon, string.Empty);

                int TRINH_LD_DUYET_KQ_XULY = 1;
                int TRINH_TP_DUYET_KQ_XULY = 2;
                int TRINH_LD_PHAN_XULY = 0;


                bool kq = false;
                string commandCode = "";
                List<string> commandList = WorkflowInstance.Instance.GetAvailabelCommands(idxulydon);
                int? stateIdGop = null;
                nguondonden = xLDInfo.NguonDonDenID;

                // tuandhh comment: nguồn đơn đến nào đều vào quy trình xử lý theo code cũ
                //if (nguondonden == (int)EnumNguonDonDen.TrucTiep || nguondonden == (int)EnumNguonDonDen.CoQuanKhac)
                //{
                //    commandCode = commandList.Where(x => x.ToString() == "TrinhKetQuaXL").FirstOrDefault();
                //}
                //else
                //{
                //    //if (sDQuyTrinhPhucTap)
                //    //{
                //    //    if (sDQTVanThuTiepNhan)
                //    //    {
                //    //        if (xLDInfo.QTTiepNhanDon == (int)EnumQTTiepNhanDon.QTVanThuTiepNhan)
                //    //        {
                //    //            commandCode = commandList.Where(x => x.ToString() == "TrinhKetQuaXL").FirstOrDefault();
                //    //        }
                //    //        if (xLDInfo.QTTiepNhanDon == (int)EnumQTTiepNhanDon.QTTiepNhanGianTiep)
                //    //        {
                //    //            commandCode = commandList.Where(x => x.ToString() == "TrinhLD").FirstOrDefault();
                //    //        }
                //    //        else if (xLDInfo.QTTiepNhanDon == 0)
                //    //        {
                //    //            commandCode = commandList.Where(x => x.ToString() == "TrinhLD").FirstOrDefault();
                //    //        }
                //    //    }

                //    //    if (xLDInfo.QTTiepNhanDon == (int)EnumQTTiepNhanDon.QTGianTiepBTD)
                //    //    {
                //    //        commandCode = commandList.Where(x => x.ToString() == "TrinhKetQuaXL").FirstOrDefault();
                //    //    }

                //    //    //qt phuc tap don thuan
                //    //    if (!sDQTVanThuTiepNhan && xLDInfo.QTTiepNhanDon != (int)EnumQTTiepNhanDon.QTGianTiepBTD)
                //    //    {
                //    //        commandCode = commandList.Where(x => x.ToString() == "TrinhLD").FirstOrDefault();
                //    //    }

                //    //}

                //    commandCode = commandList.Where(x => x.ToString() == "TrinhKetQuaXL").FirstOrDefault();
                //}
                // end tuandhh cmt

                var lanhdao = new CanBoDAL().GetByID(info.LanhDaoID ?? 0);
                if (IdentityHelper.CapHanhChinh == EnumCapHanhChinh.CapUBNDTinh.GetHashCode() || IdentityHelper.CapHanhChinh == EnumCapHanhChinh.CapPhongThuocSo.GetHashCode())
                {
                    commandCode = commandList.Where(x => x.ToString() == "TrinhKetQuaXL").FirstOrDefault();
                }
                // tuandhh bổ sung quy trình cv trình lãnh đạo SBN bỏ qua quy trình trình trưởng phòng thì gộp state
                else if (IdentityHelper.CapHanhChinh == EnumCapHanhChinh.CapSoNganh.GetHashCode())
                {
                    if (lanhdao != null)
                    {
                        if (IdentityHelper.RoleID == RoleEnum.ChuyenVien.GetHashCode() && lanhdao.RoleID == RoleEnum.LanhDao.GetHashCode())
                        {
                            stateIdGop = 6;
                        }

                        if (lanhdao.RoleID == RoleEnum.LanhDao.GetHashCode())
                        {
                            commandList = WorkflowInstance.Instance.GetCommandsByStateID(stateIdGop ?? 0);
                            commandCode = commandList.Where(x => x.ToString() == "DuyetXL").FirstOrDefault();
                        }
                        else
                            commandCode = commandList.Where(x => x.ToString() == "TiepDanTrinhTP").FirstOrDefault();
                    }
                }
                // end tuandhh bổ sung quy trình cv trình lãnh đạo SBN 
                else if (IdentityHelper.CapHanhChinh == EnumCapHanhChinh.CapPhongThuocHuyen.GetHashCode())
                {
                    if (IdentityHelper.BanTiepDan ?? false)
                    {
                        commandCode = commandList.Where(x => x.ToString() == "TrinhKetQuaXL").FirstOrDefault();
                    }
                    else
                    {
                        if (lanhdao != null)
                        {
                            if (IdentityHelper.RoleID == RoleEnum.ChuyenVien.GetHashCode() && lanhdao.RoleID == RoleEnum.LanhDao.GetHashCode())
                            {
                                stateIdGop = 6;
                            }

                            if (lanhdao.RoleID == RoleEnum.LanhDao.GetHashCode())
                            {
                                commandList = WorkflowInstance.Instance.GetCommandsByStateID(stateIdGop ?? 0);
                                commandCode = commandList.Where(x => x.ToString() == "DuyetXL").FirstOrDefault();
                            }
                            else
                                commandCode = commandList.Where(x => x.ToString() == "TiepDanTrinhTP").FirstOrDefault();
                        }
                    }
                }
                else
                {
                    commandCode = commandList.Where(x => x.ToString() == "TrinhKetQuaXL").FirstOrDefault();
                }

                int canboid = IdentityHelper.CanBoID ?? 0;
                kq = WorkflowInstance.Instance.ExecuteCommand(idxulydon, canboid, commandCode, DateTime.Now.AddDays(10), String.Empty, stateIdGop);

                if (kq == true)
                {
                    Result.Status = 1;
                    if (nguondonden == (int)EnumNguonDonDen.TrucTiep)
                        Result.Message = "Trình lãnh đạo xem xét, phê duyệt thành công";//Constant.CONTENT_TRINHKY_SUCCESS;
                    else
                    {
                        if (sDQTVanThuTiepNhan && xLDInfo.QTTiepNhanDon == (int)EnumQTTiepNhanDon.QTVanThuTiepNhan || xLDInfo.QTTiepNhanDon == (int)EnumQTTiepNhanDon.QTGianTiepBTD)
                        {
                            Result.Message = "Trình lãnh đạo xem xét, phê duyệt thành công";
                        }
                        else
                        {
                            Result.Message = "Trình lãnh đạo xem xét, phân xử lý thành công";
                        }
                    }


                    if (sDQuyTrinhPhucTap && (sDQTVanThuTiepNhan || sDQuyTrinhVanThuTiepDan))
                    {
                        #region -- send mail
                        //XuLyDonInfo xuLyDonInfo = new XuLyDonInfo();
                        //xuLyDonInfo = new XuLyDonDAL().GetByID(idxulydon);
                        //List<string> emailList = new List<string>();
                        //List<CanBoInfo> lsLanhDaoInfo = new List<CanBoInfo>();
                        //int coQuanID = IdentityHelper.CoQuanID ?? 0;
                        //lsLanhDaoInfo = new CanBo().GetLanhDaoCoQuan(coQuanID).ToList();

                        //// tuan thay doi lay email dong
                        //QL_EmailInfo Eminfo = new QL_Email().GetByLoaiEmail(Constant.DM_EMAIL_DUYETXL);
                        //List<EmailInfo> lst_Email_ND = new List<EmailInfo>();
                        //string emailTitle = string.Empty;
                        //if (lsLanhDaoInfo.Count > 0)
                        //{
                        //    foreach (CanBoInfo item in lsLanhDaoInfo)
                        //    {
                        //        if (item.Email != "")
                        //        {
                        //            EmailInfo info = new EmailInfo();
                        //            info.Email = item.Email;

                        //            if (item.GioiTinh == 0)
                        //            {
                        //                string[] arry_ten = item.TenCanBo.Split(' ');
                        //                string ten = string.Empty;
                        //                if (arry_ten.Length > 0)
                        //                {
                        //                    ten = arry_ten[arry_ten.Length - 1];
                        //                }
                        //                else
                        //                {
                        //                    ten = arry_ten[0];
                        //                }
                        //                if (Eminfo.EmailID != 0)
                        //                {
                        //                    info.NoiDungEmail = Utils.ConvertToString(Eminfo.NoiDungEmail, string.Empty).Replace("#so_don", xuLyDonInfo.SoDonThu).Replace("#can_bo", ten).Replace("#gioi_tinh", "Ông/Bà");
                        //                }
                        //            }
                        //            else if (item.GioiTinh == 1)
                        //            {
                        //                string[] arry_ten = item.TenCanBo.Split(' ');
                        //                string ten = string.Empty;
                        //                if (arry_ten.Length > 0)
                        //                {
                        //                    ten = arry_ten[arry_ten.Length - 1];
                        //                }
                        //                else
                        //                {
                        //                    ten = arry_ten[0];
                        //                }
                        //                if (Eminfo.EmailID != 0)
                        //                {
                        //                    info.NoiDungEmail = Utils.ConvertToString(Eminfo.NoiDungEmail, string.Empty).Replace("#so_don", xuLyDonInfo.SoDonThu).Replace("#can_bo", ten).Replace("#gioi_tinh", "Ông");
                        //                }
                        //            }
                        //            else
                        //            {
                        //                string[] arry_ten = item.TenCanBo.Split(' ');
                        //                string ten = string.Empty;
                        //                if (arry_ten.Length > 0)
                        //                {
                        //                    ten = arry_ten[arry_ten.Length - 1];
                        //                }
                        //                else
                        //                {
                        //                    ten = arry_ten[0];
                        //                }
                        //                if (Eminfo.EmailID != 0)
                        //                {
                        //                    info.NoiDungEmail = Utils.ConvertToString(Eminfo.NoiDungEmail, string.Empty).Replace("#so_don", xuLyDonInfo.SoDonThu).Replace("#can_bo", ten).Replace("#gioi_tinh", "Bà");
                        //                }
                        //            }

                        //            lst_Email_ND.Add(info);
                        //        }
                        //    }
                        //}
                        //if (Eminfo.EmailID != 0)
                        //{
                        //    emailTitle = Utils.ConvertToString(Eminfo.TenEmail, string.Empty);
                        //}
                        //else
                        //{
                        //}
                        //string fromEmail = ConfigurationSettings.AppSettings["Email"];
                        //string passWord = ConfigurationSettings.AppSettings["PassEmail"];

                        //if (lst_Email_ND != null && lst_Email_ND.Count > 0)
                        //{
                        //    SystemConfigInfo smtpServer = new SystemConfig().GetByKey("SMTP_SERVER");
                        //    SystemConfigInfo smtpPort = new SystemConfig().GetByKey("SMTP_PORT");
                        //    if (smtpServer != null)
                        //    {
                        //        string emailServer = smtpServer.ConfigValue;
                        //        if (smtpPort != null)
                        //        {
                        //            int port = Utils.ConvertToInt32(smtpPort.ConfigValue, 0);
                        //            //MailHelper.SendEmail(emailList, emailTitle, emailContent, fromEmail, passWord, emailServer, port);
                        //            //MailHelper.SendEmail_obj(lst_Email_ND, emailTitle, fromEmail, passWord, emailServer, port);
                        //            (new System.Threading.Tasks.Task(() => MailHelper.SendEmail_obj(lst_Email_ND, emailTitle, fromEmail, passWord, emailServer, port))).Start();
                        //        }
                        //        else
                        //        {
                        //            //MailHelper.SendEmail(emailList, emailTitle, emailContent, fromEmail, passWord, emailServer);
                        //            //MailHelper.SendEmail_obj(lst_Email_ND, emailTitle, fromEmail, passWord);
                        //            (new System.Threading.Tasks.Task(() => MailHelper.SendEmail_obj(lst_Email_ND, emailTitle, fromEmail, passWord))).Start();
                        //        }
                        //    }
                        //    else
                        //    {
                        //        //MailHelper.SendEmail(emailList, emailTitle, emailContent, fromEmail, passWord);
                        //        //MailHelper.SendEmail_obj(lst_Email_ND, emailTitle, fromEmail, passWord);
                        //        (new System.Threading.Tasks.Task(() => MailHelper.SendEmail_obj(lst_Email_ND, emailTitle, fromEmail, passWord))).Start();
                        //    }
                        //}


                        #endregion
                    }
                    else
                    {
                        new XuLyDonDAL().UpdateCanBoTiepNhan(idxulydon, canBoTiepNhanID);

                    }

                    if (info.LanhDaoID > 0)
                    {
                        // chuyên viên trình trực tiếp lãnh đạo SBN
                        if ((IdentityHelper.CapHanhChinh == EnumCapHanhChinh.CapSoNganh.GetHashCode()
                            && IdentityHelper.RoleID == RoleEnum.ChuyenVien.GetHashCode()
                            && lanhdao != null && lanhdao.RoleID == RoleEnum.LanhDao.GetHashCode())
                            || (IdentityHelper.CapHanhChinh == EnumCapHanhChinh.CapPhongThuocHuyen.GetHashCode() && !(IdentityHelper.BanTiepDan ?? false)
                            && IdentityHelper.RoleID == RoleEnum.ChuyenVien.GetHashCode()
                            && lanhdao != null && lanhdao.RoleID == RoleEnum.LanhDao.GetHashCode())
                            )
                        {
                            new XuLyDonDAL().UpdateLanhDaoDuyet(idxulydon, null, info.LanhDaoID);
                        }
                        else
                            new XuLyDonDAL().UpdateLanhDaoDuyet(idxulydon, info.LanhDaoID, null);
                    }

                }
                else
                {
                    Result.Message = Constant.CONTENT_TRINHKY_ERROR;

                }

            }

            return Result;
        }

        public IList<CanBoInfo> GetDanhSachLanhDao(IdentityHelper IdentityHelper)
        {
            var Data = new CanBo().GetDanhSachLanhDao_V2();
            if (IdentityHelper.CoQuanID > 0 && Data.Count > 0 && IdentityHelper.CapHanhChinh > 0)
            {
                var cq = new CoQuan().GetCoQuanByID(IdentityHelper.CoQuanID ?? 0);

                if (IdentityHelper.CapHanhChinh == EnumCapHanhChinh.CapUBNDTinh.GetHashCode())
                {
                    if (IdentityHelper.RoleID == RoleEnum.ChuyenVien.GetHashCode())
                    {
                        Data = Data.Where(x => x.CoQuanID == IdentityHelper.CoQuanID && x.RoleID == 1 && (x.CanBoID != 20 && x.TenCanBo != "Administrator")).ToList();
                    }
                    else if (IdentityHelper.RoleID == RoleEnum.LanhDaoPhong.GetHashCode())// lãnh đạo đơn vị
                    {
                        Data = Data.Where(x => x.CoQuanID == cq.CoQuanID && x.RoleID == 1 && (x.CanBoID != 20 && x.TenCanBo != "Administrator")).ToList();
                    }
                    else if (IdentityHelper.RoleID == RoleEnum.LanhDao.GetHashCode())// lãnh đạo đơn vị cha
                    {
                        Data = Data.Where(x => x.CoQuanID == cq.CoQuanChaID && x.RoleID == 1 && (x.CanBoID != 20 && x.TenCanBo != "Administrator")).ToList();
                    }
                }
                else if (IdentityHelper.CapHanhChinh == EnumCapHanhChinh.CapPhongThuocHuyen.GetHashCode())
                {
                    if (IdentityHelper.BanTiepDan ?? false) // ban tiếp dân cấp huyện 
                    {
                        if (IdentityHelper.RoleID == RoleEnum.ChuyenVien.GetHashCode())
                        {
                            Data = Data.Where(x => x.CoQuanID == IdentityHelper.CoQuanID && x.RoleID == 1 && (x.CanBoID != 20 && x.TenCanBo != "Administrator")).ToList();
                        }
                        else if (IdentityHelper.RoleID == RoleEnum.LanhDao.GetHashCode())// trưởng ban tiếp dân huyện
                        {
                            Data = Data.Where(x => x.CoQuanID == cq.CoQuanChaID && x.RoleID == 1 && x.ChuTichUBND == 1 && (x.CanBoID != 20 && x.TenCanBo != "Administrator")).ToList();
                        }
                        //else if (IdentityHelper.RoleID == RoleEnum.LanhDao.GetHashCode())// chủ tịch ubnd huyện
                        //{
                        //    Data = Data.Where(x => x.CoQuanID == cq.CoQuanChaID && x.RoleID == 1 && (x.CanBoID != 20 && x.TenCanBo != "Administrator")).ToList();
                        //}
                    }
                    else
                    {
                        if (IdentityHelper.RoleID == RoleEnum.ChuyenVien.GetHashCode())
                        {
                            Data = Data.Where(x => x.CoQuanID == IdentityHelper.CoQuanID && x.RoleID == 1 && (x.CanBoID != 20 && x.TenCanBo != "Administrator")).ToList();
                        }
                        else if (IdentityHelper.RoleID == RoleEnum.LanhDaoPhong.GetHashCode())// lãnh đạo phòng
                        {
                            Data = Data.Where(x => x.CoQuanID == cq.CoQuanID && x.RoleID == 1 && (x.CanBoID != 20 && x.TenCanBo != "Administrator")).ToList();
                        }
                        //else if (IdentityHelper.RoleID == RoleEnum.LanhDao.GetHashCode())// lãnh đạo đơn vị
                        //{
                        //    Data = Data.Where(x => x.CoQuanID == cq.CoQuanID && x.RoleID == 1 && (x.CanBoID != 20 && x.TenCanBo != "Administrator")).ToList();
                        //}
                    }
                }
                else if (IdentityHelper.CapHanhChinh == EnumCapHanhChinh.CapPhongThuocSo.GetHashCode())
                {
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
                else if (IdentityHelper.CapHanhChinh == EnumCapHanhChinh.CapSoNganh.GetHashCode())
                {
                    if (IdentityHelper.RoleID == RoleEnum.ChuyenVien.GetHashCode())
                    {
                        Data = Data.Where(x => x.CoQuanID == IdentityHelper.CoQuanID && (x.CanBoID != 20 && x.TenCanBo != "Administrator")).ToList();
                    }
                    else if (IdentityHelper.RoleID == RoleEnum.LanhDaoPhong.GetHashCode())// lãnh đạo phòng
                    {
                        Data = Data.Where(x => x.CoQuanID == cq.CoQuanID && x.RoleID == 1 && (x.CanBoID != 20 && x.TenCanBo != "Administrator")).ToList();
                    }
                    else if (IdentityHelper.RoleID == RoleEnum.LanhDao.GetHashCode())// lãnh đạo đơn vị
                    {
                        Data = Data.Where(x => x.CoQuanID == cq.CoQuanID && x.RoleID == 1 && (x.CanBoID != 20 && x.TenCanBo != "Administrator")).ToList();
                    }
                }
                else if (IdentityHelper.CapHanhChinh == EnumCapHanhChinh.CapUBNDHuyen.GetHashCode())
                {
                    if (IdentityHelper.RoleID == RoleEnum.ChuyenVien.GetHashCode())
                    {
                        Data = Data.Where(x => x.CoQuanID == IdentityHelper.CoQuanID && (x.CanBoID != 20 && x.TenCanBo != "Administrator")).ToList();
                    }
                    else if (IdentityHelper.RoleID == RoleEnum.LanhDaoPhong.GetHashCode())
                    {
                        Data = Data.Where(x => x.CoQuanID == cq.CoQuanID && x.RoleID == 1 && (x.CanBoID != 20 && x.TenCanBo != "Administrator")).ToList();
                    }
                    else if (IdentityHelper.RoleID == RoleEnum.LanhDao.GetHashCode())
                    {
                        Data = Data.Where(x => x.CoQuanID == cq.CoQuanChaID && x.RoleID == 1 && (x.CanBoID != 20 && x.TenCanBo != "Administrator")).ToList();
                    }
                }
                else if (IdentityHelper.CapHanhChinh == EnumCapHanhChinh.CapUBNDXa.GetHashCode())
                {
                    Data = Data.Where(x => x.CoQuanID == cq.CoQuanChaID && x.RoleID == 1 && (x.CanBoID != 20 && x.TenCanBo != "Administrator")).ToList();
                }
                else // các case còn lại k thực tế k có data
                {
                    Data = new List<CanBoInfo>();
                }

                //if (IdentityHelper.RoleID == RoleEnum.LanhDao.GetHashCode() || IdentityHelper.RoleID == RoleEnum.LanhDaoPhong.GetHashCode())
                //{
                //    var cq = new CoQuan().GetCoQuanByID(IdentityHelper.CoQuanID ?? 0);
                //    Data = Data.Where(x => x.CoQuanID == cq.CoQuanChaID && (x.CanBoID != 20 && x.TenCanBo != "Administrator")).ToList();
                //}
                //else Data = Data.Where(x => x.CoQuanID == IdentityHelper.CoQuanID && (x.CanBoID != 20 && x.TenCanBo != "Administrator")).ToList();
            }

            return Data;
        }
    }
}
