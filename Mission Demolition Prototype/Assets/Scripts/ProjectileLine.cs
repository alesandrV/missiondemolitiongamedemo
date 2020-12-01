using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    //this script had to create a trail(line) of the projectile, but something went wrong
    static public ProjectileLine S;

    [Header("Set in Inspecrot")]
    public float minDist = 0.1f;

    private LineRenderer line;
    private GameObject _poi;
    private List<Vector3> points;

    private void Awake()
    {
        S = this;
        line = GetComponent<LineRenderer>();
        line.enabled = false;
        points = new List<Vector3>();
    }

    public GameObject poi
    {
        get
        {
            return _poi;
        }
        set
        {
            _poi = value;
            if (_poi != null)
            {
                line.enabled = false;
                points = new List<Vector3>();
                AddPoint();
            }
        }
    }

    public void Clear()
    {
        //to clear the previous line
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }

    public void AddPoint()
    {
        //to add a dot to the line
        Vector3 pt = _poi.transform.position;
        if (points.Count > 0 && (pt - lastPoint).magnitude < minDist)
        {
            return;
        }

        if (points.Count == 0)
        {
            Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS;
            points.Add(pt + launchPosDiff);
            points.Add(pt);

            line.positionCount = 2;
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);

            line.enabled = true;

        }
    }

    public Vector3 lastPoint
    {
        //get the position of the last dot
        get
        {
            if (points == null)
            {
                return (Vector3.zero);
            }
            return (points[points.Count - 1]);
        }
    }

    void FixedUpdate()
    {
        //searching for POI
        if (poi == null)
        {
            if (FollowCamera.POI != null)
            {
                if (FollowCamera.POI.tag == "Projectile")
                {
                    poi = FollowCamera.POI;
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
        AddPoint();
        if (FollowCamera.POI == null)
        {
            poi = null;
        }
    }

}