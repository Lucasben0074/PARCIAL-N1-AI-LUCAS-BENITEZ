using UnityEngine;

public class GatherState : State
{
    private HunterAgent hunter;
    private BoidAgent target;

    private float gatherTimer;

    private const float GATHER_DISTANCE = 1.5f;
    private const float GATHER_TIME = 3f;

    public GatherState(
        StateMachine stateMachine,
        HunterAgent hunter
    ) : base(stateMachine)
    {
        this.hunter = hunter;
    }

    public override void Enter()
    {
        target = hunter.Perception.GetClosestDeadBoid();

        gatherTimer = 0f;

        Debug.Log("Hunter entra en GATHER");
    }

    public override void Update()
    {
        if (hunter == null)
            return;

        // El objetivo dejo de estar disponible
        if (target == null || !target.gameObject.activeSelf)
        {
            StateMachine.ChangeState(HunterStates.Patrol);
            return;
        }

        BoidHealth health = target.GetComponent<BoidHealth>();

        // Si por algon motivo ya no esta muerto,
        // cancelamos Gather.
        if (health == null || !health.IsDead)
        {
            StateMachine.ChangeState(HunterStates.Patrol);
            return;
        }

        // Si el cadaver salio de percepcion
        // cancelamos Gather
        if (!IsTargetInVision())
        {
            StateMachine.ChangeState(HunterStates.Patrol);
            return;
        }

        float distance = Vector3.Distance(
            hunter.transform.position,
            target.transform.position
        );

        // Todaviia no llego al cadaver
        if (distance > GATHER_DISTANCE)
        {
            Vector3 direction =
                target.transform.position -
                hunter.transform.position;

            direction.y = 0;

            hunter.SetDesiredDirection(direction);

            gatherTimer = 0f;

            return;
        }

        // Ya llego
        hunter.StopMovement();

        gatherTimer += Time.deltaTime;

        if (gatherTimer >= GATHER_TIME)
        {
            health.Collect();

            Debug.Log("Hunter recolectó " + target.name);

            StateMachine.ChangeState(HunterStates.Patrol);
        }
    }

    public override void Exit()
    {
        target = null;
        gatherTimer = 0f;
    }

    private bool IsTargetInVision()
    {
        foreach (BoidAgent boid in hunter.Perception.GetBoidsInVision())
        {
            if (boid == target)
                return true;
        }

        return false;
    }
}