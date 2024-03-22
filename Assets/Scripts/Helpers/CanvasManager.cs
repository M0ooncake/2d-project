using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
public class CanvasManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    [Header("Button")]
    public Button playButton;
    public Button settingsButton;
    public Button quitButton;
    public Button pauseButton;
    public Button resumeButton;
    public Button resetButton;
    public Button returnToMenu;
    public Button backButton;

    [Header("Menus")]
    public GameObject mainMenu;
    public GameObject pauseMenu;
    public GameObject settingsMenu;


    [Header("Text")]
    public TMP_Text livesText;
    public TMP_Text masterVolSliderText;
    public TMP_Text musicVolSliderText;
    public TMP_Text sfxVolSliderText;
    [Header("Slider")]
    public Slider masterVolSlider;
    public Slider musicVolSlider;
    public Slider sfxVolSlider;
    // Start is called before the first frame update
    void Start()
    {
        if (quitButton)
            quitButton.onClick.AddListener(Quit);

        if (playButton)
            playButton.onClick.AddListener(() => GameManager.Instance.ChangeScene(1));

        if (settingsButton)
            settingsButton.onClick.AddListener(() => SetMenuActive(settingsMenu, mainMenu));

        if (backButton)
            backButton.onClick.AddListener(() => SetMenuActive(mainMenu, settingsMenu));

        if (resumeButton)
            resumeButton.onClick.AddListener(() => 
            { 
                SetMenuActive(null, pauseMenu); 
                Time.timeScale = 1.0f; 
            });

        if (returnToMenu)
        {
            Time.timeScale = 1.0f;
            returnToMenu.onClick.AddListener(() => SceneManager.LoadScene(0));
        }


        if (masterVolSlider)
        {
            masterVolSlider.onValueChanged.AddListener((value) => OnSliderValueChanged(value, masterVolSliderText, "Master"));
            float newValue;
            audioMixer.GetFloat("Master", out newValue);
            masterVolSlider.value = newValue + 80;
            if (masterVolSliderText)
                masterVolSliderText.text = masterVolSlider.value.ToString();
        }

        if (musicVolSlider)
        {
            musicVolSlider.onValueChanged.AddListener((value) => OnSliderValueChanged(value, musicVolSliderText, "Music"));
            float newValue;
            audioMixer.GetFloat("Master", out newValue);
            musicVolSlider.value = newValue + 80;
            if (musicVolSliderText)
                musicVolSliderText.text = musicVolSlider.value.ToString();
        }

        if (sfxVolSlider)
        {
            sfxVolSlider.onValueChanged.AddListener((value) => OnSliderValueChanged(value, sfxVolSliderText, "SFX"));
            float newValue;
            audioMixer.GetFloat("Master", out newValue);
            sfxVolSlider.value = newValue + 80;
            if (sfxVolSliderText)
                sfxVolSliderText.text = sfxVolSlider.value.ToString();
        }
        if (livesText)
        {
            GameManager.Instance.OnLivesChange.AddListener(UpdateLifeText);
            livesText.text = "Lives: " + GameManager.Instance.lives.ToString();
        }
    }

    public void UpdateLifeText(int value)
    {
        livesText.text = "Lives: " + value.ToString();
    }

   
    void SetMenuActive(GameObject menuToActivate, GameObject menuToDeactivate)
    {
        if (menuToActivate)
            menuToActivate.SetActive(true);

        if (menuToDeactivate)
            menuToDeactivate.SetActive(false);
    }

    void OnSliderValueChanged(float value, TMP_Text volSliderText, string sliderName)
    {
        volSliderText.text = value.ToString();
        audioMixer.SetFloat(sliderName, value - 80);
    }
    void Quit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    // Update is called once per frame
    void Update()
    {
        if (!pauseMenu) return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);

            //hints for the lab
            if (pauseMenu.activeSelf)
            {
                //do something to pause
                Time.timeScale = 0.0f;
                
            }
            else
            {
                Time.timeScale = 1.0f;
                
                //do something else
            }
        }
    }
}
