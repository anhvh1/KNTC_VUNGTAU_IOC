using Com.Gosol.KNTC.DAL.KNTC;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Ultilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Gosol.KNTC.Models.HeThong;
using Workflow;
using Utils = Com.Gosol.KNTC.Ultilities.Utils;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Com.Gosol.KNTC.DAL.HeThong;
using Com.Gosol.KNTC.Security;

namespace Com.Gosol.KNTC.BUS.KNTC
{
    public class GiaoXacMinhBUS
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
            queryFilter.TrangThai = p.TrangThai;
            queryFilter.StateName = Constant.LD_Phan_GiaiQuyet;
            queryFilter.CanBoID = IdentityHelper.CanBoID ?? 0;
            queryFilter.ChuTichUBND = IdentityHelper.ChuTichUBND ?? 0;

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
            string data = "";

            #region role lanh dao
            if (IdentityHelper.RoleID == RoleEnum.LanhDao.GetHashCode() || IdentityHelper.RoleID == RoleEnum.ChuyenVien.GetHashCode())
            {
                if (IdentityHelper.RoleID == RoleEnum.ChuyenVien.GetHashCode())
                {
                    queryFilter.CanBoID = 0;
                }
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


                    if (donThuInfo.ChuyenGiaiQuyetID > 0)
                    {
                        donThuInfo.TenTrangThai = "Đã cập nhật quyết định";
                        donThuInfo.TrangThai = 1;
                    }
                    else
                    {
                        donThuInfo.TenTrangThai = "Chưa cập nhật quyết định";
                        donThuInfo.TrangThai = 0;
                    }

                    if (capUBND)
                    {

                        if (donThuInfo.StateID == 7)
                        {
                            donThuInfo.TenTrangThai = "Chưa cập nhật quyết định";
                            donThuInfo.TrangThai = 0;

                            if (donThuInfo.SoNgayConLai < 5)
                            {
                                donThuInfo.TrangThaiQuaHan = 1;
                            }
                            else
                            {
                                donThuInfo.TrangThaiQuaHan = 0;
                            }
                        }
                        else
                        {
                            donThuInfo.TenTrangThai = "Đã cập nhật quyết định";
                            donThuInfo.TrangThai = 1;
                        }
                    }

                    if (IdentityHelper.CapThanhTra == 1)
                    {
                        donThuInfo.TenTrangThai = "Chưa cập nhật quyết định";
                        donThuInfo.TrangThai = 0;

                        if (donThuInfo.StateID == 7 || donThuInfo.StateID == 18)
                        {

                            if (donThuInfo.SoNgayConLai < 5)
                            {
                                donThuInfo.TrangThaiQuaHan = 1;
                            }
                            else
                            {
                                donThuInfo.TrangThaiQuaHan = 0;
                            }
                        }
                        else
                        {
                            if (donThuInfo.StateID == 19 && IdentityHelper.RoleID == 2)
                            {

                                donThuInfo.TenTrangThai = "Chưa cập nhật quyết định";
                                donThuInfo.TrangThai = 0;

                                if (donThuInfo.SoNgayConLai < 5)
                                {
                                    donThuInfo.TrangThaiQuaHan = 1;
                                }
                                else
                                {
                                    donThuInfo.TrangThaiQuaHan = 0;
                                }

                            }
                            else
                            {

                                donThuInfo.TenTrangThai = "Đã cập nhật quyết định";
                                donThuInfo.TrangThai = 1;
                                if (donThuInfo.StateID == 8)
                                {
                                    if (donThuInfo.SoNgayConLai < 5)
                                    {
                                        donThuInfo.TrangThaiQuaHan = 1;
                                    }
                                    else
                                    {
                                        donThuInfo.TrangThaiQuaHan = 0;
                                    }
                                }
                            }

                            if (IdentityHelper.RoleID == 1)
                            {
                                if (donThuInfo.StateID == 21 || donThuInfo.StateID == 19)
                                {
                                    if (donThuInfo.SoNgayConLai < 5)
                                    {
                                        donThuInfo.TrangThaiQuaHan = 1;
                                    }
                                    else
                                    {
                                        donThuInfo.TrangThaiQuaHan = 0;
                                    }
                                }
                            }

                        }
                    }

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

            #region role chuyen vien
            //if (IdentityHelper.RoleID == RoleEnum.ChuyenVien.GetHashCode())
            //{
            //    //queryFilter.PhongBanID = IdentityHelper.GetPhongID();
            //    //queryFilter.TuNgay = Utils.ConvertToDateTime(txtTuNgay, DateTime.MinValue);
            //    //queryFilter.DenNgay = Utils.ConvertToDateTime(txtDenNgay, DateTime.MinValue);

            //    try
            //    {
            //        //donThuList = new DonThuGiaiQuyet().GetDonThuCanPhanGiaiQuyet_CV(queryFilter);
            //        if (p.LocDonPhanHoi == true)
            //        {
            //            donThuList = new DonThuGiaiQuyet().GetDonThuCanPhanGiaiQuyet_CV_PhanHoi_filter(queryFilter);
            //        }
            //        else
            //        {
            //            donThuList = new DonThuGiaiQuyet().GetDonThuCanPhanGiaiQuyet_CV_PhanHoi(queryFilter);

            //        }
            //    }
            //    catch
            //    {
            //    }

            //    foreach (var donThuInfo in donThuList)
            //    {
            //        if (donThuInfo.CoQuanID != IdentityHelper.CoQuanID)
            //        {
            //            donThuInfo.NgayQuaHan = donThuInfo.HanGQMoi;
            //        }
            //        else
            //            donThuInfo.NgayQuaHan = donThuInfo.HanGQGoc;

            //        donThuInfo.NgayQuaHanStr = Format.FormatDate(donThuInfo.NgayQuaHan);

            //        if (donThuInfo.NgayQuaHan != DateTime.MinValue)
            //            donThuInfo.SoNgayConLai = donThuInfo.NgayQuaHan.Subtract(DateTime.Now).Days;
            //        else
            //            donThuInfo.SoNgayConLai = 5;
            //    }

            //}
            #endregion

            try
            {
                #region lay full ten chu don
                //List<DTXuLyInfo> listNhomKN = new List<DTXuLyInfo>();
                //foreach (DonThuGiaiQuyetInfo dt in donThuList)
                //{
                //    DTXuLyInfo nhomKN = new DTXuLyInfo();
                //    nhomKN.NhomKNID = dt.NhomKNID;
                //    listNhomKN.Add(nhomKN);
                //}

                //List<DoiTuongKNInfo> listDoiTuongKN = new DAL.DoiTuongKN().GetDiaChiDTKhieuNaiByNhomKN(listNhomKN);
                //foreach (DonThuGiaiQuyetInfo dt in donThuList)
                //{
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


                #endregion
            }
            catch
            {
            }

            return donThuList;
        }

