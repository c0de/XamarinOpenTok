using System;
using System.Drawing;

using ObjCRuntime;
using Foundation;
using UIKit;
using AVFoundation;
using CoreTelephony;
using CoreFoundation;
using CoreMedia;

namespace OpenTok.Binding.Ios
{

	// @interface OTSession : NSObject
	[BaseType (typeof(NSObject))]
	interface OTSession
	{

		// -(id)initWithApiKey:(NSString *)apiKey sessionId:(NSString *)sessionId delegate:(id<OTSessionDelegate>)delegate;
		[Export ("initWithApiKey:sessionId:delegate:")]
		IntPtr Constructor (string apiKey, string sessionId, OTSessionDelegate sessionDelegate);

		// @property (readonly) OTSessionConnectionStatus sessionConnectionStatus;
		[Export ("sessionConnectionStatus")]
		OTSessionConnectionStatus SessionConnectionStatus { get; }

		// @property (readonly) NSString * sessionId;
		[Export ("sessionId")]
		string SessionId { get; }

		// @property (readonly) NSDictionary * streams;
		[Export ("streams")]
		NSDictionary Streams { get; }

		// @property (readonly) OTConnection * connection;
		[Export ("connection")]
		OTConnection Connection { get; }

		// @property (assign, nonatomic) id<OTSessionDelegate> delegate;
		[Export ("delegate", ArgumentSemantic.UnsafeUnretained)]
		[NullAllowed]
		NSObject WeakDelegate { get; set; }

		// @property (assign, nonatomic) id<OTSessionDelegate> delegate;
		[Wrap ("WeakDelegate")]
		OTSessionDelegate Delegate { get; set; }

		// @property (assign, nonatomic) dispatch_queue_t apiQueue;
		[Export ("apiQueue", ArgumentSemantic.UnsafeUnretained)]
		DispatchQueue ApiQueue { get; set; }

		// -(void)connectWithToken:(NSString *)token error:(OTError **)error;
		[Export ("connectWithToken:error:")]
		void ConnectWithToken (string token, out OTError error);

		// -(void)disconnect:(OTError **)error;
		[Export ("disconnect:")]
		void Disconnect (out OTError error);

		// -(void)disconnect;
		[Availability (Deprecated = Platform.iOS_Version | Platform.Mac_Version)]
		[Export ("disconnect")]
		void Disconnect ();

		// -(void)publish:(OTPublisherKit *)publisher error:(OTError **)error;
		[Export ("publish:error:")]
		void Publish (OTPublisherKit publisher, out OTError error);

		// -(void)publish:(OTPublisherKit *)publisher;
		[Availability (Deprecated = Platform.iOS_Version | Platform.Mac_Version)]
		[Export ("publish:")]
		void Publish (OTPublisherKit publisher);

		// -(void)unpublish:(OTPublisherKit *)publisher error:(OTError **)error;
		[Export ("unpublish:error:")]
		void Unpublish (OTPublisherKit publisher, out OTError error);

		// -(void)unpublish:(OTPublisherKit *)publisher;
		[Availability (Deprecated = Platform.iOS_Version | Platform.Mac_Version)]
		[Export ("unpublish:")]
		void Unpublish (OTPublisherKit publisher);

		// -(void)subscribe:(OTSubscriberKit *)subscriber error:(OTError **)error;
		[Export ("subscribe:error:")]
		void Subscribe (OTSubscriberKit subscriber, out OTError error);

		// -(void)subscribe:(OTSubscriberKit *)subscriber;
		[Availability (Deprecated = Platform.iOS_Version | Platform.Mac_Version)]
		[Export ("subscribe:")]
		void Subscribe (OTSubscriberKit subscriber);

		// -(void)unsubscribe:(OTSubscriberKit *)subscriber error:(OTError **)error;
		[Export ("unsubscribe:error:")]
		void Unsubscribe (OTSubscriberKit subscriber, out OTError error);

