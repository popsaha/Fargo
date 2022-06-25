using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace FargoWebApplication.Filter
{
    public class EncryptDecryptString
    {
        private static string ValidChars = "QAZ2WSX3" + "EDC4RFV5" + "TGB6YHN7" + "UJM8K9LP";

        public static string Encrypt(string plainText)
        {
            byte[] inputByte = Encoding.UTF8.GetBytes(plainText);
            StringBuilder sb = new StringBuilder();
            byte index;
            int hi = 5;
            int currentByte = 0;
            while (currentByte < inputByte.Length)
            {
                if (hi > 8)
                {
                    index = (byte)(inputByte[currentByte++] >> (hi - 5));
                    if (currentByte != inputByte.Length)
                    {
                        index = (byte)(((byte)(inputByte[currentByte] << (16 - hi)) >> 3) | index);
                    }
                    hi -= 3;
                }
                else if (hi == 8)
                {
                    index = (byte)(inputByte[currentByte++] >> 3);
                    hi -= 3;
                }
                else
                {
                    index = (byte)((byte)(inputByte[currentByte] << (8 - hi)) >> 3);
                    hi += 5;
                }
                sb.Append(ValidChars[index]);
            }
            return sb.ToString();
        }

        public static byte[] FromBase32String(string encryptedText)
        {
            int numBytes = encryptedText.Length * 5 / 8;
            byte[] bytes = new Byte[numBytes];
            encryptedText = encryptedText.ToUpper();
            int bit_buffer;
            int currentCharIndex;
            int bits_in_buffer;
            if (encryptedText.Length < 3)
            {
                bytes[0] = (byte)(ValidChars.IndexOf(encryptedText[0]) | ValidChars.IndexOf(encryptedText[1]) << 5);
                return bytes;
            }
            bit_buffer = (ValidChars.IndexOf(encryptedText[0]) | ValidChars.IndexOf(encryptedText[1]) << 5);
            bits_in_buffer = 10;
            currentCharIndex = 2;
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)bit_buffer;
                bit_buffer >>= 8;
                bits_in_buffer -= 8;
                while (bits_in_buffer < 8 && currentCharIndex < encryptedText.Length)
                {
                    bit_buffer |= ValidChars.IndexOf(encryptedText[currentCharIndex++]) << bits_in_buffer;
                    bits_in_buffer += 5;
                }
            }
            return bytes;
        }
    }
}