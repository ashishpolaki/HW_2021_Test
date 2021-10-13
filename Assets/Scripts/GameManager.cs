using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Pulpit pulpitPrefab;

    public HashSet<Vector3> savePreviousPositions = new HashSet<Vector3>();
    public List<Pulpit> pulpitPrefabList = new List<Pulpit>();
    int totalPupilsCount = 3;


    public Pulpit previousPulpit;
    public Pulpit nextPulpit;
    public Transform presentPulpitPos;
    public Player player;

    int score;
    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
            scoreTxt.text = score.ToString();
        }
    }


    [Space(10)]
    [Header("UI")]
    public Text scoreTxt;
    public GameObject gameOverPanel;
    bool isGameOver;
    public bool IsGameOver
    {
        get
        {
            return isGameOver;
        }
        set
        {
            isGameOver = value;
            if (isGameOver)
            {
                //Clear the data
                gameOverPanel.SetActive(true);
                Score = 0;
                savePreviousPositions.Clear();
                foreach (var item in pulpitPrefabList)
                {
                    item.StopPupilCountDownTimer();
                    item.gameObject.SetActive(false);
                    item.transform.position = Vector3.zero;
                }
                player.rigid.Sleep();
                nextPulpit = null;
                previousPulpit = null;
                presentPulpitPos = null;
            }
        }
    }
    private void Awake()
    {
        instance = this;
        Time.timeScale = 0;
    }
    public void StartButton()
    {
        Time.timeScale = 1;
    }
    void Start()
    {
        CreateItems(totalPupilsCount);
        SpawnPulpit();
    }
   
    public void Retry()
    {
        Time.timeScale = 1;
        IsGameOver = false;

        player.transform.position = new Vector3(0, 5, 0);
        SpawnPulpit();
        player.transform.rotation = Quaternion.identity;
        player.rigid.WakeUp();
    }
    void CreateItems(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var pulpit = Instantiate(pulpitPrefab, this.transform);
            pulpit.gameObject.SetActive(false);
            pulpitPrefabList.Add(pulpit);
        }
    }

    Pulpit GetPulpitFromPoolList()
    {
        return pulpitPrefabList.Find(x => !x.gameObject.activeSelf);
    }

    Vector3 RandomAdjacentPositions()
    {
        Vector3 newPosition = Vector3.zero;
        int randomSpawn = Random.Range(0, 4);
        switch (randomSpawn)
        {
            case 0:
                newPosition = new Vector3(9 + presentPulpitPos.position.x, 0, presentPulpitPos.position.z); //SpawnRight
                break;
            case 1:
                newPosition = new Vector3(-9 + presentPulpitPos.position.x, 0, presentPulpitPos.position.z); //SpawnLeft
                break;
            case 2:
                newPosition = new Vector3(presentPulpitPos.position.x, 0, 9 + presentPulpitPos.position.z);//SpawnForward
                break;
            case 3:
                newPosition = new Vector3(presentPulpitPos.position.x, 0, -9 + presentPulpitPos.position.z); //SpawnBackward
                break;
            default:
                break;
        }
        if (savePreviousPositions.Contains(newPosition))
        {
            return RandomAdjacentPositions();
        }
        else
            return newPosition;
    }


    public void SpawnPulpit()
    {
        Pulpit pulpit = GetPulpitFromPoolList();
        pulpit.gameObject.SetActive(true);
        nextPulpit = pulpit;
        if (savePreviousPositions.Count > 0) //From Second Pupil change the adjacent position randomly
        {
            pulpit.transform.position = RandomAdjacentPositions();
        }
        pulpit.StartPupilCountDownTimer();
        savePreviousPositions.Add(pulpit.transform.position);
        presentPulpitPos = pulpit.transform;
    }
}

