using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.ParameterUtils.CS
{
  internal class ParamExtractor
  {
    internal Document FamDoc { get; private set; }
    internal FamilyParameterSet FamParams { get; private set; }
    internal ParameterSet Params { get; private set; }

    public ParamExtractor(Document doc, Reference refer)
    {
      var elem = doc.GetElement(refer);
      var famSmb = (elem as FamilyInstance)?.Symbol;

      // we need to make sure that only one element is selected.
      if (famSmb != null)
      {
        // We need to get the first and only element in the selection.
        // Do this by getting an iterator.
        // MoveNext and then get the current element.
        FamDoc = doc.EditFamily(famSmb.Family);
        FamParams = FamDoc.FamilyManager.Parameters;
      }
      else
      {
        FamParams = new FamilyParameterSet();
      }

      ParameterSet parameters = new ParameterSet();
      foreach (Parameter p in elem.Parameters)
      {
        parameters.Insert(p);  // Instance Parameter Only
      }
      var typeElem = doc.GetElement(elem.GetTypeId());
      foreach (Parameter p in typeElem.Parameters)
      {
        parameters.Insert(p);  // Type Parameter Only
      }
      Params = parameters;
    }
  }
}

