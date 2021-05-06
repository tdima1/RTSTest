using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//[ExecuteInEditMode]
//[SelectionBase]
public class Grid : MonoBehaviour
{
   public Camera mainCamera;
   public int cellSize = 1;
   public int gridSize = 10;

   public GameObject cellPrefab;

   void Awake()
   {
      mainCamera = Camera.main;
      for (int i = 0; i < gridSize; i++) {
         for (int j = 0; j < gridSize; j++) {
            cellPrefab.transform.position = new Vector3(i, 0.001f, j);
            Instantiate(cellPrefab, GameObject.FindGameObjectWithTag("Grid").transform);
         }
      }
   }

   // Update is called once per frame
   void Update()
   {
      
   }
}
