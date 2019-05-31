using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStat {
    public float current;
    public float max;

    public bool full {
        get {
            return current >= max;
        }
    }
    public bool empty {
        get { 
            return current <= 0; 
        }
    }
    public bool available {
        get {
            return current > 0;
        }
    }
    public CharacterStat(float current, float max){
        this.current    = current;
        this.max        = max;
    }
    public void SetStat(float current, float max){
        this.current    = current;
        this.max        = max;
    }
}

public class Character
{
    // Basic Stats
    public CharacterStat healthPoint;
    public CharacterStat jumpForce;
    public CharacterStat jumpDelay;
    
    // Move Stats
    public CharacterStat moveSpeed;

    // Offense Stats
    public CharacterStat guard;
    public CharacterStat shootDelay;
    public CharacterStat passDelay;
    public CharacterStat shootPower;
    public CharacterStat passPower;
    public CharacterStat getTackledDelay;
    
    // Defense Stats
    public CharacterStat tackleDelay;
    public CharacterStat followDelay;

    // Magic Stats
    public CharacterStat manna;
    public MagicSkill lightMagicSkill;
    public MagicSkill heavyMagicSkill;

    // Self Damage Stats
    public CharacterStat tackledDamageToGuard; 
    public CharacterStat tackledDamageToHealth; 
    public MysteryBox usedMysteryBox;
    public CharacterStat stunnedDuration;

    public Character(){
        Initiate();
    }

    public void Initiate(){
        healthPoint     = new CharacterStat(10f, 10f);
        jumpForce       = new CharacterStat(5f, 5f); 
        jumpDelay       = new CharacterStat(5f, 5f);
        moveSpeed       = new CharacterStat(2f, 2f); 
        shootDelay      = new CharacterStat(5f, 5f);
        passDelay       = new CharacterStat(3f, 3f);
        shootPower      = new CharacterStat(5f, 5f);
        passPower       = new CharacterStat(5f, 5f);
        guard           = new CharacterStat(0f, 3f); 
        tackleDelay     = new CharacterStat(1f, 1f); 
        manna           = new CharacterStat(100f, 100f);
        followDelay     = new CharacterStat(1f, 1f);
        getTackledDelay         = new CharacterStat(5f, 5f);
        tackledDamageToGuard    = new CharacterStat(1f, 1f);
        tackledDamageToHealth   = new CharacterStat(1f, 1f);
        stunnedDuration         = new CharacterStat(0f, 10f);
        
        // <Edit later> Delete Later
        lightMagicSkill = new MagicSkill();
        lightMagicSkill = DefaultMagicSkill.DefaultSkill(MagicSkill.Category.Light, MagicSkill.Type.Speed,
                                                        "Super Fast", MagicSkill.TimeUse.Both, MagicSkill.AffectTo.Self);
        //lightMagicSkill.duration.current = 5f;                                               
        heavyMagicSkill = new MagicSkill();
        heavyMagicSkill = DefaultMagicSkill.DefaultSkill(MagicSkill.Category.Heavy, MagicSkill.Type.Speed, 
                                                        "Godly Fast", MagicSkill.TimeUse.Both, MagicSkill.AffectTo.Self); 
        //heavyMagicSkill.curDuration = 5f; 
        usedMysteryBox = null;
    }

    public void CastMagic(MagicSkill magicSkill){
        Debug.Log("HP, Damage, Power, Speed: " + healthPoint.current + ", " + tackledDamageToGuard.current + ", " + passPower.current + ", " + moveSpeed.current);
        this.tackledDamageToGuard.current   += magicSkill.statModifier.damage;
        this.tackledDamageToHealth.current  += magicSkill.statModifier.damage;
        this.healthPoint.current    += magicSkill.statModifier.healthPoint;   
        this.passPower.current      += magicSkill.statModifier.power;
        this.shootPower.current     += magicSkill.statModifier.power;
        this.moveSpeed.current      += magicSkill.statModifier.moveSpeed;
        
        lightMagicSkill.duration.current = 0f; 
        heavyMagicSkill.duration.current = 0f; 
        Debug.Log("Casting " + magicSkill.name + " Damage:" +magicSkill.statModifier.damage + "HP:"+magicSkill.statModifier.healthPoint + 
                    " Power:" + magicSkill.statModifier.power + " Speed:" + magicSkill.statModifier.moveSpeed);
        Debug.Log("HP, Damage, Power, Speed: " + healthPoint.current + ", " + tackledDamageToGuard.current + ", " + passPower.current + ", " + moveSpeed.current); 
    }

    public void RevertMagic(MagicSkill magicSkill){
        // HP doesnt need to revert
        this.tackledDamageToGuard.current   -= magicSkill.statModifier.damage;
        this.tackledDamageToHealth.current  -= magicSkill.statModifier.damage;
        this.passPower.current      -= magicSkill.statModifier.power;
        this.shootPower.current     -= magicSkill.statModifier.power;
        this.moveSpeed.current      -= magicSkill.statModifier.moveSpeed;
        magicSkill.duration.current = magicSkill.duration.max;
        magicSkill.magicCasted = false;
        //lightMagicSkill.duration.current = lightMagicSkill.duration.max; 
        //heavyMagicSkill.duration.current = heavyMagicSkill.duration.max;
        Debug.Log("Revert.");
        Debug.Log("HP, Damage, Power, Speed: " + healthPoint.current + ", " + tackledDamageToGuard.current + ", " + passPower.current + ", " + moveSpeed.current);
    }

    public float UpdateDurationMagic(MagicSkill magicSkill){
        return magicSkill.duration.current >= magicSkill.duration.max ? magicSkill.duration.max : (magicSkill.duration.current += 1f * Time.deltaTime);
    }    

    public void CastMysteryBox(MysteryBox mysteryBox){
        usedMysteryBox.casted   = true;
        usedMysteryBox          = mysteryBox;
        tackledDamageToGuard.current    += mysteryBox.statModifier.damage;
        tackledDamageToHealth.current   += mysteryBox.statModifier.damage;
        healthPoint.current     += mysteryBox.statModifier.healthPoint;
        passPower.current       += mysteryBox.statModifier.power;
        shootPower.current      += mysteryBox.statModifier.power;
        moveSpeed.current       += mysteryBox.statModifier.moveSpeed;
        Debug.Log("Using Mystery Box");
        Debug.Log("HP, Damage, Power, Speed: " + healthPoint.current + ", " + tackledDamageToGuard.current + ", " + passPower.current + ", " + moveSpeed.current); 
    }
    public void RevertMysteryBox(MysteryBox mysteryBox){
        usedMysteryBox.casted   = false;
        usedMysteryBox          = null;
        tackledDamageToGuard.current    -= mysteryBox.statModifier.damage;
        tackledDamageToHealth.current   -= mysteryBox.statModifier.damage;
        healthPoint.current     -= mysteryBox.statModifier.healthPoint;
        passPower.current       -= mysteryBox.statModifier.power;
        shootPower.current      -= mysteryBox.statModifier.power;
        moveSpeed.current       -= mysteryBox.statModifier.moveSpeed;
        Debug.Log("Reverting Mystery Box");
        Debug.Log("HP, Damage, Power, Speed: " + healthPoint.current + ", " + tackledDamageToGuard.current + ", " + passPower.current + ", " + moveSpeed.current); 
    }
}
