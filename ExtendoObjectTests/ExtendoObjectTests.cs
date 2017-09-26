using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExtendoObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace ExtendoObject.Tests
{
    [TestClass()]
    public class ExtendoObjectTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            Mapper.Initialize(cfg => { });
        }

        [TestMethod()]
        public void ExtendoObject_Set_SimpleProperty_SimplePropertyIsSet()
        {
            var originalObject = new Foo
            {
                SimpleProperty = 1
            };

            dynamic extendoObject = new ExtendoObject(originalObject);
            extendoObject.SimpleProperty = 23;

            Assert.AreEqual(23, extendoObject.SimpleProperty);
        }

        class Foo
        {
            public int SimpleProperty { get; set; }
        }
    }
}