		// -(void)unsubscribe:(OTSubscriberKit *)subscriber;
		[Availability (Deprecated = Platform.iOS_Version | Platform.Mac_Version)]
		[Export ("unsubscribe:")]
		void Unsubscribe (OTSubscriberKit subscriber);

		// -(void)signalWithType:(NSString *)type string:(NSString *)string connection:(OTConnection *)connection error:(OTError **)error;
		[Export ("signalWithType:string:connection:error:")]
		void SignalWithType (string type, string stringParam, OTConnection connection, out OTError error);
	}

	// @protocol OTSessionDelegate <NSObject>
	[Protocol, Model]
	[BaseType (typeof(NSObject))]
	interface OTSessionDelegate
	{

		// @required -(void)sessionDidConnect:(OTSession *)session;
		[Export ("sessionDidConnect:")]
		[Abstract]
		void SessionDidConnect (OTSession session);

		// @required -(void)sessionDidDisconnect:(OTSession *)session;
		[Export ("sessionDidDisconnect:")]
		[Abstract]
		void SessionDidDisconnect (OTSession session);

		// @required -(void)session:(OTSession *)session didFailWithError:(OTError *)error;
		[Export ("session:didFailWithError:")]
		[Abstract]
		void DidFailWithError (OTSession session, OTError error);

		// @required -(void)session:(OTSession *)session streamCreated:(OTStream *)stream;
		[Export ("session:streamCreated:")]
		[Abstract]
		void StreamCreated (OTSession session, OTStream stream);

		// @required -(void)session:(OTSession *)session streamDestroyed:(OTStream *)stream;
		[Export ("session:streamDestroyed:")]
		[Abstract]
		void StreamDestroyed (OTSession session, OTStream stream);

		// @optional -(void)session:(OTSession *)session connectionCreated:(OTConnection *)connection;
		[Export ("session:connectionCreated:")]
		void ConnectionCreated (OTSession session, OTConnection connection);

		// @optional -(void)session:(OTSession *)session connectionDestroyed:(OTConnection *)connection;
		[Export ("session:connectionDestroyed:")]
		void ConnectionDestroyed (OTSession session, OTConnection connection);

		// @optional -(void)session:(OTSession *)session receivedSignalType:(NSString *)type fromConnection:(OTConnection *)connection withString:(NSString *)string;
		[Export ("session:receivedSignalType:fromConnection:withString:")]
		void ReceivedSignalType (OTSession session, string type, OTConnection connection, string stringParam);

		// @optional -(void)session:(OTSession *)session archiveStartedWithId:(NSString *)archiveId name:(NSString *)name;
		[Export ("session:archiveStartedWithId:name:")]
		void ArchiveStartedWithId (OTSession session, string archiveId, string name);

		// @optional -(void)session:(OTSession *)session archiveStoppedWithId:(NSString *)archiveId;
		[Export ("session:archiveStoppedWithId:")]
		void ArchiveStoppedWithId (OTSession session, string archiveId);
	}

	// @protocol OTVideoCapture <NSObject>
	//[Protocol, Model]
	//[BaseType (typeof (NSObject))]
	//interface OTVideoCapture {
	//
	//}

	// @protocol OTVideoRender <NSObject>
	//[Protocol, Model]
	//[BaseType (typeof (NSObject))]
	//interface OTVideoRender {
	//
	//}

	// @protocol OTPublisherKitDelegate <NSObject>
	//[Protocol, Model]
	//[BaseType (typeof (NSObject))]
	//interface OTPublisherKitDelegate {
	//
	//}

	// @protocol OTPublisherKitAudioLevelDelegate <NSObject>
	//[Protocol, Model]
	//[BaseType (typeof (NSObject))]
	//interface OTPublisherKitAudioLevelDelegate {
	//
	//}

