using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public enum GameState { Play, Won, Lost }

public class GameFlowManager : MonoBehaviour
{
    [Header("Parameters")]
    public float endSceneLoadDelay = 3f;
    public CanvasGroup endGameFadeCanvasGroup;

    //[Header("Win")]
    //public string winSceneName = "WinScene";
    //public float delayBeforeFadeToBlack = 4f;
    //public float delayBeforeWinMessage = 2f;
    //public AudioClip victorySound;
    //public DisplayMessage winDisplayMessage;

    //[Header("Lose")]
    //public string loseSceneName = "LoseScene";
    //public DisplayMessage loseDisplayMessage;

    public PlayableDirector raceCountdownTrigger;
    public MotorbikeController playerMoto;
    public ControllerInput controllerInput;

    public GameState gameState { get; private set; } = GameState.Play;
    ObjectiveManager objectiveManager;
    TimeManager timeManager;
    float timeToLoadScene;
    float elapsedTimeBeforeScene = 0;

    private bool raceStarted = false;

    void Start()
    {
        if (!playerMoto)
            playerMoto = FindObjectOfType<MotorbikeController>();

        if (!controllerInput)
            controllerInput = FindObjectOfType<ControllerInput>();

        if (!playerMoto || !controllerInput)
        {
            Debug.LogError("MotorbikeController or ControllerInput missing!");
            enabled = false;
            return;
        }

        //objectiveManager = FindObjectOfType<ObjectiveManager>();
        timeManager = FindObjectOfType<TimeManager>();

        //winDisplayMessage.gameObject.SetActive(false);
        //loseDisplayMessage.gameObject.SetActive(false);

        timeManager.StopRace();

        // Disable player input until race starts
        controllerInput.enabled = false;

        ShowRaceCountdownAnimation();
        //StartCoroutine(ShowObjectivesRoutine());
        StartCoroutine(CountdownThenStartRaceRoutine());
    }

    IEnumerator CountdownThenStartRaceRoutine()
    {
        yield return new WaitForSeconds(3f);
        StartRace();
    }

    void StartRace()
    {
        raceStarted = true;
        controllerInput.enabled = true;
        timeManager.StartRace();
    }

    void ShowRaceCountdownAnimation()
    {
        if (raceCountdownTrigger != null)
            raceCountdownTrigger.Play();
    }

    //IEnumerator ShowObjectivesRoutine()
    //{
    //    while (objectiveManager.Objectives.Count == 0)
    //        yield return null;

    //    yield return new WaitForSecondsRealtime(0.2f);

    //    foreach (var obj in objectiveManager.Objectives)
    //    {
    //        if (obj.displayMessage)
    //            obj.displayMessage.Display();
    //        yield return new WaitForSecondsRealtime(1f);
    //    }
    //}

    void Update()
    {
        if (gameState != GameState.Play)
        {
            elapsedTimeBeforeScene += Time.deltaTime;

            float fadeRatio = 1 - (timeToLoadScene - Time.time) / endSceneLoadDelay;
            endGameFadeCanvasGroup.alpha = fadeRatio;

            float volume = Mathf.Clamp01(1 - fadeRatio);
            AudioUtility.SetMasterVolume(volume);

        //    if (Time.time >= timeToLoadScene)
        //    {
        //        SceneManager.LoadScene(gameState == GameState.Won ? winSceneName : loseSceneName);
        //        gameState = GameState.Play;
        //    }
        //}
        //else if (raceStarted)
        //{
        //    if (objectiveManager.AreAllObjectivesCompleted())
        //        EndGame(true);
        //    else if (timeManager.IsFinite && timeManager.IsOver)
        //        EndGame(false);
        //}
    }

    //void EndGame(bool win)
    //{
    //    Cursor.lockState = CursorLockMode.None;
    //    Cursor.visible = true;

    //    timeManager.StopRace();
    //    controllerInput.enabled = false;

    //    gameState = win ? GameState.Won : GameState.Lost;
    //    endGameFadeCanvasGroup.gameObject.SetActive(true);
    //    timeToLoadScene = Time.time + endSceneLoadDelay + delayBeforeFadeToBlack;

    //    if (win)
    //    {
    //        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
    //        audioSource.clip = victorySound;
    //        audioSource.outputAudioMixerGroup = AudioUtility.GetAudioGroup(AudioUtility.AudioGroups.HUDVictory);
    //        audioSource.PlayScheduled(AudioSettings.dspTime + delayBeforeWinMessage);

    //        winDisplayMessage.delayBeforeShowing = delayBeforeWinMessage;
    //        winDisplayMessage.gameObject.SetActive(true);
    //    }
    //    else
    //    {
    //        loseDisplayMessage.delayBeforeShowing = delayBeforeWinMessage;
    //        loseDisplayMessage.gameObject.SetActive(true);
    //    }
    //
    }
}
