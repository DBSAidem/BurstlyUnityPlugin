package com.burstly.plugins;

import java.util.HashMap;

import com.burstly.lib.conveniencelayer.Burstly;
import com.burstly.lib.ui.BurstlyView;
import com.unity3d.player.UnityPlayer;

import android.app.Activity;
import android.view.ViewGroup;
import android.widget.RelativeLayout;
import android.widget.RelativeLayout.LayoutParams;

public class BurstlyAdWrapper {
	
	
	/*
	 * Callback event constants. This is done for ease of processing and to allow callbacks to be registered to receive notification of 
	 * multiple events. Callbacks events are as follows:
	 * 
	 * 		BurstlyEventSucceeded
	 * 			Called when an ad request succeeds
	 * 		BurstlyEventFailed
	 * 			Called when an ad request fails
	 * 		BurstlyEventTakeoverFullscreen
	 * 			Called when an ad will present a full-screen canvas. This could be either an interstitial being displayed on a canvas being presented after a banner is tapped.
	 * 		BurstlyEventDismissFullscreen
	 * 			Called when an ad will dismiss a full-screen canvas. This could be either an interstitial being dismissed or a banner canvas being dismissed.
	 * 		BurstlyEventHidden
	 * 			Called when a banner ad is removed from the view hierarchy.
	 * 		BurstlyEventShown
	 * 			Called when a banner ad is added to the view hierarchy.
	 * 		BurstlyEventCached
	 * 			Called when an interstitial ad has been precached.
	 * 		BurstlyEventClicked
	 * 			Called when an ad (either banner or interstitial) is tapped.
	 */
	public enum BurstlyEvent {
	    BurstlyEventSucceeded           (0x01),
	    BurstlyEventFailed              (0x02),
	    BurstlyEventTakeoverFullscreen  (0x04),
	    BurstlyEventDismissFullscreen   (0x08),
	    BurstlyEventHidden              (0x10),
	    BurstlyEventShown               (0x20),
	    BurstlyEventCached              (0x40),
	    BurstlyEventClicked             (0x80);

	    private int mEventCode;
	    
	    BurstlyEvent(int eventCode)	{	this.mEventCode = eventCode;	}
	    public int getEventCode() 	{	return mEventCode;				}
	}

	
	private static Activity mActivity = null;
	private static RelativeLayout mBaseLayout = null;
	private static HashMap<String, BurstlyView> mBurstlyViewHashMap = null;
	private static HashMap<String, Boolean> mBurstlyViewCachedHashMap = null;
	private static String mCallbackGameObjectName = null;
	
	
	/*****************************************************************/
	/* ANDROID JAVA METHODS - MUST BE CALLED WITHIN JAVA ENVIRONMENT */
	/*****************************************************************/
	
	/*
	 * Initialises BurstlyAdWrapper. Must be called before any views are created in your activity.
	 * 
	 *  @param	aActivity	The main activity for your app
	 */
	public static void init(Activity aActivity) {
		mActivity = aActivity;
		
        mBurstlyViewHashMap = new HashMap<String, BurstlyView>();
        mBurstlyViewCachedHashMap = new HashMap<String, Boolean>();
	}
        
