//
//  BurstlyAdWrapperBridge.m
//  BurstlyC++Plugin
//
//  Created by abishek ashok on 2/12/13.
//  Copyright (c) 2013 abishek ashok. All rights reserved.
//

#import "BurstlyAdWrapperBridge.h"


@implementation BurstlyAdWrapperBridge

static BurstlyAdWrapperBridge *_sharedInstance;


#pragma mark Singleton Implementation

+ (id)allocWithZone:(NSZone*)zone {
	@synchronized(self) {
		if (_sharedInstance == nil) {
			_sharedInstance = [super allocWithZone:zone];
			return _sharedInstance; // assignment and return on first allocation
		}
	}
	return nil; //on subsequent allocation attempts return nil
}

- (id)copyWithZone:(NSZone *)zone { return self; }

- (id)retain { return self; }

- (unsigned)retainCount {
	return UINT_MAX; //denotes an object that cannot be released
}

- (oneway void)release {
	//do nothing
}

- (id)autorelease { return self; }

- (id)init {
    self = [super init];
    if (self) {
    	
    	/*
    		This following code is required to make sure that interstitial ads and banner canvases are displayed
    		properly in GL/Unity games. This is due to the GL UIViewController being oriented in a different 
    		orientation than the actual game content. Two UIViewControllers (one to display ads on the screen
    		and another to present ads modally) are required due to orientation weirdness when presenting another
    		UIViewController modally from the UIViewController (this happens on banner canvas presentation). The
    		UIViewController is rotated when the modal UIVIewController is presented, and is then cut off. Having
    		two UIViewControllers fixes this, although it does add additional processing overhead in passing through
    		touches.
    		
    		NOTE: Using UnityGetGLViewController() will display ads correctly but will have modally-presented
    			  UIViewControllers _NOT_ oriented correctly.
    	 */
    	
        _rootViewController = [[BurstlyAdViewViewController alloc] init];
        [[[UIApplication sharedApplication] keyWindow].rootViewController addChildViewController:_rootViewController];
        [[[UIApplication sharedApplication] keyWindow].rootViewController.view addSubview:_rootViewController.view];

        _viewControllerForModalPresentation = [[UIViewController alloc] init];
        _viewControllerForModalPresentation.view = [[BurstlyAdViewView alloc] initWithFrame:_rootViewController.view.frame];
        [[[UIApplication sharedApplication] keyWindow] addSubview:_viewControllerForModalPresentation.view];

        _placementDictionary = [[NSMutableDictionary alloc] init];
    }
    return self;
}

+ (BurstlyAdWrapperBridge *)sharedInstance {
	@synchronized(self) {
		if (_sharedInstance == nil) {
			[[self alloc] init]; // assignment not done here
		}
	}
	return _sharedInstance;
}

#pragma mark Class Implementation

- (void)createBurstlyBannerAdWithPlacement:(NSString *)placement appId:(NSString*)appId andZoneId:(NSString*)zoneId andFrame:(CGRect)bannerFrame {
    // NOTE: this means nothing happens if the placement has already been created
    if (!placement || [_placementDictionary objectForKey:placement]) {
        NSLog(@"Placement already exists");
        return;
    }
    
    BurstlyBannerAdView *banner = [[BurstlyBannerAdView alloc] initWithAppId:appId zoneId:zoneId frame:bannerFrame anchor:kBurstlyAnchorTop rootViewController:_viewControllerForModalPresentation delegate:self];
    banner.adRequest = [BurstlyAdRequest request];
	banner.adRequest.targettingParameters = @"";
	banner.adRequest.adParameters = @"";
    [_placementDictionary setObject:banner forKey:placement];
}

