using UnityEngine;

public class AttackState : State
{
    private HunterAgent hunter;
    private BoidAgent target;

    public AttackState(
        StateMachine stateMachine,
        HunterAgent hunter
    ) : base(stateMachine)
    {
        this.hunter = hunter;
    }

    public override void Enter()
    {
        target = hunter.Perception.GetClosestBoid();

        Debug.Log("Hunter entra en ATTACK");
    }

    public override void Update()
    {
        if (hunter == null)
            return;

        // Si ya no existe el objetivo, volvemos a Patrol.
        if (target == null)
        {
            StateMachine.ChangeState(HunterStates.Patrol);
            return;
        }

        BoidHealth health = target.GetComponent<BoidHealth>();

        // Si muria, termina Attack.
        if (health == null || health.IsDead)
        {
            StateMachine.ChangeState(HunterStates.Patrol);
            return;
        }

        // Si salio de la percepcion, dejamos de perseguirlo.
        if (!IsTargetInVision())
        {
            StateMachine.ChangeState(HunterStates.Patrol);
            return;
        }

        float distance = Vector3.Distance(
            hunter.transform.position,
            target.transform.position
        );

        // Si esta muy cerca - melee.
        if (distance <= hunter.MeleeAttackRadius)
        {
            hunter.StopMovement();

            hunter.MeleeAttack(target);

            StateMachine.ChangeState(HunterStates.Patrol);
            return;
        }

        // Si esta dentro del rango de ataque - ataque a distancia.
        if (distance <= hunter.RangeAttackRadius)
        {
            hunter.StopMovement();

            hunter.RangeAttack(target);

            StateMachine.ChangeState(HunterStates.Patrol);
            return;
        }

        // Lo ve, pero esta fuera de los rangos de ataque:
        // perseguir
        Vector3 direction =
            target.transform.position -
            hunter.transform.position;

        direction.y = 0;

        hunter.SetDesiredDirection(direction);
    }

    public override void Exit()
    {
        target = null;
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