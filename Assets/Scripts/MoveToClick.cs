using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToClick : MonoBehaviour
{
   public Camera mainCamera;
   public LayerMask groundLayer;
   public Transform player;

   private Pathfinding _pathfinding;
   private bool isMoving = false;

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
      if(Input.GetMouseButtonDown(0) && !isMoving) {

         var destination = _pathfinding.GetDestinationPoint();

         if(Vector3.Distance(Vector3Int.FloorToInt(player.transform.position), destination) <= 2 * _pathfinding.MaxProximityOfDestination &&
            Mathf.Abs(Vector3Int.FloorToInt(player.transform.position).x - destination.x) < _pathfinding.MaxProximityOfDestination &&
            Mathf.Abs(Vector3Int.FloorToInt(player.transform.position).z - destination.z) < _pathfinding.MaxProximityOfDestination) {

            isMoving = true;

            //Generate proximity matrix...
            _pathfinding.GenerateProximityMatrix(Vector3Int.FloorToInt(player.transform.position), Vector3Int.FloorToInt(destination));

            //Check for path in matrix...
            var path = _pathfinding.AStar(destination);

            //Move through path...
            StartCoroutine("MoveThroughPath", path);

         } else {
            print("Distance too long...");
         }

      }
   }

   private IEnumerator MoveThroughPath(List<GridCell> path)
   {
      var snapToGrid = player.GetComponent<SnapToGrid>();
      snapToGrid.enabled = false;

      Vector3 movement = Vector3.zero;

      foreach(var cell in path) {

         while ((cell.worldPosition - player.position).magnitude > 0.1f) {

            if (Mathf.Approximately(cell.worldPosition.x, player.position.x)) {
               movement.z = Mathf.Sign(cell.worldPosition.z - player.position.z);
            }

            if(Mathf.Approximately(cell.worldPosition.z, player.position.z)) {
               movement.x = Mathf.Sign(cell.worldPosition.x - player.position.x);
            }

            player.transform.Translate(movement * 3 * Time.deltaTime);

            yield return new WaitForFixedUpdate();
         }


         //player.transform.position = Vector3.MoveTowards(player.transform.position, cell.worldPosition, 500 * Time.deltaTime);
      }

      snapToGrid.enabled = true;
      isMoving = false;
   }
}
