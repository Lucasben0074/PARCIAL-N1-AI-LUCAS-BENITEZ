using System.Collections.Generic;
using UnityEngine;

public class AlignmentBehaviour : SteeringBehaviour
{
    [SerializeField] private BoidSensor sensor;

    public override Vector3 GetDirection()
    {
        List<BoidAgent> neighbours = sensor.GetNearbyBoids();

        if (neighbours.Count == 0)
            return Vector3.zero;

        Vector3 averageVelocity = Vector3.zero;

        foreach (BoidAgent neighbour in neighbours)
        {
            averageVelocity += neighbour.Velocity;
        }

        averageVelocity /= neighbours.Count;
        averageVelocity.y = 0;

        if (averageVelocity == Vector3.zero)
            return Vector3.zero;

        return averageVelocity.normalized;
    }
}