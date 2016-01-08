// Copyright (c) Arctium Emulation.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using DataExtractor.Maps.Defines;

namespace DataExtractor
{
    public class APAKStream : BinaryWriter
    {
        public APAKStream() : base(new MemoryStream())
        {
            Write(new[] { 'K', 'A', 'P', 'A' });
            Write((byte)1);
            Write((ushort)0);
        }

        public void WriteMap(Map map)
        {
            using (var temp = new BinaryWriter(new MemoryStream()))
            {
                foreach (var tile in map.Tiles)
                {
                    temp.Write(tile.Value.Id);
                    temp.Write(tile.Value.IndexX);
                    temp.Write(tile.Value.IndexY);

                    temp.Write(tile.Value.Chunks.Count);

                    tile.Value.Chunks.ForEach(mc =>
                    {
                        temp.Write(mc.IndexX);
                        temp.Write(mc.IndexY);
                        temp.Write(mc.Area);
                    });
                }

                var tileData = (temp.BaseStream as MemoryStream).ToArray();
                var compressedTileData = Compress(tileData);

                Write(map.Id);
                Write(map.Name);

                Write(tileData.Length);
                Write(compressedTileData.Length);
                Write(compressedTileData);
            }
        }

        byte[] Compress(byte[] data)
        {
            byte[] compressedData;

            using (var ms = new MemoryStream())
            {
                using (var ds = new DeflateStream(ms, CompressionLevel.Optimal))
                {
                    ds.Write(data, 0, data.Length);
                    ds.Flush();
                }

                compressedData = ms.ToArray();
            }

            return compressedData;
        }
    }
}
    