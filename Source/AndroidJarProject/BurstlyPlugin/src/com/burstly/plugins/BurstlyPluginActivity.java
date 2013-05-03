package com.burstly.plugins;

import android.os.Bundle;
import com.unity3d.player.UnityPlayerActivity;

public class BurstlyPluginActivity extends UnityPlayerActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        BurstlyAdWrapper.init(this);
        BurstlyCurrencyWrapper.init(this);
        
    	BurstlyAdWrapper.createViewLayout();
    }
    
    @Override
    public void onResume() {
    	BurstlyCurrencyWrapper.updateBalancesFromServer();
    	BurstlyAdWrapper.onResumeActivity(this);
        super.onResume();
    }

    @Override
    public void onPause() {
    	BurstlyCurrencyWrapper.updateBalancesFromServer();
    	BurstlyAdWrapper.onPauseActivity(this);
        super.onPause();
    }

    @Override
    public void onDestroy() {
    	BurstlyCurrencyWrapper.updateBalancesFromServer();
    	BurstlyAdWrapper.onDestroyActivity(this);
        super.onDestroy();
    }

}
