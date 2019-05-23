using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    void OnCollisionEnter(Collision other) {
        if(other.gameObject.GetComponent<Ball>() != null){
            Debug.Log("Goal! OLE!");
        }
    }
    void OnTriggerEnter(Collider other) {
        if(other.GetComponent<Ball>() !=null){
            Debug.Log("Goal! OLALA!");
        }    
    }
    void Move(Transform targetTransform){
        gameObject.transform.position = targetTransform.position;
    }

    void Update(){
        
    }
}
