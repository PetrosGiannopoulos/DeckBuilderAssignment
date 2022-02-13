using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingManager : MonoBehaviour
{

    public TextMeshProUGUI loadingText;
    public GameData gameData;
    // Start is called before the first frame update
    void Start()
    {

        //Load cards
        StartCoroutine(LoadCards());
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
                TextAsset textAsset = Resources.Load<TextAsset>("PokemonSmallDataCompressed/"+setFolderName+"/"+setsTextInfo[i][j].Name.Replace(".txt",""));
                //Debug.Log($"Data Info: {textAsset.text}");
                setDataInfo.Add(textAsset);
                Sprite spriteAsset = Resources.Load<Sprite>("PokemonSmallDataCompressed/" + setFolderName + "/" + setsTextInfo[i][j].Name.Replace(".txt", ""));
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
        
    }
}
