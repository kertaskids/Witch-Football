using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public GameObject WitchHUD;
    public GameObject LightMagicHUD;
    public GameObject HeavyMagicHUD;
    public GameObject HealthBar;
    public GameObject MannaBar;

    public WitchController witchController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void LateUpdate(){
        // Update Stun duration, Bar, Buff Status 
        //if(witch.witch.character.stunnedDuration.available){
        //}
        SetStunWitchHUD(witchController.witch.character.stunnedDuration.current/ witchController.witch.character.stunnedDuration.max);
        SetHPBar(witchController.witch.character.healthPoint.current/ witchController.witch.character.healthPoint.max);
        SetMannaBar(witchController.witch.character.manna.current/ witchController.witch.character.manna.max);
        SetLightMagicHUD(witchController.witch.character.lightMagicSkill.delay.current/ witchController.witch.character.lightMagicSkill.delay.max);
        SetHeavyMagicHUD(witchController.witch.character.heavyMagicSkill.delay.current/ witchController.witch.character.heavyMagicSkill.delay.max);
    }

    // Method: Stun, Bar, Cast skill
    void SetStunWitchHUD(float stunValue){
        GameObject stunHUD = WitchHUD.transform.Find("StunHUD").gameObject;
        stunHUD.GetComponent<Image>().fillAmount = stunValue;
    }
    void SetHPBar(float value){
        HealthBar.GetComponent<Image>().fillAmount = value;
    }
    void SetMannaBar(float value){
        MannaBar.GetComponent<Image>().fillAmount = value;
    }
    void SetLightMagicHUD(float delayValue){
        GameObject delayMagicHUD = LightMagicHUD.transform.Find("LightMagic DelayHUD").gameObject;
        delayMagicHUD.GetComponent<Image>().fillAmount = 1f - delayValue;
    }
    void SetHeavyMagicHUD(float delayValue){
        GameObject delayMagicHUD = HeavyMagicHUD.transform.Find("HeavyMagic DelayHUD").gameObject;
        delayMagicHUD.GetComponent<Image>().fillAmount = 1f - delayValue;
    }
}
