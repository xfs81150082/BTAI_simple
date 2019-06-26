using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum E_MonsterState
{
    Normal,
    Attack,
   
   
}

public class MonsterSetting : MonoBehaviour
{
    MonsterController MonsterController;
    E_MonsterState MonsterState;
    Launch monsterLaunch;
    public Launch MonsterLaunch
    {
        get
        {
            if (monsterLaunch == null)
                monsterLaunch = GetComponentInChildren<Launch>();

            return monsterLaunch; 
        }
    }

    public string targetName = "Player";
    private IsSight MonsterSight; 

    public float walkSpeed = 0;
    private float moveSpeed;
   
    public GameObject target;

    public void Awake()
    {
        MonsterController = GetComponent<MonsterController>();
        MonsterSight = GetComponentInChildren<IsSight>();
    }

    public bool CanAttack()
    {
        if (MonsterSight.IsSeeSight == true)
        {
            MonsterLaunch.isAttack = true;
            return true;
        }
        else if(MonsterSight.IsSeeSight == false)
        {
            MonsterLaunch.isAttack = false;
            return false;
        }
        else
        {
            return false;
        }
    }

    public void Idle()
    {
        
            MonsterState = E_MonsterState.Normal;
            moveSpeed = walkSpeed;
            transform.position += transform.forward * moveSpeed * Time.deltaTime;

    }

    public void Attack()
    {
        
            MonsterState = E_MonsterState.Attack;
            target = GameObject.Find(targetName);
            RotateToward(target.transform);
            moveSpeed = walkSpeed;
            MonsterState = E_MonsterState.Attack;
            //Debug.Log(MonsterLaunch.isAttack);
        


    }

  
    public void RotateToward(Transform target)
    {
        Vector3 direction = target.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = rotation;
    }

    private void Start()
    {
        MonsterState = E_MonsterState.Normal;
        moveSpeed = walkSpeed;
    }

    private void Update()
    {
        
    }
}
