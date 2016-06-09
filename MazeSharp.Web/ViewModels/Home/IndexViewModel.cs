using MazeSharp.Web.ViewModels.Shared;

namespace MazeSharp.Web.ViewModels.Home
{
    public class IndexViewModel : BasePageViewModel
    {
        public IndexViewModel(string title)
        {
            Page.Title = title;
        }

        public string Message { get; set; }
        public string Player { get; set; }
        public string MazeJson { get; set; }
    }
}