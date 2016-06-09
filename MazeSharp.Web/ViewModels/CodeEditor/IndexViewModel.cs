using System;
using MazeSharp.Web.ViewModels.Shared;

namespace MazeSharp.Web.ViewModels.CodeEditor
{
    public class IndexViewModel : BasePageViewModel
    {
        public IndexViewModel(string title)
        {
            Page.Title = title;
        }
    }
}