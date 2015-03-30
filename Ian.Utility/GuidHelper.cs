using System;

namespace Ian.Utility
{
    public static class GuidHelper
    {
        public static long ToInt64(this Guid g)
        {
            byte[] buffer = g.ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }
    }
}
