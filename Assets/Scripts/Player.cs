using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;


public class Element {
	
	public string name;
	public Color color;
	public Sprite character; 
	
	
	public Element(string aname, Color col, Sprite cha) {
		
		name = aname;
		color = col;
		character = cha;
	}
	
	public Element() {
		
	}
}


public class Player : MonoBehaviour {

	// Player Stats
	public string playerName;
	public Sprite characterSprite;

	public int healthCurrent;
	public int healthMax;
	public bool isInDanger;
	public bool isDead;
	public Sprite[] hearts;

	public int damage;

	public int attackBonus;
	public bool hasAttackBonus;
	public float attackBonusTimer;
	public int defenseBonus;
	public bool hasDefenseBonus;
	public float defenseBonusTimer;



	public int score;
	private Text scoreText;

	public int combo;
	private Text comboText;
	public int maxCombo;

	public Dictionary<string, Element> elements = new Dictionary<string, Element> ();
	
	public Element selectedElement;
	public Element element1;
	public Element element2;
	public Element element3;
	public Element element4;

	public Sprite[] elementCharacterSprites;
	
	public string selectedElementName;

	public List<Element> elementList;

	public int selectedElementPos;


	// UI

	public SpriteRenderer healthbar;

	public GameObject elementUI;
	public GameObject textBox;
	public float textWaitTime;
	public bool isShowingText;

	public GameObject element1UI;
	public Color element1UI_Color;
	public Text element1UI_Name;
	public Image element1UI_Image;

	// element 2 (top right)
	public GameObject element2UI;
	public Color element2UI_Color;
	public Text element2UI_Name;
	public Image element2UI_Image;

	public GameObject element3UI;
	public Color element3UI_Color;
	public Text element3UI_Name;
	public Image element3UI_Image;

	public GameObject element4UI;
	public Color element4UI_Color;
	public Text element4UI_Name;
	public Image element4UI_Image;
	

	

// ___________Key__________________________________

	public bool hasKey;



// _________Sound_________________

	public AudioClip deathSound;
	public AudioClip attackStrongSound;
	public AudioClip attackWeakSound;
	public AudioClip attackNeutralSound;


// ___________General_______________________________

	private GameObject start;
	private GameManager gameManager;

	private BoxCollider2D boxCollider;     
	private Rigidbody2D rigidBody;   

	private Vector2 touchOrigin;
	private Rect topTouchArea;
	private Rect bottomTouchArea;

	private float moveTime = 0.1f;           // movement speed
	public LayerMask blockingLayer;  

	public int oldHorizontal;
	public int oldVertical;
	
	private float actionTimer;
	public float actionDelay;
	private float inverseMoveTime;          // more efficient movement calculation

	// testing
	private bool canMove;
	private float sqrRemainingDistance;



		void Awake () {
			
			elements.Add ("Fire", new Element ("Fire", new Color (1f, 0.5f, 0.5f), elementCharacterSprites[0]));
			elements.Add ("Water", new Element ("Water", new Color(0f, 0.5f, 1f), elementCharacterSprites[1]));
			elements.Add ("Earth", new Element ("Earth", new Color (0.5f, 0.35f, 0f), elementCharacterSprites[2]));
			elements.Add ("Storm", new Element ("Storm", new Color (1f, 1f, 0.5f), elementCharacterSprites[3]));
			elements.Add ("Holy", new Element ("Holy", new Color (1f, 0.5f, 0f), elementCharacterSprites[4]));
			elements.Add ("Nature", new Element ("Nature", new Color(0f, 0.5f, 0f), elementCharacterSprites[5]));
			elements.Add ("Arcane", new Element ("Arcane", new Color (0.75f, 0.5f, 0.75f), elementCharacterSprites[6]));
			elements.Add ("Death", new Element ("Death", new Color (0.3f, 0.3f, 0.3f), elementCharacterSprites[7]));
			
			
			element1 = elements ["Fire"];
			element2 = elements ["Water"];
			element3 = elements ["Earth"];
			element4 = elements ["Storm"];
			
			playerName = "Bobby";
			
			damage = 1;
			
			healthCurrent = 12;
			
			healthMax = 12;
			isInDanger = false;
			
			score = 0;
			combo = 0;
		}

	

