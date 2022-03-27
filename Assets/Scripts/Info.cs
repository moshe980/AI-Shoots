using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Info : MonoBehaviour
{
    public TextMesh info;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        info.text = "HP:"+ GetComponent<FieldOfView>().getHp()+"\n"
            +"Ammo:"+ GetComponent<FieldOfView>().getAmmo();

    }
}
