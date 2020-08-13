using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
   public int speed = 1;

   private Vector3 movementDirection = new Vector3();
   private Vector3 movementThisFrame = new Vector3();
   private RaycastHit hitInfo = new RaycastHit();

   private Tree tree;

   // Start is called before the first frame update
   void Start()
   {

   }

   // Update is called once per frame
   void Update()
   {
      //GoToTree();
      GetUserInput();
      MovePlayerToClick();
   }
   private void GetUserInput()
   {
      if(Input.GetMouseButtonDown(0)) {
         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

         if(Physics.Raycast(ray, out hitInfo)) {
            movementDirection = hitInfo.point - transform.position;
         }
      }
   }

   private void MovePlayerToClick()
   {
      if ()
      movementThisFrame += new Vector3(movementDirection.x, 0, movementDirection.z) * speed * Time.deltaTime;
      transform.position = movementThisFrame;
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
