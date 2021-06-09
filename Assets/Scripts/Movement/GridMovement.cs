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
            var movementThisFrame = movementSpeed * Time.fixedDeltaTime;

            while((cell.worldPosition - unit.position).magnitude > movementThisFrame) {

               unit.position += (cell.worldPosition - unit.position).normalized * movementThisFrame;

               yield return new WaitForFixedUpdate();
            }

            snapToGrid.enabled = true;
         }

         snapToGrid.enabled = true;
      }
   }
}
