using GHIElectronics.Gadgeteer;
using MazeSharp.Interfaces;

namespace MazeSharp.Cerbot
{
    public class CerbotCell : ICell
    {
        private readonly FEZCerbot fezCerbot;

        public CerbotCell(FEZCerbot fezCerbot)
        {
            this.fezCerbot = fezCerbot;
            SensorThreshold = 13;
        }

        public double SensorThreshold { get; private set; }

        public int X { get; set; }
        public int Y { get; set; }

        public bool HasNorthWall
        {
            get
            {
                return CheckSensorsPrototype();
            }
        }

        public bool HasEastWall
        {
            get
            {
                return CheckSensorsPrototype();
            }
        }

        public bool HasSouthWall
        {
            get
            {
                return CheckSensorsPrototype();
            }
        }
        public bool HasWestWall
        {
            get
            {
                return CheckSensorsPrototype();
            }
        }
        public bool IsStart { get; set; }
        public bool IsExit { get; set; }


        private bool CheckSensorsPrototype()
        {
            // Turn north based on orientation
            // Go north for a short bit
            var leftOverEdge = fezCerbot.GetReflectiveReading(FEZCerbot.ReflectiveSensors.Left) < SensorThreshold;
            var rightOverEdge = fezCerbot.GetReflectiveReading(FEZCerbot.ReflectiveSensors.Right) < SensorThreshold;

            // Switch on left/right led if over edge
            fezCerbot.SetLedBitmask((ushort)(((leftOverEdge ? 1 : 0) << 0) + ((rightOverEdge ? 1 : 0) << 15)));

            return leftOverEdge & rightOverEdge;
        }

    }
}