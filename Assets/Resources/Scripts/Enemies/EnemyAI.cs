using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class EnemyAI : MonoBehaviour
{
    public enum MoveState
    {
        Roaming,
        ChaseTarget,
    }

    public enum AttackState
    {
        Idle,
        Attack,
    }

    public enum AttackType
    {
        Shoot,
        Melee,
    }

    public enum MoveType
    {
        Running,
        Charging,
    }

    private EnemyPathfinding enemyPathfinding;
    private EnemyShoot enemyShoot;
    private EnemyWeapon enemyWeapon;
    private Vector3 startPos;
    private Vector3 roamingPos;
    private MoveState moveState;
    private AttackState attackState;
    public AttackType attackType;
    public MoveType moveType;
    public float chargeCooldown;
    private float chargeTimer;
    private bool charging;
    private Vector3 chargePos;
    public float attackRange;
    public float targetRange;
    public float roamingRange;

    private GameObject player;
    private Transform playerPos;
    private int x, y;

    private void Awake()
    {
        enemyPathfinding = GetComponent<EnemyPathfinding>();
        enemyShoot = GetComponent<EnemyShoot>();
        enemyWeapon = transform.GetComponentInChildren<EnemyWeapon>();
        moveState = MoveState.Roaming;
        attackState = AttackState.Idle;
    }

    private void Start()
    {
        startPos = transform.position;
        roamingPos = GetRoamingPos();
        charging = false;
    }

    private void Update()
    {
        if (player == null)
        {
            player = PlayerStats.playerStats.GetPlayerObject();
            playerPos = player.transform;
        }
        else
        {
            switch (moveState)
            {
                case MoveState.Roaming:
                    enemyPathfinding.pathFinding.GetGrid().GetXY(transform.position, out x, out y);
                    if (x > 17 || y > 17)
                    {
                        enemyPathfinding.MoveToDirect(startPos);
                    }
                    else
                    {
                        while (!enemyPathfinding.MoveTo(roamingPos))
                            roamingPos = GetRoamingPos();
                        float reachedPositionDistance = 0.5f;
                        if (Vector2.Distance(transform.position, enemyPathfinding.GetTargetPos()) < reachedPositionDistance)
                        {
                            roamingPos = GetRoamingPos();
                        }

                        FindTarget();
                    }                    
                    break;

                case MoveState.ChaseTarget:
                    enemyPathfinding.pathFinding.GetGrid().GetXY(transform.position, out x, out y);
                    if (moveType == MoveType.Charging)
                    {
                        // end of charge
                        if (chargeTimer < -1.2f)
                        {
                            chargeTimer = chargeCooldown;
                            charging = false;
                        }
                        // charge for .2 seconds
                        else if (chargeTimer < -1f)
                        {
                            enemyPathfinding.speed = enemyPathfinding.maxSpeed * 5;
                            chargePos = playerPos.position;
                            charging = true;
                        }
                        // charge wind-up
                        else if (chargeTimer < 0)
                        {
                            enemyPathfinding.speed = enemyPathfinding.maxSpeed * 0.2f;
                        }
                        // before charge
                        else
                        {
                            enemyPathfinding.speed = enemyPathfinding.maxSpeed;
                        }
                        chargeTimer -= Time.deltaTime;
                    }
                    // if not charging
                    if (!charging)
                    {
                        if (x > 17 || y > 17)
                        {
                            enemyPathfinding.MoveToDirect(startPos);
                            moveState = MoveState.Roaming;
                        }
                        else if (Vector2.Distance(transform.position, playerPos.position) < 2.0f)
                        {
                            enemyPathfinding.MoveToDirect(playerPos.position);
                        }
                        else
                        {
                            enemyPathfinding.MoveTo(playerPos.position);
                        }
                    }
                    else
                    {
                        enemyPathfinding.MoveToDirect(chargePos);
                    }

                    if (Vector2.Distance(transform.position, playerPos.position) < attackRange)
                        attackState = AttackState.Attack;
                    else if (Vector2.Distance(transform.position, playerPos.position) > targetRange)
                    {
                        attackState = AttackState.Idle;
                        moveState = MoveState.Roaming;
                    }
                    
                    break;
            }
            
            switch (attackState)
            {
                case AttackState.Idle:
                    switch (attackType)
                    {
                        case AttackType.Shoot:
                            if (enemyShoot != null)
                            {
                                if (enemyShoot.IsShooting())
                                    enemyShoot.StopShooting();
                            }
                            if (enemyWeapon != null)
                            {
                                if (enemyWeapon.IsShooting())
                                    enemyWeapon.StopShooting();
                            }
                            break;

                        case AttackType.Melee:
                            break;
                    }
                    break;

                case AttackState.Attack:
                    switch (attackType)
                    {
                        case AttackType.Shoot:
                            if (enemyShoot != null)
                            {
                                if (!enemyShoot.IsShooting())
                                    enemyShoot.StartShooting(playerPos);
                            }                            
                            if (enemyWeapon != null)
                            {
                                if (!enemyWeapon.IsShooting())
                                    enemyWeapon.StartShooting(playerPos);
                            }
                            break;

                        case AttackType.Melee:
                            break;
                    }
                    break;
            }

            enemyPathfinding.ManualUpdate();
        }
    }

    private void FindTarget()
    {
        enemyPathfinding.pathFinding.GetGrid().GetXY(playerPos.position, out int x, out int y);
        if (x > 17 || y > 17)
            return;

        if (Vector2.Distance(transform.position, playerPos.position) < targetRange)
            moveState = MoveState.ChaseTarget;
        
    }

    private Vector3 GetRoamingPos()
    {
        return startPos + UtilsClass.GetRandomDir() * Random.Range(1f, roamingRange);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Walls" || collision.tag == "Enemy")
        {
            roamingPos = GetRoamingPos();
            while (!enemyPathfinding.MoveTo(roamingPos))
                roamingPos = GetRoamingPos();
        }
    }
}
