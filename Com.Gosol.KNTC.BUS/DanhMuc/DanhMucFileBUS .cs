using Com.Gosol.KNTC.DAL.DanhMuc;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.DanhMuc;
using System;
using System.Collections.Generic;

//Create by TienKM - 13/10/2022
namespace Com.Gosol.KNTC.BUS.DanhMuc
{
    public class DanhMucFileBUS
    {
        private DanhMucFileDAL _danhMucFileDAL;

        public DanhMucFileBUS()
        {
            _danhMucFileDAL = new DanhMucFileDAL();
        }

        public BaseResultModel DanhSachNhomFile(ThamSoLocDanhMuc thamSo)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = _danhMucFileDAL.DanhSachNhomFile(thamSo);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel DanhSachFile(ThamSoFileModel thamSo)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = _danhMucFileDAL.DanhSachFile(thamSo);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel ThemNhomFile(ThemNhomFileModel nhomFile)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = _danhMucFileDAL.ThemMoiNhomFile(nhomFile);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel UpdateNhomFile(DanhMucNhomFileModel nhomFile)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = _danhMucFileDAL.UpdateNhomFile(nhomFile);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel XoaNhomFile(int nhomFileID)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = _danhMucFileDAL.XoaNhomFile(nhomFileID);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel ChiTietNhomFile(int nhomFileID)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = _danhMucFileDAL.ChiTietNhomFile(nhomFileID);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel ChiTietFile(int fileID)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = _danhMucFileDAL.ChiTietFile(fileID);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel ThemFile(ThemFileModel file)
        {
            var Result = new BaseResultModel();


            try
            {
                Result = _danhMucFileDAL.ThemMoiFile(file);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel XoaFile(int fileID)
        {
            var Result = new BaseResultModel();


            try
            {
                Result = _danhMucFileDAL.XoaFile(fileID);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel UpdateFile(UpdateFileModel file)
        {
            var Result = new BaseResultModel();
            try
            {
                Result = _danhMucFileDAL.UpdateFile(file);
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
                Result.Data = null;
            }

            return Result;
        }

        public BaseResultModel DanhSachChucNang()
        {
            var Result = new BaseResultModel();


            try
            {
                Result = _danhMucFileDAL.DanhSachChucNang();
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