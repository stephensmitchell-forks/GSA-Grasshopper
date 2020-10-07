﻿using System;
using System.Reflection;
using NUnit.Framework;
using GhSA;
using GhSA.Parameters;
using Rhino.Geometry;

namespace UnitTestGhSA
{
    public class UnitTest1
    {
        [TestCase]
        public void TestMethod1()
        {
            Assembly ass1 = Assembly.LoadFile(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\Oasys\\GSA 10.1\\GsaAPI.dll");
            GsaNode node = new GsaNode(new Point3d(10, 15, 7.8));
            
            Assert.AreEqual(10, node.Point.X);
            Assert.AreEqual(15, node.Point.Y);
            Assert.AreEqual(7.8, node.Point.Z);
        }
    }
}