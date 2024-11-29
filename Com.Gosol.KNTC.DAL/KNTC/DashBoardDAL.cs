using Com.Gosol.KNTC.Ultilities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Gosol.KNTC.Models.HeThong;
using Com.Gosol.KNTC.Security;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.DAL.HeThong;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.DanhMuc;
using System.Threading;
using Com.Gosol.KNTC.Models.BaoCao;
using DocumentFormat.OpenXml.Wordprocessing;
using DataTable = System.Data.DataTable;
using Format = Com.Gosol.KNTC.Ultilities.Format;
using DocumentFormat.OpenXml.EMMA;

namespace Com.Gosol.KNTC.DAL.KNTC
{
    public class DashBoardDAL
    {
        #region Notify
        private const string COUNT_NOTIFY_LD = @"Notify_CountNotify_LD";// @"Notify_CountNotify_LD_PhanHoi";//
        private const string COUNT_NOTIFY_TP = @"Notify_CountNotify_TP";//@"Notify_CountNotify_TP_PhanHoi";//
        private const string COUNT_NOTIFY_CV = @"Notify_CountNotify_CV";

        private const string COUNT_DTCANPHANXL_LD = @"Notify_CountDTCanPhanXL_LD";
        private const string COUNT_DTCANPHANXL_TP = @"Notify_CountDTCanPhanXL_TP";
        private const string COUNT_DTCANDUYETXL_LD = @"Notify_CountDTCanDuyetXL_LD";
        private const string COUNT_DTCANDUYETXL_TP = @"Notify_CountDTCanDuyetXL_TP";

        private const string COUNT_DTCANPHANGQ_LD = @"Notify_CountDTCanPhanGQ_LD";
        private const string COUNT_DTCANPHANGQ_LD_NEW = @"Notify_CountDTCanPhanGQ_LD_new";
        private const string COUNT_DTCANPHANGQ_TP = @"Notify_CountDTCanPhanGQ_TP";
        private const string COUNT_DTCANPHANGQ_TP_NEW = @"Notify_CountDTCanPhanGQ_TP_New";
        private const string COUNT_DTCANDUYETGQ_LD = @"Notify_CountDTCanDuyetGQ_LD";
        private const string COUNT_DTCANDUYETGQ_LD_NEW = @"Notify_CountDTCanDuyetGQ_LD_New";
        private const string COUNT_DTCANDUYETGQ_TP = @"Notify_CountDTCanDuyetGQ_TP";
        private const string COUNT_VBDONDOC = @"Notify_CountVBDonDoc";
        private const string COUNT_DTCQKHAC_CHUYENDEN = @"Notify_CountDTCQKhacChuyenDen";
        private const string COUNT_PHANHOI_LD = @"Notify_CountPhanHoi_LD";
        private const string COUNT_PHANHOI_TP = @"Notify_CountPhanHoi_TP";

        private const string COUNT_DTCANXL = @"Notify_CountDTCanXuLy";
        private const string COUNT_DTCANGQ = @"Notify_CountDTCanGiaiQuyet";
        private const string COUNT_DTCANGQ_NEW = @"Notify_CountDTCanGiaiQuyet_New";

        private const string PARAM_CANBOID = "@CanBoID";
        private const string PARAM_COQUANID = "@CoQuanID";
        private const string PARAM_PHONGBANID = "@PhongBanID";
        private const string PARAM_CAPUBND = "@CapUBND";
        private const string PARAM_CAPTHANHTRA = "@CapThanhTra";
        private const string PARAM_QuyTrinhGQ = "@QuyTrinhGQ";
        private const string PARAM_DOCUMENTLIST = "@DocumentIDList";
        private const string GET_DOCUMENT_BY_STATE = "select a.* from Document a join State b on a.StateID = b.StateID where StateName = @StateName  and (DueDate >= @StartDate or @StartDate is null) and (DueDate <= @EndDate or @EndDate is null)";
        private const string PARM_STATE_NAME = "@StateName";
        private const string PARM_START_DATE = "@StartDate";
        private const string PARM_END_DATE = "@EndDate";
        private const string PARAM_TUNGAY = "@TuNgay";
        private const string PARAM_DENNGAY = "@DenNgay";
        private const string PARAM_TUNGAYINT = "@TuNgayInt";
        private const string PARAM_DENNGAYINT = "@DenNgayInt";

