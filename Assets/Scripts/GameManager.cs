using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    public List<GameObject> mSlimeList = new List<GameObject>();


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);

        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    public void GameSceneLoad()
    {
        StartCoroutine(GameSceneLoadAsync());

    }


    IEnumerator GameSceneLoadAsync()
    {
        //player ���� ����
        PlayerMovement.Instance.SetState(PlayerMovement.State.None);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameScene");
        

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        MonsterSpawner.Instance.SetPuse(false);

        //player ���� ����
        PlayerMovement.Instance.SetDirection(PlayerMovement.Direction.Down);
        PlayerMovement.Instance.transform.position = new Vector3(5.0f, 2.9f, 0.0f);
        PlayerMovement.Instance.SetState(PlayerMovement.State.Active);

        // ī�޶� fllow ����
        GameObject camera = GameObject.FindGameObjectWithTag("Camera");
        CinemachineVirtualCamera cmvCamera = camera.GetComponent<CinemachineVirtualCamera>();
        cmvCamera.Follow = PlayerMovement.Instance.transform;

        
        // main hud UIȰ��ȭ
        UIManager.instance.GetMainHud().SetInventoryButton(true);
        UIManager.instance.GetMainHud().UpdatePlayerHpBar(PlayerMovement.Instance.GetHpCount(),PlayerMovement.Instance.GetMaxHp());
        UIManager.instance.GetMainHud().UpdatePlayerGoldCount();
        UIManager.instance.GetMainHud().SetPlayerInfo(true);
        UIManager.instance.SetQuestButton(true);


    }

    public void HouseSceneLoad()
    {
    

        StartCoroutine(HouseSceneLoadAsync());
    }

    IEnumerator HouseSceneLoadAsync()
    {
        //player ���� ����
        PlayerMovement.Instance.SetState(PlayerMovement.State.None);


        //Ȱ��ȭ�� slime ���� push
        for (int i = 0; i < mSlimeList.Count;)
        {
            MonsterSpawner.Instance.PushSlime(mSlimeList[i]);
        }

        MonsterSpawner.Instance.SetPuse(true);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("HouseScene");

        while (!asyncLoad.isDone)
        {
            yield return null;
        }


        //player ���� ����
        PlayerMovement.Instance.SetDirection(PlayerMovement.Direction.Up);
        PlayerMovement.Instance.transform.position = new Vector3(1.0f, -5.0f, 0.0f);
        PlayerMovement.Instance.SetState(PlayerMovement.State.Active);

        // ī�޶� fllow ����
        GameObject camera = GameObject.FindGameObjectWithTag("Camera");
        CinemachineVirtualCamera cmvCamera = camera.GetComponent<CinemachineVirtualCamera>();
        cmvCamera.Follow = PlayerMovement.Instance.transform;

        UIManager.instance.GetMainHud().SetInventoryButton(true);
        UIManager.instance.SetPlayerInfo(true);

     
      
    }
}
