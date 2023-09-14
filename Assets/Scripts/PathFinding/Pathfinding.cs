using System;
using System.Collections.Generic;
using UnityEngine;


namespace PathFinding
{
   public class Pathfinding : MonoBehaviour
   {
      private Grid grid;
      public Transform seeker, target;

      private void Update()
      {
         FindPath(seeker.position,target.position);
      }

      private void Awake()
      {
         grid = GetComponent<Grid>();
      }

      void FindPath(Vector3 startPos, Vector3 targetPos)
      {
         Node startNode = grid.NodeFromWorldPoint(startPos);
         Node targetNode = grid.NodeFromWorldPoint(targetPos);

         List<Node> openSet = new List<Node>();
         HashSet<Node> closedSet = new HashSet<Node>();
         openSet.Add(startNode);

         while (openSet.Count > 0)
         {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
               if (openSet[i].fCost < currentNode.fCost ||openSet[i].fCost==currentNode.fCost&& openSet[i].hCost<currentNode.hCost)
               {
                  currentNode = openSet[i];
               }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);
            if (currentNode == targetNode)
            {
               RetracePath(startNode,targetNode);
               return;
            }
            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
               if(!neighbour.Walkable||closedSet.Contains(neighbour))
                  continue;
               int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
               if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
               {
                  neighbour.gCost = newMovementCostToNeighbour;
                  neighbour.hCost = GetDistance(neighbour, targetNode);
                  neighbour.Parent = currentNode;
                  
                  if(!openSet.Contains(neighbour))
                     openSet.Add(neighbour);
               }
            }
         }
      }

      void RetracePath(Node startNode, Node endNode)
      {
         List<Node> path = new List<Node>();
         Node currentNode = endNode;
         
         while (currentNode!=startNode)
         {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
         }
         path.Reverse();
         grid.path = path;
      }

      int GetDistance(Node nodeA,Node nodeB)
      {
         int dstX = Mathf.Abs(nodeA.GridX - nodeB.GridX);
         int dstY = Mathf.Abs(nodeA.GridY - nodeB.GridY);

         if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
         return 14 * dstX + 10 * (dstY - dstX);
      }
   }
}
