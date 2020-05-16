using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileHandlerLib
{
    public static class FileHandler
    {
        static int maxSize = 60000;

        static public int GetFilePartsNumber(int byteSize)
        {
            return byteSize / maxSize;
        }

        static public byte[] GetFilePart(int number, byte[] data)
        {
            byte[] d;
            if (maxSize * (number + 1) > data.Length)
            {
                d = new byte[maxSize * (number + 1) - data.Length];
            }
            else
            {
                d = new byte[maxSize];
            }
            try
            {
                for (int i = maxSize * number; i < maxSize * (number + 1); i++)
                {
                    if (i < data.Length)
                        d[i - maxSize * number] = data[i];
                    else
                    {
                        break;
                    }
                }
            }
            catch
            {
            }
            return d;
        }

        static public byte[] GetFileFromParts(byte[] mainData, byte[] partData, int index)
        {
            byte[] data = new byte[mainData.Length];
            if (index > 0)
            {
                for (int i = 0; i < index * maxSize; i++)
                {
                    data[i] = mainData[i];
                }
            }
            for (int i = index * maxSize; i < (index + 1) * maxSize; i++)
            {
                try
                {
                    data[i] = partData[i - index * maxSize];
                }
                catch
                {
                    break;
                }

            }
            return data;
        }

        static public string GetFileName(string fileType)
        {

            string filename = Directory.GetCurrentDirectory() + "\\..\\..\\Records\\file";
            int i = 0;
            while (true)
            {
                if (File.Exists(filename + i.ToString() + fileType))
                {
                    i++;
                }
                else
                {
                    return filename + i.ToString() + fileType;
                }

            }
        }
    }
}
