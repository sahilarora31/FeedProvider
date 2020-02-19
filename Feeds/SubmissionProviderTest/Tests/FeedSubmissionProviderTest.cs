﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SubmissionProvider;
using SubmissionProvider.DataModel;
using SubmissionProvider.Exceptions;
using SubmissionProvider.Interfaces;
using System.Collections.Generic;

namespace SubmissionProviderTest
{
    [TestClass]
    public class FeedSubmissionProviderTest
    {
        ILogger logger;
        FileParser FileParser;
        FeedSubmissionProvider FeedSubmissionProvider;

        [TestInitialize]
        public void Setup()
        {
            logger = new Logger();
            FileParser = new FileParser();
            FeedSubmissionProvider = new FeedSubmissionProvider(logger, FileParser);
        }
        
        [TestMethod]
        [DataRow("Capterra", "./capterra.yaml",true)]
        [DataRow("Capterra", "./capterra.xml", false)]
        [DataRow("WrongProvider", "./capterra.yaml", false)]
        public void UpdateInventoryYamlTest(string provider, string path, bool result)
        {
            Assert.AreEqual(result, FeedSubmissionProvider.UpdateInventory(provider, path));           
        }

        [TestMethod]
        [DataRow("SoftwareAdvice", "./softwareadvice.json", true)]
        [DataRow("SoftwareAdvice", "./softwareadvice.xml", false)]
        [DataRow("WrongProvider", "./softwareadvice.yaml", false)]
        public void UpdateInventoryJsonTest(string provider, string path, bool result)
        {
            Assert.AreEqual(result, FeedSubmissionProvider.UpdateInventory(provider, path));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidFilePathException))]
        public void FilePathExceptionTest()
        {
            string path = "./badPath.json";
            string provider = "SoftwareAdvice";
            var result = FeedSubmissionProvider.UpdateInventory(provider, path);
        }

        [TestMethod]
        public void JsonParserTest()
        {
            List<SoftwareAdvice> saList = new List<SoftwareAdvice>();
            string[] categories = new string[] { "cat1" };
            saList.Add(new SoftwareAdvice("title", "twitter", categories));
           
            object data = "{\r\n  \"products\": [\r\n    {\r\n      \"categories\": [\r\n        \"cat1\",\r\n    ],\r\n      \"twitter\": \"twitter\",\r\n      \"title\": \"title\"\r\n    },\r\n ]\r\n}";
            CollectionAssert.AreEqual(saList, FeedSubmissionProvider.GetSoftwareAdviceList(data));

        }
        [TestMethod]
        public void YamlParserTest()
        {
            List<Capterra> capterraList = new List<Capterra>();
            capterraList.Add(new Capterra("tags", "name", "twitter"));

            Dictionary<object, object> capterra = new Dictionary<object, object>();
            capterra.Add("name", "name");
            capterra.Add("twitter", "twitter");
            capterra.Add("tags", "tags");
            List<object> data = new List<object>();
            data.Add(capterra);
            CollectionAssert.AreEqual(capterraList, FeedSubmissionProvider.GetCapterraList(data));
        }
    }
}
