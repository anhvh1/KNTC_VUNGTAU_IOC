using Com.Gosol.KNTC.DAL.KNTC;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Ultilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.BUS.KNTC
{
    public class RenderTrangThai
    {
        public string TrangThaiMoi { get; set; }
        public int TrangThaiIDMoi { get; set; }
        public bool CheckTrangThai { get; set; }
        public void GetTrangThai(// đơn thư được tạo từ đơn vị nào thì sẽ theo quy trình đó
            int loaiQuyTrinh,
            int? huongGiaiQuyetID,
            string stateName,
            int stateID,
            int nextStateID,
            int trangThaiDuyet,
            int tringDuThao,
            IdentityHelper IdentityHelper,
            DateTime ngayCapNhatTheoDoiXuLy, // trường này ở Giải quyết đơn chuyên viên
            int? chuyenGiaiQuyetID = null,
            int? ketQuaID = null,
            int? lanhDaoDuyet2ID = null,
            // bổ sung trạng thái rút đơn bằng RutDonID
            int? rutDonID = null
            )
        {

            // 1 : SBN
            // 2 : BTD Huyện
            // 3 : Xã
            // 4 : BTD Tỉnh
            // 5 : phòng thuộc sở
            // 6 : phòng thuộc huyện
            if (loaiQuyTrinh == 1) // SBN, phòng thuộc huyện
            {
                SBN(
                    huongGiaiQuyetID,
                    stateName,
                    stateID,
                    nextStateID,
                    trangThaiDuyet,
                    tringDuThao,
                    IdentityHelper,
                    ngayCapNhatTheoDoiXuLy, // trường này ở Giải quyết đơn chuyên viên
                    chuyenGiaiQuyetID,
                    ketQuaID,
                    rutDonID
                );
            }
            else if (loaiQuyTrinh == 2)// Quy trình Huyện 
            {
                BTDCapHuyen(
                    huongGiaiQuyetID,
                    stateName,
                    stateID,
                    nextStateID,
                    trangThaiDuyet,
                    tringDuThao,
                    IdentityHelper,
                    ngayCapNhatTheoDoiXuLy, // trường này ở Giải quyết đơn chuyên viên
                    chuyenGiaiQuyetID,
                    ketQuaID,
                    lanhDaoDuyet2ID,
                    rutDonID
                );
            }
            else if (loaiQuyTrinh == 3) //Quy trình Xã
            {
                CapXa(
                    huongGiaiQuyetID,
                    stateName,
                    stateID,
                    nextStateID,
                    trangThaiDuyet,
                    tringDuThao,
                    IdentityHelper,
                    ngayCapNhatTheoDoiXuLy, // trường này ở Giải quyết đơn chuyên viên
                    chuyenGiaiQuyetID,
                    ketQuaID,
                    rutDonID
                );
            }
            else if (loaiQuyTrinh == 4)//Quy trình Tỉnh
            {
                BTDCapTinh(
                    huongGiaiQuyetID,
                    stateName,
                    stateID,
                    nextStateID,
                    trangThaiDuyet,
                    tringDuThao,
                    IdentityHelper,
                    ngayCapNhatTheoDoiXuLy, // trường này ở Giải quyết đơn chuyên viên
                    chuyenGiaiQuyetID,
                    ketQuaID,
                    rutDonID
                );
            }
            else if (loaiQuyTrinh == 5)//phòng thuộc sở
            {
                PhongThuocSo(
                    huongGiaiQuyetID,
                    stateName,
                    stateID,
                    nextStateID,
                    trangThaiDuyet,
                    tringDuThao,
                    IdentityHelper,
                    ngayCapNhatTheoDoiXuLy, // trường này ở Giải quyết đơn chuyên viên
                    chuyenGiaiQuyetID,
                    ketQuaID,
                    lanhDaoDuyet2ID,
                    rutDonID
                );
            }
            if (loaiQuyTrinh == 6) //phòng thuộc huyện 
            {
                PhongThuocHuyen(
                    huongGiaiQuyetID,
                    stateName,
                    stateID,
                    nextStateID,
                    trangThaiDuyet,
                    tringDuThao,
                    IdentityHelper,
                    ngayCapNhatTheoDoiXuLy, // trường này ở Giải quyết đơn chuyên viên
                    chuyenGiaiQuyetID,
                    ketQuaID,
                    rutDonID
                );
            }
        }
        public void BTDCapTinh(
            int? huongGiaiQuyetID,
            string stateName,
            int stateID,
            int nextStateID,
            int trangThaiDuyet,
            int tringDuThao,
            IdentityHelper IdentityHelper,
            DateTime ngayCapNhatTheoDoiXuLy, // trường này ở Giải quyết đơn chuyên viên
            int? chuyenGiaiQuyetID = null,
            int? ketQuaID = null,
            int? rutDonID = null
            )
        {
            TrangThaiIDMoi = 0;
            CheckTrangThai = false;
            if (huongGiaiQuyetID == 0)
            {
                TrangThaiMoi = "Chưa xử lý";
                TrangThaiIDMoi = 100;
            }
            else
            {
                if (stateName == Constant.CV_TiepNhan || stateID == 11)
                {
                    if (IdentityHelper.SuDungQuyTrinhPhucTap == true)
                    {
                        TrangThaiMoi = "Đã xử lý";
                        TrangThaiIDMoi = 101;
                        if (nextStateID == 11 || trangThaiDuyet == 2)
                        {
                            TrangThaiMoi = "Xử lý lại";
                            TrangThaiIDMoi = 102;
                        }
                    }
                    else
                    {
                        TrangThaiMoi = "Chưa xử lý";
                        TrangThaiIDMoi = 100;
                    }
                }
                else if (stateID == 6)
                {
                    TrangThaiIDMoi = 103;
                    TrangThaiMoi = "Trưởng ban chưa duyệt";
                    if ((IdentityHelper?.RoleID ?? 0) == RoleEnum.ChuyenVien.GetHashCode())
                    {
                        TrangThaiMoi = "Đã trình";
                    }
                    if ((IdentityHelper?.RoleID ?? 0) == RoleEnum.LanhDaoPhong.GetHashCode())
                    {
                        TrangThaiMoi = "Chưa duyệt";
                    }
                }
                else if (stateID == 7/* || stateID == 25*/)
                {
                    if (tringDuThao == 0)
                    {
                        TrangThaiMoi = "Trưởng ban đã duyệt";
                        TrangThaiIDMoi = 104;
                    }
                    else if (tringDuThao == 1)
                    {
                        TrangThaiMoi = "Trưởng ban đã trình";
                        TrangThaiIDMoi = 105;
                    }
                    else if (tringDuThao == 2)
                    {
                        TrangThaiMoi = "Đang xác minh";
                        TrangThaiIDMoi = 200;

                        if (chuyenGiaiQuyetID != null)
                        {
                            if (chuyenGiaiQuyetID > 0)
                            {
                                TrangThaiMoi = "Đã cập nhật quyết định";
                                TrangThaiIDMoi = 201;
                            }
                            else
                            {
                                TrangThaiMoi = "Chưa cập nhật quyết định";
                                TrangThaiIDMoi = 200;
                            }
                        }
                    }

                    //if (chucNangId == ChucNangEnum.GiaiQuyetDon.GetHashCode())
                    //{
                    //    TrangThaiMoi = "Chưa giao xác minh";
                    //    TrangThaiIDMoi = 202;
                    //}

                    if ((IdentityHelper?.RoleID ?? 0) == RoleEnum.LanhDao.GetHashCode() && IdentityHelper.ChuTichUBND == 1)
                    {
                        if (tringDuThao == 0)
                        {
                            TrangThaiMoi = "Chưa duyệt";
                            TrangThaiIDMoi = 104;
                        }
                        else if (tringDuThao == 1)
                        {
                            TrangThaiMoi = "Chưa ban hành quyết định giao xác minh";
                            TrangThaiIDMoi = 105;
                        }
                        else if (tringDuThao == 2)
                        {
                            TrangThaiMoi = "Đã ban hành quyết định giao xác minh";
                            TrangThaiIDMoi = 200;

                            if (chuyenGiaiQuyetID != null)
                            {
                                if (chuyenGiaiQuyetID > 0)
                                {
                                    TrangThaiMoi = "Đã cập nhật quyết định";
                                    TrangThaiIDMoi = 201;
                                }
                                else
                                {
                                    TrangThaiMoi = "Chưa cập nhật quyết định";
                                    TrangThaiIDMoi = 200;
                                }
                            }
                        }
                    }
                }
                else if (stateID == 18)
                {
                    TrangThaiMoi = "Chưa giao xác minh";
                    TrangThaiIDMoi = 202;
                }
                else if (stateID == 19)
                {
                    TrangThaiMoi = "Đang xác minh";
                    TrangThaiIDMoi = 203;

                    if (IdentityHelper.RoleID == RoleEnum.LanhDaoPhong.GetHashCode())
                    {
                        TrangThaiMoi = "Chưa giao xác minh";
                        TrangThaiIDMoi = 203;
                    }

                    if (IdentityHelper.RoleID == RoleEnum.LanhDaoPhong.GetHashCode() && stateID == 19
                    // tuandhh bổ sung cấp lãnh đạo phòng chỉ dành cho btd tỉnh, btd huyện lãnh đạo phân cho ai thì cấp đó xác minh
                    && IdentityHelper.CapHanhChinh != EnumCapHanhChinh.CapPhongThuocHuyen.GetHashCode())
                    {
                        TrangThaiMoi = "Chưa giao xác minh";
                        TrangThaiIDMoi = 202;
                    }
                }
                else if (stateID == 8)
                {
                    //TrangThaiMoi = "Chuyên viên đang xác minh";
                    TrangThaiMoi = "Đang xác minh";
                    TrangThaiIDMoi = 204;
                    if ((IdentityHelper?.RoleID ?? 0) == RoleEnum.ChuyenVien.GetHashCode())
                    {
                        if (ngayCapNhatTheoDoiXuLy == DateTime.MinValue)
                        {
                            TrangThaiMoi = "Chưa xác minh";
                            TrangThaiIDMoi = 204;
                        }
                        else
                        {
                            TrangThaiMoi = "Đang xác minh";
                            TrangThaiIDMoi = 205;
                        }
                    }
                }
                else if (stateID == 21)
                {
                    TrangThaiMoi = "Chuyên viên đã trình báo cáo xác minh";
                    TrangThaiIDMoi = 206;

                    if (IdentityHelper.RoleID == RoleEnum.ChuyenVien.GetHashCode())
                    {
                        TrangThaiMoi = "Đã trình báo cáo xác minh";
                    }
                    if (IdentityHelper.RoleID == RoleEnum.LanhDaoPhong.GetHashCode())
                    {
                        TrangThaiMoi = "Chưa duyệt báo cáo xác minh";
                    }
                    if (IdentityHelper.RoleID == RoleEnum.LanhDao.GetHashCode())
                    {
                        TrangThaiMoi = "Đang xác minh";
                    }

                    if (IdentityHelper.RoleID == RoleEnum.LanhDaoPhong.GetHashCode()
                                || (IdentityHelper.CapID == CapQuanLy.CapUBNDXa.GetHashCode() && IdentityHelper.RoleID == RoleEnum.LanhDao.GetHashCode())
                                || (IdentityHelper.CapHanhChinh == EnumCapHanhChinh.CapPhongThuocHuyen.GetHashCode() && IdentityHelper.RoleID == RoleEnum.LanhDao.GetHashCode())
                                )
                    {
                        TrangThaiMoi = "Chưa duyệt báo cáo xác minh";
                    }
                }
                else if (stateID == 22)
                {
                    TrangThaiMoi = "Trưởng phòng đã trình báo cáo xác minh";
                    TrangThaiIDMoi = 207;
                    if (IdentityHelper.RoleID == 1)
                    {
                        TrangThaiMoi = "Chưa duyệt báo cáo xác minh";
                        TrangThaiIDMoi = 207;
                    }
                }
                else if (stateID == 9)
                {
                    TrangThaiMoi = "Chưa ban hành quyết định";
                    TrangThaiIDMoi = 208;
                }
                else if (stateID == 10)
                {
                    TrangThaiMoi = "Đang thi hành quyết định giải quyết";
                    TrangThaiIDMoi = 300;

                    if (ketQuaID != 0)
                    {
                        TrangThaiMoi = "Đã cập nhập quyết định giải quyết";
                        TrangThaiIDMoi = 301;
                    }

                    if (huongGiaiQuyetID != (int)HuongGiaiQuyetEnum.DeXuatThuLy)
                    {
                        if (huongGiaiQuyetID == (int)HuongGiaiQuyetEnum.ChuyenDon)
                        {
                            TrangThaiIDMoi = 401;
                            TrangThaiMoi = "Đã chuyển đơn";
                        }
                        else if (huongGiaiQuyetID == (int)HuongGiaiQuyetEnum.HuongDan)
                        {
                            TrangThaiIDMoi = 402;
                            TrangThaiMoi = "Đã hướng dẫn";
                        }
                        else if (huongGiaiQuyetID == (int)HuongGiaiQuyetEnum.RaVanBanDonDoc || huongGiaiQuyetID == (int)HuongGiaiQuyetEnum.CongVanChiDao)
                        {
                            TrangThaiIDMoi = 403;
                            TrangThaiMoi = "Đã đôn đốc";
                        }
                        else if (huongGiaiQuyetID == (int)HuongGiaiQuyetEnum.CongVanChiDao)
                        {
                            TrangThaiIDMoi = 404;
                            TrangThaiMoi = "Đã chỉ đạo";
                        }
                        else
                        {
                            TrangThaiIDMoi = 405;
                            TrangThaiMoi = "Đã hoàn thành";
                        }
                    }
                    // bổ sung trạng thái đã rút đơn
                    if (rutDonID != 0)
                    {
                        TrangThaiIDMoi = 406;
                        TrangThaiMoi = "Đã rút đơn";
                    }
                }
            }

            CheckTrangThai = TrangThaiIDMoi > 0 ? true : false;
        }

        public void PhongThuocSo(
            int? huongGiaiQuyetID,
            string stateName,
            int stateID,
            int nextStateID,
            int trangThaiDuyet,
            int tringDuThao,
            IdentityHelper IdentityHelper,
            DateTime ngayCapNhatTheoDoiXuLy, // trường này ở Giải quyết đơn chuyên viên
            int? chuyenGiaiQuyetID = null,
            int? ketQuaID = null,
            int? lanhDaoDuyet2ID = null,
            int? rutDonID = null
            )
        {
            TrangThaiIDMoi = 0;
            CheckTrangThai = false;
            if (huongGiaiQuyetID == 0)
            {
                TrangThaiMoi = "Chưa xử lý";
                TrangThaiIDMoi = 100;
            }
            else
            {
                if (stateName == Constant.CV_TiepNhan || stateID == 11)
                {
                    if (IdentityHelper.SuDungQuyTrinhPhucTap == true)
                    {
                        TrangThaiMoi = "Đã xử lý";
                        TrangThaiIDMoi = 101;
                        if (nextStateID == 11 || trangThaiDuyet == 2)
                        {
                            TrangThaiMoi = "Xử lý lại";
                            TrangThaiIDMoi = 102;
                        }
                    }
                    else
                    {
                        TrangThaiMoi = "Chưa xử lý";
                        TrangThaiIDMoi = 100;
                    }
                }
                else if (stateID == 6)
                {
                    TrangThaiIDMoi = 103;
                    TrangThaiMoi = "Lãnh đạo chưa duyệt";
                    if ((IdentityHelper?.RoleID ?? 0) == RoleEnum.ChuyenVien.GetHashCode())
                    {
                        TrangThaiMoi = "Đã trình";
                    }
                    else /*((IdentityHelper?.RoleID ?? 0) == RoleEnum.LanhDaoPhong.GetHashCode())*/
                    {
                        TrangThaiMoi = "Chưa duyệt";
                    }
                }
                else if (stateID == 7)
                {
                    if (trangThaiDuyet == 0)
                    {
                        TrangThaiMoi = "Đã duyệt";
                        TrangThaiIDMoi = 104;
                        if ((IdentityHelper?.RoleID ?? 0) == RoleEnum.ChuyenVien.GetHashCode())
                        {
                            TrangThaiMoi = "Đã trình";
                        }
                        if (lanhDaoDuyet2ID > 0)
                        {
                            TrangThaiMoi = "Đã trình";
                            TrangThaiIDMoi = 105;
                        }
                        if (lanhDaoDuyet2ID > 0 && lanhDaoDuyet2ID == IdentityHelper.CanBoID)
                        {
                            TrangThaiMoi = "Chưa duyệt";
                            TrangThaiIDMoi = 105;
                        }

                    }
                    else if (trangThaiDuyet == 1)
                    {
                        TrangThaiMoi = "Đã duyệt";
                        TrangThaiIDMoi = 106;
                        if (lanhDaoDuyet2ID > 0 && lanhDaoDuyet2ID == IdentityHelper.CanBoID)
                        {                            
                            TrangThaiMoi = "Đã duyệt và chưa giao xác minh";
                            TrangThaiIDMoi = 202;
                            if (huongGiaiQuyetID != (int)HuongGiaiQuyetEnum.DeXuatThuLy)
                            {
                                TrangThaiMoi = "Đã duyệt";
                                TrangThaiIDMoi = 202;
                            }
                        }
                        
                    }
                }
                else if (stateID == 19)
                {
                    TrangThaiMoi = "Đang xác minh";
                    TrangThaiIDMoi = 203;

                    if (lanhDaoDuyet2ID > 0 && lanhDaoDuyet2ID != IdentityHelper.CanBoID)
                    {
                        TrangThaiMoi = "Chưa giao xác minh";
                        TrangThaiIDMoi = 203;
                    }

                    if (IdentityHelper.RoleID == RoleEnum.LanhDaoPhong.GetHashCode() && stateID == 19
                    // tuandhh bổ sung cấp lãnh đạo phòng chỉ dành cho btd tỉnh, btd huyện lãnh đạo phân cho ai thì cấp đó xác minh
                    && IdentityHelper.CapHanhChinh != EnumCapHanhChinh.CapPhongThuocHuyen.GetHashCode())
                    {
                        TrangThaiMoi = "Chưa giao xác minh";
                        TrangThaiIDMoi = 202;
                    }
                }
                else if (stateID == 8)
                {
                    //TrangThaiMoi = "Chuyên viên đang xác minh";
                    TrangThaiMoi = "Đang xác minh";
                    TrangThaiIDMoi = 204;
                    if ((IdentityHelper?.RoleID ?? 0) == RoleEnum.ChuyenVien.GetHashCode())
                    {
                        if (ngayCapNhatTheoDoiXuLy == DateTime.MinValue)
                        {
                            TrangThaiMoi = "Chưa xác minh";
                            TrangThaiIDMoi = 204;
                        }
                        else
                        {
                            TrangThaiMoi = "Đang xác minh";
                            TrangThaiIDMoi = 205;
                        }
                    }
                }
                else if (stateID == 21)
                {
                    TrangThaiMoi = "Chuyên viên đã trình báo cáo xác minh";
                    TrangThaiIDMoi = 206;

                    if (IdentityHelper.RoleID == RoleEnum.ChuyenVien.GetHashCode())
                    {
                        TrangThaiMoi = "Đã trình báo cáo xác minh";
                    }
                    if (IdentityHelper.RoleID == RoleEnum.LanhDaoPhong.GetHashCode())
                    {
                        TrangThaiMoi = "Chưa duyệt báo cáo xác minh";
                    }
                    if (IdentityHelper.RoleID == RoleEnum.LanhDao.GetHashCode())
                    {
                        TrangThaiMoi = "Đang xác minh";
                    }

                    if (IdentityHelper.RoleID == RoleEnum.LanhDaoPhong.GetHashCode()
                                || (IdentityHelper.CapID == CapQuanLy.CapUBNDXa.GetHashCode() && IdentityHelper.RoleID == RoleEnum.LanhDao.GetHashCode())
                                || (IdentityHelper.CapHanhChinh == EnumCapHanhChinh.CapPhongThuocHuyen.GetHashCode() && IdentityHelper.RoleID == RoleEnum.LanhDao.GetHashCode())
                                )
                    {
                        TrangThaiMoi = "Chưa duyệt báo cáo xác minh";
                    }
                }
                else if (stateID == 9)
                {
                    TrangThaiMoi = "Đã trình báo cáo xác minh";
                    TrangThaiIDMoi = 207;
                    if (IdentityHelper.RoleID == 1)
                    {
                        TrangThaiMoi = "Chưa duyệt báo cáo xác minh";
                        TrangThaiIDMoi = 207;
                    }
                }
                else if (stateID == 10)
                {
                    TrangThaiMoi = "Đang thi hành quyết định giải quyết";
                    TrangThaiIDMoi = 300;
                    //if (chucNangId == ChucNangEnum.CapNhatQuyetDinhGiaiQuyet.GetHashCode())
                    //{
                    //    TrangThaiMoi = "Chưa cập nhập";
                    //    TrangThaiIDMoi = 300;
                    //}
                    if (ketQuaID != 0)
                    {
                        TrangThaiMoi = "Đã cập nhập quyết định giải quyết";
                        TrangThaiIDMoi = 301;
                    }

                    if (huongGiaiQuyetID != (int)HuongGiaiQuyetEnum.DeXuatThuLy)
                    {
                        if (huongGiaiQuyetID == (int)HuongGiaiQuyetEnum.ChuyenDon)
                        {
                            TrangThaiIDMoi = 401;
                            TrangThaiMoi = "Đã chuyển đơn";
                        }
                        else if (huongGiaiQuyetID == (int)HuongGiaiQuyetEnum.HuongDan)
                        {
                            TrangThaiIDMoi = 402;
                            TrangThaiMoi = "Đã hướng dẫn";
                        }
                        else if (huongGiaiQuyetID == (int)HuongGiaiQuyetEnum.RaVanBanDonDoc || huongGiaiQuyetID == (int)HuongGiaiQuyetEnum.CongVanChiDao)
                        {
                            TrangThaiIDMoi = 403;
                            TrangThaiMoi = "Đã đôn đốc";
                        }
                        else if (huongGiaiQuyetID == (int)HuongGiaiQuyetEnum.CongVanChiDao)
                        {
                            TrangThaiIDMoi = 404;
                            TrangThaiMoi = "Đã chỉ đạo";
                        }
                        else
                        {
                            TrangThaiIDMoi = 405;
                            TrangThaiMoi = "Đã hoàn thành";
                        }
                    }
                    // bổ sung trạng thái đã rút đơn
                    if (rutDonID != 0)
                    {
                        TrangThaiIDMoi = 406;
                        TrangThaiMoi = "Đã rút đơn";
                    }
                }
            }

            CheckTrangThai = TrangThaiIDMoi > 0 ? true : false;
        }

        public void SBN( // sở ban ngành, phòng thuộc huyện, phòng thuộc sở
        int? huongGiaiQuyetID,
        string stateName,
        int stateID,
        int nextStateID,
        int trangThaiDuyet,
        int tringDuThao,
        IdentityHelper IdentityHelper,
        DateTime ngayCapNhatTheoDoiXuLy, // trường này ở Giải quyết đơn chuyên viên
        int? chuyenGiaiQuyetID = null,
        int? ketQuaID = null,
        // bổ sung trạng thái rút đơn bằng RutDonID
        int? rutDonID = null
        )
        {
            TrangThaiIDMoi = 0;
            CheckTrangThai = false;
            if (huongGiaiQuyetID == 0)
            {
                TrangThaiMoi = "Chưa xử lý";
                TrangThaiIDMoi = 100;
            }
            else
            {
                if (stateName == Constant.CV_TiepNhan || stateID == 11)
                {
                    if (IdentityHelper.SuDungQuyTrinhPhucTap == true)
                    {
                        TrangThaiMoi = "Đã xử lý";
                        TrangThaiIDMoi = 101;
                        if (nextStateID == 11 || trangThaiDuyet == 2)
                        {
                            TrangThaiMoi = "Xử lý lại";
                            TrangThaiIDMoi = 102;
                        }
                    }
                    else
                    {
                        TrangThaiMoi = "Chưa xử lý";
                        TrangThaiIDMoi = 100;
                    }
                }
                else if (stateID == 5)
                {
                    TrangThaiIDMoi = 103;
                    TrangThaiMoi = "Trưởng phòng chưa duyệt";
                    if ((IdentityHelper?.RoleID ?? 0) == RoleEnum.ChuyenVien.GetHashCode())
                    {
                        TrangThaiMoi = "Đã trình";
                    }
                    if ((IdentityHelper?.RoleID ?? 0) == RoleEnum.LanhDaoPhong.GetHashCode())
                    {
                        TrangThaiMoi = "Chưa duyệt";
                    }
                }
                else if (stateID == 6)
                {
                    TrangThaiIDMoi = 104;
                    TrangThaiMoi = "Trưởng phòng đã duyệt";
                    if ((IdentityHelper?.RoleID ?? 0) == RoleEnum.LanhDaoPhong.GetHashCode())
                    {
                        TrangThaiMoi = "Đã duyệt";
                    }
                    if ((IdentityHelper?.RoleID ?? 0) == RoleEnum.LanhDao.GetHashCode())
                    {
                        TrangThaiMoi = "Chưa duyệt";
                    }
                }
                else if (stateID == 7)
                {
                    if (trangThaiDuyet == 0)
                    {
                        TrangThaiMoi = "Trưởng phòng đã duyệt";
                        TrangThaiIDMoi = 105;
                        if ((IdentityHelper?.RoleID ?? 0) == RoleEnum.LanhDaoPhong.GetHashCode())
                        {
                            TrangThaiMoi = "Đã trình";
                        }
                        if ((IdentityHelper?.RoleID ?? 0) == RoleEnum.LanhDao.GetHashCode())
                        {
                            TrangThaiMoi = "Chưa duyệt";
                        }
                    }
                    else if (trangThaiDuyet == 1)
                    {
                        TrangThaiMoi = "Lãnh đạo đã duyệt";
                        TrangThaiIDMoi = 106;
                        if ((IdentityHelper?.RoleID ?? 0) == RoleEnum.LanhDaoPhong.GetHashCode())
                        {
                            TrangThaiMoi = "Đã duyệt";
                        }
                        if ((IdentityHelper?.RoleID ?? 0) == RoleEnum.LanhDao.GetHashCode())
                        {
                            TrangThaiMoi = "Đã duyệt và chưa giao xác minh";
                            TrangThaiIDMoi = 202;
                            if (huongGiaiQuyetID != (int)HuongGiaiQuyetEnum.DeXuatThuLy)
                            {
                                TrangThaiMoi = "Đã duyệt";
                                TrangThaiIDMoi = 202;
                            }
                        }
                    }
                }
                else if (stateID == 19)
                {
                    TrangThaiMoi = "Đang xác minh";
                    TrangThaiIDMoi = 203;

                    if (IdentityHelper.RoleID == RoleEnum.LanhDaoPhong.GetHashCode())
                    {
                        TrangThaiMoi = "Chưa giao xác minh";
                        TrangThaiIDMoi = 203;
                    }

                    if (IdentityHelper.RoleID == RoleEnum.LanhDaoPhong.GetHashCode() && stateID == 19
                    // tuandhh bổ sung cấp lãnh đạo phòng chỉ dành cho btd tỉnh, btd huyện lãnh đạo phân cho ai thì cấp đó xác minh
                    && IdentityHelper.CapHanhChinh != EnumCapHanhChinh.CapPhongThuocHuyen.GetHashCode())
                    {
                        TrangThaiMoi = "Chưa giao xác minh";
                        TrangThaiIDMoi = 202;
                    }
                }
                else if (stateID == 8)
                {
                    //TrangThaiMoi = "Chuyên viên đang xác minh";
                    TrangThaiMoi = "Đang xác minh";
                    TrangThaiIDMoi = 204;
                    if ((IdentityHelper?.RoleID ?? 0) == RoleEnum.ChuyenVien.GetHashCode())
                    {
                        if (ngayCapNhatTheoDoiXuLy == DateTime.MinValue)
                        {
                            TrangThaiMoi = "Chưa xác minh";
                            TrangThaiIDMoi = 204;
                        }
                        else
                        {
                            TrangThaiMoi = "Đã xác minh";
                            TrangThaiIDMoi = 205;
                        }
                    }
                }
                else if (stateID == 21)
                {
                    TrangThaiMoi = "Chuyên viên đã trình báo cáo xác minh";
                    TrangThaiIDMoi = 206;
                    if (IdentityHelper.RoleID == RoleEnum.ChuyenVien.GetHashCode())
                    {
                        TrangThaiMoi = "Đã trình báo cáo xác minh";
                    }
                    if (IdentityHelper.RoleID == RoleEnum.LanhDaoPhong.GetHashCode())
                    {
                        TrangThaiMoi = "Chưa duyệt báo cáo xác minh";
                    }
                    if (IdentityHelper.RoleID == RoleEnum.LanhDao.GetHashCode())
                    {
                        TrangThaiMoi = "Đang xác minh";
                    }

                    if (IdentityHelper.RoleID == RoleEnum.LanhDaoPhong.GetHashCode()
                                || (IdentityHelper.CapID == CapQuanLy.CapUBNDXa.GetHashCode() && IdentityHelper.RoleID == RoleEnum.LanhDao.GetHashCode())
                                || (IdentityHelper.CapHanhChinh == EnumCapHanhChinh.CapPhongThuocHuyen.GetHashCode() && IdentityHelper.RoleID == RoleEnum.LanhDao.GetHashCode())
                                )
                    {
                        TrangThaiMoi = "Chưa duyệt báo cáo xác minh";
                    }
                }
                else if (stateID == 9)
                {
                    TrangThaiMoi = "Đã trình báo cáo xác minh";
                    TrangThaiIDMoi = 207;
                    if (IdentityHelper.RoleID == RoleEnum.LanhDaoPhong.GetHashCode())
                    {
                        TrangThaiMoi = "Đã trình báo cáo xác minh";
                    }
                    if (IdentityHelper.RoleID == RoleEnum.LanhDao.GetHashCode())
                    {
                        TrangThaiMoi = "Chưa duyệt báo cáo xác minh";
                    }
                }
                else if (stateID == 10)
                {
                    TrangThaiMoi = "Đang thi hành quyết định giải quyết";
                    TrangThaiIDMoi = 300;

                    if (ketQuaID != 0)
                    {
                        TrangThaiMoi = "Đã cập nhập quyết định giải quyết";
                        TrangThaiIDMoi = 301;
                    }

                    if (huongGiaiQuyetID != (int)HuongGiaiQuyetEnum.DeXuatThuLy)
                    {
                        if (huongGiaiQuyetID == (int)HuongGiaiQuyetEnum.ChuyenDon)
                        {
                            TrangThaiIDMoi = 401;
                            TrangThaiMoi = "Đã chuyển đơn";
                        }
                        else if (huongGiaiQuyetID == (int)HuongGiaiQuyetEnum.HuongDan)
                        {
                            TrangThaiIDMoi = 402;
                            TrangThaiMoi = "Đã hướng dẫn";
                        }
                        else if (huongGiaiQuyetID == (int)HuongGiaiQuyetEnum.RaVanBanDonDoc || huongGiaiQuyetID == (int)HuongGiaiQuyetEnum.CongVanChiDao)
                        {
                            TrangThaiIDMoi = 403;
                            TrangThaiMoi = "Đã đôn đốc";
                        }
                        else if (huongGiaiQuyetID == (int)HuongGiaiQuyetEnum.CongVanChiDao)
                        {
                            TrangThaiIDMoi = 404;
                            TrangThaiMoi = "Đã chỉ đạo";
                        }
                        else
                        {
                            TrangThaiIDMoi = 405;
                            TrangThaiMoi = "Đã hoàn thành";
                        }
                    }
                    // bổ sung trạng thái đã rút đơn
                    if ((rutDonID ?? 0) != 0)
                    {
                        TrangThaiIDMoi = 406;
                        TrangThaiMoi = "Đã rút đơn";
                    }
                }

            }
            CheckTrangThai = TrangThaiIDMoi > 0 ? true : false;
        }
        public void BTDCapHuyen(
            int? huongGiaiQuyetID,
            string stateName,
            int stateID,
            int nextStateID,
            int trangThaiDuyet,
            int tringDuThao,
            IdentityHelper IdentityHelper,
            DateTime ngayCapNhatTheoDoiXuLy, // trường này ở Giải quyết đơn chuyên viên
            int? chuyenGiaiQuyetID = null,
            int? ketQuaID = null,
            int? lanhDaoDuyet2ID = null,
            int? rutDonID = null
            )
        {
            TrangThaiIDMoi = 0;
            CheckTrangThai = false;
            if (huongGiaiQuyetID == 0)
            {
                TrangThaiMoi = "Chưa xử lý";
                TrangThaiIDMoi = 100;
            }
            else
            {
                if (stateName == Constant.CV_TiepNhan || stateID == 11)
                {
                    if (IdentityHelper.SuDungQuyTrinhPhucTap == true)
                    {
                        TrangThaiMoi = "Đã xử lý";
                        TrangThaiIDMoi = 101;
                        if (nextStateID == 11 || trangThaiDuyet == 2)
                        {
                            TrangThaiMoi = "Xử lý lại";
                            TrangThaiIDMoi = 102;
                        }
                    }
                    else
                    {
                        TrangThaiMoi = "Chưa xử lý";
                        TrangThaiIDMoi = 100;
                    }
                }
                else if (stateID == 6)
                {
                    TrangThaiIDMoi = 103;
                    TrangThaiMoi = "Trưởng ban chưa duyệt";
                    if ((IdentityHelper?.RoleID ?? 0) == RoleEnum.ChuyenVien.GetHashCode())
                    {
                        TrangThaiMoi = "Đã trình";
                    }
                    if ((IdentityHelper?.RoleID ?? 0) == RoleEnum.LanhDao.GetHashCode())
                    {
                        TrangThaiMoi = "Chưa duyệt";
                    }
                }
                else if (stateID == 7)
                {
                    if (trangThaiDuyet == 0)
                    {
                        TrangThaiIDMoi = 104;
                        TrangThaiMoi = "Đã duyệt";
                        if (lanhDaoDuyet2ID > 0)
                        {
                            TrangThaiMoi = "Đã trình";
                            TrangThaiIDMoi = 105;
                        }
                        if ((IdentityHelper?.RoleID ?? 0) == RoleEnum.LanhDao.GetHashCode() && IdentityHelper.ChuTichUBND == 1)
                        {
                            if (lanhDaoDuyet2ID > 0)
                            {
                                TrangThaiMoi = "Chưa duyệt";
                                TrangThaiIDMoi = 105;
                            }
                            else
                            {
                                TrangThaiMoi = "Chưa duyệt";
                                TrangThaiIDMoi = 104;
                            }
                        }
                        if ((IdentityHelper?.RoleID ?? 0) == RoleEnum.ChuyenVien.GetHashCode())
                        {
                            TrangThaiMoi = "Đã trình";
                            TrangThaiIDMoi = 105;
                        }

                    }
                    else if (trangThaiDuyet == 1)
                    {
                        TrangThaiIDMoi = 106;
                        TrangThaiMoi = "Đã duyệt";
                    }

                    //if ((IdentityHelper?.RoleID ?? 0) == RoleEnum.LanhDao.GetHashCode() && IdentityHelper.ChuTichUBND == 1)
                    //{
                    //    if (tringDuThao == 0)
                    //    {
                    //        TrangThaiMoi = "Chưa duyệt";
                    //        TrangThaiIDMoi = 105;
                    //    }
                    //    else if (tringDuThao == 1)
                    //    {
                    //        TrangThaiIDMoi = 106;
                    //        TrangThaiMoi = "Đã duyệt và chưa ban hành quyết định giao xác minh";
                    //    }
                    //}
                }
                else if (stateID == 18)
                {
                    TrangThaiMoi = "Chưa giao xác minh";
                    TrangThaiIDMoi = 202;
                }
                else if (stateID == 8)
                {
                    //TrangThaiMoi = "Chuyên viên đang xác minh";
                    TrangThaiMoi = "Đang xác minh";
                    TrangThaiIDMoi = 204;
                    if (ngayCapNhatTheoDoiXuLy == DateTime.MinValue)
                    {
                        TrangThaiMoi = "Chưa xác minh";
                        TrangThaiIDMoi = 204;
                    }
                    else
                    {
                        TrangThaiMoi = "Đang xác minh";
                        TrangThaiIDMoi = 205;
                    }
                }
                else if (stateID == 22)
                {
                    TrangThaiMoi = "Đã trình báo cáo xác minh";
                    TrangThaiIDMoi = 206;

                    if (IdentityHelper.RoleID == RoleEnum.LanhDao.GetHashCode())
                    {
                        TrangThaiMoi = "Chưa duyệt báo cáo xác minh";
                        TrangThaiIDMoi = 207;
                    }
                }
                else if (stateID == 9)
                {
                    TrangThaiMoi = "Chưa ban hành quyết định";
                    TrangThaiIDMoi = 209;
                }
                else if (stateID == 10)
                {
                    TrangThaiMoi = "Đang thi hành quyết định giải quyết";
                    TrangThaiIDMoi = 300;
                    //if (chucNangId == ChucNangEnum.CapNhatQuyetDinhGiaiQuyet.GetHashCode())
                    //{
                    //    TrangThaiMoi = "Chưa cập nhập";
                    //    TrangThaiIDMoi = 300;
                    //}
                    if (ketQuaID != 0)
                    {
                        TrangThaiMoi = "Đã cập nhập quyết định giải quyết";
                        TrangThaiIDMoi = 301;
                    }

                    if (huongGiaiQuyetID != (int)HuongGiaiQuyetEnum.DeXuatThuLy)
                    {
                        if (huongGiaiQuyetID == (int)HuongGiaiQuyetEnum.ChuyenDon)
                        {
                            TrangThaiIDMoi = 401;
                            TrangThaiMoi = "Đã chuyển đơn";
                        }
                        else if (huongGiaiQuyetID == (int)HuongGiaiQuyetEnum.HuongDan)
                        {
                            TrangThaiIDMoi = 402;
                            TrangThaiMoi = "Đã hướng dẫn";
                        }
                        else if (huongGiaiQuyetID == (int)HuongGiaiQuyetEnum.RaVanBanDonDoc || huongGiaiQuyetID == (int)HuongGiaiQuyetEnum.CongVanChiDao)
                        {
                            TrangThaiIDMoi = 403;
                            TrangThaiMoi = "Đã đôn đốc";
                        }
                        else if (huongGiaiQuyetID == (int)HuongGiaiQuyetEnum.CongVanChiDao)
                        {
                            TrangThaiIDMoi = 404;
                            TrangThaiMoi = "Đã chỉ đạo";
                        }
                        else
                        {
                            TrangThaiIDMoi = 405;
                            TrangThaiMoi = "Đã hoàn thành";
                        }
                    }
                    // bổ sung trạng thái đã rút đơn
                    if (rutDonID != 0)
                    {
                        TrangThaiIDMoi = 406;
                        TrangThaiMoi = "Đã rút đơn";
                    }
                }
            }

            CheckTrangThai = TrangThaiIDMoi > 0 ? true : false;
        }

        public void CapXa(
            int? huongGiaiQuyetID,
            string stateName,
            int stateID,
            int nextStateID,
            int trangThaiDuyet,
            int tringDuThao,
            IdentityHelper IdentityHelper,
            DateTime ngayCapNhatTheoDoiXuLy, // trường này ở Giải quyết đơn chuyên viên
            int? chuyenGiaiQuyetID = null,
            int? ketQuaID = null,
            int? rutDonID = null
            )
        {
            TrangThaiIDMoi = 0;
            CheckTrangThai = false;
            if (huongGiaiQuyetID == 0)
            {
                TrangThaiMoi = "Chưa xử lý";
                TrangThaiIDMoi = 100;
            }
            else
            {
                if (stateName == Constant.CV_TiepNhan || stateID == 11)
                {
                    if (IdentityHelper.SuDungQuyTrinhPhucTap == true)
                    {
                        TrangThaiMoi = "Đã xử lý";
                        TrangThaiIDMoi = 101;
                        if (nextStateID == 11 || trangThaiDuyet == 2)
                        {
                            TrangThaiMoi = "Xử lý lại";
                            TrangThaiIDMoi = 102;
                        }
                    }
                    else
                    {
                        TrangThaiMoi = "Chưa xử lý";
                        TrangThaiIDMoi = 100;
                    }
                }
                else if (stateID == 6)
                {
                    TrangThaiIDMoi = 105;
                    if ((IdentityHelper?.RoleID ?? 0) == RoleEnum.ChuyenVien.GetHashCode())
                    {
                        TrangThaiMoi = "Đã trình";
                    }
                    if ((IdentityHelper?.RoleID ?? 0) == RoleEnum.LanhDao.GetHashCode())
                    {
                        TrangThaiMoi = "Chưa duyệt";
                    }
                }
                else if (stateID == 7)
                {
                    TrangThaiIDMoi = 106;
                    TrangThaiMoi = "Đã duyệt";
                    if ((IdentityHelper?.RoleID ?? 0) == RoleEnum.LanhDao.GetHashCode())
                    {
                        TrangThaiMoi = "Đã duyệt và chưa giao xác minh";
                        TrangThaiIDMoi = 202;
                        if (huongGiaiQuyetID != (int)HuongGiaiQuyetEnum.DeXuatThuLy)
                        {
                            TrangThaiMoi = "Đã duyệt";
                            TrangThaiIDMoi = 202;
                        }
                    }
                }
                else if (stateID == 8)
                {
                    //TrangThaiMoi = "Chuyên viên đang xác minh";
                    TrangThaiMoi = "Đang xác minh";
                    TrangThaiIDMoi = 204;
                    if (ngayCapNhatTheoDoiXuLy == DateTime.MinValue)
                    {
                        TrangThaiMoi = "Chưa xác minh";
                        TrangThaiIDMoi = 204;
                    }
                    else
                    {
                        TrangThaiMoi = "Đang xác minh";
                        TrangThaiIDMoi = 205;
                    }
                }
                else if (stateID == 21)
                {
                    TrangThaiMoi = "Đã trình báo cáo xác minh";
                    TrangThaiIDMoi = 206;

                    if (IdentityHelper.RoleID == RoleEnum.LanhDao.GetHashCode())
                    {
                        TrangThaiMoi = "Chưa duyệt báo cáo xác minh";
                        TrangThaiIDMoi = 207;
                    }
                }
                else if (stateID == 10)
                {
                    TrangThaiMoi = "Đang thi hành quyết định giải quyết";
                    TrangThaiIDMoi = 300;
                    //if (chucNangId == ChucNangEnum.CapNhatQuyetDinhGiaiQuyet.GetHashCode())
                    //{
                    //    TrangThaiMoi = "Chưa cập nhập";
                    //    TrangThaiIDMoi = 300;
                    //}
                    if (ketQuaID != 0)
                    {
                        TrangThaiMoi = "Đã cập nhập quyết định giải quyết";
                        TrangThaiIDMoi = 301;
                    }

                    if (huongGiaiQuyetID != (int)HuongGiaiQuyetEnum.DeXuatThuLy)
                    {
                        if (huongGiaiQuyetID == (int)HuongGiaiQuyetEnum.ChuyenDon)
                        {
                            TrangThaiIDMoi = 401;
                            TrangThaiMoi = "Đã chuyển đơn";
                        }
                        else if (huongGiaiQuyetID == (int)HuongGiaiQuyetEnum.HuongDan)
                        {
                            TrangThaiIDMoi = 402;
                            TrangThaiMoi = "Đã hướng dẫn";
                        }
                        else if (huongGiaiQuyetID == (int)HuongGiaiQuyetEnum.RaVanBanDonDoc || huongGiaiQuyetID == (int)HuongGiaiQuyetEnum.CongVanChiDao)
                        {
                            TrangThaiIDMoi = 403;
                            TrangThaiMoi = "Đã đôn đốc";
                        }
                        else if (huongGiaiQuyetID == (int)HuongGiaiQuyetEnum.CongVanChiDao)
                        {
                            TrangThaiIDMoi = 404;
                            TrangThaiMoi = "Đã chỉ đạo";
                        }
                        else
                        {
                            TrangThaiIDMoi = 405;
                            TrangThaiMoi = "Đã hoàn thành";
                        }
                    }
                    // bổ sung trạng thái đã rút đơn
                    if (rutDonID != 0)
                    {
                        TrangThaiIDMoi = 406;
                        TrangThaiMoi = "Đã rút đơn";
                    }
                }
            }

            CheckTrangThai = TrangThaiIDMoi > 0 ? true : false;
        }

        public void PhongThuocHuyen( // sở ban ngành, phòng thuộc huyện, phòng thuộc sở
           int? huongGiaiQuyetID,
           string stateName,
           int stateID,
           int nextStateID,
           int trangThaiDuyet,
           int tringDuThao,
           IdentityHelper IdentityHelper,
           DateTime ngayCapNhatTheoDoiXuLy, // trường này ở Giải quyết đơn chuyên viên
           int? chuyenGiaiQuyetID = null,
           int? ketQuaID = null,
           int? rutDonID = null
        )
        {
            TrangThaiIDMoi = 0;
            CheckTrangThai = false;
            if (huongGiaiQuyetID == 0)
            {
                TrangThaiMoi = "Chưa xử lý";
                TrangThaiIDMoi = 100;
            }
            else
            {
                if (stateName == Constant.CV_TiepNhan || stateID == 11)
                {
                    if (IdentityHelper.SuDungQuyTrinhPhucTap == true)
                    {
                        TrangThaiMoi = "Đã xử lý";
                        TrangThaiIDMoi = 101;
                        if (nextStateID == 11 || trangThaiDuyet == 2)
                        {
                            TrangThaiMoi = "Xử lý lại";
                            TrangThaiIDMoi = 102;
                        }
                    }
                    else
                    {
                        TrangThaiMoi = "Chưa xử lý";
                        TrangThaiIDMoi = 100;
                    }
                }
                else if (stateID == 5)
                {
                    TrangThaiIDMoi = 103;
                    TrangThaiMoi = "Trưởng phòng chưa duyệt";
                    if ((IdentityHelper?.RoleID ?? 0) == RoleEnum.ChuyenVien.GetHashCode())
                    {
                        TrangThaiMoi = "Đã trình";
                    }
                    if ((IdentityHelper?.RoleID ?? 0) == RoleEnum.LanhDaoPhong.GetHashCode())
                    {
                        TrangThaiMoi = "Chưa duyệt";
                    }
                }
                else if (stateID == 6)
                {
                    TrangThaiIDMoi = 104;
                    TrangThaiMoi = "Trưởng phòng đã duyệt";
                    if ((IdentityHelper?.RoleID ?? 0) == RoleEnum.LanhDaoPhong.GetHashCode())
                    {
                        TrangThaiMoi = "Đã duyệt";
                    }
                    if ((IdentityHelper?.RoleID ?? 0) == RoleEnum.LanhDao.GetHashCode())
                    {
                        TrangThaiMoi = "Chưa duyệt";
                    }
                }
                else if (stateID == 7)
                {
                    if (trangThaiDuyet == 0)
                    {
                        TrangThaiMoi = "Trưởng phòng đã duyệt";
                        TrangThaiIDMoi = 105;
                        if ((IdentityHelper?.RoleID ?? 0) == RoleEnum.LanhDaoPhong.GetHashCode())
                        {
                            TrangThaiMoi = "Đã trình";
                        }
                        if ((IdentityHelper?.RoleID ?? 0) == RoleEnum.LanhDao.GetHashCode())
                        {
                            TrangThaiMoi = "Chưa duyệt";
                        }
                    }
                    else if (trangThaiDuyet == 1)
                    {
                        TrangThaiMoi = "Lãnh đạo đã duyệt";
                        TrangThaiIDMoi = 106;
                        if ((IdentityHelper?.RoleID ?? 0) == RoleEnum.LanhDaoPhong.GetHashCode())
                        {
                            TrangThaiMoi = "Đã duyệt";
                        }
                        if ((IdentityHelper?.RoleID ?? 0) == RoleEnum.LanhDao.GetHashCode())
                        {
                            TrangThaiMoi = "Đã duyệt và chưa giao xác minh";
                            TrangThaiIDMoi = 202;
                            if (huongGiaiQuyetID != (int)HuongGiaiQuyetEnum.DeXuatThuLy)
                            {
                                TrangThaiMoi = "Đã duyệt";
                                TrangThaiIDMoi = 202;
                            }
                        }
                        
                    }
                }
                //else if (stateID == 19)
                //{
                //    TrangThaiMoi = "Đang xác minh";
                //    TrangThaiIDMoi = 203;

                //    if (IdentityHelper.RoleID == RoleEnum.LanhDaoPhong.GetHashCode())
                //    {
                //        TrangThaiMoi = "Chưa giao xác minh";
                //        TrangThaiIDMoi = 202;
                //    }

                //    //if (IdentityHelper.RoleID == RoleEnum.LanhDaoPhong.GetHashCode() && stateID == 19
                //    //// tuandhh bổ sung cấp lãnh đạo phòng chỉ dành cho btd tỉnh, btd huyện lãnh đạo phân cho ai thì cấp đó xác minh
                //    //&& IdentityHelper.CapHanhChinh != EnumCapHanhChinh.CapPhongThuocHuyen.GetHashCode())
                //    //{
                //    //    TrangThaiMoi = "Chưa giao xác minh";
                //    //    TrangThaiIDMoi = 202;
                //    //}
                //}
                else if (stateID == 8)
                {
                    //TrangThaiMoi = "Chuyên viên đang xác minh";
                    TrangThaiMoi = "Đang xác minh";
                    TrangThaiIDMoi = 204;
                    if ((IdentityHelper?.RoleID ?? 0) == RoleEnum.ChuyenVien.GetHashCode())
                    {
                        if (ngayCapNhatTheoDoiXuLy == DateTime.MinValue)
                        {
                            TrangThaiMoi = "Chưa xác minh";
                            TrangThaiIDMoi = 204;
                        }
                        else
                        {
                            TrangThaiMoi = "Đã xác minh";
                            TrangThaiIDMoi = 205;
                        }
                    }
                }
                else if (stateID == 22)
                {
                    TrangThaiMoi = "Chuyên viên đã trình báo cáo xác minh";
                    TrangThaiIDMoi = 206;
                    if (IdentityHelper.RoleID == RoleEnum.ChuyenVien.GetHashCode())
                    {
                        TrangThaiMoi = "Đã trình báo cáo xác minh";
                    }
                    if (IdentityHelper.RoleID == RoleEnum.LanhDaoPhong.GetHashCode())
                    {
                        TrangThaiMoi = "Chưa duyệt báo cáo xác minh";
                    }
                    if (IdentityHelper.RoleID == RoleEnum.LanhDao.GetHashCode())
                    {
                        TrangThaiMoi = "Chưa duyệt báo cáo xác minh";
                        TrangThaiIDMoi = 207;
                    }
                }
                else if (stateID == 9)
                {
                    TrangThaiMoi = "Chưa ban hành quyết định";
                    TrangThaiIDMoi = 209;
                    //TrangThaiMoi = "Đã trình báo cáo xác minh";
                    //TrangThaiIDMoi = 207;
                    //if (IdentityHelper.RoleID == RoleEnum.LanhDaoPhong.GetHashCode())
                    //{
                    //    TrangThaiMoi = "Đã trình báo cáo xác minh";
                    //}
                    //if (IdentityHelper.RoleID == RoleEnum.LanhDao.GetHashCode())
                    //{
                    //    TrangThaiMoi = "Chưa duyệt báo cáo xác minh";
                    //}
                }
                else if (stateID == 10)
                {
                    TrangThaiMoi = "Đang thi hành quyết định giải quyết";
                    TrangThaiIDMoi = 300;
                    if (ketQuaID != 0)
                    {
                        TrangThaiMoi = "Đã cập nhập quyết định giải quyết";
                        TrangThaiIDMoi = 301;
                    }

                    if (huongGiaiQuyetID != (int)HuongGiaiQuyetEnum.DeXuatThuLy)
                    {
                        if (huongGiaiQuyetID == (int)HuongGiaiQuyetEnum.ChuyenDon)
                        {
                            TrangThaiIDMoi = 401;
                            TrangThaiMoi = "Đã chuyển đơn";
                        }
                        else if (huongGiaiQuyetID == (int)HuongGiaiQuyetEnum.HuongDan)
                        {
                            TrangThaiIDMoi = 402;
                            TrangThaiMoi = "Đã hướng dẫn";
                        }
                        else if (huongGiaiQuyetID == (int)HuongGiaiQuyetEnum.RaVanBanDonDoc || huongGiaiQuyetID == (int)HuongGiaiQuyetEnum.CongVanChiDao)
                        {
                            TrangThaiIDMoi = 403;
                            TrangThaiMoi = "Đã đôn đốc";
                        }
                        else if (huongGiaiQuyetID == (int)HuongGiaiQuyetEnum.CongVanChiDao)
                        {
                            TrangThaiIDMoi = 404;
                            TrangThaiMoi = "Đã chỉ đạo";
                        }
                        else
                        {
                            TrangThaiIDMoi = 405;
                            TrangThaiMoi = "Đã hoàn thành";
                        }
                    }
                    // bổ sung trạng thái đã rút đơn
                    if (rutDonID != 0)
                    {
                        TrangThaiIDMoi = 406;
                        TrangThaiMoi = "Đã rút đơn";
                    }
                }
            }
            CheckTrangThai = TrangThaiIDMoi > 0 ? true : false;
        }

    }
}
