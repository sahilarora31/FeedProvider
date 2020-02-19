using Newtonsoft.Json.Linq;
using SubmissionProvider.DataModel;
using SubmissionProvider.Exceptions;
using SubmissionProvider.Interfaces;
using SubmissionProvider.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static SubmissionProvider.Constants;

namespace SubmissionProvider
{
    public class FeedSubmissionProvider
    {
        private ILogger _logger;
        private FileParser _FileParser;
        public FeedSubmissionProvider(ILogger logger, FileParser FileParser)
        {
            _logger = logger;
            _FileParser = FileParser;
        }

        public void ProcessFeeds(string path)
        {
            _logger.Log("\nFeed Processing started...");

            if (!File.Exists(path))
            {
                throw new InvalidFilePathException();
            }

            string[] sources = File.ReadAllLines(path);

            foreach (string source in sources)
            {
                _logger.Log($"\nProcessing file {source} ...");

                if (!ValidationUtility.ValidateSource(source))
                {
                    throw new InvalidSourceException(source);
                }
                else
                {
                    string[] sourceArr = source.Split(',');
                    string provider = sourceArr[0];
                    string sourcePath = sourceArr[1];
                    if( UpdateInventory(provider, sourcePath))
                        _logger.Log($"\nSuccessfully processed : {provider}...");
                }
            }
            _logger.Log("\nFeed Processing completed...");
        }
        public bool UpdateInventory(string provider, string path)
        {

            string extn = Path.GetExtension(path);
            string ext = extn.Substring(1).ToUpper();
            if (!Enum.TryParse(ext, out FileType fileType))
            {
                _logger.Log("\nInvalid File exension");
                return false;
            }

            if (!Enum.TryParse(provider, out ProviderEnum FeedSubmissionProvider))
            {
                _logger.Log("\nInvalid Feed Provider");
                return false;
            }

            if (!File.Exists(path))
            {
                throw new InvalidFilePathException();
            }

            IFileParser parser = _FileParser.GetParser(fileType);


            switch (FeedSubmissionProvider)
            {
                case ProviderEnum.Capterra:
                    var capterraObject = parser.ParseFile(path);
                    List<Capterra> capterraList = GetCapterraList(capterraObject);
                    //Persist to DB

                    break;
                case ProviderEnum.SoftwareAdvice:
                    var saObject = parser.ParseFile(path);
                    List<SoftwareAdvice> saList = GetSoftwareAdviceList(saObject);
                    //Persist to DB
                    break;

            }
            return true;
        }
        internal List<Capterra> GetCapterraList(object data)
        {
            List<Capterra> result = new List<Capterra>();
            try
            {
                List<object> capteraObjList = (List<object>)data;
                foreach (var capterra in capteraObjList)
                {
                    var dict = ((Dictionary<object, object>)capterra).ToDictionary(k => k.Key.ToString(), k => k.Value.ToString());
                    result.Add(new Capterra(dict));
                }
            }
            catch (Exception ex)
            {
                _logger.Log($"\nError Parsing Capterra Object {ex}");
            }
            return result;

        }
        internal List<SoftwareAdvice> GetSoftwareAdviceList(object data)
        {
            List<SoftwareAdvice> result = new List<SoftwareAdvice>();
            try
            {
                var resultObjects = (JArray)JObject.Parse(data.ToString()).SelectToken("products");

                foreach (JObject jObj in resultObjects)
                {
                    result.Add(new SoftwareAdvice(jObj));
                }
            }

            catch (Exception ex)
            {
                _logger.Log($"\nError Parsing Software Advice Object {ex}");
            }
            return result;
        }
    }
}
