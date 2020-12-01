using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode
{
    idle,
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S;

    [Header("Set in Inspector")]
    public Text uitLevel;
    public Text uitShots;
    public Text uitButton;
    public Vector3 castlePos;//the position of our castle
    public GameObject[] castles;//castles array

    [Header("Set Dynamically")]
    public int level;//current level
    public int levelMax;
    public int shotsTaken;
    public GameObject castle;//current castle
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot";//FollowCam mode

    private void Start()
    {
        S = this;

        level = 0;
        levelMax = castles.Length;
        StartLevel();
    }

    void StartLevel()
    {
        //remove the previous castle
        if (castle != null)
        {
            Destroy(castle);
        }
        //remove the previous projectile
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach(GameObject pTemp in gos)
        {
            Destroy (pTemp);
        }

        //create new castle and set new number of shots
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        shotsTaken = 0;

        //put the camera on start position
        SwitchView("Show Both");
        ProjectileLine.S.Clear();

        Goal.goalMet = false;

        UpdateGUI();

        mode = GameMode.playing;
    }

    void UpdateGUI()
    {
        //show current data with UI
        uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        uitShots.text = "Shots Taken: " + shotsTaken;
    }

    private void Update()
    {
        UpdateGUI();
        //checking the level completion 
        if ((mode == GameMode.playing) && Goal.goalMet)
        {
            mode = GameMode.levelEnd;
            SwitchView("Show Both");
            Invoke("NextLevel", 2f);
        }
    }

    void NextLevel()
    {
        level++;
        if (level == levelMax)
        {
            level = 0;
        }
        StartLevel();
    }
    
    public void SwitchView(string eView = "")
    {
        //send empty string to this method to use it in the Button from On Click()
        if (eView == "")
        {
            eView = uitButton.text;
        }
        showing = eView;
        switch (showing)
        {
            case "Show Slingshot":
                FollowCamera.POI = null;
                uitButton.text = "Show Castle";
                break;
            case "Show Castle":
                FollowCamera.POI = S.castle;
                uitButton.text = "Show Both";
                break;
            case "Show Both":
                FollowCamera.POI = GameObject.Find("ViewBoth");
                uitButton.text = "Show Slingshot";
                break;
        }
    }

    public static void ShotFired()
    {
        //fom "Slingshot" script to know that shot is done
        S.shotsTaken++;
    }
}
