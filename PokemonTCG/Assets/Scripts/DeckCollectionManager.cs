using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using UnityEditor;

public class DeckCollectionManager : MonoBehaviour
{

    private int selectedDeck;

    public Button createNewButton;
    public GameObject contentParent;
    public GameObject deckIconPrefab;

    List<GameObject> decks = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        
        GameObject homeObjects = GameObject.Find("HomeObjects");
        //Gets rid of unecessary scenes in hierarchy to avoid creating additively empty scene gameobjects 
        if(homeObjects) SceneManager.UnloadSceneAsync("HomeScene");

        //Load from disk
        LoadDecks();
    }

    public void LoadDecks()
    {
        //Debug.Log(Application.persistentDataPath);
        GameObject gameData = GameObject.Find("GameData");
        if (gameData==null) return;

        string saveFolderPath = Application.persistentDataPath + "/Decks";

        DirectoryInfo dInfo = new DirectoryInfo(saveFolderPath);
        if (!dInfo.Exists) dInfo.Create();
        FileInfo[] allDecksInfo = dInfo.GetFiles("*.txt", SearchOption.AllDirectories);

        if (allDecksInfo.Length == 0) return;
        int counter = 0;
        foreach(FileInfo fileInfo in allDecksInfo)
        {
            string[] lines = File.ReadAllLines(saveFolderPath + "/" + fileInfo.Name);
            gameData.GetComponent<GameData>().AddDeckInfo(lines);

            var deckObj = Instantiate(deckIconPrefab, contentParent.transform);
            deckObj.name = fileInfo.Name.Replace(".txt","");
            deckObj.GetComponentInChildren<TextMeshProUGUI>().text = deckObj.name;
            deckObj.GetComponent<DeckInstance>().SetDeckInfo(gameData.GetComponent<GameData>().GetDeckInfo(counter));

            counter++;
            decks.Add(deckObj);

        }
    }

    public void SaveDeck(GameObject deckIcon)
    {
        
        GameObject gameData = GameObject.Find("GameData");
        if (gameData == null) return;

        string saveFolderPath = Application.persistentDataPath + "/Decks";

        DirectoryInfo dInfo = new DirectoryInfo(saveFolderPath);
        if (!dInfo.Exists) dInfo.Create();

        string fullPath = saveFolderPath + "/" + deckIcon.GetComponentInChildren<TextMeshProUGUI>().text+".txt";
        if (!File.Exists(fullPath)) File.Create(fullPath);

        if (deckIcon.GetComponent<DeckInstance>().GetDeckInfo().Count == 0) return;
        File.WriteAllLines(fullPath,deckIcon.GetComponent<DeckInstance>().GetDeckInfo());
    }

    public void CreateNew()
    {
        //Button trigger
        StartCoroutine(CreateNewDeck());
        
    }

    IEnumerator CreateNewDeck()
    {
        createNewButton.interactable = false;

        var deckObj = Instantiate(deckIconPrefab,contentParent.transform);
        GameObject[] deckIcons = GameObject.FindGameObjectsWithTag("DeckIcon");
        int counter = 1;
        foreach (GameObject go in deckIcons) if (go.GetComponentInChildren<TextMeshProUGUI>().text.Contains("Empty")) counter++;

        deckObj.GetComponentInChildren<TextMeshProUGUI>().text = "Empty "+counter;

        SaveDeck(deckObj);

        yield return new WaitForSeconds(1);
        createNewButton.interactable = true;

    }

    public void Edit()
    {
        GameObject gameData = GameObject.Find("GameData");

        GameObject selectedDeck = GetSelectDeck();

        if (selectedDeck == null) return;
        gameData.GetComponent<GameData>().SetBuilderInfo(selectedDeck.GetComponent<DeckInstance>().GetDeckInfo());

        SceneManager.LoadScene("DeckBuilderScene", LoadSceneMode.Additive);

    }

    public GameObject GetSelectDeck()
    {

        GameObject[] deckIcons = GameObject.FindGameObjectsWithTag("DeckIcon");
        foreach(GameObject go in deckIcons)if (go.GetComponent<Outline>().enabled) return go;
        return null;

    }

    public void Import()
    {
        //Optional
    }

    public void Export()
    {
        //Optional
        string path = EditorUtility.SaveFilePanel("Select Path to Export Deck",Application.persistentDataPath+"/Decks", ".txt","txt");
        if (path.Length!=0)
        {
            if (!File.Exists(path)) File.Create(path);
            GameObject deckIcon = GetSelectDeck();
            if (deckIcon.GetComponent<DeckInstance>().GetDeckInfo().Count == 0) return;
            File.WriteAllLines(path, deckIcon.GetComponent<DeckInstance>().GetDeckInfo());
        }
        Debug.Log($"ExportPath: {path}");
    }

    public void Delete()
    {

    }

    public void GoHome()
    {
        SceneManager.LoadScene("HomeScene", LoadSceneMode.Additive);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
