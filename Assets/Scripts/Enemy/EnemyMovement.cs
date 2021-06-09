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

         foreach(var cell in path) {
            snapToGrid.enabled = false;

            var movementThisFrame = movementSpeed * Time.fixedDeltaTime;

            while((cell.worldPosition - unit.position).magnitude > movementThisFrame) {

               unit.position += (cell.worldPosition - unit.position).normalized * movementThisFrame;

               yield return new WaitForFixedUpdate();
            }
            snapToGrid.enabled = true;
         }

         yield return new WaitForSecondsRealtime(2f);

         snapToGrid.enabled = true;
         isMoving = false;
      }
   }
}
