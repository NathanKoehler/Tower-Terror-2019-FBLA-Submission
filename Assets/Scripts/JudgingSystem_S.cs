using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgingSystem_S : MonoBehaviour
{
    [SerializeField]
    private GameController_S gameController = null;
    [SerializeField]
    private GameObject audioManager = null;
    [SerializeField]
    private GameObject player = null;


    private int id = 0;

    public static JudgingSystem_S self;


    private void Awake()
    {
        if (self == null)
        {
            self = this;

            DontDestroyOnLoad(gameObject); // Basic method to remain even after scene load
            GameController_S.maintainedScripts.Add(gameObject);
        }
        else Destroy(this);
    }


    private void Start()
    {
        gameController = GameController_S.self;
        audioManager = AudioManager_S.self.gameObject;
        player = Player_S.self.gameObject;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Collectable") && collision.gameObject.GetComponent<Collectable_S>().getCollected())
        {
            CollectableAnswer_S collectable = collision.gameObject.GetComponent<CollectableAnswer_S>();

            if (collectable.getID() == id)
            {
                collectable.ColorFlash(3, Color.green);
                collision.gameObject.AddComponent<MoveToLocation_S>().setDesiredLocation(new Vector2(0, 20), true);


                gameController.CanChangeLevel();
                audioManager.GetComponent<AudioManager_S>().Play("Award Sound");
                player.GetComponent<Player_S>().setCollectable(collision.gameObject, false);
            }
            else
            {
                collectable.ColorFlash(3, Color.red);

                gameController.CannotChangeLevel();
            }
        }
    }



    // ** Get Methods **
    public static JudgingSystem_S local()
    {
        return self;
    }

    // ** Set Methods **
    public void setID(int newID)
    {
        id = newID;
    }
}
