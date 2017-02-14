using UnityEngine;
using System.Collections;




public class CreatureTile : MonoBehaviour {

	private SpriteRenderer spriteRenderer;
	private BoxCollider2D boxCollider;
	private Player player;
	private CreatureManager creatureManager;
	//private EquipmentManager equipmentManager;
	// state (dead/isAlive), health, damage, dam type, creature type, name, XP val, lootdrop, specials

	public string creatureName;
	public string type;

	public string element;
	
	public bool isFueled;
	
	public int health;
	//public int healthMax;
	public bool isAlive;

	public int damage;

	public int scoreValue;
	public string[] lootTable;

	public AudioClip deathSound;

	//public Sprite dieSprite;

	//private TextMesh damageNumber;

	public SpriteRenderer resistanceIcon;
	public Sprite strongIcon;
	public Sprite weakIcon;
	
	private SpriteRenderer heart;
	private TextMesh heartText;



	public string[] elementList = new string[] {	"Fire", "Water", "Earth", "Storm", 
													"Holy", "Nature", "Arcane", "Death"};

	void Start () {

		spriteRenderer = GetComponent<SpriteRenderer> ();

		boxCollider = GetComponent<BoxCollider2D> ();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
		creatureManager = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<CreatureManager> ();

		resistanceIcon = transform.FindChild ("damageIcon").GetComponent<SpriteRenderer> ();


	// assigns values to CreatureTile
		this.name = creatureManager.creatureList[Random.Range (0, creatureManager.creatureList.Length)];
		creatureName = this.name;

		gameObject.GetComponent<Animator> ().runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(Resources.Load (creatureName));
		health = creatureManager.bestiary[creatureName].health;
		heart = transform.FindChild ("heart").GetComponent<SpriteRenderer> ();
		heartText = transform.FindChild ("heartText").GetComponent<TextMesh> ();
		heartText.text = "" + health;


		damage = creatureManager.bestiary[creatureName].damage;
		scoreValue = creatureManager.bestiary[creatureName].scoreValue;
		lootTable = creatureManager.bestiary[creatureName].lootTable;

		element = elementList[Random.Range (0, elementList.Length)];

		deathSound = creatureManager.bestiary [creatureName].deathSound;

		SelectElement (element);
		isAlive = true;

	}

	private void StrongDamage () {
		player.UpdateCombo (true);
		health -= (2 + player.attackBonus);
		player.GetComponent<AudioSource> ().clip = player.attackStrongSound;
		player.GetComponent<AudioSource> ().Play ();

	}
	
	private void WeakDamage () {
		StartCoroutine(player.DamagePlayer(this, 2));
		player.UpdateCombo (false);
		//isFueled = true;

		player.GetComponent<AudioSource> ().clip = player.attackWeakSound;
		player.GetComponent<AudioSource> ().Play ();
	}

	private void NeutralDamage () {
		health -= (1 + player.attackBonus);
		player.UpdateCombo (false);
		StartCoroutine(player.DamagePlayer(this, 1));
		player.GetComponent<AudioSource> ().clip = player.attackNeutralSound;
		player.GetComponent<AudioSource> ().Play ();
	}

