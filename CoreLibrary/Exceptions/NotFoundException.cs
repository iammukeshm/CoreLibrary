using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLibrary.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("Resource Not Found!")
        {

        }
    }
}
