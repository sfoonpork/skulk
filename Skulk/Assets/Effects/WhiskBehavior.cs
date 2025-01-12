using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiskBehavior : MonoBehaviour
{
    private Spawner spawner;

    float globalTime = 20f;
    // Start is called before the first frame update
    void Start()
    {
        
        spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();
    }

    // Update is called once per frame
    void Update()
    {
        globalTime -= Time.deltaTime;
        if (globalTime < 0)
        {
            spawner.SpawnParticles(gameObject, 10);
            spawner.enemies.Remove(gameObject);
            Destroy(gameObject);
        }
    }
}
