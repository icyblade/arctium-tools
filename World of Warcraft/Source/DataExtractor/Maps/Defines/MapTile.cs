// Copyright (c) Arctium Emulation.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace DataExtractor.Maps.Defines
{
    public class MapTile
    {
        public ushort Id   { get; set; } // 64 * X + Y
        public byte IndexX { get; set; }
        public byte IndexY { get; set; }

        public List<MapChunk> Chunks { get; set; }
    }
}

