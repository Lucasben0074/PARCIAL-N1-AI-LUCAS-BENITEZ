using UnityEngine;

public class EvadeBehaviour : SteeringBehaviour
{
    [SerializeField] private HunterSensor sensor;

    [Header("Prediction")]
    [SerializeField] private float predictionTime = 1f;

    public override Vector3 GetDirection()
    {
        Transform hunterTransform = sensor.GetHunter();

        if (hunterTransform == null)
            return Vector3.zero;

        HunterAgent hunter =
            hunterTransform.GetComponent<HunterAgent>();

        if (hunter == null)
            return Flee(hunterTransform.position);

        Vector3 futurePosition =
            CalculateFuture(hunter);

        return Flee(futurePosition);
    }

    private Vector3 CalculateFuture(HunterAgent hunter)
    {
        return hunter.transform.position +
               hunter.Velocity * predictionTime;
    }

    private Vector3 Flee(Vector3 targetPosition)
    {
        Vector3 desiredDirection =
            transform.position - targetPosition;

        desiredDirection.y = 0;

        if (desiredDirection == Vector3.zero)
            return Vector3.zero;

        return desiredDirection.normalized;
    }
}