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
    
    public func setValue(_ value: Data?) {
        request.value = value
    }
    
    public var offset: Int {
        return request.offset
    }
}