	// Compares player element to creature element and determines Strong or Weak damage and element change
	private void ElementalDamage(string creatureElement, string playerElement) {
		switch (playerElement) {

		case "Fire":

			switch (creatureElement) {

		// Strong Player Attack
			case "Nature":
				StrongDamage ();
				element = "Fire";


				break;

			case "Death":
				StrongDamage ();
				element = "Fire";

				break;

		// Weak Player Attack
			case "Water":
				WeakDamage ();
				break;
			
			case "Storm":
				WeakDamage ();
				break;

			default:
				NeutralDamage ();
				break;
			}

			SelectElement(element);

			break;


		case "Water":
			
			switch (creatureElement) {
				
				// Strong Player Attack
			case "Fire":
				StrongDamage ();
				element = "Water";
				break;
				
			case "Earth":
				StrongDamage ();
				element = "Water";
				break;
				
				// Weak Player Attack
			case "Storm":
				WeakDamage ();
				break;
				
			case "Nature":
				WeakDamage ();
				break;

			default:
				NeutralDamage ();
				break;
			}
			SelectElement(element);
			break;





		case "Earth":
			
			switch (creatureElement) {
				
				// Strong Player Attack
			case "Storm":
				StrongDamage ();
				element = "Earth";
				break;
				
			case "Arcane":
				StrongDamage ();
				element = "Earth";
				break;
				
				// Weak Player Attack
			case "Water":
				WeakDamage ();
				break;
				
			case "Holy":
				WeakDamage ();
				break;

			default:
				NeutralDamage ();
				break;
			}
			SelectElement(element);
			break;

		case "Storm":
			
			switch (creatureElement) {
				
				// Strong Player Attack
			case "Fire":
				StrongDamage ();
				element = "Storm";
				break;
				
			case "Water":
				StrongDamage ();
				element = "Storm";
				break;
				
				// Weak Player Attack
			case "Earth":
				WeakDamage ();
				break;
				
			case "Arcane":
				WeakDamage ();
				break;

			default:
				NeutralDamage ();
				break;
			}
			SelectElement(element);
			break;

		case "Holy":
			
			switch (creatureElement) {
				
				// Strong Player Attack
			case "Death":
				StrongDamage ();
				element = "Holy";
				break;
				
			case "Earth":
				StrongDamage ();
				element = "Holy";
				break;
				
				// Weak Player Attack
			case "Nature":
				WeakDamage ();
				break;
				
			case "Arcane":
				WeakDamage ();
				break;

			default:
				NeutralDamage ();
				break;
			}
			SelectElement(element);
			break;

		case "Nature":
			
			switch (creatureElement) {
				
				// Strong Player Attack
			case "Water":
				StrongDamage ();
				element = "Nature";
				break;
				
			case "Holy":
				StrongDamage ();
				element = "Nature";
				break;
				
				// Weak Player Attack
			case "Fire":
				WeakDamage ();
				break;
				
			case "Death":
				WeakDamage ();
				break;

			default:
				NeutralDamage ();
				break;
			}
			SelectElement(element);
			break;

		case "Arcane":
			
			switch (creatureElement) {
				
				// Strong Player Attack
			case "Holy":
				StrongDamage ();
				element = "Arcane";
				break;
				
			case "Storm":
				StrongDamage ();
				element = "Arcane";
				break;
				
				// Weak Player Attack
			case "Earth":
				WeakDamage ();
				break;
				
			case "Death":
				WeakDamage ();
				break;

			default:
				NeutralDamage ();
				break;
			}
			SelectElement(element);
			break;

		case "Death":
			
			switch (creatureElement) {
				
				// Strong Player Attack
			case "Nature":
				StrongDamage ();
				element = "Death";
				break;
				
			case "Arcane":
				StrongDamage ();
				element = "Death";
				break;
				
				// Weak Player Attack
			case "Fire":
				WeakDamage ();
				break;
				
			case "Holy":
				WeakDamage ();
				break;

			default:
				NeutralDamage ();
				break;
			}
			SelectElement(element);
			break;


		default:
			SelectElement(element);
			break;
		}
	}
	

		
		
	public IEnumerator DamageCreature (int dam, string elementplayer) {	//class tar

		heart.enabled = true;
		heartText.gameObject.GetComponent<MeshRenderer> ().enabled = true;
		
		ElementalDamage (element, elementplayer);
		heartText.text = "" + health;

		yield return new WaitForSeconds (0.1f);
		spriteRenderer.color = new Color (1f, 1f, 1f, 0f);

		yield return new WaitForSeconds (0.1f);
		spriteRenderer.color = new Color (1f, 1f, 1f, 1f);


		if (health <= 0) {
			isAlive = false;
			// Show Death sprite/animation
			GetComponent<Animator>().Stop();
			spriteRenderer.sprite = creatureManager.bestiary[creatureName].deathSprite;
			//spriteRenderer.color = new Color (1f, 1f, 1f, 0.5f);

			heart.enabled = false;
			heartText.gameObject.GetComponent<MeshRenderer> ().enabled = false;
			resistanceIcon.GetComponent<SpriteRenderer>().enabled = false;
			GetComponent<AudioSource> ().clip = deathSound;
			GetComponent<AudioSource> ().Play ();


			// Remove Collider so player can move here
			boxCollider.enabled = false;


			// Move player to this spot
			//player.AttemptMove(player.oldHorizontal, player.oldVertical);

			player.UpdateScore(scoreValue);

			yield return new WaitForSeconds (0.5f);

			spriteRenderer.color = new Color (1f, 1f, 1f, 0f);
			yield return new WaitForSeconds (0.1f);
			spriteRenderer.color = new Color (1f, 1f, 1f, 1f);
			yield return new WaitForSeconds (0.1f);
			spriteRenderer.color = new Color (1f, 1f, 1f, 0f);
			yield return new WaitForSeconds (0.1f);
			spriteRenderer.color = new Color (1f, 1f, 1f, 1f);
			yield return new WaitForSeconds (0.1f);
			spriteRenderer.enabled = false;

		}

	}

	public IEnumerator ShowEnemyDamage (int dam, string type) {

		//damageNumber.text = "-" + dam;


		//damageIcon.enabled = true;
		yield return new WaitForSeconds(0.3f);
		if (isAlive) {
			//spriteRenderer.sprite = currentSprite;
		}
		//damageIcon.enabled = false;
		//damageNumber.text = "";

	}



	public void SelectElement(string ele) {
	

		GetComponent<Animator>().Play(ele);

	}




}
