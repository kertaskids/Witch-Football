using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PinUpController : MonoBehaviour
{
    // Index based on PlayerID
    public GameObject[] pinUpWitches; 
    public GameObject[] backgrounds;
    //private List<Sprite> pinUpSprites;

    void Start(){
        Init();
        // <Delete Later>
        //Perform(pinUpWitches[0].GetComponent<PinUp>(), GameObject.Find("Red Witch").GetComponent<WitchController>());
    }
    void Init(){
        // <Edit later> Adding PinUpImages based on the image on player
        // Might need to set up the native size of image and the position as well 
        /* pinUpSprites = new List<Sprite>();
        GameObject[] allWitches = GameObject.FindGameObjectsWithTag("Witch");
        foreach (GameObject w in allWitches) {
            pinUpSprites.Add(w.GetComponent<WitchController>().pinUpSprite);
        }
        for(int i = 0; i < pinUpWitches.Length; i++){
            // <Edit later> must get the sprite based on the name, not on the index
            pinUpWitches[i].GetComponent<Image>().sprite = pinUpSprites[i];
            Debug.Log("Alpha "+ pinUpWitches[i].GetComponent<Image>().color.a);
                
            // Size and position here 
        }*/
        
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
                // <Edit later> Might need to nulify it again in future
                pinUp.GetComponent<Image>().sprite = witchController.pinUpSprite;
                matchPinUp = pinUp;
                return matchPinUp;
            }
        }
        return matchPinUp;
    }
}
