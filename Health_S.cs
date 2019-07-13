using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health_S : MonoBehaviour
{
    public GameObject playerAsset;

    public GameObject healthTie;
    public GameObject healthTiePart;

    public List<GameObject> healthTies = new List<GameObject>();

    private Player_S player;
    private int playerHealth = 0;

    // Start is called before the first frame update
    void Start()
    {
        SetupHealthTies();
    }

    
    // ** Methods **
    private void SetupHealthTies()
    {
        GameObject tie;

        player = playerAsset.GetComponent<Player_S>();
        playerHealth = (int)player.getHealth();


        for (int i = playerHealth; i > 0; i -= 2)
        {
            if (i > 1) { tie = Instantiate(healthTie, transform); }
            else { tie = Instantiate(healthTiePart, transform); }
            healthTies.Add(tie);
        }
    }
    public void activateHealthTies(bool enable)
    {
        foreach (GameObject tieImage in healthTies)
        {
            tieImage.GetComponent<Image>().enabled = enable;
        }
    }



    // ** Get Methods **


    // ** Set Methods **
    public void setHealth()
    {
        playerHealth--;

        healthTies.RemoveAt(playerHealth/2);
        Destroy(transform.GetChild(playerHealth/2).gameObject);

        if (playerHealth % 2 == 1)
        {
            GameObject tie = Instantiate(healthTiePart, transform);
            healthTies.Add(tie);
        }
    }
}
