using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreController : MonoBehaviour {
	private DataController dataController;

	private GameObject bp1;
	private GameObject bp2;
	private GameObject bp3;
	private GameObject bp4;


	void Start () {

		dataController = GameObject.FindGameObjectWithTag ("DataController").GetComponent<DataController> ();

		bp1 = GameObject.Find ("BellowingPlainsI");
		bp2 = GameObject.Find ("BellowingPlainsII");
		bp3 = GameObject.Find ("BellowingPlainsIII");
		bp4 = GameObject.Find ("BellowingPlainsIV");



		bp1.transform.FindChild ("HighScore").GetComponent<Text> ().text = "" + dataController.hightScore_bp_1;
		bp2.transform.FindChild ("HighScore").GetComponent<Text> ().text = "" + dataController.hightScore_bp_2;
		bp3.transform.FindChild ("HighScore").GetComponent<Text> ().text = "" + dataController.hightScore_bp_3;
		bp4.transform.FindChild ("HighScore").GetComponent<Text> ().text = "" + dataController.hightScore_bp_4;

		bp1.SetActive (true);
		bp2.SetActive (dataController.levelCompletedArray[0]);
		bp3.SetActive (dataController.levelCompletedArray[1]);
		bp4.SetActive (dataController.levelCompletedArray[2]);



	}

}
