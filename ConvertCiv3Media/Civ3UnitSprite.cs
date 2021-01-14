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
    // will probably make these to-override methods in Civ3UnitSprite instead
    public interface ISprite {
        void Animation(Action action, Direction direction);
        void Play();
        void Stop();
        // public void Move();
        // public void PlaySound(Action action);
        // public void SetLocation(int x, int y);
    }
    public class Civ3UnitSprite {
        // TODO: handle mismatched cases in ini file .. maybe try INI then ini ?
        public Civ3UnitSprite(string unitPath) {
            // TODO: Parameterize this and/or take ini path and chop it up
            string UnitIniPath = unitPath + "Warrior.INI";
            FileIniDataParser UnitIniFile = new FileIniDataParser();
            IniData UnitIniData = UnitIniFile.ReadFile(unitPath);

            // TODO: Fix this total hack
            string[] foo = unitPath.Split(new char[]{'/','\\'});
            foo[foo.Length-2] = "Palettes";
            foo[foo.Length-1] = "ntp00.pcx";
            Console.WriteLine(String.Join("/", foo));
            Pcx UnitPal = new Pcx(String.Join("/", foo));

            // TODO: Fix this total hack
            string[] bar = unitPath.Split(new char[]{'/','\\'});
            foreach (Action item in Enum.GetValues(typeof(Action))) {
                if (UnitIniData["Animations"][item.ToString()] != "") {
                    Console.WriteLine(UnitIniData["Animations"][item.ToString()]);
                    bar[bar.Length-1] = UnitIniData["Animations"][item.ToString()];
                    Flic UnitFlic = new Flic(String.Join("/", bar));

                    byte[,] CivColorUnitPal = new byte[256,3];
                    for (int i = 0, palSplit = 64; i < 256; i++) {
                        byte[,] TempPal = i < palSplit ?  UnitPal.Palette : UnitFlic.Palette ;
                        for (int j = 0; j < 3; j++) {
                            CivColorUnitPal[i,j] = TempPal[i < palSplit ? i : i, j];
                        }
                    }

                }
            }
        }
        public virtual void InitDisplay() {
            // override this method in the display framework to convert media to display framework objects
        }
        virtual public void Animation(Action action, Direction direction) {
            // override this method in the display framework
        }
        virtual public void Play() {
            // override this method in the display framework to start animation
        }
        virtual public void Stop() {
            // override this method in the display framework to stop animation
        }
        // public void Move();
        // public void PlaySound(Action action);
    }
}