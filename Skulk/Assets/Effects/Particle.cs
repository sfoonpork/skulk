using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public float duration;
    Color color;
    void Start()
    {
        color = GetComponent<Renderer>().material.color;
    }
    
    float time;
    void Update()
    {
        time += Time.deltaTime;
        float completion = Mathf.Clamp(time / duration, 0f, 1f);
        Color newColor = new Color(color.r, color.g, color.b, 1f - completion);
        GetComponent<Renderer>().material.color = newColor;
        if (completion >= 1f) Destroy(gameObject);
    }
}
