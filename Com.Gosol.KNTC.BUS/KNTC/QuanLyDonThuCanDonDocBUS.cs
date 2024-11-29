using Com.Gosol.KNTC.DAL.KNTC;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using Com.Gosol.KNTC.Models.HeThong;
using Com.Gosol.KNTC.Ultilities;
using Com.Gosol.KNTC.DAL.HeThong;

namespace Com.Gosol.KNTC.BUS.KNTC
{
    public class QuanLyDonThuCanDonDocBUS
    {
        public List<DonThuDonDocInfo> GetBySearch(ref int TotalRow, BasePagingParamsForFilter p, IdentityHelper IdentityHelper)
        { 
            int currentPage = p.PageNumber;
            if (currentPage == 0)
            {
                currentPage = 1;
            }
            int start = (currentPage - 1) * p.PageSize;
            int end = currentPage * p.PageSize;
            List<DonThuDonDocInfo> lIST = new DonDocDAL().GetDanhSachDonThuCanDonDoc(p.TuNgay, p.DenNgay, p.CoQuanID, p.HuongXuLyID, p.LoaiKhieuToID, p.TrangThaiXuLyID, p.Keyword ?? "", IdentityHelper.CoQuanID, start, end);
            TotalRow = new DonDocDAL().CountDanhSachDonThuCanDonDoc(p.TuNgay, p.DenNgay, p.CoQuanID, p.HuongXuLyID, p.LoaiKhieuToID, p.TrangThaiXuLyID, p.Keyword ?? "", IdentityHelper.CoQuanID, 0, 1050);

            return lIST;
        }

        public BaseResultModel RaVanBanDonDoc(KetQuaInfo info)
        {
            var Result = new BaseResultModel();

            string file_upload = string.Empty;
            //KetQuaInfo info = new KetQuaInfo();
            //info.NgayDonDoc = Utils.ConvertToDateTime(txtNgayDonDoc.Text, DateTime.MinValue);
            //info.NoiDungDonDoc = Utils.ConvertToString(txtNDDonDoc.InnerText, string.Empty);
            //info.XuLyDonID = xldId;
            if (string.IsNullOrEmpty(info.NoiDungDonDoc) || string.IsNullOrEmpty(info.NgayDonDoc.ToString()))
            {
                Result.Status = 0;
                Result.Message = "Thông tin không được để trống";
                return Result;
            }

            int DonDocID = 0;
            int result = new KetQuaDAL().Insert_RaVanBanDonDoc(info, ref DonDocID);
            if(result > 0)
            {
                if(DonDocID > 0)
                {
                    //file dinh kem
                    if (info.DanhSachHoSoTaiLieu != null && info.DanhSachHoSoTaiLieu.Count > 0)
                    {
                        foreach (var item in info.DanhSachHoSoTaiLieu)
                        {
                            if (item.DanhSachFileDinhKemID != null && item.DanhSachFileDinhKemID.Count > 0)
                            {
                                new FileDinhKemDAL().UpdateFileVBDonDoc(item.DanhSachFileDinhKemID, DonDocID);
                            }
                        }

                    }
                }

                Result.Status = 1;
                Result.Message = Constant.CONTENT_MESSAGE_SUCCESS;
                Result.Data = DonDocID;
            }
            else
            {
                Result.Status = 0;
                Result.Message = Constant.CONTENT_MESSAGE_ERROR;
            }

            return Result;
        }
    }
}
