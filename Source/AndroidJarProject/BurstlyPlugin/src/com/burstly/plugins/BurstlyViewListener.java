package com.burstly.plugins;

import com.burstly.lib.ui.AdSize;
import com.burstly.lib.ui.IBurstlyAdListener;
import com.burstly.plugins.BurstlyAdWrapper.BurstlyEvent;

class BurstlyViewListener implements IBurstlyAdListener {
	
	public String mPlacementName = "";
	
	public void setPlacementName(String sPlacementName) {
		mPlacementName = sPlacementName;
	}

	@Override
	public void onCollapse() {
		
	}

	@Override
	public void onExpand(boolean arg0) {
		
	}

	@Override
	public void onHide() {
		BurstlyAdWrapper.sendCallback(mPlacementName, BurstlyEvent.BurstlyEventHidden);
	}

	@Override
	public void onShow() {
		BurstlyAdWrapper.sendCallback(mPlacementName, BurstlyEvent.BurstlyEventShown);
	}

	@Override
	public void viewDidChangeSize(AdSize fromSize, AdSize toSize) {
		
	}

	@Override
	public void adNetworkDismissFullScreen(String adNetwork) {
		BurstlyAdWrapper.sendCallback(mPlacementName, BurstlyEvent.BurstlyEventDismissFullscreen);
	}

	@Override
	public void adNetworkPresentFullScreen(String adNetwork) {
		BurstlyAdWrapper.sendCallback(mPlacementName, BurstlyEvent.BurstlyEventTakeoverFullscreen);
	}

	@Override
	public void adNetworkWasClicked(String adNetwork) {
		BurstlyAdWrapper.sendCallback(mPlacementName, BurstlyEvent.BurstlyEventClicked);
	}

	@Override
	public void attemptingToLoad(String adNetwork) {
		
	}

	@Override
	public void didLoad(String adNetwork, boolean interstitial) {
		BurstlyAdWrapper.sendCallback(mPlacementName, BurstlyEvent.BurstlyEventSucceeded);
	}

	@Override
	public void didPrecacheAd(String adNetwork) {
		BurstlyAdWrapper.sendCallback(mPlacementName, BurstlyEvent.BurstlyEventCached);
	}

	@Override
	public void failedToDisplayAds() {
		BurstlyAdWrapper.sendCallback(mPlacementName, BurstlyEvent.BurstlyEventFailed);
	}

	@Override
	public void failedToLoad(String adNetwork) {
		
	}

	@Override
	public void finishRequestToServer() {
		
	}

	@Override
	public void requestThrottled(int msThrottled) {
		
	}

	@Override
	public void startRequestToServer() {
		
	}
	

}