using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowTarget : MonoBehaviour {

    public Transform target;
    private NavMeshAgent agent;
    private Animator animator;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update() {
        agent.SetDestination(target.position);
        bool shouldMove = agent.remainingDistance > agent.stoppingDistance;
        animator.SetBool("Walk", shouldMove);
    }
}
