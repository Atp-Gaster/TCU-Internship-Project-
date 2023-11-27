using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlectionController : MonoBehaviour
{
    public Dropdown[] DropdownController;
    public GameObject MainCam;
    public GameObject SelectCam;
    public GameManagerScript GM;

    List<string> m_DropOptions = new List<string> {};
    private int[] previousValues;

    public void NextButton()
    {
        List<int> InputOrder = new List<int> { }; ;
        for (int i = 0; i < DropdownController.Length; i++)
        {
            Debug.Log(DropdownController[i].value);
            InputOrder.Add(DropdownController[i].value);
        }

        for (int j = 0; j < GM.WallObjectStorage.Length; j++)
        {
            GM.WallObjectStorage[j].SetActive(false);
        }


        GM.DuringSelection = false;
        GM.SetupWallOrder(InputOrder);
        SelectCam.SetActive(false);
        MainCam.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int j = 0; j < GM.WallObject.Length; j++)
        {
            m_DropOptions.Add(GM.WallObject[j].name);
            
        }
        for (int i = 0; i < DropdownController.Length; i++)
        {
            DropdownController[i].options.Clear();
            DropdownController[i].AddOptions(m_DropOptions);
        }

        previousValues = new int[DropdownController.Length];

        // Store the initial values of the dropdowns
        for (int i = 0; i < DropdownController.Length; i++)
        {
            previousValues[i] = DropdownController[i].value;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if any dropdown value has changed
        for (int i = 0; i < DropdownController.Length; i++)
        {
            if (DropdownController[i].value != previousValues[i])
            {
                Debug.Log("Dropdown " + i + " value changed to: " + DropdownController[i].value);
                GM.ShowWall(DropdownController[i].value);

                // Update the previous value to the new value
                previousValues[i] = DropdownController[i].value;
            }
        }
    }
}
