using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ian.Utility
{
    public static class GuidHelper
    {
        public static long ToLong(this Guid g)
        {
            byte[] buffer = g.ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }
    }
}
