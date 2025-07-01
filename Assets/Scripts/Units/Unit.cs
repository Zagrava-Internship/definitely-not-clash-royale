using UnityEngine;

public class Unit : MonoBehaviour
{
    private UnitData data;
    private int currentHealth;

    public void Initialize(UnitData unitData)
    {
        data = unitData;
        currentHealth = data.health;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
