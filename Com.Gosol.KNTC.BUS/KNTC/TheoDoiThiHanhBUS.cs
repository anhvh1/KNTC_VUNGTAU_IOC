using Com.Gosol.KNTC.DAL.HeThong;
using Com.Gosol.KNTC.DAL.KNTC;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.HeThong;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Models.TiepDan;
using Com.Gosol.KNTC.Ultilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.BUS.KNTC
{
    public class TheoDoiThiHanhBUS
    {
        public List<DonThuThiHanhInfo> GetBySearch(IdentityHelper IdentityHelper, XuLyDonParamsForFilter p, ref int TotalRow)
        {
            QueryFilterInfo query = new QueryFilterInfo();
            query.KeyWord = p.Keyword;
            query.TuNgay = p.TuNgay ?? DateTime.MinValue;
            query.DenNgay = p.DenNgay ?? DateTime.MinValue;
            query.LoaiKhieuToID = p.LoaiKhieuToID ?? 0;
            query.TrangThai = p.TrangThai;
            query.CanBoID = IdentityHelper.CanBoID ?? 0;
            query.VaiTro = (int)VaiTroEnum.PhuTrach;
            query.CoQuanID = IdentityHelper.CoQuanID ?? 0;

            int cr = p.PageNumber;
            if (cr == 0)
            {
                cr = 1;
            }
            int start = (cr - 1) * p.PageSize;
            int end = cr * p.PageSize;
            query.Start = start;
            query.End = end;
         
            List<DonThuThiHanhInfo> dsDonThu = new KetQuaDAL().QuyetDinh_Get_DanhSachDonThuCanThiHanh(query, ref TotalRow).ToList();
            if(dsDonThu != null)
            {
                foreach (var item in dsDonThu)
                {
                    if(item.NhomKNID > 0)
                    {
                        item.NhomKN = new NhomKN().GetByID(item.NhomKNID.Value);
                        if (item.NhomKN != null)
                        {
                            item.NhomKN.DanhSachDoiTuongKN = new DoiTuongKNDAL().GetCustomDataByNhomKNID(item.NhomKNID.Value).ToList();
                        }
                    }
                }
            }
            var result = new DanhSachDonThuDaDuyetKQInfo();
            result.DanhSachDonThu = dsDonThu;
            result.TotalRow = TotalRow;

            return dsDonThu;
        }

        public DonThuThiHanhInfo GetChiTietThihanh(IdentityHelper IdentityHelper, int XuLyDonID)
        {
            //var result = new KetQuaThiHanhInfo();
            var result = new KetQuaDAL().GetThiHanh_By_XuLyDonID(XuLyDonID);
            int CoQuanID = IdentityHelper.CoQuanID ?? 0;
            //result = new KetQuaDAL().QuyetDinh_GetChiTietThiHanh_By_XuLyDonID(XuLyDonID, CoQuanID);
            if (result != null)
            {
                result.ThongTinQuyetDinhGQ = new KetQuaDAL().QuyetDinh_GetBy_XuLyDonID(XuLyDonID);
                if (result.ThongTinQuyetDinhGQ != null)
                {
                    result.LoaiKhieuTo1ID = result.ThongTinQuyetDinhGQ.LoaiKhieuToID ?? 0;
                }

                if(result.ThiHanhID > 0)
                {
                    var fileDinhKem = new ThiHanhDAL().GetFileThiHanhByThiHanhID(result.ThiHanhID ?? 0, EnumLoaiFile.FileThiHanh.GetHashCode());
                    if (fileDinhKem.Count > 0)
                    {
                        result.DanhSachHoSoTaiLieu = new List<DanhSachHoSoTaiLieu>();
                        result.DanhSachHoSoTaiLieu = fileDinhKem.GroupBy(p => p.GroupUID)
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
                                                   FileUrl = y.FirstOrDefault().FileURL,
                                               }
                                               ).ToList(),

                           }
                           ).ToList();
                    }
                }
            }
            return result;
        }

        public BaseResultModel SaveThiHanh(IdentityHelper IdentityHelper, NoiDungThiHanhInfo info)
        {
            var Result = new BaseResultModel();
            info.ListFileThiHanh = new List<FileHoSoInfo>();
            #region file
            //string[] fileParts = info.ListFileString.Split(';');
            //for (int i = 0; i < fileParts.Length; i++)
            //{
            //    string fileStr = fileParts[i];
            //    if (fileStr == "")
            //    {
            //        break;
            //    }
            //    string[] dataParts = fileStr.Split(',');

            //    FileHoSoInfo infoFileHoSo = new FileHoSoInfo();
            //    infoFileHoSo.FileURL = dataParts[0];
            //    infoFileHoSo.NgayUp = Utils.ConvertToDateTime(dataParts[1], Constant.DEFAULT_DATE);
            //    infoFileHoSo.TenFile = dataParts[2];
            //    infoFileHoSo.TomTat = dataParts[3];
            //    if (dataParts[5] == "")
            //    {
            //        infoFileHoSo.NguoiUp = IdentityHelper.CanBoID ?? 0;//IdentityHelper.GetUserID();
            //    }
            //    else
            //    {
            //        infoFileHoSo.NguoiUp = Utils.ConvertToInt32(dataParts[5], 0);

            //    }
            //    infoFileHoSo.FileID = Utils.ConvertToInt32(dataParts[6], 0);
            //    int isBaoMat = Utils.ConvertToInt32(dataParts[4], 0);
            //    if (isBaoMat == (int)EnumDoBaoMat.BaoMat)
            //    {
            //        infoFileHoSo.IsBaoMat = true;
            //        infoFileHoSo.IsMaHoa = true;
            //    }
            //    else
            //    {
            //        infoFileHoSo.IsBaoMat = false;
            //        infoFileHoSo.IsMaHoa = false;
            //    }
            //    info.ListFileThiHanh.Add(infoFileHoSo);
            //}
            #endregion
            int ThiHanhID = 0;
            var ThiHanhInfo = new KetQuaDAL().GetThiHanh_By_XuLyDonID(info.XuLyDonID ?? 0);
            if (info.ThiHanhID > 0 || ThiHanhInfo.ThiHanhID > 0)
            {
                //Update
                if (info.ThiHanhID == null || info.ThiHanhID == 0) info.ThiHanhID = ThiHanhInfo.ThiHanhID;
                int val = new KetQuaDAL().QuyetDinh_CapNhat_ThiHanh(info);
                Result.Data = val;
                Result.Status = 1;
                Result.Message = "Lưu thành công";
                ThiHanhID = info.ThiHanhID ?? 0;
            }
            else
            {
                int val = new KetQuaDAL().QuyetDinh_ThemMoi_ThiHanh(info);
                Result.Data = val;
                Result.Status = 1;
                Result.Message = "Lưu thành công";
                ThiHanhID = val;
            }

            if(info.DanhSachHoSoTaiLieu != null && info.DanhSachHoSoTaiLieu.Count > 0)
            {
                foreach (var item in info.DanhSachHoSoTaiLieu)
                {
                    if (item.DanhSachFileDinhKemID != null && item.DanhSachFileDinhKemID.Count > 0)
                    {
                        new FileDinhKemDAL().UpdateFileThiHanh(item.DanhSachFileDinhKemID, ThiHanhID);
                    }
                }
            }
            return Result;
        }
    }
}
