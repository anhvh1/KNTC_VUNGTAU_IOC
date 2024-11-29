using Com.Gosol.KNTC.DAL.KNTC;
using Com.Gosol.KNTC.Model.HeThong;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.HeThong;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Security;
using Com.Gosol.KNTC.Ultilities;
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

namespace Com.Gosol.KNTC.BUS.KNTC
{
    public class PhanXuLyDonBUS
    {
        public IList<DTXuLyInfo> GetBySearch(ref int TotalRow, XuLyDonParamsForFilter p, IdentityHelper IdentityHelper)
        {
            QueryFilterInfo info = new QueryFilterInfo();
            info.CoQuanID = IdentityHelper.CoQuanID.Value; 
            info.KeyWord = p.Keyword;
            int _currentPage = p.PageNumber;
            info.Start = (_currentPage - 1) * p.PageSize;
            info.End = _currentPage * p.PageSize;
            info.LoaiKhieuToID = p.LoaiKhieuToID ?? 0;
            info.TuNgay = p.TuNgay ?? DateTime.MinValue;
            info.DenNgay = p.DenNgay ?? DateTime.MinValue;
            info.PhongBanID = IdentityHelper.PhongBanID ?? 0;
         
            IList<DTXuLyInfo> ListInfo = new List<DTXuLyInfo>();
            string data = "";

            List<int> docIDList = new List<int>();

            #region role lanh dao
            if (IdentityHelper.RoleID == RoleEnum.LanhDao.GetHashCode())
            {
                try
                {
                    ListInfo = new DTXuLy().DTCanPhanXL_LanhDao(ref TotalRow, info, docIDList);
                    #region list ten chu don, trang thai
                    if (ListInfo != null)
                    {
                        foreach (var item in ListInfo)
                        {
                            if(item.HuongGiaiQuyetID > 0)
                            {
                                item.TrangThaiXuLyStr = "Đã xử lý";
                            }
                            else
                            {
                                item.TrangThaiXuLyStr = "Chưa xử lý";
                            }

                            if (item.StateName == "LD phân xử lý")
                            {
                                item.TrangThaiPhanXuLyStr = "Chưa giao";
                                item.TrangThaiID = 0;
                                if (item.NgayXLConLai < 5)
                                {
                                    item.TrangThaiQuaHan = 1;
                                }
                                else
                                {
                                    item.TrangThaiQuaHan = 0;
                                }
                            }
                            else
                            {
                                if (item.NgayXLConLai < 5)
                                {
                                    if (item.StateName == "Chuyên viên xử lý")
                                    {
                                        item.TrangThaiQuaHan = 1;
                                    }
                                    if (item.StateName == "TP phân xử lý")
                                    {
                                        item.TrangThaiQuaHan = 1;
                                    }
                                    if (item.StateName == "TP duyệt xử lý")
                                    {
                                        item.TrangThaiQuaHan = 1;
                                    }
                                }
                                else
                                {
                                    item.TrangThaiQuaHan = 0;
                                }

                                item.TrangThaiPhanXuLyStr = "Đã giao";
                                item.TrangThaiID = 1;
                            }


                            
                        }
                        //for (int i = 0; i < ListInfo.Count(); i++)
                        //{
                        //    int donID = ListInfo[i].DonThuID;
                        //    DonThuInfo donInfo = new DonThuDAL().GetByID(donID);
                        //    int nhomKNID = donInfo.NhomKNID;
                        //    if (nhomKNID > 0)
                        //    {
                        //        donInfo.HoTen = "";
                        //        List<DoiTuongKNInfo> ltInfo = new DoiTuongKN().GetByNhomKNID(nhomKNID).ToList();
                        //        int count = 0;
                        //        foreach (var doituong in ltInfo)
                        //        {
                        //            donInfo.HoTen += doituong.HoTen;
                        //            count++;
                        //            if (count >= ltInfo.Count())
                        //                break;
                        //            else
                        //                donInfo.HoTen += ", ";
                        //        }
                        //        string lblHoTen = "";
                        //        if (ltInfo.Count > 0)
                        //        {
                        //            lblHoTen = donInfo.HoTen;
                        //            ListInfo[i].HoTenStr = lblHoTen;
                        //        }
                        //    }
                        //}
                    }
                    #endregion
                }
                catch
                {
                }
            }
            #endregion

            #region role truong phong
            if (IdentityHelper.RoleID == RoleEnum.LanhDaoPhong.GetHashCode())
            {
                try
                {
                    ListInfo = new DTXuLy().DTCanPhanXL_TruongPhong(ref TotalRow, info, docIDList);
                    #region list ten chu don
                    if (ListInfo != null)
                    {
                        foreach (var item in ListInfo)
                        {
                            if (item.HuongGiaiQuyetID > 0)
                            {
                                item.TrangThaiXuLyStr = "Đã xử lý";
                            }
                            else
                            {
                                item.TrangThaiXuLyStr = "Chưa xử lý";
                            }

                            if (item.StateName == "TP phân xử lý")
                            {
                                item.TrangThaiPhanXuLyStr = "Chưa giao";
                                if (item.NgayXLConLai < 5)
                                {
                                    item.TrangThaiQuaHan = 1;
                                }
                                else
                                {
                                    item.TrangThaiQuaHan = 0;
                                }
                            }
                            else
                            {
                                if (item.NgayXLConLai < 5 && item.StateName == "Chuyên viên xử lý")
                                {
                                    item.TrangThaiQuaHan = 1;
                                }
                                else
                                {
                                    item.TrangThaiQuaHan = 0;
                                }
                                item.TrangThaiPhanXuLyStr = "Đã giao";
                            }
                        }
                       
                        //for (int i = 0; i < ListInfo.Count(); i++)
                        //{
                        //    int donID = ListInfo[i].DonThuID;
                        //    DonThuInfo donInfo = new DonThuDAL().GetByID(donID);
                        //    int nhomKNID = donInfo.NhomKNID;
                        //    if (nhomKNID > 0)
                        //    {
                        //        donInfo.HoTen = "";
                        //        List<DoiTuongKNInfo> ltInfo = new DoiTuongKN().GetByNhomKNID(nhomKNID).ToList();
                        //        int count = 0;
                        //        foreach (var doituong in ltInfo)
                        //        {
                        //            donInfo.HoTen += doituong.HoTen;
                        //            count++;
                        //            if (count >= ltInfo.Count())
                        //                break;
                        //            else
                        //                donInfo.HoTen += ", ";
                        //        }
                        //        string lblHoTen = "";
                        //        if (ltInfo.Count > 0)
                        //        {
                        //            lblHoTen = donInfo.HoTen;
                        //            ListInfo[i].HoTenStr = lblHoTen;
                        //        }
                        //    }
                        //}
                    }
                    #endregion
                }
                catch
                {
                }
            }
            #endregion

            return ListInfo;

        }

