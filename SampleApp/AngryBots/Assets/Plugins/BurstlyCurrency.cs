using System;
using UnityEngine;
using System.Runtime.InteropServices;

public static class BurstlyCurrency {
	
	
	/***********************************************************/
	/*   Private methods to interface with the native C code   */
	/***********************************************************/
	 
	#if UNITY_IPHONE 

		[DllImport ("__Internal")]	
		private static extern void BurstlyCurrencyWrapper_initialize(string publisherId, string userId);
		
		[DllImport ("__Internal")]	
		private static extern int BurstlyCurrencyWrapper_getBalance(string currency);
		
		[DllImport ("__Internal")]	
		private static extern void BurstlyCurrencyWrapper_increaseBalance(string currency, int amount);
		
		[DllImport ("__Internal")]	
		private static extern void BurstlyCurrencyWrapper_decreaseBalance(string currency, int amount);

		[DllImport ("__Internal")]	
		private static extern void BurstlyCurrencyWrapper_updateBalancesFromServer();
		
		[DllImport ("__Internal")]	
		private static extern void BurstlyCurrencyWrapper_setCallbackGameObjectName(string callbackGameObjectName);

	#endif
	
	#if UNITY_ANDROID
	
		private static IntPtr BurstlyPluginClassLocalReference = AndroidJNI.FindClass("com/burstly/plugins/BurstlyCurrencyWrapper");
		private static IntPtr BurstlyPluginClass = AndroidJNI.NewGlobalRef(BurstlyPluginClassLocalReference);

    	private static IntPtr methodID_initialize = AndroidJNI.GetStaticMethodID(BurstlyPluginClass, "initialize", "(Ljava/lang/String;Ljava/lang/String;)V");
    	private static IntPtr methodID_getBalance = AndroidJNI.GetStaticMethodID(BurstlyPluginClass, "getBalance", "(Ljava/lang/String;)I");	
    	private static IntPtr methodID_increaseBalance = AndroidJNI.GetStaticMethodID(BurstlyPluginClass, "increaseBalance", "(Ljava/lang/String;I)V");	
    	private static IntPtr methodID_decreaseBalance = AndroidJNI.GetStaticMethodID(BurstlyPluginClass, "decreaseBalance", "(Ljava/lang/String;I)V");
    	private static IntPtr methodID_updateBalancesFromServer = AndroidJNI.GetStaticMethodID(BurstlyPluginClass, "updateBalancesFromServer", "()V");
    	private static IntPtr methodID_setCallbackGameObjectName = AndroidJNI.GetStaticMethodID(BurstlyPluginClass, "setCallbackGameObjectName", "(Ljava/lang/String;)V");		

		private static void BurstlyCurrencyWrapper_initialize(string publisherId, string userId) {
			jvalue[] args = new jvalue[2];
      		args[0].l = AndroidJNI.NewStringUTF(publisherId);
      		args[1].l = AndroidJNI.NewStringUTF(userId);
			AndroidJNI.CallStaticVoidMethod(BurstlyPluginClass, methodID_initialize, args);
		}

		private static int BurstlyCurrencyWrapper_getBalance(string currency) {
			jvalue[] args = new jvalue[1];
      		args[0].l = AndroidJNI.NewStringUTF(currency);
			return AndroidJNI.CallStaticIntMethod(BurstlyPluginClass, methodID_getBalance, args);	
		}

		private static void BurstlyCurrencyWrapper_increaseBalance(string currency, int amount) {
			jvalue[] args = new jvalue[2];
      		args[0].l = AndroidJNI.NewStringUTF(currency);
      		args[1].i = amount;
			AndroidJNI.CallStaticVoidMethod(BurstlyPluginClass, methodID_increaseBalance, args);
		}

		private static void BurstlyCurrencyWrapper_decreaseBalance(string currency, int amount) {
			jvalue[] args = new jvalue[2];
      		args[0].l = AndroidJNI.NewStringUTF(currency);
      		args[1].i = amount;
			AndroidJNI.CallStaticVoidMethod(BurstlyPluginClass, methodID_decreaseBalance, args);
		}
		
