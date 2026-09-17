using UnityEngine;

public class PatrolState : State
{
    private HunterAgent hunter;

    private int currentWaypoint;

    private const float WAYPOINT_DISTANCE = 1f;

    private float spawnTimer;

    private const float SPAWN_INTERVAL = 5f;
    private const int MAX_INTEREST_OBJECTS = 5;
    public PatrolState(
        StateMachine stateMachine,
        HunterAgent hunter
    ) : base(stateMachine)
    {
        this.hunter = hunter;
    }

    public override void Enter()
    {
    }

    public override void Update()
    {
        if (hunter == null)
            return;

        UpdateInterestObjectSpawn();

        BoidAgent deadBoid =
    hunter.Perception.GetClosestDeadBoid();

        if (deadBoid != null)
        {
            StateMachine.ChangeState(
                HunterStates.Gather
            );

            return;
        }

        if (hunter.CanAttack)
        {
            BoidAgent detectedBoid =
                hunter.Perception.GetClosestBoid();

            if (detectedBoid != null)
            {
                StateMachine.ChangeState(
                    HunterStates.Attack
                );

                return;
            }
        }

        Transform[] waypoints = hunter.Waypoints;

        if (waypoints == null || waypoints.Length == 0)
            return;

        Transform target =
            waypoints[currentWaypoint];

        if (target == null)
            return;

        Vector3 direction =
            target.position -
            hunter.transform.position;

        direction.y = 0;

        hunter.SetDesiredDirection(direction);

        float distance = Vector3.Distance(
            hunter.transform.position,
            target.position
        );

        if (distance <= WAYPOINT_DISTANCE)
        {
            currentWaypoint++;

            if (currentWaypoint >= waypoints.Length)
            {
                currentWaypoint = 0;
            }
        }
    }
    private void UpdateInterestObjectSpawn()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer < SPAWN_INTERVAL)
            return;

        spawnTimer = 0f;

        if (InterestObject.ActiveObjects >= MAX_INTEREST_OBJECTS)
            return;

        hunter.SpawnInterestObject();
    }
    public override void Exit()
    {
    }
}