namespace GeometricPublicKeyCrypto{
    class Helpers {
        
        static public string EncodeDoubleToBase64(double value)
        {
            // Convert the double to its binary representation as an 8-byte array
            byte[] bytes = BitConverter.GetBytes(value);

            // Encode the byte array to Base64 manually (optimized approach)
            const string base64Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
            char[] base64 = new char[12]; // Base64 encoding of 8 bytes will always be 12 characters

            int index = 0;
            for (int i = 0; i < 8; i += 3)
            {
                int chunk = bytes[i] | (i + 1 < 8 ? bytes[i + 1] << 8 : 0) | (i + 2 < 8 ? bytes[i + 2] << 16 : 0);
                base64[index++] = base64Chars[(chunk >> 18) & 63];
                base64[index++] = base64Chars[(chunk >> 12) & 63];
                base64[index++] = i + 1 < 8 ? base64Chars[(chunk >> 6) & 63] : '=';
                base64[index++] = i + 2 < 8 ? base64Chars[chunk & 63] : '=';
            }
            return new string(base64);
        }
        static public string ConvertDoubleToHex(double value)
        {
            // Convert the double to its binary representation as an 8-byte array
            byte[] bytes = BitConverter.GetBytes(value);

            // Build the hexadecimal string manually (optimized for speed)
            char[] hexChars = new char[16];
            const string hexLookup = "0123456789ABCDEF";

            for (int i = 0; i < bytes.Length; i++)
            {
                hexChars[i * 2] = hexLookup[bytes[i] >> 4];       // High nibble
                hexChars[i * 2 + 1] = hexLookup[bytes[i] & 0x0F]; // Low nibble
            }
            return new string(hexChars);
        }
    }
}