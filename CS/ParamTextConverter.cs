using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Text;

namespace Revit.SDK.Samples.ParameterUtils.CS
{
  internal class ParamTextConverter
  {
    Document _doc;
    Dictionary<ElementId, bool> _instOrType;

    internal ParamTextConverter(Document doc, Dictionary<ElementId, bool> instOrType)
    {
      this._doc = doc;
      this._instOrType = instOrType;
    }

    public string Pass(Parameter param)
    {
      // We will make a string that has the following format,
      // name type value
      // create a StringBuilder object to store the string of one parameter
      // using the character '\t' to delimit parameter name, type and value 
      StringBuilder sb = new StringBuilder();

      // the name of the parameter can be found from its definition.
      var name = param.Definition.Name;
      sb.AppendFormat("{0}\t", name);

      if (_instOrType.ContainsKey(param.Id))
      {
        sb.AppendFormat("{0}\t", _instOrType[param.Id] ? "Inst" : "Type");
      }
      else
      {
        sb.AppendFormat("{0}\t", "-");
      }

      // Revit parameters can be one of 5 different internal storage types:
      // double, int, string, Autodesk.Revit.DB.ElementId and None. 
      // if it is double then use AsDouble to get the double value
      // then int AsInteger, string AsString, None AsStringValue.
      // Switch based on the storage type
      switch (param.StorageType)
      {
        case Autodesk.Revit.DB.StorageType.Double:
          // append the type and value
          sb.AppendFormat("double\t{0}", param.AsDouble());
          break;
        case Autodesk.Revit.DB.StorageType.ElementId:
          // for element ids, we will try and retrieve the element from the 
          // document if it can be found we will display its name.
          sb.Append("Element\t");

          // using ActiveDocument.GetElement(the element id) to 
          // retrieve the element from the active document
          Autodesk.Revit.DB.ElementId elemId = param.AsElementId();
          Element relateElm = _doc.GetElement(elemId);

          // if there is an element then display its name, 
          // otherwise display the fact that it is not set
          sb.Append(relateElm?.Name ?? "Not set");
          break;
        case Autodesk.Revit.DB.StorageType.Integer:
          // append the type and value
          sb.AppendFormat("int\t{0}", param.AsInteger());
          break;
        case Autodesk.Revit.DB.StorageType.String:
          // append the type and value
          sb.AppendFormat("string\t{0}", param.AsString());
          break;
        case Autodesk.Revit.DB.StorageType.None:
          // append the type and value
          sb.AppendFormat("none\t{0}", param.AsValueString());
          break;
        default:
          break;
      }
      return sb.ToString();
    }
  }
}

