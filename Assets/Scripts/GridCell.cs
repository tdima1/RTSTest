using UnityEngine;

public class GridCell
{
   public Vector3 worldPosition;
   public float height { get; set; }
   public bool isVisited { get; set; }
   public bool isWalkable { get; set; }

   public Vector3 previousPoint { get; set; }

   public GridCell(Vector3 worldPosition, float height, bool isWalkable, bool isVisited = false)
   {
      this.worldPosition = worldPosition;
      this.height = height;
      this.isWalkable = isWalkable;
      this.isVisited = isVisited;
   }

}