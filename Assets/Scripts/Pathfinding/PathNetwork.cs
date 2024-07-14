using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathNetwork : MonoBehaviour
{
    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private Transform pathPrefab;
    [SerializeField] private int nodesOffset = 10;
    [SerializeField] private List<Transform> nodesList;

    public void GenerateNodes()
    {
        nodesList = new List<Transform>();
        while(transform.childCount > 0)
        {
            GameObject child = transform.GetChild(0).gameObject;
            DestroyImmediate(child);
        }
        List<Vector2Int> nodes = mapGenerator.GetWaterTilePositions();
        foreach(Vector2Int node in nodes.Where((_, i) => i % nodesOffset == 0).ToList())
        {
            Transform instance = Instantiate(pathPrefab, new Vector3(node.x + 0.5f, node.y + 0.5f, 0), Quaternion.identity, transform);
            nodesList.Add(instance);
        }
    }
}
