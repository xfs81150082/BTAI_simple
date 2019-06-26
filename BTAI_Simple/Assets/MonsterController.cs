using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BTAI;

public class MonsterController : MonoBehaviour {


    MonsterSetting Monster;
    Root AI;
    public void Awake()
    {
        Monster = GetComponent<MonsterSetting>();

        AI = BT.Root();

        AI.OpenBranch(
            BT.Selector().OpenBranch(

               BT.Sequence().OpenBranch(BT.Condition(Monster.CanAttack), BT.Call(Monster.Attack)),

               BT.Call(Monster.Idle))
              
                );
        
    }
    // Use this for initialization
    void Start ()
    {
		
	}

   
	// Update is called once per frame
	void Update ()
    {
        AI.Tick();
	}
}
