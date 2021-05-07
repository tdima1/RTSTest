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

   private void Start()
   {
      //transform.localPosition += new Vector3(0, 0, target.position.z - distance);
   }

   // Update is called once per frame
   void LateUpdate()
   {
      //transform.LookAt(target);
      //transform.rotation = Quaternion.Euler(new Vector3(45, 0, 0));  // 90 degress on the X axis - change appropriately
      //transform.rotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
      Vector3 cameraPosition = target.position + cameraOffset;
      Vector3 smoothCameraPosition = Vector3.Lerp(transform.position, cameraPosition, cameraSmoothness);
      transform.position = smoothCameraPosition;

      Quaternion cameraRotation = Quaternion.Euler(cameraRotationOffset);
      transform.rotation = cameraRotation;
   }
}
