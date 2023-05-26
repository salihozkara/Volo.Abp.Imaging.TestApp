using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Content;
using Volo.Abp.Imaging;

namespace Volo.Abp.Image.Controllers;

[Route("api/test")]
public class TestController : ImageController
{
    [HttpPost("compress/formFile")]
    [CompressImage]
    public IActionResult UploadImage(IFormFile file)
    {
        return File(file.OpenReadStream(), file.ContentType);
    }
    
    [HttpPost("compress/stream")]
    [CompressImage]
    public IActionResult UploadImage(Stream stream)
    {
        return File(stream, "image/jpeg");
    }
    
    [HttpPost("compress/bytes")]
    [CompressImage]
    public IActionResult UploadImage(IEnumerable<byte> bytes)
    {
        return File(bytes.ToArray(), "image/jpeg");
    }
    
    [HttpPost("compress/remoteStreamContent")]
    [CompressImage]
    public IActionResult UploadImage(IRemoteStreamContent remoteStreamContent)
    {
        return File(remoteStreamContent.GetStream(), "image/jpeg");
    }
    
    [HttpPost("compress/custom")]
    [CompressImage]
    public IActionResult UploadImage(CustomDto customDto)
    {
        return File(customDto.Stream, "image/jpeg");
    }
    
    [HttpPost("resize/formFile")]
    [ResizeImage(1000)]
    public IActionResult ResizeImage(IFormFile file)
    {
        return File(file.OpenReadStream(), file.ContentType);
    }
    
    [HttpPost("resize/stream")]
    [ResizeImage(1000)]
    public IActionResult ResizeImage(Stream stream)
    {
        return File(stream, "image/jpeg");
    }
    
    [HttpPost("resize/bytes")]
    [ResizeImage(1000)]
    public IActionResult ResizeImage(IEnumerable<byte> bytes)
    {
        return File(bytes.ToArray(), "image/jpeg");
    }
    
    [HttpPost("resize/remoteStreamContent")]
    [ResizeImage(1000)]
    public IActionResult ResizeImage(IRemoteStreamContent remoteStreamContent)
    {
        return File(remoteStreamContent.GetStream(), "image/jpeg");
    }
    
    [HttpPost("resize/custom")]
    [ResizeImage(1000)]
    public IActionResult ResizeImage(CustomDto customDto)
    {
        return File(customDto.Stream, "image/jpeg");
    }
}

public class CustomDto
{
    public IFormFile File { get; set; }
    public Stream Stream { get; set; }
    public IEnumerable<byte> Bytes { get; set; }
    public IRemoteStreamContent RemoteStreamContent { get; set; }
}