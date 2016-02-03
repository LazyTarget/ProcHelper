﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using FullCtrl.API.Interfaces;
using FullCtrl.API.v1.Models;
using FullCtrl.Base;

namespace FullCtrl.API.v1.Controllers
{
    public class WinServiceController : BaseController
    {
        private void AppendLinks(IResponseBase response)
        {
            var service = response.Result as WinServiceDto;
            var serviceName = service?.ServiceName;
            if (service == null || string.IsNullOrWhiteSpace(serviceName))
                return;

            var rootUri = response.Links["root"];
            response.Links["get-service"] =         Link.FromUri(new Uri(rootUri.Href + "/winservice/get/" + serviceName));
            response.Links["start-service"] =       Link.FromUri(new Uri(rootUri.Href + "/winservice/start/" + serviceName));
            response.Links["pause-service"] =       Link.FromUri(new Uri(rootUri.Href + "/winservice/pause/" + serviceName));
            response.Links["continue-service"] =    Link.FromUri(new Uri(rootUri.Href + "/winservice/continue/" + serviceName));
            response.Links["stop-service"] =        Link.FromUri(new Uri(rootUri.Href + "/winservice/stop/" + serviceName));
        }


        [HttpGet, HttpPost, HttpPut]
        [Route("api/v1/winservice")]
        [Route("api/v1/winservice/{serviceName}")]
        [Route("api/v1/winservice/get/{serviceName}")]
        public IResponseBase<WinServiceDto> Get(string serviceName)
        {
            var result = WinServiceHelper.GetService(serviceName);

            var response = CreateResponse(result);
            AppendLinks(response);
            return response;
        }


        [HttpGet, HttpPost, HttpPut]
        [Route("api/v1/winservice")]
        [Route("api/v1/winservice/list")]
        public IResponseBase<IEnumerable<WinServiceDto>> List()
        {
            var result = WinServiceHelper.GetServices();

            var response = CreateResponse<IEnumerable<WinServiceDto>>(result);
            return response;
        }


        [HttpGet, HttpPost, HttpPut]
        [Route("api/v1/winservice/list")]
        [Route("api/v1/winservice/list/{serviceName}")]
        public IResponseBase<IEnumerable<WinServiceDto>> ListByName(string serviceName)
        {
            var result = WinServiceHelper.GetServices();
            if (result != null && !string.IsNullOrEmpty(serviceName))
                result = result.Where(x => x.ServiceName.Equals(serviceName, StringComparison.InvariantCultureIgnoreCase)).ToList();

            var response = CreateResponse<IEnumerable<WinServiceDto>>(result);
            return response;
        }


        [HttpGet, HttpPost, HttpPut]
        [Route("api/v1/winservice/find")]
        [Route("api/v1/winservice/find/{serviceName}")]
        public IResponseBase<IEnumerable<WinServiceDto>> FindByName(string serviceName)
        {
            var result = WinServiceHelper.GetServices();
            if (result != null && !string.IsNullOrEmpty(serviceName))
                result = result.Where(x => x.ServiceName.IndexOf(serviceName, StringComparison.InvariantCultureIgnoreCase) >= 0).ToList();

            var response = CreateResponse<IEnumerable<WinServiceDto>>(result);
            return response;
        }


        [HttpGet, HttpPost, HttpPut]
        [Route("api/v1/winservice/find/status")]
        [Route("api/v1/winservice/find/status/{status}")]
        public IResponseBase<IEnumerable<WinServiceDto>> FindByStatus(ServiceControllerStatus status)
        {
            var result = WinServiceHelper.GetServices();
            if (result != null)
                result = result.Where(x => x.Status == status).ToList();

            var response = CreateResponse<IEnumerable<WinServiceDto>>(result);
            return response;
        }


        [HttpGet, HttpPost, HttpPut]
        [Route("api/v1/winservice/start")]
        [Route("api/v1/winservice/start/{serviceName}")]
        [Route("api/v1/winservice/start/{serviceName}/{arguments}")]
        public IResponseBase<WinServiceDto> Start(string serviceName, params string[] arguments)
        {
            var result = WinServiceHelper.StartService(serviceName);

            var response = CreateResponse(result);
            AppendLinks(response);
            return response;
        }


        [HttpGet, HttpPost, HttpPut]
        [Route("api/v1/winservice/pause")]
        [Route("api/v1/winservice/pause/{serviceName}")]
        public IResponseBase<WinServiceDto> Pause(string serviceName)
        {
            var result = WinServiceHelper.PauseService(serviceName);

            var response = CreateResponse(result);
            AppendLinks(response);
            return response;
        }


        [HttpGet, HttpPost, HttpPut]
        [Route("api/v1/winservice/continue")]
        [Route("api/v1/winservice/continue/{serviceName}")]
        public IResponseBase<WinServiceDto> Continue(string serviceName)
        {
            var result = WinServiceHelper.ContinueService(serviceName);

            var response = CreateResponse(result);
            AppendLinks(response);
            return response;
        }


        [HttpGet, HttpPost, HttpPut]
        [Route("api/v1/winservice/stop")]
        [Route("api/v1/winservice/stop/{serviceName}")]
        public IResponseBase<WinServiceDto> Stop(string serviceName)
        {
            var result = WinServiceHelper.StopService(serviceName);

            var response = CreateResponse(result);
            AppendLinks(response);
            return response;
        }
    }
}
