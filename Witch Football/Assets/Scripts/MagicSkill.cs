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
    public float mannaNeed;
    public bool magicCasted;
    public CharacterStat delay;
    public CharacterStat duration;
    public StatModifier statModifier;
    
    public MagicSkill(){
        affectTo    = AffectTo.Self;
        timeUse     = TimeUse.Both; 
        category    = Category.None;
        type        = Type.None; 
        name        = "Undefined";
        delay       = new CharacterStat(0f, 0f);
        duration    = new CharacterStat(0f, 0f);
        mannaNeed   = 0f;
        magicCasted = false;
        statModifier = new StatModifier();
    }

    public void SetSkill(Category category, Type type, string name, TimeUse timeUse, AffectTo affecTo, 
                        CharacterStat delay, CharacterStat duration, float mannaNeed, StatModifier statModifier){
        this.category   = category;
        this.type       = type;
        this.name       = name;
        this.timeUse    = timeUse;
        this.affectTo   = affecTo;
        this.mannaNeed  = mannaNeed;
        this.delay      = new CharacterStat(delay.current, delay.max);
        this.duration   = new CharacterStat(duration.current, duration.max);
        this.statModifier = new StatModifier(statModifier.damage, statModifier.healthPoint, statModifier.power, statModifier.moveSpeed);
    }
}

public static class DefaultMagicSkill {
    public static MagicSkill DefaultSkill(MagicSkill.Category category, MagicSkill.Type type, string name, 
                                            MagicSkill.TimeUse timeUse, MagicSkill.AffectTo affectTo) {
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
                magicSkill.statModifier     = new StatModifier(2f, 0f, 0f, 0f);
            } else if(magicSkill.category == MagicSkill.Category.Heavy){
                magicSkill.delay            = new CharacterStat(10f, 10f);
                magicSkill.duration         = new CharacterStat(10f, 10f);
                magicSkill.mannaNeed        = 100f;
                magicSkill.statModifier     = new StatModifier(10f, 0f, 0f, 0f);
            }
        } else if(type == MagicSkill.Type.HealthPoint){
            if(magicSkill.category == MagicSkill.Category.Light){
                magicSkill.delay            = new CharacterStat(5f, 5f);
                magicSkill.duration         = new CharacterStat(1f, 1f);
                magicSkill.mannaNeed        = 25f;
                magicSkill.statModifier     = new StatModifier(0f, 3f, 0f, 0f);
            } else if(magicSkill.category == MagicSkill.Category.Heavy){
                magicSkill.delay            = new CharacterStat(10f, 10f);
                magicSkill.duration         = new CharacterStat(1f, 1f);
                magicSkill.mannaNeed        = 100f;
                magicSkill.statModifier     = new StatModifier(0f, 10f, 0f, 0f);
            }
        } else if(type == MagicSkill.Type.Power){
            if(magicSkill.category == MagicSkill.Category.Light){
                magicSkill.delay            = new CharacterStat(5f, 5f);
                magicSkill.duration         = new CharacterStat(5f, 5f);
                magicSkill.mannaNeed        = 25f;
                magicSkill.statModifier     = new StatModifier(1f, 0f, 3f, 0f);
            } else if(magicSkill.category == MagicSkill.Category.Heavy){
                magicSkill.delay            = new CharacterStat(10f, 10f);
                magicSkill.duration         = new CharacterStat(5f, 5f);
                magicSkill.mannaNeed        = 100f;
                magicSkill.statModifier     = new StatModifier(3f, 0f, 5f, 0f);
            }
        } else if(type == MagicSkill.Type.Speed){
            if(magicSkill.category == MagicSkill.Category.Light){
                magicSkill.delay            = new CharacterStat(5f, 5f);
                magicSkill.duration         = new CharacterStat(5f, 5f);
                magicSkill.mannaNeed        = 25f;
                magicSkill.statModifier     = new StatModifier(0f, 0f, 0f, 3f);
            } else if(magicSkill.category == MagicSkill.Category.Heavy){
                magicSkill.delay            = new CharacterStat(10f, 10f);
                magicSkill.duration         = new CharacterStat(5f, 5f);
                magicSkill.mannaNeed        = 100f;
                magicSkill.statModifier     = new StatModifier(0f, 0f, 0f, 5f);
            }
        }        
        return magicSkill;
    }

    //<Edit later>
    // Make list of Skill set; static class
}