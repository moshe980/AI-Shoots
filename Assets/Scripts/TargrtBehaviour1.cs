using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargrtBehaviour1 : MonoBehaviour
{
    private Vector3 new_location;

    // Start is called before the first frame update
    void Start()
    {
        if (tag == "Live" || tag == "Ammo" || tag == "RandomWalk")
        {
          Vector3 new_location = new Vector3();
          float x, y, z;

         new_location.x = Random.Range(-90f, 90f);
          new_location.z = Random.Range(-90f, 90f);
          new_location.y = 1f;

            if(!Physics.CheckSphere(new_location, 0.5f))
           {
            transform.position = new_location;
           }
        }

        

        

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnTriggerEnter(Collider other)
    {

        if (tag == "Live"&& (other.tag == "Player"|| other.CompareTag("Enemy")))
        { 
            other.GetComponent<FieldOfView>().setHp(other.GetComponent<FieldOfView>().getHp() +20);

            new_location = new Vector3(Random.Range(-90f, 90f),0.541f, Random.Range(-90f, 90f));

            if (!Physics.CheckSphere(new_location, 0.5f))
            {
                transform.position = new_location;
            }

        }
        else if (CompareTag("Ammo")&&(other.CompareTag("Player") || other.CompareTag("Enemy" ) ))
        {
            reload(other, 5);
            other.GetComponent<FieldOfView>().setAmmo(other.GetComponent<FieldOfView>().getAmmo() +20);
            new_location = new Vector3(Random.Range(-90f, 90f), 0.541f, Random.Range(-90f, 90f));

            if (!Physics.CheckSphere(new_location, 0.5f))
            {
                transform.position = new_location;
            }

        }
        else if (CompareTag("RandomWalk") && (other.CompareTag("Player") || other.CompareTag("Enemy")))
        {

            new_location = new Vector3(Random.Range(-90f, 90f), 0.541f, Random.Range(-90f, 90f));

            if (!Physics.CheckSphere(new_location, 0.5f))
            {
                transform.position = new_location;
            }

        }
  



    }
    public void OnTriggerExit(Collider other)
    {

    }
    private IEnumerator reload(Collider other, float delay)
    {
        other.GetComponent<Unit>().speed = 0;       
        other.GetComponent<Animator>().Play("Reload");
        yield return new WaitForSeconds(delay);

    }
}
