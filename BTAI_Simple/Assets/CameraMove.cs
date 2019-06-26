using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {
    public GameObject player;
    Vector3 m_vOffset;

	// Use this for initialization
	void Start ()
    {
        m_vOffset = transform.position - player.transform.position;
	}
	
    void CameraMoving()
    {
        transform.position = player.transform.position + m_vOffset;
    }
	// Update is called once per frame
	void Update ()
    {
        CameraMoving();
	}
}
