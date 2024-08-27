using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private PathNode start;
    [SerializeField] private PathNode end;
    private Pathfinding pathfinding;
    List<PathNode> path = new List<PathNode>();

    void Awake()
    {
        pathfinding = new Pathfinding();
    }

    void Start()
    {
        path = pathfinding.FindPath(start, end);
        Debug.Log(path.Count);
    }

    void OnDrawGizmos()
    {
        int pathCount = path.Count;
        for(int i = 0; i < pathCount; i++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(path[i].transform.position, 0.5f);
            if(i < (pathCount - 1))
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(path[i].transform.position, path[i + 1].transform.position);
            }
        }
    }
}
