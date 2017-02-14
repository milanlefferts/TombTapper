using UnityEngine;
using System.Collections;

public class ExitTile : MonoBehaviour {
	private GameManager gameManager;
	void Start () {
		gameManager = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager>();
	}
	void OnTriggerEnter2D() {
		//GetComponent<BoxCollider2D> ().enabled = false;
		//gameManager.levelCompleted = true;
		StartCoroutine(gameManager.EndGame (true));
	}
}