using UnityEngine;
using System;
using System.Collections;



public class SecretWallTile : MonoBehaviour {


	public Sprite unveiledSprite;


	void OnTriggerEnter2D () {
		GetComponent<BoxCollider2D> ().enabled = false;
		GetComponent<SpriteRenderer> ().sprite = unveiledSprite;
		GetComponent<AudioSource> ().Play ();
	}
}
