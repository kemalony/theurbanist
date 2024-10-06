using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonsetup : MonoBehaviour
{
    public GameObject startcamera;
    public GameObject maincamera;
    public GameObject fpscamera;
    public GameObject startstuff;
    public GameObject gameuistuff;
    public GameObject taskpanel;
    public GameObject stats;
    public Text stattext;

    private Slider Score;
    private Slider Memnun;
    private Slider Para;
    private Slider Some;

    // Function to deactivate the GameObject
    public void start()
    {
        startcamera.SetActive(false);  // Deactivates the GameObject
        maincamera.SetActive(true);
        startstuff.SetActive(false);
        gameuistuff.SetActive(true);

        // Find the sliders in the scene by their names
        Score = GameObject.Find("Score").GetComponent<Slider>();
        Memnun = GameObject.Find("Memnun").GetComponent<Slider>();
        Para = GameObject.Find("Para").GetComponent<Slider>();
        Some = GameObject.Find("Dome").GetComponent<Slider>();

        // Load saved slider values from PlayerPrefs
        LoadSliderValues();
    }

    // Function to activate the GameObject
    public void taskk()
    {
        taskpanel.SetActive(true);
    }

    public void closetask()
    {
        taskpanel.SetActive(false);
    }

    public void openstat()
    {
        stats.SetActive(true);
    }

    public void closestat()
    {
        stats.SetActive(false);
    }

    private void LoadSliderValues()
    {
        // Check if the keys exist before loading
        if (PlayerPrefs.HasKey("Score")) Score.value = PlayerPrefs.GetFloat("Score");
        if (PlayerPrefs.HasKey("Memnun")) Memnun.value = PlayerPrefs.GetFloat("Memnun");
        if (PlayerPrefs.HasKey("Para")) Para.value = PlayerPrefs.GetFloat("Para");
        if (PlayerPrefs.HasKey("Some")) Some.value = PlayerPrefs.GetFloat("Some");

        // Update the stat text with the loaded values
        UpdateStatText();
    }

    private void UpdateStatText()
    {
        // Format the slider values into a string and set it to the stattext
        stattext.text = $"Score: {Score.value}\n" +
                        $"Memnun: {Memnun.value}\n" +
                        $"Para: {Para.value}\n" +
                        $"Some: {Some.value}";
    }
}
