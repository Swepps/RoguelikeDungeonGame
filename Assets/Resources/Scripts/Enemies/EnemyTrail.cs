using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrail : MonoBehaviour
{
    public GameObject trail;
    public float trailCooldown;
    public float trailTimeLength;

    private float trailSpawnTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        trailSpawnTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        trailSpawnTimer += Time.deltaTime;

        if (trailSpawnTimer > trailCooldown)
        {
            trailSpawnTimer -= trailCooldown;
            GameObject trailClone = Instantiate(trail, new Vector3(0, -0.2f) + transform.position, Quaternion.identity);
            Destroy(trailClone, trailTimeLength);
        }
    }
}