        public BaseResultModel CapNhatQuyetDinhGiaoXacMinh(IdentityHelper IdentityHelper, GiaoXacMinhModel GiaoXacMinhModel)
        {
            var Result = new BaseResultModel();
            int ChuyenGiaiQuyetID = PhanChoCoQuanKhac(IdentityHelper, GiaoXacMinhModel);
            //DeleteCQPhoiHop
            new ChuyenGiaiQuyet().DeleteCQPhoiHop(GiaoXacMinhModel.XuLyDonID ?? 0);
            //InsertCQPhoiHop
            if (GiaoXacMinhModel.CQPhoiHopGQ != null && GiaoXacMinhModel.CQPhoiHopGQ.Count > 0)
            {
                foreach (var item in GiaoXacMinhModel.CQPhoiHopGQ)
                {
                    item.XuLyDonID = GiaoXacMinhModel.XuLyDonID ?? 0;
                    int val = new ChuyenGiaiQuyet().InsertCQPhoiHopGQ(item);
                }
            }
            if (ChuyenGiaiQuyetID > 0 && GiaoXacMinhModel.DanhSachHoSoTaiLieu != null)
            {
                foreach (var item in GiaoXacMinhModel.DanhSachHoSoTaiLieu)
                {
                    if (item.DanhSachFileDinhKemID != null && item.DanhSachFileDinhKemID.Count > 0)
                    {
                        new FileDinhKemDAL().UpdateFileGiaoXacMinh(item.DanhSachFileDinhKemID, ChuyenGiaiQuyetID);
                    }
                }
            }
            Result.Status = 1;
            Result.Data = ChuyenGiaiQuyetID;
            Result.Message = "Cập nhật thành công";
            return Result;
        }

