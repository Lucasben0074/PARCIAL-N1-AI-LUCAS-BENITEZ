using UnityEngine;

public class InterestObjectSensor : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 6f;
    [SerializeField] private LayerMask interestObjectLayer;

    public InterestObject GetClosestObject()
    {
        Collider[] colliders = Physics.OverlapSphere(
            transform.position,
            detectionRadius,
            interestObjectLayer
        );

        InterestObject closestObject = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider col in colliders)
        {
            InterestObject interestObject =
                col.GetComponent<InterestObject>();

            if (interestObject == null)
                continue;

            float distance = Vector3.Distance(
                transform.position,
                interestObject.transform.position
            );

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestObject = interestObject;
            }
        }

        return closestObject;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(
            transform.position,
            detectionRadius
        );
    }
}