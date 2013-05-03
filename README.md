# Burstly Unity Plugin

## Contents

This repository contains the source files for the Burstly Unity Plugin as well as a Sample App. Within the Source directory there are two subdirectories: one for files that are used within the Unity editor solely and another for the Jar project that is exported and included in the Android plugin directory within Unity.

## Adding to Unity

In order to add the Burstly Unity plugin into your Unity project, take the drag and drop the contents of Source/UnityFiles/Editor into your Unity project's Assets/Editor/ folder and drag and drop the contents of Source/UnityFiles/Plugins into your Unity project's Assets/Plugins folder. Once that's done, you can add Burstly ad placements into your Unity project.

**Note**: The Burstly Unity plugin currently Windows only if Python is installed


## Deploying

Deploying to Android and iOS is seamless - just Build and Run. Our plugin takes care of adding the appropriate frameworks, etc.


## Reference

The Burstly Unity plugin manifests itself as a static class called BurstlyAds for ad (banner/interstitial) functionality and BurstlyCurrency for currency functionality. Static methods within these classes are used to interact with the native Burstly SDK from managed code. You can find inline documentation for these methods in BurstlyAds.cs and BurstlyCurrency.cs. 

**Note**: The Burstly plugin requires overriding the default UnityActivity on Android.

**Callbacks**: To enable callbacks, you must call the following for ads and currency, respectively:

	BurstlyAds.setCallbackGameObjectName("GameObjectName");
	BurstlyCurrency.setCallbackGameObjectName("GameObjectName");

The parameter passed must be the name of the GameObject you wish to receive callbacks. That GameObject must have a method called BurstlyCallback(message:String). The string passed in will be a pipe-delimited string of the placement name and the callback event as an int. Note that you can replace the callback at any time by calling the above method and that the currency class callback is independent of the ads class callback (i.e., you can have separate callbacks for currency and ads). Unfortunately, we currently do not support multiple callbacks.
	

## Sample Code

**Creating and showing a banner ad**

	BurstlyAds.createBannerPlacement("banner", "5fWofmS3902gWbwSZhXa1w", "0659117979169244027", 0, 0 , 320, 50);
	BurstlyAds.showAd("banner");
	BurstlyAds.addBannerToView("banner");

**Creating and showing an interstitial ad**

	BurstlyAds.createInterstitialPlacement("interstitial", "5fWofmS3902gWbwSZhXa1w", "0759117979169244027");
	BurstlyAds.showAd("interstitial");

