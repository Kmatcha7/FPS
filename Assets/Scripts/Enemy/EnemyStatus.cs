using UnityEngine;
using UnityEngine.UI;

public class EnemyStatus : MonoBehaviour, IDamageable
{
    public EnemyData enemyData;
    public Slider hpSlider;

    int currentHP;

    void Start()
    {
        currentHP = enemyData.maxHP;

        // HPバーを初期化
        if (hpSlider != null)
        {
            hpSlider.maxValue = enemyData.maxHP;
            hpSlider.value = currentHP;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Max(currentHP, 0);

        // HPバーを更新
        if (hpSlider != null)
        {
            hpSlider.value = currentHP;
        }

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