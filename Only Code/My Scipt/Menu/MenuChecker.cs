using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuChecker : MonoBehaviour
{
    public bool FoundHitbox = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Body")
        {
            //Debug.Log("Obejct enter in hitbox named: " + collision.name);
            FoundHitbox = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
