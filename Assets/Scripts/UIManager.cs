using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject hpCover;
    [SerializeField]
    private GameObject mpCover;
    public void UpdateHealth(float frac)
    {
        hpCover.transform.localScale = new Vector3(frac, 1, 1);
    }
    
    public void UpdateMana(float frac)
    {
        mpCover.transform.localScale = new Vector3(frac, 1, 1);
    }
}
