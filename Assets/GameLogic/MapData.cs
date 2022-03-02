using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SKCell;
public sealed class MapData : Singleton<MapData>
{
    public static readonly List<int[,]> mapTiles = new List<int[,]>()
    {
        new int[,]{{6,0,0},
                        {0,0,4}, 
                        {0,0,1}},
        new int[,]{{6,0,0},
                        {0,2,0},
                        {3,0,1}},

           new int[,]{{6,0,0},
                        {0,2,0},
                        {0,0,1}},
        new int[,]{{6,4,0},
                        {0,0,0},
                        {18,0,1}},

        new int[,]{{6,0,0},
                        {12,0,0},
                        {0,13,1}},
        new int[,]{{6,0,21},
                        {0,0,0},
                        {3,0,1}},

         new int[,]{{17,0,0,10,0},
                        {0,0,15,0,12},
                        {16,13,0,9,14},
                        {0,0,0,0,0},
                       {0,0,0,0,1}},
        new int[,]{{17,0,0,0,0},
                        {0,7,21,0,0},
                         {19,0,0,0,0},
                          {0,0,0,22,0},
                        {0,20,0,0,24}},
    };

    public static readonly int[] levelMoveLimit = new int[] { 7, 4, 5, 12 };
}
