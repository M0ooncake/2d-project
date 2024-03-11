using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class CanvasManager : MonoBehaviour
{
    
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
    public TMP_Text volSliderText;

    [Header("Slider")]
    public Slider VolSlider;
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


        if (VolSlider)
        {
            VolSlider.onValueChanged.AddListener(onSliderValueChanged);
            if (volSliderText)
                volSliderText.text = VolSlider.value.ToString();
        }
        if (livesText)
        {
            GameManager.Instance.OnLivesChange.AddListener(UpdateLifeText);
            livesText.text = "Lives: " + GameManager.Instance.lives.ToString();
        }
    }

    void UpdateLifeText(int value)
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

    void onSliderValueChanged(float value)
    {
        volSliderText.text = value.ToString();

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
