//
//  BurstlyCurrencyWrapperBridge.h
//  BurstlyC++Plugin
//
//  Created by abishek ashok on 2/12/13.
//  Copyright (c) 2013 abishek ashok. All rights reserved.
//

#import <Foundation/Foundation.h>

#import "BurstlyCurrencyWrapper.h"

#import "BurstlyCurrency.h"

@interface BurstlyCurrencyWrapperBridge : NSObject <BurstlyCurrencyDelegate> {
	BOOL _initialized;
}

+ (BurstlyCurrencyWrapperBridge *)sharedInstance;

- (void)initializeWithPublisherId:(NSString *)publisherId andUserId:(NSString *)userId;
- (NSInteger)getBalanceForCurrency:(NSString *)currency;
- (void)increaseBalanceForCurrency:(NSString *)currency byAmount:(NSInteger)amount;
- (void)decreaseBalanceForCurrency:(NSString *)currency byAmount:(NSInteger)amount;
- (void)updateBalancesFromServer;
	
@end
