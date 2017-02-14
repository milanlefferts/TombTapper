using UnityEngine;
using System;
using System.Collections;


public class Altar {

	public int id;
	public string altarName;
	public string text;
	public Action effect;
	//public int duration;
	public Sprite normalSprite;
	public Sprite desecratedSprite;


	public Altar (int aid, string aname, string atext, Action eff, Sprite norm, Sprite desc) {
		id = aid;
		altarName = aname;
		text = atext;
		effect = eff;
		normalSprite = norm;
		desecratedSprite = desc;

	}

}


public class AltarTile : MonoBehaviour {

	public int id;
	public string altarName;
	public string text;
	public Action effect;
	//public int duration;
	public Sprite normalSprite;
	public Sprite desecratedSprite;

	//public bool deactivated;

	private AltarManager altarManager;

	private Player player;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();

		altarManager = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<AltarManager> ();

		// Set Altar attributes

		this.name = altarManager.altarList[UnityEngine.Random.Range (0, altarManager.altarList.Length)];
		altarName = this.name;
		text = altarManager.altars [altarName].text;
		effect = altarManager.altars [altarName].effect;

	}
	


	void OnTriggerEnter2D () {
		GetComponent<BoxCollider2D> ().enabled = false;
		//deactivated = true;
		GetComponent<Animator> ().SetBool ("deactivated", true);
		GetComponent<SpriteRenderer> ().sprite = desecratedSprite;
		//player.player.staminaCurrent += 5;


		effect.Invoke ();
		StartCoroutine(player.PrintText(altarManager.altars[altarName].text));

	}
}
