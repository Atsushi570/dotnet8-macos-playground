using CoreGraphics;
using AppKit;
using Foundation;
using Vision;
using CoreGraphics;
using System;
using System.Linq;

namespace Desktop.Services;

public class ScreenshotOCRProcessor
{
    public ScreenshotOCRProcessor()
    {
    }

    public void PerformOCRFromScreenshot()
    {
        // スクリーンショットを取得
        var screenshot = GetScreenCGImage();

        if (screenshot == null)
        {
            Console.WriteLine("Failed to get screenshot");
            return;
        }

        // OCRを実行
        PerformOCR(screenshot);
    }

    private CGImage? GetScreenCGImage()
    {
        return CGImage.ScreenImage(0, CGRect.Infinite, CGWindowListOption.OnScreenOnly, CGWindowImageOption.BestResolution);
    }

    private NSImage CaptureScreenshot()
    {
        // メインスクリーンのサイズを取得
        CGRect screenRect = NSScreen.MainScreen.Frame;

        // グラフィックスコンテキストを作成
        using var graphicsContext = new CGBitmapContext(IntPtr.Zero,
                                                       (int)screenRect.Width,
                                                       (int)screenRect.Height,
                                                       8, // bits per component
                                                       0, // bytes per row
                                                       NSColorSpace.GenericRGBColorSpace.ColorSpace,
                                                       CGImageAlphaInfo.PremultipliedLast);

        // スクリーンの内容をコンテキストにコピー
        graphicsContext.ClearRect(screenRect);
        graphicsContext.FillRect(screenRect);
        graphicsContext.Flush();

        // CGImageを作成
        using var cgImage = graphicsContext?.ToImage();

        if (cgImage == null)
        {
            throw new Exception("Failed to create CGImage");
        }

        return new NSImage(cgImage, CGSize.Empty);
    }

    private void PerformOCR(CGImage cgImage)
    {
        var keys = new object[] { "key1", "key2" };
        var values = new object[] { "value1", "value2" };

        var dict = NSDictionary.FromObjectsAndKeys(values, keys);

        var handler = new VNImageRequestHandler(cgImage, new NSDictionary());

        var request = new VNRecognizeTextRequest((request, error) =>
        {
            if (error != null)
            {
                Console.WriteLine($"OCR Error: {error.LocalizedDescription}");
                return;
            }

            var observations = request.GetResults<VNRecognizedTextObservation>();
            ProcessObservations(observations);
        });

        try
        {
            handler.Perform(new VNRequest[] { request }, out var performError);
            if (performError != null)
            {
                Console.WriteLine($"Perform OCR Error: {performError.LocalizedDescription}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception during OCR: {ex.Message}");
        }
    }

    private void ProcessObservations(VNRecognizedTextObservation[] observations)
    {
        foreach (var observation in observations)
        {
            foreach (var text in observation.TopCandidates(1))
            {
                Console.WriteLine($"Detected text: {text.String}");
            }
        }
    }
}
