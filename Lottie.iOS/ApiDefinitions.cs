using System;
using Foundation;
using UIKit;

namespace Airbnb.Lottie
{
	// @interface LAAnimationTransitionController : NSObject <UIViewControllerAnimatedTransitioning>
	[BaseType (typeof(NSObject))]
	interface LAAnimationTransitionController : IUIViewControllerAnimatedTransitioning
	{
		// -(instancetype)initWithAnimationNamed:(NSString *)animation fromLayerNamed:(NSString *)fromLayer toLayerNamed:(NSString *)toLayer;
		[Export ("initWithAnimationNamed:fromLayerNamed:toLayerNamed:")]
		IntPtr Constructor (string animation, string fromLayer, string toLayer);
	}

	// typedef void (^LAAnimationCompletionBlock)(BOOL);
	delegate void LAAnimationCompletionBlock (bool arg0);

	// @interface LAAnimationView : UIView
	[BaseType (typeof(UIView))]
	interface LAAnimationView
	{
		// +(instancetype)animationNamed:(NSString *)animationName;
		[Static]
		[Export ("animationNamed:")]
		LAAnimationView AnimationNamed (string animationName);

		// +(instancetype)animationFromJSON:(NSDictionary *)animationJSON;
		[Static]
		[Export ("animationFromJSON:")]
		LAAnimationView AnimationFromJSON (NSDictionary animationJSON);

		// -(instancetype)initWithContentsOfURL:(NSURL *)url;
		[Export ("initWithContentsOfURL:")]
		IntPtr Constructor (NSUrl url);

		// @property (readonly, nonatomic) BOOL isAnimationPlaying;
		[Export ("isAnimationPlaying")]
		bool IsAnimationPlaying { get; }

		// @property (assign, nonatomic) BOOL loopAnimation;
		[Export ("loopAnimation")]
		bool LoopAnimation { get; set; }

		// @property (assign, nonatomic) CGFloat animationProgress;
		[Export ("animationProgress")]
		nfloat AnimationProgress { get; set; }

		// @property (assign, nonatomic) CGFloat animationSpeed;
		[Export ("animationSpeed")]
		nfloat AnimationSpeed { get; set; }

		// @property (readonly, nonatomic) CGFloat animationDuration;
		[Export ("animationDuration")]
		nfloat AnimationDuration { get; }

		// -(void)playWithCompletion:(LAAnimationCompletionBlock)completion;
		[Export ("playWithCompletion:")]
		void PlayWithCompletion (LAAnimationCompletionBlock completion);

		// -(void)play;
		[Export ("play")]
		void Play ();

		// -(void)pause;
		[Export ("pause")]
		void Pause ();

		// -(void)addSubview:(UIView *)view toLayerNamed:(NSString *)layer;
		[Export ("addSubview:toLayerNamed:")]
		void AddSubview (UIView view, string layer);
	}
}
