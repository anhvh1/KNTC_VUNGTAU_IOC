using Com.Gosol.KNTC.Models.BaoCao;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Ultilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTable = System.Data.DataTable;

namespace Com.Gosol.KNTC.DAL.KNTC
{
    public class DonThuDAL
    {
        //Su dung de goi StoreProcedure

        private const string GET_ALL = @"DonThu_GetAll";
        private const string GET_BY_ID = @"NV_DonThu_GetByID";
        private const string GETDONTHU_BY_DONTHUID = @"NV_DonThu_Get_DonThu_By_DonThuID";
        private const string INSERT = @"NV_DonThu_Insert";
        private const string INSERT_STEP1 = @"NV_DonThu_InsertStep1";
        private const string UPDATE = @"NV_DonThu_Update";
        private const string UPDATE_STEP1 = @"NV_DonThu_UpdateStep1";
        private const string UPDATE_STEP2 = @"NV_DonThu_UpdateStep2";
        private const string UPDATE_STEP3 = @"NV_DonThu_UpdateStep3";
        private const string UPDATE_STEP4 = @"NV_DonThu_UpdateStep4";
        private const string UPDATE_TRUNGDON = @"NV_DonThu_UpdateTrungDon";
        private const string DELETE = @"NV_DonThu_Delete";
        private const string GET_CUSTOMDATA = @"NV_DonThu_GetCustomData";

        private const string GET_DOCUMENT_BYID = @"Document_GetByID";

        private const string NV_DonThu_ChuaDongBo_GetBySearch = "NV_DonThu_ChuaDongBo_GetBySearch";
        private const string NV_DonThu_DaDongBo_GetBySearch = "NV_DonThu_DaDongBo_GetBySearch";

        private const string GET_DATA_HOSODONTHU = @"NV_DonThu_GetDataForHoSoDonThu";
        private const string GET_COUNT_HOSODONTHU = @"NV_DonThu_CountSearchForHoSoDonThu";

        //private const string GET_BY_PAGE = @"DM_DonThu_GetByPage";
        //private const string COUNT_ALL = @"DM_DonThu_CountAll";
        //private const string COUNT_SEARCH = @"DM_DonThu_CountSearch";
        //private const string SEARCH = @"DM_DonThu_GetBySearch";

        //check trung don
        private const string CHECK_TRUNG_DON = @"NV_DonThu_CheckTrungDon";

        //private const string GET_BY_PAGE = @"NV_DonThu_GetByPage";
        //private const string COUNT_ALL = @"NV_DonThu_CountAll";

        //quanghv: stored
        private const string GET_SO_DON_BY_CO_QUAN = @"NV_DonThu_GetSoDonByCoQuan";
        private const string GET_DONTHU_XULYLAN1 = "NV_DonThu_GetXuLyDonLan1";
        private const string GET_NHANVIEN_XULYLAN1 = @"NV_XuLyDon_GetXuLyLan1";
        private const string GET_DONTHU_GOC = @"NV_XuLyDon_GetDonThuGoc";

        private const string GET_BY_PAGE = @"NV_DonThu_GetByPage_v2";
        private const string ORDER_BY_LOAI_DON = @"NV_DonThu_OrderByLoaiDon";
        private const string ORDER_BY_TEN_CHU_DON = @"NV_DonThu_OrderByTenChuDon";
        private const string ORDER_BY_NGAY_NHAP_DON = @"NV_DonThu_OrderByNgayNhapDon";
        private const string ORDER_BY_TEN__CO_QUAN = @"NV_DonThu_OrderByTenCoQuan";
        //private const string GET_CMND_BY_DONTHU_ID = @"NV_DonThu_GetCMNDByDonThuID";
        private const string COUNT_ALL = @"NV_DonThu_CountAll";
        private const string SEARCH = @"NV_DonThu_GetBySearch";
        private const string GETDONTHU_TRUNGDON = @"NV_DonThu_GetDSDonThuTrung";
        private const string COUNT_DT_BY_DONTHUID = @"NV_DonThu_CountDTTrungByDonThuID";
        private const string GETDATA_TIEPNHAN_GIANTIEP = @"NV_DonThu_GetData_TiepNhanGianTiep";
        private const string COUNT_SEARCH = @"NV_DonThu_CountSearch";
        private const string COUNT_DATA_TIEPNHANGIANTIEP = @"NV_DontThu_CountData_TiepNhanGianTiep";
        private const string COUNT_DTYEUCAUDOITHOAI = @"NV_DonThu_CountYeuCauDoiThoai";
        private const string GET_DTYEUCAU_DOITHOAI = @"NV_DonThu_GetYeuCauDoiThoai";
        private const string GET_LSDONTHU_TIEPNHANBYDONTHUID = @"NV_DonThu_GetLsDonThuTiepNhan_ByDonThuID_CoQuanID";

        #region -- param
        private const string PARAM_DON_THU_ID = "@DonThuID";
        private const string PARAM_DOCUMENTID = @"DocumentID";
        //private const string PARAM_SO_DON_THU = "@SoDonThu";
        private const string PARAM_NGAY_NHAP_DON = "@NgayNhapDon";
        private const string PARAM_NGAY_QUA_HAN = "@NgayQuaHan";
        //private const string PARAM_NGUON_DON_DEN_ID = "@NguonDonDenID";
        //private const string PARAM_CQ_CHUYEN_DON_ID = "@CQChuyenDonID";
        //private const string PARAM_SO_CONG_VAN = "@SoCongVan";
        //private const string PARAM_NGAY_CHUYEN_DON = "@NgayChuyenDon";
        private const string PARAM_NHOM_KN_ID = "@NhomKNID";
        private const string PARAM_DOI_TUONG_BI_KN_ID = "@DoiTuongBiKNID";
        private const string PARAM_LOAI_KHIEU_TO_1ID = "@LoaiKhieuTo1ID";
        private const string PARAM_LOAI_KHIEU_TO_2ID = "@LoaiKhieuTo2ID";
        private const string PARAM_LOAI_KHIEU_TO_3ID = "@LoaiKhieuTo3ID";
        private const string PARAM_LOAI_KHIEU_TO_ID = "@LoaiKhieuToID";
        private const string PARAM_NGAY_VIET_DON = "@NgayVietDon";
        //private const string PARAM_GAP_LANH_DAO = "@GapLanhDao";
        //private const string PARAM_NGAY_GAP_LANH_DAO = "@NgayGapLanhDao";
        private const string PARAM_NOI_DUNG_DON = "@NoiDungDon";
        //private const string PARAM_CQ_DA_GIAI_QUYET_ID = "@CQDaGiaiQuyetID";
        //private const string PARAM_HUONG_GIAI_QUYET_ID = "@HuongGiaiQuyetID";
        //private const string PARAM_NOI_DUNG_HUONG_DAN = "@NoiDungHuongDan";
        //private const string PARAM_CAN_BO_XU_LY_ID = "@CanBoXuLyID";
        //private const string PARAM_CQ_TIEP_NHAN_ID = "@CoQuanTiepNhanID";
        //private const string PARAM_CAN_BO_KY_ID = "@CanBoKyID";
        //private const string PARAM_CQ_GIAI_QUYET_TIEP_ID = "@CQGiaiQuyetTiepID";

        //private const string PARAM_PT_KET_QUA_ID = "@PhanTichKQID";
        private const string PARAM_TRUNG_DON = "@TrungDon";
        //05/06/2014
        private const string PARAM_TINHID = "@TinhID";
        private const string PARAM_HUYENID = "@HuyenID";
        private const string PARAM_XAID = "@XaID";

        //param xulydonthu
        private const string PARAM_TRANG_THAI_DON_ID = "@TrangThaiDonID";
        private const string PARAM_CO_QUAN_ID = "@CoQuanID";
        private const string PARAM_TUNGAY = "@TuNgay";
        private const string PARAM_DENNGAY = "@DenNgay";
        //param getby cmnd
        private const string PARAM_CMND = "@CMND";
        private const string PARAM_HOTEN = "@HoTen";
        private const string PARAM_DIACHI = "@DiaChi";
        private const string PARAM__LOAI_KHIEU_TO_ID = "@LoaiKhieuToID";

        private const string PARAM_KEY = "@txt_search";
        private const string PARAM_KEYWORD = "@keyword";
        private const string PARAM_START = "@Start";
        private const string PARAM_END = "@End";
        private const string PARAM_DDl_SEARCH = "@ddl_search";
        private const string PARAM_PAGE = "@Page";
        private const string PARAM_LIMIT = "@Limit";
        private const string PARAM_STARTS = "@start";
        private const string PARAM_ENDS = "@end";

        //param delete DonThu by XuLyDonID
        private const string PARAM_XU_LY_DON_ID = "@XuLyDonID";

        private const string PARAM_LISTID = "@ListID";
        #endregion

        #region -- get data
        private DonThuInfo GetData(SqlDataReader dr)
        {
            DonThuInfo info = new DonThuInfo();
            info.DonThuID = Utils.GetInt32(dr["DonThuID"], 0);

            info.NhomKNID = Utils.GetInt32(dr["NhomKNID"], 0);
            info.DoiTuongBiKNID = Utils.GetInt32(dr["DoiTuongBiKNID"], 0);
            info.LoaiKhieuTo1ID = Utils.GetInt32(dr["LoaiKhieuTo1ID"], 0);
            info.LoaiKhieuTo2ID = Utils.GetInt32(dr["LoaiKhieuTo2ID"], 0);
            info.LoaiKhieuTo3ID = Utils.GetInt32(dr["LoaiKhieuTo3ID"], 0);
            info.LoaiKhieuToID = Utils.GetInt32(dr["LoaiKhieuToID"], 0);
            info.NoiDungDon = Utils.GetString(dr["NoiDungDon"], string.Empty);

            info.TinhID = Utils.ConvertToNullableInt32(dr["TinhID"], null);
            info.HuyenID = Utils.ConvertToNullableInt32(dr["HuyenID"], null);
            info.XaID = Utils.ConvertToNullableInt32(dr["XaID"], null);
            info.NgayVietDon = Utils.GetDateTime(dr["NgayVietDon"], Constant.DEFAULT_DATE);

            info.SoLan = Utils.ConvertToInt32(dr["SoLan"], 1);

            info.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
            info.TrangThaiDonID = Utils.ConvertToInt32(dr["TrangThaiDonID"], 0);
            info.LyDo = Utils.GetString(dr["LyDo"], String.Empty);

            info.TenLoaiKhieuTo1 = Utils.ConvertToString(dr["TenLoaiKhieuTo1"], String.Empty);
            info.TenLoaiKhieuTo2 = Utils.ConvertToString(dr["TenLoaiKhieuTo2"], String.Empty);
            info.TenLoaiKhieuTo3 = Utils.ConvertToString(dr["TenLoaiKhieuTo3"], String.Empty);

            info.NoiDungHuongDan = Utils.ConvertToString(dr["NoiDungHuongDan"], string.Empty);
            info.TenHuongGiaiQuyet = Utils.ConvertToString(dr["TenHuongGiaiQuyet"], string.Empty);

            return info;
        }
        #endregion

        #region -- get custom data
        private DonThuInfo GetCustomData(SqlDataReader dr)
        {
            DonThuInfo info = new DonThuInfo();
            info.DonThuID = Utils.ConvertToInt32(dr["DonThuID"], 0);
            info.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);

            return info;
        }
        #endregion

        #region -- get don thu goc data
        private DonThuInfo GetDonThuGocData(SqlDataReader dr)
        {
            DonThuInfo info = new DonThuInfo();
            info.DonThuGocID = Utils.ConvertToInt32(dr["DonThuGocID"], 0);

            return info;
        }
        #endregion

