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
    public class LanhDaoTiepDan
    {
        //Su dung de goi StoreProcedure


        private const string COUNT_SEARCH = @"DeXuatGapLD_CountSearch";
        private const string GET_SEARCH = @"DeXuatGapLD_GetSearch";
        private const string UPDATE = @"DeXuatGapLD_Update";

        //Ten cac bien dau vao

        private const string PARAM_CO_QUAN_ID = "@CoQuanID";
        private const string PARAM_DONGYTIEP = "@DongYTiep";
        private const string PARAM_KEY = "@Keyword";
        private const string PARAM_START = "@Start";
        private const string PARAM_END = "@End";
        private const string PARAM_DEXUATGAPLD_ID = "@DeXuatGapLDID";
        private const string PARAM_TIEPDANKHONGDONID = "@TiepDanKhongDonID";
        private const string PARAM_TUNGAY = "@TuNgay";
        private const string PARAM_DENNGAY = "@DenNgay";
        private const string PARAM_DATIEP = "@DaTiep";




        private LanhDaoTiepDanInfo GetData(SqlDataReader dr)
        {
            LanhDaoTiepDanInfo cInfo = new LanhDaoTiepDanInfo();
            cInfo.SoDonThu = Utils.ConvertToInt32(dr["SoDonThu"], 0);
            cInfo.DonThuID = Utils.ConvertToInt32(dr["DonThuID"].ToString(), 0);
            cInfo.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
            cInfo.TenChuDon = Utils.ConvertToString(dr["HoTen"], string.Empty);
            cInfo.DiaChi = Utils.ConvertToString(dr["DiaChiCT"], string.Empty);
            cInfo.NoiDungDon = Utils.ConvertToString(dr["NoiDungDon"], string.Empty);
            cInfo.TenLoaiKhieuTo = Utils.ConvertToString(dr["TenLoaiKhieuTo"], string.Empty);
            cInfo.ngay = Utils.ConvertToDateTime(dr["NgayTiep"], DateTime.MinValue);
            if (cInfo.NoiDungDon.Length > Constant.LengthNoiDungDon)
            {
                cInfo.NoiDungDon = cInfo.NoiDungDon.Substring(0, Constant.LengthNoiDungDon) + Constant.ChuoiCuoiNDDon;
            }
            if (cInfo.ngay != DateTime.MinValue)
                cInfo.NgayTiep = cInfo.ngay.ToString("dd/MM/yyyy");
            cInfo.DeXuatGapLDID = Utils.ConvertToInt32(dr["DeXuatGapLDID"], 0);
            cInfo.TiepDanKhongDonID = Utils.ConvertToInt32(dr["TiepDanKhongDonID"], 0);
            cInfo.tt = Utils.ConvertToBoolean(dr["DongYTiep"], false);

            if (cInfo.tt == false)
                cInfo.TrangThai = "Từ chối tiếp";
            else cInfo.TrangThai = "Đồng ý tiếp";

            cInfo.dt = Utils.ConvertToBoolean(dr["DaTiep"], false);
            if (cInfo.tt == true && cInfo.dt == false)
                cInfo.check = false;
            if (cInfo.tt == false)
                cInfo.check = true;
            if (cInfo.tt == true && cInfo.dt == true)
                cInfo.check = true;


            return cInfo;
        }

        public int CountSearch(QueryFilterInfo info)
        {

            int result = 0;
            SqlParameter[] parameters = new SqlParameter[]{

                new SqlParameter(PARAM_KEY,SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_DONGYTIEP,SqlDbType.Bit),
                new SqlParameter(PARAM_TUNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_CO_QUAN_ID,SqlDbType.Int)

            };
            parameters[0].Value = info.KeyWord;

            parameters[2].Value = info.TuNgay;
            parameters[3].Value = info.DenNgay;
            parameters[4].Value = info.CoQuanID;
            if (info.LoaiKhieuToID == 0)
                parameters[1].Value = DBNull.Value;
            if (info.LoaiKhieuToID == 1)
                parameters[1].Value = true;
            if (info.LoaiKhieuToID == 2)
                parameters[1].Value = false;

            if (info.TuNgay == DateTime.MinValue) parameters[2].Value = DBNull.Value;
            if (info.DenNgay == DateTime.MinValue) parameters[3].Value = DBNull.Value;

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

        public IList<LanhDaoTiepDanInfo> GetSearch(QueryFilterInfo info)
        {

            IList<LanhDaoTiepDanInfo> listGapLD = new List<LanhDaoTiepDanInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_KEY,SqlDbType.NVarChar,50),
                new SqlParameter(PARAM_DONGYTIEP,SqlDbType.Bit),
                new SqlParameter(PARAM_TUNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY,SqlDbType.DateTime),
                new SqlParameter(PARAM_CO_QUAN_ID,SqlDbType.Int),
                new SqlParameter(PARAM_START,SqlDbType.Int),
                new SqlParameter(PARAM_END,SqlDbType.Int)

            };
            parameters[0].Value = info.KeyWord;

            parameters[2].Value = info.TuNgay;
            parameters[3].Value = info.DenNgay;
            parameters[4].Value = info.CoQuanID;
            parameters[5].Value = info.Start;
            parameters[6].Value = info.End;
            if (info.LoaiKhieuToID == 0)
                parameters[1].Value = DBNull.Value;
            if (info.LoaiKhieuToID == 1)
                parameters[1].Value = true;
            if (info.LoaiKhieuToID == 2)
                parameters[1].Value = false;


            if (info.TuNgay == DateTime.MinValue) parameters[2].Value = DBNull.Value;
            if (info.DenNgay == DateTime.MinValue) parameters[3].Value = DBNull.Value;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_SEARCH, parameters))
                {
                    while (dr.Read())
                    {
                        LanhDaoTiepDanInfo cInfo = GetData(dr);
                        listGapLD.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {

                throw;
            }
            return listGapLD;
        }

        public int Update(int tiepdanid, bool datiep)
        {
            int result = 0;
            SqlParameter[] parameters = new SqlParameter[]{


                new SqlParameter(PARAM_TIEPDANKHONGDONID,SqlDbType.Int),
                new SqlParameter(PARAM_DATIEP,SqlDbType.Bit)

            };

            parameters[0].Value = tiepdanid;
            parameters[1].Value = datiep;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        result = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, UPDATE, parameters);
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
            return result;
        }


    }
}
