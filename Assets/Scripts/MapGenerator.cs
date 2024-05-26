using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MapGenerator : MonoBehaviour
{
    public int mapWidth = 500;
    public int mapHeight = 500;

    public RuleTile ruleTile;
    public Tilemap terrainTileMap;

    [Range(0f, 100f)]
    public float scale = 1.0F;

    [Range(0f, 10f)]
    public float modulo = 1f;
    [Range(0f, 1f)]
    public float clamp = 0.3f;

    public int[,] mapData;

    public void CalcNoise()
    {
        mapData = new int[mapWidth, mapHeight];
        float randomX = Random.Range(0, 999);
        float randomY = Random.Range(0, 999);

        for (float y = 0f; y < mapHeight; y++)
        {
            for (float x = 0f; x < mapWidth; x++)
            {
                float xCoord = randomX + x / mapWidth * scale;
                float yCoord = randomY + y / mapHeight * scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord) * modulo;
                Debug.Log($"X: {xCoord} | Y: {yCoord} | Noise Sample: {sample}");
                mapData[(int)x, (int)y] = sample > clamp ? 1 : 0;
            }
        }
    }

    public void GenerateMap()
    {
        ClearMap();
        CalcNoise();
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                if(mapData[x, y] == 1)
                {
                    terrainTileMap.SetTile(new Vector3Int(x, y), ruleTile);
                }
            }
        }
    }

    private void ClearMap()
    {
        terrainTileMap.ClearAllTiles();
    }
}
