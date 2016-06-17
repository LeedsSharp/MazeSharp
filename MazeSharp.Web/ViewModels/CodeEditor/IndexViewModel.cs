using System.Collections.Generic;
using MazeSharp.Web.ViewModels.Shared;

namespace MazeSharp.Web.ViewModels.CodeEditor
{
    public class IndexViewModel : BasePageViewModel
    {
        public IndexViewModel(string title)
        {
            Page.Title = title;
        }

        public IList<string> Diagnostics { get; set; }

        public string Source { get; set; }
    }
}