	void Start () {
		start = GameObject.FindGameObjectWithTag("Start");

		healthbar = transform.FindChild ("health").GetComponent<SpriteRenderer>();

		if (start != null) {
			transform.position = new Vector3(start.transform.position.x, start.transform.position.y, -1);
		}



		gameManager = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager> ();


		boxCollider = GetComponent<BoxCollider2D> ();
		rigidBody = GetComponent<Rigidbody2D> ();

		actionDelay = 0.3f;
		inverseMoveTime = 1f / moveTime;

		//topTouchArea = new Rect (0, Screen.height / 3, Screen.width, Screen.height - Screen.height / 3);
		//bottomTouchArea = new Rect (0, 0, Screen.width, Screen.height / 3);

		scoreText = GameObject.FindGameObjectWithTag("UI").transform.FindChild ("Score").GetComponentInChildren<Text> ();
		comboText = GameObject.FindGameObjectWithTag("UI").transform.FindChild ("Combo").GetComponentInChildren<Text> ();
		score = 0;
		combo = 0;

		// UI Setup
		elementUI = GameObject.FindGameObjectWithTag ("ElementUI");

		isShowingText = false;

		SetupUI ();

		SelectElement1();
	

		UpdateUI ();


	}

	void Update () {

		//UpdateUI ();

		if (Input.GetKeyDown (KeyCode.Alpha7)) {
			SelectElement1();
		}
				//private Color damagedUIcolor = new Color(1F, 1F, 1f, 1f);
		if (Input.GetKeyDown (KeyCode.Alpha8)) {
			SelectElement2();
		}

		if (Input.GetKeyDown (KeyCode.Alpha9)) {
			SelectElement3();
		}

		if (Input.GetKeyDown (KeyCode.Alpha0)) {
			SelectElement4();
		}

		
	


	
		int horizontal = 0;
		int vertical = 0;

		/*if (Input.GetMouseButtonDown (0)) {
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			horizontal = (int)mousePos.x;
			vertical = (int)mousePos.y;
			//horizontal = (int)mousePos.x > 0 ? 1 : -1;
			//vertical = (int)mousePos.y > 0 ? 1 : -1;
			Debug.Log (horizontal + "  " + vertical);
		}
*/
		//#if UNITY_STANDALONE || UNITY_WEBPLAYER

		if (Input.anyKeyDown && gameManager.levelFailed == false) {
			horizontal = (int)(Input.GetAxisRaw ("Horizontal"));
			vertical = (int)(Input.GetAxisRaw ("Vertical"));
			oldHorizontal = horizontal;
			oldVertical = vertical;
		
			//Check if moving horizontally, if so set vertical to zero.
			if (horizontal != 0) {
				vertical = 0;
			}

			if (horizontal != 0 || vertical != 0) {
				
				AttemptMove (horizontal, vertical);
				
				
			}
		}




		if (Input.GetMouseButtonDown(0)) {
			//GetComponent<Animator>().Play("AttackLeft");

			// checkTouch(Input.mousePosition);
		}
		

			//#endif

			/*#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
			
		//http://answers.unity3d.com/questions/610440/on-touch-event-on-game-object-on-android-2d.html

		if (Input.touchCount > 0){
			if (topTouchArea.Contains(Input.touches[0].position)) {
				Debug.Log ("touch");
			

				//Store the first touch detected.
				Touch myTouch = Input.touches[0];
				
				//Check if the phase of that touch equals Began
				if (myTouch.phase == TouchPhase.Began) {
					//If so, set touchOrigin to the position of that touch
					touchOrigin = myTouch.position;
				}
				
				//If the touch phase is not Began, and instead is equal to Ended and the x of touchOrigin is greater or equal to zero:
				else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0) {
					//Set touchEnd to equal the position of this touch
					Vector2 touchEnd = myTouch.position;
					
					//Calculate the difference between the beginning and end of the touch on the x axis.
					float x = touchEnd.x - touchOrigin.x;
					
					//Calculate the difference between the beginning and end of the touch on the y axis.
					float y = touchEnd.y - touchOrigin.y;
					
					//Set touchOrigin.x to -1 so that our else if statement will evaluate false and not repeat immediately.
					touchOrigin.x = -1;
					
					//Check if the difference along the x axis is greater than the difference along the y axis.
					if (Mathf.Abs(x) > Mathf.Abs(y)) {
						//If x is greater than zero, set horizontal to 1, otherwise set it to -1
						horizontal = x > 0 ? 1 : -1;
					}
					else {
						//If y is greater than zero, set horizontal to 1, otherwise set it to -1
						vertical = y > 0 ? 1 : -1;
					}
				}


				//Check if we have a non-zero value for horizontal or vertical
				if (horizontal != 0 || vertical != 0) {
					
					AttemptMove (horizontal, vertical);
					
					
				}

			}
		}
			
			
			#endif*/

	

		//}

	}

