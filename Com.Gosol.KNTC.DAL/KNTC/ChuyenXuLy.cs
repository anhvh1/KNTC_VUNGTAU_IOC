using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Ultilities;
using DocumentFormat.OpenXml.EMMA;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.DAL.KNTC
{
    public class ChuyenXuLy
    {
        //Su dung de goi StoreProcedure

        private const string GET_ALL = @"ChuyenXuLy_GetAll";
        private const string GET_BY_ID = @"ChuyenXuLy_GetByID";
        private const string INSERT = @"ChuyenXuLy_Insert";
        private const string UPDATE = @"ChuyenXuLy_Update";
        private const string DELETE = @"ChuyenXuLy_Delete";
        private const string SEARCH = @"ChuyenXuLy_GetBySearch";
        private const string GETALL_BY_COQUANID = @"ChuyenXuLy_GetAll_ByCoQuan";
        private const string COUNT_SEARCH = @"ChuyenXuLy_CountSearch";
        private const string GET_BY_XULYDON_ID = @"ChuyenXuLy_GetByXuLyDon";
        private const string CHUYENDON_INSERT = @"ChuyenXuLy_InsertChuyenDon";

        //Danh sach don thu trang thai dang xu ly trong co quan
        private const string DS_DONTHU_DANGXULY = @"DonThu_DSDangXuLy_GetBySearch_TP_CV";
        private const string DS_DONTHU_DANGXULY_COUNT_SEARCH = @"DonThu_DSDangXuLy_CountSearch_TP_CV";
        private const string DS_DONTHU_DADUYET_XULY = @"DonThu_DSDaDuyetKetQuaXuLy";
        private const string DS_DONTHU_DADUYET_XULY_SEARCH = @"DonThu_DSDaDuyetKetQuaXuLyCountSearch";
        private const string DS_DONTHU_DADUYET_KQ_GIAIQUYET = @"DonThu_DSDaDuyetKetQuaGiaiQuyet";
        private const string DS_DONTHU_DAGIAIQUYET = @"DonThu_DSDaGiaiQuyet";
        private const string DS_DONTHU_DADUYET_KQ_GIAIQUYET_SEARCH = @"DonThu_DSDaDuyetKetQuaGiaiQuyetCountSearch";
        private const string DS_DONTHU_DA_GIAIQUYET_SEARCH = @"DonThu_DSDaGiaiQuyetCountSearch";

        private const string DT_DANGXL_LANHDAO = @"DonThu_DTDangXL_LD_GetBySearch";
        private const string DT_DANGXL_TRUONGPHONG = @"DonThu_DTDangXL_TP_GetBySearch";
        private const string DT_DANGXL_LANHDAO_COUNT_SEARCH = @"DonThu_DTDangXL_LD_CountSearch";
        private const string DT_DANGXL_TRUONGPHONG_COUNT_SEARCH = @"DonThu_DTDangXL_TP_CountSearch";

        private const string DT_DADUYETXL_CHUYENVIEN = @"DonThu_DTDaDuyetXL_CV";
        private const string DT_DADUYETXL_TRUONGPHONG = @"DonThu_DTDaDuyetXL_TP";
        private const string DT_DADUYETXL_CHUYENVIEN_COUNT_SEARCH = @"DonThu_DTDaDuyetXL_CV_CountSearch";
        private const string DT_DADUYETXL_TRUONGPHONG_COUNT_SEARCH = @"DonThu_DTDaDuyetXL_TP_CountSearch";

        //Trang default
        private const string DS_DONTHU_QUAHAN = @"DonThu_DSDonThuQuaHan";
        private const string DS_DONTHU_MOITIEPNHAN = @"DonThu_DSDonThuMoiTiepNhan";
        private const string DS_DONTHU_CAN_XULY = @"Default_DSDonThuCanXuLy";
        private const string DS_DONTHU_CAN_GIAIQUYET = @"Default_DSDonThuCanGiaiQuyet";
        private const string DS_VUVIEC_NOIBAT = @"Default_DSVuViecNoiBat";
        private const string GET_SOLANTIEP = @"Default_GetSoLanTiep";
        private const string GET_DTCANXULY_DEFAULT = @"Default_XuLyDon_DsDonCanXuLy";
        private const string GET_DTCANGIAIQUYET_DEFAULT = @"Default_XuLyDon_DsDonCanGiaiQuyet";
        private const string GET_DTMOITIEPNHAN_DEFAULT = @"Default_XuLyDon_DsDonMoiTiepNhan";
        private const string GET_DONTHU_DANG_GQ = @"Default_XuLyDon_GetDonThuDangGQ";

        //Ten cac bien dau vao
        private const string PARAM_CHUYENXULY_ID = "@ChuyenXuLyID";
        private const string PARAM_XULYDON_ID = "@XuLyDonID";
        private const string PARAM_NGAYCAPNHAT = "@NgayCapNhat";
        private const string PARAM_THEODOI_VBDI = "@TheoDoiVBDi";
        private const string PARAM_THEODOI_VBDEN = "@TheoDoiVBDen";
        private const string PARAM_FILE_DINHKEM = "@FileDinhKem";
        private const string PARAM_COQUANGUI_ID = "@CQGuiID";
        private const string PARAM_COQUANNHAN_ID = "@CQNhanID";
        private const string PARAM_NGAYCHUYEN = "@NgayChuyen";
        private const string PARAM_KEY = "@Keyword";
        private const string PARAM_START = "@Start";
        private const string PARAM_END = "@End";

        private const string PARAM_COQUAN_ID = "@CoQuanID";
        private const string PARAM_LOAIKHIEUTO_ID = "@LoaiKhieuToID";
        private const string PARAM_TUNGAY = "@TuNgay";
        private const string PARAM_DENNGAY = "@DenNgay";
        private const string PARAM_STATE_NAME = "@StateName";
        private const string PARAM_STATEID = "@StateID";
        private const string PARAM_STATE_NAME2 = "@StateName2";
        private const string PARM_PREV_STATENAME = "@PrevStateName";
        private const string PARAM_STATE_ORDER = "@StateOrder";
        private const string PARAM_NGAYHIENTAI = @"NgayHienTai";

        private const string PARAM_IS_CHUYENVIEN = "@IsChuyenVien";
        private const string PARAM_CANBO_ID = "@CanBoID";
        private const string PARAM_VAITRO = "@VaiTro";
        private const string PARAM_PHONGBAN_ID = "@PhongBanID";
        private const string PARM_TRANGTHAI = "@TrangThai";
        private const string PARM_LOAITRANHCHAP = "@LoaiTranhChap";
        private const string PARM_HUONGXULYID = "@HuongXuLyID";

        private ChuyenXuLyInfo GetData(SqlDataReader rdr)
        {
            ChuyenXuLyInfo info = new ChuyenXuLyInfo();
            info.XuLyDonID = Utils.GetInt32(rdr["XuLyDonID"], 0);
            info.DonThuID = Utils.GetInt32(rdr["DonThuID"], 0);
            info.HoTen = Utils.GetString(rdr["HoTen"], string.Empty);
            info.SoDonThu = Utils.ConvertToString(rdr["SoDonThu"], string.Empty);
            info.NoiDungDon = Utils.GetString(rdr["NoiDungDon"], String.Empty);
            if (info.NoiDungDon.Length > Constant.LengthNoiDungDon)
            {
                info.NoiDungDon = info.NoiDungDon.Substring(0, Constant.LengthNoiDungDon) + Constant.ChuoiCuoiNDDon;
            }
            info.NgayChuyen = Utils.GetDateTime(rdr["NgayChuyen"], DateTime.MinValue);
            info.NgayChuyens = "";
            if (info.NgayChuyen != DateTime.MinValue)
            {
                info.NgayChuyens = info.NgayChuyen.ToString("dd/MM/yyyy");
            }
            info.NgayCapNhat = Utils.GetDateTime(rdr["NgayCapNhat"], DateTime.MinValue);
            info.NgayCapNhats = "";
            if (info.NgayCapNhat != DateTime.MinValue)
            {
                info.NgayCapNhats = info.NgayCapNhat.ToString("dd/MM/yyyy");
            }
            info.NguonDonDen = Utils.GetInt32(rdr["NguonDonDen"], 0);
            info.NguonDonDens = "";
            if (info.NguonDonDen == (int)EnumNguonDonDen.TrucTiep)
            {
                info.NguonDonDens = Constant.NguonDon_TrucTieps;
            }
            if (info.NguonDonDen == (int)EnumNguonDonDen.CoQuanKhac)
            {
                info.NguonDonDens = Constant.NguonDon_CoQuanKhacs;
            }
            if (info.NguonDonDen == (int)EnumNguonDonDen.BuuChinh)
            {
                info.NguonDonDens = Constant.NguonDon_BuuChinhs;
            }
            info.TenCoQuanNhan = Utils.GetString(rdr["TenCoQuanNhan"], string.Empty);
            info.TenLoaiKhieuTo = Utils.GetString(rdr["TenLoaiKhieuTo"], string.Empty);

            return info;
        }

        private ChuyenXuLyInfo GetDataByXuLyDon(SqlDataReader dr)
        {
            ChuyenXuLyInfo cInfo = new ChuyenXuLyInfo();
            cInfo.ChuyenXuLyID = Utils.ConvertToInt32(dr["ChuyenXuLyID"].ToString(), 0);
            cInfo.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"].ToString(), 0);
            cInfo.NgayCapNhat = Utils.ConvertToDateTime(dr["NgayCapNhat"].ToString(), DateTime.MinValue);
            cInfo.TheoDoiVBDi = Utils.ConvertToString(dr["TheoDoiVBDen"].ToString(), String.Empty);
            cInfo.TheoDoiVBDen = Utils.ConvertToString(dr["TheoDoiVBDen"].ToString(), String.Empty);
            cInfo.FileDinhKem = Utils.ConvertToString(dr["FileDinhKem"].ToString(), String.Empty);

            return cInfo;
        }

        private ChuyenXuLyInfo GetDataDSDonThuDangXuLy(SqlDataReader dr)
        {
            ChuyenXuLyInfo cInfo = new ChuyenXuLyInfo();
            cInfo.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"].ToString(), 0);
            cInfo.DonThuID = Utils.ConvertToInt32(dr["DonThuID"].ToString(), 0);
            cInfo.SoDonThu = Utils.ConvertToString(dr["SoDonThu"], string.Empty);
            cInfo.NguonDonDen = Utils.ConvertToInt32(dr["NguonDonDen"].ToString(), 0);
            cInfo.NoiDungDon = Utils.ConvertToString(dr["NoiDungDon"].ToString(), String.Empty);
            if (cInfo.NoiDungDon.Length > Constant.LengthNoiDungDon)
            {
                cInfo.NoiDungDon = cInfo.NoiDungDon.Substring(0, Constant.LengthNoiDungDon) + Constant.ChuoiCuoiNDDon;
            }
            cInfo.TenLoaiKhieuTo = Utils.ConvertToString(dr["TenLoaiKhieuTo"].ToString(), String.Empty);
            cInfo.HoTen = Utils.ConvertToString(dr["HoTen"].ToString(), String.Empty);
            cInfo.NgayPhan = Utils.ConvertToDateTime(dr["NgayPhan"].ToString(), DateTime.MinValue);
            cInfo.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"].ToString(), 0);
            cInfo.NgayHetHan = Utils.ConvertToDateTime(dr["NgayHetHan"].ToString(), DateTime.MinValue);
            cInfo.TenCanBoXuLy = Utils.ConvertToString(dr["TenCanBoXuLy"].ToString(), String.Empty);

            return cInfo;
        }

        private ChuyenXuLyInfo GetDataDSDonThuDaDuyetXuLy(SqlDataReader dr)
        {
            ChuyenXuLyInfo cInfo = new ChuyenXuLyInfo();
            cInfo.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"].ToString(), 0);
            cInfo.DonThuID = Utils.ConvertToInt32(dr["DonThuID"].ToString(), 0);
            cInfo.SoDonThu = Utils.ConvertToString(dr["SoDonThu"], string.Empty);
            cInfo.NguonDonDen = Utils.ConvertToInt32(dr["NguonDonDen"].ToString(), 0);
            cInfo.NoiDungDon = Utils.ConvertToString(dr["NoiDungDon"].ToString(), String.Empty);
            if (cInfo.NoiDungDon.Length > Constant.LengthNoiDungDon)
            {
                cInfo.NoiDungDon = cInfo.NoiDungDon.Substring(0, Constant.LengthNoiDungDon) + Constant.ChuoiCuoiNDDon;
            }
            cInfo.TenLoaiKhieuTo = Utils.ConvertToString(dr["TenLoaiKhieuTo"].ToString(), String.Empty);
            cInfo.HoTen = Utils.ConvertToString(dr["HoTen"].ToString(), String.Empty);
            cInfo.NgayPhan = Utils.ConvertToDateTime(dr["ModifiedDate"].ToString(), DateTime.MinValue);
            cInfo.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"].ToString(), 0);
            cInfo.NgayHetHan = Utils.ConvertToDateTime(dr["NgayHetHan"].ToString(), DateTime.MinValue);
            cInfo.TenCanBoXuLy = Utils.ConvertToString(dr["TenCanBo"].ToString(), String.Empty);

            return cInfo;
        }

        private ChuyenXuLyInfo GetDataDSDonThuDaDuyetKQGiaiQuyet(SqlDataReader dr)
        {
            ChuyenXuLyInfo cInfo = new ChuyenXuLyInfo();
            cInfo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
            cInfo.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"].ToString(), 0);
            cInfo.DonThuID = Utils.ConvertToInt32(dr["DonThuID"].ToString(), 0);
            cInfo.SoDonThu = Utils.ConvertToString(dr["SoDonThu"], string.Empty);
            cInfo.NguonDonDen = Utils.ConvertToInt32(dr["NguonDonDen"].ToString(), 0);
            cInfo.NoiDungDon = Utils.ConvertToString(dr["NoiDungDon"].ToString(), String.Empty);
            if (cInfo.NoiDungDon.Length > Constant.LengthNoiDungDon)
            {
                cInfo.NoiDungDon = cInfo.NoiDungDon.Substring(0, Constant.LengthNoiDungDon) + Constant.ChuoiCuoiNDDon;
            }
            cInfo.TenLoaiKhieuTo = Utils.ConvertToString(dr["TenLoaiKhieuTo"].ToString(), String.Empty);
            cInfo.HoTen = Utils.ConvertToString(dr["HoTen"].ToString(), String.Empty);
            cInfo.NgayPhan = Utils.ConvertToDateTime(dr["ModifiedDate"].ToString(), DateTime.MinValue);
            cInfo.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"].ToString(), 0);
            cInfo.NgayHetHan = Utils.ConvertToDateTime(dr["HanGiaiQuyetNew"].ToString(), DateTime.MinValue);
            cInfo.TenCanBoXuLy = Utils.ConvertToString(dr["TenCanBo"].ToString(), String.Empty);
            cInfo.KetQuaID = Utils.ConvertToString(dr["KetQuaID"], string.Empty);

            return cInfo;
        }

        private ChuyenXuLyInfo GetDataDSDonThuDaDuyetKQGiaiQuyet_New(SqlDataReader dr)
        {
            ChuyenXuLyInfo cInfo = new ChuyenXuLyInfo();
            cInfo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
            cInfo.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"].ToString(), 0);
            cInfo.DonThuID = Utils.ConvertToInt32(dr["DonThuID"].ToString(), 0);
            cInfo.SoDonThu = Utils.ConvertToString(dr["SoDonThu"], string.Empty);
            cInfo.NguonDonDen = Utils.ConvertToInt32(dr["NguonDonDen"].ToString(), 0);
            cInfo.NoiDungDon = Utils.ConvertToString(dr["NoiDungDon"].ToString(), String.Empty);
            if (cInfo.NoiDungDon.Length > Constant.LengthNoiDungDon)
            {
                cInfo.NoiDungDon = cInfo.NoiDungDon.Substring(0, Constant.LengthNoiDungDon) + Constant.ChuoiCuoiNDDon;
            }
            cInfo.TenLoaiKhieuTo = Utils.ConvertToString(dr["TenLoaiKhieuTo"].ToString(), String.Empty);
            cInfo.HoTen = Utils.ConvertToString(dr["HoTen"].ToString(), String.Empty);
            cInfo.NgayPhan = Utils.ConvertToDateTime(dr["ModifiedDate"].ToString(), DateTime.MinValue);
            cInfo.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"].ToString(), 0);
            cInfo.NgayHetHan = Utils.ConvertToDateTime(dr["HanGiaiQuyetNew"].ToString(), DateTime.MinValue);
            cInfo.TenCanBoXuLy = Utils.ConvertToString(dr["TenCanBo"].ToString(), String.Empty);

            cInfo.KetQuaID = Utils.ConvertToString(dr["KetQuaID"], string.Empty);
            cInfo.TenCoQuanGiaiQuyet = Utils.ConvertToString(dr["TenCoQuanGiaiQuyet"].ToString(), String.Empty);
            cInfo.TenTrangThai = Utils.ConvertToString(dr["TenTrangThai"].ToString(), String.Empty);
            cInfo.TrangThai = Utils.ConvertToInt32(dr["TrangThai"].ToString(), 0);
            return cInfo;
        }

        private ChuyenXuLyInfo GetDataDSDonThuQuaHan(SqlDataReader dr)
        {
            ChuyenXuLyInfo cInfo = new ChuyenXuLyInfo();
            cInfo.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"].ToString(), 0);
            cInfo.DonThuID = Utils.ConvertToInt32(dr["DonThuID"].ToString(), 0);
            cInfo.SoDonThu = Utils.ConvertToString(dr["SoDonThu"], string.Empty);
            cInfo.NguonDonDen = Utils.ConvertToInt32(dr["NguonDonDen"], 0);
            cInfo.NoiDungDon = Utils.ConvertToString(dr["NoiDungDon"], String.Empty);
            cInfo.TenLoaiKhieuTo = Utils.ConvertToString(dr["TenLoaiKhieuTo"], String.Empty);
            cInfo.HoTen = Utils.ConvertToString(dr["HoTen"], String.Empty);
            cInfo.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"], 0);
            cInfo.TenCanBoXuLy = Utils.ConvertToString(dr["TenCanBoXuLy"], String.Empty);
            cInfo.CQGiaoID = Utils.ConvertToInt32(dr["CQGiaoID"].ToString(), 0);
            cInfo.HanGQGoc = Utils.ConvertToDateTime(dr["HanGQGoc"], DateTime.MinValue);
            cInfo.HanGQMoi = Utils.ConvertToDateTime(dr["HanGQMoi"], DateTime.MinValue);

            return cInfo;
        }

        private ChuyenXuLyInfo GetDataDSDonThuMoiTiepNhan(SqlDataReader dr)
        {
            ChuyenXuLyInfo cInfo = new ChuyenXuLyInfo();
            cInfo.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"].ToString(), 0);
            cInfo.DonThuID = Utils.ConvertToInt32(dr["DonThuID"].ToString(), 0);
            cInfo.SoDonThu = Utils.ConvertToString(dr["SoDonThu"], string.Empty);
            cInfo.NguonDonDen = Utils.ConvertToInt32(dr["NguonDonDen"].ToString(), 0);
            cInfo.NoiDungDon = Utils.ConvertToString(dr["NoiDungDon"].ToString(), String.Empty);
            cInfo.TenLoaiKhieuTo = Utils.ConvertToString(dr["TenLoaiKhieuTo"].ToString(), String.Empty);
            cInfo.HoTen = Utils.ConvertToString(dr["HoTen"].ToString(), String.Empty);
            cInfo.NgayPhan = Utils.ConvertToDateTime(dr["NgayPhan"].ToString(), DateTime.MinValue);
            cInfo.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"].ToString(), 0);
            cInfo.NgayHetHan = Utils.ConvertToDateTime(dr["NgayHetHan"].ToString(), DateTime.MinValue);
            cInfo.TenCanBoXuLy = Utils.ConvertToString(dr["TenCanBoXuLy"].ToString(), String.Empty);

            return cInfo;
        }


        private ChuyenXuLyInfo GetDataDSDonThuCanXLCanGQ(SqlDataReader dr)
        {
            ChuyenXuLyInfo cInfo = new ChuyenXuLyInfo();
            cInfo.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"].ToString(), 0);
            cInfo.DonThuID = Utils.ConvertToInt32(dr["DonThuID"].ToString(), 0);
            cInfo.SoDonThu = Utils.ConvertToString(dr["SoDonThu"], string.Empty);
            cInfo.NguonDonDen = Utils.ConvertToInt32(dr["NguonDonDen"].ToString(), 0);
            cInfo.NoiDungDon = Utils.ConvertToString(dr["NoiDungDon"].ToString(), String.Empty);
            cInfo.TenLoaiKhieuTo = Utils.ConvertToString(dr["TenLoaiKhieuTo"].ToString(), String.Empty);
            cInfo.HoTen = Utils.ConvertToString(dr["HoTen"].ToString(), String.Empty);
            cInfo.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"].ToString(), 0);
            cInfo.NgayHetHan = Utils.ConvertToDateTime(dr["NgayHetHan"].ToString(), DateTime.MinValue);
            cInfo.TenCanBoXuLy = Utils.ConvertToString(dr["TenCanBoXuLy"].ToString(), String.Empty);

            return cInfo;
        }

        private SqlParameter[] GetInsertParms()
        {
            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int),
                new SqlParameter(PARAM_COQUANGUI_ID, SqlDbType.Int),
                new SqlParameter(PARAM_COQUANNHAN_ID, SqlDbType.Int),
                new SqlParameter(PARAM_NGAYCHUYEN, SqlDbType.DateTime),
                new SqlParameter(PARAM_FILE_DINHKEM, SqlDbType.NVarChar,2000)
            };
            return parms;
        }

        private void SetInsertParms(SqlParameter[] parms, ChuyenXuLyInfo cInfo)
        {

            parms[0].Value = cInfo.XuLyDonID;
            parms[1].Value = cInfo.CQGuiID;
            parms[2].Value = cInfo.CQNhanID;
            parms[3].Value = cInfo.NgayChuyen;
            parms[4].Value = string.IsNullOrEmpty(cInfo.FileDinhKem) ? DBNull.Value : cInfo.FileDinhKem;

            if (cInfo.CQNhanID == 0) parms[2].Value = DBNull.Value;
        }

        private SqlParameter[] GetUpdateParms()
        {
            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter(PARAM_CHUYENXULY_ID, SqlDbType.Int),
                new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int),
                new SqlParameter(PARAM_NGAYCAPNHAT,SqlDbType.DateTime),
                new SqlParameter(PARAM_THEODOI_VBDI,SqlDbType.NVarChar,500),
                new SqlParameter(PARAM_THEODOI_VBDEN,SqlDbType.NVarChar,500),
                new SqlParameter(PARAM_FILE_DINHKEM,SqlDbType.NVarChar,2000)
            };
            return parms;
        }

        private void SetUpdateParms(SqlParameter[] parms, ChuyenXuLyInfo cInfo)
        {

            parms[0].Value = cInfo.ChuyenXuLyID;
            parms[1].Value = cInfo.XuLyDonID;
            parms[2].Value = cInfo.NgayCapNhat;
            parms[3].Value = cInfo.TheoDoiVBDi;
            parms[4].Value = cInfo.TheoDoiVBDen;
            parms[5].Value = cInfo.FileDinhKem;
        }

        public IList<ChuyenXuLyInfo> GetAll()
        {
            IList<ChuyenXuLyInfo> ChuyenXuLys = new List<ChuyenXuLyInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_ALL, null))
                {

                    while (dr.Read())
                    {

                        ChuyenXuLyInfo cInfo = GetData(dr);
                        ChuyenXuLys.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return ChuyenXuLys;
        }

        public ChuyenXuLyInfo GetByID(int cID)
        {

            ChuyenXuLyInfo cInfo = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_CHUYENXULY_ID,SqlDbType.Int)
            };
            parameters[0].Value = cID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_ID, parameters))
                {

                    if (dr.Read())
                    {
                        cInfo = GetData(dr);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return cInfo;
        }

        public ChuyenXuLyInfo GetByXuLyDon(int xulydonID)
        {
            ChuyenXuLyInfo cInfo = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDON_ID,SqlDbType.Int)
            };
            parameters[0].Value = xulydonID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_XULYDON_ID, parameters))
                {

                    if (dr.Read())
                    {
                        cInfo = GetDataByXuLyDon(dr);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return cInfo;
        }

        public int Delete(int cID)
        {

            int val = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDON_ID,SqlDbType.Int)
            };
            parameters[0].Value = cID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, DELETE, parameters);
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
                conn.Close();
            }

            return val;
        }

        public int Update(ChuyenXuLyInfo cInfo)
        {

            int val = 0;
            SqlParameter[] parameters = GetUpdateParms();
            SetUpdateParms(parameters, cInfo);

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, UPDATE, parameters);
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
                conn.Close();
            }
            return val;
        }

        public int Insert(ChuyenXuLyInfo cInfo)
        {
            int val = 0;

            SqlParameter[] parameters = GetInsertParms();
            SetInsertParms(parameters, cInfo);

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, INSERT, parameters);
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
                conn.Close();
            }
            return val;
        }

        public int ChuyenDonInsert(int XuLyDonID, DateTime NgayChuyen)
        {

            int val = 0;

            SqlParameter[] parameters = new SqlParameter[]{
            new SqlParameter(PARAM_XULYDON_ID,SqlDbType.Int),
            new SqlParameter(PARAM_NGAYCHUYEN,SqlDbType.DateTime)
            };
            parameters[0].Value = XuLyDonID;
            parameters[1].Value = NgayChuyen;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, CHUYENDON_INSERT, parameters);
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
                conn.Close();
            }
            return val;
        }

        public IList<ChuyenXuLyInfo> GetBySearch(int start, int end, QueryFilterInfo querryInfo)
        {

            IList<ChuyenXuLyInfo> ChuyenXuLys = new List<ChuyenXuLyInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_COQUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_KEY,SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTO_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_START,SqlDbType.Int),
                new SqlParameter(PARAM_END,SqlDbType.Int),
                new SqlParameter(PARAM_STATE_NAME,SqlDbType.NVarChar,100),
            };

            parameters[0].Value = querryInfo.CoQuanID;
            parameters[1].Value = querryInfo.KeyWord;
            parameters[2].Value = querryInfo.LoaiKhieuToID;
            parameters[3].Value = querryInfo.TuNgay;
            parameters[4].Value = querryInfo.DenNgay;
            parameters[5].Value = start;
            parameters[6].Value = end;
            parameters[7].Value = querryInfo.StateName;

            if (querryInfo.TuNgay == DateTime.MinValue) parameters[3].Value = DBNull.Value;
            if (querryInfo.DenNgay == DateTime.MinValue) parameters[4].Value = DBNull.Value;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, SEARCH, parameters))
                {
                    while (dr.Read())
                    {
                        ChuyenXuLyInfo cInfo = GetData(dr);
                        ChuyenXuLys.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return ChuyenXuLys;
        }

        public IList<ChuyenXuLyInfo> GetAllByCoQuan(int start, int end, QueryFilterInfo querryInfo)
        {

            IList<ChuyenXuLyInfo> ChuyenXuLys = new List<ChuyenXuLyInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_COQUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_KEY,SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTO_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_START,SqlDbType.Int),
                new SqlParameter(PARAM_END,SqlDbType.Int),
                new SqlParameter(PARAM_STATE_NAME,SqlDbType.NVarChar,100),
            };

            parameters[0].Value = querryInfo.CoQuanID;
            parameters[1].Value = querryInfo.KeyWord;
            parameters[2].Value = querryInfo.LoaiKhieuToID;
            parameters[3].Value = querryInfo.TuNgay;
            parameters[4].Value = querryInfo.DenNgay;
            parameters[5].Value = start;
            parameters[6].Value = end;
            parameters[7].Value = querryInfo.StateName;

            if (querryInfo.TuNgay == DateTime.MinValue) parameters[3].Value = DBNull.Value;
            if (querryInfo.DenNgay == DateTime.MinValue) parameters[4].Value = DBNull.Value;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GETALL_BY_COQUANID, parameters))
                {
                    while (dr.Read())
                    {
                        ChuyenXuLyInfo cInfo = GetData(dr);
                        ChuyenXuLys.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return ChuyenXuLys;
        }

        public int CountSearch(QueryFilterInfo querryInfo)
        {

            int result = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_COQUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_KEY,SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTO_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_STATE_NAME,SqlDbType.NVarChar,100)
            };

            parameters[0].Value = querryInfo.CoQuanID;
            parameters[1].Value = querryInfo.KeyWord;
            parameters[2].Value = querryInfo.LoaiKhieuToID;
            parameters[3].Value = querryInfo.TuNgay;
            parameters[4].Value = querryInfo.DenNgay;
            parameters[5].Value = querryInfo.StateName;

            if (querryInfo.TuNgay == DateTime.MinValue) parameters[3].Value = DBNull.Value;
            if (querryInfo.DenNgay == DateTime.MinValue) parameters[4].Value = DBNull.Value;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_SEARCH, parameters))
                {

                    if (dr.Read())
                    {
                        result = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return result;
        }
        public IList<ChuyenXuLyInfo> GetDSDonThuDangXuLy(QueryFilterInfo info, int page)
        {
            int start = (page - 1) * Constant.PageSize;
            int end = page * Constant.PageSize;

            IList<ChuyenXuLyInfo> dsDonThu = new List<ChuyenXuLyInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_COQUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_KEY,SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTO_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_START,SqlDbType.Int),
                new SqlParameter(PARAM_END,SqlDbType.Int),
                new SqlParameter(PARAM_STATE_NAME,SqlDbType.NVarChar,100),
                new SqlParameter(PARAM_IS_CHUYENVIEN,SqlDbType.Bit),
                new SqlParameter(PARAM_CANBO_ID,SqlDbType.Int)
            };

            parameters[0].Value = info.CoQuanID;
            parameters[1].Value = info.KeyWord;
            parameters[2].Value = info.LoaiKhieuToID;
            parameters[3].Value = info.TuNgay;
            parameters[4].Value = info.DenNgay;
            parameters[5].Value = start;
            parameters[6].Value = end;
            parameters[7].Value = info.StateName;
            parameters[8].Value = info.IsLanhDao;
            parameters[9].Value = info.CanBoID;


            if ((info.TuNgay) == DateTime.MinValue)
            {
                parameters[3].Value = DBNull.Value;
            }

            if ((info.DenNgay) == DateTime.MinValue)
            {
                parameters[4].Value = DBNull.Value;
            }

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, DS_DONTHU_DANGXULY, parameters))
                {
                    while (dr.Read())
                    {
                        ChuyenXuLyInfo cInfo = GetDataDSDonThuDangXuLy(dr);
                        dsDonThu.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return dsDonThu;
        }

        public int CountSearchDSDangXuLy(QueryFilterInfo info)
        {

            int result = 0;

            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_COQUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_KEY,SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTO_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_STATE_NAME,SqlDbType.NVarChar,100),
                new SqlParameter(PARAM_IS_CHUYENVIEN,SqlDbType.Bit),
                new SqlParameter(PARAM_CANBO_ID,SqlDbType.Int)
            };

            parameters[0].Value = info.CoQuanID;
            parameters[1].Value = info.KeyWord;
            parameters[2].Value = info.LoaiKhieuToID;
            parameters[3].Value = info.TuNgay;
            parameters[4].Value = info.DenNgay;
            parameters[5].Value = info.StateName;
            parameters[6].Value = info.IsChuyenVien;
            parameters[7].Value = info.CanBoID;

            if ((info.TuNgay) == DateTime.MinValue)
            {
                parameters[3].Value = DBNull.Value;
            }

            if ((info.DenNgay) == DateTime.MinValue)
            {
                parameters[4].Value = DBNull.Value;
            }
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, DS_DONTHU_DANGXULY_COUNT_SEARCH, parameters))
                {

                    if (dr.Read())
                    {
                        result = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return result;
        }

        public IList<ChuyenXuLyInfo> GetDTDangXL(QueryFilterInfo info, int start, int end)
        {

            IList<ChuyenXuLyInfo> dsDonThu = new List<ChuyenXuLyInfo>();
            SqlParameter[] parameters;
            if (info.IsLanhDao == true)
            {
                parameters = new SqlParameter[]{
                new SqlParameter(PARAM_COQUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_KEY,SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTO_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_START,SqlDbType.Int),
                new SqlParameter(PARAM_END,SqlDbType.Int),
                new SqlParameter(PARAM_STATE_NAME,SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_STATE_NAME2,SqlDbType.NVarChar,50),
            };

                parameters[0].Value = info.CoQuanID;
                parameters[1].Value = info.KeyWord;
                parameters[2].Value = info.LoaiKhieuToID;
                parameters[3].Value = info.TuNgay;
                parameters[4].Value = info.DenNgay;
                parameters[5].Value = start;
                parameters[6].Value = end;
                parameters[7].Value = info.StateName;
                parameters[8].Value = info.StateName2;


                if (info.TuNgay == DateTime.MinValue)
                    parameters[3].Value = DBNull.Value;

                if (info.DenNgay == DateTime.MinValue)
                    parameters[4].Value = DBNull.Value;

            }
            else
            {
                parameters = new SqlParameter[]{
                new SqlParameter(PARAM_COQUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_KEY,SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTO_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_START,SqlDbType.Int),
                new SqlParameter(PARAM_END,SqlDbType.Int),
                new SqlParameter(PARAM_STATE_NAME,SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_PHONGBAN_ID,SqlDbType.Int)
            };

                parameters[0].Value = info.CoQuanID;
                parameters[1].Value = info.KeyWord;
                parameters[2].Value = info.LoaiKhieuToID;
                parameters[3].Value = info.TuNgay;
                parameters[4].Value = info.DenNgay;
                parameters[5].Value = start;
                parameters[6].Value = end;
                parameters[7].Value = info.StateName;
                parameters[8].Value = info.PhongBanID;

                if (info.TuNgay == DateTime.MinValue)
                    parameters[3].Value = DBNull.Value;

                if (info.DenNgay == DateTime.MinValue)
                    parameters[4].Value = DBNull.Value;

            }






            try
            {
                if (info.IsLanhDao == true)
                    using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, DT_DANGXL_LANHDAO, parameters))
                    {
                        while (dr.Read())
                        {
                            ChuyenXuLyInfo cInfo = GetDataDSDonThuDangXuLy(dr);
                            dsDonThu.Add(cInfo);
                        }
                        dr.Close();
                    }
                else
                    using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, DT_DANGXL_TRUONGPHONG, parameters))
                    {
                        while (dr.Read())
                        {
                            ChuyenXuLyInfo cInfo = GetDataDSDonThuDangXuLy(dr);
                            dsDonThu.Add(cInfo);
                        }
                        dr.Close();
                    }
            }
            catch
            {

                throw;
            }

            return dsDonThu;
        }

        public int CountDTDangXL(QueryFilterInfo info)
        {
            int result = 0;
            SqlParameter[] parameters;
            if (info.IsLanhDao == true)
            {
                parameters = new SqlParameter[]{
                new SqlParameter(PARAM_COQUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_KEY,SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTO_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_STATE_NAME,SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_STATE_NAME2,SqlDbType.NVarChar,50),
            };

                parameters[0].Value = info.CoQuanID;
                parameters[1].Value = info.KeyWord;
                parameters[2].Value = info.LoaiKhieuToID;
                parameters[3].Value = info.TuNgay;
                parameters[4].Value = info.DenNgay;
                parameters[5].Value = info.StateName;
                parameters[6].Value = info.StateName2;

                if (info.TuNgay == DateTime.MinValue)
                    parameters[3].Value = DBNull.Value;

                if (info.DenNgay == DateTime.MinValue)
                    parameters[4].Value = DBNull.Value;

            }
            else
            {
                parameters = new SqlParameter[]{
                new SqlParameter(PARAM_COQUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_KEY,SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTO_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_STATE_NAME,SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_PHONGBAN_ID,SqlDbType.Int)
            };

                parameters[0].Value = info.CoQuanID;
                parameters[1].Value = info.KeyWord;
                parameters[2].Value = info.LoaiKhieuToID;
                parameters[3].Value = info.TuNgay;
                parameters[4].Value = info.DenNgay;
                parameters[5].Value = info.StateName;
                parameters[6].Value = info.PhongBanID;

                if (info.TuNgay == DateTime.MinValue)
                    parameters[3].Value = DBNull.Value;

                if (info.DenNgay == DateTime.MinValue)
                    parameters[4].Value = DBNull.Value;

            }






            try
            {
                if (info.IsLanhDao == true)
                    using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, DT_DANGXL_LANHDAO_COUNT_SEARCH, parameters))
                    {
                        if (dr.Read())
                        {
                            result = Utils.ConvertToInt32(dr["CountNum"], 0);
                        }
                        dr.Close();
                    }
                else
                    using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, DT_DANGXL_TRUONGPHONG_COUNT_SEARCH, parameters))
                    {
                        if (dr.Read())
                        {
                            result = Utils.ConvertToInt32(dr["CountNum"], 0);
                        }
                        dr.Close();
                    }
            }
            catch
            {

                throw;
            }
            return result;
        }

        public IList<ChuyenXuLyInfo> GetDSDonThuDaDuyetXuLy(QueryFilterInfo info, int page)
        {
            int start = (page - 1) * Constant.PageSize;
            int end = page * Constant.PageSize;

            IList<ChuyenXuLyInfo> dsDonThu = new List<ChuyenXuLyInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_COQUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_KEY,SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTO_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_START,SqlDbType.Int),
                new SqlParameter(PARAM_END,SqlDbType.Int),
                new SqlParameter(PARAM_STATE_NAME,SqlDbType.NVarChar,100),
                new SqlParameter(PARM_PREV_STATENAME,SqlDbType.NVarChar,100),
                new SqlParameter(PARAM_CANBO_ID,SqlDbType.Int),
            };
            parameters[0].Value = info.CoQuanID;
            parameters[1].Value = info.KeyWord;
            parameters[2].Value = info.LoaiKhieuToID;
            parameters[3].Value = info.TuNgay;
            parameters[4].Value = info.DenNgay;
            parameters[5].Value = start;
            parameters[6].Value = end;
            parameters[7].Value = info.StateName;
            parameters[8].Value = info.PrevStateName;
            parameters[9].Value = info.CanBoID;

            if ((info.TuNgay) == DateTime.MinValue)
            {
                parameters[3].Value = DBNull.Value;
            }

            if ((info.DenNgay) == DateTime.MinValue)
            {
                parameters[4].Value = DBNull.Value;
            }

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, DS_DONTHU_DADUYET_XULY, parameters))
                {
                    while (dr.Read())
                    {
                        ChuyenXuLyInfo cInfo = GetDataDSDonThuDaDuyetXuLy(dr);
                        dsDonThu.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return dsDonThu;
        }

        public int CountSearchDSDaDuyetXuLy(QueryFilterInfo info)
        {

            int result = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_COQUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_KEY,SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTO_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_STATE_NAME,SqlDbType.NVarChar,100),
                new SqlParameter(PARM_PREV_STATENAME,SqlDbType.NVarChar,100),
                new SqlParameter(PARAM_CANBO_ID,SqlDbType.Int),
            };
            parameters[0].Value = info.CoQuanID;
            parameters[1].Value = info.KeyWord;
            parameters[2].Value = info.LoaiKhieuToID;
            parameters[3].Value = info.TuNgay;
            parameters[4].Value = info.DenNgay;
            parameters[5].Value = info.StateName;
            parameters[6].Value = info.PrevStateName;
            parameters[7].Value = info.CanBoID;

            if ((info.TuNgay) == DateTime.MinValue)
            {
                parameters[3].Value = DBNull.Value;
            }

            if ((info.DenNgay) == DateTime.MinValue)
            {
                parameters[4].Value = DBNull.Value;
            }
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, DS_DONTHU_DADUYET_XULY_SEARCH, parameters))
                {

                    if (dr.Read())
                    {
                        result = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return result;
        }

        public IList<ChuyenXuLyInfo> GetDTDaDuyetXL(QueryFilterInfo info, int start, int end)
        {

            IList<ChuyenXuLyInfo> dsDonThu = new List<ChuyenXuLyInfo>();
            SqlParameter[] parameters;
            if (info.IsTruongPhong == true)
            {

                parameters = new SqlParameter[]{
                new SqlParameter(PARAM_COQUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_KEY,SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTO_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_START,SqlDbType.Int),
                new SqlParameter(PARAM_END,SqlDbType.Int),
                new SqlParameter(PARAM_STATE_NAME,SqlDbType.NVarChar,100),
                new SqlParameter(PARM_PREV_STATENAME,SqlDbType.NVarChar,100),
                new SqlParameter(PARAM_PHONGBAN_ID,SqlDbType.Int),
            };
                parameters[0].Value = info.CoQuanID;
                parameters[1].Value = info.KeyWord;
                parameters[2].Value = info.LoaiKhieuToID;
                parameters[3].Value = info.TuNgay;
                parameters[4].Value = info.DenNgay;
                parameters[5].Value = start;
                parameters[6].Value = end;
                parameters[7].Value = info.StateName;
                parameters[8].Value = info.PrevStateName;
                parameters[9].Value = info.PhongBanID;

                if ((info.TuNgay) == DateTime.MinValue)
                    parameters[3].Value = DBNull.Value;
                if ((info.DenNgay) == DateTime.MinValue)
                    parameters[4].Value = DBNull.Value;

            }
            else
            {
                parameters = new SqlParameter[]{
                new SqlParameter(PARAM_COQUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_KEY,SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTO_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_START,SqlDbType.Int),
                new SqlParameter(PARAM_END,SqlDbType.Int),
                new SqlParameter(PARAM_STATE_NAME,SqlDbType.NVarChar,100),
                new SqlParameter(PARM_PREV_STATENAME,SqlDbType.NVarChar,100),
                new SqlParameter(PARAM_CANBO_ID,SqlDbType.Int),
            };
                parameters[0].Value = info.CoQuanID;
                parameters[1].Value = info.KeyWord;
                parameters[2].Value = info.LoaiKhieuToID;
                parameters[3].Value = info.TuNgay;
                parameters[4].Value = info.DenNgay;
                parameters[5].Value = start;
                parameters[6].Value = end;
                parameters[7].Value = info.StateName;
                parameters[8].Value = info.PrevStateName;
                parameters[9].Value = info.CanBoID;

                if ((info.TuNgay) == DateTime.MinValue)
                    parameters[3].Value = DBNull.Value;
                if ((info.DenNgay) == DateTime.MinValue)
                    parameters[4].Value = DBNull.Value;

            }

            try
            {
                if (info.IsTruongPhong == true)
                    using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, DT_DADUYETXL_TRUONGPHONG, parameters))
                    {
                        while (dr.Read())
                        {
                            ChuyenXuLyInfo cInfo = GetDataDSDonThuDaDuyetXuLy(dr);
                            dsDonThu.Add(cInfo);
                        }
                        dr.Close();
                    }
                else
                    using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, DT_DADUYETXL_CHUYENVIEN, parameters))
                    {
                        while (dr.Read())
                        {
                            ChuyenXuLyInfo cInfo = GetDataDSDonThuDaDuyetXuLy(dr);
                            dsDonThu.Add(cInfo);
                        }
                        dr.Close();
                    }
            }
            catch
            {

                throw;
            }
            return dsDonThu;
        }

        public int CountSearchDTDaDuyetXL(QueryFilterInfo info)
        {

            int result = 0;
            SqlParameter[] parameters;
            if (info.IsTruongPhong == true)
            {
                parameters = new SqlParameter[]{
                new SqlParameter(PARAM_COQUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_KEY,SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTO_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_STATE_NAME,SqlDbType.NVarChar,100),
                new SqlParameter(PARM_PREV_STATENAME,SqlDbType.NVarChar,100),
                new SqlParameter(PARAM_PHONGBAN_ID,SqlDbType.Int),
            };
                parameters[0].Value = info.CoQuanID;
                parameters[1].Value = info.KeyWord;
                parameters[2].Value = info.LoaiKhieuToID;
                parameters[3].Value = info.TuNgay;
                parameters[4].Value = info.DenNgay;
                parameters[5].Value = info.StateName;
                parameters[6].Value = info.PrevStateName;
                parameters[7].Value = info.PhongBanID;

                if ((info.TuNgay) == DateTime.MinValue)
                    parameters[3].Value = DBNull.Value;
                if ((info.DenNgay) == DateTime.MinValue)
                    parameters[4].Value = DBNull.Value;

            }

            else
            {
                parameters = new SqlParameter[]{
                new SqlParameter(PARAM_COQUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_KEY,SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTO_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_STATE_NAME,SqlDbType.NVarChar,100),
                new SqlParameter(PARM_PREV_STATENAME,SqlDbType.NVarChar,100),
                new SqlParameter(PARAM_CANBO_ID,SqlDbType.Int),
            };
                parameters[0].Value = info.CoQuanID;
                parameters[1].Value = info.KeyWord;
                parameters[2].Value = info.LoaiKhieuToID;
                parameters[3].Value = info.TuNgay;
                parameters[4].Value = info.DenNgay;
                parameters[5].Value = info.StateName;
                parameters[6].Value = info.PrevStateName;
                parameters[7].Value = info.CanBoID;

                if ((info.TuNgay) == DateTime.MinValue)
                    parameters[3].Value = DBNull.Value;
                if ((info.DenNgay) == DateTime.MinValue)
                    parameters[4].Value = DBNull.Value;

            }

            try
            {
                if (info.IsTruongPhong == true)
                    using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, DT_DADUYETXL_TRUONGPHONG_COUNT_SEARCH, parameters))
                    {

                        if (dr.Read())
                        {
                            result = Utils.ConvertToInt32(dr["CountNum"], 0);
                        }
                        dr.Close();
                    }
                else
                    using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, DT_DADUYETXL_CHUYENVIEN_COUNT_SEARCH, parameters))
                    {

                        if (dr.Read())
                        {
                            result = Utils.ConvertToInt32(dr["CountNum"], 0);
                        }
                        dr.Close();
                    }
            }
            catch
            {

                throw;
            }
            return result;
        }

        public IList<ChuyenXuLyInfo> GetDSDonThuDaGiaiQuyet(QueryFilterInfo info, int start, int end)
        {

            IList<ChuyenXuLyInfo> dsDonThu = new List<ChuyenXuLyInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_COQUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_KEY,SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTO_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_START,SqlDbType.Int),
                new SqlParameter(PARAM_END,SqlDbType.Int),
                new SqlParameter(PARAM_STATE_NAME,SqlDbType.NVarChar,100),
                new SqlParameter(PARAM_VAITRO,SqlDbType.TinyInt),
                new SqlParameter(PARAM_CANBO_ID,SqlDbType.Int),
            };
            parameters[0].Value = info.CoQuanID;
            parameters[1].Value = info.KeyWord;
            parameters[2].Value = info.LoaiKhieuToID;
            parameters[3].Value = info.TuNgay;
            parameters[4].Value = info.DenNgay;
            parameters[5].Value = start;
            parameters[6].Value = end;
            parameters[7].Value = info.StateName;
            parameters[8].Value = Convert.ToByte(info.VaiTro);
            parameters[9].Value = info.CanBoID;

            if ((info.TuNgay) == DateTime.MinValue)
            {
                parameters[3].Value = DBNull.Value;
            }

            if ((info.DenNgay) == DateTime.MinValue)
            {
                parameters[4].Value = DBNull.Value;
            }

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, DS_DONTHU_DAGIAIQUYET, parameters))
                {
                    while (dr.Read())
                    {
                        ChuyenXuLyInfo cInfo = GetDataDSDonThuDaDuyetKQGiaiQuyet(dr);
                        dsDonThu.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return dsDonThu;
        }

        public IList<ChuyenXuLyInfo> GetDSDonThuDaDuyetKQGiaiQuyet_Old(QueryFilterInfo info, int start, int end)
        {
            IList<ChuyenXuLyInfo> dsDonThu = new List<ChuyenXuLyInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_COQUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_KEY,SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTO_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_START,SqlDbType.Int),
                new SqlParameter(PARAM_END,SqlDbType.Int),
                new SqlParameter(PARAM_STATE_NAME,SqlDbType.NVarChar,100),
                new SqlParameter(PARM_PREV_STATENAME,SqlDbType.NVarChar,100),
                new SqlParameter(PARAM_VAITRO,SqlDbType.TinyInt),
                new SqlParameter(PARAM_CANBO_ID,SqlDbType.Int),
            };
            parameters[0].Value = info.CoQuanID;
            parameters[1].Value = info.KeyWord;
            parameters[2].Value = info.LoaiKhieuToID;
            parameters[3].Value = info.TuNgay;
            parameters[4].Value = info.DenNgay;
            parameters[5].Value = start;
            parameters[6].Value = end;
            parameters[7].Value = info.StateName;
            parameters[8].Value = info.PrevStateName;
            parameters[9].Value = Convert.ToByte(info.VaiTro);
            parameters[10].Value = info.CanBoID;

            if ((info.TuNgay) == DateTime.MinValue)
            {
                parameters[3].Value = DBNull.Value;
            }

            if ((info.DenNgay) == DateTime.MinValue)
            {
                parameters[4].Value = DBNull.Value;
            }

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, DS_DONTHU_DADUYET_KQ_GIAIQUYET, parameters))
                {
                    while (dr.Read())
                    {
                        ChuyenXuLyInfo cInfo = GetDataDSDonThuDaDuyetKQGiaiQuyet(dr);
                        dsDonThu.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return dsDonThu;
        }

        public IList<ChuyenXuLyInfo> GetDSDonThuDaDuyetKQGiaiQuyet(QueryFilterInfo info, int start, int end, ref int TotalRow)
        {
            IList<ChuyenXuLyInfo> dsDonThu = new List<ChuyenXuLyInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_COQUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_KEY,SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTO_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_START,SqlDbType.Int),
                new SqlParameter(PARAM_END,SqlDbType.Int),
                new SqlParameter(PARAM_STATE_NAME,SqlDbType.NVarChar,100),
                new SqlParameter(PARM_PREV_STATENAME,SqlDbType.NVarChar,100),
                new SqlParameter(PARAM_VAITRO,SqlDbType.TinyInt),
                new SqlParameter(PARAM_CANBO_ID,SqlDbType.Int),
                new SqlParameter("TrangThai",SqlDbType.Int),
                new SqlParameter("TotalRow",SqlDbType.Int),

            };
            parameters[0].Value = info.CoQuanID;
            parameters[1].Value = info.KeyWord;
            parameters[2].Value = info.LoaiKhieuToID;
            parameters[3].Value = info.TuNgay;
            parameters[4].Value = info.DenNgay;
            parameters[5].Value = start;
            parameters[6].Value = end;
            parameters[7].Value = info.StateName;
            parameters[8].Value = info.PrevStateName;
            parameters[9].Value = Convert.ToByte(info.VaiTro);
            parameters[10].Value = info.CanBoID;
            parameters[11].Value = info.TrangThai ?? Convert.DBNull;
            parameters[12].Direction = ParameterDirection.Output;
            parameters[12].Size = 8;


            if ((info.TuNgay) == DateTime.MinValue)
            {
                parameters[3].Value = DBNull.Value;
            }

            if ((info.DenNgay) == DateTime.MinValue)
            {
                parameters[4].Value = DBNull.Value;
            }

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "DonThu_DSDaDuyetKetQuaGiaiQuyet_New2", parameters))
                {
                    while (dr.Read())
                    {
                        ChuyenXuLyInfo cInfo = GetDataDSDonThuDaDuyetKQGiaiQuyet_New(dr);
                        cInfo.NgayTiep = Utils.ConvertToDateTime(dr["NgayNhapDon"], DateTime.Now);
                        cInfo.LoaiKhieuTo1ID = Utils.ConvertToInt32(dr["LoaiKhieuTo1ID"], 0);
                        cInfo.TrangThaiKhoa = Utils.ConvertToBoolean(dr["TrangThaiKhoa"], false);

                        cInfo.LoaiQuyTrinh = Utils.ConvertToInt32(dr["LoaiQuyTrinh"], 0);
                        cInfo.HuongGiaiQuyetID = Utils.ConvertToInt32(dr["HuongGiaiQuyetID"], 0);
                        cInfo.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        cInfo.TrangThaiDuyet = Utils.ConvertToInt32(dr["TrangThaiDuyet"], 0);
                        cInfo.TrinhDuThao = Utils.ConvertToInt32(dr["TrinhDuThao"], 0);
                        cInfo.ChuyenGiaiQuyetID = Utils.ConvertToInt32(dr["ChuyenGiaiQuyetID"], 0);
                        cInfo.NgayTiepNhan = Utils.GetDateTime(dr["NgayTiepNhan"], DateTime.MinValue);
                        cInfo.LanhDaoDuyet2ID = Utils.ConvertToInt32(dr["LanhDaoDuyet2ID"], 0); 
                        // bổ sung trạng thái ThiHanhID 
                        cInfo.ThiHanhID = Utils.ConvertToInt32(dr["ThiHanhID"], 0);
                        // bổ sung trạng thái RutDonID
                        cInfo.RutDonID = Utils.ConvertToInt32(dr["RutDonID"], 0);
                        dsDonThu.Add(cInfo);
                    }
                    dr.Close();
                }
                TotalRow = Utils.ConvertToInt32(parameters[12].Value, 0);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return dsDonThu;
        }


        public IList<ChuyenXuLyInfo> GetDSDonThuTranhChap(QueryFilterInfo info, int start, int end)
        {
            IList<ChuyenXuLyInfo> dsDonThu = new List<ChuyenXuLyInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_COQUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_KEY,SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_START,SqlDbType.Int),
                new SqlParameter(PARAM_END,SqlDbType.Int),
                new SqlParameter(PARAM_STATE_NAME,SqlDbType.NVarChar,100),
                new SqlParameter(PARM_PREV_STATENAME,SqlDbType.NVarChar,100),
                new SqlParameter(PARM_TRANGTHAI,SqlDbType.Int),
                new SqlParameter(PARM_LOAITRANHCHAP,SqlDbType.Int),
                new SqlParameter(PARM_HUONGXULYID,SqlDbType.Int),
            };
            parameters[0].Value = info.CoQuanID;
            parameters[1].Value = info.KeyWord;
            parameters[2].Value = start;
            parameters[3].Value = end;
            parameters[4].Value = info.StateName;
            parameters[5].Value = info.PrevStateName;
            parameters[6].Value = info.TrangThaiKQ;
            parameters[7].Value = info.LoaiKhieuToID;
            parameters[8].Value = info.HuongXuLyID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "DonThu_DSDonThuTranhChap", parameters))
                {
                    while (dr.Read())
                    {
                        ChuyenXuLyInfo cInfo = new ChuyenXuLyInfo();
                        cInfo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        cInfo.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"].ToString(), 0);
                        cInfo.DonThuID = Utils.ConvertToInt32(dr["DonThuID"].ToString(), 0);
                        cInfo.SoDonThu = Utils.ConvertToString(dr["SoDonThu"], string.Empty);
                        cInfo.NguonDonDen = Utils.ConvertToInt32(dr["NguonDonDen"].ToString(), 0);
                        cInfo.NoiDungDon = Utils.ConvertToString(dr["NoiDungDon"].ToString(), String.Empty);
                        if (cInfo.NoiDungDon.Length > Constant.LengthNoiDungDon)
                        {
                            cInfo.NoiDungDon = cInfo.NoiDungDon.Substring(0, Constant.LengthNoiDungDon) + Constant.ChuoiCuoiNDDon;
                        }
                        cInfo.TenLoaiKhieuTo = Utils.ConvertToString(dr["TenLoaiKhieuTo"].ToString(), String.Empty);
                        cInfo.HoTen = Utils.ConvertToString(dr["HoTen"].ToString(), String.Empty);
                        cInfo.NgayPhan = Utils.ConvertToDateTime(dr["ModifiedDate"].ToString(), DateTime.MinValue);
                        cInfo.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"].ToString(), 0);
                        cInfo.NgayHetHan = Utils.ConvertToDateTime(dr["HanGiaiQuyetNew"].ToString(), DateTime.MinValue);
                        //cInfo.TenCanBoXuLy = Utils.ConvertToString(dr["TenCanBo"].ToString(), String.Empty);
                        cInfo.KetQuaTranhChapID_Str = Utils.ConvertToString(dr["KetQuaTranhChapID"], string.Empty);
                        cInfo.KetQuaTranhChapID = Utils.ConvertToInt32(dr["KetQuaTranhChapID"], 0);
                        dsDonThu.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
            }
            return dsDonThu;
        }

        public int CountSearchDSDaGiaiQuyet(QueryFilterInfo info)
        {

            int result = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_COQUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_KEY,SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTO_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_STATE_NAME,SqlDbType.NVarChar,100),
                new SqlParameter(PARAM_VAITRO,SqlDbType.TinyInt),
                new SqlParameter(PARAM_CANBO_ID,SqlDbType.Int),
            };

            parameters[0].Value = info.CoQuanID;
            parameters[1].Value = info.KeyWord;
            parameters[2].Value = info.LoaiKhieuToID;
            parameters[3].Value = info.TuNgay;
            parameters[4].Value = info.DenNgay;
            parameters[5].Value = info.StateName;
            parameters[6].Value = Convert.ToByte(info.VaiTro);
            parameters[7].Value = info.CanBoID;

            if ((info.TuNgay) == DateTime.MinValue)
            {
                parameters[3].Value = DBNull.Value;
            }

            if ((info.DenNgay) == DateTime.MinValue)
            {
                parameters[4].Value = DBNull.Value;
            }
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, DS_DONTHU_DA_GIAIQUYET_SEARCH, parameters))
                {

                    if (dr.Read())
                    {
                        result = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return result;
        }

        public int CountSearchDSTranhChap(QueryFilterInfo info)
        {
            int result = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_COQUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_KEY,SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_STATE_NAME,SqlDbType.NVarChar,100),
                new SqlParameter(PARM_PREV_STATENAME,SqlDbType.NVarChar,100),
                new SqlParameter(PARM_TRANGTHAI,SqlDbType.Int),
                 new SqlParameter(PARM_LOAITRANHCHAP,SqlDbType.Int),
                new SqlParameter(PARM_HUONGXULYID,SqlDbType.Int),
            };
            parameters[0].Value = info.CoQuanID;
            parameters[1].Value = info.KeyWord;
            parameters[2].Value = info.StateName;
            parameters[3].Value = info.PrevStateName;
            parameters[4].Value = info.TrangThaiKQ;
            parameters[5].Value = info.LoaiKhieuToID;
            parameters[6].Value = info.HuongXuLyID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "DonThu_DSDonThuTranhChap_CountBySearch", parameters))
                {

                    if (dr.Read())
                    {
                        result = Utils.ConvertToInt32(dr["CountRow"], 0);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return result;
        }

        public int CountSearchDSDaDuyetKQGiaiQuyet(QueryFilterInfo info)
        {

            int result = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_COQUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_KEY,SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTO_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_STATE_NAME,SqlDbType.NVarChar,100),
                new SqlParameter(PARM_PREV_STATENAME,SqlDbType.NVarChar,100),
                new SqlParameter(PARAM_VAITRO,SqlDbType.TinyInt),
                new SqlParameter(PARAM_CANBO_ID,SqlDbType.Int),
            };

            parameters[0].Value = info.CoQuanID;
            parameters[1].Value = info.KeyWord;
            parameters[2].Value = info.LoaiKhieuToID;
            parameters[3].Value = info.TuNgay;
            parameters[4].Value = info.DenNgay;
            parameters[5].Value = info.StateName;
            parameters[6].Value = info.PrevStateName;
            parameters[7].Value = Convert.ToByte(info.VaiTro);
            parameters[8].Value = info.CanBoID;

            if ((info.TuNgay) == DateTime.MinValue)
            {
                parameters[3].Value = DBNull.Value;
            }

            if ((info.DenNgay) == DateTime.MinValue)
            {
                parameters[4].Value = DBNull.Value;
            }
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, DS_DONTHU_DADUYET_KQ_GIAIQUYET_SEARCH, parameters))
                {

                    if (dr.Read())
                    {
                        result = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return result;
        }

        //Trang default
        public IList<ChuyenXuLyInfo> GetDSDonThuQuaHan(QueryFilterInfo info)
        {
            IList<ChuyenXuLyInfo> dsDonThu = new List<ChuyenXuLyInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_COQUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_STATE_NAME,SqlDbType.NVarChar,100),
                new SqlParameter(PARAM_IS_CHUYENVIEN,SqlDbType.Bit),
                new SqlParameter(PARAM_CANBO_ID,SqlDbType.Int)
            };
            parameters[0].Value = info.CoQuanID;
            parameters[1].Value = info.TuNgay;
            parameters[2].Value = info.StateName;
            parameters[3].Value = info.IsChuyenVien;
            parameters[4].Value = info.CanBoID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, DS_DONTHU_QUAHAN, parameters))
                {
                    while (dr.Read())
                    {
                        ChuyenXuLyInfo cInfo = GetDataDSDonThuQuaHan(dr);
                        dsDonThu.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return dsDonThu;
        }

        public IList<ChuyenXuLyInfo> GetDSDonThuQuaHan_Default(int coQuanID)
        {
            IList<ChuyenXuLyInfo> dsDonThu = new List<ChuyenXuLyInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_COQUAN_ID,SqlDbType.Int)

            };
            parameters[0].Value = coQuanID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DONTHU_DANG_GQ, parameters))
                {
                    while (dr.Read())
                    {
                        ChuyenXuLyInfo cInfo = GetDataDSDonThuQuaHan(dr);

                        dsDonThu.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return dsDonThu;
        }

        public IList<ChuyenXuLyInfo> GetDSDonThuMoiTiepNhan_Default(int CoQuanID, int CanBoID, int StateID, int PhongBanID)
        {
            IList<ChuyenXuLyInfo> dsDonThu = new List<ChuyenXuLyInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_COQUAN_ID,SqlDbType.Int),
                  new SqlParameter(PARAM_CANBO_ID,SqlDbType.Int),
                new SqlParameter(PARAM_STATEID,SqlDbType.Int),
                new SqlParameter(PARAM_PHONGBAN_ID,SqlDbType.Int)
            };
            parameters[0].Value = CoQuanID;
            parameters[1].Value = CanBoID;
            parameters[2].Value = StateID;
            parameters[3].Value = PhongBanID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DTMOITIEPNHAN_DEFAULT, parameters))
                {
                    while (dr.Read())
                    {
                        ChuyenXuLyInfo cInfo = GetDataDSDonThuMoiTiepNhan(dr);
                        dsDonThu.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return dsDonThu;
        }

        public IList<ChuyenXuLyInfo> GetDSDonThuMoiTiepNhan(QueryFilterInfo info)
        {
            IList<ChuyenXuLyInfo> dsDonThu = new List<ChuyenXuLyInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_COQUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_STATE_NAME,SqlDbType.NVarChar,100),
                new SqlParameter(PARAM_IS_CHUYENVIEN,SqlDbType.Bit),
                new SqlParameter(PARAM_CANBO_ID,SqlDbType.Int)
            };
            parameters[0].Value = info.CoQuanID;
            parameters[1].Value = info.StateName;
            parameters[2].Value = info.IsChuyenVien;
            parameters[3].Value = info.CanBoID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, DS_DONTHU_MOITIEPNHAN, parameters))
                {
                    while (dr.Read())
                    {
                        ChuyenXuLyInfo cInfo = GetDataDSDonThuMoiTiepNhan(dr);
                        dsDonThu.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return dsDonThu;
        }

        public IList<ChuyenXuLyInfo> GetDSDonThuCanXuLy(QueryFilterInfo info)
        {
            IList<ChuyenXuLyInfo> dsDonThu = new List<ChuyenXuLyInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_COQUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_STATEID,SqlDbType.NVarChar,100),
                new SqlParameter(PARAM_IS_CHUYENVIEN,SqlDbType.Bit),
                new SqlParameter(PARAM_CANBO_ID,SqlDbType.Int),
                new SqlParameter(PARAM_PHONGBAN_ID, SqlDbType.Int)
            };
            parameters[0].Value = info.CoQuanID;
            parameters[1].Value = info.StateName;
            parameters[2].Value = info.IsChuyenVien;
            parameters[3].Value = info.CanBoID;
            parameters[4].Value = info.PhongBanID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, DS_DONTHU_CAN_XULY, parameters))
                {
                    while (dr.Read())
                    {
                        ChuyenXuLyInfo cInfo = GetDataDSDonThuCanXLCanGQ(dr);
                        dsDonThu.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return dsDonThu;
        }


        public IList<ChuyenXuLyInfo> GetDSDonThuCanXuLy_Default(int CoQuanID, int CanBoID, int StateID, int PhongBanID)
        {
            IList<ChuyenXuLyInfo> dsDonThu = new List<ChuyenXuLyInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_COQUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_CANBO_ID,SqlDbType.NVarChar,100),
                new SqlParameter(PARAM_STATEID ,SqlDbType.Int),
                new SqlParameter(PARAM_PHONGBAN_ID, SqlDbType.Int)
            };
            parameters[0].Value = CoQuanID;
            parameters[1].Value = CanBoID;
            parameters[2].Value = StateID;
            parameters[3].Value = PhongBanID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DTCANXULY_DEFAULT, parameters))
                {
                    while (dr.Read())
                    {
                        ChuyenXuLyInfo cInfo = GetDataDSDonThuCanXLCanGQ(dr);
                        dsDonThu.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return dsDonThu;
        }

        public IList<ChuyenXuLyInfo> GetDSDonThuCanGiaiQuyet(QueryFilterInfo info)
        {
            IList<ChuyenXuLyInfo> dsDonThu = new List<ChuyenXuLyInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_COQUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_STATE_NAME,SqlDbType.NVarChar,100),
                new SqlParameter(PARAM_IS_CHUYENVIEN,SqlDbType.Bit),
                new SqlParameter(PARAM_CANBO_ID,SqlDbType.Int)
            };
            parameters[0].Value = info.CoQuanID;
            parameters[1].Value = info.StateName;
            parameters[2].Value = info.IsChuyenVien;
            parameters[3].Value = info.CanBoID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, DS_DONTHU_CAN_GIAIQUYET, parameters))
                {
                    while (dr.Read())
                    {
                        ChuyenXuLyInfo cInfo = GetDataDSDonThuCanXLCanGQ(dr);
                        dsDonThu.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return dsDonThu;
        }

        public IList<ChuyenXuLyInfo> GetDSDonThuCanGiaiQuyet_Default(int CoQuanID, int CanBoID, int StateID, int PhongBanID)
        {
            IList<ChuyenXuLyInfo> dsDonThu = new List<ChuyenXuLyInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_COQUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_CANBO_ID,SqlDbType.Int),
                 new SqlParameter(PARAM_STATEID,SqlDbType.Int),
                 new  SqlParameter(PARAM_PHONGBAN_ID,SqlDbType.Int)
            };
            parameters[0].Value = CoQuanID;
            parameters[1].Value = CanBoID;
            parameters[2].Value = StateID;
            parameters[3].Value = PhongBanID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DTCANGIAIQUYET_DEFAULT, parameters))
                {
                    while (dr.Read())
                    {
                        ChuyenXuLyInfo cInfo = GetDataDSDonThuCanXLCanGQ(dr);
                        cInfo.TenCanBoGiaiQuyet = Utils.GetString(dr["TenCanBoGiaiQuyet"], string.Empty);
                        cInfo.HanGiaiQuyet = Utils.ConvertToDateTime(dr["HanGiaiQuyet"], DateTime.MinValue);
                        dsDonThu.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return dsDonThu;
        }

        public IList<ChuyenXuLyInfo> GetDSVuViecNoiBat(QueryFilterInfo info)
        {
            IList<ChuyenXuLyInfo> dsDonThu = new List<ChuyenXuLyInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_COQUAN_ID,SqlDbType.Int)
            };
            parameters[0].Value = info.CoQuanID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, DS_VUVIEC_NOIBAT, parameters))
                {
                    while (dr.Read())
                    {
                        ChuyenXuLyInfo cInfo = new ChuyenXuLyInfo();
                        cInfo.NoiDungDon = Utils.GetString(dr["NoiDungDon"], string.Empty);
                        cInfo.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
                        cInfo.DonThuID = Utils.ConvertToInt32(dr["DonThuID"], 0);
                        cInfo.SoLanTiep = Utils.ConvertToInt32(dr["SoLanTiep"], 0);
                        cInfo.SoLuongNguoiKNTC = Utils.ConvertToInt32(dr["SoLuong"], 0);

                        dsDonThu.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return dsDonThu;
        }

        public int CountSoLanTiep(int xulydonID)
        {

            int result = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDON_ID,SqlDbType.Int)
            };
            parameters[0].Value = xulydonID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_SOLANTIEP, parameters))
                {

                    if (dr.Read())
                    {
                        result = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return result;
        }

        public ChuyenXuLyInfo GetKetQuaBanHanh(int xuLyDonID)
        {
            ChuyenXuLyInfo dsDonThu = new ChuyenXuLyInfo();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDON_ID,SqlDbType.Int),
            };
            parameters[0].Value = xuLyDonID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "KetQua_GetByXuLyDonID", parameters))
                {
                    while (dr.Read())
                    {
                        dsDonThu.KetQuaBanHanhID = Utils.ConvertToInt32(dr["KetQuaID"], 0);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
            }
            return dsDonThu;
        }
    }
}
