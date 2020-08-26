using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WrestlerPose
{
    public class Player
    {
        Pose _currentPose;
        Vector2 _wrestlerPosition;
        int _score;
        string _name;
        string _currentOutcome;


        public Player(string name, Vector2 wrestlerPosition, Pose currentPose)
        {
            _name = name;
            _score = 0;
            _currentPose = currentPose;
            _wrestlerPosition = wrestlerPosition;
            _currentOutcome = "Pending";
        }

        public void SetPose(Pose pose) { _currentPose = pose; }
        public void SetScore(int score) { _score = score; }
        public void SetCurrentOutcome(string outcome) { _currentOutcome = outcome; }


        public int GetScore() { return _score; }
        public Pose GetPose() { return _currentPose; }
        public Vector2 GetPosition() { return _wrestlerPosition; }
        public string GetCurrentOutcome() { return _currentOutcome; }
        public string GetName() { return _name; }


    }
}
