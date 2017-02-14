using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Creature {
	
	public int creatureID;
	public string creatureName;

	public string element;
	
	public int health;
	public int damage;
	
	public int scoreValue;
	public string[] lootTable;

	public Sprite deathSprite;

	public AudioClip deathSound;

	//public AudioClip deathSound;

	
	public Creature (int id, string name, string ele, int hp, int dam,
	                 int scoreVal, string[] loot, Sprite deasp, AudioClip deas) {
		
		creatureID = id;
		creatureName = name;

		element = ele;
		
		
		health = hp;
		damage = dam;
		
		scoreValue = scoreVal;
		lootTable = loot;

		deathSprite = deasp;
		deathSound = deas;
		
	}
}


public class CreatureManager : MonoBehaviour {

	public Dictionary<string, Creature> bestiary = new Dictionary<string, Creature> ();
	public Dictionary<string, string[]> lootTables = new Dictionary<string, string[]> ();
	public Sprite[] deathSprites;
	public AudioClip[] deathSounds;

	public string[] creatureList;


	void Awake () {
		creatureList = new string[] {"Floating Skull", "Crabutante", "Dreadbear"};

		lootTables.Add ("Floating Skull", new string[] {"Fanged Helm", "Fanged Cloak"});
		lootTables.Add ("Dreadbear", new string[] {"Bearskin Rug", "Beartooth Dagger"});
		lootTables.Add ("Crabutante", new string[] {"Bearskin Rug", "Beartooth Dagger"});

		bestiary.Add ("Floating Skull", new Creature (0,
		                                           "Floating Skull",
		                                           "Fire",
		                                           2,
		                                           1,
		                                           10,
		                                           lootTables ["Floating Skull"],
		                                           deathSprites [0],
		                                           deathSounds[0]
		                                           ));

		bestiary.Add ("Dreadbear", new Creature (1,
		                                              "Dreadbear",
		                                              "Nature",
		                                              6,
		                                              1,
		                                              30,
		                                              lootTables ["Dreadbear"],
		                                        	deathSprites [1],
		                                       		 deathSounds[1]
		                                              ));

		bestiary.Add ("Crabutante", new Creature (2,
		                                          "Crabutante",
		                                         "Fire",
		                                         4,
		                                         1,
		                                         20,
		                                         lootTables ["Crabutante"],
		                                         deathSprites [2],
		                                         deathSounds[2]
		                                         ));


	}


}
