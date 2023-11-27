using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] GameObject _menuPanel;
    [SerializeField] GameObject _loadPanel;
    [SerializeField] Image _loadBar;

    public void OnApplicationQuit()
    {
        Application.Quit();
    }

    public void GoTo(int scene)
    {
        _menuPanel.SetActive(false);
        _loadPanel.SetActive(true);
        StartCoroutine(LoadAsync(scene));
    }

    IEnumerator LoadAsync(int scene)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene);

        Stopwatch myStopwatch = new Stopwatch(); //IA2-P4
        myStopwatch.Start();

        while (!asyncOperation.isDone)
        {
            _loadBar.fillAmount = asyncOperation.progress;

            if (myStopwatch.ElapsedMilliseconds > TimeSlicing.elapsedFramesPerSecond)
            {
                yield return new WaitForEndOfFrame();
                myStopwatch.Restart();
            }
        }
    }
}
