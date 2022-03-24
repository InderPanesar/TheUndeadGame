using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;


[Serializable]
public struct EnemySaveInformation
{
    public Vector3 position;
    public Quaternion rotation;
    public float health;
}

[Serializable]
public struct PlayerSaveInformation
{
    public Vector3 position;
    public Quaternion rotation;
    public int playerScore;
    public float maxHealth;
    public float currentHealth;
}
[Serializable]
public struct MultipleEnemiesSaveInformation
{
    public List<EnemySaveInformation> enemies;
}

[Serializable]
public struct LevelSaveInformation
{
    public PlayerSaveInformation player;
    public MultipleEnemiesSaveInformation enemies;
}



public class SavingScripts
{

    private SavingScripts() { }
    private static SavingScripts instance { get; set; }
    public static SavingScripts Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SavingScripts();
            }
            return instance;
        }
    }


    public void SaveLevel()
    {
        string levelFilename = PlayerPrefs.GetString("currentLevel", "unknown") + "_save_file";

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerMovementScript movementScript = player.GetComponent<PlayerMovementScript>();

        PlayerSaveInformation playerSaveInformation = new PlayerSaveInformation();
        playerSaveInformation.currentHealth = movementScript.CurrentHealth;
        playerSaveInformation.playerScore = movementScript.PlayerScore;
        playerSaveInformation.maxHealth = movementScript.MaxHealth;
        playerSaveInformation.position = player.transform.position;
        playerSaveInformation.rotation = player.transform.rotation;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemies");

        MultipleEnemiesSaveInformation multipleEnemiesSaveInformation = new MultipleEnemiesSaveInformation();
        multipleEnemiesSaveInformation.enemies = new List<EnemySaveInformation>();

        foreach (GameObject enemy in enemies)
        {
            RaycastTargetScript targetScript = enemy.GetComponent<RaycastTargetScript>();

            EnemySaveInformation enemySaveInformation = new EnemySaveInformation();
            enemySaveInformation.health = targetScript.health;
            enemySaveInformation.position = enemy.transform.position;
            enemySaveInformation.rotation = enemy.transform.rotation;
            multipleEnemiesSaveInformation.enemies.Add(enemySaveInformation);

        }

        LevelSaveInformation levelSaveInformation = new LevelSaveInformation();
        levelSaveInformation.enemies = multipleEnemiesSaveInformation;
        levelSaveInformation.player = playerSaveInformation;



        XmlDocument xmlDocument = new XmlDocument();
        String fileName = Application.dataPath + "/Saves/" + levelFilename;
        XmlSerializer serializer = new XmlSerializer(typeof(LevelSaveInformation));
        using (MemoryStream stream = new MemoryStream())
        {
            serializer.Serialize(stream, levelSaveInformation);
            stream.Position = 0;
            xmlDocument.Load(stream); 
            xmlDocument.Save(fileName);
        }
    }

    public void LoadSaveFile()
    {
        string levelFilename = PlayerPrefs.GetString("currentLevel", "unknown") + "_save_file";

        String fileName = Application.dataPath + "/Saves/" + levelFilename;


        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(fileName);
        string xmlString = xmlDocument.OuterXml;
        LevelSaveInformation levelSaveInformation;
        using (StringReader read = new StringReader(xmlString))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(LevelSaveInformation));
            using (XmlReader reader = new XmlTextReader(read))
            {
                levelSaveInformation = (LevelSaveInformation)serializer.Deserialize(reader);
            }
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerMovementScript movementScript = player.GetComponent<PlayerMovementScript>();
        movementScript.LoadSaveFile(levelSaveInformation.player);


    }

}
