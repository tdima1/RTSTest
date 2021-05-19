using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SnapToGrid : MonoBehaviour
{
   [SerializeField]
   private Pathfinding _pathfinding;

   [SerializeField]
   private LayerMask _obstacleLayer;
   [SerializeField]
   private LayerMask _groundLayer;

   // Update is called once per frame
   void Update()
   {
      SnapObjectToGrid();
   }

   private void SnapObjectToGrid()
   {
      var positionInGrid = _pathfinding.GetPositionInGrid(transform.position);

      transform.position = positionInGrid;
   }
}
