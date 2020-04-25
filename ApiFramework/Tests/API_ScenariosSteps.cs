﻿using ApiFramework.Helper;
using ApiFramework.TestClass;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Text;
using TechTalk.SpecFlow;

namespace ApiFramework.Tests
{
    [Binding]
    class API_ScenariosSteps
    {
        public string inputParameters;
        public JsonHelper jsonHelper = new JsonHelper();
        public WebClientHelper clientHelper = new WebClientHelper();
        public TestHelper testHelper = new TestHelper();
        public string requestType;
        public string baseUrl;
        public string response;

        [Given(@"I have a '(.*)' API '(.*)'")]
        public void GivenIHaveAAPI(string httpVerb, string API)
        {
            requestType = httpVerb;
            baseUrl = jsonHelper.GetURLsByEnvironment(API);
        }

        [Given(@"I have a json input file")]
        public void GivenIHaveAJsonInputFile(Table table)
        {
            inputParameters = jsonHelper.ReadJsonFile(table);  // Passing table
        }

        [Given(@"I have a json input file '(.*)'")]
        public void GivenIHaveAJsonInputFile(string filePath)
        {
            inputParameters = jsonHelper.ReadJsonFile(filePath); // Passing sting (file Path) 
        }

        [Given(@"Authentication Type '(.*)'")]
        public void GivenAuthenticationType(string authenticationType)
        {
            clientHelper.GetAuthorization(authenticationType); // call get Authorization method.
        }

        [Then(@"I receive API response")]
        public void ThenIReceiveAPIResponse()
        {
            response = clientHelper.GetResponse(requestType, baseUrl, inputParameters);
        }

        [Then(@"I expect status code '(.*)'")]
        public void ThenIExpectStatusCode(int expectedStatusCode)
        {
            if (testHelper.verifyApiResponseStatusCode("404", response) == false)
                Hooks.test.Fail("Response Status Code Mismatch."); // can use Assert.Fail("Response Status Code Mismatch");
            else
                Hooks.test.Pass("Response Statuc Code Matched Successfully.");

        }

        [Then(@"I verify json response body")]
        public void ThenIVerifyJsonResponseBody(Table table)
        {
            string expectedResponse = jsonHelper.ReadJsonFile(table);
            if (testHelper.verifyApiJsonResponse(expectedResponse, response) == false)
                Hooks.test.Fail("Json Response Mismatch."); // can use Assert.Fail("Json Response Mismatch.");
            else
                Hooks.test.Pass("Json Response Matched Successfully.");

        }

    }
}
