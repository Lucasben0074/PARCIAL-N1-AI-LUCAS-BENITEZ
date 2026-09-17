using UnityEngine;

public class BoidHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;

    [Header("Respawn")]
    [SerializeField] private float respawnTime = 5f;
    [SerializeField] private float respawnLimitX = 13f;
    [SerializeField] private float respawnLimitZ = 13f;

    private float currentHealth;
    private float respawnTimer;

    private bool isDead;
    private bool isCollected;

    private BoidAgent agent;
    private Renderer boidRenderer;
    private Collider boidCollider;

    public float CurrentHealth => currentHealth;
    public bool IsDead => isDead;
    public bool IsCollected => isCollected;

    private void Start()
    {
        currentHealth = maxHealth;

        agent = GetComponent<BoidAgent>();
        boidRenderer = GetComponent<Renderer>();
        boidCollider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (!isCollected)
            return;

        respawnTimer -= Time.deltaTime;

        if (respawnTimer <= 0f)
        {
            Respawn();
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;

        if (currentHealth <= 0f)
        {
            currentHealth = 0f;
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        if (agent != null)
        {
            agent.SetActive(false);
        }
    }

    public void Collect()
    {
        if (!isDead || isCollected)
            return;

        isCollected = true;
        respawnTimer = respawnTime;

        if (boidRenderer != null)
        {
            boidRenderer.enabled = false;
        }

        if (boidCollider != null)
        {
            boidCollider.enabled = false;
        }
    }

    private void Respawn()
    {
        Vector3 randomPosition = new Vector3(
            Random.Range(-respawnLimitX, respawnLimitX),
            transform.position.y,
            Random.Range(-respawnLimitZ, respawnLimitZ)
        );

        transform.position = randomPosition;

        currentHealth = maxHealth;

        isDead = false;
        isCollected = false;

        if (boidRenderer != null)
        {
            boidRenderer.enabled = true;
        }

        if (boidCollider != null)
        {
            boidCollider.enabled = true;
        }

        if (agent != null)
        {
            agent.SetActive(true);
        }

        Debug.Log(gameObject.name + " hizo RESPAWN");
    }
}