using System;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using UIKit;

namespace Airbnb.Lottie
{
	// @interface LOTAnimationTransitionController : NSObject <UIViewControllerAnimatedTransitioning>
	[BaseType (typeof(NSObject))]
	interface LOTAnimationTransitionController : IUIViewControllerAnimatedTransitioning
	{
		// -(instancetype _Nonnull)initWithAnimationNamed:(NSString * _Nonnull)animation fromLayerNamed:(NSString * _Nullable)fromLayer toLayerNamed:(NSString * _Nullable)toLayer applyAnimationTransform:(BOOL)applyAnimationTransform;
		[Export ("initWithAnimationNamed:fromLayerNamed:toLayerNamed:applyAnimationTransform:")]
		IntPtr Constructor (string animation, [NullAllowed] string fromLayer, [NullAllowed] string toLayer, bool applyAnimationTransform);

		// -(instancetype _Nonnull)initWithAnimationNamed:(NSString * _Nonnull)animation fromLayerNamed:(NSString * _Nullable)fromLayer toLayerNamed:(NSString * _Nullable)toLayer applyAnimationTransform:(BOOL)applyAnimationTransform inBundle:(NSBundle * _Nonnull)bundle;
		[Export ("initWithAnimationNamed:fromLayerNamed:toLayerNamed:applyAnimationTransform:inBundle:")]
		IntPtr Constructor (string animation, [NullAllowed] string fromLayer, [NullAllowed] string toLayer, bool applyAnimationTransform, NSBundle bundle);
	}

	// @interface LOTAnimatedControl : UIControl
	[BaseType (typeof(UIControl))]
	interface LOTAnimatedControl
	{
		// -(void)setLayerName:(NSString * _Nonnull)layerName forState:(UIControlState)state;
		[Export ("setLayerName:forState:")]
		void SetLayerName (string layerName, UIControlState state);

		// @property (readonly, nonatomic) LOTAnimationView * _Nonnull animationView;
		[Export ("animationView")]
		LOTAnimationView AnimationView { get; }

		// @property (nonatomic) LOTComposition * _Nullable animationComp;
		[NullAllowed, Export ("animationComp", ArgumentSemantic.Assign)]
		LOTComposition AnimationComp { get; set; }
	}

	// @interface LOTAnimatedSwitch : LOTAnimatedControl
	[BaseType (typeof(LOTAnimatedControl))]
	interface LOTAnimatedSwitch
	{
		// +(instancetype _Nonnull)switchNamed:(NSString * _Nonnull)toggleName;
		[Static]
		[Export ("switchNamed:")]
		LOTAnimatedSwitch SwitchNamed (string toggleName);

		// +(instancetype _Nonnull)switchNamed:(NSString * _Nonnull)toggleName inBundle:(NSBundle * _Nonnull)bundle;
		[Static]
		[Export ("switchNamed:inBundle:")]
		LOTAnimatedSwitch SwitchNamed (string toggleName, NSBundle bundle);

		// @property (getter = isOn, nonatomic) BOOL on;
		[Export ("on")]
		bool On { [Bind ("isOn")] get; set; }

		// -(void)setOn:(BOOL)on animated:(BOOL)animated;
		[Export ("setOn:animated:")]
		void SetOn (bool on, bool animated);

		// -(void)setProgressRangeForOnState:(CGFloat)fromProgress toProgress:(CGFloat)toProgress;
		[Export ("setProgressRangeForOnState:toProgress:")]
		void SetProgressRangeForOnState (nfloat fromProgress, nfloat toProgress);

		// -(void)setProgressRangeForOffState:(CGFloat)fromProgress toProgress:(CGFloat)toProgress;
		[Export ("setProgressRangeForOffState:toProgress:")]
		void SetProgressRangeForOffState (nfloat fromProgress, nfloat toProgress);
	}

	// @interface LOTCacheProvider : NSObject
	[BaseType (typeof(NSObject))]
	interface LOTCacheProvider
	{
		// +(id<LOTImageCache>)imageCache;
		// +(void)setImageCache:(id<LOTImageCache>)cache;
		[Static]
		[Export ("imageCache")]
		LOTImageCache ImageCache { get; set; }
	}

	// @protocol LOTImageCache <NSObject>
	[Protocol, Model]
	[BaseType (typeof(NSObject))]
	interface LOTImageCache
	{
		// @required -(UIImage *)imageForKey:(NSString *)key;
		[Abstract]
		[Export ("imageForKey:")]
		UIImage ImageForKey (string key);

		// @required -(void)setImage:(UIImage *)image forKey:(NSString *)key;
		[Abstract]
		[Export ("setImage:forKey:")]
		void SetImage (UIImage image, string key);
	}

