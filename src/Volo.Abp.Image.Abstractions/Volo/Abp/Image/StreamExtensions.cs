namespace Volo.Abp.Image.Abstractions.Volo.Abp.Image;

public static class StreamExtensions
{
    public static Stream ConvertToWritableStream(this Stream stream, bool disposeOriginalStream = true)
    {
        if (stream.CanWrite)
        {
            return stream;
        }

        var writableStream = new MemoryStream();
        stream.CopyTo(writableStream);
        writableStream.Position = 0;
        if (disposeOriginalStream)
        {
            stream.Dispose();
            stream.Close();
        }
        return writableStream;
    }
    
    public static async Task<Stream> ConvertToWritableStreamAsync(this Stream stream, bool disposeOriginalStream = true, CancellationToken cancellationToken = default)
    {
        if (stream.CanWrite)
        {
            return stream;
        }

        var writableStream = new MemoryStream();
        await stream.CopyToAsync(writableStream, cancellationToken);
        writableStream.Position = 0;
        if (disposeOriginalStream)
        {
            await stream.DisposeAsync();
            stream.Close();
        }
        return writableStream;
    }
}