	public void AttemptMove(int xDir, int yDir) {
		if (Time.time > actionTimer) {
			actionTimer = Time.time + actionDelay;
			RaycastHit2D hit;
			
			canMove = Move (xDir, yDir, out hit);
			
			
			if (hit.transform == null || hit.transform.tag == "Altar")
				return;

			if (hit.transform.tag == "Door" && hasKey) {
				hit.transform.GetComponent<DoorTile>().OpenDoor();
			}
			
			CreatureTile hitComponent = hit.transform.GetComponent <CreatureTile> ();
			
			if (!canMove && hitComponent != null) {


				EncounterEnemy (hitComponent, xDir, yDir);
			}
			
			
		}
	}

	public bool Move (int xDir, int yDir, out RaycastHit2D hit) {
		//Store start position to move from, based on object's current transform position.
		Vector2 start = transform.position;
		
		// Calculate end position based on the direction parameters passed in when calling Move.
		Vector2 target = start + new Vector2 (xDir, yDir);
	
		boxCollider.enabled = false;
		hit = Physics2D.Linecast (start, target, blockingLayer);	
		boxCollider.enabled = true;

	
		//if(hit.transform == null || hit.transform.tag == "Altar" ) {
		if(hit.transform == null || hit.transform.tag == "Altar" || hit.transform.tag == "Exit") {

			//staminaCurrent -= 1;
			//AlterStamina (player, -1);

			UpdateUI ();

			StartCoroutine (SmoothMovement (target));

			//transform.position = target;
			//transform.position = Vector3.Lerp (start, target, 1f*Time.deltaTime);
			return true;

		}

		
		//If something was hit, return false, Move was unsuccesful.
		return false;
	}
	
	




	public IEnumerator SmoothMovement (Vector3 end) {
		//Calculate the remaining distance to move based on the square magnitude of the difference between current position and end parameter. 
		//Square magnitude is used instead of magnitude because it's computationally cheaper.
		sqrRemainingDistance = (transform.position - end).sqrMagnitude;
		
		//While that distance is greater than a very small amount (Epsilon, almost zero):
		while(sqrRemainingDistance != 1) {
			//Find a new position proportionally closer to the end, based on the moveTime
			Vector3 newPosition = Vector3.MoveTowards(rigidBody.position, end, inverseMoveTime * Time.deltaTime);
			
			//Call MovePosition on attached Rigidbody2D and move it to the calculated position.
			rigidBody.MovePosition (newPosition);
			
			//Recalculate the remaining distance after moving.
			sqrRemainingDistance = (transform.position - end).sqrMagnitude;
			
			//Return and loop until sqrRemainingDistance is close enough to zero to end the function
			
			yield return null;
			
		}
		
		
	}
	


	public void EncounterEnemy (CreatureTile enemy, int xDir, int yDir) {

		if (xDir == 1) {
			GetComponent<Animator>().Play("AttackRight");

		} else if (xDir == -1) {
			GetComponent<Animator>().Play("AttackLeft");
		} else if (yDir == 1) {
			GetComponent<Animator>().Play("AttackUp");
		} else if (yDir == -1) {
			GetComponent<Animator>().Play("AttackDown");
		}

		GetComponent<SpriteRenderer> ().color = selectedElement.color;


		CreatureTile currentEnemy = enemy;

		//GetComponent<AudioSource> ().Play ();

		StartCoroutine(currentEnemy.DamageCreature (damage, selectedElementName));

		StartCoroutine(RotateElement(selectedElement, selectedElementPos));

	}



	public IEnumerator RotateElement(Element ele, int elepos) {
		yield return new WaitForSeconds (0.3f);

		GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f);

		
		int oldPos = selectedElementPos;

