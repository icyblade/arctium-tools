// Copyright (c) Arctium Emulation.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Concurrent;

namespace DataExtractor.Maps.Defines
{
    public class APAK
    {
        public Map this[ushort mapId]
        {
            get 
            { 
                Map map;

                if (Maps.TryGetValue(mapId, out map))
                    return map;

                return null;
            }
        }

        public string Magic { get; set; } // KAPA
        public byte Version { get; set; } // 1

        public ushort MapCount { get; set; }
        public ConcurrentDictionary<ushort, Map> Maps  { get; set; }
    }
}
    