using UnityEngine;

public class BoidAgent : MonoBehaviour
{
    public enum SteeringMode
    {
        Seek,
        Flocking,
        Evade,
        Arrive
    }

    [Header("Movement")]
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float maxSteering = 3f;

    [Header("Steering")]
    [SerializeField] private SteeringMode currentSteering;


    [SerializeField] private SeekBehaviour seek;
    [SerializeField] private FlockingBehaviour flocking;
    [SerializeField] private EvadeBehaviour evade;
    [SerializeField] private ArriveBehaviour arrive;

    [Header("Sensors")]
    [SerializeField] private HunterSensor hunterSensor;
    [SerializeField] private InterestObjectSensor interestObjectSensor;

    private Vector3 velocity;

    public Vector3 Velocity => velocity;
    public SteeringMode CurrentSteering => currentSteering;

    private bool isActive = true;

    public bool IsActive => isActive;
    private void Update()
    {
        if (!isActive)
            return;

        UpdateSteeringMode();

        Vector3 direction = GetSteeringDirection();

        Vector3 desiredVelocity = direction * maxSpeed;

        Vector3 steeringForce = desiredVelocity - velocity;

        steeringForce = Vector3.ClampMagnitude(
            steeringForce,
            maxSteering * Time.deltaTime
        );

        velocity += steeringForce;

        velocity = Vector3.ClampMagnitude(
            velocity,
            maxSpeed
        );

        transform.position += velocity * Time.deltaTime;

        if (velocity != Vector3.zero)
        {
            transform.forward = velocity.normalized;
        }
    }

    private Vector3 GetSteeringDirection()
    {
        switch (currentSteering)
        {
            case SteeringMode.Seek:
                return seek.GetDirection();

            case SteeringMode.Flocking:
                return flocking.GetDirection();
            case SteeringMode.Evade:
                return evade.GetDirection();
            case SteeringMode.Arrive:
                return arrive.GetDirection();
            default:
                return Vector3.zero;
        }
    }
    private void UpdateSteeringMode()
    {
        Transform hunter =
            hunterSensor.GetHunter();

        // PRIORIDAD 1: Hunter
        if (hunter != null)
        {
            currentSteering = SteeringMode.Evade;
            return;
        }

        InterestObject interestObject =
            interestObjectSensor.GetClosestObject();

        // PRIORIDAD 2: objeto de inter3s
        if (interestObject != null)
        {
            currentSteering = SteeringMode.Arrive;
            return;
        }

        // PRIORIDAD 3: comportamiento normal
        currentSteering = SteeringMode.Flocking;
    }
    public void SetActive(bool value)
    {
        isActive = value;

        if (!isActive)
        {
            velocity = Vector3.zero;
        }
    }
}