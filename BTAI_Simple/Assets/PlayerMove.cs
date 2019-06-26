using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {
    public float m_fSpeed;
	// Use this for initialization
	void Start ()
    {
		
	}

    void PlayerMoving()
    {
        float height = Input.GetAxis("Horizontal");
        float Vector = Input.GetAxis("Vertical");

        height = height * m_fSpeed * Time.deltaTime;
        Vector = Vector * m_fSpeed * Time.deltaTime;

        transform.Translate(Vector3.right * height);
        transform.Translate(Vector3.forward * Vector);
    }
	
	// Update is called once per frame
	void Update ()
    {
        PlayerMoving();
	}
}
