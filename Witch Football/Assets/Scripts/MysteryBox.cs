using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryBox : MonoBehaviour
{
    // Taking the enum from magic skill: type and affect to
    public MagicSkill.AffectTo affectTo;
    public MagicSkill.Type type;
    // Need stat Modifier class template here

    public MysteryBox(){
        Init();
    }
    // <Edit later>
    //public MysteryBox(MagicSkill.Type type, MagicSkill.AffectTo affectTo, MagicSkill.StatModifier modifier){}
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

public static class MysteryBoxDefault{
    private static MysteryBox _bomb;
    private static MysteryBox _mannaPotion;
    private static MysteryBox _hpPotion;
    private static MysteryBox _speedPotion;

    public static MysteryBox Bomb() {
        _bomb = new MysteryBox();
        _bomb.type = MagicSkill.Type.Damage;
        return _bomb;
    }

}
