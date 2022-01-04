using UnityEngine;
using System.Collections;

public class EnemyDroneController : MonoBehaviour {

	public GameObject[] powerUp;
	public GameObject explosion;
	public GameObject smallEnemyBullet;
	public float minReloadTime = 1.0f;
	public float maxReloadTime = 2.0f;
	private RipplePostProccessor camRipple;




	private void Start() 
	{
		camRipple = Camera.main.GetComponent<RipplePostProccessor>();
		StartCoroutine("Shoot");
	}
	
	IEnumerator Shoot() 
	{
		yield return new WaitForSeconds((Random.Range(minReloadTime, maxReloadTime)));
		while (true) 
		{
			Instantiate(smallEnemyBullet, gameObject.transform.position, gameObject.transform.rotation);
			yield return new WaitForSeconds((Random.Range(minReloadTime, maxReloadTime)));
		}
	}

	void OnTriggerExit2D(Collider2D other) 
	{
		if (other.gameObject.tag == "Boundary" && other.gameObject.name == "Left") 
		{
			gameObject.SetActive(false);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player Bullet") 
		{
      		GameController.SharedInstance.IncrementScore(10);
			float randomNumber = Random.Range(0.0f, 20.0f);
			if (randomNumber > 19.0f) 
			{
				Instantiate(powerUp[Random.Range(0, powerUp.Length)], gameObject.transform.position, gameObject.transform.rotation);
			}
			Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation);
			camRipple.RippleEffect();
			other.gameObject.SetActive(false);
			gameObject.SetActive(false);
		}
	}
}
