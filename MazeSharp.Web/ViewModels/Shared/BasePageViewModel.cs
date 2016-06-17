namespace MazeSharp.Web.ViewModels.Shared
{
    public abstract class BasePageViewModel
    {
        protected BasePageViewModel()
        {
            Page = new PageDetailsViewModel();
        }
        public PageDetailsViewModel Page { get; set; }
    }
}