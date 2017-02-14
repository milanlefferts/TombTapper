using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;



public class DoorTile : MonoBehaviour {

	public Sprite openedSprite;

	private Player player;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();


	}
	


	public void OpenDoor () {

			GetComponent<BoxCollider2D> ().enabled = false;
			GetComponent<SpriteRenderer> ().sprite = openedSprite;
			player.hasKey = false;
			GameObject.Find ("Key").GetComponent<Image> ().enabled = false;
			GetComponent<AudioSource> ().Play ();

	}
}
