using System;
using UnityEngine;
using System.Runtime.InteropServices;

public static class BurstlyAds {


	/***********************************************************/
	/*   Private methods to interface with the native C code   */
	/***********************************************************/
	 
	#if UNITY_IPHONE 
		[DllImport ("__Internal")]
		private static extern void BurstlyAdWrapper_createBannerPlacement(string placementName, string appId, string zoneId, float originX, float originY, float width, float height);
		
		[DllImport ("__Internal")]
		private static extern void BurstlyAdWrapper_createInterstitialPlacement(string placementName, string appId, string zoneId);
		
		[DllImport ("__Internal")]
		private static extern void BurstlyAdWrapper_destroyAdPlacement(string placementName);
	
		[DllImport ("__Internal")]
		private static extern void BurstlyAdWrapper_showAd(string placementName);
			
		[DllImport ("__Internal")]
		private static extern void BurstlyAdWrapper_cacheAd(string placementName);
	    
		[DllImport ("__Internal")]
		private static extern void BurstlyAdWrapper_pauseBanner(string placementName);
	
		[DllImport ("__Internal")]
		private static extern void BurstlyAdWrapper_unpauseBanner(string placementName);
	    
		[DllImport ("__Internal")]
		private static extern void BurstlyAdWrapper_addBannerToView(string placementName);
	    
		[DllImport ("__Internal")]
		private static extern void BurstlyAdWrapper_removeBannerFromView(string placementName);
	    
		[DllImport ("__Internal")]
		private static extern bool BurstlyAdWrapper_isAdCached(string placementName);
			
		[DllImport ("__Internal")]
		private static extern void BurstlyAdWrapper_setBannerOrigin(string placementName, float originX, float originY);
	
		[DllImport ("__Internal")]	
		private static extern void BurstlyAdWrapper_setBannerRefreshRate(string placementName, float refreshRate);
	
		[DllImport ("__Internal")]
		private static extern void BurstlyAdWrapper_setTargettingParameters(string placementName, string targettingParameters);
	
		[DllImport ("__Internal")]
		private static extern void BurstlyAdWrapper_setAdParameters(string placementName, string adParameters);
	
		[DllImport ("__Internal")]
		private static extern void BurstlyAdWrapper_setCallbackGameObjectName(string callbackGameObjectName);
		
	#endif
	
	#if UNITY_ANDROID
	
		private static IntPtr BurstlyPluginClassLocalReference = AndroidJNI.FindClass("com/burstly/plugins/BurstlyAdWrapper");
		private static IntPtr BurstlyPluginClass = AndroidJNI.NewGlobalRef(BurstlyPluginClassLocalReference);
	
