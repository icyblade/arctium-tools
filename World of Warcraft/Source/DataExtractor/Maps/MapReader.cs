// Copyright (c) Arctium Emulation.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using DataExtractor.Maps.Defines;

namespace DataExtractor
{
    class MapReader
    {
        BinaryReader streamReader;
        byte[] fileData;

        public void Initialize(byte[] mapData)
        {
            fileData = mapData;

            streamReader = new BinaryReader(new MemoryStream(fileData));
        }

        public void Read(Map map, int x, int y)
        {
            var tileId = (ushort)(64 * x + y);

            map.Tiles = new ConcurrentDictionary<ushort, MapTile>();
            map.Tiles.TryAdd(tileId, new MapTile 
            {
                Id = tileId,
                IndexX = (byte)x,
                IndexY = (byte)y,
                Chunks = new List<MapChunk>()
            });

            var offset = Helper.SearchOffset(fileData, streamReader.BaseStream.Position, new byte[] { 0x4B, 0x4E, 0x43, 0x4D });

            while (offset != 0)
            {
                streamReader.BaseStream.Position = offset + 12;

                var chunk = new MapChunk();

                chunk.IndexX = (byte)streamReader.ReadUInt32();
                chunk.IndexY = (byte)streamReader.ReadUInt32();

                streamReader.BaseStream.Position += 40;

                chunk.Area = (ushort)streamReader.ReadUInt32();

                streamReader.BaseStream.Position += 72;

                if (map.Tiles.ContainsKey(tileId))
                    map.Tiles[tileId].Chunks.Add(chunk);

                offset = Helper.SearchOffset(fileData, streamReader.BaseStream.Position, new byte[] { 0x4B, 0x4E, 0x43, 0x4D });
            }
        }
    }
}
