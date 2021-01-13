using System;
using System.IO;
using IniParser;
using IniParser.Model;

namespace ReadCivData.ConvertCiv3Media
{
    // Under construction
    
    // The order of direction animations in unit FLC files
    public enum Direction {
        SW,
        S,
        SE,
        E,
        NE,
        N,
        NW,
        W
    }
    public enum Action {
        BLANK,
        DEFAULT,
        WALK,
        RUN,
        ATTACK1,
        ATTACK2,
        ATTACK3,
        DEFEND,
        DEATH,
        DEAD,
        FORTIFY,
        FORTIFYHOLD,
        FIDGET,
        VICTORY,
        TURNLEFT,
        TURNRIGHT,
        BUILD,
        ROAD,
        MINE,
        IRRIGATE,
        FORTRESS,
        CAPTURE,
        STOP_AT_LAST_FRAME,
        PauseROAD,
        PauseMINE,
        PauseIRRIGATE      

    }
    public interface ISprite {
        void Animation(Action action, Direction direction);
        void Play();
        void Stop();
        // public void Move();
        // public void PlaySound(Action action);
        // public void SetLocation(int x, int y);
    }
    public class Civ3UnitSprite {
        public Civ3UnitSprite(string path) {
            FileIniDataParser UnitIniFile = new FileIniDataParser();
            IniData UnitIniData = UnitIniFile.ReadFile(path);
            Console.WriteLine(UnitIniData["Animations"][Action.RUN.ToString()]);
            foreach (Action item in Enum.GetValues(typeof(Action))) {
                if (UnitIniData["Animations"][item.ToString()] != "") {
                    Console.WriteLine(UnitIniData["Animations"][item.ToString()]);
                }
            }
            {
                
            }
        }
    }
}