	// @interface OTPublisherKit : NSObject
	[BaseType (typeof (NSObject), Delegates=new string [] {"Delegate"}, Events=new Type [] { typeof (OTPublisherKitDelegate) })]
	interface OTPublisherKit
	{

		// -(id)initWithDelegate:(id<OTPublisherKitDelegate>)delegate;
		[Export ("initWithDelegate:")]
		IntPtr Constructor ([NullAllowed] OTPublisherKitDelegate publisherKeyDelegate);

		// -(id)initWithDelegate:(id<OTPublisherKitDelegate>)delegate name:(NSString *)name;
		[Export ("initWithDelegate:name:")]
		IntPtr Constructor ([NullAllowed] OTPublisherKitDelegate publisherKitDelegate, string name);

		// @property (assign, nonatomic) id<OTPublisherKitDelegate> delegate;
		[Export ("delegate", ArgumentSemantic.UnsafeUnretained)]
		[NullAllowed]
		NSObject WeakDelegate { get; set; }

		// @property (assign, nonatomic) id<OTPublisherKitDelegate> delegate;
		[Wrap ("WeakDelegate")]
		OTPublisherKitDelegate Delegate { get; set; }

		// @property (assign, nonatomic) id<OTPublisherKitAudioLevelDelegate> audioLevelDelegate;
		[Export ("audioLevelDelegate", ArgumentSemantic.UnsafeUnretained)]
		[NullAllowed]
		NSObject WeakAudioLevelDelegate { get; set; }

		// @property (assign, nonatomic) id<OTPublisherKitAudioLevelDelegate> audioLevelDelegate;
		[Wrap ("WeakAudioLevelDelegate")]
		OTPublisherKitAudioLevelDelegate AudioLevelDelegate { get; set; }

		// @property (readonly) OTSession * session;
		[Export ("session")]
		OTSession Session { get; }

		// @property (readonly) OTStream * stream;
		[Export ("stream")]
		OTStream Stream { get; }

		// @property (readonly) NSString * name;
		[Export ("name")]
		string Name { get; }

		// @property (nonatomic) BOOL publishAudio;
		[Export ("publishAudio")]
		bool PublishAudio { get; set; }

		// @property (nonatomic) BOOL publishVideo;
		[Export ("publishVideo")]
		bool PublishVideo { get; set; }

		// @property (retain, nonatomic) id<OTVideoCapture> videoCapture;
		[Export ("videoCapture", ArgumentSemantic.Retain)]
		OTVideoCapture VideoCapture { get; set; }

		// @property (retain, nonatomic) id<OTVideoRender> videoRender;
		[Export ("videoRender", ArgumentSemantic.Retain)]
		OTVideoRender VideoRender { get; set; }
	}

	// @protocol OTPublisherKitDelegate <NSObject>
	[Protocol, Model]
	[BaseType (typeof(NSObject))]
	interface OTPublisherKitDelegate
	{

		// @required -(void)publisher:(OTPublisherKit *)publisher didFailWithError:(OTError *)error;
		[Export ("publisher:didFailWithError:"), EventArgs ("OTPublisherDelegateError")]
		void DidFailWithError (OTPublisher publisher, OTError error);

		// @optional -(void)publisher:(OTPublisherKit *)publisher streamCreated:(OTStream *)stream;
		[Export ("publisher:streamCreated:"), EventArgs ("OTPublisherDelegatePublisher")]
		void StreamCreated (OTPublisherKit publisher, OTStream stream);

		// @optional -(void)publisher:(OTPublisherKit *)publisher streamDestroyed:(OTStream *)stream;
		[Export ("publisher:streamDestroyed:"), EventArgs ("OTPublisherDelegatePublisher")]
		void StreamDestroyed (OTPublisherKit publisher, OTStream stream);

	}

	// @protocol OTPublisherKitAudioLevelDelegate <NSObject>
	[Protocol, Model]
	[BaseType (typeof(NSObject))]
	interface OTPublisherKitAudioLevelDelegate
	{

