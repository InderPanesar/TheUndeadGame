using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Script to handle behaviour on Settings Page of Game.
/// </summary>
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

    private Resolution _resolution;

    void Start()
    {
        Resolution[] resolutions = Screen.resolutions.Where(resolution => resolution.refreshRate == 60).ToArray();
        volumeSlider.value = AudioListener.volume;
        volumeSlider.onValueChanged.AddListener(delegate { UpdateVol(); });

        resolutionText.text = Screen.width + " X " + Screen.height;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].height == Screen.height && resolutions[i].width == Screen.width)
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
        submitButton.onClick.AddListener((delegate { SubmitValues(); }));

        FullScreenMode mode = Screen.fullScreenMode;
        if (mode != FullScreenMode.Windowed)
        {
            isWindowed = false;
        }
        setWindowedModeText();

    }

    void Update()
    {
        if (resolutionIndex <= 0)
        {
            resolutionDownButton.interactable = false;
        }
    }

    /// <summary>
    /// Handles when Up Arrow is clicked for Window Resolution.
    /// </summary>
    public void ResolutionButtonUpClick()
    {
        Resolution[] resolutions = Screen.resolutions.Where(resolution => resolution.refreshRate == 60).ToArray();
        resolutionIndex++;
        _resolution = resolutions[resolutionIndex];
        resolutionText.text = resolutions[resolutionIndex].width + " X " + resolutions[resolutionIndex].height;
        if (resolutionIndex >= (resolutions.Length-1)) {
            resolutionUpButton.interactable = false;
        }
        if (resolutionIndex > 0)
        {
            resolutionDownButton.interactable = true;
        }

    }

    /// <summary>
    /// Handles when Down Arrow is clicked for Window Resolution.
    /// </summary>
    public void ResolutionButtonDownClick()
    {
        Resolution[] resolutions = Screen.resolutions.Where(resolution => resolution.refreshRate == 60).ToArray();
        resolutionIndex--;
        _resolution = resolutions[resolutionIndex];

        resolutionText.text = resolutions[resolutionIndex].width + " X " + resolutions[resolutionIndex].height;

        if (resolutionIndex < (resolutions.Length - 1))
        {
            resolutionUpButton.interactable = true;
        }

    }

    /// <summary>
    /// Handles when Down Arrow is clicked for Window type (Full Screen/Windowed).
    /// </summary>
    public void WindowDownButtonClick()
    {
        isWindowed = !isWindowed;
        setWindowedModeText();
    }

    /// <summary>
    /// Handles when Up Arrow is clicked for Window type (Full Screen/Windowed).
    /// </summary>
    public void WindowUpButtonClick()
    {
        isWindowed = !isWindowed;
        setWindowedModeText();
    }

    /// <summary>
    /// Handles the updating of the text for the window mode on the UI
    /// </summary>
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


    /// <summary>
    /// Updates the volume settings in the applicaiton. 
    /// </summary>
    public void UpdateVol()
    {
        float newVol = volumeSlider.value;
        AudioListener.volume = newVol;
        print(AudioListener.volume);
        PlayerPrefs.SetFloat("AudioVolume", newVol);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Handles when the user returns out of this menu.
    /// </summary>
    void BackButtonClick()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }

    /// <summary>
    /// Confirm and make the changes to the resolution / fullscreen-windowed.
    /// </summary>
    void SubmitValues()
    {
        Screen.SetResolution(_resolution.width, _resolution.height, !isWindowed);
        SceneManager.LoadSceneAsync("MainMenu");
    }
}

