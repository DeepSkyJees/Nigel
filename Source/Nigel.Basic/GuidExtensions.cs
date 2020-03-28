using System;

namespace Nigel.Basic
{
    public static class GuidExtensions
    {
        public static string ToGuidString(this Guid guid)
        {
            return guid.ToString("N");
        }
    }
}
