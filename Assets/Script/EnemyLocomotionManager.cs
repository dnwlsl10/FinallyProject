using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyLocomotionManager : MonoBehaviour
{

    EnemyAnimatorManager enemyAnimatorManager;
    EnemyManager enemyManager;
    public Player currentTarget;
    public LayerMask detectionLayer;

    NavMeshAgent agent;
    Rigidbody enemyRigidbody;
    Vector3 targetDirection;

    public float distanceFromTarget;
    public float stoppingDistance = Mathf.Pow(0.5f,2);
    public float rotaionSpeed = 5f;
    private void Awake()
    {
        enemyManager = this.GetComponent<EnemyManager>();
        enemyAnimatorManager = this.GetComponent<EnemyAnimatorManager>();
        agent = this.GetComponentInChildren<NavMeshAgent>();
        enemyRigidbody = this.GetComponent<Rigidbody>();
    }
    public void HandleDetection()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);

        for(int i = 0; i<colliders.Length; i++)
        {
        Debug.Log("test");
        Player player = colliders[i].transform.GetComponentInParent<Player>();

            if(player != null)
            {
         
                this.targetDirection = player.transform.position - this.transform.position;

                float viewalbeAngle = Vector3.Angle(targetDirection, transform.forward);
             
                if (viewalbeAngle > enemyManager.minimumDetectionAngle && viewalbeAngle < enemyManager.maximumDetectionAngle)
                {
                    currentTarget = player;
                }
            }
        }
    }

    public void HandleMoveToTarget()
    {
        Vector3 targetDirection = currentTarget.transform.position - transform.position;
        distanceFromTarget = (currentTarget.transform.position - this.transform.position).sqrMagnitude;
        float viewaableAngle = Vector3.Angle(targetDirection, transform.forward);

        if (enemyManager.isPreformingAction)
        {
            enemyAnimatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            agent.enabled = false;
        }
        else
        {
            if(distanceFromTarget > stoppingDistance)
            {
                enemyAnimatorManager.anim.SetFloat("Vertical",1, 0.1f, Time.deltaTime);
            }
        }

        HandleRotateToWardsTarget();
        agent.transform.localPosition = Vector3.zero;
        agent.transform.localRotation = Quaternion.identity;
    }


    private void HandleRotateToWardsTarget()
    {
        if (enemyManager.isPreformingAction)
        {
            Vector3 direction = currentTarget.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            if(direction == Vector3.zero)
            {
                direction = transform.forward;
            }
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotaionSpeed);
        }
        else
        {
            Vector3 relativeDirection = transform.InverseTransformDirection(agent.desiredVelocity);
            Vector3 targetVelocity = enemyRigidbody.velocity;

            agent.enabled = true;
            agent.SetDestination(currentTarget.transform.position);
            enemyRigidbody.velocity = targetVelocity;
            transform.rotation = Quaternion.Slerp(transform.rotation, agent.transform.rotation, rotaionSpeed / Time.deltaTime);
        }
    }
    private void Update()
    {
        Debug.DrawLine(this.transform.position, targetDirection,  Color.yellow, 3f);
        Debug.DrawLine(this.transform.position, transform.forward, Color.yellow, 3f);

    }

    public void OnDrawGizmosSelected()
    {
    Gizmos.color = Color.yellow;
    Gizmos.DrawSphere(transform.position, 20);
    }
   
}



