// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("llLdzcAoY4KK5HY6swAUOWZDhnzH9Jt+NIDDThhIXFhF2fOl5TDYbt9biOuDCVHvUMefDFBL//3ikAnUEDzsdIEtHXJr5PNK4ktxQQV/X6GbGBYZKZsYExubGBgZg92/4L3dqdszBVCNlaLmecfqyCYf9Ix2ydVLthNHvpdLoxUEltNi4E5vMnSCkuliZFiAGO8FTXFub4oqfs3kb8ujJzH/Kj4B4ZD56DTOjtmdm8AgzvBP54m7nGHibF4RuhnG4qBgvyr4TDExHRC44OBVPw84JEtJIryn488uqYCXNKXWKpnfwrPsHXZdqwm7O0QEKZsYOykUHxAzn1Gf7hQYGBgcGRrrUGb47TdodTky0J6VOKvmwhzW0oVJMataZmI+ZhsaGBkY");
        private static int[] order = new int[] { 3,9,12,12,9,13,9,8,13,9,13,11,12,13,14 };
        private static int key = 25;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
