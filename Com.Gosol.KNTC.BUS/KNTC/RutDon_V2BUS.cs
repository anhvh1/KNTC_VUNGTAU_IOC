using Com.Gosol.KNTC.DAL.HeThong;
using Com.Gosol.KNTC.DAL.KNTC;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.HeThong;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Ultilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.BUS.KNTC
{
    public class RutDon_V2BUS
    {
        private RutDon_V2DAL _rutDonV2;
        public RutDon_V2BUS()
        {
            _rutDonV2 = new RutDon_V2DAL();
        }
        public BaseResultModel Insert(RutDon_V2Model rutDon, int canBoID)
        {
            var result = new BaseResultModel();
            try
            {
                //if (rutDon.XuLyDonID <= 0)
                //{
                //    result.Status = 0;
                //    result.Message = "Xử lý đơn id không được để trống";
                //    return result;
                //}
                if (string.IsNullOrEmpty(rutDon.LyDoRutDon))
                {
                    result.Status = 0;
                    result.Message = "lý do rút đơn không được để trống";
                    return result;
                }
                else
                {
                    result = _rutDonV2.Insert(rutDon, canBoID);
                }
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                throw;
            }
            return result;
        }
        public ChiTietRutDon GetByXuLyDonID(int xuLyDonID)
        {
            ChiTietRutDon chiTietRutDon = _rutDonV2.GetByXuLyDonID(xuLyDonID);

            // Sử dụng LINQ để nhóm các phần tử trong DanhSachHoSoTaiLieu theo TenFile
            var groupedFiles = chiTietRutDon.DanhSachHoSoTaiLieu
                .GroupBy(d => d.TenFile)
                .Select(g => new DanhSachHoSoTaiLieu
                {
                    TenFile = g.Key,
                    GroupUID = g.First().GroupUID,  // Lấy GroupUID từ phần tử đầu tiên trong nhóm
                    HoSoTaiLieuID = g.First().HoSoTaiLieuID,  // Lấy HoSoTaiLieuID từ phần tử đầu tiên trong nhóm
                    NguoiCapNhatID = g.First().NguoiCapNhatID,  // Lấy NguoiCapNhatID từ phần tử đầu tiên trong nhóm
                    FileType = g.First().FileType,  // Lấy FileType từ phần tử đầu tiên trong nhóm
                    TenNguoiCapNhat = g.First().TenNguoiCapNhat,  // Lấy TenNguoiCapNhat từ phần tử đầu tiên trong nhóm
                    NoiDung = g.First().NoiDung,  // Lấy NoiDung từ phần tử đầu tiên trong nhóm
                    NgayCapNhat = g.First().NgayCapNhat,  // Lấy NgayCapNhat từ phần tử đầu tiên trong nhóm
                    DanhSachFileDinhKemID = g.Where(x => x.DanhSachFileDinhKemID != null)
                                             .SelectMany(x => x.DanhSachFileDinhKemID)
                                             .ToList(),
                    FileDinhKem = g.Where(x => x.FileDinhKem != null)
                                   .SelectMany(x => x.FileDinhKem)
                                   .ToList(),  // Gộp các FileDinhKem lại với nhau
                    FileDinhKemDelete = g.Where(x => x.FileDinhKemDelete != null)
                                         .SelectMany(x => x.FileDinhKemDelete)
                                         .ToList()  // Gộp các FileDinhKemDelete lại với nhau
                })
                .ToList();

            // Tạo một đối tượng ChiTietRutDon mới với danh sách đã được nhóm
            var groupedChiTietRutDon = new ChiTietRutDon
            {
                RutDonID = chiTietRutDon.RutDonID,
                XuLyDonID = chiTietRutDon.XuLyDonID,
                LyDoRutDon = chiTietRutDon.LyDoRutDon,
                CanBoID = chiTietRutDon.CanBoID,
                TenCanBo = chiTietRutDon.TenCanBo,
                NgayCapNhap = chiTietRutDon.NgayCapNhap,
                DanhSachHoSoTaiLieu = groupedFiles
            };

            return groupedChiTietRutDon;
        }


    }
}
