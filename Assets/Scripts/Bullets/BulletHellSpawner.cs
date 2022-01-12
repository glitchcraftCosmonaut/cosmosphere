using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHellSpawner : MonoBehaviour
{
    public GameObject hitVFX;
    [SerializeField] public float damage;
    public int nrgValue;
    public bool isNRGProjectiles;
    // [SerializeField] public float duration = 2f;
    [SerializeField] public float continuousFireTimer = 2f;
    float currentTimer;
    
    [SerializeField] float angleShot = 360f;
    public int numberOfColumns;
    public float speed;
    public Sprite textureSprite;
    public Color color;
    public float lifeTime;
    public float fireRate;
    public float size;
    public float spinSpeed;
    private float time;
    private float angle;
    public Material material;

    public ParticleSystem system;
    public ParticleCollision particleCollision;
    // public ParticleDuration particleDuration;




    private void Awake()
    {
        currentTimer = continuousFireTimer;
        // transform.rotation = Quaternion.Euler(0,90,0);
        Summon();
    }

    private void OnEnable()
    {
        // Summon();
        StartCoroutine(nameof(DoEmit));
    }

    private void OnDisable() 
    {
        currentTimer = continuousFireTimer;
    }

    private void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
        currentTimer -= Time.deltaTime;
        currentTimer = Mathf.Clamp(currentTimer , 0, 100);
        transform.rotation = Quaternion.Euler(0,0, time * spinSpeed);
    }
    private void Update() 
    {
        if(currentTimer <= 0)
        {
            StopCoroutine(nameof(DoEmit));
        }
    }


    void Summon()
    {
        angle = angleShot / numberOfColumns;
        for(int i = 0; i < numberOfColumns; i++)
        {
            

            //standar material no texture
            Material particleMaterial = material;

            //create green particle system
            var particleSystemObject = new GameObject("Particle System");
            particleSystemObject.transform.Rotate(angle * i, 90 , 0); //Rotate so the system emits upward;
            // particleSystemObject.transform.Rotate(90, angle * i , 0); //Rotate so the system emits upward;
            particleSystemObject.transform.parent = this.transform;
            particleSystemObject.transform.position = this.transform.position;
            system = particleSystemObject.AddComponent<ParticleSystem>();
            particleCollision = particleSystemObject.AddComponent<ParticleCollision>();
            // particleDuration = particleSystemObject.AddComponent<ParticleDuration>();
            particleSystemObject.GetComponent<ParticleSystemRenderer>().material = particleMaterial;

            var mainModule = system.main;
            mainModule.startColor = Color.green;
            // mainModule.loop = false;
            mainModule.startSize = 0.5f;
            mainModule.startSpeed = speed;
            mainModule.maxParticles = 100000;
            mainModule.simulationSpace = ParticleSystemSimulationSpace.World;
            

            var emission = system.emission;
            emission.enabled = false;
            var shape = system.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Sprite;
            shape.sprite = null;

            var collision = system.collision;
            collision.enabled = true;
            collision.type = ParticleSystemCollisionType.World;
            collision.mode = ParticleSystemCollisionMode.Collision2D;
            collision.lifetimeLoss = 1f;
            collision.sendCollisionMessages = true;
            collision.collidesWith = LayerMask.GetMask("Player");
            // OnParticleCollision(player);

            var texture = system.textureSheetAnimation;
            texture.mode = ParticleSystemAnimationMode.Sprites;
            texture.AddSprite(textureSprite);
            texture.enabled = true;

        }

        //every 2 seconds will emit
        // InvokeRepeating("DoEmit", 1f, fireRate);
        
        // Invoke("DoEmit",1f);
        // StartCoroutine(nameof(BulletEmitting));
    }

    
    IEnumerator DoEmit()
    {
        // float fireRateComp = 0;
        while(true)
        {
            foreach(Transform child in transform)
            {   
                // system.Stop();
                system = child.GetComponent<ParticleSystem>();
                particleCollision = child.GetComponent<ParticleCollision>();
                // particleDuration = child.GetComponent<ParticleDuration>();
                //any parameter we assign in emitParams will override the current system's when call exit
                //there we will override the start color and size
                var emitParams = new ParticleSystem.EmitParams();
                // var mainModule = system.main;
                // mainModule.duration = duration;
                emitParams.startColor = color;
                emitParams.startSize = size;
                emitParams.startLifetime = lifeTime;
                system.Emit(emitParams, 10);
                // system.Play();
            }
            yield return new WaitForSeconds(fireRate);
            // yield return null;
        }
    }
    
}
