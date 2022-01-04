using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController: MonoBehaviour 
{

	public static GameController SharedInstance;

	[Header("Scoring")]
	// public Text scoreLabel;
	private int currentScore = 0;

	
	[Header("MENU")]
	public GameObject pauseMenu;
	public GameObject gameOverMenu;
	public bool isPaused;
	public bool isGameOver;

	[Header("Enemy Manager")]
	public GameObject enemyType1;
	public GameObject enemyType2;
	public GameObject enemyType3;

	public GameObject subBoss;

	public float startWait = 1.0f;
	public float waveInterval = 2.0f;
	public float spawnInterval = 0.5f;
	public int enemiesPerWave = 5;


	[Header("Boundaries")]
	public GameObject leftBoundary;                   //
	public GameObject rightBoundary;                  // References to the screen bounds: Used to ensure the player
	public GameObject topBoundary;                    // is not able to leave the screen.
	public GameObject bottomBoundary;                 //


	[Header("Data Scoring")]
	[SerializeField] public PointsHUD pointsHUD;
	[SerializeField] public HighScoreHandler highScoreHandler;
	// [SerializeField] public string playerName;
	[SerializeField] public InputField playerName;
	[SerializeField] PlayerMech player;


	void Awake () 
	{
		SharedInstance = this;
	}

	void Start () 
	{
		StartCoroutine(SpawnEnemyWaves());
		Cursor.visible = false;
		// OnDeath();
	}
	private void Update() 
	{
		if(!isGameOver)
		{
			if(Input.GetKeyDown(KeyCode.Escape))
			{
				if(isPaused)
				{
					Resume();
				}
				else
				{
					Pause();
				}
			}
			if(isGameOver == true)
			{
				ShowGameOver();
			}
		}
	}

	IEnumerator SpawnEnemyWaves () 
	{
		yield return new WaitForSeconds (startWait);
		while (true) 
		{
		  float waveType = Random.Range(0.0f, 10.0f);
			for (int i = 0 ; i < enemiesPerWave; i++) 
			{
				// Vector3 topLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight + 2, 0));
				Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight + 2, 0));
				Vector3 bottomRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, 0, 0));
				Vector3 spawnPosition = new Vector3 (bottomRight.x,Random.Range(bottomRight.y, topRight.y), 0);
				Quaternion spawnRotation = Quaternion.Euler(0, 0, 0);
				if (waveType >= 5.0f) 
				{
					GameObject enemy1 = ObjectPooler.SharedInstance.GetPooledObject("Enemy Ship 1"); 
					if (enemy1 != null) 
					{
						enemy1.transform.position = spawnPosition;
						enemy1.transform.rotation = spawnRotation;
						enemy1.SetActive(true);
					}
				}
				else if(waveType >= 3.0f)
				{
					GameObject enemy2 = ObjectPooler.SharedInstance.GetPooledObject("Enemy Ship 2"); 
					if (enemy2 != null) 
					{
						enemy2.transform.position = spawnPosition;
						enemy2.transform.rotation = spawnRotation;
						enemy2.SetActive(true);
					}
				}
				else 
				{
					GameObject enemy3 = ObjectPooler.SharedInstance.GetPooledObject("Enemy Ship 3"); 
					if (enemy3 != null) 
					{
						enemy3.transform.position = spawnPosition;
						enemy3.transform.rotation = spawnRotation;
						enemy3.SetActive(true);
					}
				} 
        	yield return new WaitForSeconds (spawnInterval);
			} 
      	yield return new WaitForSeconds (waveInterval);
		}
	}

	public void IncrementScore(int increment) 
	{
		currentScore += increment;
		pointsHUD.Points += currentScore;
	}
	public void OnDeath()
	{
		// player.gameObject.SetActive(false);
		// // if(player.isDead == true)
		// // {
		// // 	// highScoreHandler.SetHighScoreIfGreater(pointsHUD.Points);
		// // 	highScoreHandler.AddHighscoreIfPossible(new HighScoreElement(playerName, pointsHUD.Points));
		// // }
		// highScoreHandler.AddHighscoreIfPossible(new HighScoreElement(playerName, pointsHUD.Points));

	}
	

	public void ShowGameOver() 
	{
		gameOverMenu.SetActive(true);
		Cursor.visible = true;
		isGameOver = true;
	}

	public void Pause()
	{
		pauseMenu.SetActive(true);
		Time.timeScale = 0;
		AudioListener.pause = true;
		Cursor.visible = true;
		isPaused = true;

	}

	public void Resume()
	{
		pauseMenu.SetActive(false);
		Time.timeScale = 1;
		AudioListener.pause = false;
		Cursor.visible = false;
		isPaused = false;
	}

	public void RestartGame() 
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		isGameOver = false;
	}

	public void Quit()
	{
		SceneManager.LoadScene("Main Menu");
	}
}
