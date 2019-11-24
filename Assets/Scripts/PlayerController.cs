using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject fireballPrefab;
    public float speed;
    public float jumpForce;
    public float gravityScale;
    private CharacterController controller;
    [SerializeField]
    private GameObject lightningParticles;

    [SerializeField]
    private Spell currentSpell = Spell.None;

    [SerializeField]
    private GameObject sword;

    private float currentVerticalSpeed = 0f;
    private Damageable health;
    private Mana mana;
    private UIManager uiManager;
    private ParticleSystem lightningSystem;

    private bool canSwingSword = true;
    private bool canFireBall = true;

    private RaycastHit hitInfo = new RaycastHit();
    void Start()
    {
        health = GetComponent<Damageable>();
        controller = GetComponent<CharacterController>();
        mana = GetComponent<Mana>();
        uiManager = GameObject.Find("HUD").GetComponent<UIManager>();
    }

    void Update()
    {
        HandleMove();

        if (Cursor.lockState == CursorLockMode.Locked && Input.GetMouseButton(0))
        {
            CastSpell();
        }
        else if (lightningParticles.activeInHierarchy)
        {
            lightningParticles.SetActive(false);
        }

        uiManager.UpdateHealth(health.GetCurrentHealth() / health.GetMaxHealth());
        uiManager.UpdateMana(mana.GetCurrentMana() / mana.GetMaxMana());
    }

    void HandleMove()
    {
        var direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

        var velocity = direction * speed;

        velocity.y = currentVerticalSpeed;

        
        if (controller.isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = jumpForce;
        }

        velocity += Physics.gravity * gravityScale * Time.deltaTime;

        currentVerticalSpeed = velocity.y;

        velocity = transform.TransformDirection(velocity);

        controller.Move(velocity * Time.deltaTime);
    }

    public void SetSpell(Spell spell)
    {
        currentSpell = spell;
    }

    public void CastSpell()
    {
        switch (currentSpell)
        {
            case Spell.Lightning:
                if (mana.UseMana(2))
                {
                    if (Physics.Raycast(new Ray(transform.position + transform.forward * 0.3f, transform.forward), out hitInfo))
                    {
                        var health = hitInfo.transform.GetComponent<Damageable>();

                        if (health != null)
                        {
                            health.Damage(5);
                        }
                    }

                    // Send lightning particle effects
                    lightningParticles.SetActive(true);
                }
                else if (lightningParticles.activeInHierarchy)
                {
                    lightningParticles.SetActive(false);
                }
                break;
            case Spell.Fire:
                if (canFireBall && mana.UseMana(20))
                {
                    // Instantiate a fireball at player's position
                    Instantiate(fireballPrefab, transform.position + new Vector3(0.4f, 0, 0.4f), transform.rotation);
                    StartCoroutine(FireballCooldown());
                }
                break;
            case Spell.Sword:
                if (canSwingSword)
                {
                    if (Physics.SphereCast(new Ray(transform.position + transform.forward * 0.3f, transform.forward), 2f, out hitInfo))
                    {
                        var health = hitInfo.transform.GetComponent<Damageable>();

                        if (health != null)
                        {
                            health.Damage(10);
                        }

                    }
                    StartCoroutine(SwordCooldown());
                }
                break;
            default:
                break;
        }
    }

    IEnumerator SwordCooldown()
    {
        canSwingSword = false;
        // Should probably use animation events here, sword gets out of sync quickly
        var swordBehavior = sword.GetComponent<SwordBehavior>();
        swordBehavior.SetIsSwinging(true);
        yield return new WaitForSeconds(0.5f);
        canSwingSword = true;
        swordBehavior.SetIsSwinging(false);
    }

    IEnumerator FireballCooldown()
    {
        canFireBall = false;
        yield return new WaitForSeconds(0.2f);
        canFireBall = true;
    }
}

public enum Spell
{
    None,
    Fire,
    Lightning,
    Sword
}
