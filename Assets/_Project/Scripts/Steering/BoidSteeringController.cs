using UnityEngine;

public class BoidSteeringController : SteeringBehaviour
{
    [SerializeField] private HunterSensor hunterSensor;
    [SerializeField] private FlockingBehaviour flocking;
    [SerializeField] private EvadeBehaviour evade;

    public override Vector3 GetDirection()
    {
        Transform hunter = hunterSensor.GetHunter();

        if (hunter != null)
        {
            return evade.GetDirection();
        }

        return flocking.GetDirection();
    }
}