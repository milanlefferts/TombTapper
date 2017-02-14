using UnityEngine;
using System.Collections;

public class ComboPickup : MonoBehaviour {
	private Player player;
	public Sprite collected;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
		this.name = "Combo +5!";
		
	}
	
	
	void OnTriggerEnter2D () {
		
		for (int i = 0; i < 5; i++) {
			player.UpdateCombo(true);
		}

		GetComponent<BoxCollider2D> ().enabled = false;
		GetComponent<SpriteRenderer> ().sprite = collected;
		StartCoroutine (player.PrintText ("Combo +5!"));
		
	}
	
	
	
	
}
