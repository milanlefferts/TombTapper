using UnityEngine;
using System.Collections;

public class PowerupAttack : MonoBehaviour {
	private Player player;
	public Sprite collected;
	
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
		this.name = "Attack +1";
	}
	
	
	void OnTriggerEnter2D () {
		
		
		StartCoroutine(AttackBonus ());
		GetComponent<BoxCollider2D> ().enabled = false;
		GetComponent<SpriteRenderer> ().sprite = collected;
		StartCoroutine (player.PrintText ("Attack Up!"));

	}
	
	private IEnumerator AttackBonus () {
		if (player.hasAttackBonus) {
			player.attackBonusTimer += player.attackBonusTimer;
		} else {
			player.hasAttackBonus = true;
			player.attackBonusTimer = 5f;
			player.attackBonus = 1;
			
			yield return new WaitForSeconds (player.attackBonusTimer);
			
			player.attackBonus = 0;
			player.attackBonusTimer = 0f;
			player.hasAttackBonus = false;
			Debug.Log ("end att");

		}
		
		
	}
	
	
	
	
}
