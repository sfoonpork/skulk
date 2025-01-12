using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    GameObject goal;
    bool playing;
    float gameTime;
    public GameObject playerPrefab;

    int stages = 0;

    public List<GameObject> stagePrefabs = new List<GameObject>();
    public List<GameObject> enemyPrefabs = new List<GameObject>();


    GameObject stage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void CompleteStage()
    {
        // stage.GetComponent<Generator>().Clear();
        // for (int i = enemies.Count; i >= 0; i--)
        // {
        //     GameObject enemy = enemies[i];
        //     enemy.GetComponent<EnemyBehavior>().Despawn();
        // }
        GenerateStage();
    }

    public void GenerateStage()
    {
        int selected = Random.Range(0, stagePrefabs.Count);
        stage = Instantiate(stagePrefabs[selected]);
        stages += 1;
        float time = (float)stages * 4f;
        print(time);
        stage.GetComponent<Generator>().Lock(time);
        for(int i = 0; i < 64; i++)
        {
            // GenerateEnemy
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        GameObject player = GameObject.FindWithTag("Player");
        if (!playing)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Camera.main.fieldOfView = 70f;
                gameTime = 0f;
                playing = true;
                GenerateStage();
                Instantiate(playerPrefab);
            }
            
        }
        else
        {
        }
        
    }
}