		// @required -(void)publisher:(OTPublisherKit *)publisher audioLevelUpdated:(float)audioLevel;
		[Export ("publisher:audioLevelUpdated:")]
		[Abstract]
		void AudioLevelUpdated (OTPublisherKit publisher, float audioLevel);
	}

	// @protocol OTVideoRender <NSObject>
	//[Protocol, Model]
	//[BaseType (typeof (NSObject))]
	//interface OTVideoRender {
	//
	//}

	// @protocol OTSubscriberKitDelegate <NSObject>
	//[Protocol, Model]
	//[BaseType (typeof (NSObject))]
	//interface OTSubscriberKitDelegate {
	//
	//}

	// @protocol OTSubscriberKitAudioLevelDelegate <NSObject>
	//[Protocol, Model]
	//[BaseType (typeof (NSObject))]
	//interface OTSubscriberKitAudioLevelDelegate {
	//
	//}

	// @interface OTSubscriberKit : NSObject
	[BaseType (typeof (NSObject), Delegates=new string [] {"Delegate"}, Events=new Type [] { typeof (OTSubscriberKitDelegate) })]
	interface OTSubscriberKit
	{

		// -(id)initWithStream:(OTStream *)stream delegate:(id<OTSubscriberKitDelegate>)delegate;
		[Export ("initWithStream:delegate:")]
		IntPtr Constructor (OTStream stream, [NullAllowed] OTSubscriberKitDelegate subscriberKitDelegate);

		// @property (readonly) OTSession * session;
		[Export ("session")]
		OTSession Session { get; }

		// @property (readonly) OTStream * stream;
		[Export ("stream")]
		OTStream Stream { get; }

		// @property (assign, nonatomic) id<OTSubscriberKitDelegate> delegate;
		[Export ("delegate", ArgumentSemantic.UnsafeUnretained)]
		[NullAllowed]
		NSObject WeakDelegate { get; set; }

		// @property (assign, nonatomic) id<OTSubscriberKitDelegate> delegate;
		[Wrap ("WeakDelegate")]
		OTSubscriberKitDelegate Delegate { get; set; }

		// @property (assign, nonatomic) id<OTSubscriberKitAudioLevelDelegate> audioLevelDelegate;
		[Export ("audioLevelDelegate", ArgumentSemantic.UnsafeUnretained)]
		[NullAllowed]
		NSObject WeakAudioLevelDelegate { get; set; }

		// @property (assign, nonatomic) id<OTSubscriberKitAudioLevelDelegate> audioLevelDelegate;
		[Wrap ("WeakAudioLevelDelegate")]
		OTSubscriberKitAudioLevelDelegate AudioLevelDelegate { get; set; }

		// @property (nonatomic) BOOL subscribeToAudio;
		[Export ("subscribeToAudio")]
		bool SubscribeToAudio { get; set; }

		// @property (nonatomic) BOOL subscribeToVideo;
		[Export ("subscribeToVideo")]
		bool SubscribeToVideo { get; set; }

		// @property (retain, nonatomic) id<OTVideoRender> videoRender;
		[Export ("videoRender", ArgumentSemantic.Retain)]
		OTVideoRender VideoRender { get; set; }
	}

	// @protocol OTSubscriberKitDelegate <NSObject>
	[Protocol, Model]
	[BaseType (typeof(NSObject))]
	interface OTSubscriberKitDelegate
	{

		// @required -(void)subscriberDidConnectToStream:(OTSubscriberKit *)subscriber;
		[Export ("subscriberDidConnectToStream:")]
		void DidConnectToStream (OTSubscriberKit subscriber);

		// @required -(void)subscriber:(OTSubscriberKit *)subscriber didFailWithError:(OTError *)error;
		[Export ("subscriber:didFailWithError:"), EventArgs ("OTSubscriberDelegateError")]
		void DidFailWithError (OTSubscriberKit subscriber, OTError error);

