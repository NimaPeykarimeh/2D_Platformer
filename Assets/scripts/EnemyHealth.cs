using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] int health;
    [SerializeField] int maxHealth;
    [SerializeField] Rigidbody2D rb2;
    [SerializeField] Collider2D col;

    [SerializeField] float deadTimer;
    [SerializeField] float deadAnimmDuration = 0.3f;
    [SerializeField] bool isAnimPlaying;

    private void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        if (isAnimPlaying)
        {
            deadTimer -= Time.deltaTime;
            if (deadTimer <= 0)
            {
                Dead();
            }
        }
    }

    public void GetDamage(int _damage = 1)
    {
        health -= _damage;
        if (health <= 0)
        {
            ToggleDead();
        }
        else
        {
            animator.SetTrigger("Damage");
        }
    }

    void ToggleDead()
    {
        isAnimPlaying = true;
        deadTimer = deadAnimmDuration;
        animator.SetBool("Dead",true);

    }
    void Dead()
    {
        rb2.bodyType = RigidbodyType2D.Kinematic;
        rb2.velocity = Vector2.zero;
        col.enabled = false;
        isAnimPlaying = false;
    }
}
