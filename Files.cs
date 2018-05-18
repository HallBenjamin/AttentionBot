using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AttentionBot
{
    class Files
    {
        public static async Task WriteToFile(List<ulong> list, string fileName)
        {
            await Task.Run(() =>
            {
                BinaryWriter writer = new BinaryWriter(File.Open(fileName, FileMode.Truncate));
                foreach (ulong value in list)
                {
                    writer.Write(value.ToString());
                }
                writer.Close();
            });
        }

        public static async Task WriteToFile(Dictionary<ulong, ulong> dict, string keyFile, string valueFile)
        {
            await Task.Run(() =>
            {
                BinaryWriter keyWriter = new BinaryWriter(File.Open(keyFile, FileMode.Truncate));
                BinaryWriter valueWriter = new BinaryWriter(File.Open(valueFile, FileMode.Truncate));
                foreach (ulong value in dict.Keys)
                {
                    keyWriter.Write(value.ToString());
                    valueWriter.Write(dict[value].ToString());
                }
                keyWriter.Close();
                valueWriter.Close();
            });
        }
    }
}
