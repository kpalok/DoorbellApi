using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoorbellApi.Controllers
{
    [Route("download")]
    [ApiController]
    public class DownloadController : ControllerBase
    {

        [HttpGet("homeclient")]
        public FileResult DownloadHomeClient()
        {
            string filename = "kmakihima.apk";
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "/clients/" + filename;
            byte[] filedata = System.IO.File.ReadAllBytes(filepath);
            string contentType = "application/vnd.android.package-archive";

            var cd = new System.Net.Mime.ContentDisposition()
            {
                FileName = filename,
                Inline = true
            };

            Response.Headers["Content-Disposition"] = cd.ToString();

            return File(filedata, contentType);
        }

        [HttpGet("guestclient")]
        public FileResult DownloadGuestClient()
        {
            string filename = "kmakiguest.apk";
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "/clients/" + filename;
            byte[] filedata = System.IO.File.ReadAllBytes(filepath);
            string contentType = "application/vnd.android.package-archive";

            var cd = new System.Net.Mime.ContentDisposition()
            {
                FileName = filename,
                Inline = true
            };

            Response.Headers["Content-Disposition"] = cd.ToString();

            return File(filedata, contentType);
        }
    }
}