        #region -- get custom documentdata
        private DocumentInfo GetCustomDocumentData(SqlDataReader dr)
        {
            DocumentInfo info = new DocumentInfo();
            info.DueDate = Utils.ConvertToDateTime(dr["DueDate"], DateTime.MinValue);

            return info;
        }
        #endregion

        #region -- set,Get Insert Parmas
        private SqlParameter[] GetInsertParms(string step)
        {
            SqlParameter[] parms = new SqlParameter[] { };

            switch (step)
            {
                case "step1":
                    parms = new SqlParameter[]{
                        new SqlParameter(PARAM_NHOM_KN_ID, SqlDbType.Int)
                    };
                    break;
                default:
                    parms = new SqlParameter[]{
                        new SqlParameter(PARAM_NHOM_KN_ID, SqlDbType.Int),
                        new SqlParameter(PARAM_DOI_TUONG_BI_KN_ID, SqlDbType.Int),
                        new SqlParameter(PARAM_LOAI_KHIEU_TO_1ID, SqlDbType.Int),
                        new SqlParameter(PARAM_LOAI_KHIEU_TO_2ID, SqlDbType.Int),
                        new SqlParameter(PARAM_LOAI_KHIEU_TO_3ID, SqlDbType.Int),
                        new SqlParameter(PARAM_LOAI_KHIEU_TO_ID, SqlDbType.Int),
                        new SqlParameter(PARAM_NOI_DUNG_DON, SqlDbType.NChar,-1),
                        new SqlParameter(PARAM_TINHID, SqlDbType.Int),
                        new SqlParameter(PARAM_HUYENID, SqlDbType.Int),
                        new SqlParameter(PARAM_XAID, SqlDbType.Int),
                        new SqlParameter(PARAM_NGAY_VIET_DON,SqlDbType.DateTime),
                        new SqlParameter(PARAM_DIACHI, SqlDbType.NVarChar),
                    };
                    break;
            }
            return parms;
        }

        //SET iNSERT PARMS
        private void SetInsertParms(SqlParameter[] parms, DonThuInfo DTInfo, string step)
        {
            switch (step)
            {
                case "step1":
                    parms[0].Value = DTInfo.NhomKNID;
                    break;
                default:
                    parms[0].Value = DTInfo.NhomKNID;
                    parms[1].Value = DTInfo.DoiTuongBiKNID;
                    parms[2].Value = DTInfo.LoaiKhieuTo1ID;
                    parms[3].Value = DTInfo.LoaiKhieuTo2ID;
                    parms[4].Value = DTInfo.LoaiKhieuTo3ID;
                    parms[5].Value = DTInfo.LoaiKhieuToID;
                    parms[6].Value = DTInfo.NoiDungDon ?? Convert.DBNull;
                    parms[7].Value = DTInfo.TinhID;
                    parms[8].Value = DTInfo.HuyenID;
                    parms[9].Value = DTInfo.XaID;
                    parms[10].Value = DTInfo.NgayVietDon;
                    parms[11].Value = DTInfo.DiaChiPhatSinh ?? Convert.DBNull;
                    break;
            }
        }
        #endregion

        #region -- set,get update parms
        private SqlParameter[] GetUpdateParms(string step)
        {
            SqlParameter[] parms = new SqlParameter[] { };

            switch (step)
            {
                case "step1":
                    parms = new SqlParameter[]{
                        new SqlParameter(PARAM_DON_THU_ID, SqlDbType.Int),
                        new SqlParameter(PARAM_NHOM_KN_ID, SqlDbType.Int)
                    };
                    break;
                default:
                    parms = new SqlParameter[]{
                        new SqlParameter(PARAM_DON_THU_ID, SqlDbType.Int),
                        new SqlParameter(PARAM_NHOM_KN_ID, SqlDbType.Int),
                        new SqlParameter(PARAM_DOI_TUONG_BI_KN_ID, SqlDbType.Int),
                        new SqlParameter(PARAM_LOAI_KHIEU_TO_1ID, SqlDbType.Int),
                        new SqlParameter(PARAM_LOAI_KHIEU_TO_2ID, SqlDbType.Int),
                        new SqlParameter(PARAM_LOAI_KHIEU_TO_3ID, SqlDbType.Int),
                        new SqlParameter(PARAM_LOAI_KHIEU_TO_ID, SqlDbType.Int),
                        new SqlParameter(PARAM_NOI_DUNG_DON, SqlDbType.NChar,-1),
                        new SqlParameter(PARAM_TINHID, SqlDbType.Int),
                        new SqlParameter(PARAM_HUYENID, SqlDbType.Int),
                        new SqlParameter(PARAM_XAID, SqlDbType.Int),
                        new SqlParameter(PARAM_NGAY_VIET_DON,SqlDbType.DateTime),
                        new SqlParameter(PARAM_DIACHI, SqlDbType.NVarChar),
                    };
                    break;
            }

            return parms;
        }

        //set update parms
        private void SetUpdateParms(SqlParameter[] parms, DonThuInfo DTInfo, string step)
        {
            switch (step)
            {
                case "step1":
                    parms[0].Value = DTInfo.DonThuID;
                    parms[1].Value = DTInfo.NhomKNID;
                    break;
                default:
                    parms[0].Value = DTInfo.DonThuID;
                    parms[1].Value = DTInfo.NhomKNID;
                    parms[2].Value = DTInfo.DoiTuongBiKNID;
                    parms[3].Value = DTInfo.LoaiKhieuTo1ID;
                    parms[4].Value = DTInfo.LoaiKhieuTo2ID;
                    parms[5].Value = DTInfo.LoaiKhieuTo3ID;
                    parms[6].Value = DTInfo.LoaiKhieuToID;
                    parms[7].Value = DTInfo.NoiDungDon ?? Convert.DBNull;
                    parms[8].Value = DTInfo.TinhID;
                    parms[9].Value = DTInfo.HuyenID;
                    parms[10].Value = DTInfo.XaID;
                    parms[11].Value = DTInfo.NgayVietDon;
                    parms[12].Value = DTInfo.DiaChiPhatSinh ?? Convert.DBNull;
                    break;
            }
        }
        #endregion

