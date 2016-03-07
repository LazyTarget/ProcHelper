﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web.Http;
using FullCtrl.Base;

namespace FullCtrl.API.v1.Controllers
{
    public class DeviceController : BaseController
    {
        private readonly DeviceHelper _deviceHelper;

        public DeviceController()
        {
            _deviceHelper = new DeviceHelper();
        }
        

        [HttpPost, HttpPut]
        [Route("api/v1/device/take/screenshot")]
        public IResponseBase<object> TakeScreenshot()
        {
            var image = _deviceHelper.TakeScreenshot();

            //object result = image;
            //var result = image;
            var stream = new MemoryStream();
            image.Save(stream, ImageFormat.Jpeg);
            object result = stream.ToArray();

            result = image;

            var response = CreateResponse(result);
            return response;
        }

    }
}
