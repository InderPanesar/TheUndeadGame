using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SettingsScript : MonoBehaviour
{
    public Button submitButton;
    public Button resolutionDownButton;
    public Button resolutionUpButton;
    public Button windowedDownButton;
    public Button windowedUpButton;
    public Button backButton;

    public Text resolutionText;
    public Text windowedText;

    public Slider volumeSlider;

    private int resolutionIndex = 0;

    private string[] values = new string[] { "Windowed", "Fullscreen" };
    private bool isWindowed = true;


    // Start is called before the first frame update
    void Start()
    {
        Resolution[] resolutions = Screen.resolutions;
        volumeSlider.value = AudioListener.volume;
        volumeSlider.onValueChanged.AddListener(delegate { UpdateVol(); });

        Resolution resolution = Screen.currentResolution;
        resolutionText.text = resolution.width + " X " + resolution.height;
        Debug.Log(resolutionText.text);


        // Print the resolutions
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].height == resolution.height && resolutions[i].width == resolution.width)
            {
                resolutionIndex = i;
                break;
            }
        }

        resolutionUpButton.onClick.AddListener(delegate { ResolutionButtonUpClick(); });
        resolutionDownButton.onClick.AddListener((delegate { ResolutionButtonDownClick(); }));
        windowedDownButton.onClick.AddListener((delegate { WindowDownButtonClick(); }));
        windowedUpButton.onClick.AddListener((delegate { WindowUpButtonClick(); }));
        backButton.onClick.AddListener((delegate { BackButtonClick(); }));

        FullScreenMode mode = Screen.fullScreenMode;
        if (mode != FullScreenMode.Windowed)
        {
            isWindowed = false;
        }
        setWindowedModeText();

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ResolutionButtonUpClick()
    {
        Resolution[] resolutions = Screen.resolutions;
        resolutionIndex++;
        resolutionText.text = resolutions[resolutionIndex].width + " X " + resolutions[resolutionIndex].height;
        Screen.SetResolution(resolutions[resolutionIndex].height, resolutions[resolutionIndex].width, true);
        if(resolutionIndex >= (resolutions.Length-1)) {
            resolutionUpButton.interactable = false;
        }
        if (resolutionIndex > 0)
        {
            resolutionDownButton.interactable = true;
        }

    }

    public void ResolutionButtonDownClick()
    {
        Resolution[] resolutions = Screen.resolutions;
        resolutionIndex--;
        resolutionText.text = resolutions[resolutionIndex].width + " X " + resolutions[resolutionIndex].height;
        Screen.SetResolution(resolutions[resolutionIndex].height, resolutions[resolutionIndex].width, true);
        if (resolutionIndex <= 0)
        {
            resolutionDownButton.interactable = false;
        }
        if (resolutionIndex < (resolutions.Length - 1))
        {
            resolutionUpButton.interactable = true;
        }

    }

    public void WindowDownButtonClick()
    {
        isWindowed = !isWindowed;
        setWindowedModeText();
    }

    public void WindowUpButtonClick()
    {
        isWindowed = !isWindowed;
        setWindowedModeText();
    }

    private void setWindowedModeText()
    {
        if(isWindowed)
        {
            windowedText.text = values[0];
        }
        else
        {
            windowedText.text = values[1];
        }
    }

    public void UpdateVol()
    {
        float newVol = AudioListener.volume;
        newVol = volumeSlider.value;
        AudioListener.volume = newVol;
    }

    void BackButtonClick()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
