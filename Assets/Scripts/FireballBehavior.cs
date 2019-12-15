using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballBehavior : MonoBehaviour
{
    private const float Speed = 5f;
    
    // Update is called once per frame
    private void Update()
    {
        Transform trans = transform;
        trans.position += trans.forward * (Speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Fireball collision");
        var otherObject = other.gameObject;

        var health = otherObject.GetComponent<Damageable>();
        if (health != null)
        {
            health.Damage(20);
        }

        if (other.GetComponent<Flammable>() != null)
        {
            Destroy(other.gameObject);
        }

        Destroy(gameObject);
    }
}
