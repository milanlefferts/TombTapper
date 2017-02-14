using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {
	public static MusicPlayer instance = null;

	void Awake () {
		if (instance == null) {
			instance = this;
		}
		else if (instance != this) {
			Destroy (gameObject);
		}
		
		DontDestroyOnLoad (this.gameObject);
	}
	

}
