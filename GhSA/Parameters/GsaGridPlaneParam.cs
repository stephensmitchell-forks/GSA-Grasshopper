﻿using System;
using System.Collections.Generic;

using GsaAPI;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using Rhino.Display;
using Rhino.Collections;

namespace GhSA.Parameters
{
    /// <summary>
    /// Grid Plane class, this class defines the basic properties and methods for any Gsa Grid Plane
    /// </summary>
    public class GsaGridPlane

    {
        #region fields
        private Plane m_plane;
        private GridPlane m_gridplane;
        private Axis m_axis;
        #endregion
        public Plane Plane
        {
            get { return m_plane; }
            set { m_plane = value; }
        }
        public GridPlane GridPlane
        {
            get { return m_gridplane; }
            set { m_gridplane = value; }
        }
        public Axis Axis
        {
            get { return m_axis; }
            set { m_axis = value; }
        }

        #region constructors
        public GsaGridPlane()
        {
            m_plane = Plane.Unset;
            m_gridplane = new GridPlane();
            m_axis = new Axis();
        }

        public GsaGridPlane(Plane plane)
        {
            m_plane = plane;
            m_gridplane = new GridPlane();
            m_axis = new Axis();
            m_axis.Origin.X = plane.OriginX;
            m_axis.Origin.Y = plane.OriginY;
            if (plane.Normal.Z > 0)
            {
                m_axis.Origin.Z = plane.OriginZ;
                m_gridplane.Elevation = 0;
            }
            else
            {
                m_axis.Origin.Z = 0;
                m_gridplane.Elevation = plane.OriginZ;
            }
            m_axis.XVector.X = plane.XAxis.X;
            m_axis.XVector.Y = plane.XAxis.Y;
            m_axis.XVector.Z = plane.XAxis.Z;
            m_axis.XYPlane.X = plane.YAxis.X;
            m_axis.XYPlane.Y = plane.YAxis.Y;
            m_axis.XYPlane.Z = plane.YAxis.Z;
        }

        public GsaGridPlane Duplicate()
        {
            GsaGridPlane dup = new GsaGridPlane();
            dup.Plane = m_plane;
            dup.GridPlane = m_gridplane;
            dup.Axis = m_axis;
            return dup;
        }


        #endregion

        #region properties
        public bool IsValid
        {
            get
            {
                if (Plane == null) { return false; }
                return true;
            }
        }
       
        
        

        #endregion

        #region methods
        public override string ToString()
        {

            return "GridPlane " + GridPlane.Name;
        }

        #endregion
    }

    /// <summary>
    /// GsaNode Goo wrapper class, makes sure GsaNode can be used in Grasshopper.
    /// </summary>
    public class GsaGridPlaneGoo : GH_GeometricGoo<GsaGridPlane>, IGH_PreviewData
    {
        #region constructors
        public GsaGridPlaneGoo()
        {
            this.Value = new GsaGridPlane();
        }
        public GsaGridPlaneGoo(GsaGridPlane gridplane)
        {
            if (gridplane == null)
                gridplane = null;
            else
            {
                if (gridplane.Plane == null)
                    gridplane = null;
            }
            this.Value = gridplane;
        }

        public override IGH_GeometricGoo DuplicateGeometry()
        {
            return DuplicateGsaNode();
        }
        public GsaGridPlaneGoo DuplicateGsaNode()
        {
            return new GsaGridPlaneGoo(Value == null ? new GsaGridPlane() : Value.Duplicate());
        }
        #endregion

        #region properties
        public override bool IsValid
        {
            get
            {
                if (Value == null) { return false; }
                return Value.IsValid;
            }
        }
        public override string IsValidWhyNot
        {
            get
            {
                if (Value.IsValid) { return string.Empty; }
                return Value.Plane.IsValid.ToString(); //Todo: beef this up to be more informative.
            }
        }
        public override string ToString()
        {
            if (Value == null)
                return "Null GridPlane";
            else
                return Value.ToString();
        }
        public override string TypeName
        {
            get { return ("GridPlane"); }
        }
        public override string TypeDescription
        {
            get { return ("GSA Grid Plane"); }
        }

        public override BoundingBox Boundingbox
        {
            get
            {
                if (Value == null) { return BoundingBox.Empty; }
                if (Value.Plane == null) { return BoundingBox.Empty; }
                Point3d pt1 = Value.Plane.Origin;
                pt1.Z += 10;
                Point3d pt2 = Value.Plane.Origin;
                pt2.Z += -10;
                Line ln = new Line(pt1, pt2);
                LineCurve crv = new LineCurve(ln);
                return crv.GetBoundingBox(false);
            }
        }
        public override BoundingBox GetBoundingBox(Transform xform)
        {
            if (Value == null) { return BoundingBox.Empty; }
            if (Value.Plane.Origin == null) { return BoundingBox.Empty; }
            Point3d pt = Value.Plane.Origin;
            pt.Z += 0.001;
            Line ln = new Line(Value.Plane.Origin, pt);
            LineCurve crv = new LineCurve(ln);
            return crv.GetBoundingBox(xform); //BoundingBox.Empty; //Value.point.GetBoundingBox(xform);
        }
        #endregion

