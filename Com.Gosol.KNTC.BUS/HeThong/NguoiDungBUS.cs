using Com.Gosol.KNTC.DAL.HeThong;
using Com.Gosol.KNTC.Models.HeThong;

namespace Com.Gosol.KNTC.BUS.HeThong
{
    public class NguoiDungBUS
    {
        private NguoiDungDAL _NguoiDungDAL;
        public NguoiDungBUS()
        {
            _NguoiDungDAL = new NguoiDungDAL();
        }

        private NguoiDungModel GetInfoByLogin(string UserName, string Password)
        {
            return _NguoiDungDAL.GetInfoByLogin(UserName, Password);
        }
        private NguoiDungModel GetInfoByLoginCAS(string Mail)
        {
            return _NguoiDungDAL.GetInfoByLoginCAS(Mail);
        }

        public bool VerifyUser(string UserName, string Password, string Email, ref NguoiDungModel NguoiDung)
        { 
            NguoiDung = GetInfoByLogin(UserName, Password);
            if (NguoiDung != null && (NguoiDung.TrangThai == 1 || NguoiDung.TrangThai == 0))
            {
                return true;
            }
            return false;
        }

        public NguoiDungModel GetByTenNguoiDung(string TenNguoiDung)
        {
            return _NguoiDungDAL.GetByTenNguoiDung(TenNguoiDung);
        }

        public int UpdateThoiGianlogin(NguoiDungModel nguoiDungModel, ref string message)
        {
            return _NguoiDungDAL.UpdateThoiGianlogin(nguoiDungModel, ref message);
        }


        public NguoiDungModel GetInfoByLoginSSO(string UserName)
        {
            return _NguoiDungDAL.GetInfoByLoginSSO(UserName);
        }

        public bool VerifyUserSSO(string UserName, ref NguoiDungModel NguoiDung)
        {
            NguoiDung = GetInfoByLoginSSO(UserName);
            if (NguoiDung != null && (NguoiDung.TrangThai == 1 || NguoiDung.TrangThai == 0))
            {
                return true;
            }
            return false;
        }
    }
}
