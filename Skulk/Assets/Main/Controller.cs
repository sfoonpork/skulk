using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    GameObject beam;
    public Vector3 direction;
    public float speed;
    public Vector3 target;
    public GameObject gameObjectTarget;
    public GameObject anchor;
    public Vector3 origin;
    int index;
    new public GameObject gameObject;
    float time;
    float timeSinceHit;

    public static List<Grapple> grapples = new List<Grapple>();

    public void Start()
    {
        beam = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        index = grapples.Count;
        grapples.Add(this);
        Destroy(beam.GetComponent<Collider>());
    }
    
    void OnDestroy()
    {
        Destroy(beam);
        Destroy(anchor);
    }

    void FixedUpdate()
    {
        if (anchor)
        {
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            Rigidbody rbTarget = gameObjectTarget.GetComponent<Rigidbody>();
            Vector3 grappleDirection = anchor.transform.position - gameObject.transform.position;
            
            float magnitude = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ? 10f : 1f); 
            if (rb) rb.AddForce(grappleDirection.normalized * magnitude);
            if (rbTarget) rbTarget.AddForce(-grappleDirection.normalized * magnitude);
        }
    }

    void Update()
    {

    }

    void LateUpdate()
    {
        time += Time.deltaTime;

        Vector3 prev = target;
        Vector3 delta = direction * speed * Time.deltaTime;
        Vector3 next = prev + delta;

        RaycastHit hit;
        if (Physics.Raycast(prev, direction, out hit) && !anchor)
        {
            gameObjectTarget = hit.collider.gameObject;
            anchor = new GameObject("Anchor");
            anchor.transform.parent = gameObjectTarget.transform;
            anchor.transform.position = hit.point;
            Renderer renderer = GetComponent<Renderer>();

        }

        if (!gameObjectTarget)
        {
            target = next;
        }
        else
        {
            timeSinceHit += Time.deltaTime;
            target = anchor.transform.position;
            Renderer renderer = beam.GetComponent<Renderer>();
            Material material = renderer.material;
            Color emissionColor = Color.white;
            float emissionIntensity = (1f / Mathf.Pow(timeSinceHit + 1f, 2)) * 8f + 2f;
            material.SetColor("_EmissionColor", emissionColor * emissionIntensity);
            material.EnableKeyword("_EMISSION");
        }


        float angle = -index * Mathf.PI * 2f / 12f;
        Vector3 origin = gameObject.transform.position - Mathf.Sin(angle) * Camera.main.transform.right - Mathf.Cos(angle) * Camera.main.transform.up;


        float distance = Vector3.Distance(origin, target);


        beam.transform.position = Vector3.Lerp(origin, target, 0.5f);
        beam.transform.rotation = Quaternion.LookRotation(target - origin, beam.transform.up) * Quaternion.Euler(-90f, 0f, 0f);
        beam.transform.localScale = new Vector3(1f/8f, distance/2f, 1f/8f);
        
                

    }
}

public class Controller : MonoBehaviour
{

    Rigidbody rb;
    
    public AudioSource ping;
    public AudioSource jump;

    // Body
    bool grounded = false;
    bool grappling = false;
    
    // Camera
    float zoom = 0f;
    float camX = 0f;
    float camY = 0f;
    float fieldOfView = 0f;

    // Gravity
    Vector3 worldDirection;
    GameObject gyro;
    GameObject gyroSmooth;

