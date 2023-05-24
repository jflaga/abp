﻿using System.IO;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.Imaging;

public abstract class AbpImagingImageSharpProviderTestBase : AbpImagingImageSharpTestBase
{
    protected IImageCompressor ImageCompressor { get; }
    protected IImageResizer ImageResizer { get; }

    public AbpImagingImageSharpProviderTestBase()
    {
        ImageCompressor = GetRequiredService<IImageCompressor>();
        ImageResizer = GetRequiredService<IImageResizer>();
    }
}

public class ImageSharpImageCompressor_Tests : AbpImagingImageSharpProviderTestBase
{
    [Fact]
    public async Task Should_Compress_Jpg()
    {
        await using var jpegImage = ImageFileHelper.GetJpgTestFileStream();
        var compressedImage = await ImageCompressor.CompressAsync(jpegImage);

        compressedImage.ShouldNotBeNull();
        compressedImage.State.ShouldBe(ProcessState.Done);
        compressedImage.Result.Length.ShouldBeLessThan(jpegImage.Length);
        compressedImage.Result.Dispose();
    }

    [Fact]
    public async Task Should_Compress_Png()
    {
        await using var pngImage = ImageFileHelper.GetPngTestFileStream();
        var compressedImage = await ImageCompressor.CompressAsync(pngImage);

        compressedImage.ShouldNotBeNull();
        compressedImage.State.ShouldBe(ProcessState.Done);
        compressedImage.Result.Length.ShouldBeLessThan(pngImage.Length);
        compressedImage.Result.Dispose();
    }

    [Fact]
    public async Task Should_Compress_Webp()
    {
        await using var webpImage = ImageFileHelper.GetWebpTestFileStream();
        var compressedImage = await ImageCompressor.CompressAsync(webpImage);

        compressedImage.ShouldNotBeNull();
        compressedImage.State.ShouldBe(ProcessState.Done);
        compressedImage.Result.Length.ShouldBeLessThan(webpImage.Length);
        compressedImage.Result.Dispose();
    }

    [Fact]
    public async Task Should_Compress_Stream_And_Byte_Array_The_Same()
    {
        await using var jpegImage = ImageFileHelper.GetJpgTestFileStream();
        var byteArr = await jpegImage.GetAllBytesAsync();
        
        var compressedImage1 = await ImageCompressor.CompressAsync(jpegImage);
        var compressedImage2 = await ImageCompressor.CompressAsync(byteArr);
        
        compressedImage1.ShouldNotBeNull();
        compressedImage1.State.ShouldBe(ProcessState.Done);
        
        compressedImage2.ShouldNotBeNull();
        compressedImage2.State.ShouldBe(ProcessState.Done);
        
        compressedImage1.Result.Length.ShouldBeLessThan(jpegImage.Length);
        compressedImage2.Result.LongLength.ShouldBeLessThan(jpegImage.Length);
        
        compressedImage1.Result.Length.ShouldBe(compressedImage2.Result.LongLength);
        
        compressedImage1.Result.Dispose();
    }
}

public class ImageSharpImageResizer_Tests : AbpImagingImageSharpProviderTestBase
{
    [Fact]
    public async Task Should_Resize_Jpg()
    {
        await using var jpegImage = ImageFileHelper.GetJpgTestFileStream();
        var resizedImage = await ImageResizer.ResizeAsync(jpegImage, new ImageResizeArgs(100, 100));

        resizedImage.ShouldNotBeNull();
        resizedImage.State.ShouldBe(ProcessState.Done);
        resizedImage.Result.Length.ShouldBeLessThan(jpegImage.Length);
    }

    [Fact]
    public async Task Should_Resize_Png()
    {
        await using var pngImage = ImageFileHelper.GetPngTestFileStream();
        var resizedImage = await ImageResizer.ResizeAsync(pngImage, new ImageResizeArgs(100, 100));

        resizedImage.ShouldNotBeNull();
        resizedImage.State.ShouldBe(ProcessState.Done);
        resizedImage.Result.Length.ShouldBeLessThan(pngImage.Length);
    }

    [Fact]
    public async Task Should_Resize_Webp()
    {
        await using var webpImage = ImageFileHelper.GetWebpTestFileStream();
        var resizedImage = await ImageResizer.ResizeAsync(webpImage, new ImageResizeArgs(100, 100));

        resizedImage.ShouldNotBeNull();
        resizedImage.State.ShouldBe(ProcessState.Done);
        resizedImage.Result.Length.ShouldBeLessThan(webpImage.Length);
    }
    
    [Fact]
    public async Task Should_Resize_Stream_And_Byte_Array_The_Same()
    {
        await using var jpegImage = ImageFileHelper.GetJpgTestFileStream();
        var byteArr = await jpegImage.GetAllBytesAsync();
        
        var resizedImage1 = await ImageResizer.ResizeAsync(jpegImage, new ImageResizeArgs(100, 100));
        var resizedImage2 = await ImageResizer.ResizeAsync(byteArr, new ImageResizeArgs(100, 100));
        
        resizedImage1.ShouldNotBeNull();
        resizedImage1.State.ShouldBe(ProcessState.Done);
        
        resizedImage2.ShouldNotBeNull();
        resizedImage2.State.ShouldBe(ProcessState.Done);
        
        resizedImage1.Result.Length.ShouldBeLessThan(jpegImage.Length);
        resizedImage2.Result.LongLength.ShouldBeLessThan(jpegImage.Length);
        
        resizedImage1.Result.Length.ShouldBe(resizedImage2.Result.LongLength);
        
        resizedImage1.Result.Dispose();
    }
}