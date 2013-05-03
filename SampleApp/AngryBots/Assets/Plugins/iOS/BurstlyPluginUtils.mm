#import "BurstlyPluginUtils.h"

NSString *CreateNSString(const char* string) {
    return [NSString stringWithUTF8String:(string ? string : "")];
}