using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFOV : MonoBehaviour
{

    [SerializeField]
    private float m_radius;

    [SerializeField]
    [Range(0, 360)]
    private float m_angle;

    [SerializeField]
    private LayerMask m_targetMask;
    [SerializeField]
    private LayerMask m_obstructionMask;

    public bool seesPlayer;

    public GameObject player;
    private PlayerInformation playerState;

    
    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        playerState = player.GetComponent<PlayerInformation>();
        // Debug.Log("We have the player, he is at these coordinates: " + player.transform.position);
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine() { // coroutine, might've implemented wrong, TODO: check with jeff or richard
        float delay = 0.2f;            // look into fixedupdate instead but with custom N frames
        WaitForSeconds wait = new WaitForSeconds(delay);

        while (true) {
            yield return wait;
            FieldOfViewCheck();
            // Debug.Log("do we see the player T/F? " + seesPlayer);
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, GetRadius(), m_targetMask);

        if (rangeChecks.Length != 0) {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < GetAngle() / 2) {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, m_obstructionMask))
                    seesPlayer = true;
                else
                    seesPlayer = false;

            } else
                seesPlayer = false;
        } else if (seesPlayer)
            seesPlayer = false;
    }

    
    public float GetRadius() {
        // return m_radius;
        return m_radius * playerState.GetLuminance();
    }

    public float GetAngle() {
        return m_angle;
        // return m_angle * playerState.GetLuminance();
    }

}
