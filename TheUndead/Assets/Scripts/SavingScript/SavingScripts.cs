using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;


/// <summary>
/// Serializable for specific enemy information.
/// </summary>
[Serializable]
public struct EnemySaveInformation
{
    public Vector3 position;
    public Quaternion rotation;
    public float health;
    public int randomSpot;
    public bool isEnabled;
}

/// <summary>
/// Serializable for the player information.
/// </summary>
[Serializable]
public struct PlayerSaveInformation
{
    public Vector3 position;
    public Quaternion rotation;
    public int playerScore;
    public float maxHealth;
    public float currentHealth;
    public int ammo;
    public int ammoCapacity;
}

/// <summary>
/// Serializable for multiple enemies information.
/// </summary>
[Serializable]
public struct MultipleEnemiesSaveInformation
{
    public List<EnemySaveInformation> enemies;
}

/// <summary>
/// Serializable for multiple enemies and player information.
/// </summary>
[Serializable]
public struct LevelSaveInformation
{
    public PlayerSaveInformation player;
    public MultipleEnemiesSaveInformation enemies;
}


/// <summary>
/// Singleton class for each Saving Script.
/// </summary>
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


    /// <summary>
    /// Save the level which the user is currently in.
    /// </summary>
    public String SaveLevel()
    {
        CreateDirectoryIfDoesNotExist();
        string levelFilename = PlayerPrefs.GetString("currentLevel", "unknown") + "_save_file";

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerMovementScript movementScript = player.GetComponent<PlayerMovementScript>();
        PlayerStatsScript statsScript = player.GetComponent<PlayerStatsScript>();
        GunScript gunScript = player.GetComponentInChildren<GunScript>();

        PlayerSaveInformation playerSaveInformation = new PlayerSaveInformation();
        playerSaveInformation.currentHealth = statsScript.currentHealth;
        playerSaveInformation.playerScore = statsScript.playerScore;
        playerSaveInformation.maxHealth = statsScript.maxHealth;
        playerSaveInformation.position = player.transform.position;
        playerSaveInformation.rotation = player.transform.rotation;
        playerSaveInformation.ammo = gunScript.currentAmmo;
        playerSaveInformation.ammoCapacity = gunScript.ammoCapacity;

        GameObject enemyContainer = GameObject.FindGameObjectWithTag("EnemyContainer");
        EnemiesContainerScript containerScript = enemyContainer.GetComponent<EnemiesContainerScript>();

        GameObject[] enemies = containerScript.enemies;

        MultipleEnemiesSaveInformation multipleEnemiesSaveInformation = new MultipleEnemiesSaveInformation();
        multipleEnemiesSaveInformation.enemies = new List<EnemySaveInformation>();

        foreach (GameObject enemy in enemies)
        {
            RaycastTargetScript targetScript = enemy.GetComponent<RaycastTargetScript>();

            EnemySaveInformation enemySaveInformation = new EnemySaveInformation();
            enemySaveInformation.health = targetScript.health;
            enemySaveInformation.position = enemy.transform.position;
            enemySaveInformation.rotation = enemy.transform.rotation;
            enemySaveInformation.randomSpot = targetScript.RandomSpot;
            enemySaveInformation.isEnabled = targetScript.isEnabled;
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

        return "SAVED GAME!";

    }

    /// <summary>
    /// Load the save file of the level which the user is currently in.
    /// </summary>
    public String LoadSaveFile()
    {
        CreateDirectoryIfDoesNotExist();
        string levelFilename = PlayerPrefs.GetString("currentLevel", "unknown") + "_save_file";

        String fileName = Application.dataPath + "/Saves/" + levelFilename;
        if (!System.IO.File.Exists(fileName))
        {
            return "No Save File.";
        }

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


        GameObject enemyContainer = GameObject.FindGameObjectWithTag("EnemyContainer");
        EnemiesContainerScript containerScript = enemyContainer.GetComponent<EnemiesContainerScript>();

        GameObject[] enemies = containerScript.enemies;

        for (int i = 0; i < enemies.Length; i++)
        {
            GameObject enemy = enemies[i];
            RaycastTargetScript targetScript = enemy.GetComponent<RaycastTargetScript>();

            targetScript.LoadSaveFile(levelSaveInformation.enemies.enemies[i]);
        }


        return "LOADED GAME!";


    }

    private void CreateDirectoryIfDoesNotExist()
    {
        System.IO.FileInfo file = new System.IO.FileInfo(Application.dataPath + "/Saves/");
        file.Directory.Create();
    }

}
