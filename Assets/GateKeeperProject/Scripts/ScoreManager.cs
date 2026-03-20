using UnityEngine;

namespace GateKeeperProject.Scripts
{
    public class ScoreManager : MonoBehaviour
    {
        public int CurrentScore { get; private set; }=  0;
        public void AddScore(int score)
        {
            CurrentScore += score;
        }
    }
}