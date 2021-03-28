using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Qiniu.Common;
using Qiniu.Http;
using Qiniu.IO;
using Qiniu.IO.Model;
using Qiniu.Util;
using StudentApiV2.Config;
using StudentApiV2.Dtos;
using StudentApiV2.UpdateDtos;

namespace StudentApiV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : BaseController
    {
        private readonly QnySetting _Qny;

        public ImagesController(IOptions<QnySetting> Qny)
        {
            _Qny = Qny.Value;
        }

        [HttpPost]
        public UploadQiNiuResult UploadImage()
        {
            IFormFileCollection files = Request.Form.Files;
            var res = Request.Form.ToArray();
            List<Object> list = new List<Object>();
            var file = files[0];
            var filename = ContentDispositionHeaderValue
                                  .Parse(file.ContentDisposition)
                                  .FileName;

            Mac mac = new Mac(_Qny.qiniuyunAK, _Qny.qiniuyunSK);
            
           
            var key = _Qny.prefixPath + "/" + filename;//重命名文件加上时间戳
            //string key = image.ImageName;
            // 本地文件路径
            //string filePath = image.ImagePath;
            // 存储空间名
            string Bucket = "daytsg";
            // 设置上传策略
            PutPolicy putPolicy = new PutPolicy();
            // 设置要上传的目标空间
            putPolicy.Scope = Bucket + ":" + key;
            // 上传策略的过期时间(单位:秒)
            putPolicy.SetExpires(3600);
            // 文件上传完毕后，在多少天后自动被删除
            putPolicy.DeleteAfterDays = 60;
            // 生成上传token
            string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());

            Qiniu.Common.Config.AutoZone(_Qny.qiniuyunAK, Bucket, false);

            UploadManager um = new UploadManager();
            //HttpResult result = um.UploadFile(filePath, key, token);
            Stream stream = file.OpenReadStream();
            HttpResult result = um.UploadStream(stream, key, token);

            if (result.Code == 200)
            {
                return JsonConvert.DeserializeObject<UploadQiNiuResult>(result.Text);
            }
            return null;
        }

    }
}
