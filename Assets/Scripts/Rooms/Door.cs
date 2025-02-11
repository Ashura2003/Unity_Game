using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]private Transform previousRoom;
    [SerializeField] private Transform nextRoom;
    [SerializeField] private CameraController cam;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (collision.transform.position.x < transform.position.x)
            {
                cam.MoveToNewRoom(nextRoom);
                nextRoom.GetComponent<Rooms>().ActivateRoom(true);
                previousRoom.GetComponent<Rooms>().ActivateRoom(false);
            }
            else
            {
                cam.MoveToNewRoom(previousRoom);
                previousRoom.GetComponent<Rooms>().ActivateRoom(true);
                nextRoom.GetComponent<Rooms>().ActivateRoom(false);
            }
        }
    }
}
