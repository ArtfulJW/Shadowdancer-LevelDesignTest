using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Vector3 = UnityEngine.Vector3;

public class EnemyAI : MonoBehaviour
{

    [SerializeField]
    private NavMeshAgent m_Agent;
    
    [SerializeField]
    private GameObject m_player;

    [SerializeField]
    private EnemyFOV m_fov;

    [Header("Searching Phase Data")]
    private Vector3 playerLastSeenLocation;
    private float playerLastSeenTime;
    private Time searchTime;


    private void Start() {
        // playerLastSeenTime = 0.0f;

    }

    void Update()
    {
        if (m_fov.seesPlayer)
            FollowPlayer();
    }

    // Handle the different states the A.I. is currently in
    private void StateHandler() {
        // Chasing
        if (m_fov.seesPlayer)
            FollowPlayer();
        // Searching
        // else if (playerLastSeenTime )

        // Patrolling

    }

    private void FollowPlayer() {
        playerLastSeenLocation = m_player.transform.position;
        SetAgentTarget(playerLastSeenLocation);
    }

    private void SetAgentTarget(Vector3 targetPosition) {
        m_Agent.SetDestination(targetPosition);
    }
}
