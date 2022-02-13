using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

public class DeckBuilderManager : MonoBehaviour
{

    public GameObject editCardPrefab;
    public GameObject editContentParent;
    public GameObject cardCollectionPrefab;
    public GameObject contentParent;
    public Material cardCollectionInstancePrefabMaterial;
    public Scrollbar verticalScrollbar;

    List<GameObject> collectionCards = new List<GameObject>();
    bool toggleSortType = false;
    bool toggleSortHP = false;
    bool toggleSortRarity = false;
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
                if (nameData.Length > 2)
                {
                    for (int k = 2; k < nameData.Length; k++) name += " " + nameData[k];
                }
                cardCollectionInstance.name = name;
                cardCollectionInstance.GetComponentInChildren<TextMeshProUGUI>().text = name;

                cardCollectionInstance.GetComponent<Image>().sprite = spriteInstance;

                collectionCards.Add(cardCollectionInstance);
            }
        }
        verticalScrollbar.value = 1.0f;


    }

    public void FilterByType()
    {
        GameObject gameData = GameObject.Find("GameData");
        if (gameData == null) return;

        toggleSortType = !toggleSortType;
        foreach (GameObject go in collectionCards)Destroy(go);
        collectionCards.Clear();

        List<List<Sprite>> dataSprites = gameData.GetComponent<GameData>().GetDataSpriteInfo();
        List<List<TextAsset>> dataInfo = gameData.GetComponent<GameData>().GetDataInfo();

        List<Sprite> singleSpriteList = new List<Sprite>();
        List<TextAsset> singleTextAssetList = new List<TextAsset>();
        List<CollectionSortData> singleCollectionSortData = new List<CollectionSortData>();
        for (int i = 0; i < dataSprites.Count; i++)
        {
            for (int j = 0; j < dataSprites[i].Count; j++)
            {
                Sprite spriteInstance = dataSprites[i][j];
                TextAsset textInstance = dataInfo[i][j];
                singleSpriteList.Add(spriteInstance);
                singleTextAssetList.Add(textInstance);
                singleCollectionSortData.Add(new CollectionSortData(spriteInstance, textInstance));
            }
        }
        
        for (int i = 0; i < singleCollectionSortData.Count; i++)
        {
            Sprite spriteInstance = singleSpriteList[i];
            TextAsset textInstance = singleTextAssetList[i];
            

            string[] lines = textInstance.text.Split('\n');
            string[] nameData = lines[0].Split(' ');
            string name = nameData[1];
            if (nameData.Length > 2)
            {
                for (int k = 2; k < nameData.Length; k++) name += " " + nameData[k];
            }

            string[] typeData = lines[1].Split(' ');
            string type = typeData[1];

            string[] hpData = lines[2].Split(' ');
            //Debug.Log($"HPDAtaFormat: {hpData[1]}");
            /*if (hpData[1].Contains("None"))
            {
                Debug.Log($"HPDAtaFormat: {hpData[1][0]}, {hpData[1][4]}");
                
            }*/
            int hp = hpData[1].Contains("None")?0:int.Parse(hpData[1]);

            string[] rarityData = lines[3].Split(' ');
            string rarity = rarityData[1];

            singleCollectionSortData[i].name = name;
            singleCollectionSortData[i].type = type;
            singleCollectionSortData[i].HP = hp;
            singleCollectionSortData[i].rarity = GetRarityOrder(rarity);
        }

        if (toggleSortType)singleCollectionSortData.Sort((x, y) => x.type.CompareTo(y.type));
        else singleCollectionSortData.Sort((x, y) => y.type.CompareTo(x.type));
        //someList.Sort((x, y) => x.Value.Length.CompareTo(y.Value.Length));

        for (int i = 0; i < singleCollectionSortData.Count; i++)
        {
            var cardCollectionInstance = Instantiate(cardCollectionPrefab, contentParent.transform);
            
            cardCollectionInstance.name = singleCollectionSortData[i].name;
            cardCollectionInstance.GetComponentInChildren<TextMeshProUGUI>().text = singleCollectionSortData[i].name;

            cardCollectionInstance.GetComponent<Image>().sprite = singleCollectionSortData[i].sprite;

            collectionCards.Add(cardCollectionInstance);
        }
    }

    public void FilterByHP()
    {
        GameObject gameData = GameObject.Find("GameData");
        if (gameData == null) return;

        toggleSortHP = !toggleSortHP;
        foreach (GameObject go in collectionCards) Destroy(go);
        collectionCards.Clear();

        List<List<Sprite>> dataSprites = gameData.GetComponent<GameData>().GetDataSpriteInfo();
        List<List<TextAsset>> dataInfo = gameData.GetComponent<GameData>().GetDataInfo();

        List<Sprite> singleSpriteList = new List<Sprite>();
        List<TextAsset> singleTextAssetList = new List<TextAsset>();
        List<CollectionSortData> singleCollectionSortData = new List<CollectionSortData>();
        for (int i = 0; i < dataSprites.Count; i++)
        {
            for (int j = 0; j < dataSprites[i].Count; j++)
            {
                Sprite spriteInstance = dataSprites[i][j];
                TextAsset textInstance = dataInfo[i][j];
                singleSpriteList.Add(spriteInstance);
                singleTextAssetList.Add(textInstance);
                singleCollectionSortData.Add(new CollectionSortData(spriteInstance, textInstance));
            }
        }

        for (int i = 0; i < singleCollectionSortData.Count; i++)
        {
            Sprite spriteInstance = singleSpriteList[i];
            TextAsset textInstance = singleTextAssetList[i];


            string[] lines = textInstance.text.Split('\n');
            string[] nameData = lines[0].Split(' ');
            
            string name = nameData[1];
            if (nameData.Length > 2)
            {
                for(int k=2;k<nameData.Length;k++)name += " " + nameData[k];
            }

            string[] typeData = lines[1].Split(' ');
            string type = typeData[1];

            string[] hpData = lines[2].Split(' ');
            //Debug.Log($"HPDAtaFormat: {hpData[1]}");
            /*if (hpData[1].Contains("None"))
            {
                Debug.Log($"HPDAtaFormat: {hpData[1][0]}, {hpData[1][4]}");
                
            }*/
            int hp = hpData[1].Contains("None") ? 0 : int.Parse(hpData[1]);

            string[] rarityData = lines[3].Split(' ');
            string rarity = rarityData[1];

            singleCollectionSortData[i].name = name;
            singleCollectionSortData[i].type = type;
            singleCollectionSortData[i].HP = hp;
            singleCollectionSortData[i].rarity = GetRarityOrder(rarity);
        }

        if (toggleSortHP) singleCollectionSortData.Sort((x, y) => x.HP.CompareTo(y.HP));
        else singleCollectionSortData.Sort((x, y) => y.HP.CompareTo(x.HP));
        //someList.Sort((x, y) => x.Value.Length.CompareTo(y.Value.Length));

        for (int i = 0; i < singleCollectionSortData.Count; i++)
        {
            var cardCollectionInstance = Instantiate(cardCollectionPrefab, contentParent.transform);

            cardCollectionInstance.name = singleCollectionSortData[i].name;
            cardCollectionInstance.GetComponentInChildren<TextMeshProUGUI>().text = singleCollectionSortData[i].name;

            cardCollectionInstance.GetComponent<Image>().sprite = singleCollectionSortData[i].sprite;

            collectionCards.Add(cardCollectionInstance);
        }
    }

    public void FilterByRarity()
    {
        GameObject gameData = GameObject.Find("GameData");
        if (gameData == null) return;

        toggleSortRarity = !toggleSortRarity;
        foreach (GameObject go in collectionCards) Destroy(go);
        collectionCards.Clear();

        List<List<Sprite>> dataSprites = gameData.GetComponent<GameData>().GetDataSpriteInfo();
        List<List<TextAsset>> dataInfo = gameData.GetComponent<GameData>().GetDataInfo();

        List<Sprite> singleSpriteList = new List<Sprite>();
        List<TextAsset> singleTextAssetList = new List<TextAsset>();
        List<CollectionSortData> singleCollectionSortData = new List<CollectionSortData>();
        for (int i = 0; i < dataSprites.Count; i++)
        {
            for (int j = 0; j < dataSprites[i].Count; j++)
            {
                Sprite spriteInstance = dataSprites[i][j];
                TextAsset textInstance = dataInfo[i][j];
                singleSpriteList.Add(spriteInstance);
                singleTextAssetList.Add(textInstance);
                singleCollectionSortData.Add(new CollectionSortData(spriteInstance, textInstance));
            }
        }

        for (int i = 0; i < singleCollectionSortData.Count; i++)
        {
            Sprite spriteInstance = singleSpriteList[i];
            TextAsset textInstance = singleTextAssetList[i];


            string[] lines = textInstance.text.Split('\n');
            string[] nameData = lines[0].Split(' ');
            string name = nameData[1];

            if (nameData.Length > 2)
            {
                for (int k = 2; k < nameData.Length; k++) name += " " + nameData[k];
            }

            string[] typeData = lines[1].Split(' ');
            string type = typeData[1];

            string[] hpData = lines[2].Split(' ');
            //Debug.Log($"HPDAtaFormat: {hpData[1]}");
            /*if (hpData[1].Contains("None"))
            {
                Debug.Log($"HPDAtaFormat: {hpData[1][0]}, {hpData[1][4]}");
                
            }*/
            int hp = hpData[1].Contains("None") ? 0 : int.Parse(hpData[1]);

            string[] rarityData = lines[3].Split(' ');
            string rarity = rarityData[1];

            singleCollectionSortData[i].name = name;
            singleCollectionSortData[i].type = type;
            singleCollectionSortData[i].HP = hp;

            singleCollectionSortData[i].rarity = GetRarityOrder(rarity);
        }

        if (toggleSortRarity) singleCollectionSortData.Sort((x, y) => x.rarity.CompareTo(y.rarity));
        else singleCollectionSortData.Sort((x, y) => y.rarity.CompareTo(x.rarity));
        //someList.Sort((x, y) => x.Value.Length.CompareTo(y.Value.Length));

        for (int i = 0; i < singleCollectionSortData.Count; i++)
        {
            var cardCollectionInstance = Instantiate(cardCollectionPrefab, contentParent.transform);

            cardCollectionInstance.name = singleCollectionSortData[i].name;
            cardCollectionInstance.GetComponentInChildren<TextMeshProUGUI>().text = singleCollectionSortData[i].name;

            cardCollectionInstance.GetComponent<Image>().sprite = singleCollectionSortData[i].sprite;

            collectionCards.Add(cardCollectionInstance);
        }
    }

    public int GetRarityOrder(string rarity)
    {

        switch (rarity)
        {
            case "Promo":
                return 23;
            case "Common":
                return 1;
            case "Uncommon":
                return 2;
            case "Rare":
                return 3;
            case "Rare ACE":
                return 4;
            case "Rare BREAK":
                return 5;
            case "Rare Holo":
                return 6;
            case "Rare Holo EX":
                return 7;
            case "Rare Holo GX":
                return 8;
            case "Rare Holo LV.X":
                return 9;
            case "Rare Holo Star":
                return 10;
            case "Rare Holo V":
                return 11;
            case "Rare Holo VMAX":
                return 12;
            case "Rare Prime":
                return 13;
            case "Rare Prism Star":
                return 14;
            case "Rare Rainbow":
                return 15;
            case "Rare Secret":
                return 16;
            case "Rare Shining":
                return 17;
            case "Rare Shiny":
                return 18;
            case "Rare Shiny GX":
                return 19;
            case "Rare Ultra":
                return 20;
            case "Amazing Rare":
                return 21;
            case "LEGEND":
                return 22;
            default:
                break;
        }

        return 24;
        
    }
    public void CancelFilters()
    {

    }

    public void RemoveEditCard(GameObject editCardObj)
    {
        GameObject[] editCards = GameObject.FindGameObjectsWithTag("EditCard");
        foreach (GameObject go in editCards)
        {
            if (go.GetComponent<EditCardData>().GetName().Equals(editCardObj.GetComponent<EditCardData>().GetName()))
            {
                go.GetComponent<EditCardData>().SetCount(go.GetComponent<EditCardData>().GetCount()-1);
                if (go.GetComponent<EditCardData>().GetCount() <= 0)
                {
                    Destroy(go);
                    break;
                }
                go.GetComponent<EditCardData>().FitData();
            }
            
        }
    
    }

    public void AddEditCard(string name, Sprite image)
    {
        
        GameObject[] editCards = GameObject.FindGameObjectsWithTag("EditCard");

        bool found = false;
        foreach(GameObject go in editCards)
        {
            if (go.GetComponent<EditCardData>().GetName().Equals(name))
            {
                if (go.GetComponent<EditCardData>().GetCount()==4 && !go.GetComponent<EditCardData>().GetName().Contains("Energy")) return;

                go.GetComponent<EditCardData>().SetCount(go.GetComponent<EditCardData>().GetCount() + 1);
                go.GetComponent<EditCardData>().FitData();
                found = true;
                break;
            }
        }
        if (found == false)
        {
            var editCard = Instantiate(editCardPrefab, editContentParent.transform);
            editCard.name = name;
            editCard.GetComponent<EditCardData>().SetName(name);
            editCard.GetComponent<EditCardData>().SetCount(1);
            editCard.GetComponent<EditCardData>().SetImage(image);
            editCard.GetComponent<EditCardData>().FitData();
        }
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
