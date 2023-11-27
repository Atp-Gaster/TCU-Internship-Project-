using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{    
    [SerializeField] bool StartDebug = false;
    public int Hitpoint = 3;
    public bool StartMovingWall = false;
    public GameObject WallGroup;
    public GameObject[] HitboxCheckerStorage;
    public GameObject[] HitboxChecker;
    public GameObject[] WallObjectStorage;
    public GameObject[] WallObject;
    public GameObject[] HitpointBar;
    public GameObject OpenPoseManager;
    public bool DuringSelection = false;
    public bool IsEnd = false;
    public int CurrentWall = 0;
    public Text ScoreText;
    public Text PlayerText;
    public Text EndText;
    public Text CounterText;
    public int Score = 0;
    public int GameMode = 0; // 0 = single sequence , 1 = all at once
    public bool Hit = false;
    public int PlayerAScore = 0;
    public int PlayerBScore = 0;
    public int PlayerTurn = 0;
    [SerializeField] AudioSource HitSFX;
    private IEnumerator coroutine;
    public int HitpointA = 3;
    public int HitpointB = 3;
    public Animator DeadCam;
    public GameObject RestartUI;    
    public TextController TC; //Text Controller    

    private float startTime;
    private float journeyLength;


    /* Note
     * Initial stage of wall object need to start Z position at 2500
     * Final stage of wall object need to start Z position at 150 
     */

    private float EaseOut(float t) //Using for removing slow down when nearly end of every .lerp
    {
        return 1f - Mathf.Pow(1f - t, 2f);
    }

    #region Competitive_mode
    public void Continue()
    {
        float distCovered = (Time.time - startTime) * 3f;
        float fractionOfJourney = distCovered / journeyLength;

        // Apply the ease out interpolation
        float easedFraction = EaseOut(fractionOfJourney);

        //WallGroup.transform.position = Vector3.Lerp(WallGroup.transform.position, new Vector3(0, 0, -7347), 0.2f * Time.deltaTime);
        WallGroup.transform.position = Vector3.MoveTowards(WallGroup.transform.position, new Vector3(0, 0, -7347), 15);

        if (StartMovingWall)
        {
            if (WallGroup.transform.position.z >= -7347)
            {
                //Debug.Log("Start Moving Wall");
                WallGroup.gameObject.SetActive(true);
                    

                if (WallObject[CurrentWall].transform.position.z <= 160 )
                {
                    StartMovingWall = false;

                    HitboxChecker[CurrentWall].gameObject.SetActive(true);
                    coroutine = DelayStopCheckingCollsion();
                    StartCoroutine(coroutine);
                }
            }
        }                                        
    }

    #endregion   

    #region Coustom mode
        public void ResetTheOrder()
        {
            GameObject[] tempWall = new GameObject[WallObjectStorage.Length];
            GameObject[] tempHitbox = new GameObject[HitboxCheckerStorage.Length];
            WallObject = tempWall;
            HitboxChecker = tempHitbox;
            for (int i = 0; i < WallObjectStorage.Length; i++)
            {            
                WallObject[i] = WallObjectStorage[i];
                HitboxChecker[i] = HitboxCheckerStorage[i];
            }
        }

        public void SetupWallOrder(List<int> InputOrder)
        {
            Debug.Log(InputOrder);
            StartDebug = true;
            //SelectInterface.SetActive(false);
            GameObject[] rearrangedGameObjects = new GameObject[5]; // 5 This using for Set up wall Sequence
            GameObject[] rearrangedHitbox = new GameObject[5];

            //Use to set hitbox
            for (int i = 0; i < InputOrder.Count; i++)
            {
                int newIndex = InputOrder[i];
                if (newIndex < 0 || newIndex >= HitboxChecker.Length)
                {
                    Debug.LogError("Invalid index in the sequence list.");
                    return;
                }
                rearrangedHitbox[i] = HitboxChecker[newIndex];
            }

            HitboxChecker = rearrangedHitbox;

            //Use to set WallObject
            for (int i = 0; i < InputOrder.Count; i++)
            {
                int newIndex = InputOrder[i];
                if (newIndex < 0 || newIndex >= WallObject.Length)
                {
                    Debug.LogError("Invalid index in the sequence list.");
                    return;
                }

                rearrangedGameObjects[i] = WallObject[newIndex];
            }

            WallObject = rearrangedGameObjects;            
        }

        public void ShowWall(int Index)
        {
            if(!StartMovingWall)
            {
                for (int i = 0; i < WallObject.Length; i++)
                {
                    WallObject[i].SetActive(false);
                }

                for (int i = 0; i < WallObjectStorage.Length; i++)
                {
                    WallObjectStorage[i].SetActive(false);
                }

                WallObjectStorage[Index].SetActive(true);
            }       
        }

    #endregion

    #region BreakingMode

    public void ArraysMode()
    {
        if (StartMovingWall)
        {            
            if (WallObject[CurrentWall].transform.position.z >= 150)
            {
                //Debug.Log("Start Moving Wall");
                WallObject[CurrentWall].gameObject.SetActive(true);
                WallObject[CurrentWall].transform.position = Vector3.Lerp(WallObject[CurrentWall].transform.position, new Vector3(0, 0, 150), 0.5f * Time.deltaTime);
               // WallObject[CurrentWall].transform.position = Vector3.MoveTowards(WallObject[CurrentWall].transform.position, new Vector3(0, 0, 150), 15);

                if (WallObject[CurrentWall].transform.position.z <= 160)
                {
                    StartMovingWall = false;

                    HitboxChecker[CurrentWall].gameObject.SetActive(true);
                    coroutine = DelayStopCheckingCollsion();
                    StartCoroutine(coroutine);
                }
            }
        }
    }    

    #endregion
    public void OnebyOne()
    {
        if (StartMovingWall)
        {
            if (WallObject[CurrentWall].transform.position.z >= 150)
            {
                Debug.Log("Start Moving Wall");
                WallObject[CurrentWall].gameObject.SetActive(true);
                //WallObject[CurrentWall].transform.position = Vector3.Lerp(WallObject[CurrentWall].transform.position, new Vector3(0, 0, 150), 1 * Time.deltaTime);  (Lerp) - diff acc.
                WallObject[CurrentWall].transform.position = Vector3.MoveTowards(WallObject[CurrentWall].transform.position, new Vector3(0, 0, 150), 15); //Movetoward - same acc.
                if (WallObject[CurrentWall].transform.position.z <= 160)
                {
                    StartMovingWall = false;

                    HitboxChecker[CurrentWall].gameObject.SetActive(true);
                    coroutine = DelayStopCheckingCollsion();
                    StartCoroutine(coroutine);
                }
            }
        }
    }
    public void CheckHit()
    {
        if(GameMode != 2)
        {
            if (!Hit) //In case player not Hit the wall
            {               
                Score += 1;
            }
            else if (Hit) //In case player Hit the wall
            {
                Hitpoint -= 1;
                HitSFX.Play();
                Hit = false;
            }
        }       

        if (GameMode == 2 && CurrentWall % 2 != 0)
        {
            if (!Hit) //In case player not Hit the wall
            {               
                Score += 1;
                if (CurrentWall == 6)
                {                    
                    Score = 0;
                }
            }
            else if (Hit) //In case player Hit the wall
            {
                Hitpoint -= 1;
                HitSFX.Play();               
                if (GameMode == 2)
                {
                    if (CurrentWall <= 6) HitpointB -= 1; // A play B set
                    if (CurrentWall > 6) HitpointA -= 1; // B set A play
                }
                Hit = false;
            }
        }                       
        
        if(GameMode == 2 && CurrentWall < WallObject.Length)
        {             
           if(CurrentWall % 2 == 0) WallObject[CurrentWall + 1].GetComponent<ArrayWallController>().Hits = WallObject[CurrentWall].GetComponent<ArrayWallController>().Hits;
        }
    }

    void CalculatedScore(int Player)
    {
        if(Player == 0)
        {
            switch (Hitpoint)
            {
                case 3:
                    PlayerAScore = Score;
                    break;
                case 2:
                    PlayerAScore = Score / 2;
                    break;
                case 1:
                    PlayerAScore = Score / 4;
                    break;
                case 0:
                    PlayerAScore = 0;
                    break;
            }
        }
        else if (Player == 1)
        {
            switch (Hitpoint)
            {
                case 3:
                    PlayerBScore = Score;
                    break;
                case 2:
                    PlayerBScore = Score / 2;
                    break;
                case 1:
                    PlayerBScore = Score / 4;
                    break;
                case 0:
                    PlayerBScore = 0;
                    break;
            }
        }

    }

    private IEnumerator DelayStopCheckingCollsion()
    {
        if (GameMode == 0) yield return new WaitForSeconds(1);
        if (GameMode == 1) yield return new WaitForSeconds(0.15f);
        if (GameMode == 2)
        {
            if(CurrentWall % 2 == 0) yield return new WaitForSeconds(3f); // For Create wall mode
            else yield return new WaitForSeconds(3f); // For Create wall mode
        }
            

        HitboxChecker[CurrentWall].gameObject.SetActive(false);
        WallObject[CurrentWall].gameObject.SetActive(false);

        if (GameMode == 0) WallObject[CurrentWall].transform.position = new Vector3(0f, 0f, 2500f);

        CheckHit();
        CurrentWall += 1;

        if (CurrentWall < WallObject.Length) //If not end seq yet
        {
            if (GameMode == 0) yield return new WaitForSeconds(1);
            if (GameMode == 1) yield return new WaitForSeconds(0.15f); // For Competitive Mode


            if (GameMode == 2)
            {     
                if(CurrentWall <= 6) //Blue Setup
                {                            
                    PlayerBScore = Score;
                    if (CurrentWall == 6) Score = 0;
                }
                if(CurrentWall >= 7) //Red Setup
                {                                 
                    PlayerAScore = Score;                    
                }               

                if(CurrentWall % 2 == 0) PlayerTurn += 1;

                yield return new WaitForSeconds(1f);// For Create wall mode
            }               

            StartMovingWall = true;
        }
        else if (CurrentWall >= WallObject.Length) //End Wall sequence 
        {
            if (GameMode == 0)
            {
                EndText.gameObject.SetActive(true);
                IsEnd = true;               
            }
            if (GameMode == 1)
            {
                WallGroup.transform.position = new Vector3(0f, 0f, 0f);
                startTime = Time.time;
                journeyLength = Vector3.Distance(WallGroup.transform.position, new Vector3(0, 0, -7347));

                if (GameMode == 1) // For Competitive Mode Ending
                {
                    CalculatedScore(PlayerTurn);
                    PlayerTurn += 1;
                    PlayerText.color = Color.red;
                    PlayerText.text = "Player 2";
                    Hitpoint = 3;
                    CurrentWall = 0;
                    Score = 0;
                   
                    for (int i = 0; i < WallObject.Length; i++)
                    {
                        WallObject[i].SetActive(true);
                    }
                    if (PlayerTurn == 1) StartCoroutine(DelayStart());
                }
                if (PlayerTurn > 1)
                {
                    IsEnd = true;
                    if (TC != null) TC.SetResult();
                    else this.GetComponent<TextController>().SetResult();
                }                    
            }
            if (GameMode == 2)
            {
                IsEnd = true;
            }
        }
    }

    IEnumerator DelayStart()
    {
        CounterText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);        
        CounterText.text = "5";
        yield return new WaitForSeconds(1);
        CounterText.text = "4";
        yield return new WaitForSeconds(1);
        CounterText.text = "3";
        yield return new WaitForSeconds(1);
        CounterText.text = "2";
        yield return new WaitForSeconds(1);
        CounterText.text = "1";
        yield return new WaitForSeconds(1);
        CounterText.gameObject.SetActive(false);
        StartMovingWall = true;
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BacktoMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void StartGame()
    {
        StartDebug = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        //StartMovingWall = true;
        //if(!DuringSelection) StartCoroutine(DelayStart());

        if(GameMode == 0)
        {
            for (int i = 0; i < WallObject.Length; i++)
            {
                //Debug.Log("Wall " + i + " : "  + WallObject[0].transform.position.x);
                WallObject[i].transform.position = new Vector3(0, 0, 2500);
            }
        }   
        else if(GameMode == 1)
        {
            int initDistant = 1500;
            startTime = Time.time;
            journeyLength = Vector3.Distance(WallGroup.transform.position, new Vector3(0, 0, -7347));

            for (int i = 0; i < WallObject.Length; i++)
            {
                initDistant += 1000;
                WallObject[i].transform.position = new Vector3(0, 0, initDistant);
            }
        }
    }



    // Update is called once per frame
    void Update()
    {               
        if(StartMovingWall)
        {           
            switch(GameMode)
            {
                case 0: OnebyOne();
                        break;
                case 1: Continue();
                        break;
                case 2: ArraysMode();
                        break;
            }
        }

        if (ScoreText) ScoreText.text = "Score: " + Score;
        if (IsEnd)
        {
            EndText.gameObject.SetActive(true);
            ResetTheOrder();
            if (GameMode == 2)
            {
                TC.SetResult();
                TC.ScorePanal.SetActive(true);
                if (PlayerAScore > PlayerBScore) TC.WinnerText.text = "Player 1";
                else if (PlayerAScore < PlayerBScore) TC.WinnerText.text = "Player 2";
                else TC.WinnerText.text = "Draw";

                TC.ScoreATextResult.text = "Player 1 Score: " + PlayerAScore;
                TC.ScoreBTextResult.text = "Player 2 Score: " + PlayerBScore;
            }
        }
        else if (!IsEnd) EndText.gameObject.SetActive(false);

        if(StartDebug)
        {
            StartCoroutine(DelayStart());
            StartDebug = false;
        }

        if(GameMode != 2)
        {
            switch (Hitpoint)
            {
                case 3:
                    HitpointBar[0].SetActive(true);
                    HitpointBar[1].SetActive(true);
                    HitpointBar[2].SetActive(true);
                    break;
                case 2:
                    HitpointBar[2].SetActive(false);
                    break;
                case 1:
                    HitpointBar[1].SetActive(false);
                    break;
                case 0:
                    HitpointBar[0].SetActive(false);
                    if(DeadCam != null) DeadCam.SetBool("Is Dead", true);
                    if (RestartUI != null)
                    {
                        RestartUI.SetActive(true);
                        StartMovingWall = false;
                    } 
                    break;
            }
        }        

        if (GameMode == 2)
        {
           /* if (CurrentWall == 7)
            {
                //PlayerAScore = 0;
                //Score = 0;
            }*/
            if (CurrentWall <= 5)
            {
                if (CurrentWall % 2 == 0)
                {
                    PlayerText.color = Color.blue;
                    PlayerText.text = "Player 1 (Set)";
                }
                if (CurrentWall % 2 != 0)
                {
                    PlayerText.color = Color.red;
                    PlayerText.text = "Player 2 (Play)";
                }
            }

            if (CurrentWall >= 6)
            {
                if (CurrentWall % 2 == 0)
                {
                    PlayerText.color = Color.red;
                    PlayerText.text = "Player 2 (Set)";
                }
                if (CurrentWall % 2 != 0)
                {
                    PlayerText.color = Color.blue;
                    PlayerText.text = "Player 1 (Play)";
                }
            }
        }

        if (DuringSelection) OpenPoseManager.gameObject.SetActive(false);
        else if(!DuringSelection) OpenPoseManager.gameObject.SetActive(true);
    }
}
