using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using FullCtrl.Base;

namespace FullCtrl.Web.Controllers
{
    public class DashboardController : Controller
    {
        private FullCtrlAPI _api;

        public DashboardController()
        {
            _api = new FullCtrlAPI();
        }

        public ActionResult Index()
        {
            return View("Dashboard");
        }


        public ActionResult Screenshot()
        {
            // todo: cache
            
            Bitmap image = null;
            byte[] buffer = new byte[0];
            try
            {
                var source = new CancellationTokenSource();
                var task = _api.DeviceController.TakeScreenshot();
                task.ContinueWith((t) =>
                {

                });
                while (!task.IsCompleted)
                {
                    task.Wait((int) TimeSpan.FromSeconds(60).TotalMilliseconds, source.Token);
                }
                var res = task.Result?.Result;
                image = res as Bitmap;
                if (res is byte[])
                    buffer = (byte[]) res;
            }
            catch (Exception ex)
            {
                
            }
            
            var stream = new MemoryStream(buffer);
            if (image != null)
            {
                image.Save(stream, ImageFormat.Jpeg);
            }
            else
            {
                //return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Image not found");
            }


            var contentType = "image/jpeg";
            var result = new FileStreamResult(stream, contentType);
            return result;
        }



        public async Task<ActionResult> ScreenshotAsync()
        {
            // todo: cache
            
            Bitmap image = null;
            byte[] buffer = new byte[0];
            try
            {
                var task = _api.DeviceController.TakeScreenshot();
                task.ContinueWith((t) =>
                {

                });
                var r = await task;
                var res = r.Result;
                image = res as Bitmap;
                if (res is byte[])
                    buffer = (byte[]) res;
                if (res is string)
                    buffer = Convert.FromBase64String((string)res);
            }
            catch (Exception ex)
            {
                
            }
            
            var stream = new MemoryStream(buffer);
            if (image != null)
            {
                image.Save(stream, ImageFormat.Jpeg);
            }
            else
            {
                //return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Image not found");
            }


            var contentType = "image/jpeg";
            var result = new FileStreamResult(stream, contentType);
            return result;
        }


        public async Task<ActionResult> ScreenshotBase64Async()
        {
            // todo: cache

            Bitmap image = null;
            byte[] buffer = new byte[0];
            try
            {
                var task = _api.DeviceController.TakeScreenshot();
                task.ContinueWith((t) =>
                {

                });
                var r = await task;
                var res = r.Result;
                image = res as Bitmap;
                if (res is byte[])
                    buffer = (byte[])res;
                if (res is string)
                    buffer = Convert.FromBase64String((string) res);
            }
            catch (Exception ex)
            {

            }

            var stream = new MemoryStream(buffer);
            if (image != null)
            {
                image.Save(stream, ImageFormat.Jpeg);
            }
            else
            {
                //return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Image not found");
            }


            string contentType = null;
            var result = new ContentResult
            {
                Content = "data:image/jpeg;base64," + Convert.ToBase64String(buffer),
                ContentType = contentType,
            };
            return result;
        }
    }
}