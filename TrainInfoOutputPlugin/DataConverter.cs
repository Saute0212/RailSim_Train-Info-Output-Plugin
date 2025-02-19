using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrainInfoOutputPlugin
{
    public class DataConverter
    {
        //int型の必要なビット数を求める関数
        private static int GetBitCount_int(int value)
        {
            if (value == 0)
            {
                return 1;
            }
            else
            {
                return (int)Math.Ceiling(Math.Log(Math.Abs(value)));
            }
        }

        //int型の必要なバイト数を求める関数
        private static int GetByteLen_int(int bitLen)
        {
            return (int)Math.Ceiling((double)bitLen / 7);
        }

        //byte列を結合する関数
        private static byte[] CombineBytes(byte[] array1, byte[] array2)
        {
            var result = new List<byte>(array1);
            result.AddRange(array2);
            return result.ToArray();
        }

        //float型の小数部分をbyte列(1Byte)に変換する関数
        private static byte[] FloatFractionToBytes(float value)
        {
            const int bitLen = 7;

            float tmp = Math.Abs(value % 1);

            byte[] result = new byte[1];

            for (int i = 0; i < bitLen; i++)
            {
                tmp *= 2;
                if (tmp >= 1.0f)
                {
                    result[0] |= (byte)(1 << ((bitLen - 1) - i));
                    tmp -= 1.0f;
                }
            }

            result[0] |= 0x80;

            return result;
        }

        //double型の小数部分をbyte列(2Byte)に変換する関数
        private static byte[] DoubleFractionToBytes(double value)
        {
            const int bitLen = 14;

            double tmp = Math.Abs(value % 1);

            byte[] result = new byte[2];

            for (int i = 0; i < bitLen; i++)
            {
                tmp *= 2;
                if (tmp >= 1.0)
                {
                    result[i / 7] = (byte)(result[i / 7] << 1 | 1);
                    tmp -= 1.0;
                }
                else
                {
                    result[i / 7] <<= 1;
                }
            }
            
            result[0] |= 0x80;
            result[1] |= 0x80;

            return result;
        }

        //int型をbyte列に変換する関数
        public static byte[] IntToBytes(int value)
        {
            int byteLen = GetByteLen_int(GetBitCount_int(value));
            byte[] result = new byte[byteLen];

            value = Math.Abs(value);

            for (int i = 1; i <= byteLen; i++)
            {
                byte tmp = (byte)(value & 0x7F);
                tmp |= 0x80;
                result[byteLen - i] = tmp;
                value >>= 7;
            }

            return result;
        }

        //float型をbyte列に変換する関数
        public static byte[] FloatToBytes(float value)
        {
            int tmp_int = (int)Math.Truncate(value);
            float tmp_float = value - tmp_int;
            byte[] result = CombineBytes(IntToBytes(tmp_int), FloatFractionToBytes(tmp_float));
            return result;
        }

        //double型をbyte列に変換する関数
        public static byte[] DoubleToBytes(double value)
        {
            int tmp_int = (int)Math.Truncate(value);
            double tmp_double = value - tmp_int;            
            byte[] result = CombineBytes(IntToBytes(tmp_int), DoubleFractionToBytes(tmp_double));
            return result;
        }

        //正負を判定する関数
        public static bool IsNegative(object value)
        {
            if (value is int intValue)
            {
                return intValue < 0;
            }
            else if (value is float floatValue)
            {
                return floatValue < 0;
            }
            else if (value is double doubleValue)
            {
                return doubleValue < 0;
            }
            else
            {
                return false;
            }
        }

        //string型をbyte列に変換する関数
        public static byte[] StringToBytes(string str)
        {
            int index = 0;
            int len = GetStringLength(str);
            byte[] result = new byte[len];

            foreach (char c in str)
            {
                result[index] = GetCharacterBytes(c);
                index++;
            }

            return result;
        }

        //文字列の長さを取得する関数
        private static int GetStringLength(string str)
        {
            return str.Length;
        }

        //文字の変換フォーマット
        private static byte GetCharacterBytes(char c)
        {
            byte result = 0x00;

            switch (c)
            {
                //数字(0-9)
                case '0':
                    result = 0x01;
                    break;
                case '1':
                    result = 0x02;
                    break;
                case '2':
                    result = 0x03;
                    break;
                case '3':
                    result = 0x04;
                    break;
                case '4':
                    result = 0x05;
                    break;
                case '5':
                    result = 0x06;
                    break;
                case '6':
                    result = 0x07;
                    break;
                case '7':
                    result = 0x08;
                    break;
                case '8':
                    result = 0x09;
                    break;
                case '9':
                    result = 0x0A;
                    break;

                //文字(a-z)
                case 'a':
                    result = 0x0B;
                    break;
                case 'b':
                    result = 0x0C;
                    break;
                case 'c':
                    result = 0x0D;
                    break;
                case 'd':
                    result = 0x0E;
                    break;
                case 'e':
                    result = 0x0F;
                    break;
                case 'f':
                    result = 0x10;
                    break;
                case 'g':
                    result = 0x11;
                    break;
                case 'h':
                    result = 0x12;
                    break;
                case 'i':
                    result = 0x13;
                    break;
                case 'j':
                    result = 0x14;
                    break;
                case 'k':
                    result = 0x15;
                    break;
                case 'l':
                    result = 0x16;
                    break;
                case 'm':
                    result = 0x17;
                    break;
                case 'n':
                    result = 0x18;
                    break;
                case 'o':
                    result = 0x19;
                    break;
                case 'p':
                    result = 0x1A;
                    break;
                case 'q':
                    result = 0x1B;
                    break;
                case 'r':
                    result = 0x1C;
                    break;
                case 's':
                    result = 0x1D;
                    break;
                case 't':
                    result = 0x1E;
                    break;
                case 'u':
                    result = 0x1F;
                    break;
                case 'v':
                    result = 0x20;
                    break;
                case 'w':
                    result = 0x21;
                    break;
                case 'x':
                    result = 0x22;
                    break;
                case 'y':
                    result = 0x23;
                    break;
                case 'z':
                    result = 0x24;
                    break;

                //文字(A-Z)
                case 'A':
                    result = 0x25;
                    break;
                case 'B':
                    result = 0x26;
                    break;
                case 'C':
                    result = 0x27;
                    break;
                case 'D':
                    result = 0x28;
                    break;
                case 'E':
                    result = 0x29;
                    break;
                case 'F':
                    result = 0x2A;
                    break;
                case 'G':
                    result = 0x2B;
                    break;
                case 'H':
                    result = 0x2C;
                    break;
                case 'I':
                    result = 0x2D;
                    break;
                case 'J':
                    result = 0x2E;
                    break;
                case 'K':
                    result = 0x2F;
                    break;
                case 'L':
                    result = 0x30;
                    break;
                case 'M':
                    result = 0x31;
                    break;
                case 'N':
                    result = 0x32;
                    break;
                case 'O':
                    result = 0x33;
                    break;
                case 'P':
                    result = 0x34;
                    break;
                case 'Q':
                    result = 0x35;
                    break;
                case 'R':
                    result = 0x36;
                    break;
                case 'S':
                    result = 0x37;
                    break;
                case 'T':
                    result = 0x38;
                    break;
                case 'U':
                    result = 0x39;
                    break;
                case 'V':
                    result = 0x3A;
                    break;
                case 'W':
                    result = 0x3B;
                    break;
                case 'X':
                    result = 0x3C;
                    break;
                case 'Y':
                    result = 0x3D;
                    break;
                case 'Z':
                    result = 0x3E;
                    break;

                //特殊文字(! " # $ % & ' ( ) = - + * / ^ < > | { } [ ] : ; , . _ @ ` \ space)
                case '!':
                    result = 0x3F;
                    break;
                case '"':
                    result = 0x40;
                    break;
                case '#':
                    result = 0x41;
                    break;
                case '$':
                    result = 0x42;
                    break;
                case '%':
                    result = 0x43;
                    break;
                case '&':
                    result = 0x44;
                    break;
                case '\'':
                    result = 0x45;
                    break;
                case '(':
                    result = 0x46;
                    break;
                case ')':
                    result = 0x47;
                    break;
                case '=':
                    result = 0x48;
                    break;
                case '-':
                    result = 0x49;
                    break;
                case '+':
                    result = 0x4A;
                    break;
                case '*':
                    result = 0x4B;
                    break;
                case '/':
                    result = 0x4C;
                    break;
                case '^':
                    result = 0x4D;
                    break;
                case '<':
                    result = 0x4E;
                    break;
                case '>':
                    result = 0x4F;
                    break;
                case '|':
                    result = 0x50;
                    break;
                case '{':
                    result = 0x51;
                    break;
                case '}':
                    result = 0x52;
                    break;
                case '[':
                    result = 0x53;
                    break;
                case ']':
                    result = 0x54;
                    break;
                case ':':
                    result = 0x55;
                    break;
                case ';':
                    result = 0x56;
                    break;
                case ',':
                    result = 0x57;
                    break;
                case '.':
                    result = 0x58;
                    break;
                case '_':
                    result = 0x59;
                    break;
                case '@':
                    result = 0x5A;
                    break;
                case '`':
                    result = 0x5B;
                    break;
                case '\\':
                    result = 0x5C;
                    break;
                case ' ':
                    result = 0x5D;
                    break;
                default:
                    break;
            }

            result |= 0x80;

            return result;
        }
    }
}
