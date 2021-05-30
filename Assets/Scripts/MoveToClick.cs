using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoveToClick : MonoBehaviour
{
   public Camera mainCamera;
   public LayerMask GroundLayer;
   public Transform player;

   private Pathfinding _pathfinding;
   private bool isMoving = false;
   private List<GridCell> path = new List<GridCell>();

   [SerializeField]
   [Range(1, 10)]
   private int movementSpeed = 5;

   [SerializeField]
   TextMeshProUGUI textMeshPro;

   void Awake()
   {
      mainCamera = Camera.main;
      _pathfinding = player.GetComponent<Pathfinding>();
   }

   // Update is called once per frame
   void Update()
   {
      WalkToPointOnClick();
      UpdateTextUI();
   }

   private void UpdateTextUI()
   {
      if (path.Count > 0) {

         textMeshPro.text = $"Player X: {player.position.x}" + "\n" +
                              $"Player Y: {player.position.z}" + "\n\n" +

                              $"Selected Cell X: {Mathf.RoundToInt(path[path.Count - 1].worldPosition.x)}" + "\n" +
                              $"Selected Cell Y: {Mathf.RoundToInt(path[path.Count - 1].worldPosition.z)}" + "\n\n" +

                              $"Destination Cell X: {GetDestinationPoint().x}" + "\n" +
                              $"Destination Cell Y: {GetDestinationPoint().z}";
      }
   }

   private void WalkToPointOnClick()
   {
      if(!isMoving) {

         var destination = GetDestinationPoint();

         int distanceX = Vector3Int.FloorToInt(player.transform.position).x - destination.x;
         int distanceZ = Vector3Int.FloorToInt(player.transform.position).z - destination.z;

         if((distanceX + distanceZ) <= _pathfinding.MaxProximityOfDestination) {

            //Generate proximity matrix...
            _pathfinding.GenerateProximityMatrix(Vector3Int.FloorToInt(player.transform.position));

            //Check for path in matrix...
            path = _pathfinding.AStar(destination);

            //Move through path...
            if(Input.GetMouseButtonDown(0)) {
               isMoving = true;
               StartCoroutine("MoveThroughPath", path);
            }

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
         snapToGrid.enabled = false;

         while ((cell.worldPosition - player.position).magnitude > 0.1f) {

            if(Mathf.Abs(cell.worldPosition.x - player.position.x) < 0.05f) {
               movement.z = Math.Sign(cell.worldPosition.z - player.position.z);
               movement.x = 0;


            } else if (Mathf.Abs(cell.worldPosition.z - player.position.z) < 0.05f) {
               movement.x = Math.Sign(cell.worldPosition.x - player.position.x);
               movement.z = 0;


            }

            //movement.z = Math.Sign(cell.worldPosition.z - player.position.z);
            //movement.x = Math.Sign(cell.worldPosition.x - player.position.x);

            player.position += movement * movementSpeed * Time.deltaTime;


            yield return new WaitForFixedUpdate();
         }

         snapToGrid.enabled = true;

         //player.transform.position = Vector3.MoveTowards(player.transform.position, cell.worldPosition, 500 * Time.deltaTime);
      }

      snapToGrid.enabled = true;
      isMoving = false;
   }


   private Vector3Int GetDestinationPoint()
   {
      Vector3 screenPosition = Input.mousePosition;
      var mouseWorldPosition = mainCamera.ScreenPointToRay(screenPosition);

      Physics.Raycast(mouseWorldPosition, out RaycastHit hitInfo, GroundLayer);

      var destination = new Vector3Int(
         Mathf.RoundToInt(hitInfo.point.x),
         Mathf.RoundToInt(hitInfo.point.y),
         Mathf.RoundToInt(hitInfo.point.z));

      print(destination);
      return destination;
   }

}
