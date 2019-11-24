using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballBehavior : MonoBehaviour
{
    private float speed = 5f;
    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
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

        Destroy(gameObject);
    }
}
