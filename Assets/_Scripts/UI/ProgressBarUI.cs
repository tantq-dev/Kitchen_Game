using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{

    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image barImage;
    private IHasProgess hasProgress;
    private void Start()
    {
        hasProgress = hasProgressGameObject.transform.GetComponent<IHasProgess>();
        if(hasProgress==null)
        {
            Debug.LogError("Game Objet" + hasProgressGameObject + "doesnt have a component implement IHasProgress");
        }
        hasProgress.OnProgressChange += HasProgress_OnProgressChange;

        barImage.fillAmount= 0;
        Hide();
    }

    private void HasProgress_OnProgressChange(object sender, IHasProgess.OnProgressChangeEventArgs e)
    {

        barImage.fillAmount = e.progressNormalized;
        if (e.progressNormalized == 0 || e.progressNormalized == 1)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