		// @optional -(void)subscriberVideoDisabled:(OTSubscriberKit *)subscriber reason:(OTSubscriberVideoEventReason)reason;
		[Export ("subscriberVideoDisabled:reason:"), EventArgs ("OTSubscriberDelegateSubscriber")]
		void VideoDisabled (OTSubscriberKit subscriber, [NullAllowed] OTSubscriberVideoEventReason reason);

		// @optional -(void)subscriberVideoEnabled:(OTSubscriberKit *)subscriber reason:(OTSubscriberVideoEventReason)reason;
		[Export ("subscriberVideoEnabled:reason:"), EventArgs ("OTSubscriberDelegateSubscriber")]
		void VideoEnabled (OTSubscriberKit subscriber, [NullAllowed] OTSubscriberVideoEventReason reason);

		// @optional -(void)subscriberVideoDisableWarning:(OTSubscriberKit *)subscriber;
		[Export ("subscriberVideoDisableWarning:")]
		void VideoDisableWarning (OTSubscriberKit subscriber);

		// @optional -(void)subscriberVideoDisableWarningLifted:(OTSubscriberKit *)subscriber;
		[Export ("subscriberVideoDisableWarningLifted:")]
		void VideoDisableWarningLifted (OTSubscriberKit subscriber);
	}

	// @protocol OTSubscriberKitAudioLevelDelegate <NSObject>
	[Protocol, Model]
	[BaseType (typeof(NSObject))]
	interface OTSubscriberKitAudioLevelDelegate
	{

		// @required -(void)subscriber:(OTSubscriberKit *)subscriber audioLevelUpdated:(float)audioLevel;
		[Export ("subscriber:audioLevelUpdated:")]
		[Abstract]
		void AudioLevelUpdated (OTSubscriberKit subscriber, float audioLevel);
	}

	// @interface OTStream : NSObject
	[BaseType (typeof(NSObject))]
	interface OTStream
	{

		// @property (readonly) OTConnection * connection;
		[Export ("connection")]
		OTConnection Connection { get; }

		// @property (readonly) OTSession * session;
		[Export ("session")]
		OTSession Session { get; }

		// @property (readonly) NSString * streamId;
		[Export ("streamId")]
		string StreamId { get; }

		// @property (readonly) NSDate * creationTime;
		[Export ("creationTime")]
		NSDate CreationTime { get; }

		// @property (readonly) NSString * name;
		[Export ("name")]
		string Name { get; }

		// @property (readonly) BOOL hasAudio;
		[Export ("hasAudio")]
		bool HasAudio { get; }

		// @property (readonly) BOOL hasVideo;
		[Export ("hasVideo")]
		bool HasVideo { get; }

		// @property (readonly) CGSize videoDimensions;
		[Export ("videoDimensions")]
		SizeF VideoDimensions { get; }
	}

	// @interface OTConnection : NSObject
	[BaseType (typeof(NSObject))]
	interface OTConnection
	{

		// @property (readonly) NSString * connectionId;
		[Export ("connectionId")]
		string ConnectionId { get; }

		// @property (readonly) NSDate * creationTime;
		[Export ("creationTime")]
		NSDate CreationTime { get; }

		// @property (readonly) NSString * data;
		[Export ("data")]
		string Data { get; }
	}

	// @interface OTError : NSError
	[BaseType (typeof(NSError))]
	interface OTError
	{

	}

	// @interface OTVideoFormat : NSObject
	[BaseType (typeof(NSObject))]
	interface OTVideoFormat
	{

		// @property (copy, nonatomic) NSString * name;
		[Export ("name")]
		string Name { get; set; }

		// @property (assign, nonatomic) OTPixelFormat pixelFormat;
		[Export ("pixelFormat", ArgumentSemantic.UnsafeUnretained)]
		OTPixelFormat PixelFormat { get; set; }

