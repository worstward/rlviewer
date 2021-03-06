﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.TileCreator.Abstract
{
    public interface ITileCreator
    {
         bool CheckTileConsistency(string filePath, int tileCount);
         string GetDirectoryName(string filePath);
         Tile[] GetTiles(string filePath, bool forceTileGeneration = false, bool allowScrolling = false, bool synthesis = false, int startingLine = 0);

         void ClearCancelledFileTiles(string path);
         float NormalizationFactor { get; }
         float MaxValue { get; }

    }
}
