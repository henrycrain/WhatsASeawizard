using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;
    private float currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void Damage(float value)
    {
        currentHealth = Mathf.Max(0, currentHealth - value);
        
        if (currentHealth == 0)
        {
            Destroy(gameObject);
        }
    }

    public void Heal(float value)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + value);
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }
}
