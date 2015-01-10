//
//  AppDelegate.m
//  NotificationHub
//
//  Created by Edgar Clérigo on 16/06/14.
//  Copyright (c) 2014 . All rights reserved.
//

#import "AppDelegate.h"
#import <WindowsAzureMessaging/WindowsAzureMessaging.h>

@interface AppDelegate ()
@property (nonatomic,strong) SBNotificationHub *hub;
@property (nonatomic,strong) NSData *token;
@end
@implementation AppDelegate
@synthesize hub,token;

- (BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions
{
    // Override point for customization after application launch.
    [[UIApplication sharedApplication] registerForRemoteNotificationTypes:(UIRemoteNotificationTypeAlert | UIRemoteNotificationTypeBadge | UIRemoteNotificationTypeSound)];
    
#warning Did you change the connection string and the hub name ?
    hub = [[SBNotificationHub alloc] initWithConnectionString:@"<CONNECTION STRING>" notificationHubPath:@"<HUB NAME>"];
    
    return YES;
}
							
- (void)applicationWillResignActive:(UIApplication *)application
{
    // Sent when the application is about to move from active to inactive state. This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) or when the user quits the application and it begins the transition to the background state.
    // Use this method to pause ongoing tasks, disable timers, and throttle down OpenGL ES frame rates. Games should use this method to pause the game.
}

- (void)applicationDidEnterBackground:(UIApplication *)application
{
    // Use this method to release shared resources, save user data, invalidate timers, and store enough application state information to restore your application to its current state in case it is terminated later. 
    // If your application supports background execution, this method is called instead of applicationWillTerminate: when the user quits.
}

- (void)applicationWillEnterForeground:(UIApplication *)application
{
    // Called as part of the transition from the background to the inactive state; here you can undo many of the changes made on entering the background.
}

- (void)applicationDidBecomeActive:(UIApplication *)application
{
    // Restart any tasks that were paused (or not yet started) while the application was inactive. If the application was previously in the background, optionally refresh the user interface.
}

- (void)applicationWillTerminate:(UIApplication *)application
{
    // Called when the application is about to terminate. Save data if appropriate. See also applicationDidEnterBackground:.
}

-(void)application:(UIApplication *)application didRegisterForRemoteNotificationsWithDeviceToken:(NSData *)deviceToken
{
    token = deviceToken;
    
    [hub registerNativeWithDeviceToken:deviceToken tags:nil  completion:^(NSError *error) {
        if (error)
        {
            NSLog(@"error registerNativeWithDeviceToken: \n %@",error.localizedDescription);
        } else
        {
            UIAlertView *alert = [[UIAlertView alloc] initWithTitle:nil message:@"Device registado no NotificationHUB"  delegate:nil cancelButtonTitle:@"Ok" otherButtonTitles:nil];
            
            [alert show];
            NSLog(@"success registerNativeWithDeviceToken");
        }
    }];
}

-(void)application:(UIApplication *)application didReceiveRemoteNotification:(NSDictionary *)userInfo
{
    UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Remote notification" message:[[userInfo objectForKey:@"aps"] objectForKey:@"alert"]  delegate:nil cancelButtonTitle:@"Ok" otherButtonTitles:nil];
    
    [alert show];
}

-(void)registerInNotificationHUBWithTags:(NSSet*)tags
{
    if (!hub)
    {
#warning Did you change the connection string and the hub name ?
        hub = [[SBNotificationHub alloc] initWithConnectionString:@"<CONNECTION STRING>" notificationHubPath:@"<HUB NAME>"];
    }
    
    [hub registerNativeWithDeviceToken:token tags:tags completion:^(NSError *error) {
        if (error)
        {
            NSLog(@"error register for notifications: \n %@",error.localizedDescription);
        } else
        {
            UIAlertView *alert = [[UIAlertView alloc] initWithTitle:nil message:@"Registo alterado no NotificationHUB"  delegate:nil cancelButtonTitle:@"Ok" otherButtonTitles:nil];
            
            [alert show];

            NSLog(@"Success");
            
        }
    }];
}
@end
