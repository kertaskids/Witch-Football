using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchBase {
    public enum WitchClass{
        None, 
        Sorcerer,
        Cleric, 
        Wizard,
        Druid
    }
    public enum WitchType{
        None, 
        Striker,
        Defender
    }
    public WitchClass witchClass;
    public WitchType witchType;
    public Character character; 
}

public static class WitchDefault {
    // When creating a witch, need to assign Class, Type, Character, and MagicSkills
    public static WitchBase Base{
        get {
            WitchBase witch     = new WitchBase();
            witch.witchClass    = WitchBase.WitchClass.None;
            witch.witchType     = WitchBase.WitchType.None;
            
            witch.character     = new Character();
            witch.character.healthPoint     = new CharacterStat(10f, 10f);
            witch.character.jumpForce       = new CharacterStat(5f, 5f); 
            witch.character.jumpDelay       = new CharacterStat(5f, 5f);
            witch.character.moveSpeed       = new CharacterStat(2f, 2f); 
            witch.character.shootDelay      = new CharacterStat(5f, 5f);
            witch.character.passDelay       = new CharacterStat(3f, 3f);
            witch.character.shootPower      = new CharacterStat(5f, 5f);
            witch.character.passPower       = new CharacterStat(3f, 3f);
            witch.character.guard           = new CharacterStat(0f, 3f); 
            witch.character.tackleDelay     = new CharacterStat(1f, 1f); 
            witch.character.manna           = new CharacterStat(100f, 100f);
            witch.character.followDelay     = new CharacterStat(1f, 1f);
            witch.character.getTackledDelay         = new CharacterStat(5f, 5f);
            witch.character.tackledDamageToGuard    = new CharacterStat(1f, 1f);
            witch.character.tackledDamageToHealth   = new CharacterStat(1f, 1f);
            witch.character.stunnedDuration         = new CharacterStat(0f, 10f);

            //witch.character.lightMagicSkill = new MagicSkill();
            //witch.character.lightMagicSkill = DefaultMagicSkill.DefaultSkill(MagicSkill.Category.Light, MagicSkill.Type.Speed,
            //                                                    "Super Fast", MagicSkill.TimeUse.Both, MagicSkill.AffectTo.Self);
            //witch.character.heavyMagicSkill = new MagicSkill();
            //witch.character.heavyMagicSkill = DefaultMagicSkill.DefaultSkill(MagicSkill.Category.Heavy, MagicSkill.Type.Speed, 
            //                                                    "Godly Fast", MagicSkill.TimeUse.Both, MagicSkill.AffectTo.Self);
            return witch;
        }
    }

    public static WitchBase Sorcerer{
        get {
            WitchBase witch     = new WitchBase();
            witch.witchClass    = WitchBase.WitchClass.Sorcerer;
            witch.witchType     = WitchBase.WitchType.Defender;
            
            witch.character     = new Character();
            witch.character.healthPoint     = new CharacterStat(10f, 10f);
            witch.character.jumpForce       = new CharacterStat(5f, 5f); 
            witch.character.jumpDelay       = new CharacterStat(5f, 5f);
            witch.character.moveSpeed       = new CharacterStat(2f, 2f); 
            witch.character.shootDelay      = new CharacterStat(5f, 5f);
            witch.character.passDelay       = new CharacterStat(3f, 3f);
            witch.character.shootPower      = new CharacterStat(5f, 5f);
            witch.character.passPower       = new CharacterStat(3f, 3f);
            witch.character.guard           = new CharacterStat(0f, 3f); 
            witch.character.tackleDelay     = new CharacterStat(1f, 1f); 
            witch.character.manna           = new CharacterStat(100f, 100f);
            witch.character.followDelay     = new CharacterStat(1f, 1f);
            witch.character.getTackledDelay         = new CharacterStat(5f, 5f);
            witch.character.tackledDamageToGuard    = new CharacterStat(1f, 1f);
            witch.character.tackledDamageToHealth   = new CharacterStat(1f, 1f);
            witch.character.stunnedDuration         = new CharacterStat(0f, 10f);

            //witch.character.lightMagicSkill = new MagicSkill();
            //witch.character.lightMagicSkill = DefaultMagicSkill.DefaultSkill(MagicSkill.Category.Light, MagicSkill.Type.Speed,
            //                                                    "Super Fast", MagicSkill.TimeUse.Both, MagicSkill.AffectTo.Self);
            //witch.character.heavyMagicSkill = new MagicSkill();
            //witch.character.heavyMagicSkill = DefaultMagicSkill.DefaultSkill(MagicSkill.Category.Heavy, MagicSkill.Type.Speed, 
            //                                                    "Godly Fast", MagicSkill.TimeUse.Both, MagicSkill.AffectTo.Self);
            return witch;
        }
    }
}

