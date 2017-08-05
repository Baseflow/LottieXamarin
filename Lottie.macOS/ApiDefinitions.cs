using System;
using AppKit;
using CoreGraphics;
using Foundation;
using ObjCRuntime;

namespace Airbnb.Lottie
{
	// @interface LOTComposition : NSObject
	[BaseType (typeof(NSObject))]
	interface LOTComposition
	{
		// -(instancetype)initWithJSON:(NSDictionary *)jsonDictionary withAssetBundle:(NSBundle *)bundle;
		[Export ("initWithJSON:withAssetBundle:")]
		IntPtr Constructor (NSDictionary jsonDictionary, NSBundle bundle);

		// @property (readonly, nonatomic) CGRect compBounds;
		[Export ("compBounds")]
		CGRect CompBounds { get; }

		// @property (readonly, nonatomic) NSNumber * startFrame;
		[Export ("startFrame")]
		NSNumber StartFrame { get; }

		// @property (readonly, nonatomic) NSNumber * endFrame;
		[Export ("endFrame")]
		NSNumber EndFrame { get; }

		// @property (readonly, nonatomic) NSNumber * framerate;
		[Export ("framerate")]
		NSNumber Framerate { get; }

		// @property (readonly, nonatomic) NSTimeInterval timeDuration;
		[Export ("timeDuration")]
		double TimeDuration { get; }

		//// @property (readonly, nonatomic) LOTLayerGroup * layerGroup;
		//[Export ("layerGroup")]
		//LOTLayerGroup LayerGroup { get; }

		//// @property (readonly, nonatomic) LOTAssetGroup * assetGroup;
		//[Export ("assetGroup")]
		//LOTAssetGroup AssetGroup { get; }

		// @property (readwrite, nonatomic) NSString * rootDirectory;
		[Export ("rootDirectory")]
		string RootDirectory { get; set; }

		// @property (readonly, nonatomic) NSBundle * assetBundle;
		[Export ("assetBundle")]
		NSBundle AssetBundle { get; }
	}

	// typedef void (^LOTAnimationCompletionBlock)(BOOL);
	delegate void LOTAnimationCompletionBlock (bool animationFinished);

	// @interface LOTAnimationView : NSView
	[BaseType (typeof(NSView))]
	interface LOTAnimationView
	{
		// +(instancetype _Nonnull)animationNamed:(NSString * _Nonnull)animationName;
		[Static]
		[Export ("animationNamed:")]
		LOTAnimationView AnimationNamed (string animationName);

		// +(instancetype _Nonnull)animationNamed:(NSString * _Nonnull)animationName inBundle:(NSBundle * _Nonnull)bundle;
		[Static]
		[Export ("animationNamed:inBundle:")]
		LOTAnimationView AnimationNamed (string animationName, NSBundle bundle);

		// +(instancetype _Nonnull)animationFromJSON:(NSDictionary * _Nonnull)animationJSON;
		[Static]
		[Export ("animationFromJSON:")]
		LOTAnimationView AnimationFromJSON (NSDictionary animationJSON);

		// +(instancetype _Nonnull)animationWithFilePath:(NSString * _Nonnull)filePath;
		[Static]
		[Export ("animationWithFilePath:")]
		LOTAnimationView AnimationWithFilePath (string filePath);

		// +(instancetype _Nonnull)animationFromJSON:(NSDictionary * _Nullable)animationJSON inBundle:(NSBundle * _Nullable)bundle;
		[Static]
		[Export ("animationFromJSON:inBundle:")]
		LOTAnimationView AnimationFromJSON ([NullAllowed] NSDictionary animationJSON, [NullAllowed] NSBundle bundle);

		// -(instancetype _Nonnull)initWithModel:(LOTComposition * _Nullable)model inBundle:(NSBundle * _Nullable)bundle;
		[Export ("initWithModel:inBundle:")]
		IntPtr Constructor ([NullAllowed] LOTComposition model, [NullAllowed] NSBundle bundle);

		// -(instancetype _Nonnull)initWithContentsOfURL:(NSURL * _Nonnull)url;
		[Export ("initWithContentsOfURL:")]
		IntPtr Constructor (NSUrl url);

		// @property (readonly, nonatomic) BOOL isAnimationPlaying;
		[Export ("isAnimationPlaying")]
		bool IsAnimationPlaying { get; }

		// @property (assign, nonatomic) BOOL loopAnimation;
		[Export ("loopAnimation")]
		bool LoopAnimation { get; set; }

		// @property (assign, nonatomic) BOOL autoReverseAnimation;
		[Export ("autoReverseAnimation")]
		bool AutoReverseAnimation { get; set; }

		// @property (assign, nonatomic) CGFloat animationProgress;
		[Export ("animationProgress")]
		nfloat AnimationProgress { get; set; }

		// @property (assign, nonatomic) CGFloat animationSpeed;
		[Export ("animationSpeed")]
		nfloat AnimationSpeed { get; set; }

