using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class TestColider : MonoBehaviour
{
    //public GameObject PointA;
    // Start is called before the first frame update
    public GameManagerScript GameManager;
    public ArrayWallController ArrayControll;
    public bool FoundHitbox = false;
    public bool IsArraysWall = false;
    public int ArraysIndex = -99; // If the normal will having -99 meanwhile the arrays of wall will have their index according to it position
    void Start()
    {
        //GameManager = GameObject.Find("GameManagerController").GetComponent<GameManagerScript>();
        string name = this.name;

        // Option 1: Using Regular Expressions
        if(ArrayControll != null)
        {
            Match match = Regex.Match(name, @"\d+");
            if (match.Success)
            {
                ArraysIndex = int.Parse(match.Value);
            }
        }     
        else ArraysIndex = -99;

        if(GameManager.CurrentWall % 2 == 0 && GameManager.GameMode == 2) changeMaking();
    }
    //call these function when change makingmode or not
    public void changeMaking() { 

        GetComponent<BoxCollider2D>().size = new Vector2(8,4);         
    }

    
    // Update is called once per frame


    void Update()
    {
        
        if(FoundHitbox)
        {
            FoundHitbox = false;
            if(IsArraysWall == false) GameManager.Hit = true;
            if (IsArraysWall == true && ArrayControll != null)
            {
                if(GameManager.CurrentWall % 2 != 0) GameManager.Hit = true;
                ArrayControll.RecievingValue(ArraysIndex, true);
                this.gameObject.SetActive(false);
            }                           
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {      
        if (collision.gameObject.tag == "Body")
        {

            //Debug.Log("Obejct enter in hitbox named: " + collision.name);
            FoundHitbox = true;
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.LogWarning("Obejct Stay in hitbox named: " + collision.name);
        //FoundHitbox = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Body")
        {

            //Debug.Log("Obejct enter in hitbox named: " + collision.name);
            FoundHitbox = false;
        }

    }
}
