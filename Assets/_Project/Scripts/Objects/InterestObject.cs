using UnityEngine;

public class InterestObject : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;

    private float currentHealth;

    public static int ActiveObjects { get; private set; }

    public float CurrentHealth => currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;

        ActiveObjects++;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        ActiveObjects--;
    }
}