using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DanmakU;


public class PlayerCollision : MonoBehaviour
{
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<DanmakuCollider>().OnDanmakuCollision += OnDanmakuCollision;
        player = GetComponent<Player>();
    }

    void OnDanmakuCollision(DanmakuCollisionList collisionList)
    {
        Debug.Log("Player collided with Danmaku bullet");
        Debug.Log(collisionList[0].ToString());
        //your code here

        if(player.HasShield())
            {
                player.DeactiveShield();
                Instantiate(player.explosion, transform.position, transform.rotation);
            }

            else
            {
                player.health.Damage(5);
                Instantiate(player.explosion, transform.position, transform.rotation);

                if(player.health.health == 0)
                {
                    GameController.SharedInstance.ShowGameOver();  // If the player is hit by an enemy ship or laser it's game over.
                    player.playerRenderer.enabled = false;       // We can't destroy the player game object straight away or any code from this point on will not be executed
                    player.playerCollider.enabled = false;       // We turn off the players renderer so the player is not longer displayed and turn off the players collider
                    player.playerThrust.Stop();
                    Instantiate(player.explosion, transform.position, transform.rotation);   // Then we Instantiate the explosions... one at the centre and some additional around the players location for a bigger bang!
                    for (int i = 0 ; i < 8; i++) 
                    {
                        Vector3 randomOffset = new Vector3 (transform.position.x + Random.Range(-0.6f, 0.6f), transform.position.y + Random.Range(-0.6f, 0.6f), 0.0f); 
                        Instantiate(player.explosion, randomOffset, transform.rotation);
                    }

                    // Destroy(gameObject, 1.0f); // The second parameter in Destroy is a delay to make sure we have finished exploding before we remove the player from the scene.
                    gameObject.SetActive(false);
                    
                    // isDead = true;
                }  
		    }

    }
    
}
