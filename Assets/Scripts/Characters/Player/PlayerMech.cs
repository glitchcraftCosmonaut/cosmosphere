using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMech : MonoBehaviour
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
    [HideInInspector] public CircleCollider2D playerCollider;
    public Renderer playerRenderer;
    public bool isDead = false;


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

    [Header("Hitbox")]
    public float minimum = 0.0f;
    public float maximum = 1f;
    public float duration = 10.0f;
    bool faded;
    private float startTime;
    [SerializeField] SpriteRenderer hitBoxSprite;
    [SerializeField] GameObject hitbox;

    [Header("Power Ups")]
    public int upgradeState = 0;
    GameObject shield;
    public Health health;

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
        // playerRenderer = gameObject.GetComponentInChildren<Renderer>();
        shootSoundFX = gameObject.GetComponent<AudioSource>();
        currentSpeed = moveSpeed;
        activePlayerTurrets = new List<GameObject>();
		activePlayerTurrets.Add(startWeapon);
        shield = transform.Find("Shield").gameObject;

        startTime = Time.time;
        faded = true;

    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(KeyCode.LeftShift))
        {
            // Fading();
            currentSpeed = focusSpeed;
            hitbox.SetActive(true);

        }
        else
        {
            currentSpeed = moveSpeed;
            hitbox.SetActive(false);

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

    void Fading()
    {
        float t = (Time.time - startTime) / duration;
        Color color = hitBoxSprite.material.color;
        if(faded)
        {
            
            color = new Color(1f,1f,1f,Mathf.SmoothStep(minimum, maximum, t));
            if(t > 1f)
            {
                faded = false;
                startTime = Time.time;
            }
        }
        else
        {
            color = new Color(1f,1f,1f,Mathf.SmoothStep(maximum, minimum, t));
            if(t > 1f)
            {
                faded = true;
                startTime = Time.time;
            }

        }
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

    
    void ACtiveShield()
    {
        shield.SetActive(true);
    }

    public void DeactiveShield()
    {
        shield.SetActive(false);
    }
    
    public bool HasShield()
    {
        return shield.activeSelf;
    }


    
    void OnTriggerEnter2D(Collider2D other) 
    {
		if (other.gameObject.tag == "BulletPowerup") 
        {
			PowerUp powerupScript = other.gameObject.GetComponent<PowerUp>();
			powerupScript.PowerupCollected();
			UpgradeWeapons();
		} 
        if(other.gameObject.tag == "ShieldPowerup")
        {
            PowerUp powerupScript = other.gameObject.GetComponent<PowerUp>();
            powerupScript.PowerupCollected();
            ACtiveShield();
        }
        if(other.gameObject.tag == "Health Pickup")
        {
            PowerUp powerupScript = other.gameObject.GetComponent<PowerUp>();
            powerupScript.PowerupCollected();
            health.Heal(10);
        }
        if (other.gameObject.tag == "Enemy Ship 1" || other.gameObject.tag == "Enemy Ship 2" || other.gameObject.tag == "Enemy Bullet" || other.gameObject.tag == "Enemy Ship 3") 
        {
            if(HasShield())
            {
                DeactiveShield();
                Instantiate(explosion, transform.position, transform.rotation);
            }

            else
            {
                health.Damage(10);
                Instantiate(explosion, transform.position, transform.rotation);

                if(health.health == 0)
                {
                    GameController.SharedInstance.ShowGameOver();  // If the player is hit by an enemy ship or laser it's game over.
                    playerRenderer.enabled = false;       // We can't destroy the player game object straight away or any code from this point on will not be executed
                    playerCollider.enabled = false;       // We turn off the players renderer so the player is not longer displayed and turn off the players collider
                    playerThrust.Stop();
                    Instantiate(explosion, transform.position, transform.rotation);   // Then we Instantiate the explosions... one at the centre and some additional around the players location for a bigger bang!
                    for (int i = 0 ; i < 8; i++) 
                    {
                        Vector3 randomOffset = new Vector3 (transform.position.x + Random.Range(-0.6f, 0.6f), transform.position.y + Random.Range(-0.6f, 0.6f), 0.0f); 
                        Instantiate(explosion, randomOffset, transform.rotation);
                    }

                    // Destroy(gameObject, 1.0f); // The second parameter in Destroy is a delay to make sure we have finished exploding before we remove the player from the scene.
                    gameObject.SetActive(false);
                    
                    // isDead = true;
                }  
		    }
        }
            
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

    // The ScatterShot turret is shot independantly of the z key
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
}
