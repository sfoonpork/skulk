using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameV1 : MonoBehaviour
{
    private Spawner spawner;
    public Text text;
    public Text overview;
    public bool playing;
    private float rate = 0f;
    private float score;
    private float difficulty;
    private float gameTime = 0f;
    public AudioSource ost;
    public AudioSource ambience;
    public AudioSource levelup;
    int prevLevel = 0;
    public Image panel;

    void Display()
    {
        overview.text = "SCORE " + (int) score + "\nWAVE " + (int) gameTime + "\nPress SPACE to play.";
    }

    // Start is called before the first frame update
    void Start()
    {
        // Time.fixedDeltaTime = 0.01f;
        spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();
        Display();
        difficulty = 1;
        ambience = GameObject.FindWithTag("Spawner").GetComponent<AudioSource>();
    }

    float threshold;
    // Update is called once per frame
    void FixedUpdate()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (playing)
        {
            float chance = Random.Range(0f, 1f);
            threshold = ((-Mathf.Cos(2 * Mathf.PI * gameTime) + 1f)/2f * gameTime - 0.25f) / 1000f;
            // threshold = ((-Mathf.Cos(2 * Mathf.PI * gameTime) + 1f)/2f * gameTime - 0.25f) / 10f;
            if (chance <= threshold)
            {
                spawner.SpawnEnemy();
            }
            if (!player)
            {
                Display();
                score = 0f;
                playing = false;
            }
        }
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (prevLevel < (int) gameTime) {
            prevLevel += 1;
            levelup.pitch = prevLevel / 6f + 1;
            levelup.Play();
        }
        text.text = "SCORE " + (int) score + "\nWAVE " + (int) gameTime;

        GameObject player = GameObject.FindWithTag("Player");
        if (!playing)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Camera.main.fieldOfView = 70f;
                spawner.Clear();
                for (int i = 0; i < 100; i++)
                {
                    spawner.SpawnPlatform();
                }
                spawner.SpawnPlayer();
                gameTime = 0f;
                playing = true;
            }
            ost.volume += (1f - ost.volume) * 1f/64f;
            ambience.volume += (0f - ost.volume) * 1f/64f;
            
        }
        else
        {
            ost.volume += (0f - ost.volume) * 1f/64f;
            ambience.volume += (1f - ost.volume) * 1f/64f;
            // print(1);
            gameTime += Time.deltaTime / 30f;
            overview.text = "";
            score += Time.deltaTime * 100f;
        }
        panel.color = new Color(0f, 0f, 0f, ost.volume/2f);
    }
}
