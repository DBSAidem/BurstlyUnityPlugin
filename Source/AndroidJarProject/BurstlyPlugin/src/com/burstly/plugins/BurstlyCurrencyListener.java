package com.burstly.plugins;

import java.util.Map;

import com.burstly.lib.currency.BalanceUpdateInfo;
import com.burstly.lib.feature.currency.ICurrencyListener;

public class BurstlyCurrencyListener implements ICurrencyListener {

	@Override
	public void didFailToUpdateBalance(Map<String, BalanceUpdateInfo> balanceUpdateMap) {
		BurstlyCurrencyWrapper.sendCallback(false);
	}

	@Override
	public void didUpdateBalance(Map<String, BalanceUpdateInfo> balanceUpdateMap) {
		BurstlyCurrencyWrapper.sendCallback(true);
	}
	
}
