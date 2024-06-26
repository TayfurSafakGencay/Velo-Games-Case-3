using System.Collections;
using Client.BoardMain;
using Client.Panel;
using Client.Piece;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Client.Levels.Main
{
    public abstract class Level : MonoBehaviour
    {
        public enum LevelType
        {
            Timer,
            Obstacle,
            Moves,
            Bonus
        }

        [Header("References")]
        public Board Board;

        public Hud Hud;

        [Header("Score of Stars")]
        public int ScoreFirstStar;

        public int ScoreSecondStar;

        public int ScoreThirdStar;
    
        protected int CurrentScore { get; set; }

        public LevelType Type {  get; protected set; }
    
        public bool DidWin { get; protected set; }
        
        public int LevelNumber { get; protected set; }

        protected virtual void Start()
        {
            char levelNumber = SceneManager.GetActiveScene().name[^1];
            LevelNumber = (int)char.GetNumericValue(levelNumber);
            
            Hud.SetScore(CurrentScore);
        }

        protected void GameWin()
        {
            Board.GameOver();

            DidWin = true;
        
            StartCoroutine(WaitForGridFill());
        }

        protected void GameLose()
        {
            Board.GameOver();

            DidWin = false;

            StartCoroutine(WaitForGridFill());
        }

        public virtual void OnMove()
        {
        }

        public virtual void OnPieceCleared(GamePiece piece)
        {
            CurrentScore += piece.Score;
        
            Hud.SetScore(CurrentScore);
        }

        protected IEnumerator WaitForGridFill()
        {
            while (Board.IsFilling || Board.GetObjectDestroyingCount() > 0)
            {
                yield return 0;
            }

            yield return new WaitForSeconds(0.25f);

            if (DidWin)
            {
                Hud.OnGameWin(CurrentScore);
            }
            else
            {
                Hud.OnGameLose(CurrentScore);
            }
        }
    }
}
