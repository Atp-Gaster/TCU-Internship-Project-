using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuScript : MonoBehaviour
{
    int Index = 0;
    public Animator Anim;    
    public AudioSource SlideDoorFadeInSFX;
    public AudioSource SlideDoorFadeOutSFX;
    public AudioSource ClickSFX;
    public bool PlayOnce = false;
    // Start is called before the first frame update

    public void SinglePlayer()
    {
        SlideDoorFadeInSFX.Play();
        ClickSFX.Play();
        Anim.SetBool("Start", true);
        Invoke("PlayFadeIn", 2f);
        StartCoroutine(Animation());
        Index = 1;
    }

    public void MulitiPlayer()
    {
        SlideDoorFadeInSFX.Play();
        ClickSFX.Play();
        Invoke("PlayFadeIn",1f);
        Anim.SetBool("Start", true);
    }

    public void CompetitiveMode()
    {
        Index = 2;
        ClickSFX.Play();
        Anim.SetBool("Next", true);
        Invoke("PlayFadeOut", 0.2f);
        Invoke("ChangeScene", 1.5f);
    }

    public void CustomMode()
    {
        Index = 3;
        ClickSFX.Play();
        Anim.SetBool("Next", true);
        Invoke("PlayFadeOut", 0.2f);
        Invoke("ChangeScene", 1.5f);
    }

    public void ChallengeMode()
    {
        Index = 4;
        ClickSFX.Play();
        Anim.SetBool("Next", true);
        Invoke("PlayFadeOut", 0.2f);
        Invoke("ChangeScene", 1.5f);
    }

    void ChangeScene()
    {
        SceneManager.LoadScene(Index);
    }

    IEnumerator Animation()
    {        
        yield return new WaitForSeconds(3f);        
        SceneManager.LoadScene(1);
    }

    void PlayFadeIn()
    {
        SlideDoorFadeInSFX.Play();
    }

    void PlayFadeOut()
    {
        SlideDoorFadeOutSFX.Play();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
