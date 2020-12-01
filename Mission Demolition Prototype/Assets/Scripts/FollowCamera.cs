using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class FollowCamera : MonoBehaviour
{
    static public GameObject POI;//Point Of Interest. Static, because every class instance will use it

    [Header("Set in Inspector")]
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;

    [Header("Set Dynamically")]
    public float camZ;//Z coordinate which we want to get

    private void Awake()
    {
        camZ = this.transform.position.z;
    }
    private void FixedUpdate()
    {
        Vector3 destination;
        //back to 0,0,0 while there is no object
        if (POI == null)
        {
            destination = Vector3.zero;
        }
        //or get POI`s position
        else
        {
            destination = POI.transform.position;

            if(POI.tag == "Projectile")
            {
                //if our ball does not move = back to start
                if (POI.GetComponent<Rigidbody>().IsSleeping())
                {
                    POI = null;

                    return;
                }
            }
        }

        //limit x & y with min points
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        destination = Vector3.Lerp(transform.position, destination, easing);//smooth camera movement
        destination.z = camZ;

        //add our camera into "destination" position
        transform.position = destination;
        Camera.main.orthographicSize = destination.y + 10;
    }
}
