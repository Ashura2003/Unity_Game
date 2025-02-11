using UnityEngine;

public class Rooms : MonoBehaviour
{
    [SerializeField]private GameObject[] enemies;
    private Vector3[] initialPosition;

    private void Awake()
    {
        // Save the initial position of all enemies in the room
        initialPosition = new Vector3[enemies.Length];
        for (int i = 0; i < enemies.Length; i++)
        {
            if(enemies[i] != null)
            {
                initialPosition[i] = enemies[i].transform.position;
            }
        }
    }

    public void ActivateRoom(bool _status)
    {
        // Activate or Deactivate the enemies in the room
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                enemies[i].SetActive(_status);
                enemies[i].transform.position = initialPosition[i];
            }
        }
    }
}
