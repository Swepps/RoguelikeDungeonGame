using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{    
    public Rigidbody2D rb;
    public Vector2 direction;

    private SpriteRenderer spriteRenderer;
    public Animator animator;
    
    public float moveSpeed;
    public float maxSpeed;
    public bool lockMovement;

    private bool recoiling;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = Resources.Load("Animation/elf_movement_controller") as RuntimeAnimatorController;
        recoiling = false;
    }

    void Update()
    {
        if (!lockMovement)
        {
            // inputs
            direction.x = Input.GetAxisRaw("Horizontal");
            direction.y = Input.GetAxisRaw("Vertical");
            direction.Normalize();
        }        

        if (direction != Vector2.zero) // aka, is moving
        {
            SetSpriteDirection(direction);
        }
        else
        {
            animator.SetLayerWeight(1, 0);
        }
        animator.SetFloat("moveSpeed", moveSpeed/maxSpeed);
    }

    public void PlayFootStep()
    {
        if (PlayerStats.playerStats.stats.weapon == SkillPath.Weapon.STAFF)
            FindObjectOfType<AudioManager>().Play("WizardFootStep");
        else
            FindObjectOfType<AudioManager>().Play("FootStep");
    }

    public void Recoil(Vector2 recoilForce)
    {
        recoiling = true;
        rb.AddForce(recoilForce, ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        // movement
        if (!recoiling)
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
        else if (Mathf.Abs(rb.velocity.x) < 1f && Mathf.Abs(rb.velocity.y) < 1f)
            recoiling = false;
    }

    private void SetSpriteDirection(Vector2 direction)
    {
        animator.SetLayerWeight(1, 1);
        //animator.SetFloat("xDir", direction.x);
        //animator.SetFloat("yDir", direction.y);

        if (direction.x < 0)
            spriteRenderer.flipX = true;
        else if (direction.x > 0)
            spriteRenderer.flipX = false;
    }
}
