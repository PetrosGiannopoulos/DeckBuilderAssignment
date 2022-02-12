using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{

    List<List<TextAsset>> dataInfo = new List<List<TextAsset>>();
    List<List<Texture>> dataTextureInfo = new List<List<Texture>>();
    // Start is called before the first frame update
    void Start()
    {
       
    }

    public void AddDataInfo(List<TextAsset> dataInfo)
    {
        this.dataInfo.Add(dataInfo);
    }

    public void AddDataTextureInfo(List<Texture> dataTextureInfo)
    {
        this.dataTextureInfo.Add(dataTextureInfo);
    }

    public List<List<TextAsset>> GetDataInfo()
    {
        return dataInfo;
    }

    public List<List<Texture>> GetDataTextureInfo()
    {
        return dataTextureInfo;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
