//
//  OCRProcessorTest.m
//  OCRProcessorTest
//
//  Created by Atsushi Sato on 2024/01/17.
//

#import <XCTest/XCTest.h>
#import "OCRProcessor.h"

@interface OCRProcessorTest : XCTestCase

@end

@implementation OCRProcessorTest


- (void)testOCRWithValidImage {
    // テストクラスのバンドルを取得
    NSBundle *bundle = [NSBundle bundleForClass:[self class]];
    
    // バンドルから画像のパスを取得
    NSString *imagePath = [bundle pathForResource:@"ocr-test" ofType:@"png"];
    XCTAssertNotNil(imagePath, @"Image path should not be nil");

    // 画像を読み込む
    NSImage *testImage = [[NSImage alloc] initWithContentsOfFile:imagePath];
    XCTAssertNotNil(testImage, @"Test image should not be nil");


    CGImageRef cgImage = [testImage CGImageForProposedRect:nil context:nil hints:nil];
    XCTAssertNotNil((__bridge id)(cgImage), @"CGImage should not be nil");

    // performOCROnImage関数をテスト
    const char *result = performOCROnImage(cgImage);
    XCTAssert(result != NULL, @"OCR result should not be NULL");

    NSString *resultString = [NSString stringWithUTF8String:result];
    XCTAssertNotNil(resultString, @"OCR result string should not be nil");
    XCTAssertGreaterThan(resultString.length, 0, @"OCR result string should not be empty");

    // C言語スタイルの文字列を解放
    free((void *)result);
}

@end
