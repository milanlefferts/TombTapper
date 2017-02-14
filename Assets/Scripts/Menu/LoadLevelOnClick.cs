using UnityEngine;
using System.Collections;

public class LoadLevelOnClick : MonoBehaviour {


	private DataController dataController;
	void Start () {
		dataController = GameObject.FindGameObjectWithTag ("DataController").GetComponent < DataController>();

	}

	public void LoadNextLevel(string level) {

		dataController.SelectLevel (level);
	}


}
