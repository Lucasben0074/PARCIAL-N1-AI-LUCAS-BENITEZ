using UnityEngine;

public class SeekBehaviour : SteeringBehaviour
{
    [SerializeField] private Transform target;

    public override Vector3 GetDirection()
    {
        if (target == null)
            return Vector3.zero;

        Vector3 direction = target.position - transform.position;
        direction.y = 0;

        return direction.normalized;
    }
}