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
      if(Input.GetMouseButtonUp(0)) {
         var destination = _pathfinding.GetDestinationPoint();

         if(Vector3.Distance(Vector3Int.FloorToInt(player.transform.position), destination) <= 2 * _pathfinding.MaxProximityOfDestination &&
            Mathf.Abs(Vector3Int.FloorToInt(player.transform.position).x - destination.x) < _pathfinding.MaxProximityOfDestination &&
            Mathf.Abs(Vector3Int.FloorToInt(player.transform.position).z - destination.z) < _pathfinding.MaxProximityOfDestination) {

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
      foreach(var cell in path) {

         player.transform.position = Vector3.MoveTowards(player.transform.position, cell.worldPosition, 500 * Time.deltaTime);

         yield return new WaitForSeconds(0.25f);
      }
   }
}
