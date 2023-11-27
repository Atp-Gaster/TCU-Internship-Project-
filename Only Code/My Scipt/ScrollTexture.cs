using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTexture : MonoBehaviour
{
    public float Scrolly = 0.05f;
    // Start is called before the first frame update
  
    // Update is called once per frame
    void Update()
    {
        float OffsetY = Time.time * Scrolly;
        GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0, OffsetY);
    }
}