		private static IntPtr methodID_createBannerPlacement = AndroidJNI.GetStaticMethodID(BurstlyPluginClass, "createBannerPlacement", "(Ljava/lang/String;Ljava/lang/String;Ljava/lang/String;FFFF)V");
		private static IntPtr methodID_createInterstitialPlacement = AndroidJNI.GetStaticMethodID(BurstlyPluginClass, "createInterstitialPlacement", "(Ljava/lang/String;Ljava/lang/String;Ljava/lang/String;)V");
		private static IntPtr methodID_destroyAdPlacement = AndroidJNI.GetStaticMethodID(BurstlyPluginClass, "destroyAdPlacement", "(Ljava/lang/String;)V");
    	private static IntPtr methodID_showAd = AndroidJNI.GetStaticMethodID(BurstlyPluginClass, "showAd", "(Ljava/lang/String;)V");
    	private static IntPtr methodID_cacheAd = AndroidJNI.GetStaticMethodID(BurstlyPluginClass, "cacheAd", "(Ljava/lang/String;)V");
    	private static IntPtr methodID_pauseBanner = AndroidJNI.GetStaticMethodID(BurstlyPluginClass, "pauseBanner", "(Ljava/lang/String;)V");
    	private static IntPtr methodID_unpauseBanner = AndroidJNI.GetStaticMethodID(BurstlyPluginClass, "unpauseBanner", "(Ljava/lang/String;)V");
    	private static IntPtr methodID_addBannerToView = AndroidJNI.GetStaticMethodID(BurstlyPluginClass, "addBannerToView", "(Ljava/lang/String;)V");
    	private static IntPtr methodID_removeBannerFromView = AndroidJNI.GetStaticMethodID(BurstlyPluginClass, "removeBannerFromView", "(Ljava/lang/String;)V");
    	private static IntPtr methodID_isAdCached = AndroidJNI.GetStaticMethodID(BurstlyPluginClass, "isAdCached", "(Ljava/lang/String;)Z");
    	private static IntPtr methodID_setBannerOrigin = AndroidJNI.GetStaticMethodID(BurstlyPluginClass, "setBannerOrigin", "(Ljava/lang/String;FF)V");
		private static IntPtr methodID_setCallbackGameObjectName = AndroidJNI.GetStaticMethodID(BurstlyPluginClass, "setCallbackGameObjectName", "(Ljava/lang/String;)V");
    	private static IntPtr methodID_setBannerRefreshRate = AndroidJNI.GetStaticMethodID(BurstlyPluginClass, "setBannerRefreshRate", "(Ljava/lang/String;F)V");
		private static IntPtr methodID_setTargettingParameters = AndroidJNI.GetStaticMethodID(BurstlyPluginClass, "setTargettingParameters", "(Ljava/lang/String;Ljava/lang/String;)V");
		private static IntPtr methodID_setAdParameters = AndroidJNI.GetStaticMethodID(BurstlyPluginClass, "setAdParameters", "(Ljava/lang/String;Ljava/lang/String;)V");
		
		private static void BurstlyAdWrapper_createBannerPlacement(string placementName, string appId, string zoneId, float originX, float originY, float width, float height) {
			jvalue[] args = new jvalue[7];
      		args[0].l = AndroidJNI.NewStringUTF(placementName);
      		args[1].l = AndroidJNI.NewStringUTF(appId);
      		args[2].l = AndroidJNI.NewStringUTF(zoneId);
			args[3].f = originX;
			args[4].f = originY;
			args[5].f = width;
			args[6].f = height;
			AndroidJNI.CallStaticVoidMethod(BurstlyPluginClass, methodID_createBannerPlacement, args);
		}
	
		private static void BurstlyAdWrapper_createInterstitialPlacement(string placementName, string appId, string zoneId) {
			jvalue[] args = new jvalue[3];
      		args[0].l = AndroidJNI.NewStringUTF(placementName);
      		args[1].l = AndroidJNI.NewStringUTF(appId);
      		args[2].l = AndroidJNI.NewStringUTF(zoneId);
			AndroidJNI.CallStaticVoidMethod(BurstlyPluginClass, methodID_createInterstitialPlacement, args);
		}
		
		private static void BurstlyAdWrapper_destroyAdPlacement(string placementName) {
			jvalue[] args = new jvalue[1];
      		args[0].l = AndroidJNI.NewStringUTF(placementName);
			AndroidJNI.CallStaticVoidMethod(BurstlyPluginClass, methodID_destroyAdPlacement, args);
		}
			
		private static void BurstlyAdWrapper_showAd(string placementName) {
			jvalue[] args = new jvalue[1];
      		args[0].l = AndroidJNI.NewStringUTF(placementName);
			AndroidJNI.CallStaticVoidMethod(BurstlyPluginClass, methodID_showAd, args);
		}
		
		private static void BurstlyAdWrapper_cacheAd(string placementName) {
			jvalue[] args = new jvalue[1];
      		args[0].l = AndroidJNI.NewStringUTF(placementName);
			AndroidJNI.CallStaticVoidMethod(BurstlyPluginClass, methodID_cacheAd, args);
		}
	    
