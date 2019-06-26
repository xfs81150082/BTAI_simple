using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsSight : MonoBehaviour
{
    public bool IsSeeSight = false;
    public string targetName = "Player";
    public GameObject target;

    // Use this for initialization
    private void OnTriggerEnter(Collider other)
    {
        target = other.gameObject;
        IsSeeSight = true;

        Debug.Log(other.gameObject.name);
    }

    private void OnTriggerExit(Collider other)
    {
        if (target == other.gameObject)
        {
            target = null;
            IsSeeSight = false;
        }

        Debug.Log(other.gameObject.name);
    }


    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
