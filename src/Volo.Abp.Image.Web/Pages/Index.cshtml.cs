using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Imaging;

namespace Volo.Abp.Image.Web.Pages;

public class IndexModel : ImagePageModel
{
    [BindProperty]
    public IFormFile File { get; set; }

    private readonly IEnumerable<IImageResizerContributor> _imageResizerContributors;
    
    protected readonly IEnumerable<IImageCompressorContributor> ImageCompressorContributors;

    public IndexModel(IImageResizer imageResizer, IEnumerable<IImageResizerContributor> imageResizerContributors, IEnumerable<IImageCompressorContributor> imageCompressorContributors)
    {
        _imageResizerContributors = imageResizerContributors;
        ImageCompressorContributors = imageCompressorContributors;
    }

    public void OnGet()
    {
        
    }
    
    public async Task<IActionResult> OnPostResize()
    {
        var rnd = new Random();
        Dictionary<string,List<(string contributor, string data)>> results = new();

        for (var i = 0; i < 25; i++)
        {
            var args = new ImageResizeArgs(rnd.Next(100, 1000), rnd.Next(100, 1000), (ImageResizeMode)rnd.Next(0, 7));
        
            foreach (var contributor in _imageResizerContributors)
            {
                var result = await contributor.TryResizeAsync(File.OpenReadStream(), args);
                if (result.State != ProcessState.Done)
                {
                    continue;
                }

                var key = $"{args.Width}x{args.Height}x{args.Mode}";
                var newValue = (contributor.GetType().Name, Convert.ToBase64String(await result.Result.GetAllBytesAsync()));
                if(results.TryGetValue(key, out var value))
                {
                    value.Add(newValue);
                }
                else
                {
                    results.TryAdd(key ,new List<(string contributor, string data)> {newValue});
                }
            }
        }

        return PartialView("/Pages/Test.cshtml", results);
    }
    
    public async Task<IActionResult> OnPostCompress()
    {
        Dictionary<string,List<(string contributor, string data)>> results = new();

        foreach (var contributor in ImageCompressorContributors)
        {
            var result = await contributor.TryCompressAsync(File.OpenReadStream(), File.ContentType);
            if (result.State != ProcessState.Done)
            {
                continue;
            }

            var key = $"{File.FileName}";
            var newValue = (contributor.GetType().Name, Convert.ToBase64String(await result.Result.GetAllBytesAsync()));
            if(results.TryGetValue(key, out var value))
            {
                value.Add(newValue);
            }
            else
            {
                results.TryAdd(key ,new List<(string contributor, string data)> {newValue});
            }
        }
        
        return PartialView("/Pages/Test.cshtml", results);
    }
}

