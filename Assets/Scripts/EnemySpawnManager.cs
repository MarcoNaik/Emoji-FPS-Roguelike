using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawnManager : MonoBehaviour
{
   public int baseDamage;
   public float baseSpeed;
   public int baseMoney;
   public int baseHealth;
   
   public float minDistance;
   public float maxDistance;

   public List<GameObject> enemyPrefabs;
   
   private int unitSpawned;
   private int stage;

   public int enemysAlive = 0;


   private void Start()
   {
      StartCoroutine(SpawnCoroutine());
      stage = FindObjectOfType<LevelManager>().Stage;
   }


   IEnumerator SpawnCoroutine()
   {
      enemysAlive++;
      unitSpawned++;
      Spawn();
      
      float secondsToWait = -4 * (Mathf.Log(unitSpawned) - 4);

      if (secondsToWait < 1) secondsToWait = 1;
      
      yield return  new WaitForSeconds(secondsToWait);
      
      
      StartCoroutine(SpawnCoroutine());
   }

   private void Update()
   {
      if (enemysAlive<=0)
      {
         StopAllCoroutines();
         StartCoroutine(SpawnCoroutine());
      }
   }

   private void Spawn()
   {
      GameObject enemyToSpawn = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count - 1)],transform);

      enemyToSpawn.transform.position = RandomPoint(PlayerController.Instance.transform.position, minDistance, maxDistance, 1, 4);
      
      enemyToSpawn.GetComponent<EnemyController>().Set(baseSpeed+0.1f*unitSpawned*stage, baseDamage+unitSpawned*5* stage, stage*(baseMoney+unitSpawned),
         stage*(baseDamage + unitSpawned * 20));

      

   }
   
   private Vector3 RandomPoint(Vector2 origin, float minRadius, float maxRadius, float minHeight, float maxHeight){
 
      Vector2 randomDirection = (Random.insideUnitCircle * origin).normalized;
 
      float randomDistance = Random.Range(minRadius, maxRadius);
 
      Vector2 point = origin + randomDirection * randomDistance;

      float y = Random.Range(minHeight, maxHeight);
 
      return new Vector3(point.x, y , point.y);
   }
}
