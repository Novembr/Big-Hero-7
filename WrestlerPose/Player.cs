using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WrestlerPose
{

    public enum StickDirection
    {
        Up,
        Down,
        Left,
        Right,
        None
    }

    public enum DisplayCircle
    {
        Tied,
        Won,
        Lost
    }

    public class Player
    {
        Pose _currentPose;
        Vector2 _wrestlerPosition;
        int _score;
        string _name;
        StickDirection _leftStickDirection;
        StickDirection _rightStickDirection;
        private List<Pose> _currentPosePattern;
        public int roundScore { get; set; }
        public int matchScore { get; set; }
        public List<int> _poseValuesForThisAI { get; set; }
        public DisplayCircle displayCircle { get; set; }
        public SoundEffect _AIIntroSound { get; set; }
        public bool CrowdMoving { get; set; }
        public bool PlayerWinningForCrowd { get; set; }
        public List<int> RoundOutcomes { get; set; }//0 = not yet determined, 1 = win and 2 = lose
        public List<int> PoseOutcomes { get; set; }//0 = not yet determined (White) , 1 = win and 2 = lose, 3 = tie

        private SoundEffectInstance _cheerInstance;
        public SoundEffectInstance CheerInstance
        {
            get { return _cheerInstance; }
            set { _cheerInstance = value; }
        }

        private SoundEffectInstance _murmurInstance;
        public SoundEffectInstance MurmurInstance
        {
            get { return _murmurInstance; }
            set { _murmurInstance = value; }
        }

        private SoundEffectInstance _booInstance;
        public SoundEffectInstance BooInstance
        {
            get { return _booInstance; }
            set { _booInstance = value; }
        }

        public Player(string name, Vector2 wrestlerPosition, Pose currentPose, List<Pose> currentPosePattern, SoundEffectInstance booInstance, SoundEffectInstance cheerInstance, SoundEffectInstance murmurInstance)
        {
            _name = name;
            _score = 0;
            _currentPose = currentPose;
            _currentPosePattern = currentPosePattern;
            _wrestlerPosition = wrestlerPosition;
            _booInstance = booInstance;
            _cheerInstance = cheerInstance;
            _murmurInstance = murmurInstance;
            CrowdMoving = false;
            RoundOutcomes = new List<int>(); RoundOutcomes.Add(0); RoundOutcomes.Add(0); RoundOutcomes.Add(0);//for max 3 rounds
            PoseOutcomes = new List<int>(); PoseOutcomes.Add(0); PoseOutcomes.Add(0); PoseOutcomes.Add(0); PoseOutcomes.Add(0); PoseOutcomes.Add(0);//for max 5 poses
        }

        public Player(string name, Vector2 wrestlerPosition, Pose currentPose, List<Pose> currentPosePattern, List<int> poseValuesForThisAi, SoundEffect aIIntroSound)
        {
            _name = name;
            _score = 0;
            _currentPose = currentPose;
            _currentPosePattern = currentPosePattern;
            _wrestlerPosition = wrestlerPosition;
            _poseValuesForThisAI = poseValuesForThisAi;
            _AIIntroSound = aIIntroSound;
        }

        public void SetPose(Pose pose) { _currentPose = pose; }
        public void SetLeftStickDirection(StickDirection stickDirection) { _leftStickDirection = stickDirection; }
        public void SetRightStickDirection(StickDirection stickDirection) { _rightStickDirection = stickDirection; }
        
        public void SetScore(int score) 
        {
            _score = score; 
        }

        public void SetPosePattern(int index, Pose pose)
        {
            _currentPosePattern[index] = pose;
        }

        public void SetPosePattern(List<Pose> poses)
        {
            _currentPosePattern = poses;
        }

        public void AddToPosePattern(Pose pose)
        {
            _currentPosePattern.Add(pose);
        }

        public List<Pose> GetPosePattern()
        {
            return _currentPosePattern;
        }

        public void IncreaseXPosition(int x)
        {
            _wrestlerPosition = new Vector2(_wrestlerPosition.X + x, _wrestlerPosition.Y);
        }

        public int GetScore() 
        {
            return _score; 
        }
        public Pose GetPose() { return _currentPose; }
        public StickDirection GetLeftStickDirection() { return _leftStickDirection; }
        public StickDirection GetRightStickDirection() { return _rightStickDirection; }
        public Vector2 GetPosition() { return _wrestlerPosition; }
    }
}
