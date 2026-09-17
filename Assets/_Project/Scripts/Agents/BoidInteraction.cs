using UnityEngine;

public class BoidInteraction : MonoBehaviour
{
    [SerializeField] private InterestObjectSensor sensor;
    [SerializeField] private BoidHealth health;
    [SerializeField] private BoidAgent agent;

    [Header("Interaction")]
    [SerializeField] private float interactionDistance = 1.2f;
    [SerializeField] private float interactionInterval = 1f;
    [SerializeField] private float damage = 25f;

    private float interactionTimer;

    private void Update()
    {
        if (health.IsDead || health.IsCollected)
        {
            interactionTimer = 0f;
            return;
        }

        // Evade tiene prioridad sobre la interaccion.
        if (agent.CurrentSteering == BoidAgent.SteeringMode.Evade)
        {
            interactionTimer = 0f;
            return;
        }

        InterestObject target =
            sensor.GetClosestObject();

        if (target == null)
        {
            interactionTimer = 0f;
            return;
        }

        float distance = Vector3.Distance(
            transform.position,
            target.transform.position
        );

        if (distance > interactionDistance)
        {
            interactionTimer = 0f;
            return;
        }

        interactionTimer += Time.deltaTime;

        if (interactionTimer >= interactionInterval)
        {
            target.TakeDamage(damage);

            interactionTimer = 0f;
        }
    }
}