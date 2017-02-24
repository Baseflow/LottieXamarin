using System;
using Foundation;
using UIKit;

namespace Airbnb.Lottie
{
    
    // @interface LOTAnimationTransitionController : NSObject <UIViewControllerAnimatedTransitioning>
    [BaseType (typeof(NSObject))]
    interface LOTAnimationTransitionController : IUIViewControllerAnimatedTransitioning
    {
        // -(instancetype)initWithAnimationNamed:(NSString *)animation fromLayerNamed:(NSString *)fromLayer toLayerNamed:(NSString *)toLayer;
        [Export("initWithAnimationNamed:fromLayerNamed:toLayerNamed:")]
        IntPtr Constructor(string animation, string fromLayer, string toLayer);
    }

    // typedef void (^LOTAnimationCompletionBlock)(BOOL);
    delegate void LOTAnimationCompletionBlock(bool animationFinished);

	/// @interface LOTAnimationView : UIView
    [BaseType (typeof(UIView))]
    interface LOTAnimationView
    {
        // +(instancetype)animationNamed:(NSString *)animationName;
        [Static]
        [Export("animationNamed:")]
        LOTAnimationView AnimationNamed(string animationName);

        // +(instancetype)animationFromJSON:(NSDictionary *)animationJSON;
        [Static]
        [Export("animationFromJSON:")]
        LOTAnimationView AnimationFromJSON(NSDictionary animationJSON);

        // -(instancetype)initWithContentsOfURL:(NSURL *)url;
        [Export("initWithContentsOfURL:")]
        IntPtr Constructor(NSUrl url);

        // @property (readonly, nonatomic) BOOL isAnimationPlaying;
        [Export("isAnimationPlaying")]
        bool IsAnimationPlaying { get; }

        // @property (assign, nonatomic) BOOL loopAnimation;
        [Export("loopAnimation")]
        bool LoopAnimation { get; set; }

        // @property (assign, nonatomic) CGFloat animationProgress;
        [Export("animationProgress")]
        nfloat AnimationProgress { get; set; }

        // @property (assign, nonatomic) CGFloat animationSpeed;
        [Export("animationSpeed")]
        nfloat AnimationSpeed { get; set; }

        // @property (readonly, nonatomic) CGFloat animationDuration;
        [Export("animationDuration")]
        nfloat AnimationDuration { get; }

        // -(void)playWithCompletion:(LOTAnimationCompletionBlock)completion;
        [Export("playWithCompletion:")]
        void PlayWithCompletion(LOTAnimationCompletionBlock completion);

        // -(void)play;
        [Export("play")]
        void Play();

        // -(void)pause;
        [Export("pause")]
        void Pause();

        // -(void)addSubview:(UIView *)view toLayerNamed:(NSString *)layer;
        [Export("addSubview:toLayerNamed:")]
        void AddSubview(UIView view, string layer);
    }
}
