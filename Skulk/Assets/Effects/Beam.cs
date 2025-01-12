using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    float speed = 128f;
    float time = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnCollisionEnter(Collision collision)
    {
        
        GameObject player = GameObject.FindWithTag("Player");
        PlayerBehavior playerBehavior = player.GetComponent<PlayerBehavior>();
        Spawner spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();
        if (collision.gameObject == player)
        {
            playerBehavior.Damage(10f);
        }
        if (!collision.gameObject.CompareTag("Enemy"))
        {
            // spawner.SpawnParticles(gameObject, 5);
            Destroy(gameObject);
        }
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        time -= Time.fixedDeltaTime;
        if (time < 0) Destroy(gameObject);
        
        if(!Camera.main.GetComponent<GameV1>().playing) Destroy(gameObject);

        float rotationSpeed = 2.0f; // The speed of the rotation
        GameObject player = GameObject.FindWithTag("Player");
        if(!player) return;
        Transform target = player.transform;


        Vector3 directionToTarget = target.position - transform.position;
        Vector3 normalizedDirection = directionToTarget.normalized;
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, normalizedDirection) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.position += transform.up * speed * Time.fixedDeltaTime;

    }
}
