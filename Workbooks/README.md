# Xamarin Workbooks

Xamrin Workbooks is an awesome tool to experiment your code as you learn. It eliminates the need of having an IDE to write, compile and run your code. Workbooks can be created for Android, iOS, Mac, WPF, or Console to get instant live results as you learn these APIs. Workbooks are interactive so you can quickly modify the code in the document and observe the different results without recompiling and running the code.


![](https://developer.xamarin.com/guides/cross-platform/workbooks/Images/interactive-1.0.0-urho_planet_earth.png)

## The Challenge
In this challenge, you will explore Xamarin Workbooks and learn UrhoSharp APIs by live experimenting. UrhoSharp is a cross-platform high-level 3D and 2D engine that can be used to create animated 3D and 2D scenes for your applications using geometries, materials, lights, and cameras. The goal is to get your hands dirty with Workbooks by learning something new - UrhoSharp.

This challenge requires:

* Download and Install [Xamarin Workbooks](https://developer.xamarin.com/guides/cross-platform/workbooks/install/)
* Clone this repo and Navigate to `planetearth` folder


### Challenge  Walkthrough

#### Step 1: Run Xamarin Workbooks and open the file within `planetearth` folder. Double click on the file `planetearth.workbook` or from Xamarin Workbooks, `File > Open > planetearth.workbook`

#### Step 2: Go Step by Step. Start reading the document and execute the code snippets one by one. When you execute the following line of code, you should see an empty window pop-up.
```csharp
var app = await SimpleApplication.RunAsync (
	new ApplicationOptions ("Data") {
		Width = 600,
		Height = 600,
		TouchEmulation = true });
```
Isn't it cool? Keep going and complete everything. 

#### Step 3: Complete the steps and notice the Earth, Moon and the Satellites revolving.


#### Step 4: Now, here's your challenge.  Change the moon shape to `Torus`. See the image below.
![](https://github.com/nishanil/Mini-Hacks/blob/master/Workbooks/output.gif?raw=true)

Your hint: The Moon is Sphere.

## Bonus Challenge

Pick any of the samples from this [list](https://developer.xamarin.com/workbooks/). Make a change and run it on an iOS Simulator or an Android Emulator.

Have questions? Find me on twitter [@nishanil](http://twitter.com/nishanil)
