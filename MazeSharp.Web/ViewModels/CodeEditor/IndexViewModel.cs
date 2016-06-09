using System.Collections.Generic;
using MazeSharp.Web.ViewModels.Shared;

namespace MazeSharp.Web.ViewModels.CodeEditor
{
    public class IndexViewModel : BasePageViewModel
    {
        public IndexViewModel(string title)
        {
            Page.Title = title;
            Diagnostics = new List<string>();
        }

        public IList<string> Diagnostics { get; }
    }
}