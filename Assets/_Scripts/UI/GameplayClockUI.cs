using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayClockUI : MonoBehaviour
{
    [SerializeField] private Image clockImg;
    private void Update()
    {
        clockImg.fillAmount = GameManager.Instance.GetGameplayTimerNormalized();
    }
}
