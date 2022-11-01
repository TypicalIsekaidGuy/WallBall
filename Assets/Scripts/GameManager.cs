using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform  mainCamera;
    [SerializeField] private GameObject ball_prefab;
    [SerializeField] private GameObject line_prefab;
    private GameObject ball;
    [SerializeField] private GameObject obstacle_prefab;

    [SerializeField] private float speedVertical=1;
    [SerializeField] private float speedHorizontal=10;

    [SerializeField] private GameObject menuMain;
    [SerializeField] private GameObject menuInput;
    [SerializeField] private GameObject menuGameOver;

    [SerializeField] private TextMeshProUGUI textLastTry;
    [SerializeField] private TextMeshProUGUI textTriesCount;

    private int tryCount;

    private float spentTime;
    private float changeTime=15;
    private float spawnDistance=10;
    private float clearedDistance=10;
    private float tempCameraDistance= 17.8f;
    private float clearedCameraDistance;
    private List<GameObject> tempObjects = new List<GameObject>();


    void Update()
    {
        if(ball != null)
        {
            ball.transform.Translate(Vector2.right * speedHorizontal * Time.deltaTime);
            ball.transform.Translate(Vector2.down * speedVertical * Time.deltaTime);
            mainCamera.Translate(Vector2.right * speedHorizontal * Time.deltaTime);
            clearedDistance =  ball.transform.position.x;
            clearedCameraDistance = mainCamera.transform.position.x;
            if (clearedCameraDistance - tempCameraDistance > 17.8f)
            {
                tempCameraDistance +=17.8f;
                tempObjects.Add(Instantiate(line_prefab, new Vector3(tempCameraDistance+17.8f, 0, 0), new Quaternion(0, 0, 0, 0)));
            }
            spentTime += Time.deltaTime;
            if (spentTime / changeTime > 0)
            {
                spentTime -= changeTime;
                speedVertical+=1;
            }
            if (clearedDistance + 8f - spawnDistance > 0)
            {
                spawnDistance += 10;
                RandomObstacle();
            }
        }
    }
    private void RandomObstacle()
    {
         tempObjects.Add(Instantiate(obstacle_prefab,new Vector3(clearedDistance+20f, (float)Random.Range(-2, 2),0),Quaternion.Euler(new Vector3(0,0,90))));
    }
    public void ChangeSpeed(float speed)
    {
        speedHorizontal = speed;
    }
    public void GameOver()
    {
        speedVertical = 1;
        tempCameraDistance = 17.8f;
        spawnDistance = 10;
        Destroy(ball);
        foreach (var obj in tempObjects)
        {
            Destroy(obj);
        }
        menuInput.SetActive(false);
        menuGameOver.SetActive(true);
        textLastTry.text = "Последняя попытка: " + ((int)clearedDistance + 10).ToString();
        textTriesCount.text = "Всего попыток: " + (tryCount+1).ToString();
    }
    public void StartGame()
    {
        menuMain.SetActive(false);
        menuGameOver.SetActive(false);
        menuInput.SetActive(true);
        ball = Instantiate(ball_prefab,new Vector3(-8f,0,0),new Quaternion(0,0,0,0));
    }
    public void Restart()
    {
        tryCount++;
        mainCamera.position = new Vector3(0,0,-10);
        StartGame();
    }
    public void UpButton()
    {
        ball.transform.Translate(Vector2.up* speedVertical/1.5f);
    }

}
