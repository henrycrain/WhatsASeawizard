using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenuButton : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject parentMenu;
    
    public Spell containedSpell;

    private Button button;
    private PlayerController controller;
    private SpellMenu spellMenu;
    // Start is called before the first frame update
    void Start()
    {
        controller = player.GetComponent<PlayerController>();
        button = GetComponent<Button>();
        spellMenu = parentMenu.GetComponent<SpellMenu>();
        button.onClick.AddListener(() =>
        {
            controller.SetSpell(containedSpell);
            // Need to figure out how to fire events
            // then have the SpellMenu script handle it
            spellMenu.HideMenu();
        });
    }
    
}
