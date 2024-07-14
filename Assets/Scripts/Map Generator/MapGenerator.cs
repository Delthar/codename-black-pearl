using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MapGenerator : MonoBehaviour
{
    public RuleTile terrainTile;
    public RuleTile wallsTile;
    public TileBase waterTile;
    public Tilemap wallsTileMap;
    public Tilemap terrainTileMap;
    public Tilemap waterTileMap;

    public int mapWidth = 500;
    public int mapHeight = 500;
    public float randomScale = 1;
    public float randomModulo = 0.05f;

    private List<Vector2Int> terrainTilesPos;
    private List<Vector2Int> waterTilesPos;

    [Range(0f, 100f)]
    public float scale = 1.0F;

    [Range(0f, 10f)]
    public float modulo = 1f;
    [Range(0f, 1f)]
    public float clamp = 0.3f;

    public void GenerateMap()
    {
        terrainTilesPos = new List<Vector2Int>();
        waterTilesPos = new List<Vector2Int>();
        ClearMap();
        float randomX = Random.Range(-99999, 99999);
        float randomY = Random.Range(-99999, 99999);
        float scale = this.scale + Random.Range(-randomScale, randomScale);
        float modulo = this.modulo + Random.Range(-randomModulo, randomModulo);
        Vector2Int offset = new Vector2Int((int)(mapWidth * .5f), (int)(mapHeight * .5f));

        for (float x = 0f; x < mapWidth; x++)
        {
            for (float y = 0f; y < mapHeight; y++)
            {
                float xCoord = randomX + x * scale;
                float yCoord = randomY + y * scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord) * modulo;
                Vector2Int position = new Vector2Int((int)x, (int)y) - offset;
                if(sample > clamp)
                {
                    terrainTileMap.SetTile((Vector3Int)position, terrainTile);
                    terrainTilesPos.Add(position);
                }
                else
                {
                    waterTilesPos.Add(position);
                }
                waterTileMap.SetTile((Vector3Int)position, waterTile);
            }
        }

        // for (int i = 0; i < 30; i++)
        // {
        //     Vector2Int startTile = terrainTilesPos[Random.Range(0, terrainTilesPos.Count())];
        //     int curDir = 0;
        //     int randTilesAmount = Random.Range(5, 20);
        //     int clampAmount = 0;
        //     for (int t = 0; t < randTilesAmount; t++)
        //     {
        //         if (clampAmount == 0)
        //         {
        //             curDir = Random.Range(0, 3);
        //         }

        //         if (curDir == 0)
        //             startTile.y++;
        //         else if (curDir == 1)
        //             startTile.x++;
        //         else if (curDir == 2)
        //             startTile.x--;

        //         clampAmount++;
        //         if (clampAmount == 3)
        //             clampAmount = 0;
        //         if (wallsTileMap.GetTile(startTile) != null)
        //             break;
        //         if(terrainTileMap.GetTile(startTile) != null)
        //         {
        //             wallsTileMap.SetTile(startTile, wallsTile);
        //         }
        //     }
        // }
    }

    private void ClearMap()
    {
        wallsTileMap.ClearAllTiles();
        terrainTileMap.ClearAllTiles();
        waterTileMap.ClearAllTiles();
    }

    public List<Vector2Int> GetWaterTilePositions()
    {
        return waterTilesPos;
    }
}
