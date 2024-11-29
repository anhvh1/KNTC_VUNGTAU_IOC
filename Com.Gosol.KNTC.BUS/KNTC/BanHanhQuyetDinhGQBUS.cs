using Com.Gosol.KNTC.DAL.HeThong;
using Com.Gosol.KNTC.DAL.KNTC;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.HeThong;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Ultilities;
using DocumentFormat.OpenXml.EMMA;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.BUS.KNTC
{
    public class BanHanhQuyetDinhGQBUS
    {
        public IList<ChuyenXuLyInfo> GetBySearch(IdentityHelper IdentityHelper, XuLyDonParamsForFilter p, ref int TotalRow)
        {
            int cr = p.PageNumber;
            if (cr == 0)
            {
                cr = 1;
            }
            int start = (cr - 1) * p.PageSize;
            int end = cr * p.PageSize;

            QueryFilterInfo filter = new QueryFilterInfo();
            filter.CoQuanID = IdentityHelper.CoQuanID ?? 0;
            filter.KeyWord = "%" + p.Keyword + "%";
            filter.LoaiKhieuToID = p.LoaiKhieuToID ?? 0;
            filter.TuNgay = p.TuNgay ?? DateTime.MinValue;
            filter.DenNgay = p.DenNgay ?? DateTime.MinValue;
            filter.StateName = Constant.Ket_Thuc;
            filter.PrevStateName = Constant.LD_Duyet_GiaiQuyet;
            filter.CanBoID = IdentityHelper.CanBoID ?? 0;
            filter.VaiTro = (int)VaiTroEnum.PhuTrach;
            filter.TrangThai = p.TrangThai;

            IList<ChuyenXuLyInfo> ListInfo = new List<ChuyenXuLyInfo>();

            try
            {
                ListInfo = new ChuyenXuLy().GetDSDonThuDaDuyetKQGiaiQuyet(filter, start, end, ref TotalRow).ToList();
                if (ListInfo.Count > 0)
                {
                    foreach (var item in ListInfo)
                    {
                        var renderTrangThai = new RenderTrangThai();
                        renderTrangThai.GetTrangThai(
                            item.LoaiQuyTrinh,
                            item.HuongGiaiQuyetID,
                            "",
                            item.StateID,
                            0,                            
                            item.TrangThaiDuyet ?? 0,
                            item.TrinhDuThao,
                            IdentityHelper,
                            item.NgayCapNhat,
                            item.ChuyenGiaiQuyetID,
                            int.TryParse(item.KetQuaID, out int result)? result : 0,
                            item.LanhDaoDuyet2ID,
                            item.RutDonID
                            );
                        item.TrangThaiMoi = renderTrangThai.TrangThaiMoi;
                        item.TrangThaiIDMoi = renderTrangThai.TrangThaiIDMoi;
                        item.CheckTrangThai = renderTrangThai.CheckTrangThai;


                        if (item.NguonDonDen == (int)EnumNguonDonDen.BuuChinh)
                        {
                            item.NguonDonDenStr = Constant.NguonDon_BuuChinhs;
                        }
                        else if (item.NguonDonDen == (int)EnumNguonDonDen.CoQuanKhac)
                        {
                            item.NguonDonDenStr = Constant.NguonDon_CoQuanKhacs;
                        }
                        else if (item.NguonDonDen == (int)EnumNguonDonDen.TrucTiep)
                        {
                            item.NguonDonDenStr = Constant.NguonDon_TrucTieps;
                        }

                        //Don thuoc tham quyen giai quyet
                        if (item.CoQuanID == IdentityHelper.CoQuanID)
                        {
                            item.ThuocThamQuyen = "1";
                        }

                        item.HanXuLyStr = item.NgayHetHan != DateTime.MinValue ? Format.FormatDate(item.NgayHetHan) : "";

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
                            item.NhomKN = new NhomKN().GetByID(nhomKNID);
                            if (item.NhomKN != null)
                            {
                                item.NhomKN.DanhSachDoiTuongKN = new DoiTuongKNDAL().GetCustomDataByNhomKNID(nhomKNID).ToList();
                                item.listDoiTuongKN = item.NhomKN.DanhSachDoiTuongKN;
                            }
                        }

                        if (item.KetQuaID != null && item.KetQuaID.Length > 0)
                        {
                            item.TrangThai = 1;
                            item.TenTrangThai = "Đã cập nhật";
                        }
                        else
                        {
                            item.TrangThai = 0;
                            item.TenTrangThai = "Chưa cập nhật";
                        }
                    }

                }
            }
            catch
            {
            }

            return ListInfo;
        }

        public QuyetDinhInfo GetBanHanhData(int XuLyDonID)
        {
            QuyetDinhInfo kq = new QuyetDinhInfo();
            List<FileHoSoInfo> lst_File = new List<FileHoSoInfo>();
            kq = new KetQuaDAL().QuyetDinh_GetBy_XuLyDonID(XuLyDonID);
            return kq;
        }

        public BaseResultModel SaveBanHanh(IdentityHelper IdentityHelper, QuyetDinhInfo data)
        {
            var Result = new BaseResultModel();
            data.CanBoID = IdentityHelper.CanBoID;
            #region file
            //String fileDataStr = data.fileDataStr;
            //if (fileDataStr != string.Empty)
            //{
            //    data.ListFileQuyetDinh = new List<FileHoSoInfo>();
            //    string[] fileParts = fileDataStr.Split(';');
            //    for (int i = 0; i < fileParts.Length; i++)
            //    {
            //        string fileStr = fileParts[i];
            //        string[] dataParts = fileStr.Split(',');
            //        FileHoSoInfo infoFileHoSo = new FileHoSoInfo();
            //        infoFileHoSo.FileURL = dataParts[0];
            //        infoFileHoSo.NgayUp = Utils.ConvertToDateTime(dataParts[1], Constant.DEFAULT_DATE);
            //        infoFileHoSo.TenFile = dataParts[2];
            //        infoFileHoSo.TomTat = dataParts[3];
            //        infoFileHoSo.NguoiUp = IdentityHelper.CanBoID ?? 0;//IdentityHelper.GetUserID();
            //        infoFileHoSo.FileID = Utils.ConvertToInt32(dataParts[6], 0);
            //        FileLogInfo infoFileLog = new FileLogInfo();
            //        infoFileLog.LoaiLog = (int)EnumLoaiLog.Them;
            //        infoFileLog.LoaiFile = (int)EnumLoaiFile.FileBanHanhQD;
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
            //        data.ListFileQuyetDinh.Add(infoFileHoSo);
            //    }
            //}
            #endregion
            KetQuaInfo kq = new KetQuaDAL().GetCustomByXuLyDonID(data.XuLyDonID.Value);
            if (kq != null)
            {
                //Update info
                data.KetQuaID = kq.KetQuaID;
                var val = new KetQuaDAL().QuyetDinh_CapNhat(data);
                Result.Data = val;
                Result.Status = 1;
                Result.Message = "Cập nhật thành công";
                //return Result;
            }
            else
            {
                //Insert info
                var val = new KetQuaDAL().QuyetDinh_ThemMoi(data);
                data.KetQuaID = val;
                Result.Data = val;
                Result.Status = 1;
                Result.Message = "Cập nhật thành công";
                //return Result;
            }
            if (data.KetQuaID > 0 && data.DanhSachHoSoTaiLieu != null && data.DanhSachHoSoTaiLieu.Count > 0)
            {
                foreach (var item in data.DanhSachHoSoTaiLieu)
                {
                    if (item.DanhSachFileDinhKemID != null && item.DanhSachFileDinhKemID.Count > 0)
                    {
                        new FileDinhKemDAL().UpdateFileBanHanhQDGQ(item.DanhSachFileDinhKemID, data.KetQuaID);
                    }
                }
            }

            return Result;
        }

        public IList<CoQuanInfo> GetCoQuanBanHanh(IdentityHelper IdentityHelper)
        {
            var ListAllCQ = new CoQuan().GetAllCoQuan();
            List<CoQuanInfo> cqList = new List<CoQuanInfo>();
            if (IdentityHelper.CapID == CapQuanLy.CapUBNDXa.GetHashCode())
            {
                cqList = ListAllCQ.Where(x => x.CoQuanID == IdentityHelper.CoQuanID.Value).ToList();
            }
            else if (IdentityHelper.CapID == CapQuanLy.CapUBNDHuyen.GetHashCode())
            {
                var cr = ListAllCQ.Where(x => x.CoQuanID == IdentityHelper.CoQuanID.Value).FirstOrDefault();
                var pr = ListAllCQ.Where(x => x.CoQuanID == cr.CoQuanChaID).FirstOrDefault();
                cqList = new CoQuan().GetCoQuanByParentID(cr.CoQuanChaID).ToList();
                cqList.Add(pr);
            }
            else
            {
                var cr = ListAllCQ.Where(x => x.CoQuanID == IdentityHelper.CoQuanID.Value).FirstOrDefault();
                var pr = ListAllCQ.Where(x => x.CoQuanID == cr.CoQuanChaID).FirstOrDefault();
                cqList = ListAllCQ.Where(x => x.CapID == CapQuanLy.CapSoNganh.GetHashCode()).ToList();
                cqList.Add(pr);
            }
            return cqList;
        }
    }
}
