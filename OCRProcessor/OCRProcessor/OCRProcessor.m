//
//  OCRProcessor.m
//  OCRProcessor
//
//  Created by Atsushi Sato on 2024/01/17.
//

#import <Vision/Vision.h>
#import "OCRProcessor.h"

const char* performOCROnImage(CGImageRef imageRef) {
    __block NSString *resultText = @"";

    VNImageRequestHandler *handler = [[VNImageRequestHandler alloc] initWithCGImage:imageRef options:@{}];
    
    // 日本語のテキスト認識リクエストの作成
    VNRecognizeTextRequest *request = [[VNRecognizeTextRequest alloc] initWithCompletionHandler:^(VNRequest *request, NSError *error) {
        if (error) {
            NSLog(@"OCR error: %@", error);
            return;
        }

        for (VNRecognizedTextObservation *observation in request.results) {
            VNRecognizedText *topCandidate = [observation topCandidates:1].firstObject;
            if (topCandidate != nil) {
                resultText = [resultText stringByAppendingString:topCandidate.string];
            }
        }
    }];
    
    // 日本語を指定
    request.recognitionLanguages = @[@"ja-JP"];

    // OCRリクエストの実行
    NSError *error = nil;
    [handler performRequests:@[request] error:&error];
    if (error) {
        NSLog(@"Request error: %@", error);
        return strdup([resultText UTF8String]);
    }

    return strdup([resultText UTF8String]);
}