        public int PhanChoCoQuanKhac(IdentityHelper IdentityHelper, GiaoXacMinhModel GiaoXacMinhModel)
        {
            var kq = new ChuyenGiaiQuyet().PhanHoiDelete(GiaoXacMinhModel.XuLyDonID ?? 0);
            int docunmentid = GiaoXacMinhModel.XuLyDonID ?? 0;
            int LDPHAN_GQ_CQCAPDUOI = 1;
            string commandCode;

            string stateName = WorkflowInstance.Instance.GetCurrentStateOfDocument(docunmentid);
            if (stateName == Constant.LD_Phan_GiaiQuyet)
            {
                //string commandCode = WorkflowInstance.Instance.GetAvailabelCommands(docunmentid)[LDPHAN_GQ_CQCAPDUOI];
                commandCode = WorkflowInstance.Instance.GetAvailabelCommands(docunmentid).Where(x => x.ToString() == "LDPhanGQCapDuoi").FirstOrDefault();

                DateTime duedate = Utils.ConvertToDateTime(GiaoXacMinhModel.HanGiaiQuyet, DateTime.Now);
                int canboid = IdentityHelper.CanBoID ?? 0;
                WorkflowInstance.Instance.ExecuteCommand(docunmentid, canboid, commandCode, duedate, GiaoXacMinhModel.GhiChu ?? "");
                int xulydonID = GiaoXacMinhModel.XuLyDonID ?? 0;
                int stateID = new DonThuDAL().GetByXuLyDonID(xulydonID).StateID;
                //DateTime ngayHetHanCu = Utils.ConvertToDateTime(ngayhethancu, DateTime.MinValue);
                int CanBoID = Utils.ConvertToInt32(canboid, 0);
                //KetQuaInfo ketqua = new KetQuaInfo();

                var ketqua = new KetQuaDAL().Update_PreState(stateID, xulydonID, CanBoID);
            }
            else if (stateName == Constant.TP_Phan_GiaiQuyet)
            {
                commandCode = WorkflowInstance.Instance.GetAvailabelCommands(docunmentid).Where(x => x.ToString() == "TPPhanTDoanGQ").FirstOrDefault();

                DateTime duedate = Utils.ConvertToDateTime(GiaoXacMinhModel.HanGiaiQuyet, DateTime.Now);
                int canboid = IdentityHelper.CanBoID ?? 0;
                WorkflowInstance.Instance.ExecuteCommand(docunmentid, canboid, commandCode, duedate, GiaoXacMinhModel.GhiChu ?? "");
                int xulydonID = GiaoXacMinhModel.XuLyDonID ?? 0;
                int stateID = new DonThuDAL().GetByXuLyDonID(xulydonID).StateID;
                //DateTime ngayHetHanCu = Utils.ConvertToDateTime(ngayhethancu, DateTime.MinValue);
                int CanBoID = Utils.ConvertToInt32(canboid, 0);
                //KetQuaInfo ketqua = new KetQuaInfo();

                var ketqua = new KetQuaDAL().Update_PreState(stateID, xulydonID, CanBoID);
            }

            #region -- chuyen giai quyet info 
            ChuyenGiaiQuyetInfo info = new ChuyenGiaiQuyetInfo();
            int CQGiaiQuyetID = GiaoXacMinhModel.CoQuanID ?? 0;
            //int docunmentid = Utils.ConvertToInt32(xulydonid, 0);
            info.XuLyDonID = docunmentid;
            info.CoQuanPhanID = IdentityHelper.CoQuanID ?? 0;
            info.CoQuanGiaiQuyetID = CQGiaiQuyetID;
            info.GhiChu = GiaoXacMinhModel.GhiChu;
            info.NgayChuyen = DateTime.Now;
            info.NgayQuyetDinh = GiaoXacMinhModel.NgayQuyetDinh;
            info.SoQuyetDinh = GiaoXacMinhModel.SoQuyetDinh;
            info.QuyetDinh = GiaoXacMinhModel.QuyetDinh;
            //info.FileUrl = fileUrl;
            #endregion


            int val = 0;

            try
            {
                new ChuyenGiaiQuyet().DeleteChuyenCQKhac(docunmentid);
            }
            catch { }

            try
            {
                val = new ChuyenGiaiQuyet().Insert(info);

            }
            catch { }

            //try
            //{
            //    String fileDataStr = fileUrl;
            //    if (fileDataStr != string.Empty)
            //    {
            //        new ChuyenGiaiQuyet().DeleteFileChuyenGiaiQuyet(docunmentid, IdentityHelper.GetCoQuanID(), IdentityHelper.GetCanBoID(), (int)EnumLoaiFile.FileDTCPGQ);
            //        string[] fileParts = fileDataStr.Split(';');
            //        for (int i = 0; i < fileParts.Length; i++)
            //        {

            //            string fileStr = fileParts[i];
            //            string[] dataParts = fileStr.Split(',');

            //            FileHoSoInfo infoHoSo = new FileHoSoInfo();
            //            infoHoSo.ChuyenGiaiQuyetID = val;
            //            infoHoSo.FileURL = dataParts[0];
            //            infoHoSo.NgayUp = Utils.ConvertToDateTime(dataParts[1], Constant.DEFAULT_DATE);
            //            infoHoSo.TenFile = dataParts[2];
            //            infoHoSo.TomTat = dataParts[3];
            //            infoHoSo.NguoiUp = IdentityHelper.GetCanBoID();//IdentityHelper.GetUserID();
            //            infoHoSo.FileID = Utils.ConvertToInt32(dataParts[6], 0);
            //            FileLogInfo infoFileLog = new FileLogInfo();
            //            infoFileLog.LoaiLog = (int)EnumLoaiLog.Them;
            //            infoFileLog.LoaiFile = (int)EnumLoaiFile.FileDTCPGQ;
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
            //                int result = new FileHoSo().InsertFileDonThuCanPhanGiaiQuyet(infoHoSo);
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
            //catch (Exception ex)
            //{
            //}

            return val;

        }

