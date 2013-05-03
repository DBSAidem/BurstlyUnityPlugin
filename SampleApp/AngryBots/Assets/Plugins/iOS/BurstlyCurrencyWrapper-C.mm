#import <stdio.h>

#import "BurstlyCurrencyWrapper-C.h"
#import "BurstlyCurrencyWrapperBridge.h"


extern "C" {
	
	void BurstlyCurrencyWrapper_initialize(const char *publisherId, const char *userId) {
		[[BurstlyCurrencyWrapperBridge sharedInstance] initializeWithPublisherId:CreateNSString(publisherId) andUserId:CreateNSString(userId)];
	}
	
	int BurstlyCurrencyWrapper_getBalance(const char *currency) {
		return [[BurstlyCurrencyWrapperBridge sharedInstance] getBalanceForCurrency:CreateNSString(currency)];		
	}
	
	void BurstlyCurrencyWrapper_increaseBalance(const char *currency, int amount) {
		[[BurstlyCurrencyWrapperBridge sharedInstance] increaseBalanceForCurrency:CreateNSString(currency) byAmount:amount];
	}
	
	void BurstlyCurrencyWrapper_decreaseBalance(const char *currency, int amount) {
		[[BurstlyCurrencyWrapperBridge sharedInstance] decreaseBalanceForCurrency:CreateNSString(currency) byAmount:amount];
	}
	
	void BurstlyCurrencyWrapper_updateBalancesFromServer() {
		[[BurstlyCurrencyWrapperBridge sharedInstance] updateBalancesFromServer];
	}
		
}