		// @property (retain, nonatomic) NSMutableArray * bytesPerRow;
		[Export ("bytesPerRow", ArgumentSemantic.Retain)]
		NSMutableArray BytesPerRow { get; set; }

		// @property (assign, nonatomic) uint32_t imageWidth;
		[Export ("imageWidth", ArgumentSemantic.UnsafeUnretained)]
		uint ImageWidth { get; set; }

		// @property (assign, nonatomic) uint32_t imageHeight;
		[Export ("imageHeight", ArgumentSemantic.UnsafeUnretained)]
		uint ImageHeight { get; set; }

		// @property (assign, nonatomic) double estimatedFramesPerSecond;
		[Export ("estimatedFramesPerSecond", ArgumentSemantic.UnsafeUnretained)]
		double EstimatedFramesPerSecond { get; set; }

		// @property (assign, nonatomic) double estimatedCaptureDelay;
		[Export ("estimatedCaptureDelay", ArgumentSemantic.UnsafeUnretained)]
		double EstimatedCaptureDelay { get; set; }

		// +(OTVideoFormat *)videoFormatI420WithWidth:(uint32_t)width height:(uint32_t)height;
		[Static, Export ("videoFormatI420WithWidth:height:")]
		OTVideoFormat VideoFormatI420WithWidth (uint width, uint height);

		// +(OTVideoFormat *)videoFormatNV12WithWidth:(uint32_t)width height:(uint32_t)height;
		[Static, Export ("videoFormatNV12WithWidth:height:")]
		OTVideoFormat VideoFormatNV12WithWidth (uint width, uint height);
	}

	// @interface OTVideoFrame : NSObject
	[BaseType (typeof(NSObject))]
	interface OTVideoFrame
	{

		// -(id)initWithFormat:(OTVideoFormat *)videoFormat;
		[Export ("initWithFormat:")]
		IntPtr Constructor (OTVideoFormat videoFormat);

		// @property (retain, nonatomic) NSPointerArray * planes;
		[Export ("planes", ArgumentSemantic.Retain)]
		IntPtr Planes { get; set; }

		// @property (assign, nonatomic) CMTime timestamp;
		[Export ("timestamp", ArgumentSemantic.UnsafeUnretained)]
		CMTime Timestamp { get; set; }

		// @property (assign, nonatomic) OTVideoOrientation orientation;
		[Export ("orientation", ArgumentSemantic.UnsafeUnretained)]
		OTVideoOrientation Orientation { get; set; }

		// @property (retain, nonatomic) OTVideoFormat * format;
		[Export ("format", ArgumentSemantic.Retain)]
		OTVideoFormat Format { get; set; }

		// -(void)setPlanesWithPointers:(uint8_t *[])planes numPlanes:(int)numPlanes;
		[Export ("setPlanesWithPointers:numPlanes:")]
		void SetPlanesWithPointers (IntPtr planes, int numPlanes);

		// -(void)clearPlanes;
		[Export ("clearPlanes")]
		void ClearPlanes ();
	}

	// @protocol OTVideoCaptureConsumer <NSObject>
	[Protocol, Model]
	[BaseType (typeof(NSObject))]
	interface OTVideoCaptureConsumer
	{

		// @required -(void)consumeFrame:(OTVideoFrame *)frame;
		[Export ("consumeFrame:")]
		[Abstract]
		void ConsumeFrame (OTVideoFrame frame);
	}

	// @protocol OTVideoCapture <NSObject>
	[Protocol, Model]
	[BaseType (typeof(NSObject))]
	interface OTVideoCapture
	{

		// @property (assign, atomic) id<OTVideoCaptureConsumer> videoCaptureConsumer;
		[Export ("videoCaptureConsumer", ArgumentSemantic.UnsafeUnretained)]
		OTVideoCaptureConsumer VideoCaptureConsumer { get; set; }

		// @required -(void)initCapture;
		[Export ("initCapture")]
		[Abstract]
		void InitCapture ();

