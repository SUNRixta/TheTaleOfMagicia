using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public Animator sceneTransition;
    public Animator battleTransition;

    public float delay = 1f;

    public string nextScene;
    public List<string> battleScenes;

    public bool audioFade;

    public static string PreviousLevel { get; private set; }
    
    private void Start()
    {
        Screen.SetResolution(960, 640, false);
        if (AudioListener.volume < 1)
        {
            AudioListener.volume = 1;
        }
    }

    private void OnDestroy()
    {
        PreviousLevel = gameObject.scene.name;
    }

    public void LoadNextScene()
    {
        StartCoroutine(LoadLevel(nextScene));
    }

    public void LoadBattleScene()
    {
        int randomBattle = Random.Range(0, battleScenes.Count);
        StartCoroutine(LoadBattle(battleScenes[randomBattle]));
    }


    public IEnumerator LoadLevel(string levelName)
    {
        sceneTransition.SetTrigger("Start");

        float elapsedTime = 0;
        float currentVolume = AudioListener.volume;

        if (audioFade == true)
        {
            while (elapsedTime < delay)
            {
                elapsedTime += Time.deltaTime;
                AudioListener.volume = Mathf.Lerp(currentVolume, 0, elapsedTime / delay);
                yield return null;
            }
            SceneManager.LoadScene(levelName);
        }
        else
        {
            SceneManager.LoadScene(levelName);
        }
    }

    public IEnumerator LoadBattle(string levelName)
    {
        battleTransition.SetTrigger("Start");

        float elapsedTime = 0;
        float currentVolume = AudioListener.volume;

        if (audioFade == true)
        {
            while (elapsedTime < delay)
            {
                elapsedTime += Time.deltaTime;
                AudioListener.volume = Mathf.Lerp(currentVolume, 0, elapsedTime / delay);
                yield return null;
            }
            SceneManager.LoadScene(levelName);
        }
        else
        {
            SceneManager.LoadScene(levelName);
        }
    }

    public void Exit()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
