using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardSprite : MonoBehaviour
{
    public bool Flipable;
    private float lastDir = 0; // 0 = left/ forward, 180 = right/ backward from the camera
    private PlayerInput playerInput;
    private bool flip;
    private SpriteRenderer spriteRenderer;

    void Start(){
        if(Flipable){
            spriteRenderer = GetComponent<SpriteRenderer>();
            playerInput = PlayerInput.GetPlayer((int) transform.parent.GetComponent<WitchController>().playerID);
        }
    }

    void LateUpdate(){
        if(Flipable){
            ChangeDirection();
        } else {
            
        }
        transform.forward = Camera.main.transform.forward;
    }

    void ChangeDirection(){
        float horizontal    = 0;
        float vertical      = 0;

        horizontal = Input.GetAxis(playerInput.HorizontalMove);
        vertical = Input.GetAxis(playerInput.VerticalMove);

        if(horizontal > 0.1){
            lastDir = 0;
            flip = false;
        }
        if(horizontal < -0.1){
            lastDir = 180;
            flip = true;
        }
        spriteRenderer.flipX = flip;

        //transform.forward = Camera.main.transform.forward;
        //transform.forward = new Vector3(Camera.main.transform.forward.x, 
          //                              lastDir, 
            //                            Camera.main.transform.forward.z);
    }
}
