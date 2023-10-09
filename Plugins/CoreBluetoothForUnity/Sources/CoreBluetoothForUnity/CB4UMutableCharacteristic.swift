import CoreBluetooth

public class CB4UMutableCharacteristic {
    var characteristic: CBMutableCharacteristic!
    
    public var value: Data? {
        get {
            return characteristic.value
        }
        set {
            characteristic.value = newValue
        }
    }

    public var valueLength: Int {
        return characteristic.value?.count ?? 0
    }
    
    public var properties: CBCharacteristicProperties {
        get {
            return characteristic.properties
        }
        set {
            characteristic.properties = newValue
        }
    }
    
    public var permissions: CBAttributePermissions {
        get {
            return characteristic.permissions
        }
        set {
            characteristic.permissions = newValue
        }
    }
    
    public init(characteristic: CBMutableCharacteristic) {
        self.characteristic = characteristic
    }
}
