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
    public class BaoCaoPhanAnhKienNghi
    {
        //private const string GET_DONTHU_KN = "BaoCao2c_GetDonThuKN";
        private const string GET_BAO_CAO = "BaoCaoPhanAnhKienNghi";
        private const string GET_BAO_CAO_NEW = "BaoCaoPhanAnhKienNghi_New_v2";
        private const string GET_DONTHU_KN_BY_TINH = "BaoCaoPhanAnhKienNghi_GetDonThuKNByTinh";
        private const string GET_DSCHITIETDONTHU = "BaoCaoPhanAnhKienNghi_GetDSChiTietDonThu";
        private const string COUNT_DSCHITIETDONTHU = "BaoCaoPhanAnhKienNghi_CountDSChiTietDonThu";

        private const string INSERT = @"BaoCao2c_Insert";
        private const string UPDATE = @"BaoCao2c_Update";
        private const string GET_ALL_XA_BY_COQUANHUYEN = @"BaoCao2c_GetAllCapXaByHuyen";

        private const string PARM_BAOCAO2CID = @"BaoCao2cID";
        private const string PARM_CANBOID = @"CanBoID";
        private const string PARM_THANG = @"Thang";
        private const string PARM_NAM = @"Nam";
        private const string PARM_XAID = @"XaID";
        private const string PARM_TUNGAY = @"TuNgay";
        private const string PARM_DENNGAY = @"DenNgay";
        private const string PARM_NGAYNHAP = @"NgayNhap";
        private const string PARM_NGAYCHOT = @"NgayChot";
        private const string PARM_COT2 = @"Cot2";
        private const string PARM_COT3 = @"Cot3";
        private const string PARM_COT4 = @"Cot4";
        private const string PARM_COT5 = @"Cot5";
        private const string PARM_COT6 = @"Cot6";
        private const string PARM_COT7 = @"Cot7";
        private const string PARM_COT8 = @"Cot8";
        private const string PARM_COT9 = @"Cot9";
        private const string PARM_COT10 = @"Cot10";
        private const string PARM_COT11 = @"Cot11";
        private const string PARM_COT12 = @"Cot12";
        private const string PARM_COT13 = @"Cot13";
        private const string PARM_COT14 = @"Cot14";
        private const string PARM_COT15 = @"Cot15";
        private const string PARM_COT16 = @"Cot16";
        private const string PARM_COT17 = @"Cot17";
        private const string PARM_COT18 = @"Cot18";
        private const string PARM_COT19 = @"Cot19";
        private const string PARM_COT20 = @"Cot20";
        private const string PARM_COT21 = @"Cot21";
        private const string PARM_COT22 = @"Cot22";
        private const string PARM_COT23 = @"Cot23";
        private const string PARM_COT24 = @"Cot24";
        private const string PARM_COT25 = @"Cot25";
        private const string PARM_COT26 = @"Cot26";
        private const string PARM_COT27 = @"Cot27";
        private const string PARM_COT28 = @"Cot28";
        private const string PARM_GHICHU = @"GhiChu";

        private const string PARM_STARTDATE = "@StartDate";
        private const string PARM_ENDDATE = "@EndDate";
        private const string PARM_COQUANID = "@CoQuanID";
        private const string PARM_TINHID = "@TinhID";
        private const string PARAM_START = "@Start";
        private const string PARAM_END = "@End";

        private BaoCaoPhanAnhKienNghiDonThuInfo GetData(SqlDataReader rdr)
        {
            BaoCaoPhanAnhKienNghiDonThuInfo info = new BaoCaoPhanAnhKienNghiDonThuInfo();
            info.DatDaThu = Utils.ConvertToInt32(rdr["DatDaThu"], 0);
            info.DatPhaiThu = Utils.ConvertToInt32(rdr["DatPhaiThu"], 0);
            info.DonThuID = Utils.ConvertToInt32(rdr["DonThuID"], 0);
            info.HuongGiaiQuyetID = Utils.ConvertToInt32(rdr["HuongGiaiQuyetID"], 0);
            info.LoaiKetQuaID = Utils.ConvertToInt32(rdr["LoaiKetQuaID"], 0);
            info.LoaiThiHanhID = Utils.ConvertToInt32(rdr["LoaiThiHanhID"], 0);
            info.NgayNhapDon = Utils.ConvertToDateTime(rdr["NgayNhapDon"], DateTime.MinValue);
            info.NgayRaKQ = Utils.ConvertToDateTime(rdr["NgayRaKQ"], DateTime.MinValue);
            info.PhanTichKQID = Utils.ConvertToInt32(rdr["PhanTichKQ"], 0);
            info.KetQuaGQLan2 = Utils.ConvertToInt32(rdr["KetQuaGQLan2"], 0);
            info.SoDoiTuongBiXuLy = Utils.ConvertToInt32(rdr["SoDoiTuongBiXuLy"], 0);
            info.SoDoiTuongDaBiXuLy = Utils.ConvertToInt32(rdr["SoDoiTuongDaBiXuLy"], 0);
            info.SoNguoiDuocTraQuyenLoi = Utils.ConvertToInt32(rdr["SoNguoiDuocTraQuyenLoi"], 0);
            info.TienDaThu = Utils.ConvertToInt32(rdr["TienDaThu"], 0);
            info.TienPhaiThu = Utils.ConvertToInt32(rdr["TienPhaiThu"], 0);
            info.TrungDon = Utils.ConvertToBoolean(rdr["TrungDon"], false);
            info.SoLan = Utils.ConvertToInt32(rdr["SoLan"], 0);
            info.NgayThiHanh = Utils.ConvertToDateTime(rdr["NgayThiHanh"], DateTime.MinValue);
            info.ThiHanhID = Utils.ConvertToInt32(rdr["ThiHanhID"], 0);
            info.TrangThaiDonID = Utils.ConvertToInt32(rdr["TrangThaiDonID"], 0);
            info.RutDonID = Utils.ConvertToInt32(rdr["RutDonID"], 0);
            info.CQDaGiaiQuyet = Utils.ConvertToString(rdr["CQDaGiaiQuyetID"], string.Empty);
            info.CoQuanID = Utils.ConvertToInt32(rdr["CoQuanID"], 0);
            //info.CQTiepNhanID = Utils.ConvertToInt32(rdr["CQTiepNhanID"], 0);


            return info;
        }

        private BaoCaoPhanAnhKienNghiInfo GetData2c(SqlDataReader rdr)
        {
            BaoCaoPhanAnhKienNghiInfo info = new BaoCaoPhanAnhKienNghiInfo();
            info.XaID = Utils.ConvertToInt32(rdr["XaID"], 0);
            info.TenXa = Utils.ConvertToString(rdr["TenXa"], string.Empty);
            info.TenDayDu = Utils.ConvertToString(rdr["TenDayDu"], string.Empty);
            info.BaoCao2cID = Utils.ConvertToInt32(rdr["BaoCao2cID"], 0);
            info.Col2Data = Utils.ConvertToInt32(rdr["Cot2"], 0);
            info.Col3Data = Utils.ConvertToInt32(rdr["Cot3"], 0);
            info.Col4Data = Utils.ConvertToInt32(rdr["Cot4"], 0);
            info.Col5Data = Utils.ConvertToInt32(rdr["Cot5"], 0);
            info.Col6Data = Utils.ConvertToInt32(rdr["Cot6"], 0);
            info.Col7Data = Utils.ConvertToInt32(rdr["Cot7"], 0);
            info.Col8Data = Utils.ConvertToInt32(rdr["Cot8"], 0);
            info.Col9Data = Utils.ConvertToInt32(rdr["Cot9"], 0);
            info.Col10Data = Utils.ConvertToInt32(rdr["Cot10"], 0);
            info.Col11Data = Utils.ConvertToInt32(rdr["Cot11"], 0);
            info.Col12Data = Utils.ConvertToInt32(rdr["Cot12"], 0);
            info.Col13Data = Utils.ConvertToInt32(rdr["Cot13"], 0);
            info.Col14Data = Utils.ConvertToInt32(rdr["Cot14"], 0);
            info.Col15Data = Utils.ConvertToInt32(rdr["Cot15"], 0);
            info.Col16Data = Utils.GetDecimal(rdr["Cot16"], 0);
            info.Col17Data = Utils.ConvertToInt32(rdr["Cot17"], 0);
            info.Col18Data = Utils.GetDecimal(rdr["Cot18"], 0);
            info.Col19Data = Utils.ConvertToInt32(rdr["Cot19"], 0);
            info.Col20Data = Utils.ConvertToInt32(rdr["Cot20"], 0);
            info.Col21Data = Utils.ConvertToInt32(rdr["Cot21"], 0);
            info.Col22Data = Utils.ConvertToInt32(rdr["Cot22"], 0);
            info.Col23Data = Utils.ConvertToInt32(rdr["Cot23"], 0);
            info.Col24Data = Utils.ConvertToInt32(rdr["Cot24"], 0);
            info.Col25Data = Utils.ConvertToInt32(rdr["Cot25"], 0);
            info.Col26Data = Utils.ConvertToInt32(rdr["Cot26"], 0);
            info.Col27Data = Utils.ConvertToInt32(rdr["Cot27"], 0);
            info.Col28Data = Utils.ConvertToInt32(rdr["Cot28"], 0);
            info.GhiChu = Utils.ConvertToString(rdr["GhiChu"], string.Empty);
            return info;
        }
        private SqlParameter[] GetInsertParms()
        {
            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter(PARM_XAID,SqlDbType.Int),
                new SqlParameter(PARM_TUNGAY,SqlDbType.DateTime),
                new SqlParameter(PARM_DENNGAY,SqlDbType.DateTime),
                new SqlParameter(PARM_NGAYNHAP,SqlDbType.DateTime),
                new SqlParameter(PARM_COT2,SqlDbType.Int),
                new SqlParameter(PARM_COT3,SqlDbType.Int),
                new SqlParameter(PARM_COT4,SqlDbType.Int),
                new SqlParameter(PARM_COT5,SqlDbType.Int),
                new SqlParameter(PARM_COT6,SqlDbType.Int),
                new SqlParameter(PARM_COT7,SqlDbType.Int),
                new SqlParameter(PARM_COT8,SqlDbType.Int),
                new SqlParameter(PARM_COT9,SqlDbType.Int),
                new SqlParameter(PARM_COT10,SqlDbType.Int),
                new SqlParameter(PARM_COT11,SqlDbType.Int),
                new SqlParameter(PARM_COT12,SqlDbType.Int),
                new SqlParameter(PARM_COT13,SqlDbType.Int),
                new SqlParameter(PARM_COT14,SqlDbType.Int),
                new SqlParameter(PARM_COT15,SqlDbType.Int),
                new SqlParameter(PARM_COT16,SqlDbType.Decimal),
                new SqlParameter(PARM_COT17,SqlDbType.Int),
                new SqlParameter(PARM_COT18,SqlDbType.Decimal),
                new SqlParameter(PARM_COT19,SqlDbType.Int),
                new SqlParameter(PARM_COT20,SqlDbType.Int),
                new SqlParameter(PARM_COT21,SqlDbType.Int),
                new SqlParameter(PARM_COT22,SqlDbType.Int),
                new SqlParameter(PARM_COT23,SqlDbType.Int),
                new SqlParameter(PARM_COT24,SqlDbType.Int),
                new SqlParameter(PARM_COT25,SqlDbType.Int),
                new SqlParameter(PARM_COT26,SqlDbType.Int),
                new SqlParameter(PARM_COT27,SqlDbType.Int),
                new SqlParameter(PARM_COT28,SqlDbType.Int),
                new SqlParameter(PARM_GHICHU,SqlDbType.NVarChar,200),
                new SqlParameter(PARM_CANBOID,SqlDbType.Int),
                };
            return parms;
        }

        private void SetInsertParms(SqlParameter[] parms, BaoCaoPhanAnhKienNghiInfo cInfo)
        {

            parms[0].Value = cInfo.XaID;
            parms[1].Value = cInfo.TuNgay;
            parms[2].Value = cInfo.DenNgay;
            parms[3].Value = DateTime.Now;
            parms[4].Value = cInfo.Col2Data;
            parms[5].Value = cInfo.Col3Data;
            parms[6].Value = cInfo.Col4Data;
            parms[7].Value = cInfo.Col5Data;
            parms[8].Value = cInfo.Col6Data;
            parms[9].Value = cInfo.Col7Data;
            parms[10].Value = cInfo.Col8Data;
            parms[11].Value = cInfo.Col9Data;
            parms[12].Value = cInfo.Col10Data;
            parms[13].Value = cInfo.Col11Data;
            parms[14].Value = cInfo.Col12Data;
            parms[15].Value = cInfo.Col13Data;
            parms[16].Value = cInfo.Col14Data;
            parms[17].Value = cInfo.Col15Data;
            parms[18].Value = cInfo.Col16Data;
            parms[19].Value = cInfo.Col17Data;
            parms[20].Value = cInfo.Col18Data;
            parms[21].Value = cInfo.Col19Data;
            parms[22].Value = cInfo.Col20Data;
            parms[23].Value = cInfo.Col21Data;
            parms[24].Value = cInfo.Col22Data;
            parms[25].Value = cInfo.Col23Data;
            parms[26].Value = cInfo.Col24Data;
            parms[27].Value = cInfo.Col25Data;
            parms[28].Value = cInfo.Col26Data;
            parms[29].Value = cInfo.Col27Data;
            parms[30].Value = cInfo.Col28Data;
            parms[40].Value = cInfo.GhiChu;
            parms[41].Value = cInfo.CanBoID;
        }

        private SqlParameter[] GetUpdateParms()
        {
            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter(PARM_BAOCAO2CID,SqlDbType.Int),
                new SqlParameter(PARM_NGAYCHOT,SqlDbType.DateTime),
                new SqlParameter(PARM_COT2,SqlDbType.Int),
                new SqlParameter(PARM_COT3,SqlDbType.Int),
                new SqlParameter(PARM_COT4,SqlDbType.Int),
                new SqlParameter(PARM_COT5,SqlDbType.Int),
                new SqlParameter(PARM_COT6,SqlDbType.Int),
                new SqlParameter(PARM_COT7,SqlDbType.Int),
                new SqlParameter(PARM_COT8,SqlDbType.Int),
                new SqlParameter(PARM_COT9,SqlDbType.Int),
                new SqlParameter(PARM_COT10,SqlDbType.Int),
                new SqlParameter(PARM_COT11,SqlDbType.Int),
                new SqlParameter(PARM_COT12,SqlDbType.Int),
                new SqlParameter(PARM_COT13,SqlDbType.Int),
                new SqlParameter(PARM_COT14,SqlDbType.Int),
                new SqlParameter(PARM_COT15,SqlDbType.Int),
                new SqlParameter(PARM_COT16,SqlDbType.Decimal),
                new SqlParameter(PARM_COT17,SqlDbType.Int),
                new SqlParameter(PARM_COT18,SqlDbType.Decimal),
                new SqlParameter(PARM_COT19,SqlDbType.Int),
                new SqlParameter(PARM_COT20,SqlDbType.Int),
                new SqlParameter(PARM_COT21,SqlDbType.Int),
                new SqlParameter(PARM_COT22,SqlDbType.Int),
                new SqlParameter(PARM_COT23,SqlDbType.Int),
                new SqlParameter(PARM_COT24,SqlDbType.Int),
                new SqlParameter(PARM_COT25,SqlDbType.Int),
                new SqlParameter(PARM_COT26,SqlDbType.Int),
                new SqlParameter(PARM_COT27,SqlDbType.Int),
                new SqlParameter(PARM_COT28,SqlDbType.Int),
                new SqlParameter(PARM_GHICHU,SqlDbType.NVarChar,200),
                };
            return parms;
        }

        private void SetUpdateParms(SqlParameter[] parms, BaoCaoPhanAnhKienNghiInfo cInfo)
        {

            parms[0].Value = cInfo.BaoCao2cID;
            parms[1].Value = DateTime.Now;
            parms[2].Value = cInfo.Col2Data;
            parms[3].Value = cInfo.Col3Data;
            parms[4].Value = cInfo.Col4Data;
            parms[5].Value = cInfo.Col5Data;
            parms[6].Value = cInfo.Col6Data;
            parms[7].Value = cInfo.Col7Data;
            parms[8].Value = cInfo.Col8Data;
            parms[9].Value = cInfo.Col9Data;
            parms[10].Value = cInfo.Col10Data;
            parms[11].Value = cInfo.Col11Data;
            parms[12].Value = cInfo.Col12Data;
            parms[13].Value = cInfo.Col13Data;
            parms[14].Value = cInfo.Col14Data;
            parms[15].Value = cInfo.Col15Data;
            parms[16].Value = cInfo.Col16Data;
            parms[17].Value = cInfo.Col17Data;
            parms[18].Value = cInfo.Col18Data;
            parms[19].Value = cInfo.Col19Data;
            parms[20].Value = cInfo.Col20Data;
            parms[21].Value = cInfo.Col21Data;
            parms[22].Value = cInfo.Col22Data;
            parms[23].Value = cInfo.Col23Data;
            parms[24].Value = cInfo.Col24Data;
            parms[25].Value = cInfo.Col25Data;
            parms[26].Value = cInfo.Col26Data;
            parms[27].Value = cInfo.Col27Data;
            parms[28].Value = cInfo.Col28Data;
            parms[38].Value = cInfo.GhiChu;
        }
        public IList<BaoCaoPhanAnhKienNghiInfo> GetAllXaByCoQuanHuyen(int CoQuanID, int thang, int nam)
        {
            IList<BaoCaoPhanAnhKienNghiInfo> infoList = new List<BaoCaoPhanAnhKienNghiInfo>();
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARM_THANG, SqlDbType.Int),
                new SqlParameter(PARM_NAM, SqlDbType.Int)
            };
            parm[0].Value = CoQuanID;
            parm[1].Value = thang;
            parm[2].Value = nam;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_ALL_XA_BY_COQUANHUYEN, parm))
                {
                    while (dr.Read())
                    {
                        BaoCaoPhanAnhKienNghiInfo info = GetData2c(dr);
                        infoList.Add(info);
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return infoList;
        }
        //public IList<BaoCaoPhanAnhKienNghiDonThuInfo> GetDonThu(DateTime startDate, DateTime endDate, int cqID)
        //{
        //    IList<BaoCaoPhanAnhKienNghiDonThuInfo> infoList = new List<BaoCaoPhanAnhKienNghiDonThuInfo>();
        //    SqlParameter[] parm = new SqlParameter[] {
        //        new SqlParameter(PARM_STARTDATE, SqlDbType.DateTime),
        //        new SqlParameter(PARM_ENDDATE, SqlDbType.DateTime),
        //        new SqlParameter(PARM_COQUANID, SqlDbType.Int)
        //    };
        //    parm[0].Value = startDate;
        //    parm[1].Value = endDate;
        //    parm[2].Value = cqID;
        //    try
        //    {
        //        using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DONTHU_KN, parm))
        //        {
        //            while (dr.Read())
        //            {
        //                BaoCaoPhanAnhKienNghiDonThuInfo info = GetData(dr);
        //                infoList.Add(info);
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //    return infoList;
        //}

        public IList<BaoCaoPhanAnhKienNghiDonThuInfo> GetDonThuByTinh(DateTime startDate, DateTime endDate, int tinhID)
        {
            IList<BaoCaoPhanAnhKienNghiDonThuInfo> infoList = new List<BaoCaoPhanAnhKienNghiDonThuInfo>();
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_STARTDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_ENDDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_TINHID, SqlDbType.Int)
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            parm[2].Value = tinhID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DONTHU_KN_BY_TINH, parm))
                {
                    while (dr.Read())
                    {
                        BaoCaoPhanAnhKienNghiDonThuInfo info = GetData(dr);
                        infoList.Add(info);
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return infoList;
        }
        public List<TKDonThuInfo> GetDSChiTietDonThu1(DateTime startDate, DateTime endDate, List<CoQuanInfo> ListCoQuan, int start, int pagesize, int? Index, int? xemTaiLieuMat, int? canBoID, int? capID)
        {
            List<TKDonThuInfo> infoList = new List<TKDonThuInfo>();
            var pList = new SqlParameter("@ListCoQuanID", SqlDbType.Structured);
            pList.TypeName = "dbo.IntList";
            var tbCoQuanID = new DataTable();
            var tmp = ListCoQuan.Select(x => x.CoQuanID).ToList();
            tbCoQuanID.Columns.Add("CoQuanID", typeof(string));
            ListCoQuan.ForEach(x => tbCoQuanID.Rows.Add(x.CoQuanID));
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_STARTDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_ENDDATE, SqlDbType.DateTime),
               pList,
                new SqlParameter("@Index", SqlDbType.Int),
                new SqlParameter(PARAM_END, SqlDbType.Int),
                new SqlParameter(PARAM_START, SqlDbType.Int),
                 new SqlParameter("@CapID", SqlDbType.Int)
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            parm[2].Value = tbCoQuanID;
            parm[3].Value = Index ?? Convert.DBNull;
            parm[4].Value = pagesize;
            parm[5].Value = start;
            parm[6].Value = capID ?? Convert.DBNull;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "BaoCaoPhanAnhKienNghi_GetDSChiTietDonThu_New", parm))
                {
                    while (dr.Read())
                    {
                        TKDonThuInfo dtInfo = new TKDonThuInfo();

                        dtInfo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        dtInfo.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
                        dtInfo.DonThuID = Utils.ConvertToInt32(dr["DonThuID"], 0);
                        dtInfo.HuongXuLyID = Utils.ConvertToInt32(dr["HuongGiaiQuyetID"], 0);

                        dtInfo.NgayNhapDon = Utils.ConvertToDateTime(dr["NgayNhapDon"], DateTime.MinValue);
                        dtInfo.KetQuaID = Utils.ConvertToInt32(dr["KetQuaID"], 0);

                        dtInfo.SoDon = Utils.ConvertToString(dr["SoDonThu"], string.Empty);
                        dtInfo.DiaChi = Utils.ConvertToString(dr["DiaChiCT"], string.Empty);
                        string noiDungDon = Utils.ConvertToString(dr["NoiDungTiep"], string.Empty);
                        if (noiDungDon == string.Empty)
                        {
                            noiDungDon = Utils.ConvertToString(dr["NoiDungDon"], string.Empty);
                        }
                        dtInfo.NoiDungDon = noiDungDon;
                        //   dtInfo.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"], 0);
                        dtInfo.TenChuDon = Utils.ConvertToString(dr["HoTen"], string.Empty);
                        //   dtInfo.DiaChi = Utils.ConvertToString(dr["DiaChiCT"], string.Empty);
                        //DateTime ngayNhapDon = Utils.ConvertToDateTime(dr["NgayTiep"], DateTime.MinValue);
                        //if (ngayNhapDon == DateTime.MinValue)
                        //{
                        //    ngayNhapDon = Utils.ConvertToDateTime(dr["NgayNhapDon"], DateTime.MinValue);
                        //}
                        //       dtInfo.NgayNhapDon = ngayNhapDon;
                        dtInfo.NgayNhapDonStr = Format.FormatDate(dtInfo.NgayNhapDon);
                        dtInfo.TenLoaiKhieuTo = Utils.ConvertToString(dr["TenLoaiKhieuTo"], string.Empty);
                        //  --   dtInfo.KetQuaID = Utils.ConvertToInt32(dr["KetQuaID"], 0);
                        dtInfo.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        dtInfo.TenHuongGiaiQuyet = Utils.ConvertToString(dr["TenHuongGiaiQuyet"], string.Empty);
                        if (dtInfo.HuongXuLyID == 0)
                        {
                            dtInfo.KetQuaID_Str = "Chưa giải quyết";
                        }
                        else if (dtInfo.HuongXuLyID == (int)HuongGiaiQuyetEnum.DeXuatThuLy && dtInfo.StateID != 10)
                        {
                            dtInfo.KetQuaID_Str = "Đang giải quyết";
                        }
                        else
                        {
                            dtInfo.KetQuaID_Str = "Đã giải quyết";
                        }
                        List<FileHoSoInfo> fileYKienXL = new List<FileHoSoInfo>();
                        List<FileHoSoInfo> fileBanHanhQD = new List<FileHoSoInfo>();
                        if (dtInfo.XuLyDonID > 0)
                        {
                            if (xemTaiLieuMat == 1)
                            {

                                fileYKienXL = new FileHoSoDAL().GetFileYKienXuLyByXuLyDonID(dtInfo.XuLyDonID).ToList();
                            }
                            else
                            {
                                fileYKienXL = new FileHoSoDAL().GetFileYKienXuLyByXuLyDonID(dtInfo.XuLyDonID).Where(x => x.IsBaoMat != true || x.NguoiUp == canBoID).ToList();
                            }

                            int step = 0;
                            for (int i = 0; i < fileYKienXL.Count; i++)
                            {
                                if (!string.IsNullOrEmpty(fileYKienXL[i].FileURL))
                                {
                                    if (string.IsNullOrEmpty(fileYKienXL[i].TenFile))
                                    {
                                        string[] arrtenFile = fileYKienXL[i].FileURL.Split('/');
                                        if (arrtenFile.Length > 0)
                                        {
                                            string[] duoiFile = arrtenFile[arrtenFile.Length - 1].Split('_');
                                            if (duoiFile.Length > 0)
                                            {
                                                fileYKienXL[i].TenFile = duoiFile[duoiFile.Length - 1];
                                            }
                                            else
                                            {
                                                fileYKienXL[i].TenFile = arrtenFile[arrtenFile.Length - 1];
                                            }
                                        }
                                    }
                                    fileYKienXL[i].FileURL = fileYKienXL[i].FileURL.Replace(" ", "%20");
                                }
                                step++;
                                if (fileYKienXL[i].IsBaoMat == false)
                                {
                                    string sec_false = "<li style='margin-bottom: 5px;'>" + step + ". " + "<a href='" + fileYKienXL[i].FileURL + "' download>" + fileYKienXL[i].TenFile + "</a></li>";
                                    dtInfo.TenHuongGiaiQuyet = "<div style='margin-bottom: 5px;'><span>" + dtInfo.TenHuongGiaiQuyet + "</span></div>" + "<ul>" + sec_false + "</ul>";
                                }
                                else
                                {
                                    string sec_true = "<li style='margin-bottom: 5px;'>" + step + ". " + "<a style='color: #EE7600;' href=" + "/DownloadFileDonThuCT.aspx?url=" + fileYKienXL[i].FileURL + ">" + fileYKienXL[i].TenFile + "</a></li>";
                                    dtInfo.TenHuongGiaiQuyet = "<div style='margin-bottom: 5px;'><span>" + dtInfo.TenHuongGiaiQuyet + "</span></div>" + "<ul>" + sec_true + "</ul>";
                                }
                            }
                        }

                        if (dtInfo.KetQuaID > 0)
                        {
                            if (xemTaiLieuMat == 1)
                            {

                                fileBanHanhQD = new FileHoSoDAL().GetFileBanHanhQDByXuLyDonID(dtInfo.XuLyDonID).ToList();
                            }
                            else
                            {
                                fileBanHanhQD = new FileHoSoDAL().GetFileBanHanhQDByXuLyDonID(dtInfo.XuLyDonID).Where(x => x.IsBaoMat != true || x.NguoiUp == canBoID).ToList();
                            }

                            int steps = 0;
                            for (int j = 0; j < fileBanHanhQD.Count; j++)
                            {
                                if (!string.IsNullOrEmpty(fileBanHanhQD[j].FileURL))
                                {
                                    if (string.IsNullOrEmpty(fileBanHanhQD[j].TenFile))
                                    {
                                        string[] arrtenFile = fileBanHanhQD[j].FileURL.Split('/');
                                        if (arrtenFile.Length > 0)
                                        {
                                            string[] duoiFile = arrtenFile[arrtenFile.Length - 1].Split('_');
                                            if (duoiFile.Length > 0)
                                            {
                                                fileBanHanhQD[j].TenFile = duoiFile[duoiFile.Length - 1];
                                            }
                                            else
                                            {
                                                fileBanHanhQD[j].TenFile = arrtenFile[arrtenFile.Length - 1];
                                            }
                                        }
                                    }
                                    fileBanHanhQD[j].FileURL = fileBanHanhQD[j].FileURL.Replace(" ", "%20");
                                }
                                steps++;
                                if (fileBanHanhQD[j].IsBaoMat == false)
                                {
                                    string sec_false = "<li style='margin-bottom: 5px;'>" + steps + ". " + "<a href='" + fileBanHanhQD[j].FileURL + "' download>" + fileBanHanhQD[j].TenFile + "</a></li>";
                                    dtInfo.KetQuaID_Str = "<div style='margin-bottom: 5px;'><span>" + dtInfo.KetQuaID_Str + "</span></div>" + "<ul>" + sec_false + "</ul>";
                                }
                                else
                                {
                                    string sec_true = "<li style='margin-bottom: 5px;'>" + steps + ". " + "<a style='color: #EE7600;' href=" + "/DownloadFileDonThuCT.aspx?url=" + fileBanHanhQD[j].FileURL + ">" + fileBanHanhQD[j].TenFile + "</a></li>";
                                    dtInfo.KetQuaID_Str = "<div style='margin-bottom: 5px;'><span>" + dtInfo.KetQuaID_Str + "</span></div>" + "<ul>" + sec_true + "</ul>";
                                }
                            }
                        }
                        infoList.Add(dtInfo);
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            //List<TKDonThuInfo> ListDonThuID = new List<TKDonThuInfo>();
            //foreach(var i in infoList)
            //{
            //    if(!ListDonThuID.Any(x=>x.DonThuID == i.DonThuID))
            //    {
            //        ListDonThuID.Add(i);
            //    }

            //}

            return infoList;
        }


        public List<TKDonThuInfo> GetDSChiTietDonThu(DateTime startDate, DateTime endDate, List<CoQuanInfo> ListCoQuan, int start, int pagesize, int? Index, int? xemTaiLieuMat, int? canBoID, int? capID)
        {
            List<TKDonThuInfo> infoList = new List<TKDonThuInfo>();
            var pList = new SqlParameter("@ListCoQuanID", SqlDbType.Structured);
            pList.TypeName = "dbo.IntList";
            var tbCoQuanID = new DataTable();
            var tmp = ListCoQuan.Select(x => x.CoQuanID).ToList();
            tbCoQuanID.Columns.Add("CoQuanID", typeof(string));
            ListCoQuan.ForEach(x => tbCoQuanID.Rows.Add(x.CoQuanID));
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_STARTDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_ENDDATE, SqlDbType.DateTime),
               pList,
                new SqlParameter("@Index", SqlDbType.Int),
                new SqlParameter(PARAM_END, SqlDbType.Int),
                new SqlParameter(PARAM_START, SqlDbType.Int),
                 new SqlParameter("@CapID", SqlDbType.Int)
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            parm[2].Value = tbCoQuanID;
            parm[3].Value = Index ?? Convert.DBNull;
            parm[4].Value = pagesize;
            parm[5].Value = start;
            parm[6].Value = capID ?? Convert.DBNull;
            try
            {
                var st1 = new Stopwatch();
                st1.Start();
                var query = new DataTable();
                //SqlDataReader dr1 = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "BaoCaoPhanAnhKienNghi_GetDSChiTietDonThu_New", parm);
                //query.Load(dr1);

                //var l = ListCoQuan.Select(x => x.CoQuanID).ToList();
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "BaoCaoPhanAnhKienNghi_GetDSChiTietDonThu_New_v2", parm))
                {
                    st1.Stop();
                    var t1 = st1.Elapsed;
                    var st2 = new Stopwatch();
                    st2.Start();
                    while (dr.Read())
                    {
                        TKDonThuInfo dtInfo = new TKDonThuInfo();

                        dtInfo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        dtInfo.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
                        dtInfo.DonThuID = Utils.ConvertToInt32(dr["DonThuID"], 0);
                        dtInfo.HuongXuLyID = Utils.ConvertToInt32(dr["HuongGiaiQuyetID"], 0);

                        dtInfo.NgayNhapDon = Utils.ConvertToDateTime(dr["NgayNhapDon"], DateTime.MinValue);
                        dtInfo.KetQuaID = Utils.ConvertToInt32(dr["KetQuaID"], 0);

                        dtInfo.SoDon = Utils.ConvertToString(dr["SoDonThu"], string.Empty);
                        dtInfo.DiaChi = Utils.ConvertToString(dr["DiaChiCT"], string.Empty);
                        string noiDungDon = Utils.ConvertToString(dr["NoiDungTiep"], string.Empty);
                        if (noiDungDon == string.Empty)
                        {
                            noiDungDon = Utils.ConvertToString(dr["NoiDungDon"], string.Empty);
                        }
                        dtInfo.NoiDungDon = noiDungDon;
                        //   dtInfo.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"], 0);
                        dtInfo.TenChuDon = Utils.ConvertToString(dr["HoTen"], string.Empty);
                        //   dtInfo.DiaChi = Utils.ConvertToString(dr["DiaChiCT"], string.Empty);
                        //DateTime ngayNhapDon = Utils.ConvertToDateTime(dr["NgayTiep"], DateTime.MinValue);
                        //if (ngayNhapDon == DateTime.MinValue)
                        //{
                        //    ngayNhapDon = Utils.ConvertToDateTime(dr["NgayNhapDon"], DateTime.MinValue);
                        //}
                        //       dtInfo.NgayNhapDon = ngayNhapDon;
                        dtInfo.NgayNhapDonStr = Format.FormatDate(dtInfo.NgayNhapDon);
                        dtInfo.TenLoaiKhieuTo = Utils.ConvertToString(dr["TenLoaiKhieuTo"], string.Empty);
                        //  --   dtInfo.KetQuaID = Utils.ConvertToInt32(dr["KetQuaID"], 0);
                        dtInfo.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        dtInfo.TenHuongGiaiQuyet = Utils.ConvertToString(dr["TenHuongGiaiQuyet"], string.Empty);
                        if (dtInfo.HuongXuLyID == 0)
                        {
                            dtInfo.KetQuaID_Str = "Chưa giải quyết";
                        }
                        else if (dtInfo.HuongXuLyID == (int)HuongGiaiQuyetEnum.DeXuatThuLy && dtInfo.StateID != 10)
                        {
                            dtInfo.KetQuaID_Str = "Đang giải quyết";
                        }
                        else
                        {
                            dtInfo.KetQuaID_Str = "Đã giải quyết";
                        }
                        //List<FileHoSoInfo> fileYKienXL = new List<FileHoSoInfo>();
                        //List<FileHoSoInfo> fileBanHanhQD = new List<FileHoSoInfo>();
                        //if (dtInfo.XuLyDonID > 0)
                        //{
                        //    if (xemTaiLieuMat == 1)
                        //    {

                        //        fileYKienXL = new FileHoSoDAL().GetFileYKienXuLyByXuLyDonID(dtInfo.XuLyDonID).ToList();
                        //    }
                        //    else
                        //    {
                        //        fileYKienXL = new FileHoSoDAL().GetFileYKienXuLyByXuLyDonID(dtInfo.XuLyDonID).Where(x => x.IsBaoMat != true || x.NguoiUp == canBoID).ToList();
                        //    }

                        //    int step = 0;
                        //    for (int i = 0; i < fileYKienXL.Count; i++)
                        //    {
                        //        if (!string.IsNullOrEmpty(fileYKienXL[i].FileURL))
                        //        {
                        //            if (string.IsNullOrEmpty(fileYKienXL[i].TenFile))
                        //            {
                        //                string[] arrtenFile = fileYKienXL[i].FileURL.Split('/');
                        //                if (arrtenFile.Length > 0)
                        //                {
                        //                    string[] duoiFile = arrtenFile[arrtenFile.Length - 1].Split('_');
                        //                    if (duoiFile.Length > 0)
                        //                    {
                        //                        fileYKienXL[i].TenFile = duoiFile[duoiFile.Length - 1];
                        //                    }
                        //                    else
                        //                    {
                        //                        fileYKienXL[i].TenFile = arrtenFile[arrtenFile.Length - 1];
                        //                    }
                        //                }
                        //            }
                        //            fileYKienXL[i].FileURL = fileYKienXL[i].FileURL.Replace(" ", "%20");
                        //        }
                        //        step++;
                        //        if (fileYKienXL[i].IsBaoMat == false)
                        //        {
                        //            string sec_false = "<li style='margin-bottom: 5px;'>" + step + ". " + "<a href='" + fileYKienXL[i].FileURL + "' download>" + fileYKienXL[i].TenFile + "</a></li>";
                        //            dtInfo.TenHuongGiaiQuyet = "<div style='margin-bottom: 5px;'><span>" + dtInfo.TenHuongGiaiQuyet + "</span></div>" + "<ul>" + sec_false + "</ul>";
                        //        }
                        //        else
                        //        {
                        //            string sec_true = "<li style='margin-bottom: 5px;'>" + step + ". " + "<a style='color: #EE7600;' href=" + "/DownloadFileDonThuCT.aspx?url=" + fileYKienXL[i].FileURL + ">" + fileYKienXL[i].TenFile + "</a></li>";
                        //            dtInfo.TenHuongGiaiQuyet = "<div style='margin-bottom: 5px;'><span>" + dtInfo.TenHuongGiaiQuyet + "</span></div>" + "<ul>" + sec_true + "</ul>";
                        //        }
                        //    }
                        //}

                        //if (dtInfo.KetQuaID > 0)
                        //{
                        //    if (xemTaiLieuMat == 1)
                        //    {

                        //        fileBanHanhQD = new FileHoSoDAL().GetFileBanHanhQDByXuLyDonID(dtInfo.XuLyDonID).ToList();
                        //    }
                        //    else
                        //    {
                        //        fileBanHanhQD = new FileHoSoDAL().GetFileBanHanhQDByXuLyDonID(dtInfo.XuLyDonID).Where(x => x.IsBaoMat != true || x.NguoiUp == canBoID).ToList();
                        //    }

                        //    int steps = 0;
                        //    for (int j = 0; j < fileBanHanhQD.Count; j++)
                        //    {
                        //        if (!string.IsNullOrEmpty(fileBanHanhQD[j].FileURL))
                        //        {
                        //            if (string.IsNullOrEmpty(fileBanHanhQD[j].TenFile))
                        //            {
                        //                string[] arrtenFile = fileBanHanhQD[j].FileURL.Split('/');
                        //                if (arrtenFile.Length > 0)
                        //                {
                        //                    string[] duoiFile = arrtenFile[arrtenFile.Length - 1].Split('_');
                        //                    if (duoiFile.Length > 0)
                        //                    {
                        //                        fileBanHanhQD[j].TenFile = duoiFile[duoiFile.Length - 1];
                        //                    }
                        //                    else
                        //                    {
                        //                        fileBanHanhQD[j].TenFile = arrtenFile[arrtenFile.Length - 1];
                        //                    }
                        //                }
                        //            }
                        //            fileBanHanhQD[j].FileURL = fileBanHanhQD[j].FileURL.Replace(" ", "%20");
                        //        }
                        //        steps++;
                        //        if (fileBanHanhQD[j].IsBaoMat == false)
                        //        {
                        //            string sec_false = "<li style='margin-bottom: 5px;'>" + steps + ". " + "<a href='" + fileBanHanhQD[j].FileURL + "' download>" + fileBanHanhQD[j].TenFile + "</a></li>";
                        //            dtInfo.KetQuaID_Str = "<div style='margin-bottom: 5px;'><span>" + dtInfo.KetQuaID_Str + "</span></div>" + "<ul>" + sec_false + "</ul>";
                        //        }
                        //        else
                        //        {
                        //            string sec_true = "<li style='margin-bottom: 5px;'>" + steps + ". " + "<a style='color: #EE7600;' href=" + "/DownloadFileDonThuCT.aspx?url=" + fileBanHanhQD[j].FileURL + ">" + fileBanHanhQD[j].TenFile + "</a></li>";
                        //            dtInfo.KetQuaID_Str = "<div style='margin-bottom: 5px;'><span>" + dtInfo.KetQuaID_Str + "</span></div>" + "<ul>" + sec_true + "</ul>";
                        //        }
                        //    }
                        //}
                        infoList.Add(dtInfo);
                    }
                    st2.Stop();
                    var t2 = st2.Elapsed;

                }
            }
            catch (Exception e)
            {
                throw;
            }
            //List<FileHoSoInfo> fileYKienXLAll = new List<FileHoSoInfo>();
            //List<FileHoSoInfo> fileBanHanhQDAll = new List<FileHoSoInfo>();

            //if (xemTaiLieuMat == 1)
            //{
            //    fileYKienXLAll = new FileHoSoDAL().GetFileYKienXuLyByListXuLyDon(infoList).ToList();
            //    fileBanHanhQDAll = new FileHoSoDAL().GetFileBanHanhQDByListXuLyDon(infoList).ToList();
            //}
            //else
            //{
            //    fileYKienXLAll = new FileHoSoDAL().GetFileYKienXuLyByListXuLyDon(infoList).Where(x => x.IsBaoMat != true || x.NguoiUp == canBoID).ToList();
            //    fileBanHanhQDAll = new FileHoSoDAL().GetFileBanHanhQDByListXuLyDon(infoList).Where(x => x.IsBaoMat != true || x.NguoiUp == canBoID).ToList();
            //}

            //if (fileYKienXLAll.Count > 0)
            //{
            //    foreach (TKDonThuInfo info in infoList)
            //    {
            //        List<FileHoSoInfo> fileYKienXL = new List<FileHoSoInfo>();
            //        for (int i = 0; i < fileYKienXLAll.Count; i++)
            //        {
            //            if (fileYKienXLAll[i].XuLyDonID == info.XuLyDonID)
            //            {
            //                fileYKienXL.Add(fileYKienXLAll[i]);
            //            }
            //        }

            //        int step = 0;
            //        for (int i = 0; i < fileYKienXL.Count; i++)
            //        {
            //            if (!string.IsNullOrEmpty(fileYKienXL[i].FileURL))
            //            {
            //                if (string.IsNullOrEmpty(fileYKienXL[i].TenFile))
            //                {
            //                    string[] arrtenFile = fileYKienXL[i].FileURL.Split('/');
            //                    if (arrtenFile.Length > 0)
            //                    {
            //                        string[] duoiFile = arrtenFile[arrtenFile.Length - 1].Split('_');
            //                        if (duoiFile.Length > 0)
            //                        {
            //                            fileYKienXL[i].TenFile = duoiFile[duoiFile.Length - 1];
            //                        }
            //                        else
            //                        {
            //                            fileYKienXL[i].TenFile = arrtenFile[arrtenFile.Length - 1];
            //                        }
            //                    }
            //                }
            //                fileYKienXL[i].FileURL = fileYKienXL[i].FileURL.Replace(" ", "%20");
            //            }
            //            step++;
            //            if (fileYKienXL[i].IsBaoMat == false)
            //            {
            //                string sec_false = "<li style='margin-bottom: 5px;'>" + step + ". " + "<a href='" + fileYKienXL[i].FileURL + "' download>" + fileYKienXL[i].TenFile + "</a></li>";
            //                info.TenHuongGiaiQuyet = "<div style='margin-bottom: 5px;'><span>" + info.TenHuongGiaiQuyet + "</span></div>" + "<ul>" + sec_false + "</ul>";
            //            }
            //            else
            //            {
            //                string sec_true = "<li style='margin-bottom: 5px;'>" + step + ". " + "<a style='color: #EE7600;' href=" + "/DownloadFileDonThuCT.aspx?url=" + fileYKienXL[i].FileURL + ">" + fileYKienXL[i].TenFile + "</a></li>";
            //                info.TenHuongGiaiQuyet = "<div style='margin-bottom: 5px;'><span>" + info.TenHuongGiaiQuyet + "</span></div>" + "<ul>" + sec_true + "</ul>";
            //            }
            //        }
            //    }

            //}
            //if (fileBanHanhQDAll.Count > 0)
            //{
            //    foreach (TKDonThuInfo info in infoList)
            //    {
            //        List<FileHoSoInfo> fileBanHanhQD = new List<FileHoSoInfo>();
            //        for (int i = 0; i < fileBanHanhQDAll.Count; i++)
            //        {
            //            if (fileBanHanhQDAll[i].XuLyDonID == info.XuLyDonID)
            //            {
            //                fileBanHanhQD.Add(fileBanHanhQDAll[i]);
            //            }
            //        }

            //        int steps = 0;
            //        for (int j = 0; j < fileBanHanhQD.Count; j++)
            //        {
            //            if (!string.IsNullOrEmpty(fileBanHanhQD[j].FileURL))
            //            {
            //                if (string.IsNullOrEmpty(fileBanHanhQD[j].TenFile))
            //                {
            //                    string[] arrtenFile = fileBanHanhQD[j].FileURL.Split('/');
            //                    if (arrtenFile.Length > 0)
            //                    {
            //                        string[] duoiFile = arrtenFile[arrtenFile.Length - 1].Split('_');
            //                        if (duoiFile.Length > 0)
            //                        {
            //                            fileBanHanhQD[j].TenFile = duoiFile[duoiFile.Length - 1];
            //                        }
            //                        else
            //                        {
            //                            fileBanHanhQD[j].TenFile = arrtenFile[arrtenFile.Length - 1];
            //                        }
            //                    }
            //                }
            //                fileBanHanhQD[j].FileURL = fileBanHanhQD[j].FileURL.Replace(" ", "%20");
            //            }
            //            steps++;
            //            if (fileBanHanhQD[j].IsBaoMat == false)
            //            {
            //                string sec_false = "<li style='margin-bottom: 5px;'>" + steps + ". " + "<a href='" + fileBanHanhQD[j].FileURL + "' download>" + fileBanHanhQD[j].TenFile + "</a></li>";
            //                info.KetQuaID_Str = "<div style='margin-bottom: 5px;'><span>" + info.KetQuaID_Str + "</span></div>" + "<ul>" + sec_false + "</ul>";
            //            }
            //            else
            //            {
            //                string sec_true = "<li style='margin-bottom: 5px;'>" + steps + ". " + "<a style='color: #EE7600;' href=" + "/DownloadFileDonThuCT.aspx?url=" + fileBanHanhQD[j].FileURL + ">" + fileBanHanhQD[j].TenFile + "</a></li>";
            //                info.KetQuaID_Str = "<div style='margin-bottom: 5px;'><span>" + info.KetQuaID_Str + "</span></div>" + "<ul>" + sec_true + "</ul>";
            //            }
            //        }
            //    }
            //}
            return infoList;
        }

        public List<TKDonThuInfo> GetDSChiTietDonThu11(DateTime startDate, DateTime endDate, List<CoQuanInfo> ListCoQuan, int start, int pagesize, int? Index, int? xemTaiLieuMat, int? canBoID, int? capID)
        {
            List<TKDonThuInfo> infoList = new List<TKDonThuInfo>();
            var pList = new SqlParameter("@ListCoQuanID", SqlDbType.Structured);
            pList.TypeName = "dbo.IntList";
            var tbCoQuanID = new DataTable();
            var tmp = ListCoQuan.Select(x => x.CoQuanID).ToList();
            tbCoQuanID.Columns.Add("CoQuanID", typeof(string));
            ListCoQuan.ForEach(x => tbCoQuanID.Rows.Add(x.CoQuanID));
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_STARTDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_ENDDATE, SqlDbType.DateTime),
               pList,
                new SqlParameter("@Index", SqlDbType.Int),
                new SqlParameter(PARAM_END, SqlDbType.Int),
                new SqlParameter(PARAM_START, SqlDbType.Int),
                 new SqlParameter("@CapID", SqlDbType.Int)
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            parm[2].Value = tbCoQuanID;
            parm[3].Value = Index ?? Convert.DBNull;
            parm[4].Value = pagesize;
            parm[5].Value = start;
            parm[6].Value = capID ?? Convert.DBNull;
            var query = new DataTable();
            try
            {
                var st1 = new Stopwatch();
                st1.Start();

                //SqlDataReader dr1 = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "BaoCaoPhanAnhKienNghi_GetDSChiTietDonThu_New", parm);
                //query.Load(dr1);

                //var l = ListCoQuan.Select(x => x.CoQuanID).ToList();
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "BaoCaoPhanAnhKienNghi_GetDSChiTietDonThu_New", parm))
                {
                    query.Load(dr);
                    st1.Stop();
                    var t1 = st1.Elapsed;

                }
            }
            catch (Exception e)
            {
                throw;
            }
            var dataRows = query.AsEnumerable();
            List<FileHoSoInfo> fileYKienXLAll = new List<FileHoSoInfo>();
            List<FileHoSoInfo> fileBanHanhQDAll = new List<FileHoSoInfo>();
            var listXuLyDonID = dataRows.Select(x => x.Field<int>("XuLyDonID")).ToList();
            if (xemTaiLieuMat == 1)
            {
                fileYKienXLAll = new FileHoSoDAL().GetFileYKienXuLyByListXuLyDon1(listXuLyDonID).ToList();
                fileBanHanhQDAll = new FileHoSoDAL().GetFileBanHanhQDByListXuLyDon1(listXuLyDonID).ToList();
            }
            else
            {
                fileYKienXLAll = new FileHoSoDAL().GetFileYKienXuLyByListXuLyDon1(listXuLyDonID).Where(x => x.IsBaoMat != true || x.NguoiUp == canBoID).ToList();
                fileBanHanhQDAll = new FileHoSoDAL().GetFileBanHanhQDByListXuLyDon1(listXuLyDonID).Where(x => x.IsBaoMat != true || x.NguoiUp == canBoID).ToList();
            }

            var st2 = new Stopwatch();
            st2.Start();

            // for bất đồng bộ
            #region Parallel
            //Parallel.ForEach(dataRows, item =>
            //{
            //    // don tu info
            //    var info = new TKDonThuInfo();
            //    info.CoQuanID = Utils.ConvertToInt32(item.Field<int?>("CoQuanID"), 0);
            //    info.XuLyDonID = Utils.ConvertToInt32(item.Field<int?>("XuLyDonID"), 0);
            //    info.DonThuID = Utils.ConvertToInt32(item.Field<int?>("DonThuID"), 0);
            //    info.HuongXuLyID = Utils.ConvertToInt32(item.Field<int?>("HuongGiaiQuyetID"), 0);
            //    info.NgayNhapDon = Utils.ConvertToDateTime(item.Field<DateTime?>("NgayNhapDon"), DateTime.MinValue);
            //    info.KetQuaID = Utils.ConvertToInt32(item.Field<int?>("KetQuaID"), 0);
            //    info.SoDon = Utils.ConvertToString(item.Field<string>("SoDonThu"), string.Empty);
            //    info.DiaChi = Utils.ConvertToString(item.Field<string>("DiaChiCT"), string.Empty);
            //    string noiDungDon = Utils.ConvertToString(item.Field<string>("NoiDungTiep"), string.Empty);
            //    if (noiDungDon == string.Empty)
            //    {
            //        noiDungDon = Utils.ConvertToString(item.Field<string>("NoiDungDon"), string.Empty);
            //    }
            //    info.NoiDungDon = noiDungDon;
            //    info.TenChuDon = Utils.ConvertToString(item.Field<string>("HoTen"), string.Empty);
            //    info.NgayNhapDonStr = Format.FormatDate(info.NgayNhapDon);
            //    info.TenLoaiKhieuTo = Utils.ConvertToString(item.Field<string>("TenLoaiKhieuTo"), string.Empty);
            //    info.StateID = Utils.ConvertToInt32(item.Field<int?>("StateID"), 0);
            //    info.TenHuongGiaiQuyet = Utils.ConvertToString(item.Field<string>("TenHuongGiaiQuyet"), string.Empty);
            //    if (info.HuongXuLyID == 0)
            //    {
            //        info.KetQuaID_Str = "Chưa giải quyết";
            //    }
            //    else if (info.HuongXuLyID == (int)HuongGiaiQuyetEnum.DeXuatThuLy && info.StateID != 10)
            //    {
            //        info.KetQuaID_Str = "Đang giải quyết";
            //    }
            //    else
            //    {
            //        info.KetQuaID_Str = "Đã giải quyết";
            //    }

            //    // huong giai quyet
            //    List<FileHoSoInfo> fileYKienXL = new List<FileHoSoInfo>();
            //    fileYKienXL = fileYKienXLAll.Where(x => x.XuLyDonID == item.Field<int>("XuLyDonID")).ToList();
            //    int step = 0;
            //    for (int i = 0; i < fileYKienXL.Count; i++)
            //    {
            //        if (!string.IsNullOrEmpty(fileYKienXL[i].FileURL))
            //        {
            //            if (string.IsNullOrEmpty(fileYKienXL[i].TenFile))
            //            {
            //                string[] arrtenFile = fileYKienXL[i].FileURL.Split('/');
            //                if (arrtenFile.Length > 0)
            //                {
            //                    string[] duoiFile = arrtenFile[arrtenFile.Length - 1].Split('_');
            //                    if (duoiFile.Length > 0)
            //                    {
            //                        fileYKienXL[i].TenFile = duoiFile[duoiFile.Length - 1];
            //                    }
            //                    else
            //                    {
            //                        fileYKienXL[i].TenFile = arrtenFile[arrtenFile.Length - 1];
            //                    }
            //                }
            //            }
            //            fileYKienXL[i].FileURL = fileYKienXL[i].FileURL.Replace(" ", "%20");
            //        }
            //        step++;
            //        if (fileYKienXL[i].IsBaoMat == false)
            //        {
            //            string sec_false = "<li style='margin-bottom: 5px;'>" + step + ". " + "<a href='" + fileYKienXL[i].FileURL + "' download>" + fileYKienXL[i].TenFile + "</a></li>";
            //            info.TenHuongGiaiQuyet = "<div style='margin-bottom: 5px;'><span>" + info.TenHuongGiaiQuyet + "</span></div>" + "<ul>" + sec_false + "</ul>";
            //        }
            //        else
            //        {
            //            string sec_true = "<li style='margin-bottom: 5px;'>" + step + ". " + "<a style='color: #EE7600;' href=" + "/DownloadFileDonThuCT.aspx?url=" + fileYKienXL[i].FileURL + ">" + fileYKienXL[i].TenFile + "</a></li>";
            //            info.TenHuongGiaiQuyet = "<div style='margin-bottom: 5px;'><span>" + info.TenHuongGiaiQuyet + "</span></div>" + "<ul>" + sec_true + "</ul>";
            //        }
            //    }

            //    // kết quả
            //    List<FileHoSoInfo> fileBanHanhQD = new List<FileHoSoInfo>();
            //    fileBanHanhQD = fileBanHanhQDAll.Where(x => x.XuLyDonID == info.XuLyDonID).ToList();
            //    int steps = 0;
            //    for (int j = 0; j < fileBanHanhQD.Count; j++)
            //    {
            //        if (!string.IsNullOrEmpty(fileBanHanhQD[j].FileURL))
            //        {
            //            if (string.IsNullOrEmpty(fileBanHanhQD[j].TenFile))
            //            {
            //                string[] arrtenFile = fileBanHanhQD[j].FileURL.Split('/');
            //                if (arrtenFile.Length > 0)
            //                {
            //                    string[] duoiFile = arrtenFile[arrtenFile.Length - 1].Split('_');
            //                    if (duoiFile.Length > 0)
            //                    {
            //                        fileBanHanhQD[j].TenFile = duoiFile[duoiFile.Length - 1];
            //                    }
            //                    else
            //                    {
            //                        fileBanHanhQD[j].TenFile = arrtenFile[arrtenFile.Length - 1];
            //                    }
            //                }
            //            }
            //            fileBanHanhQD[j].FileURL = fileBanHanhQD[j].FileURL.Replace(" ", "%20");
            //        }
            //        steps++;
            //        if (fileBanHanhQD[j].IsBaoMat == false)
            //        {
            //            string sec_false = "<li style='margin-bottom: 5px;'>" + steps + ". " + "<a href='" + fileBanHanhQD[j].FileURL + "' download>" + fileBanHanhQD[j].TenFile + "</a></li>";
            //            info.KetQuaID_Str = "<div style='margin-bottom: 5px;'><span>" + info.KetQuaID_Str + "</span></div>" + "<ul>" + sec_false + "</ul>";
            //        }
            //        else
            //        {
            //            string sec_true = "<li style='margin-bottom: 5px;'>" + steps + ". " + "<a style='color: #EE7600;' href=" + "/DownloadFileDonThuCT.aspx?url=" + fileBanHanhQD[j].FileURL + ">" + fileBanHanhQD[j].TenFile + "</a></li>";
            //            info.KetQuaID_Str = "<div style='margin-bottom: 5px;'><span>" + info.KetQuaID_Str + "</span></div>" + "<ul>" + sec_true + "</ul>";
            //        }
            //    }
            //    infoList.Add(info);
            //});


            #endregion


            foreach (var item in dataRows)
            {
                // don tu info
                var info = new TKDonThuInfo();
                info.CoQuanID = Utils.ConvertToInt32(item.Field<int?>("CoQuanID"), 0);
                info.XuLyDonID = Utils.ConvertToInt32(item.Field<int?>("XuLyDonID"), 0);
                info.DonThuID = Utils.ConvertToInt32(item.Field<int?>("DonThuID"), 0);
                info.HuongXuLyID = Utils.ConvertToInt32(item.Field<int?>("HuongGiaiQuyetID"), 0);
                info.NgayNhapDon = Utils.ConvertToDateTime(item.Field<DateTime?>("NgayNhapDon"), DateTime.MinValue);
                info.KetQuaID = Utils.ConvertToInt32(item.Field<int?>("KetQuaID"), 0);
                info.SoDon = Utils.ConvertToString(item.Field<string>("SoDonThu"), string.Empty);
                info.DiaChi = Utils.ConvertToString(item.Field<string>("DiaChiCT"), string.Empty);
                string noiDungDon = Utils.ConvertToString(item.Field<string>("NoiDungTiep"), string.Empty);
                if (noiDungDon == string.Empty)
                {
                    noiDungDon = Utils.ConvertToString(item.Field<string>("NoiDungDon"), string.Empty);
                }
                info.NoiDungDon = noiDungDon;
                info.TenChuDon = Utils.ConvertToString(item.Field<string>("HoTen"), string.Empty);
                info.NgayNhapDonStr = Format.FormatDate(info.NgayNhapDon);
                info.TenLoaiKhieuTo = Utils.ConvertToString(item.Field<string>("TenLoaiKhieuTo"), string.Empty);
                info.StateID = Utils.ConvertToInt32(item.Field<int?>("StateID"), 0);
                info.TenHuongGiaiQuyet = Utils.ConvertToString(item.Field<string>("TenHuongGiaiQuyet"), string.Empty);
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
                // huong giai quyet
                List<FileHoSoInfo> fileYKienXL = new List<FileHoSoInfo>();
                fileYKienXL = fileYKienXLAll.Where(x => x.XuLyDonID == item.Field<int>("XuLyDonID")).ToList();
                int step = 0;
                for (int i = 0; i < fileYKienXL.Count; i++)
                {
                    if (!string.IsNullOrEmpty(fileYKienXL[i].FileURL))
                    {
                        if (string.IsNullOrEmpty(fileYKienXL[i].TenFile))
                        {
                            string[] arrtenFile = fileYKienXL[i].FileURL.Split('/');
                            if (arrtenFile.Length > 0)
                            {
                                string[] duoiFile = arrtenFile[arrtenFile.Length - 1].Split('_');
                                if (duoiFile.Length > 0)
                                {
                                    fileYKienXL[i].TenFile = duoiFile[duoiFile.Length - 1];
                                }
                                else
                                {
                                    fileYKienXL[i].TenFile = arrtenFile[arrtenFile.Length - 1];
                                }
                            }
                        }
                        fileYKienXL[i].FileURL = fileYKienXL[i].FileURL.Replace(" ", "%20");
                    }
                    step++;
                    if (fileYKienXL[i].IsBaoMat == false)
                    {
                        string sec_false = "<li style='margin-bottom: 5px;'>" + step + ". " + "<a href='" + fileYKienXL[i].FileURL + "' download>" + fileYKienXL[i].TenFile + "</a></li>";
                        info.TenHuongGiaiQuyet = "<div style='margin-bottom: 5px;'><span>" + info.TenHuongGiaiQuyet + "</span></div>" + "<ul>" + sec_false + "</ul>";
                    }
                    else
                    {
                        string sec_true = "<li style='margin-bottom: 5px;'>" + step + ". " + "<a style='color: #EE7600;' href=" + "/DownloadFileDonThuCT.aspx?url=" + fileYKienXL[i].FileURL + ">" + fileYKienXL[i].TenFile + "</a></li>";
                        info.TenHuongGiaiQuyet = "<div style='margin-bottom: 5px;'><span>" + info.TenHuongGiaiQuyet + "</span></div>" + "<ul>" + sec_true + "</ul>";
                    }
                }
                // kết quả
                List<FileHoSoInfo> fileBanHanhQD = new List<FileHoSoInfo>();
                fileBanHanhQD = fileBanHanhQDAll.Where(x => x.XuLyDonID == info.XuLyDonID).ToList();
                int steps = 0;
                for (int j = 0; j < fileBanHanhQD.Count; j++)
                {
                    if (!string.IsNullOrEmpty(fileBanHanhQD[j].FileURL))
                    {
                        if (string.IsNullOrEmpty(fileBanHanhQD[j].TenFile))
                        {
                            string[] arrtenFile = fileBanHanhQD[j].FileURL.Split('/');
                            if (arrtenFile.Length > 0)
                            {
                                string[] duoiFile = arrtenFile[arrtenFile.Length - 1].Split('_');
                                if (duoiFile.Length > 0)
                                {
                                    fileBanHanhQD[j].TenFile = duoiFile[duoiFile.Length - 1];
                                }
                                else
                                {
                                    fileBanHanhQD[j].TenFile = arrtenFile[arrtenFile.Length - 1];
                                }
                            }
                        }
                        fileBanHanhQD[j].FileURL = fileBanHanhQD[j].FileURL.Replace(" ", "%20");
                    }
                    steps++;
                    if (fileBanHanhQD[j].IsBaoMat == false)
                    {
                        string sec_false = "<li style='margin-bottom: 5px;'>" + steps + ". " + "<a href='" + fileBanHanhQD[j].FileURL + "' download>" + fileBanHanhQD[j].TenFile + "</a></li>";
                        info.KetQuaID_Str = "<div style='margin-bottom: 5px;'><span>" + info.KetQuaID_Str + "</span></div>" + "<ul>" + sec_false + "</ul>";
                    }
                    else
                    {
                        string sec_true = "<li style='margin-bottom: 5px;'>" + steps + ". " + "<a style='color: #EE7600;' href=" + "/DownloadFileDonThuCT.aspx?url=" + fileBanHanhQD[j].FileURL + ">" + fileBanHanhQD[j].TenFile + "</a></li>";
                        info.KetQuaID_Str = "<div style='margin-bottom: 5px;'><span>" + info.KetQuaID_Str + "</span></div>" + "<ul>" + sec_true + "</ul>";
                    }
                }
                infoList.Add(info);
            }
            st2.Stop();
            var t2 = st2.Elapsed;


            return infoList;
        }

        //public IList<TKDonThuInfo> GetDSChiTietDonThu(DateTime startDate, DateTime endDate, int coQuanID, int start, int end)
        //{
        //    IList<TKDonThuInfo> infoList = new List<TKDonThuInfo>();
        //    SqlParameter[] parm = new SqlParameter[] {
        //        new SqlParameter(PARM_STARTDATE, SqlDbType.DateTime),
        //        new SqlParameter(PARM_ENDDATE, SqlDbType.DateTime),
        //        new SqlParameter(PARM_COQUANID, SqlDbType.Int),
        //        new SqlParameter(PARAM_START, SqlDbType.Int),
        //        new SqlParameter(PARAM_END, SqlDbType.Int)
        //    };
        //    parm[0].Value = startDate;
        //    parm[1].Value = endDate;
        //    parm[2].Value = coQuanID;
        //    parm[3].Value = start;
        //    parm[4].Value = end;
        //    try
        //    {
        //        using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DSCHITIETDONTHU, parm))
        //        {
        //            while (dr.Read())
        //            {
        //                TKDonThuInfo dtInfo = new TKDonThuInfo();

        //                dtInfo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
        //                dtInfo.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
        //                dtInfo.DonThuID = Utils.ConvertToInt32(dr["DonThuID"], 0);
        //                dtInfo.HuongXuLyID = Utils.ConvertToInt32(dr["HuongGiaiQuyetID"], 0);
        //                dtInfo.LoaiKetQuaID = Utils.ConvertToInt32(dr["LoaiKetQuaID"], 0);
        //                dtInfo.NgayNhapDon = Utils.ConvertToDateTime(dr["NgayNhapDon"], DateTime.MinValue);
        //                dtInfo.SoDon = Utils.ConvertToString(dr["SoDonThu"], string.Empty);
        //                string noiDungDon = Utils.ConvertToString(dr["NoiDungTiep"], string.Empty);
        //                if (noiDungDon == string.Empty)
        //                {
        //                    noiDungDon = Utils.ConvertToString(dr["NoiDungDon"], string.Empty);
        //                }
        //                dtInfo.NoiDungDon = noiDungDon;
        //                dtInfo.TenChuDon = Utils.ConvertToString(dr["HoTen"], string.Empty);
        //                dtInfo.DiaChi = Utils.ConvertToString(dr["DiaChiCT"], string.Empty);
        //                DateTime ngayNhapDon = Utils.ConvertToDateTime(dr["NgayTiep"], DateTime.MinValue);
        //                if (ngayNhapDon == DateTime.MinValue)
        //                {
        //                    ngayNhapDon = Utils.ConvertToDateTime(dr["NgayNhapDon"], DateTime.MinValue);
        //                }
        //                dtInfo.NgayNhapDon = ngayNhapDon;
        //                dtInfo.NgayNhapDonStr = Format.FormatDate(dtInfo.NgayNhapDon);
        //                dtInfo.TenLoaiKhieuTo = Utils.ConvertToString(dr["TenLoaiKhieuTo"], string.Empty);
        //                dtInfo.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
        //                dtInfo.TenHuongGiaiQuyet = Utils.ConvertToString(dr["TenHuongGiaiQuyet"], string.Empty);
        //                if (dtInfo.HuongXuLyID == 0)
        //                {
        //                    dtInfo.KetQuaID_Str = "Chưa giải quyết";
        //                }
        //                else if (dtInfo.HuongXuLyID == (int)HuongGiaiQuyetEnum.DeXuatThuLy && dtInfo.StateID != 10)
        //                {
        //                    dtInfo.KetQuaID_Str = "Đang giải quyết";
        //                }
        //                else
        //                {
        //                    dtInfo.KetQuaID_Str = "Đã giải quyết";
        //                }
        //                dtInfo.RutDonID = Utils.ConvertToInt32(dr["RutDonID"], 0);
        //                dtInfo.CQDaGiaiQuyet = Utils.ConvertToString(dr["CQDaGiaiQuyetID"], string.Empty);
        //                dtInfo.NgayThiHanh = Utils.ConvertToDateTime(dr["NgayThiHanh"], DateTime.MinValue);
        //                dtInfo.NgayRaKQ = Utils.ConvertToDateTime(dr["NgayRaKQ"], DateTime.MinValue);
        //                dtInfo.ThiHanhID = Utils.ConvertToInt32(dr["ThiHanhID"], 0);
        //                dtInfo.PhanTichKQID = Utils.ConvertToInt32(dr["PhanTichKQ"], 0);
        //                dtInfo.KetQuaGQLan2 = Utils.ConvertToInt32(dr["KetQuaGQLan2"], 0);
        //                dtInfo.SoDoiTuongDaBiXuLy = Utils.ConvertToInt32(dr["SoDoiTuongDaBiXuLy"], 0);
        //                infoList.Add(dtInfo);
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //    return infoList;
        //}

        public int CountDSChiTietDonThu(DateTime startDate, DateTime endDate, int coQuanID)
        {
            object result = 0;
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_STARTDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_ENDDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_COQUANID, SqlDbType.Int),
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            parm[2].Value = coQuanID;

            try
            {
                result = SQLHelper.ExecuteScalar(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_DSCHITIETDONTHU, parm);
            }
            catch (Exception e)
            {
                throw;
            }
            return Utils.ConvertToInt32(result, 0);
        }

        // Get Bao Cao Phan Anh Kien Nghi new
        public IList<BaoCaoPhanAnhKienNghiInfo> GetBaoCao(DateTime startDate, DateTime endDate, IList<CoQuanInfo> cqList)
        {
            var pList = new SqlParameter("@ListCoQuanID", SqlDbType.Structured);
            pList.TypeName = "dbo.IntList";
            IList<BaoCaoPhanAnhKienNghiInfo> infoList = new List<BaoCaoPhanAnhKienNghiInfo>();
            DataTable tbCoQuanID = new DataTable();
            tbCoQuanID.Columns.Add("CoQuanID", typeof(string));
            foreach (CoQuanInfo cqInfo in cqList)
            {
                tbCoQuanID.Rows.Add(cqInfo.CoQuanID);
            }

            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_STARTDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_ENDDATE, SqlDbType.DateTime),
                pList
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            parm[2].Value = tbCoQuanID;
            try
            {
                //var st = new Stopwatch();
                //st.Start();
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "BaoCaoPhanAnhKienNghi_New_v2", parm))
                {
                    while (dr.Read())
                    {
                        BaoCaoPhanAnhKienNghiInfo info = GetDataBaoCao(dr);
                        infoList.Add(info);
                    }
                }
                //st.Stop();
                //var t = st.Elapsed;
            }
            catch (Exception e)
            {
                throw;
            }
            return infoList;
        }
        private BaoCaoPhanAnhKienNghiInfo GetDataBaoCao(SqlDataReader rdr)
        {
            BaoCaoPhanAnhKienNghiInfo info = new BaoCaoPhanAnhKienNghiInfo();
            //info.Col1Data = Utils.ConvertToInt32(rdr["Col1"], 0);
            info.Col2Data = Utils.ConvertToInt32(rdr["Col2Data"], 0);
            info.Col3Data = Utils.ConvertToInt32(rdr["Col3Data"], 0);
            info.Col4Data = Utils.ConvertToInt32(rdr["Col4Data"], 0);
            //info.Col5Data = Utils.ConvertToInt32(rdr["Col5"], 0);
            info.Col6Data = Utils.ConvertToInt32(rdr["Col6Data"], 0);
            info.Col7Data = Utils.ConvertToInt32(rdr["Col7Data"], 0);
            info.Col8Data = Utils.ConvertToInt32(rdr["Col8Data"], 0);
            info.Col9Data = Utils.ConvertToInt32(rdr["Col9Data"], 0);
            info.Col10Data = Utils.ConvertToInt32(rdr["Col10Data"], 0);
            info.Col11Data = Utils.ConvertToInt32(rdr["Col11Data"], 0);
            info.Col12Data = Utils.ConvertToInt32(rdr["Col12Data"], 0);
            info.Col13Data = Utils.ConvertToInt32(rdr["Col13Data"], 0);
            info.Col14Data = Utils.ConvertToInt32(rdr["Col14Data"], 0);
            info.Col15Data = Utils.ConvertToInt32(rdr["Col15Data"], 0);
            info.Col16Data = Utils.ConvertToInt32(rdr["Col16Data"], 0);
            info.Col17Data = Utils.ConvertToInt32(rdr["Col17Data"], 0);
            info.Col18Data = Utils.ConvertToInt32(rdr["Col18Data"], 0);
            info.Col19Data = Utils.ConvertToInt32(rdr["Col19Data"], 0);
            info.Col20Data = Utils.ConvertToInt32(rdr["Col20Data"], 0);
            info.Col21Data = Utils.ConvertToInt32(rdr["Col21Data"], 0);
            info.Col22Data = Utils.ConvertToInt32(rdr["Col22Data"], 0);
            info.Col23Data = Utils.ConvertToInt32(rdr["Col23Data"], 0);
            info.Col24Data = Utils.ConvertToInt32(rdr["Col24Data"], 0);
            info.Col25Data = Utils.ConvertToInt32(rdr["Col25Data"], 0);
            info.Col26Data = Utils.ConvertToInt32(rdr["Col26Data"], 0);
            info.Col27Data = Utils.ConvertToInt32(rdr["Col27Data"], 0);
            info.Col28Data = Utils.ConvertToInt32(rdr["Col28Data"], 0);
            info.SlCol10 = Utils.ConvertToInt32(rdr["slCol10"], 0);
            info.SlCol11 = Utils.ConvertToInt32(rdr["slCol11"], 0);
            info.SlCol12 = Utils.ConvertToInt32(rdr["slCol12"], 0);
            info.SlCol13 = Utils.ConvertToInt32(rdr["slCol13"], 0);
            info.SlCol14 = Utils.ConvertToInt32(rdr["slCol14"], 0);
            info.SlCol15 = Utils.ConvertToInt32(rdr["slCol15"], 0);
            info.SlCol16 = Utils.ConvertToInt32(rdr["slCol16"], 0);
            info.SlCol21 = Utils.ConvertToInt32(rdr["slCol21"], 0);
            info.SlCol22 = Utils.ConvertToInt32(rdr["slCol22"], 0);
            info.SlCol23 = Utils.ConvertToInt32(rdr["slCol23"], 0);
            info.SlCol24 = Utils.ConvertToInt32(rdr["slCol24"], 0);
            info.SlCol25 = Utils.ConvertToInt32(rdr["slCol25"], 0);
            info.SlCol26 = Utils.ConvertToInt32(rdr["slCol26"], 0);
            info.SlCol27 = Utils.ConvertToInt32(rdr["slCol27"], 0);
            info.SlCol28 = Utils.ConvertToInt32(rdr["slCol28"], 0);
            info.CoQuanID = Utils.ConvertToInt32(rdr["CoQuanID"], 0);
            return info;
        }
    }
}
