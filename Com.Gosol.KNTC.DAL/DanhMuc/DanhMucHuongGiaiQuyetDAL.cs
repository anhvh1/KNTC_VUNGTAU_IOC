using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.DanhMuc;
using Com.Gosol.KNTC.Ultilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

//Create by TienKM 13/10/2022

namespace Com.Gosol.KNTC.DAL.DanhMuc
{
    public class DanhMucHuongGiaiQuyetDAL
    {
        #region Variable

        private readonly string pHuongGiaiQuyetID = "HuongGiaiQuyetID";
        private readonly string pTenHuongGiaiQuyet = "TenHuongGiaiQuyet";
        private readonly string pMaHuongGiaiQuyet = "MaHuongGiaiQuyet";
        private readonly string pGhiChu = "GhiChu";
        private readonly string pTrangThai = "TrangThai";
        private readonly string pLimit = "Limit";
        private readonly string pOffset = "Offset";
        private const string pTotalRow = "TotalRow";

        private readonly string sDanhSachHuongGiaiQuyet = "v2_DanhMuc_HuongGiaiQuyet_GetAll";
        private readonly string sKiemTraTenTonTai = "v2_DanhMuc_HuongGiaiQuyet_KiemTenTraTonTai";
        private readonly string sThemMoi = "v2_DanhMuc_HuongGiaiQuyet_ThemMoi";
        private readonly string sXoaHuongGiaiQuyet = "v2_DanhMuc_HuongGiaiQuyet_Xoa";
        private readonly string sSuaHuongGiaiQuyet = "v2_DanhMuc_HuongGiaiQuyet_Sua";
        private readonly string sHuongGiaiQuyetChiTiet = "v2_DanhMuc_HuongGiaiQuyet_ChiTiet";
        private readonly string sHuongGiaiQuyet_CountXuLyDon = "v2_DanhMuc_HuongGiaiQuyet_CountXuLyDon";

        #endregion

        #region Function

        public BaseResultModel DanhSach(ThamSoLocDanhMuc thamSoLocDanhMuc)
        {
            var Result = new BaseResultModel();
            List<DanhMucHuongGiaiQuyetModel> Data = new();

            SqlParameter[] parameters = {
                new SqlParameter(pTenHuongGiaiQuyet, SqlDbType.NVarChar),
                new SqlParameter(pTrangThai, SqlDbType.Bit),
                new SqlParameter(pOffset, SqlDbType.Int),
                new SqlParameter(pLimit, SqlDbType.Int),
                new SqlParameter(pTotalRow, SqlDbType.Int),
            };

            int offset;
            if (thamSoLocDanhMuc.PageNumber < 1)
            {
                offset = 0;
            }
            else
            {
                offset = (thamSoLocDanhMuc.PageNumber - 1) * thamSoLocDanhMuc.PageSize;
            }

            parameters[0].Value = thamSoLocDanhMuc.Keyword ?? Convert.DBNull;
            parameters[1].Value = thamSoLocDanhMuc.Status ?? Convert.DBNull;
            parameters[2].Value = offset;
            parameters[3].Value = thamSoLocDanhMuc.PageSize;
            parameters[4].Direction= ParameterDirection.Output;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, this.sDanhSachHuongGiaiQuyet, parameters))
                {
                    while (dr.Read())
                    {
                        Data.Add(new DanhMucHuongGiaiQuyetModel
                        {
                            HuongGiaiQuyetID = Utils.ConvertToInt32(dr[pHuongGiaiQuyetID], 0),
                            TenHuongGiaiQuyet = Utils.ConvertToString(dr[pTenHuongGiaiQuyet], string.Empty),
                            MaHuongGiaiQuyet = Utils.ConvertToString(dr[pMaHuongGiaiQuyet], string.Empty),
                            GhiChu = Utils.ConvertToString(dr[pGhiChu], string.Empty),
                            TrangThai = Utils.ConvertToBoolean(dr[pTrangThai], false)
                        });
                    }

                    dr.Close();
                }

