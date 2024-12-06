using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Ultilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Gosol.KNTC.Models.BaoCao;

namespace Com.Gosol.KNTC.DAL.BaoCao
{
    public class DongBo_IOCDAL
    {
        public long Insert_One(ThongKeBC_2a_DongBo_IOC_Request item, int nguoiDungId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("ThongKeBC_2a_IOC_Insert", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    // Add parameters
                    var parameters = new Dictionary<string, object>
                    {
                        {"@CoQuanId", item.CoQuanId ?? (object)DBNull.Value},
                        {"@TongSoLuotTCD", item.TongSoLuotTCD ?? (object)DBNull.Value},
                        {"@TongSoLuotTTX", item.TongSoLuotTTX ?? (object)DBNull.Value},
                        {"@TongSoLuotTTT", item.TongSoLuotTTT ?? (object)DBNull.Value},
                        {"@TongSoLuotUQT", item.TongSoLuotUQT ?? (object)DBNull.Value},
                        {"@Thang", item.Thang ?? (object)DBNull.Value},
                        {"@Nam", item.Nam ?? (object)DBNull.Value},
                        {"@NguoiTao", nguoiDungId},
                    };

                    DataAccessHelper.SetSqlCommandParameters(cmd, parameters);

                    // Execute and get the returned ID
                    var insertedId = Convert.ToInt64(cmd.ExecuteScalar());

                    return insertedId;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public BaseResultModel Insert_BC_2a(List<ThongKeBC_2a_DongBo_IOC_Request> item, int nguoiDungId)
        {
            var result = new BaseResultModel();
            foreach (var model in item)
            {
                var check = Insert_One(model, nguoiDungId);
                if (check <= 0)
                {
                    result.Status = 0;
                    result.Message = "Lỗi trong quá trình insert";
                    return result;
                }
            }
            result.Status = 1;
            result.Message = "Thêm mới thành công";
            return result;
        }
        public long Update_One(ThongKeBC_2a_DongBo_IOC_UpdateRequest item, int nguoiDungId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("ThongKeBC_2a_IOC_Update", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    // Add parameters
                    var parameters = new Dictionary<string, object>
                    {
                        {"@Id", item.Id},
                        {"@TongSoLuotTCD", item.TongSoLuotTCD ?? (object)DBNull.Value},
                        {"@TongSoLuotTTX", item.TongSoLuotTTX ?? (object)DBNull.Value},
                        {"@TongSoLuotTTT", item.TongSoLuotTTT ?? (object)DBNull.Value},
                        {"@TongSoLuotUQT", item.TongSoLuotUQT ?? (object)DBNull.Value},
                        {"@Thang", item.Thang ?? (object)DBNull.Value},
                        {"@Nam", item.Nam ?? (object)DBNull.Value},
                        {"@NguoiSua",nguoiDungId},
                    };

                    DataAccessHelper.SetSqlCommandParameters(cmd, parameters);

                    // Execute and get the returned ID
                    var updatedId = Convert.ToInt64(cmd.ExecuteScalar());

                    return updatedId;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public BaseResultModel Update_BC_2a(List<ThongKeBC_2a_DongBo_IOC_UpdateRequest> item, int nguoiDungId)
        {
            var result = new BaseResultModel();
            foreach (var model in item)
            {
                var check = Update_One(model, nguoiDungId);
                if (check <= 0)
                {
                    result.Status = 0;
                    result.Message = "Lỗi trong quá trình cập nhật";
                    return result;
                }
            }
            result.Status = 1;
            result.Message = "Cập nhật thành công";
            return result;
        }
        public List<ThongKeBC_2a_DongBo_IOC> GetListBySearch(FilterDongBo_IOC p)
        {
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("ThongKeBC_2a_IOC_GetList", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                var parameters = new Dictionary<string, object>
                {
                    {"@Thang", p.Thang ?? (object)DBNull.Value},
                    {"@Nam", p.Nam ?? (object)DBNull.Value},
                };
                DataAccessHelper.SetSqlCommandParameters(cmd, parameters);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    var result = DataAccessHelper.MapToList<ThongKeBC_2a_DongBo_IOC>(reader);
                    return result;
                }
            }
        }

        public long Insert_2b_One(ThongKeBC_2b_DongBo_IOC_Request model, int nguoiDungId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("ThongKeBC_2b_IOC_Insert", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    // Add parameters
                    var parameters = new Dictionary<string, object>
                    {
                        {"@CoQuanId", model.CoQuanId ?? (object)DBNull.Value},
                        {"@TongSoDonXLD", model.TongSoDonXLD ?? (object)DBNull.Value},
                        {"@TSDXLDThuocThamQuyen", model.TSDXLDThuocThamQuyen ?? (object)DBNull.Value},
                        {"@TSDXLDKhongThuocThamQuyen", model.TSDXLDKhongThuocThamQuyen ?? (object)DBNull.Value},
                        {"@TSDXLDToCao", model.TSDXLDToCao ?? (object)DBNull.Value},
                        {"@TSDXLDToCaoThuocThamQuyen", model.TSDXLDToCaoThuocThamQuyen ?? (object)DBNull.Value},
                        {"@TSDXLDKhieuNai", model.TSDXLDKhieuNai ?? (object)DBNull.Value},
                        {"@TSDXLDKhieuNaiThuocThamQuyen", model.TSDXLDKhieuNaiThuocThamQuyen ?? (object)DBNull.Value},
                        {"@TSDXLDKienNghiPhanAnh", model.TSDXLDKienNghiPhanAnh ?? (object)DBNull.Value},
                        {"@TSDXLDKienNghiPhanAnhTTQ", model.TSDXLDKienNghiPhanAnhTTQ ?? (object)DBNull.Value},
                        {"@Thang", model.Thang ?? (object)DBNull.Value},
                        {"@Nam", model.Nam ?? (object)DBNull.Value},
                        {"@NguoiTao", nguoiDungId}
                    };


                    DataAccessHelper.SetSqlCommandParameters(cmd, parameters);

                    // Execute and get the returned ID
                    var insertedId = Convert.ToInt64(cmd.ExecuteScalar());

                    return insertedId;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public BaseResultModel Insert_BC_2b(List<ThongKeBC_2b_DongBo_IOC_Request> item, int nguoiDungId)
        {
            var result = new BaseResultModel();
            foreach (var model in item)
            {
                var check = Insert_2b_One(model, nguoiDungId);
                if (check <= 0)
                {
                    result.Status = 0;
                    result.Message = "Lỗi trong quá trình insert";
                    return result;
                }
            }
            result.Status = 1;
            result.Message = "Thêm mới thành công";
            return result;
        }
        public long Update_2b_One(ThongKeBC_2b_DongBo_IOC_UpdateRequest model, int nguoiDungId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("ThongKeBC_2b_IOC_Update", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    // Add parameters
                    var parameters = new Dictionary<string, object>
                    {
                        {"@Id", model.Id},
                        {"@TongSoDonXLD", model.TongSoDonXLD ?? (object)DBNull.Value},
                        {"@TSDXLDThuocThamQuyen", model.TSDXLDThuocThamQuyen ?? (object)DBNull.Value},
                        {"@TSDXLDKhongThuocThamQuyen", model.TSDXLDKhongThuocThamQuyen ?? (object)DBNull.Value},
                        {"@TSDXLDToCao", model.TSDXLDToCao ?? (object)DBNull.Value},
                        {"@TSDXLDToCaoThuocThamQuyen", model.TSDXLDToCaoThuocThamQuyen ?? (object)DBNull.Value},
                        {"@TSDXLDKhieuNai", model.TSDXLDKhieuNai ?? (object)DBNull.Value},
                        {"@TSDXLDKhieuNaiThuocThamQuyen", model.TSDXLDKhieuNaiThuocThamQuyen ?? (object)DBNull.Value},
                        {"@TSDXLDKienNghiPhanAnh", model.TSDXLDKienNghiPhanAnh ?? (object)DBNull.Value},
                        {"@TSDXLDKienNghiPhanAnhTTQ", model.TSDXLDKienNghiPhanAnhTTQ ?? (object)DBNull.Value},
                        {"@Thang", model.Thang ?? (object)DBNull.Value},
                        {"@Nam", model.Nam ?? (object)DBNull.Value},
                        {"@NguoiSua",nguoiDungId},
                    };

                    DataAccessHelper.SetSqlCommandParameters(cmd, parameters);

                    // Execute and get the returned ID
                    var updatedId = Convert.ToInt64(cmd.ExecuteScalar());

                    return updatedId;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public BaseResultModel Update_BC_2b(List<ThongKeBC_2b_DongBo_IOC_UpdateRequest> item, int nguoiDungId)
        {
            var result = new BaseResultModel();
            foreach (var model in item)
            {
                var check = Update_2b_One(model, nguoiDungId);
                if (check <= 0)
                {
                    result.Status = 0;
                    result.Message = "Lỗi trong quá trình cập nhật";
                    return result;
                }
            }
            result.Status = 1;
            result.Message = "Cập nhật thành công";
            return result;
        }
        public List<ThongKeBC_2b_DongBo_IOC> GetListBySearch_BC2b(FilterDongBo_IOC p)
        {
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("ThongKeBC_2b_IOC_GetList", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                var parameters = new Dictionary<string, object>
                {
                    {"@Thang", p.Thang ?? (object)DBNull.Value},
                    {"@Nam", p.Nam ?? (object)DBNull.Value},
                };
                DataAccessHelper.SetSqlCommandParameters(cmd, parameters);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    var result = DataAccessHelper.MapToList<ThongKeBC_2b_DongBo_IOC>(reader);
                    return result;
                }
            }
        }
    }
}
