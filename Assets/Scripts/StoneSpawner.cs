using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class StoneSpawner : MonoBehaviour
{
    public GameObject stoneParent;
    public GameObject stonePrefab;
    public float minInterval;
    public float maxInterval;
    public float stoneSpeed;

    private void Start()
    {
        StartCoroutine(RandomCreateStone());
    }

    private IEnumerator RandomCreateStone()
    {
        while (true)
        {
            CreateStone();
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));
        }
    }

    private void CreateStone()
    {
        GameObject stone = Instantiate(stonePrefab, stoneParent.transform, true);
        stone.transform.position = transform.position;  // set position to spawner
        stone.GetComponent<StoneBehavior>().speed = stoneSpeed;
    }
}