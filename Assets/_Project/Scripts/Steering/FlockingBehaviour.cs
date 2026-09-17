using UnityEngine;

public class FlockingBehaviour : SteeringBehaviour
{
    [SerializeField] private SeparationBehaviour separation;
    [SerializeField] private AlignmentBehaviour alignment;
    [SerializeField] private CohesionBehaviour cohesion;

    [Header("Weights")]
    [SerializeField] private float separationWeight = 2f;
    [SerializeField] private float alignmentWeight = 1f;
    [SerializeField] private float cohesionWeight = 1f;

    private Vector3 currentDirection;

    private void Start()
    {
        currentDirection = Random.insideUnitSphere;
        currentDirection.y = 0;
        currentDirection.Normalize();
    }

    public override Vector3 GetDirection()
    {
        Vector3 separationDirection = separation.GetDirection();
        Vector3 alignmentDirection = alignment.GetDirection();
        Vector3 cohesionDirection = cohesion.GetDirection();

        Vector3 flockingDirection =
            separationDirection * separationWeight +
            alignmentDirection * alignmentWeight +
            cohesionDirection * cohesionWeight;

        if (flockingDirection != Vector3.zero)
        {
            currentDirection = flockingDirection.normalized;
        }

        return currentDirection;
    }
}