        public static string InsertUpdatePhanGiaiQuyetArr(string xulydonid, string canboid, string vaitro, string chuyengiaiquyetid, string hoatdong)
        {
            int docunmentid = Utils.ConvertToInt32(xulydonid, 0);

            VaiTroGiaiQuyetInfo info = new VaiTroGiaiQuyetInfo();

            int val = 0;

            try
            {
                string[] listCB = canboid.Split(',');
                string[] listVT = vaitro.Split(',');
                string[] listHD = hoatdong.Split(',');
                if (listCB.Length > 1)
                {
                    for (int i = 1; i < listCB.Length; i++)
                    {
                        int hd = Utils.ConvertToInt32(listHD[i], 0);

                        info.XuLyDonID = docunmentid;
                        info.CanBoID = Utils.ConvertToInt32(listCB[i], 0);
                        info.VaiTro = Utils.ConvertToInt32(listVT[i], 0);
                        info.ChuyenGiaiQuyetID = Utils.ConvertToInt32(chuyengiaiquyetid, 0);
                        info.HoatDong = hd;
                        val = new VaiTroGiaiQuyet().InsertUpdate(info);

                    }
                }

            }
            catch
            {
            }

            try
            {
                #region -- send mail
                /*
                XuLyDonInfo xuLyDonInfo = new XuLyDonInfo();
                xuLyDonInfo = new XuLyDon().GetByID(docunmentid);
                List<string> emailList = new List<string>();
                CanBoInfo canBoInfo = new CanBoInfo();
                canBoInfo = new CanBo().GetCanBoByID(info.CanBoID);

                QL_EmailInfo Eminfo = new QL_Email().GetByLoaiEmail(Constant.DM_EMAIL_GIAOXM);
                List<EmailInfo> lst_Email_ND = new List<EmailInfo>();
                string emailTitle = string.Empty;
                if (canBoInfo != null)
                {
                    if (canBoInfo.Email != "")
                    {
                        EmailInfo em_info = new EmailInfo();
                        em_info.Email = canBoInfo.Email;

                        if (canBoInfo.GioiTinh == 0)
                        {
                            string[] arry_ten = canBoInfo.TenCanBo.Split(' ');
                            string ten = string.Empty;
                            if (arry_ten.Length > 0)
                            {
                                ten = arry_ten[arry_ten.Length - 1];
                            }
                            else
                            {
                                ten = arry_ten[0];
                            }
                            if (Eminfo.EmailID != 0)
                            {
                                em_info.NoiDungEmail = Utils.ConvertToString(Eminfo.NoiDungEmail, string.Empty).Replace("#so_don", xuLyDonInfo.SoDonThu).Replace("#can_bo", ten).Replace("#gioi_tinh", "Ông/Bà");
                            }
                        }
                        else if (canBoInfo.GioiTinh == 1)
                        {
                            string[] arry_ten = canBoInfo.TenCanBo.Split(' ');
                            string ten = string.Empty;
                            if (arry_ten.Length > 0)
                            {
                                ten = arry_ten[arry_ten.Length - 1];
                            }
                            else
                            {
                                ten = arry_ten[0];
                            }
                            if (Eminfo.EmailID != 0)
                            {
                                em_info.NoiDungEmail = Utils.ConvertToString(Eminfo.NoiDungEmail, string.Empty).Replace("#so_don", xuLyDonInfo.SoDonThu).Replace("#can_bo", ten).Replace("#gioi_tinh", "Ông");
                            }
                        }
                        else
                        {
                            string[] arry_ten = canBoInfo.TenCanBo.Split(' ');
                            string ten = string.Empty;
                            if (arry_ten.Length > 0)
                            {
                                ten = arry_ten[arry_ten.Length - 1];
                            }
                            else
                            {
                                ten = arry_ten[0];
                            }
                            if (Eminfo.EmailID != 0)
                            {
                                em_info.NoiDungEmail = Utils.ConvertToString(Eminfo.NoiDungEmail, string.Empty).Replace("#so_don", xuLyDonInfo.SoDonThu).Replace("#can_bo", ten).Replace("#gioi_tinh", "Bà");
                            }
                        }

                        lst_Email_ND.Add(em_info);
                    }

                }
                if (Eminfo.EmailID != 0)
                {
                    emailTitle = Utils.ConvertToString(Eminfo.TenEmail, string.Empty);
                }
                else
                {
                }
                string fromEmail = ConfigurationSettings.AppSettings["Email"];
                string passWord = ConfigurationSettings.AppSettings["PassEmail"];

                if (lst_Email_ND != null && lst_Email_ND.Count > 0)
                {
                    SystemConfigInfo smtpServer = new SystemConfig().GetByKey("SMTP_SERVER");
                    SystemConfigInfo smtpPort = new SystemConfig().GetByKey("SMTP_PORT");
                    if (smtpServer != null)
                    {
                        string emailServer = smtpServer.ConfigValue;
                        if (smtpPort != null)
                        {
                            int port = Utils.ConvertToInt32(smtpPort.ConfigValue, 0);
                            //MailHelper.SendEmail(emailList, emailTitle, emailContent, fromEmail, passWord, emailServer, port);
                            MailHelper.SendEmail_obj(lst_Email_ND, emailTitle, fromEmail, passWord, emailServer, port);
                        }
                        else
                        {
                            //MailHelper.SendEmail(emailList, emailTitle, emailContent, fromEmail, passWord, emailServer);
                            MailHelper.SendEmail_obj(lst_Email_ND, emailTitle, fromEmail, passWord);
                        }
                    }
                    else
                    {
                        //MailHelper.SendEmail(emailList, emailTitle, emailContent, fromEmail, passWord);
                        MailHelper.SendEmail_obj(lst_Email_ND, emailTitle, fromEmail, passWord);
                    }
                }
                */
                #endregion
            }
            catch
            {

            }
            string data = "";
            return data;

        }

