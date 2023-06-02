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
    static private MissionDemolition S; //прихований одиничний об'єкт

    [Header("Set in Inspector")]
    public Text uitLevel;   //посилання на об'єкт UIText_Level
    public Text uitShots;   //посилання на обёэкт UIText_Shots
    public Text uitButton;  //посилання на дочерний об'єкт Text в UIButton_View
    public Vector3 castlePos;   //місцезнаходження замка
    public GameObject[] castles;    //масив замків

    [Header("Set Dynamically")]
    public int level;   //поточний рівень
    public int levelMax;    //кількість рівнів
    public int shotsTaken;
    public GameObject castle;   //поточний замок
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot";   //режим FollowCam

    // Start is called before the first frame update
    void Start()
    {
        S = this;   //оприділяє одиничний об'єкт
        level = 0;
        levelMax = castles.Length;
        StartLevel();
    }
    void StartLevel()
    {
        //знищує попередній замок, якщо він існує
        if (castle != null)
            Destroy(castle);

        //знищує попередні знаряди, якшо вони існують
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject pTemp in gos)
            Destroy(pTemp);

        //створити новий замок
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        shotsTaken = 0;

        //перевстановити камеру в початкову позицію
        SwitchView("Show Both");
        ProjectileLine.S.Clear();

        //скинути ціль
        Goal.goalMet = false;

        UpdateGUI();

        mode = GameMode.playing;
    }
    void UpdateGUI()
    {
        //показати дані в елементах UI
        uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        uitShots.text = "Shots Taken: " + shotsTaken;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGUI();

        //перевірити заверщення рівня
        if ((mode == GameMode.playing) && Goal.goalMet)
        {
            //змінити режим, щоб зупинити перевірку завершення рівня
            mode = GameMode.levelEnd;
            //зменшити масштаб
            SwitchView("Show Both");
            //почати новий рівень через 2 секунди
            Invoke("NextLevel", 2f);
        }
    }
    void NextLevel()
    {
        level++;
        if (level == levelMax)
            level = 0;

        StartLevel();
    }
    public void SwitchView(string eView = "")
    {
        if(eView == "")
            eView = uitButton.text;

        showing = eView;
        switch (showing)
        {
            case "Show Slingshot":
                FollowCam.POI = null;
                uitButton.text = "Show Castle";
                break;
            case "Show Castle":
                FollowCam.POI = S.castle;
                uitButton.text = "Show Both";
                break;
            case "Show Both":
                FollowCam.POI = GameObject.Find("ViewBoth");
                uitButton.text = "Show Slingshot";
                break;
        }
    }
    //статичний метод, дозволяючий з будь-якого коду збільшити shotsTaken
    public static void ShotFired()
    {
        S.shotsTaken++;
    }
}
