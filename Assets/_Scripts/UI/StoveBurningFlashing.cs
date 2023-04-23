using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurningFlashing : MonoBehaviour
{
    private const string FLASHING_ANIMATOR = "IsFlashing" ;
    [SerializeField] private StoveCounter stoveCounter;

    private Animator flashingAnimator;
    private void Awake()
    {

        flashingAnimator = GetComponent<Animator>();
    }
    private void Start()
    {
        stoveCounter.OnProgressChange += StoveCounter_OnProgressChange;
        Hide();
    }

    private void StoveCounter_OnProgressChange(object sender, IHasProgess.OnProgressChangeEventArgs e)
    {
        float burnShowProgressAmount = .5f;
        bool show = e.progressNormalized >= burnShowProgressAmount && stoveCounter.IsFryied();
        if (show)
        {
            Show();

        }
        else
        {
            Hide();
        }
    }


    private void Show()
    {
       flashingAnimator.SetBool(FLASHING_ANIMATOR, true);
    }
    private void Hide()
    {
        flashingAnimator.SetBool(FLASHING_ANIMATOR, false);

    }
}