        #region -- get all don thu
        public IList<DonThuInfo> GetAll()
        {
            IList<DonThuInfo> ListDT = new List<DonThuInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_ALL, null))
                {

                    while (dr.Read())
                    {

                        DonThuInfo DTInfo = GetData(dr);
                        ListDT.Add(DTInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return ListDT;
        }
        #endregion

        #region -- get don thu by donthuid
        public DonThuInfo GetByID(int donthuID)
        {
            DonThuInfo DTInfo = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_DON_THU_ID,SqlDbType.Int)
            };
            parameters[0].Value = donthuID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GETDONTHU_BY_DONTHUID, parameters))
                {

                    if (dr.Read())
                    {
                        DTInfo = GetData(dr);
                        DTInfo.DiaChiPhatSinh = Utils.ConvertToString(dr["DiaChiPhatSinh"], String.Empty);
                        DTInfo.CQDaGiaiQuyetID = Utils.ConvertToString(dr["CQDaGiaiQuyetID"], String.Empty);
                        DTInfo.TenCQDaGiaiQuyet = Utils.ConvertToString(dr["TenCoQuanDaGQ"], String.Empty);
                        DTInfo.NgayNhapDon = Utils.ConvertToDateTime(dr["NgayNhapDon"], DateTime.Now);
                        DTInfo.SoDonThu = Utils.ConvertToString(dr["SoDonThu"], String.Empty);
                        DTInfo.NgayTiep = DTInfo.NgayNhapDon;
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return DTInfo;
        }
        #endregion

        #region -- get don thu by documentid
        public DocumentInfo GetDocumentByID(int documentID)
        {

            DocumentInfo dCInfo = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_DOCUMENTID,SqlDbType.Int)
            };
            parameters[0].Value = documentID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DOCUMENT_BYID, parameters))
                {
                    if (dr.Read())
                    {
                        dCInfo = GetCustomDocumentData(dr);
                        dCInfo.DueDateStr = Format.FormatDate(dCInfo.DueDate);
                        dCInfo.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return dCInfo;
        }
        #endregion

        #region -- get don thu by donthuid, xulydonid
        public DonThuInfo GetByID(int donthuID, int xulydonID)
        {

            DonThuInfo DTInfo = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_DON_THU_ID,SqlDbType.Int),
                new SqlParameter(PARAM_XU_LY_DON_ID, SqlDbType.Int)
            };
            parameters[0].Value = donthuID;
            parameters[1].Value = xulydonID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_ID, parameters))
                {

                    if (dr.Read())
                    {
                        DTInfo = GetData(dr);
                        DTInfo.NguonDonDen = Utils.ConvertToInt32(dr["NguonDonDen"], 0);
                        // bổ sung tên cơ quan chuyển đến 1/7/2024
                        DTInfo.TenNguonDonDen = Utils.GetString(dr["TenCQChuyenDonDen"], string.Empty);
                        DTInfo.TenCQChuyenDonDen = Utils.GetString(dr["TenCQChuyenDonDen"], string.Empty);
                        DTInfo.TenCanBoTiepNhan = Utils.GetString(dr["TenCanBoTiepNhan"], string.Empty);
                        DTInfo.TenCoQuanTiepNhan = Utils.GetString(dr["TenCoQuanTiepNhan"], string.Empty);
                        DTInfo.TenPhongBanTiepNhan = Utils.GetString(dr["TenPhongBanTiepNhan"], string.Empty);
                        DTInfo.CoQuanChuyenDonDi = Utils.GetString(dr["CoQuanChuyenDonDi"], string.Empty);
                        DTInfo.NgayChuyenDonSangCQKhac = Utils.ConvertToDateTime(dr["NgayChuyenDonDi"], DateTime.MinValue);
                        DTInfo.CQDaGiaiQuyetID = Utils.GetString(dr["CQDaGiaiQuyetID"], string.Empty);
                        var TenCanBoXl = Utils.GetString(dr["TenCanBoXuLy"], string.Empty);
                        if (TenCanBoXl != "")
                        {
                            DTInfo.TenCanBoXuLy = Utils.GetString(dr["TenCanBoXuLy"], string.Empty);
                        }
                        else
                        {
                            DTInfo.TenCanBoXuLy = Utils.GetString(dr["TenCanBoPhanXL"], string.Empty);
                        }


                        DTInfo.TenPhongBanXuLy = Utils.GetString(dr["TenPhongBanXuLy"], string.Empty);
                        //DTInfo.NgayQuaHanGQ = Utils.ConvertToDateTime(dr["HanGiaiQuyet"], DateTime.MinValue);
                        DTInfo.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        if (DTInfo.StateID == (int)EnumState.LDPhanGQ)
                        {
                            DTInfo.NgayQuaHanGQ = Utils.ConvertToDateTime(dr["HanGiaiQuyet"], DateTime.MinValue);
                        }
                        else
                        {
                            DTInfo.NgayQuaHanGQ = Utils.ConvertToDateTime(dr["HanGiaiQuyetDueDate"], DateTime.MinValue);

                        }

                        DTInfo.NgayNhapDon = Utils.ConvertToDateTime(dr["NgayNhapDon"], DateTime.MinValue);

                        DTInfo.TenLoaiKetQua = Utils.GetString(dr["TenLoaiKetQua"], string.Empty);
                        DTInfo.NgayXuLy = Utils.ConvertToDateTime(dr["NgayXuLy"], DateTime.MinValue);
                        DTInfo.TenCoQuanGQ = Utils.GetString(dr["TenCoQuanGQ"], string.Empty);
                        DTInfo.TenCoQuanXL = Utils.GetString(dr["TenCoQuanXuLy"], string.Empty);
                        DTInfo.NgayChuyenDon = Utils.ConvertToDateTime(dr["NgayPhanCong"], DateTime.MinValue);
                        DTInfo.HanXuLy = Utils.ConvertToDateTime(dr["HanXuLy"], DateTime.MinValue);
                        DTInfo.SoDonThu = Utils.GetString(dr["SoDonThu"], string.Empty);
                        DTInfo.NgayPhan = Utils.ConvertToDateTime(dr["NgayPhan"], DateTime.MinValue);
                        DTInfo.SoCongVan = Utils.GetString(dr["SoCongVan"], string.Empty);
                        DTInfo.SoCVBaoCaoGQ = Utils.GetString(dr["SoCVBaoCaoGQ"], string.Empty);
                        DTInfo.NgayCVBaoCaoGQ = Utils.ConvertToDateTime(dr["NgayCVBaoCaoGQ"], DateTime.MinValue);
                        DTInfo.TenCQPhan = Utils.GetString(dr["TenCQPhan"], string.Empty);
                        DTInfo.LanGiaiQuyet = Utils.ConvertToInt32(dr["LanGiaiQuyet"], 0);
                        DTInfo.TenTinh = Utils.ConvertToString(dr["TenTinh"], string.Empty);
                        DTInfo.TenHuyen = Utils.ConvertToString(dr["TenHuyen"], string.Empty);
                        DTInfo.TenXa = Utils.ConvertToString(dr["TenXa"], string.Empty);
                        DTInfo.DiaChiPhatSinh = Utils.ConvertToString(dr["DiaChiPhatSinh"], string.Empty);
                        DTInfo.HuongGiaiQuyetID = Utils.ConvertToInt32(dr["HuongGiaiQuyetID"], 0);
                        DTInfo.CapID = Utils.ConvertToInt32(dr["CapID"], 0);
                        DTInfo.HanGiaiQuyetCu = Utils.ConvertToDateTime(dr["HanGiaiQuyetCu"], DateTime.MinValue);
                        DTInfo.HanGiaiQuyetMoi = Utils.ConvertToDateTime(dr["HanGiaiQuyetMoi"], DateTime.MinValue);
                        DTInfo.HanGiaiQuyetFrist = Utils.ConvertToDateTime(dr["HanGiaiQuyetFrist"], DateTime.MinValue);
                        DTInfo.NgayXuLyDon = Utils.ConvertToDateTime(dr["NgayXuLyDon"], DateTime.MinValue);
                        DTInfo.HanGiaiQuyetCu_Str = Format.FormatDate(DTInfo.HanGiaiQuyetCu);
                        DTInfo.HanGiaiQuyetMoi_Str = Format.FormatDate(DTInfo.HanGiaiQuyetMoi);
                        DTInfo.HanGiaiQuyetFrist_Str = Format.FormatDate(DTInfo.HanGiaiQuyetFrist);
                        DTInfo.NgayXuLyDon_Str = Format.FormatDate(DTInfo.NgayXuLyDon);

                        DTInfo.NgayGiao = Utils.ConvertToDateTime(dr["NgayChuyen"], DateTime.MinValue);
                        DTInfo.NgayGiao_Str = Format.FormatDate(DTInfo.NgayGiao);
                        List<CoQuanInfo> cqPhoiHop = GetCoQuanPhoiHopByXuLyDonID(DTInfo.XuLyDonID);

                        DTInfo.TenCoQuanPhoiHop_Str = string.Join(", ", cqPhoiHop.Select(x => x.TenCoQuan.ToString()).ToArray());

                        DTInfo.TrinhDuThao = Utils.ConvertToInt32(dr["TrinhDuThao"], 0);
                        DTInfo.NgayBanHanh = Utils.ConvertToNullableDateTime(dr["NgayBanHanh"], null);
                        DTInfo.TenCanBoBanHanh = Utils.GetString(dr["TenCanBoBanHanh"], string.Empty);
                        DTInfo.TenCoQuanBanHanh = Utils.GetString(dr["TenCoQuanBanHanhXM"], string.Empty);
                        if (DTInfo.TenCoQuanTiepNhan == null || DTInfo.TenCoQuanTiepNhan == "") DTInfo.TenCoQuanTiepNhan = DTInfo.TenCoQuanXL;

                        DTInfo.NgayTiep = Utils.ConvertToDateTime(dr["NgayTiep"], DateTime.MinValue);
                        if (DTInfo.NgayTiep == DateTime.MinValue) DTInfo.NgayTiep = DTInfo.NgayNhapDon;

                        DTInfo.LanhDaoDuyet1ID = Utils.ConvertToInt32(dr["LanhDaoDuyet1ID"], 0);
                        DTInfo.LanhDaoDuyet2ID = Utils.ConvertToInt32(dr["LanhDaoDuyet2ID"], 0);

                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return DTInfo;
        }

        public List<CoQuanInfo> GetCoQuanPhoiHopByXuLyDonID(int xulydonID)
        {
            List<CoQuanInfo> cq = new List<CoQuanInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XU_LY_DON_ID, SqlDbType.Int)
            };
            parameters[0].Value = xulydonID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "CoQuanPhoiHopGQ_GetByXuLyDonID", parameters))
                {

                    while (dr.Read())
                    {
                        CoQuanInfo item = new CoQuanInfo();
                        item.TenCoQuan = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        cq.Add(item);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return cq;
        }
        #endregion

        #region -- get don thu xuly lan 1
        public DonThuInfo GetXuLyDonLan1(int donthuID)
        {

            DonThuInfo DTInfo = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_DON_THU_ID,SqlDbType.Int),
            };
            parameters[0].Value = donthuID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_NHANVIEN_XULYLAN1, parameters))
                {

                    if (dr.Read())
                    {
                        DTInfo = GetCustomData(dr);
                        DTInfo.NgayNhapDon = Utils.ConvertToDateTime(dr["NgayNhapDon"], DateTime.MinValue);
                        DTInfo.TenCoQuanTiepNhan = Utils.GetString(dr["TenCoQuanTiepNhan"], string.Empty);
                        DTInfo.NgayQuaHanGQ = Utils.ConvertToDateTime(dr["HanGiaiQuyet"], DateTime.MinValue);
                        DTInfo.KetQuaGiaiQuyet = Utils.ConvertToString(dr["KetQuaGiaiQuyet"], string.Empty);
                        DTInfo.TenCoQuanGQ = Utils.ConvertToString(dr["TenCoQuanGQ"], string.Empty);
                        DTInfo.HuongXuLy = Utils.GetString(dr["HuongXuLy"], string.Empty);
                        DTInfo.SoLan = Utils.ConvertToInt32(dr["SoLan"], 0);
                        DTInfo.YKienXuLy = Utils.GetString(dr["YKienXuLy"], string.Empty);
                        DTInfo.YKienGiaiQuyet = Utils.GetString(dr["YKienGiaiQuyet"], string.Empty);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return DTInfo;
        }
        #endregion

        #region -- get don thu goc
        public DonThuInfo GetDonThuGoc(int donthuID)
        {

            DonThuInfo DTInfo = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_DON_THU_ID,SqlDbType.Int),
            };
            parameters[0].Value = donthuID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DONTHU_GOC, parameters))
                {

                    if (dr.Read())
                    {
                        DTInfo = GetDonThuGocData(dr);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return DTInfo;
        }
        #endregion

        #region -- get custom data
        public DonThuInfo GetCustomData(int donthuID)
        {

            DonThuInfo DTInfo = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_DON_THU_ID,SqlDbType.Int)
            };
            parameters[0].Value = donthuID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_ID, parameters))
                {

                    if (dr.Read())
                    {
                        DTInfo = GetCustomData(dr);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return DTInfo;
        }
        #endregion

        #region -- get xu ly don by donthuid
        public DonThuInfo GetXuLyDonID(int donthuID)
        {

            DonThuInfo DTInfo = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_DON_THU_ID,SqlDbType.Int)
            };
            parameters[0].Value = donthuID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_CUSTOMDATA, parameters))
                {

                    if (dr.Read())
                    {
                        DTInfo = GetCustomData(dr);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return DTInfo;
        }
        public DonThuInfo GetByXuLyDonID(int? XuLyDonID)
        {

            DonThuInfo DTInfo = new DonThuInfo();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@XuLyDonID",SqlDbType.Int)
            };
            parameters[0].Value = XuLyDonID ?? Convert.DBNull;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "DonThu_GetByXuLyDonID", parameters))
                {

                    if (dr.Read())
                    {
                        //DonThuInfo info = new DonThuInfo();
                        //DTInfo.DonThuID = Utils.ConvertToInt32(dr["DonThuID"], 0);
                        //DTInfo.XuLyDonID = Utils.ConvertToInt32(dr["DocumentID"], 0);
                        DTInfo.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        DTInfo.PreState = Utils.ConvertToInt32(dr["PreState"], 0);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return DTInfo;
        }
        #endregion

        #region -----------delete----------------
        public int Delete(int DT_ID)
        {

            int val = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_DON_THU_ID,SqlDbType.Int)
            };
            parameters[0].Value = DT_ID;
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
        #endregion

        #region ------------UPDATE---------------------
        public int Update(string step, DonThuInfo DTInfo)
        {

            object val = null;
            SqlParameter[] parameters = GetUpdateParms(step);
            SetUpdateParms(parameters, DTInfo, step);

            string stored = UPDATE_STEP1;

            switch (step)
            {
                case "step1":
                    stored = UPDATE_STEP1;
                    break;
                case "trungdon":
                    stored = UPDATE_TRUNGDON;
                    break;
                default:
                    stored = UPDATE;
                    break;
            }

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, stored, parameters);
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
            return Utils.ConvertToInt32(val, 0); // tra ve so don thu
        }
        #endregion

        #region --INSERT
        public int Insert(string step, DonThuInfo DTInfo)
        {
            object insertID;

            SqlParameter[] parameters = GetInsertParms(step);
            SetInsertParms(parameters, DTInfo, step);

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        if (step == "step1")
                            insertID = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, INSERT_STEP1, parameters);
                        else
                            insertID = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, INSERT, parameters);
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
            return Utils.ConvertToInt32(insertID, 0);
        }
        #endregion

        #region -- check trung don by cmnd
        public IList<DonThuInfo> GetByCMND(int page, string cmnd)
        {
            int start = (page - 1) * Constant.PageSize;
            int end = page * Constant.PageSize;

            IList<DonThuInfo> ls_donthu = new List<DonThuInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_CMND,SqlDbType.NVarChar),
                new SqlParameter(PARAM_START,SqlDbType.Int),
                new SqlParameter(PARAM_END,SqlDbType.Int)
            };

            parameters[0].Value = cmnd;
            parameters[1].Value = start;
            parameters[2].Value = end;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, CHECK_TRUNG_DON, parameters))
                {
                    while (dr.Read())
                    {
                        DonThuInfo DTInfo = GetDataByCMND(dr);
                        ls_donthu.Add(DTInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return ls_donthu;
        }
        private DonThuInfo GetDataByCMND(SqlDataReader dr)
        {
            DonThuInfo info = new DonThuInfo();
            info.DonThuID = Utils.GetInt32(dr["DonThuID"], 0);
            info.TenLoaiDoiTuong = Utils.GetString(dr["TenLoaiDoiTuong"], string.Empty);
            info.HoTen = Utils.GetString(dr["HoTen"], string.Empty);
            //info.DiaChiCT = Utils.GetString(dr["DiaChiCT"], string.Empty);
            info.TenLoaiKhieuTo = Utils.GetString(dr["TenLoaiKhieuTo"], string.Empty);
            info.NoiDungDon = Utils.GetString(dr["NoiDungDon"], string.Empty);
            info.SoLanTrung = Utils.GetInt32(dr["SoLanTrung"], 0);


            return info;
        }
        #endregion

        #region -- get data for show
        private DonThuInfo GetDataForShow(SqlDataReader dr)
        {
            DonThuInfo DTInfo = new DonThuInfo();
            DTInfo.DonThuID = Utils.GetInt32(dr["DonThuID"], 0);

            DTInfo.NgayNhapDon = Utils.GetDateTime(dr["NgayNhapDon"], Constant.DEFAULT_DATE);

            DTInfo.NoiDungDon = Utils.GetString(dr["NoiDungDon"], string.Empty);

            if (DTInfo.NoiDungDon.Length > Constant.LengthNoiDungDon)
            {
                DTInfo.NoiDungDon = DTInfo.NoiDungDon.Substring(0, Constant.LengthNoiDungDon) + Constant.ChuoiCuoiNDDon;
            }
            DTInfo.HoTen = Utils.GetString(dr["HoTen"].ToString(), String.Empty);
            //DTInfo.DiaChiCT = Utils.GetString(dr["DiaChiCT"].ToString(), String.Empty);

            DTInfo.TenLoaiKhieuTo = Utils.GetString(dr["TenLoaiKhieuTo"].ToString(), String.Empty);
            DTInfo.TenCanBo = Utils.GetString(dr["TenCanBo"], String.Empty);
            DTInfo.TenCoQuan = Utils.GetString(dr["TenCoQuan"], String.Empty);
            DTInfo.TenXa = Utils.GetString(dr["TenXa"], String.Empty);
            DTInfo.TenHuyen = Utils.GetString(dr["TenHuyen"], String.Empty);
            DTInfo.TenTinh = Utils.GetString(dr["TenTinh"], String.Empty);
            DTInfo.SoLan = Utils.GetInt32(dr["SoLan"], 0);
            DTInfo.TrangThaiDonID = Utils.GetInt32(dr["TrangThaiDonID"], 0);
            DTInfo.CoQuanID = Utils.GetInt32(dr["CoQuanID"], 0);
            DTInfo.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);
            DTInfo.SoLuong = Utils.GetInt32(dr["SoLuong"], 0);
            DTInfo.TenLoaiDoiTuong = Utils.GetString(dr["TenLoaiDoiTuong"], String.Empty);
            DTInfo.SoDonThu = Utils.GetString(dr["SoDonThu"], string.Empty);
            DTInfo.NhomKNID = Utils.GetInt32(dr["NhomKNID"], 0);
            DTInfo.CanBoTiepNhapID = Utils.GetInt32(dr["CanBoTiepNhapID"], 0);

            return DTInfo;
        }
        #endregion


        //public DonThuInfo GetCMNDByDonThuID(int donthuID)
        //{

        //    DonThuInfo DTInfo = null;
        //    SqlParameter[] parameters = new SqlParameter[]{
        //        new SqlParameter(PARAM_DON_THU_ID,SqlDbType.Int)
        //    };
        //    parameters[0].Value = donthuID;
        //    try
        //    {
        //        using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_CMND_BY_DONTHU_ID, parameters))
        //        {

        //            if (dr.Read())
        //            {
        //                DTInfo = GetDataForShow(dr);
        //            }
        //            dr.Close();
        //        }
        //    }
        //    catch
        //    {
        //    }
        //    return DTInfo;
        //}

        #region -- get don thu getbypage
        public IList<DonThuInfo> GetByPage(int page, int coquanID, int TrangThaiDonID, string CMND, int loaiKhieuToID)
        {
            int start = (page - 1) * Constant.PageSize;
            int end = page * Constant.PageSize;

            IList<DonThuInfo> ls_donthu = new List<DonThuInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_CO_QUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TRANG_THAI_DON_ID,SqlDbType.Int),
                new SqlParameter(PARAM_STARTS,SqlDbType.Int),
                new SqlParameter(PARAM_ENDS,SqlDbType.Int),
                new SqlParameter(PARAM_CMND,SqlDbType.VarChar),
                new SqlParameter(PARAM__LOAI_KHIEU_TO_ID,SqlDbType.Int)
            };

            parameters[0].Value = coquanID;
            parameters[1].Value = TrangThaiDonID;
            parameters[2].Value = start;
            parameters[3].Value = end;
            parameters[4].Value = CMND;
            parameters[5].Value = loaiKhieuToID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_PAGE, parameters))
                {
                    while (dr.Read())
                    {
                        DonThuInfo DTInfo = GetDataForShow(dr);
                        ls_donthu.Add(DTInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return ls_donthu;
        }
        #endregion

        #region --delete don thu
        public int Delete_DonThu(int XLD_ID)
        {

            int val = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XU_LY_DON_ID,SqlDbType.Int)
            };
            parameters[0].Value = XLD_ID;
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
        #endregion

        #region -- OrderByLoaiDon
        public IList<DonThuInfo> OrderByLoaiDon(int page, int coquanID, int TrangThaiDonID, string CMND)
        {
            int start = (page - 1) * Constant.PageSize;
            int end = page * Constant.PageSize;

            IList<DonThuInfo> ls_donthu = new List<DonThuInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_CO_QUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TRANG_THAI_DON_ID,SqlDbType.Int),
                new SqlParameter(PARAM_PAGE,SqlDbType.Int),
                new SqlParameter(PARAM_LIMIT,SqlDbType.Int),
                new SqlParameter(PARAM_CMND,SqlDbType.VarChar)
            };

            parameters[0].Value = coquanID;
            parameters[1].Value = TrangThaiDonID;
            parameters[2].Value = page;
            parameters[3].Value = Constant.PageSize;
            parameters[4].Value = CMND;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, ORDER_BY_LOAI_DON, parameters))
                {
                    while (dr.Read())
                    {
                        DonThuInfo DTInfo = GetDataForShow(dr);
                        ls_donthu.Add(DTInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return ls_donthu;
        }
        #endregion

        #region -- OrderByTenChuDon
        public IList<DonThuInfo> OrderByTenChuDon(int page, int coquanID, int TrangThaiDonID, string CMND)
        {
            int start = (page - 1) * Constant.PageSize;
            int end = page * Constant.PageSize;

            IList<DonThuInfo> ls_donthu = new List<DonThuInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_CO_QUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TRANG_THAI_DON_ID,SqlDbType.Int),
                new SqlParameter(PARAM_PAGE,SqlDbType.Int),
                new SqlParameter(PARAM_LIMIT,SqlDbType.Int),
                new SqlParameter(PARAM_CMND,SqlDbType.VarChar)
            };

            parameters[0].Value = coquanID;
            parameters[1].Value = TrangThaiDonID;
            parameters[2].Value = page;
            parameters[3].Value = Constant.PageSize;
            parameters[4].Value = CMND;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, ORDER_BY_TEN_CHU_DON, parameters))
                {
                    while (dr.Read())
                    {
                        DonThuInfo DTInfo = GetDataForShow(dr);
                        ls_donthu.Add(DTInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return ls_donthu;
        }
        #endregion

        #region --OrderByTenCoQuan
        public IList<DonThuInfo> OrderByTenCoQuan(int page, int coquanID, int TrangThaiDonID, string CMND)
        {
            int start = (page - 1) * Constant.PageSize;
            int end = page * Constant.PageSize;

            IList<DonThuInfo> ls_donthu = new List<DonThuInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_CO_QUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TRANG_THAI_DON_ID,SqlDbType.Int),
                new SqlParameter(PARAM_PAGE,SqlDbType.Int),
                new SqlParameter(PARAM_LIMIT,SqlDbType.Int),
                new SqlParameter(PARAM_CMND,SqlDbType.VarChar)
            };

            parameters[0].Value = coquanID;
            parameters[1].Value = TrangThaiDonID;
            parameters[2].Value = page;
            parameters[3].Value = Constant.PageSize;
            parameters[4].Value = CMND;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, ORDER_BY_TEN__CO_QUAN, parameters))
                {
                    while (dr.Read())
                    {
                        DonThuInfo DTInfo = GetDataForShow(dr);
                        ls_donthu.Add(DTInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return ls_donthu;
        }
        #endregion

        #region -- OrderByNgayNhapDon
        public IList<DonThuInfo> OrderByNgayNhapDon(int page, int coquanID, int TrangThaiDonID, string CMND)
        {
            int start = (page - 1) * Constant.PageSize;
            int end = page * Constant.PageSize;

            IList<DonThuInfo> ls_donthu = new List<DonThuInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_CO_QUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TRANG_THAI_DON_ID,SqlDbType.Int),
                new SqlParameter(PARAM_PAGE,SqlDbType.Int),
                new SqlParameter(PARAM_LIMIT,SqlDbType.Int),
                new SqlParameter(PARAM_CMND,SqlDbType.VarChar)
            };

            parameters[0].Value = coquanID;
            parameters[1].Value = TrangThaiDonID;
            parameters[2].Value = page;
            parameters[3].Value = Constant.PageSize;
            parameters[4].Value = CMND;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, ORDER_BY_NGAY_NHAP_DON, parameters))
                {
                    while (dr.Read())
                    {
                        DonThuInfo DTInfo = GetDataForShow(dr);
                        ls_donthu.Add(DTInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return ls_donthu;
        }
        #endregion

        #region --get by search
        public IList<DonThuInfo> GetBySearch(string keyword, int lktID, DateTime tuNgay, DateTime denNgay, int coquanID, int start, int end, int huyenID = 0)
        {

            IList<DonThuInfo> ls_donthu = new List<DonThuInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_CO_QUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM__LOAI_KHIEU_TO_ID, SqlDbType.Int),
                new SqlParameter(PARAM_KEYWORD, SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_START, SqlDbType.Int),
                new SqlParameter(PARAM_END, SqlDbType.Int),
                new SqlParameter(PARAM_HUYENID, SqlDbType.Int)
                
                //new SqlParameter(PARAM_CMND,SqlDbType.VarChar)
            };

            parameters[0].Value = coquanID;
            parameters[1].Value = tuNgay;
            parameters[2].Value = denNgay;
            parameters[3].Value = lktID;
            parameters[4].Value = keyword;
            parameters[5].Value = start;
            parameters[6].Value = end;
            parameters[7].Value = huyenID;

            if (tuNgay == DateTime.MinValue) parameters[1].Value = DBNull.Value;
            if (denNgay == DateTime.MinValue) parameters[2].Value = DBNull.Value;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, SEARCH, parameters))
                {
                    while (dr.Read())
                    {
                        DonThuInfo DTInfo = new DonThuInfo();//GetDataForShow(dr);
                        DTInfo.DonThuID = Utils.GetInt32(dr["DonThuID"], 0);

                        //DTInfo.NgayNhapDon = Utils.GetDateTime(dr["NgayNhapDon"], Constant.DEFAULT_DATE);

                        DTInfo.NoiDungDon = Utils.GetString(dr["NoiDungDon"], string.Empty);

                        if (DTInfo.NoiDungDon.Length > Constant.LengthNoiDungDon)
                        {
                            DTInfo.NoiDungDon = DTInfo.NoiDungDon.Substring(0, Constant.LengthNoiDungDon) + Constant.ChuoiCuoiNDDon;
                        }
                        DTInfo.HoTen = Utils.GetString(dr["HoTen"].ToString(), String.Empty);
                        //DTInfo.DiaChiCT = Utils.GetString(dr["DiaChiCT"].ToString(), String.Empty);

                        DTInfo.TenLoaiKhieuTo = Utils.GetString(dr["TenLoaiKhieuTo"].ToString(), String.Empty);
                        //DTInfo.TenCanBo = Utils.GetString(dr["TenCanBo"], String.Empty);
                        //DTInfo.TenCoQuan = Utils.GetString(dr["TenCoQuan"], String.Empty);
                        DTInfo.TenXa = Utils.GetString(dr["TenXa"], String.Empty);
                        DTInfo.TenHuyen = Utils.GetString(dr["TenHuyen"], String.Empty);
                        DTInfo.TenTinh = Utils.GetString(dr["TenTinh"], String.Empty);
                        //DTInfo.SoLan = Utils.GetInt32(dr["SoLan"], 0);
                        //DTInfo.TrangThaiDonID = Utils.GetInt32(dr["TrangThaiDonID"], 0);
                        //DTInfo.CoQuanID = Utils.GetInt32(dr["CoQuanID"], 0);
                        //DTInfo.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);
                        DTInfo.SoLuong = Utils.GetInt32(dr["SoLuong"], 0);
                        DTInfo.TenLoaiDoiTuong = Utils.GetString(dr["TenLoaiDoiTuong"], String.Empty);
                        //DTInfo.SoDonThu = Utils.GetString(dr["SoDonThu"], string.Empty);
                        DTInfo.NhomKNID = Utils.GetInt32(dr["NhomKNID"], 0);
                        DTInfo.DoiTuongBiKNID = Utils.GetInt32(dr["DoiTuongBiKNID"], 0);
                        //DTInfo.CanBoTiepNhapID = Utils.GetInt32(dr["CanBoTiepNhapID"], 0);
                        //DTInfo.CQDaGiaiQuyet = Utils.ConvertToString(dr["CQDaGiaiQuyetID"], string.Empty);
                        //DTInfo.NgayQuaHanGQ = Utils.ConvertToDateTime(dr["HanGiaiQuyet"], DateTime.MinValue);
                        //DTInfo.NgayQuaHanGQ = Utils.GetDateTime(dr["HanGiaiQuyet"], Constant.DEFAULT_DATE);
                        //DTInfo.TenNguonDonDen = Utils.ConvertToString(dr["TenNguonDonDen"], string.Empty);
                        //DTInfo.TenHuongGiaiQuyet = Utils.ConvertToString(dr["HuongXuLy"], string.Empty);
                        ls_donthu.Add(DTInfo);
                    }
                }
            }
            catch
            {

                throw;
            }
            return ls_donthu;
        }


        public IList<DonThuInfo> GetBySearch_TraCuu_AllCoQuan(string keyword, int lktID, int coquanID, int start, int end)
        {

            IList<DonThuInfo> ls_donthu = new List<DonThuInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_CO_QUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM__LOAI_KHIEU_TO_ID, SqlDbType.Int),
                new SqlParameter(PARAM_KEYWORD, SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_START, SqlDbType.Int),
                new SqlParameter(PARAM_END, SqlDbType.Int),
            };

            parameters[0].Value = coquanID;
            parameters[1].Value = lktID;
            parameters[2].Value = keyword;
            parameters[3].Value = start;
            parameters[4].Value = end;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "NV_DonThu_GetBySearch_TraCuu_AllCoQuan_New", parameters))
                {
                    while (dr.Read())
                    {
                        DonThuInfo DTInfo = new DonThuInfo();
                        DTInfo.DonThuID = Utils.GetInt32(dr["DonThuID"], 0);
                        DTInfo.NoiDungDon = Utils.GetString(dr["NoiDungDon"], string.Empty);

                        if (DTInfo.NoiDungDon.Length > Constant.LengthNoiDungDon)
                        {
                            DTInfo.NoiDungDon = DTInfo.NoiDungDon.Substring(0, Constant.LengthNoiDungDon) + Constant.ChuoiCuoiNDDon;
                        }
                        DTInfo.HoTen = Utils.GetString(dr["HoTen"].ToString(), String.Empty);
                        DTInfo.TenLoaiKhieuTo = Utils.GetString(dr["TenLoaiKhieuTo"].ToString(), String.Empty);
                        DTInfo.TenXa = Utils.GetString(dr["TenXa"], String.Empty);
                        DTInfo.TenHuyen = Utils.GetString(dr["TenHuyen"], String.Empty);
                        DTInfo.TenTinh = Utils.GetString(dr["TenTinh"], String.Empty);
                        DTInfo.SoLuong = Utils.GetInt32(dr["SoLuong"], 0);
                        DTInfo.TenLoaiDoiTuong = Utils.GetString(dr["TenLoaiDoiTuong"], String.Empty);
                        DTInfo.NhomKNID = Utils.GetInt32(dr["NhomKNID"], 0);
                        DTInfo.DoiTuongBiKNID = Utils.GetInt32(dr["DoiTuongBiKNID"], 0);
                        DTInfo.NgayNhapDon = Utils.GetDateTime(dr["NgayNhapDon"].ToString(), DateTime.MinValue);
                        DTInfo.NgayNhapDonStr = Format.FormatDate(DTInfo.NgayNhapDon);
                        //DTInfo.TenCoQuan = Utils.GetString(dr["TenCoQuan"], String.Empty);
                        DTInfo.Count = Utils.GetInt32(dr["CountNum"], 0);
                        ls_donthu.Add(DTInfo);
                    }
                }
            }
            catch
            {

                throw;
            }
            return ls_donthu;
        }

        public IList<DonThuInfo> GetBySearch_TraCuu(string keyword, int lktID, int coquanID, int start, int end)
        {

            IList<DonThuInfo> ls_donthu = new List<DonThuInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_CO_QUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM__LOAI_KHIEU_TO_ID, SqlDbType.Int),
                new SqlParameter(PARAM_KEYWORD, SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_START, SqlDbType.Int),
                new SqlParameter(PARAM_END, SqlDbType.Int),
            };

            parameters[0].Value = coquanID;
            parameters[1].Value = lktID;
            parameters[2].Value = keyword;
            parameters[3].Value = start;
            parameters[4].Value = end;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "NV_DonThu_GetBySearch_TraCuu_New", parameters))
                {
                    while (dr.Read())
                    {
                        DonThuInfo DTInfo = new DonThuInfo();
                        DTInfo.DonThuID = Utils.GetInt32(dr["DonThuID"], 0);
                        DTInfo.NoiDungDon = Utils.GetString(dr["NoiDungDon"], string.Empty);

                        if (DTInfo.NoiDungDon.Length > Constant.LengthNoiDungDon)
                        {
                            DTInfo.NoiDungDon = DTInfo.NoiDungDon.Substring(0, Constant.LengthNoiDungDon) + Constant.ChuoiCuoiNDDon;
                        }
                        DTInfo.HoTen = Utils.GetString(dr["HoTen"].ToString(), String.Empty);
                        DTInfo.TenLoaiKhieuTo = Utils.GetString(dr["TenLoaiKhieuTo"].ToString(), String.Empty);
                        DTInfo.TenXa = Utils.GetString(dr["TenXa"], String.Empty);
                        DTInfo.TenHuyen = Utils.GetString(dr["TenHuyen"], String.Empty);
                        DTInfo.TenTinh = Utils.GetString(dr["TenTinh"], String.Empty);
                        DTInfo.SoLuong = Utils.GetInt32(dr["SoLuong"], 0);
                        DTInfo.TenLoaiDoiTuong = Utils.GetString(dr["TenLoaiDoiTuong"], String.Empty);
                        DTInfo.NhomKNID = Utils.GetInt32(dr["NhomKNID"], 0);
                        DTInfo.DoiTuongBiKNID = Utils.GetInt32(dr["DoiTuongBiKNID"], 0);
                        DTInfo.NgayNhapDon = Utils.GetDateTime(dr["NgayNhapDon"].ToString(), DateTime.MinValue);
                        DTInfo.NgayNhapDonStr = Format.FormatDate(DTInfo.NgayNhapDon);
                        //DTInfo.TenCoQuan = Utils.GetString(dr["TenCoQuan"], String.Empty);
                        DTInfo.Count = Utils.GetInt32(dr["CountNum"], 0);
                        ls_donthu.Add(DTInfo);
                    }
                }
            }
            catch
            {

                throw;
            }
            return ls_donthu;
        }

        public IList<DonThuInfo> GetBySearch_TraCuu_CapHuyen(string keyword, int lktID, int coquanID, int coQuanCapHuyen, int capHuyen, int capXa, int start, int end)
        {

            IList<DonThuInfo> ls_donthu = new List<DonThuInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_CO_QUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM__LOAI_KHIEU_TO_ID, SqlDbType.Int),
                new SqlParameter(PARAM_KEYWORD, SqlDbType.NVarChar,50),
                                new SqlParameter(@"CoQuanCapHuyenID", SqlDbType.Int),
                new SqlParameter(@"CapHuyen", SqlDbType.Int),
                new SqlParameter(@"XapXa", SqlDbType.Int),
                new SqlParameter(PARAM_START, SqlDbType.Int),
                new SqlParameter(PARAM_END, SqlDbType.Int),
            };

            parameters[0].Value = coquanID;
            parameters[1].Value = lktID;
            parameters[2].Value = keyword;
            parameters[3].Value = coQuanCapHuyen;
            parameters[4].Value = capHuyen;
            parameters[5].Value = capXa;
            parameters[6].Value = start;
            parameters[7].Value = end;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "NV_DonThu_GetBySearch_TraCuu_CapHuyen_New", parameters))
                {
                    while (dr.Read())
                    {
                        DonThuInfo DTInfo = new DonThuInfo();
                        DTInfo.DonThuID = Utils.GetInt32(dr["DonThuID"], 0);
                        DTInfo.NoiDungDon = Utils.GetString(dr["NoiDungDon"], string.Empty);

                        if (DTInfo.NoiDungDon.Length > Constant.LengthNoiDungDon)
                        {
                            DTInfo.NoiDungDon = DTInfo.NoiDungDon.Substring(0, Constant.LengthNoiDungDon) + Constant.ChuoiCuoiNDDon;
                        }
                        DTInfo.HoTen = Utils.GetString(dr["HoTen"].ToString(), String.Empty);
                        DTInfo.TenLoaiKhieuTo = Utils.GetString(dr["TenLoaiKhieuTo"].ToString(), String.Empty);
                        DTInfo.TenXa = Utils.GetString(dr["TenXa"], String.Empty);
                        DTInfo.TenHuyen = Utils.GetString(dr["TenHuyen"], String.Empty);
                        DTInfo.TenTinh = Utils.GetString(dr["TenTinh"], String.Empty);
                        DTInfo.SoLuong = Utils.GetInt32(dr["SoLuong"], 0);
                        DTInfo.TenLoaiDoiTuong = Utils.GetString(dr["TenLoaiDoiTuong"], String.Empty);
                        DTInfo.NhomKNID = Utils.GetInt32(dr["NhomKNID"], 0);
                        DTInfo.DoiTuongBiKNID = Utils.GetInt32(dr["DoiTuongBiKNID"], 0);
                        DTInfo.NgayNhapDon = Utils.GetDateTime(dr["NgayNhapDon"].ToString(), DateTime.MinValue);
                        DTInfo.NgayNhapDonStr = Format.FormatDate(DTInfo.NgayNhapDon);
                        //DTInfo.TenCoQuan = Utils.GetString(dr["TenCoQuan"], String.Empty);
                        DTInfo.Count = Utils.GetInt32(dr["CountNum"], 0);
                        ls_donthu.Add(DTInfo);
                    }
                }
            }
            catch
            {

                throw;
            }
            return ls_donthu;
        }
        #endregion

        #region -- get don thu trung
        public IList<DonThuInfo> GetDonThuTrung(int donThuID)
        {

            IList<DonThuInfo> ls_donthu = new List<DonThuInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_DON_THU_ID,SqlDbType.Int)
            };

            parameters[0].Value = donThuID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GETDONTHU_TRUNGDON, parameters))
                {
                    while (dr.Read())
                    {
                        DonThuInfo DTInfo = GetDataForShow(dr);
                        DTInfo.CQDaGiaiQuyet = Utils.ConvertToString(dr["CQDaGiaiQuyetID"], string.Empty);
                        //DTInfo.NgayQuaHanGQ = Utils.ConvertToDateTime(dr["HanGiaiQuyet"], DateTime.MinValue);
                        DTInfo.NgayQuaHanGQ = Utils.GetDateTime(dr["HanGiaiQuyet"], Constant.DEFAULT_DATE);
                        DTInfo.TenNguonDonDen = Utils.ConvertToString(dr["TenNguonDonDen"], string.Empty);
                        DTInfo.TenHuongGiaiQuyet = Utils.ConvertToString(dr["HuongXuLy"], string.Empty);
                        DTInfo.NgayNhapDonStr = Format.FormatDate(DTInfo.NgayNhapDon);
                        DTInfo.TenTinh = Utils.ConvertToString(dr["TenTinh"], string.Empty);
                        DTInfo.TenHuyen = Utils.ConvertToString(dr["TenHuyen"], string.Empty);
                        DTInfo.TenXa = Utils.ConvertToString(dr["TenXa"], string.Empty);

                        ls_donthu.Add(DTInfo);
                    }
                }
            }
            catch
            {

                throw;
            }
            return ls_donthu;
        }
        #endregion

        #region -- get don thu trung, bình thường, khiếu tố lần 2
        public IList<DonThuInfo> GetDSXuLyDonByDonThuID(int donThuID)
        {

            IList<DonThuInfo> ls_donthu = new List<DonThuInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_DON_THU_ID,SqlDbType.Int)
            };

            parameters[0].Value = donThuID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "NV_DonThu_GetDSDonThuByDonThuID", parameters))
                {
                    while (dr.Read())
                    {
                        DonThuInfo DTInfo = GetDataForShow(dr);
                        DTInfo.TenCoQuanGiaiQuyet = Utils.ConvertToString(dr["TenCoQuanGiaiQuyet"], string.Empty);
                        DTInfo.CQDaGiaiQuyet = Utils.ConvertToString(dr["CQDaGiaiQuyetID"], string.Empty);
                        //DTInfo.NgayQuaHanGQ = Utils.ConvertToDateTime(dr["HanGiaiQuyet"], DateTime.MinValue);
                        DTInfo.NgayQuaHanGQ = Utils.GetDateTime(dr["HanGiaiQuyet"], Constant.DEFAULT_DATE);
                        DTInfo.TenNguonDonDen = Utils.ConvertToString(dr["TenNguonDonDen"], string.Empty);
                        DTInfo.TenHuongGiaiQuyet = Utils.ConvertToString(dr["HuongXuLy"], string.Empty);
                        DTInfo.NgayNhapDonStr = Format.FormatDate(DTInfo.NgayNhapDon);
                        DTInfo.TenTinh = Utils.ConvertToString(dr["TenTinh"], string.Empty);
                        DTInfo.TenHuyen = Utils.ConvertToString(dr["TenHuyen"], string.Empty);
                        DTInfo.TenXa = Utils.ConvertToString(dr["TenXa"], string.Empty);
                        DTInfo.LanGiaiQuyet = Utils.ConvertToInt32(dr["LanGiaiQuyet"], 0);
                        DTInfo.StateID = Utils.ConvertToInt32(dr["StateID"], 0);

                        //DTInfo.LanGQ = new DAL.TiepCongDan.TiepDan().CountSoLanGQ(donThuID, 10);
                        ls_donthu.Add(DTInfo);
                    }
                }
            }
            catch
            {

                throw;
            }
            return ls_donthu;
        }

        public int GetDSLanTiepNhanByDonThuID(int donThuID)
        {
            int result = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_DON_THU_ID,SqlDbType.Int)
            };

            parameters[0].Value = donThuID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "NV_DonThu_DTTiepNhanByDonThuID", parameters))
                {
                    while (dr.Read())
                    {
                        result = Utils.ConvertToInt32(dr["CountRow"], 0);
                    }
                }
            }
            catch
            {

                throw;
            }
            return result;
        }

        public List<DonThuInfo> GetDSXuLyDonByListDonThuID(List<DonThuInfo> listDonThuID)
        {
            SqlParameter param = new SqlParameter(PARAM_LISTID, SqlDbType.Structured);
            param.TypeName = "IntList";
            var dataTable = new DataTable();
            dataTable.Columns.Add("n", typeof(int));
            foreach (var DonThu in listDonThuID)
            {
                dataTable.Rows.Add(DonThu.DonThuID);
            }

            List<DonThuInfo> ls_donthu = new List<DonThuInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                param
            };

            parameters[0].Value = dataTable;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "NV_GetDSXuLyDonByListDonThuID", parameters))
                {
                    while (dr.Read())
                    {
                        DonThuInfo DTInfo = new DonThuInfo();
                        DTInfo.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
                        DTInfo.DonThuID = Utils.ConvertToInt32(dr["DonThuID"], 0);
                        DTInfo.DonThuGocID = Utils.ConvertToInt32(dr["DonThuGocID"], 0);
                        ls_donthu.Add(DTInfo);
                    }
                }
            }
            catch
            {

                throw;
            }
            return ls_donthu;
        }
        #endregion

        #region -- get yeu cau doi thoai
        public IList<DonThuInfo> GetDTYeuCauDoiThoai(string keyword, int lktID, DateTime tuNgay, DateTime denNgay, int coquanID, int start, int end, int huyenID = 0)
        {

            IList<DonThuInfo> ls_donthu = new List<DonThuInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_CO_QUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM__LOAI_KHIEU_TO_ID, SqlDbType.Int),
                new SqlParameter(PARAM_KEYWORD, SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_START, SqlDbType.Int),
                new SqlParameter(PARAM_END, SqlDbType.Int),
                new SqlParameter(PARAM_HUYENID, SqlDbType.Int)
                
                //new SqlParameter(PARAM_CMND,SqlDbType.VarChar)
            };

            parameters[0].Value = coquanID;
            parameters[1].Value = tuNgay;
            parameters[2].Value = denNgay;
            parameters[3].Value = lktID;
            parameters[4].Value = keyword;
            parameters[5].Value = start;
            parameters[6].Value = end;
            parameters[7].Value = huyenID;

            if (tuNgay == DateTime.MinValue) parameters[1].Value = DBNull.Value;
            if (denNgay == DateTime.MinValue) parameters[2].Value = DBNull.Value;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DTYEUCAU_DOITHOAI, parameters))
                {
                    while (dr.Read())
                    {
                        DonThuInfo DTInfo = GetDataForShow(dr);
                        DTInfo.CQDaGiaiQuyet = Utils.ConvertToString(dr["CQDaGiaiQuyetID"], string.Empty);
                        //DTInfo.NgayQuaHanGQ = Utils.ConvertToDateTime(dr["HanGiaiQuyet"], DateTime.MinValue);
                        DTInfo.NgayQuaHanGQ = Utils.GetDateTime(dr["HanGiaiQuyet"], Constant.DEFAULT_DATE);
                        DTInfo.TenNguonDonDen = Utils.ConvertToString(dr["TenNguonDonDen"], string.Empty);
                        DTInfo.TenHuongGiaiQuyet = Utils.ConvertToString(dr["HuongXuLy"], string.Empty);
                        ls_donthu.Add(DTInfo);
                    }
                }
            }
            catch
            {

                throw;
            }
            return ls_donthu;
        }

        public int CountDTYeuCauDoiThoai(string keyword, int lktID, DateTime tuNgay, DateTime denNgay, int coquanID, int huyenID = 0)
        {

            object result = 0;


            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_CO_QUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM__LOAI_KHIEU_TO_ID, SqlDbType.Int),
                new SqlParameter(PARAM_KEYWORD, SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_HUYENID, SqlDbType.Int),
                //new SqlParameter(PARAM_CMND,SqlDbType.VarChar)
            };

            parameters[0].Value = coquanID;
            parameters[1].Value = tuNgay;
            parameters[2].Value = denNgay;
            parameters[3].Value = lktID;
            parameters[4].Value = keyword;
            parameters[5].Value = huyenID;

            if (tuNgay == DateTime.MinValue) parameters[1].Value = DBNull.Value;
            if (denNgay == DateTime.MinValue) parameters[2].Value = DBNull.Value;

            try
            {
                result = SQLHelper.ExecuteScalar(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_DTYEUCAUDOITHOAI, parameters);

            }
            catch
            {
            }

            return Utils.ConvertToInt32(result, 0);


        }
        #endregion

        #region -- count don thu trung by donthuid
        public int CountDTTrungByID(int donThuID)
        {

            object result = 0;


            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_DON_THU_ID,SqlDbType.Int)
            };

            parameters[0].Value = donThuID;

            try
            {
                result = SQLHelper.ExecuteScalar(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_DT_BY_DONTHUID, parameters);

            }
            catch
            {
            }

            return Utils.ConvertToInt32(result, 0);
        }
        #endregion

        #region -- get don thu gian tiep
        public IList<DonThuInfo> GetDataTiepGianTiep(string keyword, int lktID, DateTime tuNgay, DateTime denNgay, int coquanID, int start, int end, int huyenID = 0)
        {

            IList<DonThuInfo> ls_donthu = new List<DonThuInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_CO_QUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM__LOAI_KHIEU_TO_ID, SqlDbType.Int),
                new SqlParameter(PARAM_KEYWORD, SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_START, SqlDbType.Int),
                new SqlParameter(PARAM_END, SqlDbType.Int),
                new SqlParameter(PARAM_HUYENID, SqlDbType.Int)
                
                //new SqlParameter(PARAM_CMND,SqlDbType.VarChar)
            };

            parameters[0].Value = coquanID;
            parameters[1].Value = tuNgay;
            parameters[2].Value = denNgay;
            parameters[3].Value = lktID;
            parameters[4].Value = keyword;
            parameters[5].Value = start;
            parameters[6].Value = end;
            parameters[7].Value = huyenID;

            if (tuNgay == DateTime.MinValue) parameters[1].Value = DBNull.Value;
            if (denNgay == DateTime.MinValue) parameters[2].Value = DBNull.Value;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GETDATA_TIEPNHAN_GIANTIEP, parameters))
                {
                    while (dr.Read())
                    {
                        DonThuInfo DTInfo = GetDataForShow(dr);
                        DTInfo.CQDaGiaiQuyet = Utils.ConvertToString(dr["CQDaGiaiQuyetID"], string.Empty);
                        //DTInfo.NgayQuaHanGQ = Utils.ConvertToDateTime(dr["HanGiaiQuyet"], DateTime.MinValue);
                        DTInfo.NgayQuaHanGQ = Utils.GetDateTime(dr["HanGiaiQuyet"], Constant.DEFAULT_DATE);
                        DTInfo.TenNguonDonDen = Utils.ConvertToString(dr["TenNguonDonDen"], string.Empty);
                        DTInfo.TenHuongGiaiQuyet = Utils.ConvertToString(dr["HuongXuLy"], string.Empty);
                        DTInfo.StateName = Utils.ConvertToString(dr["StateName"], string.Empty);
                        ls_donthu.Add(DTInfo);
                    }
                }
            }
            catch
            {

                throw;
            }
            return ls_donthu;
        }
        #endregion

        //-------------------
        //public IList<DonThuInfo> GetBySearch(string keyword, int page, int coquanID, int TrangThaiDonID, int ddl_search,string CMND)
        //{
        //    int start = (page - 1) * Constant.PageSize;
        //    int end = page * Constant.PageSize;

        //    IList<DonThuInfo> ls_donthu = new List<DonThuInfo>();
        //    SqlParameter[] parameters = new SqlParameter[]{
        //        new SqlParameter(PARAM_KEY,SqlDbType.NVarChar,50),
        //        new SqlParameter(PARAM_CO_QUAN_ID,SqlDbType.Int),
        //        new SqlParameter(PARAM_TRANG_THAI_DON_ID,SqlDbType.Int),
        //        new SqlParameter(PARAM_START,SqlDbType.Int),
        //        new SqlParameter(PARAM_END,SqlDbType.Int),
        //        new SqlParameter(PARAM_DDl_SEARCH,SqlDbType.Int),
        //        new SqlParameter(PARAM_CMND,SqlDbType.VarChar)
        //    };
        //    parameters[0].Value = keyword;
        //    parameters[1].Value = coquanID;
        //    parameters[2].Value = TrangThaiDonID;
        //    parameters[3].Value = start;
        //    parameters[4].Value = end;
        //    parameters[5].Value = ddl_search;
        //    parameters[6].Value = CMND;
        //    try
        //    {
        //        using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, SEARCH, parameters))
        //        {
        //            while (dr.Read())
        //            {
        //                DonThuInfo cInfo = GetDataForShow(dr);
        //                ls_donthu.Add(cInfo);
        //            }
        //            dr.Close();
        //        }
        //    }
        //    catch
        //    {

        //        throw;
        //    }
        //    return ls_donthu;
        //}

        #region -- count search
        public int CountSearch(string keyword, int lktID, DateTime tuNgay, DateTime denNgay, int coquanID, int huyenID = 0)
        {

            object result = 0;


            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_CO_QUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM__LOAI_KHIEU_TO_ID, SqlDbType.Int),
                new SqlParameter(PARAM_KEYWORD, SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_HUYENID, SqlDbType.Int),
                //new SqlParameter(PARAM_CMND,SqlDbType.VarChar)
            };

            parameters[0].Value = coquanID;
            parameters[1].Value = tuNgay;
            parameters[2].Value = denNgay;
            parameters[3].Value = lktID;
            parameters[4].Value = keyword;
            parameters[5].Value = huyenID;

            if (tuNgay == DateTime.MinValue) parameters[1].Value = DBNull.Value;
            if (denNgay == DateTime.MinValue) parameters[2].Value = DBNull.Value;

            try
            {
                result = SQLHelper.ExecuteScalar(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_SEARCH, parameters);

            }
            catch
            {
            }

            return Utils.ConvertToInt32(result, 0);


        }

        public int CountSearch_TraCuu_AllCoQuan(string keyword, int lktID, int coquanID)
        {
            object result = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_CO_QUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM__LOAI_KHIEU_TO_ID, SqlDbType.Int),
                new SqlParameter(PARAM_KEYWORD, SqlDbType.NVarChar,50),
            };

            parameters[0].Value = coquanID;
            parameters[1].Value = lktID;
            parameters[2].Value = keyword;
            try
            {
                result = SQLHelper.ExecuteScalar(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "NV_DonThu_CountSearch_TraCuu_AllCoQuan", parameters);

            }
            catch
            {
            }
            return Utils.ConvertToInt32(result, 0);
        }


        public int CountSearch_TraCuu(string keyword, int lktID, int coquanID)
        {
            object result = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_CO_QUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM__LOAI_KHIEU_TO_ID, SqlDbType.Int),
                new SqlParameter(PARAM_KEYWORD, SqlDbType.NVarChar,50),
            };

            parameters[0].Value = coquanID;
            parameters[1].Value = lktID;
            parameters[2].Value = keyword;
            try
            {
                result = SQLHelper.ExecuteScalar(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "NV_DonThu_CountSearch_TraCuu", parameters);

            }
            catch
            {
            }
            return Utils.ConvertToInt32(result, 0);
        }

        public int CountSearch_TraCuu_CapHuyen(string keyword, int lktID, int coquanID, int coQuanCapHuyenID, int capHuyen, int capXa)
        {
            object result = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_CO_QUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM__LOAI_KHIEU_TO_ID, SqlDbType.Int),
                new SqlParameter(PARAM_KEYWORD, SqlDbType.NVarChar,50),
                new SqlParameter(@"CoQuanCapHuyenID", SqlDbType.Int),
                new SqlParameter(@"CapHuyen", SqlDbType.Int),
                new SqlParameter(@"XapXa", SqlDbType.Int),
            };

            parameters[0].Value = coquanID;
            parameters[1].Value = lktID;
            parameters[2].Value = keyword;
            parameters[3].Value = coQuanCapHuyenID;
            parameters[4].Value = capHuyen;
            parameters[5].Value = capXa;
            try
            {
                result = SQLHelper.ExecuteScalar(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "NV_DonThu_CountSearch_TraCuu_CapHuyen", parameters);

            }
            catch
            {
            }
            return Utils.ConvertToInt32(result, 0);
        }
        #endregion

        #region -- count donthu tiep nhan gian tiep
        public int CountData_TiepNhanGianTiep(string keyword, int lktID, DateTime tuNgay, DateTime denNgay, int coquanID, int huyenID = 0)
        {

            object result = 0;


            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_CO_QUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM__LOAI_KHIEU_TO_ID, SqlDbType.Int),
                new SqlParameter(PARAM_KEYWORD, SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_HUYENID, SqlDbType.Int),
                //new SqlParameter(PARAM_CMND,SqlDbType.VarChar)
            };

            parameters[0].Value = coquanID;
            parameters[1].Value = tuNgay;
            parameters[2].Value = denNgay;
            parameters[3].Value = lktID;
            parameters[4].Value = keyword;
            parameters[5].Value = huyenID;

            if (tuNgay == DateTime.MinValue) parameters[1].Value = DBNull.Value;
            if (denNgay == DateTime.MinValue) parameters[2].Value = DBNull.Value;

            try
            {
                result = SQLHelper.ExecuteScalar(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_DATA_TIEPNHANGIANTIEP, parameters);

            }
            catch
            {
            }

            return Utils.ConvertToInt32(result, 0);


        }
        #endregion

        #region -- COUNT ALL
        public int CountAll(int coQuanID, int TrangThaiID, string CMND)
        {
            object result;
            SqlParameter[] parameters = new SqlParameter[]{

                new SqlParameter(PARAM_CO_QUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TRANG_THAI_DON_ID,SqlDbType.Int),
                new SqlParameter(PARAM_CMND,SqlDbType.VarChar)
            };
            parameters[0].Value = coQuanID;
            parameters[1].Value = TrangThaiID;
            parameters[2].Value = CMND;

            //return result;

            //quanghv
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        result = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, COUNT_ALL, parameters);
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

            return Utils.ConvertToInt32(result, 0);
            //end quanghv


            //try
            //{
            //    using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_ALL, parameters))
            //    {

            //        if (dr.Read())
            //        {
            //            result = Utils.ConvertToInt32(dr["CountNum"], 0);
            //        }
            //        dr.Close();
            //    }
            //}
            //catch
            //{

            //    throw;
            //}
            //return result;
        }
        #endregion

        #region -- check trung don
        private const string CHECK_TRUNG_DON_BY_HOTEN = @"NV_DonThu_CheckTrungDonByHoTen";
        public IList<DonThuInfo> GetDonTrung(string hoTen, string cmnd, string diachi, string noidungdon)
        {
            IList<DonThuInfo> ls_donthu = new List<DonThuInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_HOTEN, SqlDbType.NVarChar, 50),
                new SqlParameter(PARAM_DIACHI, SqlDbType.NVarChar, 50),
                new SqlParameter(PARAM_CMND, SqlDbType.NVarChar, 50),
                new SqlParameter(PARAM_NOI_DUNG_DON, SqlDbType.NVarChar, 200)
            };

            parameters[0].Value = hoTen;
            parameters[1].Value = diachi;
            parameters[2].Value = cmnd;
            parameters[3].Value = noidungdon;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, CHECK_TRUNG_DON_BY_HOTEN, parameters))
                {
                    while (dr.Read())
                    {
                        DonThuInfo DTInfo = GetDataByHoTen(dr);
                        DTInfo.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
                        DTInfo.TenCoQuan = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        ls_donthu.Add(DTInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return ls_donthu;
        }
        private DonThuInfo GetDataByHoTen(SqlDataReader dr)
        {
            DonThuInfo info = new DonThuInfo();
            info.DonThuID = Utils.GetInt32(dr["DonThuID"], 0);
            info.CoQuanID = Utils.GetInt32(dr["CoQuanID"], 0);
            info.SoLanTrung = Utils.GetInt32(dr["SoLanTrung"], 0);
            info.HoTen = Utils.GetString(dr["HoTen"], string.Empty);
            info.DiaChiCT = Utils.GetString(dr["DiaChi"], string.Empty);
            info.TenLoaiKhieuTo = Utils.GetString(dr["TenLoaiKhieuTo"], string.Empty);
            info.NoiDungDon = Utils.GetString(dr["NoiDungDon"], string.Empty);
            info.NoiDungHuongDan = Utils.GetString(dr["NoiDungHuongDan"], string.Empty);
            info.TenHuongGiaiQuyet = Utils.GetString(dr["TenHuongGiaiQuyet"], string.Empty);
            info.LoaiKhieuTo1ID = Utils.ConvertToInt32(dr["LoaiKhieuTo1ID"], 0);

            return info;
        }
        #endregion

        #region -- get don thu by time
        private const string GET_IN_TIME = "NV_DonThu_GetInTime";
        public IList<DonThuInfo> GetInTime(DateTime startDate, DateTime endDate, int coquanID)
        {
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_CO_QUAN_ID, SqlDbType.Int)
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            parm[2].Value = coquanID;
            IList<DonThuInfo> LsTDKD = new List<DonThuInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_IN_TIME, parm))
                {

                    while (dr.Read())
                    {

                        DonThuInfo TDKDInfo = GetDataForShow(dr);
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
        #endregion

        #region -- GetDataForInSo
        private DonThuInfo GetDataForInSo(SqlDataReader dr)
        {
            DonThuInfo DTInfo = new DonThuInfo();
            DTInfo.DonThuID = Utils.GetInt32(dr["DonThuID"], 0);

            DTInfo.NgayNhapDon = Utils.GetDateTime(dr["NgayNhapDon"], Constant.DEFAULT_DATE);

            DTInfo.NoiDungDon = Utils.GetString(dr["NoiDungDon"], string.Empty);


            //DTInfo.HoTen = Utils.GetString(dr["HoTen"].ToString(), String.Empty);
            //DTInfo.DiaChiCT = Utils.GetString(dr["DiaChiCT"].ToString(), String.Empty);

            DTInfo.TenLoaiKhieuTo = Utils.GetString(dr["TenLoaiKhieuTo"].ToString(), String.Empty);
            //DTInfo.TenCanBo = Utils.GetString(dr["TenCanBo"], String.Empty);
            //DTInfo.TenCoQuan = Utils.GetString(dr["TenCoQuan"], String.Empty);
            //DTInfo.TenXa = Utils.GetString(dr["TenXa"], String.Empty);
            //DTInfo.TenHuyen = Utils.GetString(dr["TenHuyen"], String.Empty);
            //DTInfo.TenTinh = Utils.GetString(dr["TenTinh"], String.Empty);
            //DTInfo.SoLan = Utils.GetInt32(dr["SoLan"], 0);
            //DTInfo.TrangThaiDonID = Utils.GetInt32(dr["TrangThaiDonID"], 0);
            //DTInfo.CoQuanID = Utils.GetInt32(dr["CoQuanID"], 0);
            //DTInfo.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);
            //DTInfo.SoLuong = Utils.GetInt32(dr["SoLuong"], 0);
            //DTInfo.CMND = Utils.GetString(dr["CMND"], String.Empty);
            //DTInfo.TenLoaiDoiTuong = Utils.GetString(dr["TenLoaiDoiTuong"], String.Empty);
            //DTInfo.SoDonThu = Utils.GetInt32(dr["SoDonThu"], 0);
            //DTInfo.NhomKNID = Utils.GetInt32(dr["NhomKNID"], 0);

            return DTInfo;
        }
        #endregion

        #region -- get don thu chua dong bo
        public IList<DonThuInfo> GetDonThuChuaDongBoBySearch(string keyword, int lktID, DateTime tuNgay, DateTime denNgay, int coquanID, int start, int end, int huyenID = 0)
        {

            IList<DonThuInfo> ls_donthu = new List<DonThuInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_CO_QUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM__LOAI_KHIEU_TO_ID, SqlDbType.Int),
                new SqlParameter(PARAM_KEYWORD, SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_START, SqlDbType.Int),
                new SqlParameter(PARAM_END, SqlDbType.Int),
                new SqlParameter(PARAM_HUYENID, SqlDbType.Int)
                
                //new SqlParameter(PARAM_CMND,SqlDbType.VarChar)
            };

            parameters[0].Value = coquanID;
            parameters[1].Value = tuNgay;
            parameters[2].Value = denNgay;
            parameters[3].Value = lktID;
            parameters[4].Value = keyword;
            parameters[5].Value = start;
            parameters[6].Value = end;
            parameters[7].Value = huyenID;

            //if (tuNgay == DateTime.MinValue) parameters[1].Value = DBNull.Value;
            if (denNgay == DateTime.MinValue) parameters[2].Value = DateTime.Now;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, NV_DonThu_ChuaDongBo_GetBySearch, parameters))
                {
                    while (dr.Read())
                    {
                        DonThuInfo DTInfo = GetDataForShow(dr);
                        DTInfo.CQDaGiaiQuyet = Utils.ConvertToString(dr["CQDaGiaiQuyetID"], string.Empty);
                        //DTInfo.NgayQuaHanGQ = Utils.ConvertToDateTime(dr["HanGiaiQuyet"], DateTime.MinValue);
                        DTInfo.NgayQuaHanGQ = Utils.GetDateTime(dr["HanGiaiQuyet"], Constant.DEFAULT_DATE);
                        DTInfo.TenNguonDonDen = Utils.ConvertToString(dr["TenNguonDonDen"], string.Empty);
                        DTInfo.TenHuongGiaiQuyet = Utils.ConvertToString(dr["HuongXuLy"], string.Empty);
                        ls_donthu.Add(DTInfo);
                    }
                }
            }
            catch
            {

                throw;
            }
            return ls_donthu;
        }
        #endregion

        #region -- getby search don thu da dong bo
        public IList<DonThuInfo> GetDonThuDaDongBoBySearch(string keyword, int lktID, DateTime tuNgay, DateTime denNgay, int coquanID, int start, int end, int huyenID = 0)
        {

            IList<DonThuInfo> ls_donthu = new List<DonThuInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_CO_QUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM__LOAI_KHIEU_TO_ID, SqlDbType.Int),
                new SqlParameter(PARAM_KEYWORD, SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_START, SqlDbType.Int),
                new SqlParameter(PARAM_END, SqlDbType.Int),
                new SqlParameter(PARAM_HUYENID, SqlDbType.Int)
                
                //new SqlParameter(PARAM_CMND,SqlDbType.VarChar)
            };

            parameters[0].Value = coquanID;
            parameters[1].Value = tuNgay;
            parameters[2].Value = denNgay;
            parameters[3].Value = lktID;
            parameters[4].Value = keyword;
            parameters[5].Value = start;
            parameters[6].Value = end;
            parameters[7].Value = huyenID;

            //if (tuNgay == DateTime.MinValue) parameters[1].Value = DBNull.Value;
            if (denNgay == DateTime.MinValue) parameters[2].Value = DateTime.Now;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, NV_DonThu_DaDongBo_GetBySearch, parameters))
                {
                    while (dr.Read())
                    {
                        DonThuInfo DTInfo = GetDataForShow(dr);
                        DTInfo.CQDaGiaiQuyet = Utils.ConvertToString(dr["CQDaGiaiQuyetID"], string.Empty);
                        //DTInfo.NgayQuaHanGQ = Utils.ConvertToDateTime(dr["HanGiaiQuyet"], DateTime.MinValue);
                        DTInfo.NgayQuaHanGQ = Utils.GetDateTime(dr["HanGiaiQuyet"], Constant.DEFAULT_DATE);
                        DTInfo.TenNguonDonDen = Utils.ConvertToString(dr["TenNguonDonDen"], string.Empty);
                        DTInfo.TenHuongGiaiQuyet = Utils.ConvertToString(dr["HuongXuLy"], string.Empty);
                        DTInfo.TrangThaiDongBo = Utils.ConvertToInt32(dr["TrangThaiDongBo"], 0);
                        ls_donthu.Add(DTInfo);
                    }
                }
            }
            catch
            {

                throw;
            }
            return ls_donthu;
        }
        #endregion

        #region -- for quan ly don thu
        public IList<DonThuInfo> GetBySearchHSDT(ref int TotalRow, string keyword, int lktID, DateTime tuNgay, DateTime denNgay, int coquanID, int start, int end, int? TrangThai, int huyenID = 0)
        {

            IList<DonThuInfo> ls_donthu = new List<DonThuInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_CO_QUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM__LOAI_KHIEU_TO_ID, SqlDbType.Int),
                new SqlParameter(PARAM_KEYWORD, SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_START, SqlDbType.Int),
                new SqlParameter(PARAM_END, SqlDbType.Int),
                new SqlParameter(PARAM_HUYENID, SqlDbType.Int),
                new SqlParameter("@TotalRow",SqlDbType.Int),
                new SqlParameter("@TrangThai",SqlDbType.Int),
                
                //new SqlParameter(PARAM_CMND,SqlDbType.VarChar)
            };

            parameters[0].Value = coquanID;
            parameters[1].Value = tuNgay;
            parameters[2].Value = denNgay;
            parameters[3].Value = lktID;
            parameters[4].Value = keyword ?? "";
            parameters[5].Value = start;
            parameters[6].Value = end;
            parameters[7].Value = huyenID;
            parameters[8].Direction = ParameterDirection.Output;
            parameters[8].Size = 8;
            parameters[9].Value = TrangThai ?? Convert.DBNull;

            if (tuNgay == DateTime.MinValue) parameters[1].Value = DBNull.Value;
            if (denNgay == DateTime.MinValue) parameters[2].Value = DBNull.Value;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "NV_DonThu_GetDataForHoSoDonThu_2", parameters))
                {
                    while (dr.Read())
                    {
                        DonThuInfo DTInfo = new DonThuInfo();//GetDataForShow(dr);
                        DTInfo.DonThuID = Utils.GetInt32(dr["DonThuID"], 0);
                        // lấy thêm xử lý đơn ID
                        DTInfo.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);
                        DTInfo.NgayNhapDon = Utils.GetDateTime(dr["NgayNhapDon"], Constant.DEFAULT_DATE);

                        DTInfo.NoiDungDon = Utils.GetString(dr["NoiDungDon"], string.Empty);

                        if (DTInfo.NoiDungDon.Length > Constant.LengthNoiDungDon)
                        {
                            DTInfo.NoiDungDon = DTInfo.NoiDungDon.Substring(0, Constant.LengthNoiDungDon) + Constant.ChuoiCuoiNDDon;
                        }
                        DTInfo.HoTen = Utils.GetString(dr["HoTen"].ToString(), String.Empty);
                        //DTInfo.DiaChiCT = Utils.GetString(dr["DiaChiCT"].ToString(), String.Empty);

                        DTInfo.TenLoaiKhieuTo = Utils.GetString(dr["TenLoaiKhieuTo"].ToString(), String.Empty);
                        DTInfo.TenCanBo = Utils.GetString(dr["TenCanBo"], String.Empty);
                        DTInfo.TenCoQuan = Utils.GetString(dr["TenCoQuan"], String.Empty);
                        DTInfo.TenXa = Utils.GetString(dr["TenXa"], String.Empty);
                        DTInfo.TenHuyen = Utils.GetString(dr["TenHuyen"], String.Empty);
                        DTInfo.TenTinh = Utils.GetString(dr["TenTinh"], String.Empty);
                        DTInfo.SoLan = Utils.GetInt32(dr["SoLan"], 0);
                        DTInfo.TrangThaiDonID = Utils.GetInt32(dr["TrangThaiDonID"], 0);
                        DTInfo.CoQuanID = Utils.GetInt32(dr["CoQuanID"], 0);
                        DTInfo.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);
                        DTInfo.SoLuong = Utils.GetInt32(dr["SoLuong"], 0);
                        DTInfo.TenLoaiDoiTuong = Utils.GetString(dr["TenLoaiDoiTuong"], String.Empty);
                        DTInfo.SoDonThu = Utils.GetString(dr["SoDonThu"], string.Empty);
                        DTInfo.NhomKNID = Utils.GetInt32(dr["NhomKNID"], 0);
                        DTInfo.CanBoTiepNhapID = Utils.GetInt32(dr["CanBoTiepNhapID"], 0);
                        DTInfo.CQDaGiaiQuyet = Utils.ConvertToString(dr["CQDaGiaiQuyetID"], string.Empty);
                        DTInfo.NgayQuaHanGQ = Utils.ConvertToDateTime(dr["HanGiaiQuyet"], DateTime.MinValue);
                        DTInfo.NgayQuaHanGQ = Utils.GetDateTime(dr["HanGiaiQuyet"], Constant.DEFAULT_DATE);
                        // sửa lại lấy tên Coquan Chuyển đơn 1/7/2024
                        var tenNguonDonDen = new DonThuDAL().GetByID(DTInfo.DonThuID, DTInfo.XuLyDonID).TenNguonDonDen;
                        if (tenNguonDonDen == null || tenNguonDonDen == "")
                        {
                            DTInfo.TenNguonDonDen = Utils.ConvertToString(dr["TenNguonDonDen"], string.Empty);
                        }
                        else
                        {
                            DTInfo.TenNguonDonDen = tenNguonDonDen + " chuyển đơn";
                        }
                        DTInfo.TenHuongGiaiQuyet = Utils.ConvertToString(dr["HuongXuLy"], string.Empty);
                        ls_donthu.Add(DTInfo);
                    }
                }
                TotalRow = Utils.ConvertToInt32(parameters[8].Value, 0);
            }
            catch (Exception ex)
            {

                throw;
            }
            return ls_donthu;
        }
        public int CountHSDT(string keyword, int lktID, DateTime tuNgay, DateTime denNgay, int coquanID, int huyenID = 0)
        {

            object result = 0;


            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_CO_QUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM__LOAI_KHIEU_TO_ID, SqlDbType.Int),
                new SqlParameter(PARAM_KEYWORD, SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_HUYENID, SqlDbType.Int),
                //new SqlParameter(PARAM_CMND,SqlDbType.VarChar)
            };

            parameters[0].Value = coquanID;
            parameters[1].Value = tuNgay;
            parameters[2].Value = denNgay;
            parameters[3].Value = lktID;
            parameters[4].Value = keyword ?? Convert.DBNull;
            parameters[5].Value = huyenID;

            if (tuNgay == DateTime.MinValue) parameters[1].Value = DBNull.Value;
            if (denNgay == DateTime.MinValue) parameters[2].Value = DBNull.Value;

            try
            {
                result = SQLHelper.ExecuteScalar(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_COUNT_HOSODONTHU, parameters);

            }
            catch (Exception ex)
            {

            }

            return Utils.ConvertToInt32(result, 0);


        }
        #endregion

        #region -- get ls don thu tiep nhan by coquanid, donthuid
        public IList<DonThuInfo> GetLsDonThuTiepNhanByDonThuID(int coquanID, int donThuID)
        {
            IList<DonThuInfo> ls_donthu = new List<DonThuInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_CO_QUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_DON_THU_ID,SqlDbType.Int)
            };

            parameters[0].Value = coquanID;
            parameters[1].Value = donThuID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_LSDONTHU_TIEPNHANBYDONTHUID, parameters))
                {
                    while (dr.Read())
                    {
                        //DonThuInfo DTInfo = GetDataForShow(dr);
                        DonThuInfo DTInfo = new DonThuInfo();
                        DTInfo.DonThuID = Utils.ConvertToInt32(dr["DonThuID"], 0);
                        DTInfo.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
                        DTInfo.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"], 0);
                        ls_donthu.Add(DTInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return ls_donthu;
        }
        #endregion
    }
}
