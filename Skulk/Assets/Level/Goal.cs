using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public GameObject beam;
    public GameObject cube;
    public GameObject glass;
    float timer;
    float completionDelta;
    bool active;
    public void Unlock()
    {
        print("Unlocked");
        timer = 0f;
        active = true;
        completionDelta = 1f;
    }
    public void Lock()
    {
        print("Locked");
        timer = 0f;
        active = false;
        completionDelta = -1f;
    }

    public void Unlock(float seconds)
    {
        if (seconds == 0f) Lock(); else Unlock();
        timer = seconds;
    }

    public void Lock(float seconds)
    {
        if (seconds == 0f) Unlock(); else Lock();
        timer = seconds;
    }
    float time;

    void Start()
    {
        Unlock();
    }

    void Update()
    {
        // print(timer);
        if (timer != 0f) timer -= Time.deltaTime;
        time += Time.deltaTime * completionDelta;
        time = Mathf.Clamp(time, 0f, 1f);
        
        float completion = Mathf.Sqrt(time);
        GetComponent<CapsuleCollider>().height = 128f * completion;
        beam.transform.localScale = new Vector3(2f, 2f + completion * 126f, 2f);
        glass.transform.localScale = Vector3.one * 2.5f * (1f - completion);
        
        if (timer < 0f && active)
        {
            Lock();
        }
        else if (timer < 0f && !active)
        {
            Unlock();
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (!collider.gameObject.CompareTag("Player")) return;

        // Game game = Camera.main.GetComponent<Game>();
        // game.CompleteStage();

    }
}
