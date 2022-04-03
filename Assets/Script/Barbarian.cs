using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum eState
{
    Defult,
    Idle,
    Walk,
    Run,
    Attack,
    End,
    ChangeMotion
}

public enum ePattern
{
    Defult,
    OnePattern,
    TwoPattern,
    ThreePattern,
    DamagePattern
}
[System.Obsolete]
public class Barbarian : Enemy
{
    public System.Action OnFinishedPatten;

    private NavMeshAgent agent;
    public eState state;
    public ePattern pattern;
    private Warrior warrior;
    private Transform target;
    private Animator ani;

    float walkDistance = Mathf.Pow(4f, 2);
    float attackDistance = Mathf.Pow(1.5f, 2);
    float runDistance = Mathf.Pow(6f, 2);

    public bool isWalk;
    public bool isIdle;
    public bool isRun;
    public bool isAttack;
    public bool isDefult;
    public bool isReset;

    private float walkSpeed = 1;
    private float runSpeed = 2;

    private float currentTime = 0;
    private Vector3 dir;

    private int OnePattenCount =1;
    private int TwoPattenCount;
    private int ThreePattenCount;

    private void Awake()
    {
        this.enemyName = "Barbarian";
        this.ani = this.GetComponent<Animator>();
        this.agent = this.GetComponentInChildren<NavMeshAgent>();
        this.warrior = GameObject.FindObjectOfType<Warrior>();
        this.target = this.warrior.transform;
    }
    IEnumerator Start()
    {
        this.pattern = ePattern.Defult;

        yield return new WaitForSeconds(3f);

        this.pattern = ePattern.OnePattern;
      
    }
    void Update()
    {
       this.ChangePettern(this.pattern);

        if (Input.GetMouseButtonDown(0))
        {
            OnDamageFromPlayer();
        }
    }
    void ChangePettern(ePattern pattern)
    {
        switch (pattern)
        {
            case ePattern.OnePattern:
                {
                    this.OnOnePattern(this.state);
                }
                break;
            case ePattern.TwoPattern:
                {
                    Debug.Log("Two");
                }
                break;
            case ePattern.ThreePattern:
                {
                    Debug.Log("Three");
                }
                break;
            case ePattern.DamagePattern:
                {

                }
                break;
        }
    }

    void OnDamageFromPlayer()
    {
        Debug.Log("데미지받다");
        this.agent.isStopped = true;
        int randValue = Random.RandomRange(0, 2);
        if(randValue < 1)
        {
            this.ani.SetTrigger("GetHit");
        }
        else
        {
            this.ani.SetTrigger("GetHit2");
        }
        
        this.pattern = ePattern.DamagePattern;
    }

 

    private void FixedUpdate()
    {
        this.dir = this.target.transform.position - this.transform.position;
        dir.Normalize();
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.fixedDeltaTime * 2f);
    }


    void OnOnePattern(eState state)
    {
        switch (state)
        {
            case eState.Defult:
                {
                  
                }
                break;
            case eState.Idle:
                {
                   // Debug.Log("idle");
                    this.ChangeDetailState(eState.Walk, "Walk", !isIdle, isWalk = true);
                }
                break;
            case eState.Walk:
                {
                   // Debug.Log("Walk");
                    this.ChangeDetailAgent(walkSpeed, false);

                    if (isRunDistance())
                    {
                        this.ChangeDetailState(eState.Run, "Run", !isWalk, isRun = true);
                    }
                }
                break;
            case eState.ChangeMotion:
                {

                }
                break;
            case eState.Run:
                {
                   // Debug.Log("Run");
                    this.ChangeDetailAgent(runSpeed, false);
                    if (isAttackDistance())
                    {
                        this.ChangeDetailState(eState.Attack, "Attack", !isRun, isAttack = true);
                    }
               
                }
                break;
            case eState.Attack:
                {
                    //Debug.Log("Attack");
                    this.ChangeDetailAgent(0, true);
                    this.ChangeDetailState(eState.End, null, !isAttack, isReset = true);
                }
                break;
        }
    }

    void RandomPatten()
    {
        Debug.Log("reset Patten");
        int randValue = 0;//Random.RandomRange(0, 3);

        if(randValue < 1)
        {
            this.pattern = ePattern.OnePattern;
            this.state = eState.Defult;
        }
        else if(randValue < 2)
        {
            this.pattern = ePattern.TwoPattern;
            this.state = eState.Defult;

        }
        else if(randValue < 3)
        {
            this.pattern = ePattern.ThreePattern;
            this.state = eState.Defult;
        }

        isDefult = true;
    }


    void StartPattern()
    {
        Debug.Log("StartPatten");
       
        currentTime += Time.deltaTime;
        if (currentTime > 6)
        {
            currentTime = 0;

            if (isAttackDistance())
            {
                this.ChangeDetailState(eState.Attack, "Attack", !isDefult, isAttack = true);
             
            }
            else if (isWalkDistance())
            {
                this.ChangeDetailState(eState.Walk, "Walk", !isDefult, isWalk = true);
         
            }
            else if (isRunDistance())
            {
                this.ChangeDetailState(eState.Run, "Run", !isDefult, isRun = true);
            }
            else
            {
                this.state = eState.Idle;
            }
        }
    }

    void ChangeDetailState(eState state, string aniName, bool preState, bool currentState )
    {
        this.state = state;
        if (aniName != null)
        {
            this.ani.SetTrigger(aniName);
        }
    }

    void ChangeDetailAgent(float speed, bool isStop)
    {
        this.agent.SetDestination(this.target.position);
        this.agent.speed = speed;
        this.agent.isStopped = isStop;
    }

    
    bool isWalkDistance()
    {
        float toTargetDistance = (this.target.transform.position - this.transform.position).sqrMagnitude;

        return toTargetDistance < this.walkDistance;
    }
    bool isAttackDistance()
    {
        float toTargetDistance = (this.target.transform.position - this.transform.position).sqrMagnitude;

        return toTargetDistance < this.attackDistance;
    
    }

    bool isRunDistance()
    {
        float toTargetDistance = (this.target.transform.position - this.transform.position).sqrMagnitude;

        return toTargetDistance < this.runDistance;
    }
}
