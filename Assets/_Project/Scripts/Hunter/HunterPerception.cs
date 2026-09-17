using System.Collections.Generic;
using UnityEngine;

public class HunterPerception : MonoBehaviour
{
    [SerializeField] private float visionRadius = 8f;
    [SerializeField] private LayerMask boidLayer;

    public List<BoidAgent> GetBoidsInVision()
    {
        List<BoidAgent> detectedBoids = new List<BoidAgent>();

        Collider[] colliders = Physics.OverlapSphere(
            transform.position,
            visionRadius,
            boidLayer
        );

        foreach (Collider col in colliders)
        {
            BoidAgent boid = col.GetComponent<BoidAgent>();

            if (boid != null)
            {
                detectedBoids.Add(boid);
            }
        }

        return detectedBoids;
    }

    public BoidAgent GetClosestBoid()
    {
        List<BoidAgent> boids = GetBoidsInVision();

        BoidAgent closestBoid = null;
        float closestDistance = Mathf.Infinity;

        foreach (BoidAgent boid in boids)
        {
            BoidHealth health = boid.GetComponent<BoidHealth>();

            if (health == null || health.IsDead)
                continue;

            float distance = Vector3.Distance(
                transform.position,
                boid.transform.position
            );

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestBoid = boid;
            }
        }

        return closestBoid;
    }
    public BoidAgent GetClosestDeadBoid()
    {
        List<BoidAgent> boids = GetBoidsInVision();

        BoidAgent closestBoid = null;
        float closestDistance = Mathf.Infinity;

        foreach (BoidAgent boid in boids)
        {
            BoidHealth health = boid.GetComponent<BoidHealth>();

            if (health == null || !health.IsDead)
                continue;

            float distance = Vector3.Distance(
                transform.position,
                boid.transform.position
            );

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestBoid = boid;
            }
        }

        return closestBoid;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(
            transform.position,
            visionRadius
        );
    }
}