        public int Count_NhiemVu(int canBoID, int coQuanID, int roleID, int phongBanID, bool IsUBND)
        {
            int result = 0;

            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARAM_CANBOID, SqlDbType.Int),
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_PHONGBANID, SqlDbType.Int),
                //new SqlParameter("@RoleID", SqlDbType.Int),
                //new SqlParameter("@IsUBND", SqlDbType.Bit),

            };

            parameters[0].Value = canBoID;
            parameters[1].Value = coQuanID;
            parameters[2].Value = phongBanID;
            //parameters[3].Value = roleID;
            //parameters[4].Value = IsUBND;

            try
            {
                string storeName = "";
                if (roleID == 1)
                    storeName = COUNT_NOTIFY_LD;
                else if (roleID == 2)
                    storeName = COUNT_NOTIFY_TP;
                else
                    storeName = COUNT_NOTIFY_CV;

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, storeName, parameters))
                {
                    if (dr.Read())
                    {
                        result = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return result;
        }
        public int Count_DTCanPhanXL(int canBoID, int coQuanID, int roleID, int phongBanID)
        {
            int result = 0;

            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_PHONGBANID, SqlDbType.Int)
            };

            parameters[0].Value = coQuanID;
            parameters[1].Value = phongBanID;

            try
            {
                string storeName = "";
                if (roleID == 1)
                    storeName = COUNT_DTCANPHANXL_LD;
                else if (roleID == 2)
                    storeName = COUNT_DTCANPHANXL_TP;

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, storeName, parameters))
                {
                    if (dr.Read())
                    {
                        result = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return result;
        }
        public int Count_DTCanDuyetXL(int canBoID, int coQuanID, int roleID, int phongBanID)
        {
            int result = 0;

            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_PHONGBANID, SqlDbType.Int)
            };

            parameters[0].Value = coQuanID;
            parameters[1].Value = phongBanID;

            try
            {
                string storeName = "";
                if (roleID == 1)
                    storeName = COUNT_DTCANDUYETXL_LD;
                else if (roleID == 2)
                    storeName = COUNT_DTCANDUYETXL_TP;

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, storeName, parameters))
                {
                    if (dr.Read())
                    {
                        result = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return result;
        }
        public int Count_DTCanPhanGQ(int coQuanID, int capUBND1, int capThanhTra1, string QuyTrinhGQ)
        {
            int result = 0;
            int _QuyTrinhGQ = 0;
            if (QuyTrinhGQ == "true" || QuyTrinhGQ == "True")
            {
                _QuyTrinhGQ = 1;
            }
            else
            {
                _QuyTrinhGQ = 0;
            }
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_CAPUBND, SqlDbType.Int),
                new SqlParameter(PARAM_CAPTHANHTRA, SqlDbType.Int),
                new SqlParameter(PARAM_QuyTrinhGQ, SqlDbType.Int),
            };

            parameters[0].Value = coQuanID;
            parameters[1].Value = capUBND1;
            parameters[2].Value = capThanhTra1;
            parameters[3].Value = _QuyTrinhGQ;

            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_DTCANPHANGQ_LD_NEW, parameters))
                {
                    if (dr.Read())
                    {
                        result = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return result;
        }
        public int Count_DTCanPhanGQ_TP(int coQuanID, int canBoID, int capUBND1, int capThanhTra1)
        {
            int result = 0;

            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_CANBOID, SqlDbType.Int),
                new SqlParameter(PARAM_CAPUBND, SqlDbType.Int),
                new SqlParameter(PARAM_CAPTHANHTRA, SqlDbType.Int),
            };

            parameters[0].Value = coQuanID;
            parameters[1].Value = canBoID;
            parameters[2].Value = capUBND1;
            parameters[3].Value = capThanhTra1;
            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_DTCANPHANGQ_TP_NEW, parameters))
                {
                    if (dr.Read())
                    {
                        result = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return result;
        }
        public int Count_DTCanDuyetGQ(int coQuanID)
        {
            int result = 0;

            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int)
            };

            parameters[0].Value = coQuanID;

            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_DTCANDUYETGQ_LD_NEW, parameters))
                {
                    if (dr.Read())
                    {
                        result = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return result;
        }
        public int Count_DTCanDuyetGQ_TP(int coQuanID, int canBoID)
        {
            int result = 0;

            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_CANBOID, SqlDbType.Int)
            };

            parameters[0].Value = coQuanID;
            parameters[1].Value = canBoID;

            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_DTCANDUYETGQ_TP, parameters))
                {
                    if (dr.Read())
                    {
                        result = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return result;
        }
        public int Count_DTCanXL(int canBoID, int coQuanID, int phongBanID)
        {
            int result = 0;

            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_PHONGBANID, SqlDbType.Int),
                new SqlParameter(PARAM_CANBOID, SqlDbType.Int)
            };

            parameters[0].Value = coQuanID;
            parameters[1].Value = phongBanID;
            parameters[2].Value = canBoID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_DTCANXL, parameters))
                {
                    if (dr.Read())
                    {
                        result = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return result;
        }
        public int Count_DTCanGQ(int canBoID, int coQuanID)
        {
            int result = 0;

            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_CANBOID, SqlDbType.Int)
            };

            parameters[0].Value = coQuanID;
            parameters[1].Value = canBoID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_DTCANGQ_NEW, parameters))
                {
                    if (dr.Read())
                    {
                        result = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return result;
        }
        public int Count_VBDonDoc(int coQuanID)
        {
            int result = 0;

            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int)
            };

            parameters[0].Value = coQuanID;

            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "Notify_CountVBDonDoc_New", parameters))
                {
                    if (dr.Read())
                    {
                        result = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return result;
        }
        public int Count_VBDonDoc1(int coQuanID)
        {
            int result = 0;

            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int)
            };

            parameters[0].Value = coQuanID;

            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_VBDONDOC, parameters))
                {
                    if (dr.Read())
                    {
                        result = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return result;
        }
        public int Count_DTCQKhacChuyenDen(int canBoID, int coQuanID)
        {
            int result = 0;

            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_CANBOID, SqlDbType.Int)
            };

            parameters[0].Value = coQuanID;
            parameters[1].Value = canBoID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_DTCQKHAC_CHUYENDEN, parameters))
                {
                    if (dr.Read())
                    {
                        result = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return result;
        }
        public int Count_PhanHoi_LD(int coQuanID, int capUBND, int capThanhTra, int QuyTrinhGQ)
        {
            int result = 0;

            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_CAPUBND, SqlDbType.Int),
                new SqlParameter(PARAM_CAPTHANHTRA, SqlDbType.Int),
                new SqlParameter(PARAM_QuyTrinhGQ, SqlDbType.Int),
            };

            parameters[0].Value = coQuanID;
            parameters[1].Value = capUBND;
            parameters[2].Value = capThanhTra;
            parameters[3].Value = QuyTrinhGQ;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_PHANHOI_LD, parameters))
                {
                    if (dr.Read())
                    {
                        result = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return result;
        }
        public int Count_PhanHoi_TP(int coQuanID, int canBoID)
        {
            int result = 0;

            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARAM_CANBOID, SqlDbType.Int),
            };

            parameters[0].Value = coQuanID;
            parameters[1].Value = canBoID;
            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_PHANHOI_TP, parameters))
                {
                    if (dr.Read())
                    {
                        result = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return result;
        }
        public IList<DTXuLyInfo> GetDTCanXuLy_New(int CoQuanID, int CanBoID, List<int> docList)
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

            parms[0].Value = CoQuanID;
            parms[1].Value = CanBoID;

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

        public List<DocumentModel> GetDocumentByState(string stateName, DateTime startDate, DateTime endDate)
        {
            List<DocumentModel> docList = new List<DocumentModel>();

            IDbConnection dbConnection = DatabaseProxy.CreateDBConnection();
            IDbCommand dbCommand = DatabaseProxy.CreateDBCommand(dbConnection, GET_DOCUMENT_BY_STATE);
            DatabaseProxy.AddParameter(dbCommand, PARM_STATE_NAME, stateName);
            if (startDate != DateTime.MinValue)
                DatabaseProxy.AddParameter(dbCommand, PARM_START_DATE, startDate);
            else DatabaseProxy.AddParameter(dbCommand, PARM_START_DATE, DBNull.Value);
            if (endDate != DateTime.MinValue)
                DatabaseProxy.AddParameter(dbCommand, PARM_END_DATE, endDate);
            else DatabaseProxy.AddParameter(dbCommand, PARM_END_DATE, DBNull.Value);

            try
            {
                dbConnection.Open();
                IDataReader dataReader = dbCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    DocumentModel model = GetDocumentData(dataReader);
                    docList.Add(model);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                dbConnection.Close();
            }

            return docList;
        }

        public DocumentModel GetDocumentData(IDataReader dataReader)
        {
            DocumentModel model = new DocumentModel();

            model.DocumentID = Utils.ConvertToInt32(dataReader["DocumentID"], 0);
            model.WorkflowID = Utils.ConvertToInt32(dataReader["WorkflowID"], 0);
            model.StateID = Utils.ConvertToInt32(dataReader["StateID"], 0);
            model.DueDate = Utils.ConvertToDateTime(dataReader["DueDate"], DateTime.MinValue);

            return model;
        }
        #endregion

        public DashBoardModel GetDuLieuDashBoard(DashBoardParams p)
        {
            DashBoardModel DashBoardData = new DashBoardModel();
            DashBoardData.SoLieuTongHop = new List<Data>();
            DashBoardData.SoLieuBieuDoCot = new List<BieuDoCot>();
            DashBoardData.SoLieuBieuTron = new List<Data>();

            DashBoardData.SoLieuTongHop.Add(new Data("Lượt tiếp", 0));
            DashBoardData.SoLieuTongHop.Add(new Data("Đã xử lý", 0));
            DashBoardData.SoLieuTongHop.Add(new Data("Đã giải quyết", 0));
            DashBoardData.SoLieuTongHop.Add(new Data("Đơn phản ánh, KN", 0));
            DashBoardData.SoLieuTongHop.Add(new Data("Đơn khiếu nại", 0));
            DashBoardData.SoLieuTongHop.Add(new Data("Đơn tố cáo", 0));

            List<BCTinhHinhTD_XLD_GQInfo> lsData = new List<BCTinhHinhTD_XLD_GQInfo>();
            DBSuDungPhanMemTongInfo ldInfo = new DBSuDungPhanMemTongInfo();
            //DBGetTinhHinhTD_XLD_GQ(p.CoQuanID ?? 0, p.RoleID ?? 0, p.CapID ?? 0, p.TinhID ?? 0, p.HuyenID ?? 0, p.TuNgay, p.DenNgay, p.CapIDSelect, p.CoQuanIDSelect, ref lsData, ref ldInfo);
            List<BCTinhHinhTD_XLD_GQInfo> lsDataOld = new List<BCTinhHinhTD_XLD_GQInfo>();
            DBSuDungPhanMemTongInfo ldInfoOld = new DBSuDungPhanMemTongInfo();
            //DBGetTinhHinhTD_XLD_GQ(p.CoQuanID ?? 0, p.RoleID ?? 0, p.CapID ?? 0, p.TinhID ?? 0, p.HuyenID ?? 0, p.TuNgayCungKy, p.DenNgayCungKy, p.CapIDSelect, p.CoQuanIDSelect, ref lsDataOld, ref ldInfoOld);           

            //Thread t1 = new Thread(() => {
            DBGetTinhHinhTD_XLD_GQ(p.CoQuanID ?? 0, p.RoleID ?? 0, p.CapID ?? 0, p.TinhID ?? 0, p.HuyenID ?? 0, p.TuNgay, p.DenNgay, p.CapIDSelect, p.CoQuanIDSelect, ref lsData, ref ldInfo);
            int tong = ldInfo.TongDonThuBHGQ_KNPA + ldInfo.TongDonThuBHGQ_KN + ldInfo.TongDonThuBHGQ_TC;
            if (tong > 0)
            {
                decimal tilePAKN = Math.Round(((decimal)(ldInfo.TongDonThuBHGQ_KNPA * 100) / tong), 2);
                decimal tileKN = Math.Round(((decimal)(ldInfo.TongDonThuBHGQ_KN * 100) / tong), 2);
                decimal tileTC = 100 - tilePAKN - tileKN;
                DashBoardData.SoLieuBieuTron.Add(new Data("Đơn phản ánh, kiến nghị", tilePAKN));
                DashBoardData.SoLieuBieuTron.Add(new Data("Đơn khiếu nại", tileKN));
                DashBoardData.SoLieuBieuTron.Add(new Data("Đơn tố cáo", tileTC));
            }

            if (lsData.Count > 0)
            {
                Boolean checkCapToanTinh = false;

                var TongSoLuotTiepToanTinh = new BieuDoCot("Tổng số lượt tiếp", 1, CapQuanLy.CapToanTinh.GetHashCode(), 0, 0, 0, 0, 0);
                var DonPAKNToanTinh = new BieuDoCot("Đơn PA, KN", 2, CapQuanLy.CapToanTinh.GetHashCode(), 0, 0, 0, 0, 0);
                var DonKNToanTinh = new BieuDoCot("Đơn khiếu nại", 3, CapQuanLy.CapToanTinh.GetHashCode(), 0, 0, 0, 0, 0);
                var DonTCToanTinh = new BieuDoCot("Đơn tố cáo", 4, CapQuanLy.CapToanTinh.GetHashCode(), 0, 0, 0, 0, 0);

                foreach (var item in lsData)
                {
                    if (item.CapID == CapQuanLy.CapToanTinh.GetHashCode())
                    {
                        //DashBoardData.SoLieuBieuDoCot.Add(new BieuDoCot("Toàn tỉnh", CapQuanLy.CapToanTinh.GetHashCode(), 0, 0, 0, 0, 0));
                        checkCapToanTinh = true;
                    }
                    else if (item.CapID == CapQuanLy.CapUBNDTinh.GetHashCode())
                    {
                        var TongSoLuotTiep = new BieuDoCot("Tổng số lượt tiếp", 1, CapQuanLy.CapUBNDTinh.GetHashCode(), 0, 0, 0, 0, 0);
                        var DonPAKN = new BieuDoCot("Đơn PA, KN", 2, CapQuanLy.CapUBNDTinh.GetHashCode(), 0, 0, 0, 0, 0);
                        var DonKN = new BieuDoCot("Đơn khiếu nại", 3, CapQuanLy.CapUBNDTinh.GetHashCode(), 0, 0, 0, 0, 0);
                        var DonTC = new BieuDoCot("Đơn tố cáo", 4, CapQuanLy.CapUBNDTinh.GetHashCode(), 0, 0, 0, 0, 0);
                        if (item.LsByCoQuan != null && item.LsByCoQuan.Count > 0)
                        {
                            foreach (var cq in item.LsByCoQuan)
                            {
                                TongSoLuotTiep.TrongKy += cq.SoDonTiepDan;
                                TongSoLuotTiep.DaXuLy += cq.SoXLDaXuLy;
                                TongSoLuotTiep.DaGiaiQuyet += cq.GQDKNDaGQ + cq.GQDKNPADaGQ + cq.GQDTCDaGQ;
                                TongSoLuotTiep.ChuaGiaiQuyet += cq.ChuaGiaiQuyet;

                                DonPAKN.TrongKy += cq.SLPhanAnhKienNghi;
                                DonPAKN.DaXuLy += cq.XLDPhanAnhKienNghi;
                                DonPAKN.DaGiaiQuyet += cq.GQDKNPADaGQ;
                                DonPAKN.ChuaGiaiQuyet += cq.GQDKNPAChuaGQ;

                                DonKN.TrongKy += cq.SLKhieuNai;
                                DonKN.DaXuLy += cq.XLDKhieuNai;
                                DonKN.DaGiaiQuyet += cq.GQDKNDaGQ;
                                DonKN.ChuaGiaiQuyet += cq.GQDKNChuaGQ;

                                DonTC.TrongKy += cq.SLToCao;
                                DonTC.DaXuLy += cq.XLDToCao;
                                DonTC.DaGiaiQuyet += cq.GQDTCDaGQ;
                                DonTC.ChuaGiaiQuyet += cq.GQDTCChuaGQ;
                            }
                        }

                        DashBoardData.SoLieuBieuDoCot.Add(new BieuDoCot("Tổng số lượt tiếp", TongSoLuotTiep.LoaiCot, TongSoLuotTiep.CapID, TongSoLuotTiep.TrongKy, TongSoLuotTiep.CungKy, TongSoLuotTiep.DaXuLy, TongSoLuotTiep.DaGiaiQuyet, TongSoLuotTiep.ChuaGiaiQuyet));
                        DashBoardData.SoLieuBieuDoCot.Add(new BieuDoCot("Đơn PA, KN", DonPAKN.LoaiCot, DonPAKN.CapID, DonPAKN.TrongKy, DonPAKN.CungKy, DonPAKN.DaXuLy, DonPAKN.DaGiaiQuyet, DonPAKN.ChuaGiaiQuyet));
                        DashBoardData.SoLieuBieuDoCot.Add(new BieuDoCot("Đơn khiếu nại", DonKN.LoaiCot, DonKN.CapID, DonKN.TrongKy, DonKN.CungKy, DonKN.DaXuLy, DonKN.DaGiaiQuyet, DonKN.ChuaGiaiQuyet));
                        DashBoardData.SoLieuBieuDoCot.Add(new BieuDoCot("Đơn tố cáo", DonTC.LoaiCot, DonTC.CapID, DonTC.TrongKy, DonTC.CungKy, DonTC.DaXuLy, DonTC.DaGiaiQuyet, DonTC.ChuaGiaiQuyet));

                        TongSoLuotTiepToanTinh.TrongKy += TongSoLuotTiep.TrongKy;
                        TongSoLuotTiepToanTinh.DaXuLy += TongSoLuotTiep.DaXuLy;
                        TongSoLuotTiepToanTinh.DaGiaiQuyet += TongSoLuotTiep.DaGiaiQuyet;
                        TongSoLuotTiepToanTinh.ChuaGiaiQuyet += TongSoLuotTiep.ChuaGiaiQuyet;

                        DonPAKNToanTinh.TrongKy += DonPAKN.TrongKy;
                        DonPAKNToanTinh.DaXuLy += DonPAKN.DaXuLy;
                        DonPAKNToanTinh.DaGiaiQuyet += DonPAKN.DaGiaiQuyet;
                        DonPAKNToanTinh.ChuaGiaiQuyet += DonPAKN.ChuaGiaiQuyet;

                        DonKNToanTinh.TrongKy += DonKN.TrongKy;
                        DonKNToanTinh.DaXuLy += DonKN.DaXuLy;
                        DonKNToanTinh.DaGiaiQuyet += DonKN.DaGiaiQuyet;
                        DonKNToanTinh.ChuaGiaiQuyet += DonKN.ChuaGiaiQuyet;

                        DonTCToanTinh.TrongKy += DonTC.TrongKy;
                        DonTCToanTinh.DaXuLy += DonTC.DaXuLy;
                        DonTCToanTinh.DaGiaiQuyet += DonTC.DaGiaiQuyet;
                        DonTCToanTinh.ChuaGiaiQuyet += DonTC.ChuaGiaiQuyet;
                    }
                    else if (item.CapID == CapQuanLy.CapSoNganh.GetHashCode())
                    {
                        var TongSoLuotTiep = new BieuDoCot("Tổng số lượt tiếp", 1, CapQuanLy.CapSoNganh.GetHashCode(), 0, 0, 0, 0, 0);
                        var DonPAKN = new BieuDoCot("Đơn PA, KN", 2, CapQuanLy.CapSoNganh.GetHashCode(), 0, 0, 0, 0, 0);
                        var DonKN = new BieuDoCot("Đơn khiếu nại", 3, CapQuanLy.CapSoNganh.GetHashCode(), 0, 0, 0, 0, 0);
                        var DonTC = new BieuDoCot("Đơn tố cáo", 4, CapQuanLy.CapSoNganh.GetHashCode(), 0, 0, 0, 0, 0);
                        if (item.LsByCoQuan != null && item.LsByCoQuan.Count > 0)
                        {
                            foreach (var cq in item.LsByCoQuan)
                            {
                                TongSoLuotTiep.TrongKy += cq.SoDonTiepDan;
                                TongSoLuotTiep.DaXuLy += cq.SoXLDaXuLy;
                                TongSoLuotTiep.DaGiaiQuyet += cq.GQDKNDaGQ + cq.GQDKNPADaGQ + cq.GQDTCDaGQ;
                                TongSoLuotTiep.ChuaGiaiQuyet += cq.ChuaGiaiQuyet;

                                DonPAKN.TrongKy += cq.SLPhanAnhKienNghi;
                                DonPAKN.DaXuLy += cq.XLDPhanAnhKienNghi;
                                DonPAKN.DaGiaiQuyet += cq.GQDKNPADaGQ;
                                DonPAKN.ChuaGiaiQuyet += cq.GQDKNPAChuaGQ;

                                DonKN.TrongKy += cq.SLKhieuNai;
                                DonKN.DaXuLy += cq.XLDKhieuNai;
                                DonKN.DaGiaiQuyet += cq.GQDKNDaGQ;
                                DonKN.ChuaGiaiQuyet += cq.GQDKNChuaGQ;

                                DonTC.TrongKy += cq.SLToCao;
                                DonTC.DaXuLy += cq.XLDToCao;
                                DonTC.DaGiaiQuyet += cq.GQDTCDaGQ;
                                DonTC.ChuaGiaiQuyet += cq.GQDTCChuaGQ;
                            }
                        }

                        DashBoardData.SoLieuBieuDoCot.Add(new BieuDoCot("Tổng số lượt tiếp", TongSoLuotTiep.LoaiCot, TongSoLuotTiep.CapID, TongSoLuotTiep.TrongKy, TongSoLuotTiep.CungKy, TongSoLuotTiep.DaXuLy, TongSoLuotTiep.DaGiaiQuyet, TongSoLuotTiep.ChuaGiaiQuyet));
                        DashBoardData.SoLieuBieuDoCot.Add(new BieuDoCot("Đơn PA, KN", DonPAKN.LoaiCot, DonPAKN.CapID, DonPAKN.TrongKy, DonPAKN.CungKy, DonPAKN.DaXuLy, DonPAKN.DaGiaiQuyet, DonPAKN.ChuaGiaiQuyet));
                        DashBoardData.SoLieuBieuDoCot.Add(new BieuDoCot("Đơn khiếu nại", DonKN.LoaiCot, DonKN.CapID, DonKN.TrongKy, DonKN.CungKy, DonKN.DaXuLy, DonKN.DaGiaiQuyet, DonKN.ChuaGiaiQuyet));
                        DashBoardData.SoLieuBieuDoCot.Add(new BieuDoCot("Đơn tố cáo", DonTC.LoaiCot, DonTC.CapID, DonTC.TrongKy, DonTC.CungKy, DonTC.DaXuLy, DonTC.DaGiaiQuyet, DonTC.ChuaGiaiQuyet));

                        TongSoLuotTiepToanTinh.TrongKy += TongSoLuotTiep.TrongKy;
                        TongSoLuotTiepToanTinh.DaXuLy += TongSoLuotTiep.DaXuLy;
                        TongSoLuotTiepToanTinh.DaGiaiQuyet += TongSoLuotTiep.DaGiaiQuyet;
                        TongSoLuotTiepToanTinh.ChuaGiaiQuyet += TongSoLuotTiep.ChuaGiaiQuyet;

                        DonPAKNToanTinh.TrongKy += DonPAKN.TrongKy;
                        DonPAKNToanTinh.DaXuLy += DonPAKN.DaXuLy;
                        DonPAKNToanTinh.DaGiaiQuyet += DonPAKN.DaGiaiQuyet;
                        DonPAKNToanTinh.ChuaGiaiQuyet += DonPAKN.ChuaGiaiQuyet;

                        DonKNToanTinh.TrongKy += DonKN.TrongKy;
                        DonKNToanTinh.DaXuLy += DonKN.DaXuLy;
                        DonKNToanTinh.DaGiaiQuyet += DonKN.DaGiaiQuyet;
                        DonKNToanTinh.ChuaGiaiQuyet += DonKN.ChuaGiaiQuyet;

                        DonTCToanTinh.TrongKy += DonTC.TrongKy;
                        DonTCToanTinh.DaXuLy += DonTC.DaXuLy;
                        DonTCToanTinh.DaGiaiQuyet += DonTC.DaGiaiQuyet;
                        DonTCToanTinh.ChuaGiaiQuyet += DonTC.ChuaGiaiQuyet;
                    }
                    else if (item.CapID == CapQuanLy.CapUBNDHuyen.GetHashCode())
                    {
                        var TongSoLuotTiep = new BieuDoCot("Tổng số lượt tiếp", 1, CapQuanLy.CapUBNDHuyen.GetHashCode(), 0, 0, 0, 0, 0);
                        var DonPAKN = new BieuDoCot("Đơn PA, KN", 2, CapQuanLy.CapUBNDHuyen.GetHashCode(), 0, 0, 0, 0, 0);
                        var DonKN = new BieuDoCot("Đơn khiếu nại", 3, CapQuanLy.CapUBNDHuyen.GetHashCode(), 0, 0, 0, 0, 0);
                        var DonTC = new BieuDoCot("Đơn tố cáo", 4, CapQuanLy.CapUBNDHuyen.GetHashCode(), 0, 0, 0, 0, 0);
                        if (item.LsByCapCoQuan != null && item.LsByCapCoQuan.Count > 0)
                        {
                            foreach (var huyen in item.LsByCapCoQuan)
                            {
                                if (huyen.LsByCoQuan != null && huyen.LsByCoQuan.Count > 0)
                                {
                                    foreach (var cq in huyen.LsByCoQuan)
                                    {
                                        TongSoLuotTiep.TrongKy += cq.SoDonTiepDan;
                                        TongSoLuotTiep.DaXuLy += cq.SoXLDaXuLy;
                                        TongSoLuotTiep.DaGiaiQuyet += cq.GQDKNDaGQ + cq.GQDKNPADaGQ + cq.GQDTCDaGQ;
                                        TongSoLuotTiep.ChuaGiaiQuyet += cq.ChuaGiaiQuyet;

                                        DonPAKN.TrongKy += cq.SLPhanAnhKienNghi;
                                        DonPAKN.DaXuLy += cq.XLDPhanAnhKienNghi;
                                        DonPAKN.DaGiaiQuyet += cq.GQDKNPADaGQ;
                                        DonPAKN.ChuaGiaiQuyet += cq.GQDKNPAChuaGQ;

                                        DonKN.TrongKy += cq.SLKhieuNai;
                                        DonKN.DaXuLy += cq.XLDKhieuNai;
                                        DonKN.DaGiaiQuyet += cq.GQDKNDaGQ;
                                        DonKN.ChuaGiaiQuyet += cq.GQDKNChuaGQ;

                                        DonTC.TrongKy += cq.SLToCao;
                                        DonTC.DaXuLy += cq.XLDToCao;
                                        DonTC.DaGiaiQuyet += cq.GQDTCDaGQ;
                                        DonTC.ChuaGiaiQuyet += cq.GQDTCChuaGQ;
                                    }
                                }
                            }
                        }

                        DashBoardData.SoLieuBieuDoCot.Add(new BieuDoCot("Tổng số lượt tiếp", TongSoLuotTiep.LoaiCot, TongSoLuotTiep.CapID, TongSoLuotTiep.TrongKy, TongSoLuotTiep.CungKy, TongSoLuotTiep.DaXuLy, TongSoLuotTiep.DaGiaiQuyet, TongSoLuotTiep.ChuaGiaiQuyet));
                        DashBoardData.SoLieuBieuDoCot.Add(new BieuDoCot("Đơn PA, KN", DonPAKN.LoaiCot, DonPAKN.CapID, DonPAKN.TrongKy, DonPAKN.CungKy, DonPAKN.DaXuLy, DonPAKN.DaGiaiQuyet, DonPAKN.ChuaGiaiQuyet));
                        DashBoardData.SoLieuBieuDoCot.Add(new BieuDoCot("Đơn khiếu nại", DonKN.LoaiCot, DonKN.CapID, DonKN.TrongKy, DonKN.CungKy, DonKN.DaXuLy, DonKN.DaGiaiQuyet, DonKN.ChuaGiaiQuyet));
                        DashBoardData.SoLieuBieuDoCot.Add(new BieuDoCot("Đơn tố cáo", DonTC.LoaiCot, DonTC.CapID, DonTC.TrongKy, DonTC.CungKy, DonTC.DaXuLy, DonTC.DaGiaiQuyet, DonTC.ChuaGiaiQuyet));

                        TongSoLuotTiepToanTinh.TrongKy += TongSoLuotTiep.TrongKy;
                        TongSoLuotTiepToanTinh.DaXuLy += TongSoLuotTiep.DaXuLy;
                        TongSoLuotTiepToanTinh.DaGiaiQuyet += TongSoLuotTiep.DaGiaiQuyet;
                        TongSoLuotTiepToanTinh.ChuaGiaiQuyet += TongSoLuotTiep.ChuaGiaiQuyet;

                        DonPAKNToanTinh.TrongKy += DonPAKN.TrongKy;
                        DonPAKNToanTinh.DaXuLy += DonPAKN.DaXuLy;
                        DonPAKNToanTinh.DaGiaiQuyet += DonPAKN.DaGiaiQuyet;
                        DonPAKNToanTinh.ChuaGiaiQuyet += DonPAKN.ChuaGiaiQuyet;

                        DonKNToanTinh.TrongKy += DonKN.TrongKy;
                        DonKNToanTinh.DaXuLy += DonKN.DaXuLy;
                        DonKNToanTinh.DaGiaiQuyet += DonKN.DaGiaiQuyet;
                        DonKNToanTinh.ChuaGiaiQuyet += DonKN.ChuaGiaiQuyet;

                        DonTCToanTinh.TrongKy += DonTC.TrongKy;
                        DonTCToanTinh.DaXuLy += DonTC.DaXuLy;
                        DonTCToanTinh.DaGiaiQuyet += DonTC.DaGiaiQuyet;
                        DonTCToanTinh.ChuaGiaiQuyet += DonTC.ChuaGiaiQuyet;
                    }
                    else if (item.CapID == CapQuanLy.CapUBNDXa.GetHashCode())
                    {
                        var TongSoLuotTiep = new BieuDoCot("Tổng số lượt tiếp", 1, CapQuanLy.CapUBNDXa.GetHashCode(), 0, 0, 0, 0, 0);
                        var DonPAKN = new BieuDoCot("Đơn PA, KN", 2, CapQuanLy.CapUBNDXa.GetHashCode(), 0, 0, 0, 0, 0);
                        var DonKN = new BieuDoCot("Đơn khiếu nại", 3, CapQuanLy.CapUBNDXa.GetHashCode(), 0, 0, 0, 0, 0);
                        var DonTC = new BieuDoCot("Đơn tố cáo", 4, CapQuanLy.CapUBNDXa.GetHashCode(), 0, 0, 0, 0, 0);
                        if (item.LsByCapCoQuan != null && item.LsByCapCoQuan.Count > 0)
                        {
                            foreach (var huyen in item.LsByCapCoQuan)
                            {
                                if (huyen.LsByCoQuan != null && huyen.LsByCoQuan.Count > 0)
                                {
                                    foreach (var cq in huyen.LsByCoQuan)
                                    {
                                        TongSoLuotTiep.TrongKy += cq.SoDonTiepDan;
                                        TongSoLuotTiep.DaXuLy += cq.SoXLDaXuLy;
                                        TongSoLuotTiep.DaGiaiQuyet += cq.GQDKNDaGQ + cq.GQDKNPADaGQ + cq.GQDTCDaGQ;
                                        TongSoLuotTiep.ChuaGiaiQuyet += cq.ChuaGiaiQuyet;

                                        DonPAKN.TrongKy += cq.TongGQDKNPA;
                                        DonPAKN.DaXuLy += cq.GQDKNPADaGQ + cq.GQDKNPADangGQ;
                                        DonPAKN.DaGiaiQuyet += cq.GQDKNPADaGQ;
                                        DonPAKN.ChuaGiaiQuyet += cq.GQDKNPAChuaGQ;

                                        DonKN.TrongKy += cq.TongGQDKN;
                                        DonKN.DaXuLy += cq.GQDKNDaGQ + cq.GQDKNDangGQ;
                                        DonKN.DaGiaiQuyet += cq.GQDKNDaGQ;
                                        DonKN.ChuaGiaiQuyet += cq.GQDKNChuaGQ;

                                        DonTC.TrongKy += cq.TongGQDTC;
                                        DonTC.DaXuLy += cq.GQDTCDaGQ + cq.GQDTCDangGQ;
                                        DonTC.DaGiaiQuyet += cq.GQDTCDaGQ;
                                        DonTC.ChuaGiaiQuyet += cq.GQDTCChuaGQ;
                                    }
                                }
                            }
                        }
                        else if (item.LsByCoQuan != null && item.LsByCoQuan.Count > 0)
                        {
                            foreach (var cq in item.LsByCoQuan)
                            {
                                TongSoLuotTiep.TrongKy += cq.SoDonTiepDan;
                                TongSoLuotTiep.DaXuLy += cq.SoXLDaXuLy;
                                TongSoLuotTiep.DaGiaiQuyet += cq.GQDKNDaGQ + cq.GQDKNPADaGQ + cq.GQDTCDaGQ;
                                TongSoLuotTiep.ChuaGiaiQuyet += cq.ChuaGiaiQuyet;

                                DonPAKN.TrongKy += cq.SLPhanAnhKienNghi;
                                DonPAKN.DaXuLy += cq.XLDPhanAnhKienNghi;
                                DonPAKN.DaGiaiQuyet += cq.GQDKNPADaGQ;
                                DonPAKN.ChuaGiaiQuyet += cq.GQDKNPAChuaGQ;

                                DonKN.TrongKy += cq.SLKhieuNai;
                                DonKN.DaXuLy += cq.XLDKhieuNai;
                                DonKN.DaGiaiQuyet += cq.GQDKNDaGQ;
                                DonKN.ChuaGiaiQuyet += cq.GQDKNChuaGQ;

                                DonTC.TrongKy += cq.SLToCao;
                                DonTC.DaXuLy += cq.XLDToCao;
                                DonTC.DaGiaiQuyet += cq.GQDTCDaGQ;
                                DonTC.ChuaGiaiQuyet += cq.GQDTCChuaGQ;
                            }
                        }
                        DashBoardData.SoLieuBieuDoCot.Add(new BieuDoCot("Tổng số lượt tiếp", TongSoLuotTiep.LoaiCot, TongSoLuotTiep.CapID, TongSoLuotTiep.TrongKy, TongSoLuotTiep.CungKy, TongSoLuotTiep.DaXuLy, TongSoLuotTiep.DaGiaiQuyet, TongSoLuotTiep.ChuaGiaiQuyet));
                        DashBoardData.SoLieuBieuDoCot.Add(new BieuDoCot("Đơn PA, KN", DonPAKN.LoaiCot, DonPAKN.CapID, DonPAKN.TrongKy, DonPAKN.CungKy, DonPAKN.DaXuLy, DonPAKN.DaGiaiQuyet, DonPAKN.ChuaGiaiQuyet));
                        DashBoardData.SoLieuBieuDoCot.Add(new BieuDoCot("Đơn khiếu nại", DonKN.LoaiCot, DonKN.CapID, DonKN.TrongKy, DonKN.CungKy, DonKN.DaXuLy, DonKN.DaGiaiQuyet, DonKN.ChuaGiaiQuyet));
                        DashBoardData.SoLieuBieuDoCot.Add(new BieuDoCot("Đơn tố cáo", DonTC.LoaiCot, DonTC.CapID, DonTC.TrongKy, DonTC.CungKy, DonTC.DaXuLy, DonTC.DaGiaiQuyet, DonTC.ChuaGiaiQuyet));

                        TongSoLuotTiepToanTinh.TrongKy += TongSoLuotTiep.TrongKy;
                        TongSoLuotTiepToanTinh.DaXuLy += TongSoLuotTiep.DaXuLy;
                        TongSoLuotTiepToanTinh.DaGiaiQuyet += TongSoLuotTiep.DaGiaiQuyet;
                        TongSoLuotTiepToanTinh.ChuaGiaiQuyet += TongSoLuotTiep.ChuaGiaiQuyet;

                        DonPAKNToanTinh.TrongKy += DonPAKN.TrongKy;
                        DonPAKNToanTinh.DaXuLy += DonPAKN.DaXuLy;
                        DonPAKNToanTinh.DaGiaiQuyet += DonPAKN.DaGiaiQuyet;
                        DonPAKNToanTinh.ChuaGiaiQuyet += DonPAKN.ChuaGiaiQuyet;

                        DonKNToanTinh.TrongKy += DonKN.TrongKy;
                        DonKNToanTinh.DaXuLy += DonKN.DaXuLy;
                        DonKNToanTinh.DaGiaiQuyet += DonKN.DaGiaiQuyet;
                        DonKNToanTinh.ChuaGiaiQuyet += DonKN.ChuaGiaiQuyet;

                        DonTCToanTinh.TrongKy += DonTC.TrongKy;
                        DonTCToanTinh.DaXuLy += DonTC.DaXuLy;
                        DonTCToanTinh.DaGiaiQuyet += DonTC.DaGiaiQuyet;
                        DonTCToanTinh.ChuaGiaiQuyet += DonTC.ChuaGiaiQuyet;
                    }
                    else if (item.CapID == CapQuanLy.CapPhong.GetHashCode())
                    {
                        var TongSoLuotTiep = new BieuDoCot("Tổng số lượt tiếp", 1, CapQuanLy.CapPhong.GetHashCode(), 0, 0, 0, 0, 0);
                        var DonPAKN = new BieuDoCot("Đơn PA, KN", 2, CapQuanLy.CapPhong.GetHashCode(), 0, 0, 0, 0, 0);
                        var DonKN = new BieuDoCot("Đơn khiếu nại", 3, CapQuanLy.CapPhong.GetHashCode(), 0, 0, 0, 0, 0);
                        var DonTC = new BieuDoCot("Đơn tố cáo", 4, CapQuanLy.CapPhong.GetHashCode(), 0, 0, 0, 0, 0);
                        if (item.LsByCoQuan != null && item.LsByCoQuan.Count > 0)
                        {
                            foreach (var cq in item.LsByCoQuan)
                            {
                                TongSoLuotTiep.TrongKy += cq.SoDonTiepDan;
                                TongSoLuotTiep.DaXuLy += cq.SoXLDaXuLy;
                                TongSoLuotTiep.DaGiaiQuyet += cq.GQDKNDaGQ + cq.GQDKNPADaGQ + cq.GQDTCDaGQ;
                                TongSoLuotTiep.ChuaGiaiQuyet += cq.ChuaGiaiQuyet;

                                DonPAKN.TrongKy += cq.SLPhanAnhKienNghi;
                                DonPAKN.DaXuLy += cq.XLDPhanAnhKienNghi;
                                DonPAKN.DaGiaiQuyet += cq.GQDKNPADaGQ;
                                DonPAKN.ChuaGiaiQuyet += cq.GQDKNPAChuaGQ;

                                DonKN.TrongKy += cq.SLKhieuNai;
                                DonKN.DaXuLy += cq.XLDKhieuNai;
                                DonKN.DaGiaiQuyet += cq.GQDKNDaGQ;
                                DonKN.ChuaGiaiQuyet += cq.GQDKNChuaGQ;

                                DonTC.TrongKy += cq.SLToCao;
                                DonTC.DaXuLy += cq.XLDToCao;
                                DonTC.DaGiaiQuyet += cq.GQDTCDaGQ;
                                DonTC.ChuaGiaiQuyet += cq.GQDTCChuaGQ;
                            }
                        }
                        DashBoardData.SoLieuBieuDoCot.Add(new BieuDoCot("Tổng số lượt tiếp", TongSoLuotTiep.LoaiCot, TongSoLuotTiep.CapID, TongSoLuotTiep.TrongKy, TongSoLuotTiep.CungKy, TongSoLuotTiep.DaXuLy, TongSoLuotTiep.DaGiaiQuyet, TongSoLuotTiep.ChuaGiaiQuyet));
                        DashBoardData.SoLieuBieuDoCot.Add(new BieuDoCot("Đơn PA, KN", DonPAKN.LoaiCot, DonPAKN.CapID, DonPAKN.TrongKy, DonPAKN.CungKy, DonPAKN.DaXuLy, DonPAKN.DaGiaiQuyet, DonPAKN.ChuaGiaiQuyet));
                        DashBoardData.SoLieuBieuDoCot.Add(new BieuDoCot("Đơn khiếu nại", DonKN.LoaiCot, DonKN.CapID, DonKN.TrongKy, DonKN.CungKy, DonKN.DaXuLy, DonKN.DaGiaiQuyet, DonKN.ChuaGiaiQuyet));
                        DashBoardData.SoLieuBieuDoCot.Add(new BieuDoCot("Đơn tố cáo", DonTC.LoaiCot, DonTC.CapID, DonTC.TrongKy, DonTC.CungKy, DonTC.DaXuLy, DonTC.DaGiaiQuyet, DonTC.ChuaGiaiQuyet));

                        TongSoLuotTiepToanTinh.TrongKy += TongSoLuotTiep.TrongKy;
                        TongSoLuotTiepToanTinh.DaXuLy += TongSoLuotTiep.DaXuLy;
                        TongSoLuotTiepToanTinh.DaGiaiQuyet += TongSoLuotTiep.DaGiaiQuyet;
                        TongSoLuotTiepToanTinh.ChuaGiaiQuyet += TongSoLuotTiep.ChuaGiaiQuyet;

                        DonPAKNToanTinh.TrongKy += DonPAKN.TrongKy;
                        DonPAKNToanTinh.DaXuLy += DonPAKN.DaXuLy;
                        DonPAKNToanTinh.DaGiaiQuyet += DonPAKN.DaGiaiQuyet;
                        DonPAKNToanTinh.ChuaGiaiQuyet += DonPAKN.ChuaGiaiQuyet;

                        DonKNToanTinh.TrongKy += DonKN.TrongKy;
                        DonKNToanTinh.DaXuLy += DonKN.DaXuLy;
                        DonKNToanTinh.DaGiaiQuyet += DonKN.DaGiaiQuyet;
                        DonKNToanTinh.ChuaGiaiQuyet += DonKN.ChuaGiaiQuyet;

                        DonTCToanTinh.TrongKy += DonTC.TrongKy;
                        DonTCToanTinh.DaXuLy += DonTC.DaXuLy;
                        DonTCToanTinh.DaGiaiQuyet += DonTC.DaGiaiQuyet;
                        DonTCToanTinh.ChuaGiaiQuyet += DonTC.ChuaGiaiQuyet;
                    }
                }

                if (checkCapToanTinh)
                {
                    DashBoardData.SoLieuBieuDoCot.Add(new BieuDoCot("Tổng số lượt tiếp", TongSoLuotTiepToanTinh.LoaiCot, TongSoLuotTiepToanTinh.CapID, TongSoLuotTiepToanTinh.TrongKy, TongSoLuotTiepToanTinh.CungKy, TongSoLuotTiepToanTinh.DaXuLy, TongSoLuotTiepToanTinh.DaGiaiQuyet, TongSoLuotTiepToanTinh.ChuaGiaiQuyet));
                    DashBoardData.SoLieuBieuDoCot.Add(new BieuDoCot("Đơn PA, KN", DonPAKNToanTinh.LoaiCot, DonPAKNToanTinh.CapID, DonPAKNToanTinh.TrongKy, DonPAKNToanTinh.CungKy, DonPAKNToanTinh.DaXuLy, DonPAKNToanTinh.DaGiaiQuyet, DonPAKNToanTinh.ChuaGiaiQuyet));
                    DashBoardData.SoLieuBieuDoCot.Add(new BieuDoCot("Đơn khiếu nại", DonKNToanTinh.LoaiCot, DonKNToanTinh.CapID, DonKNToanTinh.TrongKy, DonKNToanTinh.CungKy, DonKNToanTinh.DaXuLy, DonKNToanTinh.DaGiaiQuyet, DonKNToanTinh.ChuaGiaiQuyet));
                    DashBoardData.SoLieuBieuDoCot.Add(new BieuDoCot("Đơn tố cáo", DonTCToanTinh.LoaiCot, DonTCToanTinh.CapID, DonTCToanTinh.TrongKy, DonTCToanTinh.CungKy, DonTCToanTinh.DaXuLy, DonTCToanTinh.DaGiaiQuyet, DonTCToanTinh.ChuaGiaiQuyet));
                }

                DashBoardData.SoLieuTongHop = new List<Data>();
                DashBoardData.SoLieuTongHop.Add(new Data("Lượt tiếp", ldInfo.TongTiepDan));
                DashBoardData.SoLieuTongHop.Add(new Data("Đã xử lý", ldInfo.TongXuLyDon));
                DashBoardData.SoLieuTongHop.Add(new Data("Đã giải quyết", TongSoLuotTiepToanTinh.DaGiaiQuyet));
                DashBoardData.SoLieuTongHop.Add(new Data("Đơn phản ánh, KN", ldInfo.TongDonThuBHGQ_KNPA));
                DashBoardData.SoLieuTongHop.Add(new Data("Đơn khiếu nại", ldInfo.TongDonThuBHGQ_KN));
                DashBoardData.SoLieuTongHop.Add(new Data("Đơn tố cáo", ldInfo.TongDonThuBHGQ_TC));
            }

            DashBoardData.ListCapID = DashBoardData.SoLieuBieuDoCot.Select(x => x.CapID).Distinct().ToList();
            DashBoardData.lsData = lsData;
            //});
            //t1.Start();

            //Thread t2 = new Thread(() =>
            //{
            //    DBGetTinhHinhTD_XLD_GQ(p.CoQuanID ?? 0, p.RoleID ?? 0, p.CapID ?? 0, p.TinhID ?? 0, p.HuyenID ?? 0, p.TuNgayCungKy, p.DenNgayCungKy, p.CapIDSelect, p.CoQuanIDSelect, ref lsDataOld, ref ldInfoOld);
            //});
            //t2.Start();

            //t1.Join();
            //t2.Join();

            #region old
            //if (ldInfoOld != null)
            //{
            //    DashBoardData.SoLieuBieuDoTronCungKy = new List<Data>();
            //    int tong = ldInfoOld.TongDonThuBHGQ_KNPA + ldInfoOld.TongDonThuBHGQ_KN + ldInfoOld.TongDonThuBHGQ_TC;
            //    if (tong > 0)
            //    {
            //        decimal tilePAKN = Math.Round(((decimal)(ldInfoOld.TongDonThuBHGQ_KNPA * 100) / tong), 2);
            //        decimal tileKN = Math.Round(((decimal)(ldInfoOld.TongDonThuBHGQ_KN * 100) / tong), 2);
            //        decimal tileTC = 0;
            //        if (ldInfoOld.TongDonThuBHGQ_TC > 0)
            //        {
            //            tileTC = 100 - tilePAKN - tileKN;
            //        }

            //        DashBoardData.SoLieuBieuDoTronCungKy.Add(new Data("Đơn phản ánh, kiến nghị", tilePAKN));
            //        DashBoardData.SoLieuBieuDoTronCungKy.Add(new Data("Đơn khiếu nại", tileKN));
            //        DashBoardData.SoLieuBieuDoTronCungKy.Add(new Data("Đơn tố cáo", tileTC));
            //    }
            //}
            ////update sl cung ky
            //if(lsDataOld.Count > 0)
            //{
            //    DashBoardModel DataCungKy = new DashBoardModel();
            //    DataCungKy.SoLieuBieuDoCot = new List<BieuDoCot>();
            //    Boolean checkCapToanTinh = false;

            //    var TongSoLuotTiepToanTinh = new BieuDoCot("Tổng số lượt tiếp", 1, CapQuanLy.CapToanTinh.GetHashCode(), 0, 0, 0, 0, 0);
            //    var DonPAKNToanTinh = new BieuDoCot("Đơn PA, KN", 2, CapQuanLy.CapToanTinh.GetHashCode(), 0, 0, 0, 0, 0);
            //    var DonKNToanTinh = new BieuDoCot("Đơn khiếu nại", 3, CapQuanLy.CapToanTinh.GetHashCode(), 0, 0, 0, 0, 0);
            //    var DonTCToanTinh = new BieuDoCot("Đơn tố cáo", 4, CapQuanLy.CapToanTinh.GetHashCode(), 0, 0, 0, 0, 0);

            //    foreach (var item in lsDataOld)
            //    {
            //        if (item.CapID == CapQuanLy.CapToanTinh.GetHashCode())
            //        {
            //            checkCapToanTinh = true;
            //        }
            //        else if (item.CapID == CapQuanLy.CapUBNDTinh.GetHashCode())
            //        {
            //            var TongSoLuotTiep = new BieuDoCot("Tổng số lượt tiếp", 1, CapQuanLy.CapUBNDTinh.GetHashCode(), 0, 0, 0, 0, 0);
            //            var DonPAKN = new BieuDoCot("Đơn PA, KN", 2, CapQuanLy.CapUBNDTinh.GetHashCode(), 0, 0, 0, 0, 0);
            //            var DonKN = new BieuDoCot("Đơn khiếu nại", 3, CapQuanLy.CapUBNDTinh.GetHashCode(), 0, 0, 0, 0, 0);
            //            var DonTC = new BieuDoCot("Đơn tố cáo", 4, CapQuanLy.CapUBNDTinh.GetHashCode(), 0, 0, 0, 0, 0);
            //            if (item.LsByCoQuan != null && item.LsByCoQuan.Count > 0)
            //            {
            //                foreach (var cq in item.LsByCoQuan)
            //                {
            //                    TongSoLuotTiep.TrongKy += cq.SoDonTiepDan;
            //                    DonPAKN.TrongKy += cq.SLPhanAnhKienNghi;
            //                    DonKN.TrongKy += cq.SLKhieuNai;
            //                    DonTC.TrongKy += cq.SLToCao;
            //                }
            //            }

            //            DataCungKy.SoLieuBieuDoCot.Add(new BieuDoCot("Tổng số lượt tiếp", TongSoLuotTiep.LoaiCot, TongSoLuotTiep.CapID, TongSoLuotTiep.TrongKy, TongSoLuotTiep.CungKy, TongSoLuotTiep.DaXuLy, TongSoLuotTiep.DaGiaiQuyet, TongSoLuotTiep.ChuaGiaiQuyet));
            //            DataCungKy.SoLieuBieuDoCot.Add(new BieuDoCot("Đơn PA, KN", DonPAKN.LoaiCot, DonPAKN.CapID, DonPAKN.TrongKy, DonPAKN.CungKy, DonPAKN.DaXuLy, DonPAKN.DaGiaiQuyet, DonPAKN.ChuaGiaiQuyet));
            //            DataCungKy.SoLieuBieuDoCot.Add(new BieuDoCot("Đơn khiếu nại", DonKN.LoaiCot, DonKN.CapID, DonKN.TrongKy, DonKN.CungKy, DonKN.DaXuLy, DonKN.DaGiaiQuyet, DonKN.ChuaGiaiQuyet));
            //            DataCungKy.SoLieuBieuDoCot.Add(new BieuDoCot("Đơn tố cáo", DonTC.LoaiCot, DonTC.CapID, DonTC.TrongKy, DonTC.CungKy, DonTC.DaXuLy, DonTC.DaGiaiQuyet, DonTC.ChuaGiaiQuyet));

            //            TongSoLuotTiepToanTinh.TrongKy += TongSoLuotTiep.TrongKy;
            //            DonPAKNToanTinh.TrongKy += DonPAKN.TrongKy;
            //            DonKNToanTinh.TrongKy += DonKN.TrongKy;
            //            DonTCToanTinh.TrongKy += DonTC.TrongKy;
            //        }
            //        else if (item.CapID == CapQuanLy.CapSoNganh.GetHashCode())
            //        {
            //            var TongSoLuotTiep = new BieuDoCot("Tổng số lượt tiếp", 1, CapQuanLy.CapSoNganh.GetHashCode(), 0, 0, 0, 0, 0);
            //            var DonPAKN = new BieuDoCot("Đơn PA, KN", 2, CapQuanLy.CapSoNganh.GetHashCode(), 0, 0, 0, 0, 0);
            //            var DonKN = new BieuDoCot("Đơn khiếu nại", 3, CapQuanLy.CapSoNganh.GetHashCode(), 0, 0, 0, 0, 0);
            //            var DonTC = new BieuDoCot("Đơn tố cáo", 4, CapQuanLy.CapSoNganh.GetHashCode(), 0, 0, 0, 0, 0);
            //            if (item.LsByCoQuan != null && item.LsByCoQuan.Count > 0)
            //            {
            //                foreach (var cq in item.LsByCoQuan)
            //                {
            //                    TongSoLuotTiep.TrongKy += cq.SoDonTiepDan;
            //                    DonPAKN.TrongKy += cq.SLPhanAnhKienNghi;
            //                    DonKN.TrongKy += cq.SLKhieuNai;
            //                    DonTC.TrongKy += cq.SLToCao;
            //                }
            //            }

            //            DataCungKy.SoLieuBieuDoCot.Add(new BieuDoCot("Tổng số lượt tiếp", TongSoLuotTiep.LoaiCot, TongSoLuotTiep.CapID, TongSoLuotTiep.TrongKy, TongSoLuotTiep.CungKy, TongSoLuotTiep.DaXuLy, TongSoLuotTiep.DaGiaiQuyet, TongSoLuotTiep.ChuaGiaiQuyet));
            //            DataCungKy.SoLieuBieuDoCot.Add(new BieuDoCot("Đơn PA, KN", DonPAKN.LoaiCot, DonPAKN.CapID, DonPAKN.TrongKy, DonPAKN.CungKy, DonPAKN.DaXuLy, DonPAKN.DaGiaiQuyet, DonPAKN.ChuaGiaiQuyet));
            //            DataCungKy.SoLieuBieuDoCot.Add(new BieuDoCot("Đơn khiếu nại", DonKN.LoaiCot, DonKN.CapID, DonKN.TrongKy, DonKN.CungKy, DonKN.DaXuLy, DonKN.DaGiaiQuyet, DonKN.ChuaGiaiQuyet));
            //            DataCungKy.SoLieuBieuDoCot.Add(new BieuDoCot("Đơn tố cáo", DonTC.LoaiCot, DonTC.CapID, DonTC.TrongKy, DonTC.CungKy, DonTC.DaXuLy, DonTC.DaGiaiQuyet, DonTC.ChuaGiaiQuyet));

            //            TongSoLuotTiepToanTinh.TrongKy += TongSoLuotTiep.TrongKy;             
            //            DonPAKNToanTinh.TrongKy += DonPAKN.TrongKy; 
            //            DonKNToanTinh.TrongKy += DonKN.TrongKy;
            //            DonTCToanTinh.TrongKy += DonTC.TrongKy;
            //        }
            //        else if (item.CapID == CapQuanLy.CapUBNDHuyen.GetHashCode())
            //        {
            //            var TongSoLuotTiep = new BieuDoCot("Tổng số lượt tiếp", 1, CapQuanLy.CapUBNDHuyen.GetHashCode(), 0, 0, 0, 0, 0);
            //            var DonPAKN = new BieuDoCot("Đơn PA, KN", 2, CapQuanLy.CapUBNDHuyen.GetHashCode(), 0, 0, 0, 0, 0);
            //            var DonKN = new BieuDoCot("Đơn khiếu nại", 3, CapQuanLy.CapUBNDHuyen.GetHashCode(), 0, 0, 0, 0, 0);
            //            var DonTC = new BieuDoCot("Đơn tố cáo", 4, CapQuanLy.CapUBNDHuyen.GetHashCode(), 0, 0, 0, 0, 0);
            //            if (item.LsByCapCoQuan != null && item.LsByCapCoQuan.Count > 0)
            //            {
            //                foreach (var huyen in item.LsByCapCoQuan)
            //                {
            //                    if (huyen.LsByCoQuan != null && huyen.LsByCoQuan.Count > 0)
            //                    {
            //                        foreach (var cq in huyen.LsByCoQuan)
            //                        {
            //                            TongSoLuotTiep.TrongKy += cq.SoDonTiepDan;
            //                            DonPAKN.TrongKy += cq.SLPhanAnhKienNghi;
            //                            DonKN.TrongKy += cq.SLKhieuNai;
            //                            DonTC.TrongKy += cq.SLToCao;
            //                        }
            //                    }
            //                }
            //            }

            //            DataCungKy.SoLieuBieuDoCot.Add(new BieuDoCot("Tổng số lượt tiếp", TongSoLuotTiep.LoaiCot, TongSoLuotTiep.CapID, TongSoLuotTiep.TrongKy, TongSoLuotTiep.CungKy, TongSoLuotTiep.DaXuLy, TongSoLuotTiep.DaGiaiQuyet, TongSoLuotTiep.ChuaGiaiQuyet));
            //            DataCungKy.SoLieuBieuDoCot.Add(new BieuDoCot("Đơn PA, KN", DonPAKN.LoaiCot, DonPAKN.CapID, DonPAKN.TrongKy, DonPAKN.CungKy, DonPAKN.DaXuLy, DonPAKN.DaGiaiQuyet, DonPAKN.ChuaGiaiQuyet));
            //            DataCungKy.SoLieuBieuDoCot.Add(new BieuDoCot("Đơn khiếu nại", DonKN.LoaiCot, DonKN.CapID, DonKN.TrongKy, DonKN.CungKy, DonKN.DaXuLy, DonKN.DaGiaiQuyet, DonKN.ChuaGiaiQuyet));
            //            DataCungKy.SoLieuBieuDoCot.Add(new BieuDoCot("Đơn tố cáo", DonTC.LoaiCot, DonTC.CapID, DonTC.TrongKy, DonTC.CungKy, DonTC.DaXuLy, DonTC.DaGiaiQuyet, DonTC.ChuaGiaiQuyet));

            //            TongSoLuotTiepToanTinh.TrongKy += TongSoLuotTiep.TrongKy;      
            //            DonPAKNToanTinh.TrongKy += DonPAKN.TrongKy;         
            //            DonKNToanTinh.TrongKy += DonKN.TrongKy;                 
            //            DonTCToanTinh.TrongKy += DonTC.TrongKy;                 
            //        }
            //        else if (item.CapID == CapQuanLy.CapUBNDXa.GetHashCode())
            //        {
            //            var TongSoLuotTiep = new BieuDoCot("Tổng số lượt tiếp", 1, CapQuanLy.CapUBNDXa.GetHashCode(), 0, 0, 0, 0, 0);
            //            var DonPAKN = new BieuDoCot("Đơn PA, KN", 2, CapQuanLy.CapUBNDXa.GetHashCode(), 0, 0, 0, 0, 0);
            //            var DonKN = new BieuDoCot("Đơn khiếu nại", 3, CapQuanLy.CapUBNDXa.GetHashCode(), 0, 0, 0, 0, 0);
            //            var DonTC = new BieuDoCot("Đơn tố cáo", 4, CapQuanLy.CapUBNDXa.GetHashCode(), 0, 0, 0, 0, 0);
            //            if (item.LsByCapCoQuan != null && item.LsByCapCoQuan.Count > 0)
            //            {
            //                foreach (var huyen in item.LsByCapCoQuan)
            //                {
            //                    if (huyen.LsByCoQuan != null && huyen.LsByCoQuan.Count > 0)
            //                    {
            //                        foreach (var cq in huyen.LsByCoQuan)
            //                        {
            //                            TongSoLuotTiep.TrongKy += cq.SoDonTiepDan;
            //                            DonPAKN.TrongKy += cq.SLPhanAnhKienNghi;
            //                            DonKN.TrongKy += cq.SLKhieuNai;
            //                            DonTC.TrongKy += cq.SLToCao;
            //                        }
            //                    }
            //                }
            //            }

            //            DataCungKy.SoLieuBieuDoCot.Add(new BieuDoCot("Tổng số lượt tiếp", TongSoLuotTiep.LoaiCot, TongSoLuotTiep.CapID, TongSoLuotTiep.TrongKy, TongSoLuotTiep.CungKy, TongSoLuotTiep.DaXuLy, TongSoLuotTiep.DaGiaiQuyet, TongSoLuotTiep.ChuaGiaiQuyet));
            //            DataCungKy.SoLieuBieuDoCot.Add(new BieuDoCot("Đơn PA, KN", DonPAKN.LoaiCot, DonPAKN.CapID, DonPAKN.TrongKy, DonPAKN.CungKy, DonPAKN.DaXuLy, DonPAKN.DaGiaiQuyet, DonPAKN.ChuaGiaiQuyet));
            //            DataCungKy.SoLieuBieuDoCot.Add(new BieuDoCot("Đơn khiếu nại", DonKN.LoaiCot, DonKN.CapID, DonKN.TrongKy, DonKN.CungKy, DonKN.DaXuLy, DonKN.DaGiaiQuyet, DonKN.ChuaGiaiQuyet));
            //            DataCungKy.SoLieuBieuDoCot.Add(new BieuDoCot("Đơn tố cáo", DonTC.LoaiCot, DonTC.CapID, DonTC.TrongKy, DonTC.CungKy, DonTC.DaXuLy, DonTC.DaGiaiQuyet, DonTC.ChuaGiaiQuyet));

            //            TongSoLuotTiepToanTinh.TrongKy += TongSoLuotTiep.TrongKy;   
            //            DonPAKNToanTinh.TrongKy += DonPAKN.TrongKy;
            //            DonKNToanTinh.TrongKy += DonKN.TrongKy;                       
            //            DonTCToanTinh.TrongKy += DonTC.TrongKy;
            //        }
            //    }

            //    if (checkCapToanTinh)
            //    {
            //        DataCungKy.SoLieuBieuDoCot.Add(new BieuDoCot("Tổng số lượt tiếp", TongSoLuotTiepToanTinh.LoaiCot, TongSoLuotTiepToanTinh.CapID, TongSoLuotTiepToanTinh.TrongKy, TongSoLuotTiepToanTinh.CungKy, TongSoLuotTiepToanTinh.DaXuLy, TongSoLuotTiepToanTinh.DaGiaiQuyet, TongSoLuotTiepToanTinh.ChuaGiaiQuyet));
            //        DataCungKy.SoLieuBieuDoCot.Add(new BieuDoCot("Đơn PA, KN", DonPAKNToanTinh.LoaiCot, DonPAKNToanTinh.CapID, DonPAKNToanTinh.TrongKy, DonPAKNToanTinh.CungKy, DonPAKNToanTinh.DaXuLy, DonPAKNToanTinh.DaGiaiQuyet, DonPAKNToanTinh.ChuaGiaiQuyet));
            //        DataCungKy.SoLieuBieuDoCot.Add(new BieuDoCot("Đơn khiếu nại", DonKNToanTinh.LoaiCot, DonKNToanTinh.CapID, DonKNToanTinh.TrongKy, DonKNToanTinh.CungKy, DonKNToanTinh.DaXuLy, DonKNToanTinh.DaGiaiQuyet, DonKNToanTinh.ChuaGiaiQuyet));
            //        DataCungKy.SoLieuBieuDoCot.Add(new BieuDoCot("Đơn tố cáo", DonTCToanTinh.LoaiCot, DonTCToanTinh.CapID, DonTCToanTinh.TrongKy, DonTCToanTinh.CungKy, DonTCToanTinh.DaXuLy, DonTCToanTinh.DaGiaiQuyet, DonTCToanTinh.ChuaGiaiQuyet));
            //    }

            //    if(DataCungKy.SoLieuBieuDoCot.Count > 0)
            //    {
            //        foreach (var item in DashBoardData.SoLieuBieuDoCot)
            //        {
            //            foreach (var old in DataCungKy.SoLieuBieuDoCot)
            //            {
            //                if(item.CapID == old.CapID && item.LoaiCot == old.LoaiCot)
            //                {
            //                    item.CungKy = old.TrongKy;
            //                    foreach (var child in item.Data)
            //                    {
            //                        if(child.Key == "Cùng kỳ") child.Value = old.TrongKy;
            //                    }
            //                }
            //            }
            //        }
            //    }

            //}
            #endregion

            #region VuViecCanGiaiQuyet
            //int CapUBND = 0;
            //int CapThanhTra = 1;
            //string QuyTrinhGQ = "false";
            //int _QuyTrinhGQ = 0;
            //List<int> docIDList = new List<int>();
            //List<DocumentModel> docList = GetDocumentByState(Constant.CV_XuLy, DateTime.MinValue, DateTime.MinValue);
            //foreach (var docModel in docList)
            //{
            //    docIDList.Add(docModel.DocumentID);
            //}

            //if (CanBoID != 0 && CoQuanID != 0)
            //{
            //    if (RoleID == 1)
            //    {
            //        var loadDTPhanXL = Count_DTCanPhanXL(CanBoID, CoQuanID, RoleID, PhongBanID);
            //        var loadDTDuyetXL = Count_DTCanDuyetXL(CanBoID, CoQuanID, RoleID, PhongBanID);
            //        var loadDTPhanGQ = Count_DTCanPhanGQ(CoQuanID, CapUBND, CapThanhTra, QuyTrinhGQ);
            //        var loadDTDuyetGQ = Count_DTCanDuyetGQ(CoQuanID);
            //        var loadVBDonDoc = Count_VBDonDoc(CoQuanID);
            //        var loadDTPhanHoi = Count_PhanHoi_LD(CoQuanID, CapUBND, CapThanhTra, _QuyTrinhGQ);

            //        DashBoardData.SoLieuTongHop.Add(new Data("", loadDTPhanXL));
            //    }
            //    else if (RoleID == 2)
            //    {
            //        var loadDTPhanXL = Count_DTCanPhanXL(CanBoID, CoQuanID, RoleID, PhongBanID);
            //        var loadDTDuyetXL = Count_DTCanDuyetXL(CanBoID, CoQuanID, RoleID, PhongBanID);
            //        var loadDTPhanGQ = Count_DTCanPhanGQ_TP(CoQuanID, CanBoID, CapUBND, CapThanhTra);
            //        var loadDTDuyetGQ = Count_DTCanDuyetGQ_TP(CoQuanID, CanBoID);
            //        var loadDTCanXL = GetDTCanXuLy_New(CoQuanID, CanBoID, docIDList).Where(x => x.StateID == (int)EnumState.ChuyenVienXL && x.HuongGiaiQuyetID == 0).Count();
            //        var loadDTCanGQ = Count_DTCanGQ(CanBoID, CoQuanID);
            //        var loadDTPhanHoi = Count_PhanHoi_TP(CoQuanID, CanBoID);

            //    }
            //    else if (RoleID == 3)
            //    {
            //        var loadDTCanXL = GetDTCanXuLy_New(CoQuanID, CanBoID, docIDList).Where(x => x.StateID == (int)EnumState.ChuyenVienXL && x.HuongGiaiQuyetID == 0).Count(); 
            //        var loadDTCanGQ = Count_DTCanGQ(CanBoID, CoQuanID);
            //        var loadDTCQKhacChuyenDen = Count_DTCQKhacChuyenDen(CanBoID, CoQuanID);
            //    }
            //}
            #endregion

            return DashBoardData;
        }

        public DashBoardModel GetDuLieuDashBoard_New(DashBoardParams p)
        {
            DashBoardModel DashBoardData = new DashBoardModel();
            DashBoardData.SoLieuTongHop = new List<Data>();
            DashBoardData.SoLieuBieuDoCot = new List<BieuDoCot>();
            DashBoardData.SoLieuBieuTron = new List<Data>();
            DashBoardData.ListCapID = new List<int>();

            DashBoardData.SoLieuTongHop.Add(new Data("Lượt tiếp", 0));
            DashBoardData.SoLieuTongHop.Add(new Data("Đã xử lý", 0));
            DashBoardData.SoLieuTongHop.Add(new Data("Đã giải quyết", 0));
            DashBoardData.SoLieuTongHop.Add(new Data("Đơn phản ánh, KN", 0));
            DashBoardData.SoLieuTongHop.Add(new Data("Đơn khiếu nại", 0));
            DashBoardData.SoLieuTongHop.Add(new Data("Đơn tố cáo", 0));
            DashBoardData.SoLieuTongHop.Add(new Data("Đoàn đông người", 0));

            DateTime? TuNgay = Utils.ConvertToNullableDateTime(p.TuNgay, null);
            DateTime? DenNgay = Utils.ConvertToNullableDateTime(p.DenNgay, null);
            int capID = p.CapID ?? 0;
            int tinhID = p.TinhID ?? 0;
            bool flag = true;
            bool flagCapXa = true;
            bool flagToanHuyen = true;
            bool laThanhTraTinh = false;
            var listThanhTraTinh = new SystemConfigDAL().GetByKey("Thanh_Tra_Tinh_ID").ConfigValue.Split(',').ToList().Select(x => Utils.ConvertToInt32(x.ToString(), 0)).ToList();
            if (listThanhTraTinh.Contains(p.CoQuanID ?? 0) && p.RoleID == (int)EnumChucVu.LanhDao)
            {
                laThanhTraTinh = true;
            }

            List<CoQuanInfo> cqList = new List<CoQuanInfo>();
            int CoQuanChaID = 0;

            if (laThanhTraTinh || capID == (int)CapQuanLy.CapUBNDTinh)
            {
                if (p.RoleID == (int)EnumChucVu.LanhDao)
                {
                    var CoQuanChaPhuHop = new CoQuan().GetCoQuanByTinhID_New(p.TinhID);
                    var cqList1 = new CoQuan().GetAllCapCon(CoQuanChaPhuHop.CoQuanID).ToList();
                    var cqList2 = cqList1.Where(x => x.CapID == CapQuanLy.CapUBNDHuyen.GetHashCode()).ToList();
                    cqList.AddRange(cqList2.Where(x => new CoQuan().GetAllCapCon(x.CoQuanID).ToList().Where(y => y.SuDungPM == true).ToList().Count > 0).Select(x => x));
                    cqList.AddRange(cqList1.Where(x => (x.CapID != (int)CapQuanLy.CapUBNDHuyen) && x.SuDungPM == true).Select(x => x));

                    CoQuanChaID = CoQuanChaPhuHop.CoQuanID;
                }
                else
                {
                    cqList = new List<CoQuanInfo> { new CoQuan().GetCoQuanByID(p.CoQuanID ?? 0) };
                    cqList = cqList.Where(x => x.SuDungPM == true).ToList();
                }
            }
            else if (capID == (int)CapQuanLy.CapUBNDHuyen)
            {
                if (p.RoleID == (int)EnumChucVu.LanhDao)
                {
                    var CoQuanTinh = new CoQuan().GetCoQuanByTinhID_New(p.TinhID);
                    var CoQuanChaPhuHop = new CoQuan().GetCoQuanByHuyenID_New(p.HuyenID, p.TinhID, CoQuanTinh.CoQuanID);
                    cqList = new CoQuan().GetAllCapCon(CoQuanChaPhuHop.CoQuanID).ToList();
                    cqList.Where(x => x.SuDungPM == true).ToList();
                    CoQuanChaID = CoQuanChaPhuHop.CoQuanID;
                }
                else
                {
                    var CoQuanTinh = new CoQuan().GetCoQuanByTinhID_New(p.TinhID);
                    var CoQuanChaPhuHop = new CoQuan().GetCoQuanByHuyenID_New(p.HuyenID, p.TinhID, CoQuanTinh.CoQuanID);
                    cqList = new CoQuan().GetAllCapCon(CoQuanChaPhuHop.CoQuanID).Where(x => (x.CapID == CapQuanLy.CapUBNDHuyen.GetHashCode()
                     || x.CapID == CapQuanLy.CapPhong.GetHashCode())).ToList();
                    cqList.Where(x => x.SuDungPM == true).ToList();
                }
            }
            else if (capID == (int)CapQuanLy.CapSoNganh)
            {
                cqList = new List<CoQuanInfo> { new CoQuan().GetCoQuanByID(p.CoQuanID ?? 0) };
            }
            else if (capID == (int)CapQuanLy.CapUBNDXa || capID == (int)CapQuanLy.CapPhong)
            {
                cqList = new List<CoQuanInfo> { new CoQuan().GetCoQuanByID(p.CoQuanID ?? 0) };
                cqList = cqList.Where(x => x.SuDungPM == true).ToList();
            }

            var pList = new SqlParameter("@ListCoQuanID", SqlDbType.Structured);
            pList.TypeName = "dbo.IntList";
            var tbCoQuanID = new DataTable();
            tbCoQuanID.Columns.Add("CoQuanID", typeof(string));
            cqList.ForEach(x => tbCoQuanID.Rows.Add(x.CoQuanID));
            //
            var pListCon = new SqlParameter("@ListCoQuanConID", SqlDbType.Structured);
            pListCon.TypeName = "dbo.OneID";
            var tbCoQuanConID = new DataTable();
            tbCoQuanConID.Columns.Add("CoQuanID", typeof(string));

            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter("@StartDate", SqlDbType.DateTime),
                new SqlParameter("@EndDate", SqlDbType.DateTime),
                pList,
                new SqlParameter("@Flag", SqlDbType.Int),
                new SqlParameter("@CoQuanChaID", SqlDbType.Int)
            };
            parm[0].Value = TuNgay ?? Convert.DBNull;
            parm[1].Value = DenNgay ?? Convert.DBNull;
            parm[2].Value = tbCoQuanID;
            parm[3].Value = 1;
            parm[4].Value = CoQuanChaID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_DashBoard_DuLieuTongHop_New", parm))
                {
                    while (dr.Read())
                    {
                        int SLLuotTiep = Utils.ConvertToInt32(dr["SLLuotTiep"], 0);
                        int SLDaXuLy = Utils.ConvertToInt32(dr["SLDaXuLy"], 0);
                        int SLDaGiaiQuyet = Utils.ConvertToInt32(dr["SLDaGiaiQuyet"], 0);
                        int SLKhieuNai = Utils.ConvertToInt32(dr["SLKhieuNai"], 0);
                        int SLToCao = Utils.ConvertToInt32(dr["SLToCao"], 0);
                        int SLPhanAnhKienNghi = Utils.ConvertToInt32(dr["SLPhanAnhKienNghi"], 0);
                        int SLDoanDongNguoi = Utils.ConvertToInt32(dr["SLDoanDongNguoi"], 0);

                        DashBoardData.SoLieuTongHop = new List<Data>();
                        DashBoardData.SoLieuTongHop.Add(new Data("Lượt tiếp", SLLuotTiep));
                        DashBoardData.SoLieuTongHop.Add(new Data("Đã xử lý", SLDaXuLy));
                        DashBoardData.SoLieuTongHop.Add(new Data("Đã giải quyết", SLDaGiaiQuyet));
                        DashBoardData.SoLieuTongHop.Add(new Data("Đơn phản ánh, KN", SLPhanAnhKienNghi));
                        DashBoardData.SoLieuTongHop.Add(new Data("Đơn khiếu nại", SLKhieuNai));
                        DashBoardData.SoLieuTongHop.Add(new Data("Đơn tố cáo", SLToCao));
                        DashBoardData.SoLieuTongHop.Add(new Data("Đoàn đông người", SLDoanDongNguoi));
                    }
                    dr.Close();
                }
            }
            catch (Exception e)
            {
                throw;
            }


            return DashBoardData;
        }

        public string DBGetTinhHinhTD_XLD_GQ(int CoQuanID, int RoleID, int CapID, int TinhID, int HuyenID, string tuNgay, string denNgay, string capIDSelect, string coQuanIDSelect, ref List<BCTinhHinhTD_XLD_GQInfo> lsData, ref DBSuDungPhanMemTongInfo ldInfo)
        {
            //List<BCTinhHinhTD_XLD_GQInfo> lsData = new List<BCTinhHinhTD_XLD_GQInfo>();
            //DBSuDungPhanMemTongInfo ldInfo = new DBSuDungPhanMemTongInfo();
            DateTime tuNgays = Utils.ConvertToDateTime(tuNgay, DateTime.MinValue);
            DateTime denNgays = Utils.ConvertToDateTime(denNgay, DateTime.MinValue);
            int capUser = CapID;
            int coQuanUserID = CoQuanID;
            int pRoleUser = RoleID;
            int tinhID = TinhID;
            int pCapIDSelect = Utils.ConvertToInt32(capIDSelect, 0);
            int pCoQuanIDSelect = Utils.ConvertToInt32(coQuanIDSelect, 0);

            Boolean laThanhTraTinh = false;
            var listThanhTraTinh = new SystemConfigDAL().GetByKey("Thanh_Tra_Tinh_ID").ConfigValue.Split(',').ToList().Select(x => Utils.ConvertToInt32(x.ToString(), 0)).ToList();
            if (listThanhTraTinh.Contains(CoQuanID) && RoleID == (int)EnumChucVu.LanhDao)
            {
                laThanhTraTinh = true;
            }

            bool hidenCap = false;
            /*Phân xem theo cấp và role*/
            if ((capUser == (int)CapQuanLy.CapUBNDTinh || capUser == (int)CapQuanLy.CapUBNDHuyen || coQuanUserID == (int)EnumCoQuan.ThanhTraTinh) && pRoleUser == (int)EnumChucVu.LanhDao)
            {
                if (pCapIDSelect == 3)
                {
                    capUser = 1;
                    coQuanUserID = pCoQuanIDSelect;
                }
                else if (pCapIDSelect == 4)
                {
                    capUser = 2;
                    coQuanUserID = pCoQuanIDSelect;

                }
                else if (pCapIDSelect == 5)
                {
                    capUser = 6;
                    coQuanUserID = pCoQuanIDSelect;

                }
            }


            QueryFilterInfo infoQF = new QueryFilterInfo()
            {
                TuNgayGoc = tuNgays,
                TuNgayMoi = tuNgays,
                DenNgayGoc = denNgays,
                DenNgayMoi = denNgays.AddDays(1),
                CoQuanID = coQuanUserID,
                PTKQDung = (int)PhanTichKQEnum.Dung,
                PTKQDungMotPhan = (int)PhanTichKQEnum.DungMotPhan,
                PTKQSai = (int)PhanTichKQEnum.Sai,
            };
            #region
            try
            {
                IList<BCTongHopXuLyInfo> xldList = DashBoard_ThongKeXuLyDon_GetByDate(infoQF);
                IList<BCTongHopXuLyInfo> gqdList = DashBoard_ThongKeGiaiQuyetDon_GetByDate(infoQF);

                //List<KeKhaiDuLieuDauKy_2aInfo> duLieuDauKy2aList = new DAL.BaoCao.KeKhaiDuLieuDauKy_2a().GetByDate(infoQF.TuNgayGoc, infoQF.DenNgayGoc);
                //List<KeKhaiDuLieuDauKy_2bInfo> duLieuDauKy2bList = new DAL.BaoCao.KeKhaiDuLieuDauKy_2b().GetByDate(infoQF.TuNgayGoc, infoQF.DenNgayGoc);
                //List<KeKhaiDuLieuDauKy_2cInfo> duLieuDauKy2cList = new DAL.BaoCao.KeKhaiDuLieuDauKy_2c().GetByDate(infoQF.TuNgayGoc, infoQF.DenNgayGoc);
                //List<KeKhaiDuLieuDauKy_2dInfo> duLieuDauKy2dList = new DAL.BaoCao.KeKhaiDuLieuDauKy_2d().GetByDate(infoQF.TuNgayGoc, infoQF.DenNgayGoc);

                if (laThanhTraTinh || capUser == (int)CapQuanLy.CapUBNDTinh || coQuanUserID == (int)EnumCoQuan.BanTCDTinh || (pCapIDSelect != 3 && coQuanUserID == (int)EnumCoQuan.ThanhTraTinh && pRoleUser == (int)EnumChucVu.LanhDao))
                {
                    if (coQuanUserID == (int)EnumCoQuan.BanTCDTinh && pRoleUser != (int)EnumChucVu.LanhDao)
                    {
                        hidenCap = true;
                    }
                    else
                    {
                        hidenCap = false;
                    }


                    List<BCTinhHinhTD_XLD_GQInfo> tempList = new CapDAL().GetAllCap().ToList();
                    if (pRoleUser == (int)EnumChucVu.LanhDao || laThanhTraTinh)
                    {
                        lsData = tempList.Where(x => x.CapID != (int)CapQuanLy.CapTrungUong && x.CapID != (int)CapQuanLy.CapPhong).ToList();
                    }
                    else
                    {
                        lsData = tempList.Where(x => x.CapID != (int)CapQuanLy.CapTrungUong && x.CapID != (int)CapQuanLy.CapPhong && x.CapID != (int)CapQuanLy.CapUBNDXa && x.CapID != (int)CapQuanLy.CapUBNDHuyen && x.CapID != (int)CapQuanLy.CapSoNganh).ToList();
                    }

                    foreach (BCTinhHinhTD_XLD_GQInfo item in lsData)
                    {
                        if (item.CapID == (int)CapQuanLy.CapUBNDHuyen)
                        {
                            List<HuyenInfo> huyenList = new Huyen().GetByTinh(TinhID).ToList();
                            List<BCTinhHinhTD_XLD_GQInfo> lstCQCapPhong = new CoQuan().GetCoQuanByCapForBC((int)CapQuanLy.CapPhong, tinhID).ToList();
                            List<BCTinhHinhTD_XLD_GQInfo> lstCQCapUBHuyen = new CoQuan().GetCoQuanByCapForBC((int)CapQuanLy.CapUBNDHuyen, tinhID).ToList();
                            List<BCTinhHinhTD_XLD_GQInfo> lstCQCapHuyen = new List<BCTinhHinhTD_XLD_GQInfo>();
                            foreach (BCTinhHinhTD_XLD_GQInfo itemCoQuan in lstCQCapUBHuyen)
                            {
                                lstCQCapHuyen.Add(itemCoQuan);
                            }
                            foreach (BCTinhHinhTD_XLD_GQInfo itemCoQuan in lstCQCapPhong)
                            {
                                lstCQCapHuyen.Add(itemCoQuan);
                            }

                            item.LsByCapCoQuan = new List<BCTinhHinhTD_XLD_CapCoQuan_GQInfo>();
                            foreach (HuyenInfo huyenInfo in huyenList)
                            {
                                BCTinhHinhTD_XLD_CapCoQuan_GQInfo lstXLDCapCoQuan = new BCTinhHinhTD_XLD_CapCoQuan_GQInfo();
                                lstXLDCapCoQuan.TenCoQuan = huyenInfo.TenHuyen;
                                lstXLDCapCoQuan.CoQuanID = huyenInfo.HuyenID;
                                lstXLDCapCoQuan.LsByCoQuan = lstCQCapHuyen.Where(x => x.HuyenID == huyenInfo.HuyenID).ToList();

                                foreach (BCTinhHinhTD_XLD_GQInfo itemCoQuan in lstXLDCapCoQuan.LsByCoQuan)
                                {
                                    foreach (BCTongHopXuLyInfo raw in xldList)
                                    {
                                        if (itemCoQuan.CoQuanID == raw.CoQuanID)
                                        {
                                            itemCoQuan.SoDonTiepDan = raw.SLTiepCongDan;
                                            itemCoQuan.SLKhieuNai = raw.SLKhieuNai;
                                            itemCoQuan.SLToCao = raw.SLToCao;
                                            itemCoQuan.SLPhanAnhKienNghi = raw.SLPhanAnhKienNghi;
                                            itemCoQuan.SoXLTrongHan = raw.XLDDaXuLyTrongHan;
                                            itemCoQuan.SoXLQuaHan = raw.XLDDaXuLyQuaHan;
                                            itemCoQuan.SoXLDaXuLy = raw.XLDDaXuLy;
                                            itemCoQuan.SoXLChuaXuLy = raw.XLDChuaXuLy;
                                            itemCoQuan.TongSoXL = raw.XLDTongSo;
                                            itemCoQuan.VuViecDongNguoi = raw.XLDKhieuKienDN;
                                            itemCoQuan.XLDKhieuNai = raw.XLDKhieuNai;
                                            itemCoQuan.XLDToCao = raw.XLDToCao;
                                            itemCoQuan.XLDPhanAnhKienNghi = raw.XLDPhanAnhKienNghi;
                                        }
                                    }
                                    foreach (BCTongHopXuLyInfo raw in gqdList)
                                    {
                                        var cqphItem = "," + itemCoQuan.CoQuanID + ",";
                                        if (raw.CoQuanGiaiQuyetID == 0)
                                            raw.CoQuanGiaiQuyetID = raw.CoQuanID;
                                        if (itemCoQuan.CoQuanID == raw.CoQuanGiaiQuyetID)
                                        {

                                            itemCoQuan.TongSoDonGiaoGQ += raw.GQDTongSo;
                                            itemCoQuan.SoDangGQTrongHan += raw.GQDDangGQTrongHan;
                                            itemCoQuan.SoDangGQQuaHan += raw.GQDDangGQQuaHan;
                                            itemCoQuan.TongSoDangGQ += raw.GQDDangGQ;
                                            itemCoQuan.DaCoBC += raw.GQDDaGQ;
                                            itemCoQuan.ChuaGiaiQuyet += raw.GQDChuaGQ;
                                            itemCoQuan.DaBHGQDung += raw.KQKNDung;
                                            itemCoQuan.DaBHGQDungMotPhan += raw.KQKNDungMotPhan;
                                            itemCoQuan.DaBHGQSai += raw.KQKNSai;
                                            itemCoQuan.DaBHGQ = (itemCoQuan.DaBHGQDung + itemCoQuan.DaBHGQDungMotPhan + itemCoQuan.DaBHGQSai);


                                            itemCoQuan.GQDKNDangGQ += raw.GQDKNDangGQ;
                                            itemCoQuan.GQDTCDangGQ += raw.GQDTCDangGQ;
                                            itemCoQuan.GQDKNPADangGQ += raw.GQDKNPADangGQ;
                                            itemCoQuan.GQDKNDaGQ += raw.GQDKNDaGQ;
                                            itemCoQuan.GQDTCDaGQ += raw.GQDTCDaGQ;
                                            itemCoQuan.GQDKNPADaGQ += raw.GQDKNPADaGQ;
                                            itemCoQuan.GQDKNChuaGQ += raw.GQDKNChuaGQ;
                                            itemCoQuan.GQDTCChuaGQ += raw.GQDTCChuaGQ;
                                            itemCoQuan.GQDKNPAChuaGQ += raw.GQDKNPAChuaGQ;

                                        }
                                    }

                                }
                                item.LsByCapCoQuan.Add(lstXLDCapCoQuan);
                            }
                        }
                        else if (item.CapID == (int)CapQuanLy.CapUBNDXa)
                        {
                            List<HuyenInfo> huyenList = new Huyen().GetByTinh(TinhID).ToList();
                            List<BCTinhHinhTD_XLD_GQInfo> lstCQCapPhong = new CoQuan().GetCoQuanByCapForBC((int)CapQuanLy.CapUBNDXa, tinhID).ToList();
                            item.LsByCapCoQuan = new List<BCTinhHinhTD_XLD_CapCoQuan_GQInfo>();
                            foreach (HuyenInfo huyenInfo in huyenList)
                            {
                                BCTinhHinhTD_XLD_CapCoQuan_GQInfo lstXLDCapCoQuan = new BCTinhHinhTD_XLD_CapCoQuan_GQInfo();
                                lstXLDCapCoQuan.TenCoQuan = huyenInfo.TenHuyen;
                                lstXLDCapCoQuan.CoQuanID = huyenInfo.HuyenID;
                                lstXLDCapCoQuan.LsByCoQuan = lstCQCapPhong.Where(x => x.HuyenID == huyenInfo.HuyenID).ToList();

                                foreach (BCTinhHinhTD_XLD_GQInfo itemCoQuan in lstXLDCapCoQuan.LsByCoQuan)
                                {
                                    foreach (BCTongHopXuLyInfo raw in xldList)
                                    {
                                        if (itemCoQuan.CoQuanID == raw.CoQuanID)
                                        {
                                            itemCoQuan.SoDonTiepDan = raw.SLTiepCongDan;
                                            itemCoQuan.SLKhieuNai = raw.SLKhieuNai;
                                            itemCoQuan.SLToCao = raw.SLToCao;
                                            itemCoQuan.SLPhanAnhKienNghi = raw.SLPhanAnhKienNghi;
                                            itemCoQuan.SoXLTrongHan = raw.XLDDaXuLyTrongHan;
                                            itemCoQuan.SoXLQuaHan = raw.XLDDaXuLyQuaHan;
                                            itemCoQuan.SoXLDaXuLy = raw.XLDDaXuLy;
                                            itemCoQuan.SoXLChuaXuLy = raw.XLDChuaXuLy;
                                            itemCoQuan.TongSoXL = raw.XLDTongSo;
                                            itemCoQuan.VuViecDongNguoi = raw.XLDKhieuKienDN;
                                            itemCoQuan.XLDKhieuNai = raw.XLDKhieuNai;
                                            itemCoQuan.XLDToCao = raw.XLDToCao;
                                            itemCoQuan.XLDPhanAnhKienNghi = raw.XLDPhanAnhKienNghi;

                                        }
                                    }
                                    foreach (BCTongHopXuLyInfo raw in gqdList)
                                    {
                                        var cqphItem = "," + itemCoQuan.CoQuanID + ",";
                                        if (raw.CoQuanGiaiQuyetID == 0)
                                            raw.CoQuanGiaiQuyetID = raw.CoQuanID;
                                        if (itemCoQuan.CoQuanID == raw.CoQuanGiaiQuyetID)
                                        {

                                            itemCoQuan.TongSoDonGiaoGQ += raw.GQDTongSo;
                                            itemCoQuan.SoDangGQTrongHan += raw.GQDDangGQTrongHan;
                                            itemCoQuan.SoDangGQQuaHan += raw.GQDDangGQQuaHan;
                                            itemCoQuan.TongSoDangGQ += raw.GQDDangGQ;
                                            itemCoQuan.DaCoBC += raw.GQDDaGQ;
                                            itemCoQuan.ChuaGiaiQuyet += raw.GQDChuaGQ;
                                            itemCoQuan.DaBHGQDung += raw.KQKNDung;
                                            itemCoQuan.DaBHGQDungMotPhan += raw.KQKNDungMotPhan;
                                            itemCoQuan.DaBHGQSai += raw.KQKNSai;
                                            itemCoQuan.DaBHGQ = (itemCoQuan.DaBHGQDung + itemCoQuan.DaBHGQDungMotPhan + itemCoQuan.DaBHGQSai);


                                            itemCoQuan.GQDKNDangGQ += raw.GQDKNDangGQ;
                                            itemCoQuan.GQDTCDangGQ += raw.GQDTCDangGQ;
                                            itemCoQuan.GQDKNPADangGQ += raw.GQDKNPADangGQ;
                                            itemCoQuan.GQDKNDaGQ += raw.GQDKNDaGQ;
                                            itemCoQuan.GQDTCDaGQ += raw.GQDTCDaGQ;
                                            itemCoQuan.GQDKNPADaGQ += raw.GQDKNPADaGQ;
                                            itemCoQuan.GQDKNChuaGQ += raw.GQDKNChuaGQ;
                                            itemCoQuan.GQDTCChuaGQ += raw.GQDTCChuaGQ;
                                            itemCoQuan.GQDKNPAChuaGQ += raw.GQDKNPAChuaGQ;
                                        }
                                    }

                                }
                                item.LsByCapCoQuan.Add(lstXLDCapCoQuan);
                            }
                        }
                        else
                        {
                            if (pRoleUser == (int)EnumChucVu.LanhDao)
                            {
                                item.LsByCoQuan = new CoQuan().GetCoQuanByCapForBC(item.CapID, tinhID).ToList();
                            }
                            else
                            {
                                item.LsByCoQuan = new CoQuan().GetCoQuanByCapForBC(item.CapID, tinhID).Where(x => x.CoQuanID == coQuanUserID).ToList();

                            }
                            foreach (BCTinhHinhTD_XLD_GQInfo itemCoQuan in item.LsByCoQuan)
                            {
                                foreach (BCTongHopXuLyInfo raw in xldList)
                                {
                                    if (itemCoQuan.CoQuanID == raw.CoQuanID)
                                    {
                                        itemCoQuan.SoDonTiepDan = raw.SLTiepCongDan;
                                        itemCoQuan.SLKhieuNai = raw.SLKhieuNai;
                                        itemCoQuan.SLToCao = raw.SLToCao;
                                        itemCoQuan.SLPhanAnhKienNghi = raw.SLPhanAnhKienNghi;
                                        itemCoQuan.SoXLTrongHan = raw.XLDDaXuLyTrongHan;
                                        itemCoQuan.SoXLQuaHan = raw.XLDDaXuLyQuaHan;
                                        itemCoQuan.SoXLDaXuLy = raw.XLDDaXuLy;
                                        itemCoQuan.SoXLChuaXuLy = raw.XLDChuaXuLy;
                                        itemCoQuan.TongSoXL = raw.XLDTongSo;
                                        itemCoQuan.VuViecDongNguoi = raw.XLDKhieuKienDN;
                                        itemCoQuan.XLDKhieuNai = raw.XLDKhieuNai;
                                        itemCoQuan.XLDToCao = raw.XLDToCao;
                                        itemCoQuan.XLDPhanAnhKienNghi = raw.XLDPhanAnhKienNghi;
                                    }
                                }
                                foreach (BCTongHopXuLyInfo raw in gqdList)
                                {
                                    var cqphItem = "," + itemCoQuan.CoQuanID + ",";
                                    if (raw.CoQuanGiaiQuyetID == 0)
                                        raw.CoQuanGiaiQuyetID = raw.CoQuanID;
                                    if (itemCoQuan.CoQuanID == raw.CoQuanGiaiQuyetID)
                                    {

                                        itemCoQuan.TongSoDonGiaoGQ += raw.GQDTongSo;
                                        itemCoQuan.SoDangGQTrongHan += raw.GQDDangGQTrongHan;
                                        itemCoQuan.SoDangGQQuaHan += raw.GQDDangGQQuaHan;
                                        itemCoQuan.TongSoDangGQ += raw.GQDDangGQ;
                                        itemCoQuan.DaCoBC += raw.GQDDaGQ;
                                        itemCoQuan.ChuaGiaiQuyet += raw.GQDChuaGQ;
                                        itemCoQuan.DaBHGQDung += raw.KQKNDung;
                                        itemCoQuan.DaBHGQDungMotPhan += raw.KQKNDungMotPhan;
                                        itemCoQuan.DaBHGQSai += raw.KQKNSai;
                                        itemCoQuan.DaBHGQ = (itemCoQuan.DaBHGQDung + itemCoQuan.DaBHGQDungMotPhan + itemCoQuan.DaBHGQSai);


                                        itemCoQuan.GQDKNDangGQ += raw.GQDKNDangGQ;
                                        itemCoQuan.GQDTCDangGQ += raw.GQDTCDangGQ;
                                        itemCoQuan.GQDKNPADangGQ += raw.GQDKNPADangGQ;
                                        itemCoQuan.GQDKNDaGQ += raw.GQDKNDaGQ;
                                        itemCoQuan.GQDTCDaGQ += raw.GQDTCDaGQ;
                                        itemCoQuan.GQDKNPADaGQ += raw.GQDKNPADaGQ;
                                        itemCoQuan.GQDKNChuaGQ += raw.GQDKNChuaGQ;
                                        itemCoQuan.GQDTCChuaGQ += raw.GQDTCChuaGQ;
                                        itemCoQuan.GQDKNPAChuaGQ += raw.GQDKNPAChuaGQ;
                                    }
                                }

                            }
                        }

                    }
                }
                else if ((capUser == (int)CapQuanLy.CapSoNganh && coQuanUserID != (int)EnumCoQuan.BanTCDTinh) || capUser == (int)CapQuanLy.CapPhong || capUser == (int)CapQuanLy.CapUBNDXa)
                {
                    List<BCTinhHinhTD_XLD_GQInfo> tempList = new CapDAL().GetAllCap().ToList();
                    lsData = tempList.Where(x => x.CapID == capUser).ToList();
                    foreach (BCTinhHinhTD_XLD_GQInfo item in lsData)
                    {
                        if (coQuanUserID == 0)
                        {
                            hidenCap = false;
                            item.LsByCoQuan = new CoQuan().GetCoQuanByCapForBC(item.CapID, tinhID).ToList();
                        }
                        else
                        {
                            hidenCap = true;
                            item.LsByCoQuan = new CoQuan().GetCoQuanByCapForBC(item.CapID, tinhID).Where(x => x.CoQuanID == coQuanUserID).ToList();
                        }
                        foreach (BCTinhHinhTD_XLD_GQInfo itemCoQuan in item.LsByCoQuan)
                        {
                            foreach (BCTongHopXuLyInfo raw in xldList)
                            {
                                if (itemCoQuan.CoQuanID == raw.CoQuanID)
                                {
                                    itemCoQuan.SoDonTiepDan = raw.SLTiepCongDan;
                                    itemCoQuan.SLKhieuNai = raw.SLKhieuNai;
                                    itemCoQuan.SLToCao = raw.SLToCao;
                                    itemCoQuan.SLPhanAnhKienNghi = raw.SLPhanAnhKienNghi;
                                    itemCoQuan.SoXLTrongHan = raw.XLDDaXuLyTrongHan;
                                    itemCoQuan.SoXLQuaHan = raw.XLDDaXuLyQuaHan;
                                    itemCoQuan.SoXLDaXuLy = raw.XLDDaXuLy;
                                    itemCoQuan.SoXLChuaXuLy = raw.XLDChuaXuLy;
                                    itemCoQuan.TongSoXL = raw.XLDTongSo;
                                    itemCoQuan.VuViecDongNguoi = raw.XLDKhieuKienDN;
                                    itemCoQuan.XLDKhieuNai = raw.XLDKhieuNai;
                                    itemCoQuan.XLDToCao = raw.XLDToCao;
                                    itemCoQuan.XLDPhanAnhKienNghi = raw.XLDPhanAnhKienNghi;
                                }
                            }
                            foreach (BCTongHopXuLyInfo raw in gqdList)
                            {
                                var cqphItem = "," + itemCoQuan.CoQuanID + ",";
                                if (raw.CoQuanGiaiQuyetID == 0)
                                    raw.CoQuanGiaiQuyetID = raw.CoQuanID;
                                if (itemCoQuan.CoQuanID == raw.CoQuanGiaiQuyetID)
                                {
                                    itemCoQuan.TongSoDonGiaoGQ += raw.GQDTongSo;
                                    itemCoQuan.SoDangGQTrongHan += raw.GQDDangGQTrongHan;
                                    itemCoQuan.SoDangGQQuaHan += raw.GQDDangGQQuaHan;
                                    itemCoQuan.TongSoDangGQ += raw.GQDDangGQ;
                                    itemCoQuan.DaCoBC += raw.GQDDaGQ;
                                    itemCoQuan.ChuaGiaiQuyet += raw.GQDChuaGQ;
                                    itemCoQuan.DaBHGQDung += raw.KQKNDung;
                                    itemCoQuan.DaBHGQDungMotPhan += raw.KQKNDungMotPhan;
                                    itemCoQuan.DaBHGQSai += raw.KQKNSai;
                                    itemCoQuan.DaBHGQ = (itemCoQuan.DaBHGQDung + itemCoQuan.DaBHGQDungMotPhan + itemCoQuan.DaBHGQSai);

                                    itemCoQuan.GQDKNDangGQ += raw.GQDKNDangGQ;
                                    itemCoQuan.GQDTCDangGQ += raw.GQDTCDangGQ;
                                    itemCoQuan.GQDKNPADangGQ += raw.GQDKNPADangGQ;
                                    itemCoQuan.GQDKNDaGQ += raw.GQDKNDaGQ;
                                    itemCoQuan.GQDTCDaGQ += raw.GQDTCDaGQ;
                                    itemCoQuan.GQDKNPADaGQ += raw.GQDKNPADaGQ;
                                    itemCoQuan.GQDKNChuaGQ += raw.GQDKNChuaGQ;
                                    itemCoQuan.GQDTCChuaGQ += raw.GQDTCChuaGQ;
                                    itemCoQuan.GQDKNPAChuaGQ += raw.GQDKNPAChuaGQ;
                                }
                            }
                        }
                    }
                }

                else if (capUser == (int)CapQuanLy.CapUBNDHuyen)
                {
                    hidenCap = false;
                    List<BCTinhHinhTD_XLD_GQInfo> tempList = new CapDAL().GetAllCap().ToList();

                    if (coQuanUserID == 0) /*Lãnh đạo tỉnh chọn xem cấp huyện*/
                    {
                        lsData = tempList.Where(x => x.CapID == (int)CapQuanLy.CapUBNDHuyen || x.CapID == (int)CapQuanLy.CapUBNDXa).ToList();
                        foreach (BCTinhHinhTD_XLD_GQInfo item in lsData)
                        {
                            if (item.CapID == (int)CapQuanLy.CapUBNDHuyen)
                            {
                                List<HuyenInfo> huyenList = new Huyen().GetByTinh(TinhID).ToList();
                                List<BCTinhHinhTD_XLD_GQInfo> lstCQCapPhong = new CoQuan().GetCoQuanByCapForBC((int)CapQuanLy.CapPhong, tinhID).ToList();
                                List<BCTinhHinhTD_XLD_GQInfo> lstCQCapUBHuyen = new CoQuan().GetCoQuanByCapForBC((int)CapQuanLy.CapUBNDHuyen, tinhID).ToList();
                                List<BCTinhHinhTD_XLD_GQInfo> lstCQCapHuyen = new List<BCTinhHinhTD_XLD_GQInfo>();
                                foreach (BCTinhHinhTD_XLD_GQInfo itemCoQuan in lstCQCapUBHuyen)
                                {
                                    lstCQCapHuyen.Add(itemCoQuan);
                                }
                                foreach (BCTinhHinhTD_XLD_GQInfo itemCoQuan in lstCQCapPhong)
                                {
                                    lstCQCapHuyen.Add(itemCoQuan);
                                }

                                item.LsByCapCoQuan = new List<BCTinhHinhTD_XLD_CapCoQuan_GQInfo>();
                                foreach (HuyenInfo huyenInfo in huyenList)
                                {
                                    BCTinhHinhTD_XLD_CapCoQuan_GQInfo lstXLDCapCoQuan = new BCTinhHinhTD_XLD_CapCoQuan_GQInfo();
                                    lstXLDCapCoQuan.TenCoQuan = huyenInfo.TenHuyen;
                                    lstXLDCapCoQuan.CoQuanID = huyenInfo.HuyenID;
                                    lstXLDCapCoQuan.LsByCoQuan = lstCQCapHuyen.Where(x => x.HuyenID == huyenInfo.HuyenID).ToList();

                                    foreach (BCTinhHinhTD_XLD_GQInfo itemCoQuan in lstXLDCapCoQuan.LsByCoQuan)
                                    {
                                        foreach (BCTongHopXuLyInfo raw in xldList)
                                        {
                                            if (itemCoQuan.CoQuanID == raw.CoQuanID)
                                            {
                                                itemCoQuan.SoDonTiepDan = raw.SLTiepCongDan;
                                                itemCoQuan.SLKhieuNai = raw.SLKhieuNai;
                                                itemCoQuan.SLToCao = raw.SLToCao;
                                                itemCoQuan.SLPhanAnhKienNghi = raw.SLPhanAnhKienNghi;
                                                itemCoQuan.SoXLTrongHan = raw.XLDDaXuLyTrongHan;
                                                itemCoQuan.SoXLQuaHan = raw.XLDDaXuLyQuaHan;
                                                itemCoQuan.SoXLDaXuLy = raw.XLDDaXuLy;
                                                itemCoQuan.SoXLChuaXuLy = raw.XLDChuaXuLy;
                                                itemCoQuan.TongSoXL = raw.XLDTongSo;
                                                itemCoQuan.VuViecDongNguoi = raw.XLDKhieuKienDN;
                                                itemCoQuan.XLDKhieuNai = raw.XLDKhieuNai;
                                                itemCoQuan.XLDToCao = raw.XLDToCao;
                                                itemCoQuan.XLDPhanAnhKienNghi = raw.XLDPhanAnhKienNghi;
                                            }
                                        }
                                        foreach (BCTongHopXuLyInfo raw in gqdList)
                                        {
                                            var cqphItem = "," + itemCoQuan.CoQuanID + ",";
                                            if (raw.CoQuanGiaiQuyetID == 0)
                                                raw.CoQuanGiaiQuyetID = raw.CoQuanID;
                                            if (itemCoQuan.CoQuanID == raw.CoQuanGiaiQuyetID)
                                            {

                                                itemCoQuan.TongSoDonGiaoGQ += raw.GQDTongSo;
                                                itemCoQuan.SoDangGQTrongHan += raw.GQDDangGQTrongHan;
                                                itemCoQuan.SoDangGQQuaHan += raw.GQDDangGQQuaHan;
                                                itemCoQuan.TongSoDangGQ += raw.GQDDangGQ;
                                                itemCoQuan.DaCoBC += raw.GQDDaGQ;
                                                itemCoQuan.ChuaGiaiQuyet += raw.GQDChuaGQ;
                                                itemCoQuan.DaBHGQDung += raw.KQKNDung;
                                                itemCoQuan.DaBHGQDungMotPhan += raw.KQKNDungMotPhan;
                                                itemCoQuan.DaBHGQSai += raw.KQKNSai;
                                                itemCoQuan.DaBHGQ = (itemCoQuan.DaBHGQDung + itemCoQuan.DaBHGQDungMotPhan + itemCoQuan.DaBHGQSai);


                                                itemCoQuan.GQDKNDangGQ += raw.GQDKNDangGQ;
                                                itemCoQuan.GQDTCDangGQ += raw.GQDTCDangGQ;
                                                itemCoQuan.GQDKNPADangGQ += raw.GQDKNPADangGQ;
                                                itemCoQuan.GQDKNDaGQ += raw.GQDKNDaGQ;
                                                itemCoQuan.GQDTCDaGQ += raw.GQDTCDaGQ;
                                                itemCoQuan.GQDKNPADaGQ += raw.GQDKNPADaGQ;
                                                itemCoQuan.GQDKNChuaGQ += raw.GQDKNChuaGQ;
                                                itemCoQuan.GQDTCChuaGQ += raw.GQDTCChuaGQ;
                                                itemCoQuan.GQDKNPAChuaGQ += raw.GQDKNPAChuaGQ;
                                            }
                                        }

                                    }
                                    item.LsByCapCoQuan.Add(lstXLDCapCoQuan);
                                }
                            }
                            else if (item.CapID == (int)CapQuanLy.CapUBNDXa)
                            {
                                List<HuyenInfo> huyenList = new Huyen().GetByTinh(TinhID).ToList();
                                List<BCTinhHinhTD_XLD_GQInfo> lstCQCapPhong = new CoQuan().GetCoQuanByCapForBC((int)CapQuanLy.CapUBNDXa, tinhID).ToList();
                                item.LsByCapCoQuan = new List<BCTinhHinhTD_XLD_CapCoQuan_GQInfo>();
                                foreach (HuyenInfo huyenInfo in huyenList)
                                {
                                    BCTinhHinhTD_XLD_CapCoQuan_GQInfo lstXLDCapCoQuan = new BCTinhHinhTD_XLD_CapCoQuan_GQInfo();
                                    lstXLDCapCoQuan.TenCoQuan = huyenInfo.TenHuyen;
                                    lstXLDCapCoQuan.CoQuanID = huyenInfo.HuyenID;
                                    lstXLDCapCoQuan.LsByCoQuan = lstCQCapPhong.Where(x => x.HuyenID == huyenInfo.HuyenID).ToList();

                                    foreach (BCTinhHinhTD_XLD_GQInfo itemCoQuan in lstXLDCapCoQuan.LsByCoQuan)
                                    {
                                        foreach (BCTongHopXuLyInfo raw in xldList)
                                        {
                                            if (itemCoQuan.CoQuanID == raw.CoQuanID)
                                            {
                                                itemCoQuan.SoDonTiepDan = raw.SLTiepCongDan;
                                                itemCoQuan.SLKhieuNai = raw.SLKhieuNai;
                                                itemCoQuan.SLToCao = raw.SLToCao;
                                                itemCoQuan.SLPhanAnhKienNghi = raw.SLPhanAnhKienNghi;
                                                itemCoQuan.SoXLTrongHan = raw.XLDDaXuLyTrongHan;
                                                itemCoQuan.SoXLQuaHan = raw.XLDDaXuLyQuaHan;
                                                itemCoQuan.SoXLDaXuLy = raw.XLDDaXuLy;
                                                itemCoQuan.SoXLChuaXuLy = raw.XLDChuaXuLy;
                                                itemCoQuan.TongSoXL = raw.XLDTongSo;
                                                itemCoQuan.VuViecDongNguoi = raw.XLDKhieuKienDN;
                                                itemCoQuan.XLDKhieuNai = raw.XLDKhieuNai;
                                                itemCoQuan.XLDToCao = raw.XLDToCao;
                                                itemCoQuan.XLDPhanAnhKienNghi = raw.XLDPhanAnhKienNghi;

                                            }
                                        }
                                        foreach (BCTongHopXuLyInfo raw in gqdList)
                                        {
                                            var cqphItem = "," + itemCoQuan.CoQuanID + ",";
                                            if (raw.CoQuanGiaiQuyetID == 0)
                                                raw.CoQuanGiaiQuyetID = raw.CoQuanID;
                                            if (itemCoQuan.CoQuanID == raw.CoQuanGiaiQuyetID)
                                            {

                                                itemCoQuan.TongSoDonGiaoGQ += raw.GQDTongSo;
                                                itemCoQuan.SoDangGQTrongHan += raw.GQDDangGQTrongHan;
                                                itemCoQuan.SoDangGQQuaHan += raw.GQDDangGQQuaHan;
                                                itemCoQuan.TongSoDangGQ += raw.GQDDangGQ;
                                                itemCoQuan.DaCoBC += raw.GQDDaGQ;
                                                itemCoQuan.ChuaGiaiQuyet += raw.GQDChuaGQ;
                                                itemCoQuan.DaBHGQDung += raw.KQKNDung;
                                                itemCoQuan.DaBHGQDungMotPhan += raw.KQKNDungMotPhan;
                                                itemCoQuan.DaBHGQSai += raw.KQKNSai;
                                                itemCoQuan.DaBHGQ = (itemCoQuan.DaBHGQDung + itemCoQuan.DaBHGQDungMotPhan + itemCoQuan.DaBHGQSai);


                                                itemCoQuan.GQDKNDangGQ += raw.GQDKNDangGQ;
                                                itemCoQuan.GQDTCDangGQ += raw.GQDTCDangGQ;
                                                itemCoQuan.GQDKNPADangGQ += raw.GQDKNPADangGQ;
                                                itemCoQuan.GQDKNDaGQ += raw.GQDKNDaGQ;
                                                itemCoQuan.GQDTCDaGQ += raw.GQDTCDaGQ;
                                                itemCoQuan.GQDKNPADaGQ += raw.GQDKNPADaGQ;
                                                itemCoQuan.GQDKNChuaGQ += raw.GQDKNChuaGQ;
                                                itemCoQuan.GQDTCChuaGQ += raw.GQDTCChuaGQ;
                                                itemCoQuan.GQDKNPAChuaGQ += raw.GQDKNPAChuaGQ;
                                            }
                                        }

                                    }
                                    item.LsByCapCoQuan.Add(lstXLDCapCoQuan);
                                }
                            }


                        }
                    }
                    else/*cấp huyện chọn xem thong tin trong huyện*/
                    {
                        if (pRoleUser == (int)EnumChucVu.LanhDao && CapID == (int)CapQuanLy.CapUBNDTinh) /*lãnh đạo tỉnh chọn xem thong tin trong 1 huyện*/
                        {
                            lsData = tempList.Where(x => x.CapID == (int)CapQuanLy.CapUBNDHuyen || x.CapID == (int)CapQuanLy.CapUBNDXa).ToList();
                        }
                        else if (pRoleUser == (int)EnumChucVu.LanhDao) /*lãnh đạo chọn xem thong tin trong toàn huyện*/
                        {
                            lsData = tempList.Where(x => x.CapID == (int)CapQuanLy.CapUBNDHuyen || x.CapID == (int)CapQuanLy.CapUBNDXa).ToList();
                        }
                        else
                        {
                            lsData = tempList.Where(x => x.CapID == (int)CapQuanLy.CapUBNDHuyen).ToList();
                        }


                        foreach (BCTinhHinhTD_XLD_GQInfo item in lsData)
                        {
                            if (item.CapID == (int)CapQuanLy.CapUBNDHuyen)
                            {
                                List<BCTinhHinhTD_XLD_GQInfo> lstCQCapPhong = new CoQuan().GetCoQuanByCapForBC((int)CapQuanLy.CapPhong, tinhID).ToList();
                                List<BCTinhHinhTD_XLD_GQInfo> lstCQCapUBHuyen = new CoQuan().GetCoQuanByCapForBC((int)CapQuanLy.CapUBNDHuyen, tinhID).ToList();
                                List<BCTinhHinhTD_XLD_GQInfo> lstCQCapHuyen = new List<BCTinhHinhTD_XLD_GQInfo>();
                                foreach (BCTinhHinhTD_XLD_GQInfo itemCoQuan in lstCQCapUBHuyen)
                                {
                                    lstCQCapHuyen.Add(itemCoQuan);
                                }
                                foreach (BCTinhHinhTD_XLD_GQInfo itemCoQuan in lstCQCapPhong)
                                {
                                    lstCQCapHuyen.Add(itemCoQuan);
                                }

                                item.LsByCapCoQuan = new List<BCTinhHinhTD_XLD_CapCoQuan_GQInfo>();

                                BCTinhHinhTD_XLD_CapCoQuan_GQInfo lstXLDCapCoQuan = new BCTinhHinhTD_XLD_CapCoQuan_GQInfo();
                                if (pRoleUser == (int)EnumChucVu.LanhDao)
                                {
                                    HuyenInfo huyenInfo = new Huyen().GetByID(coQuanUserID);
                                    lstXLDCapCoQuan.TenCoQuan = huyenInfo.TenHuyen;
                                    lstXLDCapCoQuan.CoQuanID = huyenInfo.HuyenID;
                                    lstXLDCapCoQuan.LsByCoQuan = lstCQCapHuyen.Where(x => x.HuyenID == huyenInfo.HuyenID).ToList();
                                }
                                else
                                {
                                    HuyenInfo huyenInfo = new Huyen().GetByID(HuyenID);
                                    lstXLDCapCoQuan.TenCoQuan = huyenInfo.TenHuyen;
                                    lstXLDCapCoQuan.CoQuanID = huyenInfo.HuyenID;
                                    lstXLDCapCoQuan.LsByCoQuan = lstCQCapHuyen.Where(x => x.CoQuanID == coQuanUserID).ToList();
                                }
                                foreach (BCTinhHinhTD_XLD_GQInfo itemCoQuan in lstXLDCapCoQuan.LsByCoQuan)
                                {
                                    foreach (BCTongHopXuLyInfo raw in xldList)
                                    {
                                        if (itemCoQuan.CoQuanID == raw.CoQuanID)
                                        {
                                            itemCoQuan.SoDonTiepDan = raw.SLTiepCongDan;
                                            itemCoQuan.SLKhieuNai = raw.SLKhieuNai;
                                            itemCoQuan.SLToCao = raw.SLToCao;
                                            itemCoQuan.SLPhanAnhKienNghi = raw.SLPhanAnhKienNghi;
                                            itemCoQuan.SoXLTrongHan = raw.XLDDaXuLyTrongHan;
                                            itemCoQuan.SoXLQuaHan = raw.XLDDaXuLyQuaHan;
                                            itemCoQuan.SoXLDaXuLy = raw.XLDDaXuLy;
                                            itemCoQuan.SoXLChuaXuLy = raw.XLDChuaXuLy;
                                            itemCoQuan.TongSoXL = raw.XLDTongSo;
                                            itemCoQuan.VuViecDongNguoi = raw.XLDKhieuKienDN;
                                            itemCoQuan.XLDKhieuNai = raw.XLDKhieuNai;
                                            itemCoQuan.XLDToCao = raw.XLDToCao;
                                            itemCoQuan.XLDPhanAnhKienNghi = raw.XLDPhanAnhKienNghi;

                                        }
                                    }
                                    foreach (BCTongHopXuLyInfo raw in gqdList)
                                    {
                                        var cqphItem = "," + itemCoQuan.CoQuanID + ",";
                                        if (raw.CoQuanGiaiQuyetID == 0)
                                            raw.CoQuanGiaiQuyetID = raw.CoQuanID;
                                        if (itemCoQuan.CoQuanID == raw.CoQuanGiaiQuyetID)
                                        {

                                            itemCoQuan.TongSoDonGiaoGQ += raw.GQDTongSo;
                                            itemCoQuan.SoDangGQTrongHan += raw.GQDDangGQTrongHan;
                                            itemCoQuan.SoDangGQQuaHan += raw.GQDDangGQQuaHan;
                                            itemCoQuan.TongSoDangGQ += raw.GQDDangGQ;
                                            itemCoQuan.DaCoBC += raw.GQDDaGQ;
                                            itemCoQuan.ChuaGiaiQuyet += raw.GQDChuaGQ;
                                            itemCoQuan.DaBHGQDung += raw.KQKNDung;
                                            itemCoQuan.DaBHGQDungMotPhan += raw.KQKNDungMotPhan;
                                            itemCoQuan.DaBHGQSai += raw.KQKNSai;
                                            itemCoQuan.DaBHGQ = (itemCoQuan.DaBHGQDung + itemCoQuan.DaBHGQDungMotPhan + itemCoQuan.DaBHGQSai);


                                            itemCoQuan.GQDKNDangGQ += raw.GQDKNDangGQ;
                                            itemCoQuan.GQDTCDangGQ += raw.GQDTCDangGQ;
                                            itemCoQuan.GQDKNPADangGQ += raw.GQDKNPADangGQ;
                                            itemCoQuan.GQDKNDaGQ += raw.GQDKNDaGQ;
                                            itemCoQuan.GQDTCDaGQ += raw.GQDTCDaGQ;
                                            itemCoQuan.GQDKNPADaGQ += raw.GQDKNPADaGQ;
                                            itemCoQuan.GQDKNChuaGQ += raw.GQDKNChuaGQ;
                                            itemCoQuan.GQDTCChuaGQ += raw.GQDTCChuaGQ;
                                            itemCoQuan.GQDKNPAChuaGQ += raw.GQDKNPAChuaGQ;
                                        }
                                    }
                                }
                                item.LsByCapCoQuan.Add(lstXLDCapCoQuan);
                            }
                            else if (item.CapID == (int)CapQuanLy.CapUBNDXa)
                            {
                                List<BCTinhHinhTD_XLD_GQInfo> lstCQCapPhong = new CoQuan().GetCoQuanByCapForBC((int)CapQuanLy.CapUBNDXa, tinhID).ToList();

                                BCTinhHinhTD_XLD_CapCoQuan_GQInfo lstXLDCapCoQuan = new BCTinhHinhTD_XLD_CapCoQuan_GQInfo();
                                HuyenInfo huyenInfo = new HuyenInfo();
                                if (pRoleUser == (int)EnumChucVu.LanhDao)
                                {
                                    huyenInfo = new Huyen().GetByID(coQuanUserID);
                                }
                                else
                                {
                                    huyenInfo = new Huyen().GetByID(HuyenID);
                                }
                                lstXLDCapCoQuan.TenCoQuan = huyenInfo.TenHuyen;
                                lstXLDCapCoQuan.CoQuanID = huyenInfo.HuyenID;
                                lstXLDCapCoQuan.LsByCoQuan = lstCQCapPhong.Where(x => x.HuyenID == huyenInfo.HuyenID).ToList();
                                item.LsByCapCoQuan = new List<BCTinhHinhTD_XLD_CapCoQuan_GQInfo>();
                                foreach (BCTinhHinhTD_XLD_GQInfo itemCoQuan in lstXLDCapCoQuan.LsByCoQuan)
                                {
                                    foreach (BCTongHopXuLyInfo raw in xldList)
                                    {
                                        if (itemCoQuan.CoQuanID == raw.CoQuanID)
                                        {
                                            itemCoQuan.SoDonTiepDan = raw.SLTiepCongDan;
                                            itemCoQuan.SLKhieuNai = raw.SLKhieuNai;
                                            itemCoQuan.SLToCao = raw.SLToCao;
                                            itemCoQuan.SLPhanAnhKienNghi = raw.SLPhanAnhKienNghi;
                                            itemCoQuan.SoXLTrongHan = raw.XLDDaXuLyTrongHan;
                                            itemCoQuan.SoXLQuaHan = raw.XLDDaXuLyQuaHan;
                                            itemCoQuan.SoXLDaXuLy = raw.XLDDaXuLy;
                                            itemCoQuan.SoXLChuaXuLy = raw.XLDChuaXuLy;
                                            itemCoQuan.TongSoXL = raw.XLDTongSo;
                                            itemCoQuan.VuViecDongNguoi = raw.XLDKhieuKienDN;
                                            itemCoQuan.XLDKhieuNai = raw.XLDKhieuNai;
                                            itemCoQuan.XLDToCao = raw.XLDToCao;
                                            itemCoQuan.XLDPhanAnhKienNghi = raw.XLDPhanAnhKienNghi;

                                        }
                                    }
                                    foreach (BCTongHopXuLyInfo raw in gqdList)
                                    {
                                        var cqphItem = "," + itemCoQuan.CoQuanID + ",";
                                        if (raw.CoQuanGiaiQuyetID == 0)
                                            raw.CoQuanGiaiQuyetID = raw.CoQuanID;
                                        if (itemCoQuan.CoQuanID == raw.CoQuanGiaiQuyetID)
                                        {

                                            itemCoQuan.TongSoDonGiaoGQ += raw.GQDTongSo;
                                            itemCoQuan.SoDangGQTrongHan += raw.GQDDangGQTrongHan;
                                            itemCoQuan.SoDangGQQuaHan += raw.GQDDangGQQuaHan;
                                            itemCoQuan.TongSoDangGQ += raw.GQDDangGQ;
                                            itemCoQuan.DaCoBC += raw.GQDDaGQ;
                                            itemCoQuan.ChuaGiaiQuyet += raw.GQDChuaGQ;
                                            itemCoQuan.DaBHGQDung += raw.KQKNDung;
                                            itemCoQuan.DaBHGQDungMotPhan += raw.KQKNDungMotPhan;
                                            itemCoQuan.DaBHGQSai += raw.KQKNSai;
                                            itemCoQuan.DaBHGQ = (itemCoQuan.DaBHGQDung + itemCoQuan.DaBHGQDungMotPhan + itemCoQuan.DaBHGQSai);


                                            itemCoQuan.GQDKNDangGQ += raw.GQDKNDangGQ;
                                            itemCoQuan.GQDTCDangGQ += raw.GQDTCDangGQ;
                                            itemCoQuan.GQDKNPADangGQ += raw.GQDKNPADangGQ;
                                            itemCoQuan.GQDKNDaGQ += raw.GQDKNDaGQ;
                                            itemCoQuan.GQDTCDaGQ += raw.GQDTCDaGQ;
                                            itemCoQuan.GQDKNPADaGQ += raw.GQDKNPADaGQ;
                                            itemCoQuan.GQDKNChuaGQ += raw.GQDKNChuaGQ;
                                            itemCoQuan.GQDTCChuaGQ += raw.GQDTCChuaGQ;
                                            itemCoQuan.GQDKNPAChuaGQ += raw.GQDKNPAChuaGQ;
                                        }
                                    }

                                }
                                item.LsByCapCoQuan.Add(lstXLDCapCoQuan);

                            }
                        }
                    }


                }
                else if (capUser == 6) /*LĐ tỉnh chọn xem cấp xa*/
                {
                    hidenCap = false;
                    List<BCTinhHinhTD_XLD_GQInfo> tempList = new CapDAL().GetAllCap().ToList();
                    if (CapID == (int)CapQuanLy.CapUBNDHuyen)
                    {
                        lsData = tempList.Where(x => x.CapID == (int)CapQuanLy.CapUBNDXa).ToList();
                        foreach (BCTinhHinhTD_XLD_GQInfo item in lsData)
                        {

                            HuyenInfo huyenInfo = new Huyen().GetByID(coQuanUserID);
                            List<BCTinhHinhTD_XLD_GQInfo> lstCQCapPhong = new CoQuan().GetCoQuanByCapForBC((int)CapQuanLy.CapUBNDXa, tinhID).ToList();
                            item.LsByCapCoQuan = new List<BCTinhHinhTD_XLD_CapCoQuan_GQInfo>();

                            BCTinhHinhTD_XLD_CapCoQuan_GQInfo lstXLDCapCoQuan = new BCTinhHinhTD_XLD_CapCoQuan_GQInfo();
                            lstXLDCapCoQuan.TenCoQuan = huyenInfo.TenHuyen;
                            lstXLDCapCoQuan.CoQuanID = huyenInfo.HuyenID;
                            lstXLDCapCoQuan.LsByCoQuan = lstCQCapPhong.Where(x => x.HuyenID == huyenInfo.HuyenID).ToList();

                            foreach (BCTinhHinhTD_XLD_GQInfo itemCoQuan in lstXLDCapCoQuan.LsByCoQuan)
                            {
                                foreach (BCTongHopXuLyInfo raw in xldList)
                                {
                                    if (itemCoQuan.CoQuanID == raw.CoQuanID)
                                    {
                                        itemCoQuan.SoDonTiepDan = raw.SLTiepCongDan;
                                        itemCoQuan.SLKhieuNai = raw.SLKhieuNai;
                                        itemCoQuan.SLToCao = raw.SLToCao;
                                        itemCoQuan.SLPhanAnhKienNghi = raw.SLPhanAnhKienNghi;
                                        itemCoQuan.SoXLTrongHan = raw.XLDDaXuLyTrongHan;
                                        itemCoQuan.SoXLQuaHan = raw.XLDDaXuLyQuaHan;
                                        itemCoQuan.SoXLDaXuLy = raw.XLDDaXuLy;
                                        itemCoQuan.SoXLChuaXuLy = raw.XLDChuaXuLy;
                                        itemCoQuan.TongSoXL = raw.XLDTongSo;
                                        itemCoQuan.VuViecDongNguoi = raw.XLDKhieuKienDN;
                                        itemCoQuan.XLDKhieuNai = raw.XLDKhieuNai;
                                        itemCoQuan.XLDToCao = raw.XLDToCao;
                                        itemCoQuan.XLDPhanAnhKienNghi = raw.XLDPhanAnhKienNghi;

                                    }
                                }
                                foreach (BCTongHopXuLyInfo raw in gqdList)
                                {
                                    var cqphItem = "," + itemCoQuan.CoQuanID + ",";
                                    if (raw.CoQuanGiaiQuyetID == 0)
                                        raw.CoQuanGiaiQuyetID = raw.CoQuanID;
                                    if (itemCoQuan.CoQuanID == raw.CoQuanGiaiQuyetID)
                                    {

                                        itemCoQuan.TongSoDonGiaoGQ += raw.GQDTongSo;
                                        itemCoQuan.SoDangGQTrongHan += raw.GQDDangGQTrongHan;
                                        itemCoQuan.SoDangGQQuaHan += raw.GQDDangGQQuaHan;
                                        itemCoQuan.TongSoDangGQ += raw.GQDDangGQ;
                                        itemCoQuan.DaCoBC += raw.GQDDaGQ;
                                        itemCoQuan.ChuaGiaiQuyet += raw.GQDChuaGQ;
                                        itemCoQuan.DaBHGQDung += raw.KQKNDung;
                                        itemCoQuan.DaBHGQDungMotPhan += raw.KQKNDungMotPhan;
                                        itemCoQuan.DaBHGQSai += raw.KQKNSai;
                                        itemCoQuan.DaBHGQ = (itemCoQuan.DaBHGQDung + itemCoQuan.DaBHGQDungMotPhan + itemCoQuan.DaBHGQSai);


                                        itemCoQuan.GQDKNDangGQ += raw.GQDKNDangGQ;
                                        itemCoQuan.GQDTCDangGQ += raw.GQDTCDangGQ;
                                        itemCoQuan.GQDKNPADangGQ += raw.GQDKNPADangGQ;
                                        itemCoQuan.GQDKNDaGQ += raw.GQDKNDaGQ;
                                        itemCoQuan.GQDTCDaGQ += raw.GQDTCDaGQ;
                                        itemCoQuan.GQDKNPADaGQ += raw.GQDKNPADaGQ;
                                        itemCoQuan.GQDKNChuaGQ += raw.GQDKNChuaGQ;
                                        itemCoQuan.GQDTCChuaGQ += raw.GQDTCChuaGQ;
                                        itemCoQuan.GQDKNPAChuaGQ += raw.GQDKNPAChuaGQ;
                                    }
                                }

                            }
                            item.LsByCapCoQuan.Add(lstXLDCapCoQuan);
                        }
                    }
                    else if (CapID == (int)CapQuanLy.CapUBNDTinh || CoQuanID == (int)EnumCoQuan.ThanhTraTinh)
                    {
                        lsData = tempList.Where(x => x.CapID == (int)CapQuanLy.CapUBNDXa).ToList();
                        foreach (BCTinhHinhTD_XLD_GQInfo item in lsData)
                        {
                            if (coQuanUserID == 0) /*Lãnh đạo tỉnh xem cấp xã tất cả các huyện*/
                            {
                                List<HuyenInfo> huyenList = new Huyen().GetByTinh(TinhID).ToList();
                                List<BCTinhHinhTD_XLD_GQInfo> lstCQCapPhong = new CoQuan().GetCoQuanByCapForBC((int)CapQuanLy.CapUBNDXa, tinhID).ToList();
                                item.LsByCapCoQuan = new List<BCTinhHinhTD_XLD_CapCoQuan_GQInfo>();
                                foreach (HuyenInfo huyenInfo in huyenList)
                                {
                                    BCTinhHinhTD_XLD_CapCoQuan_GQInfo lstXLDCapCoQuan = new BCTinhHinhTD_XLD_CapCoQuan_GQInfo();
                                    lstXLDCapCoQuan.TenCoQuan = huyenInfo.TenHuyen;
                                    lstXLDCapCoQuan.CoQuanID = huyenInfo.HuyenID;
                                    lstXLDCapCoQuan.LsByCoQuan = lstCQCapPhong.Where(x => x.HuyenID == huyenInfo.HuyenID).ToList();

                                    foreach (BCTinhHinhTD_XLD_GQInfo itemCoQuan in lstXLDCapCoQuan.LsByCoQuan)
                                    {
                                        foreach (BCTongHopXuLyInfo raw in xldList)
                                        {
                                            if (itemCoQuan.CoQuanID == raw.CoQuanID)
                                            {
                                                itemCoQuan.SoDonTiepDan = raw.SLTiepCongDan;
                                                itemCoQuan.SLKhieuNai = raw.SLKhieuNai;
                                                itemCoQuan.SLToCao = raw.SLToCao;
                                                itemCoQuan.SLPhanAnhKienNghi = raw.SLPhanAnhKienNghi;
                                                itemCoQuan.SoXLTrongHan = raw.XLDDaXuLyTrongHan;
                                                itemCoQuan.SoXLQuaHan = raw.XLDDaXuLyQuaHan;
                                                itemCoQuan.SoXLDaXuLy = raw.XLDDaXuLy;
                                                itemCoQuan.SoXLChuaXuLy = raw.XLDChuaXuLy;
                                                itemCoQuan.TongSoXL = raw.XLDTongSo;
                                                itemCoQuan.VuViecDongNguoi = raw.XLDKhieuKienDN;
                                                itemCoQuan.XLDKhieuNai = raw.XLDKhieuNai;
                                                itemCoQuan.XLDToCao = raw.XLDToCao;
                                                itemCoQuan.XLDPhanAnhKienNghi = raw.XLDPhanAnhKienNghi;

                                            }
                                        }
                                        foreach (BCTongHopXuLyInfo raw in gqdList)
                                        {
                                            var cqphItem = "," + itemCoQuan.CoQuanID + ",";
                                            if (raw.CoQuanGiaiQuyetID == 0)
                                                raw.CoQuanGiaiQuyetID = raw.CoQuanID;
                                            if (itemCoQuan.CoQuanID == raw.CoQuanGiaiQuyetID)
                                            {

                                                itemCoQuan.TongSoDonGiaoGQ += raw.GQDTongSo;
                                                itemCoQuan.SoDangGQTrongHan += raw.GQDDangGQTrongHan;
                                                itemCoQuan.SoDangGQQuaHan += raw.GQDDangGQQuaHan;
                                                itemCoQuan.TongSoDangGQ += raw.GQDDangGQ;
                                                itemCoQuan.DaCoBC += raw.GQDDaGQ;
                                                itemCoQuan.ChuaGiaiQuyet += raw.GQDChuaGQ;
                                                itemCoQuan.DaBHGQDung += raw.KQKNDung;
                                                itemCoQuan.DaBHGQDungMotPhan += raw.KQKNDungMotPhan;
                                                itemCoQuan.DaBHGQSai += raw.KQKNSai;
                                                itemCoQuan.DaBHGQ = (itemCoQuan.DaBHGQDung + itemCoQuan.DaBHGQDungMotPhan + itemCoQuan.DaBHGQSai);


                                                itemCoQuan.GQDKNDangGQ += raw.GQDKNDangGQ;
                                                itemCoQuan.GQDTCDangGQ += raw.GQDTCDangGQ;
                                                itemCoQuan.GQDKNPADangGQ += raw.GQDKNPADangGQ;
                                                itemCoQuan.GQDKNDaGQ += raw.GQDKNDaGQ;
                                                itemCoQuan.GQDTCDaGQ += raw.GQDTCDaGQ;
                                                itemCoQuan.GQDKNPADaGQ += raw.GQDKNPADaGQ;
                                                itemCoQuan.GQDKNChuaGQ += raw.GQDKNChuaGQ;
                                                itemCoQuan.GQDTCChuaGQ += raw.GQDTCChuaGQ;
                                                itemCoQuan.GQDKNPAChuaGQ += raw.GQDKNPAChuaGQ;
                                            }
                                        }

                                    }
                                    item.LsByCapCoQuan.Add(lstXLDCapCoQuan);
                                }
                            }
                            else
                            {
                                BCTinhHinhTD_XLD_CapCoQuan_GQInfo lstXLDCapCoQuan = new BCTinhHinhTD_XLD_CapCoQuan_GQInfo();
                                List<BCTinhHinhTD_XLD_GQInfo> lstCQCapPhong = new CoQuan().GetCoQuanByCapForBC((int)CapQuanLy.CapUBNDXa, tinhID).ToList();
                                HuyenInfo huyenInfo = new Huyen().GetByID(coQuanUserID);
                                lstXLDCapCoQuan.TenCoQuan = huyenInfo.TenHuyen;
                                lstXLDCapCoQuan.CoQuanID = huyenInfo.HuyenID;
                                lstXLDCapCoQuan.LsByCoQuan = lstCQCapPhong.Where(x => x.HuyenID == huyenInfo.HuyenID).ToList();
                                item.LsByCapCoQuan = new List<BCTinhHinhTD_XLD_CapCoQuan_GQInfo>();
                                foreach (BCTinhHinhTD_XLD_GQInfo itemCoQuan in lstXLDCapCoQuan.LsByCoQuan)
                                {
                                    foreach (BCTongHopXuLyInfo raw in xldList)
                                    {
                                        if (itemCoQuan.CoQuanID == raw.CoQuanID)
                                        {
                                            itemCoQuan.SoDonTiepDan = raw.SLTiepCongDan;
                                            itemCoQuan.SLKhieuNai = raw.SLKhieuNai;
                                            itemCoQuan.SLToCao = raw.SLToCao;
                                            itemCoQuan.SLPhanAnhKienNghi = raw.SLPhanAnhKienNghi;
                                            itemCoQuan.SoXLTrongHan = raw.XLDDaXuLyTrongHan;
                                            itemCoQuan.SoXLQuaHan = raw.XLDDaXuLyQuaHan;
                                            itemCoQuan.SoXLDaXuLy = raw.XLDDaXuLy;
                                            itemCoQuan.SoXLChuaXuLy = raw.XLDChuaXuLy;
                                            itemCoQuan.TongSoXL = raw.XLDTongSo;
                                            itemCoQuan.VuViecDongNguoi = raw.XLDKhieuKienDN;
                                            itemCoQuan.XLDKhieuNai = raw.XLDKhieuNai;
                                            itemCoQuan.XLDToCao = raw.XLDToCao;
                                            itemCoQuan.XLDPhanAnhKienNghi = raw.XLDPhanAnhKienNghi;

                                        }
                                    }
                                    foreach (BCTongHopXuLyInfo raw in gqdList)
                                    {
                                        var cqphItem = "," + itemCoQuan.CoQuanID + ",";
                                        if (raw.CoQuanGiaiQuyetID == 0)
                                            raw.CoQuanGiaiQuyetID = raw.CoQuanID;
                                        if (itemCoQuan.CoQuanID == raw.CoQuanGiaiQuyetID)
                                        {

                                            itemCoQuan.TongSoDonGiaoGQ += raw.GQDTongSo;
                                            itemCoQuan.SoDangGQTrongHan += raw.GQDDangGQTrongHan;
                                            itemCoQuan.SoDangGQQuaHan += raw.GQDDangGQQuaHan;
                                            itemCoQuan.TongSoDangGQ += raw.GQDDangGQ;
                                            itemCoQuan.DaCoBC += raw.GQDDaGQ;
                                            itemCoQuan.ChuaGiaiQuyet += raw.GQDChuaGQ;
                                            itemCoQuan.DaBHGQDung += raw.KQKNDung;
                                            itemCoQuan.DaBHGQDungMotPhan += raw.KQKNDungMotPhan;
                                            itemCoQuan.DaBHGQSai += raw.KQKNSai;
                                            itemCoQuan.DaBHGQ = (itemCoQuan.DaBHGQDung + itemCoQuan.DaBHGQDungMotPhan + itemCoQuan.DaBHGQSai);


                                            itemCoQuan.GQDKNDangGQ += raw.GQDKNDangGQ;
                                            itemCoQuan.GQDTCDangGQ += raw.GQDTCDangGQ;
                                            itemCoQuan.GQDKNPADangGQ += raw.GQDKNPADangGQ;
                                            itemCoQuan.GQDKNDaGQ += raw.GQDKNDaGQ;
                                            itemCoQuan.GQDTCDaGQ += raw.GQDTCDaGQ;
                                            itemCoQuan.GQDKNPADaGQ += raw.GQDKNPADaGQ;
                                            itemCoQuan.GQDKNChuaGQ += raw.GQDKNChuaGQ;
                                            itemCoQuan.GQDTCChuaGQ += raw.GQDTCChuaGQ;
                                            itemCoQuan.GQDKNPAChuaGQ += raw.GQDKNPAChuaGQ;
                                        }
                                    }
                                }
                                item.LsByCapCoQuan.Add(lstXLDCapCoQuan);
                            }

                        }
                    }




                }

                foreach (BCTinhHinhTD_XLD_GQInfo item in lsData)
                {
                    if (item.LsByCoQuan != null)
                    {
                        foreach (BCTinhHinhTD_XLD_GQInfo info in item.LsByCoQuan)
                        {
                            info.TongGQDKN = info.GQDKNDangGQ + info.GQDKNDaGQ + info.GQDKNChuaGQ;
                            info.TongGQDTC = info.GQDTCDangGQ + info.GQDTCDaGQ + info.GQDTCChuaGQ;
                            info.TongGQDKNPA = info.GQDKNPADangGQ + info.GQDKNPADaGQ + info.GQDKNPAChuaGQ;

                            //foreach (KeKhaiDuLieuDauKy_2aInfo keKhaiInfo in duLieuDauKy2aList)
                            //{
                            //    if (keKhaiInfo.CoQuanID == info.CoQuanID)
                            //    {
                            //        info.SoDonTiepDan += keKhaiInfo.TiepThuongXuyen_Luot + keKhaiInfo.LanhDaoTiep_Luot;
                            //    }
                            //}
                            //foreach (KeKhaiDuLieuDauKy_2bInfo keKhaiInfo in duLieuDauKy2bList)
                            //{
                            //    if (keKhaiInfo.CoQuanID == info.CoQuanID)
                            //    {
                            //        info.SoXLDaXuLy += keKhaiInfo.Col1;
                            //    }
                            //}
                            //foreach (KeKhaiDuLieuDauKy_2cInfo keKhaiInfo in duLieuDauKy2cList)
                            //{
                            //    if (keKhaiInfo.CoQuanID == info.CoQuanID)
                            //    {
                            //        info.TongGQDKN += keKhaiInfo.Col1;
                            //        ldInfo.TongDonThuBHGQ_KN += keKhaiInfo.Col1;
                            //    }
                            //}
                            //foreach (KeKhaiDuLieuDauKy_2dInfo keKhaiInfo in duLieuDauKy2dList)
                            //{
                            //    if (keKhaiInfo.CoQuanID == info.CoQuanID)
                            //    {
                            //        info.TongGQDTC += keKhaiInfo.Col1;
                            //        ldInfo.TongDonThuBHGQ_TC += keKhaiInfo.Col1;
                            //    }
                            //}

                            ldInfo.TongTiepDan += info.SoDonTiepDan;
                            ldInfo.TongXuLyDon += info.SoXLDaXuLy;
                            ldInfo.TongTiepDanNhomKN += info.VuViecDongNguoi;
                            ldInfo.TongDonThuBHGQ_KN += (info.GQDKNDangGQ + info.GQDKNDaGQ + info.GQDKNChuaGQ);
                            ldInfo.TongDonThuBHGQ_TC += (info.GQDTCDangGQ + info.GQDTCDaGQ + info.GQDTCChuaGQ);
                            ldInfo.TongDonThuBHGQ_KNPA += (info.GQDKNPADangGQ + info.GQDKNPADaGQ + info.GQDKNPAChuaGQ);

                        }
                    }
                    if (item.LsByCapCoQuan != null)
                    {

                        foreach (BCTinhHinhTD_XLD_CapCoQuan_GQInfo itemCapCoQuan in item.LsByCapCoQuan)
                        {
                            foreach (BCTinhHinhTD_XLD_GQInfo info in itemCapCoQuan.LsByCoQuan)
                            {
                                info.TongGQDKN = info.GQDKNDangGQ + info.GQDKNDaGQ + info.GQDKNChuaGQ;
                                info.TongGQDTC = info.GQDTCDangGQ + info.GQDTCDaGQ + info.GQDTCChuaGQ;
                                info.TongGQDKNPA = info.GQDKNPADangGQ + info.GQDKNPADaGQ + info.GQDKNPAChuaGQ;

                                //foreach (KeKhaiDuLieuDauKy_2aInfo keKhaiInfo in duLieuDauKy2aList)
                                //{
                                //    if (keKhaiInfo.CoQuanID == info.CoQuanID)
                                //    {
                                //        info.SoDonTiepDan += keKhaiInfo.TiepThuongXuyen_Luot + keKhaiInfo.LanhDaoTiep_Luot;
                                //    }
                                //}
                                //foreach (KeKhaiDuLieuDauKy_2bInfo keKhaiInfo in duLieuDauKy2bList)
                                //{
                                //    if (keKhaiInfo.CoQuanID == info.CoQuanID)
                                //    {
                                //        info.SoXLDaXuLy += keKhaiInfo.Col1;
                                //    }
                                //}
                                //foreach (KeKhaiDuLieuDauKy_2cInfo keKhaiInfo in duLieuDauKy2cList)
                                //{
                                //    if (keKhaiInfo.CoQuanID == info.CoQuanID)
                                //    {
                                //        info.TongGQDKN += keKhaiInfo.Col1;
                                //        ldInfo.TongDonThuBHGQ_KN += keKhaiInfo.Col1;
                                //    }
                                //}
                                //foreach (KeKhaiDuLieuDauKy_2dInfo keKhaiInfo in duLieuDauKy2dList)
                                //{
                                //    if (keKhaiInfo.CoQuanID == info.CoQuanID)
                                //    {
                                //        info.TongGQDTC += keKhaiInfo.Col1;
                                //        ldInfo.TongDonThuBHGQ_TC += keKhaiInfo.Col1;
                                //    }
                                //}

                                ldInfo.TongTiepDan += info.SoDonTiepDan;
                                ldInfo.TongXuLyDon += info.SoXLDaXuLy;
                                ldInfo.TongTiepDanNhomKN += info.VuViecDongNguoi;
                                ldInfo.TongDonThuBHGQ_KN += (info.GQDKNDangGQ + info.GQDKNDaGQ + info.GQDKNChuaGQ);
                                ldInfo.TongDonThuBHGQ_TC += (info.GQDTCDangGQ + info.GQDTCDaGQ + info.GQDTCChuaGQ);
                                ldInfo.TongDonThuBHGQ_KNPA += (info.GQDKNPADangGQ + info.GQDKNPADaGQ + info.GQDKNPAChuaGQ);

                            }

                        }
                    }
                }
                //them tong toan tinh, toan huyen
                int flagToanTinh = 0;
                int flagToanHuyen = 0;
                foreach (BCTinhHinhTD_XLD_GQInfo item in lsData)
                {
                    if (item.CapID == (int)CapQuanLy.CapUBNDTinh || item.CapID == (int)CapQuanLy.CapSoNganh || item.CapID == (int)CapQuanLy.CapUBNDHuyen)
                    {
                        flagToanTinh++;
                    }
                    if (item.CapID == (int)CapQuanLy.CapUBNDHuyen || item.CapID == (int)CapQuanLy.CapUBNDXa)
                    {
                        flagToanHuyen++;
                    }
                }

                if (flagToanTinh == 3)
                {
                    BCTinhHinhTD_XLD_GQInfo capInfoToanTinh = new BCTinhHinhTD_XLD_GQInfo();
                    capInfoToanTinh.CapID = (int)CapQuanLy.CapToanTinh;
                    capInfoToanTinh.TenCap = "Toàn Tỉnh";
                    lsData.Insert(0, capInfoToanTinh);
                }
                if (flagToanHuyen == 2)
                {
                    if (flagToanTinh == 3)
                    {
                        BCTinhHinhTD_XLD_GQInfo capInfoToanHuyen = new BCTinhHinhTD_XLD_GQInfo();
                        capInfoToanHuyen.CapID = (int)CapQuanLy.ToanHuyen;
                        capInfoToanHuyen.TenCap = "Toàn Huyện";
                        lsData.Insert(3, capInfoToanHuyen);
                    }
                    else
                    {
                        BCTinhHinhTD_XLD_GQInfo capInfoToanHuyen = new BCTinhHinhTD_XLD_GQInfo();
                        capInfoToanHuyen.CapID = (int)CapQuanLy.ToanHuyen;
                        capInfoToanHuyen.TenCap = "Toàn Huyện";
                        lsData.Insert(0, capInfoToanHuyen);
                    }
                }
            }

            catch (Exception ex) { }
            #endregion
            string data = "";
            //try
            //{

            //    var obj = new
            //    {
            //        lsData = lsData,
            //        ldInfo = ldInfo,
            //        hidenCap = hidenCap

            //    };
            //    JavaScriptSerializer serializer = new JavaScriptSerializer();
            //    data = serializer.Serialize(obj);
            //    return data;
            //}
            //catch
            //{
            //    return data;
            //}
            return data;
        }

        String DASHBOARD_THONGKEXULYDON_GETBYDATE_NEW = "Dashboard_ThongKeXuLyDon_GetByDate_New";
        public IList<BCTongHopXuLyInfo> DashBoard_ThongKeXuLyDon_GetByDate(QueryFilterInfo infoQF)
        {

            IList<BCTongHopXuLyInfo> result = new List<BCTongHopXuLyInfo>();
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARAM_TUNGAYINT, SqlDbType.Int),
                new SqlParameter(PARAM_DENNGAYINT, SqlDbType.Int)
            };
            parms[0].Value = infoQF.TuNgayGoc;
            parms[1].Value = infoQF.DenNgayGoc;
            parms[2].Value = Utils.ConvertToInt32(infoQF.TuNgayGoc.ToString("yyyyMMdd"), 0);
            parms[3].Value = Utils.ConvertToInt32(infoQF.DenNgayGoc.ToString("yyyyMMdd"), 0);

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, DASHBOARD_THONGKEXULYDON_GETBYDATE_NEW, parms))
                {
                    while (dr.Read())
                    {
                        BCTongHopXuLyInfo info = new BCTongHopXuLyInfo();
                        info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        info.CapID = Utils.ConvertToInt32(dr["CapID"], 0);
                        info.SLTiepCongDan = Utils.ConvertToInt32(dr["SLTiepCongDan"], 0);
                        info.SLKhieuNai = Utils.ConvertToInt32(dr["SLKhieuNai"], 0);
                        info.SLToCao = Utils.ConvertToInt32(dr["SLToCao"], 0);
                        info.SLPhanAnhKienNghi = Utils.ConvertToInt32(dr["SLPhanAnhKienNghi"], 0);
                        info.XLDTongSo = Utils.ConvertToInt32(dr["XLDTongSo"], 0);
                        info.XLDDaXuLy = Utils.ConvertToInt32(dr["XLDDaXuLy"], 0);
                        info.XLDChuaXuLy = Utils.ConvertToInt32(dr["XLDChuaXuLy"], 0);
                        info.XLDDaXuLyTrongHan = Utils.ConvertToInt32(dr["XLDDaXuLyTrongHan"], 0);
                        info.XLDDaXuLyQuaHan = Utils.ConvertToInt32(dr["XLDDaXuLyQuaHan"], 0);
                        info.XLDKhieuKienDN = Utils.ConvertToInt32(dr["XLDKhieuKienDN"], 0);
                        info.XLDKhieuNai = Utils.ConvertToInt32(dr["XLDKhieuNai"], 0);
                        info.XLDToCao = Utils.ConvertToInt32(dr["XLDToCao"], 0);
                        info.XLDPhanAnhKienNghi = Utils.ConvertToInt32(dr["XLDPhanAnhKienNghi"], 0);

                        result.Add(info);
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
        String DASHBOARD_THONGKEGIAIQUYETDON_GETBYDATE_NEW = "Dashboard_ThongKeGiaiQuyetDon_GetByDate_New";
        public IList<BCTongHopXuLyInfo> DashBoard_ThongKeGiaiQuyetDon_GetByDate(QueryFilterInfo infoQF)
        {

            IList<BCTongHopXuLyInfo> result = new List<BCTongHopXuLyInfo>();
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_TUNGAYINT, SqlDbType.Int),
                new SqlParameter(PARAM_DENNGAYINT, SqlDbType.Int)
            };
            parms[0].Value = Utils.ConvertToInt32(infoQF.TuNgayGoc.ToString("yyyyMMdd"), 0);
            parms[1].Value = Utils.ConvertToInt32(infoQF.DenNgayGoc.ToString("yyyyMMdd"), 0);
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, DASHBOARD_THONGKEGIAIQUYETDON_GETBYDATE_NEW, parms))
                {
                    while (dr.Read())
                    {
                        BCTongHopXuLyInfo info = new BCTongHopXuLyInfo();
                        info.CoQuanGiaiQuyetID = Utils.ConvertToInt32(dr["CoQuanGiaiQuyetID"], 0);
                        info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        info.GQDTongSo = Utils.ConvertToInt32(dr["GQDTongSo"], 0);
                        info.GQDChuaGQ = Utils.ConvertToInt32(dr["GQDChuaGQ"], 0);
                        info.GQDDangGQ = Utils.ConvertToInt32(dr["GQDDangGQ"], 0);
                        info.GQDDangGQTrongHan = Utils.ConvertToInt32(dr["GQDDangGQTrongHan"], 0);
                        info.GQDDangGQQuaHan = Utils.ConvertToInt32(dr["GQDDangGQQuaHan"], 0);
                        info.GQDDaGQ = Utils.ConvertToInt32(dr["GQDDaGQ"], 0);
                        info.KQKNDung = Utils.ConvertToInt32(dr["KQKNDung"], 0);
                        info.KQKNDungMotPhan = Utils.ConvertToInt32(dr["KQKNDungMotPhan"], 0);
                        info.KQKNSai = Utils.ConvertToInt32(dr["KQKNSai"], 0);

                        info.GQDKNDangGQ = Utils.ConvertToInt32(dr["GQDKNDangGQ"], 0);
                        info.GQDTCDangGQ = Utils.ConvertToInt32(dr["GQDTCDangGQ"], 0);
                        info.GQDKNPADangGQ = Utils.ConvertToInt32(dr["GQDKNPADangGQ"], 0);

                        info.GQDKNDaGQ = Utils.ConvertToInt32(dr["GQDKNDaGQ"], 0);
                        info.GQDTCDaGQ = Utils.ConvertToInt32(dr["GQDTCDaGQ"], 0);
                        info.GQDKNPADaGQ = Utils.ConvertToInt32(dr["GQDKNPADaGQ"], 0);

                        info.GQDKNChuaGQ = Utils.ConvertToInt32(dr["GQDKNChuaGQ"], 0);
                        info.GQDTCChuaGQ = Utils.ConvertToInt32(dr["GQDTCChuaGQ"], 0);
                        info.GQDKNPAChuaGQ = Utils.ConvertToInt32(dr["GQDKNPAChuaGQ"], 0);
                        result.Add(info);
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

        public List<CoQuanInfo> GetCoQuanByPhamViID(string PhamViID, int CapID, int TinhID, int HuyenID)
        {
            int phamviID = Utils.GetInt32(PhamViID, 0);
            List<CoQuanInfo> resultList = new List<CoQuanInfo>();
            List<HuyenInfo> resultListHuyen = new List<HuyenInfo>();
            int capUser = CapID;
            //Du lieu toan tinh
            if (phamviID == 2)
            {

            }
            //Du lieu cac So
            else if (phamviID == 3)
            {

                List<CoQuanInfo> cqList = new CoQuan().GetCoQuanByCap((int)CapQuanLy.CapSoNganh).ToList();

                foreach (CoQuanInfo cqInfo in cqList)
                {

                    resultList.Add(cqInfo);

                }

            }
            else if (phamviID == 4 || phamviID == 5)
            {
                List<HuyenInfo> huyenList = new Huyen().GetByTinh(TinhID).ToList();
                if (capUser == (int)CapQuanLy.CapUBNDHuyen)
                {
                    huyenList = huyenList.Where(x => x.HuyenID == HuyenID).ToList();
                }

                foreach (HuyenInfo cqInfo in huyenList)
                {
                    CoQuanInfo cq = new CoQuanInfo();
                    cq.CoQuanID = cqInfo.HuyenID;
                    cq.TenCoQuan = cqInfo.TenHuyen;
                    resultList.Add(cq);
                }

            }

            return resultList;

        }

        public SoLieuModel DashBoard_ChuTichUBND(CanhBaoParams p)
        {
            SoLieuModel Info = new SoLieuModel();

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter("@LoaiKhieuToID", SqlDbType.Int),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter("@CapID", SqlDbType.Int),
            };
            parms[0].Value = p.CoQuanID;
            parms[1].Value = p.LoaiKhieuToID ?? 0;
            parms[2].Value = p.DenNgay ?? Convert.DBNull;
            //parms[2].Value = Utils.ConvertToNullableDateTime(p.DenNgay, null) ?? Convert.DBNull;
            parms[3].Value = p.CapID ?? Convert.DBNull;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_DashBoard_ChuTichUBND", parms))
                {
                    while (dr.Read())
                    {

                        Info.CanBanHanhGXM = Utils.ConvertToInt32(dr["CanBanHanhGXM"], 0);
                        Info.DaBanHanhGXM = Utils.ConvertToInt32(dr["DaBanHanhGXM"], 0);
                        Info.CanBanHanhGQ = Utils.ConvertToInt32(dr["CanBanHanhGQ"], 0);
                        Info.DaBanHanhGQ = Utils.ConvertToInt32(dr["DaBanHanhGQ"], 0);
                        Info.QuaHanBanHanh = Utils.ConvertToInt32(dr["QuaHanBanHanh"], 0);
                        Info.DenHanBanHanh = Utils.ConvertToInt32(dr["DenHanBanHanh"], 0);
                        Info.ChuaDenHanBanHanh = Utils.ConvertToInt32(dr["ChuaDenHanBanHanh"], 0);
                    }
                    dr.Close();
                }

            }
            catch
            {

                throw;
            }
            return Info;
        }

        public SoLieuModel DashBoard_CanhBaoPheDuyetKetQuaXuLy(CanhBaoParams p)
        {
            SoLieuModel Info = new SoLieuModel();

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter("@LoaiKhieuToID", SqlDbType.Int),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter("@CapID", SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
            };
            parms[0].Value = p.CoQuanID;
            parms[1].Value = p.LoaiKhieuToID ?? 0;
            parms[2].Value = p.DenNgay ?? Convert.DBNull;
            parms[3].Value = p.CapID ?? Convert.DBNull;
            parms[4].Value = p.TuNgay ?? Convert.DBNull;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_DashBoard_CanhBaoPheDuyetKetQuaXuLy_New", parms))
                {
                    while (dr.Read())
                    {

                        Info.CanPheDuyet = Utils.ConvertToInt32(dr["CanPheDuyet"], 0);
                        Info.DaPheDuyet = Utils.ConvertToInt32(dr["DaPheDuyet"], 0);
                        Info.CanTrinhDuThao = Utils.ConvertToInt32(dr["CanTrinhDuThao"], 0);
                        Info.DaTrinhDuThao = Utils.ConvertToInt32(dr["DaTrinhDuThao"], 0);
                        Info.QuaHan = Utils.ConvertToInt32(dr["QuaHan"], 0);
                        Info.DenHan = Utils.ConvertToInt32(dr["DenHan"], 0);
                        Info.ChuaDenHan = Utils.ConvertToInt32(dr["ChuaDenHan"], 0);
                    }
                    dr.Close();
                }

            }
            catch
            {

                throw;
            }
            return Info;
        }

        public SoLieuModel DashBoard_CanhBaoXuLyDon(CanhBaoParams p)
        {
            SoLieuModel Info = new SoLieuModel();

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter("@LoaiKhieuToID", SqlDbType.Int),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter("@CapID", SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
            };
            parms[0].Value = p.CoQuanID;
            parms[1].Value = p.LoaiKhieuToID ?? 0;
            parms[2].Value = p.DenNgay ?? Convert.DBNull;
            parms[3].Value = p.CapID ?? Convert.DBNull;
            parms[4].Value = p.TuNgay ?? Convert.DBNull;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_DashBoard_CanhBaoXuLyDon_New", parms))
                {
                    while (dr.Read())
                    {

                        Info.CanXuLy = Utils.ConvertToInt32(dr["CanXuLy"], 0);
                        Info.DaXuLy = Utils.ConvertToInt32(dr["DaXuLy"], 0);
                        Info.CanTrinhKetQua = Utils.ConvertToInt32(dr["CanTrinhKetQua"], 0);
                        Info.DaTrinhKetQua = Utils.ConvertToInt32(dr["DaTrinhKetQua"], 0);
                        //Info.CanCapNhat = Utils.ConvertToInt32(dr["CanCapNhat"], 0);
                        //Info.DaCapNhat = Utils.ConvertToInt32(dr["DaCapNhat"], 0);
                        Info.QuaHan = Utils.ConvertToInt32(dr["QuaHan"], 0);
                        Info.DenHan = Utils.ConvertToInt32(dr["DenHan"], 0);
                        Info.ChuaDenHan = Utils.ConvertToInt32(dr["ChuaDenHan"], 0);
                    }
                    dr.Close();
                }

            }
            catch
            {

                throw;
            }
            return Info;
        }

        public SoLieuModel DashBoard_CanhBaoCapNhatGiaoXacMinh(CanhBaoParams p)
        {
            SoLieuModel Info = new SoLieuModel();

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter("@LoaiKhieuToID", SqlDbType.Int),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter("@CapID", SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
            };
            parms[0].Value = p.CoQuanID;
            parms[1].Value = p.LoaiKhieuToID ?? 0;
            parms[2].Value = p.DenNgay ?? Convert.DBNull;
            parms[3].Value = p.CapID ?? Convert.DBNull;
            parms[4].Value = p.TuNgay ?? Convert.DBNull;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_DashBoard_CanhBaoCapNhatGiaoXacMinh_New", parms))
                {
                    while (dr.Read())
                    {
                        Info.CanCapNhat = Utils.ConvertToInt32(dr["CanCapNhat"], 0);
                        Info.DaCapNhat = Utils.ConvertToInt32(dr["DaCapNhat"], 0);
                    }
                    dr.Close();
                }

            }
            catch
            {

                throw;
            }
            return Info;
        }

        public SoLieuModel DashBoard_CanhBaoCapNhatQuyetDinhGiaiQuyet(CanhBaoParams p)
        {
            SoLieuModel Info = new SoLieuModel();

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter("@LoaiKhieuToID", SqlDbType.Int),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter("@CapID", SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
            };
            parms[0].Value = p.CoQuanID;
            parms[1].Value = p.LoaiKhieuToID ?? 0;
            parms[2].Value = p.DenNgay ?? Convert.DBNull;
            parms[3].Value = p.CapID ?? Convert.DBNull;
            parms[4].Value = p.TuNgay ?? Convert.DBNull;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_DashBoard_CanhBaoCapNhatQuyetDinhGiaiQuyet_New", parms))
                {
                    while (dr.Read())
                    {
                        Info.CanCapNhat = Utils.ConvertToInt32(dr["CanCapNhat"], 0);
                        Info.DaCapNhat = Utils.ConvertToInt32(dr["DaCapNhat"], 0);
                    }
                    dr.Close();
                }

            }
            catch
            {

                throw;
            }
            return Info;
        }

        public SoLieuModel DashBoard_CanhBaoGiaiQuyetDon(CanhBaoParams p)
        {
            SoLieuModel Info = new SoLieuModel();

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter("@LoaiKhieuToID", SqlDbType.Int),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter("@CapID", SqlDbType.Int),
                new SqlParameter(PARAM_TUNGAY, SqlDbType.DateTime),
            };
            parms[0].Value = p.CoQuanID;
            parms[1].Value = p.LoaiKhieuToID ?? 0;
            parms[2].Value = p.DenNgay ?? Convert.DBNull;
            parms[3].Value = p.CapID ?? Convert.DBNull;
            parms[4].Value = p.TuNgay ?? Convert.DBNull;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_DashBoard_CanhBaoGiaiQuyetDon_New", parms))
                {
                    while (dr.Read())
                    {
                        Info.CanGiaoXacMinh = Utils.ConvertToInt32(dr["CanGiaoXacMinh"], 0);
                        Info.DaGiaoXacMinh = Utils.ConvertToInt32(dr["DaGiaoXacMinh"], 0);
                        Info.CanDuyetBCXacMinh = Utils.ConvertToInt32(dr["CanDuyetBCXacMinh"], 0);
                        Info.DaDuyetBCXacMinh = Utils.ConvertToInt32(dr["DaDuyetBCXacMinh"], 0);
                        Info.CanBanHanhQDGQ = Utils.ConvertToInt32(dr["CanBanHanhQDGQ"], 0);
                        Info.DaBanHanhQDGQ = Utils.ConvertToInt32(dr["DaBanHanhQDGQ"], 0);
                    }
                    dr.Close();
                }

            }
            catch
            {

                throw;
            }
            return Info;
        }

        public SoLieuModel DashBoard_CanhBaoGiaiQuyetDon_TP(CanhBaoParams p)
        {
            SoLieuModel Info = new SoLieuModel();

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_COQUANID, SqlDbType.Int),
                new SqlParameter("@LoaiKhieuToID", SqlDbType.Int),
                new SqlParameter(PARAM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter("@CapID", SqlDbType.Int),
            };
            parms[0].Value = p.CoQuanID;
            parms[1].Value = p.LoaiKhieuToID ?? 0;
            parms[2].Value = p.DenNgay ?? Convert.DBNull;
            parms[3].Value = p.CapID ?? Convert.DBNull;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_DashBoard_CanhBaoGiaiQuyetDon_TP", parms))
                {
                    while (dr.Read())
                    {
                        Info.CanGiaoXacMinh = Utils.ConvertToInt32(dr["CanGiaoXacMinh"], 0);
                        Info.DaGiaoXacMinh = Utils.ConvertToInt32(dr["DaGiaoXacMinh"], 0);
                        Info.CanDuyetBCXacMinh = Utils.ConvertToInt32(dr["CanDuyetBCXacMinh"], 0);
                        Info.DaDuyetBCXacMinh = Utils.ConvertToInt32(dr["DaDuyetBCXacMinh"], 0);
                        Info.CanBanHanhQDGQ = Utils.ConvertToInt32(dr["CanBanHanhQDGQ"], 0);
                        Info.DaBanHanhQDGQ = Utils.ConvertToInt32(dr["DaBanHanhQDGQ"], 0);
                    }
                    dr.Close();
                }

            }
            catch
            {

                throw;
            }
            return Info;
        }

        public SoLieuModel GetDataDashBoard_By_User(CanhBaoParams p)
        {
            SoLieuModel Info = new SoLieuModel();

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@CoQuanID", SqlDbType.Int),
                new SqlParameter("@LoaiKhieuToID", SqlDbType.Int),
                new SqlParameter("@DenNgay", SqlDbType.DateTime),
                new SqlParameter("@TuNgay", SqlDbType.DateTime),
                new SqlParameter("@CapID", SqlDbType.Int),
                new SqlParameter("@CanBoID", SqlDbType.Int),
            };
            parms[0].Value = p.CoQuanID;
            parms[1].Value = p.LoaiKhieuToID ?? 0;
            parms[2].Value = p.DenNgay ?? Convert.DBNull;
            parms[3].Value = p.TuNgay ?? Convert.DBNull;
            //parms[2].Value = Utils.ConvertToNullableDateTime(p.DenNgay, null) ?? Convert.DBNull;
            parms[4].Value = p.CapID ?? Convert.DBNull;
            parms[5].Value = p.CanBoID ?? Convert.DBNull;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_DashBoard_GetDataDashBoard_By_User", parms))
                {
                    while (dr.Read())
                    {
                        //Info.QuaHanBanHanh = Utils.ConvertToInt32(dr["QuaHanBanHanh"], 0);
                        //Info.DenHanBanHanh = Utils.ConvertToInt32(dr["DenHanBanHanh"], 0);
                        //Info.ChuaDenHanBanHanh = Utils.ConvertToInt32(dr["ChuaDenHanBanHanh"], 0);

                        //Info.CanBanHanhGXM = Utils.ConvertToInt32(dr["CanBanHanhGXM"], 0);
                        //Info.DaBanHanhGXM = Utils.ConvertToInt32(dr["DaBanHanhGXM"], 0);

                        //Info.CanBanHanhGQ = Utils.ConvertToInt32(dr["CanBanHanhGQ"], 0);
                        //Info.DaBanHanhGQ = Utils.ConvertToInt32(dr["DaBanHanhGQ"], 0);

                        //Info.CanPheDuyet = Utils.ConvertToInt32(dr["CanPheDuyet"], 0);
                        //Info.DaPheDuyet = Utils.ConvertToInt32(dr["DaPheDuyet"], 0);

                        Info = MapDataReaderToSoLieuModel(dr);
                    }
                    dr.Close();
                }

            }
            catch
            {

                throw;
            }
            return Info;
        }

        public SoLieuModel MapDataReaderToSoLieuModel(SqlDataReader dr)
        {
            SoLieuModel model = new SoLieuModel();

            // Duyệt qua từng thuộc tính của SoLieuModel
            foreach (var prop in typeof(SoLieuModel).GetProperties())
            {
                string propName = prop.Name;

                // Kiểm tra xem cột có tồn tại trong SqlDataReader hay không
                if (HasColumn(dr, propName))
                {
                    // Gán giá trị từ SqlDataReader vào thuộc tính của model
                    prop.SetValue(model, Utils.ConvertToInt32(dr[propName], 0));
                }
                else
                {
                    // Nếu cột không tồn tại, gán giá trị mặc định (0)
                    prop.SetValue(model, 0);
                }
            }

            return model;
        }

        public bool HasColumn(IDataRecord dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}
