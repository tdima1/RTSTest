using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
[ExecuteInEditMode]
public class FollowPlayer : MonoBehaviour
{
   [SerializeField]
   private Transform target;

   [SerializeField]
   private Vector3 cameraOffset;

   [SerializeField]
   private Vector3 cameraRotationOffset;

   [SerializeField]
   [Range(0,1)]
   private float cameraSmoothness;

   // Update is called once per frame
   void LateUpdate()
   {
      Vector3 cameraPosition = target.position + cameraOffset;
      Vector3 smoothCameraPosition = Vector3.Lerp(transform.localPosition, cameraPosition, cameraSmoothness);
      transform.localPosition = smoothCameraPosition;

      Quaternion cameraRotation = Quaternion.Euler(cameraRotationOffset);
      transform.rotation = cameraRotation;
   }
}
