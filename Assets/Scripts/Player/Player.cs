using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Gun[] guns;
    [Header("Movement")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float focusSpeed = 3f;
    public ParticleSystem playerThrust;  
    float currentSpeed;
    Vector2 movement;

    [Header("Physics and rendering")]
    Rigidbody2D playerRigidbody;
    CircleCollider2D playerCollider;
    private Renderer playerRenderer;	


    [Header("Gun n Bullets")]
    [SerializeField] float shootIntervalSeconds = 0.5f;
    [SerializeField] float shootDelay = 0.0f;
    bool shoot;
    float shootTimer = 0f;
    float delayTimer = 0f;
    private AudioSource shootSoundFX;	
    public GameObject startWeapon;						        // The players initial 'turret' gameobject
	public List<GameObject> tripleShotTurrets;			  //
	public List<GameObject> wideShotTurrets;			    // References to the upgrade weapon turrets
	public List<GameObject> scatterShotTurrets;			  //
	public List<GameObject> activePlayerTurrets;		  //
    public GameObject explosion;						          // Reference to the Expolsion prefab
	public List<GameObject> playerBullet;
    public float scatterShotTurretReloadTime = 2.0f;

    [Header("Power Ups")]
    public int upgradeState = 0;

    [Header("Boundaries")]
    private GameObject leftBoundary;                   //
    private GameObject rightBoundary;                  // References to the screen bounds: Used to ensure the player
    private GameObject topBoundary;                    // is not able to leave the screen.
    private GameObject bottomBoundary; 

    // Start is called before the first frame update
    void Start()
    {
        //bounds
        leftBoundary = GameController.SharedInstance.leftBoundary;
        rightBoundary = GameController.SharedInstance.rightBoundary;
        topBoundary = GameController.SharedInstance.topBoundary;
        bottomBoundary = GameController.SharedInstance.bottomBoundary;


        playerRigidbody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CircleCollider2D>();
        playerRenderer = gameObject.GetComponentInChildren<Renderer>();
        shootSoundFX = gameObject.GetComponent<AudioSource>();
        currentSpeed = moveSpeed;
        activePlayerTurrets = new List<GameObject>();
		activePlayerTurrets.Add(startWeapon);
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = focusSpeed;
        }
        else
        {
            currentSpeed = moveSpeed;
        }
        shoot = Input.GetKey(KeyCode.Z);

        if(shoot)
        {
            shoot = false;
            // gun.Shoot();
            if(delayTimer > shootDelay)
            {
                if(shootTimer >= shootIntervalSeconds)
                {
                    // gun.Shoot();
                    Shoot();
                    shootTimer = 0;
                }
                else
                {
                    shootTimer += Time.deltaTime;
                }
            }
            else
            {
                delayTimer += Time.deltaTime;
            }
        }
        // playerRigidbody.velocity = new Vector2(movement.x * currentSpeed, movement.y * currentSpeed);
        playerRigidbody.position = new Vector2
        (
            Mathf.Clamp (playerRigidbody.position.x, leftBoundary.transform.position.x, rightBoundary.transform.position.x),
			Mathf.Clamp (playerRigidbody.position.y, bottomBoundary.transform.position.y, topBoundary.transform.position.y)
        );
    }
    private void FixedUpdate() 
    {
        playerRigidbody.MovePosition(playerRigidbody.position + movement * currentSpeed * Time.deltaTime);
        
    }
    void Shoot() 
    {
        foreach(GameObject turret in activePlayerTurrets) 
        {
            GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject("Player Bullet"); 
            if (bullet != null) 
            {
                bullet.transform.position = turret.transform.position;
                bullet.transform.rotation = turret.transform.rotation;
                bullet.SetActive(true);
            }
        }
    shootSoundFX.Play();
  }

  void UpgradeWeapons() 
	{     

    // We check the upgrade state of the player and add the appropriate additional turret gameobjects to the active turrets List.

		if (upgradeState == 0) 
		{
			foreach(GameObject turret in tripleShotTurrets) 
			{
				activePlayerTurrets.Add(turret);
			}
		} 
		else if (upgradeState == 1) 
		{
			foreach(GameObject turret in wideShotTurrets) 
			{
				activePlayerTurrets.Add(turret);
			}
		} 
		else if (upgradeState == 2) 
		{
			StartCoroutine("ActivateScatterShotTurret");
		} 
		else 
		{
			return;
		}

		upgradeState ++;
	}

  IEnumerator ActivateScatterShotTurret() 
  {

    // The ScatterShot turret is shot independantly of the spacebar
    // This Coroutine shoots the scatteshot at a reload interval

		while (true) 
		{
			foreach(GameObject turret in scatterShotTurrets) 
			{
				GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject("Player Bullet"); 
				if (bullet != null) 
				{
					bullet.transform.position = turret.transform.position;
					bullet.transform.rotation = turret.transform.rotation;
					bullet.SetActive(true);
				}
			}
			shootSoundFX.Play();
			yield return new WaitForSeconds(scatterShotTurretReloadTime);
		}
	}

    
    void OnTriggerEnter2D(Collider2D other) 
    {
		// if (other.gameObject.tag == "Powerup") 
        // {
		// 	CollectPowerup powerupScript = other.gameObject.GetComponent<CollectPowerup>();
		// 	powerupScript.PowerupCollected();
		// 	UpgradeWeapons();
		// } 
        if (other.gameObject.tag == "Enemy Ship 1" || other.gameObject.tag == "Enemy Ship 2" || other.gameObject.tag == "Enemy Bullet") 
        {
            // GameController.SharedInstance.ShowGameOver();  // If the player is hit by an enemy ship or laser it's game over.
			playerRenderer.enabled = false;       // We can't destroy the player game object straight away or any code from this point on will not be executed
			playerCollider.enabled = false;       // We turn off the players renderer so the player is not longer displayed and turn off the players collider
            playerThrust.Stop();
			Instantiate(explosion, transform.position, transform.rotation);   // Then we Instantiate the explosions... one at the centre and some additional around the players location for a bigger bang!
			for (int i = 0 ; i < 8; i++) 
            {
				Vector3 randomOffset = new Vector3 (transform.position.x + Random.Range(-0.6f, 0.6f), transform.position.y + Random.Range(-0.6f, 0.6f), 0.0f); 
				Instantiate(explosion, randomOffset, transform.rotation);
			}
			Destroy(gameObject, 1.0f); // The second parameter in Destroy is a delay to make sure we have finished exploding before we remove the player from the scene.
		}
	}
}
