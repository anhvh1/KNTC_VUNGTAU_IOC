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
    public class SupperDeleteDT
    {
        private const string COUNT_DT_CQKHAC_CD = @"SupperDeleteDT_CountDTCQKhacCD";
        private const string GET_DT_CQKHAC_CD = @"SupperDeleteDT_GetDTCQKhacCD";

        private const string COUNT_SOTIEPNHAN_GIANTIEP_BTDTINH = @"SupperDeleteDT_CountSoTiepNhan_GianTiep_GetAll";
        private const string GET_SOTIEPNHAN_GIANTIEP_BTDTINH = @"SupperDeleteDT_GetSoTiepNhan_GianTiep_GetALL";
        private const string GET_DON_THU_DA_DUOC_XU_LY = "SupperDeleteDT_GetDonThuDaXuLy";

        //Ten cac bien dau vao
        private const string PARAM_XULYDON_ID = "@XuLyDonID";
        private const string PARAM_NGAYNHAPDON = "@NgayNhapDon";
        private const string PARAM_LOAIKHIEUTOID = "@LoaiKhieuToID";
        private const string PARAM_DON_THU_ID = "@DonThuID";

        private const string PARAM_KEY = "@Keyword";
        private const string PARAM_START = "@Start";
        private const string PARAM_END = "@End";

        private const string PARAM_TUNGAY = "@TuNgay";
        private const string PARAM_DENNGAY = "@DenNgay";
        private const string PARAM_DOCUMENTLIST = "@DocumentIDList";
        private const string PARAM_COQUANID = "@CoQuanID";
        private const string PARAM_STATENAME = "@StateName";
        private const string PARAM_CANBOID = "@CanBoID";
        private const string PARAM_PHONGBANID = "@PhongBanID";
        private const string PARM_TENCOQUAN = @"pTenCoQuan";

        private SuperDeleteDTInfo GetDataDTDaTiepNhan(SqlDataReader dr)
        {
            SuperDeleteDTInfo info = new SuperDeleteDTInfo();

            info.DonThuID = Utils.GetInt32(dr["DonThuID"], 0);
            info.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);
            info.SoDonThu = Utils.GetString(dr["SoDonThu"], string.Empty);
            info.NguonDonDen = Utils.GetInt32(dr["NguonDonDen"], 0);
            info.StateName = Utils.GetString(dr["StateName"], String.Empty);

            info.MaHoSoMotCua = Utils.ConvertToString(dr["MaHoSoMotCua"], String.Empty);
            info.SoBienNhanMotCua = Utils.ConvertToString(dr["SoBienNhanMotCua"], String.Empty);

            if (info.NguonDonDen == (int)EnumNguonDonDen.TrucTiep)
            {
                info.NguonDonDens = Constant.NguonDon_TrucTieps;
            }
            if (info.NguonDonDen == (int)EnumNguonDonDen.CoQuanKhac)
            {
                info.NguonDonDens = Constant.NguonDon_CoQuanKhacs;
                if (info.MaHoSoMotCua != string.Empty)
                {
                    info.NguonDonDens = "Liên thông một cửa";
                }
            }
            if (info.NguonDonDen == (int)EnumNguonDonDen.BuuChinh)
            {
                info.NguonDonDens = Constant.NguonDon_BuuChinhs;
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

        public int Count_SoTiepNhanGianTiep_GetAll(QueryFilterInfo info)
        {
            int result = 0;

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_KEY, SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARM_TENCOQUAN,SqlDbType.Int),

            };
            parms[0].Value = info.KeyWord;
            parms[1].Value = info.LoaiKhieuToID;
            parms[2].Value = info.TuNgay;
            parms[3].Value = info.DenNgay;
            parms[4].Value = info.CoQuanID;
            //parms[4].Value = info.CanBoID;

            if (info.TuNgay == DateTime.MinValue) parms[2].Value = DBNull.Value;
            if (info.DenNgay == DateTime.MinValue) parms[3].Value = DBNull.Value;

            //if (info.CanBoID == 0) parms[4].Value = DBNull.Value;

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

        public IList<SuperDeleteDTInfo> GetSoTiepNhanGianTiep_GetALL(QueryFilterInfo info)
        {
            IList<SuperDeleteDTInfo> ListInfo = new List<SuperDeleteDTInfo>();

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_KEY, SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_START, SqlDbType.Int),
                new SqlParameter(PARAM_END, SqlDbType.Int),
                new SqlParameter(PARM_TENCOQUAN,SqlDbType.Int)
                //new SqlParameter(PARAM_CANBOID,SqlDbType.Int)
                
            };

            parms[0].Value = info.KeyWord;
            parms[1].Value = info.LoaiKhieuToID;
            parms[2].Value = info.TuNgay;
            parms[3].Value = info.DenNgay;
            parms[4].Value = info.Start;
            parms[5].Value = info.End;
            parms[6].Value = info.CoQuanID;
            //parms[6].Value = info.CanBoID;

            if (info.TuNgay == DateTime.MinValue) parms[2].Value = DBNull.Value;
            if (info.DenNgay == DateTime.MinValue) parms[3].Value = DBNull.Value;
            //if (info.CanBoID == 0) parms[6].Value = DBNull.Value;
            //SqlParameter[] parms = GetPara_Search();
            //SetPara_Search(parms, info, docList);
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_SOTIEPNHAN_GIANTIEP_BTDTINH, parms))
                {
                    while (dr.Read())
                    {
                        SuperDeleteDTInfo Info = GetDataDTDaTiepNhan(dr);
                        Info.CBDuocChonXL = Utils.ConvertToInt32(dr["CBDuocChonXL"], 0);
                        Info.QTTiepNhanDon = Utils.ConvertToInt32(dr["QTTiepNhanDon"], 0);
                        Info.DoiTuongBiKNID = Utils.ConvertToInt32(dr["DoiTuongBiKNID"], 0);
                        Info.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"], 0);
                        if (Info.NhomKNID > 0)
                        {
                            Info.listDoiTuongKN = new DoiTuongKN().GetByNhomKNID(Info.NhomKNID).ToList();
                        }
                        Info.TenCoQuan = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        Info.NgayTiepNhan = (Utils.GetDateTime(dr["NgayTiepNhan"], DateTime.MinValue)).ToShortDateString();
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

        // anhnt 10/2/2017 Supper Delete 
        private const string DELETE_SUPPER_DTTIEPNHAN = @"DonThu_Delete_Supper_DTTiepNhan";
        private const string PARM_DOITUONGBIKNID = @"pDoiTuongBiKNID";
        private const string PARM_NHOMKNID = @"pNhomKNID";
        public int Delete_AllDonThu(int DT_ID, int xuLyDonID, int doiTuongBiKNID, int nhomKNID)
        {

            int val = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_DON_THU_ID,SqlDbType.Int),
                new SqlParameter(PARAM_XULYDON_ID,SqlDbType.Int),
                new SqlParameter(PARM_DOITUONGBIKNID,SqlDbType.Int),
                new SqlParameter(PARM_NHOMKNID,SqlDbType.Int)
            };
            parameters[0].Value = DT_ID;
            parameters[1].Value = xuLyDonID;
            parameters[2].Value = doiTuongBiKNID;
            parameters[3].Value = nhomKNID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, DELETE_SUPPER_DTTIEPNHAN, parameters);
                        trans.Commit();
                    }
                    catch
                    {
                        //trans.Rollback();
                        //throw;
                    }
                }
                conn.Close();
            }

            return val;
        }

        private const string DELETE_ALL_XULYDON = @"DonThu_Delete_Supper_XuLyDon_By_XuLyDonID";
        public int Delete_AllXuLyDon(int DT_ID, int xuLyDonID)
        {

            int val = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_DON_THU_ID,SqlDbType.Int),
                new SqlParameter(PARAM_XULYDON_ID,SqlDbType.Int)
            };
            parameters[0].Value = DT_ID;
            parameters[1].Value = xuLyDonID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, DELETE_ALL_XULYDON, parameters);
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
        private const string GET_ALLXULYDONID_BY_DONTHUID = @"SupperDeleteDT_GetAllXuLyDonID_By_DonThuID";
        public List<SuperDeleteDTInfo> ListXuLyDonID(int donThuID)
        {
            List<SuperDeleteDTInfo> ListInfo = new List<SuperDeleteDTInfo>();

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_DON_THU_ID, SqlDbType.Int),

            };

            parms[0].Value = donThuID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_ALLXULYDONID_BY_DONTHUID, parms))
                {
                    while (dr.Read())
                    {
                        SuperDeleteDTInfo Info = new SuperDeleteDTInfo();
                        Info.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
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

    }
}