        public BaseResultModel SavePhanXuLy(IdentityHelper IdentityHelper, PhanXuLyInfo info)
        {
            var Result = new BaseResultModel();
            int Data = 0;
            if(info.XuLyDonID < 0)
            {
                Result.Message = "Chưa chọn đơn thư";
                Result.Status = 1;
            }
            if (IdentityHelper.RoleID == RoleEnum.LanhDao.GetHashCode())
            {
                Data = LanhDaoPhanXuLy(IdentityHelper, info, "");
            }
            else if (IdentityHelper.RoleID == RoleEnum.LanhDaoPhong.GetHashCode())
            {
                Data = TruongPhongPhanXuLy(IdentityHelper, info, "");
            }
            if (Data > 0)
            {
                Result.Status = 1;
                Result.Message = "Phân xử lý thành công!";
            }
            else
            {
                Result.Status = 0;
                Result.Message = "Phân xử lý thất bại!";
            }
            Result.Data = Data;

            return Result;
        }

        public int LanhDaoPhanXuLy(IdentityHelper IdentityHelper, PhanXuLyInfo info, string fileUrl)
        {
            string commandCode = "";
            bool kq = true;
            int val = 0;
            
            if (info.CanBoID != 0)
            {
                commandCode = WorkflowInstance.Instance.GetAvailabelCommands(info.XuLyDonID)[1];
            }
            else
            {
                commandCode = WorkflowInstance.Instance.GetAvailabelCommands(info.XuLyDonID)[0];
            }

            kq = WorkflowInstance.Instance.ExecuteCommand(info.XuLyDonID, IdentityHelper.CanBoID.Value, commandCode, info.NgayHetHan.Value, info.GhiChu);

            if (kq == true)
            {
                //PhanXuLuInfo info = new PhanXuLuInfo();
                //info.CanBoID = CanBoID;
                //info.GhiChu = ghichu;
                //info.PhongBanID = idphongban;
                //info.XuLyDonID = idxulydon;
                info.NgayPhan = DateTime.Now;
                val = new PhanXuLy().Insert(info);
                new XuLyDonDAL().UpdateCanBoXuLy(info.XuLyDonID, info.CanBoID);
                #region file PhanXuLy
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

                //            FileHoSoInfo infoData = new FileHoSoInfo();
                //            infoData.XuLyDonID = idxulydon;
                //            infoData.FileURL = dataParts[0];
                //            infoData.NgayUp = Utils.ConvertToDateTime(dataParts[1], Constant.DEFAULT_DATE);
                //            infoData.TenFile = dataParts[2];
                //            infoData.TomTat = dataParts[3];
                //            infoData.NguoiUp = IdentityHelper.GetCanBoID();//IdentityHelper.GetUserID();
                //            infoData.FileID = Utils.ConvertToInt32(dataParts[6], 0);
                //            FileLogInfo infoFileLog = new FileLogInfo();
                //            infoFileLog.LoaiLog = (int)EnumLoaiLog.Them;
                //            infoFileLog.LoaiFile = (int)EnumLoaiFile.FilePhanXuLy;
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
                //                int result = new FileHoSo().InsertPhanXuLy(infoData);
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
            }


            return val;

        }

