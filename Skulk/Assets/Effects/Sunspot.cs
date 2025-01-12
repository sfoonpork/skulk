using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sunspot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    float time = 0f;
    float duration = 1f;
    float scale = 4f;
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        float completion = time / duration;
        if (completion > 1f) Destroy(gameObject);
        float radius = (-Mathf.Cos(2 * Mathf.PI * completion) + 1f)/2f * scale;
        transform.localScale = Vector3.one * radius;
    }
}
