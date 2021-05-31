using Assets.Scripts.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Enemy
{
   public class EnemyMovement : MonoBehaviour
   {
      private Pathfinding _pathfinding;

      [SerializeField]
      [Range(1, 10)]
      int movementSpeed = 5;

      [SerializeField]
      private bool isMoving = false;

      private void Awake()
      {
         _pathfinding = GetComponent<Pathfinding>();
      }

      private void Update()
      {
         MoveEnemy();
      }

      private void MoveEnemy()
      {
         if(!isMoving) {

            Vector3Int enemyPosition = Vector3Int.FloorToInt(transform.position);

            _pathfinding.GenerateProximityMatrix(enemyPosition);

            var destination = GenerateRandomDestination();

            var path = _pathfinding.AStar(destination);

            isMoving = true;
            StartCoroutine(MoveThroughPath(transform, path, movementSpeed));
         }
      }

      private Vector3Int GenerateRandomDestination()
      {
         var destination = new Vector3Int(
            Random.Range(Mathf.RoundToInt(transform.position.x) - _pathfinding.MaxProximityOfDestination, Mathf.RoundToInt(transform.position.x) + _pathfinding.MaxProximityOfDestination),
            0,
            Random.Range(Mathf.RoundToInt(transform.position.z) - _pathfinding.MaxProximityOfDestination, Mathf.RoundToInt(transform.position.z) + _pathfinding.MaxProximityOfDestination));

         return destination;
      }

      private IEnumerator MoveThroughPath(Transform unit, List<GridCell> path, int movementSpeed)
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

               unit.position += movement * movementSpeed * Time.deltaTime;

               yield return new WaitForFixedUpdate();
            }

            snapToGrid.enabled = true;
         }

         yield return new WaitForSeconds(5f);

         snapToGrid.enabled = true;
         isMoving = false;
      }
   }
}
