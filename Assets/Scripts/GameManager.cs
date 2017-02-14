using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {
	
	public static GameManager instance = null;
	//public TileManager tileManager;
	private DataController dataController;
	private Player player;
	
	// Map
	public int columns;
	public int rows;
	
	public GameObject[] tileArray;
	public GameObject[] tileArrayEmpty;
	public GameObject[] tileArrayEdge;
	
	private Transform tileHolder;
	
	public GameObject partyObject;
	
	public bool mapLoaded;
	//
	
	public bool gameStarted = false;


	public float timer;
	public float timerMax;
	public float timerCurrent;


	private GameObject timerUI;

	public bool levelCompleted;
	public bool levelFailed;


	// Score
	public int damageDone;
	public int damageTaken;
	public int enemiesSlain;
	public int itemsFound;
	public int altarsActivated;

	private GameObject overlay;
	private GameObject scoreUI;
	private GameObject comboUI;



	
	void Awake() {
		if (instance == null) {
			instance = this;
		}
		else if (instance != this) {
			Destroy (gameObject);
		}

		dataController = GameObject.FindGameObjectWithTag ("DataController").GetComponent <DataController>();

	}

	void Start () {
		TileSetup (dataController.nextLevel, 8, 8);

	
	}

	void Update () {



		if (mapLoaded) {

			if (!gameStarted) {
				StartCoroutine (StartGame());
			}

			timerCurrent = (timer - Time.time);
			timerUI.GetComponent<Image> ().fillAmount = (float)timerCurrent / (float)timerMax;


			if (timerCurrent <= 0) {
				StartCoroutine (EndGame (false));
			}
		}


	}

	public IEnumerator StartGame() {
		gameStarted = true;
		overlay = GameObject.FindGameObjectWithTag ("Overlay");
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();;
		
		levelCompleted = false;
		levelFailed = false;
		timer = columns * 10f;
		timerMax = timer;
		
		timerUI = GameObject.FindGameObjectWithTag ("Timer");


		//overlay.SetActive (true);

		overlay.GetComponentInChildren<Text>().text = dataController.currentLevelName;

		float alpha = 1.0f;
		int iterations = 30;

		for (int i = 0; i < iterations; i++) {
			alpha -= (1.0f / (float)iterations);

			overlay.GetComponent<Image>().color = new Color(0, 0, 0, alpha);
			yield return new WaitForSeconds(2.0f / (float)iterations);

		}

		overlay.SetActive (false);
		timerCurrent = timer;


	}

	public IEnumerator EndGame(bool success) {
		levelFailed = true;

		if (!success) {

			overlay.SetActive (true);

			overlay.GetComponentInChildren<Text>().text = "Decimated..";

			float alpha = 0.0f;
			int iterations = 30;
			
			for (int i = 0; i < iterations; i++) {
				alpha += (1.0f / (float)iterations);
				overlay.GetComponent<Image>().color = new Color(0, 0, 0, alpha);
				yield return new WaitForSeconds(2.0f / (float)iterations);


			}

			dataController.saveScore (player.score);
			Application.LoadLevel ("3_LevelSelect");

			//Application.LoadLevel (Application.loadedLevel);

		} else if (success) {
			overlay.SetActive (true);

			overlay.GetComponentInChildren<Text>().text = "You Survived!";

			float alpha = 0.0f;
			int iterations = 30;
			
			for (int i = 0; i < iterations; i++) {
				alpha += (1.0f / (float)iterations);
				
				overlay.GetComponent<Image>().color = new Color(0, 0, 0, alpha);
				yield return new WaitForSeconds(2.0f / (float)iterations);
				
			}
			int finalTimer = (int)timerCurrent;

			overlay.GetComponentInChildren<Text>().text = player.score + " Total Score";
			
			yield return new WaitForSeconds(0.5f);
			
			overlay.GetComponentInChildren<Text>().text = 
			"Total Score " + player.score + "\n" +
					"Time Left " + finalTimer;
			yield return new WaitForSeconds(0.5f);
			
			overlay.GetComponentInChildren<Text>().text = 
				
				"Total Score " + player.score + "\n" +
					"Time Left " + finalTimer + "\n" +
					"Max Combo " + player.maxCombo;
			
			yield return new WaitForSeconds(0.5f);
			


			dataController.saveScore (player.score);
			dataController.LevelCompleted (dataController.currentLevelID);

			Application.LoadLevel ("3_LevelSelect");
		}
	}




	public void TileSetup (string[] map, int col, int row) {
		columns = col;
		rows = row;
		
		tileHolder = new GameObject ("Map").transform;
		int index = 0;
		GameObject newTile = new GameObject();
		for (int y = (rows + 2); y > -0; y--) {
			for (int x = -1; x < columns + 1; x++){
				
				//	for (int y = -1 ; y < rows + 1; y--) {
				//		for (int x = -1; x < columns + 1; x--) {
				
				if (x == -1 || x == columns || y == (rows + 2) || y == 1) {
					newTile = tileArrayEdge[Random.Range (0, tileArrayEdge.Length-1)];
					
				}
				
				else {
					newTile = SelectTile(map[index]);
					
					
				}
				
				index++;
				
				GameObject spawnedTile = Instantiate (newTile, new Vector3(x,y,0f), Quaternion.identity) as GameObject;
				
				
				spawnedTile.transform.SetParent(tileHolder);
			}
			
		}


		Instantiate (partyObject, new Vector3 (0, 0, -1), Quaternion.identity);
		
		mapLoaded = true;

	}
	
	void TileSetupRandom () {
		tileHolder = new GameObject ("Map").transform;
		
		// Random Level Generator
		for (int y = -1; y < rows + 1; y++) {
			for (int x = -1; x < columns + 1; x++) {
				
				
				GameObject newTile = tileArray[Random.Range (0, tileArray.Length)];
				
				if (x == -1 || x == columns || y == -1 || y == rows) {
					newTile = tileArrayEdge[Random.Range (0, tileArrayEdge.Length)];
				}
				
				GameObject spawnedTile = Instantiate (newTile, new Vector3(x,y,0f), Quaternion.identity) as GameObject;
				
				
				spawnedTile.transform.SetParent(tileHolder);
			}
			
		}
		
	}
	
	
	GameObject SelectTile(string tiletype) {
		switch (tiletype) {
			
		case " x": // Wall
			
			return tileArrayEdge[Random.Range (0, tileArrayEdge.Length)];
			
		case "  ": // Empty 
			
			return tileArrayEmpty[Random.Range (0, tileArrayEmpty.Length)];
			
		case " s": // Shrine
			
			return tileArray[2];
			
		case " c": // Chest
			
			return tileArray[3];
			
			
		case "e1": // Enemy 1
			
			return tileArray[0];
			
		case "st": // Start
			return tileArray[4];
			
		case "ex": // Exit
			return tileArray[5];
			
		case " k": // Key
			return tileArray[6];
			
		case " d": // Door
			return tileArray[7];
			
		case " ?": // Secret Wall
			return tileArray[8];
			
		case "hp": // Health Tile
			return tileArray[9];
			
		case "pa": // Powerup Attack
			return tileArray[10];
			
		case "pd": // Powerup Defense
			return tileArray[11];
			
		case "co": // Combo Pickup
			return tileArray[12];
			
		default:
			return tileArray[1];
			
		}
		
	}
	

	
	
	
	
	/*
		// s = shrine | e1 = enemy | c = chest | x = wall | "  " = empty | st = start | ex = exit	
		// k = key | d = door | ? = secret wall | pa = powerup attack | pd = powerup defense 
		// hp = health pickup | co = combo pickup
	private string[] level1 = new string[] {  
		" x"," x"," x"," x"," x"," x"," x"," x"," x"," x",
		
		" x","ex","  ","  ","  ","  ","  ","  ","  "," x",
		
		" x"," s","  ","  "," s","ex","  ","e1"," c"," x",
		
		" x","  ","  ","  ","  ","  ","e1","  ","  "," x",
		
		" x","  ","  ","  ","e1"," c"," s","  ","e1"," x",
		
		" x"," s","  ","  ","  ","e1","  ","  ","  "," x",
		
		" x","  ","  ","e1","  ","  ","  ","  ","  "," x",
		
		" x","  ","  ","  ","  ","e1","  ","e1","  "," x",
		
		" x","  ","  ","e1","  ","  "," c","  ","  "," x",
		
		" x","  ","  ","  ","  ","  ","  ","  ","  "," x",
		
		" x","  ","  ","  ","  ","  ","  ","  ","  "," x",
		
		" x","e1","  ","  ","  ","  ","  ","  ","  "," x",
		
		" x"," s","  ","  "," s","  ","  ","e1"," c"," x",
		
		" x","  ","  ","  ","  ","  ","e1","  ","  "," x",
		
		" x","  ","  ","  ","e1"," c"," s","  ","e1"," x",
		
		" x","e1"," x","  ","  ","e1","  ","  ","  "," x",
		
		" x","e1"," x","  ","e1","  ","e1","  ","  "," x",
		
		" x","  "," x"," x","  ","  ","  "," x","  "," x",
		
		" x"," c"," x"," x"," x","st"," x"," x","  "," x",
		
		" x"," x"," x"," x"," x"," x"," x"," x"," x"," x"
	};*/
	/*
	private string[] testlevel1 = new string[] { 

		// Test Level 1
		// s = shrine | e1 = enemy | c = chest | x = wall | "  " = empty | st = start | ex = exit	
		// k = key | d = door | ? = secret wall | pa = powerup attack | pd = powerup defense 
		// hp = health pickup | co = combo pickup	
		" x"," x"," x"," x"," x"," x"," x"," x"," x"," x"," x"," x"," x"," x"," x"," x"," x"," x"," x"," x"," x"," x",
		
		" x"," x"," x"," x"," x"," x"," x"," x"," x"," x"," x"," x"," x"," x"," x"," x"," x"," x"," x"," x"," x"," x",

		" x"," k","  "," x","  ","  "," c"," x"," x","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "," x"," x"," x",
		
		" x","e1","e1"," x","  ","  ","  "," x"," x","  ","  "," x"," x"," x"," x"," x"," x","  ","  ","  "," x"," x",
		
		" x","  ","  "," x","  ","  ","  "," x"," x","  "," x"," x","  ","  ","  ","e1","  "," x","  ","  "," x"," x",
		
		" x","  ","  "," x","  ","  ","  "," x"," x","  "," x","  ","  "," x"," x"," x","  ","  "," x","  "," x"," x",
		
		" x"," x","  ","  ","  ","  ","  "," x"," x","  "," x","  "," x","ex","e1","e1"," x","  "," x","  "," x"," x",
		
		" x"," x","  ","  ","  ","  ","  "," x"," x","  "," x","  "," x"," x","e1"," c"," x","  "," x","  "," x"," x",
		
		" x","  "," x","  ","  ","  ","  "," x"," x","  "," x","  ","  ","  ","  "," x","  ","  "," x","e1"," x"," x",
		
		" x","  ","  "," x","  ","  ","  ","  "," x","  ","  "," x"," s","  "," x","  ","  "," x","  ","  "," x"," x",
		
		" x","  ","  "," x","  ","  ","  ","  ","  "," x","  ","  "," x"," x","  ","  "," x","  ","  "," x","  "," x",
		
		" x","  ","  "," x","  ","  ","  ","  ","  ","  "," x","  ","  ","  ","  "," x","  ","e1"," x","  ","  "," x",
		
		" x","  "," x","  ","  ","  ","  ","  ","  ","  ","  "," x"," x"," x"," x","  ","  "," x","  ","  ","  "," x",
		
		" x"," x","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "," x","  ","  ","  "," x",
		
		" x","  ","  ","  ","  ","e1"," x"," x","e1"," x","  ","  ","  ","  ","  ","  ","  "," x","  ","  ","  "," x",
		
		" x","  "," x","  ","e1","e1"," x","  ","  "," x"," x"," x"," d"," x"," x","  ","  "," x","  ","  ","  "," x",
		
		" x","  ","  "," x"," x"," c"," x","e1"," x"," x"," x","  ","  ","  ","  "," x"," ?"," x","  ","  ","  "," x",
		
		" x"," ?","e1"," x"," x"," x"," x","  ","  "," x"," x","  ","  ","  ","  "," x"," ?"," x","  ","  ","  "," x",
		
		" x","e1","  ","  "," x"," x","  ","  ","  ","  "," x","  ","  ","  ","  "," x","  ","  ","  ","  ","  "," x",
		
		" x","  ","  ","  ","  "," x","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "," x","  ","  ","  ","  "," x",
		
		" x"," x","  ","  ","e1","  ","  ","  ","  ","  ","  ","e1","  ","  ","  ","  ","  "," x","  ","  "," c"," x",
		
		" x"," x"," x","  "," s","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "," x"," x"," x"," x",
		
		" x"," x"," x"," x","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","e1","  ","  ","  "," x"," x"," x"," x",
		
		" x"," x"," x"," x"," x","  ","  ","  ","  ","e1"," s","e1"," c","  ","  ","  ","  ","  "," x"," x"," x"," x",
		
		" x"," x"," x"," x"," x"," x","  ","  ","  ","e1","e1","e1","e1","  ","  ","  ","  "," x"," x"," x"," x"," x",
		
		" x"," x"," x"," x"," x"," x"," x","  ","  ","e1","e1","hp"," s","  ","  "," x"," x"," x"," x"," x"," x"," x",
		
		" x"," x"," x"," x"," x"," x"," x"," x","e1","e1"," s","e1"," c","  "," x"," x"," x"," x"," x"," x"," x"," x",
		
		" x"," x"," x"," x"," x"," x"," x"," x"," x","pd"," s","pa","  "," x"," x"," x"," x"," x"," x"," x"," x"," x",
		
		" x"," x"," x"," x"," x"," x"," x"," x"," x"," x"," d"," x"," x"," x"," x"," x"," x"," x"," x"," x"," x"," x",
		
		" x"," x"," x"," x"," x"," x"," x"," x"," x"," x","co"," k"," x"," x"," x"," x"," x"," x"," x"," x"," x"," x",
		
		" x"," x"," x"," x"," x"," x"," x"," x"," x"," x","st","  "," x"," x"," x"," x"," x"," x"," x"," x"," x"," x",
		
		" x"," x"," x"," x"," x"," x"," x"," x"," x"," x","x "," x"," x"," x"," x"," x"," x"," x"," x"," x"," x"," x"
		// s = shrine | e1 = enemy | c = chest | x = wall | "  " = empty | st = start | ex = exit	
		// k = key | d = door | ? = secret wall | pa = powerup attack | pd = powerup defense 
		// hp = health pickup | co = combo pickup

	};

	*/
	/*
	private string[] empty20x30 = new string[] { 

		// s = shrine | e1 = enemy | c = chest | x = wall | "  " = empty | st = start | ex = exit	
		// k = key | d = door | ? = secret wall | pa = powerup attack | pd = powerup defense 
		// hp = health pickup | co = combo pickup

		" x","  "," x"," x"," x"," x"," x"," x"," x"," x","  "," x"," x"," x"," x"," x"," x"," x"," x","  "," x"," x",
		
		" x","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "," x",
		
		" x","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "," x",
		
		" x","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "," x",
		
		" x","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "," x",
		
		" x","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "," x",
		
		" x","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "," x",
		
		" x","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "," x",
		
		" x","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "," x",
		
		" x","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "," x",
		
		" x","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "," x",
		
		" x","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "," x",
		
		" x","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "," x",
		
		" x","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "," x",
		
		" x","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "," x",
		
		" x","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "," x",
		
		" x","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "," x",
		
		" x","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "," x",
		
		" x","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "," x",
		
		" x","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "," x",
		
		" x","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "," x",

		" x","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "," x",
		
		" x","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "," x",
		
		" x","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "," x",
		
		" x","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "," x",
		
		" x","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "," x",
		
		" x","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "," x",
		
		" x","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "," x",
		
		" x","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "," x",
		
		" x","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "," x",
		
		" x","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "," x",
		
		" x","  "," x"," x"," x"," x"," x"," x"," x"," x","  "," x"," x"," x"," x"," x"," x"," x"," x","  "," x"," x"
	};
*/





}
