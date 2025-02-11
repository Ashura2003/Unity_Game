using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Image totalHeathBar;
    [SerializeField] private Image currentHealthBar;

    private void Start()
    {
        totalHeathBar.fillAmount = playerHealth.currentHealth / 10;
    }
    private void Update()
    {
        currentHealthBar.fillAmount = playerHealth.currentHealth / 10;
    }
}
