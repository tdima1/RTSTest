using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;

//[ExecuteInEditMode]
//[SelectionBase]
public class Pathfinding : MonoBehaviour
{
   public Camera mainCamera;
   public int cellSize = 1;
   public int gridSize = 10;
   public LayerMask groundLayer;
   public LayerMask obstaclesLayer;

   public GameObject cellPrefab;


   public GridCell[,] movementRangeMatrix;


   private Dictionary<Vector3Int, Vector3Int> _visitedPoints;

   Vector2Int[] _directions;
   private int _maxProximityOfDestination = 10;

   void Awake()
   {
      _directions = new Vector2Int[]{
         Vector2Int.up * cellSize,
         Vector2Int.right * cellSize,
         Vector2Int.down * cellSize,
         Vector2Int.left * cellSize
      };

      _visitedPoints = new Dictionary<Vector3Int, Vector3Int>();

      mainCamera = Camera.main;
      //for (int i = 0; i < gridSize; i++) {
      //   for (int j = 0; j < gridSize; j++) {
      //      cellPrefab.transform.position = new Vector3(i, 0.001f, j);
      //      Instantiate(cellPrefab, GameObject.FindGameObjectWithTag("Grid").transform);
      //   }
      //}
   }

   public void GenerateProximityMatrix(Vector3 playerPosition, Vector3Int destination)
   {
      movementRangeMatrix = new GridCell[2 * _maxProximityOfDestination + 1, 2 * _maxProximityOfDestination + 1];

      if (Vector3.Distance(playerPosition, destination) > _maxProximityOfDestination) {
         return;
      }

      Vector2Int playerPlaneCoords = new Vector2Int((int)playerPosition.x, (int)playerPosition.z);

      for(int i = -_maxProximityOfDestination; i <= _maxProximityOfDestination; i++) {
         for(int j = -_maxProximityOfDestination; j <= _maxProximityOfDestination; j++) {

            Vector3Int positionToSample = new Vector3Int(playerPlaneCoords.x + i * cellSize, 100, playerPlaneCoords.y + j * cellSize);

            var a = Physics.Raycast(positionToSample, Vector3.down, out RaycastHit rayHitInfo, groundLayer | obstaclesLayer);
            bool inRangeOfObstacle = Physics.CheckSphere(rayHitInfo.point + 1.4f * Vector3.up, cellSize * 0.6f, obstaclesLayer);

            if(!inRangeOfObstacle) {
               Vector3 hitInfoIntPosition = new Vector3(Mathf.RoundToInt(rayHitInfo.point.x), rayHitInfo.point.y + 0.01f, Mathf.RoundToInt(rayHitInfo.point.z));

               var cell = new GridCell(hitInfoIntPosition, hitInfoIntPosition.y, !inRangeOfObstacle);
               movementRangeMatrix[i + _maxProximityOfDestination, j + _maxProximityOfDestination] = cell;

               cellPrefab.transform.position = cell.worldPosition;
               Instantiate(cellPrefab, GameObject.FindGameObjectWithTag("Grid").transform);
            }
         }
      }
   }





   public List<Vector3Int> BreadthFirstSearch(Vector3Int start, Vector3Int destination)
   {
      _visitedPoints = new Dictionary<Vector3Int, Vector3Int>();

      if(start.x.Equals(destination.x) && start.z.Equals(destination.z)) {
         return new List<Vector3Int>();
      }

      Queue<Vector3Int> queue = new Queue<Vector3Int>();
      queue.Enqueue(start);
      
      while (queue.Count > 0) {
         Vector3Int currentPoint = queue.Dequeue();
         
         if(currentPoint.x.Equals(destination.x) && currentPoint.z.Equals(destination.z)) {
            return BacktrackPath(start, destination);
         } else {
            //_visitedPoints.Add(currentPoint, true);
            ExploreNeighbours(currentPoint, queue);
         }
      }

      //No Path found
      return new List<Vector3Int>();
   }

   private void ExploreNeighbours(Vector3Int currentPoint, Queue<Vector3Int> queue)
   {
      foreach (var direction in _directions) {
         var neighbour = new Vector3Int(currentPoint.x + direction.x, 0, currentPoint.z + direction.y);

         var isPartofNavMesh = NavMesh.SamplePosition(neighbour, out NavMeshHit hitInfo, cellSize, NavMesh.GetAreaFromName("Not Walkable"));

         if(!_visitedPoints.ContainsKey(neighbour)) {
            _visitedPoints.Add(neighbour, currentPoint);
            queue.Enqueue(neighbour);
         }
      }
   }

   private List<Vector3Int> BacktrackPath(Vector3Int start, Vector3Int destination)
   {
      var tracker = destination;

      List<Vector3Int> path = new List<Vector3Int>();

      while(!tracker.Equals(start)) {
         path.Add(tracker);
         tracker = _visitedPoints[tracker];
      }
      path.Add(start);
      path.Reverse();

      return path;
   }

}