		private static void BurstlyAdWrapper_pauseBanner(string placementName) {
			jvalue[] args = new jvalue[1];
      		args[0].l = AndroidJNI.NewStringUTF(placementName);
			AndroidJNI.CallStaticVoidMethod(BurstlyPluginClass, methodID_pauseBanner, args);
		}
	
		private static void BurstlyAdWrapper_unpauseBanner(string placementName) {
			jvalue[] args = new jvalue[1];
      		args[0].l = AndroidJNI.NewStringUTF(placementName);
			AndroidJNI.CallStaticVoidMethod(BurstlyPluginClass, methodID_unpauseBanner, args);
		}
	    
		private static void BurstlyAdWrapper_addBannerToView(string placementName) {
			jvalue[] args = new jvalue[1];
      		args[0].l = AndroidJNI.NewStringUTF(placementName);
			AndroidJNI.CallStaticVoidMethod(BurstlyPluginClass, methodID_addBannerToView, args);
		}
	    
		private static void BurstlyAdWrapper_removeBannerFromView(string placementName) {
			jvalue[] args = new jvalue[1];
      		args[0].l = AndroidJNI.NewStringUTF(placementName);
			AndroidJNI.CallStaticVoidMethod(BurstlyPluginClass, methodID_removeBannerFromView, args);
		}
	    
		private static bool BurstlyAdWrapper_isAdCached(string placementName) {
			jvalue[] args = new jvalue[1];
      		args[0].l = AndroidJNI.NewStringUTF(placementName);
			return AndroidJNI.CallStaticBooleanMethod(BurstlyPluginClass, methodID_isAdCached, args);
		}
			
		private static void BurstlyAdWrapper_setBannerOrigin(string placementName, float originX, float originY) {
			jvalue[] args = new jvalue[3];
      		args[0].l = AndroidJNI.NewStringUTF(placementName);
      		args[1].f = originX;
			args[2].f = originY;
			AndroidJNI.CallStaticVoidMethod(BurstlyPluginClass, methodID_setBannerOrigin, args);
		}

		private static void BurstlyAdWrapper_setBannerRefreshRate(string placementName, float refreshRate) {
			jvalue[] args = new jvalue[2];
      		args[0].l = AndroidJNI.NewStringUTF(placementName);
      		args[1].f = refreshRate;
			AndroidJNI.CallStaticVoidMethod(BurstlyPluginClass, methodID_setBannerRefreshRate, args);
		}
		
		private static void BurstlyAdWrapper_setTargettingParameters(string placementName, string targettingParameters) {
			jvalue[] args = new jvalue[2];
      		args[0].l = AndroidJNI.NewStringUTF(placementName);
      		args[1].l = AndroidJNI.NewStringUTF(targettingParameters);
			AndroidJNI.CallStaticVoidMethod(BurstlyPluginClass, methodID_setTargettingParameters, args);
		}		

		private static void BurstlyAdWrapper_setAdParameters(string placementName, string adParameters) {
			jvalue[] args = new jvalue[2];
      		args[0].l = AndroidJNI.NewStringUTF(placementName);
      		args[1].l = AndroidJNI.NewStringUTF(adParameters);
			AndroidJNI.CallStaticVoidMethod(BurstlyPluginClass, methodID_setAdParameters, args);
		}		
	
		private static void BurstlyAdWrapper_setCallbackGameObjectName(string callbackGameObjectName) {
			jvalue[] args = new jvalue[1];
      		args[0].l = AndroidJNI.NewStringUTF(callbackGameObjectName);
			AndroidJNI.CallStaticVoidMethod(BurstlyPluginClass, methodID_setCallbackGameObjectName, args);
		}
		
	#endif


	/************************************************************************/
	/*   Public methods to interface with C#/Javascript code within Unity   */
	/************************************************************************/
	
