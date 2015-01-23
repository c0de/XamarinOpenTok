using System;
using ObjCRuntime;

//[assembly: LinkWith ("OpenTok.a", LinkTarget.ArmV7 | LinkTarget.ArmV7s | LinkTarget.Simulator, SmartLink = true, ForceLoad = true)]

// Linker Settings
[assembly: LinkWith("OpenTok.a", 
	LinkTarget.ArmV7 | LinkTarget.ArmV7s | LinkTarget.Simulator, 
	Frameworks = "GLKit CoreTelephony SystemConfiguration OpenGLES CoreFoundation CoreMedia CoreVideo CoreGraphics AudioToolBox AVFoundation SceneKit QuartzCore UIKit Foundation", 
	LinkerFlags = "-lstdc++.6.0.9 -lpthread -lsqlite3 -lxml2 -ObjC", 
	IsCxx = true, 
	SmartLink = true, 
	ForceLoad = true)]
