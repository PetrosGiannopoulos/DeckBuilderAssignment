using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{

    List<List<TextAsset>> dataInfo = new List<List<TextAsset>>();
    List<List<Sprite>> dataSpriteInfo = new List<List<Sprite>>();
    List<List<string>> decksInfo = new List<List<string>>();
    List<string> deckId = new List<string>();

    string deckBuilderPath;
    List<string> deckBuilderInfo = new List<string>(); 
    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    public void AddDataInfo(List<TextAsset> dataInfo)
    {
        this.dataInfo.Add(dataInfo);
    }

    public void AddDataSpriteInfo(List<Sprite> dataTextureInfo)
    {
        this.dataSpriteInfo.Add(dataTextureInfo);
    }

    public void AddDeckInfo(string[] deckInfo, string id)
    {
        List<string> listDeckInfo = new List<string>();
        for (int i = 0; i < deckInfo.Length; i++) listDeckInfo.Add(deckInfo[i]);
        decksInfo.Add(listDeckInfo);
        deckId.Add(id);
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

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
