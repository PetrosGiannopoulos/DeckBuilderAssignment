using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckBuilderManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        GameObject deckCollectionObjects = GameObject.Find("DeckCollectionObjects");
        Destroy(deckCollectionObjects);

        //Debug.Log($"PersistentPath: {Application.persistentDataPath}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
