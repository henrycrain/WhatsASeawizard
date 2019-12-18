using System;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    private AudioSource source;
    
    private void Start()
    {
        source = gameObject.GetComponent<AudioSource>();
        source.loop = true;
    }

    private void Update()
    {
        if (Input.GetAxis("Vertical") > 0 && !source.isPlaying)
        {
            source.Play();
        }
        else if (Math.Abs(Input.GetAxis("Vertical")) < 0.001f && source.isPlaying)
        {
            source.Stop();
        }
    }
}