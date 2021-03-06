﻿using System;
using System.Collections.Generic;
using Grasshopper.Kernel.Attributes;
using Grasshopper.GUI.Canvas;
using Grasshopper.GUI;
using Grasshopper.Kernel;
using Grasshopper;
using Rhino.Geometry;
using System.Windows.Forms;
using Grasshopper.Kernel.Types;
using GsaAPI;
using GhSA.Parameters;
using System.Resources;
using Grasshopper.Documentation;

namespace GhSA.Components
{
    public class EditGsaTitles : GH_Component
    {
        #region Name and Ribbon Layout
        // This region handles how the component in displayed on the ribbon
        // including name, exposure level and icon
        public override Guid ComponentGuid => new Guid("72a2666a-aa89-47a5-a922-5e63fc9cd966");
        public EditGsaTitles()
          : base("Edit GSA Titles", "Title", "Set GSA Titles for this document",
                Ribbon.CategoryName.Name(),
                Ribbon.SubCategoryName.Cat0())
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary | GH_Exposure.obscure;

        protected override System.Drawing.Bitmap Icon => GSA.Properties.Resources.EditTitle;
        #endregion

        #region Custom UI
        //This region overrides the typical component layout


        #endregion

        #region Input and output

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Job Number", "JN", "Set Job Number for this GSA Model", GH_ParamAccess.item);
            pManager.AddTextParameter("Initials", "Ini", "Set Initials for this GSA Model", GH_ParamAccess.item);
            pManager.AddTextParameter("Title", "Tit", "Set Title for this GSA Model", GH_ParamAccess.item);
            pManager.AddTextParameter("Sub Title", "Sub", "Set Sub Title for this GSA Model", GH_ParamAccess.item);
            pManager.AddTextParameter("Calculation", "Cal", "Set Calculation Heading for this GSA Model", GH_ParamAccess.item);
            pManager.AddTextParameter("Notes", "Nt", "Set Notes for this GSA Model", GH_ParamAccess.item);
            for (int i = 0; i < pManager.ParamCount; i++)
                pManager[i].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Titles", "Titles", "List of all Titles in document", GH_ParamAccess.list);
        }
        #endregion

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_String ghstr = new GH_String();
            if (DA.GetData(0, ref ghstr))
            {
                if (GH_Convert.ToString(ghstr, out string title, GH_Conversion.Both))
                {
                    Util.GsaTitles.SetJobNumber(title);
                }
            }

            ghstr = new GH_String();
            if (DA.GetData(1, ref ghstr))
            {
                if (GH_Convert.ToString(ghstr, out string title, GH_Conversion.Both))
                {
                    Util.GsaTitles.SetInitials(title);
                }
            }
            ghstr = new GH_String();
            if (DA.GetData(2, ref ghstr))
            {
                if (GH_Convert.ToString(ghstr, out string title, GH_Conversion.Both))
                {
                    Util.GsaTitles.SetTitle(title);
                }
            }
            ghstr = new GH_String();
            if (DA.GetData(3, ref ghstr))
            {
                if (GH_Convert.ToString(ghstr, out string title, GH_Conversion.Both))
                {
                    Util.GsaTitles.SetSubTitle(title);
                }
            }
            ghstr = new GH_String();
            if (DA.GetData(4, ref ghstr))
            {
                if (GH_Convert.ToString(ghstr, out string title, GH_Conversion.Both))
                {
                    Util.GsaTitles.SetCalculation(title);
                }
            }
            ghstr = new GH_String();
            if (DA.GetData(5, ref ghstr))
            {
                if (GH_Convert.ToString(ghstr, out string title, GH_Conversion.Both))
                {
                    Util.GsaTitles.SetNotes(title);
                }
            }
            
            List<string> titles = new List<string>
            {
                "Job Number: " + Util.GsaTitles.JobNumber,
                "Initials: " + Util.GsaTitles.Initials,
                "Title: " + Util.GsaTitles.Title,
                "Sub Title: " + Util.GsaTitles.SubTitle,
                "Calculation Header: " + Util.GsaTitles.Calculation,
                "Notes: " + Util.GsaTitles.Notes
            };
            
            DA.SetDataList(0, titles);
            
        }
    }
}

