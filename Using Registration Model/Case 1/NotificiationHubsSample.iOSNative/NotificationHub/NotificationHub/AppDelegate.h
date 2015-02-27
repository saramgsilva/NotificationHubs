//
//  AppDelegate.h
//  NotificationHub
//
//  Created by Edgar Clérigo on 16/06/14.
//  Copyright (c) 2014 . All rights reserved.
//

#import <UIKit/UIKit.h>

@interface AppDelegate : UIResponder <UIApplicationDelegate>

@property (strong, nonatomic) UIWindow *window;

-(void)registerInNotificationHUBWithTags:(NSSet*)tags;
@end
