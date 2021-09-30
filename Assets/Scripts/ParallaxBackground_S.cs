using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground_S : MonoBehaviour
{
    [SerializeField]
    private Transform camMainTrans;

    public Transform[] backgrounds;
    private float[] parallaxScales;
    public float speed = 1f;

    private Vector3 previousCamPos; // Used to store position of camera in previous frame


    private void Awake()
    {
        
    }


    // Start is called before the first frame update
    void Start()
    {
        previousCamPos = camMainTrans.position; // Storing pos of the previous frame

        parallaxScales = new float[backgrounds.Length];

        for (int i = 0; i < backgrounds.Length; i++)
        {
            parallaxScales[i] = backgrounds[i].position.z * -1;
        }
    }

    
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            float parallax = (previousCamPos.x - camMainTrans.position.x) * parallaxScales[i];

            // set a target  position which is the current position plus the parallax modifier
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;

            // creates a background position with a modified target x positon;
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, speed * Time.deltaTime);
        }

        // set the previous camera position to the current camera position
        previousCamPos = camMainTrans.position;
    }
}
