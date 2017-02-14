using UnityEngine;
using System.Collections;

public class HealthTile : MonoBehaviour {
	private Player player;
	public Sprite collected;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
		this.name = "Hearts +1";
		
	}
	
	
	void OnTriggerEnter2D () {

		player.healthCurrent += 4;

		if (player.healthCurrent > player.healthMax) {
			player.healthCurrent = player.healthMax;
		}
		player.healthbar.sprite =  player.hearts[player.healthCurrent];

		GetComponent<BoxCollider2D> ().enabled = false;
		GetComponent<SpriteRenderer> ().sprite = collected;
		StartCoroutine (player.PrintText ("+1 Hearts!"));

	}
	
	
	
	
}
