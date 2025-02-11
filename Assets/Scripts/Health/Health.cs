using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header ("Health")]
    [SerializeField]private float startingHealth;
    public float currentHealth { get; private set; }

    private Animator anim;

    private bool dead;

    [Header("IFrames")]
    [SerializeField] private float invincibleDuration;
    [SerializeField] private int flashes;
    private SpriteRenderer sr;

    [Header("Components")]
    [SerializeField]private Behaviour[] components;

    [Header("Audio Manager")]
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip deathSound;

    private bool invulnerable;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void takeDame(float _damage)
    {
        if (invulnerable) return;
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0 , startingHealth);
        
        if(currentHealth > 0)
        {
            // Take Damage
            anim.SetTrigger("Hurt");
            SoundManager.instance.PlaySound(hurtSound);

            // IFrames or invincibility frames
            StartCoroutine(Invulnerability());
        }
        else
        {
            //Player Dead
            if (!dead)
            {

                //// Disable player movement
                //if(GetComponent<PlayerMovement>() != null)
                //    GetComponent<PlayerMovement>().enabled = false;

                //// Disable enemy patrol
                //if (GetComponentInParent<EnemyPatrol>() != null)
                //    GetComponentInParent<EnemyPatrol>().enabled = false;

                //if (GetComponent<MeleeEnemy>() != null)
                //    GetComponent<MeleeEnemy>().enabled = false;

                foreach (Behaviour comp in components)
                {
                    comp.enabled = false;
                
                    anim.SetBool("Ground", true);
                    anim.SetTrigger("Dead");
                }
                dead = true;

                SoundManager.instance.PlaySound(deathSound);
            }
        }
    }

    public void Respawn()
    {
        dead = false;
        addHealth(startingHealth);
        anim.ResetTrigger("Dead");
        anim.Play("Player_Idle");
        StartCoroutine(Invulnerability());

        foreach (Behaviour comp in components)
        {
            comp.enabled = true;
        }
    }
    

    public void addHealth(float _health)
    {
        currentHealth = Mathf.Clamp(currentHealth + _health, 0, startingHealth);
    }

    private IEnumerator Invulnerability()
    {
        invulnerable = true;
        // Ignore collision between player and enemy
        Physics2D.IgnoreLayerCollision(10, 11, true);

        for (int i = 0; i < flashes; i++)
        {
            sr.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(invincibleDuration / (flashes * 2));
            sr.color = Color.white;
            yield return new WaitForSeconds(invincibleDuration / (flashes * 2));
        }

        // Turn off Invulnerability
        Physics2D.IgnoreLayerCollision(10, 11, false);
        invulnerable = false;
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
