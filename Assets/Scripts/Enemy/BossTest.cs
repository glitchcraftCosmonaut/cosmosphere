using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTest : MonoBehaviour
{
    [SerializeField] Transform[] bossWaypoint;
    [SerializeField] int randomWaypoint;
    [SerializeField] float bossSpeed;
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        if(player == null)
        {
            Debug.LogError("The player is null");
        }

    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        if(transform.position == bossWaypoint[0].position || transform.position == bossWaypoint[1].position || transform.position == bossWaypoint[2].position)
        {
            randomWaypoint = Random.Range(0, 3);
        }
        transform.position = Vector3.MoveTowards(transform.position, bossWaypoint[randomWaypoint].position, bossSpeed * Time.deltaTime);
    }
}