        public int TruongPhongPhanXuLy(IdentityHelper IdentityHelper, PhanXuLyInfo info, string fileUrl)
        {
            //DateTime NgayHetHan = Utils.ConvertToDateTime(ngayhethan, DateTime.Now);
            //int CanBoID = Utils.ConvertToInt32(canboid, 0);
            //int idxulydon = Utils.ConvertToInt32(xulydonid, 0);
            bool kq = true;    
            int val = 0;

            string commandCode = WorkflowInstance.Instance.GetAvailabelCommands(info.XuLyDonID)[0];
            kq = WorkflowInstance.Instance.ExecuteCommand(info.XuLyDonID, IdentityHelper.CanBoID.Value, commandCode, info.NgayHetHan.Value, info.GhiChu);

            if (kq == true)
            {
                //PhanXuLuInfo info = new PhanXuLuInfo();
                //info.CanBoID = CanBoID;
                //info.GhiChu = ghichu;
                info.PhongBanID = IdentityHelper.PhongBanID ?? 0;
                //info.XuLyDonID = idxulydon;
                info.NgayPhan = DateTime.Now;

                val = new PhanXuLy().Insert(info);
                new XuLyDonDAL().UpdateCanBoXuLy(info.XuLyDonID, info.CanBoID);

                #region file
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

                //            FileHoSoInfo infoData = new FileHoSoInfo();
                //            infoData.XuLyDonID = idxulydon;
                //            infoData.FileURL = dataParts[0];
                //            infoData.NgayUp = Utils.ConvertToDateTime(dataParts[1], Constant.DEFAULT_DATE);
                //            infoData.TenFile = dataParts[2];
                //            infoData.TomTat = dataParts[3];
                //            infoData.NguoiUp = IdentityHelper.GetCanBoID();//IdentityHelper.GetUserID();
                //            infoData.FileID = Utils.ConvertToInt32(dataParts[6], 0);
                //            FileLogInfo infoFileLog = new FileLogInfo();
                //            infoFileLog.LoaiLog = (int)EnumLoaiLog.Them;
                //            infoFileLog.LoaiFile = (int)EnumLoaiFile.FilePhanXuLy;
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
                //                int result = new FileHoSo().InsertPhanXuLy(infoData);
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


            }


            return val;
        }

