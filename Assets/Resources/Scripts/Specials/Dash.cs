using UnityEngine;
using System.Collections;

public class Dash : SpecialAttack
{
    public float dashSpeed = 15;
    public float dashTime = 0.15f;
    private PlayerMovement playerMov;

    private void Start()
    {
        playerMov = GetComponent<PlayerMovement>();
    }

    protected override bool Special()    {
        
        if (playerMov.direction == Vector2.zero) // returns false if player can't dash
            return false;

        playerMov.lockMovement = true;
        playerMov.moveSpeed += dashSpeed;
        StartCoroutine(StopDash(dashTime));

        return true;
    }

    IEnumerator StopDash(float dashTime)
    {
        yield return new WaitForSeconds(dashTime);
        playerMov.moveSpeed = playerMov.maxSpeed;
        playerMov.lockMovement = false;
    }
}
