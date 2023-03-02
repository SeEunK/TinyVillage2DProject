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
        //player 정보 수정
        PlayerController.Instance.SetState(PlayerController.State.None);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameScene");
        

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        MonsterSpawner.Instance.SetPuse(false);

        //player 정보 수정
        PlayerController.Instance.SetDirection(PlayerController.Direction.Down);
        PlayerController.Instance.transform.position = new Vector3(5.0f, 2.9f, 0.0f);
        PlayerController.Instance.SetState(PlayerController.State.Active);

        // 카메라 fllow 설정
        GameObject camera = GameObject.FindGameObjectWithTag("Camera");
        CinemachineVirtualCamera cmvCamera = camera.GetComponent<CinemachineVirtualCamera>();
        cmvCamera.Follow = PlayerController.Instance.transform;

        
        // main hud UI활성화
        UIManager.instance.GetMainHud().SetInventoryButton(true);
        UIManager.instance.GetMainHud().UpdatePlayerHpBar(UserData.instance.GetHp(),UserData.instance.GetMaxHp());
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
        //player 정보 수정
        PlayerController.Instance.SetState(PlayerController.State.None);


        //활성화된 slime 전부 push
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


        //player 정보 수정
        PlayerController.Instance.SetDirection(PlayerController.Direction.Up);
        PlayerController.Instance.transform.position = new Vector3(1.0f, -5.0f, 0.0f);
        PlayerController.Instance.SetState(PlayerController.State.Active);

        // 카메라 fllow 설정
        GameObject camera = GameObject.FindGameObjectWithTag("Camera");
        CinemachineVirtualCamera cmvCamera = camera.GetComponent<CinemachineVirtualCamera>();
        cmvCamera.Follow = PlayerController.Instance.transform;

        UIManager.instance.GetMainHud().SetInventoryButton(true);
        UIManager.instance.SetPlayerInfo(true);

     
      
    }
}
