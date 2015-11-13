using System.Threading;
using GHIElectronics.Gadgeteer;
using MazeSharp.Interfaces;
using MazeSharp.Players;

namespace MazeSharp.Cerbot
{
    public class CerbotMaze : IMaze
    {
        private readonly FEZCerbot fezCerbot;
        private readonly int runSpeed;
        private readonly int reverseSpeed;
        private int turnTime;

        public CerbotMaze(FEZCerbot fezCerbot)
        {
            this.fezCerbot = fezCerbot;
            runSpeed = 50;
            reverseSpeed = -50;
            turnTime = 1000;


            CurrentPosition = new CerbotCell(fezCerbot)
            {
                X = 0,
                Y = 0,
                IsStart = true
            };
            Orientation = Compass.East;
        }

        public Compass Orientation { get; set; }

        public ICell CurrentPosition { get; set; }
        public ICell GoNorth()
        {
            switch (Orientation)
            {
                case Compass.North:
                    return new CerbotCell(fezCerbot)
                    {
                        X = CurrentPosition.X,
                        Y = CurrentPosition.Y - 1
                    };
                default:
                    return new CerbotCell(fezCerbot);
            }
        }

        public ICell GoEast()
        {
            throw new System.NotImplementedException();
        }

        public ICell GoSouth()
        {
            throw new System.NotImplementedException();
        }

        public ICell GoWest()
        {
            throw new System.NotImplementedException();
        }

        public void GoLeft()
        {
            fezCerbot.SetMotorSpeed(runSpeed, reverseSpeed);
            Thread.Sleep(turnTime);
        }

        public void GoRight()
        {
            fezCerbot.SetMotorSpeed(reverseSpeed, runSpeed);
            Thread.Sleep(turnTime);
        }

        public void TurnAround()
        {
            fezCerbot.SetMotorSpeed(runSpeed, reverseSpeed);
            Thread.Sleep(turnTime * 2);
            //Thread.Sleep(turnTime + (interval.Milliseconds + interval.Seconds * 1000 < extraTurnThreshold ? turnTime : 0));
        }
    }
}