	/*
		Allocates and initialises a Burstly banner ad instance with the passed placementName, publisherId, zoneId, 
		x-origin, y-origin, width and height. Placement names must be unique across banner and interstitial ads.
	 */
	public static void createBannerPlacement(string placementName, string appId, string zoneId, float originX, float originY, float width, float height) {
		if ((Application.platform == RuntimePlatform.OSXEditor) || (Application.platform == RuntimePlatform.WindowsEditor)) return;
		
		BurstlyAdWrapper_createBannerPlacement(placementName, appId, zoneId, originX, originY, width, height);
	}
	
	/*
		Allocates and initialises a Burstly interstitial ad instance with the passed placementName, publisherId 
		and zoneId. Placement names must be unique across banner and interstitial ads.
	 */
	public static void createInterstitialPlacement(string placementName, string appId, string zoneId) {
		if ((Application.platform == RuntimePlatform.OSXEditor) || (Application.platform == RuntimePlatform.WindowsEditor)) return;
		
		BurstlyAdWrapper_createInterstitialPlacement(placementName, appId, zoneId);
	}
	
	/*
		Deallocates and destroys a Burstly banner or interstitial ad instance. Note that after calling this, 
		the instance will not be able to be accessed and must be recreated.
	 */
	public static void destroyAdPlacement(string placementName) {
		if ((Application.platform == RuntimePlatform.OSXEditor) || (Application.platform == RuntimePlatform.WindowsEditor)) return;
		
		BurstlyAdWrapper_destroyAdPlacement(placementName);
	}

	/*
		Requests an ad from the Burstly. If the placement is a banner placement, the placement must be added 
		to the view hierarchy by using addBannerToView(const char *placementName) to display. Interstitial placements 
		will display the ad immediately if it has been precached or as soon as it is recieved otherwise.
	 */
	public static void showAd(string placementName) {
		if ((Application.platform == RuntimePlatform.OSXEditor) || (Application.platform == RuntimePlatform.WindowsEditor)) return;
		
		BurstlyAdWrapper_showAd(placementName);
	}
		
	/*
		Precaches an interstitial ad for an interstitial placement. This will request and download an interstitial 
		ad in the background but not display it. When the ad is ready to be displayed, showAd(string placementName) 
		should be called and the ad will display.
	 */
	public static void cacheAd(string placementName) {
		if ((Application.platform == RuntimePlatform.OSXEditor) || (Application.platform == RuntimePlatform.WindowsEditor)) return;
		
		BurstlyAdWrapper_cacheAd(placementName);
	}
    
	/*
		Pauses the internal refresh timer for a banner ad.
	 */
	public static void pauseBanner(string placementName) {
		if ((Application.platform == RuntimePlatform.OSXEditor) || (Application.platform == RuntimePlatform.WindowsEditor)) return;
		
		BurstlyAdWrapper_pauseBanner(placementName);
	}

	/*
		Unpauses the internal refresh timer for a banner ad.
	 */
	public static void unpauseBanner(string placementName) {
		if ((Application.platform == RuntimePlatform.OSXEditor) || (Application.platform == RuntimePlatform.WindowsEditor)) return;
		
		BurstlyAdWrapper_unpauseBanner(placementName);
	}
    
	/*
		Adds a banner ad to the view hierarchy, placing it on the screen.
	 */
	public static void addBannerToView(string placementName) {
		if ((Application.platform == RuntimePlatform.OSXEditor) || (Application.platform == RuntimePlatform.WindowsEditor)) return;
		
		BurstlyAdWrapper_addBannerToView(placementName);
	}
    
	/*
		Removes a banner ad from the view hierarchy, removing it from the screen.
	 */
	public static void removeBannerFromView(string placementName) {
		if ((Application.platform == RuntimePlatform.OSXEditor) || (Application.platform == RuntimePlatform.WindowsEditor)) return;
		
		BurstlyAdWrapper_removeBannerFromView(placementName);
	}
    
