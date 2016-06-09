using System.Web.Mvc;
using MazeSharp.Web.ViewModels.CodeEditor;

namespace MazeSharp.Web.Controllers
{
    public class CodeEditorController : Controller
    {
        //
        // GET: /CodeEditor/

        public ActionResult Index()
        {
            var viewModel = new IndexViewModel("Code Editor");

            return View(viewModel);
        }

    }
}