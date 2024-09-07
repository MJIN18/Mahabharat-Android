using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TiltController : MonoBehaviour
{

    Rigidbody rb;

   [SerializeField]float dx;
   [SerializeField] float dy;
   [SerializeField]float moveSpeed = 20f;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        dx = Input.acceleration.x * moveSpeed;
   //     dy = Input.acceleration.y * moveSpeed;

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -2.15f, 2.15f), transform.position.y, transform.position.z);
        
   //     transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -2.15f, 2.15f), transform.position.z);
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(dx,0f,0f);

    //    rb.velocity = new Vector3(0f, dy, 0f);
    }
  
}
