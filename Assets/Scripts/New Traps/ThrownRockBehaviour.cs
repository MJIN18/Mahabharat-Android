using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownRockBehaviour : MonoBehaviour
{
    public bool shouldBreak;

    public GameObject[] pieces;
    public GameObject fullStone;
    public GameObject warnParticle;
    public GameObject dust;
    //public float sphereRadius;
    public float throwForce = 1f;

    public float maxRandomRotation = 30.0f; // Maximum random rotation in degrees


    public Transform targetObject;
    public float moveTime = 2.0f; // Time in seconds

    private Vector3 initialPosition;
    private float elapsedTime = 0.0f;

    private void Start()
    {
        if(shouldBreak)
        {
            foreach(var piece in pieces)
            {
                piece.SetActive(true);
            }
            fullStone.SetActive(false);
        }
        else
        {
            foreach (var piece in pieces)
            {
                piece.SetActive(false);
            }
            fullStone.SetActive(true);
        }

        initialPosition = transform.position;
        //StartMoving();
        //Throw();
    }

    void Throw()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Get the object's forward vector and apply force in that direction.
            Vector3 throwDirection = transform.forward;
            rb.AddForce(throwDirection * throwForce, ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(shouldBreak)
        {
            if (collision != null)
            {
                if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Player")
                {
                    foreach (GameObject piece in pieces)
                    {
                        Rigidbody rb = piece.GetComponent<Rigidbody>();
                        rb.isKinematic = false;
                        rb.useGravity = true;
                        dust.SetActive(true);
                    }
                }
            }
        }
        else
        {
            if (collision != null)
            {
                if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Player")
                {
                    Rigidbody rb = fullStone.GetComponent<Rigidbody>();
                    //rb.transform.parent = null;
                    rb.isKinematic = false;
                    rb.useGravity = true;
                    dust.SetActive(true);
                    dust.GetComponent<ParticleSystem>().Play();
                    //rb.AddForce(Vector3.up, ForceMode.Force);
                }
            }
        }
    }

    private void OnHitEvent(Collider collision)
    {
        if (shouldBreak)
        {
            if (collision != null)
            {
                if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Player")
                {
                    foreach (GameObject piece in pieces)
                    {
                        Rigidbody rb = piece.GetComponent<Rigidbody>();
                        rb.isKinematic = false;
                        rb.useGravity = true;
                        dust.SetActive(true);
                    }
                }
            }
        }
        else
        {
            if (collision != null)
            {
                if (collision.gameObject.tag == "Ground")
                {
                    if(warnParticle != null)
                    {
                        warnParticle.GetComponent<ParticleSystem>().Stop();
                        warnParticle.SetActive(false);
                    }
                    Rigidbody rb = fullStone.GetComponent<Rigidbody>();
                    //rb.transform.parent = null;
                    rb.isKinematic = false;
                    rb.useGravity = true;
                    dust.SetActive(true);
                    dust.GetComponent<ParticleSystem>().Play();
                    //rb.AddForce(Vector3.up, ForceMode.Force);
                }
            }
        }
    }

    void StartMoving()
    {
        StartCoroutine(MoveToTarget());
    }

    IEnumerator MoveToTarget()
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / moveTime;

            // Interpolate the position as before
            transform.position = Vector3.Lerp(initialPosition, targetObject.position, t);

            // Add random rotation
            //Quaternion randomRotation = Quaternion.Euler(
            //    Random.Range(-maxRandomRotation, maxRandomRotation),
            //    Random.Range(-maxRandomRotation, maxRandomRotation),
            //    Random.Range(-maxRandomRotation, maxRandomRotation)
            //);

            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, t);

            yield return null;
        }
    }
}
