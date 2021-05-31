using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Movement
{
   public static class GridMovement
   {
      public static IEnumerator MoveThroughPath(Transform unit, List<GridCell> path, int movementSpeed)
      {
         var snapToGrid = unit.GetComponent<SnapToGrid>();
         snapToGrid.enabled = false;

         Vector3 movement = Vector3.zero;

         foreach(var cell in path) {

            snapToGrid.enabled = false;

            while((cell.worldPosition - unit.position).magnitude > 0.1f) {

               if(Mathf.Abs(cell.worldPosition.x - unit.position.x) < 0.05f) {
                  movement.z = Math.Sign(cell.worldPosition.z - unit.position.z);
                  movement.x = 0;


               } else if(Mathf.Abs(cell.worldPosition.z - unit.position.z) < 0.05f) {
                  movement.x = Math.Sign(cell.worldPosition.x - unit.position.x);
                  movement.z = 0;
               }

               if(Mathf.Abs(cell.Height - unit.position.y) < 0.1f) {
                  movement.y = 0;
               } else {
                  movement.y = Math.Sign(cell.Height - unit.position.y);
               }


               if(Mathf.Abs(cell.Height - unit.position.y) < 0.05f) {
                  movement.y = 0;
               } else {
                  movement.y = Math.Sign(cell.Height - unit.position.y);
               }

               unit.position += movement * movementSpeed * Time.deltaTime;


               yield return new WaitForFixedUpdate();
            }

            snapToGrid.enabled = true;
         }

         snapToGrid.enabled = true;
      }
   }
}
