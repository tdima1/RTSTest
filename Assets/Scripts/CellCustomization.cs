using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellCustomization : MonoBehaviour
{
   [SerializeField]
   private MeshRenderer meshRenderer;
   TextMesh _textMesh;

   private void Awake()
   {
      //meshRenderer = GetComponent<MeshRenderer>();
      //_textMesh = GetComponentInChildren<TextMesh>();
      //UpdateLabel();
   }
   void Update()
   {
   }

   private void UpdateLabel()
   {
      //_textMesh.text = (Mathf.RoundToInt(transform.position.x)).ToString() + ":" + (Mathf.RoundToInt(transform.position.z)).ToString();
   }

   public void SetMaterialColor(Color color)
   {
      meshRenderer.sharedMaterial.color = color;
   }
}
