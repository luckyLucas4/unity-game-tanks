using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int m_NumRoundsToWin = 5;        
    public float m_StartDelay = 3f;         
    public float m_EndDelay = 3f;           
    public CameraControl m_CameraControl;   
    public Text m_MessageText;
    public GameObject m_TankPrefab;
    public Rigidbody m_MinePrefab;
    public TankManager[] m_Tanks;
    

    private int m_RoundNumber;              
    private WaitForSeconds m_StartWait;     
    private WaitForSeconds m_EndWait;       
    private TankManager m_RoundWinner;
    private TankManager m_GameWinner;
    private string m_MineTag = "Mine";
    private List<MineExplosion> m_Mines;
    private GameObject[] m_MineObjects;
    private List<Transform> m_MineLocations;


    private void Start()
    {
        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);

        SpawnAllTanks();
        SetCameraTargets();

        SetupMines();

        StartCoroutine(GameLoop());



        //Create a list of MineExplosions and add them from the list of mineObjects
        /*mines = new List<MineExplosion>();

        for(int i = 0; i < mineObjects.Length; i++)
        {
            mines.Add(mineObjects[i].GetComponent<MineExplosion>());
        }*/

    }


    private void SpawnAllTanks()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].m_Instance =
                Instantiate(m_TankPrefab, m_Tanks[i].m_SpawnPoint.position, m_Tanks[i].m_SpawnPoint.rotation) as GameObject;
            m_Tanks[i].m_PlayerNumber = i + 1;
            m_Tanks[i].Setup();
        }
    }


    private void SetCameraTargets()
    {
        Transform[] targets = new Transform[m_Tanks.Length];

        for (int i = 0; i < targets.Length; i++)
        {
            targets[i] = m_Tanks[i].m_Instance.transform;
        }

        m_CameraControl.m_Targets = targets;
    }


    private void SetupMines()
    {
        //Store all mine objects in an array
        m_MineObjects = GameObject.FindGameObjectsWithTag(m_MineTag);

        /*m_MineLocations = new List<Transform>();

        for(int i = 0; i < m_MineObjects.Length; i++)
        {
            m_MineLocations.Add(m_MineObjects[i].GetComponent<Transform>());
        }*/
    }

    private void ReloadMines()
    {
        for(int i = 0; i < m_MineObjects.Length; i++)
        {
            m_MineObjects[i].SetActive(true);
        }
        /*foreach(Transform location in m_MineLocations)
        {
            Rigidbody mineInstance = Instantiate(m_MinePrefab, location.position, location.rotation) as Rigidbody;
        }*/
    }


    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());


        //(Check if there are exploding mines, if so come back later)/ Destroy all mines

        /*foreach(MineExplosion mine in mines)
        {
            Destroy(mine);
            Debug.Log("Checking mine: " + mine.m_Exploding.ToString());
            /*if (mine.m_Exploding)
            {
                Debug.Log("Found exploding mine");
                //yield return null;
            }
        }*/


        if (m_GameWinner != null)
        {
            SceneManager.LoadScene(0);
        }
        else 
        {
            ReloadMines();
            StartCoroutine(GameLoop());
        }
    }


    private IEnumerator RoundStarting()
    {
        ResetAllTanks();
        DisableTankControl();

        m_CameraControl.SetStartPositionAndSize();

        m_RoundNumber++;
        m_MessageText.text = "ROUND " + m_RoundNumber;

        yield return m_StartWait;
    }


    private IEnumerator RoundPlaying()
    {
        EnableTankControl();

        m_MessageText.text = string.Empty;

        while (!OneTankLeft())
        {
            yield return null;
        }

    }


    private IEnumerator RoundEnding()
    {
        for(int i = 0; i < m_MineObjects.Length; i++)
        {
            m_MineObjects[i].SetActive(false);
        }
        DisableTankControl();

        m_RoundWinner = null;
        m_RoundWinner = GetRoundWinner();

        if(m_RoundWinner != null)
            m_RoundWinner.m_Wins++;

        m_GameWinner = GetGameWinner();

        string message = EndMessage();
        m_MessageText.text = message;

        yield return m_EndWait;
    }


    private bool OneTankLeft()
    {
        int numTanksLeft = 0;

        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].m_Instance.activeSelf)
                numTanksLeft++;
        }

        return numTanksLeft <= 1;
    }


    private TankManager GetRoundWinner()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].m_Instance.activeSelf)
                return m_Tanks[i];
        }

        return null;
    }


    private TankManager GetGameWinner()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].m_Wins == m_NumRoundsToWin)
                return m_Tanks[i];
        }

        return null;
    }


    private string EndMessage()
    {
        string message = "DRAW!";

        if (m_RoundWinner != null)
            message = m_RoundWinner.m_ColoredPlayerText + " WINS THE ROUND!";

        message += "\n\n\n\n";

        for (int i = 0; i < m_Tanks.Length; i++)
        {
            message += m_Tanks[i].m_ColoredPlayerText + ": " + m_Tanks[i].m_Wins + " WINS\n";
        }

        if (m_GameWinner != null)
            message = m_GameWinner.m_ColoredPlayerText + " WINS THE GAME!";

        return message;
    }


    private void ResetAllTanks()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].Reset();
        }
    }


    private void EnableTankControl()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].EnableControl();
        }
    }


    private void DisableTankControl()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].DisableControl();
        }
    }
}