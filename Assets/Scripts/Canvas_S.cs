using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canvas_S : MonoBehaviour // Used to easily get all assets and scripts involved with the canvas
{
    [SerializeField]
    private GameObject GunUIAsset = null;
    [SerializeField]
    private GameObject HealthUIAsset = null;
    [SerializeField]
    private GameObject QuestUIAsset = null;
    [SerializeField]
    private Text GunUIText = null;
    [SerializeField]
    private Text LevelUIText = null;


    [HideInInspector]
    public Animator GunUIAnimator = null;
    [HideInInspector]
    public Health_S HealthUI = null;
    [HideInInspector]
    public QuestionText_S QuestUI = null;


    public static Canvas_S self;


    void Awake()
    {
        if (self == null)
        {
            self = this;

            DontDestroyOnLoad(gameObject); // Basic method to remain even after scene load
            GameController_S.maintainedScripts.Add(gameObject);
        }
        else Destroy(gameObject);


        HealthUI = HealthUIAsset.GetComponent<Health_S>();
        QuestUI = QuestUIAsset.GetComponent<QuestionText_S>();
    }


    // Start is called before the first frame update
    void Start()
    {
        GunUIAnimator = GunUIAsset.GetComponent<Animator>(); // Gun UI Animator has awake methods and cannot be called too early
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    // ** Methods **
    public void hideUnnecessaryUI()
    {
        GunUIText.gameObject.GetComponent<CanvasRenderer>().SetAlpha(0);
        HealthUIAsset.SetActive(false);
        GunUIAsset.SetActive(false);
    }


    public void showUnnecessaryUI()
    {
        GunUIText.gameObject.GetComponent<CanvasRenderer>().SetAlpha(1f);
        HealthUIAsset.SetActive(true);
        GunUIAsset.SetActive(true);
    }


    // ** Set Methods ** 


    // ** Get Methods **
    public static Canvas_S local()
    {
        return self;
    }

    public Text getGunUIText()
    {
        return GunUIText;
    }

    public Text getLevelUIText()
    {
        return LevelUIText;
    }
}   
