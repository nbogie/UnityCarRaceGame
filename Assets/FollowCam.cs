using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform target;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Follow Cam started");
        
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody rb = target.GetComponent<Rigidbody>();

        transform.position = target.position + new Vector3(0, 10, -25);
        var speed = Mathf.Abs(rb.velocity.magnitude);
        if (rb){
            transform.position = target.position + new Vector3(0, 10, -5 - speed);                
        }

        transform.LookAt(target);
        
    }
}
