// Copyright (c) Arctium Emulation.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Concurrent;

namespace DataExtractor.Maps.Defines
{
    public class Map
    {
        public MapTile this[ushort tileId]
        {
            get 
            { 
                MapTile tile;

                if (Tiles.TryGetValue(tileId, out tile))
                    return tile;

                return null;
            }
        }

        public uint CompressedSize { get; set; }
        public uint UnCompressedSize { get; set; }

        public ushort Id   { get; set; }
        public string Name { get; set; }

        public ConcurrentDictionary<ushort, MapTile> Tiles { get; set; }
    }
}
