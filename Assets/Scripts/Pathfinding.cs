using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using Assets.Scripts.Grid;
using Assets.Scripts.Grid.Constants;

//[ExecuteInEditMode]
//[SelectionBase]
public class Pathfinding : MonoBehaviour
{
   public Camera mainCamera;
   public int CellSize = 1;
   public int GridSize = 10;
   public LayerMask GroundLayer;
   public LayerMask ObstaclesLayer;

   public Grid<GridCell> Grid { get; set; }

   [SerializeField]
   private GameObject cellPrefab;

   private Dictionary<Vector3Int, Vector3Int> _visitedPoints;

   private Vector2Int[] _directions;
   private int _maxProximityOfDestination = 10;


   private List<GameObject> worldCells;

   void Awake()
   {
      _directions = new Vector2Int[]{
         Vector2Int.up * CellSize,
         (Vector2Int.up + Vector2Int.right) * CellSize,
         (Vector2Int.up + Vector2Int.left) * CellSize,
         Vector2Int.down * CellSize,
         (Vector2Int.down + Vector2Int.right) * CellSize,
         (Vector2Int.down + Vector2Int.left) * CellSize,
         Vector2Int.right * CellSize,
         Vector2Int.left * CellSize,
      };

      _visitedPoints = new Dictionary<Vector3Int, Vector3Int>();

      mainCamera = Camera.main;

      worldCells = new List<GameObject>();
   }

   public void GenerateProximityMatrix(Vector3Int playerPosition)
   {
      var destination = GetDestinationPoint();

      Grid = new Grid<GridCell>(2 * _maxProximityOfDestination + 1, CellSize);

      BuildProximityMatrix(playerPosition);

      AStar(new Vector3Int());
   }

   private void BuildProximityMatrix(Vector3Int playerPosition)
   {
      var destination = GetDestinationPoint();

      if(Vector3.Distance(playerPosition, destination) > _maxProximityOfDestination) {
         return;
      }
      Vector2Int playerPlaneCoords = new Vector2Int(playerPosition.x, playerPosition.z);

      for(int i = -_maxProximityOfDestination; i <= _maxProximityOfDestination; i++) {
         for(int j = -_maxProximityOfDestination; j <= _maxProximityOfDestination; j++) {

            Vector3Int raySourcePosition = new Vector3Int(playerPlaneCoords.x + i * CellSize, 100, playerPlaneCoords.y + j * CellSize);

            var groundHit = Physics.Raycast(raySourcePosition, Vector3.down, out RaycastHit rayHitInfo, 100, GroundLayer | ObstaclesLayer);

            if(groundHit) {
               bool inRangeOfObstacle = Physics.CheckSphere(rayHitInfo.point + Vector3.up, CellSize * 0.5f, ObstaclesLayer);

               Vector3 worldPosition = new Vector3(rayHitInfo.point.x, rayHitInfo.point.y + 0.01f, rayHitInfo.point.z);
               Vector2Int gridPosition = new Vector2Int(i + _maxProximityOfDestination, j + _maxProximityOfDestination);

               GridCell cell = BuildGridCell(inRangeOfObstacle, worldPosition, gridPosition);

               Grid.SetElementAtGridPosition(gridPosition.x, gridPosition.y, cell);


               GameObject worldCell = cellPrefab;
               worldCell.transform.position = cell.worldPosition;
               var x = worldCell.GetComponent<CellCustomization>();
               x.SetMaterialColor(Color.black);

               if(cell.IsWalkable) {
                  worldCells.Add(Instantiate(worldCell, GameObject.FindGameObjectWithTag("Grid").transform));
               }
            }
         }
      }
   }

