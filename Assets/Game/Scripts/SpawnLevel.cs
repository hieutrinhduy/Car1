using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLevel : Singleton<SpawnLevel>
{
    [SerializeField] public List<GameObject> Levels;
    [SerializeField] private List<GameObject> players;
    [SerializeField] private Transform spawnLevelPoint;
    [SerializeField] private Transform spawnPlayerPoint;
    private GameObject currentplayer;
    private GameObject spawnedLevel;
    public int n;
    private void Start()
    {
        Debug.Log(GameController.Ins.level);
        n = GameController.Ins.level;
        UIManager.Ins.OnLevelChange += Ins_OnLevelChange;
        SpawnLevelMap();
        SpawnPlayer();
    }
    private void Update()
    {
        
    }
    private void Ins_OnLevelChange(object sender, System.EventArgs e)
    {
        n = GameController.Ins.level;
        GameController.Ins.Save();
        SpawnLevelMap();
        SpawnPlayer();
    }
    public void SpawnLevelMap()
    {
        SkyboxChange.Ins.ChangeSkyBox(n);
        if (Levels.Count > 0 )
        {
            if( n>0 && spawnedLevel!= null)
            {
                Destroy(spawnedLevel);
                spawnedLevel = Instantiate(Levels[n], spawnLevelPoint.position, Quaternion.identity);
                spawnedLevel.transform.parent = spawnLevelPoint;
            }
            else if(spawnedLevel != null)
            {
                Destroy(spawnedLevel);
                spawnedLevel = Instantiate(Levels[n], spawnLevelPoint.position, Quaternion.identity);
                spawnedLevel.transform.parent = spawnLevelPoint;
            }
            else
            {
                spawnedLevel = Instantiate(Levels[n], spawnLevelPoint.position, Quaternion.identity);
                spawnedLevel.transform.parent = spawnLevelPoint;
            }
        }
        else
        {
            Debug.LogWarning("LevelMap list is empty. Cannot spawn objects.");
        }
    }
    public void SpawnPlayer()
    {
        Debug.Log("SpawnPlayer");

        if (players.Count > 0)
        {
            if (currentplayer != null)
            {
                Destroy(currentplayer);
                currentplayer = Instantiate(players[GameController.Ins.carSelectionIndex], spawnPlayerPoint.position, Quaternion.identity);
                currentplayer.transform.SetParent(spawnPlayerPoint);
            }
            else
            {
                currentplayer = Instantiate(players[GameController.Ins.carSelectionIndex], spawnPlayerPoint.position, Quaternion.identity);
                currentplayer.transform.SetParent(spawnPlayerPoint);
            }
        }
        else
        {
            Debug.LogError("Players list is empty");
        }
    }
}


//player.transform.position = spawnPlayerPoint.position;
//player.transform.rotation = Quaternion.identity;
//player.GetComponent<Rigidbody>().velocity = Vector3.zero;
//player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
