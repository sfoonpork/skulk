using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> platformPrefabs = new List<GameObject>();
    public GameObject sunspotPrefab;
    public GameObject particlePrefab;
    public GameObject playerPrefab;
    public GameObject whiskPrefab;
    public GameObject saucerPrefab;
    public GameObject bulletPrefab;
    private GameObject player;
    public List<GameObject> platforms;
    public List<GameObject> particles;
    public List<GameObject> enemies;

    public void Clear()
    {
        for (int i = platforms.Count - 1; i >= 0; i--)
        {
            GameObject platform = platforms[i];
            platforms.RemoveAt(i);
            Destroy(platform);
        }
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            GameObject enemy = enemies[i];
            enemies.RemoveAt(i);
            Destroy(enemy);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // platforms = new List<GameObject>();
    }
    public void SpawnPlayer()
    {
        GameObject player = Instantiate(playerPrefab);
        // Camera.main.transform.position = player.transform.position;
        // Camera.main.transform.rotation = player.transform.rotation;
    }

    GameObject Spawn(GameObject prefab, List<GameObject> list)
    {
        Vector3 position = Random.insideUnitSphere * 64f;
        Quaternion rotation = Random.rotation;
        GameObject spawned = Instantiate(prefab, position, rotation);
        list.Add(spawned);
        spawned.transform.parent = gameObject.transform;
        return spawned;
    }

    public void SpawnParticle(GameObject parent)
    {
        GameObject particle = Instantiate(particlePrefab);
        particle.GetComponent<Particle>().duration = Random.Range(4f, 8f);
        particle.transform.parent = gameObject.transform;
        particle.transform.position = parent.transform.position;
        particle.transform.rotation = parent.transform.rotation;
        Rigidbody rb = particle.GetComponent<Rigidbody>();
        rb.velocity = Random.insideUnitSphere * 100f;
        rb.angularVelocity = Random.onUnitSphere * 100f;
    }
    public void SpawnParticles(GameObject parent, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            SpawnParticle(parent);
        }
    }

    public void SpawnPlatform()
    {
        int index = (Random.Range(0, 50) > 0  ? 0 : Random.Range(1, platformPrefabs.Count));
        GameObject platform = Spawn(platformPrefabs[index], platforms);
        if (index == 0)
        {
            Rigidbody rbPlatform = platform.GetComponent<Rigidbody>();
            platform.transform.localScale = new Vector3(Mathf.Pow(2, Random.Range(0, 4)), Mathf.Pow(2, Random.Range(0, 4)), Mathf.Pow(2, Random.Range(0, 4)));
            rbPlatform.mass = platform.transform.localScale.x * platform.transform.localScale.y * platform.transform.localScale.z * 0.5f;
        }
    }
    public void SpawnEnemy()
    {
        int select = Random.Range(1, 6);
        if (select <= 0) SpawnWhisk();
        else if (select <= 2) SpawnSaucer();
        else SpawnBullet();
    }

    public void SpawnWhisk()
    {
        GameObject whisk = Spawn(whiskPrefab, enemies);
        GameObject sunspot = Instantiate(sunspotPrefab, whisk.transform);
    }

    public void SpawnSaucer()
    {

        GameObject saucer = Spawn(saucerPrefab, enemies);
        GameObject sunspot = Instantiate(sunspotPrefab, saucer.transform);
    }

    public void SpawnBullet()
    {
        GameObject bullet = Spawn(bulletPrefab, enemies);
        GameObject sunspot = Instantiate(sunspotPrefab, bullet.transform);
        // bullet.transform.localScale = Vector3.one * 4f;
        // SphereCollider sc = bullet.AddComponent(typeof(SphereCollider)) as SphereCollider;
        // Rigidbody rb = bullet.AddComponent(typeof(Rigidbody)) as Rigidbody;
        // rb.mass = 8f;
        // rb.drag = 1f;
        // rb.angularDrag = 2f;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
