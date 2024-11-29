using Com.Gosol.KNTC.DAL.HeThong;
using Com.Gosol.KNTC.DAL.KNTC;
using Com.Gosol.KNTC.Model.HeThong;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.HeThong;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Models.TiepDan;
using Com.Gosol.KNTC.Security;
using Com.Gosol.KNTC.Ultilities;
using Microsoft.Office.Interop.Word;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using RestSharp.Extensions;
using Spire.Doc.Fields;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Workflow;

namespace Com.Gosol.KNTC.BUS.KNTC
{
    public class TiepNhanDonBUS
    {
        public IList<DTXuLyInfo> GetBySearch(ref int TotalRow, TiepDanParamsForFilter p, IdentityHelper IdentityHelper)
        {
            IList<DTXuLyInfo> ListInfo = new List<DTXuLyInfo>();

            QueryFilterInfo info = new QueryFilterInfo();
            info.CoQuanID = IdentityHelper.CoQuanID ?? 0;
            info.KeyWord = p.Keyword ?? "";
            info.Start = (p.PageNumber - 1) * p.PageSize;
            info.End = p.PageNumber * p.PageSize;
            info.LoaiKhieuToID = p.LoaiKhieuToID ?? 0;
            info.TuNgay = p.TuNgay ?? DateTime.MinValue;
            info.DenNgay = p.DenNgay ?? DateTime.MinValue;
            info.CanBoID = IdentityHelper.CanBoID ?? 0;
            info.CQChuyenDonDenID = p.CQChuyenDonDenID ?? 0;
            info.LoaiRutDon = p.LoaiRutDon ?? 0;

            ListInfo = new DTXuLy().GetSoTiepNhanGianTiep_BTDTinh(ref TotalRow, info);

            return ListInfo;

        }

