using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header ("Patrol Points")]
    [SerializeField]private Transform leftEdge;
    [SerializeField]private Transform rightEdge;

    [Header("Enemy")]
    [SerializeField] private Transform enemy;
    
    [Header("Movement parameters")]
    [SerializeField] private float speed;
    private Vector3 initScale;
    private bool movingLeft;

    [Header("Idle parameters")]
    [SerializeField] private float idleDuration;
    private float idleTimer;

    [Header("Enemy Animator")]
    [SerializeField]private Animator anim;

    private void Awake()
    {
        initScale = enemy.localScale;
    }

    private void Update()
    {
        if(movingLeft)
        {
            if(enemy.position.x >= leftEdge.position.x)
            MoveInDirection(-1);
            else
            {
                // Change direction
                ChangeDirection();
            }
        }
        else
        {
            if (enemy.position.x <= rightEdge.position.x)
                MoveInDirection(1);
            else
            {
                // Change direction
                ChangeDirection();
            }

        }
    }

    private void ChangeDirection()
    {
        anim.SetBool("Moving", false);

        idleTimer += Time.deltaTime;

        if (idleTimer >= idleDuration)
        {
            
            movingLeft = !movingLeft;
        }

       
    }

    private void OnDisable()
    {
        anim.SetBool("Moving", false);
    }

    private void MoveInDirection(int _direction)
    {
        idleTimer = 0;
        anim.SetBool("Moving", true);
        // Make enemy face the direction
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction, initScale.y, initScale.z);

        // Make the enemy move to the direction
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed, enemy.position.y , enemy.position.z);
    }
}
