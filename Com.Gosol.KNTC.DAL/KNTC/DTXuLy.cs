using Com.Gosol.KNTC.DAL.DanhMuc;
using Com.Gosol.KNTC.Model.HeThong;
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
    public class DTXuLy
    {

        #region == cac bien su dung trong store proc sql ==

        //Su dung de goi StoreProcedure
        private const string GET_ALL = @"XuLyDon_GetAll";
        private const string GET_BY_ID = @"NV_XuLyDon_GetByID";
        private const string INSERT = @"XuLyDon_Insert";
        private const string UPDATE = @"XuLyDon_Update";
        private const string DELETE = @"XuLyDon_Delete";

        private const string GET_DTCANXULY_BY_PAGE = @"XuLyDon_GetByPage";
        private const string COUNT_ALL = @"XuLyDon_CountAll";
        // don thu can xu ly
        private const string COUNT_SEARCH = @"XuLyDon_CountDTCanXL";
        private const string SEARCH = @"XuLyDon_GetDTCanXL";
        // don thu can phan xu ly
        private const string COUNT_DTCANPHANXL_LANHDAO = @"XuLyDon_CountDTCanPhanXL_LanhDao";
        private const string DTCANPHANXL_LANHDAO = @"XuLyDon_GetDTCanPhanXL_LanhDao";

        private const string COUNT_DTCANPHANXL_TRUONGPHONG = @"XuLyDon_CountDTCanPhanXL_TruongPhong";
        private const string DTCANPHANXL_TRUONGPHONG = @"XuLyDon_GetDTCanPhanXL_TruongPhong";

        // don thu can duyet xu ly
        private const string XULYDON_GETDTDUYETXL_LANHDAO_GETALL_BYCOQUANID = @"XuLyDon_GetDTDuyetXL_LanhDao_GetAll_ByCoQuanID";
        private const string XULYDON_GETDTDUYETXL_LANHDAO_GETALL_BYCOQUANID_NEW = @"XuLyDon_GetDTDuyetXL_LanhDao_GetAll_ByCoQuanID_New";
        private const string XULYDON_GETDTDUYETXL_TRUONGPHONG_GETALLBYCOQUANID = @"XuLyDon_GetDTDuyetXL_TruongPhong_GetAllByCoQuanID";
        private const string XULYDON_GETDTDUYETXL_TRUONGPHONG_GETALLBYCOQUANID_NEW = @"XuLyDon_GetDTDuyetXL_TruongPhong_GetAllByCoQuanID_New";

        private const string XULYDON_COUNTDTDUYETXL_TRUONGPHONG_COUNTALLBYCOQUANID = @"XuLyDon_CountDTDuyetXL_TruongPhong_CountAllByCoQuanID";
        private const string XULYDON_COUNTDTDUYETXL_LANHDAO_COUNTALLBYCOQUANID = @"XuLyDon_CountDTDuyetXL_LanhDao_CountAllByCoQuanID";
        private const string XULYDON_COUNTDTDUYETXL_LANHDAO_COUNTALLBYCOQUANID_NEW = @"XuLyDon_CountDTDuyetXL_LanhDao_CountAllByCoQuanID_New";

        private const string COUNT_DTDUYETXL_LANHDAO = @"XuLyDon_CountDTDuyetXL_LanhDao";
        private const string GET_DTDUYETXL_LANHDAO = @"XuLyDon_GetDTDuyetXL_LanhDao";
        private const string COUNT_DTDUYETXL_LANHDAO_NEW = @"XuLyDon_CountDTDuyetXL_LanhDao_New";
        private const string GET_DTDUYETXL_LANHDAO_NEW = @"XuLyDon_GetDTDuyetXL_LanhDao_New";

        private const string COUNT_DTDUYETXL_TRUONGPHONG = @"XuLyDon_CountDTDuyetXL_TruongPhong";
        private const string GET_DTDUYETXL_TRUONGPHONG = @"XuLyDon_GetDTDuyetXL_TruongPhong";
        private const string GET_DTDUYETXL_TRUONGPHONG_NEW = @"XuLyDon_GetDTDuyetXL_TruongPhong_New";
        // don thu da tiep nhan
        private const string COUNT_DTDA_TIEPNHAN = @"XuLyDon_CountDTDaTiepNhan";
        private const string COUNT_SOTIEPNHAN_GIANTIEP = @"XuLyDon_CountSoTiepNhan_GianTiep";
        private const string COUNT_SOTIEPNHAN_GIANTIEP_BTDTINH = @"XuLyDon_CountSoTiepNhan_GianTiep_BTDTinh";
        private const string GET_DTDA_TIEPNHAN = @"XuLyDon_GetDTDaTiepNhan";
        private const string GET_SOTIEPNHAN_GIANTIEP = @"XuLyDon_GetSoTiepNhan_GianTiep";
        private const string GET_SOTIEPNHAN_GIANTIEP_BTDTINH = @"XuLyDon_GetSoTiepNhan_GianTiep_BTDTinh";
        private const string GET_INSOGIANTIEP_CANHAN = @"XuLyDon_GetSoTiepNhan_GianTiep_CaNhan";
        private const string GET_INSOTIEPNHAN_GIANTIEP_BTDTINH = @"XuLyDon_GetInSoTiepNhan_GianTiep_BTDTinh";
        private const string GET_IN_DTDA_TIEPNHAN = @"XuLyDon_GetInDTDaTiepNhan";
        private const string GET_IN_DTDA_TIEPNHAN_CANHAN = @"XuLyDon_GetInDTDaTiepNhan_CaNhan";
        private const string GET_IN_DTDA_TIEPNHAN_NEW = @"XuLyDon_GetInDTDaTiepNhan_New";
        private const string GET_IN_DTDA_TIEPNHAN_CANHAN_NEW = @"XuLyDon_GetInDTDaTiepNhan_CaNhan_New";
        private const string GET_FILEDONTHUDATIEPNHAN_LIST = @"XuLyDon_GetFileDonThuDaTiepNhan_List";
        private const string BC_TH_SUDUNG_PM_COUNT_SODON_CHUYENDEN = @"XuLyDon_BCTinhHinhSDPM_CountDonThuChuyenDen";
        private const string COUNT_DONGIANTIEP = @"XuLyDon_BCTinhHinhSDPM_CountDonGianTiep";
        // don thu co quan khac chuyen den
        private const string COUNT_DT_CQKHAC_CD = @"XuLyDon_CountDTCQKhacCD";
        private const string GET_DT_CQKHAC_CD = @"XuLyDon_GetDTCQKhacCD";

        private const string GET_DON_THU_DA_DUOC_XU_LY = "XuLyDon_GetDonThuDaXuLy";
        private const string XuLyDon_BaoCao_TH_XuLyDon_KN_TC_DaXuLy = "XuLyDon_BaoCao_TH_XuLyDon_KN_TC_DaXuLy";

        // don thu vu viec phuc tap
        private const string GET_DT_LAVUVIECPHUCTAP = @"XuLyDon_GetDTLaVuViecPhucTap";
        private const string GET_DT_LAVUVIECDANGGIAOXACMINH = @"XuLyDon_GetDTDangGiaoXacMinh";
        private const string GET_DT_DANHDAULAVUVIECPHUCTAP = @"XuLyDon_GetDTDanhDauLaVuViecPhucTap";

        //Ten cac bien dau vao
        private const string PARAM_XULYDON_ID = "@XuLyDonID";
        private const string PARAM_NGAYNHAPDON = "@NgayNhapDon";
        private const string PARAM_LOAIKHIEUTOID = "@LoaiKhieuToID";
        private const string PARAM_CQCHUYENDONDENID = "@CQChuyenDonDenID";

        private const string PARAM_KEY = "@Keyword";
        private const string PARAM_START = "@Start";
        private const string PARAM_END = "@End";

        private const string PARAM_TUNGAY = "@TuNgay";
        private const string PARAM_DENNGAY = "@DenNgay";
        private const string PARAM_DOCUMENTLIST = "@DocumentIDList";
        private const string PARAM_COQUANID = "@CoQuanID";
        private const string PARAM_STATENAME = "@StateName";
        private const string PARAM_CANBOID = "@CanBoID";
        private const string PARAM_DONTHUID = "@DonThuID";
        private const string PARAM_XULYDONID = "@XuLyDonID";
        private const string PARAM_PHONGBANID = "@PhongBanID";


        #endregion

        #region == cac ham dung chung ==

        // get and set para
        public SqlParameter[] GetPara_Count()
        {
            //SqlParameter para = new SqlParameter(PARAM_DOCUMENTLIST, SqlDbType.Structured);
            //para.TypeName = "IntList";

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_KEY, SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_CANBOID,SqlDbType.Int),
                //para
            }; return parms;
        }

        public SqlParameter[] GetPara_Search()
        {
            SqlParameter para = new SqlParameter(PARAM_DOCUMENTLIST, SqlDbType.Structured);
            para.TypeName = "IntList";

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_KEY, SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_START, SqlDbType.Int),
                new SqlParameter(PARAM_END, SqlDbType.Int),
                new SqlParameter(PARAM_CANBOID,SqlDbType.Int),
                para
            }; return parms;
        }

        public void SetPara_Count(SqlParameter[] parms, QueryFilterInfo info)
        {
            //var dataTable = new DataTable();
            //dataTable.Columns.Add("n", typeof(int));

            //foreach (var docID in documentIDlist)
            //{
            //    dataTable.Rows.Add(docID);
            //}

            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.KeyWord;
            parms[2].Value = info.LoaiKhieuToID;
            parms[3].Value = info.TuNgay;
            parms[4].Value = info.DenNgay;
            parms[5].Value = info.CanBoID;
            //parms[6].Value = dataTable;

            if (info.TuNgay == DateTime.MinValue) parms[3].Value = DBNull.Value;
            if (info.DenNgay == DateTime.MinValue) parms[4].Value = DBNull.Value;
        }

        public void SetPara_Search(SqlParameter[] parms, QueryFilterInfo info, List<int> documentIDlist)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("n", typeof(int));

            foreach (var docID in documentIDlist)
            {
                dataTable.Rows.Add(docID);
            }

            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.KeyWord;
            parms[2].Value = info.LoaiKhieuToID;
            parms[3].Value = info.TuNgay;
            parms[4].Value = info.DenNgay;
            parms[5].Value = info.Start;
            parms[6].Value = info.End;
            parms[7].Value = info.CanBoID;

            parms[8].Value = dataTable;

            if (info.TuNgay == DateTime.MinValue) parms[3].Value = DBNull.Value;
            if (info.DenNgay == DateTime.MinValue) parms[4].Value = DBNull.Value;
        }


        private DTXuLyInfo GetData(SqlDataReader dr)
        {
            DTXuLyInfo DTXLInfo = new DTXuLyInfo();

            // xu ly don
            DTXLInfo.DonThuID = Utils.GetInt32(dr["DonThuID"], 0);
            DTXLInfo.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);
            DTXLInfo.SoDonThu = Utils.GetString(dr["SoDonThu"], string.Empty);
            DTXLInfo.NguonDonDen = Utils.GetInt32(dr["TenNguonDonDen"], 0);
            DTXLInfo.TenChuDon = Utils.GetString(dr["HoTen"], string.Empty);
            DTXLInfo.NoiDungDon = Utils.GetString(dr["NoiDungDon"], string.Empty);

            if (dr["ModifiedDate"] != null)
                DTXLInfo.ModifiedDate = Utils.GetDateTime(dr["ModifiedDate"], DateTime.MinValue);

            // don thu can duyet xu ly
            DTXLInfo.HuongXuLy = Utils.GetString(dr["TenHuongGiaiQuyet"], string.Empty);
            //DTXLInfo.NgayGui = Utils.GetString(DTXLInfo.ModifiedDate.ToString("dd/MM/yyyy"), string.Empty);
            DTXLInfo.TenCBXuLy = Utils.GetString(dr["TenCanBo"], string.Empty);

            return DTXLInfo;
        }

        private DTXuLyInfo GetData_DTCanXL(SqlDataReader dr)
        {
            DTXuLyInfo DTXLInfo = new DTXuLyInfo();

            // xu ly don
            DTXLInfo.DonThuID = Utils.GetInt32(dr["DonThuID"], 0);
            DTXLInfo.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);
            DTXLInfo.SoDonThu = Utils.GetString(dr["SoDonThu"], string.Empty);
            DTXLInfo.TenChuDon = Utils.GetString(dr["HoTen"], string.Empty);
            DTXLInfo.NoiDungDon = Utils.GetString(dr["NoiDungDon"], string.Empty);

            DTXLInfo.HuongGiaiQuyetID = Utils.GetInt32(dr["HuongGiaiQuyetID"], 0);

            if (dr["HanXuLyDueDate"] != null)
                DTXLInfo.DueDate = Utils.GetDateTime(dr["HanXuLyDueDate"], DateTime.MinValue);
            if (dr["ModifiedDate"] != null)
                DTXLInfo.ModifiedDate = Utils.GetDateTime(dr["ModifiedDate"], DateTime.MinValue);

            // don thu can xu ly
            DTXLInfo.TenLoaiKhieuTo = Utils.GetString(dr["TenLoaiKhieuTo"], string.Empty);
            DTXLInfo.HanXuLy = Utils.GetString(DTXLInfo.DueDate.ToString("dd/MM/yyyy"), string.Empty);
            DTXLInfo.NgayGiao = Utils.GetString(DTXLInfo.ModifiedDate.ToString("dd/MM/yyyy"), string.Empty);
            DTXLInfo.NguoiGiao = Utils.GetString(dr["TenCanBo"], string.Empty);

            return DTXLInfo;
        }

        private DTXuLyInfo GetDataDTPhanXuLy(SqlDataReader dr)
        {
            DTXuLyInfo info = new DTXuLyInfo();

            info.DonThuID = Utils.GetInt32(dr["DonThuID"], 0);
            info.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);
            info.SoDonThu = Utils.GetString(dr["SoDonThu"], string.Empty);
            info.NguonDonDen = Utils.GetInt32(dr["NguonDonDen"], 0);
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
            if (info.NguonDonDen == (int)EnumNguonDonDen.TraoTay)
            {
                info.NguonDonDens = Constant.NguonDon_CoQuanKhacs;
            }
            info.TenChuDon = Utils.GetString(dr["HoTen"], string.Empty);
            info.NoiDungDon = Utils.GetString(dr["NoiDungDon"], string.Empty);
            info.NgayNhapDon = Utils.ConvertToDateTime(dr["NgayNhapDon"], DateTime.MinValue);
            info.NgayNhapDons = "";
            if (info.NgayNhapDon != DateTime.MinValue)
            {
                info.NgayNhapDons = info.NgayNhapDon.ToString("dd/MM/yyyy");
            }
            info.TenLoaiKhieuTo = Utils.ConvertToString(dr["TenLoaiKhieuTo"], string.Empty);
            info.TenCBTiepNhan = Utils.ConvertToString(dr["TenCanBo"], string.Empty);

            return info;
        }
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
            // Sửa lấy ra tên cơ quan chuyển đến 1/7/2024
            var tenNguonDonDen = new DonThuDAL().GetByID(info.DonThuID, info.XuLyDonID).TenNguonDonDen;
            if (info.NguonDonDen == (int)EnumNguonDonDen.CoQuanKhac)
            {
                if (string.IsNullOrEmpty(tenNguonDonDen))
                {
                    info.NguonDonDens = Constant.NguonDon_CoQuanKhacs;
                }
                else
                {
                    info.NguonDonDens = tenNguonDonDen + " chuyển đơn";
                }
            }
            if (info.NguonDonDen == (int)EnumNguonDonDen.BuuChinh)
            {
                info.NguonDonDens = Constant.NguonDon_BuuChinhs;
            }
            if (info.NguonDonDen == (int)EnumNguonDonDen.TraoTay)
            {
                info.NguonDonDens = Constant.NguonDon_CoQuanKhacs;
            }
            info.TenChuDon = Utils.GetString(dr["HoTen"], string.Empty);
            info.NoiDungDon = Utils.GetString(dr["NoiDungDon"], string.Empty);

            info.NgayGui = Utils.GetDateTime(dr["ModifiedDate"], DateTime.MinValue);
            info.NgayGuis = string.Empty; ;
            info.NgayGuis = Format.FormatDate(info.NgayGui);
            info.TenLoaiKhieuTo = Utils.ConvertToString(dr["TenLoaiKhieuTo"], string.Empty);
            info.TenCBXuLy = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
            info.HuongXuLy = Utils.ConvertToString(dr["TenHuongGiaiQuyet"], string.Empty);
            info.PhongBanID = Utils.ConvertToInt32(dr["PhongBanID"], 0);
            if (dr["CanBoID"] != null)
                info.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);

            return info;
        }
        private DTXuLyInfo GetDataDTDaTiepNhan(SqlDataReader dr)
        {
            DTXuLyInfo info = new DTXuLyInfo();

            info.DonThuID = Utils.GetInt32(dr["DonThuID"], 0);
            info.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);
            info.SoDonThu = Utils.GetString(dr["SoDonThu"], string.Empty);
            info.NguonDonDen = Utils.GetInt32(dr["NguonDonDen"], 0);
            info.StateName = Utils.GetString(dr["StateName"], String.Empty);

            info.MaHoSoMotCua = Utils.ConvertToString(dr["MaHoSoMotCua"], String.Empty);
            info.SoBienNhanMotCua = Utils.ConvertToString(dr["SoBienNhanMotCua"], String.Empty);
            var tenNguonDonDen = new DonThuDAL().GetByID(info.DonThuID, info.XuLyDonID).TenNguonDonDen;


            if (info.NguonDonDen == (int)EnumNguonDonDen.TrucTiep)
            {
                info.NguonDonDens = Constant.NguonDon_TrucTieps;
            }
            if (info.NguonDonDen == (int)EnumNguonDonDen.CoQuanKhac)
            {
                // Sửa lại để lấy cơ quan chuyển đơn 1/7/2024
                if (tenNguonDonDen == "" || tenNguonDonDen == null)
                {
                    info.NguonDonDens = Constant.NguonDon_CoQuanKhacs;
                }
                else
                {
                    info.NguonDonDens = tenNguonDonDen + " chuyển đơn";
                }
                if (info.MaHoSoMotCua != string.Empty)
                {
                    info.NguonDonDens = "Liên thông một cửa";
                }
            }
            if (info.NguonDonDen == (int)EnumNguonDonDen.BuuChinh)
            {
                info.NguonDonDens = Constant.NguonDon_BuuChinhs;
            }
            if (info.NguonDonDen == (int)EnumNguonDonDen.TraoTay)
            {
                info.NguonDonDens = Constant.NguonDon_TraoTays;
            }


            info.TenChuDon = Utils.GetString(dr["HoTen"], string.Empty);
            info.NoiDungDon = Utils.GetString(dr["NoiDungDon"], string.Empty);
            if (info.NoiDungDon.Length > Constant.LengthNoiDungDon)
            {
                info.NoiDungDon = info.NoiDungDon.Substring(0, Constant.LengthNoiDungDon);
            }
            info.NgayNhapDon = Utils.GetDateTime(dr["NgayNhapDon"], DateTime.MinValue);
            info.NgayNhapDons = "";
            if (info.NgayNhapDon != DateTime.MinValue)
            {
                info.NgayNhapDons = info.NgayNhapDon.ToString("dd/MM/yyyy");
            }
            info.DiaChiCT = Utils.ConvertToString(dr["DiaChiCT"], string.Empty);
            info.TenLoaiKhieuTo = Utils.ConvertToString(dr["TenLoaiKhieuTo"], string.Empty);
            info.HuongGiaiQuyetID = Utils.ConvertToInt32(dr["HuongGiaiQuyetID"], 0);

            return info;
        }
        #endregion
        #region == dieu kien loc don thu can phan xu ly==
        #region== Lanh dao ==
        public SqlParameter[] GetPara_Count_LanhDao()
        {
            SqlParameter para = new SqlParameter(PARAM_DOCUMENTLIST, SqlDbType.Structured);
            para.TypeName = "IntList";

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_KEY, SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                //para
            }; return parms;
        }

        public SqlParameter[] GetPara_Search_LanhDao()
        {
            //SqlParameter para = new SqlParameter(PARAM_DOCUMENTLIST, SqlDbType.Structured);
            // para.TypeName = "IntList";

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_KEY, SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_START, SqlDbType.Int),
                new SqlParameter(PARAM_END, SqlDbType.Int),
                //para
            }; return parms;
        }

        public void SetPara_Count_LanhDao(SqlParameter[] parms, QueryFilterInfo info, List<int> documentIDlist)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("n", typeof(int));

            foreach (var docID in documentIDlist)
            {
                dataTable.Rows.Add(docID);
            }

            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.KeyWord;
            parms[2].Value = info.LoaiKhieuToID;
            parms[3].Value = info.TuNgay;
            parms[4].Value = info.DenNgay;
            //parms[5].Value = dataTable;

            if (info.TuNgay == DateTime.MinValue) parms[3].Value = DBNull.Value;
            if (info.DenNgay == DateTime.MinValue) parms[4].Value = DBNull.Value;
        }

        public SqlParameter[] GetPara_Count_LanhDao_PhanXL()
        {

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_KEY, SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
            }; return parms;
        }
        public void SetPara_Count_LanhDao_PhanXL(SqlParameter[] parms, QueryFilterInfo info)
        {
            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.KeyWord;
            parms[2].Value = info.LoaiKhieuToID;
            parms[3].Value = info.TuNgay;
            parms[4].Value = info.DenNgay;

            if (info.TuNgay == DateTime.MinValue) parms[3].Value = DBNull.Value;
            if (info.DenNgay == DateTime.MinValue) parms[4].Value = DBNull.Value;
        }
        public void SetPara_Search_LanhDao(SqlParameter[] parms, QueryFilterInfo info, List<int> documentIDlist)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("n", typeof(int));

            foreach (var docID in documentIDlist)
            {
                dataTable.Rows.Add(docID);
            }

            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.KeyWord;
            parms[2].Value = info.LoaiKhieuToID;
            parms[3].Value = info.TuNgay;
            parms[4].Value = info.DenNgay;
            parms[5].Value = info.Start;
            parms[6].Value = info.End;

            //parms[7].Value = dataTable;

            if (info.TuNgay == DateTime.MinValue) parms[3].Value = DBNull.Value;
            if (info.DenNgay == DateTime.MinValue) parms[4].Value = DBNull.Value;
        }
        #endregion
        #region == truong phong ==
        public SqlParameter[] GetPara_Count_TruongPhong()
        {
            SqlParameter para = new SqlParameter(PARAM_DOCUMENTLIST, SqlDbType.Structured);
            para.TypeName = "IntList";

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_KEY, SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_PHONGBANID,SqlDbType.Int),
               // para
            }; return parms;
        }

        public SqlParameter[] GetPara_Search_TruongPhong()
        {
            SqlParameter para = new SqlParameter(PARAM_DOCUMENTLIST, SqlDbType.Structured);
            para.TypeName = "IntList";

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_KEY, SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_START, SqlDbType.Int),
                new SqlParameter(PARAM_END, SqlDbType.Int),
                new SqlParameter(PARAM_PHONGBANID,SqlDbType.Int),
               // para
            }; return parms;
        }

        public void SetPara_Count_TruongPhong(SqlParameter[] parms, QueryFilterInfo info, List<int> documentIDlist)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("n", typeof(int));

            foreach (var docID in documentIDlist)
            {
                dataTable.Rows.Add(docID);
            }

            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.KeyWord;
            parms[2].Value = info.LoaiKhieuToID;
            parms[3].Value = info.TuNgay;
            parms[4].Value = info.DenNgay;
            parms[5].Value = info.PhongBanID;
            // parms[6].Value = dataTable;

            if (info.TuNgay == DateTime.MinValue) parms[3].Value = DBNull.Value;
            if (info.DenNgay == DateTime.MinValue) parms[4].Value = DBNull.Value;
        }

        public SqlParameter[] GetPara_Count_TruongPhong_PhanXL()
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_KEY, SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_PHONGBANID,SqlDbType.Int),
            }; return parms;
        }

        public void SetPara_Count_TruongPhong_PhanXL(SqlParameter[] parms, QueryFilterInfo info)
        {
            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.KeyWord;
            parms[2].Value = info.LoaiKhieuToID;
            parms[3].Value = info.TuNgay;
            parms[4].Value = info.DenNgay;
            parms[5].Value = info.PhongBanID;
            if (info.TuNgay == DateTime.MinValue) parms[3].Value = DBNull.Value;
            if (info.DenNgay == DateTime.MinValue) parms[4].Value = DBNull.Value;
        }

        public void SetPara_Search_TruongPhong(SqlParameter[] parms, QueryFilterInfo info, List<int> documentIDlist)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("n", typeof(int));

            foreach (var docID in documentIDlist)
            {
                dataTable.Rows.Add(docID);
            }

            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.KeyWord;
            parms[2].Value = info.LoaiKhieuToID;
            parms[3].Value = info.TuNgay;
            parms[4].Value = info.DenNgay;
            parms[5].Value = info.Start;
            parms[6].Value = info.End;
            parms[7].Value = info.PhongBanID;

            //parms[8].Value = dataTable;

            if (info.TuNgay == DateTime.MinValue) parms[3].Value = DBNull.Value;
            if (info.DenNgay == DateTime.MinValue) parms[4].Value = DBNull.Value;
        }

        #endregion
        #endregion
        #region == ==

        //private SqlParameter[] GetInsertParms()
        //{
        //    SqlParameter[] parms = new SqlParameter[]{
        //        //new SqlParameter(PARAM_TEN_NGUON_DON_DEN, SqlDbType.NVarChar, 50),

        //        };
        //    return parms;
        //}

        //private void SetInsertParms(SqlParameter[] parms, DTXuLyInfo NDNInfo)
        //{

        //    parms[0].Value = NDNInfo.TenNguonDonDen;

        //}

        //private SqlParameter[] GetUpdateParms()
        //{
        //    SqlParameter[] parms = new SqlParameter[]{

        //        //new SqlParameter(PARAM_NGUON_DON_DEN_ID,SqlDbType.Int),
        //        //new SqlParameter(PARAM_TEN_NGUON_DON_DEN,SqlDbType.NVarChar,50)

        //    };
        //    return parms;
        //}

        //private void SetUpdateParms(SqlParameter[] parms, DTXuLyInfo NDNInfo)
        //{

        //    parms[0].Value = NDNInfo.NguonDonDenID;
        //    parms[1].Value = NDNInfo.TenNguonDonDen;

        //}

        public IList<DTXuLyInfo> GetAll()
        {
            IList<DTXuLyInfo> LsNDN = new List<DTXuLyInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_ALL, null))
                {

                    while (dr.Read())
                    {

                        DTXuLyInfo NDNInfo = GetData(dr);
                        LsNDN.Add(NDNInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return LsNDN;
        }

        public DTXuLyInfo GetByID(int NDNID)
        {

            DTXuLyInfo DTXLInfo = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDON_ID,SqlDbType.Int)
            };
            parameters[0].Value = NDNID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_NV_XuLyDon_GetByID", parameters))
                {

                    if (dr.Read())
                    {
                        DTXLInfo = new DTXuLyInfo();
                        // xu ly don
                        DTXLInfo.DonThuID = Utils.GetInt32(dr["DonThuID"], 0);
                        DTXLInfo.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);
                        DTXLInfo.HuongGiaiQuyetID = Utils.GetInt32(dr["HuongGiaiQuyetID"], 0);
                        DTXLInfo.NhomKNID = Utils.GetInt32(dr["NhomKNID"], 0);
                        DTXLInfo.SoDonThu = Utils.GetString(dr["SoDonThu"], string.Empty);
                        DTXLInfo.NguonDonDen = Utils.ConvertToNullableInt32(dr["NguonDonDen"], 0);
                        DTXLInfo.CQChuyenDonDenID = Utils.ConvertToNullableInt32(dr["CQChuyenDonDenID"], 0);
                        DTXLInfo.CQChuyenDonID = Utils.ConvertToInt32(dr["CQChuyenDonID"], 0);
                        DTXLInfo.ChuyenChoCoQuan = Utils.ConvertToString(dr["ChuyenChoCoQuan"], string.Empty);
                        //DTXLInfo.TenChuDon = Utils.GetString(dr["HoTen"], string.Empty);
                        DTXLInfo.SoDonThu = Utils.GetString(dr["SoDonThu"], string.Empty);
                        DTXLInfo.NoiDungHuongDan = Utils.GetString(dr["NoiDungHuongDan"], string.Empty);
                        DTXLInfo.NoiDungBanHanhXM = Utils.GetString(dr["NoiDungBanHanhXM"], string.Empty);
                        DTXLInfo.NoiDungBanHanhGQ = Utils.GetString(dr["NoiDungBanHanhGQ"], string.Empty);
                        //DTXLInfo.YKienXuLy = Utils.GetString(dr["YKienXuLy"], string.Empty);
                        DTXLInfo.LoaiKhieuTo1ID = Utils.GetInt32(dr["LoaiKhieuTo1ID"], 0);
                        //if (dr["ModifiedDate"] != null)
                        //    DTXLInfo.ModifiedDate = Utils.GetDateTime(dr["ModifiedDate"], DateTime.MinValue);

                        // don thu can duyet xu ly
                        DTXLInfo.HuongXuLy = Utils.GetString(dr["TenHuongGiaiQuyet"], string.Empty);
                        //DTXLInfo.TenCBXuLy = Utils.GetString(dr["TenCanBo"], string.Empty);
                        DTXLInfo.CoQuanID = Utils.GetInt32(dr["CoQuanID"], 0);
                        DTXLInfo.QuyTrinhXLD = Utils.GetInt32(dr["QuyTrinhXLD"], 0);
                        DTXLInfo.QuyTrinhGQ = Utils.GetInt32(dr["QuyTrinhGQ"], 0);
                        DTXLInfo.CQDaGiaiQuyetID = Utils.GetString(dr["CQDaGiaiQuyetID"], string.Empty);
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return DTXLInfo;
        }

        public int Delete(int NDN_ID)
        {

            int val = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                //new SqlParameter(PARAM_NGUON_DON_DEN_ID,SqlDbType.Int)
            };
            parameters[0].Value = NDN_ID;
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

        //public int Update(DTXuLyInfo NDNInfo)
        //{

        //    int val = 0;
        //    SqlParameter[] parameters = GetUpdateParms();
        //    SetUpdateParms(parameters, NDNInfo);

        //    using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
        //    {

        //        conn.Open();
        //        using (SqlTransaction trans = conn.BeginTransaction())
        //        {

        //            try
        //            {
        //                val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, UPDATE, parameters);
        //                trans.Commit();
        //            }
        //            catch
        //            {
        //                trans.Rollback();
        //                throw;
        //            }
        //        }
        //        conn.Close();
        //    }
        //    return val;
        //}

        //public int Insert(DTXuLyInfo NDNInfo)
        //{

        //    int val = 0;

        //    SqlParameter[] parameters = GetInsertParms();
        //    SetInsertParms(parameters, NDNInfo);

        //    using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
        //    {

        //        conn.Open();
        //        using (SqlTransaction trans = conn.BeginTransaction())
        //        {

        //            try
        //            {
        //                val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, INSERT, parameters);
        //                trans.Commit();
        //            }
        //            catch
        //            {
        //                trans.Rollback();
        //                throw;
        //            }
        //        }
        //        conn.Close();
        //    }
        //    return val;
        //}

        #endregion

        #region ==don thu can xu ly==
        public int CountAll()
        {
            int result = 0;
            // lay document = getdocment(state + role) tren workflow dc listDocument
            // dem so document trong listDocument dc CountNum

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_ALL, null))
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

        public int Count_DTCanXuLy(QueryFilterInfo info)
        {

            int result = 0;
            SqlParameter[] parms = GetPara_Count();
            SetPara_Count(parms, info);
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_SEARCH, parms))
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



        public IList<DTXuLyInfo> GetDTCanXuLy(QueryFilterInfo info, List<int> docList)
        {

            IList<DTXuLyInfo> listDTCanXL = new List<DTXuLyInfo>();
            SqlParameter[] parms = GetPara_Search();
            SetPara_Search(parms, info, docList);
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, SEARCH, parms))
                {
                    while (dr.Read())
                    {
                        DTXuLyInfo Info = GetData_DTCanXL(dr);
                        Info.TenHuongXuLy = Utils.ConvertToString(dr["TenHuongGiaiQuyet"], string.Empty);
                        Info.NguonDonDen = Utils.ConvertToInt32(dr["NguonDonDen"], 0);
                        Info.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        string now = Format.FormatDate(DateTime.Now);
                        DateTime ngayHienTai = Utils.ConvertToDateTime(now, DateTime.MinValue);
                        Info.HanXuLy = Format.FormatDate(Info.DueDate);
                        DateTime ngayQuaHan = Utils.GetDateTime(dr["HanXuLyDueDate"], DateTime.MinValue);
                        TimeSpan ngayConLai = ngayQuaHan.Subtract(ngayHienTai);
                        Info.NgayXuLyConLai = ngayConLai.Days;
                        Info.CBDuocChonXL = Utils.ConvertToInt32(dr["CBDuocChonXL"], 0);
                        Info.QTTiepNhanDon = Utils.ConvertToInt32(dr["QTTiepNhanDon"], 0);

                        listDTCanXL.Add(Info);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return listDTCanXL;
        }

        public IList<DTXuLyInfo> GetDTCanXuLy_New(QueryFilterInfo info, List<int> docList)
        {
            SqlParameter para = new SqlParameter(PARAM_DOCUMENTLIST, SqlDbType.Structured);
            para.TypeName = "IntList";
            IList<DTXuLyInfo> listDTCanXL = new List<DTXuLyInfo>();
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),


                new SqlParameter(PARAM_CANBOID,SqlDbType.Int),
               para
            };
            var dataTable = new DataTable();
            dataTable.Columns.Add("n", typeof(int));

            foreach (var docID in docList)
            {
                dataTable.Rows.Add(docID);
            }

            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.CanBoID;

            parms[2].Value = dataTable;

            //if (info.TuNgay == DateTime.MinValue) parms[3].Value = DBNull.Value;
            //if (info.DenNgay == DateTime.MinValue) parms[4].Value = DBNull.Value;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "XuLyDon_GetDTCanXL_New", parms))
                {
                    while (dr.Read())
                    {
                        DTXuLyInfo Info = GetData_DTCanXL(dr);
                        Info.TenHuongXuLy = Utils.ConvertToString(dr["TenHuongGiaiQuyet"], string.Empty);
                        Info.NguonDonDen = Utils.ConvertToInt32(dr["NguonDonDen"], 0);
                        Info.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        string now = Format.FormatDate(DateTime.Now);
                        DateTime ngayHienTai = Utils.ConvertToDateTime(now, DateTime.MinValue);
                        Info.HanXuLy = Format.FormatDate(Info.DueDate);
                        DateTime ngayQuaHan = Utils.GetDateTime(dr["HanXuLyDueDate"], DateTime.MinValue);
                        TimeSpan ngayConLai = ngayQuaHan.Subtract(ngayHienTai);
                        Info.NgayXuLyConLai = ngayConLai.Days;
                        Info.CBDuocChonXL = Utils.ConvertToInt32(dr["CBDuocChonXL"], 0);
                        Info.QTTiepNhanDon = Utils.ConvertToInt32(dr["QTTiepNhanDon"], 0);

                        listDTCanXL.Add(Info);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return listDTCanXL;
        }
        public IList<DTXuLyInfo> GetByPage(int page)
        {
            int start = (page - 1) * Constant.PageSize;
            int end = page * Constant.PageSize;

            IList<DTXuLyInfo> ls_nguondonden = new List<DTXuLyInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_START,SqlDbType.Int),
                new SqlParameter(PARAM_END,SqlDbType.Int)
            };
            parameters[0].Value = start;
            parameters[1].Value = end;
            // lay list tu workflow roi goi store proc getbypage(ko su dung start + end)
            // roi so sanh neu trung nhau thi them vao list roi gan them start, end

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DTCANXULY_BY_PAGE, parameters))
                {
                    while (dr.Read())
                    {
                        DTXuLyInfo NDNInfo = GetData(dr);
                        ls_nguondonden.Add(NDNInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return ls_nguondonden;
        }
        #endregion

        #region ==don thu can phan xu ly==

        public int Count_DTCanPhanXL_LanhDao(QueryFilterInfo info)
        {
            int result = 0;
            SqlParameter[] parms = GetPara_Count_LanhDao_PhanXL();
            SetPara_Count_LanhDao_PhanXL(parms, info);
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_DTCANPHANXL_LANHDAO, parms))
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

        public IList<DTXuLyInfo> DTCanPhanXL_LanhDao(ref int TotalRow, QueryFilterInfo info, List<int> docList)
        {

            IList<DTXuLyInfo> ListInfo = new List<DTXuLyInfo>();
            //SqlParameter[] parms = GetPara_Search_LanhDao();
            //SetPara_Search_LanhDao(parms, info, docList);

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_KEY, SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_START, SqlDbType.Int),
                new SqlParameter(PARAM_END, SqlDbType.Int),
                new SqlParameter("@TotalRow",SqlDbType.Int),
            };
            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.KeyWord;
            parms[2].Value = info.LoaiKhieuToID;
            parms[3].Value = info.TuNgay;
            parms[4].Value = info.DenNgay;
            parms[5].Value = info.Start;
            parms[6].Value = info.End;
            parms[7].Direction = ParameterDirection.Output;
            parms[7].Size = 8;

            if (info.TuNgay == DateTime.MinValue) parms[3].Value = DBNull.Value;
            if (info.DenNgay == DateTime.MinValue) parms[4].Value = DBNull.Value;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_XuLyDon_GetDTCanPhanXL_LanhDao", parms))
                {
                    while (dr.Read())
                    {

                        DTXuLyInfo Info = GetDataDTPhanXuLy(dr);
                        string now = Format.FormatDate(DateTime.Now);
                        DateTime ngayHienTai = Utils.ConvertToDateTime(now, DateTime.MinValue);
                        Info.PhanXuLyID = Utils.ConvertToInt32(dr["PhanXuLyID"], 0);
                        Info.TenCanBoXuLy = Utils.GetString(dr["TenCanBoXuLy"], String.Empty);
                        Info.StateName = Utils.GetString(dr["StateName"], String.Empty);
                        Info.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        if (Info.StateID == 1)
                        {
                            Info.HanXuLy = Format.FormatDate(Utils.ConvertToDateTime(dr["HanXuLyDueDate"], DateTime.MinValue));
                            DateTime ngayQuaHan = Utils.ConvertToDateTime(dr["HanXuLyDueDate"], DateTime.MinValue);
                            TimeSpan ngayConLai = ngayQuaHan.Subtract(ngayHienTai);
                            Info.NgayXLConLai = ngayConLai.Days;
                        }
                        else
                        {
                            Info.HanXuLy = Format.FormatDate(Utils.ConvertToDateTime(dr["HanXuLyDueDateDaPhan"], DateTime.MinValue));
                            DateTime ngayQuaHan = Utils.ConvertToDateTime(dr["HanXuLyDueDateDaPhan"], DateTime.MinValue);
                            TimeSpan ngayConLai = ngayQuaHan.Subtract(ngayHienTai);
                            Info.NgayXLConLai = ngayConLai.Days;
                        }
                        Info.TransitionID = Utils.ConvertToInt32(dr["TransitionID"], 0);
                        Info.HuongGiaiQuyetID = Utils.GetInt32(dr["HuongGiaiQuyetID"], 0);
                        Info.NhomKNID = Utils.GetInt32(dr["NhomKNID"], 0);
                        if (Info.NhomKNID > 0)
                        {
                            Info.listDoiTuongKN = new DoiTuongKN().GetByNhomKNID(Info.NhomKNID).ToList();
                        }
                        ListInfo.Add(Info);
                    }
                    dr.Close();
                }
                TotalRow = Utils.ConvertToInt32(parms[7].Value, 0);
            }
            catch
            {

                throw;
            }
            return ListInfo;
        }
        public int Count_DTCanPhanXL_TruongPhong(QueryFilterInfo info)
        {
            int result = 0;
            SqlParameter[] parms = GetPara_Count_TruongPhong_PhanXL();
            SetPara_Count_TruongPhong_PhanXL(parms, info);
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_DTCANPHANXL_TRUONGPHONG, parms))
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

        public IList<DTXuLyInfo> DTCanPhanXL_TruongPhong(ref int TotalRow, QueryFilterInfo info, List<int> docList)
        {

            IList<DTXuLyInfo> ListInfo = new List<DTXuLyInfo>();
            //SqlParameter[] parms = GetPara_Search_TruongPhong();
            //SetPara_Search_TruongPhong(parms, info, docList);

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_KEY, SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_START, SqlDbType.Int),
                new SqlParameter(PARAM_END, SqlDbType.Int),
                new SqlParameter(PARAM_PHONGBANID,SqlDbType.Int),
                new SqlParameter("@TotalRow",SqlDbType.Int),
            };

            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.KeyWord;
            parms[2].Value = info.LoaiKhieuToID;
            parms[3].Value = info.TuNgay;
            parms[4].Value = info.DenNgay;
            parms[5].Value = info.Start;
            parms[6].Value = info.End;
            parms[7].Value = info.PhongBanID;
            parms[8].Direction = ParameterDirection.Output;
            parms[8].Size = 8;

            //parms[8].Value = dataTable;

            if (info.TuNgay == DateTime.MinValue) parms[3].Value = DBNull.Value;
            if (info.DenNgay == DateTime.MinValue) parms[4].Value = DBNull.Value;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_XuLyDon_GetDTCanPhanXL_TruongPhong", parms))
                {
                    while (dr.Read())
                    {
                        DTXuLyInfo Info = GetDataDTPhanXuLy(dr);
                        Info.HanXuLy = Format.FormatDate(Utils.ConvertToDateTime(dr["HanXuLyDueDate"], DateTime.MinValue));
                        Info.StateName = Utils.GetString(dr["StateName"], String.Empty);
                        Info.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        Info.PhanXuLyID = Utils.ConvertToInt32(dr["PhanXuLyID"], 0);
                        Info.TenCanBoXuLy = Utils.GetString(dr["TenCanBoXuLy"], String.Empty);
                        string now = Format.FormatDate(DateTime.Now);
                        DateTime ngayHienTai = Utils.ConvertToDateTime(now, DateTime.MinValue);
                        DateTime ngayQuaHan = Utils.ConvertToDateTime(dr["HanXuLyDueDate"], DateTime.MinValue);

                        TimeSpan ngayConLai = ngayQuaHan.Subtract(ngayHienTai);
                        Info.NgayXLConLai = ngayConLai.Days;
                        Info.HuongGiaiQuyetID = Utils.GetInt32(dr["HuongGiaiQuyetID"], 0);
                        Info.NhomKNID = Utils.GetInt32(dr["NhomKNID"], 0);
                        if (Info.NhomKNID > 0)
                        {
                            Info.listDoiTuongKN = new DoiTuongKN().GetByNhomKNID(Info.NhomKNID).ToList();
                        }
                        ListInfo.Add(Info);
                    }
                    dr.Close();
                }
                TotalRow = Utils.ConvertToInt32(parms[8].Value, 0);
            }
            catch
            {

                throw;
            }
            return ListInfo;
        }

        #endregion

        #region == don thu da tiep nhan ==

        public DTXuLyInfo GetDataDonThuDaXuLy(SqlDataReader dr)
        {
            DTXuLyInfo info = new DTXuLyInfo();

            info.NgayXuLy = Utils.ConvertToDateTime(dr["NgayXuLy"], DateTime.MinValue);
            info.NgayQuaHan = Utils.ConvertToDateTime(dr["HanXuLy"], DateTime.MaxValue);
            info.CanBoXuLyID = Utils.ConvertToInt32(dr["CanBoXuLyID"], 0);

            return info;
        }

        public IList<DTXuLyInfo> GetDonThuDaXuLy(DateTime startDate, DateTime endDate, int coQuanID)
        {
            IList<DTXuLyInfo> ListInfo = new List<DTXuLyInfo>();

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
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
                        DTXuLyInfo info = GetDataDonThuDaXuLy(dr);
                        info.SoDon = Utils.ConvertToString(dr["SoDonThu"], string.Empty);
                        info.NgayTiep = Utils.ConvertToDateTime(dr["NgayNhapDon"], DateTime.MinValue);
                        //info.HoTen = Utils.ConvertToString(dr["HoTen"],string.Empty);
                        //info.DiaChi = Utils.ConvertToString(dr["DiaChiCT"], string.Empty);
                        //info.NoiDungDon = Utils.ConvertToString(dr["NoiDungDon"],string.Empty);
                        //info.TenLoaiKhieuTo = Utils.ConvertToString(dr["TenLoaiKhieuTo"], string.Empty);
                        if (info.NgayTiep != DateTime.MinValue)
                        {
                            info.NgayTiepStr = Format.FormatDate(info.NgayTiep);
                        }
                        ListInfo.Add(info);
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

        public IList<DTXuLyInfo> GetDTDaTiepNhan(QueryFilterInfo info, int canBoID, ref int TotalRow)
        {
            IList<DTXuLyInfo> ListInfo = new List<DTXuLyInfo>();

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_KEY, SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_START, SqlDbType.Int),
                new SqlParameter(PARAM_END, SqlDbType.Int),
                new SqlParameter(PARAM_CANBOID,SqlDbType.Int),
                new SqlParameter("@TotalRow",SqlDbType.Int),
                new SqlParameter("@TrangThai",SqlDbType.Int),
                new SqlParameter("@HuongXuLyID",SqlDbType.Int),
            };

            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.KeyWord;
            parms[2].Value = info.LoaiKhieuToID;
            parms[3].Value = info.TuNgay;
            parms[4].Value = info.DenNgay;
            parms[5].Value = info.Start;
            parms[6].Value = info.End;
            parms[7].Value = info.CanBoID;
            parms[8].Direction = ParameterDirection.Output;
            parms[8].Size = 8;
            parms[9].Value = info.TrangThai ?? Convert.DBNull;
            parms[10].Value = info.HuongXuLyID;

            if (info.CanBoID == 0) parms[7].Value = DBNull.Value;

            if (info.TuNgay == DateTime.MinValue) parms[3].Value = DBNull.Value;
            if (info.DenNgay == DateTime.MinValue) parms[4].Value = DBNull.Value;
            //SqlParameter[] parms = GetPara_Search();
            //SetPara_Search(parms, info, docList);
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_XuLyDon_GetDTDaTiepNhan", parms))
                {
                    while (dr.Read())
                    {
                        DTXuLyInfo Info = GetDataDTDaTiepNhan(dr);
                        Info.LoaiQuyTrinh = Utils.ConvertToInt32(dr["LoaiQuyTrinh"], 0);
                        Info.HuongGiaiQuyetID = Utils.ConvertToInt32(dr["HuongGiaiQuyetID"], 0);
                        Info.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        Info.TrangThaiDuyet = Utils.ConvertToInt32(dr["TrangThaiDuyet"], 0);
                        Info.TrinhDuThao = Utils.ConvertToInt32(dr["TrinhDuThao"], 0);
                        Info.ChuyenGiaiQuyetID = Utils.ConvertToInt32(dr["ChuyenGiaiQuyetID"], 0);
                        Info.NgayTiepNhan = Utils.GetDateTime(dr["NgayTiepNhan"], DateTime.MinValue);
                        Info.KetQuaID = Utils.ConvertToInt32(dr["KetQuaID"], 0);
                        // bổ sung cơ quan chuyển đến
                        info.CQChuyenDonDenID = Utils.ConvertToInt32(dr["CQChuyenDonDenID"], 0);

                        Info.NextStateID = Utils.ConvertToInt32(dr["NextStateID"], 0);

                        Info.CBDuocChonXL = Utils.ConvertToInt32(dr["CBDuocChonXL"], 0);
                        Info.QTTiepNhanDon = Utils.ConvertToInt32(dr["QTTiepNhanDon"], 0);
                        Info.CanBoTiepNhanID = Utils.ConvertToInt32(dr["CanBoTiepNhapID"], 0);
                        Info.HuongXuLy = Utils.ConvertToString(dr["TenHuongGiaiQuyet"], String.Empty);
                        Info.NgayHetHan = Utils.ConvertToNullableDateTime(dr["NgayHetHan"], null);
                        DateTime NgayTiep = Utils.GetDateTime(dr["NgayTiep"], DateTime.MinValue);
                        if (NgayTiep != DateTime.MinValue)
                        {
                            Info.NgayNhapDon = NgayTiep;
                        }

                        if (Info.NgayHetHan != null)
                        {
                            Info.HanXuLy = Info.NgayHetHan.Value.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            if (Info.NgayNhapDon != DateTime.MinValue)
                            {
                                Info.HanXuLy = Info.NgayNhapDon.AddDays(10).ToString("dd/MM/yyyy");
                            }
                        }
                        Info.TrangThaiDuyet = Utils.ConvertToInt32(dr["TrangThaiDuyet"], 0);

                        List<DTXuLyInfo> fileYKien = new List<DTXuLyInfo>();
                        Info.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"], 0);
                        if (Info.NhomKNID > 0)
                        {
                            Info.listDoiTuongKN = new DoiTuongKN().GetByNhomKNID(Info.NhomKNID).ToList();
                        }
                        //CanBoInfo canBoInfo = new CanBo().GetCanBoByID(canBoID);

                        //if (canBoInfo.XemTaiLieuMat)
                        //{
                        //    fileYKien = GetFileDonThuDaTiepNhan(Info.XuLyDonID).ToList();
                        //}
                        //else
                        //{
                        //    fileYKien = GetFileDonThuDaTiepNhan(Info.XuLyDonID).Where(x => x.IsBaoMat != true || x.NguoiUp == info.CanBoID).ToList();
                        //}

                        //int step = 0;
                        //for (int i = 0; i < fileYKien.Count; i++)
                        //{
                        //    if (!string.IsNullOrEmpty(fileYKien[i].FileUrl))
                        //    {
                        //        if (string.IsNullOrEmpty(fileYKien[i].TenFile))
                        //        {
                        //            string[] arrtenFile = fileYKien[i].FileUrl.Split('/');
                        //            if (arrtenFile.Length > 0)
                        //            {
                        //                string[] duoiFile = arrtenFile[arrtenFile.Length - 1].Split('_');
                        //                if (duoiFile.Length > 0)
                        //                {
                        //                    fileYKien[i].TenFile = duoiFile[duoiFile.Length - 1];
                        //                }
                        //                else
                        //                {
                        //                    fileYKien[i].TenFile = arrtenFile[arrtenFile.Length - 1];
                        //                }
                        //            }
                        //        }
                        //        fileYKien[i].FileUrl = fileYKien[i].FileUrl.Replace(" ", "%20");
                        //    }
                        //    step++;
                        //    if (fileYKien[i].IsBaoMat == false)
                        //    {
                        //        string sec_false = "<li style='margin-bottom: 5px;'>" + step + ". " + "<a href='" + fileYKien[i].FileUrl + "' download>" + fileYKien[i].TenFile + "</a></li>";
                        //        Info.HuongXuLy = "<div style='margin-bottom: 5px;'><span>" + Info.HuongXuLy + "</span></div>" + "<ul>" + sec_false + "</ul>";
                        //    }
                        //    else
                        //    {
                        //        string sec_true = "<li style='margin-bottom: 5px;'>" + step + ". " + "<a style='color: #EE7600;' href=" + "/DownloadFileDonThuCT.aspx?url=" + fileYKien[i].FileUrl + ">" + fileYKien[i].TenFile + "</a></li>";
                        //        Info.HuongXuLy = "<div style='margin-bottom: 5px;'><span>" + Info.HuongXuLy + "</span></div>" + "<ul>" + sec_true + "</ul>";
                        //    }
                        //}
                        // bổ sung trang thái rút đơn
                        Info.RutDonID = Utils.ConvertToInt32(dr["RutDonID"], 0);
                        ListInfo.Add(Info);
                    }
                    dr.Close();
                }
                TotalRow = Utils.ConvertToInt32(parms[8].Value, 0);
            }
            catch (Exception ex)
            {

                throw;
            }

            return ListInfo;
        }
        public IList<DTXuLyInfo> GetDTDaTiepNhan_New(QueryFilterInfo info, int? canBoID)
        {
            IList<DTXuLyInfo> ListInfo = new List<DTXuLyInfo>();

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_CANBOID,SqlDbType.Int)

            };

            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.CanBoID;

            if (info.CanBoID == 0) parms[7].Value = DBNull.Value;

            //if (info.TuNgay == DateTime.MinValue) parms[3].Value = DBNull.Value;
            //if (info.DenNgay == DateTime.MinValue) parms[4].Value = DBNull.Value;
            //SqlParameter[] parms = GetPara_Search();
            //SetPara_Search(parms, info, docList);
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "XuLyDon_GetDTDaTiepNhan_New", parms))
                {
                    while (dr.Read())
                    {
                        DTXuLyInfo Info = GetDataDTDaTiepNhan(dr);
                        Info.CBDuocChonXL = Utils.ConvertToInt32(dr["CBDuocChonXL"], 0);
                        Info.QTTiepNhanDon = Utils.ConvertToInt32(dr["QTTiepNhanDon"], 0);
                        Info.CanBoTiepNhanID = Utils.ConvertToInt32(dr["CanBoTiepNhapID"], 0);
                        Info.HuongXuLy = Utils.ConvertToString(dr["TenHuongGiaiQuyet"], String.Empty);
                        List<DTXuLyInfo> fileYKien = new List<DTXuLyInfo>();

                        CanBoInfo canBoInfo = new CanBo().GetCanBoByID(canBoID ?? 0);

                        if (canBoInfo.XemTaiLieuMat)
                        {
                            fileYKien = GetFileDonThuDaTiepNhan(Info.XuLyDonID).ToList();
                        }
                        else
                        {
                            fileYKien = GetFileDonThuDaTiepNhan(Info.XuLyDonID).Where(x => x.IsBaoMat != true || x.NguoiUp == info.CanBoID).ToList();
                        }

                        //int step = 0;
                        //for (int i = 0; i < fileYKien.Count; i++)
                        //{
                        //    if (!string.IsNullOrEmpty(fileYKien[i].FileUrl))
                        //    {
                        //        if (string.IsNullOrEmpty(fileYKien[i].TenFile))
                        //        {
                        //            string[] arrtenFile = fileYKien[i].FileUrl.Split('/');
                        //            if (arrtenFile.Length > 0)
                        //            {
                        //                string[] duoiFile = arrtenFile[arrtenFile.Length - 1].Split('_');
                        //                if (duoiFile.Length > 0)
                        //                {
                        //                    fileYKien[i].TenFile = duoiFile[duoiFile.Length - 1];
                        //                }
                        //                else
                        //                {
                        //                    fileYKien[i].TenFile = arrtenFile[arrtenFile.Length - 1];
                        //                }
                        //            }
                        //        }
                        //        fileYKien[i].FileUrl = fileYKien[i].FileUrl.Replace(" ", "%20");
                        //    }
                        //    step++;
                        //    if (fileYKien[i].IsBaoMat == false)
                        //    {
                        //        string sec_false = "<li style='margin-bottom: 5px;'>" + step + ". " + "<a href='" + fileYKien[i].FileUrl + "' download>" + fileYKien[i].TenFile + "</a></li>";
                        //        Info.HuongXuLy = "<div style='margin-bottom: 5px;'><span>" + Info.HuongXuLy + "</span></div>" + "<ul>" + sec_false + "</ul>";
                        //    }
                        //    else
                        //    {
                        //        string sec_true = "<li style='margin-bottom: 5px;'>" + step + ". " + "<a style='color: #EE7600;' href=" + "/DownloadFileDonThuCT.aspx?url=" + fileYKien[i].FileUrl + ">" + fileYKien[i].TenFile + "</a></li>";
                        //        Info.HuongXuLy = "<div style='margin-bottom: 5px;'><span>" + Info.HuongXuLy + "</span></div>" + "<ul>" + sec_true + "</ul>";
                        //    }
                        //}
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
        private DTXuLyInfo GetFileData(SqlDataReader rdr)
        {
            DTXuLyInfo info = new DTXuLyInfo();
            info.TenFile = Utils.ConvertToString(rdr["TenFile"], String.Empty);
            info.FileUrl = Utils.ConvertToString(rdr["FileURL"], String.Empty);
            info.IsBaoMat = Utils.ConvertToBoolean(rdr["IsBaoMat"], false);
            info.NguoiUp = Utils.ConvertToInt32(rdr["NguoiUp"], 0);
            return info;
        }
        public List<DTXuLyInfo> GetFileDonThuDaTiepNhan(int XuLyDonID)
        {
            List<DTXuLyInfo> ListInfo = new List<DTXuLyInfo>();

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_XULYDONID, SqlDbType.Int),
            };

            parms[0].Value = XuLyDonID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_FILEDONTHUDATIEPNHAN_LIST, parms))
                {
                    while (dr.Read())
                    {
                        DTXuLyInfo Info = new DTXuLyInfo();
                        Info = GetFileData(dr);
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
        public IList<DTXuLyInfo> GetSoTiepNhanGianTiep(QueryFilterInfo info)
        {
            IList<DTXuLyInfo> ListInfo = new List<DTXuLyInfo>();

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_KEY, SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_START, SqlDbType.Int),
                new SqlParameter(PARAM_END, SqlDbType.Int),
                new SqlParameter(PARAM_CANBOID,SqlDbType.Int)

            };

            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.KeyWord;
            parms[2].Value = info.LoaiKhieuToID;
            parms[3].Value = info.TuNgay;
            parms[4].Value = info.DenNgay;
            parms[5].Value = info.Start;
            parms[6].Value = info.End;
            parms[7].Value = info.CanBoID;

            if (info.CanBoID == 0) parms[7].Value = DBNull.Value;

            if (info.TuNgay == DateTime.MinValue) parms[3].Value = DBNull.Value;
            if (info.DenNgay == DateTime.MinValue) parms[4].Value = DBNull.Value;
            //SqlParameter[] parms = GetPara_Search();
            //SetPara_Search(parms, info, docList);
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_SOTIEPNHAN_GIANTIEP, parms))
                {
                    while (dr.Read())
                    {
                        DTXuLyInfo Info = GetDataDTDaTiepNhan(dr);
                        Info.CBDuocChonXL = Utils.ConvertToInt32(dr["CBDuocChonXL"], 0);
                        Info.QTTiepNhanDon = Utils.ConvertToInt32(dr["QTTiepNhanDon"], 0);
                        //Info.DoiTuongBiKNID = Utils.ConvertToInt32(dr["DoiTuongBiKDID"],0);
                        //Info.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"],0);
                        Info.TenCQChuyenDonDen = Utils.ConvertToString(dr["TenCQChuyenDonDen"], string.Empty);
                        Info.SoCongVan = Utils.ConvertToString(dr["SoCongVan"], string.Empty);
                        Info.HuongXuLy = Utils.ConvertToString(dr["TenHuongGiaiQuyet"], String.Empty);
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
        public IList<DTXuLyInfo> GetSoTiepNhanGianTiep_BTDTinh(ref int TotalRow, QueryFilterInfo info)
        {
            IList<DTXuLyInfo> ListInfo = new List<DTXuLyInfo>();

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_KEY, SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_START, SqlDbType.Int),
                new SqlParameter(PARAM_END, SqlDbType.Int),
                new SqlParameter(PARAM_CANBOID,SqlDbType.Int),
                new SqlParameter(PARAM_CQCHUYENDONDENID,SqlDbType.Int),
                new SqlParameter(@"LoaiRutDon",SqlDbType.Int),
                new SqlParameter(@"TotalRow",SqlDbType.Int)
            };

            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.KeyWord;
            parms[2].Value = info.LoaiKhieuToID;
            parms[3].Value = info.TuNgay ?? Convert.DBNull;
            parms[4].Value = info.DenNgay ?? Convert.DBNull;
            parms[5].Value = info.Start;
            parms[6].Value = info.End;
            parms[7].Value = info.CanBoID;
            parms[8].Value = info.CQChuyenDonDenID;
            parms[9].Value = info.LoaiRutDon;
            parms[10].Direction = ParameterDirection.Output;
            parms[10].Size = 8;

            if (info.CanBoID == 0) parms[7].Value = DBNull.Value;

            if (info.TuNgay == DateTime.MinValue) parms[3].Value = DBNull.Value;
            if (info.DenNgay == DateTime.MinValue) parms[4].Value = DBNull.Value;
            //SqlParameter[] parms = GetPara_Search();
            //SetPara_Search(parms, info, docList);
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_XuLyDon_GetSoTiepNhan_GianTiep_BTDTinh", parms))
                {
                    while (dr.Read())
                    {
                        DTXuLyInfo Info = GetDataDTDaTiepNhan(dr);
                        Info.CBDuocChonXL = Utils.ConvertToInt32(dr["CBDuocChonXL"], 0);
                        Info.QTTiepNhanDon = Utils.ConvertToInt32(dr["QTTiepNhanDon"], 0);
                        //Info.DoiTuongBiKNID = Utils.ConvertToInt32(dr["DoiTuongBiKNID"], 0);
                        Info.TenCQChuyenDonDen = Utils.ConvertToString(dr["TenCQChuyenDonDen"], string.Empty);
                        Info.SoCongVan = Utils.ConvertToString(dr["SoCongVan"], string.Empty);
                        Info.HuongXuLy = Utils.ConvertToString(dr["TenHuongGiaiQuyet"], String.Empty);
                        Info.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"], 0);
                        if (Info.NhomKNID > 0)
                        {
                            Info.NhomKN = new NhomKN().GetByID(Info.NhomKNID);
                            Info.listDoiTuongKN = new DoiTuongKN().GetByNhomKNID(Info.NhomKNID).ToList();
                        }
                        Info.CanBoXuLyID = Utils.ConvertToInt32(dr["CanBoXuLyID"], 0);
                        Info.CanBoTiepNhanID = Utils.ConvertToInt32(dr["CanBoTiepNhapID"], 0);
                        Info.NgayTiepNhan = Utils.GetDateTime(dr["NgayTiepNhan"], DateTime.MinValue);


                        //List<DTXuLyInfo> fileYKien = new List<DTXuLyInfo>();
                        //CanBoInfo canBoInfo = new CanBo().GetCanBoByID(info.CanBoID);

                        //if (canBoInfo.XemTaiLieuMat)
                        //{
                        //    fileYKien = GetFileDonThuDaTiepNhan(Info.XuLyDonID).ToList();
                        //}
                        //else
                        //{
                        //    fileYKien = GetFileDonThuDaTiepNhan(Info.XuLyDonID).Where(x => x.IsBaoMat != true || x.NguoiUp == canBoInfo.CanBoID).ToList();
                        //}

                        //int step = 0;
                        //for (int i = 0; i < fileYKien.Count; i++)
                        //{
                        //    if (!string.IsNullOrEmpty(fileYKien[i].FileUrl))
                        //    {
                        //        if (string.IsNullOrEmpty(fileYKien[i].TenFile))
                        //        {
                        //            string[] arrtenFile = fileYKien[i].FileUrl.Split('/');
                        //            if (arrtenFile.Length > 0)
                        //            {
                        //                string[] duoiFile = arrtenFile[arrtenFile.Length - 1].Split('_');
                        //                if (duoiFile.Length > 0)
                        //                {
                        //                    fileYKien[i].TenFile = duoiFile[duoiFile.Length - 1];
                        //                }
                        //                else
                        //                {
                        //                    fileYKien[i].TenFile = arrtenFile[arrtenFile.Length - 1];
                        //                }
                        //            }
                        //        }
                        //        fileYKien[i].FileUrl = fileYKien[i].FileUrl.Replace(" ", "%20");
                        //    }
                        //    step++;
                        //    if (fileYKien[i].IsBaoMat == false)
                        //    {
                        //        string sec_false = "<li style='margin-bottom: 5px;'>" + step + ". " + "<a href='" + fileYKien[i].FileUrl + "' download>" + fileYKien[i].TenFile + "</a></li>";
                        //        Info.HuongXuLy = "<div style='margin-bottom: 5px;'><span>" + Info.HuongXuLy + "</span></div>" + "<ul>" + sec_false + "</ul>";
                        //    }
                        //    else
                        //    {
                        //        string sec_true = "<li style='margin-bottom: 5px;'>" + step + ". " + "<a style='color: #EE7600;' href=" + "/DownloadFileDonThuCT.aspx?url=" + fileYKien[i].FileUrl + ">" + fileYKien[i].TenFile + "</a></li>";
                        //        Info.HuongXuLy = "<div style='margin-bottom: 5px;'><span>" + Info.HuongXuLy + "</span></div>" + "<ul>" + sec_true + "</ul>";
                        //    }
                        //}
                        // bổ sung rút đơn ID
                        Info.RutDonID = Utils.ConvertToInt32(dr["RutDonID"], 0);
                        ListInfo.Add(Info);
                    }
                    dr.Close();
                }
                TotalRow = Utils.ConvertToInt32(parms[10].Value, 0);
            }
            catch
            {

                throw;
            }

            return ListInfo;
        }

        public IList<DTXuLyInfo> GetInSoTiepNhanGianTiep_BTDTinh(string keyword, DateTime tuNgay, DateTime denNgay, int coQuanID, int loaiKhieuToID, int cqChuyenDonDens)
        {
            IList<DTXuLyInfo> ListInfo = new List<DTXuLyInfo>();

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_KEY, SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_CQCHUYENDONDENID, SqlDbType.Int)
            };

            parms[0].Value = coQuanID;
            parms[1].Value = keyword;
            parms[2].Value = loaiKhieuToID;
            parms[3].Value = tuNgay;
            parms[4].Value = denNgay;
            parms[5].Value = cqChuyenDonDens;

            if (tuNgay == DateTime.MinValue) parms[3].Value = DBNull.Value;
            if (denNgay == DateTime.MinValue) parms[4].Value = DBNull.Value;
            //SqlParameter[] parms = GetPara_Search();
            //SetPara_Search(parms, info, docList);
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_INSOTIEPNHAN_GIANTIEP_BTDTINH, parms))
                {
                    while (dr.Read())
                    {
                        DTXuLyInfo Info = GetDataDTDaTiepNhan(dr);
                        Info.CBDuocChonXL = Utils.ConvertToInt32(dr["CBDuocChonXL"], 0);
                        Info.QTTiepNhanDon = Utils.ConvertToInt32(dr["QTTiepNhanDon"], 0);
                        Info.TenCQChuyenDonDen = Utils.ConvertToString(dr["TenCQChuyenDonDen"], string.Empty);
                        Info.SoCongVan = Utils.ConvertToString(dr["SoCongVan"], string.Empty);
                        // Info.HuongXuLy = Utils.ConvertToString(dr["TenHuongGiaiQuyet"], string.Empty);
                        Info.TenHuongGiaiQuyet = Utils.ConvertToString(dr["TenHuongGiaiQuyet"], string.Empty);
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

        public IList<DTXuLyInfo> GetInSoGianTiep_CaNhan(string keyword, DateTime tuNgay, DateTime denNgay, int coQuanID, int loaiKhieuToID, int canBoID, int cqChuyenDonDens)
        {
            IList<DTXuLyInfo> ListInfo = new List<DTXuLyInfo>();

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_KEY, SqlDbType.NVarChar,500),
                new SqlParameter(PARAM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_CANBOID, SqlDbType.Int),
                new SqlParameter(PARAM_CQCHUYENDONDENID, SqlDbType.Int)

            };

            parms[0].Value = coQuanID;
            parms[1].Value = keyword;
            parms[2].Value = loaiKhieuToID;
            parms[3].Value = tuNgay;
            parms[4].Value = denNgay;
            parms[5].Value = canBoID;
            parms[6].Value = cqChuyenDonDens;

            if (tuNgay == DateTime.MinValue) parms[3].Value = DBNull.Value;
            if (denNgay == DateTime.MinValue) parms[4].Value = DBNull.Value;
            //SqlParameter[] parms = GetPara_Search();
            //SetPara_Search(parms, info, docList);
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_INSOGIANTIEP_CANHAN, parms))
                {
                    while (dr.Read())
                    {
                        DTXuLyInfo Info = GetDataDTDaTiepNhan(dr);
                        Info.CBDuocChonXL = Utils.ConvertToInt32(dr["CBDuocChonXL"], 0);
                        Info.QTTiepNhanDon = Utils.ConvertToInt32(dr["QTTiepNhanDon"], 0);
                        // Info.HuongXuLy = Utils.ConvertToString(dr["TenHuongGiaiQuyet"], string.Empty);
                        Info.TenHuongGiaiQuyet = Utils.ConvertToString(dr["TenHuongGiaiQuyet"], string.Empty);

                        ListInfo.Add(Info);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return ListInfo;
        }

        public IList<DTXuLyInfo> GetInDTDaTiepNhan(DateTime TuNgay, DateTime DenNgay, int CoQuanID)
        {
            IList<DTXuLyInfo> ListInfo = new List<DTXuLyInfo>();

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
            };
            parms[0].Value = CoQuanID;
            parms[1].Value = TuNgay;
            parms[2].Value = DenNgay;

            if (TuNgay == DateTime.MinValue) parms[1].Value = DBNull.Value;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_IN_DTDA_TIEPNHAN, parms))
                {
                    while (dr.Read())
                    {
                        DTXuLyInfo info = GetDataDTDaTiepNhan(dr);
                        info.NgayQuaHan = Utils.ConvertToDateTime(dr["NgayQuaHan"], DateTime.MinValue);
                        ListInfo.Add(info);
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

        public IList<DTXuLyInfo> GetInDTDaTiepNhan(DateTime TuNgay, DateTime DenNgay, int CoQuanID, int LoaiKhieuToID)
        {
            IList<DTXuLyInfo> ListInfo = new List<DTXuLyInfo>();

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_LOAIKHIEUTOID, SqlDbType.Int),
            };
            parms[0].Value = CoQuanID;
            parms[1].Value = TuNgay;
            parms[2].Value = DenNgay;
            parms[3].Value = LoaiKhieuToID;

            if (TuNgay == DateTime.MinValue) parms[1].Value = DBNull.Value;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_IN_DTDA_TIEPNHAN_NEW, parms))
                {
                    while (dr.Read())
                    {
                        DTXuLyInfo info = GetDataDTDaTiepNhan(dr);
                        info.NgayQuaHan = Utils.ConvertToDateTime(dr["NgayQuaHan"], DateTime.MinValue);
                        ListInfo.Add(info);
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

        public IList<DTXuLyInfo> GetInDTDaTiepNhanCaNhan(DateTime TuNgay, DateTime DenNgay, int CoQuanID, int canBoID)
        {
            IList<DTXuLyInfo> ListInfo = new List<DTXuLyInfo>();

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_CANBOID, SqlDbType.Int)
            };
            parms[0].Value = CoQuanID;
            parms[1].Value = TuNgay;
            parms[2].Value = DenNgay;
            parms[3].Value = canBoID;

            if (TuNgay == DateTime.MinValue) parms[1].Value = DBNull.Value;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_IN_DTDA_TIEPNHAN_CANHAN, parms))
                {
                    while (dr.Read())
                    {
                        DTXuLyInfo info = GetDataDTDaTiepNhan(dr);
                        info.NgayQuaHan = Utils.ConvertToDateTime(dr["NgayQuaHan"], DateTime.MinValue);
                        ListInfo.Add(info);
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

        public IList<DTXuLyInfo> GetInDTDaTiepNhanCaNhan(DateTime TuNgay, DateTime DenNgay, int CoQuanID, int canBoID, int LoaiKhieuToID)
        {
            IList<DTXuLyInfo> ListInfo = new List<DTXuLyInfo>();

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_CANBOID, SqlDbType.Int),
                new SqlParameter(PARAM_LOAIKHIEUTOID, SqlDbType.Int),
            };
            parms[0].Value = CoQuanID;
            parms[1].Value = TuNgay;
            parms[2].Value = DenNgay;
            parms[3].Value = canBoID;
            parms[4].Value = LoaiKhieuToID;

            if (TuNgay == DateTime.MinValue) parms[1].Value = DBNull.Value;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_IN_DTDA_TIEPNHAN_CANHAN_NEW, parms))
                {
                    while (dr.Read())
                    {
                        DTXuLyInfo info = GetDataDTDaTiepNhan(dr);
                        info.NgayQuaHan = Utils.ConvertToDateTime(dr["NgayQuaHan"], DateTime.MinValue);
                        ListInfo.Add(info);
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

        public int Count_DTDaTiepNhan(QueryFilterInfo info)
        {
            int result = 0;

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_KEY, SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_CANBOID,SqlDbType.Int)

            };
            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.KeyWord;
            parms[2].Value = info.LoaiKhieuToID;
            parms[3].Value = info.TuNgay;
            parms[4].Value = info.DenNgay;
            parms[5].Value = info.CanBoID;

            if (info.TuNgay == DateTime.MinValue) parms[3].Value = DBNull.Value;
            if (info.DenNgay == DateTime.MinValue) parms[4].Value = DBNull.Value;

            if (info.CanBoID == 0) parms[5].Value = DBNull.Value;

            //SqlParameter[] parms = GetPara_Count();
            //SetPara_Count(parms, info, docList);
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_DTDA_TIEPNHAN, parms))
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

        public int Count_SoTiepNhanGianTiep(QueryFilterInfo info)
        {
            int result = 0;

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_KEY, SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_CANBOID,SqlDbType.Int),
                new SqlParameter(@"LoaiRutDon",SqlDbType.Int)
            };
            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.KeyWord;
            parms[2].Value = info.LoaiKhieuToID;
            parms[3].Value = info.TuNgay;
            parms[4].Value = info.DenNgay;
            parms[5].Value = info.CanBoID;
            parms[6].Value = info.LoaiRutDon;
            if (info.TuNgay == DateTime.MinValue) parms[3].Value = DBNull.Value;
            if (info.DenNgay == DateTime.MinValue) parms[4].Value = DBNull.Value;

            if (info.CanBoID == 0) parms[5].Value = DBNull.Value;

            //SqlParameter[] parms = GetPara_Count();
            //SetPara_Count(parms, info, docList);
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_SOTIEPNHAN_GIANTIEP, parms))
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

        public int Count_SoTiepNhanGianTiep_BTDTinh(QueryFilterInfo info)
        {
            int result = 0;

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_KEY, SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_CANBOID,SqlDbType.Int),
                new SqlParameter(PARAM_CQCHUYENDONDENID,SqlDbType.Int),
                new SqlParameter("@LoaiRutDon",SqlDbType.Int)
            };
            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.KeyWord;
            parms[2].Value = info.LoaiKhieuToID;
            parms[3].Value = info.TuNgay;
            parms[4].Value = info.DenNgay;
            parms[5].Value = info.CanBoID;
            parms[6].Value = info.CQChuyenDonDenID;
            parms[7].Value = info.LoaiRutDon;
            if (info.TuNgay == DateTime.MinValue) parms[3].Value = DBNull.Value;
            if (info.DenNgay == DateTime.MinValue) parms[4].Value = DBNull.Value;

            if (info.CanBoID == 0) parms[5].Value = DBNull.Value;

            //SqlParameter[] parms = GetPara_Count();
            //SetPara_Count(parms, info, docList);
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_SOTIEPNHAN_GIANTIEP_BTDTINH, parms))
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

        #endregion

        #region == don thu duyet ket qua xu ly ==


        public int Count_DTDuyetKQXL_LanhDao_CoutAllByCoQuanID(QueryFilterInfo info, List<int> docList)
        {
            int result = 0;
            SqlParameter[] parms = GetPara_Count_LanhDao();
            SetPara_Count_LanhDao(parms, info, docList);
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, XULYDON_COUNTDTDUYETXL_LANHDAO_COUNTALLBYCOQUANID, parms))
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

        public int Count_DTDuyetKQXL_LanhDao(QueryFilterInfo info, List<int> docList)
        {
            int result = 0;
            SqlParameter[] parms = GetPara_Count_LanhDao();
            SetPara_Count_LanhDao(parms, info, docList);
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_DTDUYETXL_LANHDAO, parms))
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

        public IList<DTXuLyInfo> DTDuyetKQXL_ChuTich(ref int TotalRow, QueryFilterInfo info, List<int> docList)
        {

            IList<DTXuLyInfo> ListInfo = new List<DTXuLyInfo>();

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_KEY, SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_START, SqlDbType.Int),
                new SqlParameter(PARAM_END, SqlDbType.Int),
                new SqlParameter("@TrangThai", SqlDbType.Int),
                new SqlParameter("@HuongXuLyID", SqlDbType.Int),
                new SqlParameter("@TotalRow",SqlDbType.Int),
                new SqlParameter("@ChuTichUBND",SqlDbType.Int),
                new SqlParameter("@CanBoID",SqlDbType.Int),
                //para
            };
            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.KeyWord ?? "";
            parms[2].Value = info.LoaiKhieuToID;
            parms[3].Value = info.TuNgay;
            parms[4].Value = info.DenNgay;
            parms[5].Value = info.Start;
            parms[6].Value = info.End;
            parms[7].Value = info.TrangThai ?? Convert.DBNull;
            parms[8].Value = info.HuongXuLyID;
            parms[9].Direction = ParameterDirection.Output;
            parms[9].Size = 8;
            parms[10].Value = info.ChuTichUBND ?? Convert.DBNull;
            parms[11].Value = info.CanBoID;

            if (info.TuNgay == DateTime.MinValue) parms[3].Value = DBNull.Value;
            if (info.DenNgay == DateTime.MinValue) parms[4].Value = DBNull.Value;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_XuLyDon_GetDTDuyetXL_ChuTich", parms))
                {
                    while (dr.Read())
                    {
                        DTXuLyInfo Info = GetDataDTDuyetXuLy(dr);
                        Info.NgayQuaHan = Utils.ConvertToDateTime(dr["HanXuLyDueDate"], DateTime.MaxValue);
                        Info.HanXuLy = Format.FormatDate(Info.NgayQuaHan);
                        Info.TransitionID = Utils.ConvertToInt32(dr["TransitionID"], 0);
                        Info.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"], 0);

                        Info.LoaiQuyTrinh = Utils.ConvertToInt32(dr["LoaiQuyTrinh"], 0);
                        Info.HuongGiaiQuyetID = Utils.ConvertToInt32(dr["HuongGiaiQuyetID"], 0);
                        Info.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        Info.TrangThaiDuyet = Utils.ConvertToInt32(dr["TrangThaiDuyet"], 0);
                        Info.TrinhDuThao = Utils.ConvertToInt32(dr["TrinhDuThao"], 0);
                        Info.ChuyenGiaiQuyetID = Utils.ConvertToInt32(dr["ChuyenGiaiQuyetID"], 0);
                        Info.NgayTiepNhan = Utils.GetDateTime(dr["NgayTiepNhan"], DateTime.MinValue);
                        Info.KetQuaID = Utils.ConvertToInt32(dr["KetQuaID"], 0);
                        //Info.Count = Utils.ConvertToInt32(dr["CountNum"], 0);
                        if (Info.NhomKNID > 0)
                        {
                            Info.listDoiTuongKN = new DoiTuongKN().GetByNhomKNID(Info.NhomKNID).ToList();
                        }
                        ListInfo.Add(Info);
                    }
                    dr.Close();
                }
                TotalRow = Utils.ConvertToInt32(parms[9].Value, 0);
            }
            catch
            {

                throw;
            }
            return ListInfo;
        }

        public IList<DTXuLyInfo> DTDuyetKQXL_LanhDao(ref int TotalRow, QueryFilterInfo info, List<int> docList)
        {

            IList<DTXuLyInfo> ListInfo = new List<DTXuLyInfo>();
            //SqlParameter[] parms = GetPara_Search_LanhDao();
            //SetPara_Search_LanhDao(parms, info, docList);

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_KEY, SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_START, SqlDbType.Int),
                new SqlParameter(PARAM_END, SqlDbType.Int),
                new SqlParameter("@TrangThai", SqlDbType.Int),
                new SqlParameter("@HuongXuLyID", SqlDbType.Int),
                new SqlParameter("@TotalRow",SqlDbType.Int),
                new SqlParameter("@ChuTichUBND",SqlDbType.Int),
                new SqlParameter("@CanBoID",SqlDbType.Int),
                //para
            };
            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.KeyWord ?? "";
            parms[2].Value = info.LoaiKhieuToID;
            parms[3].Value = info.TuNgay;
            parms[4].Value = info.DenNgay;
            parms[5].Value = info.Start;
            parms[6].Value = info.End;
            parms[7].Value = info.TrangThai ?? Convert.DBNull;
            parms[8].Value = info.HuongXuLyID;
            parms[9].Direction = ParameterDirection.Output;
            parms[9].Size = 8;
            parms[10].Value = info.ChuTichUBND ?? Convert.DBNull;
            parms[11].Value = info.CanBoID;

            if (info.TuNgay == DateTime.MinValue) parms[3].Value = DBNull.Value;
            if (info.DenNgay == DateTime.MinValue) parms[4].Value = DBNull.Value;

            try
            {
                //using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_XuLyDon_GetDTDuyetXL_LanhDao_New", parms))
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_XuLyDon_GetDTDuyetXL", parms))
                {
                    while (dr.Read())
                    {
                        DTXuLyInfo Info = GetDataDTDuyetXuLy(dr);
                        Info.NgayQuaHan = Utils.ConvertToDateTime(dr["HanXuLyDueDate"], DateTime.MaxValue);
                        Info.HanXuLy = Format.FormatDate(Info.NgayQuaHan);
                        Info.TransitionID = Utils.ConvertToInt32(dr["TransitionID"], 0);
                        Info.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"], 0);
                        Info.LanhDaoDuyet2ID = Utils.ConvertToInt32(dr["LanhDaoDuyet2ID"], 0);
                        //Info.Count = Utils.ConvertToInt32(dr["CountNum"], 0);
                        if (Info.NhomKNID > 0)
                        {
                            Info.listDoiTuongKN = new DoiTuongKN().GetByNhomKNID(Info.NhomKNID).ToList();
                        }

                        Info.LoaiQuyTrinh = Utils.ConvertToInt32(dr["LoaiQuyTrinh"], 0);
                        Info.HuongGiaiQuyetID = Utils.ConvertToInt32(dr["HuongGiaiQuyetID"], 0);
                        Info.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        Info.TrangThaiDuyet = Utils.ConvertToInt32(dr["TrangThaiDuyet"], 0);
                        Info.TrinhDuThao = Utils.ConvertToInt32(dr["TrinhDuThao"], 0);
                        Info.ChuyenGiaiQuyetID = Utils.ConvertToInt32(dr["ChuyenGiaiQuyetID"], 0);
                        Info.NgayTiepNhan = Utils.GetDateTime(dr["NgayTiepNhan"], DateTime.MinValue);
                        // bổ sung rút đơn id
                        Info.RutDonID = Utils.ConvertToInt32(dr["RutDonID"], 0);
                        ListInfo.Add(Info);
                    }
                    dr.Close();
                }
                TotalRow = Utils.ConvertToInt32(parms[9].Value, 0);
            }
            catch
            {

                throw;
            }
            return ListInfo;
        }

        public IList<DTXuLyInfo> DTDuyetKQXL_LanhDao_Huyen(ref int TotalRow, QueryFilterInfo info, List<int> docList)
        {
            IList<DTXuLyInfo> ListInfo = new List<DTXuLyInfo>();

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_KEY, SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_START, SqlDbType.Int),
                new SqlParameter(PARAM_END, SqlDbType.Int),
                new SqlParameter("@TrangThai", SqlDbType.Int),
                new SqlParameter("@HuongXuLyID", SqlDbType.Int),
                new SqlParameter("@TotalRow",SqlDbType.Int),
                new SqlParameter("@ChuTichUBND",SqlDbType.Int),
                new SqlParameter("@CanBoID",SqlDbType.Int),
                //para
            };
            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.KeyWord ?? "";
            parms[2].Value = info.LoaiKhieuToID;
            parms[3].Value = info.TuNgay;
            parms[4].Value = info.DenNgay;
            parms[5].Value = info.Start;
            parms[6].Value = info.End;
            parms[7].Value = info.TrangThai ?? Convert.DBNull;
            parms[8].Value = info.HuongXuLyID;
            parms[9].Direction = ParameterDirection.Output;
            parms[9].Size = 8;
            parms[10].Value = info.ChuTichUBND ?? Convert.DBNull;
            parms[11].Value = info.CanBoID;

            if (info.TuNgay == DateTime.MinValue) parms[3].Value = DBNull.Value;
            if (info.DenNgay == DateTime.MinValue) parms[4].Value = DBNull.Value;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_XuLyDon_GetDTDuyetXL_LanhDao_Huyen_New", parms))
                {
                    while (dr.Read())
                    {
                        DTXuLyInfo Info = GetDataDTDuyetXuLy(dr);
                        Info.NgayQuaHan = Utils.ConvertToDateTime(dr["HanXuLyDueDate"], DateTime.MaxValue);
                        Info.HanXuLy = Format.FormatDate(Info.NgayQuaHan);
                        Info.TransitionID = Utils.ConvertToInt32(dr["TransitionID"], 0);
                        Info.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"], 0);

                        Info.LoaiQuyTrinh = Utils.ConvertToInt32(dr["LoaiQuyTrinh"], 0);
                        Info.HuongGiaiQuyetID = Utils.ConvertToInt32(dr["HuongGiaiQuyetID"], 0);
                        Info.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        Info.TrangThaiDuyet = Utils.ConvertToInt32(dr["TrangThaiDuyet"], 0);
                        Info.TrinhDuThao = Utils.ConvertToInt32(dr["TrinhDuThao"], 0);
                        Info.ChuyenGiaiQuyetID = Utils.ConvertToInt32(dr["ChuyenGiaiQuyetID"], 0);
                        Info.NgayTiepNhan = Utils.GetDateTime(dr["NgayTiepNhan"], DateTime.MinValue);
                        Info.KetQuaID = Utils.ConvertToInt32(dr["KetQuaID"], 0);

                        //Info.Count = Utils.ConvertToInt32(dr["CountNum"], 0);
                        if (Info.NhomKNID > 0)
                        {
                            Info.listDoiTuongKN = new DoiTuongKN().GetByNhomKNID(Info.NhomKNID).ToList();
                        }
                        ListInfo.Add(Info);
                    }
                    dr.Close();
                }
                TotalRow = Utils.ConvertToInt32(parms[9].Value, 0);
            }
            catch
            {

                throw;
            }
            return ListInfo;
        }

        public IList<DTXuLyInfo> DTDuyetKQXL_LanhDao_Xa(ref int TotalRow, QueryFilterInfo info, List<int> docList)
        {

            IList<DTXuLyInfo> ListInfo = new List<DTXuLyInfo>();
            //SqlParameter[] parms = GetPara_Search_LanhDao();
            //SetPara_Search_LanhDao(parms, info, docList);

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_KEY, SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_START, SqlDbType.Int),
                new SqlParameter(PARAM_END, SqlDbType.Int),
                new SqlParameter("@TrangThai", SqlDbType.Int),
                new SqlParameter("@HuongXuLyID", SqlDbType.Int),
                new SqlParameter("@TotalRow",SqlDbType.Int),
                new SqlParameter("@ChuTichUBND",SqlDbType.Int),
                //para
            };
            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.KeyWord ?? "";
            parms[2].Value = info.LoaiKhieuToID;
            parms[3].Value = info.TuNgay;
            parms[4].Value = info.DenNgay;
            parms[5].Value = info.Start;
            parms[6].Value = info.End;
            parms[7].Value = info.TrangThai ?? Convert.DBNull;
            parms[8].Value = info.HuongXuLyID;
            parms[9].Direction = ParameterDirection.Output;
            parms[9].Size = 8;
            parms[10].Value = info.ChuTichUBND ?? Convert.DBNull;


            if (info.TuNgay == DateTime.MinValue) parms[3].Value = DBNull.Value;
            if (info.DenNgay == DateTime.MinValue) parms[4].Value = DBNull.Value;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_XuLyDon_GetDTDuyetXL_LanhDao_New", parms))
                {
                    while (dr.Read())
                    {
                        DTXuLyInfo Info = GetDataDTDuyetXuLy(dr);
                        Info.LoaiQuyTrinh = Utils.ConvertToInt32(dr["LoaiQuyTrinh"], 0);
                        Info.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        Info.NgayQuaHan = Utils.ConvertToDateTime(dr["HanXuLyDueDate"], DateTime.MaxValue);
                        Info.HanXuLy = Format.FormatDate(Info.NgayQuaHan);
                        Info.TransitionID = Utils.ConvertToInt32(dr["TransitionID"], 0);
                        Info.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"], 0);
                        Info.TrinhDuThao = Utils.ConvertToInt32(dr["TrinhDuThao"], 0);
                        Info.HuongGiaiQuyetID = Utils.ConvertToInt32(dr["HuongGiaiQuyetID"], 0);
                        //Info.Count = Utils.ConvertToInt32(dr["CountNum"], 0);
                        if (Info.NhomKNID > 0)
                        {
                            Info.listDoiTuongKN = new DoiTuongKN().GetByNhomKNID(Info.NhomKNID).ToList();
                        }
                        Info.NgayTiepNhan = Utils.ConvertToDateTime(dr["NgayTiepNhan"], DateTime.MaxValue);
                        // bổ sung rút đơn id
                        Info.RutDonID = Utils.ConvertToInt32(dr["RutDonID"], 0);
                        ListInfo.Add(Info);
                    }
                    dr.Close();
                }
                TotalRow = Utils.ConvertToInt32(parms[9].Value, 0);
            }
            catch
            {

                throw;
            }
            return ListInfo;
        }


        public IList<DTXuLyInfo> DTDuyetKQXL_LanhDao_GetAllByCoQuanID(ref int TotalRow, QueryFilterInfo info, List<int> docList)
        {

            IList<DTXuLyInfo> ListInfo = new List<DTXuLyInfo>();
            //SqlParameter[] parms = GetPara_Search_LanhDao();
            //SetPara_Search_LanhDao(parms, info, docList);

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_KEY, SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_START, SqlDbType.Int),
                new SqlParameter(PARAM_END, SqlDbType.Int),
                new SqlParameter("@TrangThai", SqlDbType.Int),
                new SqlParameter("@HuongXuLyID", SqlDbType.Int),
                new SqlParameter("@TotalRow",SqlDbType.Int),
                //para
            };
            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.KeyWord ?? "";
            parms[2].Value = info.LoaiKhieuToID;
            parms[3].Value = info.TuNgay;
            parms[4].Value = info.DenNgay;
            parms[5].Value = info.Start;
            parms[6].Value = info.End;
            parms[7].Value = info.TrangThai ?? Convert.DBNull;
            parms[8].Value = info.HuongXuLyID;
            parms[9].Direction = ParameterDirection.Output;
            parms[9].Size = 8;

            if (info.TuNgay == DateTime.MinValue) parms[3].Value = DBNull.Value;
            if (info.DenNgay == DateTime.MinValue) parms[4].Value = DBNull.Value;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_XuLyDon_GetDTDuyetXL_LanhDao_GetAll_ByCoQuanID", parms))
                {
                    System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                    stopwatch.Start();
                    while (dr.Read())
                    {
                        DTXuLyInfo Info = GetDataDTDuyetXuLy(dr);
                        Info.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        Info.NgayQuaHan = Utils.ConvertToDateTime(dr["HanXuLyDueDate"], DateTime.MaxValue);
                        Info.HanXuLy = Format.FormatDate(Info.NgayQuaHan);
                        Info.TransitionID = Utils.ConvertToInt32(dr["TransitionID"], 0);
                        Info.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"], 0);
                        Info.Count = Utils.ConvertToInt32(dr["CountNum"], 0);
                        if (Info.NhomKNID > 0)
                        {
                            Info.listDoiTuongKN = new DoiTuongKN().GetByNhomKNID(Info.NhomKNID).ToList();
                        }
                        ListInfo.Add(Info);
                    }
                    dr.Close();
                    var timer = stopwatch.Elapsed.TotalSeconds;
                    TotalRow = Utils.ConvertToInt32(parms[9].Value, 0);
                }
            }
            catch
            {

                throw;
            }
            return ListInfo;
        }

        public int Count_DTDuyetKQXL_TruongPhong_CoutAllByCoQuanID(QueryFilterInfo info, List<int> docList)
        {
            int result = 0;
            SqlParameter[] parms = GetPara_Count_TruongPhong();
            SetPara_Count_TruongPhong(parms, info, docList);
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, XULYDON_COUNTDTDUYETXL_TRUONGPHONG_COUNTALLBYCOQUANID, parms))
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
        public int Count_DTDuyetKQXL_TruongPhong(QueryFilterInfo info, List<int> docList)
        {
            int result = 0;
            SqlParameter[] parms = GetPara_Count_TruongPhong();
            SetPara_Count_TruongPhong(parms, info, docList);
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_DTDUYETXL_TRUONGPHONG, parms))
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

        public IList<DTXuLyInfo> DTDuyetKQXL_TruongPhong(QueryFilterInfo info, List<int> docList)
        {

            IList<DTXuLyInfo> ListInfo = new List<DTXuLyInfo>();
            SqlParameter[] parms = GetPara_Search_TruongPhong();
            SetPara_Search_TruongPhong(parms, info, docList);
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DTDUYETXL_TRUONGPHONG_NEW, parms))
                {
                    while (dr.Read())
                    {
                        DTXuLyInfo Info = GetDataDTDuyetXuLy(dr);
                        Info.NgayQuaHan = Utils.ConvertToDateTime(dr["HanXuLyDueDateLDPhan"], DateTime.MaxValue);
                        Info.HanXuLy = Format.FormatDate(Info.NgayQuaHan);
                        Info.TransitionID = Utils.ConvertToInt32(dr["TransitionID"], 0);
                        Info.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"], 0);
                        Info.Count = Utils.ConvertToInt32(dr["CountNum"], 0);
                        if (Info.NhomKNID > 0)
                        {
                            Info.listDoiTuongKN = new DoiTuongKN().GetByNhomKNID(Info.NhomKNID).ToList();
                        }

                        Info.LanhDaoDuyet2ID = Utils.ConvertToInt32(dr["LanhDaoDuyet2ID"], 0);
                        Info.LoaiQuyTrinh = Utils.ConvertToInt32(dr["LoaiQuyTrinh"], 0);
                        Info.HuongGiaiQuyetID = Utils.ConvertToInt32(dr["HuongGiaiQuyetID"], 0);
                        Info.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        Info.TrangThaiDuyet = Utils.ConvertToInt32(dr["TrangThaiDuyet"], 0);
                        Info.TrinhDuThao = Utils.ConvertToInt32(dr["TrinhDuThao"], 0);
                        Info.ChuyenGiaiQuyetID = Utils.ConvertToInt32(dr["ChuyenGiaiQuyetID"], 0);
                        Info.NgayTiepNhan = Utils.GetDateTime(dr["NgayTiepNhan"], DateTime.MinValue);
                        Info.KetQuaID = Utils.ConvertToInt32(dr["KetQuaID"], 0);
                        // bổ sung rút đơn id 
                        Info.RutDonID = Utils.ConvertToInt32(dr["RutDonID"], 0);
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

        public IList<DTXuLyInfo> DTDuyetKQXL_TruongPhong_GetAllByCoQuanID(QueryFilterInfo info, List<int> docList)
        {

            IList<DTXuLyInfo> ListInfo = new List<DTXuLyInfo>();
            SqlParameter[] parms = GetPara_Search_TruongPhong();
            SetPara_Search_TruongPhong(parms, info, docList);
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, XULYDON_GETDTDUYETXL_TRUONGPHONG_GETALLBYCOQUANID_NEW, parms))
                {
                    while (dr.Read())
                    {
                        DTXuLyInfo Info = GetDataDTDuyetXuLy(dr);
                        Info.NgayQuaHan = Utils.ConvertToDateTime(dr["HanXuLyDueDateLDPhan"], DateTime.MaxValue);
                        Info.HanXuLy = Format.FormatDate(Info.NgayQuaHan);
                        Info.TransitionID = Utils.ConvertToInt32(dr["TransitionID"], 0);
                        Info.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"], 0);
                        Info.Count = Utils.ConvertToInt32(dr["CountNum"], 0);
                        if (Info.NhomKNID > 0)
                        {
                            Info.listDoiTuongKN = new DoiTuongKN().GetByNhomKNID(Info.NhomKNID).ToList();
                        }
                        Info.LanhDaoDuyet2ID = Utils.ConvertToInt32(dr["LanhDaoDuyet2ID"], 0);

                        Info.LoaiQuyTrinh = Utils.ConvertToInt32(dr["LoaiQuyTrinh"], 0);
                        Info.HuongGiaiQuyetID = Utils.ConvertToInt32(dr["HuongGiaiQuyetID"], 0);
                        Info.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        Info.TrangThaiDuyet = Utils.ConvertToInt32(dr["TrangThaiDuyet"], 0);
                        Info.TrinhDuThao = Utils.ConvertToInt32(dr["TrinhDuThao"], 0);
                        Info.ChuyenGiaiQuyetID = Utils.ConvertToInt32(dr["ChuyenGiaiQuyetID"], 0);
                        Info.NgayTiepNhan = Utils.GetDateTime(dr["NgayTiepNhan"], DateTime.MinValue);
                        Info.KetQuaID = Utils.ConvertToInt32(dr["KetQuaID"], 0);
                        // bổ sung trạng thái rút đơn
                        Info.RutDonID = Utils.ConvertToInt32(dr["RutDonID"], 0);
                        ListInfo.Add(Info);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return ListInfo;
        }

        #endregion


        #region == don thu co quan khac chuyen den ==

        public IList<DTXuLyInfo> GetDTCQKhacCD(QueryFilterInfo info)
        {
            IList<DTXuLyInfo> ListInfo = new List<DTXuLyInfo>();

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_KEY, SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_START, SqlDbType.Int),
                new SqlParameter(PARAM_END, SqlDbType.Int),
                new SqlParameter(PARAM_STATENAME,SqlDbType.NVarChar, 50)


            };

            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.KeyWord;
            parms[2].Value = info.LoaiKhieuToID;
            parms[3].Value = info.TuNgay;
            parms[4].Value = info.DenNgay;
            parms[5].Value = info.Start;
            parms[6].Value = info.End;
            parms[7].Value = info.StateName;


            if (info.TuNgay == DateTime.MinValue) parms[3].Value = DBNull.Value;
            if (info.DenNgay == DateTime.MinValue) parms[4].Value = DBNull.Value;
            //SqlParameter[] parms = GetPara_Search();
            //SetPara_Search(parms, info, docList);
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_DT_CQKHAC_CD, parms))
                {
                    while (dr.Read())
                    {
                        DTXuLyInfo Info = GetDataDTDaTiepNhan(dr);
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
        public int Count_DTCQKhacCD(QueryFilterInfo info)
        {
            int result = 0;

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_KEY, SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_STATENAME,SqlDbType.NVarChar, 50)


            };
            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.KeyWord;
            parms[2].Value = info.LoaiKhieuToID;
            parms[3].Value = info.TuNgay;
            parms[4].Value = info.DenNgay;
            parms[5].Value = info.StateName;


            if (info.TuNgay == DateTime.MinValue) parms[3].Value = DBNull.Value;
            if (info.DenNgay == DateTime.MinValue) parms[4].Value = DBNull.Value;

            //SqlParameter[] parms = GetPara_Count();
            //SetPara_Count(parms, info, docList);
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_DT_CQKHAC_CD, parms))
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

        #endregion

        public IList<DTXuLyInfo> BaoCaoTinhHinhXuLyDon(int coQuanID, int loaiKhieuToID, DateTime tuNgay, DateTime denNgay)
        {
            IList<DTXuLyInfo> ListInfo = new List<DTXuLyInfo>();

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
            };

            parms[0].Value = coQuanID;
            parms[1].Value = loaiKhieuToID;
            parms[2].Value = tuNgay;
            parms[3].Value = denNgay;
            if (tuNgay == DateTime.MinValue) parms[2].Value = DBNull.Value;
            if (denNgay == DateTime.MinValue) parms[3].Value = DBNull.Value;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, XuLyDon_BaoCao_TH_XuLyDon_KN_TC_DaXuLy, parms))
                {
                    while (dr.Read())
                    {
                        DTXuLyInfo Info = new DTXuLyInfo();
                        Info.DonThuID = Utils.GetInt32(dr["DonThuID"], 0);
                        Info.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);
                        Info.StateName = Utils.GetString(dr["StateName"], String.Empty);
                        Info.HuongGiaiQuyetID = Utils.ConvertToInt32(dr["HuongGiaiQuyetID"], 0);
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

        public int BC_TinhHinh_SD_PhanMem_CountDonThuChuyenDen(int coQuanID, DateTime tuNgay, DateTime denNgay)
        {
            int result = 0;

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),

            };
            parms[0].Value = coQuanID;
            parms[1].Value = tuNgay;
            parms[2].Value = denNgay;

            if (tuNgay == DateTime.MinValue) parms[1].Value = DBNull.Value;
            if (denNgay == DateTime.MinValue) parms[2].Value = DBNull.Value;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, BC_TH_SUDUNG_PM_COUNT_SODON_CHUYENDEN, parms))
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
        public int BC_TinhHinh_SD_PhanMem_CountDonThuGianTiep(int coQuanID, DateTime tuNgay, DateTime denNgay)
        {
            int result = 0;

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),

            };
            parms[0].Value = coQuanID;
            parms[1].Value = tuNgay;
            parms[2].Value = denNgay;

            if (tuNgay == DateTime.MinValue) parms[1].Value = DBNull.Value;
            if (denNgay == DateTime.MinValue) parms[2].Value = DBNull.Value;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_DONGIANTIEP, parms))
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

        #region phần xử lý vụ việc phức tạp
        public IList<DTXuLyInfo> GetDTVuViecPhucTap(QueryFilterInfo info, bool flag)
        {
            IList<DTXuLyInfo> ListInfo = new List<DTXuLyInfo>();

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_KEY, SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter(PARAM_CANBOID,SqlDbType.Int)

            };

            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.KeyWord ?? "";
            parms[2].Value = info.LoaiKhieuToID;
            parms[3].Value = info.CanBoID;
            string storeProc = "";
            if (flag) storeProc = GET_DT_LAVUVIECPHUCTAP;
            else storeProc = GET_DT_LAVUVIECDANGGIAOXACMINH;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, storeProc, parms))
                {
                    while (dr.Read())
                    {
                        DTXuLyInfo Info = new DTXuLyInfo();

                        Info.DonThuID = Utils.GetInt32(dr["DonThuID"], 0);
                        Info.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);
                        Info.SoDonThu = Utils.GetString(dr["SoDonThu"], string.Empty);
                        Info.TenChuDon = Utils.GetString(dr["HoTen"], string.Empty);
                        Info.NoiDungDon = Utils.GetString(dr["NoiDungDon"], string.Empty);
                        if (Info.NoiDungDon.Length > Constant.LengthNoiDungDon)
                        {
                            Info.NoiDungDon = Info.NoiDungDon.Substring(0, Constant.LengthNoiDungDon);
                        }
                        Info.NgayNhapDon = Utils.GetDateTime(dr["NgayNhapDon"], DateTime.MinValue);
                        Info.NgayNhapDons = Format.FormatDate(Info.NgayNhapDon);
                        Info.DiaChiCT = Utils.ConvertToString(dr["DiaChiCT"], string.Empty);
                        Info.TenLoaiKhieuTo = Utils.ConvertToString(dr["TenLoaiKhieuTo"], string.Empty);
                        Info.TenCoQuan = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        Info.NhomKNID = Utils.GetInt32(dr["NhomKNID"], 0);
                        Info.NgayTiepNhan = Utils.GetDateTime(dr["NgayTiepNhan"], DateTime.MinValue);

                        if (Info.NhomKNID > 0)
                        {
                            Info.listDoiTuongKN = new DoiTuongKN().GetByNhomKNID(Info.NhomKNID).ToList();
                        }
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

        public int DanhDauLaVuViecPhucTap(List<int> lst_donThuID)

        {
            int val = 0;


            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        for (int i = 0; i < lst_donThuID.Count; i++)
                        {
                            SqlParameter[] parms = new SqlParameter[] {
                            new SqlParameter(PARAM_DONTHUID, SqlDbType.Int),
                            };

                            parms[0].Value = lst_donThuID[i];
                            SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, GET_DT_DANHDAULAVUVIECPHUCTAP, parms);
                        }
                        trans.Commit();
                        val = 1;
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


    }
}
