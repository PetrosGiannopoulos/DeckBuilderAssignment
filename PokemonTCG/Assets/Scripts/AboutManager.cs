using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class AboutManager : MonoBehaviour
{
    VideoPlayer videoPlayer;
    public Image backgroundImage;
    public GameObject roleObj;
    public GameObject nameObj;
    public GameObject sepObj;

    private float roleLocalX;
    private float nameLocalX;
    // Start is called before the first frame update
    void Start()
    {
        roleLocalX = roleObj.transform.localPosition.x;
        nameLocalX = nameObj.transform.localPosition.x;
    }

    void Awake()
    {
        GameObject homeObjects = GameObject.Find("HomeObjects");
        if (homeObjects)
        {
            SceneManager.UnloadSceneAsync("HomeScene");
        }

        StartCoroutine(FadeOutAnimation(2f));

        InitPlayback();
    }

    IEnumerator FadeOutAnimation(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        roleObj.transform.DOLocalMoveX(-500,2);
        nameObj.transform.DOLocalMoveX(700, 2);

        Color color = sepObj.GetComponent<Image>().color;
        float alphaStep = 1f/255f;

        float duration = 2f;
        float timeStep = duration * alphaStep;

        for(int i = 0; i < 255; i++)
        {
            color.a -= alphaStep;
            sepObj.GetComponent<Image>().color = color;
            yield return new WaitForSeconds(timeStep);
        }
        
    }

    IEnumerator FadeInAnimation(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        roleObj.transform.DOLocalMoveX(roleLocalX, 2);
        nameObj.transform.DOLocalMoveX(nameLocalX, 2);

        Color color = sepObj.GetComponent<Image>().color;
        float alphaStep = 1f / 255f;

        float duration = 2f;
        float timeStep = duration * alphaStep;

        for (int i = 0; i < 255; i++)
        {
            color.a += alphaStep;
            sepObj.GetComponent<Image>().color = color;
            yield return new WaitForSeconds(timeStep);
        }

    }

    private void VideoPlayer_prepareCompleted(VideoPlayer source)
    {
        backgroundImage.material.color = new Color(1, 1, 1);
        backgroundImage.color = new Color(1, 1, 1);
        source.Play();

    }

    private void VideoPlayer_loopPointReached(VideoPlayer source)
    {

        StartCoroutine(FadeInAnimation(2f));
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
        //videoPlayer.isLooping = true;

        videoPlayer.prepareCompleted += VideoPlayer_prepareCompleted;
        videoPlayer.Prepare();

        backgroundImage.material.renderQueue = 3000;

        videoPlayer.loopPointReached += VideoPlayer_loopPointReached;
    }

    // Update is called once per frame
    void Update()
    {
        if (videoPlayer != null)
        {

            if (videoPlayer.texture != null && videoPlayer.isPlaying) backgroundImage.material.SetTexture("_BaseMap", videoPlayer.texture);
        }
    }

    public void GoHome()
    {
        SceneManager.LoadScene("HomeScene", LoadSceneMode.Additive);
    }

    private void OnApplicationQuit()
    {
        backgroundImage.material.color = new Color(0, 0, 0);
    }
}
