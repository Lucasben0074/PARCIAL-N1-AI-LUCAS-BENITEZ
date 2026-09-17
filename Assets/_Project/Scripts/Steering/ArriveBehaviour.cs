using UnityEngine;

public class ArriveBehaviour : SteeringBehaviour
{
    [SerializeField] private InterestObjectSensor sensor;
    [SerializeField] private SeparationBehaviour separation;

    [Header("Arrive")]
    [SerializeField] private float slowingRadius = 3f;
    [SerializeField] private float stopRadius = 1f;

    [Header("Separation")]
    [SerializeField] private float separationWeight = 1.5f;

    public override Vector3 GetDirection()
    {
        InterestObject target = sensor.GetClosestObject();

        if (target == null)
            return Vector3.zero;

        Vector3 direction =
            target.transform.position - transform.position;

        direction.y = 0;

        float distance = direction.magnitude;

        Vector3 arriveDirection = Vector3.zero;

        
        if (distance > stopRadius)
        {
            if (distance >= slowingRadius)
            {
                arriveDirection = direction.normalized;
            }
            else
            {
                float speedFactor =
                    distance / slowingRadius;

                arriveDirection =
                    direction.normalized * speedFactor;
            }
        }

        
        Vector3 separationDirection =
            separation.GetDirection();

        
        Vector3 finalDirection =
            arriveDirection +
            separationDirection * separationWeight;

        return Vector3.ClampMagnitude(
            finalDirection,
            1f
        );
    }
}