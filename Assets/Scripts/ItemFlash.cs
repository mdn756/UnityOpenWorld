using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFlash : MonoBehaviour
{
    private Rigidbody rb;
    private float yMovement;
    private Vector3 rot_speed = new Vector3(0, 100, 0);
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        yMovement = Mathf.Sin(5.0f * Time.time);
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.x + 0.4f * yMovement, rb.velocity.z);
        rb.velocity = transform.TransformDirection(rb.velocity);
        
        Quaternion deltaRotation = Quaternion.Euler(rot_speed * Time.deltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }
}
