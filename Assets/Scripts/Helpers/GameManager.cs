using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    static GameManager instance = null;
    public static GameManager Instance => instance;
    private int _lives = 3;
    public int TestVar = 500;
    [SerializeField] PlayerController playerPrefab;
    [SerializeField] private int _maxLives = 5;
    public PlayerController PlayerInstance => playerInstance;
    PlayerController playerInstance = null;
    
    Transform currentCheckpoint;

    bool TestMode = false;

    //Fields and Properties

    public int lives
    {
        get => _lives;
        set
        {
            // Check if the player has lost a life
            if (_lives > value)
            {
                // Respawn the player
                Respawn();
            }

            // Update the number of lives
            _lives = value;

            // Check if the player has run out of lives
            if (_lives <= 0)
            {
                // Trigger game over
                GameOver();
            }

            if (TestMode) Debug.Log("Lives has been set to: " + _lives.ToString());
        }
    }



    private int _score = 0;
    public int score
    {
        get => _score;
        set
        {
            _score = value;

            if (TestMode) Debug.Log("Score has been set to: " + _score.ToString());
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }

        instance = this; 

        DontDestroyOnLoad(gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
         if (SceneManager.GetActiveScene().name == "Level")
            {
                SceneManager.LoadScene(0);
                
            } 
         else if (SceneManager.GetActiveScene().name == "GameOver")
            {
                SceneManager.LoadScene(0);
            }
         else 
                SceneManager.LoadScene(1);

        }
    }
    public void SpawnPlayer(Transform spawnLocation)
    {

        playerInstance = Instantiate(playerPrefab, spawnLocation.position, spawnLocation.rotation);
        currentCheckpoint = spawnLocation;
    }

    public void UpdateCheckpoint(Transform updatedCheckpoint)
    {
        currentCheckpoint = updatedCheckpoint;
    }

    void Respawn()
    {
        playerInstance.transform.position = currentCheckpoint.position;
    }
    void GameOver()
    {
        _lives = _maxLives;
        SceneManager.LoadScene(2);


    }
}