        public PhanXuLyInfo GetByID(int PhanXuLyID)
        {
            PhanXuLyInfo PhanXuLyInfo = new PhanXuLy().GetByID(PhanXuLyID);
            List<FileHoSoInfo> fileDinhKem = new List<FileHoSoInfo>();
            //CanBoInfo canBoInfo = new CanBo().GetCanBoByID(TiepDanInfo.CanBoTiepID ?? 0);
            //if (canBoInfo.XemTaiLieuMat)
            //{
            //    fileDinhKem = new FileHoSoDAL().GetByXuLyDonID_TrungDon(TiepDanInfo.XuLyDonID ?? 0).ToList();

            //}
            //else
            //{
            //    fileDinhKem = new FileHoSoDAL().GetByXuLyDonID_TrungDon(TiepDanInfo.XuLyDonID ?? 0).Where(x => x.IsBaoMat != true || x.CanBoID == canBoInfo.CanBoID).ToList();
            //}
            if (fileDinhKem.Count > 0)
            {
                PhanXuLyInfo.DanhSachHoSoTaiLieu = new List<DanhSachHoSoTaiLieu>();
                PhanXuLyInfo.DanhSachHoSoTaiLieu = fileDinhKem.GroupBy(p => p.GroupUID)
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
            return PhanXuLyInfo;
        }

        public IList<CoQuanInfo> GetCoQuanPhanXuLy(IdentityHelper IdentityHelper)
        {
            var ListAllCQ = new CoQuan().GetAllCoQuan();
            List<CoQuanInfo> cqList = new List<CoQuanInfo>();
            if (IdentityHelper.CapID == CapQuanLy.CapUBNDXa.GetHashCode())
            {
                cqList = ListAllCQ.Where(x => x.CoQuanID == IdentityHelper.CoQuanID.Value).ToList();
            }
            else if (IdentityHelper.CapID == CapQuanLy.CapUBNDHuyen.GetHashCode())
            {
                //var cr = ListAllCQ.Where(x => x.CoQuanID == IdentityHelper.CoQuanID.Value).FirstOrDefault();
                //var pr = ListAllCQ.Where(x => x.CoQuanID == cr.CoQuanChaID).FirstOrDefault();
                cqList = new CoQuan().GetCoQuanByParentID(IdentityHelper.CoQuanID ?? 0).Where(x => x.CapID == CapQuanLy.CapPhong.GetHashCode()).ToList();
                //cqList.Add(pr);
            }
            else
            {
                var cr = ListAllCQ.Where(x => x.CoQuanID == IdentityHelper.CoQuanID.Value).FirstOrDefault();
                var pr = ListAllCQ.Where(x => x.CoQuanID == cr.CoQuanChaID).FirstOrDefault();
                cqList = ListAllCQ.Where(x => x.CapID == CapQuanLy.CapSoNganh.GetHashCode()).ToList();
                //cqList.Add(pr);
            }
            return cqList;
        }

        public BaseResultModel PhanChoCoQuanKhac(IdentityHelper IdentityHelper, ChuyenDonModel ChuyenDonModel)
        {
            var Result = new BaseResultModel();
            var val = new XuLyDonDAL().ChuTichGiaoXuLy_UpdateXuLyDon(ChuyenDonModel.XuLyDonID ?? 0, ChuyenDonModel.CoQuanID ?? 0, ChuyenDonModel.HanXuLy, IdentityHelper.CoQuanID ?? 0);

            #region old
            //int docunmentid = ChuyenDonModel.XuLyDonID ?? 0;
            //int coQuanID = ChuyenDonModel.CoQuanID ?? 0;
            //string commandCode = "";
            //int canboid = IdentityHelper.CanBoID ?? 0;
            //bool kq = true;
            //DateTime dueDate = ChuyenDonModel.NgayChuyen ?? DateTime.Now;
            //var a = new XuLyDonDAL().UpdateNgayChuyen(docunmentid, dueDate);
            ////insert file y kien xl
            //#region upload file y kien xl
            //////String fileDataStr = hdfFileXLData.Value;
            ////if (fileUrl != string.Empty)
            ////{
            ////    try
            ////    {
            ////        String fileDataStr = fileUrl;
            ////        if (fileDataStr != string.Empty)
            ////        {
            ////            string[] fileParts = fileDataStr.Split(';');
            ////            for (int i = 0; i < fileParts.Length; i++)
            ////            {
            ////                string fileStr = fileParts[i];
            ////                string[] dataParts = fileStr.Split(',');

            ////                FileHoSoInfo fileInfo = new FileHoSoInfo();
            ////                fileInfo.XuLyDonID = docunmentid;
            ////                fileInfo.FileURL = dataParts[0];
            ////                fileInfo.NgayUp = Utils.ConvertToDateTime(dataParts[1], Constant.DEFAULT_DATE);
            ////                fileInfo.TenFile = dataParts[2];
            ////                fileInfo.TomTat = dataParts[3];
            ////                fileInfo.NguoiUp = IdentityHelper.GetCanBoID();
            ////                fileInfo.FileID = Utils.ConvertToInt32(dataParts[6], 0);
            ////                FileLogInfo infoFileLog = new FileLogInfo();
            ////                infoFileLog.LoaiLog = (int)EnumLoaiLog.Them;
            ////                infoFileLog.LoaiFile = (int)EnumLoaiFile.FileVBDonDoc;
            ////                int isBaoMat = Utils.ConvertToInt32(dataParts[4], 0);
            ////                if (isBaoMat == (int)EnumDoBaoMat.BaoMat)
            ////                {
            ////                    infoFileLog.IsBaoMat = true;
            ////                    infoFileLog.IsMaHoa = true;
            ////                }
            ////                else
            ////                {
            ////                    infoFileLog.IsBaoMat = false;
            ////                    infoFileLog.IsMaHoa = false;
            ////                }

            ////                try
            ////                {
            ////                    int result = new DAL.FileHoSo().InsertFileYKienXL(fileInfo);
            ////                    if (result > 0)
            ////                    {
            ////                        infoFileLog.FileID = result;
            ////                        new FileLog().Insert(infoFileLog);
            ////                    }

            ////                }
            ////                catch
            ////                {
            ////                }
            ////            }
            ////        }
            ////    }
            ////    catch
            ////    {
            ////    }
            ////}

            //#endregion
            //string stateName = WorkflowInstance.Instance.GetCurrentStateOfDocument(docunmentid);
            //if (stateName == Constant.CHUYENDON_RAVBDONDOC)
            //{
            //    commandCode = WorkflowInstance.Instance.GetAvailabelCommands(docunmentid).Where(x => x.ToString() == "KetThuc").FirstOrDefault();

            //}

            //int val = 0;

            //TiepDanInfo info = new TiepDanInfo();
            //try
            //{
            //    info = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().GetXuLyDonByXLDID(docunmentid);
            //}
            //catch
            //{ }
            //int chuyenDon = (int)HuongGiaiQuyetEnum.ChuyenDon;
            //int vBDonDoc = (int)HuongGiaiQuyetEnum.RaVanBanDonDoc;
            //int cVChiDao = (int)HuongGiaiQuyetEnum.CongVanChiDao;
            //bool suDungQTVanThuTiepDan = IdentityHelper.SuDungQTVanThuTiepDan ?? false;
            //if (info.HuongGiaiQuyetID == chuyenDon)
            //{
            //    #region chuyen don


            //    TiepDanInfo xldInfo = null;
            //    xldInfo = info;

            //    if (IdentityHelper.SuDungQuyTrinhPhucTap == true)
            //    {
            //        #region chuyen don
            //        int cqNhapHoID = 0;

            //        ChuyenXuLyInfo cxlInfo = new ChuyenXuLyInfo();
            //        cxlInfo.XuLyDonID = info.XuLyDonID ?? 0;
            //        if (cqNhapHoID != 0)
            //        {
            //            cxlInfo.CQGuiID = cqNhapHoID;
            //        }
            //        else
            //        {
            //            cxlInfo.CQGuiID = IdentityHelper.CoQuanID ?? 0;
            //        }
            //        cxlInfo.CQNhanID = info.CQChuyenDonID ?? 0;
            //        cxlInfo.NgayChuyen = DateTime.Now;

            //        if (info.XuLyDonID != 0)
            //            new XuLyDonDAL().UpdateCQNhanDonChuyen(docunmentid, coQuanID);


            //        #region clone chuyen don
            //        int donThuIDNew = 0;
            //        try
            //        {
            //            #region clone don thu
            //            DonThuInfo donThuInfo = new DonThuInfo();
            //            donThuInfo = new DonThuDAL().GetByID(info.DonThuID ?? 0);
            //            if (donThuInfo != null)
            //            {
            //                TiepDanInfo tiepDanInfo = new TiepDanInfo();
            //                tiepDanInfo.NhomKNID = donThuInfo.NhomKNID;
            //                tiepDanInfo.DoiTuongBiKNID = donThuInfo.DoiTuongBiKNID;
            //                tiepDanInfo.LoaiKhieuTo1ID = donThuInfo.LoaiKhieuTo1ID;
            //                tiepDanInfo.LoaiKhieuTo2ID = donThuInfo.LoaiKhieuTo2ID;
            //                tiepDanInfo.LoaiKhieuTo3ID = donThuInfo.LoaiKhieuTo3ID;
            //                tiepDanInfo.LoaiKhieuToID = donThuInfo.LoaiKhieuToID;
            //                tiepDanInfo.NoiDungDon = donThuInfo.NoiDungDon;
            //                tiepDanInfo.TrungDon = false;
            //                tiepDanInfo.TinhID = donThuInfo.TinhID;
            //                tiepDanInfo.HuyenID = donThuInfo.HuyenID;
            //                tiepDanInfo.XaID = donThuInfo.XaID;
            //                tiepDanInfo.DiaChiPhatSinh = donThuInfo.DiaChiPhatSinh;
            //                tiepDanInfo.LeTanChuyen = false;
            //                tiepDanInfo.NgayVietDon = donThuInfo.NgayVietDon;
            //                try
            //                {
            //                    donThuIDNew = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().InsertDonThu(tiepDanInfo);
            //                }
            //                catch { }
            //                //new DonThu().Insert(donThuInfo);
            //            }
            //        }
            //        catch { }
            //        #endregion

            //        int xuLyDonCloneID = 0;

            //        TiepDanInfo xldCloneInfo = info;
            //        xldCloneInfo.DonThuID = donThuIDNew;
            //        xldCloneInfo.CanBoTiepNhapID = 0;
            //        xldCloneInfo.SoDonThu = GetSoDonThu(coQuanID, IdentityHelper);
            //        xldCloneInfo.CoQuanID = coQuanID;
            //        xldCloneInfo.NguonDonDen = (int)EnumNguonDonDen.CoQuanKhac;
            //        xldCloneInfo.CQChuyenDonID = 0;
            //        xldCloneInfo.CQChuyenDonDenID = IdentityHelper.CoQuanID ?? 0;

            //        xldCloneInfo.HuongGiaiQuyetID = 0;
            //        xldCloneInfo.CanBoXuLyID = 0;
            //        xldCloneInfo.NgayChuyenDon = DateTime.Now;
            //        xldCloneInfo.DaDuyetXuLy = false;
            //        xldCloneInfo.XuLyDonIDGoc = ChuyenDonModel.XuLyDonID ?? 0;

            //        try
            //        {
            //            xuLyDonCloneID = new Com.Gosol.KNTC.DAL.KNTC.TiepDan().InsertXuLyDon(xldCloneInfo);

            //        }
            //        catch
            //        {
            //        }

            //        #region Clone file dinh kem
            //        //List<FileHoSoInfo> fileHoSoList = new FileHoSo().GetByXuLyDonID(info.XuLyDonID).ToList();
            //        //foreach (var fileInfo in fileHoSoList)
            //        //{
            //        //    fileInfo.XuLyDonID = xuLyDonCloneID;
            //        //    fileInfo.DonThuID = donThuIDNew;
            //        //    FileLogInfo infoFileLog = new FileLogInfo();
            //        //    infoFileLog.LoaiLog = (int)EnumLoaiLog.Them;
            //        //    infoFileLog.LoaiFile = fileInfo.LoaiFile;
            //        //    infoFileLog.IsBaoMat = fileInfo.IsBaoMat;
            //        //    infoFileLog.IsMaHoa = fileInfo.IsMaHoa;

            //        //    try
            //        //    {
            //        //        int resultInsert = new FileHoSo().Insert(fileInfo);
            //        //        if (resultInsert > 0)
            //        //        {
            //        //            infoFileLog.FileID = resultInsert;
            //        //            new FileLog().Insert(infoFileLog);
            //        //        }
            //        //    }
            //        //    catch
            //        //    {
            //        //    }
            //        //}
            //        #endregion

            //        #region clone file y kien xu ly
            //        //List<XuLyDonInfo> lsFileYKienXL = new XuLyDon().GetFileYKienXuLy(info.XuLyDonID).ToList();
            //        //FileHoSoInfo fileYKienXLInfo = new FileHoSoInfo();
            //        //foreach (var files in lsFileYKienXL)
            //        //{
            //        //    fileYKienXLInfo.TenFile = files.TenFileYKienXL;
            //        //    fileYKienXLInfo.TomTat = files.TomTat;
            //        //    fileYKienXLInfo.NgayUp = files.NgayUp;
            //        //    fileYKienXLInfo.NguoiUp = files.NguoiUp;
            //        //    fileYKienXLInfo.FileURL = files.FileURL;
            //        //    fileYKienXLInfo.XuLyDonID = xuLyDonCloneID;

            //        //    FileLogInfo infoFileLog = new FileLogInfo();
            //        //    infoFileLog.LoaiLog = (int)EnumLoaiLog.Them;
            //        //    infoFileLog.LoaiFile = files.LoaiFile;
            //        //    infoFileLog.IsBaoMat = files.IsBaoMat;
            //        //    infoFileLog.IsMaHoa = files.IsMaHoa;
            //        //    try
            //        //    {
            //        //        int resultInsert = new FileHoSo().InsertFileYKienXL(fileYKienXLInfo);
            //        //        if (resultInsert > 0)
            //        //        {
            //        //            infoFileLog.FileID = resultInsert;
            //        //            new FileLog().Insert(infoFileLog);
            //        //        }
            //        //    }
            //        //    catch
            //        //    {
            //        //    }
            //        //}
            //        #endregion

            //        WorkflowInstance.Instance.AttachDocument(xuLyDonCloneID, "XuLyDon", IdentityHelper.UserID ?? 0, null);
            //        #endregion

            //        try
            //        {
            //            new ChuyenXuLy().Insert(cxlInfo);
            //        }
            //        catch
            //        {
            //        }
            //        #endregion
            //    }

            //    DateTime NgayChuyen = DateTime.Now;
            //    #endregion
            //}
            //else if (info.HuongGiaiQuyetID == vBDonDoc || info.HuongGiaiQuyetID == cVChiDao)
            //{

            //    new XuLyDonDAL().UpdateCQNhanVBDonDoc(docunmentid, coQuanID);
            //}

            //kq = WorkflowInstance.Instance.ExecuteCommand(docunmentid, canboid, commandCode, dueDate, "");

            //if (kq)
            //    val = 1;
            #endregion

            Result.Data = val;
            Result.Status = 1;
            Result.Message = "Giao xử lý thành công";
            return Result;

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

    }
}
