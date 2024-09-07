using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    public Collider trapCollider;

    public GameObject[] flames;
    public GameObject[] arrows;

    public int ver;

    public float difficultyThreshold = 1f;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateTrap()
    {
        ResetTrap();
        ShootArrow();
    }

    public void ShootArrow()
    {
        trapCollider.enabled = true;
        arrows[ver].gameObject.SetActive(true);
        animator.speed = difficultyThreshold;
        animator.SetBool("Shoot", true);
    }

    public void ResetTrap()
    {
        foreach (GameObject arrow in arrows)
        {
            arrow.gameObject.SetActive(false);
        }
        foreach (GameObject flame in flames)
        {
            flame.gameObject.SetActive(false);
        }
        animator.SetBool("Shoot", false);
    }

    public void StartFlame()
    {
        flames[ver].SetActive(true);
        flames[ver].GetComponent<ParticleSystem>().Play();
    }
}