		// @required -(void)releaseCapture;
		[Export ("releaseCapture")]
		[Abstract]
		void ReleaseCapture ();

		// @required -(int32_t)startCapture;
		[Export ("startCapture")]
		[Abstract]
		int StartCapture ();

		// @required -(int32_t)stopCapture;
		[Export ("stopCapture")]
		[Abstract]
		int StopCapture ();

		// @required -(BOOL)isCaptureStarted;
		[Export ("isCaptureStarted")]
		[Abstract]
		bool IsCaptureStarted ();

		// @required -(int32_t)captureSettings:(OTVideoFormat *)videoFormat;
		[Export ("captureSettings:")]
		[Abstract]
		int CaptureSettings (OTVideoFormat videoFormat);
	}

	// @protocol OTVideoRender <NSObject>
	[Protocol, Model]
	[BaseType (typeof(NSObject))]
	interface OTVideoRender
	{

		// @required -(void)renderVideoFrame:(OTVideoFrame *)frame;
		[Export ("renderVideoFrame:")]
		[Abstract]
		void RenderVideoFrame (OTVideoFrame frame);
	}

	// @protocol OTAudioDevice <NSObject>
	//[Protocol, Model]
	//[BaseType (typeof (NSObject))]
	//interface OTAudioDevice {
	//
	//}

	// @interface OTAudioDeviceManager : NSObject
	[BaseType (typeof(NSObject))]
	interface OTAudioDeviceManager
	{

		// +(void)setAudioDevice:(id<OTAudioDevice>)device;
		[Static, Export ("setAudioDevice:")]
		void SetAudioDevice (OTAudioDevice device);

		// +(id<OTAudioDevice>)currentAudioDevice;
		[Static, Export ("currentAudioDevice")]
		OTAudioDevice CurrentAudioDevice ();
	}

	// @interface OTAudioFormat : NSObject
	[BaseType (typeof(NSObject))]
	interface OTAudioFormat
	{

		// @property (assign, nonatomic) uint16_t sampleRate;
		[Export ("sampleRate", ArgumentSemantic.UnsafeUnretained)]
		ushort SampleRate { get; set; }

		// @property (assign, nonatomic) uint8_t numChannels;
		[Export ("numChannels", ArgumentSemantic.UnsafeUnretained)]
		byte NumChannels { get; set; }
	}

	// @protocol OTAudioBus <NSObject>
	[Protocol, Model]
	[BaseType (typeof(NSObject))]
	interface OTAudioBus
	{

		// @required -(void)writeCaptureData:(void *)data numberOfSamples:(uint32_t)count;
		[Export ("writeCaptureData:numberOfSamples:")]
		[Abstract]
		void WriteCaptureData (IntPtr data, uint count);

		// @required -(uint32_t)readRenderData:(void *)data numberOfSamples:(uint32_t)count;
		[Export ("readRenderData:numberOfSamples:")]
		[Abstract]
		uint ReadRenderData (IntPtr data, uint count);
	}

	// @protocol OTAudioDevice <NSObject>
	[Protocol, Model]
	[BaseType (typeof(NSObject))]
	interface OTAudioDevice
	{

		// @required -(BOOL)setAudioBus:(id<OTAudioBus>)audioBus;
		[Export ("setAudioBus:")]
		[Abstract]
		bool SetAudioBus (OTAudioBus audioBus);

		// @required -(OTAudioFormat *)captureFormat;
		[Export ("captureFormat")]
		[Abstract]
		OTAudioFormat CaptureFormat ();

		// @required -(OTAudioFormat *)renderFormat;
		[Export ("renderFormat")]
		[Abstract]
		OTAudioFormat RenderFormat ();

		// @required -(BOOL)renderingIsAvailable;
		[Export ("renderingIsAvailable")]
		[Abstract]
		bool RenderingIsAvailable ();

