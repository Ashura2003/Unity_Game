using System.Collections;
using UnityEngine;

public class Firetrap : MonoBehaviour
{
    [SerializeField] private float damage;
    [Header ("Firetrap Timers")]
    [SerializeField] private float activationDelay;
    [SerializeField] private float activeTime;
    private Animator anim;
    private SpriteRenderer sr;

    private Health playerHealth;

    private bool triggered; // This checks if the trap is triggered or not
    private bool active; // This checks if the trap is active or not

    [Header("Sound Parameters")]
    [SerializeField] private AudioClip firetrapSound;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerHealth = collision.GetComponent<Health>();
            if (!triggered)
            {
                // Triger the firetrap
                StartCoroutine(ActivateFiretrap());
            }
            if (active)
            {
                collision.GetComponent<Health>().takeDame(damage);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerHealth = null;
        }
    }

    private void Update()
    {
        if(playerHealth != null && active)
        {
            playerHealth.takeDame(damage);
        }
    }

    private IEnumerator ActivateFiretrap()
    {
        triggered = true;
        sr.color = Color.red; // Changes the color of the firetrap to red to help players see the trap is triggered

        // Wait for the activation delay
        yield return new WaitForSeconds(activationDelay);
        SoundManager.instance.PlaySound(firetrapSound);
        sr.color = Color.white; // Changes the color of the firetrap back to white

        // Activate the firetrap
        active = true;
        anim.SetBool("Active", true);

        // Wait for the active time
        yield return new WaitForSeconds(activeTime);

        // Deactivate the firetrap
        active = false;
        triggered = false;
        anim.SetBool("Active", false);
    }
}
