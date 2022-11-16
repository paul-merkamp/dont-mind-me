using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinSpace : MonoBehaviour
{
    void Update()
    {
        if (transform.Find("GroundJar").gameObject.activeSelf == true)
        {
            SceneManager.LoadScene("WinScreen");
        }
    }
}
