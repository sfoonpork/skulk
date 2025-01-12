using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minefield : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Generator generator = GetComponent<Generator>();
        for (int i = 0; i < 64; i++)
        {
            GameObject platform = generator.Planetarium();
            platform.transform.position = Random.insideUnitSphere * 128f;
        }
        GameObject goal = generator.Goal();
        goal.transform.position = Random.insideUnitSphere * 128f;
        goal.GetComponent<Goal>().Lock(10f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
