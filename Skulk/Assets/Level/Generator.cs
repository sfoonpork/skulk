using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public List<GameObject> platformPrefabs = new List<GameObject>();
    public GameObject goalPrefab;
    public List<GameObject> platforms;

    
    public void Unlock()
    {
        goalPrefab.GetComponent<Goal>().Unlock();
    }
    public void Lock()
    {
        goalPrefab.GetComponent<Goal>().Lock();
    }

    public void Unlock(float seconds)
    {
        goalPrefab.GetComponent<Goal>().Unlock(seconds);
    }

    public void Lock(float seconds)
    {
        goalPrefab.GetComponent<Goal>().Lock(seconds);
    }

    public GameObject Any()
    {
        int selected = Random.Range(0, 8);
        
        GameObject platform = Instantiate(platformPrefabs[selected], gameObject.transform);
        platforms.Add(platform);
        return platform;
    }
    public GameObject Truss()
    {
        int selected = Random.Range(2, 5);
        
        GameObject platform = Instantiate(platformPrefabs[selected], gameObject.transform);
        platforms.Add(platform);
        return platform;
    }

    public GameObject Icosphere()
    {
        int selected = Random.Range(5, 8);
        
        GameObject platform = Instantiate(platformPrefabs[selected], gameObject.transform);
        platforms.Add(platform);
        return platform;
    }

    public GameObject Planetarium()
    {
        int selected = Random.Range(0, 5);
        if (selected > 1) selected += 3;
        
        GameObject platform = Instantiate(platformPrefabs[selected], gameObject.transform);
        platforms.Add(platform);
        return platform;
    }

    public GameObject Generic()
    {
        int selected = Random.Range(0, 2);
        
        GameObject platform = Instantiate(platformPrefabs[selected], gameObject.transform);
        platforms.Add(platform);
        return platform;
    }

    public GameObject Box()
    {
        int selected = Random.Range(0, 1);
        
        GameObject platform = Instantiate(platformPrefabs[selected], gameObject.transform);
        platforms.Add(platform);
        return platform;
    }

    public GameObject Sphere()
    {
        int selected = Random.Range(1, 2);
        
        
        GameObject platform = Instantiate(platformPrefabs[selected], gameObject.transform);
        platforms.Add(platform);
        return platform;
    }

    public GameObject Goal()
    {
        int selected = Random.Range(1, 2);
        
        
        GameObject platform = Instantiate(goalPrefab, gameObject.transform);
        platforms.Add(platform);
        return platform;
    }
    public void Clear()
    {
        for (int i = platforms.Count - 1; i >= 0; i--)
        {
            GameObject platform = platforms[platforms.Count - 1];
            platforms.Remove(platform);
            Destroy(platform);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