	// @interface LOTComposition : NSObject
	[BaseType (typeof(NSObject))]
	interface LOTComposition
	{
		// +(instancetype _Nullable)animationNamed:(NSString * _Nonnull)animationName;
		[Static]
		[Export ("animationNamed:")]
		[return: NullAllowed]
		LOTComposition AnimationNamed (string animationName);

		// +(instancetype _Nullable)animationNamed:(NSString * _Nonnull)animationName inBundle:(NSBundle * _Nonnull)bundle;
		[Static]
		[Export ("animationNamed:inBundle:")]
		[return: NullAllowed]
		LOTComposition AnimationNamed (string animationName, NSBundle bundle);

		// +(instancetype _Nullable)animationWithFilePath:(NSString * _Nonnull)filePath;
		[Static]
		[Export ("animationWithFilePath:")]
		[return: NullAllowed]
		LOTComposition AnimationWithFilePath (string filePath);

		// +(instancetype _Nonnull)animationFromJSON:(NSDictionary * _Nonnull)animationJSON;
		[Static]
		[Export ("animationFromJSON:")]
		LOTComposition AnimationFromJSON (NSDictionary animationJSON);

		// +(instancetype _Nonnull)animationFromJSON:(NSDictionary * _Nullable)animationJSON inBundle:(NSBundle * _Nullable)bundle;
		[Static]
		[Export ("animationFromJSON:inBundle:")]
		LOTComposition AnimationFromJSON ([NullAllowed] NSDictionary animationJSON, [NullAllowed] NSBundle bundle);

		// -(instancetype _Nonnull)initWithJSON:(NSDictionary * _Nullable)jsonDictionary withAssetBundle:(NSBundle * _Nullable)bundle;
		[Export ("initWithJSON:withAssetBundle:")]
		IntPtr Constructor ([NullAllowed] NSDictionary jsonDictionary, [NullAllowed] NSBundle bundle);

		// @property (readonly, nonatomic) CGRect compBounds;
		[Export ("compBounds")]
		CGRect CompBounds { get; }

		// @property (readonly, nonatomic) NSNumber * _Nullable startFrame;
		[NullAllowed, Export ("startFrame")]
		NSNumber StartFrame { get; }

		// @property (readonly, nonatomic) NSNumber * _Nullable endFrame;
		[NullAllowed, Export ("endFrame")]
		NSNumber EndFrame { get; }

		// @property (readonly, nonatomic) NSNumber * _Nullable framerate;
		[NullAllowed, Export ("framerate")]
		NSNumber Framerate { get; }

		// @property (readonly, nonatomic) NSTimeInterval timeDuration;
		[Export ("timeDuration")]
		double TimeDuration { get; }

		// @property (readonly, nonatomic) LOTLayerGroup * _Nullable layerGroup;
		//[NullAllowed, Export ("layerGroup")]
		//LOTLayerGroup LayerGroup { get; }

		// @property (readonly, nonatomic) LOTAssetGroup * _Nullable assetGroup;
		//[NullAllowed, Export ("assetGroup")]
		//LOTAssetGroup AssetGroup { get; }

		// @property (readwrite, nonatomic) NSString * _Nullable rootDirectory;
		[NullAllowed, Export ("rootDirectory")]
		string RootDirectory { get; set; }

		// @property (readonly, nonatomic) NSBundle * _Nullable assetBundle;
		[NullAllowed, Export ("assetBundle")]
		NSBundle AssetBundle { get; }

		// @property (copy, nonatomic) NSString * _Nullable cacheKey;
		[NullAllowed, Export ("cacheKey")]
		string CacheKey { get; set; }
	}

	// typedef void (^LOTAnimationCompletionBlock)(BOOL);
	delegate void LOTAnimationCompletionBlock (bool animationFinished);

	// @interface LOTAnimationView : UIView
	[BaseType (typeof(UIView))]
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

		// -(void)addSubview:(UIView * _Nonnull)view toLayerNamed:(NSString * _Nonnull)layer applyTransform:(BOOL)applyTransform;
		[Export ("addSubview:toLayerNamed:applyTransform:")]
		void AddSubview (UIView view, string layer, bool applyTransform);

		// -(CGRect)convertRect:(CGRect)rect toLayerNamed:(NSString * _Nullable)layerName;
		[Export ("convertRect:toLayerNamed:")]
		CGRect ConvertRect (CGRect rect, [NullAllowed] string layerName);
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

		// -(void)disableCaching;
		[Export ("disableCaching")]
		void DisableCaching ();
	}
}
