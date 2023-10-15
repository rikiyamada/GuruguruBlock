
//  RequestAttDialog.mm　ファイル名.mmにしたらここも.mmにしてあげて！
//  Unity-iPhone
#import <Foundation/Foundation.h>
//以下、2行追加
#import <AppTrackingTransparency/AppTrackingTransparency.h>
#import <AdSupport/AdSupport.h>
//この範囲のコードはCで書かれてるよってこと
#ifdef __cplusplus
extern "C" {
#endif
    //ATTダイアログを表示するメソッド
    void requestIDFA()
    {
        if (@available(iOS 14, *))
        {
            [ATTrackingManager requestTrackingAuthorizationWithCompletionHandler:^(ATTrackingManagerAuthorizationStatus status) {
        // Tracking authorization completed. Start loading ads here.
        // [self loadAd];
    }];
}
}
#ifdef __cplusplus
}
#endif