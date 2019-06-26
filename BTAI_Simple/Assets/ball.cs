using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball : MonoBehaviour
{
    public float speed = 10.0f;
    public string targetName = "Player";
    public bool isFollow = false;

    GameObject target;

    Vector3 origin;
	// Use this for initialization
	void Start ()
    {
        target = GameObject.Find(targetName);
        origin = target.transform.position;
	}

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update ()
    {
     
        transform.position = Vector3.MoveTowards(transform.position, isFollow ? target.transform.position : origin , speed * Time.deltaTime);
        Destroy(gameObject,2.0f);
    }
}
