using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
   [SerializeField]
   [Range(1, 50)]
   private int MaxEnemiesCount = 30;
   [SerializeField]
   [Range(25, 100)]
   private int sampleRange = 100;

   [SerializeField]
   private Transform enemiesParent;
   [SerializeField]
   private GameObject enemyPrefab;
   [SerializeField]
   private Transform playerTransform;
   [SerializeField]
   private LayerMask groundLayer;
   [SerializeField]
   private LayerMask obstacleLayer;
   [SerializeField]
   private LayerMask treesLayer;


   private List<GameObject> enemiesList;

   void Start()
   {
      enemiesList = new List<GameObject>();
      InstantiateEnemies(50);
   }


   // Update is called once per frame
   void Update()
   {

   }

   private void InstantiateEnemies(int enemiesCount)
   {
      for (int i = 0; i < enemiesCount; i++) {
         var enemy = Instantiate(enemyPrefab, enemiesParent);

         // Look for spawn position
         var spawnPosition = SamplePositionAroundPlayer(playerTransform.position, sampleRange);


         enemy.transform.position = spawnPosition;
         // Set position on the enemy


         //enemy.transform = new Vector3Int(UnityEngine.Random.Range(-10, 10),)

         enemiesList.Add(enemy);
      }
   }

   private Vector3Int SamplePositionAroundPlayer(Vector3 position, int sampleRange)
   {
      Vector3Int samplePosition = new Vector3Int {
         x = Mathf.RoundToInt(position.x),
         y = Mathf.RoundToInt(position.y),
         z = Mathf.RoundToInt(position.z),
      };

      int randomX = Random.Range(samplePosition.x - sampleRange, samplePosition.x + sampleRange);
      int randomZ = Random.Range(samplePosition.z - sampleRange, samplePosition.z + sampleRange);

      Vector3Int sampleOrigin = new Vector3Int {
         x = randomX,
         y = 100,
         z = randomZ,
      };

      bool hit = Physics.Raycast(sampleOrigin, Vector3.down, out RaycastHit rayHitInfo, 120, groundLayer | obstacleLayer | treesLayer);

      while(!hit) {
         randomX = Random.Range(samplePosition.x - sampleRange, samplePosition.x + sampleRange);
         randomZ = Random.Range(samplePosition.z - sampleRange, samplePosition.z + sampleRange);

         sampleOrigin = new Vector3Int {
            x = randomX,
            y = 100,
            z = randomZ,
         };

         hit = Physics.Raycast(sampleOrigin, Vector3.down, out rayHitInfo, 120, groundLayer | obstacleLayer);
      }

      return new Vector3Int {
         x = Mathf.RoundToInt(rayHitInfo.point.x),
         y = Mathf.RoundToInt(rayHitInfo.point.y),
         z = Mathf.RoundToInt(rayHitInfo.point.z),
      };
   }
}
