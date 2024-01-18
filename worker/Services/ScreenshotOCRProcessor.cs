using Xamarin;
using System;
using System.Runtime.InteropServices;
using CoreGraphics;
using CoreFoundation;
using ImageIO;
using AppKit;
using Foundation;
using Vision;
using System;
using System.Linq;

namespace Worker.Services;

public class ScreenshotOCRProcessor
{
    public void PerformOCRFromScreenshot()
    {
        // スクリーンショットを取得
        var screenshot = GetScreenCGImage();

        IntPtr resultPtr = PerformOCRWithCGImage(screenshot.Handle);
        string ocrResult = Marshal.PtrToStringAuto(resultPtr);

        // タイムスタンプを生成
        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");

        File.WriteAllText($"OCRResult_{timestamp}.txt", ocrResult);

    }

    [DllImport("libOCRProcessor", EntryPoint = "performOCROnImage")]
    private static extern IntPtr PerformOCRWithCGImage(IntPtr imageRef);

    private CGImage? GetScreenCGImage()
    {
        return CGImage.ScreenImage(0, CGRect.Infinite, CGWindowListOption.OnScreenOnly, CGWindowImageOption.BestResolution);
    }

}
