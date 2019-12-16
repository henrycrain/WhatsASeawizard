using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public float gravityScale;
    
    private static readonly int Walking = Animator.StringToHash("Walking");
    private static readonly int Attacking = Animator.StringToHash("Attacking");

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

    private void Start()
    {
        health = GetComponent<Damageable>();
        controller = GetComponent<CharacterController>();
        mana = GetComponent<Mana>();
        uiManager = GameObject.Find("HUD").GetComponent<UIManager>();
        animator = GetComponent<Animator>();
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

    public void CastSpell()
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
                    var trans = transform;
                    Instantiate(fireballPrefab, trans.position + new Vector3(0.4f, 0, 0.4f), trans.rotation);
                    StartCoroutine(FireballCooldown());
                }
                break;
            case Spell.Sword:
                if (canSwingSword)
                {
                    if (Physics.SphereCast(new Ray(transform.position + transform.forward * 0.3f, transform.forward), 2f, out hitInfo))
                    {
                        var targetHealth = hitInfo.transform.GetComponent<Damageable>();
                        if (targetHealth != null)
                        {
                            targetHealth.Damage(10);
                        }

                    }
                    animator.SetBool(Attacking, true);
                    StartCoroutine(SwordCooldown());
                }
                break;
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    IEnumerator SwordCooldown()
    {
        canSwingSword = false;
        // Should probably use animation events here, sword gets out of sync quickly
        yield return new WaitForSeconds(0.5f);
        canSwingSword = true;
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
