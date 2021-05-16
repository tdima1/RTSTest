using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellCustomization : MonoBehaviour
{
   [SerializeField]
   private Material cellMaterial;

   private void Start()
   {
   }

   public void SetMaterialColor(Color color)
   {
      cellMaterial.color = color;
   }
}
