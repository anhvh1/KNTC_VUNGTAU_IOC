using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Ultilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.DAL.KNTC
{
    public class ThongKeDonThu
    {
        private const string GET_DONTHU_BY_COQUAN = "ThongKeDonThu_GetDonThuByCoQuan";
        private const string GET_DONTHUNOIPHATSINH_BY_COQUAN = "ThongKeDonThu_GetDonThuNoiPhatSinhByCoQuan";
        private const string GET_DONTHU_BY_COQUAN_FULLTENLOAIKHIEUTO = "ThongKeDonThu_GetDonThuByCoQuan_FullTenLoaiKhieuTo";

        private const string GET_DONTHU_BY_COQUAN_JOIN_YKIENGQ = "ThongKeDonThu_GetDonThuByCoQuan_Join_YKienGQ";
        private const string GET_DONTHU_BY_TINH = "ThongKeDonThu_GetDonThuByTinh";
        private const string GET_DONTHUNOIPHATSINH_BY_TINH = "ThongKeDonThuNoiPhatSinh_GetDonThuByTinh";
        private const string GET_DONTHU = "ThongKeDonThu_GetDonThu";
        private const string GET_CHUYEN_GQ_JOIN_YKIENGQ = @"ChuyenGiaiQuyet_Join_YKienGQ";

        //For thong ke theo vu viec dong nguoi
        private const string GET_DONTHU_BY_COQUANID = "ThongKeDonThu_GetDonThuByCoQuanId";

        //For thong ke theo vu viec qua han
        private const string GET_DONTHUQUAHAN_BY_COQUANID = "ThongKeDonThu_GetDonThuQuaHanByCoQuanId";

        //For thong ke theo vụ viec keo dai phuc tap
        private const string GET_DONTHUKEODAI_BY_COQUANID = "ThongKeDonThu_GetDonThuPhucTapKeoDai";

        private const string GET_IN_TIME = "ThongKeDonThu_GetDonThuTiepDan";
        private const string GET_DON_THU_DA_DUOC_XU_LY = "ThongKeDonThu_GetDonThuDaXuLy";

        private const string PARM_STARTDATE = "@StartDate";
        private const string PARM_ENDDATE = "@EndDate";
        private const string PARM_COQUANID = "@CoQuanID";
        private const string PARM_TINHID = "@TinhID";
        private const string PARM_HUYENID = "@HuyenID";
        private const string PARM_XAID = "@XaID";
        private const string PARAM_LOAIKHIEUTOID = "@LoaiKhieuToID";

        private const string PARAM_TUNGAY = "TuNgay";
        private const string PARAM_DENNGAY = "DenNgay";
        private const string PARAM_COQUANPHANID = "@CoQuanPhanID";
        private const string PARM_COQUANGQID = "@CoQuanGiaiQuyetID";
        private const string PARM_START = "@Start";
        private const string PARM_END = "@End";
        private const string PARM_INDEX = "@Index";
        private const string PARM_TYPE = "@Type";
        private const string PARM_DIAPHUONGID = "@CoQuanChuDonID";

        private TKDonThuInfo GetData(SqlDataReader rdr)
        {
            TKDonThuInfo dtInfo = new TKDonThuInfo();
            dtInfo.DonThuID = Utils.ConvertToInt32(rdr["DonThuID"], 0);
            dtInfo.LoaiKhieuTo1ID = Utils.ConvertToInt32(rdr["LoaiKhieuTo1ID"], 0);
            dtInfo.LoaiKhieuTo2ID = Utils.ConvertToInt32(rdr["LoaiKhieuTo2ID"], 0);
            dtInfo.LoaiKhieuTo3ID = Utils.ConvertToInt32(rdr["LoaiKhieuTo3ID"], 0);
            dtInfo.CoQuanChuyenDonID = Utils.ConvertToInt32(rdr["CQChuyenDonDenID"], 0);
            dtInfo.TinhID = Utils.ConvertToInt32(rdr["TinhID"], 0);
            dtInfo.HuyenID = Utils.ConvertToInt32(rdr["HuyenID"], 0);
            dtInfo.XaID = Utils.ConvertToInt32(rdr["XaID"], 0);
            return dtInfo;
        }

        private TKDonThuInfo GetDataChiTiet(SqlDataReader rdr)
        {
            TKDonThuInfo dtInfo = new TKDonThuInfo();
            dtInfo = GetData(rdr);
            dtInfo.SoDon = Utils.ConvertToString(rdr["SoDonThu"], string.Empty);
            dtInfo.TenChuDon = Utils.ConvertToString(rdr["HoTen"], string.Empty);
            dtInfo.DiaChi = Utils.ConvertToString(rdr["DiaChiCT"], string.Empty);
            dtInfo.NoiDungDon = Utils.ConvertToString(rdr["NoiDungDon"], string.Empty);
            dtInfo.TenLoaiKhieuTo = Utils.ConvertToString(rdr["TenLoaiKhieuTo"], string.Empty);
            dtInfo.NgayNhapDon = Utils.ConvertToDateTime(rdr["NgayNhapDon"], DateTime.MinValue);

            return dtInfo;

        }

        private TKDonThuInfo GetDataForThongKeVuViecDongNguoi(SqlDataReader rdr)
        {
            TKDonThuInfo dtInfo = new TKDonThuInfo();
            dtInfo.TenChuDon = Utils.ConvertToString(rdr["HoTen"], string.Empty);
            dtInfo.NgayNhapDon = Utils.ConvertToDateTime(rdr["NgayNhapDon"], DateTime.MinValue);
            dtInfo.NgayXuLy = Utils.ConvertToDateTime(rdr["NgayXuLy"], DateTime.MinValue);
            dtInfo.SoLuong = Utils.ConvertToInt32(rdr["SoLuong"], 0);
            return dtInfo;
        }
        private TKDonThuInfo GetDataForThongKeVuViecQuaHan(SqlDataReader rdr)
        {
            TKDonThuInfo dtInfo = new TKDonThuInfo();
            dtInfo.TenChuDon = Utils.ConvertToString(rdr["HoTen"], string.Empty);
            dtInfo.NgayQuaHan = Utils.ConvertToDateTime(rdr["NgayQuaHan"], DateTime.MinValue);
            dtInfo.NgayNhapDon = Utils.ConvertToDateTime(rdr["NgayNhapDon"], DateTime.MinValue);
            return dtInfo;
        }
        private TKDonThuInfo GetDataForThongKeVuViecKeoDai(SqlDataReader rdr)
        {
            TKDonThuInfo dtInfo = new TKDonThuInfo();
            dtInfo.TenChuDon = Utils.ConvertToString(rdr["HoTen"], string.Empty);
            dtInfo.NgayNhapDon = Utils.ConvertToDateTime(rdr["NgayNhapDon"], DateTime.MinValue);
            dtInfo.NgayXuLy = Utils.ConvertToDateTime(rdr["NgayXuLy"], DateTime.MinValue);
            dtInfo.LanTiep = Utils.ConvertToInt32(rdr["SoLanTiep"], 0);
            return dtInfo;
        }

        public IList<TKDonThuInfo> GetDonThu(DateTime startDate, DateTime endDate)
        {
            IList<TKDonThuInfo> infoList = new List<TKDonThuInfo>();
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_STARTDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_ENDDATE, SqlDbType.DateTime)
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DONTHU, parm))
                {
                    while (dr.Read())
                    {
                        TKDonThuInfo info = GetData(dr);
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

        public IList<TKDonThuInfo> GetDonThuByTinh(DateTime startDate, DateTime endDate, int tinhID)
        {
            IList<TKDonThuInfo> infoList = new List<TKDonThuInfo>();
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
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DONTHU_BY_TINH, parm))
                {
                    while (dr.Read())
                    {
                        TKDonThuInfo info = GetDataChiTiet(dr);
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
        String GET_DONTHUNOIPHATSINH_BY_TINH_NEW = "ThongKeDonThuNoiPhatSinh_GetDonThuByTinh_New";
        public IList<TKDonThuInfo> GetDonThuNoiPhatSinhByTinh(DateTime startDate, DateTime endDate, int tinhID, int start, int end)
        {
            IList<TKDonThuInfo> infoList = new List<TKDonThuInfo>();
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_STARTDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_ENDDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_TINHID, SqlDbType.Int),
                new SqlParameter(PARM_START, SqlDbType.Int),
                new SqlParameter(PARM_END, SqlDbType.Int)

            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            parm[2].Value = tinhID;
            parm[3].Value = start;
            parm[4].Value = end;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DONTHUNOIPHATSINH_BY_TINH_NEW, parm))
                {
                    while (dr.Read())
                    {
                        TKDonThuInfo info = GetDataChiTiet(dr);
                        info.DonThuID = Utils.ConvertToInt32(dr["DonThuID"], 0);
                        info.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
                        info.DiaChiPhatSinh = Utils.ConvertToString(dr["DiaChiPhatSinh"], string.Empty);
                        info.TenTinh = Utils.ConvertToString(dr["TenTinh"], string.Empty);
                        info.TenHuyen = Utils.ConvertToString(dr["TenHuyen"], string.Empty);
                        info.TenXa = Utils.ConvertToString(dr["TenXa"], string.Empty);
                        info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        info.DiaChiCTPhatSinh = string.Empty;
                        if (info.TenXa != string.Empty)
                        {
                            if (info.DiaChiPhatSinh != string.Empty)
                            {
                                info.DiaChiCTPhatSinh = info.DiaChiPhatSinh + ", " + info.TenXa;
                            }
                            else
                            {
                                info.DiaChiCTPhatSinh = info.TenXa;
                            }

                        }
                        if (info.TenHuyen != string.Empty)
                        {
                            info.DiaChiCTPhatSinh = info.DiaChiCTPhatSinh + ", " + info.TenHuyen;
                        }
                        if (info.TenTinh != string.Empty)
                        {
                            info.DiaChiCTPhatSinh = info.DiaChiCTPhatSinh + ", " + info.TenTinh;
                        }
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

        public List<ThongKeInfo> ThongKeDonThuChuyenCoQuan_GetDonThuByCoQuan(DateTime startDate, DateTime endDate, int coQuanID)
        {
            List<ThongKeInfo> infoList = new List<ThongKeInfo>();
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_STARTDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_ENDDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_COQUANID, SqlDbType.Int)
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            parm[2].Value = coQuanID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "BaoCao_ThongKeDonThuChuyenCoQuan_GetDonThuByCoQuan", parm))
                {
                    while (dr.Read())
                    {
                        ThongKeInfo info = new ThongKeInfo();
                        info.Ten = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        info.SoLuong = Utils.ConvertToInt32(dr["SoLuongChuyen"], 0);
                        info.TyLe = Utils.ConvertToDouble(dr["TyLe"], 0);
                        infoList.Add(info);
                    }
                }
            }
            catch
            {
                throw;
            }
            return infoList;
        }

        public List<ThongKeInfo> GetcoQuanDonThuChuyenDen_GetDonThuByCoQuan(DateTime startDate, DateTime endDate, int coQuanID, int coQuanChuyenDenID)
        {
            List<ThongKeInfo> infoList = new List<ThongKeInfo>();
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_STARTDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_ENDDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_COQUANID, SqlDbType.Int),
                new SqlParameter("@CoQuanChuyenDenID", SqlDbType.Int),
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            parm[2].Value = coQuanID;
            parm[3].Value = coQuanChuyenDenID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "BaoCao_CoQuanCoDonThuChuyenDen_GetDonThuByCoQuanID", parm))
                {
                    while (dr.Read())
                    {
                        ThongKeInfo info = new ThongKeInfo();
                        info.Ten = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        info.SoLuong = Utils.ConvertToInt32(dr["SoLuongNhan"], 0);
                        info.CoQuanChuyenID = Utils.ConvertToInt32(dr["CQChuyenDonID"], 0);
                        info.TenCoQuanChuyen = Utils.ConvertToString(dr["TenCoQuanChuyen"], string.Empty);
                        infoList.Add(info);
                    }
                }
            }
            catch
            {
                throw;
            }
            return infoList;
        }

        public List<ThongKeInfo> ThongKeDonThuChuyenCoQuanDi_GetDonThuByCoQuan(DateTime startDate, DateTime endDate, int coQuanID)
        {
            List<ThongKeInfo> infoList = new List<ThongKeInfo>();
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_STARTDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_ENDDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_COQUANID, SqlDbType.Int)
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            parm[2].Value = coQuanID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "BaoCao_ThongKeDonThuChuyenDi_GetDonThuByCoQuan", parm))
                {
                    while (dr.Read())
                    {
                        ThongKeInfo info = new ThongKeInfo();
                        info.Ten = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        info.SoLuong = Utils.ConvertToInt32(dr["SoLuongChuyen"], 0);
                        info.TyLe = Utils.ConvertToDouble(dr["TyLe"], 0);
                        info.CoQuanID = coQuanID;
                        infoList.Add(info);
                    }
                }
            }
            catch
            {
                throw;
            }
            return infoList;
        }

        public List<ThongKeInfo> ThongKeDonThuChuyenCoQuan_GetDonThuByHuyenID(DateTime startDate, DateTime endDate, int huyenID)
        {
            List<ThongKeInfo> infoList = new List<ThongKeInfo>();
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_STARTDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_ENDDATE, SqlDbType.DateTime),
                new SqlParameter("@HuyenID", SqlDbType.Int)
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            parm[2].Value = huyenID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "BaoCao_ThongKeDonThuChuyenCoQuan_GetDonThuByHuyenID", parm))
                {
                    while (dr.Read())
                    {
                        ThongKeInfo info = new ThongKeInfo();
                        info.Ten = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        info.SoLuong = Utils.ConvertToInt32(dr["SoLuongChuyen"], 0);
                        info.TyLe = Utils.ConvertToDouble(dr["TyLe"], 0);
                        infoList.Add(info);
                    }
                }
            }
            catch
            {
                throw;
            }
            return infoList;
        }

        public List<ThongKeInfo> GetcoQuanDonThuChuyenDen_GetDonThuByHuyenID(DateTime startDate, DateTime endDate, int huyenID, int coQuanChuyenDenID)
        {
            List<ThongKeInfo> infoList = new List<ThongKeInfo>();
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_STARTDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_ENDDATE, SqlDbType.DateTime),
                new SqlParameter("@HuyenID", SqlDbType.Int),
                new SqlParameter("@CoQuanChuyenDenID", SqlDbType.Int),
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            parm[2].Value = huyenID;
            parm[3].Value = coQuanChuyenDenID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "BaoCao_CoQuanCoDonThuChuyenDen_GetDonThuByHuyenID", parm))
                {
                    while (dr.Read())
                    {
                        ThongKeInfo info = new ThongKeInfo();
                        info.Ten = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        info.SoLuong = Utils.ConvertToInt32(dr["SoLuongNhan"], 0);
                        info.CoQuanChuyenID = Utils.ConvertToInt32(dr["CQChuyenDonID"], 0);
                        info.TenCoQuanChuyen = Utils.ConvertToString(dr["TenCoQuanChuyen"], string.Empty);
                        infoList.Add(info);
                    }
                }
            }
            catch
            {
                throw;
            }
            return infoList;
        }

        public List<ThongKeInfo> ThongKeDonThuChuyenCoQuanDi_GetDonThuByHuyenID(DateTime startDate, DateTime endDate, int huyenID)
        {
            List<ThongKeInfo> infoList = new List<ThongKeInfo>();
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_STARTDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_ENDDATE, SqlDbType.DateTime),
                new SqlParameter("@HuyenID", SqlDbType.Int)
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            parm[2].Value = huyenID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "BaoCao_ThongKeDonThuChuyenDi_GetDonThuByHuyenID", parm))
                {
                    while (dr.Read())
                    {
                        ThongKeInfo info = new ThongKeInfo();
                        info.Ten = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        info.SoLuong = Utils.ConvertToInt32(dr["SoLuongChuyen"], 0);
                        info.TyLe = Utils.ConvertToDouble(dr["TyLe"], 0);
                        info.HuyenID = huyenID;
                        infoList.Add(info);
                    }
                }
            }
            catch
            {
                throw;
            }
            return infoList;
        }

        public List<ThongKeInfo> ThongKeDonThuChuyenCoQuan_GetDonThuByTinhID(DateTime startDate, DateTime endDate, int tinhID)
        {
            List<ThongKeInfo> infoList = new List<ThongKeInfo>();
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
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "BaoCao_ThongKeDonThuChuyenCoQuan_GetDonThuByTinhID", parm))
                {
                    while (dr.Read())
                    {
                        ThongKeInfo info = new ThongKeInfo();
                        info.Ten = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        info.SoLuong = Utils.ConvertToInt32(dr["SoLuongChuyen"], 0);
                        info.TyLe = Utils.ConvertToDouble(dr["TyLe"], 0);
                        infoList.Add(info);
                    }
                }
            }
            catch
            {
                throw;
            }
            return infoList;
        }

        public List<ThongKeInfo> GetcoQuanDonThuChuyenDen_GetDonThuByTinhID(DateTime startDate, DateTime endDate, int tinhID, int coQuanChuyenDenID)
        {
            List<ThongKeInfo> infoList = new List<ThongKeInfo>();
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_STARTDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_ENDDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_TINHID, SqlDbType.Int),
                new SqlParameter("@CoQuanChuyenDenID", SqlDbType.Int),
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            parm[2].Value = tinhID;
            parm[3].Value = coQuanChuyenDenID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "BaoCao_CoQuanCoDonThuChuyenDen_GetDonThuByTinhID", parm))
                {
                    while (dr.Read())
                    {
                        ThongKeInfo info = new ThongKeInfo();
                        info.Ten = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        info.SoLuong = Utils.ConvertToInt32(dr["SoLuongNhan"], 0);
                        info.CoQuanChuyenID = Utils.ConvertToInt32(dr["CQChuyenDonID"], 0);
                        info.TenCoQuanChuyen = Utils.ConvertToString(dr["TenCoQuanChuyen"], string.Empty);
                        infoList.Add(info);
                    }
                }
            }
            catch
            {
                throw;
            }
            return infoList;
        }

        public List<ThongKeInfo> ThongKeDonThuChuyenCoQuanDi_GetDonThuByTinhID(DateTime startDate, DateTime endDate, int tinhID)
        {
            List<ThongKeInfo> infoList = new List<ThongKeInfo>();
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
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "BaoCao_ThongKeDonThuChuyenDi_GetDonThuByTinhID", parm))
                {
                    while (dr.Read())
                    {
                        ThongKeInfo info = new ThongKeInfo();
                        info.Ten = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        info.SoLuong = Utils.ConvertToInt32(dr["SoLuongChuyen"], 0);
                        info.TyLe = Utils.ConvertToDouble(dr["TyLe"], 0);
                        info.TinhID = tinhID;
                        infoList.Add(info);
                    }
                }
            }
            catch
            {
                throw;
            }
            return infoList;
        }

        public List<ThongKeInfo> ThongKeDonThuChuyenCoQuan_GetDonThu(DateTime startDate, DateTime endDate)
        {
            List<ThongKeInfo> infoList = new List<ThongKeInfo>();
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_STARTDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_ENDDATE, SqlDbType.DateTime),
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "BaoCao_ThongKeDonThuChuyenCoQuan_GetDonThu", parm))
                {
                    while (dr.Read())
                    {
                        ThongKeInfo info = new ThongKeInfo();
                        info.Ten = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        info.SoLuong = Utils.ConvertToInt32(dr["SoLuongChuyen"], 0);
                        info.TyLe = Utils.ConvertToDouble(dr["TyLe"], 0);
                        infoList.Add(info);
                    }
                }
            }
            catch
            {
                throw;
            }
            return infoList;
        }

        public List<ThongKeInfo> GetCoQuanCoDonThuChuyenDen_GetDonThu(DateTime startDate, DateTime endDate, int coQuanChuyenDenID)
        {
            List<ThongKeInfo> infoList = new List<ThongKeInfo>();
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_STARTDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_ENDDATE, SqlDbType.DateTime),
                new SqlParameter("@CoQuanChuyenDenID", SqlDbType.Int),
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            parm[2].Value = coQuanChuyenDenID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "BaoCao_CoQuanCoDonThuChuyenDen_GetDonThu", parm))
                {
                    while (dr.Read())
                    {
                        ThongKeInfo info = new ThongKeInfo();
                        info.Ten = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        info.SoLuong = Utils.ConvertToInt32(dr["SoLuongNhan"], 0);
                        info.CoQuanChuyenID = Utils.ConvertToInt32(dr["CQChuyenDonID"], 0);
                        info.TenCoQuanChuyen = Utils.ConvertToString(dr["TenCoQuanChuyen"], string.Empty);
                        infoList.Add(info);
                    }
                }
            }
            catch
            {
                throw;
            }
            return infoList;
        }

        public List<ThongKeInfo> ThongKeDonThuChuyenCoQuanDi_GetDonThu(DateTime startDate, DateTime endDate)
        {
            List<ThongKeInfo> infoList = new List<ThongKeInfo>();
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_STARTDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_ENDDATE, SqlDbType.DateTime),
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "BaoCao_ThongKeDonThuChuyenDi_GetDonThu", parm))
                {
                    while (dr.Read())
                    {
                        ThongKeInfo info = new ThongKeInfo();
                        info.Ten = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        info.SoLuong = Utils.ConvertToInt32(dr["SoLuongChuyen"], 0);
                        info.TyLe = Utils.ConvertToDouble(dr["TyLe"], 0);
                        infoList.Add(info);
                    }
                }
            }
            catch
            {
                throw;
            }
            return infoList;
        }

        public List<TKDonThuInfo> ThongKeDonThuChuyenCoQuan_GetDanhSachChiTiet_ByCoQuan(DateTime startDate, DateTime endDate, int cqID, int chitietCqID, int coQuanChuyenID, int start, int end, int xemTaiLieuMat, int canBoID)
        {
            List<TKDonThuInfo> infoList = new List<TKDonThuInfo>();
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_STARTDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_ENDDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_COQUANID, SqlDbType.Int),
                new SqlParameter("@ChiTietCoQuanID", SqlDbType.Int),
                new SqlParameter("@CoQuanChuyenID", SqlDbType.Int),
                new SqlParameter(PARM_START, SqlDbType.Int),
                new SqlParameter(PARM_END, SqlDbType.Int),
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            parm[2].Value = cqID;
            parm[3].Value = chitietCqID;
            parm[4].Value = coQuanChuyenID;
            parm[5].Value = start;
            parm[6].Value = end;
            var query = new DataTable();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "BaoCao_ThongKeDonThuChuyenCoQuan_GetDanhSachChiTiet_ByCoQuan_New", parm))
                {
                    query.Load(dr);
                }
            }
            catch (Exception ex)
            {
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
                info.NoiDungDon = Utils.ConvertToString(item.Field<string>("NoiDungDon"), string.Empty);
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
            return infoList;
        }

        public List<TKDonThuInfo> ThongKeDonThuChuyenCoQuan_GetDanhSachChiTiet_ByTinhID(DateTime startDate, DateTime endDate, int tinhID, int chitietCqID, int coQuanChuyenID, int start, int end, int xemTaiLieuMat, int canBoID)
        {
            List<TKDonThuInfo> infoList = new List<TKDonThuInfo>();
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_STARTDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_ENDDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_TINHID, SqlDbType.Int),
                new SqlParameter("@ChiTietCoQuanID", SqlDbType.Int),
                new SqlParameter("@CoQuanChuyenID", SqlDbType.Int),
                new SqlParameter(PARM_START, SqlDbType.Int),
                new SqlParameter(PARM_END, SqlDbType.Int),
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            parm[2].Value = tinhID;
            parm[3].Value = chitietCqID;
            parm[4].Value = coQuanChuyenID;
            parm[5].Value = start;
            parm[6].Value = end;
            var query = new DataTable();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "BaoCao_ThongKeDonThuChuyenCoQuan_GetDanhSachChiTiet_ByTinhID_New", parm))
                {
                    query.Load(dr);
                }
            }
            catch
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
                info.NoiDungDon = Utils.ConvertToString(item.Field<string>("NoiDungDon"), string.Empty);
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
            return infoList;
        }

        public List<TKDonThuInfo> ThongKeDonThuChuyenCoQuan_GetDanhSachChiTiet_ByHuyenID(DateTime startDate, DateTime endDate, int huyenID, int chitietCqID, int coQuanChuyenID, int start, int end, int xemTaiLieuMat, int canBoID)
        {
            List<TKDonThuInfo> infoList = new List<TKDonThuInfo>();
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_STARTDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_ENDDATE, SqlDbType.DateTime),
                new SqlParameter("@HuyenID", SqlDbType.Int),
                new SqlParameter("@ChiTietCoQuanID", SqlDbType.Int),
                new SqlParameter("@CoQuanChuyenID", SqlDbType.Int),
                new SqlParameter(PARM_START, SqlDbType.Int),
                new SqlParameter(PARM_END, SqlDbType.Int),
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            parm[2].Value = huyenID;
            parm[3].Value = chitietCqID;
            parm[4].Value = coQuanChuyenID;
            parm[5].Value = start;
            parm[6].Value = end;
            var query = new DataTable();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "BaoCao_ThongKeDonThuChuyenCoQuan_GetDanhSachChiTiet_ByHuyenID_New", parm))
                {
                    query.Load(dr);
                }
            }
            catch
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
                info.NoiDungDon = Utils.ConvertToString(item.Field<string>("NoiDungDon"), string.Empty);
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
            return infoList;
        }

        public IList<TKDonThuInfo> GetDonThuByCoQuanFullTenLoaiKhieuTo(DateTime startDate, DateTime endDate, int cqID)
        {
            IList<TKDonThuInfo> infoList = new List<TKDonThuInfo>();
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_STARTDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_ENDDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_COQUANID, SqlDbType.Int)
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            parm[2].Value = cqID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DONTHU_BY_COQUAN_FULLTENLOAIKHIEUTO, parm))
                {
                    while (dr.Read())
                    {
                        TKDonThuInfo info = GetDataChiTiet(dr);
                        info.NgayQuaHan = Utils.ConvertToDateTime(dr["NgayQuaHan"], DateTime.MinValue);
                        info.NgayChuyenGQ = Utils.ConvertToDateTime(dr["NgayChuyenGQ"], DateTime.MinValue);
                        info.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        info.CoQuanGiaiQuyetID = Utils.ConvertToInt32(dr["CoQuanGiaiQuyetID"], 0);
                        info.HuongXuLyID = Utils.ConvertToInt32(dr["HuongGiaiQuyetID"], 0);
                        info.KetQuaID = Utils.ConvertToInt32(dr["KetQuaID"], 0);
                        info.YKienGiaiQuyetID = Utils.ConvertToInt32(dr["YKienGiaiQuyetID"], 0);
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

        public IList<TKDonThuInfo> GetDonThuByCoQuan(DateTime startDate, DateTime endDate, int cqID)
        {

            IList<TKDonThuInfo> infoList = new List<TKDonThuInfo>();
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_STARTDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_ENDDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_COQUANID, SqlDbType.Int)
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            parm[2].Value = cqID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DONTHU_BY_COQUAN, parm))
                {
                    while (dr.Read())
                    {
                        TKDonThuInfo info = GetDataChiTiet(dr);
                        info.NgayQuaHan = Utils.ConvertToDateTime(dr["NgayQuaHan"], DateTime.MinValue);
                        info.NgayChuyenGQ = Utils.ConvertToDateTime(dr["NgayChuyenGQ"], DateTime.MinValue);
                        info.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        info.CoQuanGiaiQuyetID = Utils.ConvertToInt32(dr["CoQuanGiaiQuyetID"], 0);
                        info.HuongXuLyID = Utils.ConvertToInt32(dr["HuongGiaiQuyetID"], 0);
                        info.KetQuaID = Utils.ConvertToInt32(dr["KetQuaID"], 0);
                        info.YKienGiaiQuyetID = Utils.ConvertToInt32(dr["YKienGiaiQuyetID"], 0);
                        info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
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
        String GET_DANHSACH_DONTHU_CHUYENGQ = "BC_ChuyenGQ_DanhSach";
        public IList<TKDonThuInfo> GetDanhSachDonThu_BCChuyenGQ(int xemTaiLieuMat, int canBoID, DateTime startDate, DateTime endDate, int cqID, int cqGiaiQuyetID, int index, int start, int end)
        {

            IList<TKDonThuInfo> infoList = new List<TKDonThuInfo>();
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_STARTDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_ENDDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARM_COQUANGQID, SqlDbType.Int),
                new SqlParameter(PARM_INDEX, SqlDbType.Int),
                new SqlParameter(PARM_START, SqlDbType.Int),
                new SqlParameter(PARM_END, SqlDbType.Int),
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            parm[2].Value = cqID;
            parm[3].Value = cqGiaiQuyetID;
            parm[4].Value = index;
            parm[5].Value = start;
            parm[6].Value = end;
            var query = new DataTable();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DANHSACH_DONTHU_CHUYENGQ, parm))
                {
                    query.Load(dr);
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

            foreach (var item in dataRows)
            {
                // don tu info
                var info = new TKDonThuInfo();
                info.CoQuanID = Utils.ConvertToInt32(item.Field<int?>("CoQuanID"), 0);
                info.XuLyDonID = Utils.ConvertToInt32(item.Field<int?>("XuLyDonID"), 0);
                info.DonThuID = Utils.ConvertToInt32(item.Field<int?>("DonThuID"), 0);
                info.HuongXuLyID = Utils.ConvertToInt32(item.Field<int?>("HuongGiaiQuyetID"), 0);
                info.NgayNhapDon = Utils.ConvertToDateTime(item.Field<DateTime?>("NgayNhapDon"), DateTime.MinValue);
                info.NgayQuaHan = Utils.ConvertToDateTime(item.Field<DateTime?>("NgayQuaHan"), DateTime.MinValue);
                info.NgayChuyenGQ = Utils.ConvertToDateTime(item.Field<DateTime?>("NgayChuyenGQ"), DateTime.MinValue);
                info.KetQuaID = Utils.ConvertToInt32(item.Field<int?>("KetQuaID"), 0);
                info.SoDon = Utils.ConvertToString(item.Field<string>("SoDonThu"), string.Empty);
                info.DiaChi = Utils.ConvertToString(item.Field<string>("DiaChiCT"), string.Empty);
                info.NoiDungDon = Utils.ConvertToString(item.Field<string>("NoiDungDon"), string.Empty);
                info.TenChuDon = Utils.ConvertToString(item.Field<string>("HoTen"), string.Empty);
                info.NgayNhapDonStr = Format.FormatDate(info.NgayNhapDon);
                info.TenLoaiKhieuTo = Utils.ConvertToString(item.Field<string>("TenLoaiKhieuTo"), string.Empty);
                info.StateID = Utils.ConvertToInt32(item.Field<int?>("StateID"), 0);
                info.TenHuongGiaiQuyet = Utils.ConvertToString(item.Field<string>("TenHuongGiaiQuyet"), string.Empty);
                info.HuongGiaiQuyetExcel = info.TenHuongGiaiQuyet;

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
                info.KetQuaExcel = info.KetQuaID_Str;
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
            return infoList;
        }

        String GET_DONTHUNOIPHATSINH_BY_COQUAN_NEW = "ThongKeDonThu_GetDonThuNoiPhatSinhByCoQuan_New";
        public IList<TKDonThuInfo> GetDonThuNoiPhatSinhByCoQuan(DateTime startDate, DateTime endDate, int cqID, int start, int end)
        {
            IList<TKDonThuInfo> infoList = new List<TKDonThuInfo>();
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_STARTDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_ENDDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARM_START, SqlDbType.Int),
                new SqlParameter(PARM_END, SqlDbType.Int)
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            parm[2].Value = cqID;
            parm[3].Value = start;
            parm[4].Value = end;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DONTHUNOIPHATSINH_BY_COQUAN_NEW, parm))
                {
                    while (dr.Read())
                    {
                        TKDonThuInfo info = GetDataChiTiet(dr);
                        info.DonThuID = Utils.ConvertToInt32(dr["DonThuID"], 0);
                        info.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
                        info.NgayQuaHan = Utils.ConvertToDateTime(dr["NgayQuaHan"], DateTime.MinValue);
                        info.NgayChuyenGQ = Utils.ConvertToDateTime(dr["NgayChuyenGQ"], DateTime.MinValue);
                        info.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        info.CoQuanGiaiQuyetID = Utils.ConvertToInt32(dr["CoQuanGiaiQuyetID"], 0);
                        info.HuongXuLyID = Utils.ConvertToInt32(dr["HuongGiaiQuyetID"], 0);
                        info.KetQuaID = Utils.ConvertToInt32(dr["KetQuaID"], 0);
                        info.YKienGiaiQuyetID = Utils.ConvertToInt32(dr["YKienGiaiQuyetID"], 0);
                        info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        info.DiaChiPhatSinh = Utils.ConvertToString(dr["DiaChiPhatSinh"], string.Empty);
                        info.TenTinh = Utils.ConvertToString(dr["TenTinh"], string.Empty);
                        info.TenHuyen = Utils.ConvertToString(dr["TenHuyen"], string.Empty);
                        info.TenXa = Utils.ConvertToString(dr["TenXa"], string.Empty);
                        info.DiaChiCTPhatSinh = string.Empty;
                        if (info.TenXa != string.Empty)
                        {
                            if (info.DiaChiPhatSinh != string.Empty)
                            {
                                info.DiaChiCTPhatSinh = info.DiaChiPhatSinh + ", " + info.TenXa;
                            }
                            else
                            {
                                info.DiaChiCTPhatSinh = info.TenXa;
                            }

                        }
                        if (info.TenHuyen != string.Empty)
                        {
                            info.DiaChiCTPhatSinh = info.DiaChiCTPhatSinh + ", " + info.TenHuyen;
                        }
                        if (info.TenTinh != string.Empty)
                        {
                            info.DiaChiCTPhatSinh = info.DiaChiCTPhatSinh + ", " + info.TenTinh;
                        }
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

        String GET_DONTHUNOIPHATSINH_BY_LIST_COQUAN = "ThongKeDonThu_GetDonThuNoiPhatSinhByListCoQuan";
        public IList<TKDonThuInfo> GetDonThuNoiPhatSinhByListCoQuan(DateTime startDate, DateTime endDate, List<CoQuanInfo> coQuans, int start, int end)
        {
            var pList = new SqlParameter("@ListCoQuanID", SqlDbType.Structured);
            pList.TypeName = "dbo.IntList";
            var tbCoQuanID = new DataTable();
            tbCoQuanID.Columns.Add("CoQuanID", typeof(string));
            coQuans.ForEach(x => tbCoQuanID.Rows.Add(x.CoQuanID));

            IList<TKDonThuInfo> infoList = new List<TKDonThuInfo>();
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_STARTDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_ENDDATE, SqlDbType.DateTime),
                pList,
                new SqlParameter(PARM_START, SqlDbType.Int),
                new SqlParameter(PARM_END, SqlDbType.Int)
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            parm[2].Value = tbCoQuanID;
            parm[3].Value = start;
            parm[4].Value = end;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DONTHUNOIPHATSINH_BY_LIST_COQUAN, parm))
                {
                    while (dr.Read())
                    {
                        TKDonThuInfo info = GetDataChiTiet(dr);
                        info.DonThuID = Utils.ConvertToInt32(dr["DonThuID"], 0);
                        info.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
                        info.NgayQuaHan = Utils.ConvertToDateTime(dr["NgayQuaHan"], DateTime.MinValue);
                        info.NgayChuyenGQ = Utils.ConvertToDateTime(dr["NgayChuyenGQ"], DateTime.MinValue);
                        info.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        info.CoQuanGiaiQuyetID = Utils.ConvertToInt32(dr["CoQuanGiaiQuyetID"], 0);
                        info.HuongXuLyID = Utils.ConvertToInt32(dr["HuongGiaiQuyetID"], 0);
                        info.KetQuaID = Utils.ConvertToInt32(dr["KetQuaID"], 0);
                        info.YKienGiaiQuyetID = Utils.ConvertToInt32(dr["YKienGiaiQuyetID"], 0);
                        info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        info.DiaChiPhatSinh = Utils.ConvertToString(dr["DiaChiPhatSinh"], string.Empty);
                        info.TenTinh = Utils.ConvertToString(dr["TenTinh"], string.Empty);
                        info.TenHuyen = Utils.ConvertToString(dr["TenHuyen"], string.Empty);
                        info.TenXa = Utils.ConvertToString(dr["TenXa"], string.Empty);
                        info.DiaChiCTPhatSinh = string.Empty;
                        if (info.TenXa != string.Empty)
                        {
                            if (info.DiaChiPhatSinh != string.Empty)
                            {
                                info.DiaChiCTPhatSinh = info.DiaChiPhatSinh + ", " + info.TenXa;
                            }
                            else
                            {
                                info.DiaChiCTPhatSinh = info.TenXa;
                            }

                        }
                        if (info.TenHuyen != string.Empty)
                        {
                            info.DiaChiCTPhatSinh = info.DiaChiCTPhatSinh + ", " + info.TenHuyen;
                        }
                        if (info.TenTinh != string.Empty)
                        {
                            info.DiaChiCTPhatSinh = info.DiaChiCTPhatSinh + ", " + info.TenTinh;
                        }
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

        public IList<TKDonThuInfo> GetDonThuByCoQuan_Join_YKienGQ(DateTime startDate, DateTime endDate, int cqID)
        {
            IList<TKDonThuInfo> infoList = new List<TKDonThuInfo>();
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_STARTDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_ENDDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_COQUANID, SqlDbType.Int)
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            parm[2].Value = cqID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DONTHU_BY_COQUAN_JOIN_YKIENGQ, parm))
                {
                    while (dr.Read())
                    {
                        TKDonThuInfo info = GetDataChiTiet(dr);
                        info.NgayQuaHan = Utils.ConvertToDateTime(dr["NgayQuaHan"], DateTime.MinValue);
                        info.NgayChuyenGQ = Utils.ConvertToDateTime(dr["NgayChuyenGQ"], DateTime.MinValue);
                        info.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        info.CoQuanGiaiQuyetID = Utils.ConvertToInt32(dr["CoQuanGiaiQuyetID"], 0);
                        info.HuongXuLyID = Utils.ConvertToInt32(dr["HuongGiaiQuyetID"], 0);
                        info.KetQuaID = Utils.ConvertToInt32(dr["KetQuaID"], 0);
                        info.YKienGiaiQuyetID = Utils.ConvertToInt32(dr["YKienGiaiQuyetID"], 0);
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

        public IList<TKDonThuInfo> GetInTime(DateTime startDate, DateTime endDate, int coquanID)
        {
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARM_COQUANID, SqlDbType.Int)
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            parm[2].Value = coquanID;
            IList<TKDonThuInfo> LsTDKD = new List<TKDonThuInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_IN_TIME, parm))
                {

                    while (dr.Read())
                    {

                        TKDonThuInfo TDKDInfo = GetDataForShowManage(dr);
                        TDKDInfo.CanBoTiepID = Utils.ConvertToInt32(dr["CanBoTiepID"], 0);
                        TDKDInfo.NguonDonDen = Utils.ConvertToInt32(dr["NguonDonDen"], 0);
                        if (TDKDInfo.NguonDonDen == 21)
                            TDKDInfo.NgayQuaHan = Utils.ConvertToDateTime(dr["HanXuLyDueDate"], DateTime.MaxValue);
                        else
                            TDKDInfo.NgayQuaHan = Utils.ConvertToDateTime(dr["HanXuLy"], DateTime.MaxValue);
                        LsTDKD.Add(TDKDInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return LsTDKD;
        }

        private TKDonThuInfo GetDataForShowManage(SqlDataReader dr)
        {
            TKDonThuInfo TDKDInfo = new TKDonThuInfo();
            TDKDInfo.TiepDanKhongDonID = Utils.GetInt32(dr["TiepDanKhongDonID"], 0);
            TDKDInfo.DonThuID = Utils.GetInt32(dr["DonThuID"], 0);
            TDKDInfo.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);
            TDKDInfo.NgayTiep = Utils.GetDateTime(dr["NgayTiep"], Constant.DEFAULT_DATE);
            TDKDInfo.NgayGapLanhDao = Utils.GetDateTime(dr["NgayGapLanhDao"], Constant.DEFAULT_DATE);
            TDKDInfo.NoiDungTiep = Utils.ConvertToString(dr["NoiDungTiep"].ToString(), string.Empty);
            TDKDInfo.NoiDungDon = Utils.ConvertToString(dr["NoiDungDon"].ToString(), string.Empty);
            TDKDInfo.VuViecCu = Utils.GetBoolean(dr["VuViecCu"], false);
            if (TDKDInfo.VuViecCu)
            {
                TDKDInfo.TrangThaiVuViec = "Cũ";
            }
            else
            {
                TDKDInfo.TrangThaiVuViec = "Mới";
            }
            TDKDInfo.TenCanBo = Utils.ConvertToString(dr["TenCanBo"].ToString(), string.Empty);
            if (Utils.ConvertToBoolean(dr["GapLanhDao"], false) == true)
            {
                TDKDInfo.TenCanBo = Utils.ConvertToString(dr["TenLanhDaoTiep"].ToString(), string.Empty);
            }

            TDKDInfo.TenCoQuan = Utils.GetString(dr["TenCoQuan"].ToString(), string.Empty);
            TDKDInfo.TenLoaiKhieuTo = Utils.GetString(dr["TenLoaiKhieuTo"].ToString(), string.Empty);
            TDKDInfo.SoDon = Utils.GetString(dr["SoDon"], string.Empty);
            TDKDInfo.TenChuDon = Utils.ConvertToString(dr["HoTen"], string.Empty);
            TDKDInfo.DiaChi = Utils.ConvertToString(dr["DiaChiCT"], string.Empty);
            TDKDInfo.NhomKNID = Utils.GetInt32(dr["NhomKNID"], 0);
            TDKDInfo.PhongBanID = Utils.GetInt32(dr["PhongBanID"], 0);

            return TDKDInfo;
        }

        public IList<TKDonThuInfo> GetDonThuDaXuLy(DateTime startDate, DateTime endDate, int coQuanID)
        {
            IList<TKDonThuInfo> ListInfo = new List<TKDonThuInfo>();

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime)
            };

            parms[0].Value = coQuanID;
            parms[1].Value = startDate;
            parms[2].Value = endDate;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DON_THU_DA_DUOC_XU_LY, parms))
                {
                    while (dr.Read())
                    {
                        TKDonThuInfo Info = GetDataDonThuDaXuLy(dr);
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

        public TKDonThuInfo GetDataDonThuDaXuLy(SqlDataReader dr)
        {
            TKDonThuInfo info = new TKDonThuInfo();

            info.NgayXuLy = Utils.ConvertToDateTime(dr["NgayXuLy"], DateTime.MinValue);
            info.NgayQuaHan = Utils.ConvertToDateTime(dr["HanXuLy"], DateTime.MaxValue);
            info.CanBoXuLyID = Utils.ConvertToInt32(dr["CanBoXuLyID"], 0);
            info.PhongBanID = Utils.ConvertToInt32(dr["PhongBanID"], 0);
            info.TenChuDon = Utils.ConvertToString(dr["HoTen"], string.Empty);
            info.DiaChi = Utils.ConvertToString(dr["DiaChiCT"], string.Empty);
            info.NoiDungDon = Utils.ConvertToString(dr["NoiDungDon"], string.Empty);
            info.SoDon = Utils.GetString(dr["SoDonThu"], string.Empty);
            info.NgayTiep = Utils.GetDateTime(dr["NgayNhapDon"], Constant.DEFAULT_DATE);
            info.TenLoaiKhieuTo = Utils.GetString(dr["TenLoaiKhieuTo"].ToString(), string.Empty);

            return info;
        }

        public IList<TKDonThuInfo> GetDonThuQuaHanByCoQuanID(DateTime startDate, DateTime endDate, int cqID)
        {
            IList<TKDonThuInfo> infoList = new List<TKDonThuInfo>();
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_STARTDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_ENDDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_COQUANID, SqlDbType.Int)
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            parm[2].Value = cqID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DONTHUQUAHAN_BY_COQUANID, parm))
                {
                    while (dr.Read())
                    {
                        TKDonThuInfo info = GetDataForThongKeVuViecQuaHan(dr);
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

        public IList<TKDonThuInfo> GetDonThuKeoDaiByCoQuanID(DateTime startDate, DateTime endDate, int cqID)
        {
            IList<TKDonThuInfo> infoList = new List<TKDonThuInfo>();
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_STARTDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_ENDDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_COQUANID, SqlDbType.Int)
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            parm[2].Value = cqID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DONTHUKEODAI_BY_COQUANID, parm))
                {
                    while (dr.Read())
                    {
                        TKDonThuInfo info = GetDataForThongKeVuViecKeoDai(dr);
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

        public List<TKDonThuInfo> GetChuyenGQJoinYKienGQ(int coQuanID, DateTime startDate, DateTime endDate)
        {

            List<TKDonThuInfo> cgqList = new List<TKDonThuInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_COQUANPHANID,SqlDbType.Int),
                new SqlParameter("StartDate", SqlDbType.DateTime),
                new SqlParameter("EndDate", SqlDbType.DateTime)

            };
            parameters[0].Value = coQuanID;
            parameters[1].Value = startDate;
            parameters[2].Value = endDate;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_CHUYEN_GQ_JOIN_YKIENGQ, parameters))
                {

                    while (dr.Read())
                    {
                        TKDonThuInfo cgqInfo = new TKDonThuInfo();
                        cgqInfo.CoQuanGiaiQuyetID = Utils.ConvertToInt32(dr["CoQuanGiaiQuyetID"], 0);
                        cgqInfo.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        cgqInfo.TenCoQuanGQ = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        cgqInfo.KetQuaID = Utils.ConvertToInt32(dr["KetQuaID"], 0);
                        cgqInfo.YKienGiaiQuyetID = Utils.ConvertToInt32(dr["YKienGiaiQuyetID"], 0);

                        cgqList.Add(cgqInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return cgqList;
        }

        String GET_CT_DONTHUNOIPHATSINH = "ThongKeDonThuNoiPhatSinh_GetDanhSachDonThu";
        public IList<TKDonThuInfo> GetCTDonThuNoiPhatSinh(DateTime startDate, DateTime endDate, int tinhID, int huyenID, int xaID, int loaiKhieuToID, int start, int end, int type, List<CoQuanInfo> coQuans, int xemTaiLieuMat, int canBoID)
        {
            var pList = new SqlParameter("@ListCoQuanID", SqlDbType.Structured);
            pList.TypeName = "dbo.IntList";
            var tbCoQuanID = new DataTable();
            tbCoQuanID.Columns.Add("CoQuanID", typeof(string));
            coQuans.ForEach(x => tbCoQuanID.Rows.Add(x.CoQuanID));

            IList<TKDonThuInfo> infoList = new List<TKDonThuInfo>();
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_STARTDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_ENDDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_TINHID, SqlDbType.Int),
                new SqlParameter(PARM_HUYENID, SqlDbType.Int),
                new SqlParameter(PARM_XAID, SqlDbType.Int),
                new SqlParameter(PARAM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter(PARM_START, SqlDbType.Int),
                new SqlParameter(PARM_END, SqlDbType.Int),
                pList,
                new SqlParameter(PARM_TYPE, SqlDbType.Int),
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            parm[2].Value = tinhID;
            parm[3].Value = huyenID;
            parm[4].Value = xaID;
            parm[5].Value = loaiKhieuToID;
            parm[6].Value = start;
            parm[7].Value = end;
            parm[8].Value = tbCoQuanID;
            parm[9].Value = type;
            var query = new DataTable();
            try
            {
                //using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_CT_DONTHUNOIPHATSINH, parm))
                //{
                //    while (dr.Read())
                //    {
                //        TKDonThuInfo info = GetDataChiTiet(dr);
                //        info.DonThuID = Utils.ConvertToInt32(dr["DonThuID"], 0);
                //        info.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
                //        info.DiaChiPhatSinh = Utils.ConvertToString(dr["DiaChiPhatSinh"], string.Empty);
                //        info.TenTinh = Utils.ConvertToString(dr["TenTinh"], string.Empty);
                //        info.TenHuyen = Utils.ConvertToString(dr["TenHuyen"], string.Empty);
                //        info.TenXa = Utils.ConvertToString(dr["TenXa"], string.Empty);
                //        info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                //        info.DiaChiCTPhatSinh = string.Empty;
                //        if (info.TenXa != string.Empty)
                //        {
                //            if (info.DiaChiPhatSinh != string.Empty)
                //            {
                //                info.DiaChiCTPhatSinh = info.DiaChiPhatSinh + ", " + info.TenXa;
                //            }
                //            else
                //            {
                //                info.DiaChiCTPhatSinh = info.TenXa;
                //            }

                //        }
                //        if (info.TenHuyen != string.Empty)
                //        {
                //            info.DiaChiCTPhatSinh = info.DiaChiCTPhatSinh + ", " + info.TenHuyen;
                //        }
                //        if (info.TenTinh != string.Empty)
                //        {
                //            info.DiaChiCTPhatSinh = info.DiaChiCTPhatSinh + ", " + info.TenTinh;
                //        }
                //        infoList.Add(info);

                //    }
                //}
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_CT_DONTHUNOIPHATSINH, parm))
                {
                    query.Load(dr);
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
                info.TenTinh = Utils.ConvertToString(item.Field<string>("TenTinh"), string.Empty);
                info.TenHuyen = Utils.ConvertToString(item.Field<string>("TenHuyen"), string.Empty);
                info.TenXa = Utils.ConvertToString(item.Field<string>("TenXa"), string.Empty);
                info.DiaChi = Utils.ConvertToString(item.Field<string>("DiaChiCT"), string.Empty);
                info.DiaChiPhatSinh = Utils.ConvertToString(item.Field<string>("DiaChiPhatSinh"), string.Empty);
                info.DiaChiCTPhatSinh = "";
                if (info.TenXa != string.Empty)
                {
                    if (info.DiaChiPhatSinh != string.Empty)
                    {
                        info.DiaChiCTPhatSinh = info.DiaChiPhatSinh + ", " + info.TenXa;
                    }
                    else
                    {
                        info.DiaChiCTPhatSinh = info.TenXa;
                    }

                }
                if (info.TenHuyen != string.Empty)
                {
                    info.DiaChiCTPhatSinh = info.DiaChiCTPhatSinh + ", " + info.TenHuyen;
                }
                if (info.TenTinh != string.Empty)
                {
                    info.DiaChiCTPhatSinh = info.DiaChiCTPhatSinh + ", " + info.TenTinh;
                }
                info.NoiDungDon = Utils.ConvertToString(item.Field<string>("NoiDungDon"), string.Empty);
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

            return infoList;
        }

        String GET_CT_DONTHUNOIPHATSINH_TONG = "ThongKeDonThuNoiPhatSinh_GetDanhSachDonThu_Tong";
        public IList<TKDonThuInfo> GetCTDonThuTongNoiPhatSinh(DateTime startDate, DateTime endDate, int tinhID, int type, int loaiKhieuToID, int start, int end, List<CoQuanInfo> coQuans, List<HuyenInfo> listHuyen, List<XaInfo> listXa, int xemTaiLieuMat, int canBoID)
        {
            var pList = new SqlParameter("@ListCoQuanID", SqlDbType.Structured);
            pList.TypeName = "dbo.IntList";
            var tbCoQuanID = new DataTable();
            tbCoQuanID.Columns.Add("CoQuanID", typeof(string));
            coQuans.ForEach(x => tbCoQuanID.Rows.Add(x.CoQuanID));

            var pListHuyen = new SqlParameter("@ListHuyenID", SqlDbType.Structured);
            pListHuyen.TypeName = "dbo.IntList";
            var tbHuyenID = new DataTable();
            tbHuyenID.Columns.Add("HuyenID", typeof(string));
            listHuyen.ForEach(x => tbHuyenID.Rows.Add(x.HuyenID));

            var pListXa = new SqlParameter("@ListXaID", SqlDbType.Structured);
            pListXa.TypeName = "dbo.IntList";
            var tbXaID = new DataTable();
            tbXaID.Columns.Add("XaID", typeof(string));
            listXa.ForEach(x => tbXaID.Rows.Add(x.XaID));

            IList<TKDonThuInfo> infoList = new List<TKDonThuInfo>();
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_STARTDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_ENDDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_TINHID, SqlDbType.Int),
                new SqlParameter(PARM_TYPE, SqlDbType.Int),
                new SqlParameter(PARAM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter(PARM_START, SqlDbType.Int),
                new SqlParameter(PARM_END, SqlDbType.Int),
                pList,
                pListHuyen,
                pListXa
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            parm[2].Value = tinhID;
            parm[3].Value = type;
            parm[4].Value = loaiKhieuToID;
            parm[5].Value = start;
            parm[6].Value = end;
            parm[7].Value = tbCoQuanID;
            parm[8].Value = tbHuyenID;
            parm[9].Value = tbXaID;
            var query = new DataTable();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_CT_DONTHUNOIPHATSINH_TONG, parm))
                {
                    query.Load(dr);
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
                info.TenTinh = Utils.ConvertToString(item.Field<string>("TenTinh"), string.Empty);
                info.TenHuyen = Utils.ConvertToString(item.Field<string>("TenHuyen"), string.Empty);
                info.TenXa = Utils.ConvertToString(item.Field<string>("TenXa"), string.Empty);
                info.DiaChi = Utils.ConvertToString(item.Field<string>("DiaChiCT"), string.Empty);
                info.DiaChiPhatSinh = Utils.ConvertToString(item.Field<string>("DiaChiPhatSinh"), string.Empty);
                info.DiaChiCTPhatSinh = "";
                if (info.TenXa != string.Empty)
                {
                    if (info.DiaChiPhatSinh != string.Empty)
                    {
                        info.DiaChiCTPhatSinh = info.DiaChiPhatSinh + ", " + info.TenXa;
                    }
                    else
                    {
                        info.DiaChiCTPhatSinh = info.TenXa;
                    }

                }
                if (info.TenHuyen != string.Empty)
                {
                    info.DiaChiCTPhatSinh = info.DiaChiCTPhatSinh + ", " + info.TenHuyen;
                }
                if (info.TenTinh != string.Empty)
                {
                    info.DiaChiCTPhatSinh = info.DiaChiCTPhatSinh + ", " + info.TenTinh;
                }
                info.NoiDungDon = Utils.ConvertToString(item.Field<string>("NoiDungDon"), string.Empty);
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

            return infoList;
        }


        String BC_THEODCCHUDON = "BC_TheoDCChuDon";
        public IList<TKTheoNoiPhatSinhInfo> GetSoLieuBCTheoDCChuDon(DateTime startDate, DateTime endDate, int type, List<CoQuanInfo> coQuans, List<TinhInfo> tinhList, List<HuyenInfo> huyenList, List<XaInfo> xaList)
        {
            var pList = new SqlParameter("@ListCoQuanID", SqlDbType.Structured);
            pList.TypeName = "dbo.IntList";
            var tbCoQuanID = new DataTable();
            tbCoQuanID.Columns.Add("CoQuanID", typeof(string));
            coQuans.ForEach(x => tbCoQuanID.Rows.Add(x.CoQuanID));

            var pListID = new SqlParameter("@ListID", SqlDbType.Structured);
            pListID.TypeName = "dbo.IntList";
            var tb = new DataTable();
            if (type == 1)//tinh
            {
                tb.Columns.Add("TinhID", typeof(string));
                tinhList.ForEach(x => tb.Rows.Add(x.TinhID));
            }
            else if (type == 2)//huyen
            {
                tb.Columns.Add("HuyenID", typeof(string));
                huyenList.ForEach(x => tb.Rows.Add(x.HuyenID));
            }
            else if (type == 3)//xa
            {
                tb.Columns.Add("XaID", typeof(string));
                xaList.ForEach(x => tb.Rows.Add(x.XaID));
            }

            IList<TKTheoNoiPhatSinhInfo> infoList = new List<TKTheoNoiPhatSinhInfo>();
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_STARTDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_ENDDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_TYPE, SqlDbType.Int),
                pList,
                pListID
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            parm[2].Value = type;
            parm[3].Value = tbCoQuanID;
            parm[4].Value = tb;

            try
            {
                //var st1 = new Stopwatch();
                //st1.Start();
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, BC_THEODCCHUDON, parm))
                {
                    //st1.Stop();
                    //var t1 = st1.Elapsed;
                    while (dr.Read())
                    {
                        TKTheoNoiPhatSinhInfo info = new TKTheoNoiPhatSinhInfo();
                        if (type == 1)
                        {
                            info.TinhID = Utils.ConvertToInt32(dr["TinhID"], 0);
                        }
                        else if (type == 2)
                        {
                            info.HuyenID = Utils.ConvertToInt32(dr["HuyenID"], 0);
                        }
                        else if (type == 3)
                        {
                            info.XaID = Utils.ConvertToInt32(dr["XaID"], 0);
                        }
                        info.TongSo = Utils.ConvertToInt32(dr["Tong"], 0);
                        info.SLKhieuNai = Utils.ConvertToInt32(dr["KhieuNai"], 0);
                        info.SLToCao = Utils.ConvertToInt32(dr["ToCao"], 0);
                        info.SLKienNghi = Utils.ConvertToInt32(dr["KienNghi"], 0);
                        info.SLPhanAnh = Utils.ConvertToInt32(dr["PhanAnh"], 0);
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

        String GET_DONTHU_BC_THEO_DC_CHUDON = "BC_TheoDCChuDon_DanhSach";
        public IList<TKDonThuInfo> GetDonThu_BCTheoDCChuDon(DateTime startDate, DateTime endDate, int diaPhuongID, int type, int index, int start, int end, List<CoQuanInfo> coQuans, int xemTaiLieuMat, int canBoID)
        {
            var pList = new SqlParameter("@ListCoQuanID", SqlDbType.Structured);
            pList.TypeName = "dbo.IntList";
            var tbCoQuanID = new DataTable();
            tbCoQuanID.Columns.Add("CoQuanID", typeof(string));
            coQuans.ForEach(x => tbCoQuanID.Rows.Add(x.CoQuanID));

            IList<TKDonThuInfo> infoList = new List<TKDonThuInfo>();
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_STARTDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_ENDDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_DIAPHUONGID, SqlDbType.Int),
                new SqlParameter(PARM_TYPE, SqlDbType.Int),
                new SqlParameter(PARM_INDEX, SqlDbType.Int),
                pList,
                new SqlParameter(PARM_START, SqlDbType.Int),
                new SqlParameter(PARM_END, SqlDbType.Int),
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            parm[2].Value = diaPhuongID;
            parm[3].Value = type;
            parm[4].Value = index;
            parm[5].Value = tbCoQuanID;
            parm[6].Value = start;
            parm[7].Value = end;
            var query = new DataTable();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DONTHU_BC_THEO_DC_CHUDON, parm))
                {
                    query.Load(dr);
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
                info.NoiDungDon = Utils.ConvertToString(item.Field<string>("NoiDungDon"), string.Empty);
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
                //List<FileHoSoInfo> fileYKienXL = new List<FileHoSoInfo>();
                //fileYKienXL = fileYKienXLAll.Where(x => x.XuLyDonID == item.Field<int>("XuLyDonID")).ToList();
                //int step = 0;
                //for (int i = 0; i < fileYKienXL.Count; i++)
                //{
                //    if (!string.IsNullOrEmpty(fileYKienXL[i].FileURL))
                //    {
                //        if (string.IsNullOrEmpty(fileYKienXL[i].TenFile))
                //        {
                //            string[] arrtenFile = fileYKienXL[i].FileURL.Split('/');
                //            if (arrtenFile.Length > 0)
                //            {
                //                string[] duoiFile = arrtenFile[arrtenFile.Length - 1].Split('_');
                //                if (duoiFile.Length > 0)
                //                {
                //                    fileYKienXL[i].TenFile = duoiFile[duoiFile.Length - 1];
                //                }
                //                else
                //                {
                //                    fileYKienXL[i].TenFile = arrtenFile[arrtenFile.Length - 1];
                //                }
                //            }
                //        }
                //        fileYKienXL[i].FileURL = fileYKienXL[i].FileURL.Replace(" ", "%20");
                //    }
                //    step++;
                //    if (fileYKienXL[i].IsBaoMat == false)
                //    {
                //        string sec_false = "<li style='margin-bottom: 5px;'>" + step + ". " + "<a href='" + fileYKienXL[i].FileURL + "' download>" + fileYKienXL[i].TenFile + "</a></li>";
                //        info.TenHuongGiaiQuyet = "<div style='margin-bottom: 5px;'><span>" + info.TenHuongGiaiQuyet + "</span></div>" + "<ul>" + sec_false + "</ul>";
                //    }
                //    else
                //    {
                //        string sec_true = "<li style='margin-bottom: 5px;'>" + step + ". " + "<a style='color: #EE7600;' href=" + "/DownloadFileDonThuCT.aspx?url=" + fileYKienXL[i].FileURL + ">" + fileYKienXL[i].TenFile + "</a></li>";
                //        info.TenHuongGiaiQuyet = "<div style='margin-bottom: 5px;'><span>" + info.TenHuongGiaiQuyet + "</span></div>" + "<ul>" + sec_true + "</ul>";
                //    }
                //}
                //// kết quả
                //List<FileHoSoInfo> fileBanHanhQD = new List<FileHoSoInfo>();
                //fileBanHanhQD = fileBanHanhQDAll.Where(x => x.XuLyDonID == info.XuLyDonID).ToList();
                //int steps = 0;
                //for (int j = 0; j < fileBanHanhQD.Count; j++)
                //{
                //    if (!string.IsNullOrEmpty(fileBanHanhQD[j].FileURL))
                //    {
                //        if (string.IsNullOrEmpty(fileBanHanhQD[j].TenFile))
                //        {
                //            string[] arrtenFile = fileBanHanhQD[j].FileURL.Split('/');
                //            if (arrtenFile.Length > 0)
                //            {
                //                string[] duoiFile = arrtenFile[arrtenFile.Length - 1].Split('_');
                //                if (duoiFile.Length > 0)
                //                {
                //                    fileBanHanhQD[j].TenFile = duoiFile[duoiFile.Length - 1];
                //                }
                //                else
                //                {
                //                    fileBanHanhQD[j].TenFile = arrtenFile[arrtenFile.Length - 1];
                //                }
                //            }
                //        }
                //        fileBanHanhQD[j].FileURL = fileBanHanhQD[j].FileURL.Replace(" ", "%20");
                //    }
                //    steps++;
                //    if (fileBanHanhQD[j].IsBaoMat == false)
                //    {
                //        string sec_false = "<li style='margin-bottom: 5px;'>" + steps + ". " + "<a href='" + fileBanHanhQD[j].FileURL + "' download>" + fileBanHanhQD[j].TenFile + "</a></li>";
                //        info.KetQuaID_Str = "<div style='margin-bottom: 5px;'><span>" + info.KetQuaID_Str + "</span></div>" + "<ul>" + sec_false + "</ul>";
                //    }
                //    else
                //    {
                //        string sec_true = "<li style='margin-bottom: 5px;'>" + steps + ". " + "<a style='color: #EE7600;' href=" + "/DownloadFileDonThuCT.aspx?url=" + fileBanHanhQD[j].FileURL + ">" + fileBanHanhQD[j].TenFile + "</a></li>";
                //        info.KetQuaID_Str = "<div style='margin-bottom: 5px;'><span>" + info.KetQuaID_Str + "</span></div>" + "<ul>" + sec_true + "</ul>";
                //    }
                //}
                infoList.Add(info);
            }
            return infoList;
        }

        String GET_DONTHU_BC_THEO_DC_CHUDON_TONG = "BC_TheoDCChuDon_DanhSach_Tong";
        public IList<TKDonThuInfo> GetDonThu_BCTheoDCChuDon_Tong(DateTime startDate, DateTime endDate, int diaPhuongID, int type, int index, int start, int end, List<CoQuanInfo> coQuans, List<HuyenInfo> huyenList, List<XaInfo> xaList, int xemTaiLieuMat, int canBoID)
        {
            var pList = new SqlParameter("@ListCoQuanID", SqlDbType.Structured);
            pList.TypeName = "dbo.IntList";
            var tbCoQuanID = new DataTable();
            tbCoQuanID.Columns.Add("CoQuanID", typeof(string));
            coQuans.ForEach(x => tbCoQuanID.Rows.Add(x.CoQuanID));

            var pListID = new SqlParameter("@ListID", SqlDbType.Structured);
            pListID.TypeName = "dbo.IntList";
            var tb = new DataTable();
            if (type == 2)//tinh
            {
                tb.Columns.Add("HuyenID", typeof(string));
                huyenList.ForEach(x => tb.Rows.Add(x.HuyenID));
            }
            else if (type == 3)//huyen
            {
                tb.Columns.Add("XaID", typeof(string));
                xaList.ForEach(x => tb.Rows.Add(x.XaID));
            }

            IList<TKDonThuInfo> infoList = new List<TKDonThuInfo>();
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_STARTDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_ENDDATE, SqlDbType.DateTime),
                pListID,
                new SqlParameter(PARM_TYPE, SqlDbType.Int),
                new SqlParameter(PARM_INDEX, SqlDbType.Int),
                pList,
                new SqlParameter(PARM_START, SqlDbType.Int),
                new SqlParameter(PARM_END, SqlDbType.Int),
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            parm[2].Value = tb;
            parm[3].Value = type;
            parm[4].Value = index;
            parm[5].Value = tbCoQuanID;
            parm[6].Value = start;
            parm[7].Value = end;
            var query = new DataTable();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DONTHU_BC_THEO_DC_CHUDON_TONG, parm))
                {
                    query.Load(dr);
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
                info.NoiDungDon = Utils.ConvertToString(item.Field<string>("NoiDungDon"), string.Empty);
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
            return infoList;
        }

        public IList<TKDonThuInfo> GetDonThu_BCTheoDCChuDon_Excel(ref DataTable dt, DateTime startDate, DateTime endDate, int diaPhuongID, int type, int index, int start, int end, List<CoQuanInfo> coQuans, int xemTaiLieuMat, int canBoID)
        {
            var pList = new SqlParameter("@ListCoQuanID", SqlDbType.Structured);
            pList.TypeName = "dbo.IntList";
            var tbCoQuanID = new DataTable();
            tbCoQuanID.Columns.Add("CoQuanID", typeof(string));
            coQuans.ForEach(x => tbCoQuanID.Rows.Add(x.CoQuanID));

            IList<TKDonThuInfo> infoList = new List<TKDonThuInfo>();
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_STARTDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_ENDDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_DIAPHUONGID, SqlDbType.Int),
                new SqlParameter(PARM_TYPE, SqlDbType.Int),
                new SqlParameter(PARM_INDEX, SqlDbType.Int),
                pList,
                new SqlParameter(PARM_START, SqlDbType.Int),
                new SqlParameter(PARM_END, SqlDbType.Int),
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            parm[2].Value = diaPhuongID;
            parm[3].Value = type;
            parm[4].Value = index;
            parm[5].Value = tbCoQuanID;
            parm[6].Value = start;
            parm[7].Value = end;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DONTHU_BC_THEO_DC_CHUDON, parm))
                {
                    dt.Load(dr);
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return infoList;
        }

        public IList<TKDonThuInfo> GetDonThu_BCTheoDCChuDon_Tong_Excel(ref DataTable dt, DateTime startDate, DateTime endDate, int diaPhuongID, int type, int index, int start, int end, List<CoQuanInfo> coQuans, List<HuyenInfo> huyenList, List<XaInfo> xaList, int xemTaiLieuMat, int canBoID)
        {
            var pList = new SqlParameter("@ListCoQuanID", SqlDbType.Structured);
            pList.TypeName = "dbo.IntList";
            var tbCoQuanID = new DataTable();
            tbCoQuanID.Columns.Add("CoQuanID", typeof(string));
            coQuans.ForEach(x => tbCoQuanID.Rows.Add(x.CoQuanID));

            var pListID = new SqlParameter("@ListID", SqlDbType.Structured);
            pListID.TypeName = "dbo.IntList";
            var tb = new DataTable();
            if (type == 2)//tinh
            {
                tb.Columns.Add("HuyenID", typeof(string));
                huyenList.ForEach(x => tb.Rows.Add(x.HuyenID));
            }
            else if (type == 3)//huyen
            {
                tb.Columns.Add("XaID", typeof(string));
                xaList.ForEach(x => tb.Rows.Add(x.XaID));
            }

            IList<TKDonThuInfo> infoList = new List<TKDonThuInfo>();
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_STARTDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_ENDDATE, SqlDbType.DateTime),
                pListID,
                new SqlParameter(PARM_TYPE, SqlDbType.Int),
                new SqlParameter(PARM_INDEX, SqlDbType.Int),
                pList,
                new SqlParameter(PARM_START, SqlDbType.Int),
                new SqlParameter(PARM_END, SqlDbType.Int),
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            parm[2].Value = tb;
            parm[3].Value = type;
            parm[4].Value = index;
            parm[5].Value = tbCoQuanID;
            parm[6].Value = start;
            parm[7].Value = end;

            try
            {
                //var st1 = new Stopwatch();
                //st1.Start();
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DONTHU_BC_THEO_DC_CHUDON_TONG, parm))
                {
                    //st1.Stop();
                    //var t1 = st1.Elapsed;

                    //var st2 = new Stopwatch();
                    //st2.Start();
                    dt.Load(dr);
                    //st2.Stop();
                    //var t2 = st2.Elapsed;
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return infoList;
        }

    }
}
