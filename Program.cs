using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBIN
{
    internal class Program
    {
        static void Main(string[] args)
        {
           if (args[0].Contains(".txt"))
            {
                Rebuild(args[1], args[0]);
            }
            if (args[0].Contains(".bin"))
            {
                Extract(args[0]);
            }
        }
        public static void Extract(string SBIN)
        {
            var reader = new BinaryReader(File.OpenRead(SBIN));
            Header header = new Header(reader);
            List <string> strings = new List<string>();
            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                int size = reader.ReadInt32();
                if (size > reader.BaseStream.Position)
                {
                    reader.BaseStream.Position += 4;
                }
                else
                {
                    strings.Add(Encoding.UTF8.GetString(reader.ReadBytes(size)).Replace("\n","<lf>").Replace("\r","<br>"));
                }
            }
            File.WriteAllLines(Path.GetFileNameWithoutExtension(SBIN) + ".txt", strings);
        }
        public static void Rebuild(string sbin, string txt)
        {
            var reader = new BinaryReader(File.OpenRead(sbin));
            Header header = new Header(reader);
            string[] strings = File.ReadAllLines(txt);
            int[] size = new int[strings.Length];
            var pos = reader.BaseStream.Position;
            reader.Close();
            var writer = new BinaryWriter(File.OpenWrite(sbin));
            writer.BaseStream.Position = pos;
            for (int i = 0; i < strings.Length; i++)
            {
                strings[i] = strings[i].Replace("<lf>","\n").Replace("<br>","\r");
                size[i] = strings[i].Length;
                writer.Write(size[i]);
                writer.Write(Encoding.UTF8.GetBytes(strings[i]));
            }
            byte[] bytes = new byte[] { 0x53, 0x59, 0x4D, 0x42, 0x04, 0x00, 0x00, 0x00, 0x15, 0xF5, 0x95, 0x4B, 0x00, 0x00, 0x00, 0x00 };
            writer.Write(bytes);
            writer.BaseStream.Position = pos - 0xC;
            var blockSize = writer.BaseStream.Length - writer.BaseStream.Position - 0x18;
            writer.Write((int)blockSize);
        }
    }
}
