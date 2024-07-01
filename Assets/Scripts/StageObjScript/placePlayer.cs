using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class placePlayer : MonoBehaviour
{
    [SerializeField]
    float placeWaitTime = 0.3f;
    [SerializeField]
    GameObject playerPrefab;


    void getPlayerCurrentData() { }
    IEnumerator SetPlayerLocation()
    {
        
        yield return new WaitForSeconds(placeWaitTime);
        getPlayerCurrentData();
        Instantiate(playerPrefab,transform.position,transform.rotation);//플레이어 생성
        Destroy(gameObject);

    }
    void Start()
    {
        StartCoroutine(SetPlayerLocation());
        
    }
}
