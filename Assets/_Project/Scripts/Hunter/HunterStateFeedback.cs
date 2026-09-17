using UnityEngine;

public class HunterStateFeedback : MonoBehaviour
{
    [SerializeField] private GameObject patrolIcon;
    [SerializeField] private GameObject attackIcon;
    [SerializeField] private GameObject gatherIcon;

    private State lastState;

    public void UpdateFeedback(State currentState)
    {
        if (currentState == null)
            return;

        if (currentState != lastState)
        {
            Debug.Log(
                "FEEDBACK - Estado actual: " +
                currentState.GetType().Name
            );

            lastState = currentState;
        }

        patrolIcon.SetActive(
            currentState is PatrolState
        );

        attackIcon.SetActive(
            currentState is AttackState
        );

        gatherIcon.SetActive(
            currentState is GatherState
        );
    }
}