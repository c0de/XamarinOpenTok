using System;
using ObjCRuntime;

//[assembly: LinkWith ("OpenTok.a", LinkTarget.ArmV7 | LinkTarget.ArmV7s | LinkTarget.Simulator, SmartLink = true, ForceLoad = true)]

// Linker Settings
[assembly: LinkWith("OpenTok.a", 
	LinkTarget.ArmV7 | LinkTarget.ArmV7s | LinkTarget.Arm64 | LinkTarget.Simulator | LinkTarget.Simulator64, 
	Frameworks = "SystemConfiguration OpenGLES CoreTelephony CoreFoundation CoreMedia CoreVideo CoreGraphics AudioToolBox AVFoundation SceneKit QuartzCore", 
	WeakFrameworks = "GLKit",
	LinkerFlags = "-lstdc++.6.0.9 -lpthread -lsqlite3 -lxml2 -ObjC", 
	IsCxx = true, 
	SmartLink = true, 
	ForceLoad = true)]