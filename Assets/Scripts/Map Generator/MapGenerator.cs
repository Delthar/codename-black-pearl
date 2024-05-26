using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MapGenerator : MonoBehaviour
{
    public RuleTile ruleTile;
    public TileBase waterTile;
    public Tilemap terrainTileMap;
    public Tilemap waterTileMap;

    public int mapWidth = 500;
    public int mapHeight = 500;
    public float randomScale = 1;

    [Range(0f, 100f)]
    public float scale = 1.0F;

    [Range(0f, 10f)]
    public float modulo = 1f;
    [Range(0f, 1f)]
    public float clamp = 0.3f;

    public void GenerateMap()
    {
        ClearMap();

        float randomX = Random.Range(-9999, 9999);
        float randomY = Random.Range(-9999, 9999);
        float scale = this.scale + Random.Range(-randomScale, randomScale);
        Vector3Int offset = new Vector3Int((int)(mapWidth * .5f), (int)(mapHeight * .5f));

        for (float y = 0f; y < mapHeight; y++)
        {
            for (float x = 0f; x < mapWidth; x++)
            {
                float xCoord = randomX + x * scale;
                float yCoord = randomY + y * scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord) * modulo;
                if(sample > clamp)
                {
                    terrainTileMap.SetTile(new Vector3Int((int)x, (int)y) - offset, ruleTile);
                }
                waterTileMap.SetTile(new Vector3Int((int)x, (int)y) - offset, waterTile);
            }
        }
    }

    private void ClearMap()
    {
        terrainTileMap.ClearAllTiles();
        waterTileMap.ClearAllTiles();
    }
}
