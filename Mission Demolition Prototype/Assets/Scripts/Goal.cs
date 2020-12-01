using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    static public bool goalMet = false;

    private void OnTriggerEnter(Collider other)
    {
        //get reaction of our target
        if (other.gameObject.tag == "Projectile")
        {
            Goal.goalMet = true;

            //increase the opacity to show our player his lucky hit
            Material mat = GetComponent<Renderer>().material;
            Color c = mat.color;
            c.a = 1;
            mat.color = c;
        }
    }
}
