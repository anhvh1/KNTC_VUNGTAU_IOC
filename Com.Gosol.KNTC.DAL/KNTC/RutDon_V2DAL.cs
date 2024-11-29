using Com.Gosol.KNTC.DAL.HeThong;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.HeThong;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Ultilities;
using DocumentFormat.OpenXml.Office2010.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.DAL.KNTC
{
    public class RutDon_V2DAL
    {
        public BaseResultModel Insert(RutDon_V2Model rutDon, int canBoID)
        {
            var result = new BaseResultModel();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@LyDoRutDon",SqlDbType.NVarChar),
                new SqlParameter("@CanBoID",SqlDbType.Int),
                new SqlParameter("@XuLyDonID",SqlDbType.Int),
                new SqlParameter("@NgayRutDon",SqlDbType.Date),
            };
            parameters[0].Value = rutDon.LyDoRutDon;
            parameters[1].Value = canBoID;
            parameters[2].Value = rutDon.XuLyDonID;
            parameters[3].Value = DateTime.Now;
            using (var connection = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                connection.Open();
                using (var trans = connection.BeginTransaction())
                {
                    try
                    {
                        var query = Utils.ConvertToInt32(SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, "V2_NV_RutDon_Insert", parameters).ToString(), 0);
                        result.Status = 1;
                        result.Data = query;
                        result.Message = "Rút đơn thành công";
                        trans.Commit();
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
            if (result.Status == 1)
            {
                UpdateDocument_By_XuLyDonID(rutDon.XuLyDonID);
                if (rutDon.DanhSachHoSoTaiLieu.Count > 0)
                {
                    Insert_FileRutDon(rutDon);
                }

            }
            return result;
        }
        public ChiTietRutDon GetByXuLyDonID(int xuLyDonID)
        {
            var Result = new BaseResultModel();
            ChiTietRutDon item = new ChiTietRutDon();
            FileDinhKemDAL fileDinhKemDAL = new FileDinhKemDAL();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@XuLyDonID",SqlDbType.Int),
            };
            parameters[0].Value = xuLyDonID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "V2_NV_RutDon_GetByXuLyDonID", parameters))
                {
                    while (dr.Read())
                    {
                        item.RutDonID = Utils.ConvertToInt32(dr["RutDonID"], 0);
                        item.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
                        item.LyDoRutDon = Utils.ConvertToString(dr["LyDo"], string.Empty);
                        item.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        item.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        item.NgayCapNhap = dr["NgayRutDon"] != DBNull.Value ? Convert.ToDateTime(dr["NgayRutDon"]) : (DateTime?)null;
                    }
                    dr.Close();
                }
                
                Result.Status = 1;
                var listFileRutDon = GetlistFileRutDonID(xuLyDonID);
                
                foreach (var p in listFileRutDon)
                {
                    DanhSachHoSoTaiLieu danhSachHoSoTaiLieu = new DanhSachHoSoTaiLieu
                    {
                        FileDinhKem = new List<FileDinhKemModel>() // Khởi tạo danh sách FileDinhKem
                    };
                    var file = fileDinhKemDAL.GetByID(p, EnumLoaiFile.FileRutDon.GetHashCode());
                    file.TenFile = file.FileUrl.Substring(file.FileUrl.IndexOf("_") + 29);
                    danhSachHoSoTaiLieu.FileDinhKem.Add(file);
                    danhSachHoSoTaiLieu.TenFile = file.TenFileHeThong;
                    item.DanhSachHoSoTaiLieu.Add(danhSachHoSoTaiLieu);
                }                
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
            }
            return item;
        }
        public BaseResultModel UpdateDocument_By_XuLyDonID(int xuLyDonID)
        {
            var result = new BaseResultModel();
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@XuLyDonID",SqlDbType.Int),
                };
                parameters[0].Value = xuLyDonID;
                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            result.Status = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "V2_NV_Document_UpdateByXuLyDonID", parameters);
                            trans.Commit();
                            result.Status = 1;
                            result.Message = "Cập nhật thành công!";
                            result.Data = result.Status;
                        }
                        catch (Exception ex)
                        {
                            result.Status = -1;
                            result.Message = Constant.ERR_UPDATE;
                            trans.Rollback();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                throw;
            }
            return result;
        }
        public BaseResultModel Insert_FileRutDon(RutDon_V2Model rutDon)
        {
            var result = new BaseResultModel();
            foreach (var item in rutDon.DanhSachHoSoTaiLieu)
            {
                foreach (var item1 in item.DanhSachFileDinhKemID)
                {
                    SqlParameter[] parameters = new SqlParameter[]
                    {
                        new SqlParameter("@XuLyDonID",SqlDbType.Int),
                        new SqlParameter("@FileID",SqlDbType.Int),
                    };
                    parameters[0].Value = rutDon.XuLyDonID;
                    parameters[1].Value = item1;
                    using (var connection = new SqlConnection(SQLHelper.appConnectionStrings))
                    {
                        connection.Open();
                        using (var trans = connection.BeginTransaction())
                        {
                            try
                            {
                                var query = Utils.ConvertToInt32(SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, "V2_NV_FileRutDon_Insert", parameters).ToString(), 0);
                                result.Status = 1;
                                result.Data = query;
                                result.Message = " thành công";
                                trans.Commit();
                            }
                            catch (Exception)
                            {
                                trans.Rollback();
                                throw;
                            }
                        }
                    }
                }

            }
            return result;
        }
        public List<int> GetlistFileRutDonID(int xuLyDonID)
        {
            var list = new List<int>();
            var result = new BaseResultModel();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@XuLyDonID",SqlDbType.Int),
            };
            parameters[0].Value = xuLyDonID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "V2_NV_GetListFileRutDon_By_XuLyDonID", parameters))
                {
                    while (dr.Read())
                    {
                        var fileID = Utils.ConvertToInt32(dr["FileID"], 0);
                        list.Add(fileID);
                    }
                    dr.Close();
                }


            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.ToString();
            }
            return list;
        }
    }
}
