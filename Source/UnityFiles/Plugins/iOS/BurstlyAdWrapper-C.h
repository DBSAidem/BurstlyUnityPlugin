#ifndef __Burstly__BurstlyAdWrapper_C__
#define __Burstly__BurstlyAdWrapper_C__

#import <Foundation/Foundation.h>

#import "BurstlyPluginUtils.h"


/*
    Callback event constants. This is done for ease of processing and to allow callbacks to be registered to receive notification of multiple events. Callbacks events are as follows:
 
            BurstlyEventSucceeded
                    Called when an ad request succeeds
            BurstlyEventFailed
                    Called when an ad request fails
            BurstlyEventTakeoverFullscreen
                    Called when an ad will present a full-screen canvas. This could be either an interstitial being displayed on a canvas being presented after a banner is tapped.
            BurstlyEventDismissFullscreen
                    Called when an ad will dismiss a full-screen canvas. This could be either an interstitial being dismissed or a banner canvas being dismissed.
            BurstlyEventHidden
                    Called when a banner ad is removed from the view hierarchy.
            BurstlyEventShown
                    Called when a banner ad is added to the view hierarchy.
            BurstlyEventCached
                    Called when an interstitial ad has been precached.
            BurstlyEventClicked
                    Called when an ad (either banner or interstitial) is tapped.
 */

typedef enum {
    BurstlyEventSucceeded           = 0x01,
    BurstlyEventFailed              = 0x02,
    BurstlyEventTakeoverFullscreen  = 0x04,
    BurstlyEventDismissFullscreen   = 0x08,
    BurstlyEventHidden              = 0x10,
    BurstlyEventShown               = 0x20,
    BurstlyEventCached              = 0x40,
    BurstlyEventClicked             = 0x80
} BurstlyEvent;


/*
    BurstlyAdCallback function pointer typedef for convenience. First argument is the placementName, second argument is a callbackEvent
 */
typedef void (*BurstlyAdCallback)(const char *, BurstlyEvent);

extern "C" {
    
    /*
        External callback that is called by the Objective-C / JNI layer. This needs to be written specifically for each platform and then proxy accordingly to whichever framework is being targeted.
     */
    extern void BurstlyAdWrapper_callback(const char *placementName, BurstlyEvent callbackEvent);
	
	void BurstlyAdWrapper_createBannerPlacement(const char *placementName, const char *publisherId, const char *zoneId, float originX, float originY, float width, float height);
	void BurstlyAdWrapper_createInterstitialPlacement(const char *placementName, const char *publisherId, const char *zoneId);
	
	void BurstlyAdWrapper_destroyAdPlacement(const char *placementName);
	
	void BurstlyAdWrapper_showAd(const char *placementName);
	void BurstlyAdWrapper_cacheAd(const char *placementName);
	
	void BurstlyAdWrapper_pauseBanner(const char *placementName);
	void BurstlyAdWrapper_unpauseBanner(const char *placementName);
	void BurstlyAdWrapper_addBannerToView(const char *placementName);
	void BurstlyAdWrapper_removeBannerFromView(const char *placementName);
	
	bool BurstlyAdWrapper_isAdCached(const char *placementName);
	
	void BurstlyAdWrapper_setBannerOrigin(const char *placementName, float originX, float originY);
	void BurstlyAdWrapper_setBannerRefreshRate(const char *placementName, float refreshRate);
	void BurstlyAdWrapper_setTargettingParameters(const char *placementName, const char *targettingParameters);
	void BurstlyAdWrapper_setAdParameters(const char *placementName, const char *adParameters);

}

#endif /* defined(__Burstly__BurstlyAdWrapper_C__) */