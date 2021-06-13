namespace Assets.Scripts.Combat
{
    using Assets.Scripts.Enemy;
    using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Text;
   using System.Threading.Tasks;
   using UnityEngine;

   public class UnitSelector : MonoBehaviour
   {
      [SerializeField] private Camera mainCamera;

      private GameObject lastHitEnemy;

      private void Awake()
      {
         mainCamera = GetComponentInChildren<Camera>();
      }

      private void Update()
      {
         Outline enemyOutline;

         Vector3 screenPosition = Input.mousePosition;
         var mouseWorldPosition = mainCamera.ScreenPointToRay(screenPosition);
         Physics.Raycast(mouseWorldPosition, out RaycastHit hitInfo);

         if (hitInfo.collider.name == "Enemy") {

            lastHitEnemy = hitInfo.transform.parent.gameObject;

            enemyOutline = lastHitEnemy.GetComponent<Outline>();
            enemyOutline.enabled = true;

         } else {
            enemyOutline = lastHitEnemy.GetComponent<Outline>();
            enemyOutline.enabled = false;

         }


      }
   }
}
