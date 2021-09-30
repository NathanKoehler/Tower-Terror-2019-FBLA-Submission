using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;


public class GameController_S : MonoBehaviour
{
    public static List<GameObject> maintainedScripts = new List<GameObject>();
    public static GameController_S self;

    [SerializeField]
    public List<QnA_S> qnaArray = new List<QnA_S>();

    public GameObject[] tableFurniture = null;

    [SerializeField]
    private GameObject[] layouts = null;


    [SerializeField]
    Camera mainCamera = null;
    [SerializeField]
    Camera secondCamera = null;
    [SerializeField]
    Canvas canvasAsset = null;
    [SerializeField]
    GameObject player = null;
    [SerializeField]
    AudioManager_S AudioManager = null;
    [SerializeField]
    JudgingSystem_S judgingSystem = null;
    [SerializeField]
    GameObject collectable = null;
    [SerializeField]
    GameObject elevatorRail = null;
    [SerializeField]
    GameObject elevatorBackground = null;
    [SerializeField]
    GameObject alien = null;
    [SerializeField]
    Canvas_S canvas;


    private bool canGoToNext;
    private bool canEarnPoints;
    private int level = 0;
    private int questionsWrong = 0;
    private int questionsRight = 0;
    private int amountRight = 0;


    void Awake()
    {
        if (self == null)
        {
            self = this;
            DontDestroyOnLoad(gameObject); // Basic method to remain even after scene load
        }
        else Destroy(gameObject);

    }


