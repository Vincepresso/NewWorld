using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour {

    public Animator animator;

    public int noOfClicks = 0;
    float lastClickedTime = 0;
    float maxComboDelay = 1;

    void Update() {
        Attack();
        if(Time.time - lastClickedTime > maxComboDelay) {
            noOfClicks = 0;
        }
    }

    void Attack() {
        if(Input.GetMouseButtonDown(0)) {

            lastClickedTime = Time.time;
            noOfClicks++;
            if(noOfClicks == 1) {
                //animator.SetBool("Attack1", true);
                animator.SetTrigger("attack1");
            }
            noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);
        }
    }
}
