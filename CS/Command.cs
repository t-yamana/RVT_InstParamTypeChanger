//
// (C) Copyright 2003-2019 by Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//


using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Autodesk.Revit.UI.Selection;

namespace Revit.SDK.Samples.ParameterUtils.CS
{
  /// <summary> display a Revit element property-like form
  ///           related to the selected element.
  /// </summary>
  [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
  [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
  [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
  public class Command : IExternalCommand
  {
    #region IExternalCommand Members
    /// <summary> Implement this method as an external command for Revit.
    /// </summary>
    /// <param name="commandData">An object that is passed to the external application 
    /// which contains data related to the command, 
    /// such as the application object and active view.</param>
    /// <param name="message">A message that can be set by the external application 
    /// which will be displayed if a failure or cancellation is returned by 
    /// the external command.</param>
    /// <param name="elements">A set of elements to which the external application 
    /// can add elements that are to be highlighted in case of failure or cancellation.</param>
    /// <returns>Return the status of the external command. 
    /// A result of Succeeded means that the API external method functioned as expected. 
    /// Cancelled can be used to signify that the user cancelled the external operation 
    /// at some point. Failure should be returned if the application is unable to proceed with 
    /// the operation.</returns>
    public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message,
        ElementSet elements)
    {
      // set out default result to failure.
      Autodesk.Revit.UI.Result retRes = Autodesk.Revit.UI.Result.Failed;

      Autodesk.Revit.UI.UIApplication app = commandData.Application;

      var uidoc = commandData.Application.ActiveUIDocument;
      var doc = uidoc.Document;
      var reference = uidoc.Selection.PickObject(ObjectType.Element);

      // get the elements selected
      // The current selection can be retrieved from the active 
      // document via the selection object
      var elem = doc.GetElement(reference);
      var famSmb = (doc.GetElement(reference) as FamilyInstance)?.Symbol;

      // we need to make sure that only one element is selected.
      if (famSmb != null)
      {
        // we need to get the first and only element in the selection. Do this by getting 
        // an iterator. MoveNext and then get the current element.

        var famDoc = doc.EditFamily(famSmb.Family);

        // Next we need to iterate through the parameters of the element,
        // as we iterating, we will store the strings that are to be displayed
        // for the parameters in a string list "parameterItems"
        List<string> parameterItems = new List<string>();

        FamilyParameterSet parameters = famDoc.FamilyManager.Parameters;
        var insOrType = new Dictionary<ElementId, bool>();
        foreach (FamilyParameter param in parameters)
        {
          insOrType.Add(param.Id, param.IsInstance);
        }

        var typeElem = doc.GetElement(elem.GetTypeId());
        foreach (Parameter param in typeElem.Parameters)
        {
          if (param == null) continue;

          // We will make a string that has the following format,
          // name type value
          // create a StringBuilder object to store the string of one parameter
          // using the character '\t' to delimit parameter name, type and value 
          StringBuilder sb = new StringBuilder();

          // the name of the parameter can be found from its definition.
          var name = param.Definition.Name;
          sb.AppendFormat("{0}\t", name);

          if (insOrType.ContainsKey(param.Id))
          {
            sb.AppendFormat("{0}\t", insOrType[param.Id] ? "Inst" : "Type");
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
              Element elem = app.ActiveUIDocument.Document.GetElement(elemId);

              // if there is an element then display its name, 
              // otherwise display the fact that it is not set
              sb.Append(elem != null ? elem.Name : "Not set");
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

          // add the completed line to the string list
          parameterItems.Add(sb.ToString());
        }

        // Create our dialog, passing it the parameters array for display.
        PropertiesForm propertiesForm = new PropertiesForm(parameterItems.ToArray());
        propertiesForm.StartPosition = FormStartPosition.CenterParent;
        propertiesForm.ShowDialog();
        retRes = Autodesk.Revit.UI.Result.Succeeded;
      }
      else
      {
        message = "Please select only one element";
      }
      return retRes;
    }

    #endregion
  }
}

