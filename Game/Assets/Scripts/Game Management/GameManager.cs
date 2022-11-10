using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject playerContainer;
    [SerializeField] private GameObject startCheckPoint;
    [SerializeField] private CameraMove cam;

    private GameObject currentCheckPoint;
    private Vector3 playerPos;
    private Analytics log;
    private Scene scene;
    private float startTime;
    private float timeElapsed;
    private float previousSceneTime;
    private int lastScene;

    static private GameManager instance;
    static public GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    public void Awake()
    {

        if (instance != null)
        {
            float hold = instance.previousSceneTime; // ensure time is saved between scenes 
            Destroy(instance.gameObject);
            instance = this;
            this.previousSceneTime = hold;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }
    // Start is called before the first frame update
    void Start()
    {

        startTime = Time.time - previousSceneTime;
        Scene scene = SceneManager.GetActiveScene();
        if(startCheckPoint != null)
        {
            currentCheckPoint = startCheckPoint;
        }
        
        log = Analytics.Instance;

    }

    // Update is called once per frame
    void Update()
    {

        timeElapsed = Time.time - startTime; // delta time

    }

    public void RestartAtCheckpoint()
    {

        GameObject player = Instantiate(playerContainer); 
        player.transform.position = GetPlayerCheckPoint().transform.position; // create new avatar 
        cam.setPlayer(player.transform); // reset camera to new avatar 

    }

    public GameObject GetPlayerCheckPoint()
    {

        return currentCheckPoint; // reset the checkpoint 

    }

    public void SetPlayerCheckpoint(GameObject obj)
    {

        WriteAnalytics("CheckPoint"); // new checkpoint touched
        currentCheckPoint = obj; // sets a new checkpoint

    }

    public void LevelComplete()
    {

        WriteAnalytics("Teleported"); // teleported
        NextScene();

    }

    public void UpdatePlayerPos(Vector3 location)
    {

        playerPos = location;

    }

    public void PlayerDied()
    {

        WriteAnalytics("Player Died");

    }

    public void NextScene()
    {

        scene = SceneManager.GetActiveScene();
        lastScene = SceneManager.sceneCountInBuildSettings - 1;

        int next;
        if(scene.buildIndex == lastScene)
        {
            next = 0; // return to the start again
        }
        else
        {
            next = scene.buildIndex + 1;
        }

        previousSceneTime = timeElapsed;
        SceneManager.LoadScene(next);

    }

    private void WriteAnalytics(string cause)
    {

        if(log != null)
        {
            log.WriteLine(cause,timeElapsed ,SceneManager.GetActiveScene().name, playerPos);
        }
     
    }

    public void startGame() // will need to be adjusted to save games and continue games and what not
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            previousSceneTime = 0;
            SceneManager.LoadScene(1);
        }
    }

}
