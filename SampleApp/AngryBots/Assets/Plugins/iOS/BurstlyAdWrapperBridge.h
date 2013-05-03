//
//  BurstlyAdWrapperBridge.h
//  BurstlyC++Plugin
//
//  Created by abishek ashok on 2/12/13.
//  Copyright (c) 2013 abishek ashok. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "BurstlyBannerAdView.h"
#import "BurstlyInterstitial.h"
#import "BurstlyAdViewViewController.h"

#import "BurstlyAdWrapper.h"

@interface BurstlyAdWrapperBridge : NSObject<BurstlyBannerViewDelegate, BurstlyInterstitialDelegate> {
    UIViewController *_rootViewController;
    UIViewController *_viewControllerForModalPresentation;
    NSMutableDictionary *_placementDictionary;
}

+ (BurstlyAdWrapperBridge *)sharedInstance;

- (void)createBurstlyBannerAdWithPlacement:(NSString *)placement appId:(NSString*)appId andZoneId:(NSString*)zoneId andFrame:(CGRect)bannerFrame;
- (void)createBurstlyInterstitialWithPlacement:(NSString *)placement appId:(NSString*)appId andZoneId:(NSString*)zoneId;

- (void)destroyBurstlyAdWithPlacement:(NSString *)placement;

- (void)showAdForPlacement:(NSString*)placement;
- (void)cacheAdForPlacement:(NSString*)placement;

- (void)pauseBannerForPlacement:(NSString *)placement;
- (void)unpauseBannerForPlacement:(NSString *)placement;
- (void)addBannerToViewForPlacement:(NSString *)placement;
- (void)removeBannerFromViewForPlacement:(NSString *)placement;

- (BOOL)isAdCachedForPlacement:(NSString*)placement;

- (void)setBannerOrigin:(CGPoint)origin forPlacement:(NSString *)placement;
- (void)setBannerRefreshRate:(CGFloat)refreshRate forPlacement:(NSString *)placement;

- (void)setTargettingParameters:(NSString *)targettingParameters forPlacement:(NSString *)placement;
- (void)setAdParameters:(NSString *)adParameters forPlacement:(NSString *)placement;

@end