	/*
		Returns TRUE if an interstitial ad has been precached for the given placementName, and FALSE if an interstitial 
		ad has not been precached.
	 */
	public static bool isAdCached(string placementName) {
		if ((Application.platform == RuntimePlatform.OSXEditor) || (Application.platform == RuntimePlatform.WindowsEditor)) return false;
		
		return BurstlyAdWrapper_isAdCached(placementName);
	}
		
	/*
		Sets the origin of the banner ad to the given (x, y) pair on the screen. Note that the origin references the top-left 
		corner of the banner and ad it is in relation to the screen coordinates given any rotation that has occured, with the 
		top-left of the screen being (0, 0).
	 */
	public static void setBannerOrigin(string placementName, float originX, float originY) {
		if ((Application.platform == RuntimePlatform.OSXEditor) || (Application.platform == RuntimePlatform.WindowsEditor)) return;
		
		BurstlyAdWrapper_setBannerOrigin(placementName, originX, originY);
	}

	/*
		Sets the refresh rate for the banner's internal refresh timer in seconds.
	 */
	public static void setBannerRefreshRate(string placementName, float refreshRate) {
		if ((Application.platform == RuntimePlatform.OSXEditor) || (Application.platform == RuntimePlatform.WindowsEditor)) return;
		
		BurstlyAdWrapper_setBannerRefreshRate(placementName, refreshRate);
	}

	/*	
		Sets the custom publisher targetting data key-value pairs that are to be sent back to the ad server. This should represent a 
		set of comma-delimited key-value pairs that can consist of integer, float or string (must be single-quote delimited) values, 
		e.g. "gender='m',age=22".
	 */
	public static void setTargettingParameters(string placementName, string targettingParameters) {
		if ((Application.platform == RuntimePlatform.OSXEditor) || (Application.platform == RuntimePlatform.WindowsEditor)) return;
		
		BurstlyAdWrapper_setTargettingParameters(placementName, targettingParameters);
	}

	/*	
		Sets the creative-specific ad parameters that are to be sent back to the ad server for cutomising landing page URLs. This 
		should represent a set of comma-delimited key-value pairs that can consist of integer, float or string (must be single-quote 
		delimited) values. e.g. "gender='m',age=22".
	 */
	public static void setAdParameters(string placementName, string adParameters) {
		if ((Application.platform == RuntimePlatform.OSXEditor) || (Application.platform == RuntimePlatform.WindowsEditor)) return;
		
		BurstlyAdWrapper_setAdParameters(placementName, adParameters);
	}
	
	/*
		Sets the GameObject name whose BurstlyCallback method should be called. This method should have a string parameter. The plugin 
		will pass back a string representing the placementName|callbackEvent (pipe-delimited). The callback events are defined as follows:
		
			0x01 = BurstlyEventSucceeded
                    Called when an ad request succeeds
            0x02 = BurstlyEventFailed
                    Called when an ad request fails
            0x04 = BurstlyEventTakeoverFullscreen
                    Called when an ad will present a full-screen canvas. This could be either an interstitial being displayed on a canvas 
                    being presented after a banner is tapped.
            0x08 = BurstlyEventDismissFullscreen
                    Called when an ad will dismiss a full-screen canvas. This could be either an interstitial being dismissed or a banner 
                    canvas being dismissed.
            0x10 = BurstlyEventHidden
                    Called when a banner ad is removed from the view hierarchy.
            0x20 = BurstlyEventShown
                    Called when a banner ad is added to the view hierarchy.
            0x40 = BurstlyEventCached
                    Called when an interstitial ad has been precached.
            0x80 = BurstlyEventClicked
                    Called when an ad (either banner or interstitial) is tapped.
                    
		For example, a BurstlyEventSucceeded callback event for a placement named "banner" will pass back a string "banner|0".
	 */
	public static void setCallbackGameObjectName(string callbackGameObjectName) {
		if ((Application.platform == RuntimePlatform.OSXEditor) || (Application.platform == RuntimePlatform.WindowsEditor)) return;
		
		BurstlyAdWrapper_setCallbackGameObjectName(callbackGameObjectName);
	}
	
}
