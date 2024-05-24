using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] AudioClip boxHitAudio;
    [SerializeField] Rigidbody2D[] boxParts;
    [SerializeField] Vector2 maxBoxForce;
    [SerializeField] Vector2 minBoxForce;
    [SerializeField] float angular = 10f;
    SpriteRenderer spriteRenderer;
    Collider2D _collider;
    Rigidbody2D _boxRB;
    [SerializeField] GameObject boxEffect;
    //[SerializeField] float collisionSpeed;
    //[SerializeField] Vector2 colDir;
    //[SerializeField] float onTopHitTolerance = 0.9f;
    //[SerializeField] float onTopSpeedLimit = 10f;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        _boxRB = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        colDir = (collision.gameObject.transform.position - transform.position).normalized;
    //        if (collision.relativeVelocity.magnitude >= onTopSpeedLimit && colDir.y > onTopHitTolerance)
    //        {
    //            HitBox(0);
    //            collisionSpeed = collision.relativeVelocity.magnitude;
                
    //        }
    //    }
    //}

    public void HitBox(int _hitDir)
    {
        GameObject _effect = Instantiate(boxEffect,transform.position,transform.rotation);
        Destroy(_effect,0.4f);
        //audioSource.PlayOneShot(boxHitAudio);
        foreach (Rigidbody2D part in boxParts) 
        {
            _collider.enabled = false;
            _boxRB.bodyType = RigidbodyType2D.Static;
            spriteRenderer.enabled = false;
            part.gameObject.SetActive(true);
            Vector2 _dir = new Vector2(Random.Range(minBoxForce.x,maxBoxForce.x) * _hitDir, Random.Range(minBoxForce.y, maxBoxForce.y));
            part.AddForce(_dir, ForceMode2D.Impulse);
            part.angularVelocity = angular * _dir.x;
        }
    }

}
