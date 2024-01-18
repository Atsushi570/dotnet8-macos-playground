//
//  OCRProcessor.h
//  OCRProcessor
//
//  Created by Atsushi Sato on 2024/01/17.
//

#include <CoreGraphics/CoreGraphics.h>

#ifdef __cplusplus
extern "C" {
#endif

// C言語スタイルの関数宣言
const char* performOCROnImage(CGImageRef imageRef);

#ifdef __cplusplus
}
#endif