        public BaseResultModel Save(IdentityHelper IdentityHelper, TiepDanInfo Info)
        {
            var Result = new BaseResultModel();

            bool isInsertNewDonThu = false; // Insert WF
            bool sDQTVanThuTiepNhanDon = IdentityHelper.SuDungQTVanThuTiepNhanDon ?? true;
            bool qTrinhPhucTap = IdentityHelper.SuDungQuyTrinhPhucTap ?? false;
            string state = null;
            int hxl = 0;
            //int tiepDanTrung = Utils.ConvertToInt32(hdfTiepDanTrung.Value, 0);
            //int gianTiepTrung = Utils.ConvertToInt32(hdfGianTiepTrung.Value, 0);
            //int knLan2 = Utils.ConvertToInt32(hdfKNLan2.Value, 0);
            //int knLan2_edit = Utils.ConvertToInt32(hdfDonThuKNLan2.Value, 0);
            //int huongxuly = Utils.ConvertToInt32(ddlHuongXuLy.SelectedValue, 0);

            int tiepDanTrung = 0;
            int gianTiepTrung = 0;
            int knLan2 = Info.KNLan2 ?? 0;
            int knLan2_edit = Info.DonThuKNLan2 ?? 0;
            int huongxuly = Info.HuongGiaiQuyetID ?? 0;
            bool isTiepDanTrung = false;
            if (tiepDanTrung == 1 || gianTiepTrung == 1)
                isTiepDanTrung = true;
            else
                isTiepDanTrung = false;

            bool isKNLan2 = false;
            if (knLan2 == 1)
                isKNLan2 = true;
            else
                isKNLan2 = false;

            XuLyDonInfo xulydonInfo = new XuLyDonInfo();

            //int xulydonID = Utils.ConvertToInt32(hdfXuLyDonID.Value, 0); // Trùng đơn
            //int xuLyDonOldID = Utils.ConvertToInt32(hdfXuLyDonID.Value, 0);
            //int donID = Utils.ConvertToInt32(hdfDonThuID.Value, 0); // Trung đơn, KT lần 2
            int xulydonID = 0; // Trùng đơn
            int xuLyDonOldID = 0;
            int donID = 0; // Trung đơn, KT lần 2
            int donIDFile = 0;
            // TH trùng đơn
            if (xulydonID != 0 || donID != 0)
            {
                if (gianTiepTrung == 1)
                {
                    isInsertNewDonThu = true;
                    if (qTrinhPhucTap)
                    {
                        xulydonInfo = InsertXuLyDon(IdentityHelper, Info, donID, false, isKNLan2);
                    }
                    else
                    {
                        xulydonInfo = InsertXuLyDon(IdentityHelper, Info, donID, true, isKNLan2);
                    }
                    donIDFile = donID;
                    state = "Trung đơn";
                    xulydonID = xulydonInfo.XuLyDonID;

                    //CloneFileHoSo(xuLyDonOldID, xulydonID);
                    //CapNhatFileDinhKem(xulydonID, donID);
                    CapNhatFileDinhKem(Info, xulydonID, donID);
                }
                else
                {
                    isInsertNewDonThu = false;
                    xulydonInfo = new XuLyDonDAL().GetByID(xulydonID, string.Empty);
                    state = WorkflowInstance.Instance.GetCurrentStateOfDocument(xulydonID);

                    XuLyDonInfo xldinfo = new XuLyDonDAL().GetByID(xulydonID, "step4");
                    hxl = xldinfo.HuongGiaiQuyetID;
                }

                if (donID > 0 && isKNLan2)
                {
                    isInsertNewDonThu = true;
                    if (isKNLan2)
                    {
                        xulydonInfo = CapNhatXuLyDonData(IdentityHelper, Info);
                    }

                    xulydonID = xulydonInfo.XuLyDonID;
                    donID = xulydonInfo.DonThuID;
                    donIDFile = donID;
                    //CapNhatFileDinhKem(xulydonID, donID);
                    CapNhatFileDinhKem(Info, xulydonID, donID);
                }
            }

            if (state == Constant.CV_XuLy || state == Constant.LD_Phan_GiaiQuyet || isTiepDanTrung || isKNLan2)
            {
                #region update huong xu ly
                XuLyDonInfo xldInfo = xulydonInfo;

                xldInfo.XuLyDonID = xulydonID;//Utils.ConvertToInt32(hdf_xulydonId.Value, 0); 
                xldInfo.CoQuanID = IdentityHelper.CoQuanID ?? 0;
                xldInfo.CanBoXuLyID = IdentityHelper.CanBoID ?? 0;
                xldInfo.NgayXuLy = DateTime.Now;
                xldInfo.CQNgoaiHeThong = Info.CoQuanNgoaiHeThong;

                xldInfo.HuongGiaiQuyetID = Info.HuongGiaiQuyetID ?? 0;
                xldInfo.CoQuanID = IdentityHelper.CoQuanID ?? 0;
                xldInfo.NoiDungHuongDan = Info.NoiDungHuongDan;

                if (xldInfo.HuongGiaiQuyetID == (int)HuongGiaiQuyetEnum.DeXuatThuLy)
                {
                    xldInfo.CanBoKyID = Info.CanBoKyID ?? 0;
                    xldInfo.TrangThaiDonID = (int)TrangThaiDonEnum.DeXuatThuLy;
                }
                else if (xldInfo.HuongGiaiQuyetID == (int)HuongGiaiQuyetEnum.ChuyenDon)
                {
                    xldInfo.CanBoKyID = Info.CanBoKyID ?? 0;
                    xldInfo.NoiDungHuongDan = Info.NoiDungHuongDan;
                    if (IdentityHelper.SuDungQTVanThuTiepDan == true)
                    {
                        xldInfo.NgayChuyenDon = DateTime.MinValue;
                        xldInfo.CQChuyenDonID = 0;
                    }
                    else
                    {
                        xldInfo.NgayChuyenDon = Info.NgayChuyenDon ?? DateTime.MinValue;
                        xldInfo.CQChuyenDonID = Info.CQChuyenDonID ?? 0;
                    }
                }

                xulydonInfo = xldInfo;

                new XuLyDonDAL().UpdateHXL(xldInfo);
                #endregion
            }
            else
            {
                if (state == "Chuyên viên tiếp nhận" || state == null)
                {
                    xulydonInfo = CapNhatXuLyDonData(IdentityHelper, Info);
                    xulydonID = xulydonInfo.XuLyDonID;
                    donIDFile = xulydonInfo.DonThuID;
                }
            }

            #region== Update file dinh kem don khong trung

            //CapNhatFileDinhKem(xulydonID, donIDFile);
            CapNhatFileDinhKem(Info, xulydonID, donIDFile);
            #endregion 

            //if (isInsertNewDonThu)
            if (Info.XuLyDonID == null || Info.XuLyDonID == 0)
            {
                //Gan don thu vao workflow
                WorkflowInstance.Instance.AttachDocument(xulydonID, "XuLyDon", IdentityHelper.UserID ?? 0, DateTime.Now.AddDays(10));
            }

            #region y kien xu ly
            //CapNhatYKienXL(xulydonID);
            #endregion

            if (!IdentityHelper.SuDungQuyTrinhPhucTap == true)
            {
                #region == quy trinh don gian
                int ThuLy = 4;
                int KetThuc = 5;

                int huongXL = Info.HuongGiaiQuyetID ?? 0;
                int raVBDonDoc = (int)HuongGiaiQuyetEnum.RaVanBanDonDoc;
                int cVanChiDao = (int)HuongGiaiQuyetEnum.CongVanChiDao;

                var stateName = WorkflowInstance.Instance.GetCurrentStateOfDocument(xulydonID);
                if (stateName == Constant.CV_TiepNhan)
                {
                    if (xulydonInfo.HuongGiaiQuyetID == (int)HuongGiaiQuyetEnum.ChuyenDon)
                    {
                        //ChuyenDon(xulydonInfo);
                    }
                }

                if (stateName == Constant.Ket_Thuc || stateName == Constant.LD_Phan_GiaiQuyet)
                {

                    int nhomKNID = InsertChuDon(Info);
                    int donthuID = InsertDonThu(nhomKNID, Info);
                }
                if (stateName == Constant.CV_TiepNhan)
                {
                    new XuLyDonDAL().UpdateCanBoTiepNhan(xulydonID, IdentityHelper.CanBoID ?? 0);

                    List<string> commandList = WorkflowInstance.Instance.GetAvailabelCommands(xulydonID);
                    string command = string.Empty;

                    bool isDeXuatThuLy = false;
                    if (Info.HuongGiaiQuyetID == ((int)HuongGiaiQuyetEnum.DeXuatThuLy)) isDeXuatThuLy = true;

                    if (isDeXuatThuLy)
                    {
                        if (Info.LoaiKhieuTo1ID == Constant.TranhChap && IdentityHelper.CapID == (int)CapQuanLy.CapUBNDXa)
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

                    WorkflowInstance.Instance.ExecuteCommand(xulydonID, IdentityHelper.CanBoID ?? 0, command, DateTime.Now.AddDays(10), string.Empty);
                }

                #endregion
            }

            //if (state == Constant.CV_XuLy || state == Constant.CV_TiepNhan)
            //{
            //    hdfState.Value = state;
            //}


            //if (Data > 0)
            //{
            //    Result.Status = 1;
            //    Result.Message = "Phân xử lý thành công!";
            //}
            //else
            //{
            //    Result.Status = 0;
            //    Result.Message = "Phân xử lý thất bại!";
            //}
            Result.Data = Info;
            Result.Status = 1;
            return Result;
        }

        private void CapNhatFileDinhKem(TiepDanInfo tiepDanInfo, int xuLyDonID, int donThuID)
        {
            if (tiepDanInfo.DanhSachHoSoTaiLieu != null && tiepDanInfo.DanhSachHoSoTaiLieu.Count > 0)
            {
                foreach (var item in tiepDanInfo.DanhSachHoSoTaiLieu)
                {
                    // cập nhật file
                    if (item.DanhSachFileDinhKemID != null && item.DanhSachFileDinhKemID.Count > 0)
                    {
                        new FileDinhKemDAL().UpdateFileHoSo(item.DanhSachFileDinhKemID, xuLyDonID, donThuID, item.GroupUID);
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

        private XuLyDonInfo InsertXuLyDon(IdentityHelper IdentityHelper, TiepDanInfo Info, int donID, bool isInsertHuongXuLy, bool isKNLan2)
        {
            bool sDQTVanThuTiepNhanDon = IdentityHelper.SuDungQTVanThuTiepNhanDon ?? false;
            bool sDQTVanThuTiepDan = IdentityHelper.SuDungQTVanThuTiepDan ?? false;
            int qTrinhGianTiep = IdentityHelper.QuyTrinhGianTiep ?? 0;
            //int XuLyDonGocID = Utils.ConvertToInt32(hdfXuLyDonID.Value, 0);
            //int donThuGocID = Utils.ConvertToInt32(hdfDonThuGocID.Value, 0);
            int XuLyDonGocID = 0;
            int donThuGocID = 0;
            #region -- xu ly don info 
            XuLyDonInfo xldInfo = new XuLyDonInfo();
            xldInfo.XuLyDonChuyenID = XuLyDonGocID;
            int xulydonID = Info.XuLyDonID ?? 0;
            int huongxuly = Info.HuongGiaiQuyetID ?? 0;
            //int tiepDanTrung = Utils.ConvertToInt32(hdfTiepDanTrung.Value, 0);
            //int gianTiepTrung = Utils.ConvertToInt32(hdfGianTiepTrung.Value, 0);
            int tiepDanTrung = 0;
            int gianTiepTrung = 0;

            if (gianTiepTrung == 1 || tiepDanTrung == 1)
            {
                xulydonID = 0;
                xldInfo.SoLan = 2;
            }
            else
            {
                xldInfo.SoLan = 1;
            }

            if (isKNLan2)
            {
                xldInfo.LanGiaiQuyet = 2;
                xldInfo.DonThuGocID = donThuGocID;
            }
            else
            {
                xldInfo.LanGiaiQuyet = 1;
                xldInfo.DonThuGocID = 0;
            }

            xldInfo.DonThuID = donID;
            xldInfo.XuLyDonID = xulydonID;
            xldInfo.SoDonThu = GetSoDonThu(IdentityHelper, IdentityHelper.CoQuanID ?? 0);
            //xldInfo.NgayNhapDon = Info.NgayNhapDon ?? DateTime.Now;
            xldInfo.NgayNhapDon = Info.NgayTiep ?? Info.NgayNhapDon ?? DateTime.Now;

            //xldInfo.CQDaGiaiQuyetID = Info.TenCoQuanDaGQ;
            xldInfo.CQDaGiaiQuyetID = Info.CQDaGiaiQuyetID;

            xldInfo.NgayQuaHan = DateTime.MinValue;

            xldInfo.NguonDonDenID = Info.NguonDonDen ?? 0;
            xldInfo.CoQuanID = IdentityHelper.CoQuanID ?? 0;

            xldInfo.CQChuyenDonID = Info.CQChuyenDonID ?? 0;
            xldInfo.CQChuyenDonDenID = Info.CQChuyenDonDenID ?? 0;
            xldInfo.CQNgoaiHeThong = Info.CoQuanNgoaiHeThong;
            xldInfo.NgayCQKhacChuyenDonDen = Info.NgayCQKhacChuyenDonDen ?? DateTime.Now;
            xldInfo.SoCongVan = Info.SoCongVan;
            if (IdentityHelper.SuDungQuyTrinhPhucTap == true)
            {
                if (Info.LoaiKhieuTo1ID == Constant.TranhChap && huongxuly != 0 && IdentityHelper.CapID == (int)CapQuanLy.CapUBNDXa)
                {
                    xldInfo.NgayXuLy = DateTime.Now;
                }
                else
                {
                    xldInfo.NgayXuLy = DateTime.MinValue;
                }
            }
            else
            {
                xldInfo.NgayXuLy = DateTime.Now;
            }

            xldInfo.CBDuocChonXL = 0;
            xldInfo.QTTiepNhanDon = 0;
            #endregion

            if (IdentityHelper.SuDungQuyTrinhPhucTap == true)
            {
                #region -- su dung quy trinh phuc tap
                //int quyTrinhTiepNhan = Utils.ConvertToInt32(ddlQTTiepNhanDon.SelectedValue, 0);
                //int canBoDcChonXL = Utils.ConvertToInt32(ddlCanBoXuLy.SelectedValue, 0);

                int quyTrinhTiepNhan = 0;
                int canBoDcChonXL = 0;
                if (sDQTVanThuTiepNhanDon)
                {
                    xldInfo.CBDuocChonXL = canBoDcChonXL;
                    xldInfo.QTTiepNhanDon = quyTrinhTiepNhan;
                }
                if (sDQTVanThuTiepDan && xldInfo.NguonDonDenID == (int)EnumNguonDonDen.TrucTiep)
                {
                    xldInfo.CBDuocChonXL = canBoDcChonXL;
                    xldInfo.QTTiepNhanDon = (int)EnumQTTiepNhanDon.QTVanThuTiepDan;
                }

                if (qTrinhGianTiep == (int)EnumQTTiepNhanDon.QTGianTiepBTD)
                {
                    //xldInfo.CBDuocChonXL = Utils.ConvertToInt32(ddlCBXuLyQTrinhBTD.SelectedValue, 0);
                    xldInfo.CBDuocChonXL = Info.CBDuocChonXL ?? 0;
                    xldInfo.QTTiepNhanDon = (int)EnumQTTiepNhanDon.QTGianTiepBTD;
                }
                else if (qTrinhGianTiep == 0)
                {
                    // fix tam qt chuyen don (hxl chuyen don )
                }
                #endregion
            }

            if (isInsertHuongXuLy)
            {
                #region huong xu ly info
                xldInfo.CanBoXuLyID = IdentityHelper.CanBoID ?? 0;
                xldInfo.HuongGiaiQuyetID = Info.HuongGiaiQuyetID ?? 0;
                xldInfo.CoQuanID = IdentityHelper.CoQuanID ?? 0;
                xldInfo.ThuocThamQuyen = false;
                xldInfo.DuDieuKien = false;
                xldInfo.NoiDungHuongDan = Info.NoiDungHuongDan;

                if (xldInfo.HuongGiaiQuyetID == (int)HuongGiaiQuyetEnum.DeXuatThuLy)
                {
                    xldInfo.CanBoKyID = Info.CanBoKyID ?? 0;
                    xldInfo.TrangThaiDonID = (int)TrangThaiDonEnum.DeXuatThuLy;

                }
                else if (xldInfo.HuongGiaiQuyetID == (int)HuongGiaiQuyetEnum.ChuyenDon)
                {


                    xldInfo.NoiDungHuongDan = Info.NoiDungHuongDan;

                    if (IdentityHelper.SuDungQTVanThuTiepDan == true)
                    {
                        xldInfo.NgayChuyenDon = DateTime.MinValue;
                        xldInfo.CQChuyenDonID = 0;
                    }
                    else
                    {
                        xldInfo.CQChuyenDonID = Info.CQChuyenDonID ?? 0;
                        xldInfo.NgayChuyenDon = Info.NgayChuyenDon ?? Constant.DEFAULT_DATE;
                    }
                }
                #endregion
            }

            if (xulydonID == 0)
            {
                xldInfo.CanBoTiepNhapID = IdentityHelper.CanBoID ?? 0;
                xldInfo.XuLyDonID = new XuLyDonDAL().Insert(xldInfo);
            }
            else
            {
                new XuLyDonDAL().Update(xldInfo);
            }

            return xldInfo;
        }

        private XuLyDonInfo CapNhatXuLyDonData(IdentityHelper IdentityHelper, TiepDanInfo Info)
        {
            bool sDQTVanThuTiepNhanDon = IdentityHelper.SuDungQTVanThuTiepNhanDon ?? false;
            bool sDQTVanThuTiepDan = IdentityHelper.SuDungQTVanThuTiepDan ?? false;
            int qTrinhGianTiep = IdentityHelper.QuyTrinhGianTiep ?? 0;
            int kNLan2 = Info.KNLan2 ?? 0;
            int kNLan2_edit = Info.DonThuKNLan2 ?? 0;
            bool isKNLan2 = false;

            if (kNLan2 == 1 || kNLan2_edit == 1)
            {
                isKNLan2 = true;
            }

            //if (ValidateForm())
            //{
            if (IdentityHelper.SuDungQuyTrinhPhucTap == true)
            {
                #region quy trinh phuc tap
                int xulydonID = Info.XuLyDonID ?? 0;
                XuLyDonInfo xldInfo = null;

                var stateName = WorkflowInstance.Instance.GetCurrentStateOfDocument(xulydonID);

                if (xulydonID > 0)
                {
                    xldInfo = new XuLyDonDAL().GetByID(xulydonID, string.Empty);

                    if (stateName == "Chuyên viên tiếp nhận" || stateName == string.Empty)
                    {
                        if (xldInfo.NguonDonDenID == (int)EnumNguonDonDen.TrucTiep)
                        {
                            //int donthuID = InsertDonThu(nhomKNID, Info);
                            var insertedInfo = InsertXuLyDon(IdentityHelper, Info, xldInfo.DonThuID, true, isKNLan2);
                            return insertedInfo;
                        }
                    }
                    int nhomKNID = InsertChuDon(Info);
                    int donthuID = InsertDonThu(nhomKNID, Info);

                }


                if (stateName == "Chuyên viên tiếp nhận" || stateName == string.Empty)
                {
                    int nhomKNID = InsertChuDon(Info);
                    int donthuID = InsertDonThu(nhomKNID, Info);

                    XuLyDonInfo insertedInfo = new XuLyDonInfo();
                    if (sDQTVanThuTiepDan || sDQTVanThuTiepNhanDon)
                    {
                        insertedInfo = InsertXuLyDon(IdentityHelper, Info, donthuID, true, isKNLan2);
                    }
                    else if (qTrinhGianTiep == (int)EnumQTTiepNhanDon.QTGianTiepBTD)
                    {
                        insertedInfo = InsertXuLyDon(IdentityHelper, Info, donthuID, false, isKNLan2);
                    }
                    else
                    {
                        insertedInfo = InsertXuLyDon(IdentityHelper, Info, donthuID, false, isKNLan2);
                    }

                    return insertedInfo;
                }
                else if (stateName == Constant.CV_XuLy)
                {
                    var insertedInfo = InsertXuLyDon(IdentityHelper, Info, xldInfo.DonThuID, true, isKNLan2);
                    return insertedInfo;
                }
                #endregion
            }
            else
            {
                int nhomKNID = InsertChuDon(Info);
                int donthuID = InsertDonThu(nhomKNID, Info);
                var insertedInfo = InsertXuLyDon(IdentityHelper, Info, donthuID, true, isKNLan2);

                return insertedInfo;
            }

            //}
            return new XuLyDonInfo();
        }
        private int InsertChuDon(TiepDanInfo TiepDanInfo)
        {
            var Info = TiepDanInfo.NhomKN;
            NhomKNInfo nhomInfo = new NhomKNInfo();
            nhomInfo.SoLuong = Info.SoLuong ?? 1;
            nhomInfo.LoaiDoiTuongKNID = Info.LoaiDoiTuongKNID;
            nhomInfo.TenCQ = Info.TenCQ;
            nhomInfo.DiaChiCQ = Info.DiaChiCQ;

            nhomInfo.DaiDienPhapLy = false;
            nhomInfo.DuocUyQuyen = false;

            int nhomId = Info.NhomKNID;
            if (nhomId == 0)
            {
                nhomId = new NhomKN().Insert(nhomInfo);
            }
            else
            {
                nhomInfo.NhomKNID = nhomId;
                new NhomKN().Update(nhomInfo);
            }

            //_songuoidaidien = Utils.ConvertToInt32(ddl_songuoidaidien.SelectedValue, 1);

            //Them doi tuong kn/tc
            updateDoiTuongKNTC(TiepDanInfo, 1, nhomId);

            return nhomId;
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

        private string chuyenDoiChuHoa(string str)
        {
            string strchuyendoi = "";
            string[] laytu = str.Split(' ');
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

        private int InsertDonThu(int nhomKNID, TiepDanInfo data)
        {
            int donID = data.DonThuID ?? 0;

            DonThuInfo dtInfo = new DonThuInfo();
            dtInfo.NhomKNID = nhomKNID;
            //if (data.DoiTuongBiKN != null)
            //{
            //    int dtBiKNID = UpdateDoiTuongBiKN(data);
            //    if (dtBiKNID != 0) dtInfo.DoiTuongBiKNID = dtBiKNID;
            //}


            dtInfo.DonThuID = donID;

            dtInfo.LoaiKhieuTo1ID = data.LoaiKhieuTo1ID ?? 0;
            dtInfo.LoaiKhieuTo2ID = data.LoaiKhieuTo2ID ?? 0;
            dtInfo.LoaiKhieuTo3ID = data.LoaiKhieuTo3ID ?? 0;

            //LoaiKhieuToID max
            dtInfo.LoaiKhieuToID = dtInfo.LoaiKhieuTo3ID;
            dtInfo.NoiDungDon = data.NoiDungDon;
            dtInfo.NgayVietDon = data.NgayVietDon.Value;

            //Địa chỉ phát sinh
            dtInfo.DiaChiPhatSinh = data.DiaChiPhatSinh;
            dtInfo.TinhID = data.TinhID ?? 0;
            dtInfo.HuyenID = data.HuyenID ?? 0;
            dtInfo.XaID = data.XaID ?? 0;
            if (dtInfo.DonThuID == 0)
            {
                dtInfo.DonThuID = new DonThuDAL().Insert(string.Empty, dtInfo);
            }
            else
            {
                new DonThuDAL().Update(string.Empty, dtInfo);
            }

            if (data.DanhSachDoiTuongBiKN != null && data.DanhSachDoiTuongBiKN.Count > 0)
            {
                UpdateDoiTuongBiKN_V2(data, dtInfo.DonThuID);
            }

            return dtInfo.DonThuID;
        }

        protected int UpdateDoiTuongBiKN(TiepDanInfo data)
        {

            if (data.DoiTuongBiKN == null)
            {
                return 0;
            }
            var Info = data.DoiTuongBiKN;
            int doituongbiknId = Info.DoiTuongBiKNID;
            int canhanbiknId = Info.CaNhanBiKNID ?? 0;

            //lay LoaiDoiTuongBiKNID
            int loaiBiKNID = Info.LoaiDoiTuongBiKNID ?? 0;

            //insert doi tuong bi kn
            DoiTuongBiKNInfo dtInfo = new DoiTuongBiKNInfo();

            dtInfo.DiaChiCT = Info.DiaChiCT;
            dtInfo.HuyenID = Info.HuyenID;
            dtInfo.TinhID = Info.TinhID;
            dtInfo.XaID = Info.XaID;
            dtInfo.LoaiDoiTuongBiKNID = loaiBiKNID;//Utils.ConvertToInt32(hoso_type.SelectedValue, bikn_canhanID);

            if (loaiBiKNID == DMLoaiDoiTuongBiKN.CoQuanToChuc.GetHashCode())
            {
                dtInfo.TenDoiTuongBiKN = Info.TenDoiTuongBiKN;
            }
            //------------------------------------TH CA NHAN-------------------------------------------
            //insert ca nhan bi kn/tc neu (truong hop chon loai doi tuong la ca nhan)           
            if (loaiBiKNID == DMLoaiDoiTuongBiKN.CaNhan.GetHashCode())
            {
                //dtInfo.TenDoiTuongBiKN = chuyenDoiChuHoa(txt_hoten.Text.Trim());
                dtInfo.TenDoiTuongBiKN = Info.TenDoiTuongBiKN;
            }

            if (doituongbiknId == 0)
                doituongbiknId = new DoiTuongBiKN().Insert(dtInfo);
            else
            {
                dtInfo.DoiTuongBiKNID = doituongbiknId;
                new DoiTuongBiKN().Update(dtInfo);
                //new DAL.CaNhanBiKN().Delete(canhanbiknId);
            }

            //TH doi tuong bi Kn là ca nhan, insert them thong tin ca nhan bi KN
            if (loaiBiKNID == DMLoaiDoiTuongBiKN.CaNhan.GetHashCode())
            {
                CaNhanBiKNInfo cnInfo = new CaNhanBiKNInfo();
                cnInfo.CaNhanBiKNID = canhanbiknId;
                cnInfo.NgheNghiep = Info.TenNgheNghiep;
                cnInfo.ChucVuID = Info.ChucVuID;
                cnInfo.QuocTichID = Info.QuocTichDoiTuongBiKNID;
                cnInfo.DanTocID = Info.DanTocDoiTuongBiKNID;
                cnInfo.NoiCongTac = Info.NoiCongTacDoiTuongBiKN;
                cnInfo.DoiTuongBiKNID = doituongbiknId;

                if (canhanbiknId == 0)
                {
                    new CaNhanBiKN().Insert(cnInfo);
                }
                else
                {
                    cnInfo.CaNhanBiKNID = canhanbiknId;
                    //canhanbiknId = new DAL.CaNhanBiKN().GetByID
                    new CaNhanBiKN().Update(cnInfo);
                }

            }

            return doituongbiknId;
        }

        protected void UpdateDoiTuongBiKN_V2(TiepDanInfo tiepDanInfo, int donThuID)
        {
            if (tiepDanInfo.DanhSachDoiTuongBiKN != null && tiepDanInfo.DanhSachDoiTuongBiKN.Count > 0)
            {
                var listIds = new List<int>();
                foreach (var data in tiepDanInfo.DanhSachDoiTuongBiKN)
                {
                    var Info = data;
                    int doituongbiknId = Info.DoiTuongBiKNID;
                    int canhanbiknId = Info.CaNhanBiKNID ?? 0;

                    //lay LoaiDoiTuongBiKNID
                    int loaiBiKNID = Info.LoaiDoiTuongBiKNID ?? 0;

                    //insert doi tuong bi kn
                    DoiTuongBiKNInfo dtInfo = new DoiTuongBiKNInfo();

                    dtInfo.DonThuID = donThuID;
                    dtInfo.DiaChiCT = Info.DiaChiCT;
                    dtInfo.HuyenID = Info.HuyenID;
                    dtInfo.TinhID = Info.TinhID;
                    dtInfo.XaID = Info.XaID;
                    dtInfo.LoaiDoiTuongBiKNID = loaiBiKNID;//Utils.ConvertToInt32(hoso_type.SelectedValue, bikn_canhanID);
                    dtInfo.GioiTinhDoiTuongBiKN = Info.GioiTinhDoiTuongBiKN;
                    dtInfo.TenChucVu = Info.TenChucVu;

                    if (loaiBiKNID == DMLoaiDoiTuongBiKN.CoQuanToChuc.GetHashCode())
                    {
                        dtInfo.TenDoiTuongBiKN = Info.TenDoiTuongBiKN;
                    }
                    //------------------------------------TH CA NHAN-------------------------------------------
                    //insert ca nhan bi kn/tc neu (truong hop chon loai doi tuong la ca nhan)           
                    if (loaiBiKNID == DMLoaiDoiTuongBiKN.CaNhan.GetHashCode())
                    {
                        //dtInfo.TenDoiTuongBiKN = chuyenDoiChuHoa(txt_hoten.Text.Trim());
                        dtInfo.TenDoiTuongBiKN = Info.TenDoiTuongBiKN;
                    }

                    if (doituongbiknId == 0)
                        doituongbiknId = new DoiTuongBiKN().Insert(dtInfo);

                    else
                    {
                        dtInfo.DoiTuongBiKNID = doituongbiknId;
                        new DoiTuongBiKN().Update(dtInfo);
                        //new DAL.CaNhanBiKN().Delete(canhanbiknId);
                    }


                    //TH doi tuong bi Kn là ca nhan, insert them thong tin ca nhan bi KN
                    if (loaiBiKNID == DMLoaiDoiTuongBiKN.CaNhan.GetHashCode())
                    {
                        CaNhanBiKNInfo cnInfo = new CaNhanBiKNInfo();
                        cnInfo.CaNhanBiKNID = canhanbiknId;
                        cnInfo.NgheNghiep = Info.TenNgheNghiep;
                        cnInfo.ChucVuID = Info.ChucVuID;
                        cnInfo.QuocTichID = Info.QuocTichDoiTuongBiKNID;
                        cnInfo.DanTocID = Info.DanTocDoiTuongBiKNID;
                        cnInfo.NoiCongTac = Info.NoiCongTacDoiTuongBiKN;
                        cnInfo.DoiTuongBiKNID = doituongbiknId;

                        if (canhanbiknId == 0)
                        {
                            new CaNhanBiKN().Insert(cnInfo);
                        }
                        else
                        {
                            cnInfo.CaNhanBiKNID = canhanbiknId;
                            //canhanbiknId = new DAL.CaNhanBiKN().GetByID
                            new CaNhanBiKN().Update(cnInfo);
                        }
                    }
                    listIds.Add(doituongbiknId);
                }
                new DoiTuongBiKN().DeleteByDonThuID(string.Join(",", listIds.Select(x => x)), tiepDanInfo.DonThuID ?? 0);
            }
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

        public TiepDanInfo GetByID(int XuLyDonID)
        {
            TiepDanInfo TiepDanInfo = new TiepDanInfo();
            DTXuLyInfo DTXuLyInfo = new DTXuLy().GetByID(XuLyDonID);
            TiepDanInfo.DonThuID = DTXuLyInfo.DonThuID;
            TiepDanInfo.XuLyDonID = DTXuLyInfo.XuLyDonID;
            TiepDanInfo.NhomKNID = DTXuLyInfo.NhomKNID;
            TiepDanInfo.NgayNhapDon = DTXuLyInfo.NgayNhapDon;
            TiepDanInfo.NguonDonDen = DTXuLyInfo.NguonDonDen;
            TiepDanInfo.CQChuyenDonDenID = DTXuLyInfo.CQChuyenDonDenID;
            TiepDanInfo.SoDonThu = DTXuLyInfo.SoDonThu;
            TiepDanInfo.CQDaGiaiQuyetID = DTXuLyInfo.CQDaGiaiQuyetID;

            //var TiepDanInfo = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().GetTiepDanByTiepDanID(TiepDanID);
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
                TiepDanInfo.TinhID = donthuInfo.TinhID;
                TiepDanInfo.HuyenID = donthuInfo.HuyenID;
                TiepDanInfo.XaID = donthuInfo.XaID;
                TiepDanInfo.NgayTiep = donthuInfo.NgayTiep;
                TiepDanInfo.LoaiKhieuTo1ID = donthuInfo.LoaiKhieuTo1ID;
                TiepDanInfo.LoaiKhieuTo2ID = donthuInfo.LoaiKhieuTo2ID;
                TiepDanInfo.LoaiKhieuTo3ID = donthuInfo.LoaiKhieuTo3ID == 0 ? null : donthuInfo.LoaiKhieuTo3ID;
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
                //    TiepDanInfo.DoiTuongBiKN.QuocTichDoiTuongBiKNID = canhanbiknInfo.QuocTichID;
                //    TiepDanInfo.DoiTuongBiKN.NoiCongTacDoiTuongBiKN = canhanbiknInfo.NoiCongTac;
                //    TiepDanInfo.DoiTuongBiKN.DanTocDoiTuongBiKNID = canhanbiknInfo.DanTocID;
                //    TiepDanInfo.DoiTuongBiKN.NoiCongTacDoiTuongBiKN = canhanbiknInfo.NoiCongTac;
                //    TiepDanInfo.DoiTuongBiKN.GioiTinhDoiTuongBiKN = canhanbiknInfo.GioiTinh;
                //}
            }
            //TiepDanInfo.ThanhPhanThamGia = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().GetThanhPhanThamGia(TiepDanID).ToList();

            #region file hồ sơ tài liệu bằng chứng
            List<FileHoSoInfo> fileDinhKem = new List<FileHoSoInfo>();
            CanBoInfo canBoInfo = new CanBo().GetCanBoByID(TiepDanInfo.CanBoTiepID ?? 0);
            if (canBoInfo != null && canBoInfo.XemTaiLieuMat)
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

            #endregion


            #region file trong tiếp dân, tiếp nhận đơn

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
                #endregion

            }
            #endregion

            return TiepDanInfo;
        }

        public BaseResultModel Delete(int DonThuID, int XuLyDonID)
        {
            var Result = new BaseResultModel();
            int kq = 0;
            if (DonThuID != 0)
            {
                try
                {
                    kq = new XuLyDonDAL().Delete_DonThuDaTiepNhan(DonThuID, XuLyDonID);
                    Result.Message = Constant.CONTENT_DELETE_SUCCESS;
                    Result.Status = 1;
                    Result.Data = kq;
                }
                catch
                {

                    Result.Message = Constant.CONTENT_DELETE_ERROR;
                    Result.Status = 0;
                }
            }

            return Result;
        }

        public BaseResultModel ChuyenTNDSangTDTT(IdentityHelper IdentityHelper, string xuLyDonIDIds, string donThuIDIds)
        {
            var result = new BaseResultModel();
            try
            {
                // thêm dữ liệu cho tiếp dân không đơn
                result = new XuLyDonDAL().ChuyenDon_Sang_TiepDanThuongXuyen(IdentityHelper, xuLyDonIDIds, donThuIDIds);
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
