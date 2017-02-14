using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;





public class KeyTile : MonoBehaviour {
	
	public Sprite collectedSprite;

	private Player player;
	private GameObject key;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
		key = GameObject.Find ("Key");

		
	}
	


	void OnTriggerEnter2D () {


		GetComponent<BoxCollider2D> ().enabled = false;
		GetComponent<SpriteRenderer> ().sprite = collectedSprite;

		player.hasKey = true;

		key.GetComponent<Image> ().enabled = true;
		GetComponent<AudioSource> ().Play ();

		StartCoroutine (player.PrintText ("Key Found!"));


	}
}
