using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingManager : MonoBehaviour
{

    VideoPlayer videoPlayer;
    public Image background;
    public TextMeshProUGUI loadingText;
    public GameData gameData;
    // Start is called before the first frame update
    void Start()
    {
        InitPlayback();

        //Load cards
        StartCoroutine(LoadCards());
    }

    private void VideoPlayer_prepareCompleted(VideoPlayer source)
    {
        background.material.color = new Color(1, 1, 1);
        background.color = new Color(1, 1, 1);
        source.Play();

    }

    public void StopPlayback()
    {
        videoPlayer.Stop();
    }

    public void InitPlayback()
    {
        background.material.color = new Color(0, 0, 0);
        videoPlayer = background.GetComponent<VideoPlayer>();

        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        //videoPlayer.playOnAwake = false;
        videoPlayer.isLooping = true;

        videoPlayer.prepareCompleted += VideoPlayer_prepareCompleted;
        videoPlayer.Prepare();

        background.material.renderQueue = 3000;
    }

    IEnumerator LoadCards()
    {

        string assetsPath = Application.dataPath + "/Resources/PokemonSmallDataCompressed/";
        DirectoryInfo dInfo = new DirectoryInfo(assetsPath);
        List<List<FileInfo>> setsTextInfo = new List<List<FileInfo>>();
        List<string> setFolderInfo = new List<string>();
        DirectoryInfo[] setDirectories = dInfo.GetDirectories();

        loadingText.text = "Calculating Data Sum...";
        yield return new WaitForSeconds(1);
        int setNum = 0;
        foreach (DirectoryInfo setDirInfo in setDirectories)
        {
            setNum++;
            FileInfo[] setCardInfo = setDirInfo.GetFiles("*.txt", SearchOption.AllDirectories);
            List<FileInfo> setCardTextAssetsInfo = new List<FileInfo>();
            int setCardNum = 0;
            foreach (FileInfo setCardFileInfo in setCardInfo)
            {
                setCardNum++;
                setCardTextAssetsInfo.Add(setCardFileInfo);
                loadingText.text = "Calculating Data Sum...{"+setNum+"}"+":"+setCardNum+"/"+setCardInfo.Length;
                yield return new WaitForSeconds(0.02f);
            }
            setFolderInfo.Add(setDirInfo.Name);
            setsTextInfo.Add(setCardTextAssetsInfo);
        }

        loadingText.text = "Loading... Please Wait";
        yield return new WaitForSeconds(1);
        for (int i = 0; i < setsTextInfo.Count; i++)
        {
            string setFolderName = setFolderInfo[i];
            List<TextAsset> setDataInfo = new List<TextAsset>();
            List<Sprite> setSpriteDataInfo = new List<Sprite>();
            for (int j = 0; j < setsTextInfo[i].Count; j++)
            {
                
                //Debug.Log($"Path: {setsTextInfo[i][j].Name}");
                TextAsset textAsset = Resources.Load<TextAsset>("PokemonSmallDataCompressed/"+setFolderName+"/"+setsTextInfo[i][j].Name.Replace(".txt",""));
                //Debug.Log($"Data Info: {textAsset.text}");
                setDataInfo.Add(textAsset);
                Sprite spriteAsset = Resources.Load<Sprite>("PokemonSmallDataCompressed/" + setFolderName + "/" + setsTextInfo[i][j].Name.Replace(".txt", ""));
                setSpriteDataInfo.Add(spriteAsset);

                loadingText.text = "Precaching...{" + (i+1) + "}" + ":" + (j+1) + "/" + setsTextInfo[i].Count;
                yield return new WaitForSeconds(0.02f);
            }

            gameData.AddDataInfo(setDataInfo);
            gameData.AddDataSpriteInfo(setSpriteDataInfo);
        }

        yield return new WaitForSeconds(1f);
        loadingText.text = "";

        SceneManager.LoadScene("HomeScene", LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update()
    {
        if (videoPlayer != null)
        {

            if (videoPlayer.texture != null && videoPlayer.isPlaying) background.material.SetTexture("_BaseMap", videoPlayer.texture);
        }
    }

    private void OnApplicationQuit()
    {
        background.material.color = new Color(0, 0, 0);
    }
}
