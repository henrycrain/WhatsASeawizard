using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public float gravityScale;
    
    private static readonly int Walking = Animator.StringToHash("Walking");
    private static readonly int Attacking = Animator.StringToHash("Attacking");
    private static readonly int Dying = Animator.StringToHash("Dying");

    private CharacterController controller;
    
    [SerializeField]
    private GameObject lightningParticles;
    [SerializeField]
    private GameObject fireballPrefab;
    [SerializeField]
    private Spell currentSpell = Spell.None;
    
    private float currentVerticalSpeed;
    private Damageable health;
    private Mana mana;
    private UIManager uiManager;
    private Animator animator;

    private bool canSwingSword = true;
    private bool canFireBall = true;

    private RaycastHit hitInfo;

    public HashSet<Spell> Spells { get; set; }

    private void Start()
    {
        health = GetComponent<Damageable>();
        controller = GetComponent<CharacterController>();
        mana = GetComponent<Mana>();
        uiManager = GameObject.Find("HUD").GetComponent<UIManager>();
        animator = GetComponent<Animator>();
        Spells = new HashSet<Spell>
        {
            Spell.Sword
        };
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

    private void HandleMove()
    {
        var direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        animator.SetBool(Walking, direction != Vector3.zero);
        var velocity = direction * speed;
        velocity.y = currentVerticalSpeed;

        
        if (controller.isGrounded && Input.GetButtonDown("Jump")) 
        {
            velocity.y = jumpForce;
        }

        velocity += Time.deltaTime * gravityScale * Physics.gravity;
        currentVerticalSpeed = velocity.y;

        velocity = transform.TransformDirection(velocity);

        controller.Move(velocity * Time.deltaTime);
    }

    public void SetSpell(Spell spell)
    {
        currentSpell = spell;
    }

    private void CastSpell()
    {
        switch (currentSpell)
        {
            case Spell.Lightning:
                if (mana.UseMana(2))
                {
                    if (Physics.Raycast(new Ray(transform.position + transform.forward * 0.3f, transform.forward), out hitInfo))
                    {
                        var targetHealth = hitInfo.transform.GetComponent<Damageable>();
                        if (targetHealth != null)
                        {
                            targetHealth.Damage(5);
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
                    Transform trans = transform;
                    Instantiate(fireballPrefab, trans.position + new Vector3(0.3f, 0.0f, 0.3f), trans.rotation);
                    StartCoroutine(FireballCooldown());
                }
                break;
            case Spell.Sword:
                if (canSwingSword)
                {
                    if (Physics.SphereCast(new Ray(transform.position, transform.forward), 0.3f, out hitInfo))
                    {
                        var targetHealth = hitInfo.transform.gameObject.GetComponent<Damageable>();
                        if (targetHealth != null)
                        {
                            targetHealth.Damage(10);
                        }

                    }
                    animator.SetTrigger(Attacking);
                    canSwingSword = false;
                }
                break;
        }
    }

    public void Die()
    {
        animator.SetTrigger(Dying);
        // Destroy(gameObject);
    }

    public void HandleAttackEnd()
    {
        canSwingSword = true;
    }

    private void DeathEndEvent()
    {
        // End the game in a better way (return to title screen?)
        SceneManager.LoadScene("MainMenuScene");
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
