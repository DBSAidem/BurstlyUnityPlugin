#import <stdio.h>

#import "BurstlyAdWrapper-C.h"
#import "BurstlyAdWrapperBridge.h"


extern "C" {
	
	void BurstlyAdWrapper_createBannerPlacement(const char *placementName, const char *appId, const char *zoneId, float originX, float originY, float width, float height) {
        [[BurstlyAdWrapperBridge sharedInstance] createBurstlyBannerAdWithPlacement:CreateNSString(placementName) appId:CreateNSString(appId) andZoneId:CreateNSString(zoneId) andFrame:CGRectMake(originX, originY, width, height)];
	}
	
	void BurstlyAdWrapper_createInterstitialPlacement(const char *placementName, const char *appId, const char *zoneId) {
		[[BurstlyAdWrapperBridge sharedInstance] createBurstlyInterstitialWithPlacement:CreateNSString(placementName) appId:CreateNSString(appId) andZoneId:CreateNSString(zoneId)];
	}
	
	void BurstlyAdWrapper_destroyAdPlacement(const char *placementName) {
        [[BurstlyAdWrapperBridge sharedInstance] destroyBurstlyAdWithPlacement:CreateNSString(placementName)];
	}
	
	void BurstlyAdWrapper_showAd(const char *placementName) {
        [[BurstlyAdWrapperBridge sharedInstance] showAdForPlacement:CreateNSString(placementName)];
	}
	
	void BurstlyAdWrapper_cacheAd(const char *placementName) {
        [[BurstlyAdWrapperBridge sharedInstance] cacheAdForPlacement:CreateNSString(placementName)];
	}

	void BurstlyAdWrapper_pauseBanner(const char *placementName) {
        [[BurstlyAdWrapperBridge sharedInstance] pauseBannerForPlacement:CreateNSString(placementName)];
	}
	
	void BurstlyAdWrapper_unpauseBanner(const char *placementName) {
         [[BurstlyAdWrapperBridge sharedInstance] unpauseBannerForPlacement:CreateNSString(placementName)];
	}
	
	void BurstlyAdWrapper_addBannerToView(const char *placementName) {
        [[BurstlyAdWrapperBridge sharedInstance] addBannerToViewForPlacement:CreateNSString(placementName)];
	}
	
	void BurstlyAdWrapper_removeBannerFromView(const char *placementName) {
        [[BurstlyAdWrapperBridge sharedInstance] removeBannerFromViewForPlacement:CreateNSString(placementName)];
	}
	
	bool BurstlyAdWrapper_isAdCached(const char *placementName) {
		return [[BurstlyAdWrapperBridge sharedInstance] isAdCachedForPlacement:CreateNSString(placementName)];
	}
	
	void BurstlyAdWrapper_setBannerOrigin(const char *placementName, float originX, float originY) {
        [[BurstlyAdWrapperBridge sharedInstance] setBannerOrigin:CGPointMake(originX, originY) forPlacement:CreateNSString(placementName)];
	}
	
	void BurstlyAdWrapper_setBannerRefreshRate(const char *placementName, float refreshRate) {
		[[BurstlyAdWrapperBridge sharedInstance] setBannerRefreshRate:refreshRate forPlacement:CreateNSString(placementName)];
	}
	
	void BurstlyAdWrapper_setTargettingParameters(const char *placementName, const char *targettingParameters) {
		[[BurstlyAdWrapperBridge sharedInstance] setTargettingParameters:CreateNSString(targettingParameters) forPlacement:CreateNSString(placementName)];
	}
	
	void BurstlyAdWrapper_setAdParameters(const char *placementName, const char *adParameters) {
		[[BurstlyAdWrapperBridge sharedInstance] setAdParameters:CreateNSString(adParameters) forPlacement:CreateNSString(placementName)];
	}
	
}