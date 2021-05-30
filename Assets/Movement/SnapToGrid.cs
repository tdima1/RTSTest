using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SnapToGrid : MonoBehaviour
{
   [SerializeField]
   private Pathfinding _pathfinding;
   [SerializeField]
   private float CellSize = 1;

   // Update is called once per frame
   void Update()
   {
      SnapObjectToGrid();
   }

   private void SnapObjectToGrid()
   {
      int gridPosX = Mathf.RoundToInt(transform.position.x / CellSize);
      int gridPosY = Mathf.RoundToInt(transform.position.y / CellSize);
      int gridPosZ = Mathf.RoundToInt(transform.position.z / CellSize);
      var positionInGrid = new Vector3Int(gridPosX, gridPosY, gridPosZ);

      transform.position = positionInGrid;
   }
}
