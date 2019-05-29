using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public Team.TeamParty teamParty;
    public bool move;
    public float moveSpeed;
    public Transform targetTransform;

    void OnCollisionEnter(Collision other) {
        if(other.gameObject.GetComponent<Ball>() != null){
            Debug.Log("Goal! OLE!");
            Ball ball = other.gameObject.GetComponent<Ball>();
            if(ball.lastToucher != null) {
                Match matchController = GameObject.FindObjectOfType<Match>();
                matchController.GoalScored(ball.lastToucher, this);
            }
        }
    }

    void Move(Transform targetTransform, float speed){
        transform.position = Vector3.Lerp(transform.position, targetTransform.position, speed * Time.deltaTime);
    }

    void Update(){
        if(move) {
            Move(targetTransform, moveSpeed);
        }
    }
}
