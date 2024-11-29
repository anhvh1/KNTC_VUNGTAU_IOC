namespace Com.Gosol.KNTC.Security
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections;
    using System.Web;
    //using System.Web.Configuration;
    //using System.Web.Security;

    public sealed class AKNTCessControl
    {
        private AKNTCessControl()
        {
            throw new AKNTCessControlExceptions("CanNotCreateClass");
        }

        public static void RequestAKNTCessInfo(int id)
        {
            Hashtable hashtable;
            Hashtable hashtable2;
            Hashtable hashtable3;
            Hashtable hashtable4;
            AKNTCessControlData.RequestAKNTCessRight(id, out hashtable, out hashtable2);
            AKNTCessControlData.RequestUserInfo(id, out hashtable4, out hashtable3);
            AKNTCessControlPrincipal principal = AKNTCessControlPrincipal.CreateInstance(hashtable3, hashtable, hashtable2, hashtable4);
            //HttpContext.Current.Session.Add("USER$DA31A175C7679319BFFEDF3EF282D1F4", principal);
            //HttpContext.Current.User = principal;
        }
        public static void SetAKNTCessInfo(AKNTCessControlPrincipal principal)
        {
            //Hashtable hashtable;
            //Hashtable hashtable2;
            //Hashtable hashtable3;
            //Hashtable hashtable4;
            //AKNTCessControlData.RequestAKNTCessRight(id, out hashtable, out hashtable2);
            //AKNTCessControlData.RequestUserInfo(id, out hashtable4, out hashtable3);
            //AKNTCessControlPrincipal principal = AKNTCessControlPrincipal.CreateInstance(hashtable3, hashtable, hashtable2, hashtable4);
            //HttpContext.Current.User = principal;
            //System.Threading.Thread.CurrentPrincipal = principal;
            //HttpContext.Current.Session.Add("USER$DA31A175C7679319BFFEDF3EF282D1F4", principal);
            //context.User = principal;
            //context.Session.Add("USER$DA31A175C7679319BFFEDF3EF282D1F4", principal);
        }

        public static void SignOut()
        {
            //HttpContext.Current.Session.Remove("USER$DA31A175C7679319BFFEDF3EF282D1F4");
        }

        public static void LoockScreen()
        {
            //HttpContext.Current.Session.Remove("USER$DA31A175C7679319BFFEDF3EF282D1F4");
        }

        //public static bool IsLoggedIn
        //{
        //    get
        //    {
        //        return (User != null);
        //    }
        //}

        //public static AKNTCessControlPrincipal User
        //{
        //    get
        //    {
        //        //return (HttpContext.Current.Session["USER$DA31A175C7679319BFFEDF3EF282D1F4"] as AKNTCessControlPrincipal);
        //        return ( AKNTCessControlPrincipal);
        //    }
        //}
    }
}
