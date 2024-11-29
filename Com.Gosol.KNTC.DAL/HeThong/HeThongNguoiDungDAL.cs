using Com.Gosol.KNTC.DAL.DanhMuc;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.DanhMuc;
using Com.Gosol.KNTC.Models.HeThong;
using Com.Gosol.KNTC.Security;
using Com.Gosol.KNTC.Ultilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Com.Gosol.KNTC.DAL.HeThong
{
    public class HeThongNguoiDungDAL
    {
        // Insert người dùng
        public int Insert(HeThongNguoiDungModel HeThongNguoiDungModel, ref string Message)
        {
            int val = 0;
            if (HeThongNguoiDungModel.CanBoID <= 0)
            {
                Message = "Không được để trống cán bộ";
                return val;
            }
            var NguoiDungByCanBoID = GetByCanBoID(HeThongNguoiDungModel.CanBoID.Value);
            if (NguoiDungByCanBoID.NguoiDungID > 0)
            {
                Message = "Cán bộ đã có tài khoản";
                return val;
            }
            int CoQuanSuDungPhanMemID = new DanhMucCoQuanDonViDAL().GetByID(HeThongNguoiDungModel.CoQuanID).CoQuanSuDungPhanMemID;
            var NguoiDung = GetByName(HeThongNguoiDungModel.TenNguoiDung, CoQuanSuDungPhanMemID);
            if (NguoiDung.NguoiDungID > 0)
            {
                Message = "Tên đăng nhập đã tồn tại";
                return val;
            }
            if (HeThongNguoiDungModel.TenNguoiDung.Trim().Length > 50)
            {
                Message = ConstantLogMessage.API_Error_TooLong;
                return val;

            }
            if (string.IsNullOrEmpty(HeThongNguoiDungModel.TenNguoiDung) || HeThongNguoiDungModel.TenNguoiDung.Trim().Length <= 0)
            {
                Message = ConstantLogMessage.API_Error_NotFill;
                return val;

            }
            if (HeThongNguoiDungModel.CanBoID != 0)
            {
                var CanBo = new HeThongCanBoDAL().GetCanBoByID(HeThongNguoiDungModel.CanBoID);
                if (CanBo == null || CanBo.MaCB == null)
                {
                    Message = "Cán bộ không tồn tai";
                    return val;
                }
            }
            if (NguoiDung.NguoiDungID > 0)
            {
                Message = ConstantLogMessage.Alert_Error_Exist("tên người dùng");
                return val;
            }
            var matKhauMacDinh = new SystemConfigDAL().GetByKey("MatKhau_MacDinh").ConfigValue;
            HeThongNguoiDungModel.MatKhau = Cryptor.EncryptPasswordUser(HeThongNguoiDungModel.TenNguoiDung.Trim().ToLower(), matKhauMacDinh ?? "123456");
            SqlParameter[] parameters = new SqlParameter[]
              {
                            new SqlParameter("@TenNguoiDung", SqlDbType.NVarChar),
                            new SqlParameter("@MatKhau", SqlDbType.NVarChar),
                            new SqlParameter("@GhiChu", SqlDbType.NVarChar),
                            new SqlParameter("@TrangThai", SqlDbType.Int),
                            new SqlParameter("@CanBoID", SqlDbType.Int),
                            new SqlParameter("@PublicKeys", SqlDbType.NVarChar),

              };
            parameters[0].Value = HeThongNguoiDungModel.TenNguoiDung.Trim().ToLower();
            parameters[1].Value = HeThongNguoiDungModel.MatKhau.Trim();
            parameters[2].Value = HeThongNguoiDungModel.GhiChu ?? Convert.DBNull;
            parameters[3].Value = HeThongNguoiDungModel.TrangThai ?? Convert.DBNull;
            parameters[4].Value = HeThongNguoiDungModel.CanBoID ?? Convert.DBNull;
            parameters[5].Value = HeThongNguoiDungModel.PublicKeys ?? Convert.DBNull;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, @"v1_HeThong_NguoiDung_Insert", parameters);
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                    Message = ConstantLogMessage.Alert_Insert_Success("người dùng");
                    //new DotKeKhaiDAL().GetDotKeKhaiFitForCanBo(HeThongNguoiDungModel.CanBoID.Value, HeThongNguoiDungModel.CoQuanID.Value);
                    return val;
                }
            }
        }

        // Update
        public int Update(HeThongNguoiDungModel HeThongNguoiDungModel, ref string Message)
        {

            int val = 0;
            if (HeThongNguoiDungModel.CanBoID <= 0)
            {
                Message = "Không được để trống cán bộ";
                return val;
            }
            //var NguoiDungByCanBoName = GetAll().Where(x => x.CanBoID == HeThongNguoiDungModel.CanBoID).ToList().Count;
            //if (NguoiDungByCanBoName > 0)
            //{
            //    Message = "Một cán bộ chỉ có một người dùng ! Thử lại!";
            //    return val;
            //}
            var NguoiDungByCanBoID = GetByCanBoID(HeThongNguoiDungModel.CanBoID.Value);
            if (NguoiDungByCanBoID.CanBoID == HeThongNguoiDungModel.CanBoID && NguoiDungByCanBoID.NguoiDungID != HeThongNguoiDungModel.NguoiDungID)
            {
                Message = "Cán bộ đã có tài khoản";
                return val;
            }
            int CoQuanSuDungPhanMemID = new DanhMucCoQuanDonViDAL().GetByID(HeThongNguoiDungModel.CoQuanID).CoQuanSuDungPhanMemID;
            var NguoiDung = GetByName(HeThongNguoiDungModel.TenNguoiDung, CoQuanSuDungPhanMemID);
            if (NguoiDung.NguoiDungID != HeThongNguoiDungModel.NguoiDungID && NguoiDung.TenNguoiDung == HeThongNguoiDungModel.TenNguoiDung)
            {
                Message = "Tên đăng nhập đã tồn tại";
                return val;
            }
            if (HeThongNguoiDungModel.NguoiDungID == 0)
            {
                Message = "Chưa có người dùng được chọn";
                return val;
            }
            if (HeThongNguoiDungModel.TenNguoiDung.Trim().Length > 50)
            {
                Message = ConstantLogMessage.API_Error_TooLong;
                return val;

            }
            if (string.IsNullOrEmpty(HeThongNguoiDungModel.TenNguoiDung) || HeThongNguoiDungModel.TenNguoiDung.Trim().Length <= 0)
            {
                Message = ConstantLogMessage.API_Error_NotFill;
                return val;

            }
            //if (!Utils.CheckSpecialCharacter(HeThongNguoiDungModel.TenNguoiDung))
            //{
            //    Message = ConstantLogMessage.API_Error_NotSpecialCharacter;
            //    return val;

            //}
            if (HeThongNguoiDungModel.CanBoID != null)
            {
                var CanBo = new HeThongCanBoDAL().GetCanBoByID(HeThongNguoiDungModel.CanBoID.Value);
                if (CanBo == null)
                {
                    Message = ConstantLogMessage.Alert_Error_NotFill("Cán bộ");
                    return val;
                }
            }
            var NguoiDung1 = GetByName(HeThongNguoiDungModel.TenNguoiDung, CoQuanSuDungPhanMemID);
            if (NguoiDung1.NguoiDungID > 0 && NguoiDung1.NguoiDungID != HeThongNguoiDungModel.NguoiDungID)
            {
                Message = ConstantLogMessage.Alert_Error_Exist("tên đăng nhập");
                return val;
            }
            //if (HeThongNguoiDungModel.CanBoID == null)
            //{
            //    var NguoiDungByCanBoID1 = GetAll().Where(x => x.CanBoID == null || x.CanBoID == 0).ToList().Count();
            //    if (NguoiDungByCanBoID1 > 0)
            //    {
            //        Message = "Không được để trống cán bộ! Thử lại!";
            //        return val;
            //    }
            //}
            //var NguoiDungByCanBoID = GetAll().Where(x => x.CanBoID == HeThongNguoiDungModel.CanBoID).ToList().Count();
            //if (NguoiDungByCanBoID > 0)
            //{
            //    Message = "Người dùng đã có cán bộ sở hữu !Thử lại!";
            //    return val;
            //}
            //MD5 mh = MD5.Create();
            //byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes("123456");
            //byte[] hash = mh.ComputeHash(inputBytes);
            //StringBuilder sb = new StringBuilder();

            //for (int i = 0; i < hash.Length; i++)
            //{
            //    sb.Append(hash[i].ToString("X2"));
            //}
            //string matkhau = Cryptor.EncryptPasswordUser(HeThongNguoiDungModel.TenNguoiDung.Trim(), HeThongNguoiDungModel.MatKhau.Trim());
            var matKhauMacDinh = new SystemConfigDAL().GetByKey("MatKhau_MacDinh").ConfigValue;
            SqlParameter[] parameters = new SqlParameter[]
              {
                            new SqlParameter("@NguoiDungID", SqlDbType.Int),
                            new SqlParameter("@TenNguoiDung", SqlDbType.NVarChar),
                            new SqlParameter("@GhiChu", SqlDbType.NVarChar),
                            new SqlParameter("@TrangThai", SqlDbType.Int),
                            new SqlParameter("@CanBoID", SqlDbType.Int),
                            new SqlParameter("@PublicKeys", SqlDbType.NVarChar),
                            new SqlParameter("@MatKhau", SqlDbType.NVarChar),


              };
            parameters[0].Value = HeThongNguoiDungModel.NguoiDungID;
            parameters[1].Value = HeThongNguoiDungModel.TenNguoiDung.Trim().ToLower();
            parameters[2].Value = HeThongNguoiDungModel.GhiChu ?? Convert.DBNull;
            parameters[3].Value = HeThongNguoiDungModel.TrangThai ?? Convert.DBNull;
            parameters[4].Value = HeThongNguoiDungModel.CanBoID ?? Convert.DBNull;
            parameters[5].Value = HeThongNguoiDungModel.PublicKeys ?? Convert.DBNull;
            parameters[6].Value = Cryptor.EncryptPasswordUser(HeThongNguoiDungModel.TenNguoiDung.Trim().ToLower(), matKhauMacDinh ?? "123456");
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, @"v1_HeThong_NguoiDung_Update", parameters);
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                    Message = ConstantLogMessage.Alert_Update_Success("Cán bộ");
                    return val;
                }
            }

        }

        // Delete
        public List<string> Delete(List<int> ListNguoiDungID, ref int Status)
        {
            List<string> dic = new List<string>();
            string message = "";
            if (ListNguoiDungID.Count <= 0)
            {
                message = ConstantLogMessage.API_Error_NotExist;
                Status = 0;
                dic.Add(message);
                return dic;
            }
            else
            {
                for (int i = 0; i < ListNguoiDungID.Count; i++)
                {
                    int val = 0;
                    var PhanQuyenByNguoiDung = new PhanQuyenDAL().NguoiDung_NhomNguoiDung_GetByNguoiDungID(ListNguoiDungID[i]);
                    if (PhanQuyenByNguoiDung.Count > 0)
                    {
                        message = "Người dùng đang được sử dụng ở chức năng khác!";
                        dic.Add(message);
                        Status = 0;
                        return dic;
                    }
                    //var CanBoByNguoiDung = new HeThongCanBoDAL().GetCanBoByNguoiDungID(ListNguoiDungID[i]);
                    //var KeKhaiByCanBo = new KeKhaiDAL().GetAll().Where(x => x.CanBoID == CanBoByNguoiDung.CanBoID).ToList();
                    if (GetByID(ListNguoiDungID[i]) == null)
                    {
                        message = ConstantLogMessage.API_Error_NotExist;
                        dic.Add(message);
                        Status = 0;
                        return dic;
                    }
                    //else if (KeKhaiByCanBo.Count > 0)
                    //{
                    //    dic.Add(GetByID(ListNguoiDungID[i]).TenNguoiDung + " đang có đợt kê khai. Không thể xóa!");
                    //    Status = 0;
                    //    return dic;
                    //}
                    //else if (GetByID(ListNguoiDungID[i]).TrangThai.Value == 1)
                    //{

                    //    dic.Add(GetByID(ListNguoiDungID[i]).TenNguoiDung + " đang được sử dụng. Không thể xóa!");
                    //    //return dic;
                    //}
                    else
                    {
                        SqlParameter[] parameters = new SqlParameter[]
                        {
                            new SqlParameter("@NguoiDungID", SqlDbType.Int)

                        };
                        parameters[0].Value = ListNguoiDungID[i];
                        using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                        {
                            conn.Open();
                            using (SqlTransaction trans = conn.BeginTransaction())
                            {
                                try
                                {
                                    val = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, @"v1_HeThong_NguoiDung_Delete", parameters);
                                    trans.Commit();
                                    message = GetByID(ListNguoiDungID[i]).TenNguoiDung + " xóa thành công.";
                                    Status = 1;
                                    dic.Add(message);
                                }
                                catch
                                {
                                    trans.Rollback();
                                    throw;
                                }


                            }
                        }


                    }
                }


            }
            return dic;

        }

        //public List<DanhMuKNTChucVuModel> GetAll()

        // Get By id
        public HeThongNguoiDungModel GetByID(int NguoiDungID)
        {
            HeThongNguoiDungModel nguoidung = new HeThongNguoiDungModel();
            SqlParameter[] parameters = new SqlParameter[]
              {
                new SqlParameter("@NguoiDungID",SqlDbType.Int)
              };
            parameters[0].Value = NguoiDungID;
            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v2_HeThong_Nguoidung_GetByID", parameters))
                {
                    while (dr.Read())
                    {
                        nguoidung = new HeThongNguoiDungModel(Utils.ConvertToInt32(dr["NguoiDungID"], 0), Utils.ConvertToString(dr["TenNguoiDung"], string.Empty), Utils.ConvertToString(dr["MatKhau"], string.Empty), Utils.ConvertToString(dr["GhiChu"], string.Empty), Utils.ConvertToInt32(dr["TrangThai"], 0), Utils.ConvertToInt32(dr["CanBoID"], 0), Utils.ConvertToString(dr["PublicKeys"], string.Empty), Utils.ConvertToInt32(dr["CoQuanID"], 0));
                        //nguoidung.DoiMatKhauLanDau = Utils.ConvertToBoolean(dr["PasswordChanged"], false);
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return nguoidung;
        }

        public NguoiDungModel GetByIDForPhanQuyen(int NguoiDungID)
        {
            NguoiDungModel nguoidung = new NguoiDungModel();
            SqlParameter[] parameters = new SqlParameter[]
              {
                new SqlParameter("@NguoiDungID",SqlDbType.Int)
              };
            parameters[0].Value = NguoiDungID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v2_HeThong_Nguoidung_GetByIDForPhanQuyen", parameters))
                {
                    while (dr.Read())
                    {
                        nguoidung = new NguoiDungModel();
                        nguoidung.TenNguoiDung = Utils.ConvertToString(dr["TenNguoiDung"], "");
                        nguoidung.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], "");
                        nguoidung.NguoiDungID = Utils.ConvertToInt32(dr["NguoiDungID"], 0);
                        nguoidung.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        nguoidung.CapCoQuan = Utils.ConvertToInt32(dr["CapID"], 0);
                        nguoidung.TenCoQuan = Utils.ConvertToString(dr["TenCoQuan"], "");
                        //nguoidung.VaiTro = Utils.ConvertToInt32(dr["VaiTro"], 0);
                        //nguoidung.AnhHoSo = Utils.ConvertToString(dr["AnhHoSo"], "");
                        //nguoidung.DoiMatKhauLanDau = Utils.ConvertToBoolean(dr["PasswordChanged"], false);
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return nguoidung;
        }
        //Get By Name
        public HeThongNguoiDungModel GetByName_Old(string TenNguoiDung, int CoQuanSunDungPhanMemID)
        {
            HeThongNguoiDungModel NguoiDung = new HeThongNguoiDungModel();
            SqlParameter[] parameters = new SqlParameter[]
              {
                new SqlParameter("@TenNguoiDung",SqlDbType.NVarChar),
                new SqlParameter("@CoQuanSuDungPhamMemID",SqlDbType.Int),
              };
            parameters[0].Value = TenNguoiDung.Trim();
            parameters[1].Value = CoQuanSunDungPhanMemID;
            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_HeThong_Nguoidung_GetByName", parameters))
                {
                    while (dr.Read())
                    {
                        NguoiDung = new HeThongNguoiDungModel(Utils.ConvertToInt32(dr["NguoiDungID"], 0), Utils.ConvertToString(dr["TenNguoiDung"], string.Empty), Utils.ConvertToString(dr["MatKhau"], string.Empty), Utils.ConvertToString(dr["GhiChu"], string.Empty), Utils.ConvertToInt32(dr["TrangThai"], 0), Utils.ConvertToInt32(dr["CanBoID"], 0), Utils.ConvertToString(dr["PublicKeys"], string.Empty), 0);


                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return NguoiDung;
        }

        public HeThongNguoiDungModel GetByName(string TenNguoiDung, int CoQuanSunDungPhanMemID)
        {
            HeThongNguoiDungModel NguoiDung = new HeThongNguoiDungModel();
            SqlParameter[] parameters = new SqlParameter[]
              {
                new SqlParameter("@TenNguoiDung",SqlDbType.NVarChar)
              };
            parameters[0].Value = TenNguoiDung.Trim();
            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v2_HeThong_Nguoidung_GetByName", parameters))
                {
                    while (dr.Read())
                    {
                        NguoiDung = new HeThongNguoiDungModel(Utils.ConvertToInt32(dr["NguoiDungID"], 0), Utils.ConvertToString(dr["TenNguoiDung"], string.Empty), Utils.ConvertToString(dr["MatKhau"], string.Empty), Utils.ConvertToString(dr["GhiChu"], string.Empty), Utils.ConvertToInt32(dr["TrangThai"], 0), Utils.ConvertToInt32(dr["CanBoID"], 0), Utils.ConvertToString(dr["PublicKeys"], string.Empty), 0);
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return NguoiDung;
        }
        public HeThongNguoiDungModel GetByEmailOrTenNguoiDung(string EmailUser)
        {
            HeThongNguoiDungModel NguoiDung = new HeThongNguoiDungModel();
            SqlParameter[] parameters = new SqlParameter[]
              {
                new SqlParameter("@EmailUser",SqlDbType.NVarChar),
              };
            parameters[0].Value = EmailUser.Trim();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v2_HeThong_Nguoidung_GetByEmailUser", parameters))
                {
                    while (dr.Read())
                    {
                        NguoiDung.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        NguoiDung.NguoiDungID = Utils.ConvertToInt32(dr["NguoiDungID"], 0);
                        NguoiDung.Email = Utils.ConvertToString(dr["Email"], string.Empty);
                        NguoiDung.TenNguoiDung = Utils.ConvertToString(dr["TenNguoiDung"], string.Empty);
                        NguoiDung.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return NguoiDung;
        }
        // Get list Paging
        public List<object> GetPagingBySearch(BasePagingParams p, ref int TotalRow, int? CoQuanID, int? TrangThai)
        {

            List<object> list = new List<object>();
            var listCanBo = new DanhMucCoQuanDonViDAL().GetListByUser(CoQuanID.Value, 1);
            SqlParameter[] parameters = new SqlParameter[]
          {
                new SqlParameter("@Keyword",SqlDbType.NVarChar,200),
                new SqlParameter("@OrderByName",SqlDbType.NVarChar,50),
                new SqlParameter("@OrderByOption",SqlDbType.NVarChar,50),
                new SqlParameter("@pLimit",SqlDbType.Int),
                new SqlParameter("@pOffset",SqlDbType.Int),
                new SqlParameter("@TotalRow",SqlDbType.Int),
                 new SqlParameter("@CoQuanID",SqlDbType.Int),
                  new SqlParameter("@TrangThai",SqlDbType.Int)

          };
            parameters[0].Value = p.Keyword == null ? "" : p.Keyword.Trim();
            parameters[1].Value = p.OrderByName;
            parameters[2].Value = p.OrderByOption;
            parameters[3].Value = p.Limit;
            parameters[4].Value = p.Offset;
            parameters[5].Direction = ParameterDirection.Output;
            parameters[5].Size = 8;
            parameters[6].Value = CoQuanID ?? Convert.DBNull;
            parameters[7].Value = TrangThai ?? Convert.DBNull;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, @"v1_HeThong_NguoiDung_GetPagingBySearch", parameters))
                {
                    while (dr.Read())
                    {
                        HeThongNguoiDungModelPartial NguoiDungModel = new HeThongNguoiDungModelPartial();
                        NguoiDungModel.NguoiDungID = Utils.ConvertToInt32(dr["NguoiDungID"], 0);
                        NguoiDungModel.TenNguoiDung = Utils.ConvertToString(dr["TenNguoiDung"], string.Empty);
                        NguoiDungModel.MatKhau = Utils.ConvertToString(dr["MatKhau"], string.Empty);
                        NguoiDungModel.GhiChu = Utils.ConvertToString(dr["GhiChu"], string.Empty);
                        NguoiDungModel.TrangThai = Utils.ConvertToInt32(dr["TrangThai"], 0);
                        NguoiDungModel.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        NguoiDungModel.PublicKeys = Utils.ConvertToString(dr["PublicKeys"], string.Empty);
                        NguoiDungModel.TenCoQuan = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        NguoiDungModel.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        NguoiDungModel.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        list.Add(NguoiDungModel);
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

        public List<object> GetPagingBySearch_New(BasePagingParamsForFilter p, ref int TotalRow, int NguoiDungID, int? CanBoID, int? CoQuanID)
        {
            //new HeThongCanBoDAL().GetListCanBo_Expire(CanBoID.Value, CoQuanID.Value);
            List<object> list = new List<object>();
            List<DanhMucCoQuanDonViModel> listCoQuanCon = new List<DanhMucCoQuanDonViModel>();
            if (UserRole.CheckAdmin(NguoiDungID))
            {
                listCoQuanCon = new DanhMucCoQuanDonViDAL().GetAllCapCon(0).ToList();
            }
            else
            {
                listCoQuanCon = new DanhMucCoQuanDonViDAL().GetAllCapCon(CoQuanID.Value);
            }
            var pList = new SqlParameter("@DanhSachCoQuan", SqlDbType.Structured);
            pList.TypeName = "dbo.list_ID";
            // var TrangThai = 400;
            var tbCoQuanID = new DataTable();
            tbCoQuanID.Columns.Add("ID", typeof(string));
            listCoQuanCon.ForEach(x => tbCoQuanID.Rows.Add(x.CoQuanID));
            SqlParameter[] parameters = new SqlParameter[]
          {
                new SqlParameter("@Keyword",SqlDbType.NVarChar,200),
                new SqlParameter("@OrderByName",SqlDbType.NVarChar,50),
                new SqlParameter("@OrderByOption",SqlDbType.NVarChar,50),
                new SqlParameter("@pLimit",SqlDbType.Int),
                new SqlParameter("@pOffset",SqlDbType.Int),
                new SqlParameter("@TotalRow",SqlDbType.Int),
                new SqlParameter("@CoQuanID",SqlDbType.Int),
                new SqlParameter("@TrangThai",SqlDbType.Int),
                   pList,

          };
            parameters[0].Value = p.Keyword == null ? "" : p.Keyword.Trim();
            parameters[1].Value = p.OrderByName;
            parameters[2].Value = p.OrderByOption;
            parameters[3].Value = p.Limit;
            parameters[4].Value = p.Offset;
            parameters[5].Direction = ParameterDirection.Output;
            parameters[5].Size = 8;
            parameters[6].Value = p.CoQuanID ?? Convert.DBNull;
            parameters[7].Value = p.TrangThai ?? Convert.DBNull;
            parameters[8].Value = tbCoQuanID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, @"v1_HeThong_NguoiDung_GetPagingBySearch", parameters))
                {
                    while (dr.Read())
                    {
                        HeThongNguoiDungModelPartial NguoiDungModel = new HeThongNguoiDungModelPartial();
                        NguoiDungModel.NguoiDungID = Utils.ConvertToInt32(dr["NguoiDungID"], 0);
                        NguoiDungModel.TenNguoiDung = Utils.ConvertToString(dr["TenNguoiDung"], string.Empty);
                        NguoiDungModel.MatKhau = Utils.ConvertToString(dr["MatKhau"], string.Empty);
                        NguoiDungModel.GhiChu = Utils.ConvertToString(dr["GhiChu"], string.Empty);
                        NguoiDungModel.TrangThai = Utils.ConvertToInt32(dr["TrangThai"], 0);
                        NguoiDungModel.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        NguoiDungModel.PublicKeys = Utils.ConvertToString(dr["PublicKeys"], string.Empty);
                        NguoiDungModel.TenCoQuan = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        NguoiDungModel.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        NguoiDungModel.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        list.Add(NguoiDungModel);
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
        //GetAll
        public List<HeThongNguoiDungModel> GetAll()
        {
            List<HeThongNguoiDungModel> list = new List<HeThongNguoiDungModel>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, @"v1_HeThong_NguoiDung_GetAll"))
                {
                    while (dr.Read())
                    {
                        HeThongNguoiDungModel nguoidung = new HeThongNguoiDungModel(Utils.ConvertToInt32(dr["NguoiDungID"], 0), Utils.ConvertToString(dr["TenNguoiDung"], string.Empty), Utils.ConvertToString(dr["MatKhau"], string.Empty), Utils.ConvertToString(dr["GhiChu"], string.Empty), Utils.ConvertToInt32(dr["TrangThai"], 0), Utils.ConvertToInt32(dr["CanBoID"], 0), Utils.ConvertToString(dr["PublicKeys"], string.Empty), Utils.ConvertToInt32(dr["CoQuanID"], 0));
                        list.Add(nguoidung);

                    }

                    dr.Close();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }


            return list;
        }
        //Reset Password
        public Dictionary<int, string> ResetPassword(int NguoiDungID)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            int val = 0;
            if (NguoiDungID <= 0)
            {
                dic.Add(0, ConstantLogMessage.API_Error_NotSelected);
                return dic;
            }
            var NguoiDung = GetByID(NguoiDungID);
            if (NguoiDung == null)
            {
                dic.Add(0, ConstantLogMessage.API_Error_NotSelected);
                return dic;
            }
            var matKhauMacDinh = new SystemConfigDAL().GetByKey("MAT_KHAU_MAC_DINH").ConfigValue;
            NguoiDung.MatKhau = Utils.HashFile(Encoding.ASCII.GetBytes(matKhauMacDinh ?? "123456")).ToUpper();
            //NguoiDung.MatKhau = Cryptor.EncryptPasswordUser("", matKhauMacDinh ?? "123456");
            string Message = null;
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
             {
                            new SqlParameter("@NguoiDungID", SqlDbType.Int),
                            new SqlParameter("@MatKhau", SqlDbType.NVarChar),
                            new SqlParameter("@PasswordChanged", SqlDbType.Bit),
             };
                parameters[0].Value = NguoiDung.NguoiDungID;
                parameters[1].Value = NguoiDung.MatKhau;
                parameters[2].Value = NguoiDung.DoiMatKhauLanDau;
                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            val = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, @"v1_HeThong_NguoiDung_ResetPassword", parameters);
                            trans.Commit();
                            val = val == 0 ? 1 : 1;
                        }
                        catch
                        {
                            trans.Rollback();
                            val = 0;
                            Message = ConstantLogMessage.API_Error;
                            throw;
                        }
                        Message = "Reset mật khẩu thành công";
                    }
                }
            }
            catch (Exception)
            {
                val = 0;
                Message = ConstantLogMessage.API_Error;
                throw;
            }
            dic.Add(val, Message);
            return dic;
        }

        //Send mail reset password
        public BaseResultModel SendMail_old(HeThongNguoiDungModelPartial p)
        {
            BaseResultModel BaseResultModel = new BaseResultModel();
            string TenTaiKhoan = p.TenNguoiDung;
            string Url = p.Url;
            int CoQuanSuDungPhanMemID = new DanhMucCoQuanDonViDAL().GetByID(p.CoQuanID).CoQuanSuDungPhanMemID;
            var NguoiDung = new HeThongNguoiDungDAL().GetByName(TenTaiKhoan.Trim().ToLower(), CoQuanSuDungPhanMemID);
            if (NguoiDung.NguoiDungID <= 0 || NguoiDung == null)
            {
                BaseResultModel.Message = "Tên người dùng không tồn tại!";
                BaseResultModel.Status = 0;
                return BaseResultModel;
            }
            if (string.IsNullOrEmpty(TenTaiKhoan))
            {
                BaseResultModel.Message = "Chưa nhập tên tài khoản!";
                BaseResultModel.Status = 0;
                return BaseResultModel;
            }
            else
            {
                var CanBoByNguoiDungID = new HeThongCanBoDAL().GetCanBoByNguoiDungID(NguoiDung.NguoiDungID);
                try
                {
                    var admin = new SystemConfigDAL().GetByKey("Mail_QuanTri").ConfigValue.ToString().Split(";");
                    string tkadmin = admin[0];
                    string mkadmin = admin[1];
                    var credentials = new NetworkCredential(tkadmin, mkadmin);
                    //var MaDuocGui = GetAllQuenMatKhau().OrderByDescending(x => x.QuenMatKhauID).ToList();
                    int LastMaDuocGui = 0;
                    Random oRandom = new Random();
                    LastMaDuocGui = oRandom.Next(1000, 99999);
                    if (string.IsNullOrEmpty(CanBoByNguoiDungID.Email.Trim()))
                    {
                        BaseResultModel.Message = "Tài khoản chưa có mail liên kết!";
                        BaseResultModel.Status = 0;
                        return BaseResultModel;
                    }
                    var MaEnscript = Cryptor.EncryptPasswordUser(TenTaiKhoan, LastMaDuocGui.ToString());
                    var Mail = new MailMessage()
                    {
                        From = new MailAddress(tkadmin, "Quản lý chấm công"),
                        Subject = "Thay đổi mật khẩu",
                        Body = ("<p>" + "Xin chào " + "<strong>" + NguoiDung.TenNguoiDung + "</strong>" + "!" + "</p>" +
                              "<p>" + "Tài khoản của bạn tại" + " <a style=" + "color:Blue;" + ">" + "Phần mềm chấm công " + "</a>" + "đã được yêu cầu đặt lại mật khẩu." + "</p>"
                               + "<p>" + "Truy cập đường dẫn : " + Url + "?Token=" + MaEnscript + " để đổi mật khẩu." + "</p>" +
                                "<p>" + "Trân Trọng," + "</p>" +
                                "Kê khai tài sản Team ").ToString()
                    };
                    Mail.IsBodyHtml = true;
                    //Mail.From = new MailAddress("kekhaitaisan.team@gmail.com", "Kê khai tài sản Team");
                    Mail.To.Add(new MailAddress(CanBoByNguoiDungID.Email.Trim()));
                    var client = new SmtpClient()
                    {
                        Port = 587,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Host = "smtp.gmail.com",
                        EnableSsl = true,
                        Credentials = credentials

                    };
                    client.Timeout = 30000;
                    client.Send(Mail);
                    int val = 0;
                    bool TrangThai = true;
                    QuenMatKhauModel QuenMatKhauModel = new QuenMatKhauModel();
                    SqlParameter[] parameters = new SqlParameter[]
                {
                            new SqlParameter("@TaiKhoan", SqlDbType.NVarChar),
                            new SqlParameter("@MaDuocGui", SqlDbType.NVarChar),
                            new SqlParameter("@ThoiGianGui", SqlDbType.DateTime),
                              new SqlParameter("@TrangThai", SqlDbType.Bit),
                                  new SqlParameter("@Email", SqlDbType.NVarChar),
                };
                    parameters[0].Value = TenTaiKhoan.Trim();
                    parameters[1].Value = MaEnscript;
                    parameters[2].Value = DateTime.Now;
                    parameters[3].Value = Utils.ConvertToBoolean(TrangThai, true);
                    parameters[4].Value = CanBoByNguoiDungID.Email.Trim();

                    using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                    {
                        conn.Open();
                        using (SqlTransaction trans = conn.BeginTransaction())
                        {
                            try
                            {
                                val = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, @"v1_HeThong_InsertQuenMatKhau", parameters);
                                trans.Commit();
                            }
                            catch
                            {
                                trans.Rollback();
                                throw;
                            }

                        }
                    }
                    BaseResultModel.Message = "Yêu cầu đổi mật khẩu của bạn đã được xử lý! Bạn nhớ kiểm tra mail để tiếp tục các bước còn lại ! Cảm ơn!";
                    BaseResultModel.Status = 1;
                    return BaseResultModel;

                }
                catch (Exception ex)
                {
                    BaseResultModel.Status = 0;
                    BaseResultModel.Message = "Vào link này để check quyền truy cập cho ứng dụng kém an toàn !" + "https://myaKNTCount.google.com/lesssecureapps?pli=1";
                    return BaseResultModel;
                }
            }
        }
        public BaseResultModel SendMail(HeThongNguoiDungModelPartial p)
        {
            BaseResultModel BaseResultModel = new BaseResultModel();
            string TenNguoiDung = p.TenNguoiDung;
            if (string.IsNullOrEmpty(TenNguoiDung))
            {
                BaseResultModel.Message = "Chưa nhập tên tài khoản!";
                BaseResultModel.Status = 0;
                return BaseResultModel;
            }
            var NguoiDung = new HeThongNguoiDungDAL().GetByEmailOrTenNguoiDung(TenNguoiDung.Trim().ToLower());
            if (NguoiDung.NguoiDungID <= 0 || NguoiDung == null)
            {
                BaseResultModel.Message = "Tài khoản không tồn tại";
                BaseResultModel.Status = 0;
                return BaseResultModel;
            }
            //else
            //{
            //    if (string.IsNullOrEmpty(NguoiDung.Email))
            //    {
            //        BaseResultModel.Message = "Tài khoản không có Email";
            //        BaseResultModel.Status = 0;
            //        return BaseResultModel;
            //    }
            //}
            try
            {
                var admin = new SystemConfigDAL().GetByKey("Mail_QuanTri").ConfigValue.ToString().Split(";");
                string tkadmin = admin[0];
                string mkadmin = admin[1];
                var credentials = new NetworkCredential(tkadmin, mkadmin);
                //var MaDuocGui = GetAllQuenMatKhau().OrderByDescending(x => x.QuenMatKhauID).ToList();
                int LastMaDuocGui = 0;
                Random oRandom = new Random();
                LastMaDuocGui = oRandom.Next(1000, 99999);
                var MaEnscript = Cryptor.EncryptPasswordUser(TenNguoiDung, LastMaDuocGui.ToString());
                var Mail = new MailMessage()
                {
                    From = new MailAddress(tkadmin, "Quản lý chấm công"),
                    Subject = "Thay đổi mật khẩu",
                    Body = ("<p>" + "Xin chào " + "<strong>" + NguoiDung.TenCanBo + "</strong>" + "!" + "</p>" +
                          "<p>" + "Tài khoản của bạn tại" + " <a style=" + "color:Blue;" + ">" + "Phần mềm chấm công " + "</a>" + "đã được yêu cầu đặt lại mật khẩu." + "</p>"
                           + "<p>" + "Truy cập đường dẫn : " + p.Url + "?t=" + MaEnscript + " để đổi mật khẩu." + "</p>" +
                            "<p>" + "Trân Trọng," + "</p>" +
                            "Go Solutions").ToString()
                };
                Mail.IsBodyHtml = true;
                //Mail.From = new MailAddress("kekhaitaisan.team@gmail.com", "Kê khai tài sản Team");
                Mail.To.Add(new MailAddress(NguoiDung.Email.Trim()));
                var client = new SmtpClient()
                {
                    Port = 587,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Host = "smtp.gmail.com",
                    EnableSsl = true,
                    Credentials = credentials

                };
                client.Timeout = 30000;
                client.Send(Mail);
                int val = 0;
                bool TrangThai = true;
                QuenMatKhauModel QuenMatKhauModel = new QuenMatKhauModel();
                SqlParameter[] parameters = new SqlParameter[]
                {
                     new SqlParameter("@TaiKhoan", SqlDbType.NVarChar),
                     new SqlParameter("@MaDuocGui", SqlDbType.NVarChar),
                     new SqlParameter("@ThoiGianGui", SqlDbType.DateTime),
                     new SqlParameter("@TrangThai", SqlDbType.Bit),
                     new SqlParameter("@Email", SqlDbType.NVarChar),
                };
                parameters[0].Value = NguoiDung.TenNguoiDung.Trim();
                parameters[1].Value = MaEnscript;
                parameters[2].Value = DateTime.Now;
                parameters[3].Value = Utils.ConvertToBoolean(TrangThai, true);
                parameters[4].Value = NguoiDung.Email.Trim();

                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            val = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, @"v1_HeThong_InsertQuenMatKhau", parameters);
                            trans.Commit();
                        }
                        catch
                        {
                            trans.Rollback();
                            throw;
                        }

                    }
                }
                BaseResultModel.Message = "Yêu cầu đổi mật khẩu của bạn đã được xử lý! Bạn nhớ kiểm tra mail để tiếp tục các bước còn lại ! Cảm ơn!";
                BaseResultModel.Status = 1;
                return BaseResultModel;

            }
            catch (Exception ex)
            {
                BaseResultModel.Status = 0;
                BaseResultModel.Message = "Vào link này để check quyền truy cập cho ứng dụng kém an toàn !" + "https://myaKNTCount.google.com/lesssecureapps?pli=1";
                return BaseResultModel;
            }

        }

        //public EmailManager(string hostName)
        //{
        //    m_HostName = hostName;
        //}
        // Get ALl mã gửi mail
        public List<QuenMatKhauModel> GetAllQuenMatKhau()
        {
            List<QuenMatKhauModel> list = new List<QuenMatKhauModel>();

            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_HeThong_QuenMatKhau_GetAll"))
                {
                    while (dr.Read())
                    {
                        QuenMatKhauModel QuenMatKhauModel = new QuenMatKhauModel();
                        QuenMatKhauModel.QuenMatKhauID = Utils.ConvertToInt32(dr["QuenMatKhauID"], 0);
                        QuenMatKhauModel.TaiKhoan = Utils.ConvertToString(dr["TaiKhoan"], string.Empty);
                        QuenMatKhauModel.MaDuocGui = Utils.ConvertToString(dr["MaDuocGui"], string.Empty);
                        QuenMatKhauModel.ThoiGianGui = Utils.ConvertToDateTime(dr["ThoiGianGuiMa"], DateTime.Now);
                        QuenMatKhauModel.TrangThai = Utils.ConvertToBoolean(dr["TrangThai"], false);
                        list.Add(QuenMatKhauModel);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return list;
        }


        // Get Last Quên mật khẩu 
        public QuenMatKhauModel GetLastQuenMatKhau(string TenTaiKhoan)
        {
            if (string.IsNullOrEmpty(TenTaiKhoan.Trim()))
            {
                return new QuenMatKhauModel();
            }
            QuenMatKhauModel QuenMatKhauModel = new QuenMatKhauModel();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("TenTaiKhoan",SqlDbType.NVarChar)
            };
            parameters[0].Value = TenTaiKhoan.Trim();
            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_NguoiDung_GetLastQuenMatKhauByTenTaiKhoan", parameters))
                {
                    while (dr.Read())
                    {
                        QuenMatKhauModel = new QuenMatKhauModel();
                        QuenMatKhauModel.QuenMatKhauID = Utils.ConvertToInt32(dr["QuenMatKhauID"], 0);
                        QuenMatKhauModel.TaiKhoan = Utils.ConvertToString(dr["TaiKhoan"], string.Empty);
                        QuenMatKhauModel.MaDuocGui = Utils.ConvertToString(dr["MaDuocGui"], string.Empty);
                        QuenMatKhauModel.ThoiGianGui = Utils.ConvertToDateTime(dr["ThoiGianGuiMa"], DateTime.Now);
                        QuenMatKhauModel.TrangThai = Utils.ConvertToBoolean(dr["TrangThai"], false);
                        break;
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return QuenMatKhauModel;
        }
        public QuenMatKhauModel GetQuenMatKhauByMa(string Ma)
        {
            if (string.IsNullOrEmpty(Ma))
            {
                return new QuenMatKhauModel();
            }
            QuenMatKhauModel QuenMatKhauModel = new QuenMatKhauModel();
            SqlParameter[] parameters = new SqlParameter[]
           {
                            new SqlParameter("@Ma", SqlDbType.NVarChar)


           };
            parameters[0].Value = Ma.Trim();
            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_HeThong_QuenMatKhau_GetByMa", parameters))
                {
                    while (dr.Read())
                    {
                        QuenMatKhauModel = new QuenMatKhauModel();
                        QuenMatKhauModel.QuenMatKhauID = Utils.ConvertToInt32(dr["QuenMatKhauID"], 0);
                        QuenMatKhauModel.TaiKhoan = Utils.ConvertToString(dr["TaiKhoan"], string.Empty);
                        QuenMatKhauModel.MaDuocGui = Utils.ConvertToString(dr["MaDuocGui"], string.Empty);
                        QuenMatKhauModel.ThoiGianGui = Utils.ConvertToDateTime(dr["ThoiGianGuiMa"], DateTime.Now);
                        QuenMatKhauModel.TrangThai = Utils.ConvertToBoolean(dr["TrangThai"], false);
                        QuenMatKhauModel.EmailGuiLink = Utils.ConvertToString(dr["EmailGuiLink"], string.Empty);

                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return QuenMatKhauModel;
        }
        // Check Mã Mail
        public BaseResultModel CheckMaMail(string Token)
        {
            BaseResultModel BaseResultModel = new BaseResultModel();
            var QuenMatKhauModel = GetQuenMatKhauByMa(Token);
            var NguoiDung = new HeThongNguoiDungDAL().GetByName(QuenMatKhauModel.TaiKhoan.ToString(), 0);
            if (NguoiDung.NguoiDungID <= 0)
            {
                BaseResultModel.Message = "Tài khoản đã bị khóa, vui lòng liên hệ quản trị hệ thống";
                BaseResultModel.Status = 0;
                return BaseResultModel;
            }
            var CanBoByTaiKhoan = new HeThongCanBoDAL().GetCanBoByNguoiDungID(NguoiDung.NguoiDungID);
            if (QuenMatKhauModel.QuenMatKhauID <= 0)
            {
                BaseResultModel.Message = "Đường dẫn hết hạn hoặc không đúng!";
                BaseResultModel.Status = 0;
                return BaseResultModel;
            }
            TimeSpan time = (DateTime.Now).Subtract(QuenMatKhauModel.ThoiGianGui);
            var QuenMatKhauModelStatus = GetLastQuenMatKhau(QuenMatKhauModel.TaiKhoan);
            if (QuenMatKhauModelStatus.TrangThai == false)
            {
                BaseResultModel.Message = "Đường dẫn đã được sử dụng để thay đổi mật khẩu. Vui lòng kiểm tra hoặc đăng nhập lại";
                BaseResultModel.Status = 0;
                return BaseResultModel;
            }
            else if (time.TotalDays > int.Parse(new SystemConfigDAL().GetByKey("Exp_Mail").ConfigValue))
            {
                BaseResultModel.Message = "Thời gian đổi mật khẩu quá hạn !";
                BaseResultModel.Status = 0;
                return BaseResultModel;
            }
            else if (CanBoByTaiKhoan.Email != QuenMatKhauModel.EmailGuiLink)
            {
                BaseResultModel.Message = "Đường dẫn hết hạn hoặc không đúng!";
                BaseResultModel.Status = 0;
                return BaseResultModel;
            }
            else
            {
                BaseResultModel.Message = "Mời bạn điền mật khẩu mới !";
                BaseResultModel.Status = 1;
                BaseResultModel.Data = QuenMatKhauModel.TaiKhoan;
            }
            //}
            return BaseResultModel;
        }
        // Update Quên mật khẩu
        public int UpdateQuenMatKhau(string TenDangNhap)
        {
            int val = 0;
            if (string.IsNullOrEmpty(TenDangNhap))
            {
                return val;
            }

            try
            {
                SqlParameter[] parameters = new SqlParameter[]
             {
                            new SqlParameter("@TenDangNhap", SqlDbType.NVarChar),
                            new SqlParameter("@TrangThai", SqlDbType.Bit)
             };
                parameters[0].Value = TenDangNhap;
                parameters[1].Value = false;

                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {

                            val = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, @"v1_HeThong_UpdateQuenMatKhau", parameters);
                            trans.Commit();
                        }
                        catch
                        {
                            trans.Rollback();
                            throw;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return val;
        }
        // Update Người dùng 
        public BaseResultModel UpdateNguoiDung(string TenDangNhap, string MatKhauMoi)
        {
            int val = 0;
            BaseResultModel BaseResultModel = new BaseResultModel();
            var QuenMatKhauModelStatus = GetLastQuenMatKhau(TenDangNhap);
            if (QuenMatKhauModelStatus.TrangThai == false)
            {
                BaseResultModel.Message = "Đường dẫn đã được sử dụng để thay đổi mật khẩu. Vui lòng kiểm tra hoặc đăng nhập lại";
                BaseResultModel.Status = 0;
                return BaseResultModel;
            }
            if (string.IsNullOrEmpty(MatKhauMoi) || string.IsNullOrEmpty(TenDangNhap))
            {
                BaseResultModel.Message = "Bạn chưa điền mật khẩu ! Thử lại!";
                BaseResultModel.Status = 0;
                return BaseResultModel;
            }
            else
            {
                string MatKhauEnscript = Cryptor.EncryptPasswordUser(TenDangNhap, MatKhauMoi.ToString().Trim());
                try
                {
                    SqlParameter[] parameters = new SqlParameter[]
                    {
                        new SqlParameter("@TenDangNhap", SqlDbType.NVarChar),
                        new SqlParameter("@MatKhauMoi", SqlDbType.NVarChar)
                    };
                    parameters[0].Value = TenDangNhap.Trim();
                    parameters[1].Value = MatKhauEnscript;

                    using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                    {
                        conn.Open();
                        using (SqlTransaction trans = conn.BeginTransaction())
                        {
                            try
                            {
                                val = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, @"v1_HeThong_UpdateMatKhauMoi", parameters);
                                trans.Commit();
                                BaseResultModel.Status = val;
                                BaseResultModel.Message = "Tạo mới mật khẩu người dùng thành công !";
                            }
                            catch
                            {
                                trans.Rollback();
                                throw;
                            }
                        }
                    }
                    if (val <= 0)
                    {
                        return BaseResultModel;
                    }
                    SqlParameter[] parameters1 = new SqlParameter[]
                    {
                        new SqlParameter("@TenDangNhap", SqlDbType.NVarChar)
                    };
                    parameters1[0].Value = TenDangNhap;

                    using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                    {
                        conn.Open();
                        using (SqlTransaction trans = conn.BeginTransaction())
                        {
                            try
                            {
                                val = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, @"v1_HeThong_UpdateQuenMatKhau", parameters1);
                                trans.Commit();
                                BaseResultModel.Status = val;
                            }
                            catch
                            {
                                trans.Rollback();
                                throw;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return BaseResultModel;
        }

        //Changpassword 
        public BaseResultModel ChangePassword(int NguoiDungID, string OldPassword, string NewPassword)
        {
            BaseResultModel BaseResultModel = new BaseResultModel();
            try
            {
                if (string.IsNullOrEmpty(OldPassword) || string.IsNullOrEmpty(NewPassword))
                {
                    BaseResultModel.Message = "Không được để trống trường bắt buộc!";
                    BaseResultModel.Status = 0;
                    return BaseResultModel;
                }
                else if (OldPassword == NewPassword)
                {
                    BaseResultModel.Message = "Mật khẩu mới không được giống mật khẩu cũ";
                    BaseResultModel.Status = 0;
                    return BaseResultModel;
                }
                else if (NewPassword.Length <= 3)
                {
                    BaseResultModel.Message = "Mật khẩu của bạn quá ngắn";
                    BaseResultModel.Status = 0;
                    return BaseResultModel;
                }
                //var PasswordNow = Cryptor.EncryptPasswordUser(GetByID(NguoiDungID).TenNguoiDung, OldPassword);
                var PasswordNow = Utils.HashFile(Encoding.ASCII.GetBytes(OldPassword)).ToUpper();
                StringComparer comparer = StringComparer.OrdinalIgnoreCase;
                if (comparer.Compare(PasswordNow.ToString(), GetByID(NguoiDungID).MatKhau.ToString()) != 0)
                {
                    BaseResultModel.Message = "Mật Khẩu hiện tại không đúng ! Thử lại!";
                    BaseResultModel.Status = 0;
                    return BaseResultModel;
                }
                //var PasswordNew = Cryptor.EncryptPasswordUser(GetByID(NguoiDungID).TenNguoiDung.ToLower(), NewPassword);
                var PasswordNew = Utils.HashFile(Encoding.ASCII.GetBytes(NewPassword)).ToUpper();
                string Message = null;
                int val = 0;
                try
                {
                    SqlParameter[] parameters = new SqlParameter[]
                 {
                            new SqlParameter("@NguoiDungID", SqlDbType.Int),
                            new SqlParameter("@MatKhau", SqlDbType.NVarChar),
                 };
                    parameters[0].Value = NguoiDungID;
                    parameters[1].Value = PasswordNew.ToString();
                    using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                    {
                        conn.Open();
                        using (SqlTransaction trans = conn.BeginTransaction())
                        {
                            try
                            {
                                val = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, @"v2_HeThong_NguoiDung_ResetPassword", parameters);
                                trans.Commit();
                            }
                            catch
                            {
                                trans.Rollback();
                                val = 0;
                                Message = ConstantLogMessage.API_Error;
                                throw;
                            }
                            BaseResultModel.Message = " Đổi mật khẩu thành công !";
                            BaseResultModel.Status = 1;
                            return BaseResultModel;
                        }
                    }
                }
                catch (Exception ex)
                {
                    BaseResultModel.Message = ConstantLogMessage.API_Error;
                    BaseResultModel.Status = 0;
                    return BaseResultModel;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<HeThongNguoiDungModel> GetAllByListCoQuanID(List<int> CoQuanID)
        {
            var Result = new List<HeThongNguoiDungModel>();
            var table = new DataTable();
            table.Columns.Add("CoQuanID", typeof(string));
            CoQuanID.ForEach(x => table.Rows.Add(x));

            var pList = new SqlParameter("@ListCoQuanID", SqlDbType.Structured);
            pList.TypeName = "dbo.id_list";
            SqlParameter[] parameters = new SqlParameter[]
            {
                pList
            };
            parameters[0].Value = table;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v2_HeThong_NguoiDung_GetAllInListCoQuan", parameters))
                {
                    while (dr.Read())
                    {
                        HeThongNguoiDungModel nguoiDung = new HeThongNguoiDungModel();
                        nguoiDung.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        nguoiDung.NguoiDungID = Utils.ConvertToInt32(dr["NguoiDungID"], 0);
                        nguoiDung.TenNguoiDung = Utils.ConvertToString(dr["TenNguoiDung"], string.Empty);
                        nguoiDung.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        nguoiDung.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        nguoiDung.TenCoQuan = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        Result.Add(nguoiDung);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }

        public HeThongNguoiDungModel GetByCanBoID(int CanBoID)
        {
            HeThongNguoiDungModel nguoidung = new HeThongNguoiDungModel();
            SqlParameter[] parameters = new SqlParameter[]
              {
                new SqlParameter("@CanBoID",SqlDbType.Int)
              };
            parameters[0].Value = CanBoID;
            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_HeThong_NguoiDung_GetBy_CanBoID", parameters))
                {
                    while (dr.Read())
                    {
                        nguoidung = new HeThongNguoiDungModel();
                        nguoidung.NguoiDungID = Utils.ConvertToInt32(dr["NguoiDungID"], 0);
                        nguoidung.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        nguoidung.TenNguoiDung = Utils.ConvertToString(dr["TenNguoiDung"], string.Empty);
                        nguoidung.MatKhau = Utils.ConvertToString(dr["MatKhau"], string.Empty);
                        nguoidung.TrangThai = Utils.ConvertToInt32(dr["TrangThai"], 0);
                        nguoidung.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        nguoidung.VaiTro = Utils.ConvertToInt32(dr["VaiTro"], 0);
                        break;
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return nguoidung;
        }

        // Get all người dùng by canboid
        //public HeThongNguoiDungModel Get

        /// <summary>
        /// lấy danh sách người dùng được phép thêm vào cho nhóm người dùng
        /// </summary>
        /// <param name="NhomNguoiDungID"></param>
        /// <returns></returns>
        public List<HeThongNguoiDungModelPartial> HeThong_NguoiDung_GetListBy_NhomNguoiDungID(int NhomNguoiDungID)
        {
            List<HeThongNguoiDungModelPartial> list = new List<HeThongNguoiDungModelPartial>();

            SqlParameter[] parameters = new SqlParameter[]
              {
                    new SqlParameter("@NhomNguoiDungID",SqlDbType.Int)
              };
            parameters[0].Value = NhomNguoiDungID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, @"v1_HeThong_NguoiDung_GetListBy_NhomNguoiDungID", parameters))
                {
                    while (dr.Read())
                    {
                        HeThongNguoiDungModelPartial NguoiDungModel = new HeThongNguoiDungModelPartial();
                        NguoiDungModel.NguoiDungID = Utils.ConvertToInt32(dr["NguoiDungID"], 0);
                        NguoiDungModel.TenNguoiDung = Utils.ConvertToString(dr["TenNguoiDung"], string.Empty);
                        NguoiDungModel.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        NguoiDungModel.TenCoQuan = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        list.Add(NguoiDungModel);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }
        public int UpdateTrangThai(List<int> DanhSachCanBoID, int TrangThai)
        {
            var val = 0;
            var pList = new SqlParameter("@ListCanBoID", SqlDbType.Structured);
            pList.TypeName = "dbo.list_ID";
            // var TrangThai = 400;
            var tbCanBoID = new DataTable();
            tbCanBoID.Columns.Add("ID", typeof(string));

            DanhSachCanBoID.ForEach(x => tbCanBoID.Rows.Add(x));
            SqlParameter[] parameters = new SqlParameter[]
               {
                  new SqlParameter("@TrangThai",SqlDbType.Int),
                  pList,
                  new SqlParameter("@NgayKetThucLam",SqlDbType.DateTime),
               };
            parameters[0].Value = TrangThai;
            parameters[1].Value = tbCanBoID;
            parameters[2].Value = TrangThai == (int)EnumTrangThaiNhanVien.DaNghi ? DateTime.Now : Convert.DBNull;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, @"v1_HeThongNguoiDung_UpdateTrangThai", parameters);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw;
                    }
                    return val;
                }
            }
        }

        public BaseResultModel UpdateTaiKhoanNguoiDung(int NguoiDungID, string Email)
        {
            BaseResultModel BaseResultModel = new BaseResultModel();
            try
            {
                if (Email == null || Email.Length == 0)
                {
                    BaseResultModel.Status = 0;
                    return BaseResultModel;
                }
                var matKhauMacDinh = new SystemConfigDAL().GetByKey("MatKhau_MacDinh").ConfigValue;
                var PasswordNew = Cryptor.EncryptPasswordUser(Email.ToLower(), matKhauMacDinh ?? "123456");
                string Message = null;
                int val = 0;
                try
                {
                    SqlParameter[] parameters = new SqlParameter[]
                 {
                            new SqlParameter("@NguoiDungID", SqlDbType.Int),
                            new SqlParameter("@MatKhau", SqlDbType.NVarChar),
                            new SqlParameter("@TenNguoiDung", SqlDbType.NVarChar),
                 };
                    parameters[0].Value = NguoiDungID;
                    parameters[1].Value = PasswordNew.ToString();
                    parameters[2].Value = Email;
                    using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                    {
                        conn.Open();
                        using (SqlTransaction trans = conn.BeginTransaction())
                        {
                            try
                            {
                                val = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, @"v1_HeThong_NguoiDung_UpdateTaiKhoanNguoiDung", parameters);
                                trans.Commit();
                            }
                            catch
                            {
                                trans.Rollback();
                                val = 0;
                                Message = ConstantLogMessage.API_Error;
                                throw;
                            }
                            BaseResultModel.Message = " Đổi mật khẩu thành công !";
                            BaseResultModel.Status = 1;
                            return BaseResultModel;
                        }
                    }
                }
                catch (Exception ex)
                {
                    BaseResultModel.Message = ConstantLogMessage.API_Error;
                    BaseResultModel.Status = 0;
                    return BaseResultModel;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