        public static string InsertPhanXL(string stateid, string ngayhethancu, string xulydonid, string canboid)
        {
            int stateID = Utils.ConvertToInt32(stateid, 0);
            int xulydonID = Utils.ConvertToInt32(xulydonid, 0);
            DateTime ngayHetHanCu = Utils.ConvertToDateTime(ngayhethancu, DateTime.MinValue);
            int CanBoID = Utils.ConvertToInt32(canboid, 0);
            //KetQuaInfo ketqua = new KetQuaInfo();
            try
            {
                var ketqua = new KetQuaDAL().InsertPreStateDueDate(stateID, xulydonID, ngayHetHanCu, CanBoID);
            }
            catch (Exception ex)
            {
            }

            string data = "";
            return data;

        }

        public static string InsertHistory(IdentityHelper IdentityHelper, string xulydonid, string hangiaiquyet, string txtghichu, string fileUrl, string truongPhongID, string isCapThanhTra)
        {
            var kq = new ChuyenGiaiQuyet().PhanHoiDelete(Utils.ConvertToInt32(xulydonid, 0));

            int docunmentid = Utils.ConvertToInt32(xulydonid, 0);
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
                    }

                    else
                        commandCode = WorkflowInstance.Instance.GetAvailabelCommands(docunmentid).Where(x => x.ToString() == "LDPhanTruongDoanGQ").FirstOrDefault();
                }
                else if (stateName == Constant.LD_CapDuoi_Phan_GiaiQuyet)
                {
                    if (suDungQuyTrinhGQ)
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

                DateTime duedate = Utils.ConvertToDateTime(hangiaiquyet, DateTime.Now);
                int canboid = IdentityHelper.CanBoID ?? 0;
                if (commandCode != "")
                    WorkflowInstance.Instance.ExecuteCommand(docunmentid, canboid, commandCode, duedate, txtghichu);
                int xulydonID = Utils.ConvertToInt32(xulydonid, 0);
                int stateID = new DonThuDAL().GetByXuLyDonID(xulydonID).StateID;
                //DateTime ngayHetHanCu = Utils.ConvertToDateTime(ngayhethancu, DateTime.MinValue);
                int CanBoID = Utils.ConvertToInt32(canboid, 0);
                //KetQuaInfo ketqua = new KetQuaInfo();

                var ketqua = new KetQuaDAL().Update_PreState(stateID, xulydonID, CanBoID);
            }

            string data = "";

            #region chuyen giai quyet
            int coquanid = IdentityHelper.CoQuanID ?? 0;
            ChuyenGiaiQuyetInfo cgqinfo = new ChuyenGiaiQuyetInfo();
            cgqinfo.CoQuanGiaiQuyetID = coquanid;
            cgqinfo.CoQuanPhanID = coquanid;
            cgqinfo.NgayChuyen = DateTime.Now;
            cgqinfo.XuLyDonID = docunmentid;
            cgqinfo.GhiChu = txtghichu;
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
            int capThanhTra = Utils.ConvertToInt32(isCapThanhTra, 0);
            int tPID = Utils.ConvertToInt32(truongPhongID, 0);
            new PhanTPPhanGQ().DelByXLDIDAndCanBoID(docunmentid, tPID);

            if (stateName == Constant.LD_CapDuoi_Phan_GiaiQuyet || (capThanhTra == 1 && stateName == Constant.LD_Phan_GiaiQuyet) || stateName == Constant.TP_Phan_GiaiQuyet)
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

            return data;



        }

        public static string InsertCQPhoiHop(string xuLyDonID, string coQuanPhoiHopID)
        {
            int docunmentid = Utils.ConvertToInt32(xuLyDonID, 0);
            #region -- co quan phoi hop info
            CQPhoiHopGQInfo cqPhoiHopGQInfo = new CQPhoiHopGQInfo();
            cqPhoiHopGQInfo.XuLyDonID = docunmentid;
            cqPhoiHopGQInfo.CQPhoiHopID = Utils.ConvertToInt32(coQuanPhoiHopID, 0); ;
            #endregion

            int val = 0;

            try
            {
                val = new ChuyenGiaiQuyet().InsertCQPhoiHopGQ(cqPhoiHopGQInfo);


            }
            catch { }

            string data = "";
            return data;

        }

        public static string DeleteCQPhoiHop(string xuLyDonID)
        {
            int docunmentid = Utils.ConvertToInt32(xuLyDonID, 0);

            int val = 0;

            try
            {
                val = new ChuyenGiaiQuyet().DeleteCQPhoiHop(docunmentid);
            }
            catch { }

            string data = "";
            return data;
        }

        public IList<CoQuanInfo> GetCoQuanGQ(int CoQuanID)
        {
            return new CoQuan().GetListCoQuanGQbyID(CoQuanID);
        }

        public GiaoXacMinhModel GetByID(int xuLyDonID)
        {
            GiaoXacMinhModel giaoXacMinhModel = new GiaoXacMinhModel();
            ChuyenGiaiQuyetInfo data = new ChuyenGiaiQuyet().GetChuyenGiaiQuyetCoQuanKhac(xuLyDonID);
            if (data != null)
            {
                giaoXacMinhModel.XuLyDonID = xuLyDonID;
                giaoXacMinhModel.CoQuanID = data.CoQuanGiaiQuyetID;
                giaoXacMinhModel.GhiChu = data.GhiChu;
                giaoXacMinhModel.SoQuyetDinh = data.SoQuyetDinh;
                giaoXacMinhModel.QuyetDinh = data.QuyetDinh;
                giaoXacMinhModel.NgayQuyetDinh = data.NgayQuyetDinh;
                giaoXacMinhModel.CQPhoiHopGQ = new ChuyenGiaiQuyet().GetCoQuanPhoiHop(xuLyDonID);

            }

            try
            {
                DTXuLyInfo DTXuLyInfo = new DTXuLy().GetByID(xuLyDonID);
                var DonThu = new DonThuDAL().GetByID(DTXuLyInfo.DonThuID, xuLyDonID);
                if (DonThu.HanGiaiQuyetCu != DateTime.MinValue)
                {
                    giaoXacMinhModel.HanGiaiQuyet = DonThu.HanGiaiQuyetCu;
                }
            }
            catch (Exception)
            {
            }

            try
            {
                giaoXacMinhModel.ToXacMinh = new VaiTroGiaiQuyet().GetByXuLyDonID(xuLyDonID);
                var fileXacMinh = new TheoDoiXuLyDAL().GetQuyetDinhGiaoXacMinh(xuLyDonID).ToList();
                if (fileXacMinh.Count > 0)
                {
                    giaoXacMinhModel.DanhSachHoSoTaiLieu = new List<DanhSachHoSoTaiLieu>();
                    giaoXacMinhModel.DanhSachHoSoTaiLieu = fileXacMinh.GroupBy(p => p.GroupUID)
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
                                               FileUrl = y.FirstOrDefault().FileUrl,
                                           }
                                           ).ToList(),

                       }
                       ).ToList();
                }
            }
            catch (Exception)
            {
            }

            return giaoXacMinhModel;
        }


    }
}
