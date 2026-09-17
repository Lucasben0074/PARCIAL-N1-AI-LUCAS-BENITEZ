using System.Collections.Generic;
using UnityEngine;

public class CohesionBehaviour : SteeringBehaviour
{
    [SerializeField] private BoidSensor sensor;

    public override Vector3 GetDirection()
    {
        List<BoidAgent> neighbours = sensor.GetNearbyBoids();

        if (neighbours.Count == 0)
            return Vector3.zero;

        Vector3 center = Vector3.zero;

        foreach (BoidAgent neighbour in neighbours)
        {
            center += neighbour.transform.position;
        }

        center /= neighbours.Count;

        Vector3 direction = center - transform.position;
        direction.y = 0;

        if (direction == Vector3.zero)
            return Vector3.zero;

        return direction.normalized;
    }
}