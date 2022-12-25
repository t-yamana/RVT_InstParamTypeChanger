using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Text;

namespace Revit.SDK.Samples.ParameterUtils.CS
{
  public enum FieldType
  {
    Name,
    InstOrType,
    DataType,
    Value
  };

  public class ParamTextConverter
  {
    Document _doc;
    Dictionary<ElementId, bool> _instOrType;

    internal ParamTextConverter(Document doc, Dictionary<ElementId, bool> instOrType)
    {
      this._doc = doc;
      this._instOrType = instOrType;
    }

    public string Pass(Parameter param, FieldType[] types)
    {
      // We will make a string that has the following format,
      // name type value
      // create a StringBuilder object to store the string of one parameter
      // using the character '\t' to delimit parameter name, type and value 
      StringBuilder sb = new StringBuilder();

      var i = 0;
      foreach (FieldType ft in types)
      {
        string value = "-";  // default value
        if (ft == FieldType.Name)
        {
          value = param.Definition.Name;
        }
        else if (ft == FieldType.InstOrType)
        {
          if (_instOrType.ContainsKey(param.Id))
          {
            // isInstance (bool)
            value = _instOrType[param.Id] ? "Inst" : "Type";
          }
        } 
        else if (ft == FieldType.DataType)
        {
          switch (param.StorageType)
          {
            case StorageType.Double:
              value = "double"; break;
            case StorageType.Integer:
              value = "integer"; break;
            case StorageType.String:
              value = "string"; break;
            case StorageType.ElementId:
              value = "Element"; break;
            case StorageType.None:
              value = "None"; break;
            default:
              break;
          }
        }
        else if (ft == FieldType.Value)
        {
          switch (param.StorageType)
          {
            case StorageType.Double:
              value = param.AsDouble().ToString(); break;
            case StorageType.Integer:
              value = param.AsInteger().ToString(); break;
            case StorageType.String:
              value = param.AsString(); break;
            case StorageType.ElementId:
              // using ActiveDocument.GetElement(the element id) to 
              // retrieve the element from the active document
              ElementId elemId = param.AsElementId();
              Element relateElm = _doc.GetElement(elemId);
              // if there is an element then display its name, 
              // otherwise display the fact that it is not set
              value = relateElm?.Name ?? "Not set";
              break;
            case StorageType.None:
              value = param.AsValueString(); break;
            default:
              break;
          }
        }

        sb.AppendFormat((i==types.Length-1 ? "{0}" : "{0}\t"), value);
        i++;
      }

      return sb.ToString();
    }
  }
}

