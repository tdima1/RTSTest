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

      private GameObject lastSelectedUnit;

      private void Awake()
      {
         mainCamera = GetComponentInChildren<Camera>();
      }

      private void Update()
      {
         SelectUnit();

         UnitOnClick();
      }

      private void UnitOnClick()
      {
         if (Input.GetMouseButtonDown(0) && lastSelectedUnit != null) {
            var unitMeshRenderer = lastSelectedUnit.GetComponentInChildren<MeshRenderer>();

            unitMeshRenderer.material.color = Color.red;
         }
      }

      private void SelectUnit()
      {
         Outline enemyOutline;

         Vector3 screenPosition = Input.mousePosition;
         var mouseWorldPosition = mainCamera.ScreenPointToRay(screenPosition);
         Physics.Raycast(mouseWorldPosition, out RaycastHit hitInfo);

         if(hitInfo.collider.name == "Enemy") {

            lastSelectedUnit = hitInfo.transform.parent.gameObject;

            enemyOutline = lastSelectedUnit.GetComponent<Outline>();
            enemyOutline.enabled = true;

         } else {
            enemyOutline = lastSelectedUnit.GetComponent<Outline>();
            enemyOutline.enabled = false;
            lastSelectedUnit = null;
         }
      }
   }
}
