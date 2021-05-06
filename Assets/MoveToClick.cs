using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToClick : MonoBehaviour
{
   public Camera mainCamera;
   public LayerMask groundLayer;
   public GameObject player;

   private Grid grid;
   private RaycastHit hitInfo;

   void Awake()
   {
      grid = GetComponent<Grid>();
   }

   // Update is called once per frame
   void Update()
   {
      if(Input.GetMouseButtonUp(0)) {
         var destination = GetDestinationPoint();
         print(destination);


      }
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
