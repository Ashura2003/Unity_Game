using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    // Serialized variables
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Parameters")]
    [SerializeField] private LayerMask playerLayer;

    [Header("Sound Parameters")]
    [SerializeField] private AudioClip attackSound;

    // Reference variables
    private Animator anim;
    private Health playerHealth;


    // Private variables
    private float cooldownTimer = Mathf.Infinity;
    // Reference to the enemy patrol script
    private EnemyPatrol enemyPatrol;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }
    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        // Atack only when player is in sight
        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown && playerHealth.currentHealth > 0)
            {
                // Atack the player
                cooldownTimer = 0;
                anim.SetTrigger("melee");
                SoundManager.instance.PlaySound(attackSound);
            }
        }

        if(enemyPatrol != null)
        {
            enemyPatrol.enabled = !PlayerInSight();
        }
        
    }

    private bool PlayerInSight()
    {
        // Check if player is in sight

        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if(hit.collider != null)
        {
            playerHealth = hit.collider.GetComponent<Health>();
        }

        return hit.collider != null;       
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
           new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    public void DamagePlayer()
    {
        if (PlayerInSight())
        {
            // Damage the player health
            playerHealth.takeDame(damage);

        }
    }
}
