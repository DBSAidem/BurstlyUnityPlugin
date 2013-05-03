#ifndef __Burstly__BurstlyCurrencyWrapper_C__
#define __Burstly__BurstlyCurrencyWrapper_C__

#import <Foundation/Foundation.h>

#import "BurstlyPluginUtils.h"


/*
    BurstlyCurrencyCallback function pointer typedef for convenience. First argument is the placementName, second argument is a callbackEvent
 */
typedef void (*BurstlyCurrencyCallback)(bool);

extern "C" {
    
    /*
        External callback that is called by the Objective-C / JNI layer. This needs to be written specifically for each platform and then proxy accordingly to whichever framework is being targeted.
     */
    extern void BurstlyCurrencyWrapper_callback(bool success);
	
	void BurstlyCurrencyWrapper_initialize(const char *publisherId, const char *userId);
	int BurstlyCurrencyWrapper_getBalance(const char *currency);
	void BurstlyCurrencyWrapper_increaseBalance(const char *currency, int amount);
	void BurstlyCurrencyWrapper_decreaseBalance(const char *currency, int amount);
	void BurstlyCurrencyWrapper_updateBalancesFromServer();

}

#endif /* defined(__Burstly__BurstlyCurrencyWrapper_C__) */