using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.ParameterUtils.CS
{
  internal static class ParamExtractor
  {
    public static FamilyParameterSet FamParams(Document doc, Reference refer)
    {
      var elem = doc.GetElement(refer);
      var famSmb = (elem as FamilyInstance)?.Symbol;

      // we need to make sure that only one element is selected.
      if (famSmb != null)
      {
        // We need to get the first and only element in the selection.
        // Do this by getting an iterator.
        // MoveNext and then get the current element.
        var famDoc = doc.EditFamily(famSmb.Family);
        return famDoc.FamilyManager.Parameters;
      }
      else
      {
        return new FamilyParameterSet();
      }
    }

    public static ParameterSet Params(Document doc, Reference refer)
    {
      var elem = doc.GetElement(refer);
      ParameterSet parameters = new ParameterSet();
      foreach (Parameter p in elem.Parameters)
      {
        parameters.Insert(p);
      }
      var typeElem = doc.GetElement(elem.GetTypeId());
      foreach (Parameter p in typeElem.Parameters)
      {
        parameters.Insert(p);
      }
      return parameters;
    }
  }
}

