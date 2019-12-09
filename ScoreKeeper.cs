using System.Collections.Generic;
namespace BloonPop {
    class ScoreKeeper {
        /** Fields */
        private bool tracking;

        /** Properties */
        public List<int> HighScores { get; private set; }
        public int CurrentScore { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ScoreKeeper() {
            HighScores = new List<int>();
        }

        /// <summary>
        /// Start tracking the score
        /// </summary>
        public void StartTracking() {
            CurrentScore = 0;
            tracking = true;
        }

        /// <summary>
        /// Increment the score based on the number of matched balloons
        /// </summary>
        /// <param name="numMatched"></param>
        /// <returns></returns>
        public int IncrementScore(int numMatched) {
            if (tracking && numMatched >= 3) {
                if (numMatched > 6) numMatched = 6;
                switch (numMatched) {
                    case 4:
                        CurrentScore += 150;
                        break;
                    case 5:
                        CurrentScore += 250;
                        break;
                    case 6:
                        CurrentScore += 400;
                        break;
                    default:
                        CurrentScore += 100;
                        break;
                }
            }
            return CurrentScore;
        }

        /// <summary>
        /// Stop Tracking the score and add the score to high scores
        /// </summary>
        public void StopTracking() {
            tracking = false;
            HighScores.Add(CurrentScore);
        }

    }
}