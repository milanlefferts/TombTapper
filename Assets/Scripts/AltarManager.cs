using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AltarManager : MonoBehaviour {

	private Player player;
	private bool playerLoaded;

	public Dictionary<string, Altar> altars = new Dictionary<string, Altar>();
	public Sprite[] altarNormalSprites;
	public Sprite[] altarDesecratedSprites;

	public string[] altarList;

	private GameManager gameManager;


	void Awake () {

		altars.Add ("RearrangePlayerElements", new Altar (0,
		                                                 	"RearrangePlayerElements",
		                           							"Shake it up!",
		                                                 	RearrangePlayerElements,
		                          							altarNormalSprites [0],
		                          							altarDesecratedSprites [0]));

		altars.Add ("RearrangeEnemyElements", new Altar (1,
		                                         "RearrangeEnemyElements",
		                                         "Rearranged!",
		                                         RearrangeEnemyElements,
		                                         altarNormalSprites [0],
		                                         altarDesecratedSprites [0]));

		altars.Add ("TimeBonus", new Altar (2,
		                                 		"TimeBonus",
		                                         "Bonus Time!",
		                                   		 TimeBonus,
		                                         altarNormalSprites [0],
		                                         altarDesecratedSprites [0]));
		altarList = new string[] {"RearrangePlayerElements", "RearrangeEnemyElements", "TimeBonus"};

	}


	void Start () {
		gameManager = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager> ();

	}

	void Update () {
		if (gameManager.mapLoaded) {
			if (!playerLoaded) {
				player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
				playerLoaded = true;
			}
		}
	}


	void RearrangePlayerElements () {
		Element[] eleArray = new Element[] {player.element1, player.element2, player.element3, player.element4};
		for (int i = 0; i < eleArray.Length; i++) {
			StartCoroutine (player.RotateElement(eleArray[i], i + 1));
		}

	





	}

	void RearrangeEnemyElements () {
		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");

		foreach (GameObject obj in enemies) {
			CreatureTile crea = obj.GetComponent<CreatureTile>();

			crea.element = crea.elementList[Random.Range (0, crea.elementList.Length)];

			crea.SelectElement (crea.element);
		}
	}

	void TimeBonus () {

		GameManager gameManager = this.GetComponent<GameManager> ();
		float timeAddition = 10f;
		if (gameManager.timerCurrent + timeAddition > gameManager.timerMax) {
		
			gameManager.timer += (gameManager.timerMax - gameManager.timerCurrent);
		} else {
			gameManager.timer += timeAddition;
		}

		   

	}



}
