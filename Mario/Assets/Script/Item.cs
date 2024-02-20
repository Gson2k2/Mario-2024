using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public enum ItemType
{
    Box,BoxCoin,Coin
}
public class Item : MonoBehaviour
{
    public ItemType ItemType;
    public Sprite emptyCoinBox;

    private bool isEmpty;
    private CancellationTokenSource _cancellationTokenSource;
    public void OnObjectTrigger()
    {
        switch (ItemType)
        {
            case ItemType.Box:
                Destroy(this.gameObject);
                break;
            case ItemType.BoxCoin:
                if(isEmpty) return;
                isEmpty = true;
                CoinSpawningAsync();
               break; 
            case ItemType.Coin:
                MenuController.Instance.OnCoinPlus();
                Destroy(this.gameObject);
                break;
        }
    }

    private void OnDestroy()
    {
        _cancellationTokenSource?.Cancel();
    }

    async UniTask CoinSpawningAsync()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        var coinItem = Resources.Load<GameObject>("Coin");
        var coinClone = Instantiate(coinItem, this.transform.position,Quaternion.identity);

        var coinTrans = coinClone.transform.position;

        GetComponent<SpriteRenderer>().sprite = emptyCoinBox;
        await DOTween.Sequence()
            .Append(coinClone.transform.DOMoveY(coinTrans.y + 1.5f,1f))
            .Append(coinClone.transform.DOMoveY(coinTrans.y,1f))
            .WithCancellation(cancellationToken:_cancellationTokenSource.Token);

        Destroy(coinClone);
        MenuController.Instance.OnCoinPlus();
        _cancellationTokenSource.Cancel();
    }
}
