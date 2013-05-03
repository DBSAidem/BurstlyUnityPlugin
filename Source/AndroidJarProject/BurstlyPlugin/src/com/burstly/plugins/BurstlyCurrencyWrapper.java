package com.burstly.plugins;

import android.app.Activity;

import com.burstly.lib.currency.CurrencyManager;
import com.unity3d.player.UnityPlayer;

public class BurstlyCurrencyWrapper {
	
	private static Activity mActivity = null;
	private static String mCallbackGameObjectName = null;
	
	private static CurrencyManager mCurrencyManager = null;
	
	/*****************************************************************/
	/* ANDROID JAVA METHODS - MUST BE CALLED WITHIN JAVA ENVIRONMENT */
	/*****************************************************************/
	
	/*
	 * Initialises BurstlyCurrencyWrapper. Must be called before any views are created in your activity.
	 * 
	 *  @param	aActivity	The main activity for your app
	 */
	public static void init(Activity aActivity) {
		mActivity = aActivity;
	}
	
	
	/*
		Initializes the BurstlyCurrency plugin. This method *must* be called before any other BurstlyCurrency method is called.
		You must pass in publisherId. userId may be passed in as an empty string ("") if you would like to use the default
		userId handled by BurstlyCurrency. DO NOT pass in NULL if there is no userId.
	 */
	public static void initialize(final String publisherId, final String userId) {
		if (mCurrencyManager != null) return;
		
		mActivity.runOnUiThread(new Runnable() {

			@Override
			public void run() {
				mCurrencyManager = new CurrencyManager(); 
				
				if (userId.length() == 0)
					mCurrencyManager.initManager(mActivity, publisherId);
				else
					mCurrencyManager.initManager(mActivity, publisherId, userId); 
				
				mCurrencyManager.addCurrencyListener(new BurstlyCurrencyListener());
				
				BurstlyCurrencyWrapper.updateBalancesFromServer();
			}
			
		});
	}
	
	/*
		Returns the currency balance for the currency name passed in the parameters held in the local cache. This is updated from the
		server upon calling updateBalancesFromServer().
	 */	
	public static int getBalance(String currency) {
		if (mCurrencyManager == null) return 0;
		
		return mCurrencyManager.getBalance(currency);
	}
	
	/*
		Increases the currency balance for the passed currency by the passed amount. This updates the local currency cache and also
		updates the Burstly server balance as well.
	 */	
	public static void increaseBalance(final String currency, final int amount) {
		if (mCurrencyManager == null) return;
		
		mActivity.runOnUiThread(new Runnable() {

			@Override
			public void run() {
				mCurrencyManager.increaseBalance(amount, currency);
				BurstlyCurrencyWrapper.updateBalancesFromServer();
			}
			
		});
	}
	
	/*
		Decreases the currency balance for the passed currency by the passed amount. This updates the local currency cache and also
		updates the Burstly server balance as well.
	 */
	public static void decreaseBalance(final String currency, final int amount) {
		if (mCurrencyManager == null) return;

		mActivity.runOnUiThread(new Runnable() {

			@Override
			public void run() {
				mCurrencyManager.decreaseBalance(amount, currency);
				BurstlyCurrencyWrapper.updateBalancesFromServer();
			}
			
		});
	}
	
    /*
    	Initiates a request to update the currency balances for all currencies from the Burstly server. You must register a 
    	callback using the methods below to receive notifications that this method succeeded / failed.
	 */
	public static void updateBalancesFromServer() {
		if (mCurrencyManager == null) return;
		
		mActivity.runOnUiThread(new Runnable() {

			@Override
			public void run() {
				try {
					mCurrencyManager.checkForUpdate();
				} catch (Exception e) {
					BurstlyCurrencyWrapper.sendCallback(false);
				} 
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
	protected static void sendCallback(final boolean success) {
		if (mCallbackGameObjectName == null) return;
		
		mActivity.runOnUiThread(new Runnable() {

			@Override
			public void run() {
				UnityPlayer.UnitySendMessage(mCallbackGameObjectName, "BurstlyCallback", (success ? "UPDATED" : "FAILED"));
			}
			
		});
	}
	
	
}
