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
         //Generate proximity matrix...
         _pathfinding.GenerateProximityMatrix(Vector3Int.FloorToInt(player.transform.position), Vector3Int.FloorToInt(destination));
         //Check for path in matrix...

         //Move through path...

      }
   }
}
