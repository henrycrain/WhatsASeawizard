using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellMenu : MonoBehaviour
{
    private bool spellMenuVisible = false;
    private PlayerController player;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Seawizard").GetComponent<PlayerController>();
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (!spellMenuVisible && Input.GetButtonDown("SummonSpellMenu"))
        {
            ShowMenu(player.Spells);
        }
        else if (spellMenuVisible && Input.GetButtonDown("SummonSpellMenu"))
        {
            HideMenu();
        }
    }

    public void ShowMenu(HashSet<Spell> spellsToShow)
    {
        Cursor.lockState = CursorLockMode.None;
        foreach (Transform child in transform)
        {
            var menuButton = child.gameObject.GetComponent<RadialMenuButton>();
            if (spellsToShow.Contains(menuButton.containedSpell))
            {
                child.gameObject.SetActive(true);
            }
        }
        spellMenuVisible = true;
    }

    public void HideMenu()
    {
        Cursor.lockState = CursorLockMode.Locked;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        spellMenuVisible = false;
    }
}


