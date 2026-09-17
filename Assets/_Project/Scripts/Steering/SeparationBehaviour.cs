using System.Collections.Generic;
using UnityEngine;

public class SeparationBehaviour : SteeringBehaviour
{
    [SerializeField] private BoidSensor sensor;
    [SerializeField] private float separationRadius = 2f;

    public override Vector3 GetDirection()
    {
        List<BoidAgent> neighbours = sensor.GetNearbyBoids();

        Vector3 separationDirection = Vector3.zero;

        foreach (BoidAgent neighbour in neighbours)
        {
            Vector3 difference =
                transform.position - neighbour.transform.position;

            difference.y = 0;

            float distance = difference.magnitude;

            if (distance > 0 && distance < separationRadius)
            {
                separationDirection +=
                    difference.normalized / distance;
            }
        }

        if (separationDirection == Vector3.zero)
            return Vector3.zero;

        return separationDirection.normalized;
    }
}