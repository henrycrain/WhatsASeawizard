using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBehavior : MonoBehaviour
{
    private bool isSwinging = false;
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (isSwinging)
        {
            anim.SetBool("Swing", true);
        }
        else
        {
            anim.SetBool("Swing", false);
        }
    }

    public void SetIsSwinging(bool value) => isSwinging = value;

    public bool GetIsSwinging() => isSwinging;
}
