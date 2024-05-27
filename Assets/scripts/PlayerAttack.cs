using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] Animator animator;
    [SerializeField] Transform attackCenter;
    [SerializeField] float attackRadius = 0.5f;
    [SerializeField] LayerMask attackLayer;
    [SerializeField] AudioClip boxCrashSound;
    [SerializeField] AudioClip hammerWhipSound;
    [SerializeField] AudioClip enemyHitSound;
    [SerializeField] Vector2 enemyHitForce;
    [SerializeField] KeyCode attacKey;
    [Header("SHAKE CAMERA")]
    [SerializeField] float boxShakeDuration = 0.2f;
    [SerializeField] float boxShakeFreq = 1f;
    [SerializeField] float boxShakeMagni = 1f;

    [Header("Attack Timer")]
    [SerializeField] float attackCooldown = 0.3f;
    [SerializeField] float attackTimer;
    bool canAttack;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!canAttack)
        {
            attackTimer -= Time.deltaTime;
            canAttack = attackTimer <= 0;
        }

        if (Input.GetKeyDown(attacKey) && canAttack)
        {
            Attack();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackCenter.position, attackRadius);
    }

    void Attack()
    {
        attackTimer = attackCooldown;
        canAttack = false;
        animator.SetTrigger("Attack");
        audioSource.pitch = Random.Range(0.9f, 1.15f);
        audioSource.PlayOneShot(hammerWhipSound);
        bool isHitBox = false;
        bool isHitEnemy = false;
        Collider2D[] collider = Physics2D.OverlapCircleAll(attackCenter.position,attackRadius,attackLayer);
        if (collider.Length > 0)
        {
            foreach (Collider2D _collider in collider)
            {
                if (_collider.CompareTag("Enemy"))
                {
                    if (!isHitEnemy)
                    {
                        isHitEnemy = true;
                        audioSource.pitch = 0.9f;
                        audioSource.PlayOneShot(enemyHitSound);
                    }
                    float direction = Mathf.Sign(_collider.transform.position.x - transform.position.x);
                    _collider.GetComponent<Rigidbody2D>().AddForce(new Vector2(enemyHitForce.x * direction,enemyHitForce.y),ForceMode2D.Impulse);
                    _collider.GetComponent<EnemyHealth>().GetDamage();

                    print("ATTACK");
                }
                else if (_collider.CompareTag("Box"))
                {
                    if (!isHitBox)
                    {
                        isHitBox = true;
                        audioSource.pitch = Random.Range(0.9f,1.15f);
                        audioSource.PlayOneShot(boxCrashSound);
                    }
                    float direction = Mathf.Sign(_collider.transform.position.x - transform.position.x);
                    CameraShake.instance.ShakeCamera(boxShakeDuration, boxShakeMagni, boxShakeFreq);
                    _collider.GetComponent<Box>().HitBox((int)direction);
                }
            }

            
        }
        
    }
}
