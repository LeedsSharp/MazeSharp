using System.Collections.Generic;

namespace MazeSharp.Web.ViewModels.Home
{
    public class TeamViewModel
    {
        public string Name { get; set; }
        public IList<string> Players { get; set; }
    }
}