using UnityEngine;

public class EnemyStatus : MonoBehaviour, IDamageable
{
    public EnemyData enemyData;

    int currentHP;

    void Start()
    {
        currentHP = enemyData.maxHP;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}