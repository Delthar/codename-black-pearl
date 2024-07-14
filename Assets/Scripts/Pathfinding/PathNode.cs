using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour
{
    [SerializeField] private List<PathNode> neighbor;

    private PathNode connection;
    private float g; // The G Cost, from the Start to the Target
    private float h; // The Heuristic Cost, from the Target to the Start
    private float f => g + h; // The combined F Cost
    private Vector2 position;

    #region Setters

    public void SetG(float g) => this.g = g;

    public void SetH(float h) => this.h = h;

    public void SetConnection(PathNode pathNode) => connection = pathNode;

    #endregion

    #region Getters

    public float GetG() => g;

    public float GetH() => h;

    public float GetF() => f;

    public PathNode GetConnection() => connection;

    public List<PathNode> GetNeighbors() => neighbor;

    public Vector2 GetPosition() => position;

    public float GetDistance(PathNode node) => 
        Mathf.Sqrt(Mathf.Pow(GetPosition().x - node.GetPosition().x, 2) + Mathf.Pow(GetPosition().y - node.GetPosition().y, 2));
   
    #endregion

    private void Awake() => position = transform.position;

    #region Debug
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.25f);
        if(!Application.isPlaying)
        {
            if(neighbor != null)
            {
                foreach(PathNode pathNode in neighbor)
                {
                    Gizmos.DrawLine(transform.position, pathNode.transform.position);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.075f);
        Gizmos.color = Color.red;
        if (neighbor != null)
        {
            foreach (PathNode pathNode in neighbor)
            {
                Gizmos.DrawSphere(pathNode.transform.position, 0.075f);
            }
        }
    }
    #endregion
}
