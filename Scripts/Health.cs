using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Health : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public int maxHealth = 10;
    [HideInInspector]public int health;
    public Color affectColor;
    public float affectTime = .1f;
    private bool isAffected;

    public GameObject particles;

    public float safeTime = 0;
    private void Start()
    {
        health = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        isAffected = false;
    }

    private void Update()
    {
        safeTime -= Time.deltaTime;
    }

    public void TakeDamage(int damage)
    {
        if (safeTime > 0f) { print("blocked"); return; }
        health -= damage;
        if (health <= 0) Death();
        if(!isAffected)
        StartCoroutine(affect());
    }

    private void Death()
    {
        if(particles)
        Instantiate(particles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    IEnumerator affect()
    {
        isAffected = true;
        var normalColor = spriteRenderer.color;
        spriteRenderer.color = affectColor;
        yield return new WaitForSeconds(affectTime);
        spriteRenderer.color = normalColor;
        isAffected = false;
        yield break;
    }
}
