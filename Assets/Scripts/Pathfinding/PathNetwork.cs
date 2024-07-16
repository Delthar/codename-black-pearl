using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathNetwork : MonoBehaviour
{
    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private int nodesOffset = 10;
    [SerializeField] private List<PathNode> nodesList;
    [SerializeField] private float circleCastRadius = 5;
    [SerializeField] private LayerMask raycastLayer = new LayerMask();
    [SerializeField] private LayerMask raycastLayerTerrain = new LayerMask();

    private CircleCollider2D temporaryConnectionCircleCollider;

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
            var cols = Physics2D.OverlapCircleAll(node.transform.position, circleCastRadius, raycastLayer);
            foreach(var col in cols) 
            {
                // Debug.Log(col.gameObject.name);
                Vector2 dir = (col.transform.position - node.transform.position).normalized;
                Debug.DrawRay(node.transform.position, dir, Color.yellow, 10);
                RaycastHit2D hit = Physics2D.Raycast(node.transform.position, dir, circleCastRadius);
                // Debug.DrawLine(node.transform.position, hit.point, Color.green, 10);
                // // Debug.Log($"A: {hit.transform.gameObject.layer} | B: {pathNodeLayerMaskIndex}");
                // TilemapCollider2D tilemapCollider = hit.collider.GetComponent<TilemapCollider2D>();
                if(hit && col.TryGetComponent(out PathNode pathNode) && !node.GetNeighbors().Contains(pathNode))
                {
                    node.GetNeighbors().Add(pathNode);
                }
                
            }
            temporaryConnectionCircleCollider.enabled = true;
            // break;
        }
    }

    private void OnDrawGizmosSelected()
    {
        foreach(PathNode node in nodesList)
        {
            // Gizmos.DrawWireSphere(node.transform.position, circleCastRadius);
            // node.SetConnection()
        }
    }
}
