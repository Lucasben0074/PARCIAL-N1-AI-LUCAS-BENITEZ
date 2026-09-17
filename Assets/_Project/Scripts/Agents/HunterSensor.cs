using UnityEngine;

public class HunterSensor : MonoBehaviour
{
    [SerializeField] private float visionRadius = 6f;
    [SerializeField] private LayerMask hunterLayer;

    public Transform GetHunter()
    {
        Collider[] colliders = Physics.OverlapSphere(
            transform.position,
            visionRadius,
            hunterLayer
        );

        if (colliders.Length == 0)
            return null;

        return colliders[0].transform;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, visionRadius);
    }
}