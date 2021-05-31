using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SnapToGrid : MonoBehaviour
{
   [SerializeField]
   private float CellSize = 1;

   void Update()
   {
      SnapObjectToGrid();
   }

   private void SnapObjectToGrid()
   {
      int gridPosX = Mathf.RoundToInt(transform.position.x / CellSize);
      float gridPosY = transform.position.y;
      int gridPosZ = Mathf.RoundToInt(transform.position.z / CellSize);
      var positionInGrid = new Vector3(gridPosX, gridPosY, gridPosZ);

      transform.position = positionInGrid;
   }
}
