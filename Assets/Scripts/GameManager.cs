using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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
        PlayerMovement.Instance.SetState(PlayerMovement.State.None);


        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameScene");

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        //player 정보 수정
        PlayerMovement.Instance.SetDirection(PlayerMovement.Direction.Down);
        PlayerMovement.Instance.transform.position = new Vector3(5.0f, 2.9f, 0.0f);
        PlayerMovement.Instance.SetState(PlayerMovement.State.Active);

        // 카메라 fllow 설정
        GameObject camera = GameObject.FindGameObjectWithTag("Camera");
        CinemachineVirtualCamera cmvCamera = camera.GetComponent<CinemachineVirtualCamera>();
        cmvCamera.Follow = PlayerMovement.Instance.transform;

        UIManager.instance.GetMainHud().SetInventoryButton(true);
    }

    public void HouseSceneLoad()
    {
        StartCoroutine(HouseSceneLoadAsync());
    }

    IEnumerator HouseSceneLoadAsync()
    {
        //player 정보 수정
        PlayerMovement.Instance.SetState(PlayerMovement.State.None);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("HouseScene");

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        //player 정보 수정
        PlayerMovement.Instance.SetDirection(PlayerMovement.Direction.Up);
        PlayerMovement.Instance.transform.position = new Vector3(1.0f, -5.0f, 0.0f);
        PlayerMovement.Instance.SetState(PlayerMovement.State.Active);

        // 카메라 fllow 설정
        GameObject camera = GameObject.FindGameObjectWithTag("Camera");
        CinemachineVirtualCamera cmvCamera = camera.GetComponent<CinemachineVirtualCamera>();
        cmvCamera.Follow = PlayerMovement.Instance.transform;

        UIManager.instance.GetMainHud().SetInventoryButton(true);
    }
}
