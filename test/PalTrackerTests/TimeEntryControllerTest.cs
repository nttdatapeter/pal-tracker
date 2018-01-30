using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PalTracker;
using Xunit;

namespace PalTrackerTests
{
    public class TimeEntryControllerTest
    {
        private readonly TimeEntryController _controller;
        private readonly Mock<ITimeEntryRepository> _repository;

        public TimeEntryControllerTest()
        {
            _repository = new Mock<ITimeEntryRepository>();
            _controller = new TimeEntryController(_repository.Object);
        }

        [Fact]
        public void Read()
        {
            var expected = new TimeEntry(1, 222, 333, new DateTime(2008, 08, 01, 12, 00, 01), 24);
            _repository.Setup(r => r.Contains(1)).Returns(true);
            _repository.Setup(r => r.Find(1)).Returns(expected);

            var response = _controller.Read(1);

            Assert.IsType<OkObjectResult>(response);

            var typedResponse = response as OkObjectResult;

            Assert.Equal(expected, typedResponse.Value);
            Assert.Equal(200, typedResponse.StatusCode);
        }

        [Fact]
        public void Read_NotFound()
        {
            _repository.Setup(r => r.Contains(1)).Returns(false);

            var response = _controller.Read(1);

            Assert.IsType<NotFoundResult>(response);

            var typedResponse = response as NotFoundResult;

            Assert.Equal(404, typedResponse.StatusCode);
        }

        [Fact]
        public void Create()
        {
            var toCreate = new TimeEntry(222, 333, new DateTime(2008, 08, 01, 12, 00, 01), 24);
            var expected = new TimeEntry(1, 222, 333, new DateTime(2008, 08, 01, 12, 00, 01), 24);
            _repository.Setup(r => r.Create(toCreate)).Returns(expected);

            var response = _controller.Create(toCreate);

            Assert.IsType<CreatedAtRouteResult>(response);

            var typedResponse = response as CreatedAtRouteResult;

            Assert.Equal(201, typedResponse.StatusCode);
            Assert.Equal("GetTimeEntry", typedResponse.RouteName);
            Assert.Equal(expected, typedResponse.Value);
        }

        [Fact]
        public void List()
        {
            var timeEntries = new List<TimeEntry>
            {
                new TimeEntry(1, 222, 333, new DateTime(2008, 08, 01, 12, 00, 01), 24),
                new TimeEntry(2, 999, 888, new DateTime(2018, 12, 05, 23, 00, 01), 8)
            };

            _repository.Setup(r => r.List()).Returns(timeEntries);

            var response = _controller.List();

            Assert.IsType<OkObjectResult>(response);

            var typedResponse = response as OkObjectResult;

            Assert.Equal(timeEntries, typedResponse.Value);
            Assert.Equal(200, typedResponse.StatusCode);
        }

        [Fact]
        public void Update()
        {
            var theUpdate = new TimeEntry(999, 888, new DateTime(2018, 12, 05, 23, 00, 01), 8);
            var updated = new TimeEntry(1, 999, 888, new DateTime(2018, 12, 05, 23, 00, 01), 8);

            _repository.Setup(r => r.Update(1, theUpdate)).Returns(updated);
            _repository.Setup(r => r.Contains(1)).Returns(true);

            var response = _controller.Update(1, theUpdate);

            Assert.IsType<OkObjectResult>(response);

            var typedResponse = response as OkObjectResult;

            Assert.Equal(updated, typedResponse.Value);
            Assert.Equal(200, typedResponse.StatusCode);
        }

        [Fact]
        public void Update_NotFound()
        {
            var theUpdate = new TimeEntry(999, 888, new DateTime(2018, 12, 05, 23, 00, 01), 8);

            _repository.Setup(r => r.Contains(1)).Returns(false);

            var response = _controller.Update(1, theUpdate);

            Assert.IsType<NotFoundResult>(response);

            var typedResponse = response as NotFoundResult;

            Assert.Equal(404, typedResponse.StatusCode);
        }

        [Fact]
        public void Delete()
        {
            _repository.Setup(r => r.Contains(1)).Returns(true);
            _repository.Setup(r => r.Delete(1));

            var response = _controller.Delete(1);

            Assert.IsType<NoContentResult>(response);

            var typedResponse = response as NoContentResult;

            Assert.Equal(204, typedResponse.StatusCode);
        }

        [Fact]
        public void Delete_NotFound()
        {
            _repository.Setup(r => r.Contains(1)).Returns(false);

            var response = _controller.Delete(1);

            Assert.IsType<NotFoundResult>(response);

            var typedResponse = response as NotFoundResult;

            Assert.Equal(404, typedResponse.StatusCode);
        }
    }
}