using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExtendoObject;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace ExtendoObject.Tests
{
    [TestClass()]
    public class ExtendoObjectTests
    {
        private TestClass1 _originalObject;

        [TestInitialize]
        public void TestInitialize()
        {
            Mapper.Initialize(cfg => { });

            _originalObject = new TestClass1();
        }

        [TestMethod()]
        public void ExtendoObject_Set_TopLevelExistingPrimitive()
        {
            dynamic extendoObject = new ExtendoObject(_originalObject);
            extendoObject.ExistingPrimitive = 23;

            Assert.AreEqual(23, extendoObject.ExistingPrimitive);
        }

        [TestMethod()]
        public void ExtendoObject_Set_TopLevelNewPrimitive()
        {
            dynamic extendoObject = new ExtendoObject(_originalObject);
            extendoObject.NewPrimitive = 23;

            Assert.AreEqual(23, extendoObject.NewPrimitive);
        }

        [TestMethod()]
        public void ExtendoObject_Set_NestedExistingPrimitive()
        {
            dynamic extendoObject = new ExtendoObject(_originalObject);
            extendoObject.ExistingObject.ExistingPrimitive = 24;

            Assert.AreEqual(24, extendoObject.ExistingObject.ExistingPrimitive);
        }

        [TestMethod()]
        public void ExtendoObject_Set_NewNestedPrimitive()
        {
            dynamic extendoObject = new ExtendoObject(_originalObject);
            extendoObject.NewObject.NewPrimitive = 24;

            Assert.AreEqual(24, extendoObject.NewObject.NewPrimitive);
        }

        class TestClass1
        {
            public int ExistingPrimitive { get; set; }
            public TestClass2 ExistingObject { get; set; } = new TestClass2();
        }

        class TestClass2
        {
            public int ExistingPrimitive { get; set; }
        }
    }
}