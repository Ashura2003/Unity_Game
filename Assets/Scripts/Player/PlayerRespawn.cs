using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpointSound;
    private Transform currentCheckPoint;
    private Health health;
    private UIManager ui;

    private void Awake()
    {
        health = GetComponent<Health>();
        ui = FindAnyObjectByType<UIManager>();
    }

    public void RespawnPlayer()
    {
        // Check if the player has a checkpoint
        if(currentCheckPoint == null)
        {
            //Show game over screen
            ui.GameOver();

            return;
        }

        // Respawn the player

        transform.position = currentCheckPoint.position; // Move player to the checkpoint
        health.Respawn(); // Reset player health

        // Move Camera to the checkpoint 
        Camera.main.GetComponent<CameraController>().MoveToNewRoom(currentCheckPoint.parent);

    }

    // Activate the checkpoint
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Checkpoint"))
        {
            currentCheckPoint = collision.transform; // Set the current checkpoint
            SoundManager.instance.PlaySound(checkpointSound);
            collision.GetComponent<Collider2D>().enabled = false; // Disable the checkpoint
            collision.GetComponent<Animator>().SetTrigger("Appear"); // Play the animation
        }
    }
}