		private static void BurstlyCurrencyWrapper_updateBalancesFromServer() {
			jvalue[] args = new jvalue[0];
			AndroidJNI.CallStaticVoidMethod(BurstlyPluginClass, methodID_updateBalancesFromServer, args);
		}
		
		private static void BurstlyCurrencyWrapper_setCallbackGameObjectName(string callbackGameObjectName) {
			jvalue[] args = new jvalue[1];
      		args[0].l = AndroidJNI.NewStringUTF(callbackGameObjectName);
			AndroidJNI.CallStaticVoidMethod(BurstlyPluginClass, methodID_setCallbackGameObjectName, args);
		}
		
	#endif

	
	/************************************************************************/
	/*   Public methods to interface with C#/Javascript code within Unity   */
	/************************************************************************/
	
	/*
		Initializes the BurstlyCurrency plugin. This method *must* be called before any other BurstlyCurrency method is called.
		You must pass in publisherId. userId may be passed in as an empty string ("") if you would like to use the default
		userId handled by BurstlyCurrency. DO NOT pass in NULL if there is no userId.
	 */
	public static void initialize(string publisherId, string userId) {
		if ((Application.platform == RuntimePlatform.OSXEditor) || (Application.platform == RuntimePlatform.WindowsEditor)) return;
		
		BurstlyCurrencyWrapper_initialize(publisherId, userId);
	}
	
	/*
		Returns the currency balance for the currency name passed in the parameters held in the local cache. This is updated from the
		server upon calling updateBalancesFromServer().
	 */
	public static int getBalance(string currency) {
		if ((Application.platform == RuntimePlatform.OSXEditor) || (Application.platform == RuntimePlatform.WindowsEditor)) return 0;
		
		return BurstlyCurrencyWrapper_getBalance(currency);
	} 

	/*
		Increases the currency balance for the passed currency by the passed amount. This updates the local currency cache and also
		updates the Burstly server balance as well.
	 */	
	public static void increaseBalance(string currency, int amount) {
		if ((Application.platform == RuntimePlatform.OSXEditor) || (Application.platform == RuntimePlatform.WindowsEditor)) return;
		
		BurstlyCurrencyWrapper_increaseBalance(currency, amount);
	}
	
	/*
		Decreases the currency balance for the passed currency by the passed amount. This updates the local currency cache and also
		updates the Burstly server balance as well.
	 */
	public static void decreaseBalance(string currency, int amount) {
		if ((Application.platform == RuntimePlatform.OSXEditor) || (Application.platform == RuntimePlatform.WindowsEditor)) return;
		
		BurstlyCurrencyWrapper_decreaseBalance(currency, amount);
	}
	
	/*
		Initiates a request to update the currency balances for all currencies from the Burstly server. You must use the
		setCallbackGameObjectName(string callbackGameObjectName) to set a callback GameObject to receive a callback when this request
		either finishes successfully or fails. The GameObject must have a method called BurstlyCallback(string), where the string passed
		by the BurstlyCurrency plugin will either be UPDATED or FAILED, depending on whether the update request is successful or fails.
	 */
	public static void updateBalancesFromServer() {
		if ((Application.platform == RuntimePlatform.OSXEditor) || (Application.platform == RuntimePlatform.WindowsEditor)) return;
		
		BurstlyCurrencyWrapper_updateBalancesFromServer();
	}
		
	/*
		Sets the GameObject name whose BurstlyCallback method should be called. This method should have a string parameter. The plugin 
		will pass back a string as follows:
			UPDATED		if the currency balances were successfully updated from the server
			FAILED		if the currency balances failed to update from the server
	 */
	public static void setCallbackGameObjectName(string callbackGameObjectName) {
		if ((Application.platform == RuntimePlatform.OSXEditor) || (Application.platform == RuntimePlatform.WindowsEditor)) return;
		
		BurstlyCurrencyWrapper_setCallbackGameObjectName(callbackGameObjectName);
	}
	
}
