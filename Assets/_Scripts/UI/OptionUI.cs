using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public static OptionUI Instance { get; private set; }
    [SerializeField] private Button soundFxButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button interactAltButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private TextMeshProUGUI soundText;
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private TextMeshProUGUI moveUp;
    [SerializeField] private TextMeshProUGUI moveDown;
    [SerializeField] private TextMeshProUGUI moveRight;
    [SerializeField] private TextMeshProUGUI moveLeft;
    [SerializeField] private TextMeshProUGUI interact;
    [SerializeField] private TextMeshProUGUI interactAlt;
    [SerializeField] private TextMeshProUGUI pause;
    [SerializeField] Transform pressToRebindKeyTransform;

    private void Awake()
    {
        Instance = this;
        soundFxButton.onClick.AddListener(()=>{
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        musicButton.onClick.AddListener(() => {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        closeButton.onClick.AddListener(() =>{
            Hide();

        });
        moveUpButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Move_Up);
        });
        moveRightButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Move_Right);
        });
        moveLeftButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Move_Left);
        });
        moveDownButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Move_Down);
        });
        interactButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Interact);
        });
        interactAltButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Interact_Alt);
        });
        pauseButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Pause);
        });
    }
    private void Start()
    {
        GameManager.Instance.OnGameUnpause += GameManager_OnGameUnpause;
        UpdateVisual();
        Hide();
        HidePressToRebindKey();
    }

    private void GameManager_OnGameUnpause(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void UpdateVisual()
    {
        soundText.text = "Sound Effects: "+Mathf.Round(SoundManager.Instance.GetVolume()*10f).ToString();
        musicText.text = "Music : "+Mathf.Round(MusicManager.Instance.GetVolume()*10f).ToString();
        moveDown.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        moveUp.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        moveRight.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        moveLeft.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        interact.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        interactAlt.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact_Alt);
        pause.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);

    }
    private void ShowPressToRebindKey()
    {
        pressToRebindKeyTransform.gameObject.SetActive(true);
    }
    private void HidePressToRebindKey()
    {
        pressToRebindKeyTransform.gameObject.SetActive(false);
    }
    private void RebindBinding(GameInput.Binding binding)
    {
        ShowPressToRebindKey();
        GameInput.Instance.Rebinding(binding,()=> {
            HidePressToRebindKey();
            UpdateVisual();
            }
        );
    }
}
