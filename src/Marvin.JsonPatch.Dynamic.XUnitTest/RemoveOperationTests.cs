﻿using Marvin.JsonPatch.Dynamic.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Marvin.JsonPatch.Dynamic.XUnitTest
{
    public class RemoveOperationTests
    {


        [Fact]
        public void RemovePropertyShouldFailIfRootIsAnonymous()
        {

            dynamic doc = new
            {
                Test = 1
            };

            // create patch
            JsonPatchDocument patchDoc = new JsonPatchDocument();
            patchDoc.Remove("Test");

            var serialized = JsonConvert.SerializeObject(patchDoc);
            var deserialized = JsonConvert.DeserializeObject<JsonPatchDocument>(serialized);

            Assert.Throws<JsonPatchException>(() => { deserialized.ApplyTo(doc); });

        }


        [Fact]
        public void RemovePropertyShouldFailIfItDoesntExist()
        {

            dynamic doc = new ExpandoObject();
            doc.Test = 1; 

            // create patch
            JsonPatchDocument patchDoc = new JsonPatchDocument();
            patchDoc.Remove("NonExisting");

            var serialized = JsonConvert.SerializeObject(patchDoc);
            var deserialized = JsonConvert.DeserializeObject<JsonPatchDocument>(serialized);

            Assert.Throws<JsonPatchException>(() => { deserialized.ApplyTo(doc); });

        }


         
        [Fact]
        public void RemovePropertyFromExpandoObject()
        {

            dynamic obj = new ExpandoObject();
            obj.Test = 1; 

            // create patch
            JsonPatchDocument patchDoc = new JsonPatchDocument();
            patchDoc.Remove("Test");

            var serialized = JsonConvert.SerializeObject(patchDoc);
            var deserialized = JsonConvert.DeserializeObject<JsonPatchDocument>(serialized);


            deserialized.ApplyTo(obj);

            var cont = obj as IDictionary<string, object>;

            object valueFromDictionary;

            cont.TryGetValue("Test", out valueFromDictionary);
            
            Assert.Null(valueFromDictionary);

        }



        [Fact]
        public void RemovePropertyFromExpandoObjectMixedCase()
        {

            dynamic obj = new ExpandoObject();
            obj.Test = 1;

            // create patch
            JsonPatchDocument patchDoc = new JsonPatchDocument();
            patchDoc.Remove("test");

            var serialized = JsonConvert.SerializeObject(patchDoc);
            var deserialized = JsonConvert.DeserializeObject<JsonPatchDocument>(serialized);


            deserialized.ApplyTo(obj);

            var cont = obj as IDictionary<string, object>;

            object valueFromDictionary;

            cont.TryGetValue("Test", out valueFromDictionary);

            Assert.Null(valueFromDictionary);

        }




        [Fact]
        public void RemoveNestedPropertyFromExpandoObject()
        {

            dynamic obj = new ExpandoObject();
            obj.Test = new ExpandoObject();
            obj.Test.AnotherTest = "A";

            // create patch
            JsonPatchDocument patchDoc = new JsonPatchDocument();
            patchDoc.Remove("Test");

            var serialized = JsonConvert.SerializeObject(patchDoc);
            var deserialized = JsonConvert.DeserializeObject<JsonPatchDocument>(serialized);


            deserialized.ApplyTo(obj);

            var cont = obj as IDictionary<string, object>;

            object valueFromDictionary;

            cont.TryGetValue("Test", out valueFromDictionary);

            Assert.Null(valueFromDictionary);

        }





        [Fact]
        public void RemoveNestedPropertyFromExpandoObjectMixedCase()
        {

            dynamic obj = new ExpandoObject();
            obj.Test = new ExpandoObject();
            obj.Test.AnotherTest = "A";

            // create patch
            JsonPatchDocument patchDoc = new JsonPatchDocument();
            patchDoc.Remove("test");

            var serialized = JsonConvert.SerializeObject(patchDoc);
            var deserialized = JsonConvert.DeserializeObject<JsonPatchDocument>(serialized);


            deserialized.ApplyTo(obj);

            var cont = obj as IDictionary<string, object>;

            object valueFromDictionary;

            cont.TryGetValue("Test", out valueFromDictionary);

            Assert.Null(valueFromDictionary);

        }


        [Fact]
        public void NestedRemove()
        {
            dynamic doc = new ExpandoObject();
            doc.SimpleDTO = new SimpleDTO()
                {
                    StringProperty = "A"
                };
            

            // create patch
            JsonPatchDocument patchDoc = new JsonPatchDocument();
            patchDoc.Remove("SimpleDTO/StringProperty");

            var serialized = JsonConvert.SerializeObject(patchDoc);
            var deserialized = JsonConvert.DeserializeObject<JsonPatchDocument>(serialized);


            deserialized.ApplyTo(doc);

            Assert.Equal(null, doc.SimpleDTO.StringProperty);

        }

        [Fact]
        public void NestedRemoveMixedCase()
        {
            dynamic doc = new ExpandoObject();
            doc.SimpleDTO = new SimpleDTO()
            {
                StringProperty = "A"
            };


            // create patch
            JsonPatchDocument patchDoc = new JsonPatchDocument();
            patchDoc.Remove("Simpledto/stringProperty");

            var serialized = JsonConvert.SerializeObject(patchDoc);
            var deserialized = JsonConvert.DeserializeObject<JsonPatchDocument>(serialized);


            deserialized.ApplyTo(doc);

            Assert.Equal(null, doc.SimpleDTO.StringProperty);

        }




        [Fact]
        public void NestedRemoveFromList()
        {
            dynamic doc = new ExpandoObject();
            doc.SimpleDTO = new SimpleDTO()
                {
                    IntegerList = new List<int>() { 1, 2, 3 }
                };
          


            // create patch
            JsonPatchDocument patchDoc = new JsonPatchDocument();
            patchDoc.Remove("SimpleDTO/IntegerList/2");

            var serialized = JsonConvert.SerializeObject(patchDoc);
            var deserialized = JsonConvert.DeserializeObject<JsonPatchDocument>(serialized);

            deserialized.ApplyTo(doc);

            Assert.Equal(new List<int>() { 1, 2 }, doc.SimpleDTO.IntegerList);
        }



        [Fact]
        public void NestedRemoveFromListMixedCase()
        {
            dynamic doc = new ExpandoObject();
            doc.SimpleDTO = new SimpleDTO()
            {
                IntegerList = new List<int>() { 1, 2, 3 }
            };



            // create patch
            JsonPatchDocument patchDoc = new JsonPatchDocument();
            patchDoc.Remove("SimpleDTO/Integerlist/2");

            var serialized = JsonConvert.SerializeObject(patchDoc);
            var deserialized = JsonConvert.DeserializeObject<JsonPatchDocument>(serialized);

            deserialized.ApplyTo(doc);

            Assert.Equal(new List<int>() { 1, 2 }, doc.SimpleDTO.IntegerList);
        }



        [Fact]
        public void NestedRemoveFromListInvalidPositionTooLarge()
        {
            dynamic doc = new ExpandoObject();
            doc.SimpleDTO = new SimpleDTO()
                {
                    IntegerList = new List<int>() { 1, 2, 3 }
                };
        

            // create patch
            JsonPatchDocument patchDoc = new JsonPatchDocument();
            patchDoc.Remove("SimpleDTO/IntegerList/3");

            var serialized = JsonConvert.SerializeObject(patchDoc);
            var deserialized = JsonConvert.DeserializeObject<JsonPatchDocument>(serialized);

            Assert.Throws<JsonPatchException>(() => { deserialized.ApplyTo(doc); });

        }



        [Fact]
        public void NestedRemoveFromListInvalidPositionTooSmall()
        {

            dynamic doc = new ExpandoObject();
            doc.SimpleDTO = new SimpleDTO()
                {
                    IntegerList = new List<int>() { 1, 2, 3 }
                };

            // create patch
            JsonPatchDocument patchDoc = new JsonPatchDocument();
            patchDoc.Remove("SimpleDTO/IntegerList/-1");

            var serialized = JsonConvert.SerializeObject(patchDoc);
            var deserialized = JsonConvert.DeserializeObject<JsonPatchDocument>(serialized);

            Assert.Throws<JsonPatchException>(() => { deserialized.ApplyTo(doc); });

        }


        [Fact]
        public void NestedRemoveFromEndOfList()
        {
            dynamic doc = new ExpandoObject();
            doc.SimpleDTO = new SimpleDTO()
                {
                    IntegerList = new List<int>() { 1, 2, 3 }
                };

            // create patch
            JsonPatchDocument patchDoc = new JsonPatchDocument();
            patchDoc.Remove("SimpleDTO/IntegerList/-");

            var serialized = JsonConvert.SerializeObject(patchDoc);
            var deserialized = JsonConvert.DeserializeObject<JsonPatchDocument>(serialized);

            deserialized.ApplyTo(doc);

            Assert.Equal(new List<int>() { 1, 2 }, doc.SimpleDTO.IntegerList);

        }



    }
}
