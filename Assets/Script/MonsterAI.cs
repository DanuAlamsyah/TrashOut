using UnityEngine;

public class MonsterAI : MonoBehaviour
{
    public enum MonsterState
    {
        Idle,
        Wander,
        Chase,
        Attack
    }

    [Header("Target")]
    public Transform player;

    [Header("Movement")]
    public float wanderSpeed = 1.2f;
    public float chaseSpeed = 2.5f;
    public float rotationSpeed = 8f;

    [Header("Area")]
    public float wanderRadius = 5f;
    public float detectionRadius = 7f;
    public float attackRange = 1.5f;

    [Header("Wander")]
    public float idleTime = 1.5f;
    public float wanderChangeTime = 3f;

    [Header("Attack")]
    public int attackDamage = 1;
    public float attackCooldown = 1.2f;

    private MonsterState currentState;
    private Animator animator;

    private Vector3 startPosition;
    private Vector3 wanderTarget;

    private float stateTimer;
    private float attackTimer;

    private PlayerHealth playerHealth;

    void Start()
    {
        animator = GetComponent<Animator>();

        startPosition = transform.position;
        PickNewWanderTarget();

        FindPlayer();

        currentState = MonsterState.Wander;
    }

    void Update()
    {
        if (player == null || playerHealth == null)
        {
            FindPlayer();

            if (player == null || playerHealth == null)
            {
                SetMoving(false);
                return;
            }
        }

        if (playerHealth.isDead)
        {
            SetMoving(false);
            currentState = MonsterState.Idle;
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            currentState = MonsterState.Attack;
        }
        else if (distanceToPlayer <= detectionRadius)
        {
            currentState = MonsterState.Chase;
        }
        else
        {
            if (currentState == MonsterState.Chase || currentState == MonsterState.Attack)
            {
                currentState = MonsterState.Wander;
                PickNewWanderTarget();
            }
        }

        switch (currentState)
        {
            case MonsterState.Idle:
                HandleIdle();
                break;

            case MonsterState.Wander:
                HandleWander();
                break;

            case MonsterState.Chase:
                HandleChase();
                break;

            case MonsterState.Attack:
                HandleAttack();
                break;
        }
    }

    void FindPlayer()
    {
        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();

            if (playerHealth == null)
            {
                playerHealth = player.GetComponentInParent<PlayerHealth>();
            }

            if (playerHealth == null)
            {
                playerHealth = player.GetComponentInChildren<PlayerHealth>();
            }

            if (playerHealth != null)
            {
                player = playerHealth.transform;
                return;
            }
        }

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
            playerHealth = playerObject.GetComponent<PlayerHealth>();

            if (playerHealth == null)
            {
                playerHealth = playerObject.GetComponentInParent<PlayerHealth>();
            }

            if (playerHealth == null)
            {
                playerHealth = playerObject.GetComponentInChildren<PlayerHealth>();
            }

            if (playerHealth != null)
            {
                player = playerHealth.transform;
                Debug.Log("Monster menemukan PlayerHealth pada: " + playerHealth.gameObject.name);
            }
            else
            {
                Debug.LogWarning("Player ditemukan, tapi PlayerHealth tidak ada pada object Player.");
            }
        }
        else
        {
            Debug.LogWarning("Monster tidak menemukan object dengan Tag Player.");
        }
    }

    void HandleIdle()
    {
        SetMoving(false);

        stateTimer += Time.deltaTime;

        if (stateTimer >= idleTime)
        {
            stateTimer = 0f;
            currentState = MonsterState.Wander;
            PickNewWanderTarget();
        }
    }

    void HandleWander()
    {
        SetMoving(true);
        MoveTo(wanderTarget, wanderSpeed);

        float distance = Vector3.Distance(transform.position, wanderTarget);

        stateTimer += Time.deltaTime;

        if (distance <= 0.3f || stateTimer >= wanderChangeTime)
        {
            stateTimer = 0f;
            currentState = MonsterState.Idle;
        }
    }

    void HandleChase()
    {
        SetMoving(true);
        MoveTo(player.position, chaseSpeed);
    }

    void HandleAttack()
    {
        SetMoving(false);
        RotateTo(player.position);

        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCooldown)
        {
            attackTimer = 0f;

            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
                Debug.Log(gameObject.name + " menyerang player. Damage: " + attackDamage);
            }
            else
            {
                Debug.LogWarning("Monster gagal menyerang: PlayerHealth masih null.");
            }
        }
    }

    void MoveTo(Vector3 targetPosition, float speed)
    {
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0f;

        if (direction.magnitude <= 0.05f) return;

        direction.Normalize();

        transform.position += direction * speed * Time.deltaTime;

        RotateTo(targetPosition);
    }

    void RotateTo(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0f;

        if (direction.magnitude <= 0.05f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    void PickNewWanderTarget()
    {
        Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;

        wanderTarget = startPosition + new Vector3(
            randomCircle.x,
            0f,
            randomCircle.y
        );
    }

    void SetMoving(bool moving)
    {
        if (animator != null)
        {
            animator.SetBool("isMoving", moving);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(Application.isPlaying ? startPosition : transform.position, wanderRadius);
    }
}