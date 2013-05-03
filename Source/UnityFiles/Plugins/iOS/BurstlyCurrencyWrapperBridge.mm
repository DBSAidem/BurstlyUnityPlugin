//
//  BurstlyCurrencyWrapperBridge.m
//  BurstlyC++Plugin
//
//  Created by abishek ashok on 2/12/13.
//  Copyright (c) 2013 abishek ashok. All rights reserved.
//

#import "BurstlyCurrencyWrapperBridge.h"


@implementation BurstlyCurrencyWrapperBridge

static BurstlyCurrencyWrapperBridge *_sharedInstance;


#pragma mark Singleton Implementation

+ (id)allocWithZone:(NSZone*)zone {
	@synchronized(self) {
		if (_sharedInstance == nil) {
			_sharedInstance = [super allocWithZone:zone];
			return _sharedInstance; // assignment and return on first allocation
		}
	}
	return nil; //on subsequent allocation attempts return nil
}

- (id)copyWithZone:(NSZone *)zone { return self; }

- (id)retain { return self; }

- (unsigned)retainCount {
	return UINT_MAX; //denotes an object that cannot be released
}

- (oneway void)release {
	//do nothing
}

- (id)autorelease { return self; }

- (id)init {
    self = [super init];
    if (self) {
		_initialized = NO;
    }
    return self;
}

+ (BurstlyCurrencyWrapperBridge *)sharedInstance {
	@synchronized(self) {
		if (_sharedInstance == nil) {
			[[self alloc] init]; // assignment not done here
		}
	}
	return _sharedInstance;
}

#pragma mark Class Implementation

- (void)initializeWithPublisherId:(NSString *)publisherId andUserId:(NSString *)userId {
	if (!publisherId || !userId || [publisherId compare:@""] == NSOrderedSame) return;
	
	if ([userId compare:@""] == NSOrderedSame) {
		[[BurstlyCurrency sharedCurrencyManager] setPublisherId:publisherId];
	} else {
		[[BurstlyCurrency sharedCurrencyManager] setPublisherId:publisherId andUserId:userId];
	}
	
	_initialized = YES;
	
	[[BurstlyCurrencyWrapperBridge sharedInstance] updateBalancesFromServer];
}

- (NSInteger)getBalanceForCurrency:(NSString *)currency {
	if (!_initialized) return 0;
		
	return [[BurstlyCurrency sharedCurrencyManager] currentBalanceForCurrency:currency];
}

- (void)increaseBalanceForCurrency:(NSString *)currency byAmount:(NSInteger)amount {
	if (!_initialized) return;

	[[BurstlyCurrency sharedCurrencyManager] increaseBalance:amount forCurrency:currency];
		
	[[BurstlyCurrencyWrapperBridge sharedInstance] updateBalancesFromServer];
}

- (void)decreaseBalanceForCurrency:(NSString *)currency byAmount:(NSInteger)amount {
	if (!_initialized) return;
	
	[[BurstlyCurrency sharedCurrencyManager] decreaseBalance:amount forCurrency:currency];
	
	[[BurstlyCurrencyWrapperBridge sharedInstance] updateBalancesFromServer];	
}

- (void)updateBalancesFromServer {
	if (!_initialized) return;
	
	[[BurstlyCurrency sharedCurrencyManager] checkForUpdate];
}

#pragma mark BurstlyCurrencyDelegate implementation

- (void)currencyManager:(BurstlyCurrency *)manager didUpdateBalances:(NSDictionary *)balances {
	BurstlyCurrencyWrapper_callback(true);		
}

- (void)currencyManager:(BurstlyCurrency *)manager didFailToUpdateBalanceWithError:(NSError *)error {
	BurstlyCurrencyWrapper_callback(false);
}

@end
