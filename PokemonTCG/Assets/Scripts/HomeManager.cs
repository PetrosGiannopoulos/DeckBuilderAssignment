using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class HomeManager : MonoBehaviour
{
    VideoPlayer videoPlayer;
    public Image backgroundImage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        GameObject loadingScreenUI = GameObject.Find("LoadingObjects");
        if(loadingScreenUI)Destroy(loadingScreenUI);

        
        GameObject deckCollectionObjects = GameObject.Find("DeckCollectionObjects");
        if (deckCollectionObjects)
        {
            SceneManager.UnloadSceneAsync("DeckCollectionScene");
        }

        InitPlayback();
    }

    public void DeckBuilder()
    {
        SceneManager.LoadScene("DeckCollectionScene",LoadSceneMode.Additive);
    }

    public void About()
    {

    }

    public void PlayGame()
    {
        //could be done when finished/probably not..
    }

    public void HelpPage()
    {
        //Show instructions page
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    private void VideoPlayer_prepareCompleted(VideoPlayer source)
    {
        backgroundImage.material.color = new Color(1, 1, 1);
        backgroundImage.color = new Color(1, 1, 1);
        source.Play();

    }

    public void StopPlayback()
    {
        videoPlayer.Stop();
    }

    public void InitPlayback()
    {
        backgroundImage.material.color = new Color(0, 0, 0);
        videoPlayer = backgroundImage.GetComponent<VideoPlayer>();

        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        //videoPlayer.playOnAwake = false;
        videoPlayer.isLooping = true;

        videoPlayer.prepareCompleted += VideoPlayer_prepareCompleted;
        videoPlayer.Prepare();

        backgroundImage.material.renderQueue = 3000;
    }

    // Update is called once per frame
    void Update()
    {
        if (videoPlayer != null)
        {

            if (videoPlayer.texture != null && videoPlayer.isPlaying) backgroundImage.material.SetTexture("_BaseMap", videoPlayer.texture);
        }
    }

    private void OnApplicationQuit()
    {
        backgroundImage.material.color = new Color(0, 0, 0);
    }
}
