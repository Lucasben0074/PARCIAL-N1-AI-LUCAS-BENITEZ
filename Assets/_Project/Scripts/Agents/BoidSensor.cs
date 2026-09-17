using System.Collections.Generic;
using UnityEngine;

public class BoidSensor : MonoBehaviour
{
    [SerializeField] private float perceptionRadius = 5f;
    [SerializeField] private LayerMask boidLayer;

    public List<BoidAgent> GetNearbyBoids()
    {
        List<BoidAgent> nearbyBoids = new List<BoidAgent>();

        Collider[] colliders = Physics.OverlapSphere(
            transform.position,
            perceptionRadius,
            boidLayer
        );

        foreach (Collider col in colliders)
        {
            BoidAgent boid = col.GetComponent<BoidAgent>();

            if (boid != null && boid.gameObject != gameObject)
            {
                nearbyBoids.Add(boid);
            }
        }

        return nearbyBoids;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, perceptionRadius);
    }
}