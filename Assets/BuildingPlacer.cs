using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
   [SerializeField]
   private GameObject buildingPrefab;

   [SerializeField]
   private KeyCode newObjectHotkey = KeyCode.A;

   private GameObject _currentPlacingObject;

   private float mouseWheelRotation = 0f;

    private void Update()
    {
      HandleHotkeyPressed();
      if (_currentPlacingObject != null) {
         MoveCurrentObjectToMouse();
         RotateFromMouseWheel();
         ReleaseIfClicked();
      }
    }

   private void RotateFromMouseWheel()
   {
      mouseWheelRotation += Input.mouseScrollDelta.y;
      _currentPlacingObject.transform.Rotate(Vector3.up, mouseWheelRotation * 10f);
   }

   private void ReleaseIfClicked()
   {
      if (Input.GetMouseButtonDown(0)) {
         _currentPlacingObject = null;
      }
   }

   private void MoveCurrentObjectToMouse()
   {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

      if (Physics.Raycast(ray, out RaycastHit hitInfo)) {
         _currentPlacingObject.transform.position = hitInfo.point;
         _currentPlacingObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
      }

   }

   private void HandleHotkeyPressed()
   {
      if (Input.GetKeyDown(newObjectHotkey)) {
         if (_currentPlacingObject == null) {
            _currentPlacingObject = Instantiate(buildingPrefab);
         } else {
            Destroy(_currentPlacingObject);
         }
      }
   }
}
