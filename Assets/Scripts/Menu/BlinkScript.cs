using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BlinkScript : MonoBehaviour {

	void Start () {
		StartCoroutine (Blink ());
	}
	

	IEnumerator Blink () {
		while (true) {
			yield return new WaitForSeconds (0.5f);
			GetComponent<Text> ().color = new Color (1f, 1f, 1f, 0f);

			yield return new WaitForSeconds (0.5f);
			GetComponent<Text> ().color = new Color (1f, 1f, 1f, 1f);
		}
	}
}
