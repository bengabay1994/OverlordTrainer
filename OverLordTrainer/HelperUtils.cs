
namespace OverLordTrainer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class HelperUtils
    {
        public static string GetByteArrayAsHexString(IEnumerable<byte> ba) 
        {
            return string.Join(" ", ba.Select(b => b.ToString("X2")));
        }

    }
}
