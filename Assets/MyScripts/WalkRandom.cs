using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkRandom : MonoBehaviour
{
     public float speed;
     public float maxTransform;
     public float minTransform;
     public int direction = 1;
     public SpriteRenderer sprite;

    void Start(){
        ChooseDirection();
        StartCoroutine("ChangeDirection");
        
    }

    void Update()
     {
         transform.Translate(speed * direction * Time.deltaTime, 0,  0);
         checkDirection();
     }

    IEnumerator ChangeDirection() {
    {        
        Debug.Log("direction "+direction);
        yield return new WaitForSeconds(3f);
        ChooseDirection();
    }
    }
 
     public void checkDirection()
     {
         if (transform.position.x >= maxTransform)
         {
             direction = -1;
         }
         else if (transform.position.x <= minTransform)
         {
             direction = 1;
         }
        if(direction==1){
            sprite.flipX=true;
        }else{
            sprite.flipX=false;
        }
     }

     void ChooseDirection(){
        int result = Random.Range(-1,4);
        if(result<=1){
            direction=-1;
        }else{
            direction=1;
        }
     }
}
