using System;
using UnityEngine;

public class StoneDespawner : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }
}