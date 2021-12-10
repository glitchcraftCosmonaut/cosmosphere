using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;

	[SerializeField] float randomMinY, randomMaxY, randomMinX, randomMaxX;

	private float maxX;
	private float minX;
	private float maxY;
	private float minY;

	private float tChange = 0.0f; // force new direction in the first Update
	private float randomX;
	private float randomY;

  void Start () 
  {
        maxX = GameController.SharedInstance.rightBoundary.transform.position.x;
        minX = GameController.SharedInstance.leftBoundary.transform.position.x;
        maxY = GameController.SharedInstance.topBoundary.transform.position.y;
        minY = GameController.SharedInstance.bottomBoundary.transform.position.y;
  }

	void Update() 
    {
		// change to a new random direction at random intervals
		if (Time.time >= tChange) 
        {
			randomX = Random.Range(randomMinX, randomMaxX);
			randomY = Random.Range(randomMinY, randomMaxY); //  between -2.0 and 2.0 is returned
			// set a random interval between 0.5 and 1.5
			tChange = Time.time + Random.Range(0.5f,1.5f);
		}
		Vector3 newPosition = new Vector3(randomX, randomY, 0);
		transform.Translate(newPosition * moveSpeed * Time.deltaTime);
		// if any boundary is hit, change direction.
		if (transform.position.x >= maxX || transform.position.x <= minX) 
        {
			randomX = -randomX;
		}
		if (transform.position.y >= maxY || transform.position.y <= minY) 
        {
			randomY = -randomY;
		}
		Vector3 clampedPosition = transform.position;
		clampedPosition.x = Mathf.Clamp(transform.position.x, minX, maxX);
		clampedPosition.y = Mathf.Clamp(transform.position.y, minY, maxY);
		transform.position = clampedPosition;
	}
}
