using UnityEngine;

public class GridCell
{
   public Vector3 worldPosition;
   public Vector2Int GridPosition { get; set; }

   public float Height { get; set; }
   public bool IsVisited { get; set; }
   public bool IsWalkable { get; set; }


   public int gCost;
   public int hCost;
   public int fCost;

   public GridCell previousCell { get; set; }

   public GridCell(Vector3 worldPosition, float height, bool isWalkable, bool isVisited = false)
   {
      this.worldPosition = worldPosition;
      this.Height = height;
      this.IsWalkable = isWalkable;
      this.IsVisited = isVisited;
   }

   public void CalculateFCost()
   {
      fCost = gCost + hCost;
   }

}