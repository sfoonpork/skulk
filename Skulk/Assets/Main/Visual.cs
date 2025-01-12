using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Visual : MonoBehaviour
{
    private Camera mainCamera;
    private GameObject player;
    private Spawner spawner;
    private float targetZoom;
    public PostProcessVolume volume;
    
    private float zoom;
    private Vignette vignette;
    private DepthOfField dof;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();
        volume.profile.TryGetSettings(out vignette);
        volume.profile.TryGetSettings(out dof);
    }

    public void Damage(float amount)
    {
        vignette.smoothness.value = 1f;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        vignette.smoothness.value += (0f - vignette.smoothness.value) * 1f/64f;
        // Game game = gameObject.GetComponent<Game>();
        // dof.focusDistance.value = game.playing ? 1024f : 0f;

        player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            // float scroll = Input.GetAxis("Mouse ScrollWheel");

            // Camera zoom controls
            // targetZoom -= scroll * 10f;
            // targetZoom = Mathf.Clamp(targetZoom, 0f, 30f);
            // zoom += (targetZoom - zoom) * 1f/16f;
            // mainCamera.transform.position = player.transform.position;
            // mainCamera.transform.position += -mainCamera.transform.forward * zoom;
            
            // Rigidbody rb = player.GetComponent<Rigidbody>();

            // float magnitude = Vector3.Dot(rb.velocity, mainCamera.transform.forward);

            // float targetFieldOfView = (magnitude)/1024f * 20f + 90f;
            // float targetFieldOfView = (magnitude)/256f * 10f + 100f;
            // float targetFieldOfView = (magnitude)/512f * 20f + 90f;
            // float targetFieldOfView = 90f;
            // mainCamera.fieldOfView += (targetFieldOfView - mainCamera.fieldOfView) * 1f/16f;
        }
        else
        {
            zoom = 0f;
            Vector3 center = Vector3.zero;
            float total = 0f;
            foreach (GameObject enemy in spawner.enemies)
            {
                center += enemy.transform.position;
                total += 1f;
            }
            if (total > 0f) center /= total;

            Vector3 forwardVector = mainCamera.transform.forward;
            // mainCamera.transform.rotation *= Quaternion.Euler(Time.deltaTime, 0f, 0f);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, center - forwardVector * 32f, 1f/32f);
        }
    }
}
