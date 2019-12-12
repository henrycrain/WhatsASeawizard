using System;
using UnityEngine;

public class StoneBehavior : MonoBehaviour
{
    public float speed;

    private void Update()
    {
        Vector3 position = transform.position;
        position.Set(position.x + speed * Time.deltaTime,
                           position.y,
                           position.z);
    }
}