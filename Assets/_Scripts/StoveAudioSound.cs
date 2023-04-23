using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class StoveAudioSound : MonoBehaviour
{

    [SerializeField] private StoveCounter stoveCounter;
    private AudioSource audioSource;

    private float warningSoundTimer;
    private bool playWarningSound;

    private void Awake()
    {
        audioSource= GetComponent<AudioSource>();

    }
    private void Start()
    {


        stoveCounter.OnStateChange += StoveCounter_OnStateChange;
        stoveCounter.OnProgressChange += StoveCounter_OnProgressChange;
    }

    private void StoveCounter_OnProgressChange(object sender, IHasProgess.OnProgressChangeEventArgs e)
    {
        float burnShowProgressAmount = .5f;
        playWarningSound = e.progressNormalized >= burnShowProgressAmount && stoveCounter.IsFryied();

    }

    private void StoveCounter_OnStateChange(object sender, StoveCounter.OnStateChangeEventArgs e)
    {

        bool playSound = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fryied;
        if(playSound )
        {
            audioSource.Play();
            audioSource.volume = SoundManager.Instance.GetVolume();
        }
        else
        {
            audioSource.Pause();
        }

    }
    private void Update()
    {
        if (playWarningSound)
        {
            warningSoundTimer -= Time.deltaTime;
            if (warningSoundTimer <= 0)
            {
                float warningSoundTimerMax = .2f;
                warningSoundTimer = warningSoundTimerMax;
                SoundManager.Instance.PlayWaringSound(stoveCounter.transform.position);
            }

        }
    }
}
