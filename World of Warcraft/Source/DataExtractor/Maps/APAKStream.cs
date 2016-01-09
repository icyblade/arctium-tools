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
        public BinaryWriter MapStream { get; }

        public APAKStream() : base(new MemoryStream())
        {
            MapStream = new BinaryWriter(new MemoryStream());

            Write(new[] { 'K', 'A', 'P', 'A' });
            Write((byte)1);
            Write((ushort)0);
        }

        public void WriteMapDataOffsets(ushort map, uint offset)
        {
            Write(map);
            Write(offset);
        }

        public void GenerateMapData(Map map)
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

                MapStream.Write(map.Id);
                MapStream.Write(map.Name);

                MapStream.Write(tileData.Length);
                MapStream.Write(compressedTileData.Length);
                MapStream.Write(compressedTileData);
            }
        }

        public void Finish() => Write((MapStream.BaseStream as MemoryStream).ToArray());

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
    