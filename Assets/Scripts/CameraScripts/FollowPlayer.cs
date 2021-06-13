using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
[ExecuteInEditMode]
public class FollowPlayer : MonoBehaviour
{
   [SerializeField]
   private Transform target;


   [SerializeField] private float _verticalInput;

   [SerializeField] private float _pitchPositionFactor = -4f;
   [SerializeField] private float _XTiltFactor = -20f;

   [SerializeField] private float _yawPositionFactor = 5f;

   // Update is called once per frame
   void LateUpdate()
   {
      float pitchFromPosition = transform.localPosition.y * _pitchPositionFactor;
      float pitchFromTilt = _XTiltFactor;
      float pitch = pitchFromPosition + pitchFromTilt;

      float yaw = transform.localPosition.x * _yawPositionFactor;

      transform.localRotation = Quaternion.Euler(new Vector3(pitch, yaw, 0));

      //transform.LookAt(target);
   }
}
