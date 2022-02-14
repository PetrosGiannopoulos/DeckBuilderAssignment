using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{

    List<List<TextAsset>> dataInfo = new List<List<TextAsset>>();
    List<List<Sprite>> dataSpriteInfo = new List<List<Sprite>>();

    List<TextAsset> textInfo = new List<TextAsset>();
    List<Sprite> spriteInfo = new List<Sprite>();

    List<List<string>> decksInfo = new List<List<string>>();
    List<string> deckId = new List<string>();

    string deckBuilderPath;
    List<string> deckBuilderInfo = new List<string>();
    string builderId;
    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    public void AddTextInfo(List<TextAsset> textInfo)
    {
        this.textInfo = textInfo;
    }

    public void AddTextInfo(TextAsset[] textInfo)
    {
        List<TextAsset> tempList = new List<TextAsset>();
        foreach (TextAsset t in textInfo) tempList.Add(t);

        this.textInfo = tempList;
    }

    public void AddSpriteInfo(List<Sprite> spriteInfo)
    {
        this.spriteInfo = spriteInfo;
    }

    public void AddSpriteInfo(Sprite[] spriteInfo)
    {
        List<Sprite> tempList = new List<Sprite>();
        foreach (Sprite s in spriteInfo) tempList.Add(s);

        this.spriteInfo = tempList;
    }

    public void AddDataInfo(List<TextAsset> dataInfo)
    {
        this.dataInfo.Add(dataInfo);
    }

    public void AddDataInfo(TextAsset[] dataInfo)
    {
        List<TextAsset> tempList = new List<TextAsset>();
        foreach (TextAsset t in dataInfo) tempList.Add(t);

        this.dataInfo.Add(tempList);
    }

    public void AddDataSpriteInfo(List<Sprite> dataTextureInfo)
    {
        this.dataSpriteInfo.Add(dataTextureInfo);
    }
    public void AddDataSpriteInfo(Sprite[] dataTextureInfo)
    {

        List<Sprite> tempList = new List<Sprite>();
        foreach(Sprite s in dataTextureInfo) tempList.Add(s);

        this.dataSpriteInfo.Add(tempList);
    }


    public void AddDeckInfo(string[] deckInfo, string id)
    {
        List<string> listDeckInfo = new List<string>();
        for (int i = 0; i < deckInfo.Length; i++) listDeckInfo.Add(deckInfo[i].Replace("\r", "").Replace("\n", ""));
        decksInfo.Add(listDeckInfo);
        deckId.Add(id);
    }

    public void ClearDeckInfo()
    {
        decksInfo.Clear();
        deckId.Clear();
    }

    public void SetDeckInfo(List<string> deckInfo, string id)
    {
        for(int i = 0; i < decksInfo.Count; i++)
        {
            if (deckId[i].Equals(id))
            {
                decksInfo[i] = deckInfo;
            }
        }
    }

    public void RemoveDeckInfo(string id)
    {
        int counter = 0;
        foreach(string dId in deckId)
        {
            if(dId.Equals(id))
            {
                decksInfo.RemoveAt(counter);
                
                deckId.RemoveAt(counter);
                
                return;
            }
            counter++;
        }
       
    }

    public void SetBuildId(string id)
    {
        this.builderId = id;
    }

    

    public void SetBuilderInfo(List<string> builderInfo)
    {
        this.deckBuilderInfo = builderInfo;
    }

    public void SetDeckBuilderPath(string deckBuilderPath)
    {
        this.deckBuilderPath = deckBuilderPath;
    }

    public List<string> GetBuilderInfo()
    {
        return this.deckBuilderInfo;
    }

    public string GetBuilderPath()
    {
        return this.deckBuilderPath;
    }
    public List<string> GetDeckInfo(int i)
    {
        return decksInfo[i];
    }

   

    public string GetBuilderId()
    {
        return this.builderId;
    }

    public void UnloadDecks()
    {
        decksInfo.Clear();
    }

    public List<List<TextAsset>> GetDataInfo()
    {
        return dataInfo;
    }

    public List<List<Sprite>> GetDataSpriteInfo()
    {
        return dataSpriteInfo;
    }

    public List<TextAsset> GetTextInfo()
    {
        return this.textInfo;
    }

    public List<Sprite> GetSpriteInfo()
    {
        return this.spriteInfo;
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
