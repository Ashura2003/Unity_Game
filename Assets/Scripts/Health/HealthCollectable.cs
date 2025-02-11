using UnityEngine;

public class HealthCollevtable : MonoBehaviour
{
    [SerializeField] private float healthValue;

    [Header("Sound Parameters")]
    [SerializeField] private AudioClip healthSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            SoundManager.instance.PlaySound(healthSound);
            collision.GetComponent<Health>().addHealth(healthValue);
            gameObject.SetActive(false);
        }
    }
}
