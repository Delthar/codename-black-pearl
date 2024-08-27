using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathNetwork : MonoBehaviour
{
    public static PathNetwork Instance;

    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private int nodesOffset = 10;
    [SerializeField] private List<PathNode> nodesList;
    [SerializeField] private float nodesSearchRadius = 5;
    [SerializeField] private float terrainAvoidanceRadius = 0.25f;
    [SerializeField] private LayerMask raycastLayer = new LayerMask();
    [SerializeField] private LayerMask raycastLayerTerrain = new LayerMask();

    private CircleCollider2D temporaryConnectionCircleCollider;

    private void Awake()
    {
        Instance = this;
    }

    public void GenerateNodes()
    {
        nodesList = new List<PathNode>();
        while(transform.childCount > 0)
        {
            GameObject child = transform.GetChild(0).gameObject;
            DestroyImmediate(child);
        }
        var pathNodeLayerMaskIndex = LayerMask.NameToLayer("Path Node");
        List<Vector2Int> nodes = mapGenerator.GetWaterTilePositions();
        foreach(Vector2Int node in nodes.Where((_, i) => i % nodesOffset == 0).ToList())
        {
            GameObject instance = Instantiate(nodePrefab, new Vector3(node.x + 0.5f, node.y + 0.5f, 0), Quaternion.identity, transform);
            nodesList.Add(instance.GetComponent<PathNode>());
            temporaryConnectionCircleCollider = instance.AddComponent<CircleCollider2D>();
            temporaryConnectionCircleCollider.radius = 0.1f;
        }

        foreach(var node in nodesList)
        {
            temporaryConnectionCircleCollider.enabled = false;
            var cols = Physics2D.OverlapCircleAll(node.transform.position, nodesSearchRadius, raycastLayer);
            foreach(var col in cols) 
            {
                // Debug.Log(col.gameObject.name);
                Vector2 dir = (col.transform.position - node.transform.position).normalized;
                Debug.DrawRay(node.transform.position, dir, Color.yellow, 5);
                RaycastHit2D hit = Physics2D.Raycast(node.transform.position, dir, nodesSearchRadius);
                // Debug.DrawLine(node.transform.position, hit.point, Color.green, 10);
                // // Debug.Log($"A: {hit.transform.gameObject.layer} | B: {pathNodeLayerMaskIndex}");
                // TilemapCollider2D tilemapCollider = hit.collider.GetComponent<TilemapCollider2D>();
                if(hit)
                {
                    var coll = Physics2D.CircleCast(node.transform.position, terrainAvoidanceRadius, dir, nodesSearchRadius, raycastLayerTerrain);
                    if(coll)
                    {
                        Debug.DrawLine(node.transform.position, coll.point, Color.green, 5);
                        continue;
                    }
                }
                if(hit && col.TryGetComponent(out PathNode pathNode) && !node.GetNeighbors().Contains(pathNode))
                {
                    node.GetNeighbors().Add(pathNode);
                }
                
            }
            temporaryConnectionCircleCollider.enabled = true;
            // break;
        }


        foreach(var node in nodesList)
        {
            DestroyImmediate(node.transform.GetComponent<CircleCollider2D>());
        }
    }

    public PathNode FindClosestNodeFromPosition(Vector2 position)
    {
        PathNode pathNode = null;
        float currentDistance = float.MaxValue;
        foreach(PathNode path in nodesList)
        {
            float distance = Vector2.Distance(path.transform.position, position);
            if(distance < currentDistance)
            {
                currentDistance = distance;
                pathNode = path;
            }
        }
        return pathNode;
    }

    public List<PathNode> GetPathNodes() => nodesList;
    public int GetNodesCount() => nodesList.Count;

}
