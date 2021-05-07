using UnityEngine;

internal class GridCell
{
   public Vector3 position { get; set; }
   public Vector3 previousPoint { get; set; }

   public bool isVisited { get; set; }

   public GridCell(Vector3 position, Vector3 previousPoint)
   {
      this.position = position;
      this.previousPoint = previousPoint;
      this.isVisited = false;
   }

   public override bool Equals(object obj)
   {
      return this.position.Equals((obj as GridCell).position);
   }
}