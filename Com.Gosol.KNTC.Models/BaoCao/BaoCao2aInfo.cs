﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.BaoCao
{
    public class BaoCao2aInfo
    {
        public String DonVi { get; set; }
        public bool IsCoQuan { get; set; }
        public int Col1Data { get; set; }
        public int Col2Data { get; set; }
        public int Col3Data { get; set; }
        public int Col4Data { get; set; }
        public int Col5Data { get; set; }
        public int Col6Data { get; set; }
        public int Col7Data { get; set; }
        public int Col8Data { get; set; }
        public int Col9Data { get; set; }
        public int Col10Data { get; set; }
        public int Col11Data { get; set; }
        public int Col12Data { get; set; }
        public int Col13Data { get; set; }
        public int Col14Data { get; set; }
        public int Col15Data { get; set; }
        public int Col16Data { get; set; }
        public int Col17Data { get; set; }
        public int Col18Data { get; set; }
        public int Col19Data { get; set; }
        public int Col20Data { get; set; }
        public int Col21Data { get; set; }
        public int Col22Data { get; set; }
        public int Col23Data { get; set; }
        public int Col24Data { get; set; }
        public int Col25Data { get; set; }
        public int Col26Data { get; set; }
        public int Col27Data { get; set; }
        public int Col28Data { get; set; }
        public int Col29Data { get; set; }
        public int Col30Data { get; set; }

        public String GhiChu { get; set; }
        public String CssClass { get; set; }

        public int XaID { get; set; }
        public string TenXa { get; set; }
        public string TenDayDu { get; set; }
        public int BaoCao2aID { get; set; }
        public int CapID { get; set; }
        public int CoQuanID { get; set; }
        public DateTime TuNgay { get; set; }
        public DateTime DenNgay { get; set; }
        public int CanBoID { get; set; }
        public DateTime NgayNhap { get; set; }
        public DateTime NgayChot { get; set; }
    }

    public class BaoCao2aDonThuInfo
    {
        public int DonThuID { get; set; }
        public DateTime NgayTiep { get; set; }
        public int LoaiKhieuTo1ID { get; set; }
        public int LoaiKhieuTo2ID { get; set; }
        public int LoaiKhieuTo3ID { get; set; }
        public int LoaiKhieuToID { get; set; }
        public bool GapLanhDao { get; set; }
        public DateTime NgayGapLanhDao { get; set; }
        public int CoQuanID { get; set; }
        public int SoLuong { get; set; }
        public bool VuViecCu { get; set; }
        public int CQTiepNhanID { get; set; }
        public DateTime NgayNhapDon { get; set; }

        public int HuongXuLy { get; set; }
        public int KetQuaID { get; set; }
    }
    public class BaoCao2bDonThuInfo
    {
        public int DonThuID { get; set; }
        public int KetQuaID { get; set; }
        public int LoaiKetQuaID { get; set; }
        public int HuongGiaiQuyetID { get; set; }
        public int TrangThaiDonID { get; set; }
        public int NhomKNID { get; set; }
        public DateTime NgayNhapDon { get; set; }
        public int LoaiKhieuTo1ID { get; set; }
        public int LoaiKhieuTo2ID { get; set; }
        public int LoaiKhieuTo3ID { get; set; }
        public int LoaiKhieuToID { get; set; }
        public int CoQuanID { get; set; }
        public int ThamQuyenID { get; set; }
        public int SoLuong { get; set; }
        public int SoLan { get; set; }
        public DateTime NgayRaKQ { get; set; }
        public DateTime NgayThuLy { get; set; }
        public int CQTiepNhanID { get; set; }

        public int ThamQuyenCQXuLy { get; set; }
        public int ThamQuyenCQChuyenDon { get; set; }
    }
    //public class TKDonThuInfo
    //{
    //    public int DonThuID { get; set; }

    //    public int LoaiKhieuToID { get; set; }

    //    public int LoaiKhieuTo1ID { get; set; }

    //    public int LoaiKhieuTo2ID { get; set; }

    //    public int LoaiKhieuTo3ID { get; set; }

    //    public int LoaiKetQuaID { get; set; }

    //    public int TrangThaiDonID { get; set; }

    //    public int CoQuanChuyenDonID { get; set; }

    //    public int ThamQuyenCQXuLy { get; set; }

    //    public int ThamQuyenCQChuyenDon { get; set; }

    //    public int SoLan { get; set; }

    //    public int TinhID { get; set; }

    //    public int HuyenID { get; set; }

    //    public int XaID { get; set; }

    //    public string TenTinh { get; set; }

    //    public string TenHuyen { get; set; }

    //    public string TenXa { get; set; }

    //    public int NguonDonDen { get; set; }

    //    public string TenChuDon { get; set; }

    //    public string DiaChi { get; set; }

    //    public string DiaChiPhatSinh { get; set; }

    //    public string DiaChiCTPhatSinh { get; set; }

    //    public string NoiDungDon { get; set; }

    //    public string SoDon { get; set; }

    //    public DateTime NgayNhapDon { get; set; }

    //    public DateTime? NgayCapNhat { get; set; }

    //    public string NgayNhapDonStr { get; set; }

    //    public DateTime NgayXuLy { get; set; }

    //    public DateTime NgayQuaHan { get; set; }

    //    public string NgayQuaHanStr { get; set; }

    //    public int SoLuong { get; set; }

    //    public int LanTiep { get; set; }

    //    public int XuLyDonID { get; set; }

    //    public string TenLoaiKhieuTo { get; set; }

    //    public int StateID { get; set; }

    //    public int CoQuanGiaiQuyetID { get; set; }

    //    public DateTime NgayChuyenGQ { get; set; }

    //    public string NgayChuyenGQStr { get; set; }

    //    public int HuongXuLyID { get; set; }

    //    public int KetQuaID { get; set; }

    //    public string KetQuaID_Str { get; set; }

    //    public int TiepDanKhongDonID { get; set; }

    //    public DateTime NgayTiep { get; set; }

    //    public string NgayTiepStr { get; set; }

    //    public string NoiDungTiep { get; set; }

    //    public bool VuViecCu { get; set; }

    //    public int CanBoTiepID { get; set; }

    //    public int PhongBanID { get; set; }

    //    public int CoQuanID { get; set; }

    //    public bool GapLanhDao { get; set; }

    //    public DateTime NgayGapLanhDao { get; set; }

    //    public string KetQuaTiep { get; set; }

    //    public string TenLoaiKhieuTo1 { get; set; }

    //    public string TenLoaiKhieuTo2 { get; set; }

    //    public string TenLoaiKhieuTo3 { get; set; }

    //    public string TenLanhDaoTiep { get; set; }

    //    public string TenHuongGiaiQuyet { get; set; }

    //    public int NhomKNID { get; set; }

    //    public string TenCanBo { get; set; }

    //    public string TenCoQuan { get; set; }

    //    public string TrangThaiVuViec { get; set; }

    //    public int CanBoXuLyID { get; set; }

    //    public string TenCoQuanGQ { get; set; }

    //    public int YKienGiaiQuyetID { get; set; }

    //    public int GQDChuaGQ { get; set; }

    //    public int GQDDangGQTrongHan { get; set; }

    //    public int GQDDangGQQuaHan { get; set; }

    //    public int GQDDaGQ { get; set; }

    //    public int GQDDangGQ { get; set; }

    //    public int KQKNDung { get; set; }

    //    public int KQKNDungMotPhan { get; set; }

    //    public int KQKNSai { get; set; }

    //    public int GQDKNDangGQ { get; set; }

    //    public int DaXuLy { get; set; }

    //    public int GQDTCDangGQ { get; set; }

    //    public int GQDKNPADangGQ { get; set; }

    //    public int GQDKNDaGQ { get; set; }

    //    public int GQDTCDaGQ { get; set; }

    //    public int GQDKNPADaGQ { get; set; }

    //    public int GQDKNChuaGQ { get; set; }

    //    public int GQDTCChuaGQ { get; set; }

    //    public int GQDKNPAChuaGQ { get; set; }

    //    public int RutDonID { get; set; }

    //    public string CQDaGiaiQuyet { get; set; }

    //    public DateTime NgayThiHanh { get; set; }

    //    public DateTime NgayRaKQ { get; set; }

    //    public int ThiHanhID { get; set; }

    //    public int PhanTichKQID { get; set; }

    //    public int KetQuaGQLan2 { get; set; }

    //    public int SoDoiTuongDaBiXuLy { get; set; }
    //}

    //public class FileHoSoInfo
    //{
    //    public int FileHoSoID { get; set; }

    //    public int DonDocID { get; set; }

    //    public int GiaHanGiaiQuyetID { get; set; }

    //    public string TenFile { get; set; }

    //    public string TomTat { get; set; }

    //    public DateTime NgayUp { get; set; }

    //    public string NgayUp_str { get; set; }

    //    public int NguoiUp { get; set; }

    //    public string NgayUps { get; set; }

    //    public string FileURL { get; set; }

    //    public int XuLyDonID { get; set; }

    //    public int DonThuID { get; set; }

    //    public bool IsBaoMat { get; set; }

    //    public int ChuyenGiaiQuyetID { get; set; }

    //    public int KetQuaID { get; set; }

    //    public int FileRutDonID { get; set; }

    //    public int ThiHanhID { get; set; }

    //    public string FileBase64 { get; set; }

    //    public int CanBoID { get; set; }

    //    public string TenCanBo { get; set; }

    //    public int XemTaiLieuMat { get; set; }

    //    public string TenCoQuanUp { get; set; }

    //    public bool IsMaHoa { get; set; }

    //    public int LoaiFile { get; set; }

    //    public int YKienGiaiQuyetID { get; set; }

    //    public int FilePhanXuLyID { get; set; }

    //    public string IsBaoMatString { get; set; }

    //    public int TheoDoiXuLyID { get; set; }

    //    public int DMBuocXacMinhID { get; set; }

    //    public int YKienXuLyID { get; set; }

    //    public int FileID { get; set; }

    //    public int NhomFileID { get; set; }

    //    public string TenNhomFile { get; set; }

    //    public int ThuTuHienThiNhom { get; set; }

    //    public int ThuTuHienThiFile { get; set; }

    //    public string CANBOTHEM { get; set; }

    //    public string NDFILE { get; set; }

    //    public int Type { get; set; }

    //    public DateTime? NgayCapNhat { get; set; }
    //}
}
