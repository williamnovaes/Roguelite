using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProceduralTileGenerator : MonoBehaviour {
    public TileBase groundTiles, WallsTiles;
    public Tilemap groundTilemap, wallTilemap;

    void Start() {
        int [,] map = GenerateArray(11, 20, true);
        RenderMap(map, wallTilemap, WallsTiles);
        map = GenerateArray(11, 20, false);
        RenderMap(map, groundTilemap, groundTiles);


        wallTilemap.GetTile(new Vector3Int(0, 0, 0));
    }

    public int[,] GenerateArray(int width, int height, bool isBorder) {
        int[,] map = new int[width, height];

        for (int x = 0; x <= map.GetUpperBound(0); x++) {
            for (int y = 0; y <= map.GetUpperBound(1); y++) {
                if (isBorder) {
                    if ((y == 0) || (y == height -1)
                        || (y != 0 && (x == 0 || x == width -1))) {
                        map[x, y] = 1;
                    } else {
                        map[x, y] = 0;
                    }
                } else {
                    if (y != 0 && y != height - 1
                        && x != 0 && x != width -1) {
                        map[x, y] = 1;
                    } else {
                        map[x, y] = 0;
                    }
                }
            }
        }
        return map;
    }

    public void RenderMap(int[,] map, Tilemap tilemap, TileBase tilebase) {
        //Clear the map (ensure we dont overlap)
        tilemap.ClearAllTiles();

        //loop through the width of the map
        for (int x = 0; x <= map.GetUpperBound(0); x++) {
            //loop through the height of the map
            for (int y = 0; y <= map.GetUpperBound(1); y++) {
                if (map[x, y] == 1) {
                    tilemap.SetTile(new Vector3Int(x, y, 0), tilebase);
                }
            }
        }
    }
}
