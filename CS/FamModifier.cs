using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.ParameterUtils
{
  internal class FamModifier
  {
    Document _doc;
    Document _famDoc;

    internal FamModifier(Document doc, Document famDoc)
    {
      _doc = doc;
      _famDoc = famDoc;
    }

    public void Convert2Type(FamilyParameter[] targetParams)
    {
      Transaction famdocts = new Transaction(_famDoc, "Parameter type change");

      Document updatedFamDoc =_famDoc;
      foreach (var p in targetParams)
      {
        famdocts.Start();
        updatedFamDoc.FamilyManager.MakeType(p);  // need transact one by one
        famdocts.Commit();
      }

      updatedFamDoc.LoadFamily(_doc, new LoadOpt());
      _famDoc = updatedFamDoc;  // only completed family substituted
    }

    // TODO: MakeType, MakeInstance
    // https://thebuildingcoder.typepad.com/blog/2013/04/changing-a-family-parameter-from-type-to-instance.html

    /// <summary> this is needed for No-checking family uploading in project
    /// </summary>
    private class LoadOpt : IFamilyLoadOptions
    {
      public bool OnFamilyFound(bool familyInUse, out bool overwriteParameterValues)
      {
        overwriteParameterValues = true;
        return true;
      }

      public bool OnSharedFamilyFound(Family sharedFamily, bool familyInUse, out FamilySource source, out bool overwriteParameterValues)
      {
        // When proj uses same family as other family's child
        source = FamilySource.Family;  // update family with new one

        overwriteParameterValues = true;
        return true;
      }
    }
  }
}

