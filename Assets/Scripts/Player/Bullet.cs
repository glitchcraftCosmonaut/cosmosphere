using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //Bulletworks
    public Vector2 direction = new Vector2(1, 0);
    public float speed = 2;
    public Vector2 velocity;

    //enemyBullet
    public bool isEnemy = false;


    private void OnEnable() 
    {
        Invoke("Disable", 1f);
        
    }

    void Start()
    {
        // Destroy(gameObject, 3);
    }

    // Update is called once per frame
    void Update()
    {
        velocity = direction * speed;
    }
    private void FixedUpdate() 
    {
        Vector2 pos = transform.position;

        pos += velocity * Time.fixedDeltaTime;

        transform.position = pos;    
    }
    public void Disable()
    {
        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        CancelInvoke();
    }
}
