using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshPro countDownText;
    [SerializeField] [Range(0, 60)] private float minutes, seconds;
    [SerializeField] private Animator ventsAnimator;
    [SerializeField] private AudioSource announcer, beeper, music;
    [SerializeField] private AudioClip shutdownActivatedClip, endMusicClip;
    private float timer;
    private int previousTime;

    private void Start()
    {
        StartCoroutine(Countdown());
        InvokeRepeating(nameof(BeepSound), 1, 1);
    }

    IEnumerator Countdown()
    {
        timer = seconds + (minutes * 60);

        while(timer > 0)
        {
            timer -= Time.deltaTime;
            ConvertAndShowToText();
            yield return null;
        }

        ventsAnimator.SetTrigger("Activate");
        announcer.PlayOneShot(shutdownActivatedClip);
        music.Stop();
        Monster.Instance().DefeatMonster();
        CancelInvoke();

        yield return new WaitForSeconds(7);

        announcer.loop = true;
        announcer.PlayOneShot(endMusicClip);
        PlayerController.Instance().enabled = false;
        GameOverCanvas.Instance().GameWon();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void ConvertAndShowToText()
    {
        int seconds = (int)timer % 60;
        int minutes = (int)timer / 60;
        countDownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void BeepSound()
    {
        beeper.Play();
    }
}
