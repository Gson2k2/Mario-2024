using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    public async void OnLoadScreenAsync(bool isReplay)
    {
        if (isReplay)
        {
            MenuController.Instance.OnCheckBounties();
            await SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            MenuController.Instance.gameObject.SetActive(false);
            await SceneManager.LoadSceneAsync(0).WithCancellation(cancellationToken:this.GetCancellationTokenOnDestroy());
        }
    }
    
    public async void OnLoadFirstLevel()
    {
        
        await SceneManager.LoadSceneAsync(1);
        MenuController.Instance.OnCheckBounties();
    }

    public async void OnGoToNextLevel()
    {
        try
        {
            await SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
            MenuController.Instance.OnCheckBounties();
        }
        catch (Exception e)
        {
            Debug.Log("Game End");
            await SceneManager.LoadSceneAsync(0);

        }

    }
}