    // Start is called before the first frame update
    private void Start()
    {
        
    }


    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
            Debug.Log("Quit");
        }
        if (amountRight == 50) // AMOUNT OF QUESTIONS TO ANSWER BEFORE WINNING
        {
            player.SetActive(false);
            GameObject.Find("Canvas").SetActive(false);
            GameObject.Find("Audio Manager").SetActive(false);
            GameObject.Find("Elevator").SetActive(false);
            SceneManager.LoadScene("Win");
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Next Level");
        if (collision.gameObject.CompareTag("Player") && canGoToNext)
        {
            NewLevel();
        }
    }


    // ** Methods **
    public void NewLevel()
    {
        var Enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject item in Enemies)
        {
            Destroy(item);
        }


        canGoToNext = false;

        secondCamera.transform.position = mainCamera.transform.position;
        mainCamera.enabled = false;
        secondCamera.enabled = true;
        secondCamera.gameObject.AddComponent<MoveToLocation_S>().setDesiredLocation(new Vector2(0, 5.57f), 0.2f);

        player.SetActive(false);

        canvas.hideUnnecessaryUI();
        AudioManager.StopAllUnnecessaryLoops();

        level++;

        StartCoroutine(FloorTransition());
    }


    public void GameOverScreen()
    {
        SceneManager.LoadScene("Game Over");

        foreach (GameObject obj in maintainedScripts)
        {
            Destroy(obj);
        }

        StartCoroutine(SmallUI());
    }


    public void StartGame()
    {
        gameObject.GetComponent<CameraShake_S>().setCamera(mainCamera);
        StartCoroutine(RestartEverything());
    }


    IEnumerator RestartEverything()
    {
        SceneManager.LoadScene("Floor 1");

        yield return new WaitForFixedUpdate();

        player = Player_S.self.gameObject;
        AudioManager = AudioManager_S.self;
        canvas = Canvas_S.self;
        canvasAsset = Canvas_S.self.GetComponent<Canvas>();
        judgingSystem = JudgingSystem_S.self;
        secondCamera = SecondCamera_S.self.GetComponent<Camera>();
        secondCamera.enabled = false;
        mainCamera = Camera_S.self.GetComponent<Camera>();

        
        level = 1;
        canvas.getLevelUIText().text = "level " + level;


        QnA_S[] array = Resources.LoadAll<QnA_S>("Questions"); // takes all the QnA_S files from Resources
        for (int i = 0; i < array.Length; i++)
        {
            qnaArray.Add(array[i]);
        }


        BuildLevel();
    }


    public void CanChangeLevel()
    {
        Debug.Log("Next Level Enabled");
        Canvas_S.local().QuestUI.rightSequence();

        if (canEarnPoints)
            
        {
            questionsRight++;
        }

        else
            canEarnPoints = true;
        canGoToNext = true;
    }


    public void CannotChangeLevel()
    {
        Canvas_S.local().QuestUI.wrongSequence();

        questionsWrong++;

        canEarnPoints = false;
    }


    public void BuildLevel() // Used to populate the room
    {
        amountRight++;
        int randInt = UnityEngine.Random.Range(0, layouts.Length);
        SetTableFurniture();
        Instantiate(layouts[randInt], new Vector3(0, 0, 0), Quaternion.identity);
        SetupQuestions();
    }


    private void SetTableFurniture() // Adds furniture to the top/around the tables
    {
        List<GameObject> tableFurnitureF = new List<GameObject>();
        List<GameObject> tableFurnitureB = new List<GameObject>();
        List<GameObject> tableFurnitureR = new List<GameObject>();
        List<GameObject> tableFurnitureL = new List<GameObject>();

        foreach (GameObject furniture in tableFurniture) // Loops through all sprites in the Furniture folder
        {
            // Adds Furniture with tag ___ to array ___
            if (furniture.GetComponent<Furniture_S>().facingDirection == 'F') 
                tableFurnitureF.Add(furniture);
            else if (furniture.GetComponent<Furniture_S>().facingDirection == 'B')
                tableFurnitureB.Add(furniture);
            else if (furniture.GetComponent<Furniture_S>().facingDirection == 'R')
                tableFurnitureR.Add(furniture);
            else if (furniture.GetComponent<Furniture_S>().facingDirection == 'L')
                tableFurnitureL.Add(furniture);

            else if (furniture.GetComponent<Furniture_S>().facingDirection == 'H')
            {
                tableFurnitureR.Add(furniture);
                tableFurnitureL.Add(furniture);
            }
            else if (furniture.GetComponent<Furniture_S>().facingDirection == 'V')
            {
                tableFurnitureF.Add(furniture);
                tableFurnitureB.Add(furniture);
            }
        }

        FurnitureParent_S.setAllArrays(tableFurnitureF.ToArray(), tableFurnitureB.ToArray(), tableFurnitureR.ToArray(), tableFurnitureL.ToArray());

        StartCoroutine(waitForAI());
    }




    private void SetupQuestions() // adds the questions
    {
        int selectedQnA_id = UnityEngine.Random.Range(0, qnaArray.Count);
        QnA_S selectedQnA = qnaArray[selectedQnA_id];

        judgingSystem.setID(selectedQnA.id);


        canvas.QuestUI.setQnA(selectedQnA);

        

        GameObject realAnswer = Instantiate(collectable, new Vector2(UnityEngine.Random.Range(-2f, 2f), UnityEngine.Random.Range(-1f, 2f)), Quaternion.identity);
        realAnswer.GetComponent<CollectableAnswer_S>().setID(selectedQnA.id, selectedQnA.answer);

        GameObject fakeAnswer = Instantiate(collectable, new Vector2(UnityEngine.Random.Range(-2f, 2f), UnityEngine.Random.Range(-1f, 2f)), Quaternion.identity);
        fakeAnswer.GetComponent<CollectableAnswer_S>().setID(-1, selectedQnA.fakeAnswer1);

        fakeAnswer = Instantiate(collectable, new Vector2(UnityEngine.Random.Range(-2f, 2f), UnityEngine.Random.Range(-1f, 2f)), Quaternion.identity);
        fakeAnswer.GetComponent<CollectableAnswer_S>().setID(-1, selectedQnA.fakeAnswer2);

        fakeAnswer = Instantiate(collectable, new Vector2(UnityEngine.Random.Range(-2f, 2f), UnityEngine.Random.Range(-1f, 2f)), Quaternion.identity);
        fakeAnswer.GetComponent<CollectableAnswer_S>().setID(-1, selectedQnA.fakeAnswer3);

        qnaArray.RemoveAt(selectedQnA_id);
    }


    // ** Get Methods **
    public QnA_S[] getQNAArray()
    {
        return qnaArray.ToArray();
    }


    public static GameController_S local()
    {
        return self;
    }


    // ** Coroutines **
    IEnumerator FloorTransition()
    {
        GameObject door = GameObject.Find("Door");
        GameObject elevator = GameObject.Find("Elevator");
        door.GetComponent<Animator>().SetTrigger("Activate");
        elevator.GetComponent<Animator>().SetTrigger("Activate");
        secondCamera.GetComponent<Animator>().SetTrigger("Activate");


        yield return new WaitForSecondsRealtime(2f);

        // Creates the background of the elevator
        GameObject elevatorBG = Instantiate(elevatorBackground, transform);

        Enemy_S.target = null;
        Enemy_S.foundTarget = false;



        yield return new WaitForSecondsRealtime(2f);

        // Reloads the Scene
        SceneManager.LoadScene("Floor 1");


        yield return new WaitForFixedUpdate();

        // Loads Items important to the scene transition
        BuildLevel();
        GameObject rail = Instantiate(elevatorRail, new Vector2(0, -4), Quaternion.identity);
        GameObject.Find("Tilemap_Building").GetComponent<TilemapRenderer>().enabled = false;

        for (int i = 0; i < UnityEngine.Random.Range(1 + level / 8, 4 + level / 4); i++)
        {
            Debug.Log(EnemySpawnLocation_S.Spawns.Count);
            int randVector3 = UnityEngine.Random.Range(0, EnemySpawnLocation_S.Spawns.Count);
            Vector3 enemyPos = EnemySpawnLocation_S.Spawns[randVector3];
            EnemySpawnLocation_S.Spawns.RemoveAt(randVector3);
            Instantiate(alien, enemyPos, Quaternion.identity);
        }

        yield return new WaitForFixedUpdate();

        // Finish animations related to the end of a scene transition
        door = GameObject.Find("Door");
        door.GetComponent<Animator>().SetTrigger("NotOpen");
        secondCamera.GetComponent<Animator>().SetTrigger("Activate");
        elevator.GetComponent<Animator>().SetTrigger("Activate");

        elevatorBG.GetComponent<Animator>().SetTrigger("Activate");
        elevatorBG.GetComponentInChildren<Animator>().SetTrigger("Activate");

        yield return new WaitForSecondsRealtime(4f);

        // Scene transition ends; runs code important to the new scene
        Destroy(elevatorBG);
        Destroy(rail);

        door.GetComponent<Animator>().SetTrigger("Activate");

        canvas.showUnnecessaryUI();



        Text levelUIText = canvas.getLevelUIText();
        levelUIText.text = "Level " + level; // Updates the level text


        yield return new WaitForSecondsRealtime(0.5f);

        // Player can walk around
        GameObject.Find("Tilemap_Building").GetComponent<TilemapRenderer>().enabled = true;

        player.SetActive(true);

        Enemy_S.target = player.transform;
        EnemyPistol_S.player = player.transform;
        Enemy_S.foundTarget = true;

        GameObject.Find("Hand").GetComponent<Pistol_S>().Reload(true);
        secondCamera.enabled = false;
        mainCamera.enabled = true;
    }


    IEnumerator waitForAI()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        var graphToScan = AstarPath.active.data.gridGraph;
        AstarPath.active.Scan(graphToScan);
    }
    

    IEnumerator SmallUI()
    {
        yield return new WaitForSecondsRealtime(1f);

        GameObject.Find("Canvas").transform.GetChild(1).GetComponent<Text>().text = "questions right : " + questionsRight;
        GameObject.Find("Canvas").transform.GetChild(2).GetComponent<Text>().text = "questions wrong : " + questionsWrong;
    }
}
