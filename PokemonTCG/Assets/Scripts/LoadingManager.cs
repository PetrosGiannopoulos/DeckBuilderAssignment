using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.VFX;

public class LoadingManager : MonoBehaviour
{

    VideoPlayer videoPlayer;
    public Image backgroundImage;
    public GameObject loadingVFX;
    public TextMeshProUGUI loadingText;
    public GameData gameData;

    // Start is called before the first frame update
    void Start()
    {

        if(Application.platform == RuntimePlatform.Android)
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
            loadingVFX.SetActive(false);
            InitPlayback();
        }
        

        //Load cards
        //StartCoroutine(LoadCards());
        StartCoroutine(LoadCardsForBuild());
    }


    IEnumerator LoadCardsForBuild()
    {
        

        loadingText.text = "Loading... Please Wait";
        yield return new WaitForSeconds(1);
        TextAsset[] textAssets = Resources.LoadAll<TextAsset>("PokemonETC2SmallData/");
        Sprite[] sprites = Resources.LoadAll<Sprite>("PokemonETC2SmallData/");

        for (int i = 0; i < textAssets.Length; i++)
        {
            //A little bit of drama action to showcase that each file is loaded
            loadingText.text = "Precaching..." + (i + 1) + "/" + textAssets.Length;
            yield return new WaitForSeconds(0.005f);

        }

        gameData.AddTextInfo(textAssets);
        gameData.AddSpriteInfo(sprites);

        yield return new WaitForSeconds(1);

        loadingText.text = "";

        SceneManager.LoadScene("HomeScene", LoadSceneMode.Additive);
    }

    IEnumerator LoadCards()
    {

        
        string assetsPath = Application.dataPath + "/Resources/PokemonETC2SmallData/";
        DirectoryInfo dInfo = new DirectoryInfo(assetsPath);
        List<List<FileInfo>> setsTextInfo = new List<List<FileInfo>>();
        List<string> setFolderInfo = new List<string>();

        //Doesnt work for builds because Resources becomes a single file resources.asset
        //Use LoadCardsForBuild instead
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
                yield return new WaitForSeconds(0.005f);
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
                TextAsset textAsset = Resources.Load<TextAsset>("PokemonETC2SmallData/" + setFolderName+"/"+setsTextInfo[i][j].Name.Replace(".txt",""));
                //Debug.Log($"Data Info: {textAsset.text}");
                setDataInfo.Add(textAsset);
                Sprite spriteAsset = Resources.Load<Sprite>("PokemonETC2SmallData/" + setFolderName + "/" + setsTextInfo[i][j].Name.Replace(".txt", ""));
                setSpriteDataInfo.Add(spriteAsset);

                loadingText.text = "Precaching...{" + (i+1) + "}" + ":" + (j+1) + "/" + setsTextInfo[i].Count;
                yield return new WaitForSeconds(0.005f);
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

            if (videoPlayer.texture != null && videoPlayer.isPlaying) backgroundImage.material.SetTexture("_BaseMap", videoPlayer.texture);
        }
    }

    private void VideoPlayer_prepareCompleted(VideoPlayer source)
    {
        backgroundImage.material.color = new Color(1, 1, 1);
        backgroundImage.color = new Color(1, 1, 1);
        source.Play();

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

    private void OnApplicationQuit()
    {
        backgroundImage.material.color = new Color(0, 0, 0);
    }

}
