using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DeckBuilderManager : MonoBehaviour
{

    public GameObject cardCollectionPrefab;
    public GameObject contentParent;
    public Material cardCollectionInstancePrefabMaterial;
    public Scrollbar verticalScrollbar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        GameObject deckCollectionObjects = GameObject.Find("DeckCollectionObjects");

        if (deckCollectionObjects) SceneManager.UnloadSceneAsync("DeckCollectionScene");

        LoadCollectionCards();
    }

    public void LoadCollectionCards()
    {
        GameObject gameData = GameObject.Find("GameData");
        if (gameData == null) return;

        List<List<Sprite>> dataSprites = gameData.GetComponent<GameData>().GetDataSpriteInfo();
        List<List<TextAsset>> dataInfo = gameData.GetComponent<GameData>().GetDataInfo();
        for(int i = 0; i < dataSprites.Count; i++)
        {
            for(int j = 0; j < dataSprites[i].Count; j++)
            {
                Sprite spriteInstance = dataSprites[i][j];
                TextAsset textInstance = dataInfo[i][j];

                string[] lines = textInstance.text.Split('\n');

                var cardCollectionInstance = Instantiate(cardCollectionPrefab, contentParent.transform);
                string[] nameData = lines[0].Split(' ');
                string name = nameData[1];
                cardCollectionInstance.name = name;
                cardCollectionInstance.GetComponentInChildren<TextMeshProUGUI>().text = name;

                cardCollectionInstance.GetComponent<Image>().sprite = spriteInstance;
                
                
            }
        }
        verticalScrollbar.value = 1.0f;


    }

    public void FilterByType()
    {

    }

    public void FilterByHP()
    {

    }

    public void FilterByRarity()
    {

    }

    public void CancelFilters()
    {

    }

    public void GoDeckCollection()
    {
        SceneManager.LoadScene("DeckCollectionScene",LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
