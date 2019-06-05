﻿using System.Collections;
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
    public CharacterStat tackledDamageToGuard; //<Delete later> Should not be passive. or just move it from the tackler.
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
        passPower       = new CharacterStat(3f, 3f);
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
    public void Initiate(CharacterStat healthPoint, CharacterStat jumpForce, CharacterStat jumpDelay, CharacterStat moveSpeed,
                        CharacterStat shootDelay, CharacterStat passDelay, CharacterStat shootPower, CharacterStat passPower, 
                        CharacterStat guard, CharacterStat tackleDelay, CharacterStat manna, CharacterStat followDelay, 
                        CharacterStat getTackledDelay, CharacterStat tackledDamageToGuard, CharacterStat tackledDamageToHealth,
                        CharacterStat stunnedDuration, MagicSkill lightMagicSkill, MagicSkill heavyMagicSkill){
                            this.healthPoint    = healthPoint;
                            this.jumpForce      = jumpForce;
                            this.jumpDelay      = jumpDelay;
                            this.moveSpeed      = moveSpeed;
                            this.shootDelay     = shootDelay;
                            this.passDelay      = passDelay;
                            this.shootPower     = shootPower;
                            this.passPower      = passPower;
                            this.guard          = guard;
                            this.tackleDelay    = tackleDelay;
                            this.manna          = manna;
                            this.followDelay        = followDelay;
                            this.getTackledDelay    = getTackledDelay;
                            this.tackledDamageToGuard   = tackledDamageToGuard;
                            this.tackledDamageToHealth  = tackledDamageToHealth;
                            this.stunnedDuration    = stunnedDuration;
                            this.lightMagicSkill    = lightMagicSkill;
                            this.heavyMagicSkill    = heavyMagicSkill;
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
        magicSkill.casted = false;
        //lightMagicSkill.duration.current = lightMagicSkill.duration.max; 
        //heavyMagicSkill.duration.current = heavyMagicSkill.duration.max;
        Debug.Log("Revert.");
        Debug.Log("HP, Damage, Power, Speed: " + healthPoint.current + ", " + tackledDamageToGuard.current + ", " + passPower.current + ", " + moveSpeed.current);
    }

    public float UpdateDurationMagic(MagicSkill magicSkill){
        return magicSkill.duration.current >= magicSkill.duration.max ? magicSkill.duration.max : (magicSkill.duration.current += 1f * Time.deltaTime);
    }    

    public void CastMysteryBox(MysteryBox mysteryBox){
        usedMysteryBox          = mysteryBox;
        usedMysteryBox.casted   = true;
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
        tackledDamageToGuard.current    -= mysteryBox.statModifier.damage;
        tackledDamageToHealth.current   -= mysteryBox.statModifier.damage;
        healthPoint.current     -= mysteryBox.statModifier.healthPoint;
        passPower.current       -= mysteryBox.statModifier.power;
        shootPower.current      -= mysteryBox.statModifier.power;
        moveSpeed.current       -= mysteryBox.statModifier.moveSpeed;
        Debug.Log("HP, Damage, Power, Speed: " + healthPoint.current + ", " + tackledDamageToGuard.current + ", " + passPower.current + ", " + moveSpeed.current);
        Debug.Log("Reverting Mystery Box");

        usedMysteryBox.casted   = false;
        GameObject mysteryBoxToDelete = usedMysteryBox.gameObject;
        usedMysteryBox          = null;
        Debug.Log((usedMysteryBox == null) + " " + (mysteryBoxToDelete == null));
        GameObject.Destroy(mysteryBoxToDelete);
    }
}
