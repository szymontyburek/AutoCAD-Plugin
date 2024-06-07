using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;

namespace CubeBuild
{
    public class Class1
    {
        [CommandMethod("GetStringFromUser")]
        public static void GetStringFromUser()
        {
            Document acDoc = Application.DocumentManager.MdiActiveDocument;

            PromptStringOptions pStrOpts = new PromptStringOptions("\nEnter your name: ");
            pStrOpts.AllowSpaces = true;
            PromptResult pStrRes = acDoc.Editor.GetString(pStrOpts);

            Application.ShowAlertDialog("The name entered was: " +
                                        pStrRes.StringResult);
        }
    }
}
