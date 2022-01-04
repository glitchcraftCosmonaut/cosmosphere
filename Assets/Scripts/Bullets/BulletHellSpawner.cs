using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHellSpawner : MonoBehaviour
{
    public GameObject hitVFX;
    [SerializeField] public float damage;
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




    private void Awake()
    {
        Summon();
    }

    private void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
        transform.rotation = Quaternion.Euler(0,0, time * spinSpeed);
    }


    void Summon()
    {
        angle = 360f / numberOfColumns;
        for(int i = 0; i < numberOfColumns; i++)
        {
            //standar material no texture
            Material particleMaterial = material;

            //create green particle system
            var particleSystemObject = new GameObject("Particle System");
            particleSystemObject.transform.Rotate(angle * i, 90 , 0); //Rotate so the system emits upward;
            particleSystemObject.transform.parent = this.transform;
            particleSystemObject.transform.position = this.transform.position;
            system = particleSystemObject.AddComponent<ParticleSystem>();
            particleCollision = particleSystemObject.AddComponent<ParticleCollision>();
            
            particleSystemObject.GetComponent<ParticleSystemRenderer>().material = particleMaterial;
            var mainModule = system.main;
            mainModule.startColor = Color.green;
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
        InvokeRepeating("DoEmit", 0f, fireRate);

    }

    
    void DoEmit()
    {
        foreach(Transform child in transform)
        {   
            system = child.GetComponent<ParticleSystem>();
            particleCollision = child.GetComponent<ParticleCollision>();
            //any parameter we assign in emitParams will override the current system's when call exit
            //there we will override the start color and size
            var emitParams = new ParticleSystem.EmitParams();
            emitParams.startColor = color;
            emitParams.startSize = size;
            emitParams.startLifetime = lifeTime;
            system.Emit(emitParams, 10);
        }
    }
    
}
