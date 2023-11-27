using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class ArrayWallController : MonoBehaviour
{
    public GameManagerScript GameManager;
    public List<GameObject> HitArrays = new List<GameObject> { };
    public List<int> Hits = new List<int> { };
    public GameObject[] HitboxChecker;
    public GameObject[] WallObject;
    public bool StartCreate = false;

    public void RecievingValue(int Index, bool Hit)
    {
        //Debug.Log("Index :" + Index + ", " + Hit);
        Hits.Add(Index);
    }

    public void CreateNewWall()
    {      
        for(int A = 0; A < Hits.Count; A++)
        {
            for (int i = 0; i < HitboxChecker.Length; i++)
            {
                string name = HitboxChecker[i].name;                    
                   
                Match match = Regex.Match(name, @"\d+");
                if (match.Success)
                {
                    if (Hits[A] == i)
                    {
                        if (GameManager.CurrentWall % 2 != 0)
                        {
                            HitboxChecker[i].SetActive(false);
                            WallObject[i].SetActive(false);
                        }                        
                    }                       
                }                                                          
            }
        }                  
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (StartCreate)
        {
            CreateNewWall();
        }
    }
}
