using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Ultilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.DAL.KNTC
{
    public class BCTinhHinhTD_XLD_GQ
    {
        #region -- store proc
        private const string GET_DTDUYETXL_LANHDAO_FORBC = @"XuLyDon_GetDTDuyetXL_ForBC";
        private const string GET_DONTHU_CANPhanGIAIQUYET_TRONGCOQUAN = "PhanGiaiQuyet_GetDonThuCanPhanGQ_ForBC";
        private const string GET_DS_DONTHU_CANGIAIQUYET_FORBC = @"XuLyDon_GetDonThuCanGiaiQuyet_ForBC";
        private const string DS_DONTHU_DADUYET_KQ_GQ_FORBC = @"DonThu_DSDaDuyetKQGQ_For_BC";



        private const string BAOCAO_TINHHINHTDXLDGQ_DSCOQUAN_XULYDON_GETBYDATE = @"BaoCao_TinhHinhTDXLDGQ_DSCoQuan_XuLyDon_GetByDate";
        private const string BAOCAO_TINHHINHTDXLDGQ_DSCOQUAN_GIAIQUYETDON_GETBYDATE = @"BaoCao_TinhHinhTDXLDGQ_DSCoQuan_GiaiQuyetDon_GetByDate";

        private const string DASHBOARD_TINHHINHTDXLDGQ_DSCOQUAN_GIAIQUYETDON_GETBYDATE = @"Dashboard_TinhHinhTDXLDGQ_DSCoQuan_GiaiQuyetDon_GetByDate";
        private const string DASHBOARD_THONGKEXULYDON_GETBYDATE = @"Dashboard_ThongKeXuLyDon_GetByDate";
        private const string DASHBOARD_THONGKEGIAIQUYETDON_GETBYDATE = @"Dashboard_ThongKeGiaiQuyetDon_GetByDate";

        private const string BAOCAO_TINHHINHTDXLDGQ_DSCHITIETDONTHU_TIEPDAN_GETBYDATE = @"BaoCao_TinhHinhTDXLDGQ_DSChiTietDonThu_TiepDan_GetByDate";
        private const string BAOCAO_TINHHINHTDXLDGQ_DSCHITIETDONTHU_XULYDON_GETBYDATE = @"BaoCao_TinhHinhTDXLDGQ_DSChiTietDonThu_XuLyDon_GetByDate";
        private const string BAOCAO_TINHHINHTDXLDGQ_DSCHITIETDONTHU_GIAIQUYETDON_GETBYDATE = @"BaoCao_TinhHinhTDXLDGQ_DSChiTietDonThu_GiaiQuyetDon_GetByDate";
        #endregion

        #region -- param
        private const string PARAM_KEY = "@Keyword";
        private const string PARAM_START = "@Start";
        private const string PARAM_END = "@End";

        private const string PARAM_COQUAN_ID = "@CoQuanID";
        private const string PARAM_LOAIKHIEUTO_ID = "@LoaiKhieuToID";
        private const string PARAM_TUNGAY = "@TuNgay";
        private const string PARAM_DENNGAY = "@DenNgay";
        private const string PARAM_STATE_NAME = "@StateName";
        private const string PARM_PREV_STATENAME = "@PrevStateName";

        private const string PARAM_CANBO_ID = "@CanBoID";
        private const string PARAM_VAITRO = "@VaiTro";

        private const string PARAM_TUNGAYGOC = "@TuNgayGoc";
        private const string PARAM_DENNGAYGOC = "@DenNgayGoc";
        private const string PARAM_TUNGAYMOI = "@TuNgayMoi";
        private const string PARAM_DENNGAYMOI = "@DenNgayMoi";
        private const string PARAM_COQUANID = "@CoQuanID";
        private const string PARAM_CANBOID = "@CanBoID";
        private const string PARAM_PHONGBANID = "@PhongBanID";
        private const string PARAM_LOAIXL = "@LoaiXL";
        private const string PARAM_PTKQDUNG = "@PTKQDung";
        private const string PARAM_PTKQDUNGMOTPHAN = "@PTKQDungMotPhan";
        private const string PARAM_PTKQSAI = "@PTKDSai";
        private const string PARAM_TUNGAYINT = "@TuNgayInt";
        private const string PARAM_DENNGAYINT = "@DenNgayInt";
        private const string PARAM_CAPID = "@CapID";
        private const string PARAM_TINHID = "@TinhID";
        #endregion

        #region -- get dt xl
        private DTXuLyInfo GetDataDTDuyetXuLy(SqlDataReader dr)
        {
            DTXuLyInfo info = new DTXuLyInfo();

            info.DonThuID = Utils.GetInt32(dr["DonThuID"], 0);
            info.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);
            info.SoDonThu = Utils.GetString(dr["SoDonThu"], string.Empty);
            info.NguonDonDen = Utils.GetInt32(dr["NguonDonDen"], 0);
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
            info.TenChuDon = Utils.GetString(dr["HoTens"], string.Empty);
            info.NoiDungDon = Utils.GetString(dr["NoiDungDon"], string.Empty);

            info.NgayGui = Utils.GetDateTime(dr["ModifiedDate"], DateTime.MinValue);
            info.NgayGuis = string.Empty;
            if (info.NgayGui != DateTime.MinValue)
            {
                info.NgayGuis = info.NgayGui.ToString("dd/MM/yyyy");
            }
            info.TenLoaiKhieuTo = Utils.ConvertToString(dr["TenLoaiKhieuTo"], string.Empty);
            info.TenCBXuLy = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
            info.HuongXuLy = Utils.ConvertToString(dr["TenHuongGiaiQuyet"], string.Empty);
            info.PhongBanID = Utils.ConvertToInt32(dr["PhongBanID"], 0);
            if (dr["CanBoID"] != null)
                info.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);

            return info;
        }

        public SqlParameter[] GetPara_Search_LanhDao()
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_COQUAN_ID, SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime)
            }; return parms;
        }

        public void SetPara_Search_LanhDao(SqlParameter[] parms, QueryFilterInfo info)
        {

            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.TuNgay;
            parms[2].Value = info.DenNgay;

            if (info.TuNgay == DateTime.MinValue) parms[1].Value = DBNull.Value;
            if (info.DenNgay == DateTime.MinValue) parms[2].Value = DBNull.Value;
        }

        public IList<DTXuLyInfo> DTDuyetKQXL_LanhDao(QueryFilterInfo info)
        {

            IList<DTXuLyInfo> ListInfo = new List<DTXuLyInfo>();
            SqlParameter[] parms = GetPara_Search_LanhDao();
            SetPara_Search_LanhDao(parms, info);
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DTDUYETXL_LANHDAO_FORBC, parms))
                {
                    while (dr.Read())
                    {
                        DTXuLyInfo Info = GetDataDTDuyetXuLy(dr);
                        Info.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        Info.NgayQuaHan = Utils.ConvertToDateTime(dr["HanXuLyDueDate"], DateTime.MinValue);
                        Info.NgayLDDuyetXL = Utils.ConvertToDateTime(dr["NgayXuLyDueDate"], DateTime.MinValue);
                        Info.HanXuLy = Format.FormatDate(Info.NgayQuaHan);
                        Info.TransitionID = Utils.ConvertToInt32(dr["TransitionID"], 0);
                        Info.NgayNhapDonStr = Format.FormatDate(Utils.ConvertToDateTime(dr["NgayNhapDon"], DateTime.MinValue));
                        ListInfo.Add(Info);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return ListInfo;
        }
        #endregion

        #region -- dt can phan gq ldao
        private DonThuGiaiQuyetInfo GetData(SqlDataReader rdr)
        {
            DonThuGiaiQuyetInfo info = new DonThuGiaiQuyetInfo();

            info.XuLyDonID = Utils.GetInt32(rdr["XuLyDonID"], 0);
            info.DonThuID = Utils.GetInt32(rdr["DonThuID"], 0);
            info.HoTen = Utils.GetString(rdr["HoTen"], string.Empty);
            info.SoDonThu = Utils.GetString(rdr["SoDonThu"], string.Empty);
            info.NoiDungDon = Utils.GetString(rdr["NoiDungDon"], String.Empty);
            if (info.NoiDungDon.Length > Constant.LengthNoiDungDon)
            {
                info.NoiDungDon = info.NoiDungDon.Substring(0, Constant.LengthNoiDungDon) + Constant.ChuoiCuoiNDDon;
            }

            info.TenLoaiKhieuTo = Utils.GetString(rdr["TenLoaiKhieuTo"], String.Empty);

            return info;
        }

        public SqlParameter[] GetParms_LocDL_LD()
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_COQUAN_ID, SqlDbType.Int),
                //new SqlParameter(PARM_KEYWORD, SqlDbType.NVarChar,50),
                //new SqlParameter(PARM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter("@TuNgayGoc", SqlDbType.DateTime),
                new SqlParameter("@DenNgayGoc", SqlDbType.DateTime),
                //new SqlParameter(PARM_START, SqlDbType.Int),
                //new SqlParameter(PARM_END, SqlDbType.Int),
                new SqlParameter(PARAM_STATE_NAME, SqlDbType.NVarChar,100),
                //new SqlParameter(PARM_CANBOID, SqlDbType.Int),
                //new SqlParameter(PARM_PHONGBANID, SqlDbType.Int),
                new SqlParameter("@TuNgayMoi", SqlDbType.DateTime),
                new SqlParameter("@DenNgayMoi", SqlDbType.DateTime),
            }; return parms;
        }

        public void SetParms_LocDL_LD(SqlParameter[] parms, QueryFilterInfo info)
        {
            parms[0].Value = info.CoQuanID;
            //parms[1].Value = info.KeyWord;
            //parms[2].Value = info.LoaiKhieuToID;
            parms[1].Value = info.TuNgayGoc;
            parms[2].Value = info.DenNgayGoc;
            //parms[5].Value = info.Start;
            //parms[6].Value = info.End;
            parms[3].Value = info.StateName;
            //parms[8].Value = info.CanBoID;
            //parms[9].Value = info.PhongBanID;
            parms[4].Value = info.TuNgayMoi;
            parms[5].Value = info.DenNgayMoi;

            if (info.TuNgayGoc == DateTime.MinValue) parms[1].Value = DBNull.Value;
            if (info.DenNgayGoc == DateTime.MinValue) parms[2].Value = DBNull.Value;
            if (info.TuNgayMoi == DateTime.MinValue) parms[4].Value = DBNull.Value;
            if (info.DenNgayMoi == DateTime.MinValue) parms[5].Value = DBNull.Value;
        }

        public IList<DonThuGiaiQuyetInfo> GetDonThuCanPhanGiaiQuyet(QueryFilterInfo info)
        {
            IList<DonThuGiaiQuyetInfo> phangiaiquyets = new List<DonThuGiaiQuyetInfo>();

            SqlParameter[] parms = GetParms_LocDL_LD();

            SetParms_LocDL_LD(parms, info);

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DONTHU_CANPhanGIAIQUYET_TRONGCOQUAN, parms))
                {
                    while (dr.Read())
                    {
                        DonThuGiaiQuyetInfo phanGiaiQuyetInfo = GetData(dr);
                        phanGiaiQuyetInfo.DuongDanFile = Utils.ConvertToString(dr["FileUrl"], string.Empty);
                        //phanGiaiQuyetInfo.ExecuteTime = Utils.ConvertToInt32(dr["ExecuteTime"], 1);
                        //phanGiaiQuyetInfo.DaPhanCQKhac = Utils.ConvertToInt32(dr["DaPhanCQKhac"], 1);
                        //phanGiaiQuyetInfo.DaPhanTrongCQ = Utils.ConvertToInt32(dr["DaPhanTrongCQ"], 1);
                        phanGiaiQuyetInfo.TenCoQuanPhanGQ = Utils.ConvertToString(dr["TenCoQuanPhanGQ"], string.Empty);
                        phanGiaiQuyetInfo.TenCoQuanGQ = Utils.ConvertToString(dr["TenCoQuanGQ"], string.Empty);
                        phanGiaiQuyetInfo.TenNguonDonDen = Utils.GetString(dr["TenNguonDonDen"], string.Empty);
                        phanGiaiQuyetInfo.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        phanGiaiQuyetInfo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        phanGiaiQuyetInfo.ChuyenGiaiQuyetID = Utils.ConvertToInt32(dr["CQGiaoID"], 0);
                        phanGiaiQuyetInfo.TheoDoiXuLyID = Utils.ConvertToInt32(dr["TheoDoiXuLyID"], 0);
                        phanGiaiQuyetInfo.LoaiKhieuTo1ID = Utils.ConvertToInt32(dr["LoaiKhieuTo1ID"], 0);

                        string now = Format.FormatDate(DateTime.Now);
                        DateTime ngayHienTai = Utils.ConvertToDateTime(now, DateTime.MinValue);
                        //TimeSpan ngayConLai = 0;

                        phanGiaiQuyetInfo.HanGQGoc = Utils.ConvertToDateTime(dr["HanGQGoc"], DateTime.MinValue);
                        phanGiaiQuyetInfo.HanGQMoi = Utils.ConvertToDateTime(dr["HanGQMoi"], DateTime.MinValue);
                        phanGiaiQuyetInfo.NgayCapNhat = Utils.ConvertToDateTime(dr["NgayCapNhat"], DateTime.MinValue);
                        phanGiaiQuyetInfo.NgayCapNhatStr = Format.FormatDate(phanGiaiQuyetInfo.NgayCapNhat);
                        phanGiaiQuyetInfo.NgayNhapDon = Utils.ConvertToDateTime(dr["NgayNhapDon"], DateTime.MinValue);
                        phanGiaiQuyetInfo.NgayNhapDonStr = Format.FormatDate(phanGiaiQuyetInfo.NgayNhapDon);
                        phanGiaiQuyetInfo.TenChuDon = Utils.GetString(dr["TenChuDon"], string.Empty);
                        phangiaiquyets.Add(phanGiaiQuyetInfo);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return phangiaiquyets;
        }
        #endregion

        #region -- get dt can giai quyet by co quan
        private DonThuGiaiQuyetInfo GetDataCTCanGiaiQuyet(SqlDataReader rdr)
        {
            DonThuGiaiQuyetInfo info = new DonThuGiaiQuyetInfo();

            info.XuLyDonID = Utils.GetInt32(rdr["XuLyDonID"], 0);
            info.DonThuID = Utils.GetInt32(rdr["DonThuID"], 0);
            info.HoTen = Utils.GetString(rdr["HoTen"], string.Empty);
            info.SoDonThu = Utils.GetString(rdr["SoDonThu"], string.Empty);
            info.NoiDungDon = Utils.GetString(rdr["NoiDungDon"], String.Empty);
            if (info.NoiDungDon.Length > Constant.LengthNoiDungDon)
            {
                info.NoiDungDon = info.NoiDungDon.Substring(0, Constant.LengthNoiDungDon) + Constant.ChuoiCuoiNDDon;
            }
            info.NgayPhanCong = Utils.GetDateTime(rdr["ModifiedDate"], DateTime.MinValue);
            info.NgayPhanCongs = info.NgayPhanCong.ToString("dd/MM/yyyy");
            info.HanGiaiQuyet = Utils.GetDateTime(rdr["DueDate"], DateTime.MinValue);
            info.HanGiaiQuyets = info.HanGiaiQuyet.Value.ToString("dd/MM/yyyy");
            info.TenLoaiKhieuTo = Utils.GetString(rdr["TenLoaiKhieuTo"], String.Empty);
            info.TenCanBoGiao = Utils.GetString(rdr["TenCanBo"], String.Empty);
            info.LoaiKhieuTo1ID = Utils.GetInt32(rdr["LoaiKhieuTo1ID"], 0);

            return info;
        }

        public SqlParameter[] GetParms_DTCanGiaiQuyet()
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_COQUAN_ID, SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
            }; return parms;
        }


        public void SetParms_DTCanGiaiQuyet(SqlParameter[] parms, QueryFilterInfo info)
        {
            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.TuNgay;
            parms[2].Value = info.DenNgay;

            if (info.TuNgay == DateTime.MinValue) parms[1].Value = DBNull.Value;
            if (info.DenNgay == DateTime.MinValue) parms[2].Value = DBNull.Value;
        }

        public IList<DonThuGiaiQuyetInfo> GetDonThuCanGiaiQuyet(QueryFilterInfo info)
        {
            IList<DonThuGiaiQuyetInfo> ListInfo = new List<DonThuGiaiQuyetInfo>();

            SqlParameter[] parms = GetParms_DTCanGiaiQuyet();

            SetParms_DTCanGiaiQuyet(parms, info);

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DS_DONTHU_CANGIAIQUYET_FORBC, parms))
                {
                    while (dr.Read())
                    {
                        DonThuGiaiQuyetInfo dtinfo = GetDataCTCanGiaiQuyet(dr);
                        dtinfo.CoQuanGiaoID = Utils.ConvertToInt32(dr["CoQuanGiaoID"], 0);
                        dtinfo.HanGQ = Utils.ConvertToDateTime(dr["HanGiaiQuyetNew"], DateTime.MinValue);
                        dtinfo.HanGQStr = dtinfo.HanGQ.ToString("dd/MM/yyyy");
                        dtinfo.NguonDonDen = Utils.ConvertToInt32(dr["NguonDonDen"], 0);
                        dtinfo.StateName = Utils.GetString(dr["StateName"], string.Empty);
                        dtinfo.StateID = Utils.GetInt32(dr["StateID"], 0);
                        dtinfo.NgayCapNhat = Utils.ConvertToDateTime(dr["NgayCapNhat"], DateTime.MinValue);
                        dtinfo.NgayCapNhatStr = Format.FormatDate(dtinfo.NgayCapNhat);

                        dtinfo.SoCVBaoCaoGQ = Utils.ConvertToString(dr["SoCVBaoCaoGQ"], string.Empty);
                        dtinfo.NgayCVBaoCaoGQ = Utils.ConvertToDateTime(dr["NgayCVBaoCaoGQ"], DateTime.MinValue);

                        string now = Format.FormatDate(DateTime.Now);
                        DateTime ngayHienTai = Utils.ConvertToDateTime(now, DateTime.MinValue);

                        DateTime ngayQuaHan = Utils.ConvertToDateTime(dr["HanGiaiQuyetNew"], DateTime.MinValue);
                        TimeSpan ngayConLai = ngayQuaHan.Subtract(ngayHienTai);
                        dtinfo.NgayGQConLai = ngayConLai.Days;

                        // anhnt
                        dtinfo.SoDon = Utils.ConvertToString(dr["SoDonThu"], string.Empty);
                        dtinfo.NoiDungDon = Utils.ConvertToString(dr["NoiDungDon"], string.Empty);
                        dtinfo.TenLoaiKhieuTo = Utils.ConvertToString(dr["TenLoaiKhieuTo"], string.Empty);
                        dtinfo.NgayTiep = Utils.ConvertToDateTime(dr["NgayNhapDon"], DateTime.MinValue);
                        if (dtinfo.NgayTiep != DateTime.MinValue)
                        {
                            dtinfo.NgayTiepStr = Format.FormatDate(dtinfo.NgayTiep);
                        }
                        dtinfo.DiaChi = Utils.ConvertToString(dr["DiaChiCT"], string.Empty);

                        ListInfo.Add(dtinfo);
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return ListInfo;
        }
        #endregion

        #region -- dt da ban hanh ket qua
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
            //cInfo.TenCanBoXuLy = Utils.ConvertToString(dr["TenCanBo"].ToString(), String.Empty);
            cInfo.KetQuaID = Utils.ConvertToString(dr["KetQuaID"], string.Empty);

            return cInfo;
        }

        public IList<ChuyenXuLyInfo> GetDSDonThuDaDuyetKQGiaiQuyet(QueryFilterInfo info)
        {

            IList<ChuyenXuLyInfo> dsDonThu = new List<ChuyenXuLyInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_COQUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_KEY,SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_TUNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_STATE_NAME,SqlDbType.NVarChar,100),
                new SqlParameter(PARM_PREV_STATENAME,SqlDbType.NVarChar,100),
            };
            parameters[0].Value = info.CoQuanID;
            parameters[1].Value = info.KeyWord;
            parameters[2].Value = info.TuNgay;
            parameters[3].Value = info.DenNgay;
            parameters[4].Value = info.StateName;
            parameters[5].Value = info.PrevStateName;

            if ((info.TuNgay) == DateTime.MinValue)
            {
                parameters[2].Value = DBNull.Value;
            }

            if ((info.DenNgay) == DateTime.MinValue)
            {
                parameters[3].Value = DBNull.Value;
            }

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, DS_DONTHU_DADUYET_KQ_GQ_FORBC, parameters))
                {
                    while (dr.Read())
                    {
                        ChuyenXuLyInfo cInfo = GetDataDSDonThuDaDuyetKQGiaiQuyet(dr);
                        cInfo.SoDon = Utils.ConvertToString(dr["SoDonThu"], string.Empty);
                        cInfo.NgayTiep = Utils.ConvertToDateTime(dr["NgayNhapDon"], DateTime.MinValue);
                        cInfo.DiaChi = Utils.ConvertToString(dr["DiaChiCT"], string.Empty);
                        cInfo.TenChuDon = Utils.ConvertToString(dr["TenChuDon"], string.Empty);
                        cInfo.LoaiKhieuTo1ID = Utils.ConvertToInt32(dr["LoaiKhieuTo1ID"], 0);
                        if (cInfo.NgayTiep != DateTime.MinValue)
                        {
                            cInfo.NgayTiepStr = Format.FormatDate(cInfo.NgayTiep);
                            cInfo.NgayNhapDonStr = Format.FormatDate(cInfo.NgayTiep);
                        }
                        cInfo.PhanTichKQ = Utils.ConvertToInt32(dr["PhanTichKQ"], 0);

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
        #endregion


        #region --Get data for bao cao
        String BAOCAO_TINHHINHTDXLDGQ_DSCOQUAN_GIAIQUYETDON_GETBYDATE_NEW = "BaoCao_TinhHinhTDXLDGQ_DSCoQuan_GiaiQuyetDon_GetByDate_New";
        public IList<BCTongHopXuLyInfo> DSCoQuan_GiaiQuyetDon_GetByDate(QueryFilterInfo infoQF)
        {

            IList<BCTongHopXuLyInfo> result = new List<BCTongHopXuLyInfo>();
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_TUNGAYGOC, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAYGOC, SqlDbType.DateTime),
                new SqlParameter(PARAM_TUNGAYMOI, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAYMOI, SqlDbType.DateTime),
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_PTKQDUNG, SqlDbType.Int),
                new SqlParameter(PARAM_PTKQDUNGMOTPHAN, SqlDbType.Int),
                new SqlParameter(PARAM_PTKQSAI, SqlDbType.Int)
            };
            parms[0].Value = infoQF.TuNgayGoc;
            parms[1].Value = infoQF.DenNgayGoc;
            parms[2].Value = infoQF.TuNgayMoi;
            parms[3].Value = infoQF.DenNgayMoi;
            parms[4].Value = infoQF.CoQuanID;
            parms[5].Value = infoQF.PTKQDung;
            parms[6].Value = infoQF.PTKQDungMotPhan;
            parms[7].Value = infoQF.PTKQSai;
            try
            {
                //System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                //stopwatch.Stop();
                //stopwatch.Start();
                //var a = new SQLHelper().ExecuteReader1(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, BAOCAO_TINHHINHTDXLDGQ_DSCOQUAN_GIAIQUYETDON_GETBYDATE, parms);
                ////var a = new SQLHelper().ExecuteReader1(SQLHelper.appConnectionStrings, CommandType.Text, "select * from nguoidung");
                //var timer1 = stopwatch.Elapsed.TotalSeconds;

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, BAOCAO_TINHHINHTDXLDGQ_DSCOQUAN_GIAIQUYETDON_GETBYDATE_NEW, parms))
                //using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.Text, "select * from nguoidung"))
                {
                    while (dr.Read())
                    {
                        BCTongHopXuLyInfo info = new BCTongHopXuLyInfo();
                        info.CoQuanGiaiQuyetID = Utils.ConvertToInt32(dr["CoQuanGiaiQuyetID"], 0);
                        info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        info.CQPhoiHopStr = Utils.ConvertToString(dr["CQPhoiHopStr"], String.Empty);
                        info.GQDTongSo = Utils.ConvertToInt32(dr["GQDTongSo"], 0);
                        info.GQDChuaGQ = Utils.ConvertToInt32(dr["GQDChuaGQ"], 0);
                        info.GQDDangGQ = Utils.ConvertToInt32(dr["GQDDangGQ"], 0);
                        info.GQDDangGQTrongHan = Utils.ConvertToInt32(dr["GQDDangGQTrongHan"], 0);
                        info.GQDDangGQQuaHan = Utils.ConvertToInt32(dr["GQDDangGQQuaHan"], 0);
                        info.GQDDaGQ = Utils.ConvertToInt32(dr["GQDDaGQ"], 0);
                        info.KQKNDung = Utils.ConvertToInt32(dr["KQKNDung"], 0);
                        info.KQKNDungMotPhan = Utils.ConvertToInt32(dr["KQKNDungMotPhan"], 0);
                        info.KQKNSai = Utils.ConvertToInt32(dr["KQKNSai"], 0);

                        info.GQDKNDangGQ = Utils.ConvertToInt32(dr["GQDKNDangGQ"], 0);
                        info.GQDTCDangGQ = Utils.ConvertToInt32(dr["GQDTCDangGQ"], 0);
                        info.GQDKNPADangGQ = Utils.ConvertToInt32(dr["GQDKNPADangGQ"], 0);
                        info.GQDKNDaGQ = Utils.ConvertToInt32(dr["GQDKNDaGQ"], 0);
                        info.GQDTCDaGQ = Utils.ConvertToInt32(dr["GQDTCDaGQ"], 0);
                        info.GQDKNPADaGQ = Utils.ConvertToInt32(dr["GQDKNPADaGQ"], 0);
                        result.Add(info);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        public IList<BCTongHopXuLyInfo> DoashBoardDSCoQuan_GiaiQuyetDon_GetByDate(QueryFilterInfo infoQF)
        {

            IList<BCTongHopXuLyInfo> result = new List<BCTongHopXuLyInfo>();
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_TUNGAYGOC, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAYGOC, SqlDbType.DateTime),
                new SqlParameter(PARAM_TUNGAYMOI, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAYMOI, SqlDbType.DateTime),
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_PTKQDUNG, SqlDbType.Int),
                new SqlParameter(PARAM_PTKQDUNGMOTPHAN, SqlDbType.Int),
                new SqlParameter(PARAM_PTKQSAI, SqlDbType.Int)
            };
            parms[0].Value = infoQF.TuNgayGoc;
            parms[1].Value = infoQF.DenNgayGoc;
            parms[2].Value = infoQF.TuNgayMoi;
            parms[3].Value = infoQF.DenNgayMoi;
            parms[4].Value = infoQF.CoQuanID;
            parms[5].Value = infoQF.PTKQDung;
            parms[6].Value = infoQF.PTKQDungMotPhan;
            parms[7].Value = infoQF.PTKQSai;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, DASHBOARD_TINHHINHTDXLDGQ_DSCOQUAN_GIAIQUYETDON_GETBYDATE, parms))
                {
                    while (dr.Read())
                    {
                        BCTongHopXuLyInfo info = new BCTongHopXuLyInfo();
                        info.CoQuanGiaiQuyetID = Utils.ConvertToInt32(dr["CoQuanGiaiQuyetID"], 0);
                        info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        info.CQPhoiHopStr = Utils.ConvertToString(dr["CQPhoiHopStr"], String.Empty);
                        info.GQDTongSo = Utils.ConvertToInt32(dr["GQDTongSo"], 0);
                        info.GQDChuaGQ = Utils.ConvertToInt32(dr["GQDChuaGQ"], 0);
                        info.GQDDangGQ = Utils.ConvertToInt32(dr["GQDDangGQ"], 0);
                        info.GQDDangGQTrongHan = Utils.ConvertToInt32(dr["GQDDangGQTrongHan"], 0);
                        info.GQDDangGQQuaHan = Utils.ConvertToInt32(dr["GQDDangGQQuaHan"], 0);
                        info.GQDDaGQ = Utils.ConvertToInt32(dr["GQDDaGQ"], 0);
                        info.KQKNDung = Utils.ConvertToInt32(dr["KQKNDung"], 0);
                        info.KQKNDungMotPhan = Utils.ConvertToInt32(dr["KQKNDungMotPhan"], 0);
                        info.KQKNSai = Utils.ConvertToInt32(dr["KQKNSai"], 0);

                        info.GQDKNDangGQ = Utils.ConvertToInt32(dr["GQDKNDangGQ"], 0);
                        info.GQDTCDangGQ = Utils.ConvertToInt32(dr["GQDTCDangGQ"], 0);
                        info.GQDKNPADangGQ = Utils.ConvertToInt32(dr["GQDKNPADangGQ"], 0);
                        info.GQDKNDaGQ = Utils.ConvertToInt32(dr["GQDKNDaGQ"], 0);
                        info.GQDTCDaGQ = Utils.ConvertToInt32(dr["GQDTCDaGQ"], 0);
                        info.GQDKNPADaGQ = Utils.ConvertToInt32(dr["GQDKNPADaGQ"], 0);
                        result.Add(info);
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
        String BAOCAO_TINHHINHTDXLDGQ_DSCOQUAN_XULYDON_GETBYDATE_NEW = "BaoCao_TinhHinhTDXLDGQ_DSCoQuan_XuLyDon_GetByDate_New";
        public IList<BCTongHopXuLyInfo> DSCoQuan_XuLyDon_GetByDate(QueryFilterInfo infoQF)
        {

            IList<BCTongHopXuLyInfo> result = new List<BCTongHopXuLyInfo>();
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_TUNGAYGOC, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAYGOC, SqlDbType.DateTime),
                new SqlParameter(PARAM_TUNGAYMOI, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAYMOI, SqlDbType.DateTime),
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int)
            };
            parms[0].Value = infoQF.TuNgayGoc;
            parms[1].Value = infoQF.DenNgayGoc;
            parms[2].Value = infoQF.TuNgayMoi;
            parms[3].Value = infoQF.DenNgayMoi;
            parms[4].Value = infoQF.CoQuanID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, BAOCAO_TINHHINHTDXLDGQ_DSCOQUAN_XULYDON_GETBYDATE_NEW, parms))
                {
                    while (dr.Read())
                    {
                        BCTongHopXuLyInfo info = new BCTongHopXuLyInfo();
                        info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        info.CapID = Utils.ConvertToInt32(dr["CapID"], 0);
                        info.SLTiepCongDan = Utils.ConvertToInt32(dr["SLTiepCongDan"], 0);
                        info.XLDTongSo = Utils.ConvertToInt32(dr["XLDTongSo"], 0);
                        info.XLDDaXuLy = Utils.ConvertToInt32(dr["XLDDaXuLy"], 0);
                        info.XLDChuaXuLy = Utils.ConvertToInt32(dr["XLDChuaXuLy"], 0);
                        info.XLDDaXuLyTrongHan = Utils.ConvertToInt32(dr["XLDDaXuLyTrongHan"], 0);
                        info.XLDDaXuLyQuaHan = Utils.ConvertToInt32(dr["XLDDaXuLyQuaHan"], 0);
                        info.XLDKhieuKienDN = Utils.ConvertToInt32(dr["XLDKhieuKienDN"], 0);
                        result.Add(info);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        public BCTinhHinhTD_XLD_GQInfo Dashboard_GiaiQuyetDon_GetByDate(QueryFilterInfo infoQF)
        {

            BCTinhHinhTD_XLD_GQInfo result = null;
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_TUNGAYGOC, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAYGOC, SqlDbType.DateTime),
                new SqlParameter(PARAM_TUNGAYMOI, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAYMOI, SqlDbType.DateTime),
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int)
            };
            parms[0].Value = infoQF.TuNgayGoc;
            parms[1].Value = infoQF.DenNgayGoc;
            parms[2].Value = infoQF.TuNgayMoi;
            parms[3].Value = infoQF.DenNgayMoi;
            parms[4].Value = infoQF.CoQuanID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "Dashboard_LanhDaoSuDungPhanMem_GiaiQuyetDon_GetByDate", parms))
                {
                    while (dr.Read())
                    {
                        result = new BCTinhHinhTD_XLD_GQInfo();
                        result.DaGQ = Utils.ConvertToInt32(dr["DaGQ"], 0);
                        result.ChuaBHGQ = Utils.ConvertToInt32(dr["ChuaBHGQ"], 0);
                        result.DaBHGQDonKN = Utils.ConvertToInt32(dr["DaBHGQDonKN"], 0);
                        result.DaBHGQDonTC = Utils.ConvertToInt32(dr["DaBHGQDonTC"], 0);
                        result.DaBHGQDonKNPA = Utils.ConvertToInt32(dr["DaBHGQDonKNPA"], 0);
                        result.DaBHGQ = Utils.ConvertToInt32(dr["DaBHGQ"], 0);
                        result.VuViecDongNguoi = Utils.ConvertToInt32(dr["VuViecDongNguoi"], 0);

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

        #endregion

        #region -- get chi tiet

        private DTXuLyInfo GetDSDonThu(SqlDataReader dr)
        {
            DTXuLyInfo info = new DTXuLyInfo();
            info.SoDonThu = Utils.ConvertToString(dr["SoDonThu"], String.Empty);
            info.NgayTiep = Utils.ConvertToDateTime(dr["NgayTiep"], DateTime.MinValue);
            info.NoiDungDon = Utils.ConvertToString(dr["NoiDungTiep"], String.Empty);
            info.TenLoaiKhieuTo = Utils.ConvertToString(dr["TenLoaiKhieuTo"], String.Empty);
            info.TenChuDon = Utils.ConvertToString(dr["HoTen"], String.Empty);
            info.DiaChiCT = Utils.ConvertToString(dr["DiaChiCT"], String.Empty);
            info.NgayNhapDonStr = Format.FormatDate(info.NgayTiep);
            return info;
        }
        String BAOCAO_TINHHINHTDXLDGQ_DSCHITIETDONTHU_TIEPDAN_GETBYDATE_NEW = "BaoCao_TinhHinhTDXLDGQ_DSChiTietDonThu_TiepDan_GetByDate_New";
        public IList<DTXuLyInfo> DSDonThu_TiepDan_GetByDate(QueryFilterInfo infoQF)
        {

            IList<DTXuLyInfo> result = new List<DTXuLyInfo>();
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_TUNGAYGOC, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAYGOC, SqlDbType.DateTime),
                new SqlParameter(PARAM_TUNGAYMOI, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAYMOI, SqlDbType.DateTime),
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_CAPID, SqlDbType.Int),
                new SqlParameter(PARAM_TINHID, SqlDbType.Int),
                new SqlParameter(PARAM_START, SqlDbType.Int),
                new SqlParameter(PARAM_END, SqlDbType.Int)
            };
            parms[0].Value = infoQF.TuNgayGoc;
            parms[1].Value = infoQF.DenNgayGoc;
            parms[2].Value = infoQF.TuNgayMoi;
            parms[3].Value = infoQF.DenNgayMoi;
            parms[4].Value = infoQF.CoQuanID;
            parms[5].Value = infoQF.CapID;
            parms[6].Value = infoQF.TinhID;
            parms[7].Value = infoQF.Start;
            parms[8].Value = infoQF.End;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, BAOCAO_TINHHINHTDXLDGQ_DSCHITIETDONTHU_TIEPDAN_GETBYDATE_NEW, parms))
                {
                    while (dr.Read())
                    {
                        DTXuLyInfo info = GetDSDonThu(dr);
                        if (info.NoiDungDon == string.Empty)
                        {
                            info.NoiDungDon = Utils.ConvertToString(dr["NoiDungDon"], string.Empty);
                        }
                        info.DonThuID = Utils.ConvertToInt32(dr["DonThuID"], 0);
                        info.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
                        info.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"], 0);
                        info.DiaChi = Utils.ConvertToString(dr["DiaChiCT"], string.Empty);
                        info.KetQuaID = Utils.ConvertToInt32(dr["KetQuaID"], 0);
                        info.HuongXuLyID = Utils.ConvertToInt32(dr["HuongGiaiQuyetID"], 0);
                        info.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        info.TenHuongGiaiQuyet = Utils.ConvertToString(dr["TenHuongGiaiQuyet"], string.Empty);
                        if (info.HuongXuLyID == 0)
                        {
                            info.KetQuaID_Str = "Chưa giải quyết";
                        }
                        else if (info.HuongXuLyID == (int)HuongGiaiQuyetEnum.DeXuatThuLy && info.StateID != 10)
                        {
                            info.KetQuaID_Str = "Đang giải quyết";
                        }
                        else
                        {
                            info.KetQuaID_Str = "Đã giải quyết";
                        }
                        result.Add(info);
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
        String BAOCAO_TINHHINHTDXLDGQ_DSCHITIETDONTHU_XULYDON_GETBYDATE_NEW = "BaoCao_TinhHinhTDXLDGQ_DSChiTietDonThu_XuLyDon_GetByDate_New";
        public IList<DTXuLyInfo> DSDonThu_XuLyDon_GetByDate(QueryFilterInfo infoQF)
        {

            IList<DTXuLyInfo> result = new List<DTXuLyInfo>();
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_TUNGAYGOC, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAYGOC, SqlDbType.DateTime),
                new SqlParameter(PARAM_TUNGAYMOI, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAYMOI, SqlDbType.DateTime),
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_LOAIXL, SqlDbType.Int),
                new SqlParameter(PARAM_CAPID, SqlDbType.Int),
                new SqlParameter(PARAM_TINHID, SqlDbType.Int),
                new SqlParameter(PARAM_START, SqlDbType.Int),
                new SqlParameter(PARAM_END, SqlDbType.Int)
            };
            parms[0].Value = infoQF.TuNgayGoc;
            parms[1].Value = infoQF.DenNgayGoc;
            parms[2].Value = infoQF.TuNgayMoi;
            parms[3].Value = infoQF.DenNgayMoi;
            parms[4].Value = infoQF.CoQuanID;
            parms[5].Value = infoQF.LoaiXL;
            parms[6].Value = infoQF.CapID;
            parms[7].Value = infoQF.TinhID;
            parms[8].Value = infoQF.Start;
            parms[9].Value = infoQF.End;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, BAOCAO_TINHHINHTDXLDGQ_DSCHITIETDONTHU_XULYDON_GETBYDATE_NEW, parms))
                {
                    while (dr.Read())
                    {
                        DTXuLyInfo info = GetDSDonThu(dr);
                        info.DonThuID = Utils.ConvertToInt32(dr["DonThuID"], 0);
                        info.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
                        info.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"], 0);
                        info.DiaChi = Utils.ConvertToString(dr["DiaChiCT"], string.Empty);
                        info.KetQuaID = Utils.ConvertToInt32(dr["KetQuaID"], 0);
                        info.HuongXuLyID = Utils.ConvertToInt32(dr["HuongGiaiQuyetID"], 0);
                        info.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        info.TenHuongGiaiQuyet = Utils.ConvertToString(dr["TenHuongGiaiQuyet"], string.Empty);
                        if (info.HuongXuLyID == 0)
                        {
                            info.KetQuaID_Str = "Chưa giải quyết";
                        }
                        else if (info.HuongXuLyID == (int)HuongGiaiQuyetEnum.DeXuatThuLy && info.StateID != 10)
                        {
                            info.KetQuaID_Str = "Đang giải quyết";
                        }
                        else
                        {
                            info.KetQuaID_Str = "Đã giải quyết";
                        }
                        result.Add(info);
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
        String BAOCAO_TINHHINHTDXLDGQ_DSCHITIETDONTHU_GIAIQUYETDON_GETBYDATE_NEW = "BaoCao_TinhHinhTDXLDGQ_DSChiTietDonThu_GiaiQuyetDon_GetByDate_New";
        public IList<DTXuLyInfo> DSDonThu_GiaiQuyetDon_GetByDate(QueryFilterInfo infoQF)
        {

            IList<DTXuLyInfo> result = new List<DTXuLyInfo>();
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_TUNGAYGOC, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAYGOC, SqlDbType.DateTime),
                new SqlParameter(PARAM_TUNGAYMOI, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAYMOI, SqlDbType.DateTime),
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_LOAIXL, SqlDbType.Int),
                new SqlParameter(PARAM_PTKQDUNG, SqlDbType.Int),
                new SqlParameter(PARAM_PTKQDUNGMOTPHAN, SqlDbType.Int),
                new SqlParameter(PARAM_PTKQSAI, SqlDbType.Int),
                new SqlParameter(PARAM_CAPID, SqlDbType.Int),
                new SqlParameter(PARAM_TINHID, SqlDbType.Int),
                new SqlParameter(PARAM_START, SqlDbType.Int),
                new SqlParameter(PARAM_END, SqlDbType.Int)
            };
            parms[0].Value = infoQF.TuNgayGoc;
            parms[1].Value = infoQF.DenNgayGoc;
            parms[2].Value = infoQF.TuNgayMoi;
            parms[3].Value = infoQF.DenNgayMoi;
            parms[4].Value = infoQF.CoQuanID;
            parms[5].Value = infoQF.LoaiXL;
            parms[6].Value = infoQF.PTKQDung;
            parms[7].Value = infoQF.PTKQDungMotPhan;
            parms[8].Value = infoQF.PTKQSai;
            parms[9].Value = infoQF.CapID;
            parms[10].Value = infoQF.TinhID;
            parms[11].Value = infoQF.Start;
            parms[12].Value = infoQF.End;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, BAOCAO_TINHHINHTDXLDGQ_DSCHITIETDONTHU_GIAIQUYETDON_GETBYDATE_NEW, parms))
                {
                    while (dr.Read())
                    {
                        DTXuLyInfo info = new DTXuLyInfo();
                        info.DonThuID = Utils.ConvertToInt32(dr["DonThuID"], 0);
                        info.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
                        info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanGiaiQuyetID"], 0);
                        info.SoDonThu = Utils.ConvertToString(dr["SoDonThu"], string.Empty);
                        string noiDungDon = Utils.ConvertToString(dr["NoiDungTiep"], string.Empty);
                        if (noiDungDon == string.Empty)
                        {
                            noiDungDon = Utils.ConvertToString(dr["NoiDungDon"], string.Empty);
                        }
                        info.NoiDungDon = noiDungDon;
                        info.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"], 0);
                        info.TenChuDon = Utils.ConvertToString(dr["HoTen"], string.Empty);
                        info.DiaChi = Utils.ConvertToString(dr["DiaChiCT"], string.Empty);
                        DateTime ngayNhapDon = Utils.ConvertToDateTime(dr["NgayTiep"], DateTime.MinValue);
                        if (ngayNhapDon == DateTime.MinValue)
                        {
                            ngayNhapDon = Utils.ConvertToDateTime(dr["NgayNhapDon"], DateTime.MinValue);
                        }
                        info.NgayNhapDon = ngayNhapDon;
                        info.NgayNhapDonStr = Format.FormatDate(info.NgayNhapDon);
                        info.TenLoaiKhieuTo = Utils.ConvertToString(dr["TenLoaiKhieuTo"], string.Empty);
                        info.KetQuaID = Utils.ConvertToInt32(dr["KetQuaID"], 0);
                        info.HuongXuLyID = Utils.ConvertToInt32(dr["HuongGiaiQuyetID"], 0);
                        info.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        info.TenHuongGiaiQuyet = Utils.ConvertToString(dr["TenHuongGiaiQuyet"], string.Empty);
                        if (info.HuongXuLyID == 0)
                        {
                            info.KetQuaID_Str = "Chưa giải quyết";
                        }
                        else if (info.HuongXuLyID == (int)HuongGiaiQuyetEnum.DeXuatThuLy && info.StateID != 10)
                        {
                            info.KetQuaID_Str = "Đang giải quyết";
                        }
                        else
                        {
                            info.KetQuaID_Str = "Đã giải quyết";
                        }
                        result.Add(info);
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

        #endregion

        #region thống kê DashBoard
        public int ThongKe_TinhHinhTDXLDGQ_Insert(DateTime tuNgay, DateTime denNgay)
        {
            object val;
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_TUNGAYGOC, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAYGOC, SqlDbType.DateTime)

            };
            parms[0].Value = tuNgay;
            parms[1].Value = denNgay;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, "ThongKe_TinhHinhTDXLDGQ_Insert", parms);
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
            return Utils.ConvertToInt32(val, 0);
        }

        public int ThongKe_TinhHinhGQD_Insert(DateTime tuNgay, DateTime denNgay)
        {
            object val;
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_TUNGAYGOC, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAYGOC, SqlDbType.DateTime)

            };
            parms[0].Value = tuNgay;
            parms[1].Value = denNgay;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, "ThongKe_TinhHinhGQD_Insert", parms);
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
            return Utils.ConvertToInt32(val, 0);
        }
        String DASHBOARD_THONGKEXULYDON_GETBYDATE_NEW = "Dashboard_ThongKeXuLyDon_GetByDate_New";
        public IList<BCTongHopXuLyInfo> DashBoard_ThongKeXuLyDon_GetByDate(QueryFilterInfo infoQF)
        {

            IList<BCTongHopXuLyInfo> result = new List<BCTongHopXuLyInfo>();
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_TUNGAYINT, SqlDbType.Int),
                new SqlParameter(PARAM_DENNGAYINT, SqlDbType.Int)
            };
            parms[0].Value = infoQF.TuNgayGoc;
            parms[1].Value = infoQF.DenNgayGoc;
            parms[2].Value = Utils.ConvertToInt32(infoQF.TuNgayGoc.ToString("yyyyMMdd"), 0);
            parms[3].Value = Utils.ConvertToInt32(infoQF.DenNgayGoc.ToString("yyyyMMdd"), 0);

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, DASHBOARD_THONGKEXULYDON_GETBYDATE_NEW, parms))
                {
                    while (dr.Read())
                    {
                        BCTongHopXuLyInfo info = new BCTongHopXuLyInfo();
                        info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        info.CapID = Utils.ConvertToInt32(dr["CapID"], 0);
                        info.SLTiepCongDan = Utils.ConvertToInt32(dr["SLTiepCongDan"], 0);
                        info.XLDTongSo = Utils.ConvertToInt32(dr["XLDTongSo"], 0);
                        info.XLDDaXuLy = Utils.ConvertToInt32(dr["XLDDaXuLy"], 0);
                        info.XLDChuaXuLy = Utils.ConvertToInt32(dr["XLDChuaXuLy"], 0);
                        info.XLDDaXuLyTrongHan = Utils.ConvertToInt32(dr["XLDDaXuLyTrongHan"], 0);
                        info.XLDDaXuLyQuaHan = Utils.ConvertToInt32(dr["XLDDaXuLyQuaHan"], 0);
                        info.XLDKhieuKienDN = Utils.ConvertToInt32(dr["XLDKhieuKienDN"], 0);
                        result.Add(info);
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
        String DASHBOARD_THONGKEGIAIQUYETDON_GETBYDATE_NEW = "Dashboard_ThongKeGiaiQuyetDon_GetByDate_New";
        public IList<BCTongHopXuLyInfo> DashBoard_ThongKeGiaiQuyetDon_GetByDate(QueryFilterInfo infoQF)
        {

            IList<BCTongHopXuLyInfo> result = new List<BCTongHopXuLyInfo>();
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_TUNGAYINT, SqlDbType.Int),
                new SqlParameter(PARAM_DENNGAYINT, SqlDbType.Int)
            };
            parms[0].Value = Utils.ConvertToInt32(infoQF.TuNgayGoc.ToString("yyyyMMdd"), 0);
            parms[1].Value = Utils.ConvertToInt32(infoQF.DenNgayGoc.ToString("yyyyMMdd"), 0);
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, DASHBOARD_THONGKEGIAIQUYETDON_GETBYDATE_NEW, parms))
                {
                    while (dr.Read())
                    {
                        BCTongHopXuLyInfo info = new BCTongHopXuLyInfo();
                        info.CoQuanGiaiQuyetID = Utils.ConvertToInt32(dr["CoQuanGiaiQuyetID"], 0);
                        info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        info.GQDTongSo = Utils.ConvertToInt32(dr["GQDTongSo"], 0);
                        info.GQDChuaGQ = Utils.ConvertToInt32(dr["GQDChuaGQ"], 0);
                        info.GQDDangGQ = Utils.ConvertToInt32(dr["GQDDangGQ"], 0);
                        info.GQDDangGQTrongHan = Utils.ConvertToInt32(dr["GQDDangGQTrongHan"], 0);
                        info.GQDDangGQQuaHan = Utils.ConvertToInt32(dr["GQDDangGQQuaHan"], 0);
                        info.GQDDaGQ = Utils.ConvertToInt32(dr["GQDDaGQ"], 0);
                        info.KQKNDung = Utils.ConvertToInt32(dr["KQKNDung"], 0);
                        info.KQKNDungMotPhan = Utils.ConvertToInt32(dr["KQKNDungMotPhan"], 0);
                        info.KQKNSai = Utils.ConvertToInt32(dr["KQKNSai"], 0);

                        info.GQDKNDangGQ = Utils.ConvertToInt32(dr["GQDKNDangGQ"], 0);
                        info.GQDTCDangGQ = Utils.ConvertToInt32(dr["GQDTCDangGQ"], 0);
                        info.GQDKNPADangGQ = Utils.ConvertToInt32(dr["GQDKNPADangGQ"], 0);

                        info.GQDKNDaGQ = Utils.ConvertToInt32(dr["GQDKNDaGQ"], 0);
                        info.GQDTCDaGQ = Utils.ConvertToInt32(dr["GQDTCDaGQ"], 0);
                        info.GQDKNPADaGQ = Utils.ConvertToInt32(dr["GQDKNPADaGQ"], 0);

                        info.GQDKNChuaGQ = Utils.ConvertToInt32(dr["GQDKNChuaGQ"], 0);
                        info.GQDTCChuaGQ = Utils.ConvertToInt32(dr["GQDTCChuaGQ"], 0);
                        info.GQDKNPAChuaGQ = Utils.ConvertToInt32(dr["GQDKNPAChuaGQ"], 0);
                        result.Add(info);
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

        String DASHBOARD_THONGKETONGHOP = "Dashboard_GetByDate";
        public IList<BCTongHopXuLyInfo> DashBoard_ThongKeTongHop(QueryFilterInfo infoQF, List<CoQuanInfo> coQuans)
        {
            var pList = new SqlParameter("@ListCoQuanID", SqlDbType.Structured);
            pList.TypeName = "dbo.IntList";
            var tbCoQuanID = new DataTable();
            tbCoQuanID.Columns.Add("CoQuanID", typeof(string));
            coQuans.ForEach(x => tbCoQuanID.Rows.Add(x.CoQuanID));

            IList<BCTongHopXuLyInfo> result = new List<BCTongHopXuLyInfo>();
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_TUNGAYINT, SqlDbType.Int),
                new SqlParameter(PARAM_DENNGAYINT, SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                pList
            };
            parms[0].Value = Utils.ConvertToInt32(infoQF.TuNgayGoc.ToString("yyyyMMdd"), 0);
            parms[1].Value = Utils.ConvertToInt32(infoQF.DenNgayGoc.ToString("yyyyMMdd"), 0);
            parms[2].Value = infoQF.TuNgayGoc;
            parms[3].Value = infoQF.DenNgayGoc;
            parms[4].Value = tbCoQuanID;

            try
            {
                var st1 = new Stopwatch();
                st1.Start();
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, DASHBOARD_THONGKETONGHOP, parms))
                {
                    st1.Stop();
                    var t1 = st1.Elapsed;
                    while (dr.Read())
                    {
                        BCTongHopXuLyInfo info = new BCTongHopXuLyInfo();
                        info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        info.CapID = Utils.ConvertToInt32(dr["CapID"], 0);
                        info.SLTiepCongDan = Utils.ConvertToInt32(dr["SLTiepCongDan"], 0);
                        info.XLDTongSo = Utils.ConvertToInt32(dr["XLDTongSo"], 0);
                        info.XLDDaXuLy = Utils.ConvertToInt32(dr["XLDDaXuLy"], 0);

                        //info.CoQuanGiaiQuyetID = Utils.ConvertToInt32(dr["CoQuanGiaiQuyetID"], 0);
                        //info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        info.GQDTongSo = Utils.ConvertToInt32(dr["GQDTongSo"], 0);
                        info.GQDChuaGQ = Utils.ConvertToInt32(dr["GQDChuaGQ"], 0);
                        info.GQDDangGQ = Utils.ConvertToInt32(dr["GQDDangGQ"], 0);
                        info.GQDDangGQTrongHan = Utils.ConvertToInt32(dr["GQDDangGQTrongHan"], 0);
                        info.GQDDangGQQuaHan = Utils.ConvertToInt32(dr["GQDDangGQQuaHan"], 0);
                        info.GQDDaGQ = Utils.ConvertToInt32(dr["GQDDaGQ"], 0);
                        info.KQKNDung = Utils.ConvertToInt32(dr["KQKNDung"], 0);
                        info.KQKNDungMotPhan = Utils.ConvertToInt32(dr["KQKNDungMotPhan"], 0);
                        info.KQKNSai = Utils.ConvertToInt32(dr["KQKNSai"], 0);

                        info.GQDKNDangGQ = Utils.ConvertToInt32(dr["GQDKNDangGQ"], 0);
                        info.GQDTCDangGQ = Utils.ConvertToInt32(dr["GQDTCDangGQ"], 0);
                        info.GQDKNPADangGQ = Utils.ConvertToInt32(dr["GQDKNPADangGQ"], 0);

                        info.GQDKNDaGQ = Utils.ConvertToInt32(dr["GQDKNDaGQ"], 0);
                        info.GQDTCDaGQ = Utils.ConvertToInt32(dr["GQDTCDaGQ"], 0);
                        info.GQDKNPADaGQ = Utils.ConvertToInt32(dr["GQDKNPADaGQ"], 0);

                        info.GQDKNChuaGQ = Utils.ConvertToInt32(dr["GQDKNChuaGQ"], 0);
                        info.GQDTCChuaGQ = Utils.ConvertToInt32(dr["GQDTCChuaGQ"], 0);
                        info.GQDKNPAChuaGQ = Utils.ConvertToInt32(dr["GQDKNPAChuaGQ"], 0);
                        result.Add(info);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }
        #endregion
    }
}