	/*
	 * Creates the RelativeLayout that is used by BurstlyAdWrapper to manage its views. Must be called after the GLSurfaceView is added to 
	 * the activity and all app-specific View setup is done as this will overlay a RelativeLayout over the entire content view.
	 */
    public static void createViewLayout() {
        mBaseLayout = new RelativeLayout(mActivity);
        mBaseLayout.setLayoutParams(new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.MATCH_PARENT));
        mActivity.addContentView(mBaseLayout, new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.MATCH_PARENT));
	}
    
	/*
	 * Burstly methods that are just proxied to maintain naming uniformity. Note that these MUST be called in the app's Activity
	 * lifecycle methods or undefined behaviour may occur. Sample below:
	 * 
	 * 		@Override
	 * 		protected void onPause() {
	 * 			BurstlyAdWrapper.onPauseActivity(this);
	 * 			super.onPause();
	 * 		}
	 * 
	 * 		@Override
	 * 		protected void onResume() {
	 * 			BurstlyAdWrapper.onResumeActivity(this);
	 * 			super.onResume();
	 * 		}
	 * 
	 * 		@Override
	 * 		protected void onDestroy() {
	 * 			BurstlyAdWrapper.onDestroyActivity(this);
	 * 			super.onDestroy();
	 * 		}
	 */
	public static void onPauseActivity(Activity aActivity) 		{	Burstly.onPauseActivity(aActivity);		}
	public static void onResumeActivity(Activity aActivity) 	{	Burstly.onResumeActivity(aActivity);	}
	public static void onDestroyActivity(Activity aActivity) 	{	Burstly.onDestroyActivity(aActivity);	}
    
    
	
	/*************************************************************************************************/
	/* NATIVE JNI JAVA METHODS - MAY BE CALLED WITHIN JAVA ENVIRONMENT OR IN NATIVE CODE THROUGH JNI */
    /*************************************************************************************************/              
    /* NOTE: NONE OF THESE MAY BE CALLED BEFORE init(Activity aActivity) and                         */
    /*       createViewLayout() ARE CALLED IN THE ANDROID JAVA ENVIRONMENT                           */
    /*************************************************************************************************/
	
	/*
	 * Allocates and initialises a Burstly banner ad instance with the passed placementName, publisherId, zoneId, x-origin, y-origin, width 
	 * and height. Placement names must be unique across banner and interstitial ads.
	 * 
	 * @param	sPlacementName	The placement name for the banner ad that should be craeted. This must be unique across
	 * 							all banners and interstitials.
	 * @param	sAppId			The app ID for your app.
	 * @param	sZoneId			The zone ID from which the creatives should be fetched.
	 * @param	originX			The x-offset from the origin (the top left of the screen)
	 * @param	originY			The y-offset from the origin (the top left of the screen)
	 * @param	width			The width of the banner ad.
	 * @param	height			The height of the banner ad.
	 */
	public static void createBannerPlacement(final String sPlacementName, final String sAppId, final String sZoneId, final float originX, final float originY, final float width, final float height) {
		// We cannot have multiple placements with the same placement name
		if (mBurstlyViewHashMap.containsKey(sPlacementName)) return;
		
		mActivity.runOnUiThread(new Runnable() {

			@Override
			public void run() {
				BurstlyView burstlyView = new BurstlyView(mActivity);
				burstlyView.setPublisherId(sAppId);
				burstlyView.setZoneId(sZoneId);
				burstlyView.setBurstlyViewId(sPlacementName);
				
				burstlyView.setPaused(false);
				
		        RelativeLayout.LayoutParams lp = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WRAP_CONTENT, ViewGroup.LayoutParams.WRAP_CONTENT);
		        lp.addRule(RelativeLayout.ALIGN_PARENT_TOP);
		        lp.addRule(RelativeLayout.ALIGN_PARENT_LEFT);
		        lp.topMargin = (int)originY;
		        lp.leftMargin = (int)originX;
		        burstlyView.setLayoutParams(lp);
		        
		        mBaseLayout.addView(burstlyView);
		        
		        BurstlyViewListener listener = new BurstlyViewListener();
				listener.setPlacementName(sPlacementName);
				burstlyView.setBurstlyAdListener(listener);
		        
		        mBurstlyViewHashMap.put(sPlacementName, burstlyView);
		        mBurstlyViewCachedHashMap.put(sPlacementName, false);
			}
			
		});
	}
	
	/*
	 * Allocates and initialises a Burstly interstitial ad instance with the passed placementName, publisherId and zoneId. Placement names 
	 * must be unique across banner and interstitial ads.
	 * 
	 * @param	sPlacementName	The placement name for the interstitial ad that should be craeted. This must be unique across
	 * 							all banners and interstitials.
	 * @param	sAppId			The app ID for your app.
	 * @param	sZoneId			The zone ID from which the creatives should be fetched.
	 */
	public static void createInterstitialPlacement(final String sPlacementName, final String sAppId, final String sZoneId) {
		// We cannot have multiple placements with the same placement name
		if (mBurstlyViewHashMap.containsKey(sPlacementName)) return;
		
        mActivity.runOnUiThread(new Runnable() {

			@Override
			public void run() {
				BurstlyView burstlyView = new BurstlyView(mActivity);
				burstlyView.setPublisherId(sAppId);
				burstlyView.setZoneId(sZoneId);
				burstlyView.setBurstlyViewId(sPlacementName);
				
				burstlyView.setPaused(false);
				
				BurstlyViewListener listener = new BurstlyViewListener();
				listener.setPlacementName(sPlacementName);
				burstlyView.setBurstlyAdListener(listener);
		        
		        mBurstlyViewHashMap.put(sPlacementName, burstlyView);
		        mBurstlyViewCachedHashMap.put(sPlacementName, false);
			}
			
		});
	}
	
	/*
	 * Deallocates and destroys a Burstly banner or interstitial ad instance. Note that after calling this, the instance will not be able to 
	 * be accessed and must be recreated.
	 * 
	 * @param	sPlacementName	The placement name for the ad placement that should be destroyed.
	 */
	public static void destroyAdPlacement(final String sPlacementName) {
		final BurstlyView burstlyView = mBurstlyViewHashMap.get(sPlacementName);
		if (burstlyView == null) return;
		
		mActivity.runOnUiThread(new Runnable() {

			@Override
			public void run() {
				// Need to remove the view from its parent so it gets GC'ed
				if (burstlyView.getParent() != null) mBaseLayout.removeView(burstlyView);
				
				mBurstlyViewHashMap.remove(sPlacementName);
				mBurstlyViewCachedHashMap.remove(sPlacementName);
			}
			
		});
	}
	
	/*
	 * Requests an ad from the Burstly. If the placement is a banner placement, the placement must be added to the view hierarchy by using 
	 * addBannerToView(const char *placementName) to display. Interstitial placements will display the ad immediately if it has been precached 
	 * or as soon as it is recieved otherwise.
	 * 
	 * @param	sPlacementName	The placement name for the ad placement that should be shown.
	 */
	public static void showAd(final String sPlacementName) {
		final BurstlyView burstlyView = mBurstlyViewHashMap.get(sPlacementName);
		if (burstlyView == null) return;
		
		mActivity.runOnUiThread(new Runnable() {

			@Override
			public void run() {
				burstlyView.sendRequestForAd();
				
				mBurstlyViewCachedHashMap.put(sPlacementName, false);
			}
			
		});
	}
	
	/*
	 * Precaches an interstitial ad for an interstitial placement. This will request and download an interstitial ad in the background but 
	 * not display it. When the ad is ready to be displayed, showAd(const char *placementName) should be called and the ad will display.
	 * 
	 * @param	sPlacementName	The placement name for the interstitial that should be precached.
	 */
	public static void cacheAd(final String sPlacementName) {
		final BurstlyView burstlyView = mBurstlyViewHashMap.get(sPlacementName);
		if (burstlyView == null) return;
		
		mActivity.runOnUiThread(new Runnable() {

			@Override
			public void run() {
				burstlyView.precacheAd();
				
				mBurstlyViewCachedHashMap.put(sPlacementName, false);
			}
			
		});
	}
	
	/*
	 * Pauses the internal refresh timer for a banner ad.
	 * 
	 * @param	sPlacementName	The placement name for the banner that should be paused.
	 */
	public static void pauseBanner(final String sPlacementName) {
		final BurstlyView burstlyView = mBurstlyViewHashMap.get(sPlacementName);
		if (burstlyView == null) return;
		
		mActivity.runOnUiThread(new Runnable() {

			@Override
			public void run() {
				burstlyView.setPaused(true);
			}
			
		});
	}
	
	/*
	 * Unpauses the internal refresh timer for a banner ad.
	 * 
	 * @param	sPlacementName	The placement name for the banner that should be unpaused.
	 */
	public static void unpauseBanner(final String sPlacementName) {
		final BurstlyView burstlyView = mBurstlyViewHashMap.get(sPlacementName);
		if (burstlyView == null) return;
		
		mActivity.runOnUiThread(new Runnable() {

			@Override
			public void run() {
				burstlyView.setPaused(false);
			}
			
		});
	}
	
	/*
	 * Adds a banner ad to the view hierarchy, placing it on the screen.
	 * 
	 * @param	sPlacementName	The placement name for the banner that should be added to the view hierarchy
	 */
	public static void addBannerToView(final String sPlacementName) {
		final BurstlyView burstlyView = mBurstlyViewHashMap.get(sPlacementName);
		if (burstlyView == null) return;
		
		// If the view has already been added to the layout, adding it again will cause a crash.
		if (burstlyView.getParent() != null) return;
		
		mActivity.runOnUiThread(new Runnable() {

			@Override
			public void run() {
				mBaseLayout.addView(burstlyView);
			}
			
		});
	}
	
	/*
	 * Removes a banner ad from the view hierarchy, removing it from the screen.
	 * 
	 * @param	sPlacementName	The placement name for the banner that should be removed from the view hierarchy
	 */
	public static void removeBannerFromView(final String sPlacementName) {
		final BurstlyView burstlyView = mBurstlyViewHashMap.get(sPlacementName);
		if (burstlyView == null)  return;
		
		if (burstlyView.getParent() == null) return;
		
		mActivity.runOnUiThread(new Runnable() {

			@Override
			public void run() {
				mBaseLayout.removeView(burstlyView);
			}
			
		});
	}
	
	/*
	 * Returns TRUE if an interstitial ad has been precached for the given placementName, and FALSE if an interstitial ad has not been precached.
	 * 
	 * @param	sPlacementName	The placement name for the interstitial whose cache status is being queried.
	 */
	public static boolean isAdCached(String sPlacementName) {
		BurstlyView burstlyView = mBurstlyViewHashMap.get(sPlacementName);
		if (burstlyView == null) return false;
		
		return mBurstlyViewCachedHashMap.get(sPlacementName);
	}
	
	/*
	 * Sets the origin of the banner ad to the given (x, y) pair on the screen. Note that the origin references the top-left corner of the banner 
	 * and ad it is in relation to the screen coordinates given any rotation that has occurred, with the top left of the screen being (0, 0).
	 * 
	 * @param	sPlacementName	The placement name for the banner that should be repositioned to be off (originX, originY)
	 * 							from the origin (the top left of the screen).
	 */
	public static void setBannerOrigin(final String sPlacementName, final float originX, final float originY) {
		final BurstlyView burstlyView = mBurstlyViewHashMap.get(sPlacementName);
		if (burstlyView == null) return;
		
		mActivity.runOnUiThread(new Runnable() {

			@Override
			public void run() {
				RelativeLayout.LayoutParams lp = (LayoutParams)burstlyView.getLayoutParams();
				lp.topMargin = (int)originY;
		        lp.leftMargin = (int)originX;
		        burstlyView.setLayoutParams(lp);
			}
			
		});
	}
	
	
	
	public static void setBannerRefreshRate(final String sPlacementName, final float refreshRate) {
		final BurstlyView burstlyView = mBurstlyViewHashMap.get(sPlacementName);
		if (burstlyView == null) return;
		
		mActivity.runOnUiThread(new Runnable() {

			@Override
			public void run() {
				burstlyView.setDefaultSessionLife((int) refreshRate);
			}
			
		});
	}
	
	public static void setTargettingParameters(final String sPlacementName, final String sTargettingParameters) {
		final BurstlyView burstlyView = mBurstlyViewHashMap.get(sPlacementName);
		if (burstlyView == null) return;
		
		mActivity.runOnUiThread(new Runnable() {

			@Override
			public void run() {
				burstlyView.setPubTargetingParams(sTargettingParameters);
			}
			
		});
	}
	
	public static void setAdParameters(final String sPlacementName, final String sAdParameters) {
		final BurstlyView burstlyView = mBurstlyViewHashMap.get(sPlacementName);
		if (burstlyView == null) return;
		
		mActivity.runOnUiThread(new Runnable() {

			@Override
			public void run() {
				burstlyView.setCrParms(sAdParameters);
			}
			
		});
	}
	
	
	/*
	 * Sets the name of the callback GameObject to use UnitySendMessage with. Calls the BurstlyCallback method of the GameObject with a string
	 * parameter representing the placementName|callbackEvent (pipe-delimited).
	 * 
	 * @param	sCallbackGameObjectName		The GameObject name to call UnitySendMessage. Calls its BurstlyCallback method with a string
	 * 										parameter representing the placementName|callbackEvent (pipe-delimited).
	 */
	public static void setCallbackGameObjectName(String sCallbackGameObjectName) {
		mCallbackGameObjectName = sCallbackGameObjectName; 
	}
	
	
	
	/********************************************************************************/
	/* INTERNAL JAVA METHODS - CALLED BY INTERNAL LOGIC TO FACILITATE FUNCTIONALITY */
	/********************************************************************************/              
    
	/*
	 * Invokes UnitySendMessage to send a callback to the GameObject name set in setCallbackGameObjectName(String).
	 * 
	 * @param	placementName	The placement for which the callback pertains.
	 * @param	callbackEvent	The callback event.
	 */
	protected static void sendCallback(final String placementName, final BurstlyEvent callbackEvent) {
		mActivity.runOnUiThread(new Runnable() {

			@Override
			public void run() {
				// Update the cached status of this placement
				if (callbackEvent == BurstlyEvent.BurstlyEventCached)
					mBurstlyViewCachedHashMap.put(placementName, true);

				if (mCallbackGameObjectName == null) return;
				
				UnityPlayer.UnitySendMessage(mCallbackGameObjectName, "BurstlyCallback", placementName + "|" + callbackEvent.getEventCode());
			}
			
		});
	}
	
}
