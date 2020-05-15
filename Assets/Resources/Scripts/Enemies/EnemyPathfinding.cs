using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyPathfinding : MonoBehaviour
{
    public Pathfinding pathFinding;
    private Enemy enemy;

    [HideInInspector]
    public float speed;

    public float maxSpeed;

    private List<Vector2> pathVectorList;

    private Vector3 targetPos;
    private Vector2 endPos;
    private Vector3 moveDir;

    private float pathfindingTimer = 0f;
    private const float PATHFINDING_TIMER_LENGTH = 0.3f;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        pathFinding = enemy.GetRoom().GetPathfinding();
        speed = maxSpeed;
    }

    public void ManualUpdate()
    {
        pathfindingTimer -= Time.deltaTime;

        HandleMovement();

        bool debugMode = true;
        if (debugMode)
        {
            if (pathVectorList != null)
            {
                for (int i = 0; i < pathVectorList.Count - 1; i++)
                {
                    Debug.DrawLine(new Vector3(pathVectorList[i].x, pathVectorList[i].y),
                        new Vector3(pathVectorList[i + 1].x, pathVectorList[i + 1].y),
                        Color.green, Time.deltaTime);
                }
            }
        }
    }

    private void HandleMovement()
    {
        if (pathVectorList != null && pathVectorList.Count > 1)
        {
            targetPos = pathVectorList[1];
        }
        else if (pathVectorList == null || pathVectorList.Count == 0)
        {
            targetPos = endPos;            
        }
        else
        {
            targetPos = pathVectorList[0];
        }

        moveDir = (targetPos - transform.position).normalized;
        transform.position = transform.position + moveDir * speed * Time.deltaTime;

        enemy.SetSpriteDirection(moveDir);
    }

    public bool MoveTo(Vector2 targetPos)
    {
        return SetTargetPos(targetPos);
    }

    public void MoveToDirect(Vector2 targetPos)
    {
        pathVectorList = null;
        endPos = targetPos;
    }

    public bool MoveToTimer(Vector2 targetPos)
    {
        if (pathfindingTimer <= 0f)
        {
            pathfindingTimer += PATHFINDING_TIMER_LENGTH;
            return SetTargetPos(targetPos);
        }
        else
            return true;
    }

    public void StopMoving()
    {
        pathVectorList = null;
        moveDir = Vector2.zero;
    }

    public Vector2 GetTargetPos()
    {
        return endPos;
    }

    private bool SetTargetPos(Vector2 targetPos)
    {
        pathVectorList = pathFinding.FindPath(transform.position, targetPos);

        if (pathVectorList == null)
            return false;
        else
        {
            endPos = pathVectorList[pathVectorList.Count - 1];
            return true;
        }
    }
}
