using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaucerBehavior : MonoBehaviour
{
    
    private Spawner spawner;
    private GameObject player;
    private PlayerBehavior playerBehavior;
    private Rigidbody rb;
    private float time = 0.0f;
    private float duration = 5f;
    float globalTime = 20f;

    public GameObject beamPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
        spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();
        player = GameObject.FindWithTag("Player");
        playerBehavior = player.GetComponent<PlayerBehavior>();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    public void Fire()
    {
        GameObject beam = Instantiate(beamPrefab, gameObject.transform.position, gameObject.transform.rotation);
        beam.transform.Rotate(90f, 0f, 0f, Space.Self);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        globalTime -= Time.deltaTime;
        if (globalTime < 0.0f)
        {
            spawner.SpawnParticles(gameObject, 10);
            spawner.enemies.Remove(gameObject);
            Destroy(gameObject);
        }
        Vector3 origin = transform.position;
        time -= Time.deltaTime;
        if (time <= 0f)
        {
            time += duration;
            if (player) Fire();
        }
        if (player)
        {
            Vector3 target = player.transform.position;
            target += player.GetComponent<Rigidbody>().velocity;
            Vector3 forward = transform.forward;
            Vector3 directionPlayer = target - origin;
            float distance = directionPlayer.magnitude;
            float adjustedDistance = distance * 2f + 256f;
            // float magnitude = adjustedDistance * (Vector3.Dot(directionPlayer.normalized, forward) + 1f)/2f;
            float magnitude = adjustedDistance * Vector3.Dot(directionPlayer.normalized, forward);

            // Quaternion targetRotation = Quaternion.LookRotation(directionPlayer);
            // Quaternion deltaRotation = targetRotation * Quaternion.Inverse(transform.rotation);
            // Vector3 torque = new Vector3(deltaRotation.x, deltaRotation.y, deltaRotation.z);
            // rb.AddTorque(torque * 4f, ForceMode.Impulse);

            float rotationSpeed = 2048f;
            Vector3 directionToTarget = target - transform.position;
            directionToTarget.Normalize();
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            Quaternion newRotation = Quaternion.RotateTowards(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(newRotation);

            // rb.AddForce(transform.forward * magnitude);

        }
    }
}
