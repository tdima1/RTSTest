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

      //bool isStandingOnObstacle = Physics.CheckCapsule(positionInGrid, positionInGrid - Vector3.up * 0.5f, _pathfinding.CellSize * 0.1f, _obstacleLayer);

      //bool isCloseToObstacle = Physics.CheckCapsule(positionInGrid, positionInGrid + Vector3.up, _pathfinding.CellSize, _obstacleLayer);

      //if(isStandingOnObstacle) {
      //   bool hitObstacle = Physics.Raycast(positionInGrid + Vector3.up * 100f, Vector3.down, out RaycastHit obstacleRayHitInfo, maxDistance: 100f, layerMask: _obstacleLayer);

      //   if(hitObstacle) {
      //      positionInGrid.y = (int)obstacleRayHitInfo.point.y;
      //   }

      //} else if (isCloseToObstacle){
      //   bool hitObstacle = Physics.Raycast(positionInGrid + Vector3.up * 100f, Vector3.down, out RaycastHit obstacleRayHitInfo, maxDistance: 100f, layerMask: _obstacleLayer);

      //   if(hitObstacle) {
      //      positionInGrid.y = (int)obstacleRayHitInfo.point.y;
      //   }
      //} else {
      //   positionInGrid.y = 0;
      //}


      ////if(isCloseToObstacle) {
      ////   bool hitObstacle = Physics.Raycast(positionInGrid + Vector3.up * 100f, Vector3.down, out RaycastHit obstacleRayHitInfo, maxDistance: 100f, layerMask: _obstacleLayer); 

      ////   if(hitObstacle) {
      ////      positionInGrid.y = (int)obstacleRayHitInfo.point.y;
      ////   } else {
      ////      positionInGrid.y = 0;
      ////   }
      ////} else {
      ////   positionInGrid.y = 0;
      ////}

      transform.position = positionInGrid;
   }
}
