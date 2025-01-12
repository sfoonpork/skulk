using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{

    public float health;
    private GameObject healthObject;
    private RectTransform bar;
    private Spawner spawner;

    public void Damage(float amount)
    {
        health -= amount;
        Visual visual = Camera.main.GetComponent<Visual>();
        visual.Damage(amount);
        amount = Mathf.Abs(amount);
        GameObject globalSoundsObject = GameObject.Find("GlobalSounds");
        globalSoundsObject.transform.position = gameObject.transform.position;
        AudioSource damage = globalSoundsObject.GetComponent<AudioSource>();
        if (amount == null) amount = 0f;
        damage.pitch = 1f/(amount / 25f);
        if (health < 0f) damage.pitch = 0.125f;
        damage.Play();

    }
    
    // Start is called before the first frame update
    void Start()
    {
        healthObject = Camera.main.transform.Find("Canvas").Find("Health").gameObject;
        spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();
        health = 100f;
        bar = healthObject.GetComponent<RectTransform>();
    }

    void Die()
    {
        spawner.SpawnParticles(gameObject, 100);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        health += Time.deltaTime * 4f;
        bar.sizeDelta = new Vector2(health / 100f * 512f, 64f);
        if (health <= 0f)
        {
            Die();
        }
        if (health > 100f)
        {
            health = 100f;
        }
    }
}
