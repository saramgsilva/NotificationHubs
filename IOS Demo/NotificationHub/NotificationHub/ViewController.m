//
//  ViewController.m
//  NotificationHub
//
//  Created by Edgar Clérigo on 16/06/14.
//  Copyright (c) 2014 . All rights reserved.
//

#import "ViewController.h"
#import "AppDelegate.h"

@interface ViewController ()
@property (strong, nonatomic) IBOutlet UISwitch *sportsSwitch;
@property (strong, nonatomic) IBOutlet UISwitch *musicSwitch;
@property (strong, nonatomic) IBOutlet UISwitch *newsSwitch;

@end

@implementation ViewController

- (void)viewDidLoad
{
    [super viewDidLoad];
	// Do any additional setup after loading the view, typically from a nib.
}

- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

- (IBAction)registerClick:(id)sender
{
    NSMutableSet *registerFor = nil;
    
    if ([_sportsSwitch isOn])
    {
        if (!registerFor)
            registerFor = [[NSMutableSet alloc] init];
        [registerFor addObject:@"sports"];
    }
    
    if ([_musicSwitch isOn])
    {
        if (!registerFor)
            registerFor = [[NSMutableSet alloc] init];
        [registerFor addObject:@"music"];
    }
    
    if ([_newsSwitch isOn])
    {
        if (!registerFor)
            registerFor = [[NSMutableSet alloc] init];
        [registerFor addObject:@"news"];
    }
    
    [((AppDelegate*)[[UIApplication sharedApplication] delegate]) registerInNotificationHUBWithTags:[registerFor mutableCopy]];
}
@end
