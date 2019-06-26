using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launch : MonoBehaviour {
    public GameObject ball;
    public float time = 0.5f;
    public float repeatTime = 0.5f;
    public bool isAttack = false;

    void Start()
    {

        
    }

    void Spawn()
    {
        GameObject obj = Instantiate(ball, transform.position, Quaternion.identity);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (isAttack == true)
        {
            if (IsInvoking("Spawn") == false)
                InvokeRepeating("Spawn", time, repeatTime);
        }
        else
        {
            if (IsInvoking("Spawn") == true)
                CancelInvoke("Spawn");
        }
    }
}
