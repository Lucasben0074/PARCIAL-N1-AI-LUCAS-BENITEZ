using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    [SerializeField] private HunterProjectile projectilePrefab;
    [SerializeField] private int poolSize = 10;

    private List<HunterProjectile> projectiles =
        new List<HunterProjectile>();

    private void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            HunterProjectile projectile =
                Instantiate(
                    projectilePrefab,
                    transform
                );

            projectile.gameObject.SetActive(false);

            projectiles.Add(projectile);
        }
    }

    public HunterProjectile GetProjectile()
    {
        foreach (HunterProjectile projectile in projectiles)
        {
            if (!projectile.gameObject.activeInHierarchy)
            {
                return projectile;
            }
        }

        return null;
    }
}