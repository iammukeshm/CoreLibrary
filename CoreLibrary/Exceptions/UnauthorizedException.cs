using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLibrary.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(): base("Not Authorized!")
        {

        }
    }
}