		// @property (readonly, nonatomic) CGFloat animationDuration;
		[Export ("animationDuration")]
		nfloat AnimationDuration { get; }

		// @property (assign, nonatomic) BOOL cacheEnable;
		[Export ("cacheEnable")]
		bool CacheEnable { get; set; }

		// @property (copy, nonatomic) LOTAnimationCompletionBlock _Nullable completionBlock;
		[NullAllowed, Export ("completionBlock", ArgumentSemantic.Copy)]
		LOTAnimationCompletionBlock CompletionBlock { get; set; }

		// @property (nonatomic, strong) LOTComposition * _Nonnull sceneModel;
		[Export ("sceneModel", ArgumentSemantic.Strong)]
		LOTComposition SceneModel { get; set; }

		// -(void)playToProgress:(CGFloat)toProgress withCompletion:(LOTAnimationCompletionBlock _Nullable)completion;
		[Export ("playToProgress:withCompletion:")]
		void PlayToProgress (nfloat toProgress, [NullAllowed] LOTAnimationCompletionBlock completion);

		// -(void)playFromProgress:(CGFloat)fromStartProgress toProgress:(CGFloat)toEndProgress withCompletion:(LOTAnimationCompletionBlock _Nullable)completion;
		[Export ("playFromProgress:toProgress:withCompletion:")]
		void PlayFromProgress (nfloat fromStartProgress, nfloat toEndProgress, [NullAllowed] LOTAnimationCompletionBlock completion);

		// -(void)playToFrame:(NSNumber * _Nonnull)toFrame withCompletion:(LOTAnimationCompletionBlock _Nullable)completion;
		[Export ("playToFrame:withCompletion:")]
		void PlayToFrame (NSNumber toFrame, [NullAllowed] LOTAnimationCompletionBlock completion);

		// -(void)playFromFrame:(NSNumber * _Nonnull)fromStartFrame toFrame:(NSNumber * _Nonnull)toEndFrame withCompletion:(LOTAnimationCompletionBlock _Nullable)completion;
		[Export ("playFromFrame:toFrame:withCompletion:")]
		void PlayFromFrame (NSNumber fromStartFrame, NSNumber toEndFrame, [NullAllowed] LOTAnimationCompletionBlock completion);

		// -(void)playWithCompletion:(LOTAnimationCompletionBlock _Nullable)completion;
		[Export ("playWithCompletion:")]
		void PlayWithCompletion ([NullAllowed] LOTAnimationCompletionBlock completion);

		// -(void)play;
		[Export ("play")]
		void Play ();

		// -(void)pause;
		[Export ("pause")]
		void Pause ();

		// -(void)stop;
		[Export ("stop")]
		void Stop ();

		// -(void)setProgressWithFrame:(NSNumber * _Nonnull)currentFrame;
		[Export ("setProgressWithFrame:")]
		void SetProgressWithFrame (NSNumber currentFrame);

		// -(void)setValue:(id _Nonnull)value forKeypath:(NSString * _Nonnull)keypath atFrame:(NSNumber * _Nullable)frame;
		[Export ("setValue:forKeypath:atFrame:")]
		void SetValue (NSObject value, string keypath, [NullAllowed] NSNumber frame);

		// -(void)logHierarchyKeypaths;
		[Export ("logHierarchyKeypaths")]
		void LogHierarchyKeypaths ();

		// -(void)addSubview:(NSView * _Nonnull)view toLayerNamed:(NSString * _Nonnull)layer applyTransform:(BOOL)applyTransform;
		[Export ("addSubview:toLayerNamed:applyTransform:")]
		void AddSubview (NSView view, string layer, bool applyTransform);

		// @property (nonatomic) LOTViewContentMode contentMode;
		[Export ("contentMode", ArgumentSemantic.Assign)]
		LOTViewContentMode ContentMode { get; set; }
	}

	// @interface LOTAnimationCache : NSObject
	[BaseType (typeof(NSObject))]
	interface LOTAnimationCache
	{
		// +(instancetype _Nonnull)sharedCache;
		[Static]
		[Export ("sharedCache")]
		LOTAnimationCache SharedCache ();

		// -(void)addAnimation:(LOTComposition * _Nonnull)animation forKey:(NSString * _Nonnull)key;
		[Export ("addAnimation:forKey:")]
		void AddAnimation (LOTComposition animation, string key);

		// -(LOTComposition * _Nullable)animationForKey:(NSString * _Nonnull)key;
		[Export ("animationForKey:")]
		[return: NullAllowed]
		LOTComposition AnimationForKey (string key);

		// -(void)removeAnimationForKey:(NSString * _Nonnull)key;
		[Export ("removeAnimationForKey:")]
		void RemoveAnimationForKey (string key);

		// -(void)clearCache;
		[Export ("clearCache")]
		void ClearCache ();
	}
}
