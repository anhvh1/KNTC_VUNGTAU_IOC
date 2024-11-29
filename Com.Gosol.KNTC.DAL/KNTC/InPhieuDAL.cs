using Com.Gosol.KNTC.DAL.HeThong;
using Com.Gosol.KNTC.Model.HeThong;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Ultilities;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.AspNetCore.Http;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.DAL.KNTC
{
    public class InPhieuDAL
    {
        public void FillTiepDanData(string document, TiepDanInfo TiepDanInfo)
        {
            if (TiepDanInfo != null && TiepDanInfo.TiepDanKhongDonID == 0 && TiepDanInfo.NgayTiep != null)
            {
                Replace(document, "DAY", TiepDanInfo.NgayTiep?.Day.ToString());
                Replace(document,"MONTH", TiepDanInfo.NgayTiep?.Month.ToString());
                Replace(document,"YEAR", TiepDanInfo.NgayTiep?.Year.ToString());
                Replace(document,"HOUR", TiepDanInfo.NgayTiep?.Hour.ToString());
                Replace(document, "MINUTE", TiepDanInfo.NgayTiep?.Minute.ToString());
            }
            else
            {
                Replace(document,"DAY", DateTime.Now.Day.ToString());
                Replace(document,"MONTH", DateTime.Now.Month.ToString());
                Replace(document,"YEAR", DateTime.Now.Year.ToString());
                Replace(document,"HOUR", DateTime.Now.Hour.ToString());
                Replace(document, "MINUTE", DateTime.Now.Minute.ToString());
            }

            var canBoInfo = new CanBo().GetCanBoByID(TiepDanInfo.CanBoTiepID ?? 0);
            var coQuanInfo = new CoQuan().GetCoQuanByID(TiepDanInfo.CoQuanID ?? 0);

            if (TiepDanInfo != null)
            {
                if (TiepDanInfo.SoDonThu != null)
                {
                    Replace(document, "SO_DON_THU", TiepDanInfo.SoDonThu);
                }
                else Replace(document, "SO_DON_THU", "...");

                List<ThanhPhanThamGiaInfo> thanhPhanThams = new TiepDan().GetThanhPhanThamGia(TiepDanInfo.TiepDanKhongDonID ?? 0).ToList();
                string tencb = "";
                if (thanhPhanThams.Count > 0)
                {
                    for (int i = 0; i < thanhPhanThams.Count; i++)
                    {
                        thanhPhanThams[i].TenCanBo = "- Ông (bà) " + thanhPhanThams[i].TenCanBo;
                        int tabnum = 6;
                        int length = thanhPhanThams[i].TenCanBo.Length / 6;
                        tabnum = tabnum - length;
                        string tabstr = "";
                        for (int j = 0; j < tabnum; j++)
                        {
                            tabstr += "\t";
                        }

                        if (i != thanhPhanThams.Count - 1)
                        {
                            tencb += thanhPhanThams[i].TenCanBo + tabstr + "chức vụ:  " + thanhPhanThams[i].ChucVu + "\n";
                        }
                        else tencb += thanhPhanThams[i].TenCanBo + tabstr + "chức vụ:  " + thanhPhanThams[i].ChucVu;
                    }

                }
                else if (canBoInfo != null)
                {
                    tencb = "- Ông (bà) " + canBoInfo.TenCanBo + "\t\t\t" + "chức vụ:  " + canBoInfo.TenChucVu;
                }

                if (tencb != "")
                {
                    Replace(document, "THANH_PHAN_TIEP_DAN", tencb);
                }
                else Replace(document, "THANH_PHAN_TIEP_DAN", "...");
                //Lãnh đạo tiếp
                var listThanhTraTinh = new SystemConfigDAL().GetByKey("Thanh_Tra_Tinh_ID").ConfigValue.Split(',').ToList().Select(x => Utils.ConvertToInt32(x.ToString(), 0)).ToList();
                if (TiepDanInfo.TenLanhDaoTiep != null && TiepDanInfo.TenLanhDaoTiep != "")
                {
                    string tenLanhDao = "- Ông (bà) " + TiepDanInfo.TenLanhDaoTiep;
                    int tabnum = 6;
                    int length = tenLanhDao.Length / 6;
                    tabnum = tabnum - length;
                    string tabstr = "";
                    for (int j = 0; j < tabnum; j++)
                    {
                        tabstr += "\t";
                    }

                    if (TiepDanInfo.ChucVu != null && TiepDanInfo.ChucVu.Length > 0)
                    {
                        Replace(document, "LANH_DAO", "\n" + tenLanhDao + tabstr + "chức vụ:  " + TiepDanInfo.ChucVu );
                    }
                    else if (listThanhTraTinh.Contains(coQuanInfo.CoQuanID))
                    {
                        Replace(document, "LANH_DAO", "\n" + tenLanhDao + tabstr + "chức vụ:  Chánh thanh tra" );
                    }
                    else if (coQuanInfo.CapID == CapQuanLy.CapUBNDTinh.GetHashCode() || coQuanInfo.CapID == CapQuanLy.CapToanTinh.GetHashCode())
                    {
                        Replace(document, "LANH_DAO", "\n" + tenLanhDao + tabstr + "chức vụ:  Chủ tịch Tỉnh" );
                    }
                    else if (coQuanInfo.CapID == CapQuanLy.CapUBNDHuyen.GetHashCode() || coQuanInfo.CapID == CapQuanLy.ToanHuyen.GetHashCode() || coQuanInfo.CapID == CapQuanLy.CapPhong.GetHashCode())
                    {
                        Replace(document, "LANH_DAO", "\n" + tenLanhDao + tabstr + "chức vụ:  Chủ tịch Huyện" );
                    }
                    else if (coQuanInfo.CapID == CapQuanLy.CapSoNganh.GetHashCode())
                    {
                        Replace(document, "LANH_DAO", "\n" + tenLanhDao + tabstr + "chức vụ:  Giám đốc Sở" );
                    }
                    else if (coQuanInfo.CapID == CapQuanLy.CapUBNDXa.GetHashCode())
                    {
                        Replace(document, "LANH_DAO", "\n" + tenLanhDao + tabstr + "chức vụ:  Chủ tịch Xã" );
                    }
                    else Replace(document, "LANH_DAO", "" );
                }
                else Replace(document, "LANH_DAO", "" );

                if (TiepDanInfo.TiepDanKhongDonID > 0)
                {
                    if (TiepDanInfo.NoiDungTiep != null && TiepDanInfo.NoiDungTiep != "")
                    {
                        Replace(document, "NOI_DUNG_PHANANH_KNTC", TiepDanInfo.NoiDungTiep );
                    }
                    else Replace(document, "NOI_DUNG_PHANANH_KNTC", "…………………………………………………………………………………………" );

                    if (TiepDanInfo.YeuCauNguoiDuocTiep != null && TiepDanInfo.YeuCauNguoiDuocTiep != "")
                    {
                        Replace(document, "YEU_CAU_NGUOI_DUOC_TIEP", TiepDanInfo.YeuCauNguoiDuocTiep );
                    }
                    else Replace(document, "YEU_CAU_NGUOI_DUOC_TIEP", "…………………………………………………………………………………………" );

                    if (TiepDanInfo.ThongTinTaiLieu != null && TiepDanInfo.ThongTinTaiLieu != "")
                    {
                        Replace(document, "THONG_TIN_TAI_LIEU", TiepDanInfo.ThongTinTaiLieu );
                    }
                    else Replace(document, "THONG_TIN_TAI_LIEU", "…………………………………………………………………………………………" );
                }

                if (TiepDanInfo.KetQuaTiep != null && TiepDanInfo.KetQuaTiep != "")
                {
                    Replace(document, "Y_KIEN_CAC_THANH_VIEN", TiepDanInfo.KetQuaTiep );
                }
                else Replace(document, "Y_KIEN_CAC_THANH_VIEN", "…………………………………………………………………………………………" );

                if (TiepDanInfo.KetLuanNguoiChuTri != null && TiepDanInfo.KetLuanNguoiChuTri != "")
                {
                    Replace(document, "KET_LUAN_NGUOI_CHU_TRI", TiepDanInfo.KetLuanNguoiChuTri );
                }
                else Replace(document, "KET_LUAN_NGUOI_CHU_TRI", "…………………………………………………………………………………………" );

                if (TiepDanInfo.NguoiDuocTiepPhatBieu != null && TiepDanInfo.NguoiDuocTiepPhatBieu != "")
                {
                    Replace(document, "Y_KIEN_NGUOI_DUOC_TIEP", TiepDanInfo.NguoiDuocTiepPhatBieu );
                }
                else Replace(document, "Y_KIEN_NGUOI_DUOC_TIEP", "…………………………………………………………………………………………");
            }

            if (coQuanInfo != null)
            {
                var coQuanChaInfo = new CoQuan().GetCoQuanByID(coQuanInfo.CoQuanChaID);
                if (coQuanChaInfo != null)
                {
                     Replace(document, "CO_QUAN_CHA", coQuanChaInfo.TenCoQuan);
                }
                 Replace(document, "CO_QUAN", coQuanInfo.TenCoQuan);

                if (coQuanInfo.TinhID != 0)
                {
                    var tinhInfo = new Tinh().GetByID(coQuanInfo.TinhID);

                    if (tinhInfo != null)
                    {
                         Replace(document, "TEN_TINH", tinhInfo.TenTinh);
                    }
                    else
                    {
                         Replace(document, "TEN_TINH", "");
                    }
                }
                else
                {
                     Replace(document, "TEN_TINH", "");
                }

                //Tên địa giới hành chính
                if (coQuanInfo.XaID != 0)
                {
                    var xaInfo = new Xa().GetByID(coQuanInfo.XaID);
                    if (xaInfo != null)
                    {
                         Replace(document, "TEN_DIA_DANH", xaInfo.TenXa);
                    }
                    else  Replace(document, "TEN_DIA_DANH", "");
                }
                else if (coQuanInfo.HuyenID != 0)
                {
                    var huyenInfo = new Huyen().GetByID(coQuanInfo.HuyenID);
                    if (huyenInfo != null)
                    {
                         Replace(document, "TEN_DIA_DANH", huyenInfo.TenHuyen);
                    }
                    else  Replace(document, "TEN_DIA_DANH", "");
                }
                else if (coQuanInfo.TinhID != 0)
                {
                    var tinhInfo = new Tinh().GetByID(coQuanInfo.TinhID);
                    if (tinhInfo != null)
                    {
                         Replace(document, "TEN_DIA_DANH", tinhInfo.TenTinh);
                    }
                    else  Replace(document, "TEN_DIA_DANH", "");
                }
                else
                {
                     Replace(document, "TEN_DIA_DANH", "");
                }

            }
            else
            {
                 Replace(document, "CO_QUAN_CHA", "...");
                 Replace(document, "CO_QUAN", "...");
            }

            if (TiepDanInfo.TiepDanKhongDonID > 0)
            {
                var caNhanKNInfo = new DoiTuongKN().GetFirstByNhomKNID(TiepDanInfo.NhomKNID ?? 0);
                if (caNhanKNInfo != null)
                {
                     Replace(document, "TEN_CHU_DON", caNhanKNInfo.HoTen);
                    if (caNhanKNInfo.CMND != null)
                    {
                         Replace(document, "%CMND%", caNhanKNInfo.CMND);
                    }
                    else
                    {
                         Replace(document, "%CMND%", "...");
                    }

                    if (caNhanKNInfo.NgayCap != null && caNhanKNInfo.NgayCap != DateTime.MinValue)
                    {
                        DateTime ngayCap = caNhanKNInfo.NgayCap ?? DateTime.Now;
                         Replace(document, "NGAYCAPCMND", caNhanKNInfo.NgayCap != null ? ngayCap.ToString("dd/MM/yyyy") : "");
                    }
                    else  Replace(document, "NGAYCAPCMND", "...");

                    if (caNhanKNInfo.NoiCap != null)
                    {
                         Replace(document, "NOICAPCMND", caNhanKNInfo.NoiCap);
                    }
                    else  Replace(document, "NOICAPCMND", "...");


                    string diachi = "...";
                    if (caNhanKNInfo.TenTinh != null)
                        diachi = caNhanKNInfo.TenTinh;
                    if (caNhanKNInfo.TenHuyen != null)
                        diachi = caNhanKNInfo.TenHuyen + ", " + diachi;
                    if (caNhanKNInfo.TenXa != null)
                        diachi = caNhanKNInfo.TenXa + ", " + diachi;
                    if (caNhanKNInfo.DiaChiCT != null)
                        diachi = caNhanKNInfo.DiaChiCT + ", " + diachi;

                    if (caNhanKNInfo.GioiTinh == 1)
                    {
                         Replace(document, "GIOI_TINH", "Ông");
                    }
                    else  Replace(document, "GIOI_TINH", "Bà");

                     Replace(document, "DIACHICT", diachi);
                     Replace(document, "SODIENTHOAI", caNhanKNInfo.SoDienThoai);
                }
            }

        }

        public void FillXuLyDonData(string document, XuLyDonInfo xldInfo, CoQuanInfo coQuanInfo, CanBoInfo canBoInfo)
        {
            Replace(document, "DAY", DateTime.Now.Day.ToString());
            Replace(document,"MONTH", DateTime.Now.Month.ToString());
            Replace(document,"YEAR", DateTime.Now.Year.ToString());
            Replace(document,"HOUR", DateTime.Now.Hour.ToString());
            Replace(document, "MINUTE", DateTime.Now.Minute.ToString());

            if (canBoInfo != null)
            {
                Replace(document, "TEN_CAN_BO", canBoInfo.TenCanBo);
                Replace(document, "CHUC_VU", canBoInfo.TenChucVu);
            }
            else
            {
                Replace(document, "TEN_CAN_BO", "...");
            }

            if (xldInfo != null)
            {
                Replace(document, "TEN_CAN_BO", "...");

                #region -- ten can bo giai quyet 
                if (xldInfo.TenCanBoPhuTrach != string.Empty)
                {
                    Replace(document, "TENCANBOGIAIQUYET", xldInfo.TenCanBoPhuTrach);
                }
                else
                {
                    Replace(document, "TENCANBOGIAIQUYET", ".......");
                }
                #endregion

                #region -- ten co quan giai quyet
                if (xldInfo.TenCoQuanGiaiQuyet != string.Empty)
                {
                    Replace(document, "TENCOQUANGIAIQUYET", xldInfo.TenCoQuanGiaiQuyet);
                }
                else
                {
                    Replace(document, "TENCOQUANGIAIQUYET", ".....");
                }
                #endregion

                //Replace("NGUOI_KY", "...");
                Replace(document, "SO_DON_THU", xldInfo.SoDonThu.ToString());

                /*create qrcode */
                //QRCodeGenerator qrGenerator = new QRCodeGenerator();
                //QRCodeData qrCodeData = qrGenerator.CreateQrCode(xldInfo.SoDonThu.ToString(), QRCodeGenerator.ECCLevel.Q);
                //QRCode qrCode = new QRCode(qrCodeData);
                //Bitmap qrCodeImage = qrCode.GetGraphic(5);
               
                //bool exists = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/QRCode/"));

                //if (!exists)
                //    System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/QRCode/"));
                //string fileQRcodePath = HttpContext.Current.Server.MapPath("~/QRCode/" + xldInfo.SoDonThu + Utils.GetGMTInMS().ToString().Replace(',', '_').Replace('.', '_') + ".png");
                //qrCodeImage.Save(fileQRcodePath, System.Drawing.Imaging.ImageFormat.Png);


                //Image image = Image.FromFile(fileQRcodePath);

                //TextSelection[] selections = document.FindAllString("QR_CODE", true, true);
                //int index = 0;
                //TextRange range = null;
                //if (selections != null)
                //    foreach (TextSelection selection in selections)
                //    {
                //        DocPicture pic = new DocPicture(document);
                //        pic.LoadImage(image);
                //        range = selection.GetAsOneRange();
                //        index = range.OwnerParagraph.ChildObjects.IndexOf(range);
                //        range.OwnerParagraph.ChildObjects.Insert(index, pic);
                //        range.OwnerParagraph.ChildObjects.Remove(range);
                //    }
           

                if (xldInfo.SoCongVan != string.Empty)
                    Replace(document, "SO_CONG_VAN", xldInfo.SoCongVan);
                else Replace(document, "SO_CONG_VAN", "...");

                if (xldInfo.CQChuyenDonDenID != 0)
                {
                    var cqChuyenDonDenInfo = new CoQuan().GetCoQuanByID(xldInfo.CQChuyenDonDenID);

                    Replace(document, "CQ_CHUYENDONDEN", cqChuyenDonDenInfo.TenCoQuan);
                }
                else
                {
                    Replace(document, "CQ_CHUYENDONDEN", "...");
                }

                if (xldInfo.CanBoKyID != 0)
                {
                    var lanhdaoInfo = new CanBo().GetCanBoByID(xldInfo.CanBoKyID);
                    if (lanhdaoInfo != null)
                    {
                        Replace(document,"NGUOI_KY", lanhdaoInfo.TenCanBo);
                        Replace(document, "LANH_DAO_KY", lanhdaoInfo.TenCanBo);
                    }
                    else
                    {
                        Replace(document, "NGUOI_KY", "...");
                        Replace(document, "LANH_DAO_KY", "...");
                    }
                }
                else
                {
                    Replace(document, "NGUOI_KY", "...");
                }

                if (coQuanInfo.TinhID != 0)
                {
                    var tinhInfo = new Tinh().GetByID(coQuanInfo.TinhID);

                    if (tinhInfo != null)
                    {
                        Replace(document, "TEN_TINH", tinhInfo.TenTinh);
                    }
                    else
                    {
                        Replace(document, "TEN_TINH", "");
                    }
                }
                else
                {
                    Replace(document, "TEN_TINH", "");
                }

                //Tên địa giới hành chính
                if (coQuanInfo.XaID != 0)
                {
                    var xaInfo = new Xa().GetByID(coQuanInfo.XaID);
                    if (xaInfo != null)
                    {
                        Replace(document, "TEN_DIA_DANH", xaInfo.TenXa);
                    }
                    else Replace(document, "TEN_DIA_DANH", "");
                }
                else if (coQuanInfo.HuyenID != 0)
                {
                    var huyenInfo = new Huyen().GetByID(coQuanInfo.HuyenID);
                    if (huyenInfo != null)
                    {
                        Replace(document, "TEN_DIA_DANH", huyenInfo.TenHuyen);
                    }
                    else Replace(document, "TEN_DIA_DANH", "");
                }
                else if (coQuanInfo.TinhID != 0)
                {
                    var tinhInfo = new Tinh().GetByID(coQuanInfo.TinhID);
                    if (tinhInfo != null)
                    {
                        Replace(document, "TEN_DIA_DANH", tinhInfo.TenTinh);
                    }
                    else Replace(document, "TEN_DIA_DANH", "");
                }
                else
                {
                    Replace(document, "TEN_DIA_DANH", "");
                }


                if (coQuanInfo != null)
                {
                    var coQuanChaInfo = new CoQuan().GetCoQuanByID(coQuanInfo.CoQuanChaID);

                    if (coQuanChaInfo != null)
                    {
                        Replace(document, "CO_QUAN_CHA", coQuanChaInfo.TenCoQuan);
                        Replace(document, "CO_QUAN_CHA_UC", coQuanChaInfo.TenCoQuan.ToUpper());
                    }

                    Replace(document, "CO_QUAN", coQuanInfo.TenCoQuan);
                    Replace(document, "CO_QUAN_UC", coQuanInfo.TenCoQuan.ToUpper());
                }
                else
                {
                    Replace(document,"CO_QUAN", "...");
                    Replace(document,"CO_QUAN_UC", "...");
                    Replace(document,"CO_QUAN_CHA", "...");
                    Replace(document, "CO_QUAN_CHA_UC", "...");
                }

                #region -- cq chuyen don
                if (xldInfo.CQChuyenDonID != 0)
                {
                    var coQuanChuyenDonInfo = new CoQuan().GetCoQuanByID(xldInfo.CQChuyenDonID);

                    if (coQuanChuyenDonInfo != null)
                    {
                        Replace(document, "CQ_CHUYENDON", coQuanChuyenDonInfo.TenCoQuan);
                    }
                    else
                    {
                        Replace(document, "CQ_CHUYENDON", "...");
                    }
                }
                else
                {
                    Replace(document, "CQ_CHUYENDON", "...");
                }
                #endregion
            }
        }

        public void FillDonThuData(string document, DonThuInfo donThuInfo)
        {
            //Replace dữ liệu            
            if (donThuInfo != null)
            {
                //Section section = document.Sections[0];
                //TextSelection selection = document.FindString("FILEDINHKEM", true, true);
                //if (selection != null)
                //{
                //    TextRange range = selection.GetAsOneRange();
                //    Paragraph paragraph = range.OwnerParagraph;
                //    Body body = paragraph.OwnerTextBody;
                //    int index = body.ChildObjects.IndexOf(paragraph);

                //    body.ChildObjects.Remove(paragraph);

                //    var fileDinhKemStr = string.Empty;
                //    var fileHoSoList = new FileHoSo().GetByXuLyDonID(donThuInfo.XuLyDonID);

                //    if (fileHoSoList.Count > 0)
                //    {
                //        foreach (var fileModel in fileHoSoList)
                //        {
                //            var para = body.AddParagraph();
                //            para.AppendText(fileModel.TenFile);
                //            body.ChildObjects.Insert(index, para);
                //        }
                //    }
                //}

                var caNhanKNInfo = new DoiTuongKN().GetFirstByNhomKNID(donThuInfo.NhomKNID);

                if (caNhanKNInfo != null)
                {
                    Replace(document, "TEN_CHU_DON", caNhanKNInfo.HoTen);
                    if (caNhanKNInfo.CMND != null)
                    {
                        Replace(document, "%CMND%", caNhanKNInfo.CMND);
                    }
                    else
                    {
                        Replace(document, "%CMND%", "...");
                    }

                    if (caNhanKNInfo.NgayCap != null && caNhanKNInfo.NgayCap != DateTime.MinValue)
                    {
                        DateTime ngayCap = caNhanKNInfo.NgayCap ?? DateTime.Now;
                        Replace(document, "NGAYCAPCMND", caNhanKNInfo.NgayCap != null ? ngayCap.ToString("dd/MM/yyyy") : "");
                    }
                    else Replace(document, "NGAYCAPCMND", "...");

                    if (caNhanKNInfo.NoiCap != null)
                    {
                        Replace(document, "NOICAPCMND", caNhanKNInfo.NoiCap);
                    }
                    else Replace(document, "NOICAPCMND", "...");

                    string diachi = "...";
                    if (caNhanKNInfo.TenTinh != null)
                        diachi = caNhanKNInfo.TenTinh;
                    if (caNhanKNInfo.TenHuyen != null)
                        diachi = caNhanKNInfo.TenHuyen + ", " + diachi;
                    if (caNhanKNInfo.TenXa != null)
                        diachi = caNhanKNInfo.TenXa + ", " + diachi;
                    if (caNhanKNInfo.DiaChiCT != null)
                        diachi = caNhanKNInfo.DiaChiCT + ", " + diachi;

                    if (caNhanKNInfo.GioiTinh == 1)
                    {
                        Replace(document, "GIOI_TINH", "ông");
                    }
                    else Replace(document, "GIOI_TINH", "bà");

                    Replace(document, "DIACHICT", diachi);
                    Replace(document, "SODIENTHOAI", caNhanKNInfo.SoDienThoai);
                }

                if (donThuInfo.TenLoaiKhieuTo1 != null)
                {
                    Replace(document, "LOAI_DON_UC", donThuInfo.TenLoaiKhieuTo1.ToUpper());
                }
                else
                {
                    Replace(document, "LOAI_DON_UC", "...");
                }

                if (donThuInfo.TenLoaiKhieuTo1 != null)
                {
                    Replace(document, "LOAI_DON", donThuInfo.TenLoaiKhieuTo1);
                }
                else
                {
                    Replace(document, "LOAI_DON", "...");
                }

                if (donThuInfo.NoiDungDon != null)
                {
                    Replace(document, "NOI_DUNG_DON", donThuInfo.NoiDungDon);
                }
                else
                {
                    Replace(document, "NOI_DUNG_DON", "...");
                }
            }
        }

        public void FillDTBiKNData(string document, DoiTuongBiKNInfo DTBiKNInfo)
        {
            //Replace dữ liệu            
            if (DTBiKNInfo != null)
            {
                //Section section = document.Sections[0];
                if (DTBiKNInfo.TenDoiTuongBiKN != string.Empty)
                {
                    Replace(document,"TEN_DOITUONG_BIKN", DTBiKNInfo.TenDoiTuongBiKN);
                }
                else
                {
                    Replace(document, "TEN_DOITUONG_BIKN", ".....");
                }

                string diachi = "...";
                if (DTBiKNInfo.TenTinh != null)
                    diachi = DTBiKNInfo.TenTinh;
                if (DTBiKNInfo.TenHuyen != null)
                    diachi = DTBiKNInfo.TenHuyen + ", " + diachi;
                if (DTBiKNInfo.TenXa != null)
                    diachi = DTBiKNInfo.TenXa + ", " + diachi;
                if (DTBiKNInfo.DiaChiCT != null)
                    diachi = DTBiKNInfo.DiaChiCT + ", " + diachi;


                Replace(document, "DIACHICT", diachi);

            }
        }

        public void Clear(string document)
        {
            //don thu
            Replace(document, "TEN_CHU_DON", "");
            Replace(document, "%CMND%", "...");
            Replace(document, "NGAYCAPCMND", "...");
            Replace(document, "NOICAPCMND", "...");
            Replace(document, "GIOI_TINH", "");
            Replace(document, "DIACHICT", "");
            Replace(document, "SODIENTHOAI", "");
            Replace(document, "LOAI_DON_UC", "...");
            Replace(document, "LOAI_DON", "..."); 
            Replace(document, "NOI_DUNG_DON", "...");
            //DT bi KN
            Replace(document, "TEN_DOITUONG_BIKN", ".....");
            Replace(document, "DIACHICT", "");
            //Xu ly don
            Replace(document, "TEN_CAN_BO", "");
            Replace(document, "CHUC_VU", "");
            Replace(document, "TENCANBOGIAIQUYET", ".......");
            Replace(document, "TENCOQUANGIAIQUYET", ".....");
            Replace(document, "SO_DON_THU", "");
            Replace(document, "SO_CONG_VAN", "...");
            Replace(document, "CQ_CHUYENDONDEN", "...");
            Replace(document, "NGUOI_KY", "...");
            Replace(document, "LANH_DAO_KY", "...");
            Replace(document, "TEN_TINH", "");
            Replace(document, "TEN_DIA_DANH", "");
            Replace(document, "CO_QUAN", "...");
            Replace(document, "CO_QUAN_UC", "...");
            Replace(document, "CO_QUAN_CHA", "...");
            Replace(document, "CO_QUAN_CHA_UC", "...");
            Replace(document, "CQ_CHUYENDON", "...");

            Replace(document, "FILEDINHKEM", "");
            Replace(document, "LANH_DAO", "");
            Replace(document, "THANH_PHAN_TIEP_DAN", "");
            Replace(document, "NOI_DUNG_PHANANH_KNTC", "");
            Replace(document, "YEU_CAU_NGUOI_DUOC_TIEP", "");
            Replace(document, "THONG_TIN_TAI_LIEU", "");
            Replace(document, "Y_KIEN_CAC_THANH_VIEN", "");
            Replace(document, "KET_LUAN_NGUOI_CHU_TRI", "");
            Replace(document, "Y_KIEN_NGUOI_DUOC_TIEP", "");
        }


        // To search and replace content in a document part.
        public static void Replace(string document, string matchString, string newValue)
        {
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(document, true))
            {
                string docText = null;
                using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                {
                    docText = sr.ReadToEnd();
                }

                Regex regexText = new Regex(matchString);
                docText = regexText.Replace(docText, newValue);

                using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                }
            }
        }
    }
}
