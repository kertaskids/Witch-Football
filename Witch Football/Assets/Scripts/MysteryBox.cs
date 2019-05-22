using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryBox : MonoBehaviour
{
    // Taking the enum from magic skill: type and affect to
    public MagicSkill.AffectTo affectTo;
    public MagicSkill.Type type;

    public MysteryBox(){
        Init();
    }
    public void Init(){
        affectTo    = MagicSkill.AffectTo.Self;
        type        = MagicSkill.Type.HealthPoint;
    }

    public void Start(){
        //Init();
    }

    public void UseEffect(){
        // <edit later based on the affectTo>
        WitchController witch = GameObject.Find("WitchPlayer").GetComponent<WitchController>();
        WitchController[] witches = new WitchController[] {witch};
        
        if(witches != null && witches.Length > 0){
            if(type == MagicSkill.Type.HealthPoint) {
                foreach (WitchController w in witches)
                {
                    w.character.healthPoint.current = 10f;
                }
            }
            //else if type != HP
        }
    }
}