- (void)createBurstlyInterstitialWithPlacement:(NSString *)placement appId:(NSString*)appId andZoneId:(NSString*)zoneId {
    // NOTE: this means nothing happens if the placement has already been created
    if (!placement || [_placementDictionary objectForKey:placement]) {
        NSLog(@"Placement already exists");
        return;
    }
    
    BurstlyInterstitial *interstitial = [[BurstlyInterstitial alloc] initAppId:appId zoneId:zoneId delegate:self];
    interstitial.adRequest = [BurstlyAdRequest request];
	interstitial.adRequest.targettingParameters = @"";
	interstitial.adRequest.adParameters = @"";
    [_placementDictionary setObject:interstitial forKey:placement];
}

- (void)destroyBurstlyAdWithPlacement:(NSString *)placement {
    id adPlacement = [_placementDictionary objectForKey:placement];
    if (adPlacement) {
        if ([adPlacement isKindOfClass:[BurstlyBannerAdView class]]) {
            [(BurstlyBannerAdView *)adPlacement removeFromSuperview];
            ((BurstlyBannerAdView *)adPlacement).delegate = nil;
        }
        [adPlacement release];
        adPlacement = nil;
        [_placementDictionary removeObjectForKey:placement];
    } else {
        NSLog(@"Placement does not exist.");
    }
}

- (void)showAdForPlacement:(NSString*)placement {
    id adPlacement = [_placementDictionary objectForKey:placement];
    if (adPlacement) {
        [adPlacement showAd];
    } else {
        NSLog(@"Placement does not exist.");
    }
}

- (void)cacheAdForPlacement:(NSString*)placement {
    id adPlacement = [_placementDictionary objectForKey:placement];
    if (adPlacement && [adPlacement isKindOfClass:[BurstlyInterstitial class]]) {
        [adPlacement cacheAd];
    } else {
        NSLog(@"Incorrect placement type for value. Expected type: BurstlyInterstitial");
    }
}

- (void)pauseBannerForPlacement:(NSString *)placement{
    id view = [_placementDictionary objectForKey:placement];
    if (view && [view isKindOfClass:[BurstlyBannerAdView class]]) {
        ((BurstlyBannerAdView *)view).adPaused = YES;
    } else {
        NSLog(@"Incorrect placement type for value. Expected type: BurstlyBannerAdView");
    }
}

- (void)unpauseBannerForPlacement:(NSString *)placement{
    id view = [_placementDictionary objectForKey:placement];
    if (view && [view isKindOfClass:[BurstlyBannerAdView class]]) {
        ((BurstlyBannerAdView *)view).adPaused = NO;
    } else {
        NSLog(@"Incorrect placement type for value. Expected type: BurstlyBannerAdView");
    }
}

- (void)addBannerToViewForPlacement:(NSString *)placement{
    id view = [_placementDictionary objectForKey:placement];
    if (view && [view isKindOfClass:[BurstlyBannerAdView class]]) {
        [_rootViewController.view addSubview:view];
    } else {
        NSLog(@"Incorrect placement type for value. Expected type: BurstlyBannerAdView");
    }
}

- (void)removeBannerFromViewForPlacement:(NSString *)placement{
    id view = [_placementDictionary objectForKey:placement];
    if (view && [view isKindOfClass:[BurstlyBannerAdView class]]) {
        [(BurstlyBannerAdView *)view removeFromSuperview];
    } else {
        NSLog(@"Incorrect placement type for value. Expected type: BurstlyBannerAdView");
    }
}

- (BOOL)isAdCachedForPlacement:(NSString*)placement {
    id adPlacement = [_placementDictionary objectForKey:placement];
    if (adPlacement && [adPlacement isKindOfClass:[BurstlyInterstitial class]]) {
        return ((BurstlyInterstitial *)adPlacement).state == BurstlyInterstitialStatePreCached;
    }
    return NO;
}

- (void)setBannerOrigin:(CGPoint)origin forPlacement:(NSString *)placement{
    id view = [_placementDictionary objectForKey:placement];
    if (view && [view isKindOfClass:[BurstlyBannerAdView class]]) {
        CGRect frame = [view frame];
        frame.origin.x = origin.x;
        frame.origin.y = origin.y;
        ((BurstlyBannerAdView *)view).frame = frame;
    } else {
        NSLog(@"Incorrect placement type for value. Expected type: BurstlyBannerAdView");
    }
}

