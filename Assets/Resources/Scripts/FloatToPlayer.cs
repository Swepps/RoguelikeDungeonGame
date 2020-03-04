using System.Collections;
using UnityEngine;

public class FloatToPlayer : MonoBehaviour
{
    private GameObject player;
    private float speed;
    private float currentAcc;
    public float acceleration;
    public float hangTime;

    private void Start()
    {
        player = GameObject.Find("Player");
        currentAcc = 0;
        speed = 0;
        StartCoroutine(WaitBeforeMoving());
    }

    private void Update()
    {
        if (player != null)
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        speed += currentAcc;
    }

    IEnumerator WaitBeforeMoving()
    {
        yield return new WaitForSeconds(hangTime);
        currentAcc = acceleration;
    }
}
