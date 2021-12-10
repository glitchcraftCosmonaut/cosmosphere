using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    // public bool activeShield;
    // public bool addGuns;
    private AudioSource powerupAudio;
	private CircleCollider2D powerupCollider;
	private Renderer powerupRenderer;
    // Start is called before the first frame update
    void Start()
    {
        powerupAudio = gameObject.GetComponent<AudioSource>();
		powerupCollider = gameObject.GetComponent<CircleCollider2D>();
		powerupRenderer = gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    public void PowerupCollected()
    {
        powerupCollider.enabled = false;
		powerupRenderer.enabled = false;
		powerupAudio.Play();
		Destroy(gameObject, powerupAudio.clip.length);
    }
}
