using Kiru8th.JWT;
using Kiru8th.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Kiru8th.Controllers
{
    public class UploadController : ApiController
    {
        KiruDb db = new KiruDb();
        /// <summary>
        /// 圖片上傳專區
        /// </summary>
        /// <returns></returns>
        [Route("upload")]
        [HttpPost]
        [JwtAuthFilter]
        public async Task<IHttpActionResult> Upload()
        {
            if (!this.Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Pic");
            var exists = Directory.Exists(root);
            if (!exists)
            {
                Directory.CreateDirectory("Pic");
            }

            try
            {


                var provider = new MultipartMemoryStreamProvider();
                await this.Request.Content.ReadAsMultipartAsync(provider);
                string uploadfilename = "";

                //var uploadResponse = new UploadResponse();
                foreach (var content in provider.Contents)
                {

                    var KeyName = content.Headers.ContentDisposition.Name.Trim('\"');
                    if (KeyName.Contains("photo"))
                    {
                        var fileName = content.Headers.ContentDisposition.FileName.Trim('\"');
                        string[] fileary = fileName.Split('.');
                        uploadfilename = "Photo" + DateTime.Now.ToFileTime() + "." + fileary[1];
                        var fileBytes = await content.ReadAsByteArrayAsync();

                        var outputPath = Path.Combine(root, uploadfilename);
                        using (var output = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                        {
                            await output.WriteAsync(fileBytes, 0, fileBytes.Length);
                        }


                    }
                    else
                    {

                        continue;
                    }

                    //uploadResponse.Names.Add(fileName);
                    //uploadResponse.FileNames.Add(outputPath);
                    //uploadResponse.ContentTypes.Add(content.Headers.ContentType.MediaType);
                }

                return Ok(new
                {
                    success = true,
                    picname = uploadfilename
                });
                //return this.Ok(uploadResponse);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(e.Message)
                });
            }
        }
    }
}
