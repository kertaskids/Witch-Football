﻿using System.Collections;
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
    // Match Stats
    public enum TeamParty {
        Unassigned, 
        TeamA, 
        TeamB
    };
    public TeamParty teamParty;
    public enum TeamState {
        Offense,
        Defense 
    };
    public TeamState teamState;
    // <Edit later>
    public bool possessingBall;

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
    
    // Defense Stats
    public CharacterStat tackleDelay;
    public CharacterStat followDelay;
    public CharacterStat getTackledDelay;
    public CharacterStat tackledDamageToGuard;
    public CharacterStat tackledDamageToHealth;

    // Magic Stats
    public CharacterStat manna;
    public MagicSkill lightMagicSkill;
    public MagicSkill heavyMagicSkill;

    //<Edit later> Make a new class for stats variable, i.e, Duration{elapsed, max}, Delay{remaining, max}  or just current
    // Then assign the stats variables here
    public Character(){
        Initiate();
    }

    public void Initiate(){
        teamParty       = TeamParty.Unassigned;
        teamState       = TeamState.Defense;
        possessingBall  = false;
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
        getTackledDelay         = new CharacterStat(1f, 1f);
        tackledDamageToGuard    = new CharacterStat(1f, 1f);
        tackledDamageToHealth   = new CharacterStat(1f, 1f);
        
        // <Edit later> Delete Later
        // Duration != Delay
        lightMagicSkill = new MagicSkill();
        lightMagicSkill = DefaultMagicSkill.DefaultSkill(MagicSkill.Category.Light, MagicSkill.Type.Speed,
                                                        "Super Fast", MagicSkill.TimeUse.Both, MagicSkill.AffectTo.Self);
        //lightMagicSkill.duration.current = 5f;                                               
        heavyMagicSkill = new MagicSkill();
        heavyMagicSkill = DefaultMagicSkill.DefaultSkill(MagicSkill.Category.Heavy, MagicSkill.Type.Speed, 
                                                        "Godly Fast", MagicSkill.TimeUse.Both, MagicSkill.AffectTo.Self); 
        //heavyMagicSkill.curDuration = 5f; 
    }

    public void CastMagic(MagicSkill magicSkill){
        Debug.Log("HP, Damage, Power, Speed: " + healthPoint.current +", "+tackledDamageToGuard.current+", "+passPower.current+ ", " +moveSpeed.current);
        this.tackledDamageToGuard.current   += magicSkill.modifierDamage;
        this.tackledDamageToHealth.current  += magicSkill.modifierDamage;
        this.healthPoint.current    += magicSkill.modifierHP;   
        this.passPower.current      += magicSkill.modifierPower;
        this.shootPower.current     += magicSkill.modifierPower;
        this.moveSpeed.current      += magicSkill.modifierSpeed;
        //<Edit later> Harusnya dipindah di magicskill class, karena tiap magic skill punya current duration
        lightMagicSkill.duration.current = magicSkill.category == MagicSkill.Category.Light ? 0 :  lightMagicSkill.duration.current; 
        heavyMagicSkill.duration.current = magicSkill.category == MagicSkill.Category.Heavy ? 0 :  heavyMagicSkill.duration.current; 
        Debug.Log("Casting " + magicSkill.name + " Damage:" +magicSkill.modifierDamage + "HP:"+magicSkill.modifierHP + " Power:"+magicSkill.modifierPower +" Speed:"+magicSkill.modifierSpeed);
        Debug.Log("HP, Damage, Power, Speed: " + healthPoint.current +", "+tackledDamageToGuard.current+", "+passPower.current + ", " +moveSpeed.current); 
    }

    public void RevertMagic(MagicSkill magicSkill){
        // HP doesnt need to revert
        this.tackledDamageToGuard.current   -= magicSkill.modifierDamage;
        this.tackledDamageToHealth.current  -= magicSkill.modifierDamage;
        this.passPower.current      -= magicSkill.modifierPower;
        this.shootPower.current     -= magicSkill.modifierPower;
        this.moveSpeed.current      -= magicSkill.modifierSpeed;
        lightMagicSkill.duration.current = magicSkill.category == MagicSkill.Category.Light ? lightMagicSkill.duration.max :  lightMagicSkill.duration.current; 
        heavyMagicSkill.duration.current = magicSkill.category == MagicSkill.Category.Heavy ? heavyMagicSkill.duration.max :  heavyMagicSkill.duration.current;
        Debug.Log("Revert.");
        Debug.Log("HP, Damage, Power, Speed: " + healthPoint.current +", "+tackledDamageToGuard.current +", "+passPower.current + ", " +moveSpeed.current);
    }

    //<Edit later> Delay != duration
    public float UpdateDurationMagic(MagicSkill magicSkill){
        return magicSkill.duration.current >= magicSkill.duration.max ? magicSkill.duration.max : (magicSkill.duration.current += 1f * Time.deltaTime);
    }    
}