   public List<GridCell> AStar(Vector3Int destination)
   {
      GridCell startCell = Grid.GetElementAtGridPosition(_maxProximityOfDestination, _maxProximityOfDestination);
      print($"Start cell: {startCell.worldPosition}");

      var destinationGridPosition = GetGridPosition(destination.x, destination.z);
      GridCell destinationCell = Grid.GetElementAtGridPosition(destinationGridPosition.x, destinationGridPosition.y);

      List<GridCell> openSet = new List<GridCell> {
         startCell,
      };
      List<GridCell> closedSet = new List<GridCell>();

      startCell.gCost = 0;
      startCell.hCost = CalculateDistanceCost(startCell, destinationCell);
      startCell.CalculateFCost();

      while (openSet.Count > 0) {
         GridCell currentCell = GetLowestFCostNode(openSet);
         if (currentCell == destinationCell) {
            return CalculatePath(destinationCell);
         }

         openSet.Remove(currentCell);
         closedSet.Add(currentCell);

         var neighbourCells = ExploreNeighbours(currentCell);

         foreach (var neighbourCell in neighbourCells) {
            if (neighbourCell != null) {

               if(closedSet.Contains(neighbourCell)) {
                  continue;
               }

               int tentativeGCost = currentCell.gCost + CalculateDistanceCost(currentCell, neighbourCell);
               if(tentativeGCost < neighbourCell.gCost) {
                  neighbourCell.previousCell = currentCell;
                  neighbourCell.gCost = tentativeGCost;
                  neighbourCell.hCost = CalculateDistanceCost(neighbourCell, destinationCell);
                  neighbourCell.CalculateFCost();

                  if(!openSet.Contains(neighbourCell)) {
                     openSet.Add(neighbourCell);
                  }
               }
            }
         }
      }

      return new List<GridCell>();


   }

   private List<GridCell> CalculatePath(GridCell destinationCell)
   {
      var result = new List<GridCell>();
      var currentCell = destinationCell;
      result.Add(currentCell);

      while (currentCell.previousCell != null) {
         result.Add(currentCell.previousCell);
         currentCell = currentCell.previousCell;
      }
      result.Reverse();

      return result;
   }

   private GridCell GetLowestFCostNode(List<GridCell> cellList)
   {
      GridCell min = cellList[0];
      for (int i = 1; i < cellList.Count; i++) {
         if (cellList[i].fCost < min.fCost) {
            min = cellList[i];
         }
      }

      return min;
   }

   private GridCell BuildGridCell(bool inRangeOfObstacle, Vector3 worldPosition, Vector2Int gridPosition)
   {
      var cell = new GridCell(worldPosition, worldPosition.y, !inRangeOfObstacle) {
         worldPosition = worldPosition,
         GridPosition = gridPosition,
         gCost = int.MaxValue,
         previousCell = null,
      };
      cell.CalculateFCost();
      return cell;
   }



   private Vector3Int GetWorldPosition(int x, int y, int z)
   {
      return new Vector3Int(x, y, z) * CellSize;
   }

   private Vector2Int GetGridPosition(int x, int y)
   {
      return new Vector2Int(x, y) / CellSize;
   }

   private int CalculateDistanceCost(GridCell current, GridCell next)
   {
      int xDistance = Mathf.Abs(current.GridPosition.x - next.GridPosition.x);
      int zDistance = Mathf.Abs(current.GridPosition.y - next.GridPosition.y);
      int remaining = Mathf.Abs(xDistance - zDistance);

      return Constants.DiagonalLineCost * Mathf.Min(xDistance, zDistance) + Constants.StraightLineCost * remaining;
   }
   private List<GridCell> ExploreNeighbours(GridCell currentCell)
   {
      var result = new List<GridCell>();
      foreach(var direction in _directions) {
         var neighbourGridPosition = currentCell.GridPosition + direction;

         result.Add(Grid.GetElementAtGridPosition(neighbourGridPosition.x, neighbourGridPosition.y));
      }

      return result;
   }

   public Vector3Int GetPositionInGrid(Vector3 point)
   {

      int gridPosX = Mathf.RoundToInt(point.x / CellSize);
      int gridPosY = Mathf.RoundToInt(point.y / CellSize);
      int gridPosZ = Mathf.RoundToInt(point.z / CellSize);
      return new Vector3Int(gridPosX, gridPosY, gridPosZ);
   }

   private Vector3Int GetDestinationPoint()
   {
      Vector3 screenPosition = Input.mousePosition;
      var mouseWorldPosition = mainCamera.ScreenPointToRay(screenPosition);

      Physics.Raycast(mouseWorldPosition, out RaycastHit hitInfo, GroundLayer);
      var positionInGrid = GetPositionInGrid(hitInfo.point) * CellSize;

      hitInfo.point = new Vector3Int(Mathf.FloorToInt(
         positionInGrid.x),
         positionInGrid.y,
         positionInGrid.y);

      print(hitInfo.point);

      return new Vector3Int(Mathf.FloorToInt(
         positionInGrid.x),
         0,
         positionInGrid.y);
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
            //ExploreNeighbours(currentPoint, queue);
         }
      }

      //No Path found
      return new List<Vector3Int>();
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
