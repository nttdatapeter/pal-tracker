using System;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PalTracker;
using Xunit;

namespace PalTrackerTests
{
    [Collection("Integration")]
    public class TimeEntryIntegrationTest
    {

        private readonly HttpClient _testClient;

        public TimeEntryIntegrationTest()
        {
            Environment.SetEnvironmentVariable("MYSQL__CLIENT__CONNECTIONSTRING", DbTestSupport.TestDbConnectionString);
            _testClient = IntegrationTestServer.Start().CreateClient();
            DbTestSupport.ExecuteSql("TRUNCATE TABLE time_entries");
        }

        [Fact]
        public void Read()
        {
            var id = CreateTimeEntry(new TimeEntry(999, 1010,  new DateTime(2015, 10, 10), 9));

            var response = _testClient.GetAsync($"/time-entries/{id}").Result;
            var responseBody = JObject.Parse(response.Content.ReadAsStringAsync().Result);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(id, responseBody["id"].ToObject<long>());
            Assert.Equal(999, responseBody["projectId"].ToObject<long>());
            Assert.Equal(1010, responseBody["userId"].ToObject<long>());
            Assert.Equal("10/10/2015 00:00:00", responseBody["date"].ToObject<string>());
            Assert.Equal(9, responseBody["hours"].ToObject<int>());
        }

        [Fact]
        public void Create()
        {
            var timeEntry = new TimeEntry(222, 333,  new DateTime(2008, 01, 08), 24);

            var response = _testClient.PostAsync("/time-entries", SerializePayload(timeEntry)).Result;
            var responseBody = JObject.Parse(response.Content.ReadAsStringAsync().Result);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.True(responseBody["id"].ToObject<long>() > 0);
            Assert.Equal(222, responseBody["projectId"].ToObject<long>());
            Assert.Equal(333, responseBody["userId"].ToObject<long>());
            Assert.Equal("01/08/2008 00:00:00", responseBody["date"].ToObject<string>());
            Assert.Equal(24, responseBody["hours"].ToObject<int>());
        }

        [Fact]
        public void List()
        {
            var id1 = CreateTimeEntry(new TimeEntry(222, 333,  new DateTime(2008, 01, 08), 24));
            var id2 = CreateTimeEntry(new TimeEntry(444, 555,  new DateTime(2008, 02, 10), 6));

            var response = _testClient.GetAsync("/time-entries").Result;
            var responseBody = JArray.Parse(response.Content.ReadAsStringAsync().Result);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Assert.Equal(id1, responseBody[0]["id"].ToObject<int>());
            Assert.Equal(222, responseBody[0]["projectId"].ToObject<long>());
            Assert.Equal(333, responseBody[0]["userId"].ToObject<long>());
            Assert.Equal("01/08/2008 00:00:00", responseBody[0]["date"].ToObject<string>());
            Assert.Equal(24, responseBody[0]["hours"].ToObject<int>());

            Assert.Equal(id2, responseBody[1]["id"].ToObject<int>());
            Assert.Equal(444, responseBody[1]["projectId"].ToObject<long>());
            Assert.Equal(555, responseBody[1]["userId"].ToObject<long>());
            Assert.Equal("02/10/2008 00:00:00", responseBody[1]["date"].ToObject<string>());
            Assert.Equal(6, responseBody[1]["hours"].ToObject<int>());
        }

        [Fact]
        public void Update()
        {
            var id = CreateTimeEntry(new TimeEntry(222, 333,  new DateTime(2008, 01, 08), 24));
            var updated = new TimeEntry(999, 888,  new DateTime(2012, 08, 12), 2);

            var putResponse = _testClient.PutAsync($"/time-entries/{id}", SerializePayload(updated)).Result;
            var getResponse = _testClient.GetAsync($"/time-entries/{id}").Result;
            var getAllResponse = _testClient.GetAsync("/time-entries").Result;

            Assert.Equal(HttpStatusCode.OK, putResponse.StatusCode);
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
            Assert.Equal(HttpStatusCode.OK, getAllResponse.StatusCode);

            var getAllResponseBody = JArray.Parse(getAllResponse.Content.ReadAsStringAsync().Result);

            Assert.Equal(1, getAllResponseBody.Count);
            Assert.Equal(id, getAllResponseBody[0]["id"].ToObject<int>());
            Assert.Equal(999, getAllResponseBody[0]["projectId"].ToObject<long>());
            Assert.Equal(888, getAllResponseBody[0]["userId"].ToObject<long>());
            Assert.Equal("08/12/2012 00:00:00", getAllResponseBody[0]["date"].ToObject<string>());
            Assert.Equal(2, getAllResponseBody[0]["hours"].ToObject<int>());

            var getResponseBody = JObject.Parse(getResponse.Content.ReadAsStringAsync().Result);

            Assert.Equal(id, getResponseBody["id"].ToObject<int>());
            Assert.Equal(999, getResponseBody["projectId"].ToObject<long>());
            Assert.Equal(888, getResponseBody["userId"].ToObject<long>());
            Assert.Equal("08/12/2012 00:00:00", getResponseBody["date"].ToObject<string>());
            Assert.Equal(2, getResponseBody["hours"].ToObject<int>());
        }

        [Fact]
        public void Delete()
        {
            var id = CreateTimeEntry(new TimeEntry(222, 333,  new DateTime(2008, 01, 08), 24));

            var deleteResponse = _testClient.DeleteAsync($"/time-entries/{id}").Result;
            var getResponse = _testClient.GetAsync($"/time-entries/{id}").Result;
            var getAllResponse = _testClient.GetAsync("/time-entries").Result;

            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
            Assert.Equal("[]", getAllResponse.Content.ReadAsStringAsync().Result);
        }

        private long CreateTimeEntry(TimeEntry timeEntry)
        {
            var response = _testClient.PostAsync("/time-entries", SerializePayload(timeEntry)).Result;

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var responseBody = JObject.Parse(response.Content.ReadAsStringAsync().Result);

            return responseBody["id"].ToObject<long>();
        }

        private static HttpContent SerializePayload(TimeEntry timeEntry) => new StringContent(
            JsonConvert.SerializeObject(timeEntry),
            Encoding.UTF8,
            "application/json"
        );
    }
}