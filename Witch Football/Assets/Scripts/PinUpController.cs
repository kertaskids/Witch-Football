using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PinUpController : MonoBehaviour
{
    public GameObject[] pinUpWitches; // Index based on Player
    public GameObject[] backgrounds;

    void Start(){
        Init();
        // <Delete Later>
        //Perform(pinUpWitches[0].GetComponent<PinUp>(), GameObject.Find("Red Witch").GetComponent<WitchController>());
    }
    void Init(){
        foreach(GameObject pinUp in pinUpWitches) {
            pinUp.SetActive(false);
        }
        foreach(GameObject background in backgrounds) {
            background.SetActive(false);
        }
    }
    public void Perform(WitchController witchController, float duration){
        GameObject pinUpWitch = GetMatchedPinUp(witchController).gameObject;
        pinUpWitch.SetActive(true);
        // Not necessarry
        pinUpWitch.GetComponent<PinUp>().duration = duration; 
        pinUpWitch.GetComponent<PinUp>().Perform(witchController, pinUpWitch.GetComponent<Image>());
    }
    public void Perform(PinUp pinUpWitch, WitchController witchController){
        //GetMatchedPinUp(witchController).gameObject.SetActive(true);
        pinUpWitch.gameObject.SetActive(true);
        pinUpWitch.Perform(witchController, pinUpWitch.GetComponent<Image>());
    }
    public void SetOff(GameObject pinUp){
        // Revert the position and color
        pinUp.GetComponent<PinUp>().background.SetActive(false);
        pinUp.SetActive(false);
    }
    public GameObject GetMatchedPinUp(WitchController witchController){
        GameObject matchPinUp = null;
        foreach(GameObject pinUp in pinUpWitches) {
            if(pinUp.name.Contains(witchController.playerID.ToString())){
                matchPinUp = pinUp;
                return matchPinUp;
            }
        }
        return matchPinUp;
    }

}
