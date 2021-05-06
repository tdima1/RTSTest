using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToClick : MonoBehaviour
{
   public Camera mainCamera;
   public LayerMask groundLayer;
   public GameObject player;

   private Grid grid;
   private RaycastHit hitInfo;
   private bool isMoving = false;

   void Awake()
   {
      mainCamera = Camera.main;
      grid = GetComponent<Grid>();
   }

   // Update is called once per frame
   void Update()
   {
      if(Input.GetMouseButtonUp(0)) {
         var destination = GetDestinationPoint();
         print(destination);

         if(!isMoving) {
            StartCoroutine(MoveBean(destination));
         }
      }
   }

   private IEnumerator MoveBean(Vector3 destination)
   {
      while (player.transform.position.x != destination.x || player.transform.position.z != destination.z) {
         isMoving = true;
         float moveX, moveZ;

         if (destination.x - player.transform.position.x != 0) {
            moveX = Mathf.Sign(destination.x - player.transform.position.x);
         } else {
            moveX = 0;
         }

         if (destination.z - player.transform.position.z != 0) {
            moveZ = Mathf.Sign(destination.z - player.transform.position.z);
         } else {
            moveZ = 0;
         }

         print($"X: {moveX}, Z: {moveZ}");

         var nextPosition = player.transform.position;
         nextPosition = new Vector3(
            nextPosition.x + moveX,
            0,
            nextPosition.z + moveZ
            );

         player.transform.position = nextPosition;
         yield return new WaitForSeconds(0.25f);
      }

      isMoving = false;
   }

   private Vector2Int GetPositionInGrid(Vector3 point)
   {
      int gridPosX = Mathf.RoundToInt(point.x / grid.cellSize);
      int gridPosZ = Mathf.RoundToInt(point.z / grid.cellSize);
      return new Vector2Int(gridPosX, gridPosZ);
   }

   private Vector3 GetDestinationPoint()
   {
      Vector3 screenPosition = Input.mousePosition;
      var mouseWorldPosition = mainCamera.ScreenPointToRay(screenPosition);

      Physics.Raycast(mouseWorldPosition, out hitInfo, 100, groundLayer);

      var positionInGrid = GetPositionInGrid(hitInfo.point) * grid.cellSize;

      hitInfo.point = new Vector3(Mathf.FloorToInt(
         positionInGrid.x),
         0.001f,
         positionInGrid.y);

      return hitInfo.point;
   }
}
