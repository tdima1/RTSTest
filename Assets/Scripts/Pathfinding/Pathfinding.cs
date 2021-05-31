using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Grid;
using Assets.Scripts.Grid.Constants;

//[ExecuteInEditMode]
//[SelectionBase]
public class Pathfinding : MonoBehaviour
{
   public int CellSize = 1;
   public int MaxProximityOfDestination = 10;
   public LayerMask GroundLayer;
   public LayerMask ObstaclesLayer;
   public LayerMask TreeLayer;
   public bool UseDiagonals = true;

   public Grid<GridCell> Grid { get; set; }

   [SerializeField]
   private GameObject cellPrefab;
   [SerializeField]
   private GameObject pathCellPrefab;

   private Vector2Int[] _directions;

   private Vector2Int[] _directionsWithDiagonals;
   private Vector2Int[] _directionsNoDiagonals;
   private int GridSize;


   private List<GameObject> worldCells;

   void Awake()
   {
      _directionsWithDiagonals = new Vector2Int[]{
         Vector2Int.up * CellSize,
         (Vector2Int.up + Vector2Int.right) * CellSize,
         (Vector2Int.up + Vector2Int.left) * CellSize,
         Vector2Int.down * CellSize,
         (Vector2Int.down + Vector2Int.right) * CellSize,
         (Vector2Int.down + Vector2Int.left) * CellSize,
         Vector2Int.right * CellSize,
         Vector2Int.left * CellSize,
      };

      _directionsNoDiagonals = new Vector2Int[]{
         Vector2Int.up * CellSize,
         Vector2Int.down * CellSize,
         Vector2Int.right * CellSize,
         Vector2Int.left * CellSize,
      };

      worldCells = new List<GameObject>();
   }

   public void GenerateProximityMatrix(Vector3Int unitPosition)
   {
      foreach (var cell in worldCells) {
         Destroy(cell);
      }

      GridSize = 2 * MaxProximityOfDestination + 1;

      Grid = new Grid<GridCell>(GridSize, CellSize);

      BuildProximityMatrix(unitPosition);
   }

   public List<GridCell> AStar(Vector3Int destination)
   {
      GridCell startCell = Grid.GetElementAtGridPosition(MaxProximityOfDestination, MaxProximityOfDestination);

      int destinationGridPositionX = startCell.GridPosition.x - (Mathf.RoundToInt(startCell.worldPosition.x) - destination.x);
      int destinationGridPositionY = startCell.GridPosition.y - (Mathf.RoundToInt(startCell.worldPosition.z) - destination.z);

      GridCell destinationCell = Grid.GetElementAtGridPosition(destinationGridPositionX, destinationGridPositionY);

      if(destinationCell == null) {
         print("DESTINATION OUTSIDE RANGE ...");
         return new List<GridCell>();
      }

      List<GridCell> openSet = new List<GridCell> {
         startCell,
      };
      List<GridCell> closedSet = new List<GridCell>();

      if(UseDiagonals) {
         _directions = _directionsWithDiagonals;
      } else {
         _directions = _directionsNoDiagonals;
      }

      startCell.gCost = 0;
      startCell.hCost = CalculateDistanceCost(startCell, destinationCell);
      startCell.CalculateFCost();

      while(openSet.Count > 0) {
         GridCell currentCell = GetLowestFCostNode(openSet);
         if(currentCell.GridPosition == destinationCell.GridPosition) {
            return CalculatePath(destinationCell);
         }

         openSet.Remove(currentCell);
         closedSet.Add(currentCell);

         var neighbourCells = ExploreNeighbours(currentCell);

         foreach(var neighbourCell in neighbourCells) {
            if(neighbourCell != null) {

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

      print("NO PATH FOUND...");
      return new List<GridCell>();
   }

   private void BuildProximityMatrix(Vector3Int unitPosition)
   {
      Vector2Int unitPlanePosition = new Vector2Int(unitPosition.x, unitPosition.z);

      for(int i = 0; i <= GridSize / 2; i++) {
         for(int j = GridSize / 2 - i; j <= GridSize / 2 + i; j++) {

            SetGridCell(unitPlanePosition, i, j);
            SetGridCell(unitPlanePosition, GridSize - i - 1, j);
         }
      }
   }

   private void SetGridCell(Vector2Int unitCoords, int i, int j)
   {
      Vector3Int raySourcePosition = new Vector3Int(unitCoords.x + (i - MaxProximityOfDestination) * CellSize, 100, unitCoords.y + (j - MaxProximityOfDestination) * CellSize);

      var groundHit = Physics.Raycast(raySourcePosition, Vector3.down, out RaycastHit rayHitInfo, 120, GroundLayer | ObstaclesLayer);

      if(groundHit) {
         bool inRangeOfObstacle = Physics.CheckSphere(rayHitInfo.point + Vector3.up, CellSize * 0.7f, ObstaclesLayer | TreeLayer);

         Vector3 worldPosition = new Vector3(rayHitInfo.point.x, rayHitInfo.point.y + 0.01f, rayHitInfo.point.z);
         Vector2Int gridPosition = new Vector2Int(i, j);

         GridCell cell = BuildGridCell(inRangeOfObstacle, worldPosition, gridPosition);

         Grid.SetElementAtGridPosition(gridPosition.x, gridPosition.y, cell);

         GameObject worldCell = cellPrefab;
         worldCell.transform.position = cell.worldPosition;

         if(cell.IsWalkable) {
            //worldCells.Add(Instantiate(worldCell, GameObject.FindGameObjectWithTag("Grid").transform));
         }
      }
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

      foreach (var cell in result) {
         GameObject worldCell = pathCellPrefab;
         worldCell.transform.position = cell.worldPosition;

         if(cell.IsWalkable) {
            worldCells.Add(Instantiate(worldCell, GameObject.FindGameObjectWithTag("Grid").transform));
         }
      }

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
         GridPosition = gridPosition,
         gCost = int.MaxValue,
         previousCell = null,
      };
      cell.CalculateFCost();
      return cell;
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
         var neighbourCell = Grid.GetElementAtGridPosition(neighbourGridPosition.x, neighbourGridPosition.y);

         if(neighbourCell != null && IsValidNeighbour(currentCell, neighbourCell)) {
            result.Add(neighbourCell);
         }
      }

      return result;
   }

   private bool IsValidNeighbour(GridCell currentCell, GridCell neighbourCell)
   {
      bool result = false;
      if(neighbourCell.IsWalkable &&
         Mathf.Abs(currentCell.Height - neighbourCell.Height) < 1.5f) {
         result = true;
      }

      return result;
   }
}
