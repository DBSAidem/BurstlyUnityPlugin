#import <stdio.h>

#import "BurstlyAdWrapper.h"

static NSString *_callbackGameObjectName = nil;

/*
 C-level callback that is invoked by the C layer. This is just calling the Unity callback so that the callback can be processed accordingly.
 The callback GameObject _must_ have a BurstlyCallback(string) method defined. The string passed through is the pipe-delimited concatenation of
 the placementName and the callbackEvent enum integer value.
 */
extern "C" void BurstlyAdWrapper_callback(const char *placementName, BurstlyEvent callbackEvent)  {
	if (!_callbackGameObjectName) return;
	
    UnitySendMessage([_callbackGameObjectName UTF8String], "BurstlyCallback", [[NSString stringWithFormat:@"%@|%i", CreateNSString(placementName), callbackEvent] UTF8String]);
}

extern "C" void BurstlyAdWrapper_setCallbackGameObjectName(const char *callbackGameObjectName) {
	if (_callbackGameObjectName) [_callbackGameObjectName release];
	
	_callbackGameObjectName = [CreateNSString(callbackGameObjectName) retain];
}