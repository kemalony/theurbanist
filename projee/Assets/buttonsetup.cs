using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonsetup : MonoBehaviour
{
      public GameObject startcamera;
      public GameObject maincamera;
      public GameObject fpscamera;
      public GameObject startstuff;
      public GameObject gameuistuff;
      public GameObject taskpanel;
    // Function to deactivate the GameObject
    public void start()
    {
        startcamera.SetActive(false);  // Deactivates the GameObject
        maincamera.SetActive(true);
        startstuff.SetActive(false);
        gameuistuff.SetActive(true);
    }

    // Function to activate the GameObject
    public void fps()
    {
        maincamera.SetActive(false);  // Deactivates the GameObject
        fpscamera.SetActive(true);
        taskpanel.SetActive(false);

    }
}
