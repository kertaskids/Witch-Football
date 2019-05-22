using UnityEngine;

public class MagicSkill
{
    public enum AffectTo{
        Self,
        OneTeamMate,
        AllTeamMates,
        Team,
        OneOpponent,
        AllOpponents, 
        AllOtherCharacters,
        TilesSelected,
        TIlesSurround 
    }
    public AffectTo affectTo;
    public enum TimeUse{
        Possessing, 
        NotPossessing,
        Both
    }
    public TimeUse timeUse;
    public enum Category{
        Light, 
        Heavy,
        None
    }
    public Category category;
    public enum Type{
        Damage,
        HealthPoint,
        Power,
        Speed, 
        None    
    };
    public Type type;

    public string name;
    public CharacterStat delay;
    public CharacterStat duration;
    //<Edit later> make a new modifier class and remove these
    public float mannaNeed;
    public float modifierDamage;
    public float modifierHP;
    public float modifierPower;
    public float modifierSpeed;
    public bool magicCasted;
    
    public MagicSkill(){
        affectTo    = AffectTo.Self;
        timeUse     = TimeUse.Both; 
        category    = Category.None;
        type        = Type.None; 
        name        = "Undefined";
        delay       = new CharacterStat(0f, 0f);
        duration    = new CharacterStat(0f, 0f);
        mannaNeed   = 0f;
        modifierDamage  = 0f;
        modifierHP      = 0f; 
        modifierPower   = 0f;
        modifierSpeed   = 0f;

    }

    public void SetSkill(Category category, Type type, string name, TimeUse timeUse, AffectTo affecTo, CharacterStat delay, CharacterStat duration, float mannaNeed, 
                            float modifierDamage, float modifierHP, float modifierPower, float modifierSpeed){
        this.category   = category;
        this.type       = type;
        this.name       = name;
        this.timeUse    = timeUse;
        this.affectTo   = affecTo;
        this.delay      = new CharacterStat(delay.current, delay.max);
        this.duration   = new CharacterStat(duration.current, duration.max);
        this.mannaNeed  = mannaNeed;
        this.modifierDamage = modifierDamage;
        this.modifierHP     = modifierHP; 
        this.modifierPower  = modifierPower;
        this.modifierSpeed  = modifierSpeed;
    }

}

public static class DefaultMagicSkill {
    public static MagicSkill DefaultSkill(MagicSkill.Category category, MagicSkill.Type type, string name, 
                                            MagicSkill.TimeUse timeUse, MagicSkill.AffectTo affectTo){
        MagicSkill magicSkill = new MagicSkill();
        magicSkill.category   = category;
        magicSkill.type       = type;
        magicSkill.name       = name;
        magicSkill.timeUse    = timeUse;
        magicSkill.affectTo   = affectTo;

        if(type == MagicSkill.Type.Damage){
            if(magicSkill.category == MagicSkill.Category.Light){
                magicSkill.delay            = new CharacterStat(5f, 5f);
                magicSkill.duration         = new CharacterStat(5f, 5f);
                magicSkill.mannaNeed        = 25f;
                magicSkill.modifierDamage   = 2f;
                magicSkill.modifierHP       = 0f; 
                magicSkill.modifierPower    = 0f;
                magicSkill.modifierSpeed    = 0f;
            } else if(magicSkill.category == MagicSkill.Category.Heavy){
                magicSkill.delay            = new CharacterStat(10f, 10f);
                magicSkill.duration         = new CharacterStat(10f, 10f);
                magicSkill.mannaNeed        = 100f;
                magicSkill.modifierDamage   = 10f;
                magicSkill.modifierHP       = 0f; 
                magicSkill.modifierPower    = 0f;
                magicSkill.modifierSpeed    = 0f;
            }
        } else if(type == MagicSkill.Type.HealthPoint){
            if(magicSkill.category == MagicSkill.Category.Light){
                magicSkill.delay            = new CharacterStat(5f, 5f);
                magicSkill.duration         = new CharacterStat(1f, 1f);
                magicSkill.mannaNeed        = 25f;
                magicSkill.modifierDamage   = 0f;
                magicSkill.modifierHP       = 3f; 
                magicSkill.modifierPower    = 0f;
                magicSkill.modifierSpeed    = 0f;
            } else if(magicSkill.category == MagicSkill.Category.Heavy){
                magicSkill.delay            = new CharacterStat(10f, 10f);
                magicSkill.duration         = new CharacterStat(1f, 1f);
                magicSkill.mannaNeed   = 100f;
                magicSkill.modifierDamage  = 0f;
                magicSkill.modifierHP      = 10f; 
                magicSkill.modifierPower   = 0f;
                magicSkill.modifierSpeed   = 0f;
            }
        } else if(type == MagicSkill.Type.Power){
            if(magicSkill.category == MagicSkill.Category.Light){
                magicSkill.delay            = new CharacterStat(5f, 5f);
                magicSkill.duration         = new CharacterStat(5f, 5f);
                magicSkill.mannaNeed        = 25f;
                magicSkill.modifierDamage   = 1f;
                magicSkill.modifierHP       = 0f; 
                magicSkill.modifierPower    = 3f;
                magicSkill.modifierSpeed    = 0f;
            } else if(magicSkill.category == MagicSkill.Category.Heavy){
                magicSkill.delay            = new CharacterStat(10f, 10f);
                magicSkill.duration         = new CharacterStat(5f, 5f);
                magicSkill.mannaNeed   = 100f;
                magicSkill.modifierDamage  = 3f;
                magicSkill.modifierHP      = 0f;
                magicSkill.modifierPower   = 5f;
                magicSkill.modifierSpeed   = 0f;
            }
        } else if(type == MagicSkill.Type.Speed){
            if(magicSkill.category == MagicSkill.Category.Light){
                magicSkill.delay            = new CharacterStat(5f, 5f);
                magicSkill.duration         = new CharacterStat(5f, 5f);
                magicSkill.mannaNeed        = 25f;
                magicSkill.modifierDamage   = 0f;
                magicSkill.modifierHP       = 0f; 
                magicSkill.modifierPower    = 0f;
                magicSkill.modifierSpeed    = 3f;
            } else if(magicSkill.category == MagicSkill.Category.Heavy){
                magicSkill.delay            = new CharacterStat(10f, 10f);
                magicSkill.duration         = new CharacterStat(5f, 5f);
                magicSkill.mannaNeed        = 100f;
                magicSkill.modifierDamage   = 0f;
                magicSkill.modifierHP       = 0f; 
                magicSkill.modifierPower    = 0f;
                magicSkill.modifierSpeed    = 5f;
            }
        }        
        return magicSkill;
    }
    //<Edit later> Duration != Delay
    public static void UpdateDuration(float currentDuration, float maxDuration){}
    public static void UpdateDelay(float currentDelay, float maxDelay){}
}