using UnityEngine;

public class HunterProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private float reachDistance = 0.3f;

    private Transform target;
    private float currentLifeTime;

    public void Initialize(Transform newTarget)
    {
        target = newTarget;
        currentLifeTime = lifeTime;
    }

    private void Update()
    {
        if (target == null)
        {
            gameObject.SetActive(false);
            return;
        }

        currentLifeTime -= Time.deltaTime;

        if (currentLifeTime <= 0f)
        {
            gameObject.SetActive(false);
            return;
        }

        Vector3 direction =
            target.position - transform.position;

        direction.y = 0;

        float distance = direction.magnitude;

        
        if (distance <= reachDistance)
        {
            gameObject.SetActive(false);
            return;
        }

        transform.position +=
            direction.normalized * speed * Time.deltaTime;

        if (direction != Vector3.zero)
        {
            transform.forward =
                direction.normalized;
        }
    }

    private void OnDisable()
    {
        target = null;
    }
}