- (void)setBannerRefreshRate:(CGFloat)refreshRate forPlacement:(NSString *)placement {
    id view = [_placementDictionary objectForKey:placement];
    if (view && [view isKindOfClass:[BurstlyBannerAdView class]]) {
        ((BurstlyBannerAdView *)view).defaultRefreshInterval = refreshRate;
    } else {
        NSLog(@"Incorrect placement type for value. Expected type: BurstlyBannerAdView");
    }
}

- (void)setTargettingParameters:(NSString *)targettingParameters forPlacement:(NSString *)placement {
	id adPlacement = [_placementDictionary objectForKey:placement];
    if (adPlacement) {
        if ([adPlacement isKindOfClass:[BurstlyInterstitial class]])
            ((BurstlyInterstitial *)adPlacement).adRequest.targettingParameters = targettingParameters;
        else if ([adPlacement isKindOfClass:[BurstlyBannerAdView class]])
            ((BurstlyBannerAdView *)adPlacement).adRequest.targettingParameters = targettingParameters;
    } else {
        NSLog(@"Placement does not exist.");
    }
}

- (void)setAdParameters:(NSString *)adParameters forPlacement:(NSString *)placement {
	id adPlacement = [_placementDictionary objectForKey:placement];
    if (adPlacement) {
        if ([adPlacement isKindOfClass:[BurstlyInterstitial class]])
            ((BurstlyInterstitial *)adPlacement).adRequest.adParameters = adParameters;
        else if ([adPlacement isKindOfClass:[BurstlyBannerAdView class]])
            ((BurstlyBannerAdView *)adPlacement).adRequest.adParameters = adParameters;
    } else {
        NSLog(@"Placement does not exist.");
    }	
}

#pragma mark - BurstlyBannerViewDelegate Protocol

- (void)burstlyBannerAdView:(BurstlyBannerAdView *)view willTakeOverFullScreen:(NSString*)adNetwork {
    NSArray *validKeys = [_placementDictionary allKeysForObject:view];
    if (validKeys && [validKeys count] > 0) {
        BurstlyAdWrapper_callback([(NSString *)[validKeys objectAtIndex:0] UTF8String], BurstlyEventTakeoverFullscreen);
    }
}

- (void)burstlyBannerAdView:(BurstlyBannerAdView *)view willDismissFullScreen:(NSString*)adNetwork {
    NSArray *validKeys = [_placementDictionary allKeysForObject:view];
    if (validKeys && [validKeys count] > 0) {
        BurstlyAdWrapper_callback([(NSString *)[validKeys objectAtIndex:0] UTF8String], BurstlyEventDismissFullscreen);
    }
}

- (void)burstlyBannerAdView:(BurstlyBannerAdView *)view didHide:(NSString*)lastViewedNetwork {
    NSArray *validKeys = [_placementDictionary allKeysForObject:view];
    if (validKeys && [validKeys count] > 0) {
        BurstlyAdWrapper_callback([(NSString *)[validKeys objectAtIndex:0] UTF8String], BurstlyEventHidden);
    }
}

- (void)burstlyBannerAdView:(BurstlyBannerAdView *)view didShow:(NSString*)adNetwork {
    NSArray *validKeys = [_placementDictionary allKeysForObject:view];
    if (validKeys && [validKeys count] > 0) {
        BurstlyAdWrapper_callback([(NSString *)[validKeys objectAtIndex:0] UTF8String], BurstlyEventShown);
    }
}

- (void)burstlyBannerAdView:(BurstlyBannerAdView *)view didCache:(NSString*)adNetwork {
    NSArray *validKeys = [_placementDictionary allKeysForObject:view];
    if (validKeys && [validKeys count] > 0) {
        BurstlyAdWrapper_callback([(NSString *)[validKeys objectAtIndex:0] UTF8String], BurstlyEventCached);
    }
}

