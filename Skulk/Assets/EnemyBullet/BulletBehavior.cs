using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{

    private Spawner spawner;
    private GameObject player;
    private PlayerBehavior playerBehavior;
    private Rigidbody rb;
    float globalTime = 20f;

    // Start is called before the first frame update
    void Start()
    {
        spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();
        player = GameObject.FindWithTag("Player");
        playerBehavior = player.GetComponent<PlayerBehavior>();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == player)
        {
            spawner.SpawnParticles(gameObject, 8);
            spawner.enemies.Remove(gameObject);
            Destroy(gameObject);
            playerBehavior.Damage(40f);
        }
        // if (!collision.gameObject.CompareTag("Enemy"))
        {
            // spawner.SpawnParticles(gameObject, 2);
            // spawner.enemies.Remove(gameObject);
            // Destroy(gameObject);
        }
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        globalTime -= Time.fixedDeltaTime;
        if (globalTime < 0)
        {
            spawner.SpawnParticles(gameObject, 10);
            spawner.enemies.Remove(gameObject);
            Destroy(gameObject);
        }
        Vector3 origin = transform.position;
        foreach (GameObject enemy in spawner.enemies)
        {
            if (enemy == gameObject) continue;
            Vector3 target = enemy.transform.position;
            Vector3 directionEnemy = target - origin;
            float targetDistance = 64f;
            float currentDistance = directionEnemy.magnitude;
            if (currentDistance < targetDistance)
            {
                float force = (currentDistance - targetDistance);
                rb.AddForce(directionEnemy.normalized * force);
            }
            // float forceMagnitude = 100f;
        }

        if (player)
        {
            Vector3 target = player.transform.position;
            target += player.GetComponent<Rigidbody>().velocity;
            Vector3 forward = transform.forward;
            Vector3 directionPlayer = target - origin;
            float distance = directionPlayer.magnitude;
            // float adjustedDistance = distance * 4f + 512f;
            float adjustedDistance = distance * 4f/2f + 512f/2f;
            // float magnitude = adjustedDistance * (Vector3.Dot(directionPlayer.normalized, forward) + 1f)/2f;
            float magnitude = adjustedDistance * Vector3.Dot(directionPlayer.normalized, forward);

            // Quaternion targetRotation = Quaternion.LookRotation(directionPlayer);
            // Quaternion deltaRotation = targetRotation * Quaternion.Inverse(transform.rotation);
            // Vector3 torque = new Vector3(deltaRotation.x, deltaRotation.y, deltaRotation.z);
            // rb.AddTorque(torque * 4f, ForceMode.Impulse);

            float rotationSpeed = 1024f;
            Vector3 directionToTarget = target - transform.position;
            directionToTarget.Normalize();
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            Quaternion newRotation = Quaternion.RotateTowards(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(newRotation);

            rb.AddForce(transform.forward * magnitude);
        }
    }
}
