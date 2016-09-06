using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/// <summary>
/// This script handles amount of enemies to spawn depending on currentWave which is updated
/// by TargetManager, might switch that around since think should actually be other way around
/// </summary>
namespace Phoenix
{
    public class GameManager : MonoBehaviour
    {
       
        #region All of the managers
        public static GameManager gameManager;
        private TargetManager _enemyManager;

        #endregion
        //Might change to level manager? Don't think makes sense as a name tohugh.
        private LevelAnimations levelAnims;
        //UI Variables
        private GameObject _startGamePanel;
        private GameObject _gameOverPanel;
        //Updates in game stats
        private int _playerPoints;
        private Text _displayPoints;
        private float _timeLeftInRound;
        private Text _displayTimeLeft;

        //Final end of game stats
        private Text _displayTotalPoints;
        private Text _displayFinalTime;
        

      
        #region Variables managing the round
        private int difficulty = 1;
        private int waveNumber = 1;
        private float _totalTime = 40.0f;
        #endregion

 

        public string currentDifficulty
        {
            get; set;
        }
        public float timeLeftInRound
        {
            set { _timeLeftInRound = value;
                //You were working what happend...
                _displayTimeLeft.text = _timeLeftInRound.ToString();

            }

            //return time left to keep track of in UI
            get { return _timeLeftInRound; }
        }
        /// <summary>
        /// This adds to player points and returns the tallied up score when round is over.
        /// </summary>
        public int playerPoints
        {
            set
            {
                _playerPoints += value;
                _displayPoints.text = _playerPoints.ToString();
            }
            get { return _playerPoints; }//Returning for UI
        }

        //Adds time left in round when player kills TimeTarget
        public float addTimeLeftInRound
        {
            set { _timeLeftInRound += value;}

        }

        private void Awake()
        {
            if (gameManager == null)
            {
                gameManager = this;
            }
            else
            {
                Destroy(this);
            }

            levelAnims = GameObject.Find("Level_Beta").GetComponent<LevelAnimations>();

            //Getting references to UI
            _startGamePanel = GameObject.Find("StartGamePanel");
            _gameOverPanel = GameObject.Find("GameOverPanel");
            
            _displayPoints = GameObject.Find("DisplayPoints").GetComponent<Text>();
            _displayTimeLeft = GameObject.Find("DisplayTimeLeft").GetComponent<Text>();
            _displayTotalPoints = GameObject.Find("DisplayFinalScore").GetComponent<Text>();
            _displayFinalTime = GameObject.Find("DisplayFinalTime").GetComponent<Text>();


        }

        private void Start()
        {

            _enemyManager = GetComponent<TargetManager>();
            _gameOverPanel.gameObject.SetActive(false);
            levelAnims.closeShutters();
            timeLeftInRound = 0;

        }

        #region Game state functions
        public void GoMainMenu()
        {
            _startGamePanel.SetActive(true);

        }
        public void startRound()
        {
            //if (!levelAnims.ShuttersMoving)
                levelAnims.openShutters();
            if (_gameOverPanel.gameObject.activeInHierarchy)
                _gameOverPanel.SetActive(false);
            _startGamePanel.SetActive(false);
            //This extra call is for retries, otherwise it won't spawn it.
            _enemyManager.initializeWave(100.002f, 1);
            _enemyManager.startWave();

            timeLeftInRound = _totalTime;
            playerPoints = 0;
        }

        public void waveOver()
        {
            //It's going to open and close after every wave, so for both a small pause between and that will make this an Ienum, this time not just for coroutine but for purpose
            
            //levelAnims.closeShutters();
            
            _enemyManager.startWave();
            _enemyManager.initializeWave((playerPoints + 1) * 100.002f, difficulty);
           // yield return new WaitUntil(() => { return levelAnims.ShuttersStill; });
        }
        private void endRound()
        {
            levelAnims.closeShutters();
            _enemyManager.despawnAllAnchors();
            _gameOverPanel.SetActive(true);
            
            _displayTotalPoints.text = playerPoints.ToString();
            _displayFinalTime.text = timeLeftInRound.ToString();
        }
        public void leaveGame()
        {
            //Will only work on build release of game, but does work.
            Application.Quit();
        }
        #endregion
        private void Update()
        {
            if (_enemyManager.inWave)
            {
                if (timeLeftInRound > 0)
                {
                    timeLeftInRound -= Time.deltaTime;
                }
                if (timeLeftInRound <= 0)
                {
                    endRound();
                }

                //temp input for showing updating score working.
                if (Input.GetKeyDown(KeyCode.S))
                {
                    playerPoints = 5;
                }
            }
        }
    }
}