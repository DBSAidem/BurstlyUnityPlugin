#import <stdio.h>

#import "BurstlyCurrencyWrapper.h"

static NSString *_callbackGameObjectName = nil;

/*
 C-level callback that is invoked by the C layer. This is just calling the Unity callback so that the callback can be processed accordingly.
 The callback GameObject _must_ have a BurstlyCallback(string) method defined. The string passed through is the pipe-delimited concatenation of
 the placementName and the callbackEvent enum integer value.
 */
extern "C" void BurstlyCurrencyWrapper_callback(bool success) {
	if (!_callbackGameObjectName) return;
	
	UnitySendMessage([_callbackGameObjectName UTF8String], "BurstlyCallback", (success ? [@"UPDATED" UTF8String] : [@"FAILED" UTF8String]));
}

extern "C" void BurstlyCurrencyWrapper_setCallbackGameObjectName(const char *callbackGameObjectName) {
	if (_callbackGameObjectName) [_callbackGameObjectName release];
	
	_callbackGameObjectName = [CreateNSString(callbackGameObjectName) retain];
}