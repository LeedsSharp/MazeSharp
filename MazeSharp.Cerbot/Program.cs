using System;
using System.Threading;
using GHIElectronics.Gadgeteer;
using MazeSharp.Players;

namespace MazeSharp.Cerbot
{
    public partial class Program
    {
        bool running = false;

        void ProgramStarted()
        {
            // If you have a Gadgeteer button to attach, uncomment the
            // following line, so you robot will sit until you press the button.
            // button1.ButtonReleased += (sender, e) => running = !running;
            new Thread(DoEdgeDetect).Start();
        }

        void DoEdgeDetect()
        {
            var player = new MazeSharp.Players.DepthFirstSearch();
            var maze = new CerbotMaze(Mainboard);

            const int runSpeed = 50;
            const int reverseSpeed = -50;
            const int reverseTime = 600;
            const int turnTime = 150;
            const double sensorThreshold = 13;
            const int extraTurnThreshold = 5000;

            var correctingFromEdge = false;
            var nextIsLeft = false;

            var lastCorrection = DateTime.Now;

            // If you have a Gadgeteer button to attach, comment out or remove the
            // following line, so you robot will sit until you press the button.
            Thread.Sleep(5 * 1000);
            running = true;

            while (true)
            {
                while (running)
                {
                    Thread.Sleep(1);

                    maze.CurrentPosition = player.Move(maze);
                }

                Mainboard.SetMotorSpeed(0, 0);
                Mainboard.SetLedBitmask(0x0);
                Thread.Sleep(500);
            }
        }
    }
}
