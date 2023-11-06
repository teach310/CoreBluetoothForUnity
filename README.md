# CoreBluetoothForUnity

Provides native Apple CoreBluetooth integration for use with Unity.  
CoreBluetoothForUnity provides an interface that closely mirrors the native CoreBluetooth API.  

If you are familiar with CoreBluetooth, you will find it easy to work with this plugin.

## Installation

Scoped Registry

- Name: `Teach310`
- URL: `https://registry.npmjs.com`
- Scope: `com.teach310`

## Sample

You can download samples from Package Manager.

#### Light Control

|Central|Peripheral|
|:--:|:--:|
|<img width="448" alt="lightcontrol_central" src="https://github.com/teach310/CoreBluetoothForUnity/assets/16421323/388281ef-eadd-4433-be35-5ba446f9a34c">|![output](https://github.com/teach310/CoreBluetoothForUnity/assets/16421323/ebbc6bc9-59a3-465e-a2f8-0d6aafab0f20)|


#### Button Information

|Central|Peripheral|
|:--:|:--:|
|![mov_button_information](https://github.com/teach310/CoreBluetoothForUnity/assets/16421323/8b119fda-73c0-44d8-aa53-7690526f88cf)|<img width="660" alt="button_information_peripheral" src="https://github.com/teach310/CoreBluetoothForUnity/assets/16421323/00fb797a-28e2-4337-9846-fae393941f65">|

## How to release

If you create a new instance like CBCentralManager, CBMutableService with using "new" keyword,
you must call Dispose() method when you don't need it anymore.

```csharp
CBCentralManager _centralManager;

...

void OnDestroy()
{
    if (_centralManager != null)
    {
        _centralManager.Dispose();
        _centralManager = null;
    }
}
```

## Feature Requests

This plugin isn't support all of CoreBluetooth features yet.

If you want to add some features, please create an issue or pull request.

## License

MIT License (see `LICENSE` file).
