using System.Collections.Generic;
using System.Linq;

public class Pathfinding
{
   public List<PathNode> FindPath(PathNode startNode, PathNode targetNode)
   {
        List<PathNode> openNodes = new List<PathNode>() { startNode };
        List<PathNode> closedNodes = new List<PathNode>();
        List<PathNode> path = new List<PathNode>();
        while(openNodes.Any())
        {
            // Search for a Node with the smallest F Cost
            PathNode preferredNode = openNodes[0];
            foreach(PathNode node in openNodes)
            {
                if (node.GetF() < preferredNode.GetF() || (node.GetF() == preferredNode.GetF() && node.GetH() < preferredNode.GetH()))
                    preferredNode = node;
            }

            // Add the Preferred Node to the Closed List
            closedNodes.Add(preferredNode);

            // Remove the Preferred Node from the Open List
            openNodes.Remove(preferredNode);

            // If the target Node has been reached, construct a Path and return it.
            if(preferredNode == targetNode)
            {
                PathNode currentNode = targetNode;
                while(currentNode != startNode)
                {
                    path.Add(currentNode);
                    currentNode = currentNode.GetConnection();
                }
                path.Reverse();
                return path;
            }

            // Search in all neighbours
            foreach(PathNode neighbour in preferredNode.GetNeighbors().Where(node => !closedNodes.Contains(node)))
            {
                // Checks if the neighbour node of the preferred Node is still in the Open List
                bool inSearch = openNodes.Contains(neighbour);

                // Get the travel cost to the Neighbour
                float costToNeighbor = preferredNode.GetG() + preferredNode.GetDistance(neighbour);

                // Is the Neighbour not in the Open List or is the Cost to the Neighbor smaller than the G cost?
                if(!inSearch || costToNeighbor < neighbour.GetG())
                {
                    neighbour.SetG(costToNeighbor);
                    neighbour.SetConnection(preferredNode);

                    if(!inSearch)
                    {
                        neighbour.SetH(neighbour.GetDistance(targetNode));
                        openNodes.Add(neighbour);
                    }
                }
            }
        }
        return path;
   }
}
