using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMovement : MonoBehaviour
{
    //public GameObject AssetProp;       
    public float speed = 0.5f;

    [SerializeField] Vector3 OriginalPos;
    [SerializeField] Vector3 DestinationPos = new Vector3(-200.134766f, -381.375946f, -3806);

    //Movement setup
    private float startTime;
    private float journeyLength;
    public GameManagerScript GM;

    [SerializeField] bool isMoving = false;

    


    private void Start()
    {
        startTime = Time.time;
        journeyLength = Vector3.Distance(transform.position, DestinationPos);
        OriginalPos = transform.position;

     
    }

    private void StartReturnMovement()
    {
        isMoving = true;
        transform.position = OriginalPos;
        startTime = Time.time;
        journeyLength = Vector3.Distance(transform.position, DestinationPos);
    }

    private float EaseOut(float t)
    {
        return 1f - Mathf.Pow(1f - t, 2f);
    }

    private void Update()
    {
        if (GM.StartMovingWall) isMoving = true;
        if (!GM.StartMovingWall)
        {
            //transform.position = OriginalPos;
            isMoving = false;
        }

        float distCovered = (Time.time - startTime) * speed;
        float fractionOfJourney = distCovered / journeyLength;

        if (isMoving)
        {
            // Apply the ease out interpolation
            float easedFraction = EaseOut(fractionOfJourney);

            transform.position = Vector3.Lerp(transform.position, DestinationPos, easedFraction);

            if (transform.position.z <= DestinationPos.z+ 0.5f)
            {
                isMoving = false;
                //Invoke("StartReturnMovement", 0.1f); // Delay the return movement to synchronize both objects
                StartReturnMovement();
            }
        }


    }
}
   
