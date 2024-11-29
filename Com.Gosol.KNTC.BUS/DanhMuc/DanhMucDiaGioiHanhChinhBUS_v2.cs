using Com.Gosol.KNTC.DAL.DanhMuc;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.DanhMuc;
using Com.Gosol.KNTC.Ultilities;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.BUS.DanhMuc
{
    public class DanhMucDiaGioiHanhChinhBUS_v2
    {
        private DanhMucDiaGioiHanhChinhDAL_v2 DanhMucDiaGioiHanhChinhDAL_v2;
        public DanhMucDiaGioiHanhChinhBUS_v2()
        {
            this.DanhMucDiaGioiHanhChinhDAL_v2 = new DanhMucDiaGioiHanhChinhDAL_v2();
        }

        public List<object> GetAllByCap(ThamSoLocDanhMuc1 thamSoLocDanhMuc1)
        {
            
            return new DanhMucDiaGioiHanhChinhDAL_v2().GetAllByCap( thamSoLocDanhMuc1);
        }
        public BaseResultModel GetDGHCByIDAndCap(ThamSoLocDanhMuc1 thamSoLocDanhMuc1)
        {
            var Result = new BaseResultModel();
            try
            {

                DanhMucDiaGioiHanhChinhMODlUpdatePartial_v2 Data;
                Data = DanhMucDiaGioiHanhChinhDAL_v2.GetDGHCByIDAndCap(thamSoLocDanhMuc1);
                if (Data  == null)
                {
                    Result.Status = 1;
                    Result.Message = "Các đơn vị không được để trống";
                    Result.Data = Data;
                   
                }
                
                    Result.Status = (Data.ID >  0 ) ? 1 : 0;
                    Result.Data = Data;
                   
                
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;  

            
        }
        //-----insert tinh-------
        public Dictionary<int, int> InsertTinh(DanhMucDiaGioiHanhChinhMOD_v2 DanhMucDiaGioiHanhChinhMOD_v2, ref int ID)
        {
            Dictionary<int, int> dic = new Dictionary<int, int>();
            try
            {
                dic = new DanhMucDiaGioiHanhChinhDAL_v2().InsertTinh(DanhMucDiaGioiHanhChinhMOD_v2, ref ID);
                return dic;
            }
            catch (Exception ex)
            {
                return dic;
                throw ex;
            }
        }
        //---
        /*public int UpdateTinh(DanhMucDiaGioiHanhChinhMOD_v2 DanhMucDiaGioiHanhChinhMOD_v2)
        {
            int val = 0;
            try
            {
                val = new DanhMucDiaGioiHanhChinhDAL_v2().UpdateTinh(DanhMucDiaGioiHanhChinhMOD_v2);
                return val;
            }
            catch (Exception ex)
            {
                return val;
                throw ex;
            }
        }*/
       
        // insert huyen
        public Dictionary<int, int> InsertHuyen(DanhMucDiaGioiHanhChinhMOD_v2 DanhMucDiaGioiHanhChinhMOD_v2, ref int ID)
        {
            Dictionary<int, int> dic = new Dictionary<int, int>();
            try
            {
                dic = new DanhMucDiaGioiHanhChinhDAL_v2().InsertHuyen(DanhMucDiaGioiHanhChinhMOD_v2, ref ID);
                return dic;
            }
            catch (Exception ex)
            {
                return dic;
                throw ex;
            }
        }
        //---- 
        /*public int UpdateHuyen(DanhMucDiaGioiHanhChinhMOD_v2 DanhMucDiaGioiHanhChinhMOD_v2)
        {
            int val = 0;
            try
            {
                val = new DanhMucDiaGioiHanhChinhDAL_v2().UpdateHuyen(DanhMucDiaGioiHanhChinhMOD_v2);
                return val;
            }
            catch (Exception ex)
            {
                return val;
                throw ex;
            }
        }*/
        
        // insert xa
        public Dictionary<int, int> InsertXa(DanhMucDiaGioiHanhChinhMOD_v2 DanhMucDiaGioiHanhChinhMOD_v2, ref int ID)
        {
            Dictionary<int, int> dic = new Dictionary<int, int>();
            try
            {
                dic = new DanhMucDiaGioiHanhChinhDAL_v2().InsertXa(DanhMucDiaGioiHanhChinhMOD_v2, ref ID);
                return dic;
            }
            catch (Exception ex)
            {
                return dic;
                throw ex;
            }
        }

        //----
        /*public int UpdateXa(DanhMucDiaGioiHanhChinhMOD_v2 DanhMucDiaGioiHanhChinhMOD_v2)
        {
            int val = 0;
            try
            {
                val = new DanhMucDiaGioiHanhChinhDAL_v2().UpdateXa(DanhMucDiaGioiHanhChinhMOD_v2);
                return val;
            }
            catch (Exception ex)
            {
                return val;
                throw ex;
            }
        }*/
        

        // danh sach 1
        public BaseResultModel DanhSachTinh(ThamSoLocDanhMuc_v2 p)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = DanhMucDiaGioiHanhChinhDAL_v2.DanhSachTinh(p);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }


        //-------------xoa tinh - huyen - xa 

        public BaseResultModel XoaTinh(int? TinhID)
        {
            var Result = new BaseResultModel();
            try
            {
                // kiểm tra frontend có truyền tham số lên không
                if (TinhID == null || TinhID < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng chọn thông tin trước khi thực hiện thao tác!";
                    return Result;
                }

                else
                {
                    Result = DanhMucDiaGioiHanhChinhDAL_v2.XoaTinh(TinhID.Value);
                }

            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = Constant.API_Error_System;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        

        public BaseResultModel XoaHuyen(int? HuyenID)
        {
            var Result = new BaseResultModel();
            try
            {
                // kiểm tra frontend có truyền tham số lên không
                if (HuyenID == null || HuyenID < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng chọn thông tin trước khi thực hiện thao tác!";
                    return Result;
                }
                
                else
                {
                    Result = DanhMucDiaGioiHanhChinhDAL_v2.XoaHuyen(HuyenID.Value);
                }


                
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                //Result.Message = Constant.API_Error_System;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel XoaXa(int? XaID)
        {
            var Result = new BaseResultModel();
            try
            {
                // kiểm tra frontend có truyền tham số lên không
                if (XaID == null || XaID < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng chọn thông tin trước khi thực hiện thao tác!";
                    return Result;
                } 

                Result = DanhMucDiaGioiHanhChinhDAL_v2.XoaXa(XaID.Value);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                //Result.Message = Constant.API_Error_System;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        //------------------------

        public BaseResultModel CapNhatTinh(DanhMucDiaGioiHanhChinhMODPartial_v2 item)
        {
            var Result = new BaseResultModel();
            try
            {
                // validate data nhận về từ frontend
                if (item == null  || item.ID < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng nhập thông tin cần cập nhật!";
                    return Result;
                }
                else if (item.Ten == null)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng nhập thông tin cần cập nhật!";
                    return Result;
                }
                else if (item.TenDayDu == null)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng nhập thông tin cần cập nhật!";
                    return Result;
                }else if(item.Cap == null)
                {
                    Result.Status = 0;
                    Result.Message = "Cấp không được để trống !";
                    return Result;
                }


                // thực hiện cập nhật 
                Result = DanhMucDiaGioiHanhChinhDAL_v2.CapNhatTinh(item);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        //---
        public BaseResultModel CapNhatHuyen(DanhMucDiaGioiHanhChinhMODPartial_v2 item)
        {
            var Result = new BaseResultModel();
            try
            {
                // validate data nhận về từ frontend
                if (item == null  || item.ID < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng nhập thông tin cần cập nhật!";
                    return Result;
                } else if(item.Ten == null)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng nhập thông tin cần cập nhật!";
                    return Result;
                }else if(item.TenDayDu == null)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng nhập thông tin cần cập nhật!";
                    return Result;
                }
                else if (item.Cap == null)
                {
                    Result.Status = 0;
                    Result.Message = "Cấp không được để trống !";
                    return Result;
                }

                // thực hiện cập nhật 
                Result = DanhMucDiaGioiHanhChinhDAL_v2.CapNhatHuyen(item);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

        public BaseResultModel CapNhatXa(DanhMucDiaGioiHanhChinhMODPartial_v2 item)
        {
            var Result = new BaseResultModel();
            try
            {
                // validate data nhận về từ frontend
                if (item == null  || item.ID < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng nhập thông tin cần cập nhật!";
                    return Result;
                }
                else if (item.Ten == null)
                {
                    Result.Status = 0;
                    Result.Message = " Vui lòng nhập thông tin cần cập nhật!";
                    return Result;
                }
                else if (item.TenDayDu == null)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng nhập thông tin cần cập nhật!";
                    return Result;
                }
                else if (item.Cap == null)
                {
                    Result.Status = 0;
                    Result.Message = "Cấp không được để trống !";
                    return Result;
                }   

                // thực hiện cập nhật 
                Result = DanhMucDiaGioiHanhChinhDAL_v2.CapNhatXa(item);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }
            return Result;
        }

    }
}