		// @required -(BOOL)initializeRendering;
		[Export ("initializeRendering")]
		[Abstract]
		bool InitializeRendering ();

		// @required -(BOOL)renderingIsInitialized;
		[Export ("renderingIsInitialized")]
		[Abstract]
		bool RenderingIsInitialized ();

		// @required -(BOOL)startRendering;
		[Export ("startRendering")]
		[Abstract]
		bool StartRendering ();

		// @required -(BOOL)stopRendering;
		[Export ("stopRendering")]
		[Abstract]
		bool StopRendering ();

		// @required -(BOOL)isRendering;
		[Export ("isRendering")]
		[Abstract]
		bool IsRendering ();

		// @required -(uint16_t)estimatedRenderDelay;
		[Export ("estimatedRenderDelay")]
		[Abstract]
		ushort EstimatedRenderDelay ();

		// @required -(BOOL)captureIsAvailable;
		[Export ("captureIsAvailable")]
		[Abstract]
		bool CaptureIsAvailable ();

		// @required -(BOOL)initializeCapture;
		[Export ("initializeCapture")]
		[Abstract]
		bool InitializeCapture ();

		// @required -(BOOL)captureIsInitialized;
		[Export ("captureIsInitialized")]
		[Abstract]
		bool CaptureIsInitialized ();

		// @required -(BOOL)startCapture;
		[Export ("startCapture")]
		[Abstract]
		bool StartCapture ();

		// @required -(BOOL)stopCapture;
		[Export ("stopCapture")]
		[Abstract]
		bool StopCapture ();

		// @required -(BOOL)isCapturing;
		[Export ("isCapturing")]
		[Abstract]
		bool IsCapturing ();

		// @required -(uint16_t)estimatedCaptureDelay;
		[Export ("estimatedCaptureDelay")]
		[Abstract]
		ushort EstimatedCaptureDelay ();
	}

	// @protocol OTPublisherDelegate <OTPublisherKitDelegate>
	//[Protocol, Model]
	//interface OTPublisherDelegate : OTPublisherKitDelegate {
	//
	//}

	// @protocol OTPublisherKitDelegate <NSObject>
	//[Protocol, Model]
	//[BaseType (typeof (NSObject))]
	//interface OTPublisherKitDelegate {
	//
	//}

	// @interface OTPublisher : OTPublisherKit
	[BaseType (typeof(OTPublisherKit))]
	interface OTPublisher
	{

		// @property (readonly) UIView * view;
		[Export ("view")]
		UIView View { get; }

		// @property (nonatomic) AVCaptureDevicePosition cameraPosition;
		[Export ("cameraPosition")]
		AVCaptureDevicePosition CameraPosition { get; set; }
	}

	// @protocol OTPublisherDelegate <OTPublisherKitDelegate>
	[Protocol, Model]
	interface OTPublisherDelegate : OTPublisherKitDelegate
	{

		// @optional -(void)publisher:(OTPublisher *)publisher didChangeCameraPosition:(AVCaptureDevicePosition)position;
		[Export ("publisher:didChangeCameraPosition:"), EventArgs ("OTPublisherDelegatePosition")]
		void DidChangeCameraPosition (OTPublisher publisher, AVCaptureDevicePosition position);
	}

	// @interface OTSubscriber : OTSubscriberKit
	[BaseType (typeof(OTSubscriberKit))]
	interface OTSubscriber
	{

		// @property (readonly) UIView * view;
		[Export ("view")]
		UIView View { get; }
	}

	// @protocol OTSubscriberDelegate <OTSubscriberKitDelegate>
	[Protocol, Model]
	interface OTSubscriberDelegate : OTSubscriberKitDelegate
	{

		// @required -(void)subscriberVideoDataReceived:(OTSubscriber *)subscriber;
		[Export ("subscriberVideoDataReceived:"), EventArgs ("OTSubscriberDelegateSubscriber")]
		void VideoDataReceived (OTSubscriber subscriber);
	}
}