    // Start is called before the first frame update
    void Start()
    {
        Time.fixedDeltaTime = 1f / 60f;
        gyro = new GameObject("Gyro");
        gyroSmooth = new GameObject("GyroSmooth");
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    void OnDestroy()
    {
        Destroy(gyro);
        Destroy(gyroSmooth);
    }

    void FixedUpdate()
    {
        // Gravity position
        worldDirection = Vector3.zero;
        Transform transform = gameObject.transform;
        Vector3 position = transform.position;
        Collider[] colliders = FindObjectsOfType<Collider>();
        float cumulativeMagnitude = 0f;

        foreach (Collider collider in colliders)
        {
            if (collider == null) continue;
            if (collider.gameObject == gameObject) continue;
            Rigidbody rbTarget = collider.GetComponent<Rigidbody>();

            if (!rbTarget) continue;
            
            Vector3 target = collider.ClosestPoint(position);
            Vector3 direction = target - position;
            float distance = direction.magnitude + 1f;

            if (distance == 0f) continue;
            
            const float gravitationalConstant = 1.0f;

            float forceMagnitude = gravitationalConstant * (rb.mass * rbTarget.mass) / Mathf.Pow(distance, 2);
            Vector3 force = direction.normalized * forceMagnitude;
            rb.AddForce(force);
            rbTarget.AddForceAtPosition(-force, position + direction);
            worldDirection += force;

            float magnitude = Mathf.Clamp(1f / Mathf.Pow(distance + 0.5f, 2), 0f, 1f);
            cumulativeMagnitude += magnitude;
            rb.AddForce(rbTarget.velocity * magnitude);

        }
        

        // Gravity rotation
        Quaternion rotation = Quaternion.FromToRotation(gyro.transform.up, -worldDirection);
        gyro.transform.rotation = rotation * gyro.transform.rotation;
        gyroSmooth.transform.rotation = Quaternion.Slerp(gyroSmooth.transform.rotation, gyro.transform.rotation, 1f/16f);

        // Basic movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontal, 0, vertical);
        movement = movement.normalized * Mathf.Max(Mathf.Abs(horizontal), Mathf.Abs(vertical));
        float speedMovement = 10f + (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ? 10f : 0f);
        Vector3 worldmovement = Camera.main.transform.TransformDirection(movement);
        rb.AddForce(worldmovement * speedMovement * Time.fixedDeltaTime, ForceMode.VelocityChange);

        // Ground detection
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -gyro.transform.up, out hit, 0.6f))
        {
            grounded = true;
        }
        else grounded = false;

    }

    void Fire()
    {
        // print("Fire");
        if (Grapple.grapples.Count >= 12) return;

        Grapple grapple = gameObject.AddComponent<Grapple>();
        grapple.direction =  Camera.main.transform.forward;
        grapple.gameObject = gameObject;
        grapple.speed = 128f;
        grapple.target = transform.position;
        ping.pitch = Grapple.grapples.Count + 1f;
        ping.Play();
    }

    void Release()
    {
        // print("Release");
        while (Grapple.grapples.Count > 0)
        {
            Destroy(Grapple.grapples[0]);
            Grapple.grapples.RemoveAt(0);
        } 
    }

    // Update is called once per frame
    void Update()
    {
        // Jumping and dashing
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce((grounded ? -worldDirection.normalized : Camera.main.transform.forward) * 10f, ForceMode.Impulse);
            // jump.Play();
        }

        
        // Grapple firing and releasing
        if (Input.GetMouseButtonDown(0))
        {
            grappling = true;
            Fire();
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (grappling) Fire();
        }

        if (!Input.GetMouseButton(0))
        {
            if (grappling)
            {
                grappling = false;
                Release();
            }
        }



        // Mouse movement
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        camX += mouseX;
        camY += -mouseY;
        camY = Mathf.Clamp(camY, -90f, 90f);
        gyro.transform.position = transform.position;
        gyroSmooth.transform.position = gyro.transform.position;
        
        // Zooming
        zoom -= Input.GetAxis("Mouse ScrollWheel") * 4f;
        zoom = Mathf.Clamp(zoom, 0f, 32f);
        
        // Field of view

        fieldOfView = 90f + Mathf.Clamp(rb.velocity.magnitude * 2f, 0f, 60f);
        Camera.main.fieldOfView += (fieldOfView - Camera.main.fieldOfView) * 1f/16f;

    }
    void LateUpdate()
    {
        // Camera updating
        // gyroSmooth.transform.rotation = gyro.transform.rotation;
        Camera main = Camera.main;
        main.transform.position = gyroSmooth.transform.position;
        main.transform.rotation = gyroSmooth.transform.rotation;
        main.transform.Rotate(Vector3.up, camX, Space.Self);
        main.transform.Rotate(Vector3.right, camY, Space.Self);
        main.transform.position -= main.transform.forward * zoom;
    }
}
