namespace Com.Gosol.KNTC.Security
{
    using System;

    internal class AKNTCessControlExceptions : DatabaseProxyException
    {
        public AKNTCessControlExceptions(string errorMessage) : base(errorMessage)
        {
        }
    }
}
