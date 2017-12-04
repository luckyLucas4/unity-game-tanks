﻿using UnityEngine;
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
    private WaitForSeconds m_StartWait, m_EndWait;          
    private TankManager m_RoundWinner, m_GameWinner;
    private List<MineExplosion> m_Mines;
    private GameObject[] m_MineObjects, m_NPCs;


    private void Start()
    {
        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);

        //Store all mine objects in an array
        m_MineObjects = GameObject.FindGameObjectsWithTag("Mine");
        m_NPCs = GameObject.FindGameObjectsWithTag("NPC");

        SpawnAllTanks();
        SetCameraTargets();

        StartCoroutine(GameLoop());

    }


    private void SpawnAllTanks()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].m_Instance =
                Instantiate(m_TankPrefab, m_Tanks[i].m_SpawnPoint.position, m_Tanks[i].m_SpawnPoint.rotation) as GameObject;
            m_Tanks[i].m_PlayerNumber = i + 1;
            m_Tanks[i].Setup();

            //Add player transforms to all NPCs
            for (int j = 0; j < m_NPCs.Length; j++)
            {
                m_NPCs[j].GetComponent<NPC_Movement>().m_PlayerTransforms.Add(m_Tanks[i].m_Instance.transform);
            }
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


    private void ReloadMines()
    {
        for(int i = 0; i < m_MineObjects.Length; i++)
        {
            m_MineObjects[i].SetActive(true);
        }
    }


    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());


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
        ResetNPCs();
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

    private void ResetNPCs()
    {
        for(int i = 0; i < m_NPCs.Length; i++)
        {
            m_NPCs[i].SetActive(true); 
        }
    }
}