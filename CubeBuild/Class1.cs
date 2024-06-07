using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

namespace CubeBuild
{
    public class Class1
    {
        [CommandMethod("CreateCube")]
        public static void CreateCube()
        {
            // Get the current document and database, and start a transaction
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;

            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                // Open the Block table record for read
                BlockTable acBlkTbl;
                acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId,
                                                OpenMode.ForRead) as BlockTable;

                // Open the Block table record Model space for write
                BlockTableRecord acBlkTblRec;
                acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],
                                                OpenMode.ForWrite) as BlockTableRecord;

                // Create a 3D solid wedge
                using (Solid3d acSol3D = new Solid3d())
                {
                    acSol3D.CreateBox(6, 6, 6);

                    // Position the center of the 3D solid at (5,5,0) 
                    acSol3D.TransformBy(Matrix3d.Displacement(new Point3d(5, 5, 0) -
                                                                Point3d.Origin));

                    // Add the new object to the block table record and the transaction
                    acBlkTblRec.AppendEntity(acSol3D);
                    acTrans.AddNewlyCreatedDBObject(acSol3D, true);
                }

                // Open the active viewport
                ViewportTableRecord acVportTblRec;
                acVportTblRec = acTrans.GetObject(acDoc.Editor.ActiveViewportId,
                                                    OpenMode.ForWrite) as ViewportTableRecord;

                // Rotate the view direction of the current viewport
                acVportTblRec.ViewDirection = new Vector3d(-1, -1, 1);
                acDoc.Editor.UpdateTiledViewportsFromDatabase();

                // Save the new objects to the database
                acTrans.Commit();
            }
        }
    }
}
