using UnityEngine;

public enum HunterStates
{
    Patrol,
    Attack,
    Gather
}

public class HunterAgent : MonoBehaviour
{
    [Header("Interest Objects")]
    [SerializeField] private GameObject interestObjectPrefab;

    [Header("Projectile")]
    [SerializeField] private ProjectilePool projectilePool;
    [SerializeField] private Transform firePoint;

    [Header("Movement")]
    [SerializeField] private float maxSpeed = 4f;
    [SerializeField] private float maxSteering = 3f;

    [Header("Patrol")]
    [SerializeField] private Transform[] waypoints;

    [Header("Perception")]
    [SerializeField] private HunterPerception perception;

    [Header("Attack")]
    [SerializeField] private float TBA = 3f;
    [SerializeField] private float rangeAttackRadius = 6f;
    [SerializeField] private float meleeAttackRadius = 2f;
    [SerializeField] private float meleeDamage = 50f;
    [SerializeField] private float rangeDamage = 25f;

    [Header("Visual Feedback")]
    [SerializeField] private HunterStateFeedback stateFeedback;

    private float attackTimer;

    private Vector3 velocity;

    private StateMachine stateMachine;

    public Vector3 Velocity => velocity;
    public Transform[] Waypoints => waypoints;
    public HunterPerception Perception => perception;

    public float RangeAttackRadius => rangeAttackRadius;
    public float MeleeAttackRadius => meleeAttackRadius;

    public bool CanAttack => attackTimer <= 0f;

    private void Start()
    {
        stateMachine = new StateMachine();

        PatrolState patrolState =
            new PatrolState(stateMachine, this);

        AttackState attackState =
            new AttackState(stateMachine, this);

        GatherState gatherState =
            new GatherState(stateMachine, this);

        stateMachine.RegisterState(
            HunterStates.Patrol,
            patrolState
        );

        stateMachine.RegisterState(
            HunterStates.Attack,
            attackState
        );

        stateMachine.RegisterState(
            HunterStates.Gather,
            gatherState
        );

        stateMachine.ChangeState(
            HunterStates.Patrol
        );
    }

    private void Update()
    {
        if (stateMachine == null)
            return;

        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
        }

        stateMachine.Update();

        // FEEDBACK VISUAL
        // Solo observa el estado actual de la FSM.
        if (stateFeedback != null)
        {
            stateFeedback.UpdateFeedback(
                stateMachine.CurrentState
            );
        }

        Move();
    }

    public void SetDesiredDirection(Vector3 direction)
    {
        Vector3 desiredVelocity =
            direction.normalized * maxSpeed;

        Vector3 steering =
            desiredVelocity - velocity;

        steering = Vector3.ClampMagnitude(
            steering,
            maxSteering * Time.deltaTime
        );

        velocity += steering;

        velocity = Vector3.ClampMagnitude(
            velocity,
            maxSpeed
        );
    }

    public void StopMovement()
    {
        velocity = Vector3.zero;
    }

    private void Move()
    {
        transform.position +=
            velocity * Time.deltaTime;

        if (velocity != Vector3.zero)
        {
            transform.forward =
                velocity.normalized;
        }
    }

    public void ResetAttackTimer()
    {
        attackTimer = TBA;
    }

    public void MeleeAttack(BoidAgent target)
    {
        if (target == null)
            return;

        BoidHealth health =
            target.GetComponent<BoidHealth>();

        if (health == null)
            return;

        health.TakeDamage(meleeDamage);

        Debug.Log(
            "Hunter realizó MELEE a " +
            target.name
        );

        ResetAttackTimer();
    }

    public void RangeAttack(BoidAgent target)
    {
        if (target == null)
            return;

        BoidHealth health =
            target.GetComponent<BoidHealth>();

        if (health == null)
            return;

        // El daño del ataque sigue siendo instantáneo.
        health.TakeDamage(rangeDamage);

        // Proyectil únicamente como feedback visual.
        if (projectilePool != null &&
            firePoint != null)
        {
            HunterProjectile projectile =
                projectilePool.GetProjectile();

            if (projectile != null)
            {
                projectile.transform.position =
                    firePoint.position;

                Vector3 direction =
                    target.transform.position -
                    firePoint.position;

                direction.y = 0;

                if (direction != Vector3.zero)
                {
                    projectile.transform.rotation =
                        Quaternion.LookRotation(direction);
                }

                projectile.gameObject.SetActive(true);

                projectile.Initialize(
                    target.transform
                );
            }
        }

        Debug.Log(
            "Hunter realizó ATAQUE A DISTANCIA a " +
            target.name
        );

        ResetAttackTimer();
    }

    public void SpawnInterestObject()
    {
        if (interestObjectPrefab == null)
            return;

        Instantiate(
            interestObjectPrefab,
            transform.position,
            Quaternion.identity
        );
    }

    private void OnDisable()
    {
        stateMachine = null;
    }
}