                Result.TotalRow = Convert.ToInt32(parameters[4].Value);
                Result.Status = 1;
                Result.Data = Data;
                Result.Message = "Thanh cong";
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
            }

            return Result;
        }

        public BaseResultModel ChiTiet(int HuongGiaiQuyetID)
        {
            var Result = new BaseResultModel();
            DanhMucHuongGiaiQuyetModel Data = null;

            SqlParameter[] parameters = {
                new SqlParameter(pHuongGiaiQuyetID, SqlDbType.Int)
            };
            parameters[0].Value = HuongGiaiQuyetID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, this.sHuongGiaiQuyetChiTiet, parameters))
                {
                    while (dr.Read())
                    {
                        Data = new DanhMucHuongGiaiQuyetModel
                        {
                            HuongGiaiQuyetID = Utils.ConvertToInt32(dr[pHuongGiaiQuyetID], 0),
                            TenHuongGiaiQuyet = Utils.ConvertToString(dr[pTenHuongGiaiQuyet], string.Empty),
                            MaHuongGiaiQuyet = Utils.ConvertToString(dr[pMaHuongGiaiQuyet], string.Empty),
                            GhiChu = Utils.ConvertToString(dr[pGhiChu], string.Empty),
                            TrangThai = Utils.ConvertToBoolean(dr[pTrangThai], false)
                        };
                        break;
                    }

                    dr.Close();
                }

                Result.TotalRow = 1;
                Result.Status = 1;
                if (Data == null)
                {
                    Result.Message = "Không có dữ liệu";
                }
                else
                {
                    Result.Message = "Thành công";
                    Result.Data = Data;
                }
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
            }

            return Result;
        }

        public bool KiemTraTenTonTai(string tenHuongGiaiQuyet,string maHuongGiaiQuyet, int? HuongGiaiQuyetID)
        {
            SqlParameter[] parameters = {
                new SqlParameter(pHuongGiaiQuyetID, SqlDbType.Int),
                new SqlParameter(pTenHuongGiaiQuyet, SqlDbType.NVarChar),
                new SqlParameter(pMaHuongGiaiQuyet, SqlDbType.NVarChar),
            };

            parameters[0].Value = HuongGiaiQuyetID ?? Convert.DBNull;
            parameters[1].Value = tenHuongGiaiQuyet;
            parameters[1].Value = maHuongGiaiQuyet;

            try
            {
                var rowCount = SQLHelper.ExecuteScalar(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, sKiemTraTenTonTai, parameters);
                if (Convert.ToInt32(rowCount) > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
            }

            return false;
        }
        

        public BaseResultModel XoaHuongGiaiQuyet(int HuongGiaiQuyetID)
        {
            var Result = new BaseResultModel();

            SqlParameter[] parameters = {
                new SqlParameter(pHuongGiaiQuyetID, SqlDbType.Int),
            };
            parameters[0].Value = HuongGiaiQuyetID;

            using (var connection = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                connection.Open();
                using (var trans = connection.BeginTransaction())
                {
                    try
                    {
                        var count = SQLHelper.ExecuteScalar(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, sHuongGiaiQuyet_CountXuLyDon , new []
                        {
                            new SqlParameter(pHuongGiaiQuyetID, HuongGiaiQuyetID)
                        });

                        if (Convert.ToInt32(count) > 0)
                        {
                            Result.Message = "Huớng giải quyết chứa dữ liệu không được xóa";
                            Result.Status = 0;
                            return Result;
                        }
                        
                        var effected = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, sXoaHuongGiaiQuyet, parameters);
                        trans.Commit();

                        if (effected == 0)
                        {
                            Result.Message = "ID không tồn tại";
                            Result.Status = 0;
                        }
                        else
                        {
                            Result.Status = 1;
                            Result.Message = "Xóa thành công";
                        }
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return Result;
        }

        public BaseResultModel ThemMoi(ThemDanhMucHuongGiaiQuyetModel huongGiaiQuyet)
        {
            var Result = new BaseResultModel();

            if (KiemTraTenTonTai(huongGiaiQuyet.TenHuongGiaiQuyet.Trim(),huongGiaiQuyet.MaHuongGiaiQuyet.Trim(), null))
            {
                Result.Status = -1;
                Result.Message = "Tên, Mã hướng giải quuyết đã tồn tại";
                return Result;
            }

            SqlParameter[] parameters = {
                new SqlParameter(pTenHuongGiaiQuyet, SqlDbType.NVarChar),
                new SqlParameter(pMaHuongGiaiQuyet, SqlDbType.VarChar),
                new SqlParameter(pGhiChu, SqlDbType.NVarChar),
                new SqlParameter(pTrangThai, SqlDbType.Bit)
            };

            parameters[0].Value = huongGiaiQuyet.TenHuongGiaiQuyet;
            parameters[1].Value = huongGiaiQuyet.MaHuongGiaiQuyet.Trim() ?? Convert.DBNull;
            parameters[2].Value = huongGiaiQuyet.GhiChu.Trim() ?? Convert.DBNull;
            parameters[3].Value = huongGiaiQuyet.TrangThai ?? Convert.DBNull;


            using (var connection = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                connection.Open();
                using (var trans = connection.BeginTransaction())
                {
                    try
                    {
                        var effected = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, sThemMoi, parameters);
                        Result.Data = effected;
                        trans.Commit();
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            Result.Status = 1;
            Result.Message = "Thêm mới hướng giải quyết thành công";

            return Result;
        }

        public BaseResultModel SuaHuongGiaiQuyet(DanhMucHuongGiaiQuyetModel huongGiaiQuyet)
        {
            var Result = new BaseResultModel();

            if (KiemTraTenTonTai(huongGiaiQuyet.TenHuongGiaiQuyet.Trim(),huongGiaiQuyet.MaHuongGiaiQuyet.Trim(), huongGiaiQuyet.HuongGiaiQuyetID))
            {
                Result.Status = -1;
                Result.Message = "Tên, Mã hướng giải quyết đã tồn tại";
                return Result;
            }

            SqlParameter[] parameters = {
                new SqlParameter(pHuongGiaiQuyetID, huongGiaiQuyet.HuongGiaiQuyetID),
                new SqlParameter(pTenHuongGiaiQuyet, SqlDbType.NVarChar),
                new SqlParameter(pMaHuongGiaiQuyet, SqlDbType.VarChar),
                new SqlParameter(pGhiChu, SqlDbType.NVarChar),
                new SqlParameter(pTrangThai, SqlDbType.Bit)
            };
            parameters[0].Value = huongGiaiQuyet.HuongGiaiQuyetID;
            parameters[1].Value = huongGiaiQuyet.TenHuongGiaiQuyet.Trim();
            parameters[2].Value = huongGiaiQuyet.MaHuongGiaiQuyet.Trim() ?? Convert.DBNull;
            parameters[3].Value = huongGiaiQuyet.GhiChu.Trim() ?? Convert.DBNull;
            parameters[4].Value = huongGiaiQuyet.TrangThai ?? Convert.DBNull;


            using var connection = new SqlConnection(SQLHelper.appConnectionStrings);
            connection.Open();
            using (var trans = connection.BeginTransaction())
            {
                try
                {
                    var effected = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, sSuaHuongGiaiQuyet, parameters);
                    Result.Data = effected;
                    trans.Commit();
                    if (effected == 0)
                    {
                        Result.Status = 0;
                        Result.Message = "ID không tồn tại";
                    }
                    else
                    {
                        Result.Status = 1;
                        Result.Message = "Cập nhật hướng giải quyết thành công";
                    }
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }
            }

            return Result;
        }

        #endregion
    }
}