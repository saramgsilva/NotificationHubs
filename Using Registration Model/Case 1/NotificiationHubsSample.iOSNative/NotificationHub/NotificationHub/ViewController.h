//
//  ViewController.h
//  NotificationHub
//
//  Created by Edgar Clérigo on 16/06/14.
//  Copyright (c) 2014 . All rights reserved.
//

#import <UIKit/UIKit.h>
#import <WindowsAzureMessaging/WindowsAzureMessaging.h>

@interface ViewController : UIViewController
@property (nonatomic,strong) SBNotificationHub *hub;
@end
