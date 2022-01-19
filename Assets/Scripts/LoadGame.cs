using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{
    public Animator transAnimator;
    public AudioClip buttonSound;
    AudioSource audioSource;
    bool pressedButton = false;
    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }
    private void Update()
    {
        if (transAnimator.GetCurrentAnimatorStateInfo(0).IsName("TransOut"))
        {
            if (transAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                SceneManager.LoadScene("MainGame");
            }
        }
    }
    public void LoadMainGame()
    {
        audioSource.PlayOneShot(buttonSound, 1f);
        if (pressedButton)
            return;
        transAnimator.SetBool("SceneEnded", true);
        pressedButton = true;
    }
}
