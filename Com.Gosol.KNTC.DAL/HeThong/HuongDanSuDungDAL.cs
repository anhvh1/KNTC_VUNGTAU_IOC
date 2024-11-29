using Com.Gosol.KNTC.Models.HeThong;
using Com.Gosol.KNTC.Ultilities;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Model.HeThong;
using static Com.Gosol.KNTC.Model.HeThong.ChucNangInfo;

namespace Com.Gosol.KNTC.DAL.HeThong
{
    public class HuongDanSuDungDAL
    {
        #region Params

        private readonly string pHuongDanSuDungID = "HuongDanSuDungID";
        private readonly string pChucNangID = "ChucNangID";
        private readonly string pTieuDe = "TieuDe";
        private readonly string pTenFileGoc = "TenFileGoc";
        private readonly string pTenFileHeThong = "TenFileHeThong";
        private readonly string pNgayCapNhat = "NgayCapNhat";
        private readonly string pNguoiCapNhat = "NguoiCapNhat";
        private readonly string pHighlight = "Highlight";
        private readonly string pTrangThai = "TrangThai";
        private readonly string pTenChucNang = "TenChucNang";
        private readonly string pChucNangChaID = "ChucNangChaID";
        private readonly string pMaChucNang = "MaChucNang";
        private readonly string pNhomID = "NhomID";
        private readonly string pKeyword = "Keyword";
        private readonly string pLimit = "Limit";
        private readonly string pOffset = "Offset";
        private readonly string pTotalRow = "TotalRow";

        #endregion

        #region StoreProcedure

        private readonly string sHuongDanSuDung_GetALL = "v2_HeThong_HuongDanSuDung_GetALL";
        private readonly string sHuongDanSuDung_GetALLChucNang = "v2_HeThong_HuongDanSuDung_GetALLChucNang";
        private readonly string sChucNang_KiemTraID = "v2_DanhMuc_ChucNang_KiemTraID";
        private readonly string sHuongDanSuDung_KiemTraChucNang = "v2_HeThong_HuongDanSuDung_KiemTraChucNang";
        private readonly string sHuongDanSuDung_Xoa = "v2_HeThong_HuongDanSuDung_Xoa";
        private readonly string sHuongDanSuDung_KiemTraTieuDe = "v2_HeThong_HuongDanSuDung_KiemTraTieuDe";


        #endregion