		string currentName = ele.name;
		string tempName = ele.name;

		Element newElement = new Element();


		while (currentName == tempName || 
		       element1.name == tempName ||
		       element2.name == tempName ||
		       element3.name == tempName ||
		       element4.name == tempName
		       ) {
			
			newElement = elements.ElementAt (Random.Range (0, elements.Count)).Value;
			tempName = newElement.name;
			
			yield return null;
		}
		
		if (elepos == 1) {
			element1 = newElement;
			element1UI_Color = elements [newElement.name].color;
		} else if (elepos == 2) {
			element2 = newElement;
			element2UI_Color = elements [newElement.name].color;
		} else if (elepos == 3) {
			element3 = newElement;
			element3UI_Color = elements [newElement.name].color;
		} else if (elepos == 4) {
			element4 = newElement;
			element4UI_Color = elements [newElement.name].color;
		}

		Element[] elem = new Element[] {element1, element2, element3, element4};

		selectedElementName = elem[oldPos-1].name;
		selectedElementPos = oldPos;


		if (oldPos == elepos) {
			switch (selectedElementPos) {
			case 1:
				SelectElement1 ();
				break;
			
			case 2:
				SelectElement2 ();
				break;
			
			case 3:
				SelectElement3 ();
				break;
			
			case 4:
				SelectElement4 ();
				break;

			}
		}
		
		characterSprite = elements [selectedElementName].character;
		
		GetComponent<Animator>().Play(selectedElementName);
		
