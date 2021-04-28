using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
   public int speed = 1;

   public NavMeshAgent navMeshAgent;

   private Vector3 movementDirection = new Vector3();
   private Vector3 movementThisFrame = new Vector3();
   private RaycastHit hitInfo = new RaycastHit();

   private Tree tree;

   // Start is called before the first frame update
   void Start()
   {
      navMeshAgent = GetComponent<NavMeshAgent>();
   }

   // Update is called once per frame
   void Update()
   {
      //GoToTree();
      GetUserInput();
      //MovePlayerToPosition();
   }
   private void GetUserInput()
   {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      if(Input.GetMouseButtonDown(0)) {
         if(Physics.Raycast(ray, out hitInfo)) {
            //navMeshAgent.destination = hitInfo.point;
            MovePlayerToPosition();
         }
      }

      if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) {
         //navMeshAgent.destination = transform.position;
      }
   }

   private void MovePlayerToPosition()
   {
      float distance = (hitInfo.point - transform.position).magnitude;
      movementDirection = (hitInfo.point - transform.position).normalized;
      //if(distance > 1) {
         movementThisFrame += new Vector3(movementDirection.x, 0, movementDirection.z) * speed * Time.deltaTime;
         transform.position = movementThisFrame;
      //}
   }


   private void Harvest(Tree tree)
   {
      
   }

   private void GoToTree()
   {
      tree = FindObjectOfType<Tree>();
      Vector3 heading = tree.transform.position - transform.position;
      float distance = heading.magnitude;
      Vector3 direction = heading / distance;

      if (distance > 1) {
         Vector3 movementThisFrame = direction * speed * Time.deltaTime;
         transform.localPosition += movementThisFrame;
      } else {
         Harvest(tree);
      }
   }
}