        public List<HuongDanSuDungModel> GetPagingBySearch(BasePagingParamsForFilter p, ref int TotalRow)
        {
            List<HuongDanSuDungModel> list = new List<HuongDanSuDungModel>();
            int ChucNangChaID = p.NhomChucNang ?? 0;
            //if (p.NhomChucNang != null && p.NhomChucNang.Length > 0)
            //{
            //    int Row = 0;
            //    BasePagingParamsForFilter cn = new BasePagingParamsForFilter();
            //    cn.PageSize = 1000;
            //    List<ChucNangModel> listChucNang = new ChucNangDAL().GetPagingBySearch(cn, ref Row);
            //    foreach (var item in listChucNang)
            //    {
            //        if (item.MaChucNang == p.NhomChucNang) ChucNangChaID = item.ChucNangID;
            //    }
            //}

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("Keyword", SqlDbType.NVarChar, 200),
                new SqlParameter("OrderByName", SqlDbType.NVarChar, 50),
                new SqlParameter("OrderByOption", SqlDbType.NVarChar, 50),
                new SqlParameter("pLimit", SqlDbType.Int),
                new SqlParameter("pOffset", SqlDbType.Int),
                new SqlParameter("TotalRow", SqlDbType.Int),
                new SqlParameter("ChucNangID", SqlDbType.Int),
                new SqlParameter("ChucNangChaID", SqlDbType.Int),
                new SqlParameter("CapID", SqlDbType.Int),
            };
            parameters[0].Value = p.Keyword != null ? p.Keyword : "";
            parameters[1].Value = p.OrderByName;
            parameters[2].Value = p.OrderByOption;
            parameters[3].Value = p.Limit;
            parameters[4].Value = p.Offset;
            parameters[5].Direction = ParameterDirection.Output;
            parameters[5].Size = 8;
            parameters[6].Value = p.ChucNangID ?? Convert.DBNull;
            parameters[7].Value = ChucNangChaID;
            parameters[8].Value = p.CapID ?? Convert.DBNull;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_HeThong_HuongDanSuDung_GetPagingBySearch_New",
                           parameters))
                {
                    while (dr.Read())
                    {
                        HuongDanSuDungModel item = new HuongDanSuDungModel();
                        item.ChucNangID = Utils.ConvertToInt32(dr["ChucNangID"], 0);
                        item.HuongDanSuDungID = Utils.ConvertToInt32(dr["HuongDanSuDungID"], 0);
                        item.NguoiCapNhat = Utils.ConvertToInt32(dr["NguoiCapNhat"], 0);
                        item.NgayCapNhat = Utils.ConvertToDateTime(dr["NgayCapNhat"], DateTime.Now);
                        item.TenFileGoc = Utils.ConvertToString(dr["TenFileGoc"], string.Empty);
                        item.TenFileHeThong = Utils.ConvertToString(dr["TenFileHeThong"], string.Empty);
                        item.TieuDe = Utils.ConvertToString(dr["TieuDe"], string.Empty);
                        item.TrangThai = Utils.ConvertToInt32(dr["TrangThai"], 0);
                        //item.ChucNangChaID = Utils.ConvertToInt32(dr["ChucNangChaID"], 0);
                        item.MaChucNang = Utils.ConvertToString(dr["MaChucNang"], string.Empty);
                        item.TenChucNang = Utils.ConvertToString(dr["TenChucNang"], string.Empty);
                        item.Link = Utils.ConvertToString(dr["Link"], string.Empty);
                        item.CapID = Utils.ConvertToInt32(dr["CapID"], 0);
                        list.Add(item);
                    }

                    dr.Close();
                }

                TotalRow = Utils.ConvertToInt32(parameters[5].Value, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return list;
        }

        public bool KiemTraChucNang(int ChucNangID)
        {

            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter("ChucNangID", ChucNangID)
                };
                var count = SQLHelper.ExecuteScalar(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, sChucNang_KiemTraID, parameters);

                if (Utils.ConvertToInt32(count, 0) > 0)
                {
                    return true;
                }
            }
            catch (Exception)
            {

                throw;
            }

            return false;
        }
        public bool KiemTraChucNangTonTai(int ChucNangID)
        {

            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter("ChucNangID", ChucNangID)
                };
                var count = SQLHelper.ExecuteScalar(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, sHuongDanSuDung_KiemTraChucNang, parameters);

                if (Utils.ConvertToInt32(count, 0) > 0)
                {
                    return true;
                }
            }
            catch (Exception)
            {

                throw;
            }

            return false;
        }

        public bool KiemTraID(int HuongDansdID)
        {

            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter(pHuongDanSuDungID, HuongDansdID)
                };
                var count = SQLHelper.ExecuteScalar(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_HeThong_HuongDanSuDung_KiemTraID", parameters);

                if (Utils.ConvertToInt32(count, 0) > 0)
                {
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return false;
        }

        public bool KiemTraTieuDeTonTai(string TieuDe, int? HuongDanSuDungID)
        {
            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter("TieuDe", TieuDe),
                    new SqlParameter("HuongDanSuDungID", HuongDanSuDungID ?? Convert.DBNull),
                };
                var count = SQLHelper.ExecuteScalar(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, sHuongDanSuDung_KiemTraTieuDe, parameters);

                if (Utils.ConvertToInt32(count, 0) > 0)
                {
                    return true;
                }
            }
            catch (Exception)
            {

                throw;
            }

            return false;
        }

        public BaseResultModel Insert(HuongDanSuDungModel HuongDanSuDungModel, int CanBoID)
        {
            var Result = new BaseResultModel();
            try
            {

                /* else if (string.IsNullOrEmpty(HuongDanSuDungModel.MaChucNang))
                 {
                     Result.Status = 0;
                     Result.Message = "Chức năng không được trống";
                     return Result;
                 }*/
                //else if (HuongDanSuDungModel.Base64String == null || HuongDanSuDungModel.Base64String.Length < 1)
                //{
                //    Result.Status = 0;
                //    Result.Message = "File hướng dẫn sử dụng không được trống";
                //    return Result;
                //}

                //var crHuongDanSuDung = GetByChucNangID(HuongDanSuDungModel.ChucNangID.Value);
                var crHuongDanSuDung = GetByMaChucNang(HuongDanSuDungModel.MaChucNang, HuongDanSuDungModel.CapID ?? 0);
                if (crHuongDanSuDung != null && crHuongDanSuDung.HuongDanSuDungID > 0)
                {
                    Result.Status = 0;
                    Result.Message = "Hướng dẫn sử dụng cho chức năng đã tồn tại";
                    return Result;
                }

                SqlParameter[] parameters = new SqlParameter[]
                {
                        new SqlParameter("@ChucNangID", SqlDbType.Int),
                        new SqlParameter("@TieuDe", SqlDbType.NVarChar),
                        new SqlParameter("@TenFileGoc", SqlDbType.NVarChar),
                        new SqlParameter("@TenFileHeThong", SqlDbType.NVarChar),
                        new SqlParameter("@NgayCapNhat", SqlDbType.DateTime),
                        new SqlParameter("@NguoiCapNhat", SqlDbType.Int),
                        new SqlParameter("@TrangThai", SqlDbType.Int),
                        new SqlParameter("@MaChucNang", SqlDbType.NVarChar),
                        new SqlParameter("@Link", SqlDbType.NVarChar),
                        new SqlParameter("@CapID", SqlDbType.Int),
                };
                parameters[0].Value = HuongDanSuDungModel.ChucNangID ?? Convert.DBNull;
                parameters[1].Value = HuongDanSuDungModel.TieuDe.Trim();
                parameters[2].Value = HuongDanSuDungModel.TenFileGoc ?? Convert.DBNull;
                parameters[3].Value = HuongDanSuDungModel.TenFileHeThong ?? Convert.DBNull;
                parameters[4].Value = DateTime.Now;
                parameters[5].Value = CanBoID;
                parameters[6].Value = 1;
                parameters[7].Value = HuongDanSuDungModel.MaChucNang ?? Convert.DBNull;
                parameters[8].Value = HuongDanSuDungModel.Link ?? Convert.DBNull;
                parameters[9].Value = HuongDanSuDungModel.CapID ?? Convert.DBNull;
                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            var query = Utils.ConvertToInt32(SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, "v1_HeThong_HuongDanSuDung_Insert", parameters), 0);
                            trans.Commit();
                            if (query > 0)
                            {
                                Result.Status = 1;
                                Result.Data = query;
                                Result.Message = ConstantLogMessage.Alert_Insert_Success("Hướng dẫn sử dụng");
                            }
                        }
                        catch (Exception ex)
                        {
                            Result.Status = -1;
                            Result.Message = ex.Message;
                            trans.Rollback();
                        }
                    }
                }
            }
            catch (Exception)
            {
                Result.Status = -1;
                Result.Message = ConstantLogMessage.API_Error_System;
                throw;
            }

            return Result;
        }

        public BaseResultModel Update(HuongDanSuDungModel HuongDanSuDungModel, int CanBoID)
        {
            var Result = new BaseResultModel();
            try
            {
                /*if (HuongDanSuDungModel == null || HuongDanSuDungModel.TieuDe == null || HuongDanSuDungModel.TieuDe.Trim().Length < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Tiêu đề không được trống";
                    return Result;
                }
                if (string.IsNullOrEmpty(HuongDanSuDungModel.MaChucNang))
                {
                    Result.Status = 0;
                    Result.Message = "Chức năng không được trống";
                    return Result;
                }*/
                //else if (HuongDanSuDungModel.Base64String == null || HuongDanSuDungModel.Base64String.Length < 1)
                //{
                //    Result.Status = 0;
                //    Result.Message = "File hướng dẫn sử dụng không được trống";
                //    return Result;
                //}

                //var crHuongDanSuDung = GetByChucNangID(HuongDanSuDungModel.ChucNangID.Value);
                /*var crHuongDanSuDung = GetByMaChucNang(HuongDanSuDungModel.MaChucNang);
                if (crHuongDanSuDung != null && crHuongDanSuDung.HuongDanSuDungID > 0 && crHuongDanSuDung.HuongDanSuDungID != HuongDanSuDungModel.HuongDanSuDungID)
                {
                    Result.Status = 0;
                    Result.Message = "Hướng dẫn sử dụng cho chức năng đã tồn tại";
                    return Result;
                }*/

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@HuongDanSuDungID", SqlDbType.Int),
                    new SqlParameter("@ChucNangID", SqlDbType.Int),
                    new SqlParameter("@TieuDe", SqlDbType.NVarChar),
                    new SqlParameter("@TenFileGoc", SqlDbType.NVarChar),
                    new SqlParameter("@TenFileHeThong", SqlDbType.NVarChar),
                    new SqlParameter("@NgayCapNhat", SqlDbType.DateTime),
                    new SqlParameter("@NguoiCapNhat", SqlDbType.Int),
                    new SqlParameter("@TrangThai", SqlDbType.Int),
                    new SqlParameter("@MaChucNang", SqlDbType.NVarChar),
                    new SqlParameter("@Link", SqlDbType.NVarChar),
                    new SqlParameter("@CapID", SqlDbType.Int),
                };
                parameters[0].Value = HuongDanSuDungModel.HuongDanSuDungID;
                parameters[1].Value = HuongDanSuDungModel.ChucNangID ?? Convert.DBNull;
                parameters[2].Value = HuongDanSuDungModel.TieuDe.Trim();
                parameters[3].Value = HuongDanSuDungModel.TenFileGoc ?? Convert.DBNull;
                parameters[4].Value = HuongDanSuDungModel.TenFileHeThong ?? Convert.DBNull;
                parameters[5].Value = DateTime.Now;
                parameters[6].Value = CanBoID;
                parameters[7].Value = 1;
                parameters[8].Value = HuongDanSuDungModel.MaChucNang ?? Convert.DBNull;
                parameters[9].Value = HuongDanSuDungModel.Link ?? Convert.DBNull;
                parameters[10].Value = HuongDanSuDungModel.CapID ?? Convert.DBNull;

                //if (HuongDanSuDungModel.Base64String == null || HuongDanSuDungModel.Base64String.Length < 1)
                //{
                //    parameters[3].Value = crHuongDanSuDung.TenFileGoc;
                //    parameters[4].Value = crHuongDanSuDung.TenFileHeThong;
                //}

                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            var query = Utils.ConvertToInt32(SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, "v1_HeThong_HuongDanSuDung_Update", parameters), 0);
                            trans.Commit();
                            if (query >= 0)
                            {
                                Result.Status = 1;
                                Result.Data = query;
                                Result.Message = ConstantLogMessage.Alert_Update_Success("Hướng dẫn sử dụng");
                            }
                        }
                        catch (Exception ex)
                        {
                            Result.Status = -1;
                            Result.Message = ConstantLogMessage.API_Error_System;
                            trans.Rollback();
                            throw;
                        }
                    }
                }

            }
            catch (Exception)
            {
                Result.Status = -1;
                Result.Message = ConstantLogMessage.API_Error_System;
                throw;
            }

            return Result;
        }

        public BaseResultModel Delete(List<int> ListHuongDanSuDungID, int CanBoID)
        {
            var Result = new BaseResultModel();
            if (ListHuongDanSuDungID.Count <= 0)
            {
                Result.Status = 0;
                Result.Message = "Vui lòng chọn dữ liệu trước khi xóa";
                return Result;
            }

            foreach (var t in ListHuongDanSuDungID)
            {
                int crID;
                if (!int.TryParse(t.ToString(), out crID))
                {
                    Result.Status = 0;
                    Result.Message = "Hướng dẫn sử dụng '" + t.ToString() + "' không đúng định dạng";
                    return Result;
                }
                else
                {
                    var crObj = GetByID(t);
                    if (crObj == null || crObj.HuongDanSuDungID == null || crObj.HuongDanSuDungID < 1)
                    {
                        Result.Status = 0;
                        Result.Message = "HuongDanSuDungID '" + t.ToString() + "' không tồn tại";
                        return Result;
                    }
                }
            }

            var pList = new SqlParameter("@DanhSachHuongDanSuDungID", SqlDbType.Structured);
            pList.TypeName = "dbo.list_ID";
            // var TrangThai = 400;
            var tbID = new DataTable();
            tbID.Columns.Add("ID", typeof(string));
            ListHuongDanSuDungID.ForEach(x => tbID.Rows.Add(x));
            SqlParameter[] parameters = new SqlParameter[]
            {
                pList
            };
            parameters[0].Value = tbID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        int val = 0;
                        val = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v1_HeThong_HuongDanSuDung_Delete", parameters);
                        trans.Commit();
                        if (val < 0)
                        {
                            Result.Status = 0;
                            Result.Message = "Không thể xóa Hướng dẫn sử dụng";
                            return Result;
                        }
                    }
                    catch
                    {
                        Result.Status = -1;
                        Result.Message = ConstantLogMessage.API_Error_System;
                        trans.Rollback();
                        return Result;
                        throw;
                    }
                }
            }

            Result.Status = 1;
            Result.Message = ConstantLogMessage.Alert_Delete_Success("Hướng dẫn sử dụng");
            return Result;
        }

        public HuongDanSuDungModel GetByID(int HuongDanSuDungID)
        {
            HuongDanSuDungModel Result = new HuongDanSuDungModel();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter(@"HuongDanSuDungID", SqlDbType.Int),
            };
            parameters[0].Value = HuongDanSuDungID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_HeThong_HuongDanSuDung_GetByID", parameters))
                {
                    while (dr.Read())
                    {
                        Result.ChucNangID = Utils.ConvertToInt32(dr["ChucNangID"], 0);
                        Result.HuongDanSuDungID = Utils.ConvertToInt32(dr["HuongDanSuDungID"], 0);
                        Result.NguoiCapNhat = Utils.ConvertToInt32(dr["NguoiCapNhat"], 0);
                        Result.NgayCapNhat = Utils.ConvertToDateTime(dr["NgayCapNhat"], DateTime.Now);
                        Result.TenFileGoc = Utils.ConvertToString(dr["TenFileGoc"], string.Empty);
                        Result.TenFileHeThong = Utils.ConvertToString(dr["TenFileHeThong"], string.Empty);
                        Result.TieuDe = Utils.ConvertToString(dr["TieuDe"], string.Empty);
                        Result.TrangThai = Utils.ConvertToInt32(dr["TrangThai"], 0);
                        Result.ChucNangChaID = dr[pChucNangChaID] == DBNull.Value ? null : Utils.ConvertToInt32(dr[pChucNangChaID], 0);
                        Result.TenChucNang = Utils.ConvertToString(dr[pTenChucNang], "");
                        Result.MaChucNang = Utils.ConvertToString(dr[pMaChucNang], "");
                        Result.Link = Utils.ConvertToString(dr["Link"], string.Empty);
                        Result.CapID = Utils.ConvertToInt32(dr["CapID"], 0);
                        break;
                    }

                    dr.Close();
                }
            }
            catch
            {
                throw;
            }

            return Result;
        }

        public HuongDanSuDungModel GetByMaChucNang(string MaChucNang, int CapID)
        {
            HuongDanSuDungModel Result = new HuongDanSuDungModel();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter(@"MaChucNang", SqlDbType.NVarChar),
                new SqlParameter(@"CapID", SqlDbType.Int),
            };
            parameters[0].Value = MaChucNang ?? Convert.DBNull;
            parameters[1].Value = CapID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_HeThong_HuongDanSuDung_GetByMaChucNang",
                           parameters))
                {
                    while (dr.Read())
                    {
                        //Result.ChucNangID = Utils.ConvertToInt32(dr["ChucNangID"], 0);
                        Result.HuongDanSuDungID = Utils.ConvertToInt32(dr["HuongDanSuDungID"], 0);
                        Result.NguoiCapNhat = Utils.ConvertToInt32(dr["NguoiCapNhat"], 0);
                        Result.NgayCapNhat = Utils.ConvertToDateTime(dr["NgayCapNhat"], DateTime.Now);
                        Result.TenFileGoc = Utils.ConvertToString(dr["TenFileGoc"], string.Empty);
                        Result.TenFileHeThong = Utils.ConvertToString(dr["TenFileHeThong"], string.Empty);
                        Result.TieuDe = Utils.ConvertToString(dr["TieuDe"], string.Empty);
                        Result.TrangThai = Utils.ConvertToInt32(dr["TrangThai"], 0);
                        Result.MaChucNang = Utils.ConvertToString(dr["MaChucNang"], string.Empty);
                        Result.Link = Utils.ConvertToString(dr["Link"], string.Empty);
                        break;
                    }

                    dr.Close();
                }
            }
            catch
            {
                throw;
            }

            return Result;
        }

        public List<HuongDanSuDungModel> GetByChucNangID(int ChucNangID)
        {
            List<HuongDanSuDungModel> Result = new List<HuongDanSuDungModel>();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter(@"ChucNangID", SqlDbType.NVarChar),
            };
            parameters[0].Value = ChucNangID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_HeThong_HuongDanSuDung_GetByChucNangID",
                           parameters))
                {
                    while (dr.Read())
                    {
                        var item = new HuongDanSuDungModel();
                        item.ChucNangID = Utils.ConvertToInt32(dr["ChucNangID"], 0);
                        item.HuongDanSuDungID = Utils.ConvertToInt32(dr["HuongDanSuDungID"], 0);
                        item.NguoiCapNhat = Utils.ConvertToInt32(dr["NguoiCapNhat"], 0);
                        item.NgayCapNhat = Utils.ConvertToDateTime(dr["NgayCapNhat"], DateTime.Now);
                        item.TenFileGoc = Utils.ConvertToString(dr["TenFieGoc"], string.Empty);
                        item.TenFileHeThong = Utils.ConvertToString(dr["TenFileHeThong"], string.Empty);
                        item.TieuDe = Utils.ConvertToString(dr["TieuDe"], string.Empty);
                        item.TrangThai = Utils.ConvertToInt32(dr["TrangThai"], 0);
                        item.Link = Utils.ConvertToString(dr["Link"], string.Empty);
                        Result.Add(item);
                        break;
                    }

                    dr.Close();
                }
            }
            catch
            {
                throw;
            }

            return Result;
        }

        public BaseResultModel DanhSach(HuongDanSuDungParams thamSo, string rootPath)
        {
            BaseResultModel Result = new BaseResultModel();

            SqlParameter[] parameters =
            {
                new SqlParameter(pKeyword, SqlDbType.NVarChar),
                new SqlParameter(pNhomID, SqlDbType.Int),
                new SqlParameter(pTrangThai, SqlDbType.Bit),
                new SqlParameter(pOffset, SqlDbType.Int),
                new SqlParameter(pLimit, SqlDbType.Int),
                new SqlParameter(pTotalRow, SqlDbType.Int)
            };

            int offset = thamSo.PageNumber < 1 ? 0 : (thamSo.PageNumber - 1) * thamSo.PageSize;

            parameters[0].Value = thamSo.Keyword ?? Convert.DBNull;
            parameters[1].Value = thamSo.NhomID ?? Convert.DBNull;
            parameters[2].Value = thamSo.Status ?? Convert.DBNull;
            parameters[3].Value = offset;
            parameters[4].Value = thamSo.PageSize >= 1 ? thamSo.PageSize : 1;
            parameters[5].Direction = ParameterDirection.Output;

            List<HuongDanSuDungModel_v2> Data = new List<HuongDanSuDungModel_v2>();

            try
            {
                using SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_HeThong_HuongDanSuDung_GetALL", parameters);
                while (dr.Read())
                {
                    Data.Add(new HuongDanSuDungModel_v2
                    {
                        HuongDanSuDungID = Utils.ConvertToInt32(dr[pHuongDanSuDungID], 0),
                        ChucNangID = Utils.ConvertToInt32(dr[pChucNangID], 0),
                        TieuDe = Utils.ConvertToString(dr[pTieuDe], ""),
                        TenFileGoc = Utils.ConvertToString(dr[pTenFileGoc], ""),
                        TenFileHeThong = Utils.ConvertToString(dr[pTenFileHeThong], ""),
                        NgayCapNhat = Utils.ConvertToDateTime_Hour(dr[pNgayCapNhat], DateTime.MinValue),
                        NguoiCapNhat = Utils.ConvertToString(dr["TenCanBo"], ""),
                        TrangThai = Utils.ConvertToInt32(dr[pTrangThai], 0) == 1,
                        TenChucNang = Utils.ConvertToString(dr[pTenChucNang], ""),
                        ChucNangChaID = dr[pChucNangChaID] == DBNull.Value ? null : Utils.ConvertToInt32(dr[pChucNangChaID], 0),
                        MaChucNang = Utils.ConvertToString(dr[pMaChucNang], ""),
                        Link = rootPath + Utils.ConvertToString(dr[pTenFileHeThong], "")
                    });
                }

                dr.Close();
            }
            catch (Exception)
            {
                throw;
            }

            var totalCount = Utils.ConvertToInt32(parameters[5].Value, 0);

            Result.TotalRow = totalCount;
            Result.Status = 1;
            Result.Message = totalCount == 0 || Data.Count == 0 ? Constant.NO_DATA : "Thành công";

            Result.Data = Data;
            return Result;
        }
        public BaseResultModel ChiTiet(int HuongDanSuDungID, string rootPath)
        {
            BaseResultModel Result = new BaseResultModel();

            SqlParameter[] parameters =
            {
                new SqlParameter(pHuongDanSuDungID, SqlDbType.Int)
            };

            parameters[0].Value = HuongDanSuDungID;


            HuongDanSuDungModel_v2 Data = null;

            try
            {
                using SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_HeThong_HuongDanSuDung_ChiTiet", parameters);
                while (dr.Read())
                {
                    Data = new HuongDanSuDungModel_v2
                    {
                        HuongDanSuDungID = Utils.ConvertToInt32(dr[pHuongDanSuDungID], 0),
                        ChucNangID = Utils.ConvertToInt32(dr[pChucNangID], 0),
                        TieuDe = Utils.ConvertToString(dr[pTieuDe], ""),
                        TenFileGoc = Utils.ConvertToString(dr[pTenFileGoc], ""),
                        TenFileHeThong = Utils.ConvertToString(dr[pTenFileHeThong], ""),
                        NgayCapNhat = Utils.ConvertToDateTime_Hour(dr[pNgayCapNhat], DateTime.MinValue),
                        NguoiCapNhat = Utils.ConvertToString(dr["TenCanBo"], ""),
                        TrangThai = Utils.ConvertToInt32(dr[pTrangThai], 0) == 1,
                        TenChucNang = Utils.ConvertToString(dr[pTenChucNang], ""),
                        ChucNangChaID = dr[pChucNangChaID] == DBNull.Value ? null : Utils.ConvertToInt32(dr[pChucNangChaID], 0),
                        MaChucNang = Utils.ConvertToString(dr[pMaChucNang], ""),
                        Link = rootPath + Utils.ConvertToString(dr[pTenFileHeThong], "")
                    };
                }

                dr.Close();
            }
            catch (Exception)
            {
                throw;
            }

            Result.TotalRow = 1;
            Result.Status = Data == null ? 0 : 1;
            Result.Message = Data == null ? Constant.NO_DATA : "Thành công";

            Result.Data = Data;
            return Result;
        }
        public BaseResultModel Xoa(int HuongDanSuDungID)
        {
            BaseResultModel Result = new BaseResultModel();

            SqlParameter[] parameters =
            {
                new SqlParameter(pHuongDanSuDungID, SqlDbType.Int),

            };
            parameters[0].Value = HuongDanSuDungID;

            using var con = new SqlConnection(SQLHelper.appConnectionStrings);
            con.Open();
            using var trans = con.BeginTransaction();
            try
            {
                SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, sHuongDanSuDung_Xoa, parameters);

                Result.Status = 1;
                Result.Message = Constant.MSG_SUKNTCESS;
                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
            return Result;
        }

        public BaseResultModel DanhSachChucNang()
        {
            BaseResultModel Result = new BaseResultModel();

            SqlParameter[] parameters =
            {
            };



            List<ChucNangHDSDModel> Data = new List<ChucNangHDSDModel>();

            try
            {
                using SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, sHuongDanSuDung_GetALLChucNang, parameters);
                while (dr.Read())
                {
                    Data.Add(new ChucNangHDSDModel
                    {
                        ChucNangID = Utils.ConvertToInt32(dr["ChucNangID"], 0),
                        TenChucNang = Utils.ConvertToString(dr["TenChucNang"], ""),
                        Highlight = Utils.ConvertToInt32(dr["Highlight"], 0) == 1
                    });
                }

                dr.Close();
            }
            catch (Exception)
            {
                throw;
            }



            Result.TotalRow = Data.Count;
            Result.Status = 1;
            Result.Message = Data.Count == 0 ? Constant.NO_DATA : "Thành công";

            Result.Data = Data;
            return Result;
        }

        public List<MenuModel> GetListMenuByNguoiDungID(int NguoidungID, int CapID)
        {
            List<MenuModel> Result = new List<MenuModel>();
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(@"NguoiDungID", SqlDbType.Int),
                new SqlParameter(@"CapID", SqlDbType.Int),
            };
            parameters[0].Value = NguoidungID;
            parameters[1].Value = CapID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_HeThong_HuongDanSuDung_GetMenuByNguoiDungID", parameters))
                {

                    while (dr.Read())
                    {
                        MenuModel item = new MenuModel();
                        item.MenuID = Utils.ConvertToInt32(dr["ChucNangID"], 0);
                        item.TenMenu = Utils.ConvertToString(dr["TenChucNang"], string.Empty);
                        item.MaMenu = Utils.ConvertToString(dr["MaChucNang"], String.Empty);
                        item.MenuChaID = Utils.ConvertToInt32(dr["ChucNangChaID"], 0);
                        item.ThuTuSapXep = Utils.ConvertToInt32(dr["ThuTuSapXep"], 0);
                        item.Icon = Utils.ConvertToString(dr["Icon"], String.Empty);
                        item.HienThi = Utils.ConvertToBoolean(dr["HienThiTrenMenu"], false);
                        int SL = Utils.ConvertToInt32(dr["SL"], 0);
                        if (SL > 0)
                        {
                            item.ViewOnly = false;
                        }
                        else
                        {
                            item.ViewOnly = true;
                        }

                        Result.Add(item);
                    }
                    dr.Close();

                }
                Result.ForEach(x => x.Children = Result.Where(y => y.MenuChaID == x.MenuID).ToList());
                if (Result.Any(x => x.Children.Count > 0))
                {
                    Result.RemoveAll(x => x.MenuChaID != 0);
                }
            }
            catch
            {
                throw;
            }
            return Result;
        }
    }
}