# InvestmentDataSampleApp

This sample app demonstrates the following new features in Xamarin.Forms:
- Xamarin.Forms.SwipeView (docs link TBD)
- [Xamarin.Forms.RefreshView](https://docs.microsoft.com/xamarin/xamarin-forms/user-interface/refreshview?WT.mc_id=social-github-bramin)
- [Xamarin.Forms.CollectionView](https://docs.microsoft.com/xamarin/xamarin-forms/user-interface/collectionview?WT.mc_id=social-github-bramin)
- [Xamarin.Forms.PlatformConfiguration.iOSSpecific.UIModalPresentationStyle.FormSheet](https://docs.microsoft.com/xamarin/xamarin-forms/platform/ios/page-presentation-style?WT.mc_id=social-github-bramin)

| CollectionView + RefreshView (iOS) | UIModalPresentationStyle FormSheet | CollectionView + RefreshView (Android) | SwipeView (Android) |
| ---------------------------------- | ---------------------------------- | ---------------------------------- | ------------------ |
| ![CollectionView + RefreshView Demo, iOS](https://user-images.githubusercontent.com/13558917/70351802-95d3e980-181e-11ea-9f34-357a687b5fe2.gif) | ![iOS Modal Page Demo](https://user-images.githubusercontent.com/13558917/70351803-95d3e980-181e-11ea-8435-ec4d5a3ec7fb.gif) | ![CollectionView + RefreshView Demo, Android](https://user-images.githubusercontent.com/13558917/70351805-966c8000-181e-11ea-9ea0-8cadfe0fd9a8.gif) | ![SwipeView Android](https://user-images.githubusercontent.com/13558917/70351804-966c8000-181e-11ea-990f-623a01a2ebda.gif) |

## UI Design

This sample app was made to demonstrate how to port a Desktop App to a Mobile App. This project had an existing Desktop application. They provided us a mock up of their mobile app (see below) which did not follow [iOS UI best-practices](https://developer.apple.com/library/ios/documentation/UserExperience/Conceptual/MobileHIG/). We created a sample app to demonstrate how to adhere to mobile UI while still maintaining a workflow familiar to their desktop app.

This app utilizes a SQLite databse, MVVM, along with these Xamarin.Forms controls: Carousel Page, Search Bar, Picker, Grid, StackLayout, Navigation Page, ListView, ViewCell. It also shows how to tweak the UI to best appear on larger tablet screens. It also includes a Shake Listener, and will display a pop-up whenever the user shakes the device.

### Original Desktop App Mockup
The customer provided us this wireframe as the initial design for their mobile application. Their goal was to replicate their existing desktop app .
![Original App Mockup](https://user-images.githubusercontent.com/13558917/57715612-02418e00-762c-11e9-9c45-d32c88c3aa56.png)

### Sample App on iPad
This is the sample app we provided as it appears on a tablet
![iPad Sample App](https://user-images.githubusercontent.com/13558917/57715610-01a8f780-762c-11e9-8640-d1fc6872a024.gif)

### Sample App on iPhone
This is the sample app we provided as it appears on a phone
![iPhone Sample App](https://user-images.githubusercontent.com/13558917/57715611-02418e00-762c-11e9-93cc-194aaa140ffc.gif)
