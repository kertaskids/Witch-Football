using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public float lifeTime;
    public float damageGuard;
    public float damageHealth;

    void Update(){
        if(lifeTime <= 0f){
            Destroy(gameObject);
        }
        lifeTime -= Time.deltaTime;
    }
}
