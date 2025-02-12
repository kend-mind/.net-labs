using System;
using System.Net.Http;
using Serilog;
using System.Net;
using System.IO;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using VPBANK.RMD.API.Settings;
using VPBANK.RMD.API.Common.Controllers;
using VPBANK.RMD.API.Common.Middlewares;
using VPBANK.RMD.Utils.Common;
using VPBANK.RMD.Utils.Notification.Publisher;
using VPBANK.RMD.Repositories.PhoenixConf.Interfaces.App;

namespace VPBANK.RMD.API.IFRS9.Controllers.Common
{
    [Authorize]
    public class FileController : BaseController
    {
        //private const int MaxFileSize = 10L * 1024L * 1024L * 1024L; // 10GB, adjust to your need
        public FileController(IMemoryCache memoryCache,
            IConfiguration configuration,
            IWebHostEnvironment env,
            IAppSettingsReader appSettings,
            IHttpClientFactory httpClientFactory,
            IMapper mapper,
            IRabbitMqPublisher rabbitManager,
            ISubscriberInfoRepository subscriberRepository) : base(memoryCache, configuration, env, appSettings, httpClientFactory, mapper, rabbitManager, subscriberRepository)
        {
        }

        /// <summary>
        /// 
        /// FileStreamResult – Sends binary content to the response by using a Stream instance. Here you have a stream and want to return stream content as a file.
        /// FileContentResult – Sends the contents of a binary file to the response.Here you have a byte array and want to return byte content as a file.
        /// 
        /// MIME Types for files upload and download
        /// Below are a few examples of MIME – content types for your information.
        /// 
        /// ALL files: mimeType = "application/octet-stream";
        /// 
        /// Text File – "text/plain"
        /// PNG image – "image/png"
        /// JPEG image – "image/jpeg"
        /// MSDoc – "application/vnd.ms-word"
        /// MSDoc – "application/vnd.ms-word"
        /// PDF file – "application/pdf"
        /// Excel File- "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>FileStreamResult</returns>
        [HttpGet]
        [Route("{fileName}")]
        public FileStreamResult Download(string fileName)
        {
            try
            {
                string path = this._env.WebRootPath + "\\Files\\" + fileName;
                var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                var fileStream = new FileStreamResult(stream, MediaTypeNames.Application.Octet)
                {
                    FileDownloadName = fileName
                };
                return fileStream;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
                Log.Error(ex.InnerException?.Message);
                if (ex is HttpErrorException)
                    throw;
                else
                    throw new HttpErrorException(HttpStatusCode.InternalServerError, nameof(HttpStatusCode.InternalServerError), string.Format(ErrorMessages.SE000, ex.Message));
            }
            finally
            {
            }
        }

        /// <summary>
        /// Save document in wwwroot folder.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{fileName}")]
        [Consumes(MediaTypeNames.Application.Octet)]
        public IActionResult Upload([FromBody] byte[] file, string fileName)
        {
            try
            {
                Stream stream = new MemoryStream(file);
                // Reset the position of the stream
                stream.Position = 0;
                // You have to rewind the MemoryStream before copying
                stream.Seek(0, SeekOrigin.Begin);

                //StreamWriter writer = new StreamWriter(stream);
                //writer.WriteLine("Append new line.");
                //writer.WriteLine("End.");
                //writer.Flush();

                if (!Directory.Exists(_env.WebRootPath))
                    Directory.CreateDirectory(_env.WebRootPath);
                using (FileStream fs = new FileStream($"{_env.WebRootPath}\\{fileName}", FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    stream.CopyTo(fs);
                    fs.Flush();
                    fs.Close();
                    stream.Close();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
                Log.Error(ex.InnerException?.Message);
                if (ex is HttpErrorException)
                    throw;
                else
                    throw new HttpErrorException(HttpStatusCode.InternalServerError, nameof(HttpStatusCode.InternalServerError), string.Format(ErrorMessages.SE000, ex.Message));
            }
            finally
            {
            }
        }
    }
}