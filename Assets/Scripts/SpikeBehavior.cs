using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBehavior : MonoBehaviour
{
   private void OnTriggerEnter(Collider other)
    {
        var healthOfTarget = other.GetComponent<Damageable>();
        if (healthOfTarget != null)
        {
            // Kill the thing falling on the spikes
            healthOfTarget.Damage(float.PositiveInfinity);
        }
    }
}
