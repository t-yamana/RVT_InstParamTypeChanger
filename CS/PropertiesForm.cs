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


using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.ParameterUtils.CS
{

  public partial class PropertiesForm : System.Windows.Forms.Form
  {
    /// <summary> Default constructor, initialize all controls
    /// </summary>
    private PropertiesForm() { InitializeComponent(); }
    private void PropertiesForm_Load(object sender, EventArgs e) { }

    FamilyParameterSet _famParams;
    Document _doc;
    Document _famDoc;

    /// <summary> This Form is used to display the parameters Which only
    ///           Type or Instance parameters (Built-In parameters are not included)
    /// </summary>
    /// <param name="information"> A string array that will be loaded into the list view
    /// </param>
    public PropertiesForm(Document doc, Reference reference)
        : this()
    {
      _doc = doc;
      var extractor = new ParamExtractor(doc, reference);
      _famParams = extractor.FamParams;
      _famDoc = extractor.FamDoc;

      var insOrType = new Dictionary<ElementId, bool>();
      var iterator = _famParams.GetEnumerator();
      iterator.Reset();
      while (iterator.MoveNext())
      {
        object famParam = iterator.Current;
        FamilyParameter fp = famParam as FamilyParameter;
        if (fp != null)
        {
          insOrType.Add(fp.Id, fp.IsInstance);
        }
      }

      var parameters = extractor.Params;

      // create columns for ListView
      propertyListView.Columns.Add("Name");
      propertyListView.Columns.Add("Inst/Type");
      propertyListView.Columns.Add("DataType");
      propertyListView.Columns.Add("Value");

      FieldType[] types = {
        FieldType.Name,
        FieldType.InstOrType,
        FieldType.DataType,
        FieldType.Value };

      // we need to add each string in to each row of the list view,
      // and split the string
      // into substrings delimited by '\t' then put them into the columns of the row.
      var converter = new ParamTextConverter(doc, insOrType);

      // Next we need to iterate through the parameters of the element,
      // as we iterating, we will store the strings that are to be displayed
      // for the parameters in a string list "parameterItems"

      // loop all the strings, split them, and add them to rows of the list view
      foreach (Parameter param in parameters)
      {
        if (param == null) continue;

        ListViewItem lvi = new ListViewItem(converter.Pass(param, types).Split('\t'));
        propertyListView.Items.Add(lvi);
      }
      // TODO: Make a sign for Built-In param becauz cannot be converted

      Formatting();
    }

    private void Formatting()
    {
      // sort the items in the list view ordered by ascending.
      propertyListView.Sorting = SortOrder.Ascending;
      // make the column width fit the content
      propertyListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

      // increase the width of columns by 40, make them a litter wider
      int span = 40;
      foreach (ColumnHeader ch in propertyListView.Columns)
      {
        ch.Width += span;
      }
      // the last column fit the rest of the list view
      propertyListView.Columns[propertyListView.Columns.Count - 1].Width = -2;
    }


    private void buttonToType_Click(object sender, EventArgs e)
    {
      var names = new HashSet<string>();
      foreach (ListViewItem row in propertyListView.SelectedItems)
      {
        names.Add(row.SubItems[0].Text);  // 0: name
      }
      
      // TODO: use this as Singleton
      var modifier = new FamModifier(_doc, _famDoc);

      var targetParams = selectedFamParams(names);
      try
      {
        modifier.Convert2Type(targetParams, true);
      }
      catch (Exception ex)
      {
        throw ex;  // TODO: Now there are NO catch statement for here!
      }
    }

    private void buttonToInst_Click(object sender, EventArgs e)
    {
      var names = new HashSet<string>();
      foreach (ListViewItem row in propertyListView.SelectedItems)
      {
        names.Add(row.SubItems[0].Text);  // 0: name
      }
      
      // TODO: use this as Singleton
      var modifier = new FamModifier(_doc, _famDoc);

      var targetParams = selectedFamParams(names);
      try
      {
        modifier.Convert2Type(targetParams, false);
      }
      catch (Exception ex)
      {
        throw ex;  // TODO: Now there are NO catch statement for here!
      }
    }

    private FamilyParameter[] selectedFamParams(HashSet<string> names)
    {
      // CAUTION: how to initialize Array
      FamilyParameter[] targetParams = new FamilyParameter[names.Count];

      var iterator = _famParams.GetEnumerator();
      int i = 0;
      iterator.Reset();
      while (iterator.MoveNext())
      {
        object famParam = iterator.Current;
        FamilyParameter fp = famParam as FamilyParameter;
        if (names.Contains(fp.Definition.Name))
        // if (names.Contains(fp?.Definition.Name))
        {
          targetParams[i] = fp;
          i += 1;
        }
      }
      return targetParams;
    }
  }
}

