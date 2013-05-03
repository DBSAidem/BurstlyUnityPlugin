//
//  BurstlyAdViewViewController.m
//  

#import "BurstlyAdViewView.h"

@implementation BurstlyAdViewView

- (UIView *)hitTest:(CGPoint)point withEvent:(UIEvent *)event {
    NSEnumerator *reverseE = [self.subviews reverseObjectEnumerator];
    UIView *iSubView;

    while ((iSubView = [reverseE nextObject])) {

        UIView *viewWasHit = [iSubView hitTest:[self convertPoint:point toView:iSubView] withEvent:event];
        if (viewWasHit) {
            return viewWasHit;
        }

    }
    // Pass touches through if we aren't hitting a subview
    return nil;
}

@end
