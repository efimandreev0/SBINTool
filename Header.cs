using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBIN
{
    internal class Header
    {
        public int block1size { get; set; }
        public int textblockSize { get; set; }
        public Header(BinaryReader reader)
        {
            reader.BaseStream.Position = 0x94;
            block1size = reader.ReadInt32();
            reader.BaseStream.Position += block1size + 8;
            textblockSize = reader.ReadInt32() + 0x18;
            Console.WriteLine(reader.BaseStream.Position);
            reader.BaseStream.Position += 8;
        }
    }
}
