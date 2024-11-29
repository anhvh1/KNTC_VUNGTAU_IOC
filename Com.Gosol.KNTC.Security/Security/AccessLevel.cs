using System;
using System.Web;
namespace Com.Gosol.KNTC.Security
{
   
    public enum AccessLevel
    {
        Create = 2,
        Delete = 8,
        Edit = 4,
        FullAKNTCess = 0x1f,
        NoAKNTCess = 0,
        Publish = 0x10,
        Read = 1
    }
   
}
