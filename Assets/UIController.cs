using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public GameObject joinOverlay;
    public GameObject hostOverlay;

    public void CancelJoin()
    {
        joinOverlay.SetActive(false);
    }

    public void DoJoin()
    {
        joinOverlay.SetActive(true);
        string host = PlayerPrefs.GetString("join address", "");
        int port = PlayerPrefs.GetInt("join port", -1);

        if (host != "")
        {
            string text = host + ":" + port;
            GameObject.Find("JoinInput").GetComponent<TMPro.TMP_InputField>().text = text;
        }
    }

    public void DoHost()
    {
        hostOverlay.SetActive(true);
    }

    public void SaveJoin(TMPro.TMP_InputField inputField)
    {
        string str_endpoint = inputField.text;
        string[] substrs = str_endpoint.Split(new char[] { ':' }, 2);
        string host = substrs[0];
        int port = -1;
        if(substrs.Length > 1)
        {
            int.TryParse(substrs[1], out port);
        }
        PlayerPrefs.SetString("join address", host);
        PlayerPrefs.SetInt("join port", port);
    }

    public void CancelHost()
    {
        hostOverlay.SetActive(false);
    }

    public void Cancel()
    {
        if (joinOverlay.activeSelf)
            CancelJoin();
        else if (hostOverlay.activeSelf)
            CancelHost();
        else
            DoQuit();
    }

    public void Join()
    {
        AsyncOperation asyncJoin = SceneManager.LoadSceneAsync("Scenes/Client", new LoadSceneParameters());
        asyncJoin.completed += AsyncJoin_completed;
    }

    private void AsyncJoin_completed(AsyncOperation obj)
    {
        Scene activeScene = SceneManager.GetActiveScene();
        GameObject[] roots = activeScene.GetRootGameObjects();
        foreach(GameObject root in roots)
        {
            StateHandler sh = root.GetComponent<StateHandler>();
            if (sh == null)
                continue;

            sh.remoteAddress = PlayerPrefs.GetString("join address");
            sh.remotePort = PlayerPrefs.GetInt("join port");
            sh.Setup();
        }
    }

    public void DoQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void Update()
    {
        if(Input.GetButtonUp("Cancel"))
        {
            DoQuit();
        }
    }
}
