using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mana : MonoBehaviour
{
    [SerializeField]
    private float maxMana;
    private float currentMana;
    // Start is called before the first frame update
    void Start()
    {
        currentMana = maxMana;
    }

    public bool UseMana(float value)
    {
        if (currentMana - value < 0)
        {
            return false;
        }

        currentMana -= value;

        return true;
    }

    public void RestoreMana(float value)
    {
        currentMana = Mathf.Min(currentMana + value, maxMana);
    }

    public float GetCurrentMana()
    {
        return currentMana;
    }

    public float GetMaxMana()
    {
        return maxMana;
    }
}