		UpdateUI ();




	}




	public IEnumerator DamagePlayer (CreatureTile creature, int dam) {

			
			healthCurrent -= (dam - defenseBonus);

			yield return new WaitForSeconds (0.1f);
			GetComponent<SpriteRenderer>().color = new Color (1f, 1f, 1f, 0f);
			
			yield return new WaitForSeconds (0.1f);
			GetComponent<SpriteRenderer>().color = new Color (1f, 1f, 1f, 1f);
			
			if (healthCurrent > healthMax) {
				healthCurrent = healthMax;
			} else if (healthCurrent <= 0) {

				healthCurrent = 0;
				//gameManager.levelFailed = true;
				GetComponent<AudioSource> ().clip = deathSound;
				GetComponent<AudioSource> ().Play ();

				StartCoroutine(gameManager.EndGame (false));
				//StartCoroutine(PrintText(playerName + " died!"));
			}
	
			healthbar.sprite =  hearts[healthCurrent];

			if (healthCurrent < 3 && !isInDanger) {
				StartCoroutine(PlayerDanger ());
			}


	}

	private IEnumerator PlayerDanger () {
		isInDanger = true;
		while (healthCurrent < 3) {

			GetComponent<SpriteRenderer>().color = new Color (1f, 0f, 0f, 1f);
			yield return new WaitForSeconds (0.3f);

			GetComponent<SpriteRenderer>().color = new Color (1f, 1f, 1f, 1f);
			yield return new WaitForSeconds (0.3f);


		}
		isInDanger = false;

	}



	public IEnumerator PrintText (string text) {

		textBox.GetComponentInChildren<Text> ().text = text;

		//if (isShowingText) {
			//textWaitTime += textWaitTime;

		//} else {
			textWaitTime = 0.3f;
			textBox.GetComponentInChildren<Image>().enabled = true;
			textBox.GetComponentInChildren<Text>().enabled = true;
			isShowingText = true;


		textBox.GetComponentInChildren<Text> ().transform.localScale = new Vector3(0.1f, 0.1f, 1f);

		while (textBox.GetComponentInChildren<Text> ().transform.localScale.x < 1.0f) {
			yield return new WaitForSeconds (textWaitTime / 20);
			textBox.GetComponentInChildren<Text> ().transform.localScale += new Vector3(0.1f, 0.1f, 0f);

		}


			yield return new WaitForSeconds (textWaitTime);

			textBox.GetComponentInChildren<Image>().enabled = false;
			textBox.GetComponentInChildren<Text>().enabled = false;
			isShowingText = false;

		//}





	
	}

	private void SetupUI () {

		textBox = GameObject.Find ("TextBox");

		// element 1 (top left)
		element1UI = elementUI.transform.FindChild ("Element1").gameObject;

		element1UI_Name = element1UI.transform.FindChild ("ClassName_Level").GetComponent<Text> ();
		element1UI_Image = element1UI.transform.FindChild ("CharImage").GetComponent<Image> ();

		element1UI_Color = element1.color;
		

		// element 2 (top right)
		element2UI = elementUI.transform.FindChild ("Element2").gameObject;
		
		element2UI_Name = element2UI.transform.FindChild ("ClassName_Level").GetComponent<Text> ();
		element2UI_Image = element2UI.transform.FindChild ("CharImage").GetComponent<Image> ();

		element2UI_Color = element2.color;

		// element 3 (bottom left)
		element3UI = elementUI.transform.FindChild ("Element3").gameObject;
		
		element3UI_Name = element3UI.transform.FindChild ("ClassName_Level").GetComponent<Text> ();
		element3UI_Image = element3UI.transform.FindChild ("CharImage").GetComponent<Image> ();

		element3UI_Color = element3.color;

		// element 4 (bottom right)
		element4UI = elementUI.transform.FindChild ("Element4").gameObject;
		
		element4UI_Name = element4UI.transform.FindChild ("ClassName_Level").GetComponent<Text> ();
		element4UI_Image = element4UI.transform.FindChild ("CharImage").GetComponent<Image> ();

		element4UI_Color = element4.color;

		UpdateUI ();

	}

	public void UpdateUI () {

		GetComponent<SpriteRenderer> ().sprite = characterSprite;
		
	
		element1UI_Name.text = element1.name;

		element1UI_Image.sprite = element1.character;

	
		element1UI.GetComponent<Image> ().color = element1UI_Color;

		element2UI_Name.text = element2.name;

		element2UI_Image.sprite = element2.character;

		element2UI.GetComponent<Image> ().color = element2UI_Color;


		element3UI_Name.text = element3.name;

		element3UI_Image.sprite = element3.character;

		element3UI.GetComponent<Image> ().color = element3UI_Color;
	
		element4UI_Name.text = element4.name;

		element4UI_Image.sprite = element4.character;

		element4UI.GetComponent<Image> ().color = element4UI_Color;

	}

	public void UpdateScore (int change) {
		if (combo > 0) {
			score += (combo * change);

		} else {
			score += change;
		}
		scoreText.text = "Score: " + score;

	}

	public void UpdateCombo (bool successfulCombo) {
		if (!successfulCombo) {
			combo = 0;
		} else {

			combo += 1;
			
			if (combo == 5) {
				StartCoroutine (PrintText ("Alright! \n 5 combo"));
			} else if (combo == 10) {
				StartCoroutine(PrintText("Nice! \n x10 combo"));
				
			} else if (combo == 15) {
				StartCoroutine(PrintText("Sweet! \n x15 combo"));
				
			} else if (combo == 20) {
				StartCoroutine(PrintText("Sick! \n x20 combo"));
				
			} else if (combo == 25) {
				StartCoroutine(PrintText("Groovy! \n x25 combo"));
				
			} else if (combo == 30) {
				StartCoroutine(PrintText("Perfect! \n x30 combo"));
				
			}		
		}

		comboText.text = "x" + combo;

		if (combo > maxCombo) {
			maxCombo = combo;
		}

	}


	public void SelectElement1() {

		selectedElementPos = 1;
		selectedElement = element1;
		selectedElementName = element1.name;
		element1UI_Color = elements [selectedElementName].color;
		
		element1UI_Color = new Color (element1UI_Color.r, element1UI_Color.g, element1UI_Color.b, 1f);
		element2UI_Color = new Color (element2UI_Color.r, element2UI_Color.g, element2UI_Color.b, 0.7f);
		element3UI_Color = new Color (element3UI_Color.r, element3UI_Color.g, element3UI_Color.b, 0.7f);
		element4UI_Color = new Color (element4UI_Color.r, element4UI_Color.g, element4UI_Color.b, 0.7f);
		element1UI.GetComponent<Image> ().color = element1UI_Color;
		element2UI.GetComponent<Image> ().color = element2UI_Color;
		element3UI.GetComponent<Image> ().color = element3UI_Color;
		element4UI.GetComponent<Image> ().color = element4UI_Color;

		characterSprite = elements [selectedElementName].character;
		GetComponent<Animator>().Play(selectedElementName);

		HighlightEnemies ();
		UpdateUI ();
	}
	

	public void SelectElement2() {
		selectedElementPos = 2;
		selectedElement = element2;
		selectedElementName = element2.name;
		element2UI_Color = elements [selectedElementName].color;

		element1UI_Color = new Color (element1UI_Color.r, element1UI_Color.g, element1UI_Color.b, 0.7f);
		element2UI_Color = new Color (element2UI_Color.r, element2UI_Color.g, element2UI_Color.b, 1f);
		element3UI_Color = new Color (element3UI_Color.r, element3UI_Color.g, element3UI_Color.b, 0.7f);
		element4UI_Color = new Color (element4UI_Color.r, element4UI_Color.g, element4UI_Color.b, 0.7f);
		element1UI.GetComponent<Image> ().color = element1UI_Color;
		element2UI.GetComponent<Image> ().color = element2UI_Color;
		element3UI.GetComponent<Image> ().color = element3UI_Color;
		element4UI.GetComponent<Image> ().color = element4UI_Color;
	
		characterSprite = elements [selectedElementName].character;
		GetComponent<Animator>().Play(selectedElementName);

		HighlightEnemies ();
		UpdateUI ();

	}


	public void SelectElement3() {
		selectedElementPos = 3;
		selectedElement = element3;
		selectedElementName = element3.name;
		element3UI_Color = elements [selectedElementName].color;

		element1UI_Color = new Color (element1UI_Color.r, element1UI_Color.g, element1UI_Color.b, 0.7f);
		element2UI_Color = new Color (element2UI_Color.r, element2UI_Color.g, element2UI_Color.b, 0.7f);
		element3UI_Color = new Color (element3UI_Color.r, element3UI_Color.g, element3UI_Color.b, 1f);
		element4UI_Color = new Color (element4UI_Color.r, element4UI_Color.g, element4UI_Color.b, 0.7f);
		element1UI.GetComponent<Image> ().color = element1UI_Color;
		element2UI.GetComponent<Image> ().color = element2UI_Color;
		element3UI.GetComponent<Image> ().color = element3UI_Color;
		element4UI.GetComponent<Image> ().color = element4UI_Color;
		characterSprite = elements [selectedElementName].character;
		GetComponent<Animator>().Play(selectedElementName);

		HighlightEnemies ();
		UpdateUI ();

	}


	public void SelectElement4() {
		selectedElementPos = 4;
		selectedElement = element4;
		selectedElementName = element4.name;
		element4UI_Color = elements [selectedElementName].color;

		element1UI_Color = new Color (element1UI_Color.r, element1UI_Color.g, element1UI_Color.b, 0.7f);
		element2UI_Color = new Color (element2UI_Color.r, element2UI_Color.g, element2UI_Color.b, 0.7f);
		element3UI_Color = new Color (element3UI_Color.r, element3UI_Color.g, element3UI_Color.b, 0.7f);
		element4UI_Color = new Color (element4UI_Color.r, element4UI_Color.g, element4UI_Color.b, 1f);
		element1UI.GetComponent<Image> ().color = element1UI_Color;
		element2UI.GetComponent<Image> ().color = element2UI_Color;
		element3UI.GetComponent<Image> ().color = element3UI_Color;
		element4UI.GetComponent<Image> ().color = element4UI_Color;
		characterSprite = elements[selectedElementName].character;
		GetComponent<Animator>().Play(selectedElementName);

		HighlightEnemies ();
		UpdateUI ();

	}

	private void StrongHighlight(CreatureTile crea) {
		if (crea.isAlive) {
			crea.GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 1f);
			crea.resistanceIcon.GetComponent<SpriteRenderer> ().sprite = crea.strongIcon;
			crea.resistanceIcon.GetComponent<SpriteRenderer> ().enabled = true;
		}
	}

	private void NeutralHighlight(CreatureTile crea) {
		if (crea.isAlive) {
			crea.GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 0.5f);
			crea.resistanceIcon.GetComponent<SpriteRenderer> ().enabled = false;
		}
	}

	private void WeakHighlight(CreatureTile crea) {
		if (crea.isAlive) {
			crea.GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 1f);
			crea.resistanceIcon.GetComponent<SpriteRenderer> ().sprite = crea.weakIcon;
			crea.resistanceIcon.GetComponent<SpriteRenderer> ().enabled = true;
		}
	}

	public void HighlightEnemies() {

		GameObject[] enemyArray = GameObject.FindGameObjectsWithTag ("Enemy");
		
		foreach (GameObject ene in enemyArray) {

			CreatureTile crea = ene.GetComponent<CreatureTile> ();

			//crea.element = crea.elementList [Random.Range (0, crea.elementList.Length)];
			
		
			switch (selectedElementName) {
			
			case "Fire":
			
				switch (crea.element) {
				
				// Strong Player Attack
				case "Nature":
					StrongHighlight(crea);
					break;
				
				case "Death":
					StrongHighlight(crea);
					break;
				
				// Weak Player Attack
				case "Water":
					WeakHighlight(crea);
					break;
				
				case "Storm":
					WeakHighlight(crea);
					break;
				
				default:
					NeutralHighlight(crea);
					break;
				}

				break;
			
			
			case "Water":
			
				switch (crea.element) {
				
				// Strong Player Attack
				case "Fire":
					StrongHighlight(crea);

					break;
				
				case "Earth":
					StrongHighlight(crea);

					break;
				
				// Weak Player Attack
				case "Storm":
					WeakHighlight(crea);

					break;
				
				case "Nature":
					WeakHighlight(crea);

					break;
				
				default:
					NeutralHighlight(crea);
					break;
				}
				break;
			
			
			
			
			
			case "Earth":
			
				switch (crea.element) {
				
				// Strong Player Attack
				case "Storm":
					StrongHighlight(crea);

					break;
				
				case "Arcane":
					StrongHighlight(crea);

					break;
				
				// Weak Player Attack
				case "Water":
					WeakHighlight(crea);
					break;
				
				case "Holy":
					WeakHighlight(crea);
					break;
				
				default:
					NeutralHighlight(crea);
					break;
				}
				break;
			
			case "Storm":
			
				switch (crea.element) {
				
				// Strong Player Attack
				case "Fire":
					StrongHighlight(crea);
					break;
				
				case "Water":
					StrongHighlight(crea);
					break;
				
				// Weak Player Attack
				case "Earth":
					WeakHighlight(crea);
					break;
				
				case "Arcane":
					WeakHighlight(crea);
					break;
				
				default:
					NeutralHighlight(crea);
					break;
				}
				break;
			
			case "Holy":
			
				switch (crea.element) {
				
				// Strong Player Attack
				case "Death":
					StrongHighlight(crea);
					break;
				
				case "Earth":
					StrongHighlight(crea);
					break;
				
				// Weak Player Attack
				case "Nature":
					WeakHighlight(crea);
					break;
				
				case "Arcane":
					WeakHighlight(crea);
					break;
				
				default:
					NeutralHighlight(crea);
					break;
				}
				break;
			
			case "Nature":
			
				switch (crea.element) {
				
				// Strong Player Attack
				case "Water":
					StrongHighlight(crea);
					break;
				
				case "Holy":
					StrongHighlight(crea);
					break;
				
				// Weak Player Attack
				case "Fire":
					WeakHighlight(crea);
					break;
				
				case "Death":
					WeakHighlight(crea);
					break;
				
				default:
					NeutralHighlight(crea);
					break;
				}
				break;
			
			case "Arcane":
			
				switch (crea.element) {
				
				// Strong Player Attack
				case "Holy":
					StrongHighlight(crea);
					break;
				
				case "Storm":
					StrongHighlight(crea);
					break;
				
				// Weak Player Attack
				case "Earth":
					WeakHighlight(crea);
					break;
				
				case "Death":
					WeakHighlight(crea);
					break;
				
				default:
					NeutralHighlight(crea);
					break;
				}

				break;
			
			case "Death":
			
				switch (crea.element) {
				
				// Strong Player Attack
				case "Nature":
					StrongHighlight(crea);
					break;
				
				case "Arcane":
					StrongHighlight(crea);
					break;
				
				// Weak Player Attack
				case "Fire":
					WeakHighlight(crea);
					break;
				
				case "Holy":
					WeakHighlight(crea);
					break;
				
				default:
					NeutralHighlight(crea);
					break;
				}
				break;
			
			
			default:
				break;
			}
		}
		
	}





}





