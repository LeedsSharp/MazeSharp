using System.Collections.Generic;
using MazeSharp.Web.ViewModels.Shared;

namespace MazeSharp.Web.ViewModels.Home
{
    public class IndexViewModel : BasePageViewModel
    {
        public IndexViewModel(string title)
        {
            Page.Title = title;
        }
        public string LoadedPlayer { get; set; }

        public string Message { get; set; }

        public string MazeJson { get; set; }

        public IList<TeamViewModel> Teams { get; set; }
    }
}