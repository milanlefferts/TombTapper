using UnityEngine;
using System.Collections;

public class DataController : MonoBehaviour {

	public static DataController instance = null;




	public string currentLevelName;
	public int currentLevelID;

	[HideInInspector]
	public string[] nextLevel;

	//public string[] completedLevels;

	void Awake() {
		if (instance == null) {
			instance = this;
		}
		else if (instance != this) {
			Destroy (this.gameObject);
		}
		
		DontDestroyOnLoad (this);
		
	}


	public void saveScore (int score) {
		switch (currentLevelName) {
		case "Bellowing Plains 1":
			hightScore_bp_1 = score;
			break;
			
		case "Bellowing Plains 2":
			hightScore_bp_2 = score;
			break;
			
		case "Bellowing Plains 3":
			hightScore_bp_3 = score;
			break;
			
		case "Bellowing Plains 4":
			hightScore_bp_4 = score;
			break;

		default:
			nextLevel = bp_1;
			break;
		}
	}

	public bool[] levelCompletedArray = new bool[]{false, false, false, false};

	public void LevelCompleted (int lvl) {
		levelCompletedArray [lvl] = true;
	}


	public void LoadLevel (string lvl) {
		GameManager gameManager = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager> ();
		//tileManager.TileSetup

		
	}
	

	public void SelectLevel (string lvl) {
		currentLevelName = lvl;
		switch (lvl) {
		case "Bellowing Plains 1":
			currentLevelID = 0;
			nextLevel = bp_1;
			break;

		case "Bellowing Plains 2":
			currentLevelID = 1;
			nextLevel = bp_2;
			break;

		case "Bellowing Plains 3":
			currentLevelID = 2;
			nextLevel = bp_3;
			break;

		case "Bellowing Plains 4":
			currentLevelID = 3;
			nextLevel = bp_4;
			break;


		default:
			nextLevel = bp_1;
			break;
		}

		Application.LoadLevel ("4_Game");

	}


	public int hightScore_bp_1;
	// Bellowing Plains I -> ID = 0
	private string[] bp_1 = new string[] {  
		" x"," x"," x"," x"," x"," x"," x"," x"," x"," x",
		
		" x"," x"," x"," x","ex"," x"," x"," x"," x"," x",
		
		" x"," x"," x"," x","e1"," x"," x"," x"," x"," x",
		
		" x"," x"," x","e1"," c","e1"," x"," x"," x"," x",
		
		" x"," x"," x"," x","  "," x"," x"," x"," x"," x",
		
		" x"," x"," x"," x"," d"," x"," x"," x"," x"," x",
		
		" x"," x"," x"," x","  "," x"," x"," x"," x"," x",
		
		" x"," x"," x"," x"," k","  "," x"," x"," x"," x",
		
		" x"," x"," x"," x","st"," x"," x"," x"," x"," x",
		
		" x"," x"," x"," x"," x"," x"," x"," x"," x"," x"
		
		
	};
	
	public int hightScore_bp_2;
	// Bellowing Plains II -> ID = 1
	private string[] bp_2 = new string[] {  
		" x"," x"," x"," x"," x"," x"," x"," x"," x"," x",
		
		" x"," s","e1","e1","st"," d","e1","  "," c"," x",
		
		" x"," k","e1","e1","pa"," x","  ","e1","  "," x",
		
		" x"," x"," x"," x"," x"," x"," x"," x","e1"," x",
		
		" x"," s","  ","  ","  ","e1","  ","e1","  "," x",
		
		" x"," x"," x","e1","hp","  ","e1","  ","co"," x",
		
		" x","ex"," x"," x","  "," x"," x"," x"," x"," x",
		
		" x","e1"," x"," x","  ","e1"," x"," x"," x"," x",
		
		" x","e1","e1","  ","e1","  ","e1","e1"," c"," x",
		
		" x"," x"," x"," x"," x"," x"," x"," x"," x"," x"
		
		
	};

	public int hightScore_bp_3;
	// Bellowing Plains III -> ID = 2
	private string[] bp_3 = new string[] {  
		" x"," x"," x"," x"," x"," x"," x"," x"," x"," x",
		
		" x","st","e1","e1","e1"," d","pa"," x"," k"," x",
		
		" x","e1"," x"," x"," x"," x","e1"," x","e1"," x",
		
		" x","e1"," x"," c"," x"," x","  "," x","  "," x",
		
		" x","e1"," x","e1"," d","  ","e1","co","  "," x",
		
		" x"," k"," x","e1"," x"," x"," x"," x"," x"," x",
		
		" x"," x","hp","e1"," s","  ","  ","e1"," c"," x",
		
		" x"," x"," x"," x"," x","e1"," x"," x"," x"," x",
		
		" x"," k","e1","e1","e1","  ","e1"," d","e"," x",
		
		" x"," x"," x"," x"," x"," x"," x"," x"," x"," x"
		
		
	};

	public int hightScore_bp_4;
	// Bellowing Plains IV -> ID = 3
	private string[] bp_4 = new string[] {  
		" x"," x"," x"," x"," x"," x"," x"," x"," x"," x",
		
		" x","hp","e1","pa","e1","e1","e1","e1","ex"," x",
		
		" x","  ","  ","e1","pd"," x"," x"," x"," x"," x",
		
		" x","e1"," x"," x"," x"," x","  "," c","  "," x",
		
		" x","  ","e1","co","  "," ?","e1"," c","  "," x",
		
		" x","e1","  "," x","e1"," x","e1"," c","  "," x",
		
		" x","  ","  "," x","  "," x","  "," c","  "," x",
		
		" x","e1"," s"," x","  "," x"," x"," x"," x"," x",
		
		" x"," c","  "," x","  ","e1","e1","e1","st"," x",
		
		" x"," x"," x"," x"," x"," x"," x"," x"," x"," x"
		
		
	};




	/*

	// s = shrine | e1 = enemy | c = chest | x = wall | "  " = empty | st = start | ex = exit	
	// k = key | d = door | ? = secret wall | pa = powerup attack | pd = powerup defense 
	// hp = health pickup | co = combo pickup
	private string[] empty8x8 = new string[] {  
		" x"," x"," x"," x"," x"," x"," x"," x"," x"," x",
		
		" x","  ","  ","  ","  ","  ","  ","  ","  "," x",
		
		" x","  ","  ","  ","  ","  ","  ","  ","  "," x",
		
		" x","  ","  ","  ","  ","  ","  ","  ","  "," x",
		
		" x","  ","  ","  ","  ","  ","  ","  ","  "," x",
		
		" x","  ","  ","  ","  ","  ","  ","  ","  "," x",
		
		" x","  ","  ","  ","  ","  ","  ","  ","  "," x",
		
		" x","  ","  ","  ","  ","  ","  ","  ","  "," x",
		
		" x","  ","  ","  ","  ","  ","  ","  ","  "," x",
		
		" x"," x"," x"," x"," x"," x"," x"," x"," x"," x"
		
		
	};

*/




}
