using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocker : MonoBehaviour
{
    public bool targetLockingTrap;
    public Transform stoneSpawnpoint;
    public Transform target;
    public Transform targetForRock;
    public GameObject targetParticle;

    public Collider trapCollider;

    public Animator _stoneAnim;
    public float difficultyThreshold = 1f;

    public GameObject Dust;

    public bool targetLocked;

    public GameObject[] stones;

    GameObject selectedStone;

    public float lockInTime = 0.5f;
    public float followSpeed;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        _stoneAnim.speed = difficultyThreshold;
    }

    public void LaunchTrap()
    {
        TrapReset();
        targetLocked = false;
        targetParticle.GetComponent<ParticleSystem>().Play();
        selectedStone = stones[Random.Range(0, stones.Length)];
        StartCoroutine(LockTarget(lockInTime));
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null && !targetLocked && targetLockingTrap)
        {
            // Calculate the new position to move towards the target's x position.
            Vector3 newPosition = transform.position;
            newPosition.x = Mathf.Lerp(transform.position.x, target.position.x, followSpeed * Time.deltaTime);

            // Update the object's position.
            transform.position = newPosition;
        }
    }

    IEnumerator LockTarget(float lockDuration)
    {
        yield return new WaitForSeconds(lockDuration);

        targetLocked = true;

        if(selectedStone == null)
        {
            selectedStone = stones[Random.Range(0, stones.Length)];
        }
        DropStone(selectedStone);
    }

    public void DropStone(GameObject stone)
    {
        Instantiate(stone, stoneSpawnpoint);
        _stoneAnim.SetBool("Throw", true);
        //rock.warnParticle = targetParticle;
        //rock.dust = Dust;
        //rock.targetObject = targetForRock;
    }

    public void DustParticle()
    {
        trapCollider.enabled = true;
        Dust.SetActive(true);
        Dust.GetComponent<ParticleSystem>().Play();
        targetParticle.GetComponent<ParticleSystem>().Stop();
        targetParticle.SetActive(false);
    }

    public void TrapReset()
    {
        _stoneAnim.speed = difficultyThreshold;
        foreach (Transform child in stoneSpawnpoint)
        {
            Destroy(child.gameObject);
        }
        _stoneAnim.SetBool("Throw", false);
    }
}
