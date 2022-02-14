using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using UnityEditor;
using SFB;

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

        GameObject deckBuilderObjects = GameObject.Find("DeckBuilderObjects");
        if (deckBuilderObjects) SceneManager.UnloadSceneAsync("DeckBuilderScene");

        if (Application.platform == RuntimePlatform.Android)
        {
            Destroy(GameObject.Find("ImportButton"));
            Destroy(GameObject.Find("ExportButton"));
        }

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
        gameData.GetComponent<GameData>().ClearDeckInfo();

        
        foreach(FileInfo fileInfo in allDecksInfo)
        {
            string fullPath = saveFolderPath + "/" + fileInfo.Name;

            string[] lines = File.ReadAllLines(fullPath.Replace("\r","").Replace("\n",""));
            gameData.GetComponent<GameData>().AddDeckInfo(lines,fileInfo.Name);

            var deckObj = Instantiate(deckIconPrefab, contentParent.transform);
            deckObj.name = fileInfo.Name.Replace(".txt","");
            deckObj.GetComponentInChildren<TextMeshProUGUI>().text = deckObj.name;
            deckObj.GetComponent<DeckInstance>().SetDeckInfo(gameData.GetComponent<GameData>().GetDeckInfo(counter));

            deckObj.GetComponent<DeckInstance>().id = fileInfo.Name;
            deckObj.GetComponent<DeckInstance>().path = saveFolderPath + "/" + fileInfo.Name;
            

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
        deckIcon.GetComponent<DeckInstance>().path = fullPath;
        FileStream fstream = null;
        if (!File.Exists(fullPath))
        {
            fstream = File.Create(fullPath);
            fstream.Close();

        }

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

        deckObj.GetComponent<DeckInstance>().id = deckObj.GetComponentInChildren<TextMeshProUGUI>().text+".txt";
        deckObj.name = deckObj.GetComponentInChildren<TextMeshProUGUI>().text;

        GameObject gameData = GameObject.Find("GameData");
        string[] lines = new string[] { };
        gameData.GetComponent<GameData>().AddDeckInfo(lines, deckObj.GetComponent<DeckInstance>().id);

        decks.Add(deckObj);
        SaveDeck(deckObj);

        yield return new WaitForSeconds(1);
        createNewButton.interactable = true;

    }

    public void Edit()
    {
        GameObject gameData = GameObject.Find("GameData");

        GameObject selectedDeck = GetSelectDeck();

        if (selectedDeck == null) return;
        gameData.GetComponent<GameData>().SetDeckBuilderPath(selectedDeck.GetComponent<DeckInstance>().path);
        gameData.GetComponent<GameData>().SetBuildId(selectedDeck.GetComponent<DeckInstance>().id);
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

        GameObject gameData = GameObject.Find("GameData");
        if (gameData == null) return;
        //Optional
        //string path = EditorUtility.OpenFilePanel("Import Deck", "", "txt");
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Import Deck", "","txt",false);
        string path = paths[0];
        if (path.Length != 0)
        {
            string[] lines = File.ReadAllLines(path);

            string fileName = Path.GetFileName(path).Replace(".txt", "");

            GameObject[] deckIcons = GameObject.FindGameObjectsWithTag("DeckIcon");
            bool changeName = false;
            foreach(GameObject goDeck in deckIcons)
            {
                if (goDeck.name.Equals(fileName))
                {
                    changeName = true;
                    break;
                }
            }

            var deckObj = Instantiate(deckIconPrefab, contentParent.transform);

            if (changeName) fileName += "_2";

            deckObj.GetComponentInChildren<TextMeshProUGUI>().text = fileName;
            gameData.GetComponent<GameData>().AddDeckInfo(lines,fileName+".txt");
            deckObj.GetComponent<DeckInstance>().SetDeckInfo(lines);

            deckObj.GetComponent<DeckInstance>().id = fileName + ".txt";
            
            decks.Add(deckObj);

            SaveDeck(deckObj);
        }
    }

    public void Export()
    {
        //Optional
        //string path = EditorUtility.SaveFilePanel("Select Path to Export Deck",Application.persistentDataPath+"/Decks", ".txt","txt");
        string path = StandaloneFileBrowser.SaveFilePanel("Select Path to Export Deck", Application.persistentDataPath + "/Decks", ".txt","txt");
        if (path.Length!=0)
        {
            FileStream fstream = null;
            if (!File.Exists(path))
            {
                fstream = File.Create(path);
                fstream.Close();
            }
            GameObject deckIcon = GetSelectDeck();
            if (deckIcon.GetComponent<DeckInstance>().GetDeckInfo().Count == 0) return;
            File.WriteAllLines(path, deckIcon.GetComponent<DeckInstance>().GetDeckInfo().ToArray());
            
        }
        Debug.Log($"ExportPath: {path}");
    }

    public void Delete()
    {
        GameObject deckIcon = GetSelectDeck();
        GameObject gameData = GameObject.Find("GameData");
        if (gameData == null)
        {
            Debug.Log("GameData is null");
            return;
        }
        if (decks.Count == 0)
        {
            Debug.Log("DeckCount is 0");
            return;
        }
        if (GetSelectDeck() == null)
        {
            Debug.Log("SelectDeck is null");
            return;
        }

        string path = deckIcon.GetComponent<DeckInstance>().path;
        
        if (File.Exists(path))
        {
            gameData.GetComponent<GameData>().RemoveDeckInfo(deckIcon.GetComponent<DeckInstance>().id);
            Destroy(deckIcon);
            decks.Remove(deckIcon);
            
            File.Delete(path);

        }


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
