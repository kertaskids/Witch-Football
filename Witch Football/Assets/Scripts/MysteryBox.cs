using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MysteryBox : MonoBehaviour
{
    public MagicSkill.AffectTo affectTo;
    public MagicSkill.Type type;
    public StatModifier statModifier;
    public float duration;
    public bool casted;

    public void Init(MagicSkill.Type type, MagicSkill.AffectTo affectTo, StatModifier modifier, float duration){
        this.affectTo    = affectTo;
        this.type        = type;
        this.statModifier = modifier;
        this.duration   = duration;
        this.casted     = false;
    }

    public void Start(){
        // <Edit later> assign the static variables here from the default class
    }

    public void UseEffect(WitchController casterWitch){
        // Every affecto need to check the null first
        // <Edit later>Assign all witches and team here is better

        // Need to check if the player's box is null or not, and if its casting still or not
        if(affectTo == MagicSkill.AffectTo.Self){
            if(casterWitch.character.usedMysteryBox == null) {
                if(!casterWitch.character.usedMysteryBox.casted){ // No need to check if its casted
                    casterWitch.character.CastMysteryBox(this);
                    Debug.Log("Casting MysteryBox" + this.gameObject.name + " on " + "" + casterWitch.gameObject.name);
                }
            } else {
                Debug.Log("Casting MysteryBox Failed.");
            }
        } else if (affectTo == MagicSkill.AffectTo.OneTeamMate){
            // Randomly cast on one team mate, if not casting, the apply
            if(casterWitch.teamMates.Length >= 1){
                int r = Random.Range(0, casterWitch.teamMates.Length - 1);
                GameObject teamMate = casterWitch.teamMates[r];
                if(teamMate.GetComponent<WitchController>().character.usedMysteryBox == null){ // casted?
                    teamMate.GetComponent<WitchController>().character.CastMysteryBox(this);
                    Debug.Log("Casting MysteryBox" + this.gameObject.name + " on " + "" + teamMate.name);
                } else {
                    Debug.Log("Casting MysteryBox Failed.");
                }
            } else {
                Debug.Log("Casting MysteryBox Failed. Has no team mates.");
            }
        } else if (affectTo == MagicSkill.AffectTo.AllTeamMates){
            
        } else if (affectTo == MagicSkill.AffectTo.Team){
            GameObject[] allWitches = GameObject.FindGameObjectsWithTag("Witch");
            foreach (GameObject w in allWitches)
            {
                if(w.GetComponent<WitchController>().teamParty == casterWitch.teamParty){
                    if(w.GetComponent<WitchController>().character.usedMysteryBox == null){ //casted?
                        w.GetComponent<WitchController>().character.CastMysteryBox(this);
                        Debug.Log("Casting MysteryBox" + this.gameObject.name + " on " + "" + w.name);
                    } else {
                        Debug.Log("Casting MysteryBox Failed.");
                    }
                }
            }
        } else if (affectTo == MagicSkill.AffectTo.OneOpponent){
            GameObject[] allWitches = GameObject.FindGameObjectsWithTag("Witch");
            List<GameObject> allOpponents = new List<GameObject>();
            foreach (GameObject w in allWitches)
            {
                if(w.GetComponent<WitchController>().teamParty != casterWitch.teamParty){
                    allOpponents.Add(w);
                }    
            }
            if(allOpponents.Count >= 1){
                int r = Random.Range(0, allOpponents.Count - 1);
                if(allOpponents[r].GetComponent<WitchController>().character.usedMysteryBox == null){ //casted?
                    allOpponents[r].GetComponent<WitchController>().character.CastMysteryBox(this);
                    Debug.Log("Casting MysteryBox" + this.gameObject.name + " on " + "" + allOpponents[r].name);
                } else {
                    Debug.Log("Casting MysteryBox Failed.");
                }
            } else {
                Debug.Log("Casting MysteryBox Failed. Has no team mates.");
            }
        } else if (affectTo == MagicSkill.AffectTo.AllOpponents){
            
        } else if (affectTo == MagicSkill.AffectTo.AllOtherCharacters){
            GameObject[] allWitches = GameObject.FindGameObjectsWithTag("Witch");
            List<GameObject> allOpponents = new List<GameObject>();
            foreach (GameObject w in allWitches)
            {
                if(w.GetComponent<WitchController>().teamParty != casterWitch.teamParty){
                    allOpponents.Add(w);
                }    
            }
            if(allOpponents.Count >= 1){
                foreach (GameObject w in allOpponents)
                {
                    if(w.GetComponent<WitchController>().teamParty != casterWitch.teamParty){
                        if(w.GetComponent<WitchController>().character.usedMysteryBox == null){ //casted?
                            w.GetComponent<WitchController>().character.CastMysteryBox(this);
                            Debug.Log("Casting MysteryBox" + this.gameObject.name + " on " + "" + w.name);
                        } else {
                            Debug.Log("Casting MysteryBox Failed.");
                        }
                    }
                }
            } else {
                Debug.Log("Casting MysteryBox Failed. Has no team mates.");
            }
        }
    }
}

// <Edit later> Delete this or Change Mysterybox class into non monobehaviour class
/* public static class MysteryBoxDefault{
    
    private static MagicSkill.Type type; 
    private static MagicSkill.AffectTo affectTo;
    private static float duration;
    static StatModifier statModifier;
    
    public static MysteryBoxDefault HealthPotion{
        get {
            type = MagicSkill.Type.HealthPoint;
            affectTo = MagicSkill.AffectTo.Self;
            duration = 5f;
            statModifier = new StatModifier(0f, 5f, 0f, 0f);
            return;
        }
    }

    public static MysteryBox HealthPotion {
        get {
            MysteryBox mysteryBox = new MysteryBox();
            StatModifier statModifier = new StatModifier(0f, 5f, 0f, 0f);
            mysteryBox.Init(MagicSkill.Type.HealthPoint, MagicSkill.AffectTo.Self, statModifier, 5f);
            return mysteryBox;
        }
    }
    public static MysteryBox DamagePotion {
        get {
            MysteryBox mysteryBox = new MysteryBox();
            StatModifier statModifier = new StatModifier(3f, 0f, 0f, 0f);
            mysteryBox.Init(MagicSkill.Type.HealthPoint, MagicSkill.AffectTo.Self, statModifier, 5f);
            return mysteryBox;
        }
    }

}*/