        #region casting methods
        public override bool CastTo<Q>(out Q target)
        {
            // This function is called when Grasshopper needs to convert this 
            // instance of GsaGridPlane into some other type Q.            
            

            if (typeof(Q).IsAssignableFrom(typeof(GsaGridPlane)))
            {
                if (Value == null)
                    target = default;
                else
                    target = (Q)(object)Value;
                return true;
            }

            if (typeof(Q).IsAssignableFrom(typeof(GH_Plane)))
            {
                if (Value == null)
                    target = default;
                else
                {
                    GH_Plane pln = new GH_Plane();
                    GH_Convert.ToGHPlane(Value.Plane, GH_Conversion.Both, ref pln);
                    target = (Q)(object)pln;
                }
                return true;
            }

            target = default;
            return false;
        }
        public override bool CastFrom(object source)
        {
            // This function is called when Grasshopper needs to convert other data 
            // into GsaGridPlane.


            if (source == null) { return false; }

            //Cast from GsaGridPlane
            if (typeof(GsaGridPlane).IsAssignableFrom(source.GetType()))
            {
                Value = (GsaGridPlane)source;
                return true;
            }

            //Cast from Plane
            Plane pln = new Plane();

            if (GH_Convert.ToPlane(source, ref pln, GH_Conversion.Both))
            {
                GsaGridPlane gridplane = new GsaGridPlane(pln);
                this.Value = gridplane;
                return true;
            }

            return false;
        }
        #endregion

        #region transformation methods
        public override IGH_GeometricGoo Transform(Transform xform)
        {
            if (Value == null) { return null; }
            if (Value.Plane == null) { return null; }

            Plane pln = Value.Plane;
            pln.Transform(xform);
            GsaGridPlane gridplane = new GsaGridPlane(pln);
            return new GsaGridPlaneGoo(gridplane);
        }

        public override IGH_GeometricGoo Morph(SpaceMorph xmorph)
        {
            if (Value == null) { return null; }
            if (Value.Plane == null) { return null; }

            Plane pln = Value.Plane;
            xmorph.Morph(ref pln);
            GsaGridPlane gridplane = new GsaGridPlane(pln);
            return new GsaGridPlaneGoo(gridplane);
        }

        #endregion

        #region drawing methods
        public BoundingBox ClippingBox
        {
            get { return Boundingbox; }
        }
        public void DrawViewportMeshes(GH_PreviewMeshArgs args) 
        {
            //No meshes are drawn.   
        }
        public void DrawViewportWires(GH_PreviewWireArgs args)
        {
            if (Value == null) { return; }


            if (Value.Plane.IsValid)
            {
                if (args.Color == System.Drawing.Color.FromArgb(255, 150, 0, 0)) // this is a workaround to change colour between selected and not
                {
                    GH_Plane.DrawPlane(args.Pipeline, Value.Plane, 16, 16, System.Drawing.Color.LightGray, System.Drawing.Color.Red, System.Drawing.Color.Green);
                    args.Pipeline.DrawPoint(Value.Plane.Origin, Rhino.Display.PointStyle.RoundSimple, 3, UI.Colour.Node);
                }
                    
                else
                {
                    GH_Plane.DrawPlane(args.Pipeline, Value.Plane, 16, 16, System.Drawing.Color.DarkGray, System.Drawing.Color.Red, System.Drawing.Color.Green);
                    args.Pipeline.DrawPoint(Value.Plane.Origin, Rhino.Display.PointStyle.RoundControlPoint, 3, UI.Colour.NodeSelected);
                }
                    
            }
        }
        #endregion
    }

    /// <summary>
    /// This class provides a Parameter interface for the Data_GsaNode type.
    /// </summary>
    public class GsaGridPlaneParameter : GH_PersistentGeometryParam<GsaGridPlaneGoo>, IGH_PreviewObject
    {
        public GsaGridPlaneParameter()
          : base(new GH_InstanceDescription("GSA Grid Plane", "GridPlane", "Maintains a collection of GSA Grid Plane data.", GhSA.Components.Ribbon.CategoryName.Name(), GhSA.Components.Ribbon.SubCategoryName.Cat9()))
        {
        }

        public override Guid ComponentGuid => new Guid("161e2439-83b6-4fda-abb9-2ed938612530");

        public override GH_Exposure Exposure => GH_Exposure.quarternary;

        //protected override Bitmap Icon => Resources.CrossSections;

        //We do not allow users to pick parameter, 
        //therefore the following 4 methods disable all this ui.
        protected override GH_GetterResult Prompt_Plural(ref List<GsaGridPlaneGoo> values)
        {
            return GH_GetterResult.cancel;
        }
        protected override GH_GetterResult Prompt_Singular(ref GsaGridPlaneGoo value)
        {
            return GH_GetterResult.cancel;
        }
        protected override System.Windows.Forms.ToolStripMenuItem Menu_CustomSingleValueItem()
        {
            System.Windows.Forms.ToolStripMenuItem item = new System.Windows.Forms.ToolStripMenuItem
            {
                Text = "Not available",
                Visible = false
            };
            return item;
        }
        protected override System.Windows.Forms.ToolStripMenuItem Menu_CustomMultiValueItem()
        {
            System.Windows.Forms.ToolStripMenuItem item = new System.Windows.Forms.ToolStripMenuItem
            {
                Text = "Not available",
                Visible = false
            };
            return item;
        }

        #region preview methods
        public BoundingBox ClippingBox
        {
            get
            {
                return Preview_ComputeClippingBox();
            }
        }
        public void DrawViewportMeshes(IGH_PreviewArgs args)
        {
            //Meshes aren't drawn.
        }
        public void DrawViewportWires(IGH_PreviewArgs args)
        {
            //Use a standard method to draw gunk, you don't have to specifically implement this.
            
            
            Preview_DrawWires(args);
            

        }

        private bool m_hidden = false;
        public bool Hidden
        {
            get { return m_hidden; }
            set { m_hidden = value; }
        }
        public bool IsPreviewCapable
        {
            get { return true; }
        }
        #endregion
    }

}
