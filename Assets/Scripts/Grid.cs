using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//[ExecuteInEditMode]
//[SelectionBase]
public class Grid : MonoBehaviour
{
   public Camera mainCamera;
   public int cellSize = 1;
   public int gridSize = 10;

   public GameObject cellPrefab;

   private Dictionary<Vector3Int, Vector3Int> _visitedPoints;

   Vector2Int[] _directions = {
      Vector2Int.up,
      Vector2Int.right,
      Vector2Int.down,
      Vector2Int.left
   };

   void Awake()
   {
      _visitedPoints = new Dictionary<Vector3Int, Vector3Int>();

      mainCamera = Camera.main;
      for (int i = 0; i < gridSize; i++) {
         for (int j = 0; j < gridSize; j++) {
            cellPrefab.transform.position = new Vector3(i, 0.001f, j);
            Instantiate(cellPrefab, GameObject.FindGameObjectWithTag("Grid").transform);
         }
      }
   }

   // Update is called once per frame
   void Update()
   {
      
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
