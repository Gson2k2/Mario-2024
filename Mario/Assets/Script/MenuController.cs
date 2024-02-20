using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public static MenuController Instance;

    public TextMeshProUGUI coinUIScore;
    public GameObject winUI;
    public GameObject RePlayUI;
    public GameObject BackUI;
    public GameObject returnMainTitleUI;

    public float LeftPos;
    public float RightPos;

    private int coinScore;

    public int startCoinScore;
    void Awake()
    {
        DontDestroyOnLoad(this);
        Instance = this;
    }

    public void OnCompleteUI()
    {
        startCoinScore = coinScore;
        winUI.SetActive(true);
    }

    public async void OnReturnToMainMenu()
    {
        Destroy(gameObject);
        await SceneManager.LoadSceneAsync(0);
    }

    public void OnGameOverUI()
    {
        BackUI.SetActive(false);
        RePlayUI.SetActive(true);
        
        returnMainTitleUI.SetActive(true);
    }

    public void OnCheckBounties()
    {
        BackUI.SetActive(true);
        returnMainTitleUI.SetActive(false);
        
        coinScore = startCoinScore;
        coinUIScore.text = coinScore.ToString();
        
        LeftPos = GameObject.Find("Left - Border").transform.position.x;
        RightPos = GameObject.Find("Right - Border").transform.position.x;
    }

    public void OnCoinPlus()
    {
        coinUIScore.text = coinScore++.ToString();
    }

    private void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }
}
