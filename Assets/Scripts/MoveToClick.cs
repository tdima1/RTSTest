using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToClick : MonoBehaviour
{
   public Camera mainCamera;
   public LayerMask groundLayer;
   public NavMeshAgent player;

   private List<Vector3Int> _waypoints = new List<Vector3Int>();
   private int destPointIndex = 0;

   private Pathfinding _pathfinding;
   private RaycastHit hitInfo;

   void Awake()
   {
      mainCamera = Camera.main;
      _pathfinding = GetComponent<Pathfinding>();
   }

   // Update is called once per frame
   void Update()
   {
      WalkToPointOnClick();
   }

   private void WalkToPointOnClick()
   {
      if(Input.GetMouseButtonUp(0)) {
         var destination = GetDestinationPoint();
         print(destination);
         //Generate proximity matrix...
         _pathfinding.GenerateProximityMatrix(player.transform.position, destination);
         //Check for path in matrix...

         //Move through path...




         //Pathfinding -> returns list of points....
         //var positionAsVector3Int = new Vector3Int((int)player.transform.position.x, 0, (int)player.transform.position.z);
         //_waypoints = _grid.BreadthFirstSearch(positionAsVector3Int, destination);
         //destPointIndex = 0;
      }
      if(!player.pathPending && player.remainingDistance < 0.001f && destPointIndex < _waypoints.Count) {
         GotoNextPoint();
      }
   }

   void GotoNextPoint()
   {
      // Returns if no points have been set up
      if(_waypoints.Count == 0)
         return;

      // Set the agent to go to the currently selected destination.
      player.destination = _waypoints[destPointIndex];

      // Choose the next point in the array as the destination,
      // cycling to the start if necessary.
      destPointIndex = (destPointIndex + 1);
   }

   //private IEnumerator MoveBean(Vector3 destination)
   //{
   //   while (player.transform.position.x != destination.x || player.transform.position.z != destination.z) {
   //      isMoving = true;
   //      float moveX, moveZ;

   //      if (destination.x - player.transform.position.x != 0) {
   //         moveX = Mathf.Sign(destination.x - player.transform.position.x);
   //      } else {
   //         moveX = 0;
   //      }

   //      if (destination.z - player.transform.position.z != 0) {
   //         moveZ = Mathf.Sign(destination.z - player.transform.position.z);
   //      } else {
   //         moveZ = 0;
   //      }

   //      print($"X: {moveX}, Z: {moveZ}");

   //      var nextPosition = player.transform.position;
   //      nextPosition = new Vector3(
   //         nextPosition.x + moveX,
   //         0,
   //         nextPosition.z + moveZ
   //         );

   //      player.transform.position = nextPosition;
   //      yield return new WaitForSeconds(0.25f);
   //   }

   //   isMoving = false;
   //}

   private Vector2Int GetPositionInGrid(Vector3 point)
   {
      int gridPosX = Mathf.RoundToInt(point.x / _pathfinding.cellSize);
      int gridPosZ = Mathf.RoundToInt(point.z / _pathfinding.cellSize);
      return new Vector2Int(gridPosX, gridPosZ);
   }

   private Vector3Int GetDestinationPoint()
   {
      Vector3 screenPosition = Input.mousePosition;
      var mouseWorldPosition = mainCamera.ScreenPointToRay(screenPosition);

      Physics.Raycast(mouseWorldPosition, out hitInfo, 100, groundLayer);
      var positionInGrid = GetPositionInGrid(hitInfo.point) * _pathfinding.cellSize;

      hitInfo.point = new Vector3Int(Mathf.FloorToInt(
         positionInGrid.x),
         0,
         positionInGrid.y);

      return new Vector3Int(Mathf.FloorToInt(
         positionInGrid.x),
         0,
         positionInGrid.y);
   }
}
