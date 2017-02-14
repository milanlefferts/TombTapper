using UnityEngine;
using System.Collections;

public class ChestTile : MonoBehaviour {
	private Player player;
	public int score;
	public Sprite opened;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
		score = 100;
		this.name = "Chest: " + score;

	}
	

	void OnTriggerEnter2D () {

		//Debug.Log ("Opened Chest!");
		GetComponent<BoxCollider2D> ().enabled = false;
		GetComponent<SpriteRenderer> ().sprite = opened;
		player.UpdateScore (score);
		StartCoroutine (player.PrintText ("Found treasure! \n+" + score));
		//player.inventory.Add (content, equipmentManager.armory [content]);

		//Debug.Log ("Found " + content + "!");
	}




}
