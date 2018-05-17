1. Control Panel -> Programs and features -> Visual Studio 2017 -> Right click and click change -> Click Individual components -> Ensure '.NET Framework 4.7.1 SDK' and '.NET Framework 4.7.1 targeting pack' are selected
2. Add Telerik Nuget Source  https://nuget.telerik.com/nuget
3. Tools -> Android -> Android SDK Manager -> Android 8.1 - Oreo -> Select 'Android SDK Platform 27'
4. Sort following error: 
	Severity	Code	Description	Project	File	Line	Suppression State
	Error		An error occurred trying to install required android components on Project 'NssApp.Android'.
	Project 'NssApp.Android' requires the following components installed on your machine: 
	Xamarin.Android.Support.v8.RenderScript
	JavaLibraryReference: http://dl-ssl.google.com/android/repository/build-tools_r24-macosx.zip--24
	NativeLibraryReference: http://dl-ssl.google.com/android/repository/build-tools_r24-macosx.zip--24
	NativeLibraryReference: http://dl-ssl.google.com/android/repository/build-tools_r24-macosx.zip--24
	NativeLibraryReference: http://dl-ssl.google.com/android/repository/build-tools_r24-macosx.zip--24
	NativeLibraryReference: http://dl-ssl.google.com/android/repository/build-tools_r24-macosx.zip--24


	Please double-click here to install it.

	Intallation Errors: XA5209 Unzipping failed. Please download 'http://dl-ssl.google.com/android/repository/build-tools_r24-macosx.zip' and extract it to the 'C:\Users\craig\AppData\Local\Xamarin\Xamarin.Android.Support.v8.RenderScript\24\content' directory
	XA5209 Reason: Could not find a part of the path 'C:\Users\craig\AppData\Local\Xamarin\Xamarin.Android.Support.v8.RenderScript\24\content\android-N\'.
	XA5207 Please install package: 'Xamarin.Android.Support.v8' available in SDK installer. Java library file 'C:\Users\craig\AppData\Local\Xamarin\Xamarin.Android.Support.v8.RenderScript\24\content\android-N/renderscript/lib/renderscript-v8.jar' doesn't exist.
	XA5210 Please install package: 'Xamarin.Android.Support.v8' available in SDK installer. Native library file 'C:\Users\craig\AppData\Local\Xamarin\Xamarin.Android.Support.v8.RenderScript\24\content\android-N/renderscript/lib/packaged/armeabi-v7a/librsjni.so' doesn't exist.
	XA5210 Please install package: 'Xamarin.Android.Support.v8' available in SDK installer. Native library file 'C:\Users\craig\AppData\Local\Xamarin\Xamarin.Android.Support.v8.RenderScript\24\content\android-N/renderscript/lib/packaged/armeabi-v7a/libRSSupport.so' doesn't exist.
	XA5210 Please install package: 'Xamarin.Android.Support.v8' available in SDK installer. Native library file 'C:\Users\craig\AppData\Local\Xamarin\Xamarin.Android.Support.v8.RenderScript\24\content\android-N/renderscript/lib/packaged/x86/librsjni.so' doesn't exist.
	XA5210 Please install package: 'Xamarin.Android.Support.v8' available in SDK installer. Native library file 'C:\Users\craig\AppData\Local\Xamarin\Xamarin.Android.Support.v8.RenderScript\24\content\android-N/renderscript/lib/packaged/x86/libRSSupport.so' doesn't exist.		 	0	
