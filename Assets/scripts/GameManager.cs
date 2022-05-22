
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour

{
    [SerializeField]
    private GameObject hazardPrefab;
    [SerializeField]
    private TMPro.TextMeshProUGUI scoreText;
    [SerializeField]
    private Image backgroundMenu;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject mainVCam;
    [SerializeField]
    private GameObject zoomVCam;
    [SerializeField]
    private GameObject platform;
    [SerializeField]
    private GameObject gameOverMenu;
    private int             highScore;
    private int             score;
    private float           timer;
    private bool            gameOver;
    private Coroutine hazardsCoroutine;

   

    private static GameManager instance;
    private const string HighScorePreferenceKey = "HighScore";
    public static GameManager Instance => instance;

    public int HighScore => highScore;

    // Start is called before the first frame update
    void Start()
    {
        instance = this; 
        highScore = PlayerPrefs.GetInt(HighScorePreferenceKey);
    }

    private void OnEnable(){
        player.SetActive(true);

        mainVCam.SetActive(true);
        zoomVCam.SetActive(false);

        gameOver = false;
        score = 0;
        timer = 0;
        scoreText.text = "0";
        hazardsCoroutine = StartCoroutine(SpawnHazards());  
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(Time.timeScale == 0)
            {
                UnPause();
            }
            if(Time.timeScale == 1)
            {
                Pause();
            }
        }

        if (gameOver)
            return;

        timer += Time.deltaTime;

        if (timer >= 1f)
        {
            score++;
            scoreText.text = score.ToString();

            timer = 0;
        }

    }

    private void Pause(){
        LeanTween.value(1, 0, 0.75f)
                    .setOnUpdate(SetTimeScale)
                    .setIgnoreTimeScale(true);
                backgroundMenu.gameObject.SetActive(true);
    }

    private void UnPause(){
        LeanTween.value(0, 1, 0.75f)
                    .setOnUpdate(SetTimeScale)
                    .setIgnoreTimeScale(true);
                backgroundMenu.gameObject.SetActive(false);
    }

    private void SetTimeScale(float value)
    {
        Time.timeScale = value;
        Time.fixedDeltaTime = 0.02f * value;
    }

    private IEnumerator SpawnHazards()
    {

        var hazardToSpawn = Random.Range(1, 4);

        for(int i  = 0; i < hazardToSpawn; i++)
        {
            var x = Random.Range(-10, 10);
            var z = Random.Range(-10, 10);

            var drag = Random.Range(0f, 2f);
            var hazard = Instantiate(hazardPrefab, new Vector3(x, 11, z), Quaternion.identity);

            hazard.GetComponent<Rigidbody>().drag = drag;

        }

        var timeToWait = Random.Range(0.5f, 1.5f);
        yield return new WaitForSeconds(1f);

        yield return SpawnHazards();
    }

    public void GameOver()
    {
        StopCoroutine(hazardsCoroutine);
        gameOver = true;

        if (Time.timeScale < 1)
        {
            UnPause();
        }

        if (score > highScore){
            highScore = score;
            PlayerPrefs.SetInt(HighScorePreferenceKey, highScore);

        }

        platform.transform.rotation = new Quaternion(0,0,0,1);
        platform.GetComponent<Rigidbody>().angularVelocity = new Vector3(0,0,0);

        mainVCam.SetActive(false);
        zoomVCam.SetActive(true);

        gameObject.SetActive(false);
        gameOverMenu.SetActive(true);
    }
    public void Enable()
    {
        gameObject.SetActive(true);
    }
}