- (void)burstlyBannerAdView:(BurstlyBannerAdView *)view wasClicked:(NSString*)adNetwork {
    NSArray *validKeys = [_placementDictionary allKeysForObject:view];
    if (validKeys && [validKeys count] > 0) {
        BurstlyAdWrapper_callback([(NSString *)[validKeys objectAtIndex:0] UTF8String], BurstlyEventClicked);
    }
}

- (void) burstlyBannerAdView:(BurstlyBannerAdView *)view didFailWithError:(NSError*)error {
    NSArray *validKeys = [_placementDictionary allKeysForObject:view];
    if (validKeys && [validKeys count] > 0) {
        BurstlyAdWrapper_callback([(NSString *)[validKeys objectAtIndex:0] UTF8String], BurstlyEventFailed);
    }
}

#pragma mark - BurstlyInterstitialDelegate Protocol

- (UIViewController*)viewControllerForModalPresentation:(BurstlyInterstitial *)interstitial {
    return _viewControllerForModalPresentation;
}

- (void)burstlyInterstitial:(BurstlyInterstitial *)ad willTakeOverFullScreen:(NSString*)adNetwork {
    NSArray *validKeys = [_placementDictionary allKeysForObject:ad];
    if (validKeys && [validKeys count] > 0) {
        BurstlyAdWrapper_callback([(NSString *)[validKeys objectAtIndex:0] UTF8String], BurstlyEventTakeoverFullscreen);
    }
}

- (void)burstlyInterstitial:(BurstlyInterstitial *)ad willDismissFullScreen:(NSString*)adNetwork {
    NSArray *validKeys = [_placementDictionary allKeysForObject:ad];
    if (validKeys && [validKeys count] > 0) {
        BurstlyAdWrapper_callback([(NSString *)[validKeys objectAtIndex:0] UTF8String], BurstlyEventDismissFullscreen);
    }
}

- (void)burstlyInterstitial:(BurstlyInterstitial *)ad didHide:(NSString*)lastViewedNetwork {
    NSArray *validKeys = [_placementDictionary allKeysForObject:ad];
    if (validKeys && [validKeys count] > 0) {
        BurstlyAdWrapper_callback([(NSString *)[validKeys objectAtIndex:0] UTF8String], BurstlyEventHidden);
    }
}

- (void)burstlyInterstitial:(BurstlyInterstitial *)ad didShow:(NSString*)adNetwork {
    NSArray *validKeys = [_placementDictionary allKeysForObject:ad];
    if (validKeys && [validKeys count] > 0) {
        BurstlyAdWrapper_callback([(NSString *)[validKeys objectAtIndex:0] UTF8String], BurstlyEventShown);
    }
}

- (void)burstlyInterstitial:(BurstlyInterstitial *)ad didCache:(NSString*)adNetwork {
    NSArray *validKeys = [_placementDictionary allKeysForObject:ad];
    if (validKeys && [validKeys count] > 0) {
        BurstlyAdWrapper_callback([(NSString *)[validKeys objectAtIndex:0] UTF8String], BurstlyEventCached);
    }
}

- (void)burstlyInterstitial:(BurstlyInterstitial *)ad wasClicked:(NSString*)adNetwork {
    NSArray *validKeys = [_placementDictionary allKeysForObject:ad];
    if (validKeys && [validKeys count] > 0) {
        BurstlyAdWrapper_callback([(NSString *)[validKeys objectAtIndex:0] UTF8String], BurstlyEventClicked);
    }
}

- (void) burstlyInterstitial:(BurstlyInterstitial *)ad didFailWithError:(NSError*)error {
    NSArray *validKeys = [_placementDictionary allKeysForObject:ad];
    if (validKeys && [validKeys count] > 0) {
        BurstlyAdWrapper_callback([(NSString *)[validKeys objectAtIndex:0] UTF8String], BurstlyEventFailed);
    }
}

@end
