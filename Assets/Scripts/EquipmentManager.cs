using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Equipment {
	public int id;
	public string equipmentName;
	public string equipmentType;
	public string text;

	public int healthModifier;
	public int staminaModifier;
	
	public int blockModifier;
	public int dodgeModifier;
	public int luckModifier;

	public int daggerModifier;
	public int fistModifier;
	public int bowModifier;

	public int maceModifier;
	public int swordModifier;
	public int axeModifier;

	public int holyModifier;
	public int darkModifier;
	public int natureModifier;

	public Equipment (int eqid, string eqname, string eqtype, string eqtext, int health, int stam, int blo, int dod, int luc, 
	                  int cru, int pie, int sla, int ble, int poi, int fir, int aqu, int sto, int dea) {
		id = eqid;
		equipmentName = eqname;
		equipmentType = eqtype;
		text = eqtext;
		
		healthModifier = health;
		staminaModifier = stam;
		
		blockModifier = blo;
		dodgeModifier = dod;
		luckModifier = luc;
		
		daggerModifier = cru;
		fistModifier = pie;
		bowModifier = sla;
		maceModifier = ble;
		swordModifier = poi;
		
		axeModifier = fir;
		holyModifier= aqu;
		darkModifier = sto;
		natureModifier = dea;
	}
	

}

public class EquipmentManager : MonoBehaviour {

	public Dictionary<string, Equipment> armory = new Dictionary<string, Equipment>();
	// Use this for initialization
	void Awake () {
		armory.Add ("", new Equipment (0,						// id
		                                             "",		// name
		                                             "",					// type
		                                             "",						// text
		                                             0,0,					//health, stamina
		                                             0, 0, 0,				//block, dodge, luck
		                                             0, 0, 0, 0, 0,			// cru, pie, sla, ble, poi
		                                             0, 0, 0, 0			// fir, aqu, sto, dea, arc
		                                             ));

		armory.Add ("Boulder's Gape", new Equipment (1,						// id
		                                            "Boulder's Gape",		// name
		                                            "head",					// type
		                                            "",						// text
		                                            0,0,					//health, stamina
		                                            0, 0, 0,				//block, dodge, luck
		                                            1, 0, 0, 0, 0,			// cru, pie, sla, ble, poi
		                                            0, 0, 0, 0			// fir, aqu, sto, dea, arc
		));

		armory.Add ("Fireheart Gown", new Equipment (2,						// id
		                                             "Fireheart Gown",		// name
		                                             "chest",					// type
		                                             "",						// text
		                                             0,0,					//health, stamina
		                                             0, 0, 0,				//block, dodge, luck
		                                             0, 0, 0, 0, 0,			// cru, pie, sla, ble, poi
		                                             5, 0, 0, 0			// fir, aqu, sto, dea, arc
		                                             ));

		armory.Add ("Swamp Stompers", new Equipment (3,						// id
		                                             "Swamp Stompers",		// name
		                                             "legs",					// type
		                                             "",						// text
		                                             0,0,					//health, stamina
		                                             0, 0, 0,				//block, dodge, luck
		                                             0, 0, 0, 0, 1,			// cru, pie, sla, ble, poi
		                                             0, 0, 0, 0			// fir, aqu, sto, dea, arc
		                                             ));

		armory.Add ("Ethereal Shroud", new Equipment (4,						// id
		                                             "Ethereal Shroud",		// name
		                                             "chest",					// type
		                                             "",						// text
		                                             0,0,					//health, stamina
		                                             0, 3, 0,				//block, dodge, luck
		                                             0, 0, 0, 0, 0,			// cru, pie, sla, ble, poi
		                                             0, 0, 0, 0			// fir, aqu, sto, dea, arc
		                                             ));

		armory.Add ("Fanged Helm", new Equipment (5,						// id
		                                          "Fanged Helm",		// name
		                                              "head",					// type
		                                              "",						// text
		                                              10,0,					//health, stamina
		                                              0, 0, 0,				//block, dodge, luck
		                                              0, 0, 0, 0, 0,			// cru, pie, sla, ble, poi
		                                              0, 0, 0, 0			// fir, aqu, sto, dea, arc
		                                              ));

		armory.Add ("Fanged Cloak", new Equipment (6,						// id
		                                              "Fanged Cloak",		// name
		                                              "chest",					// type
		                                              "",						// text
		                                              0,0,					//health, stamina
		                                              0, 0, 0,				//block, dodge, luck
		                                              0, 0, 0, 3, 0,			// cru, pie, sla, ble, poi
		                                              0, 0, 0, 0			// fir, aqu, sto, dea, arc
		                                              ));


		armory.Add ("Skincrawler Garb", new Equipment (7,						// id
		                                               "Skincrawler Garb",		// name
		                                              "chest",					// type
		                                              "",						// text
		                                              0,1,					//health, stamina
		                                              1, 0, 0,				//block, dodge, luck
		                                              0, 0, 0, 0, 0,			// cru, pie, sla, ble, poi
		                                              0, 0, 0, 1			// fir, aqu, sto, dea, arc
		                                              ));

		armory.Add ("Blindfold", new Equipment (8,						// id
		                                        "Blindfold",		// name
		                                              "head",					// type
		                                              "",						// text
		                                              0,0,					//health, stamina
		                                              -3, -3, 6,				//block, dodge, luck
		                                              0, 0, 0, 0, 0,			// cru, pie, sla, ble, poi
		                                              0, 0, 0, 0			// fir, aqu, sto, dea, arc
		                                              ));



		
		






		
		
	}



	













}
