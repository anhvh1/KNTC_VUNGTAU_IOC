using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Ultilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.DAL.KNTC
{
    public class BCTongHopKQTDXLD
    {
        #region -- store proc
        private const string BAOCAO_TONGHOPKQXLD_DSCANBO_GIAIQUYETDON_GETBYDATE = @"BaoCao_TongHopKQXLD_DSCanBo_GiaiQuyetDon_GetByDate";
        private const string BAOCAO_TONGHOPKQXLD_DSCANBO_XULYDON_GETBYDATE = @"BaoCao_TongHopKQXLD_DSCanBo_XuLyDon_GetByDate";
        private const string BAOCAO_TONGHOPKQXLD_DSCOQUAN_GIAIQUYETDON_GETBYDATE = @"BaoCao_TongHopKQXLD_DSCoQuan_GiaiQuyetDon_GetByDate";
        private const string BAOCAO_TONGHOPKQXLD_DSCOQUAN_XULYDON_GETBYDATE = @"BaoCao_TongHopKQXLD_DSCoQuan_XuLyDon_GetByDate";
        private const string BAOCAO_TONGHOPKQXLD_DSPHONGBAN_GIAIQUYETDON_GETBYDATE = @"BaoCao_TongHopKQXLD_DSPhongBan_GiaiQuyetDon_GetByDate";

        private const string BAOCAO_TONGHOPKQXLD_DSCANBO_XULYDON_GETBYDATE_NEW = @"BaoCao_TongHopKQXLD_DSCanBo_XuLyDon_GetByDate_Edit";
        private const string BAOCAO_TONGHOPKQXLD_DSCANBO_GIAIQUYETDON_GETBYDATE_NEW = @"BaoCao_TongHopKQXLD_DSCanBo_GiaiQuyetDon_GetByDate_Edit";
        private const string BAOCAO_TONGHOPKQXLD_DSPHONGBAN_GIAIQUYETDON_GETBYDATE_NEW = @"BaoCao_TongHopKQXLD_DSPhongBan_GiaiQuyetDon_GetByDate_New";
        private const string BAOCAO_TONGHOPKQXLD_DSPHONGBAN_GIAIQUYETDON_GETBYDATE_NEW_V2 = @"BaoCao_TongHopKQXLD_DSPhongBan_GiaiQuyetDon_GetByDate_New_v2";
        private const string BAOCAO_TONGHOPKQXLD_DSCOQUAN_GIAIQUYETDON_GETBYDATE_NEW = @"BaoCao_TongHopKQXLD_DSCoQuan_GiaiQuyetDon_GetByDate_New";
        private const string BAOCAO_TONGHOPKQXLD_DSCOQUAN_GIAIQUYETDON_GETBYDATE_NEW_V2 = @"BaoCao_TongHopKQXLD_DSCoQuan_GiaiQuyetDon_GetByDate_New_v2";

        private const string BAOCAO_TONGHOPKQXLD_DSCHITIETDONTHU_TIEPDAN_GETBYDATE = @"BaoCao_TongHopKQXLD_DSChiTietDonThu_TiepDan_GetByDate";
        private const string BAOCAO_TONGHOPKQXLD_DSCHITIETDONTHU_XULYDON_GETBYDATE = @"BaoCao_TongHopKQXLD_DSChiTietDonThu_XuLyDon_GetByDate";
        private const string BAOCAO_TONGHOPKQXLD_DSCHITIETDONTHU_GIAIQUYETDON_GETBYDATE = @"BaoCao_TongHopKQXLD_DSChiTietDonThu_GiaiQuyetDon_GetByDate";

        private const string BAOCAO_TONGHOPKQXLD_DSCHITIETDONTHU_CANBO_GIAIQUYETDON_GETBYDATE = @"BaoCao_TongHopKQXLD_DSChiTietDonThu_CanBo_GiaiQuyetDon_GetByDate";
        private const string BAOCAO_TONGHOPKQXLD_DSCHITIETDONTHU_PHONGBAN_GIAIQUYETDON_GETBYDATE = @"BaoCao_TongHopKQXLD_DSChiTietDonThu_PhongBan_GiaiQuyetDon_GetByDate";

        #endregion

        #region -- param
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
        private const string PARAM_PTKDSAI = "@PTKDSai";
        private const string PARAM_LOAIDON = "@LoaiDon";
        private const string PARAM_START = "@Start";
        private const string PARAM_END = "@End";

        #endregion

        #region -- get data bao cao

        public IList<BCTongHopXuLyInfo> DSCanBo_GiaiQuyetDon_GetByDate(QueryFilterInfo infoQF)
        {

            IList<BCTongHopXuLyInfo> result = new List<BCTongHopXuLyInfo>();
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_TUNGAYGOC, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAYGOC, SqlDbType.DateTime),
                new SqlParameter(PARAM_TUNGAYMOI, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAYMOI, SqlDbType.DateTime),
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_LOAIDON, SqlDbType.Int)
            };
            parms[0].Value = infoQF.TuNgayGoc;
            parms[1].Value = infoQF.DenNgayGoc;
            parms[2].Value = infoQF.TuNgayMoi;
            parms[3].Value = infoQF.DenNgayMoi;
            parms[4].Value = infoQF.CoQuanID;
            parms[5].Value = infoQF.LoaiDon;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, BAOCAO_TONGHOPKQXLD_DSCANBO_GIAIQUYETDON_GETBYDATE_NEW, parms))
                {
                    while (dr.Read())
                    {
                        BCTongHopXuLyInfo info = new BCTongHopXuLyInfo();
                        info.PhongBanID = Utils.ConvertToInt32(dr["PhongBanID"], 0);
                        info.TenPhongBan = Utils.ConvertToString(dr["TenPhongBan"], String.Empty);
                        info.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        info.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], String.Empty);
                        info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        info.GQDTongSo = Utils.ConvertToInt32(dr["GQDTongSo"], 0);
                        info.GQDChuaGQ = Utils.ConvertToInt32(dr["GQDChuaGQ"], 0);
                        info.GQDDangGQ = Utils.ConvertToInt32(dr["GQDDangGQ"], 0);
                        info.GQDDangGQTrongHan = Utils.ConvertToInt32(dr["GQDDangGQTrongHan"], 0);
                        info.GQDDangGQQuaHan = Utils.ConvertToInt32(dr["GQDDangGQQuaHan"], 0);
                        info.GQDDaGQ = Utils.ConvertToInt32(dr["GQDDaGQ"], 0);
                        info.DaCoBaoCao = Utils.ConvertToInt32(dr["DaCoBaoCao"], 0);
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

        public IList<BCTongHopXuLyInfo> DSPhongBan_GiaiQuyetDon_GetByDate(QueryFilterInfo infoQF)
        {

            IList<BCTongHopXuLyInfo> result = new List<BCTongHopXuLyInfo>();
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_TUNGAYGOC, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAYGOC, SqlDbType.DateTime),
                new SqlParameter(PARAM_TUNGAYMOI, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAYMOI, SqlDbType.DateTime),
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_LOAIDON, SqlDbType.Int)
            };
            parms[0].Value = infoQF.TuNgayGoc;
            parms[1].Value = infoQF.DenNgayGoc;
            parms[2].Value = infoQF.TuNgayMoi;
            parms[3].Value = infoQF.DenNgayMoi;
            parms[4].Value = infoQF.CoQuanID;
            parms[5].Value = infoQF.LoaiDon;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, BAOCAO_TONGHOPKQXLD_DSPHONGBAN_GIAIQUYETDON_GETBYDATE_NEW_V2, parms))
                {
                    while (dr.Read())
                    {
                        BCTongHopXuLyInfo info = new BCTongHopXuLyInfo();
                        info.PhongBanID = Utils.ConvertToInt32(dr["PhongBanID"], 0);
                        info.GQDTongSo = Utils.ConvertToInt32(dr["GQDTongSo"], 0);
                        info.GQDGiaoPhongNV = Utils.ConvertToInt32(dr["GQDGiaoPhongNV"], 0);
                        info.GQDChuaGQ = Utils.ConvertToInt32(dr["GQDChuaGQ"], 0);
                        info.GQDDangGQ = Utils.ConvertToInt32(dr["GQDDangGQ"], 0);
                        info.GQDDangGQTrongHan = Utils.ConvertToInt32(dr["GQDDangGQTrongHan"], 0);
                        info.GQDDangGQQuaHan = Utils.ConvertToInt32(dr["GQDDangGQQuaHan"], 0);
                        info.GQDDaGQ = Utils.ConvertToInt32(dr["GQDDaGQ"], 0);
                        info.DaCoBaoCao = Utils.ConvertToInt32(dr["DaCoBaoCao"], 0);
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

        public IList<BCTongHopXuLyInfo> DSCoQuan_GiaiQuyetDon_GetByDate(QueryFilterInfo infoQF)
        {

            IList<BCTongHopXuLyInfo> result = new List<BCTongHopXuLyInfo>();
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_TUNGAYGOC, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAYGOC, SqlDbType.DateTime),
                new SqlParameter(PARAM_TUNGAYMOI, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAYMOI, SqlDbType.DateTime),
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_LOAIDON, SqlDbType.Int)
            };
            parms[0].Value = infoQF.TuNgayGoc;
            parms[1].Value = infoQF.DenNgayGoc;
            parms[2].Value = infoQF.TuNgayMoi;
            parms[3].Value = infoQF.DenNgayMoi;
            parms[4].Value = infoQF.CoQuanID;
            parms[5].Value = infoQF.LoaiDon;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, BAOCAO_TONGHOPKQXLD_DSCOQUAN_GIAIQUYETDON_GETBYDATE_NEW_V2, parms))
                {
                    while (dr.Read())
                    {
                        BCTongHopXuLyInfo info = new BCTongHopXuLyInfo();
                        info.CoQuanGiaiQuyetID = Utils.ConvertToInt32(dr["CoQuanGiaiQuyetID"], 0);
                        info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        info.CQPhoiHopID = Utils.ConvertToInt32(dr["CQPhoiHopID"], 0);
                        info.GQDTongSo = Utils.ConvertToInt32(dr["GQDTongSo"], 0);
                        info.GQDGiaoPhongNV = Utils.ConvertToInt32(dr["GQDGiaoPhongNV"], 0);
                        info.GQDChuaGQ = Utils.ConvertToInt32(dr["GQDChuaGQ"], 0);
                        info.GQDDangGQ = Utils.ConvertToInt32(dr["GQDDangGQ"], 0);
                        info.GQDDangGQTrongHan = Utils.ConvertToInt32(dr["GQDDangGQTrongHan"], 0);
                        info.GQDDangGQQuaHan = Utils.ConvertToInt32(dr["GQDDangGQQuaHan"], 0);
                        info.GQDDaGQ = Utils.ConvertToInt32(dr["GQDDaGQ"], 0);
                        info.DaCoBaoCao = Utils.ConvertToInt32(dr["DaCoBaoCao"], 0);
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

        public IList<BCTongHopXuLyInfo> DSCanBo_XuLyDon_GetByDate(QueryFilterInfo infoQF)
        {

            IList<BCTongHopXuLyInfo> result = new List<BCTongHopXuLyInfo>();
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_TUNGAYGOC, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAYGOC, SqlDbType.DateTime),
                new SqlParameter(PARAM_TUNGAYMOI, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAYMOI, SqlDbType.DateTime),
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_LOAIDON, SqlDbType.Int)
            };
            parms[0].Value = infoQF.TuNgayGoc;
            parms[1].Value = infoQF.DenNgayGoc;
            parms[2].Value = infoQF.TuNgayMoi;
            parms[3].Value = infoQF.DenNgayMoi;
            parms[4].Value = infoQF.CoQuanID;
            parms[5].Value = infoQF.LoaiDon;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, BAOCAO_TONGHOPKQXLD_DSCANBO_XULYDON_GETBYDATE_NEW, parms))
                {
                    while (dr.Read())
                    {
                        BCTongHopXuLyInfo info = new BCTongHopXuLyInfo();
                        info.PhongBanID = Utils.ConvertToInt32(dr["PhongBanID"], 0);
                        info.TenPhongBan = Utils.ConvertToString(dr["TenPhongBan"], String.Empty);
                        info.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        info.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], String.Empty);
                        info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);

                        info.SLTiepCongDan = Utils.ConvertToInt32(dr["SLTiepCongDan"], 0);
                        info.XLDTongSo = Utils.ConvertToInt32(dr["XLDTongSo"], 0);
                        info.XLDDaXuLy = Utils.ConvertToInt32(dr["XLDDaXuLy"], 0);
                        info.XLDChuaXuLy = Utils.ConvertToInt32(dr["XLDChuaXuLy"], 0);
                        info.XLDDaXuLyTrongHan = Utils.ConvertToInt32(dr["XLDDaXuLyTrongHan"], 0);
                        info.XLDDaXuLyQuaHan = Utils.ConvertToInt32(dr["XLDDaXuLyQuaHan"], 0);
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

        public IList<BCTongHopXuLyInfo> DSCoQuan_XuLyDon_GetByDate(QueryFilterInfo infoQF)
        {

            IList<BCTongHopXuLyInfo> result = new List<BCTongHopXuLyInfo>();
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_TUNGAYGOC, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAYGOC, SqlDbType.DateTime),
                new SqlParameter(PARAM_TUNGAYMOI, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAYMOI, SqlDbType.DateTime),
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_LOAIDON, SqlDbType.Int)
            };
            parms[0].Value = infoQF.TuNgayGoc;
            parms[1].Value = infoQF.DenNgayGoc;
            parms[2].Value = infoQF.TuNgayMoi;
            parms[3].Value = infoQF.DenNgayMoi;
            parms[4].Value = infoQF.CoQuanID;
            parms[5].Value = infoQF.LoaiDon;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, BAOCAO_TONGHOPKQXLD_DSCOQUAN_XULYDON_GETBYDATE, parms))
                {
                    while (dr.Read())
                    {
                        BCTongHopXuLyInfo info = new BCTongHopXuLyInfo();
                        info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);

                        info.SLTiepCongDan = Utils.ConvertToInt32(dr["SLTiepCongDan"], 0);
                        info.XLDTongSo = Utils.ConvertToInt32(dr["XLDTongSo"], 0);
                        info.XLDDaXuLy = Utils.ConvertToInt32(dr["XLDDaXuLy"], 0);
                        info.XLDChuaXuLy = Utils.ConvertToInt32(dr["XLDChuaXuLy"], 0);
                        info.XLDDaXuLyTrongHan = Utils.ConvertToInt32(dr["XLDDaXuLyTrongHan"], 0);
                        info.XLDDaXuLyQuaHan = Utils.ConvertToInt32(dr["XLDDaXuLyQuaHan"], 0);
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

        #region -- get chi tiet

        private BCTongHopXuLyInfo GetDSDonThu(SqlDataReader dr)
        {
            BCTongHopXuLyInfo info = new BCTongHopXuLyInfo();
            info.SoDon = Utils.ConvertToString(dr["SoDon"], String.Empty);
            info.NgayTiep = Utils.ConvertToDateTime(dr["NgayTiep"], DateTime.MinValue);
            info.NoiDungTiep = Utils.ConvertToString(dr["NoiDungTiep"], String.Empty);
            info.TenLoaiKhieuTo = Utils.ConvertToString(dr["TenLoaiKhieuTo"], String.Empty);
            info.HoTen = Utils.ConvertToString(dr["HoTen"], String.Empty);
            info.DiaChiCT = Utils.ConvertToString(dr["DiaChiCT"], String.Empty);
            info.NgayTiepStr = Format.FormatDate(info.NgayTiep);
            return info;
        }

        String BAOCAO_TONGHOPKQXLD_DSCHITIETDONTHU_TIEPDAN_GETBYDATE_NEW = "BaoCao_TongHopKQXLD_DSChiTietDonThu_TiepDan_GetByDate_New";
        public IList<BCTongHopXuLyInfo> DSDonThu_TiepDan_GetByDate(QueryFilterInfo infoQF)
        {

            IList<BCTongHopXuLyInfo> result = new List<BCTongHopXuLyInfo>();
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_TUNGAYGOC, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAYGOC, SqlDbType.DateTime),
                new SqlParameter(PARAM_TUNGAYMOI, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAYMOI, SqlDbType.DateTime),
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_PHONGBANID, SqlDbType.Int),
                new SqlParameter(PARAM_CANBOID, SqlDbType.Int),
                new SqlParameter(PARAM_LOAIDON, SqlDbType.Int),
                new SqlParameter(PARAM_START, SqlDbType.Int),
                new SqlParameter(PARAM_END, SqlDbType.Int)
            };
            parms[0].Value = infoQF.TuNgayGoc;
            parms[1].Value = infoQF.DenNgayGoc;
            parms[2].Value = infoQF.TuNgayMoi;
            parms[3].Value = infoQF.DenNgayMoi;
            parms[4].Value = infoQF.CoQuanID;
            parms[5].Value = infoQF.PhongBanID;
            parms[6].Value = infoQF.CanBoID;
            parms[7].Value = infoQF.LoaiDon;
            parms[8].Value = infoQF.Start;
            parms[9].Value = infoQF.End;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, BAOCAO_TONGHOPKQXLD_DSCHITIETDONTHU_TIEPDAN_GETBYDATE_NEW, parms))
                {
                    while (dr.Read())
                    {
                        BCTongHopXuLyInfo info = GetDSDonThu(dr);
                        info.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
                        info.DonThuID = Utils.ConvertToInt32(dr["DonThuID"], 0);
                        info.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"], 0);
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

        String BAOCAO_TONGHOPKQXLD_DSCHITIETDONTHU_XULYDON_GETBYDATE_NEW = "BaoCao_TongHopKQXLD_DSChiTietDonThu_XuLyDon_GetByDate_New";
        public IList<BCTongHopXuLyInfo> DSDonThu_XuLyDon_GetByDate(QueryFilterInfo infoQF)
        {

            IList<BCTongHopXuLyInfo> result = new List<BCTongHopXuLyInfo>();
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_TUNGAYGOC, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAYGOC, SqlDbType.DateTime),
                new SqlParameter(PARAM_TUNGAYMOI, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAYMOI, SqlDbType.DateTime),
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_PHONGBANID, SqlDbType.Int),
                new SqlParameter(PARAM_CANBOID, SqlDbType.Int),
                new SqlParameter(PARAM_LOAIXL, SqlDbType.Int),
                new SqlParameter(PARAM_LOAIDON, SqlDbType.Int),
                new SqlParameter(PARAM_START, SqlDbType.Int),
                new SqlParameter(PARAM_END, SqlDbType.Int)
            };
            parms[0].Value = infoQF.TuNgayGoc;
            parms[1].Value = infoQF.DenNgayGoc;
            parms[2].Value = infoQF.TuNgayMoi;
            parms[3].Value = infoQF.DenNgayMoi;
            parms[4].Value = infoQF.CoQuanID;
            parms[5].Value = infoQF.PhongBanID;
            parms[6].Value = infoQF.CanBoID;
            parms[7].Value = infoQF.LoaiXL;
            parms[8].Value = infoQF.LoaiDon;
            parms[9].Value = infoQF.Start;
            parms[10].Value = infoQF.End;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, BAOCAO_TONGHOPKQXLD_DSCHITIETDONTHU_XULYDON_GETBYDATE_NEW, parms))
                {
                    while (dr.Read())
                    {
                        BCTongHopXuLyInfo info = GetDSDonThu(dr);
                        info.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
                        info.DonThuID = Utils.ConvertToInt32(dr["DonThuID"], 0);
                        info.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"], 0);
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

        String BAOCAO_TONGHOPKQXLD_DSCHITIETDONTHU_GIAIQUYETDON_GETBYDATE_NEW_V2 = "BaoCao_TongHopKQXLD_DSChiTietDonThu_GiaiQuyetDon_GetByDate_New_v2";
        public IList<BCTongHopXuLyInfo> DSDonThu_GiaiQuyetDon_GetByDate(QueryFilterInfo infoQF)
        {

            IList<BCTongHopXuLyInfo> result = new List<BCTongHopXuLyInfo>();
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_TUNGAYGOC, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAYGOC, SqlDbType.DateTime),
                new SqlParameter(PARAM_TUNGAYMOI, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAYMOI, SqlDbType.DateTime),
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_PHONGBANID, SqlDbType.Int),
                new SqlParameter(PARAM_CANBOID, SqlDbType.Int),
                new SqlParameter(PARAM_LOAIXL, SqlDbType.Int),
                new SqlParameter(PARAM_LOAIDON, SqlDbType.Int),
                new SqlParameter(PARAM_START, SqlDbType.Int),
                new SqlParameter(PARAM_END, SqlDbType.Int)
            };
            parms[0].Value = infoQF.TuNgayGoc;
            parms[1].Value = infoQF.DenNgayGoc;
            parms[2].Value = infoQF.TuNgayMoi;
            parms[3].Value = infoQF.DenNgayMoi;
            parms[4].Value = infoQF.CoQuanID;
            parms[5].Value = infoQF.PhongBanID;
            parms[6].Value = infoQF.CanBoID;
            parms[7].Value = infoQF.LoaiXL;
            parms[8].Value = infoQF.LoaiDon;
            parms[9].Value = infoQF.Start;
            parms[10].Value = infoQF.End;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, BAOCAO_TONGHOPKQXLD_DSCHITIETDONTHU_GIAIQUYETDON_GETBYDATE_NEW_V2, parms))
                {
                    while (dr.Read())
                    {
                        BCTongHopXuLyInfo info = GetDSDonThu(dr);
                        info.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
                        info.DonThuID = Utils.ConvertToInt32(dr["DonThuID"], 0);
                        info.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"], 0);
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
        String BAOCAO_TONGHOPKQXLD_DSCHITIETDONTHU_CANBO_GIAIQUYETDON_GETBYDATE_NEW = "BaoCao_TongHopKQXLD_DSChiTietDonThu_CanBo_GiaiQuyetDon_GetByDate_Edit";
        public IList<BCTongHopXuLyInfo> DSDonThu_CanBo_GiaiQuyetDon_GetByDate(QueryFilterInfo infoQF)
        {

            IList<BCTongHopXuLyInfo> result = new List<BCTongHopXuLyInfo>();
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_TUNGAYGOC, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAYGOC, SqlDbType.DateTime),
                new SqlParameter(PARAM_TUNGAYMOI, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAYMOI, SqlDbType.DateTime),
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_PHONGBANID, SqlDbType.Int),
                new SqlParameter(PARAM_CANBOID, SqlDbType.Int),
                new SqlParameter(PARAM_LOAIXL, SqlDbType.Int),
                new SqlParameter(PARAM_LOAIDON, SqlDbType.Int),
                new SqlParameter(PARAM_START, SqlDbType.Int),
                new SqlParameter(PARAM_END, SqlDbType.Int)
            };
            parms[0].Value = infoQF.TuNgayGoc;
            parms[1].Value = infoQF.DenNgayGoc;
            parms[2].Value = infoQF.TuNgayMoi;
            parms[3].Value = infoQF.DenNgayMoi;
            parms[4].Value = infoQF.CoQuanID;
            parms[5].Value = infoQF.PhongBanID;
            parms[6].Value = infoQF.CanBoID;
            parms[7].Value = infoQF.LoaiXL;
            parms[8].Value = infoQF.LoaiDon;
            parms[9].Value = infoQF.Start;
            parms[10].Value = infoQF.End;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, BAOCAO_TONGHOPKQXLD_DSCHITIETDONTHU_CANBO_GIAIQUYETDON_GETBYDATE_NEW, parms))
                {
                    while (dr.Read())
                    {
                        BCTongHopXuLyInfo info = GetDSDonThu(dr);
                        info.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
                        info.DonThuID = Utils.ConvertToInt32(dr["DonThuID"], 0);
                        info.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"], 0);
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

        String BAOCAO_TONGHOPKQXLD_DSCHITIETDONTHU_PHONGBAN_GIAIQUYETDON_GETBYDATE_NEW_V2 = "BaoCao_TongHopKQXLD_DSChiTietDonThu_PhongBan_GiaiQuyetDon_GetByDate_New_v2";
        public IList<BCTongHopXuLyInfo> DSDonThu_PhongBan_GiaiQuyetDon_GetByDate(QueryFilterInfo infoQF)
        {

            IList<BCTongHopXuLyInfo> result = new List<BCTongHopXuLyInfo>();
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_TUNGAYGOC, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAYGOC, SqlDbType.DateTime),
                new SqlParameter(PARAM_TUNGAYMOI, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAYMOI, SqlDbType.DateTime),
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_PHONGBANID, SqlDbType.Int),
                new SqlParameter(PARAM_CANBOID, SqlDbType.Int),
                new SqlParameter(PARAM_LOAIXL, SqlDbType.Int),
                new SqlParameter(PARAM_LOAIDON, SqlDbType.Int),
                new SqlParameter(PARAM_START, SqlDbType.Int),
                new SqlParameter(PARAM_END, SqlDbType.Int)
            };
            parms[0].Value = infoQF.TuNgayGoc;
            parms[1].Value = infoQF.DenNgayGoc;
            parms[2].Value = infoQF.TuNgayMoi;
            parms[3].Value = infoQF.DenNgayMoi;
            parms[4].Value = infoQF.CoQuanID;
            parms[5].Value = infoQF.PhongBanID;
            parms[6].Value = infoQF.CanBoID;
            parms[7].Value = infoQF.LoaiXL;
            parms[8].Value = infoQF.LoaiDon;
            parms[9].Value = infoQF.Start;
            parms[10].Value = infoQF.End;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, BAOCAO_TONGHOPKQXLD_DSCHITIETDONTHU_PHONGBAN_GIAIQUYETDON_GETBYDATE_NEW_V2, parms))
                {
                    while (dr.Read())
                    {
                        BCTongHopXuLyInfo info = GetDSDonThu(dr);
                        info.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
                        info.DonThuID = Utils.ConvertToInt32(dr["DonThuID"], 0);
                        info.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"], 0);
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
    }
}
