using UnityEngine;

public class SpikeHead : Enemy_Damage // Will damage the player everytime they touch
{
    [Header("Spike Head Attributes")]
    [SerializeField] private float speed;
    [SerializeField] private float range;
    [SerializeField] private float checkDelay;
    [SerializeField] private LayerMask playerLayer;
    private float checkTimer;
    private Vector3 destination; // The direction the spike head will move in
    private bool attacking;
    private Vector3[] directions = new Vector3[4]; // The four directions the spike head can move in

    [Header("Sound Parameters")]
    [SerializeField] private AudioClip impactSound;

    private void OnEnable()
    {
        Stop();
    }

    private void Update()
    {
        // If the player is within range, the spike head will move towards the player
        if (attacking)
        {
        transform.Translate(destination * Time.deltaTime * speed);
        }
        else
        {
            checkTimer += Time.deltaTime;
            if (checkTimer > checkDelay)
                CheckForPlayer();
        }
    }

    private void CheckForPlayer()
    {
        // Check if the player is within range
        calculateDirections();

        // Check if spike head sees player in all four directions
        for (int i = 0; i < directions.Length; i++)
        {
            Debug.DrawRay(transform.position, directions[i], Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i], range, playerLayer);

            if (hit.collider != null)
            {
                attacking = true;
                destination = directions[i];
                checkTimer = 0;
            }
        }
    }

    private void calculateDirections()
    {
        directions[0] = transform.right * range; // Checks to the right direction
        directions[1] = -transform.right * range; // Checks to the left direction
        directions[2] = transform.up * range; // Checks to the up direction
        directions[3] = -transform.up * range; // Checks to the down direction
    }

    private void Stop()
    {
        destination = transform.position;
        attacking = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SoundManager.instance.PlaySound(impactSound);
        base.OnTriggerEnter2D(collision); // Execute the parent class method
        Stop();
    }
}
