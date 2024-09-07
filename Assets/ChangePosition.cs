using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePosition : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(DisableCollider());
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ohert========" + other.name + "tag======" + other.tag);
        if (other.gameObject.tag == "Traps")
        {
            Debug.Log("Yes its being called bro"+this.transform.parent.name+"Objects previous position========="+this.transform.parent.position);
            this.transform.parent.transform.position = new Vector3(this.transform.parent.position.x, this.transform.parent.position.y, this.transform.parent.position.z + 5);
            Debug.Log("Yes its being called bro 11" + this.transform.parent.name + "Objects after position=========" + this.transform.parent.position);
        }
        else if (other.gameObject.tag == "Coin")
        {
            Debug.Log("Yes its being called bro");
            this.transform.parent.transform.position = new Vector3(this.transform.parent.position.x, this.transform.parent.position.y, this.transform.parent.position.z + 5);
        }

        GetComponent<BoxCollider>().enabled = false;
    }


    IEnumerator DisableCollider()
    {
        yield return new WaitForSeconds(2f);
        GetComponent<BoxCollider>().enabled = false;
    }
}
