using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public GameObject player;
    public float smoothing;
    public Vector3 offset;

    private void Start()
    {
        player = PlayerStats.playerStats.GetPlayer();
    }

    private void FixedUpdate()
    {
        if (player != null)
        {
            Vector3 newPos = Vector3.Lerp(transform.position, player.transform.position + offset, smoothing);
            transform.position = newPos;
        }        
    }
}
