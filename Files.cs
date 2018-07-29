using System;
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
                BinaryWriter writer = new BinaryWriter(File.Open(fileName, FileMode.Create));
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
                BinaryWriter keyWriter = new BinaryWriter(File.Open(keyFile, FileMode.Create));
                BinaryWriter valueWriter = new BinaryWriter(File.Open(valueFile, FileMode.Create));
                foreach (ulong value in dict.Keys)
                {
                    keyWriter.Write(value.ToString());
                    valueWriter.Write(dict[value].ToString());
                }
                keyWriter.Close();
                valueWriter.Close();
            });
        }

        public static async Task<List<ulong>> FileToList(string fileName)
        {
            List<ulong> list = new List<ulong>();
            string item;

            BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.OpenOrCreate));
            for (int i = 0; i < reader.BaseStream.Length; i += item.Length + 1)
            {
                item = reader.ReadString();
                list.Add(Convert.ToUInt64(item));
            }
            reader.Close();

            return await Task.Run(() =>
            {
                return list;
            });
        }

        public static async Task<Dictionary<ulong, ulong>> FileToDict(string keyFile, string valueFile)
        {
            Dictionary<ulong, ulong> dict = new Dictionary<ulong, ulong>();
            List<ulong> keyList = new List<ulong>();
            List<ulong> valueList = new List<ulong>();

            string keyItem;
            BinaryReader keyReader = new BinaryReader(File.Open(keyFile, FileMode.OpenOrCreate));
            for (int i = 0; i < keyReader.BaseStream.Length; i += keyItem.Length + 1)
            {
                keyItem = keyReader.ReadString();
                keyList.Add(Convert.ToUInt64(keyItem));
            }
            keyReader.Close();

            string valueItem;
            BinaryReader valueReader = new BinaryReader(File.Open(valueFile, FileMode.OpenOrCreate));
            for (int i = 0; i < valueReader.BaseStream.Length; i += valueItem.Length + 1)
            {
                valueItem = valueReader.ReadString();
                valueList.Add(Convert.ToUInt64(valueItem));
            }
            valueReader.Close();

            for (int i = 0; i < keyList.Count; i++)
            {
                dict.Put(keyList[i], valueList[i]);
            }

            return await Task.Run(() =>
            {
                return dict;
            });
        }
    }
}
