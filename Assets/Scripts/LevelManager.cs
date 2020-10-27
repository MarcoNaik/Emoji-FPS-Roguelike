using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; protected set; }

    void Awake()
    {
        
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        Stage = 0;
    }
    
    public int Stage { get; private set; }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
        PlayerController.Instance.transform.position = Vector3.up*3;
        Stage++;
        StartCoroutine(generatorRoutine());
    }

    IEnumerator generatorRoutine()
    {
        yield return new WaitForSeconds(1);
        FindObjectOfType<DungeonScripts.DungeonGenerator>().targetRooms = 12 * Stage;
        FindObjectOfType<DungeonScripts.DungeonGenerator>().StartGeneration();
    }
    
    
}
