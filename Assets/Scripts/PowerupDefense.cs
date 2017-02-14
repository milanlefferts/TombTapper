using UnityEngine;
using System.Collections;

public class PowerupDefense : MonoBehaviour {
	private Player player;
	public Sprite collected;

	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
		this.name = "Defense +1";
	}
	
	
	void OnTriggerEnter2D () {

		StartCoroutine(DefenseBonus ());

		GetComponent<BoxCollider2D> ().enabled = false;
		GetComponent<SpriteRenderer> ().sprite = collected;
		StartCoroutine (player.PrintText ("Defense Up!"));

	}

	private IEnumerator DefenseBonus () {
		if (player.hasDefenseBonus) {
			player.defenseBonusTimer += player.defenseBonusTimer;
		} else {
			player.hasDefenseBonus = true;
			player.defenseBonusTimer = 5f;
			player.defenseBonus = 1;

			yield return new WaitForSeconds (player.defenseBonusTimer);

			player.defenseBonus = 0;
			player.defenseBonusTimer = 0f;
			player.hasDefenseBonus = false;
		}


	}
	
	
	
	
}
