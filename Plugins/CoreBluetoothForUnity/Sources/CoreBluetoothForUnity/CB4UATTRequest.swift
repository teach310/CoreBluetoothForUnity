import CoreBluetooth

public class CB4UATTRequest {
    var request: CBATTRequest!
    
    public init(request: CBATTRequest) {
        self.request = request
    }
    
    public var central: CB4UCentral {
        return CB4UCentral(central: request.central)
    }
    
    public var characteristic: CBCharacteristic {
        return request.characteristic
    }
    
    public var value: Data? {
        get {
            return request.value
        }
        set {
            request.value = newValue
        }
    }
    
    public var valueLength: Int {
        return request.value?.count ?? 0
    }
    
    public var offset: Int {
        return request.offset
    }
}
